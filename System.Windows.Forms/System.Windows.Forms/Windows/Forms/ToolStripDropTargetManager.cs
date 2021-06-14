using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace System.Windows.Forms
{
	// Token: 0x020003B3 RID: 947
	internal class ToolStripDropTargetManager : IDropTarget
	{
		// Token: 0x06003E77 RID: 15991 RVA: 0x00110A92 File Offset: 0x0010EC92
		public ToolStripDropTargetManager(ToolStrip owner)
		{
			this.owner = owner;
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
		}

		// Token: 0x06003E78 RID: 15992 RVA: 0x00110AAF File Offset: 0x0010ECAF
		public void EnsureRegistered(IDropTarget dropTarget)
		{
			this.SetAcceptDrops(true);
		}

		// Token: 0x06003E79 RID: 15993 RVA: 0x00110AB8 File Offset: 0x0010ECB8
		public void EnsureUnRegistered(IDropTarget dropTarget)
		{
			for (int i = 0; i < this.owner.Items.Count; i++)
			{
				if (this.owner.Items[i].AllowDrop)
				{
					return;
				}
			}
			if (this.owner.AllowDrop || this.owner.AllowItemReorder)
			{
				return;
			}
			this.SetAcceptDrops(false);
			this.owner.DropTargetManager = null;
		}

		// Token: 0x06003E7A RID: 15994 RVA: 0x00110B27 File Offset: 0x0010ED27
		private ToolStripItem FindItemAtPoint(int x, int y)
		{
			return this.owner.GetItemAt(this.owner.PointToClient(new Point(x, y)));
		}

		// Token: 0x06003E7B RID: 15995 RVA: 0x00110B48 File Offset: 0x0010ED48
		public void OnDragEnter(DragEventArgs e)
		{
			if (this.owner.AllowItemReorder && e.Data.GetDataPresent(typeof(ToolStripItem)))
			{
				this.lastDropTarget = this.owner.ItemReorderDropTarget;
			}
			else
			{
				ToolStripItem toolStripItem = this.FindItemAtPoint(e.X, e.Y);
				if (toolStripItem != null && toolStripItem.AllowDrop)
				{
					this.lastDropTarget = toolStripItem;
				}
				else if (this.owner.AllowDrop)
				{
					this.lastDropTarget = this.owner;
				}
				else
				{
					this.lastDropTarget = null;
				}
			}
			if (this.lastDropTarget != null)
			{
				this.lastDropTarget.OnDragEnter(e);
			}
		}

		// Token: 0x06003E7C RID: 15996 RVA: 0x00110BEC File Offset: 0x0010EDEC
		public void OnDragOver(DragEventArgs e)
		{
			IDropTarget dropTarget;
			if (this.owner.AllowItemReorder && e.Data.GetDataPresent(typeof(ToolStripItem)))
			{
				dropTarget = this.owner.ItemReorderDropTarget;
			}
			else
			{
				ToolStripItem toolStripItem = this.FindItemAtPoint(e.X, e.Y);
				if (toolStripItem != null && toolStripItem.AllowDrop)
				{
					dropTarget = toolStripItem;
				}
				else if (this.owner.AllowDrop)
				{
					dropTarget = this.owner;
				}
				else
				{
					dropTarget = null;
				}
			}
			if (dropTarget != this.lastDropTarget)
			{
				this.UpdateDropTarget(dropTarget, e);
			}
			if (this.lastDropTarget != null)
			{
				this.lastDropTarget.OnDragOver(e);
			}
		}

		// Token: 0x06003E7D RID: 15997 RVA: 0x00110C8C File Offset: 0x0010EE8C
		public void OnDragLeave(EventArgs e)
		{
			if (this.lastDropTarget != null)
			{
				this.lastDropTarget.OnDragLeave(e);
			}
			this.lastDropTarget = null;
		}

		// Token: 0x06003E7E RID: 15998 RVA: 0x00110CA9 File Offset: 0x0010EEA9
		public void OnDragDrop(DragEventArgs e)
		{
			if (this.lastDropTarget != null)
			{
				this.lastDropTarget.OnDragDrop(e);
			}
			this.lastDropTarget = null;
		}

		// Token: 0x06003E7F RID: 15999 RVA: 0x00110CC8 File Offset: 0x0010EEC8
		private void SetAcceptDrops(bool accept)
		{
			if (this.owner.AllowDrop && accept)
			{
				IntSecurity.ClipboardRead.Demand();
			}
			if (accept && this.owner.IsHandleCreated)
			{
				try
				{
					if (Application.OleRequired() != ApartmentState.STA)
					{
						throw new ThreadStateException(SR.GetString("ThreadMustBeSTA"));
					}
					if (accept)
					{
						int num = UnsafeNativeMethods.RegisterDragDrop(new HandleRef(this.owner, this.owner.Handle), new DropTarget(this));
						if (num != 0 && num != -2147221247)
						{
							throw new Win32Exception(num);
						}
					}
					else
					{
						IntSecurity.ClipboardRead.Assert();
						try
						{
							int num2 = UnsafeNativeMethods.RevokeDragDrop(new HandleRef(this.owner, this.owner.Handle));
							if (num2 != 0 && num2 != -2147221248)
							{
								throw new Win32Exception(num2);
							}
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
				}
				catch (Exception innerException)
				{
					throw new InvalidOperationException(SR.GetString("DragDropRegFailed"), innerException);
				}
			}
		}

		// Token: 0x06003E80 RID: 16000 RVA: 0x00110DC4 File Offset: 0x0010EFC4
		private void UpdateDropTarget(IDropTarget newTarget, DragEventArgs e)
		{
			if (newTarget != this.lastDropTarget)
			{
				if (this.lastDropTarget != null)
				{
					this.OnDragLeave(new EventArgs());
				}
				this.lastDropTarget = newTarget;
				if (newTarget != null)
				{
					this.OnDragEnter(new DragEventArgs(e.Data, e.KeyState, e.X, e.Y, e.AllowedEffect, e.Effect)
					{
						Effect = DragDropEffects.None
					});
				}
			}
		}

		// Token: 0x04002409 RID: 9225
		private IDropTarget lastDropTarget;

		// Token: 0x0400240A RID: 9226
		private ToolStrip owner;

		// Token: 0x0400240B RID: 9227
		internal static readonly TraceSwitch DragDropDebug;
	}
}
