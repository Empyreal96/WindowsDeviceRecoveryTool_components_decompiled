using System;
using System.Collections.Generic;
using Microsoft.Data.OData;

namespace System.Data.Services.Client.Materialization
{
	// Token: 0x02000057 RID: 87
	internal class FeedAndEntryMaterializerAdapter
	{
		// Token: 0x060002D1 RID: 721 RVA: 0x0000D47B File Offset: 0x0000B67B
		internal FeedAndEntryMaterializerAdapter(ODataMessageReader messageReader, ODataReaderWrapper reader, ClientEdmModel model, MergeOption mergeOption) : this(ODataUtils.GetReadFormat(messageReader), reader, model, mergeOption)
		{
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000D48D File Offset: 0x0000B68D
		internal FeedAndEntryMaterializerAdapter(ODataFormat odataFormat, ODataReaderWrapper reader, ClientEdmModel model, MergeOption mergeOption)
		{
			this.readODataFormat = odataFormat;
			this.clientEdmModel = model;
			this.mergeOption = mergeOption;
			this.reader = reader;
			this.currentEntry = null;
			this.currentFeed = null;
			this.feedEntries = null;
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x0000D4C7 File Offset: 0x0000B6C7
		public ODataFeed CurrentFeed
		{
			get
			{
				return this.currentFeed;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x0000D4CF File Offset: 0x0000B6CF
		public ODataEntry CurrentEntry
		{
			get
			{
				return this.currentEntry;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x0000D4D7 File Offset: 0x0000B6D7
		public bool IsEndOfStream
		{
			get
			{
				return this.reader.State == ODataReaderState.Completed;
			}
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000D4E8 File Offset: 0x0000B6E8
		public long GetCountValue(bool readIfNoFeed)
		{
			if (this.currentFeed == null && this.currentEntry == null && readIfNoFeed && this.TryReadFeed(true, out this.currentFeed))
			{
				this.feedEntries = MaterializerFeed.GetFeed(this.currentFeed).Entries.GetEnumerator();
			}
			if (this.currentFeed != null && this.currentFeed.Count != null)
			{
				return this.currentFeed.Count.Value;
			}
			throw new InvalidOperationException(Strings.MaterializeFromAtom_CountNotPresent);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000D570 File Offset: 0x0000B770
		public bool Read()
		{
			if (this.feedEntries != null)
			{
				if (this.feedEntries.MoveNext())
				{
					this.currentEntry = this.feedEntries.Current;
					return true;
				}
				this.feedEntries = null;
				this.currentEntry = null;
			}
			ODataReaderState state = this.reader.State;
			switch (state)
			{
			case ODataReaderState.Start:
			{
				ODataFeed odataFeed;
				MaterializerEntry materializerEntry;
				if (this.TryReadFeedOrEntry(true, out odataFeed, out materializerEntry))
				{
					this.currentEntry = ((materializerEntry != null) ? materializerEntry.Entry : null);
					this.currentFeed = odataFeed;
					if (this.currentFeed != null)
					{
						this.feedEntries = MaterializerFeed.GetFeed(this.currentFeed).Entries.GetEnumerator();
					}
					return true;
				}
				throw new NotImplementedException();
			}
			case ODataReaderState.FeedStart:
			case ODataReaderState.EntryStart:
				break;
			case ODataReaderState.FeedEnd:
			case ODataReaderState.EntryEnd:
				if (this.TryRead() || this.reader.State != ODataReaderState.Completed)
				{
					throw Error.InternalError(InternalError.UnexpectedReadState);
				}
				this.currentEntry = null;
				return false;
			default:
				if (state == ODataReaderState.Completed)
				{
					this.currentEntry = null;
					return false;
				}
				break;
			}
			throw Error.InternalError(InternalError.UnexpectedReadState);
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000D66B File Offset: 0x0000B86B
		public void Dispose()
		{
			if (this.feedEntries != null)
			{
				this.feedEntries.Dispose();
				this.feedEntries = null;
			}
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000D688 File Offset: 0x0000B888
		private bool TryReadFeedOrEntry(bool lazy, out ODataFeed feed, out MaterializerEntry entry)
		{
			if (this.TryStartReadFeedOrEntry())
			{
				if (this.reader.State == ODataReaderState.EntryStart)
				{
					entry = this.ReadEntryCore();
					feed = null;
				}
				else
				{
					entry = null;
					feed = this.ReadFeedCore(lazy);
				}
			}
			else
			{
				feed = null;
				entry = null;
			}
			return feed != null || entry != null;
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000D6DA File Offset: 0x0000B8DA
		private bool TryStartReadFeedOrEntry()
		{
			return this.TryRead() && (this.reader.State == ODataReaderState.FeedStart || this.reader.State == ODataReaderState.EntryStart);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000D704 File Offset: 0x0000B904
		private bool TryReadFeed(bool lazy, out ODataFeed feed)
		{
			if (this.TryStartReadFeedOrEntry())
			{
				this.ExpectState(ODataReaderState.FeedStart);
				feed = this.ReadFeedCore(lazy);
			}
			else
			{
				feed = null;
			}
			return feed != null;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000D72C File Offset: 0x0000B92C
		private ODataFeed ReadFeedCore(bool lazy)
		{
			this.ExpectState(ODataReaderState.FeedStart);
			ODataFeed odataFeed = (ODataFeed)this.reader.Item;
			IEnumerable<ODataEntry> enumerable = this.LazyReadEntries();
			if (lazy)
			{
				MaterializerFeed.CreateFeed(odataFeed, enumerable);
			}
			else
			{
				MaterializerFeed.CreateFeed(odataFeed, new List<ODataEntry>(enumerable));
			}
			return odataFeed;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000D860 File Offset: 0x0000BA60
		private IEnumerable<ODataEntry> LazyReadEntries()
		{
			MaterializerEntry entryAndState;
			while (this.TryReadEntry(out entryAndState))
			{
				yield return entryAndState.Entry;
			}
			yield break;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000D87D File Offset: 0x0000BA7D
		private bool TryReadEntry(out MaterializerEntry entry)
		{
			if (this.TryStartReadFeedOrEntry())
			{
				this.ExpectState(ODataReaderState.EntryStart);
				entry = this.ReadEntryCore();
				return true;
			}
			entry = null;
			return false;
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000D89C File Offset: 0x0000BA9C
		private MaterializerEntry ReadEntryCore()
		{
			this.ExpectState(ODataReaderState.EntryStart);
			ODataEntry odataEntry = (ODataEntry)this.reader.Item;
			List<ODataNavigationLink> list = new List<ODataNavigationLink>();
			MaterializerEntry materializerEntry;
			if (odataEntry != null)
			{
				materializerEntry = MaterializerEntry.CreateEntry(odataEntry, this.readODataFormat, this.mergeOption != MergeOption.NoTracking, this.clientEdmModel);
				for (;;)
				{
					this.AssertRead();
					switch (this.reader.State)
					{
					case ODataReaderState.EntryEnd:
						goto IL_7B;
					case ODataReaderState.NavigationLinkStart:
						list.Add(this.ReadNavigationLink());
						goto IL_7B;
					}
					break;
					IL_7B:
					if (this.reader.State == ODataReaderState.EntryEnd)
					{
						goto Block_3;
					}
				}
				throw Error.InternalError(InternalError.UnexpectedReadState);
				Block_3:
				materializerEntry.UpdateEntityDescriptor();
			}
			else
			{
				materializerEntry = MaterializerEntry.CreateEmpty();
				this.ReadAndExpectState(ODataReaderState.EntryEnd);
			}
			foreach (ODataNavigationLink link in list)
			{
				materializerEntry.AddNavigationLink(link);
			}
			return materializerEntry;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000D98C File Offset: 0x0000BB8C
		private ODataNavigationLink ReadNavigationLink()
		{
			ODataNavigationLink odataNavigationLink = (ODataNavigationLink)this.reader.Item;
			ODataFeed odataFeed;
			MaterializerEntry entry;
			if (this.TryReadFeedOrEntry(false, out odataFeed, out entry))
			{
				if (odataFeed != null)
				{
					MaterializerNavigationLink.CreateLink(odataNavigationLink, odataFeed);
				}
				else
				{
					MaterializerNavigationLink.CreateLink(odataNavigationLink, entry);
				}
				this.ReadAndExpectState(ODataReaderState.NavigationLinkEnd);
			}
			this.ExpectState(ODataReaderState.NavigationLinkEnd);
			return odataNavigationLink;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000D9DC File Offset: 0x0000BBDC
		private bool TryRead()
		{
			bool result;
			try
			{
				result = this.reader.Read();
			}
			catch (ODataErrorException ex)
			{
				throw new DataServiceClientException(Strings.Deserialize_ServerException(ex.Error.Message), ex);
			}
			catch (ODataException ex2)
			{
				throw new InvalidOperationException(ex2.Message, ex2);
			}
			return result;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000DA3C File Offset: 0x0000BC3C
		private void ReadAndExpectState(ODataReaderState expectedState)
		{
			this.AssertRead();
			this.ExpectState(expectedState);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000DA4B File Offset: 0x0000BC4B
		private void AssertRead()
		{
			if (!this.TryRead())
			{
				throw Error.InternalError(InternalError.UnexpectedReadState);
			}
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000DA5C File Offset: 0x0000BC5C
		private void ExpectState(ODataReaderState expectedState)
		{
			if (this.reader.State != expectedState)
			{
				throw Error.InternalError(InternalError.UnexpectedReadState);
			}
		}

		// Token: 0x04000261 RID: 609
		private readonly ODataFormat readODataFormat;

		// Token: 0x04000262 RID: 610
		private readonly ODataReaderWrapper reader;

		// Token: 0x04000263 RID: 611
		private readonly ClientEdmModel clientEdmModel;

		// Token: 0x04000264 RID: 612
		private readonly MergeOption mergeOption;

		// Token: 0x04000265 RID: 613
		private IEnumerator<ODataEntry> feedEntries;

		// Token: 0x04000266 RID: 614
		private ODataFeed currentFeed;

		// Token: 0x04000267 RID: 615
		private ODataEntry currentEntry;
	}
}
