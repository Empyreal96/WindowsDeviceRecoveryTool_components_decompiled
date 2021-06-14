using System;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000017 RID: 23
	public class MobileCoreVHD : MobileCoreImage
	{
		// Token: 0x060000A2 RID: 162 RVA: 0x00005C44 File Offset: 0x00003E44
		internal MobileCoreVHD(string path) : base(path)
		{
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00005C58 File Offset: 0x00003E58
		private void MountWithRetry(bool readOnly)
		{
			if (base.IsMounted)
			{
				return;
			}
			int num = 0;
			int num2 = 3;
			bool flag = false;
			do
			{
				flag = false;
				try
				{
					this.MountVHD(readOnly);
				}
				catch (Exception)
				{
					num++;
					flag = (num < num2);
					if (!flag)
					{
						throw;
					}
					Thread.Sleep(1000);
				}
			}
			while (flag);
			base.IsMounted = true;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00005CB4 File Offset: 0x00003EB4
		public override void MountReadOnly()
		{
			bool readOnly = true;
			this.MountWithRetry(readOnly);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00005CCC File Offset: 0x00003ECC
		public override void Mount()
		{
			bool readOnly = false;
			this.MountWithRetry(readOnly);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00005CE4 File Offset: 0x00003EE4
		private void MountVHD(bool readOnly)
		{
			lock (MobileCoreVHD._lockObj)
			{
				this.m_partitions.Clear();
				try
				{
					this._hndlVirtDisk = CommonUtils.MountVHD(this.m_mobileCoreImagePath, readOnly);
					int capacity = 1024;
					StringBuilder stringBuilder = new StringBuilder(capacity);
					int virtualDiskPhysicalPath = VirtualDiskLib.GetVirtualDiskPhysicalPath(this._hndlVirtDisk, ref capacity, stringBuilder);
					if (0 < virtualDiskPhysicalPath)
					{
						throw new Win32Exception(virtualDiskPhysicalPath);
					}
					this.m_partitions.PopulateFromPhysicalDeviceId(stringBuilder.ToString());
					if (this.m_partitions.Count == 0)
					{
						throw new IUException("Could not retrieve logical drive information for {0}", new object[]
						{
							stringBuilder
						});
					}
				}
				catch (Exception)
				{
					this.Unmount();
					throw;
				}
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00005DB4 File Offset: 0x00003FB4
		public override void Unmount()
		{
			lock (MobileCoreVHD._lockObj)
			{
				CommonUtils.DismountVHD(this._hndlVirtDisk);
				this._hndlVirtDisk = IntPtr.Zero;
				this.m_partitions.Clear();
				base.IsMounted = false;
			}
		}

		// Token: 0x04000038 RID: 56
		private const int SLEEP_1000 = 1000;

		// Token: 0x04000039 RID: 57
		private const int MAX_RETRY = 3;

		// Token: 0x0400003A RID: 58
		private IntPtr _hndlVirtDisk = IntPtr.Zero;

		// Token: 0x0400003B RID: 59
		private static readonly object _lockObj = new object();
	}
}
