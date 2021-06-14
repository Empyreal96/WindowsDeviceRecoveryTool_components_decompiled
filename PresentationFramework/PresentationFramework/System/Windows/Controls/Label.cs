using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using System.Windows.Markup;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Represents the text label for a control and provides support for access keys.</summary>
	// Token: 0x020004F9 RID: 1273
	[Localizability(LocalizationCategory.Label)]
	public class Label : ContentControl
	{
		// Token: 0x0600511A RID: 20762 RVA: 0x0016BF94 File Offset: 0x0016A194
		static Label()
		{
			EventManager.RegisterClassHandler(typeof(Label), AccessKeyManager.AccessKeyPressedEvent, new AccessKeyPressedEventHandler(Label.OnAccessKeyPressed));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Label), new FrameworkPropertyMetadata(typeof(Label)));
			Label._dType = DependencyObjectType.FromSystemTypeInternal(typeof(Label));
			Control.IsTabStopProperty.OverrideMetadata(typeof(Label), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			UIElement.FocusableProperty.OverrideMetadata(typeof(Label), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			ControlsTraceLogger.AddControl(TelemetryControls.Label);
		}

		/// <summary>Gets or sets the element that receives focus when the user presses the label's access key. </summary>
		/// <returns>The <see cref="T:System.Windows.UIElement" /> that receives focus when the user presses the access key. The default is <see langword="null" />.</returns>
		// Token: 0x170013B0 RID: 5040
		// (get) Token: 0x0600511C RID: 20764 RVA: 0x0016C09D File Offset: 0x0016A29D
		// (set) Token: 0x0600511D RID: 20765 RVA: 0x0016C0AF File Offset: 0x0016A2AF
		[TypeConverter(typeof(NameReferenceConverter))]
		public UIElement Target
		{
			get
			{
				return (UIElement)base.GetValue(Label.TargetProperty);
			}
			set
			{
				base.SetValue(Label.TargetProperty, value);
			}
		}

		// Token: 0x0600511E RID: 20766 RVA: 0x0016C0C0 File Offset: 0x0016A2C0
		private static void OnTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Label label = (Label)d;
			UIElement uielement = (UIElement)e.OldValue;
			UIElement uielement2 = (UIElement)e.NewValue;
			if (uielement != null)
			{
				object value = uielement.GetValue(Label.LabeledByProperty);
				if (value == label)
				{
					uielement.ClearValue(Label.LabeledByProperty);
				}
			}
			if (uielement2 != null)
			{
				uielement2.SetValue(Label.LabeledByProperty, label);
			}
		}

		// Token: 0x0600511F RID: 20767 RVA: 0x0016C11B File Offset: 0x0016A31B
		internal static Label GetLabeledBy(DependencyObject o)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			return (Label)o.GetValue(Label.LabeledByProperty);
		}

		/// <summary>Provides an appropriate <see cref="T:System.Windows.Automation.Peers.LabelAutomationPeer" /> implementation for this control, as part of the WPF infrastructure.</summary>
		/// <returns>The type-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> implementation.</returns>
		// Token: 0x06005120 RID: 20768 RVA: 0x0016C13B File Offset: 0x0016A33B
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new LabelAutomationPeer(this);
		}

		// Token: 0x06005121 RID: 20769 RVA: 0x0016C144 File Offset: 0x0016A344
		private static void OnAccessKeyPressed(object sender, AccessKeyPressedEventArgs e)
		{
			Label label = sender as Label;
			if (!e.Handled && e.Scope == null && (e.Target == null || e.Target == label))
			{
				e.Target = label.Target;
			}
		}

		// Token: 0x170013B1 RID: 5041
		// (get) Token: 0x06005122 RID: 20770 RVA: 0x0016C185 File Offset: 0x0016A385
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return Label._dType;
			}
		}

		/// <summary> Identifies the <see cref="P:System.Windows.Controls.Label.Target" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Label.Target" /> dependency property.</returns>
		// Token: 0x04002C54 RID: 11348
		public static readonly DependencyProperty TargetProperty = DependencyProperty.Register("Target", typeof(UIElement), typeof(Label), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Label.OnTargetChanged)));

		// Token: 0x04002C55 RID: 11349
		private static readonly DependencyProperty LabeledByProperty = DependencyProperty.RegisterAttached("LabeledBy", typeof(Label), typeof(Label), new FrameworkPropertyMetadata(null));

		// Token: 0x04002C56 RID: 11350
		private static DependencyObjectType _dType;
	}
}
