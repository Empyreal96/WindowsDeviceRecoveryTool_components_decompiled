using System;
using System.Text;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage
{
	// Token: 0x02000004 RID: 4
	public class DownloadParameters
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000025 RID: 37 RVA: 0x0000282C File Offset: 0x00000A2C
		// (set) Token: 0x06000026 RID: 38 RVA: 0x00002843 File Offset: 0x00000A43
		public QueryParameters DiscoveryParameters { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000027 RID: 39 RVA: 0x0000284C File Offset: 0x00000A4C
		// (set) Token: 0x06000028 RID: 40 RVA: 0x00002863 File Offset: 0x00000A63
		public string FileExtension { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000029 RID: 41 RVA: 0x0000286C File Offset: 0x00000A6C
		// (set) Token: 0x0600002A RID: 42 RVA: 0x00002883 File Offset: 0x00000A83
		public string DestinationFolder { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600002B RID: 43 RVA: 0x0000288C File Offset: 0x00000A8C
		// (set) Token: 0x0600002C RID: 44 RVA: 0x000028A3 File Offset: 0x00000AA3
		public bool FilesVersioned { get; set; }

		// Token: 0x0600002D RID: 45 RVA: 0x000028AC File Offset: 0x00000AAC
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("FileExtension: {0}, ", this.FileExtension);
			stringBuilder.AppendFormat("DestinationFolder: {0}, ", this.DestinationFolder);
			stringBuilder.AppendFormat("DiscoveryParameters: {0}, ", this.DiscoveryParameters);
			return stringBuilder.ToString();
		}
	}
}
