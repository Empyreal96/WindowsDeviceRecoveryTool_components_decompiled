using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies constants that define how the <see cref="T:System.Windows.Forms.WebBrowser" /> control can refresh its contents.</summary>
	// Token: 0x02000426 RID: 1062
	public enum WebBrowserRefreshOption
	{
		/// <summary>A refresh that requests a copy of the current Web page that has been cached on the server.</summary>
		// Token: 0x04002718 RID: 10008
		Normal,
		/// <summary>A refresh that requests an update only if the current Web page has expired.</summary>
		// Token: 0x04002719 RID: 10009
		IfExpired,
		/// <summary>For internal use only; do not use.</summary>
		// Token: 0x0400271A RID: 10010
		Continue,
		/// <summary>A refresh that requests the latest version of the current Web page.</summary>
		// Token: 0x0400271B RID: 10011
		Completely
	}
}
