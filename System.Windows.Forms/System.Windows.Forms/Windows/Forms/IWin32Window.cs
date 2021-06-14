using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Provides an interface to expose Win32 HWND handles.</summary>
	// Token: 0x0200029F RID: 671
	[Guid("458AB8A2-A1EA-4d7b-8EBE-DEE5D3D9442C")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	public interface IWin32Window
	{
		/// <summary>Gets the handle to the window represented by the implementer.</summary>
		/// <returns>A handle to the window represented by the implementer.</returns>
		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x06002705 RID: 9989
		IntPtr Handle { get; }
	}
}
