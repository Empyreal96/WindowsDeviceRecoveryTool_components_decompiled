using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Threading;
using MS.Internal;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents an element that has a value within a specific range. </summary>
	// Token: 0x0200059F RID: 1439
	[DefaultEvent("ValueChanged")]
	[DefaultProperty("Value")]
	public abstract class RangeBase : Control
	{
		// Token: 0x06005F3F RID: 24383 RVA: 0x001AB680 File Offset: 0x001A9880
		static RangeBase()
		{
			RangeBase.ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>), typeof(RangeBase));
			RangeBase.MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(RangeBase), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(RangeBase.OnMinimumChanged)), new ValidateValueCallback(RangeBase.IsValidDoubleValue));
			RangeBase.MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(RangeBase), new FrameworkPropertyMetadata(1.0, new PropertyChangedCallback(RangeBase.OnMaximumChanged), new CoerceValueCallback(RangeBase.CoerceMaximum)), new ValidateValueCallback(RangeBase.IsValidDoubleValue));
			RangeBase.ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(RangeBase), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(RangeBase.OnValueChanged), new CoerceValueCallback(RangeBase.ConstrainToRange)), new ValidateValueCallback(RangeBase.IsValidDoubleValue));
			RangeBase.LargeChangeProperty = DependencyProperty.Register("LargeChange", typeof(double), typeof(RangeBase), new FrameworkPropertyMetadata(1.0), new ValidateValueCallback(RangeBase.IsValidChange));
			RangeBase.SmallChangeProperty = DependencyProperty.Register("SmallChange", typeof(double), typeof(RangeBase), new FrameworkPropertyMetadata(0.1), new ValidateValueCallback(RangeBase.IsValidChange));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(RangeBase), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(RangeBase), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
		}

		/// <summary>Occurs when the range value changes. </summary>
		// Token: 0x1400011A RID: 282
		// (add) Token: 0x06005F41 RID: 24385 RVA: 0x001AB886 File Offset: 0x001A9A86
		// (remove) Token: 0x06005F42 RID: 24386 RVA: 0x001AB894 File Offset: 0x001A9A94
		[Category("Behavior")]
		public event RoutedPropertyChangedEventHandler<double> ValueChanged
		{
			add
			{
				base.AddHandler(RangeBase.ValueChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(RangeBase.ValueChangedEvent, value);
			}
		}

		/// <summary>Gets or sets the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> possible <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> of the range element.  </summary>
		/// <returns>
		///     <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> possible <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> of the range element. The default is 0.</returns>
		// Token: 0x170016F7 RID: 5879
		// (get) Token: 0x06005F43 RID: 24387 RVA: 0x001AB8A2 File Offset: 0x001A9AA2
		// (set) Token: 0x06005F44 RID: 24388 RVA: 0x001AB8B4 File Offset: 0x001A9AB4
		[Bindable(true)]
		[Category("Behavior")]
		public double Minimum
		{
			get
			{
				return (double)base.GetValue(RangeBase.MinimumProperty);
			}
			set
			{
				base.SetValue(RangeBase.MinimumProperty, value);
			}
		}

		// Token: 0x06005F45 RID: 24389 RVA: 0x001AB8C8 File Offset: 0x001A9AC8
		private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RangeBase rangeBase = (RangeBase)d;
			RangeBaseAutomationPeer rangeBaseAutomationPeer = UIElementAutomationPeer.FromElement(rangeBase) as RangeBaseAutomationPeer;
			if (rangeBaseAutomationPeer != null)
			{
				rangeBaseAutomationPeer.RaiseMinimumPropertyChangedEvent((double)e.OldValue, (double)e.NewValue);
			}
			rangeBase.CoerceValue(RangeBase.MaximumProperty);
			rangeBase.CoerceValue(RangeBase.ValueProperty);
			rangeBase.OnMinimumChanged((double)e.OldValue, (double)e.NewValue);
		}

		/// <summary>Called when the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> property changes. </summary>
		/// <param name="oldMinimum">Old value of the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> property.</param>
		/// <param name="newMinimum">New value of the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> property.</param>
		// Token: 0x06005F46 RID: 24390 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnMinimumChanged(double oldMinimum, double newMinimum)
		{
		}

		// Token: 0x06005F47 RID: 24391 RVA: 0x001AB940 File Offset: 0x001A9B40
		private static object CoerceMaximum(DependencyObject d, object value)
		{
			RangeBase rangeBase = (RangeBase)d;
			double minimum = rangeBase.Minimum;
			if ((double)value < minimum)
			{
				return minimum;
			}
			return value;
		}

		/// <summary>Gets or sets the highest possible <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> of the range element.  </summary>
		/// <returns>The highest possible <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> of the range element. The default is 1.</returns>
		// Token: 0x170016F8 RID: 5880
		// (get) Token: 0x06005F48 RID: 24392 RVA: 0x001AB96C File Offset: 0x001A9B6C
		// (set) Token: 0x06005F49 RID: 24393 RVA: 0x001AB97E File Offset: 0x001A9B7E
		[Bindable(true)]
		[Category("Behavior")]
		public double Maximum
		{
			get
			{
				return (double)base.GetValue(RangeBase.MaximumProperty);
			}
			set
			{
				base.SetValue(RangeBase.MaximumProperty, value);
			}
		}

		// Token: 0x06005F4A RID: 24394 RVA: 0x001AB994 File Offset: 0x001A9B94
		private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RangeBase rangeBase = (RangeBase)d;
			RangeBaseAutomationPeer rangeBaseAutomationPeer = UIElementAutomationPeer.FromElement(rangeBase) as RangeBaseAutomationPeer;
			if (rangeBaseAutomationPeer != null)
			{
				rangeBaseAutomationPeer.RaiseMaximumPropertyChangedEvent((double)e.OldValue, (double)e.NewValue);
			}
			rangeBase.CoerceValue(RangeBase.ValueProperty);
			rangeBase.OnMaximumChanged((double)e.OldValue, (double)e.NewValue);
		}

		/// <summary>Called when the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> property changes. </summary>
		/// <param name="oldMaximum">Old value of the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> property.</param>
		/// <param name="newMaximum">New value of the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> property.</param>
		// Token: 0x06005F4B RID: 24395 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnMaximumChanged(double oldMaximum, double newMaximum)
		{
		}

		// Token: 0x06005F4C RID: 24396 RVA: 0x001ABA00 File Offset: 0x001A9C00
		internal static object ConstrainToRange(DependencyObject d, object value)
		{
			RangeBase rangeBase = (RangeBase)d;
			double minimum = rangeBase.Minimum;
			double num = (double)value;
			if (num < minimum)
			{
				return minimum;
			}
			double maximum = rangeBase.Maximum;
			if (num > maximum)
			{
				return maximum;
			}
			return value;
		}

		/// <summary>Gets or sets the current magnitude of the range control.  </summary>
		/// <returns>The current magnitude of the range control. The default is 0.</returns>
		// Token: 0x170016F9 RID: 5881
		// (get) Token: 0x06005F4D RID: 24397 RVA: 0x001ABA40 File Offset: 0x001A9C40
		// (set) Token: 0x06005F4E RID: 24398 RVA: 0x001ABA52 File Offset: 0x001A9C52
		[Bindable(true)]
		[Category("Behavior")]
		public double Value
		{
			get
			{
				return (double)base.GetValue(RangeBase.ValueProperty);
			}
			set
			{
				base.SetValue(RangeBase.ValueProperty, value);
			}
		}

		// Token: 0x06005F4F RID: 24399 RVA: 0x001ABA68 File Offset: 0x001A9C68
		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RangeBase rangeBase = (RangeBase)d;
			RangeBaseAutomationPeer rangeBaseAutomationPeer = UIElementAutomationPeer.FromElement(rangeBase) as RangeBaseAutomationPeer;
			if (rangeBaseAutomationPeer != null)
			{
				rangeBaseAutomationPeer.RaiseValuePropertyChangedEvent((double)e.OldValue, (double)e.NewValue);
			}
			rangeBase.OnValueChanged((double)e.OldValue, (double)e.NewValue);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.Primitives.RangeBase.ValueChanged" /> routed event. </summary>
		/// <param name="oldValue">Old value of the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> property</param>
		/// <param name="newValue">New value of the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> property</param>
		// Token: 0x06005F50 RID: 24400 RVA: 0x001ABAC8 File Offset: 0x001A9CC8
		protected virtual void OnValueChanged(double oldValue, double newValue)
		{
			base.RaiseEvent(new RoutedPropertyChangedEventArgs<double>(oldValue, newValue)
			{
				RoutedEvent = RangeBase.ValueChangedEvent
			});
		}

		// Token: 0x06005F51 RID: 24401 RVA: 0x001ABAF0 File Offset: 0x001A9CF0
		private static bool IsValidDoubleValue(object value)
		{
			double num = (double)value;
			return !DoubleUtil.IsNaN(num) && !double.IsInfinity(num);
		}

		// Token: 0x06005F52 RID: 24402 RVA: 0x001ABB18 File Offset: 0x001A9D18
		private static bool IsValidChange(object value)
		{
			double num = (double)value;
			return RangeBase.IsValidDoubleValue(value) && num >= 0.0;
		}

		/// <summary>Gets or sets a value to be added to or subtracted from the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> of a <see cref="T:System.Windows.Controls.Primitives.RangeBase" /> control.  </summary>
		/// <returns>
		///     <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> to add to or subtract from the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> of the <see cref="T:System.Windows.Controls.Primitives.RangeBase" /> element. The default is 1.</returns>
		// Token: 0x170016FA RID: 5882
		// (get) Token: 0x06005F53 RID: 24403 RVA: 0x001ABB45 File Offset: 0x001A9D45
		// (set) Token: 0x06005F54 RID: 24404 RVA: 0x001ABB57 File Offset: 0x001A9D57
		[Bindable(true)]
		[Category("Behavior")]
		public double LargeChange
		{
			get
			{
				return (double)base.GetValue(RangeBase.LargeChangeProperty);
			}
			set
			{
				base.SetValue(RangeBase.LargeChangeProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> to be added to or subtracted from the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> of a <see cref="T:System.Windows.Controls.Primitives.RangeBase" /> control.  </summary>
		/// <returns>
		///     <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> to add to or subtract from the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> of the <see cref="T:System.Windows.Controls.Primitives.RangeBase" /> element. The default is 0.1.</returns>
		// Token: 0x170016FB RID: 5883
		// (get) Token: 0x06005F55 RID: 24405 RVA: 0x001ABB6A File Offset: 0x001A9D6A
		// (set) Token: 0x06005F56 RID: 24406 RVA: 0x001ABB7C File Offset: 0x001A9D7C
		[Bindable(true)]
		[Category("Behavior")]
		public double SmallChange
		{
			get
			{
				return (double)base.GetValue(RangeBase.SmallChangeProperty);
			}
			set
			{
				base.SetValue(RangeBase.SmallChangeProperty, value);
			}
		}

		// Token: 0x06005F57 RID: 24407 RVA: 0x001ABB90 File Offset: 0x001A9D90
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (!base.IsEnabled)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Disabled",
					"Normal"
				});
			}
			else if (base.IsMouseOver)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"MouseOver",
					"Normal"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
			if (base.IsKeyboardFocused)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Focused",
					"Unfocused"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "Unfocused", useTransitions);
			}
			base.ChangeVisualState(useTransitions);
		}

		/// <summary>Provides a string representation of a <see cref="T:System.Windows.Controls.Primitives.RangeBase" /> object. </summary>
		/// <returns>Returns the string representation of a <see cref="T:System.Windows.Controls.Primitives.RangeBase" /> object.</returns>
		// Token: 0x06005F58 RID: 24408 RVA: 0x001ABC34 File Offset: 0x001A9E34
		public override string ToString()
		{
			string text = base.GetType().ToString();
			double min = double.NaN;
			double max = double.NaN;
			double val = double.NaN;
			bool valuesDefined = false;
			if (base.CheckAccess())
			{
				min = this.Minimum;
				max = this.Maximum;
				val = this.Value;
				valuesDefined = true;
			}
			else
			{
				base.Dispatcher.Invoke(DispatcherPriority.Send, new TimeSpan(0, 0, 0, 0, 20), new DispatcherOperationCallback(delegate(object o)
				{
					min = this.Minimum;
					max = this.Maximum;
					val = this.Value;
					valuesDefined = true;
					return null;
				}), null);
			}
			if (valuesDefined)
			{
				return SR.Get("ToStringFormatString_RangeBase", new object[]
				{
					text,
					min,
					max,
					val
				});
			}
			return text;
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> dependency property.</returns>
		// Token: 0x04003097 RID: 12439
		public static readonly DependencyProperty MinimumProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> dependency property.</returns>
		// Token: 0x04003098 RID: 12440
		public static readonly DependencyProperty MaximumProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> dependency property.</returns>
		// Token: 0x04003099 RID: 12441
		public static readonly DependencyProperty ValueProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.RangeBase.LargeChange" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.RangeBase.LargeChange" /> dependency property.</returns>
		// Token: 0x0400309A RID: 12442
		public static readonly DependencyProperty LargeChangeProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.RangeBase.SmallChange" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.RangeBase.SmallChange" /> dependency property.</returns>
		// Token: 0x0400309B RID: 12443
		public static readonly DependencyProperty SmallChangeProperty;
	}
}
