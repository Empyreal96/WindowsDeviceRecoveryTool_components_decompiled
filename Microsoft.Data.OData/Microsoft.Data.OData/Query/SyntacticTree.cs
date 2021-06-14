using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Data.OData.Query.SyntacticAst;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x020000D4 RID: 212
	internal sealed class SyntacticTree
	{
		// Token: 0x0600052E RID: 1326 RVA: 0x00011B10 File Offset: 0x0000FD10
		public SyntacticTree(ICollection<string> path, QueryToken filter, IEnumerable<OrderByToken> orderByTokens, SelectToken select, ExpandToken expand, int? skip, int? top, InlineCountKind? inlineCount, string format, IEnumerable<CustomQueryOptionToken> queryOptions)
		{
			ExceptionUtils.CheckArgumentNotNull<ICollection<string>>(path, "path");
			this.path = path;
			this.filter = filter;
			this.orderByTokens = new ReadOnlyEnumerableForUriParser<OrderByToken>(orderByTokens ?? ((IEnumerable<OrderByToken>)new OrderByToken[0]));
			this.select = select;
			this.expand = expand;
			this.skip = skip;
			this.top = top;
			this.inlineCount = inlineCount;
			this.format = format;
			this.queryOptions = new ReadOnlyEnumerableForUriParser<CustomQueryOptionToken>(queryOptions ?? ((IEnumerable<CustomQueryOptionToken>)new CustomQueryOptionToken[0]));
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x0600052F RID: 1327 RVA: 0x00011BA3 File Offset: 0x0000FDA3
		public ICollection<string> Path
		{
			get
			{
				return this.path;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x00011BAB File Offset: 0x0000FDAB
		public QueryToken Filter
		{
			get
			{
				return this.filter;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x00011BB3 File Offset: 0x0000FDB3
		public IEnumerable<OrderByToken> OrderByTokens
		{
			get
			{
				return this.orderByTokens;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x00011BBB File Offset: 0x0000FDBB
		public SelectToken Select
		{
			get
			{
				return this.select;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000533 RID: 1331 RVA: 0x00011BC3 File Offset: 0x0000FDC3
		public ExpandToken Expand
		{
			get
			{
				return this.expand;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x00011BCB File Offset: 0x0000FDCB
		public int? Skip
		{
			get
			{
				return this.skip;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x00011BD3 File Offset: 0x0000FDD3
		public int? Top
		{
			get
			{
				return this.top;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x00011BDB File Offset: 0x0000FDDB
		public string Format
		{
			get
			{
				return this.format;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x00011BE3 File Offset: 0x0000FDE3
		public InlineCountKind? InlineCount
		{
			get
			{
				return this.inlineCount;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x00011BEB File Offset: 0x0000FDEB
		public IEnumerable<CustomQueryOptionToken> QueryOptions
		{
			get
			{
				return this.queryOptions;
			}
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00011BF3 File Offset: 0x0000FDF3
		public static SyntacticTree ParseUri(Uri queryUri, Uri serviceBaseUri)
		{
			return SyntacticTree.ParseUri(queryUri, serviceBaseUri, 800);
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00011C04 File Offset: 0x0000FE04
		public static SyntacticTree ParseUri(Uri queryUri, Uri serviceBaseUri, int maxDepth)
		{
			ExceptionUtils.CheckArgumentNotNull<Uri>(queryUri, "queryUri");
			if (!queryUri.IsAbsoluteUri)
			{
				throw new ArgumentException(Strings.SyntacticTree_UriMustBeAbsolute(queryUri), "queryUri");
			}
			ExceptionUtils.CheckArgumentNotNull<Uri>(serviceBaseUri, "serviceBaseUri");
			if (!serviceBaseUri.IsAbsoluteUri)
			{
				throw new ArgumentException(Strings.SyntacticTree_UriMustBeAbsolute(serviceBaseUri), "serviceBaseUri");
			}
			if (maxDepth <= 0)
			{
				throw new ArgumentException(Strings.SyntacticTree_MaxDepthInvalid, "maxDepth");
			}
			UriPathParser uriPathParser = new UriPathParser(maxDepth);
			ICollection<string> collection = uriPathParser.ParsePathIntoSegments(queryUri, serviceBaseUri);
			List<CustomQueryOptionToken> list = UriUtils.ParseQueryOptions(queryUri);
			QueryToken queryToken = null;
			string queryOptionValueAndRemove = list.GetQueryOptionValueAndRemove("$filter");
			if (queryOptionValueAndRemove != null)
			{
				UriQueryExpressionParser uriQueryExpressionParser = new UriQueryExpressionParser(maxDepth);
				queryToken = uriQueryExpressionParser.ParseFilter(queryOptionValueAndRemove);
			}
			IEnumerable<OrderByToken> enumerable = null;
			string queryOptionValueAndRemove2 = list.GetQueryOptionValueAndRemove("$orderby");
			if (queryOptionValueAndRemove2 != null)
			{
				UriQueryExpressionParser uriQueryExpressionParser2 = new UriQueryExpressionParser(maxDepth);
				enumerable = uriQueryExpressionParser2.ParseOrderBy(queryOptionValueAndRemove2);
			}
			SelectToken selectToken = null;
			string queryOptionValueAndRemove3 = list.GetQueryOptionValueAndRemove("$select");
			if (queryOptionValueAndRemove3 != null)
			{
				ISelectExpandTermParser selectExpandTermParser = SelectExpandTermParserFactory.Create(queryOptionValueAndRemove3);
				selectToken = selectExpandTermParser.ParseSelect();
			}
			ExpandToken expandToken = null;
			string queryOptionValueAndRemove4 = list.GetQueryOptionValueAndRemove("$expand");
			if (queryOptionValueAndRemove4 != null)
			{
				ISelectExpandTermParser selectExpandTermParser2 = SelectExpandTermParserFactory.Create(queryOptionValueAndRemove4);
				expandToken = selectExpandTermParser2.ParseExpand();
			}
			int? num = null;
			string queryOptionValueAndRemove5 = list.GetQueryOptionValueAndRemove("$skip");
			if (queryOptionValueAndRemove5 != null)
			{
				int value;
				if (!UriPrimitiveTypeParser.TryUriStringToNonNegativeInteger(queryOptionValueAndRemove5, out value))
				{
					throw new ODataException(Strings.SyntacticTree_InvalidSkipQueryOptionValue(queryOptionValueAndRemove5));
				}
				num = new int?(value);
			}
			int? num2 = null;
			string queryOptionValueAndRemove6 = list.GetQueryOptionValueAndRemove("$top");
			if (queryOptionValueAndRemove6 != null)
			{
				int value2;
				if (!UriPrimitiveTypeParser.TryUriStringToNonNegativeInteger(queryOptionValueAndRemove6, out value2))
				{
					throw new ODataException(Strings.SyntacticTree_InvalidTopQueryOptionValue(queryOptionValueAndRemove6));
				}
				num2 = new int?(value2);
			}
			string queryOptionValueAndRemove7 = list.GetQueryOptionValueAndRemove("$inlinecount");
			InlineCountKind? inlineCountKind = QueryTokenUtils.ParseInlineCountKind(queryOptionValueAndRemove7);
			string queryOptionValueAndRemove8 = list.GetQueryOptionValueAndRemove("$format");
			return new SyntacticTree(collection, queryToken, enumerable, selectToken, expandToken, num, num2, inlineCountKind, queryOptionValueAndRemove8, (list.Count == 0) ? null : new ReadOnlyCollection<CustomQueryOptionToken>(list));
		}

		// Token: 0x040001EC RID: 492
		private const int DefaultMaxDepth = 800;

		// Token: 0x040001ED RID: 493
		private readonly ICollection<string> path;

		// Token: 0x040001EE RID: 494
		private readonly QueryToken filter;

		// Token: 0x040001EF RID: 495
		private readonly IEnumerable<OrderByToken> orderByTokens;

		// Token: 0x040001F0 RID: 496
		private readonly SelectToken select;

		// Token: 0x040001F1 RID: 497
		private readonly ExpandToken expand;

		// Token: 0x040001F2 RID: 498
		private readonly int? skip;

		// Token: 0x040001F3 RID: 499
		private readonly int? top;

		// Token: 0x040001F4 RID: 500
		private readonly string format;

		// Token: 0x040001F5 RID: 501
		private readonly InlineCountKind? inlineCount;

		// Token: 0x040001F6 RID: 502
		private readonly IEnumerable<CustomQueryOptionToken> queryOptions;
	}
}
