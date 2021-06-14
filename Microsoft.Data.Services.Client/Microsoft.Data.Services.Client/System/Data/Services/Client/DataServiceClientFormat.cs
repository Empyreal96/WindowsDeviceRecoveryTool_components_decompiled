using System;
using System.Data.Services.Common;
using System.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x02000059 RID: 89
	public sealed class DataServiceClientFormat
	{
		// Token: 0x060002E7 RID: 743 RVA: 0x0000DAF4 File Offset: 0x0000BCF4
		internal DataServiceClientFormat(DataServiceContext context)
		{
			this.ODataFormat = ODataFormat.Atom;
			this.context = context;
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x0000DB0E File Offset: 0x0000BD0E
		// (set) Token: 0x060002E9 RID: 745 RVA: 0x0000DB16 File Offset: 0x0000BD16
		public ODataFormat ODataFormat { get; private set; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002EA RID: 746 RVA: 0x0000DB1F File Offset: 0x0000BD1F
		// (set) Token: 0x060002EB RID: 747 RVA: 0x0000DB27 File Offset: 0x0000BD27
		public Func<IEdmModel> LoadServiceModel { get; set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060002EC RID: 748 RVA: 0x0000DB30 File Offset: 0x0000BD30
		internal bool UsingAtom
		{
			get
			{
				return this.ODataFormat == ODataFormat.Atom;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060002ED RID: 749 RVA: 0x0000DB3F File Offset: 0x0000BD3F
		internal ODataFormat UriLiteralFormat
		{
			get
			{
				if (!this.UsingAtom)
				{
					return ODataFormat.Json;
				}
				return ODataFormat.VerboseJson;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0000DB54 File Offset: 0x0000BD54
		// (set) Token: 0x060002EF RID: 751 RVA: 0x0000DB5C File Offset: 0x0000BD5C
		internal IEdmModel ServiceModel { get; private set; }

		// Token: 0x060002F0 RID: 752 RVA: 0x0000DB68 File Offset: 0x0000BD68
		public void UseJson(IEdmModel serviceModel)
		{
			Util.CheckArgumentNull<IEdmModel>(serviceModel, "serviceModel");
			if (this.context.HasAtomEventHandlers)
			{
				throw new InvalidOperationException(Strings.DataServiceClientFormat_AtomEventsOnlySupportedWithAtomFormat);
			}
			if (this.context.MaxProtocolVersion < DataServiceProtocolVersion.V3)
			{
				throw new InvalidOperationException(Strings.DataServiceClientFormat_JsonUnsupportedForLessThanV3);
			}
			this.ODataFormat = ODataFormat.Json;
			this.ServiceModel = serviceModel;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000DBC4 File Offset: 0x0000BDC4
		public void UseJson()
		{
			IEdmModel edmModel = null;
			if (this.LoadServiceModel != null)
			{
				edmModel = this.LoadServiceModel();
			}
			if (edmModel == null)
			{
				throw new InvalidOperationException(Strings.DataServiceClientFormat_LoadServiceModelRequired);
			}
			this.UseJson(edmModel);
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000DBFC File Offset: 0x0000BDFC
		public void UseAtom()
		{
			this.ODataFormat = ODataFormat.Atom;
			this.ServiceModel = null;
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000DC10 File Offset: 0x0000BE10
		internal void SetRequestAcceptHeader(HeaderCollection headers)
		{
			this.SetAcceptHeaderAndCharset(headers, this.ChooseMediaType("application/atom+xml,application/xml", false));
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000DC25 File Offset: 0x0000BE25
		internal void SetRequestAcceptHeaderForQuery(HeaderCollection headers, QueryComponents components)
		{
			this.SetAcceptHeaderAndCharset(headers, this.ChooseMediaType("application/atom+xml,application/xml", components.HasSelectQueryOption));
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000DC3F File Offset: 0x0000BE3F
		internal void SetRequestAcceptHeaderForStream(HeaderCollection headers)
		{
			this.SetAcceptHeaderAndCharset(headers, "*/*");
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000DC4D File Offset: 0x0000BE4D
		internal void SetRequestAcceptHeaderForCount(HeaderCollection headers)
		{
			this.SetAcceptHeaderAndCharset(headers, "text/plain");
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000DC5B File Offset: 0x0000BE5B
		internal void SetRequestAcceptHeaderForBatch(HeaderCollection headers)
		{
			this.SetAcceptHeaderAndCharset(headers, "multipart/mixed");
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000DC69 File Offset: 0x0000BE69
		internal void SetRequestContentTypeForEntry(HeaderCollection headers)
		{
			this.SetRequestContentTypeHeader(headers, this.ChooseMediaType("application/atom+xml", false));
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000DC7E File Offset: 0x0000BE7E
		internal void SetRequestContentTypeForOperationParameters(HeaderCollection headers)
		{
			this.SetRequestContentTypeHeader(headers, this.ChooseMediaType("application/json;odata=verbose", false));
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000DC93 File Offset: 0x0000BE93
		internal void SetRequestContentTypeForLinks(HeaderCollection headers)
		{
			this.SetRequestContentTypeHeader(headers, this.ChooseMediaType("application/xml", false));
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000DCA8 File Offset: 0x0000BEA8
		internal void ValidateCanWriteRequestFormat(IODataRequestMessage requestMessage, bool isParameterPayload)
		{
			string header = requestMessage.GetHeader("Content-Type");
			this.ValidateContentType(header, isParameterPayload);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000DCCC File Offset: 0x0000BECC
		internal void ValidateCanReadResponseFormat(IODataResponseMessage responseMessage)
		{
			string header = responseMessage.GetHeader("Content-Type");
			this.ValidateContentType(header, false);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000DCED File Offset: 0x0000BEED
		private static void ThrowInvalidOperationExceptionForJsonLightWithoutModel()
		{
			throw new InvalidOperationException(Strings.DataServiceClientFormat_ValidServiceModelRequiredForJson);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000DCF9 File Offset: 0x0000BEF9
		private static void ThrowNotSupportedExceptionForJsonVerbose(string contentType)
		{
			throw new NotSupportedException(Strings.DataServiceClientFormat_JsonVerboseUnsupported(contentType));
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000DD30 File Offset: 0x0000BF30
		private void ValidateContentType(string contentType, bool isParameterPayload)
		{
			if (string.IsNullOrEmpty(contentType))
			{
				return;
			}
			string value;
			ContentTypeUtil.MediaParameter[] array = ContentTypeUtil.ReadContentType(contentType, out value);
			if ("application/json".Equals(value, StringComparison.OrdinalIgnoreCase))
			{
				if (this.context.MaxProtocolVersion >= DataServiceProtocolVersion.V3)
				{
					if (array != null)
					{
						if (array.Any((ContentTypeUtil.MediaParameter p) => p.Name.Equals("odata", StringComparison.OrdinalIgnoreCase) && p.Value.Equals("verbose", StringComparison.OrdinalIgnoreCase)))
						{
							goto IL_56;
						}
					}
					if (this.ServiceModel == null)
					{
						DataServiceClientFormat.ThrowInvalidOperationExceptionForJsonLightWithoutModel();
						return;
					}
					return;
				}
				IL_56:
				if (!isParameterPayload)
				{
					DataServiceClientFormat.ThrowNotSupportedExceptionForJsonVerbose(contentType);
					return;
				}
			}
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000DDAA File Offset: 0x0000BFAA
		private void SetRequestContentTypeHeader(HeaderCollection headers, string mediaType)
		{
			if (mediaType == "application/json;odata=minimalmetadata")
			{
				headers.SetRequestVersion(Util.DataServiceVersion3, this.context.MaxProtocolVersionAsVersion);
			}
			headers.SetHeaderIfUnset("Content-Type", mediaType);
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000DDDB File Offset: 0x0000BFDB
		private void SetAcceptHeaderAndCharset(HeaderCollection headers, string mediaType)
		{
			headers.SetHeaderIfUnset("Accept", mediaType);
			headers.SetHeaderIfUnset("Accept-Charset", "UTF-8");
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000DDF9 File Offset: 0x0000BFF9
		private string ChooseMediaType(string valueIfUsingAtom, bool hasSelectQueryOption)
		{
			if (this.UsingAtom)
			{
				return valueIfUsingAtom;
			}
			if (hasSelectQueryOption)
			{
				return "application/json;odata=fullmetadata";
			}
			return "application/json;odata=minimalmetadata";
		}

		// Token: 0x04000268 RID: 616
		private const string MimeApplicationAtom = "application/atom+xml";

		// Token: 0x04000269 RID: 617
		private const string MimeApplicationJson = "application/json";

		// Token: 0x0400026A RID: 618
		private const string MimeApplicationJsonODataLight = "application/json;odata=minimalmetadata";

		// Token: 0x0400026B RID: 619
		private const string MimeApplicationJsonODataLightWithAllMetadata = "application/json;odata=fullmetadata";

		// Token: 0x0400026C RID: 620
		private const string MimeApplicationJsonODataVerbose = "application/json;odata=verbose";

		// Token: 0x0400026D RID: 621
		private const string MimeODataParameterVerboseValue = "verbose";

		// Token: 0x0400026E RID: 622
		private const string MimeMultiPartMixed = "multipart/mixed";

		// Token: 0x0400026F RID: 623
		private const string MimeApplicationXml = "application/xml";

		// Token: 0x04000270 RID: 624
		private const string MimeApplicationAtomOrXml = "application/atom+xml,application/xml";

		// Token: 0x04000271 RID: 625
		private const string Utf8Encoding = "UTF-8";

		// Token: 0x04000272 RID: 626
		private const string HttpAcceptCharset = "Accept-Charset";

		// Token: 0x04000273 RID: 627
		private readonly DataServiceContext context;
	}
}
