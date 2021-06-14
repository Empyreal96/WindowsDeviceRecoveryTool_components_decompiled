using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000B1 RID: 177
	public class JsonArrayContract : JsonContainerContract
	{
		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x0002106F File Offset: 0x0001F26F
		// (set) Token: 0x060008A9 RID: 2217 RVA: 0x00021077 File Offset: 0x0001F277
		public Type CollectionItemType { get; private set; }

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x060008AA RID: 2218 RVA: 0x00021080 File Offset: 0x0001F280
		// (set) Token: 0x060008AB RID: 2219 RVA: 0x00021088 File Offset: 0x0001F288
		public bool IsMultidimensionalArray { get; private set; }

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060008AC RID: 2220 RVA: 0x00021091 File Offset: 0x0001F291
		// (set) Token: 0x060008AD RID: 2221 RVA: 0x00021099 File Offset: 0x0001F299
		internal bool IsArray { get; private set; }

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x000210A2 File Offset: 0x0001F2A2
		// (set) Token: 0x060008AF RID: 2223 RVA: 0x000210AA File Offset: 0x0001F2AA
		internal bool ShouldCreateWrapper { get; private set; }

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x000210B3 File Offset: 0x0001F2B3
		// (set) Token: 0x060008B1 RID: 2225 RVA: 0x000210BB File Offset: 0x0001F2BB
		internal bool CanDeserialize { get; private set; }

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060008B2 RID: 2226 RVA: 0x000210C4 File Offset: 0x0001F2C4
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

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060008B3 RID: 2227 RVA: 0x000210EA File Offset: 0x0001F2EA
		internal bool HasParametrizedCreator
		{
			get
			{
				return this._parametrizedCreator != null || this._parametrizedConstructor != null;
			}
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x00021104 File Offset: 0x0001F304
		public JsonArrayContract(Type underlyingType) : base(underlyingType)
		{
			this.ContractType = JsonContractType.Array;
			this.IsArray = base.CreatedType.IsArray;
			bool canDeserialize;
			Type type;
			if (this.IsArray)
			{
				this.CollectionItemType = ReflectionUtils.GetCollectionItemType(base.UnderlyingType);
				this.IsReadOnlyOrFixedSize = true;
				this._genericCollectionDefinitionType = typeof(List<>).MakeGenericType(new Type[]
				{
					this.CollectionItemType
				});
				canDeserialize = true;
				this.IsMultidimensionalArray = (this.IsArray && base.UnderlyingType.GetArrayRank() > 1);
			}
			else if (typeof(IList).IsAssignableFrom(underlyingType))
			{
				if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof(ICollection<>), out this._genericCollectionDefinitionType))
				{
					this.CollectionItemType = this._genericCollectionDefinitionType.GetGenericArguments()[0];
				}
				else
				{
					this.CollectionItemType = ReflectionUtils.GetCollectionItemType(underlyingType);
				}
				if (underlyingType == typeof(IList))
				{
					base.CreatedType = typeof(List<object>);
				}
				if (this.CollectionItemType != null)
				{
					this._parametrizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(underlyingType, this.CollectionItemType);
				}
				this.IsReadOnlyOrFixedSize = ReflectionUtils.InheritsGenericDefinition(underlyingType, typeof(ReadOnlyCollection<>));
				canDeserialize = true;
			}
			else if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof(ICollection<>), out this._genericCollectionDefinitionType))
			{
				this.CollectionItemType = this._genericCollectionDefinitionType.GetGenericArguments()[0];
				if (ReflectionUtils.IsGenericDefinition(underlyingType, typeof(ICollection<>)) || ReflectionUtils.IsGenericDefinition(underlyingType, typeof(IList<>)))
				{
					base.CreatedType = typeof(List<>).MakeGenericType(new Type[]
					{
						this.CollectionItemType
					});
				}
				if (ReflectionUtils.IsGenericDefinition(underlyingType, typeof(ISet<>)))
				{
					base.CreatedType = typeof(HashSet<>).MakeGenericType(new Type[]
					{
						this.CollectionItemType
					});
				}
				this._parametrizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(underlyingType, this.CollectionItemType);
				canDeserialize = true;
				this.ShouldCreateWrapper = true;
			}
			else if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof(IReadOnlyCollection<>), out type))
			{
				this.CollectionItemType = type.GetGenericArguments()[0];
				if (ReflectionUtils.IsGenericDefinition(underlyingType, typeof(IReadOnlyCollection<>)) || ReflectionUtils.IsGenericDefinition(underlyingType, typeof(IReadOnlyList<>)))
				{
					base.CreatedType = typeof(ReadOnlyCollection<>).MakeGenericType(new Type[]
					{
						this.CollectionItemType
					});
				}
				this._genericCollectionDefinitionType = typeof(List<>).MakeGenericType(new Type[]
				{
					this.CollectionItemType
				});
				this._parametrizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(base.CreatedType, this.CollectionItemType);
				this.IsReadOnlyOrFixedSize = true;
				canDeserialize = this.HasParametrizedCreator;
			}
			else if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof(IEnumerable<>), out type))
			{
				this.CollectionItemType = type.GetGenericArguments()[0];
				if (ReflectionUtils.IsGenericDefinition(base.UnderlyingType, typeof(IEnumerable<>)))
				{
					base.CreatedType = typeof(List<>).MakeGenericType(new Type[]
					{
						this.CollectionItemType
					});
				}
				this._parametrizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(underlyingType, this.CollectionItemType);
				if (!this.HasParametrizedCreator && underlyingType.Name == "FSharpList`1")
				{
					FSharpUtils.EnsureInitialized(underlyingType.Assembly());
					this._parametrizedCreator = FSharpUtils.CreateSeq(this.CollectionItemType);
				}
				if (underlyingType.IsGenericType() && underlyingType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
				{
					this._genericCollectionDefinitionType = type;
					this.IsReadOnlyOrFixedSize = false;
					this.ShouldCreateWrapper = false;
					canDeserialize = true;
				}
				else
				{
					this._genericCollectionDefinitionType = typeof(List<>).MakeGenericType(new Type[]
					{
						this.CollectionItemType
					});
					this.IsReadOnlyOrFixedSize = true;
					this.ShouldCreateWrapper = true;
					canDeserialize = this.HasParametrizedCreator;
				}
			}
			else
			{
				canDeserialize = false;
				this.ShouldCreateWrapper = true;
			}
			this.CanDeserialize = canDeserialize;
			Type createdType;
			ObjectConstructor<object> parametrizedCreator;
			if (ImmutableCollectionsUtils.TryBuildImmutableForArrayContract(underlyingType, this.CollectionItemType, out createdType, out parametrizedCreator))
			{
				base.CreatedType = createdType;
				this._parametrizedCreator = parametrizedCreator;
				this.IsReadOnlyOrFixedSize = true;
				this.CanDeserialize = true;
			}
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x0002154C File Offset: 0x0001F74C
		internal IWrappedCollection CreateWrapper(object list)
		{
			if (this._genericWrapperCreator == null)
			{
				this._genericWrapperType = typeof(CollectionWrapper<>).MakeGenericType(new Type[]
				{
					this.CollectionItemType
				});
				Type type;
				if (ReflectionUtils.InheritsGenericDefinition(this._genericCollectionDefinitionType, typeof(List<>)) || this._genericCollectionDefinitionType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
				{
					type = typeof(ICollection<>).MakeGenericType(new Type[]
					{
						this.CollectionItemType
					});
				}
				else
				{
					type = this._genericCollectionDefinitionType;
				}
				ConstructorInfo constructor = this._genericWrapperType.GetConstructor(new Type[]
				{
					type
				});
				this._genericWrapperCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor(constructor);
			}
			return (IWrappedCollection)this._genericWrapperCreator(new object[]
			{
				list
			});
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x00021634 File Offset: 0x0001F834
		internal IList CreateTemporaryCollection()
		{
			if (this._genericTemporaryCollectionCreator == null)
			{
				Type type = this.IsMultidimensionalArray ? typeof(object) : this.CollectionItemType;
				Type type2 = typeof(List<>).MakeGenericType(new Type[]
				{
					type
				});
				this._genericTemporaryCollectionCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(type2);
			}
			return (IList)this._genericTemporaryCollectionCreator();
		}

		// Token: 0x040002F4 RID: 756
		private readonly Type _genericCollectionDefinitionType;

		// Token: 0x040002F5 RID: 757
		private Type _genericWrapperType;

		// Token: 0x040002F6 RID: 758
		private ObjectConstructor<object> _genericWrapperCreator;

		// Token: 0x040002F7 RID: 759
		private Func<object> _genericTemporaryCollectionCreator;

		// Token: 0x040002F8 RID: 760
		private readonly ConstructorInfo _parametrizedConstructor;

		// Token: 0x040002F9 RID: 761
		private ObjectConstructor<object> _parametrizedCreator;
	}
}
