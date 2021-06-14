using System;

namespace Microsoft.Data.OData
{
	// Token: 0x02000238 RID: 568
	public sealed class ODataCollectionStart : ODataAnnotatable
	{
		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06001232 RID: 4658 RVA: 0x00044441 File Offset: 0x00042641
		// (set) Token: 0x06001233 RID: 4659 RVA: 0x00044449 File Offset: 0x00042649
		public string Name { get; set; }

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06001234 RID: 4660 RVA: 0x00044452 File Offset: 0x00042652
		// (set) Token: 0x06001235 RID: 4661 RVA: 0x0004445A File Offset: 0x0004265A
		internal ODataCollectionStartSerializationInfo SerializationInfo
		{
			get
			{
				return this.serializationInfo;
			}
			set
			{
				this.serializationInfo = ODataCollectionStartSerializationInfo.Validate(value);
			}
		}

		// Token: 0x04000690 RID: 1680
		private ODataCollectionStartSerializationInfo serializationInfo;
	}
}
