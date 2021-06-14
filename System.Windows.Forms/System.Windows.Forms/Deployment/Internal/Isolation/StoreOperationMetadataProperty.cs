using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000056 RID: 86
	internal struct StoreOperationMetadataProperty
	{
		// Token: 0x06000190 RID: 400 RVA: 0x00007272 File Offset: 0x00005472
		public StoreOperationMetadataProperty(Guid PropertySet, string Name)
		{
			this = new StoreOperationMetadataProperty(PropertySet, Name, null);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000727D File Offset: 0x0000547D
		public StoreOperationMetadataProperty(Guid PropertySet, string Name, string Value)
		{
			this.GuidPropertySet = PropertySet;
			this.Name = Name;
			this.Value = Value;
			this.ValueSize = ((Value != null) ? new IntPtr((Value.Length + 1) * 2) : IntPtr.Zero);
		}

		// Token: 0x04000169 RID: 361
		public Guid GuidPropertySet;

		// Token: 0x0400016A RID: 362
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x0400016B RID: 363
		[MarshalAs(UnmanagedType.SysUInt)]
		public IntPtr ValueSize;

		// Token: 0x0400016C RID: 364
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Value;
	}
}
