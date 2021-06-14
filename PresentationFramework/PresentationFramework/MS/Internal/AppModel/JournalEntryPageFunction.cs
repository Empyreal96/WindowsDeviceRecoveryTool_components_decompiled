using System;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x0200078A RID: 1930
	[Serializable]
	internal abstract class JournalEntryPageFunction : JournalEntry, ISerializable
	{
		// Token: 0x0600795C RID: 31068 RVA: 0x0022720F File Offset: 0x0022540F
		internal JournalEntryPageFunction(JournalEntryGroupState jeGroupState, PageFunctionBase pageFunction) : base(jeGroupState, null)
		{
			this.PageFunctionId = pageFunction.PageFunctionId;
			this.ParentPageFunctionId = pageFunction.ParentPageFunctionId;
		}

		// Token: 0x0600795D RID: 31069 RVA: 0x00227234 File Offset: 0x00225434
		protected JournalEntryPageFunction(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._pageFunctionId = (Guid)info.GetValue("_pageFunctionId", typeof(Guid));
			this._parentPageFunctionId = (Guid)info.GetValue("_parentPageFunctionId", typeof(Guid));
		}

		// Token: 0x0600795E RID: 31070 RVA: 0x00227289 File Offset: 0x00225489
		[SecurityCritical]
		[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("_pageFunctionId", this._pageFunctionId);
			info.AddValue("_parentPageFunctionId", this._parentPageFunctionId);
		}

		// Token: 0x17001CAB RID: 7339
		// (get) Token: 0x0600795F RID: 31071 RVA: 0x002272BF File Offset: 0x002254BF
		// (set) Token: 0x06007960 RID: 31072 RVA: 0x002272C7 File Offset: 0x002254C7
		internal Guid PageFunctionId
		{
			get
			{
				return this._pageFunctionId;
			}
			set
			{
				this._pageFunctionId = value;
			}
		}

		// Token: 0x17001CAC RID: 7340
		// (get) Token: 0x06007961 RID: 31073 RVA: 0x002272D0 File Offset: 0x002254D0
		// (set) Token: 0x06007962 RID: 31074 RVA: 0x002272D8 File Offset: 0x002254D8
		internal Guid ParentPageFunctionId
		{
			get
			{
				return this._parentPageFunctionId;
			}
			set
			{
				this._parentPageFunctionId = value;
			}
		}

		// Token: 0x06007963 RID: 31075 RVA: 0x00016748 File Offset: 0x00014948
		internal override bool IsPageFunction()
		{
			return true;
		}

		// Token: 0x06007964 RID: 31076 RVA: 0x0000B02A File Offset: 0x0000922A
		internal override bool IsAlive()
		{
			return false;
		}

		// Token: 0x06007965 RID: 31077
		internal abstract PageFunctionBase ResumePageFunction();

		// Token: 0x06007966 RID: 31078 RVA: 0x002272E4 File Offset: 0x002254E4
		internal static int GetParentPageJournalIndex(NavigationService NavigationService, Journal journal, PageFunctionBase endingPF)
		{
			for (int i = journal.CurrentIndex - 1; i >= 0; i--)
			{
				JournalEntry journalEntry = journal[i];
				if (!(journalEntry.NavigationServiceId != NavigationService.GuidId))
				{
					JournalEntryPageFunction journalEntryPageFunction = journalEntry as JournalEntryPageFunction;
					if (endingPF.ParentPageFunctionId == Guid.Empty)
					{
						if (journalEntryPageFunction == null)
						{
							return i;
						}
					}
					else if (journalEntryPageFunction != null && journalEntryPageFunction.PageFunctionId == endingPF.ParentPageFunctionId)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x04003980 RID: 14720
		private Guid _pageFunctionId;

		// Token: 0x04003981 RID: 14721
		private Guid _parentPageFunctionId;

		// Token: 0x04003982 RID: 14722
		internal const int _NoParentPage = -1;
	}
}
