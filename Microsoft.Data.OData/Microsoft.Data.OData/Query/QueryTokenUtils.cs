using System;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x020000B6 RID: 182
	internal static class QueryTokenUtils
	{
		// Token: 0x06000472 RID: 1138 RVA: 0x0000E950 File Offset: 0x0000CB50
		internal static InlineCountKind? ParseInlineCountKind(string inlineCount)
		{
			if (inlineCount == null)
			{
				return null;
			}
			if (string.Equals(inlineCount, "allpages", StringComparison.OrdinalIgnoreCase))
			{
				return new InlineCountKind?(InlineCountKind.AllPages);
			}
			if (string.Equals(inlineCount, "none", StringComparison.OrdinalIgnoreCase))
			{
				return new InlineCountKind?(InlineCountKind.None);
			}
			throw new ODataException(Strings.SyntacticTree_InvalidInlineCountQueryOptionValue(inlineCount, string.Join(", ", new string[]
			{
				"none",
				"allpages"
			})));
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0000E9C4 File Offset: 0x0000CBC4
		internal static KeywordKind? ParseKeywordKind(string segment)
		{
			if (segment != null)
			{
				if (segment == "$metadata")
				{
					return new KeywordKind?(KeywordKind.Metadata);
				}
				if (segment == "$count")
				{
					return new KeywordKind?(KeywordKind.Count);
				}
				if (segment == "$value")
				{
					return new KeywordKind?(KeywordKind.Value);
				}
				if (segment == "$batch")
				{
					return new KeywordKind?(KeywordKind.Batch);
				}
				if (segment == "$links")
				{
					return new KeywordKind?(KeywordKind.Links);
				}
			}
			return null;
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0000EA48 File Offset: 0x0000CC48
		internal static string GetNameFromKeywordKind(KeywordKind keyword)
		{
			switch (keyword)
			{
			case KeywordKind.Metadata:
				return "$metadata";
			case KeywordKind.Value:
				return "$value";
			case KeywordKind.Batch:
				return "$batch";
			case KeywordKind.Links:
				return "$links";
			case KeywordKind.Count:
				return "$count";
			default:
				throw new InvalidOperationException("Should not have reached here with kind: " + keyword);
			}
		}
	}
}
