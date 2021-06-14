using System;
using MS.Internal;

namespace System.Windows.Controls
{
	/// <summary>Provides information for the <see cref="E:System.Windows.Controls.Validation.Error" /> attached event.</summary>
	// Token: 0x02000553 RID: 1363
	public class ValidationErrorEventArgs : RoutedEventArgs
	{
		// Token: 0x06005994 RID: 22932 RVA: 0x0018B99F File Offset: 0x00189B9F
		internal ValidationErrorEventArgs(ValidationError validationError, ValidationErrorEventAction action)
		{
			Invariant.Assert(validationError != null);
			base.RoutedEvent = Validation.ErrorEvent;
			this._validationError = validationError;
			this._action = action;
		}

		/// <summary>Gets the error that caused this <see cref="E:System.Windows.Controls.Validation.Error" /> event.</summary>
		/// <returns>The <see cref="T:System.Windows.Controls.ValidationError" /> object that caused this <see cref="E:System.Windows.Controls.Validation.Error" /> event.</returns>
		// Token: 0x170015C7 RID: 5575
		// (get) Token: 0x06005995 RID: 22933 RVA: 0x0018B9C9 File Offset: 0x00189BC9
		public ValidationError Error
		{
			get
			{
				return this._validationError;
			}
		}

		/// <summary>Gets a value that indicates whether the error is a new error or an existing error that has now been cleared.</summary>
		/// <returns>A <see cref="T:System.Windows.Controls.ValidationErrorEventAction" /> value that indicates whether the error is a new error or an existing error that has now been cleared.</returns>
		// Token: 0x170015C8 RID: 5576
		// (get) Token: 0x06005996 RID: 22934 RVA: 0x0018B9D1 File Offset: 0x00189BD1
		public ValidationErrorEventAction Action
		{
			get
			{
				return this._action;
			}
		}

		/// <summary>Invokes the specified handler in a type-specific way on the specified object.</summary>
		/// <param name="genericHandler">The generic handler to call in a type-specific way.</param>
		/// <param name="genericTarget">The object to invoke the handler on.</param>
		// Token: 0x06005997 RID: 22935 RVA: 0x0018B9DC File Offset: 0x00189BDC
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			EventHandler<ValidationErrorEventArgs> eventHandler = (EventHandler<ValidationErrorEventArgs>)genericHandler;
			eventHandler(genericTarget, this);
		}

		// Token: 0x04002F11 RID: 12049
		private ValidationError _validationError;

		// Token: 0x04002F12 RID: 12050
		private ValidationErrorEventAction _action;
	}
}
