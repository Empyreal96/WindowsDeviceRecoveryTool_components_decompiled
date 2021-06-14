using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.Binding.BindingComplete" /> event. </summary>
	// Token: 0x02000121 RID: 289
	public class BindingCompleteEventArgs : CancelEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.BindingCompleteEventArgs" /> class with the specified binding, error state and text, binding context, exception, and whether the binding should be cancelled.</summary>
		/// <param name="binding">The binding associated with this occurrence of a <see cref="E:System.Windows.Forms.Binding.BindingComplete" /> event.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.BindingCompleteState" /> values.</param>
		/// <param name="context">One of the <see cref="T:System.Windows.Forms.BindingCompleteContext" /> values. </param>
		/// <param name="errorText">The error text or exception message for errors that occurred during the binding.</param>
		/// <param name="exception">The <see cref="T:System.Exception" /> that occurred during the binding.</param>
		/// <param name="cancel">
		///       <see langword="true" /> to cancel the binding and keep focus on the current control; <see langword="false" /> to allow focus to shift to another control.</param>
		// Token: 0x060007AD RID: 1965 RVA: 0x000175C9 File Offset: 0x000157C9
		public BindingCompleteEventArgs(Binding binding, BindingCompleteState state, BindingCompleteContext context, string errorText, Exception exception, bool cancel) : base(cancel)
		{
			this.binding = binding;
			this.state = state;
			this.context = context;
			this.errorText = ((errorText == null) ? string.Empty : errorText);
			this.exception = exception;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.BindingCompleteEventArgs" /> class with the specified binding, error state and text, binding context, and exception.</summary>
		/// <param name="binding">The binding associated with this occurrence of a <see cref="E:System.Windows.Forms.Binding.BindingComplete" /> event.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.BindingCompleteState" /> values.</param>
		/// <param name="context">One of the <see cref="T:System.Windows.Forms.BindingCompleteContext" /> values. </param>
		/// <param name="errorText">The error text or exception message for errors that occurred during the binding.</param>
		/// <param name="exception">The <see cref="T:System.Exception" /> that occurred during the binding.</param>
		// Token: 0x060007AE RID: 1966 RVA: 0x00017603 File Offset: 0x00015803
		public BindingCompleteEventArgs(Binding binding, BindingCompleteState state, BindingCompleteContext context, string errorText, Exception exception) : this(binding, state, context, errorText, exception, true)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.BindingCompleteEventArgs" /> class with the specified binding, error state and text, and binding context.</summary>
		/// <param name="binding">The binding associated with this occurrence of a <see cref="E:System.Windows.Forms.Binding.BindingComplete" /> event.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.BindingCompleteState" /> values.</param>
		/// <param name="context">One of the <see cref="T:System.Windows.Forms.BindingCompleteContext" /> values. </param>
		/// <param name="errorText">The error text or exception message for errors that occurred during the binding.</param>
		// Token: 0x060007AF RID: 1967 RVA: 0x00017613 File Offset: 0x00015813
		public BindingCompleteEventArgs(Binding binding, BindingCompleteState state, BindingCompleteContext context, string errorText) : this(binding, state, context, errorText, null, true)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.BindingCompleteEventArgs" /> class with the specified binding, error state, and binding context.</summary>
		/// <param name="binding">The binding associated with this occurrence of a <see cref="E:System.Windows.Forms.Binding.BindingComplete" /> event.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.BindingCompleteState" /> values.</param>
		/// <param name="context">One of the <see cref="T:System.Windows.Forms.BindingCompleteContext" /> values. </param>
		// Token: 0x060007B0 RID: 1968 RVA: 0x00017622 File Offset: 0x00015822
		public BindingCompleteEventArgs(Binding binding, BindingCompleteState state, BindingCompleteContext context) : this(binding, state, context, string.Empty, null, false)
		{
		}

		/// <summary>Gets the binding associated with this occurrence of a <see cref="E:System.Windows.Forms.Binding.BindingComplete" /> event.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Binding" /> associated with this <see cref="T:System.Windows.Forms.BindingCompleteEventArgs" />.</returns>
		// Token: 0x17000249 RID: 585
		// (get) Token: 0x060007B1 RID: 1969 RVA: 0x00017634 File Offset: 0x00015834
		public Binding Binding
		{
			get
			{
				return this.binding;
			}
		}

		/// <summary>Gets the completion state of the binding operation.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.BindingCompleteState" /> values.</returns>
		// Token: 0x1700024A RID: 586
		// (get) Token: 0x060007B2 RID: 1970 RVA: 0x0001763C File Offset: 0x0001583C
		public BindingCompleteState BindingCompleteState
		{
			get
			{
				return this.state;
			}
		}

		/// <summary>Gets the direction of the binding operation.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.BindingCompleteContext" /> values. </returns>
		// Token: 0x1700024B RID: 587
		// (get) Token: 0x060007B3 RID: 1971 RVA: 0x00017644 File Offset: 0x00015844
		public BindingCompleteContext BindingCompleteContext
		{
			get
			{
				return this.context;
			}
		}

		/// <summary>Gets the text description of the error that occurred during the binding operation.</summary>
		/// <returns>The text description of the error that occurred during the binding operation.</returns>
		// Token: 0x1700024C RID: 588
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x0001764C File Offset: 0x0001584C
		public string ErrorText
		{
			get
			{
				return this.errorText;
			}
		}

		/// <summary>Gets the exception that occurred during the binding operation.</summary>
		/// <returns>The <see cref="T:System.Exception" /> that occurred during the binding operation.</returns>
		// Token: 0x1700024D RID: 589
		// (get) Token: 0x060007B5 RID: 1973 RVA: 0x00017654 File Offset: 0x00015854
		public Exception Exception
		{
			get
			{
				return this.exception;
			}
		}

		// Token: 0x040005FE RID: 1534
		private Binding binding;

		// Token: 0x040005FF RID: 1535
		private BindingCompleteState state;

		// Token: 0x04000600 RID: 1536
		private BindingCompleteContext context;

		// Token: 0x04000601 RID: 1537
		private string errorText;

		// Token: 0x04000602 RID: 1538
		private Exception exception;
	}
}
