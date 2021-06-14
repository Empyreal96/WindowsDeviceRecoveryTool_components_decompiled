using System;
using System.Security;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000644 RID: 1604
	internal sealed class Section : UnmanagedHandle
	{
		// Token: 0x06006A40 RID: 27200 RVA: 0x001E3F5E File Offset: 0x001E215E
		internal Section(StructuralCache structuralCache) : base(structuralCache.PtsContext)
		{
			this._structuralCache = structuralCache;
		}

		// Token: 0x06006A41 RID: 27201 RVA: 0x001E3F73 File Offset: 0x001E2173
		public override void Dispose()
		{
			this.DestroyStructure();
			base.Dispose();
		}

		// Token: 0x06006A42 RID: 27202 RVA: 0x00118BF9 File Offset: 0x00116DF9
		internal void FSkipPage(out int fSkip)
		{
			fSkip = 0;
		}

		// Token: 0x06006A43 RID: 27203 RVA: 0x001E3F84 File Offset: 0x001E2184
		internal void GetPageDimensions(out uint fswdir, out int fHeaderFooterAtTopBottom, out int durPage, out int dvrPage, ref PTS.FSRECT fsrcMargin)
		{
			Size pageSize = this._structuralCache.CurrentFormatContext.PageSize;
			durPage = TextDpi.ToTextDpi(pageSize.Width);
			dvrPage = TextDpi.ToTextDpi(pageSize.Height);
			Thickness pageMargin = this._structuralCache.CurrentFormatContext.PageMargin;
			TextDpi.EnsureValidPageMargin(ref pageMargin, pageSize);
			fsrcMargin.u = TextDpi.ToTextDpi(pageMargin.Left);
			fsrcMargin.v = TextDpi.ToTextDpi(pageMargin.Top);
			fsrcMargin.du = durPage - TextDpi.ToTextDpi(pageMargin.Left + pageMargin.Right);
			fsrcMargin.dv = dvrPage - TextDpi.ToTextDpi(pageMargin.Top + pageMargin.Bottom);
			this.StructuralCache.PageFlowDirection = (FlowDirection)this._structuralCache.PropertyOwner.GetValue(FrameworkElement.FlowDirectionProperty);
			fswdir = PTS.FlowDirectionToFswdir(this.StructuralCache.PageFlowDirection);
			fHeaderFooterAtTopBottom = 0;
		}

		// Token: 0x06006A44 RID: 27204 RVA: 0x001E4075 File Offset: 0x001E2275
		[SecurityCritical]
		internal unsafe void GetJustificationProperties(IntPtr* rgnms, int cnms, int fLastSectionNotBroken, out int fJustify, out PTS.FSKALIGNPAGE fskal, out int fCancelAtLastColumn)
		{
			fJustify = 0;
			fCancelAtLastColumn = 0;
			fskal = PTS.FSKALIGNPAGE.fskalpgTop;
		}

		// Token: 0x06006A45 RID: 27205 RVA: 0x001E4083 File Offset: 0x001E2283
		internal void GetNextSection(out int fSuccess, out IntPtr nmsNext)
		{
			fSuccess = 0;
			nmsNext = IntPtr.Zero;
		}

		// Token: 0x06006A46 RID: 27206 RVA: 0x001E4090 File Offset: 0x001E2290
		internal void GetSectionProperties(out int fNewPage, out uint fswdir, out int fApplyColumnBalancing, out int ccol, out int cSegmentDefinedColumnSpanAreas, out int cHeightDefinedColumnSpanAreas)
		{
			ColumnPropertiesGroup columnProperties = new ColumnPropertiesGroup(this.Element);
			Size pageSize = this._structuralCache.CurrentFormatContext.PageSize;
			double lineHeightValue = DynamicPropertyReader.GetLineHeightValue(this.Element);
			Thickness pageMargin = this._structuralCache.CurrentFormatContext.PageMargin;
			double pageFontSize = (double)this._structuralCache.PropertyOwner.GetValue(TextElement.FontSizeProperty);
			FontFamily pageFontFamily = (FontFamily)this._structuralCache.PropertyOwner.GetValue(TextElement.FontFamilyProperty);
			bool finitePage = this._structuralCache.CurrentFormatContext.FinitePage;
			fNewPage = 0;
			fswdir = PTS.FlowDirectionToFswdir((FlowDirection)this._structuralCache.PropertyOwner.GetValue(FrameworkElement.FlowDirectionProperty));
			fApplyColumnBalancing = 0;
			ccol = PtsHelper.CalculateColumnCount(columnProperties, lineHeightValue, pageSize.Width - (pageMargin.Left + pageMargin.Right), pageFontSize, pageFontFamily, finitePage);
			cSegmentDefinedColumnSpanAreas = 0;
			cHeightDefinedColumnSpanAreas = 0;
		}

		// Token: 0x06006A47 RID: 27207 RVA: 0x001E4177 File Offset: 0x001E2377
		internal void GetMainTextSegment(out IntPtr nmSegment)
		{
			if (this._mainTextSegment == null)
			{
				this._mainTextSegment = new ContainerParagraph(this.Element, this._structuralCache);
			}
			nmSegment = this._mainTextSegment.Handle;
		}

		// Token: 0x06006A48 RID: 27208 RVA: 0x001E41A8 File Offset: 0x001E23A8
		internal void GetHeaderSegment(IntPtr pfsbrpagePrelim, uint fswdir, out int fHeaderPresent, out int fHardMargin, out int dvrMaxHeight, out int dvrFromEdge, out uint fswdirHeader, out IntPtr nmsHeader)
		{
			fHeaderPresent = 0;
			fHardMargin = 0;
			dvrMaxHeight = (dvrFromEdge = 0);
			fswdirHeader = fswdir;
			nmsHeader = IntPtr.Zero;
		}

		// Token: 0x06006A49 RID: 27209 RVA: 0x001E41D4 File Offset: 0x001E23D4
		internal void GetFooterSegment(IntPtr pfsbrpagePrelim, uint fswdir, out int fFooterPresent, out int fHardMargin, out int dvrMaxHeight, out int dvrFromEdge, out uint fswdirFooter, out IntPtr nmsFooter)
		{
			fFooterPresent = 0;
			fHardMargin = 0;
			dvrMaxHeight = (dvrFromEdge = 0);
			fswdirFooter = fswdir;
			nmsFooter = IntPtr.Zero;
		}

		// Token: 0x06006A4A RID: 27210 RVA: 0x001E4200 File Offset: 0x001E2400
		[SecurityCritical]
		internal unsafe void GetSectionColumnInfo(uint fswdir, int ncol, PTS.FSCOLUMNINFO* pfscolinfo, out int ccol)
		{
			ColumnPropertiesGroup columnProperties = new ColumnPropertiesGroup(this.Element);
			Size pageSize = this._structuralCache.CurrentFormatContext.PageSize;
			double lineHeightValue = DynamicPropertyReader.GetLineHeightValue(this.Element);
			Thickness pageMargin = this._structuralCache.CurrentFormatContext.PageMargin;
			double pageFontSize = (double)this._structuralCache.PropertyOwner.GetValue(TextElement.FontSizeProperty);
			FontFamily pageFontFamily = (FontFamily)this._structuralCache.PropertyOwner.GetValue(TextElement.FontFamilyProperty);
			bool finitePage = this._structuralCache.CurrentFormatContext.FinitePage;
			ccol = ncol;
			PtsHelper.GetColumnsInfo(columnProperties, lineHeightValue, pageSize.Width - (pageMargin.Left + pageMargin.Right), pageFontSize, pageFontFamily, ncol, pfscolinfo, finitePage);
		}

		// Token: 0x06006A4B RID: 27211 RVA: 0x001E4083 File Offset: 0x001E2283
		internal void GetEndnoteSegment(out int fEndnotesPresent, out IntPtr nmsEndnotes)
		{
			fEndnotesPresent = 0;
			nmsEndnotes = IntPtr.Zero;
		}

		// Token: 0x06006A4C RID: 27212 RVA: 0x001E42BB File Offset: 0x001E24BB
		internal void GetEndnoteSeparators(out IntPtr nmsEndnoteSeparator, out IntPtr nmsEndnoteContSeparator, out IntPtr nmsEndnoteContNotice)
		{
			nmsEndnoteSeparator = IntPtr.Zero;
			nmsEndnoteContSeparator = IntPtr.Zero;
			nmsEndnoteContNotice = IntPtr.Zero;
		}

		// Token: 0x06006A4D RID: 27213 RVA: 0x001E42D2 File Offset: 0x001E24D2
		internal void InvalidateFormatCache()
		{
			if (this._mainTextSegment != null)
			{
				this._mainTextSegment.InvalidateFormatCache();
			}
		}

		// Token: 0x06006A4E RID: 27214 RVA: 0x001E42E7 File Offset: 0x001E24E7
		internal void ClearUpdateInfo()
		{
			if (this._mainTextSegment != null)
			{
				this._mainTextSegment.ClearUpdateInfo();
			}
		}

		// Token: 0x06006A4F RID: 27215 RVA: 0x001E42FC File Offset: 0x001E24FC
		internal void InvalidateStructure()
		{
			if (this._mainTextSegment != null)
			{
				DtrList dtrList = this._structuralCache.DtrList;
				if (dtrList != null)
				{
					this._mainTextSegment.InvalidateStructure(dtrList[0].StartIndex);
				}
			}
		}

		// Token: 0x06006A50 RID: 27216 RVA: 0x001E433B File Offset: 0x001E253B
		internal void DestroyStructure()
		{
			if (this._mainTextSegment != null)
			{
				this._mainTextSegment.Dispose();
				this._mainTextSegment = null;
			}
		}

		// Token: 0x06006A51 RID: 27217 RVA: 0x001E4357 File Offset: 0x001E2557
		internal void UpdateSegmentLastFormatPositions()
		{
			if (this._mainTextSegment != null)
			{
				this._mainTextSegment.UpdateLastFormatPositions();
			}
		}

		// Token: 0x1700198B RID: 6539
		// (get) Token: 0x06006A52 RID: 27218 RVA: 0x001E436C File Offset: 0x001E256C
		internal bool CanUpdate
		{
			get
			{
				return this._mainTextSegment != null;
			}
		}

		// Token: 0x1700198C RID: 6540
		// (get) Token: 0x06006A53 RID: 27219 RVA: 0x001E4377 File Offset: 0x001E2577
		internal StructuralCache StructuralCache
		{
			get
			{
				return this._structuralCache;
			}
		}

		// Token: 0x1700198D RID: 6541
		// (get) Token: 0x06006A54 RID: 27220 RVA: 0x001E437F File Offset: 0x001E257F
		internal DependencyObject Element
		{
			get
			{
				return this._structuralCache.PropertyOwner;
			}
		}

		// Token: 0x04003434 RID: 13364
		private BaseParagraph _mainTextSegment;

		// Token: 0x04003435 RID: 13365
		private readonly StructuralCache _structuralCache;
	}
}
