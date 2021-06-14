using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Microsoft.WindowsPhone.FeatureAPI;
using Microsoft.WindowsPhone.ImageUpdate.Tools.Common;

namespace Microsoft.WindowsPhone.Imaging
{
	// Token: 0x0200001B RID: 27
	[XmlRoot(ElementName = "Project", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003", IsNullable = false)]
	[XmlType(Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
	public class PropsProject
	{
		// Token: 0x06000154 RID: 340 RVA: 0x00006F35 File Offset: 0x00005135
		public PropsProject()
		{
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00006F3D File Offset: 0x0000513D
		public PropsProject(List<string> supportedUILanguages, List<string> supportedLocales, List<string> supportedResolutions, string buildType, string cpuType, string MSPackageRoot)
		{
			this._supportedUILangs = supportedUILanguages;
			this._supportedLocales = supportedLocales;
			this._supportedResolutions = supportedResolutions;
			this._buildType = buildType;
			this._cpuType = cpuType;
			this._MSPackageRoot = MSPackageRoot;
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00006F72 File Offset: 0x00005172
		// (set) Token: 0x06000157 RID: 343 RVA: 0x00006F7A File Offset: 0x0000517A
		[XmlIgnore]
		public List<PropsFile> Files
		{
			get
			{
				return this.ItemGroup;
			}
			set
			{
				this.ItemGroup = value;
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00006FA4 File Offset: 0x000051A4
		public void AddPackages(FeatureManifest fm)
		{
			List<FeatureManifest.FMPkgInfo> list = new List<FeatureManifest.FMPkgInfo>();
			fm.GetAllPackageByGroups(ref list, this._supportedUILangs, this._supportedLocales, this._supportedResolutions, this._buildType, this._cpuType, this._MSPackageRoot);
			if (this.Files == null)
			{
				this.Files = new List<PropsFile>();
			}
			foreach (FeatureManifest.FMPkgInfo fmpkgInfo in list)
			{
				PropsFile propFile = new PropsFile();
				string rawBasePath = fmpkgInfo.RawBasePath;
				string fileName = Path.GetFileName(fmpkgInfo.PackagePath);
				propFile.Include = this.ConvertToInclude(rawBasePath, fileName);
				if ((from prop in this.Files
				where prop.Include.Equals(propFile.Include, StringComparison.OrdinalIgnoreCase)
				select prop).Count<PropsFile>() == 0)
				{
					propFile.Feature = PropsProject.FeatureTypes.Generated_Product_Packages.ToString();
					propFile.InstallPath = this.ConvertToInstallPath(rawBasePath);
					this.SetGUID(ref propFile);
					propFile.Owner = "FeatureManifest";
					propFile.BusinessReason = "Device Imaging";
					this.Files.Add(propFile);
				}
			}
		}

		// Token: 0x06000159 RID: 345 RVA: 0x000070FC File Offset: 0x000052FC
		private string ConvertToInstallPath(string installPath)
		{
			string text = installPath.Replace(Path.GetFileName(installPath), "").TrimEnd(new char[]
			{
				'\\'
			});
			text = text.Replace("$(cputype)", "$(_BuildArch)");
			text = text.Replace("$(buildtype)", "$(_BuildType)");
			return text.Replace("$(mspackageroot)", "$(WP_PACKAGES_INSTALL_PATH)");
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00007164 File Offset: 0x00005364
		private string ConvertToInclude(string include, string fileName)
		{
			string text = include.Replace(Path.GetFileName(include), fileName);
			text = text.Replace("$(cputype)", "$(_BuildArch)");
			text = text.Replace("$(buildtype)", "$(_BuildType)");
			return text.Replace("$(mspackageroot)", "$(BINARY_ROOT)\\prebuilt");
		}

		// Token: 0x0600015B RID: 347 RVA: 0x000071B8 File Offset: 0x000053B8
		private void SetGUID(ref PropsFile propsFile)
		{
			string text = Guid.NewGuid().ToString("B");
			if (string.Equals(this._cpuType, FeatureManifest.CPUType_ARM, StringComparison.OrdinalIgnoreCase))
			{
				if (string.Equals(this._buildType, "fre", StringComparison.OrdinalIgnoreCase))
				{
					propsFile.MC_ARM_FRE = text;
					return;
				}
				propsFile.MC_ARM_CHK = text;
				return;
			}
			else if (string.Equals(this._cpuType, FeatureManifest.CPUType_X86, StringComparison.OrdinalIgnoreCase))
			{
				if (string.Equals(this._buildType, "fre", StringComparison.OrdinalIgnoreCase))
				{
					propsFile.MC_X86_FRE = text;
					return;
				}
				propsFile.MC_X86_CHK = text;
				return;
			}
			else
			{
				if (!string.Equals(this._cpuType, FeatureManifest.CPUType_ARM64, StringComparison.OrdinalIgnoreCase))
				{
					if (string.Equals(this._cpuType, FeatureManifest.CPUType_AMD64, StringComparison.OrdinalIgnoreCase))
					{
						if (string.Equals(this._buildType, "fre", StringComparison.OrdinalIgnoreCase))
						{
							propsFile.MC_AMD64_FRE = text;
							return;
						}
						propsFile.MC_AMD64_CHK = text;
					}
					return;
				}
				if (string.Equals(this._buildType, "fre", StringComparison.OrdinalIgnoreCase))
				{
					propsFile.MC_ARM64_FRE = text;
					return;
				}
				propsFile.MC_ARM64_CHK = text;
				return;
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x000072B8 File Offset: 0x000054B8
		public static void ValidateAndLoad(ref PropsProject xmlInput, string xmlFile, IULogger logger)
		{
			string text = string.Empty;
			string propsProjectSchema = BuildPaths.PropsProjectSchema;
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			foreach (string text2 in manifestResourceNames)
			{
				if (text2.Contains(propsProjectSchema))
				{
					text = text2;
					break;
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				throw new ImageCommonException("ImageCommon!ValidateInput: XSD resource was not found: " + propsProjectSchema);
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
					throw new ImageCommonException("ImageCommon!ValidateInput: Unable to validate Props Project XSD.", innerException);
				}
			}
			logger.LogInfo("ImageCommon: Successfully validated the Props Project XML: {0}", new object[]
			{
				xmlFile
			});
			TextReader textReader = new StreamReader(xmlFile);
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(PropsProject));
				xmlInput = (PropsProject)xmlSerializer.Deserialize(textReader);
			}
			catch (Exception innerException2)
			{
				throw new ImageCommonException("ImageCommon!ValidateInput: Unable to parse Props Project XML file.", innerException2);
			}
			finally
			{
				textReader.Close();
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x000073F0 File Offset: 0x000055F0
		public void Merge(PropsProject sourceProps)
		{
			if (this.Files == null)
			{
				if (sourceProps != null && sourceProps.Files != null)
				{
					this.Files = sourceProps.Files;
					return;
				}
			}
			else if (sourceProps != null && sourceProps.Files != null)
			{
				this.Files.AddRange(sourceProps.Files);
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00007430 File Offset: 0x00005630
		public void WriteToFile(string fileName)
		{
			TextWriter textWriter = new StreamWriter(fileName);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(PropsProject));
			try
			{
				xmlSerializer.Serialize(textWriter, this);
			}
			catch (Exception innerException)
			{
				throw new ImageCommonException("ImageCommon!WriteToFile: Unable to write Props Project XML file '" + fileName + "'", innerException);
			}
			finally
			{
				textWriter.Close();
			}
		}

		// Token: 0x0400009F RID: 159
		private List<string> _supportedUILangs;

		// Token: 0x040000A0 RID: 160
		private List<string> _supportedLocales;

		// Token: 0x040000A1 RID: 161
		private List<string> _supportedResolutions;

		// Token: 0x040000A2 RID: 162
		private string _buildType;

		// Token: 0x040000A3 RID: 163
		private string _cpuType;

		// Token: 0x040000A4 RID: 164
		private string _MSPackageRoot;

		// Token: 0x040000A5 RID: 165
		[XmlArrayItem(ElementName = "File", Type = typeof(PropsFile), IsNullable = false)]
		[XmlArray]
		public List<PropsFile> ItemGroup;

		// Token: 0x0200001C RID: 28
		public enum FeatureTypes
		{
			// Token: 0x040000A7 RID: 167
			Generated_Product_Packages
		}
	}
}
