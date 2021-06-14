using System;
using System.Collections.Generic;
using System.Data.Services.Client.Materialization;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x02000056 RID: 86
	public class DataServiceClientResponsePipelineConfiguration
	{
		// Token: 0x060002B9 RID: 697 RVA: 0x0000CE6C File Offset: 0x0000B06C
		internal DataServiceClientResponsePipelineConfiguration(object sender)
		{
			this.sender = sender;
			this.readingEndEntryActions = new List<Action<ReadingEntryArgs>>();
			this.readingEndFeedActions = new List<Action<ReadingFeedArgs>>();
			this.readingEndNavigationLinkActions = new List<Action<ReadingNavigationLinkArgs>>();
			this.readingStartEntryActions = new List<Action<ReadingEntryArgs>>();
			this.readingStartFeedActions = new List<Action<ReadingFeedArgs>>();
			this.readingStartNavigationLinkActions = new List<Action<ReadingNavigationLinkArgs>>();
			this.materializedEntityActions = new List<Action<MaterializedEntityArgs>>();
			this.messageReaderSettingsConfigurationActions = new List<Action<MessageReaderSettingsArgs>>();
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060002BA RID: 698 RVA: 0x0000CEE0 File Offset: 0x0000B0E0
		// (remove) Token: 0x060002BB RID: 699 RVA: 0x0000CF18 File Offset: 0x0000B118
		internal event EventHandler<ReadingWritingEntityEventArgs> ReadingAtomEntity;

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060002BC RID: 700 RVA: 0x0000CF50 File Offset: 0x0000B150
		internal bool HasConfigurations
		{
			get
			{
				return this.readingStartEntryActions.Count > 0 || this.readingEndEntryActions.Count > 0 || this.readingStartFeedActions.Count > 0 || this.readingEndFeedActions.Count > 0 || this.readingStartNavigationLinkActions.Count > 0 || this.readingEndNavigationLinkActions.Count > 0;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060002BD RID: 701 RVA: 0x0000CFB3 File Offset: 0x0000B1B3
		internal bool HasAtomReadingEntityHandlers
		{
			get
			{
				return this.ReadingAtomEntity != null;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060002BE RID: 702 RVA: 0x0000CFC1 File Offset: 0x0000B1C1
		internal bool HasReadingEntityHandlers
		{
			get
			{
				return this.ReadingAtomEntity != null || this.materializedEntityActions.Count > 0;
			}
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000CFDC File Offset: 0x0000B1DC
		public DataServiceClientResponsePipelineConfiguration OnMessageReaderSettingsCreated(Action<MessageReaderSettingsArgs> messageReaderSettingsAction)
		{
			WebUtil.CheckArgumentNull<Action<MessageReaderSettingsArgs>>(messageReaderSettingsAction, "messageReaderSettingsAction");
			this.messageReaderSettingsConfigurationActions.Add(messageReaderSettingsAction);
			return this;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000CFF7 File Offset: 0x0000B1F7
		public DataServiceClientResponsePipelineConfiguration OnEntryStarted(Action<ReadingEntryArgs> action)
		{
			WebUtil.CheckArgumentNull<Action<ReadingEntryArgs>>(action, "action");
			this.readingStartEntryActions.Add(action);
			return this;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000D012 File Offset: 0x0000B212
		public DataServiceClientResponsePipelineConfiguration OnEntryEnded(Action<ReadingEntryArgs> action)
		{
			WebUtil.CheckArgumentNull<Action<ReadingEntryArgs>>(action, "action");
			this.readingEndEntryActions.Add(action);
			return this;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000D02D File Offset: 0x0000B22D
		public DataServiceClientResponsePipelineConfiguration OnFeedStarted(Action<ReadingFeedArgs> action)
		{
			WebUtil.CheckArgumentNull<Action<ReadingFeedArgs>>(action, "action");
			this.readingStartFeedActions.Add(action);
			return this;
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000D048 File Offset: 0x0000B248
		public DataServiceClientResponsePipelineConfiguration OnFeedEnded(Action<ReadingFeedArgs> action)
		{
			WebUtil.CheckArgumentNull<Action<ReadingFeedArgs>>(action, "action");
			this.readingEndFeedActions.Add(action);
			return this;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000D063 File Offset: 0x0000B263
		public DataServiceClientResponsePipelineConfiguration OnNavigationLinkStarted(Action<ReadingNavigationLinkArgs> action)
		{
			WebUtil.CheckArgumentNull<Action<ReadingNavigationLinkArgs>>(action, "action");
			this.readingStartNavigationLinkActions.Add(action);
			return this;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000D07E File Offset: 0x0000B27E
		public DataServiceClientResponsePipelineConfiguration OnNavigationLinkEnded(Action<ReadingNavigationLinkArgs> action)
		{
			WebUtil.CheckArgumentNull<Action<ReadingNavigationLinkArgs>>(action, "action");
			this.readingEndNavigationLinkActions.Add(action);
			return this;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000D099 File Offset: 0x0000B299
		public DataServiceClientResponsePipelineConfiguration OnEntityMaterialized(Action<MaterializedEntityArgs> action)
		{
			WebUtil.CheckArgumentNull<Action<MaterializedEntityArgs>>(action, "action");
			this.materializedEntityActions.Add(action);
			return this;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000D0B4 File Offset: 0x0000B2B4
		internal void ExecuteReaderSettingsConfiguration(ODataMessageReaderSettingsBase readerSettings)
		{
			if (this.messageReaderSettingsConfigurationActions.Count > 0)
			{
				MessageReaderSettingsArgs obj = new MessageReaderSettingsArgs(new DataServiceClientMessageReaderSettingsShim(readerSettings));
				foreach (Action<MessageReaderSettingsArgs> action in this.messageReaderSettingsConfigurationActions)
				{
					action(obj);
				}
			}
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000D124 File Offset: 0x0000B324
		internal void ExecuteOnEntryEndActions(ODataEntry entry)
		{
			if (this.readingEndEntryActions.Count > 0)
			{
				ReadingEntryArgs obj = new ReadingEntryArgs(entry);
				foreach (Action<ReadingEntryArgs> action in this.readingEndEntryActions)
				{
					action(obj);
				}
			}
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000D18C File Offset: 0x0000B38C
		internal void ExecuteOnEntryStartActions(ODataEntry entry)
		{
			if (this.readingStartEntryActions.Count > 0)
			{
				ReadingEntryArgs obj = new ReadingEntryArgs(entry);
				foreach (Action<ReadingEntryArgs> action in this.readingStartEntryActions)
				{
					action(obj);
				}
			}
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000D1F4 File Offset: 0x0000B3F4
		internal void ExecuteOnFeedEndActions(ODataFeed feed)
		{
			if (this.readingEndFeedActions.Count > 0)
			{
				ReadingFeedArgs obj = new ReadingFeedArgs(feed);
				foreach (Action<ReadingFeedArgs> action in this.readingEndFeedActions)
				{
					action(obj);
				}
			}
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000D25C File Offset: 0x0000B45C
		internal void ExecuteOnFeedStartActions(ODataFeed feed)
		{
			if (this.readingStartFeedActions.Count > 0)
			{
				ReadingFeedArgs obj = new ReadingFeedArgs(feed);
				foreach (Action<ReadingFeedArgs> action in this.readingStartFeedActions)
				{
					action(obj);
				}
			}
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000D2C4 File Offset: 0x0000B4C4
		internal void ExecuteOnNavigationEndActions(ODataNavigationLink link)
		{
			if (this.readingEndNavigationLinkActions.Count > 0)
			{
				ReadingNavigationLinkArgs obj = new ReadingNavigationLinkArgs(link);
				foreach (Action<ReadingNavigationLinkArgs> action in this.readingEndNavigationLinkActions)
				{
					action(obj);
				}
			}
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000D32C File Offset: 0x0000B52C
		internal void ExecuteOnNavigationStartActions(ODataNavigationLink link)
		{
			if (this.readingStartNavigationLinkActions.Count > 0)
			{
				ReadingNavigationLinkArgs obj = new ReadingNavigationLinkArgs(link);
				foreach (Action<ReadingNavigationLinkArgs> action in this.readingStartNavigationLinkActions)
				{
					action(obj);
				}
			}
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000D394 File Offset: 0x0000B594
		internal void ExecuteEntityMaterializedActions(ODataEntry entry, object entity)
		{
			if (this.materializedEntityActions.Count > 0)
			{
				MaterializedEntityArgs obj = new MaterializedEntityArgs(entry, entity);
				foreach (Action<MaterializedEntityArgs> action in this.materializedEntityActions)
				{
					action(obj);
				}
			}
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000D400 File Offset: 0x0000B600
		internal void FireReadingAtomEntityEvent(MaterializerEntry materializerEntry)
		{
			if (this.ReadingAtomEntity != null && materializerEntry.Format == ODataFormat.Atom)
			{
				ReadingEntityInfo annotation = materializerEntry.Entry.GetAnnotation<ReadingEntityInfo>();
				ReadingWritingEntityEventArgs e = new ReadingWritingEntityEventArgs(materializerEntry.ResolvedObject, annotation.EntryPayload, annotation.BaseUri);
				this.ReadingAtomEntity(this.sender, e);
			}
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000D458 File Offset: 0x0000B658
		internal void FireEndEntryEvents(MaterializerEntry entry)
		{
			if (this.HasReadingEntityHandlers)
			{
				this.ExecuteEntityMaterializedActions(entry.Entry, entry.ResolvedObject);
				this.FireReadingAtomEntityEvent(entry);
			}
		}

		// Token: 0x04000257 RID: 599
		private readonly List<Action<ReadingEntryArgs>> readingStartEntryActions;

		// Token: 0x04000258 RID: 600
		private readonly List<Action<ReadingEntryArgs>> readingEndEntryActions;

		// Token: 0x04000259 RID: 601
		private readonly List<Action<ReadingFeedArgs>> readingStartFeedActions;

		// Token: 0x0400025A RID: 602
		private readonly List<Action<ReadingFeedArgs>> readingEndFeedActions;

		// Token: 0x0400025B RID: 603
		private readonly List<Action<ReadingNavigationLinkArgs>> readingStartNavigationLinkActions;

		// Token: 0x0400025C RID: 604
		private readonly List<Action<ReadingNavigationLinkArgs>> readingEndNavigationLinkActions;

		// Token: 0x0400025D RID: 605
		private readonly List<Action<MaterializedEntityArgs>> materializedEntityActions;

		// Token: 0x0400025E RID: 606
		private readonly List<Action<MessageReaderSettingsArgs>> messageReaderSettingsConfigurationActions;

		// Token: 0x0400025F RID: 607
		private readonly object sender;
	}
}
