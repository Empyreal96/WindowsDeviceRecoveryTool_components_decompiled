using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000066 RID: 102
	[Guid("0af57545-a72a-4fbe-813c-8554ed7d4528")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IActContext
	{
		// Token: 0x060001F8 RID: 504
		[SecurityCritical]
		void GetAppId([MarshalAs(UnmanagedType.Interface)] out object AppId);

		// Token: 0x060001F9 RID: 505
		[SecurityCritical]
		void EnumCategories([In] uint Flags, [In] IReferenceIdentity CategoryToMatch, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object EnumOut);

		// Token: 0x060001FA RID: 506
		[SecurityCritical]
		void EnumSubcategories([In] uint Flags, [In] IDefinitionIdentity CategoryId, [MarshalAs(UnmanagedType.LPWStr)] [In] string SubcategoryPattern, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object EnumOut);

		// Token: 0x060001FB RID: 507
		[SecurityCritical]
		void EnumCategoryInstances([In] uint Flags, [In] IDefinitionIdentity CategoryId, [MarshalAs(UnmanagedType.LPWStr)] [In] string Subcategory, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object EnumOut);

		// Token: 0x060001FC RID: 508
		[SecurityCritical]
		void ReplaceStringMacros([In] uint Flags, [MarshalAs(UnmanagedType.LPWStr)] [In] string Culture, [MarshalAs(UnmanagedType.LPWStr)] [In] string ReplacementPattern, [MarshalAs(UnmanagedType.LPWStr)] out string Replaced);

		// Token: 0x060001FD RID: 509
		[SecurityCritical]
		void GetComponentStringTableStrings([In] uint Flags, [MarshalAs(UnmanagedType.SysUInt)] [In] IntPtr ComponentIndex, [MarshalAs(UnmanagedType.SysUInt)] [In] IntPtr StringCount, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] SourceStrings, [MarshalAs(UnmanagedType.LPArray)] out string[] DestinationStrings, [MarshalAs(UnmanagedType.SysUInt)] [In] IntPtr CultureFallbacks);

		// Token: 0x060001FE RID: 510
		[SecurityCritical]
		void GetApplicationProperties([In] uint Flags, [In] UIntPtr cProperties, [MarshalAs(UnmanagedType.LPArray)] [In] string[] PropertyNames, [MarshalAs(UnmanagedType.LPArray)] out string[] PropertyValues, [MarshalAs(UnmanagedType.LPArray)] out UIntPtr[] ComponentIndicies);

		// Token: 0x060001FF RID: 511
		[SecurityCritical]
		void ApplicationBasePath([In] uint Flags, [MarshalAs(UnmanagedType.LPWStr)] out string ApplicationPath);

		// Token: 0x06000200 RID: 512
		[SecurityCritical]
		void GetComponentManifest([In] uint Flags, [In] IDefinitionIdentity ComponentId, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ManifestInteface);

		// Token: 0x06000201 RID: 513
		[SecurityCritical]
		void GetComponentPayloadPath([In] uint Flags, [In] IDefinitionIdentity ComponentId, [MarshalAs(UnmanagedType.LPWStr)] out string PayloadPath);

		// Token: 0x06000202 RID: 514
		[SecurityCritical]
		void FindReferenceInContext([In] uint dwFlags, [In] IReferenceIdentity Reference, [MarshalAs(UnmanagedType.Interface)] out object MatchedDefinition);

		// Token: 0x06000203 RID: 515
		[SecurityCritical]
		void CreateActContextFromCategoryInstance([In] uint dwFlags, [In] ref CATEGORY_INSTANCE CategoryInstance, [MarshalAs(UnmanagedType.Interface)] out object ppCreatedAppContext);

		// Token: 0x06000204 RID: 516
		[SecurityCritical]
		void EnumComponents([In] uint dwFlags, [MarshalAs(UnmanagedType.Interface)] out object ppIdentityEnum);

		// Token: 0x06000205 RID: 517
		[SecurityCritical]
		void PrepareForExecution([MarshalAs(UnmanagedType.SysInt)] [In] IntPtr Inputs, [MarshalAs(UnmanagedType.SysInt)] [In] IntPtr Outputs);

		// Token: 0x06000206 RID: 518
		[SecurityCritical]
		void SetApplicationRunningState([In] uint dwFlags, [In] uint ulState, out uint ulDisposition);

		// Token: 0x06000207 RID: 519
		[SecurityCritical]
		void GetApplicationStateFilesystemLocation([In] uint dwFlags, [In] UIntPtr Component, [MarshalAs(UnmanagedType.SysInt)] [In] IntPtr pCoordinateList, [MarshalAs(UnmanagedType.LPWStr)] out string ppszPath);

		// Token: 0x06000208 RID: 520
		[SecurityCritical]
		void FindComponentsByDefinition([In] uint dwFlags, [In] UIntPtr ComponentCount, [MarshalAs(UnmanagedType.LPArray)] [In] IDefinitionIdentity[] Components, [MarshalAs(UnmanagedType.LPArray)] [Out] UIntPtr[] Indicies, [MarshalAs(UnmanagedType.LPArray)] [Out] uint[] Dispositions);

		// Token: 0x06000209 RID: 521
		[SecurityCritical]
		void FindComponentsByReference([In] uint dwFlags, [In] UIntPtr Components, [MarshalAs(UnmanagedType.LPArray)] [In] IReferenceIdentity[] References, [MarshalAs(UnmanagedType.LPArray)] [Out] UIntPtr[] Indicies, [MarshalAs(UnmanagedType.LPArray)] [Out] uint[] Dispositions);
	}
}
