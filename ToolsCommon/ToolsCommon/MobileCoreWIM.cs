using System;
using System.Threading;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000018 RID: 24
	public class MobileCoreWIM : MobileCoreImage
	{
		// Token: 0x060000A9 RID: 169 RVA: 0x00005E24 File Offset: 0x00004024
		internal MobileCoreWIM(string path) : base(path)
		{
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00005E30 File Offset: 0x00004030
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
					this.MountWIM(readOnly);
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

		// Token: 0x060000AB RID: 171 RVA: 0x00005E8C File Offset: 0x0000408C
		public override void MountReadOnly()
		{
			bool readOnly = true;
			this.MountWithRetry(readOnly);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00005EA4 File Offset: 0x000040A4
		public override void Mount()
		{
			bool readOnly = false;
			this.MountWithRetry(readOnly);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00005EBC File Offset: 0x000040BC
		private void MountWIM(bool readOnly)
		{
			lock (MobileCoreWIM._lockObj)
			{
				this.m_partitions.Clear();
				string text = string.Empty;
				if (!readOnly)
				{
					text = FileUtils.GetTempDirectory();
				}
				string tempDirectory = FileUtils.GetTempDirectory();
				if (CommonUtils.MountWIM(this.m_mobileCoreImagePath, tempDirectory, text))
				{
					this.m_partitions.Add(new ImagePartition("WIM", tempDirectory));
					this._tmpDir = text;
					this._mountPoint = tempDirectory;
					this._commitChanges = !readOnly;
				}
				else
				{
					FileUtils.DeleteTree(tempDirectory);
					if (!string.IsNullOrEmpty(text))
					{
						FileUtils.DeleteTree(text);
					}
				}
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00005F68 File Offset: 0x00004168
		public override void Unmount()
		{
			lock (MobileCoreWIM._lockObj)
			{
				CommonUtils.DismountWIM(this.m_mobileCoreImagePath, this._mountPoint, this._commitChanges);
				this.m_partitions.Clear();
				base.IsMounted = false;
				if (!string.IsNullOrEmpty(this._tmpDir))
				{
					FileUtils.DeleteTree(this._tmpDir);
				}
				this._tmpDir = null;
				this._mountPoint = null;
			}
		}

		// Token: 0x0400003C RID: 60
		private const int SLEEP_1000 = 1000;

		// Token: 0x0400003D RID: 61
		private const int MAX_RETRY = 3;

		// Token: 0x0400003E RID: 62
		private bool _commitChanges;

		// Token: 0x0400003F RID: 63
		private string _tmpDir;

		// Token: 0x04000040 RID: 64
		private string _mountPoint;

		// Token: 0x04000041 RID: 65
		private static readonly object _lockObj = new object();
	}
}
