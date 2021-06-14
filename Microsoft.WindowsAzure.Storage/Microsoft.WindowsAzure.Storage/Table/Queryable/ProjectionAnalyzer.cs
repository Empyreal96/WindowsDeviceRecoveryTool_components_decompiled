using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x0200011F RID: 287
	internal static class ProjectionAnalyzer
	{
		// Token: 0x06001375 RID: 4981 RVA: 0x00048BB4 File Offset: 0x00046DB4
		internal static bool Analyze(LambdaExpression le, ResourceExpression re, bool matchMembers)
		{
			if (le.Body.NodeType == ExpressionType.Constant)
			{
				if (CommonUtil.IsClientType(le.Body.Type))
				{
					throw new NotSupportedException("Referencing of local entity type instances not supported when projecting results.");
				}
				re.Projection = new ProjectionQueryOptionExpression(le.Body.Type, le, new List<string>());
				return true;
			}
			else
			{
				if (le.Body.NodeType == ExpressionType.Call)
				{
					MethodCallExpression methodCallExpression = le.Body as MethodCallExpression;
					if (methodCallExpression.Method == ReflectionUtil.ProjectMethodInfo.MakeGenericMethod(new Type[]
					{
						le.Body.Type
					}))
					{
						ConstantExpression constantExpression = methodCallExpression.Arguments[1] as ConstantExpression;
						re.Projection = new ProjectionQueryOptionExpression(le.Body.Type, ProjectionQueryOptionExpression.DefaultLambda, new List<string>((string[])constantExpression.Value));
						return true;
					}
				}
				if (le.Body.NodeType == ExpressionType.MemberInit || le.Body.NodeType == ExpressionType.New)
				{
					ProjectionAnalyzer.AnalyzeResourceExpression(le, re);
					return true;
				}
				if (matchMembers)
				{
					Expression expression = ProjectionAnalyzer.SkipConverts(le.Body);
					if (expression.NodeType == ExpressionType.MemberAccess)
					{
						ProjectionAnalyzer.AnalyzeResourceExpression(le, re);
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x00048CDC File Offset: 0x00046EDC
		internal static void Analyze(LambdaExpression e, PathBox pb)
		{
			bool flag = CommonUtil.IsClientType(e.Body.Type);
			pb.PushParamExpression(e.Parameters.Last<ParameterExpression>());
			if (!flag)
			{
				ProjectionAnalyzer.NonEntityProjectionAnalyzer.Analyze(e.Body, pb);
			}
			else
			{
				ExpressionType nodeType = e.Body.NodeType;
				if (nodeType == ExpressionType.Constant)
				{
					throw new NotSupportedException("Referencing of local entity type instances not supported when projecting results.");
				}
				if (nodeType != ExpressionType.MemberInit)
				{
					if (nodeType == ExpressionType.New)
					{
						throw new NotSupportedException("Construction of entity type instances must use object initializer with default constructor.");
					}
					ProjectionAnalyzer.NonEntityProjectionAnalyzer.Analyze(e.Body, pb);
				}
				else
				{
					ProjectionAnalyzer.EntityProjectionAnalyzer.Analyze((MemberInitExpression)e.Body, pb);
				}
			}
			pb.PopParamExpression();
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x00048D74 File Offset: 0x00046F74
		internal static bool IsMethodCallAllowedEntitySequence(MethodCallExpression call)
		{
			return ReflectionUtil.IsSequenceMethod(call.Method, SequenceMethod.ToList) || ReflectionUtil.IsSequenceMethod(call.Method, SequenceMethod.Select);
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x00048D98 File Offset: 0x00046F98
		internal static void CheckChainedSequence(MethodCallExpression call, Type type)
		{
			if (ReflectionUtil.IsSequenceMethod(call.Method, SequenceMethod.Select))
			{
				MethodCallExpression methodCallExpression = ResourceBinder.StripTo<MethodCallExpression>(call.Arguments[0]);
				if (methodCallExpression != null && ReflectionUtil.IsSequenceMethod(methodCallExpression.Method, SequenceMethod.Select))
				{
					throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Constructing or initializing instances of the type {0} with the expression {1} is not supported.", new object[]
					{
						type,
						call.ToString()
					}));
				}
			}
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x00048E00 File Offset: 0x00047000
		private static void Analyze(MemberInitExpression mie, PathBox pb)
		{
			bool flag = CommonUtil.IsClientType(mie.Type);
			if (flag)
			{
				ProjectionAnalyzer.EntityProjectionAnalyzer.Analyze(mie, pb);
				return;
			}
			ProjectionAnalyzer.NonEntityProjectionAnalyzer.Analyze(mie, pb);
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x00048E2C File Offset: 0x0004702C
		private static void AnalyzeResourceExpression(LambdaExpression lambda, ResourceExpression resource)
		{
			PathBox pathBox = new PathBox();
			ProjectionAnalyzer.Analyze(lambda, pathBox);
			resource.Projection = new ProjectionQueryOptionExpression(lambda.Body.Type, lambda, pathBox.ProjectionPaths.ToList<string>());
			resource.ExpandPaths = pathBox.ExpandPaths.Union(resource.ExpandPaths, StringComparer.Ordinal).ToList<string>();
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x00048E8C File Offset: 0x0004708C
		private static Expression SkipConverts(Expression expression)
		{
			Expression expression2 = expression;
			while (expression2.NodeType == ExpressionType.Convert || expression2.NodeType == ExpressionType.ConvertChecked)
			{
				expression2 = ((UnaryExpression)expression2).Operand;
			}
			return expression2;
		}

		// Token: 0x02000120 RID: 288
		private class EntityProjectionAnalyzer : ALinqExpressionVisitor
		{
			// Token: 0x0600137C RID: 4988 RVA: 0x00048EBE File Offset: 0x000470BE
			private EntityProjectionAnalyzer(PathBox pb, Type type)
			{
				this.box = pb;
				this.type = type;
			}

			// Token: 0x0600137D RID: 4989 RVA: 0x00048ED4 File Offset: 0x000470D4
			internal static void Analyze(MemberInitExpression mie, PathBox pb)
			{
				ProjectionAnalyzer.EntityProjectionAnalyzer entityProjectionAnalyzer = new ProjectionAnalyzer.EntityProjectionAnalyzer(pb, mie.Type);
				MemberAssignmentAnalysis memberAssignmentAnalysis = null;
				foreach (MemberBinding memberBinding in mie.Bindings)
				{
					MemberAssignment memberAssignment = memberBinding as MemberAssignment;
					entityProjectionAnalyzer.Visit(memberAssignment.Expression);
					if (memberAssignment != null)
					{
						MemberAssignmentAnalysis memberAssignmentAnalysis2 = MemberAssignmentAnalysis.Analyze(pb.ParamExpressionInScope, memberAssignment.Expression);
						if (memberAssignmentAnalysis2.IncompatibleAssignmentsException != null)
						{
							throw memberAssignmentAnalysis2.IncompatibleAssignmentsException;
						}
						Type memberType = ProjectionAnalyzer.EntityProjectionAnalyzer.GetMemberType(memberAssignment.Member);
						Expression[] expressionsBeyondTargetEntity = memberAssignmentAnalysis2.GetExpressionsBeyondTargetEntity();
						if (expressionsBeyondTargetEntity.Length == 0)
						{
							throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Initializing instances of the entity type {0} with the expression {1} is not supported.", new object[]
							{
								memberType,
								memberAssignment.Expression
							}));
						}
						MemberExpression memberExpression = expressionsBeyondTargetEntity[expressionsBeyondTargetEntity.Length - 1] as MemberExpression;
						if (memberExpression != null && memberExpression.Member.Name != memberAssignment.Member.Name)
						{
							throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Cannot assign the value from the {0} property to the {1} property.  When projecting results into a entity type, the property names of the source type and the target type must match for the properties being projected.", new object[]
							{
								memberExpression.Member.Name,
								memberAssignment.Member.Name
							}));
						}
						memberAssignmentAnalysis2.CheckCompatibleAssignments(mie.Type, ref memberAssignmentAnalysis);
						bool flag = CommonUtil.IsClientType(memberType);
						bool flag2 = CommonUtil.IsClientType(memberExpression.Type);
						if (flag2 && !flag)
						{
							throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Constructing or initializing instances of the type {0} with the expression {1} is not supported.", new object[]
							{
								memberType,
								memberAssignment.Expression
							}));
						}
					}
				}
			}

			// Token: 0x0600137E RID: 4990 RVA: 0x00049098 File Offset: 0x00047298
			internal override Expression VisitUnary(UnaryExpression u)
			{
				if (ResourceBinder.PatternRules.MatchConvertToAssignable(u))
				{
					return base.VisitUnary(u);
				}
				if (u.NodeType == ExpressionType.Convert || u.NodeType == ExpressionType.ConvertChecked)
				{
					Type type = Nullable.GetUnderlyingType(u.Operand.Type) ?? u.Operand.Type;
					Type type2 = Nullable.GetUnderlyingType(u.Type) ?? u.Type;
					if (ClientConvert.IsKnownType(type) && ClientConvert.IsKnownType(type2))
					{
						return base.Visit(u.Operand);
					}
				}
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Initializing instances of the entity type {0} with the expression {1} is not supported.", new object[]
				{
					this.type,
					u.ToString()
				}));
			}

			// Token: 0x0600137F RID: 4991 RVA: 0x0004914C File Offset: 0x0004734C
			internal override Expression VisitBinary(BinaryExpression b)
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Initializing instances of the entity type {0} with the expression {1} is not supported.", new object[]
				{
					this.type,
					b.ToString()
				}));
			}

			// Token: 0x06001380 RID: 4992 RVA: 0x00049188 File Offset: 0x00047388
			internal override Expression VisitTypeIs(TypeBinaryExpression b)
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Initializing instances of the entity type {0} with the expression {1} is not supported.", new object[]
				{
					this.type,
					b.ToString()
				}));
			}

			// Token: 0x06001381 RID: 4993 RVA: 0x000491C4 File Offset: 0x000473C4
			internal override Expression VisitConditional(ConditionalExpression c)
			{
				ResourceBinder.PatternRules.MatchNullCheckResult matchNullCheckResult = ResourceBinder.PatternRules.MatchNullCheck(this.box.ParamExpressionInScope, c);
				if (matchNullCheckResult.Match)
				{
					this.Visit(matchNullCheckResult.AssignExpression);
					return c;
				}
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Initializing instances of the entity type {0} with the expression {1} is not supported.", new object[]
				{
					this.type,
					c.ToString()
				}));
			}

			// Token: 0x06001382 RID: 4994 RVA: 0x0004922C File Offset: 0x0004742C
			internal override Expression VisitConstant(ConstantExpression c)
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Initializing instances of the entity type {0} with the expression {1} is not supported.", new object[]
				{
					this.type,
					c.ToString()
				}));
			}

			// Token: 0x06001383 RID: 4995 RVA: 0x00049268 File Offset: 0x00047468
			internal override Expression VisitMemberAccess(MemberExpression m)
			{
				if (!CommonUtil.IsClientType(m.Expression.Type))
				{
					throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Initializing instances of the entity type {0} with the expression {1} is not supported.", new object[]
					{
						this.type,
						m.ToString()
					}));
				}
				PropertyInfo pi = null;
				if (ResourceBinder.PatternRules.MatchNonPrivateReadableProperty(m, out pi))
				{
					Expression result = base.VisitMemberAccess(m);
					this.box.AppendToPath(pi);
					return result;
				}
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Initializing instances of the entity type {0} with the expression {1} is not supported.", new object[]
				{
					this.type,
					m.ToString()
				}));
			}

			// Token: 0x06001384 RID: 4996 RVA: 0x00049308 File Offset: 0x00047508
			internal override Expression VisitMethodCall(MethodCallExpression m)
			{
				if (ProjectionAnalyzer.IsMethodCallAllowedEntitySequence(m))
				{
					ProjectionAnalyzer.CheckChainedSequence(m, this.type);
					return base.VisitMethodCall(m);
				}
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Initializing instances of the entity type {0} with the expression {1} is not supported.", new object[]
				{
					this.type,
					m.ToString()
				}));
			}

			// Token: 0x06001385 RID: 4997 RVA: 0x00049360 File Offset: 0x00047560
			internal override Expression VisitInvocation(InvocationExpression iv)
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Initializing instances of the entity type {0} with the expression {1} is not supported.", new object[]
				{
					this.type,
					iv.ToString()
				}));
			}

			// Token: 0x06001386 RID: 4998 RVA: 0x0004939B File Offset: 0x0004759B
			internal override Expression VisitLambda(LambdaExpression lambda)
			{
				ProjectionAnalyzer.Analyze(lambda, this.box);
				return lambda;
			}

			// Token: 0x06001387 RID: 4999 RVA: 0x000493AC File Offset: 0x000475AC
			internal override Expression VisitListInit(ListInitExpression init)
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Initializing instances of the entity type {0} with the expression {1} is not supported.", new object[]
				{
					this.type,
					init.ToString()
				}));
			}

			// Token: 0x06001388 RID: 5000 RVA: 0x000493E8 File Offset: 0x000475E8
			internal override Expression VisitNewArray(NewArrayExpression na)
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Initializing instances of the entity type {0} with the expression {1} is not supported.", new object[]
				{
					this.type,
					na.ToString()
				}));
			}

			// Token: 0x06001389 RID: 5001 RVA: 0x00049423 File Offset: 0x00047623
			internal override Expression VisitMemberInit(MemberInitExpression init)
			{
				ProjectionAnalyzer.Analyze(init, this.box);
				return init;
			}

			// Token: 0x0600138A RID: 5002 RVA: 0x00049434 File Offset: 0x00047634
			internal override NewExpression VisitNew(NewExpression nex)
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Initializing instances of the entity type {0} with the expression {1} is not supported.", new object[]
				{
					this.type,
					nex.ToString()
				}));
			}

			// Token: 0x0600138B RID: 5003 RVA: 0x0004946F File Offset: 0x0004766F
			internal override Expression VisitParameter(ParameterExpression p)
			{
				if (p != this.box.ParamExpressionInScope)
				{
					throw new NotSupportedException("Can only project the last entity type in the query being translated.");
				}
				this.box.StartNewPath();
				return p;
			}

			// Token: 0x0600138C RID: 5004 RVA: 0x00049498 File Offset: 0x00047698
			private static Type GetMemberType(MemberInfo member)
			{
				PropertyInfo propertyInfo = member as PropertyInfo;
				if (propertyInfo != null)
				{
					return propertyInfo.PropertyType;
				}
				FieldInfo fieldInfo = member as FieldInfo;
				return fieldInfo.FieldType;
			}

			// Token: 0x040005BD RID: 1469
			private readonly PathBox box;

			// Token: 0x040005BE RID: 1470
			private readonly Type type;
		}

		// Token: 0x02000121 RID: 289
		private class NonEntityProjectionAnalyzer : DataServiceALinqExpressionVisitor
		{
			// Token: 0x0600138D RID: 5005 RVA: 0x000494C9 File Offset: 0x000476C9
			private NonEntityProjectionAnalyzer(PathBox pb, Type type)
			{
				this.box = pb;
				this.type = type;
			}

			// Token: 0x0600138E RID: 5006 RVA: 0x000494E0 File Offset: 0x000476E0
			internal static void Analyze(Expression e, PathBox pb)
			{
				ProjectionAnalyzer.NonEntityProjectionAnalyzer nonEntityProjectionAnalyzer = new ProjectionAnalyzer.NonEntityProjectionAnalyzer(pb, e.Type);
				MemberInitExpression memberInitExpression = e as MemberInitExpression;
				if (memberInitExpression != null)
				{
					using (IEnumerator<MemberBinding> enumerator = memberInitExpression.Bindings.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							MemberBinding memberBinding = enumerator.Current;
							MemberAssignment memberAssignment = memberBinding as MemberAssignment;
							if (memberAssignment != null)
							{
								nonEntityProjectionAnalyzer.Visit(memberAssignment.Expression);
							}
						}
						return;
					}
				}
				nonEntityProjectionAnalyzer.Visit(e);
			}

			// Token: 0x0600138F RID: 5007 RVA: 0x00049564 File Offset: 0x00047764
			internal override Expression VisitUnary(UnaryExpression u)
			{
				if (!ResourceBinder.PatternRules.MatchConvertToAssignable(u) && CommonUtil.IsClientType(u.Operand.Type))
				{
					throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Constructing or initializing instances of the type {0} with the expression {1} is not supported.", new object[]
					{
						this.type,
						u.ToString()
					}));
				}
				return base.VisitUnary(u);
			}

			// Token: 0x06001390 RID: 5008 RVA: 0x000495C4 File Offset: 0x000477C4
			internal override Expression VisitBinary(BinaryExpression b)
			{
				if (CommonUtil.IsClientType(b.Left.Type) || CommonUtil.IsClientType(b.Right.Type))
				{
					throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Constructing or initializing instances of the type {0} with the expression {1} is not supported.", new object[]
					{
						this.type,
						b.ToString()
					}));
				}
				return base.VisitBinary(b);
			}

			// Token: 0x06001391 RID: 5009 RVA: 0x0004962C File Offset: 0x0004782C
			internal override Expression VisitTypeIs(TypeBinaryExpression b)
			{
				if (CommonUtil.IsClientType(b.Expression.Type))
				{
					throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Constructing or initializing instances of the type {0} with the expression {1} is not supported.", new object[]
					{
						this.type,
						b.ToString()
					}));
				}
				return base.VisitTypeIs(b);
			}

			// Token: 0x06001392 RID: 5010 RVA: 0x00049684 File Offset: 0x00047884
			internal override Expression VisitConditional(ConditionalExpression c)
			{
				ResourceBinder.PatternRules.MatchNullCheckResult matchNullCheckResult = ResourceBinder.PatternRules.MatchNullCheck(this.box.ParamExpressionInScope, c);
				if (matchNullCheckResult.Match)
				{
					this.Visit(matchNullCheckResult.AssignExpression);
					return c;
				}
				if (CommonUtil.IsClientType(c.Test.Type) || CommonUtil.IsClientType(c.IfTrue.Type) || CommonUtil.IsClientType(c.IfFalse.Type))
				{
					throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Constructing or initializing instances of the type {0} with the expression {1} is not supported.", new object[]
					{
						this.type,
						c.ToString()
					}));
				}
				return base.VisitConditional(c);
			}

			// Token: 0x06001393 RID: 5011 RVA: 0x00049728 File Offset: 0x00047928
			internal override Expression VisitMemberAccess(MemberExpression m)
			{
				if (ClientConvert.IsKnownNullableType(m.Expression.Type))
				{
					return base.VisitMemberAccess(m);
				}
				if (!CommonUtil.IsClientType(m.Expression.Type))
				{
					throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Constructing or initializing instances of the type {0} with the expression {1} is not supported.", new object[]
					{
						this.type,
						m.ToString()
					}));
				}
				PropertyInfo pi = null;
				if (ResourceBinder.PatternRules.MatchNonPrivateReadableProperty(m, out pi))
				{
					Expression result = base.VisitMemberAccess(m);
					this.box.AppendToPath(pi);
					return result;
				}
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Constructing or initializing instances of the type {0} with the expression {1} is not supported.", new object[]
				{
					this.type,
					m.ToString()
				}));
			}

			// Token: 0x06001394 RID: 5012 RVA: 0x000497F0 File Offset: 0x000479F0
			internal override Expression VisitMethodCall(MethodCallExpression m)
			{
				if (ProjectionAnalyzer.IsMethodCallAllowedEntitySequence(m))
				{
					ProjectionAnalyzer.CheckChainedSequence(m, this.type);
					return base.VisitMethodCall(m);
				}
				if (m.Object == null || !CommonUtil.IsClientType(m.Object.Type))
				{
					if (!m.Arguments.Any((Expression a) => CommonUtil.IsClientType(a.Type)))
					{
						return base.VisitMethodCall(m);
					}
				}
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Constructing or initializing instances of the type {0} with the expression {1} is not supported.", new object[]
				{
					this.type,
					m.ToString()
				}));
			}

			// Token: 0x06001395 RID: 5013 RVA: 0x000498A4 File Offset: 0x00047AA4
			internal override Expression VisitInvocation(InvocationExpression iv)
			{
				if (!CommonUtil.IsClientType(iv.Expression.Type))
				{
					if (!iv.Arguments.Any((Expression a) => CommonUtil.IsClientType(a.Type)))
					{
						return base.VisitInvocation(iv);
					}
				}
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Constructing or initializing instances of the type {0} with the expression {1} is not supported.", new object[]
				{
					this.type,
					iv.ToString()
				}));
			}

			// Token: 0x06001396 RID: 5014 RVA: 0x00049923 File Offset: 0x00047B23
			internal override Expression VisitLambda(LambdaExpression lambda)
			{
				ProjectionAnalyzer.Analyze(lambda, this.box);
				return lambda;
			}

			// Token: 0x06001397 RID: 5015 RVA: 0x00049932 File Offset: 0x00047B32
			internal override Expression VisitMemberInit(MemberInitExpression init)
			{
				ProjectionAnalyzer.Analyze(init, this.box);
				return init;
			}

			// Token: 0x06001398 RID: 5016 RVA: 0x00049944 File Offset: 0x00047B44
			internal override NewExpression VisitNew(NewExpression nex)
			{
				if (CommonUtil.IsClientType(nex.Type))
				{
					throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Constructing or initializing instances of the type {0} with the expression {1} is not supported.", new object[]
					{
						this.type,
						nex.ToString()
					}));
				}
				return base.VisitNew(nex);
			}

			// Token: 0x06001399 RID: 5017 RVA: 0x00049994 File Offset: 0x00047B94
			internal override Expression VisitParameter(ParameterExpression p)
			{
				if (p != this.box.ParamExpressionInScope)
				{
					throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Constructing or initializing instances of the type {0} with the expression {1} is not supported.", new object[]
					{
						this.type,
						p.ToString()
					}));
				}
				this.box.StartNewPath();
				return p;
			}

			// Token: 0x0600139A RID: 5018 RVA: 0x000499EC File Offset: 0x00047BEC
			internal override Expression VisitConstant(ConstantExpression c)
			{
				if (CommonUtil.IsClientType(c.Type))
				{
					throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Constructing or initializing instances of the type {0} with the expression {1} is not supported.", new object[]
					{
						this.type,
						c.ToString()
					}));
				}
				return base.VisitConstant(c);
			}

			// Token: 0x040005BF RID: 1471
			private PathBox box;

			// Token: 0x040005C0 RID: 1472
			private Type type;
		}
	}
}
