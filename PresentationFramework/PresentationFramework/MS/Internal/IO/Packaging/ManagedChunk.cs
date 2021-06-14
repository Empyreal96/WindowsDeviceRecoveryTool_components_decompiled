using System;
using MS.Internal.Interop;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x02000665 RID: 1637
	internal class ManagedChunk
	{
		// Token: 0x06006C6C RID: 27756 RVA: 0x001F3B58 File Offset: 0x001F1D58
		internal ManagedChunk(uint index, CHUNK_BREAKTYPE breakType, ManagedFullPropSpec attribute, uint lcid, CHUNKSTATE flags)
		{
			Invariant.Assert(breakType >= CHUNK_BREAKTYPE.CHUNK_NO_BREAK && breakType <= CHUNK_BREAKTYPE.CHUNK_EOC);
			Invariant.Assert(attribute != null);
			this._index = index;
			this._breakType = breakType;
			this._lcid = lcid;
			this._attribute = attribute;
			this._flags = flags;
			this._idChunkSource = this._index;
		}

		// Token: 0x170019E7 RID: 6631
		// (get) Token: 0x06006C6D RID: 27757 RVA: 0x001F3BB8 File Offset: 0x001F1DB8
		// (set) Token: 0x06006C6E RID: 27758 RVA: 0x001F3BC0 File Offset: 0x001F1DC0
		internal uint ID
		{
			get
			{
				return this._index;
			}
			set
			{
				this._index = value;
			}
		}

		// Token: 0x170019E8 RID: 6632
		// (get) Token: 0x06006C6F RID: 27759 RVA: 0x001F3BC9 File Offset: 0x001F1DC9
		internal CHUNKSTATE Flags
		{
			get
			{
				return this._flags;
			}
		}

		// Token: 0x170019E9 RID: 6633
		// (get) Token: 0x06006C70 RID: 27760 RVA: 0x001F3BD1 File Offset: 0x001F1DD1
		// (set) Token: 0x06006C71 RID: 27761 RVA: 0x001F3BD9 File Offset: 0x001F1DD9
		internal CHUNK_BREAKTYPE BreakType
		{
			get
			{
				return this._breakType;
			}
			set
			{
				Invariant.Assert(value >= CHUNK_BREAKTYPE.CHUNK_NO_BREAK && value <= CHUNK_BREAKTYPE.CHUNK_EOC);
				this._breakType = value;
			}
		}

		// Token: 0x170019EA RID: 6634
		// (get) Token: 0x06006C72 RID: 27762 RVA: 0x001F3BF5 File Offset: 0x001F1DF5
		// (set) Token: 0x06006C73 RID: 27763 RVA: 0x001F3BFD File Offset: 0x001F1DFD
		internal uint Locale
		{
			get
			{
				return this._lcid;
			}
			set
			{
				this._lcid = value;
			}
		}

		// Token: 0x170019EB RID: 6635
		// (get) Token: 0x06006C74 RID: 27764 RVA: 0x001F3C06 File Offset: 0x001F1E06
		internal uint ChunkSource
		{
			get
			{
				return this._idChunkSource;
			}
		}

		// Token: 0x170019EC RID: 6636
		// (get) Token: 0x06006C75 RID: 27765 RVA: 0x001F3C0E File Offset: 0x001F1E0E
		internal uint StartSource
		{
			get
			{
				return this._startSource;
			}
		}

		// Token: 0x170019ED RID: 6637
		// (get) Token: 0x06006C76 RID: 27766 RVA: 0x001F3C16 File Offset: 0x001F1E16
		internal uint LenSource
		{
			get
			{
				return this._lenSource;
			}
		}

		// Token: 0x170019EE RID: 6638
		// (get) Token: 0x06006C77 RID: 27767 RVA: 0x001F3C1E File Offset: 0x001F1E1E
		// (set) Token: 0x06006C78 RID: 27768 RVA: 0x001F3C26 File Offset: 0x001F1E26
		internal ManagedFullPropSpec Attribute
		{
			get
			{
				return this._attribute;
			}
			set
			{
				this._attribute = value;
			}
		}

		// Token: 0x0400353F RID: 13631
		private uint _index;

		// Token: 0x04003540 RID: 13632
		private CHUNK_BREAKTYPE _breakType;

		// Token: 0x04003541 RID: 13633
		private CHUNKSTATE _flags;

		// Token: 0x04003542 RID: 13634
		private uint _lcid;

		// Token: 0x04003543 RID: 13635
		private ManagedFullPropSpec _attribute;

		// Token: 0x04003544 RID: 13636
		private uint _idChunkSource;

		// Token: 0x04003545 RID: 13637
		private uint _startSource;

		// Token: 0x04003546 RID: 13638
		private uint _lenSource;
	}
}
