using System;
using System.Globalization;
using System.Xml.Linq;

namespace Microsoft.WindowsAzure.Storage.Shared.Protocol
{
	// Token: 0x0200015E RID: 350
	public sealed class GeoReplicationStats
	{
		// Token: 0x06001517 RID: 5399 RVA: 0x0005022C File Offset: 0x0004E42C
		private GeoReplicationStats()
		{
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06001518 RID: 5400 RVA: 0x00050234 File Offset: 0x0004E434
		// (set) Token: 0x06001519 RID: 5401 RVA: 0x0005023C File Offset: 0x0004E43C
		public GeoReplicationStatus Status { get; private set; }

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x0600151A RID: 5402 RVA: 0x00050245 File Offset: 0x0004E445
		// (set) Token: 0x0600151B RID: 5403 RVA: 0x0005024D File Offset: 0x0004E44D
		public DateTimeOffset? LastSyncTime { get; private set; }

		// Token: 0x0600151C RID: 5404 RVA: 0x00050258 File Offset: 0x0004E458
		internal static GeoReplicationStatus GetGeoReplicationStatus(string geoReplicationStatus)
		{
			if (geoReplicationStatus != null)
			{
				if (geoReplicationStatus == "unavailable")
				{
					return GeoReplicationStatus.Unavailable;
				}
				if (geoReplicationStatus == "live")
				{
					return GeoReplicationStatus.Live;
				}
				if (geoReplicationStatus == "bootstrap")
				{
					return GeoReplicationStatus.Bootstrap;
				}
			}
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid geo-replication status in response: '{0}'", new object[]
			{
				geoReplicationStatus
			}), "geoReplicationStatus");
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x000502C0 File Offset: 0x0004E4C0
		internal static GeoReplicationStats ReadGeoReplicationStatsFromXml(XElement element)
		{
			string value = element.Element("LastSyncTime").Value;
			return new GeoReplicationStats
			{
				Status = GeoReplicationStats.GetGeoReplicationStatus(element.Element("Status").Value),
				LastSyncTime = (string.IsNullOrEmpty(value) ? null : new DateTimeOffset?(DateTimeOffset.Parse(value, CultureInfo.InvariantCulture)))
			};
		}

		// Token: 0x04000994 RID: 2452
		private const string StatusName = "Status";

		// Token: 0x04000995 RID: 2453
		private const string LastSyncTimeName = "LastSyncTime";
	}
}
