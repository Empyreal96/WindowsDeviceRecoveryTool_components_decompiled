using System;
using System.Collections.Generic;
using System.Data.Services.Client.Metadata;
using Microsoft.Data.Edm;
using Microsoft.Data.OData;

namespace System.Data.Services.Client.Materialization
{
	// Token: 0x02000072 RID: 114
	internal sealed class ODataPropertyMaterializer : ODataMessageReaderMaterializer
	{
		// Token: 0x060003C6 RID: 966 RVA: 0x00010238 File Offset: 0x0000E438
		public ODataPropertyMaterializer(ODataMessageReader reader, IODataMaterializerContext materializerContext, Type expectedType, bool? singleResult) : base(reader, materializerContext, expectedType, singleResult)
		{
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x00010245 File Offset: 0x0000E445
		internal override object CurrentValue
		{
			get
			{
				return this.currentValue;
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00010250 File Offset: 0x0000E450
		protected override void ReadWithExpectedType(IEdmTypeReference expectedClientType, IEdmTypeReference expectedReaderType)
		{
			ODataProperty odataProperty = this.messageReader.ReadProperty(expectedReaderType);
			Type type = Nullable.GetUnderlyingType(base.ExpectedType) ?? base.ExpectedType;
			object value = odataProperty.Value;
			if (expectedClientType.IsCollection())
			{
				Type type2 = type;
				Type type3 = ClientTypeUtil.GetImplementationType(type, typeof(ICollection<>));
				object collectionInstance;
				if (type3 != null)
				{
					type2 = type3.GetGenericArguments()[0];
					collectionInstance = base.CollectionValueMaterializationPolicy.CreateCollectionPropertyInstance(odataProperty, type);
				}
				else
				{
					type3 = typeof(ICollection<>).MakeGenericType(new Type[]
					{
						type2
					});
					collectionInstance = base.CollectionValueMaterializationPolicy.CreateCollectionPropertyInstance(odataProperty, type3);
				}
				base.CollectionValueMaterializationPolicy.ApplyCollectionDataValues(odataProperty, collectionInstance, type2, ClientTypeUtil.GetAddToCollectionDelegate(type3));
				this.currentValue = collectionInstance;
				return;
			}
			if (expectedClientType.IsComplex())
			{
				ODataComplexValue complexValue = value as ODataComplexValue;
				base.ComplexValueMaterializationPolicy.MaterializeComplexTypeProperty(type, complexValue);
				this.currentValue = complexValue.GetMaterializedValue();
				return;
			}
			this.currentValue = base.PrimitivePropertyConverter.ConvertPrimitiveValue(odataProperty.Value, base.ExpectedType);
		}

		// Token: 0x040002B5 RID: 693
		private object currentValue;
	}
}
