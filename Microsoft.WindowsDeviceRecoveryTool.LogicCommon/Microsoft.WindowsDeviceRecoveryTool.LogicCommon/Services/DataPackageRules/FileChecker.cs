using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.WindowsDeviceRecoveryTool.Common;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services.DataPackageRules
{
	// Token: 0x02000034 RID: 52
	[Export]
	public class FileChecker
	{
		// Token: 0x060002C5 RID: 709 RVA: 0x0000B870 File Offset: 0x00009A70
		[ImportingConstructor]
		public FileChecker(Crc32Service crc32Service, Md5Sevice md5Sevice, Sha256Service sha256Service)
		{
			this.crc32Service = crc32Service;
			this.crc32Service.Crc32ProgressEvent += this.Progress;
			this.checksumServices = new List<IChecksumService>
			{
				md5Sevice,
				sha256Service
			};
			this.md5Sevice = md5Sevice;
			this.sha256Service = sha256Service;
			this.md5Sevice.ProgressEvent += this.Progress;
			this.sha256Service.ProgressEvent += this.Progress;
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x0000B900 File Offset: 0x00009B00
		// (set) Token: 0x060002C7 RID: 711 RVA: 0x0000B917 File Offset: 0x00009B17
		public double CrcCurrentProgress { get; set; }

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x0000B920 File Offset: 0x00009B20
		// (set) Token: 0x060002C9 RID: 713 RVA: 0x0000B937 File Offset: 0x00009B37
		public double ProgressModificator { get; set; }

		// Token: 0x060002CA RID: 714 RVA: 0x0000B940 File Offset: 0x00009B40
		public static void ValidateSpaceAvailability(string path, long sizeNeeded)
		{
			string pathRoot = Path.GetPathRoot(path);
			try
			{
				if (string.IsNullOrEmpty(pathRoot))
				{
					throw new CannotAccessDirectoryException(path);
				}
				long availableFreeSpace = new DriveInfo(pathRoot).AvailableFreeSpace;
				if (availableFreeSpace < sizeNeeded)
				{
					throw new NotEnoughSpaceException
					{
						Available = availableFreeSpace,
						Needed = sizeNeeded,
						Disk = pathRoot
					};
				}
			}
			catch (Exception ex)
			{
				if (ex is NotEnoughSpaceException)
				{
					throw;
				}
				throw new CannotAccessDirectoryException(path);
			}
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000BA14 File Offset: 0x00009C14
		public void CheckFilesCorrectness(string destinationPath, IEnumerable<File4> files, CancellationToken token)
		{
			IList<File4> list = (files as IList<File4>) ?? files.ToList<File4>();
			long num = list.Sum((File4 file) => this.GetFileLength(Path.Combine(destinationPath, file.FileName)));
			this.CrcCurrentProgress = 0.0;
			foreach (File4 file2 in list)
			{
				long fileLength = this.GetFileLength(Path.Combine(destinationPath, file2.FileName));
				this.ProgressModificator = (double)fileLength / (double)num;
				token.ThrowIfCancellationRequested();
				if (!this.CheckSumEqual(Path.Combine(destinationPath, file2.FileName), file2.Checksum, token))
				{
					throw new Crc32Exception(file2.FileName);
				}
				this.CrcCurrentProgress += this.ProgressModificator * 100.0;
			}
			this.Progress(100);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000BB44 File Offset: 0x00009D44
		public long GetFileLength(string file)
		{
			FileInfo fileInfo = new FileInfo(file);
			return fileInfo.Length;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000BB8C File Offset: 0x00009D8C
		public byte[] CheckFile(string checksumTypeName, string filePath, CancellationToken cancellationToken)
		{
			IChecksumService checksumService = this.checksumServices.FirstOrDefault((IChecksumService ch) => ch.IsOfType(checksumTypeName));
			if (checksumService == null)
			{
				throw new InvalidOperationException(string.Format("No checksum service found for checksum: {0}", checksumTypeName));
			}
			return checksumService.CalculateChecksum(filePath, cancellationToken);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000BBF0 File Offset: 0x00009DF0
		public bool CheckSumEqual(string fileName, string crc, CancellationToken token)
		{
			uint num = this.crc32Service.CalculateCrc32(fileName, token);
			return num == Convert.ToUInt32(crc);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000BC1C File Offset: 0x00009E1C
		public ReadOnlyCollection<string> FindLocalVplFilePaths(string productType, string productCode, string searchPath)
		{
			LocalDataPackageAccess localDataPackageAccess = new LocalDataPackageAccess();
			ReadOnlyCollection<string> vplPathList;
			if (string.IsNullOrEmpty(productCode))
			{
				vplPathList = localDataPackageAccess.GetVplPathList(productType, searchPath);
			}
			else
			{
				vplPathList = localDataPackageAccess.GetVplPathList(productType, productCode, searchPath);
			}
			return vplPathList;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000BC56 File Offset: 0x00009E56
		public void SetProgressHandler(Action<double> action)
		{
			this.progressHandler = action;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000BC80 File Offset: 0x00009E80
		public void CheckFiles(List<FileCrcInfo> filesToCheck, CancellationToken cancellationToken)
		{
			Tracer<FileChecker>.LogEntry("CheckFiles");
			long num = filesToCheck.Sum((FileCrcInfo file) => this.GetFileLength(file.FilePath));
			this.CrcCurrentProgress = 0.0;
			foreach (FileCrcInfo fileCrcInfo in filesToCheck)
			{
				long fileLength = this.GetFileLength(fileCrcInfo.FilePath);
				this.ProgressModificator = (double)fileLength / (double)num;
				uint num2 = this.crc32Service.CalculateCrc32(fileCrcInfo.FilePath, cancellationToken);
				if (num2 != uint.Parse(fileCrcInfo.Crc, NumberStyles.HexNumber))
				{
					Tracer<FileChecker>.WriteInformation("Crc check failed: {0} crc: {1}", new object[]
					{
						fileCrcInfo.FileName,
						num2.ToString("X8")
					});
					throw new Crc32Exception(fileCrcInfo.FileName);
				}
				this.CrcCurrentProgress += this.ProgressModificator * 100.0;
			}
			this.Progress(100);
			Tracer<FileChecker>.LogExit("CheckFiles");
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000BDE0 File Offset: 0x00009FE0
		private void Progress(int progress)
		{
			double progressToShow = this.CrcCurrentProgress + (double)progress * this.ProgressModificator;
			if (this.progressHandler != null)
			{
				AppDispatcher.Execute(delegate
				{
					this.progressHandler((double)((int)progressToShow));
				}, false);
			}
		}

		// Token: 0x04000166 RID: 358
		private readonly Crc32Service crc32Service;

		// Token: 0x04000167 RID: 359
		private readonly Md5Sevice md5Sevice;

		// Token: 0x04000168 RID: 360
		private readonly Sha256Service sha256Service;

		// Token: 0x04000169 RID: 361
		private List<IChecksumService> checksumServices;

		// Token: 0x0400016A RID: 362
		private Action<double> progressHandler;
	}
}
