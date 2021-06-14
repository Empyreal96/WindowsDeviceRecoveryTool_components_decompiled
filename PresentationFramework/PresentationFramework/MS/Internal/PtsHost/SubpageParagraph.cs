using System;
using System.Security;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000649 RID: 1609
	internal class SubpageParagraph : BaseParagraph
	{
		// Token: 0x06006A94 RID: 27284 RVA: 0x001E5A36 File Offset: 0x001E3C36
		internal SubpageParagraph(DependencyObject element, StructuralCache structuralCache) : base(element, structuralCache)
		{
		}

		// Token: 0x06006A95 RID: 27285 RVA: 0x001E5A47 File Offset: 0x001E3C47
		public override void Dispose()
		{
			base.Dispose();
			if (this._mainTextSegment != null)
			{
				this._mainTextSegment.Dispose();
				this._mainTextSegment = null;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06006A96 RID: 27286 RVA: 0x001E5A6F File Offset: 0x001E3C6F
		internal override void GetParaProperties(ref PTS.FSPAP fspap)
		{
			base.GetParaProperties(ref fspap, false);
			fspap.idobj = PtsHost.SubpageParagraphId;
			if (this._mainTextSegment == null)
			{
				this._mainTextSegment = new ContainerParagraph(this._element, this._structuralCache);
			}
		}

		// Token: 0x06006A97 RID: 27287 RVA: 0x001E5AA4 File Offset: 0x001E3CA4
		internal override void CreateParaclient(out IntPtr paraClientHandle)
		{
			SubpageParaClient subpageParaClient = new SubpageParaClient(this);
			paraClientHandle = subpageParaClient.Handle;
		}

		// Token: 0x06006A98 RID: 27288 RVA: 0x001E5AC0 File Offset: 0x001E3CC0
		[SecurityCritical]
		internal unsafe void FormatParaFinite(SubpageParaClient paraClient, IntPtr pbrkrecIn, int fBRFromPreviousPage, IntPtr footnoteRejector, int fEmptyOk, int fSuppressTopSpace, uint fswdir, ref PTS.FSRECT fsrcToFill, MarginCollapsingState mcs, PTS.FSKCLEAR fskclearIn, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA fsksuppresshardbreakbeforefirstparaIn, out PTS.FSFMTR fsfmtr, out IntPtr pfspara, out IntPtr pbrkrecOut, out int dvrUsed, out PTS.FSBBOX fsbbox, out IntPtr pmcsclientOut, out PTS.FSKCLEAR fskclearOut, out int dvrTopSpace)
		{
			uint num = PTS.FlowDirectionToFswdir((FlowDirection)base.Element.GetValue(FrameworkElement.FlowDirectionProperty));
			if (mcs != null && pbrkrecIn != IntPtr.Zero)
			{
				mcs = null;
			}
			PTS.FSRECT fsrect = default(PTS.FSRECT);
			int num2 = fsrcToFill.du;
			int num3 = fsrcToFill.dv;
			Invariant.Assert(base.Element is TableCell || base.Element is AnchoredBlock);
			fskclearIn = PTS.WrapDirectionToFskclear((WrapDirection)base.Element.GetValue(Block.ClearFloatersProperty));
			int num4 = 0;
			MarginCollapsingState marginCollapsingState = null;
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Element, base.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (num != fswdir)
			{
				PTS.FSRECT pageRect = base.StructuralCache.CurrentFormatContext.PageRect;
				PTS.Validate(PTS.FsTransformRectangle(fswdir, ref pageRect, ref fsrcToFill, num, out fsrcToFill));
				mbpInfo.MirrorMargin();
			}
			num2 = Math.Max(1, num2 - (mbpInfo.MBPLeft + mbpInfo.MBPRight));
			if (pbrkrecIn == IntPtr.Zero)
			{
				MarginCollapsingState.CollapseTopMargin(base.PtsContext, mbpInfo, mcs, out marginCollapsingState, out num4);
				if (PTS.ToBoolean(fSuppressTopSpace))
				{
					num4 = 0;
				}
				num3 = Math.Max(1, num3 - (num4 + mbpInfo.BPTop));
				if (marginCollapsingState != null)
				{
					marginCollapsingState.Dispose();
					marginCollapsingState = null;
				}
			}
			fsrect.du = num2;
			fsrect.dv = num3;
			ColumnPropertiesGroup columnProperties = new ColumnPropertiesGroup(this._element);
			double lineHeightValue = DynamicPropertyReader.GetLineHeightValue(this._element);
			double pageFontSize = (double)this._structuralCache.PropertyOwner.GetValue(TextElement.FontSizeProperty);
			FontFamily pageFontFamily = (FontFamily)this._structuralCache.PropertyOwner.GetValue(TextElement.FontFamilyProperty);
			int num5 = PtsHelper.CalculateColumnCount(columnProperties, lineHeightValue, TextDpi.FromTextDpi(num2), pageFontSize, pageFontFamily, false);
			PTS.FSCOLUMNINFO[] array = new PTS.FSCOLUMNINFO[num5];
			fixed (PTS.FSCOLUMNINFO* ptr = array)
			{
				PtsHelper.GetColumnsInfo(columnProperties, lineHeightValue, TextDpi.FromTextDpi(num2), pageFontSize, pageFontFamily, num5, ptr, false);
			}
			base.StructuralCache.CurrentFormatContext.PushNewPageData(new Size(TextDpi.FromTextDpi(num2), TextDpi.FromTextDpi(num3)), default(Thickness), false, true);
			fixed (PTS.FSCOLUMNINFO* ptr2 = array)
			{
				PTS.Validate(PTS.FsCreateSubpageFinite(base.PtsContext.Context, pbrkrecIn, fBRFromPreviousPage, this._mainTextSegment.Handle, footnoteRejector, fEmptyOk, fSuppressTopSpace, fswdir, num2, num3, ref fsrect, num5, ptr2, 0, 0, null, null, 0, null, null, PTS.FromBoolean(false), fsksuppresshardbreakbeforefirstparaIn, out fsfmtr, out pfspara, out pbrkrecOut, out dvrUsed, out fsbbox, out pmcsclientOut, out dvrTopSpace), base.PtsContext);
			}
			base.StructuralCache.CurrentFormatContext.PopPageData();
			fskclearOut = PTS.FSKCLEAR.fskclearNone;
			if (PTS.ToBoolean(fsbbox.fDefined))
			{
				dvrUsed = Math.Max(dvrUsed, fsbbox.fsrc.dv + fsbbox.fsrc.v);
				fsrcToFill.du = Math.Max(fsrcToFill.du, fsbbox.fsrc.du + fsbbox.fsrc.u);
			}
			if (pbrkrecIn == IntPtr.Zero)
			{
				dvrTopSpace = ((mbpInfo.BPTop != 0) ? num4 : dvrTopSpace);
				dvrUsed += num4 + mbpInfo.BPTop;
			}
			if (pmcsclientOut != IntPtr.Zero)
			{
				marginCollapsingState = (base.PtsContext.HandleToObject(pmcsclientOut) as MarginCollapsingState);
				PTS.ValidateHandle(marginCollapsingState);
				pmcsclientOut = IntPtr.Zero;
			}
			if (fsfmtr.kstop >= PTS.FSFMTRKSTOP.fmtrNoProgressOutOfSpace)
			{
				dvrUsed = (dvrTopSpace = 0);
			}
			else
			{
				if (fsfmtr.kstop == PTS.FSFMTRKSTOP.fmtrGoalReached)
				{
					MarginCollapsingState marginCollapsingState2;
					int num6;
					MarginCollapsingState.CollapseBottomMargin(base.PtsContext, mbpInfo, marginCollapsingState, out marginCollapsingState2, out num6);
					pmcsclientOut = ((marginCollapsingState2 != null) ? marginCollapsingState2.Handle : IntPtr.Zero);
					if (pmcsclientOut == IntPtr.Zero)
					{
						dvrUsed += num6 + mbpInfo.BPBottom;
					}
				}
				fsbbox.fsrc.u = fsrcToFill.u + mbpInfo.MarginLeft;
				fsbbox.fsrc.v = fsrcToFill.v + dvrTopSpace;
				fsbbox.fsrc.du = Math.Max(fsrcToFill.du - (mbpInfo.MarginLeft + mbpInfo.MarginRight), 0);
				fsbbox.fsrc.dv = Math.Max(dvrUsed - dvrTopSpace, 0);
			}
			if (num != fswdir)
			{
				PTS.FSRECT pageRect2 = base.StructuralCache.CurrentFormatContext.PageRect;
				PTS.Validate(PTS.FsTransformBbox(num, ref pageRect2, ref fsbbox, fswdir, out fsbbox));
			}
			if (marginCollapsingState != null)
			{
				marginCollapsingState.Dispose();
				marginCollapsingState = null;
			}
			paraClient.SetChunkInfo(pbrkrecIn == IntPtr.Zero, pbrkrecOut == IntPtr.Zero);
		}

		// Token: 0x06006A99 RID: 27289 RVA: 0x001E5F78 File Offset: 0x001E4178
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal unsafe void FormatParaBottomless(SubpageParaClient paraClient, int fSuppressTopSpace, uint fswdir, int urTrack, int durTrack, int vrTrack, MarginCollapsingState mcs, PTS.FSKCLEAR fskclearIn, int fInterruptable, out PTS.FSFMTRBL fsfmtrbl, out IntPtr pfspara, out int dvrUsed, out PTS.FSBBOX fsbbox, out IntPtr pmcsclientOut, out PTS.FSKCLEAR fskclearOut, out int dvrTopSpace, out int fPageBecomesUninterruptable)
		{
			uint num = PTS.FlowDirectionToFswdir((FlowDirection)base.Element.GetValue(FrameworkElement.FlowDirectionProperty));
			int num2 = durTrack;
			int urMargin;
			int vrMargin = urMargin = 0;
			Invariant.Assert(base.Element is TableCell || base.Element is AnchoredBlock);
			fskclearIn = PTS.WrapDirectionToFskclear((WrapDirection)base.Element.GetValue(Block.ClearFloatersProperty));
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Element, base.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (num != fswdir)
			{
				PTS.FSRECT fsrect = new PTS.FSRECT(urTrack, 0, durTrack, 0);
				PTS.FSRECT pageRect = base.StructuralCache.CurrentFormatContext.PageRect;
				PTS.Validate(PTS.FsTransformRectangle(fswdir, ref pageRect, ref fsrect, num, out fsrect));
				urTrack = fsrect.u;
				durTrack = fsrect.du;
				mbpInfo.MirrorMargin();
			}
			num2 = Math.Max(1, num2 - (mbpInfo.MBPLeft + mbpInfo.MBPRight));
			MarginCollapsingState marginCollapsingState;
			int num3;
			MarginCollapsingState.CollapseTopMargin(base.PtsContext, mbpInfo, mcs, out marginCollapsingState, out num3);
			if (marginCollapsingState != null)
			{
				marginCollapsingState.Dispose();
				marginCollapsingState = null;
			}
			int durMargin = num2;
			ColumnPropertiesGroup columnProperties = new ColumnPropertiesGroup(this._element);
			double lineHeightValue = DynamicPropertyReader.GetLineHeightValue(this._element);
			double pageFontSize = (double)this._structuralCache.PropertyOwner.GetValue(TextElement.FontSizeProperty);
			FontFamily pageFontFamily = (FontFamily)this._structuralCache.PropertyOwner.GetValue(TextElement.FontFamilyProperty);
			int num4 = PtsHelper.CalculateColumnCount(columnProperties, lineHeightValue, TextDpi.FromTextDpi(num2), pageFontSize, pageFontFamily, false);
			PTS.FSCOLUMNINFO[] array = new PTS.FSCOLUMNINFO[num4];
			fixed (PTS.FSCOLUMNINFO* ptr = array)
			{
				PtsHelper.GetColumnsInfo(columnProperties, lineHeightValue, TextDpi.FromTextDpi(num2), pageFontSize, pageFontFamily, num4, ptr, false);
			}
			base.StructuralCache.CurrentFormatContext.PushNewPageData(new Size(TextDpi.FromTextDpi(num2), TextDpi.MaxWidth), default(Thickness), false, false);
			fixed (PTS.FSCOLUMNINFO* ptr2 = array)
			{
				PTS.Validate(PTS.FsCreateSubpageBottomless(base.PtsContext.Context, this._mainTextSegment.Handle, fSuppressTopSpace, fswdir, num2, urMargin, durMargin, vrMargin, num4, ptr2, 0, null, null, 0, null, null, PTS.FromBoolean(this._isInterruptible), out fsfmtrbl, out pfspara, out dvrUsed, out fsbbox, out pmcsclientOut, out dvrTopSpace, out fPageBecomesUninterruptable), base.PtsContext);
			}
			base.StructuralCache.CurrentFormatContext.PopPageData();
			fskclearOut = PTS.FSKCLEAR.fskclearNone;
			if (fsfmtrbl != PTS.FSFMTRBL.fmtrblCollision)
			{
				if (pmcsclientOut != IntPtr.Zero)
				{
					marginCollapsingState = (base.PtsContext.HandleToObject(pmcsclientOut) as MarginCollapsingState);
					PTS.ValidateHandle(marginCollapsingState);
					pmcsclientOut = IntPtr.Zero;
				}
				MarginCollapsingState marginCollapsingState2;
				int num5;
				MarginCollapsingState.CollapseBottomMargin(base.PtsContext, mbpInfo, marginCollapsingState, out marginCollapsingState2, out num5);
				pmcsclientOut = ((marginCollapsingState2 != null) ? marginCollapsingState2.Handle : IntPtr.Zero);
				if (marginCollapsingState != null)
				{
					marginCollapsingState.Dispose();
					marginCollapsingState = null;
				}
				if (PTS.ToBoolean(fsbbox.fDefined))
				{
					dvrUsed = Math.Max(dvrUsed, fsbbox.fsrc.dv + fsbbox.fsrc.v);
					durTrack = Math.Max(durTrack, fsbbox.fsrc.du + fsbbox.fsrc.u);
				}
				dvrTopSpace = ((mbpInfo.BPTop != 0) ? num3 : dvrTopSpace);
				dvrUsed += num3 + mbpInfo.BPTop + (num5 + mbpInfo.BPBottom);
				fsbbox.fsrc.u = urTrack + mbpInfo.MarginLeft;
				fsbbox.fsrc.v = vrTrack + dvrTopSpace;
				fsbbox.fsrc.du = Math.Max(durTrack - (mbpInfo.MarginLeft + mbpInfo.MarginRight), 0);
				fsbbox.fsrc.dv = Math.Max(dvrUsed - dvrTopSpace, 0);
			}
			else
			{
				pfspara = IntPtr.Zero;
				dvrUsed = (dvrTopSpace = 0);
			}
			if (num != fswdir)
			{
				PTS.FSRECT pageRect2 = base.StructuralCache.CurrentFormatContext.PageRect;
				PTS.Validate(PTS.FsTransformBbox(num, ref pageRect2, ref fsbbox, fswdir, out fsbbox));
			}
			paraClient.SetChunkInfo(true, true);
		}

		// Token: 0x06006A9A RID: 27290 RVA: 0x001E6394 File Offset: 0x001E4594
		[SecurityCritical]
		internal unsafe void UpdateBottomlessPara(IntPtr pfspara, SubpageParaClient paraClient, int fSuppressTopSpace, uint fswdir, int urTrack, int durTrack, int vrTrack, MarginCollapsingState mcs, PTS.FSKCLEAR fskclearIn, int fInterruptable, out PTS.FSFMTRBL fsfmtrbl, out int dvrUsed, out PTS.FSBBOX fsbbox, out IntPtr pmcsclientOut, out PTS.FSKCLEAR fskclearOut, out int dvrTopSpace, out int fPageBecomesUninterruptable)
		{
			uint num = PTS.FlowDirectionToFswdir((FlowDirection)base.Element.GetValue(FrameworkElement.FlowDirectionProperty));
			int num2 = durTrack;
			int urMargin;
			int vrMargin = urMargin = 0;
			Invariant.Assert(base.Element is TableCell || base.Element is AnchoredBlock);
			fskclearIn = PTS.WrapDirectionToFskclear((WrapDirection)base.Element.GetValue(Block.ClearFloatersProperty));
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Element, base.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (num != fswdir)
			{
				PTS.FSRECT fsrect = new PTS.FSRECT(urTrack, 0, durTrack, 0);
				PTS.FSRECT pageRect = base.StructuralCache.CurrentFormatContext.PageRect;
				PTS.Validate(PTS.FsTransformRectangle(fswdir, ref pageRect, ref fsrect, num, out fsrect));
				urTrack = fsrect.u;
				durTrack = fsrect.du;
				mbpInfo.MirrorMargin();
			}
			num2 = Math.Max(1, num2 - (mbpInfo.MBPLeft + mbpInfo.MBPRight));
			MarginCollapsingState marginCollapsingState;
			int num3;
			MarginCollapsingState.CollapseTopMargin(base.PtsContext, mbpInfo, mcs, out marginCollapsingState, out num3);
			if (marginCollapsingState != null)
			{
				marginCollapsingState.Dispose();
				marginCollapsingState = null;
			}
			int durMargin = num2;
			ColumnPropertiesGroup columnProperties = new ColumnPropertiesGroup(this._element);
			double lineHeightValue = DynamicPropertyReader.GetLineHeightValue(this._element);
			double pageFontSize = (double)this._structuralCache.PropertyOwner.GetValue(TextElement.FontSizeProperty);
			FontFamily pageFontFamily = (FontFamily)this._structuralCache.PropertyOwner.GetValue(TextElement.FontFamilyProperty);
			int num4 = PtsHelper.CalculateColumnCount(columnProperties, lineHeightValue, TextDpi.FromTextDpi(num2), pageFontSize, pageFontFamily, false);
			PTS.FSCOLUMNINFO[] array = new PTS.FSCOLUMNINFO[num4];
			fixed (PTS.FSCOLUMNINFO* ptr = array)
			{
				PtsHelper.GetColumnsInfo(columnProperties, lineHeightValue, TextDpi.FromTextDpi(num2), pageFontSize, pageFontFamily, num4, ptr, false);
			}
			base.StructuralCache.CurrentFormatContext.PushNewPageData(new Size(TextDpi.FromTextDpi(num2), TextDpi.MaxWidth), default(Thickness), true, false);
			fixed (PTS.FSCOLUMNINFO* ptr2 = array)
			{
				PTS.Validate(PTS.FsUpdateBottomlessSubpage(base.PtsContext.Context, pfspara, this._mainTextSegment.Handle, fSuppressTopSpace, fswdir, num2, urMargin, durMargin, vrMargin, num4, ptr2, 0, null, null, 0, null, null, PTS.FromBoolean(true), out fsfmtrbl, out dvrUsed, out fsbbox, out pmcsclientOut, out dvrTopSpace, out fPageBecomesUninterruptable), base.PtsContext);
			}
			base.StructuralCache.CurrentFormatContext.PopPageData();
			fskclearOut = PTS.FSKCLEAR.fskclearNone;
			if (fsfmtrbl != PTS.FSFMTRBL.fmtrblCollision)
			{
				if (pmcsclientOut != IntPtr.Zero)
				{
					marginCollapsingState = (base.PtsContext.HandleToObject(pmcsclientOut) as MarginCollapsingState);
					PTS.ValidateHandle(marginCollapsingState);
					pmcsclientOut = IntPtr.Zero;
				}
				MarginCollapsingState marginCollapsingState2;
				int num5;
				MarginCollapsingState.CollapseBottomMargin(base.PtsContext, mbpInfo, marginCollapsingState, out marginCollapsingState2, out num5);
				pmcsclientOut = ((marginCollapsingState2 != null) ? marginCollapsingState2.Handle : IntPtr.Zero);
				if (marginCollapsingState != null)
				{
					marginCollapsingState.Dispose();
					marginCollapsingState = null;
				}
				if (PTS.ToBoolean(fsbbox.fDefined))
				{
					dvrUsed = Math.Max(dvrUsed, fsbbox.fsrc.dv + fsbbox.fsrc.v);
					durTrack = Math.Max(durTrack, fsbbox.fsrc.du + fsbbox.fsrc.u);
				}
				dvrTopSpace = ((mbpInfo.BPTop != 0) ? num3 : dvrTopSpace);
				dvrUsed += num3 + mbpInfo.BPTop + (num5 + mbpInfo.BPBottom);
				fsbbox.fsrc.u = urTrack + mbpInfo.MarginLeft;
				fsbbox.fsrc.v = vrTrack + dvrTopSpace;
				fsbbox.fsrc.du = Math.Max(durTrack - (mbpInfo.MarginLeft + mbpInfo.MarginRight), 0);
				fsbbox.fsrc.dv = Math.Max(dvrUsed - dvrTopSpace, 0);
			}
			else
			{
				pfspara = IntPtr.Zero;
				dvrUsed = (dvrTopSpace = 0);
			}
			if (num != fswdir)
			{
				PTS.FSRECT pageRect2 = base.StructuralCache.CurrentFormatContext.PageRect;
				PTS.Validate(PTS.FsTransformBbox(num, ref pageRect2, ref fsbbox, fswdir, out fsbbox));
			}
			paraClient.SetChunkInfo(true, true);
		}

		// Token: 0x06006A9B RID: 27291 RVA: 0x001E67AB File Offset: 0x001E49AB
		internal override void ClearUpdateInfo()
		{
			if (this._mainTextSegment != null)
			{
				this._mainTextSegment.ClearUpdateInfo();
			}
			base.ClearUpdateInfo();
		}

		// Token: 0x06006A9C RID: 27292 RVA: 0x001E67C6 File Offset: 0x001E49C6
		internal override bool InvalidateStructure(int startPosition)
		{
			if (this._mainTextSegment != null && this._mainTextSegment.InvalidateStructure(startPosition))
			{
				this._mainTextSegment.Dispose();
				this._mainTextSegment = null;
			}
			return this._mainTextSegment == null;
		}

		// Token: 0x06006A9D RID: 27293 RVA: 0x001E67F9 File Offset: 0x001E49F9
		internal override void InvalidateFormatCache()
		{
			if (this._mainTextSegment != null)
			{
				this._mainTextSegment.InvalidateFormatCache();
			}
		}

		// Token: 0x06006A9E RID: 27294 RVA: 0x001E680E File Offset: 0x001E4A0E
		internal void UpdateSegmentLastFormatPositions()
		{
			this._mainTextSegment.UpdateLastFormatPositions();
		}

		// Token: 0x0400344D RID: 13389
		private BaseParagraph _mainTextSegment;

		// Token: 0x0400344E RID: 13390
		protected bool _isInterruptible = true;
	}
}
