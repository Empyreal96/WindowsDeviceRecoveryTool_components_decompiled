using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Markup;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;

namespace System.Windows.Media.Animation
{
	/// <summary> Animates the value of a <see cref="T:System.Windows.Thickness" /> property along a set of <see cref="P:System.Windows.Media.Animation.ThicknessAnimationUsingKeyFrames.KeyFrames" />.</summary>
	// Token: 0x02000191 RID: 401
	[ContentProperty("KeyFrames")]
	public class ThicknessAnimationUsingKeyFrames : ThicknessAnimationBase, IKeyFrameAnimation, IAddChild
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Media.Animation.ThicknessAnimationUsingKeyFrames" /> class.</summary>
		// Token: 0x06001759 RID: 5977 RVA: 0x0007270F File Offset: 0x0007090F
		public ThicknessAnimationUsingKeyFrames()
		{
			this._areKeyTimesValid = true;
		}

		/// <summary>Creates a modifiable clone of this <see cref="T:System.Windows.Media.Animation.ThicknessAnimationUsingKeyFrames" />, making deep copies of this object's values. When copying dependency properties, this method copies resource references and data bindings (but they might no longer resolve) but not animations or their current values.</summary>
		/// <returns>A modifiable clone of the current object. The cloned object's <see cref="P:System.Windows.Freezable.IsFrozen" /> property will be <see langword="false" /> even if the source's <see cref="P:System.Windows.Freezable.IsFrozen" /> property was <see langword="true." /></returns>
		// Token: 0x0600175A RID: 5978 RVA: 0x0007271E File Offset: 0x0007091E
		public new ThicknessAnimationUsingKeyFrames Clone()
		{
			return (ThicknessAnimationUsingKeyFrames)base.Clone();
		}

		/// <summary>Creates a modifiable clone of this <see cref="T:System.Windows.Media.Animation.ThicknessAnimationUsingKeyFrames" /> object, making deep copies of this object's current values. Resource references, data bindings, and animations are not copied, but their current values are. </summary>
		/// <returns>A modifiable clone of the current object. The cloned object's <see cref="P:System.Windows.Freezable.IsFrozen" /> property will be <see langword="false" /> even if the source's <see cref="P:System.Windows.Freezable.IsFrozen" /> property was <see langword="true" />.</returns>
		// Token: 0x0600175B RID: 5979 RVA: 0x0007272B File Offset: 0x0007092B
		public new ThicknessAnimationUsingKeyFrames CloneCurrentValue()
		{
			return (ThicknessAnimationUsingKeyFrames)base.CloneCurrentValue();
		}

		/// <summary>Makes this instance of <see cref="T:System.Windows.Media.Animation.ThicknessAnimationUsingKeyFrames" /> object unmodifiable or determines whether it can be made unmodifiable..</summary>
		/// <param name="isChecking">
		///       <see langword="true" /> to check if this instance can be frozen; <see langword="false" /> to freeze this instance. </param>
		/// <returns>If <paramref name="isChecking" /> is true, this method returns <see langword="true" /> if this instance can be made read-only, or <see langword="false" /> if it cannot be made read-only. If <paramref name="isChecking" /> is false, this method returns <see langword="true" /> if this instance is now read-only, or <see langword="false" /> if it cannot be made read-only, with the side effect of having begun to change the frozen status of this object.</returns>
		// Token: 0x0600175C RID: 5980 RVA: 0x00072738 File Offset: 0x00070938
		protected override bool FreezeCore(bool isChecking)
		{
			bool flag = base.FreezeCore(isChecking);
			flag &= Freezable.Freeze(this._keyFrames, isChecking);
			if (flag & !this._areKeyTimesValid)
			{
				this.ResolveKeyTimes();
			}
			return flag;
		}

		/// <summary>Called when the current <see cref="T:System.Windows.Media.Animation.ThicknessAnimationUsingKeyFrames" /> object is modified.</summary>
		// Token: 0x0600175D RID: 5981 RVA: 0x00072770 File Offset: 0x00070970
		protected override void OnChanged()
		{
			this._areKeyTimesValid = false;
			base.OnChanged();
		}

