using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x0200011B RID: 283
	internal abstract class ResourceExpression : Expression
	{
		// Token: 0x0600134D RID: 4941 RVA: 0x000486D4 File Offset: 0x000468D4
		internal ResourceExpression(Expression source, ExpressionType nodeType, Type type, List<string> expandPaths, CountOption countOption, Dictionary<ConstantExpression, ConstantExpression> customQueryOptions, ProjectionQueryOptionExpression projection) : base(nodeType, type)
		{
			this.expandPaths = (expandPaths ?? new List<string>());
			this.countOption = countOption;
			this.customQueryOptions = (customQueryOptions ?? new Dictionary<ConstantExpression, ConstantExpression>(ReferenceEqualityComparer<ConstantExpression>.Instance));
			this.projection = projection;
			this.Source = source;
		}

		// Token: 0x0600134E RID: 4942
		internal abstract ResourceExpression CreateCloneWithNewType(Type type);

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x0600134F RID: 4943
		internal abstract bool HasQueryOptions { get; }

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06001350 RID: 4944
		internal abstract Type ResourceType { get; }

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06001351 RID: 4945
		internal abstract bool IsSingleton { get; }

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06001352 RID: 4946 RVA: 0x00048727 File Offset: 0x00046927
		// (set) Token: 0x06001353 RID: 4947 RVA: 0x0004872F File Offset: 0x0004692F
		internal virtual List<string> ExpandPaths
		{
			get
			{
				return this.expandPaths;
			}
			set
			{
				this.expandPaths = value;
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06001354 RID: 4948 RVA: 0x00048738 File Offset: 0x00046938
		// (set) Token: 0x06001355 RID: 4949 RVA: 0x00048740 File Offset: 0x00046940
		internal virtual CountOption CountOption
		{
			get
			{
				return this.countOption;
			}
			set
			{
				this.countOption = value;
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06001356 RID: 4950 RVA: 0x00048749 File Offset: 0x00046949
		// (set) Token: 0x06001357 RID: 4951 RVA: 0x00048751 File Offset: 0x00046951
		internal virtual Dictionary<ConstantExpression, ConstantExpression> CustomQueryOptions
		{
			get
			{
				return this.customQueryOptions;
			}
			set
			{
				this.customQueryOptions = value;
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06001358 RID: 4952 RVA: 0x0004875A File Offset: 0x0004695A
		// (set) Token: 0x06001359 RID: 4953 RVA: 0x00048762 File Offset: 0x00046962
		internal ProjectionQueryOptionExpression Projection
		{
			get
			{
				return this.projection;
			}
			set
			{
				this.projection = value;
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x0600135A RID: 4954 RVA: 0x0004876B File Offset: 0x0004696B
		// (set) Token: 0x0600135B RID: 4955 RVA: 0x00048773 File Offset: 0x00046973
		internal Expression Source { get; private set; }

		// Token: 0x0600135C RID: 4956 RVA: 0x0004877C File Offset: 0x0004697C
		internal InputReferenceExpression CreateReference()
		{
			if (this.inputRef == null)
			{
				this.inputRef = new InputReferenceExpression(this);
			}
			return this.inputRef;
		}

		// Token: 0x040005A9 RID: 1449
		protected InputReferenceExpression inputRef;

		// Token: 0x040005AA RID: 1450
		private List<string> expandPaths;

		// Token: 0x040005AB RID: 1451
		private CountOption countOption;

		// Token: 0x040005AC RID: 1452
		private Dictionary<ConstantExpression, ConstantExpression> customQueryOptions;

		// Token: 0x040005AD RID: 1453
		private ProjectionQueryOptionExpression projection;
	}
}
