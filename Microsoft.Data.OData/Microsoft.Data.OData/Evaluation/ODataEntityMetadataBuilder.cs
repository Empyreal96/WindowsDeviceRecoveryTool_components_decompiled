using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.OData.JsonLight;

namespace Microsoft.Data.OData.Evaluation
{
	// Token: 0x02000100 RID: 256
	internal abstract class ODataEntityMetadataBuilder
	{
		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x000182E7 File Offset: 0x000164E7
		internal static ODataEntityMetadataBuilder Null
		{
			get
			{
				return ODataEntityMetadataBuilder.NullEntityMetadataBuilder.Instance;
			}
		}

		// Token: 0x060006DE RID: 1758
		internal abstract Uri GetEditLink();

		// Token: 0x060006DF RID: 1759
		internal abstract Uri GetReadLink();

		// Token: 0x060006E0 RID: 1760
		internal abstract string GetId();

		// Token: 0x060006E1 RID: 1761
		internal abstract string GetETag();

		// Token: 0x060006E2 RID: 1762 RVA: 0x000182EE File Offset: 0x000164EE
		internal virtual ODataStreamReferenceValue GetMediaResource()
		{
			return null;
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x00018304 File Offset: 0x00016504
		internal virtual IEnumerable<ODataProperty> GetProperties(IEnumerable<ODataProperty> nonComputedProperties)
		{
			if (nonComputedProperties != null)
			{
				return from p in nonComputedProperties
				where !(p.ODataValue is ODataStreamReferenceValue)
				select p;
			}
			return null;
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0001832E File Offset: 0x0001652E
		internal virtual IEnumerable<ODataAction> GetActions()
		{
			return null;
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x00018331 File Offset: 0x00016531
		internal virtual IEnumerable<ODataFunction> GetFunctions()
		{
			return null;
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00018334 File Offset: 0x00016534
		internal virtual void MarkNavigationLinkProcessed(string navigationPropertyName)
		{
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00018336 File Offset: 0x00016536
		internal virtual ODataJsonLightReaderNavigationLinkInfo GetNextUnprocessedNavigationLink()
		{
			return null;
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00018339 File Offset: 0x00016539
		internal virtual Uri GetStreamEditLink(string streamPropertyName)
		{
			ExceptionUtils.CheckArgumentStringNotEmpty(streamPropertyName, "streamPropertyName");
			return null;
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00018347 File Offset: 0x00016547
		internal virtual Uri GetStreamReadLink(string streamPropertyName)
		{
			ExceptionUtils.CheckArgumentStringNotEmpty(streamPropertyName, "streamPropertyName");
			return null;
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00018355 File Offset: 0x00016555
		internal virtual Uri GetNavigationLinkUri(string navigationPropertyName, Uri navigationLinkUrl, bool hasNavigationLinkUrl)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(navigationPropertyName, "navigationPropertyName");
			return null;
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x00018363 File Offset: 0x00016563
		internal virtual Uri GetAssociationLinkUri(string navigationPropertyName, Uri associationLinkUrl, bool hasAssociationLinkUrl)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(navigationPropertyName, "navigationPropertyName");
			return null;
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x00018371 File Offset: 0x00016571
		internal virtual Uri GetOperationTargetUri(string operationName, string bindingParameterTypeName)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(operationName, "operationName");
			return null;
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0001837F File Offset: 0x0001657F
		internal virtual string GetOperationTitle(string operationName)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(operationName, "operationName");
			return null;
		}

		// Token: 0x02000101 RID: 257
		private sealed class NullEntityMetadataBuilder : ODataEntityMetadataBuilder
		{
			// Token: 0x060006F0 RID: 1776 RVA: 0x00018395 File Offset: 0x00016595
			private NullEntityMetadataBuilder()
			{
			}

			// Token: 0x060006F1 RID: 1777 RVA: 0x0001839D File Offset: 0x0001659D
			internal override Uri GetEditLink()
			{
				return null;
			}

			// Token: 0x060006F2 RID: 1778 RVA: 0x000183A0 File Offset: 0x000165A0
			internal override Uri GetReadLink()
			{
				return null;
			}

			// Token: 0x060006F3 RID: 1779 RVA: 0x000183A3 File Offset: 0x000165A3
			internal override string GetId()
			{
				return null;
			}

			// Token: 0x060006F4 RID: 1780 RVA: 0x000183A6 File Offset: 0x000165A6
			internal override string GetETag()
			{
				return null;
			}

			// Token: 0x040002A3 RID: 675
			internal static readonly ODataEntityMetadataBuilder.NullEntityMetadataBuilder Instance = new ODataEntityMetadataBuilder.NullEntityMetadataBuilder();
		}
	}
}
