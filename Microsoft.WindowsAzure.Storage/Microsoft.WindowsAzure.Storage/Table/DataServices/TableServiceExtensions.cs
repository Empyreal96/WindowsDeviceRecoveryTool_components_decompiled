using System;
using System.Data.Services.Client;
using System.Linq;

namespace Microsoft.WindowsAzure.Storage.Table.DataServices
{
	// Token: 0x02000047 RID: 71
	[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
	public static class TableServiceExtensions
	{
		// Token: 0x06000C8F RID: 3215 RVA: 0x0002CFA7 File Offset: 0x0002B1A7
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		public static TableServiceQuery<TElement> AsTableServiceQuery<TElement>(this IQueryable<TElement> query, TableServiceContext context)
		{
			return new TableServiceQuery<TElement>(query as DataServiceQuery<TElement>, context);
		}
	}
}
