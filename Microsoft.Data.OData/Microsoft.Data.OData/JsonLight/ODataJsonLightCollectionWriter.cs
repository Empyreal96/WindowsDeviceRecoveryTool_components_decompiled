using System;
using System.Threading.Tasks;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x02000188 RID: 392
	internal sealed class ODataJsonLightCollectionWriter : ODataCollectionWriterCore
	{
		// Token: 0x06000B15 RID: 2837 RVA: 0x00024C76 File Offset: 0x00022E76
		internal ODataJsonLightCollectionWriter(ODataJsonLightOutputContext jsonLightOutputContext, IEdmTypeReference itemTypeReference) : base(jsonLightOutputContext, itemTypeReference)
		{
			this.jsonLightOutputContext = jsonLightOutputContext;
			this.jsonLightCollectionSerializer = new ODataJsonLightCollectionSerializer(this.jsonLightOutputContext, true);
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x00024C99 File Offset: 0x00022E99
		internal ODataJsonLightCollectionWriter(ODataJsonLightOutputContext jsonLightOutputContext, IEdmTypeReference expectedItemType, IODataReaderWriterListener listener) : base(jsonLightOutputContext, expectedItemType, listener)
		{
			this.jsonLightOutputContext = jsonLightOutputContext;
			this.jsonLightCollectionSerializer = new ODataJsonLightCollectionSerializer(this.jsonLightOutputContext, false);
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x00024CBD File Offset: 0x00022EBD
		protected override void VerifyNotDisposed()
		{
			this.jsonLightOutputContext.VerifyNotDisposed();
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x00024CCA File Offset: 0x00022ECA
		protected override void FlushSynchronously()
		{
			this.jsonLightOutputContext.Flush();
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x00024CD7 File Offset: 0x00022ED7
		protected override Task FlushAsynchronously()
		{
			return this.jsonLightOutputContext.FlushAsync();
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x00024CE4 File Offset: 0x00022EE4
		protected override void StartPayload()
		{
			this.jsonLightCollectionSerializer.WritePayloadStart();
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x00024CF1 File Offset: 0x00022EF1
		protected override void EndPayload()
		{
			this.jsonLightCollectionSerializer.WritePayloadEnd();
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x00024CFE File Offset: 0x00022EFE
		protected override void StartCollection(ODataCollectionStart collectionStart)
		{
			this.jsonLightCollectionSerializer.WriteCollectionStart(collectionStart, base.ItemTypeReference);
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x00024D12 File Offset: 0x00022F12
		protected override void EndCollection()
		{
			this.jsonLightCollectionSerializer.WriteCollectionEnd();
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x00024D20 File Offset: 0x00022F20
		protected override void WriteCollectionItem(object item, IEdmTypeReference expectedItemType)
		{
			if (item == null)
			{
				ValidationUtils.ValidateNullCollectionItem(expectedItemType, this.jsonLightOutputContext.MessageWriterSettings.WriterBehavior);
				this.jsonLightOutputContext.JsonWriter.WriteValue(null);
				return;
			}
			ODataComplexValue odataComplexValue = item as ODataComplexValue;
			if (odataComplexValue != null)
			{
				this.jsonLightCollectionSerializer.WriteComplexValue(odataComplexValue, expectedItemType, false, false, base.DuplicatePropertyNamesChecker);
				base.DuplicatePropertyNamesChecker.Clear();
				return;
			}
			this.jsonLightCollectionSerializer.WritePrimitiveValue(item, expectedItemType);
		}

		// Token: 0x04000410 RID: 1040
		private readonly ODataJsonLightOutputContext jsonLightOutputContext;

		// Token: 0x04000411 RID: 1041
		private readonly ODataJsonLightCollectionSerializer jsonLightCollectionSerializer;
	}
}
