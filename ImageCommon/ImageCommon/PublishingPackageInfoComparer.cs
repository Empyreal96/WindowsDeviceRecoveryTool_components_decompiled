using System;
using System.Collections.Generic;

namespace Microsoft.WindowsPhone.Imaging
{
	// Token: 0x02000023 RID: 35
	public class PublishingPackageInfoComparer : EqualityComparer<PublishingPackageInfo>
	{
		// Token: 0x060001B6 RID: 438 RVA: 0x00009FF6 File Offset: 0x000081F6
		protected PublishingPackageInfoComparer()
		{
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00009FFE File Offset: 0x000081FE
		public static EqualityComparer<PublishingPackageInfo> IgnorePaths
		{
			get
			{
				return new EqualityComparerPublishingPackage(PublishingPackageInfo.PublishingPackageInfoComparison.IgnorePaths);
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x0000A006 File Offset: 0x00008206
		public static EqualityComparer<PublishingPackageInfo> UniqueID
		{
			get
			{
				return new EqualityComparerPublishingPackage(PublishingPackageInfo.PublishingPackageInfoComparison.OnlyUniqueID);
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x0000A00E File Offset: 0x0000820E
		public static EqualityComparer<PublishingPackageInfo> UniqueIDAndFeatureID
		{
			get
			{
				return new EqualityComparerPublishingPackage(PublishingPackageInfo.PublishingPackageInfoComparison.OnlyUniqueIDAndFeatureID);
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000A016 File Offset: 0x00008216
		public override bool Equals(PublishingPackageInfo x, PublishingPackageInfo y)
		{
			if (x == null)
			{
				return y == null;
			}
			return x.Equals(y);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000A029 File Offset: 0x00008229
		public override int GetHashCode(PublishingPackageInfo pkg)
		{
			return pkg.GetHashCode();
		}
	}
}
