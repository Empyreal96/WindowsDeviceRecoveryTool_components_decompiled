using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services
{
	// Token: 0x0200003C RID: 60
	[Export]
	public class Md5Sevice : IDisposable, IChecksumService
	{
		// Token: 0x06000318 RID: 792 RVA: 0x0000CC3E File Offset: 0x0000AE3E
		[ImportingConstructor]
		public Md5Sevice()
		{
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000319 RID: 793 RVA: 0x0000CC4C File Offset: 0x0000AE4C
		// (remove) Token: 0x0600031A RID: 794 RVA: 0x0000CC88 File Offset: 0x0000AE88
		public event Action<int> ProgressEvent;

		// Token: 0x0600031B RID: 795 RVA: 0x0000CCC4 File Offset: 0x0000AEC4
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000CCD8 File Offset: 0x0000AED8
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				this.disposed = true;
			}
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000CD00 File Offset: 0x0000AF00
		public byte[] CalculateMd5(string filePath, CancellationToken cancellationToken)
		{
			FileInfo fileInfo = new FileInfo(filePath);
			if (!fileInfo.Exists)
			{
				throw new InvalidOperationException(string.Format("File '{0}' not found.", filePath));
			}
			byte[] result;
			using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				result = this.CalculateMd5(fileStream, cancellationToken);
			}
			return result;
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000CD6C File Offset: 0x0000AF6C
		private byte[] CalculateMd5(Stream fileStream, CancellationToken cancellationToken)
		{
			byte[] result;
			if (fileStream.Length == 0L)
			{
				result = null;
			}
			else
			{
				using (MD5 md = MD5.Create())
				{
					byte[] array = new byte[4096];
					long num = 0L;
					int num2;
					do
					{
						num2 = fileStream.Read(array, 0, 4096);
						if (num2 > 0)
						{
							md.TransformBlock(array, 0, num2, array, 0);
						}
						num += (long)num2;
						if (num % 4096000L == 0L)
						{
							this.RaiseProgressEvent((int)(num * 100L / fileStream.Length % 101L));
						}
						cancellationToken.ThrowIfCancellationRequested();
					}
					while (num2 > 0);
					md.TransformFinalBlock(array, 0, num2);
					result = md.Hash;
				}
			}
			return result;
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000CE58 File Offset: 0x0000B058
		private void RaiseProgressEvent(int progress)
		{
			Action<int> progressEvent = this.ProgressEvent;
			if (progressEvent != null)
			{
				progressEvent(progress);
			}
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000CE80 File Offset: 0x0000B080
		public bool IsOfType(string checksumTypeName)
		{
			return string.Equals(checksumTypeName, "md5", StringComparison.InvariantCultureIgnoreCase);
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000CEA0 File Offset: 0x0000B0A0
		public byte[] CalculateChecksum(string filePath, CancellationToken cancellationToken)
		{
			return this.CalculateMd5(filePath, cancellationToken);
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000CEBC File Offset: 0x0000B0BC
		public byte[] CalculateChecksum(FileStream fileStream, CancellationToken cancellationToken)
		{
			return this.CalculateMd5(fileStream, cancellationToken);
		}

		// Token: 0x0400017E RID: 382
		private const string MsrChecksumTypeName = "md5";

		// Token: 0x0400017F RID: 383
		private bool disposed;
	}
}