		/// <summary>Creates a new instance of <see cref="T:System.Windows.Media.Animation.ThicknessAnimationUsingKeyFrames" />. </summary>
		/// <returns>A new instance of <see cref="T:System.Windows.Media.Animation.ThicknessAnimationUsingKeyFrames" />.</returns>
		// Token: 0x0600175E RID: 5982 RVA: 0x0007277F File Offset: 0x0007097F
		protected override Freezable CreateInstanceCore()
		{
			return new ThicknessAnimationUsingKeyFrames();
		}

		/// <summary>Makes this instance a deep copy of the specified <see cref="T:System.Windows.Media.Animation.ThicknessAnimationUsingKeyFrames" />. When copying dependency properties, this method copies resource references and data bindings (but they might no longer resolve) but not animations or their current values.</summary>
		/// <param name="sourceFreezable">The <see cref="T:System.Windows.Media.Animation.ThicknessAnimationUsingKeyFrames" /> to clone.</param>
		// Token: 0x0600175F RID: 5983 RVA: 0x00072788 File Offset: 0x00070988
		protected override void CloneCore(Freezable sourceFreezable)
		{
			ThicknessAnimationUsingKeyFrames sourceAnimation = (ThicknessAnimationUsingKeyFrames)sourceFreezable;
			base.CloneCore(sourceFreezable);
			this.CopyCommon(sourceAnimation, false);
		}

		/// <summary>Makes this instance a modifiable deep copy of the specified <see cref="T:System.Windows.Media.Animation.ThicknessAnimationUsingKeyFrames" /> using current property values. Resource references, data bindings, and animations are not copied, but their current values are.</summary>
		/// <param name="sourceFreezable">The <see cref="T:System.Windows.Media.Animation.ThicknessAnimationUsingKeyFrames" /> to clone.</param>
		// Token: 0x06001760 RID: 5984 RVA: 0x000727AC File Offset: 0x000709AC
		protected override void CloneCurrentValueCore(Freezable sourceFreezable)
		{
			ThicknessAnimationUsingKeyFrames sourceAnimation = (ThicknessAnimationUsingKeyFrames)sourceFreezable;
			base.CloneCurrentValueCore(sourceFreezable);
			this.CopyCommon(sourceAnimation, true);
		}

		/// <summary>Makes this instance a clone of the specified <see cref="T:System.Windows.Media.Animation.ThicknessAnimationUsingKeyFrames" /> object. </summary>
		/// <param name="source">The <see cref="T:System.Windows.Media.Animation.ThicknessAnimationUsingKeyFrames" /> object to clone.</param>
		// Token: 0x06001761 RID: 5985 RVA: 0x000727D0 File Offset: 0x000709D0
		protected override void GetAsFrozenCore(Freezable source)
		{
			ThicknessAnimationUsingKeyFrames sourceAnimation = (ThicknessAnimationUsingKeyFrames)source;
			base.GetAsFrozenCore(source);
			this.CopyCommon(sourceAnimation, false);
		}

		/// <summary>Makes this instance a frozen clone of the specified <see cref="T:System.Windows.Media.Animation.ThicknessAnimationUsingKeyFrames" />. Resource references, data bindings, and animations are not copied, but their current values are.</summary>
		/// <param name="source">The <see cref="T:System.Windows.Media.Animation.ThicknessAnimationUsingKeyFrames" /> to copy and freeze.</param>
		// Token: 0x06001762 RID: 5986 RVA: 0x000727F4 File Offset: 0x000709F4
		protected override void GetCurrentValueAsFrozenCore(Freezable source)
		{
			ThicknessAnimationUsingKeyFrames sourceAnimation = (ThicknessAnimationUsingKeyFrames)source;
			base.GetCurrentValueAsFrozenCore(source);
			this.CopyCommon(sourceAnimation, true);
		}

