using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200009E RID: 158
	public class DefaultContractResolver : IContractResolver
	{
		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x0001E5DC File Offset: 0x0001C7DC
		internal static IContractResolver Instance
		{
			get
			{
				return DefaultContractResolver._instance;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x0001E5E3 File Offset: 0x0001C7E3
		public bool DynamicCodeGeneration
		{
			get
			{
				return JsonTypeReflector.DynamicCodeGeneration;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x0001E5EA File Offset: 0x0001C7EA
		// (set) Token: 0x060007FC RID: 2044 RVA: 0x0001E5F2 File Offset: 0x0001C7F2
		[Obsolete("DefaultMembersSearchFlags is obsolete. To modify the members serialized inherit from DefaultContractResolver and override the GetSerializableMembers method instead.")]
		public BindingFlags DefaultMembersSearchFlags { get; set; }

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060007FD RID: 2045 RVA: 0x0001E5FB File Offset: 0x0001C7FB
		// (set) Token: 0x060007FE RID: 2046 RVA: 0x0001E603 File Offset: 0x0001C803
		public bool SerializeCompilerGeneratedMembers { get; set; }

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x0001E60C File Offset: 0x0001C80C
		// (set) Token: 0x06000800 RID: 2048 RVA: 0x0001E614 File Offset: 0x0001C814
		public bool IgnoreSerializableInterface { get; set; }

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000801 RID: 2049 RVA: 0x0001E61D File Offset: 0x0001C81D
		// (set) Token: 0x06000802 RID: 2050 RVA: 0x0001E625 File Offset: 0x0001C825
		public bool IgnoreSerializableAttribute { get; set; }

		// Token: 0x06000803 RID: 2051 RVA: 0x0001E62E File Offset: 0x0001C82E
		public DefaultContractResolver() : this(false)
		{
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x0001E637 File Offset: 0x0001C837
		public DefaultContractResolver(bool shareCache)
		{
			this.DefaultMembersSearchFlags = (BindingFlags.Instance | BindingFlags.Public);
			this.IgnoreSerializableAttribute = true;
			this._sharedCache = shareCache;
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0001E660 File Offset: 0x0001C860
		internal DefaultContractResolverState GetState()
		{
			if (this._sharedCache)
			{
				return DefaultContractResolver._sharedState;
			}
			return this._instanceState;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0001E678 File Offset: 0x0001C878
		public virtual JsonContract ResolveContract(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			DefaultContractResolverState state = this.GetState();
			ResolverContractKey key = new ResolverContractKey(base.GetType(), type);
			Dictionary<ResolverContractKey, JsonContract> contractCache = state.ContractCache;
			JsonContract jsonContract;
			if (contractCache == null || !contractCache.TryGetValue(key, out jsonContract))
			{
				jsonContract = this.CreateContract(type);
				lock (DefaultContractResolver.TypeContractCacheLock)
				{
					contractCache = state.ContractCache;
					Dictionary<ResolverContractKey, JsonContract> dictionary = (contractCache != null) ? new Dictionary<ResolverContractKey, JsonContract>(contractCache) : new Dictionary<ResolverContractKey, JsonContract>();
					dictionary[key] = jsonContract;
					state.ContractCache = dictionary;
				}
			}
			return jsonContract;
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0001E740 File Offset: 0x0001C940
		protected virtual List<MemberInfo> GetSerializableMembers(Type objectType)
		{
			bool ignoreSerializableAttribute = this.IgnoreSerializableAttribute;
			MemberSerialization objectMemberSerialization = JsonTypeReflector.GetObjectMemberSerialization(objectType, ignoreSerializableAttribute);
			List<MemberInfo> list = (from m in ReflectionUtils.GetFieldsAndProperties(objectType, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
			where !ReflectionUtils.IsIndexedProperty(m)
			select m).ToList<MemberInfo>();
			List<MemberInfo> list2 = new List<MemberInfo>();
			if (objectMemberSerialization != MemberSerialization.Fields)
			{
				DataContractAttribute dataContractAttribute = JsonTypeReflector.GetDataContractAttribute(objectType);
				List<MemberInfo> list3 = (from m in ReflectionUtils.GetFieldsAndProperties(objectType, this.DefaultMembersSearchFlags)
				where !ReflectionUtils.IsIndexedProperty(m)
				select m).ToList<MemberInfo>();
				foreach (MemberInfo memberInfo in list)
				{
					if (this.SerializeCompilerGeneratedMembers || !memberInfo.IsDefined(typeof(CompilerGeneratedAttribute), true))
					{
						if (list3.Contains(memberInfo))
						{
							list2.Add(memberInfo);
						}
						else if (JsonTypeReflector.GetAttribute<JsonPropertyAttribute>(memberInfo) != null)
						{
							list2.Add(memberInfo);
						}
						else if (dataContractAttribute != null && JsonTypeReflector.GetAttribute<DataMemberAttribute>(memberInfo) != null)
						{
							list2.Add(memberInfo);
						}
						else if (objectMemberSerialization == MemberSerialization.Fields && memberInfo.MemberType() == MemberTypes.Field)
						{
							list2.Add(memberInfo);
						}
					}
				}
				Type type;
				if (objectType.AssignableToTypeName("System.Data.Objects.DataClasses.EntityObject", out type))
				{
					list2 = list2.Where(new Func<MemberInfo, bool>(this.ShouldSerializeEntityMember)).ToList<MemberInfo>();
				}
			}
			else
			{
				foreach (MemberInfo memberInfo2 in list)
				{
					FieldInfo fieldInfo = memberInfo2 as FieldInfo;
					if (fieldInfo != null && !fieldInfo.IsStatic)
					{
						list2.Add(memberInfo2);
					}
				}
			}
			return list2;
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0001E90C File Offset: 0x0001CB0C
		private bool ShouldSerializeEntityMember(MemberInfo memberInfo)
		{
			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			return !(propertyInfo != null) || !propertyInfo.PropertyType.IsGenericType() || !(propertyInfo.PropertyType.GetGenericTypeDefinition().FullName == "System.Data.Objects.DataClasses.EntityReference`1");
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0001E958 File Offset: 0x0001CB58
		protected virtual JsonObjectContract CreateObjectContract(Type objectType)
		{
			JsonObjectContract jsonObjectContract = new JsonObjectContract(objectType);
			this.InitializeContract(jsonObjectContract);
			bool ignoreSerializableAttribute = this.IgnoreSerializableAttribute;
			jsonObjectContract.MemberSerialization = JsonTypeReflector.GetObjectMemberSerialization(jsonObjectContract.NonNullableUnderlyingType, ignoreSerializableAttribute);
			jsonObjectContract.Properties.AddRange(this.CreateProperties(jsonObjectContract.NonNullableUnderlyingType, jsonObjectContract.MemberSerialization));
			JsonObjectAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonObjectAttribute>(jsonObjectContract.NonNullableUnderlyingType);
			if (cachedAttribute != null)
			{
				jsonObjectContract.ItemRequired = cachedAttribute._itemRequired;
			}
			if (jsonObjectContract.IsInstantiable)
			{
				ConstructorInfo attributeConstructor = this.GetAttributeConstructor(jsonObjectContract.NonNullableUnderlyingType);
				if (attributeConstructor != null)
				{
					jsonObjectContract.OverrideConstructor = attributeConstructor;
					jsonObjectContract.CreatorParameters.AddRange(this.CreateConstructorParameters(attributeConstructor, jsonObjectContract.Properties));
				}
				else if (jsonObjectContract.MemberSerialization == MemberSerialization.Fields)
				{
					if (JsonTypeReflector.FullyTrusted)
					{
						jsonObjectContract.DefaultCreator = new Func<object>(jsonObjectContract.GetUninitializedObject);
					}
				}
				else if (jsonObjectContract.DefaultCreator == null || jsonObjectContract.DefaultCreatorNonPublic)
				{
					ConstructorInfo parametrizedConstructor = this.GetParametrizedConstructor(jsonObjectContract.NonNullableUnderlyingType);
					if (parametrizedConstructor != null)
					{
						jsonObjectContract.ParametrizedConstructor = parametrizedConstructor;
						jsonObjectContract.CreatorParameters.AddRange(this.CreateConstructorParameters(parametrizedConstructor, jsonObjectContract.Properties));
					}
				}
			}
			MemberInfo extensionDataMemberForType = this.GetExtensionDataMemberForType(jsonObjectContract.NonNullableUnderlyingType);
			if (extensionDataMemberForType != null)
			{
				DefaultContractResolver.SetExtensionDataDelegates(jsonObjectContract, extensionDataMemberForType);
			}
			return jsonObjectContract;
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0001EB74 File Offset: 0x0001CD74
		private MemberInfo GetExtensionDataMemberForType(Type type)
		{
			IEnumerable<MemberInfo> source = this.GetClassHierarchyForType(type).SelectMany(delegate(Type baseType)
			{
				IList<MemberInfo> list = new List<MemberInfo>();
				list.AddRange(baseType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
				list.AddRange(baseType.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
				return list;
			});
			return source.LastOrDefault(delegate(MemberInfo m)
			{
				MemberTypes memberTypes = m.MemberType();
				if (memberTypes != MemberTypes.Property && memberTypes != MemberTypes.Field)
				{
					return false;
				}
				if (!m.IsDefined(typeof(JsonExtensionDataAttribute), false))
				{
					return false;
				}
				Type memberUnderlyingType = ReflectionUtils.GetMemberUnderlyingType(m);
				Type type2;
				if (ReflectionUtils.ImplementsGenericDefinition(memberUnderlyingType, typeof(IDictionary<, >), out type2))
				{
					Type type3 = type2.GetGenericArguments()[0];
					Type type4 = type2.GetGenericArguments()[1];
					if (type3.IsAssignableFrom(typeof(string)) && type4.IsAssignableFrom(typeof(JToken)))
					{
						return true;
					}
				}
				throw new JsonException("Invalid extension data attribute on '{0}'. Member '{1}' type must implement IDictionary<string, JToken>.".FormatWith(CultureInfo.InvariantCulture, DefaultContractResolver.GetClrTypeFullName(m.DeclaringType), m.Name));
			});
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0001EC8C File Offset: 0x0001CE8C
		private static void SetExtensionDataDelegates(JsonObjectContract contract, MemberInfo member)
		{
			JsonExtensionDataAttribute attribute = ReflectionUtils.GetAttribute<JsonExtensionDataAttribute>(member);
			if (attribute == null)
			{
				return;
			}
			Type memberUnderlyingType = ReflectionUtils.GetMemberUnderlyingType(member);
			Type type;
			ReflectionUtils.ImplementsGenericDefinition(memberUnderlyingType, typeof(IDictionary<, >), out type);
			Type type2 = type.GetGenericArguments()[0];
			Type type3 = type.GetGenericArguments()[1];
			bool isJTokenValueType = typeof(JToken).IsAssignableFrom(type3);
			Type type4;
			if (ReflectionUtils.IsGenericDefinition(memberUnderlyingType, typeof(IDictionary<, >)))
			{
				type4 = typeof(Dictionary<, >).MakeGenericType(new Type[]
				{
					type2,
					type3
				});
			}
			else
			{
				type4 = memberUnderlyingType;
			}
			MethodInfo method = memberUnderlyingType.GetMethod("Add", new Type[]
			{
				type2,
				type3
			});
			Func<object, object> getExtensionDataDictionary = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(member);
			Action<object, object> setExtensionDataDictionary = JsonTypeReflector.ReflectionDelegateFactory.CreateSet<object>(member);
			Func<object> createExtensionDataDictionary = JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(type4);
			MethodCall<object, object> setExtensionDataDictionaryValue = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method);
			ExtensionDataSetter extensionDataSetter = delegate(object o, string key, object value)
			{
				object obj = getExtensionDataDictionary(o);
				if (obj == null)
				{
					obj = createExtensionDataDictionary();
					setExtensionDataDictionary(o, obj);
				}
				if (isJTokenValueType && !(value is JToken))
				{
					value = ((value != null) ? JToken.FromObject(value) : JValue.CreateNull());
				}
				setExtensionDataDictionaryValue(obj, new object[]
				{
					key,
					value
				});
			};
			Type type5 = typeof(DefaultContractResolver.DictionaryEnumerator<, >).MakeGenericType(new Type[]
			{
				type2,
				type3
			});
			ConstructorInfo method2 = type5.GetConstructors().First<ConstructorInfo>();
			ObjectConstructor<object> createEnumerableWrapper = JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor(method2);
			ExtensionDataGetter extensionDataGetter = delegate(object o)
			{
				object obj = getExtensionDataDictionary(o);
				if (obj == null)
				{
					return null;
				}
				return (IEnumerable<KeyValuePair<object, object>>)createEnumerableWrapper(new object[]
				{
					obj
				});
			};
			if (attribute.ReadData)
			{
				contract.ExtensionDataSetter = extensionDataSetter;
			}
			if (attribute.WriteData)
			{
				contract.ExtensionDataGetter = extensionDataGetter;
			}
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0001EE34 File Offset: 0x0001D034
		private ConstructorInfo GetAttributeConstructor(Type objectType)
		{
			IList<ConstructorInfo> list = (from c in objectType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
			where c.IsDefined(typeof(JsonConstructorAttribute), true)
			select c).ToList<ConstructorInfo>();
			if (list.Count > 1)
			{
				throw new JsonException("Multiple constructors with the JsonConstructorAttribute.");
			}
			if (list.Count == 1)
			{
				return list[0];
			}
			if (objectType == typeof(Version))
			{
				return objectType.GetConstructor(new Type[]
				{
					typeof(int),
					typeof(int),
					typeof(int),
					typeof(int)
				});
			}
			return null;
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0001EEEC File Offset: 0x0001D0EC
		private ConstructorInfo GetParametrizedConstructor(Type objectType)
		{
			IList<ConstructorInfo> list = objectType.GetConstructors(BindingFlags.Instance | BindingFlags.Public).ToList<ConstructorInfo>();
			if (list.Count == 1)
			{
				return list[0];
			}
			return null;
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x0001EF1C File Offset: 0x0001D11C
		protected virtual IList<JsonProperty> CreateConstructorParameters(ConstructorInfo constructor, JsonPropertyCollection memberProperties)
		{
			ParameterInfo[] parameters = constructor.GetParameters();
			JsonPropertyCollection jsonPropertyCollection = new JsonPropertyCollection(constructor.DeclaringType);
			foreach (ParameterInfo parameterInfo in parameters)
			{
				JsonProperty jsonProperty = (parameterInfo.Name != null) ? memberProperties.GetClosestMatchProperty(parameterInfo.Name) : null;
				if (jsonProperty != null && jsonProperty.PropertyType != parameterInfo.ParameterType)
				{
					jsonProperty = null;
				}
				if (jsonProperty != null || parameterInfo.Name != null)
				{
					JsonProperty jsonProperty2 = this.CreatePropertyFromConstructorParameter(jsonProperty, parameterInfo);
					if (jsonProperty2 != null)
					{
						jsonPropertyCollection.AddProperty(jsonProperty2);
					}
				}
			}
			return jsonPropertyCollection;
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0001EFAC File Offset: 0x0001D1AC
		protected virtual JsonProperty CreatePropertyFromConstructorParameter(JsonProperty matchingMemberProperty, ParameterInfo parameterInfo)
		{
			JsonProperty jsonProperty = new JsonProperty();
			jsonProperty.PropertyType = parameterInfo.ParameterType;
			jsonProperty.AttributeProvider = new ReflectionAttributeProvider(parameterInfo);
			bool flag;
			this.SetPropertySettingsFromAttributes(jsonProperty, parameterInfo, parameterInfo.Name, parameterInfo.Member.DeclaringType, MemberSerialization.OptOut, out flag);
			jsonProperty.Readable = false;
			jsonProperty.Writable = true;
			if (matchingMemberProperty != null)
			{
				jsonProperty.PropertyName = ((jsonProperty.PropertyName != parameterInfo.Name) ? jsonProperty.PropertyName : matchingMemberProperty.PropertyName);
				jsonProperty.Converter = (jsonProperty.Converter ?? matchingMemberProperty.Converter);
				jsonProperty.MemberConverter = (jsonProperty.MemberConverter ?? matchingMemberProperty.MemberConverter);
				if (!jsonProperty._hasExplicitDefaultValue && matchingMemberProperty._hasExplicitDefaultValue)
				{
					jsonProperty.DefaultValue = matchingMemberProperty.DefaultValue;
				}
				JsonProperty jsonProperty2 = jsonProperty;
				Required? required = jsonProperty._required;
				jsonProperty2._required = ((required != null) ? new Required?(required.GetValueOrDefault()) : matchingMemberProperty._required);
				JsonProperty jsonProperty3 = jsonProperty;
				bool? isReference = jsonProperty.IsReference;
				jsonProperty3.IsReference = ((isReference != null) ? new bool?(isReference.GetValueOrDefault()) : matchingMemberProperty.IsReference);
				JsonProperty jsonProperty4 = jsonProperty;
				NullValueHandling? nullValueHandling = jsonProperty.NullValueHandling;
				jsonProperty4.NullValueHandling = ((nullValueHandling != null) ? new NullValueHandling?(nullValueHandling.GetValueOrDefault()) : matchingMemberProperty.NullValueHandling);
				JsonProperty jsonProperty5 = jsonProperty;
				DefaultValueHandling? defaultValueHandling = jsonProperty.DefaultValueHandling;
				jsonProperty5.DefaultValueHandling = ((defaultValueHandling != null) ? new DefaultValueHandling?(defaultValueHandling.GetValueOrDefault()) : matchingMemberProperty.DefaultValueHandling);
				JsonProperty jsonProperty6 = jsonProperty;
				ReferenceLoopHandling? referenceLoopHandling = jsonProperty.ReferenceLoopHandling;
				jsonProperty6.ReferenceLoopHandling = ((referenceLoopHandling != null) ? new ReferenceLoopHandling?(referenceLoopHandling.GetValueOrDefault()) : matchingMemberProperty.ReferenceLoopHandling);
				JsonProperty jsonProperty7 = jsonProperty;
				ObjectCreationHandling? objectCreationHandling = jsonProperty.ObjectCreationHandling;
				jsonProperty7.ObjectCreationHandling = ((objectCreationHandling != null) ? new ObjectCreationHandling?(objectCreationHandling.GetValueOrDefault()) : matchingMemberProperty.ObjectCreationHandling);
				JsonProperty jsonProperty8 = jsonProperty;
				TypeNameHandling? typeNameHandling = jsonProperty.TypeNameHandling;
				jsonProperty8.TypeNameHandling = ((typeNameHandling != null) ? new TypeNameHandling?(typeNameHandling.GetValueOrDefault()) : matchingMemberProperty.TypeNameHandling);
			}
			return jsonProperty;
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x0001F1A2 File Offset: 0x0001D3A2
		protected virtual JsonConverter ResolveContractConverter(Type objectType)
		{
			return JsonTypeReflector.GetJsonConverter(objectType);
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x0001F1AA File Offset: 0x0001D3AA
		private Func<object> GetDefaultCreator(Type createdType)
		{
			return JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(createdType);
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x0001F1B8 File Offset: 0x0001D3B8
		[SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.Runtime.Serialization.DataContractAttribute.#get_IsReference()")]
		private void InitializeContract(JsonContract contract)
		{
			JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(contract.NonNullableUnderlyingType);
			if (cachedAttribute != null)
			{
				contract.IsReference = cachedAttribute._isReference;
			}
			else
			{
				DataContractAttribute dataContractAttribute = JsonTypeReflector.GetDataContractAttribute(contract.NonNullableUnderlyingType);
				if (dataContractAttribute != null && dataContractAttribute.IsReference)
				{
					contract.IsReference = new bool?(true);
				}
			}
			contract.Converter = this.ResolveContractConverter(contract.NonNullableUnderlyingType);
			contract.InternalConverter = JsonSerializer.GetMatchingConverter(DefaultContractResolver.BuiltInConverters, contract.NonNullableUnderlyingType);
			if (contract.IsInstantiable && (ReflectionUtils.HasDefaultConstructor(contract.CreatedType, true) || contract.CreatedType.IsValueType()))
			{
				contract.DefaultCreator = this.GetDefaultCreator(contract.CreatedType);
				contract.DefaultCreatorNonPublic = (!contract.CreatedType.IsValueType() && ReflectionUtils.GetDefaultConstructor(contract.CreatedType) == null);
			}
			this.ResolveCallbackMethods(contract, contract.NonNullableUnderlyingType);
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0001F298 File Offset: 0x0001D498
		private void ResolveCallbackMethods(JsonContract contract, Type t)
		{
			List<SerializationCallback> list;
			List<SerializationCallback> list2;
			List<SerializationCallback> list3;
			List<SerializationCallback> list4;
			List<SerializationErrorCallback> list5;
			this.GetCallbackMethodsForType(t, out list, out list2, out list3, out list4, out list5);
			if (list != null && t.Name != "FSharpSet`1" && t.Name != "FSharpMap`2")
			{
				contract.OnSerializingCallbacks.AddRange(list);
			}
			if (list2 != null)
			{
				contract.OnSerializedCallbacks.AddRange(list2);
			}
			if (list3 != null)
			{
				contract.OnDeserializingCallbacks.AddRange(list3);
			}
			if (list4 != null && t.Name != "FSharpSet`1" && t.Name != "FSharpMap`2" && (!t.IsGenericType() || t.GetGenericTypeDefinition() != typeof(ConcurrentDictionary<, >)))
			{
				contract.OnDeserializedCallbacks.AddRange(list4);
			}
			if (list5 != null)
			{
				contract.OnErrorCallbacks.AddRange(list5);
			}
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0001F36C File Offset: 0x0001D56C
		private void GetCallbackMethodsForType(Type type, out List<SerializationCallback> onSerializing, out List<SerializationCallback> onSerialized, out List<SerializationCallback> onDeserializing, out List<SerializationCallback> onDeserialized, out List<SerializationErrorCallback> onError)
		{
			onSerializing = null;
			onSerialized = null;
			onDeserializing = null;
			onDeserialized = null;
			onError = null;
			foreach (Type type2 in this.GetClassHierarchyForType(type))
			{
				MethodInfo currentCallback = null;
				MethodInfo currentCallback2 = null;
				MethodInfo currentCallback3 = null;
				MethodInfo currentCallback4 = null;
				MethodInfo currentCallback5 = null;
				foreach (MethodInfo methodInfo in type2.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				{
					if (!methodInfo.ContainsGenericParameters)
					{
						Type type3 = null;
						ParameterInfo[] parameters = methodInfo.GetParameters();
						if (DefaultContractResolver.IsValidCallback(methodInfo, parameters, typeof(OnSerializingAttribute), currentCallback, ref type3))
						{
							onSerializing = (onSerializing ?? new List<SerializationCallback>());
							onSerializing.Add(JsonContract.CreateSerializationCallback(methodInfo));
							currentCallback = methodInfo;
						}
						if (DefaultContractResolver.IsValidCallback(methodInfo, parameters, typeof(OnSerializedAttribute), currentCallback2, ref type3))
						{
							onSerialized = (onSerialized ?? new List<SerializationCallback>());
							onSerialized.Add(JsonContract.CreateSerializationCallback(methodInfo));
							currentCallback2 = methodInfo;
						}
						if (DefaultContractResolver.IsValidCallback(methodInfo, parameters, typeof(OnDeserializingAttribute), currentCallback3, ref type3))
						{
							onDeserializing = (onDeserializing ?? new List<SerializationCallback>());
							onDeserializing.Add(JsonContract.CreateSerializationCallback(methodInfo));
							currentCallback3 = methodInfo;
						}
						if (DefaultContractResolver.IsValidCallback(methodInfo, parameters, typeof(OnDeserializedAttribute), currentCallback4, ref type3))
						{
							onDeserialized = (onDeserialized ?? new List<SerializationCallback>());
							onDeserialized.Add(JsonContract.CreateSerializationCallback(methodInfo));
							currentCallback4 = methodInfo;
						}
						if (DefaultContractResolver.IsValidCallback(methodInfo, parameters, typeof(OnErrorAttribute), currentCallback5, ref type3))
						{
							onError = (onError ?? new List<SerializationErrorCallback>());
							onError.Add(JsonContract.CreateSerializationErrorCallback(methodInfo));
							currentCallback5 = methodInfo;
						}
					}
				}
			}
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0001F548 File Offset: 0x0001D748
		private List<Type> GetClassHierarchyForType(Type type)
		{
			List<Type> list = new List<Type>();
			Type type2 = type;
			while (type2 != null && type2 != typeof(object))
			{
				list.Add(type2);
				type2 = type2.BaseType();
			}
			list.Reverse();
			return list;
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0001F590 File Offset: 0x0001D790
		protected virtual JsonDictionaryContract CreateDictionaryContract(Type objectType)
		{
			JsonDictionaryContract jsonDictionaryContract = new JsonDictionaryContract(objectType);
			this.InitializeContract(jsonDictionaryContract);
			jsonDictionaryContract.PropertyNameResolver = new Func<string, string>(this.ResolvePropertyName);
			return jsonDictionaryContract;
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0001F5C0 File Offset: 0x0001D7C0
		protected virtual JsonArrayContract CreateArrayContract(Type objectType)
		{
			JsonArrayContract jsonArrayContract = new JsonArrayContract(objectType);
			this.InitializeContract(jsonArrayContract);
			return jsonArrayContract;
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x0001F5DC File Offset: 0x0001D7DC
		protected virtual JsonPrimitiveContract CreatePrimitiveContract(Type objectType)
		{
			JsonPrimitiveContract jsonPrimitiveContract = new JsonPrimitiveContract(objectType);
			this.InitializeContract(jsonPrimitiveContract);
			return jsonPrimitiveContract;
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x0001F5F8 File Offset: 0x0001D7F8
		protected virtual JsonLinqContract CreateLinqContract(Type objectType)
		{
			JsonLinqContract jsonLinqContract = new JsonLinqContract(objectType);
			this.InitializeContract(jsonLinqContract);
			return jsonLinqContract;
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x0001F614 File Offset: 0x0001D814
		protected virtual JsonISerializableContract CreateISerializableContract(Type objectType)
		{
			JsonISerializableContract jsonISerializableContract = new JsonISerializableContract(objectType);
			this.InitializeContract(jsonISerializableContract);
			ConstructorInfo constructor = jsonISerializableContract.NonNullableUnderlyingType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
			{
				typeof(SerializationInfo),
				typeof(StreamingContext)
			}, null);
			if (constructor != null)
			{
				ObjectConstructor<object> iserializableCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor(constructor);
				jsonISerializableContract.ISerializableCreator = iserializableCreator;
			}
			return jsonISerializableContract;
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x0001F680 File Offset: 0x0001D880
		protected virtual JsonDynamicContract CreateDynamicContract(Type objectType)
		{
			JsonDynamicContract jsonDynamicContract = new JsonDynamicContract(objectType);
			this.InitializeContract(jsonDynamicContract);
			jsonDynamicContract.PropertyNameResolver = new Func<string, string>(this.ResolvePropertyName);
			jsonDynamicContract.Properties.AddRange(this.CreateProperties(objectType, MemberSerialization.OptOut));
			return jsonDynamicContract;
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0001F6C4 File Offset: 0x0001D8C4
		protected virtual JsonStringContract CreateStringContract(Type objectType)
		{
			JsonStringContract jsonStringContract = new JsonStringContract(objectType);
			this.InitializeContract(jsonStringContract);
			return jsonStringContract;
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x0001F6E0 File Offset: 0x0001D8E0
		protected virtual JsonContract CreateContract(Type objectType)
		{
			if (DefaultContractResolver.IsJsonPrimitiveType(objectType))
			{
				return this.CreatePrimitiveContract(objectType);
			}
			Type type = ReflectionUtils.EnsureNotNullableType(objectType);
			JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(type);
			if (cachedAttribute is JsonObjectAttribute)
			{
				return this.CreateObjectContract(objectType);
			}
			if (cachedAttribute is JsonArrayAttribute)
			{
				return this.CreateArrayContract(objectType);
			}
			if (cachedAttribute is JsonDictionaryAttribute)
			{
				return this.CreateDictionaryContract(objectType);
			}
			if (type == typeof(JToken) || type.IsSubclassOf(typeof(JToken)))
			{
				return this.CreateLinqContract(objectType);
			}
			if (CollectionUtils.IsDictionaryType(type))
			{
				return this.CreateDictionaryContract(objectType);
			}
			if (typeof(IEnumerable).IsAssignableFrom(type))
			{
				return this.CreateArrayContract(objectType);
			}
			if (DefaultContractResolver.CanConvertToString(type))
			{
				return this.CreateStringContract(objectType);
			}
			if (!this.IgnoreSerializableInterface && typeof(ISerializable).IsAssignableFrom(type))
			{
				return this.CreateISerializableContract(objectType);
			}
			if (typeof(IDynamicMetaObjectProvider).IsAssignableFrom(type))
			{
				return this.CreateDynamicContract(objectType);
			}
			if (DefaultContractResolver.IsIConvertible(type))
			{
				return this.CreatePrimitiveContract(type);
			}
			return this.CreateObjectContract(objectType);
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x0001F7F4 File Offset: 0x0001D9F4
		internal static bool IsJsonPrimitiveType(Type t)
		{
			PrimitiveTypeCode typeCode = ConvertUtils.GetTypeCode(t);
			return typeCode != PrimitiveTypeCode.Empty && typeCode != PrimitiveTypeCode.Object;
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x0001F814 File Offset: 0x0001DA14
		internal static bool IsIConvertible(Type t)
		{
			return (typeof(IConvertible).IsAssignableFrom(t) || (ReflectionUtils.IsNullableType(t) && typeof(IConvertible).IsAssignableFrom(Nullable.GetUnderlyingType(t)))) && !typeof(JToken).IsAssignableFrom(t);
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x0001F868 File Offset: 0x0001DA68
		internal static bool CanConvertToString(Type type)
		{
			TypeConverter converter = ConvertUtils.GetConverter(type);
			return (converter != null && !(converter is ComponentConverter) && !(converter is ReferenceConverter) && converter.GetType() != typeof(TypeConverter) && converter.CanConvertTo(typeof(string))) || (type == typeof(Type) || type.IsSubclassOf(typeof(Type)));
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x0001F8E4 File Offset: 0x0001DAE4
		private static bool IsValidCallback(MethodInfo method, ParameterInfo[] parameters, Type attributeType, MethodInfo currentCallback, ref Type prevAttributeType)
		{
			if (!method.IsDefined(attributeType, false))
			{
				return false;
			}
			if (currentCallback != null)
			{
				throw new JsonException("Invalid attribute. Both '{0}' and '{1}' in type '{2}' have '{3}'.".FormatWith(CultureInfo.InvariantCulture, method, currentCallback, DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), attributeType));
			}
			if (prevAttributeType != null)
			{
				throw new JsonException("Invalid Callback. Method '{3}' in type '{2}' has both '{0}' and '{1}'.".FormatWith(CultureInfo.InvariantCulture, prevAttributeType, attributeType, DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), method));
			}
			if (method.IsVirtual)
			{
				throw new JsonException("Virtual Method '{0}' of type '{1}' cannot be marked with '{2}' attribute.".FormatWith(CultureInfo.InvariantCulture, method, DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), attributeType));
			}
			if (method.ReturnType != typeof(void))
			{
				throw new JsonException("Serialization Callback '{1}' in type '{0}' must return void.".FormatWith(CultureInfo.InvariantCulture, DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), method));
			}
			if (attributeType == typeof(OnErrorAttribute))
			{
				if (parameters == null || parameters.Length != 2 || parameters[0].ParameterType != typeof(StreamingContext) || parameters[1].ParameterType != typeof(ErrorContext))
				{
					throw new JsonException("Serialization Error Callback '{1}' in type '{0}' must have two parameters of type '{2}' and '{3}'.".FormatWith(CultureInfo.InvariantCulture, DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), method, typeof(StreamingContext), typeof(ErrorContext)));
				}
			}
			else if (parameters == null || parameters.Length != 1 || parameters[0].ParameterType != typeof(StreamingContext))
			{
				throw new JsonException("Serialization Callback '{1}' in type '{0}' must have a single parameter of type '{2}'.".FormatWith(CultureInfo.InvariantCulture, DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), method, typeof(StreamingContext)));
			}
			prevAttributeType = attributeType;
			return true;
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0001FA94 File Offset: 0x0001DC94
		internal static string GetClrTypeFullName(Type type)
		{
			if (type.IsGenericTypeDefinition() || !type.ContainsGenericParameters())
			{
				return type.FullName;
			}
			return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", new object[]
			{
				type.Namespace,
				type.Name
			});
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0001FB0C File Offset: 0x0001DD0C
		protected virtual IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
		{
			List<MemberInfo> serializableMembers = this.GetSerializableMembers(type);
			if (serializableMembers == null)
			{
				throw new JsonSerializationException("Null collection of seralizable members returned.");
			}
			JsonPropertyCollection jsonPropertyCollection = new JsonPropertyCollection(type);
			foreach (MemberInfo member in serializableMembers)
			{
				JsonProperty jsonProperty = this.CreateProperty(member, memberSerialization);
				if (jsonProperty != null)
				{
					DefaultContractResolverState state = this.GetState();
					lock (state.NameTable)
					{
						jsonProperty.PropertyName = state.NameTable.Add(jsonProperty.PropertyName);
					}
					jsonPropertyCollection.AddProperty(jsonProperty);
				}
			}
			return jsonPropertyCollection.OrderBy(delegate(JsonProperty p)
			{
				int? order = p.Order;
				if (order == null)
				{
					return -1;
				}
				return order.GetValueOrDefault();
			}).ToList<JsonProperty>();
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x0001FC00 File Offset: 0x0001DE00
		protected virtual IValueProvider CreateMemberValueProvider(MemberInfo member)
		{
			IValueProvider result;
			if (this.DynamicCodeGeneration)
			{
				result = new DynamicValueProvider(member);
			}
			else
			{
				result = new ReflectionValueProvider(member);
			}
			return result;
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x0001FC28 File Offset: 0x0001DE28
		protected virtual JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
		{
			JsonProperty jsonProperty = new JsonProperty();
			jsonProperty.PropertyType = ReflectionUtils.GetMemberUnderlyingType(member);
			jsonProperty.DeclaringType = member.DeclaringType;
			jsonProperty.ValueProvider = this.CreateMemberValueProvider(member);
			jsonProperty.AttributeProvider = new ReflectionAttributeProvider(member);
			bool flag;
			this.SetPropertySettingsFromAttributes(jsonProperty, member, member.Name, member.DeclaringType, memberSerialization, out flag);
			if (memberSerialization != MemberSerialization.Fields)
			{
				jsonProperty.Readable = ReflectionUtils.CanReadMemberValue(member, flag);
				jsonProperty.Writable = ReflectionUtils.CanSetMemberValue(member, flag, jsonProperty.HasMemberAttribute);
			}
			else
			{
				jsonProperty.Readable = true;
				jsonProperty.Writable = true;
			}
			jsonProperty.ShouldSerialize = this.CreateShouldSerializeTest(member);
			this.SetIsSpecifiedActions(jsonProperty, member, flag);
			return jsonProperty;
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x0001FCD0 File Offset: 0x0001DED0
		private void SetPropertySettingsFromAttributes(JsonProperty property, object attributeProvider, string name, Type declaringType, MemberSerialization memberSerialization, out bool allowNonPublicAccess)
		{
			DataContractAttribute dataContractAttribute = JsonTypeReflector.GetDataContractAttribute(declaringType);
			MemberInfo memberInfo = attributeProvider as MemberInfo;
			DataMemberAttribute dataMemberAttribute;
			if (dataContractAttribute != null && memberInfo != null)
			{
				dataMemberAttribute = JsonTypeReflector.GetDataMemberAttribute(memberInfo);
			}
			else
			{
				dataMemberAttribute = null;
			}
			JsonPropertyAttribute attribute = JsonTypeReflector.GetAttribute<JsonPropertyAttribute>(attributeProvider);
			if (attribute != null)
			{
				property.HasMemberAttribute = true;
			}
			string propertyName;
			if (attribute != null && attribute.PropertyName != null)
			{
				propertyName = attribute.PropertyName;
			}
			else if (dataMemberAttribute != null && dataMemberAttribute.Name != null)
			{
				propertyName = dataMemberAttribute.Name;
			}
			else
			{
				propertyName = name;
			}
			property.PropertyName = this.ResolvePropertyName(propertyName);
			property.UnderlyingName = name;
			bool flag = false;
			if (attribute != null)
			{
				property._required = attribute._required;
				property.Order = attribute._order;
				property.DefaultValueHandling = attribute._defaultValueHandling;
				flag = true;
			}
			else if (dataMemberAttribute != null)
			{
				property._required = new Required?(dataMemberAttribute.IsRequired ? Required.AllowNull : Required.Default);
				property.Order = ((dataMemberAttribute.Order != -1) ? new int?(dataMemberAttribute.Order) : null);
				property.DefaultValueHandling = ((!dataMemberAttribute.EmitDefaultValue) ? new DefaultValueHandling?(DefaultValueHandling.Ignore) : null);
				flag = true;
			}
			bool flag2 = JsonTypeReflector.GetAttribute<JsonIgnoreAttribute>(attributeProvider) != null || JsonTypeReflector.GetAttribute<JsonExtensionDataAttribute>(attributeProvider) != null || JsonTypeReflector.GetAttribute<NonSerializedAttribute>(attributeProvider) != null;
			if (memberSerialization != MemberSerialization.OptIn)
			{
				bool flag3 = JsonTypeReflector.GetAttribute<IgnoreDataMemberAttribute>(attributeProvider) != null;
				property.Ignored = (flag2 || flag3);
			}
			else
			{
				property.Ignored = (flag2 || !flag);
			}
			property.Converter = JsonTypeReflector.GetJsonConverter(attributeProvider);
			property.MemberConverter = JsonTypeReflector.GetJsonConverter(attributeProvider);
			DefaultValueAttribute attribute2 = JsonTypeReflector.GetAttribute<DefaultValueAttribute>(attributeProvider);
			if (attribute2 != null)
			{
				property.DefaultValue = attribute2.Value;
			}
			property.NullValueHandling = ((attribute != null) ? attribute._nullValueHandling : null);
			property.ReferenceLoopHandling = ((attribute != null) ? attribute._referenceLoopHandling : null);
			property.ObjectCreationHandling = ((attribute != null) ? attribute._objectCreationHandling : null);
			property.TypeNameHandling = ((attribute != null) ? attribute._typeNameHandling : null);
			property.IsReference = ((attribute != null) ? attribute._isReference : null);
			property.ItemIsReference = ((attribute != null) ? attribute._itemIsReference : null);
			property.ItemConverter = ((attribute != null && attribute.ItemConverterType != null) ? JsonTypeReflector.CreateJsonConverterInstance(attribute.ItemConverterType, attribute.ItemConverterParameters) : null);
			property.ItemReferenceLoopHandling = ((attribute != null) ? attribute._itemReferenceLoopHandling : null);
			property.ItemTypeNameHandling = ((attribute != null) ? attribute._itemTypeNameHandling : null);
			allowNonPublicAccess = false;
			if ((this.DefaultMembersSearchFlags & BindingFlags.NonPublic) == BindingFlags.NonPublic)
			{
				allowNonPublicAccess = true;
			}
			if (attribute != null)
			{
				allowNonPublicAccess = true;
			}
			if (memberSerialization == MemberSerialization.Fields)
			{
				allowNonPublicAccess = true;
			}
			if (dataMemberAttribute != null)
			{
				allowNonPublicAccess = true;
				property.HasMemberAttribute = true;
			}
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x0001FFCC File Offset: 0x0001E1CC
		private Predicate<object> CreateShouldSerializeTest(MemberInfo member)
		{
			MethodInfo method = member.DeclaringType.GetMethod("ShouldSerialize" + member.Name, ReflectionUtils.EmptyTypes);
			if (method == null || method.ReturnType != typeof(bool))
			{
				return null;
			}
			MethodCall<object, object> shouldSerializeCall = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method);
			return (object o) => (bool)shouldSerializeCall(o, new object[0]);
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x0002005C File Offset: 0x0001E25C
		private void SetIsSpecifiedActions(JsonProperty property, MemberInfo member, bool allowNonPublicAccess)
		{
			MemberInfo memberInfo = member.DeclaringType.GetProperty(member.Name + "Specified");
			if (memberInfo == null)
			{
				memberInfo = member.DeclaringType.GetField(member.Name + "Specified");
			}
			if (memberInfo == null || ReflectionUtils.GetMemberUnderlyingType(memberInfo) != typeof(bool))
			{
				return;
			}
			Func<object, object> specifiedPropertyGet = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(memberInfo);
			property.GetIsSpecified = ((object o) => (bool)specifiedPropertyGet(o));
			if (ReflectionUtils.CanSetMemberValue(memberInfo, allowNonPublicAccess, false))
			{
				property.SetIsSpecified = JsonTypeReflector.ReflectionDelegateFactory.CreateSet<object>(memberInfo);
			}
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x0002010F File Offset: 0x0001E30F
		protected internal virtual string ResolvePropertyName(string propertyName)
		{
			return propertyName;
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x00020112 File Offset: 0x0001E312
		public string GetResolvedPropertyName(string propertyName)
		{
			return this.ResolvePropertyName(propertyName);
		}

		// Token: 0x040002B0 RID: 688
		private static readonly IContractResolver _instance = new DefaultContractResolver(true);

		// Token: 0x040002B1 RID: 689
		private static readonly JsonConverter[] BuiltInConverters = new JsonConverter[]
		{
			new EntityKeyMemberConverter(),
			new ExpandoObjectConverter(),
			new XmlNodeConverter(),
			new BinaryConverter(),
			new DataSetConverter(),
			new DataTableConverter(),
			new DiscriminatedUnionConverter(),
			new KeyValuePairConverter(),
			new BsonObjectIdConverter(),
			new RegexConverter()
		};

		// Token: 0x040002B2 RID: 690
		private static readonly object TypeContractCacheLock = new object();

		// Token: 0x040002B3 RID: 691
		private static readonly DefaultContractResolverState _sharedState = new DefaultContractResolverState();

		// Token: 0x040002B4 RID: 692
		private readonly DefaultContractResolverState _instanceState = new DefaultContractResolverState();

		// Token: 0x040002B5 RID: 693
		private readonly bool _sharedCache;

		// Token: 0x0200009F RID: 159
		internal struct DictionaryEnumerator<TEnumeratorKey, TEnumeratorValue> : IEnumerable<KeyValuePair<object, object>>, IEnumerable, IEnumerator<KeyValuePair<object, object>>, IDisposable, IEnumerator
		{
			// Token: 0x06000832 RID: 2098 RVA: 0x000201A7 File Offset: 0x0001E3A7
			public DictionaryEnumerator(IEnumerable<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> e)
			{
				ValidationUtils.ArgumentNotNull(e, "e");
				this._e = e.GetEnumerator();
			}

			// Token: 0x06000833 RID: 2099 RVA: 0x000201C0 File Offset: 0x0001E3C0
			public bool MoveNext()
			{
				return this._e.MoveNext();
			}

			// Token: 0x06000834 RID: 2100 RVA: 0x000201CD File Offset: 0x0001E3CD
			public void Reset()
			{
				this._e.Reset();
			}

			// Token: 0x170001B4 RID: 436
			// (get) Token: 0x06000835 RID: 2101 RVA: 0x000201DC File Offset: 0x0001E3DC
			public KeyValuePair<object, object> Current
			{
				get
				{
					KeyValuePair<TEnumeratorKey, TEnumeratorValue> keyValuePair = this._e.Current;
					object key = keyValuePair.Key;
					KeyValuePair<TEnumeratorKey, TEnumeratorValue> keyValuePair2 = this._e.Current;
					return new KeyValuePair<object, object>(key, keyValuePair2.Value);
				}
			}

			// Token: 0x06000836 RID: 2102 RVA: 0x0002021E File Offset: 0x0001E41E
			public void Dispose()
			{
				this._e.Dispose();
			}

			// Token: 0x170001B5 RID: 437
			// (get) Token: 0x06000837 RID: 2103 RVA: 0x0002022B File Offset: 0x0001E42B
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06000838 RID: 2104 RVA: 0x00020238 File Offset: 0x0001E438
			public IEnumerator<KeyValuePair<object, object>> GetEnumerator()
			{
				return this;
			}

			// Token: 0x06000839 RID: 2105 RVA: 0x00020245 File Offset: 0x0001E445
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this;
			}

			// Token: 0x040002C0 RID: 704
			private readonly IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> _e;
		}
	}
}
