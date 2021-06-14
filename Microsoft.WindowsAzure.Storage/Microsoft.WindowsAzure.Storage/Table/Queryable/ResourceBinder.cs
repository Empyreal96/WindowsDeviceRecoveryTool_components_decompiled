using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x02000129 RID: 297
	internal class ResourceBinder : DataServiceALinqExpressionVisitor
	{
		// Token: 0x060013BF RID: 5055 RVA: 0x0004B030 File Offset: 0x00049230
		internal static Expression Bind(Expression e)
		{
			ResourceBinder resourceBinder = new ResourceBinder();
			Expression expression = resourceBinder.Visit(e);
			ResourceBinder.VerifyKeyPredicates(expression);
			ResourceBinder.VerifyNotSelectManyProjection(expression);
			return expression;
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x0004B058 File Offset: 0x00049258
		internal static bool IsMissingKeyPredicates(Expression expression)
		{
			ResourceExpression resourceExpression = expression as ResourceExpression;
			if (resourceExpression != null)
			{
				if (ResourceBinder.IsMissingKeyPredicates(resourceExpression.Source))
				{
					return true;
				}
				if (resourceExpression.Source != null)
				{
					ResourceSetExpression resourceSetExpression = resourceExpression.Source as ResourceSetExpression;
					if (resourceSetExpression != null && !resourceSetExpression.HasKeyPredicate)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x0004B0A0 File Offset: 0x000492A0
		internal static void VerifyKeyPredicates(Expression e)
		{
			if (ResourceBinder.IsMissingKeyPredicates(e))
			{
				throw new NotSupportedException("Navigation properties can only be selected from a single resource. Specify a key predicate to restrict the entity set to a single instance.");
			}
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x0004B0B8 File Offset: 0x000492B8
		internal static void VerifyNotSelectManyProjection(Expression expression)
		{
			ResourceSetExpression resourceSetExpression = expression as ResourceSetExpression;
			if (resourceSetExpression != null)
			{
				ProjectionQueryOptionExpression projection = resourceSetExpression.Projection;
				if (projection != null)
				{
					MethodCallExpression methodCallExpression = ResourceBinder.StripTo<MethodCallExpression>(projection.Selector.Body);
					if (methodCallExpression != null && methodCallExpression.Method.Name == "SelectMany")
					{
						throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "The expression type {0} is not supported.", new object[]
						{
							methodCallExpression
						}));
					}
				}
				else if (resourceSetExpression.HasTransparentScope)
				{
					throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "The expression type {0} is not supported.", new object[]
					{
						resourceSetExpression
					}));
				}
			}
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x0004B154 File Offset: 0x00049354
		private static Expression AnalyzePredicate(MethodCallExpression mce)
		{
			ResourceSetExpression resourceSetExpression;
			LambdaExpression lambdaExpression;
			if (!ResourceBinder.TryGetResourceSetMethodArguments(mce, out resourceSetExpression, out lambdaExpression))
			{
				ResourceBinder.ValidationRules.RequireNonSingleton(mce.Arguments[0]);
				return mce;
			}
			List<Expression> list = new List<Expression>();
			ResourceBinder.AddConjuncts(lambdaExpression.Body, list);
			Dictionary<ResourceSetExpression, List<Expression>> dictionary = new Dictionary<ResourceSetExpression, List<Expression>>(ReferenceEqualityComparer<ResourceSetExpression>.Instance);
			List<ResourceExpression> list2 = new List<ResourceExpression>();
			foreach (Expression e in list)
			{
				Expression item = InputBinder.Bind(e, resourceSetExpression, lambdaExpression.Parameters[0], list2);
				if (list2.Count > 1)
				{
					return mce;
				}
				ResourceSetExpression resourceSetExpression2 = (list2.Count == 0) ? resourceSetExpression : (list2[0] as ResourceSetExpression);
				if (resourceSetExpression2 == null)
				{
					return mce;
				}
				List<Expression> list3 = null;
				if (!dictionary.TryGetValue(resourceSetExpression2, out list3))
				{
					list3 = new List<Expression>();
					dictionary[resourceSetExpression2] = list3;
				}
				list3.Add(item);
				list2.Clear();
			}
			List<Expression> list4;
			if (dictionary.TryGetValue(resourceSetExpression, out list4))
			{
				dictionary.Remove(resourceSetExpression);
			}
			else
			{
				list4 = null;
			}
			if (list4 != null && list4.Count > 0)
			{
				if (resourceSetExpression.KeyPredicate != null)
				{
					Expression item2 = ResourceBinder.BuildKeyPredicateFilter(resourceSetExpression.CreateReference(), resourceSetExpression.KeyPredicate);
					list4.Add(item2);
					resourceSetExpression.KeyPredicate = null;
				}
				int num;
				Expression expression;
				if (resourceSetExpression.Filter != null)
				{
					num = 0;
					expression = resourceSetExpression.Filter.Predicate;
				}
				else
				{
					num = 1;
					expression = list4[0];
				}
				for (int i = num; i < list4.Count; i++)
				{
					expression = Expression.And(expression, list4[i]);
				}
				ResourceBinder.AddSequenceQueryOption(resourceSetExpression, new FilterQueryOptionExpression(mce.Method.ReturnType, expression));
			}
			return resourceSetExpression;
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x0004B32C File Offset: 0x0004952C
		private static Expression BuildKeyPredicateFilter(InputReferenceExpression input, Dictionary<PropertyInfo, ConstantExpression> keyValuesDictionary)
		{
			Expression expression = null;
			foreach (KeyValuePair<PropertyInfo, ConstantExpression> keyValuePair in keyValuesDictionary)
			{
				Expression expression2 = Expression.Equal(Expression.Property(input, keyValuePair.Key), keyValuePair.Value);
				if (expression == null)
				{
					expression = expression2;
				}
				else
				{
					expression = Expression.And(expression, expression2);
				}
			}
			return expression;
		}

		// Token: 0x060013C5 RID: 5061 RVA: 0x0004B3A0 File Offset: 0x000495A0
		private static void AddConjuncts(Expression e, List<Expression> conjuncts)
		{
			if (ResourceBinder.PatternRules.MatchAnd(e))
			{
				BinaryExpression binaryExpression = (BinaryExpression)e;
				ResourceBinder.AddConjuncts(binaryExpression.Left, conjuncts);
				ResourceBinder.AddConjuncts(binaryExpression.Right, conjuncts);
				return;
			}
			conjuncts.Add(e);
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x0004B3DC File Offset: 0x000495DC
		internal bool AnalyzeProjection(MethodCallExpression mce, SequenceMethod sequenceMethod, out Expression e)
		{
			e = mce;
			bool matchMembers = true;
			ResourceExpression resourceExpression = this.Visit(mce.Arguments[0]) as ResourceExpression;
			if (resourceExpression == null)
			{
				return false;
			}
			if (sequenceMethod == SequenceMethod.SelectManyResultSelector)
			{
				Expression expression = mce.Arguments[1];
				if (!ResourceBinder.PatternRules.MatchParameterMemberAccess(expression))
				{
					return false;
				}
				Expression expression2 = mce.Arguments[2];
				LambdaExpression lambdaExpression;
				if (!ResourceBinder.PatternRules.MatchDoubleArgumentLambda(expression2, out lambdaExpression))
				{
					return false;
				}
				if (ResourceBinder.ExpressionPresenceVisitor.IsExpressionPresent(lambdaExpression.Parameters[0], lambdaExpression.Body))
				{
					return false;
				}
				List<ResourceExpression> referencedInputs = new List<ResourceExpression>();
				LambdaExpression lambdaExpression2 = ResourceBinder.StripTo<LambdaExpression>(expression);
				Expression expression3 = InputBinder.Bind(lambdaExpression2.Body, resourceExpression, lambdaExpression2.Parameters[0], referencedInputs);
				expression3 = ResourceBinder.StripCastMethodCalls(expression3);
				MemberExpression memberExpression;
				if (!ResourceBinder.PatternRules.MatchPropertyProjectionSet(resourceExpression, expression3, out memberExpression))
				{
					return false;
				}
				expression3 = memberExpression;
				ResourceExpression resourceExpression2 = ResourceBinder.CreateResourceSetExpression(mce.Method.ReturnType, resourceExpression, expression3, TypeSystem.GetElementType(expression3.Type));
				if (!ResourceBinder.PatternRules.MatchMemberInitExpressionWithDefaultConstructor(resourceExpression2, lambdaExpression) && !ResourceBinder.PatternRules.MatchNewExpression(resourceExpression2, lambdaExpression))
				{
					return false;
				}
				lambdaExpression = Expression.Lambda(lambdaExpression.Body, new ParameterExpression[]
				{
					lambdaExpression.Parameters[1]
				});
				ResourceExpression resourceExpression3 = resourceExpression2.CreateCloneWithNewType(mce.Type);
				bool flag;
				try
				{
					flag = ProjectionAnalyzer.Analyze(lambdaExpression, resourceExpression3, false);
				}
				catch (NotSupportedException)
				{
					flag = false;
				}
				if (!flag)
				{
					return false;
				}
				e = resourceExpression3;
				ResourceBinder.ValidationRules.RequireCanProject(resourceExpression2);
			}
			else
			{
				LambdaExpression le;
				if (!ResourceBinder.PatternRules.MatchSingleArgumentLambda(mce.Arguments[1], out le))
				{
					return false;
				}
				le = ProjectionRewriter.TryToRewrite(le, resourceExpression.ResourceType);
				ResourceExpression resourceExpression4 = resourceExpression.CreateCloneWithNewType(mce.Type);
				if (!ProjectionAnalyzer.Analyze(le, resourceExpression4, matchMembers))
				{
					return false;
				}
				ResourceBinder.ValidationRules.RequireCanProject(resourceExpression);
				e = resourceExpression4;
			}
			return true;
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x0004B59C File Offset: 0x0004979C
		internal static Expression AnalyzeNavigation(MethodCallExpression mce)
		{
			Expression expression = mce.Arguments[0];
			LambdaExpression lambdaExpression;
			if (!ResourceBinder.PatternRules.MatchSingleArgumentLambda(mce.Arguments[1], out lambdaExpression))
			{
				return mce;
			}
			if (ResourceBinder.PatternRules.MatchIdentitySelector(lambdaExpression))
			{
				return expression;
			}
			if (ResourceBinder.PatternRules.MatchTransparentIdentitySelector(expression, lambdaExpression))
			{
				return ResourceBinder.RemoveTransparentScope(mce.Method.ReturnType, (ResourceSetExpression)expression);
			}
			ResourceExpression resourceExpression;
			Expression expression2;
			MemberExpression memberExpression;
			if (ResourceBinder.IsValidNavigationSource(expression, out resourceExpression) && ResourceBinder.TryBindToInput(resourceExpression, lambdaExpression, out expression2) && ResourceBinder.PatternRules.MatchPropertyProjectionSingleton(resourceExpression, expression2, out memberExpression))
			{
				expression2 = memberExpression;
				return ResourceBinder.CreateNavigationPropertySingletonExpression(mce.Method.ReturnType, resourceExpression, expression2);
			}
			return mce;
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x0004B62E File Offset: 0x0004982E
		private static bool IsValidNavigationSource(Expression input, out ResourceExpression sourceExpression)
		{
			ResourceBinder.ValidationRules.RequireCanNavigate(input);
			sourceExpression = (input as ResourceExpression);
			return sourceExpression != null;
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x0004B648 File Offset: 0x00049848
		private static Expression LimitCardinality(MethodCallExpression mce, int maxCardinality)
		{
			if (mce.Arguments.Count != 1)
			{
				return mce;
			}
			ResourceSetExpression resourceSetExpression = mce.Arguments[0] as ResourceSetExpression;
			if (resourceSetExpression != null)
			{
				if (!resourceSetExpression.HasKeyPredicate && resourceSetExpression.NodeType != (ExpressionType)10001 && (resourceSetExpression.Take == null || (int)resourceSetExpression.Take.TakeAmount.Value > maxCardinality))
				{
					ResourceBinder.AddSequenceQueryOption(resourceSetExpression, new TakeQueryOptionExpression(mce.Type, Expression.Constant(maxCardinality)));
				}
				return mce.Arguments[0];
			}
			if (mce.Arguments[0] is NavigationPropertySingletonExpression)
			{
				return mce.Arguments[0];
			}
			return mce;
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x0004B6FC File Offset: 0x000498FC
		private static Expression AnalyzeCast(MethodCallExpression mce)
		{
			ResourceExpression resourceExpression = mce.Arguments[0] as ResourceExpression;
			if (resourceExpression != null)
			{
				return resourceExpression.CreateCloneWithNewType(mce.Method.ReturnType);
			}
			return mce;
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x0004B744 File Offset: 0x00049944
		private static ResourceSetExpression CreateResourceSetExpression(Type type, ResourceExpression source, Expression memberExpression, Type resourceType)
		{
			Type elementType = TypeSystem.GetElementType(type);
			Type type2 = typeof(IOrderedQueryable<>).MakeGenericType(new Type[]
			{
				elementType
			});
			ResourceSetExpression result = new ResourceSetExpression(type2, source, memberExpression, resourceType, source.ExpandPaths.ToList<string>(), source.CountOption, source.CustomQueryOptions.ToDictionary((KeyValuePair<ConstantExpression, ConstantExpression> kvp) => kvp.Key, (KeyValuePair<ConstantExpression, ConstantExpression> kvp) => kvp.Value), null);
			source.ExpandPaths.Clear();
			source.CountOption = CountOption.None;
			source.CustomQueryOptions.Clear();
			return result;
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x0004B808 File Offset: 0x00049A08
		private static NavigationPropertySingletonExpression CreateNavigationPropertySingletonExpression(Type type, ResourceExpression source, Expression memberExpression)
		{
			NavigationPropertySingletonExpression result = new NavigationPropertySingletonExpression(type, source, memberExpression, memberExpression.Type, source.ExpandPaths.ToList<string>(), source.CountOption, source.CustomQueryOptions.ToDictionary((KeyValuePair<ConstantExpression, ConstantExpression> kvp) => kvp.Key, (KeyValuePair<ConstantExpression, ConstantExpression> kvp) => kvp.Value), null);
			source.ExpandPaths.Clear();
			source.CountOption = CountOption.None;
			source.CustomQueryOptions.Clear();
			return result;
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x0004B89C File Offset: 0x00049A9C
		private static ResourceSetExpression RemoveTransparentScope(Type expectedResultType, ResourceSetExpression input)
		{
			ResourceSetExpression resourceSetExpression = new ResourceSetExpression(expectedResultType, input.Source, input.MemberExpression, input.ResourceType, input.ExpandPaths, input.CountOption, input.CustomQueryOptions, input.Projection);
			resourceSetExpression.KeyPredicate = input.KeyPredicate;
			foreach (QueryOptionExpression qoe in input.SequenceQueryOptions)
			{
				resourceSetExpression.AddSequenceQueryOption(qoe);
			}
			resourceSetExpression.OverrideInputReference(input);
			return resourceSetExpression;
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x0004B930 File Offset: 0x00049B30
		internal static Expression StripConvertToAssignable(Expression e)
		{
			UnaryExpression unaryExpression = e as UnaryExpression;
			Expression result;
			if (unaryExpression != null && ResourceBinder.PatternRules.MatchConvertToAssignable(unaryExpression))
			{
				result = unaryExpression.Operand;
			}
			else
			{
				result = e;
			}
			return result;
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x0004B95C File Offset: 0x00049B5C
		internal static T StripTo<T>(Expression expression) where T : Expression
		{
			Expression expression2;
			do
			{
				expression2 = expression;
				expression = ((expression.NodeType == ExpressionType.Quote) ? ((UnaryExpression)expression).Operand : expression);
				expression = ResourceBinder.StripConvertToAssignable(expression);
			}
			while (expression2 != expression);
			return expression2 as T;
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x0004B99C File Offset: 0x00049B9C
		internal override Expression VisitResourceSetExpression(ResourceSetExpression rse)
		{
			if (rse.NodeType == (ExpressionType)10000)
			{
				return new ResourceSetExpression(rse.Type, rse.Source, rse.MemberExpression, rse.ResourceType, null, CountOption.None, null, null);
			}
			return rse;
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x0004B9CE File Offset: 0x00049BCE
		private static bool TryGetResourceSetMethodArguments(MethodCallExpression mce, out ResourceSetExpression input, out LambdaExpression lambda)
		{
			input = null;
			lambda = null;
			input = (mce.Arguments[0] as ResourceSetExpression);
			return input != null && ResourceBinder.PatternRules.MatchSingleArgumentLambda(mce.Arguments[1], out lambda);
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x0004BA04 File Offset: 0x00049C04
		private static bool TryBindToInput(ResourceExpression input, LambdaExpression le, out Expression bound)
		{
			List<ResourceExpression> list = new List<ResourceExpression>();
			bound = InputBinder.Bind(le.Body, input, le.Parameters[0], list);
			if (list.Count > 1 || (list.Count == 1 && list[0] != input))
			{
				bound = null;
			}
			return bound != null;
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x0004BA5C File Offset: 0x00049C5C
		private static Expression AnalyzeResourceSetConstantMethod(MethodCallExpression mce, Func<MethodCallExpression, ResourceExpression, ConstantExpression, Expression> constantMethodAnalyzer)
		{
			ResourceExpression arg = (ResourceExpression)mce.Arguments[0];
			ConstantExpression constantExpression = ResourceBinder.StripTo<ConstantExpression>(mce.Arguments[1]);
			if (constantExpression == null)
			{
				return mce;
			}
			return constantMethodAnalyzer(mce, arg, constantExpression);
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x0004BA9C File Offset: 0x00049C9C
		private static Expression AnalyzeCountMethod(MethodCallExpression mce)
		{
			ResourceExpression resourceExpression = (ResourceExpression)mce.Arguments[0];
			if (resourceExpression == null)
			{
				return mce;
			}
			ResourceBinder.ValidationRules.RequireCanAddCount(resourceExpression);
			ResourceBinder.ValidationRules.RequireNonSingleton(resourceExpression);
			resourceExpression.CountOption = CountOption.ValueOnly;
			return resourceExpression;
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x0004BAD4 File Offset: 0x00049CD4
		private static void AddSequenceQueryOption(ResourceExpression target, QueryOptionExpression qoe)
		{
			ResourceBinder.ValidationRules.RequireNonSingleton(target);
			ResourceSetExpression resourceSetExpression = (ResourceSetExpression)target;
			if (qoe.NodeType == (ExpressionType)10006)
			{
				if (resourceSetExpression.Take != null)
				{
					throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "The {0} query option cannot be specified after the {1} query option.", new object[]
					{
						"filter",
						"top"
					}));
				}
				if (resourceSetExpression.Projection != null)
				{
					throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "The {0} query option cannot be specified after the {1} query option.", new object[]
					{
						"filter",
						"select"
					}));
				}
			}
			resourceSetExpression.AddSequenceQueryOption(qoe);
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x0004BB6C File Offset: 0x00049D6C
		internal override Expression VisitBinary(BinaryExpression b)
		{
			Expression expression = base.VisitBinary(b);
			if (ResourceBinder.PatternRules.MatchStringAddition(expression))
			{
				BinaryExpression binaryExpression = ResourceBinder.StripTo<BinaryExpression>(expression);
				MethodInfo method = typeof(string).GetMethod("Concat", new Type[]
				{
					typeof(string),
					typeof(string)
				});
				return Expression.Call(method, new Expression[]
				{
					binaryExpression.Left,
					binaryExpression.Right
				});
			}
			return expression;
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x0004BBF0 File Offset: 0x00049DF0
		internal override Expression VisitMemberAccess(MemberExpression m)
		{
			Expression expression = base.VisitMemberAccess(m);
			MemberExpression memberExpression = ResourceBinder.StripTo<MemberExpression>(expression);
			PropertyInfo pi;
			MethodInfo method;
			if (memberExpression != null && ResourceBinder.PatternRules.MatchNonPrivateReadableProperty(memberExpression, out pi) && TypeSystem.TryGetPropertyAsMethod(pi, out method))
			{
				return Expression.Call(memberExpression.Expression, method);
			}
			return expression;
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x0004BC88 File Offset: 0x00049E88
		internal override Expression VisitMethodCall(MethodCallExpression mce)
		{
			SequenceMethod sequenceMethod;
			Expression expression;
			if (ReflectionUtil.TryIdentifySequenceMethod(mce.Method, out sequenceMethod) && (sequenceMethod == SequenceMethod.Select || sequenceMethod == SequenceMethod.SelectManyResultSelector) && this.AnalyzeProjection(mce, sequenceMethod, out expression))
			{
				return expression;
			}
			expression = base.VisitMethodCall(mce);
			mce = (expression as MethodCallExpression);
			if (mce == null)
			{
				return expression;
			}
			if (ReflectionUtil.TryIdentifySequenceMethod(mce.Method, out sequenceMethod))
			{
				SequenceMethod sequenceMethod2 = sequenceMethod;
				if (sequenceMethod2 <= SequenceMethod.Take)
				{
					switch (sequenceMethod2)
					{
					case SequenceMethod.Where:
						return ResourceBinder.AnalyzePredicate(mce);
					case SequenceMethod.WhereOrdinal:
					case SequenceMethod.OfType:
						break;
					case SequenceMethod.Cast:
						return ResourceBinder.AnalyzeCast(mce);
					case SequenceMethod.Select:
						return ResourceBinder.AnalyzeNavigation(mce);
					default:
						if (sequenceMethod2 == SequenceMethod.Take)
						{
							return ResourceBinder.AnalyzeResourceSetConstantMethod(mce, delegate(MethodCallExpression callExp, ResourceExpression resource, ConstantExpression takeCount)
							{
								ResourceBinder.AddSequenceQueryOption(resource, new TakeQueryOptionExpression(callExp.Type, takeCount));
								return resource;
							});
						}
						break;
					}
				}
				else
				{
					switch (sequenceMethod2)
					{
					case SequenceMethod.First:
					case SequenceMethod.FirstOrDefault:
						return ResourceBinder.LimitCardinality(mce, 1);
					case SequenceMethod.FirstPredicate:
						break;
					default:
						switch (sequenceMethod2)
						{
						case SequenceMethod.Single:
						case SequenceMethod.SingleOrDefault:
							return ResourceBinder.LimitCardinality(mce, 2);
						case SequenceMethod.SinglePredicate:
							break;
						default:
							switch (sequenceMethod2)
							{
							case SequenceMethod.Count:
							case SequenceMethod.LongCount:
								return ResourceBinder.AnalyzeCountMethod(mce);
							}
							break;
						}
						break;
					}
				}
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "The method '{0}' is not supported", new object[]
				{
					mce.Method.Name
				}));
			}
			if (!(mce.Method.DeclaringType == typeof(TableQueryableExtensions)))
			{
				return mce;
			}
			Type[] genericArguments = mce.Method.GetGenericArguments();
			Type type = genericArguments[0];
			if (mce.Method == TableQueryableExtensions.WithOptionsMethodInfo.MakeGenericMethod(new Type[]
			{
				type
			}))
			{
				return ResourceBinder.AnalyzeResourceSetConstantMethod(mce, delegate(MethodCallExpression callExp, ResourceExpression resource, ConstantExpression options)
				{
					ResourceBinder.AddSequenceQueryOption(resource, new RequestOptionsQueryOptionExpression(callExp.Type, options));
					return resource;
				});
			}
			if (mce.Method == TableQueryableExtensions.WithContextMethodInfo.MakeGenericMethod(new Type[]
			{
				type
			}))
			{
				return ResourceBinder.AnalyzeResourceSetConstantMethod(mce, delegate(MethodCallExpression callExp, ResourceExpression resource, ConstantExpression ctx)
				{
					ResourceBinder.AddSequenceQueryOption(resource, new OperationContextQueryOptionExpression(callExp.Type, ctx));
					return resource;
				});
			}
			if (genericArguments.Length > 1 && mce.Method == TableQueryableExtensions.ResolveMethodInfo.MakeGenericMethod(new Type[]
			{
				type,
				genericArguments[1]
			}))
			{
				return ResourceBinder.AnalyzeResourceSetConstantMethod(mce, delegate(MethodCallExpression callExp, ResourceExpression resource, ConstantExpression resolver)
				{
					ResourceBinder.AddSequenceQueryOption(resource, new EntityResolverQueryOptionExpression(callExp.Type, resolver));
					return resource;
				});
			}
			throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "The method '{0}' is not supported", new object[]
			{
				mce.Method.Name
			}));
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x0004BF20 File Offset: 0x0004A120
		private static Expression StripCastMethodCalls(Expression expression)
		{
			MethodCallExpression methodCallExpression = ResourceBinder.StripTo<MethodCallExpression>(expression);
			while (methodCallExpression != null && ReflectionUtil.IsSequenceMethod(methodCallExpression.Method, SequenceMethod.Cast))
			{
				expression = methodCallExpression.Arguments[0];
				methodCallExpression = ResourceBinder.StripTo<MethodCallExpression>(expression);
			}
			return expression;
		}

		// Token: 0x0200012A RID: 298
		internal static class PatternRules
		{
			// Token: 0x060013E3 RID: 5091 RVA: 0x0004BF65 File Offset: 0x0004A165
			internal static bool MatchConvertToAssignable(UnaryExpression expression)
			{
				return (expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked || expression.NodeType == ExpressionType.TypeAs) && expression.Type.IsAssignableFrom(expression.Operand.Type);
			}

			// Token: 0x060013E4 RID: 5092 RVA: 0x0004BFA0 File Offset: 0x0004A1A0
			internal static bool MatchParameterMemberAccess(Expression expression)
			{
				LambdaExpression lambdaExpression = ResourceBinder.StripTo<LambdaExpression>(expression);
				if (lambdaExpression == null || lambdaExpression.Parameters.Count != 1)
				{
					return false;
				}
				ParameterExpression parameterExpression = lambdaExpression.Parameters[0];
				Expression expression2 = ResourceBinder.StripCastMethodCalls(lambdaExpression.Body);
				for (MemberExpression memberExpression = ResourceBinder.StripTo<MemberExpression>(expression2); memberExpression != null; memberExpression = ResourceBinder.StripTo<MemberExpression>(memberExpression.Expression))
				{
					if (memberExpression.Expression == parameterExpression)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x060013E5 RID: 5093 RVA: 0x0004C004 File Offset: 0x0004A204
			internal static bool MatchPropertyAccess(Expression e, out MemberExpression member, out Expression instance, out List<string> propertyPath)
			{
				instance = null;
				propertyPath = null;
				MemberExpression memberExpression = ResourceBinder.StripTo<MemberExpression>(e);
				member = memberExpression;
				while (memberExpression != null)
				{
					PropertyInfo propertyInfo;
					if (ResourceBinder.PatternRules.MatchNonPrivateReadableProperty(memberExpression, out propertyInfo))
					{
						if (propertyPath == null)
						{
							propertyPath = new List<string>();
						}
						propertyPath.Insert(0, propertyInfo.Name);
						e = memberExpression.Expression;
						memberExpression = ResourceBinder.StripTo<MemberExpression>(e);
					}
					else
					{
						memberExpression = null;
					}
				}
				if (propertyPath != null)
				{
					instance = e;
					return true;
				}
				return false;
			}

			// Token: 0x060013E6 RID: 5094 RVA: 0x0004C066 File Offset: 0x0004A266
			internal static bool MatchConstant(Expression e, out ConstantExpression constExpr)
			{
				constExpr = (e as ConstantExpression);
				return constExpr != null;
			}

			// Token: 0x060013E7 RID: 5095 RVA: 0x0004C078 File Offset: 0x0004A278
			internal static bool MatchAnd(Expression e)
			{
				BinaryExpression binaryExpression = e as BinaryExpression;
				return binaryExpression != null && (binaryExpression.NodeType == ExpressionType.And || binaryExpression.NodeType == ExpressionType.AndAlso);
			}

			// Token: 0x060013E8 RID: 5096 RVA: 0x0004C0A8 File Offset: 0x0004A2A8
			internal static bool MatchNonPrivateReadableProperty(Expression e, out PropertyInfo propInfo)
			{
				MemberExpression memberExpression = e as MemberExpression;
				if (memberExpression == null)
				{
					propInfo = null;
					return false;
				}
				return ResourceBinder.PatternRules.MatchNonPrivateReadableProperty(memberExpression, out propInfo);
			}

			// Token: 0x060013E9 RID: 5097 RVA: 0x0004C0CC File Offset: 0x0004A2CC
			internal static bool MatchNonPrivateReadableProperty(MemberExpression me, out PropertyInfo propInfo)
			{
				propInfo = null;
				if (me.Member.MemberType == MemberTypes.Property)
				{
					PropertyInfo propertyInfo = (PropertyInfo)me.Member;
					if (propertyInfo.CanRead && !TypeSystem.IsPrivate(propertyInfo))
					{
						propInfo = propertyInfo;
						return true;
					}
				}
				return false;
			}

			// Token: 0x060013EA RID: 5098 RVA: 0x0004C110 File Offset: 0x0004A310
			internal static bool MatchReferenceEquals(Expression expression)
			{
				MethodCallExpression methodCallExpression = expression as MethodCallExpression;
				return methodCallExpression != null && methodCallExpression.Method == typeof(object).GetMethod("ReferenceEquals");
			}

			// Token: 0x060013EB RID: 5099 RVA: 0x0004C148 File Offset: 0x0004A348
			internal static bool MatchResource(Expression expression, out ResourceExpression resource)
			{
				resource = (expression as ResourceExpression);
				return resource != null;
			}

			// Token: 0x060013EC RID: 5100 RVA: 0x0004C15A File Offset: 0x0004A35A
			internal static bool MatchDoubleArgumentLambda(Expression expression, out LambdaExpression lambda)
			{
				return ResourceBinder.PatternRules.MatchNaryLambda(expression, 2, out lambda);
			}

			// Token: 0x060013ED RID: 5101 RVA: 0x0004C164 File Offset: 0x0004A364
			internal static bool MatchIdentitySelector(LambdaExpression lambda)
			{
				ParameterExpression parameterExpression = lambda.Parameters[0];
				return parameterExpression == ResourceBinder.StripTo<ParameterExpression>(lambda.Body);
			}

			// Token: 0x060013EE RID: 5102 RVA: 0x0004C18C File Offset: 0x0004A38C
			internal static bool MatchSingleArgumentLambda(Expression expression, out LambdaExpression lambda)
			{
				return ResourceBinder.PatternRules.MatchNaryLambda(expression, 1, out lambda);
			}

			// Token: 0x060013EF RID: 5103 RVA: 0x0004C198 File Offset: 0x0004A398
			internal static bool MatchTransparentIdentitySelector(Expression input, LambdaExpression selector)
			{
				if (selector.Parameters.Count != 1)
				{
					return false;
				}
				ResourceSetExpression resourceSetExpression = input as ResourceSetExpression;
				if (resourceSetExpression == null || resourceSetExpression.TransparentScope == null)
				{
					return false;
				}
				Expression body = selector.Body;
				ParameterExpression parameterExpression = selector.Parameters[0];
				MemberExpression memberExpression;
				Expression expression;
				List<string> list;
				return ResourceBinder.PatternRules.MatchPropertyAccess(body, out memberExpression, out expression, out list) && (expression == parameterExpression && list.Count == 1) && list[0] == resourceSetExpression.TransparentScope.Accessor;
			}

			// Token: 0x060013F0 RID: 5104 RVA: 0x0004C218 File Offset: 0x0004A418
			internal static bool MatchIdentityProjectionResultSelector(Expression e)
			{
				LambdaExpression lambdaExpression = (LambdaExpression)e;
				return lambdaExpression.Body == lambdaExpression.Parameters[1];
			}

			// Token: 0x060013F1 RID: 5105 RVA: 0x0004C25C File Offset: 0x0004A45C
			internal static bool MatchTransparentScopeSelector(ResourceSetExpression input, LambdaExpression resultSelector, out ResourceSetExpression.TransparentAccessors transparentScope)
			{
				transparentScope = null;
				if (resultSelector.Body.NodeType != ExpressionType.New)
				{
					return false;
				}
				NewExpression newExpression = (NewExpression)resultSelector.Body;
				if (newExpression.Arguments.Count < 2)
				{
					return false;
				}
				if (newExpression.Type.BaseType != typeof(object))
				{
					return false;
				}
				ParameterInfo[] parameters = newExpression.Constructor.GetParameters();
				if (newExpression.Members.Count != parameters.Length)
				{
					return false;
				}
				ResourceSetExpression resourceSetExpression = input.Source as ResourceSetExpression;
				int num = -1;
				ParameterExpression parameterExpression = resultSelector.Parameters[0];
				ParameterExpression parameterExpression2 = resultSelector.Parameters[1];
				MemberInfo[] array = new MemberInfo[newExpression.Members.Count];
				PropertyInfo[] properties = newExpression.Type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
				Dictionary<string, Expression> dictionary = new Dictionary<string, Expression>(parameters.Length - 1, StringComparer.Ordinal);
				for (int i = 0; i < newExpression.Arguments.Count; i++)
				{
					Expression expression = newExpression.Arguments[i];
					MemberInfo member = newExpression.Members[i];
					if (!ResourceBinder.PatternRules.ExpressionIsSimpleAccess(expression, resultSelector.Parameters))
					{
						return false;
					}
					if (member.MemberType == MemberTypes.Method)
					{
						member = (from property in properties
						where property.GetGetMethod() == member
						select property).FirstOrDefault<PropertyInfo>();
						if (member == null)
						{
							return false;
						}
					}
					if (member.Name != parameters[i].Name)
					{
						return false;
					}
					array[i] = member;
					ParameterExpression parameterExpression3 = ResourceBinder.StripTo<ParameterExpression>(expression);
					if (parameterExpression2 == parameterExpression3)
					{
						if (num != -1)
						{
							return false;
						}
						num = i;
					}
					else if (parameterExpression == parameterExpression3)
					{
						dictionary[member.Name] = resourceSetExpression.CreateReference();
					}
					else
					{
						List<ResourceExpression> list = new List<ResourceExpression>();
						InputBinder.Bind(expression, resourceSetExpression, resultSelector.Parameters[0], list);
						if (list.Count != 1)
						{
							return false;
						}
						dictionary[member.Name] = list[0].CreateReference();
					}
				}
				if (num == -1)
				{
					return false;
				}
				string name = array[num].Name;
				transparentScope = new ResourceSetExpression.TransparentAccessors(name, dictionary);
				return true;
			}

			// Token: 0x060013F2 RID: 5106 RVA: 0x0004C4A3 File Offset: 0x0004A6A3
			internal static bool MatchPropertyProjectionSet(ResourceExpression input, Expression potentialPropertyRef, out MemberExpression navigationMember)
			{
				return ResourceBinder.PatternRules.MatchNavigationPropertyProjection(input, potentialPropertyRef, true, out navigationMember);
			}

			// Token: 0x060013F3 RID: 5107 RVA: 0x0004C4AE File Offset: 0x0004A6AE
			internal static bool MatchPropertyProjectionSingleton(ResourceExpression input, Expression potentialPropertyRef, out MemberExpression navigationMember)
			{
				return ResourceBinder.PatternRules.MatchNavigationPropertyProjection(input, potentialPropertyRef, false, out navigationMember);
			}

			// Token: 0x060013F4 RID: 5108 RVA: 0x0004C4BC File Offset: 0x0004A6BC
			private static bool MatchNavigationPropertyProjection(ResourceExpression input, Expression potentialPropertyRef, bool requireSet, out MemberExpression navigationMember)
			{
				Expression expression;
				List<string> list;
				if (ResourceBinder.PatternRules.MatchNonSingletonProperty(potentialPropertyRef) == requireSet && ResourceBinder.PatternRules.MatchPropertyAccess(potentialPropertyRef, out navigationMember, out expression, out list) && expression == input.CreateReference())
				{
					return true;
				}
				navigationMember = null;
				return false;
			}

			// Token: 0x060013F5 RID: 5109 RVA: 0x0004C4F0 File Offset: 0x0004A6F0
			internal static bool MatchMemberInitExpressionWithDefaultConstructor(Expression source, LambdaExpression e)
			{
				MemberInitExpression memberInitExpression = ResourceBinder.StripTo<MemberInitExpression>(e.Body);
				ResourceExpression resourceExpression;
				return ResourceBinder.PatternRules.MatchResource(source, out resourceExpression) && memberInitExpression != null && memberInitExpression.NewExpression.Arguments.Count == 0;
			}

			// Token: 0x060013F6 RID: 5110 RVA: 0x0004C52C File Offset: 0x0004A72C
			internal static bool MatchNewExpression(Expression source, LambdaExpression e)
			{
				ResourceExpression resourceExpression;
				return ResourceBinder.PatternRules.MatchResource(source, out resourceExpression) && e.Body is NewExpression;
			}

			// Token: 0x060013F7 RID: 5111 RVA: 0x0004C553 File Offset: 0x0004A753
			internal static bool MatchNot(Expression expression)
			{
				return expression.NodeType == ExpressionType.Not;
			}

			// Token: 0x060013F8 RID: 5112 RVA: 0x0004C560 File Offset: 0x0004A760
			internal static bool MatchNonSingletonProperty(Expression e)
			{
				return TypeSystem.FindIEnumerable(e.Type) != null && e.Type != typeof(char[]) && e.Type != typeof(byte[]);
			}

			// Token: 0x060013F9 RID: 5113 RVA: 0x0004C5B0 File Offset: 0x0004A7B0
			internal static ResourceBinder.PatternRules.MatchNullCheckResult MatchNullCheck(Expression entityInScope, ConditionalExpression conditional)
			{
				ResourceBinder.PatternRules.MatchNullCheckResult result = default(ResourceBinder.PatternRules.MatchNullCheckResult);
				ResourceBinder.PatternRules.MatchEqualityCheckResult matchEqualityCheckResult = ResourceBinder.PatternRules.MatchEquality(conditional.Test);
				if (!matchEqualityCheckResult.Match)
				{
					return result;
				}
				Expression expression;
				if (matchEqualityCheckResult.EqualityYieldsTrue)
				{
					if (!ResourceBinder.PatternRules.MatchNullConstant(conditional.IfTrue))
					{
						return result;
					}
					expression = conditional.IfFalse;
				}
				else
				{
					if (!ResourceBinder.PatternRules.MatchNullConstant(conditional.IfFalse))
					{
						return result;
					}
					expression = conditional.IfTrue;
				}
				Expression expression2;
				if (ResourceBinder.PatternRules.MatchNullConstant(matchEqualityCheckResult.TestLeft))
				{
					expression2 = matchEqualityCheckResult.TestRight;
				}
				else
				{
					if (!ResourceBinder.PatternRules.MatchNullConstant(matchEqualityCheckResult.TestRight))
					{
						return result;
					}
					expression2 = matchEqualityCheckResult.TestLeft;
				}
				MemberAssignmentAnalysis memberAssignmentAnalysis = MemberAssignmentAnalysis.Analyze(entityInScope, expression);
				if (memberAssignmentAnalysis.MultiplePathsFound)
				{
					return result;
				}
				MemberAssignmentAnalysis memberAssignmentAnalysis2 = MemberAssignmentAnalysis.Analyze(entityInScope, expression2);
				if (memberAssignmentAnalysis2.MultiplePathsFound)
				{
					return result;
				}
				Expression[] expressionsToTargetEntity = memberAssignmentAnalysis.GetExpressionsToTargetEntity();
				Expression[] expressionsToTargetEntity2 = memberAssignmentAnalysis2.GetExpressionsToTargetEntity();
				if (expressionsToTargetEntity2.Length > expressionsToTargetEntity.Length)
				{
					return result;
				}
				for (int i = 0; i < expressionsToTargetEntity2.Length; i++)
				{
					Expression expression3 = expressionsToTargetEntity[i];
					Expression expression4 = expressionsToTargetEntity2[i];
					if (expression3 != expression4)
					{
						if (expression3.NodeType != expression4.NodeType || expression3.NodeType != ExpressionType.MemberAccess)
						{
							return result;
						}
						if (((MemberExpression)expression3).Member != ((MemberExpression)expression4).Member)
						{
							return result;
						}
					}
				}
				result.AssignExpression = expression;
				result.Match = true;
				result.TestToNullExpression = expression2;
				return result;
			}

			// Token: 0x060013FA RID: 5114 RVA: 0x0004C70C File Offset: 0x0004A90C
			internal static bool MatchNullConstant(Expression expression)
			{
				ConstantExpression constantExpression = expression as ConstantExpression;
				return constantExpression != null && constantExpression.Value == null;
			}

			// Token: 0x060013FB RID: 5115 RVA: 0x0004C72E File Offset: 0x0004A92E
			internal static bool MatchBinaryExpression(Expression e)
			{
				return e is BinaryExpression;
			}

			// Token: 0x060013FC RID: 5116 RVA: 0x0004C739 File Offset: 0x0004A939
			internal static bool MatchBinaryEquality(Expression e)
			{
				return ResourceBinder.PatternRules.MatchBinaryExpression(e) && ((BinaryExpression)e).NodeType == ExpressionType.Equal;
			}

			// Token: 0x060013FD RID: 5117 RVA: 0x0004C754 File Offset: 0x0004A954
			internal static bool MatchStringAddition(Expression e)
			{
				if (e.NodeType == ExpressionType.Add)
				{
					BinaryExpression binaryExpression = e as BinaryExpression;
					return binaryExpression != null && binaryExpression.Left.Type == typeof(string) && binaryExpression.Right.Type == typeof(string);
				}
				return false;
			}

			// Token: 0x060013FE RID: 5118 RVA: 0x0004C7B0 File Offset: 0x0004A9B0
			internal static ResourceBinder.PatternRules.MatchEqualityCheckResult MatchEquality(Expression expression)
			{
				ResourceBinder.PatternRules.MatchEqualityCheckResult result = default(ResourceBinder.PatternRules.MatchEqualityCheckResult);
				result.Match = false;
				result.EqualityYieldsTrue = true;
				while (!ResourceBinder.PatternRules.MatchReferenceEquals(expression))
				{
					if (!ResourceBinder.PatternRules.MatchNot(expression))
					{
						BinaryExpression binaryExpression = expression as BinaryExpression;
						if (binaryExpression != null)
						{
							if (binaryExpression.NodeType == ExpressionType.NotEqual)
							{
								result.EqualityYieldsTrue = !result.EqualityYieldsTrue;
							}
							else if (binaryExpression.NodeType != ExpressionType.Equal)
							{
								return result;
							}
							result.TestLeft = binaryExpression.Left;
							result.TestRight = binaryExpression.Right;
							result.Match = true;
						}
						return result;
					}
					result.EqualityYieldsTrue = !result.EqualityYieldsTrue;
					expression = ((UnaryExpression)expression).Operand;
				}
				MethodCallExpression methodCallExpression = (MethodCallExpression)expression;
				result.Match = true;
				result.TestLeft = methodCallExpression.Arguments[0];
				result.TestRight = methodCallExpression.Arguments[1];
				return result;
			}

			// Token: 0x060013FF RID: 5119 RVA: 0x0004C890 File Offset: 0x0004AA90
			private static bool ExpressionIsSimpleAccess(Expression argument, ReadOnlyCollection<ParameterExpression> expressions)
			{
				Expression expression = argument;
				MemberExpression memberExpression;
				do
				{
					memberExpression = (expression as MemberExpression);
					if (memberExpression != null)
					{
						expression = memberExpression.Expression;
					}
				}
				while (memberExpression != null);
				ParameterExpression parameterExpression = expression as ParameterExpression;
				return parameterExpression != null && expressions.Contains(parameterExpression);
			}

			// Token: 0x06001400 RID: 5120 RVA: 0x0004C8C8 File Offset: 0x0004AAC8
			private static bool MatchNaryLambda(Expression expression, int parameterCount, out LambdaExpression lambda)
			{
				lambda = null;
				LambdaExpression lambdaExpression = ResourceBinder.StripTo<LambdaExpression>(expression);
				if (lambdaExpression != null && lambdaExpression.Parameters.Count == parameterCount)
				{
					lambda = lambdaExpression;
				}
				return lambda != null;
			}

			// Token: 0x0200012B RID: 299
			internal struct MatchNullCheckResult
			{
				// Token: 0x04000685 RID: 1669
				internal Expression AssignExpression;

				// Token: 0x04000686 RID: 1670
				internal bool Match;

				// Token: 0x04000687 RID: 1671
				internal Expression TestToNullExpression;
			}

			// Token: 0x0200012C RID: 300
			internal struct MatchEqualityCheckResult
			{
				// Token: 0x04000688 RID: 1672
				internal bool EqualityYieldsTrue;

				// Token: 0x04000689 RID: 1673
				internal bool Match;

				// Token: 0x0400068A RID: 1674
				internal Expression TestLeft;

				// Token: 0x0400068B RID: 1675
				internal Expression TestRight;
			}
		}

		// Token: 0x0200012D RID: 301
		private static class ValidationRules
		{
			// Token: 0x06001401 RID: 5121 RVA: 0x0004C8FC File Offset: 0x0004AAFC
			internal static void RequireCanNavigate(Expression e)
			{
				ResourceSetExpression resourceSetExpression = e as ResourceSetExpression;
				if (resourceSetExpression != null && resourceSetExpression.HasSequenceQueryOptions)
				{
					throw new NotSupportedException("Can only specify query options (orderby, where, take, skip) after last navigation.");
				}
				ResourceExpression resourceExpression;
				if (ResourceBinder.PatternRules.MatchResource(e, out resourceExpression) && resourceExpression.Projection != null)
				{
					throw new NotSupportedException("Can only specify query options (orderby, where, take, skip) after last navigation.");
				}
			}

			// Token: 0x06001402 RID: 5122 RVA: 0x0004C944 File Offset: 0x0004AB44
			internal static void RequireCanProject(Expression e)
			{
				ResourceExpression resourceExpression = (ResourceExpression)e;
				if (!ResourceBinder.PatternRules.MatchResource(e, out resourceExpression))
				{
					throw new NotSupportedException("Can only project the last entity type in the query being translated.");
				}
				if (resourceExpression.Projection != null)
				{
					throw new NotSupportedException("Cannot translate multiple Linq Select operations in a single 'select' query option.");
				}
				if (resourceExpression.ExpandPaths.Count > 0)
				{
					throw new NotSupportedException("Cannot create projection while there is an explicit expansion specified on the same query.");
				}
			}

			// Token: 0x06001403 RID: 5123 RVA: 0x0004C99C File Offset: 0x0004AB9C
			internal static void RequireCanAddCount(Expression e)
			{
				ResourceExpression resourceExpression = (ResourceExpression)e;
				if (!ResourceBinder.PatternRules.MatchResource(e, out resourceExpression))
				{
					throw new NotSupportedException("Cannot add count option to the resource set.");
				}
				if (resourceExpression.CountOption != CountOption.None)
				{
					throw new NotSupportedException("Cannot add count option to the resource set because it would conflict with existing count options.");
				}
			}

			// Token: 0x06001404 RID: 5124 RVA: 0x0004C9D8 File Offset: 0x0004ABD8
			internal static void RequireNonSingleton(Expression e)
			{
				ResourceExpression resourceExpression = e as ResourceExpression;
				if (resourceExpression != null && resourceExpression.IsSingleton)
				{
					throw new NotSupportedException("Cannot specify query options (orderby, where, take, skip) on single resource.");
				}
			}
		}

		// Token: 0x0200012E RID: 302
		private sealed class PropertyInfoEqualityComparer : IEqualityComparer<PropertyInfo>
		{
			// Token: 0x06001405 RID: 5125 RVA: 0x0004CA02 File Offset: 0x0004AC02
			private PropertyInfoEqualityComparer()
			{
			}

			// Token: 0x06001406 RID: 5126 RVA: 0x0004CA0C File Offset: 0x0004AC0C
			public bool Equals(PropertyInfo left, PropertyInfo right)
			{
				return object.ReferenceEquals(left, right) || (!(null == left) && !(null == right) && object.ReferenceEquals(left.DeclaringType, right.DeclaringType) && left.Name.Equals(right.Name));
			}

			// Token: 0x06001407 RID: 5127 RVA: 0x0004CA5E File Offset: 0x0004AC5E
			public int GetHashCode(PropertyInfo obj)
			{
				if (!(null != obj))
				{
					return 0;
				}
				return obj.GetHashCode();
			}

			// Token: 0x0400068C RID: 1676
			internal static readonly ResourceBinder.PropertyInfoEqualityComparer Instance = new ResourceBinder.PropertyInfoEqualityComparer();
		}

		// Token: 0x0200012F RID: 303
		private sealed class ExpressionPresenceVisitor : DataServiceALinqExpressionVisitor
		{
			// Token: 0x06001409 RID: 5129 RVA: 0x0004CA7D File Offset: 0x0004AC7D
			private ExpressionPresenceVisitor(Expression target)
			{
				this.target = target;
			}

			// Token: 0x0600140A RID: 5130 RVA: 0x0004CA8C File Offset: 0x0004AC8C
			internal static bool IsExpressionPresent(Expression target, Expression tree)
			{
				ResourceBinder.ExpressionPresenceVisitor expressionPresenceVisitor = new ResourceBinder.ExpressionPresenceVisitor(target);
				expressionPresenceVisitor.Visit(tree);
				return expressionPresenceVisitor.found;
			}

			// Token: 0x0600140B RID: 5131 RVA: 0x0004CAB0 File Offset: 0x0004ACB0
			internal override Expression Visit(Expression exp)
			{
				Expression result;
				if (this.found || object.ReferenceEquals(this.target, exp))
				{
					this.found = true;
					result = exp;
				}
				else
				{
					result = base.Visit(exp);
				}
				return result;
			}

			// Token: 0x0400068D RID: 1677
			private readonly Expression target;

			// Token: 0x0400068E RID: 1678
			private bool found;
		}
	}
}
