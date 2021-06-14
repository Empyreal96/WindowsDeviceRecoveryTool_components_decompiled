using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x0200011A RID: 282
	internal class MemberAssignmentAnalysis : ALinqExpressionVisitor
	{
		// Token: 0x0600133D RID: 4925 RVA: 0x00048332 File Offset: 0x00046532
		private MemberAssignmentAnalysis(Expression entity)
		{
			this.entity = entity;
			this.pathFromEntity = new List<Expression>();
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x0600133E RID: 4926 RVA: 0x0004834C File Offset: 0x0004654C
		internal Exception IncompatibleAssignmentsException
		{
			get
			{
				return this.incompatibleAssignmentsException;
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x0600133F RID: 4927 RVA: 0x00048354 File Offset: 0x00046554
		internal bool MultiplePathsFound
		{
			get
			{
				return this.multiplePathsFound;
			}
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x0004835C File Offset: 0x0004655C
		internal static MemberAssignmentAnalysis Analyze(Expression entityInScope, Expression assignmentExpression)
		{
			MemberAssignmentAnalysis memberAssignmentAnalysis = new MemberAssignmentAnalysis(entityInScope);
			memberAssignmentAnalysis.Visit(assignmentExpression);
			return memberAssignmentAnalysis;
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x0004837C File Offset: 0x0004657C
		internal Exception CheckCompatibleAssignments(Type targetType, ref MemberAssignmentAnalysis previous)
		{
			if (previous == null)
			{
				previous = this;
				return null;
			}
			Expression[] expressionsToTargetEntity = previous.GetExpressionsToTargetEntity();
			Expression[] expressionsToTargetEntity2 = this.GetExpressionsToTargetEntity();
			return MemberAssignmentAnalysis.CheckCompatibleAssignments(targetType, expressionsToTargetEntity, expressionsToTargetEntity2);
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x000483A9 File Offset: 0x000465A9
		internal override Expression Visit(Expression expression)
		{
			if (this.multiplePathsFound || this.incompatibleAssignmentsException != null)
			{
				return expression;
			}
			return base.Visit(expression);
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x000483C4 File Offset: 0x000465C4
		internal override Expression VisitConditional(ConditionalExpression c)
		{
			ResourceBinder.PatternRules.MatchNullCheckResult matchNullCheckResult = ResourceBinder.PatternRules.MatchNullCheck(this.entity, c);
			Expression result;
			if (matchNullCheckResult.Match)
			{
				this.Visit(matchNullCheckResult.AssignExpression);
				result = c;
			}
			else
			{
				result = base.VisitConditional(c);
			}
			return result;
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x00048402 File Offset: 0x00046602
		internal override Expression VisitParameter(ParameterExpression p)
		{
			if (p == this.entity)
			{
				if (this.pathFromEntity.Count != 0)
				{
					this.multiplePathsFound = true;
				}
				else
				{
					this.pathFromEntity.Add(p);
				}
			}
			return p;
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x00048430 File Offset: 0x00046630
		internal override Expression VisitMemberInit(MemberInitExpression init)
		{
			MemberAssignmentAnalysis memberAssignmentAnalysis = null;
			foreach (MemberBinding memberBinding in init.Bindings)
			{
				MemberAssignment memberAssignment = memberBinding as MemberAssignment;
				if (memberAssignment != null)
				{
					MemberAssignmentAnalysis memberAssignmentAnalysis2 = MemberAssignmentAnalysis.Analyze(this.entity, memberAssignment.Expression);
					if (memberAssignmentAnalysis2.MultiplePathsFound)
					{
						this.multiplePathsFound = true;
						break;
					}
					Exception ex = memberAssignmentAnalysis2.CheckCompatibleAssignments(init.Type, ref memberAssignmentAnalysis);
					if (ex != null)
					{
						this.incompatibleAssignmentsException = ex;
						break;
					}
					if (this.pathFromEntity.Count == 0)
					{
						this.pathFromEntity.AddRange(memberAssignmentAnalysis2.GetExpressionsToTargetEntity());
					}
				}
			}
			return init;
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x000484F0 File Offset: 0x000466F0
		internal override Expression VisitMemberAccess(MemberExpression m)
		{
			Expression result = base.VisitMemberAccess(m);
			if (this.pathFromEntity.Contains(m.Expression))
			{
				this.pathFromEntity.Add(m);
			}
			return result;
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x00048525 File Offset: 0x00046725
		internal override Expression VisitMethodCall(MethodCallExpression call)
		{
			if (ReflectionUtil.IsSequenceMethod(call.Method, SequenceMethod.Select))
			{
				this.Visit(call.Arguments[0]);
				return call;
			}
			return base.VisitMethodCall(call);
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x00048554 File Offset: 0x00046754
		internal Expression[] GetExpressionsBeyondTargetEntity()
		{
			if (this.pathFromEntity.Count <= 1)
			{
				return MemberAssignmentAnalysis.EmptyExpressionArray;
			}
			return new Expression[]
			{
				this.pathFromEntity[this.pathFromEntity.Count - 1]
			};
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x00048598 File Offset: 0x00046798
		internal Expression[] GetExpressionsToTargetEntity()
		{
			if (this.pathFromEntity.Count <= 1)
			{
				return MemberAssignmentAnalysis.EmptyExpressionArray;
			}
			Expression[] array = new Expression[this.pathFromEntity.Count - 1];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.pathFromEntity[i];
			}
			return array;
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x000485EC File Offset: 0x000467EC
		private static Exception CheckCompatibleAssignments(Type targetType, Expression[] previous, Expression[] candidate)
		{
			if (previous.Length != candidate.Length)
			{
				throw MemberAssignmentAnalysis.CheckCompatibleAssignmentsFail(targetType, previous, candidate);
			}
			for (int i = 0; i < previous.Length; i++)
			{
				Expression expression = previous[i];
				Expression expression2 = candidate[i];
				if (expression.NodeType != expression2.NodeType)
				{
					throw MemberAssignmentAnalysis.CheckCompatibleAssignmentsFail(targetType, previous, candidate);
				}
				if (expression != expression2)
				{
					if (expression.NodeType != ExpressionType.MemberAccess)
					{
						return MemberAssignmentAnalysis.CheckCompatibleAssignmentsFail(targetType, previous, candidate);
					}
					if (((MemberExpression)expression).Member.Name != ((MemberExpression)expression2).Member.Name)
					{
						return MemberAssignmentAnalysis.CheckCompatibleAssignmentsFail(targetType, previous, candidate);
					}
				}
			}
			return null;
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x00048680 File Offset: 0x00046880
		private static Exception CheckCompatibleAssignmentsFail(Type targetType, Expression[] previous, Expression[] candidate)
		{
			return new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Cannot initialize an instance of entity type '{0}' because '{1}' and '{2}' do not refer to the same source entity.", new object[]
			{
				targetType.FullName,
				previous.LastOrDefault<Expression>(),
				candidate.LastOrDefault<Expression>()
			}));
		}

		// Token: 0x040005A4 RID: 1444
		internal static readonly Expression[] EmptyExpressionArray = new Expression[0];

		// Token: 0x040005A5 RID: 1445
		private readonly Expression entity;

		// Token: 0x040005A6 RID: 1446
		private Exception incompatibleAssignmentsException;

		// Token: 0x040005A7 RID: 1447
		private bool multiplePathsFound;

		// Token: 0x040005A8 RID: 1448
		private List<Expression> pathFromEntity;
	}
}
