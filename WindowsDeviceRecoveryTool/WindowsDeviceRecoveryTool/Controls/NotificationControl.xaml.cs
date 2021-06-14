using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls
{
	// Token: 0x0200002B RID: 43
	public sealed partial class NotificationControl : Border
	{
		// Token: 0x0600015D RID: 349 RVA: 0x0000A3F3 File Offset: 0x000085F3
		public NotificationControl()
		{
			this.InitializeComponent();
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600015E RID: 350 RVA: 0x0000A408 File Offset: 0x00008608
		// (set) Token: 0x0600015F RID: 351 RVA: 0x0000A42A File Offset: 0x0000862A
		public Brush Foreground
		{
			get
			{
				return (Brush)base.GetValue(NotificationControl.ForegroundProperty);
			}
			set
			{
				base.SetValue(NotificationControl.ForegroundProperty, value);
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000160 RID: 352 RVA: 0x0000A43C File Offset: 0x0000863C
		// (set) Token: 0x06000161 RID: 353 RVA: 0x0000A45E File Offset: 0x0000865E
		public bool? ShowNotification
		{
			get
			{
				return (bool?)base.GetValue(NotificationControl.ShowNotificationProperty);
			}
			set
			{
				base.SetValue(NotificationControl.ShowNotificationProperty, value);
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000162 RID: 354 RVA: 0x0000A474 File Offset: 0x00008674
		// (set) Token: 0x06000163 RID: 355 RVA: 0x0000A496 File Offset: 0x00008696
		public string NotificationHeader
		{
			get
			{
				return (string)base.GetValue(NotificationControl.NotificationHeaderProperty);
			}
			set
			{
				base.SetValue(NotificationControl.NotificationHeaderProperty, value);
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000164 RID: 356 RVA: 0x0000A4A8 File Offset: 0x000086A8
		// (set) Token: 0x06000165 RID: 357 RVA: 0x0000A4CA File Offset: 0x000086CA
		public string NotificationText
		{
			get
			{
				return (string)base.GetValue(NotificationControl.NotificationTextProperty);
			}
			set
			{
				base.SetValue(NotificationControl.NotificationTextProperty, value);
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000166 RID: 358 RVA: 0x0000A4DC File Offset: 0x000086DC
		// (set) Token: 0x06000167 RID: 359 RVA: 0x0000A4FE File Offset: 0x000086FE
		public Style HeaderStyle
		{
			get
			{
				return (Style)base.GetValue(NotificationControl.HeaderStyleProperty);
			}
			set
			{
				base.SetValue(NotificationControl.HeaderStyleProperty, value);
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000168 RID: 360 RVA: 0x0000A510 File Offset: 0x00008710
		// (set) Token: 0x06000169 RID: 361 RVA: 0x0000A532 File Offset: 0x00008732
		public Style MessageStyle
		{
			get
			{
				return (Style)base.GetValue(NotificationControl.MessageStyleProperty);
			}
			set
			{
				base.SetValue(NotificationControl.MessageStyleProperty, value);
			}
		}

		// Token: 0x040000A7 RID: 167
		public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register("Foreground", typeof(Brush), typeof(NotificationControl), new PropertyMetadata(null));

		// Token: 0x040000A8 RID: 168
		public static readonly DependencyProperty NotificationHeaderProperty = DependencyProperty.Register("NotificationHeader", typeof(string), typeof(NotificationControl), new PropertyMetadata(null));

		// Token: 0x040000A9 RID: 169
		public static readonly DependencyProperty NotificationTextProperty = DependencyProperty.Register("NotificationText", typeof(string), typeof(NotificationControl), new PropertyMetadata(null));

		// Token: 0x040000AA RID: 170
		public static readonly DependencyProperty HeaderStyleProperty = DependencyProperty.Register("HeaderStyle", typeof(Style), typeof(NotificationControl), new PropertyMetadata(null));

		// Token: 0x040000AB RID: 171
		public static readonly DependencyProperty MessageStyleProperty = DependencyProperty.Register("MessageStyle", typeof(Style), typeof(NotificationControl), new PropertyMetadata(null));

		// Token: 0x040000AC RID: 172
		public static readonly DependencyProperty ShowNotificationProperty = DependencyProperty.Register("ShowNotification", typeof(bool?), typeof(NotificationControl), new PropertyMetadata(null));
	}
}
