using System;
using System.Xml.Linq;

namespace Microsoft.WindowsAzure.Storage.Shared.Protocol
{
	// Token: 0x02000167 RID: 359
	public sealed class ServiceStats
	{
		// Token: 0x0600154A RID: 5450 RVA: 0x00050F00 File Offset: 0x0004F100
		private ServiceStats()
		{
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x0600154B RID: 5451 RVA: 0x00050F08 File Offset: 0x0004F108
		// (set) Token: 0x0600154C RID: 5452 RVA: 0x00050F10 File Offset: 0x0004F110
		public GeoReplicationStats GeoReplication { get; private set; }

		// Token: 0x0600154D RID: 5453 RVA: 0x00050F1C File Offset: 0x0004F11C
		internal static ServiceStats FromServiceXml(XDocument serviceStatsDocument)
		{
			XElement xelement = serviceStatsDocument.Element("StorageServiceStats");
			return new ServiceStats
			{
				GeoReplication = GeoReplicationStats.ReadGeoReplicationStatsFromXml(xelement.Element("GeoReplication"))
			};
		}

		// Token: 0x040009C6 RID: 2502
		private const string StorageServiceStatsName = "StorageServiceStats";

		// Token: 0x040009C7 RID: 2503
		private const string GeoReplicationName = "GeoReplication";
	}
}
