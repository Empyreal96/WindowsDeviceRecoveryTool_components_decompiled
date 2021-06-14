using System;
using System.Security;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000624 RID: 1572
	internal sealed class FloaterParagraph : FloaterBaseParagraph
	{
		// Token: 0x0600681F RID: 26655 RVA: 0x001D4DBB File Offset: 0x001D2FBB
		internal FloaterParagraph(TextElement element, StructuralCache structuralCache) : base(element, structuralCache)
		{
		}

		// Token: 0x06006820 RID: 26656 RVA: 0x001D4DC5 File Offset: 0x001D2FC5
		internal override void UpdGetParaChange(out PTS.FSKCHANGE fskch, out int fNoFurtherChanges)
		{
			base.UpdGetParaChange(out fskch, out fNoFurtherChanges);
			fskch = PTS.FSKCHANGE.fskchNew;
		}

		// Token: 0x06006821 RID: 26657 RVA: 0x001D4DD2 File Offset: 0x001D2FD2
		public override void Dispose()
		{
			base.Dispose();
			if (this._mainTextSegment != null)
			{
				this._mainTextSegment.Dispose();
				this._mainTextSegment = null;
			}
		}

		// Token: 0x06006822 RID: 26658 RVA: 0x001D4DF4 File Offset: 0x001D2FF4
		internal override void CreateParaclient(out IntPtr paraClientHandle)
		{
			FloaterParaClient floaterParaClient = new FloaterParaClient(this);
			paraClientHandle = floaterParaClient.Handle;
			if (this._mainTextSegment == null)
			{
				this._mainTextSegment = new ContainerParagraph(base.Element, base.StructuralCache);
			}
		}

		// Token: 0x06006823 RID: 26659 RVA: 0x001D4E2F File Offset: 0x001D302F
		internal override void CollapseMargin(BaseParaClient paraClient, MarginCollapsingState mcs, uint fswdir, bool suppressTopSpace, out int dvr)
		{
			dvr = 0;
		}

		// Token: 0x06006824 RID: 26660 RVA: 0x001D4E38 File Offset: 0x001D3038
		internal override void GetFloaterProperties(uint fswdirTrack, out PTS.FSFLOATERPROPS fsfloaterprops)
		{
			fsfloaterprops = default(PTS.FSFLOATERPROPS);
			fsfloaterprops.fFloat = 1;
			fsfloaterprops.fskclear = PTS.WrapDirectionToFskclear((WrapDirection)base.Element.GetValue(Block.ClearFloatersProperty));
			switch (this.HorizontalAlignment)
			{
			case HorizontalAlignment.Center:
				fsfloaterprops.fskfloatalignment = PTS.FSKFLOATALIGNMENT.fskfloatalignCenter;
				goto IL_62;
			case HorizontalAlignment.Right:
				fsfloaterprops.fskfloatalignment = PTS.FSKFLOATALIGNMENT.fskfloatalignMax;
				goto IL_62;
			}
			fsfloaterprops.fskfloatalignment = PTS.FSKFLOATALIGNMENT.fskfloatalignMin;
			IL_62:
			fsfloaterprops.fskwr = PTS.WrapDirectionToFskwrap(this.WrapDirection);
			fsfloaterprops.fDelayNoProgress = 1;
		}

		// Token: 0x06006825 RID: 26661 RVA: 0x001D4EC0 File Offset: 0x001D30C0
		[SecurityCritical]
		internal override void FormatFloaterContentFinite(FloaterBaseParaClient paraClient, IntPtr pbrkrecIn, int fBRFromPreviousPage, IntPtr footnoteRejector, int fEmptyOk, int fSuppressTopSpace, uint fswdir, int fAtMaxWidth, int durAvailable, int dvrAvailable, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA fsksuppresshardbreakbeforefirstparaIn, out PTS.FSFMTR fsfmtr, out IntPtr pfsFloatContent, out IntPtr pbrkrecOut, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices)
		{
			uint num = PTS.FlowDirectionToFswdir((FlowDirection)base.Element.GetValue(FrameworkElement.FlowDirectionProperty));
			Invariant.Assert(paraClient is FloaterParaClient);
			if (this.IsFloaterRejected(PTS.ToBoolean(fAtMaxWidth), TextDpi.FromTextDpi(durAvailable)))
			{
				durFloaterWidth = (dvrFloaterHeight = 0);
				cPolygons = (cVertices = 0);
				fsfmtr = default(PTS.FSFMTR);
				fsfmtr.kstop = PTS.FSFMTRKSTOP.fmtrNoProgressOutOfSpace;
				fsfmtr.fContainsItemThatStoppedBeforeFootnote = 0;
				fsfmtr.fForcedProgress = 0;
				fsbbox = default(PTS.FSBBOX);
				fsbbox.fDefined = 0;
				pbrkrecOut = IntPtr.Zero;
				pfsFloatContent = IntPtr.Zero;
			}
			else
			{
				if (!base.StructuralCache.CurrentFormatContext.FinitePage)
				{
					if (double.IsInfinity(base.StructuralCache.CurrentFormatContext.PageHeight))
					{
						if (dvrAvailable > 1073741823)
						{
							dvrAvailable = Math.Min(dvrAvailable, 1073741823);
							fEmptyOk = 0;
						}
					}
					else
					{
						dvrAvailable = Math.Min(dvrAvailable, TextDpi.ToTextDpi(base.StructuralCache.CurrentFormatContext.PageHeight));
					}
				}
				MbpInfo mbpInfo = MbpInfo.FromElement(base.Element, base.StructuralCache.TextFormatterHost.PixelsPerDip);
				double num2 = this.CalculateWidth(TextDpi.FromTextDpi(durAvailable));
				int num3;
				this.AdjustDurAvailable(num2, ref durAvailable, out num3);
				int num4 = Math.Max(1, dvrAvailable - (mbpInfo.MBPTop + mbpInfo.MBPBottom));
				PTS.FSRECT fsrect = default(PTS.FSRECT);
				fsrect.du = num3;
				fsrect.dv = num4;
				int num5 = 1;
				PTS.FSCOLUMNINFO[] array = new PTS.FSCOLUMNINFO[num5];
				array[0].durBefore = 0;
				array[0].durWidth = num3;
				IntPtr zero;
				int num6;
				this.CreateSubpageFiniteHelper(base.PtsContext, pbrkrecIn, fBRFromPreviousPage, this._mainTextSegment.Handle, footnoteRejector, fEmptyOk, 1, fswdir, num3, num4, ref fsrect, num5, array, 0, fsksuppresshardbreakbeforefirstparaIn, out fsfmtr, out pfsFloatContent, out pbrkrecOut, out dvrFloaterHeight, out fsbbox, out zero, out num6);
				if (fsfmtr.kstop >= PTS.FSFMTRKSTOP.fmtrNoProgressOutOfSpace)
				{
					durFloaterWidth = (dvrFloaterHeight = 0);
					cPolygons = (cVertices = 0);
				}
				else
				{
					if (PTS.ToBoolean(fsbbox.fDefined))
					{
						if (fsbbox.fsrc.du < num3 && double.IsNaN(num2) && this.HorizontalAlignment != HorizontalAlignment.Stretch)
						{
							if (pfsFloatContent != IntPtr.Zero)
							{
								PTS.Validate(PTS.FsDestroySubpage(base.PtsContext.Context, pfsFloatContent), base.PtsContext);
								pfsFloatContent = IntPtr.Zero;
							}
							if (pbrkrecOut != IntPtr.Zero)
							{
								PTS.Validate(PTS.FsDestroySubpageBreakRecord(base.PtsContext.Context, pbrkrecOut), base.PtsContext);
								pbrkrecOut = IntPtr.Zero;
							}
							if (zero != IntPtr.Zero)
							{
								MarginCollapsingState marginCollapsingState = base.PtsContext.HandleToObject(zero) as MarginCollapsingState;
								PTS.ValidateHandle(marginCollapsingState);
								marginCollapsingState.Dispose();
								zero = IntPtr.Zero;
							}
							num3 = fsbbox.fsrc.du + 1;
							fsrect.du = num3;
							fsrect.dv = num4;
							array[0].durWidth = num3;
							this.CreateSubpageFiniteHelper(base.PtsContext, pbrkrecIn, fBRFromPreviousPage, this._mainTextSegment.Handle, footnoteRejector, fEmptyOk, 1, fswdir, num3, num4, ref fsrect, num5, array, 0, fsksuppresshardbreakbeforefirstparaIn, out fsfmtr, out pfsFloatContent, out pbrkrecOut, out dvrFloaterHeight, out fsbbox, out zero, out num6);
						}
					}
					else
					{
						num3 = TextDpi.ToTextDpi(TextDpi.MinWidth);
					}
					if (zero != IntPtr.Zero)
					{
						MarginCollapsingState marginCollapsingState2 = base.PtsContext.HandleToObject(zero) as MarginCollapsingState;
						PTS.ValidateHandle(marginCollapsingState2);
						marginCollapsingState2.Dispose();
						zero = IntPtr.Zero;
					}
					durFloaterWidth = num3 + mbpInfo.MBPLeft + mbpInfo.MBPRight;
					dvrFloaterHeight += mbpInfo.MBPTop + mbpInfo.MBPBottom;
					fsbbox.fsrc.u = 0;
					fsbbox.fsrc.v = 0;
					fsbbox.fsrc.du = durFloaterWidth;
					fsbbox.fsrc.dv = dvrFloaterHeight;
					fsbbox.fDefined = 1;
					cPolygons = (cVertices = 0);
					if (durFloaterWidth > durAvailable || dvrFloaterHeight > dvrAvailable)
					{
						if (PTS.ToBoolean(fEmptyOk))
						{
							if (pfsFloatContent != IntPtr.Zero)
							{
								PTS.Validate(PTS.FsDestroySubpage(base.PtsContext.Context, pfsFloatContent), base.PtsContext);
								pfsFloatContent = IntPtr.Zero;
							}
							if (pbrkrecOut != IntPtr.Zero)
							{
								PTS.Validate(PTS.FsDestroySubpageBreakRecord(base.PtsContext.Context, pbrkrecOut), base.PtsContext);
								pbrkrecOut = IntPtr.Zero;
							}
							cPolygons = (cVertices = 0);
							fsfmtr.kstop = PTS.FSFMTRKSTOP.fmtrNoProgressOutOfSpace;
						}
						else
						{
							fsfmtr.fForcedProgress = 1;
						}
					}
				}
			}
			((FloaterParaClient)paraClient).SubpageHandle = pfsFloatContent;
		}

		// Token: 0x06006826 RID: 26662 RVA: 0x001D536C File Offset: 0x001D356C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override void FormatFloaterContentBottomless(FloaterBaseParaClient paraClient, int fSuppressTopSpace, uint fswdir, int fAtMaxWidth, int durAvailable, int dvrAvailable, out PTS.FSFMTRBL fsfmtrbl, out IntPtr pfsFloatContent, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices)
		{
			uint num = PTS.FlowDirectionToFswdir((FlowDirection)base.Element.GetValue(FrameworkElement.FlowDirectionProperty));
			Invariant.Assert(paraClient is FloaterParaClient);
			if (this.IsFloaterRejected(PTS.ToBoolean(fAtMaxWidth), TextDpi.FromTextDpi(durAvailable)))
			{
				durFloaterWidth = durAvailable + 1;
				dvrFloaterHeight = dvrAvailable + 1;
				cPolygons = (cVertices = 0);
				fsfmtrbl = PTS.FSFMTRBL.fmtrblInterrupted;
				fsbbox = default(PTS.FSBBOX);
				fsbbox.fDefined = 0;
				pfsFloatContent = IntPtr.Zero;
			}
			else
			{
				MbpInfo mbpInfo = MbpInfo.FromElement(base.Element, base.StructuralCache.TextFormatterHost.PixelsPerDip);
				double num2 = this.CalculateWidth(TextDpi.FromTextDpi(durAvailable));
				int num3;
				this.AdjustDurAvailable(num2, ref durAvailable, out num3);
				int durMargin = num3;
				int urMargin;
				int vrMargin = urMargin = 0;
				int num4 = 1;
				PTS.FSCOLUMNINFO[] array = new PTS.FSCOLUMNINFO[num4];
				array[0].durBefore = 0;
				array[0].durWidth = num3;
				this.InvalidateMainTextSegment();
				IntPtr zero;
				int num5;
				int num6;
				this.CreateSubpageBottomlessHelper(base.PtsContext, this._mainTextSegment.Handle, 1, fswdir, num3, urMargin, durMargin, vrMargin, num4, array, out fsfmtrbl, out pfsFloatContent, out dvrFloaterHeight, out fsbbox, out zero, out num5, out num6);
				if (fsfmtrbl != PTS.FSFMTRBL.fmtrblCollision)
				{
					if (PTS.ToBoolean(fsbbox.fDefined))
					{
						if (fsbbox.fsrc.du < num3 && double.IsNaN(num2) && this.HorizontalAlignment != HorizontalAlignment.Stretch)
						{
							if (pfsFloatContent != IntPtr.Zero)
							{
								PTS.Validate(PTS.FsDestroySubpage(base.PtsContext.Context, pfsFloatContent), base.PtsContext);
							}
							if (zero != IntPtr.Zero)
							{
								MarginCollapsingState marginCollapsingState = base.PtsContext.HandleToObject(zero) as MarginCollapsingState;
								PTS.ValidateHandle(marginCollapsingState);
								marginCollapsingState.Dispose();
								zero = IntPtr.Zero;
							}
							durMargin = (num3 = fsbbox.fsrc.du + 1);
							array[0].durWidth = num3;
							this.CreateSubpageBottomlessHelper(base.PtsContext, this._mainTextSegment.Handle, 1, fswdir, num3, urMargin, durMargin, vrMargin, num4, array, out fsfmtrbl, out pfsFloatContent, out dvrFloaterHeight, out fsbbox, out zero, out num5, out num6);
						}
					}
					else
					{
						num3 = TextDpi.ToTextDpi(TextDpi.MinWidth);
					}
					if (zero != IntPtr.Zero)
					{
						MarginCollapsingState marginCollapsingState2 = base.PtsContext.HandleToObject(zero) as MarginCollapsingState;
						PTS.ValidateHandle(marginCollapsingState2);
						marginCollapsingState2.Dispose();
						zero = IntPtr.Zero;
					}
					durFloaterWidth = num3 + mbpInfo.MBPLeft + mbpInfo.MBPRight;
					dvrFloaterHeight += mbpInfo.MBPTop + mbpInfo.MBPBottom;
					if (dvrFloaterHeight > dvrAvailable || (durFloaterWidth > durAvailable && !PTS.ToBoolean(fAtMaxWidth)))
					{
						if (pfsFloatContent != IntPtr.Zero)
						{
							PTS.Validate(PTS.FsDestroySubpage(base.PtsContext.Context, pfsFloatContent), base.PtsContext);
						}
						cPolygons = (cVertices = 0);
						pfsFloatContent = IntPtr.Zero;
					}
					else
					{
						fsbbox.fsrc.u = 0;
						fsbbox.fsrc.v = 0;
						fsbbox.fsrc.du = durFloaterWidth;
						fsbbox.fsrc.dv = dvrFloaterHeight;
						cPolygons = (cVertices = 0);
					}
				}
				else
				{
					durFloaterWidth = (dvrFloaterHeight = 0);
					cPolygons = (cVertices = 0);
					pfsFloatContent = IntPtr.Zero;
				}
			}
			((FloaterParaClient)paraClient).SubpageHandle = pfsFloatContent;
		}

		// Token: 0x06006827 RID: 26663 RVA: 0x001D56C4 File Offset: 0x001D38C4
		internal override void UpdateBottomlessFloaterContent(FloaterBaseParaClient paraClient, int fSuppressTopSpace, uint fswdir, int fAtMaxWidth, int durAvailable, int dvrAvailable, IntPtr pfsFloatContent, out PTS.FSFMTRBL fsfmtrbl, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices)
		{
			fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
			durFloaterWidth = (dvrFloaterHeight = (cPolygons = (cVertices = 0)));
			fsbbox = default(PTS.FSBBOX);
			Invariant.Assert(false, "No appropriate handling for update in attached object floater.");
		}

		// Token: 0x06006828 RID: 26664 RVA: 0x001D56FE File Offset: 0x001D38FE
		internal override void GetMCSClientAfterFloater(uint fswdirTrack, MarginCollapsingState mcs, out IntPtr pmcsclientOut)
		{
			if (mcs != null)
			{
				pmcsclientOut = mcs.Handle;
				return;
			}
			pmcsclientOut = IntPtr.Zero;
		}

		// Token: 0x06006829 RID: 26665 RVA: 0x001D5713 File Offset: 0x001D3913
		internal override void ClearUpdateInfo()
		{
			if (this._mainTextSegment != null)
			{
				this._mainTextSegment.ClearUpdateInfo();
			}
			base.ClearUpdateInfo();
		}

		// Token: 0x0600682A RID: 26666 RVA: 0x001D572E File Offset: 0x001D392E
		internal override bool InvalidateStructure(int startPosition)
		{
			if (this._mainTextSegment != null && this._mainTextSegment.InvalidateStructure(startPosition))
			{
				this._mainTextSegment.Dispose();
				this._mainTextSegment = null;
			}
			return this._mainTextSegment == null;
		}

		// Token: 0x0600682B RID: 26667 RVA: 0x001D5761 File Offset: 0x001D3961
		internal override void InvalidateFormatCache()
		{
			if (this._mainTextSegment != null)
			{
				this._mainTextSegment.InvalidateFormatCache();
			}
		}

		// Token: 0x0600682C RID: 26668 RVA: 0x001D5776 File Offset: 0x001D3976
		internal void UpdateSegmentLastFormatPositions()
		{
			this._mainTextSegment.UpdateLastFormatPositions();
		}

		// Token: 0x0600682D RID: 26669 RVA: 0x001D5784 File Offset: 0x001D3984
		private void AdjustDurAvailable(double specifiedWidth, ref int durAvailable, out int subpageWidth)
		{
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Element, base.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (double.IsNaN(specifiedWidth))
			{
				subpageWidth = Math.Max(1, durAvailable - (mbpInfo.MBPLeft + mbpInfo.MBPRight));
				return;
			}
			TextDpi.EnsureValidPageWidth(ref specifiedWidth);
			int num = TextDpi.ToTextDpi(specifiedWidth);
			if (num + mbpInfo.MarginRight + mbpInfo.MarginLeft <= durAvailable)
			{
				durAvailable = num + mbpInfo.MarginLeft + mbpInfo.MarginRight;
				subpageWidth = Math.Max(1, num - (mbpInfo.BPLeft + mbpInfo.BPRight));
				return;
			}
			subpageWidth = Math.Max(1, durAvailable - (mbpInfo.MBPLeft + mbpInfo.MBPRight));
		}

		// Token: 0x0600682E RID: 26670 RVA: 0x001D5830 File Offset: 0x001D3A30
		[SecurityCritical]
		private unsafe void CreateSubpageFiniteHelper(PtsContext ptsContext, IntPtr brParaIn, int fFromPreviousPage, IntPtr nSeg, IntPtr pFtnRej, int fEmptyOk, int fSuppressTopSpace, uint fswdir, int lWidth, int lHeight, ref PTS.FSRECT rcMargin, int cColumns, PTS.FSCOLUMNINFO[] columnInfoCollection, int fApplyColumnBalancing, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA fsksuppresshardbreakbeforefirstparaIn, out PTS.FSFMTR fsfmtr, out IntPtr pSubPage, out IntPtr brParaOut, out int dvrUsed, out PTS.FSBBOX fsBBox, out IntPtr pfsMcsClient, out int topSpace)
		{
			base.StructuralCache.CurrentFormatContext.PushNewPageData(new Size(TextDpi.FromTextDpi(lWidth), TextDpi.FromTextDpi(lHeight)), default(Thickness), false, true);
			fixed (PTS.FSCOLUMNINFO* ptr = columnInfoCollection)
			{
				PTS.Validate(PTS.FsCreateSubpageFinite(ptsContext.Context, brParaIn, fFromPreviousPage, nSeg, pFtnRej, fEmptyOk, fSuppressTopSpace, fswdir, lWidth, lHeight, ref rcMargin, cColumns, ptr, 0, 0, null, null, 0, null, null, 0, fsksuppresshardbreakbeforefirstparaIn, out fsfmtr, out pSubPage, out brParaOut, out dvrUsed, out fsBBox, out pfsMcsClient, out topSpace), ptsContext);
			}
			base.StructuralCache.CurrentFormatContext.PopPageData();
		}

		// Token: 0x0600682F RID: 26671 RVA: 0x001D58DC File Offset: 0x001D3ADC
		[SecurityCritical]
		private unsafe void CreateSubpageBottomlessHelper(PtsContext ptsContext, IntPtr nSeg, int fSuppressTopSpace, uint fswdir, int lWidth, int urMargin, int durMargin, int vrMargin, int cColumns, PTS.FSCOLUMNINFO[] columnInfoCollection, out PTS.FSFMTRBL pfsfmtr, out IntPtr ppSubPage, out int pdvrUsed, out PTS.FSBBOX pfsBBox, out IntPtr pfsMcsClient, out int pTopSpace, out int fPageBecomesUninterruptible)
		{
			base.StructuralCache.CurrentFormatContext.PushNewPageData(new Size(TextDpi.FromTextDpi(lWidth), TextDpi.MaxWidth), default(Thickness), false, false);
			fixed (PTS.FSCOLUMNINFO* ptr = columnInfoCollection)
			{
				PTS.Validate(PTS.FsCreateSubpageBottomless(ptsContext.Context, nSeg, fSuppressTopSpace, fswdir, lWidth, urMargin, durMargin, vrMargin, cColumns, ptr, 0, null, null, 0, null, null, 0, out pfsfmtr, out ppSubPage, out pdvrUsed, out pfsBBox, out pfsMcsClient, out pTopSpace, out fPageBecomesUninterruptible), ptsContext);
			}
			base.StructuralCache.CurrentFormatContext.PopPageData();
		}

		// Token: 0x06006830 RID: 26672 RVA: 0x001D597C File Offset: 0x001D3B7C
		private void InvalidateMainTextSegment()
		{
			DtrList dtrList = base.StructuralCache.DtrsFromRange(base.ParagraphStartCharacterPosition, base.LastFormatCch);
			if (dtrList != null && dtrList.Length > 0)
			{
				this._mainTextSegment.InvalidateStructure(dtrList[0].StartIndex);
			}
		}

		// Token: 0x17001921 RID: 6433
		// (get) Token: 0x06006831 RID: 26673 RVA: 0x001D59C8 File Offset: 0x001D3BC8
		private HorizontalAlignment HorizontalAlignment
		{
			get
			{
				if (base.Element is Floater)
				{
					return ((Floater)base.Element).HorizontalAlignment;
				}
				Figure figure = (Figure)base.Element;
				FigureHorizontalAnchor horizontalAnchor = figure.HorizontalAnchor;
				if (horizontalAnchor == FigureHorizontalAnchor.PageLeft || horizontalAnchor == FigureHorizontalAnchor.ContentLeft || horizontalAnchor == FigureHorizontalAnchor.ColumnLeft)
				{
					return HorizontalAlignment.Left;
				}
				if (horizontalAnchor == FigureHorizontalAnchor.PageRight || horizontalAnchor == FigureHorizontalAnchor.ContentRight || horizontalAnchor == FigureHorizontalAnchor.ColumnRight)
				{
					return HorizontalAlignment.Right;
				}
				if (horizontalAnchor == FigureHorizontalAnchor.PageCenter || horizontalAnchor != FigureHorizontalAnchor.ContentCenter)
				{
				}
				return HorizontalAlignment.Center;
			}
		}

		// Token: 0x17001922 RID: 6434
		// (get) Token: 0x06006832 RID: 26674 RVA: 0x001D5A30 File Offset: 0x001D3C30
		private WrapDirection WrapDirection
		{
			get
			{
				if (!(base.Element is Floater))
				{
					Figure figure = (Figure)base.Element;
					return figure.WrapDirection;
				}
				if (this.HorizontalAlignment == HorizontalAlignment.Stretch)
				{
					return WrapDirection.None;
				}
				return WrapDirection.Both;
			}
		}

		// Token: 0x06006833 RID: 26675 RVA: 0x001D5A6C File Offset: 0x001D3C6C
		private double CalculateWidth(double spaceAvailable)
		{
			if (base.Element is Floater)
			{
				return ((Floater)base.Element).Width;
			}
			bool flag;
			double val = FigureHelper.CalculateFigureWidth(base.StructuralCache, (Figure)base.Element, ((Figure)base.Element).Width, out flag);
			if (flag)
			{
				return double.NaN;
			}
			return Math.Min(val, spaceAvailable);
		}

		// Token: 0x06006834 RID: 26676 RVA: 0x001D5AD8 File Offset: 0x001D3CD8
		private bool IsFloaterRejected(bool fAtMaxWidth, double availableSpace)
		{
			if (fAtMaxWidth)
			{
				return false;
			}
			if (base.Element is Floater && this.HorizontalAlignment != HorizontalAlignment.Stretch)
			{
				return false;
			}
			if (base.Element is Figure)
			{
				FigureLength width = ((Figure)base.Element).Width;
				if (width.IsAuto)
				{
					return false;
				}
				if (width.IsAbsolute && width.Value < availableSpace)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040033C7 RID: 13255
		private BaseParagraph _mainTextSegment;
	}
}
