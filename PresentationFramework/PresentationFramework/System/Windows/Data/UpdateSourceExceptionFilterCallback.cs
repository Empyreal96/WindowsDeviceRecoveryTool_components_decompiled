using System;

namespace System.Windows.Data
{
	/// <summary>Represents the method that handles exceptions that are thrown during the update of the binding source value. This must be used with the <see cref="T:System.Windows.Controls.ExceptionValidationRule" />.</summary>
	/// <param name="bindExpression">The object with the exception.</param>
	/// <param name="exception">The exception encountered.</param>
	/// <returns>An object that is typically one of the following:ValueDescription
	///             <see langword="null" />
	///           To ignore any exceptions. The default behavior (if there is no <see cref="T:System.Windows.Data.UpdateSourceExceptionFilterCallback" />) is to create a <see cref="T:System.Windows.Controls.ValidationError" /> with the exception and adds it to the <see cref="P:System.Windows.Controls.Validation.Errors" /> collection of the bound element.Any objectTo create a <see cref="T:System.Windows.Controls.ValidationError" /> object with the <see cref="P:System.Windows.Controls.ValidationError.ErrorContent" /> set to that object.The <see cref="T:System.Windows.Controls.ValidationError" /> object is added to <see cref="P:System.Windows.Controls.Validation.Errors" /> collection of the bound element.A <see cref="T:System.Windows.Controls.ValidationError" /> objectTo set the <see cref="T:System.Windows.Data.BindingExpression" /> or <see cref="T:System.Windows.Data.MultiBindingExpression" /> object as the <see cref="P:System.Windows.Controls.ValidationError.BindingInError" />. The <see cref="T:System.Windows.Controls.ValidationError" /> object is added to <see cref="P:System.Windows.Controls.Validation.Errors" /> collection of the bound element.</returns>
	// Token: 0x0200019E RID: 414
	// (Invoke) Token: 0x06001842 RID: 6210
	public delegate object UpdateSourceExceptionFilterCallback(object bindExpression, Exception exception);
}
