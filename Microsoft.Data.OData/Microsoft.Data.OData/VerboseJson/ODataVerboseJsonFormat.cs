using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.VerboseJson
{
	// Token: 0x020001CC RID: 460
	internal sealed class ODataVerboseJsonFormat : ODataFormat
	{
		// Token: 0x06000E4E RID: 3662 RVA: 0x0003211E File Offset: 0x0003031E
		public override string ToString()
		{
			return "VerboseJson";
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x00032128 File Offset: 0x00030328
		internal override IEnumerable<ODataPayloadKind> DetectPayloadKind(IODataResponseMessage responseMessage, ODataPayloadKindDetectionInfo detectionInfo)
		{
			ExceptionUtils.CheckArgumentNotNull<IODataResponseMessage>(responseMessage, "responseMessage");
			ExceptionUtils.CheckArgumentNotNull<ODataPayloadKindDetectionInfo>(detectionInfo, "detectionInfo");
			Stream stream = ((ODataMessage)responseMessage).GetStream();
			return this.DetectPayloadKindImplementation(stream, true, true, detectionInfo);
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x00032164 File Offset: 0x00030364
		internal override IEnumerable<ODataPayloadKind> DetectPayloadKind(IODataRequestMessage requestMessage, ODataPayloadKindDetectionInfo detectionInfo)
		{
			ExceptionUtils.CheckArgumentNotNull<IODataRequestMessage>(requestMessage, "requestMessage");
			ExceptionUtils.CheckArgumentNotNull<ODataPayloadKindDetectionInfo>(detectionInfo, "detectionInfo");
			Stream stream = ((ODataMessage)requestMessage).GetStream();
			return this.DetectPayloadKindImplementation(stream, false, true, detectionInfo);
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x000321A0 File Offset: 0x000303A0
		internal override ODataInputContext CreateInputContext(ODataPayloadKind readerPayloadKind, ODataMessage message, MediaType contentType, Encoding encoding, ODataMessageReaderSettings messageReaderSettings, ODataVersion version, bool readingResponse, IEdmModel model, IODataUrlResolver urlResolver, object payloadKindDetectionFormatState)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataMessage>(message, "message");
			ExceptionUtils.CheckArgumentNotNull<ODataMessageReaderSettings>(messageReaderSettings, "messageReaderSettings");
			Stream stream = message.GetStream();
			return new ODataVerboseJsonInputContext(this, stream, encoding, messageReaderSettings, version, readingResponse, true, model, urlResolver);
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x000321E0 File Offset: 0x000303E0
		internal override ODataOutputContext CreateOutputContext(ODataMessage message, MediaType mediaType, Encoding encoding, ODataMessageWriterSettings messageWriterSettings, bool writingResponse, IEdmModel model, IODataUrlResolver urlResolver)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataMessage>(message, "message");
			ExceptionUtils.CheckArgumentNotNull<ODataMessageWriterSettings>(messageWriterSettings, "messageWriterSettings");
			Stream stream = message.GetStream();
			return new ODataVerboseJsonOutputContext(this, stream, encoding, messageWriterSettings, writingResponse, true, model, urlResolver);
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x00032240 File Offset: 0x00030440
		internal override Task<IEnumerable<ODataPayloadKind>> DetectPayloadKindAsync(IODataResponseMessageAsync responseMessage, ODataPayloadKindDetectionInfo detectionInfo)
		{
			ExceptionUtils.CheckArgumentNotNull<IODataResponseMessageAsync>(responseMessage, "responseMessage");
			ExceptionUtils.CheckArgumentNotNull<ODataPayloadKindDetectionInfo>(detectionInfo, "detectionInfo");
			return ((ODataMessage)responseMessage).GetStreamAsync().FollowOnSuccessWith((Task<Stream> streamTask) => this.DetectPayloadKindImplementation(streamTask.Result, true, false, detectionInfo));
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x000322BC File Offset: 0x000304BC
		internal override Task<IEnumerable<ODataPayloadKind>> DetectPayloadKindAsync(IODataRequestMessageAsync requestMessage, ODataPayloadKindDetectionInfo detectionInfo)
		{
			ExceptionUtils.CheckArgumentNotNull<IODataRequestMessageAsync>(requestMessage, "requestMessage");
			ExceptionUtils.CheckArgumentNotNull<ODataPayloadKindDetectionInfo>(detectionInfo, "detectionInfo");
			return ((ODataMessage)requestMessage).GetStreamAsync().FollowOnSuccessWith((Task<Stream> streamTask) => this.DetectPayloadKindImplementation(streamTask.Result, false, false, detectionInfo));
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x00032360 File Offset: 0x00030560
		internal override Task<ODataInputContext> CreateInputContextAsync(ODataPayloadKind readerPayloadKind, ODataMessage message, MediaType contentType, Encoding encoding, ODataMessageReaderSettings messageReaderSettings, ODataVersion version, bool readingResponse, IEdmModel model, IODataUrlResolver urlResolver, object payloadKindDetectionFormatState)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataMessage>(message, "message");
			ExceptionUtils.CheckArgumentNotNull<ODataMessageReaderSettings>(messageReaderSettings, "messageReaderSettings");
			return message.GetStreamAsync().FollowOnSuccessWith((Task<Stream> streamTask) => new ODataVerboseJsonInputContext(this, streamTask.Result, encoding, messageReaderSettings, version, readingResponse, false, model, urlResolver));
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x00032418 File Offset: 0x00030618
		internal override Task<ODataOutputContext> CreateOutputContextAsync(ODataMessage message, MediaType mediaType, Encoding encoding, ODataMessageWriterSettings messageWriterSettings, bool writingResponse, IEdmModel model, IODataUrlResolver urlResolver)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataMessage>(message, "message");
			ExceptionUtils.CheckArgumentNotNull<ODataMessageWriterSettings>(messageWriterSettings, "messageWriterSettings");
			return message.GetStreamAsync().FollowOnSuccessWith((Task<Stream> streamTask) => new ODataVerboseJsonOutputContext(this, streamTask.Result, encoding, messageWriterSettings, writingResponse, false, model, urlResolver));
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x0003248C File Offset: 0x0003068C
		private IEnumerable<ODataPayloadKind> DetectPayloadKindImplementation(Stream messageStream, bool readingResponse, bool synchronous, ODataPayloadKindDetectionInfo detectionInfo)
		{
			IEnumerable<ODataPayloadKind> result;
			using (ODataVerboseJsonInputContext odataVerboseJsonInputContext = new ODataVerboseJsonInputContext(this, messageStream, detectionInfo.GetEncoding(), detectionInfo.MessageReaderSettings, ODataVersion.V3, readingResponse, synchronous, detectionInfo.Model, null))
			{
				result = odataVerboseJsonInputContext.DetectPayloadKind();
			}
			return result;
		}
	}
}
