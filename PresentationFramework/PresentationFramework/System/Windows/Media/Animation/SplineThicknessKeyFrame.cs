using System;
using MS.Internal.PresentationFramework;

namespace System.Windows.Media.Animation
{
	/// <summary>Animates from the <see cref="T:System.Windows.Thickness" /> value of the previous key frame to its own <see cref="P:System.Windows.Media.Animation.ThicknessKeyFrame.Value" /> using splined interpolation.  </summary>
	// Token: 0x0200018D RID: 397
	public class SplineThicknessKeyFrame : ThicknessKeyFrame
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Media.Animation.SplineThicknessKeyFrame" /> class. </summary>
		// Token: 0x06001729 RID: 5929 RVA: 0x00071CC9 File Offset: 0x0006FEC9
		public SplineThicknessKeyFrame()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Media.Animation.SplineThicknessKeyFrame" /> class with the specified ending value.  </summary>
		/// <param name="value">Ending value (also known as "target value") for the key frame.</param>
		// Token: 0x0600172A RID: 5930 RVA: 0x00071E56 File Offset: 0x00070056
		public SplineThicknessKeyFrame(Thickness value) : this()
		{
			base.Value = value;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Media.Animation.SplineThicknessKeyFrame" /> class with the specified ending value and key time. </summary>
		/// <param name="value">Ending value (also known as "target value") for the key frame.</param>
		/// <param name="keyTime">Key time for the key frame. The key time determines when the target value is reached which is also when the key frame ends.</param>
		// Token: 0x0600172B RID: 5931 RVA: 0x00071E65 File Offset: 0x00070065
		public SplineThicknessKeyFrame(Thickness value, KeyTime keyTime) : this()
		{
			base.Value = value;
			base.KeyTime = keyTime;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Media.Animation.SplineThicknessKeyFrame" /> class with the specified ending value, key time, and <see cref="T:System.Windows.Media.Animation.KeySpline" />. </summary>
		/// <param name="value">Ending value (also known as "target value") for the key frame.</param>
		/// <param name="keyTime">Key time for the key frame. The key time determines when the target value is reached which is also when the key frame ends.</param>
		/// <param name="keySpline">
		///       <see cref="T:System.Windows.Media.Animation.KeySpline" /> for the key frame. The <see cref="T:System.Windows.Media.Animation.KeySpline" /> represents a Bezier curve which defines animation progress of the key frame.</param>
		// Token: 0x0600172C RID: 5932 RVA: 0x00071E7B File Offset: 0x0007007B
		public SplineThicknessKeyFrame(Thickness value, KeyTime keyTime, KeySpline keySpline) : this()
		{
			if (keySpline == null)
			{
				throw new ArgumentNullException("keySpline");
			}
			base.Value = value;
			base.KeyTime = keyTime;
			this.KeySpline = keySpline;
		}

		/// <summary>Creates a new instance of <see cref="T:System.Windows.Media.Animation.SplineThicknessKeyFrame" />.</summary>
		/// <returns>The new instance.</returns>
		// Token: 0x0600172D RID: 5933 RVA: 0x00071EA6 File Offset: 0x000700A6
		protected override Freezable CreateInstanceCore()
		{
			return new SplineThicknessKeyFrame();
		}

		/// <summary>Uses splined interpolation to transition between the previous key frame value and the value of the current key frame.</summary>
		/// <param name="baseValue">The value to animate from.</param>
		/// <param name="keyFrameProgress">A value between 0.0 and 1.0, inclusive, that specifies the percentage of time that has elapsed for this key frame.</param>
		/// <returns>The output value of this key frame given the specified base value and progress.</returns>
		// Token: 0x0600172E RID: 5934 RVA: 0x00071EB0 File Offset: 0x000700B0
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
			double splineProgress = this.KeySpline.GetSplineProgress(keyFrameProgress);
			return AnimatedTypeHelpers.InterpolateThickness(baseValue, base.Value, splineProgress);
		}

		/// <summary>Gets or sets the two control points that define animation progress for this key frame.  </summary>
		/// <returns>The two control points that specify the cubic  Bezier curve which defines the progress of the key frame.</returns>
		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x0600172F RID: 5935 RVA: 0x00071EF8 File Offset: 0x000700F8
		// (set) Token: 0x06001730 RID: 5936 RVA: 0x00071F0A File Offset: 0x0007010A
		public KeySpline KeySpline
		{
			get
			{
				return (KeySpline)base.GetValue(SplineThicknessKeyFrame.KeySplineProperty);
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				base.SetValue(SplineThicknessKeyFrame.KeySplineProperty, value);
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Media.Animation.SplineThicknessKeyFrame.KeySpline" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Media.Animation.SplineThicknessKeyFrame.KeySpline" /> dependency property.</returns>
		// Token: 0x040012B1 RID: 4785
		public static readonly DependencyProperty KeySplineProperty = DependencyProperty.Register("KeySpline", typeof(KeySpline), typeof(SplineThicknessKeyFrame), new PropertyMetadata(new KeySpline()));
	}
}
