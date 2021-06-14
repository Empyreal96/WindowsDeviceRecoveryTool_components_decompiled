using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Interop;
using MS.Win32;

namespace MS.Internal.Controls
{
	// Token: 0x02000753 RID: 1875
	internal class ActiveXSite : UnsafeNativeMethods.IOleControlSite, UnsafeNativeMethods.IOleClientSite, UnsafeNativeMethods.IOleInPlaceSite, UnsafeNativeMethods.IPropertyNotifySink
	{
		// Token: 0x06007773 RID: 30579 RVA: 0x00221CB9 File Offset: 0x0021FEB9
		[SecurityCritical]
		internal ActiveXSite(ActiveXHost host)
		{
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			this._host = host;
		}

		// Token: 0x06007774 RID: 30580 RVA: 0x0000B02A File Offset: 0x0000922A
		int UnsafeNativeMethods.IOleControlSite.OnControlInfoChanged()
		{
			return 0;
		}

		// Token: 0x06007775 RID: 30581 RVA: 0x00221B22 File Offset: 0x0021FD22
		int UnsafeNativeMethods.IOleControlSite.LockInPlaceActive(int fLock)
		{
			return -2147467263;
		}

		// Token: 0x06007776 RID: 30582 RVA: 0x00221CD6 File Offset: 0x0021FED6
		int UnsafeNativeMethods.IOleControlSite.GetExtendedControl(out object ppDisp)
		{
			ppDisp = null;
			return -2147467263;
		}

		// Token: 0x06007777 RID: 30583 RVA: 0x00221CE0 File Offset: 0x0021FEE0
		int UnsafeNativeMethods.IOleControlSite.TransformCoords(NativeMethods.POINT pPtlHimetric, NativeMethods.POINTF pPtfContainer, int dwFlags)
		{
			if ((dwFlags & 4) != 0)
			{
				if ((dwFlags & 2) != 0)
				{
					pPtfContainer.x = (float)ActiveXHelper.HM2Pix(pPtlHimetric.x, ActiveXHelper.LogPixelsX);
					pPtfContainer.y = (float)ActiveXHelper.HM2Pix(pPtlHimetric.y, ActiveXHelper.LogPixelsY);
				}
				else
				{
					if ((dwFlags & 1) == 0)
					{
						return -2147024809;
					}
					pPtfContainer.x = (float)ActiveXHelper.HM2Pix(pPtlHimetric.x, ActiveXHelper.LogPixelsX);
					pPtfContainer.y = (float)ActiveXHelper.HM2Pix(pPtlHimetric.y, ActiveXHelper.LogPixelsY);
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
					pPtlHimetric.x = ActiveXHelper.Pix2HM((int)pPtfContainer.x, ActiveXHelper.LogPixelsX);
					pPtlHimetric.y = ActiveXHelper.Pix2HM((int)pPtfContainer.y, ActiveXHelper.LogPixelsY);
				}
				else
				{
					if ((dwFlags & 1) == 0)
					{
						return -2147024809;
					}
					pPtlHimetric.x = ActiveXHelper.Pix2HM((int)pPtfContainer.x, ActiveXHelper.LogPixelsX);
					pPtlHimetric.y = ActiveXHelper.Pix2HM((int)pPtfContainer.y, ActiveXHelper.LogPixelsY);
				}
			}
			return 0;
		}

		// Token: 0x06007778 RID: 30584 RVA: 0x00016748 File Offset: 0x00014948
		int UnsafeNativeMethods.IOleControlSite.TranslateAccelerator(ref MSG pMsg, int grfModifiers)
		{
			return 1;
		}

		// Token: 0x06007779 RID: 30585 RVA: 0x0000B02A File Offset: 0x0000922A
		int UnsafeNativeMethods.IOleControlSite.OnFocus(int fGotFocus)
		{
			return 0;
		}

		// Token: 0x0600777A RID: 30586 RVA: 0x00221B22 File Offset: 0x0021FD22
		int UnsafeNativeMethods.IOleControlSite.ShowPropertyFrame()
		{
			return -2147467263;
		}

		// Token: 0x0600777B RID: 30587 RVA: 0x00221B22 File Offset: 0x0021FD22
		int UnsafeNativeMethods.IOleClientSite.SaveObject()
		{
			return -2147467263;
		}

		// Token: 0x0600777C RID: 30588 RVA: 0x00221DE4 File Offset: 0x0021FFE4
		int UnsafeNativeMethods.IOleClientSite.GetMoniker(int dwAssign, int dwWhichMoniker, out object moniker)
		{
			moniker = null;
			return -2147467263;
		}

		// Token: 0x0600777D RID: 30589 RVA: 0x00221DEE File Offset: 0x0021FFEE
		[SecurityCritical]
		int UnsafeNativeMethods.IOleClientSite.GetContainer(out UnsafeNativeMethods.IOleContainer container)
		{
			container = this.Host.Container;
			return 0;
		}

