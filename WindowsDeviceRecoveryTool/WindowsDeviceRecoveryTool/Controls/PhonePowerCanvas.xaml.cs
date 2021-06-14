using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls
{
	// Token: 0x0200002D RID: 45
	public partial class PhonePowerCanvas : Grid, INotifyPropertyChanged
	{
		// Token: 0x06000178 RID: 376 RVA: 0x0000A8E7 File Offset: 0x00008AE7
		public PhonePowerCanvas()
		{
			this.InitializeComponent();
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000179 RID: 377 RVA: 0x0000A8FC File Offset: 0x00008AFC
		// (remove) Token: 0x0600017A RID: 378 RVA: 0x0000A938 File Offset: 0x00008B38
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600017B RID: 379 RVA: 0x0000A974 File Offset: 0x00008B74
		// (set) Token: 0x0600017C RID: 380 RVA: 0x0000A996 File Offset: 0x00008B96
		public Brush PhoneColor
		{
			get
			{
				return (Brush)base.GetValue(PhonePowerCanvas.PhoneColorProperty);
			}
			set
			{
				base.SetValue(PhonePowerCanvas.PhoneColorProperty, value);
				this.OnPropertyChanged("PhoneColor");
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600017D RID: 381 RVA: 0x0000A9B4 File Offset: 0x00008BB4
		// (set) Token: 0x0600017E RID: 382 RVA: 0x0000A9D6 File Offset: 0x00008BD6
		public Brush ButtonColor
		{
			get
			{
				return (Brush)base.GetValue(PhonePowerCanvas.ButtonColorProperty);
			}
			set
			{
				base.SetValue(PhonePowerCanvas.ButtonColorProperty, value);
				this.OnPropertyChanged("ButtonColor");
			}
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000A9F4 File Offset: 0x00008BF4
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x040000B5 RID: 181
		public static readonly DependencyProperty PhoneColorProperty = DependencyProperty.Register("PhoneColor", typeof(Brush), typeof(PhonePowerCanvas), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 0, 0))));

		// Token: 0x040000B6 RID: 182
		public static readonly DependencyProperty ButtonColorProperty = DependencyProperty.Register("ButtonColor", typeof(Brush), typeof(PhonePowerCanvas), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 0, 0))));
	}
}
