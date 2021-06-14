using System;

namespace Microsoft.Data.OData
{
	// Token: 0x020001BC RID: 444
	public sealed class ODataPayloadKindDetectionResult
	{
		// Token: 0x06000DCC RID: 3532 RVA: 0x0002FF22 File Offset: 0x0002E122
		internal ODataPayloadKindDetectionResult(ODataPayloadKind payloadKind, ODataFormat format)
		{
			this.payloadKind = payloadKind;
			this.format = format;
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000DCD RID: 3533 RVA: 0x0002FF38 File Offset: 0x0002E138
		public ODataPayloadKind PayloadKind
		{
			get
			{
				return this.payloadKind;
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000DCE RID: 3534 RVA: 0x0002FF40 File Offset: 0x0002E140
		public ODataFormat Format
		{
			get
			{
				return this.format;
			}
		}

		// Token: 0x040004A6 RID: 1190
		private readonly ODataPayloadKind payloadKind;

		// Token: 0x040004A7 RID: 1191
		private readonly ODataFormat format;
	}
}
