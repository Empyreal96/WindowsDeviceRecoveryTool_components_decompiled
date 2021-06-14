using System;

namespace System.Data.Services.Client
{
	// Token: 0x02000016 RID: 22
	internal static class XmlConstants
	{
		// Token: 0x0400001F RID: 31
		internal const string ClrServiceInitializationMethodName = "InitializeService";

		// Token: 0x04000020 RID: 32
		internal const string HttpContentID = "Content-ID";

		// Token: 0x04000021 RID: 33
		internal const string HttpContentLength = "Content-Length";

		// Token: 0x04000022 RID: 34
		internal const string HttpContentType = "Content-Type";

		// Token: 0x04000023 RID: 35
		internal const string HttpContentDisposition = "Content-Disposition";

		// Token: 0x04000024 RID: 36
		internal const string HttpDataServiceVersion = "DataServiceVersion";

		// Token: 0x04000025 RID: 37
		internal const string HttpMaxDataServiceVersion = "MaxDataServiceVersion";

		// Token: 0x04000026 RID: 38
		internal const string HttpPrefer = "Prefer";

		// Token: 0x04000027 RID: 39
		internal const string HttpPreferReturnNoContent = "return-no-content";

		// Token: 0x04000028 RID: 40
		internal const string HttpPreferReturnContent = "return-content";

		// Token: 0x04000029 RID: 41
		internal const string HttpCacheControlNoCache = "no-cache";

		// Token: 0x0400002A RID: 42
		internal const string HttpCharsetParameter = "charset";

		// Token: 0x0400002B RID: 43
		internal const string HttpMethodGet = "GET";

		// Token: 0x0400002C RID: 44
		internal const string HttpMethodPost = "POST";

		// Token: 0x0400002D RID: 45
		internal const string HttpMethodPut = "PUT";

		// Token: 0x0400002E RID: 46
		internal const string HttpMethodDelete = "DELETE";

		// Token: 0x0400002F RID: 47
		internal const string HttpMethodMerge = "MERGE";

		// Token: 0x04000030 RID: 48
		internal const string HttpMethodPatch = "PATCH";

		// Token: 0x04000031 RID: 49
		internal const string HttpQueryStringExpand = "$expand";

		// Token: 0x04000032 RID: 50
		internal const string HttpQueryStringFilter = "$filter";

		// Token: 0x04000033 RID: 51
		internal const string HttpQueryStringOrderBy = "$orderby";

		// Token: 0x04000034 RID: 52
		internal const string HttpQueryStringSkip = "$skip";

		// Token: 0x04000035 RID: 53
		internal const string HttpQueryStringTop = "$top";

		// Token: 0x04000036 RID: 54
		internal const string HttpQueryStringInlineCount = "$inlinecount";

		// Token: 0x04000037 RID: 55
		internal const string HttpQueryStringSkipToken = "$skiptoken";

		// Token: 0x04000038 RID: 56
		internal const string SkipTokenPropertyPrefix = "SkipTokenProperty";

		// Token: 0x04000039 RID: 57
		internal const string HttpQueryStringValueCount = "$count";

		// Token: 0x0400003A RID: 58
		internal const string HttpQueryStringSelect = "$select";

		// Token: 0x0400003B RID: 59
		internal const string HttpQueryStringFormat = "$format";

		// Token: 0x0400003C RID: 60
		internal const string HttpQueryStringCallback = "$callback";

		// Token: 0x0400003D RID: 61
		internal const string HttpQValueParameter = "q";

		// Token: 0x0400003E RID: 62
		internal const string HttpXMethod = "X-HTTP-Method";

		// Token: 0x0400003F RID: 63
		internal const string HttpRequestAccept = "Accept";

		// Token: 0x04000040 RID: 64
		internal const string HttpRequestAcceptCharset = "Accept-Charset";

		// Token: 0x04000041 RID: 65
		internal const string HttpRequestIfMatch = "If-Match";

		// Token: 0x04000042 RID: 66
		internal const string HttpRequestIfNoneMatch = "If-None-Match";

		// Token: 0x04000043 RID: 67
		internal const string HttpUserAgent = "User-Agent";

		// Token: 0x04000044 RID: 68
		internal const string HttpMultipartBoundary = "boundary";

		// Token: 0x04000045 RID: 69
		internal const string XContentTypeOptions = "X-Content-Type-Options";

		// Token: 0x04000046 RID: 70
		internal const string XContentTypeOptionNoSniff = "nosniff";

