using System;
using System.IO;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000020 RID: 32
	public class ImagePartition
	{
		// Token: 0x06000112 RID: 274 RVA: 0x00007789 File Offset: 0x00005989
		protected ImagePartition()
		{
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00007791 File Offset: 0x00005991
		public ImagePartition(string name, string root)
		{
			this.Name = name;
			this.Root = root;
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000114 RID: 276 RVA: 0x000077A7 File Offset: 0x000059A7
		// (set) Token: 0x06000115 RID: 277 RVA: 0x000077AF File Offset: 0x000059AF
		public string PhysicalDeviceId { get; protected set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000116 RID: 278 RVA: 0x000077B8 File Offset: 0x000059B8
		// (set) Token: 0x06000117 RID: 279 RVA: 0x000077C0 File Offset: 0x000059C0
		public string Name { get; protected set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000118 RID: 280 RVA: 0x000077C9 File Offset: 0x000059C9
		// (set) Token: 0x06000119 RID: 281 RVA: 0x000077D1 File Offset: 0x000059D1
		public string Root { get; protected set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600011A RID: 282 RVA: 0x000077DA File Offset: 0x000059DA
		// (set) Token: 0x0600011B RID: 283 RVA: 0x000077E2 File Offset: 0x000059E2
		public DriveInfo MountedDriveInfo { get; protected set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600011C RID: 284 RVA: 0x000077EC File Offset: 0x000059EC
		public FileInfo[] Files
		{
			get
			{
				if (this._files == null && !string.IsNullOrEmpty(this.Root))
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(this.Root);
					this._files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
				}
				return this._files;
			}
		}

		// Token: 0x04000063 RID: 99
		private FileInfo[] _files;
	}
}
