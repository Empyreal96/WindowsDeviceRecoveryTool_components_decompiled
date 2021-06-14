using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.MaskedTextBox.TypeValidationCompleted" /> event. </summary>
	// Token: 0x02000415 RID: 1045
	public class TypeValidationEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TypeValidationEventArgs" /> class.</summary>
		/// <param name="validatingType">The <see cref="T:System.Type" /> that the formatted input string was being validated against. </param>
		/// <param name="isValidInput">A <see cref="T:System.Boolean" /> value indicating whether the formatted string was successfully converted to the validating type. </param>
		/// <param name="returnValue">An <see cref="T:System.Object" /> that is the result of the formatted string being converted to the target type. </param>
		/// <param name="message">A <see cref="T:System.String" /> containing a description of the conversion process. </param>
		// Token: 0x06004760 RID: 18272 RVA: 0x00130E37 File Offset: 0x0012F037
		public TypeValidationEventArgs(Type validatingType, bool isValidInput, object returnValue, string message)
		{
			this.validatingType = validatingType;
			this.isValidInput = isValidInput;
			this.returnValue = returnValue;
			this.message = message;
		}

		/// <summary>Gets or sets a value indicating whether the event should be canceled.</summary>
		/// <returns>
		///     <see langword="true" /> if the event should be canceled and focus retained by the <see cref="T:System.Windows.Forms.MaskedTextBox" /> control; otherwise, <see langword="false" /> to continue validation processing.</returns>
		// Token: 0x170011D7 RID: 4567
		// (get) Token: 0x06004761 RID: 18273 RVA: 0x00130E5C File Offset: 0x0012F05C
		// (set) Token: 0x06004762 RID: 18274 RVA: 0x00130E64 File Offset: 0x0012F064
		public bool Cancel
		{
			get
			{
				return this.cancel;
			}
			set
			{
				this.cancel = value;
			}
		}

		/// <summary>Gets a value indicating whether the formatted input string was successfully converted to the validating type.</summary>
		/// <returns>
		///     <see langword="true" /> if the formatted input string can be converted into the type specified by the <see cref="P:System.Windows.Forms.TypeValidationEventArgs.ValidatingType" /> property; otherwise, <see langword="false" />. </returns>
		// Token: 0x170011D8 RID: 4568
		// (get) Token: 0x06004763 RID: 18275 RVA: 0x00130E6D File Offset: 0x0012F06D
		public bool IsValidInput
		{
			get
			{
				return this.isValidInput;
			}
		}

		/// <summary>Gets a text message describing the conversion process.</summary>
		/// <returns>A <see cref="T:System.String" /> containing a description of the conversion process.</returns>
		// Token: 0x170011D9 RID: 4569
		// (get) Token: 0x06004764 RID: 18276 RVA: 0x00130E75 File Offset: 0x0012F075
		public string Message
		{
			get
			{
				return this.message;
			}
		}

		/// <summary>Gets the object that results from the conversion of the formatted input string.</summary>
		/// <returns>If the validation is successful, an <see cref="T:System.Object" /> that represents the converted type; otherwise, <see langword="null" />. </returns>
		// Token: 0x170011DA RID: 4570
		// (get) Token: 0x06004765 RID: 18277 RVA: 0x00130E7D File Offset: 0x0012F07D
		public object ReturnValue
		{
			get
			{
				return this.returnValue;
			}
		}

		/// <summary>Gets the type that the formatted input string is being validated against.</summary>
		/// <returns>The target <see cref="T:System.Type" /> of the conversion process. This should never be <see langword="null" />.</returns>
		// Token: 0x170011DB RID: 4571
		// (get) Token: 0x06004766 RID: 18278 RVA: 0x00130E85 File Offset: 0x0012F085
		public Type ValidatingType
		{
			get
			{
				return this.validatingType;
			}
		}

		// Token: 0x040026B6 RID: 9910
		private Type validatingType;

		// Token: 0x040026B7 RID: 9911
		private string message;

		// Token: 0x040026B8 RID: 9912
		private bool isValidInput;

		// Token: 0x040026B9 RID: 9913
		private object returnValue;

		// Token: 0x040026BA RID: 9914
		private bool cancel;
	}
}
