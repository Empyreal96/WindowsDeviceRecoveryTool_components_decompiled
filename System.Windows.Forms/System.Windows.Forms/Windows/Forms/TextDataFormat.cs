using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the formats used with text-related methods of the <see cref="T:System.Windows.Forms.Clipboard" /> and <see cref="T:System.Windows.Forms.DataObject" /> classes.</summary>
	// Token: 0x02000393 RID: 915
	public enum TextDataFormat
	{
		/// <summary>Specifies the standard ANSI text format.</summary>
		// Token: 0x040022B1 RID: 8881
		Text,
		/// <summary>Specifies the standard Windows Unicode text format.</summary>
		// Token: 0x040022B2 RID: 8882
		UnicodeText,
		/// <summary>Specifies text consisting of rich text format (RTF) data.</summary>
		// Token: 0x040022B3 RID: 8883
		Rtf,
		/// <summary>Specifies text consisting of HTML data.</summary>
		// Token: 0x040022B4 RID: 8884
		Html,
		/// <summary>Specifies a comma-separated value (CSV) format, which is a common interchange format used by spreadsheets.</summary>
		// Token: 0x040022B5 RID: 8885
		CommaSeparatedValue
	}
}
