using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace System.Data.Services.Client
{
	// Token: 0x020000D6 RID: 214
	internal class NavigationPropertySingletonExpression : ResourceExpression
	{
		// Token: 0x060006D1 RID: 1745 RVA: 0x0001C5A4 File Offset: 0x0001A7A4
		internal NavigationPropertySingletonExpression(Type type, Expression source, Expression memberExpression, Type resourceType, List<string> expandPaths, CountOption countOption, Dictionary<ConstantExpression, ConstantExpression> customQueryOptions, ProjectionQueryOptionExpression projection, Type resourceTypeAs, Version uriVersion) : base(source, type, expandPaths, countOption, customQueryOptions, projection, resourceTypeAs, uriVersion)
		{
			this.memberExpression = memberExpression;
			this.resourceType = resourceType;
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060006D2 RID: 1746 RVA: 0x0001C5D4 File Offset: 0x0001A7D4
		public override ExpressionType NodeType
		{
			get
			{
				return (ExpressionType)10002;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x0001C5DB File Offset: 0x0001A7DB
		internal MemberExpression MemberExpression
		{
			get
			{
				return (MemberExpression)this.memberExpression;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060006D4 RID: 1748 RVA: 0x0001C5E8 File Offset: 0x0001A7E8
		internal override Type ResourceType
		{
			get
			{
				return this.resourceType;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x0001C5F0 File Offset: 0x0001A7F0
		internal override bool IsSingleton
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060006D6 RID: 1750 RVA: 0x0001C5F3 File Offset: 0x0001A7F3
		internal override bool HasQueryOptions
		{
			get
			{
				return this.ExpandPaths.Count > 0 || this.CountOption == CountOption.InlineAll || this.CustomQueryOptions.Count > 0 || base.Projection != null;
			}
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0001C63C File Offset: 0x0001A83C
		internal override ResourceExpression CreateCloneWithNewType(Type type)
		{
			return new NavigationPropertySingletonExpression(type, this.source, this.MemberExpression, TypeSystem.GetElementType(type), this.ExpandPaths.ToList<string>(), this.CountOption, this.CustomQueryOptions.ToDictionary((KeyValuePair<ConstantExpression, ConstantExpression> kvp) => kvp.Key, (KeyValuePair<ConstantExpression, ConstantExpression> kvp) => kvp.Value), base.Projection, base.ResourceTypeAs, base.UriVersion);
		}

		// Token: 0x04000432 RID: 1074
		private readonly Expression memberExpression;

		// Token: 0x04000433 RID: 1075
		private readonly Type resourceType;
	}
}
