using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Interop;
using MS.Win32;

namespace System.Windows.Interop
{
	/// <summary>Hosts a Win32 window as an element within Windows Presentation Foundation (WPF) content. </summary>
	// Token: 0x020005BE RID: 1470
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public abstract class HwndHost : FrameworkElement, IDisposable, IWin32Window, IKeyboardInputSink
	{
		// Token: 0x060061F5 RID: 25077 RVA: 0x001B7C30 File Offset: 0x001B5E30
		static HwndHost()
		{
			UIElement.FocusableProperty.OverrideMetadata(typeof(HwndHost), new FrameworkPropertyMetadata(true));
			HwndHost.DpiChangedEvent = Window.DpiChangedEvent.AddOwner(typeof(HwndHost));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Interop.HwndHost" /> class. </summary>
		// Token: 0x060061F6 RID: 25078 RVA: 0x001B7C6A File Offset: 0x001B5E6A
		[SecurityCritical]
		protected HwndHost()
		{
			this.Initialize(false);
		}

		// Token: 0x060061F7 RID: 25079 RVA: 0x001B7C79 File Offset: 0x001B5E79
		[SecurityCritical]
		internal HwndHost(bool fTrusted)
		{
			this.Initialize(fTrusted);
		}

		/// <summary>Performs the final cleanup before the garbage collector destroys the object. </summary>
		// Token: 0x060061F8 RID: 25080 RVA: 0x001B7C88 File Offset: 0x001B5E88
		~HwndHost()
		{
			this.Dispose(false);
		}

		/// <summary>Immediately frees any system resources that the object might hold. </summary>
		// Token: 0x060061F9 RID: 25081 RVA: 0x001B7CB8 File Offset: 0x001B5EB8
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Gets the window handle  of the hosted window. </summary>
		/// <returns>The window handle.</returns>
		// Token: 0x1700178C RID: 6028
		// (get) Token: 0x060061FA RID: 25082 RVA: 0x001B7CC7 File Offset: 0x001B5EC7
		public IntPtr Handle
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				return this.CriticalHandle;
			}
		}

		/// <summary>Occurs for each unhandled message that is received by the hosted window. </summary>
		// Token: 0x14000125 RID: 293
		// (add) Token: 0x060061FB RID: 25083 RVA: 0x001B7CD4 File Offset: 0x001B5ED4
		// (remove) Token: 0x060061FC RID: 25084 RVA: 0x001B7CFC File Offset: 0x001B5EFC
		public event HwndSourceHook MessageHook
		{
			[SecuritySafeCritical]
			add
			{
				SecurityHelper.DemandUnmanagedCode();
				if (this._hooks == null)
				{
					this._hooks = new ArrayList(8);
				}
				this._hooks.Add(value);
			}
			[SecuritySafeCritical]
			remove
			{
				SecurityHelper.DemandUnmanagedCode();
				if (this._hooks != null)
				{
					this._hooks.Remove(value);
					if (this._hooks.Count == 0)
					{
						this._hooks = null;
					}
				}
			}
		}