		// Token: 0x04000047 RID: 71
		internal const string HttpMultipartBoundaryBatch = "batch";

		// Token: 0x04000048 RID: 72
		internal const string HttpMultipartBoundaryChangeSet = "changeset";

		// Token: 0x04000049 RID: 73
		internal const string HttpResponseAllow = "Allow";

		// Token: 0x0400004A RID: 74
		internal const string HttpResponseCacheControl = "Cache-Control";

		// Token: 0x0400004B RID: 75
		internal const string HttpResponseETag = "ETag";

		// Token: 0x0400004C RID: 76
		internal const string HttpResponseLocation = "Location";

		// Token: 0x0400004D RID: 77
		internal const string HttpDataServiceId = "DataServiceId";

		// Token: 0x0400004E RID: 78
		internal const string HttpResponseStatusCode = "Status-Code";

		// Token: 0x0400004F RID: 79
		internal const string HttpMultipartBoundaryBatchResponse = "batchresponse";

		// Token: 0x04000050 RID: 80
		internal const string HttpMultipartBoundaryChangesetResponse = "changesetresponse";

		// Token: 0x04000051 RID: 81
		internal const string HttpContentTransferEncoding = "Content-Transfer-Encoding";

		// Token: 0x04000052 RID: 82
		internal const string HttpVersionInBatching = "HTTP/1.1";

		// Token: 0x04000053 RID: 83
		internal const string HttpAnyETag = "*";

		// Token: 0x04000054 RID: 84
		internal const string HttpWeakETagPrefix = "W/\"";

		// Token: 0x04000055 RID: 85
		internal const string HttpAccept = "Accept";

		// Token: 0x04000056 RID: 86
		internal const string HttpAcceptCharset = "Accept-Charset";

		// Token: 0x04000057 RID: 87
		internal const string HttpCookie = "Cookie";

		// Token: 0x04000058 RID: 88
		internal const string HttpSlug = "Slug";

		// Token: 0x04000059 RID: 89
		internal const string MimeAny = "*/*";

		// Token: 0x0400005A RID: 90
		internal const string MimeApplicationOctetStream = "application/octet-stream";

		// Token: 0x0400005B RID: 91
		internal const string MimeApplicationType = "application";

		// Token: 0x0400005C RID: 92
		internal const string MimeJsonSubType = "json";

		// Token: 0x0400005D RID: 93
		internal const string MimeXmlSubType = "xml";

		// Token: 0x0400005E RID: 94
		internal const string MimeODataParameterName = "odata";

		// Token: 0x0400005F RID: 95
		internal const string MimeMultiPartMixed = "multipart/mixed";

		// Token: 0x04000060 RID: 96
		internal const string MimeTextPlain = "text/plain";

		// Token: 0x04000061 RID: 97
		internal const string MimeTextType = "text";

		// Token: 0x04000062 RID: 98
		internal const string MimeTextXml = "text/xml";

		// Token: 0x04000063 RID: 99
		internal const string BatchRequestContentTransferEncoding = "binary";

		// Token: 0x04000064 RID: 100
		internal const string MimeTypeUtf8Encoding = ";charset=UTF-8";

		// Token: 0x04000065 RID: 101
		internal const string UriHttpAbsolutePrefix = "http://host";

		// Token: 0x04000066 RID: 102
		internal const string UriMetadataSegment = "$metadata";

		// Token: 0x04000067 RID: 103
		internal const string UriValueSegment = "$value";

		// Token: 0x04000068 RID: 104
		internal const string UriBatchSegment = "$batch";

		// Token: 0x04000069 RID: 105
		internal const string UriLinkSegment = "$links";

		// Token: 0x0400006A RID: 106
		internal const string UriCountSegment = "$count";

		// Token: 0x0400006B RID: 107
		internal const string UriRowCountAllOption = "allpages";

		// Token: 0x0400006C RID: 108
		internal const string UriRowCountOffOption = "none";

		// Token: 0x0400006D RID: 109
		internal const string AnyMethodName = "any";

		// Token: 0x0400006E RID: 110
		internal const string AllMethodName = "all";

		// Token: 0x0400006F RID: 111
		internal const string ImplicitFilterParameter = "$it";

		// Token: 0x04000070 RID: 112
		internal const string WcfBinaryElementName = "Binary";

		// Token: 0x04000071 RID: 113
		internal const string AtomNamespacePrefix = "atom";

		// Token: 0x04000072 RID: 114
		internal const string AtomContentElementName = "content";

