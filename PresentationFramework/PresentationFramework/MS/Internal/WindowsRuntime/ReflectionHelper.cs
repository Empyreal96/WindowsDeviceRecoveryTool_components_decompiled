using System;
using System.Reflection;

namespace MS.Internal.WindowsRuntime
{
	// Token: 0x020007F0 RID: 2032
	internal static class ReflectionHelper
	{
		// Token: 0x06007D45 RID: 32069 RVA: 0x0023345C File Offset: 0x0023165C
		public static TResult ReflectionStaticCall<TResult>(this Type type, string methodName)
		{
			MethodInfo method = type.GetMethod(methodName, Type.EmptyTypes);
			if (method == null)
			{
				throw new MissingMethodException(methodName);
			}
			object obj = method.Invoke(null, null);
			return (TResult)((object)obj);
		}

		// Token: 0x06007D46 RID: 32070 RVA: 0x00233498 File Offset: 0x00231698
		public static TResult ReflectionStaticCall<TResult, TArg>(this Type type, string methodName, TArg arg)
		{
			MethodInfo method = type.GetMethod(methodName, new Type[]
			{
				typeof(TArg)
			});
			if (method == null)
			{
				throw new MissingMethodException(methodName);
			}
			object obj = method.Invoke(null, new object[]
			{
				arg
			});
			return (TResult)((object)obj);
		}

		// Token: 0x06007D47 RID: 32071 RVA: 0x002334F0 File Offset: 0x002316F0
		public static TResult ReflectionCall<TResult>(this object obj, string methodName)
		{
			object obj2 = obj.ReflectionCall(methodName);
			return (TResult)((object)obj2);
		}

		// Token: 0x06007D48 RID: 32072 RVA: 0x0023350C File Offset: 0x0023170C
		public static object ReflectionCall(this object obj, string methodName)
		{
			MethodInfo method = obj.GetType().GetMethod(methodName, Type.EmptyTypes);
			if (method == null)
			{
				throw new MissingMethodException(methodName);
			}
			return method.Invoke(obj, null);
		}

		// Token: 0x06007D49 RID: 32073 RVA: 0x00233548 File Offset: 0x00231748
		public static object ReflectionCall<TArg1>(this object obj, string methodName, TArg1 arg1)
		{
			MethodInfo method = obj.GetType().GetMethod(methodName, new Type[]
			{
				typeof(TArg1)
			});
			if (method == null)
			{
				throw new MissingMethodException(methodName);
			}
			return method.Invoke(obj, new object[]
			{
				arg1
			});
		}

		// Token: 0x06007D4A RID: 32074 RVA: 0x002335A0 File Offset: 0x002317A0
		public static TResult ReflectionCall<TResult, TArg1>(this object obj, string methodName, TArg1 arg1)
		{
			object obj2 = obj.ReflectionCall(methodName, arg1);
			return (TResult)((object)obj2);
		}

		// Token: 0x06007D4B RID: 32075 RVA: 0x002335BC File Offset: 0x002317BC
		public static object ReflectionCall<TArg1, TArg2>(this object obj, string methodName, TArg1 arg1, TArg2 arg2)
		{
			MethodInfo method = obj.GetType().GetMethod(methodName, new Type[]
			{
				typeof(TArg1),
				typeof(TArg2)
			});
			if (method == null)
			{
				throw new MissingMethodException(methodName);
			}
			return method.Invoke(obj, new object[]
			{
				arg1,
				arg2
			});
		}

		// Token: 0x06007D4C RID: 32076 RVA: 0x00233628 File Offset: 0x00231828
		public static TResult ReflectionCall<TResult, TArg1, TArg2>(this object obj, string methodName, TArg1 arg1, TArg2 arg2)
		{
			object obj2 = obj.ReflectionCall(methodName, arg1, arg2);
			return (TResult)((object)obj2);
		}

		// Token: 0x06007D4D RID: 32077 RVA: 0x00233648 File Offset: 0x00231848
		public static TResult ReflectionGetField<TResult>(this object obj, string fieldName)
		{
			FieldInfo field = obj.GetType().GetField(fieldName);
			if (field == null)
			{
				throw new MissingFieldException(fieldName);
			}
			object value = field.GetValue(obj);
			return (TResult)((object)value);
		}

		// Token: 0x06007D4E RID: 32078 RVA: 0x00233680 File Offset: 0x00231880
		public static object ReflectionNew(this Type type)
		{
			ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
			if (constructor == null)
			{
				string message = type.FullName + "." + type.Name + "()";
				throw new MissingMethodException(message);
			}
			return constructor.Invoke(null);
		}

		// Token: 0x06007D4F RID: 32079 RVA: 0x002336CC File Offset: 0x002318CC
		public static object ReflectionNew<TArg1>(this Type type, TArg1 arg1)
		{
			ConstructorInfo constructor = type.GetConstructor(new Type[]
			{
				typeof(TArg1)
			});
			if (constructor == null)
			{
				string message = string.Format("{0}.{1}({2})", type.FullName, type.Name, typeof(TArg1).Name);
				throw new MissingMethodException(message);
			}
			return constructor.Invoke(new object[]
			{
				arg1
			});
		}

		// Token: 0x06007D50 RID: 32080 RVA: 0x00233740 File Offset: 0x00231940
		public static object ReflectionNew<TArg1, TArg2>(this Type type, TArg1 arg1, TArg2 arg2)
		{
			ConstructorInfo constructor = type.GetConstructor(new Type[]
			{
				typeof(TArg1),
				typeof(TArg2)
			});
			if (constructor == null)
			{
				string message = string.Format("{0}.{1}({2},{3})", new object[]
				{
					type.FullName,
					type.Name,
					typeof(TArg1).Name,
					typeof(TArg2).Name
				});
				throw new MissingMethodException(message);
			}
			return constructor.Invoke(new object[]
			{
				arg1,
				arg2
			});
		}

		// Token: 0x06007D51 RID: 32081 RVA: 0x002337EC File Offset: 0x002319EC
		public static TResult ReflectionGetProperty<TResult>(this object obj, string propertyName)
		{
			Type type = obj.GetType();
			PropertyInfo property = type.GetProperty(propertyName);
			if (property == null)
			{
				throw new MissingMemberException(propertyName);
			}
			return (TResult)((object)property.GetValue(obj));
		}

		// Token: 0x06007D52 RID: 32082 RVA: 0x00233824 File Offset: 0x00231A24
		public static object ReflectionGetProperty(this object obj, string propertyName)
		{
			return obj.ReflectionGetProperty(propertyName);
		}

		// Token: 0x06007D53 RID: 32083 RVA: 0x00233830 File Offset: 0x00231A30
		public static TResult ReflectionStaticGetProperty<TResult>(this Type type, string propertyName)
		{
			PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Static);
			if (property == null)
			{
				throw new MissingMemberException(propertyName);
			}
			return (TResult)((object)property.GetValue(null));
		}
	}
}
