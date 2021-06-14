using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x020000FB RID: 251
	internal sealed class ODataAtomOutputContext : ODataOutputContext
	{
		// Token: 0x06000694 RID: 1684 RVA: 0x000177D0 File Offset: 0x000159D0
		internal ODataAtomOutputContext(ODataFormat format, Stream messageStream, Encoding encoding, ODataMessageWriterSettings messageWriterSettings, bool writingResponse, bool synchronous, IEdmModel model, IODataUrlResolver urlResolver) : base(format, messageWriterSettings, writingResponse, synchronous, model, urlResolver)
		{
			try
			{
				this.messageOutputStream = messageStream;
				Stream stream;
				if (synchronous)
				{
					stream = messageStream;
				}
				else
				{
					this.asynchronousOutputStream = new AsyncBufferedStream(messageStream);
					stream = this.asynchronousOutputStream;
				}
				this.xmlRootWriter = ODataAtomWriterUtils.CreateXmlWriter(stream, messageWriterSettings, encoding);
				this.xmlWriter = this.xmlRootWriter;
			}
			catch (Exception e)
			{
				if (ExceptionUtils.IsCatchableExceptionType(e) && messageStream != null)
				{
					messageStream.Dispose();
				}
				throw;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000695 RID: 1685 RVA: 0x0001785C File Offset: 0x00015A5C
		internal XmlWriter XmlWriter
		{
			get
			{
				return this.xmlWriter;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000696 RID: 1686 RVA: 0x00017864 File Offset: 0x00015A64
		internal AtomAndVerboseJsonTypeNameOracle TypeNameOracle
		{
			get
			{
				return this.typeNameOracle;
			}
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0001786C File Offset: 0x00015A6C
		internal void VerifyNotDisposed()
		{
			if (this.messageOutputStream == null)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x00017887 File Offset: 0x00015A87
		internal void Flush()
		{
			this.xmlWriter.Flush();
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x000178B9 File Offset: 0x00015AB9
		internal Task FlushAsync()
		{
			return TaskUtils.GetTaskForSynchronousOperationReturningTask(delegate()
			{
				this.xmlWriter.Flush();
				return this.asynchronousOutputStream.FlushAsync();
			}).FollowOnSuccessWithTask((Task asyncBufferedStreamFlushTask) => this.messageOutputStream.FlushAsync());
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x000178DD File Offset: 0x00015ADD
		internal override void WriteInStreamError(ODataError error, bool includeDebugInformation)
		{
			this.WriteInStreamErrorImplementation(error, includeDebugInformation);
			this.Flush();
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0001791C File Offset: 0x00015B1C
		internal override Task WriteInStreamErrorAsync(ODataError error, bool includeDebugInformation)
		{
			return TaskUtils.GetTaskForSynchronousOperationReturningTask(delegate()
			{
				this.WriteInStreamErrorImplementation(error, includeDebugInformation);
				return this.FlushAsync();
			});
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00017955 File Offset: 0x00015B55
		internal override ODataWriter CreateODataFeedWriter(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			return this.CreateODataFeedWriterImplementation(entitySet, entityType);
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00017980 File Offset: 0x00015B80
		internal override Task<ODataWriter> CreateODataFeedWriterAsync(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			return TaskUtils.GetTaskForSynchronousOperation<ODataWriter>(() => this.CreateODataFeedWriterImplementation(entitySet, entityType));
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x000179B9 File Offset: 0x00015BB9
		internal override ODataWriter CreateODataEntryWriter(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			return this.CreateODataEntryWriterImplementation(entitySet, entityType);
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x000179E4 File Offset: 0x00015BE4
		internal override Task<ODataWriter> CreateODataEntryWriterAsync(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			return TaskUtils.GetTaskForSynchronousOperation<ODataWriter>(() => this.CreateODataEntryWriterImplementation(entitySet, entityType));
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x00017A1D File Offset: 0x00015C1D
		internal override ODataCollectionWriter CreateODataCollectionWriter(IEdmTypeReference itemTypeReference)
		{
			return this.CreateODataCollectionWriterImplementation(itemTypeReference);
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x00017A44 File Offset: 0x00015C44
		internal override Task<ODataCollectionWriter> CreateODataCollectionWriterAsync(IEdmTypeReference itemTypeReference)
		{
			return TaskUtils.GetTaskForSynchronousOperation<ODataCollectionWriter>(() => this.CreateODataCollectionWriterImplementation(itemTypeReference));
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00017A76 File Offset: 0x00015C76
		internal override void WriteServiceDocument(ODataWorkspace defaultWorkspace)
		{
			this.WriteServiceDocumentImplementation(defaultWorkspace);
			this.Flush();
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x00017AAC File Offset: 0x00015CAC
		internal override Task WriteServiceDocumentAsync(ODataWorkspace defaultWorkspace)
		{
			return TaskUtils.GetTaskForSynchronousOperationReturningTask(delegate()
			{
				this.WriteServiceDocumentImplementation(defaultWorkspace);
				return this.FlushAsync();
			});
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x00017ADE File Offset: 0x00015CDE
		internal override void WriteProperty(ODataProperty property)
		{
			this.WritePropertyImplementation(property);
			this.Flush();
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00017B14 File Offset: 0x00015D14
		internal override Task WritePropertyAsync(ODataProperty property)
		{
			return TaskUtils.GetTaskForSynchronousOperationReturningTask(delegate()
			{
				this.WritePropertyImplementation(property);
				return this.FlushAsync();
			});
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00017B46 File Offset: 0x00015D46
		internal override void WriteError(ODataError error, bool includeDebugInformation)
		{
			this.WriteErrorImplementation(error, includeDebugInformation);
			this.Flush();
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x00017B84 File Offset: 0x00015D84
		internal override Task WriteErrorAsync(ODataError error, bool includeDebugInformation)
		{
			return TaskUtils.GetTaskForSynchronousOperationReturningTask(delegate()
			{
				this.WriteErrorImplementation(error, includeDebugInformation);
				return this.FlushAsync();
			});
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x00017BBD File Offset: 0x00015DBD
		internal override void WriteEntityReferenceLinks(ODataEntityReferenceLinks links, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty)
		{
			this.WriteEntityReferenceLinksImplementation(links);
			this.Flush();
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x00017BF4 File Offset: 0x00015DF4
		internal override Task WriteEntityReferenceLinksAsync(ODataEntityReferenceLinks links, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty)
		{
			return TaskUtils.GetTaskForSynchronousOperationReturningTask(delegate()
			{
				this.WriteEntityReferenceLinksImplementation(links);
				return this.FlushAsync();
			});
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x00017C26 File Offset: 0x00015E26
		internal override void WriteEntityReferenceLink(ODataEntityReferenceLink link, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty)
		{
			this.WriteEntityReferenceLinkImplementation(link);
			this.Flush();
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x00017C5C File Offset: 0x00015E5C
		internal override Task WriteEntityReferenceLinkAsync(ODataEntityReferenceLink link, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty)
		{
			return TaskUtils.GetTaskForSynchronousOperationReturningTask(delegate()
			{
				this.WriteEntityReferenceLinkImplementation(link);
				return this.FlushAsync();
			});
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x00017C8E File Offset: 0x00015E8E
		internal void InitializeWriterCustomization()
		{
			this.xmlCustomizationWriters = new Stack<XmlWriter>();
			this.xmlCustomizationWriters.Push(this.xmlRootWriter);
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x00017CAC File Offset: 0x00015EAC
		internal void PushCustomWriter(XmlWriter customXmlWriter)
		{
			this.xmlCustomizationWriters.Push(customXmlWriter);
			this.xmlWriter = customXmlWriter;
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00017CC4 File Offset: 0x00015EC4
		internal XmlWriter PopCustomWriter()
		{
			XmlWriter result = this.xmlCustomizationWriters.Pop();
			this.xmlWriter = this.xmlCustomizationWriters.Peek();
			return result;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00017CF0 File Offset: 0x00015EF0
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			try
			{
				if (this.messageOutputStream != null)
				{
					this.xmlRootWriter.Flush();
					if (this.asynchronousOutputStream != null)
					{
						this.asynchronousOutputStream.FlushSync();
						this.asynchronousOutputStream.Dispose();
					}
					this.messageOutputStream.Dispose();
				}
			}
			finally
			{
				this.messageOutputStream = null;
				this.asynchronousOutputStream = null;
				this.xmlWriter = null;
			}
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x00017D68 File Offset: 0x00015F68
		private void WriteInStreamErrorImplementation(ODataError error, bool includeDebugInformation)
		{
			if (this.outputInStreamErrorListener != null)
			{
				this.outputInStreamErrorListener.OnInStreamError();
			}
			ODataAtomWriterUtils.WriteError(this.xmlWriter, error, includeDebugInformation, base.MessageWriterSettings.MessageQuotas.MaxNestingDepth);
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x00017D9C File Offset: 0x00015F9C
		private ODataWriter CreateODataFeedWriterImplementation(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			ODataAtomWriter result = new ODataAtomWriter(this, entitySet, entityType, true);
			this.outputInStreamErrorListener = result;
			return result;
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x00017DBC File Offset: 0x00015FBC
		private ODataWriter CreateODataEntryWriterImplementation(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			ODataAtomWriter result = new ODataAtomWriter(this, entitySet, entityType, false);
			this.outputInStreamErrorListener = result;
			return result;
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00017DDC File Offset: 0x00015FDC
		private ODataCollectionWriter CreateODataCollectionWriterImplementation(IEdmTypeReference itemTypeReference)
		{
			ODataAtomCollectionWriter result = new ODataAtomCollectionWriter(this, itemTypeReference);
			this.outputInStreamErrorListener = result;
			return result;
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x00017DFC File Offset: 0x00015FFC
		private void WritePropertyImplementation(ODataProperty property)
		{
			ODataAtomPropertyAndValueSerializer odataAtomPropertyAndValueSerializer = new ODataAtomPropertyAndValueSerializer(this);
			odataAtomPropertyAndValueSerializer.WriteTopLevelProperty(property);
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x00017E18 File Offset: 0x00016018
		private void WriteServiceDocumentImplementation(ODataWorkspace defaultWorkspace)
		{
			ODataAtomServiceDocumentSerializer odataAtomServiceDocumentSerializer = new ODataAtomServiceDocumentSerializer(this);
			odataAtomServiceDocumentSerializer.WriteServiceDocument(defaultWorkspace);
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x00017E34 File Offset: 0x00016034
		private void WriteErrorImplementation(ODataError error, bool includeDebugInformation)
		{
			ODataAtomSerializer odataAtomSerializer = new ODataAtomSerializer(this);
			odataAtomSerializer.WriteTopLevelError(error, includeDebugInformation);
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x00017E50 File Offset: 0x00016050
		private void WriteEntityReferenceLinksImplementation(ODataEntityReferenceLinks links)
		{
			ODataAtomEntityReferenceLinkSerializer odataAtomEntityReferenceLinkSerializer = new ODataAtomEntityReferenceLinkSerializer(this);
			odataAtomEntityReferenceLinkSerializer.WriteEntityReferenceLinks(links);
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x00017E6C File Offset: 0x0001606C
		private void WriteEntityReferenceLinkImplementation(ODataEntityReferenceLink link)
		{
			ODataAtomEntityReferenceLinkSerializer odataAtomEntityReferenceLinkSerializer = new ODataAtomEntityReferenceLinkSerializer(this);
			odataAtomEntityReferenceLinkSerializer.WriteEntityReferenceLink(link);
		}

		// Token: 0x04000290 RID: 656
		private readonly AtomAndVerboseJsonTypeNameOracle typeNameOracle = new AtomAndVerboseJsonTypeNameOracle();

		// Token: 0x04000291 RID: 657
		private Stream messageOutputStream;

		// Token: 0x04000292 RID: 658
		private AsyncBufferedStream asynchronousOutputStream;

		// Token: 0x04000293 RID: 659
		private XmlWriter xmlRootWriter;

		// Token: 0x04000294 RID: 660
		private XmlWriter xmlWriter;

		// Token: 0x04000295 RID: 661
		private Stack<XmlWriter> xmlCustomizationWriters;

		// Token: 0x04000296 RID: 662
		private IODataOutputInStreamErrorListener outputInStreamErrorListener;
	}
}