		// Token: 0x04000073 RID: 115
		internal const string AtomEntryElementName = "entry";

		// Token: 0x04000074 RID: 116
		internal const string AtomFeedElementName = "feed";

		// Token: 0x04000075 RID: 117
		internal const string AtomAuthorElementName = "author";

		// Token: 0x04000076 RID: 118
		internal const string AtomContributorElementName = "contributor";

		// Token: 0x04000077 RID: 119
		internal const string AtomCategoryElementName = "category";

		// Token: 0x04000078 RID: 120
		internal const string AtomLinkElementName = "link";

		// Token: 0x04000079 RID: 121
		internal const string AtomCategorySchemeAttributeName = "scheme";

		// Token: 0x0400007A RID: 122
		internal const string AtomCategoryTermAttributeName = "term";

		// Token: 0x0400007B RID: 123
		internal const string AtomIdElementName = "id";

		// Token: 0x0400007C RID: 124
		internal const string AtomLinkRelationAttributeName = "rel";

		// Token: 0x0400007D RID: 125
		internal const string AtomContentSrcAttributeName = "src";

		// Token: 0x0400007E RID: 126
		internal const string AtomLinkNextAttributeString = "next";

		// Token: 0x0400007F RID: 127
		internal const string MetadataAttributeEpmContentKind = "FC_ContentKind";

		// Token: 0x04000080 RID: 128
		internal const string MetadataAttributeEpmKeepInContent = "FC_KeepInContent";

		// Token: 0x04000081 RID: 129
		internal const string MetadataAttributeEpmNsPrefix = "FC_NsPrefix";

		// Token: 0x04000082 RID: 130
		internal const string MetadataAttributeEpmNsUri = "FC_NsUri";

		// Token: 0x04000083 RID: 131
		internal const string MetadataAttributeEpmTargetPath = "FC_TargetPath";

		// Token: 0x04000084 RID: 132
		internal const string MetadataAttributeEpmSourcePath = "FC_SourcePath";

		// Token: 0x04000085 RID: 133
		internal const string SyndAuthorEmail = "SyndicationAuthorEmail";

		// Token: 0x04000086 RID: 134
		internal const string SyndAuthorName = "SyndicationAuthorName";

		// Token: 0x04000087 RID: 135
		internal const string SyndAuthorUri = "SyndicationAuthorUri";

		// Token: 0x04000088 RID: 136
		internal const string SyndPublished = "SyndicationPublished";

		// Token: 0x04000089 RID: 137
		internal const string SyndRights = "SyndicationRights";

		// Token: 0x0400008A RID: 138
		internal const string SyndSummary = "SyndicationSummary";

		// Token: 0x0400008B RID: 139
		internal const string SyndTitle = "SyndicationTitle";

		// Token: 0x0400008C RID: 140
		internal const string AtomUpdatedElementName = "updated";

		// Token: 0x0400008D RID: 141
		internal const string SyndContributorEmail = "SyndicationContributorEmail";

		// Token: 0x0400008E RID: 142
		internal const string SyndContributorName = "SyndicationContributorName";

		// Token: 0x0400008F RID: 143
		internal const string SyndContributorUri = "SyndicationContributorUri";

		// Token: 0x04000090 RID: 144
		internal const string SyndUpdated = "SyndicationUpdated";

		// Token: 0x04000091 RID: 145
		internal const string SyndContentKindPlaintext = "text";

		// Token: 0x04000092 RID: 146
		internal const string SyndContentKindHtml = "html";

		// Token: 0x04000093 RID: 147
		internal const string SyndContentKindXHtml = "xhtml";

		// Token: 0x04000094 RID: 148
		internal const string AtomHRefAttributeName = "href";

		// Token: 0x04000095 RID: 149
		internal const string AtomHRefLangAttributeName = "hreflang";

		// Token: 0x04000096 RID: 150
		internal const string AtomSummaryElementName = "summary";

		// Token: 0x04000097 RID: 151
		internal const string AtomNameElementName = "name";

		// Token: 0x04000098 RID: 152
		internal const string AtomEmailElementName = "email";

		// Token: 0x04000099 RID: 153
		internal const string AtomUriElementName = "uri";

		// Token: 0x0400009A RID: 154
		internal const string AtomPublishedElementName = "published";

		// Token: 0x0400009B RID: 155
		internal const string AtomRightsElementName = "rights";

