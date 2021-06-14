using System;

namespace System.Windows.Controls
{
	/// <summary>Provides event information for events that occur when a tooltip opens or closes.</summary>
	// Token: 0x02000549 RID: 1353
	public sealed class ToolTipEventArgs : RoutedEventArgs
	{
		// Token: 0x060058C1 RID: 22721 RVA: 0x001890C1 File Offset: 0x001872C1
		internal ToolTipEventArgs(bool opening)
		{
			if (opening)
			{
				base.RoutedEvent = ToolTipService.ToolTipOpeningEvent;
				return;
			}
			base.RoutedEvent = ToolTipService.ToolTipClosingEvent;
		}

		// Token: 0x060058C2 RID: 22722 RVA: 0x001890E4 File Offset: 0x001872E4
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			ToolTipEventHandler toolTipEventHandler = (ToolTipEventHandler)genericHandler;
			toolTipEventHandler(genericTarget, this);
		}
	}
}
