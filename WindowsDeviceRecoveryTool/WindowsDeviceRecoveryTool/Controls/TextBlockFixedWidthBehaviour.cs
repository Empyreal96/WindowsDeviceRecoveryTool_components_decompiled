using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls
{
	// Token: 0x0200002E RID: 46
	public class TextBlockFixedWidthBehaviour : Behavior<TextBlock>
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000183 RID: 387 RVA: 0x0000AB20 File Offset: 0x00008D20
		// (set) Token: 0x06000184 RID: 388 RVA: 0x0000AB42 File Offset: 0x00008D42
		public string Text
		{
			get
			{
				return (string)base.GetValue(TextBlockFixedWidthBehaviour.TextProperty);
			}
			set
			{
				base.SetValue(TextBlockFixedWidthBehaviour.TextProperty, value);
			}
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000AB54 File Offset: 0x00008D54
		private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBlockFixedWidthBehaviour textBlockFixedWidthBehaviour = d as TextBlockFixedWidthBehaviour;
			if (textBlockFixedWidthBehaviour != null)
			{
				textBlockFixedWidthBehaviour.ChangeTextBlockSize(textBlockFixedWidthBehaviour.parent.ActualWidth);
			}
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000AB88 File Offset: 0x00008D88
		protected override void OnAttached()
		{
			if (base.AssociatedObject.Parent is FrameworkElement)
			{
				this.parent = (base.AssociatedObject.Parent as FrameworkElement);
				this.parent.SizeChanged += this.OnParentSizeChanged;
				Binding binding = new Binding("Text");
				binding.Source = base.AssociatedObject;
				BindingOperations.SetBinding(this, TextBlockFixedWidthBehaviour.TextProperty, binding);
			}
			base.OnAttached();
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000AC0C File Offset: 0x00008E0C
		private void OnParentSizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.ChangeTextBlockSize(e.NewSize.Width);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000AC30 File Offset: 0x00008E30
		private void ChangeTextBlockSize(double parentWidth)
		{
			FormattedText formattedText = new FormattedText(base.AssociatedObject.Text, base.AssociatedObject.Language.GetEquivalentCulture(), base.AssociatedObject.FlowDirection, new Typeface(base.AssociatedObject.FontFamily, base.AssociatedObject.FontStyle, base.AssociatedObject.FontWeight, base.AssociatedObject.FontStretch), base.AssociatedObject.FontSize, base.AssociatedObject.Foreground);
			double val = parentWidth - 50.0;
			double num = Math.Min(val, formattedText.Width);
			base.AssociatedObject.Width = ((num > 0.0) ? num : 0.0);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000ACEF File Offset: 0x00008EEF
		protected override void OnDetaching()
		{
			this.parent.SizeChanged -= this.OnParentSizeChanged;
			this.parent = null;
			base.OnDetaching();
		}

		// Token: 0x040000BB RID: 187
		private FrameworkElement parent = null;

		// Token: 0x040000BC RID: 188
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TextBlockFixedWidthBehaviour), new PropertyMetadata(null, new PropertyChangedCallback(TextBlockFixedWidthBehaviour.OnTextChanged)));
	}
}
