using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Table.Queryable;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x0200014A RID: 330
	internal class TableQueryProvider : IQueryProvider
	{
		// Token: 0x17000347 RID: 839
		// (get) Token: 0x060014BF RID: 5311 RVA: 0x0004F4E1 File Offset: 0x0004D6E1
		// (set) Token: 0x060014C0 RID: 5312 RVA: 0x0004F4E9 File Offset: 0x0004D6E9
		internal CloudTable Table { get; private set; }

		// Token: 0x060014C1 RID: 5313 RVA: 0x0004F4F2 File Offset: 0x0004D6F2
		public TableQueryProvider(CloudTable table)
		{
			this.Table = table;
		}

		// Token: 0x060014C2 RID: 5314 RVA: 0x0004F501 File Offset: 0x0004D701
		public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
		{
			CommonUtility.AssertNotNull("expression", expression);
			return new TableQuery<TElement>(expression, this);
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x0004F518 File Offset: 0x0004D718
		public IQueryable CreateQuery(Expression expression)
		{
			CommonUtility.AssertNotNull("expression", expression);
			Type elementType = TypeSystem.GetElementType(expression.Type);
			Type type = typeof(TableQuery<>).MakeGenericType(new Type[]
			{
				elementType
			});
			object[] arguments = new object[]
			{
				expression,
				this
			};
			ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[]
			{
				typeof(Expression),
				typeof(TableQueryProvider)
			}, null);
			return (IQueryable)TableQueryProvider.ConstructorInvoke(constructor, arguments);
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x0004F5B0 File Offset: 0x0004D7B0
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public object Execute(Expression expression)
		{
			CommonUtility.AssertNotNull("expression", expression);
			return ReflectionUtil.TableQueryProviderReturnSingletonMethodInfo.MakeGenericMethod(new Type[]
			{
				expression.Type
			}).Invoke(this, new object[]
			{
				expression
			});
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x0004F5F8 File Offset: 0x0004D7F8
		public TResult Execute<TResult>(Expression expression)
		{
			CommonUtility.AssertNotNull("expression", expression);
			return (TResult)((object)ReflectionUtil.TableQueryProviderReturnSingletonMethodInfo.MakeGenericMethod(new Type[]
			{
				typeof(TResult)
			}).Invoke(this, new object[]
			{
				expression
			}));
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x0004F648 File Offset: 0x0004D848
		internal TElement ReturnSingleton<TElement>(Expression expression)
		{
			IQueryable<TElement> source = new TableQuery<TElement>(expression, this);
			MethodCallExpression methodCallExpression = expression as MethodCallExpression;
			SequenceMethod sequenceMethod;
			if (ReflectionUtil.TryIdentifySequenceMethod(methodCallExpression.Method, out sequenceMethod))
			{
				SequenceMethod sequenceMethod2 = sequenceMethod;
				switch (sequenceMethod2)
				{
				case SequenceMethod.First:
					return source.AsEnumerable<TElement>().First<TElement>();
				case SequenceMethod.FirstPredicate:
					break;
				case SequenceMethod.FirstOrDefault:
					return source.AsEnumerable<TElement>().FirstOrDefault<TElement>();
				default:
					switch (sequenceMethod2)
					{
					case SequenceMethod.Single:
						return source.AsEnumerable<TElement>().Single<TElement>();
					case SequenceMethod.SingleOrDefault:
						return source.AsEnumerable<TElement>().SingleOrDefault<TElement>();
					}
					break;
				}
			}
			throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "The method '{0}' is not supported.", new object[]
			{
				methodCallExpression.Method.Name
			}));
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x0004F6FE File Offset: 0x0004D8FE
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		internal static object ConstructorInvoke(ConstructorInfo constructor, object[] arguments)
		{
			if (constructor == null)
			{
				throw new MissingMethodException();
			}
			return constructor.Invoke(arguments);
		}
	}
}
