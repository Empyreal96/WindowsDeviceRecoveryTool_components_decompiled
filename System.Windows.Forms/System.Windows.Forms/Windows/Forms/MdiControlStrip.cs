using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Windows.Forms
{
	// Token: 0x020002DB RID: 731
	internal class MdiControlStrip : MenuStrip
	{
		// Token: 0x06002C22 RID: 11298 RVA: 0x000CDD48 File Offset: 0x000CBF48
		public MdiControlStrip(IWin32Window target)
		{
			IntPtr systemMenu = UnsafeNativeMethods.GetSystemMenu(new HandleRef(this, Control.GetSafeHandle(target)), false);
			this.target = target;
			this.minimize = new MdiControlStrip.ControlBoxMenuItem(systemMenu, 61472, target);
			this.close = new MdiControlStrip.ControlBoxMenuItem(systemMenu, 61536, target);
			this.restore = new MdiControlStrip.ControlBoxMenuItem(systemMenu, 61728, target);
			this.system = new MdiControlStrip.SystemMenuItem();
			Control control = target as Control;
			if (control != null)
			{
				control.HandleCreated += this.OnTargetWindowHandleRecreated;
				control.Disposed += this.OnTargetWindowDisposed;
			}
			this.Items.AddRange(new ToolStripItem[]
			{
				this.minimize,
				this.restore,
				this.close,
				this.system
			});
			base.SuspendLayout();
			foreach (object obj in this.Items)
			{
				ToolStripItem toolStripItem = (ToolStripItem)obj;
				toolStripItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
				toolStripItem.MergeIndex = 0;
				toolStripItem.MergeAction = MergeAction.Insert;
				toolStripItem.Overflow = ToolStripItemOverflow.Never;
				toolStripItem.Alignment = ToolStripItemAlignment.Right;
				toolStripItem.Padding = Padding.Empty;
				toolStripItem.ImageScaling = ToolStripItemImageScaling.SizeToFit;
			}
			this.system.Image = this.GetTargetWindowIcon();
			this.system.Alignment = ToolStripItemAlignment.Left;
			this.system.DropDownOpening += this.OnSystemMenuDropDownOpening;
			this.system.ImageScaling = ToolStripItemImageScaling.None;
			this.system.DoubleClickEnabled = true;
			this.system.DoubleClick += this.OnSystemMenuDoubleClick;
			this.system.Padding = Padding.Empty;
			this.system.ShortcutKeys = (Keys.LButton | Keys.MButton | Keys.Back | Keys.ShiftKey | Keys.Space | Keys.F17 | Keys.Alt);
			base.ResumeLayout(false);
		}

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x06002C23 RID: 11299 RVA: 0x000CDF28 File Offset: 0x000CC128
		public ToolStripMenuItem Close
		{
			get
			{
				return this.close;
			}
		}

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x06002C24 RID: 11300 RVA: 0x000CDF30 File Offset: 0x000CC130
		// (set) Token: 0x06002C25 RID: 11301 RVA: 0x000CDF38 File Offset: 0x000CC138
		internal MenuStrip MergedMenu
		{
			get
			{
				return this.mergedMenu;
			}
			set
			{
				this.mergedMenu = value;
			}
		}

		// Token: 0x06002C26 RID: 11302 RVA: 0x000CDF44 File Offset: 0x000CC144
		private Image GetTargetWindowIcon()
		{
			Image result = null;
			IntPtr intPtr = UnsafeNativeMethods.SendMessage(new HandleRef(this, Control.GetSafeHandle(this.target)), 127, 0, 0);
			IntSecurity.ObjectFromWin32Handle.Assert();
			try
			{
				Icon original = (intPtr != IntPtr.Zero) ? Icon.FromHandle(intPtr) : Form.DefaultIcon;
				Icon icon = new Icon(original, SystemInformation.SmallIconSize);
				result = icon.ToBitmap();
				icon.Dispose();
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return result;
		}

		// Token: 0x06002C27 RID: 11303 RVA: 0x000CDFC8 File Offset: 0x000CC1C8
		protected internal override void OnItemAdded(ToolStripItemEventArgs e)
		{
			base.OnItemAdded(e);
		}

		// Token: 0x06002C28 RID: 11304 RVA: 0x000CDFD1 File Offset: 0x000CC1D1
		private void OnTargetWindowDisposed(object sender, EventArgs e)
		{
			this.UnhookTarget();
			this.target = null;
		}

		// Token: 0x06002C29 RID: 11305 RVA: 0x000CDFE0 File Offset: 0x000CC1E0
		private void OnTargetWindowHandleRecreated(object sender, EventArgs e)
		{
			this.system.SetNativeTargetWindow(this.target);
			this.minimize.SetNativeTargetWindow(this.target);
			this.close.SetNativeTargetWindow(this.target);
			this.restore.SetNativeTargetWindow(this.target);
			IntPtr systemMenu = UnsafeNativeMethods.GetSystemMenu(new HandleRef(this, Control.GetSafeHandle(this.target)), false);
			this.system.SetNativeTargetMenu(systemMenu);
			this.minimize.SetNativeTargetMenu(systemMenu);
			this.close.SetNativeTargetMenu(systemMenu);
			this.restore.SetNativeTargetMenu(systemMenu);
			if (this.system.HasDropDownItems)
			{
				this.system.DropDown.Items.Clear();
				this.system.DropDown.Dispose();
			}
			this.system.Image = this.GetTargetWindowIcon();
		}

		// Token: 0x06002C2A RID: 11306 RVA: 0x000CE0BC File Offset: 0x000CC2BC
		private void OnSystemMenuDropDownOpening(object sender, EventArgs e)
		{
			if (!this.system.HasDropDownItems && this.target != null)
			{
				this.system.DropDown = ToolStripDropDownMenu.FromHMenu(UnsafeNativeMethods.GetSystemMenu(new HandleRef(this, Control.GetSafeHandle(this.target)), false), this.target);
				return;
			}
			if (this.MergedMenu == null)
			{
				this.system.DropDown.Dispose();
			}
		}

		// Token: 0x06002C2B RID: 11307 RVA: 0x000CE124 File Offset: 0x000CC324
		private void OnSystemMenuDoubleClick(object sender, EventArgs e)
		{
			this.Close.PerformClick();
		}

		// Token: 0x06002C2C RID: 11308 RVA: 0x000CE131 File Offset: 0x000CC331
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.UnhookTarget();
				this.target = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x06002C2D RID: 11309 RVA: 0x000CE14C File Offset: 0x000CC34C
		private void UnhookTarget()
		{
			if (this.target != null)
			{
				Control control = this.target as Control;
				if (control != null)
				{
					control.HandleCreated -= this.OnTargetWindowHandleRecreated;
					control.Disposed -= this.OnTargetWindowDisposed;
				}
				this.target = null;
			}
		}

		// Token: 0x040012D7 RID: 4823
		private ToolStripMenuItem system;

		// Token: 0x040012D8 RID: 4824
		private ToolStripMenuItem close;

		// Token: 0x040012D9 RID: 4825
		private ToolStripMenuItem minimize;

		// Token: 0x040012DA RID: 4826
		private ToolStripMenuItem restore;

		// Token: 0x040012DB RID: 4827
		private MenuStrip mergedMenu;

		// Token: 0x040012DC RID: 4828
		private IWin32Window target;

		// Token: 0x02000617 RID: 1559
		internal class ControlBoxMenuItem : ToolStripMenuItem
		{
			// Token: 0x06005DDA RID: 24026 RVA: 0x00185B0B File Offset: 0x00183D0B
			internal ControlBoxMenuItem(IntPtr hMenu, int nativeMenuCommandId, IWin32Window targetWindow) : base(hMenu, nativeMenuCommandId, targetWindow)
			{
			}

			// Token: 0x17001686 RID: 5766
			// (get) Token: 0x06005DDB RID: 24027 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			internal override bool CanKeyboardSelect
			{
				get
				{
					return false;
				}
			}
		}

		// Token: 0x02000618 RID: 1560
		internal class SystemMenuItem : ToolStripMenuItem
		{
			// Token: 0x06005DDC RID: 24028 RVA: 0x00185B16 File Offset: 0x00183D16
			public SystemMenuItem()
			{
				if (AccessibilityImprovements.Level1)
				{
					base.AccessibleName = SR.GetString("MDIChildSystemMenuItemAccessibleName");
				}
			}

			// Token: 0x06005DDD RID: 24029 RVA: 0x00185B35 File Offset: 0x00183D35
			protected internal override bool ProcessCmdKey(ref Message m, Keys keyData)
			{
				if (base.Visible && base.ShortcutKeys == keyData)
				{
					base.ShowDropDown();
					base.DropDown.SelectNextToolStripItem(null, true);
					return true;
				}
				return base.ProcessCmdKey(ref m, keyData);
			}

			// Token: 0x06005DDE RID: 24030 RVA: 0x00185B66 File Offset: 0x00183D66
			protected override void OnOwnerChanged(EventArgs e)
			{
				if (this.HasDropDownItems && base.DropDown.Visible)
				{
					base.HideDropDown();
				}
				base.OnOwnerChanged(e);
			}
		}
	}
}
