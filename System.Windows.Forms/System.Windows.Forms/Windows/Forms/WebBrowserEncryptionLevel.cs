using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies constants that define the encryption methods used by documents displayed in the <see cref="T:System.Windows.Forms.WebBrowser" /> control.</summary>
	// Token: 0x02000424 RID: 1060
	public enum WebBrowserEncryptionLevel
	{
		/// <summary>No security encryption.</summary>
		// Token: 0x0400270A RID: 9994
		Insecure,
		/// <summary>Multiple security encryption methods in different Web page frames.</summary>
		// Token: 0x0400270B RID: 9995
		Mixed,
		/// <summary>Unknown security encryption.</summary>
		// Token: 0x0400270C RID: 9996
		Unknown,
		/// <summary>40-bit security encryption.</summary>
		// Token: 0x0400270D RID: 9997
		Bit40,
		/// <summary>56-bit security encryption.</summary>
		// Token: 0x0400270E RID: 9998
		Bit56,
		/// <summary>Fortezza security encryption.</summary>
		// Token: 0x0400270F RID: 9999
		Fortezza,
		/// <summary>128-bit security encryption.</summary>
		// Token: 0x04002710 RID: 10000
		Bit128
	}
}
