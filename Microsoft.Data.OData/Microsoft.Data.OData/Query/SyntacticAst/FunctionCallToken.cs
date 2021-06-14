using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x020000C9 RID: 201
	internal sealed class FunctionCallToken : QueryToken
	{
		// Token: 0x060004E8 RID: 1256 RVA: 0x00011408 File Offset: 0x0000F608
		public FunctionCallToken(string name, IEnumerable<QueryToken> argumentValues)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(name, "name");
			this.name = name;
			IEnumerable<FunctionParameterToken> enumerable;
			if (argumentValues != null)
			{
				enumerable = new ReadOnlyEnumerableForUriParser<FunctionParameterToken>(from v in argumentValues
				select new FunctionParameterToken(null, v));
			}
			else
			{
				enumerable = new ReadOnlyEnumerableForUriParser<FunctionParameterToken>(FunctionParameterToken.EmptyParameterList);
			}
			this.arguments = enumerable;
			this.source = null;
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00011471 File Offset: 0x0000F671
		public FunctionCallToken(string name, IEnumerable<FunctionParameterToken> arguments, QueryToken source)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(name, "name");
			this.name = name;
			this.arguments = new ReadOnlyEnumerableForUriParser<FunctionParameterToken>(arguments ?? ((IEnumerable<FunctionParameterToken>)FunctionParameterToken.EmptyParameterList));
			this.source = source;
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x000114AC File Offset: 0x0000F6AC
		public override QueryTokenKind Kind
		{
			get
			{
				return QueryTokenKind.FunctionCall;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x000114AF File Offset: 0x0000F6AF
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x000114B7 File Offset: 0x0000F6B7
		public IEnumerable<FunctionParameterToken> Arguments
		{
			get
			{
				return this.arguments;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x000114BF File Offset: 0x0000F6BF
		public QueryToken Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x000114C7 File Offset: 0x0000F6C7
		public override T Accept<T>(ISyntacticTreeVisitor<T> visitor)
		{
			return visitor.Visit(this);
		}

		// Token: 0x040001CE RID: 462
		private readonly string name;

		// Token: 0x040001CF RID: 463
		private readonly IEnumerable<FunctionParameterToken> arguments;

		// Token: 0x040001D0 RID: 464
		private readonly QueryToken source;
	}
}
