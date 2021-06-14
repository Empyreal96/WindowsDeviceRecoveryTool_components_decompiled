using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Printing;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Navigation;
using MS.Internal.Commands;
using MS.Win32;

namespace MS.Internal.AppModel
{
	// Token: 0x0200079A RID: 1946
	internal sealed class RootBrowserWindow : NavigationWindow, IWindowService, IJournalNavigationScopeHost, INavigatorBase
	{
		// Token: 0x060079E1 RID: 31201 RVA: 0x00228A90 File Offset: 0x00226C90
		static RootBrowserWindow()
		{
			CommandHelpers.RegisterCommandHandler(typeof(RootBrowserWindow), ApplicationCommands.Print, new ExecutedRoutedEventHandler(RootBrowserWindow.OnCommandPrint), new CanExecuteRoutedEventHandler(RootBrowserWindow.OnQueryEnabledCommandPrint));
		}

		// Token: 0x060079E2 RID: 31202 RVA: 0x00228AC0 File Offset: 0x00226CC0
		[SecurityCritical]
		private RootBrowserWindow() : base(true)
		{
			bool flag = this.IsDownlevelPlatform && BrowserInteropHelper.IsHostedInIEorWebOC;
			if (!flag || !BrowserInteropHelper.IsAvalonTopLevel)
			{
				base.SetValue(KeyboardNavigation.TabNavigationProperty, KeyboardNavigationMode.Continue);
				base.SetValue(KeyboardNavigation.ControlTabNavigationProperty, KeyboardNavigationMode.Continue);
			}
		}

		// Token: 0x060079E3 RID: 31203 RVA: 0x00228B1C File Offset: 0x00226D1C
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new RootBrowserWindowAutomationPeer(this);
		}

		// Token: 0x060079E4 RID: 31204 RVA: 0x00228B24 File Offset: 0x00226D24
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override void OnInitialized(EventArgs args)
		{
			base.AddHandler(Hyperlink.RequestSetStatusBarEvent, new RoutedEventHandler(this.OnRequestSetStatusBar_Hyperlink));
			base.OnInitialized(args);
		}

		// Token: 0x060079E5 RID: 31205 RVA: 0x00228B44 File Offset: 0x00226D44
		protected override Size MeasureOverride(Size constraint)
		{
			return base.MeasureOverride(this.GetSizeInLogicalUnits());
		}

		// Token: 0x060079E6 RID: 31206 RVA: 0x00228B52 File Offset: 0x00226D52
		protected override Size ArrangeOverride(Size arrangeBounds)
		{
			base.ArrangeOverride(this.GetSizeInLogicalUnits());
			return arrangeBounds;
		}

		// Token: 0x060079E7 RID: 31207 RVA: 0x00002137 File Offset: 0x00000337
		protected override void OnStateChanged(EventArgs e)
		{
		}

		// Token: 0x060079E8 RID: 31208 RVA: 0x00002137 File Offset: 0x00000337
		protected override void OnLocationChanged(EventArgs e)
		{
		}

		// Token: 0x060079E9 RID: 31209 RVA: 0x00002137 File Offset: 0x00000337
		protected override void OnClosing(CancelEventArgs e)
		{
		}

		// Token: 0x060079EA RID: 31210 RVA: 0x00002137 File Offset: 0x00000337
		protected override void OnClosed(EventArgs e)
		{
		}

		// Token: 0x060079EB RID: 31211 RVA: 0x00228B62 File Offset: 0x00226D62
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override void OnContentRendered(EventArgs e)
		{
			base.OnContentRendered(e);
			if (!this._loadingCompletePosted)
			{
				RootBrowserWindow.Browser.PostReadyStateChange(4);
				this._loadingCompletePosted = true;
			}
		}

