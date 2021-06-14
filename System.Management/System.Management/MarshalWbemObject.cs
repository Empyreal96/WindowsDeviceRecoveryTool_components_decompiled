using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000057 RID: 87
	internal class MarshalWbemObject : ICustomMarshaler
	{
		// Token: 0x06000385 RID: 901 RVA: 0x00021821 File Offset: 0x0001FA21
		public static ICustomMarshaler GetInstance(string cookie)
		{
			return new MarshalWbemObject(cookie);
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00021829 File Offset: 0x0001FA29
		private MarshalWbemObject(string cookie)
		{
			this.cookie = cookie;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00002208 File Offset: 0x00000408
		public void CleanUpManagedData(object obj)
		{
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00002208 File Offset: 0x00000408
		public void CleanUpNativeData(IntPtr pObj)
		{
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00021838 File Offset: 0x0001FA38
		public int GetNativeDataSize()
		{
			return -1;
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0002183B File Offset: 0x0001FA3B
		public IntPtr MarshalManagedToNative(object obj)
		{
			return (IntPtr)obj;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00021843 File Offset: 0x0001FA43
		public object MarshalNativeToManaged(IntPtr pObj)
		{
			return new IWbemClassObjectFreeThreaded(pObj);
		}

		// Token: 0x0400024C RID: 588
		private string cookie;
	}
}
