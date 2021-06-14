using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData
{
	// Token: 0x0200023F RID: 575
	public sealed class ProjectedPropertiesAnnotation
	{
		// Token: 0x0600125C RID: 4700 RVA: 0x00044E57 File Offset: 0x00043057
		public ProjectedPropertiesAnnotation(IEnumerable<string> projectedPropertyNames)
		{
			ExceptionUtils.CheckArgumentNotNull<IEnumerable<string>>(projectedPropertyNames, "projectedPropertyNames");
			this.projectedProperties = new HashSet<string>(projectedPropertyNames, StringComparer.Ordinal);
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x00044E7B File Offset: 0x0004307B
		internal ProjectedPropertiesAnnotation()
		{
			this.projectedProperties = new HashSet<string>(StringComparer.Ordinal);
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x0600125E RID: 4702 RVA: 0x00044E93 File Offset: 0x00043093
		internal static ProjectedPropertiesAnnotation EmptyProjectedPropertiesInstance
		{
			get
			{
				return ProjectedPropertiesAnnotation.emptyProjectedPropertiesMarker;
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x0600125F RID: 4703 RVA: 0x00044E9A File Offset: 0x0004309A
		internal static ProjectedPropertiesAnnotation AllProjectedPropertiesInstance
		{
			get
			{
				return ProjectedPropertiesAnnotation.allProjectedPropertiesMarker;
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06001260 RID: 4704 RVA: 0x00044EA1 File Offset: 0x000430A1
		internal IEnumerable<string> ProjectedProperties
		{
			get
			{
				return this.projectedProperties;
			}
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x00044EA9 File Offset: 0x000430A9
		internal bool IsPropertyProjected(string propertyName)
		{
			return this.projectedProperties.Contains(propertyName);
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x00044EB7 File Offset: 0x000430B7
		internal void Add(string propertyName)
		{
			if (object.ReferenceEquals(ProjectedPropertiesAnnotation.AllProjectedPropertiesInstance, this))
			{
				return;
			}
			if (!this.projectedProperties.Contains(propertyName))
			{
				this.projectedProperties.Add(propertyName);
			}
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x00044EE2 File Offset: 0x000430E2
		internal void Remove(string propertyName)
		{
			this.projectedProperties.Remove(propertyName);
		}

		// Token: 0x040006A6 RID: 1702
		internal const string StarSegment = "*";

		// Token: 0x040006A7 RID: 1703
		private static readonly ProjectedPropertiesAnnotation emptyProjectedPropertiesMarker = new ProjectedPropertiesAnnotation(new string[0]);

		// Token: 0x040006A8 RID: 1704
		private static readonly ProjectedPropertiesAnnotation allProjectedPropertiesMarker = new ProjectedPropertiesAnnotation(new string[]
		{
			"*"
		});

		// Token: 0x040006A9 RID: 1705
		private readonly HashSet<string> projectedProperties;
	}
}
