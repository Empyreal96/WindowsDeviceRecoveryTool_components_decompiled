using System;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Represents a control that creates a container that has a border and a header for user interface (UI) content.</summary>
	// Token: 0x020004E3 RID: 1251
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public class GroupBox : HeaderedContentControl
	{
		// Token: 0x06004E05 RID: 19973 RVA: 0x0015FC28 File Offset: 0x0015DE28
		static GroupBox()
		{
			UIElement.FocusableProperty.OverrideMetadata(typeof(GroupBox), new FrameworkPropertyMetadata(false));
			Control.IsTabStopProperty.OverrideMetadata(typeof(GroupBox), new FrameworkPropertyMetadata(false));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(GroupBox), new FrameworkPropertyMetadata(typeof(GroupBox)));
			EventManager.RegisterClassHandler(typeof(GroupBox), AccessKeyManager.AccessKeyPressedEvent, new AccessKeyPressedEventHandler(GroupBox.OnAccessKeyPressed));
			ControlsTraceLogger.AddControl(TelemetryControls.GroupBox);
		}

		/// <summary>Creates an implementation of <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> for the <see cref="T:System.Windows.Controls.GroupBox" /> control.</summary>
		/// <returns>A <see cref="T:System.Windows.Automation.Peers.GroupBoxAutomationPeer" /> for the <see cref="T:System.Windows.Controls.GroupBox" />.</returns>
		// Token: 0x06004E06 RID: 19974 RVA: 0x0015FCC1 File Offset: 0x0015DEC1
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new GroupBoxAutomationPeer(this);
		}

		/// <summary>Responds when the <see cref="P:System.Windows.Controls.AccessText.AccessKey" /> for the <see cref="T:System.Windows.Controls.GroupBox" /> is pressed.</summary>
		/// <param name="e">The event information.</param>
		// Token: 0x06004E07 RID: 19975 RVA: 0x0015FCC9 File Offset: 0x0015DEC9
		protected override void OnAccessKey(AccessKeyEventArgs e)
		{
			this.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
		}

		// Token: 0x06004E08 RID: 19976 RVA: 0x0015FCD8 File Offset: 0x0015DED8
		private static void OnAccessKeyPressed(object sender, AccessKeyPressedEventArgs e)
		{
			if (!e.Handled && e.Scope == null && e.Target == null)
			{
				e.Target = (sender as GroupBox);
			}
		}
	}
}
