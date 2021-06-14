using System;

namespace System.Windows.Navigation
{
	// Token: 0x0200031B RID: 795
	internal class PageFunctionReturnInfo : NavigateInfo
	{
		// Token: 0x06002A00 RID: 10752 RVA: 0x000C1BA1 File Offset: 0x000BFDA1
		internal PageFunctionReturnInfo(PageFunctionBase finishingChildPageFunction, Uri source, NavigationMode navigationMode, JournalEntry journalEntry, object returnEventArgs) : base(source, navigationMode, journalEntry)
		{
			this._returnEventArgs = returnEventArgs;
			this._finishingChildPageFunction = finishingChildPageFunction;
		}

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x06002A01 RID: 10753 RVA: 0x000C1BBC File Offset: 0x000BFDBC
		internal object ReturnEventArgs
		{
			get
			{
				return this._returnEventArgs;
			}
		}

		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x06002A02 RID: 10754 RVA: 0x000C1BC4 File Offset: 0x000BFDC4
		internal PageFunctionBase FinishingChildPageFunction
		{
			get
			{
				return this._finishingChildPageFunction;
			}
		}

		// Token: 0x04001C23 RID: 7203
		private object _returnEventArgs;

		// Token: 0x04001C24 RID: 7204
		private PageFunctionBase _finishingChildPageFunction;
	}
}
