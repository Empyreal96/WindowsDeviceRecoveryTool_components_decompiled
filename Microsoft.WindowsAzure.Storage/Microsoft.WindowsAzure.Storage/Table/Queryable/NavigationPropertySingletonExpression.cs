using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x0200011C RID: 284
	internal class NavigationPropertySingletonExpression : ResourceExpression
	{
		// Token: 0x0600135D RID: 4957 RVA: 0x00048798 File Offset: 0x00046998
		internal NavigationPropertySingletonExpression(Type type, Expression source, Expression memberExpression, Type resourceType, List<string> expandPaths, CountOption countOption, Dictionary<ConstantExpression, ConstantExpression> customQueryOptions, ProjectionQueryOptionExpression projection) : base(source, (ExpressionType)10002, type, expandPaths, countOption, customQueryOptions, projection)
		{
			this.memberExpression = memberExpression;
			this.resourceType = resourceType;
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x0600135E RID: 4958 RVA: 0x000487BE File Offset: 0x000469BE
		internal MemberExpression MemberExpression
		{
			get
			{
				return (MemberExpression)this.memberExpression;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x0600135F RID: 4959 RVA: 0x000487CB File Offset: 0x000469CB
		internal override Type ResourceType
		{
			get
			{
				return this.resourceType;
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06001360 RID: 4960 RVA: 0x000487D3 File Offset: 0x000469D3
		internal override bool IsSingleton
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06001361 RID: 4961 RVA: 0x000487D6 File Offset: 0x000469D6
		internal override bool HasQueryOptions
		{
			get
			{
				return this.ExpandPaths.Count > 0 || this.CountOption == CountOption.InlineAll || this.CustomQueryOptions.Count > 0 || base.Projection != null;
			}
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x00048820 File Offset: 0x00046A20
		internal override ResourceExpression CreateCloneWithNewType(Type type)
		{
			return new NavigationPropertySingletonExpression(type, base.Source, this.MemberExpression, TypeSystem.GetElementType(type), this.ExpandPaths.ToList<string>(), this.CountOption, this.CustomQueryOptions.ToDictionary((KeyValuePair<ConstantExpression, ConstantExpression> kvp) => kvp.Key, (KeyValuePair<ConstantExpression, ConstantExpression> kvp) => kvp.Value), base.Projection);
		}

		// Token: 0x040005AF RID: 1455
		private readonly Expression memberExpression;

		// Token: 0x040005B0 RID: 1456
		private readonly Type resourceType;
	}
}
