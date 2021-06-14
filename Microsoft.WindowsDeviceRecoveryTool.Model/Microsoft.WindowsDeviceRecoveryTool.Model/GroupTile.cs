using System;
using System.Collections.Generic;

namespace Microsoft.WindowsDeviceRecoveryTool.Model
{
	// Token: 0x0200001C RID: 28
	public class GroupTile : Tile
	{
		// Token: 0x060000D7 RID: 215 RVA: 0x00003738 File Offset: 0x00001938
		public GroupTile(IEnumerable<Tile> tilesInGroup)
		{
			if (tilesInGroup == null)
			{
				throw new ArgumentNullException("tilesInGroup");
			}
			this.TilesInGroup = tilesInGroup;
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00003770 File Offset: 0x00001970
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x00003787 File Offset: 0x00001987
		public IEnumerable<Tile> TilesInGroup { get; private set; }
	}
}
