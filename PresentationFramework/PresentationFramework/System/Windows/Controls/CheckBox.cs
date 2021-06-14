using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Represents a control that a user can select and clear.   </summary>
	// Token: 0x02000481 RID: 1153
	[DefaultEvent("CheckStateChanged")]
	[Localizability(LocalizationCategory.CheckBox)]
	public class CheckBox : ToggleButton
	{
		// Token: 0x0600433D RID: 17213 RVA: 0x00133A38 File Offset: 0x00131C38
		static CheckBox()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckBox), new FrameworkPropertyMetadata(typeof(CheckBox)));
			CheckBox._dType = DependencyObjectType.FromSystemTypeInternal(typeof(CheckBox));
			KeyboardNavigation.AcceptsReturnProperty.OverrideMetadata(typeof(CheckBox), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			ControlsTraceLogger.AddControl(TelemetryControls.CheckBox);
		}

		/// <summary>Creates an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> for the <see cref="T:System.Windows.Controls.CheckBox" />.</summary>
		/// <returns>An <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> for the <see cref="T:System.Windows.Controls.CheckBox" />.</returns>
		// Token: 0x0600433F RID: 17215 RVA: 0x00133AAA File Offset: 0x00131CAA
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new CheckBoxAutomationPeer(this);
		}

		/// <summary> Responds to a <see cref="T:System.Windows.Controls.CheckBox" /><see cref="E:System.Windows.UIElement.KeyDown" /> event. </summary>
		/// <param name="e">The <see cref="T:System.Windows.Input.KeyEventArgs" /> that contains the event data. </param>
		// Token: 0x06004340 RID: 17216 RVA: 0x00133AB4 File Offset: 0x00131CB4
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (!base.IsThreeState)
			{
				if (e.Key == Key.OemPlus || e.Key == Key.Add)
				{
					e.Handled = true;
					base.ClearValue(ButtonBase.IsPressedPropertyKey);
					base.SetCurrentValueInternal(ToggleButton.IsCheckedProperty, BooleanBoxes.TrueBox);
					return;
				}
				if (e.Key == Key.OemMinus || e.Key == Key.Subtract)
				{
					e.Handled = true;
					base.ClearValue(ButtonBase.IsPressedPropertyKey);
					base.SetCurrentValueInternal(ToggleButton.IsCheckedProperty, BooleanBoxes.FalseBox);
				}
			}
		}

		/// <summary>Called when the access key for a <see cref="T:System.Windows.Controls.CheckBox" /> is invoked. </summary>
		/// <param name="e">The <see cref="T:System.Windows.Input.AccessKeyEventArgs" /> that contains the event data.</param>
		// Token: 0x06004341 RID: 17217 RVA: 0x00133B43 File Offset: 0x00131D43
		protected override void OnAccessKey(AccessKeyEventArgs e)
		{
			if (!base.IsKeyboardFocused)
			{
				base.Focus();
			}
			base.OnAccessKey(e);
		}

		// Token: 0x1700107F RID: 4223
		// (get) Token: 0x06004342 RID: 17218 RVA: 0x00133B5B File Offset: 0x00131D5B
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return CheckBox._dType;
			}
		}

		// Token: 0x0400283C RID: 10300
		private static DependencyObjectType _dType;
	}
}
