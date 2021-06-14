using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200007E RID: 126
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("a504e5b0-8ccf-4cb4-9902-c9d1b9abd033")]
	[ComImport]
	internal interface ICMS
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600020E RID: 526
		IDefinitionIdentity Identity { [SecurityCritical] get; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600020F RID: 527
		ISection FileSection { [SecurityCritical] get; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000210 RID: 528
		ISection CategoryMembershipSection { [SecurityCritical] get; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000211 RID: 529
		ISection COMRedirectionSection { [SecurityCritical] get; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000212 RID: 530
		ISection ProgIdRedirectionSection { [SecurityCritical] get; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000213 RID: 531
		ISection CLRSurrogateSection { [SecurityCritical] get; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000214 RID: 532
		ISection AssemblyReferenceSection { [SecurityCritical] get; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000215 RID: 533
		ISection WindowClassSection { [SecurityCritical] get; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000216 RID: 534
		ISection StringSection { [SecurityCritical] get; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000217 RID: 535
		ISection EntryPointSection { [SecurityCritical] get; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000218 RID: 536
		ISection PermissionSetSection { [SecurityCritical] get; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000219 RID: 537
		ISectionEntry MetadataSectionEntry { [SecurityCritical] get; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600021A RID: 538
		ISection AssemblyRequestSection { [SecurityCritical] get; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600021B RID: 539
		ISection RegistryKeySection { [SecurityCritical] get; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600021C RID: 540
		ISection DirectorySection { [SecurityCritical] get; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600021D RID: 541
		ISection FileAssociationSection { [SecurityCritical] get; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600021E RID: 542
		ISection CompatibleFrameworksSection { [SecurityCritical] get; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600021F RID: 543
		ISection EventSection { [SecurityCritical] get; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000220 RID: 544
		ISection EventMapSection { [SecurityCritical] get; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000221 RID: 545
		ISection EventTagSection { [SecurityCritical] get; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000222 RID: 546
		ISection CounterSetSection { [SecurityCritical] get; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000223 RID: 547
		ISection CounterSection { [SecurityCritical] get; }
	}
}
