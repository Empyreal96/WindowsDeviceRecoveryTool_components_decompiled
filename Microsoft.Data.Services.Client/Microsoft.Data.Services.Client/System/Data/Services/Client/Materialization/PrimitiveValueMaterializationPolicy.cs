using System;
using System.Data.Services.Client.Metadata;

namespace System.Data.Services.Client.Materialization
{
	// Token: 0x02000049 RID: 73
	internal class PrimitiveValueMaterializationPolicy
	{
		// Token: 0x06000254 RID: 596 RVA: 0x0000C6D8 File Offset: 0x0000A8D8
		internal PrimitiveValueMaterializationPolicy(IODataMaterializerContext context, SimpleLazy<PrimitivePropertyConverter> lazyPrimitivePropertyConverter)
		{
			this.context = context;
			this.lazyPrimitivePropertyConverter = lazyPrimitivePropertyConverter;
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000255 RID: 597 RVA: 0x0000C6EE File Offset: 0x0000A8EE
		private PrimitivePropertyConverter PrimitivePropertyConverter
		{
			get
			{
				return this.lazyPrimitivePropertyConverter.Value;
			}
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000C704 File Offset: 0x0000A904
		public object MaterializePrimitiveDataValue(Type collectionItemType, string wireTypeName, object item)
		{
			object result = null;
			this.MaterializePrimitiveDataValue(collectionItemType, wireTypeName, item, () => "TODO: Is this reachable?", out result);
			return result;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000C744 File Offset: 0x0000A944
		public object MaterializePrimitiveDataValueCollectionElement(Type collectionItemType, string wireTypeName, object item)
		{
			object result = null;
			this.MaterializePrimitiveDataValue(collectionItemType, wireTypeName, item, () => Strings.Collection_NullCollectionItemsNotSupported, out result);
			return result;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000C780 File Offset: 0x0000A980
		private bool MaterializePrimitiveDataValue(Type type, string wireTypeName, object value, Func<string> throwOnNullMessage, out object materializedValue)
		{
			Type type2 = Nullable.GetUnderlyingType(type) ?? type;
			PrimitiveType primitiveType;
			bool flag = PrimitiveType.TryGetPrimitiveType(type2, out primitiveType);
			if (!flag)
			{
				ClientTypeAnnotation clientTypeAnnotation = this.context.ResolveTypeForMaterialization(type, wireTypeName);
				flag = PrimitiveType.TryGetPrimitiveType(clientTypeAnnotation.ElementType, out primitiveType);
			}
			if (flag)
			{
				if (value == null)
				{
					if (!ClientTypeUtil.CanAssignNull(type))
					{
						throw new InvalidOperationException(throwOnNullMessage());
					}
					materializedValue = null;
				}
				else
				{
					materializedValue = this.PrimitivePropertyConverter.ConvertPrimitiveValue(value, type2);
				}
				return true;
			}
			materializedValue = null;
			return false;
		}

		// Token: 0x0400023C RID: 572
		private readonly IODataMaterializerContext context;

		// Token: 0x0400023D RID: 573
		private readonly SimpleLazy<PrimitivePropertyConverter> lazyPrimitivePropertyConverter;
	}
}
