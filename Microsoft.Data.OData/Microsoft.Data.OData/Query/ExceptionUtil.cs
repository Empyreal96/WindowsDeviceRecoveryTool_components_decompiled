using System;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x0200000F RID: 15
	internal static class ExceptionUtil
	{
		// Token: 0x06000055 RID: 85 RVA: 0x00002EEB File Offset: 0x000010EB
		internal static ODataException CreateResourceNotFound(string identifier)
		{
			return ExceptionUtil.ResourceNotFoundError(Strings.RequestUriProcessor_ResourceNotFound(identifier));
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002EF8 File Offset: 0x000010F8
		internal static ODataException ResourceNotFoundError(string errorMessage)
		{
			return new ODataUnrecognizedPathException(errorMessage);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002F00 File Offset: 0x00001100
		internal static ODataException CreateSyntaxError()
		{
			return ExceptionUtil.CreateBadRequestError(Strings.RequestUriProcessor_SyntaxError);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002F0C File Offset: 0x0000110C
		internal static ODataException CreateBadRequestError(string message)
		{
			return new ODataException(message);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002F14 File Offset: 0x00001114
		internal static void ThrowSyntaxErrorIfNotValid(bool valid)
		{
			if (!valid)
			{
				throw ExceptionUtil.CreateSyntaxError();
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002F1F File Offset: 0x0000111F
		internal static void ThrowIfResourceDoesNotExist(bool resourceExists, string identifier)
		{
			if (!resourceExists)
			{
				throw ExceptionUtil.CreateResourceNotFound(identifier);
			}
		}
	}
}