		/// <summary>Occurs after the DPI of the screen on which the HwndHost is displayed changes.</summary>
		// Token: 0x14000126 RID: 294
		// (add) Token: 0x060061FD RID: 25085 RVA: 0x001B7D2B File Offset: 0x001B5F2B
		// (remove) Token: 0x060061FE RID: 25086 RVA: 0x001B7D39 File Offset: 0x001B5F39
		public event DpiChangedEventHandler DpiChanged
		{
			add
			{
				base.AddHandler(HwndHost.DpiChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(HwndHost.DpiChangedEvent, value);
			}
		}

		/// <summary> Called when the hosted window receives a WM_KEYUP message. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x060061FF RID: 25087 RVA: 0x001B7D48 File Offset: 0x001B5F48
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override void OnKeyUp(KeyEventArgs e)
		{
			MSG msg;
			if (this._fTrusted.Value)
			{
				msg = ComponentDispatcher.UnsecureCurrentKeyboardMessage;
			}
			else
			{
				msg = ComponentDispatcher.CurrentKeyboardMessage;
			}
			ModifierKeys systemModifierKeys = HwndKeyboardInputProvider.GetSystemModifierKeys();
			bool flag = ((IKeyboardInputSink)this).TranslateAccelerator(ref msg, systemModifierKeys);
			if (flag)
			{
				e.Handled = flag;
			}
			base.OnKeyUp(e);
		}

		/// <summary>Called when the DPI at which this HwndHost is rendered changes.</summary>
		/// <param name="oldDpi">The original DPI scale setting.</param>
		/// <param name="newDpi">The new DPI scale setting.</param>
		// Token: 0x06006200 RID: 25088 RVA: 0x001B7D91 File Offset: 0x001B5F91
		[SecuritySafeCritical]
		protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
		{
			base.RaiseEvent(new DpiChangedEventArgs(oldDpi, newDpi, HwndHost.DpiChangedEvent, this));
			this.UpdateWindowPos();
		}

		/// <summary>Called when the hosted window receives a WM_KEYDOWN message. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06006201 RID: 25089 RVA: 0x001B7DAC File Offset: 0x001B5FAC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override void OnKeyDown(KeyEventArgs e)
		{
			MSG msg;
			if (this._fTrusted.Value)
			{
				msg = ComponentDispatcher.UnsecureCurrentKeyboardMessage;
			}
			else
			{
				msg = ComponentDispatcher.CurrentKeyboardMessage;
			}
			ModifierKeys systemModifierKeys = HwndKeyboardInputProvider.GetSystemModifierKeys();
			bool flag = ((IKeyboardInputSink)this).TranslateAccelerator(ref msg, systemModifierKeys);
			if (flag)
			{
				e.Handled = flag;
			}
			base.OnKeyDown(e);
		}

		/// <summary>Registers the <see cref="T:System.Windows.Interop.IKeyboardInputSink" /> interface of a contained component.  </summary>
		/// <param name="sink">The <see cref="T:System.Windows.Interop.IKeyboardInputSink" /> sink of the contained component.</param>
		/// <returns>The <see cref="T:System.Windows.Interop.IKeyboardInputSite" /> site of the contained component.</returns>
		// Token: 0x06006202 RID: 25090 RVA: 0x001B7DF5 File Offset: 0x001B5FF5
		[SecurityCritical]
		[UIPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		protected virtual IKeyboardInputSite RegisterKeyboardInputSinkCore(IKeyboardInputSink sink)
		{
			throw new InvalidOperationException(SR.Get("HwndHostDoesNotSupportChildKeyboardSinks"));
		}

		/// <summary>For a description of this member, see <see cref="M:System.Windows.Interop.IKeyboardInputSink.RegisterKeyboardInputSink(System.Windows.Interop.IKeyboardInputSink)" />.</summary>
		/// <param name="sink">The <see cref="T:System.Windows.Interop.IKeyboardInputSink" /> sink of the contained component.</param>
		/// <returns>The <see cref="T:System.Windows.Interop.IKeyboardInputSite" /> site of the contained component.</returns>
		// Token: 0x06006203 RID: 25091 RVA: 0x001B7E06 File Offset: 0x001B6006
		[SecurityCritical]
		IKeyboardInputSite IKeyboardInputSink.RegisterKeyboardInputSink(IKeyboardInputSink sink)
		{
			return this.RegisterKeyboardInputSinkCore(sink);
		}

		/// <summary>Processes keyboard input at the keydown message level.</summary>
		/// <param name="msg">The message and associated data. Do not modify this structure. It is passed by reference for performance reasons only.</param>
		/// <param name="modifiers">Modifier keys.</param>
		/// <returns>Always returns <see langword="false" />.</returns>
		// Token: 0x06006204 RID: 25092 RVA: 0x0000B02A File Offset: 0x0000922A
		[SecurityCritical]
		[UIPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		protected virtual bool TranslateAcceleratorCore(ref MSG msg, ModifierKeys modifiers)
		{
			return false;
		}

		/// <summary>For a description of this member, see <see cref="M:System.Windows.Interop.IKeyboardInputSink.TranslateAccelerator(System.Windows.Interop.MSG@,System.Windows.Input.ModifierKeys)" />.</summary>
		/// <param name="msg">The message and associated data. Do not modify this structure. It is passed by reference for performance reasons only.</param>
		/// <param name="modifiers">Modifier keys.</param>
		/// <returns>
		///     <see langword="true" /> if the message was handled by the method implementation; otherwise, <see langword="false" />.</returns>
		// Token: 0x06006205 RID: 25093 RVA: 0x001B7E0F File Offset: 0x001B600F
		[SecurityCritical]
		bool IKeyboardInputSink.TranslateAccelerator(ref MSG msg, ModifierKeys modifiers)
		{
			return this.TranslateAcceleratorCore(ref msg, modifiers);
		}

		/// <summary>Sets focus on either the first tab stop or the last tab stop of the sink.</summary>
		/// <param name="request">Specifies whether focus should be set to the first or the last tab stop.</param>
		/// <returns>Always returns <see langword="false" />.</returns>
		// Token: 0x06006206 RID: 25094 RVA: 0x0000B02A File Offset: 0x0000922A
		protected virtual bool TabIntoCore(TraversalRequest request)
		{
			return false;
		}

		/// <summary>For a description of this member, see <see cref="M:System.Windows.Interop.IKeyboardInputSink.TabInto(System.Windows.Input.TraversalRequest)" />.</summary>
		/// <param name="request">Specifies whether focus should be set to the first or the last tab stop.</param>
		/// <returns>
		///     <see langword="true" /> if the focus has been set as requested; <see langword="false" />, if there are no tab stops.</returns>
		// Token: 0x06006207 RID: 25095 RVA: 0x001B7E19 File Offset: 0x001B6019
		bool IKeyboardInputSink.TabInto(TraversalRequest request)
		{
			return this.TabIntoCore(request);
		}

		/// <summary>For a description of this member, see <see cref="P:System.Windows.Interop.IKeyboardInputSink.KeyboardInputSite" />.</summary>
		/// <returns>A reference to the container's <see cref="T:System.Windows.Interop.IKeyboardInputSite" /> interface.</returns>
		// Token: 0x1700178D RID: 6029
		// (get) Token: 0x06006208 RID: 25096 RVA: 0x001B7E22 File Offset: 0x001B6022
		// (set) Token: 0x06006209 RID: 25097 RVA: 0x001B7E2A File Offset: 0x001B602A
		IKeyboardInputSite IKeyboardInputSink.KeyboardInputSite { get; [SecurityCritical] set; }

		/// <summary>Called when one of the mnemonics (access keys) for this sink is invoked.</summary>
		/// <param name="msg">The message for the mnemonic and associated data.</param>
		/// <param name="modifiers">Modifier keys.</param>
		/// <returns>Always returns <see langword="false" />.</returns>
		// Token: 0x0600620A RID: 25098 RVA: 0x0000B02A File Offset: 0x0000922A
		[SecurityCritical]
		[UIPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		protected virtual bool OnMnemonicCore(ref MSG msg, ModifierKeys modifiers)
		{
			return false;
		}

		/// <summary>For a description of this member, see <see cref="M:System.Windows.Interop.IKeyboardInputSink.OnMnemonic(System.Windows.Interop.MSG@,System.Windows.Input.ModifierKeys)" />.</summary>
		/// <param name="msg">The message for the mnemonic and associated data. Do not modify this message structure. It is passed by reference for performance reasons only.</param>
		/// <param name="modifiers">Modifier keys.</param>
		/// <returns>
		///     <see langword="true" /> if the message was handled; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600620B RID: 25099 RVA: 0x001B7E33 File Offset: 0x001B6033
		[SecurityCritical]
		bool IKeyboardInputSink.OnMnemonic(ref MSG msg, ModifierKeys modifiers)
		{
			return this.OnMnemonicCore(ref msg, modifiers);
		}

		/// <summary>Processes WM_CHAR, WM_SYSCHAR, WM_DEADCHAR, and WM_SYSDEADCHAR input messages before the <see cref="M:System.Windows.Interop.IKeyboardInputSink.OnMnemonic(System.Windows.Interop.MSG@,System.Windows.Input.ModifierKeys)" /> method is called.</summary>
		/// <param name="msg">The message and associated data. Do not modify this structure. It is passed by reference for performance reasons only.</param>
		/// <param name="modifiers">Modifier keys.</param>
		/// <returns>Always returns <see langword="false" />.</returns>
		// Token: 0x0600620C RID: 25100 RVA: 0x0000B02A File Offset: 0x0000922A
		[SecurityCritical]
		[UIPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		protected virtual bool TranslateCharCore(ref MSG msg, ModifierKeys modifiers)
		{
			return false;
		}

		/// <summary>For a description of this member, see <see cref="M:System.Windows.Interop.IKeyboardInputSink.TranslateChar(System.Windows.Interop.MSG@,System.Windows.Input.ModifierKeys)" />.</summary>
		/// <param name="msg">The message and associated data. Do not modify this structure. It is passed by reference for performance reasons only.</param>
		/// <param name="modifiers">Modifier keys.</param>
		/// <returns>
		///     <see langword="true" /> if the message was processed and <see cref="M:System.Windows.Interop.IKeyboardInputSink.OnMnemonic(System.Windows.Interop.MSG@,System.Windows.Input.ModifierKeys)" /> should not be called; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600620D RID: 25101 RVA: 0x001B7E3D File Offset: 0x001B603D
		[SecurityCritical]
		bool IKeyboardInputSink.TranslateChar(ref MSG msg, ModifierKeys modifiers)
		{
			return this.TranslateCharCore(ref msg, modifiers);
		}

		/// <summary>Gets a value that indicates whether the sink or one of its contained components has focus.</summary>
		/// <returns>
		///     <see langword="true" /> if the sink or one of its contained components has focus; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600620E RID: 25102 RVA: 0x001B7E48 File Offset: 0x001B6048
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected virtual bool HasFocusWithinCore()
		{
			HandleRef hwnd = new HandleRef(this, UnsafeNativeMethods.GetFocus());
			return this._hwnd.Handle != IntPtr.Zero && (hwnd.Handle == this._hwnd.Handle || UnsafeNativeMethods.IsChild(this._hwnd, hwnd));
		}

		/// <summary>For a description of this member, see <see cref="M:System.Windows.Media.FamilyTypefaceCollection.System#Collections#IList#Remove(System.Object)" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the sink or one of its contained components has focus; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600620F RID: 25103 RVA: 0x001B7EA3 File Offset: 0x001B60A3
		bool IKeyboardInputSink.HasFocusWithin()
		{
			return this.HasFocusWithinCore();
		}

		/// <summary> Updates the child window's size, visibility, and position to reflect the current state of the element. </summary>
		// Token: 0x06006210 RID: 25104 RVA: 0x001B7EAC File Offset: 0x001B60AC
		[SecurityCritical]
		public void UpdateWindowPos()
		{
			if (this._isDisposed)
			{
				return;
			}
			PresentationSource presentationSource = null;
			CompositionTarget compositionTarget = null;
			if (this.CriticalHandle != IntPtr.Zero && base.IsVisible)
			{
				presentationSource = PresentationSource.CriticalFromVisual(this, false);
				if (presentationSource != null)
				{
					compositionTarget = presentationSource.CompositionTarget;
				}
			}
			if (compositionTarget != null && compositionTarget.RootVisual != null)
			{
				NativeMethods.RECT rc = this.CalculateAssignedRC(presentationSource);
				Rect rcBoundingBox = PointUtil.ToRect(rc);
				this.OnWindowPositionChanged(rcBoundingBox);
				UnsafeNativeMethods.ShowWindowAsync(this._hwnd, 5);
				return;
			}
			UnsafeNativeMethods.ShowWindowAsync(this._hwnd, 0);
		}

		// Token: 0x06006211 RID: 25105 RVA: 0x001B7F30 File Offset: 0x001B6130
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private NativeMethods.RECT CalculateAssignedRC(PresentationSource source)
		{
			Rect rectElement = new Rect(base.RenderSize);
			Rect rectRoot = PointUtil.ElementToRoot(rectElement, this, source);
			Rect rect = PointUtil.RootToClient(rectRoot, source);
			IntPtr parent = UnsafeNativeMethods.GetParent(this._hwnd);
			NativeMethods.RECT rc = PointUtil.FromRect(rect);
			NativeMethods.RECT rect2 = PointUtil.AdjustForRightToLeft(rc, new HandleRef(null, parent));
			if (!CoreAppContextSwitches.DoNotUsePresentationDpiCapabilityTier2OrGreater)
			{
				rect2 = this.AdjustRectForDpi(rect2);
			}
			return rect2;
		}

		// Token: 0x1700178E RID: 6030
		// (get) Token: 0x06006212 RID: 25106 RVA: 0x001B7F94 File Offset: 0x001B6194
		private double DpiParentToChildRatio
		{
			[SecuritySafeCritical]
			get
			{
				if (!this._hasDpiAwarenessContextTransition)
				{
					return 1.0;
				}
				DpiScale2 windowDpi = DpiUtil.GetWindowDpi(this._hwnd.Handle, false);
				DpiScale2 windowDpi2 = DpiUtil.GetWindowDpi(UnsafeNativeMethods.GetParent(this._hwnd), false);
				if (windowDpi == null || windowDpi2 == null)
				{
					return 1.0;
				}
				return windowDpi2.DpiScaleX / windowDpi.DpiScaleX;
			}
		}

		// Token: 0x06006213 RID: 25107 RVA: 0x001B8000 File Offset: 0x001B6200
		private NativeMethods.RECT AdjustRectForDpi(NativeMethods.RECT rcRect)
		{
			if (this._hasDpiAwarenessContextTransition)
			{
				double dpiParentToChildRatio = this.DpiParentToChildRatio;
				rcRect.left = (int)((double)rcRect.left / dpiParentToChildRatio);
				rcRect.top = (int)((double)rcRect.top / dpiParentToChildRatio);
				rcRect.right = (int)((double)rcRect.right / dpiParentToChildRatio);
				rcRect.bottom = (int)((double)rcRect.bottom / dpiParentToChildRatio);
			}
			return rcRect;
		}

		/// <summary>Immediately frees any system resources that the object might hold. </summary>
		/// <param name="disposing">Set to <see langword="true" /> if called from an explicit disposer and <see langword="false" /> otherwise.</param>
		// Token: 0x06006214 RID: 25108 RVA: 0x001B8064 File Offset: 0x001B6264
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected virtual void Dispose(bool disposing)
		{
			if (this._isDisposed)
			{
				return;
			}
			if (disposing)
			{
				base.VerifyAccess();
				if (this._hwndSubclass != null)
				{
					if (this._fTrusted.Value)
					{
						this._hwndSubclass.CriticalDetach(false);
					}
					else
					{
						this._hwndSubclass.RequestDetach(false);
					}
					this._hwndSubclass = null;
				}
				this._hooks = null;
				PresentationSource.RemoveSourceChangedHandler(this, new SourceChangedEventHandler(this.OnSourceChanged));
			}
			if (this._weakEventDispatcherShutdown != null)
			{
				this._weakEventDispatcherShutdown.Dispose();
				this._weakEventDispatcherShutdown = null;
			}
			this.DestroyWindow();
			this._isDisposed = true;
		}

		// Token: 0x06006215 RID: 25109 RVA: 0x001B80FA File Offset: 0x001B62FA
		private void OnDispatcherShutdown(object sender, EventArgs e)
		{
			this.Dispose();
		}

		/// <summary>When overridden in a derived class, creates the window to be hosted. </summary>
		/// <param name="hwndParent">The window handle of the parent window.</param>
		/// <returns>The handle to the child Win32 window to create.</returns>
		// Token: 0x06006216 RID: 25110
		protected abstract HandleRef BuildWindowCore(HandleRef hwndParent);

		/// <summary>When overridden in a derived class, destroys the hosted window. </summary>
		/// <param name="hwnd">A structure that contains the window handle.</param>
		// Token: 0x06006217 RID: 25111
		protected abstract void DestroyWindowCore(HandleRef hwnd);

		/// <summary>When overridden in a derived class, accesses the window process (handle) of the hosted child window. </summary>
		/// <param name="hwnd">The window handle of the hosted window.</param>
		/// <param name="msg">The message to act upon.</param>
		/// <param name="wParam">Information that may be relevant to handling the message. This is typically used to store small pieces of information, such as flags.</param>
		/// <param name="lParam">Information that may be relevant to handling the message. This is typically used to reference an object.</param>
		/// <param name="handled">Whether events resulting should be marked handled.</param>
		/// <returns>The window handle of the child window.</returns>
		// Token: 0x06006218 RID: 25112 RVA: 0x001B8104 File Offset: 0x001B6304
		[SecurityCritical]
		protected unsafe virtual IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			this.DemandIfUntrusted();
			if (msg != 61)
			{
				if (msg != 70)
				{
					if (msg == 130)
					{
						this._hwnd = new HandleRef(null, IntPtr.Zero);
					}
				}
				else
				{
					PresentationSource presentationSource = PresentationSource.CriticalFromVisual(this, false);
					if (presentationSource != null)
					{
						NativeMethods.RECT rect = this.CalculateAssignedRC(presentationSource);
						NativeMethods.WINDOWPOS* ptr = (NativeMethods.WINDOWPOS*)((void*)lParam);
						ptr->cx = rect.right - rect.left;
						ptr->cy = rect.bottom - rect.top;
						ptr->flags &= -2;
						ptr->x = rect.left;
						ptr->y = rect.top;
						ptr->flags &= -3;
						ptr->flags |= 256;
					}
				}
				return IntPtr.Zero;
			}
			handled = true;
			return this.OnWmGetObject(wParam, lParam);
		}

		/// <summary>Creates an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> for <see cref="T:System.Windows.Interop.HwndHost" /> . </summary>
		/// <returns>The type-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> implementation. </returns>
		// Token: 0x06006219 RID: 25113 RVA: 0x001B81DF File Offset: 0x001B63DF
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new HwndHostAutomationPeer(this);
		}

		// Token: 0x0600621A RID: 25114 RVA: 0x001B81E8 File Offset: 0x001B63E8
		[SecurityCritical]
		private IntPtr OnWmGetObject(IntPtr wparam, IntPtr lparam)
		{
			IntPtr result = IntPtr.Zero;
			AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(this);
			if (automationPeer != null)
			{
				IRawElementProviderSimple interopChild = automationPeer.GetInteropChild();
				result = AutomationInteropProvider.ReturnRawElementProvider(this.CriticalHandle, wparam, lparam, interopChild);
			}
			return result;
		}

		/// <summary> Called when the hosted window's position changes. </summary>
		/// <param name="rcBoundingBox">The window's position.</param>
		// Token: 0x0600621B RID: 25115 RVA: 0x001B821C File Offset: 0x001B641C
		[SecurityCritical]
		protected virtual void OnWindowPositionChanged(Rect rcBoundingBox)
		{
			if (this._isDisposed)
			{
				return;
			}
			UnsafeNativeMethods.SetWindowPos(this._hwnd, new HandleRef(null, IntPtr.Zero), (int)rcBoundingBox.X, (int)rcBoundingBox.Y, (int)rcBoundingBox.Width, (int)rcBoundingBox.Height, 16660);
		}

		/// <summary>Returns the size of the window represented by the <see cref="T:System.Windows.Interop.HwndHost" /> object, as requested by layout engine operations. </summary>
		/// <param name="constraint">The size of the <see cref="T:System.Windows.Interop.HwndHost" /> object.</param>
		/// <returns>The size of the <see cref="T:System.Windows.Interop.HwndHost" /> object.</returns>
		// Token: 0x0600621C RID: 25116 RVA: 0x001B8270 File Offset: 0x001B6470
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override Size MeasureOverride(Size constraint)
		{
			this.DemandIfUntrusted();
			Size result = new Size(0.0, 0.0);
			if (this.CriticalHandle != IntPtr.Zero)
			{
				result.Width = Math.Min(this._desiredSize.Width, constraint.Width);
				result.Height = Math.Min(this._desiredSize.Height, constraint.Height);
			}
			return result;
		}

		// Token: 0x0600621D RID: 25117 RVA: 0x001B82EB File Offset: 0x001B64EB
		internal override DrawingGroup GetDrawing()
		{
			return this.GetDrawingHelper();
		}

		// Token: 0x0600621E RID: 25118 RVA: 0x001B82F3 File Offset: 0x001B64F3
		internal override Rect GetContentBounds()
		{
			return new Rect(base.RenderSize);
		}

		// Token: 0x0600621F RID: 25119 RVA: 0x001B8300 File Offset: 0x001B6500
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private DrawingGroup GetDrawingHelper()
		{
			SecurityHelper.DemandUIWindowPermission();
			DrawingGroup drawingGroup = null;
			if (this._hwnd.Handle != IntPtr.Zero && UnsafeNativeMethods.IsWindow(this._hwnd))
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				SafeNativeMethods.GetWindowRect(this._hwnd, ref rect);
				int num = rect.right - rect.left;
				int num2 = rect.bottom - rect.top;
				HandleRef hDC = new HandleRef(this, UnsafeNativeMethods.GetDC(new HandleRef(this, IntPtr.Zero)));
				if (hDC.Handle != IntPtr.Zero)
				{
					HandleRef handleRef = new HandleRef(this, IntPtr.Zero);
					HandleRef hObject = new HandleRef(this, IntPtr.Zero);
					try
					{
						handleRef = new HandleRef(this, UnsafeNativeMethods.CriticalCreateCompatibleDC(hDC));
						if (handleRef.Handle != IntPtr.Zero)
						{
							hObject = new HandleRef(this, UnsafeNativeMethods.CriticalCreateCompatibleBitmap(hDC, num, num2));
							if (hObject.Handle != IntPtr.Zero)
							{
								IntPtr obj = UnsafeNativeMethods.CriticalSelectObject(handleRef, hObject.Handle);
								try
								{
									NativeMethods.RECT rect2 = new NativeMethods.RECT(0, 0, num, num2);
									IntPtr brush = UnsafeNativeMethods.CriticalGetStockObject(0);
									UnsafeNativeMethods.CriticalFillRect(handleRef.Handle, ref rect2, brush);
									if (!UnsafeNativeMethods.CriticalPrintWindow(this._hwnd, handleRef, 0))
									{
										UnsafeNativeMethods.SendMessage(this._hwnd.Handle, WindowMessage.WM_PRINT, handleRef.Handle, (IntPtr)30);
									}
									else
									{
										UnsafeNativeMethods.CriticalRedrawWindow(this._hwnd, IntPtr.Zero, IntPtr.Zero, 129);
									}
									drawingGroup = new DrawingGroup();
									BitmapSource imageSource = Imaging.CriticalCreateBitmapSourceFromHBitmap(hObject.Handle, IntPtr.Zero, Int32Rect.Empty, null, WICBitmapAlphaChannelOption.WICBitmapIgnoreAlpha);
									Rect rect3 = new Rect(base.RenderSize);
									drawingGroup.Children.Add(new ImageDrawing(imageSource, rect3));
									drawingGroup.Freeze();
								}
								finally
								{
									UnsafeNativeMethods.CriticalSelectObject(handleRef, obj);
								}
							}
						}
					}
					finally
					{
						UnsafeNativeMethods.ReleaseDC(new HandleRef(this, IntPtr.Zero), hDC);
						hDC = new HandleRef(null, IntPtr.Zero);
						if (hObject.Handle != IntPtr.Zero)
						{
							UnsafeNativeMethods.DeleteObject(hObject);
							hObject = new HandleRef(this, IntPtr.Zero);
						}
						if (handleRef.Handle != IntPtr.Zero)
						{
							UnsafeNativeMethods.CriticalDeleteDC(handleRef);
							handleRef = new HandleRef(this, IntPtr.Zero);
						}
					}
				}
			}
			return drawingGroup;
		}

		// Token: 0x06006220 RID: 25120 RVA: 0x001B8588 File Offset: 0x001B6788
		[SecurityCritical]
		private void Initialize(bool fTrusted)
		{
			this._fTrusted = new SecurityCriticalDataForSet<bool>(fTrusted);
			this._hwndSubclassHook = new HwndWrapperHook(this.SubclassWndProc);
			this._handlerLayoutUpdated = new EventHandler(this.OnLayoutUpdated);
			this._handlerEnabledChanged = new DependencyPropertyChangedEventHandler(this.OnEnabledChanged);
			this._handlerVisibleChanged = new DependencyPropertyChangedEventHandler(this.OnVisibleChanged);
			PresentationSource.AddSourceChangedHandler(this, new SourceChangedEventHandler(this.OnSourceChanged));
			this._weakEventDispatcherShutdown = new HwndHost.WeakEventDispatcherShutdown(this, base.Dispatcher);
		}

		// Token: 0x06006221 RID: 25121 RVA: 0x001B860D File Offset: 0x001B680D
		[SecurityCritical]
		private void DemandIfUntrusted()
		{
			if (!this._fTrusted.Value)
			{
				SecurityHelper.DemandUnmanagedCode();
			}
		}

		// Token: 0x06006222 RID: 25122 RVA: 0x001B8624 File Offset: 0x001B6824
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void OnSourceChanged(object sender, SourceChangedEventArgs e)
		{
			IKeyboardInputSite keyboardInputSite = ((IKeyboardInputSink)this).KeyboardInputSite;
			if (keyboardInputSite != null)
			{
				if (this._fTrusted.Value)
				{
					new UIPermission(PermissionState.Unrestricted).Assert();
				}
				try
				{
					((IKeyboardInputSink)this).KeyboardInputSite = null;
					keyboardInputSite.Unregister();
				}
				finally
				{
					if (this._fTrusted.Value)
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}
			IKeyboardInputSink keyboardInputSink = PresentationSource.CriticalFromVisual(this, false) as IKeyboardInputSink;
			if (keyboardInputSink != null)
			{
				if (this._fTrusted.Value)
				{
					new UIPermission(PermissionState.Unrestricted).Assert();
				}
				try
				{
					((IKeyboardInputSink)this).KeyboardInputSite = keyboardInputSink.RegisterKeyboardInputSink(this);
				}
				finally
				{
					if (this._fTrusted.Value)
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}
			this.BuildOrReparentWindow();
		}

		// Token: 0x06006223 RID: 25123 RVA: 0x001B86E4 File Offset: 0x001B68E4
		private void OnLayoutUpdated(object sender, EventArgs e)
		{
			this.UpdateWindowPos();
		}

		// Token: 0x06006224 RID: 25124 RVA: 0x001B86EC File Offset: 0x001B68EC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void OnEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (this._isDisposed)
			{
				return;
			}
			bool enable = (bool)e.NewValue;
			UnsafeNativeMethods.EnableWindow(this._hwnd, enable);
		}

		// Token: 0x06006225 RID: 25125 RVA: 0x001B871C File Offset: 0x001B691C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void OnVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (this._isDisposed)
			{
				return;
			}
			bool flag = (bool)e.NewValue;
			if (flag)
			{
				UnsafeNativeMethods.ShowWindowAsync(this._hwnd, 8);
				return;
			}
			UnsafeNativeMethods.ShowWindowAsync(this._hwnd, 0);
		}

		// Token: 0x06006226 RID: 25126 RVA: 0x001B8760 File Offset: 0x001B6960
		[SecuritySafeCritical]
		private void BuildOrReparentWindow()
		{
			this.DemandIfUntrusted();
			if (this._isBuildingWindow || this._isDisposed)
			{
				return;
			}
			this._isBuildingWindow = true;
			IntPtr intPtr = IntPtr.Zero;
			PresentationSource presentationSource = PresentationSource.CriticalFromVisual(this, false);
			if (presentationSource != null)
			{
				HwndSource hwndSource = presentationSource as HwndSource;
				if (hwndSource != null)
				{
					intPtr = hwndSource.CriticalHandle;
				}
			}
			else
			{
				PresentationSource presentationSource2 = PresentationSource.CriticalFromVisual(this, true);
				if (presentationSource2 != null && TraceHwndHost.IsEnabled)
				{
					TraceHwndHost.Trace(TraceEventType.Warning, TraceHwndHost.HwndHostIn3D);
				}
			}
			try
			{
				if (intPtr != IntPtr.Zero)
				{
					if (this._hwnd.Handle == IntPtr.Zero)
					{
						this.BuildWindow(new HandleRef(null, intPtr));
						base.LayoutUpdated += this._handlerLayoutUpdated;
						base.IsEnabledChanged += this._handlerEnabledChanged;
						base.IsVisibleChanged += this._handlerVisibleChanged;
					}
					else if (intPtr != UnsafeNativeMethods.GetParent(this._hwnd))
					{
						UnsafeNativeMethods.SetParent(this._hwnd, new HandleRef(null, intPtr));
					}
				}
				else
				{
					IntPtr value = (!FrameworkAppContextSwitches.DisableDevDiv1035544) ? this.Handle : this._hwnd.Handle;
					if (value != IntPtr.Zero)
					{
						HwndWrapper dpiAwarenessCompatibleNotificationWindow = SystemResources.GetDpiAwarenessCompatibleNotificationWindow(this._hwnd);
						if (dpiAwarenessCompatibleNotificationWindow != null)
						{
							UnsafeNativeMethods.SetParent(this._hwnd, new HandleRef(null, dpiAwarenessCompatibleNotificationWindow.Handle));
							SystemResources.DelayHwndShutdown();
						}
						else
						{
							Trace.WriteLineIf(dpiAwarenessCompatibleNotificationWindow == null, string.Format("- Warning - Notification Window is null\n{0}", new StackTrace(true).ToString()));
						}
					}
				}
			}
			finally
			{
				this._isBuildingWindow = false;
			}
		}

		// Token: 0x06006227 RID: 25127 RVA: 0x001B88F8 File Offset: 0x001B6AF8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void BuildWindow(HandleRef hwndParent)
		{
			this.DemandIfUntrusted();
			this._hwnd = this.BuildWindowCore(hwndParent);
			if (this._hwnd.Handle == IntPtr.Zero || !UnsafeNativeMethods.IsWindow(this._hwnd))
			{
				throw new InvalidOperationException(SR.Get("ChildWindowNotCreated"));
			}
			int windowLong = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, this._hwnd.Handle), -16);
			if ((windowLong & 1073741824) == 0)
			{
				throw new InvalidOperationException(SR.Get("HostedWindowMustBeAChildWindow"));
			}
			if (hwndParent.Handle != UnsafeNativeMethods.GetParent(this._hwnd))
			{
				throw new InvalidOperationException(SR.Get("ChildWindowMustHaveCorrectParent"));
			}
			if (DpiUtil.GetDpiAwarenessContext(this._hwnd.Handle) != DpiUtil.GetDpiAwarenessContext(hwndParent.Handle))
			{
				this._hasDpiAwarenessContextTransition = true;
			}
			int num;
			int windowThreadProcessId = UnsafeNativeMethods.GetWindowThreadProcessId(this._hwnd, out num);
			if (windowThreadProcessId == SafeNativeMethods.GetCurrentThreadId() && num == SafeNativeMethods.GetCurrentProcessId())
			{
				this._hwndSubclass = new HwndSubclass(this._hwndSubclassHook);
				this._hwndSubclass.CriticalAttach(this._hwnd.Handle);
			}
			UnsafeNativeMethods.ShowWindowAsync(this._hwnd, 0);
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			SafeNativeMethods.GetWindowRect(this._hwnd, ref rect);
			PresentationSource presentationSource = PresentationSource.CriticalFromVisual(this, false);
			Point point = new Point((double)rect.left, (double)rect.top);
			Point point2 = new Point((double)rect.right, (double)rect.bottom);
			point = presentationSource.CompositionTarget.TransformFromDevice.Transform(point);
			point2 = presentationSource.CompositionTarget.TransformFromDevice.Transform(point2);
			this._desiredSize = new Size(point2.X - point.X, point2.Y - point.Y);
			base.InvalidateMeasure();
		}

