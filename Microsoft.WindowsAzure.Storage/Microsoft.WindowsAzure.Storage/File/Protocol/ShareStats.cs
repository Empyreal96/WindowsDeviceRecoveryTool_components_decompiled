using System;
using System.Globalization;
using System.Xml.Linq;

namespace Microsoft.WindowsAzure.Storage.File.Protocol
{
	// Token: 0x020000F2 RID: 242
	public sealed class ShareStats
	{
		// Token: 0x0600122B RID: 4651 RVA: 0x00043AC9 File Offset: 0x00041CC9
		private ShareStats()
		{
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x0600122C RID: 4652 RVA: 0x00043AD1 File Offset: 0x00041CD1
		// (set) Token: 0x0600122D RID: 4653 RVA: 0x00043AD9 File Offset: 0x00041CD9
		public int Usage { get; private set; }

		// Token: 0x0600122E RID: 4654 RVA: 0x00043AE4 File Offset: 0x00041CE4
		internal static ShareStats FromServiceXml(XDocument shareStatsDocument)
		{
			XElement xelement = shareStatsDocument.Element("ShareStats");
			return new ShareStats
			{
				Usage = int.Parse(xelement.Element("ShareUsage").Value, CultureInfo.InvariantCulture)
			};
		}

		// Token: 0x04000520 RID: 1312
		private const string ShareStatsName = "ShareStats";

		// Token: 0x04000521 RID: 1313
		private const string ShareUsageName = "ShareUsage";
	}
}
