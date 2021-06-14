using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.SecureBoot;
using Microsoft.WindowsPhone.ImageUpdate.PkgCommon;
using Microsoft.WindowsPhone.ImageUpdate.Tools.Common;

namespace Microsoft.WindowsPhone.Imaging
{
	// Token: 0x0200002B RID: 43
	public class PlatformManifestGen
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x0000B8B0 File Offset: 0x00009AB0
		public bool ErrorsFound
		{
			get
			{
				return this.ErrorMessages.Length > 0;
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000B8D0 File Offset: 0x00009AD0
		public PlatformManifestGen(Guid featureManifestID, string buildBranchInfo, string signInfoPath, ReleaseType releaseType, IULogger logger)
		{
			this._logger = logger;
			this._signinfoPath = signInfoPath;
			this._pmAPI = new PlatformManifest(featureManifestID, buildBranchInfo);
			this._pmAPI.ImageType = ((releaseType == 2) ? 0 : 1);
			if (!string.IsNullOrWhiteSpace(this._signinfoPath) && Directory.Exists(this._signinfoPath))
			{
				this._doSignInfo = true;
			}
			if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(PlatformManifestGen.c_strSignInfoEnabledEnvVar)))
			{
				if (!this._doSignInfo)
				{
					throw new ImageCommonException("ImageCommon!PlatformManifestGen::PlatformManifestGen: The SignInfo Path does not exist '" + this._signinfoPath + "' but is required.");
				}
			}
			else
			{
				this._doSignInfo = false;
			}
			if (this._doSignInfo && PlatformManifestGen._signInfoFiles == null)
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(this._signinfoPath);
				PlatformManifestGen._signInfoFiles = (from file in directoryInfo.GetFiles("*.signinfo")
				select file.Name.ToLower()).ToList<string>();
			}
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000B9CC File Offset: 0x00009BCC
		public void AddPackages(List<IPkgInfo> packages)
		{
			foreach (IPkgInfo package in packages)
			{
				this.AddPackage(package);
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000BA4C File Offset: 0x00009C4C
		public void AddPackage(IPkgInfo package)
		{
			this._pmAPI.AddStringEntry(package.Name);
			if (this._doSignInfo)
			{
				new StringBuilder();
				foreach (IFileEntry fileEntry in package.Files)
				{
					if (fileEntry.SignInfoRequired)
					{
						byte[] inArray = PlatformManifestGen.HexStringToByteArray(fileEntry.FileHash);
						string fileHash = Convert.ToBase64String(inArray).Replace('/', '-').ToLower();
						if (fileHash == string.Empty)
						{
							this._logger.LogWarning("Warning: File '{0}' in package '{1}' requires signInfo but has empty fileHash!", new object[]
							{
								fileEntry.DevicePath,
								package.Name
							});
						}
						else
						{
							List<string> list = (from file in PlatformManifestGen._signInfoFiles
							where file.Contains(fileHash)
							select file).ToList<string>();
							if (list.Count<string>() != 1)
							{
								if (list.Count<string>() == 0)
								{
									string filename = Path.GetFileName(fileEntry.CabPath);
									if (PlatformManifestGen._signInfoFiles.Any((string file) => file.StartsWith(filename, StringComparison.OrdinalIgnoreCase)))
									{
										this.ErrorMessages.AppendLine(string.Concat(new string[]
										{
											"Error: File '",
											fileEntry.DevicePath,
											"' in package '",
											package.Name,
											"' failed to find any SignInfo files using the following pattern: ",
											fileHash,
											" (Filename found but using different hashes)"
										}));
										continue;
									}
									this.ErrorMessages.AppendLine(string.Concat(new string[]
									{
										"Error: File '",
										fileEntry.DevicePath,
										"' in package '",
										package.Name,
										"' failed to find any SignInfo files using the following pattern: ",
										fileHash
									}));
									continue;
								}
								else
								{
									string text = string.Concat(new string[]
									{
										"Error: File '",
										fileEntry.DevicePath,
										"' in package '",
										package.Name,
										"' failed as we found multiple SignInfo files using the following pattern: ",
										fileHash
									});
									this.ErrorMessages.AppendLine(text);
									this._logger.LogError(text, new object[0]);
									using (List<string>.Enumerator enumerator2 = list.GetEnumerator())
									{
										while (enumerator2.MoveNext())
										{
											string arg = enumerator2.Current;
											this._logger.LogError(string.Format("Error: \tMatching file: '{0}'", arg), new object[0]);
										}
										continue;
									}
								}
							}
							string text2 = Path.Combine(this._signinfoPath, list[0]);
							try
							{
								this._pmAPI.AddBinaryFromSignInfo(text2);
							}
							catch
							{
								this.ErrorMessages.AppendLine(string.Concat(new string[]
								{
									"Error: File '",
									fileEntry.DevicePath,
									"' in package '",
									package.Name,
									"' failed to be added to the Platform Manifest"
								}));
							}
						}
					}
				}
			}
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000BDF0 File Offset: 0x00009FF0
		public static byte[] HexStringToByteArray(string hex)
		{
			return (from x in Enumerable.Range(0, hex.Length)
			where x % 2 == 0
			select Convert.ToByte(hex.Substring(x, 2), 16)).ToArray<byte>();
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000BE53 File Offset: 0x0000A053
		public void WriteToFile(string outputFile)
		{
			this._pmAPI.WriteToFile(outputFile);
		}

		// Token: 0x0400013B RID: 315
		private IULogger _logger;

		// Token: 0x0400013C RID: 316
		private string _signinfoPath;

		// Token: 0x0400013D RID: 317
		private bool _doSignInfo;

		// Token: 0x0400013E RID: 318
		private static List<string> _signInfoFiles = null;

		// Token: 0x0400013F RID: 319
		private PlatformManifest _pmAPI;

		// Token: 0x04000140 RID: 320
		public static string c_strSignInfoExtension = ".signinfo";

		// Token: 0x04000141 RID: 321
		public static string c_strSignInfoDir = "SignInfo";

		// Token: 0x04000142 RID: 322
		public static string c_strPlatformManifestMainOSDevicePath = "\\Windows\\System32\\PlatformManifest\\";

		// Token: 0x04000143 RID: 323
		public static string c_strPlatformManifestEFIESPDevicePath = "\\EFI\\Microsoft\\Boot\\PlatformManifest\\";

		// Token: 0x04000144 RID: 324
		public static string c_strPlatformManifestSubcomponent = "PlatformManifest";

		// Token: 0x04000145 RID: 325
		public static string c_strPlatformManifestExtension = ".pm";

		// Token: 0x04000146 RID: 326
		public static string c_strSignInfoEnabledEnvVar = "GENERATE_SIGNINFO_FILES";

		// Token: 0x04000147 RID: 327
		public StringBuilder ErrorMessages = new StringBuilder();
	}
}
