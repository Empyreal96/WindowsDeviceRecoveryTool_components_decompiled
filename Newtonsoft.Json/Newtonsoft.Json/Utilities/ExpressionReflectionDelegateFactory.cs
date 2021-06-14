using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020000F3 RID: 243
	internal class ExpressionReflectionDelegateFactory : ReflectionDelegateFactory
	{
		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000B65 RID: 2917 RVA: 0x0002E305 File Offset: 0x0002C505
		internal static ReflectionDelegateFactory Instance
		{
			get
			{
				return ExpressionReflectionDelegateFactory._instance;
			}
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x0002E30C File Offset: 0x0002C50C
		public override ObjectConstructor<object> CreateParametrizedConstructor(MethodBase method)
		{
			ValidationUtils.ArgumentNotNull(method, "method");
			Type typeFromHandle = typeof(object);
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object[]), "args");
			Expression body = this.BuildMethodCall(method, typeFromHandle, null, parameterExpression);
			LambdaExpression lambdaExpression = Expression.Lambda(typeof(ObjectConstructor<object>), body, new ParameterExpression[]
			{
				parameterExpression
			});
			return (ObjectConstructor<object>)lambdaExpression.Compile();
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x0002E380 File Offset: 0x0002C580
		public override MethodCall<T, object> CreateMethodCall<T>(MethodBase method)
		{
			ValidationUtils.ArgumentNotNull(method, "method");
			Type typeFromHandle = typeof(object);
			ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle, "target");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object[]), "args");
			Expression body = this.BuildMethodCall(method, typeFromHandle, parameterExpression, parameterExpression2);
			LambdaExpression lambdaExpression = Expression.Lambda(typeof(MethodCall<T, object>), body, new ParameterExpression[]
			{
				parameterExpression,
				parameterExpression2
			});
			return (MethodCall<T, object>)lambdaExpression.Compile();
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x0002E408 File Offset: 0x0002C608
		private Expression BuildMethodCall(MethodBase method, Type type, ParameterExpression targetParameterExpression, ParameterExpression argsParameterExpression)
		{
			ParameterInfo[] parameters = method.GetParameters();
			Expression[] array = new Expression[parameters.Length];
			IList<ExpressionReflectionDelegateFactory.ByRefParameter> list = new List<ExpressionReflectionDelegateFactory.ByRefParameter>();
			for (int i = 0; i < parameters.Length; i++)
			{
				ParameterInfo parameterInfo = parameters[i];
				Type type2 = parameterInfo.ParameterType;
				bool flag = false;
				if (type2.IsByRef)
				{
					type2 = type2.GetElementType();
					flag = true;
				}
				Expression index = Expression.Constant(i);
				Expression expression = Expression.ArrayIndex(argsParameterExpression, index);
				Expression expression3;
				if (type2.IsValueType())
				{
					BinaryExpression expression2 = Expression.Coalesce(expression, Expression.New(type2));
					expression3 = this.EnsureCastExpression(expression2, type2);
				}
				else
				{
					expression3 = this.EnsureCastExpression(expression, type2);
				}
				if (flag)
				{
					ParameterExpression parameterExpression = Expression.Variable(type2);
					list.Add(new ExpressionReflectionDelegateFactory.ByRefParameter
					{
						Value = expression3,
						Variable = parameterExpression,
						IsOut = parameterInfo.IsOut
					});
					expression3 = parameterExpression;
				}
				array[i] = expression3;
			}
			Expression expression4;
			if (method.IsConstructor)
			{
				expression4 = Expression.New((ConstructorInfo)method, array);
			}
			else if (method.IsStatic)
			{
				expression4 = Expression.Call((MethodInfo)method, array);
			}
			else
			{
				Expression instance = this.EnsureCastExpression(targetParameterExpression, method.DeclaringType);
				expression4 = Expression.Call(instance, (MethodInfo)method, array);
			}
			if (method is MethodInfo)
			{
				MethodInfo methodInfo = (MethodInfo)method;
				if (methodInfo.ReturnType != typeof(void))
				{
					expression4 = this.EnsureCastExpression(expression4, type);
				}
				else
				{
					expression4 = Expression.Block(expression4, Expression.Constant(null));
				}
			}
			else
			{
				expression4 = this.EnsureCastExpression(expression4, type);
			}
			if (list.Count > 0)
			{
				IList<ParameterExpression> list2 = new List<ParameterExpression>();
				IList<Expression> list3 = new List<Expression>();
				foreach (ExpressionReflectionDelegateFactory.ByRefParameter byRefParameter in list)
				{
					if (!byRefParameter.IsOut)
					{
						list3.Add(Expression.Assign(byRefParameter.Variable, byRefParameter.Value));
					}
					list2.Add(byRefParameter.Variable);
				}
				list3.Add(expression4);
				expression4 = Expression.Block(list2, list3);
			}
			return expression4;
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x0002E65C File Offset: 0x0002C85C
		public override Func<T> CreateDefaultConstructor<T>(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			if (type.IsAbstract())
			{
				return () => (T)((object)Activator.CreateInstance(type));
			}
			Func<T> result;
			try
			{
				Type typeFromHandle = typeof(T);
				Expression expression = Expression.New(type);
				expression = this.EnsureCastExpression(expression, typeFromHandle);
				LambdaExpression lambdaExpression = Expression.Lambda(typeof(Func<T>), expression, new ParameterExpression[0]);
				Func<T> func = (Func<T>)lambdaExpression.Compile();
				result = func;
			}
			catch
			{
				result = (() => (T)((object)Activator.CreateInstance(type)));
			}
			return result;
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x0002E720 File Offset: 0x0002C920
		public override Func<T, object> CreateGet<T>(PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			Type typeFromHandle = typeof(T);
			Type typeFromHandle2 = typeof(object);
			ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle, "instance");
			MethodInfo getMethod = propertyInfo.GetGetMethod(true);
			Expression expression;
			if (getMethod.IsStatic)
			{
				expression = Expression.MakeMemberAccess(null, propertyInfo);
			}
			else
			{
				Expression expression2 = this.EnsureCastExpression(parameterExpression, propertyInfo.DeclaringType);
				expression = Expression.MakeMemberAccess(expression2, propertyInfo);
			}
			expression = this.EnsureCastExpression(expression, typeFromHandle2);
			LambdaExpression lambdaExpression = Expression.Lambda(typeof(Func<T, object>), expression, new ParameterExpression[]
			{
				parameterExpression
			});
			return (Func<T, object>)lambdaExpression.Compile();
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x0002E7C8 File Offset: 0x0002C9C8
		public override Func<T, object> CreateGet<T>(FieldInfo fieldInfo)
		{
			ValidationUtils.ArgumentNotNull(fieldInfo, "fieldInfo");
			ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "source");
			Expression expression;
			if (fieldInfo.IsStatic)
			{
				expression = Expression.Field(null, fieldInfo);
			}
			else
			{
				Expression expression2 = this.EnsureCastExpression(parameterExpression, fieldInfo.DeclaringType);
				expression = Expression.Field(expression2, fieldInfo);
			}
			expression = this.EnsureCastExpression(expression, typeof(object));
			return Expression.Lambda<Func<T, object>>(expression, new ParameterExpression[]
			{
				parameterExpression
			}).Compile();
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x0002E84C File Offset: 0x0002CA4C
		public override Action<T, object> CreateSet<T>(FieldInfo fieldInfo)
		{
			ValidationUtils.ArgumentNotNull(fieldInfo, "fieldInfo");
			if (fieldInfo.DeclaringType.IsValueType() || fieldInfo.IsInitOnly)
			{
				return LateBoundReflectionDelegateFactory.Instance.CreateSet<T>(fieldInfo);
			}
			ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "source");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object), "value");
			Expression expression;
			if (fieldInfo.IsStatic)
			{
				expression = Expression.Field(null, fieldInfo);
			}
			else
			{
				Expression expression2 = this.EnsureCastExpression(parameterExpression, fieldInfo.DeclaringType);
				expression = Expression.Field(expression2, fieldInfo);
			}
			Expression right = this.EnsureCastExpression(parameterExpression2, expression.Type);
			BinaryExpression body = Expression.Assign(expression, right);
			LambdaExpression lambdaExpression = Expression.Lambda(typeof(Action<T, object>), body, new ParameterExpression[]
			{
				parameterExpression,
				parameterExpression2
			});
			return (Action<T, object>)lambdaExpression.Compile();
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x0002E928 File Offset: 0x0002CB28
		public override Action<T, object> CreateSet<T>(PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			if (propertyInfo.DeclaringType.IsValueType())
			{
				return LateBoundReflectionDelegateFactory.Instance.CreateSet<T>(propertyInfo);
			}
			Type typeFromHandle = typeof(T);
			Type typeFromHandle2 = typeof(object);
			ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle, "instance");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeFromHandle2, "value");
			Expression expression = this.EnsureCastExpression(parameterExpression2, propertyInfo.PropertyType);
			MethodInfo setMethod = propertyInfo.GetSetMethod(true);
			Expression body;
			if (setMethod.IsStatic)
			{
				body = Expression.Call(setMethod, expression);
			}
			else
			{
				Expression instance = this.EnsureCastExpression(parameterExpression, propertyInfo.DeclaringType);
				body = Expression.Call(instance, setMethod, new Expression[]
				{
					expression
				});
			}
			LambdaExpression lambdaExpression = Expression.Lambda(typeof(Action<T, object>), body, new ParameterExpression[]
			{
				parameterExpression,
				parameterExpression2
			});
			return (Action<T, object>)lambdaExpression.Compile();
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x0002EA18 File Offset: 0x0002CC18
		private Expression EnsureCastExpression(Expression expression, Type targetType)
		{
			Type type = expression.Type;
			if (type == targetType || (!type.IsValueType() && targetType.IsAssignableFrom(type)))
			{
				return expression;
			}
			return Expression.Convert(expression, targetType);
		}

		// Token: 0x0400041B RID: 1051
		private static readonly ExpressionReflectionDelegateFactory _instance = new ExpressionReflectionDelegateFactory();

		// Token: 0x020000F4 RID: 244
		private class ByRefParameter
		{
			// Token: 0x0400041C RID: 1052
			public Expression Value;

			// Token: 0x0400041D RID: 1053
			public ParameterExpression Variable;

			// Token: 0x0400041E RID: 1054
			public bool IsOut;
		}
	}
}