		// Token: 0x0400009C RID: 156
		internal const string AtomPublishingCollectionElementName = "collection";

		// Token: 0x0400009D RID: 157
		internal const string AtomPublishingServiceElementName = "service";

		// Token: 0x0400009E RID: 158
		internal const string AtomPublishingWorkspaceDefaultValue = "Default";

		// Token: 0x0400009F RID: 159
		internal const string AtomPublishingWorkspaceElementName = "workspace";

		// Token: 0x040000A0 RID: 160
		internal const string AtomTitleElementName = "title";

		// Token: 0x040000A1 RID: 161
		internal const string AtomTypeAttributeName = "type";

		// Token: 0x040000A2 RID: 162
		internal const string AtomSelfRelationAttributeValue = "self";

		// Token: 0x040000A3 RID: 163
		internal const string AtomEditRelationAttributeValue = "edit";

		// Token: 0x040000A4 RID: 164
		internal const string AtomEditMediaRelationAttributeValue = "edit-media";

		// Token: 0x040000A5 RID: 165
		internal const string AtomAlternateRelationAttributeValue = "alternate";

		// Token: 0x040000A6 RID: 166
		internal const string AtomRelatedRelationAttributeValue = "related";

		// Token: 0x040000A7 RID: 167
		internal const string AtomEnclosureRelationAttributeValue = "enclosure";

		// Token: 0x040000A8 RID: 168
		internal const string AtomViaRelationAttributeValue = "via";

		// Token: 0x040000A9 RID: 169
		internal const string AtomDescribedByRelationAttributeValue = "describedby";

		// Token: 0x040000AA RID: 170
		internal const string AtomServiceRelationAttributeValue = "service";

		// Token: 0x040000AB RID: 171
		internal const string AtomNullAttributeName = "null";

		// Token: 0x040000AC RID: 172
		internal const string AtomETagAttributeName = "etag";

		// Token: 0x040000AD RID: 173
		internal const string AtomInlineElementName = "inline";

		// Token: 0x040000AE RID: 174
		internal const string AtomPropertiesElementName = "properties";

		// Token: 0x040000AF RID: 175
		internal const string RowCountElement = "count";

		// Token: 0x040000B0 RID: 176
		internal const string XmlCollectionItemElementName = "element";

		// Token: 0x040000B1 RID: 177
		internal const string XmlErrorElementName = "error";

		// Token: 0x040000B2 RID: 178
		internal const string XmlErrorCodeElementName = "code";

		// Token: 0x040000B3 RID: 179
		internal const string XmlErrorInnerElementName = "innererror";

		// Token: 0x040000B4 RID: 180
		internal const string XmlErrorInternalExceptionElementName = "internalexception";

		// Token: 0x040000B5 RID: 181
		internal const string XmlErrorTypeElementName = "type";

		// Token: 0x040000B6 RID: 182
		internal const string XmlErrorStackTraceElementName = "stacktrace";

		// Token: 0x040000B7 RID: 183
		internal const string XmlErrorMessageElementName = "message";

		// Token: 0x040000B8 RID: 184
		internal const string XmlFalseLiteral = "false";

		// Token: 0x040000B9 RID: 185
		internal const string XmlTrueLiteral = "true";

		// Token: 0x040000BA RID: 186
		internal const string XmlBaseAttributeName = "base";

		// Token: 0x040000BB RID: 187
		internal const string XmlLangAttributeName = "lang";

		// Token: 0x040000BC RID: 188
		internal const string XmlSpaceAttributeName = "space";

		// Token: 0x040000BD RID: 189
		internal const string XmlSpacePreserveValue = "preserve";

		// Token: 0x040000BE RID: 190
		internal const string XmlBaseAttributeNameWithPrefix = "xml:base";

		// Token: 0x040000BF RID: 191
		internal const string EdmV1Namespace = "http://schemas.microsoft.com/ado/2006/04/edm";

		// Token: 0x040000C0 RID: 192
		internal const string EdmV1dot1Namespace = "http://schemas.microsoft.com/ado/2007/05/edm";

		// Token: 0x040000C1 RID: 193
		internal const string EdmV1dot2Namespace = "http://schemas.microsoft.com/ado/2008/01/edm";

		// Token: 0x040000C2 RID: 194
		internal const string EdmAnnotationsNamespace = "http://schemas.microsoft.com/ado/2009/02/edm/annotation";

		// Token: 0x040000C3 RID: 195
		internal const string DataWebNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices";

