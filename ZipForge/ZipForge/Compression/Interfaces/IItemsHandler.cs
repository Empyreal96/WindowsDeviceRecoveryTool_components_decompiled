using System;
using System.IO;

namespace ComponentAce.Compression.Interfaces
{
	// Token: 0x02000044 RID: 68
	public interface IItemsHandler
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060002DC RID: 732
		// (set) Token: 0x060002DD RID: 733
		IItemsArray ItemsArray { get; set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060002DE RID: 734
		// (set) Token: 0x060002DF RID: 735
		IItemsArray ItemsArrayBackup { get; set; }

		// Token: 0x060002E0 RID: 736
		void LoadItemsArray();

		// Token: 0x060002E1 RID: 737
		void SaveItemsArray();

		// Token: 0x060002E2 RID: 738
		void SaveItemsArray(Stream stream);
	}
}
