using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using MS.Internal.Documents;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200060F RID: 1551
	internal sealed class BreakRecordTable
	{
		// Token: 0x06006758 RID: 26456 RVA: 0x001CE1EA File Offset: 0x001CC3EA
		internal BreakRecordTable(FlowDocumentPaginator owner)
		{
			this._owner = owner;
			this._breakRecords = new List<BreakRecordTable.BreakRecordTableEntry>();
		}

		// Token: 0x06006759 RID: 26457 RVA: 0x001CE204 File Offset: 0x001CC404
		internal PageBreakRecord GetPageBreakRecord(int pageNumber)
		{
			PageBreakRecord pageBreakRecord = null;
			Invariant.Assert(pageNumber >= 0 && pageNumber <= this._breakRecords.Count, "Invalid PageNumber.");
			if (pageNumber > 0)
			{
				Invariant.Assert(this._breakRecords[pageNumber - 1] != null, "Invalid BreakRecordTable entry.");
				pageBreakRecord = this._breakRecords[pageNumber - 1].BreakRecord;
				Invariant.Assert(pageBreakRecord != null, "BreakRecord can be null only for the first page.");
			}
			return pageBreakRecord;
		}

		// Token: 0x0600675A RID: 26458 RVA: 0x001CE278 File Offset: 0x001CC478
		internal FlowDocumentPage GetCachedDocumentPage(int pageNumber)
		{
			FlowDocumentPage flowDocumentPage = null;
			if (pageNumber < this._breakRecords.Count)
			{
				Invariant.Assert(this._breakRecords[pageNumber] != null, "Invalid BreakRecordTable entry.");
				WeakReference documentPage = this._breakRecords[pageNumber].DocumentPage;
				if (documentPage != null)
				{
					flowDocumentPage = (documentPage.Target as FlowDocumentPage);
					if (flowDocumentPage != null && flowDocumentPage.IsDisposed)
					{
						flowDocumentPage = null;
					}
				}
			}
			return flowDocumentPage;
		}

		// Token: 0x0600675B RID: 26459 RVA: 0x001CE2E0 File Offset: 0x001CC4E0
		internal bool GetPageNumberForContentPosition(TextPointer contentPosition, ref int pageNumber)
		{
			bool result = false;
			Invariant.Assert(pageNumber >= 0 && pageNumber <= this._breakRecords.Count, "Invalid PageNumber.");
			while (pageNumber < this._breakRecords.Count)
			{
				Invariant.Assert(this._breakRecords[pageNumber] != null, "Invalid BreakRecordTable entry.");
				ReadOnlyCollection<TextSegment> textSegments = this._breakRecords[pageNumber].TextSegments;
				if (textSegments == null)
				{
					break;
				}
				if (TextDocumentView.Contains(contentPosition, textSegments))
				{
					result = true;
					break;
				}
				pageNumber++;
			}
			return result;
		}

		// Token: 0x0600675C RID: 26460 RVA: 0x001CE368 File Offset: 0x001CC568
		internal void OnInvalidateLayout()
		{
			if (this._breakRecords.Count > 0)
			{
				this.InvalidateBreakRecords(0, this._breakRecords.Count);
				this._owner.InitiateNextAsyncOperation();
				this._owner.OnPagesChanged(0, 1073741823);
			}
		}

		// Token: 0x0600675D RID: 26461 RVA: 0x001CE3A8 File Offset: 0x001CC5A8
		internal void OnInvalidateLayout(ITextPointer start, ITextPointer end)
		{
			if (this._breakRecords.Count > 0)
			{
				int num;
				int num2;
				this.GetAffectedPages(start, end, out num, out num2);
				num2 = this._breakRecords.Count - num;
				if (num2 > 0)
				{
					this.InvalidateBreakRecords(num, num2);
					this._owner.InitiateNextAsyncOperation();
					this._owner.OnPagesChanged(num, 1073741823);
				}
			}
		}

		// Token: 0x0600675E RID: 26462 RVA: 0x001CE405 File Offset: 0x001CC605
		internal void OnInvalidateRender()
		{
			if (this._breakRecords.Count > 0)
			{
				this.DisposePages(0, this._breakRecords.Count);
				this._owner.OnPagesChanged(0, this._breakRecords.Count);
			}
		}

		// Token: 0x0600675F RID: 26463 RVA: 0x001CE440 File Offset: 0x001CC640
		internal void OnInvalidateRender(ITextPointer start, ITextPointer end)
		{
			if (this._breakRecords.Count > 0)
			{
				int num;
				int num2;
				this.GetAffectedPages(start, end, out num, out num2);
				if (num2 > 0)
				{
					this.DisposePages(num, num2);
					this._owner.OnPagesChanged(num, num2);
				}
			}
		}

		// Token: 0x06006760 RID: 26464 RVA: 0x001CE480 File Offset: 0x001CC680
		internal void UpdateEntry(int pageNumber, FlowDocumentPage page, PageBreakRecord brOut, TextPointer dependentMax)
		{
			Invariant.Assert(pageNumber >= 0 && pageNumber <= this._breakRecords.Count, "The previous BreakRecord does not exist.");
			Invariant.Assert(page != null && page != DocumentPage.Missing, "Cannot update BRT with an invalid document page.");
			ITextView textView = (ITextView)((IServiceProvider)page).GetService(typeof(ITextView));
			Invariant.Assert(textView != null, "Cannot access ITextView for FlowDocumentPage.");
			bool isClean = this.IsClean;
			BreakRecordTable.BreakRecordTableEntry breakRecordTableEntry = new BreakRecordTable.BreakRecordTableEntry();
			breakRecordTableEntry.BreakRecord = brOut;
			breakRecordTableEntry.DocumentPage = new WeakReference(page);
			breakRecordTableEntry.TextSegments = textView.TextSegments;
			breakRecordTableEntry.DependentMax = dependentMax;
			if (pageNumber == this._breakRecords.Count)
			{
				this._breakRecords.Add(breakRecordTableEntry);
				this._owner.OnPaginationProgress(pageNumber, 1);
			}
			else
			{
				if (this._breakRecords[pageNumber].BreakRecord != null && this._breakRecords[pageNumber].BreakRecord != breakRecordTableEntry.BreakRecord)
				{
					this._breakRecords[pageNumber].BreakRecord.Dispose();
				}
				if (this._breakRecords[pageNumber].DocumentPage != null && this._breakRecords[pageNumber].DocumentPage.Target != null && this._breakRecords[pageNumber].DocumentPage.Target != breakRecordTableEntry.DocumentPage.Target)
				{
					((FlowDocumentPage)this._breakRecords[pageNumber].DocumentPage.Target).Dispose();
				}
				this._breakRecords[pageNumber] = breakRecordTableEntry;
			}
			if (!isClean && this.IsClean)
			{
				this._owner.OnPaginationCompleted();
			}
		}

		// Token: 0x06006761 RID: 26465 RVA: 0x001CE624 File Offset: 0x001CC824
		internal bool HasPageBreakRecord(int pageNumber)
		{
			Invariant.Assert(pageNumber >= 0, "Page number cannot be negative.");
			if (pageNumber == 0)
			{
				return true;
			}
			if (pageNumber > this._breakRecords.Count)
			{
				return false;
			}
			Invariant.Assert(this._breakRecords[pageNumber - 1] != null, "Invalid BreakRecordTable entry.");
			return this._breakRecords[pageNumber - 1].BreakRecord != null;
		}

		// Token: 0x170018FA RID: 6394
		// (get) Token: 0x06006762 RID: 26466 RVA: 0x001CE688 File Offset: 0x001CC888
		internal int Count
		{
			get
			{
				return this._breakRecords.Count;
			}
		}

		// Token: 0x170018FB RID: 6395
		// (get) Token: 0x06006763 RID: 26467 RVA: 0x001CE698 File Offset: 0x001CC898
		internal bool IsClean
		{
			get
			{
				if (this._breakRecords.Count == 0)
				{
					return false;
				}
				Invariant.Assert(this._breakRecords[this._breakRecords.Count - 1] != null, "Invalid BreakRecordTable entry.");
				return this._breakRecords[this._breakRecords.Count - 1].BreakRecord == null;
			}
		}

		// Token: 0x06006764 RID: 26468 RVA: 0x001CE6FC File Offset: 0x001CC8FC
		private void DisposePages(int start, int count)
		{
			int i = start + count - 1;
			Invariant.Assert(start >= 0 && start < this._breakRecords.Count, "Invalid starting index for BreakRecordTable invalidation.");
			Invariant.Assert(start + count <= this._breakRecords.Count, "Partial invalidation of BreakRecordTable is not allowed.");
			while (i >= start)
			{
				Invariant.Assert(this._breakRecords[i] != null, "Invalid BreakRecordTable entry.");
				WeakReference documentPage = this._breakRecords[i].DocumentPage;
				if (documentPage != null && documentPage.Target != null)
				{
					((FlowDocumentPage)documentPage.Target).Dispose();
				}
				this._breakRecords[i].DocumentPage = null;
				i--;
			}
		}

		// Token: 0x06006765 RID: 26469 RVA: 0x001CE7B0 File Offset: 0x001CC9B0
		private void InvalidateBreakRecords(int start, int count)
		{
			int i = start + count - 1;
			Invariant.Assert(start >= 0 && start < this._breakRecords.Count, "Invalid starting index for BreakRecordTable invalidation.");
			Invariant.Assert(start + count == this._breakRecords.Count, "Partial invalidation of BreakRecordTable is not allowed.");
			while (i >= start)
			{
				Invariant.Assert(this._breakRecords[i] != null, "Invalid BreakRecordTable entry.");
				WeakReference documentPage = this._breakRecords[i].DocumentPage;
				if (documentPage != null && documentPage.Target != null)
				{
					((FlowDocumentPage)documentPage.Target).Dispose();
				}
				if (this._breakRecords[i].BreakRecord != null)
				{
					this._breakRecords[i].BreakRecord.Dispose();
				}
				this._breakRecords.RemoveAt(i);
				i--;
			}
		}

		// Token: 0x06006766 RID: 26470 RVA: 0x001CE884 File Offset: 0x001CCA84
		private void GetAffectedPages(ITextPointer start, ITextPointer end, out int pageStart, out int pageCount)
		{
			for (pageStart = 0; pageStart < this._breakRecords.Count; pageStart++)
			{
				Invariant.Assert(this._breakRecords[pageStart] != null, "Invalid BreakRecordTable entry.");
				TextPointer dependentMax = this._breakRecords[pageStart].DependentMax;
				if (dependentMax != null && start.CompareTo(dependentMax) <= 0)
				{
					break;
				}
				ReadOnlyCollection<TextSegment> textSegments = this._breakRecords[pageStart].TextSegments;
				if (textSegments == null)
				{
					break;
				}
				bool flag = false;
				foreach (TextSegment textSegment in textSegments)
				{
					if (start.CompareTo(textSegment.End) <= 0)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			pageCount = this._breakRecords.Count - pageStart;
		}

		// Token: 0x0400336C RID: 13164
		private FlowDocumentPaginator _owner;

		// Token: 0x0400336D RID: 13165
		private List<BreakRecordTable.BreakRecordTableEntry> _breakRecords;

		// Token: 0x02000A1E RID: 2590
		private class BreakRecordTableEntry
		{
			// Token: 0x040046FB RID: 18171
			public PageBreakRecord BreakRecord;

			// Token: 0x040046FC RID: 18172
			public ReadOnlyCollection<TextSegment> TextSegments;

			// Token: 0x040046FD RID: 18173
			public WeakReference DocumentPage;

			// Token: 0x040046FE RID: 18174
			public TextPointer DependentMax;
		}
	}
}
