using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData
{
	// Token: 0x020001BA RID: 442
	internal sealed class ODataPayloadKindDetectionInfo
	{
		// Token: 0x06000DC4 RID: 3524 RVA: 0x0002FE80 File Offset: 0x0002E080
		internal ODataPayloadKindDetectionInfo(MediaType contentType, Encoding encoding, ODataMessageReaderSettings messageReaderSettings, IEdmModel model, IEnumerable<ODataPayloadKind> possiblePayloadKinds)
		{
			ExceptionUtils.CheckArgumentNotNull<MediaType>(contentType, "contentType");
			ExceptionUtils.CheckArgumentNotNull<ODataMessageReaderSettings>(messageReaderSettings, "readerSettings");
			ExceptionUtils.CheckArgumentNotNull<IEnumerable<ODataPayloadKind>>(possiblePayloadKinds, "possiblePayloadKinds");
			this.contentType = contentType;
			this.encoding = encoding;
			this.messageReaderSettings = messageReaderSettings;
			this.model = model;
			this.possiblePayloadKinds = possiblePayloadKinds;
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000DC5 RID: 3525 RVA: 0x0002FEDA File Offset: 0x0002E0DA
		public ODataMessageReaderSettings MessageReaderSettings
		{
			get
			{
				return this.messageReaderSettings;
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000DC6 RID: 3526 RVA: 0x0002FEE2 File Offset: 0x0002E0E2
		public IEdmModel Model
		{
			get
			{
				return this.model;
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000DC7 RID: 3527 RVA: 0x0002FEEA File Offset: 0x0002E0EA
		public IEnumerable<ODataPayloadKind> PossiblePayloadKinds
		{
			get
			{
				return this.possiblePayloadKinds;
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000DC8 RID: 3528 RVA: 0x0002FEF2 File Offset: 0x0002E0F2
		internal MediaType ContentType
		{
			get
			{
				return this.contentType;
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000DC9 RID: 3529 RVA: 0x0002FEFA File Offset: 0x0002E0FA
		internal object PayloadKindDetectionFormatState
		{
			get
			{
				return this.payloadKindDetectionFormatState;
			}
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x0002FF02 File Offset: 0x0002E102
		public Encoding GetEncoding()
		{
			return this.encoding ?? this.contentType.SelectEncoding();
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x0002FF19 File Offset: 0x0002E119
		public void SetPayloadKindDetectionFormatState(object state)
		{
			this.payloadKindDetectionFormatState = state;
		}

		// Token: 0x04000497 RID: 1175
		private readonly MediaType contentType;

		// Token: 0x04000498 RID: 1176
		private readonly Encoding encoding;

		// Token: 0x04000499 RID: 1177
		private readonly ODataMessageReaderSettings messageReaderSettings;

		// Token: 0x0400049A RID: 1178
		private readonly IEdmModel model;

		// Token: 0x0400049B RID: 1179
		private readonly IEnumerable<ODataPayloadKind> possiblePayloadKinds;

		// Token: 0x0400049C RID: 1180
		private object payloadKindDetectionFormatState;
	}
}
