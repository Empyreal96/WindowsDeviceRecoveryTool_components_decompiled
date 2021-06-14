using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls
{
	// Token: 0x02000028 RID: 40
	public partial class DeviceSwInfoControl : ContentControl
	{
		// Token: 0x06000132 RID: 306 RVA: 0x00009CD4 File Offset: 0x00007ED4
		public DeviceSwInfoControl()
		{
			this.InitializeComponent();
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00009CE8 File Offset: 0x00007EE8
		// (set) Token: 0x06000134 RID: 308 RVA: 0x00009D0A File Offset: 0x00007F0A
		public Visibility AkVersionVisibility
		{
			get
			{
				return (Visibility)base.GetValue(DeviceSwInfoControl.AkVersionVisibilityProperty);
			}
			set
			{
				base.SetValue(DeviceSwInfoControl.AkVersionVisibilityProperty, value);
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00009D20 File Offset: 0x00007F20
		// (set) Token: 0x06000136 RID: 310 RVA: 0x00009D42 File Offset: 0x00007F42
		public Visibility FirmwareVersionVisibility
		{
			get
			{
				return (Visibility)base.GetValue(DeviceSwInfoControl.FirmwareVersionVisibilityProperty);
			}
			set
			{
				base.SetValue(DeviceSwInfoControl.FirmwareVersionVisibilityProperty, value);
			}
		}

		// Token: 0x04000092 RID: 146
		public static readonly DependencyProperty AkVersionVisibilityProperty = DependencyProperty.Register("AkVersionVisibility", typeof(Visibility), typeof(DeviceSwInfoControl), null);

		// Token: 0x04000093 RID: 147
		public static readonly DependencyProperty FirmwareVersionVisibilityProperty = DependencyProperty.Register("FirmwareVersionVisibility", typeof(Visibility), typeof(DeviceSwInfoControl), null);
	}
}
