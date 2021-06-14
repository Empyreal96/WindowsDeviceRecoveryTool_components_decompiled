using System;

namespace System.Drawing.Internal
{
	// Token: 0x020000DB RID: 219
	internal static class DeviceContexts
	{
		// Token: 0x06000B9D RID: 2973 RVA: 0x0002A75C File Offset: 0x0002895C
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

		// Token: 0x06000B9E RID: 2974 RVA: 0x0002A7B4 File Offset: 0x000289B4
		private static void OnDcDisposing(object sender, EventArgs e)
		{
			DeviceContext deviceContext = sender as DeviceContext;
			if (deviceContext != null)
			{
				deviceContext.Disposing -= DeviceContexts.OnDcDisposing;
				DeviceContexts.RemoveDeviceContext(deviceContext);
			}
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x0002A7E3 File Offset: 0x000289E3
		internal static void RemoveDeviceContext(DeviceContext dc)
		{
			if (DeviceContexts.activeDeviceContexts == null)
			{
				return;
			}
			DeviceContexts.activeDeviceContexts.RemoveByHashCode(dc);
		}

		// Token: 0x04000A3E RID: 2622
		[ThreadStatic]
		private static ClientUtils.WeakRefCollection activeDeviceContexts;
	}
}
