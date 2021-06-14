using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData
{
	// Token: 0x020000EA RID: 234
	internal abstract class ODataDeserializer
	{
		// Token: 0x060005CE RID: 1486 RVA: 0x00014425 File Offset: 0x00012625
		protected ODataDeserializer(ODataInputContext inputContext)
		{
			this.inputContext = inputContext;
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x00014434 File Offset: 0x00012634
		internal bool UseClientFormatBehavior
		{
			get
			{
				return this.inputContext.UseClientFormatBehavior;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060005D0 RID: 1488 RVA: 0x00014441 File Offset: 0x00012641
		internal bool UseServerFormatBehavior
		{
			get
			{
				return this.inputContext.UseServerFormatBehavior;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060005D1 RID: 1489 RVA: 0x0001444E File Offset: 0x0001264E
		internal bool UseDefaultFormatBehavior
		{
			get
			{
				return this.inputContext.UseDefaultFormatBehavior;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060005D2 RID: 1490 RVA: 0x0001445B File Offset: 0x0001265B
		internal ODataMessageReaderSettings MessageReaderSettings
		{
			get
			{
				return this.inputContext.MessageReaderSettings;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060005D3 RID: 1491 RVA: 0x00014468 File Offset: 0x00012668
		internal ODataVersion Version
		{
			get
			{
				return this.inputContext.Version;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060005D4 RID: 1492 RVA: 0x00014475 File Offset: 0x00012675
		internal bool ReadingResponse
		{
			get
			{
				return this.inputContext.ReadingResponse;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060005D5 RID: 1493 RVA: 0x00014482 File Offset: 0x00012682
		internal IEdmModel Model
		{
			get
			{
				return this.inputContext.Model;
			}
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0001448F File Offset: 0x0001268F
		internal DuplicatePropertyNamesChecker CreateDuplicatePropertyNamesChecker()
		{
			return this.inputContext.CreateDuplicatePropertyNamesChecker();
		}

		// Token: 0x04000268 RID: 616
		private readonly ODataInputContext inputContext;
	}
}
