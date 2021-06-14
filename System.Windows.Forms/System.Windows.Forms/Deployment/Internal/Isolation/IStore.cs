using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000060 RID: 96
	[Guid("a5c62f6d-5e3e-4cd9-b345-6b281d7a1d1e")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IStore
	{
		// Token: 0x060001B4 RID: 436
		[SecurityCritical]
		void Transact([In] IntPtr cOperation, [MarshalAs(UnmanagedType.LPArray)] [In] StoreTransactionOperation[] rgOperations, [MarshalAs(UnmanagedType.LPArray)] [Out] uint[] rgDispositions, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] rgResults);

		// Token: 0x060001B5 RID: 437
		[SecurityCritical]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object BindReferenceToAssembly([In] uint Flags, [In] IReferenceIdentity ReferenceIdentity, [In] uint cDeploymentsToIgnore, [MarshalAs(UnmanagedType.LPArray)] [In] IDefinitionIdentity[] DefinitionIdentity_DeploymentsToIgnore, [In] ref Guid riid);

		// Token: 0x060001B6 RID: 438
		[SecurityCritical]
		void CalculateDelimiterOfDeploymentsBasedOnQuota([In] uint dwFlags, [In] IntPtr cDeployments, [MarshalAs(UnmanagedType.LPArray)] [In] IDefinitionAppId[] rgpIDefinitionAppId_Deployments, [In] ref StoreApplicationReference InstallerReference, [In] ulong ulonglongQuota, [In] [Out] ref IntPtr Delimiter, [In] [Out] ref ulong SizeSharedWithExternalDeployment, [In] [Out] ref ulong SizeConsumedByInputDeploymentArray);

		// Token: 0x060001B7 RID: 439
		[SecurityCritical]
		IntPtr BindDefinitions([In] uint Flags, [MarshalAs(UnmanagedType.SysInt)] [In] IntPtr Count, [MarshalAs(UnmanagedType.LPArray)] [In] IDefinitionIdentity[] DefsToBind, [In] uint DeploymentsToIgnore, [MarshalAs(UnmanagedType.LPArray)] [In] IDefinitionIdentity[] DefsToIgnore);

		// Token: 0x060001B8 RID: 440
		[SecurityCritical]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object GetAssemblyInformation([In] uint Flags, [In] IDefinitionIdentity DefinitionIdentity, [In] ref Guid riid);

		// Token: 0x060001B9 RID: 441
		[SecurityCritical]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object EnumAssemblies([In] uint Flags, [In] IReferenceIdentity ReferenceIdentity_ToMatch, [In] ref Guid riid);

		// Token: 0x060001BA RID: 442
		[SecurityCritical]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object EnumFiles([In] uint Flags, [In] IDefinitionIdentity DefinitionIdentity, [In] ref Guid riid);

		// Token: 0x060001BB RID: 443
		[SecurityCritical]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object EnumInstallationReferences([In] uint Flags, [In] IDefinitionIdentity DefinitionIdentity, [In] ref Guid riid);

		// Token: 0x060001BC RID: 444
		[SecurityCritical]
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string LockAssemblyPath([In] uint Flags, [In] IDefinitionIdentity DefinitionIdentity, out IntPtr Cookie);

		// Token: 0x060001BD RID: 445
		[SecurityCritical]
		void ReleaseAssemblyPath([In] IntPtr Cookie);

		// Token: 0x060001BE RID: 446
		[SecurityCritical]
		ulong QueryChangeID([In] IDefinitionIdentity DefinitionIdentity);

		// Token: 0x060001BF RID: 447
		[SecurityCritical]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object EnumCategories([In] uint Flags, [In] IReferenceIdentity ReferenceIdentity_ToMatch, [In] ref Guid riid);

		// Token: 0x060001C0 RID: 448
		[SecurityCritical]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object EnumSubcategories([In] uint Flags, [In] IDefinitionIdentity CategoryId, [MarshalAs(UnmanagedType.LPWStr)] [In] string SubcategoryPathPattern, [In] ref Guid riid);

		// Token: 0x060001C1 RID: 449
		[SecurityCritical]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object EnumCategoryInstances([In] uint Flags, [In] IDefinitionIdentity CategoryId, [MarshalAs(UnmanagedType.LPWStr)] [In] string SubcategoryPath, [In] ref Guid riid);

		// Token: 0x060001C2 RID: 450
		[SecurityCritical]
		void GetDeploymentProperty([In] uint Flags, [In] IDefinitionAppId DeploymentInPackage, [In] ref StoreApplicationReference Reference, [In] ref Guid PropertySet, [MarshalAs(UnmanagedType.LPWStr)] [In] string pcwszPropertyName, out BLOB blob);

		// Token: 0x060001C3 RID: 451
		[SecurityCritical]
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string LockApplicationPath([In] uint Flags, [In] IDefinitionAppId ApId, out IntPtr Cookie);

		// Token: 0x060001C4 RID: 452
		[SecurityCritical]
		void ReleaseApplicationPath([In] IntPtr Cookie);

		// Token: 0x060001C5 RID: 453
		[SecurityCritical]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object EnumPrivateFiles([In] uint Flags, [In] IDefinitionAppId Application, [In] IDefinitionIdentity DefinitionIdentity, [In] ref Guid riid);

		// Token: 0x060001C6 RID: 454
		[SecurityCritical]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object EnumInstallerDeploymentMetadata([In] uint Flags, [In] ref StoreApplicationReference Reference, [In] IReferenceAppId Filter, [In] ref Guid riid);

		// Token: 0x060001C7 RID: 455
		[SecurityCritical]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object EnumInstallerDeploymentMetadataProperties([In] uint Flags, [In] ref StoreApplicationReference Reference, [In] IDefinitionAppId Filter, [In] ref Guid riid);
	}
}
