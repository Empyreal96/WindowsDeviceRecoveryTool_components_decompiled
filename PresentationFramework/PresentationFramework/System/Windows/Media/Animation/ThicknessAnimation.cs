using System;
using System.Globalization;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;

namespace System.Windows.Media.Animation
{
	/// <summary> Animates the value of a  <see cref="T:System.Windows.Thickness" /> property between two target values using      linear interpolation over a specified <see cref="P:System.Windows.Media.Animation.Timeline.Duration" />. </summary>
	// Token: 0x0200018F RID: 399
	public class ThicknessAnimation : ThicknessAnimationBase
	{
		// Token: 0x0600173B RID: 5947 RVA: 0x00072034 File Offset: 0x00070234
		static ThicknessAnimation()
		{
			Type typeFromHandle = typeof(Thickness?);
			Type typeFromHandle2 = typeof(ThicknessAnimation);
			PropertyChangedCallback propertyChangedCallback = new PropertyChangedCallback(ThicknessAnimation.AnimationFunction_Changed);
			ValidateValueCallback validateValueCallback = new ValidateValueCallback(ThicknessAnimation.ValidateFromToOrByValue);
			ThicknessAnimation.FromProperty = DependencyProperty.Register("From", typeFromHandle, typeFromHandle2, new PropertyMetadata(null, propertyChangedCallback), validateValueCallback);
			ThicknessAnimation.ToProperty = DependencyProperty.Register("To", typeFromHandle, typeFromHandle2, new PropertyMetadata(null, propertyChangedCallback), validateValueCallback);
			ThicknessAnimation.ByProperty = DependencyProperty.Register("By", typeFromHandle, typeFromHandle2, new PropertyMetadata(null, propertyChangedCallback), validateValueCallback);
			ThicknessAnimation.EasingFunctionProperty = DependencyProperty.Register("EasingFunction", typeof(IEasingFunction), typeFromHandle2);
		}

		/// <summary> Initializes a new instance of the <see cref="T:System.Windows.Media.Animation.ThicknessAnimation" /> class. </summary>
		// Token: 0x0600173C RID: 5948 RVA: 0x000720D6 File Offset: 0x000702D6
		public ThicknessAnimation()
		{
		}

		/// <summary> Initializes a new instance of the <see cref="T:System.Windows.Media.Animation.ThicknessAnimation" /> class that animates to the specified value over the specified duration. The starting value for the animation is the base value of the property being animated or the output from another animation. </summary>
		/// <param name="toValue">The destination value of the animation. </param>
		/// <param name="duration">The length of time the animation takes to play from start to finish, once. See the <see cref="P:System.Windows.Media.Animation.Timeline.Duration" /> property for more information.</param>
		// Token: 0x0600173D RID: 5949 RVA: 0x000720DE File Offset: 0x000702DE
		public ThicknessAnimation(Thickness toValue, Duration duration) : this()
		{
			this.To = new Thickness?(toValue);
			base.Duration = duration;
		}

		/// <summary> Initializes a new instance of the <see cref="T:System.Windows.Media.Animation.ThicknessAnimation" /> class that animates to the specified value over the specified duration and has the specified fill behavior. The starting value for the animation is the base value of the property being animated or the output from another animation. </summary>
		/// <param name="toValue">The destination value of the animation. </param>
		/// <param name="duration">The length of time the animation takes to play from start to finish, once. See the <see cref="P:System.Windows.Media.Animation.Timeline.Duration" /> property for more information.</param>
		/// <param name="fillBehavior">Specifies how the animation behaves when it is not active.</param>
		// Token: 0x0600173E RID: 5950 RVA: 0x000720F9 File Offset: 0x000702F9
		public ThicknessAnimation(Thickness toValue, Duration duration, FillBehavior fillBehavior) : this()
		{
			this.To = new Thickness?(toValue);
			base.Duration = duration;
			base.FillBehavior = fillBehavior;
		}

		/// <summary> Initializes a new instance of the <see cref="T:System.Windows.Media.Animation.ThicknessAnimation" /> class that animates from the specified starting value to the specified destination value over the specified duration. </summary>
		/// <param name="fromValue">The starting value of the animation.</param>
		/// <param name="toValue">The destination value of the animation. </param>
		/// <param name="duration">The length of time the animation takes to play from start to finish, once. See the <see cref="P:System.Windows.Media.Animation.Timeline.Duration" /> property for more information.</param>
		// Token: 0x0600173F RID: 5951 RVA: 0x0007211B File Offset: 0x0007031B
		public ThicknessAnimation(Thickness fromValue, Thickness toValue, Duration duration) : this()
		{
			this.From = new Thickness?(fromValue);
			this.To = new Thickness?(toValue);
			base.Duration = duration;
		}

