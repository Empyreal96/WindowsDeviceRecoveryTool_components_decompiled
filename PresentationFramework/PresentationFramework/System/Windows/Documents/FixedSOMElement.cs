using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace System.Windows.Documents
{
	// Token: 0x02000358 RID: 856
	internal abstract class FixedSOMElement : FixedSOMSemanticBox
	{
		// Token: 0x06002DAA RID: 11690 RVA: 0x000CD9C0 File Offset: 0x000CBBC0
		protected FixedSOMElement(FixedNode fixedNode, int startIndex, int endIndex, GeneralTransform transform)
		{
			this._fixedNode = fixedNode;
			this._startIndex = startIndex;
			this._endIndex = endIndex;
			Transform affineTransform = transform.AffineTransform;
			if (affineTransform != null)
			{
				this._mat = affineTransform.Value;
				return;
			}
			this._mat = Transform.Identity.Value;
		}

		// Token: 0x06002DAB RID: 11691 RVA: 0x000CDA10 File Offset: 0x000CBC10
		protected FixedSOMElement(FixedNode fixedNode, GeneralTransform transform)
		{
			this._fixedNode = fixedNode;
			Transform affineTransform = transform.AffineTransform;
			if (affineTransform != null)
			{
				this._mat = affineTransform.Value;
				return;
			}
			this._mat = Transform.Identity.Value;
		}

		// Token: 0x06002DAC RID: 11692 RVA: 0x000CDA54 File Offset: 0x000CBC54
		public static FixedSOMElement CreateFixedSOMElement(FixedPage page, UIElement uiElement, FixedNode fixedNode, int startIndex, int endIndex)
		{
			FixedSOMElement result = null;
			if (uiElement is Glyphs)
			{
				Glyphs glyphs = uiElement as Glyphs;
				if (glyphs.UnicodeString.Length > 0)
				{
					GlyphRun glyphRun = glyphs.ToGlyphRun();
					Rect boundingRect = glyphRun.ComputeAlignmentBox();
					boundingRect.Offset(glyphs.OriginX, glyphs.OriginY);
					GeneralTransform transform = glyphs.TransformToAncestor(page);
					if (startIndex < 0)
					{
						startIndex = 0;
					}
					if (endIndex < 0)
					{
						endIndex = ((glyphRun.Characters == null) ? 0 : glyphRun.Characters.Count);
					}
					result = FixedSOMTextRun.Create(boundingRect, transform, glyphs, fixedNode, startIndex, endIndex, false);
				}
			}
			else if (uiElement is Image)
			{
				result = FixedSOMImage.Create(page, uiElement as Image, fixedNode);
			}
			else if (uiElement is Path)
			{
				result = FixedSOMImage.Create(page, uiElement as Path, fixedNode);
			}
			return result;
		}

		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x06002DAD RID: 11693 RVA: 0x000CDB11 File Offset: 0x000CBD11
		public FixedNode FixedNode
		{
			get
			{
				return this._fixedNode;
			}
		}

		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x06002DAE RID: 11694 RVA: 0x000CDB19 File Offset: 0x000CBD19
		public int StartIndex
		{
			get
			{
				return this._startIndex;
			}
		}

		// Token: 0x17000B65 RID: 2917
		// (get) Token: 0x06002DAF RID: 11695 RVA: 0x000CDB21 File Offset: 0x000CBD21
		public int EndIndex
		{
			get
			{
				return this._endIndex;
			}
		}

		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x06002DB0 RID: 11696 RVA: 0x000CDB29 File Offset: 0x000CBD29
		// (set) Token: 0x06002DB1 RID: 11697 RVA: 0x000CDB31 File Offset: 0x000CBD31
		internal FlowNode FlowNode
		{
			get
			{
				return this._flowNode;
			}
			set
			{
				this._flowNode = value;
			}
		}

		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x06002DB2 RID: 11698 RVA: 0x000CDB3A File Offset: 0x000CBD3A
		// (set) Token: 0x06002DB3 RID: 11699 RVA: 0x000CDB42 File Offset: 0x000CBD42
		internal int OffsetInFlowNode
		{
			get
			{
				return this._offsetInFlowNode;
			}
			set
			{
				this._offsetInFlowNode = value;
			}
		}

		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x06002DB4 RID: 11700 RVA: 0x000CDB4B File Offset: 0x000CBD4B
		internal Matrix Matrix
		{
			get
			{
				return this._mat;
			}
		}

		// Token: 0x04001DBC RID: 7612
		protected FixedNode _fixedNode;

		// Token: 0x04001DBD RID: 7613
		protected int _startIndex;

		// Token: 0x04001DBE RID: 7614
		protected int _endIndex;

		// Token: 0x04001DBF RID: 7615
		protected Matrix _mat;

		// Token: 0x04001DC0 RID: 7616
		private FlowNode _flowNode;

		// Token: 0x04001DC1 RID: 7617
		private int _offsetInFlowNode;
	}
}
