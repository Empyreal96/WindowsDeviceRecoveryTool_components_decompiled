using System;
using System.Threading.Tasks;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x02000181 RID: 385
	internal sealed class ODataJsonLightCollectionReader : ODataCollectionReaderCoreAsync
	{
		// Token: 0x06000ACE RID: 2766 RVA: 0x0002416B File Offset: 0x0002236B
		internal ODataJsonLightCollectionReader(ODataJsonLightInputContext jsonLightInputContext, IEdmTypeReference expectedItemTypeReference, IODataReaderWriterListener listener) : base(jsonLightInputContext, expectedItemTypeReference, listener)
		{
			this.jsonLightInputContext = jsonLightInputContext;
			this.jsonLightCollectionDeserializer = new ODataJsonLightCollectionDeserializer(jsonLightInputContext);
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x0002418C File Offset: 0x0002238C
		protected override bool ReadAtStartImplementation()
		{
			DuplicatePropertyNamesChecker duplicatePropertyNamesChecker = this.jsonLightInputContext.CreateDuplicatePropertyNamesChecker();
			this.jsonLightCollectionDeserializer.ReadPayloadStart(ODataPayloadKind.Collection, duplicatePropertyNamesChecker, base.IsReadingNestedPayload, false);
			return this.ReadAtStartImplementationSynchronously(duplicatePropertyNamesChecker);
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x000241DC File Offset: 0x000223DC
		protected override Task<bool> ReadAtStartImplementationAsync()
		{
			DuplicatePropertyNamesChecker duplicatePropertyNamesChecker = this.jsonLightInputContext.CreateDuplicatePropertyNamesChecker();
			return this.jsonLightCollectionDeserializer.ReadPayloadStartAsync(ODataPayloadKind.Collection, duplicatePropertyNamesChecker, base.IsReadingNestedPayload, false).FollowOnSuccessWith((Task t) => this.ReadAtStartImplementationSynchronously(duplicatePropertyNamesChecker));
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x00024231 File Offset: 0x00022431
		protected override bool ReadAtCollectionStartImplementation()
		{
			return this.ReadAtCollectionStartImplementationSynchronously();
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x00024239 File Offset: 0x00022439
		protected override Task<bool> ReadAtCollectionStartImplementationAsync()
		{
			return TaskUtils.GetTaskForSynchronousOperation<bool>(new Func<bool>(this.ReadAtCollectionStartImplementationSynchronously));
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x0002424C File Offset: 0x0002244C
		protected override bool ReadAtValueImplementation()
		{
			return this.ReadAtValueImplementationSynchronously();
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x00024254 File Offset: 0x00022454
		protected override Task<bool> ReadAtValueImplementationAsync()
		{
			return TaskUtils.GetTaskForSynchronousOperation<bool>(new Func<bool>(this.ReadAtValueImplementationSynchronously));
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x00024267 File Offset: 0x00022467
		protected override bool ReadAtCollectionEndImplementation()
		{
			return this.ReadAtCollectionEndImplementationSynchronously();
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x0002426F File Offset: 0x0002246F
		protected override Task<bool> ReadAtCollectionEndImplementationAsync()
		{
			return TaskUtils.GetTaskForSynchronousOperation<bool>(new Func<bool>(this.ReadAtCollectionEndImplementationSynchronously));
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x00024284 File Offset: 0x00022484
		private bool ReadAtStartImplementationSynchronously(DuplicatePropertyNamesChecker duplicatePropertyNamesChecker)
		{
			base.ExpectedItemTypeReference = ReaderValidationUtils.ValidateCollectionMetadataUriAndGetPayloadItemTypeReference(this.jsonLightCollectionDeserializer.MetadataUriParseResult, base.ExpectedItemTypeReference);
			IEdmTypeReference edmTypeReference;
			ODataCollectionStart item = this.jsonLightCollectionDeserializer.ReadCollectionStart(duplicatePropertyNamesChecker, base.IsReadingNestedPayload, base.ExpectedItemTypeReference, out edmTypeReference);
			if (edmTypeReference != null)
			{
				base.ExpectedItemTypeReference = edmTypeReference;
			}
			this.jsonLightCollectionDeserializer.JsonReader.ReadStartArray();
			base.EnterScope(ODataCollectionReaderState.CollectionStart, item);
			return true;
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x000242EC File Offset: 0x000224EC
		private bool ReadAtCollectionStartImplementationSynchronously()
		{
			if (this.jsonLightCollectionDeserializer.JsonReader.NodeType == JsonNodeType.EndArray)
			{
				base.ReplaceScope(ODataCollectionReaderState.CollectionEnd, this.Item);
			}
			else
			{
				object item = this.jsonLightCollectionDeserializer.ReadCollectionItem(base.ExpectedItemTypeReference, base.CollectionValidator);
				base.EnterScope(ODataCollectionReaderState.Value, item);
			}
			return true;
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x0002433C File Offset: 0x0002253C
		private bool ReadAtValueImplementationSynchronously()
		{
			if (this.jsonLightCollectionDeserializer.JsonReader.NodeType == JsonNodeType.EndArray)
			{
				base.PopScope(ODataCollectionReaderState.Value);
				base.ReplaceScope(ODataCollectionReaderState.CollectionEnd, this.Item);
			}
			else
			{
				object item = this.jsonLightCollectionDeserializer.ReadCollectionItem(base.ExpectedItemTypeReference, base.CollectionValidator);
				base.ReplaceScope(ODataCollectionReaderState.Value, item);
			}
			return true;
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x00024393 File Offset: 0x00022593
		private bool ReadAtCollectionEndImplementationSynchronously()
		{
			base.PopScope(ODataCollectionReaderState.CollectionEnd);
			this.jsonLightCollectionDeserializer.ReadCollectionEnd(base.IsReadingNestedPayload);
			this.jsonLightCollectionDeserializer.ReadPayloadEnd(base.IsReadingNestedPayload);
			base.ReplaceScope(ODataCollectionReaderState.Completed, null);
			return false;
		}

		// Token: 0x040003FE RID: 1022
		private readonly ODataJsonLightInputContext jsonLightInputContext;

		// Token: 0x040003FF RID: 1023
		private readonly ODataJsonLightCollectionDeserializer jsonLightCollectionDeserializer;
	}
}
