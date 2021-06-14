using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.WindowsAzure.Storage
{
	// Token: 0x02000072 RID: 114
	internal sealed class EncryptionAgent
	{
		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000E72 RID: 3698 RVA: 0x000380F1 File Offset: 0x000362F1
		// (set) Token: 0x06000E73 RID: 3699 RVA: 0x000380F9 File Offset: 0x000362F9
		public string Protocol { get; set; }

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000E74 RID: 3700 RVA: 0x00038102 File Offset: 0x00036302
		// (set) Token: 0x06000E75 RID: 3701 RVA: 0x0003810A File Offset: 0x0003630A
		[JsonConverter(typeof(StringEnumConverter))]
		public EncryptionAlgorithm EncryptionAlgorithm { get; set; }

		// Token: 0x06000E76 RID: 3702 RVA: 0x00038113 File Offset: 0x00036313
		public EncryptionAgent(string protocol, EncryptionAlgorithm algorithm)
		{
			this.Protocol = protocol;
			this.EncryptionAlgorithm = algorithm;
		}
	}
}
