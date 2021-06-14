using System;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents an item of a <see cref="T:System.Windows.Controls.Primitives.StatusBar" /> control. </summary>
	// Token: 0x020005AA RID: 1450
	[Localizability(LocalizationCategory.Inherit)]
	public class StatusBarItem : ContentControl
	{
		// Token: 0x06006025 RID: 24613 RVA: 0x001AF70C File Offset: 0x001AD90C
		static StatusBarItem()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(StatusBarItem), new FrameworkPropertyMetadata(typeof(StatusBarItem)));
			StatusBarItem._dType = DependencyObjectType.FromSystemTypeInternal(typeof(StatusBarItem));
			Control.IsTabStopProperty.OverrideMetadata(typeof(StatusBarItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			AutomationProperties.IsOffscreenBehaviorProperty.OverrideMetadata(typeof(StatusBarItem), new FrameworkPropertyMetadata(IsOffscreenBehavior.FromClip));
		}

		/// <summary>Specifies an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> for the <see cref="T:System.Windows.Controls.Primitives.StatusBarItem" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Automation.Peers.StatusBarItemAutomationPeer" /> for this <see cref="T:System.Windows.Controls.Primitives.StatusBarItem" />.</returns>
		// Token: 0x06006026 RID: 24614 RVA: 0x001AF78D File Offset: 0x001AD98D
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new StatusBarItemAutomationPeer(this);
		}

		// Token: 0x17001721 RID: 5921
		// (get) Token: 0x06006027 RID: 24615 RVA: 0x001AF795 File Offset: 0x001AD995
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return StatusBarItem._dType;
			}
		}

		// Token: 0x040030F0 RID: 12528
		private static DependencyObjectType _dType;
	}
}
