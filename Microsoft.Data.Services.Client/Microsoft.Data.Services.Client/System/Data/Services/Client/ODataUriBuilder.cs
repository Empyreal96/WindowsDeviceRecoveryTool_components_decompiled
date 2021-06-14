using System;
using Microsoft.Data.Edm.Values;

namespace System.Data.Services.Client
{
	// Token: 0x02000005 RID: 5
	internal abstract class ODataUriBuilder
	{
		// Token: 0x06000015 RID: 21 RVA: 0x0000260A File Offset: 0x0000080A
		internal virtual Uri BuildBaseUri()
		{
			return null;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000260D File Offset: 0x0000080D
		internal virtual Uri BuildEntitySetUri(Uri baseUri, string entitySetName)
		{
			Util.CheckArgumentNullAndEmpty(entitySetName, "entitySetName");
			return null;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000261B File Offset: 0x0000081B
		internal virtual Uri BuildEntityInstanceUri(Uri baseUri, IEdmStructuredValue entityInstance)
		{
			Util.CheckArgumentNull<IEdmStructuredValue>(entityInstance, "entityInstance");
			return null;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000262A File Offset: 0x0000082A
		internal virtual Uri BuildStreamEditLinkUri(Uri baseUri, string streamPropertyName)
		{
			Util.CheckArgumentNotEmpty(streamPropertyName, "streamPropertyName");
			return null;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002638 File Offset: 0x00000838
		internal virtual Uri BuildStreamReadLinkUri(Uri baseUri, string streamPropertyName)
		{
			Util.CheckArgumentNotEmpty(streamPropertyName, "streamPropertyName");
			return null;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002646 File Offset: 0x00000846
		internal virtual Uri BuildNavigationLinkUri(Uri baseUri, string navigationPropertyName)
		{
			Util.CheckArgumentNullAndEmpty(navigationPropertyName, "navigationPropertyName");
			return null;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002654 File Offset: 0x00000854
		internal virtual Uri BuildAssociationLinkUri(Uri baseUri, string navigationPropertyName)
		{
			Util.CheckArgumentNullAndEmpty(navigationPropertyName, "navigationPropertyName");
			return null;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002662 File Offset: 0x00000862
		internal virtual Uri BuildOperationTargetUri(Uri baseUri, string operationName, string bindingParameterTypeName)
		{
			Util.CheckArgumentNullAndEmpty(operationName, "operationName");
			return null;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002670 File Offset: 0x00000870
		internal virtual Uri AppendTypeSegment(Uri baseUri, string typeName)
		{
			Util.CheckArgumentNullAndEmpty(typeName, "typeName");
			return null;
		}
	}
}
