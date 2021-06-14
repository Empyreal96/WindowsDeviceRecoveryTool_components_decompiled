using System;

namespace System.Windows.Media.Animation
{
	/// <summary>Abstract class that, when implemented, animates a <see cref="T:System.Windows.Thickness" /> value. </summary>
	// Token: 0x02000190 RID: 400
	public abstract class ThicknessAnimationBase : AnimationTimeline
	{
		/// <summary>Creates a modifiable clone of this <see cref="T:System.Windows.Media.Animation.ThicknessAnimationBase" />, making deep copies of this object's values. When copying dependency properties, this method copies resource references and data bindings (but they might no longer resolve) but not animations or their current values.</summary>
		/// <returns>A modifiable clone of the current object. The cloned object's <see cref="P:System.Windows.Freezable.IsFrozen" /> property will be <see langword="false" /> even if the source's <see cref="P:System.Windows.Freezable.IsFrozen" /> property was <see langword="true." /></returns>
		// Token: 0x06001754 RID: 5972 RVA: 0x00072690 File Offset: 0x00070890
		public new ThicknessAnimationBase Clone()
		{
			return (ThicknessAnimationBase)base.Clone();
		}

		/// <summary>Gets the current value of the animation.</summary>
		/// <param name="defaultOriginValue">The origin value provided to the animation if the animation does not have its own start value. </param>
		/// <param name="defaultDestinationValue">The destination value provided to the animation if the animation does not have its own destination value.</param>
		/// <param name="animationClock">The <see cref="T:System.Windows.Media.Animation.AnimationClock" /> which can generate the <see cref="P:System.Windows.Media.Animation.Clock.CurrentTime" /> or <see cref="P:System.Windows.Media.Animation.Clock.CurrentProgress" /> value to be used by the animation to generate its output value.</param>
		/// <returns>The current value of the animation.</returns>
		// Token: 0x06001755 RID: 5973 RVA: 0x0007269D File Offset: 0x0007089D
		public sealed override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
		{
			if (defaultOriginValue == null)
			{
				throw new ArgumentNullException("defaultOriginValue");
			}
			if (defaultDestinationValue == null)
			{
				throw new ArgumentNullException("defaultDestinationValue");
			}
			return this.GetCurrentValue((Thickness)defaultOriginValue, (Thickness)defaultDestinationValue, animationClock);
		}

		/// <summary>Gets the type of value this animation generates.</summary>
		/// <returns>The type of value produced by this animation.</returns>
		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06001756 RID: 5974 RVA: 0x000726D3 File Offset: 0x000708D3
		public sealed override Type TargetPropertyType
		{
			get
			{
				base.ReadPreamble();
				return typeof(Thickness);
			}
		}

		/// <summary>Gets the current value of the animation.</summary>
		/// <param name="defaultOriginValue">The origin value provided to the animation if the animation does not have its own start value. </param>
		/// <param name="defaultDestinationValue">The destination value provided to the animation if the animation does not have its own destination value.</param>
		/// <param name="animationClock">The <see cref="T:System.Windows.Media.Animation.AnimationClock" /> which can generate the <see cref="P:System.Windows.Media.Animation.Clock.CurrentTime" /> or <see cref="P:System.Windows.Media.Animation.Clock.CurrentProgress" /> value to be used by the animation to generate its output value.</param>
		/// <returns>The current value of this animation.</returns>
		// Token: 0x06001757 RID: 5975 RVA: 0x000726E5 File Offset: 0x000708E5
		public Thickness GetCurrentValue(Thickness defaultOriginValue, Thickness defaultDestinationValue, AnimationClock animationClock)
		{
			base.ReadPreamble();
			if (animationClock == null)
			{
				throw new ArgumentNullException("animationClock");
			}
			if (animationClock.CurrentState == ClockState.Stopped)
			{
				return defaultDestinationValue;
			}
			return this.GetCurrentValueCore(defaultOriginValue, defaultDestinationValue, animationClock);
		}

		/// <summary>Calculates a value that represents the current value of the property being animated, as determined by the host animation. </summary>
		/// <param name="defaultOriginValue">The suggested origin value, used if the animation does not have its own explicitly set start value. </param>
		/// <param name="defaultDestinationValue">The suggested destination value, used if the animation does not have its own explicitly set end value.</param>
		/// <param name="animationClock">An <see cref="T:System.Windows.Media.Animation.AnimationClock" /> that generates the <see cref="P:System.Windows.Media.Animation.Clock.CurrentTime" /> or <see cref="P:System.Windows.Media.Animation.Clock.CurrentProgress" /> used by the host animation.</param>
		/// <returns>The current value of this animation.</returns>
		// Token: 0x06001758 RID: 5976
		protected abstract Thickness GetCurrentValueCore(Thickness defaultOriginValue, Thickness defaultDestinationValue, AnimationClock animationClock);
	}
}
