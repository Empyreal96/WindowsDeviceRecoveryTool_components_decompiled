using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ListView.ItemDrag" /> event of the <see cref="T:System.Windows.Forms.ListView" /> and <see cref="T:System.Windows.Forms.TreeView" /> controls.</summary>
	// Token: 0x0200029D RID: 669
	[ComVisible(true)]
	public class ItemDragEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ItemDragEventArgs" /> class with a specified mouse button.</summary>
		/// <param name="button">A bitwise combination of <see cref="T:System.Windows.Forms.MouseButtons" /> values that indicates which mouse buttons were pressed. </param>
		// Token: 0x060026FD RID: 9981 RVA: 0x000B770A File Offset: 0x000B590A
		public ItemDragEventArgs(MouseButtons button)
		{
			this.button = button;
			this.item = null;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ItemDragEventArgs" /> class with a specified mouse button and the item that is being dragged.</summary>
		/// <param name="button">A bitwise combination of <see cref="T:System.Windows.Forms.MouseButtons" /> values that indicates which mouse buttons were pressed. </param>
		/// <param name="item">The item being dragged. </param>
		// Token: 0x060026FE RID: 9982 RVA: 0x000B7720 File Offset: 0x000B5920
		public ItemDragEventArgs(MouseButtons button, object item)
		{
			this.button = button;
			this.item = item;
		}

		/// <summary>Gets a value that indicates which mouse buttons were pressed during the drag operation.</summary>
		/// <returns>A bitwise combination of <see cref="T:System.Windows.Forms.MouseButtons" /> values.</returns>
		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x060026FF RID: 9983 RVA: 0x000B7736 File Offset: 0x000B5936
		public MouseButtons Button
		{
			get
			{
				return this.button;
			}
		}

		/// <summary>Gets the item that is being dragged.</summary>
		/// <returns>An object that represents the item being dragged.</returns>
		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x06002700 RID: 9984 RVA: 0x000B773E File Offset: 0x000B593E
		public object Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x04001076 RID: 4214
		private readonly MouseButtons button;

		// Token: 0x04001077 RID: 4215
		private readonly object item;
	}
}
