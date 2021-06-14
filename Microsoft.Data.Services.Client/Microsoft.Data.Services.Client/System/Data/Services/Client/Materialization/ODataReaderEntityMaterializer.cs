using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData;

namespace System.Data.Services.Client.Materialization
{
	// Token: 0x0200003A RID: 58
	internal class ODataReaderEntityMaterializer : ODataEntityMaterializer
	{
		// Token: 0x060001E3 RID: 483 RVA: 0x0000A83C File Offset: 0x00008A3C
		public ODataReaderEntityMaterializer(ODataMessageReader odataMessageReader, ODataReaderWrapper reader, IODataMaterializerContext materializerContext, EntityTrackingAdapter entityTrackingAdapter, QueryComponents queryComponents, Type expectedType, ProjectionPlan materializeEntryPlan) : base(materializerContext, entityTrackingAdapter, queryComponents, expectedType, materializeEntryPlan)
		{
			this.messageReader = odataMessageReader;
			this.feedEntryAdapter = new FeedAndEntryMaterializerAdapter(odataMessageReader, reader, materializerContext.Model, entityTrackingAdapter.MergeOption);
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000A86E File Offset: 0x00008A6E
		internal override ODataFeed CurrentFeed
		{
			get
			{
				return this.feedEntryAdapter.CurrentFeed;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x0000A87B File Offset: 0x00008A7B
		internal override ODataEntry CurrentEntry
		{
			get
			{
				return this.feedEntryAdapter.CurrentEntry;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x0000A888 File Offset: 0x00008A88
		internal override bool IsEndOfStream
		{
			get
			{
				return this.IsDisposed || this.feedEntryAdapter.IsEndOfStream;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000A89F File Offset: 0x00008A9F
		internal override long CountValue
		{
			get
			{
				return this.feedEntryAdapter.GetCountValue(!this.IsDisposed);
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000A8B5 File Offset: 0x00008AB5
		internal override bool IsCountable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000A8B8 File Offset: 0x00008AB8
		protected override bool IsDisposed
		{
			get
			{
				return this.messageReader == null;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001EA RID: 490 RVA: 0x0000A8C3 File Offset: 0x00008AC3
		protected override ODataFormat Format
		{
			get
			{
				return ODataUtils.GetReadFormat(this.messageReader);
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000A8D0 File Offset: 0x00008AD0
		internal static MaterializerEntry ParseSingleEntityPayload(IODataResponseMessage message, ResponseInfo responseInfo, Type expectedType)
		{
			ODataPayloadKind messageType = ODataPayloadKind.Entry;
			MaterializerEntry entry;
			using (ODataMessageReader odataMessageReader = ODataMaterializer.CreateODataMessageReader(message, responseInfo, ref messageType))
			{
				IEdmType expectedType2 = responseInfo.TypeResolver.ResolveExpectedTypeForReading(expectedType);
				ODataReaderWrapper reader = ODataReaderWrapper.Create(odataMessageReader, messageType, expectedType2, responseInfo.ResponsePipeline);
				FeedAndEntryMaterializerAdapter feedAndEntryMaterializerAdapter = new FeedAndEntryMaterializerAdapter(odataMessageReader, reader, responseInfo.Model, responseInfo.MergeOption);
				ODataEntry odataEntry = null;
				bool flag = false;
				while (feedAndEntryMaterializerAdapter.Read())
				{
					flag |= (feedAndEntryMaterializerAdapter.CurrentFeed != null);
					if (feedAndEntryMaterializerAdapter.CurrentEntry != null)
					{
						if (odataEntry != null)
						{
							throw new InvalidOperationException(Strings.AtomParser_SingleEntry_MultipleFound);
						}
						odataEntry = feedAndEntryMaterializerAdapter.CurrentEntry;
					}
				}
				if (odataEntry == null)
				{
					if (flag)
					{
						throw new InvalidOperationException(Strings.AtomParser_SingleEntry_NoneFound);
					}
					throw new InvalidOperationException(Strings.AtomParser_SingleEntry_ExpectedFeedOrEntry);
				}
				else
				{
					entry = MaterializerEntry.GetEntry(odataEntry);
				}
			}
			return entry;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000A9A4 File Offset: 0x00008BA4
		protected override void OnDispose()
		{
			if (this.messageReader != null)
			{
				this.messageReader.Dispose();
				this.messageReader = null;
			}
			this.feedEntryAdapter.Dispose();
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000A9CB File Offset: 0x00008BCB
		protected override bool ReadNextFeedOrEntry()
		{
			return this.feedEntryAdapter.Read();
		}

		// Token: 0x04000211 RID: 529
		private FeedAndEntryMaterializerAdapter feedEntryAdapter;

		// Token: 0x04000212 RID: 530
		private ODataMessageReader messageReader;
	}
}
