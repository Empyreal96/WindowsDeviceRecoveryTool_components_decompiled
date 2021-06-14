using System;
using System.ComponentModel;

namespace ComponentAce.Compression.Archiver
{
	// Token: 0x020000A0 RID: 160
	[ToolboxItem(false)]
	public class SpanningOptions : Component
	{
		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600072E RID: 1838 RVA: 0x0002BFCE File Offset: 0x0002AFCE
		// (set) Token: 0x0600072F RID: 1839 RVA: 0x0002BFD6 File Offset: 0x0002AFD6
		[Description("Determines how the archive volumes will be named.")]
		public bool AdvancedNaming
		{
			get
			{
				return this.FAdvancedNaming;
			}
			set
			{
				this.FAdvancedNaming = value;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000730 RID: 1840 RVA: 0x0002BFDF File Offset: 0x0002AFDF
		// (set) Token: 0x06000731 RID: 1841 RVA: 0x0002BFE7 File Offset: 0x0002AFE7
		[Description("Specifies the size of first volume in bytes.")]
		public long FirstVolumeSize
		{
			get
			{
				return this.FFirstVolumeSize;
			}
			set
			{
				this.FFirstVolumeSize = value;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000732 RID: 1842 RVA: 0x0002BFF0 File Offset: 0x0002AFF0
		// (set) Token: 0x06000733 RID: 1843 RVA: 0x0002BFF8 File Offset: 0x0002AFF8
		[Description("Specifies the size of the volumes for archive splitting or spanning.")]
		public VolumeSize VolumeSize
		{
			get
			{
				return this.FVolumeSize;
			}
			set
			{
				this.FVolumeSize = value;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000734 RID: 1844 RVA: 0x0002C001 File Offset: 0x0002B001
		// (set) Token: 0x06000735 RID: 1845 RVA: 0x0002C009 File Offset: 0x0002B009
		[Description("Specifies the custom size of the volumes for archive splitting or spanning.")]
		public long CustomVolumeSize
		{
			get
			{
				return this.FCustomVolumeSize;
			}
			set
			{
				this.SetCustomVolumeSize(value);
			}
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0002C012 File Offset: 0x0002B012
		public SpanningOptions()
		{
			this.FAdvancedNaming = true;
			this.FFirstVolumeSize = 0L;
			this.FVolumeSize = VolumeSize.AutoDetect;
			this.FCustomVolumeSize = 0L;
			this.FSaveDirToFirstVolume = false;
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x0002C03F File Offset: 0x0002B03F
		internal void SetCustomVolumeSize(long value)
		{
			this.FCustomVolumeSize = ((value < 65536L) ? 65536L : value);
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0002C05C File Offset: 0x0002B05C
		public void Assign(object source)
		{
			if (source is SpanningOptions)
			{
				SpanningOptions spanningOptions = (SpanningOptions)source;
				this.FAdvancedNaming = spanningOptions.FAdvancedNaming;
				this.FFirstVolumeSize = spanningOptions.FFirstVolumeSize;
				this.FVolumeSize = spanningOptions.FVolumeSize;
				this.FCustomVolumeSize = spanningOptions.FCustomVolumeSize;
				this.FSaveDirToFirstVolume = spanningOptions.FSaveDirToFirstVolume;
			}
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0002C0B4 File Offset: 0x0002B0B4
		internal long GetVolumeSize(int volumeNumber)
		{
			if (volumeNumber == 0 && this.FFirstVolumeSize > 0L)
			{
				return this.FFirstVolumeSize;
			}
			switch (this.FVolumeSize)
			{
			case VolumeSize.AutoDetect:
				return -1L;
			case VolumeSize.Custom:
				return this.FCustomVolumeSize;
			case VolumeSize.Disk1_44MB:
				return 1457664L;
			case VolumeSize.Disk100MB:
				return 104857600L;
			case VolumeSize.Disk200MB:
				return 209715200L;
			case VolumeSize.Disk600MB:
				return 629145600L;
			case VolumeSize.Disk650MB:
				return 681574400L;
			case VolumeSize.Disk700MB:
				return 734003200L;
			case VolumeSize.Disk4700MB:
				return 4928307200L;
			default:
				return -1L;
			}
		}

		// Token: 0x040003DE RID: 990
		internal bool FAdvancedNaming;

		// Token: 0x040003DF RID: 991
		internal long FFirstVolumeSize;

		// Token: 0x040003E0 RID: 992
		internal VolumeSize FVolumeSize;

		// Token: 0x040003E1 RID: 993
		internal long FCustomVolumeSize;

		// Token: 0x040003E2 RID: 994
		internal bool FSaveDirToFirstVolume;
	}
}