		// Token: 0x040000C4 RID: 196
		internal const string DataWebMetadataNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

		// Token: 0x040000C5 RID: 197
		internal const string DataWebRelatedNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/";

		// Token: 0x040000C6 RID: 198
		internal const string DataWebRelatedLinkNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/relatedlinks/";

		// Token: 0x040000C7 RID: 199
		internal const string DataWebMediaResourceNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/";

		// Token: 0x040000C8 RID: 200
		internal const string DataWebMediaResourceEditNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/edit-media/";

		// Token: 0x040000C9 RID: 201
		internal const string DataWebSchemeNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/scheme";

		// Token: 0x040000CA RID: 202
		internal const string AppNamespace = "http://www.w3.org/2007/app";

		// Token: 0x040000CB RID: 203
		internal const string AtomNamespace = "http://www.w3.org/2005/Atom";

		// Token: 0x040000CC RID: 204
		internal const string XmlnsNamespacePrefix = "xmlns";

		// Token: 0x040000CD RID: 205
		internal const string XmlNamespacePrefix = "xml";

		// Token: 0x040000CE RID: 206
		internal const string DataWebNamespacePrefix = "d";

		// Token: 0x040000CF RID: 207
		internal const string DataWebMetadataNamespacePrefix = "m";

		// Token: 0x040000D0 RID: 208
		internal const string XmlNamespacesNamespace = "http://www.w3.org/2000/xmlns/";

		// Token: 0x040000D1 RID: 209
		internal const string EdmxNamespace = "http://schemas.microsoft.com/ado/2007/06/edmx";

		// Token: 0x040000D2 RID: 210
		internal const string EdmxNamespacePrefix = "edmx";

		// Token: 0x040000D3 RID: 211
		internal const string IanaLinkRelationsNamespace = "http://www.iana.org/assignments/relation/";

		// Token: 0x040000D4 RID: 212
		internal const string EmptyNamespace = "";

		// Token: 0x040000D5 RID: 213
		internal const string Association = "Association";

		// Token: 0x040000D6 RID: 214
		internal const string AssociationSet = "AssociationSet";

		// Token: 0x040000D7 RID: 215
		internal const string ComplexType = "ComplexType";

		// Token: 0x040000D8 RID: 216
		internal const string Dependent = "Dependent";

		// Token: 0x040000D9 RID: 217
		internal const string EdmCollectionTypeName = "Collection";

		// Token: 0x040000DA RID: 218
		internal const string ActualEdmType = "ActualEdmType";

		// Token: 0x040000DB RID: 219
		internal const string EdmTypeRefElementName = "TypeRef";

		// Token: 0x040000DC RID: 220
		internal const string EdmEntitySetAttributeName = "EntitySet";

		// Token: 0x040000DD RID: 221
		internal const string EdmEntitySetPathAttributeName = "EntitySetPath";

		// Token: 0x040000DE RID: 222
		internal const string EdmBindableAttributeName = "Bindable";

		// Token: 0x040000DF RID: 223
		internal const string EdmComposableAttributeName = "Composable";

		// Token: 0x040000E0 RID: 224
		internal const string EdmSideEffectingAttributeName = "SideEffecting";

		// Token: 0x040000E1 RID: 225
		internal const string EdmFunctionImportElementName = "FunctionImport";

		// Token: 0x040000E2 RID: 226
		internal const string EdmModeAttributeName = "Mode";

		// Token: 0x040000E3 RID: 227
		internal const string EdmModeInValue = "In";

		// Token: 0x040000E4 RID: 228
		internal const string EdmParameterElementName = "Parameter";

		// Token: 0x040000E5 RID: 229
		internal const string EdmReturnTypeAttributeName = "ReturnType";

		// Token: 0x040000E6 RID: 230
		internal const string ActualReturnTypeAttributeName = "ActualReturnType";

		// Token: 0x040000E7 RID: 231
		internal const string End = "End";

		// Token: 0x040000E8 RID: 232
		internal const string EntityType = "EntityType";

		// Token: 0x040000E9 RID: 233
		internal const string EntityContainer = "EntityContainer";

		// Token: 0x040000EA RID: 234
		internal const string Key = "Key";

		// Token: 0x040000EB RID: 235
		internal const string NavigationProperty = "NavigationProperty";

		// Token: 0x040000EC RID: 236
		internal const string OnDelete = "OnDelete";

		// Token: 0x040000ED RID: 237
		internal const string Principal = "Principal";

