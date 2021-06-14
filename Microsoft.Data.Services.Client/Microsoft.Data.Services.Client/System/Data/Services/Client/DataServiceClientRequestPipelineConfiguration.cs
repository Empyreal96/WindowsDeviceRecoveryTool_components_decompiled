using System;
using System.Collections.Generic;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x0200004F RID: 79
	public class DataServiceClientRequestPipelineConfiguration
	{
		// Token: 0x06000289 RID: 649 RVA: 0x0000C9B4 File Offset: 0x0000ABB4
		internal DataServiceClientRequestPipelineConfiguration()
		{
			this.writeEntityReferenceLinkActions = new List<Action<WritingEntityReferenceLinkArgs>>();
			this.writingEndEntryActions = new List<Action<WritingEntryArgs>>();
			this.writingEndNavigationLinkActions = new List<Action<WritingNavigationLinkArgs>>();
			this.writingStartEntryActions = new List<Action<WritingEntryArgs>>();
			this.writingStartNavigationLinkActions = new List<Action<WritingNavigationLinkArgs>>();
			this.messageWriterSettingsConfigurationActions = new List<Action<MessageWriterSettingsArgs>>();
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0000CA09 File Offset: 0x0000AC09
		// (set) Token: 0x0600028B RID: 651 RVA: 0x0000CA11 File Offset: 0x0000AC11
		public Func<DataServiceClientRequestMessageArgs, DataServiceClientRequestMessage> OnMessageCreating
		{
			get
			{
				return this.onmessageCreating;
			}
			set
			{
				if (this.ContextUsingSendingRequest)
				{
					throw new DataServiceClientException(Strings.Context_SendingRequest_InvalidWhenUsingOnMessageCreating);
				}
				this.onmessageCreating = value;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600028C RID: 652 RVA: 0x0000CA2D File Offset: 0x0000AC2D
		internal bool HasOnMessageCreating
		{
			get
			{
				return this.OnMessageCreating != null;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600028D RID: 653 RVA: 0x0000CA3B File Offset: 0x0000AC3B
		// (set) Token: 0x0600028E RID: 654 RVA: 0x0000CA43 File Offset: 0x0000AC43
		internal bool ContextUsingSendingRequest { get; set; }

		// Token: 0x0600028F RID: 655 RVA: 0x0000CA4C File Offset: 0x0000AC4C
		public DataServiceClientRequestPipelineConfiguration OnMessageWriterSettingsCreated(Action<MessageWriterSettingsArgs> args)
		{
			WebUtil.CheckArgumentNull<Action<MessageWriterSettingsArgs>>(args, "args");
			this.messageWriterSettingsConfigurationActions.Add(args);
			return this;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000CA67 File Offset: 0x0000AC67
		public DataServiceClientRequestPipelineConfiguration OnEntryStarting(Action<WritingEntryArgs> action)
		{
			WebUtil.CheckArgumentNull<Action<WritingEntryArgs>>(action, "action");
			this.writingStartEntryActions.Add(action);
			return this;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000CA82 File Offset: 0x0000AC82
		public DataServiceClientRequestPipelineConfiguration OnEntryEnding(Action<WritingEntryArgs> action)
		{
			WebUtil.CheckArgumentNull<Action<WritingEntryArgs>>(action, "action");
			this.writingEndEntryActions.Add(action);
			return this;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000CA9D File Offset: 0x0000AC9D
		public DataServiceClientRequestPipelineConfiguration OnEntityReferenceLink(Action<WritingEntityReferenceLinkArgs> action)
		{
			WebUtil.CheckArgumentNull<Action<WritingEntityReferenceLinkArgs>>(action, "action");
			this.writeEntityReferenceLinkActions.Add(action);
			return this;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000CAB8 File Offset: 0x0000ACB8
		public DataServiceClientRequestPipelineConfiguration OnNavigationLinkStarting(Action<WritingNavigationLinkArgs> action)
		{
			WebUtil.CheckArgumentNull<Action<WritingNavigationLinkArgs>>(action, "action");
			this.writingStartNavigationLinkActions.Add(action);
			return this;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000CAD3 File Offset: 0x0000ACD3
		public DataServiceClientRequestPipelineConfiguration OnNavigationLinkEnding(Action<WritingNavigationLinkArgs> action)
		{
			WebUtil.CheckArgumentNull<Action<WritingNavigationLinkArgs>>(action, "action");
			this.writingEndNavigationLinkActions.Add(action);
			return this;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000CAF0 File Offset: 0x0000ACF0
		internal void ExecuteWriterSettingsConfiguration(ODataMessageWriterSettingsBase writerSettings)
		{
			if (this.messageWriterSettingsConfigurationActions.Count > 0)
			{
				MessageWriterSettingsArgs obj = new MessageWriterSettingsArgs(new DataServiceClientMessageWriterSettingsShim(writerSettings));
				foreach (Action<MessageWriterSettingsArgs> action in this.messageWriterSettingsConfigurationActions)
				{
					action(obj);
				}
			}
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000CB60 File Offset: 0x0000AD60
		internal void ExecuteOnEntryEndActions(ODataEntry entry, object entity)
		{
			if (this.writingEndEntryActions.Count > 0)
			{
				WritingEntryArgs obj = new WritingEntryArgs(entry, entity);
				foreach (Action<WritingEntryArgs> action in this.writingEndEntryActions)
				{
					action(obj);
				}
			}
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000CBCC File Offset: 0x0000ADCC
		internal void ExecuteOnEntryStartActions(ODataEntry entry, object entity)
		{
			if (this.writingStartEntryActions.Count > 0)
			{
				WritingEntryArgs obj = new WritingEntryArgs(entry, entity);
				foreach (Action<WritingEntryArgs> action in this.writingStartEntryActions)
				{
					action(obj);
				}
			}
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000CC38 File Offset: 0x0000AE38
		internal void ExecuteOnNavigationLinkEndActions(ODataNavigationLink link, object source, object target)
		{
			if (this.writingEndNavigationLinkActions.Count > 0)
			{
				WritingNavigationLinkArgs obj = new WritingNavigationLinkArgs(link, source, target);
				foreach (Action<WritingNavigationLinkArgs> action in this.writingEndNavigationLinkActions)
				{
					action(obj);
				}
			}
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000CCA4 File Offset: 0x0000AEA4
		internal void ExecuteOnNavigationLinkStartActions(ODataNavigationLink link, object source, object target)
		{
			if (this.writingStartNavigationLinkActions.Count > 0)
			{
				WritingNavigationLinkArgs obj = new WritingNavigationLinkArgs(link, source, target);
				foreach (Action<WritingNavigationLinkArgs> action in this.writingStartNavigationLinkActions)
				{
					action(obj);
				}
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000CD10 File Offset: 0x0000AF10
		internal void ExecuteEntityReferenceLinkActions(ODataEntityReferenceLink entityReferenceLink, object source, object target)
		{
			if (this.writeEntityReferenceLinkActions.Count > 0)
			{
				WritingEntityReferenceLinkArgs obj = new WritingEntityReferenceLinkArgs(entityReferenceLink, source, target);
				foreach (Action<WritingEntityReferenceLinkArgs> action in this.writeEntityReferenceLinkActions)
				{
					action(obj);
				}
			}
		}

		// Token: 0x0400024B RID: 587
		private readonly List<Action<WritingEntryArgs>> writingStartEntryActions;

		// Token: 0x0400024C RID: 588
		private readonly List<Action<WritingEntryArgs>> writingEndEntryActions;

		// Token: 0x0400024D RID: 589
		private readonly List<Action<WritingEntityReferenceLinkArgs>> writeEntityReferenceLinkActions;

		// Token: 0x0400024E RID: 590
		private readonly List<Action<WritingNavigationLinkArgs>> writingStartNavigationLinkActions;

		// Token: 0x0400024F RID: 591
		private readonly List<Action<WritingNavigationLinkArgs>> writingEndNavigationLinkActions;

		// Token: 0x04000250 RID: 592
		private readonly List<Action<MessageWriterSettingsArgs>> messageWriterSettingsConfigurationActions;

		// Token: 0x04000251 RID: 593
		private Func<DataServiceClientRequestMessageArgs, DataServiceClientRequestMessage> onmessageCreating;
	}
}
