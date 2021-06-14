using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SoftwareRepository.Streaming
{
	// Token: 0x02000012 RID: 18
	[DataContract]
	public class UrlSelectionResult
	{
		// Token: 0x0600005F RID: 95 RVA: 0x00003158 File Offset: 0x00001358
		public UrlSelectionResult()
		{
			this.UrlResults = new List<UrlResult>();
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000060 RID: 96 RVA: 0x0000316B File Offset: 0x0000136B
		// (set) Token: 0x06000061 RID: 97 RVA: 0x00003173 File Offset: 0x00001373
		[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		[DataMember(Name = "urlResults")]
		public List<UrlResult> UrlResults { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000062 RID: 98 RVA: 0x0000317C File Offset: 0x0000137C
		// (set) Token: 0x06000063 RID: 99 RVA: 0x00003184 File Offset: 0x00001384
		[DataMember(Name = "testBytes")]
		public long TestBytes { get; set; }
	}
}
