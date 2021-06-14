using System;
using System.Linq;

namespace Microsoft.Data.OData
{
	// Token: 0x02000128 RID: 296
	internal class AnnotationFilter
	{
		// Token: 0x060007DF RID: 2015 RVA: 0x0001A357 File Offset: 0x00018557
		private AnnotationFilter(AnnotationFilterPattern[] prioritizedPatternsToMatch)
		{
			this.prioritizedPatternsToMatch = prioritizedPatternsToMatch;
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0001A374 File Offset: 0x00018574
		internal static AnnotationFilter Create(string filter)
		{
			if (string.IsNullOrEmpty(filter))
			{
				return AnnotationFilter.ExcludeAll;
			}
			AnnotationFilterPattern[] array = (from pattern in filter.Split(AnnotationFilter.AnnotationFilterPatternSeparator)
			select AnnotationFilterPattern.Create(pattern.Trim())).ToArray<AnnotationFilterPattern>();
			AnnotationFilterPattern.Sort(array);
			if (array[0] == AnnotationFilterPattern.IncludeAllPattern)
			{
				return AnnotationFilter.IncludeAll;
			}
			if (array[0] == AnnotationFilterPattern.ExcludeAllPattern)
			{
				return AnnotationFilter.ExcludeAll;
			}
			return new AnnotationFilter(array);
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x0001A3F0 File Offset: 0x000185F0
		internal virtual bool Matches(string annotationName)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(annotationName, "annotationName");
			foreach (AnnotationFilterPattern annotationFilterPattern in this.prioritizedPatternsToMatch)
			{
				if (annotationFilterPattern.Matches(annotationName))
				{
					return !annotationFilterPattern.IsExclude;
				}
			}
			return false;
		}

		// Token: 0x040002FD RID: 765
		private static readonly AnnotationFilter IncludeAll = new AnnotationFilter.IncludeAllFilter();

		// Token: 0x040002FE RID: 766
		private static readonly AnnotationFilter ExcludeAll = new AnnotationFilter.ExcludeAllFilter();

		// Token: 0x040002FF RID: 767
		private static readonly char[] AnnotationFilterPatternSeparator = new char[]
		{
			','
		};

		// Token: 0x04000300 RID: 768
		private readonly AnnotationFilterPattern[] prioritizedPatternsToMatch;

		// Token: 0x02000129 RID: 297
		private sealed class IncludeAllFilter : AnnotationFilter
		{
			// Token: 0x060007E4 RID: 2020 RVA: 0x0001A46F File Offset: 0x0001866F
			internal IncludeAllFilter() : base(new AnnotationFilterPattern[0])
			{
			}

			// Token: 0x060007E5 RID: 2021 RVA: 0x0001A47D File Offset: 0x0001867D
			internal override bool Matches(string annotationName)
			{
				ExceptionUtils.CheckArgumentStringNotNullOrEmpty(annotationName, "annotationName");
				return true;
			}
		}

		// Token: 0x0200012A RID: 298
		private sealed class ExcludeAllFilter : AnnotationFilter
		{
			// Token: 0x060007E6 RID: 2022 RVA: 0x0001A48B File Offset: 0x0001868B
			internal ExcludeAllFilter() : base(new AnnotationFilterPattern[0])
			{
			}

			// Token: 0x060007E7 RID: 2023 RVA: 0x0001A499 File Offset: 0x00018699
			internal override bool Matches(string annotationName)
			{
				ExceptionUtils.CheckArgumentStringNotNullOrEmpty(annotationName, "annotationName");
				return false;
			}
		}
	}
}
