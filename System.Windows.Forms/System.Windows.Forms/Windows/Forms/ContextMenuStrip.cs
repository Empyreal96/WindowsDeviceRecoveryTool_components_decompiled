using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Represents a shortcut menu. </summary>
	// Token: 0x02000158 RID: 344
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultEvent("Opening")]
	[SRDescription("DescriptionContextMenuStrip")]
	public class ContextMenuStrip : ToolStripDropDownMenu
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ContextMenuStrip" /> class and associates it with the specified container.</summary>
		/// <param name="container">A component that implements <see cref="T:System.ComponentModel.IContainer" /> that is the container of the <see cref="T:System.Windows.Forms.ContextMenuStrip" />.</param>
		// Token: 0x06000C02 RID: 3074 RVA: 0x00026959 File Offset: 0x00024B59
		public ContextMenuStrip(IContainer container)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			container.Add(this);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ContextMenuStrip" /> class. </summary>
		// Token: 0x06000C03 RID: 3075 RVA: 0x00026976 File Offset: 0x00024B76
		public ContextMenuStrip()
		{
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.ContextMenuStrip" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x06000C04 RID: 3076 RVA: 0x0002697E File Offset: 0x00024B7E
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		/// <summary>Gets the last control that caused this <see cref="T:System.Windows.Forms.ContextMenuStrip" /> to be displayed.</summary>
		/// <returns>The control that caused this <see cref="T:System.Windows.Forms.ContextMenuStrip" /> to be displayed.</returns>
		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000C05 RID: 3077 RVA: 0x00026987 File Offset: 0x00024B87
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ContextMenuStripSourceControlDescr")]
		public Control SourceControl
		{
			[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
			get
			{
				return base.SourceControlInternal;
			}
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x00026990 File Offset: 0x00024B90
		internal ContextMenuStrip Clone()
		{
			ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
			contextMenuStrip.Events.AddHandlers(base.Events);
			contextMenuStrip.AutoClose = base.AutoClose;
			contextMenuStrip.AutoSize = this.AutoSize;
			contextMenuStrip.Bounds = base.Bounds;
			contextMenuStrip.ImageList = base.ImageList;
			contextMenuStrip.ShowCheckMargin = base.ShowCheckMargin;
			contextMenuStrip.ShowImageMargin = base.ShowImageMargin;
			for (int i = 0; i < this.Items.Count; i++)
			{
				ToolStripItem toolStripItem = this.Items[i];
				if (toolStripItem is ToolStripSeparator)
				{
					contextMenuStrip.Items.Add(new ToolStripSeparator());
				}
				else if (toolStripItem is ToolStripMenuItem)
				{
					ToolStripMenuItem toolStripMenuItem = toolStripItem as ToolStripMenuItem;
					contextMenuStrip.Items.Add(toolStripMenuItem.Clone());
				}
			}
			return contextMenuStrip;
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x00026A5C File Offset: 0x00024C5C
		internal void ShowInternal(Control source, Point location, bool isKeyboardActivated)
		{
			base.Show(source, location);
			if (isKeyboardActivated)
			{
				ToolStripManager.ModalMenuFilter.Instance.ShowUnderlines = true;
			}
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x00026A74 File Offset: 0x00024C74
		internal void ShowInTaskbar(int x, int y)
		{
			base.WorkingAreaConstrained = false;
			Rectangle rectangle = base.CalculateDropDownLocation(new Point(x, y), ToolStripDropDownDirection.AboveLeft);
			Rectangle bounds = Screen.FromRectangle(rectangle).Bounds;
			if (rectangle.Y < bounds.Y)
			{
				rectangle = base.CalculateDropDownLocation(new Point(x, y), ToolStripDropDownDirection.BelowLeft);
			}
			else if (rectangle.X < bounds.X)
			{
				rectangle = base.CalculateDropDownLocation(new Point(x, y), ToolStripDropDownDirection.AboveRight);
			}
			rectangle = WindowsFormsUtils.ConstrainToBounds(bounds, rectangle);
			base.Show(rectangle.X, rectangle.Y);
		}

		/// <summary>Sets the control to the specified visible state.</summary>
		/// <param name="visible">
		///       <see langword="true" /> to make the control visible; otherwise, <see langword="false" />.</param>
		// Token: 0x06000C09 RID: 3081 RVA: 0x00026AFF File Offset: 0x00024CFF
		protected override void SetVisibleCore(bool visible)
		{
			if (!visible)
			{
				base.WorkingAreaConstrained = true;
			}
			base.SetVisibleCore(visible);
		}
	}
}
