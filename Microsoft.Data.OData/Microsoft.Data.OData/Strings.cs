using System;

namespace Microsoft.Data.OData
{
	// Token: 0x020002B2 RID: 690
	internal static class Strings
	{
		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06001793 RID: 6035 RVA: 0x000543C7 File Offset: 0x000525C7
		internal static string ExceptionUtils_ArgumentStringEmpty
		{
			get
			{
				return TextRes.GetString("ExceptionUtils_ArgumentStringEmpty");
			}
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06001794 RID: 6036 RVA: 0x000543D3 File Offset: 0x000525D3
		internal static string ODataRequestMessage_AsyncNotAvailable
		{
			get
			{
				return TextRes.GetString("ODataRequestMessage_AsyncNotAvailable");
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06001795 RID: 6037 RVA: 0x000543DF File Offset: 0x000525DF
		internal static string ODataRequestMessage_StreamTaskIsNull
		{
			get
			{
				return TextRes.GetString("ODataRequestMessage_StreamTaskIsNull");
			}
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06001796 RID: 6038 RVA: 0x000543EB File Offset: 0x000525EB
		internal static string ODataRequestMessage_MessageStreamIsNull
		{
			get
			{
				return TextRes.GetString("ODataRequestMessage_MessageStreamIsNull");
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06001797 RID: 6039 RVA: 0x000543F7 File Offset: 0x000525F7
		internal static string ODataResponseMessage_AsyncNotAvailable
		{
			get
			{
				return TextRes.GetString("ODataResponseMessage_AsyncNotAvailable");
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06001798 RID: 6040 RVA: 0x00054403 File Offset: 0x00052603
		internal static string ODataResponseMessage_StreamTaskIsNull
		{
			get
			{
				return TextRes.GetString("ODataResponseMessage_StreamTaskIsNull");
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06001799 RID: 6041 RVA: 0x0005440F File Offset: 0x0005260F
		internal static string ODataResponseMessage_MessageStreamIsNull
		{
			get
			{
				return TextRes.GetString("ODataResponseMessage_MessageStreamIsNull");
			}
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x0600179A RID: 6042 RVA: 0x0005441B File Offset: 0x0005261B
		internal static string AsyncBufferedStream_WriterDisposedWithoutFlush
		{
			get
			{
				return TextRes.GetString("AsyncBufferedStream_WriterDisposedWithoutFlush");
			}
		}

		// Token: 0x0600179B RID: 6043 RVA: 0x00054428 File Offset: 0x00052628
		internal static string ODataOutputContext_UnsupportedPayloadKindForFormat(object p0, object p1)
		{
			return TextRes.GetString("ODataOutputContext_UnsupportedPayloadKindForFormat", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600179C RID: 6044 RVA: 0x00054450 File Offset: 0x00052650
		internal static string ODataOutputContext_CustomInstanceAnnotationsNotSupportedForFormat(object p0)
		{
			return TextRes.GetString("ODataOutputContext_CustomInstanceAnnotationsNotSupportedForFormat", new object[]
			{
				p0
			});
		}

		// Token: 0x0600179D RID: 6045 RVA: 0x00054474 File Offset: 0x00052674
		internal static string ODataInputContext_UnsupportedPayloadKindForFormat(object p0, object p1)
		{
			return TextRes.GetString("ODataInputContext_UnsupportedPayloadKindForFormat", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x0005449C File Offset: 0x0005269C
		internal static string ODataJsonLightSerializer_RelativeUriUsedWithoutMetadataDocumentUriOrMetadata(object p0)
		{
			return TextRes.GetString("ODataJsonLightSerializer_RelativeUriUsedWithoutMetadataDocumentUriOrMetadata", new object[]
			{
				p0
			});
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x000544C0 File Offset: 0x000526C0
		internal static string ODataWriter_RelativeUriUsedWithoutBaseUriSpecified(object p0)
		{
			return TextRes.GetString("ODataWriter_RelativeUriUsedWithoutBaseUriSpecified", new object[]
			{
				p0
			});
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x000544E4 File Offset: 0x000526E4
		internal static string ODataWriter_StreamPropertiesMustBePropertiesOfODataEntry(object p0)
		{
			return TextRes.GetString("ODataWriter_StreamPropertiesMustBePropertiesOfODataEntry", new object[]
			{
				p0
			});
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x00054508 File Offset: 0x00052708
		internal static string ODataWriterCore_InvalidStateTransition(object p0, object p1)
		{
			return TextRes.GetString("ODataWriterCore_InvalidStateTransition", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x00054530 File Offset: 0x00052730
		internal static string ODataWriterCore_InvalidTransitionFromStart(object p0, object p1)
		{
			return TextRes.GetString("ODataWriterCore_InvalidTransitionFromStart", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x00054558 File Offset: 0x00052758
		internal static string ODataWriterCore_InvalidTransitionFromEntry(object p0, object p1)
		{
			return TextRes.GetString("ODataWriterCore_InvalidTransitionFromEntry", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x00054580 File Offset: 0x00052780
		internal static string ODataWriterCore_InvalidTransitionFromNullEntry(object p0, object p1)
		{
			return TextRes.GetString("ODataWriterCore_InvalidTransitionFromNullEntry", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x000545A8 File Offset: 0x000527A8
		internal static string ODataWriterCore_InvalidTransitionFromFeed(object p0, object p1)
		{
			return TextRes.GetString("ODataWriterCore_InvalidTransitionFromFeed", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x000545D0 File Offset: 0x000527D0
		internal static string ODataWriterCore_InvalidTransitionFromExpandedLink(object p0, object p1)
		{
			return TextRes.GetString("ODataWriterCore_InvalidTransitionFromExpandedLink", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x000545F8 File Offset: 0x000527F8
		internal static string ODataWriterCore_InvalidTransitionFromCompleted(object p0, object p1)
		{
			return TextRes.GetString("ODataWriterCore_InvalidTransitionFromCompleted", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x00054620 File Offset: 0x00052820
		internal static string ODataWriterCore_InvalidTransitionFromError(object p0, object p1)
		{
			return TextRes.GetString("ODataWriterCore_InvalidTransitionFromError", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x00054648 File Offset: 0x00052848
		internal static string ODataWriterCore_WriteEndCalledInInvalidState(object p0)
		{
			return TextRes.GetString("ODataWriterCore_WriteEndCalledInInvalidState", new object[]
			{
				p0
			});
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x060017AA RID: 6058 RVA: 0x0005466B File Offset: 0x0005286B
		internal static string ODataWriterCore_OnlyTopLevelFeedsSupportInlineCount
		{
			get
			{
				return TextRes.GetString("ODataWriterCore_OnlyTopLevelFeedsSupportInlineCount");
			}
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x060017AB RID: 6059 RVA: 0x00054677 File Offset: 0x00052877
		internal static string ODataWriterCore_InlineCountInRequest
		{
			get
			{
				return TextRes.GetString("ODataWriterCore_InlineCountInRequest");
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x060017AC RID: 6060 RVA: 0x00054683 File Offset: 0x00052883
		internal static string ODataWriterCore_CannotWriteTopLevelFeedWithEntryWriter
		{
			get
			{
				return TextRes.GetString("ODataWriterCore_CannotWriteTopLevelFeedWithEntryWriter");
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x060017AD RID: 6061 RVA: 0x0005468F File Offset: 0x0005288F
		internal static string ODataWriterCore_CannotWriteTopLevelEntryWithFeedWriter
		{
			get
			{
				return TextRes.GetString("ODataWriterCore_CannotWriteTopLevelEntryWithFeedWriter");
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x060017AE RID: 6062 RVA: 0x0005469B File Offset: 0x0005289B
		internal static string ODataWriterCore_SyncCallOnAsyncWriter
		{
			get
			{
				return TextRes.GetString("ODataWriterCore_SyncCallOnAsyncWriter");
			}
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x060017AF RID: 6063 RVA: 0x000546A7 File Offset: 0x000528A7
		internal static string ODataWriterCore_AsyncCallOnSyncWriter
		{
			get
			{
				return TextRes.GetString("ODataWriterCore_AsyncCallOnSyncWriter");
			}
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x060017B0 RID: 6064 RVA: 0x000546B3 File Offset: 0x000528B3
		internal static string ODataWriterCore_EntityReferenceLinkWithoutNavigationLink
		{
			get
			{
				return TextRes.GetString("ODataWriterCore_EntityReferenceLinkWithoutNavigationLink");
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x060017B1 RID: 6065 RVA: 0x000546BF File Offset: 0x000528BF
		internal static string ODataWriterCore_EntityReferenceLinkInResponse
		{
			get
			{
				return TextRes.GetString("ODataWriterCore_EntityReferenceLinkInResponse");
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x060017B2 RID: 6066 RVA: 0x000546CB File Offset: 0x000528CB
		internal static string ODataWriterCore_DeferredLinkInRequest
		{
			get
			{
				return TextRes.GetString("ODataWriterCore_DeferredLinkInRequest");
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x060017B3 RID: 6067 RVA: 0x000546D7 File Offset: 0x000528D7
		internal static string ODataWriterCore_MultipleItemsInNavigationLinkContent
		{
			get
			{
				return TextRes.GetString("ODataWriterCore_MultipleItemsInNavigationLinkContent");
			}
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x060017B4 RID: 6068 RVA: 0x000546E3 File Offset: 0x000528E3
		internal static string ODataWriterCore_DeltaLinkNotSupportedOnExpandedFeed
		{
			get
			{
				return TextRes.GetString("ODataWriterCore_DeltaLinkNotSupportedOnExpandedFeed");
			}
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x000546F0 File Offset: 0x000528F0
		internal static string DuplicatePropertyNamesChecker_DuplicatePropertyNamesNotAllowed(object p0)
		{
			return TextRes.GetString("DuplicatePropertyNamesChecker_DuplicatePropertyNamesNotAllowed", new object[]
			{
				p0
			});
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x00054714 File Offset: 0x00052914
		internal static string DuplicatePropertyNamesChecker_MultipleLinksForSingleton(object p0)
		{
			return TextRes.GetString("DuplicatePropertyNamesChecker_MultipleLinksForSingleton", new object[]
			{
				p0
			});
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x00054738 File Offset: 0x00052938
		internal static string DuplicatePropertyNamesChecker_DuplicateAnnotationNotAllowed(object p0)
		{
			return TextRes.GetString("DuplicatePropertyNamesChecker_DuplicateAnnotationNotAllowed", new object[]
			{
				p0
			});
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x0005475C File Offset: 0x0005295C
		internal static string DuplicatePropertyNamesChecker_DuplicateAnnotationForPropertyNotAllowed(object p0, object p1)
		{
			return TextRes.GetString("DuplicatePropertyNamesChecker_DuplicateAnnotationForPropertyNotAllowed", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x00054784 File Offset: 0x00052984
		internal static string DuplicatePropertyNamesChecker_DuplicateAnnotationForInstanceAnnotationNotAllowed(object p0, object p1)
		{
			return TextRes.GetString("DuplicatePropertyNamesChecker_DuplicateAnnotationForInstanceAnnotationNotAllowed", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x000547AC File Offset: 0x000529AC
		internal static string DuplicatePropertyNamesChecker_PropertyAnnotationAfterTheProperty(object p0, object p1)
		{
			return TextRes.GetString("DuplicatePropertyNamesChecker_PropertyAnnotationAfterTheProperty", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x000547D4 File Offset: 0x000529D4
		internal static string AtomValueUtils_CannotConvertValueToAtomPrimitive(object p0)
		{
			return TextRes.GetString("AtomValueUtils_CannotConvertValueToAtomPrimitive", new object[]
			{
				p0
			});
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x000547F8 File Offset: 0x000529F8
		internal static string ODataJsonWriter_UnsupportedValueType(object p0)
		{
			return TextRes.GetString("ODataJsonWriter_UnsupportedValueType", new object[]
			{
				p0
			});
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x060017BD RID: 6077 RVA: 0x0005481B File Offset: 0x00052A1B
		internal static string ODataException_GeneralError
		{
			get
			{
				return TextRes.GetString("ODataException_GeneralError");
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x060017BE RID: 6078 RVA: 0x00054827 File Offset: 0x00052A27
		internal static string ODataErrorException_GeneralError
		{
			get
			{
				return TextRes.GetString("ODataErrorException_GeneralError");
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x060017BF RID: 6079 RVA: 0x00054833 File Offset: 0x00052A33
		internal static string ODataUriParserException_GeneralError
		{
			get
			{
				return TextRes.GetString("ODataUriParserException_GeneralError");
			}
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x00054840 File Offset: 0x00052A40
		internal static string ODataVersionChecker_MaxProtocolVersionExceeded(object p0, object p1)
		{
			return TextRes.GetString("ODataVersionChecker_MaxProtocolVersionExceeded", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x00054868 File Offset: 0x00052A68
		internal static string ODataVersionChecker_PropertyNotSupportedForODataVersionGreaterThanX(object p0, object p1)
		{
			return TextRes.GetString("ODataVersionChecker_PropertyNotSupportedForODataVersionGreaterThanX", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x00054890 File Offset: 0x00052A90
		internal static string ODataVersionChecker_ParameterPayloadNotSupported(object p0)
		{
			return TextRes.GetString("ODataVersionChecker_ParameterPayloadNotSupported", new object[]
			{
				p0
			});
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x000548B4 File Offset: 0x00052AB4
		internal static string ODataVersionChecker_AssociationLinksNotSupported(object p0)
		{
			return TextRes.GetString("ODataVersionChecker_AssociationLinksNotSupported", new object[]
			{
				p0
			});
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x000548D8 File Offset: 0x00052AD8
		internal static string ODataVersionChecker_InlineCountNotSupported(object p0)
		{
			return TextRes.GetString("ODataVersionChecker_InlineCountNotSupported", new object[]
			{
				p0
			});
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x000548FC File Offset: 0x00052AFC
		internal static string ODataVersionChecker_NextLinkNotSupported(object p0)
		{
			return TextRes.GetString("ODataVersionChecker_NextLinkNotSupported", new object[]
			{
				p0
			});
		}

		// Token: 0x060017C6 RID: 6086 RVA: 0x00054920 File Offset: 0x00052B20
		internal static string ODataVersionChecker_DeltaLinkNotSupported(object p0)
		{
			return TextRes.GetString("ODataVersionChecker_DeltaLinkNotSupported", new object[]
			{
				p0
			});
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x00054944 File Offset: 0x00052B44
		internal static string ODataVersionChecker_CollectionPropertiesNotSupported(object p0, object p1)
		{
			return TextRes.GetString("ODataVersionChecker_CollectionPropertiesNotSupported", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x0005496C File Offset: 0x00052B6C
		internal static string ODataVersionChecker_CollectionNotSupported(object p0)
		{
			return TextRes.GetString("ODataVersionChecker_CollectionNotSupported", new object[]
			{
				p0
			});
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x00054990 File Offset: 0x00052B90
		internal static string ODataVersionChecker_StreamPropertiesNotSupported(object p0)
		{
			return TextRes.GetString("ODataVersionChecker_StreamPropertiesNotSupported", new object[]
			{
				p0
			});
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x000549B4 File Offset: 0x00052BB4
		internal static string ODataVersionChecker_EpmVersionNotSupported(object p0, object p1, object p2)
		{
			return TextRes.GetString("ODataVersionChecker_EpmVersionNotSupported", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x060017CB RID: 6091 RVA: 0x000549DF File Offset: 0x00052BDF
		internal static string ODataVersionChecker_ProtocolVersion3IsNotSupported
		{
			get
			{
				return TextRes.GetString("ODataVersionChecker_ProtocolVersion3IsNotSupported");
			}
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x000549EC File Offset: 0x00052BEC
		internal static string ODataVersionChecker_GeographyAndGeometryNotSupported(object p0)
		{
			return TextRes.GetString("ODataVersionChecker_GeographyAndGeometryNotSupported", new object[]
			{
				p0
			});
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x060017CD RID: 6093 RVA: 0x00054A0F File Offset: 0x00052C0F
		internal static string ODataAtomCollectionWriter_CollectionNameMustNotBeNull
		{
			get
			{
				return TextRes.GetString("ODataAtomCollectionWriter_CollectionNameMustNotBeNull");
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x060017CE RID: 6094 RVA: 0x00054A1B File Offset: 0x00052C1B
		internal static string ODataAtomWriter_StartEntryXmlCustomizationCallbackReturnedSameInstance
		{
			get
			{
				return TextRes.GetString("ODataAtomWriter_StartEntryXmlCustomizationCallbackReturnedSameInstance");
			}
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x060017CF RID: 6095 RVA: 0x00054A27 File Offset: 0x00052C27
		internal static string ODataAtomWriterMetadataUtils_AuthorMetadataMustNotContainNull
		{
			get
			{
				return TextRes.GetString("ODataAtomWriterMetadataUtils_AuthorMetadataMustNotContainNull");
			}
		}

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x060017D0 RID: 6096 RVA: 0x00054A33 File Offset: 0x00052C33
		internal static string ODataAtomWriterMetadataUtils_CategoryMetadataMustNotContainNull
		{
			get
			{
				return TextRes.GetString("ODataAtomWriterMetadataUtils_CategoryMetadataMustNotContainNull");
			}
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x060017D1 RID: 6097 RVA: 0x00054A3F File Offset: 0x00052C3F
		internal static string ODataAtomWriterMetadataUtils_ContributorMetadataMustNotContainNull
		{
			get
			{
				return TextRes.GetString("ODataAtomWriterMetadataUtils_ContributorMetadataMustNotContainNull");
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x060017D2 RID: 6098 RVA: 0x00054A4B File Offset: 0x00052C4B
		internal static string ODataAtomWriterMetadataUtils_LinkMetadataMustNotContainNull
		{
			get
			{
				return TextRes.GetString("ODataAtomWriterMetadataUtils_LinkMetadataMustNotContainNull");
			}
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x060017D3 RID: 6099 RVA: 0x00054A57 File Offset: 0x00052C57
		internal static string ODataAtomWriterMetadataUtils_LinkMustSpecifyHref
		{
			get
			{
				return TextRes.GetString("ODataAtomWriterMetadataUtils_LinkMustSpecifyHref");
			}
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x060017D4 RID: 6100 RVA: 0x00054A63 File Offset: 0x00052C63
		internal static string ODataAtomWriterMetadataUtils_CategoryMustSpecifyTerm
		{
			get
			{
				return TextRes.GetString("ODataAtomWriterMetadataUtils_CategoryMustSpecifyTerm");
			}
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x00054A70 File Offset: 0x00052C70
		internal static string ODataAtomWriterMetadataUtils_LinkHrefsMustMatch(object p0, object p1)
		{
			return TextRes.GetString("ODataAtomWriterMetadataUtils_LinkHrefsMustMatch", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x00054A98 File Offset: 0x00052C98
		internal static string ODataAtomWriterMetadataUtils_LinkTitlesMustMatch(object p0, object p1)
		{
			return TextRes.GetString("ODataAtomWriterMetadataUtils_LinkTitlesMustMatch", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x00054AC0 File Offset: 0x00052CC0
		internal static string ODataAtomWriterMetadataUtils_LinkRelationsMustMatch(object p0, object p1)
		{
			return TextRes.GetString("ODataAtomWriterMetadataUtils_LinkRelationsMustMatch", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x00054AE8 File Offset: 0x00052CE8
		internal static string ODataAtomWriterMetadataUtils_LinkMediaTypesMustMatch(object p0, object p1)
		{
			return TextRes.GetString("ODataAtomWriterMetadataUtils_LinkMediaTypesMustMatch", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x00054B10 File Offset: 0x00052D10
		internal static string ODataAtomWriterMetadataUtils_InvalidAnnotationValue(object p0, object p1)
		{
			return TextRes.GetString("ODataAtomWriterMetadataUtils_InvalidAnnotationValue", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x060017DA RID: 6106 RVA: 0x00054B37 File Offset: 0x00052D37
		internal static string ODataAtomWriterMetadataUtils_CategoriesHrefWithOtherValues
		{
			get
			{
				return TextRes.GetString("ODataAtomWriterMetadataUtils_CategoriesHrefWithOtherValues");
			}
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x00054B44 File Offset: 0x00052D44
		internal static string ODataAtomWriterMetadataUtils_CategoryTermsMustMatch(object p0, object p1)
		{
			return TextRes.GetString("ODataAtomWriterMetadataUtils_CategoryTermsMustMatch", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x00054B6C File Offset: 0x00052D6C
		internal static string ODataAtomWriterMetadataUtils_CategorySchemesMustMatch(object p0, object p1)
		{
			return TextRes.GetString("ODataAtomWriterMetadataUtils_CategorySchemesMustMatch", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x00054B94 File Offset: 0x00052D94
		internal static string ODataAtomMetadataEpmMerge_TextKindConflict(object p0, object p1, object p2)
		{
			return TextRes.GetString("ODataAtomMetadataEpmMerge_TextKindConflict", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x00054BC0 File Offset: 0x00052DC0
		internal static string ODataAtomMetadataEpmMerge_TextValueConflict(object p0, object p1, object p2)
		{
			return TextRes.GetString("ODataAtomMetadataEpmMerge_TextValueConflict", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x060017DF RID: 6111 RVA: 0x00054BEB File Offset: 0x00052DEB
		internal static string ODataMessageWriter_WriterAlreadyUsed
		{
			get
			{
				return TextRes.GetString("ODataMessageWriter_WriterAlreadyUsed");
			}
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x00054BF8 File Offset: 0x00052DF8
		internal static string ODataMessageWriter_InvalidContentTypeForWritingRawValue(object p0)
		{
			return TextRes.GetString("ODataMessageWriter_InvalidContentTypeForWritingRawValue", new object[]
			{
				p0
			});
		}

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x060017E1 RID: 6113 RVA: 0x00054C1B File Offset: 0x00052E1B
		internal static string ODataMessageWriter_EntityReferenceLinksInRequestNotAllowed
		{
			get
			{
				return TextRes.GetString("ODataMessageWriter_EntityReferenceLinksInRequestNotAllowed");
			}
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x060017E2 RID: 6114 RVA: 0x00054C27 File Offset: 0x00052E27
		internal static string ODataMessageWriter_ErrorPayloadInRequest
		{
			get
			{
				return TextRes.GetString("ODataMessageWriter_ErrorPayloadInRequest");
			}
		}

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x060017E3 RID: 6115 RVA: 0x00054C33 File Offset: 0x00052E33
		internal static string ODataMessageWriter_ServiceDocumentInRequest
		{
			get
			{
				return TextRes.GetString("ODataMessageWriter_ServiceDocumentInRequest");
			}
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x060017E4 RID: 6116 RVA: 0x00054C3F File Offset: 0x00052E3F
		internal static string ODataMessageWriter_MetadataDocumentInRequest
		{
			get
			{
				return TextRes.GetString("ODataMessageWriter_MetadataDocumentInRequest");
			}
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x060017E5 RID: 6117 RVA: 0x00054C4B File Offset: 0x00052E4B
		internal static string ODataMessageWriter_CannotWriteNullInRawFormat
		{
			get
			{
				return TextRes.GetString("ODataMessageWriter_CannotWriteNullInRawFormat");
			}
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x00054C58 File Offset: 0x00052E58
		internal static string ODataMessageWriter_CannotSetHeadersWithInvalidPayloadKind(object p0)
		{
			return TextRes.GetString("ODataMessageWriter_CannotSetHeadersWithInvalidPayloadKind", new object[]
			{
				p0
			});
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x00054C7C File Offset: 0x00052E7C
		internal static string ODataMessageWriter_IncompatiblePayloadKinds(object p0, object p1)
		{
			return TextRes.GetString("ODataMessageWriter_IncompatiblePayloadKinds", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x00054CA4 File Offset: 0x00052EA4
		internal static string ODataMessageWriter_CannotWriteStreamPropertyAsTopLevelProperty(object p0)
		{
			return TextRes.GetString("ODataMessageWriter_CannotWriteStreamPropertyAsTopLevelProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x060017E9 RID: 6121 RVA: 0x00054CC8 File Offset: 0x00052EC8
		internal static string ODataMessageWriter_InvalidPropertyOwningType(object p0, object p1)
		{
			return TextRes.GetString("ODataMessageWriter_InvalidPropertyOwningType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x00054CF0 File Offset: 0x00052EF0
		internal static string ODataMessageWriter_InvalidPropertyProducingFunctionImport(object p0)
		{
			return TextRes.GetString("ODataMessageWriter_InvalidPropertyProducingFunctionImport", new object[]
			{
				p0
			});
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x060017EB RID: 6123 RVA: 0x00054D13 File Offset: 0x00052F13
		internal static string ODataMessageWriter_WriteErrorAlreadyCalled
		{
			get
			{
				return TextRes.GetString("ODataMessageWriter_WriteErrorAlreadyCalled");
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x060017EC RID: 6124 RVA: 0x00054D1F File Offset: 0x00052F1F
		internal static string ODataMessageWriter_CannotWriteInStreamErrorForRawValues
		{
			get
			{
				return TextRes.GetString("ODataMessageWriter_CannotWriteInStreamErrorForRawValues");
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x060017ED RID: 6125 RVA: 0x00054D2B File Offset: 0x00052F2B
		internal static string ODataMessageWriter_CannotWriteMetadataWithoutModel
		{
			get
			{
				return TextRes.GetString("ODataMessageWriter_CannotWriteMetadataWithoutModel");
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x060017EE RID: 6126 RVA: 0x00054D37 File Offset: 0x00052F37
		internal static string ODataMessageWriter_CannotSpecifyFunctionImportWithoutModel
		{
			get
			{
				return TextRes.GetString("ODataMessageWriter_CannotSpecifyFunctionImportWithoutModel");
			}
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x00054D44 File Offset: 0x00052F44
		internal static string ODataMessageWriter_EntityReferenceLinksWithSingletonNavPropNotAllowed(object p0)
		{
			return TextRes.GetString("ODataMessageWriter_EntityReferenceLinksWithSingletonNavPropNotAllowed", new object[]
			{
				p0
			});
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x00054D68 File Offset: 0x00052F68
		internal static string ODataMessageWriter_JsonPaddingOnInvalidContentType(object p0)
		{
			return TextRes.GetString("ODataMessageWriter_JsonPaddingOnInvalidContentType", new object[]
			{
				p0
			});
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x00054D8C File Offset: 0x00052F8C
		internal static string ODataMessageWriter_NonCollectionType(object p0)
		{
			return TextRes.GetString("ODataMessageWriter_NonCollectionType", new object[]
			{
				p0
			});
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x060017F2 RID: 6130 RVA: 0x00054DAF File Offset: 0x00052FAF
		internal static string ODataMessageWriterSettings_MessageWriterSettingsXmlCustomizationCallbacksMustBeSpecifiedBoth
		{
			get
			{
				return TextRes.GetString("ODataMessageWriterSettings_MessageWriterSettingsXmlCustomizationCallbacksMustBeSpecifiedBoth");
			}
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x00054DBC File Offset: 0x00052FBC
		internal static string ODataCollectionWriter_CannotCreateCollectionWriterForFormat(object p0)
		{
			return TextRes.GetString("ODataCollectionWriter_CannotCreateCollectionWriterForFormat", new object[]
			{
				p0
			});
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x00054DE0 File Offset: 0x00052FE0
		internal static string ODataCollectionWriterCore_InvalidTransitionFromStart(object p0, object p1)
		{
			return TextRes.GetString("ODataCollectionWriterCore_InvalidTransitionFromStart", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x00054E08 File Offset: 0x00053008
		internal static string ODataCollectionWriterCore_InvalidTransitionFromCollection(object p0, object p1)
		{
			return TextRes.GetString("ODataCollectionWriterCore_InvalidTransitionFromCollection", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060017F6 RID: 6134 RVA: 0x00054E30 File Offset: 0x00053030
		internal static string ODataCollectionWriterCore_InvalidTransitionFromItem(object p0, object p1)
		{
			return TextRes.GetString("ODataCollectionWriterCore_InvalidTransitionFromItem", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x00054E58 File Offset: 0x00053058
		internal static string ODataCollectionWriterCore_WriteEndCalledInInvalidState(object p0)
		{
			return TextRes.GetString("ODataCollectionWriterCore_WriteEndCalledInInvalidState", new object[]
			{
				p0
			});
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x060017F8 RID: 6136 RVA: 0x00054E7B File Offset: 0x0005307B
		internal static string ODataCollectionWriterCore_SyncCallOnAsyncWriter
		{
			get
			{
				return TextRes.GetString("ODataCollectionWriterCore_SyncCallOnAsyncWriter");
			}
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x060017F9 RID: 6137 RVA: 0x00054E87 File Offset: 0x00053087
		internal static string ODataCollectionWriterCore_AsyncCallOnSyncWriter
		{
			get
			{
				return TextRes.GetString("ODataCollectionWriterCore_AsyncCallOnSyncWriter");
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x060017FA RID: 6138 RVA: 0x00054E93 File Offset: 0x00053093
		internal static string ODataCollectionWriterCore_CollectionsMustNotHaveEmptyName
		{
			get
			{
				return TextRes.GetString("ODataCollectionWriterCore_CollectionsMustNotHaveEmptyName");
			}
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x00054EA0 File Offset: 0x000530A0
		internal static string ODataCollectionWriterCore_CollectionNameDoesntMatchFunctionImportName(object p0, object p1)
		{
			return TextRes.GetString("ODataCollectionWriterCore_CollectionNameDoesntMatchFunctionImportName", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x00054EC8 File Offset: 0x000530C8
		internal static string ODataCollectionWriterCore_NonCollectionType(object p0, object p1)
		{
			return TextRes.GetString("ODataCollectionWriterCore_NonCollectionType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060017FD RID: 6141 RVA: 0x00054EF0 File Offset: 0x000530F0
		internal static string ODataBatch_InvalidHttpMethodForQueryOperation(object p0)
		{
			return TextRes.GetString("ODataBatch_InvalidHttpMethodForQueryOperation", new object[]
			{
				p0
			});
		}

		// Token: 0x060017FE RID: 6142 RVA: 0x00054F14 File Offset: 0x00053114
		internal static string ODataBatch_InvalidHttpMethodForChangeSetRequest(object p0)
		{
			return TextRes.GetString("ODataBatch_InvalidHttpMethodForChangeSetRequest", new object[]
			{
				p0
			});
		}

		// Token: 0x060017FF RID: 6143 RVA: 0x00054F38 File Offset: 0x00053138
		internal static string ODataBatchOperationHeaderDictionary_KeyNotFound(object p0)
		{
			return TextRes.GetString("ODataBatchOperationHeaderDictionary_KeyNotFound", new object[]
			{
				p0
			});
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x00054F5C File Offset: 0x0005315C
		internal static string ODataBatchOperationHeaderDictionary_DuplicateCaseInsensitiveKeys(object p0)
		{
			return TextRes.GetString("ODataBatchOperationHeaderDictionary_DuplicateCaseInsensitiveKeys", new object[]
			{
				p0
			});
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06001801 RID: 6145 RVA: 0x00054F7F File Offset: 0x0005317F
		internal static string ODataParameterWriter_InStreamErrorNotSupported
		{
			get
			{
				return TextRes.GetString("ODataParameterWriter_InStreamErrorNotSupported");
			}
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06001802 RID: 6146 RVA: 0x00054F8B File Offset: 0x0005318B
		internal static string ODataParameterWriter_CannotCreateParameterWriterOnResponseMessage
		{
			get
			{
				return TextRes.GetString("ODataParameterWriter_CannotCreateParameterWriterOnResponseMessage");
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06001803 RID: 6147 RVA: 0x00054F97 File Offset: 0x00053197
		internal static string ODataParameterWriterCore_SyncCallOnAsyncWriter
		{
			get
			{
				return TextRes.GetString("ODataParameterWriterCore_SyncCallOnAsyncWriter");
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06001804 RID: 6148 RVA: 0x00054FA3 File Offset: 0x000531A3
		internal static string ODataParameterWriterCore_AsyncCallOnSyncWriter
		{
			get
			{
				return TextRes.GetString("ODataParameterWriterCore_AsyncCallOnSyncWriter");
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06001805 RID: 6149 RVA: 0x00054FAF File Offset: 0x000531AF
		internal static string ODataParameterWriterCore_CannotWriteStart
		{
			get
			{
				return TextRes.GetString("ODataParameterWriterCore_CannotWriteStart");
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06001806 RID: 6150 RVA: 0x00054FBB File Offset: 0x000531BB
		internal static string ODataParameterWriterCore_CannotWriteParameter
		{
			get
			{
				return TextRes.GetString("ODataParameterWriterCore_CannotWriteParameter");
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06001807 RID: 6151 RVA: 0x00054FC7 File Offset: 0x000531C7
		internal static string ODataParameterWriterCore_CannotWriteEnd
		{
			get
			{
				return TextRes.GetString("ODataParameterWriterCore_CannotWriteEnd");
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06001808 RID: 6152 RVA: 0x00054FD3 File Offset: 0x000531D3
		internal static string ODataParameterWriterCore_CannotWriteInErrorOrCompletedState
		{
			get
			{
				return TextRes.GetString("ODataParameterWriterCore_CannotWriteInErrorOrCompletedState");
			}
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x00054FE0 File Offset: 0x000531E0
		internal static string ODataParameterWriterCore_DuplicatedParameterNameNotAllowed(object p0)
		{
			return TextRes.GetString("ODataParameterWriterCore_DuplicatedParameterNameNotAllowed", new object[]
			{
				p0
			});
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x00055004 File Offset: 0x00053204
		internal static string ODataParameterWriterCore_CannotWriteValueOnNonValueTypeKind(object p0, object p1)
		{
			return TextRes.GetString("ODataParameterWriterCore_CannotWriteValueOnNonValueTypeKind", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x0005502C File Offset: 0x0005322C
		internal static string ODataParameterWriterCore_CannotWriteValueOnNonSupportedValueType(object p0, object p1)
		{
			return TextRes.GetString("ODataParameterWriterCore_CannotWriteValueOnNonSupportedValueType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x00055054 File Offset: 0x00053254
		internal static string ODataParameterWriterCore_CannotCreateCollectionWriterOnNonCollectionTypeKind(object p0, object p1)
		{
			return TextRes.GetString("ODataParameterWriterCore_CannotCreateCollectionWriterOnNonCollectionTypeKind", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x0005507C File Offset: 0x0005327C
		internal static string ODataParameterWriterCore_ParameterNameNotFoundInFunctionImport(object p0, object p1)
		{
			return TextRes.GetString("ODataParameterWriterCore_ParameterNameNotFoundInFunctionImport", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x000550A4 File Offset: 0x000532A4
		internal static string ODataParameterWriterCore_MissingParameterInParameterPayload(object p0, object p1)
		{
			return TextRes.GetString("ODataParameterWriterCore_MissingParameterInParameterPayload", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x0600180F RID: 6159 RVA: 0x000550CB File Offset: 0x000532CB
		internal static string ODataBatchWriter_FlushOrFlushAsyncCalledInStreamRequestedState
		{
			get
			{
				return TextRes.GetString("ODataBatchWriter_FlushOrFlushAsyncCalledInStreamRequestedState");
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06001810 RID: 6160 RVA: 0x000550D7 File Offset: 0x000532D7
		internal static string ODataBatchWriter_CannotCompleteBatchWithActiveChangeSet
		{
			get
			{
				return TextRes.GetString("ODataBatchWriter_CannotCompleteBatchWithActiveChangeSet");
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06001811 RID: 6161 RVA: 0x000550E3 File Offset: 0x000532E3
		internal static string ODataBatchWriter_CannotStartChangeSetWithActiveChangeSet
		{
			get
			{
				return TextRes.GetString("ODataBatchWriter_CannotStartChangeSetWithActiveChangeSet");
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06001812 RID: 6162 RVA: 0x000550EF File Offset: 0x000532EF
		internal static string ODataBatchWriter_CannotCompleteChangeSetWithoutActiveChangeSet
		{
			get
			{
				return TextRes.GetString("ODataBatchWriter_CannotCompleteChangeSetWithoutActiveChangeSet");
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06001813 RID: 6163 RVA: 0x000550FB File Offset: 0x000532FB
		internal static string ODataBatchWriter_InvalidTransitionFromStart
		{
			get
			{
				return TextRes.GetString("ODataBatchWriter_InvalidTransitionFromStart");
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06001814 RID: 6164 RVA: 0x00055107 File Offset: 0x00053307
		internal static string ODataBatchWriter_InvalidTransitionFromBatchStarted
		{
			get
			{
				return TextRes.GetString("ODataBatchWriter_InvalidTransitionFromBatchStarted");
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06001815 RID: 6165 RVA: 0x00055113 File Offset: 0x00053313
		internal static string ODataBatchWriter_InvalidTransitionFromChangeSetStarted
		{
			get
			{
				return TextRes.GetString("ODataBatchWriter_InvalidTransitionFromChangeSetStarted");
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06001816 RID: 6166 RVA: 0x0005511F File Offset: 0x0005331F
		internal static string ODataBatchWriter_InvalidTransitionFromOperationCreated
		{
			get
			{
				return TextRes.GetString("ODataBatchWriter_InvalidTransitionFromOperationCreated");
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06001817 RID: 6167 RVA: 0x0005512B File Offset: 0x0005332B
		internal static string ODataBatchWriter_InvalidTransitionFromOperationContentStreamRequested
		{
			get
			{
				return TextRes.GetString("ODataBatchWriter_InvalidTransitionFromOperationContentStreamRequested");
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06001818 RID: 6168 RVA: 0x00055137 File Offset: 0x00053337
		internal static string ODataBatchWriter_InvalidTransitionFromOperationContentStreamDisposed
		{
			get
			{
				return TextRes.GetString("ODataBatchWriter_InvalidTransitionFromOperationContentStreamDisposed");
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06001819 RID: 6169 RVA: 0x00055143 File Offset: 0x00053343
		internal static string ODataBatchWriter_InvalidTransitionFromChangeSetCompleted
		{
			get
			{
				return TextRes.GetString("ODataBatchWriter_InvalidTransitionFromChangeSetCompleted");
			}
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x0600181A RID: 6170 RVA: 0x0005514F File Offset: 0x0005334F
		internal static string ODataBatchWriter_InvalidTransitionFromBatchCompleted
		{
			get
			{
				return TextRes.GetString("ODataBatchWriter_InvalidTransitionFromBatchCompleted");
			}
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x0600181B RID: 6171 RVA: 0x0005515B File Offset: 0x0005335B
		internal static string ODataBatchWriter_CannotCreateRequestOperationWhenWritingResponse
		{
			get
			{
				return TextRes.GetString("ODataBatchWriter_CannotCreateRequestOperationWhenWritingResponse");
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x0600181C RID: 6172 RVA: 0x00055167 File Offset: 0x00053367
		internal static string ODataBatchWriter_CannotCreateResponseOperationWhenWritingRequest
		{
			get
			{
				return TextRes.GetString("ODataBatchWriter_CannotCreateResponseOperationWhenWritingRequest");
			}
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x00055174 File Offset: 0x00053374
		internal static string ODataBatchWriter_MaxBatchSizeExceeded(object p0)
		{
			return TextRes.GetString("ODataBatchWriter_MaxBatchSizeExceeded", new object[]
			{
				p0
			});
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x00055198 File Offset: 0x00053398
		internal static string ODataBatchWriter_MaxChangeSetSizeExceeded(object p0)
		{
			return TextRes.GetString("ODataBatchWriter_MaxChangeSetSizeExceeded", new object[]
			{
				p0
			});
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x0600181F RID: 6175 RVA: 0x000551BB File Offset: 0x000533BB
		internal static string ODataBatchWriter_SyncCallOnAsyncWriter
		{
			get
			{
				return TextRes.GetString("ODataBatchWriter_SyncCallOnAsyncWriter");
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06001820 RID: 6176 RVA: 0x000551C7 File Offset: 0x000533C7
		internal static string ODataBatchWriter_AsyncCallOnSyncWriter
		{
			get
			{
				return TextRes.GetString("ODataBatchWriter_AsyncCallOnSyncWriter");
			}
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x000551D4 File Offset: 0x000533D4
		internal static string ODataBatchWriter_DuplicateContentIDsNotAllowed(object p0)
		{
			return TextRes.GetString("ODataBatchWriter_DuplicateContentIDsNotAllowed", new object[]
			{
				p0
			});
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06001822 RID: 6178 RVA: 0x000551F7 File Offset: 0x000533F7
		internal static string ODataBatchWriter_CannotWriteInStreamErrorForBatch
		{
			get
			{
				return TextRes.GetString("ODataBatchWriter_CannotWriteInStreamErrorForBatch");
			}
		}

		// Token: 0x06001823 RID: 6179 RVA: 0x00055204 File Offset: 0x00053404
		internal static string ODataBatchUtils_RelativeUriUsedWithoutBaseUriSpecified(object p0)
		{
			return TextRes.GetString("ODataBatchUtils_RelativeUriUsedWithoutBaseUriSpecified", new object[]
			{
				p0
			});
		}

		// Token: 0x06001824 RID: 6180 RVA: 0x00055228 File Offset: 0x00053428
		internal static string ODataBatchUtils_RelativeUriStartingWithDollarUsedWithoutBaseUriSpecified(object p0)
		{
			return TextRes.GetString("ODataBatchUtils_RelativeUriStartingWithDollarUsedWithoutBaseUriSpecified", new object[]
			{
				p0
			});
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06001825 RID: 6181 RVA: 0x0005524B File Offset: 0x0005344B
		internal static string ODataBatchOperationMessage_VerifyNotCompleted
		{
			get
			{
				return TextRes.GetString("ODataBatchOperationMessage_VerifyNotCompleted");
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06001826 RID: 6182 RVA: 0x00055257 File Offset: 0x00053457
		internal static string ODataBatchOperationStream_Disposed
		{
			get
			{
				return TextRes.GetString("ODataBatchOperationStream_Disposed");
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06001827 RID: 6183 RVA: 0x00055263 File Offset: 0x00053463
		internal static string ODataBatchReader_CannotCreateRequestOperationWhenReadingResponse
		{
			get
			{
				return TextRes.GetString("ODataBatchReader_CannotCreateRequestOperationWhenReadingResponse");
			}
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06001828 RID: 6184 RVA: 0x0005526F File Offset: 0x0005346F
		internal static string ODataBatchReader_CannotCreateResponseOperationWhenReadingRequest
		{
			get
			{
				return TextRes.GetString("ODataBatchReader_CannotCreateResponseOperationWhenReadingRequest");
			}
		}

		// Token: 0x06001829 RID: 6185 RVA: 0x0005527C File Offset: 0x0005347C
		internal static string ODataBatchReader_InvalidStateForCreateOperationRequestMessage(object p0)
		{
			return TextRes.GetString("ODataBatchReader_InvalidStateForCreateOperationRequestMessage", new object[]
			{
				p0
			});
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x0600182A RID: 6186 RVA: 0x0005529F File Offset: 0x0005349F
		internal static string ODataBatchReader_OperationRequestMessageAlreadyCreated
		{
			get
			{
				return TextRes.GetString("ODataBatchReader_OperationRequestMessageAlreadyCreated");
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x0600182B RID: 6187 RVA: 0x000552AB File Offset: 0x000534AB
		internal static string ODataBatchReader_OperationResponseMessageAlreadyCreated
		{
			get
			{
				return TextRes.GetString("ODataBatchReader_OperationResponseMessageAlreadyCreated");
			}
		}

		// Token: 0x0600182C RID: 6188 RVA: 0x000552B8 File Offset: 0x000534B8
		internal static string ODataBatchReader_InvalidStateForCreateOperationResponseMessage(object p0)
		{
			return TextRes.GetString("ODataBatchReader_InvalidStateForCreateOperationResponseMessage", new object[]
			{
				p0
			});
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x0600182D RID: 6189 RVA: 0x000552DB File Offset: 0x000534DB
		internal static string ODataBatchReader_CannotUseReaderWhileOperationStreamActive
		{
			get
			{
				return TextRes.GetString("ODataBatchReader_CannotUseReaderWhileOperationStreamActive");
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x0600182E RID: 6190 RVA: 0x000552E7 File Offset: 0x000534E7
		internal static string ODataBatchReader_SyncCallOnAsyncReader
		{
			get
			{
				return TextRes.GetString("ODataBatchReader_SyncCallOnAsyncReader");
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x0600182F RID: 6191 RVA: 0x000552F3 File Offset: 0x000534F3
		internal static string ODataBatchReader_AsyncCallOnSyncReader
		{
			get
			{
				return TextRes.GetString("ODataBatchReader_AsyncCallOnSyncReader");
			}
		}

		// Token: 0x06001830 RID: 6192 RVA: 0x00055300 File Offset: 0x00053500
		internal static string ODataBatchReader_ReadOrReadAsyncCalledInInvalidState(object p0)
		{
			return TextRes.GetString("ODataBatchReader_ReadOrReadAsyncCalledInInvalidState", new object[]
			{
				p0
			});
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x00055324 File Offset: 0x00053524
		internal static string ODataBatchReader_MaxBatchSizeExceeded(object p0)
		{
			return TextRes.GetString("ODataBatchReader_MaxBatchSizeExceeded", new object[]
			{
				p0
			});
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x00055348 File Offset: 0x00053548
		internal static string ODataBatchReader_MaxChangeSetSizeExceeded(object p0)
		{
			return TextRes.GetString("ODataBatchReader_MaxChangeSetSizeExceeded", new object[]
			{
				p0
			});
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06001833 RID: 6195 RVA: 0x0005536B File Offset: 0x0005356B
		internal static string ODataBatchReader_NoMessageWasCreatedForOperation
		{
			get
			{
				return TextRes.GetString("ODataBatchReader_NoMessageWasCreatedForOperation");
			}
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x00055378 File Offset: 0x00053578
		internal static string ODataBatchReader_DuplicateContentIDsNotAllowed(object p0)
		{
			return TextRes.GetString("ODataBatchReader_DuplicateContentIDsNotAllowed", new object[]
			{
				p0
			});
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x0005539C File Offset: 0x0005359C
		internal static string ODataBatchReaderStream_InvalidHeaderSpecified(object p0)
		{
			return TextRes.GetString("ODataBatchReaderStream_InvalidHeaderSpecified", new object[]
			{
				p0
			});
		}

		// Token: 0x06001836 RID: 6198 RVA: 0x000553C0 File Offset: 0x000535C0
		internal static string ODataBatchReaderStream_InvalidRequestLine(object p0)
		{
			return TextRes.GetString("ODataBatchReaderStream_InvalidRequestLine", new object[]
			{
				p0
			});
		}

		// Token: 0x06001837 RID: 6199 RVA: 0x000553E4 File Offset: 0x000535E4
		internal static string ODataBatchReaderStream_InvalidResponseLine(object p0)
		{
			return TextRes.GetString("ODataBatchReaderStream_InvalidResponseLine", new object[]
			{
				p0
			});
		}

		// Token: 0x06001838 RID: 6200 RVA: 0x00055408 File Offset: 0x00053608
		internal static string ODataBatchReaderStream_InvalidHttpVersionSpecified(object p0, object p1)
		{
			return TextRes.GetString("ODataBatchReaderStream_InvalidHttpVersionSpecified", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001839 RID: 6201 RVA: 0x00055430 File Offset: 0x00053630
		internal static string ODataBatchReaderStream_NonIntegerHttpStatusCode(object p0)
		{
			return TextRes.GetString("ODataBatchReaderStream_NonIntegerHttpStatusCode", new object[]
			{
				p0
			});
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x0600183A RID: 6202 RVA: 0x00055453 File Offset: 0x00053653
		internal static string ODataBatchReaderStream_MissingContentTypeHeader
		{
			get
			{
				return TextRes.GetString("ODataBatchReaderStream_MissingContentTypeHeader");
			}
		}

		// Token: 0x0600183B RID: 6203 RVA: 0x00055460 File Offset: 0x00053660
		internal static string ODataBatchReaderStream_MissingOrInvalidContentEncodingHeader(object p0, object p1)
		{
			return TextRes.GetString("ODataBatchReaderStream_MissingOrInvalidContentEncodingHeader", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x00055488 File Offset: 0x00053688
		internal static string ODataBatchReaderStream_InvalidContentTypeSpecified(object p0, object p1, object p2, object p3)
		{
			return TextRes.GetString("ODataBatchReaderStream_InvalidContentTypeSpecified", new object[]
			{
				p0,
				p1,
				p2,
				p3
			});
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x000554B8 File Offset: 0x000536B8
		internal static string ODataBatchReaderStream_InvalidContentLengthSpecified(object p0)
		{
			return TextRes.GetString("ODataBatchReaderStream_InvalidContentLengthSpecified", new object[]
			{
				p0
			});
		}

		// Token: 0x0600183E RID: 6206 RVA: 0x000554DC File Offset: 0x000536DC
		internal static string ODataBatchReaderStream_DuplicateHeaderFound(object p0)
		{
			return TextRes.GetString("ODataBatchReaderStream_DuplicateHeaderFound", new object[]
			{
				p0
			});
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x0600183F RID: 6207 RVA: 0x000554FF File Offset: 0x000536FF
		internal static string ODataBatchReaderStream_NestedChangesetsAreNotSupported
		{
			get
			{
				return TextRes.GetString("ODataBatchReaderStream_NestedChangesetsAreNotSupported");
			}
		}

		// Token: 0x06001840 RID: 6208 RVA: 0x0005550C File Offset: 0x0005370C
		internal static string ODataBatchReaderStream_MultiByteEncodingsNotSupported(object p0)
		{
			return TextRes.GetString("ODataBatchReaderStream_MultiByteEncodingsNotSupported", new object[]
			{
				p0
			});
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06001841 RID: 6209 RVA: 0x0005552F File Offset: 0x0005372F
		internal static string ODataBatchReaderStream_UnexpectedEndOfInput
		{
			get
			{
				return TextRes.GetString("ODataBatchReaderStream_UnexpectedEndOfInput");
			}
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x0005553C File Offset: 0x0005373C
		internal static string ODataBatchReaderStreamBuffer_BoundaryLineSecurityLimitReached(object p0)
		{
			return TextRes.GetString("ODataBatchReaderStreamBuffer_BoundaryLineSecurityLimitReached", new object[]
			{
				p0
			});
		}

		// Token: 0x06001843 RID: 6211 RVA: 0x00055560 File Offset: 0x00053760
		internal static string HttpUtils_MediaTypeUnspecified(object p0)
		{
			return TextRes.GetString("HttpUtils_MediaTypeUnspecified", new object[]
			{
				p0
			});
		}

		// Token: 0x06001844 RID: 6212 RVA: 0x00055584 File Offset: 0x00053784
		internal static string HttpUtils_MediaTypeRequiresSlash(object p0)
		{
			return TextRes.GetString("HttpUtils_MediaTypeRequiresSlash", new object[]
			{
				p0
			});
		}

		// Token: 0x06001845 RID: 6213 RVA: 0x000555A8 File Offset: 0x000537A8
		internal static string HttpUtils_MediaTypeRequiresSubType(object p0)
		{
			return TextRes.GetString("HttpUtils_MediaTypeRequiresSubType", new object[]
			{
				p0
			});
		}

		// Token: 0x06001846 RID: 6214 RVA: 0x000555CC File Offset: 0x000537CC
		internal static string HttpUtils_MediaTypeMissingParameterValue(object p0)
		{
			return TextRes.GetString("HttpUtils_MediaTypeMissingParameterValue", new object[]
			{
				p0
			});
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06001847 RID: 6215 RVA: 0x000555EF File Offset: 0x000537EF
		internal static string HttpUtils_MediaTypeMissingParameterName
		{
			get
			{
				return TextRes.GetString("HttpUtils_MediaTypeMissingParameterName");
			}
		}

		// Token: 0x06001848 RID: 6216 RVA: 0x000555FC File Offset: 0x000537FC
		internal static string HttpUtils_EscapeCharWithoutQuotes(object p0, object p1, object p2, object p3)
		{
			return TextRes.GetString("HttpUtils_EscapeCharWithoutQuotes", new object[]
			{
				p0,
				p1,
				p2,
				p3
			});
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x0005562C File Offset: 0x0005382C
		internal static string HttpUtils_EscapeCharAtEnd(object p0, object p1, object p2, object p3)
		{
			return TextRes.GetString("HttpUtils_EscapeCharAtEnd", new object[]
			{
				p0,
				p1,
				p2,
				p3
			});
		}

		// Token: 0x0600184A RID: 6218 RVA: 0x0005565C File Offset: 0x0005385C
		internal static string HttpUtils_ClosingQuoteNotFound(object p0, object p1, object p2)
		{
			return TextRes.GetString("HttpUtils_ClosingQuoteNotFound", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x0600184B RID: 6219 RVA: 0x00055688 File Offset: 0x00053888
		internal static string HttpUtils_InvalidCharacterInQuotedParameterValue(object p0, object p1, object p2, object p3)
		{
			return TextRes.GetString("HttpUtils_InvalidCharacterInQuotedParameterValue", new object[]
			{
				p0,
				p1,
				p2,
				p3
			});
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x0600184C RID: 6220 RVA: 0x000556B7 File Offset: 0x000538B7
		internal static string HttpUtils_ContentTypeMissing
		{
			get
			{
				return TextRes.GetString("HttpUtils_ContentTypeMissing");
			}
		}

		// Token: 0x0600184D RID: 6221 RVA: 0x000556C4 File Offset: 0x000538C4
		internal static string HttpUtils_MediaTypeRequiresSemicolonBeforeParameter(object p0)
		{
			return TextRes.GetString("HttpUtils_MediaTypeRequiresSemicolonBeforeParameter", new object[]
			{
				p0
			});
		}

		// Token: 0x0600184E RID: 6222 RVA: 0x000556E8 File Offset: 0x000538E8
		internal static string HttpUtils_InvalidQualityValueStartChar(object p0, object p1)
		{
			return TextRes.GetString("HttpUtils_InvalidQualityValueStartChar", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600184F RID: 6223 RVA: 0x00055710 File Offset: 0x00053910
		internal static string HttpUtils_InvalidQualityValue(object p0, object p1)
		{
			return TextRes.GetString("HttpUtils_InvalidQualityValue", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x00055738 File Offset: 0x00053938
		internal static string HttpUtils_CannotConvertCharToInt(object p0)
		{
			return TextRes.GetString("HttpUtils_CannotConvertCharToInt", new object[]
			{
				p0
			});
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x0005575C File Offset: 0x0005395C
		internal static string HttpUtils_MissingSeparatorBetweenCharsets(object p0)
		{
			return TextRes.GetString("HttpUtils_MissingSeparatorBetweenCharsets", new object[]
			{
				p0
			});
		}

		// Token: 0x06001852 RID: 6226 RVA: 0x00055780 File Offset: 0x00053980
		internal static string HttpUtils_InvalidSeparatorBetweenCharsets(object p0)
		{
			return TextRes.GetString("HttpUtils_InvalidSeparatorBetweenCharsets", new object[]
			{
				p0
			});
		}

		// Token: 0x06001853 RID: 6227 RVA: 0x000557A4 File Offset: 0x000539A4
		internal static string HttpUtils_InvalidCharsetName(object p0)
		{
			return TextRes.GetString("HttpUtils_InvalidCharsetName", new object[]
			{
				p0
			});
		}

		// Token: 0x06001854 RID: 6228 RVA: 0x000557C8 File Offset: 0x000539C8
		internal static string HttpUtils_UnexpectedEndOfQValue(object p0)
		{
			return TextRes.GetString("HttpUtils_UnexpectedEndOfQValue", new object[]
			{
				p0
			});
		}

		// Token: 0x06001855 RID: 6229 RVA: 0x000557EC File Offset: 0x000539EC
		internal static string HttpUtils_ExpectedLiteralNotFoundInString(object p0, object p1, object p2)
		{
			return TextRes.GetString("HttpUtils_ExpectedLiteralNotFoundInString", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x06001856 RID: 6230 RVA: 0x00055818 File Offset: 0x00053A18
		internal static string HttpUtils_InvalidHttpMethodString(object p0)
		{
			return TextRes.GetString("HttpUtils_InvalidHttpMethodString", new object[]
			{
				p0
			});
		}

		// Token: 0x06001857 RID: 6231 RVA: 0x0005583C File Offset: 0x00053A3C
		internal static string HttpUtils_NoOrMoreThanOneContentTypeSpecified(object p0)
		{
			return TextRes.GetString("HttpUtils_NoOrMoreThanOneContentTypeSpecified", new object[]
			{
				p0
			});
		}

		// Token: 0x06001858 RID: 6232 RVA: 0x00055860 File Offset: 0x00053A60
		internal static string HttpHeaderValueLexer_UnrecognizedSeparator(object p0, object p1, object p2, object p3)
		{
			return TextRes.GetString("HttpHeaderValueLexer_UnrecognizedSeparator", new object[]
			{
				p0,
				p1,
				p2,
				p3
			});
		}

		// Token: 0x06001859 RID: 6233 RVA: 0x00055890 File Offset: 0x00053A90
		internal static string HttpHeaderValueLexer_TokenExpectedButFoundQuotedString(object p0, object p1, object p2)
		{
			return TextRes.GetString("HttpHeaderValueLexer_TokenExpectedButFoundQuotedString", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x0600185A RID: 6234 RVA: 0x000558BC File Offset: 0x00053ABC
		internal static string HttpHeaderValueLexer_FailedToReadTokenOrQuotedString(object p0, object p1, object p2)
		{
			return TextRes.GetString("HttpHeaderValueLexer_FailedToReadTokenOrQuotedString", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x0600185B RID: 6235 RVA: 0x000558E8 File Offset: 0x00053AE8
		internal static string HttpHeaderValueLexer_InvalidSeparatorAfterQuotedString(object p0, object p1, object p2, object p3)
		{
			return TextRes.GetString("HttpHeaderValueLexer_InvalidSeparatorAfterQuotedString", new object[]
			{
				p0,
				p1,
				p2,
				p3
			});
		}

		// Token: 0x0600185C RID: 6236 RVA: 0x00055918 File Offset: 0x00053B18
		internal static string HttpHeaderValueLexer_EndOfFileAfterSeparator(object p0, object p1, object p2, object p3)
		{
			return TextRes.GetString("HttpHeaderValueLexer_EndOfFileAfterSeparator", new object[]
			{
				p0,
				p1,
				p2,
				p3
			});
		}

		// Token: 0x0600185D RID: 6237 RVA: 0x00055948 File Offset: 0x00053B48
		internal static string MediaType_EncodingNotSupported(object p0)
		{
			return TextRes.GetString("MediaType_EncodingNotSupported", new object[]
			{
				p0
			});
		}

		// Token: 0x0600185E RID: 6238 RVA: 0x0005596C File Offset: 0x00053B6C
		internal static string MediaTypeUtils_DidNotFindMatchingMediaType(object p0, object p1)
		{
			return TextRes.GetString("MediaTypeUtils_DidNotFindMatchingMediaType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x00055994 File Offset: 0x00053B94
		internal static string MediaTypeUtils_CannotDetermineFormatFromContentType(object p0, object p1)
		{
			return TextRes.GetString("MediaTypeUtils_CannotDetermineFormatFromContentType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001860 RID: 6240 RVA: 0x000559BC File Offset: 0x00053BBC
		internal static string MediaTypeUtils_NoOrMoreThanOneContentTypeSpecified(object p0)
		{
			return TextRes.GetString("MediaTypeUtils_NoOrMoreThanOneContentTypeSpecified", new object[]
			{
				p0
			});
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x000559E0 File Offset: 0x00053BE0
		internal static string MediaTypeUtils_BoundaryMustBeSpecifiedForBatchPayloads(object p0, object p1)
		{
			return TextRes.GetString("MediaTypeUtils_BoundaryMustBeSpecifiedForBatchPayloads", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001862 RID: 6242 RVA: 0x00055A08 File Offset: 0x00053C08
		internal static string EntityPropertyMapping_EpmAttribute(object p0)
		{
			return TextRes.GetString("EntityPropertyMapping_EpmAttribute", new object[]
			{
				p0
			});
		}

		// Token: 0x06001863 RID: 6243 RVA: 0x00055A2C File Offset: 0x00053C2C
		internal static string EntityPropertyMapping_InvalidTargetPath(object p0)
		{
			return TextRes.GetString("EntityPropertyMapping_InvalidTargetPath", new object[]
			{
				p0
			});
		}

		// Token: 0x06001864 RID: 6244 RVA: 0x00055A50 File Offset: 0x00053C50
		internal static string EntityPropertyMapping_TargetNamespaceUriNotValid(object p0)
		{
			return TextRes.GetString("EntityPropertyMapping_TargetNamespaceUriNotValid", new object[]
			{
				p0
			});
		}

		// Token: 0x06001865 RID: 6245 RVA: 0x00055A74 File Offset: 0x00053C74
		internal static string EpmSourceTree_InvalidSourcePath(object p0, object p1)
		{
			return TextRes.GetString("EpmSourceTree_InvalidSourcePath", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001866 RID: 6246 RVA: 0x00055A9C File Offset: 0x00053C9C
		internal static string EpmSourceTree_EndsWithNonPrimitiveType(object p0)
		{
			return TextRes.GetString("EpmSourceTree_EndsWithNonPrimitiveType", new object[]
			{
				p0
			});
		}

		// Token: 0x06001867 RID: 6247 RVA: 0x00055AC0 File Offset: 0x00053CC0
		internal static string EpmSourceTree_TraversalOfNonComplexType(object p0)
		{
			return TextRes.GetString("EpmSourceTree_TraversalOfNonComplexType", new object[]
			{
				p0
			});
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x00055AE4 File Offset: 0x00053CE4
		internal static string EpmSourceTree_DuplicateEpmAttributesWithSameSourceName(object p0, object p1)
		{
			return TextRes.GetString("EpmSourceTree_DuplicateEpmAttributesWithSameSourceName", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x00055B0C File Offset: 0x00053D0C
		internal static string EpmSourceTree_MissingPropertyOnType(object p0, object p1)
		{
			return TextRes.GetString("EpmSourceTree_MissingPropertyOnType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x00055B34 File Offset: 0x00053D34
		internal static string EpmSourceTree_MissingPropertyOnInstance(object p0, object p1)
		{
			return TextRes.GetString("EpmSourceTree_MissingPropertyOnInstance", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600186B RID: 6251 RVA: 0x00055B5C File Offset: 0x00053D5C
		internal static string EpmSourceTree_StreamPropertyCannotBeMapped(object p0, object p1)
		{
			return TextRes.GetString("EpmSourceTree_StreamPropertyCannotBeMapped", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x00055B84 File Offset: 0x00053D84
		internal static string EpmSourceTree_SpatialTypeCannotBeMapped(object p0, object p1)
		{
			return TextRes.GetString("EpmSourceTree_SpatialTypeCannotBeMapped", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x00055BAC File Offset: 0x00053DAC
		internal static string EpmSourceTree_OpenPropertySpatialTypeCannotBeMapped(object p0, object p1)
		{
			return TextRes.GetString("EpmSourceTree_OpenPropertySpatialTypeCannotBeMapped", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x00055BD4 File Offset: 0x00053DD4
		internal static string EpmSourceTree_OpenComplexPropertyCannotBeMapped(object p0, object p1)
		{
			return TextRes.GetString("EpmSourceTree_OpenComplexPropertyCannotBeMapped", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600186F RID: 6255 RVA: 0x00055BFC File Offset: 0x00053DFC
		internal static string EpmSourceTree_CollectionPropertyCannotBeMapped(object p0, object p1)
		{
			return TextRes.GetString("EpmSourceTree_CollectionPropertyCannotBeMapped", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001870 RID: 6256 RVA: 0x00055C24 File Offset: 0x00053E24
		internal static string EpmTargetTree_InvalidTargetPath_EmptySegment(object p0)
		{
			return TextRes.GetString("EpmTargetTree_InvalidTargetPath_EmptySegment", new object[]
			{
				p0
			});
		}

		// Token: 0x06001871 RID: 6257 RVA: 0x00055C48 File Offset: 0x00053E48
		internal static string EpmTargetTree_InvalidTargetPath_MixedContent(object p0, object p1)
		{
			return TextRes.GetString("EpmTargetTree_InvalidTargetPath_MixedContent", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001872 RID: 6258 RVA: 0x00055C70 File Offset: 0x00053E70
		internal static string EpmTargetTree_AttributeInMiddle(object p0)
		{
			return TextRes.GetString("EpmTargetTree_AttributeInMiddle", new object[]
			{
				p0
			});
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x00055C94 File Offset: 0x00053E94
		internal static string EpmTargetTree_DuplicateEpmAttributesWithSameTargetName(object p0, object p1, object p2, object p3)
		{
			return TextRes.GetString("EpmTargetTree_DuplicateEpmAttributesWithSameTargetName", new object[]
			{
				p0,
				p1,
				p2,
				p3
			});
		}

		// Token: 0x06001874 RID: 6260 RVA: 0x00055CC4 File Offset: 0x00053EC4
		internal static string EpmSyndicationWriter_DateTimePropertyCanNotBeConverted(object p0)
		{
			return TextRes.GetString("EpmSyndicationWriter_DateTimePropertyCanNotBeConverted", new object[]
			{
				p0
			});
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x00055CE8 File Offset: 0x00053EE8
		internal static string EpmSyndicationWriter_EmptyCollectionMappedToAuthor(object p0)
		{
			return TextRes.GetString("EpmSyndicationWriter_EmptyCollectionMappedToAuthor", new object[]
			{
				p0
			});
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x00055D0C File Offset: 0x00053F0C
		internal static string EpmSyndicationWriter_NullValueForAttributeTarget(object p0, object p1, object p2)
		{
			return TextRes.GetString("EpmSyndicationWriter_NullValueForAttributeTarget", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x00055D38 File Offset: 0x00053F38
		internal static string EpmSyndicationWriter_InvalidLinkLengthValue(object p0)
		{
			return TextRes.GetString("EpmSyndicationWriter_InvalidLinkLengthValue", new object[]
			{
				p0
			});
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x00055D5C File Offset: 0x00053F5C
		internal static string EpmSyndicationWriter_InvalidValueForLinkRelCriteriaAttribute(object p0, object p1, object p2)
		{
			return TextRes.GetString("EpmSyndicationWriter_InvalidValueForLinkRelCriteriaAttribute", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x00055D88 File Offset: 0x00053F88
		internal static string EpmSyndicationWriter_InvalidValueForCategorySchemeCriteriaAttribute(object p0, object p1, object p2)
		{
			return TextRes.GetString("EpmSyndicationWriter_InvalidValueForCategorySchemeCriteriaAttribute", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x0600187A RID: 6266 RVA: 0x00055DB4 File Offset: 0x00053FB4
		internal static string ExpressionLexer_ExpectedLiteralToken(object p0)
		{
			return TextRes.GetString("ExpressionLexer_ExpectedLiteralToken", new object[]
			{
				p0
			});
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x00055DD8 File Offset: 0x00053FD8
		internal static string UriUtils_InvalidRelativeUriForEscaping(object p0, object p1)
		{
			return TextRes.GetString("UriUtils_InvalidRelativeUriForEscaping", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x00055E00 File Offset: 0x00054000
		internal static string ODataUriUtils_ConvertToUriLiteralUnsupportedType(object p0)
		{
			return TextRes.GetString("ODataUriUtils_ConvertToUriLiteralUnsupportedType", new object[]
			{
				p0
			});
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x00055E24 File Offset: 0x00054024
		internal static string ODataUriUtils_ConvertToUriLiteralUnsupportedFormat(object p0)
		{
			return TextRes.GetString("ODataUriUtils_ConvertToUriLiteralUnsupportedFormat", new object[]
			{
				p0
			});
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x0600187E RID: 6270 RVA: 0x00055E47 File Offset: 0x00054047
		internal static string ODataUriUtils_ConvertFromUriLiteralTypeRefWithoutModel
		{
			get
			{
				return TextRes.GetString("ODataUriUtils_ConvertFromUriLiteralTypeRefWithoutModel");
			}
		}

		// Token: 0x0600187F RID: 6271 RVA: 0x00055E54 File Offset: 0x00054054
		internal static string ODataUriUtils_ConvertFromUriLiteralTypeVerificationFailure(object p0, object p1)
		{
			return TextRes.GetString("ODataUriUtils_ConvertFromUriLiteralTypeVerificationFailure", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x00055E7C File Offset: 0x0005407C
		internal static string ODataUriUtils_ConvertFromUriLiteralNullTypeVerificationFailure(object p0, object p1)
		{
			return TextRes.GetString("ODataUriUtils_ConvertFromUriLiteralNullTypeVerificationFailure", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x00055EA4 File Offset: 0x000540A4
		internal static string ODataUriUtils_ConvertFromUriLiteralNullOnNonNullableType(object p0)
		{
			return TextRes.GetString("ODataUriUtils_ConvertFromUriLiteralNullOnNonNullableType", new object[]
			{
				p0
			});
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x00055EC8 File Offset: 0x000540C8
		internal static string ODataUtils_CannotConvertValueToRawPrimitive(object p0)
		{
			return TextRes.GetString("ODataUtils_CannotConvertValueToRawPrimitive", new object[]
			{
				p0
			});
		}

		// Token: 0x06001883 RID: 6275 RVA: 0x00055EEC File Offset: 0x000540EC
		internal static string ODataUtils_DidNotFindDefaultMediaType(object p0)
		{
			return TextRes.GetString("ODataUtils_DidNotFindDefaultMediaType", new object[]
			{
				p0
			});
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06001884 RID: 6276 RVA: 0x00055F0F File Offset: 0x0005410F
		internal static string ODataUtils_CannotSaveAnnotationsToBuiltInModel
		{
			get
			{
				return TextRes.GetString("ODataUtils_CannotSaveAnnotationsToBuiltInModel");
			}
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x00055F1C File Offset: 0x0005411C
		internal static string ODataUtils_UnsupportedVersionHeader(object p0)
		{
			return TextRes.GetString("ODataUtils_UnsupportedVersionHeader", new object[]
			{
				p0
			});
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06001886 RID: 6278 RVA: 0x00055F3F File Offset: 0x0005413F
		internal static string ODataUtils_UnsupportedVersionNumber
		{
			get
			{
				return TextRes.GetString("ODataUtils_UnsupportedVersionNumber");
			}
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06001887 RID: 6279 RVA: 0x00055F4B File Offset: 0x0005414B
		internal static string ODataUtils_NullValueForMimeTypeAnnotation
		{
			get
			{
				return TextRes.GetString("ODataUtils_NullValueForMimeTypeAnnotation");
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06001888 RID: 6280 RVA: 0x00055F57 File Offset: 0x00054157
		internal static string ODataUtils_NullValueForHttpMethodAnnotation
		{
			get
			{
				return TextRes.GetString("ODataUtils_NullValueForHttpMethodAnnotation");
			}
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06001889 RID: 6281 RVA: 0x00055F63 File Offset: 0x00054163
		internal static string ODataUtils_IsAlwaysBindableAnnotationSetForANonBindableFunctionImport
		{
			get
			{
				return TextRes.GetString("ODataUtils_IsAlwaysBindableAnnotationSetForANonBindableFunctionImport");
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x0600188A RID: 6282 RVA: 0x00055F6F File Offset: 0x0005416F
		internal static string ODataUtils_UnexpectedIsAlwaysBindableAnnotationInANonBindableFunctionImport
		{
			get
			{
				return TextRes.GetString("ODataUtils_UnexpectedIsAlwaysBindableAnnotationInANonBindableFunctionImport");
			}
		}

		// Token: 0x0600188B RID: 6283 RVA: 0x00055F7C File Offset: 0x0005417C
		internal static string ReaderUtils_EnumerableModified(object p0)
		{
			return TextRes.GetString("ReaderUtils_EnumerableModified", new object[]
			{
				p0
			});
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x00055FA0 File Offset: 0x000541A0
		internal static string ReaderValidationUtils_NullValueForNonNullableType(object p0)
		{
			return TextRes.GetString("ReaderValidationUtils_NullValueForNonNullableType", new object[]
			{
				p0
			});
		}

		// Token: 0x0600188D RID: 6285 RVA: 0x00055FC4 File Offset: 0x000541C4
		internal static string ReaderValidationUtils_NullNamedValueForNonNullableType(object p0, object p1)
		{
			return TextRes.GetString("ReaderValidationUtils_NullNamedValueForNonNullableType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x0600188E RID: 6286 RVA: 0x00055FEB File Offset: 0x000541EB
		internal static string ReaderValidationUtils_EntityReferenceLinkMissingUri
		{
			get
			{
				return TextRes.GetString("ReaderValidationUtils_EntityReferenceLinkMissingUri");
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x0600188F RID: 6287 RVA: 0x00055FF7 File Offset: 0x000541F7
		internal static string ReaderValidationUtils_ValueWithoutType
		{
			get
			{
				return TextRes.GetString("ReaderValidationUtils_ValueWithoutType");
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06001890 RID: 6288 RVA: 0x00056003 File Offset: 0x00054203
		internal static string ReaderValidationUtils_EntryWithoutType
		{
			get
			{
				return TextRes.GetString("ReaderValidationUtils_EntryWithoutType");
			}
		}

		// Token: 0x06001891 RID: 6289 RVA: 0x00056010 File Offset: 0x00054210
		internal static string ReaderValidationUtils_DerivedComplexTypesAreNotAllowed(object p0, object p1)
		{
			return TextRes.GetString("ReaderValidationUtils_DerivedComplexTypesAreNotAllowed", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001892 RID: 6290 RVA: 0x00056038 File Offset: 0x00054238
		internal static string ReaderValidationUtils_CannotConvertPrimitiveValue(object p0)
		{
			return TextRes.GetString("ReaderValidationUtils_CannotConvertPrimitiveValue", new object[]
			{
				p0
			});
		}

		// Token: 0x06001893 RID: 6291 RVA: 0x0005605C File Offset: 0x0005425C
		internal static string ReaderValidationUtils_MessageReaderSettingsBaseUriMustBeNullOrAbsolute(object p0)
		{
			return TextRes.GetString("ReaderValidationUtils_MessageReaderSettingsBaseUriMustBeNullOrAbsolute", new object[]
			{
				p0
			});
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06001894 RID: 6292 RVA: 0x0005607F File Offset: 0x0005427F
		internal static string ReaderValidationUtils_UndeclaredPropertyBehaviorKindSpecifiedOnRequest
		{
			get
			{
				return TextRes.GetString("ReaderValidationUtils_UndeclaredPropertyBehaviorKindSpecifiedOnRequest");
			}
		}

		// Token: 0x06001895 RID: 6293 RVA: 0x0005608C File Offset: 0x0005428C
		internal static string ReaderValidationUtils_UndeclaredPropertyBehaviorKindSpecifiedForOpenType(object p0, object p1)
		{
			return TextRes.GetString("ReaderValidationUtils_UndeclaredPropertyBehaviorKindSpecifiedForOpenType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001896 RID: 6294 RVA: 0x000560B4 File Offset: 0x000542B4
		internal static string ReaderValidationUtils_MetadataUriValidationInvalidExpectedEntitySet(object p0, object p1, object p2)
		{
			return TextRes.GetString("ReaderValidationUtils_MetadataUriValidationInvalidExpectedEntitySet", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x000560E0 File Offset: 0x000542E0
		internal static string ReaderValidationUtils_MetadataUriValidationInvalidExpectedEntityType(object p0, object p1, object p2)
		{
			return TextRes.GetString("ReaderValidationUtils_MetadataUriValidationInvalidExpectedEntityType", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x0005610C File Offset: 0x0005430C
		internal static string ReaderValidationUtils_MetadataUriValidationNonMatchingPropertyNames(object p0, object p1, object p2, object p3)
		{
			return TextRes.GetString("ReaderValidationUtils_MetadataUriValidationNonMatchingPropertyNames", new object[]
			{
				p0,
				p1,
				p2,
				p3
			});
		}

		// Token: 0x06001899 RID: 6297 RVA: 0x0005613C File Offset: 0x0005433C
		internal static string ReaderValidationUtils_MetadataUriValidationNonMatchingDeclaringTypes(object p0, object p1, object p2, object p3)
		{
			return TextRes.GetString("ReaderValidationUtils_MetadataUriValidationNonMatchingDeclaringTypes", new object[]
			{
				p0,
				p1,
				p2,
				p3
			});
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x0005616C File Offset: 0x0005436C
		internal static string ReaderValidationUtils_MetadataUriValidationNonMatchingCollectionNames(object p0, object p1, object p2)
		{
			return TextRes.GetString("ReaderValidationUtils_MetadataUriValidationNonMatchingCollectionNames", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x00056198 File Offset: 0x00054398
		internal static string ReaderValidationUtils_MetadataUriValidationNonMatchingCollectionItemTypes(object p0, object p1, object p2, object p3)
		{
			return TextRes.GetString("ReaderValidationUtils_MetadataUriValidationNonMatchingCollectionItemTypes", new object[]
			{
				p0,
				p1,
				p2,
				p3
			});
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x000561C8 File Offset: 0x000543C8
		internal static string ReaderValidationUtils_MetadataUriValidationPropertyWithExpectedFunctionImport(object p0, object p1, object p2, object p3)
		{
			return TextRes.GetString("ReaderValidationUtils_MetadataUriValidationPropertyWithExpectedFunctionImport", new object[]
			{
				p0,
				p1,
				p2,
				p3
			});
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x000561F8 File Offset: 0x000543F8
		internal static string ReaderValidationUtils_MetadataUriValidationFunctionImportWithExpectedProperty(object p0, object p1, object p2, object p3)
		{
			return TextRes.GetString("ReaderValidationUtils_MetadataUriValidationFunctionImportWithExpectedProperty", new object[]
			{
				p0,
				p1,
				p2,
				p3
			});
		}

		// Token: 0x0600189E RID: 6302 RVA: 0x00056228 File Offset: 0x00054428
		internal static string ReaderValidationUtils_NonMatchingCollectionNames(object p0, object p1)
		{
			return TextRes.GetString("ReaderValidationUtils_NonMatchingCollectionNames", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x00056250 File Offset: 0x00054450
		internal static string ReaderValidationUtils_NonMatchingPropertyNames(object p0, object p1)
		{
			return TextRes.GetString("ReaderValidationUtils_NonMatchingPropertyNames", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x00056278 File Offset: 0x00054478
		internal static string ReaderValidationUtils_MetadataUriValidationNonMatchingPropertyDeclaringTypes(object p0, object p1, object p2, object p3)
		{
			return TextRes.GetString("ReaderValidationUtils_MetadataUriValidationNonMatchingPropertyDeclaringTypes", new object[]
			{
				p0,
				p1,
				p2,
				p3
			});
		}

		// Token: 0x060018A1 RID: 6305 RVA: 0x000562A8 File Offset: 0x000544A8
		internal static string ReaderValidationUtils_MetadataUriValidationNonMatchingPropertyTypes(object p0, object p1, object p2, object p3, object p4)
		{
			return TextRes.GetString("ReaderValidationUtils_MetadataUriValidationNonMatchingPropertyTypes", new object[]
			{
				p0,
				p1,
				p2,
				p3,
				p4
			});
		}

		// Token: 0x060018A2 RID: 6306 RVA: 0x000562DC File Offset: 0x000544DC
		internal static string ReaderValidationUtils_MetadataUriValidationNonMatchingFunctionImportNames(object p0, object p1, object p2)
		{
			return TextRes.GetString("ReaderValidationUtils_MetadataUriValidationNonMatchingFunctionImportNames", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x060018A3 RID: 6307 RVA: 0x00056308 File Offset: 0x00054508
		internal static string ReaderValidationUtils_MetadataUriValidationNonMatchingFunctionImportReturnTypes(object p0, object p1, object p2, object p3)
		{
			return TextRes.GetString("ReaderValidationUtils_MetadataUriValidationNonMatchingFunctionImportReturnTypes", new object[]
			{
				p0,
				p1,
				p2,
				p3
			});
		}

		// Token: 0x060018A4 RID: 6308 RVA: 0x00056338 File Offset: 0x00054538
		internal static string ReaderValidationUtils_TypeInMetadataUriDoesNotMatchExpectedType(object p0, object p1, object p2)
		{
			return TextRes.GetString("ReaderValidationUtils_TypeInMetadataUriDoesNotMatchExpectedType", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x060018A5 RID: 6309 RVA: 0x00056364 File Offset: 0x00054564
		internal static string ReaderValidationUtils_MetadataUriDoesNotReferTypeAssignableToExpectedType(object p0, object p1, object p2)
		{
			return TextRes.GetString("ReaderValidationUtils_MetadataUriDoesNotReferTypeAssignableToExpectedType", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x060018A6 RID: 6310 RVA: 0x0005638F File Offset: 0x0005458F
		internal static string ODataMessageReader_ReaderAlreadyUsed
		{
			get
			{
				return TextRes.GetString("ODataMessageReader_ReaderAlreadyUsed");
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x060018A7 RID: 6311 RVA: 0x0005639B File Offset: 0x0005459B
		internal static string ODataMessageReader_ErrorPayloadInRequest
		{
			get
			{
				return TextRes.GetString("ODataMessageReader_ErrorPayloadInRequest");
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x060018A8 RID: 6312 RVA: 0x000563A7 File Offset: 0x000545A7
		internal static string ODataMessageReader_ServiceDocumentInRequest
		{
			get
			{
				return TextRes.GetString("ODataMessageReader_ServiceDocumentInRequest");
			}
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x060018A9 RID: 6313 RVA: 0x000563B3 File Offset: 0x000545B3
		internal static string ODataMessageReader_MetadataDocumentInRequest
		{
			get
			{
				return TextRes.GetString("ODataMessageReader_MetadataDocumentInRequest");
			}
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x000563C0 File Offset: 0x000545C0
		internal static string ODataMessageReader_ExpectedTypeSpecifiedWithoutMetadata(object p0)
		{
			return TextRes.GetString("ODataMessageReader_ExpectedTypeSpecifiedWithoutMetadata", new object[]
			{
				p0
			});
		}

		// Token: 0x060018AB RID: 6315 RVA: 0x000563E4 File Offset: 0x000545E4
		internal static string ODataMessageReader_EntitySetSpecifiedWithoutMetadata(object p0)
		{
			return TextRes.GetString("ODataMessageReader_EntitySetSpecifiedWithoutMetadata", new object[]
			{
				p0
			});
		}

		// Token: 0x060018AC RID: 6316 RVA: 0x00056408 File Offset: 0x00054608
		internal static string ODataMessageReader_FunctionImportSpecifiedWithoutMetadata(object p0)
		{
			return TextRes.GetString("ODataMessageReader_FunctionImportSpecifiedWithoutMetadata", new object[]
			{
				p0
			});
		}

		// Token: 0x060018AD RID: 6317 RVA: 0x0005642C File Offset: 0x0005462C
		internal static string ODataMessageReader_ProducingFunctionImportNonCollectionType(object p0, object p1)
		{
			return TextRes.GetString("ODataMessageReader_ProducingFunctionImportNonCollectionType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060018AE RID: 6318 RVA: 0x00056454 File Offset: 0x00054654
		internal static string ODataMessageReader_ExpectedCollectionTypeWrongKind(object p0)
		{
			return TextRes.GetString("ODataMessageReader_ExpectedCollectionTypeWrongKind", new object[]
			{
				p0
			});
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x060018AF RID: 6319 RVA: 0x00056477 File Offset: 0x00054677
		internal static string ODataMessageReader_ExpectedPropertyTypeEntityCollectionKind
		{
			get
			{
				return TextRes.GetString("ODataMessageReader_ExpectedPropertyTypeEntityCollectionKind");
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x060018B0 RID: 6320 RVA: 0x00056483 File Offset: 0x00054683
		internal static string ODataMessageReader_ExpectedPropertyTypeEntityKind
		{
			get
			{
				return TextRes.GetString("ODataMessageReader_ExpectedPropertyTypeEntityKind");
			}
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x060018B1 RID: 6321 RVA: 0x0005648F File Offset: 0x0005468F
		internal static string ODataMessageReader_ExpectedPropertyTypeStream
		{
			get
			{
				return TextRes.GetString("ODataMessageReader_ExpectedPropertyTypeStream");
			}
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x0005649C File Offset: 0x0005469C
		internal static string ODataMessageReader_ExpectedValueTypeWrongKind(object p0)
		{
			return TextRes.GetString("ODataMessageReader_ExpectedValueTypeWrongKind", new object[]
			{
				p0
			});
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x060018B3 RID: 6323 RVA: 0x000564BF File Offset: 0x000546BF
		internal static string ODataMessageReader_NoneOrEmptyContentTypeHeader
		{
			get
			{
				return TextRes.GetString("ODataMessageReader_NoneOrEmptyContentTypeHeader");
			}
		}

		// Token: 0x060018B4 RID: 6324 RVA: 0x000564CC File Offset: 0x000546CC
		internal static string ODataMessageReader_WildcardInContentType(object p0)
		{
			return TextRes.GetString("ODataMessageReader_WildcardInContentType", new object[]
			{
				p0
			});
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x060018B5 RID: 6325 RVA: 0x000564EF File Offset: 0x000546EF
		internal static string ODataMessageReader_EntityReferenceLinksInRequestNotAllowed
		{
			get
			{
				return TextRes.GetString("ODataMessageReader_EntityReferenceLinksInRequestNotAllowed");
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x060018B6 RID: 6326 RVA: 0x000564FB File Offset: 0x000546FB
		internal static string ODataMessageReader_GetFormatCalledBeforeReadingStarted
		{
			get
			{
				return TextRes.GetString("ODataMessageReader_GetFormatCalledBeforeReadingStarted");
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x060018B7 RID: 6327 RVA: 0x00056507 File Offset: 0x00054707
		internal static string ODataMessageReader_DetectPayloadKindMultipleTimes
		{
			get
			{
				return TextRes.GetString("ODataMessageReader_DetectPayloadKindMultipleTimes");
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x060018B8 RID: 6328 RVA: 0x00056513 File Offset: 0x00054713
		internal static string ODataMessageReader_PayloadKindDetectionRunning
		{
			get
			{
				return TextRes.GetString("ODataMessageReader_PayloadKindDetectionRunning");
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x060018B9 RID: 6329 RVA: 0x0005651F File Offset: 0x0005471F
		internal static string ODataMessageReader_PayloadKindDetectionInServerMode
		{
			get
			{
				return TextRes.GetString("ODataMessageReader_PayloadKindDetectionInServerMode");
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x060018BA RID: 6330 RVA: 0x0005652B File Offset: 0x0005472B
		internal static string ODataMessageReader_ParameterPayloadInResponse
		{
			get
			{
				return TextRes.GetString("ODataMessageReader_ParameterPayloadInResponse");
			}
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x00056538 File Offset: 0x00054738
		internal static string ODataMessageReader_SingletonNavigationPropertyForEntityReferenceLinks(object p0, object p1)
		{
			return TextRes.GetString("ODataMessageReader_SingletonNavigationPropertyForEntityReferenceLinks", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x060018BC RID: 6332 RVA: 0x0005655F File Offset: 0x0005475F
		internal static string ODataMessage_MustNotModifyMessage
		{
			get
			{
				return TextRes.GetString("ODataMessage_MustNotModifyMessage");
			}
		}

		// Token: 0x060018BD RID: 6333 RVA: 0x0005656C File Offset: 0x0005476C
		internal static string ODataMediaTypeUtils_BoundaryMustBeSpecifiedForBatchPayloads(object p0, object p1)
		{
			return TextRes.GetString("ODataMediaTypeUtils_BoundaryMustBeSpecifiedForBatchPayloads", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x060018BE RID: 6334 RVA: 0x00056593 File Offset: 0x00054793
		internal static string ODataReaderCore_SyncCallOnAsyncReader
		{
			get
			{
				return TextRes.GetString("ODataReaderCore_SyncCallOnAsyncReader");
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x060018BF RID: 6335 RVA: 0x0005659F File Offset: 0x0005479F
		internal static string ODataReaderCore_AsyncCallOnSyncReader
		{
			get
			{
				return TextRes.GetString("ODataReaderCore_AsyncCallOnSyncReader");
			}
		}

		// Token: 0x060018C0 RID: 6336 RVA: 0x000565AC File Offset: 0x000547AC
		internal static string ODataReaderCore_ReadOrReadAsyncCalledInInvalidState(object p0)
		{
			return TextRes.GetString("ODataReaderCore_ReadOrReadAsyncCalledInInvalidState", new object[]
			{
				p0
			});
		}

		// Token: 0x060018C1 RID: 6337 RVA: 0x000565D0 File Offset: 0x000547D0
		internal static string ODataReaderCore_NoReadCallsAllowed(object p0)
		{
			return TextRes.GetString("ODataReaderCore_NoReadCallsAllowed", new object[]
			{
				p0
			});
		}

		// Token: 0x060018C2 RID: 6338 RVA: 0x000565F4 File Offset: 0x000547F4
		internal static string ODataJsonReader_CannotReadEntriesOfFeed(object p0)
		{
			return TextRes.GetString("ODataJsonReader_CannotReadEntriesOfFeed", new object[]
			{
				p0
			});
		}

		// Token: 0x060018C3 RID: 6339 RVA: 0x00056618 File Offset: 0x00054818
		internal static string ODataJsonReader_CannotReadFeedStart(object p0)
		{
			return TextRes.GetString("ODataJsonReader_CannotReadFeedStart", new object[]
			{
				p0
			});
		}

		// Token: 0x060018C4 RID: 6340 RVA: 0x0005663C File Offset: 0x0005483C
		internal static string ODataJsonReader_CannotReadEntryStart(object p0)
		{
			return TextRes.GetString("ODataJsonReader_CannotReadEntryStart", new object[]
			{
				p0
			});
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x060018C5 RID: 6341 RVA: 0x0005665F File Offset: 0x0005485F
		internal static string ODataJsonReader_ParsingWithoutMetadata
		{
			get
			{
				return TextRes.GetString("ODataJsonReader_ParsingWithoutMetadata");
			}
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x060018C6 RID: 6342 RVA: 0x0005666B File Offset: 0x0005486B
		internal static string ODataJsonReaderUtils_CannotConvertInt64OrDecimal
		{
			get
			{
				return TextRes.GetString("ODataJsonReaderUtils_CannotConvertInt64OrDecimal");
			}
		}

		// Token: 0x060018C7 RID: 6343 RVA: 0x00056678 File Offset: 0x00054878
		internal static string ODataJsonReaderUtils_CannotConvertInt32(object p0)
		{
			return TextRes.GetString("ODataJsonReaderUtils_CannotConvertInt32", new object[]
			{
				p0
			});
		}

		// Token: 0x060018C8 RID: 6344 RVA: 0x0005669C File Offset: 0x0005489C
		internal static string ODataJsonReaderUtils_CannotConvertDouble(object p0)
		{
			return TextRes.GetString("ODataJsonReaderUtils_CannotConvertDouble", new object[]
			{
				p0
			});
		}

		// Token: 0x060018C9 RID: 6345 RVA: 0x000566C0 File Offset: 0x000548C0
		internal static string ODataJsonReaderUtils_CannotConvertBoolean(object p0)
		{
			return TextRes.GetString("ODataJsonReaderUtils_CannotConvertBoolean", new object[]
			{
				p0
			});
		}

		// Token: 0x060018CA RID: 6346 RVA: 0x000566E4 File Offset: 0x000548E4
		internal static string ODataJsonReaderUtils_CannotConvertDateTime(object p0)
		{
			return TextRes.GetString("ODataJsonReaderUtils_CannotConvertDateTime", new object[]
			{
				p0
			});
		}

		// Token: 0x060018CB RID: 6347 RVA: 0x00056708 File Offset: 0x00054908
		internal static string ODataJsonReaderUtils_CannotConvertDateTimeOffset(object p0)
		{
			return TextRes.GetString("ODataJsonReaderUtils_CannotConvertDateTimeOffset", new object[]
			{
				p0
			});
		}

		// Token: 0x060018CC RID: 6348 RVA: 0x0005672C File Offset: 0x0005492C
		internal static string ODataJsonReaderUtils_MultipleMetadataPropertiesWithSameName(object p0)
		{
			return TextRes.GetString("ODataJsonReaderUtils_MultipleMetadataPropertiesWithSameName", new object[]
			{
				p0
			});
		}

		// Token: 0x060018CD RID: 6349 RVA: 0x00056750 File Offset: 0x00054950
		internal static string ODataJsonReaderUtils_MultipleEntityReferenceLinksWrapperPropertiesWithSameName(object p0)
		{
			return TextRes.GetString("ODataJsonReaderUtils_MultipleEntityReferenceLinksWrapperPropertiesWithSameName", new object[]
			{
				p0
			});
		}

		// Token: 0x060018CE RID: 6350 RVA: 0x00056774 File Offset: 0x00054974
		internal static string ODataJsonReaderUtils_MultipleErrorPropertiesWithSameName(object p0)
		{
			return TextRes.GetString("ODataJsonReaderUtils_MultipleErrorPropertiesWithSameName", new object[]
			{
				p0
			});
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x00056798 File Offset: 0x00054998
		internal static string ODataJsonReaderUtils_FeedPropertyWithNullValue(object p0)
		{
			return TextRes.GetString("ODataJsonReaderUtils_FeedPropertyWithNullValue", new object[]
			{
				p0
			});
		}

		// Token: 0x060018D0 RID: 6352 RVA: 0x000567BC File Offset: 0x000549BC
		internal static string ODataJsonReaderUtils_MediaResourcePropertyWithNullValue(object p0)
		{
			return TextRes.GetString("ODataJsonReaderUtils_MediaResourcePropertyWithNullValue", new object[]
			{
				p0
			});
		}

		// Token: 0x060018D1 RID: 6353 RVA: 0x000567E0 File Offset: 0x000549E0
		internal static string ODataJsonReaderUtils_EntityReferenceLinksInlineCountWithNullValue(object p0)
		{
			return TextRes.GetString("ODataJsonReaderUtils_EntityReferenceLinksInlineCountWithNullValue", new object[]
			{
				p0
			});
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x00056804 File Offset: 0x00054A04
		internal static string ODataJsonReaderUtils_EntityReferenceLinksPropertyWithNullValue(object p0)
		{
			return TextRes.GetString("ODataJsonReaderUtils_EntityReferenceLinksPropertyWithNullValue", new object[]
			{
				p0
			});
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x00056828 File Offset: 0x00054A28
		internal static string ODataJsonReaderUtils_MetadataPropertyWithNullValue(object p0)
		{
			return TextRes.GetString("ODataJsonReaderUtils_MetadataPropertyWithNullValue", new object[]
			{
				p0
			});
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x060018D4 RID: 6356 RVA: 0x0005684B File Offset: 0x00054A4B
		internal static string ODataJsonDeserializer_DataWrapperPropertyNotFound
		{
			get
			{
				return TextRes.GetString("ODataJsonDeserializer_DataWrapperPropertyNotFound");
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x060018D5 RID: 6357 RVA: 0x00056857 File Offset: 0x00054A57
		internal static string ODataJsonDeserializer_DataWrapperMultipleProperties
		{
			get
			{
				return TextRes.GetString("ODataJsonDeserializer_DataWrapperMultipleProperties");
			}
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x00056864 File Offset: 0x00054A64
		internal static string ODataJsonDeserializer_RelativeUriUsedWithoutBaseUriSpecified(object p0)
		{
			return TextRes.GetString("ODataJsonDeserializer_RelativeUriUsedWithoutBaseUriSpecified", new object[]
			{
				p0
			});
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x060018D7 RID: 6359 RVA: 0x00056887 File Offset: 0x00054A87
		internal static string ODataJsonCollectionDeserializer_MissingResultsPropertyForCollection
		{
			get
			{
				return TextRes.GetString("ODataJsonCollectionDeserializer_MissingResultsPropertyForCollection");
			}
		}

		// Token: 0x060018D8 RID: 6360 RVA: 0x00056894 File Offset: 0x00054A94
		internal static string ODataJsonCollectionDeserializer_CannotReadCollectionContentStart(object p0)
		{
			return TextRes.GetString("ODataJsonCollectionDeserializer_CannotReadCollectionContentStart", new object[]
			{
				p0
			});
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x060018D9 RID: 6361 RVA: 0x000568B7 File Offset: 0x00054AB7
		internal static string ODataJsonCollectionDeserializer_MultipleResultsPropertiesForCollection
		{
			get
			{
				return TextRes.GetString("ODataJsonCollectionDeserializer_MultipleResultsPropertiesForCollection");
			}
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x060018DA RID: 6362 RVA: 0x000568C3 File Offset: 0x00054AC3
		internal static string ODataJsonEntityReferenceLinkDeserializer_ExpectedEntityReferenceLinksResultsPropertyNotFound
		{
			get
			{
				return TextRes.GetString("ODataJsonEntityReferenceLinkDeserializer_ExpectedEntityReferenceLinksResultsPropertyNotFound");
			}
		}

		// Token: 0x060018DB RID: 6363 RVA: 0x000568D0 File Offset: 0x00054AD0
		internal static string ODataJsonEntityReferenceLinkDeserializer_EntityReferenceLinkMustBeObjectValue(object p0)
		{
			return TextRes.GetString("ODataJsonEntityReferenceLinkDeserializer_EntityReferenceLinkMustBeObjectValue", new object[]
			{
				p0
			});
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x060018DC RID: 6364 RVA: 0x000568F3 File Offset: 0x00054AF3
		internal static string ODataJsonEntityReferenceLinkDeserializer_MultipleUriPropertiesInEntityReferenceLink
		{
			get
			{
				return TextRes.GetString("ODataJsonEntityReferenceLinkDeserializer_MultipleUriPropertiesInEntityReferenceLink");
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x060018DD RID: 6365 RVA: 0x000568FF File Offset: 0x00054AFF
		internal static string ODataJsonEntityReferenceLinkDeserializer_EntityReferenceLinkUriCannotBeNull
		{
			get
			{
				return TextRes.GetString("ODataJsonEntityReferenceLinkDeserializer_EntityReferenceLinkUriCannotBeNull");
			}
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x060018DE RID: 6366 RVA: 0x0005690B File Offset: 0x00054B0B
		internal static string ODataJsonEntryAndFeedDeserializer_ExpectedFeedResultsPropertyNotFound
		{
			get
			{
				return TextRes.GetString("ODataJsonEntryAndFeedDeserializer_ExpectedFeedResultsPropertyNotFound");
			}
		}

		// Token: 0x060018DF RID: 6367 RVA: 0x00056918 File Offset: 0x00054B18
		internal static string ODataJsonEntryAndFeedDeserializer_CannotReadFeedContentStart(object p0)
		{
			return TextRes.GetString("ODataJsonEntryAndFeedDeserializer_CannotReadFeedContentStart", new object[]
			{
				p0
			});
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x060018E0 RID: 6368 RVA: 0x0005693B File Offset: 0x00054B3B
		internal static string ODataJsonEntryAndFeedDeserializer_MultipleMetadataPropertiesInEntryValue
		{
			get
			{
				return TextRes.GetString("ODataJsonEntryAndFeedDeserializer_MultipleMetadataPropertiesInEntryValue");
			}
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x060018E1 RID: 6369 RVA: 0x00056947 File Offset: 0x00054B47
		internal static string ODataJsonEntryAndFeedDeserializer_MultipleUriPropertiesInDeferredLink
		{
			get
			{
				return TextRes.GetString("ODataJsonEntryAndFeedDeserializer_MultipleUriPropertiesInDeferredLink");
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x060018E2 RID: 6370 RVA: 0x00056953 File Offset: 0x00054B53
		internal static string ODataJsonEntryAndFeedDeserializer_DeferredLinkUriCannotBeNull
		{
			get
			{
				return TextRes.GetString("ODataJsonEntryAndFeedDeserializer_DeferredLinkUriCannotBeNull");
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x060018E3 RID: 6371 RVA: 0x0005695F File Offset: 0x00054B5F
		internal static string ODataJsonEntryAndFeedDeserializer_DeferredLinkMissingUri
		{
			get
			{
				return TextRes.GetString("ODataJsonEntryAndFeedDeserializer_DeferredLinkMissingUri");
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x060018E4 RID: 6372 RVA: 0x0005696B File Offset: 0x00054B6B
		internal static string ODataJsonEntryAndFeedDeserializer_CannotReadNavigationPropertyValue
		{
			get
			{
				return TextRes.GetString("ODataJsonEntryAndFeedDeserializer_CannotReadNavigationPropertyValue");
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x060018E5 RID: 6373 RVA: 0x00056977 File Offset: 0x00054B77
		internal static string ODataJsonEntryAndFeedDeserializer_MultipleFeedResultsPropertiesFound
		{
			get
			{
				return TextRes.GetString("ODataJsonEntryAndFeedDeserializer_MultipleFeedResultsPropertiesFound");
			}
		}

		// Token: 0x060018E6 RID: 6374 RVA: 0x00056984 File Offset: 0x00054B84
		internal static string ODataJsonEntryAndFeedDeserializer_MultipleMetadataPropertiesForStreamProperty(object p0)
		{
			return TextRes.GetString("ODataJsonEntryAndFeedDeserializer_MultipleMetadataPropertiesForStreamProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x060018E7 RID: 6375 RVA: 0x000569A7 File Offset: 0x00054BA7
		internal static string ODataJsonEntryAndFeedDeserializer_CannotParseStreamReference
		{
			get
			{
				return TextRes.GetString("ODataJsonEntryAndFeedDeserializer_CannotParseStreamReference");
			}
		}

		// Token: 0x060018E8 RID: 6376 RVA: 0x000569B4 File Offset: 0x00054BB4
		internal static string ODataJsonEntryAndFeedDeserializer_PropertyInEntryMustHaveObjectValue(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonEntryAndFeedDeserializer_PropertyInEntryMustHaveObjectValue", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060018E9 RID: 6377 RVA: 0x000569DC File Offset: 0x00054BDC
		internal static string ODataJsonEntryAndFeedDeserializer_CannotReadSingletonNavigationPropertyValue(object p0)
		{
			return TextRes.GetString("ODataJsonEntryAndFeedDeserializer_CannotReadSingletonNavigationPropertyValue", new object[]
			{
				p0
			});
		}

		// Token: 0x060018EA RID: 6378 RVA: 0x00056A00 File Offset: 0x00054C00
		internal static string ODataJsonEntryAndFeedDeserializer_CannotReadCollectionNavigationPropertyValue(object p0)
		{
			return TextRes.GetString("ODataJsonEntryAndFeedDeserializer_CannotReadCollectionNavigationPropertyValue", new object[]
			{
				p0
			});
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x060018EB RID: 6379 RVA: 0x00056A23 File Offset: 0x00054C23
		internal static string ODataJsonEntryAndFeedDeserializer_StreamPropertyInRequest
		{
			get
			{
				return TextRes.GetString("ODataJsonEntryAndFeedDeserializer_StreamPropertyInRequest");
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x060018EC RID: 6380 RVA: 0x00056A2F File Offset: 0x00054C2F
		internal static string ODataJsonLightEntryAndFeedSerializer_AnnotationGroupWithoutName
		{
			get
			{
				return TextRes.GetString("ODataJsonLightEntryAndFeedSerializer_AnnotationGroupWithoutName");
			}
		}

		// Token: 0x060018ED RID: 6381 RVA: 0x00056A3C File Offset: 0x00054C3C
		internal static string ODataJsonLightEntryAndFeedSerializer_AnnotationGroupMemberWithoutName(object p0)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedSerializer_AnnotationGroupMemberWithoutName", new object[]
			{
				p0
			});
		}

		// Token: 0x060018EE RID: 6382 RVA: 0x00056A60 File Offset: 0x00054C60
		internal static string ODataJsonLightEntryAndFeedSerializer_AnnotationGroupMemberWithInvalidValue(object p0, object p1, object p2)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedSerializer_AnnotationGroupMemberWithInvalidValue", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x060018EF RID: 6383 RVA: 0x00056A8B File Offset: 0x00054C8B
		internal static string ODataJsonLightEntryAndFeedSerializer_AnnotationGroupInRequest
		{
			get
			{
				return TextRes.GetString("ODataJsonLightEntryAndFeedSerializer_AnnotationGroupInRequest");
			}
		}

		// Token: 0x060018F0 RID: 6384 RVA: 0x00056A98 File Offset: 0x00054C98
		internal static string ODataJsonLightEntryAndFeedSerializer_AnnotationGroupMemberMustBeAnnotation(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedSerializer_AnnotationGroupMemberMustBeAnnotation", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060018F1 RID: 6385 RVA: 0x00056AC0 File Offset: 0x00054CC0
		internal static string ODataJsonLightEntryAndFeedSerializer_DuplicateAnnotationGroup(object p0)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedSerializer_DuplicateAnnotationGroup", new object[]
			{
				p0
			});
		}

		// Token: 0x060018F2 RID: 6386 RVA: 0x00056AE4 File Offset: 0x00054CE4
		internal static string ODataJsonLightEntryAndFeedSerializer_ActionsAndFunctionsGroupMustSpecifyTarget(object p0)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedSerializer_ActionsAndFunctionsGroupMustSpecifyTarget", new object[]
			{
				p0
			});
		}

		// Token: 0x060018F3 RID: 6387 RVA: 0x00056B08 File Offset: 0x00054D08
		internal static string ODataJsonLightEntryAndFeedSerializer_ActionsAndFunctionsGroupMustNotHaveDuplicateTarget(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedSerializer_ActionsAndFunctionsGroupMustNotHaveDuplicateTarget", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060018F4 RID: 6388 RVA: 0x00056B30 File Offset: 0x00054D30
		internal static string ODataJsonErrorDeserializer_TopLevelErrorWithInvalidProperty(object p0)
		{
			return TextRes.GetString("ODataJsonErrorDeserializer_TopLevelErrorWithInvalidProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x060018F5 RID: 6389 RVA: 0x00056B54 File Offset: 0x00054D54
		internal static string ODataJsonErrorDeserializer_TopLevelErrorMessageValueWithInvalidProperty(object p0)
		{
			return TextRes.GetString("ODataJsonErrorDeserializer_TopLevelErrorMessageValueWithInvalidProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x060018F6 RID: 6390 RVA: 0x00056B78 File Offset: 0x00054D78
		internal static string ODataVerboseJsonErrorDeserializer_TopLevelErrorValueWithInvalidProperty(object p0)
		{
			return TextRes.GetString("ODataVerboseJsonErrorDeserializer_TopLevelErrorValueWithInvalidProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x060018F7 RID: 6391 RVA: 0x00056B9B File Offset: 0x00054D9B
		internal static string ODataJsonPropertyAndValueDeserializer_TopLevelPropertyWithoutMetadata
		{
			get
			{
				return TextRes.GetString("ODataJsonPropertyAndValueDeserializer_TopLevelPropertyWithoutMetadata");
			}
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x060018F8 RID: 6392 RVA: 0x00056BA7 File Offset: 0x00054DA7
		internal static string ODataJsonPropertyAndValueDeserializer_InvalidTopLevelPropertyPayload
		{
			get
			{
				return TextRes.GetString("ODataJsonPropertyAndValueDeserializer_InvalidTopLevelPropertyPayload");
			}
		}

		// Token: 0x060018F9 RID: 6393 RVA: 0x00056BB4 File Offset: 0x00054DB4
		internal static string ODataJsonPropertyAndValueDeserializer_CannotReadPropertyValue(object p0)
		{
			return TextRes.GetString("ODataJsonPropertyAndValueDeserializer_CannotReadPropertyValue", new object[]
			{
				p0
			});
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x060018FA RID: 6394 RVA: 0x00056BD7 File Offset: 0x00054DD7
		internal static string ODataJsonPropertyAndValueDeserializer_MultipleMetadataPropertiesInComplexValue
		{
			get
			{
				return TextRes.GetString("ODataJsonPropertyAndValueDeserializer_MultipleMetadataPropertiesInComplexValue");
			}
		}

		// Token: 0x060018FB RID: 6395 RVA: 0x00056BE4 File Offset: 0x00054DE4
		internal static string ODataJsonPropertyAndValueDeserializer_MultiplePropertiesInCollectionWrapper(object p0)
		{
			return TextRes.GetString("ODataJsonPropertyAndValueDeserializer_MultiplePropertiesInCollectionWrapper", new object[]
			{
				p0
			});
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x060018FC RID: 6396 RVA: 0x00056C07 File Offset: 0x00054E07
		internal static string ODataJsonPropertyAndValueDeserializer_CollectionWithoutResults
		{
			get
			{
				return TextRes.GetString("ODataJsonPropertyAndValueDeserializer_CollectionWithoutResults");
			}
		}

		// Token: 0x060018FD RID: 6397 RVA: 0x00056C14 File Offset: 0x00054E14
		internal static string ODataJsonPropertyAndValueDeserializer_InvalidTypeName(object p0)
		{
			return TextRes.GetString("ODataJsonPropertyAndValueDeserializer_InvalidTypeName", new object[]
			{
				p0
			});
		}

		// Token: 0x060018FE RID: 6398 RVA: 0x00056C38 File Offset: 0x00054E38
		internal static string ODataJsonPropertyAndValueDeserializer_InvalidPrimitiveTypeName(object p0)
		{
			return TextRes.GetString("ODataJsonPropertyAndValueDeserializer_InvalidPrimitiveTypeName", new object[]
			{
				p0
			});
		}

		// Token: 0x060018FF RID: 6399 RVA: 0x00056C5C File Offset: 0x00054E5C
		internal static string ODataJsonPropertyAndValueDeserializer_MetadataPropertyMustHaveObjectValue(object p0)
		{
			return TextRes.GetString("ODataJsonPropertyAndValueDeserializer_MetadataPropertyMustHaveObjectValue", new object[]
			{
				p0
			});
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06001900 RID: 6400 RVA: 0x00056C7F File Offset: 0x00054E7F
		internal static string ODataJsonServiceDocumentDeserializer_MultipleEntitySetsPropertiesForServiceDocument
		{
			get
			{
				return TextRes.GetString("ODataJsonServiceDocumentDeserializer_MultipleEntitySetsPropertiesForServiceDocument");
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06001901 RID: 6401 RVA: 0x00056C8B File Offset: 0x00054E8B
		internal static string ODataJsonServiceDocumentDeserializer_NoEntitySetsPropertyForServiceDocument
		{
			get
			{
				return TextRes.GetString("ODataJsonServiceDocumentDeserializer_NoEntitySetsPropertyForServiceDocument");
			}
		}

		// Token: 0x06001902 RID: 6402 RVA: 0x00056C98 File Offset: 0x00054E98
		internal static string ODataCollectionReaderCore_ReadOrReadAsyncCalledInInvalidState(object p0)
		{
			return TextRes.GetString("ODataCollectionReaderCore_ReadOrReadAsyncCalledInInvalidState", new object[]
			{
				p0
			});
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06001903 RID: 6403 RVA: 0x00056CBB File Offset: 0x00054EBB
		internal static string ODataCollectionReaderCore_SyncCallOnAsyncReader
		{
			get
			{
				return TextRes.GetString("ODataCollectionReaderCore_SyncCallOnAsyncReader");
			}
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001904 RID: 6404 RVA: 0x00056CC7 File Offset: 0x00054EC7
		internal static string ODataCollectionReaderCore_AsyncCallOnSyncReader
		{
			get
			{
				return TextRes.GetString("ODataCollectionReaderCore_AsyncCallOnSyncReader");
			}
		}

		// Token: 0x06001905 RID: 6405 RVA: 0x00056CD4 File Offset: 0x00054ED4
		internal static string ODataCollectionReaderCore_ExpectedItemTypeSetInInvalidState(object p0, object p1)
		{
			return TextRes.GetString("ODataCollectionReaderCore_ExpectedItemTypeSetInInvalidState", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001906 RID: 6406 RVA: 0x00056CFC File Offset: 0x00054EFC
		internal static string ODataParameterReaderCore_ReadOrReadAsyncCalledInInvalidState(object p0)
		{
			return TextRes.GetString("ODataParameterReaderCore_ReadOrReadAsyncCalledInInvalidState", new object[]
			{
				p0
			});
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06001907 RID: 6407 RVA: 0x00056D1F File Offset: 0x00054F1F
		internal static string ODataParameterReaderCore_SyncCallOnAsyncReader
		{
			get
			{
				return TextRes.GetString("ODataParameterReaderCore_SyncCallOnAsyncReader");
			}
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06001908 RID: 6408 RVA: 0x00056D2B File Offset: 0x00054F2B
		internal static string ODataParameterReaderCore_AsyncCallOnSyncReader
		{
			get
			{
				return TextRes.GetString("ODataParameterReaderCore_AsyncCallOnSyncReader");
			}
		}

		// Token: 0x06001909 RID: 6409 RVA: 0x00056D38 File Offset: 0x00054F38
		internal static string ODataParameterReaderCore_SubReaderMustBeCreatedAndReadToCompletionBeforeTheNextReadOrReadAsyncCall(object p0, object p1)
		{
			return TextRes.GetString("ODataParameterReaderCore_SubReaderMustBeCreatedAndReadToCompletionBeforeTheNextReadOrReadAsyncCall", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600190A RID: 6410 RVA: 0x00056D60 File Offset: 0x00054F60
		internal static string ODataParameterReaderCore_SubReaderMustBeInCompletedStateBeforeTheNextReadOrReadAsyncCall(object p0, object p1)
		{
			return TextRes.GetString("ODataParameterReaderCore_SubReaderMustBeInCompletedStateBeforeTheNextReadOrReadAsyncCall", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600190B RID: 6411 RVA: 0x00056D88 File Offset: 0x00054F88
		internal static string ODataParameterReaderCore_InvalidCreateReaderMethodCalledForState(object p0, object p1)
		{
			return TextRes.GetString("ODataParameterReaderCore_InvalidCreateReaderMethodCalledForState", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600190C RID: 6412 RVA: 0x00056DB0 File Offset: 0x00054FB0
		internal static string ODataParameterReaderCore_CreateReaderAlreadyCalled(object p0, object p1)
		{
			return TextRes.GetString("ODataParameterReaderCore_CreateReaderAlreadyCalled", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x00056DD8 File Offset: 0x00054FD8
		internal static string ODataParameterReaderCore_ParameterNameNotInMetadata(object p0, object p1)
		{
			return TextRes.GetString("ODataParameterReaderCore_ParameterNameNotInMetadata", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600190E RID: 6414 RVA: 0x00056E00 File Offset: 0x00055000
		internal static string ODataParameterReaderCore_DuplicateParametersInPayload(object p0)
		{
			return TextRes.GetString("ODataParameterReaderCore_DuplicateParametersInPayload", new object[]
			{
				p0
			});
		}

		// Token: 0x0600190F RID: 6415 RVA: 0x00056E24 File Offset: 0x00055024
		internal static string ODataParameterReaderCore_ParametersMissingInPayload(object p0, object p1)
		{
			return TextRes.GetString("ODataParameterReaderCore_ParametersMissingInPayload", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001910 RID: 6416 RVA: 0x00056E4C File Offset: 0x0005504C
		internal static string ODataJsonParameterReader_UnsupportedPrimitiveParameterType(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonParameterReader_UnsupportedPrimitiveParameterType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001911 RID: 6417 RVA: 0x00056E74 File Offset: 0x00055074
		internal static string ODataJsonParameterReader_UnsupportedParameterTypeKind(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonParameterReader_UnsupportedParameterTypeKind", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001912 RID: 6418 RVA: 0x00056E9C File Offset: 0x0005509C
		internal static string ODataJsonParameterReader_NullCollectionExpected(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonParameterReader_NullCollectionExpected", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001913 RID: 6419 RVA: 0x00056EC4 File Offset: 0x000550C4
		internal static string ODataJsonInputContext_FunctionImportCannotBeNullForCreateParameterReader(object p0)
		{
			return TextRes.GetString("ODataJsonInputContext_FunctionImportCannotBeNullForCreateParameterReader", new object[]
			{
				p0
			});
		}

		// Token: 0x06001914 RID: 6420 RVA: 0x00056EE8 File Offset: 0x000550E8
		internal static string ODataJsonCollectionReader_CannotReadWrappedCollectionStart(object p0)
		{
			return TextRes.GetString("ODataJsonCollectionReader_CannotReadWrappedCollectionStart", new object[]
			{
				p0
			});
		}

		// Token: 0x06001915 RID: 6421 RVA: 0x00056F0C File Offset: 0x0005510C
		internal static string ODataJsonCollectionReader_CannotReadCollectionStart(object p0)
		{
			return TextRes.GetString("ODataJsonCollectionReader_CannotReadCollectionStart", new object[]
			{
				p0
			});
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06001916 RID: 6422 RVA: 0x00056F2F File Offset: 0x0005512F
		internal static string ODataJsonCollectionReader_ParsingWithoutMetadata
		{
			get
			{
				return TextRes.GetString("ODataJsonCollectionReader_ParsingWithoutMetadata");
			}
		}

		// Token: 0x06001917 RID: 6423 RVA: 0x00056F3C File Offset: 0x0005513C
		internal static string ValidationUtils_ActionsAndFunctionsMustSpecifyMetadata(object p0)
		{
			return TextRes.GetString("ValidationUtils_ActionsAndFunctionsMustSpecifyMetadata", new object[]
			{
				p0
			});
		}

		// Token: 0x06001918 RID: 6424 RVA: 0x00056F60 File Offset: 0x00055160
		internal static string ValidationUtils_ActionsAndFunctionsMustSpecifyTarget(object p0)
		{
			return TextRes.GetString("ValidationUtils_ActionsAndFunctionsMustSpecifyTarget", new object[]
			{
				p0
			});
		}

		// Token: 0x06001919 RID: 6425 RVA: 0x00056F84 File Offset: 0x00055184
		internal static string ValidationUtils_EnumerableContainsANullItem(object p0)
		{
			return TextRes.GetString("ValidationUtils_EnumerableContainsANullItem", new object[]
			{
				p0
			});
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x0600191A RID: 6426 RVA: 0x00056FA7 File Offset: 0x000551A7
		internal static string ValidationUtils_AssociationLinkMustSpecifyName
		{
			get
			{
				return TextRes.GetString("ValidationUtils_AssociationLinkMustSpecifyName");
			}
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x0600191B RID: 6427 RVA: 0x00056FB3 File Offset: 0x000551B3
		internal static string ValidationUtils_AssociationLinkMustSpecifyUrl
		{
			get
			{
				return TextRes.GetString("ValidationUtils_AssociationLinkMustSpecifyUrl");
			}
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x0600191C RID: 6428 RVA: 0x00056FBF File Offset: 0x000551BF
		internal static string ValidationUtils_TypeNameMustNotBeEmpty
		{
			get
			{
				return TextRes.GetString("ValidationUtils_TypeNameMustNotBeEmpty");
			}
		}

		// Token: 0x0600191D RID: 6429 RVA: 0x00056FCC File Offset: 0x000551CC
		internal static string ValidationUtils_PropertyDoesNotExistOnType(object p0, object p1)
		{
			return TextRes.GetString("ValidationUtils_PropertyDoesNotExistOnType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x0600191E RID: 6430 RVA: 0x00056FF3 File Offset: 0x000551F3
		internal static string ValidationUtils_ResourceCollectionMustSpecifyUrl
		{
			get
			{
				return TextRes.GetString("ValidationUtils_ResourceCollectionMustSpecifyUrl");
			}
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x0600191F RID: 6431 RVA: 0x00056FFF File Offset: 0x000551FF
		internal static string ValidationUtils_ResourceCollectionUrlMustNotBeNull
		{
			get
			{
				return TextRes.GetString("ValidationUtils_ResourceCollectionUrlMustNotBeNull");
			}
		}

		// Token: 0x06001920 RID: 6432 RVA: 0x0005700C File Offset: 0x0005520C
		internal static string ValidationUtils_NonPrimitiveTypeForPrimitiveValue(object p0)
		{
			return TextRes.GetString("ValidationUtils_NonPrimitiveTypeForPrimitiveValue", new object[]
			{
				p0
			});
		}

		// Token: 0x06001921 RID: 6433 RVA: 0x00057030 File Offset: 0x00055230
		internal static string ValidationUtils_UnsupportedPrimitiveType(object p0)
		{
			return TextRes.GetString("ValidationUtils_UnsupportedPrimitiveType", new object[]
			{
				p0
			});
		}

		// Token: 0x06001922 RID: 6434 RVA: 0x00057054 File Offset: 0x00055254
		internal static string ValidationUtils_IncompatiblePrimitiveItemType(object p0, object p1, object p2, object p3)
		{
			return TextRes.GetString("ValidationUtils_IncompatiblePrimitiveItemType", new object[]
			{
				p0,
				p1,
				p2,
				p3
			});
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06001923 RID: 6435 RVA: 0x00057083 File Offset: 0x00055283
		internal static string ValidationUtils_NonStreamingCollectionElementsMustNotBeNull
		{
			get
			{
				return TextRes.GetString("ValidationUtils_NonStreamingCollectionElementsMustNotBeNull");
			}
		}

		// Token: 0x06001924 RID: 6436 RVA: 0x00057090 File Offset: 0x00055290
		internal static string ValidationUtils_InvalidCollectionTypeName(object p0)
		{
			return TextRes.GetString("ValidationUtils_InvalidCollectionTypeName", new object[]
			{
				p0
			});
		}

		// Token: 0x06001925 RID: 6437 RVA: 0x000570B4 File Offset: 0x000552B4
		internal static string ValidationUtils_UnrecognizedTypeName(object p0)
		{
			return TextRes.GetString("ValidationUtils_UnrecognizedTypeName", new object[]
			{
				p0
			});
		}

		// Token: 0x06001926 RID: 6438 RVA: 0x000570D8 File Offset: 0x000552D8
		internal static string ValidationUtils_IncorrectTypeKind(object p0, object p1, object p2)
		{
			return TextRes.GetString("ValidationUtils_IncorrectTypeKind", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x00057104 File Offset: 0x00055304
		internal static string ValidationUtils_IncorrectTypeKindNoTypeName(object p0, object p1)
		{
			return TextRes.GetString("ValidationUtils_IncorrectTypeKindNoTypeName", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001928 RID: 6440 RVA: 0x0005712C File Offset: 0x0005532C
		internal static string ValidationUtils_IncorrectValueTypeKind(object p0, object p1)
		{
			return TextRes.GetString("ValidationUtils_IncorrectValueTypeKind", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06001929 RID: 6441 RVA: 0x00057153 File Offset: 0x00055353
		internal static string ValidationUtils_LinkMustSpecifyName
		{
			get
			{
				return TextRes.GetString("ValidationUtils_LinkMustSpecifyName");
			}
		}

		// Token: 0x0600192A RID: 6442 RVA: 0x00057160 File Offset: 0x00055360
		internal static string ValidationUtils_MismatchPropertyKindForStreamProperty(object p0)
		{
			return TextRes.GetString("ValidationUtils_MismatchPropertyKindForStreamProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x0600192B RID: 6443 RVA: 0x00057184 File Offset: 0x00055384
		internal static string ValidationUtils_InvalidEtagValue(object p0)
		{
			return TextRes.GetString("ValidationUtils_InvalidEtagValue", new object[]
			{
				p0
			});
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x0600192C RID: 6444 RVA: 0x000571A7 File Offset: 0x000553A7
		internal static string ValidationUtils_NestedCollectionsAreNotSupported
		{
			get
			{
				return TextRes.GetString("ValidationUtils_NestedCollectionsAreNotSupported");
			}
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x0600192D RID: 6445 RVA: 0x000571B3 File Offset: 0x000553B3
		internal static string ValidationUtils_StreamReferenceValuesNotSupportedInCollections
		{
			get
			{
				return TextRes.GetString("ValidationUtils_StreamReferenceValuesNotSupportedInCollections");
			}
		}

		// Token: 0x0600192E RID: 6446 RVA: 0x000571C0 File Offset: 0x000553C0
		internal static string ValidationUtils_IncompatibleType(object p0, object p1)
		{
			return TextRes.GetString("ValidationUtils_IncompatibleType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600192F RID: 6447 RVA: 0x000571E8 File Offset: 0x000553E8
		internal static string ValidationUtils_OpenCollectionProperty(object p0)
		{
			return TextRes.GetString("ValidationUtils_OpenCollectionProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x06001930 RID: 6448 RVA: 0x0005720C File Offset: 0x0005540C
		internal static string ValidationUtils_OpenStreamProperty(object p0)
		{
			return TextRes.GetString("ValidationUtils_OpenStreamProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x06001931 RID: 6449 RVA: 0x00057230 File Offset: 0x00055430
		internal static string ValidationUtils_InvalidCollectionTypeReference(object p0)
		{
			return TextRes.GetString("ValidationUtils_InvalidCollectionTypeReference", new object[]
			{
				p0
			});
		}

		// Token: 0x06001932 RID: 6450 RVA: 0x00057254 File Offset: 0x00055454
		internal static string ValidationUtils_EntryWithMediaResourceAndNonMLEType(object p0)
		{
			return TextRes.GetString("ValidationUtils_EntryWithMediaResourceAndNonMLEType", new object[]
			{
				p0
			});
		}

		// Token: 0x06001933 RID: 6451 RVA: 0x00057278 File Offset: 0x00055478
		internal static string ValidationUtils_EntryWithoutMediaResourceAndMLEType(object p0)
		{
			return TextRes.GetString("ValidationUtils_EntryWithoutMediaResourceAndMLEType", new object[]
			{
				p0
			});
		}

		// Token: 0x06001934 RID: 6452 RVA: 0x0005729C File Offset: 0x0005549C
		internal static string ValidationUtils_EntryTypeNotAssignableToExpectedType(object p0, object p1)
		{
			return TextRes.GetString("ValidationUtils_EntryTypeNotAssignableToExpectedType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001935 RID: 6453 RVA: 0x000572C4 File Offset: 0x000554C4
		internal static string ValidationUtils_OpenNavigationProperty(object p0, object p1)
		{
			return TextRes.GetString("ValidationUtils_OpenNavigationProperty", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001936 RID: 6454 RVA: 0x000572EC File Offset: 0x000554EC
		internal static string ValidationUtils_NavigationPropertyExpected(object p0, object p1, object p2)
		{
			return TextRes.GetString("ValidationUtils_NavigationPropertyExpected", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x06001937 RID: 6455 RVA: 0x00057318 File Offset: 0x00055518
		internal static string ValidationUtils_InvalidBatchBoundaryDelimiterLength(object p0, object p1)
		{
			return TextRes.GetString("ValidationUtils_InvalidBatchBoundaryDelimiterLength", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001938 RID: 6456 RVA: 0x00057340 File Offset: 0x00055540
		internal static string ValidationUtils_RecursionDepthLimitReached(object p0)
		{
			return TextRes.GetString("ValidationUtils_RecursionDepthLimitReached", new object[]
			{
				p0
			});
		}

		// Token: 0x06001939 RID: 6457 RVA: 0x00057364 File Offset: 0x00055564
		internal static string ValidationUtils_MaxDepthOfNestedEntriesExceeded(object p0)
		{
			return TextRes.GetString("ValidationUtils_MaxDepthOfNestedEntriesExceeded", new object[]
			{
				p0
			});
		}

		// Token: 0x0600193A RID: 6458 RVA: 0x00057388 File Offset: 0x00055588
		internal static string ValidationUtils_NullCollectionItemForNonNullableType(object p0)
		{
			return TextRes.GetString("ValidationUtils_NullCollectionItemForNonNullableType", new object[]
			{
				p0
			});
		}

		// Token: 0x0600193B RID: 6459 RVA: 0x000573AC File Offset: 0x000555AC
		internal static string ValidationUtils_PropertiesMustNotContainReservedChars(object p0, object p1)
		{
			return TextRes.GetString("ValidationUtils_PropertiesMustNotContainReservedChars", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600193C RID: 6460 RVA: 0x000573D4 File Offset: 0x000555D4
		internal static string ValidationUtils_MaxNumberOfEntityPropertyMappingsExceeded(object p0, object p1)
		{
			return TextRes.GetString("ValidationUtils_MaxNumberOfEntityPropertyMappingsExceeded", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x0600193D RID: 6461 RVA: 0x000573FB File Offset: 0x000555FB
		internal static string ValidationUtils_WorkspaceCollectionsMustNotContainNullItem
		{
			get
			{
				return TextRes.GetString("ValidationUtils_WorkspaceCollectionsMustNotContainNullItem");
			}
		}

		// Token: 0x0600193E RID: 6462 RVA: 0x00057408 File Offset: 0x00055608
		internal static string ValidationUtils_InvalidMetadataReferenceProperty(object p0)
		{
			return TextRes.GetString("ValidationUtils_InvalidMetadataReferenceProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x0600193F RID: 6463 RVA: 0x0005742B File Offset: 0x0005562B
		internal static string ODataAtomWriter_FeedsMustHaveNonEmptyId
		{
			get
			{
				return TextRes.GetString("ODataAtomWriter_FeedsMustHaveNonEmptyId");
			}
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06001940 RID: 6464 RVA: 0x00057437 File Offset: 0x00055637
		internal static string WriterValidationUtils_PropertyMustNotBeNull
		{
			get
			{
				return TextRes.GetString("WriterValidationUtils_PropertyMustNotBeNull");
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06001941 RID: 6465 RVA: 0x00057443 File Offset: 0x00055643
		internal static string WriterValidationUtils_PropertiesMustHaveNonEmptyName
		{
			get
			{
				return TextRes.GetString("WriterValidationUtils_PropertiesMustHaveNonEmptyName");
			}
		}

		// Token: 0x06001942 RID: 6466 RVA: 0x00057450 File Offset: 0x00055650
		internal static string WriterValidationUtils_PropertyNameDoesntMatchFunctionImportName(object p0, object p1)
		{
			return TextRes.GetString("WriterValidationUtils_PropertyNameDoesntMatchFunctionImportName", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06001943 RID: 6467 RVA: 0x00057477 File Offset: 0x00055677
		internal static string WriterValidationUtils_MissingTypeNameWithMetadata
		{
			get
			{
				return TextRes.GetString("WriterValidationUtils_MissingTypeNameWithMetadata");
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06001944 RID: 6468 RVA: 0x00057483 File Offset: 0x00055683
		internal static string WriterValidationUtils_NextPageLinkInRequest
		{
			get
			{
				return TextRes.GetString("WriterValidationUtils_NextPageLinkInRequest");
			}
		}

		// Token: 0x06001945 RID: 6469 RVA: 0x00057490 File Offset: 0x00055690
		internal static string WriterValidationUtils_ResourceCollectionMustHaveUniqueName(object p0)
		{
			return TextRes.GetString("WriterValidationUtils_ResourceCollectionMustHaveUniqueName", new object[]
			{
				p0
			});
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06001946 RID: 6470 RVA: 0x000574B3 File Offset: 0x000556B3
		internal static string WriterValidationUtils_DefaultStreamWithContentTypeWithoutReadLink
		{
			get
			{
				return TextRes.GetString("WriterValidationUtils_DefaultStreamWithContentTypeWithoutReadLink");
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06001947 RID: 6471 RVA: 0x000574BF File Offset: 0x000556BF
		internal static string WriterValidationUtils_DefaultStreamWithReadLinkWithoutContentType
		{
			get
			{
				return TextRes.GetString("WriterValidationUtils_DefaultStreamWithReadLinkWithoutContentType");
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06001948 RID: 6472 RVA: 0x000574CB File Offset: 0x000556CB
		internal static string WriterValidationUtils_StreamReferenceValueMustHaveEditLinkOrReadLink
		{
			get
			{
				return TextRes.GetString("WriterValidationUtils_StreamReferenceValueMustHaveEditLinkOrReadLink");
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06001949 RID: 6473 RVA: 0x000574D7 File Offset: 0x000556D7
		internal static string WriterValidationUtils_StreamReferenceValueMustHaveEditLinkToHaveETag
		{
			get
			{
				return TextRes.GetString("WriterValidationUtils_StreamReferenceValueMustHaveEditLinkToHaveETag");
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x0600194A RID: 6474 RVA: 0x000574E3 File Offset: 0x000556E3
		internal static string WriterValidationUtils_StreamReferenceValueEmptyContentType
		{
			get
			{
				return TextRes.GetString("WriterValidationUtils_StreamReferenceValueEmptyContentType");
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x0600194B RID: 6475 RVA: 0x000574EF File Offset: 0x000556EF
		internal static string WriterValidationUtils_EntriesMustHaveNonEmptyId
		{
			get
			{
				return TextRes.GetString("WriterValidationUtils_EntriesMustHaveNonEmptyId");
			}
		}

		// Token: 0x0600194C RID: 6476 RVA: 0x000574FC File Offset: 0x000556FC
		internal static string WriterValidationUtils_MessageWriterSettingsBaseUriMustBeNullOrAbsolute(object p0)
		{
			return TextRes.GetString("WriterValidationUtils_MessageWriterSettingsBaseUriMustBeNullOrAbsolute", new object[]
			{
				p0
			});
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x0600194D RID: 6477 RVA: 0x0005751F File Offset: 0x0005571F
		internal static string WriterValidationUtils_EntityReferenceLinkUrlMustNotBeNull
		{
			get
			{
				return TextRes.GetString("WriterValidationUtils_EntityReferenceLinkUrlMustNotBeNull");
			}
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x0600194E RID: 6478 RVA: 0x0005752B File Offset: 0x0005572B
		internal static string WriterValidationUtils_EntityReferenceLinksLinkMustNotBeNull
		{
			get
			{
				return TextRes.GetString("WriterValidationUtils_EntityReferenceLinksLinkMustNotBeNull");
			}
		}

		// Token: 0x0600194F RID: 6479 RVA: 0x00057538 File Offset: 0x00055738
		internal static string WriterValidationUtils_EntryTypeInExpandedLinkNotCompatibleWithNavigationPropertyType(object p0, object p1)
		{
			return TextRes.GetString("WriterValidationUtils_EntryTypeInExpandedLinkNotCompatibleWithNavigationPropertyType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x00057560 File Offset: 0x00055760
		internal static string WriterValidationUtils_ExpandedLinkIsCollectionTrueWithEntryContent(object p0)
		{
			return TextRes.GetString("WriterValidationUtils_ExpandedLinkIsCollectionTrueWithEntryContent", new object[]
			{
				p0
			});
		}

		// Token: 0x06001951 RID: 6481 RVA: 0x00057584 File Offset: 0x00055784
		internal static string WriterValidationUtils_ExpandedLinkIsCollectionFalseWithFeedContent(object p0)
		{
			return TextRes.GetString("WriterValidationUtils_ExpandedLinkIsCollectionFalseWithFeedContent", new object[]
			{
				p0
			});
		}

		// Token: 0x06001952 RID: 6482 RVA: 0x000575A8 File Offset: 0x000557A8
		internal static string WriterValidationUtils_ExpandedLinkIsCollectionTrueWithEntryMetadata(object p0)
		{
			return TextRes.GetString("WriterValidationUtils_ExpandedLinkIsCollectionTrueWithEntryMetadata", new object[]
			{
				p0
			});
		}

		// Token: 0x06001953 RID: 6483 RVA: 0x000575CC File Offset: 0x000557CC
		internal static string WriterValidationUtils_ExpandedLinkIsCollectionFalseWithFeedMetadata(object p0)
		{
			return TextRes.GetString("WriterValidationUtils_ExpandedLinkIsCollectionFalseWithFeedMetadata", new object[]
			{
				p0
			});
		}

		// Token: 0x06001954 RID: 6484 RVA: 0x000575F0 File Offset: 0x000557F0
		internal static string WriterValidationUtils_ExpandedLinkWithFeedPayloadAndEntryMetadata(object p0)
		{
			return TextRes.GetString("WriterValidationUtils_ExpandedLinkWithFeedPayloadAndEntryMetadata", new object[]
			{
				p0
			});
		}

		// Token: 0x06001955 RID: 6485 RVA: 0x00057614 File Offset: 0x00055814
		internal static string WriterValidationUtils_ExpandedLinkWithEntryPayloadAndFeedMetadata(object p0)
		{
			return TextRes.GetString("WriterValidationUtils_ExpandedLinkWithEntryPayloadAndFeedMetadata", new object[]
			{
				p0
			});
		}

		// Token: 0x06001956 RID: 6486 RVA: 0x00057638 File Offset: 0x00055838
		internal static string WriterValidationUtils_CollectionPropertiesMustNotHaveNullValue(object p0)
		{
			return TextRes.GetString("WriterValidationUtils_CollectionPropertiesMustNotHaveNullValue", new object[]
			{
				p0
			});
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x0005765C File Offset: 0x0005585C
		internal static string WriterValidationUtils_NonNullablePropertiesMustNotHaveNullValue(object p0, object p1)
		{
			return TextRes.GetString("WriterValidationUtils_NonNullablePropertiesMustNotHaveNullValue", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001958 RID: 6488 RVA: 0x00057684 File Offset: 0x00055884
		internal static string WriterValidationUtils_StreamPropertiesMustNotHaveNullValue(object p0)
		{
			return TextRes.GetString("WriterValidationUtils_StreamPropertiesMustNotHaveNullValue", new object[]
			{
				p0
			});
		}

		// Token: 0x06001959 RID: 6489 RVA: 0x000576A8 File Offset: 0x000558A8
		internal static string WriterValidationUtils_OperationInRequest(object p0)
		{
			return TextRes.GetString("WriterValidationUtils_OperationInRequest", new object[]
			{
				p0
			});
		}

		// Token: 0x0600195A RID: 6490 RVA: 0x000576CC File Offset: 0x000558CC
		internal static string WriterValidationUtils_AssociationLinkInRequest(object p0)
		{
			return TextRes.GetString("WriterValidationUtils_AssociationLinkInRequest", new object[]
			{
				p0
			});
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x000576F0 File Offset: 0x000558F0
		internal static string WriterValidationUtils_StreamPropertyInRequest(object p0)
		{
			return TextRes.GetString("WriterValidationUtils_StreamPropertyInRequest", new object[]
			{
				p0
			});
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x00057714 File Offset: 0x00055914
		internal static string WriterValidationUtils_MessageWriterSettingsMetadataDocumentUriMustBeNullOrAbsolute(object p0)
		{
			return TextRes.GetString("WriterValidationUtils_MessageWriterSettingsMetadataDocumentUriMustBeNullOrAbsolute", new object[]
			{
				p0
			});
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x00057738 File Offset: 0x00055938
		internal static string WriterValidationUtils_NavigationLinkMustSpecifyUrl(object p0)
		{
			return TextRes.GetString("WriterValidationUtils_NavigationLinkMustSpecifyUrl", new object[]
			{
				p0
			});
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x0005775C File Offset: 0x0005595C
		internal static string WriterValidationUtils_NavigationLinkMustSpecifyIsCollection(object p0)
		{
			return TextRes.GetString("WriterValidationUtils_NavigationLinkMustSpecifyIsCollection", new object[]
			{
				p0
			});
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x0600195F RID: 6495 RVA: 0x0005777F File Offset: 0x0005597F
		internal static string WriterValidationUtils_MessageWriterSettingsJsonPaddingOnRequestMessage
		{
			get
			{
				return TextRes.GetString("WriterValidationUtils_MessageWriterSettingsJsonPaddingOnRequestMessage");
			}
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x0005778C File Offset: 0x0005598C
		internal static string XmlReaderExtension_InvalidNodeInStringValue(object p0)
		{
			return TextRes.GetString("XmlReaderExtension_InvalidNodeInStringValue", new object[]
			{
				p0
			});
		}

		// Token: 0x06001961 RID: 6497 RVA: 0x000577B0 File Offset: 0x000559B0
		internal static string XmlReaderExtension_InvalidRootNode(object p0)
		{
			return TextRes.GetString("XmlReaderExtension_InvalidRootNode", new object[]
			{
				p0
			});
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x000577D4 File Offset: 0x000559D4
		internal static string ODataAtomInputContext_NonEmptyElementWithNullAttribute(object p0)
		{
			return TextRes.GetString("ODataAtomInputContext_NonEmptyElementWithNullAttribute", new object[]
			{
				p0
			});
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x000577F8 File Offset: 0x000559F8
		internal static string ODataMetadataInputContext_ErrorReadingMetadata(object p0)
		{
			return TextRes.GetString("ODataMetadataInputContext_ErrorReadingMetadata", new object[]
			{
				p0
			});
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x0005781C File Offset: 0x00055A1C
		internal static string ODataMetadataOutputContext_ErrorWritingMetadata(object p0)
		{
			return TextRes.GetString("ODataMetadataOutputContext_ErrorWritingMetadata", new object[]
			{
				p0
			});
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x00057840 File Offset: 0x00055A40
		internal static string EpmExtensionMethods_InvalidKeepInContentOnType(object p0, object p1)
		{
			return TextRes.GetString("EpmExtensionMethods_InvalidKeepInContentOnType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001966 RID: 6502 RVA: 0x00057868 File Offset: 0x00055A68
		internal static string EpmExtensionMethods_InvalidKeepInContentOnProperty(object p0, object p1, object p2)
		{
			return TextRes.GetString("EpmExtensionMethods_InvalidKeepInContentOnProperty", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x00057894 File Offset: 0x00055A94
		internal static string EpmExtensionMethods_InvalidTargetTextContentKindOnType(object p0, object p1)
		{
			return TextRes.GetString("EpmExtensionMethods_InvalidTargetTextContentKindOnType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001968 RID: 6504 RVA: 0x000578BC File Offset: 0x00055ABC
		internal static string EpmExtensionMethods_InvalidTargetTextContentKindOnProperty(object p0, object p1, object p2)
		{
			return TextRes.GetString("EpmExtensionMethods_InvalidTargetTextContentKindOnProperty", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x000578E8 File Offset: 0x00055AE8
		internal static string EpmExtensionMethods_MissingAttributeOnType(object p0, object p1)
		{
			return TextRes.GetString("EpmExtensionMethods_MissingAttributeOnType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x00057910 File Offset: 0x00055B10
		internal static string EpmExtensionMethods_MissingAttributeOnProperty(object p0, object p1, object p2)
		{
			return TextRes.GetString("EpmExtensionMethods_MissingAttributeOnProperty", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x0005793C File Offset: 0x00055B3C
		internal static string EpmExtensionMethods_AttributeNotAllowedForCustomMappingOnType(object p0, object p1)
		{
			return TextRes.GetString("EpmExtensionMethods_AttributeNotAllowedForCustomMappingOnType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600196C RID: 6508 RVA: 0x00057964 File Offset: 0x00055B64
		internal static string EpmExtensionMethods_AttributeNotAllowedForCustomMappingOnProperty(object p0, object p1, object p2)
		{
			return TextRes.GetString("EpmExtensionMethods_AttributeNotAllowedForCustomMappingOnProperty", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x0600196D RID: 6509 RVA: 0x00057990 File Offset: 0x00055B90
		internal static string EpmExtensionMethods_AttributeNotAllowedForAtomPubMappingOnType(object p0, object p1)
		{
			return TextRes.GetString("EpmExtensionMethods_AttributeNotAllowedForAtomPubMappingOnType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x000579B8 File Offset: 0x00055BB8
		internal static string EpmExtensionMethods_AttributeNotAllowedForAtomPubMappingOnProperty(object p0, object p1, object p2)
		{
			return TextRes.GetString("EpmExtensionMethods_AttributeNotAllowedForAtomPubMappingOnProperty", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x000579E4 File Offset: 0x00055BE4
		internal static string EpmExtensionMethods_CannotConvertEdmAnnotationValue(object p0, object p1, object p2)
		{
			return TextRes.GetString("EpmExtensionMethods_CannotConvertEdmAnnotationValue", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06001970 RID: 6512 RVA: 0x00057A0F File Offset: 0x00055C0F
		internal static string ODataAtomReader_MediaLinkEntryMismatch
		{
			get
			{
				return TextRes.GetString("ODataAtomReader_MediaLinkEntryMismatch");
			}
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x00057A1C File Offset: 0x00055C1C
		internal static string ODataAtomReader_FeedNavigationLinkForResourceReferenceProperty(object p0)
		{
			return TextRes.GetString("ODataAtomReader_FeedNavigationLinkForResourceReferenceProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06001972 RID: 6514 RVA: 0x00057A3F File Offset: 0x00055C3F
		internal static string ODataAtomReader_ExpandedFeedInEntryNavigationLink
		{
			get
			{
				return TextRes.GetString("ODataAtomReader_ExpandedFeedInEntryNavigationLink");
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06001973 RID: 6515 RVA: 0x00057A4B File Offset: 0x00055C4B
		internal static string ODataAtomReader_ExpandedEntryInFeedNavigationLink
		{
			get
			{
				return TextRes.GetString("ODataAtomReader_ExpandedEntryInFeedNavigationLink");
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06001974 RID: 6516 RVA: 0x00057A57 File Offset: 0x00055C57
		internal static string ODataAtomReader_DeferredEntryInFeedNavigationLink
		{
			get
			{
				return TextRes.GetString("ODataAtomReader_DeferredEntryInFeedNavigationLink");
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06001975 RID: 6517 RVA: 0x00057A63 File Offset: 0x00055C63
		internal static string ODataAtomReader_EntryXmlCustomizationCallbackReturnedSameInstance
		{
			get
			{
				return TextRes.GetString("ODataAtomReader_EntryXmlCustomizationCallbackReturnedSameInstance");
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06001976 RID: 6518 RVA: 0x00057A6F File Offset: 0x00055C6F
		internal static string ODataAtomReaderUtils_InvalidTypeName
		{
			get
			{
				return TextRes.GetString("ODataAtomReaderUtils_InvalidTypeName");
			}
		}

		// Token: 0x06001977 RID: 6519 RVA: 0x00057A7C File Offset: 0x00055C7C
		internal static string ODataAtomDeserializer_RelativeUriUsedWithoutBaseUriSpecified(object p0)
		{
			return TextRes.GetString("ODataAtomDeserializer_RelativeUriUsedWithoutBaseUriSpecified", new object[]
			{
				p0
			});
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06001978 RID: 6520 RVA: 0x00057A9F File Offset: 0x00055C9F
		internal static string ODataAtomCollectionDeserializer_TypeOrNullAttributeNotAllowed
		{
			get
			{
				return TextRes.GetString("ODataAtomCollectionDeserializer_TypeOrNullAttributeNotAllowed");
			}
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x00057AAC File Offset: 0x00055CAC
		internal static string ODataAtomCollectionDeserializer_WrongCollectionItemElementName(object p0, object p1)
		{
			return TextRes.GetString("ODataAtomCollectionDeserializer_WrongCollectionItemElementName", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x00057AD4 File Offset: 0x00055CD4
		internal static string ODataAtomCollectionDeserializer_TopLevelCollectionElementWrongNamespace(object p0, object p1)
		{
			return TextRes.GetString("ODataAtomCollectionDeserializer_TopLevelCollectionElementWrongNamespace", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600197B RID: 6523 RVA: 0x00057AFC File Offset: 0x00055CFC
		internal static string ODataAtomPropertyAndValueDeserializer_TopLevelPropertyElementWrongNamespace(object p0, object p1)
		{
			return TextRes.GetString("ODataAtomPropertyAndValueDeserializer_TopLevelPropertyElementWrongNamespace", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x00057B24 File Offset: 0x00055D24
		internal static string ODataAtomPropertyAndValueDeserializer_NonEmptyElementWithNullAttribute(object p0)
		{
			return TextRes.GetString("ODataAtomPropertyAndValueDeserializer_NonEmptyElementWithNullAttribute", new object[]
			{
				p0
			});
		}

		// Token: 0x0600197D RID: 6525 RVA: 0x00057B48 File Offset: 0x00055D48
		internal static string ODataAtomPropertyAndValueDeserializer_InvalidCollectionElement(object p0, object p1)
		{
			return TextRes.GetString("ODataAtomPropertyAndValueDeserializer_InvalidCollectionElement", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600197E RID: 6526 RVA: 0x00057B70 File Offset: 0x00055D70
		internal static string ODataAtomPropertyAndValueDeserializer_NavigationPropertyInProperties(object p0, object p1)
		{
			return TextRes.GetString("ODataAtomPropertyAndValueDeserializer_NavigationPropertyInProperties", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x00057B98 File Offset: 0x00055D98
		internal static string ODataAtomPropertyAndValueSerializer_NullValueNotAllowedForInstanceAnnotation(object p0, object p1)
		{
			return TextRes.GetString("ODataAtomPropertyAndValueSerializer_NullValueNotAllowedForInstanceAnnotation", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06001980 RID: 6528 RVA: 0x00057BBF File Offset: 0x00055DBF
		internal static string EdmLibraryExtensions_CollectionItemCanBeOnlyPrimitiveOrComplex
		{
			get
			{
				return TextRes.GetString("EdmLibraryExtensions_CollectionItemCanBeOnlyPrimitiveOrComplex");
			}
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x00057BCC File Offset: 0x00055DCC
		internal static string ODataAtomEntryAndFeedDeserializer_ElementExpected(object p0)
		{
			return TextRes.GetString("ODataAtomEntryAndFeedDeserializer_ElementExpected", new object[]
			{
				p0
			});
		}

		// Token: 0x06001982 RID: 6530 RVA: 0x00057BF0 File Offset: 0x00055DF0
		internal static string ODataAtomEntryAndFeedDeserializer_EntryElementWrongName(object p0, object p1)
		{
			return TextRes.GetString("ODataAtomEntryAndFeedDeserializer_EntryElementWrongName", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06001983 RID: 6531 RVA: 0x00057C17 File Offset: 0x00055E17
		internal static string ODataAtomEntryAndFeedDeserializer_ContentWithSourceLinkIsNotEmpty
		{
			get
			{
				return TextRes.GetString("ODataAtomEntryAndFeedDeserializer_ContentWithSourceLinkIsNotEmpty");
			}
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x00057C24 File Offset: 0x00055E24
		internal static string ODataAtomEntryAndFeedDeserializer_ContentWithWrongType(object p0)
		{
			return TextRes.GetString("ODataAtomEntryAndFeedDeserializer_ContentWithWrongType", new object[]
			{
				p0
			});
		}

		// Token: 0x06001985 RID: 6533 RVA: 0x00057C48 File Offset: 0x00055E48
		internal static string ODataAtomEntryAndFeedDeserializer_ContentWithInvalidNode(object p0)
		{
			return TextRes.GetString("ODataAtomEntryAndFeedDeserializer_ContentWithInvalidNode", new object[]
			{
				p0
			});
		}

		// Token: 0x06001986 RID: 6534 RVA: 0x00057C6C File Offset: 0x00055E6C
		internal static string ODataAtomEntryAndFeedDeserializer_FeedElementWrongName(object p0, object p1)
		{
			return TextRes.GetString("ODataAtomEntryAndFeedDeserializer_FeedElementWrongName", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001987 RID: 6535 RVA: 0x00057C94 File Offset: 0x00055E94
		internal static string ODataAtomEntryAndFeedDeserializer_UnknownElementInInline(object p0)
		{
			return TextRes.GetString("ODataAtomEntryAndFeedDeserializer_UnknownElementInInline", new object[]
			{
				p0
			});
		}

		// Token: 0x06001988 RID: 6536 RVA: 0x00057CB8 File Offset: 0x00055EB8
		internal static string ODataAtomEntryAndFeedDeserializer_MultipleExpansionsInInline(object p0)
		{
			return TextRes.GetString("ODataAtomEntryAndFeedDeserializer_MultipleExpansionsInInline", new object[]
			{
				p0
			});
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06001989 RID: 6537 RVA: 0x00057CDB File Offset: 0x00055EDB
		internal static string ODataAtomEntryAndFeedDeserializer_MultipleInlineElementsInLink
		{
			get
			{
				return TextRes.GetString("ODataAtomEntryAndFeedDeserializer_MultipleInlineElementsInLink");
			}
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x00057CE8 File Offset: 0x00055EE8
		internal static string ODataAtomEntryAndFeedDeserializer_StreamPropertyWithMultipleEditLinks(object p0)
		{
			return TextRes.GetString("ODataAtomEntryAndFeedDeserializer_StreamPropertyWithMultipleEditLinks", new object[]
			{
				p0
			});
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x00057D0C File Offset: 0x00055F0C
		internal static string ODataAtomEntryAndFeedDeserializer_StreamPropertyWithMultipleReadLinks(object p0)
		{
			return TextRes.GetString("ODataAtomEntryAndFeedDeserializer_StreamPropertyWithMultipleReadLinks", new object[]
			{
				p0
			});
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x00057D30 File Offset: 0x00055F30
		internal static string ODataAtomEntryAndFeedDeserializer_StreamPropertyWithMultipleContentTypes(object p0)
		{
			return TextRes.GetString("ODataAtomEntryAndFeedDeserializer_StreamPropertyWithMultipleContentTypes", new object[]
			{
				p0
			});
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x00057D54 File Offset: 0x00055F54
		internal static string ODataAtomEntryAndFeedDeserializer_StreamPropertyDuplicatePropertyName(object p0)
		{
			return TextRes.GetString("ODataAtomEntryAndFeedDeserializer_StreamPropertyDuplicatePropertyName", new object[]
			{
				p0
			});
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x0600198E RID: 6542 RVA: 0x00057D77 File Offset: 0x00055F77
		internal static string ODataAtomEntryAndFeedDeserializer_StreamPropertyWithEmptyName
		{
			get
			{
				return TextRes.GetString("ODataAtomEntryAndFeedDeserializer_StreamPropertyWithEmptyName");
			}
		}

		// Token: 0x0600198F RID: 6543 RVA: 0x00057D84 File Offset: 0x00055F84
		internal static string ODataAtomEntryAndFeedDeserializer_OperationMissingMetadataAttribute(object p0)
		{
			return TextRes.GetString("ODataAtomEntryAndFeedDeserializer_OperationMissingMetadataAttribute", new object[]
			{
				p0
			});
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x00057DA8 File Offset: 0x00055FA8
		internal static string ODataAtomEntryAndFeedDeserializer_OperationMissingTargetAttribute(object p0)
		{
			return TextRes.GetString("ODataAtomEntryAndFeedDeserializer_OperationMissingTargetAttribute", new object[]
			{
				p0
			});
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x00057DCC File Offset: 0x00055FCC
		internal static string ODataAtomEntryAndFeedDeserializer_MultipleLinksInEntry(object p0)
		{
			return TextRes.GetString("ODataAtomEntryAndFeedDeserializer_MultipleLinksInEntry", new object[]
			{
				p0
			});
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x00057DF0 File Offset: 0x00055FF0
		internal static string ODataAtomEntryAndFeedDeserializer_MultipleLinksInFeed(object p0)
		{
			return TextRes.GetString("ODataAtomEntryAndFeedDeserializer_MultipleLinksInFeed", new object[]
			{
				p0
			});
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x00057E14 File Offset: 0x00056014
		internal static string ODataAtomEntryAndFeedDeserializer_DuplicateElements(object p0, object p1)
		{
			return TextRes.GetString("ODataAtomEntryAndFeedDeserializer_DuplicateElements", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x00057E3C File Offset: 0x0005603C
		internal static string ODataAtomEntryAndFeedDeserializer_InvalidTypeAttributeOnAssociationLink(object p0)
		{
			return TextRes.GetString("ODataAtomEntryAndFeedDeserializer_InvalidTypeAttributeOnAssociationLink", new object[]
			{
				p0
			});
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06001995 RID: 6549 RVA: 0x00057E5F File Offset: 0x0005605F
		internal static string ODataAtomEntryAndFeedDeserializer_EncounteredAnnotationInNestedFeed
		{
			get
			{
				return TextRes.GetString("ODataAtomEntryAndFeedDeserializer_EncounteredAnnotationInNestedFeed");
			}
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06001996 RID: 6550 RVA: 0x00057E6B File Offset: 0x0005606B
		internal static string ODataAtomEntryAndFeedDeserializer_EncounteredDeltaLinkInNestedFeed
		{
			get
			{
				return TextRes.GetString("ODataAtomEntryAndFeedDeserializer_EncounteredDeltaLinkInNestedFeed");
			}
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x00057E78 File Offset: 0x00056078
		internal static string ODataAtomEntryAndFeedDeserializer_AnnotationWithNonDotTarget(object p0, object p1)
		{
			return TextRes.GetString("ODataAtomEntryAndFeedDeserializer_AnnotationWithNonDotTarget", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x00057EA0 File Offset: 0x000560A0
		internal static string ODataAtomServiceDocumentDeserializer_ServiceDocumentRootElementWrongNameOrNamespace(object p0, object p1)
		{
			return TextRes.GetString("ODataAtomServiceDocumentDeserializer_ServiceDocumentRootElementWrongNameOrNamespace", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06001999 RID: 6553 RVA: 0x00057EC7 File Offset: 0x000560C7
		internal static string ODataAtomServiceDocumentDeserializer_MissingWorkspaceElement
		{
			get
			{
				return TextRes.GetString("ODataAtomServiceDocumentDeserializer_MissingWorkspaceElement");
			}
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x0600199A RID: 6554 RVA: 0x00057ED3 File Offset: 0x000560D3
		internal static string ODataAtomServiceDocumentDeserializer_MultipleWorkspaceElementsFound
		{
			get
			{
				return TextRes.GetString("ODataAtomServiceDocumentDeserializer_MultipleWorkspaceElementsFound");
			}
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x00057EE0 File Offset: 0x000560E0
		internal static string ODataAtomServiceDocumentDeserializer_UnexpectedElementInServiceDocument(object p0)
		{
			return TextRes.GetString("ODataAtomServiceDocumentDeserializer_UnexpectedElementInServiceDocument", new object[]
			{
				p0
			});
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x00057F04 File Offset: 0x00056104
		internal static string ODataAtomServiceDocumentDeserializer_UnexpectedElementInWorkspace(object p0)
		{
			return TextRes.GetString("ODataAtomServiceDocumentDeserializer_UnexpectedElementInWorkspace", new object[]
			{
				p0
			});
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x00057F28 File Offset: 0x00056128
		internal static string ODataAtomServiceDocumentDeserializer_UnexpectedElementInResourceCollection(object p0)
		{
			return TextRes.GetString("ODataAtomServiceDocumentDeserializer_UnexpectedElementInResourceCollection", new object[]
			{
				p0
			});
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x00057F4C File Offset: 0x0005614C
		internal static string ODataAtomEntryMetadataDeserializer_InvalidTextConstructKind(object p0, object p1)
		{
			return TextRes.GetString("ODataAtomEntryMetadataDeserializer_InvalidTextConstructKind", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x00057F74 File Offset: 0x00056174
		internal static string ODataAtomMetadataDeserializer_MultipleSingletonMetadataElements(object p0, object p1)
		{
			return TextRes.GetString("ODataAtomMetadataDeserializer_MultipleSingletonMetadataElements", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x00057F9C File Offset: 0x0005619C
		internal static string ODataAtomErrorDeserializer_InvalidRootElement(object p0, object p1)
		{
			return TextRes.GetString("ODataAtomErrorDeserializer_InvalidRootElement", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x00057FC4 File Offset: 0x000561C4
		internal static string ODataAtomErrorDeserializer_MultipleErrorElementsWithSameName(object p0)
		{
			return TextRes.GetString("ODataAtomErrorDeserializer_MultipleErrorElementsWithSameName", new object[]
			{
				p0
			});
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x00057FE8 File Offset: 0x000561E8
		internal static string ODataAtomErrorDeserializer_MultipleInnerErrorElementsWithSameName(object p0)
		{
			return TextRes.GetString("ODataAtomErrorDeserializer_MultipleInnerErrorElementsWithSameName", new object[]
			{
				p0
			});
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x0005800C File Offset: 0x0005620C
		internal static string ODataAtomEntityReferenceLinkDeserializer_InvalidEntityReferenceLinkStartElement(object p0, object p1)
		{
			return TextRes.GetString("ODataAtomEntityReferenceLinkDeserializer_InvalidEntityReferenceLinkStartElement", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x00058034 File Offset: 0x00056234
		internal static string ODataAtomEntityReferenceLinkDeserializer_InvalidEntityReferenceLinksStartElement(object p0, object p1)
		{
			return TextRes.GetString("ODataAtomEntityReferenceLinkDeserializer_InvalidEntityReferenceLinksStartElement", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x0005805C File Offset: 0x0005625C
		internal static string ODataAtomEntityReferenceLinkDeserializer_MultipleEntityReferenceLinksElementsWithSameName(object p0, object p1)
		{
			return TextRes.GetString("ODataAtomEntityReferenceLinkDeserializer_MultipleEntityReferenceLinksElementsWithSameName", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x00058084 File Offset: 0x00056284
		internal static string EpmReader_OpenComplexOrCollectionEpmProperty(object p0)
		{
			return TextRes.GetString("EpmReader_OpenComplexOrCollectionEpmProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x060019A7 RID: 6567 RVA: 0x000580A8 File Offset: 0x000562A8
		internal static string EpmSyndicationReader_MultipleValuesForNonCollectionProperty(object p0, object p1, object p2)
		{
			return TextRes.GetString("EpmSyndicationReader_MultipleValuesForNonCollectionProperty", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x060019A8 RID: 6568 RVA: 0x000580D4 File Offset: 0x000562D4
		internal static string ODataAtomServiceDocumentMetadataDeserializer_InvalidFixedAttributeValue(object p0)
		{
			return TextRes.GetString("ODataAtomServiceDocumentMetadataDeserializer_InvalidFixedAttributeValue", new object[]
			{
				p0
			});
		}

		// Token: 0x060019A9 RID: 6569 RVA: 0x000580F8 File Offset: 0x000562F8
		internal static string ODataAtomServiceDocumentMetadataDeserializer_MultipleTitleElementsFound(object p0)
		{
			return TextRes.GetString("ODataAtomServiceDocumentMetadataDeserializer_MultipleTitleElementsFound", new object[]
			{
				p0
			});
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x060019AA RID: 6570 RVA: 0x0005811B File Offset: 0x0005631B
		internal static string ODataAtomServiceDocumentMetadataDeserializer_MultipleAcceptElementsFoundInCollection
		{
			get
			{
				return TextRes.GetString("ODataAtomServiceDocumentMetadataDeserializer_MultipleAcceptElementsFoundInCollection");
			}
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x00058128 File Offset: 0x00056328
		internal static string ODataAtomServiceDocumentMetadataSerializer_ResourceCollectionNameAndTitleMismatch(object p0, object p1)
		{
			return TextRes.GetString("ODataAtomServiceDocumentMetadataSerializer_ResourceCollectionNameAndTitleMismatch", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x00058150 File Offset: 0x00056350
		internal static string CollectionWithoutExpectedTypeValidator_InvalidItemTypeKind(object p0)
		{
			return TextRes.GetString("CollectionWithoutExpectedTypeValidator_InvalidItemTypeKind", new object[]
			{
				p0
			});
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x00058174 File Offset: 0x00056374
		internal static string CollectionWithoutExpectedTypeValidator_IncompatibleItemTypeKind(object p0, object p1)
		{
			return TextRes.GetString("CollectionWithoutExpectedTypeValidator_IncompatibleItemTypeKind", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060019AE RID: 6574 RVA: 0x0005819C File Offset: 0x0005639C
		internal static string CollectionWithoutExpectedTypeValidator_IncompatibleItemTypeName(object p0, object p1)
		{
			return TextRes.GetString("CollectionWithoutExpectedTypeValidator_IncompatibleItemTypeName", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060019AF RID: 6575 RVA: 0x000581C4 File Offset: 0x000563C4
		internal static string FeedWithoutExpectedTypeValidator_IncompatibleTypes(object p0, object p1)
		{
			return TextRes.GetString("FeedWithoutExpectedTypeValidator_IncompatibleTypes", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060019B0 RID: 6576 RVA: 0x000581EC File Offset: 0x000563EC
		internal static string MessageStreamWrappingStream_ByteLimitExceeded(object p0, object p1)
		{
			return TextRes.GetString("MessageStreamWrappingStream_ByteLimitExceeded", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060019B1 RID: 6577 RVA: 0x00058214 File Offset: 0x00056414
		internal static string MetadataUtils_ResolveTypeName(object p0)
		{
			return TextRes.GetString("MetadataUtils_ResolveTypeName", new object[]
			{
				p0
			});
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x00058238 File Offset: 0x00056438
		internal static string EdmValueUtils_UnsupportedPrimitiveType(object p0)
		{
			return TextRes.GetString("EdmValueUtils_UnsupportedPrimitiveType", new object[]
			{
				p0
			});
		}

		// Token: 0x060019B3 RID: 6579 RVA: 0x0005825C File Offset: 0x0005645C
		internal static string EdmValueUtils_IncorrectPrimitiveTypeKind(object p0, object p1, object p2)
		{
			return TextRes.GetString("EdmValueUtils_IncorrectPrimitiveTypeKind", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x060019B4 RID: 6580 RVA: 0x00058288 File Offset: 0x00056488
		internal static string EdmValueUtils_IncorrectPrimitiveTypeKindNoTypeName(object p0, object p1)
		{
			return TextRes.GetString("EdmValueUtils_IncorrectPrimitiveTypeKindNoTypeName", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x000582B0 File Offset: 0x000564B0
		internal static string EdmValueUtils_CannotConvertTypeToClrValue(object p0)
		{
			return TextRes.GetString("EdmValueUtils_CannotConvertTypeToClrValue", new object[]
			{
				p0
			});
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x000582D4 File Offset: 0x000564D4
		internal static string ODataEdmStructuredValue_UndeclaredProperty(object p0, object p1)
		{
			return TextRes.GetString("ODataEdmStructuredValue_UndeclaredProperty", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x000582FC File Offset: 0x000564FC
		internal static string ODataModelAnnotationEvaluator_AmbiguousAnnotationTerm(object p0, object p1)
		{
			return TextRes.GetString("ODataModelAnnotationEvaluator_AmbiguousAnnotationTerm", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060019B8 RID: 6584 RVA: 0x00058324 File Offset: 0x00056524
		internal static string ODataModelAnnotationEvaluator_AmbiguousAnnotationTermWithQualifier(object p0, object p1, object p2)
		{
			return TextRes.GetString("ODataModelAnnotationEvaluator_AmbiguousAnnotationTermWithQualifier", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x060019B9 RID: 6585 RVA: 0x00058350 File Offset: 0x00056550
		internal static string ODataModelAnnotationEvaluator_AnnotationTermWithInvalidQualifier(object p0, object p1, object p2)
		{
			return TextRes.GetString("ODataModelAnnotationEvaluator_AnnotationTermWithInvalidQualifier", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x0005837C File Offset: 0x0005657C
		internal static string ODataModelAnnotationEvaluator_AnnotationTermWithUnsupportedQualifier(object p0, object p1, object p2, object p3)
		{
			return TextRes.GetString("ODataModelAnnotationEvaluator_AnnotationTermWithUnsupportedQualifier", new object[]
			{
				p0,
				p1,
				p2,
				p3
			});
		}

		// Token: 0x060019BB RID: 6587 RVA: 0x000583AC File Offset: 0x000565AC
		internal static string ODataMetadataBuilder_MissingEntitySetUri(object p0)
		{
			return TextRes.GetString("ODataMetadataBuilder_MissingEntitySetUri", new object[]
			{
				p0
			});
		}

		// Token: 0x060019BC RID: 6588 RVA: 0x000583D0 File Offset: 0x000565D0
		internal static string ODataMetadataBuilder_MissingSegmentForEntitySetUriSuffix(object p0, object p1)
		{
			return TextRes.GetString("ODataMetadataBuilder_MissingSegmentForEntitySetUriSuffix", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x000583F8 File Offset: 0x000565F8
		internal static string ODataMetadataBuilder_MissingEntityInstanceUri(object p0)
		{
			return TextRes.GetString("ODataMetadataBuilder_MissingEntityInstanceUri", new object[]
			{
				p0
			});
		}

		// Token: 0x060019BE RID: 6590 RVA: 0x0005841C File Offset: 0x0005661C
		internal static string ODataJsonLightInputContext_EntityTypeMustBeCompatibleWithEntitySetBaseType(object p0, object p1, object p2)
		{
			return TextRes.GetString("ODataJsonLightInputContext_EntityTypeMustBeCompatibleWithEntitySetBaseType", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x060019BF RID: 6591 RVA: 0x00058447 File Offset: 0x00056647
		internal static string ODataJsonLightInputContext_MetadataDocumentUriMissing
		{
			get
			{
				return TextRes.GetString("ODataJsonLightInputContext_MetadataDocumentUriMissing");
			}
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x060019C0 RID: 6592 RVA: 0x00058453 File Offset: 0x00056653
		internal static string ODataJsonLightInputContext_PayloadKindDetectionForRequest
		{
			get
			{
				return TextRes.GetString("ODataJsonLightInputContext_PayloadKindDetectionForRequest");
			}
		}

		// Token: 0x060019C1 RID: 6593 RVA: 0x00058460 File Offset: 0x00056660
		internal static string ODataJsonLightInputContext_FunctionImportCannotBeNullForCreateParameterReader(object p0)
		{
			return TextRes.GetString("ODataJsonLightInputContext_FunctionImportCannotBeNullForCreateParameterReader", new object[]
			{
				p0
			});
		}

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x060019C2 RID: 6594 RVA: 0x00058483 File Offset: 0x00056683
		internal static string ODataJsonLightInputContext_NoEntitySetForRequest
		{
			get
			{
				return TextRes.GetString("ODataJsonLightInputContext_NoEntitySetForRequest");
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x060019C3 RID: 6595 RVA: 0x0005848F File Offset: 0x0005668F
		internal static string ODataJsonLightInputContext_FunctionImportOrItemTypeRequiredForCollectionReaderInRequests
		{
			get
			{
				return TextRes.GetString("ODataJsonLightInputContext_FunctionImportOrItemTypeRequiredForCollectionReaderInRequests");
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x060019C4 RID: 6596 RVA: 0x0005849B File Offset: 0x0005669B
		internal static string ODataJsonLightInputContext_NavigationPropertyRequiredForReadEntityReferenceLinkInRequests
		{
			get
			{
				return TextRes.GetString("ODataJsonLightInputContext_NavigationPropertyRequiredForReadEntityReferenceLinkInRequests");
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x060019C5 RID: 6597 RVA: 0x000584A7 File Offset: 0x000566A7
		internal static string ODataJsonLightInputContext_ModelRequiredForReading
		{
			get
			{
				return TextRes.GetString("ODataJsonLightInputContext_ModelRequiredForReading");
			}
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x060019C6 RID: 6598 RVA: 0x000584B3 File Offset: 0x000566B3
		internal static string ODataJsonLightInputContext_BaseUriMustBeNonNullAndAbsolute
		{
			get
			{
				return TextRes.GetString("ODataJsonLightInputContext_BaseUriMustBeNonNullAndAbsolute");
			}
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x060019C7 RID: 6599 RVA: 0x000584BF File Offset: 0x000566BF
		internal static string ODataJsonLightInputContext_ItemTypeRequiredForCollectionReaderInRequests
		{
			get
			{
				return TextRes.GetString("ODataJsonLightInputContext_ItemTypeRequiredForCollectionReaderInRequests");
			}
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x060019C8 RID: 6600 RVA: 0x000584CB File Offset: 0x000566CB
		internal static string ODataJsonLightInputContext_NoItemTypeSpecified
		{
			get
			{
				return TextRes.GetString("ODataJsonLightInputContext_NoItemTypeSpecified");
			}
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x060019C9 RID: 6601 RVA: 0x000584D7 File Offset: 0x000566D7
		internal static string ODataJsonLightDeserializer_MetadataLinkNotFoundAsFirstProperty
		{
			get
			{
				return TextRes.GetString("ODataJsonLightDeserializer_MetadataLinkNotFoundAsFirstProperty");
			}
		}

		// Token: 0x060019CA RID: 6602 RVA: 0x000584E4 File Offset: 0x000566E4
		internal static string ODataJsonLightDeserializer_RequiredPropertyNotFound(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightDeserializer_RequiredPropertyNotFound", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060019CB RID: 6603 RVA: 0x0005850C File Offset: 0x0005670C
		internal static string ODataJsonLightDeserializer_OnlyODataTypeAnnotationCanTargetInstanceAnnotation(object p0, object p1, object p2)
		{
			return TextRes.GetString("ODataJsonLightDeserializer_OnlyODataTypeAnnotationCanTargetInstanceAnnotation", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x060019CC RID: 6604 RVA: 0x00058538 File Offset: 0x00056738
		internal static string ODataJsonLightDeserializer_AnnotationTargetingInstanceAnnotationWithoutValue(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightDeserializer_AnnotationTargetingInstanceAnnotationWithoutValue", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x060019CD RID: 6605 RVA: 0x0005855F File Offset: 0x0005675F
		internal static string ODataJsonLightWriter_EntityReferenceLinkAfterFeedInRequest
		{
			get
			{
				return TextRes.GetString("ODataJsonLightWriter_EntityReferenceLinkAfterFeedInRequest");
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x060019CE RID: 6606 RVA: 0x0005856B File Offset: 0x0005676B
		internal static string ODataJsonLightWriter_InstanceAnnotationNotSupportedOnExpandedFeed
		{
			get
			{
				return TextRes.GetString("ODataJsonLightWriter_InstanceAnnotationNotSupportedOnExpandedFeed");
			}
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x060019CF RID: 6607 RVA: 0x00058577 File Offset: 0x00056777
		internal static string ODataJsonLightOutputContext_MetadataDocumentUriMissing
		{
			get
			{
				return TextRes.GetString("ODataJsonLightOutputContext_MetadataDocumentUriMissing");
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x060019D0 RID: 6608 RVA: 0x00058583 File Offset: 0x00056783
		internal static string ODataJsonLightPropertyAndValueSerializer_NoExpectedTypeOrTypeNameSpecifiedForComplexValueRequest
		{
			get
			{
				return TextRes.GetString("ODataJsonLightPropertyAndValueSerializer_NoExpectedTypeOrTypeNameSpecifiedForComplexValueRequest");
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x060019D1 RID: 6609 RVA: 0x0005858F File Offset: 0x0005678F
		internal static string ODataJsonLightPropertyAndValueSerializer_NoExpectedTypeOrTypeNameSpecifiedForCollectionValueInRequest
		{
			get
			{
				return TextRes.GetString("ODataJsonLightPropertyAndValueSerializer_NoExpectedTypeOrTypeNameSpecifiedForCollectionValueInRequest");
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x060019D2 RID: 6610 RVA: 0x0005859B File Offset: 0x0005679B
		internal static string ODataJsonLightServiceDocumentSerializer_ResourceCollectionMustSpecifyName
		{
			get
			{
				return TextRes.GetString("ODataJsonLightServiceDocumentSerializer_ResourceCollectionMustSpecifyName");
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x060019D3 RID: 6611 RVA: 0x000585A7 File Offset: 0x000567A7
		internal static string ODataFeedAndEntryTypeContext_MetadataOrSerializationInfoMissing
		{
			get
			{
				return TextRes.GetString("ODataFeedAndEntryTypeContext_MetadataOrSerializationInfoMissing");
			}
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x060019D4 RID: 6612 RVA: 0x000585B3 File Offset: 0x000567B3
		internal static string ODataFeedAndEntryTypeContext_ODataEntryTypeNameMissing
		{
			get
			{
				return TextRes.GetString("ODataFeedAndEntryTypeContext_ODataEntryTypeNameMissing");
			}
		}

		// Token: 0x060019D5 RID: 6613 RVA: 0x000585C0 File Offset: 0x000567C0
		internal static string ODataJsonLightMetadataUriBuilder_ValidateDerivedType(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightMetadataUriBuilder_ValidateDerivedType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x060019D6 RID: 6614 RVA: 0x000585E7 File Offset: 0x000567E7
		internal static string ODataJsonLightMetadataUriBuilder_TypeNameMissingForTopLevelCollectionWhenWritingResponsePayload
		{
			get
			{
				return TextRes.GetString("ODataJsonLightMetadataUriBuilder_TypeNameMissingForTopLevelCollectionWhenWritingResponsePayload");
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x060019D7 RID: 6615 RVA: 0x000585F3 File Offset: 0x000567F3
		internal static string ODataJsonLightMetadataUriBuilder_EntitySetOrNavigationPropertyMissingForTopLevelEntityReferenceLinkResponse
		{
			get
			{
				return TextRes.GetString("ODataJsonLightMetadataUriBuilder_EntitySetOrNavigationPropertyMissingForTopLevelEntityReferenceLinkResponse");
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x060019D8 RID: 6616 RVA: 0x000585FF File Offset: 0x000567FF
		internal static string ODataJsonLightMetadataUriBuilder_EntitySetOrNavigationPropertyMissingForTopLevelEntityReferenceLinksResponse
		{
			get
			{
				return TextRes.GetString("ODataJsonLightMetadataUriBuilder_EntitySetOrNavigationPropertyMissingForTopLevelEntityReferenceLinksResponse");
			}
		}

		// Token: 0x060019D9 RID: 6617 RVA: 0x0005860C File Offset: 0x0005680C
		internal static string ODataJsonLightPropertyAndValueDeserializer_UnexpectedAnnotationProperties(object p0)
		{
			return TextRes.GetString("ODataJsonLightPropertyAndValueDeserializer_UnexpectedAnnotationProperties", new object[]
			{
				p0
			});
		}

		// Token: 0x060019DA RID: 6618 RVA: 0x00058630 File Offset: 0x00056830
		internal static string ODataJsonLightPropertyAndValueDeserializer_UnexpectedPropertyAnnotation(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightPropertyAndValueDeserializer_UnexpectedPropertyAnnotation", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x00058658 File Offset: 0x00056858
		internal static string ODataJsonLightPropertyAndValueDeserializer_UnexpectedODataPropertyAnnotation(object p0)
		{
			return TextRes.GetString("ODataJsonLightPropertyAndValueDeserializer_UnexpectedODataPropertyAnnotation", new object[]
			{
				p0
			});
		}

		// Token: 0x060019DC RID: 6620 RVA: 0x0005867C File Offset: 0x0005687C
		internal static string ODataJsonLightPropertyAndValueDeserializer_UnexpectedProperty(object p0)
		{
			return TextRes.GetString("ODataJsonLightPropertyAndValueDeserializer_UnexpectedProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x060019DD RID: 6621 RVA: 0x0005869F File Offset: 0x0005689F
		internal static string ODataJsonLightPropertyAndValueDeserializer_InvalidTopLevelPropertyPayload
		{
			get
			{
				return TextRes.GetString("ODataJsonLightPropertyAndValueDeserializer_InvalidTopLevelPropertyPayload");
			}
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x000586AC File Offset: 0x000568AC
		internal static string ODataJsonLightPropertyAndValueDeserializer_InvalidTopLevelPropertyName(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightPropertyAndValueDeserializer_InvalidTopLevelPropertyName", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x000586D4 File Offset: 0x000568D4
		internal static string ODataJsonLightPropertyAndValueDeserializer_InvalidTypeName(object p0)
		{
			return TextRes.GetString("ODataJsonLightPropertyAndValueDeserializer_InvalidTypeName", new object[]
			{
				p0
			});
		}

		// Token: 0x060019E0 RID: 6624 RVA: 0x000586F8 File Offset: 0x000568F8
		internal static string ODataJsonLightPropertyAndValueDeserializer_InvalidPrimitiveTypeName(object p0)
		{
			return TextRes.GetString("ODataJsonLightPropertyAndValueDeserializer_InvalidPrimitiveTypeName", new object[]
			{
				p0
			});
		}

		// Token: 0x060019E1 RID: 6625 RVA: 0x0005871C File Offset: 0x0005691C
		internal static string ODataJsonLightPropertyAndValueDeserializer_TopLevelPropertyAnnotationWithoutProperty(object p0)
		{
			return TextRes.GetString("ODataJsonLightPropertyAndValueDeserializer_TopLevelPropertyAnnotationWithoutProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x00058740 File Offset: 0x00056940
		internal static string ODataJsonLightPropertyAndValueDeserializer_ComplexValuePropertyAnnotationWithoutProperty(object p0)
		{
			return TextRes.GetString("ODataJsonLightPropertyAndValueDeserializer_ComplexValuePropertyAnnotationWithoutProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x00058764 File Offset: 0x00056964
		internal static string ODataJsonLightPropertyAndValueDeserializer_ComplexValueWithPropertyTypeAnnotation(object p0)
		{
			return TextRes.GetString("ODataJsonLightPropertyAndValueDeserializer_ComplexValueWithPropertyTypeAnnotation", new object[]
			{
				p0
			});
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x060019E4 RID: 6628 RVA: 0x00058787 File Offset: 0x00056987
		internal static string ODataJsonLightPropertyAndValueDeserializer_ComplexTypeAnnotationNotFirst
		{
			get
			{
				return TextRes.GetString("ODataJsonLightPropertyAndValueDeserializer_ComplexTypeAnnotationNotFirst");
			}
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x00058794 File Offset: 0x00056994
		internal static string ODataJsonLightPropertyAndValueDeserializer_UnexpectedDataPropertyAnnotation(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightPropertyAndValueDeserializer_UnexpectedDataPropertyAnnotation", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x000587BC File Offset: 0x000569BC
		internal static string ODataJsonLightPropertyAndValueDeserializer_TypePropertyAfterValueProperty(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightPropertyAndValueDeserializer_TypePropertyAfterValueProperty", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060019E7 RID: 6631 RVA: 0x000587E4 File Offset: 0x000569E4
		internal static string ODataJsonLightPropertyAndValueDeserializer_ODataTypeAnnotationInPrimitiveValue(object p0)
		{
			return TextRes.GetString("ODataJsonLightPropertyAndValueDeserializer_ODataTypeAnnotationInPrimitiveValue", new object[]
			{
				p0
			});
		}

		// Token: 0x060019E8 RID: 6632 RVA: 0x00058808 File Offset: 0x00056A08
		internal static string ODataJsonLightPropertyAndValueDeserializer_TopLevelPropertyWithPrimitiveNullValue(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightPropertyAndValueDeserializer_TopLevelPropertyWithPrimitiveNullValue", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x00058830 File Offset: 0x00056A30
		internal static string ODataJsonLightPropertyAndValueDeserializer_UnexpectedMetadataReferenceProperty(object p0)
		{
			return TextRes.GetString("ODataJsonLightPropertyAndValueDeserializer_UnexpectedMetadataReferenceProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x060019EA RID: 6634 RVA: 0x00058854 File Offset: 0x00056A54
		internal static string ODataJsonLightPropertyAndValueDeserializer_NoPropertyAndAnnotationAllowedInNullPayload(object p0)
		{
			return TextRes.GetString("ODataJsonLightPropertyAndValueDeserializer_NoPropertyAndAnnotationAllowedInNullPayload", new object[]
			{
				p0
			});
		}

		// Token: 0x060019EB RID: 6635 RVA: 0x00058878 File Offset: 0x00056A78
		internal static string ODataJsonLightPropertyAndValueDeserializer_EdmNullInMetadataUriWithoutNullValueInPayload(object p0)
		{
			return TextRes.GetString("ODataJsonLightPropertyAndValueDeserializer_EdmNullInMetadataUriWithoutNullValueInPayload", new object[]
			{
				p0
			});
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x060019EC RID: 6636 RVA: 0x0005889B File Offset: 0x00056A9B
		internal static string ODataJsonReaderCoreUtils_CannotReadSpatialPropertyValue
		{
			get
			{
				return TextRes.GetString("ODataJsonReaderCoreUtils_CannotReadSpatialPropertyValue");
			}
		}

		// Token: 0x060019ED RID: 6637 RVA: 0x000588A8 File Offset: 0x00056AA8
		internal static string ODataJsonLightReaderUtils_AnnotationWithNullValue(object p0)
		{
			return TextRes.GetString("ODataJsonLightReaderUtils_AnnotationWithNullValue", new object[]
			{
				p0
			});
		}

		// Token: 0x060019EE RID: 6638 RVA: 0x000588CC File Offset: 0x00056ACC
		internal static string ODataJsonLightReaderUtils_InvalidValueForODataNullAnnotation(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightReaderUtils_InvalidValueForODataNullAnnotation", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x000588F4 File Offset: 0x00056AF4
		internal static string JsonLightInstanceAnnotationWriter_DuplicateAnnotationNameInCollection(object p0)
		{
			return TextRes.GetString("JsonLightInstanceAnnotationWriter_DuplicateAnnotationNameInCollection", new object[]
			{
				p0
			});
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x00058918 File Offset: 0x00056B18
		internal static string ODataJsonLightMetadataUriParser_ServiceDocumentUriMustNotHaveFragment(object p0)
		{
			return TextRes.GetString("ODataJsonLightMetadataUriParser_ServiceDocumentUriMustNotHaveFragment", new object[]
			{
				p0
			});
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x060019F1 RID: 6641 RVA: 0x0005893B File Offset: 0x00056B3B
		internal static string ODataJsonLightMetadataUriParser_NullMetadataDocumentUri
		{
			get
			{
				return TextRes.GetString("ODataJsonLightMetadataUriParser_NullMetadataDocumentUri");
			}
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x00058948 File Offset: 0x00056B48
		internal static string ODataJsonLightMetadataUriParser_MetadataUriDoesNotMatchExpectedPayloadKind(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightMetadataUriParser_MetadataUriDoesNotMatchExpectedPayloadKind", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x00058970 File Offset: 0x00056B70
		internal static string ODataJsonLightMetadataUriParser_InvalidEntitySetNameOrTypeName(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightMetadataUriParser_InvalidEntitySetNameOrTypeName", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x00058998 File Offset: 0x00056B98
		internal static string ODataJsonLightMetadataUriParser_InvalidPropertyName(object p0, object p1, object p2)
		{
			return TextRes.GetString("ODataJsonLightMetadataUriParser_InvalidPropertyName", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x000589C4 File Offset: 0x00056BC4
		internal static string ODataJsonLightMetadataUriParser_InvalidEntityWithTypeCastUriSuffix(object p0, object p1, object p2)
		{
			return TextRes.GetString("ODataJsonLightMetadataUriParser_InvalidEntityWithTypeCastUriSuffix", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x000589F0 File Offset: 0x00056BF0
		internal static string ODataJsonLightMetadataUriParser_InvalidEntityTypeInTypeCast(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightMetadataUriParser_InvalidEntityTypeInTypeCast", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x00058A18 File Offset: 0x00056C18
		internal static string ODataJsonLightMetadataUriParser_IncompatibleEntityTypeInTypeCast(object p0, object p1, object p2, object p3)
		{
			return TextRes.GetString("ODataJsonLightMetadataUriParser_IncompatibleEntityTypeInTypeCast", new object[]
			{
				p0,
				p1,
				p2,
				p3
			});
		}

		// Token: 0x060019F8 RID: 6648 RVA: 0x00058A48 File Offset: 0x00056C48
		internal static string ODataJsonLightMetadataUriParser_InvalidEntityReferenceLinkUriSuffix(object p0, object p1, object p2)
		{
			return TextRes.GetString("ODataJsonLightMetadataUriParser_InvalidEntityReferenceLinkUriSuffix", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x060019F9 RID: 6649 RVA: 0x00058A74 File Offset: 0x00056C74
		internal static string ODataJsonLightMetadataUriParser_InvalidPropertyForEntityReferenceLinkUri(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightMetadataUriParser_InvalidPropertyForEntityReferenceLinkUri", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x00058A9C File Offset: 0x00056C9C
		internal static string ODataJsonLightMetadataUriParser_InvalidSingletonNavPropertyForEntityReferenceLinkUri(object p0, object p1, object p2)
		{
			return TextRes.GetString("ODataJsonLightMetadataUriParser_InvalidSingletonNavPropertyForEntityReferenceLinkUri", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x060019FB RID: 6651 RVA: 0x00058AC8 File Offset: 0x00056CC8
		internal static string ODataJsonLightMetadataUriParser_FragmentWithInvalidNumberOfParts(object p0, object p1, object p2)
		{
			return TextRes.GetString("ODataJsonLightMetadataUriParser_FragmentWithInvalidNumberOfParts", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x060019FC RID: 6652 RVA: 0x00058AF4 File Offset: 0x00056CF4
		internal static string ODataJsonLightMetadataUriParser_InvalidEntitySetOrFunctionImportName(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightMetadataUriParser_InvalidEntitySetOrFunctionImportName", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060019FD RID: 6653 RVA: 0x00058B1C File Offset: 0x00056D1C
		internal static string ODataJsonLightMetadataUriParser_InvalidPayloadKindWithSelectQueryOption(object p0)
		{
			return TextRes.GetString("ODataJsonLightMetadataUriParser_InvalidPayloadKindWithSelectQueryOption", new object[]
			{
				p0
			});
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x060019FE RID: 6654 RVA: 0x00058B3F File Offset: 0x00056D3F
		internal static string ODataJsonLightMetadataUriParser_NoModel
		{
			get
			{
				return TextRes.GetString("ODataJsonLightMetadataUriParser_NoModel");
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x060019FF RID: 6655 RVA: 0x00058B4B File Offset: 0x00056D4B
		internal static string ODataJsonLightMetadataUriParser_ModelResolverReturnedNull
		{
			get
			{
				return TextRes.GetString("ODataJsonLightMetadataUriParser_ModelResolverReturnedNull");
			}
		}

		// Token: 0x06001A00 RID: 6656 RVA: 0x00058B58 File Offset: 0x00056D58
		internal static string ODataJsonLightMetadataUriParser_InvalidAssociationLink(object p0)
		{
			return TextRes.GetString("ODataJsonLightMetadataUriParser_InvalidAssociationLink", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x00058B7C File Offset: 0x00056D7C
		internal static string ODataJsonLightMetadataUriParser_InvalidEntitySetName(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightMetadataUriParser_InvalidEntitySetName", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06001A02 RID: 6658 RVA: 0x00058BA3 File Offset: 0x00056DA3
		internal static string ODataJsonLightEntryAndFeedDeserializer_EntryTypeAnnotationNotFirst
		{
			get
			{
				return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_EntryTypeAnnotationNotFirst");
			}
		}

		// Token: 0x06001A03 RID: 6659 RVA: 0x00058BB0 File Offset: 0x00056DB0
		internal static string ODataJsonLightEntryAndFeedDeserializer_EntryInstanceAnnotationPrecededByProperty(object p0)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_EntryInstanceAnnotationPrecededByProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x00058BD4 File Offset: 0x00056DD4
		internal static string ODataJsonLightEntryAndFeedDeserializer_CannotReadFeedContentStart(object p0)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_CannotReadFeedContentStart", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x00058BF8 File Offset: 0x00056DF8
		internal static string ODataJsonLightEntryAndFeedDeserializer_ExpectedFeedPropertyNotFound(object p0)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_ExpectedFeedPropertyNotFound", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x00058C1C File Offset: 0x00056E1C
		internal static string ODataJsonLightEntryAndFeedDeserializer_InvalidNodeTypeForItemsInFeed(object p0)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_InvalidNodeTypeForItemsInFeed", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x00058C40 File Offset: 0x00056E40
		internal static string ODataJsonLightEntryAndFeedDeserializer_InvalidPropertyAnnotationInTopLevelFeed(object p0)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_InvalidPropertyAnnotationInTopLevelFeed", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A08 RID: 6664 RVA: 0x00058C64 File Offset: 0x00056E64
		internal static string ODataJsonLightEntryAndFeedDeserializer_InvalidPropertyInTopLevelFeed(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_InvalidPropertyInTopLevelFeed", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06001A09 RID: 6665 RVA: 0x00058C8B File Offset: 0x00056E8B
		internal static string ODataJsonLightEntryAndFeedDeserializer_FeedPropertyAnnotationForTopLevelFeed
		{
			get
			{
				return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_FeedPropertyAnnotationForTopLevelFeed");
			}
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x00058C98 File Offset: 0x00056E98
		internal static string ODataJsonLightEntryAndFeedDeserializer_PropertyWithoutValueWithWrongType(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_PropertyWithoutValueWithWrongType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x00058CC0 File Offset: 0x00056EC0
		internal static string ODataJsonLightEntryAndFeedDeserializer_OpenPropertyWithoutValue(object p0)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_OpenPropertyWithoutValue", new object[]
			{
				p0
			});
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06001A0C RID: 6668 RVA: 0x00058CE3 File Offset: 0x00056EE3
		internal static string ODataJsonLightEntryAndFeedDeserializer_StreamPropertyInRequest
		{
			get
			{
				return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_StreamPropertyInRequest");
			}
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x00058CF0 File Offset: 0x00056EF0
		internal static string ODataJsonLightEntryAndFeedDeserializer_UnexpectedStreamPropertyAnnotation(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_UnexpectedStreamPropertyAnnotation", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x00058D18 File Offset: 0x00056F18
		internal static string ODataJsonLightEntryAndFeedDeserializer_StreamPropertyWithValue(object p0)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_StreamPropertyWithValue", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x00058D3C File Offset: 0x00056F3C
		internal static string ODataJsonLightEntryAndFeedDeserializer_UnexpectedDeferredLinkPropertyAnnotation(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_UnexpectedDeferredLinkPropertyAnnotation", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x00058D64 File Offset: 0x00056F64
		internal static string ODataJsonLightEntryAndFeedDeserializer_CannotReadSingletonNavigationPropertyValue(object p0)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_CannotReadSingletonNavigationPropertyValue", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x00058D88 File Offset: 0x00056F88
		internal static string ODataJsonLightEntryAndFeedDeserializer_CannotReadCollectionNavigationPropertyValue(object p0)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_CannotReadCollectionNavigationPropertyValue", new object[]
			{
				p0
			});
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06001A12 RID: 6674 RVA: 0x00058DAB File Offset: 0x00056FAB
		internal static string ODataJsonLightEntryAndFeedDeserializer_CannotReadNavigationPropertyValue
		{
			get
			{
				return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_CannotReadNavigationPropertyValue");
			}
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x00058DB8 File Offset: 0x00056FB8
		internal static string ODataJsonLightEntryAndFeedDeserializer_UnexpectedExpandedSingletonNavigationLinkPropertyAnnotation(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_UnexpectedExpandedSingletonNavigationLinkPropertyAnnotation", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x00058DE0 File Offset: 0x00056FE0
		internal static string ODataJsonLightEntryAndFeedDeserializer_UnexpectedExpandedCollectionNavigationLinkPropertyAnnotation(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_UnexpectedExpandedCollectionNavigationLinkPropertyAnnotation", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x00058E08 File Offset: 0x00057008
		internal static string ODataJsonLightEntryAndFeedDeserializer_DuplicateExpandedFeedAnnotation(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_DuplicateExpandedFeedAnnotation", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x00058E30 File Offset: 0x00057030
		internal static string ODataJsonLightEntryAndFeedDeserializer_UnexpectedPropertyAnnotationAfterExpandedFeed(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_UnexpectedPropertyAnnotationAfterExpandedFeed", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06001A17 RID: 6679 RVA: 0x00058E57 File Offset: 0x00057057
		internal static string ODataJsonLightEntryAndFeedDeserializer_AnnotationGroupWithoutName
		{
			get
			{
				return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_AnnotationGroupWithoutName");
			}
		}

		// Token: 0x06001A18 RID: 6680 RVA: 0x00058E64 File Offset: 0x00057064
		internal static string ODataJsonLightEntryAndFeedDeserializer_AnnotationGroupMemberWithoutName(object p0)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_AnnotationGroupMemberWithoutName", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A19 RID: 6681 RVA: 0x00058E88 File Offset: 0x00057088
		internal static string ODataJsonLightEntryAndFeedDeserializer_AnnotationGroupMemberWithInvalidValue(object p0, object p1, object p2)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_AnnotationGroupMemberWithInvalidValue", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06001A1A RID: 6682 RVA: 0x00058EB3 File Offset: 0x000570B3
		internal static string ODataJsonLightEntryAndFeedDeserializer_AnnotationGroupInRequest
		{
			get
			{
				return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_AnnotationGroupInRequest");
			}
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x00058EC0 File Offset: 0x000570C0
		internal static string ODataJsonLightEntryAndFeedDeserializer_UnexpectedNavigationLinkInRequestPropertyAnnotation(object p0, object p1, object p2)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_UnexpectedNavigationLinkInRequestPropertyAnnotation", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x06001A1C RID: 6684 RVA: 0x00058EEC File Offset: 0x000570EC
		internal static string ODataJsonLightEntryAndFeedDeserializer_ArrayValueForSingletonBindPropertyAnnotation(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_ArrayValueForSingletonBindPropertyAnnotation", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x00058F14 File Offset: 0x00057114
		internal static string ODataJsonLightEntryAndFeedDeserializer_StringValueForCollectionBindPropertyAnnotation(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_StringValueForCollectionBindPropertyAnnotation", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A1E RID: 6686 RVA: 0x00058F3C File Offset: 0x0005713C
		internal static string ODataJsonLightEntryAndFeedDeserializer_EmptyBindArray(object p0)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_EmptyBindArray", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x00058F60 File Offset: 0x00057160
		internal static string ODataJsonLightEntryAndFeedDeserializer_NavigationPropertyWithoutValueAndEntityReferenceLink(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_NavigationPropertyWithoutValueAndEntityReferenceLink", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A20 RID: 6688 RVA: 0x00058F88 File Offset: 0x00057188
		internal static string ODataJsonLightEntryAndFeedDeserializer_SingletonNavigationPropertyWithBindingAndValue(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_SingletonNavigationPropertyWithBindingAndValue", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A21 RID: 6689 RVA: 0x00058FB0 File Offset: 0x000571B0
		internal static string ODataJsonLightEntryAndFeedDeserializer_PropertyWithoutValueWithUnknownType(object p0)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_PropertyWithoutValueWithUnknownType", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x00058FD4 File Offset: 0x000571D4
		internal static string ODataJsonLightEntryAndFeedDeserializer_FunctionImportIsNotActionOrFunction(object p0)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_FunctionImportIsNotActionOrFunction", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A23 RID: 6691 RVA: 0x00058FF8 File Offset: 0x000571F8
		internal static string ODataJsonLightEntryAndFeedDeserializer_MultipleOptionalPropertiesInOperation(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_MultipleOptionalPropertiesInOperation", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A24 RID: 6692 RVA: 0x00059020 File Offset: 0x00057220
		internal static string ODataJsonLightEntryAndFeedDeserializer_MultipleTargetPropertiesInOperation(object p0)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_MultipleTargetPropertiesInOperation", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A25 RID: 6693 RVA: 0x00059044 File Offset: 0x00057244
		internal static string ODataJsonLightEntryAndFeedDeserializer_OperationMissingTargetProperty(object p0)
		{
			return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_OperationMissingTargetProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06001A26 RID: 6694 RVA: 0x00059067 File Offset: 0x00057267
		internal static string ODataJsonLightEntryAndFeedDeserializer_MetadataReferencePropertyInRequest
		{
			get
			{
				return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_MetadataReferencePropertyInRequest");
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06001A27 RID: 6695 RVA: 0x00059073 File Offset: 0x00057273
		internal static string ODataJsonLightEntryAndFeedDeserializer_EncounteredAnnotationGroupInUnexpectedPosition
		{
			get
			{
				return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_EncounteredAnnotationGroupInUnexpectedPosition");
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06001A28 RID: 6696 RVA: 0x0005907F File Offset: 0x0005727F
		internal static string ODataJsonLightEntryAndFeedDeserializer_EntryTypeAlreadySpecified
		{
			get
			{
				return TextRes.GetString("ODataJsonLightEntryAndFeedDeserializer_EntryTypeAlreadySpecified");
			}
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x0005908C File Offset: 0x0005728C
		internal static string ODataJsonLightValidationUtils_OperationPropertyCannotBeNull(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightValidationUtils_OperationPropertyCannotBeNull", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A2A RID: 6698 RVA: 0x000590B4 File Offset: 0x000572B4
		internal static string ODataJsonLightValidationUtils_OpenMetadataReferencePropertyNotSupported(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightValidationUtils_OpenMetadataReferencePropertyNotSupported", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A2B RID: 6699 RVA: 0x000590DC File Offset: 0x000572DC
		internal static string ODataJsonLightDeserializer_RelativeUriUsedWithouODataMetadataAnnotation(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightDeserializer_RelativeUriUsedWithouODataMetadataAnnotation", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A2C RID: 6700 RVA: 0x00059104 File Offset: 0x00057304
		internal static string ODataJsonLightEntryMetadataContext_MetadataAnnotationMustBeInPayload(object p0)
		{
			return TextRes.GetString("ODataJsonLightEntryMetadataContext_MetadataAnnotationMustBeInPayload", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A2D RID: 6701 RVA: 0x00059128 File Offset: 0x00057328
		internal static string ODataJsonLightCollectionDeserializer_ExpectedCollectionPropertyNotFound(object p0)
		{
			return TextRes.GetString("ODataJsonLightCollectionDeserializer_ExpectedCollectionPropertyNotFound", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A2E RID: 6702 RVA: 0x0005914C File Offset: 0x0005734C
		internal static string ODataJsonLightCollectionDeserializer_CannotReadCollectionContentStart(object p0)
		{
			return TextRes.GetString("ODataJsonLightCollectionDeserializer_CannotReadCollectionContentStart", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A2F RID: 6703 RVA: 0x00059170 File Offset: 0x00057370
		internal static string ODataJsonLightCollectionDeserializer_CannotReadCollectionEnd(object p0)
		{
			return TextRes.GetString("ODataJsonLightCollectionDeserializer_CannotReadCollectionEnd", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A30 RID: 6704 RVA: 0x00059194 File Offset: 0x00057394
		internal static string ODataJsonLightCollectionDeserializer_InvalidCollectionTypeName(object p0)
		{
			return TextRes.GetString("ODataJsonLightCollectionDeserializer_InvalidCollectionTypeName", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A31 RID: 6705 RVA: 0x000591B8 File Offset: 0x000573B8
		internal static string ODataJsonLightEntityReferenceLinkDeserializer_EntityReferenceLinkMustBeObjectValue(object p0)
		{
			return TextRes.GetString("ODataJsonLightEntityReferenceLinkDeserializer_EntityReferenceLinkMustBeObjectValue", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A32 RID: 6706 RVA: 0x000591DC File Offset: 0x000573DC
		internal static string ODataJsonLightEntityReferenceLinkDeserializer_PropertyAnnotationForEntityReferenceLink(object p0)
		{
			return TextRes.GetString("ODataJsonLightEntityReferenceLinkDeserializer_PropertyAnnotationForEntityReferenceLink", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A33 RID: 6707 RVA: 0x00059200 File Offset: 0x00057400
		internal static string ODataJsonLightEntityReferenceLinkDeserializer_InvalidAnnotationInEntityReferenceLink(object p0)
		{
			return TextRes.GetString("ODataJsonLightEntityReferenceLinkDeserializer_InvalidAnnotationInEntityReferenceLink", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A34 RID: 6708 RVA: 0x00059224 File Offset: 0x00057424
		internal static string ODataJsonLightEntityReferenceLinkDeserializer_InvalidPropertyInEntityReferenceLink(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightEntityReferenceLinkDeserializer_InvalidPropertyInEntityReferenceLink", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A35 RID: 6709 RVA: 0x0005924C File Offset: 0x0005744C
		internal static string ODataJsonLightEntityReferenceLinkDeserializer_MissingEntityReferenceLinkProperty(object p0)
		{
			return TextRes.GetString("ODataJsonLightEntityReferenceLinkDeserializer_MissingEntityReferenceLinkProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A36 RID: 6710 RVA: 0x00059270 File Offset: 0x00057470
		internal static string ODataJsonLightEntityReferenceLinkDeserializer_MultipleUriPropertiesInEntityReferenceLink(object p0)
		{
			return TextRes.GetString("ODataJsonLightEntityReferenceLinkDeserializer_MultipleUriPropertiesInEntityReferenceLink", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A37 RID: 6711 RVA: 0x00059294 File Offset: 0x00057494
		internal static string ODataJsonLightEntityReferenceLinkDeserializer_EntityReferenceLinkUrlCannotBeNull(object p0)
		{
			return TextRes.GetString("ODataJsonLightEntityReferenceLinkDeserializer_EntityReferenceLinkUrlCannotBeNull", new object[]
			{
				p0
			});
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06001A38 RID: 6712 RVA: 0x000592B7 File Offset: 0x000574B7
		internal static string ODataJsonLightEntityReferenceLinkDeserializer_PropertyAnnotationForEntityReferenceLinks
		{
			get
			{
				return TextRes.GetString("ODataJsonLightEntityReferenceLinkDeserializer_PropertyAnnotationForEntityReferenceLinks");
			}
		}

		// Token: 0x06001A39 RID: 6713 RVA: 0x000592C4 File Offset: 0x000574C4
		internal static string ODataJsonLightEntityReferenceLinkDeserializer_InvalidEntityReferenceLinksPropertyFound(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightEntityReferenceLinkDeserializer_InvalidEntityReferenceLinksPropertyFound", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A3A RID: 6714 RVA: 0x000592EC File Offset: 0x000574EC
		internal static string ODataJsonLightEntityReferenceLinkDeserializer_InvalidPropertyAnnotationInEntityReferenceLinks(object p0)
		{
			return TextRes.GetString("ODataJsonLightEntityReferenceLinkDeserializer_InvalidPropertyAnnotationInEntityReferenceLinks", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A3B RID: 6715 RVA: 0x00059310 File Offset: 0x00057510
		internal static string ODataJsonLightEntityReferenceLinkDeserializer_ExpectedEntityReferenceLinksPropertyNotFound(object p0)
		{
			return TextRes.GetString("ODataJsonLightEntityReferenceLinkDeserializer_ExpectedEntityReferenceLinksPropertyNotFound", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A3C RID: 6716 RVA: 0x00059334 File Offset: 0x00057534
		internal static string ODataJsonOperationsDeserializerUtils_OperationPropertyCannotBeNull(object p0, object p1, object p2)
		{
			return TextRes.GetString("ODataJsonOperationsDeserializerUtils_OperationPropertyCannotBeNull", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x06001A3D RID: 6717 RVA: 0x00059360 File Offset: 0x00057560
		internal static string ODataJsonOperationsDeserializerUtils_OperationsPropertyMustHaveObjectValue(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonOperationsDeserializerUtils_OperationsPropertyMustHaveObjectValue", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A3E RID: 6718 RVA: 0x00059388 File Offset: 0x00057588
		internal static string ODataJsonOperationsDeserializerUtils_RepeatMetadataValue(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonOperationsDeserializerUtils_RepeatMetadataValue", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x000593B0 File Offset: 0x000575B0
		internal static string ODataJsonOperationsDeserializerUtils_MetadataMustHaveArrayValue(object p0, object p1, object p2)
		{
			return TextRes.GetString("ODataJsonOperationsDeserializerUtils_MetadataMustHaveArrayValue", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x06001A40 RID: 6720 RVA: 0x000593DC File Offset: 0x000575DC
		internal static string ODataJsonOperationsDeserializerUtils_OperationMetadataArrayExpectedAnObject(object p0, object p1, object p2)
		{
			return TextRes.GetString("ODataJsonOperationsDeserializerUtils_OperationMetadataArrayExpectedAnObject", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x06001A41 RID: 6721 RVA: 0x00059408 File Offset: 0x00057608
		internal static string ODataJsonOperationsDeserializerUtils_MultipleOptionalPropertiesInOperation(object p0, object p1, object p2)
		{
			return TextRes.GetString("ODataJsonOperationsDeserializerUtils_MultipleOptionalPropertiesInOperation", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x06001A42 RID: 6722 RVA: 0x00059434 File Offset: 0x00057634
		internal static string ODataJsonOperationsDeserializerUtils_MultipleTargetPropertiesInOperation(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonOperationsDeserializerUtils_MultipleTargetPropertiesInOperation", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A43 RID: 6723 RVA: 0x0005945C File Offset: 0x0005765C
		internal static string ODataJsonOperationsDeserializerUtils_OperationMissingTargetProperty(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonOperationsDeserializerUtils_OperationMissingTargetProperty", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A44 RID: 6724 RVA: 0x00059484 File Offset: 0x00057684
		internal static string ODataJsonLightServiceDocumentDeserializer_DuplicatePropertiesInServiceDocument(object p0)
		{
			return TextRes.GetString("ODataJsonLightServiceDocumentDeserializer_DuplicatePropertiesInServiceDocument", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A45 RID: 6725 RVA: 0x000594A8 File Offset: 0x000576A8
		internal static string ODataJsonLightServiceDocumentDeserializer_DuplicatePropertiesInResourceCollection(object p0)
		{
			return TextRes.GetString("ODataJsonLightServiceDocumentDeserializer_DuplicatePropertiesInResourceCollection", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A46 RID: 6726 RVA: 0x000594CC File Offset: 0x000576CC
		internal static string ODataJsonLightServiceDocumentDeserializer_MissingValuePropertyInServiceDocument(object p0)
		{
			return TextRes.GetString("ODataJsonLightServiceDocumentDeserializer_MissingValuePropertyInServiceDocument", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A47 RID: 6727 RVA: 0x000594F0 File Offset: 0x000576F0
		internal static string ODataJsonLightServiceDocumentDeserializer_MissingRequiredPropertyInResourceCollection(object p0)
		{
			return TextRes.GetString("ODataJsonLightServiceDocumentDeserializer_MissingRequiredPropertyInResourceCollection", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A48 RID: 6728 RVA: 0x00059514 File Offset: 0x00057714
		internal static string ODataJsonLightServiceDocumentDeserializer_PropertyAnnotationInServiceDocument(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightServiceDocumentDeserializer_PropertyAnnotationInServiceDocument", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A49 RID: 6729 RVA: 0x0005953C File Offset: 0x0005773C
		internal static string ODataJsonLightServiceDocumentDeserializer_InstanceAnnotationInServiceDocument(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightServiceDocumentDeserializer_InstanceAnnotationInServiceDocument", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A4A RID: 6730 RVA: 0x00059564 File Offset: 0x00057764
		internal static string ODataJsonLightServiceDocumentDeserializer_PropertyAnnotationInResourceCollection(object p0)
		{
			return TextRes.GetString("ODataJsonLightServiceDocumentDeserializer_PropertyAnnotationInResourceCollection", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A4B RID: 6731 RVA: 0x00059588 File Offset: 0x00057788
		internal static string ODataJsonLightServiceDocumentDeserializer_InstanceAnnotationInResourceCollection(object p0)
		{
			return TextRes.GetString("ODataJsonLightServiceDocumentDeserializer_InstanceAnnotationInResourceCollection", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A4C RID: 6732 RVA: 0x000595AC File Offset: 0x000577AC
		internal static string ODataJsonLightServiceDocumentDeserializer_UnexpectedPropertyInResourceCollection(object p0, object p1, object p2)
		{
			return TextRes.GetString("ODataJsonLightServiceDocumentDeserializer_UnexpectedPropertyInResourceCollection", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x06001A4D RID: 6733 RVA: 0x000595D8 File Offset: 0x000577D8
		internal static string ODataJsonLightServiceDocumentDeserializer_UnexpectedPropertyInServiceDocument(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightServiceDocumentDeserializer_UnexpectedPropertyInServiceDocument", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A4E RID: 6734 RVA: 0x00059600 File Offset: 0x00057800
		internal static string ODataJsonLightServiceDocumentDeserializer_PropertyAnnotationWithoutProperty(object p0)
		{
			return TextRes.GetString("ODataJsonLightServiceDocumentDeserializer_PropertyAnnotationWithoutProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06001A4F RID: 6735 RVA: 0x00059623 File Offset: 0x00057823
		internal static string ODataJsonLightParameterDeserializer_PropertyAnnotationForParameters
		{
			get
			{
				return TextRes.GetString("ODataJsonLightParameterDeserializer_PropertyAnnotationForParameters");
			}
		}

		// Token: 0x06001A50 RID: 6736 RVA: 0x00059630 File Offset: 0x00057830
		internal static string ODataJsonLightParameterDeserializer_PropertyAnnotationWithoutPropertyForParameters(object p0)
		{
			return TextRes.GetString("ODataJsonLightParameterDeserializer_PropertyAnnotationWithoutPropertyForParameters", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A51 RID: 6737 RVA: 0x00059654 File Offset: 0x00057854
		internal static string ODataJsonLightParameterDeserializer_UnsupportedPrimitiveParameterType(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightParameterDeserializer_UnsupportedPrimitiveParameterType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A52 RID: 6738 RVA: 0x0005967C File Offset: 0x0005787C
		internal static string ODataJsonLightParameterDeserializer_NullCollectionExpected(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightParameterDeserializer_NullCollectionExpected", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A53 RID: 6739 RVA: 0x000596A4 File Offset: 0x000578A4
		internal static string ODataJsonLightParameterDeserializer_UnsupportedParameterTypeKind(object p0, object p1)
		{
			return TextRes.GetString("ODataJsonLightParameterDeserializer_UnsupportedParameterTypeKind", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06001A54 RID: 6740 RVA: 0x000596CB File Offset: 0x000578CB
		internal static string SelectedPropertiesNode_StarSegmentNotLastSegment
		{
			get
			{
				return TextRes.GetString("SelectedPropertiesNode_StarSegmentNotLastSegment");
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06001A55 RID: 6741 RVA: 0x000596D7 File Offset: 0x000578D7
		internal static string SelectedPropertiesNode_StarSegmentAfterTypeSegment
		{
			get
			{
				return TextRes.GetString("SelectedPropertiesNode_StarSegmentAfterTypeSegment");
			}
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x000596E4 File Offset: 0x000578E4
		internal static string ODataJsonLightErrorDeserializer_PropertyAnnotationNotAllowedInErrorPayload(object p0)
		{
			return TextRes.GetString("ODataJsonLightErrorDeserializer_PropertyAnnotationNotAllowedInErrorPayload", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x00059708 File Offset: 0x00057908
		internal static string ODataJsonLightErrorDeserializer_InstanceAnnotationNotAllowedInErrorPayload(object p0)
		{
			return TextRes.GetString("ODataJsonLightErrorDeserializer_InstanceAnnotationNotAllowedInErrorPayload", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A58 RID: 6744 RVA: 0x0005972C File Offset: 0x0005792C
		internal static string ODataJsonLightErrorDeserializer_PropertyAnnotationWithoutPropertyForError(object p0)
		{
			return TextRes.GetString("ODataJsonLightErrorDeserializer_PropertyAnnotationWithoutPropertyForError", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x00059750 File Offset: 0x00057950
		internal static string ODataJsonLightErrorDeserializer_TopLevelErrorValueWithInvalidProperty(object p0)
		{
			return TextRes.GetString("ODataJsonLightErrorDeserializer_TopLevelErrorValueWithInvalidProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x00059774 File Offset: 0x00057974
		internal static string ODataConventionalUriBuilder_EntityTypeWithNoKeyProperties(object p0)
		{
			return TextRes.GetString("ODataConventionalUriBuilder_EntityTypeWithNoKeyProperties", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A5B RID: 6747 RVA: 0x00059798 File Offset: 0x00057998
		internal static string ODataConventionalUriBuilder_NullKeyValue(object p0, object p1)
		{
			return TextRes.GetString("ODataConventionalUriBuilder_NullKeyValue", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A5C RID: 6748 RVA: 0x000597C0 File Offset: 0x000579C0
		internal static string ODataEntryMetadataContext_EntityTypeWithNoKeyProperties(object p0)
		{
			return TextRes.GetString("ODataEntryMetadataContext_EntityTypeWithNoKeyProperties", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A5D RID: 6749 RVA: 0x000597E4 File Offset: 0x000579E4
		internal static string ODataEntryMetadataContext_NullKeyValue(object p0, object p1)
		{
			return TextRes.GetString("ODataEntryMetadataContext_NullKeyValue", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x0005980C File Offset: 0x00057A0C
		internal static string ODataEntryMetadataContext_KeyOrETagValuesMustBePrimitiveValues(object p0, object p1)
		{
			return TextRes.GetString("ODataEntryMetadataContext_KeyOrETagValuesMustBePrimitiveValues", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A5F RID: 6751 RVA: 0x00059834 File Offset: 0x00057A34
		internal static string EdmValueUtils_NonPrimitiveValue(object p0, object p1)
		{
			return TextRes.GetString("EdmValueUtils_NonPrimitiveValue", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x0005985C File Offset: 0x00057A5C
		internal static string EdmValueUtils_PropertyDoesntExist(object p0, object p1)
		{
			return TextRes.GetString("EdmValueUtils_PropertyDoesntExist", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06001A61 RID: 6753 RVA: 0x00059883 File Offset: 0x00057A83
		internal static string JsonLightAnnotationGroupDeserializer_AnnotationGroupDeclarationWithoutName
		{
			get
			{
				return TextRes.GetString("JsonLightAnnotationGroupDeserializer_AnnotationGroupDeclarationWithoutName");
			}
		}

		// Token: 0x06001A62 RID: 6754 RVA: 0x00059890 File Offset: 0x00057A90
		internal static string JsonLightAnnotationGroupDeserializer_InvalidAnnotationFoundInsideAnnotationGroup(object p0)
		{
			return TextRes.GetString("JsonLightAnnotationGroupDeserializer_InvalidAnnotationFoundInsideAnnotationGroup", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A63 RID: 6755 RVA: 0x000598B4 File Offset: 0x00057AB4
		internal static string JsonLightAnnotationGroupDeserializer_InvalidAnnotationFoundInsideNamedAnnotationGroup(object p0, object p1)
		{
			return TextRes.GetString("JsonLightAnnotationGroupDeserializer_InvalidAnnotationFoundInsideNamedAnnotationGroup", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06001A64 RID: 6756 RVA: 0x000598DB File Offset: 0x00057ADB
		internal static string JsonLightAnnotationGroupDeserializer_EncounteredMultipleNameProperties
		{
			get
			{
				return TextRes.GetString("JsonLightAnnotationGroupDeserializer_EncounteredMultipleNameProperties");
			}
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x000598E8 File Offset: 0x00057AE8
		internal static string JsonLightAnnotationGroupDeserializer_UndefinedAnnotationGroupReference(object p0)
		{
			return TextRes.GetString("JsonLightAnnotationGroupDeserializer_UndefinedAnnotationGroupReference", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A66 RID: 6758 RVA: 0x0005990C File Offset: 0x00057B0C
		internal static string JsonLightAnnotationGroupDeserializer_MultipleAnnotationGroupsWithSameName(object p0)
		{
			return TextRes.GetString("JsonLightAnnotationGroupDeserializer_MultipleAnnotationGroupsWithSameName", new object[]
			{
				p0
			});
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06001A67 RID: 6759 RVA: 0x0005992F File Offset: 0x00057B2F
		internal static string ODataPrimitiveValue_CannotCreateODataPrimitiveValueFromNull
		{
			get
			{
				return TextRes.GetString("ODataPrimitiveValue_CannotCreateODataPrimitiveValueFromNull");
			}
		}

		// Token: 0x06001A68 RID: 6760 RVA: 0x0005993C File Offset: 0x00057B3C
		internal static string ODataPrimitiveValue_CannotCreateODataPrimitiveValueFromUnsupportedValueType(object p0)
		{
			return TextRes.GetString("ODataPrimitiveValue_CannotCreateODataPrimitiveValueFromUnsupportedValueType", new object[]
			{
				p0
			});
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06001A69 RID: 6761 RVA: 0x0005995F File Offset: 0x00057B5F
		internal static string ODataAnnotatable_InstanceAnnotationsOnlyOnODataError
		{
			get
			{
				return TextRes.GetString("ODataAnnotatable_InstanceAnnotationsOnlyOnODataError");
			}
		}

		// Token: 0x06001A6A RID: 6762 RVA: 0x0005996C File Offset: 0x00057B6C
		internal static string ODataInstanceAnnotation_NeedPeriodInName(object p0)
		{
			return TextRes.GetString("ODataInstanceAnnotation_NeedPeriodInName", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x00059990 File Offset: 0x00057B90
		internal static string ODataInstanceAnnotation_ReservedNamesNotAllowed(object p0, object p1)
		{
			return TextRes.GetString("ODataInstanceAnnotation_ReservedNamesNotAllowed", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A6C RID: 6764 RVA: 0x000599B8 File Offset: 0x00057BB8
		internal static string ODataInstanceAnnotation_BadTermName(object p0)
		{
			return TextRes.GetString("ODataInstanceAnnotation_BadTermName", new object[]
			{
				p0
			});
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06001A6D RID: 6765 RVA: 0x000599DB File Offset: 0x00057BDB
		internal static string ODataInstanceAnnotation_ValueCannotBeODataStreamReferenceValue
		{
			get
			{
				return TextRes.GetString("ODataInstanceAnnotation_ValueCannotBeODataStreamReferenceValue");
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06001A6E RID: 6766 RVA: 0x000599E7 File Offset: 0x00057BE7
		internal static string ODataJsonLightValueSerializer_MissingTypeNameOnComplex
		{
			get
			{
				return TextRes.GetString("ODataJsonLightValueSerializer_MissingTypeNameOnComplex");
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06001A6F RID: 6767 RVA: 0x000599F3 File Offset: 0x00057BF3
		internal static string ODataJsonLightValueSerializer_MissingTypeNameOnCollection
		{
			get
			{
				return TextRes.GetString("ODataJsonLightValueSerializer_MissingTypeNameOnCollection");
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06001A70 RID: 6768 RVA: 0x000599FF File Offset: 0x00057BFF
		internal static string AtomInstanceAnnotation_MissingTermAttributeOnAnnotationElement
		{
			get
			{
				return TextRes.GetString("AtomInstanceAnnotation_MissingTermAttributeOnAnnotationElement");
			}
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x00059A0C File Offset: 0x00057C0C
		internal static string AtomInstanceAnnotation_AttributeValueNotationUsedWithIncompatibleType(object p0, object p1)
		{
			return TextRes.GetString("AtomInstanceAnnotation_AttributeValueNotationUsedWithIncompatibleType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x00059A34 File Offset: 0x00057C34
		internal static string AtomInstanceAnnotation_AttributeValueNotationUsedOnNonEmptyElement(object p0)
		{
			return TextRes.GetString("AtomInstanceAnnotation_AttributeValueNotationUsedOnNonEmptyElement", new object[]
			{
				p0
			});
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06001A73 RID: 6771 RVA: 0x00059A57 File Offset: 0x00057C57
		internal static string AtomInstanceAnnotation_MultipleAttributeValueNotationAttributes
		{
			get
			{
				return TextRes.GetString("AtomInstanceAnnotation_MultipleAttributeValueNotationAttributes");
			}
		}

		// Token: 0x06001A74 RID: 6772 RVA: 0x00059A64 File Offset: 0x00057C64
		internal static string AnnotationFilterPattern_InvalidPatternMissingDot(object p0)
		{
			return TextRes.GetString("AnnotationFilterPattern_InvalidPatternMissingDot", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A75 RID: 6773 RVA: 0x00059A88 File Offset: 0x00057C88
		internal static string AnnotationFilterPattern_InvalidPatternEmptySegment(object p0)
		{
			return TextRes.GetString("AnnotationFilterPattern_InvalidPatternEmptySegment", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A76 RID: 6774 RVA: 0x00059AAC File Offset: 0x00057CAC
		internal static string AnnotationFilterPattern_InvalidPatternWildCardInSegment(object p0)
		{
			return TextRes.GetString("AnnotationFilterPattern_InvalidPatternWildCardInSegment", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A77 RID: 6775 RVA: 0x00059AD0 File Offset: 0x00057CD0
		internal static string AnnotationFilterPattern_InvalidPatternWildCardMustBeInLastSegment(object p0)
		{
			return TextRes.GetString("AnnotationFilterPattern_InvalidPatternWildCardMustBeInLastSegment", new object[]
			{
				p0
			});
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06001A78 RID: 6776 RVA: 0x00059AF3 File Offset: 0x00057CF3
		internal static string JsonFullMetadataLevel_MissingEntitySet
		{
			get
			{
				return TextRes.GetString("JsonFullMetadataLevel_MissingEntitySet");
			}
		}

		// Token: 0x06001A79 RID: 6777 RVA: 0x00059B00 File Offset: 0x00057D00
		internal static string ODataQueryUtils_DidNotFindServiceOperation(object p0)
		{
			return TextRes.GetString("ODataQueryUtils_DidNotFindServiceOperation", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A7A RID: 6778 RVA: 0x00059B24 File Offset: 0x00057D24
		internal static string ODataQueryUtils_FoundMultipleServiceOperations(object p0)
		{
			return TextRes.GetString("ODataQueryUtils_FoundMultipleServiceOperations", new object[]
			{
				p0
			});
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06001A7B RID: 6779 RVA: 0x00059B47 File Offset: 0x00057D47
		internal static string ODataQueryUtils_CannotSetMetadataAnnotationOnPrimitiveType
		{
			get
			{
				return TextRes.GetString("ODataQueryUtils_CannotSetMetadataAnnotationOnPrimitiveType");
			}
		}

		// Token: 0x06001A7C RID: 6780 RVA: 0x00059B54 File Offset: 0x00057D54
		internal static string ODataQueryUtils_DidNotFindEntitySet(object p0)
		{
			return TextRes.GetString("ODataQueryUtils_DidNotFindEntitySet", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A7D RID: 6781 RVA: 0x00059B78 File Offset: 0x00057D78
		internal static string BinaryOperatorQueryNode_InvalidOperandType(object p0, object p1)
		{
			return TextRes.GetString("BinaryOperatorQueryNode_InvalidOperandType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A7E RID: 6782 RVA: 0x00059BA0 File Offset: 0x00057DA0
		internal static string BinaryOperatorQueryNode_OperandsMustHaveSameTypes(object p0, object p1)
		{
			return TextRes.GetString("BinaryOperatorQueryNode_OperandsMustHaveSameTypes", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x00059BC8 File Offset: 0x00057DC8
		internal static string SyntacticTree_UriMustBeAbsolute(object p0)
		{
			return TextRes.GetString("SyntacticTree_UriMustBeAbsolute", new object[]
			{
				p0
			});
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06001A80 RID: 6784 RVA: 0x00059BEB File Offset: 0x00057DEB
		internal static string SyntacticTree_MaxDepthInvalid
		{
			get
			{
				return TextRes.GetString("SyntacticTree_MaxDepthInvalid");
			}
		}

		// Token: 0x06001A81 RID: 6785 RVA: 0x00059BF8 File Offset: 0x00057DF8
		internal static string SyntacticTree_InvalidSkipQueryOptionValue(object p0)
		{
			return TextRes.GetString("SyntacticTree_InvalidSkipQueryOptionValue", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A82 RID: 6786 RVA: 0x00059C1C File Offset: 0x00057E1C
		internal static string SyntacticTree_InvalidTopQueryOptionValue(object p0)
		{
			return TextRes.GetString("SyntacticTree_InvalidTopQueryOptionValue", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x00059C40 File Offset: 0x00057E40
		internal static string SyntacticTree_InvalidInlineCountQueryOptionValue(object p0, object p1)
		{
			return TextRes.GetString("SyntacticTree_InvalidInlineCountQueryOptionValue", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A84 RID: 6788 RVA: 0x00059C68 File Offset: 0x00057E68
		internal static string QueryOptionUtils_QueryParameterMustBeSpecifiedOnce(object p0)
		{
			return TextRes.GetString("QueryOptionUtils_QueryParameterMustBeSpecifiedOnce", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A85 RID: 6789 RVA: 0x00059C8C File Offset: 0x00057E8C
		internal static string UriBuilder_NotSupportedClrLiteral(object p0)
		{
			return TextRes.GetString("UriBuilder_NotSupportedClrLiteral", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A86 RID: 6790 RVA: 0x00059CB0 File Offset: 0x00057EB0
		internal static string UriBuilder_NotSupportedQueryToken(object p0)
		{
			return TextRes.GetString("UriBuilder_NotSupportedQueryToken", new object[]
			{
				p0
			});
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06001A87 RID: 6791 RVA: 0x00059CD3 File Offset: 0x00057ED3
		internal static string UriQueryExpressionParser_TooDeep
		{
			get
			{
				return TextRes.GetString("UriQueryExpressionParser_TooDeep");
			}
		}

		// Token: 0x06001A88 RID: 6792 RVA: 0x00059CE0 File Offset: 0x00057EE0
		internal static string UriQueryExpressionParser_ExpressionExpected(object p0, object p1)
		{
			return TextRes.GetString("UriQueryExpressionParser_ExpressionExpected", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A89 RID: 6793 RVA: 0x00059D08 File Offset: 0x00057F08
		internal static string UriQueryExpressionParser_OpenParenExpected(object p0, object p1)
		{
			return TextRes.GetString("UriQueryExpressionParser_OpenParenExpected", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x00059D30 File Offset: 0x00057F30
		internal static string UriQueryExpressionParser_CloseParenOrCommaExpected(object p0, object p1)
		{
			return TextRes.GetString("UriQueryExpressionParser_CloseParenOrCommaExpected", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A8B RID: 6795 RVA: 0x00059D58 File Offset: 0x00057F58
		internal static string UriQueryExpressionParser_CloseParenOrOperatorExpected(object p0, object p1)
		{
			return TextRes.GetString("UriQueryExpressionParser_CloseParenOrOperatorExpected", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06001A8C RID: 6796 RVA: 0x00059D7F File Offset: 0x00057F7F
		internal static string UriQueryExpressionParser_RepeatedVisitor
		{
			get
			{
				return TextRes.GetString("UriQueryExpressionParser_RepeatedVisitor");
			}
		}

		// Token: 0x06001A8D RID: 6797 RVA: 0x00059D8C File Offset: 0x00057F8C
		internal static string UriQueryExpressionParser_CannotCreateStarTokenFromNonStar(object p0)
		{
			return TextRes.GetString("UriQueryExpressionParser_CannotCreateStarTokenFromNonStar", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A8E RID: 6798 RVA: 0x00059DB0 File Offset: 0x00057FB0
		internal static string UriQueryExpressionParser_RangeVariableAlreadyDeclared(object p0)
		{
			return TextRes.GetString("UriQueryExpressionParser_RangeVariableAlreadyDeclared", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x00059DD4 File Offset: 0x00057FD4
		internal static string UriQueryPathParser_RequestUriDoesNotHaveTheCorrectBaseUri(object p0, object p1)
		{
			return TextRes.GetString("UriQueryPathParser_RequestUriDoesNotHaveTheCorrectBaseUri", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06001A90 RID: 6800 RVA: 0x00059DFB File Offset: 0x00057FFB
		internal static string UriQueryPathParser_SyntaxError
		{
			get
			{
				return TextRes.GetString("UriQueryPathParser_SyntaxError");
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06001A91 RID: 6801 RVA: 0x00059E07 File Offset: 0x00058007
		internal static string UriQueryPathParser_TooManySegments
		{
			get
			{
				return TextRes.GetString("UriQueryPathParser_TooManySegments");
			}
		}

		// Token: 0x06001A92 RID: 6802 RVA: 0x00059E14 File Offset: 0x00058014
		internal static string UriQueryPathParser_InvalidKeyValueLiteral(object p0)
		{
			return TextRes.GetString("UriQueryPathParser_InvalidKeyValueLiteral", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A93 RID: 6803 RVA: 0x00059E38 File Offset: 0x00058038
		internal static string PropertyInfoTypeAnnotation_CannotFindProperty(object p0, object p1, object p2)
		{
			return TextRes.GetString("PropertyInfoTypeAnnotation_CannotFindProperty", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06001A94 RID: 6804 RVA: 0x00059E63 File Offset: 0x00058063
		internal static string SelectionItemBinder_NonNavigationPathToken
		{
			get
			{
				return TextRes.GetString("SelectionItemBinder_NonNavigationPathToken");
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06001A95 RID: 6805 RVA: 0x00059E6F File Offset: 0x0005806F
		internal static string SelectTreeNormalizer_NonPathProperty
		{
			get
			{
				return TextRes.GetString("SelectTreeNormalizer_NonPathProperty");
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06001A96 RID: 6806 RVA: 0x00059E7B File Offset: 0x0005807B
		internal static string ExpandItem_NonEntityNavProp
		{
			get
			{
				return TextRes.GetString("ExpandItem_NonEntityNavProp");
			}
		}

		// Token: 0x06001A97 RID: 6807 RVA: 0x00059E88 File Offset: 0x00058088
		internal static string MetadataBinder_UnsupportedQueryTokenKind(object p0)
		{
			return TextRes.GetString("MetadataBinder_UnsupportedQueryTokenKind", new object[]
			{
				p0
			});
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06001A98 RID: 6808 RVA: 0x00059EAB File Offset: 0x000580AB
		internal static string MetadataBinder_UnsupportedExtensionToken
		{
			get
			{
				return TextRes.GetString("MetadataBinder_UnsupportedExtensionToken");
			}
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x00059EB8 File Offset: 0x000580B8
		internal static string MetadataBinder_RootSegmentResourceNotFound(object p0)
		{
			return TextRes.GetString("MetadataBinder_RootSegmentResourceNotFound", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x00059EDC File Offset: 0x000580DC
		internal static string MetadataBinder_KeyValueApplicableOnlyToEntityType(object p0)
		{
			return TextRes.GetString("MetadataBinder_KeyValueApplicableOnlyToEntityType", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x00059F00 File Offset: 0x00058100
		internal static string MetadataBinder_PropertyNotDeclared(object p0, object p1)
		{
			return TextRes.GetString("MetadataBinder_PropertyNotDeclared", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x00059F28 File Offset: 0x00058128
		internal static string MetadataBinder_PropertyNotDeclaredOrNotKeyInKeyValue(object p0, object p1)
		{
			return TextRes.GetString("MetadataBinder_PropertyNotDeclaredOrNotKeyInKeyValue", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x00059F50 File Offset: 0x00058150
		internal static string MetadataBinder_UnnamedKeyValueOnTypeWithMultipleKeyProperties(object p0)
		{
			return TextRes.GetString("MetadataBinder_UnnamedKeyValueOnTypeWithMultipleKeyProperties", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x00059F74 File Offset: 0x00058174
		internal static string MetadataBinder_DuplicitKeyPropertyInKeyValues(object p0)
		{
			return TextRes.GetString("MetadataBinder_DuplicitKeyPropertyInKeyValues", new object[]
			{
				p0
			});
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x00059F98 File Offset: 0x00058198
		internal static string MetadataBinder_NotAllKeyPropertiesSpecifiedInKeyValues(object p0)
		{
			return TextRes.GetString("MetadataBinder_NotAllKeyPropertiesSpecifiedInKeyValues", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AA0 RID: 6816 RVA: 0x00059FBC File Offset: 0x000581BC
		internal static string MetadataBinder_CannotConvertToType(object p0, object p1)
		{
			return TextRes.GetString("MetadataBinder_CannotConvertToType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001AA1 RID: 6817 RVA: 0x00059FE4 File Offset: 0x000581E4
		internal static string MetadataBinder_NonQueryableServiceOperationWithKeyLookup(object p0)
		{
			return TextRes.GetString("MetadataBinder_NonQueryableServiceOperationWithKeyLookup", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AA2 RID: 6818 RVA: 0x0005A008 File Offset: 0x00058208
		internal static string MetadataBinder_QueryServiceOperationOfNonEntityType(object p0, object p1, object p2)
		{
			return TextRes.GetString("MetadataBinder_QueryServiceOperationOfNonEntityType", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x06001AA3 RID: 6819 RVA: 0x0005A034 File Offset: 0x00058234
		internal static string MetadataBinder_ServiceOperationParameterMissing(object p0, object p1)
		{
			return TextRes.GetString("MetadataBinder_ServiceOperationParameterMissing", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001AA4 RID: 6820 RVA: 0x0005A05C File Offset: 0x0005825C
		internal static string MetadataBinder_ServiceOperationParameterInvalidType(object p0, object p1, object p2, object p3)
		{
			return TextRes.GetString("MetadataBinder_ServiceOperationParameterInvalidType", new object[]
			{
				p0,
				p1,
				p2,
				p3
			});
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06001AA5 RID: 6821 RVA: 0x0005A08B File Offset: 0x0005828B
		internal static string MetadataBinder_FilterExpressionNotSingleValue
		{
			get
			{
				return TextRes.GetString("MetadataBinder_FilterExpressionNotSingleValue");
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06001AA6 RID: 6822 RVA: 0x0005A097 File Offset: 0x00058297
		internal static string MetadataBinder_OrderByExpressionNotSingleValue
		{
			get
			{
				return TextRes.GetString("MetadataBinder_OrderByExpressionNotSingleValue");
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06001AA7 RID: 6823 RVA: 0x0005A0A3 File Offset: 0x000582A3
		internal static string MetadataBinder_PropertyAccessWithoutParentParameter
		{
			get
			{
				return TextRes.GetString("MetadataBinder_PropertyAccessWithoutParentParameter");
			}
		}

		// Token: 0x06001AA8 RID: 6824 RVA: 0x0005A0B0 File Offset: 0x000582B0
		internal static string MetadataBinder_MultiValuePropertyNotSupportedInExpression(object p0)
		{
			return TextRes.GetString("MetadataBinder_MultiValuePropertyNotSupportedInExpression", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AA9 RID: 6825 RVA: 0x0005A0D4 File Offset: 0x000582D4
		internal static string MetadataBinder_BinaryOperatorOperandNotSingleValue(object p0)
		{
			return TextRes.GetString("MetadataBinder_BinaryOperatorOperandNotSingleValue", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AAA RID: 6826 RVA: 0x0005A0F8 File Offset: 0x000582F8
		internal static string MetadataBinder_UnaryOperatorOperandNotSingleValue(object p0)
		{
			return TextRes.GetString("MetadataBinder_UnaryOperatorOperandNotSingleValue", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AAB RID: 6827 RVA: 0x0005A11C File Offset: 0x0005831C
		internal static string MetadataBinder_PropertyAccessSourceNotSingleValue(object p0)
		{
			return TextRes.GetString("MetadataBinder_PropertyAccessSourceNotSingleValue", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AAC RID: 6828 RVA: 0x0005A140 File Offset: 0x00058340
		internal static string MetadataBinder_IncompatibleOperandsError(object p0, object p1, object p2)
		{
			return TextRes.GetString("MetadataBinder_IncompatibleOperandsError", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x06001AAD RID: 6829 RVA: 0x0005A16C File Offset: 0x0005836C
		internal static string MetadataBinder_IncompatibleOperandError(object p0, object p1)
		{
			return TextRes.GetString("MetadataBinder_IncompatibleOperandError", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001AAE RID: 6830 RVA: 0x0005A194 File Offset: 0x00058394
		internal static string MetadataBinder_UnknownFunction(object p0)
		{
			return TextRes.GetString("MetadataBinder_UnknownFunction", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AAF RID: 6831 RVA: 0x0005A1B8 File Offset: 0x000583B8
		internal static string MetadataBinder_FunctionArgumentNotSingleValue(object p0)
		{
			return TextRes.GetString("MetadataBinder_FunctionArgumentNotSingleValue", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AB0 RID: 6832 RVA: 0x0005A1DC File Offset: 0x000583DC
		internal static string MetadataBinder_NoApplicableFunctionFound(object p0, object p1)
		{
			return TextRes.GetString("MetadataBinder_NoApplicableFunctionFound", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06001AB1 RID: 6833 RVA: 0x0005A203 File Offset: 0x00058403
		internal static string MetadataBinder_BuiltInFunctionSignatureWithoutAReturnType
		{
			get
			{
				return TextRes.GetString("MetadataBinder_BuiltInFunctionSignatureWithoutAReturnType");
			}
		}

		// Token: 0x06001AB2 RID: 6834 RVA: 0x0005A210 File Offset: 0x00058410
		internal static string MetadataBinder_UnsupportedSystemQueryOption(object p0)
		{
			return TextRes.GetString("MetadataBinder_UnsupportedSystemQueryOption", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AB3 RID: 6835 RVA: 0x0005A234 File Offset: 0x00058434
		internal static string MetadataBinder_BoundNodeCannotBeNull(object p0)
		{
			return TextRes.GetString("MetadataBinder_BoundNodeCannotBeNull", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AB4 RID: 6836 RVA: 0x0005A258 File Offset: 0x00058458
		internal static string MetadataBinder_TopRequiresNonNegativeInteger(object p0)
		{
			return TextRes.GetString("MetadataBinder_TopRequiresNonNegativeInteger", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x0005A27C File Offset: 0x0005847C
		internal static string MetadataBinder_SkipRequiresNonNegativeInteger(object p0)
		{
			return TextRes.GetString("MetadataBinder_SkipRequiresNonNegativeInteger", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AB6 RID: 6838 RVA: 0x0005A2A0 File Offset: 0x000584A0
		internal static string MetadataBinder_ServiceOperationWithoutResultKind(object p0)
		{
			return TextRes.GetString("MetadataBinder_ServiceOperationWithoutResultKind", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AB7 RID: 6839 RVA: 0x0005A2C4 File Offset: 0x000584C4
		internal static string MetadataBinder_HierarchyNotFollowed(object p0, object p1)
		{
			return TextRes.GetString("MetadataBinder_HierarchyNotFollowed", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06001AB8 RID: 6840 RVA: 0x0005A2EB File Offset: 0x000584EB
		internal static string MetadataBinder_MustBeCalledOnRoot
		{
			get
			{
				return TextRes.GetString("MetadataBinder_MustBeCalledOnRoot");
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06001AB9 RID: 6841 RVA: 0x0005A2F7 File Offset: 0x000584F7
		internal static string MetadataBinder_NoTypeSupported
		{
			get
			{
				return TextRes.GetString("MetadataBinder_NoTypeSupported");
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06001ABA RID: 6842 RVA: 0x0005A303 File Offset: 0x00058503
		internal static string MetadataBinder_LambdaParentMustBeCollection
		{
			get
			{
				return TextRes.GetString("MetadataBinder_LambdaParentMustBeCollection");
			}
		}

		// Token: 0x06001ABB RID: 6843 RVA: 0x0005A310 File Offset: 0x00058510
		internal static string MetadataBinder_ParameterNotInScope(object p0)
		{
			return TextRes.GetString("MetadataBinder_ParameterNotInScope", new object[]
			{
				p0
			});
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06001ABC RID: 6844 RVA: 0x0005A333 File Offset: 0x00058533
		internal static string MetadataBinder_NullNavigationProperty
		{
			get
			{
				return TextRes.GetString("MetadataBinder_NullNavigationProperty");
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06001ABD RID: 6845 RVA: 0x0005A33F File Offset: 0x0005853F
		internal static string MetadataBinder_NavigationPropertyNotFollowingSingleEntityType
		{
			get
			{
				return TextRes.GetString("MetadataBinder_NavigationPropertyNotFollowingSingleEntityType");
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06001ABE RID: 6846 RVA: 0x0005A34B File Offset: 0x0005854B
		internal static string MetadataBinder_AnyAllExpressionNotSingleValue
		{
			get
			{
				return TextRes.GetString("MetadataBinder_AnyAllExpressionNotSingleValue");
			}
		}

		// Token: 0x06001ABF RID: 6847 RVA: 0x0005A358 File Offset: 0x00058558
		internal static string MetadataBinder_CastOrIsOfExpressionWithWrongNumberOfOperands(object p0)
		{
			return TextRes.GetString("MetadataBinder_CastOrIsOfExpressionWithWrongNumberOfOperands", new object[]
			{
				p0
			});
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06001AC0 RID: 6848 RVA: 0x0005A37B File Offset: 0x0005857B
		internal static string MetadataBinder_CastOrIsOfFunctionWithoutATypeArgument
		{
			get
			{
				return TextRes.GetString("MetadataBinder_CastOrIsOfFunctionWithoutATypeArgument");
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06001AC1 RID: 6849 RVA: 0x0005A387 File Offset: 0x00058587
		internal static string MetadataBinder_CastOrIsOfCollectionsNotSupported
		{
			get
			{
				return TextRes.GetString("MetadataBinder_CastOrIsOfCollectionsNotSupported");
			}
		}

		// Token: 0x06001AC2 RID: 6850 RVA: 0x0005A394 File Offset: 0x00058594
		internal static string MetadataBinder_SpatialLengthFunctionWithInvalidArgs(object p0)
		{
			return TextRes.GetString("MetadataBinder_SpatialLengthFunctionWithInvalidArgs", new object[]
			{
				p0
			});
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06001AC3 RID: 6851 RVA: 0x0005A3B7 File Offset: 0x000585B7
		internal static string MetadataBinder_SpatialLengthFunctionWithoutASingleValueArg
		{
			get
			{
				return TextRes.GetString("MetadataBinder_SpatialLengthFunctionWithoutASingleValueArg");
			}
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06001AC4 RID: 6852 RVA: 0x0005A3C3 File Offset: 0x000585C3
		internal static string MetadataBinder_SpatialLengthFunctionWithOutLineStringArg
		{
			get
			{
				return TextRes.GetString("MetadataBinder_SpatialLengthFunctionWithOutLineStringArg");
			}
		}

		// Token: 0x06001AC5 RID: 6853 RVA: 0x0005A3D0 File Offset: 0x000585D0
		internal static string MetadataBinder_SpatialIntersectsFunctionWithInvalidArgs(object p0)
		{
			return TextRes.GetString("MetadataBinder_SpatialIntersectsFunctionWithInvalidArgs", new object[]
			{
				p0
			});
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06001AC6 RID: 6854 RVA: 0x0005A3F3 File Offset: 0x000585F3
		internal static string MetadataBinder_SpatialIntersectsFunctionWithoutASingleValueArg
		{
			get
			{
				return TextRes.GetString("MetadataBinder_SpatialIntersectsFunctionWithoutASingleValueArg");
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06001AC7 RID: 6855 RVA: 0x0005A3FF File Offset: 0x000585FF
		internal static string MetadataBinder_SpatialIntersectsFunctionWithInvalidArgTypes
		{
			get
			{
				return TextRes.GetString("MetadataBinder_SpatialIntersectsFunctionWithInvalidArgTypes");
			}
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06001AC8 RID: 6856 RVA: 0x0005A40B File Offset: 0x0005860B
		internal static string MetadataBinder_NonValidTypeArgument
		{
			get
			{
				return TextRes.GetString("MetadataBinder_NonValidTypeArgument");
			}
		}

		// Token: 0x06001AC9 RID: 6857 RVA: 0x0005A418 File Offset: 0x00058618
		internal static string MetadataBinder_OperatorNotSupportedInThisVersion(object p0)
		{
			return TextRes.GetString("MetadataBinder_OperatorNotSupportedInThisVersion", new object[]
			{
				p0
			});
		}

		// Token: 0x06001ACA RID: 6858 RVA: 0x0005A43C File Offset: 0x0005863C
		internal static string MetadataBinder_KeywordNotSupportedInThisRelease(object p0)
		{
			return TextRes.GetString("MetadataBinder_KeywordNotSupportedInThisRelease", new object[]
			{
				p0
			});
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06001ACB RID: 6859 RVA: 0x0005A45F File Offset: 0x0005865F
		internal static string MetadataBinder_CollectionOpenPropertiesNotSupportedInThisRelease
		{
			get
			{
				return TextRes.GetString("MetadataBinder_CollectionOpenPropertiesNotSupportedInThisRelease");
			}
		}

		// Token: 0x06001ACC RID: 6860 RVA: 0x0005A46C File Offset: 0x0005866C
		internal static string MetadataBinder_IllegalSegmentType(object p0)
		{
			return TextRes.GetString("MetadataBinder_IllegalSegmentType", new object[]
			{
				p0
			});
		}

		// Token: 0x06001ACD RID: 6861 RVA: 0x0005A490 File Offset: 0x00058690
		internal static string MetadataBinder_QueryOptionNotApplicable(object p0)
		{
			return TextRes.GetString("MetadataBinder_QueryOptionNotApplicable", new object[]
			{
				p0
			});
		}

		// Token: 0x06001ACE RID: 6862 RVA: 0x0005A4B4 File Offset: 0x000586B4
		internal static string FunctionCallBinder_CannotFindASuitableOverload(object p0, object p1)
		{
			return TextRes.GetString("FunctionCallBinder_CannotFindASuitableOverload", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001ACF RID: 6863 RVA: 0x0005A4DC File Offset: 0x000586DC
		internal static string FunctionCallBinder_NonSingleValueParent(object p0)
		{
			return TextRes.GetString("FunctionCallBinder_NonSingleValueParent", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AD0 RID: 6864 RVA: 0x0005A500 File Offset: 0x00058700
		internal static string FunctionCallBinder_FoundInvalidFunctionImports(object p0)
		{
			return TextRes.GetString("FunctionCallBinder_FoundInvalidFunctionImports", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AD1 RID: 6865 RVA: 0x0005A524 File Offset: 0x00058724
		internal static string FunctionCallBinder_BuiltInFunctionMustHaveHaveNullParent(object p0)
		{
			return TextRes.GetString("FunctionCallBinder_BuiltInFunctionMustHaveHaveNullParent", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AD2 RID: 6866 RVA: 0x0005A548 File Offset: 0x00058748
		internal static string FunctionCallBinder_CallingFunctionOnOpenProperty(object p0)
		{
			return TextRes.GetString("FunctionCallBinder_CallingFunctionOnOpenProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06001AD3 RID: 6867 RVA: 0x0005A56B File Offset: 0x0005876B
		internal static string FunctionCallParser_DuplicateParameterName
		{
			get
			{
				return TextRes.GetString("FunctionCallParser_DuplicateParameterName");
			}
		}

		// Token: 0x06001AD4 RID: 6868 RVA: 0x0005A578 File Offset: 0x00058778
		internal static string ODataUriParser_InvalidInlineCount(object p0)
		{
			return TextRes.GetString("ODataUriParser_InvalidInlineCount", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AD5 RID: 6869 RVA: 0x0005A59C File Offset: 0x0005879C
		internal static string CastBinder_ChildTypeIsNotEntity(object p0)
		{
			return TextRes.GetString("CastBinder_ChildTypeIsNotEntity", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AD6 RID: 6870 RVA: 0x0005A5C0 File Offset: 0x000587C0
		internal static string BatchReferenceSegment_InvalidContentID(object p0)
		{
			return TextRes.GetString("BatchReferenceSegment_InvalidContentID", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AD7 RID: 6871 RVA: 0x0005A5E4 File Offset: 0x000587E4
		internal static string SelectExpandBinder_UnknownPropertyType(object p0)
		{
			return TextRes.GetString("SelectExpandBinder_UnknownPropertyType", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AD8 RID: 6872 RVA: 0x0005A608 File Offset: 0x00058808
		internal static string SelectExpandBinder_CantFindProperty(object p0)
		{
			return TextRes.GetString("SelectExpandBinder_CantFindProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AD9 RID: 6873 RVA: 0x0005A62C File Offset: 0x0005882C
		internal static string SelectionItemBinder_NoExpandForSelectedProperty(object p0)
		{
			return TextRes.GetString("SelectionItemBinder_NoExpandForSelectedProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06001ADA RID: 6874 RVA: 0x0005A64F File Offset: 0x0005884F
		internal static string SelectionItemBinder_NonPathSelectToken
		{
			get
			{
				return TextRes.GetString("SelectionItemBinder_NonPathSelectToken");
			}
		}

		// Token: 0x06001ADB RID: 6875 RVA: 0x0005A65C File Offset: 0x0005885C
		internal static string SelectionItemBinder_NonEntityTypeSegment(object p0)
		{
			return TextRes.GetString("SelectionItemBinder_NonEntityTypeSegment", new object[]
			{
				p0
			});
		}

		// Token: 0x06001ADC RID: 6876 RVA: 0x0005A680 File Offset: 0x00058880
		internal static string SelectExpandPathBinder_FollowNonTypeSegment(object p0)
		{
			return TextRes.GetString("SelectExpandPathBinder_FollowNonTypeSegment", new object[]
			{
				p0
			});
		}

		// Token: 0x06001ADD RID: 6877 RVA: 0x0005A6A4 File Offset: 0x000588A4
		internal static string SelectPropertyVisitor_SystemTokenInSelect(object p0)
		{
			return TextRes.GetString("SelectPropertyVisitor_SystemTokenInSelect", new object[]
			{
				p0
			});
		}

		// Token: 0x06001ADE RID: 6878 RVA: 0x0005A6C8 File Offset: 0x000588C8
		internal static string SelectPropertyVisitor_InvalidSegmentInSelectPath(object p0)
		{
			return TextRes.GetString("SelectPropertyVisitor_InvalidSegmentInSelectPath", new object[]
			{
				p0
			});
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001ADF RID: 6879 RVA: 0x0005A6EB File Offset: 0x000588EB
		internal static string SelectPropertyVisitor_DisparateTypeSegmentsInSelectExpand
		{
			get
			{
				return TextRes.GetString("SelectPropertyVisitor_DisparateTypeSegmentsInSelectExpand");
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06001AE0 RID: 6880 RVA: 0x0005A6F7 File Offset: 0x000588F7
		internal static string SelectExpandClause_CannotDeleteFromAllSelection
		{
			get
			{
				return TextRes.GetString("SelectExpandClause_CannotDeleteFromAllSelection");
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06001AE1 RID: 6881 RVA: 0x0005A703 File Offset: 0x00058903
		internal static string SegmentFactory_LinksSegmentNotFollowedByNavProp
		{
			get
			{
				return TextRes.GetString("SegmentFactory_LinksSegmentNotFollowedByNavProp");
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06001AE2 RID: 6882 RVA: 0x0005A70F File Offset: 0x0005890F
		internal static string ExpandItemBinder_TraversingANonNormalizedTree
		{
			get
			{
				return TextRes.GetString("ExpandItemBinder_TraversingANonNormalizedTree");
			}
		}

		// Token: 0x06001AE3 RID: 6883 RVA: 0x0005A71C File Offset: 0x0005891C
		internal static string ExpandItemBinder_CannotFindType(object p0)
		{
			return TextRes.GetString("ExpandItemBinder_CannotFindType", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AE4 RID: 6884 RVA: 0x0005A740 File Offset: 0x00058940
		internal static string ExpandItemBinder_PropertyIsNotANavigationProperty(object p0, object p1)
		{
			return TextRes.GetString("ExpandItemBinder_PropertyIsNotANavigationProperty", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06001AE5 RID: 6885 RVA: 0x0005A767 File Offset: 0x00058967
		internal static string ExpandItemBinder_TypeSegmentNotFollowedByPath
		{
			get
			{
				return TextRes.GetString("ExpandItemBinder_TypeSegmentNotFollowedByPath");
			}
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06001AE6 RID: 6886 RVA: 0x0005A773 File Offset: 0x00058973
		internal static string ExpandItemBinder_PathTooDeep
		{
			get
			{
				return TextRes.GetString("ExpandItemBinder_PathTooDeep");
			}
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06001AE7 RID: 6887 RVA: 0x0005A77F File Offset: 0x0005897F
		internal static string Nodes_CollectionNavigationNode_MustHaveSingleMultiplicity
		{
			get
			{
				return TextRes.GetString("Nodes_CollectionNavigationNode_MustHaveSingleMultiplicity");
			}
		}

		// Token: 0x06001AE8 RID: 6888 RVA: 0x0005A78C File Offset: 0x0005898C
		internal static string Nodes_NonentityParameterQueryNodeWithEntityType(object p0)
		{
			return TextRes.GetString("Nodes_NonentityParameterQueryNodeWithEntityType", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AE9 RID: 6889 RVA: 0x0005A7B0 File Offset: 0x000589B0
		internal static string Nodes_EntityCollectionServiceOperationRequiresEntityReturnType(object p0)
		{
			return TextRes.GetString("Nodes_EntityCollectionServiceOperationRequiresEntityReturnType", new object[]
			{
				p0
			});
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06001AEA RID: 6890 RVA: 0x0005A7D3 File Offset: 0x000589D3
		internal static string Nodes_CollectionNavigationNode_MustHaveManyMultiplicity
		{
			get
			{
				return TextRes.GetString("Nodes_CollectionNavigationNode_MustHaveManyMultiplicity");
			}
		}

		// Token: 0x06001AEB RID: 6891 RVA: 0x0005A7E0 File Offset: 0x000589E0
		internal static string Nodes_PropertyAccessShouldBeNonEntityProperty(object p0)
		{
			return TextRes.GetString("Nodes_PropertyAccessShouldBeNonEntityProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AEC RID: 6892 RVA: 0x0005A804 File Offset: 0x00058A04
		internal static string Nodes_PropertyAccessTypeShouldNotBeCollection(object p0)
		{
			return TextRes.GetString("Nodes_PropertyAccessTypeShouldNotBeCollection", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AED RID: 6893 RVA: 0x0005A828 File Offset: 0x00058A28
		internal static string Nodes_PropertyAccessTypeMustBeCollection(object p0)
		{
			return TextRes.GetString("Nodes_PropertyAccessTypeMustBeCollection", new object[]
			{
				p0
			});
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06001AEE RID: 6894 RVA: 0x0005A84B File Offset: 0x00058A4B
		internal static string Nodes_NonStaticEntitySetExpressionsAreNotSupportedInThisRelease
		{
			get
			{
				return TextRes.GetString("Nodes_NonStaticEntitySetExpressionsAreNotSupportedInThisRelease");
			}
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06001AEF RID: 6895 RVA: 0x0005A857 File Offset: 0x00058A57
		internal static string Nodes_CollectionFunctionCallNode_ItemTypeMustBePrimitiveOrComplex
		{
			get
			{
				return TextRes.GetString("Nodes_CollectionFunctionCallNode_ItemTypeMustBePrimitiveOrComplex");
			}
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06001AF0 RID: 6896 RVA: 0x0005A863 File Offset: 0x00058A63
		internal static string Nodes_EntityCollectionFunctionCallNode_ItemTypeMustBeAnEntity
		{
			get
			{
				return TextRes.GetString("Nodes_EntityCollectionFunctionCallNode_ItemTypeMustBeAnEntity");
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06001AF1 RID: 6897 RVA: 0x0005A86F File Offset: 0x00058A6F
		internal static string ExpandTreeNormalizer_CallAddTermsOnUnexpandedTerms
		{
			get
			{
				return TextRes.GetString("ExpandTreeNormalizer_CallAddTermsOnUnexpandedTerms");
			}
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06001AF2 RID: 6898 RVA: 0x0005A87B File Offset: 0x00058A7B
		internal static string ExpandTreeNormalizer_NonPathInPropertyChain
		{
			get
			{
				return TextRes.GetString("ExpandTreeNormalizer_NonPathInPropertyChain");
			}
		}

		// Token: 0x06001AF3 RID: 6899 RVA: 0x0005A888 File Offset: 0x00058A88
		internal static string UriSelectParser_TermIsNotValid(object p0)
		{
			return TextRes.GetString("UriSelectParser_TermIsNotValid", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x0005A8AC File Offset: 0x00058AAC
		internal static string UriSelectParser_FunctionsAreNotAllowed(object p0)
		{
			return TextRes.GetString("UriSelectParser_FunctionsAreNotAllowed", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AF5 RID: 6901 RVA: 0x0005A8D0 File Offset: 0x00058AD0
		internal static string UriSelectParser_InvalidTopOption(object p0)
		{
			return TextRes.GetString("UriSelectParser_InvalidTopOption", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AF6 RID: 6902 RVA: 0x0005A8F4 File Offset: 0x00058AF4
		internal static string UriSelectParser_InvalidSkipOption(object p0)
		{
			return TextRes.GetString("UriSelectParser_InvalidSkipOption", new object[]
			{
				p0
			});
		}

		// Token: 0x06001AF7 RID: 6903 RVA: 0x0005A918 File Offset: 0x00058B18
		internal static string UriSelectParser_SystemTokenInSelectExpand(object p0, object p1)
		{
			return TextRes.GetString("UriSelectParser_SystemTokenInSelectExpand", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06001AF8 RID: 6904 RVA: 0x0005A93F File Offset: 0x00058B3F
		internal static string UriParser_NeedServiceRootForThisOverload
		{
			get
			{
				return TextRes.GetString("UriParser_NeedServiceRootForThisOverload");
			}
		}

		// Token: 0x06001AF9 RID: 6905 RVA: 0x0005A94C File Offset: 0x00058B4C
		internal static string UriParser_UriMustBeAbsolute(object p0)
		{
			return TextRes.GetString("UriParser_UriMustBeAbsolute", new object[]
			{
				p0
			});
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06001AFA RID: 6906 RVA: 0x0005A96F File Offset: 0x00058B6F
		internal static string UriParser_NegativeLimit
		{
			get
			{
				return TextRes.GetString("UriParser_NegativeLimit");
			}
		}

		// Token: 0x06001AFB RID: 6907 RVA: 0x0005A97C File Offset: 0x00058B7C
		internal static string UriParser_ExpandCountExceeded(object p0, object p1)
		{
			return TextRes.GetString("UriParser_ExpandCountExceeded", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001AFC RID: 6908 RVA: 0x0005A9A4 File Offset: 0x00058BA4
		internal static string UriParser_ExpandDepthExceeded(object p0, object p1)
		{
			return TextRes.GetString("UriParser_ExpandDepthExceeded", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001AFD RID: 6909 RVA: 0x0005A9CC File Offset: 0x00058BCC
		internal static string PathParser_ServiceOperationWithoutResultKindAttribute(object p0)
		{
			return TextRes.GetString("PathParser_ServiceOperationWithoutResultKindAttribute", new object[]
			{
				p0
			});
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06001AFE RID: 6910 RVA: 0x0005A9EF File Offset: 0x00058BEF
		internal static string PathParser_FunctionsAreNotSupported
		{
			get
			{
				return TextRes.GetString("PathParser_FunctionsAreNotSupported");
			}
		}

		// Token: 0x06001AFF RID: 6911 RVA: 0x0005A9FC File Offset: 0x00058BFC
		internal static string PathParser_ServiceOperationsWithSameName(object p0)
		{
			return TextRes.GetString("PathParser_ServiceOperationsWithSameName", new object[]
			{
				p0
			});
		}

		// Token: 0x06001B00 RID: 6912 RVA: 0x0005AA20 File Offset: 0x00058C20
		internal static string PathParser_LinksNotSupported(object p0)
		{
			return TextRes.GetString("PathParser_LinksNotSupported", new object[]
			{
				p0
			});
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06001B01 RID: 6913 RVA: 0x0005AA43 File Offset: 0x00058C43
		internal static string PathParser_CannotUseValueOnCollection
		{
			get
			{
				return TextRes.GetString("PathParser_CannotUseValueOnCollection");
			}
		}

		// Token: 0x06001B02 RID: 6914 RVA: 0x0005AA50 File Offset: 0x00058C50
		internal static string PathParser_TypeMustBeRelatedToSet(object p0, object p1, object p2)
		{
			return TextRes.GetString("PathParser_TypeMustBeRelatedToSet", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06001B03 RID: 6915 RVA: 0x0005AA7B File Offset: 0x00058C7B
		internal static string ODataFeed_MustNotContainBothNextPageLinkAndDeltaLink
		{
			get
			{
				return TextRes.GetString("ODataFeed_MustNotContainBothNextPageLinkAndDeltaLink");
			}
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06001B04 RID: 6916 RVA: 0x0005AA87 File Offset: 0x00058C87
		internal static string ODataExpandPath_OnlyLastSegmentMustBeNavigationProperty
		{
			get
			{
				return TextRes.GetString("ODataExpandPath_OnlyLastSegmentMustBeNavigationProperty");
			}
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x0005AA94 File Offset: 0x00058C94
		internal static string ODataExpandPath_InvalidExpandPathSegment(object p0)
		{
			return TextRes.GetString("ODataExpandPath_InvalidExpandPathSegment", new object[]
			{
				p0
			});
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06001B06 RID: 6918 RVA: 0x0005AAB7 File Offset: 0x00058CB7
		internal static string ODataSelectPath_CannotEndInTypeSegment
		{
			get
			{
				return TextRes.GetString("ODataSelectPath_CannotEndInTypeSegment");
			}
		}

		// Token: 0x06001B07 RID: 6919 RVA: 0x0005AAC4 File Offset: 0x00058CC4
		internal static string ODataSelectPath_InvalidSelectPathSegmentType(object p0)
		{
			return TextRes.GetString("ODataSelectPath_InvalidSelectPathSegmentType", new object[]
			{
				p0
			});
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06001B08 RID: 6920 RVA: 0x0005AAE7 File Offset: 0x00058CE7
		internal static string ODataSelectPath_OperationSegmentCanOnlyBeLastSegment
		{
			get
			{
				return TextRes.GetString("ODataSelectPath_OperationSegmentCanOnlyBeLastSegment");
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06001B09 RID: 6921 RVA: 0x0005AAF3 File Offset: 0x00058CF3
		internal static string ODataSelectPath_NavPropSegmentCanOnlyBeLastSegment
		{
			get
			{
				return TextRes.GetString("ODataSelectPath_NavPropSegmentCanOnlyBeLastSegment");
			}
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x0005AB00 File Offset: 0x00058D00
		internal static string RequestUriProcessor_EntitySetNotSpecified(object p0)
		{
			return TextRes.GetString("RequestUriProcessor_EntitySetNotSpecified", new object[]
			{
				p0
			});
		}

		// Token: 0x06001B0B RID: 6923 RVA: 0x0005AB24 File Offset: 0x00058D24
		internal static string RequestUriProcessor_TargetEntitySetNotFound(object p0)
		{
			return TextRes.GetString("RequestUriProcessor_TargetEntitySetNotFound", new object[]
			{
				p0
			});
		}

		// Token: 0x06001B0C RID: 6924 RVA: 0x0005AB48 File Offset: 0x00058D48
		internal static string RequestUriProcessor_FoundInvalidFunctionImport(object p0)
		{
			return TextRes.GetString("RequestUriProcessor_FoundInvalidFunctionImport", new object[]
			{
				p0
			});
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06001B0D RID: 6925 RVA: 0x0005AB6B File Offset: 0x00058D6B
		internal static string OperationSegment_ReturnTypeForMultipleOverloads
		{
			get
			{
				return TextRes.GetString("OperationSegment_ReturnTypeForMultipleOverloads");
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06001B0E RID: 6926 RVA: 0x0005AB77 File Offset: 0x00058D77
		internal static string OperationSegment_CannotReturnNull
		{
			get
			{
				return TextRes.GetString("OperationSegment_CannotReturnNull");
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06001B0F RID: 6927 RVA: 0x0005AB83 File Offset: 0x00058D83
		internal static string SingleValueFunctionCallNode_FunctionImportsWithLegacyConstructor
		{
			get
			{
				return TextRes.GetString("SingleValueFunctionCallNode_FunctionImportsWithLegacyConstructor");
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06001B10 RID: 6928 RVA: 0x0005AB8F File Offset: 0x00058D8F
		internal static string SingleEntityFunctionCallNode_CallFunctionImportsUsingLegacyConstructor
		{
			get
			{
				return TextRes.GetString("SingleEntityFunctionCallNode_CallFunctionImportsUsingLegacyConstructor");
			}
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06001B11 RID: 6929 RVA: 0x0005AB9B File Offset: 0x00058D9B
		internal static string SegmentArgumentParser_TryConvertValuesForNamedValues
		{
			get
			{
				return TextRes.GetString("SegmentArgumentParser_TryConvertValuesForNamedValues");
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06001B12 RID: 6930 RVA: 0x0005ABA7 File Offset: 0x00058DA7
		internal static string SegmentArgumentParser_TryConvertValuesToNonPrimitive
		{
			get
			{
				return TextRes.GetString("SegmentArgumentParser_TryConvertValuesToNonPrimitive");
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06001B13 RID: 6931 RVA: 0x0005ABB3 File Offset: 0x00058DB3
		internal static string SegmentArgumentParser_TryConvertValuesForPositionalValues
		{
			get
			{
				return TextRes.GetString("SegmentArgumentParser_TryConvertValuesForPositionalValues");
			}
		}

		// Token: 0x06001B14 RID: 6932 RVA: 0x0005ABC0 File Offset: 0x00058DC0
		internal static string FunctionOverloadResolver_NoSingleMatchFound(object p0, object p1)
		{
			return TextRes.GetString("FunctionOverloadResolver_NoSingleMatchFound", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001B15 RID: 6933 RVA: 0x0005ABE8 File Offset: 0x00058DE8
		internal static string FunctionOverloadResolver_MultipleActionOverloads(object p0)
		{
			return TextRes.GetString("FunctionOverloadResolver_MultipleActionOverloads", new object[]
			{
				p0
			});
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06001B16 RID: 6934 RVA: 0x0005AC0B File Offset: 0x00058E0B
		internal static string RequestUriProcessor_EmptySegmentInRequestUrl
		{
			get
			{
				return TextRes.GetString("RequestUriProcessor_EmptySegmentInRequestUrl");
			}
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06001B17 RID: 6935 RVA: 0x0005AC17 File Offset: 0x00058E17
		internal static string RequestUriProcessor_SyntaxError
		{
			get
			{
				return TextRes.GetString("RequestUriProcessor_SyntaxError");
			}
		}

		// Token: 0x06001B18 RID: 6936 RVA: 0x0005AC24 File Offset: 0x00058E24
		internal static string RequestUriProcessor_CannotSpecifyAfterPostLinkSegment(object p0, object p1)
		{
			return TextRes.GetString("RequestUriProcessor_CannotSpecifyAfterPostLinkSegment", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06001B19 RID: 6937 RVA: 0x0005AC4B File Offset: 0x00058E4B
		internal static string RequestUriProcessor_CountOnRoot
		{
			get
			{
				return TextRes.GetString("RequestUriProcessor_CountOnRoot");
			}
		}

		// Token: 0x06001B1A RID: 6938 RVA: 0x0005AC58 File Offset: 0x00058E58
		internal static string RequestUriProcessor_MustBeLeafSegment(object p0)
		{
			return TextRes.GetString("RequestUriProcessor_MustBeLeafSegment", new object[]
			{
				p0
			});
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x0005AC7C File Offset: 0x00058E7C
		internal static string RequestUriProcessor_LinkSegmentMustBeFollowedByEntitySegment(object p0, object p1)
		{
			return TextRes.GetString("RequestUriProcessor_LinkSegmentMustBeFollowedByEntitySegment", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x0005ACA4 File Offset: 0x00058EA4
		internal static string RequestUriProcessor_MissingSegmentAfterLink(object p0)
		{
			return TextRes.GetString("RequestUriProcessor_MissingSegmentAfterLink", new object[]
			{
				p0
			});
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x0005ACC8 File Offset: 0x00058EC8
		internal static string RequestUriProcessor_CountNotSupported(object p0)
		{
			return TextRes.GetString("RequestUriProcessor_CountNotSupported", new object[]
			{
				p0
			});
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x0005ACEC File Offset: 0x00058EEC
		internal static string RequestUriProcessor_CannotQuerySingletons(object p0, object p1)
		{
			return TextRes.GetString("RequestUriProcessor_CannotQuerySingletons", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001B1F RID: 6943 RVA: 0x0005AD14 File Offset: 0x00058F14
		internal static string RequestUriProcessor_CannotQueryCollections(object p0)
		{
			return TextRes.GetString("RequestUriProcessor_CannotQueryCollections", new object[]
			{
				p0
			});
		}

		// Token: 0x06001B20 RID: 6944 RVA: 0x0005AD38 File Offset: 0x00058F38
		internal static string RequestUriProcessor_SegmentDoesNotSupportKeyPredicates(object p0)
		{
			return TextRes.GetString("RequestUriProcessor_SegmentDoesNotSupportKeyPredicates", new object[]
			{
				p0
			});
		}

		// Token: 0x06001B21 RID: 6945 RVA: 0x0005AD5C File Offset: 0x00058F5C
		internal static string RequestUriProcessor_ValueSegmentAfterScalarPropertySegment(object p0, object p1)
		{
			return TextRes.GetString("RequestUriProcessor_ValueSegmentAfterScalarPropertySegment", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x0005AD84 File Offset: 0x00058F84
		internal static string RequestUriProcessor_InvalidTypeIdentifier_UnrelatedType(object p0, object p1)
		{
			return TextRes.GetString("RequestUriProcessor_InvalidTypeIdentifier_UnrelatedType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x0005ADAC File Offset: 0x00058FAC
		internal static string ResourceType_ComplexTypeCannotBeOpen(object p0)
		{
			return TextRes.GetString("ResourceType_ComplexTypeCannotBeOpen", new object[]
			{
				p0
			});
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06001B24 RID: 6948 RVA: 0x0005ADCF File Offset: 0x00058FCF
		internal static string BadRequest_ValuesCannotBeReturnedForSpatialTypes
		{
			get
			{
				return TextRes.GetString("BadRequest_ValuesCannotBeReturnedForSpatialTypes");
			}
		}

		// Token: 0x06001B25 RID: 6949 RVA: 0x0005ADDC File Offset: 0x00058FDC
		internal static string OpenNavigationPropertiesNotSupportedOnOpenTypes(object p0)
		{
			return TextRes.GetString("OpenNavigationPropertiesNotSupportedOnOpenTypes", new object[]
			{
				p0
			});
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06001B26 RID: 6950 RVA: 0x0005ADFF File Offset: 0x00058FFF
		internal static string BadRequest_ResourceCanBeCrossReferencedOnlyForBindOperation
		{
			get
			{
				return TextRes.GetString("BadRequest_ResourceCanBeCrossReferencedOnlyForBindOperation");
			}
		}

		// Token: 0x06001B27 RID: 6951 RVA: 0x0005AE0C File Offset: 0x0005900C
		internal static string DataServiceConfiguration_ResponseVersionIsBiggerThanProtocolVersion(object p0, object p1)
		{
			return TextRes.GetString("DataServiceConfiguration_ResponseVersionIsBiggerThanProtocolVersion", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001B28 RID: 6952 RVA: 0x0005AE34 File Offset: 0x00059034
		internal static string BadRequest_KeyCountMismatch(object p0)
		{
			return TextRes.GetString("BadRequest_KeyCountMismatch", new object[]
			{
				p0
			});
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06001B29 RID: 6953 RVA: 0x0005AE57 File Offset: 0x00059057
		internal static string RequestUriProcessor_KeysMustBeNamed
		{
			get
			{
				return TextRes.GetString("RequestUriProcessor_KeysMustBeNamed");
			}
		}

		// Token: 0x06001B2A RID: 6954 RVA: 0x0005AE64 File Offset: 0x00059064
		internal static string RequestUriProcessor_ResourceNotFound(object p0)
		{
			return TextRes.GetString("RequestUriProcessor_ResourceNotFound", new object[]
			{
				p0
			});
		}

		// Token: 0x06001B2B RID: 6955 RVA: 0x0005AE88 File Offset: 0x00059088
		internal static string RequestUriProcessor_BatchedActionOnEntityCreatedInSameChangeset(object p0)
		{
			return TextRes.GetString("RequestUriProcessor_BatchedActionOnEntityCreatedInSameChangeset", new object[]
			{
				p0
			});
		}

		// Token: 0x06001B2C RID: 6956 RVA: 0x0005AEAC File Offset: 0x000590AC
		internal static string RequestUriProcessor_IEnumerableServiceOperationsCannotBeFurtherComposed(object p0)
		{
			return TextRes.GetString("RequestUriProcessor_IEnumerableServiceOperationsCannotBeFurtherComposed", new object[]
			{
				p0
			});
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06001B2D RID: 6957 RVA: 0x0005AECF File Offset: 0x000590CF
		internal static string RequestUriProcessor_Forbidden
		{
			get
			{
				return TextRes.GetString("RequestUriProcessor_Forbidden");
			}
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06001B2E RID: 6958 RVA: 0x0005AEDB File Offset: 0x000590DB
		internal static string RequestUriProcessor_OperationSegmentBoundToANonEntityType
		{
			get
			{
				return TextRes.GetString("RequestUriProcessor_OperationSegmentBoundToANonEntityType");
			}
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x0005AEE8 File Offset: 0x000590E8
		internal static string General_InternalError(object p0)
		{
			return TextRes.GetString("General_InternalError", new object[]
			{
				p0
			});
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x0005AF0C File Offset: 0x0005910C
		internal static string ExceptionUtils_CheckIntegerNotNegative(object p0)
		{
			return TextRes.GetString("ExceptionUtils_CheckIntegerNotNegative", new object[]
			{
				p0
			});
		}

		// Token: 0x06001B31 RID: 6961 RVA: 0x0005AF30 File Offset: 0x00059130
		internal static string ExceptionUtils_CheckIntegerPositive(object p0)
		{
			return TextRes.GetString("ExceptionUtils_CheckIntegerPositive", new object[]
			{
				p0
			});
		}

		// Token: 0x06001B32 RID: 6962 RVA: 0x0005AF54 File Offset: 0x00059154
		internal static string ExceptionUtils_CheckLongPositive(object p0)
		{
			return TextRes.GetString("ExceptionUtils_CheckLongPositive", new object[]
			{
				p0
			});
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06001B33 RID: 6963 RVA: 0x0005AF77 File Offset: 0x00059177
		internal static string ExceptionUtils_ArgumentStringNullOrEmpty
		{
			get
			{
				return TextRes.GetString("ExceptionUtils_ArgumentStringNullOrEmpty");
			}
		}

		// Token: 0x06001B34 RID: 6964 RVA: 0x0005AF84 File Offset: 0x00059184
		internal static string ExpressionToken_IdentifierExpected(object p0)
		{
			return TextRes.GetString("ExpressionToken_IdentifierExpected", new object[]
			{
				p0
			});
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x0005AFA8 File Offset: 0x000591A8
		internal static string ExpressionLexer_UnterminatedStringLiteral(object p0, object p1)
		{
			return TextRes.GetString("ExpressionLexer_UnterminatedStringLiteral", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x0005AFD0 File Offset: 0x000591D0
		internal static string ExpressionLexer_InvalidCharacter(object p0, object p1, object p2)
		{
			return TextRes.GetString("ExpressionLexer_InvalidCharacter", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x0005AFFC File Offset: 0x000591FC
		internal static string ExpressionLexer_SyntaxError(object p0, object p1)
		{
			return TextRes.GetString("ExpressionLexer_SyntaxError", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001B38 RID: 6968 RVA: 0x0005B024 File Offset: 0x00059224
		internal static string ExpressionLexer_UnterminatedLiteral(object p0, object p1)
		{
			return TextRes.GetString("ExpressionLexer_UnterminatedLiteral", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001B39 RID: 6969 RVA: 0x0005B04C File Offset: 0x0005924C
		internal static string ExpressionLexer_DigitExpected(object p0, object p1)
		{
			return TextRes.GetString("ExpressionLexer_DigitExpected", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06001B3A RID: 6970 RVA: 0x0005B073 File Offset: 0x00059273
		internal static string ExpressionLexer_UnbalancedBracketExpression
		{
			get
			{
				return TextRes.GetString("ExpressionLexer_UnbalancedBracketExpression");
			}
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x0005B080 File Offset: 0x00059280
		internal static string UriQueryExpressionParser_UnrecognizedLiteral(object p0, object p1, object p2, object p3)
		{
			return TextRes.GetString("UriQueryExpressionParser_UnrecognizedLiteral", new object[]
			{
				p0,
				p1,
				p2,
				p3
			});
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x0005B0B0 File Offset: 0x000592B0
		internal static string JsonReader_UnexpectedComma(object p0)
		{
			return TextRes.GetString("JsonReader_UnexpectedComma", new object[]
			{
				p0
			});
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06001B3D RID: 6973 RVA: 0x0005B0D3 File Offset: 0x000592D3
		internal static string JsonReader_MultipleTopLevelValues
		{
			get
			{
				return TextRes.GetString("JsonReader_MultipleTopLevelValues");
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06001B3E RID: 6974 RVA: 0x0005B0DF File Offset: 0x000592DF
		internal static string JsonReader_EndOfInputWithOpenScope
		{
			get
			{
				return TextRes.GetString("JsonReader_EndOfInputWithOpenScope");
			}
		}

		// Token: 0x06001B3F RID: 6975 RVA: 0x0005B0EC File Offset: 0x000592EC
		internal static string JsonReader_UnexpectedToken(object p0)
		{
			return TextRes.GetString("JsonReader_UnexpectedToken", new object[]
			{
				p0
			});
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06001B40 RID: 6976 RVA: 0x0005B10F File Offset: 0x0005930F
		internal static string JsonReader_UnrecognizedToken
		{
			get
			{
				return TextRes.GetString("JsonReader_UnrecognizedToken");
			}
		}

		// Token: 0x06001B41 RID: 6977 RVA: 0x0005B11C File Offset: 0x0005931C
		internal static string JsonReader_MissingColon(object p0)
		{
			return TextRes.GetString("JsonReader_MissingColon", new object[]
			{
				p0
			});
		}

		// Token: 0x06001B42 RID: 6978 RVA: 0x0005B140 File Offset: 0x00059340
		internal static string JsonReader_UnrecognizedEscapeSequence(object p0)
		{
			return TextRes.GetString("JsonReader_UnrecognizedEscapeSequence", new object[]
			{
				p0
			});
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06001B43 RID: 6979 RVA: 0x0005B163 File Offset: 0x00059363
		internal static string JsonReader_UnexpectedEndOfString
		{
			get
			{
				return TextRes.GetString("JsonReader_UnexpectedEndOfString");
			}
		}

		// Token: 0x06001B44 RID: 6980 RVA: 0x0005B170 File Offset: 0x00059370
		internal static string JsonReader_InvalidNumberFormat(object p0)
		{
			return TextRes.GetString("JsonReader_InvalidNumberFormat", new object[]
			{
				p0
			});
		}

		// Token: 0x06001B45 RID: 6981 RVA: 0x0005B194 File Offset: 0x00059394
		internal static string JsonReader_MissingComma(object p0)
		{
			return TextRes.GetString("JsonReader_MissingComma", new object[]
			{
				p0
			});
		}

		// Token: 0x06001B46 RID: 6982 RVA: 0x0005B1B8 File Offset: 0x000593B8
		internal static string JsonReader_InvalidPropertyNameOrUnexpectedComma(object p0)
		{
			return TextRes.GetString("JsonReader_InvalidPropertyNameOrUnexpectedComma", new object[]
			{
				p0
			});
		}

		// Token: 0x06001B47 RID: 6983 RVA: 0x0005B1DC File Offset: 0x000593DC
		internal static string JsonReaderExtensions_UnexpectedNodeDetected(object p0, object p1)
		{
			return TextRes.GetString("JsonReaderExtensions_UnexpectedNodeDetected", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001B48 RID: 6984 RVA: 0x0005B204 File Offset: 0x00059404
		internal static string JsonReaderExtensions_CannotReadPropertyValueAsString(object p0, object p1)
		{
			return TextRes.GetString("JsonReaderExtensions_CannotReadPropertyValueAsString", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06001B49 RID: 6985 RVA: 0x0005B22C File Offset: 0x0005942C
		internal static string JsonReaderExtensions_CannotReadValueAsString(object p0)
		{
			return TextRes.GetString("JsonReaderExtensions_CannotReadValueAsString", new object[]
			{
				p0
			});
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x0005B250 File Offset: 0x00059450
		internal static string JsonReaderExtensions_CannotReadValueAsDouble(object p0)
		{
			return TextRes.GetString("JsonReaderExtensions_CannotReadValueAsDouble", new object[]
			{
				p0
			});
		}
	}
}
