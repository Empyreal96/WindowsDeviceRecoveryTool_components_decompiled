using System;

namespace System.Drawing.Drawing2D
{
	/// <summary>Defines a blend pattern for a <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" /> object. This class cannot be inherited.</summary>
	// Token: 0x020000B3 RID: 179
	public sealed class Blend
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.Blend" /> class.</summary>
		// Token: 0x06000A3A RID: 2618 RVA: 0x00025A18 File Offset: 0x00023C18
		public Blend()
		{
			this.factors = new float[1];
			this.positions = new float[1];
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.Blend" /> class with the specified number of factors and positions.</summary>
		/// <param name="count">The number of elements in the <see cref="P:System.Drawing.Drawing2D.Blend.Factors" /> and <see cref="P:System.Drawing.Drawing2D.Blend.Positions" /> arrays. </param>
		// Token: 0x06000A3B RID: 2619 RVA: 0x00025A38 File Offset: 0x00023C38
		public Blend(int count)
		{
			this.factors = new float[count];
			this.positions = new float[count];
		}

		/// <summary>Gets or sets an array of blend factors for the gradient.</summary>
		/// <returns>An array of blend factors that specify the percentages of the starting color and the ending color to be used at the corresponding position.</returns>
		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06000A3C RID: 2620 RVA: 0x00025A58 File Offset: 0x00023C58
		// (set) Token: 0x06000A3D RID: 2621 RVA: 0x00025A60 File Offset: 0x00023C60
		public float[] Factors
		{
			get
			{
				return this.factors;
			}
			set
			{
				this.factors = value;
			}
		}

		/// <summary>Gets or sets an array of blend positions for the gradient.</summary>
		/// <returns>An array of blend positions that specify the percentages of distance along the gradient line.</returns>
		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06000A3E RID: 2622 RVA: 0x00025A69 File Offset: 0x00023C69
		// (set) Token: 0x06000A3F RID: 2623 RVA: 0x00025A71 File Offset: 0x00023C71
		public float[] Positions
		{
			get
			{
				return this.positions;
			}
			set
			{
				this.positions = value;
			}
		}

		// Token: 0x0400095D RID: 2397
		private float[] factors;

		// Token: 0x0400095E RID: 2398
		private float[] positions;
	}
}
