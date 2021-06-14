using System;

namespace System.Windows.Controls
{
	/// <summary>Represents a validation error that is created either by the binding engine when a <see cref="T:System.Windows.Controls.ValidationRule" /> reports a validation error, or through the <see cref="M:System.Windows.Controls.Validation.MarkInvalid(System.Windows.Data.BindingExpressionBase,System.Windows.Controls.ValidationError)" /> method explicitly.</summary>
	// Token: 0x02000551 RID: 1361
	public class ValidationError
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.ValidationError" /> class with the specified parameters.</summary>
		/// <param name="ruleInError">The rule that detected validation error.</param>
		/// <param name="bindingInError">The <see cref="T:System.Windows.Data.BindingExpression" /> or <see cref="T:System.Windows.Data.MultiBindingExpression" /> object with the validation error.</param>
		/// <param name="errorContent">Information about the error.</param>
		/// <param name="exception">The exception that caused the validation failure. This parameter is optional and can be set to <see langword="null" />.</param>
		// Token: 0x0600598B RID: 22923 RVA: 0x0018B90C File Offset: 0x00189B0C
		public ValidationError(ValidationRule ruleInError, object bindingInError, object errorContent, Exception exception)
		{
			if (ruleInError == null)
			{
				throw new ArgumentNullException("ruleInError");
			}
			if (bindingInError == null)
			{
				throw new ArgumentNullException("bindingInError");
			}
			this._ruleInError = ruleInError;
			this._bindingInError = bindingInError;
			this._errorContent = errorContent;
			this._exception = exception;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.ValidationError" /> class with the specified parameters.</summary>
		/// <param name="ruleInError">The rule that detected validation error.</param>
		/// <param name="bindingInError">The <see cref="T:System.Windows.Data.BindingExpression" /> or <see cref="T:System.Windows.Data.MultiBindingExpression" /> object with the validation error.</param>
		// Token: 0x0600598C RID: 22924 RVA: 0x0018B958 File Offset: 0x00189B58
		public ValidationError(ValidationRule ruleInError, object bindingInError) : this(ruleInError, bindingInError, null, null)
		{
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Controls.ValidationRule" /> object that was the cause of this <see cref="T:System.Windows.Controls.ValidationError" />, if the error is the result of a validation rule.</summary>
		/// <returns>The <see cref="T:System.Windows.Controls.ValidationRule" /> object, if the error is the result of a validation rule.</returns>
		// Token: 0x170015C3 RID: 5571
		// (get) Token: 0x0600598D RID: 22925 RVA: 0x0018B964 File Offset: 0x00189B64
		// (set) Token: 0x0600598E RID: 22926 RVA: 0x0018B96C File Offset: 0x00189B6C
		public ValidationRule RuleInError
		{
			get
			{
				return this._ruleInError;
			}
			set
			{
				this._ruleInError = value;
			}
		}

		/// <summary>Gets or sets an object that provides additional context for this <see cref="T:System.Windows.Controls.ValidationError" />, such as a string describing the error.</summary>
		/// <returns>An object that provides additional context for this <see cref="T:System.Windows.Controls.ValidationError" />.</returns>
		// Token: 0x170015C4 RID: 5572
		// (get) Token: 0x0600598F RID: 22927 RVA: 0x0018B975 File Offset: 0x00189B75
		// (set) Token: 0x06005990 RID: 22928 RVA: 0x0018B97D File Offset: 0x00189B7D
		public object ErrorContent
		{
			get
			{
				return this._errorContent;
			}
			set
			{
				this._errorContent = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Exception" /> object that was the cause of this <see cref="T:System.Windows.Controls.ValidationError" />, if the error is the result of an exception.</summary>
		/// <returns>The <see cref="T:System.Exception" /> object, if the error is the result of an exception.</returns>
		// Token: 0x170015C5 RID: 5573
		// (get) Token: 0x06005991 RID: 22929 RVA: 0x0018B986 File Offset: 0x00189B86
		// (set) Token: 0x06005992 RID: 22930 RVA: 0x0018B98E File Offset: 0x00189B8E
		public Exception Exception
		{
			get
			{
				return this._exception;
			}
			set
			{
				this._exception = value;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Data.BindingExpression" /> or <see cref="T:System.Windows.Data.MultiBindingExpression" /> object of this <see cref="T:System.Windows.Controls.ValidationError" />. The object is either marked invalid explicitly or has a failed validation rule.</summary>
		/// <returns>The <see cref="T:System.Windows.Data.BindingExpression" /> or <see cref="T:System.Windows.Data.MultiBindingExpression" /> object of this <see cref="T:System.Windows.Controls.ValidationError" />.</returns>
		// Token: 0x170015C6 RID: 5574
		// (get) Token: 0x06005993 RID: 22931 RVA: 0x0018B997 File Offset: 0x00189B97
		public object BindingInError
		{
			get
			{
				return this._bindingInError;
			}
		}

		// Token: 0x04002F0A RID: 12042
		private ValidationRule _ruleInError;

		// Token: 0x04002F0B RID: 12043
		private object _errorContent;

		// Token: 0x04002F0C RID: 12044
		private Exception _exception;

		// Token: 0x04002F0D RID: 12045
		private object _bindingInError;
	}
}
