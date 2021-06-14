using System;

namespace Microsoft.Data.OData
{
	// Token: 0x020002AC RID: 684
	public sealed class ODataProperty : ODataAnnotatable
	{
		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06001712 RID: 5906 RVA: 0x000531C6 File Offset: 0x000513C6
		// (set) Token: 0x06001713 RID: 5907 RVA: 0x000531CE File Offset: 0x000513CE
		public string Name { get; set; }

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06001714 RID: 5908 RVA: 0x000531D7 File Offset: 0x000513D7
		// (set) Token: 0x06001715 RID: 5909 RVA: 0x000531EE File Offset: 0x000513EE
		public object Value
		{
			get
			{
				if (this.odataValue == null)
				{
					return null;
				}
				return this.odataValue.FromODataValue();
			}
			set
			{
				this.odataValue = value.ToODataValue();
			}
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06001716 RID: 5910 RVA: 0x000531FC File Offset: 0x000513FC
		internal ODataValue ODataValue
		{
			get
			{
				return this.odataValue;
			}
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06001717 RID: 5911 RVA: 0x00053204 File Offset: 0x00051404
		// (set) Token: 0x06001718 RID: 5912 RVA: 0x0005320C File Offset: 0x0005140C
		internal ODataPropertySerializationInfo SerializationInfo
		{
			get
			{
				return this.serializationInfo;
			}
			set
			{
				this.serializationInfo = value;
			}
		}

		// Token: 0x0400098F RID: 2447
		private ODataValue odataValue;

		// Token: 0x04000990 RID: 2448
		private ODataPropertySerializationInfo serializationInfo;
	}
}