		// Token: 0x040000EE RID: 238
		internal const string Property = "Property";

		// Token: 0x040000EF RID: 239
		internal const string PropertyRef = "PropertyRef";

		// Token: 0x040000F0 RID: 240
		internal const string ReferentialConstraint = "ReferentialConstraint";

		// Token: 0x040000F1 RID: 241
		internal const string Role = "Role";

		// Token: 0x040000F2 RID: 242
		internal const string Schema = "Schema";

		// Token: 0x040000F3 RID: 243
		internal const string EdmxElement = "Edmx";

		// Token: 0x040000F4 RID: 244
		internal const string EdmxDataServicesElement = "DataServices";

		// Token: 0x040000F5 RID: 245
		internal const string EdmxVersion = "Version";

		// Token: 0x040000F6 RID: 246
		internal const string EdmxVersionValue = "1.0";

		// Token: 0x040000F7 RID: 247
		internal const string ActionElementName = "action";

		// Token: 0x040000F8 RID: 248
		internal const string FunctionElementName = "function";

		// Token: 0x040000F9 RID: 249
		internal const string ActionMetadataAttributeName = "metadata";

		// Token: 0x040000FA RID: 250
		internal const string ActionTargetAttributeName = "target";

		// Token: 0x040000FB RID: 251
		internal const string ActionTitleAttributeName = "title";

		// Token: 0x040000FC RID: 252
		internal const string BaseType = "BaseType";

		// Token: 0x040000FD RID: 253
		internal const string EntitySet = "EntitySet";

		// Token: 0x040000FE RID: 254
		internal const string EntitySetPath = "EntitySetPath";

		// Token: 0x040000FF RID: 255
		internal const string FromRole = "FromRole";

		// Token: 0x04000100 RID: 256
		internal const string Abstract = "Abstract";

		// Token: 0x04000101 RID: 257
		internal const string Multiplicity = "Multiplicity";

		// Token: 0x04000102 RID: 258
		internal const string Name = "Name";

		// Token: 0x04000103 RID: 259
		internal const string Namespace = "Namespace";

		// Token: 0x04000104 RID: 260
		internal const string ToRole = "ToRole";

		// Token: 0x04000105 RID: 261
		internal const string Type = "Type";

		// Token: 0x04000106 RID: 262
		internal const string Relationship = "Relationship";

		// Token: 0x04000107 RID: 263
		internal const string Using = "Using";

		// Token: 0x04000108 RID: 264
		internal const string Many = "*";

		// Token: 0x04000109 RID: 265
		internal const string One = "1";

		// Token: 0x0400010A RID: 266
		internal const string ZeroOrOne = "0..1";

		// Token: 0x0400010B RID: 267
		internal const string CsdlNullableAttributeName = "Nullable";

		// Token: 0x0400010C RID: 268
		internal const string CsdlPrecisionAttributeName = "Precision";

		// Token: 0x0400010D RID: 269
		internal const string CsdlScaleAttributeName = "Scale";

		// Token: 0x0400010E RID: 270
		internal const string CsdlMaxLengthAttributeName = "MaxLength";

		// Token: 0x0400010F RID: 271
		internal const string CsdlFixedLengthAttributeName = "FixedLength";

		// Token: 0x04000110 RID: 272
		internal const string CsdlUnicodeAttributeName = "Unicode";

		// Token: 0x04000111 RID: 273
		internal const string CsdlCollationAttributeName = "Collation";

		// Token: 0x04000112 RID: 274
		internal const string CsdlSridAttributeName = "SRID";

		// Token: 0x04000113 RID: 275
		internal const string CsdlConcurrencyAttributeName = "ConcurrencyMode";

		// Token: 0x04000114 RID: 276
		internal const string CsdlDefaultValueAttributeName = "DefaultValue";

		// Token: 0x04000115 RID: 277
		internal const string CsdlMaxLengthAttributeMaxValue = "Max";

		// Token: 0x04000116 RID: 278
		internal const string CsdlStoreGeneratedPattern = "StoreGeneratedPattern";

		// Token: 0x04000117 RID: 279
		internal const string CsdlStoreGeneratedPatternComputed = "Computed";

		// Token: 0x04000118 RID: 280
		internal const string CsdlStoreGeneratedPatternIdentity = "Identity";

		// Token: 0x04000119 RID: 281
		internal const string DataWebMimeTypeAttributeName = "MimeType";

		// Token: 0x0400011A RID: 282
		internal const string DataWebOpenTypeAttributeName = "OpenType";

