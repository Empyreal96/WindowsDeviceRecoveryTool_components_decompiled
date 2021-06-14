using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData.Evaluation
{
	// Token: 0x02000137 RID: 311
	internal abstract class ODataUriBuilder
	{
		// Token: 0x06000847 RID: 2119 RVA: 0x0001B39C File Offset: 0x0001959C
		internal virtual Uri BuildBaseUri()
		{
			return null;
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0001B39F File Offset: 0x0001959F
		internal virtual Uri BuildEntitySetUri(Uri baseUri, string entitySetName)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(entitySetName, "entitySetName");
			return null;
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0001B3AD File Offset: 0x000195AD
		internal virtual Uri BuildEntityInstanceUri(Uri baseUri, ICollection<KeyValuePair<string, object>> keyProperties, string entityTypeName)
		{
			ExceptionUtils.CheckArgumentNotNull<ICollection<KeyValuePair<string, object>>>(keyProperties, "keyProperties");
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(entityTypeName, "entityTypeName");
			return null;
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0001B3C6 File Offset: 0x000195C6
		internal virtual Uri BuildStreamEditLinkUri(Uri baseUri, string streamPropertyName)
		{
			ExceptionUtils.CheckArgumentStringNotEmpty(streamPropertyName, "streamPropertyName");
			return null;
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0001B3D4 File Offset: 0x000195D4
		internal virtual Uri BuildStreamReadLinkUri(Uri baseUri, string streamPropertyName)
		{
			ExceptionUtils.CheckArgumentStringNotEmpty(streamPropertyName, "streamPropertyName");
			return null;
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0001B3E2 File Offset: 0x000195E2
		internal virtual Uri BuildNavigationLinkUri(Uri baseUri, string navigationPropertyName)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(navigationPropertyName, "navigationPropertyName");
			return null;
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0001B3F0 File Offset: 0x000195F0
		internal virtual Uri BuildAssociationLinkUri(Uri baseUri, string navigationPropertyName)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(navigationPropertyName, "navigationPropertyName");
			return null;
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0001B3FE File Offset: 0x000195FE
		internal virtual Uri BuildOperationTargetUri(Uri baseUri, string operationName, string bindingParameterTypeName)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(operationName, "operationName");
			return null;
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0001B40C File Offset: 0x0001960C
		internal virtual Uri AppendTypeSegment(Uri baseUri, string typeName)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(typeName, "typeName");
			return null;
		}
	}
}
