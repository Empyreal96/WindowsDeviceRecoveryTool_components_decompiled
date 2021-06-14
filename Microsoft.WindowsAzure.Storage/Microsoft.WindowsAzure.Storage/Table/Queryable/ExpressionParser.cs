using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x02000115 RID: 277
	internal class ExpressionParser : DataServiceALinqExpressionVisitor
	{
		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x060012F5 RID: 4853 RVA: 0x000473CC File Offset: 0x000455CC
		// (set) Token: 0x060012F6 RID: 4854 RVA: 0x000473D4 File Offset: 0x000455D4
		internal TableRequestOptions RequestOptions { get; set; }

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x060012F7 RID: 4855 RVA: 0x000473DD File Offset: 0x000455DD
		// (set) Token: 0x060012F8 RID: 4856 RVA: 0x000473E5 File Offset: 0x000455E5
		internal OperationContext OperationContext { get; set; }

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x060012F9 RID: 4857 RVA: 0x000473EE File Offset: 0x000455EE
		// (set) Token: 0x060012FA RID: 4858 RVA: 0x000473F6 File Offset: 0x000455F6
		internal ConstantExpression Resolver { get; set; }

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x060012FB RID: 4859 RVA: 0x000473FF File Offset: 0x000455FF
		// (set) Token: 0x060012FC RID: 4860 RVA: 0x00047407 File Offset: 0x00045607
		internal ProjectionQueryOptionExpression Projection { get; set; }

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x060012FD RID: 4861 RVA: 0x00047410 File Offset: 0x00045610
		// (set) Token: 0x060012FE RID: 4862 RVA: 0x00047418 File Offset: 0x00045618
		public int? TakeCount
		{
			get
			{
				return this.takeCount;
			}
			set
			{
				if (value != null && value.Value <= 0)
				{
					throw new ArgumentException("Take count must be positive and greater than 0.");
				}
				this.takeCount = value;
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x060012FF RID: 4863 RVA: 0x0004743F File Offset: 0x0004563F
		// (set) Token: 0x06001300 RID: 4864 RVA: 0x00047447 File Offset: 0x00045647
		public string FilterString { get; set; }

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06001301 RID: 4865 RVA: 0x00047450 File Offset: 0x00045650
		// (set) Token: 0x06001302 RID: 4866 RVA: 0x00047458 File Offset: 0x00045658
		public IList<string> SelectColumns { get; set; }

		// Token: 0x06001303 RID: 4867 RVA: 0x00047461 File Offset: 0x00045661
		internal ExpressionParser()
		{
			this.SelectColumns = new List<string>();
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x00047480 File Offset: 0x00045680
		internal void Translate(Expression e)
		{
			this.Visit(e);
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x0004748C File Offset: 0x0004568C
		internal override Expression VisitMethodCall(MethodCallExpression m)
		{
			throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "The method '{0}' is not supported.", new object[]
			{
				m.Method.Name
			}));
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x000474C4 File Offset: 0x000456C4
		internal override Expression VisitUnary(UnaryExpression u)
		{
			throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "The unary operator '{0}' is not supported.", new object[]
			{
				u.NodeType.ToString()
			}));
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x00047500 File Offset: 0x00045700
		internal override Expression VisitBinary(BinaryExpression b)
		{
			throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "The binary operator '{0}' is not supported.", new object[]
			{
				b.NodeType.ToString()
			}));
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x0004753C File Offset: 0x0004573C
		internal override Expression VisitConstant(ConstantExpression c)
		{
			throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "The constant for '{0}' is not supported.", new object[]
			{
				c.Value
			}));
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x0004756E File Offset: 0x0004576E
		internal override Expression VisitTypeIs(TypeBinaryExpression b)
		{
			throw new NotSupportedException("An operation between an expression and a type is not supported.");
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x0004757A File Offset: 0x0004577A
		internal override Expression VisitConditional(ConditionalExpression c)
		{
			throw new NotSupportedException("The conditional expression is not supported.");
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x00047586 File Offset: 0x00045786
		internal override Expression VisitParameter(ParameterExpression p)
		{
			throw new NotSupportedException("The parameter expression is not supported.");
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x00047594 File Offset: 0x00045794
		internal override Expression VisitMemberAccess(MemberExpression m)
		{
			throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "The member access of '{0}' is not supported.", new object[]
			{
				m.Member.Name
			}));
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x000475CB File Offset: 0x000457CB
		internal override Expression VisitLambda(LambdaExpression lambda)
		{
			throw new NotSupportedException("Lambda Expressions not supported.");
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x000475D7 File Offset: 0x000457D7
		internal override NewExpression VisitNew(NewExpression nex)
		{
			throw new NotSupportedException("New Expressions not supported.");
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x000475E3 File Offset: 0x000457E3
		internal override Expression VisitMemberInit(MemberInitExpression init)
		{
			throw new NotSupportedException("Member Init Expressions not supported.");
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x000475EF File Offset: 0x000457EF
		internal override Expression VisitListInit(ListInitExpression init)
		{
			throw new NotSupportedException("List Init Expressions not supported.");
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x000475FB File Offset: 0x000457FB
		internal override Expression VisitNewArray(NewArrayExpression na)
		{
			throw new NotSupportedException("New Array Expressions not supported.");
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x00047607 File Offset: 0x00045807
		internal override Expression VisitInvocation(InvocationExpression iv)
		{
			throw new NotSupportedException("Invocation Expressions not supported.");
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x00047613 File Offset: 0x00045813
		internal override Expression VisitNavigationPropertySingletonExpression(NavigationPropertySingletonExpression npse)
		{
			throw new NotSupportedException("Navigation not supported.");
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x0004761F File Offset: 0x0004581F
		internal override Expression VisitResourceSetExpression(ResourceSetExpression rse)
		{
			this.VisitQueryOptions(rse);
			return rse;
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x0004762C File Offset: 0x0004582C
		internal void VisitQueryOptions(ResourceExpression re)
		{
			if (re.HasQueryOptions)
			{
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
						case (ExpressionType)10006:
							this.VisitQueryOptionExpression((FilterQueryOptionExpression)expression);
							break;
						case (ExpressionType)10009:
							this.VisitQueryOptionExpression((RequestOptionsQueryOptionExpression)expression);
							break;
						case (ExpressionType)10010:
							this.VisitQueryOptionExpression((OperationContextQueryOptionExpression)expression);
							break;
						case (ExpressionType)10011:
							this.VisitQueryOptionExpression((EntityResolverQueryOptionExpression)expression);
							break;
						}
					}
				}
				if (re.Projection != null && re.Projection.Paths.Count > 0)
				{
					this.Projection = re.Projection;
					this.SelectColumns = re.Projection.Paths;
				}
				if (re.CustomQueryOptions.Count > 0)
				{
					this.VisitCustomQueryOptions(re.CustomQueryOptions);
				}
			}
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x0004774C File Offset: 0x0004594C
		internal void VisitQueryOptionExpression(RequestOptionsQueryOptionExpression roqoe)
		{
			this.RequestOptions = (TableRequestOptions)roqoe.RequestOptions.Value;
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x00047764 File Offset: 0x00045964
		internal void VisitQueryOptionExpression(OperationContextQueryOptionExpression ocqoe)
		{
			this.OperationContext = (OperationContext)ocqoe.OperationContext.Value;
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x0004777C File Offset: 0x0004597C
		internal void VisitQueryOptionExpression(EntityResolverQueryOptionExpression erqoe)
		{
			this.Resolver = erqoe.Resolver;
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x0004778A File Offset: 0x0004598A
		internal void VisitQueryOptionExpression(TakeQueryOptionExpression tqoe)
		{
			this.TakeCount = new int?((int)tqoe.TakeAmount.Value);
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x000477A7 File Offset: 0x000459A7
		internal void VisitQueryOptionExpression(FilterQueryOptionExpression fqoe)
		{
			this.FilterString = ExpressionParser.ExpressionToString(fqoe.Predicate);
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x000477BA File Offset: 0x000459BA
		internal void VisitCustomQueryOptions(Dictionary<ConstantExpression, ConstantExpression> options)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x000477C1 File Offset: 0x000459C1
		private static string ExpressionToString(Expression expression)
		{
			return ExpressionWriter.ExpressionToString(expression);
		}

		// Token: 0x04000593 RID: 1427
		private int? takeCount = null;
	}
}
