using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using MS.Internal.PresentationFramework;
using MS.Win32;

namespace Microsoft.Win32
{
	/// <summary>An abstract base class for displaying common Win32 dialogs.</summary>
	// Token: 0x0200008E RID: 142
	[UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)]
	public abstract class CommonDialog
	{
		/// <summary>When overridden in a derived class, resets the properties of a common dialog to their default values.</summary>
		// Token: 0x060001DA RID: 474
		[SecurityCritical]
		public abstract void Reset();

		/// <summary>Displays a common dialog.</summary>
		/// <returns>If the user clicks the OK button of the dialog that is displayed (e.g. <see cref="T:Microsoft.Win32.OpenFileDialog" />, <see cref="T:Microsoft.Win32.SaveFileDialog" />), <see langword="true" /> is returned; otherwise, <see langword="false" />.</returns>
		// Token: 0x060001DB RID: 475 RVA: 0x0000455C File Offset: 0x0000275C
		[SecurityCritical]
		public virtual bool? ShowDialog()
		{
			this.CheckPermissionsToShowDialog();
			if (!Environment.UserInteractive)
			{
				throw new InvalidOperationException(SR.Get("CantShowModalOnNonInteractive"));
			}
			IntPtr intPtr = UnsafeNativeMethods.GetActiveWindow();
			if (intPtr == IntPtr.Zero && Application.Current != null)
			{
				intPtr = Application.Current.ParkingHwnd;
			}
			HwndWrapper hwndWrapper = null;
			bool? result;
			try
			{
				if (intPtr == IntPtr.Zero)
				{
					hwndWrapper = new HwndWrapper(0, 0, 0, 0, 0, 0, 0, "", IntPtr.Zero, null);
					intPtr = hwndWrapper.Handle;
				}
				this._hwndOwnerWindow = intPtr;
				try
				{
					ComponentDispatcher.CriticalPushModal();
					result = new bool?(this.RunDialog(intPtr));
				}
				finally
				{
					ComponentDispatcher.CriticalPopModal();
				}
			}
			finally
			{
				if (hwndWrapper != null)
				{
					hwndWrapper.Dispose();
				}
			}
			return result;
		}

		/// <summary>Displays a common dialog.</summary>
		/// <param name="owner">Handle to the window that owns the dialog. </param>
		/// <returns>If the user clicks the OK button of the dialog that is displayed (e.g. <see cref="T:Microsoft.Win32.OpenFileDialog" />, <see cref="T:Microsoft.Win32.SaveFileDialog" />), <see langword="true" /> is returned; otherwise, <see langword="false" />.</returns>
		// Token: 0x060001DC RID: 476 RVA: 0x00004624 File Offset: 0x00002824
		[SecurityCritical]
		public bool? ShowDialog(Window owner)
		{
			this.CheckPermissionsToShowDialog();
			if (owner == null)
			{
				return this.ShowDialog();
			}
			if (!Environment.UserInteractive)
			{
				throw new InvalidOperationException(SR.Get("CantShowModalOnNonInteractive"));
			}
			IntPtr criticalHandle = new WindowInteropHelper(owner).CriticalHandle;
			if (criticalHandle == IntPtr.Zero)
			{
				throw new InvalidOperationException();
			}
			this._hwndOwnerWindow = criticalHandle;
			bool? result;
			try
			{
				ComponentDispatcher.CriticalPushModal();
				result = new bool?(this.RunDialog(criticalHandle));
			}
			finally
			{
				ComponentDispatcher.CriticalPopModal();
			}
			return result;
		}

		/// <summary>Gets or sets an object associated with the dialog. This provides the ability to attach an arbitrary object to the dialog.</summary>
		/// <returns>A <see cref="T:System.Object" /> that is attached or associated with a dialog.</returns>
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060001DD RID: 477 RVA: 0x000046AC File Offset: 0x000028AC
		// (set) Token: 0x060001DE RID: 478 RVA: 0x000046B4 File Offset: 0x000028B4
		public object Tag
		{
			get
			{
				return this._userData;
			}
			set
			{
				this._userData = value;
			}
		}

		/// <summary>Defines the common dialog box hook procedure that is overridden to add specific functionality to a common dialog box.</summary>
		/// <param name="hwnd">Window handle for the Win32 dialog.</param>
		/// <param name="msg">Windows message to be processed by the Win32 dialog.</param>
		/// <param name="wParam">Parameters for dialog actions.</param>
		/// <param name="lParam">Parameters for dialog actions.</param>
		/// <returns>Always returns <see cref="F:System.IntPtr.Zero" />.</returns>
		// Token: 0x060001DF RID: 479 RVA: 0x000046BD File Offset: 0x000028BD
		[SecurityCritical]
		protected virtual IntPtr HookProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam)
		{
			if (msg == 272)
			{
				this.MoveToScreenCenter(new HandleRef(this, hwnd));
				return new IntPtr(1);
			}
			return IntPtr.Zero;
		}

		/// <summary>When overridden in a derived class, is called to display a particular type of Win32 common dialog.</summary>
		/// <param name="hwndOwner">Handle to the window that owns the dialog box. </param>
		/// <returns>If the user clicks the OK button of the dialog that is displayed (e.g. <see cref="T:Microsoft.Win32.OpenFileDialog" />, <see cref="T:Microsoft.Win32.SaveFileDialog" />), <see langword="true" /> is returned; otherwise, <see langword="false" />.</returns>
		// Token: 0x060001E0 RID: 480
		protected abstract bool RunDialog(IntPtr hwndOwner);

		/// <summary>Determines whether sufficient permissions for displaying a dialog exist.</summary>
		/// <exception cref="T:System.Security.SecurityException">if sufficient permissions do not exist to display a dialog.</exception>
		// Token: 0x060001E1 RID: 481 RVA: 0x000046E0 File Offset: 0x000028E0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected virtual void CheckPermissionsToShowDialog()
		{
			if (this._thread != Thread.CurrentThread)
			{
				throw new InvalidOperationException(SR.Get("CantShowOnDifferentThread"));
			}
			SecurityHelper.DemandUIWindowPermission();
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00004704 File Offset: 0x00002904
		[SecurityCritical]
		internal void MoveToScreenCenter(HandleRef hWnd)
		{
			IntPtr intPtr = IntPtr.Zero;
			if (this._hwndOwnerWindow != IntPtr.Zero)
			{
				intPtr = SafeNativeMethods.MonitorFromWindow(new HandleRef(this, this._hwndOwnerWindow), 2);
				if (intPtr != IntPtr.Zero)
				{
					NativeMethods.RECT rect = default(NativeMethods.RECT);
					SafeNativeMethods.GetWindowRect(hWnd, ref rect);
					Size currentSizeDeviceUnits = new Size((double)(rect.right - rect.left), (double)(rect.bottom - rect.top));
					double a = 0.0;
					double a2 = 0.0;
					Window.CalculateCenterScreenPosition(intPtr, currentSizeDeviceUnits, ref a, ref a2);
					UnsafeNativeMethods.SetWindowPos(hWnd, NativeMethods.NullHandleRef, (int)Math.Round(a), (int)Math.Round(a2), 0, 0, 21);
				}
			}
		}

		// Token: 0x04000577 RID: 1399
		private object _userData;

		// Token: 0x04000578 RID: 1400
		private Thread _thread = Thread.CurrentThread;

		// Token: 0x04000579 RID: 1401
		[SecurityCritical]
		private IntPtr _hwndOwnerWindow;
	}
}
