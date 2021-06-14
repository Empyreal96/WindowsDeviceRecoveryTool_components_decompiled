using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Win32;
using Standard;

namespace System.Windows.Shell
{
	// Token: 0x02000152 RID: 338
	internal class WindowChromeWorker : DependencyObject
	{
		// Token: 0x06000EE9 RID: 3817 RVA: 0x0003922C File Offset: 0x0003742C
		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		public WindowChromeWorker()
		{
			this._messageTable = new List<KeyValuePair<WM, MessageHandler>>
			{
				new KeyValuePair<WM, MessageHandler>(WM.SETTEXT, new MessageHandler(this._HandleSetTextOrIcon)),
				new KeyValuePair<WM, MessageHandler>(WM.SETICON, new MessageHandler(this._HandleSetTextOrIcon)),
				new KeyValuePair<WM, MessageHandler>(WM.NCACTIVATE, new MessageHandler(this._HandleNCActivate)),
				new KeyValuePair<WM, MessageHandler>(WM.NCCALCSIZE, new MessageHandler(this._HandleNCCalcSize)),
				new KeyValuePair<WM, MessageHandler>(WM.NCHITTEST, new MessageHandler(this._HandleNCHitTest)),
				new KeyValuePair<WM, MessageHandler>(WM.NCRBUTTONUP, new MessageHandler(this._HandleNCRButtonUp)),
				new KeyValuePair<WM, MessageHandler>(WM.SIZE, new MessageHandler(this._HandleSize)),
				new KeyValuePair<WM, MessageHandler>(WM.WINDOWPOSCHANGED, new MessageHandler(this._HandleWindowPosChanged)),
				new KeyValuePair<WM, MessageHandler>(WM.DWMCOMPOSITIONCHANGED, new MessageHandler(this._HandleDwmCompositionChanged))
			};
			if (Utility.IsPresentationFrameworkVersionLessThan4)
			{
				this._messageTable.AddRange(new KeyValuePair<WM, MessageHandler>[]
				{
					new KeyValuePair<WM, MessageHandler>(WM.WININICHANGE, new MessageHandler(this._HandleSettingChange)),
					new KeyValuePair<WM, MessageHandler>(WM.ENTERSIZEMOVE, new MessageHandler(this._HandleEnterSizeMove)),
					new KeyValuePair<WM, MessageHandler>(WM.EXITSIZEMOVE, new MessageHandler(this._HandleExitSizeMove)),
					new KeyValuePair<WM, MessageHandler>(WM.MOVE, new MessageHandler(this._HandleMove))
				});
			}
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x000393C4 File Offset: 0x000375C4
		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		public void SetWindowChrome(WindowChrome newChrome)
		{
			base.VerifyAccess();
			if (newChrome == this._chromeInfo)
			{
				return;
			}
			if (this._chromeInfo != null)
			{
				this._chromeInfo.PropertyChangedThatRequiresRepaint -= this._OnChromePropertyChangedThatRequiresRepaint;
			}
			this._chromeInfo = newChrome;
			if (this._chromeInfo != null)
			{
				this._chromeInfo.PropertyChangedThatRequiresRepaint += this._OnChromePropertyChangedThatRequiresRepaint;
			}
			this._ApplyNewCustomChrome();
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x0003942C File Offset: 0x0003762C
		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		private void _OnChromePropertyChangedThatRequiresRepaint(object sender, EventArgs e)
		{
			this._UpdateFrameState(true);
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x00039438 File Offset: 0x00037638
		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		private static void _OnChromeWorkerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = (Window)d;
			WindowChromeWorker windowChromeWorker = (WindowChromeWorker)e.NewValue;
			windowChromeWorker._SetWindow(window);
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x00039460 File Offset: 0x00037660
		[SecurityCritical]
		private void _SetWindow(Window window)
		{
			this.UnsubscribeWindowEvents();
			this._window = window;
			this._hwnd = new WindowInteropHelper(this._window).Handle;
			Utility.AddDependencyPropertyChangeListener(this._window, Control.TemplateProperty, new EventHandler(this._OnWindowPropertyChangedThatRequiresTemplateFixup));
			Utility.AddDependencyPropertyChangeListener(this._window, FrameworkElement.FlowDirectionProperty, new EventHandler(this._OnWindowPropertyChangedThatRequiresTemplateFixup));
			this._window.Closed += this._UnsetWindow;
			if (IntPtr.Zero != this._hwnd)
			{
				this._hwndSource = HwndSource.FromHwnd(this._hwnd);
				this._window.ApplyTemplate();
				if (this._chromeInfo != null)
				{
					this._ApplyNewCustomChrome();
					return;
				}
			}
			else
			{
				this._window.SourceInitialized += this._WindowSourceInitialized;
			}
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x00039534 File Offset: 0x00037734
		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		private void _WindowSourceInitialized(object sender, EventArgs e)
		{
			this._hwnd = new WindowInteropHelper(this._window).Handle;
			this._hwndSource = HwndSource.FromHwnd(this._hwnd);
			if (this._chromeInfo != null)
			{
				this._ApplyNewCustomChrome();
			}
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x0003956C File Offset: 0x0003776C
		[SecurityCritical]
		private void UnsubscribeWindowEvents()
		{
			if (this._window != null)
			{
				Utility.RemoveDependencyPropertyChangeListener(this._window, Control.TemplateProperty, new EventHandler(this._OnWindowPropertyChangedThatRequiresTemplateFixup));
				Utility.RemoveDependencyPropertyChangeListener(this._window, FrameworkElement.FlowDirectionProperty, new EventHandler(this._OnWindowPropertyChangedThatRequiresTemplateFixup));
				this._window.SourceInitialized -= this._WindowSourceInitialized;
				this._window.StateChanged -= this._FixupRestoreBounds;
			}
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x000395E7 File Offset: 0x000377E7
		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		private void _UnsetWindow(object sender, EventArgs e)
		{
			this.UnsubscribeWindowEvents();
			if (this._chromeInfo != null)
			{
				this._chromeInfo.PropertyChangedThatRequiresRepaint -= this._OnChromePropertyChangedThatRequiresRepaint;
			}
			this._RestoreStandardChromeState(true);
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x00039615 File Offset: 0x00037815
		public static WindowChromeWorker GetWindowChromeWorker(Window window)
		{
			Verify.IsNotNull<Window>(window, "window");
			return (WindowChromeWorker)window.GetValue(WindowChromeWorker.WindowChromeWorkerProperty);
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x00039632 File Offset: 0x00037832
		public static void SetWindowChromeWorker(Window window, WindowChromeWorker chrome)
		{
			Verify.IsNotNull<Window>(window, "window");
			window.SetValue(WindowChromeWorker.WindowChromeWorkerProperty, chrome);
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x0003964B File Offset: 0x0003784B
		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		private void _OnWindowPropertyChangedThatRequiresTemplateFixup(object sender, EventArgs e)
		{
			if (this._chromeInfo != null && this._hwnd != IntPtr.Zero)
			{
				this._window.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new WindowChromeWorker._Action(this._FixupTemplateIssues));
			}
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x00039688 File Offset: 0x00037888
		[SecurityCritical]
		private void _ApplyNewCustomChrome()
		{
			if (this._hwnd == IntPtr.Zero || this._hwndSource.IsDisposed)
			{
				return;
			}
			if (this._chromeInfo == null)
			{
				this._RestoreStandardChromeState(false);
				return;
			}
			if (!this._isHooked)
			{
				this._hwndSource.AddHook(new HwndSourceHook(this._WndProc));
				this._isHooked = true;
			}
			this._FixupTemplateIssues();
			this._UpdateSystemMenu(new WindowState?(this._window.WindowState));
			this._UpdateFrameState(true);
			NativeMethods.SetWindowPos(this._hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOSIZE | SWP.NOZORDER);
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x00039728 File Offset: 0x00037928
		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		private void RetryFixupTemplateIssuesOnVisualChildrenAdded(object sender, EventArgs e)
		{
			if (sender == this._window)
			{
				this._window.VisualChildrenChanged -= this.RetryFixupTemplateIssuesOnVisualChildrenAdded;
				this._window.Dispatcher.BeginInvoke(DispatcherPriority.Render, new WindowChromeWorker._Action(this._FixupTemplateIssues));
			}
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x00039768 File Offset: 0x00037968
		[SecurityCritical]
		private void _FixupTemplateIssues()
		{
			if (this._window.Template == null)
			{
				return;
			}
			if (VisualTreeHelper.GetChildrenCount(this._window) == 0)
			{
				this._window.VisualChildrenChanged += this.RetryFixupTemplateIssuesOnVisualChildrenAdded;
				return;
			}
			Thickness margin = default(Thickness);
			FrameworkElement frameworkElement = (FrameworkElement)VisualTreeHelper.GetChild(this._window, 0);
			if (this._chromeInfo.NonClientFrameEdges != NonClientFrameEdges.None)
			{
				if (Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 2))
				{
					margin.Top -= SystemParameters.WindowResizeBorderThickness.Top;
				}
				if (Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 1))
				{
					margin.Left -= SystemParameters.WindowResizeBorderThickness.Left;
				}
				if (Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 8))
				{
					margin.Bottom -= SystemParameters.WindowResizeBorderThickness.Bottom;
				}
				if (Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 4))
				{
					margin.Right -= SystemParameters.WindowResizeBorderThickness.Right;
				}
			}
			if (Utility.IsPresentationFrameworkVersionLessThan4)
			{
				DpiScale dpi = this._window.GetDpi();
				RECT windowRect = NativeMethods.GetWindowRect(this._hwnd);
				RECT rect = this._GetAdjustedWindowRect(windowRect);
				Rect rect2 = DpiHelper.DeviceRectToLogical(new Rect((double)windowRect.Left, (double)windowRect.Top, (double)windowRect.Width, (double)windowRect.Height), dpi.DpiScaleX, dpi.DpiScaleY);
				Rect rect3 = DpiHelper.DeviceRectToLogical(new Rect((double)rect.Left, (double)rect.Top, (double)rect.Width, (double)rect.Height), dpi.DpiScaleX, dpi.DpiScaleY);
				if (!Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 1))
				{
					margin.Right -= SystemParameters.WindowResizeBorderThickness.Left;
				}
				if (!Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 4))
				{
					margin.Right -= SystemParameters.WindowResizeBorderThickness.Right;
				}
				if (!Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 2))
				{
					margin.Bottom -= SystemParameters.WindowResizeBorderThickness.Top;
				}
				if (!Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 8))
				{
					margin.Bottom -= SystemParameters.WindowResizeBorderThickness.Bottom;
				}
				margin.Bottom -= SystemParameters.WindowCaptionHeight;
				Transform renderTransform;
				if (this._window.FlowDirection == FlowDirection.RightToLeft)
				{
					Thickness thickness = new Thickness(rect2.Left - rect3.Left, rect2.Top - rect3.Top, rect3.Right - rect2.Right, rect3.Bottom - rect2.Bottom);
					renderTransform = new MatrixTransform(1.0, 0.0, 0.0, 1.0, -(thickness.Left + thickness.Right), 0.0);
				}
				else
				{
					renderTransform = null;
				}
				frameworkElement.RenderTransform = renderTransform;
			}
			frameworkElement.Margin = margin;
			if (Utility.IsPresentationFrameworkVersionLessThan4 && !this._isFixedUp)
			{
				this._hasUserMovedWindow = false;
				this._window.StateChanged += this._FixupRestoreBounds;
				this._isFixedUp = true;
			}
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x00039AD8 File Offset: 0x00037CD8
		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		private void _FixupRestoreBounds(object sender, EventArgs e)
		{
			if ((this._window.WindowState == WindowState.Maximized || this._window.WindowState == WindowState.Minimized) && this._hasUserMovedWindow)
			{
				DpiScale dpi = this._window.GetDpi();
				this._hasUserMovedWindow = false;
				WINDOWPLACEMENT windowPlacement = NativeMethods.GetWindowPlacement(this._hwnd);
				RECT rect = this._GetAdjustedWindowRect(new RECT
				{
					Bottom = 100,
					Right = 100
				});
				Point point = DpiHelper.DevicePixelsToLogical(new Point((double)(windowPlacement.rcNormalPosition.Left - rect.Left), (double)(windowPlacement.rcNormalPosition.Top - rect.Top)), dpi.DpiScaleX, dpi.DpiScaleY);
				this._window.Top = point.Y;
				this._window.Left = point.X;
			}
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x00039BB8 File Offset: 0x00037DB8
		[SecurityCritical]
		private RECT _GetAdjustedWindowRect(RECT rcWindow)
		{
			WS dwStyle = (WS)((int)NativeMethods.GetWindowLongPtr(this._hwnd, GWL.STYLE));
			WS_EX dwExStyle = (WS_EX)((int)NativeMethods.GetWindowLongPtr(this._hwnd, GWL.EXSTYLE));
			return NativeMethods.AdjustWindowRectEx(rcWindow, dwStyle, false, dwExStyle);
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06000EF9 RID: 3833 RVA: 0x00039BF4 File Offset: 0x00037DF4
		private bool _IsWindowDocked
		{
			[SecurityCritical]
			get
			{
				if (this._window.WindowState != WindowState.Normal)
				{
					return false;
				}
				DpiScale dpi = this._window.GetDpi();
				RECT rect = this._GetAdjustedWindowRect(new RECT
				{
					Bottom = 100,
					Right = 100
				});
				Point point = new Point(this._window.Left, this._window.Top);
				point -= (Vector)DpiHelper.DevicePixelsToLogical(new Point((double)rect.Left, (double)rect.Top), dpi.DpiScaleX, dpi.DpiScaleY);
				return this._window.RestoreBounds.Location != point;
			}
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x00039CAC File Offset: 0x00037EAC
		[SecurityCritical]
		private IntPtr _WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			foreach (KeyValuePair<WM, MessageHandler> keyValuePair in this._messageTable)
			{
				if (keyValuePair.Key == (WM)msg)
				{
					return keyValuePair.Value((WM)msg, wParam, lParam, out handled);
				}
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x00039D20 File Offset: 0x00037F20
		[SecurityCritical]
		private IntPtr _HandleSetTextOrIcon(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			bool flag = this._ModifyStyle(WS.VISIBLE, WS.OVERLAPPED);
			IntPtr result = NativeMethods.DefWindowProc(this._hwnd, uMsg, wParam, lParam);
			if (flag)
			{
				this._ModifyStyle(WS.OVERLAPPED, WS.VISIBLE);
			}
			handled = true;
			return result;
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x00039D60 File Offset: 0x00037F60
		[SecurityCritical]
		private IntPtr _HandleNCActivate(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			IntPtr result = NativeMethods.DefWindowProc(this._hwnd, WM.NCACTIVATE, wParam, new IntPtr(-1));
			handled = true;
			return result;
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x00039D8C File Offset: 0x00037F8C
		[SecurityCritical]
		private IntPtr _HandleNCCalcSize(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			if (this._chromeInfo.NonClientFrameEdges != NonClientFrameEdges.None)
			{
				DpiScale dpi = this._window.GetDpi();
				Thickness thickness = DpiHelper.LogicalThicknessToDevice(SystemParameters.WindowResizeBorderThickness, dpi.DpiScaleX, dpi.DpiScaleY);
				RECT rect = (RECT)Marshal.PtrToStructure(lParam, typeof(RECT));
				if (Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 2))
				{
					rect.Top += (int)thickness.Top;
				}
				if (Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 1))
				{
					rect.Left += (int)thickness.Left;
				}
				if (Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 8))
				{
					rect.Bottom -= (int)thickness.Bottom;
				}
				if (Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 4))
				{
					rect.Right -= (int)thickness.Right;
				}
				Marshal.StructureToPtr(rect, lParam, false);
			}
			handled = true;
			IntPtr zero = IntPtr.Zero;
			if (wParam.ToInt32() != 0)
			{
				zero = new IntPtr(768);
			}
			return zero;
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x00039EB8 File Offset: 0x000380B8
		private HT _GetHTFromResizeGripDirection(ResizeGripDirection direction)
		{
			bool flag = this._window.FlowDirection == FlowDirection.RightToLeft;
			switch (direction)
			{
			case ResizeGripDirection.TopLeft:
				if (!flag)
				{
					return HT.TOPLEFT;
				}
				return HT.TOPRIGHT;
			case ResizeGripDirection.Top:
				return HT.TOP;
			case ResizeGripDirection.TopRight:
				if (!flag)
				{
					return HT.TOPRIGHT;
				}
				return HT.TOPLEFT;
			case ResizeGripDirection.Right:
				if (!flag)
				{
					return HT.RIGHT;
				}
				return HT.LEFT;
			case ResizeGripDirection.BottomRight:
				if (!flag)
				{
					return HT.BOTTOMRIGHT;
				}
				return HT.BOTTOMLEFT;
			case ResizeGripDirection.Bottom:
				return HT.BOTTOM;
			case ResizeGripDirection.BottomLeft:
				if (!flag)
				{
					return HT.BOTTOMLEFT;
				}
				return HT.BOTTOMRIGHT;
			case ResizeGripDirection.Left:
				if (!flag)
				{
					return HT.LEFT;
				}
				return HT.RIGHT;
			default:
				return HT.NOWHERE;
			}
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x00039F3C File Offset: 0x0003813C
		[SecurityCritical]
		private IntPtr _HandleNCHitTest(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			DpiScale dpi = this._window.GetDpi();
			Point point = new Point((double)Utility.GET_X_LPARAM(lParam), (double)Utility.GET_Y_LPARAM(lParam));
			Rect deviceRectangle = this._GetWindowRect();
			Point point2 = point;
			point2.Offset(-deviceRectangle.X, -deviceRectangle.Y);
			point2 = DpiHelper.DevicePixelsToLogical(point2, dpi.DpiScaleX, dpi.DpiScaleY);
			IInputElement inputElement = this._window.InputHitTest(point2);
			if (inputElement != null)
			{
				if (WindowChrome.GetIsHitTestVisibleInChrome(inputElement))
				{
					handled = true;
					return new IntPtr(1);
				}
				ResizeGripDirection resizeGripDirection = WindowChrome.GetResizeGripDirection(inputElement);
				if (resizeGripDirection != ResizeGripDirection.None)
				{
					handled = true;
					return new IntPtr((int)this._GetHTFromResizeGripDirection(resizeGripDirection));
				}
			}
			if (this._chromeInfo.UseAeroCaptionButtons && Utility.IsOSVistaOrNewer && this._chromeInfo.GlassFrameThickness != default(Thickness) && this._isGlassEnabled)
			{
				IntPtr intPtr;
				handled = NativeMethods.DwmDefWindowProc(this._hwnd, uMsg, wParam, lParam, out intPtr);
				if (IntPtr.Zero != intPtr)
				{
					return intPtr;
				}
			}
			HT value = this._HitTestNca(DpiHelper.DeviceRectToLogical(deviceRectangle, dpi.DpiScaleX, dpi.DpiScaleY), DpiHelper.DevicePixelsToLogical(point, dpi.DpiScaleX, dpi.DpiScaleY));
			handled = true;
			return new IntPtr((int)value);
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x0003A07B File Offset: 0x0003827B
		[SecurityCritical]
		private IntPtr _HandleNCRButtonUp(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			if (2 == wParam.ToInt32())
			{
				SystemCommands.ShowSystemMenuPhysicalCoordinates(this._window, new Point((double)Utility.GET_X_LPARAM(lParam), (double)Utility.GET_Y_LPARAM(lParam)));
			}
			handled = false;
			return IntPtr.Zero;
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x0003A0B0 File Offset: 0x000382B0
		[SecurityCritical]
		private IntPtr _HandleSize(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			WindowState? assumeState = null;
			if (wParam.ToInt32() == 2)
			{
				assumeState = new WindowState?(WindowState.Maximized);
			}
			this._UpdateSystemMenu(assumeState);
			handled = false;
			return IntPtr.Zero;
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x0003A0E8 File Offset: 0x000382E8
		[SecurityCritical]
		private IntPtr _HandleWindowPosChanged(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			WINDOWPOS windowpos = (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));
			if (!Utility.IsFlagSet(windowpos.flags, 1))
			{
				this._UpdateSystemMenu(null);
				if (!this._isGlassEnabled)
				{
					this._SetRoundingRegion(new WINDOWPOS?(windowpos));
				}
			}
			handled = false;
			return IntPtr.Zero;
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x0003A145 File Offset: 0x00038345
		[SecurityCritical]
		private IntPtr _HandleDwmCompositionChanged(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			this._UpdateFrameState(false);
			handled = false;
			return IntPtr.Zero;
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x0003A157 File Offset: 0x00038357
		[SecurityCritical]
		private IntPtr _HandleSettingChange(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			this._FixupTemplateIssues();
			handled = false;
			return IntPtr.Zero;
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x0003A168 File Offset: 0x00038368
		[SecurityCritical]
		private IntPtr _HandleEnterSizeMove(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			this._isUserResizing = true;
			if (this._window.WindowState != WindowState.Maximized && !this._IsWindowDocked)
			{
				this._windowPosAtStartOfUserMove = new Point(this._window.Left, this._window.Top);
			}
			handled = false;
			return IntPtr.Zero;
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x0003A1BC File Offset: 0x000383BC
		private IntPtr _HandleExitSizeMove(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			this._isUserResizing = false;
			if (this._window.WindowState == WindowState.Maximized)
			{
				this._window.Top = this._windowPosAtStartOfUserMove.Y;
				this._window.Left = this._windowPosAtStartOfUserMove.X;
			}
			handled = false;
			return IntPtr.Zero;
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x0003A213 File Offset: 0x00038413
		private IntPtr _HandleMove(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			if (this._isUserResizing)
			{
				this._hasUserMovedWindow = true;
			}
			handled = false;
			return IntPtr.Zero;
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x0003A230 File Offset: 0x00038430
		[SecurityCritical]
		private bool _ModifyStyle(WS removeStyle, WS addStyle)
		{
			WS ws = (WS)NativeMethods.GetWindowLongPtr(this._hwnd, GWL.STYLE).ToInt32();
			WS ws2 = (ws & ~removeStyle) | addStyle;
			if (ws == ws2)
			{
				return false;
			}
			NativeMethods.SetWindowLongPtr(this._hwnd, GWL.STYLE, new IntPtr((int)ws2));
			return true;
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x0003A278 File Offset: 0x00038478
		[SecurityCritical]
		private WindowState _GetHwndState()
		{
			WINDOWPLACEMENT windowPlacement = NativeMethods.GetWindowPlacement(this._hwnd);
			SW showCmd = windowPlacement.showCmd;
			if (showCmd == SW.SHOWMINIMIZED)
			{
				return WindowState.Minimized;
			}
			if (showCmd != SW.SHOWMAXIMIZED)
			{
				return WindowState.Normal;
			}
			return WindowState.Maximized;
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x0003A2A8 File Offset: 0x000384A8
		[SecurityCritical]
		private Rect _GetWindowRect()
		{
			RECT windowRect = NativeMethods.GetWindowRect(this._hwnd);
			return new Rect((double)windowRect.Left, (double)windowRect.Top, (double)windowRect.Width, (double)windowRect.Height);
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x0003A2E8 File Offset: 0x000384E8
		[SecurityCritical]
		private void _UpdateSystemMenu(WindowState? assumeState)
		{
			WindowState windowState = assumeState ?? this._GetHwndState();
			if (assumeState != null || this._lastMenuState != windowState)
			{
				this._lastMenuState = windowState;
				bool flag = this._ModifyStyle(WS.VISIBLE, WS.OVERLAPPED);
				IntPtr systemMenu = NativeMethods.GetSystemMenu(this._hwnd, false);
				if (IntPtr.Zero != systemMenu)
				{
					WS value = (WS)NativeMethods.GetWindowLongPtr(this._hwnd, GWL.STYLE).ToInt32();
					bool flag2 = Utility.IsFlagSet((int)value, 131072);
					bool flag3 = Utility.IsFlagSet((int)value, 65536);
					bool flag4 = Utility.IsFlagSet((int)value, 262144);
					if (windowState != WindowState.Minimized)
					{
						if (windowState == WindowState.Maximized)
						{
							NativeMethods.EnableMenuItem(systemMenu, SC.RESTORE, MF.ENABLED);
							NativeMethods.EnableMenuItem(systemMenu, SC.MOVE, MF.GRAYED | MF.DISABLED);
							NativeMethods.EnableMenuItem(systemMenu, SC.SIZE, MF.GRAYED | MF.DISABLED);
							NativeMethods.EnableMenuItem(systemMenu, SC.MINIMIZE, flag2 ? MF.ENABLED : (MF.GRAYED | MF.DISABLED));
							NativeMethods.EnableMenuItem(systemMenu, SC.MAXIMIZE, MF.GRAYED | MF.DISABLED);
						}
						else
						{
							NativeMethods.EnableMenuItem(systemMenu, SC.RESTORE, MF.GRAYED | MF.DISABLED);
							NativeMethods.EnableMenuItem(systemMenu, SC.MOVE, MF.ENABLED);
							NativeMethods.EnableMenuItem(systemMenu, SC.SIZE, flag4 ? MF.ENABLED : (MF.GRAYED | MF.DISABLED));
							NativeMethods.EnableMenuItem(systemMenu, SC.MINIMIZE, flag2 ? MF.ENABLED : (MF.GRAYED | MF.DISABLED));
							NativeMethods.EnableMenuItem(systemMenu, SC.MAXIMIZE, flag3 ? MF.ENABLED : (MF.GRAYED | MF.DISABLED));
						}
					}
					else
					{
						NativeMethods.EnableMenuItem(systemMenu, SC.RESTORE, MF.ENABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.MOVE, MF.GRAYED | MF.DISABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.SIZE, MF.GRAYED | MF.DISABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.MINIMIZE, MF.GRAYED | MF.DISABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.MAXIMIZE, flag3 ? MF.ENABLED : (MF.GRAYED | MF.DISABLED));
					}
				}
				if (flag)
				{
					this._ModifyStyle(WS.OVERLAPPED, WS.VISIBLE);
				}
			}
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x0003A4A0 File Offset: 0x000386A0
		[SecurityCritical]
		private void _UpdateFrameState(bool force)
		{
			if (IntPtr.Zero == this._hwnd || this._hwndSource.IsDisposed)
			{
				return;
			}
			bool flag = NativeMethods.DwmIsCompositionEnabled();
			if (force || flag != this._isGlassEnabled)
			{
				this._isGlassEnabled = (flag && this._chromeInfo.GlassFrameThickness != default(Thickness));
				if (!this._isGlassEnabled)
				{
					this._SetRoundingRegion(null);
				}
				else
				{
					this._ClearRoundingRegion();
					this._ExtendGlassFrame();
				}
				NativeMethods.SetWindowPos(this._hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOSIZE | SWP.NOZORDER);
			}
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x0003A543 File Offset: 0x00038743
		[SecurityCritical]
		private void _ClearRoundingRegion()
		{
			NativeMethods.SetWindowRgn(this._hwnd, IntPtr.Zero, NativeMethods.IsWindowVisible(this._hwnd));
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x0003A560 File Offset: 0x00038760
		[SecurityCritical]
		private void _SetRoundingRegion(WINDOWPOS? wp)
		{
			WINDOWPLACEMENT windowPlacement = NativeMethods.GetWindowPlacement(this._hwnd);
			if (windowPlacement.showCmd == SW.SHOWMAXIMIZED)
			{
				int num;
				int num2;
				if (wp != null)
				{
					num = wp.Value.x;
					num2 = wp.Value.y;
				}
				else
				{
					Rect rect = this._GetWindowRect();
					num = (int)rect.Left;
					num2 = (int)rect.Top;
				}
				IntPtr hMonitor = NativeMethods.MonitorFromWindow(this._hwnd, 2U);
				MONITORINFO monitorInfo = NativeMethods.GetMonitorInfo(hMonitor);
				RECT rcWork = monitorInfo.rcWork;
				rcWork.Offset(-num, -num2);
				IntPtr hRgn = IntPtr.Zero;
				try
				{
					hRgn = NativeMethods.CreateRectRgnIndirect(rcWork);
					NativeMethods.SetWindowRgn(this._hwnd, hRgn, NativeMethods.IsWindowVisible(this._hwnd));
					hRgn = IntPtr.Zero;
					return;
				}
				finally
				{
					Utility.SafeDeleteObject(ref hRgn);
				}
			}
			Size size;
			if (wp != null && !Utility.IsFlagSet(wp.Value.flags, 1))
			{
				size = new Size((double)wp.Value.cx, (double)wp.Value.cy);
			}
			else
			{
				if (wp != null && this._lastRoundingState == this._window.WindowState)
				{
					return;
				}
				size = this._GetWindowRect().Size;
			}
			this._lastRoundingState = this._window.WindowState;
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				DpiScale dpi = this._window.GetDpi();
				double num3 = Math.Min(size.Width, size.Height);
				double num4 = DpiHelper.LogicalPixelsToDevice(new Point(this._chromeInfo.CornerRadius.TopLeft, 0.0), dpi.DpiScaleX, dpi.DpiScaleY).X;
				num4 = Math.Min(num4, num3 / 2.0);
				if (WindowChromeWorker._IsUniform(this._chromeInfo.CornerRadius))
				{
					intPtr = WindowChromeWorker._CreateRoundRectRgn(new Rect(size), num4);
				}
				else
				{
					intPtr = WindowChromeWorker._CreateRoundRectRgn(new Rect(0.0, 0.0, size.Width / 2.0 + num4, size.Height / 2.0 + num4), num4);
					double num5 = DpiHelper.LogicalPixelsToDevice(new Point(this._chromeInfo.CornerRadius.TopRight, 0.0), dpi.DpiScaleX, dpi.DpiScaleY).X;
					num5 = Math.Min(num5, num3 / 2.0);
					Rect region = new Rect(0.0, 0.0, size.Width / 2.0 + num5, size.Height / 2.0 + num5);
					region.Offset(size.Width / 2.0 - num5, 0.0);
					WindowChromeWorker._CreateAndCombineRoundRectRgn(intPtr, region, num5);
					double num6 = DpiHelper.LogicalPixelsToDevice(new Point(this._chromeInfo.CornerRadius.BottomLeft, 0.0), dpi.DpiScaleX, dpi.DpiScaleY).X;
					num6 = Math.Min(num6, num3 / 2.0);
					Rect region2 = new Rect(0.0, 0.0, size.Width / 2.0 + num6, size.Height / 2.0 + num6);
					region2.Offset(0.0, size.Height / 2.0 - num6);
					WindowChromeWorker._CreateAndCombineRoundRectRgn(intPtr, region2, num6);
					double num7 = DpiHelper.LogicalPixelsToDevice(new Point(this._chromeInfo.CornerRadius.BottomRight, 0.0), dpi.DpiScaleX, dpi.DpiScaleY).X;
					num7 = Math.Min(num7, num3 / 2.0);
					Rect region3 = new Rect(0.0, 0.0, size.Width / 2.0 + num7, size.Height / 2.0 + num7);
					region3.Offset(size.Width / 2.0 - num7, size.Height / 2.0 - num7);
					WindowChromeWorker._CreateAndCombineRoundRectRgn(intPtr, region3, num7);
				}
				NativeMethods.SetWindowRgn(this._hwnd, intPtr, NativeMethods.IsWindowVisible(this._hwnd));
				intPtr = IntPtr.Zero;
			}
			finally
			{
				Utility.SafeDeleteObject(ref intPtr);
			}
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x0003AA4C File Offset: 0x00038C4C
		[SecurityCritical]
		private static IntPtr _CreateRoundRectRgn(Rect region, double radius)
		{
			if (DoubleUtilities.AreClose(0.0, radius))
			{
				return NativeMethods.CreateRectRgn((int)Math.Floor(region.Left), (int)Math.Floor(region.Top), (int)Math.Ceiling(region.Right), (int)Math.Ceiling(region.Bottom));
			}
			return NativeMethods.CreateRoundRectRgn((int)Math.Floor(region.Left), (int)Math.Floor(region.Top), (int)Math.Ceiling(region.Right) + 1, (int)Math.Ceiling(region.Bottom) + 1, (int)Math.Ceiling(radius), (int)Math.Ceiling(radius));
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x0003AAF0 File Offset: 0x00038CF0
		[SecurityCritical]
		private static void _CreateAndCombineRoundRectRgn(IntPtr hrgnSource, Rect region, double radius)
		{
			IntPtr hrgnSrc = IntPtr.Zero;
			try
			{
				hrgnSrc = WindowChromeWorker._CreateRoundRectRgn(region, radius);
				if (NativeMethods.CombineRgn(hrgnSource, hrgnSource, hrgnSrc, RGN.OR) == CombineRgnResult.ERROR)
				{
					throw new InvalidOperationException("Unable to combine two HRGNs.");
				}
			}
			catch
			{
				Utility.SafeDeleteObject(ref hrgnSrc);
				throw;
			}
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x0003AB40 File Offset: 0x00038D40
		private static bool _IsUniform(CornerRadius cornerRadius)
		{
			return DoubleUtilities.AreClose(cornerRadius.BottomLeft, cornerRadius.BottomRight) && DoubleUtilities.AreClose(cornerRadius.TopLeft, cornerRadius.TopRight) && DoubleUtilities.AreClose(cornerRadius.BottomLeft, cornerRadius.TopRight);
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x0003AB94 File Offset: 0x00038D94
		[SecurityCritical]
		private void _ExtendGlassFrame()
		{
			if (!Utility.IsOSVistaOrNewer)
			{
				return;
			}
			if (IntPtr.Zero == this._hwnd)
			{
				return;
			}
			if (!NativeMethods.DwmIsCompositionEnabled())
			{
				this._hwndSource.CompositionTarget.BackgroundColor = SystemColors.WindowColor;
				return;
			}
			DpiScale dpi = this._window.GetDpi();
			this._hwndSource.CompositionTarget.BackgroundColor = Colors.Transparent;
			Thickness thickness = DpiHelper.LogicalThicknessToDevice(this._chromeInfo.GlassFrameThickness, dpi.DpiScaleX, dpi.DpiScaleY);
			if (this._chromeInfo.NonClientFrameEdges != NonClientFrameEdges.None)
			{
				Thickness thickness2 = DpiHelper.LogicalThicknessToDevice(SystemParameters.WindowResizeBorderThickness, dpi.DpiScaleX, dpi.DpiScaleY);
				if (Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 2))
				{
					thickness.Top -= thickness2.Top;
					thickness.Top = Math.Max(0.0, thickness.Top);
				}
				if (Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 1))
				{
					thickness.Left -= thickness2.Left;
					thickness.Left = Math.Max(0.0, thickness.Left);
				}
				if (Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 8))
				{
					thickness.Bottom -= thickness2.Bottom;
					thickness.Bottom = Math.Max(0.0, thickness.Bottom);
				}
				if (Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 4))
				{
					thickness.Right -= thickness2.Right;
					thickness.Right = Math.Max(0.0, thickness.Right);
				}
			}
			MARGINS margins = new MARGINS
			{
				cxLeftWidth = (int)Math.Ceiling(thickness.Left),
				cxRightWidth = (int)Math.Ceiling(thickness.Right),
				cyTopHeight = (int)Math.Ceiling(thickness.Top),
				cyBottomHeight = (int)Math.Ceiling(thickness.Bottom)
			};
			NativeMethods.DwmExtendFrameIntoClientArea(this._hwnd, ref margins);
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x0003ADBC File Offset: 0x00038FBC
		private HT _HitTestNca(Rect windowPosition, Point mousePosition)
		{
			int num = 1;
			int num2 = 1;
			bool flag = false;
			if (mousePosition.Y >= windowPosition.Top && mousePosition.Y < windowPosition.Top + this._chromeInfo.ResizeBorderThickness.Top + this._chromeInfo.CaptionHeight)
			{
				flag = (mousePosition.Y < windowPosition.Top + this._chromeInfo.ResizeBorderThickness.Top);
				num = 0;
			}
			else if (mousePosition.Y < windowPosition.Bottom && mousePosition.Y >= windowPosition.Bottom - (double)((int)this._chromeInfo.ResizeBorderThickness.Bottom))
			{
				num = 2;
			}
			if (mousePosition.X >= windowPosition.Left && mousePosition.X < windowPosition.Left + (double)((int)this._chromeInfo.ResizeBorderThickness.Left))
			{
				num2 = 0;
			}
			else if (mousePosition.X < windowPosition.Right && mousePosition.X >= windowPosition.Right - this._chromeInfo.ResizeBorderThickness.Right)
			{
				num2 = 2;
			}
			if (num == 0 && num2 != 1 && !flag)
			{
				num = 1;
			}
			HT ht = WindowChromeWorker._HitTestBorders[num, num2];
			if (ht == HT.TOP && !flag)
			{
				ht = HT.CAPTION;
			}
			return ht;
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x0003AF0C File Offset: 0x0003910C
		[SecuritySafeCritical]
		private bool GetEffectiveClientArea(ref NativeMethods.RECT rcClient)
		{
			if (this._window == null || this._chromeInfo == null)
			{
				return false;
			}
			DpiScale dpi = this._window.GetDpi();
			double captionHeight = this._chromeInfo.CaptionHeight;
			Thickness resizeBorderThickness = this._chromeInfo.ResizeBorderThickness;
			RECT windowRect = NativeMethods.GetWindowRect(this._hwnd);
			Size size = DpiHelper.DeviceSizeToLogical(new Size((double)windowRect.Width, (double)windowRect.Height), dpi.DpiScaleX, dpi.DpiScaleY);
			Point logicalPoint = new Point(resizeBorderThickness.Left, resizeBorderThickness.Top + captionHeight);
			Point logicalPoint2 = new Point(size.Width - resizeBorderThickness.Right, size.Height - resizeBorderThickness.Bottom);
			Point point = DpiHelper.LogicalPixelsToDevice(logicalPoint, dpi.DpiScaleX, dpi.DpiScaleY);
			Point point2 = DpiHelper.LogicalPixelsToDevice(logicalPoint2, dpi.DpiScaleX, dpi.DpiScaleY);
			rcClient.left = (int)point.X;
			rcClient.top = (int)point.Y;
			rcClient.right = (int)point2.X;
			rcClient.bottom = (int)point2.Y;
			return true;
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x0003B028 File Offset: 0x00039228
		[SecurityCritical]
		private void _RestoreStandardChromeState(bool isClosing)
		{
			base.VerifyAccess();
			this._UnhookCustomChrome();
			if (!isClosing && !this._hwndSource.IsDisposed)
			{
				this._RestoreFrameworkIssueFixups();
				this._RestoreGlassFrame();
				this._RestoreHrgn();
				this._window.InvalidateMeasure();
			}
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x0003B063 File Offset: 0x00039263
		[SecurityCritical]
		private void _UnhookCustomChrome()
		{
			if (this._isHooked)
			{
				this._hwndSource.RemoveHook(new HwndSourceHook(this._WndProc));
				this._isHooked = false;
			}
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x0003B08C File Offset: 0x0003928C
		[SecurityCritical]
		private void _RestoreFrameworkIssueFixups()
		{
			FrameworkElement frameworkElement = (FrameworkElement)VisualTreeHelper.GetChild(this._window, 0);
			frameworkElement.Margin = default(Thickness);
			if (Utility.IsPresentationFrameworkVersionLessThan4)
			{
				this._window.StateChanged -= this._FixupRestoreBounds;
				this._isFixedUp = false;
			}
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x0003B0E0 File Offset: 0x000392E0
		[SecurityCritical]
		private void _RestoreGlassFrame()
		{
			if (!Utility.IsOSVistaOrNewer || this._hwnd == IntPtr.Zero)
			{
				return;
			}
			this._hwndSource.CompositionTarget.BackgroundColor = SystemColors.WindowColor;
			if (NativeMethods.DwmIsCompositionEnabled())
			{
				MARGINS margins = default(MARGINS);
				NativeMethods.DwmExtendFrameIntoClientArea(this._hwnd, ref margins);
			}
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x0003B138 File Offset: 0x00039338
		[SecurityCritical]
		private void _RestoreHrgn()
		{
			this._ClearRoundingRegion();
			NativeMethods.SetWindowPos(this._hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOSIZE | SWP.NOZORDER);
		}

		// Token: 0x0400116D RID: 4461
		private const SWP _SwpFlags = SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOSIZE | SWP.NOZORDER;

		// Token: 0x0400116E RID: 4462
		private readonly List<KeyValuePair<WM, MessageHandler>> _messageTable;

		// Token: 0x0400116F RID: 4463
		private Window _window;

		// Token: 0x04001170 RID: 4464
		[SecurityCritical]
		private IntPtr _hwnd;

		// Token: 0x04001171 RID: 4465
		[SecurityCritical]
		private HwndSource _hwndSource;

		// Token: 0x04001172 RID: 4466
		private bool _isHooked;

		// Token: 0x04001173 RID: 4467
		private bool _isFixedUp;

		// Token: 0x04001174 RID: 4468
		private bool _isUserResizing;

		// Token: 0x04001175 RID: 4469
		private bool _hasUserMovedWindow;

		// Token: 0x04001176 RID: 4470
		private Point _windowPosAtStartOfUserMove;

		// Token: 0x04001177 RID: 4471
		private WindowChrome _chromeInfo;

		// Token: 0x04001178 RID: 4472
		private WindowState _lastRoundingState;

		// Token: 0x04001179 RID: 4473
		private WindowState _lastMenuState;

		// Token: 0x0400117A RID: 4474
		private bool _isGlassEnabled;

		// Token: 0x0400117B RID: 4475
		public static readonly DependencyProperty WindowChromeWorkerProperty = DependencyProperty.RegisterAttached("WindowChromeWorker", typeof(WindowChromeWorker), typeof(WindowChromeWorker), new PropertyMetadata(null, new PropertyChangedCallback(WindowChromeWorker._OnChromeWorkerChanged)));

		// Token: 0x0400117C RID: 4476
		private static readonly HT[,] _HitTestBorders = new HT[,]
		{
			{
				HT.TOPLEFT,
				HT.TOP,
				HT.TOPRIGHT
			},
			{
				HT.LEFT,
				HT.CLIENT,
				HT.RIGHT
			},
			{
				HT.BOTTOMLEFT,
				HT.BOTTOM,
				HT.BOTTOMRIGHT
			}
		};

		// Token: 0x02000842 RID: 2114
		// (Invoke) Token: 0x06007F13 RID: 32531
		private delegate void _Action();
	}
}
