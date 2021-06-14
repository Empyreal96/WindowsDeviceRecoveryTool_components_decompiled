using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SoftwareRepository.Discovery
{
	// Token: 0x02000022 RID: 34
	[DataContract]
	public class DiscoveryParameters
	{
		// Token: 0x06000112 RID: 274 RVA: 0x0000416F File Offset: 0x0000236F
		public DiscoveryParameters() : this(DiscoveryCondition.Default)
		{
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00004178 File Offset: 0x00002378
		public DiscoveryParameters(DiscoveryCondition condition)
		{
			this.APIVersion = "1";
			this.Query = new DiscoveryQueryParameters();
			this.Condition = new List<string>();
			if (condition == DiscoveryCondition.All)
			{
				this.Condition.Add("all");
				return;
			}
			if (condition == DiscoveryCondition.Latest)
			{
				this.Condition.Add("latest");
				return;
			}
			this.Condition.Add("default");
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000114 RID: 276 RVA: 0x000041E6 File Offset: 0x000023E6
		// (set) Token: 0x06000115 RID: 277 RVA: 0x000041EE File Offset: 0x000023EE
		[DataMember(Name = "api-version")]
		public string APIVersion { get; set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000116 RID: 278 RVA: 0x000041F7 File Offset: 0x000023F7
		// (set) Token: 0x06000117 RID: 279 RVA: 0x000041FF File Offset: 0x000023FF
		[DataMember(Name = "query")]
		public DiscoveryQueryParameters Query { get; set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00004208 File Offset: 0x00002408
		// (set) Token: 0x06000119 RID: 281 RVA: 0x00004210 File Offset: 0x00002410
		[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		[DataMember(Name = "condition")]
		public List<string> Condition { get; set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00004219 File Offset: 0x00002419
		// (set) Token: 0x0600011B RID: 283 RVA: 0x00004221 File Offset: 0x00002421
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
		[DataMember(Name = "response")]
		public List<string> Response { get; set; }
	}
}
