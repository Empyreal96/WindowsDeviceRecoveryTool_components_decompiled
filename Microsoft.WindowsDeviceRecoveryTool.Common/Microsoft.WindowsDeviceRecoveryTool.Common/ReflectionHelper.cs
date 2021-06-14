using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Microsoft.WindowsDeviceRecoveryTool.Common
{
	// Token: 0x0200000A RID: 10
	[CompilerGenerated]
	public static class ReflectionHelper
	{
		// Token: 0x0600003A RID: 58 RVA: 0x00002D54 File Offset: 0x00000F54
		public static string GetName<T>(this T instance, Expression<Func<T, object>> expression)
		{
			if (expression == null)
			{
				throw new ArgumentException("The expression cannot be null.");
			}
			return ReflectionHelper.GetName<T>(expression);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002D84 File Offset: 0x00000F84
		public static string GetName<T>(Expression<Func<T, object>> expression)
		{
			if (expression == null)
			{
				throw new ArgumentException("The expression cannot be null.");
			}
			return ReflectionHelper.GetName(expression.Body);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002DB8 File Offset: 0x00000FB8
		public static string GetName(this object instance, Expression<Action> expression)
		{
			if (expression == null)
			{
				throw new ArgumentException("The expression cannot be null.");
			}
			return ReflectionHelper.GetName(expression.Body);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002DEC File Offset: 0x00000FEC
		public static string GetName<T>(this object instance, Expression<Action<T>> expression)
		{
			if (expression == null)
			{
				throw new ArgumentException("The expression cannot be null.");
			}
			return ReflectionHelper.GetName(expression.Body);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002E20 File Offset: 0x00001020
		public static string GetName<T>(this T instance, Expression<Action<T>> expression)
		{
			if (expression == null)
			{
				throw new ArgumentException("The expression cannot be null.");
			}
			return ReflectionHelper.GetName(expression.Body);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002E54 File Offset: 0x00001054
		public static string GetName<T>(Expression<Action<T>> expression)
		{
			if (expression == null)
			{
				throw new ArgumentException("The expression cannot be null.");
			}
			return ReflectionHelper.GetName(expression.Body);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002E88 File Offset: 0x00001088
		public static string GetName<T>(Expression<Func<T>> expression)
		{
			if (expression == null)
			{
				throw new ArgumentException("The expression cannot be null.");
			}
			return ReflectionHelper.GetName(expression.Body);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002EBC File Offset: 0x000010BC
		private static string GetName(Expression expression)
		{
			if (expression == null)
			{
				throw new ArgumentException("The expression cannot be null.");
			}
			string name;
			if (expression is MemberExpression)
			{
				MemberExpression memberExpression = (MemberExpression)expression;
				name = memberExpression.Member.Name;
			}
			else if (expression is MethodCallExpression)
			{
				MethodCallExpression methodCallExpression = (MethodCallExpression)expression;
				name = methodCallExpression.Method.Name;
			}
			else
			{
				if (!(expression is UnaryExpression))
				{
					throw new ArgumentException("Invalid expression");
				}
				UnaryExpression unaryExpression = (UnaryExpression)expression;
				name = ReflectionHelper.GetName(unaryExpression);
			}
			return name;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002F64 File Offset: 0x00001164
		private static string GetName(UnaryExpression unaryExpression)
		{
			string name;
			if (unaryExpression.Operand is MethodCallExpression)
			{
				MethodCallExpression methodCallExpression = (MethodCallExpression)unaryExpression.Operand;
				name = methodCallExpression.Method.Name;
			}
			else
			{
				name = ((MemberExpression)unaryExpression.Operand).Member.Name;
			}
			return name;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002FBC File Offset: 0x000011BC
		public static TAttr GetAttribute<TAttr>(this object currentObject) where TAttr : Attribute
		{
			Type type = currentObject.GetType();
			TAttr result;
			if (type.IsDefined(typeof(TAttr), false))
			{
				result = (TAttr)((object)type.GetCustomAttributes(typeof(TAttr), false).First<object>());
			}
			else
			{
				result = default(TAttr);
			}
			return result;
		}
	}
}
