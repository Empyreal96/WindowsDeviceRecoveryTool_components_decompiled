using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Json;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.VerboseJson
{
	// Token: 0x02000239 RID: 569
	internal sealed class ODataVerboseJsonCollectionReader : ODataCollectionReaderCore
	{
		// Token: 0x06001237 RID: 4663 RVA: 0x00044470 File Offset: 0x00042670
		internal ODataVerboseJsonCollectionReader(ODataVerboseJsonInputContext verboseJsonInputContext, IEdmTypeReference expectedItemTypeReference, IODataReaderWriterListener listener) : base(verboseJsonInputContext, expectedItemTypeReference, listener)
		{
			this.verboseJsonInputContext = verboseJsonInputContext;
			this.verboseJsonCollectionDeserializer = new ODataVerboseJsonCollectionDeserializer(verboseJsonInputContext);
			if (!verboseJsonInputContext.Model.IsUserModel())
			{
				throw new ODataException(Strings.ODataJsonCollectionReader_ParsingWithoutMetadata);
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06001238 RID: 4664 RVA: 0x000444A6 File Offset: 0x000426A6
		private bool IsResultsWrapperExpected
		{
			get
			{
				return this.verboseJsonInputContext.Version >= ODataVersion.V2 && this.verboseJsonInputContext.ReadingResponse;
			}
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x000444C4 File Offset: 0x000426C4
		protected override bool ReadAtStartImplementation()
		{
			this.verboseJsonCollectionDeserializer.ReadPayloadStart(base.IsReadingNestedPayload);
			if (this.IsResultsWrapperExpected && this.verboseJsonCollectionDeserializer.JsonReader.NodeType != JsonNodeType.StartObject)
			{
				throw new ODataException(Strings.ODataJsonCollectionReader_CannotReadWrappedCollectionStart(this.verboseJsonCollectionDeserializer.JsonReader.NodeType));
			}
			if (!this.IsResultsWrapperExpected && this.verboseJsonCollectionDeserializer.JsonReader.NodeType != JsonNodeType.StartArray)
			{
				throw new ODataException(Strings.ODataJsonCollectionReader_CannotReadCollectionStart(this.verboseJsonCollectionDeserializer.JsonReader.NodeType));
			}
			ODataCollectionStart item = this.verboseJsonCollectionDeserializer.ReadCollectionStart(this.IsResultsWrapperExpected);
			this.verboseJsonCollectionDeserializer.JsonReader.ReadStartArray();
			base.EnterScope(ODataCollectionReaderState.CollectionStart, item);
			return true;
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x00044584 File Offset: 0x00042784
		protected override bool ReadAtCollectionStartImplementation()
		{
			if (this.verboseJsonCollectionDeserializer.JsonReader.NodeType == JsonNodeType.EndArray)
			{
				base.ReplaceScope(ODataCollectionReaderState.CollectionEnd, this.Item);
			}
			else
			{
				object item = this.verboseJsonCollectionDeserializer.ReadCollectionItem(base.ExpectedItemTypeReference, base.CollectionValidator);
				base.EnterScope(ODataCollectionReaderState.Value, item);
			}
			return true;
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x000445D4 File Offset: 0x000427D4
		protected override bool ReadAtValueImplementation()
		{
			if (this.verboseJsonCollectionDeserializer.JsonReader.NodeType == JsonNodeType.EndArray)
			{
				base.PopScope(ODataCollectionReaderState.Value);
				base.ReplaceScope(ODataCollectionReaderState.CollectionEnd, this.Item);
			}
			else
			{
				object item = this.verboseJsonCollectionDeserializer.ReadCollectionItem(base.ExpectedItemTypeReference, base.CollectionValidator);
				base.ReplaceScope(ODataCollectionReaderState.Value, item);
			}
			return true;
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x0004462B File Offset: 0x0004282B
		protected override bool ReadAtCollectionEndImplementation()
		{
			base.PopScope(ODataCollectionReaderState.CollectionEnd);
			this.verboseJsonCollectionDeserializer.ReadCollectionEnd(this.IsResultsWrapperExpected);
			this.verboseJsonCollectionDeserializer.ReadPayloadEnd(base.IsReadingNestedPayload);
			base.ReplaceScope(ODataCollectionReaderState.Completed, null);
			return false;
		}

		// Token: 0x04000692 RID: 1682
		private readonly ODataVerboseJsonInputContext verboseJsonInputContext;

		// Token: 0x04000693 RID: 1683
		private readonly ODataVerboseJsonCollectionDeserializer verboseJsonCollectionDeserializer;
	}
}
