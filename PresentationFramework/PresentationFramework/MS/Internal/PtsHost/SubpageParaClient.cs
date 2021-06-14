using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000648 RID: 1608
	internal class SubpageParaClient : BaseParaClient
	{
		// Token: 0x06006A83 RID: 27267 RVA: 0x001E4947 File Offset: 0x001E2B47
		internal SubpageParaClient(SubpageParagraph paragraph) : base(paragraph)
		{
		}

		// Token: 0x06006A84 RID: 27268 RVA: 0x001E495C File Offset: 0x001E2B5C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public override void Dispose()
		{
			this._visual = null;
			if (this._paraHandle.Value != IntPtr.Zero)
			{
				PTS.Validate(PTS.FsDestroySubpage(base.PtsContext.Context, this._paraHandle.Value));
				this._paraHandle.Value = IntPtr.Zero;
			}
			base.Dispose();
			GC.SuppressFinalize(this);
		}

		// Token: 0x06006A85 RID: 27269 RVA: 0x001E49C4 File Offset: 0x001E2BC4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override void OnArrange()
		{
			base.OnArrange();
			((SubpageParagraph)base.Paragraph).UpdateSegmentLastFormatPositions();
			PTS.FSSUBPAGEDETAILS fssubpagedetails;
			PTS.Validate(PTS.FsQuerySubpageDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubpagedetails));
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Paragraph.Element, base.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (base.ThisFlowDirection != base.PageFlowDirection)
			{
				mbpInfo.MirrorBP();
			}
			if (!this.IsFirstChunk)
			{
				mbpInfo.Border = new Thickness(mbpInfo.Border.Left, 0.0, mbpInfo.Border.Right, mbpInfo.Border.Bottom);
				mbpInfo.Padding = new Thickness(mbpInfo.Padding.Left, 0.0, mbpInfo.Padding.Right, mbpInfo.Padding.Bottom);
			}
			if (!this.IsLastChunk)
			{
				mbpInfo.Border = new Thickness(mbpInfo.Border.Left, mbpInfo.Border.Top, mbpInfo.Border.Right, 0.0);
				mbpInfo.Padding = new Thickness(mbpInfo.Padding.Left, mbpInfo.Padding.Top, mbpInfo.Padding.Right, 0.0);
			}
			this._contentRect.u = this._rect.u + mbpInfo.BPLeft;
			this._contentRect.du = Math.Max(TextDpi.ToTextDpi(TextDpi.MinWidth), this._rect.du - mbpInfo.BPRight - mbpInfo.BPLeft);
			this._contentRect.v = this._rect.v + mbpInfo.BPTop;
			this._contentRect.dv = Math.Max(TextDpi.ToTextDpi(TextDpi.MinWidth), this._rect.dv - mbpInfo.BPBottom - mbpInfo.BPTop);
			this._paddingRect.u = this._rect.u + mbpInfo.BorderLeft;
			this._paddingRect.du = Math.Max(TextDpi.ToTextDpi(TextDpi.MinWidth), this._rect.du - mbpInfo.BorderRight - mbpInfo.BorderLeft);
			this._paddingRect.v = this._rect.v + mbpInfo.BorderTop;
			this._paddingRect.dv = Math.Max(TextDpi.ToTextDpi(TextDpi.MinWidth), this._rect.dv - mbpInfo.BorderBottom - mbpInfo.BorderTop);
			if (PTS.ToBoolean(fssubpagedetails.fSimple))
			{
				this._pageContextOfThisPage.PageRect = new PTS.FSRECT(fssubpagedetails.u.simple.trackdescr.fsrc);
				base.Paragraph.StructuralCache.CurrentArrangeContext.PushNewPageData(this._pageContextOfThisPage, fssubpagedetails.u.simple.trackdescr.fsrc, base.Paragraph.StructuralCache.CurrentArrangeContext.FinitePage);
				PtsHelper.ArrangeTrack(base.PtsContext, ref fssubpagedetails.u.simple.trackdescr, fssubpagedetails.u.simple.fswdir);
				base.Paragraph.StructuralCache.CurrentArrangeContext.PopPageData();
				return;
			}
			this._pageContextOfThisPage.PageRect = new PTS.FSRECT(fssubpagedetails.u.complex.fsrc);
			if (fssubpagedetails.u.complex.cBasicColumns != 0)
			{
				PTS.FSTRACKDESCRIPTION[] array;
				PtsHelper.TrackListFromSubpage(base.PtsContext, this._paraHandle.Value, ref fssubpagedetails, out array);
				for (int i = 0; i < array.Length; i++)
				{
					base.Paragraph.StructuralCache.CurrentArrangeContext.PushNewPageData(this._pageContextOfThisPage, array[i].fsrc, base.Paragraph.StructuralCache.CurrentArrangeContext.FinitePage);
					PtsHelper.ArrangeTrack(base.PtsContext, ref array[i], fssubpagedetails.u.complex.fswdir);
					base.Paragraph.StructuralCache.CurrentArrangeContext.PopPageData();
				}
			}
		}

		// Token: 0x06006A86 RID: 27270 RVA: 0x001E4E20 File Offset: 0x001E3020
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override IInputElement InputHitTest(PTS.FSPOINT pt)
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
			if (inputElement == null && base.Rect.Contains(pt))
			{
				if (this.ContentRect.Contains(pt))
				{
					pt = new PTS.FSPOINT(pt.u - this.ContentRect.u, pt.v - this.ContentRect.v);
					PTS.FSSUBPAGEDETAILS fssubpagedetails;
					PTS.Validate(PTS.FsQuerySubpageDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubpagedetails));
					if (PTS.ToBoolean(fssubpagedetails.fSimple))
					{
						inputElement = PtsHelper.InputHitTestTrack(base.PtsContext, pt, ref fssubpagedetails.u.simple.trackdescr);
					}
					else if (fssubpagedetails.u.complex.cBasicColumns != 0)
					{
						PTS.FSTRACKDESCRIPTION[] array;
						PtsHelper.TrackListFromSubpage(base.PtsContext, this._paraHandle.Value, ref fssubpagedetails, out array);
						int num2 = 0;
						while (num2 < array.Length && inputElement == null)
						{
							inputElement = PtsHelper.InputHitTestTrack(base.PtsContext, pt, ref array[num2]);
							num2++;
						}
					}
				}
				if (inputElement == null)
				{
					inputElement = (base.Paragraph.Element as IInputElement);
				}
			}
			return inputElement;
		}

		// Token: 0x06006A87 RID: 27271 RVA: 0x001E4F8C File Offset: 0x001E318C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override List<Rect> GetRectangles(ContentElement e, int start, int length)
		{
			List<Rect> list = new List<Rect>();
			if (base.Paragraph.Element as ContentElement == e)
			{
				this.GetRectanglesForParagraphElement(out list);
			}
			else
			{
				PTS.FSSUBPAGEDETAILS fssubpagedetails;
				PTS.Validate(PTS.FsQuerySubpageDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubpagedetails));
				if (PTS.ToBoolean(fssubpagedetails.fSimple))
				{
					list = PtsHelper.GetRectanglesInTrack(base.PtsContext, e, start, length, ref fssubpagedetails.u.simple.trackdescr);
				}
				else if (fssubpagedetails.u.complex.cBasicColumns != 0)
				{
					PTS.FSTRACKDESCRIPTION[] array;
					PtsHelper.TrackListFromSubpage(base.PtsContext, this._paraHandle.Value, ref fssubpagedetails, out array);
					for (int i = 0; i < array.Length; i++)
					{
						List<Rect> rectanglesInTrack = PtsHelper.GetRectanglesInTrack(base.PtsContext, e, start, length, ref array[i]);
						Invariant.Assert(rectanglesInTrack != null);
						if (rectanglesInTrack.Count != 0)
						{
							list.AddRange(rectanglesInTrack);
						}
					}
				}
				list = PtsHelper.OffsetRectangleList(list, TextDpi.FromTextDpi(this.ContentRect.u), TextDpi.FromTextDpi(this.ContentRect.v));
			}
			Invariant.Assert(list != null);
			return list;
		}

		// Token: 0x06006A88 RID: 27272 RVA: 0x001E50B0 File Offset: 0x001E32B0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override void ValidateVisual(PTS.FSKUPDATE fskupdInherited)
		{
			PTS.FSSUBPAGEDETAILS fssubpagedetails;
			PTS.Validate(PTS.FsQuerySubpageDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubpagedetails));
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Paragraph.Element, base.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (base.ThisFlowDirection != base.PageFlowDirection)
			{
				mbpInfo.MirrorBP();
			}
			Brush backgroundBrush = (Brush)base.Paragraph.Element.GetValue(TextElement.BackgroundProperty);
			this.Visual.DrawBackgroundAndBorder(backgroundBrush, mbpInfo.BorderBrush, mbpInfo.Border, this._rect.FromTextDpi(), this.IsFirstChunk, this.IsLastChunk);
			if (this._visual.Children.Count != 2)
			{
				this._visual.Children.Clear();
				this._visual.Children.Add(new ContainerVisual());
				this._visual.Children.Add(new ContainerVisual());
			}
			ContainerVisual containerVisual = (ContainerVisual)this._visual.Children[0];
			ContainerVisual containerVisual2 = (ContainerVisual)this._visual.Children[1];
			if (PTS.ToBoolean(fssubpagedetails.fSimple))
			{
				PTS.FSKUPDATE fskupdate = fssubpagedetails.u.simple.trackdescr.fsupdinf.fskupd;
				if (fskupdate == PTS.FSKUPDATE.fskupdInherited)
				{
					fskupdate = fskupdInherited;
				}
				VisualCollection children = containerVisual.Children;
				if (fskupdate == PTS.FSKUPDATE.fskupdNew)
				{
					children.Clear();
					children.Add(new ContainerVisual());
				}
				else if (children.Count == 1 && children[0] is SectionVisual)
				{
					children.Clear();
					children.Add(new ContainerVisual());
				}
				ContainerVisual containerVisual3 = (ContainerVisual)children[0];
				PtsHelper.UpdateTrackVisuals(base.PtsContext, containerVisual3.Children, fskupdInherited, ref fssubpagedetails.u.simple.trackdescr);
			}
			else
			{
				bool flag = fssubpagedetails.u.complex.cBasicColumns == 0;
				if (!flag)
				{
					PTS.FSTRACKDESCRIPTION[] array;
					PtsHelper.TrackListFromSubpage(base.PtsContext, this._paraHandle.Value, ref fssubpagedetails, out array);
					flag = (array.Length == 0);
					if (!flag)
					{
						ErrorHandler.Assert(fskupdInherited != PTS.FSKUPDATE.fskupdShifted, ErrorHandler.UpdateShiftedNotValid);
						VisualCollection children2 = containerVisual.Children;
						if (children2.Count == 0)
						{
							children2.Add(new SectionVisual());
						}
						else if (!(children2[0] is SectionVisual))
						{
							children2.Clear();
							children2.Add(new SectionVisual());
						}
						SectionVisual sectionVisual = (SectionVisual)children2[0];
						ColumnPropertiesGroup columnProperties = new ColumnPropertiesGroup(base.Paragraph.Element);
						sectionVisual.DrawColumnRules(ref array, TextDpi.FromTextDpi(fssubpagedetails.u.complex.fsrc.v), TextDpi.FromTextDpi(fssubpagedetails.u.complex.fsrc.dv), columnProperties);
						children2 = sectionVisual.Children;
						if (fskupdInherited == PTS.FSKUPDATE.fskupdNew)
						{
							children2.Clear();
							for (int i = 0; i < array.Length; i++)
							{
								children2.Add(new ContainerVisual());
							}
						}
						ErrorHandler.Assert(children2.Count == array.Length, ErrorHandler.ColumnVisualCountMismatch);
						for (int j = 0; j < array.Length; j++)
						{
							ContainerVisual containerVisual4 = (ContainerVisual)children2[j];
							PtsHelper.UpdateTrackVisuals(base.PtsContext, containerVisual4.Children, fskupdInherited, ref array[j]);
						}
					}
				}
				if (flag)
				{
					this._visual.Children.Clear();
				}
			}
			containerVisual.Offset = new PTS.FSVECTOR(this.ContentRect.u, this.ContentRect.v).FromTextDpi();
			containerVisual2.Offset = new PTS.FSVECTOR(this.ContentRect.u, this.ContentRect.v).FromTextDpi();
			PTS.FSRECT fsrect = new PTS.FSRECT(this._paddingRect.u - this._contentRect.u, this._paddingRect.v - this._contentRect.v, this._paddingRect.du, this._paddingRect.dv);
			PtsHelper.ClipChildrenToRect(this._visual, fsrect.FromTextDpi());
			PtsHelper.UpdateFloatingElementVisuals(containerVisual2, this._pageContextOfThisPage.FloatingElementList);
		}

		// Token: 0x06006A89 RID: 27273 RVA: 0x001E54FC File Offset: 0x001E36FC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override void UpdateViewport(ref PTS.FSRECT viewport)
		{
			PTS.FSSUBPAGEDETAILS fssubpagedetails;
			PTS.Validate(PTS.FsQuerySubpageDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubpagedetails));
			PTS.FSRECT fsrect = default(PTS.FSRECT);
			fsrect.u = viewport.u - this.ContentRect.u;
			fsrect.v = viewport.v - this.ContentRect.v;
			fsrect.du = viewport.du;
			fsrect.dv = viewport.dv;
			if (PTS.ToBoolean(fssubpagedetails.fSimple))
			{
				PtsHelper.UpdateViewportTrack(base.PtsContext, ref fssubpagedetails.u.simple.trackdescr, ref fsrect);
				return;
			}
			if (fssubpagedetails.u.complex.cBasicColumns != 0)
			{
				PTS.FSTRACKDESCRIPTION[] array;
				PtsHelper.TrackListFromSubpage(base.PtsContext, this._paraHandle.Value, ref fssubpagedetails, out array);
				if (array.Length != 0)
				{
					for (int i = 0; i < array.Length; i++)
					{
						PtsHelper.UpdateViewportTrack(base.PtsContext, ref array[i], ref fsrect);
					}
				}
			}
		}

		// Token: 0x06006A8A RID: 27274 RVA: 0x001E560C File Offset: 0x001E380C
		internal override ParagraphResult CreateParagraphResult()
		{
			return new SubpageParagraphResult(this);
		}

		// Token: 0x06006A8B RID: 27275 RVA: 0x001E5614 File Offset: 0x001E3814
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override TextContentRange GetTextContentRange()
		{
			PTS.FSSUBPAGEDETAILS fssubpagedetails;
			PTS.Validate(PTS.FsQuerySubpageDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubpagedetails));
			TextContentRange textContentRange;
			if (PTS.ToBoolean(fssubpagedetails.fSimple))
			{
				textContentRange = PtsHelper.TextContentRangeFromTrack(base.PtsContext, fssubpagedetails.u.simple.trackdescr.pfstrack);
			}
			else
			{
				textContentRange = new TextContentRange();
				if (fssubpagedetails.u.complex.cBasicColumns != 0)
				{
					PTS.FSTRACKDESCRIPTION[] array;
					PtsHelper.TrackListFromSubpage(base.PtsContext, this._paraHandle.Value, ref fssubpagedetails, out array);
					for (int i = 0; i < array.Length; i++)
					{
						textContentRange.Merge(PtsHelper.TextContentRangeFromTrack(base.PtsContext, array[i].pfstrack));
					}
				}
			}
			TextElement textElement = base.Paragraph.Element as TextElement;
			if (this._isFirstChunk)
			{
				textContentRange.Merge(TextContainerHelper.GetTextContentRangeForTextElementEdge(textElement, ElementEdge.BeforeStart));
			}
			if (this._isLastChunk)
			{
				textContentRange.Merge(TextContainerHelper.GetTextContentRangeForTextElementEdge(textElement, ElementEdge.AfterEnd));
			}
			Invariant.Assert(textContentRange != null);
			return textContentRange;
		}

		// Token: 0x06006A8C RID: 27276 RVA: 0x001E571C File Offset: 0x001E391C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal ReadOnlyCollection<ColumnResult> GetColumnResults(out bool hasTextContent)
		{
			List<ColumnResult> list = new List<ColumnResult>(0);
			Vector contentOffset = default(Vector);
			hasTextContent = false;
			PTS.FSSUBPAGEDETAILS fssubpagedetails;
			PTS.Validate(PTS.FsQuerySubpageDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubpagedetails));
			if (PTS.ToBoolean(fssubpagedetails.fSimple))
			{
				PTS.FSTRACKDETAILS fstrackdetails;
				PTS.Validate(PTS.FsQueryTrackDetails(base.PtsContext.Context, fssubpagedetails.u.simple.trackdescr.pfstrack, out fstrackdetails));
				if (fstrackdetails.cParas > 0)
				{
					list = new List<ColumnResult>(1);
					ColumnResult columnResult = new ColumnResult(this, ref fssubpagedetails.u.simple.trackdescr, contentOffset);
					list.Add(columnResult);
					if (columnResult.HasTextContent)
					{
						hasTextContent = true;
					}
				}
			}
			else if (fssubpagedetails.u.complex.cBasicColumns != 0)
			{
				PTS.FSTRACKDESCRIPTION[] array;
				PtsHelper.TrackListFromSubpage(base.PtsContext, this._paraHandle.Value, ref fssubpagedetails, out array);
				list = new List<ColumnResult>(fssubpagedetails.u.complex.cBasicColumns);
				for (int i = 0; i < array.Length; i++)
				{
					PTS.FSTRACKDETAILS fstrackdetails2;
					PTS.Validate(PTS.FsQueryTrackDetails(base.PtsContext.Context, array[i].pfstrack, out fstrackdetails2));
					if (fstrackdetails2.cParas > 0)
					{
						ColumnResult columnResult2 = new ColumnResult(this, ref array[i], contentOffset);
						list.Add(columnResult2);
						if (columnResult2.HasTextContent)
						{
							hasTextContent = true;
						}
					}
				}
			}
			return new ReadOnlyCollection<ColumnResult>(list);
		}

		// Token: 0x06006A8D RID: 27277 RVA: 0x001E5894 File Offset: 0x001E3A94
		[SecurityCritical]
		internal ReadOnlyCollection<ParagraphResult> GetParagraphResultsFromColumn(IntPtr pfstrack, Vector parentOffset, out bool hasTextContent)
		{
			PTS.FSTRACKDETAILS fstrackdetails;
			PTS.Validate(PTS.FsQueryTrackDetails(base.PtsContext.Context, pfstrack, out fstrackdetails));
			hasTextContent = false;
			if (fstrackdetails.cParas == 0)
			{
				return null;
			}
			PTS.FSPARADESCRIPTION[] array;
			PtsHelper.ParaListFromTrack(base.PtsContext, pfstrack, ref fstrackdetails, out array);
			List<ParagraphResult> list = new List<ParagraphResult>(array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				BaseParaClient baseParaClient = base.PtsContext.HandleToObject(array[i].pfsparaclient) as BaseParaClient;
				PTS.ValidateHandle(baseParaClient);
				ParagraphResult paragraphResult = baseParaClient.CreateParagraphResult();
				if (paragraphResult.HasTextContent)
				{
					hasTextContent = true;
				}
				list.Add(paragraphResult);
			}
			return new ReadOnlyCollection<ParagraphResult>(list);
		}

		// Token: 0x06006A8E RID: 27278 RVA: 0x001E5938 File Offset: 0x001E3B38
		[SecurityCritical]
		internal TextContentRange GetTextContentRangeFromColumn(IntPtr pfstrack)
		{
			PTS.FSTRACKDETAILS fstrackdetails;
			PTS.Validate(PTS.FsQueryTrackDetails(base.PtsContext.Context, pfstrack, out fstrackdetails));
			TextContentRange textContentRange = new TextContentRange();
			if (fstrackdetails.cParas != 0)
			{
				PTS.FSPARADESCRIPTION[] array;
				PtsHelper.ParaListFromTrack(base.PtsContext, pfstrack, ref fstrackdetails, out array);
				for (int i = 0; i < array.Length; i++)
				{
					BaseParaClient baseParaClient = base.PtsContext.HandleToObject(array[i].pfsparaclient) as BaseParaClient;
					PTS.ValidateHandle(baseParaClient);
					textContentRange.Merge(baseParaClient.GetTextContentRange());
				}
			}
			return textContentRange;
		}

		// Token: 0x06006A8F RID: 27279 RVA: 0x001E59BE File Offset: 0x001E3BBE
		internal void SetChunkInfo(bool isFirstChunk, bool isLastChunk)
		{
			this._isFirstChunk = isFirstChunk;
			this._isLastChunk = isLastChunk;
		}

		// Token: 0x170019A1 RID: 6561
		// (get) Token: 0x06006A90 RID: 27280 RVA: 0x001E59CE File Offset: 0x001E3BCE
		internal override bool IsFirstChunk
		{
			get
			{
				return this._isFirstChunk;
			}
		}

		// Token: 0x170019A2 RID: 6562
		// (get) Token: 0x06006A91 RID: 27281 RVA: 0x001E59D6 File Offset: 0x001E3BD6
		internal override bool IsLastChunk
		{
			get
			{
				return this._isLastChunk;
			}
		}

		// Token: 0x170019A3 RID: 6563
		// (get) Token: 0x06006A92 RID: 27282 RVA: 0x001E59E0 File Offset: 0x001E3BE0
		internal ReadOnlyCollection<ParagraphResult> FloatingElementResults
		{
			get
			{
				List<ParagraphResult> list = new List<ParagraphResult>(0);
				List<BaseParaClient> floatingElementList = this._pageContextOfThisPage.FloatingElementList;
				if (floatingElementList != null)
				{
					for (int i = 0; i < floatingElementList.Count; i++)
					{
						ParagraphResult item = floatingElementList[i].CreateParagraphResult();
						list.Add(item);
					}
				}
				return new ReadOnlyCollection<ParagraphResult>(list);
			}
		}

		// Token: 0x170019A4 RID: 6564
		// (get) Token: 0x06006A93 RID: 27283 RVA: 0x001E5A2E File Offset: 0x001E3C2E
		internal PTS.FSRECT ContentRect
		{
			get
			{
				return this._contentRect;
			}
		}

		// Token: 0x04003448 RID: 13384
		private bool _isFirstChunk;

		// Token: 0x04003449 RID: 13385
		private bool _isLastChunk;

		// Token: 0x0400344A RID: 13386
		private PTS.FSRECT _contentRect;

		// Token: 0x0400344B RID: 13387
		private PTS.FSRECT _paddingRect;

		// Token: 0x0400344C RID: 13388
		private PageContext _pageContextOfThisPage = new PageContext();
	}
}
