using System;
using System.Collections;

namespace System.Windows.Controls
{
	// Token: 0x0200047D RID: 1149
	internal class CalendarSelectionChangedEventArgs : SelectionChangedEventArgs
	{
		// Token: 0x0600432C RID: 17196 RVA: 0x001335CE File Offset: 0x001317CE
		public CalendarSelectionChangedEventArgs(RoutedEvent eventId, IList removedItems, IList addedItems) : base(eventId, removedItems, addedItems)
		{
		}

		// Token: 0x0600432D RID: 17197 RVA: 0x001335DC File Offset: 0x001317DC
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			EventHandler<SelectionChangedEventArgs> eventHandler = genericHandler as EventHandler<SelectionChangedEventArgs>;
			if (eventHandler != null)
			{
				eventHandler(genericTarget, this);
				return;
			}
			base.InvokeEventHandler(genericHandler, genericTarget);
		}
	}
}
