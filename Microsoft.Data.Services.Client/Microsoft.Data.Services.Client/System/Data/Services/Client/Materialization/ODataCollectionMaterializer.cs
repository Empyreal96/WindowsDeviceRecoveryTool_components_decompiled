using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Services.Client.Metadata;
using Microsoft.Data.Edm;
using Microsoft.Data.OData;

namespace System.Data.Services.Client.Materialization
{
	// Token: 0x0200006B RID: 107
	internal sealed class ODataCollectionMaterializer : ODataMessageReaderMaterializer
	{
		// Token: 0x06000399 RID: 921 RVA: 0x0000FD28 File Offset: 0x0000DF28
		public ODataCollectionMaterializer(ODataMessageReader reader, IODataMaterializerContext materializerContext, Type expectedType, bool? singleResult) : base(reader, materializerContext, expectedType, singleResult)
		{
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600039A RID: 922 RVA: 0x0000FD35 File Offset: 0x0000DF35
		internal override object CurrentValue
		{
			get
			{
				return this.currentValue;
			}
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000FD40 File Offset: 0x0000DF40
		protected override void ReadWithExpectedType(IEdmTypeReference expectedClientType, IEdmTypeReference expectedReaderType)
		{
			if (!expectedClientType.IsCollection())
			{
				throw new DataServiceClientException(Strings.AtomMaterializer_TypeShouldBeCollectionError(expectedClientType.FullName()));
			}
			Type type = Nullable.GetUnderlyingType(base.ExpectedType) ?? base.ExpectedType;
			WebUtil.IsCLRTypeCollection(type, base.MaterializerContext.Model);
			Type type2 = type;
			Type type3 = ClientTypeUtil.GetImplementationType(type, typeof(ICollection<>));
			if (type3 != null)
			{
				type2 = type3.GetGenericArguments()[0];
			}
			else
			{
				type3 = typeof(ICollection<>).MakeGenericType(new Type[]
				{
					type2
				});
			}
			Type backingTypeForCollectionProperty = WebUtil.GetBackingTypeForCollectionProperty(type3, type2);
			object collectionInstance = base.CollectionValueMaterializationPolicy.CreateCollectionInstance((IEdmCollectionTypeReference)expectedClientType, backingTypeForCollectionProperty);
			ODataCollectionReader collectionReader = this.messageReader.CreateODataCollectionReader();
			ODataCollectionMaterializer.NonEntityItemsEnumerable items = new ODataCollectionMaterializer.NonEntityItemsEnumerable(collectionReader);
			base.CollectionValueMaterializationPolicy.ApplyCollectionDataValues(items, null, collectionInstance, type2, ClientTypeUtil.GetAddToCollectionDelegate(type3));
			this.currentValue = collectionInstance;
		}

		// Token: 0x040002AE RID: 686
		private object currentValue;

		// Token: 0x0200006C RID: 108
		private class NonEntityItemsEnumerable : IEnumerable, IEnumerator
		{
			// Token: 0x0600039C RID: 924 RVA: 0x0000FE25 File Offset: 0x0000E025
			internal NonEntityItemsEnumerable(ODataCollectionReader collectionReader)
			{
				this.collectionReader = collectionReader;
			}

			// Token: 0x170000ED RID: 237
			// (get) Token: 0x0600039D RID: 925 RVA: 0x0000FE34 File Offset: 0x0000E034
			public object Current
			{
				get
				{
					return this.collectionReader.Item;
				}
			}

			// Token: 0x0600039E RID: 926 RVA: 0x0000FE41 File Offset: 0x0000E041
			public IEnumerator GetEnumerator()
			{
				return this;
			}

			// Token: 0x0600039F RID: 927 RVA: 0x0000FE44 File Offset: 0x0000E044
			public bool MoveNext()
			{
				bool flag;
				for (flag = this.collectionReader.Read(); flag && this.collectionReader.State != ODataCollectionReaderState.Value; flag = this.collectionReader.Read())
				{
				}
				return flag;
			}

			// Token: 0x060003A0 RID: 928 RVA: 0x0000FE7D File Offset: 0x0000E07D
			public void Reset()
			{
				throw new InvalidOperationException(Strings.AtomMaterializer_ResetAfterEnumeratorCreationError);
			}

			// Token: 0x040002AF RID: 687
			private readonly ODataCollectionReader collectionReader;
		}
	}
}
