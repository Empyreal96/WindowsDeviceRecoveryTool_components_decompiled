using System;

namespace Microsoft.Data.Edm.Library
{
	// Token: 0x020001E7 RID: 487
	public static class EdmConstants
	{
		// Token: 0x04000555 RID: 1365
		internal const string EdmNamespace = "Edm";

		// Token: 0x04000556 RID: 1366
		internal const string TransientNamespace = "Transient";

		// Token: 0x04000557 RID: 1367
		internal const string XmlPrefix = "xml";

		// Token: 0x04000558 RID: 1368
		internal const string XmlNamespacePrefix = "xmlns";

		// Token: 0x04000559 RID: 1369
		internal const string DocumentationUri = "http://schemas.microsoft.com/ado/2011/04/edm/documentation";

		// Token: 0x0400055A RID: 1370
		internal const string DocumentationAnnotation = "Documentation";

		// Token: 0x0400055B RID: 1371
		internal const string InternalUri = "http://schemas.microsoft.com/ado/2011/04/edm/internal";

		// Token: 0x0400055C RID: 1372
		internal const string DataServiceVersion = "DataServiceVersion";

		// Token: 0x0400055D RID: 1373
		internal const string MaxDataServiceVersion = "MaxDataServiceVersion";

		// Token: 0x0400055E RID: 1374
		internal const string EdmVersionAnnotation = "EdmVersion";

		// Token: 0x0400055F RID: 1375
		internal const string FacetName_Nullable = "Nullable";

		// Token: 0x04000560 RID: 1376
		internal const string FacetName_Precision = "Precision";

		// Token: 0x04000561 RID: 1377
		internal const string FacetName_Scale = "Scale";

		// Token: 0x04000562 RID: 1378
		internal const string FacetName_MaxLength = "MaxLength";

		// Token: 0x04000563 RID: 1379
		internal const string FacetName_FixedLength = "FixedLength";

		// Token: 0x04000564 RID: 1380
		internal const string FacetName_Unicode = "Unicode";

		// Token: 0x04000565 RID: 1381
		internal const string FacetName_Collation = "Collation";

		// Token: 0x04000566 RID: 1382
		internal const string FacetName_Srid = "SRID";

		// Token: 0x04000567 RID: 1383
		internal const string Value_UnknownType = "UnknownType";

		// Token: 0x04000568 RID: 1384
		internal const string Value_UnnamedType = "UnnamedType";

		// Token: 0x04000569 RID: 1385
		internal const string Value_Max = "Max";

		// Token: 0x0400056A RID: 1386
		internal const string Value_SridVariable = "Variable";

		// Token: 0x0400056B RID: 1387
		internal const string Type_Association = "Association";

		// Token: 0x0400056C RID: 1388
		internal const string Type_Collection = "Collection";

		// Token: 0x0400056D RID: 1389
		internal const string Type_Complex = "Complex";

		// Token: 0x0400056E RID: 1390
		internal const string Type_Entity = "Entity";

		// Token: 0x0400056F RID: 1391
		internal const string Type_EntityReference = "EntityReference";

		// Token: 0x04000570 RID: 1392
		internal const string Type_Enum = "Enum";

		// Token: 0x04000571 RID: 1393
		internal const string Type_Row = "Row";

		// Token: 0x04000572 RID: 1394
		internal const string Type_Primitive = "Primitive";

		// Token: 0x04000573 RID: 1395
		internal const string Type_Binary = "Binary";

		// Token: 0x04000574 RID: 1396
		internal const string Type_Decimal = "Decimal";

		// Token: 0x04000575 RID: 1397
		internal const string Type_String = "String";

		// Token: 0x04000576 RID: 1398
		internal const string Type_Stream = "Stream";

		// Token: 0x04000577 RID: 1399
		internal const string Type_Spatial = "Spatial";

		// Token: 0x04000578 RID: 1400
		internal const string Type_Temporal = "Temporal";

		// Token: 0x04000579 RID: 1401
		internal const string Type_Structured = "Structured";

		// Token: 0x0400057A RID: 1402
		internal const int Max_Precision = 2147483647;

		// Token: 0x0400057B RID: 1403
		internal const int Min_Precision = 0;

		// Token: 0x0400057C RID: 1404
		public static readonly Version EdmVersion1 = new Version(1, 0);

		// Token: 0x0400057D RID: 1405
		public static readonly Version EdmVersion1_1 = new Version(1, 1);

		// Token: 0x0400057E RID: 1406
		public static readonly Version EdmVersion1_2 = new Version(1, 2);

		// Token: 0x0400057F RID: 1407
		public static readonly Version EdmVersion2 = new Version(2, 0);

		// Token: 0x04000580 RID: 1408
		public static readonly Version EdmVersion3 = new Version(3, 0);

		// Token: 0x04000581 RID: 1409
		public static readonly Version EdmVersionLatest = EdmConstants.EdmVersion3;
	}
}
