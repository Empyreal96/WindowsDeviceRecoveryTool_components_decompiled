using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls
{
	// Token: 0x0200002F RID: 47
	public partial class VariantPackageControl : UserControl
	{
		// Token: 0x0600018C RID: 396 RVA: 0x0000AD5F File Offset: 0x00008F5F
		public VariantPackageControl()
		{
			this.InitializeComponent();
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600018D RID: 397 RVA: 0x0000AD74 File Offset: 0x00008F74
		// (set) Token: 0x0600018E RID: 398 RVA: 0x0000AD96 File Offset: 0x00008F96
		public bool IsControlVisible
		{
			get
			{
				return (bool)base.GetValue(VariantPackageControl.IsControlVisibleProperty);
			}
			set
			{
				base.SetValue(VariantPackageControl.IsControlVisibleProperty, value);
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600018F RID: 399 RVA: 0x0000ADAC File Offset: 0x00008FAC
		// (set) Token: 0x06000190 RID: 400 RVA: 0x0000ADCE File Offset: 0x00008FCE
		public Visibility AkVersionVisibility
		{
			get
			{
				return (Visibility)base.GetValue(VariantPackageControl.AkVersionVisibilityProperty);
			}
			set
			{
				base.SetValue(VariantPackageControl.AkVersionVisibilityProperty, value);
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000191 RID: 401 RVA: 0x0000ADE4 File Offset: 0x00008FE4
		// (set) Token: 0x06000192 RID: 402 RVA: 0x0000AE06 File Offset: 0x00009006
		public Visibility PlatformIdVisibility
		{
			get
			{
				return (Visibility)base.GetValue(VariantPackageControl.PlatformIdVisibilityProperty);
			}
			set
			{
				base.SetValue(VariantPackageControl.PlatformIdVisibilityProperty, value);
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000193 RID: 403 RVA: 0x0000AE1C File Offset: 0x0000901C
		// (set) Token: 0x06000194 RID: 404 RVA: 0x0000AE3E File Offset: 0x0000903E
		public Visibility FirmwareVersionVisibility
		{
			get
			{
				return (Visibility)base.GetValue(VariantPackageControl.FirmwareVersionVisibilityProperty);
			}
			set
			{
				base.SetValue(VariantPackageControl.FirmwareVersionVisibilityProperty, value);
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000195 RID: 405 RVA: 0x0000AE54 File Offset: 0x00009054
		// (set) Token: 0x06000196 RID: 406 RVA: 0x0000AE76 File Offset: 0x00009076
		public Visibility BuildVersionVisibility
		{
			get
			{
				return (Visibility)base.GetValue(VariantPackageControl.BuildVersionVisibilityProperty);
			}
			set
			{
				base.SetValue(VariantPackageControl.BuildVersionVisibilityProperty, value);
			}
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000AE8C File Offset: 0x0000908C
		private static void OnIsControlVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			VariantPackageControl variantPackageControl = d as VariantPackageControl;
			if (variantPackageControl != null)
			{
				variantPackageControl.MainStackPanel.Visibility = (Visibility)new BooleanToVisibilityConverter().Convert(e.NewValue, null, null, null);
			}
		}

		// Token: 0x040000BD RID: 189
		public static readonly DependencyProperty AkVersionVisibilityProperty = DependencyProperty.Register("AkVersionVisibility", typeof(Visibility), typeof(VariantPackageControl), null);

		// Token: 0x040000BE RID: 190
		public static readonly DependencyProperty PlatformIdVisibilityProperty = DependencyProperty.Register("PlatformIdVisibility", typeof(Visibility), typeof(VariantPackageControl), null);

		// Token: 0x040000BF RID: 191
		public static readonly DependencyProperty IsControlVisibleProperty = DependencyProperty.Register("IsControlVisible", typeof(bool), typeof(VariantPackageControl), new PropertyMetadata(false, new PropertyChangedCallback(VariantPackageControl.OnIsControlVisibleChanged)));

		// Token: 0x040000C0 RID: 192
		public static readonly DependencyProperty FirmwareVersionVisibilityProperty = DependencyProperty.Register("FirmwareVersionVisibility", typeof(Visibility), typeof(VariantPackageControl), null);

		// Token: 0x040000C1 RID: 193
		public static readonly DependencyProperty BuildVersionVisibilityProperty = DependencyProperty.Register("BuildVersionVisibility", typeof(Visibility), typeof(VariantPackageControl), null);
	}
}
