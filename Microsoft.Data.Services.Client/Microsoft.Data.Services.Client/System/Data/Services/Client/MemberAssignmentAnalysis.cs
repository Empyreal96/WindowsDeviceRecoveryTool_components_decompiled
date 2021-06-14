using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace System.Data.Services.Client
{
	// Token: 0x02000076 RID: 118
	internal class MemberAssignmentAnalysis : ALinqExpressionVisitor
	{
		// Token: 0x060003E6 RID: 998 RVA: 0x00010C02 File Offset: 0x0000EE02
		private MemberAssignmentAnalysis(Expression entity)
		{
			this.entity = entity;
			this.pathFromEntity = new List<Expression>();
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x00010C1C File Offset: 0x0000EE1C
		internal Exception IncompatibleAssignmentsException
		{
			get
			{
				return this.incompatibleAssignmentsException;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x00010C24 File Offset: 0x0000EE24
		internal bool MultiplePathsFound
		{
			get
			{
				return this.multiplePathsFound;
			}
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00010C2C File Offset: 0x0000EE2C
		internal static MemberAssignmentAnalysis Analyze(Expression entityInScope, Expression assignmentExpression)
		{
			MemberAssignmentAnalysis memberAssignmentAnalysis = new MemberAssignmentAnalysis(entityInScope);
			memberAssignmentAnalysis.Visit(assignmentExpression);
			return memberAssignmentAnalysis;
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00010C4C File Offset: 0x0000EE4C
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

		// Token: 0x060003EB RID: 1003 RVA: 0x00010C79 File Offset: 0x0000EE79
		internal override Expression Visit(Expression expression)
		{
			if (this.multiplePathsFound || this.incompatibleAssignmentsException != null)
			{
				return expression;
			}
			return base.Visit(expression);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00010C94 File Offset: 0x0000EE94
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

		// Token: 0x060003ED RID: 1005 RVA: 0x00010CD2 File Offset: 0x0000EED2
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

		// Token: 0x060003EE RID: 1006 RVA: 0x00010D00 File Offset: 0x0000EF00
		internal override NewExpression VisitNew(NewExpression nex)
		{
			if (nex.Members == null)
			{
				return base.VisitNew(nex);
			}
			MemberAssignmentAnalysis memberAssignmentAnalysis = null;
			foreach (Expression expressionToAssign in nex.Arguments)
			{
				if (!this.CheckCompatibleAssigmentExpression(expressionToAssign, nex.Type, ref memberAssignmentAnalysis))
				{
					break;
				}
			}
			return nex;
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00010D6C File Offset: 0x0000EF6C
		internal override Expression VisitMemberInit(MemberInitExpression init)
		{
			MemberAssignmentAnalysis memberAssignmentAnalysis = null;
			foreach (MemberBinding memberBinding in init.Bindings)
			{
				MemberAssignment memberAssignment = memberBinding as MemberAssignment;
				if (memberAssignment != null && !this.CheckCompatibleAssigmentExpression(memberAssignment.Expression, init.Type, ref memberAssignmentAnalysis))
				{
					break;
				}
			}
			return init;
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00010DDC File Offset: 0x0000EFDC
		internal override Expression VisitMemberAccess(MemberExpression m)
		{
			Expression result = base.VisitMemberAccess(m);
			Type type;
			Expression item = ResourceBinder.StripTo<Expression>(m.Expression, out type);
			if (this.pathFromEntity.Contains(item))
			{
				this.pathFromEntity.Add(m);
			}
			return result;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00010E1A File Offset: 0x0000F01A
		internal override Expression VisitMethodCall(MethodCallExpression call)
		{
			if (ReflectionUtil.IsSequenceMethod(call.Method, SequenceMethod.Select))
			{
				this.Visit(call.Arguments[0]);
				return call;
			}
			return base.VisitMethodCall(call);
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00010E48 File Offset: 0x0000F048
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

		// Token: 0x060003F3 RID: 1011 RVA: 0x00010E8C File Offset: 0x0000F08C
		internal Expression[] GetExpressionsToTargetEntity()
		{
			return this.GetExpressionsToTargetEntity(true);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00010E98 File Offset: 0x0000F098
		internal Expression[] GetExpressionsToTargetEntity(bool ignoreLastExpression)
		{
			int num = ignoreLastExpression ? 1 : 0;
			if (this.pathFromEntity.Count <= num)
			{
				return MemberAssignmentAnalysis.EmptyExpressionArray;
			}
			Expression[] array = new Expression[this.pathFromEntity.Count - num];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.pathFromEntity[i];
			}
			return array;
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x00010EF4 File Offset: 0x0000F0F4
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

		// Token: 0x060003F6 RID: 1014 RVA: 0x00010F88 File Offset: 0x0000F188
		private static Exception CheckCompatibleAssignmentsFail(Type targetType, Expression[] previous, Expression[] candidate)
		{
			string message = Strings.ALinq_ProjectionMemberAssignmentMismatch(targetType.FullName, previous.LastOrDefault<Expression>(), candidate.LastOrDefault<Expression>());
			return new NotSupportedException(message);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00010FB4 File Offset: 0x0000F1B4
		private bool CheckCompatibleAssigmentExpression(Expression expressionToAssign, Type initType, ref MemberAssignmentAnalysis previousNested)
		{
			MemberAssignmentAnalysis memberAssignmentAnalysis = MemberAssignmentAnalysis.Analyze(this.entity, expressionToAssign);
			if (memberAssignmentAnalysis.MultiplePathsFound)
			{
				this.multiplePathsFound = true;
				return false;
			}
			Exception ex = memberAssignmentAnalysis.CheckCompatibleAssignments(initType, ref previousNested);
			if (ex != null)
			{
				this.incompatibleAssignmentsException = ex;
				return false;
			}
			if (this.pathFromEntity.Count == 0)
			{
				this.pathFromEntity.AddRange(memberAssignmentAnalysis.GetExpressionsToTargetEntity());
			}
			return true;
		}

		// Token: 0x040002B9 RID: 697
		internal static readonly Expression[] EmptyExpressionArray = new Expression[0];

		// Token: 0x040002BA RID: 698
		private readonly Expression entity;

		// Token: 0x040002BB RID: 699
		private Exception incompatibleAssignmentsException;

		// Token: 0x040002BC RID: 700
		private bool multiplePathsFound;

		// Token: 0x040002BD RID: 701
		private List<Expression> pathFromEntity;
	}
}
