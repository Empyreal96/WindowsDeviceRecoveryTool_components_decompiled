using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x02000154 RID: 340
	internal sealed class ComponentManagerBroker : MarshalByRefObject
	{
		// Token: 0x06000B78 RID: 2936 RVA: 0x0002447C File Offset: 0x0002267C
		static ComponentManagerBroker()
		{
			int currentProcessId = SafeNativeMethods.GetCurrentProcessId();
			ComponentManagerBroker._syncObject = new object();
			ComponentManagerBroker._remoteObjectName = string.Format(CultureInfo.CurrentCulture, "ComponentManagerBroker.{0}.{1:X}", new object[]
			{
				Application.WindowsFormsVersion,
				currentProcessId
			});
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x000244C4 File Offset: 0x000226C4
		public ComponentManagerBroker()
		{
			if (ComponentManagerBroker._broker == null)
			{
				ComponentManagerBroker._broker = this;
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000B7A RID: 2938 RVA: 0x000244D9 File Offset: 0x000226D9
		internal ComponentManagerBroker Singleton
		{
			get
			{
				return ComponentManagerBroker._broker;
			}
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x000244E0 File Offset: 0x000226E0
		internal void ClearComponentManager()
		{
			this._proxy = null;
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x0000DE5C File Offset: 0x0000C05C
		public override object InitializeLifetimeService()
		{
			return null;
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x000244EC File Offset: 0x000226EC
		public UnsafeNativeMethods.IMsoComponentManager GetProxy(long pCM)
		{
			if (this._proxy == null)
			{
				UnsafeNativeMethods.IMsoComponentManager original = (UnsafeNativeMethods.IMsoComponentManager)Marshal.GetObjectForIUnknown((IntPtr)pCM);
				this._proxy = new ComponentManagerProxy(this, original);
			}
			return this._proxy;
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x00024528 File Offset: 0x00022728
		internal static UnsafeNativeMethods.IMsoComponentManager GetComponentManager(IntPtr pOriginal)
		{
			object syncObject = ComponentManagerBroker._syncObject;
			lock (syncObject)
			{
				if (ComponentManagerBroker._broker == null)
				{
					UnsafeNativeMethods.ICorRuntimeHost corRuntimeHost = (UnsafeNativeMethods.ICorRuntimeHost)RuntimeEnvironment.GetRuntimeInterfaceAsObject(typeof(UnsafeNativeMethods.CorRuntimeHost).GUID, typeof(UnsafeNativeMethods.ICorRuntimeHost).GUID);
					object obj;
					int defaultDomain = corRuntimeHost.GetDefaultDomain(out obj);
					AppDomain appDomain = obj as AppDomain;
					if (appDomain == null)
					{
						appDomain = AppDomain.CurrentDomain;
					}
					if (appDomain == AppDomain.CurrentDomain)
					{
						ComponentManagerBroker._broker = new ComponentManagerBroker();
					}
					else
					{
						ComponentManagerBroker._broker = ComponentManagerBroker.GetRemotedComponentManagerBroker(appDomain);
					}
				}
			}
			return ComponentManagerBroker._broker.GetProxy((long)pOriginal);
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x000245E0 File Offset: 0x000227E0
		private static ComponentManagerBroker GetRemotedComponentManagerBroker(AppDomain domain)
		{
			Type typeFromHandle = typeof(ComponentManagerBroker);
			ComponentManagerBroker componentManagerBroker = (ComponentManagerBroker)domain.CreateInstanceAndUnwrap(typeFromHandle.Assembly.FullName, typeFromHandle.FullName);
			return componentManagerBroker.Singleton;
		}

		// Token: 0x04000745 RID: 1861
		private static object _syncObject;

		// Token: 0x04000746 RID: 1862
		private static string _remoteObjectName;

		// Token: 0x04000747 RID: 1863
		private static ComponentManagerBroker _broker;

		// Token: 0x04000748 RID: 1864
		[ThreadStatic]
		private ComponentManagerProxy _proxy;
	}
}
