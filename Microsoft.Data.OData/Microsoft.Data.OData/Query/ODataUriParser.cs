using System;
using System.Collections.Generic;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;
using Microsoft.Data.OData.Query.SemanticAst;
using Microsoft.Data.OData.Query.SyntacticAst;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x0200004E RID: 78
	public sealed class ODataUriParser
	{
		// Token: 0x060001FE RID: 510 RVA: 0x00007D5C File Offset: 0x00005F5C
		public ODataUriParser(IEdmModel model, Uri serviceRoot)
		{
			this.configuration = new ODataUriParserConfiguration(model, serviceRoot);
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001FF RID: 511 RVA: 0x00007D71 File Offset: 0x00005F71
		public ODataUriParserSettings Settings
		{
			get
			{
				return this.configuration.Settings;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000200 RID: 512 RVA: 0x00007D7E File Offset: 0x00005F7E
		public IEdmModel Model
		{
			get
			{
				return this.configuration.Model;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000201 RID: 513 RVA: 0x00007D8B File Offset: 0x00005F8B
		public Uri ServiceRoot
		{
			get
			{
				return this.configuration.ServiceRoot;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000202 RID: 514 RVA: 0x00007D98 File Offset: 0x00005F98
		// (set) Token: 0x06000203 RID: 515 RVA: 0x00007DA5 File Offset: 0x00005FA5
		public ODataUrlConventions UrlConventions
		{
			get
			{
				return this.configuration.UrlConventions;
			}
			set
			{
				this.configuration.UrlConventions = value;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000204 RID: 516 RVA: 0x00007DB3 File Offset: 0x00005FB3
		// (set) Token: 0x06000205 RID: 517 RVA: 0x00007DC0 File Offset: 0x00005FC0
		public Func<string, BatchReferenceSegment> BatchReferenceCallback
		{
			get
			{
				return this.configuration.BatchReferenceCallback;
			}
			set
			{
				this.configuration.BatchReferenceCallback = value;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000206 RID: 518 RVA: 0x00007DCE File Offset: 0x00005FCE
		// (set) Token: 0x06000207 RID: 519 RVA: 0x00007DDB File Offset: 0x00005FDB
		public Func<string, string> FunctionParameterAliasCallback
		{
			get
			{
				return this.configuration.FunctionParameterAliasCallback;
			}
			set
			{
				this.configuration.FunctionParameterAliasCallback = value;
			}
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00007DEC File Offset: 0x00005FEC
		public static FilterClause ParseFilter(string filter, IEdmModel model, IEdmType elementType)
		{
			ODataUriParser odataUriParser = new ODataUriParser(model, null);
			return odataUriParser.ParseFilter(filter, elementType, null);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00007E0C File Offset: 0x0000600C
		public static FilterClause ParseFilter(string filter, IEdmModel model, IEdmType elementType, IEdmEntitySet entitySet)
		{
			ODataUriParser odataUriParser = new ODataUriParser(model, null);
			return odataUriParser.ParseFilter(filter, elementType, entitySet);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00007E2C File Offset: 0x0000602C
		public static OrderByClause ParseOrderBy(string orderBy, IEdmModel model, IEdmType elementType)
		{
			ODataUriParser odataUriParser = new ODataUriParser(model, null);
			return odataUriParser.ParseOrderBy(orderBy, elementType, null);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00007E4C File Offset: 0x0000604C
		public static OrderByClause ParseOrderBy(string orderBy, IEdmModel model, IEdmType elementType, IEdmEntitySet entitySet)
		{
			ODataUriParser odataUriParser = new ODataUriParser(model, null);
			return odataUriParser.ParseOrderBy(orderBy, elementType, entitySet);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00007E6A File Offset: 0x0000606A
		public FilterClause ParseFilter(string filter, IEdmType elementType, IEdmEntitySet entitySet)
		{
			return this.ParseFilterImplementation(filter, elementType, entitySet);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00007E75 File Offset: 0x00006075
		public OrderByClause ParseOrderBy(string orderBy, IEdmType elementType, IEdmEntitySet entitySet)
		{
			return this.ParseOrderByImplementation(orderBy, elementType, entitySet);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00007E80 File Offset: 0x00006080
		public ODataPath ParsePath(Uri pathUri)
		{
			ExceptionUtils.CheckArgumentNotNull<Uri>(pathUri, "pathUri");
			if (this.configuration.ServiceRoot == null)
			{
				throw new ODataException(Strings.UriParser_NeedServiceRootForThisOverload);
			}
			if (!pathUri.IsAbsoluteUri)
			{
				throw new ODataException(Strings.UriParser_UriMustBeAbsolute(pathUri));
			}
			UriPathParser uriPathParser = new UriPathParser(this.Settings.PathLimit);
			ICollection<string> segments = uriPathParser.ParsePathIntoSegments(pathUri, this.configuration.ServiceRoot);
			return ODataPathFactory.BindPath(segments, this.configuration);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00007EFA File Offset: 0x000060FA
		public SelectExpandClause ParseSelectAndExpand(string select, string expand, IEdmEntityType elementType, IEdmEntitySet entitySet)
		{
			return this.ParseSelectAndExpandImplementation(select, expand, elementType, entitySet);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00007F07 File Offset: 0x00006107
		internal ODataUri ParseUri(Uri fullUri)
		{
			return this.ParseUriImplementation(fullUri);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00007F10 File Offset: 0x00006110
		internal InlineCountKind ParseInlineCount(string inlineCount)
		{
			return this.ParseInlineCountImplementation(inlineCount);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00007F1C File Offset: 0x0000611C
		private ODataUri ParseUriImplementation(Uri fullUri)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmModel>(this.configuration.Model, "model");
			ExceptionUtils.CheckArgumentNotNull<Uri>(this.configuration.ServiceRoot, "serviceRoot");
			ExceptionUtils.CheckArgumentNotNull<Uri>(fullUri, "fullUri");
			SyntacticTree syntacticTree = SyntacticTree.ParseUri(fullUri, this.configuration.ServiceRoot, this.Settings.FilterLimit);
			ExceptionUtils.CheckArgumentNotNull<SyntacticTree>(syntacticTree, "syntax");
			BindingState bindingState = new BindingState(this.configuration);
			MetadataBinder @object = new MetadataBinder(bindingState);
			ODataUriSemanticBinder odataUriSemanticBinder = new ODataUriSemanticBinder(bindingState, new MetadataBinder.QueryTokenVisitor(@object.Bind));
			return odataUriSemanticBinder.BindTree(syntacticTree);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00007FB4 File Offset: 0x000061B4
		private FilterClause ParseFilterImplementation(string filter, IEdmType elementType, IEdmEntitySet entitySet)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataUriParserConfiguration>(this.configuration, "this.configuration");
			ExceptionUtils.CheckArgumentNotNull<IEdmType>(elementType, "elementType");
			ExceptionUtils.CheckArgumentNotNull<string>(filter, "filter");
			UriQueryExpressionParser uriQueryExpressionParser = new UriQueryExpressionParser(this.Settings.FilterLimit);
			QueryToken filter2 = uriQueryExpressionParser.ParseFilter(filter);
			BindingState bindingState = new BindingState(this.configuration);
			bindingState.ImplicitRangeVariable = NodeFactory.CreateImplicitRangeVariable(elementType.ToTypeReference(), entitySet);
			bindingState.RangeVariables.Push(bindingState.ImplicitRangeVariable);
			MetadataBinder @object = new MetadataBinder(bindingState);
			FilterBinder filterBinder = new FilterBinder(new MetadataBinder.QueryTokenVisitor(@object.Bind), bindingState);
			return filterBinder.BindFilter(filter2);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00008058 File Offset: 0x00006258
		private OrderByClause ParseOrderByImplementation(string orderBy, IEdmType elementType, IEdmEntitySet entitySet)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmModel>(this.configuration.Model, "model");
			ExceptionUtils.CheckArgumentNotNull<IEdmType>(elementType, "elementType");
			ExceptionUtils.CheckArgumentNotNull<string>(orderBy, "orderBy");
			UriQueryExpressionParser uriQueryExpressionParser = new UriQueryExpressionParser(this.Settings.OrderByLimit);
			IEnumerable<OrderByToken> orderByTokens = uriQueryExpressionParser.ParseOrderBy(orderBy);
			BindingState bindingState = new BindingState(this.configuration);
			bindingState.ImplicitRangeVariable = NodeFactory.CreateImplicitRangeVariable(elementType.ToTypeReference(), entitySet);
			bindingState.RangeVariables.Push(bindingState.ImplicitRangeVariable);
			MetadataBinder @object = new MetadataBinder(bindingState);
			OrderByBinder orderByBinder = new OrderByBinder(new MetadataBinder.QueryTokenVisitor(@object.Bind));
			return orderByBinder.BindOrderBy(bindingState, orderByTokens);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00008100 File Offset: 0x00006300
		private SelectExpandClause ParseSelectAndExpandImplementation(string select, string expand, IEdmEntityType elementType, IEdmEntitySet entitySet)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmModel>(this.configuration.Model, "model");
			ExceptionUtils.CheckArgumentNotNull<IEdmEntityType>(elementType, "elementType");
			ISelectExpandTermParser selectExpandTermParser = SelectExpandTermParserFactory.Create(select, this.Settings);
			SelectToken selectToken = selectExpandTermParser.ParseSelect();
			ISelectExpandTermParser selectExpandTermParser2 = SelectExpandTermParserFactory.Create(expand, this.Settings);
			ExpandToken expandToken = selectExpandTermParser2.ParseExpand();
			return SelectExpandSemanticBinder.Parse(elementType, entitySet, expandToken, selectToken, this.configuration);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00008168 File Offset: 0x00006368
		private InlineCountKind ParseInlineCountImplementation(string inlineCount)
		{
			inlineCount = inlineCount.Trim();
			string a;
			if ((a = inlineCount) != null)
			{
				if (a == "allpages")
				{
					return InlineCountKind.AllPages;
				}
				if (a == "none")
				{
					return InlineCountKind.None;
				}
			}
			throw new ODataException(Strings.ODataUriParser_InvalidInlineCount(inlineCount));
		}

		// Token: 0x04000083 RID: 131
		private readonly ODataUriParserConfiguration configuration;
	}
}
