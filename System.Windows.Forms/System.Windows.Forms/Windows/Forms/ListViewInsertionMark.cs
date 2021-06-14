using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Used to indicate the expected drop location when an item is dragged to a new position in a <see cref="T:System.Windows.Forms.ListView" /> control. This functionality is available only on Windows XP and later.</summary>
	// Token: 0x020002CB RID: 715
	public sealed class ListViewInsertionMark
	{
		// Token: 0x06002AD9 RID: 10969 RVA: 0x000C947F File Offset: 0x000C767F
		internal ListViewInsertionMark(ListView listView)
		{
			this.listView = listView;
		}

		/// <summary>Gets or sets a value indicating whether the insertion mark appears to the right of the item with the index specified by the <see cref="P:System.Windows.Forms.ListViewInsertionMark.Index" /> property.</summary>
		/// <returns>
		///     <see langword="true" /> if the insertion mark appears to the right of the item with the index specified by the <see cref="P:System.Windows.Forms.ListViewInsertionMark.Index" /> property; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x06002ADA RID: 10970 RVA: 0x000C9499 File Offset: 0x000C7699
		// (set) Token: 0x06002ADB RID: 10971 RVA: 0x000C94A1 File Offset: 0x000C76A1
		public bool AppearsAfterItem
		{
			get
			{
				return this.appearsAfterItem;
			}
			set
			{
				if (this.appearsAfterItem != value)
				{
					this.appearsAfterItem = value;
					if (this.listView.IsHandleCreated)
					{
						this.UpdateListView();
					}
				}
			}
		}

		/// <summary>Gets the bounding rectangle of the insertion mark.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the position and size of the insertion mark.</returns>
		// Token: 0x17000A53 RID: 2643
		// (get) Token: 0x06002ADC RID: 10972 RVA: 0x000C94C8 File Offset: 0x000C76C8
		public Rectangle Bounds
		{
			get
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				this.listView.SendMessage(4265, 0, ref rect);
				return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
			}
		}

		/// <summary>Gets or sets the color of the insertion mark.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> value that represents the color of the insertion mark. The default value is the value of the <see cref="P:System.Windows.Forms.ListView.ForeColor" /> property.</returns>
		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x06002ADD RID: 10973 RVA: 0x000C950E File Offset: 0x000C770E
		// (set) Token: 0x06002ADE RID: 10974 RVA: 0x000C9548 File Offset: 0x000C7748
		public Color Color
		{
			get
			{
				if (this.color.IsEmpty)
				{
					this.color = SafeNativeMethods.ColorFromCOLORREF((int)this.listView.SendMessage(4267, 0, 0));
				}
				return this.color;
			}
			set
			{
				if (this.color != value)
				{
					this.color = value;
					if (this.listView.IsHandleCreated)
					{
						this.listView.SendMessage(4266, 0, SafeNativeMethods.ColorToCOLORREF(this.color));
					}
				}
			}
		}

		/// <summary>Gets or sets the index of the item next to which the insertion mark appears.</summary>
		/// <returns>The index of the item next to which the insertion mark appears or -1 when the insertion mark is hidden.</returns>
		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x06002ADF RID: 10975 RVA: 0x000C9594 File Offset: 0x000C7794
		// (set) Token: 0x06002AE0 RID: 10976 RVA: 0x000C959C File Offset: 0x000C779C
		public int Index
		{
			get
			{
				return this.index;
			}
			set
			{
				if (this.index != value)
				{
					this.index = value;
					if (this.listView.IsHandleCreated)
					{
						this.UpdateListView();
					}
				}
			}
		}

		/// <summary>Retrieves the index of the item closest to the specified point.</summary>
		/// <param name="pt">A <see cref="T:System.Drawing.Point" /> representing the location from which to find the nearest item. </param>
		/// <returns>The index of the item closest to the specified point or -1 if the closest item is the item currently being dragged.</returns>
		// Token: 0x06002AE1 RID: 10977 RVA: 0x000C95C4 File Offset: 0x000C77C4
		public int NearestIndex(Point pt)
		{
			NativeMethods.POINT point = new NativeMethods.POINT();
			point.x = pt.X;
			point.y = pt.Y;
			NativeMethods.LVINSERTMARK lvinsertmark = new NativeMethods.LVINSERTMARK();
			UnsafeNativeMethods.SendMessage(new HandleRef(this.listView, this.listView.Handle), 4264, point, lvinsertmark);
			return lvinsertmark.iItem;
		}

		// Token: 0x06002AE2 RID: 10978 RVA: 0x000C9620 File Offset: 0x000C7820
		internal void UpdateListView()
		{
			NativeMethods.LVINSERTMARK lvinsertmark = new NativeMethods.LVINSERTMARK();
			lvinsertmark.dwFlags = (this.appearsAfterItem ? 1 : 0);
			lvinsertmark.iItem = this.index;
			UnsafeNativeMethods.SendMessage(new HandleRef(this.listView, this.listView.Handle), 4262, 0, lvinsertmark);
			if (!this.color.IsEmpty)
			{
				this.listView.SendMessage(4266, 0, SafeNativeMethods.ColorToCOLORREF(this.color));
			}
		}

		// Token: 0x0400126C RID: 4716
		private ListView listView;

		// Token: 0x0400126D RID: 4717
		private int index;

		// Token: 0x0400126E RID: 4718
		private Color color = Color.Empty;

		// Token: 0x0400126F RID: 4719
		private bool appearsAfterItem;
	}
}
