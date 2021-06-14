using System;

namespace Microsoft.Data.OData
{
	// Token: 0x02000256 RID: 598
	internal static class WriterUtils
	{
		// Token: 0x060013B4 RID: 5044 RVA: 0x00049D66 File Offset: 0x00047F66
		internal static bool ShouldSkipProperty(this ProjectedPropertiesAnnotation projectedProperties, string propertyName)
		{
			return projectedProperties != null && (object.ReferenceEquals(ProjectedPropertiesAnnotation.EmptyProjectedPropertiesInstance, projectedProperties) || (!object.ReferenceEquals(ProjectedPropertiesAnnotation.AllProjectedPropertiesInstance, projectedProperties) && !projectedProperties.IsPropertyProjected(propertyName)));
		}
	}
}
