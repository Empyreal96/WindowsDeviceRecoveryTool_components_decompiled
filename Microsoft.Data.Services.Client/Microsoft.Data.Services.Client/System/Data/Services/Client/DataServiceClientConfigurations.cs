using System;

namespace System.Data.Services.Client
{
	// Token: 0x02000040 RID: 64
	public class DataServiceClientConfigurations
	{
		// Token: 0x06000202 RID: 514 RVA: 0x0000AC96 File Offset: 0x00008E96
		internal DataServiceClientConfigurations(object sender)
		{
			this.ResponsePipeline = new DataServiceClientResponsePipelineConfiguration(sender);
			this.RequestPipeline = new DataServiceClientRequestPipelineConfiguration();
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000203 RID: 515 RVA: 0x0000ACB5 File Offset: 0x00008EB5
		// (set) Token: 0x06000204 RID: 516 RVA: 0x0000ACBD File Offset: 0x00008EBD
		public DataServiceClientResponsePipelineConfiguration ResponsePipeline { get; private set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000205 RID: 517 RVA: 0x0000ACC6 File Offset: 0x00008EC6
		// (set) Token: 0x06000206 RID: 518 RVA: 0x0000ACCE File Offset: 0x00008ECE
		public DataServiceClientRequestPipelineConfiguration RequestPipeline { get; private set; }
	}
}
