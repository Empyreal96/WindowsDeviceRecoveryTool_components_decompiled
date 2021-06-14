using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData
{
	// Token: 0x020001E9 RID: 489
	internal sealed class ODataRawValueFormat : ODataFormat
	{
		// Token: 0x06000F0D RID: 3853 RVA: 0x00035D0D File Offset: 0x00033F0D
		public override string ToString()
		{
			return "RawValue";
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x00035D14 File Offset: 0x00033F14
		internal override IEnumerable<ODataPayloadKind> DetectPayloadKind(IODataResponseMessage responseMessage, ODataPayloadKindDetectionInfo detectionInfo)
		{
			ExceptionUtils.CheckArgumentNotNull<IODataResponseMessage>(responseMessage, "responseMessage");
			ExceptionUtils.CheckArgumentNotNull<ODataPayloadKindDetectionInfo>(detectionInfo, "detectionInfo");
			return ODataRawValueFormat.DetectPayloadKindImplementation(detectionInfo.ContentType);
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x00035D37 File Offset: 0x00033F37
		internal override IEnumerable<ODataPayloadKind> DetectPayloadKind(IODataRequestMessage requestMessage, ODataPayloadKindDetectionInfo detectionInfo)
		{
			ExceptionUtils.CheckArgumentNotNull<IODataRequestMessage>(requestMessage, "requestMessage");
			ExceptionUtils.CheckArgumentNotNull<ODataPayloadKindDetectionInfo>(detectionInfo, "detectionInfo");
			return ODataRawValueFormat.DetectPayloadKindImplementation(detectionInfo.ContentType);
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x00035D5C File Offset: 0x00033F5C
		internal override ODataInputContext CreateInputContext(ODataPayloadKind readerPayloadKind, ODataMessage message, MediaType contentType, Encoding encoding, ODataMessageReaderSettings messageReaderSettings, ODataVersion version, bool readingResponse, IEdmModel model, IODataUrlResolver urlResolver, object payloadKindDetectionFormatState)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataMessage>(message, "message");
			ExceptionUtils.CheckArgumentNotNull<ODataMessageReaderSettings>(messageReaderSettings, "messageReaderSettings");
			Stream stream = message.GetStream();
			return new ODataRawInputContext(this, stream, encoding, messageReaderSettings, version, readingResponse, true, model, urlResolver, readerPayloadKind);
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x00035D9C File Offset: 0x00033F9C
		internal override ODataOutputContext CreateOutputContext(ODataMessage message, MediaType mediaType, Encoding encoding, ODataMessageWriterSettings messageWriterSettings, bool writingResponse, IEdmModel model, IODataUrlResolver urlResolver)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataMessage>(message, "message");
			ExceptionUtils.CheckArgumentNotNull<ODataMessageWriterSettings>(messageWriterSettings, "messageWriterSettings");
			Stream stream = message.GetStream();
			return new ODataRawOutputContext(this, stream, encoding, messageWriterSettings, writingResponse, true, model, urlResolver);
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x00035DF4 File Offset: 0x00033FF4
		internal override Task<IEnumerable<ODataPayloadKind>> DetectPayloadKindAsync(IODataResponseMessageAsync responseMessage, ODataPayloadKindDetectionInfo detectionInfo)
		{
			ExceptionUtils.CheckArgumentNotNull<IODataResponseMessageAsync>(responseMessage, "responseMessage");
			ExceptionUtils.CheckArgumentNotNull<ODataPayloadKindDetectionInfo>(detectionInfo, "detectionInfo");
			return TaskUtils.GetTaskForSynchronousOperation<IEnumerable<ODataPayloadKind>>(() => ODataRawValueFormat.DetectPayloadKindImplementation(detectionInfo.ContentType));
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x00035E54 File Offset: 0x00034054
		internal override Task<IEnumerable<ODataPayloadKind>> DetectPayloadKindAsync(IODataRequestMessageAsync requestMessage, ODataPayloadKindDetectionInfo detectionInfo)
		{
			ExceptionUtils.CheckArgumentNotNull<IODataRequestMessageAsync>(requestMessage, "requestMessage");
			ExceptionUtils.CheckArgumentNotNull<ODataPayloadKindDetectionInfo>(detectionInfo, "detectionInfo");
			return TaskUtils.GetTaskForSynchronousOperation<IEnumerable<ODataPayloadKind>>(() => ODataRawValueFormat.DetectPayloadKindImplementation(detectionInfo.ContentType));
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x00035EF0 File Offset: 0x000340F0
		internal override Task<ODataInputContext> CreateInputContextAsync(ODataPayloadKind readerPayloadKind, ODataMessage message, MediaType contentType, Encoding encoding, ODataMessageReaderSettings messageReaderSettings, ODataVersion version, bool readingResponse, IEdmModel model, IODataUrlResolver urlResolver, object payloadKindDetectionFormatState)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataMessage>(message, "message");
			ExceptionUtils.CheckArgumentNotNull<ODataMessageReaderSettings>(messageReaderSettings, "messageReaderSettings");
			return message.GetStreamAsync().FollowOnSuccessWith((Task<Stream> streamTask) => new ODataRawInputContext(this, streamTask.Result, encoding, messageReaderSettings, version, readingResponse, false, model, urlResolver, readerPayloadKind));
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x00035FB0 File Offset: 0x000341B0
		internal override Task<ODataOutputContext> CreateOutputContextAsync(ODataMessage message, MediaType mediaType, Encoding encoding, ODataMessageWriterSettings messageWriterSettings, bool writingResponse, IEdmModel model, IODataUrlResolver urlResolver)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataMessage>(message, "message");
			ExceptionUtils.CheckArgumentNotNull<ODataMessageWriterSettings>(messageWriterSettings, "messageWriterSettings");
			return message.GetStreamAsync().FollowOnSuccessWith((Task<Stream> streamTask) => new ODataRawOutputContext(this, streamTask.Result, encoding, messageWriterSettings, writingResponse, false, model, urlResolver));
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x00036024 File Offset: 0x00034224
		private static IEnumerable<ODataPayloadKind> DetectPayloadKindImplementation(MediaType contentType)
		{
			if (HttpUtils.CompareMediaTypeNames("text", contentType.TypeName) && HttpUtils.CompareMediaTypeNames("text/plain", contentType.SubTypeName))
			{
				return new ODataPayloadKind[]
				{
					ODataPayloadKind.Value
				};
			}
			return new ODataPayloadKind[]
			{
				ODataPayloadKind.BinaryValue
			};
		}
	}
}
