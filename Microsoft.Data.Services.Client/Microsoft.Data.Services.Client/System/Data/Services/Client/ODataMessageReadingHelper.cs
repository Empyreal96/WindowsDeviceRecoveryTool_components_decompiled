using System;
using System.Xml;
using Microsoft.Data.Edm;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x02000077 RID: 119
	internal class ODataMessageReadingHelper
	{
		// Token: 0x060003F9 RID: 1017 RVA: 0x00011020 File Offset: 0x0000F220
		internal ODataMessageReadingHelper(ResponseInfo responseInfo)
		{
			this.responseInfo = responseInfo;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x00011030 File Offset: 0x0000F230
		internal ODataMessageReaderSettings CreateSettings(Func<ODataEntry, XmlReader, Uri, XmlReader> entryXmlCustomizer)
		{
			ODataMessageReaderSettings odataMessageReaderSettings = new ODataMessageReaderSettings();
			if (!this.responseInfo.ResponsePipeline.HasAtomReadingEntityHandlers)
			{
				entryXmlCustomizer = null;
			}
			Func<IEdmType, string, IEdmType> typeResolver = new Func<IEdmType, string, IEdmType>(this.responseInfo.TypeResolver.ResolveWireTypeName);
			if (this.responseInfo.Context.Format.ServiceModel != null)
			{
				typeResolver = null;
			}
			odataMessageReaderSettings.EnableWcfDataServicesClientBehavior(typeResolver, this.responseInfo.DataNamespace, UriUtil.UriToString(this.responseInfo.TypeScheme), entryXmlCustomizer);
			odataMessageReaderSettings.BaseUri = this.responseInfo.BaseUriResolver.BaseUriOrNull;
			odataMessageReaderSettings.UndeclaredPropertyBehaviorKinds = ODataUndeclaredPropertyBehaviorKinds.ReportUndeclaredLinkProperty;
			odataMessageReaderSettings.MaxProtocolVersion = CommonUtil.ConvertToODataVersion(this.responseInfo.MaxProtocolVersion);
			if (this.responseInfo.IgnoreMissingProperties)
			{
				odataMessageReaderSettings.UndeclaredPropertyBehaviorKinds |= ODataUndeclaredPropertyBehaviorKinds.IgnoreUndeclaredValueProperty;
			}
			CommonUtil.SetDefaultMessageQuotas(odataMessageReaderSettings.MessageQuotas);
			this.responseInfo.ResponsePipeline.ExecuteReaderSettingsConfiguration(odataMessageReaderSettings);
			return odataMessageReaderSettings;
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00011116 File Offset: 0x0000F316
		internal ODataMessageReader CreateReader(IODataResponseMessage responseMessage, ODataMessageReaderSettings settings)
		{
			this.responseInfo.Context.Format.ValidateCanReadResponseFormat(responseMessage);
			return new ODataMessageReader(responseMessage, settings, this.responseInfo.TypeResolver.ReaderModel);
		}

		// Token: 0x040002BE RID: 702
		private readonly ResponseInfo responseInfo;
	}
}
