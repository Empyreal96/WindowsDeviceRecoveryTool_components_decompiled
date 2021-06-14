using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Data.Services.Client
{
	// Token: 0x020000C1 RID: 193
	internal abstract class ResourceExpression : Expression
	{
		// Token: 0x06000623 RID: 1571 RVA: 0x000186C4 File Offset: 0x000168C4
		internal ResourceExpression(Expression source, Type type, List<string> expandPaths, CountOption countOption, Dictionary<ConstantExpression, ConstantExpression> customQueryOptions, ProjectionQueryOptionExpression projection, Type resourceTypeAs, Version uriVersion)
		{
			this.source = source;
			this.type = type;
			this.expandPaths = (expandPaths ?? new List<string>());
			this.countOption = countOption;
			this.customQueryOptions = (customQueryOptions ?? new Dictionary<ConstantExpression, ConstantExpression>(ReferenceEqualityComparer<ConstantExpression>.Instance));
			this.projection = projection;
			this.ResourceTypeAs = resourceTypeAs;
			this.uriVersion = (uriVersion ?? Util.DataServiceVersion1);
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000624 RID: 1572 RVA: 0x00018734 File Offset: 0x00016934
		public override Type Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x06000625 RID: 1573
		internal abstract ResourceExpression CreateCloneWithNewType(Type type);

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000626 RID: 1574
		internal abstract bool HasQueryOptions { get; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000627 RID: 1575
		internal abstract Type ResourceType { get; }

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x0001873C File Offset: 0x0001693C
		// (set) Token: 0x06000629 RID: 1577 RVA: 0x00018744 File Offset: 0x00016944
		internal Type ResourceTypeAs { get; set; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x0001874D File Offset: 0x0001694D
		internal Version UriVersion
		{
			get
			{
				return this.uriVersion;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x0600062B RID: 1579
		internal abstract bool IsSingleton { get; }

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x00018755 File Offset: 0x00016955
		// (set) Token: 0x0600062D RID: 1581 RVA: 0x0001875D File Offset: 0x0001695D
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

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x0600062E RID: 1582 RVA: 0x00018766 File Offset: 0x00016966
		// (set) Token: 0x0600062F RID: 1583 RVA: 0x0001876E File Offset: 0x0001696E
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

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x00018777 File Offset: 0x00016977
		// (set) Token: 0x06000631 RID: 1585 RVA: 0x0001877F File Offset: 0x0001697F
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

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000632 RID: 1586 RVA: 0x00018788 File Offset: 0x00016988
		// (set) Token: 0x06000633 RID: 1587 RVA: 0x00018790 File Offset: 0x00016990
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

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x00018799 File Offset: 0x00016999
		internal Expression Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x000187A1 File Offset: 0x000169A1
		internal InputReferenceExpression CreateReference()
		{
			if (this.inputRef == null)
			{
				this.inputRef = new InputReferenceExpression(this);
			}
			return this.inputRef;
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x000187BD File Offset: 0x000169BD
		internal void RaiseUriVersion(Version newVersion)
		{
			WebUtil.RaiseVersion(ref this.uriVersion, newVersion);
		}

		// Token: 0x040003F3 RID: 1011
		protected readonly Expression source;

		// Token: 0x040003F4 RID: 1012
		protected InputReferenceExpression inputRef;

		// Token: 0x040003F5 RID: 1013
		private Type type;

		// Token: 0x040003F6 RID: 1014
		private List<string> expandPaths;

		// Token: 0x040003F7 RID: 1015
		private CountOption countOption;

		// Token: 0x040003F8 RID: 1016
		private Dictionary<ConstantExpression, ConstantExpression> customQueryOptions;

		// Token: 0x040003F9 RID: 1017
		private ProjectionQueryOptionExpression projection;

		// Token: 0x040003FA RID: 1018
		private Version uriVersion;
	}
}
