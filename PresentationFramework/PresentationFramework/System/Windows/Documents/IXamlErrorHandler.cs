using System;

namespace System.Windows.Documents
{
	// Token: 0x02000390 RID: 912
	internal interface IXamlErrorHandler
	{
		// Token: 0x0600319F RID: 12703
		void Error(string message, XamlToRtfError xamlToRtfError);

		// Token: 0x060031A0 RID: 12704
		void FatalError(string message, XamlToRtfError xamlToRtfError);

		// Token: 0x060031A1 RID: 12705
		void IgnorableWarning(string message, XamlToRtfError xamlToRtfError);
	}
}
