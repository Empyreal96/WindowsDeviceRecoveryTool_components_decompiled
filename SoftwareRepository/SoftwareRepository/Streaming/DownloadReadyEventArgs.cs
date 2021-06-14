using System;

namespace SoftwareRepository.Streaming
{
	// Token: 0x02000010 RID: 16
	public class DownloadReadyEventArgs : EventArgs
	{
		// Token: 0x06000059 RID: 89 RVA: 0x00003109 File Offset: 0x00001309
		public DownloadReadyEventArgs(string packageId, string fileName)
		{
			this.PackageId = packageId;
			this.FileName = fileName;
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600005A RID: 90 RVA: 0x0000311F File Offset: 0x0000131F
		// (set) Token: 0x0600005B RID: 91 RVA: 0x00003127 File Offset: 0x00001327
		public string PackageId { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00003130 File Offset: 0x00001330
		// (set) Token: 0x0600005D RID: 93 RVA: 0x00003138 File Offset: 0x00001338
		public string FileName { get; set; }
	}
}
