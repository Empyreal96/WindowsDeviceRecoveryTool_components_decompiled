using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x02000134 RID: 308
	public static class TableQueryableExtensions
	{
		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06001423 RID: 5155 RVA: 0x0004CDE1 File Offset: 0x0004AFE1
		// (set) Token: 0x06001424 RID: 5156 RVA: 0x0004CDE8 File Offset: 0x0004AFE8
		internal static MethodInfo WithOptionsMethodInfo { get; set; }

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06001425 RID: 5157 RVA: 0x0004CDF0 File Offset: 0x0004AFF0
		// (set) Token: 0x06001426 RID: 5158 RVA: 0x0004CDF7 File Offset: 0x0004AFF7
		internal static MethodInfo WithContextMethodInfo { get; set; }

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06001427 RID: 5159 RVA: 0x0004CDFF File Offset: 0x0004AFFF
		// (set) Token: 0x06001428 RID: 5160 RVA: 0x0004CE06 File Offset: 0x0004B006
		internal static MethodInfo ResolveMethodInfo { get; set; }

		// Token: 0x06001429 RID: 5161 RVA: 0x0004CE44 File Offset: 0x0004B044
		static TableQueryableExtensions()
		{
			Type typeFromHandle = typeof(TableQueryableExtensions);
			MethodInfo[] methods = typeFromHandle.GetMethods(BindingFlags.Static | BindingFlags.Public);
			TableQueryableExtensions.WithOptionsMethodInfo = (from m in methods
			where m.Name == "WithOptions"
			select m).FirstOrDefault<MethodInfo>();
			TableQueryableExtensions.WithContextMethodInfo = (from m in methods
			where m.Name == "WithContext"
			select m).FirstOrDefault<MethodInfo>();
			TableQueryableExtensions.ResolveMethodInfo = (from m in methods
			where m.Name == "Resolve"
			select m).FirstOrDefault<MethodInfo>();
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x0004CEEC File Offset: 0x0004B0EC
		public static TableQuery<TElement> WithOptions<TElement>(this IQueryable<TElement> query, TableRequestOptions options)
		{
			CommonUtility.AssertNotNull("options", options);
			if (!(query is TableQuery<TElement>))
			{
				throw new NotSupportedException("Query must be a TableQuery<T>");
			}
			return (TableQuery<TElement>)query.Provider.CreateQuery<TElement>(Expression.Call(null, TableQueryableExtensions.WithOptionsMethodInfo.MakeGenericMethod(new Type[]
			{
				typeof(TElement)
			}), new Expression[]
			{
				query.Expression,
				Expression.Constant(options, typeof(TableRequestOptions))
			}));
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x0004CF70 File Offset: 0x0004B170
		public static TableQuery<TElement> WithContext<TElement>(this IQueryable<TElement> query, OperationContext operationContext)
		{
			CommonUtility.AssertNotNull("operationContext", operationContext);
			if (!(query is TableQuery<TElement>))
			{
				throw new NotSupportedException("Query must be a TableQuery<T>");
			}
			return (TableQuery<TElement>)query.Provider.CreateQuery<TElement>(Expression.Call(null, TableQueryableExtensions.WithContextMethodInfo.MakeGenericMethod(new Type[]
			{
				typeof(TElement)
			}), new Expression[]
			{
				query.Expression,
				Expression.Constant(operationContext, typeof(OperationContext))
			}));
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x0004CFF4 File Offset: 0x0004B1F4
		public static TableQuery<TResolved> Resolve<TElement, TResolved>(this IQueryable<TElement> query, EntityResolver<TResolved> resolver)
		{
			CommonUtility.AssertNotNull("resolver", resolver);
			if (!(query is TableQuery<TElement>))
			{
				throw new NotSupportedException("Query must be a TableQuery<T>");
			}
			return (TableQuery<TResolved>)query.Provider.CreateQuery<TResolved>(Expression.Call(null, TableQueryableExtensions.ResolveMethodInfo.MakeGenericMethod(new Type[]
			{
				typeof(TElement),
				typeof(TResolved)
			}), new Expression[]
			{
				query.Expression,
				Expression.Constant(resolver, typeof(EntityResolver<TResolved>))
			}));
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x0004D088 File Offset: 0x0004B288
		public static TableQuery<TElement> AsTableQuery<TElement>(this IQueryable<TElement> query)
		{
			TableQuery<TElement> tableQuery = query as TableQuery<TElement>;
			if (tableQuery == null)
			{
				throw new NotSupportedException("Query must be a TableQuery<T>");
			}
			return tableQuery;
		}
	}
}
