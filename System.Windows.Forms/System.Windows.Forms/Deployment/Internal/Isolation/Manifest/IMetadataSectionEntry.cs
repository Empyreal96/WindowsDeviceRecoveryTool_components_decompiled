using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000C9 RID: 201
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("AB1ED79F-943E-407d-A80B-0744E3A95B28")]
	[ComImport]
	internal interface IMetadataSectionEntry
	{
		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060002CC RID: 716
		MetadataSectionEntry AllData { [SecurityCritical] get; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060002CD RID: 717
		uint SchemaVersion { [SecurityCritical] get; }

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060002CE RID: 718
		uint ManifestFlags { [SecurityCritical] get; }

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060002CF RID: 719
		uint UsagePatterns { [SecurityCritical] get; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060002D0 RID: 720
		IDefinitionIdentity CdfIdentity { [SecurityCritical] get; }

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060002D1 RID: 721
		string LocalPath { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060002D2 RID: 722
		uint HashAlgorithm { [SecurityCritical] get; }

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060002D3 RID: 723
		object ManifestHash { [SecurityCritical] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060002D4 RID: 724
		string ContentType { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060002D5 RID: 725
		string RuntimeImageVersion { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060002D6 RID: 726
		object MvidValue { [SecurityCritical] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060002D7 RID: 727
		IDescriptionMetadataEntry DescriptionData { [SecurityCritical] get; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060002D8 RID: 728
		IDeploymentMetadataEntry DeploymentData { [SecurityCritical] get; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060002D9 RID: 729
		IDependentOSMetadataEntry DependentOSData { [SecurityCritical] get; }

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060002DA RID: 730
		string defaultPermissionSetID { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060002DB RID: 731
		string RequestedExecutionLevel { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060002DC RID: 732
		bool RequestedExecutionLevelUIAccess { [SecurityCritical] get; }

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060002DD RID: 733
		IReferenceIdentity ResourceTypeResourcesDependency { [SecurityCritical] get; }

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060002DE RID: 734
		IReferenceIdentity ResourceTypeManifestResourcesDependency { [SecurityCritical] get; }

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060002DF RID: 735
		string KeyInfoElement { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060002E0 RID: 736
		ICompatibleFrameworksMetadataEntry CompatibleFrameworksData { [SecurityCritical] get; }
	}
}
