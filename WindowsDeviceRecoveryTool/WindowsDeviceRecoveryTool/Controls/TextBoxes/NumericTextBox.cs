using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.WindowsDeviceRecoveryTool.Controls.TextBoxes.Validation;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls.TextBoxes
{
	// Token: 0x0200003C RID: 60
	public class NumericTextBox : ValidatedTextBox
	{
		// Token: 0x0600021F RID: 543 RVA: 0x0000E18F File Offset: 0x0000C38F
		public NumericTextBox()
		{
			this.MinValue = int.MinValue;
			this.MaxValue = int.MaxValue;
			DataObject.AddPastingHandler(this, new DataObjectPastingEventHandler(this.OnPaste));
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0000E1C8 File Offset: 0x0000C3C8
		// (set) Token: 0x06000221 RID: 545 RVA: 0x0000E1DF File Offset: 0x0000C3DF
		public int MinValue { get; set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000222 RID: 546 RVA: 0x0000E1E8 File Offset: 0x0000C3E8
		// (set) Token: 0x06000223 RID: 547 RVA: 0x0000E1FF File Offset: 0x0000C3FF
		public int MaxValue { get; set; }

		// Token: 0x06000224 RID: 548 RVA: 0x0000E208 File Offset: 0x0000C408
		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			if (e.Key == Key.Space)
			{
				e.Handled = true;
			}
			else
			{
				base.OnPreviewKeyDown(e);
			}
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000E23C File Offset: 0x0000C43C
		protected override void OnTextChanged(TextChangedEventArgs e)
		{
			base.OnTextChanged(e);
			base.Text = NumericTextBox.RemoveTrailingZeros(base.Text);
			if (!this.IsTextAllowed(base.Text))
			{
				base.Text = string.Empty;
			}
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000E284 File Offset: 0x0000C484
		protected override void OnPreviewTextInput(TextCompositionEventArgs e)
		{
			if (e != null)
			{
				string text = new string(base.Text.ToCharArray());
				text = text.Remove(text.IndexOf(base.SelectedText, StringComparison.Ordinal), base.SelectedText.Length);
				e.Handled = !this.IsTextAllowed(text.Insert(base.CaretIndex, e.Text));
			}
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000E2F4 File Offset: 0x0000C4F4
		private static string RemoveTrailingZeros(string text)
		{
			string text2 = new string(text.ToCharArray());
			while (text2.StartsWith("0", StringComparison.Ordinal) && text2.Length > 1)
			{
				text2 = text2.Remove(0, 1);
			}
			return text2;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000E340 File Offset: 0x0000C540
		private void OnPaste(object sender, DataObjectPastingEventArgs e)
		{
			if (e != null)
			{
				if (e.DataObject.GetDataPresent(typeof(string)))
				{
					string text = new string(base.Text.ToCharArray());
					text = new string(text.ToCharArray());
					text = text.Remove(text.IndexOf(base.SelectedText, StringComparison.Ordinal), base.SelectedText.Length);
					text = text.Insert(base.CaretIndex, (string)e.DataObject.GetData(typeof(string)));
					text = NumericTextBox.RemoveTrailingZeros(text);
					if (!this.IsTextAllowed(text))
					{
						e.CancelCommand();
					}
				}
				else
				{
					e.CancelCommand();
				}
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000E40C File Offset: 0x0000C60C
		private bool IsTextAllowed(string text)
		{
			int num;
			return (!text.StartsWith("0", StringComparison.Ordinal) || text.Length <= 1) && int.TryParse(text, out num) && this.MinValue <= num && num <= this.MaxValue;
		}
	}
}
