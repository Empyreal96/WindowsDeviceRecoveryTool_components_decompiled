using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000426 RID: 1062
	internal class TextTreeTextBlock : SplayTreeNode
	{
		// Token: 0x06003DC7 RID: 15815 RVA: 0x0011CB00 File Offset: 0x0011AD00
		internal TextTreeTextBlock(int size)
		{
			Invariant.Assert(size > 0);
			Invariant.Assert(size <= 4096);
			this._text = new char[size];
			this._gapSize = size;
		}

		// Token: 0x06003DC8 RID: 15816 RVA: 0x0011CB34 File Offset: 0x0011AD34
		internal int InsertText(int logicalOffset, object text, int textStartIndex, int textEndIndex)
		{
			Invariant.Assert(text is string || text is char[], "Bad text parameter!");
			Invariant.Assert(textStartIndex <= textEndIndex, "Bad start/end index!");
			base.Splay();
			int num = textEndIndex - textStartIndex;
			if (this._text.Length < 4096 && num > this._gapSize)
			{
				char[] array = new char[Math.Min(this.Count + num, 4096)];
				Array.Copy(this._text, 0, array, 0, this._gapOffset);
				int num2 = this._text.Length - (this._gapOffset + this._gapSize);
				Array.Copy(this._text, this._gapOffset + this._gapSize, array, array.Length - num2, num2);
				this._gapSize += array.Length - this._text.Length;
				this._text = array;
			}
			if (logicalOffset != this._gapOffset)
			{
				this.MoveGap(logicalOffset);
			}
			num = Math.Min(num, this._gapSize);
			string text2 = text as string;
			if (text2 != null)
			{
				text2.CopyTo(textStartIndex, this._text, logicalOffset, num);
			}
			else
			{
				char[] sourceArray = (char[])text;
				Array.Copy(sourceArray, textStartIndex, this._text, logicalOffset, num);
			}
			this._gapOffset += num;
			this._gapSize -= num;
			return num;
		}

		// Token: 0x06003DC9 RID: 15817 RVA: 0x0011CC8C File Offset: 0x0011AE8C
		internal TextTreeTextBlock SplitBlock()
		{
			Invariant.Assert(this._gapSize == 0, "Splitting non-full block!");
			Invariant.Assert(this._text.Length == 4096, "Splitting non-max sized block!");
			TextTreeTextBlock textTreeTextBlock = new TextTreeTextBlock(4096);
			bool insertBefore;
			if (this._gapOffset < 2048)
			{
				Array.Copy(this._text, 0, textTreeTextBlock._text, 0, this._gapOffset);
				textTreeTextBlock._gapOffset = this._gapOffset;
				textTreeTextBlock._gapSize = 4096 - this._gapOffset;
				this._gapSize += this._gapOffset;
				this._gapOffset = 0;
				insertBefore = true;
			}
			else
			{
				Array.Copy(this._text, this._gapOffset, textTreeTextBlock._text, this._gapOffset, 4096 - this._gapOffset);
				Invariant.Assert(textTreeTextBlock._gapOffset == 0);
				textTreeTextBlock._gapSize = this._gapOffset;
				this._gapSize = 4096 - this._gapOffset;
				insertBefore = false;
			}
			textTreeTextBlock.InsertAtNode(this, insertBefore);
			return textTreeTextBlock;
		}

		// Token: 0x06003DCA RID: 15818 RVA: 0x0011CD94 File Offset: 0x0011AF94
		internal void RemoveText(int logicalOffset, int count)
		{
			Invariant.Assert(logicalOffset >= 0);
			Invariant.Assert(count >= 0);
			Invariant.Assert(logicalOffset + count <= this.Count, "Removing too much text!");
			int num = count;
			int count2 = this.Count;
			base.Splay();
			if (logicalOffset < this._gapOffset)
			{
				if (logicalOffset + count < this._gapOffset)
				{
					this.MoveGap(logicalOffset + count);
				}
				int num2 = (logicalOffset + count == this._gapOffset) ? count : (this._gapOffset - logicalOffset);
				this._gapOffset -= num2;
				this._gapSize += num2;
				logicalOffset = this._gapOffset;
				count -= num2;
			}
			logicalOffset += this._gapSize;
			if (logicalOffset > this._gapOffset + this._gapSize)
			{
				this.MoveGap(logicalOffset - this._gapSize);
			}
			this._gapSize += count;
			Invariant.Assert(this._gapOffset + this._gapSize <= this._text.Length);
			Invariant.Assert(count2 == this.Count + num);
		}

		// Token: 0x06003DCB RID: 15819 RVA: 0x0011CEA4 File Offset: 0x0011B0A4
		internal int ReadText(int logicalOffset, int count, char[] chars, int charsStartIndex)
		{
			int num = count;
			if (logicalOffset < this._gapOffset)
			{
				int num2 = Math.Min(count, this._gapOffset - logicalOffset);
				Array.Copy(this._text, logicalOffset, chars, charsStartIndex, num2);
				count -= num2;
				charsStartIndex += num2;
				logicalOffset = this._gapOffset;
			}
			if (count > 0)
			{
				logicalOffset += this._gapSize;
				int num2 = Math.Min(count, this._text.Length - logicalOffset);
				Array.Copy(this._text, logicalOffset, chars, charsStartIndex, num2);
				count -= num2;
			}
			return num - count;
		}

		// Token: 0x17000F59 RID: 3929
		// (get) Token: 0x06003DCC RID: 15820 RVA: 0x0011CF25 File Offset: 0x0011B125
		// (set) Token: 0x06003DCD RID: 15821 RVA: 0x0011CF2D File Offset: 0x0011B12D
		internal override SplayTreeNode ParentNode
		{
			get
			{
				return this._parentNode;
			}
			set
			{
				this._parentNode = value;
			}
		}

		// Token: 0x17000F5A RID: 3930
		// (get) Token: 0x06003DCE RID: 15822 RVA: 0x0000C238 File Offset: 0x0000A438
		// (set) Token: 0x06003DCF RID: 15823 RVA: 0x0011CF36 File Offset: 0x0011B136
		internal override SplayTreeNode ContainedNode
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "Can't set ContainedNode on a TextTreeTextBlock!");
			}
		}

		// Token: 0x17000F5B RID: 3931
		// (get) Token: 0x06003DD0 RID: 15824 RVA: 0x0011CF43 File Offset: 0x0011B143
		// (set) Token: 0x06003DD1 RID: 15825 RVA: 0x0011CF4B File Offset: 0x0011B14B
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

		// Token: 0x17000F5C RID: 3932
		// (get) Token: 0x06003DD2 RID: 15826 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003DD3 RID: 15827 RVA: 0x0011CAC1 File Offset: 0x0011ACC1
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

		// Token: 0x17000F5D RID: 3933
		// (get) Token: 0x06003DD4 RID: 15828 RVA: 0x0011CF54 File Offset: 0x0011B154
		// (set) Token: 0x06003DD5 RID: 15829 RVA: 0x0011CF5C File Offset: 0x0011B15C
		internal override SplayTreeNode LeftChildNode
		{
			get
			{
				return this._leftChildNode;
			}
			set
			{
				this._leftChildNode = (TextTreeTextBlock)value;
			}
		}

		// Token: 0x17000F5E RID: 3934
		// (get) Token: 0x06003DD6 RID: 15830 RVA: 0x0011CF6A File Offset: 0x0011B16A
		// (set) Token: 0x06003DD7 RID: 15831 RVA: 0x0011CF72 File Offset: 0x0011B172
		internal override SplayTreeNode RightChildNode
		{
			get
			{
				return this._rightChildNode;
			}
			set
			{
				this._rightChildNode = (TextTreeTextBlock)value;
			}
		}

		// Token: 0x17000F5F RID: 3935
		// (get) Token: 0x06003DD8 RID: 15832 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003DD9 RID: 15833 RVA: 0x0011CF80 File Offset: 0x0011B180
		internal override uint Generation
		{
			get
			{
				return 0U;
			}
			set
			{
				Invariant.Assert(false, "TextTreeTextBlock does not track Generation!");
			}
		}

		// Token: 0x17000F60 RID: 3936
		// (get) Token: 0x06003DDA RID: 15834 RVA: 0x0011BEC8 File Offset: 0x0011A0C8
		// (set) Token: 0x06003DDB RID: 15835 RVA: 0x0011CF8D File Offset: 0x0011B18D
		internal override int SymbolOffsetCache
		{
			get
			{
				return -1;
			}
			set
			{
				Invariant.Assert(false, "TextTreeTextBlock does not track SymbolOffsetCache!");
			}
		}

		// Token: 0x17000F61 RID: 3937
		// (get) Token: 0x06003DDC RID: 15836 RVA: 0x0011CF9A File Offset: 0x0011B19A
		// (set) Token: 0x06003DDD RID: 15837 RVA: 0x0011CFA2 File Offset: 0x0011B1A2
		internal override int SymbolCount
		{
			get
			{
				return this.Count;
			}
			set
			{
				Invariant.Assert(false, "Can't set SymbolCount on TextTreeTextBlock!");
			}
		}

		// Token: 0x17000F62 RID: 3938
		// (get) Token: 0x06003DDE RID: 15838 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003DDF RID: 15839 RVA: 0x0011CAC1 File Offset: 0x0011ACC1
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

		// Token: 0x17000F63 RID: 3939
		// (get) Token: 0x06003DE0 RID: 15840 RVA: 0x0011CFAF File Offset: 0x0011B1AF
		internal int Count
		{
			get
			{
				return this._text.Length - this._gapSize;
			}
		}

		// Token: 0x17000F64 RID: 3940
		// (get) Token: 0x06003DE1 RID: 15841 RVA: 0x0011CFC0 File Offset: 0x0011B1C0
		internal int FreeCapacity
		{
			get
			{
				return this._gapSize;
			}
		}

		// Token: 0x17000F65 RID: 3941
		// (get) Token: 0x06003DE2 RID: 15842 RVA: 0x0011CFC8 File Offset: 0x0011B1C8
		internal int GapOffset
		{
			get
			{
				return this._gapOffset;
			}
		}

		// Token: 0x06003DE3 RID: 15843 RVA: 0x0011CFD0 File Offset: 0x0011B1D0
		private void MoveGap(int offset)
		{
			int sourceIndex;
			int destinationIndex;
			int length;
			if (offset < this._gapOffset)
			{
				sourceIndex = offset;
				destinationIndex = offset + this._gapSize;
				length = this._gapOffset - offset;
			}
			else
			{
				sourceIndex = this._gapOffset + this._gapSize;
				destinationIndex = this._gapOffset;
				length = offset - this._gapOffset;
			}
			Array.Copy(this._text, sourceIndex, this._text, destinationIndex, length);
			this._gapOffset = offset;
		}

		// Token: 0x04002676 RID: 9846
		private int _leftSymbolCount;

		// Token: 0x04002677 RID: 9847
		private SplayTreeNode _parentNode;

		// Token: 0x04002678 RID: 9848
		private TextTreeTextBlock _leftChildNode;

		// Token: 0x04002679 RID: 9849
		private TextTreeTextBlock _rightChildNode;

		// Token: 0x0400267A RID: 9850
		private char[] _text;

		// Token: 0x0400267B RID: 9851
		private int _gapOffset;

		// Token: 0x0400267C RID: 9852
		private int _gapSize;

		// Token: 0x0400267D RID: 9853
		internal const int MaxBlockSize = 4096;
	}
}