		// Token: 0x06006228 RID: 25128 RVA: 0x001B8AC8 File Offset: 0x001B6CC8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void DestroyWindow()
		{
			if (this.CriticalHandle == IntPtr.Zero)
			{
				return;
			}
			if (!base.CheckAccess())
			{
				base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.AsyncDestroyWindow), null);
				return;
			}
			HandleRef hwnd = this._hwnd;
			this._hwnd = new HandleRef(null, IntPtr.Zero);
			this.DestroyWindowCore(hwnd);
		}

		// Token: 0x06006229 RID: 25129 RVA: 0x001B8B2B File Offset: 0x001B6D2B
		private object AsyncDestroyWindow(object arg)
		{
			this.DestroyWindow();
			return null;
		}

		// Token: 0x1700178F RID: 6031
		// (get) Token: 0x0600622A RID: 25130 RVA: 0x001B8B34 File Offset: 0x001B6D34
		internal IntPtr CriticalHandle
		{
			[SecurityCritical]
			get
			{
				if (this._hwnd.Handle != IntPtr.Zero && !UnsafeNativeMethods.IsWindow(this._hwnd))
				{
					this._hwnd = new HandleRef(null, IntPtr.Zero);
				}
				return this._hwnd.Handle;
			}
		}

		// Token: 0x0600622B RID: 25131 RVA: 0x001B8B84 File Offset: 0x001B6D84
		[SecurityCritical]
		private IntPtr SubclassWndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			IntPtr result = IntPtr.Zero;
			result = this.WndProc(hwnd, msg, wParam, lParam, ref handled);
			if (!handled && this._hooks != null)
			{
				int i = 0;
				int count = this._hooks.Count;
				while (i < count)
				{
					result = ((HwndSourceHook)this._hooks[i])(hwnd, msg, wParam, lParam, ref handled);
					if (handled)
					{
						break;
					}
					i++;
				}
			}
			return result;
		}

		// Token: 0x0400317F RID: 12671
		private DependencyPropertyChangedEventHandler _handlerEnabledChanged;

		// Token: 0x04003180 RID: 12672
		private DependencyPropertyChangedEventHandler _handlerVisibleChanged;

		// Token: 0x04003181 RID: 12673
		private EventHandler _handlerLayoutUpdated;

		// Token: 0x04003182 RID: 12674
		[SecurityCritical]
		private HwndSubclass _hwndSubclass;

		// Token: 0x04003183 RID: 12675
		[SecurityCritical]
		private HwndWrapperHook _hwndSubclassHook;

		// Token: 0x04003184 RID: 12676
		[SecurityCritical]
		private HandleRef _hwnd;

		// Token: 0x04003185 RID: 12677
		private ArrayList _hooks;

		// Token: 0x04003186 RID: 12678
		private Size _desiredSize;

		// Token: 0x04003187 RID: 12679
		private bool _hasDpiAwarenessContextTransition;

		// Token: 0x04003188 RID: 12680
		private SecurityCriticalDataForSet<bool> _fTrusted;

		// Token: 0x04003189 RID: 12681
		private bool _isBuildingWindow;

		// Token: 0x0400318A RID: 12682
		private bool _isDisposed;

		// Token: 0x0400318B RID: 12683
		private HwndHost.WeakEventDispatcherShutdown _weakEventDispatcherShutdown;

		// Token: 0x020009FD RID: 2557
		private class WeakEventDispatcherShutdown : WeakReference
		{
			// Token: 0x060089E0 RID: 35296 RVA: 0x00256889 File Offset: 0x00254A89
			public WeakEventDispatcherShutdown(HwndHost hwndHost, Dispatcher that) : base(hwndHost)
			{
				this._that = that;
				this._that.ShutdownFinished += this.OnShutdownFinished;
			}

			// Token: 0x060089E1 RID: 35297 RVA: 0x002568B0 File Offset: 0x00254AB0
			public void OnShutdownFinished(object sender, EventArgs e)
			{
				HwndHost hwndHost = this.Target as HwndHost;
				if (hwndHost != null)
				{
					hwndHost.OnDispatcherShutdown(sender, e);
					return;
				}
				this.Dispose();
			}

			// Token: 0x060089E2 RID: 35298 RVA: 0x002568DB File Offset: 0x00254ADB
			public void Dispose()
			{
				if (this._that != null)
				{
					this._that.ShutdownFinished -= this.OnShutdownFinished;
				}
			}

			// Token: 0x040046AA RID: 18090
			private Dispatcher _that;
		}
	}
}
