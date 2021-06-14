using System;
using System.Collections.Generic;
using System.Data.Services.Client.Materialization;
using System.Data.Services.Client.Metadata;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Services.Client
{
	// Token: 0x02000082 RID: 130
	internal class ProjectionPlanCompiler : ALinqExpressionVisitor
	{
		// Token: 0x06000459 RID: 1113 RVA: 0x00011D24 File Offset: 0x0000FF24
		private ProjectionPlanCompiler(Dictionary<Expression, Expression> normalizerRewrites)
		{
			this.annotations = new Dictionary<Expression, ProjectionPlanCompiler.ExpressionAnnotation>(ReferenceEqualityComparer<Expression>.Instance);
			this.materializerExpression = Expression.Parameter(typeof(object), "mat");
			this.normalizerRewrites = normalizerRewrites;
			this.pathBuilder = new ProjectionPathBuilder();
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00011D74 File Offset: 0x0000FF74
		internal static ProjectionPlan CompilePlan(LambdaExpression projection, Dictionary<Expression, Expression> normalizerRewrites)
		{
			ProjectionPlanCompiler projectionPlanCompiler = new ProjectionPlanCompiler(normalizerRewrites);
			Expression expression = projectionPlanCompiler.Visit(projection);
			return new ProjectionPlan
			{
				Plan = (Func<object, object, Type, object>)((LambdaExpression)expression).Compile(),
				ProjectedType = projection.Body.Type
			};
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00011DC0 File Offset: 0x0000FFC0
		internal override Expression VisitBinary(BinaryExpression b)
		{
			Expression expressionBeforeNormalization = this.GetExpressionBeforeNormalization(b);
			if (expressionBeforeNormalization == b)
			{
				return base.VisitBinary(b);
			}
			return this.Visit(expressionBeforeNormalization);
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00011DE8 File Offset: 0x0000FFE8
		internal override Expression VisitConditional(ConditionalExpression conditional)
		{
			Expression expressionBeforeNormalization = this.GetExpressionBeforeNormalization(conditional);
			if (expressionBeforeNormalization != conditional)
			{
				return this.Visit(expressionBeforeNormalization);
			}
			ResourceBinder.PatternRules.MatchNullCheckResult nullCheck = ResourceBinder.PatternRules.MatchNullCheck(this.pathBuilder.LambdaParameterInScope, conditional);
			if (!nullCheck.Match || !ClientTypeUtil.TypeOrElementTypeIsEntity(ResourceBinder.StripConvertToAssignable(nullCheck.TestToNullExpression).Type))
			{
				Expression expression = null;
				if (nullCheck.Match)
				{
					Expression expression2 = this.Visit(nullCheck.TestToNullExpression);
					if (expression2.NodeType == ExpressionType.Convert)
					{
						expression2 = ((UnaryExpression)expression2).Operand;
					}
					expression = Expression.MakeBinary(ExpressionType.Equal, expression2, Expression.Constant(null));
				}
				if (expression == null)
				{
					expression = this.Visit(conditional.Test);
				}
				Expression expression3 = this.Visit(conditional.IfTrue);
				Expression expression4 = this.Visit(conditional.IfFalse);
				if (expression != conditional.Test || expression3 != conditional.IfTrue || expression4 != conditional.IfFalse)
				{
					return Expression.Condition(expression, expression3, expression4, expression3.Type.IsAssignableFrom(expression4.Type) ? expression3.Type : expression4.Type);
				}
			}
			return this.RebindConditionalNullCheck(conditional, nullCheck);
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00011F00 File Offset: 0x00010100
		internal override Expression VisitUnary(UnaryExpression u)
		{
			Expression expressionBeforeNormalization = this.GetExpressionBeforeNormalization(u);
			Expression expression;
			if (expressionBeforeNormalization == u)
			{
				expression = base.VisitUnary(u);
				UnaryExpression unaryExpression = expression as UnaryExpression;
				ProjectionPlanCompiler.ExpressionAnnotation value;
				if (unaryExpression != null && this.annotations.TryGetValue(unaryExpression.Operand, out value))
				{
					this.annotations[expression] = value;
				}
			}
			else
			{
				expression = this.Visit(expressionBeforeNormalization);
			}
			return expression;
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00011F58 File Offset: 0x00010158
		internal override Expression VisitMemberAccess(MemberExpression m)
		{
			Expression expression = m.Expression;
			Expression result;
			if (PrimitiveType.IsKnownNullableType(expression.Type))
			{
				result = base.VisitMemberAccess(m);
			}
			else
			{
				Expression expression2 = this.Visit(expression);
				ProjectionPlanCompiler.ExpressionAnnotation baseAnnotation;
				if (this.annotations.TryGetValue(expression2, out baseAnnotation))
				{
					result = this.RebindMemberAccess(m, baseAnnotation);
				}
				else
				{
					result = Expression.MakeMemberAccess(expression2, m.Member);
				}
			}
			return result;
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00011FB4 File Offset: 0x000101B4
		internal override Expression VisitParameter(ParameterExpression p)
		{
			ProjectionPlanCompiler.ExpressionAnnotation annotation;
			Expression result;
			if (this.annotations.TryGetValue(p, out annotation))
			{
				result = this.RebindParameter(p, annotation);
			}
			else
			{
				result = base.VisitParameter(p);
			}
			return result;
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00011FE8 File Offset: 0x000101E8
		internal override Expression VisitMemberInit(MemberInitExpression init)
		{
			this.pathBuilder.EnterMemberInit(init);
			Expression result;
			if (this.pathBuilder.CurrentIsEntity && init.Bindings.Count > 0)
			{
				result = this.RebindEntityMemberInit(init);
			}
			else
			{
				result = base.VisitMemberInit(init);
			}
			this.pathBuilder.LeaveMemberInit();
			return result;
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0001203C File Offset: 0x0001023C
		internal override Expression VisitMethodCall(MethodCallExpression m)
		{
			Expression expressionBeforeNormalization = this.GetExpressionBeforeNormalization(m);
			if (expressionBeforeNormalization != m)
			{
				return this.Visit(expressionBeforeNormalization);
			}
			Expression result;
			if (this.pathBuilder.CurrentIsEntity)
			{
				if (m.Method.Name == "Select")
				{
					result = this.RebindMethodCallForMemberSelect(m);
				}
				else if (m.Method.Name == "ToList")
				{
					result = this.RebindMethodCallForMemberToList(m);
				}
				else
				{
					result = base.VisitMethodCall(m);
				}
			}
			else if (ProjectionAnalyzer.IsMethodCallAllowedEntitySequence(m))
			{
				result = this.RebindMethodCallForNewSequence(m);
			}
			else
			{
				result = base.VisitMethodCall(m);
			}
			return result;
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x000120D4 File Offset: 0x000102D4
		internal override Expression Visit(Expression exp)
		{
			if (exp == null)
			{
				return exp;
			}
			if (exp.NodeType != ExpressionType.New)
			{
				return base.Visit(exp);
			}
			NewExpression nex = (NewExpression)exp;
			if (ResourceBinder.PatternRules.MatchNewDataServiceCollectionOfT(nex))
			{
				return this.RebindNewExpressionForDataServiceCollectionOfT(nex);
			}
			return this.VisitNew(nex);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00012118 File Offset: 0x00010318
		internal override Expression VisitLambda(LambdaExpression lambda)
		{
			Expression result;
			if (!this.topLevelProjectionFound || (lambda.Parameters.Count == 1 && ClientTypeUtil.TypeOrElementTypeIsEntity(lambda.Parameters[0].Type)))
			{
				this.topLevelProjectionFound = true;
				ParameterExpression parameterExpression = Expression.Parameter(typeof(Type), "type" + this.identifierId);
				ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object), "entry" + this.identifierId);
				this.identifierId++;
				this.pathBuilder.EnterLambdaScope(lambda, parameterExpression2, parameterExpression);
				ProjectionPath projectionPath = new ProjectionPath(lambda.Parameters[0], parameterExpression, parameterExpression2);
				ProjectionPathSegment projectionPathSegment = new ProjectionPathSegment(projectionPath, null, null);
				projectionPath.Add(projectionPathSegment);
				this.annotations[lambda.Parameters[0]] = new ProjectionPlanCompiler.ExpressionAnnotation
				{
					Segment = projectionPathSegment
				};
				Expression expression = this.Visit(lambda.Body);
				if (expression.Type.IsValueType())
				{
					expression = Expression.Convert(expression, typeof(object));
				}
				result = Expression.Lambda<Func<object, object, Type, object>>(expression, new ParameterExpression[]
				{
					this.materializerExpression,
					parameterExpression2,
					parameterExpression
				});
				this.pathBuilder.LeaveLambdaScope();
			}
			else
			{
				result = base.VisitLambda(lambda);
			}
			return result;
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00012280 File Offset: 0x00010480
		private static Expression CallMaterializer(string methodName, params Expression[] arguments)
		{
			return ProjectionPlanCompiler.CallMaterializerWithType(methodName, null, arguments);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0001228C File Offset: 0x0001048C
		private static Expression CallMaterializerWithType(string methodName, Type[] typeArguments, params Expression[] arguments)
		{
			MethodInfo methodInfo = typeof(ODataEntityMaterializerInvoker).GetMethod(methodName, false, true);
			if (typeArguments != null)
			{
				methodInfo = methodInfo.MakeGenericMethod(typeArguments);
			}
			return ProjectionPlanCompiler.dynamicProxyMethodGenerator.GetCallWrapper(methodInfo, arguments);
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x000122C3 File Offset: 0x000104C3
		private static Expression RebindConstructor(ConstructorInfo info, params Expression[] arguments)
		{
			return ProjectionPlanCompiler.dynamicProxyMethodGenerator.GetCallWrapper(info, arguments);
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x000122D4 File Offset: 0x000104D4
		private Expression CallCheckValueForPathIsNull(Expression entry, Expression entryType, ProjectionPath path)
		{
			Expression expression = ProjectionPlanCompiler.CallMaterializer("ProjectionCheckValueForPathIsNull", new Expression[]
			{
				entry,
				entryType,
				Expression.Constant(path, typeof(object))
			});
			this.annotations.Add(expression, new ProjectionPlanCompiler.ExpressionAnnotation
			{
				Segment = path[path.Count - 1]
			});
			return expression;
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00012338 File Offset: 0x00010538
		private Expression CallValueForPath(Expression entry, Expression entryType, ProjectionPath path)
		{
			Expression expression = ProjectionPlanCompiler.CallMaterializer("ProjectionValueForPath", new Expression[]
			{
				this.materializerExpression,
				entry,
				entryType,
				Expression.Constant(path, typeof(object))
			});
			this.annotations.Add(expression, new ProjectionPlanCompiler.ExpressionAnnotation
			{
				Segment = path[path.Count - 1]
			});
			return expression;
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x000123A4 File Offset: 0x000105A4
		private Expression CallValueForPathWithType(Expression entry, Expression entryType, ProjectionPath path, Type type)
		{
			Expression expression = this.CallValueForPath(entry, entryType, path);
			Expression expression2 = Expression.Convert(expression, type);
			this.annotations.Add(expression2, new ProjectionPlanCompiler.ExpressionAnnotation
			{
				Segment = path[path.Count - 1]
			});
			return expression2;
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x000123EC File Offset: 0x000105EC
		private Expression RebindConditionalNullCheck(ConditionalExpression conditional, ResourceBinder.PatternRules.MatchNullCheckResult nullCheck)
		{
			Expression key = this.Visit(nullCheck.TestToNullExpression);
			Expression expression = this.Visit(nullCheck.AssignExpression);
			ProjectionPlanCompiler.ExpressionAnnotation expressionAnnotation;
			if (!this.annotations.TryGetValue(key, out expressionAnnotation))
			{
				return base.VisitConditional(conditional);
			}
			ProjectionPathSegment segment = expressionAnnotation.Segment;
			Expression expression2 = this.CallCheckValueForPathIsNull(segment.StartPath.RootEntry, segment.StartPath.ExpectedRootType, segment.StartPath);
			Expression test = expression2;
			Expression ifTrue = Expression.Constant(null, expression.Type);
			Expression ifFalse = expression;
			return Expression.Condition(test, ifTrue, ifFalse);
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x000124A4 File Offset: 0x000106A4
		private Expression RebindEntityMemberInit(MemberInitExpression init)
		{
			Expression[] array;
			if (!this.pathBuilder.HasRewrites)
			{
				MemberAssignmentAnalysis memberAssignmentAnalysis = MemberAssignmentAnalysis.Analyze(this.pathBuilder.LambdaParameterInScope, ((MemberAssignment)init.Bindings[0]).Expression);
				array = memberAssignmentAnalysis.GetExpressionsToTargetEntity();
			}
			else
			{
				array = MemberAssignmentAnalysis.EmptyExpressionArray;
			}
			Expression parameterEntryInScope = this.pathBuilder.ParameterEntryInScope;
			List<string> list = new List<string>();
			List<Func<object, object, Type, object>> list2 = new List<Func<object, object, Type, object>>();
			Type type = init.NewExpression.Type;
			Expression expression = Expression.Constant(type, typeof(Type));
			string[] names = (from e in array.Skip(1)
			select ((MemberExpression)e).Member.Name).ToArray<string>();
			Expression expression2;
			Expression expression3;
			ParameterExpression parameterExpression;
			ParameterExpression parameterExpression2;
			if (array.Length <= 1)
			{
				expression2 = this.pathBuilder.ParameterEntryInScope;
				expression3 = this.pathBuilder.ExpectedParamTypeInScope;
				parameterExpression = (ParameterExpression)this.pathBuilder.ParameterEntryInScope;
				parameterExpression2 = (ParameterExpression)this.pathBuilder.ExpectedParamTypeInScope;
			}
			else
			{
				expression2 = this.GetDeepestEntry(array);
				expression3 = expression;
				parameterExpression = Expression.Parameter(typeof(object), "subentry" + this.identifierId++);
				parameterExpression2 = (ParameterExpression)this.pathBuilder.ExpectedParamTypeInScope;
				ProjectionPath projectionPath = new ProjectionPath((ParameterExpression)this.pathBuilder.LambdaParameterInScope, this.pathBuilder.ExpectedParamTypeInScope, this.pathBuilder.ParameterEntryInScope, array.Skip(1));
				this.annotations.Add(expression2, new ProjectionPlanCompiler.ExpressionAnnotation
				{
					Segment = projectionPath[projectionPath.Count - 1]
				});
				this.annotations.Add(parameterExpression, new ProjectionPlanCompiler.ExpressionAnnotation
				{
					Segment = projectionPath[projectionPath.Count - 1]
				});
				this.pathBuilder.RegisterRewrite(this.pathBuilder.LambdaParameterInScope, names, parameterExpression);
			}
			for (int i = 0; i < init.Bindings.Count; i++)
			{
				MemberAssignment memberAssignment = (MemberAssignment)init.Bindings[i];
				list.Add(memberAssignment.Member.Name);
				LambdaExpression lambdaExpression;
				if (ClientTypeUtil.TypeOrElementTypeIsEntity(ClientTypeUtil.GetMemberType(memberAssignment.Member)) && memberAssignment.Expression.NodeType == ExpressionType.MemberInit)
				{
					Expression expression4 = ProjectionPlanCompiler.CallMaterializer("ProjectionGetEntry", new Expression[]
					{
						parameterEntryInScope,
						Expression.Constant(memberAssignment.Member.Name, typeof(string))
					});
					ParameterExpression parameterExpression3 = Expression.Parameter(typeof(object), "subentry" + this.identifierId++);
					ProjectionPlanCompiler.ExpressionAnnotation expressionAnnotation;
					ProjectionPath projectionPath2;
					if (this.annotations.TryGetValue(this.pathBuilder.ParameterEntryInScope, out expressionAnnotation))
					{
						projectionPath2 = new ProjectionPath((ParameterExpression)this.pathBuilder.LambdaParameterInScope, this.pathBuilder.ExpectedParamTypeInScope, parameterEntryInScope);
						projectionPath2.AddRange(expressionAnnotation.Segment.StartPath);
					}
					else
					{
						projectionPath2 = new ProjectionPath((ParameterExpression)this.pathBuilder.LambdaParameterInScope, this.pathBuilder.ExpectedParamTypeInScope, parameterEntryInScope, array.Skip(1));
					}
					Type reflectedType = memberAssignment.Member.ReflectedType;
					ProjectionPathSegment projectionPathSegment = new ProjectionPathSegment(projectionPath2, memberAssignment.Member.Name, reflectedType);
					projectionPath2.Add(projectionPathSegment);
					string[] names2 = (from m in projectionPath2
					where m.Member != null
					select m.Member).ToArray<string>();
					this.annotations.Add(parameterExpression3, new ProjectionPlanCompiler.ExpressionAnnotation
					{
						Segment = projectionPathSegment
					});
					this.pathBuilder.RegisterRewrite(this.pathBuilder.LambdaParameterInScope, names2, parameterExpression3);
					Expression expression5 = this.Visit(memberAssignment.Expression);
					this.pathBuilder.RevokeRewrite(this.pathBuilder.LambdaParameterInScope, names2);
					this.annotations.Remove(parameterExpression3);
					expression5 = Expression.Convert(expression5, typeof(object));
					ParameterExpression[] parameters = new ParameterExpression[]
					{
						this.materializerExpression,
						parameterExpression3,
						parameterExpression2
					};
					lambdaExpression = Expression.Lambda(expression5, parameters);
					Expression[] arguments = new Expression[]
					{
						this.materializerExpression,
						expression4,
						parameterExpression2
					};
					ParameterExpression[] parameters2 = new ParameterExpression[]
					{
						this.materializerExpression,
						(ParameterExpression)parameterEntryInScope,
						parameterExpression2
					};
					lambdaExpression = Expression.Lambda(Expression.Invoke(lambdaExpression, arguments), parameters2);
				}
				else
				{
					Expression expression6 = this.Visit(memberAssignment.Expression);
					expression6 = Expression.Convert(expression6, typeof(object));
					ParameterExpression[] parameters3 = new ParameterExpression[]
					{
						this.materializerExpression,
						parameterExpression,
						parameterExpression2
					};
					lambdaExpression = Expression.Lambda(expression6, parameters3);
				}
				list2.Add((Func<object, object, Type, object>)lambdaExpression.Compile());
			}
			for (int j = 1; j < array.Length; j++)
			{
				this.pathBuilder.RevokeRewrite(this.pathBuilder.LambdaParameterInScope, names);
				this.annotations.Remove(expression2);
				this.annotations.Remove(parameterExpression);
			}
			Expression expression7 = ProjectionPlanCompiler.CallMaterializer("ProjectionInitializeEntity", new Expression[]
			{
				this.materializerExpression,
				expression2,
				expression3,
				expression,
				Expression.Constant(list.ToArray()),
				Expression.Constant(list2.ToArray())
			});
			return Expression.Convert(expression7, type);
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00012A80 File Offset: 0x00010C80
		private Expression GetDeepestEntry(Expression[] path)
		{
			Expression expression = null;
			int num = 1;
			do
			{
				expression = ProjectionPlanCompiler.CallMaterializer("ProjectionGetEntry", new Expression[]
				{
					expression ?? this.pathBuilder.ParameterEntryInScope,
					Expression.Constant(((MemberExpression)path[num]).Member.Name, typeof(string))
				});
				num++;
			}
			while (num < path.Length);
			return expression;
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00012AE8 File Offset: 0x00010CE8
		private Expression GetExpressionBeforeNormalization(Expression expression)
		{
			Expression expression2;
			if (this.normalizerRewrites != null && this.normalizerRewrites.TryGetValue(expression, out expression2))
			{
				expression = expression2;
			}
			return expression;
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00012B14 File Offset: 0x00010D14
		private Expression RebindParameter(Expression expression, ProjectionPlanCompiler.ExpressionAnnotation annotation)
		{
			Expression result = this.CallValueForPathWithType(annotation.Segment.StartPath.RootEntry, annotation.Segment.StartPath.ExpectedRootType, annotation.Segment.StartPath, expression.Type);
			ProjectionPath projectionPath = new ProjectionPath(annotation.Segment.StartPath.Root, annotation.Segment.StartPath.ExpectedRootType, annotation.Segment.StartPath.RootEntry);
			ProjectionPathSegment projectionPathSegment = new ProjectionPathSegment(projectionPath, null, null);
			projectionPath.Add(projectionPathSegment);
			this.annotations[expression] = new ProjectionPlanCompiler.ExpressionAnnotation
			{
				Segment = projectionPathSegment
			};
			return result;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00012BBC File Offset: 0x00010DBC
		private Expression RebindMemberAccess(MemberExpression m, ProjectionPlanCompiler.ExpressionAnnotation baseAnnotation)
		{
			Expression expression = m.Expression;
			Expression expression2 = this.pathBuilder.GetRewrite(expression);
			if (expression2 != null)
			{
				Expression expression3 = Expression.Constant(expression.Type, typeof(Type));
				ProjectionPath projectionPath = new ProjectionPath(expression2 as ParameterExpression, expression3, expression2);
				ProjectionPathSegment item = new ProjectionPathSegment(projectionPath, m);
				projectionPath.Add(item);
				expression2 = this.CallValueForPathWithType(expression2, expression3, projectionPath, m.Type);
			}
			else
			{
				ProjectionPathSegment item2 = new ProjectionPathSegment(baseAnnotation.Segment.StartPath, m);
				baseAnnotation.Segment.StartPath.Add(item2);
				expression2 = this.CallValueForPathWithType(baseAnnotation.Segment.StartPath.RootEntry, baseAnnotation.Segment.StartPath.ExpectedRootType, baseAnnotation.Segment.StartPath, m.Type);
			}
			return expression2;
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00012CB4 File Offset: 0x00010EB4
		private Expression RebindNewExpressionForDataServiceCollectionOfT(NewExpression nex)
		{
			NewExpression newExpression = this.VisitNew(nex);
			Expression result = null;
			ProjectionPlanCompiler.ExpressionAnnotation expressionAnnotation = null;
			if (newExpression != null)
			{
				ConstructorInfo info = nex.Type.GetInstanceConstructors(false).First((ConstructorInfo c) => c.GetParameters().Length == 7 && c.GetParameters()[0].ParameterType == typeof(object));
				Type type = typeof(IEnumerable<>).MakeGenericType(new Type[]
				{
					nex.Type.GetGenericArguments()[0]
				});
				if (newExpression.Arguments.Count == 1 && newExpression.Constructor == nex.Type.GetInstanceConstructor(true, new Type[]
				{
					type
				}) && this.annotations.TryGetValue(newExpression.Arguments[0], out expressionAnnotation))
				{
					result = ProjectionPlanCompiler.RebindConstructor(info, new Expression[]
					{
						this.materializerExpression,
						Expression.Constant(null, typeof(DataServiceContext)),
						newExpression.Arguments[0],
						Expression.Constant(TrackingMode.AutoChangeTracking, typeof(TrackingMode)),
						Expression.Constant(null, typeof(string)),
						Expression.Constant(null, typeof(Func<EntityChangedParams, bool>)),
						Expression.Constant(null, typeof(Func<EntityCollectionChangedParams, bool>))
					});
				}
				else if (newExpression.Arguments.Count == 2 && this.annotations.TryGetValue(newExpression.Arguments[0], out expressionAnnotation))
				{
					result = ProjectionPlanCompiler.RebindConstructor(info, new Expression[]
					{
						this.materializerExpression,
						Expression.Constant(null, typeof(DataServiceContext)),
						newExpression.Arguments[0],
						newExpression.Arguments[1],
						Expression.Constant(null, typeof(string)),
						Expression.Constant(null, typeof(Func<EntityChangedParams, bool>)),
						Expression.Constant(null, typeof(Func<EntityCollectionChangedParams, bool>))
					});
				}
				else if (newExpression.Arguments.Count == 5 && this.annotations.TryGetValue(newExpression.Arguments[0], out expressionAnnotation))
				{
					result = ProjectionPlanCompiler.RebindConstructor(info, new Expression[]
					{
						this.materializerExpression,
						Expression.Constant(null, typeof(DataServiceContext)),
						newExpression.Arguments[0],
						newExpression.Arguments[1],
						newExpression.Arguments[2],
						newExpression.Arguments[3],
						newExpression.Arguments[4]
					});
				}
				else if (newExpression.Arguments.Count == 6 && typeof(DataServiceContext).IsAssignableFrom(newExpression.Arguments[0].Type) && this.annotations.TryGetValue(newExpression.Arguments[1], out expressionAnnotation))
				{
					result = ProjectionPlanCompiler.RebindConstructor(info, new Expression[]
					{
						this.materializerExpression,
						newExpression.Arguments[0],
						newExpression.Arguments[1],
						newExpression.Arguments[2],
						newExpression.Arguments[3],
						newExpression.Arguments[4],
						newExpression.Arguments[5]
					});
				}
			}
			if (expressionAnnotation != null)
			{
				this.annotations.Add(newExpression, expressionAnnotation);
			}
			return result;
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00013074 File Offset: 0x00011274
		private Expression RebindMethodCallForMemberSelect(MethodCallExpression call)
		{
			Expression expression = null;
			Expression expression2 = this.Visit(call.Arguments[0]);
			ProjectionPlanCompiler.ExpressionAnnotation expressionAnnotation;
			this.annotations.TryGetValue(expression2, out expressionAnnotation);
			if (expressionAnnotation != null)
			{
				LambdaExpression lambdaExpression = call.Arguments[1] as LambdaExpression;
				ParameterExpression parameterExpression = lambdaExpression.Parameters.Last<ParameterExpression>();
				Expression expression3 = this.Visit(call.Arguments[1]);
				if (ClientTypeUtil.TypeOrElementTypeIsEntity(parameterExpression.Type))
				{
					Type type = call.Method.ReturnType.GetGenericArguments()[0];
					expression = ProjectionPlanCompiler.CallMaterializer("ProjectionSelect", new Expression[]
					{
						this.materializerExpression,
						this.pathBuilder.ParameterEntryInScope,
						this.pathBuilder.ExpectedParamTypeInScope,
						Expression.Constant(type, typeof(Type)),
						Expression.Constant(expressionAnnotation.Segment.StartPath, typeof(object)),
						expression3
					});
					this.annotations.Add(expression, expressionAnnotation);
					expression = ProjectionPlanCompiler.CallMaterializerWithType("EnumerateAsElementType", new Type[]
					{
						type
					}, new Expression[]
					{
						expression
					});
					this.annotations.Add(expression, expressionAnnotation);
				}
				else
				{
					expression = Expression.Call(call.Method, expression2, expression3);
					this.annotations.Add(expression, expressionAnnotation);
				}
			}
			if (expression == null)
			{
				expression = base.VisitMethodCall(call);
			}
			return expression;
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x000131E8 File Offset: 0x000113E8
		private Expression RebindMethodCallForMemberToList(MethodCallExpression call)
		{
			Expression expression = this.Visit(call.Arguments[0]);
			ProjectionPlanCompiler.ExpressionAnnotation value;
			if (this.annotations.TryGetValue(expression, out value))
			{
				expression = this.TypedEnumerableToList(expression, call.Type);
				this.annotations.Add(expression, value);
			}
			return expression;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00013234 File Offset: 0x00011434
		private Expression RebindMethodCallForNewSequence(MethodCallExpression call)
		{
			Expression expression = null;
			if (call.Method.Name == "Select")
			{
				Expression expression2 = this.Visit(call.Arguments[0]);
				ProjectionPlanCompiler.ExpressionAnnotation expressionAnnotation;
				this.annotations.TryGetValue(expression2, out expressionAnnotation);
				if (expressionAnnotation != null)
				{
					LambdaExpression lambdaExpression = call.Arguments[1] as LambdaExpression;
					ParameterExpression parameterExpression = lambdaExpression.Parameters.Last<ParameterExpression>();
					Expression expression3 = this.Visit(call.Arguments[1]);
					if (ClientTypeUtil.TypeOrElementTypeIsEntity(parameterExpression.Type))
					{
						Type type = call.Method.ReturnType.GetGenericArguments()[0];
						expression = ProjectionPlanCompiler.CallMaterializer("ProjectionSelect", new Expression[]
						{
							this.materializerExpression,
							this.pathBuilder.ParameterEntryInScope,
							this.pathBuilder.ExpectedParamTypeInScope,
							Expression.Constant(type, typeof(Type)),
							Expression.Constant(expressionAnnotation.Segment.StartPath, typeof(object)),
							expression3
						});
						this.annotations.Add(expression, expressionAnnotation);
						expression = ProjectionPlanCompiler.CallMaterializerWithType("EnumerateAsElementType", new Type[]
						{
							type
						}, new Expression[]
						{
							expression
						});
						this.annotations.Add(expression, expressionAnnotation);
					}
					else
					{
						expression = Expression.Call(call.Method, expression2, expression3);
						this.annotations.Add(expression, expressionAnnotation);
					}
				}
			}
			else
			{
				Expression expression4 = this.Visit(call.Arguments[0]);
				ProjectionPlanCompiler.ExpressionAnnotation value;
				if (this.annotations.TryGetValue(expression4, out value))
				{
					expression = this.TypedEnumerableToList(expression4, call.Type);
					this.annotations.Add(expression, value);
				}
			}
			if (expression == null)
			{
				expression = base.VisitMethodCall(call);
			}
			return expression;
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00013408 File Offset: 0x00011608
		private Expression TypedEnumerableToList(Expression source, Type targetType)
		{
			Type type = source.Type.GetGenericArguments()[0];
			Type type2 = targetType.GetGenericArguments()[0];
			return ProjectionPlanCompiler.CallMaterializerWithType("ListAsElementType", new Type[]
			{
				type,
				type2
			}, new Expression[]
			{
				this.materializerExpression,
				source
			});
		}

		// Token: 0x040002DA RID: 730
		private static readonly DynamicProxyMethodGenerator dynamicProxyMethodGenerator = new DynamicProxyMethodGenerator();

		// Token: 0x040002DB RID: 731
		private readonly Dictionary<Expression, ProjectionPlanCompiler.ExpressionAnnotation> annotations;

		// Token: 0x040002DC RID: 732
		private readonly ParameterExpression materializerExpression;

		// Token: 0x040002DD RID: 733
		private readonly Dictionary<Expression, Expression> normalizerRewrites;

		// Token: 0x040002DE RID: 734
		private int identifierId;

		// Token: 0x040002DF RID: 735
		private ProjectionPathBuilder pathBuilder;

		// Token: 0x040002E0 RID: 736
		private bool topLevelProjectionFound;

		// Token: 0x02000083 RID: 131
		internal class ExpressionAnnotation
		{
			// Token: 0x1700011B RID: 283
			// (get) Token: 0x0600047A RID: 1146 RVA: 0x0001346D File Offset: 0x0001166D
			// (set) Token: 0x0600047B RID: 1147 RVA: 0x00013475 File Offset: 0x00011675
			internal ProjectionPathSegment Segment { get; set; }
		}
	}
}
