using System;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MS.Internal;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Indicates the progress of an operation. </summary>
	// Token: 0x0200051D RID: 1309
	[TemplatePart(Name = "PART_Track", Type = typeof(FrameworkElement))]
	[TemplatePart(Name = "PART_Indicator", Type = typeof(FrameworkElement))]
	[TemplatePart(Name = "PART_GlowRect", Type = typeof(FrameworkElement))]
	public class ProgressBar : RangeBase
	{
		// Token: 0x0600548D RID: 21645 RVA: 0x00176808 File Offset: 0x00174A08
		static ProgressBar()
		{
			UIElement.FocusableProperty.OverrideMetadata(typeof(ProgressBar), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressBar), new FrameworkPropertyMetadata(typeof(ProgressBar)));
			ProgressBar._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ProgressBar));
			RangeBase.MaximumProperty.OverrideMetadata(typeof(ProgressBar), new FrameworkPropertyMetadata(100.0));
			Control.ForegroundProperty.OverrideMetadata(typeof(ProgressBar), new FrameworkPropertyMetadata(new PropertyChangedCallback(ProgressBar.OnForegroundChanged)));
			ControlsTraceLogger.AddControl(TelemetryControls.ProgressBar);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.ProgressBar" /> class. </summary>
		// Token: 0x0600548E RID: 21646 RVA: 0x00176942 File Offset: 0x00174B42
		public ProgressBar()
		{
			base.IsVisibleChanged += delegate(object s, DependencyPropertyChangedEventArgs e)
			{
				this.UpdateAnimation();
			};
		}

		/// <summary>Gets or sets whether the <see cref="T:System.Windows.Controls.ProgressBar" /> shows actual values or generic, continuous progress feedback.   </summary>
		/// <returns>
		///     <see langword="false" /> if the <see cref="T:System.Windows.Controls.ProgressBar" /> shows actual values; <see langword="true" /> if the <see cref="T:System.Windows.Controls.ProgressBar" /> shows generic progress. The default is <see langword="false" />.</returns>
		// Token: 0x17001490 RID: 5264
		// (get) Token: 0x0600548F RID: 21647 RVA: 0x0017695C File Offset: 0x00174B5C
		// (set) Token: 0x06005490 RID: 21648 RVA: 0x0017696E File Offset: 0x00174B6E
		public bool IsIndeterminate
		{
			get
			{
				return (bool)base.GetValue(ProgressBar.IsIndeterminateProperty);
			}
			set
			{
				base.SetValue(ProgressBar.IsIndeterminateProperty, value);
			}
		}

		// Token: 0x06005491 RID: 21649 RVA: 0x0017697C File Offset: 0x00174B7C
		private static void OnIsIndeterminateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ProgressBar progressBar = (ProgressBar)d;
			ProgressBarAutomationPeer progressBarAutomationPeer = UIElementAutomationPeer.FromElement(progressBar) as ProgressBarAutomationPeer;
			if (progressBarAutomationPeer != null)
			{
				progressBarAutomationPeer.InvalidatePeer();
			}
			progressBar.SetProgressBarGlowElementBrush();
			progressBar.SetProgressBarIndicatorLength();
			progressBar.UpdateVisualState();
		}

		// Token: 0x06005492 RID: 21650 RVA: 0x001769B8 File Offset: 0x00174BB8
		private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ProgressBar progressBar = (ProgressBar)d;
			progressBar.SetProgressBarGlowElementBrush();
		}

		/// <summary>Gets or sets the orientation of a <see cref="T:System.Windows.Controls.ProgressBar" />: horizontal or vertical.   </summary>
		/// <returns>One of the <see cref="T:System.Windows.Controls.Orientation" /> values. The default is <see cref="F:System.Windows.Controls.Orientation.Horizontal" />.</returns>
		// Token: 0x17001491 RID: 5265
		// (get) Token: 0x06005493 RID: 21651 RVA: 0x001769D2 File Offset: 0x00174BD2
		// (set) Token: 0x06005494 RID: 21652 RVA: 0x001769E4 File Offset: 0x00174BE4
		public Orientation Orientation
		{
			get
			{
				return (Orientation)base.GetValue(ProgressBar.OrientationProperty);
			}
			set
			{
				base.SetValue(ProgressBar.OrientationProperty, value);
			}
		}

		// Token: 0x06005495 RID: 21653 RVA: 0x001769F8 File Offset: 0x00174BF8
		internal static bool IsValidOrientation(object o)
		{
			Orientation orientation = (Orientation)o;
			return orientation == Orientation.Horizontal || orientation == Orientation.Vertical;
		}

		// Token: 0x06005496 RID: 21654 RVA: 0x00176A18 File Offset: 0x00174C18
		private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ProgressBar progressBar = (ProgressBar)d;
			progressBar.SetProgressBarIndicatorLength();
		}

		// Token: 0x06005497 RID: 21655 RVA: 0x00176A34 File Offset: 0x00174C34
		private void SetProgressBarIndicatorLength()
		{
			if (this._track != null && this._indicator != null)
			{
				double minimum = base.Minimum;
				double maximum = base.Maximum;
				double value = base.Value;
				double num = (this.IsIndeterminate || maximum <= minimum) ? 1.0 : ((value - minimum) / (maximum - minimum));
				this._indicator.Width = num * this._track.ActualWidth;
				this.UpdateAnimation();
			}
		}

		// Token: 0x06005498 RID: 21656 RVA: 0x00176AA4 File Offset: 0x00174CA4
		private void SetProgressBarGlowElementBrush()
		{
			if (this._glow == null)
			{
				return;
			}
			this._glow.InvalidateProperty(UIElement.OpacityMaskProperty);
			this._glow.InvalidateProperty(Shape.FillProperty);
			if (this.IsIndeterminate)
			{
				if (base.Foreground is SolidColorBrush)
				{
					Color color = ((SolidColorBrush)base.Foreground).Color;
					LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
					linearGradientBrush.StartPoint = new Point(0.0, 0.0);
					linearGradientBrush.EndPoint = new Point(1.0, 0.0);
					linearGradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 0.0));
					linearGradientBrush.GradientStops.Add(new GradientStop(color, 0.4));
					linearGradientBrush.GradientStops.Add(new GradientStop(color, 0.6));
					linearGradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 1.0));
					this._glow.SetCurrentValue(Shape.FillProperty, linearGradientBrush);
					return;
				}
				LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush();
				linearGradientBrush2.StartPoint = new Point(0.0, 0.0);
				linearGradientBrush2.EndPoint = new Point(1.0, 0.0);
				linearGradientBrush2.GradientStops.Add(new GradientStop(Colors.Transparent, 0.0));
				linearGradientBrush2.GradientStops.Add(new GradientStop(Colors.Black, 0.4));
				linearGradientBrush2.GradientStops.Add(new GradientStop(Colors.Black, 0.6));
				linearGradientBrush2.GradientStops.Add(new GradientStop(Colors.Transparent, 1.0));
				this._glow.SetCurrentValue(UIElement.OpacityMaskProperty, linearGradientBrush2);
				this._glow.SetCurrentValue(Shape.FillProperty, base.Foreground);
			}
		}

		// Token: 0x06005499 RID: 21657 RVA: 0x00176CA8 File Offset: 0x00174EA8
		private void UpdateAnimation()
		{
			if (this._glow != null)
			{
				if (base.IsVisible && this._glow.Width > 0.0 && this._indicator.Width > 0.0)
				{
					double num = this._indicator.Width + this._glow.Width;
					double num2 = -1.0 * this._glow.Width;
					TimeSpan timeSpan = TimeSpan.FromSeconds((double)((int)(num - num2)) / 200.0);
					TimeSpan t = TimeSpan.FromSeconds(1.0);
					TimeSpan value;
					if (DoubleUtil.GreaterThan(this._glow.Margin.Left, num2) && DoubleUtil.LessThan(this._glow.Margin.Left, num - 1.0))
					{
						value = TimeSpan.FromSeconds(-1.0 * (this._glow.Margin.Left - num2) / 200.0);
					}
					else
					{
						value = TimeSpan.Zero;
					}
					ThicknessAnimationUsingKeyFrames thicknessAnimationUsingKeyFrames = new ThicknessAnimationUsingKeyFrames();
					thicknessAnimationUsingKeyFrames.BeginTime = new TimeSpan?(value);
					thicknessAnimationUsingKeyFrames.Duration = new Duration(timeSpan + t);
					thicknessAnimationUsingKeyFrames.RepeatBehavior = RepeatBehavior.Forever;
					thicknessAnimationUsingKeyFrames.KeyFrames.Add(new LinearThicknessKeyFrame(new Thickness(num2, 0.0, 0.0, 0.0), TimeSpan.FromSeconds(0.0)));
					thicknessAnimationUsingKeyFrames.KeyFrames.Add(new LinearThicknessKeyFrame(new Thickness(num, 0.0, 0.0, 0.0), timeSpan));
					this._glow.BeginAnimation(FrameworkElement.MarginProperty, thicknessAnimationUsingKeyFrames);
					return;
				}
				this._glow.BeginAnimation(FrameworkElement.MarginProperty, null);
			}
		}

		// Token: 0x0600549A RID: 21658 RVA: 0x00176EA2 File Offset: 0x001750A2
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (!this.IsIndeterminate)
			{
				VisualStateManager.GoToState(this, "Determinate", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Indeterminate", useTransitions);
			}
			base.ChangeValidationVisualState(useTransitions);
		}

		/// <summary>Provides an appropriate <see cref="T:System.Windows.Automation.Peers.ProgressBarAutomationPeer" /> implementation for this control, as part of the WPF automation infrastructure.</summary>
		/// <returns>The type-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> implementation.</returns>
		// Token: 0x0600549B RID: 21659 RVA: 0x00176ECF File Offset: 0x001750CF
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ProgressBarAutomationPeer(this);
		}

		/// <summary>Updates the current position of the <see cref="T:System.Windows.Controls.ProgressBar" /> when the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> property changes. </summary>
		/// <param name="oldMinimum">Old value of the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> property.</param>
		/// <param name="newMinimum">New value of the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> property.</param>
		// Token: 0x0600549C RID: 21660 RVA: 0x00176ED7 File Offset: 0x001750D7
		protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
		{
			base.OnMinimumChanged(oldMinimum, newMinimum);
			this.SetProgressBarIndicatorLength();
		}

		/// <summary>Updates the current position of the <see cref="T:System.Windows.Controls.ProgressBar" /> when the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> property changes. </summary>
		/// <param name="oldMaximum">Old value of the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> property.</param>
		/// <param name="newMaximum">New value of the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> property.</param>
		// Token: 0x0600549D RID: 21661 RVA: 0x00176EE7 File Offset: 0x001750E7
		protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
		{
			base.OnMaximumChanged(oldMaximum, newMaximum);
			this.SetProgressBarIndicatorLength();
		}

		/// <summary>Updates the current position of the <see cref="T:System.Windows.Controls.ProgressBar" /> when the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> property changes. </summary>
		/// <param name="oldValue">Old value of the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> property.</param>
		/// <param name="newValue">New value of the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> property.</param>
		// Token: 0x0600549E RID: 21662 RVA: 0x00176EF7 File Offset: 0x001750F7
		protected override void OnValueChanged(double oldValue, double newValue)
		{
			base.OnValueChanged(oldValue, newValue);
			this.SetProgressBarIndicatorLength();
		}

		/// <summary>Called when a template is applied to a <see cref="T:System.Windows.Controls.ProgressBar" />.</summary>
		// Token: 0x0600549F RID: 21663 RVA: 0x00176F08 File Offset: 0x00175108
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (this._track != null)
			{
				this._track.SizeChanged -= this.OnTrackSizeChanged;
			}
			this._track = (base.GetTemplateChild("PART_Track") as FrameworkElement);
			this._indicator = (base.GetTemplateChild("PART_Indicator") as FrameworkElement);
			this._glow = (base.GetTemplateChild("PART_GlowRect") as FrameworkElement);
			if (this._track != null)
			{
				this._track.SizeChanged += this.OnTrackSizeChanged;
			}
			if (this.IsIndeterminate)
			{
				this.SetProgressBarGlowElementBrush();
			}
		}

		// Token: 0x060054A0 RID: 21664 RVA: 0x00176FA9 File Offset: 0x001751A9
		private void OnTrackSizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.SetProgressBarIndicatorLength();
		}

		// Token: 0x17001492 RID: 5266
		// (get) Token: 0x060054A1 RID: 21665 RVA: 0x00176FB1 File Offset: 0x001751B1
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ProgressBar._dType;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ProgressBar.IsIndeterminate" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ProgressBar.IsIndeterminate" /> dependency property.</returns>
		// Token: 0x04002DA0 RID: 11680
		public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register("IsIndeterminate", typeof(bool), typeof(ProgressBar), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(ProgressBar.OnIsIndeterminateChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ProgressBar.Orientation" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ProgressBar.Orientation" /> dependency property.</returns>
		// Token: 0x04002DA1 RID: 11681
		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(ProgressBar), new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(ProgressBar.OnOrientationChanged)), new ValidateValueCallback(ProgressBar.IsValidOrientation));

		// Token: 0x04002DA2 RID: 11682
		private const string TrackTemplateName = "PART_Track";

		// Token: 0x04002DA3 RID: 11683
		private const string IndicatorTemplateName = "PART_Indicator";

		// Token: 0x04002DA4 RID: 11684
		private const string GlowingRectTemplateName = "PART_GlowRect";

		// Token: 0x04002DA5 RID: 11685
		private FrameworkElement _track;

		// Token: 0x04002DA6 RID: 11686
		private FrameworkElement _indicator;

		// Token: 0x04002DA7 RID: 11687
		private FrameworkElement _glow;

		// Token: 0x04002DA8 RID: 11688
		private static DependencyObjectType _dType;
	}
}
