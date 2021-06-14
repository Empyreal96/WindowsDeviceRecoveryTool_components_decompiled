using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Data.Services.Client
{
	// Token: 0x020000BC RID: 188
	internal class QueryComponents
	{
		// Token: 0x06000604 RID: 1540 RVA: 0x000171F4 File Offset: 0x000153F4
		internal QueryComponents(Uri uri, Version version, Type lastSegmentType, LambdaExpression projection, Dictionary<Expression, Expression> normalizerRewrites)
		{
			this.projection = projection;
			this.normalizerRewrites = normalizerRewrites;
			this.lastSegmentType = lastSegmentType;
			this.Uri = uri;
			this.version = version;
			this.httpMethod = "GET";
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0001722C File Offset: 0x0001542C
		internal QueryComponents(Uri uri, Version version, Type lastSegmentType, LambdaExpression projection, Dictionary<Expression, Expression> normalizerRewrites, string httpMethod, bool? singleResult, List<BodyOperationParameter> bodyOperationParameters, List<UriOperationParameter> uriOperationParameters)
		{
			this.projection = projection;
			this.normalizerRewrites = normalizerRewrites;
			this.lastSegmentType = lastSegmentType;
			this.Uri = uri;
			this.version = version;
			this.httpMethod = httpMethod;
			this.uriOperationParameters = uriOperationParameters;
			this.bodyOperationParameters = bodyOperationParameters;
			this.singleResult = singleResult;
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x00017284 File Offset: 0x00015484
		internal Dictionary<Expression, Expression> NormalizerRewrites
		{
			get
			{
				return this.normalizerRewrites;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000607 RID: 1543 RVA: 0x0001728C File Offset: 0x0001548C
		internal LambdaExpression Projection
		{
			get
			{
				return this.projection;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000608 RID: 1544 RVA: 0x00017294 File Offset: 0x00015494
		internal Type LastSegmentType
		{
			get
			{
				return this.lastSegmentType;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000609 RID: 1545 RVA: 0x0001729C File Offset: 0x0001549C
		internal Version Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x000172A4 File Offset: 0x000154A4
		internal string HttpMethod
		{
			get
			{
				return this.httpMethod;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600060B RID: 1547 RVA: 0x000172AC File Offset: 0x000154AC
		internal List<UriOperationParameter> UriOperationParameters
		{
			get
			{
				return this.uriOperationParameters;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x000172B4 File Offset: 0x000154B4
		internal List<BodyOperationParameter> BodyOperationParameters
		{
			get
			{
				return this.bodyOperationParameters;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600060D RID: 1549 RVA: 0x000172BC File Offset: 0x000154BC
		internal bool? SingleResult
		{
			get
			{
				return this.singleResult;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x000172C4 File Offset: 0x000154C4
		internal bool HasSelectQueryOption
		{
			get
			{
				return this.Uri != null && QueryComponents.ContainsSelectQueryOption(UriUtil.UriToString(this.Uri));
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600060F RID: 1551 RVA: 0x000172E6 File Offset: 0x000154E6
		// (set) Token: 0x06000610 RID: 1552 RVA: 0x000172EE File Offset: 0x000154EE
		internal Uri Uri { get; set; }

		// Token: 0x06000611 RID: 1553 RVA: 0x000172F7 File Offset: 0x000154F7
		private static bool ContainsSelectQueryOption(string queryString)
		{
			return queryString.Contains("?$select=") || queryString.Contains("&$select=");
		}

		// Token: 0x04000335 RID: 821
		private const string SelectQueryOption = "$select=";

		// Token: 0x04000336 RID: 822
		private const string SelectQueryOptionWithQuestionMark = "?$select=";

		// Token: 0x04000337 RID: 823
		private const string SelectQueryOptionWithAmpersand = "&$select=";

		// Token: 0x04000338 RID: 824
		private readonly Type lastSegmentType;

		// Token: 0x04000339 RID: 825
		private readonly Dictionary<Expression, Expression> normalizerRewrites;

		// Token: 0x0400033A RID: 826
		private readonly LambdaExpression projection;

		// Token: 0x0400033B RID: 827
		private readonly string httpMethod;

		// Token: 0x0400033C RID: 828
		private readonly List<UriOperationParameter> uriOperationParameters;

		// Token: 0x0400033D RID: 829
		private readonly List<BodyOperationParameter> bodyOperationParameters;

		// Token: 0x0400033E RID: 830
		private readonly bool? singleResult;

		// Token: 0x0400033F RID: 831
		private Version version;
	}
}
