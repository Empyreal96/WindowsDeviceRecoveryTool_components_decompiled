using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x02000155 RID: 341
	internal class ComponentManagerProxy : MarshalByRefObject, UnsafeNativeMethods.IMsoComponentManager, UnsafeNativeMethods.IMsoComponent
	{
		// Token: 0x06000B80 RID: 2944 RVA: 0x0002461B File Offset: 0x0002281B
		internal ComponentManagerProxy(ComponentManagerBroker broker, UnsafeNativeMethods.IMsoComponentManager original)
		{
			this._broker = broker;
			this._original = original;
			this._creationThread = SafeNativeMethods.GetCurrentThreadId();
			this._refCount = 0;
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x00024644 File Offset: 0x00022844
		private void Dispose()
		{
			if (this._original != null)
			{
				Marshal.ReleaseComObject(this._original);
				this._original = null;
				this._components = null;
				this._componentId = (IntPtr)0;
				this._refCount = 0;
				this._broker.ClearComponentManager();
			}
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x0000DE5C File Offset: 0x0000C05C
		public override object InitializeLifetimeService()
		{
			return null;
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x00024691 File Offset: 0x00022891
		private bool RevokeComponent()
		{
			return this._original.FRevokeComponent(this._componentId);
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000B84 RID: 2948 RVA: 0x000246A4 File Offset: 0x000228A4
		private UnsafeNativeMethods.IMsoComponent Component
		{
			get
			{
				if (this._trackingComponent != null)
				{
					return this._trackingComponent;
				}
				if (this._activeComponent != null)
				{
					return this._activeComponent;
				}
				return null;
			}
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x000246C8 File Offset: 0x000228C8
		bool UnsafeNativeMethods.IMsoComponent.FDebugMessage(IntPtr hInst, int msg, IntPtr wparam, IntPtr lparam)
		{
			UnsafeNativeMethods.IMsoComponent component = this.Component;
			return component != null && component.FDebugMessage(hInst, msg, wparam, lparam);
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x000246EC File Offset: 0x000228EC
		bool UnsafeNativeMethods.IMsoComponent.FPreTranslateMessage(ref NativeMethods.MSG msg)
		{
			UnsafeNativeMethods.IMsoComponent component = this.Component;
			return component != null && component.FPreTranslateMessage(ref msg);
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x0002470C File Offset: 0x0002290C
		void UnsafeNativeMethods.IMsoComponent.OnEnterState(int uStateID, bool fEnter)
		{
			if (this._components != null)
			{
				foreach (UnsafeNativeMethods.IMsoComponent msoComponent in this._components.Values)
				{
					msoComponent.OnEnterState(uStateID, fEnter);
				}
			}
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x00024770 File Offset: 0x00022970
		void UnsafeNativeMethods.IMsoComponent.OnAppActivate(bool fActive, int dwOtherThreadID)
		{
			if (this._components != null)
			{
				foreach (UnsafeNativeMethods.IMsoComponent msoComponent in this._components.Values)
				{
					msoComponent.OnAppActivate(fActive, dwOtherThreadID);
				}
			}
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x000247D4 File Offset: 0x000229D4
		void UnsafeNativeMethods.IMsoComponent.OnLoseActivation()
		{
			if (this._activeComponent != null)
			{
				this._activeComponent.OnLoseActivation();
			}
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x000247EC File Offset: 0x000229EC
		void UnsafeNativeMethods.IMsoComponent.OnActivationChange(UnsafeNativeMethods.IMsoComponent component, bool fSameComponent, int pcrinfo, bool fHostIsActivating, int pchostinfo, int dwReserved)
		{
			if (this._components != null)
			{
				foreach (UnsafeNativeMethods.IMsoComponent msoComponent in this._components.Values)
				{
					msoComponent.OnActivationChange(component, fSameComponent, pcrinfo, fHostIsActivating, pchostinfo, dwReserved);
				}
			}
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x00024854 File Offset: 0x00022A54
		bool UnsafeNativeMethods.IMsoComponent.FDoIdle(int grfidlef)
		{
			bool flag = false;
			if (this._components != null)
			{
				foreach (UnsafeNativeMethods.IMsoComponent msoComponent in this._components.Values)
				{
					flag |= msoComponent.FDoIdle(grfidlef);
				}
			}
			return flag;
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x000248BC File Offset: 0x00022ABC
		bool UnsafeNativeMethods.IMsoComponent.FContinueMessageLoop(int reason, int pvLoopData, NativeMethods.MSG[] msgPeeked)
		{
			bool flag = false;
			if (this._refCount == 0 && this._componentId != (IntPtr)0 && this.RevokeComponent())
			{
				this._components.Clear();
				this._componentId = (IntPtr)0;
			}
			if (this._components != null)
			{
				foreach (UnsafeNativeMethods.IMsoComponent msoComponent in this._components.Values)
				{
					flag |= msoComponent.FContinueMessageLoop(reason, pvLoopData, msgPeeked);
				}
			}
			return flag;
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x0000E214 File Offset: 0x0000C414
		bool UnsafeNativeMethods.IMsoComponent.FQueryTerminate(bool fPromptUser)
		{
			return true;
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x00024960 File Offset: 0x00022B60
		void UnsafeNativeMethods.IMsoComponent.Terminate()
		{
			if (this._components != null && this._components.Values.Count > 0)
			{
				UnsafeNativeMethods.IMsoComponent[] array = new UnsafeNativeMethods.IMsoComponent[this._components.Values.Count];
				this._components.Values.CopyTo(array, 0);
				foreach (UnsafeNativeMethods.IMsoComponent msoComponent in array)
				{
					msoComponent.Terminate();
				}
			}
			if (this._original != null)
			{
				this.RevokeComponent();
			}
			this.Dispose();
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x000249E0 File Offset: 0x00022BE0
		IntPtr UnsafeNativeMethods.IMsoComponent.HwndGetWindow(int dwWhich, int dwReserved)
		{
			UnsafeNativeMethods.IMsoComponent component = this.Component;
			if (component != null)
			{
				return component.HwndGetWindow(dwWhich, dwReserved);
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x00024A05 File Offset: 0x00022C05
		int UnsafeNativeMethods.IMsoComponentManager.QueryService(ref Guid guidService, ref Guid iid, out object ppvObj)
		{
			return this._original.QueryService(ref guidService, ref iid, out ppvObj);
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x00024A15 File Offset: 0x00022C15
		bool UnsafeNativeMethods.IMsoComponentManager.FDebugMessage(IntPtr hInst, int msg, IntPtr wparam, IntPtr lparam)
		{
			return this._original.FDebugMessage(hInst, msg, wparam, lparam);
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x00024A28 File Offset: 0x00022C28
		bool UnsafeNativeMethods.IMsoComponentManager.FRegisterComponent(UnsafeNativeMethods.IMsoComponent component, NativeMethods.MSOCRINFOSTRUCT pcrinfo, out IntPtr dwComponentID)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			dwComponentID = (IntPtr)0;
			if (this._refCount == 0 && !this._original.FRegisterComponent(this, pcrinfo, out this._componentId))
			{
				return false;
			}
			this._refCount++;
			if (this._components == null)
			{
				this._components = new Dictionary<int, UnsafeNativeMethods.IMsoComponent>();
			}
			this._nextComponentId++;
			if (this._nextComponentId == 2147483647)
			{
				this._nextComponentId = 1;
			}
			bool flag = false;
			while (this._components.ContainsKey(this._nextComponentId))
			{
				this._nextComponentId++;
				if (this._nextComponentId == 2147483647)
				{
					if (flag)
					{
						throw new InvalidOperationException(SR.GetString("ComponentManagerProxyOutOfMemory"));
					}
					flag = true;
					this._nextComponentId = 1;
				}
			}
			this._components.Add(this._nextComponentId, component);
			dwComponentID = (IntPtr)this._nextComponentId;
			return true;
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x00024B1C File Offset: 0x00022D1C
		bool UnsafeNativeMethods.IMsoComponentManager.FRevokeComponent(IntPtr dwComponentID)
		{
			int num = (int)((long)dwComponentID);
			if (this._original == null)
			{
				return false;
			}
			if (this._components == null || num <= 0 || !this._components.ContainsKey(num))
			{
				return false;
			}
			if (this._refCount == 1 && SafeNativeMethods.GetCurrentThreadId() == this._creationThread && !this.RevokeComponent())
			{
				return false;
			}
			this._refCount--;
			this._components.Remove(num);
			if (this._refCount <= 0)
			{
				this.Dispose();
			}
			if (num == this._activeComponentId)
			{
				this._activeComponent = null;
				this._activeComponentId = 0;
			}
			if (num == this._trackingComponentId)
			{
				this._trackingComponent = null;
				this._trackingComponentId = 0;
			}
			return true;
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x00024BD0 File Offset: 0x00022DD0
		bool UnsafeNativeMethods.IMsoComponentManager.FUpdateComponentRegistration(IntPtr dwComponentID, NativeMethods.MSOCRINFOSTRUCT info)
		{
			return this._original != null && this._original.FUpdateComponentRegistration(this._componentId, info);
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x00024BF0 File Offset: 0x00022DF0
		bool UnsafeNativeMethods.IMsoComponentManager.FOnComponentActivate(IntPtr dwComponentID)
		{
			int num = (int)((long)dwComponentID);
			if (this._original == null)
			{
				return false;
			}
			if (this._components == null || num <= 0 || !this._components.ContainsKey(num))
			{
				return false;
			}
			if (!this._original.FOnComponentActivate(this._componentId))
			{
				return false;
			}
			this._activeComponent = this._components[num];
			this._activeComponentId = num;
			return true;
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x00024C5C File Offset: 0x00022E5C
		bool UnsafeNativeMethods.IMsoComponentManager.FSetTrackingComponent(IntPtr dwComponentID, bool fTrack)
		{
			int num = (int)((long)dwComponentID);
			if (this._original == null)
			{
				return false;
			}
			if (this._components == null || num <= 0 || !this._components.ContainsKey(num))
			{
				return false;
			}
			if (!this._original.FSetTrackingComponent(this._componentId, fTrack))
			{
				return false;
			}
			if (fTrack)
			{
				this._trackingComponent = this._components[num];
				this._trackingComponentId = num;
			}
			else
			{
				this._trackingComponent = null;
				this._trackingComponentId = 0;
			}
			return true;
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x00024CDC File Offset: 0x00022EDC
		void UnsafeNativeMethods.IMsoComponentManager.OnComponentEnterState(IntPtr dwComponentID, int uStateID, int uContext, int cpicmExclude, int rgpicmExclude, int dwReserved)
		{
			if (this._original == null)
			{
				return;
			}
			if ((uContext == 0 || uContext == 1) && this._components != null)
			{
				foreach (UnsafeNativeMethods.IMsoComponent msoComponent in this._components.Values)
				{
					msoComponent.OnEnterState(uStateID, true);
				}
			}
			this._original.OnComponentEnterState(this._componentId, uStateID, uContext, cpicmExclude, rgpicmExclude, dwReserved);
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x00024D68 File Offset: 0x00022F68
		bool UnsafeNativeMethods.IMsoComponentManager.FOnComponentExitState(IntPtr dwComponentID, int uStateID, int uContext, int cpicmExclude, int rgpicmExclude)
		{
			if (this._original == null)
			{
				return false;
			}
			if ((uContext == 0 || uContext == 1) && this._components != null)
			{
				foreach (UnsafeNativeMethods.IMsoComponent msoComponent in this._components.Values)
				{
					msoComponent.OnEnterState(uStateID, false);
				}
			}
			return this._original.FOnComponentExitState(this._componentId, uStateID, uContext, cpicmExclude, rgpicmExclude);
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x00024DF4 File Offset: 0x00022FF4
		bool UnsafeNativeMethods.IMsoComponentManager.FInState(int uStateID, IntPtr pvoid)
		{
			return this._original != null && this._original.FInState(uStateID, pvoid);
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x00024E0D File Offset: 0x0002300D
		bool UnsafeNativeMethods.IMsoComponentManager.FContinueIdle()
		{
			return this._original != null && this._original.FContinueIdle();
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x00024E24 File Offset: 0x00023024
		bool UnsafeNativeMethods.IMsoComponentManager.FPushMessageLoop(IntPtr dwComponentID, int reason, int pvLoopData)
		{
			return this._original != null && this._original.FPushMessageLoop(this._componentId, reason, pvLoopData);
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x00024E43 File Offset: 0x00023043
		bool UnsafeNativeMethods.IMsoComponentManager.FCreateSubComponentManager(object punkOuter, object punkServProv, ref Guid riid, out IntPtr ppvObj)
		{
			if (this._original == null)
			{
				ppvObj = IntPtr.Zero;
				return false;
			}
			return this._original.FCreateSubComponentManager(punkOuter, punkServProv, ref riid, out ppvObj);
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x00024E67 File Offset: 0x00023067
		bool UnsafeNativeMethods.IMsoComponentManager.FGetParentComponentManager(out UnsafeNativeMethods.IMsoComponentManager ppicm)
		{
			if (this._original == null)
			{
				ppicm = null;
				return false;
			}
			return this._original.FGetParentComponentManager(out ppicm);
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x00024E84 File Offset: 0x00023084
		bool UnsafeNativeMethods.IMsoComponentManager.FGetActiveComponent(int dwgac, UnsafeNativeMethods.IMsoComponent[] ppic, NativeMethods.MSOCRINFOSTRUCT info, int dwReserved)
		{
			if (this._original == null)
			{
				return false;
			}
			if (this._original.FGetActiveComponent(dwgac, ppic, info, dwReserved))
			{
				if (ppic[0] == this)
				{
					if (dwgac == 0)
					{
						ppic[0] = this._activeComponent;
					}
					else if (dwgac == 1)
					{
						ppic[0] = this._trackingComponent;
					}
					else if (dwgac == 2 && this._trackingComponent != null)
					{
						ppic[0] = this._trackingComponent;
					}
				}
				return ppic[0] != null;
			}
			return false;
		}

		// Token: 0x04000749 RID: 1865
		private ComponentManagerBroker _broker;

		// Token: 0x0400074A RID: 1866
		private UnsafeNativeMethods.IMsoComponentManager _original;

		// Token: 0x0400074B RID: 1867
		private int _refCount;

		// Token: 0x0400074C RID: 1868
		private int _creationThread;

		// Token: 0x0400074D RID: 1869
		private IntPtr _componentId;

		// Token: 0x0400074E RID: 1870
		private int _nextComponentId;

		// Token: 0x0400074F RID: 1871
		private Dictionary<int, UnsafeNativeMethods.IMsoComponent> _components;

		// Token: 0x04000750 RID: 1872
		private UnsafeNativeMethods.IMsoComponent _activeComponent;

		// Token: 0x04000751 RID: 1873
		private int _activeComponentId;

		// Token: 0x04000752 RID: 1874
		private UnsafeNativeMethods.IMsoComponent _trackingComponent;

		// Token: 0x04000753 RID: 1875
		private int _trackingComponentId;
	}
}