		/// <summary> Initializes a new instance of the <see cref="T:System.Windows.Media.Animation.ThicknessAnimation" /> class that animates from the specified starting value to the specified destination value over the specified duration and has the specified fill behavior. </summary>
		/// <param name="fromValue">The starting value of the animation.</param>
		/// <param name="toValue">The destination value of the animation. </param>
		/// <param name="duration">The length of time the animation takes to play from start to finish, once. See the <see cref="P:System.Windows.Media.Animation.Timeline.Duration" /> property for more information.</param>
		/// <param name="fillBehavior">Specifies how the animation behaves when it is not active.</param>
		// Token: 0x06001740 RID: 5952 RVA: 0x00072142 File Offset: 0x00070342
		public ThicknessAnimation(Thickness fromValue, Thickness toValue, Duration duration, FillBehavior fillBehavior) : this()
		{
			this.From = new Thickness?(fromValue);
			this.To = new Thickness?(toValue);
			base.Duration = duration;
			base.FillBehavior = fillBehavior;
		}

		/// <summary>Creates a modifiable clone of this <see cref="T:System.Windows.Media.Animation.ThicknessAnimation" />, making deep copies of this object's values. When copying dependency properties, this method copies resource references and data bindings (but they might no longer resolve) but not animations or their current values.</summary>
		/// <returns>A modifiable clone of the current object. The cloned object's <see cref="P:System.Windows.Freezable.IsFrozen" /> property will be <see langword="false" /> even if the source's <see cref="P:System.Windows.Freezable.IsFrozen" /> property was <see langword="true." /></returns>
		// Token: 0x06001741 RID: 5953 RVA: 0x00072171 File Offset: 0x00070371
		public new ThicknessAnimation Clone()
		{
			return (ThicknessAnimation)base.Clone();
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Windows.Media.Animation.ThicknessAnimation" />.          </summary>
		/// <returns>The new instance.</returns>
		// Token: 0x06001742 RID: 5954 RVA: 0x0007217E File Offset: 0x0007037E
		protected override Freezable CreateInstanceCore()
		{
			return new ThicknessAnimation();
		}

		/// <summary>Calculates a value that represents the current value of the property being animated, as determined by the <see cref="T:System.Windows.Media.Animation.ThicknessAnimation" />. </summary>
		/// <param name="defaultOriginValue">The suggested origin value, used if the animation does not have its own explicitly set start value.</param>
		/// <param name="defaultDestinationValue">The suggested destination value, used if the animation does not have its own explicitly set end value.</param>
		/// <param name="animationClock">An <see cref="T:System.Windows.Media.Animation.AnimationClock" /> that generates the <see cref="P:System.Windows.Media.Animation.Clock.CurrentTime" /> or <see cref="P:System.Windows.Media.Animation.Clock.CurrentProgress" /> used by the animation.</param>
		/// <returns>The calculated value of the property, as determined by the current animation.</returns>
		// Token: 0x06001743 RID: 5955 RVA: 0x00072188 File Offset: 0x00070388
		protected override Thickness GetCurrentValueCore(Thickness defaultOriginValue, Thickness defaultDestinationValue, AnimationClock animationClock)
		{
			if (!this._isAnimationFunctionValid)
			{
				this.ValidateAnimationFunction();
			}
			double num = animationClock.CurrentProgress.Value;
			IEasingFunction easingFunction = this.EasingFunction;
			if (easingFunction != null)
			{
				num = easingFunction.Ease(num);
			}
			Thickness thickness = default(Thickness);
			Thickness thickness2 = default(Thickness);
			Thickness value = default(Thickness);
			Thickness value2 = default(Thickness);
			bool flag = false;
			bool flag2 = false;
			switch (this._animationType)
			{
			case AnimationType.Automatic:
				thickness = defaultOriginValue;
				thickness2 = defaultDestinationValue;
				flag = true;
				flag2 = true;
				break;
			case AnimationType.From:
				thickness = this._keyValues[0];
				thickness2 = defaultDestinationValue;
				flag2 = true;
				break;
			case AnimationType.To:
				thickness = defaultOriginValue;
				thickness2 = this._keyValues[0];
				flag = true;
				break;
			case AnimationType.By:
				thickness2 = this._keyValues[0];
				value2 = defaultOriginValue;
				flag = true;
				break;
			case AnimationType.FromTo:
				thickness = this._keyValues[0];
				thickness2 = this._keyValues[1];
				if (this.IsAdditive)
				{
					value2 = defaultOriginValue;
					flag = true;
				}
				break;
			case AnimationType.FromBy:
				thickness = this._keyValues[0];
				thickness2 = AnimatedTypeHelpers.AddThickness(this._keyValues[0], this._keyValues[1]);
				if (this.IsAdditive)
				{
					value2 = defaultOriginValue;
					flag = true;
				}
				break;
			}
			if (flag && !AnimatedTypeHelpers.IsValidAnimationValueThickness(defaultOriginValue))
			{
				throw new InvalidOperationException(SR.Get("Animation_Invalid_DefaultValue", new object[]
				{
					base.GetType(),
					"origin",
					defaultOriginValue.ToString(CultureInfo.InvariantCulture)
				}));
			}
			if (flag2 && !AnimatedTypeHelpers.IsValidAnimationValueThickness(defaultDestinationValue))
			{
				throw new InvalidOperationException(SR.Get("Animation_Invalid_DefaultValue", new object[]
				{
					base.GetType(),
					"destination",
					defaultDestinationValue.ToString(CultureInfo.InvariantCulture)
				}));
			}
			if (this.IsCumulative)
			{
				double num2 = (double)(animationClock.CurrentIteration - 1).Value;
				if (num2 > 0.0)
				{
					Thickness value3 = AnimatedTypeHelpers.SubtractThickness(thickness2, thickness);
					value = AnimatedTypeHelpers.ScaleThickness(value3, num2);
				}
			}
			return AnimatedTypeHelpers.AddThickness(value2, AnimatedTypeHelpers.AddThickness(value, AnimatedTypeHelpers.InterpolateThickness(thickness, thickness2, num)));
		}

		// Token: 0x06001744 RID: 5956 RVA: 0x000723C4 File Offset: 0x000705C4
		private void ValidateAnimationFunction()
		{
			this._animationType = AnimationType.Automatic;
			this._keyValues = null;
			if (this.From != null)
			{
				if (this.To != null)
				{
					this._animationType = AnimationType.FromTo;
					this._keyValues = new Thickness[2];
					this._keyValues[0] = this.From.Value;
					this._keyValues[1] = this.To.Value;
				}
				else if (this.By != null)
				{
					this._animationType = AnimationType.FromBy;
					this._keyValues = new Thickness[2];
					this._keyValues[0] = this.From.Value;
					this._keyValues[1] = this.By.Value;
				}
				else
				{
					this._animationType = AnimationType.From;
					this._keyValues = new Thickness[1];
					this._keyValues[0] = this.From.Value;
				}
			}
			else if (this.To != null)
			{
				this._animationType = AnimationType.To;
				this._keyValues = new Thickness[1];
				this._keyValues[0] = this.To.Value;
			}
			else if (this.By != null)
			{
				this._animationType = AnimationType.By;
				this._keyValues = new Thickness[1];
				this._keyValues[0] = this.By.Value;
			}
			this._isAnimationFunctionValid = true;
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x0007255C File Offset: 0x0007075C
		private static void AnimationFunction_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ThicknessAnimation thicknessAnimation = (ThicknessAnimation)d;
			thicknessAnimation._isAnimationFunctionValid = false;
			thicknessAnimation.PropertyChanged(e.Property);
		}

		// Token: 0x06001746 RID: 5958 RVA: 0x00072584 File Offset: 0x00070784
		private static bool ValidateFromToOrByValue(object value)
		{
			Thickness? thickness = (Thickness?)value;
			return thickness == null || AnimatedTypeHelpers.IsValidAnimationValueThickness(thickness.Value);
		}

		/// <summary>  Gets or sets the animation's starting value.  </summary>
		/// <returns>The starting value of the animation. The default value is null.</returns>
		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06001747 RID: 5959 RVA: 0x000725AF File Offset: 0x000707AF
		// (set) Token: 0x06001748 RID: 5960 RVA: 0x000725C1 File Offset: 0x000707C1
		public Thickness? From
		{
			get
			{
				return (Thickness?)base.GetValue(ThicknessAnimation.FromProperty);
			}
			set
			{
				base.SetValueInternal(ThicknessAnimation.FromProperty, value);
			}
		}

		/// <summary> Gets or sets the animation's ending value.  </summary>
		/// <returns>The ending value of the animation. The default value is null.</returns>
		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06001749 RID: 5961 RVA: 0x000725D4 File Offset: 0x000707D4
		// (set) Token: 0x0600174A RID: 5962 RVA: 0x000725E6 File Offset: 0x000707E6
		public Thickness? To
		{
			get
			{
				return (Thickness?)base.GetValue(ThicknessAnimation.ToProperty);
			}
			set
			{
				base.SetValueInternal(ThicknessAnimation.ToProperty, value);
			}
		}

		/// <summary> Gets or sets the total amount by which the animation changes its starting value.  </summary>
		/// <returns>The total amount by which the animation changes its starting value.     The default value is null.</returns>
		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x0600174B RID: 5963 RVA: 0x000725F9 File Offset: 0x000707F9
		// (set) Token: 0x0600174C RID: 5964 RVA: 0x0007260B File Offset: 0x0007080B
		public Thickness? By
		{
			get
			{
				return (Thickness?)base.GetValue(ThicknessAnimation.ByProperty);
			}
			set
			{
				base.SetValueInternal(ThicknessAnimation.ByProperty, value);
			}
		}

		/// <summary>Gets or sets the easing function applied to this animation.</summary>
		/// <returns>The easing function applied to this animation.</returns>
		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x0600174D RID: 5965 RVA: 0x0007261E File Offset: 0x0007081E
		// (set) Token: 0x0600174E RID: 5966 RVA: 0x00072630 File Offset: 0x00070830
		public IEasingFunction EasingFunction
		{
			get
			{
				return (IEasingFunction)base.GetValue(ThicknessAnimation.EasingFunctionProperty);
			}
			set
			{
				base.SetValueInternal(ThicknessAnimation.EasingFunctionProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the target property's current value should be added to this animation's starting value.  </summary>
		/// <returns>
		///     <see langword="true" /> if the target property's current value should be added to this animation's starting value; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x0600174F RID: 5967 RVA: 0x0007263E File Offset: 0x0007083E
		// (set) Token: 0x06001750 RID: 5968 RVA: 0x00072650 File Offset: 0x00070850
		public bool IsAdditive
		{
			get
			{
				return (bool)base.GetValue(AnimationTimeline.IsAdditiveProperty);
			}
			set
			{
				base.SetValueInternal(AnimationTimeline.IsAdditiveProperty, BooleanBoxes.Box(value));
			}
		}

		/// <summary> Gets or sets a value that specifies whether the animation's value accumulates when it repeats.  </summary>
		/// <returns>
		///     true if the animation accumulates its values when its <see cref="P:System.Windows.Media.Animation.Timeline.RepeatBehavior" /> property causes it to repeat its simple duration. otherwise, false. The default value is false.</returns>
		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06001751 RID: 5969 RVA: 0x00072663 File Offset: 0x00070863
		// (set) Token: 0x06001752 RID: 5970 RVA: 0x00072675 File Offset: 0x00070875
		public bool IsCumulative
		{
			get
			{
				return (bool)base.GetValue(AnimationTimeline.IsCumulativeProperty);
			}
			set
			{
				base.SetValueInternal(AnimationTimeline.IsCumulativeProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x040012B3 RID: 4787
		private Thickness[] _keyValues;

		// Token: 0x040012B4 RID: 4788
		private AnimationType _animationType;

		// Token: 0x040012B5 RID: 4789
		private bool _isAnimationFunctionValid;

		/// <summary> Identifies the <see cref="P:System.Windows.Media.Animation.ThicknessAnimation.From" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Media.Animation.ThicknessAnimation.From" /> dependency property.</returns>
		// Token: 0x040012B6 RID: 4790
		public static readonly DependencyProperty FromProperty;

		/// <summary> Identifies the <see cref="P:System.Windows.Media.Animation.ThicknessAnimation.To" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Media.Animation.ThicknessAnimation.To" /> dependency property.</returns>
		// Token: 0x040012B7 RID: 4791
		public static readonly DependencyProperty ToProperty;

		/// <summary> Identifies the <see cref="P:System.Windows.Media.Animation.ThicknessAnimation.By" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Media.Animation.ThicknessAnimation.By" /> dependency property.</returns>
		// Token: 0x040012B8 RID: 4792
		public static readonly DependencyProperty ByProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Media.Animation.ThicknessAnimation.EasingFunction" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Media.Animation.ThicknessAnimation.EasingFunction" /> dependency property.</returns>
		// Token: 0x040012B9 RID: 4793
		public static readonly DependencyProperty EasingFunctionProperty;
	}
}
