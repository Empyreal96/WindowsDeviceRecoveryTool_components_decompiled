using System;
using Microsoft.Data.OData;

namespace Microsoft.WindowsAzure.Storage.Table.Protocol
{
	// Token: 0x02000151 RID: 337
	public static class TableConstants
	{
		// Token: 0x0400082E RID: 2094
		public const int TableServiceBatchMaximumOperations = 100;

		// Token: 0x0400082F RID: 2095
		public const string TableServicePrefixForTableContinuation = "x-ms-continuation-";

		// Token: 0x04000830 RID: 2096
		public const string TableServiceNextPartitionKey = "NextPartitionKey";

		// Token: 0x04000831 RID: 2097
		public const string TableServiceNextRowKey = "NextRowKey";

		// Token: 0x04000832 RID: 2098
		public const string TableServiceNextTableName = "NextTableName";

		// Token: 0x04000833 RID: 2099
		public const int TableServiceMaxResults = 1000;

		// Token: 0x04000834 RID: 2100
		public const int TableServiceMaxStringPropertySizeInBytes = 65536;

		// Token: 0x04000835 RID: 2101
		public const long TableServiceMaxPayload = 20971520L;

		// Token: 0x04000836 RID: 2102
		public const int TableServiceMaxStringPropertySizeInChars = 32768;

		// Token: 0x04000837 RID: 2103
		public const string TableServiceTablesName = "Tables";

		// Token: 0x04000838 RID: 2104
		public const string PartitionKey = "PartitionKey";

		// Token: 0x04000839 RID: 2105
		public const string RowKey = "RowKey";

		// Token: 0x0400083A RID: 2106
		public const string Timestamp = "Timestamp";

		// Token: 0x0400083B RID: 2107
		public const string Etag = "ETag";

		// Token: 0x0400083C RID: 2108
		public const string TableName = "TableName";

		// Token: 0x0400083D RID: 2109
		public const string Filter = "$filter";

		// Token: 0x0400083E RID: 2110
		public const string Top = "$top";

		// Token: 0x0400083F RID: 2111
		public const string Select = "$select";

		// Token: 0x04000840 RID: 2112
		public static readonly DateTimeOffset MinDateTime = new DateTimeOffset(1601, 1, 1, 0, 0, 0, TimeSpan.Zero);

		// Token: 0x04000841 RID: 2113
		internal static ODataVersion ODataProtocolVersion = ODataVersion.V3;
	}
}
