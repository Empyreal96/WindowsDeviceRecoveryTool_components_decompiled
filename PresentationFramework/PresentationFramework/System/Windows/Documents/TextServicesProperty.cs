using System;
using System.Security;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x02000418 RID: 1048
	internal class TextServicesProperty
	{
		// Token: 0x06003C8D RID: 15501 RVA: 0x00117FE6 File Offset: 0x001161E6
		internal TextServicesProperty(TextStore textstore)
		{
			this._textstore = textstore;
		}

		// Token: 0x06003C8E RID: 15502 RVA: 0x00117FF5 File Offset: 0x001161F5
		[SecurityCritical]
		internal void OnEndEdit(UnsafeNativeMethods.ITfContext context, int ecReadOnly, UnsafeNativeMethods.ITfEditRecord editRecord)
		{
			if (this._propertyRanges == null)
			{
				this._propertyRanges = new TextServicesDisplayAttributePropertyRanges(this._textstore);
			}
			this._propertyRanges.OnEndEdit(context, ecReadOnly, editRecord);
		}

		// Token: 0x06003C8F RID: 15503 RVA: 0x00118020 File Offset: 0x00116220
		internal void OnLayoutUpdated()
		{
			TextServicesDisplayAttributePropertyRanges textServicesDisplayAttributePropertyRanges = this._propertyRanges as TextServicesDisplayAttributePropertyRanges;
			if (textServicesDisplayAttributePropertyRanges != null)
			{
				textServicesDisplayAttributePropertyRanges.OnLayoutUpdated();
			}
		}

		// Token: 0x0400262B RID: 9771
		private TextServicesPropertyRanges _propertyRanges;

		// Token: 0x0400262C RID: 9772
		private readonly TextStore _textstore;
	}
}
