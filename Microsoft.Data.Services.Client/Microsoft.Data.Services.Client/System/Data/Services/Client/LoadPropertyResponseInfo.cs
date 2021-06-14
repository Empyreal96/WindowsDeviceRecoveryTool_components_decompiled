using System;
using System.Data.Services.Client.Metadata;

namespace System.Data.Services.Client
{
	// Token: 0x02000088 RID: 136
	internal class LoadPropertyResponseInfo : ResponseInfo
	{
		// Token: 0x060004D4 RID: 1236 RVA: 0x00013C9C File Offset: 0x00011E9C
		internal LoadPropertyResponseInfo(RequestInfo requestInfo, MergeOption mergeOption, EntityDescriptor entityDescriptor, ClientPropertyAnnotation property) : base(requestInfo, mergeOption)
		{
			this.EntityDescriptor = entityDescriptor;
			this.Property = property;
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x00013CB5 File Offset: 0x00011EB5
		// (set) Token: 0x060004D6 RID: 1238 RVA: 0x00013CBD File Offset: 0x00011EBD
		internal EntityDescriptor EntityDescriptor { get; private set; }

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x00013CC6 File Offset: 0x00011EC6
		// (set) Token: 0x060004D8 RID: 1240 RVA: 0x00013CCE File Offset: 0x00011ECE
		internal ClientPropertyAnnotation Property { get; private set; }
	}
}
