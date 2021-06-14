using System;
using System.Collections.Generic;

namespace Microsoft.WindowsPhone.Imaging
{
	// Token: 0x02000024 RID: 36
	public class EqualityComparerPublishingPackage : EqualityComparer<PublishingPackageInfo>
	{
		// Token: 0x060001BC RID: 444 RVA: 0x0000A031 File Offset: 0x00008231
		public EqualityComparerPublishingPackage(PublishingPackageInfo.PublishingPackageInfoComparison compareType)
		{
			this._compareType = compareType;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000A040 File Offset: 0x00008240
		public override bool Equals(PublishingPackageInfo x, PublishingPackageInfo y)
		{
			if (x == null)
			{
				return y == null;
			}
			return x.Equals(y, this._compareType);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000A059 File Offset: 0x00008259
		public override int GetHashCode(PublishingPackageInfo pkg)
		{
			return pkg.GetHashCode(this._compareType);
		}

		// Token: 0x04000107 RID: 263
		private PublishingPackageInfo.PublishingPackageInfoComparison _compareType;
	}
}
