using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
	/// <summary>Defines a placeable metafile. Not inheritable.</summary>
	// Token: 0x020000B0 RID: 176
	[StructLayout(LayoutKind.Sequential)]
	public sealed class WmfPlaceableFileHeader
	{
		/// <summary>Gets or sets a value indicating the presence of a placeable metafile header.</summary>
		/// <returns>A value indicating presence of a placeable metafile header.</returns>
		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000A14 RID: 2580 RVA: 0x0002574F File Offset: 0x0002394F
		// (set) Token: 0x06000A15 RID: 2581 RVA: 0x00025757 File Offset: 0x00023957
		public int Key
		{
			get
			{
				return this.key;
			}
			set
			{
				this.key = value;
			}
		}

		/// <summary>Gets or sets the handle of the metafile in memory.</summary>
		/// <returns>The handle of the metafile in memory.</returns>
		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06000A16 RID: 2582 RVA: 0x00025760 File Offset: 0x00023960
		// (set) Token: 0x06000A17 RID: 2583 RVA: 0x00025768 File Offset: 0x00023968
		public short Hmf
		{
			get
			{
				return this.hmf;
			}
			set
			{
				this.hmf = value;
			}
		}

		/// <summary>Gets or sets the x-coordinate of the upper-left corner of the bounding rectangle of the metafile image on the output device.</summary>
		/// <returns>The x-coordinate of the upper-left corner of the bounding rectangle of the metafile image on the output device.</returns>
		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06000A18 RID: 2584 RVA: 0x00025771 File Offset: 0x00023971
		// (set) Token: 0x06000A19 RID: 2585 RVA: 0x00025779 File Offset: 0x00023979
		public short BboxLeft
		{
			get
			{
				return this.bboxLeft;
			}
			set
			{
				this.bboxLeft = value;
			}
		}

		/// <summary>Gets or sets the y-coordinate of the upper-left corner of the bounding rectangle of the metafile image on the output device.</summary>
		/// <returns>The y-coordinate of the upper-left corner of the bounding rectangle of the metafile image on the output device.</returns>
		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06000A1A RID: 2586 RVA: 0x00025782 File Offset: 0x00023982
		// (set) Token: 0x06000A1B RID: 2587 RVA: 0x0002578A File Offset: 0x0002398A
		public short BboxTop
		{
			get
			{
				return this.bboxTop;
			}
			set
			{
				this.bboxTop = value;
			}
		}

		/// <summary>Gets or sets the x-coordinate of the lower-right corner of the bounding rectangle of the metafile image on the output device.</summary>
		/// <returns>The x-coordinate of the lower-right corner of the bounding rectangle of the metafile image on the output device.</returns>
		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06000A1C RID: 2588 RVA: 0x00025793 File Offset: 0x00023993
		// (set) Token: 0x06000A1D RID: 2589 RVA: 0x0002579B File Offset: 0x0002399B
		public short BboxRight
		{
			get
			{
				return this.bboxRight;
			}
			set
			{
				this.bboxRight = value;
			}
		}

		/// <summary>Gets or sets the y-coordinate of the lower-right corner of the bounding rectangle of the metafile image on the output device.</summary>
		/// <returns>The y-coordinate of the lower-right corner of the bounding rectangle of the metafile image on the output device.</returns>
		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000A1E RID: 2590 RVA: 0x000257A4 File Offset: 0x000239A4
		// (set) Token: 0x06000A1F RID: 2591 RVA: 0x000257AC File Offset: 0x000239AC
		public short BboxBottom
		{
			get
			{
				return this.bboxBottom;
			}
			set
			{
				this.bboxBottom = value;
			}
		}

		/// <summary>Gets or sets the number of twips per inch.</summary>
		/// <returns>The number of twips per inch.</returns>
		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000A20 RID: 2592 RVA: 0x000257B5 File Offset: 0x000239B5
		// (set) Token: 0x06000A21 RID: 2593 RVA: 0x000257BD File Offset: 0x000239BD
		public short Inch
		{
			get
			{
				return this.inch;
			}
			set
			{
				this.inch = value;
			}
		}

		/// <summary>Reserved. Do not use.</summary>
		/// <returns>Reserved. Do not use.</returns>
		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000A22 RID: 2594 RVA: 0x000257C6 File Offset: 0x000239C6
		// (set) Token: 0x06000A23 RID: 2595 RVA: 0x000257CE File Offset: 0x000239CE
		public int Reserved
		{
			get
			{
				return this.reserved;
			}
			set
			{
				this.reserved = value;
			}
		}

		/// <summary>Gets or sets the checksum value for the previous ten <see langword="WORD" /> s in the header.</summary>
		/// <returns>The checksum value for the previous ten <see langword="WORD" /> s in the header.</returns>
		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000A24 RID: 2596 RVA: 0x000257D7 File Offset: 0x000239D7
		// (set) Token: 0x06000A25 RID: 2597 RVA: 0x000257DF File Offset: 0x000239DF
		public short Checksum
		{
			get
			{
				return this.checksum;
			}
			set
			{
				this.checksum = value;
			}
		}

		// Token: 0x04000950 RID: 2384
		private int key = -1698247209;

		// Token: 0x04000951 RID: 2385
		private short hmf;

		// Token: 0x04000952 RID: 2386
		private short bboxLeft;

		// Token: 0x04000953 RID: 2387
		private short bboxTop;

		// Token: 0x04000954 RID: 2388
		private short bboxRight;

		// Token: 0x04000955 RID: 2389
		private short bboxBottom;

		// Token: 0x04000956 RID: 2390
		private short inch;

		// Token: 0x04000957 RID: 2391
		private int reserved;

		// Token: 0x04000958 RID: 2392
		private short checksum;
	}
}
