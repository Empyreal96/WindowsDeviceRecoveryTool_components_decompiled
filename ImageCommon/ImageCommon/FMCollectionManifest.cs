using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using Microsoft.WindowsPhone.ImageUpdate.Tools.Common;

namespace Microsoft.WindowsPhone.Imaging
{
	// Token: 0x02000026 RID: 38
	[XmlType(Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
	[XmlRoot(ElementName = "FMCollectionManifest", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate", IsNullable = false)]
	public class FMCollectionManifest
	{
		// Token: 0x060001CE RID: 462 RVA: 0x0000AFCC File Offset: 0x000091CC
		public static void ValidateAndLoad(ref FMCollectionManifest xmlInput, string xmlFile, IULogger logger)
		{
			string text = string.Empty;
			string fmcollectionSchema = BuildPaths.FMCollectionSchema;
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			foreach (string text2 in manifestResourceNames)
			{
				if (text2.Contains(fmcollectionSchema))
				{
					text = text2;
					break;
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				throw new ImageCommonException("ImageCommon!ValidateAndLoad: XSD resource was not found: " + fmcollectionSchema);
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
					throw new ImageCommonException("ImageCommon!ValidateAndLoad: Unable to validate FM Collection Manifest XSD for file '" + xmlFile + "'.", innerException);
				}
			}
			logger.LogInfo("ImageCommon: Successfully validated the Feature Manifest XML: {0}", new object[]
			{
				xmlFile
			});
			TextReader textReader = new StreamReader(LongPathFile.OpenRead(xmlFile));
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(FMCollectionManifest));
			try
			{
				xmlInput = (FMCollectionManifest)xmlSerializer.Deserialize(textReader);
			}
			catch (Exception innerException2)
			{
				throw new ImageCommonException("ImageCommon!ValidateAndLoad: Unable to parse FM Collection XML file '" + xmlFile + "'.", innerException2);
			}
			finally
			{
				textReader.Close();
			}
			List<IGrouping<string, FMCollectionItem>> list = (from g in xmlInput.FMs.GroupBy((FMCollectionItem fm) => fm.ID, StringComparer.OrdinalIgnoreCase)
			where g.Count<FMCollectionItem>() > 1
			select g).ToList<IGrouping<string, FMCollectionItem>>();
			if (list.Count<IGrouping<string, FMCollectionItem>>() > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (IGrouping<string, FMCollectionItem> grouping in list)
				{
					stringBuilder.AppendLine(Environment.NewLine + "\t" + grouping.Key + ": ");
					foreach (FMCollectionItem fmcollectionItem in grouping)
					{
						stringBuilder.AppendLine("\t\t" + fmcollectionItem.Path);
					}
				}
				throw new ImageCommonException("ImageCommon!ValidateAndLoad: Duplicate FMIDs found in FM Collection XML file '" + xmlFile + "': " + stringBuilder.ToString());
			}
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000B2C8 File Offset: 0x000094C8
		public void ValidateFeatureIdentiferPackages(List<PublishingPackageInfo> packages)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			using (List<FeatureIdentifierPackage>.Enumerator enumerator = this.FeatureIdentifierPackages.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FeatureIdentifierPackage fip = enumerator.Current;
					if (fip.FixUpAction == FeatureIdentifierPackage.FixUpActions.None || (fip.FixUpAction == FeatureIdentifierPackage.FixUpActions.AndFeature && !string.IsNullOrEmpty(fip.ID)))
					{
						List<PublishingPackageInfo> source = (from pkg in packages
						where string.Equals(pkg.FeatureID, fip.FeatureID, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(fip.ID) && string.Equals(pkg.ID, fip.ID, StringComparison.OrdinalIgnoreCase) && string.Equals(pkg.Partition, fip.Partition, StringComparison.OrdinalIgnoreCase)
						select pkg).ToList<PublishingPackageInfo>();
						if (source.Count<PublishingPackageInfo>() == 0)
						{
							flag = true;
							stringBuilder.AppendFormat("\t{0} (FeatureID={1})\n", fip.ID, fip.FeatureID);
						}
					}
				}
			}
			if (flag)
			{
				throw new ImageCommonException("ImageCommon!ValidateAndLoad: The following Feature Identifier Packages specified in the FMCollectionManifest could not be found:" + stringBuilder);
			}
		}

		// Token: 0x04000114 RID: 276
		[DefaultValue(false)]
		public bool IsBuildFeatureEnabled;

		// Token: 0x04000115 RID: 277
		[XmlArray]
		[XmlArrayItem(ElementName = "Language", Type = typeof(string), IsNullable = false)]
		public List<string> SupportedLanguages = new List<string>();

		// Token: 0x04000116 RID: 278
		[XmlArray]
		[XmlArrayItem(ElementName = "Locale", Type = typeof(string), IsNullable = false)]
		public List<string> SupportedLocales = new List<string>();

		// Token: 0x04000117 RID: 279
		[XmlArray]
		[XmlArrayItem(ElementName = "Resolution", Type = typeof(string), IsNullable = false)]
		public List<string> SupportedResolutions = new List<string>();

		// Token: 0x04000118 RID: 280
		[XmlArray]
		[XmlArrayItem(ElementName = "FM", Type = typeof(FMCollectionItem), IsNullable = false)]
		public List<FMCollectionItem> FMs = new List<FMCollectionItem>();

		// Token: 0x04000119 RID: 281
		[XmlArray]
		[XmlArrayItem(ElementName = "FeatureIdentifierPackage", Type = typeof(FeatureIdentifierPackage), IsNullable = false)]
		public List<FeatureIdentifierPackage> FeatureIdentifierPackages = new List<FeatureIdentifierPackage>();
	}
}
