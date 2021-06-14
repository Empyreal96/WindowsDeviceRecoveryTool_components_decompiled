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
	// Token: 0x0200002C RID: 44
	public partial class PhoneVolumesCanvas : Grid, INotifyPropertyChanged
	{
		// Token: 0x0600016D RID: 365 RVA: 0x0000A6AF File Offset: 0x000088AF
		public PhoneVolumesCanvas()
		{
			this.InitializeComponent();
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600016E RID: 366 RVA: 0x0000A6C4 File Offset: 0x000088C4
		// (remove) Token: 0x0600016F RID: 367 RVA: 0x0000A700 File Offset: 0x00008900
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000170 RID: 368 RVA: 0x0000A73C File Offset: 0x0000893C
		// (set) Token: 0x06000171 RID: 369 RVA: 0x0000A75E File Offset: 0x0000895E
		public Brush ButtonColor
		{
			get
			{
				return (Brush)base.GetValue(PhoneVolumesCanvas.ButtonColorProperty);
			}
			set
			{
				base.SetValue(PhoneVolumesCanvas.ButtonColorProperty, value);
				this.OnPropertyChanged("ButtonColor");
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000172 RID: 370 RVA: 0x0000A77C File Offset: 0x0000897C
		// (set) Token: 0x06000173 RID: 371 RVA: 0x0000A79E File Offset: 0x0000899E
		public Brush PhoneColor
		{
			get
			{
				return (Brush)base.GetValue(PhoneVolumesCanvas.PhoneColorProperty);
			}
			set
			{
				base.SetValue(PhoneVolumesCanvas.PhoneColorProperty, value);
				this.OnPropertyChanged("PhoneColor");
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000A7BC File Offset: 0x000089BC
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x040000AF RID: 175
		public static readonly DependencyProperty PhoneColorProperty = DependencyProperty.Register("PhoneColor", typeof(Brush), typeof(PhoneVolumesCanvas), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 0, 0))));

		// Token: 0x040000B0 RID: 176
		public static readonly DependencyProperty ButtonColorProperty = DependencyProperty.Register("ButtonColor", typeof(Brush), typeof(PhoneVolumesCanvas), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 0, 0))));
	}
}
