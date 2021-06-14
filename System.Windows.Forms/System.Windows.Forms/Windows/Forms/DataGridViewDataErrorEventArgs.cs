using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.DataError" /> event.</summary>
	// Token: 0x020001BE RID: 446
	public class DataGridViewDataErrorEventArgs : DataGridViewCellCancelEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewDataErrorEventArgs" /> class.</summary>
		/// <param name="exception">The exception that occurred.</param>
		/// <param name="columnIndex">The column index of the cell that raised the <see cref="E:System.Windows.Forms.DataGridView.DataError" />.</param>
		/// <param name="rowIndex">The row index of the cell that raised the <see cref="E:System.Windows.Forms.DataGridView.DataError" />.</param>
		/// <param name="context">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewDataErrorContexts" /> values indicating the context in which the error occurred. </param>
		// Token: 0x06001D05 RID: 7429 RVA: 0x0009351D File Offset: 0x0009171D
		public DataGridViewDataErrorEventArgs(Exception exception, int columnIndex, int rowIndex, DataGridViewDataErrorContexts context) : base(columnIndex, rowIndex)
		{
			this.exception = exception;
			this.context = context;
		}

		/// <summary>Gets details about the state of the <see cref="T:System.Windows.Forms.DataGridView" /> when the error occurred.</summary>
		/// <returns>A bitwise combination of the <see cref="T:System.Windows.Forms.DataGridViewDataErrorContexts" /> values that specifies the context in which the error occurred.</returns>
		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06001D06 RID: 7430 RVA: 0x00093536 File Offset: 0x00091736
		public DataGridViewDataErrorContexts Context
		{
			get
			{
				return this.context;
			}
		}

		/// <summary>Gets the exception that represents the error.</summary>
		/// <returns>An <see cref="T:System.Exception" /> that represents the error.</returns>
		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06001D07 RID: 7431 RVA: 0x0009353E File Offset: 0x0009173E
		public Exception Exception
		{
			get
			{
				return this.exception;
			}
		}

		/// <summary>Gets or sets a value indicating whether to throw the exception after the <see cref="T:System.Windows.Forms.DataGridViewDataErrorEventHandler" /> delegate is finished with it.</summary>
		/// <returns>
		///     <see langword="true" /> if the exception should be thrown; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentException">When setting this property to <see langword="true" />, the <see cref="P:System.Windows.Forms.DataGridViewDataErrorEventArgs.Exception" /> property value is <see langword="null" />.</exception>
		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06001D08 RID: 7432 RVA: 0x00093546 File Offset: 0x00091746
		// (set) Token: 0x06001D09 RID: 7433 RVA: 0x0009354E File Offset: 0x0009174E
		public bool ThrowException
		{
			get
			{
				return this.throwException;
			}
			set
			{
				if (value && this.exception == null)
				{
					throw new ArgumentException(SR.GetString("DataGridView_CannotThrowNullException"));
				}
				this.throwException = value;
			}
		}

		// Token: 0x04000CF6 RID: 3318
		private Exception exception;

		// Token: 0x04000CF7 RID: 3319
		private bool throwException;

		// Token: 0x04000CF8 RID: 3320
		private DataGridViewDataErrorContexts context;
	}
}
