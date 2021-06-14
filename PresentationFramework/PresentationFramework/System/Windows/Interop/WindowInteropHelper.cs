using System;
using System.Security;
using MS.Internal;

namespace System.Windows.Interop
{
	/// <summary>Assists interoperation between Windows Presentation Foundation (WPF) and Win32 code. </summary>
	// Token: 0x020005C1 RID: 1473
	public sealed class WindowInteropHelper
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Interop.WindowInteropHelper" /> class for a specified Windows Presentation Foundation (WPF) window. </summary>
		/// <param name="window">A WPF window object.</param>
		// Token: 0x06006247 RID: 25159 RVA: 0x001B8BEE File Offset: 0x001B6DEE
		public WindowInteropHelper(Window window)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}
			this._window = window;
		}

		/// <summary>Gets the window handle for a Windows Presentation Foundation (WPF) window that is used to create this <see cref="T:System.Windows.Interop.WindowInteropHelper" />. </summary>
		/// <returns>The Windows Presentation Foundation (WPF) window handle (HWND).</returns>
		// Token: 0x1700179D RID: 6045
		// (get) Token: 0x06006248 RID: 25160 RVA: 0x001B8C0B File Offset: 0x001B6E0B
		public IntPtr Handle
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUIWindowPermission();
				return this.CriticalHandle;
			}
		}

		// Token: 0x1700179E RID: 6046
		// (get) Token: 0x06006249 RID: 25161 RVA: 0x001B8C18 File Offset: 0x001B6E18
		internal IntPtr CriticalHandle
		{
			[SecurityCritical]
			get
			{
				Invariant.Assert(this._window != null, "Cannot be null since we verify in the constructor");
				return this._window.CriticalHandle;
			}
		}

		/// <summary>Gets or sets the handle of the Windows Presentation Foundation (WPF) owner window. </summary>
		/// <returns>The owner window handle (HWND).</returns>
		// Token: 0x1700179F RID: 6047
		// (get) Token: 0x0600624A RID: 25162 RVA: 0x001B8C38 File Offset: 0x001B6E38
		// (set) Token: 0x0600624B RID: 25163 RVA: 0x001B8C4A File Offset: 0x001B6E4A
		public IntPtr Owner
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUIWindowPermission();
				return this._window.OwnerHandle;
			}
			[SecurityCritical]
			set
			{
				SecurityHelper.DemandUIWindowPermission();
				this._window.OwnerHandle = value;
			}
		}

		/// <summary>Creates the HWND of the window if the HWND has not been created yet.</summary>
		/// <returns>An <see cref="T:System.IntPtr" /> that represents the HWND.</returns>
		// Token: 0x0600624C RID: 25164 RVA: 0x001B8C5D File Offset: 0x001B6E5D
		[SecurityCritical]
		public IntPtr EnsureHandle()
		{
			SecurityHelper.DemandUIWindowPermission();
			if (this.CriticalHandle == IntPtr.Zero)
			{
				this._window.CreateSourceWindow(false);
			}
			return this.CriticalHandle;
		}

		// Token: 0x0400318C RID: 12684
		private Window _window;
	}
}
