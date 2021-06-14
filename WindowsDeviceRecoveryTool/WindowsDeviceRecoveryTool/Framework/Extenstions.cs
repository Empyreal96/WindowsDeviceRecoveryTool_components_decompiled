using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;
using Microsoft.WindowsDeviceRecoveryTool.Common;

namespace Microsoft.WindowsDeviceRecoveryTool.Framework
{
	// Token: 0x02000047 RID: 71
	public static class Extenstions
	{
		// Token: 0x06000276 RID: 630 RVA: 0x0000F234 File Offset: 0x0000D434
		public static void Run(this ICommand command)
		{
			command.Execute(null);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000F23F File Offset: 0x0000D43F
		public static void Run(this ICommand command, object parameter)
		{
			command.Execute(parameter);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000F24C File Offset: 0x0000D44C
		public static void Run(this IDictionary<string, IDelegateCommand> dictionary, Expression<Action> expression)
		{
			string name = dictionary.GetName(expression);
			Extenstions.Run(dictionary, expression.Body as MethodCallExpression, name, expression.Parameters);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000F27C File Offset: 0x0000D47C
		public static void Run<T>(this IDictionary<string, IDelegateCommand> dictionary, Expression<Action<T>> expression)
		{
			string name = ReflectionHelper.GetName<T>(expression);
			Extenstions.Run(dictionary, expression.Body as MethodCallExpression, name, expression.Parameters);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000F2AC File Offset: 0x0000D4AC
		public static void Run<T>(this IDictionary<string, IDelegateCommand> dictionary, Expression<Func<T>> expression)
		{
			string name = ReflectionHelper.GetName<T>(expression);
			Extenstions.Run(dictionary, expression.Body as MethodCallExpression, name, expression.Parameters);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000F2DC File Offset: 0x0000D4DC
		public static void RunAsyncCommandSync<T>(this IDictionary<string, IDelegateCommand> dictionary, Expression<Action<T>> expression)
		{
			string name = ReflectionHelper.GetName<T>(expression);
			IAsyncDelegateCommand asyncDelegateCommand = dictionary[name] as IAsyncDelegateCommand;
			if (asyncDelegateCommand != null)
			{
				Extenstions.Run(dictionary, expression.Body as MethodCallExpression, name, expression.Parameters);
				asyncDelegateCommand.Wait();
			}
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000F328 File Offset: 0x0000D528
		public static void RaiseCanExecuteChanged<T>(this IDictionary<string, IDelegateCommand> dictionary, Expression<Action<T>> expression)
		{
			string name = ReflectionHelper.GetName<T>(expression);
			dictionary[name].RaiseCanExecuteChanged();
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000F34C File Offset: 0x0000D54C
		public static void RaiseCanExecuteChanged<T>(this IDictionary<string, IDelegateCommand> dictionary, Expression<Func<T>> expression)
		{
			string name = ReflectionHelper.GetName<T>(expression);
			dictionary[name].RaiseCanExecuteChanged();
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000F370 File Offset: 0x0000D570
		public static T Get<T>(this ExportProvider container)
		{
			return container.GetExportedValue<T>();
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000F388 File Offset: 0x0000D588
		public static T Get<T>(this ExportProvider container, string name)
		{
			return container.GetExportedValue<T>(name);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000F3A4 File Offset: 0x0000D5A4
		private static void Run(IDictionary<string, IDelegateCommand> dictionary, MethodCallExpression expressionBody, string commandName, IEnumerable<ParameterExpression> parameters)
		{
			if (expressionBody.Arguments.Any<Expression>())
			{
				Expression expression = expressionBody.Arguments[0];
				ConstantExpression constantExpression = expression as ConstantExpression;
				if (constantExpression != null)
				{
					dictionary[commandName].Execute(constantExpression.Value);
					return;
				}
				if (expression is MemberExpression || expression is NewExpression)
				{
					LambdaExpression lambdaExpression = Expression.Lambda(expression, parameters);
					Delegate @delegate = lambdaExpression.Compile();
					try
					{
						object parameter = @delegate.DynamicInvoke(new object[expressionBody.Arguments.Count - 1]);
						dictionary[commandName].Execute(parameter);
					}
					catch (TargetParameterCountException)
					{
						object parameter = @delegate.DynamicInvoke(new object[expressionBody.Arguments.Count]);
						dictionary[commandName].Execute(parameter);
					}
					return;
				}
			}
			dictionary[commandName].Execute(null);
		}
	}
}
