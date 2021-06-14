using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Services.Client.Materialization;
using System.Data.Services.Client.Metadata;
using System.IO;
using System.Xml;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x02000112 RID: 274
	internal class MaterializeAtom : IDisposable, IEnumerable, IEnumerator
	{
		// Token: 0x060008E2 RID: 2274 RVA: 0x0002484C File Offset: 0x00022A4C
		internal MaterializeAtom(ResponseInfo responseInfo, QueryComponents queryComponents, ProjectionPlan plan, IODataResponseMessage responseMessage, ODataPayloadKind payloadKind)
		{
			this.responseInfo = responseInfo;
			this.elementType = queryComponents.LastSegmentType;
			this.expectingPrimitiveValue = PrimitiveType.IsKnownNullableType(this.elementType);
			Type type;
			Type typeForMaterializer = MaterializeAtom.GetTypeForMaterializer(this.expectingPrimitiveValue, this.elementType, responseInfo.Model, out type);
			this.materializer = ODataMaterializer.CreateMaterializerForMessage(responseMessage, responseInfo, typeForMaterializer, queryComponents, plan, payloadKind);
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x000248B0 File Offset: 0x00022AB0
		internal MaterializeAtom(ResponseInfo responseInfo, IEnumerable<ODataEntry> entries, Type elementType, ODataFormat format)
		{
			this.responseInfo = responseInfo;
			this.elementType = elementType;
			this.expectingPrimitiveValue = PrimitiveType.IsKnownNullableType(elementType);
			Type type;
			Type typeForMaterializer = MaterializeAtom.GetTypeForMaterializer(this.expectingPrimitiveValue, this.elementType, responseInfo.Model, out type);
			QueryComponents queryComponents = new QueryComponents(null, Util.DataServiceVersionEmpty, elementType, null, null);
			ODataMaterializerContext materializerContext = new ODataMaterializerContext(responseInfo);
			EntityTrackingAdapter entityTrackingAdapter = new EntityTrackingAdapter(responseInfo.EntityTracker, responseInfo.MergeOption, responseInfo.Model, responseInfo.Context);
			this.materializer = new ODataEntriesEntityMaterializer(entries, materializerContext, entityTrackingAdapter, queryComponents, typeForMaterializer, null, format);
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x00024940 File Offset: 0x00022B40
		private MaterializeAtom()
		{
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x060008E5 RID: 2277 RVA: 0x00024948 File Offset: 0x00022B48
		public object Current
		{
			get
			{
				return this.current;
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x060008E6 RID: 2278 RVA: 0x0002495D File Offset: 0x00022B5D
		internal static MaterializeAtom EmptyResults
		{
			get
			{
				return new MaterializeAtom.ResultsWrapper(null, null, null);
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x060008E7 RID: 2279 RVA: 0x00024967 File Offset: 0x00022B67
		internal bool IsCountable
		{
			get
			{
				return this.materializer != null && this.materializer.IsCountable;
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x060008E8 RID: 2280 RVA: 0x0002497E File Offset: 0x00022B7E
		internal virtual DataServiceContext Context
		{
			get
			{
				return this.responseInfo.Context;
			}
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0002498B File Offset: 0x00022B8B
		public void Dispose()
		{
			this.current = null;
			if (this.materializer != null)
			{
				this.materializer.Dispose();
			}
			if (this.writer != null)
			{
				this.writer.Dispose();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x000249C0 File Offset: 0x00022BC0
		public virtual IEnumerator GetEnumerator()
		{
			this.CheckGetEnumerator();
			return this;
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x000249CC File Offset: 0x00022BCC
		private static Type GetTypeForMaterializer(bool expectingPrimitiveValue, Type elementType, ClientEdmModel model, out Type implementationType)
		{
			if (!expectingPrimitiveValue && typeof(IEnumerable).IsAssignableFrom(elementType))
			{
				implementationType = ClientTypeUtil.GetImplementationType(elementType, typeof(ICollection<>));
				if (implementationType != null)
				{
					Type type = implementationType.GetGenericArguments()[0];
					if (ClientTypeUtil.TypeIsEntity(type, model))
					{
						return type;
					}
				}
			}
			implementationType = null;
			return elementType;
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x00024A24 File Offset: 0x00022C24
		public bool MoveNext()
		{
			bool applyingChanges = this.responseInfo.ApplyingChanges;
			bool result;
			try
			{
				this.responseInfo.ApplyingChanges = true;
				result = this.MoveNextInternal();
			}
			finally
			{
				this.responseInfo.ApplyingChanges = applyingChanges;
			}
			return result;
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x00024A70 File Offset: 0x00022C70
		private bool MoveNextInternal()
		{
			if (this.materializer == null)
			{
				return false;
			}
			this.current = null;
			this.materializer.ClearLog();
			bool flag = false;
			Type type;
			MaterializeAtom.GetTypeForMaterializer(this.expectingPrimitiveValue, this.elementType, this.responseInfo.Model, out type);
			if (type != null)
			{
				if (this.moved)
				{
					return false;
				}
				Type type2 = type.GetGenericArguments()[0];
				type = this.elementType;
				if (type.IsInterface())
				{
					type = typeof(Collection<>).MakeGenericType(new Type[]
					{
						type2
					});
				}
				IList list = (IList)Activator.CreateInstance(type);
				while (this.materializer.Read())
				{
					list.Add(this.materializer.CurrentValue);
				}
				this.moved = true;
				this.current = list;
				flag = true;
			}
			if (this.current == null)
			{
				if (this.expectingPrimitiveValue && this.moved)
				{
					flag = false;
				}
				else
				{
					flag = this.materializer.Read();
					if (flag)
					{
						this.current = this.materializer.CurrentValue;
					}
					this.moved = true;
				}
			}
			this.materializer.ApplyLogToContext();
			return flag;
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x00024B90 File Offset: 0x00022D90
		void IEnumerator.Reset()
		{
			throw Error.NotSupported();
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x00024B97 File Offset: 0x00022D97
		internal static MaterializeAtom CreateWrapper(DataServiceContext context, IEnumerable results)
		{
			return new MaterializeAtom.ResultsWrapper(context, results, null);
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x00024BA1 File Offset: 0x00022DA1
		internal static MaterializeAtom CreateWrapper(DataServiceContext context, IEnumerable results, DataServiceQueryContinuation continuation)
		{
			return new MaterializeAtom.ResultsWrapper(context, results, continuation);
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x00024BAB File Offset: 0x00022DAB
		internal void SetInsertingObject(object addedObject)
		{
			((ODataEntityMaterializer)this.materializer).TargetInstance = addedObject;
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x00024BBE File Offset: 0x00022DBE
		internal long CountValue()
		{
			return this.materializer.CountValue;
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x00024BCC File Offset: 0x00022DCC
		internal virtual DataServiceQueryContinuation GetContinuation(IEnumerable key)
		{
			DataServiceQueryContinuation result;
			if (key == null)
			{
				if ((this.expectingPrimitiveValue && !this.moved) || (!this.expectingPrimitiveValue && !this.materializer.IsEndOfStream))
				{
					throw new InvalidOperationException(Strings.MaterializeFromAtom_TopLevelLinkNotAvailable);
				}
				if (this.expectingPrimitiveValue || this.materializer.CurrentFeed == null)
				{
					result = null;
				}
				else
				{
					result = DataServiceQueryContinuation.Create(this.materializer.CurrentFeed.NextPageLink, this.materializer.MaterializeEntryPlan);
				}
			}
			else if (!this.materializer.NextLinkTable.TryGetValue(key, out result))
			{
				throw new ArgumentException(Strings.MaterializeFromAtom_CollectionKeyNotPresentInLinkTable);
			}
			return result;
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x00024C69 File Offset: 0x00022E69
		private void CheckGetEnumerator()
		{
			if (this.calledGetEnumerator)
			{
				throw Error.NotSupported(Strings.Deserialize_GetEnumerator);
			}
			this.calledGetEnumerator = true;
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x00024C88 File Offset: 0x00022E88
		internal static string ReadElementString(XmlReader reader, bool checkNullAttribute)
		{
			string text = null;
			bool flag = checkNullAttribute && !Util.DoesNullAttributeSayTrue(reader);
			if (!reader.IsEmptyElement)
			{
				while (reader.Read())
				{
					XmlNodeType nodeType = reader.NodeType;
					switch (nodeType)
					{
					case XmlNodeType.Element:
					case XmlNodeType.Attribute:
						goto IL_86;
					case XmlNodeType.Text:
					case XmlNodeType.CDATA:
						break;
					default:
						if (nodeType == XmlNodeType.Comment)
						{
							continue;
						}
						switch (nodeType)
						{
						case XmlNodeType.Whitespace:
							continue;
						case XmlNodeType.SignificantWhitespace:
							break;
						case XmlNodeType.EndElement:
						{
							string result;
							if ((result = text) == null)
							{
								if (!flag)
								{
									return null;
								}
								result = string.Empty;
							}
							return result;
						}
						default:
							goto IL_86;
						}
						break;
					}
					if (text != null)
					{
						throw Error.InvalidOperation(Strings.Deserialize_MixedTextWithComment);
					}
					text = reader.Value;
					continue;
					IL_86:
					throw Error.InvalidOperation(Strings.Deserialize_ExpectingSimpleValue);
				}
				throw Error.InvalidOperation(Strings.Deserialize_ExpectingSimpleValue);
			}
			if (!flag)
			{
				return null;
			}
			return string.Empty;
		}

		// Token: 0x04000542 RID: 1346
		private readonly ResponseInfo responseInfo;

		// Token: 0x04000543 RID: 1347
		private readonly Type elementType;

		// Token: 0x04000544 RID: 1348
		private readonly bool expectingPrimitiveValue;

		// Token: 0x04000545 RID: 1349
		private readonly ODataMaterializer materializer;

		// Token: 0x04000546 RID: 1350
		private object current;

		// Token: 0x04000547 RID: 1351
		private bool calledGetEnumerator;

		// Token: 0x04000548 RID: 1352
		private bool moved;

		// Token: 0x04000549 RID: 1353
		private TextWriter writer;

		// Token: 0x02000113 RID: 275
		private class ResultsWrapper : MaterializeAtom
		{
			// Token: 0x060008F6 RID: 2294 RVA: 0x00024D38 File Offset: 0x00022F38
			internal ResultsWrapper(DataServiceContext context, IEnumerable results, DataServiceQueryContinuation continuation)
			{
				this.context = context;
				this.results = (results ?? new object[0]);
				this.continuation = continuation;
			}

			// Token: 0x17000211 RID: 529
			// (get) Token: 0x060008F7 RID: 2295 RVA: 0x00024D5F File Offset: 0x00022F5F
			internal override DataServiceContext Context
			{
				get
				{
					return this.context;
				}
			}

			// Token: 0x060008F8 RID: 2296 RVA: 0x00024D67 File Offset: 0x00022F67
			internal override DataServiceQueryContinuation GetContinuation(IEnumerable key)
			{
				if (key == null)
				{
					return this.continuation;
				}
				throw new InvalidOperationException(Strings.MaterializeFromAtom_GetNestLinkForFlatCollection);
			}

			// Token: 0x060008F9 RID: 2297 RVA: 0x00024D7D File Offset: 0x00022F7D
			public override IEnumerator GetEnumerator()
			{
				return this.results.GetEnumerator();
			}

			// Token: 0x0400054A RID: 1354
			private readonly IEnumerable results;

			// Token: 0x0400054B RID: 1355
			private readonly DataServiceQueryContinuation continuation;

			// Token: 0x0400054C RID: 1356
			private readonly DataServiceContext context;
		}
	}
}
