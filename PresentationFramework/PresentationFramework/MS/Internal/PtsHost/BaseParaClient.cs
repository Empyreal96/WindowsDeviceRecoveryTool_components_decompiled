using System;
using System.Collections.Generic;
using System.Security;
using System.Windows;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200060D RID: 1549
	internal abstract class BaseParaClient : UnmanagedHandle
	{
		// Token: 0x06006733 RID: 26419 RVA: 0x001CDE6D File Offset: 0x001CC06D
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected BaseParaClient(BaseParagraph paragraph) : base(paragraph.PtsContext)
		{
			this._paraHandle = new SecurityCriticalDataForSet<IntPtr>(IntPtr.Zero);
			this._paragraph = paragraph;
		}

		// Token: 0x06006734 RID: 26420 RVA: 0x001CDE94 File Offset: 0x001CC094
		[SecurityCritical]
		internal void Arrange(IntPtr pfspara, PTS.FSRECT rcPara, int dvrTopSpace, uint fswdirParent)
		{
			this._paraHandle.Value = pfspara;
			this._rect = rcPara;
			this._dvrTopSpace = dvrTopSpace;
			this._pageContext = this.Paragraph.StructuralCache.CurrentArrangeContext.PageContext;
			this._flowDirectionParent = PTS.FswdirToFlowDirection(fswdirParent);
			this._flowDirection = (FlowDirection)this.Paragraph.Element.GetValue(FrameworkElement.FlowDirectionProperty);
			this.OnArrange();
		}

		// Token: 0x06006735 RID: 26421 RVA: 0x001CDF09 File Offset: 0x001CC109
		internal virtual int GetFirstTextLineBaseline()
		{
			return this._rect.v + this._rect.dv;
		}

		// Token: 0x06006736 RID: 26422 RVA: 0x001CDF22 File Offset: 0x001CC122
		internal void TransferDisplayInfo(BaseParaClient oldParaClient)
		{
			this._visual = oldParaClient._visual;
			oldParaClient._visual = null;
		}

		// Token: 0x06006737 RID: 26423 RVA: 0x0000C238 File Offset: 0x0000A438
		internal virtual IInputElement InputHitTest(PTS.FSPOINT pt)
		{
			return null;
		}

		// Token: 0x06006738 RID: 26424 RVA: 0x001CDF37 File Offset: 0x001CC137
		internal virtual List<Rect> GetRectangles(ContentElement e, int start, int length)
		{
			return new List<Rect>();
		}

		// Token: 0x06006739 RID: 26425 RVA: 0x001CDF40 File Offset: 0x001CC140
		internal virtual void GetRectanglesForParagraphElement(out List<Rect> rectangles)
		{
			rectangles = new List<Rect>();
			Rect item = TextDpi.FromTextRect(this._rect);
			rectangles.Add(item);
		}

		// Token: 0x0600673A RID: 26426 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void ValidateVisual(PTS.FSKUPDATE fskupdInherited)
		{
		}

		// Token: 0x0600673B RID: 26427 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void UpdateViewport(ref PTS.FSRECT viewport)
		{
		}

		// Token: 0x0600673C RID: 26428
		internal abstract ParagraphResult CreateParagraphResult();

		// Token: 0x0600673D RID: 26429
		internal abstract TextContentRange GetTextContentRange();

		// Token: 0x170018EC RID: 6380
		// (get) Token: 0x0600673E RID: 26430 RVA: 0x001CDF68 File Offset: 0x001CC168
		internal virtual ParagraphVisual Visual
		{
			get
			{
				if (this._visual == null)
				{
					this._visual = new ParagraphVisual();
				}
				return this._visual;
			}
		}

		// Token: 0x170018ED RID: 6381
		// (get) Token: 0x0600673F RID: 26431 RVA: 0x00016748 File Offset: 0x00014948
		internal virtual bool IsFirstChunk
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170018EE RID: 6382
		// (get) Token: 0x06006740 RID: 26432 RVA: 0x00016748 File Offset: 0x00014948
		internal virtual bool IsLastChunk
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170018EF RID: 6383
		// (get) Token: 0x06006741 RID: 26433 RVA: 0x001CDF83 File Offset: 0x001CC183
		internal BaseParagraph Paragraph
		{
			get
			{
				return this._paragraph;
			}
		}

		// Token: 0x170018F0 RID: 6384
		// (get) Token: 0x06006742 RID: 26434 RVA: 0x001CDF8B File Offset: 0x001CC18B
		internal PTS.FSRECT Rect
		{
			get
			{
				return this._rect;
			}
		}

		// Token: 0x170018F1 RID: 6385
		// (get) Token: 0x06006743 RID: 26435 RVA: 0x001CDF93 File Offset: 0x001CC193
		internal FlowDirection ThisFlowDirection
		{
			get
			{
				return this._flowDirection;
			}
		}

		// Token: 0x170018F2 RID: 6386
		// (get) Token: 0x06006744 RID: 26436 RVA: 0x001CDF9B File Offset: 0x001CC19B
		internal FlowDirection ParentFlowDirection
		{
			get
			{
				return this._flowDirectionParent;
			}
		}

		// Token: 0x170018F3 RID: 6387
		// (get) Token: 0x06006745 RID: 26437 RVA: 0x001CDFA3 File Offset: 0x001CC1A3
		internal FlowDirection PageFlowDirection
		{
			get
			{
				return this.Paragraph.StructuralCache.PageFlowDirection;
			}
		}

		// Token: 0x06006746 RID: 26438 RVA: 0x001CDFB5 File Offset: 0x001CC1B5
		protected virtual void OnArrange()
		{
			this.Paragraph.UpdateLastFormatPositions();
		}

		// Token: 0x0400335D RID: 13149
		protected readonly BaseParagraph _paragraph;

		// Token: 0x0400335E RID: 13150
		protected SecurityCriticalDataForSet<IntPtr> _paraHandle;

		// Token: 0x0400335F RID: 13151
		protected PTS.FSRECT _rect;

		// Token: 0x04003360 RID: 13152
		protected int _dvrTopSpace;

		// Token: 0x04003361 RID: 13153
		protected ParagraphVisual _visual;

		// Token: 0x04003362 RID: 13154
		protected PageContext _pageContext;

		// Token: 0x04003363 RID: 13155
		protected FlowDirection _flowDirectionParent;

		// Token: 0x04003364 RID: 13156
		protected FlowDirection _flowDirection;
	}
}
