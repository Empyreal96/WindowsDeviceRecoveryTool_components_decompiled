using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData
{
	// Token: 0x020001CD RID: 461
	internal sealed class ODataBatchFormat : ODataFormat
	{
		// Token: 0x06000E59 RID: 3673 RVA: 0x000324E8 File Offset: 0x000306E8
		public override string ToString()
		{
			return "Batch";
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x000324EF File Offset: 0x000306EF
		internal override IEnumerable<ODataPayloadKind> DetectPayloadKind(IODataResponseMessage responseMessage, ODataPayloadKindDetectionInfo detectionInfo)
		{
			ExceptionUtils.CheckArgumentNotNull<IODataResponseMessage>(responseMessage, "responseMessage");
			ExceptionUtils.CheckArgumentNotNull<ODataPayloadKindDetectionInfo>(detectionInfo, "detectionInfo");
			return ODataBatchFormat.DetectPayloadKindImplementation(detectionInfo.ContentType);
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x00032512 File Offset: 0x00030712
		internal override IEnumerable<ODataPayloadKind> DetectPayloadKind(IODataRequestMessage requestMessage, ODataPayloadKindDetectionInfo detectionInfo)
		{
			ExceptionUtils.CheckArgumentNotNull<IODataRequestMessage>(requestMessage, "requestMessage");
			ExceptionUtils.CheckArgumentNotNull<ODataPayloadKindDetectionInfo>(detectionInfo, "detectionInfo");
			return ODataBatchFormat.DetectPayloadKindImplementation(detectionInfo.ContentType);
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x00032538 File Offset: 0x00030738
		internal override ODataInputContext CreateInputContext(ODataPayloadKind readerPayloadKind, ODataMessage message, MediaType contentType, Encoding encoding, ODataMessageReaderSettings messageReaderSettings, ODataVersion version, bool readingResponse, IEdmModel model, IODataUrlResolver urlResolver, object payloadKindDetectionFormatState)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataMessage>(message, "message");
			ExceptionUtils.CheckArgumentNotNull<ODataMessageReaderSettings>(messageReaderSettings, "messageReaderSettings");
			Stream stream = message.GetStream();
			return new ODataRawInputContext(this, stream, encoding, messageReaderSettings, version, readingResponse, true, model, urlResolver, readerPayloadKind);
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x00032578 File Offset: 0x00030778
		internal override ODataOutputContext CreateOutputContext(ODataMessage message, MediaType mediaType, Encoding encoding, ODataMessageWriterSettings messageWriterSettings, bool writingResponse, IEdmModel model, IODataUrlResolver urlResolver)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataMessage>(message, "message");
			ExceptionUtils.CheckArgumentNotNull<ODataMessageWriterSettings>(messageWriterSettings, "messageWriterSettings");
			Stream stream = message.GetStream();
			return new ODataRawOutputContext(this, stream, encoding, messageWriterSettings, writingResponse, true, model, urlResolver);
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x000325D0 File Offset: 0x000307D0
		internal override Task<IEnumerable<ODataPayloadKind>> DetectPayloadKindAsync(IODataResponseMessageAsync responseMessage, ODataPayloadKindDetectionInfo detectionInfo)
		{
			ExceptionUtils.CheckArgumentNotNull<IODataResponseMessageAsync>(responseMessage, "responseMessage");
			ExceptionUtils.CheckArgumentNotNull<ODataPayloadKindDetectionInfo>(detectionInfo, "detectionInfo");
			return TaskUtils.GetTaskForSynchronousOperation<IEnumerable<ODataPayloadKind>>(() => ODataBatchFormat.DetectPayloadKindImplementation(detectionInfo.ContentType));
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x00032630 File Offset: 0x00030830
		internal override Task<IEnumerable<ODataPayloadKind>> DetectPayloadKindAsync(IODataRequestMessageAsync requestMessage, ODataPayloadKindDetectionInfo detectionInfo)
		{
			ExceptionUtils.CheckArgumentNotNull<IODataRequestMessageAsync>(requestMessage, "requestMessage");
			ExceptionUtils.CheckArgumentNotNull<ODataPayloadKindDetectionInfo>(detectionInfo, "detectionInfo");
			return TaskUtils.GetTaskForSynchronousOperation<IEnumerable<ODataPayloadKind>>(() => ODataBatchFormat.DetectPayloadKindImplementation(detectionInfo.ContentType));
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x000326CC File Offset: 0x000308CC
		internal override Task<ODataInputContext> CreateInputContextAsync(ODataPayloadKind readerPayloadKind, ODataMessage message, MediaType contentType, Encoding encoding, ODataMessageReaderSettings messageReaderSettings, ODataVersion version, bool readingResponse, IEdmModel model, IODataUrlResolver urlResolver, object payloadKindDetectionFormatState)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataMessage>(message, "message");
			ExceptionUtils.CheckArgumentNotNull<ODataMessageReaderSettings>(messageReaderSettings, "messageReaderSettings");
			return message.GetStreamAsync().FollowOnSuccessWith((Task<Stream> streamTask) => new ODataRawInputContext(this, streamTask.Result, encoding, messageReaderSettings, version, readingResponse, false, model, urlResolver, readerPayloadKind));
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x0003278C File Offset: 0x0003098C
		internal override Task<ODataOutputContext> CreateOutputContextAsync(ODataMessage message, MediaType mediaType, Encoding encoding, ODataMessageWriterSettings messageWriterSettings, bool writingResponse, IEdmModel model, IODataUrlResolver urlResolver)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataMessage>(message, "message");
			ExceptionUtils.CheckArgumentNotNull<ODataMessageWriterSettings>(messageWriterSettings, "messageWriterSettings");
			return message.GetStreamAsync().FollowOnSuccessWith((Task<Stream> streamTask) => new ODataRawOutputContext(this, streamTask.Result, encoding, messageWriterSettings, writingResponse, false, model, urlResolver));
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x00032814 File Offset: 0x00030A14
		private static IEnumerable<ODataPayloadKind> DetectPayloadKindImplementation(MediaType contentType)
		{
			if (HttpUtils.CompareMediaTypeNames("multipart", contentType.TypeName) && HttpUtils.CompareMediaTypeNames("mixed", contentType.SubTypeName) && contentType.Parameters != null)
			{
				if (contentType.Parameters.Any((KeyValuePair<string, string> kvp) => HttpUtils.CompareMediaTypeParameterNames("boundary", kvp.Key)))
				{
					return new ODataPayloadKind[]
					{
						ODataPayloadKind.Batch
					};
				}
			}
			return Enumerable.Empty<ODataPayloadKind>();
		}
	}
}
