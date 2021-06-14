using System;
using System.Data.Services.Client.Materialization;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x02000081 RID: 129
	internal class ProjectionPlan
	{
		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000451 RID: 1105 RVA: 0x00011CD8 File Offset: 0x0000FED8
		// (set) Token: 0x06000452 RID: 1106 RVA: 0x00011CE0 File Offset: 0x0000FEE0
		internal Type LastSegmentType { get; set; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000453 RID: 1107 RVA: 0x00011CE9 File Offset: 0x0000FEE9
		// (set) Token: 0x06000454 RID: 1108 RVA: 0x00011CF1 File Offset: 0x0000FEF1
		internal Func<object, object, Type, object> Plan { get; set; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x00011CFA File Offset: 0x0000FEFA
		// (set) Token: 0x06000456 RID: 1110 RVA: 0x00011D02 File Offset: 0x0000FF02
		internal Type ProjectedType { get; set; }

		// Token: 0x06000457 RID: 1111 RVA: 0x00011D0B File Offset: 0x0000FF0B
		internal object Run(ODataEntityMaterializer materializer, ODataEntry entry, Type expectedType)
		{
			return this.Plan(materializer, entry, expectedType);
		}
	}
}
