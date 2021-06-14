using System;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls
{
	// Token: 0x02000024 RID: 36
	public class LocalizationTextBlock : TextBlock
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000106 RID: 262 RVA: 0x000084C4 File Offset: 0x000066C4
		// (set) Token: 0x06000107 RID: 263 RVA: 0x000084E6 File Offset: 0x000066E6
		public string LocalizationText
		{
			get
			{
				return (string)base.GetValue(LocalizationTextBlock.LocalizationTextProperty);
			}
			set
			{
				base.SetValue(LocalizationTextBlock.LocalizationTextProperty, value);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000108 RID: 264 RVA: 0x000084F8 File Offset: 0x000066F8
		// (set) Token: 0x06000109 RID: 265 RVA: 0x0000851A File Offset: 0x0000671A
		public string NullValue
		{
			get
			{
				return (string)base.GetValue(LocalizationTextBlock.NullValueProperty);
			}
			set
			{
				base.SetValue(LocalizationTextBlock.NullValueProperty, value);
			}
		}

		// Token: 0x04000088 RID: 136
		public static readonly DependencyProperty LocalizationTextProperty = DependencyProperty.Register("LocalizationText", typeof(string), typeof(LocalizationTextBlock), new PropertyMetadata(null));

		// Token: 0x04000089 RID: 137
		public static readonly DependencyProperty NullValueProperty = DependencyProperty.Register("NullValue", typeof(string), typeof(LocalizationTextBlock), new PropertyMetadata(null));
	}
}