		// Token: 0x06001763 RID: 5987 RVA: 0x00072818 File Offset: 0x00070A18
		private void CopyCommon(ThicknessAnimationUsingKeyFrames sourceAnimation, bool isCurrentValueClone)
		{
			this._areKeyTimesValid = sourceAnimation._areKeyTimesValid;
			if (this._areKeyTimesValid && sourceAnimation._sortedResolvedKeyFrames != null)
			{
				this._sortedResolvedKeyFrames = (ResolvedKeyFrameEntry[])sourceAnimation._sortedResolvedKeyFrames.Clone();
			}
			if (sourceAnimation._keyFrames != null)
			{
				if (isCurrentValueClone)
				{
					this._keyFrames = (ThicknessKeyFrameCollection)sourceAnimation._keyFrames.CloneCurrentValue();
				}
				else
				{
					this._keyFrames = sourceAnimation._keyFrames.Clone();
				}
				base.OnFreezablePropertyChanged(null, this._keyFrames);
			}
		}

		/// <summary>Adds a child object.</summary>
		/// <param name="child">The child object to add.</param>
		// Token: 0x06001764 RID: 5988 RVA: 0x00072898 File Offset: 0x00070A98
		void IAddChild.AddChild(object child)
		{
			base.WritePreamble();
			if (child == null)
			{
				throw new ArgumentNullException("child");
			}
			this.AddChild(child);
			base.WritePostscript();
		}

