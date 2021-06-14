using System;
using System.Globalization;

namespace ClickerUtilityLibrary.Misc
{
	// Token: 0x02000009 RID: 9
	public class ImageVersion
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000042 RID: 66 RVA: 0x0000482F File Offset: 0x00002A2F
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00004837 File Offset: 0x00002A37
		public byte Major { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00004840 File Offset: 0x00002A40
		// (set) Token: 0x06000045 RID: 69 RVA: 0x00004848 File Offset: 0x00002A48
		public byte Minor { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00004851 File Offset: 0x00002A51
		// (set) Token: 0x06000047 RID: 71 RVA: 0x00004859 File Offset: 0x00002A59
		public short BuildNumber { get; private set; }

		// Token: 0x06000048 RID: 72 RVA: 0x00004862 File Offset: 0x00002A62
		public ImageVersion(byte major, byte minor, short buildNumber)
		{
			this.Major = major;
			this.Minor = minor;
			this.BuildNumber = buildNumber;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00004884 File Offset: 0x00002A84
		public ImageVersion()
		{
			this.Major = 0;
			this.Minor = 0;
			this.BuildNumber = 0;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000048A6 File Offset: 0x00002AA6
		public ImageVersion(int version)
		{
			this.BuildNumber = (short)(version & 65535);
			this.Minor = (byte)(version >> 16 & 255);
			this.Major = (byte)(version >> 24 & 255);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000048E4 File Offset: 0x00002AE4
		public int ToInt32()
		{
			int num = (int)this.BuildNumber;
			num |= (int)this.Minor << 16;
			return num | (int)this.Major << 24;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00004918 File Offset: 0x00002B18
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}", new object[]
			{
				this.Major,
				this.Minor,
				this.BuildNumber
			});
		}
	}
}
