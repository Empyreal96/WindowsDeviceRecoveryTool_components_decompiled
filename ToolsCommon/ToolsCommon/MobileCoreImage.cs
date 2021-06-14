using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000016 RID: 22
	public abstract class MobileCoreImage
	{
		// Token: 0x06000095 RID: 149 RVA: 0x00005970 File Offset: 0x00003B70
		protected MobileCoreImage(string path)
		{
			this.m_mobileCoreImagePath = path;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000598C File Offset: 0x00003B8C
		public static MobileCoreImage Create(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException(path);
			}
			if (!LongPathFile.Exists(path))
			{
				throw new FileNotFoundException(string.Format("The specified file ({0}) is not a valid VHD image.", path));
			}
			FileInfo fileInfo = new FileInfo(path);
			MobileCoreImage result;
			if (fileInfo.Extension.Equals(".VHD", StringComparison.OrdinalIgnoreCase))
			{
				result = new MobileCoreVHD(path);
			}
			else
			{
				if (!fileInfo.Extension.Equals(".WIM", StringComparison.OrdinalIgnoreCase))
				{
					throw new ArgumentException(string.Format("The specified file ({0}) is not a valid VHD image.", path));
				}
				result = new MobileCoreWIM(path);
			}
			return result;
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00005A14 File Offset: 0x00003C14
		public string ImagePath
		{
			get
			{
				return this.m_mobileCoreImagePath;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00005A1C File Offset: 0x00003C1C
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00005A24 File Offset: 0x00003C24
		public bool IsMounted { get; protected set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00005A30 File Offset: 0x00003C30
		public ReadOnlyCollection<ImagePartition> Partitions
		{
			get
			{
				ReadOnlyCollection<ImagePartition> result = null;
				if (this.IsMounted)
				{
					result = new ReadOnlyCollection<ImagePartition>(this.m_partitions);
				}
				return result;
			}
		}

		// Token: 0x0600009B RID: 155
		public abstract void Mount();

		// Token: 0x0600009C RID: 156
		public abstract void MountReadOnly();

		// Token: 0x0600009D RID: 157
		public abstract void Unmount();

		// Token: 0x0600009E RID: 158 RVA: 0x00005A54 File Offset: 0x00003C54
		public ImagePartition GetPartition(MobileCorePartitionType type)
		{
			ImagePartition imagePartition = null;
			if (!this.IsMounted)
			{
				return null;
			}
			foreach (ImagePartition imagePartition2 in this.Partitions)
			{
				if (imagePartition2.Root != null && type == MobileCorePartitionType.System)
				{
					string path = Path.Combine(imagePartition2.Root, "Windows\\System32");
					if (LongPathDirectory.Exists(path))
					{
						imagePartition = imagePartition2;
					}
				}
			}
			if (imagePartition == null)
			{
				throw new IUException("Request partition {0} cannot be found in the image", new object[]
				{
					Enum.GetName(typeof(MobileCorePartitionType), type)
				});
			}
			return imagePartition;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00005B04 File Offset: 0x00003D04
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.IsMounted)
			{
				using (IEnumerator<ImagePartition> enumerator = this.Partitions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ImagePartition imagePartition = enumerator.Current;
						stringBuilder.AppendFormat("{0}, Root = {1}", imagePartition.Name, imagePartition.Root);
					}
					goto IL_5B;
				}
			}
			stringBuilder.AppendLine("This image is not mounted");
			IL_5B:
			return stringBuilder.ToString();
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00005B84 File Offset: 0x00003D84
		public AclCollection GetFileSystemACLs()
		{
			ImagePartition partition = this.GetPartition(MobileCorePartitionType.System);
			bool flag = false;
			if (!this.IsMounted)
			{
				this.Mount();
				flag = true;
			}
			AclCollection result = null;
			try
			{
				result = SecurityUtils.GetFileSystemACLs(partition.Root);
			}
			finally
			{
				if (flag)
				{
					this.Unmount();
				}
			}
			return result;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00005BD8 File Offset: 0x00003DD8
		public AclCollection GetRegistryACLs()
		{
			ImagePartition partition = this.GetPartition(MobileCorePartitionType.System);
			bool flag = false;
			if (!this.IsMounted)
			{
				this.Mount();
				flag = true;
			}
			AclCollection aclCollection = null;
			try
			{
				aclCollection = new AclCollection();
				string hiveRoot = Path.Combine(partition.Root, "Windows\\System32\\Config");
				aclCollection.UnionWith(SecurityUtils.GetRegistryACLs(hiveRoot));
			}
			finally
			{
				if (flag)
				{
					this.Unmount();
				}
			}
			return aclCollection;
		}

		// Token: 0x0400002E RID: 46
		private const string EXTENSION_VHD = ".VHD";

		// Token: 0x0400002F RID: 47
		private const string EXTENSION_WIM = ".WIM";

		// Token: 0x04000030 RID: 48
		private const string ERROR_IMAGENOTFOUND = "The specified file ({0}) either does not exist or cannot be read.";

		// Token: 0x04000031 RID: 49
		private const string ERROR_INVALIDIMAGE = "The specified file ({0}) is not a valid VHD image.";

		// Token: 0x04000032 RID: 50
		private const string STR_HIVE_PATH = "Windows\\System32\\Config";

		// Token: 0x04000033 RID: 51
		private const string ERROR_NO_SUCH_PARTITION = "Request partition {0} cannot be found in the image";

		// Token: 0x04000034 RID: 52
		private const string STR_SYSTEM32_DIR = "Windows\\System32";

		// Token: 0x04000035 RID: 53
		protected string m_mobileCoreImagePath;

		// Token: 0x04000036 RID: 54
		protected ImagePartitionCollection m_partitions = new ImagePartitionCollection();
	}
}