		/// <summary>Adds a child <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" /> to this <see cref="T:System.Windows.Media.Animation.ThicknessAnimationUsingKeyFrames" />. </summary>
		/// <param name="child">The object to be added as the child of this <see cref="T:System.Windows.Media.Animation.ThicknessAnimationUsingKeyFrames" />. </param>
		/// <exception cref="T:System.ArgumentException">The parameter <paramref name="child" /> is not a <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" />.</exception>
		// Token: 0x06001765 RID: 5989 RVA: 0x000728BC File Offset: 0x00070ABC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void AddChild(object child)
		{
			ThicknessKeyFrame thicknessKeyFrame = child as ThicknessKeyFrame;
			if (thicknessKeyFrame != null)
			{
				this.KeyFrames.Add(thicknessKeyFrame);
				return;
			}
			throw new ArgumentException(SR.Get("Animation_ChildMustBeKeyFrame"), "child");
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="childText">The text to add to the object.</param>
		// Token: 0x06001766 RID: 5990 RVA: 0x000728F5 File Offset: 0x00070AF5
		void IAddChild.AddText(string childText)
		{
			if (childText == null)
			{
				throw new ArgumentNullException("childText");
			}
			this.AddText(childText);
		}

		/// <summary>Adds a text string as a child of this <see cref="T:System.Windows.Media.Animation.ThicknessAnimationUsingKeyFrames" />.</summary>
		/// <param name="childText">The text added to the <see cref="T:System.Windows.Media.Animation.ThicknessAnimationUsingKeyFrames" />.</param>
		/// <exception cref="T:System.InvalidOperationException">A <see cref="T:System.Windows.Media.Animation.ThicknessAnimationUsingKeyFrames" /> does not accept text as a child, so this method will raise this exception unless a derived class has overridden this behavior which allows text to be added.</exception>
		// Token: 0x06001767 RID: 5991 RVA: 0x0007290C File Offset: 0x00070B0C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void AddText(string childText)
		{
			throw new InvalidOperationException(SR.Get("Animation_NoTextChildren"));
		}

		/// <summary> Calculates a value that represents the current value of the property being animated, as determined by this instance of <see cref="T:System.Windows.Media.Animation.ThicknessAnimationUsingKeyFrames" />.</summary>
		/// <param name="defaultOriginValue">The suggested origin value, used if the animation does not have its own explicitly set start value.</param>
		/// <param name="defaultDestinationValue">The suggested destination value, used if the animation does not have its own explicitly set end value.</param>
		/// <param name="animationClock">An <see cref="T:System.Windows.Media.Animation.AnimationClock" /> that generates the <see cref="P:System.Windows.Media.Animation.Clock.CurrentTime" /> or <see cref="P:System.Windows.Media.Animation.Clock.CurrentProgress" /> used by the host animation.</param>
		/// <returns>The calculated value of the property, as determined by the current instance.</returns>
		// Token: 0x06001768 RID: 5992 RVA: 0x00072920 File Offset: 0x00070B20
		protected sealed override Thickness GetCurrentValueCore(Thickness defaultOriginValue, Thickness defaultDestinationValue, AnimationClock animationClock)
		{
			if (this._keyFrames == null)
			{
				return defaultDestinationValue;
			}
			if (!this._areKeyTimesValid)
			{
				this.ResolveKeyTimes();
			}
			if (this._sortedResolvedKeyFrames == null)
			{
				return defaultDestinationValue;
			}
			TimeSpan value = animationClock.CurrentTime.Value;
			int num = this._sortedResolvedKeyFrames.Length;
			int num2 = num - 1;
			int i;
			for (i = 0; i < num; i++)
			{
				if (!(value > this._sortedResolvedKeyFrames[i]._resolvedKeyTime))
				{
					break;
				}
			}
			while (i < num2 && value == this._sortedResolvedKeyFrames[i + 1]._resolvedKeyTime)
			{
				i++;
			}
			Thickness thickness;
			if (i == num)
			{
				thickness = this.GetResolvedKeyFrameValue(num2);
			}
			else if (value == this._sortedResolvedKeyFrames[i]._resolvedKeyTime)
			{
				thickness = this.GetResolvedKeyFrameValue(i);
			}
			else
			{
				Thickness baseValue;
				double keyFrameProgress;
				if (i == 0)
				{
					if (this.IsAdditive)
					{
						baseValue = AnimatedTypeHelpers.GetZeroValueThickness(defaultOriginValue);
					}
					else
					{
						baseValue = defaultOriginValue;
					}
					keyFrameProgress = value.TotalMilliseconds / this._sortedResolvedKeyFrames[0]._resolvedKeyTime.TotalMilliseconds;
				}
				else
				{
					int num3 = i - 1;
					TimeSpan resolvedKeyTime = this._sortedResolvedKeyFrames[num3]._resolvedKeyTime;
					baseValue = this.GetResolvedKeyFrameValue(num3);
					TimeSpan timeSpan = value - resolvedKeyTime;
					TimeSpan timeSpan2 = this._sortedResolvedKeyFrames[i]._resolvedKeyTime - resolvedKeyTime;
					keyFrameProgress = timeSpan.TotalMilliseconds / timeSpan2.TotalMilliseconds;
				}
				thickness = this.GetResolvedKeyFrame(i).InterpolateValue(baseValue, keyFrameProgress);
			}
			if (this.IsCumulative)
			{
				double num4 = (double)(animationClock.CurrentIteration - 1).Value;
				if (num4 > 0.0)
				{
					thickness = AnimatedTypeHelpers.AddThickness(thickness, AnimatedTypeHelpers.ScaleThickness(this.GetResolvedKeyFrameValue(num2), num4));
				}
			}
			if (this.IsAdditive)
			{
				return AnimatedTypeHelpers.AddThickness(defaultOriginValue, thickness);
			}
			return thickness;
		}

		/// <summary>Provide a custom natural <see cref="T:System.Windows.Duration" /> when the <see cref="T:System.Windows.Duration" /> property is set to <see cref="P:System.Windows.Duration.Automatic" />. </summary>
		/// <param name="clock">The <see cref="T:System.Windows.Media.Animation.Clock" /> whose natural duration is desired.</param>
		/// <returns>If the last key frame of this animation is a <see cref="T:System.Windows.Media.Animation.KeyTime" />, then this value is used as the <see cref="P:System.Windows.Media.Animation.Clock.NaturalDuration" />; otherwise it will be one second.</returns>
		// Token: 0x06001769 RID: 5993 RVA: 0x00072B1D File Offset: 0x00070D1D
		protected sealed override Duration GetNaturalDurationCore(Clock clock)
		{
			return new Duration(this.LargestTimeSpanKeyTime);
		}

		/// <summary>Gets or sets an ordered collection P:System.Windows.Media.Animation.IKeyFrameAnimation.KeyFrames associated with this animation sequence.</summary>
		/// <returns>An <see cref="T:System.Collections.IList" /> of <see cref="P:System.Windows.Media.Animation.IKeyFrameAnimation.KeyFrames" />.</returns>
		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x0600176A RID: 5994 RVA: 0x00072B2A File Offset: 0x00070D2A
		// (set) Token: 0x0600176B RID: 5995 RVA: 0x00072B32 File Offset: 0x00070D32
		IList IKeyFrameAnimation.KeyFrames
		{
			get
			{
				return this.KeyFrames;
			}
			set
			{
				this.KeyFrames = (ThicknessKeyFrameCollection)value;
			}
		}

		/// <summary> Gets or sets the collection of <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" /> objects that define the animation. </summary>
		/// <returns>The collection of <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" /> objects that define the animation. The default value is <see cref="P:System.Windows.Media.Animation.ThicknessKeyFrameCollection.Empty" />.</returns>
		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x0600176C RID: 5996 RVA: 0x00072B40 File Offset: 0x00070D40
		// (set) Token: 0x0600176D RID: 5997 RVA: 0x00072B9A File Offset: 0x00070D9A
		public ThicknessKeyFrameCollection KeyFrames
		{
			get
			{
				base.ReadPreamble();
				if (this._keyFrames == null)
				{
					if (base.IsFrozen)
					{
						this._keyFrames = ThicknessKeyFrameCollection.Empty;
					}
					else
					{
						base.WritePreamble();
						this._keyFrames = new ThicknessKeyFrameCollection();
						base.OnFreezablePropertyChanged(null, this._keyFrames);
						base.WritePostscript();
					}
				}
				return this._keyFrames;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				base.WritePreamble();
				if (value != this._keyFrames)
				{
					base.OnFreezablePropertyChanged(this._keyFrames, value);
					this._keyFrames = value;
					base.WritePostscript();
				}
			}
		}

		/// <summary>Returns true if the value of the <see cref="P:System.Windows.Media.Animation.ThicknessAnimationUsingKeyFrames.KeyFrames" /> property of this instance of <see cref="T:System.Windows.Media.Animation.ThicknessAnimationUsingKeyFrames" /> should be value-serialized.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value should be serialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600176E RID: 5998 RVA: 0x00072BD3 File Offset: 0x00070DD3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeKeyFrames()
		{
			base.ReadPreamble();
			return this._keyFrames != null && this._keyFrames.Count > 0;
		}

		/// <summary>Gets a value that specifies whether the animation's output value is added to the base value of the property being animated.  </summary>
		/// <returns>
		///     <see langword="true" /> if the animation adds its output value to the base value of the property being animated instead of replacing it; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x0600176F RID: 5999 RVA: 0x0007263E File Offset: 0x0007083E
		// (set) Token: 0x06001770 RID: 6000 RVA: 0x00072650 File Offset: 0x00070850
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

		/// <summary>Gets or sets a value that specifies whether the animation's value accumulates when it repeats.</summary>
		/// <returns>
		///     <see langword="true" /> if the animation accumulates its values when its <see cref="P:System.Windows.Media.Animation.Timeline.RepeatBehavior" /> property causes it to repeat its simple duration; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06001771 RID: 6001 RVA: 0x00072663 File Offset: 0x00070863
		// (set) Token: 0x06001772 RID: 6002 RVA: 0x00072675 File Offset: 0x00070875
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

		// Token: 0x06001773 RID: 6003 RVA: 0x00072BF3 File Offset: 0x00070DF3
		private Thickness GetResolvedKeyFrameValue(int resolvedKeyFrameIndex)
		{
			return this.GetResolvedKeyFrame(resolvedKeyFrameIndex).Value;
		}

		// Token: 0x06001774 RID: 6004 RVA: 0x00072C01 File Offset: 0x00070E01
		private ThicknessKeyFrame GetResolvedKeyFrame(int resolvedKeyFrameIndex)
		{
			return this._keyFrames[this._sortedResolvedKeyFrames[resolvedKeyFrameIndex]._originalKeyFrameIndex];
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06001775 RID: 6005 RVA: 0x00072C20 File Offset: 0x00070E20
		private TimeSpan LargestTimeSpanKeyTime
		{
			get
			{
				bool flag = false;
				TimeSpan timeSpan = TimeSpan.Zero;
				if (this._keyFrames != null)
				{
					int count = this._keyFrames.Count;
					for (int i = 0; i < count; i++)
					{
						KeyTime keyTime = this._keyFrames[i].KeyTime;
						if (keyTime.Type == KeyTimeType.TimeSpan)
						{
							flag = true;
							if (keyTime.TimeSpan > timeSpan)
							{
								timeSpan = keyTime.TimeSpan;
							}
						}
					}
				}
				if (flag)
				{
					return timeSpan;
				}
				return TimeSpan.FromSeconds(1.0);
			}
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x00072CA0 File Offset: 0x00070EA0
		private void ResolveKeyTimes()
		{
			int num = 0;
			if (this._keyFrames != null)
			{
				num = this._keyFrames.Count;
			}
			if (num == 0)
			{
				this._sortedResolvedKeyFrames = null;
				this._areKeyTimesValid = true;
				return;
			}
			this._sortedResolvedKeyFrames = new ResolvedKeyFrameEntry[num];
			int i;
			for (i = 0; i < num; i++)
			{
				this._sortedResolvedKeyFrames[i]._originalKeyFrameIndex = i;
			}
			TimeSpan resolvedKeyTime = TimeSpan.Zero;
			Duration duration = base.Duration;
			if (duration.HasTimeSpan)
			{
				resolvedKeyTime = duration.TimeSpan;
			}
			else
			{
				resolvedKeyTime = this.LargestTimeSpanKeyTime;
			}
			int num2 = num - 1;
			ArrayList arrayList = new ArrayList();
			bool flag = false;
			i = 0;
			while (i < num)
			{
				KeyTime keyTime = this._keyFrames[i].KeyTime;
				switch (keyTime.Type)
				{
				case KeyTimeType.Uniform:
				case KeyTimeType.Paced:
					if (i == num2)
					{
						this._sortedResolvedKeyFrames[i]._resolvedKeyTime = resolvedKeyTime;
						i++;
					}
					else if (i == 0 && keyTime.Type == KeyTimeType.Paced)
					{
						this._sortedResolvedKeyFrames[i]._resolvedKeyTime = TimeSpan.Zero;
						i++;
					}
					else
					{
						if (keyTime.Type == KeyTimeType.Paced)
						{
							flag = true;
						}
						ThicknessAnimationUsingKeyFrames.KeyTimeBlock keyTimeBlock = default(ThicknessAnimationUsingKeyFrames.KeyTimeBlock);
						keyTimeBlock.BeginIndex = i;
						while (++i < num2)
						{
							KeyTimeType type = this._keyFrames[i].KeyTime.Type;
							if (type == KeyTimeType.Percent || type == KeyTimeType.TimeSpan)
							{
								break;
							}
							if (type == KeyTimeType.Paced)
							{
								flag = true;
							}
						}
						keyTimeBlock.EndIndex = i;
						arrayList.Add(keyTimeBlock);
					}
					break;
				case KeyTimeType.Percent:
					this._sortedResolvedKeyFrames[i]._resolvedKeyTime = TimeSpan.FromMilliseconds(keyTime.Percent * resolvedKeyTime.TotalMilliseconds);
					i++;
					break;
				case KeyTimeType.TimeSpan:
					this._sortedResolvedKeyFrames[i]._resolvedKeyTime = keyTime.TimeSpan;
					i++;
					break;
				}
			}
			for (int j = 0; j < arrayList.Count; j++)
			{
				ThicknessAnimationUsingKeyFrames.KeyTimeBlock keyTimeBlock2 = (ThicknessAnimationUsingKeyFrames.KeyTimeBlock)arrayList[j];
				TimeSpan timeSpan = TimeSpan.Zero;
				if (keyTimeBlock2.BeginIndex > 0)
				{
					timeSpan = this._sortedResolvedKeyFrames[keyTimeBlock2.BeginIndex - 1]._resolvedKeyTime;
				}
				long num3 = (long)(keyTimeBlock2.EndIndex - keyTimeBlock2.BeginIndex + 1);
				TimeSpan t = TimeSpan.FromTicks((this._sortedResolvedKeyFrames[keyTimeBlock2.EndIndex]._resolvedKeyTime - timeSpan).Ticks / num3);
				i = keyTimeBlock2.BeginIndex;
				TimeSpan timeSpan2 = timeSpan + t;
				while (i < keyTimeBlock2.EndIndex)
				{
					this._sortedResolvedKeyFrames[i]._resolvedKeyTime = timeSpan2;
					timeSpan2 += t;
					i++;
				}
			}
			if (flag)
			{
				this.ResolvePacedKeyTimes();
			}
			Array.Sort<ResolvedKeyFrameEntry>(this._sortedResolvedKeyFrames);
			this._areKeyTimesValid = true;
		}

		// Token: 0x06001777 RID: 6007 RVA: 0x00072F7C File Offset: 0x0007117C
		private void ResolvePacedKeyTimes()
		{
			int num = 1;
			int num2 = this._sortedResolvedKeyFrames.Length - 1;
			do
			{
				if (this._keyFrames[num].KeyTime.Type == KeyTimeType.Paced)
				{
					int num3 = num;
					List<double> list = new List<double>();
					TimeSpan resolvedKeyTime = this._sortedResolvedKeyFrames[num - 1]._resolvedKeyTime;
					double num4 = 0.0;
					Thickness from = this._keyFrames[num - 1].Value;
					do
					{
						Thickness value = this._keyFrames[num].Value;
						num4 += AnimatedTypeHelpers.GetSegmentLengthThickness(from, value);
						list.Add(num4);
						from = value;
						num++;
					}
					while (num < num2 && this._keyFrames[num].KeyTime.Type == KeyTimeType.Paced);
					num4 += AnimatedTypeHelpers.GetSegmentLengthThickness(from, this._keyFrames[num].Value);
					TimeSpan timeSpan = this._sortedResolvedKeyFrames[num]._resolvedKeyTime - resolvedKeyTime;
					int i = 0;
					int num5 = num3;
					while (i < list.Count)
					{
						this._sortedResolvedKeyFrames[num5]._resolvedKeyTime = resolvedKeyTime + TimeSpan.FromMilliseconds(list[i] / num4 * timeSpan.TotalMilliseconds);
						i++;
						num5++;
					}
				}
				else
				{
					num++;
				}
			}
			while (num < num2);
		}

		// Token: 0x040012BA RID: 4794
		private ThicknessKeyFrameCollection _keyFrames;

		// Token: 0x040012BB RID: 4795
		private ResolvedKeyFrameEntry[] _sortedResolvedKeyFrames;

		// Token: 0x040012BC RID: 4796
		private bool _areKeyTimesValid;

		// Token: 0x0200085E RID: 2142
		private struct KeyTimeBlock
		{
			// Token: 0x0400408F RID: 16527
			public int BeginIndex;

			// Token: 0x04004090 RID: 16528
			public int EndIndex;
		}
	}
}
