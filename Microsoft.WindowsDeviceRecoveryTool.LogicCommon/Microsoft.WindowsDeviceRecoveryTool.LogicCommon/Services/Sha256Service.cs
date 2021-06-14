using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services
{
	// Token: 0x02000040 RID: 64
	[Export]
	public class Sha256Service : IDisposable, IChecksumService
	{
		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000361 RID: 865 RVA: 0x0000FF7C File Offset: 0x0000E17C
		// (remove) Token: 0x06000362 RID: 866 RVA: 0x0000FFB8 File Offset: 0x0000E1B8
		public event Action<int> ProgressEvent;

		// Token: 0x06000363 RID: 867 RVA: 0x0000FFF4 File Offset: 0x0000E1F4
		[ImportingConstructor]
		public Sha256Service()
		{
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000FFFF File Offset: 0x0000E1FF
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00010014 File Offset: 0x0000E214
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				this.disposed = true;
			}
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0001003C File Offset: 0x0000E23C
		private void RaiseProgressEvent(int progress)
		{
			Action<int> progressEvent = this.ProgressEvent;
			if (progressEvent != null)
			{
				progressEvent(progress);
			}
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00010064 File Offset: 0x0000E264
		public bool IsOfType(string checksumTypeName)
		{
			bool flag = string.Equals(checksumTypeName, "sha-256", StringComparison.InvariantCultureIgnoreCase);
			if (!flag)
			{
				flag = string.Equals(checksumTypeName, "sha256", StringComparison.InvariantCultureIgnoreCase);
			}
			return flag;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00010098 File Offset: 0x0000E298
		public byte[] CalculateChecksum(string filePath, CancellationToken cancellationToken)
		{
			FileInfo fileInfo = new FileInfo(filePath);
			if (!fileInfo.Exists)
			{
				throw new InvalidOperationException(string.Format("File '{0}' not found.", filePath));
			}
			byte[] result;
			using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				result = this.CalculateChecksum(fileStream, cancellationToken);
			}
			return result;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00010104 File Offset: 0x0000E304
		public byte[] CalculateChecksum(FileStream fileStream, CancellationToken cancellationToken)
		{
			byte[] result;
			if (fileStream.Length == 0L)
			{
				result = null;
			}
			else
			{
				using (SHA256Managed sha256Managed = new SHA256Managed())
				{
					byte[] array = new byte[4096];
					long num = 0L;
					int num2;
					do
					{
						num2 = fileStream.Read(array, 0, 4096);
						if (num2 > 0)
						{
							sha256Managed.TransformBlock(array, 0, num2, array, 0);
						}
						num += (long)num2;
						if (num % 4096000L == 0L)
						{
							this.RaiseProgressEvent((int)(num * 100L / fileStream.Length % 101L));
						}
						cancellationToken.ThrowIfCancellationRequested();
					}
					while (num2 > 0);
					sha256Managed.TransformFinalBlock(array, 0, num2);
					result = sha256Managed.Hash;
				}
			}
			return result;
		}

		// Token: 0x04000195 RID: 405
		private const string MsrChecksumTypeName = "sha-256";

		// Token: 0x04000196 RID: 406
		private const string MsrChecksumTypeNameOther = "sha256";

		// Token: 0x04000197 RID: 407
		private bool disposed;
	}
}
