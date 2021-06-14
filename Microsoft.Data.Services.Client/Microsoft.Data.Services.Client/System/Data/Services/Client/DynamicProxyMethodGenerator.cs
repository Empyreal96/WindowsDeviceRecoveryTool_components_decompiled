using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Security;
using System.Security.Permissions;

namespace System.Data.Services.Client
{
	// Token: 0x02000061 RID: 97
	internal class DynamicProxyMethodGenerator
	{
		// Token: 0x06000327 RID: 807 RVA: 0x0000E100 File Offset: 0x0000C300
		internal Expression GetCallWrapper(MethodBase method, params Expression[] arguments)
		{
			if (!this.ThisAssemblyCanCreateHostedDynamicMethodsWithSkipVisibility())
			{
				return DynamicProxyMethodGenerator.WrapOriginalMethodWithExpression(method, arguments);
			}
			return DynamicProxyMethodGenerator.GetDynamicMethodCallWrapper(method, arguments);
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000E119 File Offset: 0x0000C319
		protected virtual bool ThisAssemblyCanCreateHostedDynamicMethodsWithSkipVisibility()
		{
			return typeof(DynamicProxyMethodGenerator).Assembly.IsFullyTrusted;
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000E138 File Offset: 0x0000C338
		[SecuritySafeCritical]
		private static Expression GetDynamicMethodCallWrapper(MethodBase method, params Expression[] arguments)
		{
			if (method.DeclaringType == null || method.DeclaringType.Assembly != typeof(DynamicProxyMethodGenerator).Assembly)
			{
				return DynamicProxyMethodGenerator.WrapOriginalMethodWithExpression(method, arguments);
			}
			string name = "_dynamic_" + method.ReflectedType.Name + "_" + method.Name;
			MethodInfo methodInfo = null;
			lock (DynamicProxyMethodGenerator.dynamicProxyMethods)
			{
				DynamicProxyMethodGenerator.dynamicProxyMethods.TryGetValue(method, out methodInfo);
			}
			if (methodInfo != null)
			{
				return Expression.Call(methodInfo, arguments);
			}
			Type[] array = (from p in method.GetParameters()
			select p.ParameterType).ToArray<Type>();
			MethodInfo methodInfo2 = method as MethodInfo;
			DynamicMethod dynamicMethod = DynamicProxyMethodGenerator.CreateDynamicMethod(name, (methodInfo2 == null) ? method.ReflectedType : methodInfo2.ReturnType, array);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			for (int i = 0; i < array.Length; i++)
			{
				switch (i)
				{
				case 0:
					ilgenerator.Emit(OpCodes.Ldarg_0);
					break;
				case 1:
					ilgenerator.Emit(OpCodes.Ldarg_1);
					break;
				case 2:
					ilgenerator.Emit(OpCodes.Ldarg_2);
					break;
				case 3:
					ilgenerator.Emit(OpCodes.Ldarg_3);
					break;
				default:
					ilgenerator.Emit(OpCodes.Ldarg, i);
					break;
				}
			}
			if (methodInfo2 == null)
			{
				ilgenerator.Emit(OpCodes.Newobj, (ConstructorInfo)method);
			}
			else
			{
				ilgenerator.EmitCall(OpCodes.Call, methodInfo2, null);
			}
			ilgenerator.Emit(OpCodes.Ret);
			lock (DynamicProxyMethodGenerator.dynamicProxyMethods)
			{
				if (!DynamicProxyMethodGenerator.dynamicProxyMethods.ContainsKey(method))
				{
					DynamicProxyMethodGenerator.dynamicProxyMethods.Add(method, dynamicMethod);
				}
			}
			return Expression.Call(dynamicMethod, arguments);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000E348 File Offset: 0x0000C548
		[SecurityCritical]
		[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
		private static DynamicMethod CreateDynamicMethod(string name, Type returnType, Type[] parameterTypes)
		{
			return new DynamicMethod(name, returnType, parameterTypes, typeof(DynamicProxyMethodGenerator).Module, true);
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000E364 File Offset: 0x0000C564
		private static Expression WrapOriginalMethodWithExpression(MethodBase method, Expression[] arguments)
		{
			MethodInfo methodInfo = method as MethodInfo;
			if (methodInfo != null)
			{
				return Expression.Call(methodInfo, arguments);
			}
			return Expression.New((ConstructorInfo)method, arguments);
		}

		// Token: 0x04000289 RID: 649
		private static Dictionary<MethodBase, MethodInfo> dynamicProxyMethods = new Dictionary<MethodBase, MethodInfo>(EqualityComparer<MethodBase>.Default);
	}
}
