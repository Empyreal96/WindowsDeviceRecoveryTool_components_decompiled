using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Data.Services.Client
{
	// Token: 0x020000E0 RID: 224
	internal sealed class DataServiceQueryProvider : IQueryProvider
	{
		// Token: 0x06000747 RID: 1863 RVA: 0x0001F24D File Offset: 0x0001D44D
		internal DataServiceQueryProvider(DataServiceContext context)
		{
			this.Context = context;
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0001F25C File Offset: 0x0001D45C
		public IQueryable CreateQuery(Expression expression)
		{
			Util.CheckArgumentNull<Expression>(expression, "expression");
			Type elementType = TypeSystem.GetElementType(expression.Type);
			Type type = typeof(DataServiceQuery<>.DataServiceOrderedQuery).MakeGenericType(new Type[]
			{
				elementType
			});
			object[] arguments = new object[]
			{
				expression,
				this
			};
			ConstructorInfo instanceConstructor = type.GetInstanceConstructor(false, new Type[]
			{
				typeof(Expression),
				typeof(DataServiceQueryProvider)
			});
			return (IQueryable)Util.ConstructorInvoke(instanceConstructor, arguments);
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0001F2EF File Offset: 0x0001D4EF
		public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
		{
			Util.CheckArgumentNull<Expression>(expression, "expression");
			return new DataServiceQuery<TElement>.DataServiceOrderedQuery(expression, this);
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0001F304 File Offset: 0x0001D504
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public object Execute(Expression expression)
		{
			Util.CheckArgumentNull<Expression>(expression, "expression");
			MethodInfo method = typeof(DataServiceQueryProvider).GetMethod("ReturnSingleton", false, false);
			return method.MakeGenericMethod(new Type[]
			{
				expression.Type
			}).Invoke(this, new object[]
			{
				expression
			});
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0001F35D File Offset: 0x0001D55D
		public TResult Execute<TResult>(Expression expression)
		{
			Util.CheckArgumentNull<Expression>(expression, "expression");
			return this.ReturnSingleton<TResult>(expression);
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0001F374 File Offset: 0x0001D574
		internal TElement ReturnSingleton<TElement>(Expression expression)
		{
			IQueryable<TElement> queryable = new DataServiceQuery<TElement>.DataServiceOrderedQuery(expression, this);
			MethodCallExpression methodCallExpression = expression as MethodCallExpression;
			SequenceMethod sequenceMethod;
			if (ReflectionUtil.TryIdentifySequenceMethod(methodCallExpression.Method, out sequenceMethod))
			{
				SequenceMethod sequenceMethod2 = sequenceMethod;
				switch (sequenceMethod2)
				{
				case SequenceMethod.First:
					return queryable.AsEnumerable<TElement>().First<TElement>();
				case SequenceMethod.FirstPredicate:
					break;
				case SequenceMethod.FirstOrDefault:
					return queryable.AsEnumerable<TElement>().FirstOrDefault<TElement>();
				default:
					switch (sequenceMethod2)
					{
					case SequenceMethod.Single:
						return queryable.AsEnumerable<TElement>().Single<TElement>();
					case SequenceMethod.SinglePredicate:
						break;
					case SequenceMethod.SingleOrDefault:
						return queryable.AsEnumerable<TElement>().SingleOrDefault<TElement>();
					default:
						switch (sequenceMethod2)
						{
						case SequenceMethod.Count:
						case SequenceMethod.LongCount:
							return (TElement)((object)Convert.ChangeType(((DataServiceQuery<TElement>)queryable).GetQuerySetCount(this.Context), typeof(TElement), CultureInfo.InvariantCulture.NumberFormat));
						}
						break;
					}
					break;
				}
				throw Error.MethodNotSupported(methodCallExpression);
			}
			throw Error.MethodNotSupported(methodCallExpression);
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0001F458 File Offset: 0x0001D658
		internal QueryComponents Translate(Expression e)
		{
			bool addTrailingParens = false;
			Dictionary<Expression, Expression> dictionary = null;
			if (!(e is ResourceSetExpression))
			{
				dictionary = new Dictionary<Expression, Expression>(ReferenceEqualityComparer<Expression>.Instance);
				e = Evaluator.PartialEval(e);
				e = ExpressionNormalizer.Normalize(e, dictionary);
				e = ResourceBinder.Bind(e, this.Context);
				addTrailingParens = true;
			}
			Uri uri;
			Version version;
			UriWriter.Translate(this.Context, addTrailingParens, e, out uri, out version);
			ResourceExpression resourceExpression = e as ResourceExpression;
			Type lastSegmentType = (resourceExpression.Projection == null) ? resourceExpression.ResourceType : resourceExpression.Projection.Selector.Parameters[0].Type;
			LambdaExpression projection = (resourceExpression.Projection == null) ? null : resourceExpression.Projection.Selector;
			return new QueryComponents(uri, version, lastSegmentType, projection, dictionary);
		}

		// Token: 0x04000487 RID: 1159
		internal readonly DataServiceContext Context;
	}
}
