using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the types of input and output streams used to load and save data in the <see cref="T:System.Windows.Forms.RichTextBox" /> control.</summary>
	// Token: 0x0200033C RID: 828
	public enum RichTextBoxStreamType
	{
		/// <summary>A Rich Text Format (RTF) stream.</summary>
		// Token: 0x0400202B RID: 8235
		RichText,
		/// <summary>A plain text stream that includes spaces in places of Object Linking and Embedding (OLE) objects.</summary>
		// Token: 0x0400202C RID: 8236
		PlainText,
		/// <summary>A Rich Text Format (RTF) stream with spaces in place of OLE objects. This value is only valid for use with the <see cref="M:System.Windows.Forms.RichTextBox.SaveFile(System.String)" /> method of the <see cref="T:System.Windows.Forms.RichTextBox" /> control.</summary>
		// Token: 0x0400202D RID: 8237
		RichNoOleObjs,
		/// <summary>A plain text stream with a textual representation of OLE objects. This value is only valid for use with the <see cref="M:System.Windows.Forms.RichTextBox.SaveFile(System.String)" /> method of the <see cref="T:System.Windows.Forms.RichTextBox" /> control.</summary>
		// Token: 0x0400202E RID: 8238
		TextTextOleObjs,
		/// <summary>A text stream that contains spaces in place of Object Linking and Embedding (OLE) objects. The text is encoded in Unicode.</summary>
		// Token: 0x0400202F RID: 8239
		UnicodePlainText
	}
}
