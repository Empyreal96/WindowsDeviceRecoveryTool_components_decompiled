using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Data.Edm;
using Microsoft.Data.OData;

namespace System.Data.Services.Client.Materialization
{
	// Token: 0x02000038 RID: 56
	internal abstract class ODataMaterializer : IDisposable
	{
		// Token: 0x060001A1 RID: 417 RVA: 0x000090F8 File Offset: 0x000072F8
		protected ODataMaterializer(IODataMaterializerContext materializerContext, Type expectedType)
		{
			this.ExpectedType = expectedType;
			this.MaterializerContext = materializerContext;
			this.nextLinkTable = new Dictionary<IEnumerable, DataServiceQueryContinuation>(ReferenceEqualityComparer<IEnumerable>.Instance);
			this.lazyPrimitivePropertyConverter = new SimpleLazy<PrimitivePropertyConverter>(() => new PrimitivePropertyConverter(this.Format));
			this.primitiveValueMaterializationPolicy = new PrimitiveValueMaterializationPolicy(this.MaterializerContext, this.lazyPrimitivePropertyConverter);
			this.collectionValueMaterializationPolicy = new CollectionValueMaterializationPolicy(this.MaterializerContext, this.primitiveValueMaterializationPolicy);
			this.complexValueMaterializerPolicy = new ComplexValueMaterializationPolicy(this.MaterializerContext, this.lazyPrimitivePropertyConverter);
			this.collectionValueMaterializationPolicy.ComplexValueMaterializationPolicy = this.complexValueMaterializerPolicy;
			this.complexValueMaterializerPolicy.CollectionValueMaterializationPolicy = this.collectionValueMaterializationPolicy;
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001A2 RID: 418
		internal abstract object CurrentValue { get; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001A3 RID: 419
		internal abstract ODataFeed CurrentFeed { get; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001A4 RID: 420
		internal abstract ODataEntry CurrentEntry { get; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x000091AE File Offset: 0x000073AE
		internal Dictionary<IEnumerable, DataServiceQueryContinuation> NextLinkTable
		{
			get
			{
				return this.nextLinkTable;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001A6 RID: 422
		internal abstract bool IsEndOfStream { get; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x000091B6 File Offset: 0x000073B6
		internal virtual bool IsCountable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001A8 RID: 424
		internal abstract long CountValue { get; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001A9 RID: 425
		internal abstract ProjectionPlan MaterializeEntryPlan { get; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001AA RID: 426 RVA: 0x000091B9 File Offset: 0x000073B9
		// (set) Token: 0x060001AB RID: 427 RVA: 0x000091C1 File Offset: 0x000073C1
		protected internal IODataMaterializerContext MaterializerContext { get; private set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001AC RID: 428
		protected abstract bool IsDisposed { get; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001AD RID: 429 RVA: 0x000091CA File Offset: 0x000073CA
		// (set) Token: 0x060001AE RID: 430 RVA: 0x000091D2 File Offset: 0x000073D2
		private protected Type ExpectedType { protected get; private set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001AF RID: 431 RVA: 0x000091DB File Offset: 0x000073DB
		protected CollectionValueMaterializationPolicy CollectionValueMaterializationPolicy
		{
			get
			{
				return this.collectionValueMaterializationPolicy;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x000091E3 File Offset: 0x000073E3
		protected ComplexValueMaterializationPolicy ComplexValueMaterializationPolicy
		{
			get
			{
				return this.complexValueMaterializerPolicy;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x000091EB File Offset: 0x000073EB
		protected PrimitivePropertyConverter PrimitivePropertyConverter
		{
			get
			{
				return this.lazyPrimitivePropertyConverter.Value;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x000091F8 File Offset: 0x000073F8
		protected PrimitiveValueMaterializationPolicy PrimitiveValueMaterializier
		{
			get
			{
				return this.primitiveValueMaterializationPolicy;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001B3 RID: 435
		protected abstract ODataFormat Format { get; }

		// Token: 0x060001B4 RID: 436 RVA: 0x00009200 File Offset: 0x00007400
		public static ODataMaterializer CreateMaterializerForMessage(IODataResponseMessage responseMessage, ResponseInfo responseInfo, Type materializerType, QueryComponents queryComponents, ProjectionPlan plan, ODataPayloadKind payloadKind)
		{
			ODataMessageReader odataMessageReader = ODataMaterializer.CreateODataMessageReader(responseMessage, responseInfo, ref payloadKind);
			IEdmType edmType = null;
			ODataMaterializer result;
			try
			{
				ODataMaterializerContext materializerContext = new ODataMaterializerContext(responseInfo);
				if (materializerType != typeof(object))
				{
					edmType = responseInfo.TypeResolver.ResolveExpectedTypeForReading(materializerType);
				}
				ODataMaterializer odataMaterializer;
				if (payloadKind != ODataPayloadKind.Entry && payloadKind != ODataPayloadKind.Feed)
				{
					switch (payloadKind)
					{
					case ODataPayloadKind.Property:
						if (edmType != null && edmType.TypeKind == EdmTypeKind.Entity)
						{
							throw Error.InvalidOperation(Strings.AtomMaterializer_InvalidEntityType(materializerType.FullName));
						}
						odataMaterializer = new ODataPropertyMaterializer(odataMessageReader, materializerContext, materializerType, queryComponents.SingleResult);
						goto IL_17F;
					case ODataPayloadKind.EntityReferenceLink:
					case ODataPayloadKind.EntityReferenceLinks:
						odataMaterializer = new ODataLinksMaterializer(odataMessageReader, materializerContext, materializerType, queryComponents.SingleResult);
						goto IL_17F;
					case ODataPayloadKind.Value:
						odataMaterializer = new ODataValueMaterializer(odataMessageReader, materializerContext, materializerType, queryComponents.SingleResult);
						goto IL_17F;
					case ODataPayloadKind.Collection:
						odataMaterializer = new ODataCollectionMaterializer(odataMessageReader, materializerContext, materializerType, queryComponents.SingleResult);
						goto IL_17F;
					case ODataPayloadKind.Error:
					{
						ODataError odataError = odataMessageReader.ReadError();
						throw new ODataErrorException(odataError.Message, odataError);
					}
					}
					throw Error.InvalidOperation(Strings.AtomMaterializer_InvalidResponsePayload(responseInfo.DataNamespace));
				}
				if (edmType != null && edmType.TypeKind != EdmTypeKind.Entity)
				{
					throw Error.InvalidOperation(Strings.AtomMaterializer_InvalidNonEntityType(materializerType.FullName));
				}
				ODataReaderWrapper reader = ODataReaderWrapper.Create(odataMessageReader, payloadKind, edmType, responseInfo.ResponsePipeline);
				EntityTrackingAdapter entityTrackingAdapter = new EntityTrackingAdapter(responseInfo.EntityTracker, responseInfo.MergeOption, responseInfo.Model, responseInfo.Context);
				LoadPropertyResponseInfo loadPropertyResponseInfo = responseInfo as LoadPropertyResponseInfo;
				if (loadPropertyResponseInfo != null)
				{
					odataMaterializer = new ODataLoadNavigationPropertyMaterializer(odataMessageReader, reader, materializerContext, entityTrackingAdapter, queryComponents, materializerType, plan, loadPropertyResponseInfo);
				}
				else
				{
					odataMaterializer = new ODataReaderEntityMaterializer(odataMessageReader, reader, materializerContext, entityTrackingAdapter, queryComponents, materializerType, plan);
				}
				IL_17F:
				result = odataMaterializer;
			}
			catch (Exception e)
			{
				if (CommonUtil.IsCatchableExceptionType(e))
				{
					odataMessageReader.Dispose();
				}
				throw;
			}
			return result;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x000093C4 File Offset: 0x000075C4
		public bool Read()
		{
			this.VerifyNotDisposed();
			return this.ReadImplementation();
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x000093D2 File Offset: 0x000075D2
		public void Dispose()
		{
			this.OnDispose();
		}

		// Token: 0x060001B7 RID: 439
		internal abstract void ClearLog();

		// Token: 0x060001B8 RID: 440
		internal abstract void ApplyLogToContext();

		// Token: 0x060001B9 RID: 441 RVA: 0x00009414 File Offset: 0x00007614
		protected static ODataMessageReader CreateODataMessageReader(IODataResponseMessage responseMessage, ResponseInfo responseInfo, ref ODataPayloadKind payloadKind)
		{
			ODataMessageReaderSettings settings = responseInfo.ReadHelper.CreateSettings(new Func<ODataEntry, XmlReader, Uri, XmlReader>(ReadingEntityInfo.BufferAndCacheEntryPayload));
			ODataMessageReader odataMessageReader = responseInfo.ReadHelper.CreateReader(responseMessage, settings);
			if (payloadKind == ODataPayloadKind.Unsupported)
			{
				List<ODataPayloadKindDetectionResult> list = odataMessageReader.DetectPayloadKind().ToList<ODataPayloadKindDetectionResult>();
				if (list.Count == 0)
				{
					throw Error.InvalidOperation(Strings.AtomMaterializer_InvalidResponsePayload(responseInfo.DataNamespace));
				}
				ODataPayloadKindDetectionResult odataPayloadKindDetectionResult = list.FirstOrDefault((ODataPayloadKindDetectionResult k) => k.PayloadKind == ODataPayloadKind.EntityReferenceLink || k.PayloadKind == ODataPayloadKind.EntityReferenceLinks);
				if (odataPayloadKindDetectionResult == null)
				{
					ODataVersion dataServiceVersion = responseMessage.GetDataServiceVersion(CommonUtil.ConvertToODataVersion(responseInfo.MaxProtocolVersion));
					if (dataServiceVersion < ODataVersion.V3)
					{
						if (list.Any((ODataPayloadKindDetectionResult pk) => pk.PayloadKind == ODataPayloadKind.Property))
						{
							if (list.Any((ODataPayloadKindDetectionResult pk) => pk.PayloadKind == ODataPayloadKind.Collection))
							{
								odataPayloadKindDetectionResult = list.Single((ODataPayloadKindDetectionResult pk) => pk.PayloadKind == ODataPayloadKind.Collection);
								goto IL_110;
							}
						}
					}
					odataPayloadKindDetectionResult = list.First<ODataPayloadKindDetectionResult>();
				}
				IL_110:
				if (odataPayloadKindDetectionResult.Format != ODataFormat.Atom && odataPayloadKindDetectionResult.Format != ODataFormat.VerboseJson && odataPayloadKindDetectionResult.Format != ODataFormat.Json && odataPayloadKindDetectionResult.Format != ODataFormat.RawValue)
				{
					throw Error.InvalidOperation(Strings.AtomMaterializer_InvalidContentTypeEncountered(responseMessage.GetHeader("Content-Type")));
				}
				payloadKind = odataPayloadKindDetectionResult.PayloadKind;
			}
			return odataMessageReader;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00009584 File Offset: 0x00007784
		protected void VerifyNotDisposed()
		{
			if (this.IsDisposed)
			{
				throw new ObjectDisposedException(typeof(ODataEntityMaterializer).FullName);
			}
		}

		// Token: 0x060001BB RID: 443
		protected abstract bool ReadImplementation();

		// Token: 0x060001BC RID: 444
		protected abstract void OnDispose();

		// Token: 0x04000200 RID: 512
		internal static readonly ODataNavigationLink[] EmptyLinks = new ODataNavigationLink[0];

		// Token: 0x04000201 RID: 513
		protected static readonly ODataProperty[] EmptyProperties = new ODataProperty[0];

		// Token: 0x04000202 RID: 514
		protected Dictionary<IEnumerable, DataServiceQueryContinuation> nextLinkTable;

		// Token: 0x04000203 RID: 515
		private readonly CollectionValueMaterializationPolicy collectionValueMaterializationPolicy;

		// Token: 0x04000204 RID: 516
		private readonly ComplexValueMaterializationPolicy complexValueMaterializerPolicy;

		// Token: 0x04000205 RID: 517
		private readonly PrimitiveValueMaterializationPolicy primitiveValueMaterializationPolicy;

		// Token: 0x04000206 RID: 518
		private SimpleLazy<PrimitivePropertyConverter> lazyPrimitivePropertyConverter;
	}
}
