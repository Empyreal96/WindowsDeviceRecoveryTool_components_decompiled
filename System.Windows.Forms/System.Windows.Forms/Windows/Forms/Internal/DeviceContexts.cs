using System;

namespace System.Windows.Forms.Internal
{
	// Token: 0x020004E8 RID: 1256
	internal static class DeviceContexts
	{
		// Token: 0x06005326 RID: 21286 RVA: 0x0015CDAC File Offset: 0x0015AFAC
		internal static void AddDeviceContext(DeviceContext dc)
		{
			if (DeviceContexts.activeDeviceContexts == null)
			{
				DeviceContexts.activeDeviceContexts = new ClientUtils.WeakRefCollection();
				DeviceContexts.activeDeviceContexts.RefCheckThreshold = 20;
			}
			if (!DeviceContexts.activeDeviceContexts.Contains(dc))
			{
				dc.Disposing += DeviceContexts.OnDcDisposing;
				DeviceContexts.activeDeviceContexts.Add(dc);
			}
		}

		// Token: 0x06005327 RID: 21287 RVA: 0x0015CE04 File Offset: 0x0015B004
		private static void OnDcDisposing(object sender, EventArgs e)
		{
			DeviceContext deviceContext = sender as DeviceContext;
			if (deviceContext != null)
			{
				deviceContext.Disposing -= DeviceContexts.OnDcDisposing;
				DeviceContexts.RemoveDeviceContext(deviceContext);
			}
		}

		// Token: 0x06005328 RID: 21288 RVA: 0x0015CE33 File Offset: 0x0015B033
		internal static void RemoveDeviceContext(DeviceContext dc)
		{
			if (DeviceContexts.activeDeviceContexts == null)
			{
				return;
			}
			DeviceContexts.activeDeviceContexts.RemoveByHashCode(dc);
		}

		// Token: 0x06005329 RID: 21289 RVA: 0x0015CE48 File Offset: 0x0015B048
		internal static bool IsFontInUse(WindowsFont wf)
		{
			if (wf == null)
			{
				return false;
			}
			for (int i = 0; i < DeviceContexts.activeDeviceContexts.Count; i++)
			{
				DeviceContext deviceContext = DeviceContexts.activeDeviceContexts[i] as DeviceContext;
				if (deviceContext != null && (deviceContext.ActiveFont == wf || deviceContext.IsFontOnContextStack(wf)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400352C RID: 13612
		[ThreadStatic]
		private static ClientUtils.WeakRefCollection activeDeviceContexts;
	}
}
