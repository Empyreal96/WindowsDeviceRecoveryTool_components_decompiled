using System;

namespace System.Windows.Media.Animation
{
	/// <summary>Abstract class that, when implemented, defines an animation segment with its own target value and interpolation method for a <see cref="T:System.Windows.Media.Animation.ThicknessAnimationUsingKeyFrames" />. </summary>
	// Token: 0x0200018B RID: 395
	public abstract class ThicknessKeyFrame : Freezable, IKeyFrame
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" /> class.</summary>
		// Token: 0x06001718 RID: 5912 RVA: 0x000382C5 File Offset: 0x000364C5
		protected ThicknessKeyFrame()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" /> class that has the specified target <see cref="P:System.Windows.Media.Animation.ThicknessKeyFrame.Value" />.  </summary>
		/// <param name="value">The <see cref="P:System.Windows.Media.Animation.ThicknessKeyFrame.Value" /> of the new <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" /> instance.</param>
		// Token: 0x06001719 RID: 5913 RVA: 0x00071D01 File Offset: 0x0006FF01
		protected ThicknessKeyFrame(Thickness value) : this()
		{
			this.Value = value;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" /> class that has the specified target <see cref="P:System.Windows.Media.Animation.ThicknessKeyFrame.Value" /> and <see cref="P:System.Windows.Media.Animation.ThicknessKeyFrame.KeyTime" />.  </summary>
		/// <param name="value">The <see cref="P:System.Windows.Media.Animation.ThicknessKeyFrame.Value" /> of the new <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" /> instance.</param>
		/// <param name="keyTime">The <see cref="P:System.Windows.Media.Animation.ThicknessKeyFrame.KeyTime" /> of the new <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" /> instance.</param>
		// Token: 0x0600171A RID: 5914 RVA: 0x00071D10 File Offset: 0x0006FF10
		protected ThicknessKeyFrame(Thickness value, KeyTime keyTime) : this()
		{
			this.Value = value;
			this.KeyTime = keyTime;
		}

		/// <summary> Gets or sets the time at which the key frame's target <see cref="P:System.Windows.Media.Animation.ThicknessKeyFrame.Value" /> should be reached.  </summary>
		/// <returns>The time at which the key frame's current value should be equal to its <see cref="P:System.Windows.Media.Animation.ThicknessKeyFrame.Value" /> property. The default value is <see cref="P:System.Windows.Media.Animation.KeyTime.Uniform" />.</returns>
		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x0600171B RID: 5915 RVA: 0x00071D26 File Offset: 0x0006FF26
		// (set) Token: 0x0600171C RID: 5916 RVA: 0x00071D38 File Offset: 0x0006FF38
		public KeyTime KeyTime
		{
			get
			{
				return (KeyTime)base.GetValue(ThicknessKeyFrame.KeyTimeProperty);
			}
			set
			{
				base.SetValueInternal(ThicknessKeyFrame.KeyTimeProperty, value);
			}
		}

		/// <summary>Gets or sets the value associated with a <see cref="T:System.Windows.Media.Animation.KeyTime" /> instance. </summary>
		/// <returns>The current value for this property. </returns>
		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x0600171D RID: 5917 RVA: 0x00071D4B File Offset: 0x0006FF4B
		// (set) Token: 0x0600171E RID: 5918 RVA: 0x00071D58 File Offset: 0x0006FF58
		object IKeyFrame.Value
		{
			get
			{
				return this.Value;
			}
			set
			{
				this.Value = (Thickness)value;
			}
		}

		/// <summary> Gets or sets the key frame's target value.  </summary>
		/// <returns>The key frame's target value, which is the value of this key frame at its specified <see cref="P:System.Windows.Media.Animation.ThicknessKeyFrame.KeyTime" />. The default value is 0.</returns>
		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x0600171F RID: 5919 RVA: 0x00071D66 File Offset: 0x0006FF66
		// (set) Token: 0x06001720 RID: 5920 RVA: 0x00071D78 File Offset: 0x0006FF78
		public Thickness Value
		{
			get
			{
				return (Thickness)base.GetValue(ThicknessKeyFrame.ValueProperty);
			}
			set
			{
				base.SetValueInternal(ThicknessKeyFrame.ValueProperty, value);
			}
		}

		/// <summary>Returns the interpolated value of a specific key frame at the progress increment provided. </summary>
		/// <param name="baseValue">The value to animate from.</param>
		/// <param name="keyFrameProgress">A value between 0.0 and 1.0, inclusive, that specifies the percentage of time that has elapsed for this key frame.</param>
		/// <returns>The output value of this key frame given the specified base value and progress.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Occurs if <paramref name="keyFrameProgress" /> is not between 0.0 and 1.0, inclusive.</exception>
		// Token: 0x06001721 RID: 5921 RVA: 0x00071D8B File Offset: 0x0006FF8B
		public Thickness InterpolateValue(Thickness baseValue, double keyFrameProgress)
		{
			if (keyFrameProgress < 0.0 || keyFrameProgress > 1.0)
			{
				throw new ArgumentOutOfRangeException("keyFrameProgress");
			}
			return this.InterpolateValueCore(baseValue, keyFrameProgress);
		}

		/// <summary>Calculates the value of a key frame at the progress increment provided. </summary>
		/// <param name="baseValue">The value to animate from; typically the value of the previous key frame.</param>
		/// <param name="keyFrameProgress">A value between 0.0 and 1.0, inclusive, that specifies the percentage of time that has elapsed for this key frame.</param>
		/// <returns>The output value of this key frame given the specified base value and progress.</returns>
		// Token: 0x06001722 RID: 5922
		protected abstract Thickness InterpolateValueCore(Thickness baseValue, double keyFrameProgress);

		/// <summary>Identifies the <see cref="P:System.Windows.Media.Animation.ThicknessKeyFrame.KeyTime" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Media.Animation.ThicknessKeyFrame.KeyTime" /> dependency property.</returns>
		// Token: 0x040012AF RID: 4783
		public static readonly DependencyProperty KeyTimeProperty = DependencyProperty.Register("KeyTime", typeof(KeyTime), typeof(ThicknessKeyFrame), new PropertyMetadata(KeyTime.Uniform));

		/// <summary>Identifies the <see cref="P:System.Windows.Media.Animation.ThicknessKeyFrame.Value" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Media.Animation.ThicknessKeyFrame.Value" /> dependency property.</returns>
		// Token: 0x040012B0 RID: 4784
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(Thickness), typeof(ThicknessKeyFrame), new PropertyMetadata());
	}
}
