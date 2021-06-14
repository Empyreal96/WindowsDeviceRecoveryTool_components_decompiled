using System;
using System.Diagnostics;

namespace Microsoft.Data.OData
{
	// Token: 0x0200025B RID: 603
	[DebuggerDisplay("{Url.OriginalString}")]
	public sealed class ODataEntityReferenceLink : ODataItem
	{
		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x060013E8 RID: 5096 RVA: 0x0004AA6F File Offset: 0x00048C6F
		// (set) Token: 0x060013E9 RID: 5097 RVA: 0x0004AA77 File Offset: 0x00048C77
		public Uri Url { get; set; }

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x060013EA RID: 5098 RVA: 0x0004AA80 File Offset: 0x00048C80
		// (set) Token: 0x060013EB RID: 5099 RVA: 0x0004AA88 File Offset: 0x00048C88
		internal ODataEntityReferenceLinkSerializationInfo SerializationInfo
		{
			get
			{
				return this.serializationInfo;
			}
			set
			{
				this.serializationInfo = ODataEntityReferenceLinkSerializationInfo.Validate(value);
			}
		}

		// Token: 0x04000716 RID: 1814
		private ODataEntityReferenceLinkSerializationInfo serializationInfo;
	}
}
