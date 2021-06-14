using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000057 RID: 87
	internal struct StoreOperationSetDeploymentMetadata
	{
		// Token: 0x06000192 RID: 402 RVA: 0x000072B3 File Offset: 0x000054B3
		public StoreOperationSetDeploymentMetadata(IDefinitionAppId Deployment, StoreApplicationReference Reference, StoreOperationMetadataProperty[] SetProperties)
		{
			this = new StoreOperationSetDeploymentMetadata(Deployment, Reference, SetProperties, null);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x000072C0 File Offset: 0x000054C0
		[SecuritySafeCritical]
		public StoreOperationSetDeploymentMetadata(IDefinitionAppId Deployment, StoreApplicationReference Reference, StoreOperationMetadataProperty[] SetProperties, StoreOperationMetadataProperty[] TestProperties)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationSetDeploymentMetadata));
			this.Flags = StoreOperationSetDeploymentMetadata.OpFlags.Nothing;
			this.Deployment = Deployment;
			if (SetProperties != null)
			{
				this.PropertiesToSet = StoreOperationSetDeploymentMetadata.MarshalProperties(SetProperties);
				this.cPropertiesToSet = new IntPtr(SetProperties.Length);
			}
			else
			{
				this.PropertiesToSet = IntPtr.Zero;
				this.cPropertiesToSet = IntPtr.Zero;
			}
			if (TestProperties != null)
			{
				this.PropertiesToTest = StoreOperationSetDeploymentMetadata.MarshalProperties(TestProperties);
				this.cPropertiesToTest = new IntPtr(TestProperties.Length);
			}
			else
			{
				this.PropertiesToTest = IntPtr.Zero;
				this.cPropertiesToTest = IntPtr.Zero;
			}
			this.InstallerReference = Reference.ToIntPtr();
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000736C File Offset: 0x0000556C
		[SecurityCritical]
		public void Destroy()
		{
			if (this.PropertiesToSet != IntPtr.Zero)
			{
				StoreOperationSetDeploymentMetadata.DestroyProperties(this.PropertiesToSet, (ulong)this.cPropertiesToSet.ToInt64());
				this.PropertiesToSet = IntPtr.Zero;
				this.cPropertiesToSet = IntPtr.Zero;
			}
			if (this.PropertiesToTest != IntPtr.Zero)
			{
				StoreOperationSetDeploymentMetadata.DestroyProperties(this.PropertiesToTest, (ulong)this.cPropertiesToTest.ToInt64());
				this.PropertiesToTest = IntPtr.Zero;
				this.cPropertiesToTest = IntPtr.Zero;
			}
			if (this.InstallerReference != IntPtr.Zero)
			{
				StoreApplicationReference.Destroy(this.InstallerReference);
				this.InstallerReference = IntPtr.Zero;
			}
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00007420 File Offset: 0x00005620
		[SecurityCritical]
		private static void DestroyProperties(IntPtr rgItems, ulong iItems)
		{
			if (rgItems != IntPtr.Zero)
			{
				ulong num = (ulong)((long)Marshal.SizeOf(typeof(StoreOperationMetadataProperty)));
				for (ulong num2 = 0UL; num2 < iItems; num2 += 1UL)
				{
					Marshal.DestroyStructure(new IntPtr((long)(num2 * num + (ulong)rgItems.ToInt64())), typeof(StoreOperationMetadataProperty));
				}
				Marshal.FreeCoTaskMem(rgItems);
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00007480 File Offset: 0x00005680
		[SecurityCritical]
		private static IntPtr MarshalProperties(StoreOperationMetadataProperty[] Props)
		{
			if (Props == null || Props.Length == 0)
			{
				return IntPtr.Zero;
			}
			int num = Marshal.SizeOf(typeof(StoreOperationMetadataProperty));
			IntPtr result = Marshal.AllocCoTaskMem(num * Props.Length);
			for (int num2 = 0; num2 != Props.Length; num2++)
			{
				Marshal.StructureToPtr(Props[num2], new IntPtr((long)(num2 * num) + result.ToInt64()), false);
			}
			return result;
		}

		// Token: 0x0400016D RID: 365
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x0400016E RID: 366
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationSetDeploymentMetadata.OpFlags Flags;

		// Token: 0x0400016F RID: 367
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Deployment;

		// Token: 0x04000170 RID: 368
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr InstallerReference;

		// Token: 0x04000171 RID: 369
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr cPropertiesToTest;

		// Token: 0x04000172 RID: 370
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr PropertiesToTest;

		// Token: 0x04000173 RID: 371
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr cPropertiesToSet;

		// Token: 0x04000174 RID: 372
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr PropertiesToSet;

		// Token: 0x02000524 RID: 1316
		[Flags]
		public enum OpFlags
		{
			// Token: 0x04003710 RID: 14096
			Nothing = 0
		}

		// Token: 0x02000525 RID: 1317
		public enum Disposition
		{
			// Token: 0x04003712 RID: 14098
			Failed,
			// Token: 0x04003713 RID: 14099
			Set = 2
		}
	}
}
