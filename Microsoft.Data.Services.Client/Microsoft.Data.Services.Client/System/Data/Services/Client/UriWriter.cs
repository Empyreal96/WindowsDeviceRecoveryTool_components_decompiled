using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace System.Data.Services.Client
{
	// Token: 0x020000DF RID: 223
	internal class UriWriter : DataServiceALinqExpressionVisitor
	{
		// Token: 0x06000727 RID: 1831 RVA: 0x0001EA4D File Offset: 0x0001CC4D
		private UriWriter(DataServiceContext context)
		{
			this.context = context;
			this.uriBuilder = new StringBuilder();
			this.uriVersion = Util.DataServiceVersion1;
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0001EA84 File Offset: 0x0001CC84
		internal static void Translate(DataServiceContext context, bool addTrailingParens, Expression e, out Uri uri, out Version version)
		{
			UriWriter uriWriter = new UriWriter(context);
			uriWriter.leafResourceSet = (addTrailingParens ? (e as ResourceSetExpression) : null);
			uriWriter.Visit(e);
			uri = UriUtil.CreateUri(uriWriter.uriBuilder.ToString(), UriKind.Absolute);
			version = uriWriter.uriVersion;
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0001EACE File Offset: 0x0001CCCE
		internal override Expression VisitMethodCall(MethodCallExpression m)
		{
			throw Error.MethodNotSupported(m);
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0001EAD6 File Offset: 0x0001CCD6
		internal override Expression VisitUnary(UnaryExpression u)
		{
			throw new NotSupportedException(Strings.ALinq_UnaryNotSupported(u.NodeType.ToString()));
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0001EAF2 File Offset: 0x0001CCF2
		internal override Expression VisitBinary(BinaryExpression b)
		{
			throw new NotSupportedException(Strings.ALinq_BinaryNotSupported(b.NodeType.ToString()));
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0001EB0E File Offset: 0x0001CD0E
		internal override Expression VisitConstant(ConstantExpression c)
		{
			throw new NotSupportedException(Strings.ALinq_ConstantNotSupported(c.Value));
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0001EB20 File Offset: 0x0001CD20
		internal override Expression VisitTypeIs(TypeBinaryExpression b)
		{
			throw new NotSupportedException(Strings.ALinq_TypeBinaryNotSupported);
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0001EB2C File Offset: 0x0001CD2C
		internal override Expression VisitConditional(ConditionalExpression c)
		{
			throw new NotSupportedException(Strings.ALinq_ConditionalNotSupported);
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0001EB38 File Offset: 0x0001CD38
		internal override Expression VisitParameter(ParameterExpression p)
		{
			throw new NotSupportedException(Strings.ALinq_ParameterNotSupported);
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0001EB44 File Offset: 0x0001CD44
		internal override Expression VisitMemberAccess(MemberExpression m)
		{
			throw new NotSupportedException(Strings.ALinq_MemberAccessNotSupported(m.Member.Name));
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0001EB5B File Offset: 0x0001CD5B
		internal override Expression VisitLambda(LambdaExpression lambda)
		{
			throw new NotSupportedException(Strings.ALinq_LambdaNotSupported);
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0001EB67 File Offset: 0x0001CD67
		internal override NewExpression VisitNew(NewExpression nex)
		{
			throw new NotSupportedException(Strings.ALinq_NewNotSupported);
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0001EB73 File Offset: 0x0001CD73
		internal override Expression VisitMemberInit(MemberInitExpression init)
		{
			throw new NotSupportedException(Strings.ALinq_MemberInitNotSupported);
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0001EB7F File Offset: 0x0001CD7F
		internal override Expression VisitListInit(ListInitExpression init)
		{
			throw new NotSupportedException(Strings.ALinq_ListInitNotSupported);
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0001EB8B File Offset: 0x0001CD8B
		internal override Expression VisitNewArray(NewArrayExpression na)
		{
			throw new NotSupportedException(Strings.ALinq_NewArrayNotSupported);
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0001EB97 File Offset: 0x0001CD97
		internal override Expression VisitInvocation(InvocationExpression iv)
		{
			throw new NotSupportedException(Strings.ALinq_InvocationNotSupported);
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x0001EBA3 File Offset: 0x0001CDA3
		internal override Expression VisitNavigationPropertySingletonExpression(NavigationPropertySingletonExpression npse)
		{
			this.Visit(npse.Source);
			this.uriBuilder.Append('/').Append(this.ExpressionToString(npse.MemberExpression, true));
			this.VisitQueryOptions(npse);
			return npse;
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0001EBF8 File Offset: 0x0001CDF8
		internal override Expression VisitResourceSetExpression(ResourceSetExpression rse)
		{
			if (rse.NodeType == (ExpressionType)10001)
			{
				this.Visit(rse.Source);
				this.uriBuilder.Append('/').Append(this.ExpressionToString(rse.MemberExpression, true));
			}
			else
			{
				string entitySetName = (string)((ConstantExpression)rse.MemberExpression).Value;
				this.uriBuilder.Append(this.context.BaseUriResolver.GetEntitySetUri(entitySetName));
			}
			WebUtil.RaiseVersion(ref this.uriVersion, rse.UriVersion);
			if (rse.ResourceTypeAs != null)
			{
				this.uriBuilder.Append('/');
				UriHelper.AppendTypeSegment(this.uriBuilder, rse.ResourceTypeAs, this.context, true, ref this.uriVersion);
			}
			if (rse.KeyPredicateConjuncts.Count > 0)
			{
				this.context.UrlConventions.AppendKeyExpression<KeyValuePair<PropertyInfo, ConstantExpression>>(rse.GetKeyProperties(), (KeyValuePair<PropertyInfo, ConstantExpression> kvp) => kvp.Key.Name, (KeyValuePair<PropertyInfo, ConstantExpression> kvp) => kvp.Value.Value, this.uriBuilder);
			}
			else if (rse == this.leafResourceSet)
			{
				this.uriBuilder.Append('(');
				this.uriBuilder.Append(')');
			}
			if (rse.CountOption == CountOption.ValueOnly)
			{
				this.uriBuilder.Append('/').Append('$').Append("count");
				WebUtil.RaiseVersion(ref this.uriVersion, Util.DataServiceVersion2);
			}
			this.VisitQueryOptions(rse);
			return rse;
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0001ED8C File Offset: 0x0001CF8C
		internal void VisitQueryOptions(ResourceExpression re)
		{
			if (re.HasQueryOptions)
			{
				this.uriBuilder.Append('?');
				ResourceSetExpression resourceSetExpression = re as ResourceSetExpression;
				if (resourceSetExpression != null)
				{
					foreach (object obj in resourceSetExpression.SequenceQueryOptions)
					{
						Expression expression = (Expression)obj;
						switch (expression.NodeType)
						{
						case (ExpressionType)10003:
							this.VisitQueryOptionExpression((TakeQueryOptionExpression)expression);
							break;
						case (ExpressionType)10004:
							this.VisitQueryOptionExpression((SkipQueryOptionExpression)expression);
							break;
						case (ExpressionType)10005:
							this.VisitQueryOptionExpression((OrderByQueryOptionExpression)expression);
							break;
						case (ExpressionType)10006:
							this.VisitQueryOptionExpression((FilterQueryOptionExpression)expression);
							break;
						}
					}
				}
				if (re.ExpandPaths.Count > 0)
				{
					this.VisitExpandOptions(re.ExpandPaths);
				}
				if (re.Projection != null && re.Projection.Paths.Count > 0)
				{
					this.VisitProjectionPaths(re.Projection.Paths);
				}
				if (re.CountOption == CountOption.InlineAll)
				{
					this.VisitCountOptions();
				}
				if (re.CustomQueryOptions.Count > 0)
				{
					this.VisitCustomQueryOptions(re.CustomQueryOptions);
				}
				this.AppendCachedQueryOptionsToUriBuilder();
			}
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0001EEB5 File Offset: 0x0001D0B5
		internal void VisitQueryOptionExpression(SkipQueryOptionExpression sqoe)
		{
			this.AddAsCachedQueryOption('$' + "skip", this.ExpressionToString(sqoe.SkipAmount, false));
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0001EEDB File Offset: 0x0001D0DB
		internal void VisitQueryOptionExpression(TakeQueryOptionExpression tqoe)
		{
			this.AddAsCachedQueryOption('$' + "top", this.ExpressionToString(tqoe.TakeAmount, false));
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0001EF01 File Offset: 0x0001D101
		internal void VisitQueryOptionExpression(FilterQueryOptionExpression fqoe)
		{
			this.AddAsCachedQueryOption('$' + "filter", this.ExpressionToString(fqoe.GetPredicate(), false));
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0001EF28 File Offset: 0x0001D128
		internal void VisitQueryOptionExpression(OrderByQueryOptionExpression oboe)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			for (;;)
			{
				OrderByQueryOptionExpression.Selector selector = oboe.Selectors[num];
				stringBuilder.Append(this.ExpressionToString(selector.Expression, false));
				if (selector.Descending)
				{
					stringBuilder.Append(' ');
					stringBuilder.Append("desc");
				}
				if (++num == oboe.Selectors.Count)
				{
					break;
				}
				stringBuilder.Append(',');
			}
			this.AddAsCachedQueryOption('$' + "orderby", stringBuilder.ToString());
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x0001EFB8 File Offset: 0x0001D1B8
		internal void VisitExpandOptions(List<string> paths)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			for (;;)
			{
				stringBuilder.Append(paths[num]);
				if (++num == paths.Count)
				{
					break;
				}
				stringBuilder.Append(',');
			}
			this.AddAsCachedQueryOption('$' + "expand", stringBuilder.ToString());
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x0001F010 File Offset: 0x0001D210
		internal void VisitProjectionPaths(List<string> paths)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			for (;;)
			{
				string value = paths[num];
				stringBuilder.Append(value);
				if (++num == paths.Count)
				{
					break;
				}
				stringBuilder.Append(',');
			}
			this.AddAsCachedQueryOption('$' + "select", stringBuilder.ToString());
			WebUtil.RaiseVersion(ref this.uriVersion, Util.DataServiceVersion2);
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x0001F07A File Offset: 0x0001D27A
		internal void VisitCountOptions()
		{
			this.AddAsCachedQueryOption('$' + "inlinecount", "allpages");
			WebUtil.RaiseVersion(ref this.uriVersion, Util.DataServiceVersion2);
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x0001F0A8 File Offset: 0x0001D2A8
		internal void VisitCustomQueryOptions(Dictionary<ConstantExpression, ConstantExpression> options)
		{
			List<ConstantExpression> list = options.Keys.ToList<ConstantExpression>();
			List<ConstantExpression> list2 = options.Values.ToList<ConstantExpression>();
			for (int i = 0; i < list.Count; i++)
			{
				string optionKey = string.Concat(list[i].Value);
				string optionValue = string.Concat(list2[i].Value);
				this.AddAsCachedQueryOption(optionKey, optionValue);
			}
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x0001F10C File Offset: 0x0001D30C
		private void AddAsCachedQueryOption(string optionKey, string optionValue)
		{
			List<string> list = null;
			if (!this.cachedQueryOptions.TryGetValue(optionKey, out list))
			{
				list = new List<string>();
				this.cachedQueryOptions.Add(optionKey, list);
			}
			list.Add(optionValue);
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0001F148 File Offset: 0x0001D348
		private void AppendCachedQueryOptionsToUriBuilder()
		{
			int num = 0;
			foreach (KeyValuePair<string, List<string>> keyValuePair in this.cachedQueryOptions)
			{
				if (num++ != 0)
				{
					this.uriBuilder.Append('&');
				}
				string key = keyValuePair.Key;
				string value = string.Join(",", keyValuePair.Value);
				this.uriBuilder.Append(key);
				this.uriBuilder.Append('=');
				this.uriBuilder.Append(value);
				if (key.Equals('$' + "inlinecount", StringComparison.Ordinal) || key.Equals('$' + "select", StringComparison.Ordinal))
				{
					WebUtil.RaiseVersion(ref this.uriVersion, Util.DataServiceVersion2);
				}
			}
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0001F238 File Offset: 0x0001D438
		private string ExpressionToString(Expression expression, bool inPath)
		{
			return ExpressionWriter.ExpressionToString(this.context, expression, inPath, ref this.uriVersion);
		}

		// Token: 0x04000480 RID: 1152
		private readonly DataServiceContext context;

		// Token: 0x04000481 RID: 1153
		private readonly StringBuilder uriBuilder;

		// Token: 0x04000482 RID: 1154
		private Version uriVersion;

		// Token: 0x04000483 RID: 1155
		private ResourceSetExpression leafResourceSet;

		// Token: 0x04000484 RID: 1156
		private Dictionary<string, List<string>> cachedQueryOptions = new Dictionary<string, List<string>>(StringComparer.Ordinal);
	}
}
