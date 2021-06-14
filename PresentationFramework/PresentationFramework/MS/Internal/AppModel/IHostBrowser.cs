using System;
using System.Runtime.InteropServices;
using System.Security;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x02000773 RID: 1907
	[Guid("AD5D6F02-5F4E-4D77-9FC0-381981317144")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[ComImport]
	internal interface IHostBrowser
	{
		// Token: 0x060078E9 RID: 30953
		string DetermineTopLevel(out bool pbIsTopLevel);

		// Token: 0x060078EA RID: 30954
		void Navigate(string url, string targetName = null, string headers = null);

		// Token: 0x060078EB RID: 30955
		void GoBack();

		// Token: 0x060078EC RID: 30956
		void GoForward();

		// Token: 0x060078ED RID: 30957
		void SetTitle([MarshalAs(UnmanagedType.LPWStr)] string title);

		// Token: 0x060078EE RID: 30958
		[PreserveSig]
		long SetStatusText([MarshalAs(UnmanagedType.LPWStr)] string text);

		// Token: 0x060078EF RID: 30959
		void SetWidth(uint width);

		// Token: 0x060078F0 RID: 30960
		void SetHeight(uint height);

		// Token: 0x060078F1 RID: 30961
		uint GetWidth();

		// Token: 0x060078F2 RID: 30962
		uint GetHeight();

		// Token: 0x060078F3 RID: 30963
		int GetLeft();

		// Token: 0x060078F4 RID: 30964
		int GetTop();

		// Token: 0x060078F5 RID: 30965
		string GetCookie_DoNotUse(string url, string cookieName, bool thirdParty);

		// Token: 0x060078F6 RID: 30966
		void SetCookie_NoNotUse(string url, string cookieName, string cookieData, bool thirdParty, string P3PHeader = null);

		// Token: 0x060078F7 RID: 30967
		[PreserveSig]
		HRESULT GetUserAgentString(out string userAgent);

		// Token: 0x060078F8 RID: 30968
		void CreateWebBrowserControl_DoNotUse(out IntPtr ppWebBrowser);

		// Token: 0x060078F9 RID: 30969
		void TabOut_v35SP1QFE(bool forward);
	}
}
