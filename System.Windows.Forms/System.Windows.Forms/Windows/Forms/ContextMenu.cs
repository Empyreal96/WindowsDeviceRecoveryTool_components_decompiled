using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Represents a shortcut menu. Although <see cref="T:System.Windows.Forms.ContextMenuStrip" /> replaces and adds functionality to the <see cref="T:System.Windows.Forms.ContextMenu" /> control of previous versions, <see cref="T:System.Windows.Forms.ContextMenu" /> is retained for both backward compatibility and future use if you choose.</summary>
	// Token: 0x02000157 RID: 343
	[DefaultEvent("Popup")]
	public class ContextMenu : Menu
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ContextMenu" /> class with no menu items specified.</summary>
		// Token: 0x06000BF0 RID: 3056 RVA: 0x00026749 File Offset: 0x00024949
		public ContextMenu() : base(null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ContextMenu" /> class with a specified set of <see cref="T:System.Windows.Forms.MenuItem" /> objects.</summary>
		/// <param name="menuItems">An array of <see cref="T:System.Windows.Forms.MenuItem" /> objects that represent the menu items to add to the shortcut menu. </param>
		// Token: 0x06000BF1 RID: 3057 RVA: 0x00026759 File Offset: 0x00024959
		public ContextMenu(MenuItem[] menuItems) : base(menuItems)
		{
		}

		/// <summary>Gets the control that is displaying the shortcut menu.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Control" /> that represents the control that is displaying the shortcut menu. If no control has displayed the shortcut menu, the property returns <see langword="null" />.</returns>
		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000BF2 RID: 3058 RVA: 0x00026769 File Offset: 0x00024969
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ContextMenuSourceControlDescr")]
		public Control SourceControl
		{
			[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
			get
			{
				return this.sourceControl;
			}
		}

		/// <summary>Occurs before the shortcut menu is displayed.</summary>
		// Token: 0x14000067 RID: 103
		// (add) Token: 0x06000BF3 RID: 3059 RVA: 0x00026771 File Offset: 0x00024971
		// (remove) Token: 0x06000BF4 RID: 3060 RVA: 0x0002678A File Offset: 0x0002498A
		[SRDescription("MenuItemOnInitDescr")]
		public event EventHandler Popup
		{
			add
			{
				this.onPopup = (EventHandler)Delegate.Combine(this.onPopup, value);
			}
			remove
			{
				this.onPopup = (EventHandler)Delegate.Remove(this.onPopup, value);
			}
		}

		/// <summary>Occurs when the shortcut menu collapses.</summary>
		// Token: 0x14000068 RID: 104
		// (add) Token: 0x06000BF5 RID: 3061 RVA: 0x000267A3 File Offset: 0x000249A3
		// (remove) Token: 0x06000BF6 RID: 3062 RVA: 0x000267BC File Offset: 0x000249BC
		[SRDescription("ContextMenuCollapseDescr")]
		public event EventHandler Collapse
		{
			add
			{
				this.onCollapse = (EventHandler)Delegate.Combine(this.onCollapse, value);
			}
			remove
			{
				this.onCollapse = (EventHandler)Delegate.Remove(this.onCollapse, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether text displayed by the control is displayed from right to left.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.RightToLeft" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned to the property is not a valid member of the <see cref="T:System.Windows.Forms.RightToLeft" /> enumeration. </exception>
		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000BF7 RID: 3063 RVA: 0x000267D5 File Offset: 0x000249D5
		// (set) Token: 0x06000BF8 RID: 3064 RVA: 0x000267FC File Offset: 0x000249FC
		[Localizable(true)]
		[DefaultValue(RightToLeft.No)]
		[SRDescription("MenuRightToLeftDescr")]
		public virtual RightToLeft RightToLeft
		{
			get
			{
				if (RightToLeft.Inherit != this.rightToLeft)
				{
					return this.rightToLeft;
				}
				if (this.sourceControl != null)
				{
					return this.sourceControl.RightToLeft;
				}
				return RightToLeft.No;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("RightToLeft", (int)value, typeof(RightToLeft));
				}
				if (this.RightToLeft != value)
				{
					this.rightToLeft = value;
					base.UpdateRtl(value == RightToLeft.Yes);
				}
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000BF9 RID: 3065 RVA: 0x00026849 File Offset: 0x00024A49
		internal override bool RenderIsRightToLeft
		{
			get
			{
				return this.rightToLeft == RightToLeft.Yes;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ContextMenu.Popup" /> event </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000BFA RID: 3066 RVA: 0x00026854 File Offset: 0x00024A54
		protected internal virtual void OnPopup(EventArgs e)
		{
			if (this.onPopup != null)
			{
				this.onPopup(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ContextMenu.Collapse" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000BFB RID: 3067 RVA: 0x0002686B File Offset: 0x00024A6B
		protected internal virtual void OnCollapse(EventArgs e)
		{
			if (this.onCollapse != null)
			{
				this.onCollapse(this, e);
			}
		}

		/// <summary>Processes a command key.</summary>
		/// <param name="msg">A <see cref="T:System.Windows.Forms.Message" />, passed by reference, that represents the window message to process. </param>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process. </param>
		/// <param name="control">The control to which the command key applies.</param>
		/// <returns>
		///     <see langword="true" /> if the character was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000BFC RID: 3068 RVA: 0x00026882 File Offset: 0x00024A82
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected internal virtual bool ProcessCmdKey(ref Message msg, Keys keyData, Control control)
		{
			this.sourceControl = control;
			return this.ProcessCmdKey(ref msg, keyData);
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x00026893 File Offset: 0x00024A93
		private void ResetRightToLeft()
		{
			this.RightToLeft = RightToLeft.No;
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x0002689C File Offset: 0x00024A9C
		internal virtual bool ShouldSerializeRightToLeft()
		{
			return RightToLeft.Inherit != this.rightToLeft;
		}

		/// <summary>Displays the shortcut menu at the specified position.</summary>
		/// <param name="control">A <see cref="T:System.Windows.Forms.Control" /> that specifies the control with which this shortcut menu is associated. </param>
		/// <param name="pos">A <see cref="T:System.Drawing.Point" /> that specifies the coordinates at which to display the menu. These coordinates are specified relative to the client coordinates of the control specified in the <paramref name="control" /> parameter. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="control" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The handle of the control does not exist or the control is not visible.</exception>
		// Token: 0x06000BFF RID: 3071 RVA: 0x000268AA File Offset: 0x00024AAA
		public void Show(Control control, Point pos)
		{
			this.Show(control, pos, 66);
		}

		/// <summary>Displays the shortcut menu at the specified position and with the specified alignment.</summary>
		/// <param name="control">A <see cref="T:System.Windows.Forms.Control" /> that specifies the control with which this shortcut menu is associated.</param>
		/// <param name="pos">A <see cref="T:System.Drawing.Point" /> that specifies the coordinates at which to display the menu. These coordinates are specified relative to the client coordinates of the control specified in the <paramref name="control" /> parameter.</param>
		/// <param name="alignment">A <see cref="T:System.Windows.Forms.LeftRightAlignment" /> that specifies the alignment of the control relative to the <paramref name="pos" /> parameter.</param>
		// Token: 0x06000C00 RID: 3072 RVA: 0x000268B6 File Offset: 0x00024AB6
		public void Show(Control control, Point pos, LeftRightAlignment alignment)
		{
			if (alignment == LeftRightAlignment.Left)
			{
				this.Show(control, pos, 74);
				return;
			}
			this.Show(control, pos, 66);
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x000268D0 File Offset: 0x00024AD0
		private void Show(Control control, Point pos, int flags)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			if (!control.IsHandleCreated || !control.Visible)
			{
				throw new ArgumentException(SR.GetString("ContextMenuInvalidParent"), "control");
			}
			this.sourceControl = control;
			this.OnPopup(EventArgs.Empty);
			pos = control.PointToScreen(pos);
			SafeNativeMethods.TrackPopupMenuEx(new HandleRef(this, base.Handle), flags, pos.X, pos.Y, new HandleRef(control, control.Handle), null);
		}

		// Token: 0x04000764 RID: 1892
		private EventHandler onPopup;

		// Token: 0x04000765 RID: 1893
		private EventHandler onCollapse;

		// Token: 0x04000766 RID: 1894
		internal Control sourceControl;

		// Token: 0x04000767 RID: 1895
		private RightToLeft rightToLeft = RightToLeft.Inherit;
	}
}
