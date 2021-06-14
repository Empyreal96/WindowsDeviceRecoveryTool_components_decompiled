using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services
{
	// Token: 0x02000033 RID: 51
	[Export]
	public class Crc32Service : IDisposable
	{
		// Token: 0x060002BD RID: 701 RVA: 0x0000B5ED File Offset: 0x000097ED
		[ImportingConstructor]
		public Crc32Service()
		{
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060002BE RID: 702 RVA: 0x0000B5F8 File Offset: 0x000097F8
		// (remove) Token: 0x060002BF RID: 703 RVA: 0x0000B634 File Offset: 0x00009834
		public event Action<int> Crc32ProgressEvent;

		// Token: 0x060002C0 RID: 704 RVA: 0x0000B670 File Offset: 0x00009870
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000B684 File Offset: 0x00009884
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				this.disposed = true;
			}
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000B6AC File Offset: 0x000098AC
		public uint CalculateCrc32(string filePath, CancellationToken cancellationToken)
		{
			FileInfo fileInfo = new FileInfo(filePath);
			if (!fileInfo.Exists)
			{
				throw new InvalidOperationException(string.Format("File '{0}' not found.", filePath));
			}
			uint result;
			using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				result = this.CalculateCrc32(fileStream, cancellationToken);
			}
			return result;
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000B718 File Offset: 0x00009918
		private uint CalculateCrc32(Stream fileStream, CancellationToken cancellationToken)
		{
			uint[] array = new uint[256];
			for (uint num = 0U; num < 256U; num += 1U)
			{
				uint num2 = num;
				for (int i = 8; i > 0; i--)
				{
					if ((num2 & 1U) == 1U)
					{
						num2 = (num2 >> 1 ^ 3988292384U);
					}
					else
					{
						num2 >>= 1;
					}
				}
				array[(int)((UIntPtr)num)] = num2;
			}
			uint num3 = uint.MaxValue;
			long num4 = 0L;
			for (;;)
			{
				byte[] array2 = new byte[65536];
				int num5 = fileStream.Read(array2, 0, array2.Length);
				num4 += (long)num5;
				if (0 == num5)
				{
					break;
				}
				for (int j = 0; j < num5; j++)
				{
					num3 = (num3 >> 8 ^ array[(int)((UIntPtr)((uint)array2[j] ^ (num3 & 255U)))]);
				}
				if (num4 % 65536000L == 0L)
				{
					this.RaiseProgressEvent((int)(num4 * 100L / fileStream.Length % 101L));
				}
				cancellationToken.ThrowIfCancellationRequested();
			}
			return ~num3;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000B848 File Offset: 0x00009A48
		private void RaiseProgressEvent(int progress)
		{
			Action<int> crc32ProgressEvent = this.Crc32ProgressEvent;
			if (crc32ProgressEvent != null)
			{
				crc32ProgressEvent(progress);
			}
		}

		// Token: 0x04000164 RID: 356
		private bool disposed;
	}
}
