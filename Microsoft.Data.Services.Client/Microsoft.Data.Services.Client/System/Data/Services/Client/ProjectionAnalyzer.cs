using System;
using System.Collections.Generic;
using System.Data.Services.Client.Metadata;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Services.Client
{
	// Token: 0x020000B6 RID: 182
	internal static class ProjectionAnalyzer
	{
		// Token: 0x060005CD RID: 1485 RVA: 0x000161A0 File Offset: 0x000143A0
		internal static bool Analyze(LambdaExpression le, ResourceExpression re, bool matchMembers, DataServiceContext context)
		{
			if (le.Body.NodeType == ExpressionType.Constant)
			{
				if (ClientTypeUtil.TypeOrElementTypeIsEntity(le.Body.Type))
				{
					throw new NotSupportedException(Strings.ALinq_CannotCreateConstantEntity);
				}
				re.Projection = new ProjectionQueryOptionExpression(le.Body.Type, le, new List<string>());
				return true;
			}
			else
			{
				if (le.Body.NodeType == ExpressionType.MemberInit || le.Body.NodeType == ExpressionType.New)
				{
					ProjectionAnalyzer.AnalyzeResourceExpression(le, re, context);
					return true;
				}
				if (matchMembers)
				{
					Expression expression = ProjectionAnalyzer.SkipConverts(le.Body);
					if (expression.NodeType == ExpressionType.MemberAccess)
					{
						ProjectionAnalyzer.AnalyzeResourceExpression(le, re, context);
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00016244 File Offset: 0x00014444
		private static void Analyze(LambdaExpression e, PathBox pb, DataServiceContext context)
		{
			bool flag = ClientTypeUtil.TypeOrElementTypeIsEntity(e.Body.Type);
			ParameterExpression parameterExpression = e.Parameters.Last<ParameterExpression>();
			bool flag2 = ClientTypeUtil.TypeOrElementTypeIsEntity(parameterExpression.Type);
			if (flag2)
			{
				pb.PushParamExpression(parameterExpression);
			}
			if (!flag)
			{
				ProjectionAnalyzer.NonEntityProjectionAnalyzer.Analyze(e.Body, pb, context);
			}
			else
			{
				ExpressionType nodeType = e.Body.NodeType;
				if (nodeType == ExpressionType.Constant)
				{
					throw new NotSupportedException(Strings.ALinq_CannotCreateConstantEntity);
				}
				if (nodeType != ExpressionType.MemberInit)
				{
					if (nodeType == ExpressionType.New)
					{
						throw new NotSupportedException(Strings.ALinq_CannotConstructKnownEntityTypes);
					}
					ProjectionAnalyzer.NonEntityProjectionAnalyzer.Analyze(e.Body, pb, context);
				}
				else
				{
					ProjectionAnalyzer.EntityProjectionAnalyzer.Analyze((MemberInitExpression)e.Body, pb, context);
				}
			}
			if (flag2)
			{
				pb.PopParamExpression();
			}
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x000162F3 File Offset: 0x000144F3
		internal static bool IsMethodCallAllowedEntitySequence(MethodCallExpression call)
		{
			return ReflectionUtil.IsSequenceMethod(call.Method, SequenceMethod.ToList) || ReflectionUtil.IsSequenceMethod(call.Method, SequenceMethod.Select);
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00016318 File Offset: 0x00014518
		internal static void CheckChainedSequence(MethodCallExpression call, Type type)
		{
			if (ReflectionUtil.IsSequenceSelectMethod(call.Method))
			{
				MethodCallExpression methodCallExpression = ResourceBinder.StripTo<MethodCallExpression>(call.Arguments[0]);
				if (methodCallExpression != null && ReflectionUtil.IsSequenceSelectMethod(methodCallExpression.Method))
				{
					throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjection(type, call.ToString()));
				}
			}
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00016368 File Offset: 0x00014568
		internal static bool IsCollectionProducingExpression(Expression e)
		{
			if (TypeSystem.FindIEnumerable(e.Type) != null)
			{
				Type elementType = TypeSystem.GetElementType(e.Type);
				Type dataServiceCollectionOfT = WebUtil.GetDataServiceCollectionOfT(new Type[]
				{
					elementType
				});
				if (typeof(List<>).MakeGenericType(new Type[]
				{
					elementType
				}).IsAssignableFrom(e.Type) || (dataServiceCollectionOfT != null && dataServiceCollectionOfT.IsAssignableFrom(e.Type)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x000163E8 File Offset: 0x000145E8
		internal static bool IsDisallowedExpressionForMethodCall(Expression e, ClientEdmModel model)
		{
			MemberExpression memberExpression = e as MemberExpression;
			return (memberExpression == null || !ClientTypeUtil.TypeIsEntity(memberExpression.Expression.Type, model)) && ProjectionAnalyzer.IsCollectionProducingExpression(e);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0001641C File Offset: 0x0001461C
		private static void Analyze(MemberInitExpression mie, PathBox pb, DataServiceContext context)
		{
			bool flag = ClientTypeUtil.TypeOrElementTypeIsEntity(mie.Type);
			if (flag)
			{
				ProjectionAnalyzer.EntityProjectionAnalyzer.Analyze(mie, pb, context);
				return;
			}
			ProjectionAnalyzer.NonEntityProjectionAnalyzer.Analyze(mie, pb, context);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0001644C File Offset: 0x0001464C
		private static void AnalyzeResourceExpression(LambdaExpression lambda, ResourceExpression resource, DataServiceContext context)
		{
			PathBox pathBox = new PathBox();
			ProjectionAnalyzer.Analyze(lambda, pathBox, context);
			resource.Projection = new ProjectionQueryOptionExpression(lambda.Body.Type, lambda, pathBox.ProjectionPaths.ToList<string>());
			resource.ExpandPaths = pathBox.ExpandPaths.Union(resource.ExpandPaths, StringComparer.Ordinal).ToList<string>();
			resource.RaiseUriVersion(pathBox.UriVersion);
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x000164B8 File Offset: 0x000146B8
		private static Expression SkipConverts(Expression expression)
		{
			Expression expression2 = expression;
			while (expression2.NodeType == ExpressionType.Convert || expression2.NodeType == ExpressionType.ConvertChecked)
			{
				expression2 = ((UnaryExpression)expression2).Operand;
			}
			return expression2;
		}

		// Token: 0x020000B7 RID: 183
		private class EntityProjectionAnalyzer : ALinqExpressionVisitor
		{
			// Token: 0x060005D6 RID: 1494 RVA: 0x000164EA File Offset: 0x000146EA
			private EntityProjectionAnalyzer(PathBox pb, Type type, DataServiceContext context)
			{
				this.box = pb;
				this.type = type;
				this.context = context;
			}

			// Token: 0x060005D7 RID: 1495 RVA: 0x00016508 File Offset: 0x00014708
			internal static void Analyze(MemberInitExpression mie, PathBox pb, DataServiceContext context)
			{
				ProjectionAnalyzer.EntityProjectionAnalyzer entityProjectionAnalyzer = new ProjectionAnalyzer.EntityProjectionAnalyzer(pb, mie.Type, context);
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
						Type memberType = ClientTypeUtil.GetMemberType(memberAssignment.Member);
						Expression[] expressionsBeyondTargetEntity = memberAssignmentAnalysis2.GetExpressionsBeyondTargetEntity();
						if (expressionsBeyondTargetEntity.Length == 0)
						{
							throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjectionToEntity(memberType, memberAssignment.Expression));
						}
						MemberExpression memberExpression = expressionsBeyondTargetEntity[expressionsBeyondTargetEntity.Length - 1] as MemberExpression;
						memberAssignmentAnalysis2.CheckCompatibleAssignments(mie.Type, ref memberAssignmentAnalysis);
						if (memberExpression != null)
						{
							if (memberExpression.Member.Name != memberAssignment.Member.Name)
							{
								throw new NotSupportedException(Strings.ALinq_PropertyNamesMustMatchInProjections(memberExpression.Member.Name, memberAssignment.Member.Name));
							}
							bool flag = ClientTypeUtil.TypeOrElementTypeIsEntity(memberType);
							bool flag2 = ClientTypeUtil.TypeOrElementTypeIsEntity(memberExpression.Type);
							if (flag2 && !flag)
							{
								throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjection(memberType, memberAssignment.Expression));
							}
						}
					}
				}
			}

			// Token: 0x060005D8 RID: 1496 RVA: 0x00016678 File Offset: 0x00014878
			internal override Expression VisitUnary(UnaryExpression u)
			{
				if (ResourceBinder.PatternRules.MatchConvertToAssignable(u) || (u.NodeType == ExpressionType.TypeAs && this.leafExpressionIsMemberAccess))
				{
					return base.VisitUnary(u);
				}
				if (u.NodeType == ExpressionType.Convert || u.NodeType == ExpressionType.ConvertChecked)
				{
					Type type = Nullable.GetUnderlyingType(u.Operand.Type) ?? u.Operand.Type;
					Type type2 = Nullable.GetUnderlyingType(u.Type) ?? u.Type;
					if (PrimitiveType.IsKnownType(type) && PrimitiveType.IsKnownType(type2))
					{
						return base.Visit(u.Operand);
					}
				}
				throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjectionToEntity(this.type, u.ToString()));
			}

			// Token: 0x060005D9 RID: 1497 RVA: 0x00016724 File Offset: 0x00014924
			internal override Expression VisitBinary(BinaryExpression b)
			{
				throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjectionToEntity(this.type, b.ToString()));
			}

			// Token: 0x060005DA RID: 1498 RVA: 0x0001673C File Offset: 0x0001493C
			internal override Expression VisitTypeIs(TypeBinaryExpression b)
			{
				throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjectionToEntity(this.type, b.ToString()));
			}

			// Token: 0x060005DB RID: 1499 RVA: 0x00016754 File Offset: 0x00014954
			internal override Expression VisitConditional(ConditionalExpression c)
			{
				ResourceBinder.PatternRules.MatchNullCheckResult matchNullCheckResult = ResourceBinder.PatternRules.MatchNullCheck(this.box.ParamExpressionInScope, c);
				if (matchNullCheckResult.Match)
				{
					this.Visit(matchNullCheckResult.AssignExpression);
					return c;
				}
				throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjectionToEntity(this.type, c.ToString()));
			}

			// Token: 0x060005DC RID: 1500 RVA: 0x000167A2 File Offset: 0x000149A2
			internal override Expression VisitConstant(ConstantExpression c)
			{
				throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjectionToEntity(this.type, c.ToString()));
			}

			// Token: 0x060005DD RID: 1501 RVA: 0x000167BC File Offset: 0x000149BC
			internal override Expression VisitMemberAccess(MemberExpression m)
			{
				this.leafExpressionIsMemberAccess = true;
				if (!ClientTypeUtil.TypeOrElementTypeIsEntity(m.Expression.Type) || ProjectionAnalyzer.IsCollectionProducingExpression(m.Expression))
				{
					throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjectionToEntity(this.type, m.ToString()));
				}
				PropertyInfo pi;
				Expression expression;
				if (ResourceBinder.PatternRules.MatchNonPrivateReadableProperty(m, out pi, out expression))
				{
					Expression result = base.VisitMemberAccess(m);
					Type convertedSourceType;
					ResourceBinder.StripTo<Expression>(m.Expression, out convertedSourceType);
					this.box.AppendPropertyToPath(pi, convertedSourceType, this.context);
					this.leafExpressionIsMemberAccess = false;
					return result;
				}
				throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjectionToEntity(this.type, m.ToString()));
			}

			// Token: 0x060005DE RID: 1502 RVA: 0x00016870 File Offset: 0x00014A70
			internal override Expression VisitMethodCall(MethodCallExpression m)
			{
				if ((m.Object != null && (ProjectionAnalyzer.IsDisallowedExpressionForMethodCall(m.Object, this.context.Model) || !ClientTypeUtil.TypeOrElementTypeIsEntity(m.Object.Type))) || m.Arguments.Any((Expression a) => ProjectionAnalyzer.IsDisallowedExpressionForMethodCall(a, this.context.Model)) || (m.Object == null && !ClientTypeUtil.TypeOrElementTypeIsEntity(m.Arguments[0].Type)))
				{
					throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjection(this.type, m.ToString()));
				}
				if (ProjectionAnalyzer.IsMethodCallAllowedEntitySequence(m))
				{
					ProjectionAnalyzer.CheckChainedSequence(m, this.type);
					return base.VisitMethodCall(m);
				}
				throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjectionToEntity(this.type, m.ToString()));
			}

			// Token: 0x060005DF RID: 1503 RVA: 0x00016931 File Offset: 0x00014B31
			internal override Expression VisitInvocation(InvocationExpression iv)
			{
				throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjectionToEntity(this.type, iv.ToString()));
			}

			// Token: 0x060005E0 RID: 1504 RVA: 0x00016949 File Offset: 0x00014B49
			internal override Expression VisitLambda(LambdaExpression lambda)
			{
				ProjectionAnalyzer.Analyze(lambda, this.box, this.context);
				return lambda;
			}

			// Token: 0x060005E1 RID: 1505 RVA: 0x0001695E File Offset: 0x00014B5E
			internal override Expression VisitListInit(ListInitExpression init)
			{
				throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjectionToEntity(this.type, init.ToString()));
			}

			// Token: 0x060005E2 RID: 1506 RVA: 0x00016976 File Offset: 0x00014B76
			internal override Expression VisitNewArray(NewArrayExpression na)
			{
				throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjectionToEntity(this.type, na.ToString()));
			}

			// Token: 0x060005E3 RID: 1507 RVA: 0x0001698E File Offset: 0x00014B8E
			internal override Expression VisitMemberInit(MemberInitExpression init)
			{
				if (!ClientTypeUtil.TypeOrElementTypeIsEntity(init.Type))
				{
					throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjectionToEntity(this.type, init.ToString()));
				}
				ProjectionAnalyzer.Analyze(init, this.box, this.context);
				return init;
			}

			// Token: 0x060005E4 RID: 1508 RVA: 0x000169C8 File Offset: 0x00014BC8
			internal override NewExpression VisitNew(NewExpression nex)
			{
				if (ResourceBinder.PatternRules.MatchNewDataServiceCollectionOfT(nex))
				{
					if (ClientTypeUtil.TypeOrElementTypeIsEntity(nex.Type))
					{
						foreach (Expression expression in nex.Arguments)
						{
							if (expression.NodeType != ExpressionType.Constant)
							{
								base.Visit(expression);
							}
						}
						return nex;
					}
				}
				else if (ResourceBinder.PatternRules.MatchNewCollectionOfT(nex) && !ClientTypeUtil.TypeOrElementTypeIsEntity(nex.Type))
				{
					foreach (Expression expression2 in nex.Arguments)
					{
						if (expression2.NodeType != ExpressionType.Constant)
						{
							base.Visit(expression2);
						}
					}
					return nex;
				}
				throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjectionToEntity(this.type, nex.ToString()));
			}

			// Token: 0x060005E5 RID: 1509 RVA: 0x00016AB0 File Offset: 0x00014CB0
			internal override Expression VisitParameter(ParameterExpression p)
			{
				if (p != this.box.ParamExpressionInScope)
				{
					throw new NotSupportedException(Strings.ALinq_CanOnlyProjectTheLeaf);
				}
				this.box.StartNewPath();
				return p;
			}

			// Token: 0x04000328 RID: 808
			private readonly PathBox box;

			// Token: 0x04000329 RID: 809
			private readonly Type type;

			// Token: 0x0400032A RID: 810
			private bool leafExpressionIsMemberAccess;

			// Token: 0x0400032B RID: 811
			private readonly DataServiceContext context;
		}

		// Token: 0x020000B9 RID: 185
		private class NonEntityProjectionAnalyzer : DataServiceALinqExpressionVisitor
		{
			// Token: 0x060005EC RID: 1516 RVA: 0x00016C3D File Offset: 0x00014E3D
			private NonEntityProjectionAnalyzer(PathBox pb, Type type, DataServiceContext context)
			{
				this.box = pb;
				this.type = type;
				this.context = context;
			}

			// Token: 0x060005ED RID: 1517 RVA: 0x00016C5C File Offset: 0x00014E5C
			internal static void Analyze(Expression e, PathBox pb, DataServiceContext context)
			{
				ProjectionAnalyzer.NonEntityProjectionAnalyzer nonEntityProjectionAnalyzer = new ProjectionAnalyzer.NonEntityProjectionAnalyzer(pb, e.Type, context);
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

			// Token: 0x060005EE RID: 1518 RVA: 0x00016CE0 File Offset: 0x00014EE0
			internal override Expression VisitUnary(UnaryExpression u)
			{
				if (!ResourceBinder.PatternRules.MatchConvertToAssignable(u))
				{
					if (u.NodeType == ExpressionType.TypeAs && this.leafExpressionIsMemberAccess)
					{
						return base.VisitUnary(u);
					}
					if (ClientTypeUtil.TypeOrElementTypeIsEntity(u.Operand.Type))
					{
						throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjection(this.type, u.ToString()));
					}
				}
				return base.VisitUnary(u);
			}

			// Token: 0x060005EF RID: 1519 RVA: 0x00016D40 File Offset: 0x00014F40
			internal override Expression VisitBinary(BinaryExpression b)
			{
				if (ClientTypeUtil.TypeOrElementTypeIsEntity(b.Left.Type) || ClientTypeUtil.TypeOrElementTypeIsEntity(b.Right.Type) || ProjectionAnalyzer.IsCollectionProducingExpression(b.Left) || ProjectionAnalyzer.IsCollectionProducingExpression(b.Right))
				{
					throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjection(this.type, b.ToString()));
				}
				return base.VisitBinary(b);
			}

			// Token: 0x060005F0 RID: 1520 RVA: 0x00016DA9 File Offset: 0x00014FA9
			internal override Expression VisitTypeIs(TypeBinaryExpression b)
			{
				if (ClientTypeUtil.TypeOrElementTypeIsEntity(b.Expression.Type) || ProjectionAnalyzer.IsCollectionProducingExpression(b.Expression))
				{
					throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjection(this.type, b.ToString()));
				}
				return base.VisitTypeIs(b);
			}

			// Token: 0x060005F1 RID: 1521 RVA: 0x00016DE8 File Offset: 0x00014FE8
			internal override Expression VisitConditional(ConditionalExpression c)
			{
				ResourceBinder.PatternRules.MatchNullCheckResult matchNullCheckResult = ResourceBinder.PatternRules.MatchNullCheck(this.box.ParamExpressionInScope, c);
				if (matchNullCheckResult.Match)
				{
					this.Visit(matchNullCheckResult.AssignExpression);
					return c;
				}
				if (ClientTypeUtil.TypeOrElementTypeIsEntity(c.Test.Type) || ClientTypeUtil.TypeOrElementTypeIsEntity(c.IfTrue.Type) || ClientTypeUtil.TypeOrElementTypeIsEntity(c.IfFalse.Type) || ProjectionAnalyzer.IsCollectionProducingExpression(c.Test) || ProjectionAnalyzer.IsCollectionProducingExpression(c.IfTrue) || ProjectionAnalyzer.IsCollectionProducingExpression(c.IfFalse))
				{
					throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjection(this.type, c.ToString()));
				}
				return base.VisitConditional(c);
			}

			// Token: 0x060005F2 RID: 1522 RVA: 0x00016E9C File Offset: 0x0001509C
			internal override Expression VisitMemberAccess(MemberExpression m)
			{
				Type type = m.Expression.Type;
				this.leafExpressionIsMemberAccess = true;
				if (PrimitiveType.IsKnownNullableType(type))
				{
					this.leafExpressionIsMemberAccess = false;
					return base.VisitMemberAccess(m);
				}
				if (ProjectionAnalyzer.IsCollectionProducingExpression(m.Expression))
				{
					throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjection(this.type, m.ToString()));
				}
				PropertyInfo pi;
				Expression expression;
				if (ResourceBinder.PatternRules.MatchNonPrivateReadableProperty(m, out pi, out expression))
				{
					Expression result = base.VisitMemberAccess(m);
					if (ClientTypeUtil.TypeOrElementTypeIsEntity(type))
					{
						Type convertedSourceType;
						ResourceBinder.StripTo<Expression>(m.Expression, out convertedSourceType);
						this.box.AppendPropertyToPath(pi, convertedSourceType, this.context);
						this.leafExpressionIsMemberAccess = false;
					}
					return result;
				}
				throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjection(this.type, m.ToString()));
			}

			// Token: 0x060005F3 RID: 1523 RVA: 0x00016F74 File Offset: 0x00015174
			internal override Expression VisitMethodCall(MethodCallExpression m)
			{
				if ((m.Object != null && ProjectionAnalyzer.IsDisallowedExpressionForMethodCall(m.Object, this.context.Model)) || m.Arguments.Any((Expression a) => ProjectionAnalyzer.IsDisallowedExpressionForMethodCall(a, this.context.Model)))
				{
					throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjection(this.type, m.ToString()));
				}
				ProjectionAnalyzer.CheckChainedSequence(m, this.type);
				if (ProjectionAnalyzer.IsMethodCallAllowedEntitySequence(m))
				{
					return base.VisitMethodCall(m);
				}
				if (m.Object == null || !ClientTypeUtil.TypeOrElementTypeIsEntity(m.Object.Type))
				{
					if (!m.Arguments.Any((Expression a) => ClientTypeUtil.TypeOrElementTypeIsEntity(a.Type)))
					{
						return base.VisitMethodCall(m);
					}
				}
				throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjection(this.type, m.ToString()));
			}

			// Token: 0x060005F4 RID: 1524 RVA: 0x0001706C File Offset: 0x0001526C
			internal override Expression VisitInvocation(InvocationExpression iv)
			{
				if (!ClientTypeUtil.TypeOrElementTypeIsEntity(iv.Expression.Type) && !ProjectionAnalyzer.IsCollectionProducingExpression(iv.Expression))
				{
					if (!iv.Arguments.Any((Expression a) => ClientTypeUtil.TypeOrElementTypeIsEntity(a.Type) || ProjectionAnalyzer.IsCollectionProducingExpression(a)))
					{
						return base.VisitInvocation(iv);
					}
				}
				throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjection(this.type, iv.ToString()));
			}

			// Token: 0x060005F5 RID: 1525 RVA: 0x000170E0 File Offset: 0x000152E0
			internal override Expression VisitLambda(LambdaExpression lambda)
			{
				ProjectionAnalyzer.Analyze(lambda, this.box, this.context);
				return lambda;
			}

			// Token: 0x060005F6 RID: 1526 RVA: 0x000170F5 File Offset: 0x000152F5
			internal override Expression VisitMemberInit(MemberInitExpression init)
			{
				ProjectionAnalyzer.Analyze(init, this.box, this.context);
				return init;
			}

			// Token: 0x060005F7 RID: 1527 RVA: 0x0001710A File Offset: 0x0001530A
			internal override NewExpression VisitNew(NewExpression nex)
			{
				if (ClientTypeUtil.TypeOrElementTypeIsEntity(nex.Type) && !ResourceBinder.PatternRules.MatchNewDataServiceCollectionOfT(nex))
				{
					throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjection(this.type, nex.ToString()));
				}
				return base.VisitNew(nex);
			}

			// Token: 0x060005F8 RID: 1528 RVA: 0x0001713F File Offset: 0x0001533F
			internal override Expression VisitParameter(ParameterExpression p)
			{
				if (ClientTypeUtil.TypeOrElementTypeIsEntity(p.Type))
				{
					if (p != this.box.ParamExpressionInScope)
					{
						throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjection(this.type, p.ToString()));
					}
					this.box.StartNewPath();
				}
				return p;
			}

			// Token: 0x060005F9 RID: 1529 RVA: 0x0001717F File Offset: 0x0001537F
			internal override Expression VisitConstant(ConstantExpression c)
			{
				if (ClientTypeUtil.TypeOrElementTypeIsEntity(c.Type))
				{
					throw new NotSupportedException(Strings.ALinq_ExpressionNotSupportedInProjection(this.type, c.ToString()));
				}
				return base.VisitConstant(c);
			}

			// Token: 0x0400032C RID: 812
			private PathBox box;

			// Token: 0x0400032D RID: 813
			private Type type;

			// Token: 0x0400032E RID: 814
			private bool leafExpressionIsMemberAccess;

			// Token: 0x0400032F RID: 815
			private readonly DataServiceContext context;
		}
	}
}
