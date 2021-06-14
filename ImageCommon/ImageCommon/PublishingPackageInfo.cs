using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.WindowsPhone.FeatureAPI;
using Microsoft.WindowsPhone.ImageUpdate.PkgCommon;
using Microsoft.WindowsPhone.ImageUpdate.Tools.Common;

namespace Microsoft.WindowsPhone.Imaging
{
	// Token: 0x0200001F RID: 31
	public class PublishingPackageInfo
	{
		// Token: 0x060001AC RID: 428 RVA: 0x000098D4 File Offset: 0x00007AD4
		public PublishingPackageInfo()
		{
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00009914 File Offset: 0x00007B14
		public PublishingPackageInfo(PublishingPackageInfo pkg)
		{
			this.UserInstallable = pkg.UserInstallable;
			this.FeatureID = pkg.FeatureID;
			this.FMID = pkg.FMID;
			this.ID = pkg.ID;
			this.IsFeatureIdentifierPackage = pkg.IsFeatureIdentifierPackage;
			this.OwnerType = pkg.OwnerType;
			this.Partition = pkg.Partition;
			this.Version = pkg.Version;
			this.Path = pkg.Path;
			this.PreviousPath = pkg.PreviousPath;
			this.ReleaseType = pkg.ReleaseType;
			this.SatelliteType = pkg.SatelliteType;
			this.SatelliteValue = pkg.SatelliteValue;
			this.SourceFMFile = pkg.SourceFMFile;
			this.UpdateType = pkg.UpdateType;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00009A14 File Offset: 0x00007C14
		public PublishingPackageInfo(FeatureManifest.FMPkgInfo pkgInfo, FMCollectionItem fmCollectionItem, string msPackageRoot, bool isUserInstallable = false)
		{
			char[] trimChars = new char[]
			{
				'\\'
			};
			string text = pkgInfo.PackagePath;
			text = text.Substring(0, text.Length - text.Substring(text.LastIndexOf(".")).Length);
			text += ".cab";
			if (!LongPathFile.Exists(pkgInfo.PackagePath) && !LongPathFile.Exists(text))
			{
				throw new FileNotFoundException("ImageCommon!PublishingPackageInfo: The package file '" + pkgInfo.PackagePath + "' could not be found.");
			}
			if (string.IsNullOrEmpty(pkgInfo.ID) || string.IsNullOrEmpty(pkgInfo.Partition) || pkgInfo.Version == null || pkgInfo.Version == VersionInfo.Empty || (LongPathFile.Exists(text) && string.Compare(text, pkgInfo.PackagePath) != 0))
			{
				IPkgInfo pkgInfo2 = Package.LoadFromCab(LongPathFile.Exists(text) ? text : pkgInfo.PackagePath);
				this.ID = pkgInfo2.Name;
				this.Partition = pkgInfo2.Partition;
				this.Version = pkgInfo2.Version;
			}
			else
			{
				this.ID = pkgInfo.ID;
				this.Partition = pkgInfo.Partition;
				this.Version = pkgInfo.Version.Value;
			}
			this.Path = pkgInfo.PackagePath.Replace(msPackageRoot, "").Trim(trimChars);
			this.SourceFMFile = System.IO.Path.GetFileName(fmCollectionItem.Path.ToUpper());
			switch (pkgInfo.FMGroup)
			{
			case 4:
				this.FeatureID = pkgInfo.FeatureID.Replace(4.ToString(), 7.ToString());
				break;
			case 5:
				this.FeatureID = pkgInfo.FeatureID.Replace(5.ToString(), 8.ToString());
				break;
			default:
				this.FeatureID = pkgInfo.FeatureID;
				break;
			}
			this.FMID = fmCollectionItem.ID;
			if (isUserInstallable)
			{
				this.FeatureID = this.UserInstallableFeatureIDPrefix + this.FeatureID;
			}
			this.OwnerType = fmCollectionItem.ownerType;
			this.ReleaseType = fmCollectionItem.releaseType;
			this.UserInstallable = fmCollectionItem.UserInstallable;
			if (!string.IsNullOrEmpty(pkgInfo.Language))
			{
				this.SatelliteType = PublishingPackageInfo.SatelliteTypes.Language;
				this.SatelliteValue = pkgInfo.Language;
			}
			else if (!string.IsNullOrEmpty(pkgInfo.Resolution))
			{
				this.SatelliteType = PublishingPackageInfo.SatelliteTypes.Resolution;
				this.SatelliteValue = pkgInfo.Resolution;
			}
			else
			{
				this.SatelliteType = PublishingPackageInfo.SatelliteTypes.Base;
				this.SatelliteValue = null;
			}
			this.IsFeatureIdentifierPackage = pkgInfo.FeatureIdentifierPackage;
			this.UpdateType = PublishingPackageInfo.UpdateTypes.Canonical;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00009D00 File Offset: 0x00007F00
		public bool Equals(PublishingPackageInfo pkg, PublishingPackageInfo.PublishingPackageInfoComparison compareType)
		{
			return this.ID.Equals(pkg.ID, StringComparison.OrdinalIgnoreCase) && this.Partition.Equals(pkg.Partition, StringComparison.OrdinalIgnoreCase) && (compareType == PublishingPackageInfo.PublishingPackageInfoComparison.OnlyUniqueID || (this.FeatureID.Equals(pkg.FeatureID, StringComparison.OrdinalIgnoreCase) && string.Equals(this.FMID, pkg.FMID, StringComparison.OrdinalIgnoreCase) && (compareType == PublishingPackageInfo.PublishingPackageInfoComparison.OnlyUniqueIDAndFeatureID || (this.UserInstallable == pkg.UserInstallable && this.OwnerType == pkg.OwnerType && this.ReleaseType == pkg.ReleaseType && this.SatelliteType == pkg.SatelliteType && string.Equals(this.SatelliteValue, pkg.SatelliteValue, StringComparison.OrdinalIgnoreCase) && (compareType == PublishingPackageInfo.PublishingPackageInfoComparison.IgnorePaths || (this.Path.Equals(pkg.Path, StringComparison.OrdinalIgnoreCase) && string.Equals(this.PreviousPath, pkg.PreviousPath, StringComparison.OrdinalIgnoreCase)))))));
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00009DEC File Offset: 0x00007FEC
		public int GetHashCode(PublishingPackageInfo.PublishingPackageInfoComparison compareType)
		{
			int num = this.ID.ToUpper().GetHashCode();
			num ^= this.Partition.ToUpper().GetHashCode();
			if (compareType != PublishingPackageInfo.PublishingPackageInfoComparison.OnlyUniqueID)
			{
				num ^= this.FeatureID.ToUpper().GetHashCode();
				if (!string.IsNullOrEmpty(this.FMID))
				{
					num ^= this.FMID.GetHashCode();
				}
			}
			if (compareType != PublishingPackageInfo.PublishingPackageInfoComparison.OnlyUniqueIDAndFeatureID && compareType != PublishingPackageInfo.PublishingPackageInfoComparison.OnlyUniqueID)
			{
				num ^= this.UserInstallable.GetHashCode();
				num ^= this.OwnerType.GetHashCode();
				num ^= this.ReleaseType.GetHashCode();
				num ^= this.SatelliteType.GetHashCode();
				if (!string.IsNullOrEmpty(this.SatelliteValue))
				{
					num ^= this.SatelliteValue.ToUpper().GetHashCode();
				}
				if (compareType != PublishingPackageInfo.PublishingPackageInfoComparison.IgnorePaths)
				{
					num ^= this.Path.ToUpper().GetHashCode();
					if (!string.IsNullOrEmpty(this.PreviousPath))
					{
						num ^= this.PreviousPath.ToUpper().GetHashCode();
					}
				}
			}
			return num;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00009EFB File Offset: 0x000080FB
		public PublishingPackageInfo SetPreviousPath(string path)
		{
			this.PreviousPath = path;
			return this;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00009F05 File Offset: 0x00008105
		public PublishingPackageInfo SetUpdateType(PublishingPackageInfo.UpdateTypes type)
		{
			this.UpdateType = type;
			return this;
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00009F0F File Offset: 0x0000810F
		[XmlIgnore]
		public string FeatureIDWithFMID
		{
			get
			{
				if (!string.IsNullOrEmpty(this.FMID))
				{
					return FeatureManifest.GetFeatureIDWithFMID(this.FeatureID, this.FMID);
				}
				return this.FeatureID;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x00009F38 File Offset: 0x00008138
		[XmlIgnore]
		public FeatureManifest.PackageGroups FMGroup
		{
			get
			{
				FeatureManifest.PackageGroups result = 0;
				if (!string.IsNullOrEmpty(this.FeatureID) && this.FeatureID.Contains('_'))
				{
					string text = this.FeatureID.Substring(0, this.FeatureID.IndexOf('_'));
					if (!Enum.TryParse<FeatureManifest.PackageGroups>(text, out result))
					{
						if (text.Equals("MS", StringComparison.OrdinalIgnoreCase))
						{
							result = 9;
						}
						else if (text.Equals("OEM", StringComparison.OrdinalIgnoreCase))
						{
							result = 10;
						}
					}
				}
				return result;
			}
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00009FAC File Offset: 0x000081AC
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				this.ID,
				" (",
				this.FeatureIDWithFMID,
				") : ",
				this.UpdateType
			});
		}

		// Token: 0x040000EB RID: 235
		public readonly string UserInstallableFeatureIDPrefix = "USERINSTALLABLE_";

		// Token: 0x040000EC RID: 236
		public string ID;

		// Token: 0x040000ED RID: 237
		[DefaultValue(false)]
		public bool IsFeatureIdentifierPackage;

		// Token: 0x040000EE RID: 238
		public string Path;

		// Token: 0x040000EF RID: 239
		[DefaultValue(null)]
		public string PreviousPath;

		// Token: 0x040000F0 RID: 240
		[DefaultValue("MainOS")]
		public string Partition = "MainOS";

		// Token: 0x040000F1 RID: 241
		public string FeatureID;

		// Token: 0x040000F2 RID: 242
		public string FMID;

		// Token: 0x040000F3 RID: 243
		public VersionInfo Version = VersionInfo.Empty;

		// Token: 0x040000F4 RID: 244
		[DefaultValue(2)]
		public ReleaseType ReleaseType = 2;

		// Token: 0x040000F5 RID: 245
		[DefaultValue(1)]
		public OwnerType OwnerType = 1;

		// Token: 0x040000F6 RID: 246
		[DefaultValue(false)]
		public bool UserInstallable;

		// Token: 0x040000F7 RID: 247
		[DefaultValue(PublishingPackageInfo.SatelliteTypes.Base)]
		public PublishingPackageInfo.SatelliteTypes SatelliteType;

		// Token: 0x040000F8 RID: 248
		[DefaultValue(null)]
		public string SatelliteValue;

		// Token: 0x040000F9 RID: 249
		[DefaultValue(PublishingPackageInfo.UpdateTypes.Canonical)]
		public PublishingPackageInfo.UpdateTypes UpdateType = PublishingPackageInfo.UpdateTypes.Canonical;

		// Token: 0x040000FA RID: 250
		public string SourceFMFile;

		// Token: 0x02000020 RID: 32
		public enum PublishingPackageInfoComparison
		{
			// Token: 0x040000FC RID: 252
			IgnorePaths,
			// Token: 0x040000FD RID: 253
			OnlyUniqueID,
			// Token: 0x040000FE RID: 254
			OnlyUniqueIDAndFeatureID
		}

		// Token: 0x02000021 RID: 33
		public enum UpdateTypes
		{
			// Token: 0x04000100 RID: 256
			PKR,
			// Token: 0x04000101 RID: 257
			Diff,
			// Token: 0x04000102 RID: 258
			Canonical
		}

		// Token: 0x02000022 RID: 34
		public enum SatelliteTypes
		{
			// Token: 0x04000104 RID: 260
			Base,
			// Token: 0x04000105 RID: 261
			Language,
			// Token: 0x04000106 RID: 262
			Resolution
		}
	}
}
