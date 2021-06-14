using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData
{
	// Token: 0x020000FA RID: 250
	internal abstract class ODataOutputContext : IDisposable
	{
		// Token: 0x0600066A RID: 1642 RVA: 0x0001759C File Offset: 0x0001579C
		protected ODataOutputContext(ODataFormat format, ODataMessageWriterSettings messageWriterSettings, bool writingResponse, bool synchronous, IEdmModel model, IODataUrlResolver urlResolver)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataFormat>(format, "format");
			ExceptionUtils.CheckArgumentNotNull<ODataMessageWriterSettings>(messageWriterSettings, "messageWriterSettings");
			this.format = format;
			this.messageWriterSettings = messageWriterSettings;
			this.writingResponse = writingResponse;
			this.synchronous = synchronous;
			this.model = (model ?? EdmCoreModel.Instance);
			this.urlResolver = urlResolver;
			this.edmTypeResolver = EdmTypeWriterResolver.Instance;
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x00017606 File Offset: 0x00015806
		internal ODataMessageWriterSettings MessageWriterSettings
		{
			get
			{
				return this.messageWriterSettings;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x0600066C RID: 1644 RVA: 0x00017610 File Offset: 0x00015810
		internal ODataVersion Version
		{
			get
			{
				return this.messageWriterSettings.Version.Value;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x0600066D RID: 1645 RVA: 0x00017630 File Offset: 0x00015830
		internal bool WritingResponse
		{
			get
			{
				return this.writingResponse;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x00017638 File Offset: 0x00015838
		internal bool Synchronous
		{
			get
			{
				return this.synchronous;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x0600066F RID: 1647 RVA: 0x00017640 File Offset: 0x00015840
		internal IEdmModel Model
		{
			get
			{
				return this.model;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000670 RID: 1648 RVA: 0x00017648 File Offset: 0x00015848
		internal IODataUrlResolver UrlResolver
		{
			get
			{
				return this.urlResolver;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000671 RID: 1649 RVA: 0x00017650 File Offset: 0x00015850
		internal EdmTypeResolver EdmTypeResolver
		{
			get
			{
				return this.edmTypeResolver;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000672 RID: 1650 RVA: 0x00017658 File Offset: 0x00015858
		protected internal bool UseClientFormatBehavior
		{
			get
			{
				return this.messageWriterSettings.WriterBehavior.FormatBehaviorKind == ODataBehaviorKind.WcfDataServicesClient;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x0001766D File Offset: 0x0001586D
		protected internal bool UseServerFormatBehavior
		{
			get
			{
				return this.messageWriterSettings.WriterBehavior.FormatBehaviorKind == ODataBehaviorKind.WcfDataServicesServer;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000674 RID: 1652 RVA: 0x00017682 File Offset: 0x00015882
		protected internal bool UseDefaultFormatBehavior
		{
			get
			{
				return this.messageWriterSettings.WriterBehavior.FormatBehaviorKind == ODataBehaviorKind.Default;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000675 RID: 1653 RVA: 0x00017697 File Offset: 0x00015897
		protected internal bool UseServerApiBehavior
		{
			get
			{
				return this.messageWriterSettings.WriterBehavior.ApiBehaviorKind == ODataBehaviorKind.WcfDataServicesServer;
			}
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x000176AC File Offset: 0x000158AC
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x000176BB File Offset: 0x000158BB
		internal virtual void WriteInStreamError(ODataError error, bool includeDebugInformation)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Error);
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x000176C5 File Offset: 0x000158C5
		internal virtual Task WriteInStreamErrorAsync(ODataError error, bool includeDebugInformation)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Error);
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x000176CF File Offset: 0x000158CF
		internal virtual ODataWriter CreateODataFeedWriter(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Feed);
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x000176D8 File Offset: 0x000158D8
		internal virtual Task<ODataWriter> CreateODataFeedWriterAsync(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Feed);
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x000176E1 File Offset: 0x000158E1
		internal virtual ODataWriter CreateODataEntryWriter(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Entry);
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x000176EA File Offset: 0x000158EA
		internal virtual Task<ODataWriter> CreateODataEntryWriterAsync(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Entry);
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x000176F3 File Offset: 0x000158F3
		internal virtual ODataCollectionWriter CreateODataCollectionWriter(IEdmTypeReference itemTypeReference)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Collection);
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x000176FC File Offset: 0x000158FC
		internal virtual Task<ODataCollectionWriter> CreateODataCollectionWriterAsync(IEdmTypeReference itemTypeReference)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Collection);
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00017705 File Offset: 0x00015905
		internal virtual ODataBatchWriter CreateODataBatchWriter(string batchBoundary)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Batch);
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x0001770F File Offset: 0x0001590F
		internal virtual Task<ODataBatchWriter> CreateODataBatchWriterAsync(string batchBoundary)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Batch);
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00017719 File Offset: 0x00015919
		internal virtual ODataParameterWriter CreateODataParameterWriter(IEdmFunctionImport functionImport)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Error);
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00017723 File Offset: 0x00015923
		internal virtual Task<ODataParameterWriter> CreateODataParameterWriterAsync(IEdmFunctionImport functionImport)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Error);
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0001772D File Offset: 0x0001592D
		internal virtual void WriteServiceDocument(ODataWorkspace defaultWorkspace)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.ServiceDocument);
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x00017736 File Offset: 0x00015936
		internal virtual Task WriteServiceDocumentAsync(ODataWorkspace defaultWorkspace)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.ServiceDocument);
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0001773F File Offset: 0x0001593F
		internal virtual void WriteProperty(ODataProperty property)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Property);
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00017748 File Offset: 0x00015948
		internal virtual Task WritePropertyAsync(ODataProperty property)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Property);
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00017751 File Offset: 0x00015951
		internal virtual void WriteError(ODataError error, bool includeDebugInformation)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Error);
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x0001775B File Offset: 0x0001595B
		internal virtual Task WriteErrorAsync(ODataError error, bool includeDebugInformation)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Error);
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x00017765 File Offset: 0x00015965
		internal virtual void WriteEntityReferenceLinks(ODataEntityReferenceLinks links, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.EntityReferenceLinks);
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0001776E File Offset: 0x0001596E
		internal virtual Task WriteEntityReferenceLinksAsync(ODataEntityReferenceLinks links, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.EntityReferenceLinks);
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00017777 File Offset: 0x00015977
		internal virtual void WriteEntityReferenceLink(ODataEntityReferenceLink link, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.EntityReferenceLink);
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00017780 File Offset: 0x00015980
		internal virtual Task WriteEntityReferenceLinkAsync(ODataEntityReferenceLink link, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.EntityReferenceLink);
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x00017789 File Offset: 0x00015989
		internal virtual void WriteValue(object value)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Value);
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x00017792 File Offset: 0x00015992
		internal virtual Task WriteValueAsync(object value)
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.Value);
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0001779B File Offset: 0x0001599B
		internal virtual void WriteMetadataDocument()
		{
			throw this.CreatePayloadKindNotSupportedException(ODataPayloadKind.MetadataDocument);
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x000177A5 File Offset: 0x000159A5
		[Conditional("DEBUG")]
		internal void AssertSynchronous()
		{
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x000177A7 File Offset: 0x000159A7
		[Conditional("DEBUG")]
		internal void AssertAsynchronous()
		{
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x000177A9 File Offset: 0x000159A9
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x000177AB File Offset: 0x000159AB
		private ODataException CreatePayloadKindNotSupportedException(ODataPayloadKind payloadKind)
		{
			return new ODataException(Strings.ODataOutputContext_UnsupportedPayloadKindForFormat(this.format.ToString(), payloadKind.ToString()));
		}

		// Token: 0x04000289 RID: 649
		private readonly ODataFormat format;

		// Token: 0x0400028A RID: 650
		private readonly ODataMessageWriterSettings messageWriterSettings;

		// Token: 0x0400028B RID: 651
		private readonly bool writingResponse;

		// Token: 0x0400028C RID: 652
		private readonly bool synchronous;

		// Token: 0x0400028D RID: 653
		private readonly IEdmModel model;

		// Token: 0x0400028E RID: 654
		private readonly IODataUrlResolver urlResolver;

		// Token: 0x0400028F RID: 655
		private readonly EdmTypeResolver edmTypeResolver;
	}
}
