using System;
using Microsoft.Data.OData;

namespace System.Data.Services.Client.Materialization
{
	// Token: 0x0200006F RID: 111
	internal static class ODataItemExtensions
	{
		// Token: 0x060003B4 RID: 948 RVA: 0x00010004 File Offset: 0x0000E204
		public static object GetMaterializedValue(this ODataProperty property)
		{
			ODataAnnotatable annotatableObject = (property.Value as ODataAnnotatable) ?? property;
			return ODataItemExtensions.GetMaterializedValueCore(annotatableObject);
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00010028 File Offset: 0x0000E228
		public static bool HasMaterializedValue(this ODataProperty property)
		{
			ODataAnnotatable annotatableObject = (property.Value as ODataAnnotatable) ?? property;
			return ODataItemExtensions.HasMaterializedValueCore(annotatableObject);
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0001004C File Offset: 0x0000E24C
		public static void SetMaterializedValue(this ODataProperty property, object materializedValue)
		{
			ODataAnnotatable annotatableObject = (property.Value as ODataAnnotatable) ?? property;
			ODataItemExtensions.SetMaterializedValueCore(annotatableObject, materializedValue);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00010071 File Offset: 0x0000E271
		public static object GetMaterializedValue(this ODataComplexValue complexValue)
		{
			return ODataItemExtensions.GetMaterializedValueCore(complexValue);
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00010079 File Offset: 0x0000E279
		public static bool HasMaterializedValue(this ODataComplexValue complexValue)
		{
			return ODataItemExtensions.HasMaterializedValueCore(complexValue);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00010081 File Offset: 0x0000E281
		public static void SetMaterializedValue(this ODataComplexValue complexValue, object materializedValue)
		{
			ODataItemExtensions.SetMaterializedValueCore(complexValue, materializedValue);
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0001008C File Offset: 0x0000E28C
		private static object GetMaterializedValueCore(ODataAnnotatable annotatableObject)
		{
			ODataItemExtensions.MaterializerPropertyValue annotation = annotatableObject.GetAnnotation<ODataItemExtensions.MaterializerPropertyValue>();
			return annotation.Value;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x000100A6 File Offset: 0x0000E2A6
		private static bool HasMaterializedValueCore(ODataAnnotatable annotatableObject)
		{
			return annotatableObject.GetAnnotation<ODataItemExtensions.MaterializerPropertyValue>() != null;
		}

		// Token: 0x060003BC RID: 956 RVA: 0x000100B4 File Offset: 0x0000E2B4
		private static void SetMaterializedValueCore(ODataAnnotatable annotatableObject, object materializedValue)
		{
			ODataItemExtensions.MaterializerPropertyValue annotation = new ODataItemExtensions.MaterializerPropertyValue
			{
				Value = materializedValue
			};
			annotatableObject.SetAnnotation<ODataItemExtensions.MaterializerPropertyValue>(annotation);
		}

		// Token: 0x02000070 RID: 112
		private class MaterializerPropertyValue
		{
			// Token: 0x170000F5 RID: 245
			// (get) Token: 0x060003BD RID: 957 RVA: 0x000100D7 File Offset: 0x0000E2D7
			// (set) Token: 0x060003BE RID: 958 RVA: 0x000100DF File Offset: 0x0000E2DF
			public object Value { get; set; }
		}
	}
}
