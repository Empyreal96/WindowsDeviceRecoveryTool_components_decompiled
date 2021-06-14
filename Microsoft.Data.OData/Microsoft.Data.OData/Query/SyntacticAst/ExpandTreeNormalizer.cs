using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x02000027 RID: 39
	internal static class ExpandTreeNormalizer
	{
		// Token: 0x060000FD RID: 253 RVA: 0x00004C90 File Offset: 0x00002E90
		public static ExpandToken NormalizeExpandTree(ExpandToken treeToNormalize)
		{
			ExpandToken treeToCollapse = ExpandTreeNormalizer.InvertPaths(treeToNormalize);
			return ExpandTreeNormalizer.CombineTerms(treeToCollapse);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00004CAC File Offset: 0x00002EAC
		public static ExpandToken InvertPaths(ExpandToken treeToInvert)
		{
			List<ExpandTermToken> list = new List<ExpandTermToken>();
			foreach (ExpandTermToken expandTermToken in treeToInvert.ExpandTerms)
			{
				PathReverser visitor = new PathReverser();
				PathSegmentToken pathToNavProp = expandTermToken.PathToNavProp.Accept<PathSegmentToken>(visitor);
				ExpandTermToken item = new ExpandTermToken(pathToNavProp, expandTermToken.FilterOption, expandTermToken.OrderByOption, expandTermToken.TopOption, expandTermToken.SkipOption, expandTermToken.InlineCountOption, expandTermToken.SelectOption, expandTermToken.ExpandOption);
				list.Add(item);
			}
			return new ExpandToken(list);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00004D50 File Offset: 0x00002F50
		public static ExpandToken CombineTerms(ExpandToken treeToCollapse)
		{
			Dictionary<PathSegmentToken, ExpandTermToken> dictionary = new Dictionary<PathSegmentToken, ExpandTermToken>(new PathSegmentTokenEqualityComparer());
			foreach (ExpandTermToken termToExpand in treeToCollapse.ExpandTerms)
			{
				ExpandTermToken expandedTerm = ExpandTreeNormalizer.BuildSubExpandTree(termToExpand);
				ExpandTreeNormalizer.AddOrCombine(dictionary, expandedTerm);
			}
			return new ExpandToken(dictionary.Values);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00004DBC File Offset: 0x00002FBC
		public static ExpandTermToken BuildSubExpandTree(ExpandTermToken termToExpand)
		{
			if (termToExpand.PathToNavProp.NextToken == null)
			{
				return termToExpand;
			}
			PathSegmentToken pathToNavProp = termToExpand.PathToNavProp;
			PathSegmentToken pathSegmentToken = pathToNavProp;
			while (pathSegmentToken.IsNamespaceOrContainerQualified())
			{
				pathSegmentToken = pathSegmentToken.NextToken;
				if (pathSegmentToken == null)
				{
					throw new ODataException(Strings.ExpandTreeNormalizer_NonPathInPropertyChain);
				}
			}
			PathSegmentToken nextToken = pathSegmentToken.NextToken;
			pathSegmentToken.SetNextToken(null);
			ExpandToken expandOption;
			if (nextToken != null)
			{
				ExpandTermToken termToExpand2 = new ExpandTermToken(nextToken, termToExpand.FilterOption, termToExpand.OrderByOption, termToExpand.TopOption, termToExpand.SkipOption, termToExpand.InlineCountOption, termToExpand.SelectOption, null);
				ExpandTermToken expandTermToken = ExpandTreeNormalizer.BuildSubExpandTree(termToExpand2);
				expandOption = new ExpandToken(new ExpandTermToken[]
				{
					expandTermToken
				});
			}
			else
			{
				expandOption = new ExpandToken(new ExpandTermToken[0]);
			}
			return new ExpandTermToken(pathToNavProp, termToExpand.FilterOption, termToExpand.OrderByOption, termToExpand.TopOption, termToExpand.SkipOption, termToExpand.InlineCountOption, termToExpand.SelectOption, expandOption);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00004E98 File Offset: 0x00003098
		public static ExpandTermToken CombineTerms(ExpandTermToken existingToken, ExpandTermToken newToken)
		{
			List<ExpandTermToken> expandTerms = ExpandTreeNormalizer.CombineChildNodes(existingToken, newToken).ToList<ExpandTermToken>();
			return new ExpandTermToken(existingToken.PathToNavProp, existingToken.FilterOption, existingToken.OrderByOption, existingToken.TopOption, existingToken.SkipOption, existingToken.InlineCountOption, existingToken.SelectOption, new ExpandToken(expandTerms));
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00004EE8 File Offset: 0x000030E8
		public static IEnumerable<ExpandTermToken> CombineChildNodes(ExpandTermToken existingToken, ExpandTermToken newToken)
		{
			if (existingToken.ExpandOption == null && newToken.ExpandOption == null)
			{
				return new List<ExpandTermToken>();
			}
			Dictionary<PathSegmentToken, ExpandTermToken> dictionary = new Dictionary<PathSegmentToken, ExpandTermToken>(new PathSegmentTokenEqualityComparer());
			if (existingToken.ExpandOption != null)
			{
				ExpandTreeNormalizer.AddChildOptionsToDictionary(existingToken, dictionary);
			}
			if (newToken.ExpandOption != null)
			{
				ExpandTreeNormalizer.AddChildOptionsToDictionary(newToken, dictionary);
			}
			return dictionary.Values;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00004F3C File Offset: 0x0000313C
		private static void AddChildOptionsToDictionary(ExpandTermToken newToken, Dictionary<PathSegmentToken, ExpandTermToken> combinedTerms)
		{
			foreach (ExpandTermToken expandedTerm in newToken.ExpandOption.ExpandTerms)
			{
				ExpandTreeNormalizer.AddOrCombine(combinedTerms, expandedTerm);
			}
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00004F90 File Offset: 0x00003190
		private static void AddOrCombine(IDictionary<PathSegmentToken, ExpandTermToken> combinedTerms, ExpandTermToken expandedTerm)
		{
			ExpandTermToken newToken;
			if (combinedTerms.TryGetValue(expandedTerm.PathToNavProp, out newToken))
			{
				combinedTerms[expandedTerm.PathToNavProp] = ExpandTreeNormalizer.CombineTerms(expandedTerm, newToken);
				return;
			}
			combinedTerms.Add(expandedTerm.PathToNavProp, expandedTerm);
		}
	}
}
