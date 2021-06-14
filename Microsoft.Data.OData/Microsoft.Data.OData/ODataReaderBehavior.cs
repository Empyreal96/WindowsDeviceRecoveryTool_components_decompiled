using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData
{
	// Token: 0x020001EF RID: 495
	internal sealed class ODataReaderBehavior
	{
		// Token: 0x06000F27 RID: 3879 RVA: 0x00036233 File Offset: 0x00034433
		private ODataReaderBehavior(ODataBehaviorKind formatBehaviorKind, ODataBehaviorKind apiBehaviorKind, bool allowDuplicatePropertyNames, bool usesV1Provider, Func<IEdmType, string, IEdmType> typeResolver, string odataNamespace, string typeScheme)
		{
			this.formatBehaviorKind = formatBehaviorKind;
			this.apiBehaviorKind = apiBehaviorKind;
			this.allowDuplicatePropertyNames = allowDuplicatePropertyNames;
			this.usesV1Provider = usesV1Provider;
			this.typeResolver = typeResolver;
			this.odataNamespace = odataNamespace;
			this.typeScheme = typeScheme;
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000F28 RID: 3880 RVA: 0x00036270 File Offset: 0x00034470
		internal static ODataReaderBehavior DefaultBehavior
		{
			get
			{
				return ODataReaderBehavior.defaultReaderBehavior;
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000F29 RID: 3881 RVA: 0x00036277 File Offset: 0x00034477
		internal string ODataTypeScheme
		{
			get
			{
				return this.typeScheme;
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000F2A RID: 3882 RVA: 0x0003627F File Offset: 0x0003447F
		internal string ODataNamespace
		{
			get
			{
				return this.odataNamespace;
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000F2B RID: 3883 RVA: 0x00036287 File Offset: 0x00034487
		internal bool AllowDuplicatePropertyNames
		{
			get
			{
				return this.allowDuplicatePropertyNames;
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000F2C RID: 3884 RVA: 0x0003628F File Offset: 0x0003448F
		internal bool UseV1ProviderBehavior
		{
			get
			{
				return this.usesV1Provider;
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000F2D RID: 3885 RVA: 0x00036297 File Offset: 0x00034497
		internal Func<IEdmType, string, IEdmType> TypeResolver
		{
			get
			{
				return this.typeResolver;
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000F2E RID: 3886 RVA: 0x0003629F File Offset: 0x0003449F
		internal ODataBehaviorKind FormatBehaviorKind
		{
			get
			{
				return this.formatBehaviorKind;
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000F2F RID: 3887 RVA: 0x000362A7 File Offset: 0x000344A7
		internal ODataBehaviorKind ApiBehaviorKind
		{
			get
			{
				return this.apiBehaviorKind;
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000F30 RID: 3888 RVA: 0x000362AF File Offset: 0x000344AF
		// (set) Token: 0x06000F31 RID: 3889 RVA: 0x000362B7 File Offset: 0x000344B7
		internal Func<IEdmEntityType, bool> OperationsBoundToEntityTypeMustBeContainerQualified
		{
			get
			{
				return this.operationsBoundToEntityTypeMustBeContainerQualified;
			}
			set
			{
				this.operationsBoundToEntityTypeMustBeContainerQualified = value;
			}
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x000362C0 File Offset: 0x000344C0
		internal static ODataReaderBehavior CreateWcfDataServicesClientBehavior(Func<IEdmType, string, IEdmType> typeResolver, string odataNamespace, string typeScheme)
		{
			return new ODataReaderBehavior(ODataBehaviorKind.WcfDataServicesClient, ODataBehaviorKind.WcfDataServicesClient, true, false, typeResolver, odataNamespace, typeScheme);
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x000362CE File Offset: 0x000344CE
		internal static ODataReaderBehavior CreateWcfDataServicesServerBehavior(bool usesV1Provider)
		{
			return new ODataReaderBehavior(ODataBehaviorKind.WcfDataServicesServer, ODataBehaviorKind.WcfDataServicesServer, true, usesV1Provider, null, "http://schemas.microsoft.com/ado/2007/08/dataservices", "http://schemas.microsoft.com/ado/2007/08/dataservices/scheme");
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x000362E4 File Offset: 0x000344E4
		internal void ResetFormatBehavior()
		{
			this.formatBehaviorKind = ODataBehaviorKind.Default;
			this.allowDuplicatePropertyNames = false;
			this.usesV1Provider = false;
			this.odataNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices";
			this.typeScheme = "http://schemas.microsoft.com/ado/2007/08/dataservices/scheme";
			this.operationsBoundToEntityTypeMustBeContainerQualified = null;
		}

		// Token: 0x04000550 RID: 1360
		private static readonly ODataReaderBehavior defaultReaderBehavior = new ODataReaderBehavior(ODataBehaviorKind.Default, ODataBehaviorKind.Default, false, false, null, "http://schemas.microsoft.com/ado/2007/08/dataservices", "http://schemas.microsoft.com/ado/2007/08/dataservices/scheme");

		// Token: 0x04000551 RID: 1361
		private readonly ODataBehaviorKind apiBehaviorKind;

		// Token: 0x04000552 RID: 1362
		private readonly Func<IEdmType, string, IEdmType> typeResolver;

		// Token: 0x04000553 RID: 1363
		private bool allowDuplicatePropertyNames;

		// Token: 0x04000554 RID: 1364
		private bool usesV1Provider;

		// Token: 0x04000555 RID: 1365
		private string typeScheme;

		// Token: 0x04000556 RID: 1366
		private string odataNamespace;

		// Token: 0x04000557 RID: 1367
		private ODataBehaviorKind formatBehaviorKind;

		// Token: 0x04000558 RID: 1368
		private Func<IEdmEntityType, bool> operationsBoundToEntityTypeMustBeContainerQualified;
	}
}
