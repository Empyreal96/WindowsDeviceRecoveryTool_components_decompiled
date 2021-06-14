using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Services.Client
{
	// Token: 0x0200012D RID: 301
	public class DataServiceQuery<TElement> : DataServiceQuery, IQueryable<TElement>, IEnumerable<!0>, IQueryable, IEnumerable
	{
		// Token: 0x06000AB0 RID: 2736 RVA: 0x0002AB49 File Offset: 0x00028D49
		private DataServiceQuery(Expression expression, DataServiceQueryProvider provider)
		{
			this.queryExpression = expression;
			this.queryProvider = provider;
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000AB1 RID: 2737 RVA: 0x0002AB5F File Offset: 0x00028D5F
		public override Type ElementType
		{
			get
			{
				return typeof(TElement);
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x0002AB6B File Offset: 0x00028D6B
		public override Expression Expression
		{
			get
			{
				return this.queryExpression;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x0002AB73 File Offset: 0x00028D73
		public override IQueryProvider Provider
		{
			get
			{
				return this.queryProvider;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x0002AB7B File Offset: 0x00028D7B
		// (set) Token: 0x06000AB5 RID: 2741 RVA: 0x0002AB88 File Offset: 0x00028D88
		public override Uri RequestUri
		{
			get
			{
				return this.Translate().Uri;
			}
			internal set
			{
				this.Translate().Uri = value;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x0002AB96 File Offset: 0x00028D96
		internal override ProjectionPlan Plan
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x0002AB99 File Offset: 0x00028D99
		private DataServiceContext Context
		{
			get
			{
				return this.queryProvider.Context;
			}
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x0002ABA6 File Offset: 0x00028DA6
		public new IAsyncResult BeginExecute(AsyncCallback callback, object state)
		{
			return base.BeginExecute(this, this.Context, callback, state, "Execute");
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x0002ABBC File Offset: 0x00028DBC
		public new IEnumerable<TElement> EndExecute(IAsyncResult asyncResult)
		{
			return DataServiceRequest.EndExecute<TElement>(this, this.Context, "Execute", asyncResult);
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x0002ABD0 File Offset: 0x00028DD0
		public new IEnumerable<TElement> Execute()
		{
			return base.Execute<TElement>(this.Context, this.Translate());
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x0002ABE4 File Offset: 0x00028DE4
		public DataServiceQuery<TElement> Expand(string path)
		{
			Util.CheckArgumentNullAndEmpty(path, "path");
			return (DataServiceQuery<TElement>)this.Provider.CreateQuery<TElement>(Expression.Call(Expression.Convert(this.Expression, typeof(DataServiceQuery<TElement>.DataServiceOrderedQuery)), DataServiceQuery<TElement>.expandMethodInfo, new Expression[]
			{
				Expression.Constant(path)
			}));
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x0002AC3C File Offset: 0x00028E3C
		public DataServiceQuery<TElement> Expand<TTarget>(Expression<Func<TElement, TTarget>> navigationPropertyAccessor)
		{
			Util.CheckArgumentNull<Expression<Func<TElement, TTarget>>>(navigationPropertyAccessor, "navigationPropertyAccessor");
			MethodInfo method = DataServiceQuery<TElement>.expandGenericMethodInfo.MakeGenericMethod(new Type[]
			{
				typeof(TTarget)
			});
			return (DataServiceQuery<TElement>)this.Provider.CreateQuery<TElement>(Expression.Call(Expression.Convert(this.Expression, typeof(DataServiceQuery<TElement>.DataServiceOrderedQuery)), method, new Expression[]
			{
				navigationPropertyAccessor
			}));
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x0002ACAC File Offset: 0x00028EAC
		public DataServiceQuery<TElement> IncludeTotalCount()
		{
			MethodInfo method = typeof(DataServiceQuery<TElement>).GetMethod("IncludeTotalCount");
			return (DataServiceQuery<TElement>)this.Provider.CreateQuery<TElement>(Expression.Call(Expression.Convert(this.Expression, typeof(DataServiceQuery<TElement>.DataServiceOrderedQuery)), method));
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x0002ACFC File Offset: 0x00028EFC
		public DataServiceQuery<TElement> AddQueryOption(string name, object value)
		{
			Util.CheckArgumentNull<string>(name, "name");
			Util.CheckArgumentNull<object>(value, "value");
			MethodInfo method = typeof(DataServiceQuery<TElement>).GetMethod("AddQueryOption");
			return (DataServiceQuery<TElement>)this.Provider.CreateQuery<TElement>(Expression.Call(Expression.Convert(this.Expression, typeof(DataServiceQuery<TElement>.DataServiceOrderedQuery)), method, new Expression[]
			{
				Expression.Constant(name),
				Expression.Constant(value, typeof(object))
			}));
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x0002AD85 File Offset: 0x00028F85
		public IEnumerator<TElement> GetEnumerator()
		{
			return this.Execute().GetEnumerator();
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x0002AD94 File Offset: 0x00028F94
		public override string ToString()
		{
			string result;
			try
			{
				result = this.QueryComponents(this.Context.Model).Uri.ToString();
			}
			catch (NotSupportedException ex)
			{
				result = Strings.ALinq_TranslationError(ex.Message);
			}
			return result;
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x0002ADE0 File Offset: 0x00028FE0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x0002ADE8 File Offset: 0x00028FE8
		internal override QueryComponents QueryComponents(ClientEdmModel model)
		{
			return this.Translate();
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x0002ADF0 File Offset: 0x00028FF0
		internal override IEnumerable ExecuteInternal()
		{
			return this.Execute();
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x0002ADF8 File Offset: 0x00028FF8
		internal override IAsyncResult BeginExecuteInternal(AsyncCallback callback, object state)
		{
			return this.BeginExecute(callback, state);
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x0002AE02 File Offset: 0x00029002
		internal override IEnumerable EndExecuteInternal(IAsyncResult asyncResult)
		{
			return this.EndExecute(asyncResult);
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x0002AE0B File Offset: 0x0002900B
		private QueryComponents Translate()
		{
			if (this.queryComponents == null)
			{
				this.queryComponents = this.queryProvider.Translate(this.queryExpression);
			}
			return this.queryComponents;
		}

		// Token: 0x040005DD RID: 1501
		private static readonly MethodInfo expandMethodInfo = typeof(DataServiceQuery<TElement>).GetMethod("Expand", new Type[]
		{
			typeof(string)
		});

		// Token: 0x040005DE RID: 1502
		private static readonly MethodInfo expandGenericMethodInfo = (MethodInfo)typeof(DataServiceQuery<TElement>).GetMember("Expand*").Single((MemberInfo m) => ((MethodInfo)m).GetGenericArguments().Count<Type>() == 1);

		// Token: 0x040005DF RID: 1503
		private readonly Expression queryExpression;

		// Token: 0x040005E0 RID: 1504
		private readonly DataServiceQueryProvider queryProvider;

		// Token: 0x040005E1 RID: 1505
		private QueryComponents queryComponents;

		// Token: 0x0200012E RID: 302
		internal class DataServiceOrderedQuery : DataServiceQuery<TElement>, IOrderedQueryable<TElement>, IQueryable<TElement>, IEnumerable<!0>, IOrderedQueryable, IQueryable, IEnumerable
		{
			// Token: 0x06000AC9 RID: 2761 RVA: 0x0002AEC3 File Offset: 0x000290C3
			internal DataServiceOrderedQuery(Expression expression, DataServiceQueryProvider provider) : base(expression, provider)
			{
			}
		}
	}
}