		// Token: 0x0400011B RID: 283
		internal const string DataWebAccessHasStreamAttribute = "HasStream";

		// Token: 0x0400011C RID: 284
		internal const string DataWebAccessDefaultStreamPropertyValue = "true";

		// Token: 0x0400011D RID: 285
		internal const string IsDefaultEntityContainerAttribute = "IsDefaultEntityContainer";

		// Token: 0x0400011E RID: 286
		internal const string ServiceOperationHttpMethodName = "HttpMethod";

		// Token: 0x0400011F RID: 287
		internal const string UriElementName = "uri";

		// Token: 0x04000120 RID: 288
		internal const string NextElementName = "next";

		// Token: 0x04000121 RID: 289
		internal const string LinkCollectionElementName = "links";

		// Token: 0x04000122 RID: 290
		internal const string JsonError = "error";

		// Token: 0x04000123 RID: 291
		internal const string JsonErrorCode = "code";

		// Token: 0x04000124 RID: 292
		internal const string JsonErrorInner = "innererror";

		// Token: 0x04000125 RID: 293
		internal const string JsonErrorInternalException = "internalexception";

		// Token: 0x04000126 RID: 294
		internal const string JsonErrorMessage = "message";

		// Token: 0x04000127 RID: 295
		internal const string JsonErrorStackTrace = "stacktrace";

		// Token: 0x04000128 RID: 296
		internal const string JsonErrorType = "type";

		// Token: 0x04000129 RID: 297
		internal const string JsonErrorValue = "value";

		// Token: 0x0400012A RID: 298
		internal const string EdmNamespace = "Edm";

		// Token: 0x0400012B RID: 299
		internal const string EdmBinaryTypeName = "Edm.Binary";

		// Token: 0x0400012C RID: 300
		internal const string EdmBooleanTypeName = "Edm.Boolean";

		// Token: 0x0400012D RID: 301
		internal const string EdmByteTypeName = "Edm.Byte";

		// Token: 0x0400012E RID: 302
		internal const string EdmDateTimeTypeName = "Edm.DateTime";

		// Token: 0x0400012F RID: 303
		internal const string EdmDecimalTypeName = "Edm.Decimal";

		// Token: 0x04000130 RID: 304
		internal const string EdmDoubleTypeName = "Edm.Double";

		// Token: 0x04000131 RID: 305
		internal const string EdmGuidTypeName = "Edm.Guid";

		// Token: 0x04000132 RID: 306
		internal const string EdmSingleTypeName = "Edm.Single";

		// Token: 0x04000133 RID: 307
		internal const string EdmSByteTypeName = "Edm.SByte";

		// Token: 0x04000134 RID: 308
		internal const string EdmInt16TypeName = "Edm.Int16";

		// Token: 0x04000135 RID: 309
		internal const string EdmInt32TypeName = "Edm.Int32";

		// Token: 0x04000136 RID: 310
		internal const string EdmInt64TypeName = "Edm.Int64";

		// Token: 0x04000137 RID: 311
		internal const string EdmStringTypeName = "Edm.String";

		// Token: 0x04000138 RID: 312
		internal const string EdmStreamTypeName = "Edm.Stream";

		// Token: 0x04000139 RID: 313
		internal const string CollectionTypeQualifier = "Collection";

		// Token: 0x0400013A RID: 314
		internal const string EdmGeographyTypeName = "Edm.Geography";

		// Token: 0x0400013B RID: 315
		internal const string EdmPointTypeName = "Edm.GeographyPoint";

		// Token: 0x0400013C RID: 316
		internal const string EdmLineStringTypeName = "Edm.GeographyLineString";

		// Token: 0x0400013D RID: 317
		internal const string EdmPolygonTypeName = "Edm.GeographyPolygon";

		// Token: 0x0400013E RID: 318
		internal const string EdmGeographyCollectionTypeName = "Edm.GeographyCollection";

		// Token: 0x0400013F RID: 319
		internal const string EdmMultiPolygonTypeName = "Edm.GeographyMultiPolygon";

		// Token: 0x04000140 RID: 320
		internal const string EdmMultiLineStringTypeName = "Edm.GeographyMultiLineString";

		// Token: 0x04000141 RID: 321
		internal const string EdmMultiPointTypeName = "Edm.GeographyMultiPoint";

		// Token: 0x04000142 RID: 322
		internal const string EdmGeometryTypeName = "Edm.Geometry";

