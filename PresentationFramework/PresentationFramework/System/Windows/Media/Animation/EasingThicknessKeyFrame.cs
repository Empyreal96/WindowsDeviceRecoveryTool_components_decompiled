using System;
using MS.Internal.PresentationFramework;

namespace System.Windows.Media.Animation
{
	/// <summary>A class that enables you to associate easing functions with a <see cref="T:System.Windows.Media.Animation.ThicknessAnimationUsingKeyFrames" /> key frame animation.</summary>
	// Token: 0x0200018E RID: 398
	public class EasingThicknessKeyFrame : ThicknessKeyFrame
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Media.Animation.EasingThicknessKeyFrame" /> class. </summary>
		// Token: 0x06001732 RID: 5938 RVA: 0x00071CC9 File Offset: 0x0006FEC9
		public EasingThicknessKeyFrame()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Media.Animation.EasingThicknessKeyFrame" /> class with the specified <see cref="T:System.Windows.Thickness" /> value. </summary>
		/// <param name="value">The initial <see cref="T:System.Windows.Thickness" /> value.</param>
		// Token: 0x06001733 RID: 5939 RVA: 0x00071F55 File Offset: 0x00070155
		public EasingThicknessKeyFrame(Thickness value) : this()
		{
			base.Value = value;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Media.Animation.EasingThicknessKeyFrame" /> class with the specified <see cref="T:System.Windows.Thickness" /> value and key time. </summary>
		/// <param name="value">The initial <see cref="T:System.Windows.Thickness" /> value.</param>
		/// <param name="keyTime">The initial key time.</param>
		// Token: 0x06001734 RID: 5940 RVA: 0x00071F64 File Offset: 0x00070164
		public EasingThicknessKeyFrame(Thickness value, KeyTime keyTime) : this()
		{
			base.Value = value;
			base.KeyTime = keyTime;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Media.Animation.EasingThicknessKeyFrame" /> class with the specified <see cref="T:System.Windows.Thickness" /> value, key time, and easing function. </summary>
		/// <param name="value">The initial <see cref="T:System.Windows.Thickness" /> value.</param>
		/// <param name="keyTime">The initial key time.</param>
		/// <param name="easingFunction">The easing function.</param>
		// Token: 0x06001735 RID: 5941 RVA: 0x00071F7A File Offset: 0x0007017A
		public EasingThicknessKeyFrame(Thickness value, KeyTime keyTime, IEasingFunction easingFunction) : this()
		{
			base.Value = value;
			base.KeyTime = keyTime;
			this.EasingFunction = easingFunction;
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Windows.Freezable" /> derived class. When creating a derived class, you must override this method.</summary>
		/// <returns>The new instance.</returns>
		// Token: 0x06001736 RID: 5942 RVA: 0x00071F97 File Offset: 0x00070197
		protected override Freezable CreateInstanceCore()
		{
			return new EasingThicknessKeyFrame();
		}

		/// <summary>Interpolates, according to the easing function used, between the previous key frame value and the value of the current key frame, using the supplied progress increment.</summary>
		/// <param name="baseValue"> The value to animate from.</param>
		/// <param name="keyFrameProgress"> A value between 0.0 and 1.0, inclusive, that specifies the percentage of time that has elapsed for this key frame.</param>
		/// <returns>The output value of this key frame given the specified base value and progress.</returns>
		// Token: 0x06001737 RID: 5943 RVA: 0x00071FA0 File Offset: 0x000701A0
		protected override Thickness InterpolateValueCore(Thickness baseValue, double keyFrameProgress)
		{
			IEasingFunction easingFunction = this.EasingFunction;
			if (easingFunction != null)
			{
				keyFrameProgress = easingFunction.Ease(keyFrameProgress);
			}
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

		/// <summary>Gets or sets the easing function applied to the key frame.</summary>
		/// <returns>The easing function applied to the key frame.</returns>
		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06001738 RID: 5944 RVA: 0x00071FEE File Offset: 0x000701EE
		// (set) Token: 0x06001739 RID: 5945 RVA: 0x00072000 File Offset: 0x00070200
		public IEasingFunction EasingFunction
		{
			get
			{
				return (IEasingFunction)base.GetValue(EasingThicknessKeyFrame.EasingFunctionProperty);
			}
			set
			{
				base.SetValueInternal(EasingThicknessKeyFrame.EasingFunctionProperty, value);
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Media.Animation.EasingThicknessKeyFrame.EasingFunction" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Media.Animation.EasingThicknessKeyFrame.EasingFunction" /> dependency property.</returns>
		// Token: 0x040012B2 RID: 4786
		public static readonly DependencyProperty EasingFunctionProperty = DependencyProperty.Register("EasingFunction", typeof(IEasingFunction), typeof(EasingThicknessKeyFrame));
	}
}
