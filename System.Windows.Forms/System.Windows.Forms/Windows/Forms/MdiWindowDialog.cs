using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x020002DE RID: 734
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed partial class MdiWindowDialog : Form
	{
		// Token: 0x06002C36 RID: 11318 RVA: 0x000CE560 File Offset: 0x000CC760
		public MdiWindowDialog()
		{
			this.InitializeComponent();
		}

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x06002C37 RID: 11319 RVA: 0x000CE56E File Offset: 0x000CC76E
		public Form ActiveChildForm
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x06002C38 RID: 11320 RVA: 0x000CE578 File Offset: 0x000CC778
		public void SetItems(Form active, Form[] all)
		{
			int selectedIndex = 0;
			for (int i = 0; i < all.Length; i++)
			{
				if (all[i].Visible)
				{
					int num = this.itemList.Items.Add(new MdiWindowDialog.ListItem(all[i]));
					if (all[i].Equals(active))
					{
						selectedIndex = num;
					}
				}
			}
			this.active = active;
			this.itemList.SelectedIndex = selectedIndex;
		}

		// Token: 0x06002C39 RID: 11321 RVA: 0x000CE5D8 File Offset: 0x000CC7D8
		private void ItemList_doubleClick(object source, EventArgs e)
		{
			this.okButton.PerformClick();
		}

		// Token: 0x06002C3A RID: 11322 RVA: 0x000CE5E8 File Offset: 0x000CC7E8
		private void ItemList_selectedIndexChanged(object source, EventArgs e)
		{
			MdiWindowDialog.ListItem listItem = (MdiWindowDialog.ListItem)this.itemList.SelectedItem;
			if (listItem != null)
			{
				this.active = listItem.form;
			}
		}

		// Token: 0x040012E9 RID: 4841
		private Form active;

		// Token: 0x02000619 RID: 1561
		private class ListItem
		{
			// Token: 0x06005DDF RID: 24031 RVA: 0x00185B8A File Offset: 0x00183D8A
			public ListItem(Form f)
			{
				this.form = f;
			}

			// Token: 0x06005DE0 RID: 24032 RVA: 0x00185B99 File Offset: 0x00183D99
			public override string ToString()
			{
				return this.form.Text;
			}

			// Token: 0x04003A16 RID: 14870
			public Form form;
		}
	}
}
