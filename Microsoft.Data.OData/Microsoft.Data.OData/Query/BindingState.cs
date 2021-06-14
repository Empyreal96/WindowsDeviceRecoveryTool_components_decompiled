using System;
using System.Collections.Generic;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Query.SemanticAst;
using Microsoft.Data.OData.Query.SyntacticAst;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x0200000B RID: 11
	internal sealed class BindingState
	{
		// Token: 0x0600002F RID: 47 RVA: 0x00002909 File Offset: 0x00000B09
		internal BindingState(ODataUriParserConfiguration configuration)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataUriParserConfiguration>(configuration, "configuration");
			this.configuration = configuration;
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000030 RID: 48 RVA: 0x0000292E File Offset: 0x00000B2E
		internal IEdmModel Model
		{
			get
			{
				return this.configuration.Model;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000031 RID: 49 RVA: 0x0000293B File Offset: 0x00000B3B
		internal ODataUriParserConfiguration Configuration
		{
			get
			{
				return this.configuration;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002943 File Offset: 0x00000B43
		// (set) Token: 0x06000033 RID: 51 RVA: 0x0000294B File Offset: 0x00000B4B
		internal RangeVariable ImplicitRangeVariable
		{
			get
			{
				return this.implicitRangeVariable;
			}
			set
			{
				this.implicitRangeVariable = value;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002954 File Offset: 0x00000B54
		internal Stack<RangeVariable> RangeVariables
		{
			get
			{
				return this.rangeVariables;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000035 RID: 53 RVA: 0x0000295C File Offset: 0x00000B5C
		// (set) Token: 0x06000036 RID: 54 RVA: 0x00002964 File Offset: 0x00000B64
		internal List<CustomQueryOptionToken> QueryOptions
		{
			get
			{
				return this.queryOptions;
			}
			set
			{
				this.queryOptions = value;
			}
		}

		// Token: 0x0400000E RID: 14
		private readonly ODataUriParserConfiguration configuration;

		// Token: 0x0400000F RID: 15
		private readonly Stack<RangeVariable> rangeVariables = new Stack<RangeVariable>();

		// Token: 0x04000010 RID: 16
		private RangeVariable implicitRangeVariable;

		// Token: 0x04000011 RID: 17
		private List<CustomQueryOptionToken> queryOptions;
	}
}
