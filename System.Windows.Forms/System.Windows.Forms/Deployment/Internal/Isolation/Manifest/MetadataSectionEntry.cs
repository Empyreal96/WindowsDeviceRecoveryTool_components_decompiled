using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000C7 RID: 199
	[StructLayout(LayoutKind.Sequential)]
	internal class MetadataSectionEntry : IDisposable
	{
		// Token: 0x060002C8 RID: 712 RVA: 0x000089E4 File Offset: 0x00006BE4
		~MetadataSectionEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00008A14 File Offset: 0x00006C14
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00008A20 File Offset: 0x00006C20
		[SecuritySafeCritical]
		public void Dispose(bool fDisposing)
		{
			if (this.ManifestHash != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.ManifestHash);
				this.ManifestHash = IntPtr.Zero;
			}
			if (this.MvidValue != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.MvidValue);
				this.MvidValue = IntPtr.Zero;
			}
			if (fDisposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x0400030F RID: 783
		public uint SchemaVersion;

		// Token: 0x04000310 RID: 784
		public uint ManifestFlags;

		// Token: 0x04000311 RID: 785
		public uint UsagePatterns;

		// Token: 0x04000312 RID: 786
		public IDefinitionIdentity CdfIdentity;

		// Token: 0x04000313 RID: 787
		[MarshalAs(UnmanagedType.LPWStr)]
		public string LocalPath;

		// Token: 0x04000314 RID: 788
		public uint HashAlgorithm;

		// Token: 0x04000315 RID: 789
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr ManifestHash;

		// Token: 0x04000316 RID: 790
		public uint ManifestHashSize;

		// Token: 0x04000317 RID: 791
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ContentType;

		// Token: 0x04000318 RID: 792
		[MarshalAs(UnmanagedType.LPWStr)]
		public string RuntimeImageVersion;

		// Token: 0x04000319 RID: 793
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr MvidValue;

		// Token: 0x0400031A RID: 794
		public uint MvidValueSize;

		// Token: 0x0400031B RID: 795
		public DescriptionMetadataEntry DescriptionData;

		// Token: 0x0400031C RID: 796
		public DeploymentMetadataEntry DeploymentData;

		// Token: 0x0400031D RID: 797
		public DependentOSMetadataEntry DependentOSData;

		// Token: 0x0400031E RID: 798
		[MarshalAs(UnmanagedType.LPWStr)]
		public string defaultPermissionSetID;

		// Token: 0x0400031F RID: 799
		[MarshalAs(UnmanagedType.LPWStr)]
		public string RequestedExecutionLevel;

		// Token: 0x04000320 RID: 800
		public bool RequestedExecutionLevelUIAccess;

		// Token: 0x04000321 RID: 801
		public IReferenceIdentity ResourceTypeResourcesDependency;

		// Token: 0x04000322 RID: 802
		public IReferenceIdentity ResourceTypeManifestResourcesDependency;

		// Token: 0x04000323 RID: 803
		[MarshalAs(UnmanagedType.LPWStr)]
		public string KeyInfoElement;

		// Token: 0x04000324 RID: 804
		public CompatibleFrameworksMetadataEntry CompatibleFrameworksData;
	}
}
