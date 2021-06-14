using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace System.Data.Services.Client.Metadata
{
	// Token: 0x02000132 RID: 306
	internal static class ClientTypeUtil
	{
		// Token: 0x06000AF6 RID: 2806 RVA: 0x0002B996 File Offset: 0x00029B96
		internal static void SetClientTypeAnnotation(this IEdmModel model, IEdmType edmType, ClientTypeAnnotation annotation)
		{
			model.SetAnnotationValue(edmType, annotation);
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0002B9A0 File Offset: 0x00029BA0
		internal static ClientTypeAnnotation GetClientTypeAnnotation(this ClientEdmModel model, Type type)
		{
			IEdmType orCreateEdmType = model.GetOrCreateEdmType(type);
			return model.GetClientTypeAnnotation(orCreateEdmType);
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x0002B9BC File Offset: 0x00029BBC
		internal static ClientTypeAnnotation GetClientTypeAnnotation(this IEdmModel model, IEdmType edmType)
		{
			return model.GetAnnotationValue(edmType);
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0002B9C5 File Offset: 0x00029BC5
		internal static void SetClientPropertyAnnotation(this IEdmProperty edmProperty, ClientPropertyAnnotation annotation)
		{
			annotation.Model.SetAnnotationValue(edmProperty, annotation);
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0002B9D4 File Offset: 0x00029BD4
		internal static ClientPropertyAnnotation GetClientPropertyAnnotation(this IEdmModel model, IEdmProperty edmProperty)
		{
			return model.GetAnnotationValue(edmProperty);
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0002B9E0 File Offset: 0x00029BE0
		internal static ClientTypeAnnotation GetClientTypeAnnotation(this IEdmModel model, IEdmProperty edmProperty)
		{
			IEdmType definition = edmProperty.Type.Definition;
			return model.GetAnnotationValue(definition);
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0002BA00 File Offset: 0x00029C00
		internal static IEdmTypeReference ToEdmTypeReference(this IEdmType edmType, bool isNullable)
		{
			return edmType.ToTypeReference(isNullable);
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0002BA0C File Offset: 0x00029C0C
		internal static string FullName(this IEdmType edmType)
		{
			IEdmSchemaElement edmSchemaElement = edmType as IEdmSchemaElement;
			if (edmSchemaElement != null)
			{
				return edmSchemaElement.FullName();
			}
			return null;
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x0002BA2C File Offset: 0x00029C2C
		internal static MethodInfo GetMethodForGenericType(Type propertyType, Type genericTypeDefinition, string methodName, out Type type)
		{
			type = null;
			Type implementationType = ClientTypeUtil.GetImplementationType(propertyType, genericTypeDefinition);
			if (null != implementationType)
			{
				Type[] genericArguments = implementationType.GetGenericArguments();
				MethodInfo method = implementationType.GetMethod(methodName);
				type = genericArguments[genericArguments.Length - 1];
				return method;
			}
			return null;
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x0002BA68 File Offset: 0x00029C68
		internal static Action<object, object> GetAddToCollectionDelegate(Type listType)
		{
			Type type;
			MethodInfo addToCollectionMethod = ClientTypeUtil.GetAddToCollectionMethod(listType, out type);
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "list");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object), "element");
			Expression body = Expression.Call(Expression.Convert(parameterExpression, listType), addToCollectionMethod, new Expression[]
			{
				Expression.Convert(parameterExpression2, type)
			});
			LambdaExpression lambdaExpression = Expression.Lambda(body, new ParameterExpression[]
			{
				parameterExpression,
				parameterExpression2
			});
			return (Action<object, object>)lambdaExpression.Compile();
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x0002BAF5 File Offset: 0x00029CF5
		internal static MethodInfo GetAddToCollectionMethod(Type collectionType, out Type type)
		{
			return ClientTypeUtil.GetMethodForGenericType(collectionType, typeof(ICollection<>), "Add", out type);
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x0002BB10 File Offset: 0x00029D10
		internal static Type GetImplementationType(Type type, Type genericTypeDefinition)
		{
			if (ClientTypeUtil.IsConstructedGeneric(type, genericTypeDefinition))
			{
				return type;
			}
			Type type2 = null;
			foreach (Type type3 in type.GetInterfaces())
			{
				if (ClientTypeUtil.IsConstructedGeneric(type3, genericTypeDefinition))
				{
					if (!(null == type2))
					{
						throw Error.NotSupported(Strings.ClientType_MultipleImplementationNotSupported);
					}
					type2 = type3;
				}
			}
			return type2;
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x0002BB65 File Offset: 0x00029D65
		internal static bool TypeIsEntity(Type t, ClientEdmModel model)
		{
			return model.GetOrCreateEdmType(t).TypeKind == EdmTypeKind.Entity;
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x0002BB76 File Offset: 0x00029D76
		internal static bool TypeOrElementTypeIsEntity(Type type)
		{
			type = TypeSystem.GetElementType(type);
			type = (Nullable.GetUnderlyingType(type) ?? type);
			return !PrimitiveType.IsKnownType(type) && ClientTypeUtil.GetKeyPropertiesOnType(type) != null;
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x0002BBA3 File Offset: 0x00029DA3
		internal static bool IsDataServiceCollection(Type type)
		{
			while (type != null)
			{
				if (type.IsGenericType() && WebUtil.IsDataServiceCollectionType(type.GetGenericTypeDefinition()))
				{
					return true;
				}
				type = type.GetBaseType();
			}
			return false;
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x0002BBD0 File Offset: 0x00029DD0
		internal static bool CanAssignNull(Type type)
		{
			return !type.IsValueType() || (type.IsGenericType() && type.GetGenericTypeDefinition() == typeof(Nullable<>));
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x0002BEDC File Offset: 0x0002A0DC
		internal static IEnumerable<PropertyInfo> GetPropertiesOnType(Type type, bool declaredOnly)
		{
			type.ToString();
			if (!PrimitiveType.IsKnownType(type))
			{
				foreach (PropertyInfo propertyInfo in type.GetPublicProperties(true, declaredOnly))
				{
					Type propertyType = propertyInfo.PropertyType;
					propertyType = (Nullable.GetUnderlyingType(propertyType) ?? propertyType);
					if (!propertyType.IsPointer && (!propertyType.IsArray || !(typeof(byte[]) != propertyType) || !(typeof(char[]) != propertyType)) && !(typeof(IntPtr) == propertyType) && !(typeof(UIntPtr) == propertyType) && (!declaredOnly || !ClientTypeUtil.IsOverride(type, propertyInfo)) && propertyInfo.CanRead && (!propertyType.IsValueType() || propertyInfo.CanWrite) && !propertyType.ContainsGenericParameters() && propertyInfo.GetIndexParameters().Length == 0)
					{
						yield return propertyInfo;
					}
				}
			}
			yield break;
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x0002BF00 File Offset: 0x0002A100
		internal static PropertyInfo[] GetKeyPropertiesOnType(Type type)
		{
			bool flag;
			return ClientTypeUtil.GetKeyPropertiesOnType(type, out flag);
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x0002BF7C File Offset: 0x0002A17C
		internal static PropertyInfo[] GetKeyPropertiesOnType(Type type, out bool hasProperties)
		{
			if (CommonUtil.IsUnsupportedType(type))
			{
				throw new InvalidOperationException(Strings.ClientType_UnsupportedType(type));
			}
			string p = type.ToString();
			IEnumerable<object> customAttributes = type.GetCustomAttributes(true);
			bool flag = customAttributes.OfType<DataServiceEntityAttribute>().Any<DataServiceEntityAttribute>();
			DataServiceKeyAttribute dataServiceKeyAttribute = customAttributes.OfType<DataServiceKeyAttribute>().FirstOrDefault<DataServiceKeyAttribute>();
			List<PropertyInfo> list = new List<PropertyInfo>();
			PropertyInfo[] properties = ClientTypeUtil.GetPropertiesOnType(type, false).ToArray<PropertyInfo>();
			hasProperties = (properties.Length > 0);
			ClientTypeUtil.KeyKind keyKind = ClientTypeUtil.KeyKind.NotKey;
			ClientTypeUtil.KeyKind keyKind2 = ClientTypeUtil.KeyKind.NotKey;
			foreach (PropertyInfo propertyInfo in properties)
			{
				if ((keyKind2 = ClientTypeUtil.IsKeyProperty(propertyInfo, dataServiceKeyAttribute)) != ClientTypeUtil.KeyKind.NotKey)
				{
					if (keyKind2 > keyKind)
					{
						list.Clear();
						keyKind = keyKind2;
						list.Add(propertyInfo);
					}
					else if (keyKind2 == keyKind)
					{
						list.Add(propertyInfo);
					}
				}
			}
			Type type2 = null;
			foreach (PropertyInfo propertyInfo2 in list)
			{
				if (null == type2)
				{
					type2 = propertyInfo2.DeclaringType;
				}
				else if (type2 != propertyInfo2.DeclaringType)
				{
					throw Error.InvalidOperation(Strings.ClientType_KeysOnDifferentDeclaredType(p));
				}
				if (!PrimitiveType.IsKnownType(propertyInfo2.PropertyType))
				{
					throw Error.InvalidOperation(Strings.ClientType_KeysMustBeSimpleTypes(p));
				}
			}
			if (keyKind2 == ClientTypeUtil.KeyKind.AttributedKey && list.Count != dataServiceKeyAttribute.KeyNames.Count)
			{
				string p2 = (from string a in dataServiceKeyAttribute.KeyNames
				where null == (from b in properties
				where b.Name == a
				select b).FirstOrDefault<PropertyInfo>()
				select a).First<string>();
				throw Error.InvalidOperation(Strings.ClientType_MissingProperty(p, p2));
			}
			if (list.Count > 0)
			{
				return list.ToArray();
			}
			if (!flag)
			{
				return null;
			}
			return ClientTypeUtil.EmptyPropertyInfoArray;
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x0002C154 File Offset: 0x0002A354
		internal static Type GetMemberType(MemberInfo member)
		{
			PropertyInfo propertyInfo = member as PropertyInfo;
			if (propertyInfo != null)
			{
				return propertyInfo.PropertyType;
			}
			FieldInfo fieldInfo = member as FieldInfo;
			return fieldInfo.FieldType;
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x0002C188 File Offset: 0x0002A388
		private static ClientTypeUtil.KeyKind IsKeyProperty(PropertyInfo propertyInfo, DataServiceKeyAttribute dataServiceKeyAttribute)
		{
			string name = propertyInfo.Name;
			ClientTypeUtil.KeyKind result = ClientTypeUtil.KeyKind.NotKey;
			if (dataServiceKeyAttribute != null && dataServiceKeyAttribute.KeyNames.Contains(name))
			{
				result = ClientTypeUtil.KeyKind.AttributedKey;
			}
			else if (name.EndsWith("ID", StringComparison.Ordinal))
			{
				string name2 = propertyInfo.DeclaringType.Name;
				if (name.Length == name2.Length + 2 && name.StartsWith(name2, StringComparison.Ordinal))
				{
					result = ClientTypeUtil.KeyKind.TypeNameId;
				}
				else if (2 == name.Length)
				{
					result = ClientTypeUtil.KeyKind.Id;
				}
			}
			return result;
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x0002C1F7 File Offset: 0x0002A3F7
		private static bool IsConstructedGeneric(Type type, Type genericTypeDefinition)
		{
			return type.IsGenericType() && type.GetGenericTypeDefinition() == genericTypeDefinition && !type.ContainsGenericParameters();
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x0002C21C File Offset: 0x0002A41C
		private static bool IsOverride(Type type, PropertyInfo propertyInfo)
		{
			MethodInfo getMethod = propertyInfo.GetGetMethod();
			return getMethod != null && getMethod.GetBaseDefinition().DeclaringType != type;
		}

		// Token: 0x040005FB RID: 1531
		internal static readonly PropertyInfo[] EmptyPropertyInfoArray = new PropertyInfo[0];

		// Token: 0x02000133 RID: 307
		private enum KeyKind
		{
			// Token: 0x040005FD RID: 1533
			NotKey,
			// Token: 0x040005FE RID: 1534
			Id,
			// Token: 0x040005FF RID: 1535
			TypeNameId,
			// Token: 0x04000600 RID: 1536
			AttributedKey
		}
	}
}
