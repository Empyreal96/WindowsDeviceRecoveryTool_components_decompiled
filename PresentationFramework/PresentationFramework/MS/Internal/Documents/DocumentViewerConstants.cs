using System;

namespace MS.Internal.Documents
{
	// Token: 0x020006C2 RID: 1730
	internal static class DocumentViewerConstants
	{
		// Token: 0x17001A8C RID: 6796
		// (get) Token: 0x06006FCF RID: 28623 RVA: 0x002021DE File Offset: 0x002003DE
		public static double MinimumZoom
		{
			get
			{
				return 5.0;
			}
		}

		// Token: 0x17001A8D RID: 6797
		// (get) Token: 0x06006FD0 RID: 28624 RVA: 0x002021E9 File Offset: 0x002003E9
		public static double MaximumZoom
		{
			get
			{
				return 5000.0;
			}
		}

		// Token: 0x17001A8E RID: 6798
		// (get) Token: 0x06006FD1 RID: 28625 RVA: 0x002021F4 File Offset: 0x002003F4
		public static double MinimumScale
		{
			get
			{
				return 0.05;
			}
		}

		// Token: 0x17001A8F RID: 6799
		// (get) Token: 0x06006FD2 RID: 28626 RVA: 0x002021FF File Offset: 0x002003FF
		public static double MinimumThumbnailsScale
		{
			get
			{
				return 0.125;
			}
		}

		// Token: 0x17001A90 RID: 6800
		// (get) Token: 0x06006FD3 RID: 28627 RVA: 0x0020220A File Offset: 0x0020040A
		public static double MaximumScale
		{
			get
			{
				return 50.0;
			}
		}

		// Token: 0x17001A91 RID: 6801
		// (get) Token: 0x06006FD4 RID: 28628 RVA: 0x00096AE4 File Offset: 0x00094CE4
		public static int MaximumMaxPagesAcross
		{
			get
			{
				return 32;
			}
		}

		// Token: 0x040036D2 RID: 14034
		private const double _minimumZoom = 5.0;

		// Token: 0x040036D3 RID: 14035
		private const double _minimumThumbnailsZoom = 12.5;

		// Token: 0x040036D4 RID: 14036
		private const double _maximumZoom = 5000.0;

		// Token: 0x040036D5 RID: 14037
		private const int _maximumMaxPagesAcross = 32;
	}
}
