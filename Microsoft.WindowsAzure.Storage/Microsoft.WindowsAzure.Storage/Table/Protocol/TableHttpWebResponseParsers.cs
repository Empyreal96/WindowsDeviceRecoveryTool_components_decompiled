using System;
using System.IO;
using System.Net;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Table.Protocol
{
	// Token: 0x0200004C RID: 76
	public static class TableHttpWebResponseParsers
	{
		// Token: 0x06000CC9 RID: 3273 RVA: 0x0002D6DA File Offset: 0x0002B8DA
		public static string GetRequestId(HttpWebResponse response)
		{
			return Response.GetRequestId(response);
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x0002D6E2 File Offset: 0x0002B8E2
		public static ServiceProperties ReadServiceProperties(Stream inputStream)
		{
			return HttpResponseParsers.ReadServiceProperties(inputStream);
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x0002D6EA File Offset: 0x0002B8EA
		public static ServiceStats ReadServiceStats(Stream inputStream)
		{
			return HttpResponseParsers.ReadServiceStats(inputStream);
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x0002D6F2 File Offset: 0x0002B8F2
		public static void ReadSharedAccessIdentifiers(Stream inputStream, TablePermissions permissions)
		{
			CommonUtility.AssertNotNull("permissions", permissions);
			HttpResponseParsers.ReadSharedAccessIdentifiers<SharedAccessTablePolicy>(permissions.SharedAccessPolicies, new TableAccessPolicyResponse(inputStream));
		}
	}
}
