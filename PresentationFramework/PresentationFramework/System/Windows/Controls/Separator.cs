using System;
using System.Windows.Automation.Peers;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary> Control that is used to separate items in items controls. </summary>
	// Token: 0x02000531 RID: 1329
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public class Separator : Control
	{
		// Token: 0x060055F8 RID: 22008 RVA: 0x0017D208 File Offset: 0x0017B408
		static Separator()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Separator), new FrameworkPropertyMetadata(typeof(Separator)));
			Separator._dType = DependencyObjectType.FromSystemTypeInternal(typeof(Separator));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(Separator), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			ControlsTraceLogger.AddControl(TelemetryControls.Separator);
		}

		// Token: 0x060055F9 RID: 22009 RVA: 0x0017D275 File Offset: 0x0017B475
		internal static void PrepareContainer(Control container)
		{
			if (container != null)
			{
				container.IsEnabled = false;
				container.HorizontalContentAlignment = HorizontalAlignment.Stretch;
			}
		}

		/// <summary>Provides an appropriate <see cref="T:System.Windows.Automation.Peers.SeparatorAutomationPeer" /> implementation for this control, as part of the WPF automation infrastructure.</summary>
		/// <returns>The type-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> implementation.</returns>
		// Token: 0x060055FA RID: 22010 RVA: 0x0017D288 File Offset: 0x0017B488
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new SeparatorAutomationPeer(this);
		}

		// Token: 0x170014E4 RID: 5348
		// (get) Token: 0x060055FB RID: 22011 RVA: 0x0017D290 File Offset: 0x0017B490
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return Separator._dType;
			}
		}

		// Token: 0x04002E22 RID: 11810
		private static DependencyObjectType _dType;
	}
}
