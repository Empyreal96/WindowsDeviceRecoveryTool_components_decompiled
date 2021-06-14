using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData
{
	// Token: 0x02000191 RID: 401
	internal abstract class ODataInputContext : IDisposable
	{
		// Token: 0x06000B8F RID: 2959 RVA: 0x00028A14 File Offset: 0x00026C14
		protected ODataInputContext(ODataFormat format, ODataMessageReaderSettings messageReaderSettings, ODataVersion version, bool readingResponse, bool synchronous, IEdmModel model, IODataUrlResolver urlResolver)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataFormat>(format, "format");
			ExceptionUtils.CheckArgumentNotNull<ODataMessageReaderSettings>(messageReaderSettings, "messageReaderSettings");
			this.format = format;
			this.messageReaderSettings = messageReaderSettings;
			this.version = version;
			this.readingResponse = readingResponse;
			this.synchronous = synchronous;
			this.model = model;
			this.urlResolver = urlResolver;
			this.edmTypeResolver = new EdmTypeReaderResolver(this.Model, this.MessageReaderSettings.ReaderBehavior, this.Version);
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000B90 RID: 2960 RVA: 0x00028A94 File Offset: 0x00026C94
		internal ODataMessageReaderSettings MessageReaderSettings
		{
			get
			{
				return this.messageReaderSettings;
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000B91 RID: 2961 RVA: 0x00028A9C File Offset: 0x00026C9C
		internal ODataVersion Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000B92 RID: 2962 RVA: 0x00028AA4 File Offset: 0x00026CA4
		internal bool ReadingResponse
		{
			get
			{
				return this.readingResponse;
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000B93 RID: 2963 RVA: 0x00028AAC File Offset: 0x00026CAC
		internal bool Synchronous
		{
			get
			{
				return this.synchronous;
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000B94 RID: 2964 RVA: 0x00028AB4 File Offset: 0x00026CB4
		internal IEdmModel Model
		{
			get
			{
				return this.model;
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000B95 RID: 2965 RVA: 0x00028ABC File Offset: 0x00026CBC
		internal EdmTypeResolver EdmTypeResolver
		{
			get
			{
				return this.edmTypeResolver;
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000B96 RID: 2966 RVA: 0x00028AC4 File Offset: 0x00026CC4
		internal IODataUrlResolver UrlResolver
		{
			get
			{
				return this.urlResolver;
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000B97 RID: 2967 RVA: 0x00028ACC File Offset: 0x00026CCC
		protected internal bool UseClientFormatBehavior
		{
			get
			{
				return this.messageReaderSettings.ReaderBehavior.FormatBehaviorKind == ODataBehaviorKind.WcfDataServicesClient;
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000B98 RID: 2968 RVA: 0x00028AE1 File Offset: 0x00026CE1
		protected internal bool UseServerFormatBehavior
		{
			get
			{
				return this.messageReaderSettings.ReaderBehavior.FormatBehaviorKind == ODataBehaviorKind.WcfDataServicesServer;
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000B99 RID: 2969 RVA: 0x00028AF6 File Offset: 0x00026CF6
		protected internal bool UseDefaultFormatBehavior
		{
			get
			{
				return this.messageReaderSettings.ReaderBehavior.FormatBehaviorKind == ODataBehaviorKind.Default;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000B9A RID: 2970 RVA: 0x00028B0B File Offset: 0x00026D0B
		protected internal bool UseClientApiBehavior
		{
			get
			{
				return this.messageReaderSettings.ReaderBehavior.ApiBehaviorKind == ODataBehaviorKind.WcfDataServicesClient;
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000B9B RID: 2971 RVA: 0x00028B20 File Offset: 0x00026D20
		protected internal bool UseServerApiBehavior
		{
			get
			{
				return this.messageReaderSettings.ReaderBehavior.ApiBehaviorKind == ODataBehaviorKind.WcfDataServicesServer;
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000B9C RID: 2972 RVA: 0x00028B35 File Offset: 0x00026D35
		protected internal bool UseDefaultApiBehavior
		{
			get
			{
				return this.messageReaderSettings.ReaderBehavior.ApiBehaviorKind == ODataBehaviorKind.Default;
			}
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x00028B4A File Offset: 0x00026D4A
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x00028B59 File Offset: 0x00026D59
		internal virtual ODataReader CreateFeedReader(IEdmEntitySet entitySet, IEdmEntityType expectedBaseEntityType)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Feed);
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x00028B62 File Offset: 0x00026D62
		internal virtual Task<ODataReader> CreateFeedReaderAsync(IEdmEntitySet entitySet, IEdmEntityType expectedBaseEntityType)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Feed);
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x00028B6B File Offset: 0x00026D6B
		internal virtual ODataReader CreateEntryReader(IEdmEntitySet entitySet, IEdmEntityType expectedEntityType)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Entry);
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x00028B74 File Offset: 0x00026D74
		internal virtual Task<ODataReader> CreateEntryReaderAsync(IEdmEntitySet entitySet, IEdmEntityType expectedEntityType)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Entry);
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x00028B7D File Offset: 0x00026D7D
		internal virtual ODataCollectionReader CreateCollectionReader(IEdmTypeReference expectedItemTypeReference)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Collection);
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x00028B86 File Offset: 0x00026D86
		internal virtual Task<ODataCollectionReader> CreateCollectionReaderAsync(IEdmTypeReference expectedItemTypeReference)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Collection);
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x00028B8F File Offset: 0x00026D8F
		internal virtual ODataBatchReader CreateBatchReader(string batchBoundary)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Batch);
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x00028B99 File Offset: 0x00026D99
		internal virtual Task<ODataBatchReader> CreateBatchReaderAsync(string batchBoundary)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Batch);
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x00028BA3 File Offset: 0x00026DA3
		internal virtual ODataParameterReader CreateParameterReader(IEdmFunctionImport functionImport)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Parameter);
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x00028BAD File Offset: 0x00026DAD
		internal virtual Task<ODataParameterReader> CreateParameterReaderAsync(IEdmFunctionImport functionImport)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Parameter);
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x00028BB7 File Offset: 0x00026DB7
		internal virtual ODataWorkspace ReadServiceDocument()
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.ServiceDocument);
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x00028BC0 File Offset: 0x00026DC0
		internal virtual Task<ODataWorkspace> ReadServiceDocumentAsync()
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.ServiceDocument);
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x00028BC9 File Offset: 0x00026DC9
		internal virtual IEdmModel ReadMetadataDocument()
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.MetadataDocument);
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x00028BD3 File Offset: 0x00026DD3
		internal virtual ODataProperty ReadProperty(IEdmStructuralProperty property, IEdmTypeReference expectedPropertyTypeReference)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Property);
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x00028BDC File Offset: 0x00026DDC
		internal virtual Task<ODataProperty> ReadPropertyAsync(IEdmStructuralProperty property, IEdmTypeReference expectedPropertyTypeReference)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Property);
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x00028BE5 File Offset: 0x00026DE5
		internal virtual ODataError ReadError()
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Error);
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x00028BEF File Offset: 0x00026DEF
		internal virtual Task<ODataError> ReadErrorAsync()
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Error);
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x00028BF9 File Offset: 0x00026DF9
		internal virtual ODataEntityReferenceLinks ReadEntityReferenceLinks(IEdmNavigationProperty navigationProperty)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.EntityReferenceLinks);
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x00028C02 File Offset: 0x00026E02
		internal virtual Task<ODataEntityReferenceLinks> ReadEntityReferenceLinksAsync(IEdmNavigationProperty navigationProperty)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.EntityReferenceLinks);
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x00028C0B File Offset: 0x00026E0B
		internal virtual ODataEntityReferenceLink ReadEntityReferenceLink(IEdmNavigationProperty navigationProperty)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.EntityReferenceLink);
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x00028C14 File Offset: 0x00026E14
		internal virtual Task<ODataEntityReferenceLink> ReadEntityReferenceLinkAsync(IEdmNavigationProperty navigationProperty)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.EntityReferenceLink);
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x00028C1D File Offset: 0x00026E1D
		internal virtual object ReadValue(IEdmPrimitiveTypeReference expectedPrimitiveTypeReference)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Value);
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x00028C26 File Offset: 0x00026E26
		internal virtual Task<object> ReadValueAsync(IEdmPrimitiveTypeReference expectedPrimitiveTypeReference)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Value);
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x00028C2F File Offset: 0x00026E2F
		internal void VerifyNotDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x00028C4A File Offset: 0x00026E4A
		[Conditional("DEBUG")]
		internal void AssertSynchronous()
		{
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x00028C4C File Offset: 0x00026E4C
		[Conditional("DEBUG")]
		internal void AssertAsynchronous()
		{
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x00028C4E File Offset: 0x00026E4E
		internal DuplicatePropertyNamesChecker CreateDuplicatePropertyNamesChecker()
		{
			return new DuplicatePropertyNamesChecker(this.MessageReaderSettings.ReaderBehavior.AllowDuplicatePropertyNames, this.ReadingResponse);
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x00028C6B File Offset: 0x00026E6B
		internal Uri ResolveUri(Uri baseUri, Uri payloadUri)
		{
			if (this.UrlResolver != null)
			{
				return this.UrlResolver.ResolveUrl(baseUri, payloadUri);
			}
			return null;
		}

		// Token: 0x06000BBA RID: 3002
		protected abstract void DisposeImplementation();

		// Token: 0x06000BBB RID: 3003 RVA: 0x00028C84 File Offset: 0x00026E84
		private void Dispose(bool disposing)
		{
			this.disposed = true;
			if (disposing)
			{
				this.DisposeImplementation();
			}
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x00028C96 File Offset: 0x00026E96
		private ODataException CreatePayloadKindNotSupportedException(ODataPayloadKind payloadKind)
		{
			return new ODataException(Strings.ODataInputContext_UnsupportedPayloadKindForFormat(this.format.ToString(), payloadKind.ToString()));
		}

		// Token: 0x0400041C RID: 1052
		private readonly ODataFormat format;

		// Token: 0x0400041D RID: 1053
		private readonly ODataMessageReaderSettings messageReaderSettings;

		// Token: 0x0400041E RID: 1054
		private readonly ODataVersion version;

		// Token: 0x0400041F RID: 1055
		private readonly bool readingResponse;

		// Token: 0x04000420 RID: 1056
		private readonly bool synchronous;

		// Token: 0x04000421 RID: 1057
		private readonly IODataUrlResolver urlResolver;

		// Token: 0x04000422 RID: 1058
		private readonly IEdmModel model;

		// Token: 0x04000423 RID: 1059
		private readonly EdmTypeResolver edmTypeResolver;

		// Token: 0x04000424 RID: 1060
		private bool disposed;
	}
}
