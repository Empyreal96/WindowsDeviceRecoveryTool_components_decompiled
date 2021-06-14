using System;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;

namespace Microsoft.WindowsDeviceRecoveryTool.Model
{
	// Token: 0x0200004C RID: 76
	public class PlatformId
	{
		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600025C RID: 604 RVA: 0x00006C90 File Offset: 0x00004E90
		// (set) Token: 0x0600025D RID: 605 RVA: 0x00006CA7 File Offset: 0x00004EA7
		public string Manufacturer { get; private set; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600025E RID: 606 RVA: 0x00006CB0 File Offset: 0x00004EB0
		// (set) Token: 0x0600025F RID: 607 RVA: 0x00006CC7 File Offset: 0x00004EC7
		public string Family { get; private set; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000260 RID: 608 RVA: 0x00006CD0 File Offset: 0x00004ED0
		// (set) Token: 0x06000261 RID: 609 RVA: 0x00006CE7 File Offset: 0x00004EE7
		public string ProductName { get; private set; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000262 RID: 610 RVA: 0x00006CF0 File Offset: 0x00004EF0
		// (set) Token: 0x06000263 RID: 611 RVA: 0x00006D07 File Offset: 0x00004F07
		public string Version { get; private set; }

		// Token: 0x06000264 RID: 612 RVA: 0x00006D10 File Offset: 0x00004F10
		public void SetPlatformId(string platformId)
		{
			if (string.Equals("*", platformId, StringComparison.InvariantCultureIgnoreCase))
			{
				this.fullPlatformId = platformId;
				this.Manufacturer = (this.Family = (this.ProductName = (this.Version = string.Empty)));
			}
			else
			{
				string[] array = platformId.Split(new char[]
				{
					'.'
				}, 4);
				if (array.Length < 3)
				{
					Tracer<PlatformId>.WriteWarning("Platform ID is incorrect: {0}", new object[]
					{
						platformId
					});
				}
				this.Manufacturer = ((array.Length >= 1) ? array[0] : string.Empty);
				this.Family = ((array.Length >= 2) ? array[1] : string.Empty);
				this.ProductName = ((array.Length >= 3) ? array[2] : string.Empty);
				this.Version = ((array.Length >= 4) ? array[3] : string.Empty);
				this.fullPlatformId = platformId;
			}
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00006E10 File Offset: 0x00005010
		public bool IsCompatibleWithDevicePlatformId(PlatformId devicePlatformId)
		{
			bool result;
			if (string.Equals(string.Empty, this.fullPlatformId, StringComparison.InvariantCultureIgnoreCase) || string.Equals("*", this.fullPlatformId, StringComparison.InvariantCultureIgnoreCase))
			{
				result = true;
			}
			else if (string.Compare(this.Manufacturer, devicePlatformId.Manufacturer, StringComparison.CurrentCultureIgnoreCase) != 0)
			{
				Tracer<PlatformId>.WriteVerbose("Platform ID manufacturers do not match: {0} - {1}", new object[]
				{
					this.Manufacturer,
					devicePlatformId.Manufacturer
				});
				result = false;
			}
			else if (string.Compare(this.Family, devicePlatformId.Family, StringComparison.CurrentCultureIgnoreCase) != 0)
			{
				Tracer<PlatformId>.WriteVerbose("Platform ID families do not match: {0} - {1}", new object[]
				{
					this.Family,
					devicePlatformId.Family
				});
				result = false;
			}
			else if (string.Compare(this.ProductName, devicePlatformId.ProductName, StringComparison.CurrentCultureIgnoreCase) != 0)
			{
				Tracer<PlatformId>.WriteVerbose("Platform ID product names do not match: {0} - {1}", new object[]
				{
					this.ProductName,
					devicePlatformId.ProductName
				});
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00006F28 File Offset: 0x00005128
		public override string ToString()
		{
			return this.fullPlatformId;
		}

		// Token: 0x0400021D RID: 541
		public const string WildcardPlatformId = "*";

		// Token: 0x0400021E RID: 542
		private string fullPlatformId;
	}
}
