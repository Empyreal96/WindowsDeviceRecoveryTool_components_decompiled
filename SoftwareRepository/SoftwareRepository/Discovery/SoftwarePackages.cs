using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SoftwareRepository.Discovery
{
	// Token: 0x02000027 RID: 39
	[DataContract]
	public class SoftwarePackages
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000158 RID: 344 RVA: 0x000044CB File Offset: 0x000026CB
		// (set) Token: 0x06000159 RID: 345 RVA: 0x000044D3 File Offset: 0x000026D3
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
		[DataMember(Name = "softwarePackages")]
		public List<SoftwarePackage> SoftwarePackageList { get; set; }
	}
}
