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
	// Token: 0x02000614 RID: 1556
	internal class ContainerParaClient : BaseParaClient
	{
		// Token: 0x0600678C RID: 26508 RVA: 0x001CF362 File Offset: 0x001CD562
		internal ContainerParaClient(ContainerParagraph paragraph) : base(paragraph)
		{
		}

		// Token: 0x0600678D RID: 26509 RVA: 0x001CF36C File Offset: 0x001CD56C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override void OnArrange()
		{
			base.OnArrange();
			PTS.FSSUBTRACKDETAILS fssubtrackdetails;
			PTS.Validate(PTS.FsQuerySubtrackDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubtrackdetails));
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Paragraph.Element, base.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (base.ParentFlowDirection != base.PageFlowDirection)
			{
				mbpInfo.MirrorMargin();
			}
			this._rect.u = this._rect.u + mbpInfo.MarginLeft;
			this._rect.du = this._rect.du - (mbpInfo.MarginLeft + mbpInfo.MarginRight);
			this._rect.du = Math.Max(TextDpi.ToTextDpi(TextDpi.MinWidth), this._rect.du);
			this._rect.dv = Math.Max(TextDpi.ToTextDpi(TextDpi.MinWidth), this._rect.dv);
			uint fswdirTrack = PTS.FlowDirectionToFswdir(this._flowDirection);
			if (fssubtrackdetails.cParas != 0)
			{
				PTS.FSPARADESCRIPTION[] arrayParaDesc;
				PtsHelper.ParaListFromSubtrack(base.PtsContext, this._paraHandle.Value, ref fssubtrackdetails, out arrayParaDesc);
				PtsHelper.ArrangeParaList(base.PtsContext, fssubtrackdetails.fsrc, arrayParaDesc, fswdirTrack);
			}
		}

		// Token: 0x0600678E RID: 26510 RVA: 0x001CF498 File Offset: 0x001CD698
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override IInputElement InputHitTest(PTS.FSPOINT pt)
		{
			IInputElement inputElement = null;
			PTS.FSSUBTRACKDETAILS fssubtrackdetails;
			PTS.Validate(PTS.FsQuerySubtrackDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubtrackdetails));
			if (fssubtrackdetails.cParas != 0)
			{
				PTS.FSPARADESCRIPTION[] arrayParaDesc;
				PtsHelper.ParaListFromSubtrack(base.PtsContext, this._paraHandle.Value, ref fssubtrackdetails, out arrayParaDesc);
				inputElement = PtsHelper.InputHitTestParaList(base.PtsContext, pt, ref fssubtrackdetails.fsrc, arrayParaDesc);
			}
			if (inputElement == null && this._rect.Contains(pt))
			{
				inputElement = (base.Paragraph.Element as IInputElement);
			}
			return inputElement;
		}

		// Token: 0x0600678F RID: 26511 RVA: 0x001CF524 File Offset: 0x001CD724
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
				PTS.FSSUBTRACKDETAILS fssubtrackdetails;
				PTS.Validate(PTS.FsQuerySubtrackDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubtrackdetails));
				if (fssubtrackdetails.cParas != 0)
				{
					PTS.FSPARADESCRIPTION[] arrayParaDesc;
					PtsHelper.ParaListFromSubtrack(base.PtsContext, this._paraHandle.Value, ref fssubtrackdetails, out arrayParaDesc);
					list = PtsHelper.GetRectanglesInParaList(base.PtsContext, e, start, length, arrayParaDesc);
				}
				else
				{
					list = new List<Rect>();
				}
			}
			Invariant.Assert(list != null);
			return list;
		}

		// Token: 0x06006790 RID: 26512 RVA: 0x001CF5BC File Offset: 0x001CD7BC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override void ValidateVisual(PTS.FSKUPDATE fskupdInherited)
		{
			PTS.FSSUBTRACKDETAILS fssubtrackdetails;
			PTS.Validate(PTS.FsQuerySubtrackDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubtrackdetails));
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Paragraph.Element, base.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (base.ThisFlowDirection != base.PageFlowDirection)
			{
				mbpInfo.MirrorBP();
			}
			Brush backgroundBrush = (Brush)base.Paragraph.Element.GetValue(TextElement.BackgroundProperty);
			this._visual.DrawBackgroundAndBorder(backgroundBrush, mbpInfo.BorderBrush, mbpInfo.Border, this._rect.FromTextDpi(), this.IsFirstChunk, this.IsLastChunk);
			if (fssubtrackdetails.cParas != 0)
			{
				PTS.FSPARADESCRIPTION[] arrayParaDesc;
				PtsHelper.ParaListFromSubtrack(base.PtsContext, this._paraHandle.Value, ref fssubtrackdetails, out arrayParaDesc);
				PtsHelper.UpdateParaListVisuals(base.PtsContext, this._visual.Children, fskupdInherited, arrayParaDesc);
				return;
			}
			this._visual.Children.Clear();
		}

		// Token: 0x06006791 RID: 26513 RVA: 0x001CF6BC File Offset: 0x001CD8BC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override void UpdateViewport(ref PTS.FSRECT viewport)
		{
			PTS.FSSUBTRACKDETAILS fssubtrackdetails;
			PTS.Validate(PTS.FsQuerySubtrackDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubtrackdetails));
			if (fssubtrackdetails.cParas != 0)
			{
				PTS.FSPARADESCRIPTION[] arrayParaDesc;
				PtsHelper.ParaListFromSubtrack(base.PtsContext, this._paraHandle.Value, ref fssubtrackdetails, out arrayParaDesc);
				PtsHelper.UpdateViewportParaList(base.PtsContext, arrayParaDesc, ref viewport);
			}
		}

		// Token: 0x06006792 RID: 26514 RVA: 0x001CF71A File Offset: 0x001CD91A
		internal override ParagraphResult CreateParagraphResult()
		{
			return new ContainerParagraphResult(this);
		}

		// Token: 0x06006793 RID: 26515 RVA: 0x001CF724 File Offset: 0x001CD924
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override TextContentRange GetTextContentRange()
		{
			TextElement textElement = base.Paragraph.Element as TextElement;
			Invariant.Assert(textElement != null, "Expecting TextElement as owner of ContainerParagraph.");
			PTS.FSSUBTRACKDETAILS fssubtrackdetails;
			PTS.Validate(PTS.FsQuerySubtrackDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubtrackdetails));
			TextContentRange textContentRange;
			if (fssubtrackdetails.cParas == 0 || (this._isFirstChunk && this._isLastChunk))
			{
				textContentRange = TextContainerHelper.GetTextContentRangeForTextElement(textElement);
			}
			else
			{
				PTS.FSPARADESCRIPTION[] array;
				PtsHelper.ParaListFromSubtrack(base.PtsContext, this._paraHandle.Value, ref fssubtrackdetails, out array);
				textContentRange = new TextContentRange();
				for (int i = 0; i < array.Length; i++)
				{
					BaseParaClient baseParaClient = base.Paragraph.StructuralCache.PtsContext.HandleToObject(array[i].pfsparaclient) as BaseParaClient;
					PTS.ValidateHandle(baseParaClient);
					textContentRange.Merge(baseParaClient.GetTextContentRange());
				}
				if (this._isFirstChunk)
				{
					textContentRange.Merge(TextContainerHelper.GetTextContentRangeForTextElementEdge(textElement, ElementEdge.BeforeStart));
				}
				if (this._isLastChunk)
				{
					textContentRange.Merge(TextContainerHelper.GetTextContentRangeForTextElementEdge(textElement, ElementEdge.AfterEnd));
				}
			}
			Invariant.Assert(textContentRange != null);
			return textContentRange;
		}

		// Token: 0x06006794 RID: 26516 RVA: 0x001CF838 File Offset: 0x001CDA38
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal ReadOnlyCollection<ParagraphResult> GetChildrenParagraphResults(out bool hasTextContent)
		{
			PTS.FSSUBTRACKDETAILS fssubtrackdetails;
			PTS.Validate(PTS.FsQuerySubtrackDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubtrackdetails));
			hasTextContent = false;
			if (fssubtrackdetails.cParas == 0)
			{
				return new ReadOnlyCollection<ParagraphResult>(new List<ParagraphResult>(0));
			}
			PTS.FSPARADESCRIPTION[] array;
			PtsHelper.ParaListFromSubtrack(base.PtsContext, this._paraHandle.Value, ref fssubtrackdetails, out array);
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

		// Token: 0x06006795 RID: 26517 RVA: 0x001CF8F7 File Offset: 0x001CDAF7
		internal void SetChunkInfo(bool isFirstChunk, bool isLastChunk)
		{
			this._isFirstChunk = isFirstChunk;
			this._isLastChunk = isLastChunk;
		}

		// Token: 0x06006796 RID: 26518 RVA: 0x001CF908 File Offset: 0x001CDB08
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override int GetFirstTextLineBaseline()
		{
			PTS.FSSUBTRACKDETAILS fssubtrackdetails;
			PTS.Validate(PTS.FsQuerySubtrackDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubtrackdetails));
			if (fssubtrackdetails.cParas == 0)
			{
				return this._rect.v;
			}
			PTS.FSPARADESCRIPTION[] array;
			PtsHelper.ParaListFromSubtrack(base.PtsContext, this._paraHandle.Value, ref fssubtrackdetails, out array);
			BaseParaClient baseParaClient = base.PtsContext.HandleToObject(array[0].pfsparaclient) as BaseParaClient;
			PTS.ValidateHandle(baseParaClient);
			return baseParaClient.GetFirstTextLineBaseline();
		}

		// Token: 0x06006797 RID: 26519 RVA: 0x001CF990 File Offset: 0x001CDB90
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition, Rect visibleRect)
		{
			bool flag;
			ReadOnlyCollection<ParagraphResult> childrenParagraphResults = this.GetChildrenParagraphResults(out flag);
			Invariant.Assert(childrenParagraphResults != null, "Paragraph collection is null.");
			if (childrenParagraphResults.Count > 0)
			{
				return TextDocumentView.GetTightBoundingGeometryFromTextPositionsHelper(childrenParagraphResults, startPosition, endPosition, TextDpi.FromTextDpi(this._dvrTopSpace), visibleRect);
			}
			return null;
		}

		// Token: 0x17001910 RID: 6416
		// (get) Token: 0x06006798 RID: 26520 RVA: 0x001CF9D3 File Offset: 0x001CDBD3
		internal override bool IsFirstChunk
		{
			get
			{
				return this._isFirstChunk;
			}
		}

		// Token: 0x17001911 RID: 6417
		// (get) Token: 0x06006799 RID: 26521 RVA: 0x001CF9DB File Offset: 0x001CDBDB
		internal override bool IsLastChunk
		{
			get
			{
				return this._isLastChunk;
			}
		}

		// Token: 0x0400337A RID: 13178
		private bool _isFirstChunk;

		// Token: 0x0400337B RID: 13179
		private bool _isLastChunk;
	}
}
