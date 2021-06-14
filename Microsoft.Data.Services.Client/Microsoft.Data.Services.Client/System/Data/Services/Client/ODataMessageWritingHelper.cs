using System;
using System.Xml;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x02000078 RID: 120
	internal class ODataMessageWritingHelper
	{
		// Token: 0x060003FC RID: 1020 RVA: 0x00011145 File Offset: 0x0000F345
		internal ODataMessageWritingHelper(RequestInfo requestInfo)
		{
			this.requestInfo = requestInfo;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00011154 File Offset: 0x0000F354
		internal ODataMessageWriterSettings CreateSettings(Func<ODataEntry, XmlWriter, XmlWriter> startEntryXmlCustomizationCallback, Action<ODataEntry, XmlWriter, XmlWriter> endEntryXmlCustomizationCallback, bool isBatchPartRequest)
		{
			ODataMessageWriterSettings odataMessageWriterSettings = new ODataMessageWriterSettings
			{
				CheckCharacters = false,
				Indent = false,
				DisableMessageStreamDisposal = !isBatchPartRequest
			};
			CommonUtil.SetDefaultMessageQuotas(odataMessageWriterSettings.MessageQuotas);
			if (!this.requestInfo.HasWritingEventHandlers)
			{
				startEntryXmlCustomizationCallback = null;
				endEntryXmlCustomizationCallback = null;
			}
			odataMessageWriterSettings.EnableWcfDataServicesClientBehavior(startEntryXmlCustomizationCallback, endEntryXmlCustomizationCallback, this.requestInfo.DataNamespace, this.requestInfo.TypeScheme.AbsoluteUri);
			this.requestInfo.Configurations.RequestPipeline.ExecuteWriterSettingsConfiguration(odataMessageWriterSettings);
			return odataMessageWriterSettings;
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x000111D9 File Offset: 0x0000F3D9
		internal ODataMessageWriter CreateWriter(IODataRequestMessage requestMessage, ODataMessageWriterSettings writerSettings, bool isParameterPayload)
		{
			this.requestInfo.Context.Format.ValidateCanWriteRequestFormat(requestMessage, isParameterPayload);
			return new ODataMessageWriter(requestMessage, writerSettings, this.requestInfo.Model);
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00011204 File Offset: 0x0000F404
		internal ODataRequestMessageWrapper CreateRequestMessage(BuildingRequestEventArgs requestMessageArgs)
		{
			return ODataRequestMessageWrapper.CreateRequestMessageWrapper(requestMessageArgs, this.requestInfo);
		}

		// Token: 0x040002BF RID: 703
		private readonly RequestInfo requestInfo;
	}
}
