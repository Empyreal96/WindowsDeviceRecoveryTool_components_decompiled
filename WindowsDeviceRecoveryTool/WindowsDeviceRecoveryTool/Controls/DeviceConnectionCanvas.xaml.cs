using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls
{
	// Token: 0x0200003A RID: 58
	public partial class DeviceConnectionCanvas : Grid
	{
		// Token: 0x06000210 RID: 528 RVA: 0x0000DEBF File Offset: 0x0000C0BF
		public DeviceConnectionCanvas()
		{
			this.InitializeComponent();
			base.Loaded += this.OnDeviceConnectionCanvasLoaded;
			base.Unloaded += this.OnDeviceConnectionCanvasUnLoaded;
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0000DEF8 File Offset: 0x0000C0F8
		// (set) Token: 0x06000212 RID: 530 RVA: 0x0000DF1A File Offset: 0x0000C11A
		public bool PlayAnimation
		{
			get
			{
				return (bool)base.GetValue(DeviceConnectionCanvas.PlayAnimationProperty);
			}
			set
			{
				base.SetValue(DeviceConnectionCanvas.PlayAnimationProperty, value);
			}
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000DF30 File Offset: 0x0000C130
		private void OnDeviceConnectionCanvasLoaded(object sender, RoutedEventArgs e)
		{
			Storyboard storyboard = base.FindResource("FadeInOutAnimation") as Storyboard;
			if (storyboard != null)
			{
				if (this.PlayAnimation)
				{
					storyboard.Begin();
				}
			}
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000DF70 File Offset: 0x0000C170
		private void OnDeviceConnectionCanvasUnLoaded(object sender, RoutedEventArgs e)
		{
			Storyboard storyboard = base.FindResource("FadeInOutAnimation") as Storyboard;
			if (storyboard != null)
			{
				if (this.PlayAnimation)
				{
					storyboard.Stop();
				}
			}
		}

		// Token: 0x040000DD RID: 221
		public static readonly DependencyProperty PlayAnimationProperty = DependencyProperty.Register("PlayAnimation", typeof(bool), typeof(DeviceConnectionCanvas), new PropertyMetadata(false));
	}
}
