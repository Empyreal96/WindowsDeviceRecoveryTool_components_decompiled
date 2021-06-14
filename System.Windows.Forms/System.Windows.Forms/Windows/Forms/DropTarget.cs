using System;
using System.Runtime.InteropServices.ComTypes;

namespace System.Windows.Forms
{
	// Token: 0x02000238 RID: 568
	internal class DropTarget : UnsafeNativeMethods.IOleDropTarget
	{
		// Token: 0x060021BA RID: 8634 RVA: 0x000A5128 File Offset: 0x000A3328
		public DropTarget(IDropTarget owner)
		{
			this.owner = owner;
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x000A5138 File Offset: 0x000A3338
		private DragEventArgs CreateDragEventArgs(object pDataObj, int grfKeyState, NativeMethods.POINTL pt, int pdwEffect)
		{
			IDataObject data;
			if (pDataObj == null)
			{
				data = this.lastDataObject;
			}
			else if (pDataObj is IDataObject)
			{
				data = (IDataObject)pDataObj;
			}
			else
			{
				if (!(pDataObj is IDataObject))
				{
					return null;
				}
				data = new DataObject(pDataObj);
			}
			DragEventArgs result = new DragEventArgs(data, grfKeyState, pt.x, pt.y, (DragDropEffects)pdwEffect, this.lastEffect);
			this.lastDataObject = data;
			return result;
		}

		// Token: 0x060021BC RID: 8636 RVA: 0x000A519C File Offset: 0x000A339C
		int UnsafeNativeMethods.IOleDropTarget.OleDragEnter(object pDataObj, int grfKeyState, UnsafeNativeMethods.POINTSTRUCT pt, ref int pdwEffect)
		{
			DragEventArgs dragEventArgs = this.CreateDragEventArgs(pDataObj, grfKeyState, new NativeMethods.POINTL
			{
				x = pt.x,
				y = pt.y
			}, pdwEffect);
			if (dragEventArgs != null)
			{
				this.owner.OnDragEnter(dragEventArgs);
				pdwEffect = (int)dragEventArgs.Effect;
				this.lastEffect = dragEventArgs.Effect;
			}
			else
			{
				pdwEffect = 0;
			}
			return 0;
		}

		// Token: 0x060021BD RID: 8637 RVA: 0x000A5200 File Offset: 0x000A3400
		int UnsafeNativeMethods.IOleDropTarget.OleDragOver(int grfKeyState, UnsafeNativeMethods.POINTSTRUCT pt, ref int pdwEffect)
		{
			DragEventArgs dragEventArgs = this.CreateDragEventArgs(null, grfKeyState, new NativeMethods.POINTL
			{
				x = pt.x,
				y = pt.y
			}, pdwEffect);
			this.owner.OnDragOver(dragEventArgs);
			pdwEffect = (int)dragEventArgs.Effect;
			this.lastEffect = dragEventArgs.Effect;
			return 0;
		}

		// Token: 0x060021BE RID: 8638 RVA: 0x000A5258 File Offset: 0x000A3458
		int UnsafeNativeMethods.IOleDropTarget.OleDragLeave()
		{
			this.owner.OnDragLeave(EventArgs.Empty);
			return 0;
		}

		// Token: 0x060021BF RID: 8639 RVA: 0x000A526C File Offset: 0x000A346C
		int UnsafeNativeMethods.IOleDropTarget.OleDrop(object pDataObj, int grfKeyState, UnsafeNativeMethods.POINTSTRUCT pt, ref int pdwEffect)
		{
			DragEventArgs dragEventArgs = this.CreateDragEventArgs(pDataObj, grfKeyState, new NativeMethods.POINTL
			{
				x = pt.x,
				y = pt.y
			}, pdwEffect);
			if (dragEventArgs != null)
			{
				this.owner.OnDragDrop(dragEventArgs);
				pdwEffect = (int)dragEventArgs.Effect;
			}
			else
			{
				pdwEffect = 0;
			}
			this.lastEffect = DragDropEffects.None;
			this.lastDataObject = null;
			return 0;
		}

		// Token: 0x04000EAD RID: 3757
		private IDataObject lastDataObject;

		// Token: 0x04000EAE RID: 3758
		private DragDropEffects lastEffect;

		// Token: 0x04000EAF RID: 3759
		private IDropTarget owner;
	}
}
