using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls.TextBoxes.Validation
{
	// Token: 0x0200003B RID: 59
	public class ValidatedTextBox : TextBox, IDisposable
	{
		// Token: 0x06000218 RID: 536 RVA: 0x0000E085 File Offset: 0x0000C285
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000E098 File Offset: 0x0000C298
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
				}
				this.disposed = true;
			}
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000E0C8 File Offset: 0x0000C2C8
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			base.OnGotFocus(e);
			if (this.ValidationSucceeded())
			{
				this.validatedText = base.Text;
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000E0FC File Offset: 0x0000C2FC
		protected override void OnLostFocus(RoutedEventArgs e)
		{
			base.OnLostFocus(e);
			if (!this.ValidationSucceeded())
			{
				base.Text = this.validatedText;
			}
			this.ClearValidationErrors();
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000E134 File Offset: 0x0000C334
		private bool ValidationSucceeded()
		{
			return !Validation.GetHasError(this);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000E150 File Offset: 0x0000C350
		private void ClearValidationErrors()
		{
			BindingExpression bindingExpression = base.GetBindingExpression(TextBox.TextProperty);
			if (bindingExpression != null)
			{
				Validation.ClearInvalid(bindingExpression);
			}
		}

		// Token: 0x040000E3 RID: 227
		private bool disposed;

		// Token: 0x040000E4 RID: 228
		private string validatedText = string.Empty;
	}
}
