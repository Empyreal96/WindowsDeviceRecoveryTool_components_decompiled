using System;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	/// <summary>Provides a simple way to create a control.</summary>
	// Token: 0x0200054F RID: 1359
	public class UserControl : ContentControl
	{
		// Token: 0x06005968 RID: 22888 RVA: 0x0018B030 File Offset: 0x00189230
		static UserControl()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(UserControl), new FrameworkPropertyMetadata(typeof(UserControl)));
			UserControl._dType = DependencyObjectType.FromSystemTypeInternal(typeof(UserControl));
			UIElement.FocusableProperty.OverrideMetadata(typeof(UserControl), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			KeyboardNavigation.IsTabStopProperty.OverrideMetadata(typeof(UserControl), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			Control.HorizontalContentAlignmentProperty.OverrideMetadata(typeof(UserControl), new FrameworkPropertyMetadata(HorizontalAlignment.Stretch));
			Control.VerticalContentAlignmentProperty.OverrideMetadata(typeof(UserControl), new FrameworkPropertyMetadata(VerticalAlignment.Stretch));
		}

		// Token: 0x0600596A RID: 22890 RVA: 0x0018B0EE File Offset: 0x001892EE
		internal override void AdjustBranchSource(RoutedEventArgs e)
		{
			e.Source = this;
		}

		/// <summary>Creates and returns an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> for this <see cref="T:System.Windows.Controls.UserControl" />.</summary>
		/// <returns>A new <see cref="T:System.Windows.Automation.Peers.UserControlAutomationPeer" /> for this <see cref="T:System.Windows.Controls.UserControl" />.</returns>
		// Token: 0x0600596B RID: 22891 RVA: 0x0018B0F7 File Offset: 0x001892F7
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new UserControlAutomationPeer(this);
		}

		// Token: 0x170015C1 RID: 5569
		// (get) Token: 0x0600596C RID: 22892 RVA: 0x001422C1 File Offset: 0x001404C1
		internal override FrameworkElement StateGroupsRoot
		{
			get
			{
				return base.Content as FrameworkElement;
			}
		}

		// Token: 0x170015C2 RID: 5570
		// (get) Token: 0x0600596D RID: 22893 RVA: 0x0018B0FF File Offset: 0x001892FF
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return UserControl._dType;
			}
		}

		// Token: 0x04002EFF RID: 12031
		private static DependencyObjectType _dType;
	}
}
