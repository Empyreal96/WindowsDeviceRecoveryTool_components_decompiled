using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Data.Services.Client
{
	// Token: 0x0200010D RID: 269
	public class QueryOperationResponse : OperationResponse, IEnumerable
	{
		// Token: 0x060008BE RID: 2238 RVA: 0x0002454A File Offset: 0x0002274A
		internal QueryOperationResponse(HeaderCollection headers, DataServiceRequest query, MaterializeAtom results) : base(headers)
		{
			this.query = query;
			this.results = results;
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x060008BF RID: 2239 RVA: 0x00024561 File Offset: 0x00022761
		public DataServiceRequest Query
		{
			get
			{
				return this.query;
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x060008C0 RID: 2240 RVA: 0x00024569 File Offset: 0x00022769
		public virtual long TotalCount
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x060008C1 RID: 2241 RVA: 0x00024570 File Offset: 0x00022770
		internal MaterializeAtom Results
		{
			get
			{
				if (base.Error != null)
				{
					throw System.Data.Services.Client.Error.InvalidOperation(Strings.Context_BatchExecuteError, base.Error);
				}
				return this.results;
			}
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x0002459E File Offset: 0x0002279E
		public IEnumerator GetEnumerator()
		{
			return this.GetEnumeratorHelper<IEnumerator>(() => this.Results.GetEnumerator());
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x000245B2 File Offset: 0x000227B2
		public DataServiceQueryContinuation GetContinuation()
		{
			return this.results.GetContinuation(null);
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x000245C0 File Offset: 0x000227C0
		public DataServiceQueryContinuation GetContinuation(IEnumerable collection)
		{
			return this.results.GetContinuation(collection);
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x000245CE File Offset: 0x000227CE
		public DataServiceQueryContinuation<T> GetContinuation<T>(IEnumerable<T> collection)
		{
			return (DataServiceQueryContinuation<T>)this.results.GetContinuation(collection);
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x000245E4 File Offset: 0x000227E4
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		internal static QueryOperationResponse GetInstance(Type elementType, HeaderCollection headers, DataServiceRequest query, MaterializeAtom results)
		{
			Type type = typeof(QueryOperationResponse<>).MakeGenericType(new Type[]
			{
				elementType
			});
			return (QueryOperationResponse)Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, new object[]
			{
				headers,
				query,
				results
			}, CultureInfo.InvariantCulture);
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x00024638 File Offset: 0x00022838
		protected T GetEnumeratorHelper<T>(Func<T> getEnumerator) where T : IEnumerator
		{
			if (getEnumerator == null)
			{
				throw new ArgumentNullException("getEnumerator");
			}
			if (this.Results.Context != null)
			{
				bool? singleResult = this.Query.QueryComponents(this.Results.Context.Model).SingleResult;
				if (singleResult != null && !singleResult.Value)
				{
					IEnumerator enumerator = this.Results.GetEnumerator();
					if (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						ICollection collection = obj as ICollection;
						if (collection == null)
						{
							throw new DataServiceClientException(Strings.AtomMaterializer_CollectionExpectedCollection(obj.GetType().ToString()));
						}
						return (T)((object)collection.GetEnumerator());
					}
				}
			}
			return getEnumerator();
		}

		// Token: 0x04000513 RID: 1299
		private readonly DataServiceRequest query;

		// Token: 0x04000514 RID: 1300
		private readonly MaterializeAtom results;
	}
}
