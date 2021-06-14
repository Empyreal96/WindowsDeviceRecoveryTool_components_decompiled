using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000425 RID: 1061
	internal class TextTreeRootTextBlock : SplayTreeNode
	{
		// Token: 0x06003DB2 RID: 15794 RVA: 0x0011CA7C File Offset: 0x0011AC7C
		internal TextTreeRootTextBlock()
		{
			TextTreeTextBlock textTreeTextBlock = new TextTreeTextBlock(2);
			textTreeTextBlock.InsertAtNode(this, ElementEdge.AfterStart);
		}

		// Token: 0x17000F4F RID: 3919
		// (get) Token: 0x06003DB3 RID: 15795 RVA: 0x0000C238 File Offset: 0x0000A438
		// (set) Token: 0x06003DB4 RID: 15796 RVA: 0x0011CA9E File Offset: 0x0011AC9E
		internal override SplayTreeNode ParentNode
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "Can't set ParentNode on TextBlock root!");
			}
		}

		// Token: 0x17000F50 RID: 3920
		// (get) Token: 0x06003DB5 RID: 15797 RVA: 0x0011CAAB File Offset: 0x0011ACAB
		// (set) Token: 0x06003DB6 RID: 15798 RVA: 0x0011CAB3 File Offset: 0x0011ACB3
		internal override SplayTreeNode ContainedNode
		{
			get
			{
				return this._containedNode;
			}
			set
			{
				this._containedNode = (TextTreeTextBlock)value;
			}
		}

		// Token: 0x17000F51 RID: 3921
		// (get) Token: 0x06003DB7 RID: 15799 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003DB8 RID: 15800 RVA: 0x0011C46B File Offset: 0x0011A66B
		internal override int LeftSymbolCount
		{
			get
			{
				return 0;
			}
			set
			{
				Invariant.Assert(false, "TextContainer root is never a sibling!");
			}
		}

		// Token: 0x17000F52 RID: 3922
		// (get) Token: 0x06003DB9 RID: 15801 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003DBA RID: 15802 RVA: 0x0011CAC1 File Offset: 0x0011ACC1
		internal override int LeftCharCount
		{
			get
			{
				return 0;
			}
			set
			{
				Invariant.Assert(value == 0);
			}
		}

		// Token: 0x17000F53 RID: 3923
		// (get) Token: 0x06003DBB RID: 15803 RVA: 0x0000C238 File Offset: 0x0000A438
		// (set) Token: 0x06003DBC RID: 15804 RVA: 0x0011CACC File Offset: 0x0011ACCC
		internal override SplayTreeNode LeftChildNode
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "TextBlock root never has sibling nodes!");
			}
		}

		// Token: 0x17000F54 RID: 3924
		// (get) Token: 0x06003DBD RID: 15805 RVA: 0x0000C238 File Offset: 0x0000A438
		// (set) Token: 0x06003DBE RID: 15806 RVA: 0x0011CACC File Offset: 0x0011ACCC
		internal override SplayTreeNode RightChildNode
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "TextBlock root never has sibling nodes!");
			}
		}

		// Token: 0x17000F55 RID: 3925
		// (get) Token: 0x06003DBF RID: 15807 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003DC0 RID: 15808 RVA: 0x0011CAD9 File Offset: 0x0011ACD9
		internal override uint Generation
		{
			get
			{
				return 0U;
			}
			set
			{
				Invariant.Assert(false, "TextTreeRootTextBlock does not track Generation!");
			}
		}

		// Token: 0x17000F56 RID: 3926
		// (get) Token: 0x06003DC1 RID: 15809 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003DC2 RID: 15810 RVA: 0x0011CAE6 File Offset: 0x0011ACE6
		internal override int SymbolOffsetCache
		{
			get
			{
				return 0;
			}
			set
			{
				Invariant.Assert(false, "TextTreeRootTextBlock does not track SymbolOffsetCache!");
			}
		}

		// Token: 0x17000F57 RID: 3927
		// (get) Token: 0x06003DC3 RID: 15811 RVA: 0x0011BEC8 File Offset: 0x0011A0C8
		// (set) Token: 0x06003DC4 RID: 15812 RVA: 0x0011CAF3 File Offset: 0x0011ACF3
		internal override int SymbolCount
		{
			get
			{
				return -1;
			}
			set
			{
				Invariant.Assert(false, "TextTreeRootTextBlock does not track symbol count!");
			}
		}

		// Token: 0x17000F58 RID: 3928
		// (get) Token: 0x06003DC5 RID: 15813 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003DC6 RID: 15814 RVA: 0x0011CAC1 File Offset: 0x0011ACC1
		internal override int IMECharCount
		{
			get
			{
				return 0;
			}
			set
			{
				Invariant.Assert(value == 0);
			}
		}

		// Token: 0x04002675 RID: 9845
		private TextTreeTextBlock _containedNode;
	}
}
