using System;

namespace System.Drawing.Drawing2D
{
	/// <summary>Contains the graphical data that makes up a <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> object. This class cannot be inherited.</summary>
	// Token: 0x020000CC RID: 204
	public sealed class PathData
	{
		/// <summary>Gets or sets an array of <see cref="T:System.Drawing.PointF" /> structures that represents the points through which the path is constructed.</summary>
		/// <returns>An array of <see cref="T:System.Drawing.PointF" /> objects that represents the points through which the path is constructed.</returns>
		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06000B33 RID: 2867 RVA: 0x00029207 File Offset: 0x00027407
		// (set) Token: 0x06000B34 RID: 2868 RVA: 0x0002920F File Offset: 0x0002740F
		public PointF[] Points
		{
			get
			{
				return this.points;
			}
			set
			{
				this.points = value;
			}
		}

		/// <summary>Gets or sets the types of the corresponding points in the path.</summary>
		/// <returns>An array of bytes that specify the types of the corresponding points in the path.</returns>
		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000B35 RID: 2869 RVA: 0x00029218 File Offset: 0x00027418
		// (set) Token: 0x06000B36 RID: 2870 RVA: 0x00029220 File Offset: 0x00027420
		public byte[] Types
		{
			get
			{
				return this.types;
			}
			set
			{
				this.types = value;
			}
		}

		// Token: 0x040009F0 RID: 2544
		private PointF[] points;

		// Token: 0x040009F1 RID: 2545
		private byte[] types;
	}
}
