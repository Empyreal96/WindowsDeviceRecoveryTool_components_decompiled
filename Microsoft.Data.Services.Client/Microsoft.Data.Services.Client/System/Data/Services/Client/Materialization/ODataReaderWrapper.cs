using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData;

namespace System.Data.Services.Client.Materialization
{
	// Token: 0x02000047 RID: 71
	internal class ODataReaderWrapper
	{
		// Token: 0x06000248 RID: 584 RVA: 0x0000C1D8 File Offset: 0x0000A3D8
		private ODataReaderWrapper(ODataReader reader, DataServiceClientResponsePipelineConfiguration responsePipeline)
		{
			this.reader = reader;
			this.responsePipeline = responsePipeline;
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000249 RID: 585 RVA: 0x0000C1EE File Offset: 0x0000A3EE
		public ODataReaderState State
		{
			get
			{
				return this.reader.State;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600024A RID: 586 RVA: 0x0000C1FB File Offset: 0x0000A3FB
		public ODataItem Item
		{
			get
			{
				return this.reader.Item;
			}
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000C208 File Offset: 0x0000A408
		public bool Read()
		{
			bool flag = this.reader.Read();
			if (flag && this.responsePipeline.HasConfigurations)
			{
				switch (this.reader.State)
				{
				case ODataReaderState.FeedStart:
					this.responsePipeline.ExecuteOnFeedStartActions((ODataFeed)this.reader.Item);
					break;
				case ODataReaderState.FeedEnd:
					this.responsePipeline.ExecuteOnFeedEndActions((ODataFeed)this.reader.Item);
					break;
				case ODataReaderState.EntryStart:
					this.responsePipeline.ExecuteOnEntryStartActions((ODataEntry)this.reader.Item);
					break;
				case ODataReaderState.EntryEnd:
					this.responsePipeline.ExecuteOnEntryEndActions((ODataEntry)this.reader.Item);
					break;
				case ODataReaderState.NavigationLinkStart:
					this.responsePipeline.ExecuteOnNavigationStartActions((ODataNavigationLink)this.reader.Item);
					break;
				case ODataReaderState.NavigationLinkEnd:
					this.responsePipeline.ExecuteOnNavigationEndActions((ODataNavigationLink)this.reader.Item);
					break;
				}
			}
			return flag;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000C318 File Offset: 0x0000A518
		internal static ODataReaderWrapper Create(ODataMessageReader messageReader, ODataPayloadKind messageType, IEdmType expectedType, DataServiceClientResponsePipelineConfiguration responsePipeline)
		{
			IEdmEntityType edmEntityType = expectedType as IEdmEntityType;
			if (messageType == ODataPayloadKind.Entry)
			{
				return new ODataReaderWrapper(messageReader.CreateODataEntryReader(edmEntityType), responsePipeline);
			}
			return new ODataReaderWrapper(messageReader.CreateODataFeedReader(edmEntityType), responsePipeline);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000C34B File Offset: 0x0000A54B
		internal static ODataReaderWrapper CreateForTest(ODataReader reader, DataServiceClientResponsePipelineConfiguration responsePipeline)
		{
			return new ODataReaderWrapper(reader, responsePipeline);
		}

		// Token: 0x04000237 RID: 567
		private readonly ODataReader reader;

		// Token: 0x04000238 RID: 568
		private readonly DataServiceClientResponsePipelineConfiguration responsePipeline;
	}
}
