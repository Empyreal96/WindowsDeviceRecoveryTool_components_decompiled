using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData
{
	// Token: 0x02000259 RID: 601
	public sealed class ODataEntityReferenceLinks : ODataAnnotatable
	{
		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x060013DE RID: 5086 RVA: 0x0004AA16 File Offset: 0x00048C16
		// (set) Token: 0x060013DF RID: 5087 RVA: 0x0004AA1E File Offset: 0x00048C1E
		public long? Count { get; set; }

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x060013E0 RID: 5088 RVA: 0x0004AA27 File Offset: 0x00048C27
		// (set) Token: 0x060013E1 RID: 5089 RVA: 0x0004AA2F File Offset: 0x00048C2F
		public Uri NextPageLink { get; set; }

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x060013E2 RID: 5090 RVA: 0x0004AA38 File Offset: 0x00048C38
		// (set) Token: 0x060013E3 RID: 5091 RVA: 0x0004AA40 File Offset: 0x00048C40
		public IEnumerable<ODataEntityReferenceLink> Links { get; set; }

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x060013E4 RID: 5092 RVA: 0x0004AA49 File Offset: 0x00048C49
		// (set) Token: 0x060013E5 RID: 5093 RVA: 0x0004AA51 File Offset: 0x00048C51
		internal ODataEntityReferenceLinksSerializationInfo SerializationInfo
		{
			get
			{
				return this.serializationInfo;
			}
			set
			{
				this.serializationInfo = ODataEntityReferenceLinksSerializationInfo.Validate(value);
			}
		}

		// Token: 0x04000712 RID: 1810
		private ODataEntityReferenceLinksSerializationInfo serializationInfo;
	}
}
