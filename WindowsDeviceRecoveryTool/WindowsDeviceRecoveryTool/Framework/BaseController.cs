using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Windows.Input;

namespace Microsoft.WindowsDeviceRecoveryTool.Framework
{
	// Token: 0x02000026 RID: 38
	public class BaseController : IController
	{
		// Token: 0x0600010C RID: 268 RVA: 0x00008593 File Offset: 0x00006793
		public BaseController(ICommandRepository commandRepository, EventAggregator eventAggregator)
		{
			this.commands = commandRepository;
			this.eventAggregator = eventAggregator;
			this.InitializeCommands();
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600010D RID: 269 RVA: 0x000085B4 File Offset: 0x000067B4
		protected EventAggregator EventAggregator
		{
			get
			{
				return this.eventAggregator;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600010E RID: 270 RVA: 0x000085CC File Offset: 0x000067CC
		protected ICommandRepository Commands
		{
			get
			{
				return this.commands;
			}
		}

		// Token: 0x0600010F RID: 271 RVA: 0x000085E4 File Offset: 0x000067E4
		private static bool MethodHasAttribute<T>(MethodInfo method) where T : Attribute
		{
			return method.GetCustomAttributes(typeof(T), false).Any<object>();
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000860C File Offset: 0x0000680C
		private static bool MethodTypeIsVoid(MethodInfo method)
		{
			return method.ReturnType == typeof(void);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00008634 File Offset: 0x00006834
		private static bool MethodHasParameters(MethodInfo method, int number = 1)
		{
			return method.GetParameters().Count<ParameterInfo>() == number;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00008654 File Offset: 0x00006854
		private bool MethodIsAsynchronous(MethodInfo method)
		{
			bool result;
			if (BaseController.MethodHasAttribute<CustomCommandAttribute>(method))
			{
				CustomCommandAttribute customCommandAttribute = method.GetCustomAttributes<CustomCommandAttribute>().FirstOrDefault<CustomCommandAttribute>();
				result = customCommandAttribute.IsAsynchronous;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x0000868C File Offset: 0x0000688C
		private bool IsBadDesignedAsyncMethod(MethodInfo method)
		{
			if (this.MethodIsAsynchronous(method))
			{
				ParameterInfo[] parameters = method.GetParameters();
				if (parameters.Count<ParameterInfo>() != 2 || !(parameters[1].ParameterType == typeof(CancellationToken)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00008708 File Offset: 0x00006908
		private void InitializeCommands()
		{
			List<MethodInfo> list = base.GetType().GetMethods().ToList<MethodInfo>();
			List<MethodInfo> list2 = (from method in list
			where method.Name.StartsWith("CanExecute", StringComparison.Ordinal)
			select method).ToList<MethodInfo>();
			List<MethodInfo> list3 = list.Except(list2).ToList<MethodInfo>();
			foreach (MethodInfo methodInfo in list3)
			{
				if (this.IsBadDesignedAsyncMethod(methodInfo))
				{
					throw new MissingMethodException(string.Format("{0}.{1} method has wrong signature. It need to have two parameters and second should be CancellationToken.", base.GetType().Name, methodInfo.Name));
				}
				if (BaseController.MethodHasAttribute<CustomCommandAttribute>(methodInfo) && BaseController.MethodTypeIsVoid(methodInfo) && methodInfo.GetParameters().Count<ParameterInfo>() < 2)
				{
					this.ProcessMethod(methodInfo, list2);
				}
				else if (this.MethodIsAsynchronous(methodInfo) && BaseController.MethodHasParameters(methodInfo, 2) && methodInfo.GetParameters()[1].ParameterType == typeof(CancellationToken))
				{
					this.ProcessAsyncMethod(methodInfo, list2);
				}
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x0000887C File Offset: 0x00006A7C
		private void ProcessMethod(MethodInfo method, List<MethodInfo> canMethods)
		{
			if (BaseController.MethodHasParameters(method, 1))
			{
				ParameterInfo parameterInfo = method.GetParameters().FirstOrDefault<ParameterInfo>();
				Type type = typeof(Action<>).MakeGenericType(new Type[]
				{
					parameterInfo.ParameterType
				});
				Type type2 = typeof(DelegateCommand<>).MakeGenericType(new Type[]
				{
					parameterInfo.ParameterType
				});
				Delegate @delegate = Delegate.CreateDelegate(type, this, method);
				Func<object, bool> canMethodInstance = this.GetCanMethodInstance(method, canMethods);
				KeyGesture keyGesture = this.GetKeyGesture(method);
				IDelegateCommand value = (IDelegateCommand)Activator.CreateInstance(type2, new object[]
				{
					@delegate,
					canMethodInstance,
					keyGesture
				});
				this.Commands.Add(method.Name, value);
			}
			else if (!method.GetParameters().Any<ParameterInfo>())
			{
				Func<object, bool> canMethodInstance = this.GetCanMethodInstance(method, canMethods);
				KeyGesture keyGesture = this.GetKeyGesture(method);
				this.Commands.Add(method.Name, new DelegateCommand<object>(delegate(object parameter)
				{
					method.Invoke(this, null);
				}, canMethodInstance, keyGesture));
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000089FC File Offset: 0x00006BFC
		private void ProcessAsyncMethod(MethodInfo method, List<MethodInfo> canMethods)
		{
			ParameterInfo parameterInfo = method.GetParameters().FirstOrDefault<ParameterInfo>();
			Type type = typeof(Action<, >).MakeGenericType(new Type[]
			{
				parameterInfo.ParameterType,
				typeof(CancellationToken)
			});
			Type type2 = typeof(AsyncDelegateCommand<>).MakeGenericType(new Type[]
			{
				parameterInfo.ParameterType
			});
			Delegate @delegate = Delegate.CreateDelegate(type, this, method);
			Func<object, bool> canMethodInstance = this.GetCanMethodInstance(method, canMethods);
			KeyGesture keyGesture = this.GetKeyGesture(method);
			IAsyncDelegateCommand value = (IAsyncDelegateCommand)Activator.CreateInstance(type2, new object[]
			{
				@delegate,
				canMethodInstance,
				keyGesture
			});
			this.Commands.Add(method.Name, value);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00008B18 File Offset: 0x00006D18
		private Func<object, bool> GetCanMethodInstance(MethodInfo method, IEnumerable<MethodInfo> canMethods)
		{
			MethodInfo methodInfo = canMethods.FirstOrDefault((MethodInfo cm) => cm.Name == "CanExecute" + method.Name);
			Func<object, bool> result;
			if (methodInfo != null && BaseController.MethodHasParameters(methodInfo, 1) && methodInfo.ReturnType == typeof(bool))
			{
				List<Type> list = (from p in methodInfo.GetParameters()
				select p.ParameterType).ToList<Type>();
				list.Add(methodInfo.ReturnType);
				Type funcType = Expression.GetFuncType(list.ToArray());
				result = (Func<object, bool>)Delegate.CreateDelegate(funcType, this, methodInfo);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00008BDC File Offset: 0x00006DDC
		private KeyGesture GetKeyGesture(MethodInfo method)
		{
			CustomCommandAttribute customCommandAttribute = (CustomCommandAttribute)method.GetCustomAttribute(typeof(CustomCommandAttribute));
			return customCommandAttribute.KeyGesture;
		}

		// Token: 0x0400008A RID: 138
		private readonly ICommandRepository commands;

		// Token: 0x0400008B RID: 139
		private readonly EventAggregator eventAggregator;
	}
}
