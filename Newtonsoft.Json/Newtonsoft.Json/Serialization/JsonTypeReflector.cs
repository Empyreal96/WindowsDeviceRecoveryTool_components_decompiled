using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000C7 RID: 199
	internal static class JsonTypeReflector
	{
		// Token: 0x060009F4 RID: 2548 RVA: 0x00027888 File Offset: 0x00025A88
		public static T GetCachedAttribute<T>(object attributeProvider) where T : Attribute
		{
			return CachedAttributeGetter<T>.GetAttribute(attributeProvider);
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x00027890 File Offset: 0x00025A90
		public static DataContractAttribute GetDataContractAttribute(Type type)
		{
			Type type2 = type;
			while (type2 != null)
			{
				DataContractAttribute attribute = CachedAttributeGetter<DataContractAttribute>.GetAttribute(type2);
				if (attribute != null)
				{
					return attribute;
				}
				type2 = type2.BaseType();
			}
			return null;
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x000278C0 File Offset: 0x00025AC0
		public static DataMemberAttribute GetDataMemberAttribute(MemberInfo memberInfo)
		{
			if (memberInfo.MemberType() == MemberTypes.Field)
			{
				return CachedAttributeGetter<DataMemberAttribute>.GetAttribute(memberInfo);
			}
			PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
			DataMemberAttribute attribute = CachedAttributeGetter<DataMemberAttribute>.GetAttribute(propertyInfo);
			if (attribute == null && propertyInfo.IsVirtual())
			{
				Type type = propertyInfo.DeclaringType;
				while (attribute == null && type != null)
				{
					PropertyInfo propertyInfo2 = (PropertyInfo)ReflectionUtils.GetMemberInfoFromType(type, propertyInfo);
					if (propertyInfo2 != null && propertyInfo2.IsVirtual())
					{
						attribute = CachedAttributeGetter<DataMemberAttribute>.GetAttribute(propertyInfo2);
					}
					type = type.BaseType();
				}
			}
			return attribute;
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x00027938 File Offset: 0x00025B38
		public static MemberSerialization GetObjectMemberSerialization(Type objectType, bool ignoreSerializableAttribute)
		{
			JsonObjectAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonObjectAttribute>(objectType);
			if (cachedAttribute != null)
			{
				return cachedAttribute.MemberSerialization;
			}
			DataContractAttribute dataContractAttribute = JsonTypeReflector.GetDataContractAttribute(objectType);
			if (dataContractAttribute != null)
			{
				return MemberSerialization.OptIn;
			}
			if (!ignoreSerializableAttribute)
			{
				SerializableAttribute cachedAttribute2 = JsonTypeReflector.GetCachedAttribute<SerializableAttribute>(objectType);
				if (cachedAttribute2 != null)
				{
					return MemberSerialization.Fields;
				}
			}
			return MemberSerialization.OptOut;
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x00027974 File Offset: 0x00025B74
		public static JsonConverter GetJsonConverter(object attributeProvider)
		{
			JsonConverterAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonConverterAttribute>(attributeProvider);
			if (cachedAttribute != null)
			{
				Func<object[], JsonConverter> func = JsonTypeReflector.JsonConverterCreatorCache.Get(cachedAttribute.ConverterType);
				if (func != null)
				{
					return func(cachedAttribute.ConverterParameters);
				}
			}
			return null;
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x000279B0 File Offset: 0x00025BB0
		public static JsonConverter CreateJsonConverterInstance(Type converterType, object[] converterArgs)
		{
			Func<object[], JsonConverter> func = JsonTypeReflector.JsonConverterCreatorCache.Get(converterType);
			return func(converterArgs);
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x00027AD0 File Offset: 0x00025CD0
		private static Func<object[], JsonConverter> GetJsonConverterCreator(Type converterType)
		{
			Func<object> defaultConstructor = ReflectionUtils.HasDefaultConstructor(converterType, false) ? JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(converterType) : null;
			return delegate(object[] parameters)
			{
				JsonConverter result;
				try
				{
					if (parameters != null)
					{
						Type[] types = (from param in parameters
						select param.GetType()).ToArray<Type>();
						ConstructorInfo constructor = converterType.GetConstructor(types);
						if (!(null != constructor))
						{
							throw new JsonException("No matching parameterized constructor found for '{0}'.".FormatWith(CultureInfo.InvariantCulture, converterType));
						}
						ObjectConstructor<object> objectConstructor = JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor(constructor);
						result = (JsonConverter)objectConstructor(parameters);
					}
					else
					{
						if (defaultConstructor == null)
						{
							throw new JsonException("No parameterless constructor defined for '{0}'.".FormatWith(CultureInfo.InvariantCulture, converterType));
						}
						result = (JsonConverter)defaultConstructor();
					}
				}
				catch (Exception innerException)
				{
					throw new JsonException("Error creating '{0}'.".FormatWith(CultureInfo.InvariantCulture, converterType), innerException);
				}
				return result;
			};
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x00027B1D File Offset: 0x00025D1D
		public static TypeConverter GetTypeConverter(Type type)
		{
			return TypeDescriptor.GetConverter(type);
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x00027B25 File Offset: 0x00025D25
		private static Type GetAssociatedMetadataType(Type type)
		{
			return JsonTypeReflector.AssociatedMetadataTypesCache.Get(type);
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x00027B34 File Offset: 0x00025D34
		private static Type GetAssociateMetadataTypeFromAttribute(Type type)
		{
			Attribute[] attributes = ReflectionUtils.GetAttributes(type, null, true);
			foreach (Attribute attribute in attributes)
			{
				Type type2 = attribute.GetType();
				if (string.Equals(type2.FullName, "System.ComponentModel.DataAnnotations.MetadataTypeAttribute", StringComparison.Ordinal))
				{
					if (JsonTypeReflector._metadataTypeAttributeReflectionObject == null)
					{
						JsonTypeReflector._metadataTypeAttributeReflectionObject = ReflectionObject.Create(type2, new string[]
						{
							"MetadataClassType"
						});
					}
					return (Type)JsonTypeReflector._metadataTypeAttributeReflectionObject.GetValue(attribute, "MetadataClassType");
				}
			}
			return null;
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x00027BC0 File Offset: 0x00025DC0
		private static T GetAttribute<T>(Type type) where T : Attribute
		{
			Type associatedMetadataType = JsonTypeReflector.GetAssociatedMetadataType(type);
			T attribute;
			if (associatedMetadataType != null)
			{
				attribute = ReflectionUtils.GetAttribute<T>(associatedMetadataType, true);
				if (attribute != null)
				{
					return attribute;
				}
			}
			attribute = ReflectionUtils.GetAttribute<T>(type, true);
			if (attribute != null)
			{
				return attribute;
			}
			foreach (Type attributeProvider in type.GetInterfaces())
			{
				attribute = ReflectionUtils.GetAttribute<T>(attributeProvider, true);
				if (attribute != null)
				{
					return attribute;
				}
			}
			return default(T);
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x00027C44 File Offset: 0x00025E44
		private static T GetAttribute<T>(MemberInfo memberInfo) where T : Attribute
		{
			Type associatedMetadataType = JsonTypeReflector.GetAssociatedMetadataType(memberInfo.DeclaringType);
			T attribute;
			if (associatedMetadataType != null)
			{
				MemberInfo memberInfoFromType = ReflectionUtils.GetMemberInfoFromType(associatedMetadataType, memberInfo);
				if (memberInfoFromType != null)
				{
					attribute = ReflectionUtils.GetAttribute<T>(memberInfoFromType, true);
					if (attribute != null)
					{
						return attribute;
					}
				}
			}
			attribute = ReflectionUtils.GetAttribute<T>(memberInfo, true);
			if (attribute != null)
			{
				return attribute;
			}
			if (memberInfo.DeclaringType != null)
			{
				foreach (Type targetType in memberInfo.DeclaringType.GetInterfaces())
				{
					MemberInfo memberInfoFromType2 = ReflectionUtils.GetMemberInfoFromType(targetType, memberInfo);
					if (memberInfoFromType2 != null)
					{
						attribute = ReflectionUtils.GetAttribute<T>(memberInfoFromType2, true);
						if (attribute != null)
						{
							return attribute;
						}
					}
				}
			}
			return default(T);
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x00027D08 File Offset: 0x00025F08
		public static T GetAttribute<T>(object provider) where T : Attribute
		{
			Type type = provider as Type;
			if (type != null)
			{
				return JsonTypeReflector.GetAttribute<T>(type);
			}
			MemberInfo memberInfo = provider as MemberInfo;
			if (memberInfo != null)
			{
				return JsonTypeReflector.GetAttribute<T>(memberInfo);
			}
			return ReflectionUtils.GetAttribute<T>(provider, true);
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000A01 RID: 2561 RVA: 0x00027D4C File Offset: 0x00025F4C
		public static bool DynamicCodeGeneration
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonTypeReflector._dynamicCodeGeneration == null)
				{
					try
					{
						new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Demand();
						new ReflectionPermission(ReflectionPermissionFlag.RestrictedMemberAccess).Demand();
						new SecurityPermission(SecurityPermissionFlag.SkipVerification).Demand();
						new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
						new SecurityPermission(PermissionState.Unrestricted).Demand();
						JsonTypeReflector._dynamicCodeGeneration = new bool?(true);
					}
					catch (Exception)
					{
						JsonTypeReflector._dynamicCodeGeneration = new bool?(false);
					}
				}
				return JsonTypeReflector._dynamicCodeGeneration.Value;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000A02 RID: 2562 RVA: 0x00027DD4 File Offset: 0x00025FD4
		public static bool FullyTrusted
		{
			get
			{
				if (JsonTypeReflector._fullyTrusted == null)
				{
					AppDomain currentDomain = AppDomain.CurrentDomain;
					JsonTypeReflector._fullyTrusted = new bool?(currentDomain.IsHomogenous && currentDomain.IsFullyTrusted);
				}
				return JsonTypeReflector._fullyTrusted.Value;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000A03 RID: 2563 RVA: 0x00027E18 File Offset: 0x00026018
		public static ReflectionDelegateFactory ReflectionDelegateFactory
		{
			get
			{
				if (JsonTypeReflector.DynamicCodeGeneration)
				{
					return DynamicReflectionDelegateFactory.Instance;
				}
				return LateBoundReflectionDelegateFactory.Instance;
			}
		}

		// Token: 0x04000360 RID: 864
		public const string IdPropertyName = "$id";

		// Token: 0x04000361 RID: 865
		public const string RefPropertyName = "$ref";

		// Token: 0x04000362 RID: 866
		public const string TypePropertyName = "$type";

		// Token: 0x04000363 RID: 867
		public const string ValuePropertyName = "$value";

		// Token: 0x04000364 RID: 868
		public const string ArrayValuesPropertyName = "$values";

		// Token: 0x04000365 RID: 869
		public const string ShouldSerializePrefix = "ShouldSerialize";

		// Token: 0x04000366 RID: 870
		public const string SpecifiedPostfix = "Specified";

		// Token: 0x04000367 RID: 871
		private static bool? _dynamicCodeGeneration;

		// Token: 0x04000368 RID: 872
		private static bool? _fullyTrusted;

		// Token: 0x04000369 RID: 873
		private static readonly ThreadSafeStore<Type, Func<object[], JsonConverter>> JsonConverterCreatorCache = new ThreadSafeStore<Type, Func<object[], JsonConverter>>(new Func<Type, Func<object[], JsonConverter>>(JsonTypeReflector.GetJsonConverterCreator));

		// Token: 0x0400036A RID: 874
		private static readonly ThreadSafeStore<Type, Type> AssociatedMetadataTypesCache = new ThreadSafeStore<Type, Type>(new Func<Type, Type>(JsonTypeReflector.GetAssociateMetadataTypeFromAttribute));

		// Token: 0x0400036B RID: 875
		private static ReflectionObject _metadataTypeAttributeReflectionObject;
	}
}
