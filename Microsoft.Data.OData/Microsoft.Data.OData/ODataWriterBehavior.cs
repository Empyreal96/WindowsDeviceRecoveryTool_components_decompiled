using System;

namespace Microsoft.Data.OData
{
	// Token: 0x020001F0 RID: 496
	internal sealed class ODataWriterBehavior
	{
		// Token: 0x06000F36 RID: 3894 RVA: 0x00036333 File Offset: 0x00034533
		private ODataWriterBehavior(ODataBehaviorKind formatBehaviorKind, ODataBehaviorKind apiBehaviorKind, bool usesV1Provider, bool allowNullValuesForNonNullablePrimitiveTypes, bool allowDuplicatePropertyNames, string odataNamespace, string typeScheme)
		{
			this.formatBehaviorKind = formatBehaviorKind;
			this.apiBehaviorKind = apiBehaviorKind;
			this.usesV1Provider = usesV1Provider;
			this.allowNullValuesForNonNullablePrimitiveTypes = allowNullValuesForNonNullablePrimitiveTypes;
			this.allowDuplicatePropertyNames = allowDuplicatePropertyNames;
			this.odataNamespace = odataNamespace;
			this.typeScheme = typeScheme;
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000F37 RID: 3895 RVA: 0x00036370 File Offset: 0x00034570
		internal static ODataWriterBehavior DefaultBehavior
		{
			get
			{
				return ODataWriterBehavior.defaultWriterBehavior;
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000F38 RID: 3896 RVA: 0x00036377 File Offset: 0x00034577
		internal string ODataTypeScheme
		{
			get
			{
				return this.typeScheme;
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000F39 RID: 3897 RVA: 0x0003637F File Offset: 0x0003457F
		internal string ODataNamespace
		{
			get
			{
				return this.odataNamespace;
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000F3A RID: 3898 RVA: 0x00036387 File Offset: 0x00034587
		internal bool UseV1ProviderBehavior
		{
			get
			{
				return this.usesV1Provider;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000F3B RID: 3899 RVA: 0x0003638F File Offset: 0x0003458F
		internal bool AllowNullValuesForNonNullablePrimitiveTypes
		{
			get
			{
				return this.allowNullValuesForNonNullablePrimitiveTypes;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000F3C RID: 3900 RVA: 0x00036397 File Offset: 0x00034597
		internal bool AllowDuplicatePropertyNames
		{
			get
			{
				return this.allowDuplicatePropertyNames;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000F3D RID: 3901 RVA: 0x0003639F File Offset: 0x0003459F
		internal ODataBehaviorKind FormatBehaviorKind
		{
			get
			{
				return this.formatBehaviorKind;
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000F3E RID: 3902 RVA: 0x000363A7 File Offset: 0x000345A7
		internal ODataBehaviorKind ApiBehaviorKind
		{
			get
			{
				return this.apiBehaviorKind;
			}
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x000363AF File Offset: 0x000345AF
		internal static ODataWriterBehavior CreateWcfDataServicesClientBehavior(string odataNamespace, string typeScheme)
		{
			return new ODataWriterBehavior(ODataBehaviorKind.WcfDataServicesClient, ODataBehaviorKind.WcfDataServicesClient, false, false, false, odataNamespace, typeScheme);
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x000363BD File Offset: 0x000345BD
		internal static ODataWriterBehavior CreateWcfDataServicesServerBehavior(bool usesV1Provider)
		{
			return new ODataWriterBehavior(ODataBehaviorKind.WcfDataServicesServer, ODataBehaviorKind.WcfDataServicesServer, usesV1Provider, true, true, "http://schemas.microsoft.com/ado/2007/08/dataservices", "http://schemas.microsoft.com/ado/2007/08/dataservices/scheme");
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x000363D3 File Offset: 0x000345D3
		internal void UseDefaultFormatBehavior()
		{
			this.formatBehaviorKind = ODataBehaviorKind.Default;
			this.usesV1Provider = false;
			this.allowNullValuesForNonNullablePrimitiveTypes = false;
			this.allowDuplicatePropertyNames = false;
			this.odataNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices";
			this.typeScheme = "http://schemas.microsoft.com/ado/2007/08/dataservices/scheme";
		}

		// Token: 0x04000559 RID: 1369
		private static readonly ODataWriterBehavior defaultWriterBehavior = new ODataWriterBehavior(ODataBehaviorKind.Default, ODataBehaviorKind.Default, false, false, false, "http://schemas.microsoft.com/ado/2007/08/dataservices", "http://schemas.microsoft.com/ado/2007/08/dataservices/scheme");

		// Token: 0x0400055A RID: 1370
		private readonly ODataBehaviorKind apiBehaviorKind;

		// Token: 0x0400055B RID: 1371
		private bool usesV1Provider;

		// Token: 0x0400055C RID: 1372
		private bool allowNullValuesForNonNullablePrimitiveTypes;

		// Token: 0x0400055D RID: 1373
		private bool allowDuplicatePropertyNames;

		// Token: 0x0400055E RID: 1374
		private string typeScheme;

		// Token: 0x0400055F RID: 1375
		private string odataNamespace;

		// Token: 0x04000560 RID: 1376
		private ODataBehaviorKind formatBehaviorKind;
	}
}
