using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SoftwareRepository.Streaming
{
	// Token: 0x02000014 RID: 20
	public class FileStreamer : Streamer
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000071 RID: 113 RVA: 0x000031EF File Offset: 0x000013EF
		private static HashSet<char> InvalidFileCharsSet
		{
			get
			{
				HashSet<char> result;
				if ((result = FileStreamer.invalidFileCharsSet) == null)
				{
					result = (FileStreamer.invalidFileCharsSet = new HashSet<char>(Path.GetInvalidFileNameChars()));
				}
				return result;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000072 RID: 114 RVA: 0x0000320A File Offset: 0x0000140A
		public static string DefaultResumeFolder
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SoftwareRepositoryResume");
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000073 RID: 115 RVA: 0x0000321D File Offset: 0x0000141D
		// (set) Token: 0x06000074 RID: 116 RVA: 0x00003225 File Offset: 0x00001425
		public string ResumeFileName { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000075 RID: 117 RVA: 0x0000322E File Offset: 0x0000142E
		// (set) Token: 0x06000076 RID: 118 RVA: 0x00003236 File Offset: 0x00001436
		public string FileName { get; set; }

		// Token: 0x06000077 RID: 119 RVA: 0x0000323F File Offset: 0x0000143F
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "multi")]
		public FileStreamer(string downloadPath, string packageId, bool multiPath = false) : this(downloadPath, packageId, FileStreamer.DefaultResumeFolder, multiPath)
		{
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003250 File Offset: 0x00001450
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "multi")]
		public FileStreamer(string downloadPath, string packageId, string resumeFolder, bool multiPath = false)
		{
			if (string.IsNullOrEmpty(downloadPath))
			{
				throw new ArgumentNullException("downloadPath");
			}
			if (string.IsNullOrEmpty(packageId))
			{
				throw new ArgumentNullException("packageId");
			}
			if (string.IsNullOrEmpty(resumeFolder))
			{
				throw new ArgumentNullException("resumeFolder");
			}
			this.FileName = downloadPath;
			this.ResumeFileName = FileStreamer.GetResumePath(downloadPath, packageId, resumeFolder, multiPath);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000032B4 File Offset: 0x000014B4
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "multi")]
		[SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
		public static string GetResumePath(string downloadPath, string packageId, string resumeFolder = null, bool multiPath = false)
		{
			if (string.IsNullOrEmpty(downloadPath))
			{
				throw new ArgumentNullException("downloadPath");
			}
			if (string.IsNullOrEmpty(packageId))
			{
				throw new ArgumentNullException("packageId");
			}
			resumeFolder = (resumeFolder ?? FileStreamer.DefaultResumeFolder);
			string str;
			if (multiPath)
			{
				using (SHA256 sha = SHA256.Create())
				{
					byte[] source = sha.ComputeHash(Encoding.Unicode.GetBytes(downloadPath));
					str = BitConverter.ToString(source.Take(4).Concat(source.Skip(28)).ToArray<byte>()).Replace("-", string.Empty).ToLowerInvariant();
					goto IL_93;
				}
			}
			str = Path.GetFileName(downloadPath);
			IL_93:
			return Path.Combine(resumeFolder, new string(packageId.Select(delegate(char c)
			{
				if (!FileStreamer.InvalidFileCharsSet.Contains(c))
				{
					return c;
				}
				return '-';
			}).ToArray<char>()) + "_" + str + ".resume");
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000033AC File Offset: 0x000015AC
		public override void SetMetadata(byte[] metadata)
		{
			if (metadata != null)
			{
				string directoryName = Path.GetDirectoryName(this.ResumeFileName);
				if (!string.IsNullOrEmpty(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				using (FileStream fileStream = new FileStream(this.ResumeFileName, FileMode.Create))
				{
					fileStream.Write(metadata, 0, metadata.Length);
					return;
				}
			}
			this.ClearMetadata();
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003414 File Offset: 0x00001614
		public override byte[] GetMetadata()
		{
			if (File.Exists(this.ResumeFileName))
			{
				if (File.Exists(this.FileName))
				{
					using (FileStream fileStream = new FileStream(this.ResumeFileName, FileMode.Open))
					{
						fileStream.Seek(0L, SeekOrigin.Begin);
						byte[] array = new byte[fileStream.Length];
						fileStream.Read(array, 0, array.Length);
						return array;
					}
				}
				this.ClearMetadata();
			}
			return null;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003494 File Offset: 0x00001694
		public override void ClearMetadata()
		{
			if (File.Exists(this.ResumeFileName))
			{
				File.Delete(this.ResumeFileName);
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000034B0 File Offset: 0x000016B0
		protected override Stream GetStreamInternal()
		{
			string directoryName = Path.GetDirectoryName(this.FileName);
			if (!string.IsNullOrEmpty(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			return new FileStream(this.FileName, FileMode.OpenOrCreate);
		}

		// Token: 0x0400004A RID: 74
		private static HashSet<char> invalidFileCharsSet;

		// Token: 0x0400004B RID: 75
		public const string ResumeExtension = ".resume";

		// Token: 0x0400004C RID: 76
		public const string AppDataSubFolder = "SoftwareRepositoryResume";
	}
}
