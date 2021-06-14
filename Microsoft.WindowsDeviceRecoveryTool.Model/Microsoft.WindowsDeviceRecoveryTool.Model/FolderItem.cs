using System;
using System.Collections.ObjectModel;

namespace Microsoft.WindowsDeviceRecoveryTool.Model
{
	// Token: 0x0200001A RID: 26
	public class FolderItem
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00003324 File Offset: 0x00001524
		// (set) Token: 0x060000B1 RID: 177 RVA: 0x0000333B File Offset: 0x0000153B
		public string Title { get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00003344 File Offset: 0x00001544
		// (set) Token: 0x060000B3 RID: 179 RVA: 0x0000335B File Offset: 0x0000155B
		public string Path { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00003364 File Offset: 0x00001564
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x0000337B File Offset: 0x0000157B
		public FolderType Type { get; set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00003384 File Offset: 0x00001584
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x0000339B File Offset: 0x0000159B
		public bool IsExtended { get; set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x000033A4 File Offset: 0x000015A4
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x000033BB File Offset: 0x000015BB
		public ObservableCollection<FolderItem> Items { get; set; }

		// Token: 0x060000BA RID: 186 RVA: 0x000033C4 File Offset: 0x000015C4
		public override string ToString()
		{
			return this.Title;
		}
	}
}
