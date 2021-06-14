using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.BindingManagerBase.DataError" /> event. </summary>
	// Token: 0x02000126 RID: 294
	public class BindingManagerDataErrorEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.BindingManagerDataErrorEventArgs" /> class. </summary>
		/// <param name="exception">The <see cref="T:System.Exception" /> that occurred in the binding process that caused the <see cref="E:System.Windows.Forms.BindingManagerBase.DataError" /> event to be raised.</param>
		// Token: 0x06000801 RID: 2049 RVA: 0x0001815D File Offset: 0x0001635D
		public BindingManagerDataErrorEventArgs(Exception exception)
		{
			this.exception = exception;
		}

		/// <summary>Gets the <see cref="T:System.Exception" /> caught in the binding process that caused the <see cref="E:System.Windows.Forms.BindingManagerBase.DataError" /> event to be raised.</summary>
		/// <returns>The <see cref="T:System.Exception" /> that caused the <see cref="E:System.Windows.Forms.BindingManagerBase.DataError" /> event to be raised. </returns>
		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000802 RID: 2050 RVA: 0x0001816C File Offset: 0x0001636C
		public Exception Exception
		{
			get
			{
				return this.exception;
			}
		}

		// Token: 0x0400060F RID: 1551
		private Exception exception;
	}
}
