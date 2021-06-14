using System;
using MS.Internal.Interop;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x02000660 RID: 1632
	internal class IndexingContentUnit : ManagedChunk
	{
		// Token: 0x06006C2F RID: 27695 RVA: 0x001F1EDE File Offset: 0x001F00DE
		internal IndexingContentUnit(string contents, uint chunkID, CHUNK_BREAKTYPE breakType, ManagedFullPropSpec attribute, uint lcid) : base(chunkID, breakType, attribute, lcid, CHUNKSTATE.CHUNK_TEXT)
		{
			this._contents = contents;
		}

		// Token: 0x06006C30 RID: 27696 RVA: 0x001F1EF4 File Offset: 0x001F00F4
		internal void InitIndexingContentUnit(string contents, uint chunkID, CHUNK_BREAKTYPE breakType, ManagedFullPropSpec attribute, uint lcid)
		{
			this._contents = contents;
			base.ID = chunkID;
			base.BreakType = breakType;
			base.Attribute = attribute;
			base.Locale = lcid;
		}

		// Token: 0x170019E0 RID: 6624
		// (get) Token: 0x06006C31 RID: 27697 RVA: 0x001F1F1B File Offset: 0x001F011B
		internal string Text
		{
			get
			{
				return this._contents;
			}
		}

		// Token: 0x04003519 RID: 13593
		private string _contents;
	}
}
