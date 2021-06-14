using System;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using MS.Internal.Commands;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Represents a Windows button control, which reacts to the <see cref="E:System.Windows.Controls.Primitives.ButtonBase.Click" /> event.</summary>
	// Token: 0x02000474 RID: 1140
	public class Button : ButtonBase
	{
		// Token: 0x06004280 RID: 17024 RVA: 0x00130CC4 File Offset: 0x0012EEC4
		static Button()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Button), new FrameworkPropertyMetadata(typeof(Button)));
			Button._dType = DependencyObjectType.FromSystemTypeInternal(typeof(Button));
			if (ButtonBase.CommandProperty != null)
			{
				UIElement.IsEnabledProperty.OverrideMetadata(typeof(Button), new FrameworkPropertyMetadata(new PropertyChangedCallback(Button.OnIsEnabledChanged)));
			}
			ControlsTraceLogger.AddControl(TelemetryControls.Button);
		}

		/// <summary>Gets or sets a value that indicates whether a <see cref="T:System.Windows.Controls.Button" /> is the default button. A user invokes the default button by pressing the ENTER key.   </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.Button" /> is the default button; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700105A RID: 4186
		// (get) Token: 0x06004282 RID: 17026 RVA: 0x00130DFB File Offset: 0x0012EFFB
		// (set) Token: 0x06004283 RID: 17027 RVA: 0x00130E0D File Offset: 0x0012F00D
		public bool IsDefault
		{
			get
			{
				return (bool)base.GetValue(Button.IsDefaultProperty);
			}
			set
			{
				base.SetValue(Button.IsDefaultProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06004284 RID: 17028 RVA: 0x00130E20 File Offset: 0x0012F020
		private static void OnIsDefaultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Button button = d as Button;
			KeyboardFocusChangedEventHandler keyboardFocusChangedEventHandler = Button.FocusChangedEventHandlerField.GetValue(button);
			if (keyboardFocusChangedEventHandler == null)
			{
				keyboardFocusChangedEventHandler = new KeyboardFocusChangedEventHandler(button.OnFocusChanged);
				Button.FocusChangedEventHandlerField.SetValue(button, keyboardFocusChangedEventHandler);
			}
			if ((bool)e.NewValue)
			{
				AccessKeyManager.Register("\r", button);
				KeyboardNavigation.Current.FocusChanged += keyboardFocusChangedEventHandler;
				button.UpdateIsDefaulted(Keyboard.FocusedElement);
				return;
			}
			AccessKeyManager.Unregister("\r", button);
			KeyboardNavigation.Current.FocusChanged -= keyboardFocusChangedEventHandler;
			button.UpdateIsDefaulted(null);
		}

		// Token: 0x06004285 RID: 17029 RVA: 0x00130EAC File Offset: 0x0012F0AC
		private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Button button = (Button)d;
			if (button.IsDefault)
			{
				button.UpdateIsDefaulted(Keyboard.FocusedElement);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a <see cref="T:System.Windows.Controls.Button" /> is a Cancel button. A user can activate the Cancel button by pressing the ESC key.  </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.Button" /> is a Cancel button; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700105B RID: 4187
		// (get) Token: 0x06004286 RID: 17030 RVA: 0x00130ED3 File Offset: 0x0012F0D3
		// (set) Token: 0x06004287 RID: 17031 RVA: 0x00130EE5 File Offset: 0x0012F0E5
		public bool IsCancel
		{
			get
			{
				return (bool)base.GetValue(Button.IsCancelProperty);
			}
			set
			{
				base.SetValue(Button.IsCancelProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06004288 RID: 17032 RVA: 0x00130EF8 File Offset: 0x0012F0F8
		private static void OnIsCancelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Button element = d as Button;
			if ((bool)e.NewValue)
			{
				AccessKeyManager.Register("\u001b", element);
				return;
			}
			AccessKeyManager.Unregister("\u001b", element);
		}

		/// <summary>Gets a value that indicates whether a <see cref="T:System.Windows.Controls.Button" /> is the button that is activated when a user presses ENTER.   </summary>
		/// <returns>
		///     <see langword="true" /> if the button is activated when the user presses ENTER; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700105C RID: 4188
		// (get) Token: 0x06004289 RID: 17033 RVA: 0x00130F31 File Offset: 0x0012F131
		public bool IsDefaulted
		{
			get
			{
				return (bool)base.GetValue(Button.IsDefaultedProperty);
			}
		}

		// Token: 0x0600428A RID: 17034 RVA: 0x00130F43 File Offset: 0x0012F143
		private void OnFocusChanged(object sender, KeyboardFocusChangedEventArgs e)
		{
			this.UpdateIsDefaulted(Keyboard.FocusedElement);
		}

		// Token: 0x0600428B RID: 17035 RVA: 0x00130F50 File Offset: 0x0012F150
		private void UpdateIsDefaulted(IInputElement focus)
		{
			if (!this.IsDefault || focus == null || !base.IsEnabled)
			{
				base.SetValue(Button.IsDefaultedPropertyKey, BooleanBoxes.FalseBox);
				return;
			}
			DependencyObject dependencyObject = focus as DependencyObject;
			object value = BooleanBoxes.FalseBox;
			try
			{
				AccessKeyPressedEventArgs accessKeyPressedEventArgs = new AccessKeyPressedEventArgs();
				focus.RaiseEvent(accessKeyPressedEventArgs);
				object scope = accessKeyPressedEventArgs.Scope;
				accessKeyPressedEventArgs = new AccessKeyPressedEventArgs();
				base.RaiseEvent(accessKeyPressedEventArgs);
				object scope2 = accessKeyPressedEventArgs.Scope;
				if (scope2 == scope && (dependencyObject == null || !(bool)dependencyObject.GetValue(KeyboardNavigation.AcceptsReturnProperty)))
				{
					value = BooleanBoxes.TrueBox;
				}
			}
			finally
			{
				base.SetValue(Button.IsDefaultedPropertyKey, value);
			}
		}

		/// <summary>Creates an appropriate <see cref="T:System.Windows.Automation.Peers.ButtonAutomationPeer" /> for this control as part of the WPF infrastructure.</summary>
		/// <returns>A <see cref="T:System.Windows.Automation.Peers.ButtonAutomationPeer" /> for this control.</returns>
		// Token: 0x0600428C RID: 17036 RVA: 0x00130FF8 File Offset: 0x0012F1F8
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ButtonAutomationPeer(this);
		}

		/// <summary>Called when a <see cref="T:System.Windows.Controls.Button" /> is clicked. </summary>
		// Token: 0x0600428D RID: 17037 RVA: 0x00131000 File Offset: 0x0012F200
		protected override void OnClick()
		{
			if (AutomationPeer.ListenerExists(AutomationEvents.InvokePatternOnInvoked))
			{
				AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(this);
				if (automationPeer != null)
				{
					automationPeer.RaiseAutomationEvent(AutomationEvents.InvokePatternOnInvoked);
				}
			}
			try
			{
				base.OnClick();
			}
			finally
			{
				if (base.Command == null && this.IsCancel)
				{
					CommandHelpers.ExecuteCommand(Window.DialogCancelCommand, null, this);
				}
			}
		}

		// Token: 0x1700105D RID: 4189
		// (get) Token: 0x0600428E RID: 17038 RVA: 0x000957A4 File Offset: 0x000939A4
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 42;
			}
		}

		// Token: 0x1700105E RID: 4190
		// (get) Token: 0x0600428F RID: 17039 RVA: 0x0013105C File Offset: 0x0012F25C
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return Button._dType;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Button.IsDefault" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Button.IsDefault" /> dependency property.</returns>
		// Token: 0x040027F8 RID: 10232
		public static readonly DependencyProperty IsDefaultProperty = DependencyProperty.Register("IsDefault", typeof(bool), typeof(Button), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(Button.OnIsDefaultChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Button.IsCancel" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Button.IsCancel" /> dependency property.</returns>
		// Token: 0x040027F9 RID: 10233
		public static readonly DependencyProperty IsCancelProperty = DependencyProperty.Register("IsCancel", typeof(bool), typeof(Button), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(Button.OnIsCancelChanged)));

		// Token: 0x040027FA RID: 10234
		private static readonly DependencyPropertyKey IsDefaultedPropertyKey = DependencyProperty.RegisterReadOnly("IsDefaulted", typeof(bool), typeof(Button), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Button.IsDefaulted" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Button.IsDefaulted" /> dependency property.</returns>
		// Token: 0x040027FB RID: 10235
		public static readonly DependencyProperty IsDefaultedProperty = Button.IsDefaultedPropertyKey.DependencyProperty;

		// Token: 0x040027FC RID: 10236
		private static readonly UncommonField<KeyboardFocusChangedEventHandler> FocusChangedEventHandlerField = new UncommonField<KeyboardFocusChangedEventHandler>();

		// Token: 0x040027FD RID: 10237
		private static DependencyObjectType _dType;
	}
}