		// Token: 0x060079EC RID: 31212 RVA: 0x00228B88 File Offset: 0x00226D88
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.Key == Key.F6 && (e.KeyboardDevice.Modifiers & ~ModifierKeys.Shift) == ModifierKeys.None && FrameworkElement.KeyboardNavigation.Navigate((e.KeyboardDevice.FocusedElement as DependencyObject) ?? this, Key.Tab, e.KeyboardDevice.Modifiers | ModifierKeys.Control, false))
			{
				e.Handled = true;
			}
			if (!e.Handled)
			{
				base.OnKeyDown(e);
			}
		}

		// Token: 0x060079ED RID: 31213 RVA: 0x00228BF8 File Offset: 0x00226DF8
		[SecurityCritical]
		internal static RootBrowserWindow CreateAndInitialize()
		{
			RootBrowserWindow rootBrowserWindow = new RootBrowserWindow();
			rootBrowserWindow.InitializeRBWStyle();
			return rootBrowserWindow;
		}

		// Token: 0x060079EE RID: 31214 RVA: 0x00228C14 File Offset: 0x00226E14
		[SecurityCritical]
		internal override void CreateAllStyle()
		{
			Invariant.Assert(RootBrowserWindow.App != null, "RootBrowserWindow must be created in an Application");
			IHostService hostService = (IHostService)RootBrowserWindow.App.GetService(typeof(IHostService));
			Invariant.Assert(hostService != null, "IHostService in RootBrowserWindow cannot be null");
			Invariant.Assert(hostService.HostWindowHandle != IntPtr.Zero, "IHostService.HostWindowHandle in RootBrowserWindow cannot be null");
			UIPermission uipermission = new UIPermission(UIPermissionWindow.AllWindows);
			uipermission.Assert();
			try
			{
				base.OwnerHandle = hostService.HostWindowHandle;
				base.Win32Style = 1174405120;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x060079EF RID: 31215 RVA: 0x00228CB4 File Offset: 0x00226EB4
		[SecurityCritical]
		internal override HwndSourceParameters CreateHwndSourceParameters()
		{
			HwndSourceParameters result = base.CreateHwndSourceParameters();
			result.TreatAsInputRoot = true;
			result.TreatAncestorsAsNonClientArea = true;
			return result;
		}

		// Token: 0x060079F0 RID: 31216 RVA: 0x00228CDC File Offset: 0x00226EDC
		[SecurityCritical]
		internal override void CreateSourceWindowDuringShow()
		{
			RootBrowserWindow.Browser.OnBeforeShowNavigationWindow();
			base.CreateSourceWindowDuringShow();
			Invariant.Assert(!base.IsSourceWindowNull, "Failed to create HwndSourceWindow for browser hosting");
			this._sourceWindowCreationCompleted = true;
			if (this._rectSet)
			{
				this._rectSet = false;
				this.ResizeMove(this._xDeviceUnits, this._yDeviceUnits, this._widthDeviceUnits, this._heightDeviceUnits);
			}
			if (base.IsSourceWindowNull)
			{
				return;
			}
			this.SetUpInputHooks();
			IntPtr ancestor = UnsafeNativeMethods.GetAncestor(new HandleRef(this, base.CriticalHandle), 2);
			IntPtr foregroundWindow = UnsafeNativeMethods.GetForegroundWindow();
			base.HandleActivate(foregroundWindow == ancestor);
		}

		// Token: 0x060079F1 RID: 31217 RVA: 0x00002137 File Offset: 0x00000337
		internal override void TryClearingMainWindow()
		{
		}

		// Token: 0x060079F2 RID: 31218 RVA: 0x00002137 File Offset: 0x00000337
		internal override void CorrectStyleForBorderlessWindowCase()
		{
		}

		// Token: 0x060079F3 RID: 31219 RVA: 0x00228D75 File Offset: 0x00226F75
		internal override void GetRequestedDimensions(ref double requestedLeft, ref double requestedTop, ref double requestedWidth, ref double requestedHeight)
		{
			requestedTop = 0.0;
			requestedLeft = 0.0;
			requestedWidth = base.Width;
			requestedHeight = base.Height;
		}

		// Token: 0x060079F4 RID: 31220 RVA: 0x00228D9E File Offset: 0x00226F9E
		[SecurityCritical]
		internal override void SetupInitialState(double requestedTop, double requestedLeft, double requestedWidth, double requestedHeight)
		{
			this.SetBrowserSize();
			base.SetRootVisual();
		}

		// Token: 0x060079F5 RID: 31221 RVA: 0x00094E43 File Offset: 0x00093043
		internal override int nCmdForShow()
		{
			return 5;
		}

		// Token: 0x060079F6 RID: 31222 RVA: 0x0000B02A File Offset: 0x0000922A
		internal override bool HandleWmNcHitTestMsg(IntPtr lParam, ref IntPtr refInt)
		{
			return false;
		}

		// Token: 0x060079F7 RID: 31223 RVA: 0x00228DAC File Offset: 0x00226FAC
		internal override Window.WindowMinMax GetWindowMinMax()
		{
			return new Window.WindowMinMax(0.0, double.PositiveInfinity);
		}

		// Token: 0x060079F8 RID: 31224 RVA: 0x00002137 File Offset: 0x00000337
		internal override void WmMoveChangedHelper()
		{
		}

		// Token: 0x060079F9 RID: 31225 RVA: 0x00228DC8 File Offset: 0x00226FC8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void ResizeMove(int xDeviceUnits, int yDeviceUnits, int widthDeviceUnits, int heightDeviceUnits)
		{
			if (!this._sourceWindowCreationCompleted)
			{
				this._xDeviceUnits = xDeviceUnits;
				this._yDeviceUnits = yDeviceUnits;
				this._widthDeviceUnits = widthDeviceUnits;
				this._heightDeviceUnits = heightDeviceUnits;
				this._rectSet = true;
				return;
			}
			Invariant.Assert(!base.IsSourceWindowNull, "sourceWindow cannot be null if _sourceWindowCreationCompleted is true");
			HandleRef hWnd = new HandleRef(this, base.CriticalHandle);
			UnsafeNativeMethods.SetWindowPos(hWnd, NativeMethods.NullHandleRef, xDeviceUnits, yDeviceUnits, widthDeviceUnits, heightDeviceUnits, 84);
		}

		// Token: 0x060079FA RID: 31226 RVA: 0x00228E38 File Offset: 0x00227038
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override void UpdateTitle(string titleStr)
		{
			IBrowserCallbackServices browser = RootBrowserWindow.Browser;
			if (browser != null)
			{
				string title = this.PruneTitleString(titleStr);
				try
				{
					BrowserInteropHelper.HostBrowser.SetTitle(title);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147418111)
					{
						throw;
					}
				}
			}
		}

		// Token: 0x060079FB RID: 31227 RVA: 0x00228E88 File Offset: 0x00227088
		[SecurityCritical]
		internal void SetStatusBarText(string statusString)
		{
			if (BrowserInteropHelper.HostBrowser != null)
			{
				BrowserInteropHelper.HostBrowser.SetStatusText(statusString);
			}
		}

		// Token: 0x060079FC RID: 31228 RVA: 0x00228EA0 File Offset: 0x002270A0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override void UpdateHeight(double newHeightLogicalUnits)
		{
			uint num = (uint)Math.Round(base.LogicalToDeviceUnits(new Point(0.0, newHeightLogicalUnits)).Y);
			if (BrowserInteropHelper.HostBrowser != null)
			{
				uint maxWindowHeight = this.GetMaxWindowHeight();
				num = ((num > maxWindowHeight) ? maxWindowHeight : num);
				num = ((num < 200U) ? 200U : num);
				BrowserInteropHelper.HostBrowser.SetHeight(num);
			}
		}

		// Token: 0x060079FD RID: 31229 RVA: 0x00228F04 File Offset: 0x00227104
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override void UpdateWidth(double newWidthLogicalUnits)
		{
			uint num = (uint)Math.Round(base.LogicalToDeviceUnits(new Point(newWidthLogicalUnits, 0.0)).X);
			if (BrowserInteropHelper.HostBrowser != null)
			{
				uint maxWindowWidth = this.GetMaxWindowWidth();
				num = ((num > maxWindowWidth) ? maxWindowWidth : num);
				num = ((num < 200U) ? 200U : num);
				BrowserInteropHelper.HostBrowser.SetWidth(num);
			}
		}

		// Token: 0x060079FE RID: 31230 RVA: 0x00228F68 File Offset: 0x00227168
		internal void SetJournalForBrowserInterop(Journal journal)
		{
			Invariant.Assert(journal != null, "Failed to get Journal for browser integration");
			base.JournalNavigationScope.Journal = journal;
		}

		// Token: 0x060079FF RID: 31231 RVA: 0x00228F84 File Offset: 0x00227184
		void IJournalNavigationScopeHost.OnJournalAvailable()
		{
			base.Journal.BackForwardStateChange += this.HandleBackForwardStateChange;
		}

		// Token: 0x06007A00 RID: 31232 RVA: 0x00228FA0 File Offset: 0x002271A0
		[SecurityCritical]
		bool IJournalNavigationScopeHost.GoBackOverride()
		{
			if (this.HasTravelLogIntegration)
			{
				if (BrowserInteropHelper.HostBrowser != null)
				{
					try
					{
						BrowserInteropHelper.HostBrowser.GoBack();
					}
					catch (OperationCanceledException)
					{
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06007A01 RID: 31233 RVA: 0x00228FE0 File Offset: 0x002271E0
		[SecurityCritical]
		bool IJournalNavigationScopeHost.GoForwardOverride()
		{
			if (this.HasTravelLogIntegration)
			{
				if (BrowserInteropHelper.HostBrowser != null)
				{
					try
					{
						BrowserInteropHelper.HostBrowser.GoForward();
					}
					catch (OperationCanceledException)
					{
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06007A02 RID: 31234 RVA: 0x00229020 File Offset: 0x00227220
		internal override void VerifyApiSupported()
		{
			throw new InvalidOperationException(SR.Get("NotSupportedInBrowser"));
		}

		// Token: 0x06007A03 RID: 31235 RVA: 0x00002137 File Offset: 0x00000337
		internal override void ClearResizeGripControl(Control oldCtrl)
		{
		}

		// Token: 0x06007A04 RID: 31236 RVA: 0x00002137 File Offset: 0x00000337
		internal override void SetResizeGripControl(Control ctrl)
		{
		}

		// Token: 0x06007A05 RID: 31237 RVA: 0x00229031 File Offset: 0x00227231
		internal void AddLayoutUpdatedHandler()
		{
			base.LayoutUpdated += this.OnLayoutUpdated;
		}

		// Token: 0x06007A06 RID: 31238 RVA: 0x00229048 File Offset: 0x00227248
		internal void TabInto(bool forward)
		{
			TraversalRequest request = new TraversalRequest(forward ? FocusNavigationDirection.First : FocusNavigationDirection.Last);
			this.MoveFocus(request);
		}

		// Token: 0x06007A07 RID: 31239 RVA: 0x0022906C File Offset: 0x0022726C
		private void InitializeRBWStyle()
		{
			if (!this.HasTravelLogIntegration)
			{
				base.SetResourceReference(FrameworkElement.StyleProperty, SystemParameters.NavigationChromeDownLevelStyleKey);
				base.SetValue(FrameworkElement.DefaultStyleKeyProperty, SystemParameters.NavigationChromeDownLevelStyleKey);
				return;
			}
			base.SetResourceReference(FrameworkElement.StyleProperty, SystemParameters.NavigationChromeStyleKey);
			base.SetValue(FrameworkElement.DefaultStyleKeyProperty, SystemParameters.NavigationChromeStyleKey);
		}

		// Token: 0x06007A08 RID: 31240 RVA: 0x002290C4 File Offset: 0x002272C4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void SetUpInputHooks()
		{
			new UIPermission(PermissionState.Unrestricted).Assert();
			IKeyboardInputSink keyboardInputSink;
			try
			{
				this._inputPostFilter = new HwndWrapperHook(BrowserInteropHelper.PostFilterInput);
				HwndSource hwndSourceWindow = base.HwndSourceWindow;
				hwndSourceWindow.HwndWrapper.AddHookLast(this._inputPostFilter);
				keyboardInputSink = hwndSourceWindow;
			}
			finally
			{
				CodeAccessPermission.RevertAll();
			}
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
			try
			{
				keyboardInputSink.KeyboardInputSite = new RootBrowserWindow.KeyInputSite(new SecurityCriticalData<IKeyboardInputSink>(keyboardInputSink));
			}
			finally
			{
				CodeAccessPermission.RevertAll();
			}
		}

		// Token: 0x06007A09 RID: 31241 RVA: 0x00229150 File Offset: 0x00227350
		private void SetBrowserSize()
		{
			Point point = base.LogicalToDeviceUnits(new Point(base.Width, base.Height));
			if (!DoubleUtil.IsNaN(base.Width))
			{
				this.UpdateWidth(point.X);
			}
			if (!DoubleUtil.IsNaN(base.Height))
			{
				this.UpdateHeight(point.Y);
			}
		}

		// Token: 0x06007A0A RID: 31242 RVA: 0x002291AC File Offset: 0x002273AC
		private string PruneTitleString(string rawString)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			for (int i = 0; i < rawString.Length; i++)
			{
				if (!char.IsWhiteSpace(rawString[i]))
				{
					stringBuilder.Append(rawString[i]);
					flag = true;
				}
				else if (flag)
				{
					stringBuilder.Append(' ');
					flag = false;
				}
			}
			return stringBuilder.ToString().TrimEnd(new char[]
			{
				' '
			});
		}

		// Token: 0x06007A0B RID: 31243 RVA: 0x00229218 File Offset: 0x00227418
		private void OnLayoutUpdated(object obj, EventArgs args)
		{
			try
			{
				this.VerifyWebOCOverlap(base.NavigationService);
			}
			finally
			{
				this._webBrowserList.Clear();
			}
		}

		// Token: 0x06007A0C RID: 31244 RVA: 0x00229250 File Offset: 0x00227450
		private void VerifyWebOCOverlap(NavigationService navigationService)
		{
			for (int i = 0; i < navigationService.ChildNavigationServices.Count; i++)
			{
				NavigationService navigationService2 = (NavigationService)navigationService.ChildNavigationServices[i];
				WebBrowser webBrowser = navigationService2.WebBrowser;
				if (webBrowser != null)
				{
					for (int j = 0; j < this._webBrowserList.Count; j++)
					{
						Rect rect = Rect.Intersect(webBrowser.BoundRect, this._webBrowserList[j].BoundRect);
						if (rect.Width > 0.0 && rect.Height > 0.0)
						{
							throw new InvalidOperationException(SR.Get("WebBrowserOverlap"));
						}
					}
					this._webBrowserList.Add(webBrowser);
				}
				else
				{
					this.VerifyWebOCOverlap(navigationService2);
				}
			}
		}

		// Token: 0x06007A0D RID: 31245 RVA: 0x00229314 File Offset: 0x00227514
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void HandleBackForwardStateChange(object sender, EventArgs args)
		{
			if (!this.HasTravelLogIntegration)
			{
				return;
			}
			IBrowserCallbackServices browser = RootBrowserWindow.Browser;
			if (browser != null)
			{
				browser.UpdateBackForwardState();
			}
		}

		// Token: 0x06007A0E RID: 31246 RVA: 0x0022933C File Offset: 0x0022753C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private uint GetMaxWindowWidth()
		{
			NativeMethods.RECT workAreaBoundsForNearestMonitor = base.WorkAreaBoundsForNearestMonitor;
			int left = BrowserInteropHelper.HostBrowser.GetLeft();
			uint width = BrowserInteropHelper.HostBrowser.GetWidth();
			uint num = (uint)(workAreaBoundsForNearestMonitor.right - left);
			return (num > width) ? num : width;
		}

		// Token: 0x06007A0F RID: 31247 RVA: 0x0022937C File Offset: 0x0022757C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private uint GetMaxWindowHeight()
		{
			NativeMethods.RECT workAreaBoundsForNearestMonitor = base.WorkAreaBoundsForNearestMonitor;
			int top = BrowserInteropHelper.HostBrowser.GetTop();
			uint height = BrowserInteropHelper.HostBrowser.GetHeight();
			uint num = (uint)(workAreaBoundsForNearestMonitor.bottom - top);
			return (num > height) ? num : height;
		}

		// Token: 0x06007A10 RID: 31248 RVA: 0x002293BC File Offset: 0x002275BC
		private Size GetSizeInLogicalUnits()
		{
			Size windowSize;
			if (base.IsSourceWindowNull || base.IsCompositionTargetInvalid)
			{
				windowSize = new Size((double)this._widthDeviceUnits, (double)this._heightDeviceUnits);
			}
			else
			{
				windowSize = base.WindowSize;
				Point point = base.DeviceToLogicalUnits(new Point(windowSize.Width, windowSize.Height));
				windowSize = new Size(point.X, point.Y);
			}
			return windowSize;
		}

		// Token: 0x06007A11 RID: 31249 RVA: 0x00229428 File Offset: 0x00227628
		[SecurityCritical]
		private void OnRequestSetStatusBar_Hyperlink(object sender, RoutedEventArgs e)
		{
			RequestSetStatusBarEventArgs requestSetStatusBarEventArgs = e as RequestSetStatusBarEventArgs;
			if (requestSetStatusBarEventArgs != null)
			{
				this.SetStatusBarText(requestSetStatusBarEventArgs.Text);
			}
		}

		// Token: 0x06007A12 RID: 31250 RVA: 0x0022944C File Offset: 0x0022764C
		private static void OnCommandPrint(object sender, ExecutedRoutedEventArgs e)
		{
			RootBrowserWindow rootBrowserWindow = sender as RootBrowserWindow;
			Invariant.Assert(rootBrowserWindow != null);
			if (!rootBrowserWindow._isPrintingFromRBW)
			{
				Visual visual = rootBrowserWindow.Content as Visual;
				if (visual == null)
				{
					IInputElement inputElement = rootBrowserWindow.Content as IInputElement;
					if (inputElement != null)
					{
						rootBrowserWindow._isPrintingFromRBW = true;
						try
						{
							if (ApplicationCommands.Print.CanExecute(null, inputElement))
							{
								ApplicationCommands.Print.Execute(null, inputElement);
								return;
							}
						}
						finally
						{
							rootBrowserWindow._isPrintingFromRBW = false;
						}
					}
				}
				PrintDialog printDialog = new PrintDialog();
				if (printDialog.ShowDialog() == true)
				{
					string printJobDescription = RootBrowserWindow.GetPrintJobDescription(RootBrowserWindow.App.MainWindow);
					if (visual == null)
					{
						INavigatorImpl navigatorImpl = rootBrowserWindow;
						Invariant.Assert(navigatorImpl != null);
						visual = navigatorImpl.FindRootViewer();
						Invariant.Assert(visual != null);
					}
					Rect imageableRect = RootBrowserWindow.GetImageableRect(printDialog);
					VisualBrush visualBrush = new VisualBrush(visual);
					visualBrush.Stretch = Stretch.None;
					DrawingVisual drawingVisual = new DrawingVisual();
					DrawingContext drawingContext = drawingVisual.RenderOpen();
					drawingContext.DrawRectangle(visualBrush, null, new Rect(imageableRect.X, imageableRect.Y, visual.VisualDescendantBounds.Width, visual.VisualDescendantBounds.Height));
					drawingContext.Close();
					printDialog.PrintVisual(drawingVisual, printJobDescription);
				}
			}
		}

		// Token: 0x06007A13 RID: 31251 RVA: 0x002295A4 File Offset: 0x002277A4
		private static void OnQueryEnabledCommandPrint(object sender, CanExecuteRoutedEventArgs e)
		{
			RootBrowserWindow rootBrowserWindow = sender as RootBrowserWindow;
			Invariant.Assert(rootBrowserWindow != null);
			if (!e.Handled && !rootBrowserWindow._isPrintingFromRBW)
			{
				e.CanExecute = (rootBrowserWindow.Content != null);
			}
		}

		// Token: 0x06007A14 RID: 31252 RVA: 0x002295E0 File Offset: 0x002277E0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static Rect GetImageableRect(PrintDialog dialog)
		{
			Rect empty = Rect.Empty;
			Invariant.Assert(dialog != null, "Dialog should not be null.");
			PrintCapabilities printCapabilities = null;
			CodeAccessPermission codeAccessPermission = SystemDrawingHelper.NewDefaultPrintingPermission();
			codeAccessPermission.Assert();
			try
			{
				PrintQueue printQueue = dialog.PrintQueue;
				if (printQueue != null)
				{
					printCapabilities = printQueue.GetPrintCapabilities();
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			if (printCapabilities != null)
			{
				PageImageableArea pageImageableArea = printCapabilities.PageImageableArea;
				if (pageImageableArea != null)
				{
					empty = new Rect(pageImageableArea.OriginWidth, pageImageableArea.OriginHeight, pageImageableArea.ExtentWidth, pageImageableArea.ExtentHeight);
				}
			}
			if (empty == Rect.Empty)
			{
				empty = new Rect(15.0, 15.0, dialog.PrintableAreaWidth, dialog.PrintableAreaHeight);
			}
			return empty;
		}

		// Token: 0x06007A15 RID: 31253 RVA: 0x002296A0 File Offset: 0x002278A0
		private static string GetPrintJobDescription(Window window)
		{
			Invariant.Assert(window != null, "Window should not be null.");
			string text = null;
			string text2 = null;
			string text3 = window.Title;
			if (text3 != null)
			{
				text3 = text3.Trim();
			}
			Page page = window.Content as Page;
			if (page != null)
			{
				text2 = page.Title;
				if (text2 != null)
				{
					text2 = text2.Trim();
				}
			}
			if (!string.IsNullOrEmpty(text3))
			{
				if (!string.IsNullOrEmpty(text2))
				{
					text = SR.Get("PrintJobDescription", new object[]
					{
						text3,
						text2
					});
				}
				else
				{
					text = text3;
				}
			}
			if (text == null && !string.IsNullOrEmpty(text2))
			{
				text = text2;
			}
			if (text == null && BrowserInteropHelper.Source != null)
			{
				Uri source = BrowserInteropHelper.Source;
				if (source.IsFile)
				{
					text = source.LocalPath;
				}
				else
				{
					text = source.ToString();
				}
			}
			if (text == null)
			{
				text = SR.Get("UntitledPrintJobDescription");
			}
			return text;
		}

		// Token: 0x17001CC2 RID: 7362
		// (get) Token: 0x06007A16 RID: 31254 RVA: 0x00036A24 File Offset: 0x00034C24
		private static Application App
		{
			get
			{
				return Application.Current;
			}
		}

		// Token: 0x17001CC3 RID: 7363
		// (get) Token: 0x06007A17 RID: 31255 RVA: 0x0022976C File Offset: 0x0022796C
		private static IBrowserCallbackServices Browser
		{
			get
			{
				return (RootBrowserWindow.App == null) ? null : RootBrowserWindow.App.BrowserCallbackServices;
			}
		}

		// Token: 0x17001CC4 RID: 7364
		// (get) Token: 0x06007A18 RID: 31256 RVA: 0x00229790 File Offset: 0x00227990
		private bool IsDownlevelPlatform
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				if (!this._isDownlevelPlatformValid)
				{
					IBrowserCallbackServices browser = RootBrowserWindow.Browser;
					this._isDownlevelPlatform = (browser != null && browser.IsDownlevelPlatform());
					this._isDownlevelPlatformValid = true;
				}
				return this._isDownlevelPlatform;
			}
		}

		// Token: 0x17001CC5 RID: 7365
		// (get) Token: 0x06007A19 RID: 31257 RVA: 0x002297CA File Offset: 0x002279CA
		internal bool HasTravelLogIntegration
		{
			get
			{
				return !this.IsDownlevelPlatform && BrowserInteropHelper.IsAvalonTopLevel;
			}
		}

		// Token: 0x040039A6 RID: 14758
		private int _xDeviceUnits;

		// Token: 0x040039A7 RID: 14759
		private int _yDeviceUnits;

		// Token: 0x040039A8 RID: 14760
		private int _widthDeviceUnits;

		// Token: 0x040039A9 RID: 14761
		private int _heightDeviceUnits;

		// Token: 0x040039AA RID: 14762
		private bool _rectSet;

		// Token: 0x040039AB RID: 14763
		private bool _isPrintingFromRBW;

		// Token: 0x040039AC RID: 14764
		private bool _isDownlevelPlatformValid;

		// Token: 0x040039AD RID: 14765
		private bool _isDownlevelPlatform;

		// Token: 0x040039AE RID: 14766
		private bool _sourceWindowCreationCompleted;

		// Token: 0x040039AF RID: 14767
		private List<WebBrowser> _webBrowserList = new List<WebBrowser>();

		// Token: 0x040039B0 RID: 14768
		[SecurityCritical]
		private HwndWrapperHook _inputPostFilter;

		// Token: 0x040039B1 RID: 14769
		private bool _loadingCompletePosted;

		// Token: 0x040039B2 RID: 14770
		private const int READYSTATE_COMPLETE = 4;

		// Token: 0x040039B3 RID: 14771
		private const int NON_PRINTABLE_MARGIN = 15;

		// Token: 0x040039B4 RID: 14772
		private const int MIN_BROWSER_WIDTH_DEVICE_UNITS = 200;

		// Token: 0x040039B5 RID: 14773
		private const int MIN_BROWSER_HEIGHT_DEVICE_UNITS = 200;

		// Token: 0x02000B76 RID: 2934
		private class PrintVisual : ContainerVisual
		{
		}

		// Token: 0x02000B77 RID: 2935
		private class KeyInputSite : IKeyboardInputSite
		{
			// Token: 0x06008E35 RID: 36405 RVA: 0x0025B8C9 File Offset: 0x00259AC9
			internal KeyInputSite(SecurityCriticalData<IKeyboardInputSink> sink)
			{
				this._sink = sink;
			}

			// Token: 0x06008E36 RID: 36406 RVA: 0x0025B8D8 File Offset: 0x00259AD8
			[SecurityCritical]
			[SecurityTreatAsSafe]
			void IKeyboardInputSite.Unregister()
			{
				this._sink = new SecurityCriticalData<IKeyboardInputSink>(null);
			}

			// Token: 0x17001FA0 RID: 8096
			// (get) Token: 0x06008E37 RID: 36407 RVA: 0x0025B8E6 File Offset: 0x00259AE6
			IKeyboardInputSink IKeyboardInputSite.Sink
			{
				[SecurityCritical]
				get
				{
					return this._sink.Value;
				}
			}

			// Token: 0x06008E38 RID: 36408 RVA: 0x0025B8F3 File Offset: 0x00259AF3
			[SecurityCritical]
			[SecurityTreatAsSafe]
			bool IKeyboardInputSite.OnNoMoreTabStops(TraversalRequest request)
			{
				return RootBrowserWindow.Browser.TabOut(request.FocusNavigationDirection == FocusNavigationDirection.Next);
			}

			// Token: 0x04004B62 RID: 19298
			private SecurityCriticalData<IKeyboardInputSink> _sink;
		}
	}
}
