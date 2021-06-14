using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using MS.Internal;

namespace System.Windows.Controls
{
	/// <summary>Represents a rule that checks for errors that are raised by the <see cref="T:System.ComponentModel.IDataErrorInfo" /> implementation of the source object.</summary>
	// Token: 0x02000491 RID: 1169
	public sealed class DataErrorValidationRule : ValidationRule
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataErrorValidationRule" /> class.</summary>
		// Token: 0x060044E7 RID: 17639 RVA: 0x0013885E File Offset: 0x00136A5E
		public DataErrorValidationRule() : base(ValidationStep.UpdatedValue, true)
		{
		}

		/// <summary>Performs validation checks on a value.</summary>
		/// <param name="value">The value to check.</param>
		/// <param name="cultureInfo">The culture to use in this rule.</param>
		/// <returns>The result of the validation.</returns>
		// Token: 0x060044E8 RID: 17640 RVA: 0x00138868 File Offset: 0x00136A68
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			BindingGroup bindingGroup;
			if ((bindingGroup = (value as BindingGroup)) != null)
			{
				IList items = bindingGroup.Items;
				for (int i = items.Count - 1; i >= 0; i--)
				{
					IDataErrorInfo dataErrorInfo = items[i] as IDataErrorInfo;
					if (dataErrorInfo != null)
					{
						string error = dataErrorInfo.Error;
						if (!string.IsNullOrEmpty(error))
						{
							return new ValidationResult(false, error);
						}
					}
				}
			}
			else
			{
				BindingExpression bindingExpression;
				if ((bindingExpression = (value as BindingExpression)) == null)
				{
					throw new InvalidOperationException(SR.Get("ValidationRule_UnexpectedValue", new object[]
					{
						this,
						value
					}));
				}
				IDataErrorInfo dataErrorInfo2 = bindingExpression.SourceItem as IDataErrorInfo;
				string text = (dataErrorInfo2 != null) ? bindingExpression.SourcePropertyName : null;
				if (!string.IsNullOrEmpty(text))
				{
					string text2;
					try
					{
						text2 = dataErrorInfo2[text];
					}
					catch (Exception ex)
					{
						if (CriticalExceptions.IsCriticalApplicationException(ex))
						{
							throw;
						}
						text2 = null;
						if (TraceData.IsEnabled)
						{
							TraceData.Trace(TraceEventType.Error, TraceData.DataErrorInfoFailed(new object[]
							{
								text,
								dataErrorInfo2.GetType().FullName,
								ex.GetType().FullName,
								ex.Message
							}), bindingExpression);
						}
					}
					if (!string.IsNullOrEmpty(text2))
					{
						return new ValidationResult(false, text2);
					}
				}
			}
			return ValidationResult.ValidResult;
		}

		// Token: 0x040028B4 RID: 10420
		internal static readonly DataErrorValidationRule Instance = new DataErrorValidationRule();
	}
}