		// Token: 0x04000143 RID: 323
		internal const string EdmGeometryPointTypeName = "Edm.GeometryPoint";

		// Token: 0x04000144 RID: 324
		internal const string EdmGeometryLineStringTypeName = "Edm.GeometryLineString";

		// Token: 0x04000145 RID: 325
		internal const string EdmGeometryPolygonTypeName = "Edm.GeometryPolygon";

		// Token: 0x04000146 RID: 326
		internal const string EdmGeometryCollectionTypeName = "Edm.GeometryCollection";

		// Token: 0x04000147 RID: 327
		internal const string EdmGeometryMultiPolygonTypeName = "Edm.GeometryMultiPolygon";

		// Token: 0x04000148 RID: 328
		internal const string EdmGeometryMultiLineStringTypeName = "Edm.GeometryMultiLineString";

		// Token: 0x04000149 RID: 329
		internal const string EdmGeometryMultiPointTypeName = "Edm.GeometryMultiPoint";

		// Token: 0x0400014A RID: 330
		internal const string EdmTimeTypeName = "Edm.Time";

		// Token: 0x0400014B RID: 331
		internal const string EdmDateTimeOffsetTypeName = "Edm.DateTimeOffset";

		// Token: 0x0400014C RID: 332
		internal const string DataServiceVersion1Dot0 = "1.0";

		// Token: 0x0400014D RID: 333
		internal const string DataServiceVersion2Dot0 = "2.0";

		// Token: 0x0400014E RID: 334
		internal const string DataServiceVersion3Dot0 = "3.0";

		// Token: 0x0400014F RID: 335
		internal const string DataServiceVersionCurrent = "2.0;";

		// Token: 0x04000150 RID: 336
		internal const int DataServiceVersionCurrentMajor = 1;

		// Token: 0x04000151 RID: 337
		internal const int DataServiceVersionCurrentMinor = 0;

		// Token: 0x04000152 RID: 338
		internal const string LiteralPrefixBinary = "binary";

		// Token: 0x04000153 RID: 339
		internal const string LiteralPrefixDateTime = "datetime";

		// Token: 0x04000154 RID: 340
		internal const string LiteralPrefixGuid = "guid";

		// Token: 0x04000155 RID: 341
		internal const string LiteralPrefixGeography = "geography";

		// Token: 0x04000156 RID: 342
		internal const string LiteralPrefixGeometry = "geometry";

		// Token: 0x04000157 RID: 343
		internal const string LiteralPrefixDateTimeOffset = "datetimeoffset";

		// Token: 0x04000158 RID: 344
		internal const string LiteralPrefixTime = "time";

		// Token: 0x04000159 RID: 345
		internal const string LiteralPrefixShortBinary = "X";

		// Token: 0x0400015A RID: 346
		internal const string LiteralSuffixDecimal = "M";

		// Token: 0x0400015B RID: 347
		internal const string LiteralSuffixInt64 = "L";

		// Token: 0x0400015C RID: 348
		internal const string LiteralSuffixSingle = "f";

		// Token: 0x0400015D RID: 349
		internal const string LiteralSuffixDouble = "D";

		// Token: 0x0400015E RID: 350
		internal const string NullLiteralInETag = "null";

		// Token: 0x0400015F RID: 351
		internal const string MicrosoftDataServicesRequestUri = "MicrosoftDataServicesRequestUri";

		// Token: 0x04000160 RID: 352
		internal const string MicrosoftDataServicesRootUri = "MicrosoftDataServicesRootUri";

		// Token: 0x04000161 RID: 353
		internal const string GeoRssNamespace = "http://www.georss.org/georss";

		// Token: 0x04000162 RID: 354
		internal const string GeoRssPrefix = "georss";

		// Token: 0x04000163 RID: 355
		internal const string GmlNamespace = "http://www.opengis.net/gml";

		// Token: 0x04000164 RID: 356
		internal const string GmlPrefix = "gml";

		// Token: 0x04000165 RID: 357
		internal const string GeoRssWhere = "where";

		// Token: 0x04000166 RID: 358
		internal const string GeoRssPoint = "point";

		// Token: 0x04000167 RID: 359
		internal const string GeoRssLine = "line";

		// Token: 0x04000168 RID: 360
		internal const string GmlPosition = "pos";

		// Token: 0x04000169 RID: 361
		internal const string GmlPositionList = "posList";

		// Token: 0x0400016A RID: 362
		internal const string GmlLineString = "LineString";
	}
}
