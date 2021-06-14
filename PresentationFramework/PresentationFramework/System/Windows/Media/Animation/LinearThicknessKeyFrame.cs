using System;
using MS.Internal.PresentationFramework;

namespace System.Windows.Media.Animation
{
	/// <summary>Animates from the <see cref="T:System.Windows.Thickness" /> value of the previous key frame to its own <see cref="P:System.Windows.Media.Animation.ThicknessKeyFrame.Value" /> using linear interpolation.  </summary>
	// Token: 0x0200018C RID: 396
	public class LinearThicknessKeyFrame : ThicknessKeyFrame
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Media.Animation.LinearThicknessKeyFrame" /> class.</summary>
		// Token: 0x06001724 RID: 5924 RVA: 0x00071CC9 File Offset: 0x0006FEC9
		public LinearThicknessKeyFrame()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Media.Animation.LinearThicknessKeyFrame" /> class with the specified ending value. </summary>
		/// <param name="value">Ending value (also known as "target value") for the key frame.</param>
		// Token: 0x06001725 RID: 5925 RVA: 0x00071CD1 File Offset: 0x0006FED1
		public LinearThicknessKeyFrame(Thickness value) : base(value)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Media.Animation.LinearThicknessKeyFrame" /> class with the specified ending value and key time.</summary>
		/// <param name="value">Ending value (also known as "target value") for the key frame.</param>
		/// <param name="keyTime">Key time for the key frame. The key time determines when the target value is reached which is also when the key frame ends.</param>
		// Token: 0x06001726 RID: 5926 RVA: 0x00071CDA File Offset: 0x0006FEDA
		public LinearThicknessKeyFrame(Thickness value, KeyTime keyTime) : base(value, keyTime)
		{
		}

		/// <summary>Creates a new instance of <see cref="T:System.Windows.Media.Animation.LinearThicknessKeyFrame" />.</summary>
		/// <returns>The new instance.</returns>
		// Token: 0x06001727 RID: 5927 RVA: 0x00071E1F File Offset: 0x0007001F
		protected override Freezable CreateInstanceCore()
		{
			return new LinearThicknessKeyFrame();
		}

		/// <summary>Interpolates, in a linear fashion, between the previous key frame value and the value of the current key frame, using the supplied progress increment. </summary>
		/// <param name="baseValue">The value to animate from.</param>
		/// <param name="keyFrameProgress">A value between 0.0 and 1.0, inclusive, that specifies the percentage of time that has elapsed for this key frame.</param>
		/// <returns>The output value of this key frame given the specified base value and progress.</returns>
		// Token: 0x06001728 RID: 5928 RVA: 0x00071E26 File Offset: 0x00070026
		protected override Thickness InterpolateValueCore(Thickness baseValue, double keyFrameProgress)
		{
			if (keyFrameProgress == 0.0)
			{
				return baseValue;
			}
			if (keyFrameProgress == 1.0)
			{
				return base.Value;
			}
			return AnimatedTypeHelpers.InterpolateThickness(baseValue, base.Value, keyFrameProgress);
		}
	}
}
