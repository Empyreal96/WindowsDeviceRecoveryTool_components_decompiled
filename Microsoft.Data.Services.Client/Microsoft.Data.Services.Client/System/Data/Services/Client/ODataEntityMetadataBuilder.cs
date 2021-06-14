using System;

namespace System.Data.Services.Client
{
	// Token: 0x02000004 RID: 4
	internal abstract class ODataEntityMetadataBuilder
	{
		// Token: 0x0600000A RID: 10
		internal abstract Uri GetEditLink();

		// Token: 0x0600000B RID: 11
		internal abstract Uri GetReadLink();

		// Token: 0x0600000C RID: 12
		internal abstract string GetId();

		// Token: 0x0600000D RID: 13
		internal abstract string GetETag();

		// Token: 0x0600000E RID: 14 RVA: 0x000025AE File Offset: 0x000007AE
		internal virtual Uri GetStreamEditLink(string streamPropertyName)
		{
			Util.CheckArgumentNotEmpty(streamPropertyName, "streamPropertyName");
			return null;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000025BC File Offset: 0x000007BC
		internal virtual Uri GetStreamReadLink(string streamPropertyName)
		{
			Util.CheckArgumentNotEmpty(streamPropertyName, "streamPropertyName");
			return null;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000025CA File Offset: 0x000007CA
		internal virtual Uri GetNavigationLinkUri(string navigationPropertyName, Uri navigationLinkUrl, bool hasNavigationLinkUrl)
		{
			Util.CheckArgumentNullAndEmpty(navigationPropertyName, "navigationPropertyName");
			return null;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000025D8 File Offset: 0x000007D8
		internal virtual Uri GetAssociationLinkUri(string navigationPropertyName, Uri associationLinkUrl, bool hasAssociationLinkUrl)
		{
			Util.CheckArgumentNullAndEmpty(navigationPropertyName, "navigationPropertyName");
			return null;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000025E6 File Offset: 0x000007E6
		internal virtual Uri GetOperationTargetUri(string operationName, string bindingParameterTypeName)
		{
			Util.CheckArgumentNullAndEmpty(operationName, "operationName");
			return null;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000025F4 File Offset: 0x000007F4
		internal virtual string GetOperationTitle(string operationName)
		{
			Util.CheckArgumentNullAndEmpty(operationName, "operationName");
			return null;
		}
	}
}
