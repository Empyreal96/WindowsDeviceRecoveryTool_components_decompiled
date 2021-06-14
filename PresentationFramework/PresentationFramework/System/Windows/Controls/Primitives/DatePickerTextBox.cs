using System;
using System.Windows.Data;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents the text input of a <see cref="T:System.Windows.Controls.DatePicker" />.</summary>
	// Token: 0x02000582 RID: 1410
	[TemplatePart(Name = "PART_Watermark", Type = typeof(ContentControl))]
	public sealed class DatePickerTextBox : TextBox
	{
		// Token: 0x06005D5F RID: 23903 RVA: 0x001A4898 File Offset: 0x001A2A98
		static DatePickerTextBox()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DatePickerTextBox), new FrameworkPropertyMetadata(typeof(DatePickerTextBox)));
			TextBox.TextProperty.OverrideMetadata(typeof(DatePickerTextBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.Primitives.DatePickerTextBox" /> class. </summary>
		// Token: 0x06005D60 RID: 23904 RVA: 0x001A4924 File Offset: 0x001A2B24
		public DatePickerTextBox()
		{
			base.SetCurrentValue(DatePickerTextBox.WatermarkProperty, SR.Get("DatePickerTextBox_DefaultWatermarkText"));
			base.Loaded += this.OnLoaded;
			base.IsEnabledChanged += this.OnDatePickerTextBoxIsEnabledChanged;
		}

		// Token: 0x17001695 RID: 5781
		// (get) Token: 0x06005D61 RID: 23905 RVA: 0x001A4970 File Offset: 0x001A2B70
		// (set) Token: 0x06005D62 RID: 23906 RVA: 0x001A497D File Offset: 0x001A2B7D
		internal object Watermark
		{
			get
			{
				return base.GetValue(DatePickerTextBox.WatermarkProperty);
			}
			set
			{
				base.SetValue(DatePickerTextBox.WatermarkProperty, value);
			}
		}

		/// <summary>Builds the visual tree for the <see cref="T:System.Windows.Controls.Primitives.DatePickerTextBox" /> when a new template is applied.</summary>
		// Token: 0x06005D63 RID: 23907 RVA: 0x001A498C File Offset: 0x001A2B8C
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.elementContent = this.ExtractTemplatePart<ContentControl>("PART_Watermark");
			if (this.elementContent != null)
			{
				Binding binding = new Binding("Watermark");
				binding.Source = this;
				this.elementContent.SetBinding(ContentControl.ContentProperty, binding);
			}
			this.OnWatermarkChanged();
		}

		// Token: 0x06005D64 RID: 23908 RVA: 0x001A49E2 File Offset: 0x001A2BE2
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			base.OnGotFocus(e);
			if (base.IsEnabled && !string.IsNullOrEmpty(base.Text))
			{
				base.Select(0, base.Text.Length);
			}
		}

		// Token: 0x06005D65 RID: 23909 RVA: 0x001A4A12 File Offset: 0x001A2C12
		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			base.ApplyTemplate();
		}

		// Token: 0x06005D66 RID: 23910 RVA: 0x001A4A1C File Offset: 0x001A2C1C
		internal override void ChangeVisualState(bool useTransitions)
		{
			base.ChangeVisualState(useTransitions);
			if (this.Watermark != null && string.IsNullOrEmpty(base.Text))
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Watermarked",
					"Unwatermarked"
				});
				return;
			}
			VisualStates.GoToState(this, useTransitions, new string[]
			{
				"Unwatermarked"
			});
		}

		// Token: 0x06005D67 RID: 23911 RVA: 0x001A4A78 File Offset: 0x001A2C78
		private T ExtractTemplatePart<T>(string partName) where T : DependencyObject
		{
			DependencyObject templateChild = base.GetTemplateChild(partName);
			return DatePickerTextBox.ExtractTemplatePart<T>(partName, templateChild);
		}

		// Token: 0x06005D68 RID: 23912 RVA: 0x001A4A94 File Offset: 0x001A2C94
		private static T ExtractTemplatePart<T>(string partName, DependencyObject obj) where T : DependencyObject
		{
			return obj as T;
		}

		// Token: 0x06005D69 RID: 23913 RVA: 0x001A4AA4 File Offset: 0x001A2CA4
		private void OnDatePickerTextBoxIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			bool flag = (bool)e.NewValue;
			base.SetCurrentValueInternal(TextBoxBase.IsReadOnlyProperty, BooleanBoxes.Box(!flag));
		}

		// Token: 0x06005D6A RID: 23914 RVA: 0x001A4AD4 File Offset: 0x001A2CD4
		private void OnWatermarkChanged()
		{
			if (this.elementContent != null)
			{
				Control control = this.Watermark as Control;
				if (control != null)
				{
					control.IsTabStop = false;
					control.IsHitTestVisible = false;
				}
			}
		}

		// Token: 0x06005D6B RID: 23915 RVA: 0x001A4B08 File Offset: 0x001A2D08
		private static void OnWatermarkPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			DatePickerTextBox datePickerTextBox = sender as DatePickerTextBox;
			datePickerTextBox.OnWatermarkChanged();
			datePickerTextBox.UpdateVisualState();
		}

		// Token: 0x04003015 RID: 12309
		private const string ElementContentName = "PART_Watermark";

		// Token: 0x04003016 RID: 12310
		private ContentControl elementContent;

		// Token: 0x04003017 RID: 12311
		internal static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(object), typeof(DatePickerTextBox), new PropertyMetadata(new PropertyChangedCallback(DatePickerTextBox.OnWatermarkPropertyChanged)));
	}
}
