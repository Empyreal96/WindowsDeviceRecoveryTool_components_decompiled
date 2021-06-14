using System;
using System.Globalization;
using System.Security;

namespace System.Windows.Forms
{
	// Token: 0x020002DC RID: 732
	internal class MdiWindowListStrip : MenuStrip
	{
		// Token: 0x06002C2F RID: 11311 RVA: 0x000CE1A3 File Offset: 0x000CC3A3
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.mdiParent = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x06002C30 RID: 11312 RVA: 0x000CE1B8 File Offset: 0x000CC3B8
		internal ToolStripMenuItem MergeItem
		{
			get
			{
				if (this.mergeItem == null)
				{
					this.mergeItem = new ToolStripMenuItem();
					this.mergeItem.MergeAction = MergeAction.MatchOnly;
				}
				if (this.mergeItem.Owner == null)
				{
					this.Items.Add(this.mergeItem);
				}
				return this.mergeItem;
			}
		}

		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x06002C31 RID: 11313 RVA: 0x000CE209 File Offset: 0x000CC409
		// (set) Token: 0x06002C32 RID: 11314 RVA: 0x000CE211 File Offset: 0x000CC411
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

		// Token: 0x06002C33 RID: 11315 RVA: 0x000CE21C File Offset: 0x000CC41C
		public void PopulateItems(Form mdiParent, ToolStripMenuItem mdiMergeItem, bool includeSeparator)
		{
			this.mdiParent = mdiParent;
			base.SuspendLayout();
			this.MergeItem.DropDown.SuspendLayout();
			try
			{
				ToolStripMenuItem toolStripMenuItem = this.MergeItem;
				toolStripMenuItem.DropDownItems.Clear();
				toolStripMenuItem.Text = mdiMergeItem.Text;
				Form[] mdiChildren = mdiParent.MdiChildren;
				if (mdiChildren != null && mdiChildren.Length != 0)
				{
					if (includeSeparator)
					{
						ToolStripSeparator toolStripSeparator = new ToolStripSeparator();
						toolStripSeparator.MergeAction = MergeAction.Append;
						toolStripSeparator.MergeIndex = -1;
						toolStripMenuItem.DropDownItems.Add(toolStripSeparator);
					}
					Form activeMdiChild = mdiParent.ActiveMdiChild;
					int num = 0;
					int num2 = 1;
					int num3 = 0;
					bool flag = false;
					for (int i = 0; i < mdiChildren.Length; i++)
					{
						if (mdiChildren[i].Visible && mdiChildren[i].CloseReason == CloseReason.None)
						{
							num++;
							if ((flag && num3 < 9) || (!flag && num3 < 8) || mdiChildren[i].Equals(activeMdiChild))
							{
								string text = WindowsFormsUtils.EscapeTextWithAmpersands(mdiParent.MdiChildren[i].Text);
								text = ((text == null) ? string.Empty : text);
								ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem(mdiParent.MdiChildren[i]);
								toolStripMenuItem2.Text = string.Format(CultureInfo.CurrentCulture, "&{0} {1}", new object[]
								{
									num2,
									text
								});
								toolStripMenuItem2.MergeAction = MergeAction.Append;
								toolStripMenuItem2.MergeIndex = num2;
								toolStripMenuItem2.Click += this.OnWindowListItemClick;
								if (mdiChildren[i].Equals(activeMdiChild))
								{
									toolStripMenuItem2.Checked = true;
									flag = true;
								}
								num2++;
								num3++;
								toolStripMenuItem.DropDownItems.Add(toolStripMenuItem2);
							}
						}
					}
					if (num > 9)
					{
						ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem();
						toolStripMenuItem3.Text = SR.GetString("MDIMenuMoreWindows");
						toolStripMenuItem3.Click += this.OnMoreWindowsMenuItemClick;
						toolStripMenuItem3.MergeAction = MergeAction.Append;
						toolStripMenuItem.DropDownItems.Add(toolStripMenuItem3);
					}
				}
			}
			finally
			{
				base.ResumeLayout(false);
				this.MergeItem.DropDown.ResumeLayout(false);
			}
		}

		// Token: 0x06002C34 RID: 11316 RVA: 0x000CE440 File Offset: 0x000CC640
		private void OnMoreWindowsMenuItemClick(object sender, EventArgs e)
		{
			Form[] mdiChildren = this.mdiParent.MdiChildren;
			if (mdiChildren != null)
			{
				IntSecurity.AllWindows.Assert();
				try
				{
					using (MdiWindowDialog mdiWindowDialog = new MdiWindowDialog())
					{
						mdiWindowDialog.SetItems(this.mdiParent.ActiveMdiChild, mdiChildren);
						DialogResult dialogResult = mdiWindowDialog.ShowDialog();
						if (dialogResult == DialogResult.OK)
						{
							mdiWindowDialog.ActiveChildForm.Activate();
							if (mdiWindowDialog.ActiveChildForm.ActiveControl != null && !mdiWindowDialog.ActiveChildForm.ActiveControl.Focused)
							{
								mdiWindowDialog.ActiveChildForm.ActiveControl.Focus();
							}
						}
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
		}

		// Token: 0x06002C35 RID: 11317 RVA: 0x000CE4F4 File Offset: 0x000CC6F4
		private void OnWindowListItemClick(object sender, EventArgs e)
		{
			ToolStripMenuItem toolStripMenuItem = sender as ToolStripMenuItem;
			if (toolStripMenuItem != null)
			{
				Form mdiForm = toolStripMenuItem.MdiForm;
				if (mdiForm != null)
				{
					IntSecurity.ModifyFocus.Assert();
					try
					{
						mdiForm.Activate();
						if (mdiForm.ActiveControl != null && !mdiForm.ActiveControl.Focused)
						{
							mdiForm.ActiveControl.Focus();
						}
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}
		}

		// Token: 0x040012DD RID: 4829
		private Form mdiParent;

		// Token: 0x040012DE RID: 4830
		private ToolStripMenuItem mergeItem;

		// Token: 0x040012DF RID: 4831
		private MenuStrip mergedMenu;
	}
}
