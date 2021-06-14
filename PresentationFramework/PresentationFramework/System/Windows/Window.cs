using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shell;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Internal.Interop;
using MS.Internal.KnownBoxes;
using MS.Win32;

namespace System.Windows
{
	/// <summary>Provides the ability to create, configure, show, and manage the lifetime of windows and dialog boxes.</summary>
	// Token: 0x0200013B RID: 315
	[Localizability(LocalizationCategory.Ignore)]
	[UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)]
	public class Window : ContentControl, IWindowService
	{
		// Token: 0x06000D1B RID: 3355 RVA: 0x00030BA0 File Offset: 0x0002EDA0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		static Window()
		{
			FrameworkElement.HeightProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(new PropertyChangedCallback(Window._OnHeightChanged)));
			FrameworkElement.MinHeightProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(new PropertyChangedCallback(Window._OnMinHeightChanged)));
			FrameworkElement.MaxHeightProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(new PropertyChangedCallback(Window._OnMaxHeightChanged)));
			FrameworkElement.WidthProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(new PropertyChangedCallback(Window._OnWidthChanged)));
			FrameworkElement.MinWidthProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(new PropertyChangedCallback(Window._OnMinWidthChanged)));
			FrameworkElement.MaxWidthProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(new PropertyChangedCallback(Window._OnMaxWidthChanged)));
			UIElement.VisibilityProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(Visibility.Collapsed, new PropertyChangedCallback(Window._OnVisibilityChanged), new CoerceValueCallback(Window.CoerceVisibility)));
			Control.IsTabStopProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
			KeyboardNavigation.ControlTabNavigationProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
			FocusManager.IsFocusScopeProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(typeof(Window)));
			Window._dType = DependencyObjectType.FromSystemTypeInternal(typeof(Window));
			FrameworkElement.FlowDirectionProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(new PropertyChangedCallback(Window._OnFlowDirectionChanged)));
			UIElement.RenderTransformProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(Transform.Identity, new PropertyChangedCallback(Window._OnRenderTransformChanged), new CoerceValueCallback(Window.CoerceRenderTransform)));
			UIElement.ClipToBoundsProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(Window._OnClipToBoundsChanged), new CoerceValueCallback(Window.CoerceClipToBounds)));
			Window.WM_TASKBARBUTTONCREATED = UnsafeNativeMethods.RegisterWindowMessage("TaskbarButtonCreated");
			Window.WM_APPLYTASKBARITEMINFO = UnsafeNativeMethods.RegisterWindowMessage("WPF_ApplyTaskbarItemInfo");
			EventManager.RegisterClassHandler(typeof(Window), UIElement.ManipulationCompletedEvent, new EventHandler<ManipulationCompletedEventArgs>(Window.OnStaticManipulationCompleted), true);
			EventManager.RegisterClassHandler(typeof(Window), UIElement.ManipulationInertiaStartingEvent, new EventHandler<ManipulationInertiaStartingEventArgs>(Window.OnStaticManipulationInertiaStarting), true);
			Window.DpiChangedEvent = EventManager.RegisterRoutedEvent("DpiChanged", RoutingStrategy.Bubble, typeof(DpiChangedEventHandler), typeof(Window));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Window" /> class. </summary>
		// Token: 0x06000D1C RID: 3356 RVA: 0x00031320 File Offset: 0x0002F520
		[SecurityCritical]
		public Window()
		{
			SecurityHelper.DemandUnmanagedCode();
			this._inTrustedSubWindow = false;
			this.Initialize();
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x000313E0 File Offset: 0x0002F5E0
		[SecurityCritical]
		internal Window(bool inRbw)
		{
			if (inRbw)
			{
				this._inTrustedSubWindow = true;
			}
			else
			{
				this._inTrustedSubWindow = false;
				SecurityHelper.DemandUnmanagedCode();
			}
			this.Initialize();
		}

		/// <summary>Opens a window and returns without waiting for the newly opened window to close.</summary>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="M:System.Windows.Window.Show" /> is called on a window that is closing (<see cref="E:System.Windows.Window.Closing" />) or has been closed (<see cref="E:System.Windows.Window.Closed" />).</exception>
		// Token: 0x06000D1E RID: 3358 RVA: 0x000314AC File Offset: 0x0002F6AC
		public void Show()
		{
			this.VerifyContextAndObjectState();
			this.VerifyCanShow();
			this.VerifyNotClosing();
			this.VerifyConsistencyWithAllowsTransparency();
			this.UpdateVisibilityProperty(Visibility.Visible);
			this.ShowHelper(BooleanBoxes.TrueBox);
		}

		/// <summary>Makes a window invisible.</summary>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="M:System.Windows.Window.Hide" /> is called on a window that is closing (<see cref="E:System.Windows.Window.Closing" />) or has been closed (<see cref="E:System.Windows.Window.Closed" />).</exception>
		// Token: 0x06000D1F RID: 3359 RVA: 0x000314D9 File Offset: 0x0002F6D9
		public void Hide()
		{
			this.VerifyContextAndObjectState();
			if (this._disposed)
			{
				return;
			}
			this.UpdateVisibilityProperty(Visibility.Hidden);
			this.ShowHelper(BooleanBoxes.FalseBox);
		}

		/// <summary>Manually closes a <see cref="T:System.Windows.Window" />.</summary>
		// Token: 0x06000D20 RID: 3360 RVA: 0x000314FD File Offset: 0x0002F6FD
		[SecurityCritical]
		public void Close()
		{
			this.VerifyApiSupported();
			SecurityHelper.DemandUIWindowPermission();
			this.VerifyContextAndObjectState();
			this.InternalClose(false, false);
		}

		/// <summary>Allows a window to be dragged by a mouse with its left button down over an exposed area of the window's client area.</summary>
		/// <exception cref="T:System.InvalidOperationException">The left mouse button is not down.</exception>
		// Token: 0x06000D21 RID: 3361 RVA: 0x00031518 File Offset: 0x0002F718
		[SecurityCritical]
		public void DragMove()
		{
			this.VerifyApiSupported();
			SecurityHelper.DemandUIWindowPermission();
			this.VerifyContextAndObjectState();
			this.VerifyHwndCreateShowState();
			if (this.IsSourceWindowNull || this.IsCompositionTargetInvalid)
			{
				return;
			}
			if (Mouse.LeftButton != MouseButtonState.Pressed)
			{
				throw new InvalidOperationException(SR.Get("DragMoveFail"));
			}
			if (this.WindowState == WindowState.Normal)
			{
				UnsafeNativeMethods.SendMessage(this.CriticalHandle, WindowMessage.WM_SYSCOMMAND, (IntPtr)61458, IntPtr.Zero);
				UnsafeNativeMethods.SendMessage(this.CriticalHandle, WindowMessage.WM_LBUTTONUP, IntPtr.Zero, IntPtr.Zero);
				return;
			}
		}

		/// <summary>Opens a window and returns only when the newly opened window is closed.</summary>
		/// <returns>A <see cref="T:System.Nullable`1" /> value of type <see cref="T:System.Boolean" /> that specifies whether the activity was accepted (<see langword="true" />) or canceled (<see langword="false" />). The return value is the value of the <see cref="P:System.Windows.Window.DialogResult" /> property before a window closes.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="M:System.Windows.Window.ShowDialog" /> is called on a <see cref="T:System.Windows.Window" /> that is visible-or-
		///         <see cref="M:System.Windows.Window.ShowDialog" /> is called on a visible <see cref="T:System.Windows.Window" /> that was opened by calling <see cref="M:System.Windows.Window.ShowDialog" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="M:System.Windows.Window.ShowDialog" /> is called on a window that is closing (<see cref="E:System.Windows.Window.Closing" />) or has been closed (<see cref="E:System.Windows.Window.Closed" />).</exception>
		// Token: 0x06000D22 RID: 3362 RVA: 0x000315AC File Offset: 0x0002F7AC
		[SecurityCritical]
		public bool? ShowDialog()
		{
			this.VerifyApiSupported();
			SecurityHelper.DemandUnrestrictedUIPermission();
			this.VerifyContextAndObjectState();
			this.VerifyCanShow();
			this.VerifyNotClosing();
			this.VerifyConsistencyWithAllowsTransparency();
			if (this._isVisible)
			{
				throw new InvalidOperationException(SR.Get("ShowDialogOnVisible"));
			}
			if (this._showingAsDialog)
			{
				throw new InvalidOperationException(SR.Get("ShowDialogOnModal"));
			}
			this._dialogOwnerHandle = this._ownerHandle;
			if (!UnsafeNativeMethods.IsWindow(new HandleRef(null, this._dialogOwnerHandle)))
			{
				this._dialogOwnerHandle = IntPtr.Zero;
			}
			this._dialogPreviousActiveHandle = UnsafeNativeMethods.GetActiveWindow();
			if (this._dialogOwnerHandle == IntPtr.Zero)
			{
				this._dialogOwnerHandle = this._dialogPreviousActiveHandle;
			}
			if (this._dialogOwnerHandle != IntPtr.Zero && this._dialogOwnerHandle == UnsafeNativeMethods.GetDesktopWindow())
			{
				this._dialogOwnerHandle = IntPtr.Zero;
			}
			if (this._dialogOwnerHandle != IntPtr.Zero)
			{
				while (this._dialogOwnerHandle != IntPtr.Zero)
				{
					int windowLong = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, this._dialogOwnerHandle), -16);
					if ((windowLong & 1073741824) != 1073741824)
					{
						break;
					}
					this._dialogOwnerHandle = UnsafeNativeMethods.GetParent(new HandleRef(null, this._dialogOwnerHandle));
				}
			}
			this._threadWindowHandles = new ArrayList();
			UnsafeNativeMethods.EnumThreadWindows(SafeNativeMethods.GetCurrentThreadId(), new NativeMethods.EnumThreadWindowsCallback(this.ThreadWindowsCallback), NativeMethods.NullHandleRef);
			this.EnableThreadWindows(false);
			IntPtr capture = SafeNativeMethods.GetCapture();
			if (capture != IntPtr.Zero)
			{
				SafeNativeMethods.ReleaseCapture();
			}
			this.EnsureDialogCommand();
			try
			{
				this._showingAsDialog = true;
				this.Show();
			}
			catch
			{
				if (this._threadWindowHandles != null)
				{
					this.EnableThreadWindows(true);
				}
				if (this._dialogPreviousActiveHandle != IntPtr.Zero && UnsafeNativeMethods.IsWindow(new HandleRef(null, this._dialogPreviousActiveHandle)))
				{
					UnsafeNativeMethods.TrySetFocus(new HandleRef(null, this._dialogPreviousActiveHandle), ref this._dialogPreviousActiveHandle);
				}
				this.ClearShowKeyboardCueState();
				this._showingAsDialog = false;
				throw;
			}
			finally
			{
				this._showingAsDialog = false;
			}
			return this._dialogResult;
		}

		/// <summary>Attempts to bring the window to the foreground and activates it.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Window" /> was successfully activated; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000D23 RID: 3363 RVA: 0x000317D4 File Offset: 0x0002F9D4
		[SecurityCritical]
		public bool Activate()
		{
			this.VerifyApiSupported();
			SecurityHelper.DemandUIWindowPermission();
			this.VerifyContextAndObjectState();
			this.VerifyHwndCreateShowState();
			return !this.IsSourceWindowNull && !this.IsCompositionTargetInvalid && UnsafeNativeMethods.SetForegroundWindow(new HandleRef(null, this.CriticalHandle));
		}

		/// <summary>Gets an enumerator for a window's logical child elements.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> a window's logical child elements.</returns>
		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06000D24 RID: 3364 RVA: 0x00031810 File Offset: 0x0002FA10
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				return new SingleChildEnumerator(base.Content);
			}
		}

		/// <summary>Returns a reference to the <see cref="T:System.Windows.Window" /> object that hosts the content tree within which the dependency object is located.</summary>
		/// <param name="dependencyObject">The dependency object.</param>
		/// <returns>A <see cref="T:System.Windows.Window" /> reference to the host window.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dependencyObject" /> is null.</exception>
		// Token: 0x06000D25 RID: 3365 RVA: 0x0003181D File Offset: 0x0002FA1D
		public static Window GetWindow(DependencyObject dependencyObject)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			return dependencyObject.GetValue(Window.IWindowServiceProperty) as Window;
		}

		/// <summary>Gets or sets the Windows 7 taskbar thumbnail for the <see cref="T:System.Windows.Window" />.</summary>
		/// <returns>The Windows 7 taskbar thumbnail for the <see cref="T:System.Windows.Window" />.</returns>
		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06000D26 RID: 3366 RVA: 0x0003183D File Offset: 0x0002FA3D
		// (set) Token: 0x06000D27 RID: 3367 RVA: 0x00031855 File Offset: 0x0002FA55
		public TaskbarItemInfo TaskbarItemInfo
		{
			get
			{
				this.VerifyContextAndObjectState();
				return (TaskbarItemInfo)base.GetValue(Window.TaskbarItemInfoProperty);
			}
			set
			{
				this.VerifyContextAndObjectState();
				base.SetValue(Window.TaskbarItemInfoProperty, value);
			}
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x0003186C File Offset: 0x0002FA6C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void OnTaskbarItemInfoChanged(DependencyPropertyChangedEventArgs e)
		{
			TaskbarItemInfo taskbarItemInfo = (TaskbarItemInfo)e.OldValue;
			TaskbarItemInfo taskbarItemInfo2 = (TaskbarItemInfo)e.NewValue;
			if (!Utilities.IsOSWindows7OrNewer)
			{
				return;
			}
			if (!e.IsASubPropertyChange)
			{
				if (taskbarItemInfo != null)
				{
					taskbarItemInfo.PropertyChanged -= this.OnTaskbarItemInfoSubPropertyChanged;
				}
				if (taskbarItemInfo2 != null)
				{
					taskbarItemInfo2.PropertyChanged += this.OnTaskbarItemInfoSubPropertyChanged;
				}
				this.ApplyTaskbarItemInfo();
			}
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x000318D4 File Offset: 0x0002FAD4
		[SecurityCritical]
		private void HandleTaskbarListError(HRESULT hr)
		{
			if (hr.Failed)
			{
				if (hr == (HRESULT)Win32Error.ERROR_TIMEOUT)
				{
					if (TraceShell.IsEnabled)
					{
						TraceShell.Trace(TraceEventType.Error, TraceShell.ExplorerTaskbarTimeout);
						TraceShell.Trace(TraceEventType.Warning, TraceShell.ExplorerTaskbarRetrying);
					}
					this._taskbarRetryTimer.Start();
					return;
				}
				if (hr == (HRESULT)Win32Error.ERROR_INVALID_WINDOW_HANDLE)
				{
					if (TraceShell.IsEnabled)
					{
						TraceShell.Trace(TraceEventType.Warning, TraceShell.ExplorerTaskbarNotRunning);
					}
					Utilities.SafeRelease<ITaskbarList3>(ref this._taskbarList);
					return;
				}
				if (TraceShell.IsEnabled)
				{
					TraceShell.Trace(TraceEventType.Error, TraceShell.NativeTaskbarError(new object[]
					{
						hr.ToString()
					}));
				}
			}
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x00031980 File Offset: 0x0002FB80
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void OnTaskbarItemInfoSubPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender != this.TaskbarItemInfo)
			{
				return;
			}
			if (this._taskbarList == null)
			{
				return;
			}
			if (this._taskbarRetryTimer != null && this._taskbarRetryTimer.IsEnabled)
			{
				return;
			}
			DependencyProperty property = e.Property;
			HRESULT hr = HRESULT.S_OK;
			if (property == TaskbarItemInfo.ProgressStateProperty)
			{
				hr = this.UpdateTaskbarProgressState();
			}
			else if (property == TaskbarItemInfo.ProgressValueProperty)
			{
				hr = this.UpdateTaskbarProgressValue();
			}
			else if (property == TaskbarItemInfo.OverlayProperty)
			{
				hr = this.UpdateTaskbarOverlay();
			}
			else if (property == TaskbarItemInfo.DescriptionProperty)
			{
				hr = this.UpdateTaskbarDescription();
			}
			else if (property == TaskbarItemInfo.ThumbnailClipMarginProperty)
			{
				hr = this.UpdateTaskbarThumbnailClipping();
			}
			else if (property == TaskbarItemInfo.ThumbButtonInfosProperty)
			{
				hr = this.UpdateTaskbarThumbButtons();
			}
			this.HandleTaskbarListError(hr);
		}

		/// <summary>Gets or sets a value that indicates whether a window's client area supports transparency. </summary>
		/// <returns>
		///     <see langword="true" /> if the window supports transparency; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Window.AllowsTransparency" /> is changed after a window has been shown.</exception>
		/// <exception cref="T:System.InvalidOperationException">A window that has a <see cref="P:System.Windows.Window.WindowStyle" /> value of anything other than <see cref="F:System.Windows.WindowStyle.None" />.</exception>
		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x06000D2B RID: 3371 RVA: 0x00031A2F File Offset: 0x0002FC2F
		// (set) Token: 0x06000D2C RID: 3372 RVA: 0x00031A41 File Offset: 0x0002FC41
		public bool AllowsTransparency
		{
			get
			{
				return (bool)base.GetValue(Window.AllowsTransparencyProperty);
			}
			set
			{
				base.SetValue(Window.AllowsTransparencyProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x00002137 File Offset: 0x00000337
		private static void OnAllowsTransparencyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x00031A54 File Offset: 0x0002FC54
		private static object CoerceAllowsTransparency(DependencyObject d, object value)
		{
			value = Window.VerifyAccessCoercion(d, value);
			if (!((Window)d).IsSourceWindowNull)
			{
				throw new InvalidOperationException(SR.Get("ChangeNotAllowedAfterShow"));
			}
			return value;
		}

		/// <summary>Gets or sets a window's title. </summary>
		/// <returns>A <see cref="T:System.String" /> that contains the window's title.</returns>
		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06000D2F RID: 3375 RVA: 0x00031A7D File Offset: 0x0002FC7D
		// (set) Token: 0x06000D30 RID: 3376 RVA: 0x00031A95 File Offset: 0x0002FC95
		[Localizability(LocalizationCategory.Title)]
		public string Title
		{
			get
			{
				this.VerifyContextAndObjectState();
				return (string)base.GetValue(Window.TitleProperty);
			}
			set
			{
				this.VerifyContextAndObjectState();
				base.SetValue(Window.TitleProperty, value);
			}
		}

		/// <summary>Gets or sets a window's icon.  </summary>
		/// <returns>An <see cref="T:System.Windows.Media.ImageSource" /> object that represents the icon.</returns>
		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06000D31 RID: 3377 RVA: 0x00031AA9 File Offset: 0x0002FCA9
		// (set) Token: 0x06000D32 RID: 3378 RVA: 0x00031AC7 File Offset: 0x0002FCC7
		public ImageSource Icon
		{
			get
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				return (ImageSource)base.GetValue(Window.IconProperty);
			}
			[SecurityCritical]
			set
			{
				this.VerifyApiSupported();
				SecurityHelper.DemandUIWindowPermission();
				this.VerifyContextAndObjectState();
				base.SetValue(Window.IconProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a window will automatically size itself to fit the size of its content. </summary>
		/// <returns>A <see cref="T:System.Windows.SizeToContent" /> value. The default is <see cref="F:System.Windows.SizeToContent.Manual" />.</returns>
		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06000D33 RID: 3379 RVA: 0x00031AE6 File Offset: 0x0002FCE6
		// (set) Token: 0x06000D34 RID: 3380 RVA: 0x00031B04 File Offset: 0x0002FD04
		public SizeToContent SizeToContent
		{
			get
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				return (SizeToContent)base.GetValue(Window.SizeToContentProperty);
			}
			set
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				base.SetValue(Window.SizeToContentProperty, value);
			}
		}

		/// <summary>Gets or sets the position of the window's top edge, in relation to the desktop. </summary>
		/// <returns>The position of the window's top, in logical units (1/96").</returns>
		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06000D35 RID: 3381 RVA: 0x00031B23 File Offset: 0x0002FD23
		// (set) Token: 0x06000D36 RID: 3382 RVA: 0x00031B41 File Offset: 0x0002FD41
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		public double Top
		{
			get
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				return (double)base.GetValue(Window.TopProperty);
			}
			set
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				base.SetValue(Window.TopProperty, value);
			}
		}

		/// <summary>Gets or sets the position of the window's left edge, in relation to the desktop. </summary>
		/// <returns>The position of the window's left edge, in logical units (1/96th of an inch).</returns>
		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06000D37 RID: 3383 RVA: 0x00031B60 File Offset: 0x0002FD60
		// (set) Token: 0x06000D38 RID: 3384 RVA: 0x00031B7E File Offset: 0x0002FD7E
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		public double Left
		{
			get
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				return (double)base.GetValue(Window.LeftProperty);
			}
			set
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				base.SetValue(Window.LeftProperty, value);
			}
		}

		/// <summary>Gets the size and location of a window before being either minimized or maximized.</summary>
		/// <returns>A <see cref="T:System.Windows.Rect" /> that specifies the size and location of a window before being either minimized or maximized.</returns>
		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06000D39 RID: 3385 RVA: 0x00031B9D File Offset: 0x0002FD9D
		public Rect RestoreBounds
		{
			[SecurityCritical]
			get
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				if (!this._inTrustedSubWindow)
				{
					SecurityHelper.DemandUIWindowPermission();
				}
				if (this.IsSourceWindowNull || this.IsCompositionTargetInvalid)
				{
					return Rect.Empty;
				}
				return this.GetNormalRectLogicalUnits(this.CriticalHandle);
			}
		}

		/// <summary>Gets or sets the position of the window when first shown.</summary>
		/// <returns>A <see cref="T:System.Windows.WindowStartupLocation" /> value that specifies the top/left position of a window when first shown. The default is <see cref="F:System.Windows.WindowStartupLocation.Manual" />.</returns>
		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06000D3A RID: 3386 RVA: 0x00031BDA File Offset: 0x0002FDDA
		// (set) Token: 0x06000D3B RID: 3387 RVA: 0x00031BEE File Offset: 0x0002FDEE
		[DefaultValue(WindowStartupLocation.Manual)]
		public WindowStartupLocation WindowStartupLocation
		{
			get
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				return this._windowStartupLocation;
			}
			set
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				if (!Window.IsValidWindowStartupLocation(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(WindowStartupLocation));
				}
				this._windowStartupLocation = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the window has a task bar button. </summary>
		/// <returns>
		///     <see langword="true" /> if the window has a task bar button; otherwise, <see langword="false" />. Does not apply when the window is hosted in a browser.</returns>
		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06000D3C RID: 3388 RVA: 0x00031C21 File Offset: 0x0002FE21
		// (set) Token: 0x06000D3D RID: 3389 RVA: 0x00031C3F File Offset: 0x0002FE3F
		public bool ShowInTaskbar
		{
			get
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				return (bool)base.GetValue(Window.ShowInTaskbarProperty);
			}
			set
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				base.SetValue(Window.ShowInTaskbarProperty, BooleanBoxes.Box(value));
			}
		}

		/// <summary>Gets a value that indicates whether the window is active. </summary>
		/// <returns>
		///     <see langword="true" /> if the window is active; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06000D3E RID: 3390 RVA: 0x00031C5E File Offset: 0x0002FE5E
		public bool IsActive
		{
			get
			{
				this.VerifyContextAndObjectState();
				return (bool)base.GetValue(Window.IsActiveProperty);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Window" /> that owns this <see cref="T:System.Windows.Window" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Window" /> object that represents the owner of this <see cref="T:System.Windows.Window" />.</returns>
		/// <exception cref="T:System.ArgumentException">A window tries to own itself-or-Two windows try to own each other.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Window.Owner" /> property is set on a visible window shown using <see cref="M:System.Windows.Window.ShowDialog" />-or-The <see cref="P:System.Windows.Window.Owner" /> property is set with a window that has not been previously shown.</exception>
		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06000D3F RID: 3391 RVA: 0x00031C76 File Offset: 0x0002FE76
		// (set) Token: 0x06000D40 RID: 3392 RVA: 0x00031C90 File Offset: 0x0002FE90
		[DefaultValue(null)]
		public Window Owner
		{
			[SecurityCritical]
			get
			{
				this.VerifyApiSupported();
				SecurityHelper.DemandUIWindowPermission();
				this.VerifyContextAndObjectState();
				return this._ownerWindow;
			}
			[SecurityCritical]
			set
			{
				this.VerifyApiSupported();
				SecurityHelper.DemandUIWindowPermission();
				this.VerifyContextAndObjectState();
				if (value == this)
				{
					throw new ArgumentException(SR.Get("CannotSetOwnerToItself"));
				}
				if (this._showingAsDialog)
				{
					throw new InvalidOperationException(SR.Get("CantSetOwnerAfterDialogIsShown"));
				}
				if (value != null && value.IsSourceWindowNull)
				{
					if (value._disposed)
					{
						throw new InvalidOperationException(SR.Get("CantSetOwnerToClosedWindow"));
					}
					throw new InvalidOperationException(SR.Get("CantSetOwnerWhosHwndIsNotCreated"));
				}
				else
				{
					if (this._ownerWindow == value)
					{
						return;
					}
					if (!this._disposed)
					{
						if (value != null)
						{
							WindowCollection ownedWindows = this.OwnedWindows;
							for (int i = 0; i < ownedWindows.Count; i++)
							{
								if (ownedWindows[i] == value)
								{
									throw new ArgumentException(SR.Get("CircularOwnerChild", new object[]
									{
										value,
										this
									}));
								}
							}
						}
						if (this._ownerWindow != null)
						{
							this._ownerWindow.OwnedWindowsInternal.Remove(this);
						}
					}
					this._ownerWindow = value;
					if (this._disposed)
					{
						return;
					}
					this.SetOwnerHandle((this._ownerWindow != null) ? this._ownerWindow.CriticalHandle : IntPtr.Zero);
					if (this._ownerWindow != null)
					{
						this._ownerWindow.OwnedWindowsInternal.Add(this);
					}
					return;
				}
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06000D41 RID: 3393 RVA: 0x00031DC5 File Offset: 0x0002FFC5
		private bool IsOwnerNull
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				return this._ownerWindow == null;
			}
		}

		/// <summary>Gets a collection of windows for which this window is the owner.</summary>
		/// <returns>A <see cref="T:System.Windows.WindowCollection" /> that contains references to the windows for which this window is the owner.</returns>
		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06000D42 RID: 3394 RVA: 0x00031DD0 File Offset: 0x0002FFD0
		public WindowCollection OwnedWindows
		{
			get
			{
				this.VerifyContextAndObjectState();
				return this.OwnedWindowsInternal.Clone();
			}
		}

		/// <summary>Gets or sets the dialog result value, which is the value that is returned from the <see cref="M:System.Windows.Window.ShowDialog" /> method.</summary>
		/// <returns>A <see cref="T:System.Nullable`1" /> value of type <see cref="T:System.Boolean" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Window.DialogResult" /> is set before a window is opened by calling <see cref="M:System.Windows.Window.ShowDialog" />. -or-
		///         <see cref="P:System.Windows.Window.DialogResult" /> is set on a window that is opened by calling <see cref="M:System.Windows.Window.Show" />.</exception>
		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06000D43 RID: 3395 RVA: 0x00031DE3 File Offset: 0x0002FFE3
		// (set) Token: 0x06000D44 RID: 3396 RVA: 0x00031DF8 File Offset: 0x0002FFF8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[TypeConverter(typeof(DialogResultConverter))]
		public bool? DialogResult
		{
			get
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				return this._dialogResult;
			}
			set
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				if (this._showingAsDialog)
				{
					if (this._dialogResult != value)
					{
						this._dialogResult = value;
						if (!this._isClosing)
						{
							this.Close();
							return;
						}
					}
					return;
				}
				throw new InvalidOperationException(SR.Get("DialogResultMustBeSetAfterShowDialog"));
			}
		}

		/// <summary>Gets or sets a window's border style. </summary>
		/// <returns>A <see cref="T:System.Windows.WindowStyle" /> that specifies a window's border style. The default is <see cref="F:System.Windows.WindowStyle.SingleBorderWindow" />.</returns>
		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06000D45 RID: 3397 RVA: 0x00031E70 File Offset: 0x00030070
		// (set) Token: 0x06000D46 RID: 3398 RVA: 0x00031E8E File Offset: 0x0003008E
		public WindowStyle WindowStyle
		{
			get
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				return (WindowStyle)base.GetValue(Window.WindowStyleProperty);
			}
			set
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				base.SetValue(Window.WindowStyleProperty, value);
			}
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x00031EAD File Offset: 0x000300AD
		private static object CoerceWindowStyle(DependencyObject d, object value)
		{
			value = Window.VerifyAccessCoercion(d, value);
			if (!((Window)d).IsSourceWindowNull)
			{
				((Window)d).VerifyConsistencyWithAllowsTransparency((WindowStyle)value);
			}
			return value;
		}

		/// <summary>Gets or sets a value that indicates whether a window is restored, minimized, or maximized. </summary>
		/// <returns>A <see cref="T:System.Windows.WindowState" /> that determines whether a window is restored, minimized, or maximized. The default is <see cref="F:System.Windows.WindowState.Normal" /> (restored).</returns>
		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06000D48 RID: 3400 RVA: 0x00031ED7 File Offset: 0x000300D7
		// (set) Token: 0x06000D49 RID: 3401 RVA: 0x00031EF5 File Offset: 0x000300F5
		public WindowState WindowState
		{
			get
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				return (WindowState)base.GetValue(Window.WindowStateProperty);
			}
			set
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				base.SetValue(Window.WindowStateProperty, value);
			}
		}

		/// <summary>Gets or sets the resize mode. </summary>
		/// <returns>A <see cref="T:System.Windows.ResizeMode" /> value specifying the resize mode.</returns>
		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06000D4A RID: 3402 RVA: 0x00031F14 File Offset: 0x00030114
		// (set) Token: 0x06000D4B RID: 3403 RVA: 0x00031F32 File Offset: 0x00030132
		public ResizeMode ResizeMode
		{
			get
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				return (ResizeMode)base.GetValue(Window.ResizeModeProperty);
			}
			set
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				base.SetValue(Window.ResizeModeProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a window appears in the topmost z-order. </summary>
		/// <returns>
		///     <see langword="true" /> if the window is topmost; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06000D4C RID: 3404 RVA: 0x00031F51 File Offset: 0x00030151
		// (set) Token: 0x06000D4D RID: 3405 RVA: 0x00031F6F File Offset: 0x0003016F
		public bool Topmost
		{
			get
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				return (bool)base.GetValue(Window.TopmostProperty);
			}
			set
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				base.SetValue(Window.TopmostProperty, BooleanBoxes.Box(value));
			}
		}

		/// <summary>Gets or sets a value that indicates whether a window is activated when first shown. </summary>
		/// <returns>
		///     <see langword="true" /> if a window is activated when first shown; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06000D4E RID: 3406 RVA: 0x00031F8E File Offset: 0x0003018E
		// (set) Token: 0x06000D4F RID: 3407 RVA: 0x00031FAC File Offset: 0x000301AC
		public bool ShowActivated
		{
			get
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				return (bool)base.GetValue(Window.ShowActivatedProperty);
			}
			set
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				base.SetValue(Window.ShowActivatedProperty, BooleanBoxes.Box(value));
			}
		}

		/// <summary>This event is raised to support interoperation with Win32. See <see cref="T:System.Windows.Interop.HwndSource" />.</summary>
		// Token: 0x1400002F RID: 47
		// (add) Token: 0x06000D50 RID: 3408 RVA: 0x00031FCB File Offset: 0x000301CB
		// (remove) Token: 0x06000D51 RID: 3409 RVA: 0x00031FDE File Offset: 0x000301DE
		public event EventHandler SourceInitialized
		{
			add
			{
				this.Events.AddHandler(Window.EVENT_SOURCEINITIALIZED, value);
			}
			remove
			{
				this.Events.RemoveHandler(Window.EVENT_SOURCEINITIALIZED, value);
			}
		}

		/// <summary>Occurs after the DPI of the screen on which the Window is displayed changes.</summary>
		// Token: 0x14000030 RID: 48
		// (add) Token: 0x06000D52 RID: 3410 RVA: 0x00031FF1 File Offset: 0x000301F1
		// (remove) Token: 0x06000D53 RID: 3411 RVA: 0x00031FFF File Offset: 0x000301FF
		public event DpiChangedEventHandler DpiChanged
		{
			add
			{
				base.AddHandler(Window.DpiChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(Window.DpiChangedEvent, value);
			}
		}

		/// <summary>Occurs when a window becomes the foreground window.</summary>
		// Token: 0x14000031 RID: 49
		// (add) Token: 0x06000D54 RID: 3412 RVA: 0x0003200D File Offset: 0x0003020D
		// (remove) Token: 0x06000D55 RID: 3413 RVA: 0x00032020 File Offset: 0x00030220
		public event EventHandler Activated
		{
			add
			{
				this.Events.AddHandler(Window.EVENT_ACTIVATED, value);
			}
			remove
			{
				this.Events.RemoveHandler(Window.EVENT_ACTIVATED, value);
			}
		}

		/// <summary>Occurs when a window becomes a background window.</summary>
		// Token: 0x14000032 RID: 50
		// (add) Token: 0x06000D56 RID: 3414 RVA: 0x00032033 File Offset: 0x00030233
		// (remove) Token: 0x06000D57 RID: 3415 RVA: 0x00032046 File Offset: 0x00030246
		public event EventHandler Deactivated
		{
			add
			{
				this.Events.AddHandler(Window.EVENT_DEACTIVATED, value);
			}
			remove
			{
				this.Events.RemoveHandler(Window.EVENT_DEACTIVATED, value);
			}
		}

		/// <summary>Occurs when the window's <see cref="P:System.Windows.Window.WindowState" /> property changes.</summary>
		// Token: 0x14000033 RID: 51
		// (add) Token: 0x06000D58 RID: 3416 RVA: 0x00032059 File Offset: 0x00030259
		// (remove) Token: 0x06000D59 RID: 3417 RVA: 0x0003206C File Offset: 0x0003026C
		public event EventHandler StateChanged
		{
			add
			{
				this.Events.AddHandler(Window.EVENT_STATECHANGED, value);
			}
			remove
			{
				this.Events.RemoveHandler(Window.EVENT_STATECHANGED, value);
			}
		}

		/// <summary>Occurs when the window's location changes.</summary>
		// Token: 0x14000034 RID: 52
		// (add) Token: 0x06000D5A RID: 3418 RVA: 0x0003207F File Offset: 0x0003027F
		// (remove) Token: 0x06000D5B RID: 3419 RVA: 0x00032092 File Offset: 0x00030292
		public event EventHandler LocationChanged
		{
			add
			{
				this.Events.AddHandler(Window.EVENT_LOCATIONCHANGED, value);
			}
			remove
			{
				this.Events.RemoveHandler(Window.EVENT_LOCATIONCHANGED, value);
			}
		}

		/// <summary>Occurs directly after <see cref="M:System.Windows.Window.Close" /> is called, and can be handled to cancel window closure.</summary>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.UIElement.Visibility" /> is set, or <see cref="M:System.Windows.Window.Show" />, <see cref="M:System.Windows.Window.ShowDialog" />, or <see cref="M:System.Windows.Window.Close" /> is called while a window is closing.</exception>
		// Token: 0x14000035 RID: 53
		// (add) Token: 0x06000D5C RID: 3420 RVA: 0x000320A5 File Offset: 0x000302A5
		// (remove) Token: 0x06000D5D RID: 3421 RVA: 0x000320B8 File Offset: 0x000302B8
		public event CancelEventHandler Closing
		{
			add
			{
				this.Events.AddHandler(Window.EVENT_CLOSING, value);
			}
			remove
			{
				this.Events.RemoveHandler(Window.EVENT_CLOSING, value);
			}
		}

		/// <summary>Occurs when the window is about to close.</summary>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.UIElement.Visibility" /> is set, or <see cref="M:System.Windows.Window.Show" />, <see cref="M:System.Windows.Window.ShowDialog" />, or <see cref="M:System.Windows.Window.Hide" /> is called while a window is closing.</exception>
		// Token: 0x14000036 RID: 54
		// (add) Token: 0x06000D5E RID: 3422 RVA: 0x000320CB File Offset: 0x000302CB
		// (remove) Token: 0x06000D5F RID: 3423 RVA: 0x000320DE File Offset: 0x000302DE
		public event EventHandler Closed
		{
			add
			{
				this.Events.AddHandler(Window.EVENT_CLOSED, value);
			}
			remove
			{
				this.Events.RemoveHandler(Window.EVENT_CLOSED, value);
			}
		}

		/// <summary>Occurs after a window's content has been rendered.</summary>
		// Token: 0x14000037 RID: 55
		// (add) Token: 0x06000D60 RID: 3424 RVA: 0x000320F1 File Offset: 0x000302F1
		// (remove) Token: 0x06000D61 RID: 3425 RVA: 0x00032104 File Offset: 0x00030304
		public event EventHandler ContentRendered
		{
			add
			{
				this.Events.AddHandler(Window.EVENT_CONTENTRENDERED, value);
			}
			remove
			{
				this.Events.RemoveHandler(Window.EVENT_CONTENTRENDERED, value);
			}
		}

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x06000D62 RID: 3426 RVA: 0x00032117 File Offset: 0x00030317
		// (remove) Token: 0x06000D63 RID: 3427 RVA: 0x0003212A File Offset: 0x0003032A
		internal event EventHandler<EventArgs> VisualChildrenChanged
		{
			add
			{
				this.Events.AddHandler(Window.EVENT_VISUALCHILDRENCHANGED, value);
			}
			remove
			{
				this.Events.RemoveHandler(Window.EVENT_VISUALCHILDRENCHANGED, value);
			}
		}

		/// <summary>Creates and returns a <see cref="T:System.Windows.Automation.Peers.WindowAutomationPeer" /> object for this <see cref="T:System.Windows.Window" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Automation.Peers.WindowAutomationPeer" /> object for this <see cref="T:System.Windows.Window" />.</returns>
		// Token: 0x06000D64 RID: 3428 RVA: 0x0003213D File Offset: 0x0003033D
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new WindowAutomationPeer(this);
		}

		/// <summary>Called when the DPI at which this window is rendered changes.</summary>
		/// <param name="oldDpi">The previous DPI scale setting.</param>
		/// <param name="newDpi">The new DPI scale setting.</param>
		// Token: 0x06000D65 RID: 3429 RVA: 0x00032145 File Offset: 0x00030345
		protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
		{
			base.RaiseEvent(new DpiChangedEventArgs(oldDpi, newDpi, Window.DpiChangedEvent, this));
		}

		/// <summary>Called when the parent of the window is changed. </summary>
		/// <param name="oldParent">The previous parent. Set to null if the <see cref="T:System.Windows.DependencyObject" /> did not have a previous parent.</param>
		// Token: 0x06000D66 RID: 3430 RVA: 0x0003215A File Offset: 0x0003035A
		protected internal sealed override void OnVisualParentChanged(DependencyObject oldParent)
		{
			this.VerifyContextAndObjectState();
			base.OnVisualParentChanged(oldParent);
			if (VisualTreeHelper.GetParent(this) != null)
			{
				throw new InvalidOperationException(SR.Get("WindowMustBeRoot"));
			}
		}

		/// <summary>Override this method to measure the size of a window.</summary>
		/// <param name="availableSize">A <see cref="T:System.Windows.Size" /> that reflects the available size that this window can give to the child. Infinity can be given as a value to indicate that the window will size to whatever content is available.</param>
		/// <returns>A <see cref="T:System.Windows.Size" /> that reflects the size that this window determines it needs during layout, based on its calculations of children's sizes.</returns>
		// Token: 0x06000D67 RID: 3431 RVA: 0x00032184 File Offset: 0x00030384
		protected override Size MeasureOverride(Size availableSize)
		{
			this.VerifyContextAndObjectState();
			Size constraint = new Size(availableSize.Width, availableSize.Height);
			Window.WindowMinMax windowMinMax = this.GetWindowMinMax();
			constraint.Width = Math.Max(windowMinMax.minWidth, Math.Min(constraint.Width, windowMinMax.maxWidth));
			constraint.Height = Math.Max(windowMinMax.minHeight, Math.Min(constraint.Height, windowMinMax.maxHeight));
			Size result = this.MeasureOverrideHelper(constraint);
			result = new Size(Math.Max(result.Width, windowMinMax.minWidth), Math.Max(result.Height, windowMinMax.minHeight));
			return result;
		}

		/// <summary>Override this method to arrange and size a window and its child elements. </summary>
		/// <param name="arrangeBounds">A <see cref="T:System.Windows.Size" /> that reflects the final size that the window should use to arrange itself and its children.</param>
		/// <returns>A <see cref="T:System.Windows.Size" /> that reflects the actual size that was used.</returns>
		// Token: 0x06000D68 RID: 3432 RVA: 0x00032230 File Offset: 0x00030430
		protected override Size ArrangeOverride(Size arrangeBounds)
		{
			this.VerifyContextAndObjectState();
			Window.WindowMinMax windowMinMax = this.GetWindowMinMax();
			arrangeBounds.Width = Math.Max(windowMinMax.minWidth, Math.Min(arrangeBounds.Width, windowMinMax.maxWidth));
			arrangeBounds.Height = Math.Max(windowMinMax.minHeight, Math.Min(arrangeBounds.Height, windowMinMax.maxHeight));
			if (this.IsSourceWindowNull || this.IsCompositionTargetInvalid)
			{
				return arrangeBounds;
			}
			if (this.VisualChildrenCount > 0)
			{
				UIElement uielement = this.GetVisualChild(0) as UIElement;
				if (uielement != null)
				{
					Size hwndNonClientAreaSizeInMeasureUnits = this.GetHwndNonClientAreaSizeInMeasureUnits();
					Size size = new Size
					{
						Width = Math.Max(0.0, arrangeBounds.Width - hwndNonClientAreaSizeInMeasureUnits.Width),
						Height = Math.Max(0.0, arrangeBounds.Height - hwndNonClientAreaSizeInMeasureUnits.Height)
					};
					uielement.Arrange(new Rect(size));
					if (base.FlowDirection == FlowDirection.RightToLeft)
					{
						FrameworkElement.InternalSetLayoutTransform(uielement, new MatrixTransform(-1.0, 0.0, 0.0, 1.0, size.Width, 0.0));
					}
				}
			}
			return arrangeBounds;
		}

		/// <summary>Called when the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property changes.</summary>
		/// <param name="oldContent">A reference to the root of the old content tree.</param>
		/// <param name="newContent">A reference to the root of the new content tree.</param>
		// Token: 0x06000D69 RID: 3433 RVA: 0x0003236F File Offset: 0x0003056F
		protected override void OnContentChanged(object oldContent, object newContent)
		{
			base.OnContentChanged(oldContent, newContent);
			this.SetIWindowService();
			if (base.IsLoaded)
			{
				this.PostContentRendered();
				return;
			}
			if (!this._postContentRenderedFromLoadedHandler)
			{
				base.Loaded += this.LoadedHandler;
				this._postContentRenderedFromLoadedHandler = true;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Window.SourceInitialized" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000D6A RID: 3434 RVA: 0x000323B0 File Offset: 0x000305B0
		protected virtual void OnSourceInitialized(EventArgs e)
		{
			this.VerifyContextAndObjectState();
			EventHandler eventHandler = (EventHandler)this.Events[Window.EVENT_SOURCEINITIALIZED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Window.Activated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000D6B RID: 3435 RVA: 0x000323E4 File Offset: 0x000305E4
		protected virtual void OnActivated(EventArgs e)
		{
			this.VerifyContextAndObjectState();
			EventHandler eventHandler = (EventHandler)this.Events[Window.EVENT_ACTIVATED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Window.Deactivated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000D6C RID: 3436 RVA: 0x00032418 File Offset: 0x00030618
		protected virtual void OnDeactivated(EventArgs e)
		{
			this.VerifyContextAndObjectState();
			EventHandler eventHandler = (EventHandler)this.Events[Window.EVENT_DEACTIVATED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Window.StateChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000D6D RID: 3437 RVA: 0x0003244C File Offset: 0x0003064C
		protected virtual void OnStateChanged(EventArgs e)
		{
			this.VerifyContextAndObjectState();
			EventHandler eventHandler = (EventHandler)this.Events[Window.EVENT_STATECHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Window.LocationChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000D6E RID: 3438 RVA: 0x00032480 File Offset: 0x00030680
		protected virtual void OnLocationChanged(EventArgs e)
		{
			this.VerifyContextAndObjectState();
			EventHandler eventHandler = (EventHandler)this.Events[Window.EVENT_LOCATIONCHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Window.Closing" /> event.</summary>
		/// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data.</param>
		// Token: 0x06000D6F RID: 3439 RVA: 0x000324B4 File Offset: 0x000306B4
		protected virtual void OnClosing(CancelEventArgs e)
		{
			this.VerifyContextAndObjectState();
			CancelEventHandler cancelEventHandler = (CancelEventHandler)this.Events[Window.EVENT_CLOSING];
			if (cancelEventHandler != null)
			{
				cancelEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Window.Closed" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000D70 RID: 3440 RVA: 0x000324E8 File Offset: 0x000306E8
		protected virtual void OnClosed(EventArgs e)
		{
			this.VerifyContextAndObjectState();
			EventHandler eventHandler = (EventHandler)this.Events[Window.EVENT_CLOSED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Window.ContentRendered" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000D71 RID: 3441 RVA: 0x0003251C File Offset: 0x0003071C
		protected virtual void OnContentRendered(EventArgs e)
		{
			this.VerifyContextAndObjectState();
			DependencyObject dependencyObject = base.Content as DependencyObject;
			if (dependencyObject != null)
			{
				IInputElement focusedElement = FocusManager.GetFocusedElement(dependencyObject);
				if (focusedElement != null)
				{
					focusedElement.Focus();
				}
			}
			EventHandler eventHandler = (EventHandler)this.Events[Window.EVENT_CONTENTRENDERED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x00032570 File Offset: 0x00030770
		protected internal override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
		{
			this.VerifyContextAndObjectState();
			EventHandler<EventArgs> eventHandler = this.Events[Window.EVENT_VISUALCHILDRENCHANGED] as EventHandler<EventArgs>;
			if (eventHandler != null)
			{
				eventHandler(this, new EventArgs());
			}
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x000325A8 File Offset: 0x000307A8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal Point DeviceToLogicalUnits(Point ptDeviceUnits)
		{
			Invariant.Assert(!this.IsCompositionTargetInvalid, "IsCompositionTargetInvalid is supposed to be false here");
			return this._swh.CompositionTarget.TransformFromDevice.Transform(ptDeviceUnits);
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x000325E4 File Offset: 0x000307E4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal Point LogicalToDeviceUnits(Point ptLogicalUnits)
		{
			Invariant.Assert(!this.IsCompositionTargetInvalid, "IsCompositionTargetInvalid is supposed to be false here");
			return this._swh.CompositionTarget.TransformToDevice.Transform(ptLogicalUnits);
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x0003261F File Offset: 0x0003081F
		internal static bool VisibilityToBool(Visibility v)
		{
			return v == Visibility.Visible || (v - Visibility.Hidden > 1 && false);
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x00032631 File Offset: 0x00030831
		internal virtual void SetResizeGripControl(Control ctrl)
		{
			this._resizeGripControl = ctrl;
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x0003263A File Offset: 0x0003083A
		internal virtual void ClearResizeGripControl(Control oldCtrl)
		{
			if (oldCtrl == this._resizeGripControl)
			{
				this._resizeGripControl = null;
			}
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x0003264C File Offset: 0x0003084C
		internal virtual void TryClearingMainWindow()
		{
			if (this.IsInsideApp && this == this.App.MainWindow)
			{
				this.App.MainWindow = null;
			}
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x00032670 File Offset: 0x00030870
		[SecurityCritical]
		internal void InternalClose(bool shutdown, bool ignoreCancel)
		{
			this.VerifyNotClosing();
			if (this._disposed)
			{
				return;
			}
			this._appShuttingDown = shutdown;
			this._ignoreCancel = ignoreCancel;
			if (!this.IsSourceWindowNull)
			{
				UnsafeNativeMethods.UnsafeSendMessage(this.CriticalHandle, WindowMessage.WM_CLOSE, (IntPtr)0, (IntPtr)0);
				return;
			}
			this._isClosing = true;
			CancelEventArgs cancelEventArgs = new CancelEventArgs(false);
			try
			{
				this.OnClosing(cancelEventArgs);
			}
			catch
			{
				this.CloseWindowBeforeShow();
				throw;
			}
			if (this.ShouldCloseWindow(cancelEventArgs.Cancel))
			{
				this.CloseWindowBeforeShow();
				return;
			}
			this._isClosing = false;
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x00032704 File Offset: 0x00030904
		[SecurityCritical]
		private void CloseWindowBeforeShow()
		{
			this.InternalDispose();
			this.OnClosed(EventArgs.Empty);
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06000D7B RID: 3451 RVA: 0x00032717 File Offset: 0x00030917
		internal bool IsSourceWindowNull
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				return this._swh == null || this._swh.IsSourceWindowNull;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06000D7C RID: 3452 RVA: 0x0003272E File Offset: 0x0003092E
		internal bool IsCompositionTargetInvalid
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				return this._swh == null || this._swh.IsCompositionTargetInvalid;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06000D7D RID: 3453 RVA: 0x00032745 File Offset: 0x00030945
		internal NativeMethods.RECT WorkAreaBoundsForNearestMonitor
		{
			get
			{
				return this._swh.WorkAreaBoundsForNearestMonitor;
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06000D7E RID: 3454 RVA: 0x00032752 File Offset: 0x00030952
		internal Size WindowSize
		{
			get
			{
				return this._swh.WindowSize;
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06000D7F RID: 3455 RVA: 0x0003275F File Offset: 0x0003095F
		internal HwndSource HwndSourceWindow
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				SecurityHelper.DemandUIWindowPermission();
				if (this._swh != null)
				{
					return this._swh.HwndSourceWindow;
				}
				return null;
			}
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x0003277C File Offset: 0x0003097C
		[SecurityCritical]
		private void InternalDispose()
		{
			this._disposed = true;
			this.UpdateWindowListsOnClose();
			if (this._taskbarRetryTimer != null)
			{
				this._taskbarRetryTimer.Stop();
				this._taskbarRetryTimer = null;
			}
			try
			{
				this.ClearSourceWindow();
				Utilities.SafeDispose<HwndWrapper>(ref this._hiddenWindow);
				Utilities.SafeDispose<NativeMethods.IconHandle>(ref this._defaultLargeIconHandle);
				Utilities.SafeDispose<NativeMethods.IconHandle>(ref this._defaultSmallIconHandle);
				Utilities.SafeDispose<NativeMethods.IconHandle>(ref this._currentLargeIconHandle);
				Utilities.SafeDispose<NativeMethods.IconHandle>(ref this._currentSmallIconHandle);
				Utilities.SafeRelease<ITaskbarList3>(ref this._taskbarList);
			}
			finally
			{
				this._isClosing = false;
			}
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x00032814 File Offset: 0x00030A14
		internal override void OnAncestorChanged()
		{
			base.OnAncestorChanged();
			if (base.Parent != null)
			{
				throw new InvalidOperationException(SR.Get("WindowMustBeRoot"));
			}
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x00032834 File Offset: 0x00030A34
		internal virtual void CreateAllStyle()
		{
			this._Style = 34078720;
			this._StyleEx = 0;
			this.CreateWindowStyle();
			this.CreateWindowState();
			if (this._isVisible)
			{
				this._Style |= 268435456;
			}
			this.SetTaskbarStatus();
			this.CreateTopmost();
			this.CreateResizibility();
			this.CreateRtl();
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x00032891 File Offset: 0x00030A91
		[SecurityCritical]
		internal virtual void CreateSourceWindowDuringShow()
		{
			this.CreateSourceWindow(true);
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x0003289C File Offset: 0x00030A9C
		[SecurityCritical]
		internal void CreateSourceWindow(bool duringShow)
		{
			this.VerifyContextAndObjectState();
			this.VerifyCanShow();
			this.VerifyNotClosing();
			this.VerifyConsistencyWithAllowsTransparency();
			if (!duringShow)
			{
				this.VerifyApiSupported();
			}
			double num = 0.0;
			double num2 = 0.0;
			double requestedWidth = 0.0;
			double requestedHeight = 0.0;
			this.GetRequestedDimensions(ref num2, ref num, ref requestedWidth, ref requestedHeight);
			Window.WindowStartupTopLeftPointHelper windowStartupTopLeftPointHelper = new Window.WindowStartupTopLeftPointHelper(new Point(num2, num));
			using (Window.HwndStyleManager hwndStyleManager = Window.HwndStyleManager.StartManaging(this, this.StyleFromHwnd, this.StyleExFromHwnd))
			{
				this.CreateAllStyle();
				HwndSourceParameters parameters = this.CreateHwndSourceParameters();
				if (windowStartupTopLeftPointHelper.ScreenTopLeft != null)
				{
					Point value = windowStartupTopLeftPointHelper.ScreenTopLeft.Value;
					parameters.SetPosition((int)value.X, (int)value.Y);
				}
				HwndSource hwndSource = new HwndSource(parameters);
				this._swh = new Window.SourceWindowHelper(hwndSource);
				hwndSource.SizeToContentChanged += this.OnSourceSizeToContentChanged;
				hwndStyleManager.Dirty = false;
				this.CorrectStyleForBorderlessWindowCase();
			}
			this._swh.AddDisposedHandler(new EventHandler(this.OnSourceWindowDisposed));
			this._hwndCreatedButNotShown = !duringShow;
			if (Utilities.IsOSWindows7OrNewer)
			{
				MSGFLTINFO msgfltinfo;
				UnsafeNativeMethods.ChangeWindowMessageFilterEx(this._swh.CriticalHandle, Window.WM_TASKBARBUTTONCREATED, MSGFLT.ALLOW, out msgfltinfo);
				UnsafeNativeMethods.ChangeWindowMessageFilterEx(this._swh.CriticalHandle, WindowMessage.WM_COMMAND, MSGFLT.ALLOW, out msgfltinfo);
			}
			this.SetupInitialState(num, num2, requestedWidth, requestedHeight);
			this.OnSourceInitialized(EventArgs.Empty);
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x00032A30 File Offset: 0x00030C30
		[SecurityCritical]
		internal virtual HwndSourceParameters CreateHwndSourceParameters()
		{
			return new HwndSourceParameters(this.Title, int.MinValue, int.MinValue)
			{
				UsesPerPixelOpacity = this.AllowsTransparency,
				WindowStyle = this._Style,
				ExtendedWindowStyle = this._StyleEx,
				ParentWindow = this._ownerHandle,
				AdjustSizingForNonClientArea = true,
				HwndSourceHook = new HwndSourceHook(this.WindowFilterMessage)
			};
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x00032AA4 File Offset: 0x00030CA4
		private void OnSourceSizeToContentChanged(object sender, EventArgs args)
		{
			this.SizeToContent = this.HwndSourceSizeToContent;
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x00032AB4 File Offset: 0x00030CB4
		internal virtual void CorrectStyleForBorderlessWindowCase()
		{
			using (Window.HwndStyleManager.StartManaging(this, this.StyleFromHwnd, this.StyleExFromHwnd))
			{
				if (this.WindowStyle == WindowStyle.None)
				{
					this._Style = this._swh.StyleFromHwnd;
					this._Style &= -12582913;
				}
			}
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x00032B1C File Offset: 0x00030D1C
		internal virtual void GetRequestedDimensions(ref double requestedLeft, ref double requestedTop, ref double requestedWidth, ref double requestedHeight)
		{
			requestedTop = this.Top;
			requestedLeft = this.Left;
			requestedWidth = base.Width;
			requestedHeight = base.Height;
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x00032B40 File Offset: 0x00030D40
		[SecurityCritical]
		internal virtual void SetupInitialState(double requestedTop, double requestedLeft, double requestedWidth, double requestedHeight)
		{
			this.HwndSourceSizeToContent = (SizeToContent)base.GetValue(Window.SizeToContentProperty);
			this.UpdateIcon();
			NativeMethods.RECT windowBounds = this.WindowBounds;
			Size currentSizeDeviceUnits = new Size((double)(windowBounds.right - windowBounds.left), (double)(windowBounds.bottom - windowBounds.top));
			double num = (double)windowBounds.left;
			double num2 = (double)windowBounds.top;
			bool flag = false;
			Point point = this.DeviceToLogicalUnits(new Point(num, num2));
			this._actualLeft = point.X;
			this._actualTop = point.Y;
			try
			{
				this._updateHwndLocation = false;
				base.CoerceValue(Window.TopProperty);
				base.CoerceValue(Window.LeftProperty);
			}
			finally
			{
				this._updateHwndLocation = true;
			}
			Point point2 = this.LogicalToDeviceUnits(new Point(requestedWidth, requestedHeight));
			Point point3 = this.LogicalToDeviceUnits(new Point(requestedLeft, requestedTop));
			if (!DoubleUtil.IsNaN(requestedWidth) && !DoubleUtil.AreClose(currentSizeDeviceUnits.Width, point2.X))
			{
				flag = true;
				currentSizeDeviceUnits.Width = point2.X;
				if (this.WindowState != WindowState.Normal)
				{
					this.UpdateHwndRestoreBounds(requestedWidth, Window.BoundsSpecified.Width);
				}
			}
			if (!DoubleUtil.IsNaN(requestedHeight) && !DoubleUtil.AreClose(currentSizeDeviceUnits.Height, point2.Y))
			{
				flag = true;
				currentSizeDeviceUnits.Height = point2.Y;
				if (this.WindowState != WindowState.Normal)
				{
					this.UpdateHwndRestoreBounds(requestedHeight, Window.BoundsSpecified.Height);
				}
			}
			if (!DoubleUtil.IsNaN(requestedLeft) && !DoubleUtil.AreClose(num, point3.X))
			{
				flag = true;
				num = point3.X;
				if (this.WindowState != WindowState.Normal)
				{
					this.UpdateHwndRestoreBounds(requestedLeft, Window.BoundsSpecified.Left);
				}
			}
			if (!DoubleUtil.IsNaN(requestedTop) && !DoubleUtil.AreClose(num2, point3.Y))
			{
				flag = true;
				num2 = point3.Y;
				if (this.WindowState != WindowState.Normal)
				{
					this.UpdateHwndRestoreBounds(requestedTop, Window.BoundsSpecified.Top);
				}
			}
			Point point4 = this.LogicalToDeviceUnits(new Point(base.MinWidth, base.MinHeight));
			Point point5 = this.LogicalToDeviceUnits(new Point(base.MaxWidth, base.MaxHeight));
			if (!double.IsPositiveInfinity(point5.X) && currentSizeDeviceUnits.Width > point5.X)
			{
				flag = true;
				currentSizeDeviceUnits.Width = point5.X;
			}
			if (!double.IsPositiveInfinity(point4.Y) && currentSizeDeviceUnits.Height > point5.Y)
			{
				flag = true;
				currentSizeDeviceUnits.Height = point5.Y;
			}
			if (currentSizeDeviceUnits.Width < point4.X)
			{
				flag = true;
				currentSizeDeviceUnits.Width = point4.X;
			}
			if (currentSizeDeviceUnits.Height < point4.Y)
			{
				flag = true;
				currentSizeDeviceUnits.Height = point4.Y;
			}
			flag = (this.CalculateWindowLocation(ref num, ref num2, currentSizeDeviceUnits) || flag);
			if (flag && this.WindowState == WindowState.Normal)
			{
				UnsafeNativeMethods.SetWindowPos(new HandleRef(this, this.CriticalHandle), new HandleRef(null, IntPtr.Zero), DoubleUtil.DoubleToInt(num), DoubleUtil.DoubleToInt(num2), DoubleUtil.DoubleToInt(currentSizeDeviceUnits.Width), DoubleUtil.DoubleToInt(currentSizeDeviceUnits.Height), 20);
				try
				{
					this._updateHwndLocation = false;
					this._updateStartupLocation = true;
					base.CoerceValue(Window.TopProperty);
					base.CoerceValue(Window.LeftProperty);
				}
				finally
				{
					this._updateHwndLocation = true;
					this._updateStartupLocation = false;
				}
			}
			if (!this.HwndCreatedButNotShown)
			{
				this.SetRootVisualAndUpdateSTC();
			}
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x00032E90 File Offset: 0x00031090
		[SecurityCritical]
		internal void SetRootVisual()
		{
			this.SetIWindowService();
			if (!this.IsSourceWindowNull)
			{
				this._swh.RootVisual = this;
			}
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x00032EAC File Offset: 0x000310AC
		[SecurityCritical]
		internal void SetRootVisualAndUpdateSTC()
		{
			this.SetRootVisual();
			if (!this.IsSourceWindowNull && (this.SizeToContent != SizeToContent.Manual || this.HwndCreatedButNotShown))
			{
				NativeMethods.RECT windowBounds = this.WindowBounds;
				double val = (double)windowBounds.left;
				double val2 = (double)windowBounds.top;
				Point point = this.LogicalToDeviceUnits(new Point(base.ActualWidth, base.ActualHeight));
				if (this.CalculateWindowLocation(ref val, ref val2, new Size(point.X, point.Y)) && this.WindowState == WindowState.Normal)
				{
					UnsafeNativeMethods.SetWindowPos(new HandleRef(this, this.CriticalHandle), new HandleRef(null, IntPtr.Zero), DoubleUtil.DoubleToInt(val), DoubleUtil.DoubleToInt(val2), 0, 0, 21);
					try
					{
						this._updateHwndLocation = false;
						this._updateStartupLocation = true;
						base.CoerceValue(Window.TopProperty);
						base.CoerceValue(Window.LeftProperty);
					}
					finally
					{
						this._updateHwndLocation = true;
						this._updateStartupLocation = false;
					}
				}
			}
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x00032FA8 File Offset: 0x000311A8
		private void CreateWindowStyle()
		{
			this._Style &= -12582913;
			this._StyleEx &= -641;
			switch (this.WindowStyle)
			{
			case WindowStyle.None:
				this._Style &= -12582913;
				return;
			case WindowStyle.SingleBorderWindow:
				this._Style |= 12582912;
				return;
			case WindowStyle.ThreeDBorderWindow:
				this._Style |= 12582912;
				this._StyleEx |= 512;
				return;
			case WindowStyle.ToolWindow:
				this._Style |= 12582912;
				this._StyleEx |= 128;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x00033066 File Offset: 0x00031266
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal virtual void UpdateTitle(string title)
		{
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				UnsafeNativeMethods.SetWindowText(new HandleRef(this, this.CriticalHandle), title);
			}
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x0003308C File Offset: 0x0003128C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void UpdateHwndSizeOnWidthHeightChange(double widthLogicalUnits, double heightLogicalUnits)
		{
			if (!this._inTrustedSubWindow)
			{
				SecurityHelper.DemandUIWindowPermission();
			}
			Point point = this.LogicalToDeviceUnits(new Point(widthLogicalUnits, heightLogicalUnits));
			UnsafeNativeMethods.SetWindowPos(new HandleRef(this, this.CriticalHandle), new HandleRef(null, IntPtr.Zero), 0, 0, DoubleUtil.DoubleToInt(point.X), DoubleUtil.DoubleToInt(point.Y), 22);
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x000330F0 File Offset: 0x000312F0
		internal void HandleActivate(bool windowActivated)
		{
			if (windowActivated && !this.IsActive)
			{
				base.SetValue(Window.IsActivePropertyKey, BooleanBoxes.TrueBox);
				this.OnActivated(EventArgs.Empty);
				return;
			}
			if (!windowActivated && this.IsActive)
			{
				base.SetValue(Window.IsActivePropertyKey, BooleanBoxes.FalseBox);
				this.OnDeactivated(EventArgs.Empty);
			}
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x0003314C File Offset: 0x0003134C
		internal virtual void UpdateHeight(double newHeight)
		{
			if (this.WindowState == WindowState.Normal)
			{
				this.UpdateHwndSizeOnWidthHeightChange(this.DeviceToLogicalUnits(new Point((double)this.WindowBounds.Width, 0.0)).X, newHeight);
				return;
			}
			this.UpdateHwndRestoreBounds(newHeight, Window.BoundsSpecified.Height);
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x0003319C File Offset: 0x0003139C
		internal virtual void UpdateWidth(double newWidth)
		{
			if (this.WindowState == WindowState.Normal)
			{
				this.UpdateHwndSizeOnWidthHeightChange(newWidth, this.DeviceToLogicalUnits(new Point(0.0, (double)this.WindowBounds.Height)).Y);
				return;
			}
			this.UpdateHwndRestoreBounds(newWidth, Window.BoundsSpecified.Width);
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal virtual void VerifyApiSupported()
		{
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06000D93 RID: 3475 RVA: 0x000331EC File Offset: 0x000313EC
		internal bool HwndCreatedButNotShown
		{
			get
			{
				return this._hwndCreatedButNotShown;
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06000D94 RID: 3476 RVA: 0x000331F4 File Offset: 0x000313F4
		internal bool IsDisposed
		{
			get
			{
				return this._disposed;
			}
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x06000D95 RID: 3477 RVA: 0x000331FC File Offset: 0x000313FC
		internal bool IsVisibilitySet
		{
			get
			{
				this.VerifyContextAndObjectState();
				return this._isVisibilitySet;
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x06000D96 RID: 3478 RVA: 0x0003320A File Offset: 0x0003140A
		internal IntPtr CriticalHandle
		{
			[SecurityCritical]
			get
			{
				this.VerifyContextAndObjectState();
				if (this._swh != null)
				{
					return this._swh.CriticalHandle;
				}
				return IntPtr.Zero;
			}
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06000D97 RID: 3479 RVA: 0x0003322B File Offset: 0x0003142B
		// (set) Token: 0x06000D98 RID: 3480 RVA: 0x0003323E File Offset: 0x0003143E
		internal IntPtr OwnerHandle
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				SecurityHelper.DemandUIWindowPermission();
				this.VerifyContextAndObjectState();
				return this._ownerHandle;
			}
			[SecurityCritical]
			[SecurityTreatAsSafe]
			set
			{
				SecurityHelper.DemandUIWindowPermission();
				this.VerifyContextAndObjectState();
				if (this._showingAsDialog)
				{
					throw new InvalidOperationException(SR.Get("CantSetOwnerAfterDialogIsShown"));
				}
				this.SetOwnerHandle(value);
			}
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06000D99 RID: 3481 RVA: 0x0003326A File Offset: 0x0003146A
		// (set) Token: 0x06000D9A RID: 3482 RVA: 0x00033278 File Offset: 0x00031478
		internal int Win32Style
		{
			get
			{
				this.VerifyContextAndObjectState();
				return this._Style;
			}
			set
			{
				this.VerifyContextAndObjectState();
				this._Style = value;
			}
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06000D9B RID: 3483 RVA: 0x00033287 File Offset: 0x00031487
		// (set) Token: 0x06000D9C RID: 3484 RVA: 0x000332BC File Offset: 0x000314BC
		internal int _Style
		{
			get
			{
				if (this.Manager != null)
				{
					return this._styleDoNotUse.Value;
				}
				if (this.IsSourceWindowNull)
				{
					return this._styleDoNotUse.Value;
				}
				return this._swh.StyleFromHwnd;
			}
			[SecurityCritical]
			[SecurityTreatAsSafe]
			set
			{
				SecurityHelper.DemandUIWindowPermission();
				this._styleDoNotUse = new SecurityCriticalDataForSet<int>(value);
				this.Manager.Dirty = true;
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06000D9D RID: 3485 RVA: 0x000332DB File Offset: 0x000314DB
		// (set) Token: 0x06000D9E RID: 3486 RVA: 0x00033310 File Offset: 0x00031510
		internal int _StyleEx
		{
			get
			{
				if (this.Manager != null)
				{
					return this._styleExDoNotUse.Value;
				}
				if (this.IsSourceWindowNull)
				{
					return this._styleExDoNotUse.Value;
				}
				return this._swh.StyleExFromHwnd;
			}
			[SecurityCritical]
			[SecurityTreatAsSafe]
			set
			{
				SecurityHelper.DemandUIWindowPermission();
				this._styleExDoNotUse = new SecurityCriticalDataForSet<int>(value);
				this.Manager.Dirty = true;
			}
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06000D9F RID: 3487 RVA: 0x0003332F File Offset: 0x0003152F
		// (set) Token: 0x06000DA0 RID: 3488 RVA: 0x00033337 File Offset: 0x00031537
		internal Window.HwndStyleManager Manager
		{
			get
			{
				return this._manager;
			}
			set
			{
				this._manager = value;
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06000DA1 RID: 3489 RVA: 0x0000B02A File Offset: 0x0000922A
		bool IWindowService.UserResized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x00033340 File Offset: 0x00031540
		private Size MeasureOverrideHelper(Size constraint)
		{
			if (this.IsSourceWindowNull || this.IsCompositionTargetInvalid)
			{
				return new Size(0.0, 0.0);
			}
			if (this.VisualChildrenCount > 0)
			{
				UIElement uielement = this.GetVisualChild(0) as UIElement;
				if (uielement != null)
				{
					Size hwndNonClientAreaSizeInMeasureUnits = this.GetHwndNonClientAreaSizeInMeasureUnits();
					uielement.Measure(new Size
					{
						Width = ((constraint.Width == double.PositiveInfinity) ? double.PositiveInfinity : Math.Max(0.0, constraint.Width - hwndNonClientAreaSizeInMeasureUnits.Width)),
						Height = ((constraint.Height == double.PositiveInfinity) ? double.PositiveInfinity : Math.Max(0.0, constraint.Height - hwndNonClientAreaSizeInMeasureUnits.Height))
					});
					Size desiredSize = uielement.DesiredSize;
					return new Size(desiredSize.Width + hwndNonClientAreaSizeInMeasureUnits.Width, desiredSize.Height + hwndNonClientAreaSizeInMeasureUnits.Height);
				}
			}
			return this._swh.GetSizeFromHwndInMeasureUnits();
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x00033464 File Offset: 0x00031664
		internal virtual Window.WindowMinMax GetWindowMinMax()
		{
			Window.WindowMinMax result = default(Window.WindowMinMax);
			Invariant.Assert(!this.IsCompositionTargetInvalid, "IsCompositionTargetInvalid is supposed to be false here");
			double x = this._trackMaxWidthDeviceUnits;
			double y = this._trackMaxHeightDeviceUnits;
			if (this.WindowState == WindowState.Maximized)
			{
				x = Math.Max(this._trackMaxWidthDeviceUnits, this._windowMaxWidthDeviceUnits);
				y = Math.Max(this._trackMaxHeightDeviceUnits, this._windowMaxHeightDeviceUnits);
			}
			Point point = this.DeviceToLogicalUnits(new Point(x, y));
			Point point2 = this.DeviceToLogicalUnits(new Point(this._trackMinWidthDeviceUnits, this._trackMinHeightDeviceUnits));
			result.minWidth = Math.Max(base.MinWidth, point2.X);
			if (base.MinWidth > base.MaxWidth)
			{
				result.maxWidth = Math.Min(base.MinWidth, point.X);
			}
			else if (!double.IsPositiveInfinity(base.MaxWidth))
			{
				result.maxWidth = Math.Min(base.MaxWidth, point.X);
			}
			else
			{
				result.maxWidth = point.X;
			}
			result.minHeight = Math.Max(base.MinHeight, point2.Y);
			if (base.MinHeight > base.MaxHeight)
			{
				result.maxHeight = Math.Min(base.MinHeight, point.Y);
			}
			else if (!double.IsPositiveInfinity(base.MaxHeight))
			{
				result.maxHeight = Math.Min(base.MaxHeight, point.Y);
			}
			else
			{
				result.maxHeight = point.Y;
			}
			return result;
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x000335DF File Offset: 0x000317DF
		private void LoadedHandler(object sender, RoutedEventArgs e)
		{
			if (this._postContentRenderedFromLoadedHandler)
			{
				this.PostContentRendered();
				this._postContentRenderedFromLoadedHandler = false;
				base.Loaded -= this.LoadedHandler;
			}
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x00033608 File Offset: 0x00031808
		private void PostContentRendered()
		{
			if (this._contentRenderedCallback != null)
			{
				this._contentRenderedCallback.Abort();
			}
			this._contentRenderedCallback = base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object unused)
			{
				this._contentRenderedCallback = null;
				this.OnContentRendered(EventArgs.Empty);
				return null;
			}), this);
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x00033640 File Offset: 0x00031840
		private void EnsureDialogCommand()
		{
			if (!Window._dialogCommandAdded)
			{
				CommandBinding commandBinding = new CommandBinding(Window.DialogCancelCommand);
				commandBinding.Executed += Window.OnDialogCommand;
				CommandManager.RegisterClassCommandBinding(typeof(Window), commandBinding);
				Window._dialogCommandAdded = true;
			}
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x00033688 File Offset: 0x00031888
		private static void OnDialogCommand(object target, ExecutedRoutedEventArgs e)
		{
			Window window = target as Window;
			window.OnDialogCancelCommand();
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x000336A2 File Offset: 0x000318A2
		private void OnDialogCancelCommand()
		{
			if (this._showingAsDialog)
			{
				this.DialogResult = new bool?(false);
			}
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x000336B8 File Offset: 0x000318B8
		private bool ThreadWindowsCallback(IntPtr hWnd, IntPtr lparam)
		{
			if (SafeNativeMethods.IsWindowVisible(new HandleRef(null, hWnd)) && SafeNativeMethods.IsWindowEnabled(new HandleRef(null, hWnd)))
			{
				this._threadWindowHandles.Add(hWnd);
			}
			return true;
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x000336EC File Offset: 0x000318EC
		[SecurityCritical]
		private void EnableThreadWindows(bool state)
		{
			for (int i = 0; i < this._threadWindowHandles.Count; i++)
			{
				IntPtr handle = (IntPtr)this._threadWindowHandles[i];
				if (UnsafeNativeMethods.IsWindow(new HandleRef(null, handle)))
				{
					UnsafeNativeMethods.EnableWindowNoThrow(new HandleRef(null, handle), state);
				}
			}
			if (state)
			{
				this._threadWindowHandles = null;
			}
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x00033748 File Offset: 0x00031948
		private void Initialize()
		{
			base.BypassLayoutPolicies = true;
			if (this.IsInsideApp)
			{
				if (Application.Current.Dispatcher.Thread == Dispatcher.CurrentDispatcher.Thread)
				{
					this.App.WindowsInternal.Add(this);
					if (this.App.MainWindow == null)
					{
						this.App.MainWindow = this;
						return;
					}
				}
				else
				{
					this.App.NonAppWindowsInternal.Add(this);
				}
			}
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x000337BD File Offset: 0x000319BD
		internal void VerifyContextAndObjectState()
		{
			base.VerifyAccess();
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x000337C5 File Offset: 0x000319C5
		private void VerifyCanShow()
		{
			if (this._disposed)
			{
				throw new InvalidOperationException(SR.Get("ReshowNotAllowed"));
			}
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x000337DF File Offset: 0x000319DF
		private void VerifyNotClosing()
		{
			if (this._isClosing)
			{
				throw new InvalidOperationException(SR.Get("InvalidOperationDuringClosing"));
			}
			if (!this.IsSourceWindowNull && this.IsCompositionTargetInvalid)
			{
				throw new InvalidOperationException(SR.Get("InvalidCompositionTarget"));
			}
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x00033819 File Offset: 0x00031A19
		private void VerifyHwndCreateShowState()
		{
			if (this.HwndCreatedButNotShown)
			{
				throw new InvalidOperationException(SR.Get("NotAllowedBeforeShow"));
			}
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x00033833 File Offset: 0x00031A33
		private void SetIWindowService()
		{
			if (base.GetValue(Window.IWindowServiceProperty) == null)
			{
				base.SetValue(Window.IWindowServiceProperty, this);
			}
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x00033850 File Offset: 0x00031A50
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private IntPtr GetCurrentMonitorFromMousePosition()
		{
			SecurityHelper.DemandUnmanagedCode();
			NativeMethods.POINT point = new NativeMethods.POINT();
			UnsafeNativeMethods.TryGetCursorPos(point);
			NativeMethods.POINTSTRUCT pt = new NativeMethods.POINTSTRUCT(point.x, point.y);
			return SafeNativeMethods.MonitorFromPoint(pt, 2);
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x0003388C File Offset: 0x00031A8C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private bool CalculateWindowLocation(ref double leftDeviceUnits, ref double topDeviceUnits, Size currentSizeDeviceUnits)
		{
			double value = leftDeviceUnits;
			double value2 = topDeviceUnits;
			switch (this._windowStartupLocation)
			{
			case WindowStartupLocation.Manual:
				goto IL_259;
			case WindowStartupLocation.CenterScreen:
				break;
			case WindowStartupLocation.CenterOwner:
			{
				Rect rect = Rect.Empty;
				if (this.CanCenterOverWPFOwner)
				{
					if (this.Owner.WindowState == WindowState.Maximized || this.Owner.WindowState == WindowState.Minimized)
					{
						break;
					}
					Point point;
					if (this.Owner.CriticalHandle == IntPtr.Zero)
					{
						point = this.Owner.LogicalToDeviceUnits(new Point(this.Owner.Width, this.Owner.Height));
					}
					else
					{
						Size windowSize = this.Owner.WindowSize;
						point = new Point(windowSize.Width, windowSize.Height);
					}
					Point point2 = this.Owner.LogicalToDeviceUnits(new Point(this.Owner.Left, this.Owner.Top));
					rect = new Rect(point2.X, point2.Y, point.X, point.Y);
				}
				else if (this._ownerHandle != IntPtr.Zero && UnsafeNativeMethods.IsWindow(new HandleRef(null, this._ownerHandle)))
				{
					rect = this.GetNormalRectDeviceUnits(this._ownerHandle);
				}
				if (!rect.IsEmpty)
				{
					leftDeviceUnits = rect.X + (rect.Width - currentSizeDeviceUnits.Width) / 2.0;
					topDeviceUnits = rect.Y + (rect.Height - currentSizeDeviceUnits.Height) / 2.0;
					NativeMethods.RECT rect2 = Window.WorkAreaBoundsForHwnd(this._ownerHandle);
					leftDeviceUnits = Math.Min(leftDeviceUnits, (double)rect2.right - currentSizeDeviceUnits.Width);
					leftDeviceUnits = Math.Max(leftDeviceUnits, (double)rect2.left);
					topDeviceUnits = Math.Min(topDeviceUnits, (double)rect2.bottom - currentSizeDeviceUnits.Height);
					topDeviceUnits = Math.Max(topDeviceUnits, (double)rect2.top);
					goto IL_259;
				}
				goto IL_259;
			}
			default:
				goto IL_259;
			}
			IntPtr intPtr = IntPtr.Zero;
			if (this._ownerHandle == IntPtr.Zero || (this._hiddenWindow != null && this._hiddenWindow.Handle == this._ownerHandle))
			{
				intPtr = this.GetCurrentMonitorFromMousePosition();
			}
			else
			{
				intPtr = Window.MonitorFromWindow(this._ownerHandle);
			}
			if (intPtr != IntPtr.Zero)
			{
				Window.CalculateCenterScreenPosition(intPtr, currentSizeDeviceUnits, ref leftDeviceUnits, ref topDeviceUnits);
			}
			IL_259:
			return !DoubleUtil.AreClose(value, leftDeviceUnits) || !DoubleUtil.AreClose(value2, topDeviceUnits);
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x00033B0C File Offset: 0x00031D0C
		[SecurityCritical]
		private static NativeMethods.RECT WorkAreaBoundsForHwnd(IntPtr hwnd)
		{
			IntPtr hMonitor = Window.MonitorFromWindow(hwnd);
			return Window.WorkAreaBoundsForMointor(hMonitor);
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x00033B28 File Offset: 0x00031D28
		[SecurityCritical]
		private static NativeMethods.RECT WorkAreaBoundsForMointor(IntPtr hMonitor)
		{
			NativeMethods.MONITORINFOEX monitorinfoex = new NativeMethods.MONITORINFOEX();
			SafeNativeMethods.GetMonitorInfo(new HandleRef(null, hMonitor), monitorinfoex);
			return monitorinfoex.rcWork;
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x00033B50 File Offset: 0x00031D50
		[SecurityCritical]
		private static IntPtr MonitorFromWindow(IntPtr hwnd)
		{
			IntPtr intPtr = SafeNativeMethods.MonitorFromWindow(new HandleRef(null, hwnd), 2);
			if (intPtr == IntPtr.Zero)
			{
				throw new Win32Exception();
			}
			return intPtr;
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x00033B80 File Offset: 0x00031D80
		[SecurityCritical]
		internal static void CalculateCenterScreenPosition(IntPtr hMonitor, Size currentSizeDeviceUnits, ref double leftDeviceUnits, ref double topDeviceUnits)
		{
			NativeMethods.RECT rect = Window.WorkAreaBoundsForMointor(hMonitor);
			double num = (double)(rect.right - rect.left);
			double num2 = (double)(rect.bottom - rect.top);
			leftDeviceUnits = (double)rect.left + (num - currentSizeDeviceUnits.Width) / 2.0;
			topDeviceUnits = (double)rect.top + (num2 - currentSizeDeviceUnits.Height) / 2.0;
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06000DB7 RID: 3511 RVA: 0x00033BEC File Offset: 0x00031DEC
		private bool CanCenterOverWPFOwner
		{
			get
			{
				return this.Owner != null && (!this.Owner.IsSourceWindowNull || (!DoubleUtil.IsNaN(this.Owner.Width) && !DoubleUtil.IsNaN(this.Owner.Height))) && !DoubleUtil.IsNaN(this.Owner.Left) && !DoubleUtil.IsNaN(this.Owner.Top);
			}
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x00033C60 File Offset: 0x00031E60
		[SecurityCritical]
		private Rect GetNormalRectDeviceUnits(IntPtr hwndHandle)
		{
			int windowLong = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, hwndHandle), -20);
			NativeMethods.WINDOWPLACEMENT windowplacement = default(NativeMethods.WINDOWPLACEMENT);
			windowplacement.length = Marshal.SizeOf(typeof(NativeMethods.WINDOWPLACEMENT));
			UnsafeNativeMethods.GetWindowPlacement(new HandleRef(this, hwndHandle), ref windowplacement);
			Point pt = new Point((double)windowplacement.rcNormalPosition_left, (double)windowplacement.rcNormalPosition_top);
			if ((windowLong & 128) == 0)
			{
				pt = this.TransformWorkAreaScreenArea(pt, Window.TransformType.WorkAreaToScreenArea);
			}
			Point point = new Point((double)(windowplacement.rcNormalPosition_right - windowplacement.rcNormalPosition_left), (double)(windowplacement.rcNormalPosition_bottom - windowplacement.rcNormalPosition_top));
			return new Rect(pt.X, pt.Y, point.X, point.Y);
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x00033D14 File Offset: 0x00031F14
		[SecurityCritical]
		private Rect GetNormalRectLogicalUnits(IntPtr hwndHandle)
		{
			Rect normalRectDeviceUnits = this.GetNormalRectDeviceUnits(hwndHandle);
			Point point = this.DeviceToLogicalUnits(new Point(normalRectDeviceUnits.Width, normalRectDeviceUnits.Height));
			Point point2 = this.DeviceToLogicalUnits(new Point(normalRectDeviceUnits.X, normalRectDeviceUnits.Y));
			return new Rect(point2.X, point2.Y, point.X, point.Y);
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x00033D80 File Offset: 0x00031F80
		private void CreateWindowState()
		{
			switch (this.WindowState)
			{
			case WindowState.Normal:
				break;
			case WindowState.Minimized:
				this._Style |= 536870912;
				break;
			case WindowState.Maximized:
				this._Style |= 16777216;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x00033DCC File Offset: 0x00031FCC
		private void CreateTopmost()
		{
			if (this.Topmost)
			{
				this._StyleEx |= 8;
				return;
			}
			this._StyleEx &= -9;
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x00033DF4 File Offset: 0x00031FF4
		private void CreateResizibility()
		{
			this._Style &= -458753;
			switch (this.ResizeMode)
			{
			case ResizeMode.NoResize:
				break;
			case ResizeMode.CanMinimize:
				this._Style |= 131072;
				return;
			case ResizeMode.CanResize:
			case ResizeMode.CanResizeWithGrip:
				this._Style |= 458752;
				break;
			default:
				return;
			}
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x00033E58 File Offset: 0x00032058
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void UpdateIcon()
		{
			NativeMethods.IconHandle defaultLargeIconHandle;
			NativeMethods.IconHandle defaultSmallIconHandle;
			if (this._icon != null)
			{
				IconHelper.GetIconHandlesFromImageSource(this._icon, out defaultLargeIconHandle, out defaultSmallIconHandle);
			}
			else if (this._defaultLargeIconHandle == null && this._defaultSmallIconHandle == null)
			{
				IconHelper.GetDefaultIconHandles(out defaultLargeIconHandle, out defaultSmallIconHandle);
				this._defaultLargeIconHandle = defaultLargeIconHandle;
				this._defaultSmallIconHandle = defaultSmallIconHandle;
			}
			else
			{
				defaultLargeIconHandle = this._defaultLargeIconHandle;
				defaultSmallIconHandle = this._defaultSmallIconHandle;
			}
			HandleRef[] array = new HandleRef[2];
			array[0] = new HandleRef(this, this.CriticalHandle);
			HandleRef[] array2 = array;
			int num = 1;
			if (this._hiddenWindow != null)
			{
				array2[1] = new HandleRef(this._hiddenWindow, this._hiddenWindow.Handle);
				num++;
			}
			for (int i = 0; i < num; i++)
			{
				HandleRef hWnd = array2[i];
				UnsafeNativeMethods.SendMessage(hWnd, WindowMessage.WM_SETICON, (IntPtr)1, defaultLargeIconHandle);
				UnsafeNativeMethods.SendMessage(hWnd, WindowMessage.WM_SETICON, (IntPtr)0, defaultSmallIconHandle);
			}
			if (this._currentLargeIconHandle != null && this._currentLargeIconHandle != this._defaultLargeIconHandle)
			{
				this._currentLargeIconHandle.Dispose();
			}
			if (this._currentSmallIconHandle != null && this._currentSmallIconHandle != this._defaultSmallIconHandle)
			{
				this._currentSmallIconHandle.Dispose();
			}
			this._currentLargeIconHandle = defaultLargeIconHandle;
			this._currentSmallIconHandle = defaultSmallIconHandle;
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x00033F8C File Offset: 0x0003218C
		[SecurityCritical]
		private void SetOwnerHandle(IntPtr ownerHandle)
		{
			if (this._ownerHandle == ownerHandle && this._ownerHandle == IntPtr.Zero)
			{
				return;
			}
			this._ownerHandle = ((IntPtr.Zero == ownerHandle && !this.ShowInTaskbar) ? this.EnsureHiddenWindow().Handle : ownerHandle);
			if (!this.IsSourceWindowNull)
			{
				UnsafeNativeMethods.SetWindowLong(new HandleRef(null, this.CriticalHandle), -8, this._ownerHandle);
				if (this._ownerWindow != null && this._ownerWindow.CriticalHandle != this._ownerHandle)
				{
					this._ownerWindow.OwnedWindowsInternal.Remove(this);
					this._ownerWindow = null;
				}
			}
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x0003403D File Offset: 0x0003223D
		[SecurityCritical]
		private void OnSourceWindowDisposed(object sender, EventArgs e)
		{
			if (!this._disposed)
			{
				this.InternalDispose();
			}
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x00034050 File Offset: 0x00032250
		[SecurityCritical]
		private IntPtr WindowFilterMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			IntPtr zero = IntPtr.Zero;
			if (msg != 5)
			{
				if (msg == 36)
				{
					handled = this.WmGetMinMaxInfo(lParam);
				}
			}
			else
			{
				handled = this.WmSizeChanged(wParam);
			}
			if (this._swh != null && this._swh.CompositionTarget != null)
			{
				if (msg == (int)Window.WM_TASKBARBUTTONCREATED || msg == (int)Window.WM_APPLYTASKBARITEMINFO)
				{
					if (this._taskbarRetryTimer != null)
					{
						this._taskbarRetryTimer.Stop();
					}
					this.ApplyTaskbarItemInfo();
				}
				else
				{
					if (msg <= 16)
					{
						switch (msg)
						{
						case 2:
							handled = this.WmDestroy();
							return zero;
						case 3:
							handled = this.WmMoveChanged();
							return zero;
						case 4:
						case 5:
							break;
						case 6:
							handled = this.WmActivate(wParam);
							return zero;
						default:
							if (msg == 16)
							{
								handled = this.WmClose();
								return zero;
							}
							break;
						}
					}
					else
					{
						if (msg == 24)
						{
							handled = this.WmShowWindow(wParam, lParam);
							return zero;
						}
						if (msg == 132)
						{
							handled = this.WmNcHitTest(lParam, ref zero);
							return zero;
						}
						if (msg == 273)
						{
							handled = this.WmCommand(wParam, lParam);
							return zero;
						}
					}
					handled = false;
				}
			}
			return zero;
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x0003416C File Offset: 0x0003236C
		private bool WmCommand(IntPtr wParam, IntPtr lParam)
		{
			if (NativeMethods.SignedHIWORD(wParam.ToInt32()) == 6144)
			{
				TaskbarItemInfo taskbarItemInfo = this.TaskbarItemInfo;
				if (taskbarItemInfo != null)
				{
					int num = NativeMethods.SignedLOWORD(wParam.ToInt32());
					if (num >= 0 && num < taskbarItemInfo.ThumbButtonInfos.Count)
					{
						taskbarItemInfo.ThumbButtonInfos[num].InvokeClick();
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x000341CC File Offset: 0x000323CC
		private bool WmClose()
		{
			if (this.IsSourceWindowNull || this.IsCompositionTargetInvalid)
			{
				return false;
			}
			this._isClosing = true;
			CancelEventArgs cancelEventArgs = new CancelEventArgs(false);
			try
			{
				this.OnClosing(cancelEventArgs);
			}
			catch
			{
				this.CloseWindowFromWmClose();
				throw;
			}
			if (this.ShouldCloseWindow(cancelEventArgs.Cancel))
			{
				this.CloseWindowFromWmClose();
				return false;
			}
			this._isClosing = false;
			this._dialogResult = null;
			return true;
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x00034248 File Offset: 0x00032448
		private void CloseWindowFromWmClose()
		{
			if (this._showingAsDialog)
			{
				this.DoDialogHide();
			}
			this.ClearRootVisual();
			this.ClearHiddenWindowIfAny();
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x00034264 File Offset: 0x00032464
		private bool ShouldCloseWindow(bool cancelled)
		{
			return !cancelled || this._appShuttingDown || this._ignoreCancel;
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x0003427C File Offset: 0x0003247C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void DoDialogHide()
		{
			SecurityHelper.DemandUnmanagedCode();
			if (this._dispatcherFrame != null)
			{
				this._dispatcherFrame.Continue = false;
				this._dispatcherFrame = null;
			}
			if (this._dialogResult == null)
			{
				this._dialogResult = new bool?(false);
			}
			this._showingAsDialog = false;
			bool isActiveWindow = this._swh.IsActiveWindow;
			this.EnableThreadWindows(true);
			if (isActiveWindow && this._dialogPreviousActiveHandle != IntPtr.Zero && UnsafeNativeMethods.IsWindow(new HandleRef(this, this._dialogPreviousActiveHandle)))
			{
				UnsafeNativeMethods.SetActiveWindow(new HandleRef(this, this._dialogPreviousActiveHandle));
			}
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x00034318 File Offset: 0x00032518
		[SecurityCritical]
		private void UpdateWindowListsOnClose()
		{
			WindowCollection ownedWindowsInternal = this.OwnedWindowsInternal;
			while (ownedWindowsInternal.Count > 0)
			{
				ownedWindowsInternal[0].InternalClose(false, true);
			}
			if (!this.IsOwnerNull)
			{
				this.Owner.OwnedWindowsInternal.Remove(this);
			}
			if (this.IsInsideApp)
			{
				if (Application.Current.Dispatcher.Thread == Dispatcher.CurrentDispatcher.Thread)
				{
					this.App.WindowsInternal.Remove(this);
					if (!this._appShuttingDown && ((this.App.Windows.Count == 0 && this.App.ShutdownMode == ShutdownMode.OnLastWindowClose) || (this.App.MainWindow == this && this.App.ShutdownMode == ShutdownMode.OnMainWindowClose)))
					{
						this.App.CriticalShutdown(0);
					}
					this.TryClearingMainWindow();
					return;
				}
				this.App.NonAppWindowsInternal.Remove(this);
			}
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x000343FC File Offset: 0x000325FC
		[SecurityCritical]
		private bool WmDestroy()
		{
			if (this.IsSourceWindowNull)
			{
				return false;
			}
			if (!this._disposed)
			{
				this.InternalDispose();
			}
			this.OnClosed(EventArgs.Empty);
			return false;
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x00034424 File Offset: 0x00032624
		private bool WmActivate(IntPtr wParam)
		{
			if (this.IsSourceWindowNull || this.IsCompositionTargetInvalid)
			{
				return false;
			}
			bool windowActivated = NativeMethods.SignedLOWORD(wParam) != 0;
			this.HandleActivate(windowActivated);
			return false;
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x0003445C File Offset: 0x0003265C
		private void UpdateDimensionsToRestoreBounds()
		{
			Rect restoreBounds = this.RestoreBounds;
			base.SetValue(Window.LeftProperty, restoreBounds.Left);
			base.SetValue(Window.TopProperty, restoreBounds.Top);
			base.SetValue(FrameworkElement.WidthProperty, restoreBounds.Width);
			base.SetValue(FrameworkElement.HeightProperty, restoreBounds.Height);
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x000344CC File Offset: 0x000326CC
		private bool WmSizeChanged(IntPtr wParam)
		{
			if (this.IsSourceWindowNull || this.IsCompositionTargetInvalid)
			{
				return false;
			}
			NativeMethods.RECT windowBounds = this.WindowBounds;
			Point ptDeviceUnits = new Point((double)(windowBounds.right - windowBounds.left), (double)(windowBounds.bottom - windowBounds.top));
			Point point = this.DeviceToLogicalUnits(ptDeviceUnits);
			try
			{
				this._updateHwndSize = false;
				base.SetValue(FrameworkElement.WidthProperty, point.X);
				base.SetValue(FrameworkElement.HeightProperty, point.Y);
			}
			finally
			{
				this._updateHwndSize = true;
			}
			HRESULT hresult = this.UpdateTaskbarThumbnailClipping();
			switch ((int)wParam)
			{
			case 0:
				if (this._previousWindowState != WindowState.Normal)
				{
					if (this.WindowState != WindowState.Normal)
					{
						this.WindowState = WindowState.Normal;
						this.WmMoveChangedHelper();
					}
					this._previousWindowState = WindowState.Normal;
					this.OnStateChanged(EventArgs.Empty);
				}
				break;
			case 1:
				if (this._previousWindowState != WindowState.Minimized)
				{
					if (this.WindowState != WindowState.Minimized)
					{
						try
						{
							this._updateHwndSize = false;
							this._updateHwndLocation = false;
							this.UpdateDimensionsToRestoreBounds();
						}
						finally
						{
							this._updateHwndSize = true;
							this._updateHwndLocation = true;
						}
						this.WindowState = WindowState.Minimized;
					}
					this._previousWindowState = WindowState.Minimized;
					this.OnStateChanged(EventArgs.Empty);
				}
				break;
			case 2:
				if (this._previousWindowState != WindowState.Maximized)
				{
					if (this.WindowState != WindowState.Maximized)
					{
						try
						{
							this._updateHwndLocation = false;
							this._updateHwndSize = false;
							this.UpdateDimensionsToRestoreBounds();
						}
						finally
						{
							this._updateHwndSize = true;
							this._updateHwndLocation = true;
						}
						this.WindowState = WindowState.Maximized;
					}
					this._windowMaxWidthDeviceUnits = Math.Max(this._windowMaxWidthDeviceUnits, ptDeviceUnits.X);
					this._windowMaxHeightDeviceUnits = Math.Max(this._windowMaxHeightDeviceUnits, ptDeviceUnits.Y);
					this._previousWindowState = WindowState.Maximized;
					this.OnStateChanged(EventArgs.Empty);
				}
				break;
			}
			return false;
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x000346B8 File Offset: 0x000328B8
		[SecurityCritical]
		private bool WmMoveChanged()
		{
			if (this.IsSourceWindowNull || this.IsCompositionTargetInvalid)
			{
				return false;
			}
			if (!this._inTrustedSubWindow)
			{
				SecurityHelper.DemandUIWindowPermission();
			}
			NativeMethods.RECT windowBounds = this.WindowBounds;
			Point point = this.DeviceToLogicalUnits(new Point((double)windowBounds.left, (double)windowBounds.top));
			if (!DoubleUtil.AreClose(this._actualLeft, point.X) || !DoubleUtil.AreClose(this._actualTop, point.Y))
			{
				this._actualLeft = point.X;
				this._actualTop = point.Y;
				this.WmMoveChangedHelper();
				AutomationPeer automationPeer = UIElementAutomationPeer.FromElement(this);
				if (automationPeer != null)
				{
					automationPeer.InvalidatePeer();
				}
			}
			return false;
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x00034760 File Offset: 0x00032960
		internal virtual void WmMoveChangedHelper()
		{
			if (this.WindowState == WindowState.Normal)
			{
				try
				{
					this._updateHwndLocation = false;
					base.SetValue(Window.LeftProperty, this._actualLeft);
					base.SetValue(Window.TopProperty, this._actualTop);
				}
				finally
				{
					this._updateHwndLocation = true;
				}
				this.OnLocationChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x000347D0 File Offset: 0x000329D0
		[SecurityCritical]
		private bool WmGetMinMaxInfo(IntPtr lParam)
		{
			NativeMethods.MINMAXINFO minmaxinfo = (NativeMethods.MINMAXINFO)UnsafeNativeMethods.PtrToStructure(lParam, typeof(NativeMethods.MINMAXINFO));
			this._trackMinWidthDeviceUnits = (double)minmaxinfo.ptMinTrackSize.x;
			this._trackMinHeightDeviceUnits = (double)minmaxinfo.ptMinTrackSize.y;
			this._trackMaxWidthDeviceUnits = (double)minmaxinfo.ptMaxTrackSize.x;
			this._trackMaxHeightDeviceUnits = (double)minmaxinfo.ptMaxTrackSize.y;
			this._windowMaxWidthDeviceUnits = (double)minmaxinfo.ptMaxSize.x;
			this._windowMaxHeightDeviceUnits = (double)minmaxinfo.ptMaxSize.y;
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				Window.WindowMinMax windowMinMax = this.GetWindowMinMax();
				Point point = this.LogicalToDeviceUnits(new Point(windowMinMax.minWidth, windowMinMax.minHeight));
				Point point2 = this.LogicalToDeviceUnits(new Point(windowMinMax.maxWidth, windowMinMax.maxHeight));
				minmaxinfo.ptMinTrackSize.x = DoubleUtil.DoubleToInt(point.X);
				minmaxinfo.ptMinTrackSize.y = DoubleUtil.DoubleToInt(point.Y);
				minmaxinfo.ptMaxTrackSize.x = DoubleUtil.DoubleToInt(point2.X);
				minmaxinfo.ptMaxTrackSize.y = DoubleUtil.DoubleToInt(point2.Y);
				Marshal.StructureToPtr(minmaxinfo, lParam, true);
			}
			return true;
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x00034911 File Offset: 0x00032B11
		private bool WmNcHitTest(IntPtr lParam, ref IntPtr refInt)
		{
			return !this.IsSourceWindowNull && !this.IsCompositionTargetInvalid && this.HandleWmNcHitTestMsg(lParam, ref refInt);
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x00034930 File Offset: 0x00032B30
		internal virtual bool HandleWmNcHitTestMsg(IntPtr lParam, ref IntPtr refInt)
		{
			if (this._resizeGripControl == null || this.ResizeMode != ResizeMode.CanResizeWithGrip)
			{
				return false;
			}
			int x = NativeMethods.SignedLOWORD(lParam);
			int y = NativeMethods.SignedHIWORD(lParam);
			NativeMethods.POINT pointRelativeToWindow = this.GetPointRelativeToWindow(x, y);
			Point point = this.DeviceToLogicalUnits(new Point((double)pointRelativeToWindow.x, (double)pointRelativeToWindow.y));
			GeneralTransform generalTransform = base.TransformToDescendant(this._resizeGripControl);
			Point point2 = point;
			if (generalTransform == null || !generalTransform.TryTransform(point, out point2))
			{
				return false;
			}
			if (point2.X < 0.0 || point2.Y < 0.0 || point2.X > this._resizeGripControl.RenderSize.Width || point2.Y > this._resizeGripControl.RenderSize.Height)
			{
				return false;
			}
			if (base.FlowDirection == FlowDirection.RightToLeft)
			{
				refInt = new IntPtr(16);
			}
			else
			{
				refInt = new IntPtr(17);
			}
			return true;
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x00034A24 File Offset: 0x00032C24
		private bool WmShowWindow(IntPtr wParam, IntPtr lParam)
		{
			if (this.IsSourceWindowNull || this.IsCompositionTargetInvalid)
			{
				return false;
			}
			int num = NativeMethods.IntPtrToInt32(lParam);
			if (num != 1)
			{
				if (num == 3)
				{
					this._isVisible = true;
					this.UpdateVisibilityProperty(Visibility.Visible);
				}
			}
			else
			{
				this._isVisible = false;
				this.UpdateVisibilityProperty(Visibility.Hidden);
			}
			return false;
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x00034A74 File Offset: 0x00032C74
		private static void _OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = (Window)d;
			window.OnIconChanged(e.NewValue as ImageSource);
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x00034A9A File Offset: 0x00032C9A
		private void OnIconChanged(ImageSource newIcon)
		{
			this._icon = newIcon;
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				this.UpdateIcon();
			}
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x00034ABC File Offset: 0x00032CBC
		private static void _OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = (Window)d;
			window.OnTitleChanged();
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x00034AD6 File Offset: 0x00032CD6
		private static bool _ValidateText(object value)
		{
			return value != null;
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x00034ADC File Offset: 0x00032CDC
		private void OnTitleChanged()
		{
			this.UpdateTitle(this.Title);
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x00034AEC File Offset: 0x00032CEC
		private static void _OnShowInTaskbarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = (Window)d;
			window.OnShowInTaskbarChanged();
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x00034B08 File Offset: 0x00032D08
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void OnShowInTaskbarChanged()
		{
			if (!this._inTrustedSubWindow)
			{
				SecurityHelper.DemandUIWindowPermission();
			}
			this.VerifyApiSupported();
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				bool flag = false;
				if (this._isVisible)
				{
					UnsafeNativeMethods.SetWindowPos(new HandleRef(this, this.CriticalHandle), NativeMethods.NullHandleRef, 0, 0, 0, 0, 1175);
					flag = true;
				}
				using (Window.HwndStyleManager.StartManaging(this, this.StyleFromHwnd, this.StyleExFromHwnd))
				{
					this.SetTaskbarStatus();
				}
				if (flag)
				{
					UnsafeNativeMethods.SetWindowPos(new HandleRef(this, this.CriticalHandle), NativeMethods.NullHandleRef, 0, 0, 0, 0, 1111);
				}
			}
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x00034BBC File Offset: 0x00032DBC
		private static bool _ValidateWindowStateCallback(object value)
		{
			return Window.IsValidWindowState((WindowState)value);
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x00034BCC File Offset: 0x00032DCC
		private static void _OnWindowStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = (Window)d;
			window.OnWindowStateChanged((WindowState)e.NewValue);
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x00034BF4 File Offset: 0x00032DF4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void OnWindowStateChanged(WindowState windowState)
		{
			SecurityHelper.DemandUIWindowPermission();
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				if (this._isVisible)
				{
					HandleRef hWnd = new HandleRef(this, this.CriticalHandle);
					int style = this._Style;
					switch (windowState)
					{
					case WindowState.Normal:
						if ((style & 16777216) == 16777216)
						{
							if (this.ShowActivated || this.IsActive)
							{
								UnsafeNativeMethods.ShowWindow(hWnd, 9);
							}
							else
							{
								UnsafeNativeMethods.ShowWindow(hWnd, 4);
							}
						}
						else if ((style & 536870912) == 536870912)
						{
							NativeMethods.WINDOWPLACEMENT windowplacement = default(NativeMethods.WINDOWPLACEMENT);
							windowplacement.length = Marshal.SizeOf(windowplacement);
							UnsafeNativeMethods.GetWindowPlacement(hWnd, ref windowplacement);
							if ((windowplacement.flags & 2) == 2)
							{
								UnsafeNativeMethods.ShowWindow(hWnd, 9);
							}
							else if (this.ShowActivated)
							{
								UnsafeNativeMethods.ShowWindow(hWnd, 9);
							}
							else
							{
								UnsafeNativeMethods.ShowWindow(hWnd, 4);
							}
						}
						break;
					case WindowState.Minimized:
						if ((style & 536870912) != 536870912)
						{
							UnsafeNativeMethods.ShowWindow(hWnd, 6);
						}
						break;
					case WindowState.Maximized:
						if ((style & 16777216) != 16777216)
						{
							UnsafeNativeMethods.ShowWindow(hWnd, 3);
						}
						break;
					}
				}
			}
			else
			{
				this._previousWindowState = windowState;
			}
			try
			{
				this._updateHwndLocation = false;
				base.CoerceValue(Window.TopProperty);
				base.CoerceValue(Window.LeftProperty);
			}
			finally
			{
				this._updateHwndLocation = true;
			}
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x00034D64 File Offset: 0x00032F64
		private static bool _ValidateWindowStyleCallback(object value)
		{
			return Window.IsValidWindowStyle((WindowStyle)value);
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x00034D74 File Offset: 0x00032F74
		private static void _OnWindowStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = (Window)d;
			window.OnWindowStyleChanged((WindowStyle)e.NewValue);
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x00034D9C File Offset: 0x00032F9C
		private void OnWindowStyleChanged(WindowStyle windowStyle)
		{
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				using (Window.HwndStyleManager.StartManaging(this, this.StyleFromHwnd, this.StyleExFromHwnd))
				{
					this.CreateWindowStyle();
				}
			}
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x00034DF0 File Offset: 0x00032FF0
		private static void _OnTopmostChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = (Window)d;
			window.OnTopmostChanged((bool)e.NewValue);
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x00034E18 File Offset: 0x00033018
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void OnTopmostChanged(bool topmost)
		{
			SecurityHelper.DemandUIWindowPermission();
			this.VerifyApiSupported();
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				HandleRef hWndInsertAfter = topmost ? NativeMethods.HWND_TOPMOST : NativeMethods.HWND_NOTOPMOST;
				UnsafeNativeMethods.SetWindowPos(new HandleRef(null, this.CriticalHandle), hWndInsertAfter, 0, 0, 0, 0, 19);
			}
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x00034E6C File Offset: 0x0003306C
		private static object CoerceVisibility(DependencyObject d, object value)
		{
			Window window = (Window)d;
			if ((Visibility)value == Visibility.Visible)
			{
				window.VerifyCanShow();
				window.VerifyConsistencyWithAllowsTransparency();
				window.VerifyNotClosing();
				window.VerifyConsistencyWithShowActivated();
			}
			return value;
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x00034EA4 File Offset: 0x000330A4
		private static void _OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = (Window)d;
			window._isVisibilitySet = true;
			if (window._visibilitySetInternally)
			{
				return;
			}
			bool flag = Window.VisibilityToBool((Visibility)e.NewValue);
			window.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(window.ShowHelper), flag ? BooleanBoxes.TrueBox : BooleanBoxes.FalseBox);
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x00034F03 File Offset: 0x00033103
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void SafeCreateWindowDuringShow()
		{
			if (this.IsSourceWindowNull)
			{
				if (!this._inTrustedSubWindow)
				{
					SecurityHelper.DemandUIWindowPermission();
				}
				this.CreateSourceWindowDuringShow();
				return;
			}
			if (this.HwndCreatedButNotShown)
			{
				this.SetRootVisualAndUpdateSTC();
				this._hwndCreatedButNotShown = false;
			}
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x00034F36 File Offset: 0x00033136
		private void SetShowKeyboardCueState()
		{
			if (KeyboardNavigation.IsKeyboardMostRecentInputDevice())
			{
				this._previousKeyboardCuesProperty = (bool)base.GetValue(KeyboardNavigation.ShowKeyboardCuesProperty);
				base.SetValue(KeyboardNavigation.ShowKeyboardCuesProperty, BooleanBoxes.TrueBox);
				this._resetKeyboardCuesProperty = true;
			}
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x00034F6C File Offset: 0x0003316C
		private void ClearShowKeyboardCueState()
		{
			if (this._resetKeyboardCuesProperty)
			{
				this._resetKeyboardCuesProperty = false;
				base.SetValue(KeyboardNavigation.ShowKeyboardCuesProperty, BooleanBoxes.Box(this._previousKeyboardCuesProperty));
			}
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x00034F94 File Offset: 0x00033194
		private void UpdateVisibilityProperty(Visibility value)
		{
			try
			{
				this._visibilitySetInternally = true;
				base.SetValue(UIElement.VisibilityProperty, value);
			}
			finally
			{
				this._visibilitySetInternally = false;
			}
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x00034FD4 File Offset: 0x000331D4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private object ShowHelper(object booleanBox)
		{
			if (this._disposed)
			{
				return null;
			}
			bool flag = (bool)booleanBox;
			this._isClosing = false;
			if (this._isVisible == flag)
			{
				return null;
			}
			if (flag)
			{
				if (Application.IsShuttingDown)
				{
					return null;
				}
				this.SetShowKeyboardCueState();
				this.SafeCreateWindowDuringShow();
				this._isVisible = true;
			}
			else
			{
				if (!this._inTrustedSubWindow)
				{
					SecurityHelper.DemandUIWindowPermission();
				}
				this.ClearShowKeyboardCueState();
				if (this._showingAsDialog)
				{
					this.DoDialogHide();
				}
				this._isVisible = false;
			}
			if (!this.IsSourceWindowNull)
			{
				int num;
				if (flag)
				{
					num = this.nCmdForShow();
				}
				else
				{
					num = 0;
				}
				bool flag2 = (bool)base.GetValue(Window.TopmostProperty);
				if (flag2 && FrameworkCompatibilityPreferences.GetUseSetWindowPosForTopmostWindows() && (num == 5 || num == 8))
				{
					int num2 = (num == 8) ? 16 : 0;
					UnsafeNativeMethods.SetWindowPos(new HandleRef(this, this.CriticalHandle), NativeMethods.HWND_TOPMOST, 0, 0, 0, 0, num2 | 2 | 1 | 512 | 64);
				}
				else
				{
					UnsafeNativeMethods.ShowWindow(new HandleRef(this, this.CriticalHandle), num);
				}
				this.SafeStyleSetter();
			}
			if (this._showingAsDialog && this._isVisible)
			{
				try
				{
					ComponentDispatcher.PushModal();
					this._dispatcherFrame = new DispatcherFrame();
					Dispatcher.PushFrame(this._dispatcherFrame);
				}
				finally
				{
					ComponentDispatcher.PopModal();
				}
			}
			return null;
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x00035120 File Offset: 0x00033320
		internal virtual int nCmdForShow()
		{
			WindowState windowState = this.WindowState;
			int result;
			if (windowState != WindowState.Minimized)
			{
				if (windowState == WindowState.Maximized)
				{
					result = 3;
				}
				else
				{
					result = (this.ShowActivated ? 5 : 8);
				}
			}
			else
			{
				result = (this.ShowActivated ? 2 : 7);
			}
			return result;
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x00035160 File Offset: 0x00033360
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void SafeStyleSetter()
		{
			new UIPermission(UIPermissionWindow.AllWindows).Assert();
			try
			{
				using (Window.HwndStyleManager.StartManaging(this, this.StyleFromHwnd, this.StyleExFromHwnd))
				{
					this._Style = (this._isVisible ? (this._Style | 268435456) : this._Style);
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x000351DC File Offset: 0x000333DC
		private static bool _ValidateSizeToContentCallback(object value)
		{
			return Window.IsValidSizeToContent((SizeToContent)value);
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x000351EC File Offset: 0x000333EC
		private static object _SizeToContentGetValueOverride(DependencyObject d)
		{
			Window window = d as Window;
			return window.SizeToContent;
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x0003520C File Offset: 0x0003340C
		private static void _OnSizeToContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = d as Window;
			window.OnSizeToContentChanged((SizeToContent)e.NewValue);
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x00035232 File Offset: 0x00033432
		private void OnSizeToContentChanged(SizeToContent sizeToContent)
		{
			this.VerifyApiSupported();
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				this.HwndSourceSizeToContent = sizeToContent;
			}
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x00035254 File Offset: 0x00033454
		private static void ValidateLengthForHeightWidth(double l)
		{
			if (!double.IsPositiveInfinity(l) && !DoubleUtil.IsNaN(l) && (l > 2147483647.0 || l < -2147483648.0))
			{
				throw new ArgumentException(SR.Get("ValueNotBetweenInt32MinMax", new object[]
				{
					l
				}));
			}
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x000352A8 File Offset: 0x000334A8
		private static void ValidateTopLeft(double length)
		{
			if (double.IsPositiveInfinity(length) || double.IsNegativeInfinity(length))
			{
				throw new ArgumentException(SR.Get("InvalidValueForTopLeft", new object[]
				{
					length
				}));
			}
			if (length > 2147483647.0 || length < -2147483648.0)
			{
				throw new ArgumentException(SR.Get("ValueNotBetweenInt32MinMax", new object[]
				{
					length
				}));
			}
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x0003531C File Offset: 0x0003351C
		private static void _OnHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = d as Window;
			if (window._updateHwndSize)
			{
				window.OnHeightChanged((double)e.NewValue);
			}
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x0003534A File Offset: 0x0003354A
		private void OnHeightChanged(double height)
		{
			Window.ValidateLengthForHeightWidth(height);
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid && !DoubleUtil.IsNaN(height))
			{
				this.UpdateHeight(height);
			}
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x00035374 File Offset: 0x00033574
		private static void _OnMinHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = d as Window;
			window.OnMinHeightChanged((double)e.NewValue);
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x0003539C File Offset: 0x0003359C
		private void OnMinHeightChanged(double minHeight)
		{
			this.VerifyApiSupported();
			Window.ValidateLengthForHeightWidth(minHeight);
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				NativeMethods.RECT windowBounds = this.WindowBounds;
				Point point = this.DeviceToLogicalUnits(new Point((double)windowBounds.Width, (double)windowBounds.Height));
				if (minHeight > point.Y && this.WindowState == WindowState.Normal)
				{
					this.UpdateHwndSizeOnWidthHeightChange(point.X, minHeight);
				}
			}
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x00035408 File Offset: 0x00033608
		private static void _OnMaxHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = d as Window;
			window.OnMaxHeightChanged((double)e.NewValue);
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x00035430 File Offset: 0x00033630
		private void OnMaxHeightChanged(double maxHeight)
		{
			this.VerifyApiSupported();
			Window.ValidateLengthForHeightWidth(base.MaxHeight);
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				NativeMethods.RECT windowBounds = this.WindowBounds;
				Point point = this.DeviceToLogicalUnits(new Point((double)windowBounds.Width, (double)windowBounds.Height));
				if (maxHeight < point.Y && this.WindowState == WindowState.Normal)
				{
					this.UpdateHwndSizeOnWidthHeightChange(point.X, maxHeight);
				}
			}
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x000354A4 File Offset: 0x000336A4
		private static void _OnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = d as Window;
			if (window._updateHwndSize)
			{
				window.OnWidthChanged((double)e.NewValue);
			}
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x000354D2 File Offset: 0x000336D2
		private void OnWidthChanged(double width)
		{
			Window.ValidateLengthForHeightWidth(width);
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid && !DoubleUtil.IsNaN(width))
			{
				this.UpdateWidth(width);
			}
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x000354FC File Offset: 0x000336FC
		private static void _OnMinWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = d as Window;
			window.OnMinWidthChanged((double)e.NewValue);
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x00035524 File Offset: 0x00033724
		private void OnMinWidthChanged(double minWidth)
		{
			this.VerifyApiSupported();
			Window.ValidateLengthForHeightWidth(minWidth);
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				NativeMethods.RECT windowBounds = this.WindowBounds;
				Point point = this.DeviceToLogicalUnits(new Point((double)windowBounds.Width, (double)windowBounds.Height));
				if (minWidth > point.X && this.WindowState == WindowState.Normal)
				{
					this.UpdateHwndSizeOnWidthHeightChange(minWidth, point.Y);
				}
			}
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x00035590 File Offset: 0x00033790
		private static void _OnMaxWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = d as Window;
			window.OnMaxWidthChanged((double)e.NewValue);
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x000355B8 File Offset: 0x000337B8
		private void OnMaxWidthChanged(double maxWidth)
		{
			this.VerifyApiSupported();
			Window.ValidateLengthForHeightWidth(maxWidth);
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				NativeMethods.RECT windowBounds = this.WindowBounds;
				Point point = this.DeviceToLogicalUnits(new Point((double)windowBounds.Width, (double)windowBounds.Height));
				if (maxWidth < point.X && this.WindowState == WindowState.Normal)
				{
					this.UpdateHwndSizeOnWidthHeightChange(maxWidth, point.Y);
				}
			}
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x00035624 File Offset: 0x00033824
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void UpdateHwndRestoreBounds(double newValue, Window.BoundsSpecified specifiedRestoreBounds)
		{
			SecurityHelper.DemandUIWindowPermission();
			NativeMethods.WINDOWPLACEMENT windowplacement = default(NativeMethods.WINDOWPLACEMENT);
			windowplacement.length = Marshal.SizeOf(typeof(NativeMethods.WINDOWPLACEMENT));
			UnsafeNativeMethods.GetWindowPlacement(new HandleRef(this, this.CriticalHandle), ref windowplacement);
			double x = this.LogicalToDeviceUnits(new Point(newValue, 0.0)).X;
			switch (specifiedRestoreBounds)
			{
			case Window.BoundsSpecified.Height:
				windowplacement.rcNormalPosition_bottom = windowplacement.rcNormalPosition_top + DoubleUtil.DoubleToInt(x);
				break;
			case Window.BoundsSpecified.Width:
				windowplacement.rcNormalPosition_right = windowplacement.rcNormalPosition_left + DoubleUtil.DoubleToInt(x);
				break;
			case Window.BoundsSpecified.Top:
			{
				double num = newValue;
				if ((this.StyleExFromHwnd & 128) == 0)
				{
					num = this.TransformWorkAreaScreenArea(new Point(0.0, num), Window.TransformType.ScreenAreaToWorkArea).Y;
				}
				num = this.LogicalToDeviceUnits(new Point(0.0, num)).Y;
				int num2 = windowplacement.rcNormalPosition_bottom - windowplacement.rcNormalPosition_top;
				windowplacement.rcNormalPosition_top = DoubleUtil.DoubleToInt(num);
				windowplacement.rcNormalPosition_bottom = windowplacement.rcNormalPosition_top + num2;
				break;
			}
			case Window.BoundsSpecified.Left:
			{
				double num3 = newValue;
				if ((this.StyleExFromHwnd & 128) == 0)
				{
					num3 = this.TransformWorkAreaScreenArea(new Point(num3, 0.0), Window.TransformType.ScreenAreaToWorkArea).X;
				}
				num3 = this.LogicalToDeviceUnits(new Point(num3, 0.0)).X;
				int num4 = windowplacement.rcNormalPosition_right - windowplacement.rcNormalPosition_left;
				windowplacement.rcNormalPosition_left = DoubleUtil.DoubleToInt(num3);
				windowplacement.rcNormalPosition_right = windowplacement.rcNormalPosition_left + num4;
				break;
			}
			}
			if (!this._isVisible)
			{
				windowplacement.showCmd = 0;
			}
			UnsafeNativeMethods.SetWindowPlacement(new HandleRef(this, this.CriticalHandle), ref windowplacement);
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x000357F0 File Offset: 0x000339F0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private Point TransformWorkAreaScreenArea(Point pt, Window.TransformType transformType)
		{
			int num = 0;
			int num2 = 0;
			if (!this._inTrustedSubWindow)
			{
				SecurityHelper.DemandUIWindowPermission();
			}
			IntPtr intPtr = SafeNativeMethods.MonitorFromWindow(new HandleRef(this, this.CriticalHandle), 0);
			if (intPtr != IntPtr.Zero)
			{
				NativeMethods.MONITORINFOEX monitorinfoex = new NativeMethods.MONITORINFOEX();
				monitorinfoex.cbSize = Marshal.SizeOf(typeof(NativeMethods.MONITORINFOEX));
				SafeNativeMethods.GetMonitorInfo(new HandleRef(this, intPtr), monitorinfoex);
				NativeMethods.RECT rcWork = monitorinfoex.rcWork;
				NativeMethods.RECT rcMonitor = monitorinfoex.rcMonitor;
				num = rcWork.left - rcMonitor.left;
				num2 = rcWork.top - rcMonitor.top;
			}
			Point result;
			if (transformType == Window.TransformType.WorkAreaToScreenArea)
			{
				result = new Point(pt.X + (double)num, pt.Y + (double)num2);
			}
			else
			{
				result = new Point(pt.X - (double)num, pt.Y - (double)num2);
			}
			return result;
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x000358C8 File Offset: 0x00033AC8
		private static object CoerceTop(DependencyObject d, object value)
		{
			Window window = d as Window;
			window.VerifyApiSupported();
			double num = (double)value;
			Window.ValidateTopLeft(num);
			if (window.IsSourceWindowNull || window.IsCompositionTargetInvalid)
			{
				return value;
			}
			if (double.IsNaN(num))
			{
				return window._actualTop;
			}
			if (window.WindowState != WindowState.Normal)
			{
				return value;
			}
			if (window._updateStartupLocation && window.WindowStartupLocation != WindowStartupLocation.Manual)
			{
				return window._actualTop;
			}
			return value;
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x0003593C File Offset: 0x00033B3C
		private static void _OnTopChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = d as Window;
			if (window._updateHwndLocation)
			{
				window.OnTopChanged((double)e.NewValue);
			}
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x0003596C File Offset: 0x00033B6C
		private void OnTopChanged(double newTop)
		{
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				if (!DoubleUtil.IsNaN(newTop))
				{
					if (this.WindowState == WindowState.Normal)
					{
						Invariant.Assert(!double.IsNaN(this._actualLeft), "_actualLeft cannot be NaN after show");
						this.UpdateHwndPositionOnTopLeftChange(double.IsNaN(this.Left) ? this._actualLeft : this.Left, newTop);
						return;
					}
					this.UpdateHwndRestoreBounds(newTop, Window.BoundsSpecified.Top);
					return;
				}
			}
			else
			{
				this._actualTop = newTop;
			}
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x000359E4 File Offset: 0x00033BE4
		private static object CoerceLeft(DependencyObject d, object value)
		{
			Window window = d as Window;
			window.VerifyApiSupported();
			double num = (double)value;
			Window.ValidateTopLeft(num);
			if (window.IsSourceWindowNull || window.IsCompositionTargetInvalid)
			{
				return value;
			}
			if (double.IsNaN(num))
			{
				return window._actualLeft;
			}
			if (window.WindowState != WindowState.Normal)
			{
				return value;
			}
			if (window._updateStartupLocation && window.WindowStartupLocation != WindowStartupLocation.Manual)
			{
				return window._actualLeft;
			}
			return value;
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x00035A58 File Offset: 0x00033C58
		private static void _OnLeftChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = d as Window;
			if (window._updateHwndLocation)
			{
				window.OnLeftChanged((double)e.NewValue);
			}
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x00035A88 File Offset: 0x00033C88
		private void OnLeftChanged(double newLeft)
		{
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				if (!DoubleUtil.IsNaN(newLeft))
				{
					if (this.WindowState == WindowState.Normal)
					{
						Invariant.Assert(!double.IsNaN(this._actualTop), "_actualTop cannot be NaN after show");
						this.UpdateHwndPositionOnTopLeftChange(newLeft, double.IsNaN(this.Top) ? this._actualTop : this.Top);
						return;
					}
					this.UpdateHwndRestoreBounds(newLeft, Window.BoundsSpecified.Left);
					return;
				}
			}
			else
			{
				this._actualLeft = newLeft;
			}
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x00035B00 File Offset: 0x00033D00
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void UpdateHwndPositionOnTopLeftChange(double leftLogicalUnits, double topLogicalUnits)
		{
			SecurityHelper.DemandUIWindowPermission();
			Point point = this.LogicalToDeviceUnits(new Point(leftLogicalUnits, topLogicalUnits));
			UnsafeNativeMethods.SetWindowPos(new HandleRef(this, this.CriticalHandle), new HandleRef(null, IntPtr.Zero), DoubleUtil.DoubleToInt(point.X), DoubleUtil.DoubleToInt(point.Y), 0, 0, 21);
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x00035B59 File Offset: 0x00033D59
		private static bool _ValidateResizeModeCallback(object value)
		{
			return Window.IsValidResizeMode((ResizeMode)value);
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x00035B68 File Offset: 0x00033D68
		private static void _OnResizeModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = d as Window;
			window.OnResizeModeChanged();
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x00035B84 File Offset: 0x00033D84
		private void OnResizeModeChanged()
		{
			this.VerifyApiSupported();
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				using (Window.HwndStyleManager.StartManaging(this, this.StyleFromHwnd, this.StyleExFromHwnd))
				{
					this.CreateResizibility();
				}
			}
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x00035BDC File Offset: 0x00033DDC
		private static object VerifyAccessCoercion(DependencyObject d, object value)
		{
			((Window)d).VerifyApiSupported();
			return value;
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x00035BEC File Offset: 0x00033DEC
		private static void _OnFlowDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = d as Window;
			window.OnFlowDirectionChanged();
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x00035C08 File Offset: 0x00033E08
		private void OnFlowDirectionChanged()
		{
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				using (Window.HwndStyleManager.StartManaging(this, this.StyleFromHwnd, this.StyleExFromHwnd))
				{
					this.CreateRtl();
				}
			}
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x00035C5C File Offset: 0x00033E5C
		private static object CoerceRenderTransform(DependencyObject d, object value)
		{
			Transform transform = (Transform)value;
			if (value != null)
			{
				if (transform != null)
				{
					Matrix value2 = transform.Value;
					if (transform.Value.IsIdentity)
					{
						return value;
					}
				}
				throw new InvalidOperationException(SR.Get("TransformNotSupported"));
			}
			return value;
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x00002137 File Offset: 0x00000337
		private static void _OnRenderTransformChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x00035C9E File Offset: 0x00033E9E
		private static object CoerceClipToBounds(DependencyObject d, object value)
		{
			if ((bool)value)
			{
				throw new InvalidOperationException(SR.Get("ClipToBoundsNotSupported"));
			}
			return value;
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x00002137 File Offset: 0x00000337
		private static void _OnClipToBoundsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x00035CBC File Offset: 0x00033EBC
		[SecurityCritical]
		private HwndWrapper EnsureHiddenWindow()
		{
			if (this._hiddenWindow == null)
			{
				this._hiddenWindow = new HwndWrapper(0, 13565952, 0, int.MinValue, int.MinValue, int.MinValue, int.MinValue, "Hidden Window", IntPtr.Zero, null);
			}
			return this._hiddenWindow;
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x00035D08 File Offset: 0x00033F08
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void SetTaskbarStatus()
		{
			if (!this.ShowInTaskbar)
			{
				SecurityHelper.DemandUIWindowPermission();
				this.EnsureHiddenWindow();
				if (this._ownerHandle == IntPtr.Zero)
				{
					this.SetOwnerHandle(this._hiddenWindow.Handle);
					if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
					{
						this.UpdateIcon();
					}
				}
				this._StyleEx &= -262145;
				return;
			}
			this._StyleEx |= 262144;
			if (!this.IsSourceWindowNull && this._hiddenWindow != null && this._ownerHandle == this._hiddenWindow.Handle)
			{
				this.SetOwnerHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x00035DBA File Offset: 0x00033FBA
		[SecurityCritical]
		private void OnTaskbarRetryTimerTick(object sender, EventArgs e)
		{
			UnsafeNativeMethods.PostMessage(new HandleRef(this, this.CriticalHandle), Window.WM_APPLYTASKBARITEMINFO, IntPtr.Zero, IntPtr.Zero);
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x00035DDC File Offset: 0x00033FDC
		[SecurityCritical]
		private void ApplyTaskbarItemInfo()
		{
			if (!Utilities.IsOSWindows7OrNewer)
			{
				if (TraceShell.IsEnabled)
				{
					TraceShell.Trace(TraceEventType.Warning, TraceShell.NotOnWindows7);
				}
				return;
			}
			if (this.IsSourceWindowNull || this.IsCompositionTargetInvalid)
			{
				return;
			}
			if (this._taskbarRetryTimer != null && this._taskbarRetryTimer.IsEnabled)
			{
				return;
			}
			if (this._taskbarList == null)
			{
				if (this.TaskbarItemInfo == null)
				{
					return;
				}
				ITaskbarList taskbarList = null;
				try
				{
					taskbarList = (ITaskbarList)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("56FDF344-FD6D-11d0-958A-006097C9A090")));
					taskbarList.HrInit();
					this._taskbarList = (ITaskbarList3)taskbarList;
					taskbarList = null;
				}
				finally
				{
					Utilities.SafeRelease<ITaskbarList>(ref taskbarList);
				}
				this._overlaySize = new Size((double)UnsafeNativeMethods.GetSystemMetrics(SM.CXSMICON), (double)UnsafeNativeMethods.GetSystemMetrics(SM.CYSMICON));
				if (this._taskbarRetryTimer == null)
				{
					this._taskbarRetryTimer = new DispatcherTimer
					{
						Interval = new TimeSpan(0, 1, 0)
					};
					this._taskbarRetryTimer.Tick += this.OnTaskbarRetryTimerTick;
				}
			}
			HRESULT hr = HRESULT.S_OK;
			hr = this.RegisterTaskbarThumbButtons();
			if (hr.Succeeded)
			{
				hr = this.UpdateTaskbarProgressState();
			}
			if (hr.Succeeded)
			{
				hr = this.UpdateTaskbarOverlay();
			}
			if (hr.Succeeded)
			{
				hr = this.UpdateTaskbarDescription();
			}
			if (hr.Succeeded)
			{
				hr = this.UpdateTaskbarThumbnailClipping();
			}
			if (hr.Succeeded)
			{
				hr = this.UpdateTaskbarThumbButtons();
			}
			this.HandleTaskbarListError(hr);
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x00035F40 File Offset: 0x00034140
		[SecurityCritical]
		private HRESULT UpdateTaskbarProgressState()
		{
			TaskbarItemInfo taskbarItemInfo = this.TaskbarItemInfo;
			TBPF tbpFlags = TBPF.NOPROGRESS;
			if (taskbarItemInfo != null)
			{
				switch (taskbarItemInfo.ProgressState)
				{
				case TaskbarItemProgressState.None:
					tbpFlags = TBPF.NOPROGRESS;
					break;
				case TaskbarItemProgressState.Indeterminate:
					tbpFlags = TBPF.INDETERMINATE;
					break;
				case TaskbarItemProgressState.Normal:
					tbpFlags = TBPF.NORMAL;
					break;
				case TaskbarItemProgressState.Error:
					tbpFlags = TBPF.ERROR;
					break;
				case TaskbarItemProgressState.Paused:
					tbpFlags = TBPF.PAUSED;
					break;
				default:
					tbpFlags = TBPF.NOPROGRESS;
					break;
				}
			}
			HRESULT result = this._taskbarList.SetProgressState(this.CriticalHandle, tbpFlags);
			if (result.Succeeded)
			{
				result = this.UpdateTaskbarProgressValue();
			}
			return result;
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x00035FB8 File Offset: 0x000341B8
		[SecurityCritical]
		private HRESULT UpdateTaskbarProgressValue()
		{
			TaskbarItemInfo taskbarItemInfo = this.TaskbarItemInfo;
			if (taskbarItemInfo == null || taskbarItemInfo.ProgressState == TaskbarItemProgressState.None || taskbarItemInfo.ProgressState == TaskbarItemProgressState.Indeterminate)
			{
				return HRESULT.S_OK;
			}
			ulong ullCompleted = (ulong)(taskbarItemInfo.ProgressValue * 1000.0);
			return this._taskbarList.SetProgressValue(this.CriticalHandle, ullCompleted, 1000UL);
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x00036010 File Offset: 0x00034210
		[SecurityCritical]
		private HRESULT UpdateTaskbarOverlay()
		{
			TaskbarItemInfo taskbarItemInfo = this.TaskbarItemInfo;
			NativeMethods.IconHandle iconHandle = NativeMethods.IconHandle.GetInvalidIcon();
			HRESULT result;
			try
			{
				if (taskbarItemInfo != null && taskbarItemInfo.Overlay != null)
				{
					iconHandle = IconHelper.CreateIconHandleFromImageSource(taskbarItemInfo.Overlay, this._overlaySize);
				}
				result = this._taskbarList.SetOverlayIcon(this.CriticalHandle, iconHandle, null);
			}
			finally
			{
				iconHandle.Dispose();
			}
			return result;
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x00036078 File Offset: 0x00034278
		[SecurityCritical]
		private HRESULT UpdateTaskbarDescription()
		{
			TaskbarItemInfo taskbarItemInfo = this.TaskbarItemInfo;
			string pszTip = "";
			if (taskbarItemInfo != null)
			{
				pszTip = (taskbarItemInfo.Description ?? "");
			}
			return this._taskbarList.SetThumbnailTooltip(this.CriticalHandle, pszTip);
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x000360B8 File Offset: 0x000342B8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private HRESULT UpdateTaskbarThumbnailClipping()
		{
			if (this._taskbarList == null)
			{
				return HRESULT.S_OK;
			}
			if (this._taskbarRetryTimer != null && this._taskbarRetryTimer.IsEnabled)
			{
				return HRESULT.S_FALSE;
			}
			if (UnsafeNativeMethods.IsIconic(this.CriticalHandle))
			{
				return HRESULT.S_FALSE;
			}
			TaskbarItemInfo taskbarItemInfo = this.TaskbarItemInfo;
			NativeMethods.RefRECT prcClip = null;
			if (taskbarItemInfo != null && !taskbarItemInfo.ThumbnailClipMargin.IsZero)
			{
				Thickness thumbnailClipMargin = taskbarItemInfo.ThumbnailClipMargin;
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				SafeNativeMethods.GetClientRect(new HandleRef(this, this.CriticalHandle), ref rect);
				Rect rect2 = new Rect(this.DeviceToLogicalUnits(new Point((double)rect.left, (double)rect.top)), this.DeviceToLogicalUnits(new Point((double)rect.right, (double)rect.bottom)));
				if (thumbnailClipMargin.Left + thumbnailClipMargin.Right >= rect2.Width || thumbnailClipMargin.Top + thumbnailClipMargin.Bottom >= rect2.Height)
				{
					prcClip = new NativeMethods.RefRECT(0, 0, 0, 0);
				}
				else
				{
					Rect rect3 = new Rect(this.LogicalToDeviceUnits(new Point(thumbnailClipMargin.Left, thumbnailClipMargin.Top)), this.LogicalToDeviceUnits(new Point(rect2.Width - thumbnailClipMargin.Right, rect2.Height - thumbnailClipMargin.Bottom)));
					prcClip = new NativeMethods.RefRECT((int)rect3.Left, (int)rect3.Top, (int)rect3.Right, (int)rect3.Bottom);
				}
			}
			return this._taskbarList.SetThumbnailClip(this.CriticalHandle, prcClip);
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x00036240 File Offset: 0x00034440
		[SecurityCritical]
		private HRESULT RegisterTaskbarThumbButtons()
		{
			THUMBBUTTON[] array = new THUMBBUTTON[7];
			for (int i = 0; i < 7; i++)
			{
				array[i] = new THUMBBUTTON
				{
					iId = (uint)i,
					dwFlags = (THBF.DISABLED | THBF.NOBACKGROUND | THBF.HIDDEN | THBF.NONINTERACTIVE),
					dwMask = (THB.ICON | THB.TOOLTIP | THB.FLAGS)
				};
			}
			HRESULT hresult = this._taskbarList.ThumbBarAddButtons(this.CriticalHandle, (uint)array.Length, array);
			if (hresult == HRESULT.E_INVALIDARG)
			{
				hresult = HRESULT.S_FALSE;
			}
			return hresult;
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x000362B4 File Offset: 0x000344B4
		[SecurityCritical]
		private HRESULT UpdateTaskbarThumbButtons()
		{
			THUMBBUTTON[] array = new THUMBBUTTON[7];
			TaskbarItemInfo taskbarItemInfo = this.TaskbarItemInfo;
			ThumbButtonInfoCollection thumbButtonInfoCollection = null;
			if (taskbarItemInfo != null)
			{
				thumbButtonInfoCollection = taskbarItemInfo.ThumbButtonInfos;
			}
			List<NativeMethods.IconHandle> list = new List<NativeMethods.IconHandle>();
			HRESULT result;
			try
			{
				uint num = 0U;
				if (thumbButtonInfoCollection == null)
				{
					goto IL_1B4;
				}
				using (FreezableCollection<ThumbButtonInfo>.Enumerator enumerator = thumbButtonInfoCollection.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ThumbButtonInfo thumbButtonInfo = enumerator.Current;
						THUMBBUTTON thumbbutton = new THUMBBUTTON
						{
							iId = num,
							dwMask = (THB.ICON | THB.TOOLTIP | THB.FLAGS)
						};
						switch (thumbButtonInfo.Visibility)
						{
						case Visibility.Visible:
							goto IL_A1;
						case Visibility.Hidden:
							thumbbutton.dwFlags = (THBF.DISABLED | THBF.NOBACKGROUND);
							thumbbutton.hIcon = IntPtr.Zero;
							break;
						case Visibility.Collapsed:
							thumbbutton.dwFlags = THBF.HIDDEN;
							break;
						default:
							goto IL_A1;
						}
						IL_14E:
						array[(int)num] = thumbbutton;
						num += 1U;
						if (num == 7U)
						{
							break;
						}
						continue;
						IL_A1:
						thumbbutton.szTip = (thumbButtonInfo.Description ?? "");
						if (thumbButtonInfo.ImageSource != null)
						{
							NativeMethods.IconHandle iconHandle = IconHelper.CreateIconHandleFromImageSource(thumbButtonInfo.ImageSource, this._overlaySize);
							thumbbutton.hIcon = iconHandle.CriticalGetHandle();
							list.Add(iconHandle);
						}
						if (!thumbButtonInfo.IsBackgroundVisible)
						{
							thumbbutton.dwFlags |= THBF.NOBACKGROUND;
						}
						if (!thumbButtonInfo.IsEnabled)
						{
							thumbbutton.dwFlags |= THBF.DISABLED;
						}
						else
						{
							thumbbutton.dwFlags |= THBF.ENABLED;
						}
						if (!thumbButtonInfo.IsInteractive)
						{
							thumbbutton.dwFlags |= THBF.NONINTERACTIVE;
						}
						if (thumbButtonInfo.DismissWhenClicked)
						{
							thumbbutton.dwFlags |= THBF.DISMISSONCLICK;
							goto IL_14E;
						}
						goto IL_14E;
					}
					goto IL_1B4;
				}
				IL_181:
				array[(int)num] = new THUMBBUTTON
				{
					iId = num,
					dwFlags = (THBF.DISABLED | THBF.NOBACKGROUND | THBF.HIDDEN),
					dwMask = (THB.ICON | THB.TOOLTIP | THB.FLAGS)
				};
				num += 1U;
				IL_1B4:
				if (num < 7U)
				{
					goto IL_181;
				}
				result = this._taskbarList.ThumbBarUpdateButtons(this.CriticalHandle, (uint)array.Length, array);
			}
			finally
			{
				foreach (NativeMethods.IconHandle iconHandle2 in list)
				{
					iconHandle2.Dispose();
				}
			}
			return result;
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x00036518 File Offset: 0x00034718
		private void CreateRtl()
		{
			if (base.FlowDirection == FlowDirection.LeftToRight)
			{
				this._StyleEx &= -4194305;
				return;
			}
			if (base.FlowDirection == FlowDirection.RightToLeft)
			{
				this._StyleEx |= 4194304;
				return;
			}
			throw new InvalidOperationException(SR.Get("IncorrectFlowDirection"));
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x0003656C File Offset: 0x0003476C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void Flush()
		{
			Window.HwndStyleManager manager = this.Manager;
			if (manager.Dirty && this.CriticalHandle != IntPtr.Zero)
			{
				UnsafeNativeMethods.CriticalSetWindowLong(new HandleRef(this, this.CriticalHandle), -16, (IntPtr)this._styleDoNotUse.Value);
				UnsafeNativeMethods.CriticalSetWindowLong(new HandleRef(this, this.CriticalHandle), -20, (IntPtr)this._styleExDoNotUse.Value);
				UnsafeNativeMethods.SetWindowPos(new HandleRef(this, this.CriticalHandle), NativeMethods.NullHandleRef, 0, 0, 0, 0, 55);
				manager.Dirty = false;
			}
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x00036606 File Offset: 0x00034806
		private void ClearRootVisual()
		{
			if (this._swh != null)
			{
				this._swh.ClearRootVisual();
			}
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x0003661B File Offset: 0x0003481B
		private NativeMethods.POINT GetPointRelativeToWindow(int x, int y)
		{
			return this._swh.GetPointRelativeToWindow(x, y, base.FlowDirection);
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x00036630 File Offset: 0x00034830
		private Size GetHwndNonClientAreaSizeInMeasureUnits()
		{
			if (!this.AllowsTransparency)
			{
				return this._swh.GetHwndNonClientAreaSizeInMeasureUnits();
			}
			return new Size(0.0, 0.0);
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x00036660 File Offset: 0x00034860
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void ClearSourceWindow()
		{
			if (this._swh != null)
			{
				try
				{
					this._swh.RemoveDisposedHandler(new EventHandler(this.OnSourceWindowDisposed));
				}
				finally
				{
					HwndSource hwndSourceWindow = this._swh.HwndSourceWindow;
					this._swh = null;
					if (hwndSourceWindow != null)
					{
						hwndSourceWindow.SizeToContentChanged -= this.OnSourceSizeToContentChanged;
					}
				}
			}
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x000366C8 File Offset: 0x000348C8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void ClearHiddenWindowIfAny()
		{
			if (this._hiddenWindow != null && this._hiddenWindow.Handle == this._ownerHandle)
			{
				this.SetOwnerHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x000366F5 File Offset: 0x000348F5
		private void VerifyConsistencyWithAllowsTransparency()
		{
			if (this.AllowsTransparency)
			{
				this.VerifyConsistencyWithAllowsTransparency(this.WindowStyle);
			}
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x0003670B File Offset: 0x0003490B
		private void VerifyConsistencyWithAllowsTransparency(WindowStyle style)
		{
			if (this.AllowsTransparency && style != WindowStyle.None)
			{
				throw new InvalidOperationException(SR.Get("MustUseWindowStyleNone"));
			}
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x00036728 File Offset: 0x00034928
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void VerifyConsistencyWithShowActivated()
		{
			if (!this._inTrustedSubWindow && this.WindowState == WindowState.Maximized && !this.ShowActivated)
			{
				throw new InvalidOperationException(SR.Get("ShowNonActivatedAndMaximized"));
			}
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x00036753 File Offset: 0x00034953
		private static bool IsValidSizeToContent(SizeToContent value)
		{
			return value == SizeToContent.Manual || value == SizeToContent.Width || value == SizeToContent.Height || value == SizeToContent.WidthAndHeight;
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x00036753 File Offset: 0x00034953
		private static bool IsValidResizeMode(ResizeMode value)
		{
			return value == ResizeMode.NoResize || value == ResizeMode.CanMinimize || value == ResizeMode.CanResize || value == ResizeMode.CanResizeWithGrip;
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x00036766 File Offset: 0x00034966
		private static bool IsValidWindowStartupLocation(WindowStartupLocation value)
		{
			return value == WindowStartupLocation.CenterOwner || value == WindowStartupLocation.CenterScreen || value == WindowStartupLocation.Manual;
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x00036766 File Offset: 0x00034966
		private static bool IsValidWindowState(WindowState value)
		{
			return value == WindowState.Maximized || value == WindowState.Minimized || value == WindowState.Normal;
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x00036753 File Offset: 0x00034953
		private static bool IsValidWindowStyle(WindowStyle value)
		{
			return value == WindowStyle.None || value == WindowStyle.SingleBorderWindow || value == WindowStyle.ThreeDBorderWindow || value == WindowStyle.ToolWindow;
		}

		/// <summary>Called when the <see cref="E:System.Windows.UIElement.ManipulationBoundaryFeedback" /> event occurs.</summary>
		/// <param name="e">The data for the event. </param>
		// Token: 0x06000E28 RID: 3624 RVA: 0x00036778 File Offset: 0x00034978
		protected override void OnManipulationBoundaryFeedback(ManipulationBoundaryFeedbackEventArgs e)
		{
			base.OnManipulationBoundaryFeedback(e);
			if (!PresentationSource.UnderSamePresentationSource(new DependencyObject[]
			{
				e.OriginalSource as DependencyObject,
				this
			}))
			{
				return;
			}
			if (!e.Handled)
			{
				if ((this._currentPanningTarget == null || !this._currentPanningTarget.IsAlive || this._currentPanningTarget.Target != e.OriginalSource) && this._swh != null)
				{
					NativeMethods.RECT windowBounds = this.WindowBounds;
					this._prePanningLocation = this.DeviceToLogicalUnits(new Point((double)windowBounds.left, (double)windowBounds.top));
				}
				ManipulationDelta boundaryFeedback = e.BoundaryFeedback;
				this.UpdatePanningFeedback(boundaryFeedback.Translation, e.OriginalSource);
				e.CompensateForBoundaryFeedback = new Func<Point, Point>(this.CompensateForPanningFeedback);
			}
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x00036838 File Offset: 0x00034A38
		private static void OnStaticManipulationInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
		{
			Window window = sender as Window;
			if (window != null)
			{
				window.EndPanningFeedback(true);
			}
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x00036858 File Offset: 0x00034A58
		private static void OnStaticManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
		{
			Window window = sender as Window;
			if (window != null)
			{
				window.EndPanningFeedback(false);
			}
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x00036878 File Offset: 0x00034A78
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void UpdatePanningFeedback(Vector totalOverpanOffset, object originalSource)
		{
			if (this._currentPanningTarget != null && !this._currentPanningTarget.IsAlive)
			{
				this._currentPanningTarget = null;
				this.EndPanningFeedback(false);
			}
			if (this._swh != null)
			{
				if (this._currentPanningTarget == null)
				{
					this._currentPanningTarget = new WeakReference(originalSource);
				}
				if (originalSource == this._currentPanningTarget.Target)
				{
					this._swh.UpdatePanningFeedback(totalOverpanOffset, false);
				}
			}
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x000368DF File Offset: 0x00034ADF
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void EndPanningFeedback(bool animateBack)
		{
			if (this._swh != null)
			{
				this._swh.EndPanningFeedback(animateBack);
			}
			this._currentPanningTarget = null;
			this._prePanningLocation = new Point(double.NaN, double.NaN);
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x0003691C File Offset: 0x00034B1C
		private Point CompensateForPanningFeedback(Point point)
		{
			if (!double.IsNaN(this._prePanningLocation.X) && !double.IsNaN(this._prePanningLocation.Y) && this._swh != null)
			{
				NativeMethods.RECT windowBounds = this.WindowBounds;
				Point point2 = this.DeviceToLogicalUnits(new Point((double)windowBounds.left, (double)windowBounds.top));
				return new Point(point.X - (this._prePanningLocation.X - point2.X), point.Y - (this._prePanningLocation.Y - point2.Y));
			}
			return point;
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06000E2E RID: 3630 RVA: 0x000369B3 File Offset: 0x00034BB3
		// (set) Token: 0x06000E2F RID: 3631 RVA: 0x000369C0 File Offset: 0x00034BC0
		private SizeToContent HwndSourceSizeToContent
		{
			get
			{
				return this._swh.HwndSourceSizeToContent;
			}
			set
			{
				this._swh.HwndSourceSizeToContent = value;
			}
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06000E30 RID: 3632 RVA: 0x000369CE File Offset: 0x00034BCE
		private NativeMethods.RECT WindowBounds
		{
			get
			{
				return this._swh.WindowBounds;
			}
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06000E31 RID: 3633 RVA: 0x000369DB File Offset: 0x00034BDB
		private int StyleFromHwnd
		{
			get
			{
				if (this._swh == null)
				{
					return 0;
				}
				return this._swh.StyleFromHwnd;
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06000E32 RID: 3634 RVA: 0x000369F2 File Offset: 0x00034BF2
		private int StyleExFromHwnd
		{
			get
			{
				if (this._swh == null)
				{
					return 0;
				}
				return this._swh.StyleExFromHwnd;
			}
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06000E33 RID: 3635 RVA: 0x00036A09 File Offset: 0x00034C09
		private WindowCollection OwnedWindowsInternal
		{
			get
			{
				if (this._ownedWindows == null)
				{
					this._ownedWindows = new WindowCollection();
				}
				return this._ownedWindows;
			}
		}

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06000E34 RID: 3636 RVA: 0x00036A24 File Offset: 0x00034C24
		private Application App
		{
			get
			{
				return Application.Current;
			}
		}

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06000E35 RID: 3637 RVA: 0x00036A2B File Offset: 0x00034C2B
		private bool IsInsideApp
		{
			get
			{
				return Application.Current != null;
			}
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06000E36 RID: 3638 RVA: 0x00036A35 File Offset: 0x00034C35
		private EventHandlerList Events
		{
			get
			{
				if (this._events == null)
				{
					this._events = new EventHandlerList();
				}
				return this._events;
			}
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06000E37 RID: 3639 RVA: 0x00036A50 File Offset: 0x00034C50
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return Window._dType;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Window.TaskbarItemInfo" /> dependency property.</summary>
		/// <returns>The <see cref="P:System.Windows.Window.TaskbarItemInfo" /> dependency property.</returns>
		// Token: 0x04000B3C RID: 2876
		public static readonly DependencyProperty TaskbarItemInfoProperty = DependencyProperty.Register("TaskbarItemInfo", typeof(TaskbarItemInfo), typeof(Window), new PropertyMetadata(null, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Window)d).OnTaskbarItemInfoChanged(e);
		}, new CoerceValueCallback(Window.VerifyAccessCoercion)));

		/// <summary>Identifies the <see cref="P:System.Windows.Window.AllowsTransparency" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Window.AllowsTransparency" /> dependency property.</returns>
		// Token: 0x04000B3E RID: 2878
		public static readonly DependencyProperty AllowsTransparencyProperty = DependencyProperty.Register("AllowsTransparency", typeof(bool), typeof(Window), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(Window.OnAllowsTransparencyChanged), new CoerceValueCallback(Window.CoerceAllowsTransparency)));

		/// <summary>Identifies the <see cref="P:System.Windows.Window.Title" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Window.Title" /> dependency property.</returns>
		// Token: 0x04000B3F RID: 2879
		public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(Window), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(Window._OnTitleChanged)), new ValidateValueCallback(Window._ValidateText));

		/// <summary>Identifies the <see cref="P:System.Windows.Window.Icon" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Window.Icon" /> dependency property.</returns>
		// Token: 0x04000B40 RID: 2880
		public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(Window), new FrameworkPropertyMetadata(new PropertyChangedCallback(Window._OnIconChanged), new CoerceValueCallback(Window.VerifyAccessCoercion)));

		/// <summary>Identifies the <see cref="P:System.Windows.Window.SizeToContent" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Window.SizeToContent" /> dependency property.</returns>
		// Token: 0x04000B41 RID: 2881
		public static readonly DependencyProperty SizeToContentProperty = DependencyProperty.Register("SizeToContent", typeof(SizeToContent), typeof(Window), new FrameworkPropertyMetadata(SizeToContent.Manual, new PropertyChangedCallback(Window._OnSizeToContentChanged)), new ValidateValueCallback(Window._ValidateSizeToContentCallback));

		/// <summary>Identifies the <see cref="P:System.Windows.Window.Top" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Window.Top" /> dependency property.</returns>
		// Token: 0x04000B42 RID: 2882
		public static readonly DependencyProperty TopProperty = Canvas.TopProperty.AddOwner(typeof(Window), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(Window._OnTopChanged), new CoerceValueCallback(Window.CoerceTop)));

		/// <summary>Identifies the <see cref="P:System.Windows.Window.Left" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Window.Left" /> dependency property.</returns>
		// Token: 0x04000B43 RID: 2883
		public static readonly DependencyProperty LeftProperty = Canvas.LeftProperty.AddOwner(typeof(Window), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(Window._OnLeftChanged), new CoerceValueCallback(Window.CoerceLeft)));

		/// <summary>Identifies the <see cref="P:System.Windows.Window.ShowInTaskbar" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Window.ShowInTaskbar" /> dependency property.</returns>
		// Token: 0x04000B44 RID: 2884
		public static readonly DependencyProperty ShowInTaskbarProperty = DependencyProperty.Register("ShowInTaskbar", typeof(bool), typeof(Window), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, new PropertyChangedCallback(Window._OnShowInTaskbarChanged), new CoerceValueCallback(Window.VerifyAccessCoercion)));

		// Token: 0x04000B45 RID: 2885
		private static readonly DependencyPropertyKey IsActivePropertyKey = DependencyProperty.RegisterReadOnly("IsActive", typeof(bool), typeof(Window), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Window.IsActive" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Window.IsActive" /> dependency property.</returns>
		// Token: 0x04000B46 RID: 2886
		public static readonly DependencyProperty IsActiveProperty = Window.IsActivePropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Window.WindowStyle" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Window.WindowStyle" /> dependency property.</returns>
		// Token: 0x04000B47 RID: 2887
		public static readonly DependencyProperty WindowStyleProperty = DependencyProperty.Register("WindowStyle", typeof(WindowStyle), typeof(Window), new FrameworkPropertyMetadata(WindowStyle.SingleBorderWindow, new PropertyChangedCallback(Window._OnWindowStyleChanged), new CoerceValueCallback(Window.CoerceWindowStyle)), new ValidateValueCallback(Window._ValidateWindowStyleCallback));

		/// <summary>Identifies the <see cref="P:System.Windows.Window.WindowState" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Window.WindowState" /> dependency property.</returns>
		// Token: 0x04000B48 RID: 2888
		public static readonly DependencyProperty WindowStateProperty = DependencyProperty.Register("WindowState", typeof(WindowState), typeof(Window), new FrameworkPropertyMetadata(WindowState.Normal, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Window._OnWindowStateChanged), new CoerceValueCallback(Window.VerifyAccessCoercion)), new ValidateValueCallback(Window._ValidateWindowStateCallback));

		/// <summary>Identifies the <see cref="P:System.Windows.Window.ResizeMode" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Window.ResizeMode" /> dependency property.</returns>
		// Token: 0x04000B49 RID: 2889
		public static readonly DependencyProperty ResizeModeProperty = DependencyProperty.Register("ResizeMode", typeof(ResizeMode), typeof(Window), new FrameworkPropertyMetadata(ResizeMode.CanResize, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(Window._OnResizeModeChanged), new CoerceValueCallback(Window.VerifyAccessCoercion)), new ValidateValueCallback(Window._ValidateResizeModeCallback));

		/// <summary>Identifies the <see cref="P:System.Windows.Window.Topmost" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Window.Topmost" /> dependency property.</returns>
		// Token: 0x04000B4A RID: 2890
		public static readonly DependencyProperty TopmostProperty = DependencyProperty.Register("Topmost", typeof(bool), typeof(Window), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(Window._OnTopmostChanged), new CoerceValueCallback(Window.VerifyAccessCoercion)));

		/// <summary>Identifies the <see cref="P:System.Windows.Window.ShowActivated" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Window.ShowActivated" /> dependency property.</returns>
		// Token: 0x04000B4B RID: 2891
		public static readonly DependencyProperty ShowActivatedProperty = DependencyProperty.Register("ShowActivated", typeof(bool), typeof(Window), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, null, new CoerceValueCallback(Window.VerifyAccessCoercion)));

		// Token: 0x04000B4C RID: 2892
		internal static readonly RoutedCommand DialogCancelCommand = new RoutedCommand("DialogCancel", typeof(Window));

		// Token: 0x04000B4D RID: 2893
		private Window.SourceWindowHelper _swh;

		// Token: 0x04000B4E RID: 2894
		private Window _ownerWindow;

		// Token: 0x04000B4F RID: 2895
		[SecurityCritical]
		private IntPtr _ownerHandle = IntPtr.Zero;

		// Token: 0x04000B50 RID: 2896
		private WindowCollection _ownedWindows;

		// Token: 0x04000B51 RID: 2897
		private ArrayList _threadWindowHandles;

		// Token: 0x04000B52 RID: 2898
		private bool _updateHwndSize = true;

		// Token: 0x04000B53 RID: 2899
		private bool _updateHwndLocation = true;

		// Token: 0x04000B54 RID: 2900
		private bool _updateStartupLocation;

		// Token: 0x04000B55 RID: 2901
		private bool _isVisible;

		// Token: 0x04000B56 RID: 2902
		private bool _isVisibilitySet;

		// Token: 0x04000B57 RID: 2903
		private bool _resetKeyboardCuesProperty;

		// Token: 0x04000B58 RID: 2904
		private bool _previousKeyboardCuesProperty;

		// Token: 0x04000B59 RID: 2905
		private static bool _dialogCommandAdded;

		// Token: 0x04000B5A RID: 2906
		private bool _postContentRenderedFromLoadedHandler;

		// Token: 0x04000B5B RID: 2907
		private bool _disposed;

		// Token: 0x04000B5C RID: 2908
		private bool _appShuttingDown;

		// Token: 0x04000B5D RID: 2909
		private bool _ignoreCancel;

		// Token: 0x04000B5E RID: 2910
		private bool _showingAsDialog;

		// Token: 0x04000B5F RID: 2911
		private bool _isClosing;

		// Token: 0x04000B60 RID: 2912
		private bool _visibilitySetInternally;

		// Token: 0x04000B61 RID: 2913
		private bool _hwndCreatedButNotShown;

		// Token: 0x04000B62 RID: 2914
		private double _trackMinWidthDeviceUnits;

		// Token: 0x04000B63 RID: 2915
		private double _trackMinHeightDeviceUnits;

		// Token: 0x04000B64 RID: 2916
		private double _trackMaxWidthDeviceUnits = double.PositiveInfinity;

		// Token: 0x04000B65 RID: 2917
		private double _trackMaxHeightDeviceUnits = double.PositiveInfinity;

		// Token: 0x04000B66 RID: 2918
		private double _windowMaxWidthDeviceUnits = double.PositiveInfinity;

		// Token: 0x04000B67 RID: 2919
		private double _windowMaxHeightDeviceUnits = double.PositiveInfinity;

		// Token: 0x04000B68 RID: 2920
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private double _actualTop = double.NaN;

		// Token: 0x04000B69 RID: 2921
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private double _actualLeft = double.NaN;

		// Token: 0x04000B6A RID: 2922
		[SecurityCritical]
		private bool _inTrustedSubWindow;

		// Token: 0x04000B6B RID: 2923
		private ImageSource _icon;

		// Token: 0x04000B6C RID: 2924
		private NativeMethods.IconHandle _defaultLargeIconHandle;

		// Token: 0x04000B6D RID: 2925
		private NativeMethods.IconHandle _defaultSmallIconHandle;

		// Token: 0x04000B6E RID: 2926
		private NativeMethods.IconHandle _currentLargeIconHandle;

		// Token: 0x04000B6F RID: 2927
		private NativeMethods.IconHandle _currentSmallIconHandle;

		// Token: 0x04000B70 RID: 2928
		private bool? _dialogResult;

		// Token: 0x04000B71 RID: 2929
		[SecurityCritical]
		private IntPtr _dialogOwnerHandle = IntPtr.Zero;

		// Token: 0x04000B72 RID: 2930
		[SecurityCritical]
		private IntPtr _dialogPreviousActiveHandle;

		// Token: 0x04000B73 RID: 2931
		private DispatcherFrame _dispatcherFrame;

		// Token: 0x04000B74 RID: 2932
		private WindowStartupLocation _windowStartupLocation;

		// Token: 0x04000B75 RID: 2933
		private WindowState _previousWindowState;

		// Token: 0x04000B76 RID: 2934
		[SecurityCritical]
		private HwndWrapper _hiddenWindow;

		// Token: 0x04000B77 RID: 2935
		private EventHandlerList _events;

		// Token: 0x04000B78 RID: 2936
		private SecurityCriticalDataForSet<int> _styleDoNotUse;

		// Token: 0x04000B79 RID: 2937
		private SecurityCriticalDataForSet<int> _styleExDoNotUse;

		// Token: 0x04000B7A RID: 2938
		private Window.HwndStyleManager _manager;

		// Token: 0x04000B7B RID: 2939
		private Control _resizeGripControl;

		// Token: 0x04000B7C RID: 2940
		private Point _prePanningLocation = new Point(double.NaN, double.NaN);

		// Token: 0x04000B7D RID: 2941
		private static readonly object EVENT_SOURCEINITIALIZED = new object();

		// Token: 0x04000B7E RID: 2942
		private static readonly object EVENT_CLOSING = new object();

		// Token: 0x04000B7F RID: 2943
		private static readonly object EVENT_CLOSED = new object();

		// Token: 0x04000B80 RID: 2944
		private static readonly object EVENT_ACTIVATED = new object();

		// Token: 0x04000B81 RID: 2945
		private static readonly object EVENT_DEACTIVATED = new object();

		// Token: 0x04000B82 RID: 2946
		private static readonly object EVENT_STATECHANGED = new object();

		// Token: 0x04000B83 RID: 2947
		private static readonly object EVENT_LOCATIONCHANGED = new object();

		// Token: 0x04000B84 RID: 2948
		private static readonly object EVENT_CONTENTRENDERED = new object();

		// Token: 0x04000B85 RID: 2949
		private static readonly object EVENT_VISUALCHILDRENCHANGED = new object();

		// Token: 0x04000B86 RID: 2950
		[SecurityCritical]
		private static readonly WindowMessage WM_TASKBARBUTTONCREATED;

		// Token: 0x04000B87 RID: 2951
		[SecurityCritical]
		private static readonly WindowMessage WM_APPLYTASKBARITEMINFO;

		// Token: 0x04000B88 RID: 2952
		private const int c_MaximumThumbButtons = 7;

		// Token: 0x04000B89 RID: 2953
		[SecurityCritical]
		private ITaskbarList3 _taskbarList;

		// Token: 0x04000B8A RID: 2954
		[SecurityCritical]
		private DispatcherTimer _taskbarRetryTimer;

		// Token: 0x04000B8B RID: 2955
		private Size _overlaySize;

		// Token: 0x04000B8C RID: 2956
		internal static readonly DependencyProperty IWindowServiceProperty = DependencyProperty.RegisterAttached("IWindowService", typeof(IWindowService), typeof(Window), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.OverridesInheritanceBehavior));

		// Token: 0x04000B8D RID: 2957
		private DispatcherOperation _contentRenderedCallback;

		// Token: 0x04000B8E RID: 2958
		private WeakReference _currentPanningTarget;

		// Token: 0x04000B8F RID: 2959
		private static DependencyObjectType _dType;

		// Token: 0x02000835 RID: 2101
		internal struct WindowMinMax
		{
			// Token: 0x06007EBE RID: 32446 RVA: 0x00236C01 File Offset: 0x00234E01
			internal WindowMinMax(double minSize, double maxSize)
			{
				this.minWidth = minSize;
				this.maxWidth = maxSize;
				this.minHeight = minSize;
				this.maxHeight = maxSize;
			}

			// Token: 0x04003CC2 RID: 15554
			internal double minWidth;

			// Token: 0x04003CC3 RID: 15555
			internal double maxWidth;

			// Token: 0x04003CC4 RID: 15556
			internal double minHeight;

			// Token: 0x04003CC5 RID: 15557
			internal double maxHeight;
		}

		// Token: 0x02000836 RID: 2102
		internal class SourceWindowHelper
		{
			// Token: 0x06007EBF RID: 32447 RVA: 0x00236C1F File Offset: 0x00234E1F
			[SecurityCritical]
			internal SourceWindowHelper(HwndSource sourceWindow)
			{
				this._sourceWindow = sourceWindow;
			}

			// Token: 0x17001D75 RID: 7541
			// (get) Token: 0x06007EC0 RID: 32448 RVA: 0x00236C2E File Offset: 0x00234E2E
			internal bool IsSourceWindowNull
			{
				[SecurityCritical]
				[SecurityTreatAsSafe]
				get
				{
					return this._sourceWindow == null;
				}
			}

			// Token: 0x17001D76 RID: 7542
			// (get) Token: 0x06007EC1 RID: 32449 RVA: 0x00236C39 File Offset: 0x00234E39
			internal bool IsCompositionTargetInvalid
			{
				[SecurityCritical]
				[SecurityTreatAsSafe]
				get
				{
					return this.CompositionTarget == null;
				}
			}

			// Token: 0x17001D77 RID: 7543
			// (get) Token: 0x06007EC2 RID: 32450 RVA: 0x00236C44 File Offset: 0x00234E44
			internal IntPtr CriticalHandle
			{
				[SecurityCritical]
				get
				{
					if (this._sourceWindow != null)
					{
						return this._sourceWindow.CriticalHandle;
					}
					return IntPtr.Zero;
				}
			}

			// Token: 0x17001D78 RID: 7544
			// (get) Token: 0x06007EC3 RID: 32451 RVA: 0x00236C60 File Offset: 0x00234E60
			internal NativeMethods.RECT WorkAreaBoundsForNearestMonitor
			{
				[SecurityCritical]
				[SecurityTreatAsSafe]
				get
				{
					NativeMethods.MONITORINFOEX monitorinfoex = new NativeMethods.MONITORINFOEX();
					monitorinfoex.cbSize = Marshal.SizeOf(typeof(NativeMethods.MONITORINFOEX));
					IntPtr intPtr = SafeNativeMethods.MonitorFromWindow(new HandleRef(this, this.CriticalHandle), 2);
					if (intPtr != IntPtr.Zero)
					{
						SafeNativeMethods.GetMonitorInfo(new HandleRef(this, intPtr), monitorinfoex);
					}
					return monitorinfoex.rcWork;
				}
			}

			// Token: 0x17001D79 RID: 7545
			// (get) Token: 0x06007EC4 RID: 32452 RVA: 0x00236CBC File Offset: 0x00234EBC
			private NativeMethods.RECT ClientBounds
			{
				[SecurityCritical]
				[SecurityTreatAsSafe]
				get
				{
					NativeMethods.RECT result = new NativeMethods.RECT(0, 0, 0, 0);
					SafeNativeMethods.GetClientRect(new HandleRef(this, this.CriticalHandle), ref result);
					return result;
				}
			}

			// Token: 0x17001D7A RID: 7546
			// (get) Token: 0x06007EC5 RID: 32453 RVA: 0x00236CE8 File Offset: 0x00234EE8
			internal NativeMethods.RECT WindowBounds
			{
				[SecurityCritical]
				[SecurityTreatAsSafe]
				get
				{
					NativeMethods.RECT result = new NativeMethods.RECT(0, 0, 0, 0);
					SafeNativeMethods.GetWindowRect(new HandleRef(this, this.CriticalHandle), ref result);
					return result;
				}
			}

			// Token: 0x06007EC6 RID: 32454 RVA: 0x00236D14 File Offset: 0x00234F14
			[SecurityCritical]
			private NativeMethods.POINT GetWindowScreenLocation(FlowDirection flowDirection)
			{
				NativeMethods.POINT point = new NativeMethods.POINT(0, 0);
				if (flowDirection == FlowDirection.RightToLeft)
				{
					NativeMethods.RECT rect = new NativeMethods.RECT(0, 0, 0, 0);
					SafeNativeMethods.GetClientRect(new HandleRef(this, this.CriticalHandle), ref rect);
					point = new NativeMethods.POINT(rect.right, rect.top);
				}
				UnsafeNativeMethods.ClientToScreen(new HandleRef(this, this._sourceWindow.CriticalHandle), point);
				return point;
			}

			// Token: 0x17001D7B RID: 7547
			// (get) Token: 0x06007EC7 RID: 32455 RVA: 0x00236D75 File Offset: 0x00234F75
			// (set) Token: 0x06007EC8 RID: 32456 RVA: 0x00236D82 File Offset: 0x00234F82
			internal SizeToContent HwndSourceSizeToContent
			{
				[SecurityCritical]
				[SecurityTreatAsSafe]
				get
				{
					return this._sourceWindow.SizeToContent;
				}
				[SecurityCritical]
				[SecurityTreatAsSafe]
				set
				{
					this._sourceWindow.SizeToContent = value;
				}
			}

			// Token: 0x17001D7C RID: 7548
			// (set) Token: 0x06007EC9 RID: 32457 RVA: 0x00236D90 File Offset: 0x00234F90
			internal Visual RootVisual
			{
				[SecurityCritical]
				set
				{
					this._sourceWindow.RootVisual = value;
				}
			}

			// Token: 0x17001D7D RID: 7549
			// (get) Token: 0x06007ECA RID: 32458 RVA: 0x00236D9E File Offset: 0x00234F9E
			internal bool IsActiveWindow
			{
				[SecurityCritical]
				[SecurityTreatAsSafe]
				get
				{
					return this._sourceWindow.CriticalHandle == UnsafeNativeMethods.GetActiveWindow();
				}
			}

			// Token: 0x17001D7E RID: 7550
			// (get) Token: 0x06007ECB RID: 32459 RVA: 0x00236DB5 File Offset: 0x00234FB5
			internal HwndSource HwndSourceWindow
			{
				[SecurityCritical]
				get
				{
					return this._sourceWindow;
				}
			}

			// Token: 0x17001D7F RID: 7551
			// (get) Token: 0x06007ECC RID: 32460 RVA: 0x00236DC0 File Offset: 0x00234FC0
			internal HwndTarget CompositionTarget
			{
				[SecurityCritical]
				get
				{
					if (this._sourceWindow != null)
					{
						HwndTarget compositionTarget = this._sourceWindow.CompositionTarget;
						if (compositionTarget != null && !compositionTarget.IsDisposed)
						{
							return compositionTarget;
						}
					}
					return null;
				}
			}

			// Token: 0x17001D80 RID: 7552
			// (get) Token: 0x06007ECD RID: 32461 RVA: 0x00236DF0 File Offset: 0x00234FF0
			internal Size WindowSize
			{
				get
				{
					NativeMethods.RECT windowBounds = this.WindowBounds;
					return new Size((double)(windowBounds.right - windowBounds.left), (double)(windowBounds.bottom - windowBounds.top));
				}
			}

			// Token: 0x17001D81 RID: 7553
			// (get) Token: 0x06007ECE RID: 32462 RVA: 0x00236E25 File Offset: 0x00235025
			internal int StyleExFromHwnd
			{
				[SecurityCritical]
				[SecurityTreatAsSafe]
				get
				{
					return UnsafeNativeMethods.GetWindowLong(new HandleRef(this, this.CriticalHandle), -20);
				}
			}

			// Token: 0x17001D82 RID: 7554
			// (get) Token: 0x06007ECF RID: 32463 RVA: 0x00236E3A File Offset: 0x0023503A
			internal int StyleFromHwnd
			{
				[SecurityCritical]
				[SecurityTreatAsSafe]
				get
				{
					return UnsafeNativeMethods.GetWindowLong(new HandleRef(this, this.CriticalHandle), -16);
				}
			}

			// Token: 0x06007ED0 RID: 32464 RVA: 0x00236E50 File Offset: 0x00235050
			[SecurityCritical]
			[SecurityTreatAsSafe]
			internal NativeMethods.POINT GetPointRelativeToWindow(int x, int y, FlowDirection flowDirection)
			{
				NativeMethods.POINT windowScreenLocation = this.GetWindowScreenLocation(flowDirection);
				return new NativeMethods.POINT(x - windowScreenLocation.x, y - windowScreenLocation.y);
			}

			// Token: 0x06007ED1 RID: 32465 RVA: 0x00236E7C File Offset: 0x0023507C
			[SecurityCritical]
			[SecurityTreatAsSafe]
			internal Size GetSizeFromHwndInMeasureUnits()
			{
				Point point = new Point(0.0, 0.0);
				NativeMethods.RECT windowBounds = this.WindowBounds;
				point.X = (double)(windowBounds.right - windowBounds.left);
				point.Y = (double)(windowBounds.bottom - windowBounds.top);
				point = this._sourceWindow.CompositionTarget.TransformFromDevice.Transform(point);
				return new Size(point.X, point.Y);
			}

			// Token: 0x06007ED2 RID: 32466 RVA: 0x00236F00 File Offset: 0x00235100
			[SecurityCritical]
			[SecurityTreatAsSafe]
			internal Size GetHwndNonClientAreaSizeInMeasureUnits()
			{
				NativeMethods.RECT clientBounds = this.ClientBounds;
				NativeMethods.RECT windowBounds = this.WindowBounds;
				Point point = new Point((double)(windowBounds.right - windowBounds.left - (clientBounds.right - clientBounds.left)), (double)(windowBounds.bottom - windowBounds.top - (clientBounds.bottom - clientBounds.top)));
				point = this._sourceWindow.CompositionTarget.TransformFromDevice.Transform(point);
				return new Size(Math.Max(0.0, point.X), Math.Max(0.0, point.Y));
			}

			// Token: 0x06007ED3 RID: 32467 RVA: 0x00236FA3 File Offset: 0x002351A3
			[SecurityCritical]
			[SecurityTreatAsSafe]
			internal void ClearRootVisual()
			{
				if (this._sourceWindow.RootVisual != null)
				{
					this._sourceWindow.RootVisual = null;
				}
			}

			// Token: 0x06007ED4 RID: 32468 RVA: 0x00236FBE File Offset: 0x002351BE
			[SecurityCritical]
			internal void AddDisposedHandler(EventHandler theHandler)
			{
				if (this._sourceWindow != null)
				{
					this._sourceWindow.Disposed += theHandler;
				}
			}

			// Token: 0x06007ED5 RID: 32469 RVA: 0x00236FD4 File Offset: 0x002351D4
			[SecurityCritical]
			internal void RemoveDisposedHandler(EventHandler theHandler)
			{
				if (this._sourceWindow != null)
				{
					this._sourceWindow.Disposed -= theHandler;
				}
			}

			// Token: 0x06007ED6 RID: 32470 RVA: 0x00236FEA File Offset: 0x002351EA
			[SecurityCritical]
			[SecurityTreatAsSafe]
			internal void UpdatePanningFeedback(Vector totalOverpanOffset, bool animate)
			{
				if (this._panningFeedback == null && this._sourceWindow != null)
				{
					this._panningFeedback = new HwndPanningFeedback(this._sourceWindow);
				}
				if (this._panningFeedback != null)
				{
					this._panningFeedback.UpdatePanningFeedback(totalOverpanOffset, animate);
				}
			}

			// Token: 0x06007ED7 RID: 32471 RVA: 0x00237022 File Offset: 0x00235222
			[SecurityCritical]
			[SecurityTreatAsSafe]
			internal void EndPanningFeedback(bool animateBack)
			{
				if (this._panningFeedback != null)
				{
					this._panningFeedback.EndPanningFeedback(animateBack);
					this._panningFeedback = null;
				}
			}

			// Token: 0x04003CC6 RID: 15558
			[SecurityCritical]
			private HwndSource _sourceWindow;

			// Token: 0x04003CC7 RID: 15559
			[SecurityCritical]
			private HwndPanningFeedback _panningFeedback;
		}

		// Token: 0x02000837 RID: 2103
		internal class HwndStyleManager : IDisposable
		{
			// Token: 0x06007ED8 RID: 32472 RVA: 0x0023703F File Offset: 0x0023523F
			internal static Window.HwndStyleManager StartManaging(Window w, int Style, int StyleEx)
			{
				if (w.Manager == null)
				{
					return new Window.HwndStyleManager(w, Style, StyleEx);
				}
				w.Manager._refCount++;
				return w.Manager;
			}

			// Token: 0x06007ED9 RID: 32473 RVA: 0x0023706C File Offset: 0x0023526C
			private HwndStyleManager(Window w, int Style, int StyleEx)
			{
				this._window = w;
				this._window.Manager = this;
				if (!w.IsSourceWindowNull)
				{
					this._window._Style = Style;
					this._window._StyleEx = StyleEx;
					this.Dirty = false;
				}
				this._refCount = 1;
			}

			// Token: 0x06007EDA RID: 32474 RVA: 0x002370C0 File Offset: 0x002352C0
			void IDisposable.Dispose()
			{
				this._refCount--;
				if (this._refCount == 0)
				{
					this._window.Flush();
					if (this._window.Manager == this)
					{
						this._window.Manager = null;
					}
				}
			}

			// Token: 0x17001D83 RID: 7555
			// (get) Token: 0x06007EDB RID: 32475 RVA: 0x002370FD File Offset: 0x002352FD
			// (set) Token: 0x06007EDC RID: 32476 RVA: 0x00237105 File Offset: 0x00235305
			internal bool Dirty
			{
				get
				{
					return this._fDirty;
				}
				set
				{
					this._fDirty = value;
				}
			}

			// Token: 0x04003CC8 RID: 15560
			private Window _window;

			// Token: 0x04003CC9 RID: 15561
			private int _refCount;

			// Token: 0x04003CCA RID: 15562
			private bool _fDirty;
		}

		// Token: 0x02000838 RID: 2104
		private class WindowStartupTopLeftPointHelper
		{
			// Token: 0x17001D84 RID: 7556
			// (get) Token: 0x06007EDD RID: 32477 RVA: 0x0023710E File Offset: 0x0023530E
			internal Point LogicalTopLeft { get; }

			// Token: 0x17001D85 RID: 7557
			// (get) Token: 0x06007EDE RID: 32478 RVA: 0x00237116 File Offset: 0x00235316
			// (set) Token: 0x06007EDF RID: 32479 RVA: 0x0023711E File Offset: 0x0023531E
			internal Point? ScreenTopLeft { get; private set; }

			// Token: 0x06007EE0 RID: 32480 RVA: 0x00237127 File Offset: 0x00235327
			internal WindowStartupTopLeftPointHelper(Point topLeft)
			{
				this.LogicalTopLeft = topLeft;
				if (this.IsHelperNeeded)
				{
					this.IdentifyScreenTopLeft();
				}
			}

			// Token: 0x17001D86 RID: 7558
			// (get) Token: 0x06007EE1 RID: 32481 RVA: 0x00237144 File Offset: 0x00235344
			private bool IsHelperNeeded
			{
				[SecuritySafeCritical]
				get
				{
					if (CoreAppContextSwitches.DoNotUsePresentationDpiCapabilityTier2OrGreater)
					{
						return false;
					}
					if (!HwndTarget.IsPerMonitorDpiScalingEnabled)
					{
						return false;
					}
					if (HwndTarget.IsProcessPerMonitorDpiAware != null)
					{
						return HwndTarget.IsProcessPerMonitorDpiAware.Value;
					}
					return DpiUtil.GetProcessDpiAwareness(IntPtr.Zero) == NativeMethods.PROCESS_DPI_AWARENESS.PROCESS_PER_MONITOR_DPI_AWARE;
				}
			}

			// Token: 0x06007EE2 RID: 32482 RVA: 0x00237190 File Offset: 0x00235390
			[SecuritySafeCritical]
			private void IdentifyScreenTopLeft()
			{
				HandleRef hWnd = new HandleRef(null, IntPtr.Zero);
				IntPtr dc = UnsafeNativeMethods.GetDC(hWnd);
				UnsafeNativeMethods.EnumDisplayMonitors(dc, IntPtr.Zero, new NativeMethods.MonitorEnumProc(this.MonitorEnumProc), IntPtr.Zero);
				UnsafeNativeMethods.ReleaseDC(hWnd, new HandleRef(null, dc));
			}

			// Token: 0x06007EE3 RID: 32483 RVA: 0x002371DC File Offset: 0x002353DC
			[SecurityCritical]
			private bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, ref NativeMethods.RECT lprcMonitor, IntPtr dwData)
			{
				bool result = true;
				uint num;
				uint num2;
				if (UnsafeNativeMethods.GetDpiForMonitor(new HandleRef(null, hMonitor), NativeMethods.MONITOR_DPI_TYPE.MDT_EFFECTIVE_DPI, out num, out num2) == 0U)
				{
					double num3 = num * 1.0 / 96.0;
					double num4 = num2 * 1.0 / 96.0;
					if (new Rect
					{
						X = (double)lprcMonitor.left / num3,
						Y = (double)lprcMonitor.top / num4,
						Width = (double)(lprcMonitor.right - lprcMonitor.left) / num3,
						Height = (double)(lprcMonitor.bottom - lprcMonitor.top) / num4
					}.Contains(this.LogicalTopLeft))
					{
						this.ScreenTopLeft = new Point?(new Point
						{
							X = this.LogicalTopLeft.X * num3,
							Y = this.LogicalTopLeft.Y * num4
						});
						result = false;
					}
				}
				return result;
			}
		}

		// Token: 0x02000839 RID: 2105
		private enum TransformType
		{
			// Token: 0x04003CCE RID: 15566
			WorkAreaToScreenArea,
			// Token: 0x04003CCF RID: 15567
			ScreenAreaToWorkArea
		}

		// Token: 0x0200083A RID: 2106
		private enum BoundsSpecified
		{
			// Token: 0x04003CD1 RID: 15569
			Height,
			// Token: 0x04003CD2 RID: 15570
			Width,
			// Token: 0x04003CD3 RID: 15571
			Top,
			// Token: 0x04003CD4 RID: 15572
			Left
		}
	}
}