		// Token: 0x0600777E RID: 30590 RVA: 0x00221E00 File Offset: 0x00220000
		[SecurityCritical]
		int UnsafeNativeMethods.IOleClientSite.ShowObject()
		{
			if (this.HostState >= ActiveXHelper.ActiveXState.InPlaceActive)
			{
				IntPtr intPtr;
				if (NativeMethods.Succeeded(this.Host.ActiveXInPlaceObject.GetWindow(out intPtr)))
				{
					if (this.Host.ControlHandle.Handle != intPtr && intPtr != IntPtr.Zero)
					{
						this.Host.AttachWindow(intPtr);
						this.OnActiveXRectChange(this.Host.Bounds);
					}
				}
				else if (this.Host.ActiveXInPlaceObject is UnsafeNativeMethods.IOleInPlaceObjectWindowless)
				{
					throw new InvalidOperationException(SR.Get("AxWindowlessControl"));
				}
			}
			return 0;
		}

		// Token: 0x0600777F RID: 30591 RVA: 0x0000B02A File Offset: 0x0000922A
		int UnsafeNativeMethods.IOleClientSite.OnShowWindow(int fShow)
		{
			return 0;
		}

		// Token: 0x06007780 RID: 30592 RVA: 0x00221B22 File Offset: 0x0021FD22
		int UnsafeNativeMethods.IOleClientSite.RequestNewObjectLayout()
		{
			return -2147467263;
		}

		// Token: 0x06007781 RID: 30593 RVA: 0x00221EA0 File Offset: 0x002200A0
		[SecurityCritical]
		IntPtr UnsafeNativeMethods.IOleInPlaceSite.GetWindow()
		{
			IntPtr handle;
			try
			{
				handle = this.Host.ParentHandle.Handle;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return handle;
		}

		// Token: 0x06007782 RID: 30594 RVA: 0x00221B22 File Offset: 0x0021FD22
		int UnsafeNativeMethods.IOleInPlaceSite.ContextSensitiveHelp(int fEnterMode)
		{
			return -2147467263;
		}

		// Token: 0x06007783 RID: 30595 RVA: 0x0000B02A File Offset: 0x0000922A
		int UnsafeNativeMethods.IOleInPlaceSite.CanInPlaceActivate()
		{
			return 0;
		}

		// Token: 0x06007784 RID: 30596 RVA: 0x00221ED8 File Offset: 0x002200D8
		int UnsafeNativeMethods.IOleInPlaceSite.OnInPlaceActivate()
		{
			this.HostState = ActiveXHelper.ActiveXState.InPlaceActive;
			if (!this.HostBounds.IsEmpty)
			{
				this.OnActiveXRectChange(this.HostBounds);
			}
			return 0;
		}

		// Token: 0x06007785 RID: 30597 RVA: 0x00221EFC File Offset: 0x002200FC
		[SecurityCritical]
		int UnsafeNativeMethods.IOleInPlaceSite.OnUIActivate()
		{
			this.HostState = ActiveXHelper.ActiveXState.UIActive;
			this.Host.Container.OnUIActivate(this.Host);
			return 0;
		}

		// Token: 0x06007786 RID: 30598 RVA: 0x00221F1C File Offset: 0x0022011C
		[SecurityCritical]
		int UnsafeNativeMethods.IOleInPlaceSite.GetWindowContext(out UnsafeNativeMethods.IOleInPlaceFrame ppFrame, out UnsafeNativeMethods.IOleInPlaceUIWindow ppDoc, NativeMethods.COMRECT lprcPosRect, NativeMethods.COMRECT lprcClipRect, NativeMethods.OLEINPLACEFRAMEINFO lpFrameInfo)
		{
			ppDoc = null;
			ppFrame = this.Host.Container;
			lprcPosRect.left = this.Host.Bounds.left;
			lprcPosRect.top = this.Host.Bounds.top;
			lprcPosRect.right = this.Host.Bounds.right;
			lprcPosRect.bottom = this.Host.Bounds.bottom;
			lprcClipRect = this.Host.Bounds;
			if (lpFrameInfo != null)
			{
				lpFrameInfo.cb = (uint)Marshal.SizeOf(typeof(NativeMethods.OLEINPLACEFRAMEINFO));
				lpFrameInfo.fMDIApp = false;
				lpFrameInfo.hAccel = IntPtr.Zero;
				lpFrameInfo.cAccelEntries = 0U;
				lpFrameInfo.hwndFrame = this.Host.ParentHandle.Handle;
			}
			return 0;
		}

		// Token: 0x06007787 RID: 30599 RVA: 0x00016748 File Offset: 0x00014948
		int UnsafeNativeMethods.IOleInPlaceSite.Scroll(NativeMethods.SIZE scrollExtant)
		{
			return 1;
		}

		// Token: 0x06007788 RID: 30600 RVA: 0x00221FEF File Offset: 0x002201EF
		[SecurityCritical]
		int UnsafeNativeMethods.IOleInPlaceSite.OnUIDeactivate(int fUndoable)
		{
			this.Host.Container.OnUIDeactivate(this.Host);
			if (this.HostState > ActiveXHelper.ActiveXState.InPlaceActive)
			{
				this.HostState = ActiveXHelper.ActiveXState.InPlaceActive;
			}
			return 0;
		}

		// Token: 0x06007789 RID: 30601 RVA: 0x00222018 File Offset: 0x00220218
		[SecurityCritical]
		int UnsafeNativeMethods.IOleInPlaceSite.OnInPlaceDeactivate()
		{
			if (this.HostState == ActiveXHelper.ActiveXState.UIActive)
			{
				((UnsafeNativeMethods.IOleInPlaceSite)this).OnUIDeactivate(0);
			}
			this.Host.Container.OnInPlaceDeactivate(this.Host);
			this.HostState = ActiveXHelper.ActiveXState.Running;
			return 0;
		}

		// Token: 0x0600778A RID: 30602 RVA: 0x0000B02A File Offset: 0x0000922A
		int UnsafeNativeMethods.IOleInPlaceSite.DiscardUndoState()
		{
			return 0;
		}

		// Token: 0x0600778B RID: 30603 RVA: 0x00222049 File Offset: 0x00220249
		[SecurityCritical]
		int UnsafeNativeMethods.IOleInPlaceSite.DeactivateAndUndo()
		{
			return this.Host.ActiveXInPlaceObject.UIDeactivate();
		}

		// Token: 0x0600778C RID: 30604 RVA: 0x0022205B File Offset: 0x0022025B
		int UnsafeNativeMethods.IOleInPlaceSite.OnPosRectChange(NativeMethods.COMRECT lprcPosRect)
		{
			return this.OnActiveXRectChange(lprcPosRect);
		}

		// Token: 0x17001C5C RID: 7260
		// (get) Token: 0x0600778D RID: 30605 RVA: 0x00222064 File Offset: 0x00220264
		// (set) Token: 0x0600778E RID: 30606 RVA: 0x00222071 File Offset: 0x00220271
		private ActiveXHelper.ActiveXState HostState
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				return this.Host.ActiveXState;
			}
			[SecurityCritical]
			[SecurityTreatAsSafe]
			set
			{
				this.Host.ActiveXState = value;
			}
		}

