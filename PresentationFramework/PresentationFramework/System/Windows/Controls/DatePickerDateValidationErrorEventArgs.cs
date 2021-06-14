using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DatePicker.DateValidationError" /> event.</summary>
	// Token: 0x020004C2 RID: 1218
	public class DatePickerDateValidationErrorEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DatePickerDateValidationErrorEventArgs" /> class. </summary>
		/// <param name="exception">The initial exception from the <see cref="E:System.Windows.Controls.DatePicker.DateValidationError" /> event.</param>
		/// <param name="text">The text that caused the <see cref="E:System.Windows.Controls.DatePicker.DateValidationError" /> event.</param>
		// Token: 0x06004A23 RID: 18979 RVA: 0x0014F4B6 File Offset: 0x0014D6B6
		public DatePickerDateValidationErrorEventArgs(Exception exception, string text)
		{
			this.Text = text;
			this.Exception = exception;
		}

		/// <summary>Gets the initial exception associated with the <see cref="E:System.Windows.Controls.DatePicker.DateValidationError" /> event.</summary>
		/// <returns>The exception associated with the validation failure.</returns>
		// Token: 0x17001215 RID: 4629
		// (get) Token: 0x06004A24 RID: 18980 RVA: 0x0014F4CC File Offset: 0x0014D6CC
		// (set) Token: 0x06004A25 RID: 18981 RVA: 0x0014F4D4 File Offset: 0x0014D6D4
		public Exception Exception { get; private set; }

		/// <summary>Gets or sets the text that caused the <see cref="E:System.Windows.Controls.DatePicker.DateValidationError" /> event.</summary>
		/// <returns>The text that caused the validation failure.</returns>
		// Token: 0x17001216 RID: 4630
		// (get) Token: 0x06004A26 RID: 18982 RVA: 0x0014F4DD File Offset: 0x0014D6DD
		// (set) Token: 0x06004A27 RID: 18983 RVA: 0x0014F4E5 File Offset: 0x0014D6E5
		public string Text { get; private set; }

		/// <summary>Gets or sets a value that indicates whether <see cref="P:System.Windows.Controls.DatePickerDateValidationErrorEventArgs.Exception" /> should be thrown.</summary>
		/// <returns>
		///     <see langword="true" /> if the exception should be thrown; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001217 RID: 4631
		// (get) Token: 0x06004A28 RID: 18984 RVA: 0x0014F4EE File Offset: 0x0014D6EE
		// (set) Token: 0x06004A29 RID: 18985 RVA: 0x0014F4F6 File Offset: 0x0014D6F6
		public bool ThrowException
		{
			get
			{
				return this._throwException;
			}
			set
			{
				this._throwException = value;
			}
		}

		// Token: 0x04002A4A RID: 10826
		private bool _throwException;
	}
}
