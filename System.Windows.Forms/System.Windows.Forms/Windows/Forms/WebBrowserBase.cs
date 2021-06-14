using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Windows.Forms
{
	/// <summary>Provides a wrapper for a generic ActiveX control for use as a base class by the <see cref="T:System.Windows.Forms.WebBrowser" /> control.</summary>
	// Token: 0x02000427 RID: 1063
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultProperty("Name")]
	[DefaultEvent("Enter")]
	[Designer("System.Windows.Forms.Design.AxDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class WebBrowserBase : Control
	{
		// Token: 0x060049DE RID: 18910 RVA: 0x00133954 File Offset: 0x00131B54
		internal WebBrowserBase(string clsidString)
		{
			if (Application.OleRequired() != ApartmentState.STA)
			{
				throw new ThreadStateException(SR.GetString("AXMTAThread", new object[]
				{
					clsidString
				}));
			}
			base.SetStyle(ControlStyles.UserPaint, false);
			this.clsid = new Guid(clsidString);
			this.webBrowserBaseChangingSize.Width = -1;
			this.SetAXHostState(WebBrowserHelper.isMaskEdit, this.clsid.Equals(WebBrowserHelper.maskEdit_Clsid));
		}

		/// <summary>Gets the underlying ActiveX <see langword="WebBrowser" /> control.</summary>
		/// <returns>An <see cref="T:System.Object" /> representing the underlying ActiveX <see langword="WebBrowser" /> control.</returns>
		// Token: 0x17001229 RID: 4649
		// (get) Token: 0x060049DF RID: 18911 RVA: 0x001339D9 File Offset: 0x00131BD9
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object ActiveXInstance
		{
			get
			{
				return this.activeXInstance;
			}
		}

		/// <summary>Returns a reference to the unmanaged ActiveX control site.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.WebBrowserSiteBase" /> that represents the site of the underlying ActiveX control.</returns>
		// Token: 0x060049E0 RID: 18912 RVA: 0x001339E1 File Offset: 0x00131BE1
		protected virtual WebBrowserSiteBase CreateWebBrowserSiteBase()
		{
			return new WebBrowserSiteBase(this);
		}

		/// <summary>Called by the control when the underlying ActiveX control is created.</summary>
		/// <param name="nativeActiveXObject">An object that represents the underlying ActiveX control.</param>
		// Token: 0x060049E1 RID: 18913 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void AttachInterfaces(object nativeActiveXObject)
		{
		}

		/// <summary>Called by the control when the underlying ActiveX control is discarded.</summary>
		// Token: 0x060049E2 RID: 18914 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void DetachInterfaces()
		{
		}

		/// <summary>Called by the control to prepare it for listening to events. </summary>
		// Token: 0x060049E3 RID: 18915 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void CreateSink()
		{
		}

		/// <summary>Called by the control when it stops listening to events.</summary>
		// Token: 0x060049E4 RID: 18916 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void DetachSink()
		{
		}

		/// <summary>This method is not supported by this control.</summary>
		/// <param name="bitmap">A <see cref="T:System.Drawing.Bitmap" />.</param>
		/// <param name="targetBounds">A <see cref="T:System.Drawing.Rectangle" />. </param>
		// Token: 0x060049E5 RID: 18917 RVA: 0x00012A9D File Offset: 0x00010C9D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new void DrawToBitmap(Bitmap bitmap, Rectangle targetBounds)
		{
			base.DrawToBitmap(bitmap, targetBounds);
		}

		/// <summary>Gets or sets the site of the control.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.ISite" /> associated with the <see cref="T:System.Windows.Forms.Control" />, if any.</returns>
		// Token: 0x1700122A RID: 4650
		// (set) Token: 0x060049E6 RID: 18918 RVA: 0x001339EC File Offset: 0x00131BEC
		public override ISite Site
		{
			set
			{
				bool flag = this.RemoveSelectionHandler();
				base.Site = value;
				if (flag)
				{
					this.AddSelectionHandler();
				}
			}
		}

		// Token: 0x060049E7 RID: 18919 RVA: 0x00133A10 File Offset: 0x00131C10
		internal override void OnBoundsUpdate(int x, int y, int width, int height)
		{
			if (this.ActiveXState >= WebBrowserHelper.AXState.InPlaceActive)
			{
				try
				{
					this.webBrowserBaseChangingSize.Width = width;
					this.webBrowserBaseChangingSize.Height = height;
					this.AXInPlaceObject.SetObjectRects(new NativeMethods.COMRECT(new Rectangle(0, 0, width, height)), WebBrowserHelper.GetClipRect());
				}
				finally
				{
					this.webBrowserBaseChangingSize.Width = -1;
				}
			}
			base.OnBoundsUpdate(x, y, width, height);
		}

		/// <summary>Processes a dialog key if the WebBrowser ActiveX control does not process it. </summary>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process. </param>
		/// <returns>
		///     <see langword="true" /> if the key was processed by the <see cref="T:System.Windows.Forms.WebBrowserBase" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060049E8 RID: 18920 RVA: 0x00133A88 File Offset: 0x00131C88
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			return !this.ignoreDialogKeys && base.ProcessDialogKey(keyData);
		}

		/// <summary>Preprocesses keyboard or input messages within the message loop before they are dispatched.</summary>
		/// <param name="msg">A <see cref="T:System.Windows.Forms.Message" />, passed by reference, that represents the message to process. The possible values are WM_KEYDOWN, WM_SYSKEYDOWN, WM_CHAR, and WM_SYSCHAR.</param>
		/// <returns>
		///     <see langword="true" /> if the message was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x060049E9 RID: 18921 RVA: 0x00133A9C File Offset: 0x00131C9C
		public override bool PreProcessMessage(ref Message msg)
		{
			if (this.IsUserMode)
			{
				if (this.GetAXHostState(WebBrowserHelper.siteProcessedInputKey))
				{
					return base.PreProcessMessage(ref msg);
				}
				NativeMethods.MSG msg2 = default(NativeMethods.MSG);
				msg2.message = msg.Msg;
				msg2.wParam = msg.WParam;
				msg2.lParam = msg.LParam;
				msg2.hwnd = msg.HWnd;
				this.SetAXHostState(WebBrowserHelper.siteProcessedInputKey, false);
				try
				{
					if (this.axOleInPlaceObject != null)
					{
						int num = this.axOleInPlaceActiveObject.TranslateAccelerator(ref msg2);
						if (num == 0)
						{
							return true;
						}
						msg.Msg = msg2.message;
						msg.WParam = msg2.wParam;
						msg.LParam = msg2.lParam;
						msg.HWnd = msg2.hwnd;
						if (num == 1)
						{
							bool result = false;
							this.ignoreDialogKeys = true;
							try
							{
								result = base.PreProcessMessage(ref msg);
							}
							finally
							{
								this.ignoreDialogKeys = false;
							}
							return result;
						}
						if (this.GetAXHostState(WebBrowserHelper.siteProcessedInputKey))
						{
							return base.PreProcessMessage(ref msg);
						}
						return false;
					}
				}
				finally
				{
					this.SetAXHostState(WebBrowserHelper.siteProcessedInputKey, false);
				}
				return false;
			}
			return false;
		}

		/// <summary>Processes a mnemonic character.</summary>
		/// <param name="charCode">The character to process.</param>
		/// <returns>
		///     <see langword="true" /> if the character was processed as a mnemonic by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x060049EA RID: 18922 RVA: 0x00133BD0 File Offset: 0x00131DD0
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			bool result = false;
			if (base.CanSelect)
			{
				try
				{
					NativeMethods.tagCONTROLINFO tagCONTROLINFO = new NativeMethods.tagCONTROLINFO();
					int controlInfo = this.axOleControl.GetControlInfo(tagCONTROLINFO);
					if (NativeMethods.Succeeded(controlInfo))
					{
						NativeMethods.MSG msg = default(NativeMethods.MSG);
						msg.hwnd = IntPtr.Zero;
						msg.message = 260;
						msg.wParam = (IntPtr)((int)char.ToUpper(charCode, CultureInfo.CurrentCulture));
						msg.lParam = (IntPtr)538443777;
						msg.time = SafeNativeMethods.GetTickCount();
						NativeMethods.POINT point = new NativeMethods.POINT();
						UnsafeNativeMethods.GetCursorPos(point);
						msg.pt_x = point.x;
						msg.pt_y = point.y;
						if (SafeNativeMethods.IsAccelerator(new HandleRef(tagCONTROLINFO, tagCONTROLINFO.hAccel), (int)tagCONTROLINFO.cAccel, ref msg, null))
						{
							this.axOleControl.OnMnemonic(ref msg);
							this.FocusInternal();
							result = true;
						}
					}
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsCriticalException(ex))
					{
						throw;
					}
				}
			}
			return result;
		}

		/// <summary>This member overrides <see cref="M:System.Windows.Forms.Control.WndProc(System.Windows.Forms.Message@)" />.</summary>
		/// <param name="m">The windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
		// Token: 0x060049EB RID: 18923 RVA: 0x00133CDC File Offset: 0x00131EDC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg <= 33)
			{
				if (msg <= 8)
				{
					if (msg != 2)
					{
						if (msg != 8)
						{
							goto IL_182;
						}
						this.hwndFocus = m.WParam;
						try
						{
							base.WndProc(ref m);
							return;
						}
						finally
						{
							this.hwndFocus = IntPtr.Zero;
						}
					}
					IntPtr handle;
					if (this.ActiveXState >= WebBrowserHelper.AXState.InPlaceActive && NativeMethods.Succeeded(this.AXInPlaceObject.GetWindow(out handle)))
					{
						Application.ParkHandle(new HandleRef(this.AXInPlaceObject, handle), DpiAwarenessContext.DPI_AWARENESS_CONTEXT_UNSPECIFIED);
					}
					if (base.RecreatingHandle)
					{
						this.axReloadingState = this.axState;
					}
					this.TransitionDownTo(WebBrowserHelper.AXState.Running);
					if (this.axWindow != null)
					{
						this.axWindow.ReleaseHandle();
					}
					this.OnHandleDestroyed(EventArgs.Empty);
					return;
				}
				if (msg - 20 > 1 && msg != 32)
				{
					if (msg != 33)
					{
						goto IL_182;
					}
					goto IL_C8;
				}
			}
			else if (msg <= 123)
			{
				if (msg != 43)
				{
					if (msg == 83)
					{
						base.WndProc(ref m);
						this.DefWndProc(ref m);
						return;
					}
					if (msg != 123)
					{
						goto IL_182;
					}
				}
			}
			else if (msg != 273)
			{
				switch (msg)
				{
				case 513:
				case 516:
				case 519:
					goto IL_C8;
				case 514:
				case 515:
				case 517:
				case 518:
				case 520:
				case 521:
					break;
				default:
					if (msg != 8277)
					{
						goto IL_182;
					}
					break;
				}
			}
			else
			{
				if (!Control.ReflectMessageInternal(m.LParam, ref m))
				{
					this.DefWndProc(ref m);
					return;
				}
				return;
			}
			this.DefWndProc(ref m);
			return;
			IL_C8:
			if (!base.DesignMode && this.containingControl != null && this.containingControl.ActiveControl != this)
			{
				this.FocusInternal();
			}
			this.DefWndProc(ref m);
			return;
			IL_182:
			if (m.Msg == WebBrowserHelper.REGMSG_MSG)
			{
				m.Result = (IntPtr)123;
				return;
			}
			base.WndProc(ref m);
		}

		/// <summary>This member overrides <see cref="M:System.Windows.Forms.Control.OnParentChanged(System.EventArgs)" />.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		/// <exception cref="T:System.Reflection.TargetInvocationException">Unable to get the window handle for the ActiveX control. Windowless ActiveX controls are not supported.</exception>
		// Token: 0x060049EC RID: 18924 RVA: 0x00133EA0 File Offset: 0x001320A0
		protected override void OnParentChanged(EventArgs e)
		{
			Control parentInternal = this.ParentInternal;
			if ((base.Visible && parentInternal != null && parentInternal.Visible) || base.IsHandleCreated)
			{
				this.TransitionUpTo(WebBrowserHelper.AXState.InPlaceActive);
			}
			base.OnParentChanged(e);
		}

		/// <summary>This member overrides <see cref="M:System.Windows.Forms.Control.OnVisibleChanged(System.EventArgs)" />.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		/// <exception cref="T:System.Reflection.TargetInvocationException">Unable to get the window handle for the ActiveX control. Windowless ActiveX controls are not supported.</exception>
		// Token: 0x060049ED RID: 18925 RVA: 0x00133EDD File Offset: 0x001320DD
		protected override void OnVisibleChanged(EventArgs e)
		{
			if (base.Visible && !base.Disposing && !base.IsDisposed)
			{
				this.TransitionUpTo(WebBrowserHelper.AXState.InPlaceActive);
			}
			base.OnVisibleChanged(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.GotFocus" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060049EE RID: 18926 RVA: 0x00133F05 File Offset: 0x00132105
		protected override void OnGotFocus(EventArgs e)
		{
			if (this.ActiveXState < WebBrowserHelper.AXState.UIActive)
			{
				this.TransitionUpTo(WebBrowserHelper.AXState.UIActive);
			}
			base.OnGotFocus(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.LostFocus" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060049EF RID: 18927 RVA: 0x00133F1E File Offset: 0x0013211E
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			if (!base.ContainsFocus)
			{
				this.TransitionDownTo(WebBrowserHelper.AXState.InPlaceActive);
			}
		}

		/// <summary>This method is not meaningful for this control.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> object.</param>
		// Token: 0x060049F0 RID: 18928 RVA: 0x0000701A File Offset: 0x0000521A
		protected override void OnRightToLeftChanged(EventArgs e)
		{
		}

		// Token: 0x060049F1 RID: 18929 RVA: 0x00133F36 File Offset: 0x00132136
		internal override bool CanSelectCore()
		{
			return this.ActiveXState >= WebBrowserHelper.AXState.InPlaceActive && base.CanSelectCore();
		}

		// Token: 0x060049F2 RID: 18930 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal override bool AllowsKeyboardToolTip()
		{
			return false;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060049F3 RID: 18931 RVA: 0x00133F49 File Offset: 0x00132149
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.AmbientChanged(-703);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ForeColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060049F4 RID: 18932 RVA: 0x00133F5D File Offset: 0x0013215D
		protected override void OnForeColorChanged(EventArgs e)
		{
			base.OnForeColorChanged(e);
			this.AmbientChanged(-704);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.BackColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060049F5 RID: 18933 RVA: 0x00133F71 File Offset: 0x00132171
		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackColorChanged(e);
			this.AmbientChanged(-701);
		}

		// Token: 0x060049F6 RID: 18934 RVA: 0x00133F85 File Offset: 0x00132185
		internal override void RecreateHandleCore()
		{
			if (!this.inRtlRecreate)
			{
				base.RecreateHandleCore();
			}
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" /> and its child controls and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		// Token: 0x060049F7 RID: 18935 RVA: 0x00133F95 File Offset: 0x00132195
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.TransitionDownTo(WebBrowserHelper.AXState.Passive);
			}
			base.Dispose(disposing);
		}

		// Token: 0x1700122B RID: 4651
		// (get) Token: 0x060049F8 RID: 18936 RVA: 0x00133FA8 File Offset: 0x001321A8
		// (set) Token: 0x060049F9 RID: 18937 RVA: 0x00133FB0 File Offset: 0x001321B0
		internal WebBrowserHelper.AXState ActiveXState
		{
			get
			{
				return this.axState;
			}
			set
			{
				this.axState = value;
			}
		}

		// Token: 0x060049FA RID: 18938 RVA: 0x00133FB9 File Offset: 0x001321B9
		internal bool GetAXHostState(int mask)
		{
			return this.axHostState[mask];
		}

		// Token: 0x060049FB RID: 18939 RVA: 0x00133FC7 File Offset: 0x001321C7
		internal void SetAXHostState(int mask, bool value)
		{
			this.axHostState[mask] = value;
		}

		// Token: 0x060049FC RID: 18940 RVA: 0x00029068 File Offset: 0x00027268
		internal IntPtr GetHandleNoCreate()
		{
			if (!base.IsHandleCreated)
			{
				return IntPtr.Zero;
			}
			return base.Handle;
		}

		// Token: 0x060049FD RID: 18941 RVA: 0x00133FD8 File Offset: 0x001321D8
		internal void TransitionUpTo(WebBrowserHelper.AXState state)
		{
			if (!this.GetAXHostState(WebBrowserHelper.inTransition))
			{
				this.SetAXHostState(WebBrowserHelper.inTransition, true);
				try
				{
					while (state > this.ActiveXState)
					{
						switch (this.ActiveXState)
						{
						case WebBrowserHelper.AXState.Passive:
							this.TransitionFromPassiveToLoaded();
							continue;
						case WebBrowserHelper.AXState.Loaded:
							this.TransitionFromLoadedToRunning();
							continue;
						case WebBrowserHelper.AXState.Running:
							this.TransitionFromRunningToInPlaceActive();
							continue;
						case WebBrowserHelper.AXState.InPlaceActive:
							this.TransitionFromInPlaceActiveToUIActive();
							continue;
						}
						WebBrowserHelper.AXState activeXState = this.ActiveXState;
						this.ActiveXState = activeXState + 1;
					}
				}
				finally
				{
					this.SetAXHostState(WebBrowserHelper.inTransition, false);
				}
			}
		}

		// Token: 0x060049FE RID: 18942 RVA: 0x0013407C File Offset: 0x0013227C
		internal void TransitionDownTo(WebBrowserHelper.AXState state)
		{
			if (!this.GetAXHostState(WebBrowserHelper.inTransition))
			{
				this.SetAXHostState(WebBrowserHelper.inTransition, true);
				try
				{
					while (state < this.ActiveXState)
					{
						WebBrowserHelper.AXState activeXState = this.ActiveXState;
						switch (activeXState)
						{
						case WebBrowserHelper.AXState.Loaded:
							this.TransitionFromLoadedToPassive();
							continue;
						case WebBrowserHelper.AXState.Running:
							this.TransitionFromRunningToLoaded();
							continue;
						case (WebBrowserHelper.AXState)3:
							break;
						case WebBrowserHelper.AXState.InPlaceActive:
							this.TransitionFromInPlaceActiveToRunning();
							continue;
						default:
							if (activeXState == WebBrowserHelper.AXState.UIActive)
							{
								this.TransitionFromUIActiveToInPlaceActive();
								continue;
							}
							break;
						}
						activeXState = this.ActiveXState;
						this.ActiveXState = activeXState - 1;
					}
				}
				finally
				{
					this.SetAXHostState(WebBrowserHelper.inTransition, false);
				}
			}
		}

		// Token: 0x060049FF RID: 18943 RVA: 0x00134120 File Offset: 0x00132320
		internal bool DoVerb(int verb)
		{
			int num = this.axOleObject.DoVerb(verb, IntPtr.Zero, this.ActiveXSite, 0, base.Handle, new NativeMethods.COMRECT(base.Bounds));
			return num == 0;
		}

		// Token: 0x1700122C RID: 4652
		// (get) Token: 0x06004A00 RID: 18944 RVA: 0x0013415B File Offset: 0x0013235B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal ContainerControl ContainingControl
		{
			get
			{
				if (this.containingControl == null || this.GetAXHostState(WebBrowserHelper.recomputeContainingControl))
				{
					this.containingControl = this.FindContainerControlInternal();
				}
				return this.containingControl;
			}
		}

		// Token: 0x06004A01 RID: 18945 RVA: 0x00134184 File Offset: 0x00132384
		internal WebBrowserContainer CreateWebBrowserContainer()
		{
			if (this.wbContainer == null)
			{
				this.wbContainer = new WebBrowserContainer(this);
			}
			return this.wbContainer;
		}

		// Token: 0x06004A02 RID: 18946 RVA: 0x001341A0 File Offset: 0x001323A0
		internal WebBrowserContainer GetParentContainer()
		{
			if (this.container == null)
			{
				this.container = WebBrowserContainer.FindContainerForControl(this);
			}
			if (this.container == null)
			{
				this.container = this.CreateWebBrowserContainer();
				this.container.AddControl(this);
			}
			return this.container;
		}

		// Token: 0x06004A03 RID: 18947 RVA: 0x001341DC File Offset: 0x001323DC
		internal void SetEditMode(WebBrowserHelper.AXEditMode em)
		{
			this.axEditMode = em;
		}

		// Token: 0x06004A04 RID: 18948 RVA: 0x001341E8 File Offset: 0x001323E8
		internal void SetSelectionStyle(WebBrowserHelper.SelectionStyle selectionStyle)
		{
			if (base.DesignMode)
			{
				ISelectionService selectionService = WebBrowserHelper.GetSelectionService(this);
				this.selectionStyle = selectionStyle;
				if (selectionService != null && selectionService.GetComponentSelected(this))
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this)["SelectionStyle"];
					if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(int))
					{
						propertyDescriptor.SetValue(this, (int)selectionStyle);
					}
				}
			}
		}

		// Token: 0x06004A05 RID: 18949 RVA: 0x00134250 File Offset: 0x00132450
		internal void AddSelectionHandler()
		{
			if (!this.GetAXHostState(WebBrowserHelper.addedSelectionHandler))
			{
				this.SetAXHostState(WebBrowserHelper.addedSelectionHandler, true);
				ISelectionService selectionService = WebBrowserHelper.GetSelectionService(this);
				if (selectionService != null)
				{
					selectionService.SelectionChanging += this.SelectionChangeHandler;
				}
			}
		}

		// Token: 0x06004A06 RID: 18950 RVA: 0x0013428C File Offset: 0x0013248C
		internal bool RemoveSelectionHandler()
		{
			bool axhostState = this.GetAXHostState(WebBrowserHelper.addedSelectionHandler);
			if (axhostState)
			{
				this.SetAXHostState(WebBrowserHelper.addedSelectionHandler, false);
				ISelectionService selectionService = WebBrowserHelper.GetSelectionService(this);
				if (selectionService != null)
				{
					selectionService.SelectionChanging -= this.SelectionChangeHandler;
				}
			}
			return axhostState;
		}

		// Token: 0x06004A07 RID: 18951 RVA: 0x001342CC File Offset: 0x001324CC
		internal void AttachWindow(IntPtr hwnd)
		{
			UnsafeNativeMethods.SetParent(new HandleRef(null, hwnd), new HandleRef(this, base.Handle));
			if (this.axWindow != null)
			{
				this.axWindow.ReleaseHandle();
			}
			this.axWindow = new WebBrowserBase.WebBrowserBaseNativeWindow(this);
			this.axWindow.AssignHandle(hwnd, false);
			base.UpdateZOrder();
			base.UpdateBounds();
			Size size = base.Size;
			size = this.SetExtent(size.Width, size.Height);
			Point location = base.Location;
			base.Bounds = new Rectangle(location.X, location.Y, size.Width, size.Height);
		}

		// Token: 0x1700122D RID: 4653
		// (get) Token: 0x06004A08 RID: 18952 RVA: 0x00134374 File Offset: 0x00132574
		internal bool IsUserMode
		{
			get
			{
				return this.Site == null || !base.DesignMode;
			}
		}

		// Token: 0x06004A09 RID: 18953 RVA: 0x0013438C File Offset: 0x0013258C
		internal void MakeDirty()
		{
			ISite site = this.Site;
			if (site != null)
			{
				IComponentChangeService componentChangeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					componentChangeService.OnComponentChanging(this, null);
					componentChangeService.OnComponentChanged(this, null, null, null);
				}
			}
		}

		// Token: 0x1700122E RID: 4654
		// (get) Token: 0x06004A0A RID: 18954 RVA: 0x001343CE File Offset: 0x001325CE
		// (set) Token: 0x06004A0B RID: 18955 RVA: 0x001343D6 File Offset: 0x001325D6
		internal int NoComponentChangeEvents
		{
			get
			{
				return this.noComponentChange;
			}
			set
			{
				this.noComponentChange = value;
			}
		}

		// Token: 0x06004A0C RID: 18956 RVA: 0x001343DF File Offset: 0x001325DF
		private void StartEvents()
		{
			if (!this.GetAXHostState(WebBrowserHelper.sinkAttached))
			{
				this.SetAXHostState(WebBrowserHelper.sinkAttached, true);
				this.CreateSink();
			}
			this.ActiveXSite.StartEvents();
		}

		// Token: 0x06004A0D RID: 18957 RVA: 0x0013440B File Offset: 0x0013260B
		private void StopEvents()
		{
			if (this.GetAXHostState(WebBrowserHelper.sinkAttached))
			{
				this.SetAXHostState(WebBrowserHelper.sinkAttached, false);
				this.DetachSink();
			}
			this.ActiveXSite.StopEvents();
		}

		// Token: 0x06004A0E RID: 18958 RVA: 0x00134437 File Offset: 0x00132637
		private void TransitionFromPassiveToLoaded()
		{
			if (this.ActiveXState == WebBrowserHelper.AXState.Passive)
			{
				this.activeXInstance = UnsafeNativeMethods.CoCreateInstance(ref this.clsid, null, 1, ref NativeMethods.ActiveX.IID_IUnknown);
				this.ActiveXState = WebBrowserHelper.AXState.Loaded;
				this.AttachInterfacesInternal();
			}
		}

		// Token: 0x06004A0F RID: 18959 RVA: 0x00134468 File Offset: 0x00132668
		private void TransitionFromLoadedToPassive()
		{
			if (this.ActiveXState == WebBrowserHelper.AXState.Loaded)
			{
				int noComponentChangeEvents = this.NoComponentChangeEvents;
				this.NoComponentChangeEvents = noComponentChangeEvents + 1;
				try
				{
					if (this.activeXInstance != null)
					{
						this.DetachInterfacesInternal();
						Marshal.FinalReleaseComObject(this.activeXInstance);
						this.activeXInstance = null;
					}
				}
				finally
				{
					noComponentChangeEvents = this.NoComponentChangeEvents;
					this.NoComponentChangeEvents = noComponentChangeEvents - 1;
				}
				this.ActiveXState = WebBrowserHelper.AXState.Passive;
			}
		}

		// Token: 0x06004A10 RID: 18960 RVA: 0x001344DC File Offset: 0x001326DC
		private void TransitionFromLoadedToRunning()
		{
			if (this.ActiveXState == WebBrowserHelper.AXState.Loaded)
			{
				int num = 0;
				int miscStatus = this.axOleObject.GetMiscStatus(1, out num);
				if (NativeMethods.Succeeded(miscStatus) && (num & 131072) != 0)
				{
					this.axOleObject.SetClientSite(this.ActiveXSite);
				}
				if (!base.DesignMode)
				{
					this.StartEvents();
				}
				this.ActiveXState = WebBrowserHelper.AXState.Running;
			}
		}

		// Token: 0x06004A11 RID: 18961 RVA: 0x0013453C File Offset: 0x0013273C
		private void TransitionFromRunningToLoaded()
		{
			if (this.ActiveXState == WebBrowserHelper.AXState.Running)
			{
				this.StopEvents();
				WebBrowserContainer parentContainer = this.GetParentContainer();
				if (parentContainer != null)
				{
					parentContainer.RemoveControl(this);
				}
				this.axOleObject.SetClientSite(null);
				this.ActiveXState = WebBrowserHelper.AXState.Loaded;
			}
		}

		// Token: 0x06004A12 RID: 18962 RVA: 0x00134580 File Offset: 0x00132780
		private void TransitionFromRunningToInPlaceActive()
		{
			if (this.ActiveXState == WebBrowserHelper.AXState.Running)
			{
				try
				{
					this.DoVerb(-5);
				}
				catch (Exception inner)
				{
					throw new TargetInvocationException(SR.GetString("AXNohWnd", new object[]
					{
						base.GetType().Name
					}), inner);
				}
				base.CreateControl(true);
				this.ActiveXState = WebBrowserHelper.AXState.InPlaceActive;
			}
		}

		// Token: 0x06004A13 RID: 18963 RVA: 0x001345E8 File Offset: 0x001327E8
		private void TransitionFromInPlaceActiveToRunning()
		{
			if (this.ActiveXState == WebBrowserHelper.AXState.InPlaceActive)
			{
				ContainerControl containerControl = this.ContainingControl;
				if (containerControl != null && containerControl.ActiveControl == this)
				{
					containerControl.SetActiveControlInternal(null);
				}
				this.AXInPlaceObject.InPlaceDeactivate();
				this.ActiveXState = WebBrowserHelper.AXState.Running;
			}
		}

		// Token: 0x06004A14 RID: 18964 RVA: 0x0013462C File Offset: 0x0013282C
		private void TransitionFromInPlaceActiveToUIActive()
		{
			if (this.ActiveXState == WebBrowserHelper.AXState.InPlaceActive)
			{
				try
				{
					this.DoVerb(-4);
				}
				catch (Exception inner)
				{
					throw new TargetInvocationException(SR.GetString("AXNohWnd", new object[]
					{
						base.GetType().Name
					}), inner);
				}
				this.ActiveXState = WebBrowserHelper.AXState.UIActive;
			}
		}

		// Token: 0x06004A15 RID: 18965 RVA: 0x0013468C File Offset: 0x0013288C
		private void TransitionFromUIActiveToInPlaceActive()
		{
			if (this.ActiveXState == WebBrowserHelper.AXState.UIActive)
			{
				int num = this.AXInPlaceObject.UIDeactivate();
				this.ActiveXState = WebBrowserHelper.AXState.InPlaceActive;
			}
		}

		// Token: 0x1700122F RID: 4655
		// (get) Token: 0x06004A16 RID: 18966 RVA: 0x001346B5 File Offset: 0x001328B5
		internal WebBrowserSiteBase ActiveXSite
		{
			get
			{
				if (this.axSite == null)
				{
					this.axSite = this.CreateWebBrowserSiteBase();
				}
				return this.axSite;
			}
		}

		// Token: 0x06004A17 RID: 18967 RVA: 0x001346D4 File Offset: 0x001328D4
		private void AttachInterfacesInternal()
		{
			this.axOleObject = (UnsafeNativeMethods.IOleObject)this.activeXInstance;
			this.axOleInPlaceObject = (UnsafeNativeMethods.IOleInPlaceObject)this.activeXInstance;
			this.axOleInPlaceActiveObject = (UnsafeNativeMethods.IOleInPlaceActiveObject)this.activeXInstance;
			this.axOleControl = (UnsafeNativeMethods.IOleControl)this.activeXInstance;
			this.AttachInterfaces(this.activeXInstance);
		}

		// Token: 0x06004A18 RID: 18968 RVA: 0x00134731 File Offset: 0x00132931
		private void DetachInterfacesInternal()
		{
			this.axOleObject = null;
			this.axOleInPlaceObject = null;
			this.axOleInPlaceActiveObject = null;
			this.axOleControl = null;
			this.DetachInterfaces();
		}

		// Token: 0x17001230 RID: 4656
		// (get) Token: 0x06004A19 RID: 18969 RVA: 0x00134755 File Offset: 0x00132955
		private EventHandler SelectionChangeHandler
		{
			get
			{
				if (this.selectionChangeHandler == null)
				{
					this.selectionChangeHandler = new EventHandler(this.OnNewSelection);
				}
				return this.selectionChangeHandler;
			}
		}

		// Token: 0x06004A1A RID: 18970 RVA: 0x00134778 File Offset: 0x00132978
		private void OnNewSelection(object sender, EventArgs e)
		{
			if (base.DesignMode)
			{
				ISelectionService selectionService = WebBrowserHelper.GetSelectionService(this);
				if (selectionService != null)
				{
					if (!selectionService.GetComponentSelected(this))
					{
						if (this.EditMode)
						{
							this.GetParentContainer().OnExitEditMode(this);
							this.SetEditMode(WebBrowserHelper.AXEditMode.None);
						}
						this.SetSelectionStyle(WebBrowserHelper.SelectionStyle.Selected);
						this.RemoveSelectionHandler();
						return;
					}
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this)["SelectionStyle"];
					if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(int))
					{
						int num = (int)propertyDescriptor.GetValue(this);
						if (num != (int)this.selectionStyle)
						{
							propertyDescriptor.SetValue(this, this.selectionStyle);
						}
					}
				}
			}
		}

		// Token: 0x06004A1B RID: 18971 RVA: 0x00134824 File Offset: 0x00132A24
		private Size SetExtent(int width, int height)
		{
			NativeMethods.tagSIZEL tagSIZEL = new NativeMethods.tagSIZEL();
			tagSIZEL.cx = width;
			tagSIZEL.cy = height;
			bool flag = base.DesignMode;
			try
			{
				this.Pixel2hiMetric(tagSIZEL, tagSIZEL);
				this.axOleObject.SetExtent(1, tagSIZEL);
			}
			catch (COMException)
			{
				flag = true;
			}
			if (flag)
			{
				this.axOleObject.GetExtent(1, tagSIZEL);
				try
				{
					this.axOleObject.SetExtent(1, tagSIZEL);
				}
				catch (COMException ex)
				{
				}
			}
			return this.GetExtent();
		}

		// Token: 0x06004A1C RID: 18972 RVA: 0x001348B0 File Offset: 0x00132AB0
		private Size GetExtent()
		{
			NativeMethods.tagSIZEL tagSIZEL = new NativeMethods.tagSIZEL();
			this.axOleObject.GetExtent(1, tagSIZEL);
			this.HiMetric2Pixel(tagSIZEL, tagSIZEL);
			return new Size(tagSIZEL.cx, tagSIZEL.cy);
		}

		// Token: 0x06004A1D RID: 18973 RVA: 0x001348EC File Offset: 0x00132AEC
		private void HiMetric2Pixel(NativeMethods.tagSIZEL sz, NativeMethods.tagSIZEL szout)
		{
			NativeMethods._POINTL pointl = new NativeMethods._POINTL();
			pointl.x = sz.cx;
			pointl.y = sz.cy;
			NativeMethods.tagPOINTF tagPOINTF = new NativeMethods.tagPOINTF();
			((UnsafeNativeMethods.IOleControlSite)this.ActiveXSite).TransformCoords(pointl, tagPOINTF, 6);
			szout.cx = (int)tagPOINTF.x;
			szout.cy = (int)tagPOINTF.y;
		}

		// Token: 0x06004A1E RID: 18974 RVA: 0x00134948 File Offset: 0x00132B48
		private void Pixel2hiMetric(NativeMethods.tagSIZEL sz, NativeMethods.tagSIZEL szout)
		{
			NativeMethods.tagPOINTF tagPOINTF = new NativeMethods.tagPOINTF();
			tagPOINTF.x = (float)sz.cx;
			tagPOINTF.y = (float)sz.cy;
			NativeMethods._POINTL pointl = new NativeMethods._POINTL();
			((UnsafeNativeMethods.IOleControlSite)this.ActiveXSite).TransformCoords(pointl, tagPOINTF, 10);
			szout.cx = pointl.x;
			szout.cy = pointl.y;
		}

		// Token: 0x17001231 RID: 4657
		// (get) Token: 0x06004A1F RID: 18975 RVA: 0x001349A3 File Offset: 0x00132BA3
		private bool EditMode
		{
			get
			{
				return this.axEditMode > WebBrowserHelper.AXEditMode.None;
			}
		}

		// Token: 0x06004A20 RID: 18976 RVA: 0x001349B0 File Offset: 0x00132BB0
		internal ContainerControl FindContainerControlInternal()
		{
			if (this.Site != null)
			{
				IDesignerHost designerHost = (IDesignerHost)this.Site.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					IComponent rootComponent = designerHost.RootComponent;
					if (rootComponent != null && rootComponent is ContainerControl)
					{
						return (ContainerControl)rootComponent;
					}
				}
			}
			ContainerControl containerControl = null;
			for (Control control = this; control != null; control = control.ParentInternal)
			{
				ContainerControl containerControl2 = control as ContainerControl;
				if (containerControl2 != null)
				{
					containerControl = containerControl2;
				}
			}
			if (containerControl == null)
			{
				containerControl = (Control.FromHandle(UnsafeNativeMethods.GetParent(new HandleRef(this, base.Handle))) as ContainerControl);
			}
			if (containerControl is Application.ParkingWindow)
			{
				containerControl = null;
			}
			this.SetAXHostState(WebBrowserHelper.recomputeContainingControl, containerControl == null);
			return containerControl;
		}

		// Token: 0x06004A21 RID: 18977 RVA: 0x00134A54 File Offset: 0x00132C54
		private void AmbientChanged(int dispid)
		{
			if (this.activeXInstance != null)
			{
				try
				{
					base.Invalidate();
					this.axOleControl.OnAmbientPropertyChange(dispid);
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsCriticalException(ex))
					{
						throw;
					}
				}
			}
		}

		// Token: 0x17001232 RID: 4658
		// (get) Token: 0x06004A22 RID: 18978 RVA: 0x00134A9C File Offset: 0x00132C9C
		internal UnsafeNativeMethods.IOleInPlaceObject AXInPlaceObject
		{
			get
			{
				return this.axOleInPlaceObject;
			}
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Size" /> of the control.</returns>
		// Token: 0x17001233 RID: 4659
		// (get) Token: 0x06004A23 RID: 18979 RVA: 0x00012055 File Offset: 0x00010255
		protected override Size DefaultSize
		{
			get
			{
				return new Size(75, 23);
			}
		}

		/// <summary>Determines if a character is an input character that the control recognizes.</summary>
		/// <param name="charCode">The character to test.</param>
		/// <returns>
		///     <see langword="true" /> if the character should be sent directly to the control and not preprocessed; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004A24 RID: 18980 RVA: 0x0000E214 File Offset: 0x0000C414
		protected override bool IsInputChar(char charCode)
		{
			return true;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		/// <exception cref="T:System.Threading.ThreadStateException">The <see cref="P:System.Threading.Thread.ApartmentState" /> property of the application is not set to <see cref="F:System.Threading.ApartmentState.STA" />. </exception>
		// Token: 0x06004A25 RID: 18981 RVA: 0x00134AA4 File Offset: 0x00132CA4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnHandleCreated(EventArgs e)
		{
			if (Application.OleRequired() != ApartmentState.STA)
			{
				throw new ThreadStateException(SR.GetString("ThreadMustBeSTA"));
			}
			base.OnHandleCreated(e);
			if (this.axReloadingState != WebBrowserHelper.AXState.Passive && this.axReloadingState != this.axState)
			{
				if (this.axState < this.axReloadingState)
				{
					this.TransitionUpTo(this.axReloadingState);
				}
				else
				{
					this.TransitionDownTo(this.axReloadingState);
				}
				this.axReloadingState = WebBrowserHelper.AXState.Passive;
			}
		}

		/// <summary>This property is not meaningful for this control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background of the control.</returns>
		// Token: 0x17001234 RID: 4660
		// (get) Token: 0x06004A26 RID: 18982 RVA: 0x00011FB1 File Offset: 0x000101B1
		// (set) Token: 0x06004A27 RID: 18983 RVA: 0x00011FB9 File Offset: 0x000101B9
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		/// <summary>This property is not meaningful for this control.</summary>
		/// <returns>The value of this property is not meaningful for this control.</returns>
		// Token: 0x17001235 RID: 4661
		// (get) Token: 0x06004A28 RID: 18984 RVA: 0x00012071 File Offset: 0x00010271
		// (set) Token: 0x06004A29 RID: 18985 RVA: 0x00012079 File Offset: 0x00010279
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
			}
		}

		/// <summary>This property is not meaningful for this control.</summary>
		/// <returns>The value of this property is not meaningful for this control.</returns>
		// Token: 0x17001236 RID: 4662
		// (get) Token: 0x06004A2A RID: 18986 RVA: 0x00012082 File Offset: 0x00010282
		// (set) Token: 0x06004A2B RID: 18987 RVA: 0x0001208A File Offset: 0x0001028A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		/// <summary>This property is not meaningful for this control.</summary>
		/// <returns>The value of this property is not meaningful for this control.</returns>
		// Token: 0x17001237 RID: 4663
		// (get) Token: 0x06004A2C RID: 18988 RVA: 0x00011FE4 File Offset: 0x000101E4
		// (set) Token: 0x06004A2D RID: 18989 RVA: 0x00011FEC File Offset: 0x000101EC
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ImeMode ImeMode
		{
			get
			{
				return base.ImeMode;
			}
			set
			{
				base.ImeMode = value;
			}
		}

		/// <summary>This property is not supported by this control.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		/// <exception cref="T:System.NotSupportedException">This property is being set.</exception>
		// Token: 0x17001238 RID: 4664
		// (get) Token: 0x06004A2E RID: 18990 RVA: 0x000B0BBD File Offset: 0x000AEDBD
		// (set) Token: 0x06004A2F RID: 18991 RVA: 0x00134B14 File Offset: 0x00132D14
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool AllowDrop
		{
			get
			{
				return base.AllowDrop;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("WebBrowserAllowDropNotSupported"));
			}
		}

		/// <summary>This property is not supported by this control.</summary>
		/// <returns>
		///     <see langword="null" />.</returns>
		/// <exception cref="T:System.NotSupportedException">This property is being set.</exception>
		// Token: 0x17001239 RID: 4665
		// (get) Token: 0x06004A30 RID: 18992 RVA: 0x00011FC2 File Offset: 0x000101C2
		// (set) Token: 0x06004A31 RID: 18993 RVA: 0x00134B25 File Offset: 0x00132D25
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("WebBrowserBackgroundImageNotSupported"));
			}
		}

		/// <summary>This property is not supported by this control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImageLayout" /> values.</returns>
		/// <exception cref="T:System.NotSupportedException">This property is being set.</exception>
		// Token: 0x1700123A RID: 4666
		// (get) Token: 0x06004A32 RID: 18994 RVA: 0x00011FD3 File Offset: 0x000101D3
		// (set) Token: 0x06004A33 RID: 18995 RVA: 0x00134B36 File Offset: 0x00132D36
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("WebBrowserBackgroundImageLayoutNotSupported"));
			}
		}

		/// <summary>This property is not supported by this control.</summary>
		/// <returns>The value of this property is not meaningful for this control.</returns>
		/// <exception cref="T:System.NotSupportedException">This property is being set.</exception>
		// Token: 0x1700123B RID: 4667
		// (get) Token: 0x06004A34 RID: 18996 RVA: 0x00012033 File Offset: 0x00010233
		// (set) Token: 0x06004A35 RID: 18997 RVA: 0x00134B47 File Offset: 0x00132D47
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Cursor Cursor
		{
			get
			{
				return base.Cursor;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("WebBrowserCursorNotSupported"));
			}
		}

		/// <summary>This property is not supported by this control.</summary>
		/// <returns>
		///     <see langword="true" /> in all cases.</returns>
		/// <exception cref="T:System.NotSupportedException">This property is being set.</exception>
		// Token: 0x1700123C RID: 4668
		// (get) Token: 0x06004A36 RID: 18998 RVA: 0x00012060 File Offset: 0x00010260
		// (set) Token: 0x06004A37 RID: 18999 RVA: 0x00134B58 File Offset: 0x00132D58
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool Enabled
		{
			get
			{
				return base.Enabled;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("WebBrowserEnabledNotSupported"));
			}
		}

		/// <summary>This property is not supported by this control.</summary>
		/// <returns>The value of this property is not meaningful for this control.</returns>
		/// <exception cref="T:System.NotSupportedException">This property is being set.</exception>
		// Token: 0x1700123D RID: 4669
		// (get) Token: 0x06004A38 RID: 19000 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		// (set) Token: 0x06004A39 RID: 19001 RVA: 0x00134B69 File Offset: 0x00132D69
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Localizable(false)]
		public override RightToLeft RightToLeft
		{
			get
			{
				return RightToLeft.No;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("WebBrowserRightToLeftNotSupported"));
			}
		}

		/// <summary>This property is not supported by this control.</summary>
		/// <returns>The text displayed in the control.</returns>
		/// <exception cref="T:System.NotSupportedException">This property is being set.</exception>
		// Token: 0x1700123E RID: 4670
		// (get) Token: 0x06004A3A RID: 19002 RVA: 0x000E9114 File Offset: 0x000E7314
		// (set) Token: 0x06004A3B RID: 19003 RVA: 0x00134B7A File Offset: 0x00132D7A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Bindable(false)]
		public override string Text
		{
			get
			{
				return "";
			}
			set
			{
				throw new NotSupportedException(SR.GetString("WebBrowserTextNotSupported"));
			}
		}

		/// <summary>This property is not supported by this control.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		/// <exception cref="T:System.NotSupportedException">This property is being set.</exception>
		// Token: 0x1700123F RID: 4671
		// (get) Token: 0x06004A3C RID: 19004 RVA: 0x00134B8B File Offset: 0x00132D8B
		// (set) Token: 0x06004A3D RID: 19005 RVA: 0x00134B93 File Offset: 0x00132D93
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool UseWaitCursor
		{
			get
			{
				return base.UseWaitCursor;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("WebBrowserUseWaitCursorNotSupported"));
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003C4 RID: 964
		// (add) Token: 0x06004A3E RID: 19006 RVA: 0x00012256 File Offset: 0x00010456
		// (remove) Token: 0x06004A3F RID: 19007 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageLayoutChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"BackgroundImageLayoutChanged"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003C5 RID: 965
		// (add) Token: 0x06004A40 RID: 19008 RVA: 0x00134BA4 File Offset: 0x00132DA4
		// (remove) Token: 0x06004A41 RID: 19009 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler Enter
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"Enter"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003C6 RID: 966
		// (add) Token: 0x06004A42 RID: 19010 RVA: 0x00134BC3 File Offset: 0x00132DC3
		// (remove) Token: 0x06004A43 RID: 19011 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler Leave
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"Leave"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003C7 RID: 967
		// (add) Token: 0x06004A44 RID: 19012 RVA: 0x00134BE2 File Offset: 0x00132DE2
		// (remove) Token: 0x06004A45 RID: 19013 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler MouseCaptureChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseCaptureChanged"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003C8 RID: 968
		// (add) Token: 0x06004A46 RID: 19014 RVA: 0x00011FF5 File Offset: 0x000101F5
		// (remove) Token: 0x06004A47 RID: 19015 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MouseEventHandler MouseClick
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseClick"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003C9 RID: 969
		// (add) Token: 0x06004A48 RID: 19016 RVA: 0x00012014 File Offset: 0x00010214
		// (remove) Token: 0x06004A49 RID: 19017 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MouseEventHandler MouseDoubleClick
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseDoubleClick"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003CA RID: 970
		// (add) Token: 0x06004A4A RID: 19018 RVA: 0x00012218 File Offset: 0x00010418
		// (remove) Token: 0x06004A4B RID: 19019 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackColorChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"BackColorChanged"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003CB RID: 971
		// (add) Token: 0x06004A4C RID: 19020 RVA: 0x00012237 File Offset: 0x00010437
		// (remove) Token: 0x06004A4D RID: 19021 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"BackgroundImageChanged"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003CC RID: 972
		// (add) Token: 0x06004A4E RID: 19022 RVA: 0x00012275 File Offset: 0x00010475
		// (remove) Token: 0x06004A4F RID: 19023 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BindingContextChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"BindingContextChanged"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003CD RID: 973
		// (add) Token: 0x06004A50 RID: 19024 RVA: 0x000122B3 File Offset: 0x000104B3
		// (remove) Token: 0x06004A51 RID: 19025 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler CursorChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"CursorChanged"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003CE RID: 974
		// (add) Token: 0x06004A52 RID: 19026 RVA: 0x000122D2 File Offset: 0x000104D2
		// (remove) Token: 0x06004A53 RID: 19027 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler EnabledChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"EnabledChanged"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003CF RID: 975
		// (add) Token: 0x06004A54 RID: 19028 RVA: 0x000122F1 File Offset: 0x000104F1
		// (remove) Token: 0x06004A55 RID: 19029 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler FontChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"FontChanged"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003D0 RID: 976
		// (add) Token: 0x06004A56 RID: 19030 RVA: 0x00134C01 File Offset: 0x00132E01
		// (remove) Token: 0x06004A57 RID: 19031 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ForeColorChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"ForeColorChanged"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003D1 RID: 977
		// (add) Token: 0x06004A58 RID: 19032 RVA: 0x0001232F File Offset: 0x0001052F
		// (remove) Token: 0x06004A59 RID: 19033 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler RightToLeftChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"RightToLeftChanged"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003D2 RID: 978
		// (add) Token: 0x06004A5A RID: 19034 RVA: 0x0001234E File Offset: 0x0001054E
		// (remove) Token: 0x06004A5B RID: 19035 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler TextChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"TextChanged"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003D3 RID: 979
		// (add) Token: 0x06004A5C RID: 19036 RVA: 0x0001236D File Offset: 0x0001056D
		// (remove) Token: 0x06004A5D RID: 19037 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler Click
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"Click"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003D4 RID: 980
		// (add) Token: 0x06004A5E RID: 19038 RVA: 0x0001238C File Offset: 0x0001058C
		// (remove) Token: 0x06004A5F RID: 19039 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event DragEventHandler DragDrop
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"DragDrop"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003D5 RID: 981
		// (add) Token: 0x06004A60 RID: 19040 RVA: 0x00134C20 File Offset: 0x00132E20
		// (remove) Token: 0x06004A61 RID: 19041 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event DragEventHandler DragEnter
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"DragEnter"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003D6 RID: 982
		// (add) Token: 0x06004A62 RID: 19042 RVA: 0x000123CA File Offset: 0x000105CA
		// (remove) Token: 0x06004A63 RID: 19043 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event DragEventHandler DragOver
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"DragOver"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003D7 RID: 983
		// (add) Token: 0x06004A64 RID: 19044 RVA: 0x000123E9 File Offset: 0x000105E9
		// (remove) Token: 0x06004A65 RID: 19045 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler DragLeave
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"DragLeave"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003D8 RID: 984
		// (add) Token: 0x06004A66 RID: 19046 RVA: 0x00134C3F File Offset: 0x00132E3F
		// (remove) Token: 0x06004A67 RID: 19047 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event GiveFeedbackEventHandler GiveFeedback
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"GiveFeedback"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003D9 RID: 985
		// (add) Token: 0x06004A68 RID: 19048 RVA: 0x00012427 File Offset: 0x00010627
		// (remove) Token: 0x06004A69 RID: 19049 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event HelpEventHandler HelpRequested
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"HelpRequested"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003DA RID: 986
		// (add) Token: 0x06004A6A RID: 19050 RVA: 0x00012446 File Offset: 0x00010646
		// (remove) Token: 0x06004A6B RID: 19051 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event PaintEventHandler Paint
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"Paint"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003DB RID: 987
		// (add) Token: 0x06004A6C RID: 19052 RVA: 0x00012465 File Offset: 0x00010665
		// (remove) Token: 0x06004A6D RID: 19053 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event QueryContinueDragEventHandler QueryContinueDrag
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"QueryContinueDrag"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003DC RID: 988
		// (add) Token: 0x06004A6E RID: 19054 RVA: 0x00012484 File Offset: 0x00010684
		// (remove) Token: 0x06004A6F RID: 19055 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event QueryAccessibilityHelpEventHandler QueryAccessibilityHelp
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"QueryAccessibilityHelp"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003DD RID: 989
		// (add) Token: 0x06004A70 RID: 19056 RVA: 0x000124A3 File Offset: 0x000106A3
		// (remove) Token: 0x06004A71 RID: 19057 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler DoubleClick
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"DoubleClick"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003DE RID: 990
		// (add) Token: 0x06004A72 RID: 19058 RVA: 0x00134C5E File Offset: 0x00132E5E
		// (remove) Token: 0x06004A73 RID: 19059 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ImeModeChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"ImeModeChanged"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003DF RID: 991
		// (add) Token: 0x06004A74 RID: 19060 RVA: 0x000124E1 File Offset: 0x000106E1
		// (remove) Token: 0x06004A75 RID: 19061 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event KeyEventHandler KeyDown
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"KeyDown"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003E0 RID: 992
		// (add) Token: 0x06004A76 RID: 19062 RVA: 0x00012500 File Offset: 0x00010700
		// (remove) Token: 0x06004A77 RID: 19063 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event KeyPressEventHandler KeyPress
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"KeyPress"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003E1 RID: 993
		// (add) Token: 0x06004A78 RID: 19064 RVA: 0x0001251F File Offset: 0x0001071F
		// (remove) Token: 0x06004A79 RID: 19065 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event KeyEventHandler KeyUp
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"KeyUp"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003E2 RID: 994
		// (add) Token: 0x06004A7A RID: 19066 RVA: 0x0001253E File Offset: 0x0001073E
		// (remove) Token: 0x06004A7B RID: 19067 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event LayoutEventHandler Layout
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"Layout"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003E3 RID: 995
		// (add) Token: 0x06004A7C RID: 19068 RVA: 0x0001255D File Offset: 0x0001075D
		// (remove) Token: 0x06004A7D RID: 19069 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MouseEventHandler MouseDown
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseDown"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003E4 RID: 996
		// (add) Token: 0x06004A7E RID: 19070 RVA: 0x00134C7D File Offset: 0x00132E7D
		// (remove) Token: 0x06004A7F RID: 19071 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler MouseEnter
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseEnter"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003E5 RID: 997
		// (add) Token: 0x06004A80 RID: 19072 RVA: 0x00134C9C File Offset: 0x00132E9C
		// (remove) Token: 0x06004A81 RID: 19073 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler MouseLeave
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseLeave"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003E6 RID: 998
		// (add) Token: 0x06004A82 RID: 19074 RVA: 0x000125BA File Offset: 0x000107BA
		// (remove) Token: 0x06004A83 RID: 19075 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler MouseHover
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseHover"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003E7 RID: 999
		// (add) Token: 0x06004A84 RID: 19076 RVA: 0x000125D9 File Offset: 0x000107D9
		// (remove) Token: 0x06004A85 RID: 19077 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MouseEventHandler MouseMove
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseMove"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003E8 RID: 1000
		// (add) Token: 0x06004A86 RID: 19078 RVA: 0x000125F8 File Offset: 0x000107F8
		// (remove) Token: 0x06004A87 RID: 19079 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MouseEventHandler MouseUp
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseUp"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003E9 RID: 1001
		// (add) Token: 0x06004A88 RID: 19080 RVA: 0x00012617 File Offset: 0x00010817
		// (remove) Token: 0x06004A89 RID: 19081 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MouseEventHandler MouseWheel
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseWheel"
				}));
			}
			remove
			{
			}
		}

		/// <summary>Occurs when the focus or keyboard user interface (UI) cues change.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003EA RID: 1002
		// (add) Token: 0x06004A8A RID: 19082 RVA: 0x00012636 File Offset: 0x00010836
		// (remove) Token: 0x06004A8B RID: 19083 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event UICuesEventHandler ChangeUICues
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"ChangeUICues"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not supported by this control.</summary>
		/// <exception cref="T:System.NotSupportedException">A handler is being added to this event.</exception>
		// Token: 0x140003EB RID: 1003
		// (add) Token: 0x06004A8C RID: 19084 RVA: 0x00012655 File Offset: 0x00010855
		// (remove) Token: 0x06004A8D RID: 19085 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler StyleChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"StyleChanged"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x0400271C RID: 10012
		private WebBrowserHelper.AXState axState;

		// Token: 0x0400271D RID: 10013
		private WebBrowserHelper.AXState axReloadingState;

		// Token: 0x0400271E RID: 10014
		private WebBrowserHelper.AXEditMode axEditMode;

		// Token: 0x0400271F RID: 10015
		private bool inRtlRecreate;

		// Token: 0x04002720 RID: 10016
		private BitVector32 axHostState;

		// Token: 0x04002721 RID: 10017
		private WebBrowserHelper.SelectionStyle selectionStyle;

		// Token: 0x04002722 RID: 10018
		private int noComponentChange;

		// Token: 0x04002723 RID: 10019
		private WebBrowserSiteBase axSite;

		// Token: 0x04002724 RID: 10020
		private ContainerControl containingControl;

		// Token: 0x04002725 RID: 10021
		private IntPtr hwndFocus = IntPtr.Zero;

		// Token: 0x04002726 RID: 10022
		private EventHandler selectionChangeHandler;

		// Token: 0x04002727 RID: 10023
		private Guid clsid;

		// Token: 0x04002728 RID: 10024
		private UnsafeNativeMethods.IOleObject axOleObject;

		// Token: 0x04002729 RID: 10025
		private UnsafeNativeMethods.IOleInPlaceObject axOleInPlaceObject;

		// Token: 0x0400272A RID: 10026
		private UnsafeNativeMethods.IOleInPlaceActiveObject axOleInPlaceActiveObject;

		// Token: 0x0400272B RID: 10027
		private UnsafeNativeMethods.IOleControl axOleControl;

		// Token: 0x0400272C RID: 10028
		private WebBrowserBase.WebBrowserBaseNativeWindow axWindow;

		// Token: 0x0400272D RID: 10029
		private Size webBrowserBaseChangingSize = Size.Empty;

		// Token: 0x0400272E RID: 10030
		private WebBrowserContainer wbContainer;

		// Token: 0x0400272F RID: 10031
		private bool ignoreDialogKeys;

		// Token: 0x04002730 RID: 10032
		internal WebBrowserContainer container;

		// Token: 0x04002731 RID: 10033
		internal object activeXInstance;

		// Token: 0x020007FF RID: 2047
		private class WebBrowserBaseNativeWindow : NativeWindow
		{
			// Token: 0x06006E31 RID: 28209 RVA: 0x00193591 File Offset: 0x00191791
			public WebBrowserBaseNativeWindow(WebBrowserBase ax)
			{
				this.WebBrowserBase = ax;
			}

			// Token: 0x06006E32 RID: 28210 RVA: 0x001935A0 File Offset: 0x001917A0
			protected override void WndProc(ref Message m)
			{
				int msg = m.Msg;
				if (msg == 70)
				{
					this.WmWindowPosChanging(ref m);
					return;
				}
				base.WndProc(ref m);
			}

			// Token: 0x06006E33 RID: 28211 RVA: 0x001935C8 File Offset: 0x001917C8
			private unsafe void WmWindowPosChanging(ref Message m)
			{
				NativeMethods.WINDOWPOS* ptr = (NativeMethods.WINDOWPOS*)((void*)m.LParam);
				ptr->x = 0;
				ptr->y = 0;
				Size webBrowserBaseChangingSize = this.WebBrowserBase.webBrowserBaseChangingSize;
				if (webBrowserBaseChangingSize.Width == -1)
				{
					ptr->cx = this.WebBrowserBase.Width;
					ptr->cy = this.WebBrowserBase.Height;
				}
				else
				{
					ptr->cx = webBrowserBaseChangingSize.Width;
					ptr->cy = webBrowserBaseChangingSize.Height;
				}
				m.Result = (IntPtr)0;
			}

			// Token: 0x0400422B RID: 16939
			private WebBrowserBase WebBrowserBase;
		}
	}
}
