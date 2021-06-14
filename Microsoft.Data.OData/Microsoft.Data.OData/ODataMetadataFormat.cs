using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Atom;

namespace Microsoft.Data.OData
{
	// Token: 0x020001CE RID: 462
	internal sealed class ODataMetadataFormat : ODataFormat
	{
		// Token: 0x06000E65 RID: 3685 RVA: 0x00032892 File Offset: 0x00030A92
		public override string ToString()
		{
			return "Metadata";
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x0003289C File Offset: 0x00030A9C
		internal override IEnumerable<ODataPayloadKind> DetectPayloadKind(IODataResponseMessage responseMessage, ODataPayloadKindDetectionInfo detectionInfo)
		{
			ExceptionUtils.CheckArgumentNotNull<IODataResponseMessage>(responseMessage, "responseMessage");
			ExceptionUtils.CheckArgumentNotNull<ODataPayloadKindDetectionInfo>(detectionInfo, "detectionInfo");
			Stream stream = ((ODataMessage)responseMessage).GetStream();
			return ODataMetadataFormat.DetectPayloadKindImplementation(stream, detectionInfo);
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x000328D2 File Offset: 0x00030AD2
		internal override IEnumerable<ODataPayloadKind> DetectPayloadKind(IODataRequestMessage requestMessage, ODataPayloadKindDetectionInfo detectionInfo)
		{
			ExceptionUtils.CheckArgumentNotNull<IODataRequestMessage>(requestMessage, "requestMessage");
			ExceptionUtils.CheckArgumentNotNull<ODataPayloadKindDetectionInfo>(detectionInfo, "detectionInfo");
			return Enumerable.Empty<ODataPayloadKind>();
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x000328F0 File Offset: 0x00030AF0
		internal override ODataInputContext CreateInputContext(ODataPayloadKind readerPayloadKind, ODataMessage message, MediaType contentType, Encoding encoding, ODataMessageReaderSettings messageReaderSettings, ODataVersion version, bool readingResponse, IEdmModel model, IODataUrlResolver urlResolver, object payloadKindDetectionFormatState)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataMessage>(message, "message");
			ExceptionUtils.CheckArgumentNotNull<ODataMessageReaderSettings>(messageReaderSettings, "messageReaderSettings");
			Stream stream = message.GetStream();
			return new ODataMetadataInputContext(this, stream, encoding, messageReaderSettings, version, readingResponse, true, model, urlResolver);
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x00032930 File Offset: 0x00030B30
		internal override ODataOutputContext CreateOutputContext(ODataMessage message, MediaType mediaType, Encoding encoding, ODataMessageWriterSettings messageWriterSettings, bool writingResponse, IEdmModel model, IODataUrlResolver urlResolver)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataMessage>(message, "message");
			ExceptionUtils.CheckArgumentNotNull<ODataMessageWriterSettings>(messageWriterSettings, "messageWriterSettings");
			Stream stream = message.GetStream();
			return new ODataMetadataOutputContext(this, stream, encoding, messageWriterSettings, writingResponse, true, model, urlResolver);
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x00032988 File Offset: 0x00030B88
		internal override Task<IEnumerable<ODataPayloadKind>> DetectPayloadKindAsync(IODataResponseMessageAsync responseMessage, ODataPayloadKindDetectionInfo detectionInfo)
		{
			ExceptionUtils.CheckArgumentNotNull<IODataResponseMessageAsync>(responseMessage, "responseMessage");
			ExceptionUtils.CheckArgumentNotNull<ODataPayloadKindDetectionInfo>(detectionInfo, "detectionInfo");
			return ((ODataMessage)responseMessage).GetStreamAsync().FollowOnSuccessWith((Task<Stream> streamTask) => ODataMetadataFormat.DetectPayloadKindImplementation(streamTask.Result, detectionInfo));
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x000329D9 File Offset: 0x00030BD9
		internal override Task<IEnumerable<ODataPayloadKind>> DetectPayloadKindAsync(IODataRequestMessageAsync requestMessage, ODataPayloadKindDetectionInfo detectionInfo)
		{
			ExceptionUtils.CheckArgumentNotNull<IODataRequestMessageAsync>(requestMessage, "requestMessage");
			ExceptionUtils.CheckArgumentNotNull<ODataPayloadKindDetectionInfo>(detectionInfo, "detectionInfo");
			return TaskUtils.GetCompletedTask<IEnumerable<ODataPayloadKind>>(Enumerable.Empty<ODataPayloadKind>());
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x000329FB File Offset: 0x00030BFB
		internal override Task<ODataInputContext> CreateInputContextAsync(ODataPayloadKind readerPayloadKind, ODataMessage message, MediaType contentType, Encoding encoding, ODataMessageReaderSettings messageReaderSettings, ODataVersion version, bool readingResponse, IEdmModel model, IODataUrlResolver urlResolver, object payloadKindDetectionFormatState)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataMessage>(message, "message");
			ExceptionUtils.CheckArgumentNotNull<ODataMessageReaderSettings>(messageReaderSettings, "messageReaderSettings");
			throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataMetadataFormat_CreateInputContextAsync));
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x00032A25 File Offset: 0x00030C25
		internal override Task<ODataOutputContext> CreateOutputContextAsync(ODataMessage message, MediaType mediaType, Encoding encoding, ODataMessageWriterSettings messageWriterSettings, bool writingResponse, IEdmModel model, IODataUrlResolver urlResolver)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataMessage>(message, "message");
			ExceptionUtils.CheckArgumentNotNull<ODataMessageWriterSettings>(messageWriterSettings, "messageWriterSettings");
			throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataMetadataFormat_CreateOutputContextAsync));
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x00032A50 File Offset: 0x00030C50
		private static IEnumerable<ODataPayloadKind> DetectPayloadKindImplementation(Stream messageStream, ODataPayloadKindDetectionInfo detectionInfo)
		{
			try
			{
				using (XmlReader xmlReader = ODataAtomReaderUtils.CreateXmlReader(messageStream, detectionInfo.GetEncoding(), detectionInfo.MessageReaderSettings))
				{
					string namespaceURI;
					if (xmlReader.TryReadToNextElement() && string.CompareOrdinal("Edmx", xmlReader.LocalName) == 0 && (namespaceURI = xmlReader.NamespaceURI) != null && (namespaceURI == "http://schemas.microsoft.com/ado/2007/06/edmx" || namespaceURI == "http://schemas.microsoft.com/ado/2008/10/edmx" || namespaceURI == "http://schemas.microsoft.com/ado/2009/11/edmx"))
					{
						return new ODataPayloadKind[]
						{
							ODataPayloadKind.MetadataDocument
						};
					}
				}
			}
			catch (XmlException)
			{
			}
			return Enumerable.Empty<ODataPayloadKind>();
		}
	}
}
