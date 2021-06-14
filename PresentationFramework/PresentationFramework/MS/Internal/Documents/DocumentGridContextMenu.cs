using System;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace MS.Internal.Documents
{
	// Token: 0x020006BD RID: 1725
	internal static class DocumentGridContextMenu
	{
		// Token: 0x06006F8A RID: 28554 RVA: 0x00200FFC File Offset: 0x001FF1FC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static void RegisterClassHandler()
		{
			EventManager.RegisterClassHandler(typeof(DocumentGrid), FrameworkElement.ContextMenuOpeningEvent, new ContextMenuEventHandler(DocumentGridContextMenu.OnContextMenuOpening));
			EventManager.RegisterClassHandler(typeof(DocumentApplicationDocumentViewer), FrameworkElement.ContextMenuOpeningEvent, new ContextMenuEventHandler(DocumentGridContextMenu.OnDocumentViewerContextMenuOpening));
		}

		// Token: 0x06006F8B RID: 28555 RVA: 0x0020104C File Offset: 0x001FF24C
		[SecurityCritical]
		private static void OnDocumentViewerContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			if (e.CursorLeft == -1.0)
			{
				DocumentViewer documentViewer = sender as DocumentViewer;
				if (documentViewer != null && documentViewer.ScrollViewer != null)
				{
					DocumentGridContextMenu.OnContextMenuOpening(documentViewer.ScrollViewer.Content, e);
				}
			}
		}

		// Token: 0x06006F8C RID: 28556 RVA: 0x00201090 File Offset: 0x001FF290
		[SecurityCritical]
		private static void OnContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			DocumentGrid documentGrid = sender as DocumentGrid;
			if (documentGrid == null)
			{
				return;
			}
			if (!(documentGrid.DocumentViewerOwner is DocumentApplicationDocumentViewer))
			{
				return;
			}
			if (documentGrid.DocumentViewerOwner.ContextMenu != null || documentGrid.DocumentViewerOwner.ScrollViewer.ContextMenu != null)
			{
				return;
			}
			ContextMenu contextMenu = documentGrid.ContextMenu;
			if (documentGrid.ReadLocalValue(FrameworkElement.ContextMenuProperty) == null)
			{
				return;
			}
			if (contextMenu != null)
			{
				return;
			}
			contextMenu = new DocumentGridContextMenu.ViewerContextMenu();
			contextMenu.Placement = PlacementMode.RelativePoint;
			contextMenu.PlacementTarget = documentGrid;
			((DocumentGridContextMenu.ViewerContextMenu)contextMenu).AddMenuItems(documentGrid, e.UserInitiated);
			Point position;
			if (e.CursorLeft == -1.0)
			{
				position = new Point(0.5 * documentGrid.ViewportWidth, 0.5 * documentGrid.ViewportHeight);
			}
			else
			{
				position = Mouse.GetPosition(documentGrid);
			}
			contextMenu.HorizontalOffset = position.X;
			contextMenu.VerticalOffset = position.Y;
			contextMenu.IsOpen = true;
			e.Handled = true;
		}

		// Token: 0x040036BD RID: 14013
		private const double KeyboardInvokedSentinel = -1.0;

		// Token: 0x02000B34 RID: 2868
		private class ViewerContextMenu : ContextMenu
		{
			// Token: 0x06008D6B RID: 36203 RVA: 0x00259724 File Offset: 0x00257924
			[SecurityCritical]
			internal void AddMenuItems(DocumentGrid dg, bool userInitiated)
			{
				if (!userInitiated)
				{
					SecurityHelper.DemandAllClipboardPermission();
				}
				base.Name = "ViewerContextMenu";
				this.SetMenuProperties(new DocumentGridContextMenu.EditorMenuItem(), dg, ApplicationCommands.Copy);
				this.SetMenuProperties(new MenuItem(), dg, ApplicationCommands.SelectAll);
				this.AddSeparator();
				this.SetMenuProperties(new MenuItem(), dg, NavigationCommands.PreviousPage, SR.Get("DocumentApplicationContextMenuPreviousPageHeader"), SR.Get("DocumentApplicationContextMenuPreviousPageInputGesture"));
				this.SetMenuProperties(new MenuItem(), dg, NavigationCommands.NextPage, SR.Get("DocumentApplicationContextMenuNextPageHeader"), SR.Get("DocumentApplicationContextMenuNextPageInputGesture"));
				this.SetMenuProperties(new MenuItem(), dg, NavigationCommands.FirstPage, null, SR.Get("DocumentApplicationContextMenuFirstPageInputGesture"));
				this.SetMenuProperties(new MenuItem(), dg, NavigationCommands.LastPage, null, SR.Get("DocumentApplicationContextMenuLastPageInputGesture"));
				this.AddSeparator();
				this.SetMenuProperties(new MenuItem(), dg, ApplicationCommands.Print);
			}

			// Token: 0x06008D6C RID: 36204 RVA: 0x0024A333 File Offset: 0x00248533
			private void AddSeparator()
			{
				base.Items.Add(new Separator());
			}

			// Token: 0x06008D6D RID: 36205 RVA: 0x00259805 File Offset: 0x00257A05
			private void SetMenuProperties(MenuItem menuItem, DocumentGrid dg, RoutedUICommand command)
			{
				this.SetMenuProperties(menuItem, dg, command, null, null);
			}

			// Token: 0x06008D6E RID: 36206 RVA: 0x00259814 File Offset: 0x00257A14
			private void SetMenuProperties(MenuItem menuItem, DocumentGrid dg, RoutedUICommand command, string header, string inputGestureText)
			{
				menuItem.Command = command;
				menuItem.CommandTarget = dg.DocumentViewerOwner;
				if (header == null)
				{
					menuItem.Header = command.Text;
				}
				else
				{
					menuItem.Header = header;
				}
				if (inputGestureText != null)
				{
					menuItem.InputGestureText = inputGestureText;
				}
				menuItem.Name = "ViewerContextMenu_" + command.Name;
				base.Items.Add(menuItem);
			}
		}

		// Token: 0x02000B35 RID: 2869
		private class EditorMenuItem : MenuItem
		{
			// Token: 0x06008D70 RID: 36208 RVA: 0x0024A6C6 File Offset: 0x002488C6
			internal EditorMenuItem()
			{
			}

			// Token: 0x06008D71 RID: 36209 RVA: 0x0024A6CE File Offset: 0x002488CE
			[SecurityCritical]
			internal override void OnClickCore(bool userInitiated)
			{
				base.OnClickImpl(userInitiated);
			}
		}
	}
}
