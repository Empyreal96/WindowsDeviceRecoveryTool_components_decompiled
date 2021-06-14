using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using Microsoft.WindowsPhone.FeatureAPI;
using Microsoft.WindowsPhone.ImageUpdate.PkgCommon;
using Microsoft.WindowsPhone.ImageUpdate.Tools.Common;

namespace Microsoft.WindowsPhone.Imaging
{
	// Token: 0x0200001E RID: 30
	[XmlType(Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
	[XmlRoot(ElementName = "PublishingPackageInfo", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate", IsNullable = false)]
	public class PublishingPackageList
	{
		// Token: 0x06000161 RID: 353 RVA: 0x000074AC File Offset: 0x000056AC
		public PublishingPackageList()
		{
			if (this.Packages == null)
			{
				this.Packages = new List<PublishingPackageInfo>();
			}
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00007674 File Offset: 0x00005874
		public PublishingPackageList(string sourceListPath, string destListPath, IULogger logger)
		{
			PublishingPackageList publishingPackageList = new PublishingPackageList();
			PublishingPackageList publishingPackageList2 = new PublishingPackageList();
			PublishingPackageList.ValidateAndLoad(ref publishingPackageList, sourceListPath, logger);
			PublishingPackageList.ValidateAndLoad(ref publishingPackageList2, destListPath, logger);
			List<PublishingPackageInfo> list = publishingPackageList.Packages;
			List<PublishingPackageInfo> list2 = publishingPackageList2.Packages;
			this.IsUpdateList = true;
			this.FeatureIdentifierPackages = publishingPackageList.FeatureIdentifierPackages;
			this.IsTargetFeatureEnabled = publishingPackageList.IsTargetFeatureEnabled;
			this.Packages = new List<PublishingPackageInfo>();
			IEnumerable<PublishingPackageInfo> enumerable = from srcPkg in list
			join destPkg in list2 on srcPkg.ID equals destPkg.ID
			where !destPkg.Path.Equals(srcPkg.Path, StringComparison.OrdinalIgnoreCase) && destPkg.Equals(srcPkg, PublishingPackageInfo.PublishingPackageInfoComparison.IgnorePaths)
			select destPkg.SetPreviousPath(srcPkg.Path);
			enumerable = (from pkg in enumerable
			select pkg.SetUpdateType(PublishingPackageInfo.UpdateTypes.Diff)).ToList<PublishingPackageInfo>();
			this.Packages.AddRange(enumerable);
			list = list.Except(enumerable, PublishingPackageInfoComparer.IgnorePaths).ToList<PublishingPackageInfo>();
			list2 = list2.Except(enumerable, PublishingPackageInfoComparer.IgnorePaths).ToList<PublishingPackageInfo>();
			this.Packages.AddRange(from pkg in list2.Intersect(list, PublishingPackageInfoComparer.IgnorePaths)
			select pkg.SetUpdateType(PublishingPackageInfo.UpdateTypes.Diff));
			this.Packages.AddRange(from pkg in list2.Except(list, PublishingPackageInfoComparer.IgnorePaths)
			select pkg.SetUpdateType(PublishingPackageInfo.UpdateTypes.Canonical));
			this.Packages.AddRange(from pkg in list.Except(list2, PublishingPackageInfoComparer.IgnorePaths)
			select pkg.SetUpdateType(PublishingPackageInfo.UpdateTypes.PKR));
			if (publishingPackageList.IsTargetFeatureEnabled || publishingPackageList.MSFeatureGroups.Count<FMFeatureGrouping>() > 0 || publishingPackageList.OEMFeatureGroups.Count<FMFeatureGrouping>() > 0)
			{
				this.MSFeatureGroups = publishingPackageList.MSFeatureGroups;
				this.OEMFeatureGroups = publishingPackageList.OEMFeatureGroups;
				return;
			}
			this.MSFeatureGroups = publishingPackageList2.MSFeatureGroups;
			this.OEMFeatureGroups = publishingPackageList2.OEMFeatureGroups;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x000078F4 File Offset: 0x00005AF4
		public Dictionary<string, string> GetFeatureIDWithFMIDPackages(OwnerType forOwner = 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			List<string> featureIDWithFMIDs = this.GetFeatureIDWithFMIDs(forOwner);
			foreach (string text in featureIDWithFMIDs)
			{
				List<PublishingPackageInfo> allPackagesForFeatureIDWithFMID = this.GetAllPackagesForFeatureIDWithFMID(text, forOwner);
				List<PublishingPackageInfo> list = (from pkg in allPackagesForFeatureIDWithFMID
				where pkg.IsFeatureIdentifierPackage
				select pkg).ToList<PublishingPackageInfo>();
				if (list.Count<PublishingPackageInfo>() > 1)
				{
					flag = true;
					stringBuilder.Append(Environment.NewLine + text + " : ");
					using (List<PublishingPackageInfo>.Enumerator enumerator2 = list.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							PublishingPackageInfo publishingPackageInfo = enumerator2.Current;
							stringBuilder.Append(Environment.NewLine + "\t" + publishingPackageInfo.ID);
						}
						continue;
					}
				}
				string value = "";
				if (list.Count<PublishingPackageInfo>() == 1)
				{
					value = list.ElementAt(0).ID;
				}
				dictionary.Add(text, value);
			}
			if (flag)
			{
				throw new AmbiguousArgumentException("Some features have more than one FeatureIdentifierPackage defined: " + stringBuilder.ToString());
			}
			return dictionary;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00007A88 File Offset: 0x00005C88
		public List<string> GetFeatureIDs(OwnerType forOwner = 0)
		{
			IEnumerable<PublishingPackageInfo> source = from pkg in this.Packages
			where pkg.OwnerType == 1 || pkg.OwnerType == 2
			select pkg;
			if (forOwner != null)
			{
				source = from pkg in this.Packages
				where pkg.OwnerType == forOwner
				select pkg;
			}
			return (from pkg in source
			select pkg.FeatureID).Distinct(this.IgnoreCase).ToList<string>();
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00007B5C File Offset: 0x00005D5C
		public List<string> GetFeatureIDWithFMIDs(OwnerType forOwner = 0)
		{
			IEnumerable<PublishingPackageInfo> source = from pkg in this.Packages
			where pkg.OwnerType == 1 || pkg.OwnerType == 2
			select pkg;
			if (forOwner != null)
			{
				source = from pkg in this.Packages
				where pkg.OwnerType == forOwner
				select pkg;
			}
			return (from pkg in source
			select pkg.FeatureIDWithFMID).Distinct(this.IgnoreCase).ToList<string>();
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00007BFC File Offset: 0x00005DFC
		public List<PublishingPackageInfo> GetAllPackagesForFeature(string FeatureID, OwnerType forOwner = 0)
		{
			return this.GetAllPackagesForFeatures(new List<string>
			{
				FeatureID
			}, forOwner);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00007C4C File Offset: 0x00005E4C
		public List<PublishingPackageInfo> GetAllPackagesForFeatureIDWithFMID(string FeatureIDWithFMID, OwnerType forOwner = 0)
		{
			List<PublishingPackageInfo> list = (from pkg in this.Packages
			where FeatureIDWithFMID.Equals(pkg.FeatureIDWithFMID, StringComparison.OrdinalIgnoreCase)
			select pkg).ToList<PublishingPackageInfo>();
			if (forOwner != null)
			{
				list = (from pkg in list
				where pkg.OwnerType == forOwner
				select pkg).ToList<PublishingPackageInfo>();
			}
			return list;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00007D28 File Offset: 0x00005F28
		private List<PublishingPackageInfo> GetAllPackagesForFeaturesAndFMs(List<string> FeatureIDs, List<string> fmFilter, OwnerType forOwner = 0)
		{
			List<PublishingPackageInfo> allPackagesForFeatures = this.GetAllPackagesForFeatures(FeatureIDs, forOwner);
			List<string> newFMFilter = (from fm in fmFilter
			select fm.ToUpper().Replace("SKU", "FM")).ToList<string>();
			List<string> newSKUFilter = (from fm in fmFilter
			select fm.ToUpper().Replace("FM", "SKU")).ToList<string>();
			IEnumerable<PublishingPackageInfo> source = from pkg in allPackagesForFeatures
			where newFMFilter.Contains(pkg.SourceFMFile, this.IgnoreCase) || newSKUFilter.Contains(pkg.SourceFMFile, this.IgnoreCase)
			select pkg;
			return source.ToList<PublishingPackageInfo>();
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00007E14 File Offset: 0x00006014
		public List<PublishingPackageInfo> GetAllPackagesForFeatures(List<string> FeatureIDs, OwnerType forOwner = 0)
		{
			IEnumerable<PublishingPackageInfo> source = from pkg in this.Packages
			where pkg.OwnerType == forOwner && FeatureIDs.Contains(pkg.FeatureID, this.IgnoreCase)
			select pkg;
			if (forOwner == null)
			{
				source = from pkg in this.Packages
				where FeatureIDs.Contains(pkg.FeatureID, this.IgnoreCase)
				select pkg;
			}
			return source.ToList<PublishingPackageInfo>();
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00007E84 File Offset: 0x00006084
		public List<PublishingPackageInfo> GetUpdatePackageList(OEMInput orgOemInput, OEMInput newOemInput, OEMInput.OEMFeatureTypes featureFilter, OwnerType forOwner = 0)
		{
			List<string> featureList = orgOemInput.GetFeatureList(featureFilter);
			List<string> featureList2 = newOemInput.GetFeatureList(featureFilter);
			List<string> fms = orgOemInput.GetFMs();
			List<string> fms2 = newOemInput.GetFMs();
			return this.GetUpdatePackageList(featureList, featureList2, orgOemInput.SupportedLanguages.UserInterface, newOemInput.SupportedLanguages.UserInterface, newOemInput.Resolutions, fms, fms2, forOwner, false);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x000083BC File Offset: 0x000065BC
		private List<PublishingPackageInfo> GetUpdatePackageList(List<string> orgFeatures, List<string> newFeatures, List<string> orgLangs, List<string> newLangs, List<string> resolutions, List<string> orgFMs, List<string> newFMs, OwnerType forOwner = 0, bool DoOnlyChanges = false)
		{
			List<PublishingPackageInfo> list = new List<PublishingPackageInfo>();
			if (forOwner != 1 && forOwner != 2 && forOwner != null)
			{
				return list;
			}
			List<string> addFeatures = newFeatures.Except(orgFeatures, this.IgnoreCase).ToList<string>();
			List<string> removeFeatures = orgFeatures.Except(newFeatures, this.IgnoreCase).ToList<string>();
			List<string> commonFeatures = newFeatures.Intersect(orgFeatures, this.IgnoreCase).ToList<string>();
			List<string> addLangs = newLangs.Except(orgLangs, this.IgnoreCase).ToList<string>();
			List<string> removeLangs = orgLangs.Except(newLangs, this.IgnoreCase).ToList<string>();
			List<string> commonLangs = newLangs.Intersect(orgLangs, this.IgnoreCase).ToList<string>();
			List<string> orgFMsNormalized = (from fm in orgFMs
			select this.NormalizeFM(fm)).ToList<string>();
			List<string> addFMs = newFMs.Except(orgFMsNormalized, this.IgnoreCase).ToList<string>();
			List<string> removeFMs = orgFMsNormalized.Except(newFMs, this.IgnoreCase).ToList<string>();
			List<string> commonFMs = newFMs.Intersect(orgFMsNormalized, this.IgnoreCase).ToList<string>();
			List<PublishingPackageInfo> list2 = (from pkg in this.Packages
			where addFeatures.Contains(pkg.FeatureID, this.IgnoreCase) && pkg.UpdateType != PublishingPackageInfo.UpdateTypes.PKR && newFMs.Contains(this.NormalizeFM(pkg.SourceFMFile), this.IgnoreCase) && (pkg.SatelliteType == PublishingPackageInfo.SatelliteTypes.Base || (pkg.SatelliteType == PublishingPackageInfo.SatelliteTypes.Resolution && resolutions.Contains(pkg.SatelliteValue, this.IgnoreCase)) || (pkg.SatelliteType == PublishingPackageInfo.SatelliteTypes.Language && newLangs.Contains(pkg.SatelliteValue, this.IgnoreCase)))
			select pkg).ToList<PublishingPackageInfo>();
			List<PublishingPackageInfo> list3 = (from pkg in this.Packages
			where removeFeatures.Contains(pkg.FeatureID, this.IgnoreCase) && orgFMsNormalized.Contains(this.NormalizeFM(pkg.SourceFMFile), this.IgnoreCase) && (pkg.SatelliteType == PublishingPackageInfo.SatelliteTypes.Base || (pkg.SatelliteType == PublishingPackageInfo.SatelliteTypes.Resolution && resolutions.Contains(pkg.SatelliteValue, this.IgnoreCase)) || (pkg.SatelliteType == PublishingPackageInfo.SatelliteTypes.Language && orgLangs.Contains(pkg.SatelliteValue, this.IgnoreCase)))
			select pkg).ToList<PublishingPackageInfo>();
			list2.AddRange((from pkg in this.Packages
			where commonFeatures.Contains(pkg.FeatureID, this.IgnoreCase) && addFMs.Contains(this.NormalizeFM(pkg.SourceFMFile), this.IgnoreCase) && pkg.UpdateType != PublishingPackageInfo.UpdateTypes.PKR && (pkg.SatelliteType == PublishingPackageInfo.SatelliteTypes.Base || (pkg.SatelliteType == PublishingPackageInfo.SatelliteTypes.Resolution && resolutions.Contains(pkg.SatelliteValue, this.IgnoreCase)) || (pkg.SatelliteType == PublishingPackageInfo.SatelliteTypes.Language && newLangs.Contains(pkg.SatelliteValue, this.IgnoreCase)))
			select pkg).ToList<PublishingPackageInfo>());
			list3.AddRange((from pkg in this.Packages
			where commonFeatures.Contains(pkg.FeatureID, this.IgnoreCase) && removeFMs.Contains(this.NormalizeFM(pkg.SourceFMFile), this.IgnoreCase) && (!this.IsUpdateList || pkg.UpdateType != PublishingPackageInfo.UpdateTypes.Canonical) && (pkg.SatelliteType == PublishingPackageInfo.SatelliteTypes.Base || (pkg.SatelliteType == PublishingPackageInfo.SatelliteTypes.Resolution && resolutions.Contains(pkg.SatelliteValue, this.IgnoreCase)) || (pkg.SatelliteType == PublishingPackageInfo.SatelliteTypes.Language && orgLangs.Contains(pkg.SatelliteValue, this.IgnoreCase)))
			select pkg).ToList<PublishingPackageInfo>());
			list2.AddRange((from pkg in this.Packages
			where commonFeatures.Contains(pkg.FeatureID, this.IgnoreCase) && newFMs.Contains(this.NormalizeFM(pkg.SourceFMFile), this.IgnoreCase) && pkg.UpdateType != PublishingPackageInfo.UpdateTypes.PKR && pkg.SatelliteType == PublishingPackageInfo.SatelliteTypes.Language && addLangs.Contains(pkg.SatelliteValue, this.IgnoreCase)
			select pkg).ToList<PublishingPackageInfo>());
			list3.AddRange((from pkg in this.Packages
			where commonFeatures.Contains(pkg.FeatureID, this.IgnoreCase) && orgFMsNormalized.Contains(this.NormalizeFM(pkg.SourceFMFile), this.IgnoreCase) && pkg.SatelliteType == PublishingPackageInfo.SatelliteTypes.Language && removeLangs.Contains(pkg.SatelliteValue, this.IgnoreCase)
			select pkg).ToList<PublishingPackageInfo>());
			if (this.IsUpdateList && !DoOnlyChanges)
			{
				list.AddRange((from pkg in this.Packages
				where commonFeatures.Contains(pkg.FeatureID, this.IgnoreCase) && commonFMs.Contains(this.NormalizeFM(pkg.SourceFMFile), this.IgnoreCase) && (pkg.SatelliteType == PublishingPackageInfo.SatelliteTypes.Base || (pkg.SatelliteType == PublishingPackageInfo.SatelliteTypes.Resolution && resolutions.Contains(pkg.SatelliteValue, this.IgnoreCase)) || (pkg.SatelliteType == PublishingPackageInfo.SatelliteTypes.Language && commonLangs.Contains(pkg.SatelliteValue, this.IgnoreCase)))
				select pkg).ToList<PublishingPackageInfo>());
				list3 = list3.Except(from pkg in list3
				where pkg.UpdateType == PublishingPackageInfo.UpdateTypes.Canonical && pkg.OwnerType != 1
				select pkg).ToList<PublishingPackageInfo>();
			}
			list2 = this.ChangeToCanonicals(list2);
			list3 = this.ChangeToPKRs(list3);
			list.AddRange(list2);
			list.AddRange(list3);
			if (forOwner != null)
			{
				list = (from pkg in list
				where pkg.OwnerType == forOwner
				select pkg).ToList<PublishingPackageInfo>();
			}
			return this.RemoveDuplicatesPkgs(list);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x000086F2 File Offset: 0x000068F2
		private string NormalizeFM(string fm)
		{
			return fm.ToUpper().Replace("SKU", "FM");
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00008714 File Offset: 0x00006914
		public UpdateOSInput GetUpdateInput(string build1OEMInput, string build2OEMInput, string msPackageRoot, CpuId cpuType, IULogger logger)
		{
			UpdateOSInput updateOSInput = new UpdateOSInput();
			List<string> list = new List<string>();
			char[] trimChars = new char[]
			{
				'\\'
			};
			msPackageRoot = msPackageRoot.Trim(trimChars);
			OEMInput orgOemInput = new OEMInput();
			OEMInput oeminput = new OEMInput();
			OEMInput.ValidateInput(ref orgOemInput, build1OEMInput, logger, msPackageRoot, cpuType.ToString());
			OEMInput.ValidateInput(ref oeminput, build2OEMInput, logger, msPackageRoot, cpuType.ToString());
			List<PublishingPackageInfo> updatePackageList = this.GetUpdatePackageList(orgOemInput, oeminput, 268435455, 0);
			list = (from pkg in updatePackageList
			select pkg.Path).ToList<string>();
			list = list.Distinct(this.IgnoreCase).ToList<string>();
			for (int i = 0; i < list.Count; i++)
			{
				list[i] = Path.Combine(msPackageRoot, list[i]);
			}
			updateOSInput.PackageFiles = list;
			updateOSInput.Description = "(Updating to)" + oeminput.Description;
			return updateOSInput;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000881C File Offset: 0x00006A1C
		public List<PublishingPackageInfo> GetPackageListForPOP(OEMInput oemInput1, OEMInput oemInput2)
		{
			List<PublishingPackageInfo> list = new List<PublishingPackageInfo>();
			list.AddRange(this.GetMSPackageListForPOP(oemInput1, oemInput2));
			list.AddRange(this.GetOEMPackageListForPOP(oemInput1, oemInput2));
			return list;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000885C File Offset: 0x00006A5C
		private List<string> GetDepricatedFeatures(List<string> featureIDs)
		{
			List<string> list = new List<string>();
			if (!this.IsUpdateList)
			{
				return list;
			}
			foreach (string text in featureIDs)
			{
				if ((from pkg in this.GetAllPackagesForFeature(text, 0)
				where pkg.UpdateType != PublishingPackageInfo.UpdateTypes.PKR
				select pkg).Count<PublishingPackageInfo>() == 0)
				{
					list.Add(text);
				}
			}
			return list;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00008924 File Offset: 0x00006B24
		private List<string> GetDepricatedLangs(List<string> Langs)
		{
			List<string> list = new List<string>();
			if (!this.IsUpdateList)
			{
				return list;
			}
			using (List<string>.Enumerator enumerator = Langs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string lang = enumerator.Current;
					if ((from pkg in this.Packages
					where pkg.SatelliteType == PublishingPackageInfo.SatelliteTypes.Language && pkg.SatelliteValue.Equals(lang, StringComparison.OrdinalIgnoreCase) && pkg.UpdateType != PublishingPackageInfo.UpdateTypes.PKR
					select pkg).Count<PublishingPackageInfo>() == 0)
					{
						list.Add(lang);
					}
				}
			}
			return list;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x000089C4 File Offset: 0x00006BC4
		public List<PublishingPackageInfo> GetMSPackageListForPOP(OEMInput orgOemInput, OEMInput newOemInput)
		{
			List<string> list = orgOemInput.GetFeatureList(268433407);
			List<string> featureList = newOemInput.GetFeatureList(268433407);
			List<string> fms = orgOemInput.GetFMs();
			List<string> fms2 = newOemInput.GetFMs();
			List<string> list2 = orgOemInput.SupportedLanguages.UserInterface;
			List<string> userInterface = newOemInput.SupportedLanguages.UserInterface;
			list = list.Except(this.GetDepricatedFeatures(list)).ToList<string>();
			list2 = list2.Except(this.GetDepricatedLangs(list2)).ToList<string>();
			bool doOnlyChanges = true;
			List<PublishingPackageInfo> list3 = this.GetUpdatePackageList(list, featureList, list2, userInterface, newOemInput.Resolutions, fms, fms2, 1, doOnlyChanges);
			if (this.IsUpdateList)
			{
				list3 = list3.Except(this.GetSourceOnlyPkgs((from pkg in list3
				where pkg.UpdateType == PublishingPackageInfo.UpdateTypes.PKR
				select pkg).ToList<PublishingPackageInfo>())).ToList<PublishingPackageInfo>();
			}
			return list3;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00008AE4 File Offset: 0x00006CE4
		private List<PublishingPackageInfo> GetSourceOnlyPkgs(List<PublishingPackageInfo> list)
		{
			List<string> second = (from pkg in this.Packages
			where pkg.UpdateType == PublishingPackageInfo.UpdateTypes.Diff || pkg.UpdateType == PublishingPackageInfo.UpdateTypes.Canonical
			select pkg.ID).Distinct<string>().ToList<string>();
			List<string> first = (from pkg in this.Packages
			select pkg.ID).Distinct(StringComparer.OrdinalIgnoreCase).ToList<string>();
			List<string> inSourceOnlyPackageIDs = first.Except(second, this.IgnoreCase).Distinct(this.IgnoreCase).ToList<string>();
			return (from pkg in list
			where inSourceOnlyPackageIDs.Contains(pkg.ID, StringComparer.OrdinalIgnoreCase)
			select pkg).ToList<PublishingPackageInfo>();
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00008C3C File Offset: 0x00006E3C
		private List<PublishingPackageInfo> RemoveDuplicatesPkgs(List<PublishingPackageInfo> pkgList)
		{
			List<PublishingPackageInfo> list = new List<PublishingPackageInfo>();
			List<string> excludePackages = new List<string>();
			List<PublishingPackageInfo> source = (from pkg in pkgList
			where pkg.UpdateType == PublishingPackageInfo.UpdateTypes.PKR
			select pkg).ToList<PublishingPackageInfo>();
			List<PublishingPackageInfo> source2 = (from pkg in pkgList
			where pkg.UpdateType == PublishingPackageInfo.UpdateTypes.Canonical
			select pkg).ToList<PublishingPackageInfo>();
			List<PublishingPackageInfo> collection = (from pkg in pkgList
			where pkg.UpdateType == PublishingPackageInfo.UpdateTypes.Diff
			select pkg).ToList<PublishingPackageInfo>();
			excludePackages = (from pkgID in source
			select pkgID.ID).Intersect(from dupPkg in source2
			select dupPkg.ID, this.IgnoreCase).ToList<string>();
			list.AddRange((from pkg in source
			where !excludePackages.Contains(pkg.ID, this.IgnoreCase)
			select pkg).ToList<PublishingPackageInfo>());
			list.AddRange((from pkg in source2
			where !excludePackages.Contains(pkg.ID, this.IgnoreCase)
			select pkg).ToList<PublishingPackageInfo>());
			list.AddRange(collection);
			return list.Distinct<PublishingPackageInfo>().ToList<PublishingPackageInfo>();
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00008D90 File Offset: 0x00006F90
		private List<PublishingPackageInfo> ChangeToCanonicals(List<PublishingPackageInfo> pkgs)
		{
			List<PublishingPackageInfo> list = new List<PublishingPackageInfo>();
			foreach (PublishingPackageInfo pkg in pkgs)
			{
				PublishingPackageInfo item = this.ChangeToCanonical(pkg);
				list.Add(item);
			}
			return list.Distinct<PublishingPackageInfo>().ToList<PublishingPackageInfo>();
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00008DF8 File Offset: 0x00006FF8
		private PublishingPackageInfo ChangeToCanonical(PublishingPackageInfo pkg)
		{
			PublishingPackageInfo publishingPackageInfo = new PublishingPackageInfo(pkg);
			if (publishingPackageInfo.UpdateType != PublishingPackageInfo.UpdateTypes.Canonical)
			{
				publishingPackageInfo.UpdateType = PublishingPackageInfo.UpdateTypes.Canonical;
			}
			return publishingPackageInfo;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00008E20 File Offset: 0x00007020
		private List<PublishingPackageInfo> ChangeToPKRs(List<PublishingPackageInfo> pkgs)
		{
			List<PublishingPackageInfo> list = new List<PublishingPackageInfo>();
			foreach (PublishingPackageInfo pkg in pkgs)
			{
				PublishingPackageInfo item = this.ChangeToPKR(pkg);
				list.Add(item);
			}
			return list.Distinct<PublishingPackageInfo>().ToList<PublishingPackageInfo>();
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00008E88 File Offset: 0x00007088
		private PublishingPackageInfo ChangeToPKR(PublishingPackageInfo pkg)
		{
			PublishingPackageInfo publishingPackageInfo = new PublishingPackageInfo(pkg);
			if (publishingPackageInfo.UpdateType != PublishingPackageInfo.UpdateTypes.PKR)
			{
				string text = publishingPackageInfo.Path.ToLower();
				text = text.Replace(PkgConstants.c_strPackageExtension, PkgConstants.c_strRemovalPkgExtension);
				text = text.Replace(PkgConstants.c_strDiffPackageExtension, PkgConstants.c_strRemovalPkgExtension);
				text = text.Replace(".spkg.cab", ".spkr.cab");
				text = text.ToLower().Replace(".spku.cab", ".spkr.cab");
				publishingPackageInfo.Path = text;
				publishingPackageInfo.UpdateType = PublishingPackageInfo.UpdateTypes.PKR;
			}
			return publishingPackageInfo;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00008F14 File Offset: 0x00007114
		public List<PublishingPackageInfo> GetOEMPackageListForPOP(OEMInput oemInput1, OEMInput oemInput2)
		{
			List<PublishingPackageInfo> list = this.GetUpdatePackageList(oemInput1, oemInput2, 268434431, 2);
			if (this.IsUpdateList)
			{
				list = list.Except(this.GetTargetOnlyPkgs((from pkg in list
				where pkg.UpdateType == PublishingPackageInfo.UpdateTypes.PKR
				select pkg).ToList<PublishingPackageInfo>())).ToList<PublishingPackageInfo>();
			}
			return list;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00008FB4 File Offset: 0x000071B4
		private List<PublishingPackageInfo> GetTargetOnlyPkgs(List<PublishingPackageInfo> list)
		{
			List<string> sourcePackageIDs = (from pkg in this.Packages
			where pkg.UpdateType == PublishingPackageInfo.UpdateTypes.Diff || pkg.UpdateType == PublishingPackageInfo.UpdateTypes.PKR
			select pkg.ID).Distinct<string>().ToList<string>();
			return (from pkg in list
			where !sourcePackageIDs.Contains(pkg.ID, StringComparer.OrdinalIgnoreCase)
			select pkg).ToList<PublishingPackageInfo>();
		}

		// Token: 0x0600017A RID: 378 RVA: 0x000090A0 File Offset: 0x000072A0
		public List<PublishingPackageInfo> GetAllPackagesForImage(OEMInput oemInput, OwnerType forOwnerType = 0)
		{
			List<PublishingPackageInfo> list = new List<PublishingPackageInfo>();
			OEMInput.OEMFeatureTypes oemfeatureTypes = 268435455;
			if (forOwnerType == 1)
			{
				oemfeatureTypes = 268433407;
			}
			else if (forOwnerType == 2)
			{
				oemfeatureTypes = 268434431;
			}
			else if (forOwnerType != null)
			{
				return list;
			}
			List<string> featureList = oemInput.GetFeatureList(oemfeatureTypes);
			List<string> fms = oemInput.GetFMs();
			List<string> langs = oemInput.SupportedLanguages.UserInterface;
			List<string> res = oemInput.Resolutions;
			List<PublishingPackageInfo> allPackagesForFeaturesAndFMs = this.GetAllPackagesForFeaturesAndFMs(featureList, fms, forOwnerType);
			list.AddRange((from pkg in allPackagesForFeaturesAndFMs
			where pkg.SatelliteType == PublishingPackageInfo.SatelliteTypes.Base
			select pkg).ToList<PublishingPackageInfo>());
			list.AddRange((from pkg in allPackagesForFeaturesAndFMs
			where pkg.SatelliteType == PublishingPackageInfo.SatelliteTypes.Language && langs.Contains(pkg.SatelliteValue, this.IgnoreCase)
			select pkg).ToList<PublishingPackageInfo>());
			list.AddRange((from pkg in allPackagesForFeaturesAndFMs
			where pkg.SatelliteType == PublishingPackageInfo.SatelliteTypes.Resolution && res.Contains(pkg.SatelliteValue, this.IgnoreCase)
			select pkg).ToList<PublishingPackageInfo>());
			return list.Distinct<PublishingPackageInfo>().ToList<PublishingPackageInfo>();
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00009200 File Offset: 0x00007400
		public void ValidateConstraints()
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			IEnumerable<IGrouping<string, PublishingPackageInfo>> enumerable = from pkg in this.Packages
			where pkg.OwnerType == 1
			group pkg by pkg.ID;
			foreach (IGrouping<string, PublishingPackageInfo> source in enumerable)
			{
				if (source.Count<PublishingPackageInfo>() > 1)
				{
					IEnumerable<string> enumerable2 = (from pkg in source
					select pkg.FeatureID).Distinct(this.IgnoreCase);
					if (source.Count<PublishingPackageInfo>() != enumerable2.Count<string>())
					{
						IEnumerable<IGrouping<string, PublishingPackageInfo>> source2 = from pkg in source
						group pkg by pkg.FeatureID;
						foreach (IGrouping<string, PublishingPackageInfo> source3 in from gp in source2
						where gp.Count<PublishingPackageInfo>() > 1
						select gp)
						{
							stringBuilder.AppendLine(string.Concat(new object[]
							{
								"\t",
								source3.First<PublishingPackageInfo>().FeatureID,
								": (",
								source3.First<PublishingPackageInfo>().ID,
								" Count=",
								source3.Count<PublishingPackageInfo>(),
								")\n"
							}));
						}
					}
					if (this.IsTargetFeatureEnabled)
					{
						List<List<string>> list = (from fGroup in this.OEMFeatureGroups
						where fGroup.Constraint == 3 || fGroup.Constraint == 2
						select fGroup.AllFeatureIDs).ToList<List<string>>();
						list.AddRange(from fGroup in this.MSFeatureGroups
						where fGroup.Constraint == 3 || fGroup.Constraint == 2
						select fGroup.AllFeatureIDs);
						list.AddRange(this.GetImplicitConstraints());
						bool flag = false;
						foreach (List<string> second in list)
						{
							if (enumerable2.Except(second).Count<string>() == 0)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							stringBuilder2.AppendLine(string.Concat(new string[]
							{
								"\t",
								source.First<PublishingPackageInfo>().ID,
								": (",
								string.Join(", ", enumerable2.ToArray<string>()),
								")\n"
							}));
						}
					}
				}
			}
			StringBuilder stringBuilder3 = new StringBuilder();
			if (stringBuilder.Length != 0)
			{
				stringBuilder3.AppendLine("The following Features have packages listed more than once:\n" + stringBuilder.ToString());
			}
			if (stringBuilder2.Length != 0)
			{
				stringBuilder3.AppendLine("The following package is included in multiple features without constraints preventing them from being included in the same image:\n" + stringBuilder2.ToString());
			}
			if (stringBuilder3.Length != 0)
			{
				throw new ImageCommonException(stringBuilder3.ToString());
			}
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00009614 File Offset: 0x00007814
		public List<List<string>> GetImplicitConstraints()
		{
			List<List<string>> list = new List<List<string>>();
			IEnumerable<FeatureManifest.PackageGroups> enumerable = from pkg in this.Packages
			where pkg.FMGroup != 9 && pkg.FMGroup != 10 && pkg.FMGroup != 0
			select pkg.FMGroup;
			using (IEnumerator<FeatureManifest.PackageGroups> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FeatureManifest.PackageGroups fmGroup = enumerator.Current;
					list.Add((from pkg in this.Packages
					where pkg.FMGroup == fmGroup
					select pkg.FeatureID.ToString()).Distinct(this.IgnoreCase).ToList<string>());
				}
			}
			return list;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00009710 File Offset: 0x00007910
		public static void ValidateAndLoad(ref PublishingPackageList xmlInput, string xmlFile, IULogger logger)
		{
			string text = string.Empty;
			string publishingPackageInfoSchema = BuildPaths.PublishingPackageInfoSchema;
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			foreach (string text2 in manifestResourceNames)
			{
				if (text2.Contains(publishingPackageInfoSchema))
				{
					text = text2;
					break;
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				throw new ImageCommonException("ImageCommon!ValidateAndLoad: XSD resource was not found: " + publishingPackageInfoSchema);
			}
			using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(text))
			{
				XsdValidator xsdValidator = new XsdValidator();
				try
				{
					xsdValidator.ValidateXsd(manifestResourceStream, xmlFile, logger);
				}
				catch (XsdValidatorException innerException)
				{
					throw new ImageCommonException("ImageCommon!ValidateAndLoad: Unable to validate Publishing Package Info XSD for file '" + xmlFile + "'.", innerException);
				}
			}
			logger.LogInfo("ImageCommon: Successfully validated the Publishing Package Info XML: {0}", new object[]
			{
				xmlFile
			});
			TextReader textReader = new StreamReader(LongPathFile.OpenRead(xmlFile));
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(PublishingPackageList));
			try
			{
				xmlInput = (PublishingPackageList)xmlSerializer.Deserialize(textReader);
			}
			catch (Exception innerException2)
			{
				throw new ImageCommonException("ImageCommon!ValidateAndLoad: Unable to parse Publishing Package Info XML file '" + xmlFile + "'", innerException2);
			}
			finally
			{
				textReader.Close();
			}
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00009864 File Offset: 0x00007A64
		public void WriteToFile(string xmlFile)
		{
			TextWriter textWriter = new StreamWriter(LongPathFile.OpenWrite(xmlFile));
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(PublishingPackageList));
			try
			{
				xmlSerializer.Serialize(textWriter, this);
			}
			catch (Exception innerException)
			{
				throw new ImageCommonException("ImageCommon!WriteToFile: Unable to write Publishing Package List XML file '" + xmlFile + "'", innerException);
			}
			finally
			{
				textWriter.Close();
			}
		}

		// Token: 0x040000B5 RID: 181
		private const string DiffPackageExtension = ".spku.cab";

		// Token: 0x040000B6 RID: 182
		private const string CanonicalPackageExtension = ".spkg.cab";

		// Token: 0x040000B7 RID: 183
		private const string RemovePackageExtension = ".spkr.cab";

		// Token: 0x040000B8 RID: 184
		[DefaultValue(false)]
		public bool IsUpdateList;

		// Token: 0x040000B9 RID: 185
		[DefaultValue(false)]
		public bool IsTargetFeatureEnabled;

		// Token: 0x040000BA RID: 186
		[XmlArray]
		[XmlArrayItem(ElementName = "FeatureGroup", Type = typeof(FMFeatureGrouping), IsNullable = false)]
		public List<FMFeatureGrouping> MSFeatureGroups;

		// Token: 0x040000BB RID: 187
		[XmlArray]
		[XmlArrayItem(ElementName = "FeatureGroup", Type = typeof(FMFeatureGrouping), IsNullable = false)]
		public List<FMFeatureGrouping> OEMFeatureGroups;

		// Token: 0x040000BC RID: 188
		[XmlArray]
		[XmlArrayItem(ElementName = "PackageInfo", Type = typeof(PublishingPackageInfo), IsNullable = false)]
		public List<PublishingPackageInfo> Packages;

		// Token: 0x040000BD RID: 189
		[XmlArray]
		[XmlArrayItem(ElementName = "FeatureIdentifierPackage", Type = typeof(FeatureIdentifierPackage), IsNullable = false)]
		public List<FeatureIdentifierPackage> FeatureIdentifierPackages;

		// Token: 0x040000BE RID: 190
		[XmlIgnore]
		private StringComparer IgnoreCase = StringComparer.OrdinalIgnoreCase;
	}
}
