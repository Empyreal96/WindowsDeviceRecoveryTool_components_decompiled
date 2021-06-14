using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x02000190 RID: 400
	internal sealed class ODataJsonLightFormat : ODataFormat
	{
		// Token: 0x06000B83 RID: 2947 RVA: 0x00028567 File Offset: 0x00026767
		public override string ToString()
		{
			return "JsonLight";
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x00028570 File Offset: 0x00026770
		internal override IEnumerable<ODataPayloadKind> DetectPayloadKind(IODataResponseMessage responseMessage, ODataPayloadKindDetectionInfo detectionInfo)
		{
			ExceptionUtils.CheckArgumentNotNull<IODataResponseMessage>(responseMessage, "responseMessage");
			ExceptionUtils.CheckArgumentNotNull<ODataPayloadKindDetectionInfo>(detectionInfo, "detectionInfo");
			ODataMessage odataMessage = (ODataMessage)responseMessage;
			Stream stream = odataMessage.GetStream();
			return this.DetectPayloadKindImplementation(stream, odataMessage, true, detectionInfo);
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x000285AC File Offset: 0x000267AC
		internal override IEnumerable<ODataPayloadKind> DetectPayloadKind(IODataRequestMessage requestMessage, ODataPayloadKindDetectionInfo detectionInfo)
		{
			ExceptionUtils.CheckArgumentNotNull<IODataRequestMessage>(requestMessage, "requestMessage");
			ExceptionUtils.CheckArgumentNotNull<ODataPayloadKindDetectionInfo>(detectionInfo, "detectionInfo");
			ODataMessage odataMessage = (ODataMessage)requestMessage;
			Stream stream = odataMessage.GetStream();
			return this.DetectPayloadKindImplementation(stream, odataMessage, false, detectionInfo);
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x000285E8 File Offset: 0x000267E8
		internal override ODataInputContext CreateInputContext(ODataPayloadKind readerPayloadKind, ODataMessage message, MediaType contentType, Encoding encoding, ODataMessageReaderSettings messageReaderSettings, ODataVersion version, bool readingResponse, IEdmModel model, IODataUrlResolver urlResolver, object payloadKindDetectionFormatState)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataMessage>(message, "message");
			ExceptionUtils.CheckArgumentNotNull<ODataMessageReaderSettings>(messageReaderSettings, "messageReaderSettings");
			Stream stream = message.GetStream();
			return new ODataJsonLightInputContext(this, stream, contentType, encoding, messageReaderSettings, version, readingResponse, true, model, urlResolver, (ODataJsonLightPayloadKindDetectionState)payloadKindDetectionFormatState);
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x00028630 File Offset: 0x00026830
		internal override ODataOutputContext CreateOutputContext(ODataMessage message, MediaType mediaType, Encoding encoding, ODataMessageWriterSettings messageWriterSettings, bool writingResponse, IEdmModel model, IODataUrlResolver urlResolver)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataMessage>(message, "message");
			ExceptionUtils.CheckArgumentNotNull<ODataMessageWriterSettings>(messageWriterSettings, "messageWriterSettings");
			Stream stream = message.GetStream();
			return new ODataJsonLightOutputContext(this, stream, mediaType, encoding, messageWriterSettings, writingResponse, true, model, urlResolver);
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x00028698 File Offset: 0x00026898
		internal override Task<IEnumerable<ODataPayloadKind>> DetectPayloadKindAsync(IODataResponseMessageAsync responseMessage, ODataPayloadKindDetectionInfo detectionInfo)
		{
			ExceptionUtils.CheckArgumentNotNull<IODataResponseMessageAsync>(responseMessage, "responseMessage");
			ExceptionUtils.CheckArgumentNotNull<ODataPayloadKindDetectionInfo>(detectionInfo, "detectionInfo");
			ODataMessage message = (ODataMessage)responseMessage;
			return message.GetStreamAsync().FollowOnSuccessWithTask((Task<Stream> streamTask) => this.DetectPayloadKindImplementationAsync(streamTask.Result, message, true, detectionInfo));
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x00028724 File Offset: 0x00026924
		internal override Task<IEnumerable<ODataPayloadKind>> DetectPayloadKindAsync(IODataRequestMessageAsync requestMessage, ODataPayloadKindDetectionInfo detectionInfo)
		{
			ExceptionUtils.CheckArgumentNotNull<IODataRequestMessageAsync>(requestMessage, "requestMessage");
			ExceptionUtils.CheckArgumentNotNull<ODataPayloadKindDetectionInfo>(detectionInfo, "detectionInfo");
			ODataMessage message = (ODataMessage)requestMessage;
			return message.GetStreamAsync().FollowOnSuccessWithTask((Task<Stream> streamTask) => this.DetectPayloadKindImplementationAsync(streamTask.Result, message, false, detectionInfo));
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x000287E4 File Offset: 0x000269E4
		internal override Task<ODataInputContext> CreateInputContextAsync(ODataPayloadKind readerPayloadKind, ODataMessage message, MediaType contentType, Encoding encoding, ODataMessageReaderSettings messageReaderSettings, ODataVersion version, bool readingResponse, IEdmModel model, IODataUrlResolver urlResolver, object payloadKindDetectionFormatState)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataMessage>(message, "message");
			ExceptionUtils.CheckArgumentNotNull<ODataMessageReaderSettings>(messageReaderSettings, "messageReaderSettings");
			return message.GetStreamAsync().FollowOnSuccessWith((Task<Stream> streamTask) => new ODataJsonLightInputContext(this, streamTask.Result, contentType, encoding, messageReaderSettings, version, readingResponse, false, model, urlResolver, (ODataJsonLightPayloadKindDetectionState)payloadKindDetectionFormatState));
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x000288BC File Offset: 0x00026ABC
		internal override Task<ODataOutputContext> CreateOutputContextAsync(ODataMessage message, MediaType mediaType, Encoding encoding, ODataMessageWriterSettings messageWriterSettings, bool writingResponse, IEdmModel model, IODataUrlResolver urlResolver)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataMessage>(message, "message");
			ExceptionUtils.CheckArgumentNotNull<ODataMessageWriterSettings>(messageWriterSettings, "messageWriterSettings");
			return message.GetStreamAsync().FollowOnSuccessWith((Task<Stream> streamTask) => new ODataJsonLightOutputContext(this, streamTask.Result, mediaType, encoding, messageWriterSettings, writingResponse, false, model, urlResolver));
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x00028938 File Offset: 0x00026B38
		private IEnumerable<ODataPayloadKind> DetectPayloadKindImplementation(Stream messageStream, ODataMessage message, bool readingResponse, ODataPayloadKindDetectionInfo detectionInfo)
		{
			IEnumerable<ODataPayloadKind> result;
			using (ODataJsonLightInputContext odataJsonLightInputContext = new ODataJsonLightInputContext(this, messageStream, detectionInfo.ContentType, detectionInfo.GetEncoding(), detectionInfo.MessageReaderSettings, ODataVersion.V3, readingResponse, true, detectionInfo.Model, null, null))
			{
				result = odataJsonLightInputContext.DetectPayloadKind(detectionInfo);
			}
			return result;
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x000289AC File Offset: 0x00026BAC
		private Task<IEnumerable<ODataPayloadKind>> DetectPayloadKindImplementationAsync(Stream messageStream, ODataMessage message, bool readingResponse, ODataPayloadKindDetectionInfo detectionInfo)
		{
			ODataJsonLightInputContext jsonLightInputContext = new ODataJsonLightInputContext(this, messageStream, detectionInfo.ContentType, detectionInfo.GetEncoding(), detectionInfo.MessageReaderSettings, ODataVersion.V3, readingResponse, false, detectionInfo.Model, null, null);
			return jsonLightInputContext.DetectPayloadKindAsync(detectionInfo).FollowAlwaysWith(delegate(Task<IEnumerable<ODataPayloadKind>> t)
			{
				jsonLightInputContext.Dispose();
			});
		}
	}
}
