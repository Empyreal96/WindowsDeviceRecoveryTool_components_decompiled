using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000427 RID: 1063
	internal class TextTreeTextElementNode : TextTreeNode
	{
		// Token: 0x06003DE4 RID: 15844 RVA: 0x0011D035 File Offset: 0x0011B235
		internal TextTreeTextElementNode()
		{
			this._symbolOffsetCache = -1;
		}

		// Token: 0x06003DE5 RID: 15845 RVA: 0x0011D044 File Offset: 0x0011B244
		internal override TextTreeNode Clone()
		{
			return new TextTreeTextElementNode
			{
				_symbolCount = this._symbolCount,
				_imeCharCount = this._imeCharCount,
				_textElement = this._textElement
			};
		}

		// Token: 0x06003DE6 RID: 15846 RVA: 0x0011D07C File Offset: 0x0011B27C
		internal override TextPointerContext GetPointerContext(LogicalDirection direction)
		{
			if (direction != LogicalDirection.Forward)
			{
				return TextPointerContext.ElementEnd;
			}
			return TextPointerContext.ElementStart;
		}

		// Token: 0x17000F66 RID: 3942
		// (get) Token: 0x06003DE7 RID: 15847 RVA: 0x0011D085 File Offset: 0x0011B285
		// (set) Token: 0x06003DE8 RID: 15848 RVA: 0x0011D08D File Offset: 0x0011B28D
		internal override SplayTreeNode ParentNode
		{
			get
			{
				return this._parentNode;
			}
			set
			{
				this._parentNode = (TextTreeNode)value;
			}
		}

		// Token: 0x17000F67 RID: 3943
		// (get) Token: 0x06003DE9 RID: 15849 RVA: 0x0011D09B File Offset: 0x0011B29B
		// (set) Token: 0x06003DEA RID: 15850 RVA: 0x0011D0A3 File Offset: 0x0011B2A3
		internal override SplayTreeNode ContainedNode
		{
			get
			{
				return this._containedNode;
			}
			set
			{
				this._containedNode = (TextTreeNode)value;
			}
		}

		// Token: 0x17000F68 RID: 3944
		// (get) Token: 0x06003DEB RID: 15851 RVA: 0x0011D0B1 File Offset: 0x0011B2B1
		// (set) Token: 0x06003DEC RID: 15852 RVA: 0x0011D0B9 File Offset: 0x0011B2B9
		internal override int LeftSymbolCount
		{
			get
			{
				return this._leftSymbolCount;
			}
			set
			{
				this._leftSymbolCount = value;
			}
		}

		// Token: 0x17000F69 RID: 3945
		// (get) Token: 0x06003DED RID: 15853 RVA: 0x0011D0C2 File Offset: 0x0011B2C2
		// (set) Token: 0x06003DEE RID: 15854 RVA: 0x0011D0CA File Offset: 0x0011B2CA
		internal override int LeftCharCount
		{
			get
			{
				return this._leftCharCount;
			}
			set
			{
				this._leftCharCount = value;
			}
		}

		// Token: 0x17000F6A RID: 3946
		// (get) Token: 0x06003DEF RID: 15855 RVA: 0x0011D0D3 File Offset: 0x0011B2D3
		// (set) Token: 0x06003DF0 RID: 15856 RVA: 0x0011D0DB File Offset: 0x0011B2DB
		internal override SplayTreeNode LeftChildNode
		{
			get
			{
				return this._leftChildNode;
			}
			set
			{
				this._leftChildNode = (TextTreeNode)value;
			}
		}

		// Token: 0x17000F6B RID: 3947
		// (get) Token: 0x06003DF1 RID: 15857 RVA: 0x0011D0E9 File Offset: 0x0011B2E9
		// (set) Token: 0x06003DF2 RID: 15858 RVA: 0x0011D0F1 File Offset: 0x0011B2F1
		internal override SplayTreeNode RightChildNode
		{
			get
			{
				return this._rightChildNode;
			}
			set
			{
				this._rightChildNode = (TextTreeNode)value;
			}
		}

		// Token: 0x17000F6C RID: 3948
		// (get) Token: 0x06003DF3 RID: 15859 RVA: 0x0011D0FF File Offset: 0x0011B2FF
		// (set) Token: 0x06003DF4 RID: 15860 RVA: 0x0011D107 File Offset: 0x0011B307
		internal override uint Generation
		{
			get
			{
				return this._generation;
			}
			set
			{
				this._generation = value;
			}
		}

		// Token: 0x17000F6D RID: 3949
		// (get) Token: 0x06003DF5 RID: 15861 RVA: 0x0011D110 File Offset: 0x0011B310
		// (set) Token: 0x06003DF6 RID: 15862 RVA: 0x0011D118 File Offset: 0x0011B318
		internal override int SymbolOffsetCache
		{
			get
			{
				return this._symbolOffsetCache;
			}
			set
			{
				this._symbolOffsetCache = value;
			}
		}

		// Token: 0x17000F6E RID: 3950
		// (get) Token: 0x06003DF7 RID: 15863 RVA: 0x0011D121 File Offset: 0x0011B321
		// (set) Token: 0x06003DF8 RID: 15864 RVA: 0x0011D129 File Offset: 0x0011B329
		internal override int SymbolCount
		{
			get
			{
				return this._symbolCount;
			}
			set
			{
				this._symbolCount = value;
			}
		}

		// Token: 0x17000F6F RID: 3951
		// (get) Token: 0x06003DF9 RID: 15865 RVA: 0x0011D132 File Offset: 0x0011B332
		// (set) Token: 0x06003DFA RID: 15866 RVA: 0x0011D13A File Offset: 0x0011B33A
		internal override int IMECharCount
		{
			get
			{
				return this._imeCharCount;
			}
			set
			{
				this._imeCharCount = value;
			}
		}

		// Token: 0x17000F70 RID: 3952
		// (get) Token: 0x06003DFB RID: 15867 RVA: 0x0011D143 File Offset: 0x0011B343
		// (set) Token: 0x06003DFC RID: 15868 RVA: 0x0011D150 File Offset: 0x0011B350
		internal override bool BeforeStartReferenceCount
		{
			get
			{
				return (this._edgeReferenceCounts & ElementEdge.BeforeStart) > (ElementEdge)0;
			}
			set
			{
				Invariant.Assert(value);
				this._edgeReferenceCounts |= ElementEdge.BeforeStart;
			}
		}

		// Token: 0x17000F71 RID: 3953
		// (get) Token: 0x06003DFD RID: 15869 RVA: 0x0011D166 File Offset: 0x0011B366
		// (set) Token: 0x06003DFE RID: 15870 RVA: 0x0011D173 File Offset: 0x0011B373
		internal override bool AfterStartReferenceCount
		{
			get
			{
				return (this._edgeReferenceCounts & ElementEdge.AfterStart) > (ElementEdge)0;
			}
			set
			{
				Invariant.Assert(value);
				this._edgeReferenceCounts |= ElementEdge.AfterStart;
			}
		}

		// Token: 0x17000F72 RID: 3954
		// (get) Token: 0x06003DFF RID: 15871 RVA: 0x0011D189 File Offset: 0x0011B389
		// (set) Token: 0x06003E00 RID: 15872 RVA: 0x0011D196 File Offset: 0x0011B396
		internal override bool BeforeEndReferenceCount
		{
			get
			{
				return (this._edgeReferenceCounts & ElementEdge.BeforeEnd) > (ElementEdge)0;
			}
			set
			{
				Invariant.Assert(value);
				this._edgeReferenceCounts |= ElementEdge.BeforeEnd;
			}
		}

		// Token: 0x17000F73 RID: 3955
		// (get) Token: 0x06003E01 RID: 15873 RVA: 0x0011D1AC File Offset: 0x0011B3AC
		// (set) Token: 0x06003E02 RID: 15874 RVA: 0x0011D1B9 File Offset: 0x0011B3B9
		internal override bool AfterEndReferenceCount
		{
			get
			{
				return (this._edgeReferenceCounts & ElementEdge.AfterEnd) > (ElementEdge)0;
			}
			set
			{
				Invariant.Assert(value);
				this._edgeReferenceCounts |= ElementEdge.AfterEnd;
			}
		}

		// Token: 0x17000F74 RID: 3956
		// (get) Token: 0x06003E03 RID: 15875 RVA: 0x0011D1CF File Offset: 0x0011B3CF
		// (set) Token: 0x06003E04 RID: 15876 RVA: 0x0011D1D7 File Offset: 0x0011B3D7
		internal TextElement TextElement
		{
			get
			{
				return this._textElement;
			}
			set
			{
				this._textElement = value;
			}
		}

		// Token: 0x17000F75 RID: 3957
		// (get) Token: 0x06003E05 RID: 15877 RVA: 0x0011D1E0 File Offset: 0x0011B3E0
		internal int IMELeftEdgeCharCount
		{
			get
			{
				if (this._textElement != null)
				{
					return this._textElement.IMELeftEdgeCharCount;
				}
				return -1;
			}
		}

		// Token: 0x17000F76 RID: 3958
		// (get) Token: 0x06003E06 RID: 15878 RVA: 0x0011D1F7 File Offset: 0x0011B3F7
		internal bool IsFirstSibling
		{
			get
			{
				base.Splay();
				return this._leftChildNode == null;
			}
		}

		// Token: 0x0400267E RID: 9854
		private int _leftSymbolCount;

		// Token: 0x0400267F RID: 9855
		private int _leftCharCount;

		// Token: 0x04002680 RID: 9856
		private TextTreeNode _parentNode;

		// Token: 0x04002681 RID: 9857
		private TextTreeNode _leftChildNode;

		// Token: 0x04002682 RID: 9858
		private TextTreeNode _rightChildNode;

		// Token: 0x04002683 RID: 9859
		private TextTreeNode _containedNode;

		// Token: 0x04002684 RID: 9860
		private uint _generation;

		// Token: 0x04002685 RID: 9861
		private int _symbolOffsetCache;

		// Token: 0x04002686 RID: 9862
		private int _symbolCount;

		// Token: 0x04002687 RID: 9863
		private int _imeCharCount;

		// Token: 0x04002688 RID: 9864
		private TextElement _textElement;

		// Token: 0x04002689 RID: 9865
		private ElementEdge _edgeReferenceCounts;
	}
}
