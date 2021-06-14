using System;

namespace System.Windows.Forms
{
	// Token: 0x020003D7 RID: 983
	internal class MenuTimer
	{
		// Token: 0x06004147 RID: 16711 RVA: 0x00119C3C File Offset: 0x00117E3C
		public MenuTimer()
		{
			this.autoMenuExpandTimer.Tick += this.OnTick;
			this.slowShow = Math.Max(this.quickShow, SystemInformation.MenuShowDelay);
		}

		// Token: 0x17001049 RID: 4169
		// (get) Token: 0x06004148 RID: 16712 RVA: 0x00119C8E File Offset: 0x00117E8E
		// (set) Token: 0x06004149 RID: 16713 RVA: 0x00119C96 File Offset: 0x00117E96
		private ToolStripMenuItem CurrentItem
		{
			get
			{
				return this.currentItem;
			}
			set
			{
				this.currentItem = value;
			}
		}

		// Token: 0x1700104A RID: 4170
		// (get) Token: 0x0600414A RID: 16714 RVA: 0x00119C9F File Offset: 0x00117E9F
		// (set) Token: 0x0600414B RID: 16715 RVA: 0x00119CA7 File Offset: 0x00117EA7
		public bool InTransition
		{
			get
			{
				return this.inTransition;
			}
			set
			{
				this.inTransition = value;
			}
		}

		// Token: 0x0600414C RID: 16716 RVA: 0x00119CB0 File Offset: 0x00117EB0
		public void Start(ToolStripMenuItem item)
		{
			if (this.InTransition)
			{
				return;
			}
			this.StartCore(item);
		}

		// Token: 0x0600414D RID: 16717 RVA: 0x00119CC4 File Offset: 0x00117EC4
		private void StartCore(ToolStripMenuItem item)
		{
			if (item != this.CurrentItem)
			{
				this.Cancel(this.CurrentItem);
			}
			this.CurrentItem = item;
			if (item != null)
			{
				this.CurrentItem = item;
				this.autoMenuExpandTimer.Interval = (item.IsOnDropDown ? this.slowShow : this.quickShow);
				this.autoMenuExpandTimer.Enabled = true;
			}
		}

		// Token: 0x0600414E RID: 16718 RVA: 0x00119D24 File Offset: 0x00117F24
		public void Transition(ToolStripMenuItem fromItem, ToolStripMenuItem toItem)
		{
			if (toItem == null && this.InTransition)
			{
				this.Cancel();
				this.EndTransition(true);
				return;
			}
			if (this.fromItem != fromItem)
			{
				this.fromItem = fromItem;
				this.CancelCore();
				this.StartCore(toItem);
			}
			this.CurrentItem = toItem;
			this.InTransition = true;
		}

		// Token: 0x0600414F RID: 16719 RVA: 0x00119D75 File Offset: 0x00117F75
		public void Cancel()
		{
			if (this.InTransition)
			{
				return;
			}
			this.CancelCore();
		}

		// Token: 0x06004150 RID: 16720 RVA: 0x00119D86 File Offset: 0x00117F86
		public void Cancel(ToolStripMenuItem item)
		{
			if (this.InTransition)
			{
				return;
			}
			if (item == this.CurrentItem)
			{
				this.CancelCore();
			}
		}

		// Token: 0x06004151 RID: 16721 RVA: 0x00119DA0 File Offset: 0x00117FA0
		private void CancelCore()
		{
			this.autoMenuExpandTimer.Enabled = false;
			this.CurrentItem = null;
		}

		// Token: 0x06004152 RID: 16722 RVA: 0x00119DB8 File Offset: 0x00117FB8
		private void EndTransition(bool forceClose)
		{
			ToolStripMenuItem toolStripMenuItem = this.fromItem;
			this.fromItem = null;
			if (this.InTransition)
			{
				this.InTransition = false;
				bool flag = forceClose || (this.CurrentItem != null && this.CurrentItem != toolStripMenuItem && this.CurrentItem.Selected);
				if (flag && toolStripMenuItem != null && toolStripMenuItem.HasDropDownItems)
				{
					toolStripMenuItem.HideDropDown();
				}
			}
		}

		// Token: 0x06004153 RID: 16723 RVA: 0x00119E1C File Offset: 0x0011801C
		internal void HandleToolStripMouseLeave(ToolStrip toolStrip)
		{
			if (this.InTransition && toolStrip == this.fromItem.ParentInternal)
			{
				if (this.CurrentItem != null)
				{
					this.CurrentItem.Select();
					return;
				}
			}
			else if (toolStrip.IsDropDown && toolStrip.ActiveDropDowns.Count > 0)
			{
				ToolStripDropDown toolStripDropDown = toolStrip.ActiveDropDowns[0] as ToolStripDropDown;
				ToolStripMenuItem toolStripMenuItem = (toolStripDropDown == null) ? null : (toolStripDropDown.OwnerItem as ToolStripMenuItem);
				if (toolStripMenuItem != null && toolStripMenuItem.Pressed)
				{
					toolStripMenuItem.Select();
				}
			}
		}

		// Token: 0x06004154 RID: 16724 RVA: 0x00119EA0 File Offset: 0x001180A0
		private void OnTick(object sender, EventArgs e)
		{
			this.autoMenuExpandTimer.Enabled = false;
			if (this.CurrentItem == null)
			{
				return;
			}
			this.EndTransition(false);
			if (this.CurrentItem != null && !this.CurrentItem.IsDisposed && this.CurrentItem.Selected && this.CurrentItem.Enabled && ToolStripManager.ModalMenuFilter.InMenuMode)
			{
				this.CurrentItem.OnMenuAutoExpand();
			}
		}

		// Token: 0x0400250D RID: 9485
		private Timer autoMenuExpandTimer = new Timer();

		// Token: 0x0400250E RID: 9486
		private ToolStripMenuItem currentItem;

		// Token: 0x0400250F RID: 9487
		private ToolStripMenuItem fromItem;

		// Token: 0x04002510 RID: 9488
		private bool inTransition;

		// Token: 0x04002511 RID: 9489
		private int quickShow = 1;

		// Token: 0x04002512 RID: 9490
		private int slowShow;
	}
}
