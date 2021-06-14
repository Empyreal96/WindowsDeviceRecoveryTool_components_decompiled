using System;

namespace System.Windows.Controls
{
	/// <summary>Specifies the formats that an <see cref="T:System.Windows.Controls.InkCanvas" /> will accept from the Clipboard.</summary>
	// Token: 0x020004EB RID: 1259
	public enum InkCanvasClipboardFormat
	{
		/// <summary>Indicates that the <see cref="T:System.Windows.Controls.InkCanvas" /> accepts Ink Serialized Format (ISF).</summary>
		// Token: 0x04002BF9 RID: 11257
		InkSerializedFormat,
		/// <summary>Indicates that the <see cref="T:System.Windows.Controls.InkCanvas" /> accepts text.</summary>
		// Token: 0x04002BFA RID: 11258
		Text,
		/// <summary>Indicates that the <see cref="T:System.Windows.Controls.InkCanvas" /> accepts "Extensible Application Markup Language" (XAML) format.</summary>
		// Token: 0x04002BFB RID: 11259
		Xaml
	}
}
