using System;
using Microsoft.Data.OData.Evaluation;
using Microsoft.Data.OData.JsonLight;

namespace Microsoft.Data.OData
{
	// Token: 0x020001FC RID: 508
	public abstract class ODataOperation : ODataAnnotatable
	{
		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000F72 RID: 3954 RVA: 0x00037494 File Offset: 0x00035694
		// (set) Token: 0x06000F73 RID: 3955 RVA: 0x0003749C File Offset: 0x0003569C
		public Uri Metadata { get; set; }

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000F74 RID: 3956 RVA: 0x000374A8 File Offset: 0x000356A8
		// (set) Token: 0x06000F75 RID: 3957 RVA: 0x000374F2 File Offset: 0x000356F2
		public string Title
		{
			get
			{
				string result;
				if (!this.hasNonComputedTitle)
				{
					if ((result = this.computedTitle) == null)
					{
						if (this.metadataBuilder != null)
						{
							return this.computedTitle = this.metadataBuilder.GetOperationTitle(this.operationFullName);
						}
						return null;
					}
				}
				else
				{
					result = this.title;
				}
				return result;
			}
			set
			{
				this.title = value;
				this.hasNonComputedTitle = true;
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000F76 RID: 3958 RVA: 0x00037504 File Offset: 0x00035704
		// (set) Token: 0x06000F77 RID: 3959 RVA: 0x00037554 File Offset: 0x00035754
		public Uri Target
		{
			get
			{
				Uri result;
				if (!this.hasNonComputedTarget)
				{
					if ((result = this.computedTarget) == null)
					{
						if (this.metadataBuilder != null)
						{
							return this.computedTarget = this.metadataBuilder.GetOperationTargetUri(this.operationFullName, this.bindingParameterTypeName);
						}
						return null;
					}
				}
				else
				{
					result = this.target;
				}
				return result;
			}
			set
			{
				this.target = value;
				this.hasNonComputedTarget = true;
			}
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x00037564 File Offset: 0x00035764
		internal void SetMetadataBuilder(ODataEntityMetadataBuilder builder, Uri metadataDocumentUri)
		{
			ODataJsonLightValidationUtils.ValidateOperation(metadataDocumentUri, this);
			this.metadataBuilder = builder;
			this.operationFullName = ODataJsonLightUtils.GetFullyQualifiedFunctionImportName(metadataDocumentUri, UriUtilsCommon.UriToString(this.Metadata), out this.bindingParameterTypeName);
			this.computedTitle = null;
			this.computedTarget = null;
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x0003759F File Offset: 0x0003579F
		internal ODataEntityMetadataBuilder GetMetadataBuilder()
		{
			return this.metadataBuilder;
		}

		// Token: 0x04000578 RID: 1400
		private ODataEntityMetadataBuilder metadataBuilder;

		// Token: 0x04000579 RID: 1401
		private string title;

		// Token: 0x0400057A RID: 1402
		private bool hasNonComputedTitle;

		// Token: 0x0400057B RID: 1403
		private string computedTitle;

		// Token: 0x0400057C RID: 1404
		private Uri target;

		// Token: 0x0400057D RID: 1405
		private bool hasNonComputedTarget;

		// Token: 0x0400057E RID: 1406
		private Uri computedTarget;

		// Token: 0x0400057F RID: 1407
		private string operationFullName;

		// Token: 0x04000580 RID: 1408
		private string bindingParameterTypeName;
	}
}
