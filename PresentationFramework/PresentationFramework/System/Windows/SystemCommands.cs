using System;
using System.Security;
using System.Security.Permissions;
using System.Windows.Input;
using System.Windows.Interop;
using Standard;

namespace System.Windows
{
	/// <summary>Defines routed commands that are common to window management.</summary>
	// Token: 0x0200010E RID: 270
	public static class SystemCommands
	{
		/// <summary>Gets a command that closes a window.</summary>
		/// <returns>A command that closes a window.</returns>
		// Token: 0x17000264 RID: 612
		// (get) Token: 0x060009F2 RID: 2546 RVA: 0x00021B75 File Offset: 0x0001FD75
		// (set) Token: 0x060009F3 RID: 2547 RVA: 0x00021B7C File Offset: 0x0001FD7C
		public static RoutedCommand CloseWindowCommand { get; private set; } = new RoutedCommand("CloseWindow", typeof(SystemCommands));

		/// <summary>Gets a command that maximizes a window.</summary>
		/// <returns>A command that maximizes a window.</returns>
		// Token: 0x17000265 RID: 613
		// (get) Token: 0x060009F4 RID: 2548 RVA: 0x00021B84 File Offset: 0x0001FD84
		// (set) Token: 0x060009F5 RID: 2549 RVA: 0x00021B8B File Offset: 0x0001FD8B
		public static RoutedCommand MaximizeWindowCommand { get; private set; } = new RoutedCommand("MaximizeWindow", typeof(SystemCommands));

		/// <summary>Gets a command that maximizes a window.</summary>
		/// <returns>A command that maximizes a window.</returns>
		// Token: 0x17000266 RID: 614
		// (get) Token: 0x060009F6 RID: 2550 RVA: 0x00021B93 File Offset: 0x0001FD93
		// (set) Token: 0x060009F7 RID: 2551 RVA: 0x00021B9A File Offset: 0x0001FD9A
		public static RoutedCommand MinimizeWindowCommand { get; private set; } = new RoutedCommand("MinimizeWindow", typeof(SystemCommands));

		/// <summary>Gets a command that restores a window.</summary>
		/// <returns>A command that restores a window.</returns>
		// Token: 0x17000267 RID: 615
		// (get) Token: 0x060009F8 RID: 2552 RVA: 0x00021BA2 File Offset: 0x0001FDA2
		// (set) Token: 0x060009F9 RID: 2553 RVA: 0x00021BA9 File Offset: 0x0001FDA9
		public static RoutedCommand RestoreWindowCommand { get; private set; } = new RoutedCommand("RestoreWindow", typeof(SystemCommands));

		/// <summary>Gets a command that displays the system menu.</summary>
		/// <returns>A command that displays the system menu.</returns>
		// Token: 0x17000268 RID: 616
		// (get) Token: 0x060009FA RID: 2554 RVA: 0x00021BB1 File Offset: 0x0001FDB1
		// (set) Token: 0x060009FB RID: 2555 RVA: 0x00021BB8 File Offset: 0x0001FDB8
		public static RoutedCommand ShowSystemMenuCommand { get; private set; } = new RoutedCommand("ShowSystemMenu", typeof(SystemCommands));

		// Token: 0x060009FD RID: 2557 RVA: 0x00021C4C File Offset: 0x0001FE4C
		[SecurityCritical]
		private static void _PostSystemCommand(Window window, SC command)
		{
			IntPtr handle = new WindowInteropHelper(window).Handle;
			if (handle == IntPtr.Zero || !NativeMethods.IsWindow(handle))
			{
				return;
			}
			NativeMethods.PostMessage(handle, WM.SYSCOMMAND, new IntPtr((int)command), IntPtr.Zero);
		}

		/// <summary>Closes the specified window.</summary>
		/// <param name="window">The window to close.</param>
		// Token: 0x060009FE RID: 2558 RVA: 0x00021C91 File Offset: 0x0001FE91
		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		public static void CloseWindow(Window window)
		{
			Verify.IsNotNull<Window>(window, "window");
			SystemCommands._PostSystemCommand(window, SC.CLOSE);
		}

		/// <summary>
		///     Maximizes the specified window.</summary>
		/// <param name="window">The window to maximize.</param>
		// Token: 0x060009FF RID: 2559 RVA: 0x00021CA9 File Offset: 0x0001FEA9
		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		public static void MaximizeWindow(Window window)
		{
			Verify.IsNotNull<Window>(window, "window");
			SystemCommands._PostSystemCommand(window, SC.MAXIMIZE);
		}

		/// <summary>Minimizes the specified window.</summary>
		/// <param name="window">The window to minimize.</param>
		// Token: 0x06000A00 RID: 2560 RVA: 0x00021CC1 File Offset: 0x0001FEC1
		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		public static void MinimizeWindow(Window window)
		{
			Verify.IsNotNull<Window>(window, "window");
			SystemCommands._PostSystemCommand(window, SC.MINIMIZE);
		}

		/// <summary>Restores the specified widow.</summary>
		/// <param name="window">The window to restore.</param>
		// Token: 0x06000A01 RID: 2561 RVA: 0x00021CD9 File Offset: 0x0001FED9
		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		public static void RestoreWindow(Window window)
		{
			Verify.IsNotNull<Window>(window, "window");
			SystemCommands._PostSystemCommand(window, SC.RESTORE);
		}

		/// <summary>Displays the system menu for the specified window.</summary>
		/// <param name="window">The window to have its system menu displayed.</param>
		/// <param name="screenLocation">The location of the system menu.</param>
		// Token: 0x06000A02 RID: 2562 RVA: 0x00021CF4 File Offset: 0x0001FEF4
		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		public static void ShowSystemMenu(Window window, Point screenLocation)
		{
			Verify.IsNotNull<Window>(window, "window");
			DpiScale dpi = window.GetDpi();
			SystemCommands.ShowSystemMenuPhysicalCoordinates(window, DpiHelper.LogicalPixelsToDevice(screenLocation, dpi.DpiScaleX, dpi.DpiScaleY));
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x00021D30 File Offset: 0x0001FF30
		[SecurityCritical]
		internal static void ShowSystemMenuPhysicalCoordinates(Window window, Point physicalScreenLocation)
		{
			Verify.IsNotNull<Window>(window, "window");
			IntPtr handle = new WindowInteropHelper(window).Handle;
			if (handle == IntPtr.Zero || !NativeMethods.IsWindow(handle))
			{
				return;
			}
			IntPtr systemMenu = NativeMethods.GetSystemMenu(handle, false);
			uint num = NativeMethods.TrackPopupMenuEx(systemMenu, 256U, (int)physicalScreenLocation.X, (int)physicalScreenLocation.Y, handle, IntPtr.Zero);
			if (num != 0U)
			{
				NativeMethods.PostMessage(handle, WM.SYSCOMMAND, new IntPtr((long)((ulong)num)), IntPtr.Zero);
			}
		}
	}
}
