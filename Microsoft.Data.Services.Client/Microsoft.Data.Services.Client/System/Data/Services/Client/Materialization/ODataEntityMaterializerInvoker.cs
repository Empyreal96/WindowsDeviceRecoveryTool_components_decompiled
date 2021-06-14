using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Data.OData;

namespace System.Data.Services.Client.Materialization
{
	// Token: 0x0200006E RID: 110
	internal static class ODataEntityMaterializerInvoker
	{
		// Token: 0x060003AB RID: 939 RVA: 0x0000FF30 File Offset: 0x0000E130
		internal static IEnumerable<T> EnumerateAsElementType<T>(IEnumerable source)
		{
			return ODataEntityMaterializer.EnumerateAsElementType<T>(source);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000FF38 File Offset: 0x0000E138
		internal static List<TTarget> ListAsElementType<T, TTarget>(object materializer, IEnumerable<T> source) where T : TTarget
		{
			return ODataEntityMaterializer.ListAsElementType<T, TTarget>((ODataEntityMaterializer)materializer, source);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000FF46 File Offset: 0x0000E146
		internal static bool ProjectionCheckValueForPathIsNull(object entry, Type expectedType, object path)
		{
			return ODataEntityMaterializer.ProjectionCheckValueForPathIsNull(MaterializerEntry.GetEntry((ODataEntry)entry), expectedType, (ProjectionPath)path);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000FF5F File Offset: 0x0000E15F
		internal static IEnumerable ProjectionSelect(object materializer, object entry, Type expectedType, Type resultType, object path, Func<object, object, Type, object> selector)
		{
			return ODataEntityMaterializer.ProjectionSelect((ODataEntityMaterializer)materializer, MaterializerEntry.GetEntry((ODataEntry)entry), expectedType, resultType, (ProjectionPath)path, selector);
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000FF82 File Offset: 0x0000E182
		internal static object ProjectionGetEntry(object entry, string name)
		{
			return ODataEntityMaterializer.ProjectionGetEntry(MaterializerEntry.GetEntry((ODataEntry)entry), name);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000FF95 File Offset: 0x0000E195
		internal static object ProjectionInitializeEntity(object materializer, object entry, Type expectedType, Type resultType, string[] properties, Func<object, object, Type, object>[] propertyValues)
		{
			return ODataEntityMaterializer.ProjectionInitializeEntity((ODataEntityMaterializer)materializer, MaterializerEntry.GetEntry((ODataEntry)entry), expectedType, resultType, properties, propertyValues);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000FFB3 File Offset: 0x0000E1B3
		internal static object ProjectionValueForPath(object materializer, object entry, Type expectedType, object path)
		{
			return ((ODataEntityMaterializer)materializer).ProjectionValueForPath(MaterializerEntry.GetEntry((ODataEntry)entry), expectedType, (ProjectionPath)path);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000FFD2 File Offset: 0x0000E1D2
		internal static object DirectMaterializePlan(object materializer, object entry, Type expectedEntryType)
		{
			return ODataEntityMaterializer.DirectMaterializePlan((ODataEntityMaterializer)materializer, MaterializerEntry.GetEntry((ODataEntry)entry), expectedEntryType);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000FFEB File Offset: 0x0000E1EB
		internal static object ShallowMaterializePlan(object materializer, object entry, Type expectedEntryType)
		{
			return ODataEntityMaterializer.ShallowMaterializePlan((ODataEntityMaterializer)materializer, MaterializerEntry.GetEntry((ODataEntry)entry), expectedEntryType);
		}
	}
}
