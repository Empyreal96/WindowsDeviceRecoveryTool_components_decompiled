using System;
using Microsoft.Data.OData.Evaluation;

namespace Microsoft.Data.OData
{
	// Token: 0x020002AB RID: 683
	public sealed class ODataStreamReferenceValue : ODataValue
	{
		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06001707 RID: 5895 RVA: 0x000530C0 File Offset: 0x000512C0
		// (set) Token: 0x06001708 RID: 5896 RVA: 0x0005310A File Offset: 0x0005130A
		public Uri EditLink
		{
			get
			{
				Uri result;
				if (!this.hasNonComputedEditLink)
				{
					if ((result = this.computedEditLink) == null)
					{
						if (this.metadataBuilder != null)
						{
							return this.computedEditLink = this.metadataBuilder.GetStreamEditLink(this.edmPropertyName);
						}
						return null;
					}
				}
				else
				{
					result = this.editLink;
				}
				return result;
			}
			set
			{
				this.editLink = value;
				this.hasNonComputedEditLink = true;
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x06001709 RID: 5897 RVA: 0x0005311C File Offset: 0x0005131C
		// (set) Token: 0x0600170A RID: 5898 RVA: 0x00053166 File Offset: 0x00051366
		public Uri ReadLink
		{
			get
			{
				Uri result;
				if (!this.hasNonComputedReadLink)
				{
					if ((result = this.computedReadLink) == null)
					{
						if (this.metadataBuilder != null)
						{
							return this.computedReadLink = this.metadataBuilder.GetStreamReadLink(this.edmPropertyName);
						}
						return null;
					}
				}
				else
				{
					result = this.readLink;
				}
				return result;
			}
			set
			{
				this.readLink = value;
				this.hasNonComputedReadLink = true;
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x0600170B RID: 5899 RVA: 0x00053176 File Offset: 0x00051376
		// (set) Token: 0x0600170C RID: 5900 RVA: 0x0005317E File Offset: 0x0005137E
		public string ContentType { get; set; }

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x0600170D RID: 5901 RVA: 0x00053187 File Offset: 0x00051387
		// (set) Token: 0x0600170E RID: 5902 RVA: 0x0005318F File Offset: 0x0005138F
		public string ETag { get; set; }

		// Token: 0x0600170F RID: 5903 RVA: 0x00053198 File Offset: 0x00051398
		internal void SetMetadataBuilder(ODataEntityMetadataBuilder builder, string propertyName)
		{
			this.metadataBuilder = builder;
			this.edmPropertyName = propertyName;
			this.computedEditLink = null;
			this.computedReadLink = null;
		}

		// Token: 0x06001710 RID: 5904 RVA: 0x000531B6 File Offset: 0x000513B6
		internal ODataEntityMetadataBuilder GetMetadataBuilder()
		{
			return this.metadataBuilder;
		}

		// Token: 0x04000985 RID: 2437
		private ODataEntityMetadataBuilder metadataBuilder;

		// Token: 0x04000986 RID: 2438
		private string edmPropertyName;

		// Token: 0x04000987 RID: 2439
		private Uri editLink;

		// Token: 0x04000988 RID: 2440
		private Uri computedEditLink;

		// Token: 0x04000989 RID: 2441
		private bool hasNonComputedEditLink;

		// Token: 0x0400098A RID: 2442
		private Uri readLink;

		// Token: 0x0400098B RID: 2443
		private Uri computedReadLink;

		// Token: 0x0400098C RID: 2444
		private bool hasNonComputedReadLink;
	}
}
