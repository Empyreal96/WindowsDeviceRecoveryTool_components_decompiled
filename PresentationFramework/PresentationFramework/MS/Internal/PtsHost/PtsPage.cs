using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200063C RID: 1596
	internal class PtsPage : IDisposable
	{
		// Token: 0x060069ED RID: 27117 RVA: 0x001E20E7 File Offset: 0x001E02E7
		internal PtsPage(Section section) : this()
		{
			this._section = section;
		}

		// Token: 0x060069EE RID: 27118 RVA: 0x001E20F6 File Offset: 0x001E02F6
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private PtsPage()
		{
			this._ptsPage = new SecurityCriticalDataForSet<IntPtr>(IntPtr.Zero);
		}

		// Token: 0x060069EF RID: 27119 RVA: 0x001E211C File Offset: 0x001E031C
		~PtsPage()
		{
			this.Dispose(false);
		}

		// Token: 0x060069F0 RID: 27120 RVA: 0x001E214C File Offset: 0x001E034C
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060069F1 RID: 27121 RVA: 0x001E215C File Offset: 0x001E035C
		internal bool PrepareForBottomlessUpdate()
		{
			bool flag = !this.IsEmpty;
			if (!this._section.CanUpdate)
			{
				flag = false;
			}
			else if (this._section.StructuralCache != null)
			{
				if (this._section.StructuralCache.ForceReformat)
				{
					flag = false;
					this._section.StructuralCache.ClearUpdateInfo(true);
				}
				else if (this._section.StructuralCache.DtrList != null && !flag)
				{
					this._section.InvalidateStructure();
					this._section.StructuralCache.ClearUpdateInfo(false);
				}
			}
			return flag;
		}

		// Token: 0x060069F2 RID: 27122 RVA: 0x001E21EC File Offset: 0x001E03EC
		internal bool PrepareForFiniteUpdate(PageBreakRecord breakRecord)
		{
			bool flag = !this.IsEmpty;
			if (this._section.StructuralCache != null)
			{
				if (this._section.StructuralCache.ForceReformat)
				{
					flag = false;
					this._section.InvalidateStructure();
					this._section.StructuralCache.ClearUpdateInfo(this._section.StructuralCache.DestroyStructure);
				}
				else if (this._section.StructuralCache.DtrList != null)
				{
					this._section.InvalidateStructure();
					if (!flag)
					{
						this._section.StructuralCache.ClearUpdateInfo(false);
					}
				}
				else
				{
					flag = false;
					this._section.StructuralCache.ClearUpdateInfo(false);
				}
			}
			return flag;
		}

		// Token: 0x060069F3 RID: 27123 RVA: 0x001E229C File Offset: 0x001E049C
		internal IInputElement InputHitTest(Point p)
		{
			IInputElement result = null;
			if (!this.IsEmpty)
			{
				PTS.FSPOINT pt = TextDpi.ToTextPoint(p);
				result = this.InputHitTestPage(pt);
			}
			return result;
		}

		// Token: 0x060069F4 RID: 27124 RVA: 0x001E22C4 File Offset: 0x001E04C4
		internal List<Rect> GetRectangles(ContentElement e, int start, int length)
		{
			List<Rect> result = new List<Rect>();
			if (!this.IsEmpty)
			{
				result = this.GetRectanglesInPage(e, start, length);
			}
			return result;
		}

		// Token: 0x060069F5 RID: 27125 RVA: 0x001E22EA File Offset: 0x001E04EA
		private static object BackgroundFormatStatic(object arg)
		{
			Invariant.Assert(arg is PtsPage);
			((PtsPage)arg).BackgroundFormat();
			return null;
		}

		// Token: 0x060069F6 RID: 27126 RVA: 0x001E2308 File Offset: 0x001E0508
		private void BackgroundFormat()
		{
			FlowDocument formattingOwner = this._section.StructuralCache.FormattingOwner;
			if (formattingOwner.Formatter is FlowDocumentFormatter)
			{
				this._section.StructuralCache.BackgroundFormatInfo.BackgroundFormat(formattingOwner.BottomlessFormatter, false);
			}
		}

		// Token: 0x060069F7 RID: 27127 RVA: 0x001E2350 File Offset: 0x001E0550
		private void DeferFormattingToBackground()
		{
			int cpinterrupted = this._section.StructuralCache.BackgroundFormatInfo.CPInterrupted;
			int cchAllText = this._section.StructuralCache.BackgroundFormatInfo.CchAllText;
			DirtyTextRange dtr = new DirtyTextRange(cpinterrupted, cchAllText - cpinterrupted, cchAllText - cpinterrupted, false);
			this._section.StructuralCache.AddDirtyTextRange(dtr);
			this._backgroundFormatOperation = Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, PtsPage.BackgroundUpdateCallback, this);
		}

		// Token: 0x060069F8 RID: 27128 RVA: 0x001E23C0 File Offset: 0x001E05C0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void CreateBottomlessPage()
		{
			this.OnBeforeFormatPage(false, false);
			if (TracePageFormatting.IsEnabled)
			{
				TracePageFormatting.Trace(TraceEventType.Start, TracePageFormatting.FormatPage, this.PageContext, this.PtsContext);
			}
			PTS.FSFMTRBL fsfmtrbl;
			IntPtr value;
			int num = PTS.FsCreatePageBottomless(this.PtsContext.Context, this._section.Handle, out fsfmtrbl, out value);
			if (num != 0)
			{
				this._ptsPage.Value = IntPtr.Zero;
				PTS.ValidateAndTrace(num, this.PtsContext);
			}
			else
			{
				this._ptsPage.Value = value;
			}
			if (TracePageFormatting.IsEnabled)
			{
				TracePageFormatting.Trace(TraceEventType.Stop, TracePageFormatting.FormatPage, this.PageContext, this.PtsContext);
			}
			this.OnAfterFormatPage(true, false);
			if (fsfmtrbl == PTS.FSFMTRBL.fmtrblInterrupted)
			{
				this.DeferFormattingToBackground();
			}
		}

		// Token: 0x060069F9 RID: 27129 RVA: 0x001E2478 File Offset: 0x001E0678
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void UpdateBottomlessPage()
		{
			if (this.IsEmpty)
			{
				return;
			}
			this.OnBeforeFormatPage(false, true);
			if (TracePageFormatting.IsEnabled)
			{
				TracePageFormatting.Trace(TraceEventType.Start, TracePageFormatting.FormatPage, this.PageContext, this.PtsContext);
			}
			PTS.FSFMTRBL fsfmtrbl;
			int num = PTS.FsUpdateBottomlessPage(this.PtsContext.Context, this._ptsPage.Value, this._section.Handle, out fsfmtrbl);
			if (num != 0)
			{
				this.DestroyPage();
				PTS.ValidateAndTrace(num, this.PtsContext);
			}
			if (TracePageFormatting.IsEnabled)
			{
				TracePageFormatting.Trace(TraceEventType.Stop, TracePageFormatting.FormatPage, this.PageContext, this.PtsContext);
			}
			this.OnAfterFormatPage(true, true);
			if (fsfmtrbl == PTS.FSFMTRBL.fmtrblInterrupted)
			{
				this.DeferFormattingToBackground();
			}
		}

		// Token: 0x060069FA RID: 27130 RVA: 0x001E252C File Offset: 0x001E072C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void CreateFinitePage(PageBreakRecord breakRecord)
		{
			this.OnBeforeFormatPage(true, false);
			if (TracePageFormatting.IsEnabled)
			{
				TracePageFormatting.Trace(TraceEventType.Start, TracePageFormatting.FormatPage, this.PageContext, this.PtsContext);
			}
			IntPtr pfsBRPageStart = (breakRecord != null) ? breakRecord.BreakRecord : IntPtr.Zero;
			PTS.FSFMTR fsfmtr;
			IntPtr value;
			IntPtr zero;
			int num = PTS.FsCreatePageFinite(this.PtsContext.Context, pfsBRPageStart, this._section.Handle, out fsfmtr, out value, out zero);
			if (num != 0)
			{
				this._ptsPage.Value = IntPtr.Zero;
				zero = IntPtr.Zero;
				PTS.ValidateAndTrace(num, this.PtsContext);
			}
			else
			{
				this._ptsPage.Value = value;
			}
			if (zero != IntPtr.Zero)
			{
				StructuralCache structuralCache = this._section.StructuralCache;
				if (structuralCache != null)
				{
					this._breakRecord = new PageBreakRecord(this.PtsContext, new SecurityCriticalDataForSet<IntPtr>(zero), (breakRecord != null) ? (breakRecord.PageNumber + 1) : 1);
				}
			}
			if (TracePageFormatting.IsEnabled)
			{
				TracePageFormatting.Trace(TraceEventType.Stop, TracePageFormatting.FormatPage, this.PageContext, this.PtsContext);
			}
			this.OnAfterFormatPage(true, false);
		}

		// Token: 0x060069FB RID: 27131 RVA: 0x001E263C File Offset: 0x001E083C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void UpdateFinitePage(PageBreakRecord breakRecord)
		{
			if (this.IsEmpty)
			{
				return;
			}
			this.OnBeforeFormatPage(true, true);
			if (TracePageFormatting.IsEnabled)
			{
				TracePageFormatting.Trace(TraceEventType.Start, TracePageFormatting.FormatPage, this.PageContext, this.PtsContext);
			}
			IntPtr pfsBRPageStart = (breakRecord != null) ? breakRecord.BreakRecord : IntPtr.Zero;
			PTS.FSFMTR fsfmtr;
			IntPtr intPtr;
			int num = PTS.FsUpdateFinitePage(this.PtsContext.Context, this._ptsPage.Value, pfsBRPageStart, this._section.Handle, out fsfmtr, out intPtr);
			if (num != 0)
			{
				this.DestroyPage();
				PTS.ValidateAndTrace(num, this.PtsContext);
			}
			if (intPtr != IntPtr.Zero)
			{
				StructuralCache structuralCache = this._section.StructuralCache;
				if (structuralCache != null)
				{
					this._breakRecord = new PageBreakRecord(this.PtsContext, new SecurityCriticalDataForSet<IntPtr>(intPtr), (breakRecord != null) ? (breakRecord.PageNumber + 1) : 1);
				}
			}
			if (TracePageFormatting.IsEnabled)
			{
				TracePageFormatting.Trace(TraceEventType.Stop, TracePageFormatting.FormatPage, this.PageContext, this.PtsContext);
			}
			this.OnAfterFormatPage(true, true);
		}

		// Token: 0x060069FC RID: 27132 RVA: 0x001E273C File Offset: 0x001E093C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void ArrangePage()
		{
			if (this.IsEmpty)
			{
				return;
			}
			this._section.UpdateSegmentLastFormatPositions();
			PTS.FSPAGEDETAILS fspagedetails;
			PTS.Validate(PTS.FsQueryPageDetails(this.PtsContext.Context, this._ptsPage.Value, out fspagedetails));
			if (PTS.ToBoolean(fspagedetails.fSimple))
			{
				this._section.StructuralCache.CurrentArrangeContext.PushNewPageData(this._pageContextOfThisPage, fspagedetails.u.simple.trackdescr.fsrc, this._finitePage);
				PtsHelper.ArrangeTrack(this.PtsContext, ref fspagedetails.u.simple.trackdescr, PTS.FlowDirectionToFswdir(this._section.StructuralCache.PageFlowDirection));
				this._section.StructuralCache.CurrentArrangeContext.PopPageData();
				return;
			}
			ErrorHandler.Assert(fspagedetails.u.complex.cFootnoteColumns == 0, ErrorHandler.NotSupportedFootnotes);
			if (fspagedetails.u.complex.cSections != 0)
			{
				PTS.FSSECTIONDESCRIPTION[] array;
				PtsHelper.SectionListFromPage(this.PtsContext, this._ptsPage.Value, ref fspagedetails, out array);
				for (int i = 0; i < array.Length; i++)
				{
					this.ArrangeSection(ref array[i]);
				}
			}
		}

		// Token: 0x060069FD RID: 27133 RVA: 0x001E2870 File Offset: 0x001E0A70
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void UpdateViewport(ref PTS.FSRECT viewport)
		{
			if (!this.IsEmpty)
			{
				PTS.FSPAGEDETAILS fspagedetails;
				PTS.Validate(PTS.FsQueryPageDetails(this.PtsContext.Context, this._ptsPage.Value, out fspagedetails));
				if (PTS.ToBoolean(fspagedetails.fSimple))
				{
					PtsHelper.UpdateViewportTrack(this.PtsContext, ref fspagedetails.u.simple.trackdescr, ref viewport);
					return;
				}
				ErrorHandler.Assert(fspagedetails.u.complex.cFootnoteColumns == 0, ErrorHandler.NotSupportedFootnotes);
				if (fspagedetails.u.complex.cSections != 0)
				{
					PTS.FSSECTIONDESCRIPTION[] array;
					PtsHelper.SectionListFromPage(this.PtsContext, this._ptsPage.Value, ref fspagedetails, out array);
					for (int i = 0; i < array.Length; i++)
					{
						this.UpdateViewportSection(ref array[i], ref viewport);
					}
				}
			}
		}

		// Token: 0x060069FE RID: 27134 RVA: 0x001E293A File Offset: 0x001E0B3A
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void ClearUpdateInfo()
		{
			if (!this.IsEmpty)
			{
				PTS.Validate(PTS.FsClearUpdateInfoInPage(this.PtsContext.Context, this._ptsPage.Value), this.PtsContext);
			}
		}

		// Token: 0x060069FF RID: 27135 RVA: 0x001E296C File Offset: 0x001E0B6C
		internal ContainerVisual GetPageVisual()
		{
			if (this._visual == null)
			{
				this._visual = new ContainerVisual();
			}
			if (!this.IsEmpty)
			{
				this.UpdatePageVisuals(this._calculatedSize);
			}
			else
			{
				this._visual.Children.Clear();
			}
			return this._visual;
		}

		// Token: 0x17001973 RID: 6515
		// (get) Token: 0x06006A00 RID: 27136 RVA: 0x001E29B8 File Offset: 0x001E0BB8
		internal PageBreakRecord BreakRecord
		{
			get
			{
				return this._breakRecord;
			}
		}

		// Token: 0x17001974 RID: 6516
		// (get) Token: 0x06006A01 RID: 27137 RVA: 0x001E29C0 File Offset: 0x001E0BC0
		internal Size CalculatedSize
		{
			get
			{
				return this._calculatedSize;
			}
		}

		// Token: 0x17001975 RID: 6517
		// (get) Token: 0x06006A02 RID: 27138 RVA: 0x001E29C8 File Offset: 0x001E0BC8
		internal Size ContentSize
		{
			get
			{
				return this._contentSize;
			}
		}

		// Token: 0x17001976 RID: 6518
		// (get) Token: 0x06006A03 RID: 27139 RVA: 0x001E29D0 File Offset: 0x001E0BD0
		internal bool FinitePage
		{
			get
			{
				return this._finitePage;
			}
		}

		// Token: 0x17001977 RID: 6519
		// (get) Token: 0x06006A04 RID: 27140 RVA: 0x001E29D8 File Offset: 0x001E0BD8
		internal PageContext PageContext
		{
			get
			{
				return this._pageContextOfThisPage;
			}
		}

		// Token: 0x17001978 RID: 6520
		// (get) Token: 0x06006A05 RID: 27141 RVA: 0x001E29E0 File Offset: 0x001E0BE0
		internal bool IncrementalUpdate
		{
			get
			{
				return this._incrementalUpdate;
			}
		}

		// Token: 0x17001979 RID: 6521
		// (get) Token: 0x06006A06 RID: 27142 RVA: 0x001E29E8 File Offset: 0x001E0BE8
		internal PtsContext PtsContext
		{
			get
			{
				return this._section.PtsContext;
			}
		}

		// Token: 0x1700197A RID: 6522
		// (get) Token: 0x06006A07 RID: 27143 RVA: 0x001E29F5 File Offset: 0x001E0BF5
		internal IntPtr PageHandle
		{
			get
			{
				return this._ptsPage.Value;
			}
		}

		// Token: 0x1700197B RID: 6523
		// (get) Token: 0x06006A08 RID: 27144 RVA: 0x001E2A02 File Offset: 0x001E0C02
		// (set) Token: 0x06006A09 RID: 27145 RVA: 0x001E2A0A File Offset: 0x001E0C0A
		internal bool UseSizingWorkaroundForTextBox
		{
			get
			{
				return this._useSizingWorkaroundForTextBox;
			}
			set
			{
				this._useSizingWorkaroundForTextBox = value;
			}
		}

		// Token: 0x06006A0A RID: 27146 RVA: 0x001E2A14 File Offset: 0x001E0C14
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void Dispose(bool disposing)
		{
			if (Interlocked.CompareExchange(ref this._disposed, 1, 0) == 0)
			{
				if (!this.IsEmpty)
				{
					this._section.PtsContext.OnPageDisposed(this._ptsPage, disposing, true);
				}
				this._ptsPage.Value = IntPtr.Zero;
				this._breakRecord = null;
				this._visual = null;
				this._backgroundFormatOperation = null;
			}
		}

		// Token: 0x06006A0B RID: 27147 RVA: 0x001E2A78 File Offset: 0x001E0C78
		private void OnBeforeFormatPage(bool finitePage, bool incremental)
		{
			if (!incremental && !this.IsEmpty)
			{
				this.DestroyPage();
			}
			this._incrementalUpdate = incremental;
			this._finitePage = finitePage;
			this._breakRecord = null;
			this._pageContextOfThisPage.PageRect = new PTS.FSRECT(new Rect(this._section.StructuralCache.CurrentFormatContext.PageSize));
			if (this._backgroundFormatOperation != null)
			{
				this._backgroundFormatOperation.Abort();
			}
			if (!this._finitePage)
			{
				this._section.StructuralCache.BackgroundFormatInfo.UpdateBackgroundFormatInfo();
			}
		}

		// Token: 0x06006A0C RID: 27148 RVA: 0x001E2B08 File Offset: 0x001E0D08
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void OnAfterFormatPage(bool setSize, bool incremental)
		{
			if (setSize)
			{
				PTS.FSRECT rect = this.GetRect();
				PTS.FSBBOX boundingBox = this.GetBoundingBox();
				if (!this.FinitePage && PTS.ToBoolean(boundingBox.fDefined))
				{
					rect.dv = Math.Max(rect.dv, boundingBox.fsrc.dv);
				}
				this._calculatedSize.Width = Math.Max(TextDpi.MinWidth, TextDpi.FromTextDpi(rect.du));
				this._calculatedSize.Height = Math.Max(TextDpi.MinWidth, TextDpi.FromTextDpi(rect.dv));
				if (PTS.ToBoolean(boundingBox.fDefined))
				{
					this._contentSize.Width = Math.Max(Math.Max(TextDpi.FromTextDpi(boundingBox.fsrc.du), TextDpi.MinWidth), this._calculatedSize.Width);
					this._contentSize.Height = Math.Max(TextDpi.MinWidth, TextDpi.FromTextDpi(boundingBox.fsrc.dv));
					if (!this.FinitePage)
					{
						this._contentSize.Height = Math.Max(this._contentSize.Height, this._calculatedSize.Height);
					}
				}
				else
				{
					this._contentSize = this._calculatedSize;
				}
			}
			if (!this.IsEmpty && !incremental)
			{
				this.PtsContext.OnPageCreated(this._ptsPage);
			}
			if (this._section.StructuralCache != null)
			{
				this._section.StructuralCache.ClearUpdateInfo(false);
			}
		}

		// Token: 0x06006A0D RID: 27149 RVA: 0x001E2C7C File Offset: 0x001E0E7C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private PTS.FSRECT GetRect()
		{
			PTS.FSRECT result;
			if (this.IsEmpty)
			{
				result = default(PTS.FSRECT);
			}
			else
			{
				PTS.FSPAGEDETAILS fspagedetails;
				PTS.Validate(PTS.FsQueryPageDetails(this.PtsContext.Context, this._ptsPage.Value, out fspagedetails));
				if (PTS.ToBoolean(fspagedetails.fSimple))
				{
					result = fspagedetails.u.simple.trackdescr.fsrc;
				}
				else
				{
					ErrorHandler.Assert(fspagedetails.u.complex.cFootnoteColumns == 0, ErrorHandler.NotSupportedFootnotes);
					result = fspagedetails.u.complex.fsrcPageBody;
				}
			}
			return result;
		}

		// Token: 0x06006A0E RID: 27150 RVA: 0x001E2D14 File Offset: 0x001E0F14
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private PTS.FSBBOX GetBoundingBox()
		{
			PTS.FSBBOX result = default(PTS.FSBBOX);
			if (!this.IsEmpty)
			{
				PTS.FSPAGEDETAILS fspagedetails;
				PTS.Validate(PTS.FsQueryPageDetails(this.PtsContext.Context, this._ptsPage.Value, out fspagedetails));
				if (PTS.ToBoolean(fspagedetails.fSimple))
				{
					result = fspagedetails.u.simple.trackdescr.fsbbox;
				}
				else
				{
					ErrorHandler.Assert(fspagedetails.u.complex.cFootnoteColumns == 0, ErrorHandler.NotSupportedFootnotes);
					result = fspagedetails.u.complex.fsbboxPageBody;
				}
			}
			return result;
		}

		// Token: 0x06006A0F RID: 27151 RVA: 0x001E2DA8 File Offset: 0x001E0FA8
		[SecurityCritical]
		private void ArrangeSection(ref PTS.FSSECTIONDESCRIPTION sectionDesc)
		{
			PTS.FSSECTIONDETAILS fssectiondetails;
			PTS.Validate(PTS.FsQuerySectionDetails(this.PtsContext.Context, sectionDesc.pfssection, out fssectiondetails));
			if (PTS.ToBoolean(fssectiondetails.fFootnotesAsPagenotes))
			{
				ErrorHandler.Assert(fssectiondetails.u.withpagenotes.cEndnoteColumns == 0, ErrorHandler.NotSupportedFootnotes);
				if (fssectiondetails.u.withpagenotes.cBasicColumns != 0)
				{
					PTS.FSTRACKDESCRIPTION[] array;
					PtsHelper.TrackListFromSection(this.PtsContext, sectionDesc.pfssection, ref fssectiondetails, out array);
					for (int i = 0; i < array.Length; i++)
					{
						this._section.StructuralCache.CurrentArrangeContext.PushNewPageData(this._pageContextOfThisPage, array[i].fsrc, this._finitePage);
						PtsHelper.ArrangeTrack(this.PtsContext, ref array[i], fssectiondetails.u.withpagenotes.fswdir);
						this._section.StructuralCache.CurrentArrangeContext.PopPageData();
					}
					return;
				}
			}
			else
			{
				ErrorHandler.Assert(false, ErrorHandler.NotSupportedCompositeColumns);
			}
		}

		// Token: 0x06006A10 RID: 27152 RVA: 0x001E2EA8 File Offset: 0x001E10A8
		[SecurityCritical]
		private void UpdateViewportSection(ref PTS.FSSECTIONDESCRIPTION sectionDesc, ref PTS.FSRECT viewport)
		{
			PTS.FSSECTIONDETAILS fssectiondetails;
			PTS.Validate(PTS.FsQuerySectionDetails(this.PtsContext.Context, sectionDesc.pfssection, out fssectiondetails));
			if (PTS.ToBoolean(fssectiondetails.fFootnotesAsPagenotes))
			{
				ErrorHandler.Assert(fssectiondetails.u.withpagenotes.cEndnoteColumns == 0, ErrorHandler.NotSupportedFootnotes);
				if (fssectiondetails.u.withpagenotes.cBasicColumns != 0)
				{
					PTS.FSTRACKDESCRIPTION[] array;
					PtsHelper.TrackListFromSection(this.PtsContext, sectionDesc.pfssection, ref fssectiondetails, out array);
					for (int i = 0; i < array.Length; i++)
					{
						PtsHelper.UpdateViewportTrack(this.PtsContext, ref array[i], ref viewport);
					}
					return;
				}
			}
			else
			{
				ErrorHandler.Assert(false, ErrorHandler.NotSupportedCompositeColumns);
			}
		}

		// Token: 0x06006A11 RID: 27153 RVA: 0x001E2F50 File Offset: 0x001E1150
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void UpdatePageVisuals(Size arrangeSize)
		{
			Invariant.Assert(!this.IsEmpty);
			PTS.FSPAGEDETAILS fspagedetails;
			PTS.Validate(PTS.FsQueryPageDetails(this.PtsContext.Context, this._ptsPage.Value, out fspagedetails));
			if (fspagedetails.fskupd == PTS.FSKUPDATE.fskupdNoChange)
			{
				return;
			}
			ErrorHandler.Assert(fspagedetails.fskupd != PTS.FSKUPDATE.fskupdShifted, ErrorHandler.UpdateShiftedNotValid);
			if (this._visual.Children.Count != 2)
			{
				this._visual.Children.Clear();
				this._visual.Children.Add(new ContainerVisual());
				this._visual.Children.Add(new ContainerVisual());
			}
			ContainerVisual containerVisual = (ContainerVisual)this._visual.Children[0];
			ContainerVisual visual = (ContainerVisual)this._visual.Children[1];
			if (PTS.ToBoolean(fspagedetails.fSimple))
			{
				PTS.FSKUPDATE fskupd = fspagedetails.u.simple.trackdescr.fsupdinf.fskupd;
				if (fskupd == PTS.FSKUPDATE.fskupdInherited)
				{
					fskupd = fspagedetails.fskupd;
				}
				VisualCollection children = containerVisual.Children;
				if (fskupd == PTS.FSKUPDATE.fskupdNew)
				{
					children.Clear();
					children.Add(new ContainerVisual());
				}
				else if (children.Count == 1 && children[0] is SectionVisual)
				{
					children.Clear();
					children.Add(new ContainerVisual());
				}
				ContainerVisual containerVisual2 = (ContainerVisual)children[0];
				PtsHelper.UpdateTrackVisuals(this.PtsContext, containerVisual2.Children, fspagedetails.fskupd, ref fspagedetails.u.simple.trackdescr);
			}
			else
			{
				ErrorHandler.Assert(fspagedetails.u.complex.cFootnoteColumns == 0, ErrorHandler.NotSupportedFootnotes);
				bool flag = fspagedetails.u.complex.cSections == 0;
				if (!flag)
				{
					PTS.FSSECTIONDESCRIPTION[] array;
					PtsHelper.SectionListFromPage(this.PtsContext, this._ptsPage.Value, ref fspagedetails, out array);
					flag = (array.Length == 0);
					if (!flag)
					{
						ErrorHandler.Assert(array.Length == 1, ErrorHandler.NotSupportedMultiSection);
						VisualCollection children = containerVisual.Children;
						if (children.Count == 0)
						{
							children.Add(new SectionVisual());
						}
						else if (!(children[0] is SectionVisual))
						{
							children.Clear();
							children.Add(new SectionVisual());
						}
						this.UpdateSectionVisuals((SectionVisual)children[0], fspagedetails.fskupd, ref array[0]);
					}
				}
				if (flag)
				{
					containerVisual.Children.Clear();
				}
			}
			PtsHelper.UpdateFloatingElementVisuals(visual, this._pageContextOfThisPage.FloatingElementList);
		}

		// Token: 0x06006A12 RID: 27154 RVA: 0x001E31D4 File Offset: 0x001E13D4
		[SecurityCritical]
		private void UpdateSectionVisuals(SectionVisual visual, PTS.FSKUPDATE fskupdInherited, ref PTS.FSSECTIONDESCRIPTION sectionDesc)
		{
			PTS.FSKUPDATE fskupdate = sectionDesc.fsupdinf.fskupd;
			if (fskupdate == PTS.FSKUPDATE.fskupdInherited)
			{
				fskupdate = fskupdInherited;
			}
			ErrorHandler.Assert(fskupdate != PTS.FSKUPDATE.fskupdShifted, ErrorHandler.UpdateShiftedNotValid);
			if (fskupdate == PTS.FSKUPDATE.fskupdNoChange)
			{
				return;
			}
			PTS.FSSECTIONDETAILS fssectiondetails;
			PTS.Validate(PTS.FsQuerySectionDetails(this.PtsContext.Context, sectionDesc.pfssection, out fssectiondetails));
			bool flag;
			if (PTS.ToBoolean(fssectiondetails.fFootnotesAsPagenotes))
			{
				ErrorHandler.Assert(fssectiondetails.u.withpagenotes.cEndnoteColumns == 0, ErrorHandler.NotSupportedFootnotes);
				flag = (fssectiondetails.u.withpagenotes.cBasicColumns == 0);
				if (!flag)
				{
					PTS.FSTRACKDESCRIPTION[] array;
					PtsHelper.TrackListFromSection(this.PtsContext, sectionDesc.pfssection, ref fssectiondetails, out array);
					flag = (array.Length == 0);
					if (!flag)
					{
						ColumnPropertiesGroup columnProperties = new ColumnPropertiesGroup(this._section.Element);
						visual.DrawColumnRules(ref array, TextDpi.FromTextDpi(sectionDesc.fsrc.v), TextDpi.FromTextDpi(sectionDesc.fsrc.dv), columnProperties);
						VisualCollection children = visual.Children;
						if (fskupdate == PTS.FSKUPDATE.fskupdNew)
						{
							children.Clear();
							for (int i = 0; i < array.Length; i++)
							{
								children.Add(new ContainerVisual());
							}
						}
						ErrorHandler.Assert(children.Count == array.Length, ErrorHandler.ColumnVisualCountMismatch);
						for (int j = 0; j < array.Length; j++)
						{
							ContainerVisual containerVisual = (ContainerVisual)children[j];
							PtsHelper.UpdateTrackVisuals(this.PtsContext, containerVisual.Children, fskupdate, ref array[j]);
						}
					}
				}
			}
			else
			{
				ErrorHandler.Assert(false, ErrorHandler.NotSupportedCompositeColumns);
				flag = true;
			}
			if (flag)
			{
				visual.Children.Clear();
			}
		}

		// Token: 0x06006A13 RID: 27155 RVA: 0x001E336C File Offset: 0x001E156C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private IInputElement InputHitTestPage(PTS.FSPOINT pt)
		{
			IInputElement inputElement = null;
			if (this._pageContextOfThisPage.FloatingElementList != null)
			{
				int num = 0;
				while (num < this._pageContextOfThisPage.FloatingElementList.Count && inputElement == null)
				{
					BaseParaClient baseParaClient = this._pageContextOfThisPage.FloatingElementList[num];
					inputElement = baseParaClient.InputHitTest(pt);
					num++;
				}
			}
			if (inputElement == null)
			{
				PTS.FSPAGEDETAILS fspagedetails;
				PTS.Validate(PTS.FsQueryPageDetails(this.PtsContext.Context, this._ptsPage.Value, out fspagedetails));
				if (PTS.ToBoolean(fspagedetails.fSimple))
				{
					if (fspagedetails.u.simple.trackdescr.fsrc.Contains(pt))
					{
						inputElement = PtsHelper.InputHitTestTrack(this.PtsContext, pt, ref fspagedetails.u.simple.trackdescr);
					}
				}
				else
				{
					ErrorHandler.Assert(fspagedetails.u.complex.cFootnoteColumns == 0, ErrorHandler.NotSupportedFootnotes);
					if (fspagedetails.u.complex.cSections != 0)
					{
						PTS.FSSECTIONDESCRIPTION[] array;
						PtsHelper.SectionListFromPage(this.PtsContext, this._ptsPage.Value, ref fspagedetails, out array);
						int num2 = 0;
						while (num2 < array.Length && inputElement == null)
						{
							if (array[num2].fsrc.Contains(pt))
							{
								inputElement = this.InputHitTestSection(pt, ref array[num2]);
							}
							num2++;
						}
					}
				}
			}
			return inputElement;
		}

		// Token: 0x06006A14 RID: 27156 RVA: 0x001E34C0 File Offset: 0x001E16C0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private List<Rect> GetRectanglesInPage(ContentElement e, int start, int length)
		{
			List<Rect> list = new List<Rect>();
			Invariant.Assert(!this.IsEmpty);
			PTS.FSPAGEDETAILS fspagedetails;
			PTS.Validate(PTS.FsQueryPageDetails(this.PtsContext.Context, this._ptsPage.Value, out fspagedetails));
			if (PTS.ToBoolean(fspagedetails.fSimple))
			{
				list = PtsHelper.GetRectanglesInTrack(this.PtsContext, e, start, length, ref fspagedetails.u.simple.trackdescr);
			}
			else
			{
				ErrorHandler.Assert(fspagedetails.u.complex.cFootnoteColumns == 0, ErrorHandler.NotSupportedFootnotes);
				if (fspagedetails.u.complex.cSections != 0)
				{
					PTS.FSSECTIONDESCRIPTION[] array;
					PtsHelper.SectionListFromPage(this.PtsContext, this._ptsPage.Value, ref fspagedetails, out array);
					for (int i = 0; i < array.Length; i++)
					{
						list = this.GetRectanglesInSection(e, start, length, ref array[i]);
						Invariant.Assert(list != null);
						if (list.Count != 0)
						{
							break;
						}
					}
				}
				else
				{
					list = new List<Rect>();
				}
			}
			return list;
		}

		// Token: 0x06006A15 RID: 27157 RVA: 0x001E35B8 File Offset: 0x001E17B8
		[SecurityCritical]
		private IInputElement InputHitTestSection(PTS.FSPOINT pt, ref PTS.FSSECTIONDESCRIPTION sectionDesc)
		{
			IInputElement result = null;
			PTS.FSSECTIONDETAILS fssectiondetails;
			PTS.Validate(PTS.FsQuerySectionDetails(this.PtsContext.Context, sectionDesc.pfssection, out fssectiondetails));
			if (PTS.ToBoolean(fssectiondetails.fFootnotesAsPagenotes))
			{
				ErrorHandler.Assert(fssectiondetails.u.withpagenotes.cEndnoteColumns == 0, ErrorHandler.NotSupportedFootnotes);
				if (fssectiondetails.u.withpagenotes.cBasicColumns != 0)
				{
					PTS.FSTRACKDESCRIPTION[] array;
					PtsHelper.TrackListFromSection(this.PtsContext, sectionDesc.pfssection, ref fssectiondetails, out array);
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].fsrc.Contains(pt))
						{
							result = PtsHelper.InputHitTestTrack(this.PtsContext, pt, ref array[i]);
							break;
						}
					}
				}
			}
			else
			{
				ErrorHandler.Assert(false, ErrorHandler.NotSupportedCompositeColumns);
			}
			return result;
		}

		// Token: 0x06006A16 RID: 27158 RVA: 0x001E367C File Offset: 0x001E187C
		[SecurityCritical]
		private List<Rect> GetRectanglesInSection(ContentElement e, int start, int length, ref PTS.FSSECTIONDESCRIPTION sectionDesc)
		{
			PTS.FSSECTIONDETAILS fssectiondetails;
			PTS.Validate(PTS.FsQuerySectionDetails(this.PtsContext.Context, sectionDesc.pfssection, out fssectiondetails));
			List<Rect> list = new List<Rect>();
			if (PTS.ToBoolean(fssectiondetails.fFootnotesAsPagenotes))
			{
				ErrorHandler.Assert(fssectiondetails.u.withpagenotes.cEndnoteColumns == 0, ErrorHandler.NotSupportedFootnotes);
				if (fssectiondetails.u.withpagenotes.cBasicColumns != 0)
				{
					PTS.FSTRACKDESCRIPTION[] array;
					PtsHelper.TrackListFromSection(this.PtsContext, sectionDesc.pfssection, ref fssectiondetails, out array);
					for (int i = 0; i < array.Length; i++)
					{
						List<Rect> rectanglesInTrack = PtsHelper.GetRectanglesInTrack(this.PtsContext, e, start, length, ref array[i]);
						Invariant.Assert(rectanglesInTrack != null);
						if (rectanglesInTrack.Count != 0)
						{
							list.AddRange(rectanglesInTrack);
						}
					}
				}
			}
			else
			{
				ErrorHandler.Assert(false, ErrorHandler.NotSupportedCompositeColumns);
			}
			return list;
		}

		// Token: 0x06006A17 RID: 27159 RVA: 0x001E3750 File Offset: 0x001E1950
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void DestroyPage()
		{
			if (this._ptsPage.Value != IntPtr.Zero)
			{
				this.PtsContext.OnPageDisposed(this._ptsPage, true, false);
				this._ptsPage.Value = IntPtr.Zero;
			}
		}

		// Token: 0x1700197C RID: 6524
		// (get) Token: 0x06006A18 RID: 27160 RVA: 0x001E378C File Offset: 0x001E198C
		private bool IsEmpty
		{
			get
			{
				return this._ptsPage.Value == IntPtr.Zero;
			}
		}

		// Token: 0x0400341B RID: 13339
		private static DispatcherOperationCallback BackgroundUpdateCallback = new DispatcherOperationCallback(PtsPage.BackgroundFormatStatic);

		// Token: 0x0400341C RID: 13340
		private readonly Section _section;

		// Token: 0x0400341D RID: 13341
		private PageBreakRecord _breakRecord;

		// Token: 0x0400341E RID: 13342
		private ContainerVisual _visual;

		// Token: 0x0400341F RID: 13343
		private DispatcherOperation _backgroundFormatOperation;

		// Token: 0x04003420 RID: 13344
		private Size _calculatedSize;

		// Token: 0x04003421 RID: 13345
		private Size _contentSize;

		// Token: 0x04003422 RID: 13346
		private PageContext _pageContextOfThisPage = new PageContext();

		// Token: 0x04003423 RID: 13347
		private SecurityCriticalDataForSet<IntPtr> _ptsPage;

		// Token: 0x04003424 RID: 13348
		private bool _finitePage;

		// Token: 0x04003425 RID: 13349
		private bool _incrementalUpdate;

		// Token: 0x04003426 RID: 13350
		internal bool _useSizingWorkaroundForTextBox;

		// Token: 0x04003427 RID: 13351
		private int _disposed;
	}
}