		// Token: 0x17001C5D RID: 7261
		// (get) Token: 0x0600778F RID: 30607 RVA: 0x0022207F File Offset: 0x0022027F
		internal NativeMethods.COMRECT HostBounds
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				return this.Host.Bounds;
			}
		}

		// Token: 0x06007790 RID: 30608 RVA: 0x0022208C File Offset: 0x0022028C
		[SecurityCritical]
		void UnsafeNativeMethods.IPropertyNotifySink.OnChanged(int dispid)
		{
			try
			{
				this.OnPropertyChanged(dispid);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		// Token: 0x06007791 RID: 30609 RVA: 0x0000B02A File Offset: 0x0000922A
		int UnsafeNativeMethods.IPropertyNotifySink.OnRequestEdit(int dispid)
		{
			return 0;
		}

		// Token: 0x06007792 RID: 30610 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		internal virtual void OnPropertyChanged(int dispid)
		{
		}

		// Token: 0x17001C5E RID: 7262
		// (get) Token: 0x06007793 RID: 30611 RVA: 0x002220B8 File Offset: 0x002202B8
		internal ActiveXHost Host
		{
			[SecurityCritical]
			get
			{
				return this._host;
			}
		}

		// Token: 0x06007794 RID: 30612 RVA: 0x002220C0 File Offset: 0x002202C0
		[SecurityCritical]
		internal void StartEvents()
		{
			if (this._connectionPoint != null)
			{
				return;
			}
			object activeXInstance = this.Host.ActiveXInstance;
			if (activeXInstance != null)
			{
				try
				{
					this._connectionPoint = new ConnectionPointCookie(activeXInstance, this, typeof(UnsafeNativeMethods.IPropertyNotifySink));
				}
				catch (Exception ex)
				{
					if (CriticalExceptions.IsCriticalException(ex))
					{
						throw;
					}
				}
			}
		}

		// Token: 0x06007795 RID: 30613 RVA: 0x0022211C File Offset: 0x0022031C
		[SecurityCritical]
		internal void StopEvents()
		{
			if (this._connectionPoint != null)
			{
				this._connectionPoint.Disconnect();
				this._connectionPoint = null;
			}
		}

		// Token: 0x06007796 RID: 30614 RVA: 0x00222138 File Offset: 0x00220338
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal int OnActiveXRectChange(NativeMethods.COMRECT lprcPosRect)
		{
			if (this.Host.ActiveXInPlaceObject != null)
			{
				this.Host.ActiveXInPlaceObject.SetObjectRects(lprcPosRect, lprcPosRect);
				this.Host.Bounds = lprcPosRect;
			}
			return 0;
		}

		// Token: 0x040038C4 RID: 14532
		[SecurityCritical]
		private ActiveXHost _host;

		// Token: 0x040038C5 RID: 14533
		[SecurityCritical]
		private ConnectionPointCookie _connectionPoint;
	}
}
