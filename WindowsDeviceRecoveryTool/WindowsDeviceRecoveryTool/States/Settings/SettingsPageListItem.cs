using System;
using System.Windows;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Settings
{
	// Token: 0x020000B1 RID: 177
	public sealed class SettingsPageListItem : DependencyObject
	{
		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x0001AD0C File Offset: 0x00018F0C
		// (set) Token: 0x06000533 RID: 1331 RVA: 0x0001AD2E File Offset: 0x00018F2E
		public string Title
		{
			get
			{
				return (string)base.GetValue(SettingsPageListItem.TitleProperty);
			}
			set
			{
				base.SetValue(SettingsPageListItem.TitleProperty, value);
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x0001AD40 File Offset: 0x00018F40
		// (set) Token: 0x06000535 RID: 1333 RVA: 0x0001AD62 File Offset: 0x00018F62
		public SettingsPage Page
		{
			get
			{
				return (SettingsPage)base.GetValue(SettingsPageListItem.PageProperty);
			}
			set
			{
				base.SetValue(SettingsPageListItem.PageProperty, value);
			}
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0001AD78 File Offset: 0x00018F78
		public override string ToString()
		{
			return this.Title;
		}

		// Token: 0x04000239 RID: 569
		public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(SettingsPageListItem));

		// Token: 0x0400023A RID: 570
		public static readonly DependencyProperty PageProperty = DependencyProperty.Register("Page", typeof(SettingsPage), typeof(SettingsPageListItem));
	}
}
