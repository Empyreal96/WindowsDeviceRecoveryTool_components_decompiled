using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000B7 RID: 183
	public class JsonDictionaryContract : JsonContainerContract
	{
		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060008C7 RID: 2247 RVA: 0x000216A1 File Offset: 0x0001F8A1
		// (set) Token: 0x060008C8 RID: 2248 RVA: 0x000216A9 File Offset: 0x0001F8A9
		public Func<string, string> PropertyNameResolver { get; set; }

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060008C9 RID: 2249 RVA: 0x000216B2 File Offset: 0x0001F8B2
		// (set) Token: 0x060008CA RID: 2250 RVA: 0x000216BA File Offset: 0x0001F8BA
		public Type DictionaryKeyType { get; private set; }

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060008CB RID: 2251 RVA: 0x000216C3 File Offset: 0x0001F8C3
		// (set) Token: 0x060008CC RID: 2252 RVA: 0x000216CB File Offset: 0x0001F8CB
		public Type DictionaryValueType { get; private set; }

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060008CD RID: 2253 RVA: 0x000216D4 File Offset: 0x0001F8D4
		// (set) Token: 0x060008CE RID: 2254 RVA: 0x000216DC File Offset: 0x0001F8DC
		internal JsonContract KeyContract { get; set; }

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060008CF RID: 2255 RVA: 0x000216E5 File Offset: 0x0001F8E5
		// (set) Token: 0x060008D0 RID: 2256 RVA: 0x000216ED File Offset: 0x0001F8ED
		internal bool ShouldCreateWrapper { get; private set; }

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060008D1 RID: 2257 RVA: 0x000216F6 File Offset: 0x0001F8F6
		internal ObjectConstructor<object> ParametrizedCreator
		{
			get
			{
				if (this._parametrizedCreator == null)
				{
					this._parametrizedCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor(this._parametrizedConstructor);
				}
				return this._parametrizedCreator;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060008D2 RID: 2258 RVA: 0x0002171C File Offset: 0x0001F91C
		internal bool HasParametrizedCreator
		{
			get
			{
				return this._parametrizedCreator != null || this._parametrizedConstructor != null;
			}
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x00021734 File Offset: 0x0001F934
		public JsonDictionaryContract(Type underlyingType) : base(underlyingType)
		{
			this.ContractType = JsonContractType.Dictionary;
			Type type;
			Type type2;
			if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof(IDictionary<, >), out this._genericCollectionDefinitionType))
			{
				type = this._genericCollectionDefinitionType.GetGenericArguments()[0];
				type2 = this._genericCollectionDefinitionType.GetGenericArguments()[1];
				if (ReflectionUtils.IsGenericDefinition(base.UnderlyingType, typeof(IDictionary<, >)))
				{
					base.CreatedType = typeof(Dictionary<, >).MakeGenericType(new Type[]
					{
						type,
						type2
					});
				}
				this.IsReadOnlyOrFixedSize = ReflectionUtils.InheritsGenericDefinition(underlyingType, typeof(ReadOnlyDictionary<, >));
			}
			else if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof(IReadOnlyDictionary<, >), out this._genericCollectionDefinitionType))
			{
				type = this._genericCollectionDefinitionType.GetGenericArguments()[0];
				type2 = this._genericCollectionDefinitionType.GetGenericArguments()[1];
				if (ReflectionUtils.IsGenericDefinition(base.UnderlyingType, typeof(IReadOnlyDictionary<, >)))
				{
					base.CreatedType = typeof(ReadOnlyDictionary<, >).MakeGenericType(new Type[]
					{
						type,
						type2
					});
				}
				this.IsReadOnlyOrFixedSize = true;
			}
			else
			{
				ReflectionUtils.GetDictionaryKeyValueTypes(base.UnderlyingType, out type, out type2);
				if (base.UnderlyingType == typeof(IDictionary))
				{
					base.CreatedType = typeof(Dictionary<object, object>);
				}
			}
			if (type != null && type2 != null)
			{
				this._parametrizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(base.CreatedType, typeof(KeyValuePair<, >).MakeGenericType(new Type[]
				{
					type,
					type2
				}));
				if (!this.HasParametrizedCreator && underlyingType.Name == "FSharpMap`2")
				{
					FSharpUtils.EnsureInitialized(underlyingType.Assembly());
					this._parametrizedCreator = FSharpUtils.CreateMap(type, type2);
				}
			}
			this.ShouldCreateWrapper = !typeof(IDictionary).IsAssignableFrom(base.CreatedType);
			this.DictionaryKeyType = type;
			this.DictionaryValueType = type2;
			Type createdType;
			ObjectConstructor<object> parametrizedCreator;
			if (ImmutableCollectionsUtils.TryBuildImmutableForDictionaryContract(underlyingType, this.DictionaryKeyType, this.DictionaryValueType, out createdType, out parametrizedCreator))
			{
				base.CreatedType = createdType;
				this._parametrizedCreator = parametrizedCreator;
				this.IsReadOnlyOrFixedSize = true;
			}
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x00021964 File Offset: 0x0001FB64
		internal IWrappedDictionary CreateWrapper(object dictionary)
		{
			if (this._genericWrapperCreator == null)
			{
				this._genericWrapperType = typeof(DictionaryWrapper<, >).MakeGenericType(new Type[]
				{
					this.DictionaryKeyType,
					this.DictionaryValueType
				});
				ConstructorInfo constructor = this._genericWrapperType.GetConstructor(new Type[]
				{
					this._genericCollectionDefinitionType
				});
				this._genericWrapperCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor(constructor);
			}
			return (IWrappedDictionary)this._genericWrapperCreator(new object[]
			{
				dictionary
			});
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x000219F4 File Offset: 0x0001FBF4
		internal IDictionary CreateTemporaryDictionary()
		{
			if (this._genericTemporaryDictionaryCreator == null)
			{
				Type type = typeof(Dictionary<, >).MakeGenericType(new Type[]
				{
					this.DictionaryKeyType,
					this.DictionaryValueType
				});
				this._genericTemporaryDictionaryCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(type);
			}
			return (IDictionary)this._genericTemporaryDictionaryCreator();
		}

		// Token: 0x04000309 RID: 777
		private readonly Type _genericCollectionDefinitionType;

		// Token: 0x0400030A RID: 778
		private Type _genericWrapperType;

		// Token: 0x0400030B RID: 779
		private ObjectConstructor<object> _genericWrapperCreator;

		// Token: 0x0400030C RID: 780
		private Func<object> _genericTemporaryDictionaryCreator;

		// Token: 0x0400030D RID: 781
		private readonly ConstructorInfo _parametrizedConstructor;

		// Token: 0x0400030E RID: 782
		private ObjectConstructor<object> _parametrizedCreator;
	}
}
