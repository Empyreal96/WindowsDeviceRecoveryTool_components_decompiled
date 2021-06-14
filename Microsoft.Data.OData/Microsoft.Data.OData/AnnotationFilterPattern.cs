using System;

namespace Microsoft.Data.OData
{
	// Token: 0x0200012B RID: 299
	internal abstract class AnnotationFilterPattern : IComparable<AnnotationFilterPattern>
	{
		// Token: 0x060007E8 RID: 2024 RVA: 0x0001A4A7 File Offset: 0x000186A7
		private AnnotationFilterPattern(string pattern, bool isExclude)
		{
			this.isExclude = isExclude;
			this.Pattern = pattern;
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x060007E9 RID: 2025 RVA: 0x0001A4BD File Offset: 0x000186BD
		internal virtual bool IsExclude
		{
			get
			{
				return this.isExclude;
			}
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x0001A4C8 File Offset: 0x000186C8
		public int CompareTo(AnnotationFilterPattern other)
		{
			ExceptionUtils.CheckArgumentNotNull<AnnotationFilterPattern>(other, "other");
			int num = AnnotationFilterPattern.ComparePatternPriority(this.Pattern, other.Pattern);
			if (num != 0)
			{
				return num;
			}
			if (this.IsExclude == other.IsExclude)
			{
				return 0;
			}
			if (!this.IsExclude)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0001A514 File Offset: 0x00018714
		internal static AnnotationFilterPattern Create(string pattern)
		{
			AnnotationFilterPattern.ValidatePattern(pattern);
			bool flag = AnnotationFilterPattern.RemoveExcludeOperator(ref pattern);
			if (pattern == "*")
			{
				if (!flag)
				{
					return AnnotationFilterPattern.IncludeAllPattern;
				}
				return AnnotationFilterPattern.ExcludeAllPattern;
			}
			else
			{
				if (pattern.EndsWith(".*", StringComparison.Ordinal))
				{
					return new AnnotationFilterPattern.StartsWithPattern(pattern.Substring(0, pattern.Length - 1), flag);
				}
				return new AnnotationFilterPattern.ExactMatchPattern(pattern, flag);
			}
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0001A576 File Offset: 0x00018776
		internal static void Sort(AnnotationFilterPattern[] pattersToSort)
		{
			Array.Sort<AnnotationFilterPattern>(pattersToSort);
		}

		// Token: 0x060007ED RID: 2029
		internal abstract bool Matches(string annotationName);

		// Token: 0x060007EE RID: 2030 RVA: 0x0001A580 File Offset: 0x00018780
		private static int ComparePatternPriority(string pattern1, string pattern2)
		{
			if (pattern1 == pattern2)
			{
				return 0;
			}
			if (pattern1 == "*")
			{
				return 1;
			}
			if (pattern2 == "*")
			{
				return -1;
			}
			if (pattern1.StartsWith(pattern2, StringComparison.Ordinal))
			{
				return -1;
			}
			if (pattern2.StartsWith(pattern1, StringComparison.Ordinal))
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x0001A5CF File Offset: 0x000187CF
		private static bool RemoveExcludeOperator(ref string pattern)
		{
			if (pattern[0] == '-')
			{
				pattern = pattern.Substring(1);
				return true;
			}
			return false;
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x0001A5EC File Offset: 0x000187EC
		private static void ValidatePattern(string pattern)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(pattern, "pattern");
			string text = pattern;
			AnnotationFilterPattern.RemoveExcludeOperator(ref text);
			if (text == "*")
			{
				return;
			}
			string[] array = text.Split(new char[]
			{
				'.'
			});
			int num = array.Length;
			if (num == 1)
			{
				throw new ArgumentException(Strings.AnnotationFilterPattern_InvalidPatternMissingDot(pattern));
			}
			for (int i = 0; i < num; i++)
			{
				string text2 = array[i];
				if (string.IsNullOrEmpty(text2))
				{
					throw new ArgumentException(Strings.AnnotationFilterPattern_InvalidPatternEmptySegment(pattern));
				}
				if (text2 != "*" && text2.Contains("*"))
				{
					throw new ArgumentException(Strings.AnnotationFilterPattern_InvalidPatternWildCardInSegment(pattern));
				}
				bool flag = i + 1 == num;
				if (text2 == "*" && !flag)
				{
					throw new ArgumentException(Strings.AnnotationFilterPattern_InvalidPatternWildCardMustBeInLastSegment(pattern));
				}
			}
		}

		// Token: 0x04000302 RID: 770
		private const char NamespaceSeparator = '.';

		// Token: 0x04000303 RID: 771
		private const char ExcludeOperator = '-';

		// Token: 0x04000304 RID: 772
		private const string WildCard = "*";

		// Token: 0x04000305 RID: 773
		private const string DotStar = ".*";

		// Token: 0x04000306 RID: 774
		internal static readonly AnnotationFilterPattern IncludeAllPattern = new AnnotationFilterPattern.WildCardPattern(false);

		// Token: 0x04000307 RID: 775
		internal static readonly AnnotationFilterPattern ExcludeAllPattern = new AnnotationFilterPattern.WildCardPattern(true);

		// Token: 0x04000308 RID: 776
		protected readonly string Pattern;

		// Token: 0x04000309 RID: 777
		private readonly bool isExclude;

		// Token: 0x0200012C RID: 300
		private sealed class WildCardPattern : AnnotationFilterPattern
		{
			// Token: 0x060007F2 RID: 2034 RVA: 0x0001A6D3 File Offset: 0x000188D3
			internal WildCardPattern(bool isExclude) : base("*", isExclude)
			{
			}

			// Token: 0x060007F3 RID: 2035 RVA: 0x0001A6E1 File Offset: 0x000188E1
			internal override bool Matches(string annotationName)
			{
				return true;
			}
		}

		// Token: 0x0200012D RID: 301
		private sealed class StartsWithPattern : AnnotationFilterPattern
		{
			// Token: 0x060007F4 RID: 2036 RVA: 0x0001A6E4 File Offset: 0x000188E4
			internal StartsWithPattern(string pattern, bool isExclude) : base(pattern, isExclude)
			{
			}

			// Token: 0x060007F5 RID: 2037 RVA: 0x0001A6EE File Offset: 0x000188EE
			internal override bool Matches(string annotationName)
			{
				return annotationName.StartsWith(this.Pattern, StringComparison.Ordinal);
			}
		}

		// Token: 0x0200012E RID: 302
		private sealed class ExactMatchPattern : AnnotationFilterPattern
		{
			// Token: 0x060007F6 RID: 2038 RVA: 0x0001A6FD File Offset: 0x000188FD
			internal ExactMatchPattern(string pattern, bool isExclude) : base(pattern, isExclude)
			{
			}

			// Token: 0x060007F7 RID: 2039 RVA: 0x0001A707 File Offset: 0x00018907
			internal override bool Matches(string annotationName)
			{
				return annotationName.Equals(this.Pattern, StringComparison.Ordinal);
			}
		}
	}
}
