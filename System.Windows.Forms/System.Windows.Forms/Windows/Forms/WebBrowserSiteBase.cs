using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Implements the interfaces of an ActiveX site for use as a base class by the <see cref="T:System.Windows.Forms.WebBrowser.WebBrowserSite" /> class.</summary>
	// Token: 0x02000432 RID: 1074
	public class WebBrowserSiteBase : UnsafeNativeMethods.IOleControlSite, UnsafeNativeMethods.IOleClientSite, UnsafeNativeMethods.IOleInPlaceSite, UnsafeNativeMethods.ISimpleFrameSite, UnsafeNativeMethods.IPropertyNotifySink, IDisposable
	{
		// Token: 0x06004AD2 RID: 19154 RVA: 0x001355DD File Offset: 0x001337DD
		internal WebBrowserSiteBase(WebBrowserBase h)
		{
			if (h == null)
			{
				throw new ArgumentNullException("h");
			}
			this.host = h;
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Windows.Forms.WebBrowserSiteBase" />. </summary>
		// Token: 0x06004AD3 RID: 19155 RVA: 0x001355FA File Offset: 0x001337FA
		public void Dispose()
		{
			this.Dispose(true);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.WebBrowserSiteBase" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x06004AD4 RID: 19156 RVA: 0x00135603 File Offset: 0x00133803
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.StopEvents();
			}
		}

		// Token: 0x17001248 RID: 4680
		// (get) Token: 0x06004AD5 RID: 19157 RVA: 0x0013560E File Offset: 0x0013380E
		internal WebBrowserBase Host
		{
			get
			{
				return this.host;
			}
		}

		// Token: 0x06004AD6 RID: 19158 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		int UnsafeNativeMethods.IOleControlSite.OnControlInfoChanged()
		{
			return 0;
		}

		// Token: 0x06004AD7 RID: 19159 RVA: 0x00033B0C File Offset: 0x00031D0C
		int UnsafeNativeMethods.IOleControlSite.LockInPlaceActive(int fLock)
		{
			return -2147467263;
		}

		// Token: 0x06004AD8 RID: 19160 RVA: 0x00033BEB File Offset: 0x00031DEB
		int UnsafeNativeMethods.IOleControlSite.GetExtendedControl(out object ppDisp)
		{
			ppDisp = null;
			return -2147467263;
		}

		// Token: 0x06004AD9 RID: 19161 RVA: 0x00135618 File Offset: 0x00133818
		int UnsafeNativeMethods.IOleControlSite.TransformCoords(NativeMethods._POINTL pPtlHimetric, NativeMethods.tagPOINTF pPtfContainer, int dwFlags)
		{
			if ((dwFlags & 4) != 0)
			{
				if ((dwFlags & 2) != 0)
				{
					pPtfContainer.x = (float)WebBrowserHelper.HM2Pix(pPtlHimetric.x, WebBrowserHelper.LogPixelsX);
					pPtfContainer.y = (float)WebBrowserHelper.HM2Pix(pPtlHimetric.y, WebBrowserHelper.LogPixelsY);
				}
				else
				{
					if ((dwFlags & 1) == 0)
					{
						return -2147024809;
					}
					pPtfContainer.x = (float)WebBrowserHelper.HM2Pix(pPtlHimetric.x, WebBrowserHelper.LogPixelsX);
					pPtfContainer.y = (float)WebBrowserHelper.HM2Pix(pPtlHimetric.y, WebBrowserHelper.LogPixelsY);
				}
			}
			else
			{
				if ((dwFlags & 8) == 0)
				{
					return -2147024809;
				}
				if ((dwFlags & 2) != 0)
				{
					pPtlHimetric.x = WebBrowserHelper.Pix2HM((int)pPtfContainer.x, WebBrowserHelper.LogPixelsX);
					pPtlHimetric.y = WebBrowserHelper.Pix2HM((int)pPtfContainer.y, WebBrowserHelper.LogPixelsY);
				}
				else
				{
					if ((dwFlags & 1) == 0)
					{
						return -2147024809;
					}
					pPtlHimetric.x = WebBrowserHelper.Pix2HM((int)pPtfContainer.x, WebBrowserHelper.LogPixelsX);
					pPtlHimetric.y = WebBrowserHelper.Pix2HM((int)pPtfContainer.y, WebBrowserHelper.LogPixelsY);
				}
			}
			return 0;
		}

		// Token: 0x06004ADA RID: 19162 RVA: 0x0013571C File Offset: 0x0013391C
		int UnsafeNativeMethods.IOleControlSite.TranslateAccelerator(ref NativeMethods.MSG pMsg, int grfModifiers)
		{
			this.Host.SetAXHostState(WebBrowserHelper.siteProcessedInputKey, true);
			Message message = default(Message);
			message.Msg = pMsg.message;
			message.WParam = pMsg.wParam;
			message.LParam = pMsg.lParam;
			message.HWnd = pMsg.hwnd;
			int result;
			try
			{
				result = ((this.Host.PreProcessControlMessage(ref message) == PreProcessControlState.MessageProcessed) ? 0 : 1);
			}
			finally
			{
				this.Host.SetAXHostState(WebBrowserHelper.siteProcessedInputKey, false);
			}
			return result;
		}

		// Token: 0x06004ADB RID: 19163 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		int UnsafeNativeMethods.IOleControlSite.OnFocus(int fGotFocus)
		{
			return 0;
		}

		// Token: 0x06004ADC RID: 19164 RVA: 0x00033B0C File Offset: 0x00031D0C
		int UnsafeNativeMethods.IOleControlSite.ShowPropertyFrame()
		{
			return -2147467263;
		}

		// Token: 0x06004ADD RID: 19165 RVA: 0x00033B0C File Offset: 0x00031D0C
		int UnsafeNativeMethods.IOleClientSite.SaveObject()
		{
			return -2147467263;
		}

		// Token: 0x06004ADE RID: 19166 RVA: 0x00033B13 File Offset: 0x00031D13
		int UnsafeNativeMethods.IOleClientSite.GetMoniker(int dwAssign, int dwWhichMoniker, out object moniker)
		{
			moniker = null;
			return -2147467263;
		}

		// Token: 0x06004ADF RID: 19167 RVA: 0x001357B4 File Offset: 0x001339B4
		int UnsafeNativeMethods.IOleClientSite.GetContainer(out UnsafeNativeMethods.IOleContainer container)
		{
			container = this.Host.GetParentContainer();
			return 0;
		}

		// Token: 0x06004AE0 RID: 19168 RVA: 0x001357C4 File Offset: 0x001339C4
		int UnsafeNativeMethods.IOleClientSite.ShowObject()
		{
			if (this.Host.ActiveXState >= WebBrowserHelper.AXState.InPlaceActive)
			{
				IntPtr intPtr;
				if (NativeMethods.Succeeded(this.Host.AXInPlaceObject.GetWindow(out intPtr)))
				{
					if (this.Host.GetHandleNoCreate() != intPtr && intPtr != IntPtr.Zero)
					{
						this.Host.AttachWindow(intPtr);
						this.OnActiveXRectChange(new NativeMethods.COMRECT(this.Host.Bounds));
					}
				}
				else if (this.Host.AXInPlaceObject is UnsafeNativeMethods.IOleInPlaceObjectWindowless)
				{
					throw new InvalidOperationException(SR.GetString("AXWindowlessControl"));
				}
			}
			return 0;
		}

		// Token: 0x06004AE1 RID: 19169 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		int UnsafeNativeMethods.IOleClientSite.OnShowWindow(int fShow)
		{
			return 0;
		}

		// Token: 0x06004AE2 RID: 19170 RVA: 0x00033B0C File Offset: 0x00031D0C
		int UnsafeNativeMethods.IOleClientSite.RequestNewObjectLayout()
		{
			return -2147467263;
		}

		// Token: 0x06004AE3 RID: 19171 RVA: 0x00135864 File Offset: 0x00133A64
		IntPtr UnsafeNativeMethods.IOleInPlaceSite.GetWindow()
		{
			IntPtr parent;
			try
			{
				parent = UnsafeNativeMethods.GetParent(new HandleRef(this.Host, this.Host.Handle));
			}
			catch (Exception ex)
			{
				throw;
			}
			return parent;
		}

		// Token: 0x06004AE4 RID: 19172 RVA: 0x00033B0C File Offset: 0x00031D0C
		int UnsafeNativeMethods.IOleInPlaceSite.ContextSensitiveHelp(int fEnterMode)
		{
			return -2147467263;
		}

		// Token: 0x06004AE5 RID: 19173 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		int UnsafeNativeMethods.IOleInPlaceSite.CanInPlaceActivate()
		{
			return 0;
		}

		// Token: 0x06004AE6 RID: 19174 RVA: 0x001358A4 File Offset: 0x00133AA4
		int UnsafeNativeMethods.IOleInPlaceSite.OnInPlaceActivate()
		{
			this.Host.ActiveXState = WebBrowserHelper.AXState.InPlaceActive;
			this.OnActiveXRectChange(new NativeMethods.COMRECT(this.Host.Bounds));
			return 0;
		}

		// Token: 0x06004AE7 RID: 19175 RVA: 0x001358CA File Offset: 0x00133ACA
		int UnsafeNativeMethods.IOleInPlaceSite.OnUIActivate()
		{
			this.Host.ActiveXState = WebBrowserHelper.AXState.UIActive;
			this.Host.GetParentContainer().OnUIActivate(this.Host);
			return 0;
		}

		// Token: 0x06004AE8 RID: 19176 RVA: 0x001358F0 File Offset: 0x00133AF0
		int UnsafeNativeMethods.IOleInPlaceSite.GetWindowContext(out UnsafeNativeMethods.IOleInPlaceFrame ppFrame, out UnsafeNativeMethods.IOleInPlaceUIWindow ppDoc, NativeMethods.COMRECT lprcPosRect, NativeMethods.COMRECT lprcClipRect, NativeMethods.tagOIFI lpFrameInfo)
		{
			ppDoc = null;
			ppFrame = this.Host.GetParentContainer();
			lprcPosRect.left = this.Host.Bounds.X;
			lprcPosRect.top = this.Host.Bounds.Y;
			lprcPosRect.right = this.Host.Bounds.Width + this.Host.Bounds.X;
			lprcPosRect.bottom = this.Host.Bounds.Height + this.Host.Bounds.Y;
			lprcClipRect = WebBrowserHelper.GetClipRect();
			if (lpFrameInfo != null)
			{
				lpFrameInfo.cb = Marshal.SizeOf(typeof(NativeMethods.tagOIFI));
				lpFrameInfo.fMDIApp = false;
				lpFrameInfo.hAccel = IntPtr.Zero;
				lpFrameInfo.cAccelEntries = 0;
				lpFrameInfo.hwndFrame = ((this.Host.ParentInternal == null) ? IntPtr.Zero : this.Host.ParentInternal.Handle);
			}
			return 0;
		}

		// Token: 0x06004AE9 RID: 19177 RVA: 0x0000E214 File Offset: 0x0000C414
		int UnsafeNativeMethods.IOleInPlaceSite.Scroll(NativeMethods.tagSIZE scrollExtant)
		{
			return 1;
		}

		// Token: 0x06004AEA RID: 19178 RVA: 0x00135A02 File Offset: 0x00133C02
		int UnsafeNativeMethods.IOleInPlaceSite.OnUIDeactivate(int fUndoable)
		{
			this.Host.GetParentContainer().OnUIDeactivate(this.Host);
			if (this.Host.ActiveXState > WebBrowserHelper.AXState.InPlaceActive)
			{
				this.Host.ActiveXState = WebBrowserHelper.AXState.InPlaceActive;
			}
			return 0;
		}

		// Token: 0x06004AEB RID: 19179 RVA: 0x00135A35 File Offset: 0x00133C35
		int UnsafeNativeMethods.IOleInPlaceSite.OnInPlaceDeactivate()
		{
			if (this.Host.ActiveXState == WebBrowserHelper.AXState.UIActive)
			{
				((UnsafeNativeMethods.IOleInPlaceSite)this).OnUIDeactivate(0);
			}
			this.Host.GetParentContainer().OnInPlaceDeactivate(this.Host);
			this.Host.ActiveXState = WebBrowserHelper.AXState.Running;
			return 0;
		}

		// Token: 0x06004AEC RID: 19180 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		int UnsafeNativeMethods.IOleInPlaceSite.DiscardUndoState()
		{
			return 0;
		}

		// Token: 0x06004AED RID: 19181 RVA: 0x00135A70 File Offset: 0x00133C70
		int UnsafeNativeMethods.IOleInPlaceSite.DeactivateAndUndo()
		{
			return this.Host.AXInPlaceObject.UIDeactivate();
		}

		// Token: 0x06004AEE RID: 19182 RVA: 0x00135A82 File Offset: 0x00133C82
		int UnsafeNativeMethods.IOleInPlaceSite.OnPosRectChange(NativeMethods.COMRECT lprcPosRect)
		{
			return this.OnActiveXRectChange(lprcPosRect);
		}

		// Token: 0x06004AEF RID: 19183 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		int UnsafeNativeMethods.ISimpleFrameSite.PreMessageFilter(IntPtr hwnd, int msg, IntPtr wp, IntPtr lp, ref IntPtr plResult, ref int pdwCookie)
		{
			return 0;
		}

		// Token: 0x06004AF0 RID: 19184 RVA: 0x0000E214 File Offset: 0x0000C414
		int UnsafeNativeMethods.ISimpleFrameSite.PostMessageFilter(IntPtr hwnd, int msg, IntPtr wp, IntPtr lp, ref IntPtr plResult, int dwCookie)
		{
			return 1;
		}

		// Token: 0x06004AF1 RID: 19185 RVA: 0x00135A8C File Offset: 0x00133C8C
		void UnsafeNativeMethods.IPropertyNotifySink.OnChanged(int dispid)
		{
			if (this.Host.NoComponentChangeEvents != 0)
			{
				return;
			}
			WebBrowserBase webBrowserBase = this.Host;
			int noComponentChangeEvents = webBrowserBase.NoComponentChangeEvents;
			webBrowserBase.NoComponentChangeEvents = noComponentChangeEvents + 1;
			try
			{
				this.OnPropertyChanged(dispid);
			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				WebBrowserBase webBrowserBase2 = this.Host;
				noComponentChangeEvents = webBrowserBase2.NoComponentChangeEvents;
				webBrowserBase2.NoComponentChangeEvents = noComponentChangeEvents - 1;
			}
		}

		// Token: 0x06004AF2 RID: 19186 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		int UnsafeNativeMethods.IPropertyNotifySink.OnRequestEdit(int dispid)
		{
			return 0;
		}

		// Token: 0x06004AF3 RID: 19187 RVA: 0x00135AFC File Offset: 0x00133CFC
		internal virtual void OnPropertyChanged(int dispid)
		{
			try
			{
				ISite site = this.Host.Site;
				if (site != null)
				{
					IComponentChangeService componentChangeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
					if (componentChangeService != null)
					{
						try
						{
							componentChangeService.OnComponentChanging(this.Host, null);
						}
						catch (CheckoutException ex)
						{
							if (ex == CheckoutException.Canceled)
							{
								return;
							}
							throw ex;
						}
						componentChangeService.OnComponentChanged(this.Host, null, null, null);
					}
				}
			}
			catch (Exception ex2)
			{
				throw;
			}
		}

		// Token: 0x06004AF4 RID: 19188 RVA: 0x00135B80 File Offset: 0x00133D80
		internal WebBrowserBase GetAXHost()
		{
			return this.Host;
		}

		// Token: 0x06004AF5 RID: 19189 RVA: 0x00135B88 File Offset: 0x00133D88
		internal void StartEvents()
		{
			if (this.connectionPoint != null)
			{
				return;
			}
			object activeXInstance = this.Host.activeXInstance;
			if (activeXInstance != null)
			{
				try
				{
					this.connectionPoint = new AxHost.ConnectionPointCookie(activeXInstance, this, typeof(UnsafeNativeMethods.IPropertyNotifySink));
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

		// Token: 0x06004AF6 RID: 19190 RVA: 0x00135BE4 File Offset: 0x00133DE4
		internal void StopEvents()
		{
			if (this.connectionPoint != null)
			{
				this.connectionPoint.Disconnect();
				this.connectionPoint = null;
			}
		}

		// Token: 0x06004AF7 RID: 19191 RVA: 0x00135C00 File Offset: 0x00133E00
		private int OnActiveXRectChange(NativeMethods.COMRECT lprcPosRect)
		{
			this.Host.AXInPlaceObject.SetObjectRects(NativeMethods.COMRECT.FromXYWH(0, 0, lprcPosRect.right - lprcPosRect.left, lprcPosRect.bottom - lprcPosRect.top), WebBrowserHelper.GetClipRect());
			this.Host.MakeDirty();
			return 0;
		}

		// Token: 0x04002751 RID: 10065
		private WebBrowserBase host;

		// Token: 0x04002752 RID: 10066
		private AxHost.ConnectionPointCookie connectionPoint;
	}
}
