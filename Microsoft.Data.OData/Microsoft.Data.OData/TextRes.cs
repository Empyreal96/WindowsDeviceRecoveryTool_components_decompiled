using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace Microsoft.Data.OData
{
	// Token: 0x020002B1 RID: 689
	internal sealed class TextRes
	{
		// Token: 0x0600178A RID: 6026 RVA: 0x00054284 File Offset: 0x00052484
		internal TextRes()
		{
			this.resources = new ResourceManager("Microsoft.Data.OData", base.GetType().Assembly);
		}

		// Token: 0x0600178B RID: 6027 RVA: 0x000542A8 File Offset: 0x000524A8
		private static TextRes GetLoader()
		{
			if (TextRes.loader == null)
			{
				TextRes value = new TextRes();
				Interlocked.CompareExchange<TextRes>(ref TextRes.loader, value, null);
			}
			return TextRes.loader;
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x0600178C RID: 6028 RVA: 0x000542D4 File Offset: 0x000524D4
		private static CultureInfo Culture
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x0600178D RID: 6029 RVA: 0x000542D7 File Offset: 0x000524D7
		public static ResourceManager Resources
		{
			get
			{
				return TextRes.GetLoader().resources;
			}
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x000542E4 File Offset: 0x000524E4
		public static string GetString(string name, params object[] args)
		{
			TextRes textRes = TextRes.GetLoader();
			if (textRes == null)
			{
				return null;
			}
			string @string = textRes.resources.GetString(name, TextRes.Culture);
			if (args != null && args.Length > 0)
			{
				for (int i = 0; i < args.Length; i++)
				{
					string text = args[i] as string;
					if (text != null && text.Length > 1024)
					{
						args[i] = text.Substring(0, 1021) + "...";
					}
				}
				return string.Format(CultureInfo.CurrentCulture, @string, args);
			}
			return @string;
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x00054368 File Offset: 0x00052568
		public static string GetString(string name)
		{
			TextRes textRes = TextRes.GetLoader();
			if (textRes == null)
			{
				return null;
			}
			return textRes.resources.GetString(name, TextRes.Culture);
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x00054391 File Offset: 0x00052591
		public static string GetString(string name, out bool usedFallback)
		{
			usedFallback = false;
			return TextRes.GetString(name);
		}

		// Token: 0x06001791 RID: 6033 RVA: 0x0005439C File Offset: 0x0005259C
		public static object GetObject(string name)
		{
			TextRes textRes = TextRes.GetLoader();
			if (textRes == null)
			{
				return null;
			}
			return textRes.resources.GetObject(name, TextRes.Culture);
		}

		// Token: 0x040009AE RID: 2478
		internal const string ExceptionUtils_ArgumentStringEmpty = "ExceptionUtils_ArgumentStringEmpty";

		// Token: 0x040009AF RID: 2479
		internal const string ODataRequestMessage_AsyncNotAvailable = "ODataRequestMessage_AsyncNotAvailable";

		// Token: 0x040009B0 RID: 2480
		internal const string ODataRequestMessage_StreamTaskIsNull = "ODataRequestMessage_StreamTaskIsNull";

		// Token: 0x040009B1 RID: 2481
		internal const string ODataRequestMessage_MessageStreamIsNull = "ODataRequestMessage_MessageStreamIsNull";

		// Token: 0x040009B2 RID: 2482
		internal const string ODataResponseMessage_AsyncNotAvailable = "ODataResponseMessage_AsyncNotAvailable";

		// Token: 0x040009B3 RID: 2483
		internal const string ODataResponseMessage_StreamTaskIsNull = "ODataResponseMessage_StreamTaskIsNull";

		// Token: 0x040009B4 RID: 2484
		internal const string ODataResponseMessage_MessageStreamIsNull = "ODataResponseMessage_MessageStreamIsNull";

		// Token: 0x040009B5 RID: 2485
		internal const string AsyncBufferedStream_WriterDisposedWithoutFlush = "AsyncBufferedStream_WriterDisposedWithoutFlush";

		// Token: 0x040009B6 RID: 2486
		internal const string ODataOutputContext_UnsupportedPayloadKindForFormat = "ODataOutputContext_UnsupportedPayloadKindForFormat";

		// Token: 0x040009B7 RID: 2487
		internal const string ODataOutputContext_CustomInstanceAnnotationsNotSupportedForFormat = "ODataOutputContext_CustomInstanceAnnotationsNotSupportedForFormat";

		// Token: 0x040009B8 RID: 2488
		internal const string ODataInputContext_UnsupportedPayloadKindForFormat = "ODataInputContext_UnsupportedPayloadKindForFormat";

		// Token: 0x040009B9 RID: 2489
		internal const string ODataJsonLightSerializer_RelativeUriUsedWithoutMetadataDocumentUriOrMetadata = "ODataJsonLightSerializer_RelativeUriUsedWithoutMetadataDocumentUriOrMetadata";

		// Token: 0x040009BA RID: 2490
		internal const string ODataWriter_RelativeUriUsedWithoutBaseUriSpecified = "ODataWriter_RelativeUriUsedWithoutBaseUriSpecified";

		// Token: 0x040009BB RID: 2491
		internal const string ODataWriter_StreamPropertiesMustBePropertiesOfODataEntry = "ODataWriter_StreamPropertiesMustBePropertiesOfODataEntry";

		// Token: 0x040009BC RID: 2492
		internal const string ODataWriterCore_InvalidStateTransition = "ODataWriterCore_InvalidStateTransition";

		// Token: 0x040009BD RID: 2493
		internal const string ODataWriterCore_InvalidTransitionFromStart = "ODataWriterCore_InvalidTransitionFromStart";

		// Token: 0x040009BE RID: 2494
		internal const string ODataWriterCore_InvalidTransitionFromEntry = "ODataWriterCore_InvalidTransitionFromEntry";

		// Token: 0x040009BF RID: 2495
		internal const string ODataWriterCore_InvalidTransitionFromNullEntry = "ODataWriterCore_InvalidTransitionFromNullEntry";

		// Token: 0x040009C0 RID: 2496
		internal const string ODataWriterCore_InvalidTransitionFromFeed = "ODataWriterCore_InvalidTransitionFromFeed";

		// Token: 0x040009C1 RID: 2497
		internal const string ODataWriterCore_InvalidTransitionFromExpandedLink = "ODataWriterCore_InvalidTransitionFromExpandedLink";

		// Token: 0x040009C2 RID: 2498
		internal const string ODataWriterCore_InvalidTransitionFromCompleted = "ODataWriterCore_InvalidTransitionFromCompleted";

		// Token: 0x040009C3 RID: 2499
		internal const string ODataWriterCore_InvalidTransitionFromError = "ODataWriterCore_InvalidTransitionFromError";

		// Token: 0x040009C4 RID: 2500
		internal const string ODataWriterCore_WriteEndCalledInInvalidState = "ODataWriterCore_WriteEndCalledInInvalidState";

		// Token: 0x040009C5 RID: 2501
		internal const string ODataWriterCore_OnlyTopLevelFeedsSupportInlineCount = "ODataWriterCore_OnlyTopLevelFeedsSupportInlineCount";

		// Token: 0x040009C6 RID: 2502
		internal const string ODataWriterCore_InlineCountInRequest = "ODataWriterCore_InlineCountInRequest";

		// Token: 0x040009C7 RID: 2503
		internal const string ODataWriterCore_CannotWriteTopLevelFeedWithEntryWriter = "ODataWriterCore_CannotWriteTopLevelFeedWithEntryWriter";

		// Token: 0x040009C8 RID: 2504
		internal const string ODataWriterCore_CannotWriteTopLevelEntryWithFeedWriter = "ODataWriterCore_CannotWriteTopLevelEntryWithFeedWriter";

		// Token: 0x040009C9 RID: 2505
		internal const string ODataWriterCore_SyncCallOnAsyncWriter = "ODataWriterCore_SyncCallOnAsyncWriter";

		// Token: 0x040009CA RID: 2506
		internal const string ODataWriterCore_AsyncCallOnSyncWriter = "ODataWriterCore_AsyncCallOnSyncWriter";

		// Token: 0x040009CB RID: 2507
		internal const string ODataWriterCore_EntityReferenceLinkWithoutNavigationLink = "ODataWriterCore_EntityReferenceLinkWithoutNavigationLink";

		// Token: 0x040009CC RID: 2508
		internal const string ODataWriterCore_EntityReferenceLinkInResponse = "ODataWriterCore_EntityReferenceLinkInResponse";

		// Token: 0x040009CD RID: 2509
		internal const string ODataWriterCore_DeferredLinkInRequest = "ODataWriterCore_DeferredLinkInRequest";

		// Token: 0x040009CE RID: 2510
		internal const string ODataWriterCore_MultipleItemsInNavigationLinkContent = "ODataWriterCore_MultipleItemsInNavigationLinkContent";

		// Token: 0x040009CF RID: 2511
		internal const string ODataWriterCore_DeltaLinkNotSupportedOnExpandedFeed = "ODataWriterCore_DeltaLinkNotSupportedOnExpandedFeed";

		// Token: 0x040009D0 RID: 2512
		internal const string DuplicatePropertyNamesChecker_DuplicatePropertyNamesNotAllowed = "DuplicatePropertyNamesChecker_DuplicatePropertyNamesNotAllowed";

		// Token: 0x040009D1 RID: 2513
		internal const string DuplicatePropertyNamesChecker_MultipleLinksForSingleton = "DuplicatePropertyNamesChecker_MultipleLinksForSingleton";

		// Token: 0x040009D2 RID: 2514
		internal const string DuplicatePropertyNamesChecker_DuplicateAnnotationNotAllowed = "DuplicatePropertyNamesChecker_DuplicateAnnotationNotAllowed";

		// Token: 0x040009D3 RID: 2515
		internal const string DuplicatePropertyNamesChecker_DuplicateAnnotationForPropertyNotAllowed = "DuplicatePropertyNamesChecker_DuplicateAnnotationForPropertyNotAllowed";

		// Token: 0x040009D4 RID: 2516
		internal const string DuplicatePropertyNamesChecker_DuplicateAnnotationForInstanceAnnotationNotAllowed = "DuplicatePropertyNamesChecker_DuplicateAnnotationForInstanceAnnotationNotAllowed";

		// Token: 0x040009D5 RID: 2517
		internal const string DuplicatePropertyNamesChecker_PropertyAnnotationAfterTheProperty = "DuplicatePropertyNamesChecker_PropertyAnnotationAfterTheProperty";

		// Token: 0x040009D6 RID: 2518
		internal const string AtomValueUtils_CannotConvertValueToAtomPrimitive = "AtomValueUtils_CannotConvertValueToAtomPrimitive";

		// Token: 0x040009D7 RID: 2519
		internal const string ODataJsonWriter_UnsupportedValueType = "ODataJsonWriter_UnsupportedValueType";

		// Token: 0x040009D8 RID: 2520
		internal const string ODataException_GeneralError = "ODataException_GeneralError";

		// Token: 0x040009D9 RID: 2521
		internal const string ODataErrorException_GeneralError = "ODataErrorException_GeneralError";

		// Token: 0x040009DA RID: 2522
		internal const string ODataUriParserException_GeneralError = "ODataUriParserException_GeneralError";

		// Token: 0x040009DB RID: 2523
		internal const string ODataVersionChecker_MaxProtocolVersionExceeded = "ODataVersionChecker_MaxProtocolVersionExceeded";

		// Token: 0x040009DC RID: 2524
		internal const string ODataVersionChecker_PropertyNotSupportedForODataVersionGreaterThanX = "ODataVersionChecker_PropertyNotSupportedForODataVersionGreaterThanX";

		// Token: 0x040009DD RID: 2525
		internal const string ODataVersionChecker_ParameterPayloadNotSupported = "ODataVersionChecker_ParameterPayloadNotSupported";

		// Token: 0x040009DE RID: 2526
		internal const string ODataVersionChecker_AssociationLinksNotSupported = "ODataVersionChecker_AssociationLinksNotSupported";

		// Token: 0x040009DF RID: 2527
		internal const string ODataVersionChecker_InlineCountNotSupported = "ODataVersionChecker_InlineCountNotSupported";

		// Token: 0x040009E0 RID: 2528
		internal const string ODataVersionChecker_NextLinkNotSupported = "ODataVersionChecker_NextLinkNotSupported";

		// Token: 0x040009E1 RID: 2529
		internal const string ODataVersionChecker_DeltaLinkNotSupported = "ODataVersionChecker_DeltaLinkNotSupported";

		// Token: 0x040009E2 RID: 2530
		internal const string ODataVersionChecker_CollectionPropertiesNotSupported = "ODataVersionChecker_CollectionPropertiesNotSupported";

		// Token: 0x040009E3 RID: 2531
		internal const string ODataVersionChecker_CollectionNotSupported = "ODataVersionChecker_CollectionNotSupported";

		// Token: 0x040009E4 RID: 2532
		internal const string ODataVersionChecker_StreamPropertiesNotSupported = "ODataVersionChecker_StreamPropertiesNotSupported";

		// Token: 0x040009E5 RID: 2533
		internal const string ODataVersionChecker_EpmVersionNotSupported = "ODataVersionChecker_EpmVersionNotSupported";

		// Token: 0x040009E6 RID: 2534
		internal const string ODataVersionChecker_ProtocolVersion3IsNotSupported = "ODataVersionChecker_ProtocolVersion3IsNotSupported";

		// Token: 0x040009E7 RID: 2535
		internal const string ODataVersionChecker_GeographyAndGeometryNotSupported = "ODataVersionChecker_GeographyAndGeometryNotSupported";

		// Token: 0x040009E8 RID: 2536
		internal const string ODataAtomCollectionWriter_CollectionNameMustNotBeNull = "ODataAtomCollectionWriter_CollectionNameMustNotBeNull";

		// Token: 0x040009E9 RID: 2537
		internal const string ODataAtomWriter_StartEntryXmlCustomizationCallbackReturnedSameInstance = "ODataAtomWriter_StartEntryXmlCustomizationCallbackReturnedSameInstance";

		// Token: 0x040009EA RID: 2538
		internal const string ODataAtomWriterMetadataUtils_AuthorMetadataMustNotContainNull = "ODataAtomWriterMetadataUtils_AuthorMetadataMustNotContainNull";

		// Token: 0x040009EB RID: 2539
		internal const string ODataAtomWriterMetadataUtils_CategoryMetadataMustNotContainNull = "ODataAtomWriterMetadataUtils_CategoryMetadataMustNotContainNull";

		// Token: 0x040009EC RID: 2540
		internal const string ODataAtomWriterMetadataUtils_ContributorMetadataMustNotContainNull = "ODataAtomWriterMetadataUtils_ContributorMetadataMustNotContainNull";

		// Token: 0x040009ED RID: 2541
		internal const string ODataAtomWriterMetadataUtils_LinkMetadataMustNotContainNull = "ODataAtomWriterMetadataUtils_LinkMetadataMustNotContainNull";

		// Token: 0x040009EE RID: 2542
		internal const string ODataAtomWriterMetadataUtils_LinkMustSpecifyHref = "ODataAtomWriterMetadataUtils_LinkMustSpecifyHref";

		// Token: 0x040009EF RID: 2543
		internal const string ODataAtomWriterMetadataUtils_CategoryMustSpecifyTerm = "ODataAtomWriterMetadataUtils_CategoryMustSpecifyTerm";

		// Token: 0x040009F0 RID: 2544
		internal const string ODataAtomWriterMetadataUtils_LinkHrefsMustMatch = "ODataAtomWriterMetadataUtils_LinkHrefsMustMatch";

		// Token: 0x040009F1 RID: 2545
		internal const string ODataAtomWriterMetadataUtils_LinkTitlesMustMatch = "ODataAtomWriterMetadataUtils_LinkTitlesMustMatch";

		// Token: 0x040009F2 RID: 2546
		internal const string ODataAtomWriterMetadataUtils_LinkRelationsMustMatch = "ODataAtomWriterMetadataUtils_LinkRelationsMustMatch";

		// Token: 0x040009F3 RID: 2547
		internal const string ODataAtomWriterMetadataUtils_LinkMediaTypesMustMatch = "ODataAtomWriterMetadataUtils_LinkMediaTypesMustMatch";

		// Token: 0x040009F4 RID: 2548
		internal const string ODataAtomWriterMetadataUtils_InvalidAnnotationValue = "ODataAtomWriterMetadataUtils_InvalidAnnotationValue";

		// Token: 0x040009F5 RID: 2549
		internal const string ODataAtomWriterMetadataUtils_CategoriesHrefWithOtherValues = "ODataAtomWriterMetadataUtils_CategoriesHrefWithOtherValues";

		// Token: 0x040009F6 RID: 2550
		internal const string ODataAtomWriterMetadataUtils_CategoryTermsMustMatch = "ODataAtomWriterMetadataUtils_CategoryTermsMustMatch";

		// Token: 0x040009F7 RID: 2551
		internal const string ODataAtomWriterMetadataUtils_CategorySchemesMustMatch = "ODataAtomWriterMetadataUtils_CategorySchemesMustMatch";

		// Token: 0x040009F8 RID: 2552
		internal const string ODataAtomMetadataEpmMerge_TextKindConflict = "ODataAtomMetadataEpmMerge_TextKindConflict";

		// Token: 0x040009F9 RID: 2553
		internal const string ODataAtomMetadataEpmMerge_TextValueConflict = "ODataAtomMetadataEpmMerge_TextValueConflict";

		// Token: 0x040009FA RID: 2554
		internal const string ODataMessageWriter_WriterAlreadyUsed = "ODataMessageWriter_WriterAlreadyUsed";

		// Token: 0x040009FB RID: 2555
		internal const string ODataMessageWriter_InvalidContentTypeForWritingRawValue = "ODataMessageWriter_InvalidContentTypeForWritingRawValue";

		// Token: 0x040009FC RID: 2556
		internal const string ODataMessageWriter_EntityReferenceLinksInRequestNotAllowed = "ODataMessageWriter_EntityReferenceLinksInRequestNotAllowed";

		// Token: 0x040009FD RID: 2557
		internal const string ODataMessageWriter_ErrorPayloadInRequest = "ODataMessageWriter_ErrorPayloadInRequest";

		// Token: 0x040009FE RID: 2558
		internal const string ODataMessageWriter_ServiceDocumentInRequest = "ODataMessageWriter_ServiceDocumentInRequest";

		// Token: 0x040009FF RID: 2559
		internal const string ODataMessageWriter_MetadataDocumentInRequest = "ODataMessageWriter_MetadataDocumentInRequest";

		// Token: 0x04000A00 RID: 2560
		internal const string ODataMessageWriter_CannotWriteNullInRawFormat = "ODataMessageWriter_CannotWriteNullInRawFormat";

		// Token: 0x04000A01 RID: 2561
		internal const string ODataMessageWriter_CannotSetHeadersWithInvalidPayloadKind = "ODataMessageWriter_CannotSetHeadersWithInvalidPayloadKind";

		// Token: 0x04000A02 RID: 2562
		internal const string ODataMessageWriter_IncompatiblePayloadKinds = "ODataMessageWriter_IncompatiblePayloadKinds";

		// Token: 0x04000A03 RID: 2563
		internal const string ODataMessageWriter_CannotWriteStreamPropertyAsTopLevelProperty = "ODataMessageWriter_CannotWriteStreamPropertyAsTopLevelProperty";

		// Token: 0x04000A04 RID: 2564
		internal const string ODataMessageWriter_InvalidPropertyOwningType = "ODataMessageWriter_InvalidPropertyOwningType";

		// Token: 0x04000A05 RID: 2565
		internal const string ODataMessageWriter_InvalidPropertyProducingFunctionImport = "ODataMessageWriter_InvalidPropertyProducingFunctionImport";

		// Token: 0x04000A06 RID: 2566
		internal const string ODataMessageWriter_WriteErrorAlreadyCalled = "ODataMessageWriter_WriteErrorAlreadyCalled";

		// Token: 0x04000A07 RID: 2567
		internal const string ODataMessageWriter_CannotWriteInStreamErrorForRawValues = "ODataMessageWriter_CannotWriteInStreamErrorForRawValues";

		// Token: 0x04000A08 RID: 2568
		internal const string ODataMessageWriter_CannotWriteMetadataWithoutModel = "ODataMessageWriter_CannotWriteMetadataWithoutModel";

		// Token: 0x04000A09 RID: 2569
		internal const string ODataMessageWriter_CannotSpecifyFunctionImportWithoutModel = "ODataMessageWriter_CannotSpecifyFunctionImportWithoutModel";

		// Token: 0x04000A0A RID: 2570
		internal const string ODataMessageWriter_EntityReferenceLinksWithSingletonNavPropNotAllowed = "ODataMessageWriter_EntityReferenceLinksWithSingletonNavPropNotAllowed";

		// Token: 0x04000A0B RID: 2571
		internal const string ODataMessageWriter_JsonPaddingOnInvalidContentType = "ODataMessageWriter_JsonPaddingOnInvalidContentType";

		// Token: 0x04000A0C RID: 2572
		internal const string ODataMessageWriter_NonCollectionType = "ODataMessageWriter_NonCollectionType";

		// Token: 0x04000A0D RID: 2573
		internal const string ODataMessageWriterSettings_MessageWriterSettingsXmlCustomizationCallbacksMustBeSpecifiedBoth = "ODataMessageWriterSettings_MessageWriterSettingsXmlCustomizationCallbacksMustBeSpecifiedBoth";

		// Token: 0x04000A0E RID: 2574
		internal const string ODataCollectionWriter_CannotCreateCollectionWriterForFormat = "ODataCollectionWriter_CannotCreateCollectionWriterForFormat";

		// Token: 0x04000A0F RID: 2575
		internal const string ODataCollectionWriterCore_InvalidTransitionFromStart = "ODataCollectionWriterCore_InvalidTransitionFromStart";

		// Token: 0x04000A10 RID: 2576
		internal const string ODataCollectionWriterCore_InvalidTransitionFromCollection = "ODataCollectionWriterCore_InvalidTransitionFromCollection";

		// Token: 0x04000A11 RID: 2577
		internal const string ODataCollectionWriterCore_InvalidTransitionFromItem = "ODataCollectionWriterCore_InvalidTransitionFromItem";

		// Token: 0x04000A12 RID: 2578
		internal const string ODataCollectionWriterCore_WriteEndCalledInInvalidState = "ODataCollectionWriterCore_WriteEndCalledInInvalidState";

		// Token: 0x04000A13 RID: 2579
		internal const string ODataCollectionWriterCore_SyncCallOnAsyncWriter = "ODataCollectionWriterCore_SyncCallOnAsyncWriter";

		// Token: 0x04000A14 RID: 2580
		internal const string ODataCollectionWriterCore_AsyncCallOnSyncWriter = "ODataCollectionWriterCore_AsyncCallOnSyncWriter";

		// Token: 0x04000A15 RID: 2581
		internal const string ODataCollectionWriterCore_CollectionsMustNotHaveEmptyName = "ODataCollectionWriterCore_CollectionsMustNotHaveEmptyName";

		// Token: 0x04000A16 RID: 2582
		internal const string ODataCollectionWriterCore_CollectionNameDoesntMatchFunctionImportName = "ODataCollectionWriterCore_CollectionNameDoesntMatchFunctionImportName";

		// Token: 0x04000A17 RID: 2583
		internal const string ODataCollectionWriterCore_NonCollectionType = "ODataCollectionWriterCore_NonCollectionType";

		// Token: 0x04000A18 RID: 2584
		internal const string ODataBatch_InvalidHttpMethodForQueryOperation = "ODataBatch_InvalidHttpMethodForQueryOperation";

		// Token: 0x04000A19 RID: 2585
		internal const string ODataBatch_InvalidHttpMethodForChangeSetRequest = "ODataBatch_InvalidHttpMethodForChangeSetRequest";

		// Token: 0x04000A1A RID: 2586
		internal const string ODataBatchOperationHeaderDictionary_KeyNotFound = "ODataBatchOperationHeaderDictionary_KeyNotFound";

		// Token: 0x04000A1B RID: 2587
		internal const string ODataBatchOperationHeaderDictionary_DuplicateCaseInsensitiveKeys = "ODataBatchOperationHeaderDictionary_DuplicateCaseInsensitiveKeys";

		// Token: 0x04000A1C RID: 2588
		internal const string ODataParameterWriter_InStreamErrorNotSupported = "ODataParameterWriter_InStreamErrorNotSupported";

		// Token: 0x04000A1D RID: 2589
		internal const string ODataParameterWriter_CannotCreateParameterWriterOnResponseMessage = "ODataParameterWriter_CannotCreateParameterWriterOnResponseMessage";

		// Token: 0x04000A1E RID: 2590
		internal const string ODataParameterWriterCore_SyncCallOnAsyncWriter = "ODataParameterWriterCore_SyncCallOnAsyncWriter";

		// Token: 0x04000A1F RID: 2591
		internal const string ODataParameterWriterCore_AsyncCallOnSyncWriter = "ODataParameterWriterCore_AsyncCallOnSyncWriter";

		// Token: 0x04000A20 RID: 2592
		internal const string ODataParameterWriterCore_CannotWriteStart = "ODataParameterWriterCore_CannotWriteStart";

		// Token: 0x04000A21 RID: 2593
		internal const string ODataParameterWriterCore_CannotWriteParameter = "ODataParameterWriterCore_CannotWriteParameter";

		// Token: 0x04000A22 RID: 2594
		internal const string ODataParameterWriterCore_CannotWriteEnd = "ODataParameterWriterCore_CannotWriteEnd";

		// Token: 0x04000A23 RID: 2595
		internal const string ODataParameterWriterCore_CannotWriteInErrorOrCompletedState = "ODataParameterWriterCore_CannotWriteInErrorOrCompletedState";

		// Token: 0x04000A24 RID: 2596
		internal const string ODataParameterWriterCore_DuplicatedParameterNameNotAllowed = "ODataParameterWriterCore_DuplicatedParameterNameNotAllowed";

		// Token: 0x04000A25 RID: 2597
		internal const string ODataParameterWriterCore_CannotWriteValueOnNonValueTypeKind = "ODataParameterWriterCore_CannotWriteValueOnNonValueTypeKind";

		// Token: 0x04000A26 RID: 2598
		internal const string ODataParameterWriterCore_CannotWriteValueOnNonSupportedValueType = "ODataParameterWriterCore_CannotWriteValueOnNonSupportedValueType";

		// Token: 0x04000A27 RID: 2599
		internal const string ODataParameterWriterCore_CannotCreateCollectionWriterOnNonCollectionTypeKind = "ODataParameterWriterCore_CannotCreateCollectionWriterOnNonCollectionTypeKind";

		// Token: 0x04000A28 RID: 2600
		internal const string ODataParameterWriterCore_ParameterNameNotFoundInFunctionImport = "ODataParameterWriterCore_ParameterNameNotFoundInFunctionImport";

		// Token: 0x04000A29 RID: 2601
		internal const string ODataParameterWriterCore_MissingParameterInParameterPayload = "ODataParameterWriterCore_MissingParameterInParameterPayload";

		// Token: 0x04000A2A RID: 2602
		internal const string ODataBatchWriter_FlushOrFlushAsyncCalledInStreamRequestedState = "ODataBatchWriter_FlushOrFlushAsyncCalledInStreamRequestedState";

		// Token: 0x04000A2B RID: 2603
		internal const string ODataBatchWriter_CannotCompleteBatchWithActiveChangeSet = "ODataBatchWriter_CannotCompleteBatchWithActiveChangeSet";

		// Token: 0x04000A2C RID: 2604
		internal const string ODataBatchWriter_CannotStartChangeSetWithActiveChangeSet = "ODataBatchWriter_CannotStartChangeSetWithActiveChangeSet";

		// Token: 0x04000A2D RID: 2605
		internal const string ODataBatchWriter_CannotCompleteChangeSetWithoutActiveChangeSet = "ODataBatchWriter_CannotCompleteChangeSetWithoutActiveChangeSet";

		// Token: 0x04000A2E RID: 2606
		internal const string ODataBatchWriter_InvalidTransitionFromStart = "ODataBatchWriter_InvalidTransitionFromStart";

		// Token: 0x04000A2F RID: 2607
		internal const string ODataBatchWriter_InvalidTransitionFromBatchStarted = "ODataBatchWriter_InvalidTransitionFromBatchStarted";

		// Token: 0x04000A30 RID: 2608
		internal const string ODataBatchWriter_InvalidTransitionFromChangeSetStarted = "ODataBatchWriter_InvalidTransitionFromChangeSetStarted";

		// Token: 0x04000A31 RID: 2609
		internal const string ODataBatchWriter_InvalidTransitionFromOperationCreated = "ODataBatchWriter_InvalidTransitionFromOperationCreated";

		// Token: 0x04000A32 RID: 2610
		internal const string ODataBatchWriter_InvalidTransitionFromOperationContentStreamRequested = "ODataBatchWriter_InvalidTransitionFromOperationContentStreamRequested";

		// Token: 0x04000A33 RID: 2611
		internal const string ODataBatchWriter_InvalidTransitionFromOperationContentStreamDisposed = "ODataBatchWriter_InvalidTransitionFromOperationContentStreamDisposed";

		// Token: 0x04000A34 RID: 2612
		internal const string ODataBatchWriter_InvalidTransitionFromChangeSetCompleted = "ODataBatchWriter_InvalidTransitionFromChangeSetCompleted";

		// Token: 0x04000A35 RID: 2613
		internal const string ODataBatchWriter_InvalidTransitionFromBatchCompleted = "ODataBatchWriter_InvalidTransitionFromBatchCompleted";

		// Token: 0x04000A36 RID: 2614
		internal const string ODataBatchWriter_CannotCreateRequestOperationWhenWritingResponse = "ODataBatchWriter_CannotCreateRequestOperationWhenWritingResponse";

		// Token: 0x04000A37 RID: 2615
		internal const string ODataBatchWriter_CannotCreateResponseOperationWhenWritingRequest = "ODataBatchWriter_CannotCreateResponseOperationWhenWritingRequest";

		// Token: 0x04000A38 RID: 2616
		internal const string ODataBatchWriter_MaxBatchSizeExceeded = "ODataBatchWriter_MaxBatchSizeExceeded";

		// Token: 0x04000A39 RID: 2617
		internal const string ODataBatchWriter_MaxChangeSetSizeExceeded = "ODataBatchWriter_MaxChangeSetSizeExceeded";

		// Token: 0x04000A3A RID: 2618
		internal const string ODataBatchWriter_SyncCallOnAsyncWriter = "ODataBatchWriter_SyncCallOnAsyncWriter";

		// Token: 0x04000A3B RID: 2619
		internal const string ODataBatchWriter_AsyncCallOnSyncWriter = "ODataBatchWriter_AsyncCallOnSyncWriter";

		// Token: 0x04000A3C RID: 2620
		internal const string ODataBatchWriter_DuplicateContentIDsNotAllowed = "ODataBatchWriter_DuplicateContentIDsNotAllowed";

		// Token: 0x04000A3D RID: 2621
		internal const string ODataBatchWriter_CannotWriteInStreamErrorForBatch = "ODataBatchWriter_CannotWriteInStreamErrorForBatch";

		// Token: 0x04000A3E RID: 2622
		internal const string ODataBatchUtils_RelativeUriUsedWithoutBaseUriSpecified = "ODataBatchUtils_RelativeUriUsedWithoutBaseUriSpecified";

		// Token: 0x04000A3F RID: 2623
		internal const string ODataBatchUtils_RelativeUriStartingWithDollarUsedWithoutBaseUriSpecified = "ODataBatchUtils_RelativeUriStartingWithDollarUsedWithoutBaseUriSpecified";

		// Token: 0x04000A40 RID: 2624
		internal const string ODataBatchOperationMessage_VerifyNotCompleted = "ODataBatchOperationMessage_VerifyNotCompleted";

		// Token: 0x04000A41 RID: 2625
		internal const string ODataBatchOperationStream_Disposed = "ODataBatchOperationStream_Disposed";

		// Token: 0x04000A42 RID: 2626
		internal const string ODataBatchReader_CannotCreateRequestOperationWhenReadingResponse = "ODataBatchReader_CannotCreateRequestOperationWhenReadingResponse";

		// Token: 0x04000A43 RID: 2627
		internal const string ODataBatchReader_CannotCreateResponseOperationWhenReadingRequest = "ODataBatchReader_CannotCreateResponseOperationWhenReadingRequest";

		// Token: 0x04000A44 RID: 2628
		internal const string ODataBatchReader_InvalidStateForCreateOperationRequestMessage = "ODataBatchReader_InvalidStateForCreateOperationRequestMessage";

		// Token: 0x04000A45 RID: 2629
		internal const string ODataBatchReader_OperationRequestMessageAlreadyCreated = "ODataBatchReader_OperationRequestMessageAlreadyCreated";

		// Token: 0x04000A46 RID: 2630
		internal const string ODataBatchReader_OperationResponseMessageAlreadyCreated = "ODataBatchReader_OperationResponseMessageAlreadyCreated";

		// Token: 0x04000A47 RID: 2631
		internal const string ODataBatchReader_InvalidStateForCreateOperationResponseMessage = "ODataBatchReader_InvalidStateForCreateOperationResponseMessage";

		// Token: 0x04000A48 RID: 2632
		internal const string ODataBatchReader_CannotUseReaderWhileOperationStreamActive = "ODataBatchReader_CannotUseReaderWhileOperationStreamActive";

		// Token: 0x04000A49 RID: 2633
		internal const string ODataBatchReader_SyncCallOnAsyncReader = "ODataBatchReader_SyncCallOnAsyncReader";

		// Token: 0x04000A4A RID: 2634
		internal const string ODataBatchReader_AsyncCallOnSyncReader = "ODataBatchReader_AsyncCallOnSyncReader";

		// Token: 0x04000A4B RID: 2635
		internal const string ODataBatchReader_ReadOrReadAsyncCalledInInvalidState = "ODataBatchReader_ReadOrReadAsyncCalledInInvalidState";

		// Token: 0x04000A4C RID: 2636
		internal const string ODataBatchReader_MaxBatchSizeExceeded = "ODataBatchReader_MaxBatchSizeExceeded";

		// Token: 0x04000A4D RID: 2637
		internal const string ODataBatchReader_MaxChangeSetSizeExceeded = "ODataBatchReader_MaxChangeSetSizeExceeded";

		// Token: 0x04000A4E RID: 2638
		internal const string ODataBatchReader_NoMessageWasCreatedForOperation = "ODataBatchReader_NoMessageWasCreatedForOperation";

		// Token: 0x04000A4F RID: 2639
		internal const string ODataBatchReader_DuplicateContentIDsNotAllowed = "ODataBatchReader_DuplicateContentIDsNotAllowed";

		// Token: 0x04000A50 RID: 2640
		internal const string ODataBatchReaderStream_InvalidHeaderSpecified = "ODataBatchReaderStream_InvalidHeaderSpecified";

		// Token: 0x04000A51 RID: 2641
		internal const string ODataBatchReaderStream_InvalidRequestLine = "ODataBatchReaderStream_InvalidRequestLine";

		// Token: 0x04000A52 RID: 2642
		internal const string ODataBatchReaderStream_InvalidResponseLine = "ODataBatchReaderStream_InvalidResponseLine";

		// Token: 0x04000A53 RID: 2643
		internal const string ODataBatchReaderStream_InvalidHttpVersionSpecified = "ODataBatchReaderStream_InvalidHttpVersionSpecified";

		// Token: 0x04000A54 RID: 2644
		internal const string ODataBatchReaderStream_NonIntegerHttpStatusCode = "ODataBatchReaderStream_NonIntegerHttpStatusCode";

		// Token: 0x04000A55 RID: 2645
		internal const string ODataBatchReaderStream_MissingContentTypeHeader = "ODataBatchReaderStream_MissingContentTypeHeader";

		// Token: 0x04000A56 RID: 2646
		internal const string ODataBatchReaderStream_MissingOrInvalidContentEncodingHeader = "ODataBatchReaderStream_MissingOrInvalidContentEncodingHeader";

		// Token: 0x04000A57 RID: 2647
		internal const string ODataBatchReaderStream_InvalidContentTypeSpecified = "ODataBatchReaderStream_InvalidContentTypeSpecified";

		// Token: 0x04000A58 RID: 2648
		internal const string ODataBatchReaderStream_InvalidContentLengthSpecified = "ODataBatchReaderStream_InvalidContentLengthSpecified";

		// Token: 0x04000A59 RID: 2649
		internal const string ODataBatchReaderStream_DuplicateHeaderFound = "ODataBatchReaderStream_DuplicateHeaderFound";

		// Token: 0x04000A5A RID: 2650
		internal const string ODataBatchReaderStream_NestedChangesetsAreNotSupported = "ODataBatchReaderStream_NestedChangesetsAreNotSupported";

		// Token: 0x04000A5B RID: 2651
		internal const string ODataBatchReaderStream_MultiByteEncodingsNotSupported = "ODataBatchReaderStream_MultiByteEncodingsNotSupported";

		// Token: 0x04000A5C RID: 2652
		internal const string ODataBatchReaderStream_UnexpectedEndOfInput = "ODataBatchReaderStream_UnexpectedEndOfInput";

		// Token: 0x04000A5D RID: 2653
		internal const string ODataBatchReaderStreamBuffer_BoundaryLineSecurityLimitReached = "ODataBatchReaderStreamBuffer_BoundaryLineSecurityLimitReached";

		// Token: 0x04000A5E RID: 2654
		internal const string HttpUtils_MediaTypeUnspecified = "HttpUtils_MediaTypeUnspecified";

		// Token: 0x04000A5F RID: 2655
		internal const string HttpUtils_MediaTypeRequiresSlash = "HttpUtils_MediaTypeRequiresSlash";

		// Token: 0x04000A60 RID: 2656
		internal const string HttpUtils_MediaTypeRequiresSubType = "HttpUtils_MediaTypeRequiresSubType";

		// Token: 0x04000A61 RID: 2657
		internal const string HttpUtils_MediaTypeMissingParameterValue = "HttpUtils_MediaTypeMissingParameterValue";

		// Token: 0x04000A62 RID: 2658
		internal const string HttpUtils_MediaTypeMissingParameterName = "HttpUtils_MediaTypeMissingParameterName";

		// Token: 0x04000A63 RID: 2659
		internal const string HttpUtils_EscapeCharWithoutQuotes = "HttpUtils_EscapeCharWithoutQuotes";

		// Token: 0x04000A64 RID: 2660
		internal const string HttpUtils_EscapeCharAtEnd = "HttpUtils_EscapeCharAtEnd";

		// Token: 0x04000A65 RID: 2661
		internal const string HttpUtils_ClosingQuoteNotFound = "HttpUtils_ClosingQuoteNotFound";

		// Token: 0x04000A66 RID: 2662
		internal const string HttpUtils_InvalidCharacterInQuotedParameterValue = "HttpUtils_InvalidCharacterInQuotedParameterValue";

		// Token: 0x04000A67 RID: 2663
		internal const string HttpUtils_ContentTypeMissing = "HttpUtils_ContentTypeMissing";

		// Token: 0x04000A68 RID: 2664
		internal const string HttpUtils_MediaTypeRequiresSemicolonBeforeParameter = "HttpUtils_MediaTypeRequiresSemicolonBeforeParameter";

		// Token: 0x04000A69 RID: 2665
		internal const string HttpUtils_InvalidQualityValueStartChar = "HttpUtils_InvalidQualityValueStartChar";

		// Token: 0x04000A6A RID: 2666
		internal const string HttpUtils_InvalidQualityValue = "HttpUtils_InvalidQualityValue";

		// Token: 0x04000A6B RID: 2667
		internal const string HttpUtils_CannotConvertCharToInt = "HttpUtils_CannotConvertCharToInt";

		// Token: 0x04000A6C RID: 2668
		internal const string HttpUtils_MissingSeparatorBetweenCharsets = "HttpUtils_MissingSeparatorBetweenCharsets";

		// Token: 0x04000A6D RID: 2669
		internal const string HttpUtils_InvalidSeparatorBetweenCharsets = "HttpUtils_InvalidSeparatorBetweenCharsets";

		// Token: 0x04000A6E RID: 2670
		internal const string HttpUtils_InvalidCharsetName = "HttpUtils_InvalidCharsetName";

		// Token: 0x04000A6F RID: 2671
		internal const string HttpUtils_UnexpectedEndOfQValue = "HttpUtils_UnexpectedEndOfQValue";

		// Token: 0x04000A70 RID: 2672
		internal const string HttpUtils_ExpectedLiteralNotFoundInString = "HttpUtils_ExpectedLiteralNotFoundInString";

		// Token: 0x04000A71 RID: 2673
		internal const string HttpUtils_InvalidHttpMethodString = "HttpUtils_InvalidHttpMethodString";

		// Token: 0x04000A72 RID: 2674
		internal const string HttpUtils_NoOrMoreThanOneContentTypeSpecified = "HttpUtils_NoOrMoreThanOneContentTypeSpecified";

		// Token: 0x04000A73 RID: 2675
		internal const string HttpHeaderValueLexer_UnrecognizedSeparator = "HttpHeaderValueLexer_UnrecognizedSeparator";

		// Token: 0x04000A74 RID: 2676
		internal const string HttpHeaderValueLexer_TokenExpectedButFoundQuotedString = "HttpHeaderValueLexer_TokenExpectedButFoundQuotedString";

		// Token: 0x04000A75 RID: 2677
		internal const string HttpHeaderValueLexer_FailedToReadTokenOrQuotedString = "HttpHeaderValueLexer_FailedToReadTokenOrQuotedString";

		// Token: 0x04000A76 RID: 2678
		internal const string HttpHeaderValueLexer_InvalidSeparatorAfterQuotedString = "HttpHeaderValueLexer_InvalidSeparatorAfterQuotedString";

		// Token: 0x04000A77 RID: 2679
		internal const string HttpHeaderValueLexer_EndOfFileAfterSeparator = "HttpHeaderValueLexer_EndOfFileAfterSeparator";

		// Token: 0x04000A78 RID: 2680
		internal const string MediaType_EncodingNotSupported = "MediaType_EncodingNotSupported";

		// Token: 0x04000A79 RID: 2681
		internal const string MediaTypeUtils_DidNotFindMatchingMediaType = "MediaTypeUtils_DidNotFindMatchingMediaType";

		// Token: 0x04000A7A RID: 2682
		internal const string MediaTypeUtils_CannotDetermineFormatFromContentType = "MediaTypeUtils_CannotDetermineFormatFromContentType";

		// Token: 0x04000A7B RID: 2683
		internal const string MediaTypeUtils_NoOrMoreThanOneContentTypeSpecified = "MediaTypeUtils_NoOrMoreThanOneContentTypeSpecified";

		// Token: 0x04000A7C RID: 2684
		internal const string MediaTypeUtils_BoundaryMustBeSpecifiedForBatchPayloads = "MediaTypeUtils_BoundaryMustBeSpecifiedForBatchPayloads";

		// Token: 0x04000A7D RID: 2685
		internal const string EntityPropertyMapping_EpmAttribute = "EntityPropertyMapping_EpmAttribute";

		// Token: 0x04000A7E RID: 2686
		internal const string EntityPropertyMapping_InvalidTargetPath = "EntityPropertyMapping_InvalidTargetPath";

		// Token: 0x04000A7F RID: 2687
		internal const string EntityPropertyMapping_TargetNamespaceUriNotValid = "EntityPropertyMapping_TargetNamespaceUriNotValid";

		// Token: 0x04000A80 RID: 2688
		internal const string EpmSourceTree_InvalidSourcePath = "EpmSourceTree_InvalidSourcePath";

		// Token: 0x04000A81 RID: 2689
		internal const string EpmSourceTree_EndsWithNonPrimitiveType = "EpmSourceTree_EndsWithNonPrimitiveType";

		// Token: 0x04000A82 RID: 2690
		internal const string EpmSourceTree_TraversalOfNonComplexType = "EpmSourceTree_TraversalOfNonComplexType";

		// Token: 0x04000A83 RID: 2691
		internal const string EpmSourceTree_DuplicateEpmAttributesWithSameSourceName = "EpmSourceTree_DuplicateEpmAttributesWithSameSourceName";

		// Token: 0x04000A84 RID: 2692
		internal const string EpmSourceTree_MissingPropertyOnType = "EpmSourceTree_MissingPropertyOnType";

		// Token: 0x04000A85 RID: 2693
		internal const string EpmSourceTree_MissingPropertyOnInstance = "EpmSourceTree_MissingPropertyOnInstance";

		// Token: 0x04000A86 RID: 2694
		internal const string EpmSourceTree_StreamPropertyCannotBeMapped = "EpmSourceTree_StreamPropertyCannotBeMapped";

		// Token: 0x04000A87 RID: 2695
		internal const string EpmSourceTree_SpatialTypeCannotBeMapped = "EpmSourceTree_SpatialTypeCannotBeMapped";

		// Token: 0x04000A88 RID: 2696
		internal const string EpmSourceTree_OpenPropertySpatialTypeCannotBeMapped = "EpmSourceTree_OpenPropertySpatialTypeCannotBeMapped";

		// Token: 0x04000A89 RID: 2697
		internal const string EpmSourceTree_OpenComplexPropertyCannotBeMapped = "EpmSourceTree_OpenComplexPropertyCannotBeMapped";

		// Token: 0x04000A8A RID: 2698
		internal const string EpmSourceTree_CollectionPropertyCannotBeMapped = "EpmSourceTree_CollectionPropertyCannotBeMapped";

		// Token: 0x04000A8B RID: 2699
		internal const string EpmTargetTree_InvalidTargetPath_EmptySegment = "EpmTargetTree_InvalidTargetPath_EmptySegment";

		// Token: 0x04000A8C RID: 2700
		internal const string EpmTargetTree_InvalidTargetPath_MixedContent = "EpmTargetTree_InvalidTargetPath_MixedContent";

		// Token: 0x04000A8D RID: 2701
		internal const string EpmTargetTree_AttributeInMiddle = "EpmTargetTree_AttributeInMiddle";

		// Token: 0x04000A8E RID: 2702
		internal const string EpmTargetTree_DuplicateEpmAttributesWithSameTargetName = "EpmTargetTree_DuplicateEpmAttributesWithSameTargetName";

		// Token: 0x04000A8F RID: 2703
		internal const string EpmSyndicationWriter_DateTimePropertyCanNotBeConverted = "EpmSyndicationWriter_DateTimePropertyCanNotBeConverted";

		// Token: 0x04000A90 RID: 2704
		internal const string EpmSyndicationWriter_EmptyCollectionMappedToAuthor = "EpmSyndicationWriter_EmptyCollectionMappedToAuthor";

		// Token: 0x04000A91 RID: 2705
		internal const string EpmSyndicationWriter_NullValueForAttributeTarget = "EpmSyndicationWriter_NullValueForAttributeTarget";

		// Token: 0x04000A92 RID: 2706
		internal const string EpmSyndicationWriter_InvalidLinkLengthValue = "EpmSyndicationWriter_InvalidLinkLengthValue";

		// Token: 0x04000A93 RID: 2707
		internal const string EpmSyndicationWriter_InvalidValueForLinkRelCriteriaAttribute = "EpmSyndicationWriter_InvalidValueForLinkRelCriteriaAttribute";

		// Token: 0x04000A94 RID: 2708
		internal const string EpmSyndicationWriter_InvalidValueForCategorySchemeCriteriaAttribute = "EpmSyndicationWriter_InvalidValueForCategorySchemeCriteriaAttribute";

		// Token: 0x04000A95 RID: 2709
		internal const string ExpressionLexer_ExpectedLiteralToken = "ExpressionLexer_ExpectedLiteralToken";

		// Token: 0x04000A96 RID: 2710
		internal const string UriUtils_InvalidRelativeUriForEscaping = "UriUtils_InvalidRelativeUriForEscaping";

		// Token: 0x04000A97 RID: 2711
		internal const string ODataUriUtils_ConvertToUriLiteralUnsupportedType = "ODataUriUtils_ConvertToUriLiteralUnsupportedType";

		// Token: 0x04000A98 RID: 2712
		internal const string ODataUriUtils_ConvertToUriLiteralUnsupportedFormat = "ODataUriUtils_ConvertToUriLiteralUnsupportedFormat";

		// Token: 0x04000A99 RID: 2713
		internal const string ODataUriUtils_ConvertFromUriLiteralTypeRefWithoutModel = "ODataUriUtils_ConvertFromUriLiteralTypeRefWithoutModel";

		// Token: 0x04000A9A RID: 2714
		internal const string ODataUriUtils_ConvertFromUriLiteralTypeVerificationFailure = "ODataUriUtils_ConvertFromUriLiteralTypeVerificationFailure";

		// Token: 0x04000A9B RID: 2715
		internal const string ODataUriUtils_ConvertFromUriLiteralNullTypeVerificationFailure = "ODataUriUtils_ConvertFromUriLiteralNullTypeVerificationFailure";

		// Token: 0x04000A9C RID: 2716
		internal const string ODataUriUtils_ConvertFromUriLiteralNullOnNonNullableType = "ODataUriUtils_ConvertFromUriLiteralNullOnNonNullableType";

		// Token: 0x04000A9D RID: 2717
		internal const string ODataUtils_CannotConvertValueToRawPrimitive = "ODataUtils_CannotConvertValueToRawPrimitive";

		// Token: 0x04000A9E RID: 2718
		internal const string ODataUtils_DidNotFindDefaultMediaType = "ODataUtils_DidNotFindDefaultMediaType";

		// Token: 0x04000A9F RID: 2719
		internal const string ODataUtils_CannotSaveAnnotationsToBuiltInModel = "ODataUtils_CannotSaveAnnotationsToBuiltInModel";

		// Token: 0x04000AA0 RID: 2720
		internal const string ODataUtils_UnsupportedVersionHeader = "ODataUtils_UnsupportedVersionHeader";

		// Token: 0x04000AA1 RID: 2721
		internal const string ODataUtils_UnsupportedVersionNumber = "ODataUtils_UnsupportedVersionNumber";

		// Token: 0x04000AA2 RID: 2722
		internal const string ODataUtils_NullValueForMimeTypeAnnotation = "ODataUtils_NullValueForMimeTypeAnnotation";

		// Token: 0x04000AA3 RID: 2723
		internal const string ODataUtils_NullValueForHttpMethodAnnotation = "ODataUtils_NullValueForHttpMethodAnnotation";

		// Token: 0x04000AA4 RID: 2724
		internal const string ODataUtils_IsAlwaysBindableAnnotationSetForANonBindableFunctionImport = "ODataUtils_IsAlwaysBindableAnnotationSetForANonBindableFunctionImport";

		// Token: 0x04000AA5 RID: 2725
		internal const string ODataUtils_UnexpectedIsAlwaysBindableAnnotationInANonBindableFunctionImport = "ODataUtils_UnexpectedIsAlwaysBindableAnnotationInANonBindableFunctionImport";

		// Token: 0x04000AA6 RID: 2726
		internal const string ReaderUtils_EnumerableModified = "ReaderUtils_EnumerableModified";

		// Token: 0x04000AA7 RID: 2727
		internal const string ReaderValidationUtils_NullValueForNonNullableType = "ReaderValidationUtils_NullValueForNonNullableType";

		// Token: 0x04000AA8 RID: 2728
		internal const string ReaderValidationUtils_NullNamedValueForNonNullableType = "ReaderValidationUtils_NullNamedValueForNonNullableType";

		// Token: 0x04000AA9 RID: 2729
		internal const string ReaderValidationUtils_EntityReferenceLinkMissingUri = "ReaderValidationUtils_EntityReferenceLinkMissingUri";

		// Token: 0x04000AAA RID: 2730
		internal const string ReaderValidationUtils_ValueWithoutType = "ReaderValidationUtils_ValueWithoutType";

		// Token: 0x04000AAB RID: 2731
		internal const string ReaderValidationUtils_EntryWithoutType = "ReaderValidationUtils_EntryWithoutType";

		// Token: 0x04000AAC RID: 2732
		internal const string ReaderValidationUtils_DerivedComplexTypesAreNotAllowed = "ReaderValidationUtils_DerivedComplexTypesAreNotAllowed";

		// Token: 0x04000AAD RID: 2733
		internal const string ReaderValidationUtils_CannotConvertPrimitiveValue = "ReaderValidationUtils_CannotConvertPrimitiveValue";

		// Token: 0x04000AAE RID: 2734
		internal const string ReaderValidationUtils_MessageReaderSettingsBaseUriMustBeNullOrAbsolute = "ReaderValidationUtils_MessageReaderSettingsBaseUriMustBeNullOrAbsolute";

		// Token: 0x04000AAF RID: 2735
		internal const string ReaderValidationUtils_UndeclaredPropertyBehaviorKindSpecifiedOnRequest = "ReaderValidationUtils_UndeclaredPropertyBehaviorKindSpecifiedOnRequest";

		// Token: 0x04000AB0 RID: 2736
		internal const string ReaderValidationUtils_UndeclaredPropertyBehaviorKindSpecifiedForOpenType = "ReaderValidationUtils_UndeclaredPropertyBehaviorKindSpecifiedForOpenType";

		// Token: 0x04000AB1 RID: 2737
		internal const string ReaderValidationUtils_MetadataUriValidationInvalidExpectedEntitySet = "ReaderValidationUtils_MetadataUriValidationInvalidExpectedEntitySet";

		// Token: 0x04000AB2 RID: 2738
		internal const string ReaderValidationUtils_MetadataUriValidationInvalidExpectedEntityType = "ReaderValidationUtils_MetadataUriValidationInvalidExpectedEntityType";

		// Token: 0x04000AB3 RID: 2739
		internal const string ReaderValidationUtils_MetadataUriValidationNonMatchingPropertyNames = "ReaderValidationUtils_MetadataUriValidationNonMatchingPropertyNames";

		// Token: 0x04000AB4 RID: 2740
		internal const string ReaderValidationUtils_MetadataUriValidationNonMatchingDeclaringTypes = "ReaderValidationUtils_MetadataUriValidationNonMatchingDeclaringTypes";

		// Token: 0x04000AB5 RID: 2741
		internal const string ReaderValidationUtils_MetadataUriValidationNonMatchingCollectionNames = "ReaderValidationUtils_MetadataUriValidationNonMatchingCollectionNames";

		// Token: 0x04000AB6 RID: 2742
		internal const string ReaderValidationUtils_MetadataUriValidationNonMatchingCollectionItemTypes = "ReaderValidationUtils_MetadataUriValidationNonMatchingCollectionItemTypes";

		// Token: 0x04000AB7 RID: 2743
		internal const string ReaderValidationUtils_MetadataUriValidationPropertyWithExpectedFunctionImport = "ReaderValidationUtils_MetadataUriValidationPropertyWithExpectedFunctionImport";

		// Token: 0x04000AB8 RID: 2744
		internal const string ReaderValidationUtils_MetadataUriValidationFunctionImportWithExpectedProperty = "ReaderValidationUtils_MetadataUriValidationFunctionImportWithExpectedProperty";

		// Token: 0x04000AB9 RID: 2745
		internal const string ReaderValidationUtils_NonMatchingCollectionNames = "ReaderValidationUtils_NonMatchingCollectionNames";

		// Token: 0x04000ABA RID: 2746
		internal const string ReaderValidationUtils_NonMatchingPropertyNames = "ReaderValidationUtils_NonMatchingPropertyNames";

		// Token: 0x04000ABB RID: 2747
		internal const string ReaderValidationUtils_MetadataUriValidationNonMatchingPropertyDeclaringTypes = "ReaderValidationUtils_MetadataUriValidationNonMatchingPropertyDeclaringTypes";

		// Token: 0x04000ABC RID: 2748
		internal const string ReaderValidationUtils_MetadataUriValidationNonMatchingPropertyTypes = "ReaderValidationUtils_MetadataUriValidationNonMatchingPropertyTypes";

		// Token: 0x04000ABD RID: 2749
		internal const string ReaderValidationUtils_MetadataUriValidationNonMatchingFunctionImportNames = "ReaderValidationUtils_MetadataUriValidationNonMatchingFunctionImportNames";

		// Token: 0x04000ABE RID: 2750
		internal const string ReaderValidationUtils_MetadataUriValidationNonMatchingFunctionImportReturnTypes = "ReaderValidationUtils_MetadataUriValidationNonMatchingFunctionImportReturnTypes";

		// Token: 0x04000ABF RID: 2751
		internal const string ReaderValidationUtils_TypeInMetadataUriDoesNotMatchExpectedType = "ReaderValidationUtils_TypeInMetadataUriDoesNotMatchExpectedType";

		// Token: 0x04000AC0 RID: 2752
		internal const string ReaderValidationUtils_MetadataUriDoesNotReferTypeAssignableToExpectedType = "ReaderValidationUtils_MetadataUriDoesNotReferTypeAssignableToExpectedType";

		// Token: 0x04000AC1 RID: 2753
		internal const string ODataMessageReader_ReaderAlreadyUsed = "ODataMessageReader_ReaderAlreadyUsed";

		// Token: 0x04000AC2 RID: 2754
		internal const string ODataMessageReader_ErrorPayloadInRequest = "ODataMessageReader_ErrorPayloadInRequest";

		// Token: 0x04000AC3 RID: 2755
		internal const string ODataMessageReader_ServiceDocumentInRequest = "ODataMessageReader_ServiceDocumentInRequest";

		// Token: 0x04000AC4 RID: 2756
		internal const string ODataMessageReader_MetadataDocumentInRequest = "ODataMessageReader_MetadataDocumentInRequest";

		// Token: 0x04000AC5 RID: 2757
		internal const string ODataMessageReader_ExpectedTypeSpecifiedWithoutMetadata = "ODataMessageReader_ExpectedTypeSpecifiedWithoutMetadata";

		// Token: 0x04000AC6 RID: 2758
		internal const string ODataMessageReader_EntitySetSpecifiedWithoutMetadata = "ODataMessageReader_EntitySetSpecifiedWithoutMetadata";

		// Token: 0x04000AC7 RID: 2759
		internal const string ODataMessageReader_FunctionImportSpecifiedWithoutMetadata = "ODataMessageReader_FunctionImportSpecifiedWithoutMetadata";

		// Token: 0x04000AC8 RID: 2760
		internal const string ODataMessageReader_ProducingFunctionImportNonCollectionType = "ODataMessageReader_ProducingFunctionImportNonCollectionType";

		// Token: 0x04000AC9 RID: 2761
		internal const string ODataMessageReader_ExpectedCollectionTypeWrongKind = "ODataMessageReader_ExpectedCollectionTypeWrongKind";

		// Token: 0x04000ACA RID: 2762
		internal const string ODataMessageReader_ExpectedPropertyTypeEntityCollectionKind = "ODataMessageReader_ExpectedPropertyTypeEntityCollectionKind";

		// Token: 0x04000ACB RID: 2763
		internal const string ODataMessageReader_ExpectedPropertyTypeEntityKind = "ODataMessageReader_ExpectedPropertyTypeEntityKind";

		// Token: 0x04000ACC RID: 2764
		internal const string ODataMessageReader_ExpectedPropertyTypeStream = "ODataMessageReader_ExpectedPropertyTypeStream";

		// Token: 0x04000ACD RID: 2765
		internal const string ODataMessageReader_ExpectedValueTypeWrongKind = "ODataMessageReader_ExpectedValueTypeWrongKind";

		// Token: 0x04000ACE RID: 2766
		internal const string ODataMessageReader_NoneOrEmptyContentTypeHeader = "ODataMessageReader_NoneOrEmptyContentTypeHeader";

		// Token: 0x04000ACF RID: 2767
		internal const string ODataMessageReader_WildcardInContentType = "ODataMessageReader_WildcardInContentType";

		// Token: 0x04000AD0 RID: 2768
		internal const string ODataMessageReader_EntityReferenceLinksInRequestNotAllowed = "ODataMessageReader_EntityReferenceLinksInRequestNotAllowed";

		// Token: 0x04000AD1 RID: 2769
		internal const string ODataMessageReader_GetFormatCalledBeforeReadingStarted = "ODataMessageReader_GetFormatCalledBeforeReadingStarted";

		// Token: 0x04000AD2 RID: 2770
		internal const string ODataMessageReader_DetectPayloadKindMultipleTimes = "ODataMessageReader_DetectPayloadKindMultipleTimes";

		// Token: 0x04000AD3 RID: 2771
		internal const string ODataMessageReader_PayloadKindDetectionRunning = "ODataMessageReader_PayloadKindDetectionRunning";

		// Token: 0x04000AD4 RID: 2772
		internal const string ODataMessageReader_PayloadKindDetectionInServerMode = "ODataMessageReader_PayloadKindDetectionInServerMode";

		// Token: 0x04000AD5 RID: 2773
		internal const string ODataMessageReader_ParameterPayloadInResponse = "ODataMessageReader_ParameterPayloadInResponse";

		// Token: 0x04000AD6 RID: 2774
		internal const string ODataMessageReader_SingletonNavigationPropertyForEntityReferenceLinks = "ODataMessageReader_SingletonNavigationPropertyForEntityReferenceLinks";

		// Token: 0x04000AD7 RID: 2775
		internal const string ODataMessage_MustNotModifyMessage = "ODataMessage_MustNotModifyMessage";

		// Token: 0x04000AD8 RID: 2776
		internal const string ODataMediaTypeUtils_BoundaryMustBeSpecifiedForBatchPayloads = "ODataMediaTypeUtils_BoundaryMustBeSpecifiedForBatchPayloads";

		// Token: 0x04000AD9 RID: 2777
		internal const string ODataReaderCore_SyncCallOnAsyncReader = "ODataReaderCore_SyncCallOnAsyncReader";

		// Token: 0x04000ADA RID: 2778
		internal const string ODataReaderCore_AsyncCallOnSyncReader = "ODataReaderCore_AsyncCallOnSyncReader";

		// Token: 0x04000ADB RID: 2779
		internal const string ODataReaderCore_ReadOrReadAsyncCalledInInvalidState = "ODataReaderCore_ReadOrReadAsyncCalledInInvalidState";

		// Token: 0x04000ADC RID: 2780
		internal const string ODataReaderCore_NoReadCallsAllowed = "ODataReaderCore_NoReadCallsAllowed";

		// Token: 0x04000ADD RID: 2781
		internal const string ODataJsonReader_CannotReadEntriesOfFeed = "ODataJsonReader_CannotReadEntriesOfFeed";

		// Token: 0x04000ADE RID: 2782
		internal const string ODataJsonReader_CannotReadFeedStart = "ODataJsonReader_CannotReadFeedStart";

		// Token: 0x04000ADF RID: 2783
		internal const string ODataJsonReader_CannotReadEntryStart = "ODataJsonReader_CannotReadEntryStart";

		// Token: 0x04000AE0 RID: 2784
		internal const string ODataJsonReader_ParsingWithoutMetadata = "ODataJsonReader_ParsingWithoutMetadata";

		// Token: 0x04000AE1 RID: 2785
		internal const string ODataJsonReaderUtils_CannotConvertInt64OrDecimal = "ODataJsonReaderUtils_CannotConvertInt64OrDecimal";

		// Token: 0x04000AE2 RID: 2786
		internal const string ODataJsonReaderUtils_CannotConvertInt32 = "ODataJsonReaderUtils_CannotConvertInt32";

		// Token: 0x04000AE3 RID: 2787
		internal const string ODataJsonReaderUtils_CannotConvertDouble = "ODataJsonReaderUtils_CannotConvertDouble";

		// Token: 0x04000AE4 RID: 2788
		internal const string ODataJsonReaderUtils_CannotConvertBoolean = "ODataJsonReaderUtils_CannotConvertBoolean";

		// Token: 0x04000AE5 RID: 2789
		internal const string ODataJsonReaderUtils_CannotConvertDateTime = "ODataJsonReaderUtils_CannotConvertDateTime";

		// Token: 0x04000AE6 RID: 2790
		internal const string ODataJsonReaderUtils_CannotConvertDateTimeOffset = "ODataJsonReaderUtils_CannotConvertDateTimeOffset";

		// Token: 0x04000AE7 RID: 2791
		internal const string ODataJsonReaderUtils_MultipleMetadataPropertiesWithSameName = "ODataJsonReaderUtils_MultipleMetadataPropertiesWithSameName";

		// Token: 0x04000AE8 RID: 2792
		internal const string ODataJsonReaderUtils_MultipleEntityReferenceLinksWrapperPropertiesWithSameName = "ODataJsonReaderUtils_MultipleEntityReferenceLinksWrapperPropertiesWithSameName";

		// Token: 0x04000AE9 RID: 2793
		internal const string ODataJsonReaderUtils_MultipleErrorPropertiesWithSameName = "ODataJsonReaderUtils_MultipleErrorPropertiesWithSameName";

		// Token: 0x04000AEA RID: 2794
		internal const string ODataJsonReaderUtils_FeedPropertyWithNullValue = "ODataJsonReaderUtils_FeedPropertyWithNullValue";

		// Token: 0x04000AEB RID: 2795
		internal const string ODataJsonReaderUtils_MediaResourcePropertyWithNullValue = "ODataJsonReaderUtils_MediaResourcePropertyWithNullValue";

		// Token: 0x04000AEC RID: 2796
		internal const string ODataJsonReaderUtils_EntityReferenceLinksInlineCountWithNullValue = "ODataJsonReaderUtils_EntityReferenceLinksInlineCountWithNullValue";

		// Token: 0x04000AED RID: 2797
		internal const string ODataJsonReaderUtils_EntityReferenceLinksPropertyWithNullValue = "ODataJsonReaderUtils_EntityReferenceLinksPropertyWithNullValue";

		// Token: 0x04000AEE RID: 2798
		internal const string ODataJsonReaderUtils_MetadataPropertyWithNullValue = "ODataJsonReaderUtils_MetadataPropertyWithNullValue";

		// Token: 0x04000AEF RID: 2799
		internal const string ODataJsonDeserializer_DataWrapperPropertyNotFound = "ODataJsonDeserializer_DataWrapperPropertyNotFound";

		// Token: 0x04000AF0 RID: 2800
		internal const string ODataJsonDeserializer_DataWrapperMultipleProperties = "ODataJsonDeserializer_DataWrapperMultipleProperties";

		// Token: 0x04000AF1 RID: 2801
		internal const string ODataJsonDeserializer_RelativeUriUsedWithoutBaseUriSpecified = "ODataJsonDeserializer_RelativeUriUsedWithoutBaseUriSpecified";

		// Token: 0x04000AF2 RID: 2802
		internal const string ODataJsonCollectionDeserializer_MissingResultsPropertyForCollection = "ODataJsonCollectionDeserializer_MissingResultsPropertyForCollection";

		// Token: 0x04000AF3 RID: 2803
		internal const string ODataJsonCollectionDeserializer_CannotReadCollectionContentStart = "ODataJsonCollectionDeserializer_CannotReadCollectionContentStart";

		// Token: 0x04000AF4 RID: 2804
		internal const string ODataJsonCollectionDeserializer_MultipleResultsPropertiesForCollection = "ODataJsonCollectionDeserializer_MultipleResultsPropertiesForCollection";

		// Token: 0x04000AF5 RID: 2805
		internal const string ODataJsonEntityReferenceLinkDeserializer_ExpectedEntityReferenceLinksResultsPropertyNotFound = "ODataJsonEntityReferenceLinkDeserializer_ExpectedEntityReferenceLinksResultsPropertyNotFound";

		// Token: 0x04000AF6 RID: 2806
		internal const string ODataJsonEntityReferenceLinkDeserializer_EntityReferenceLinkMustBeObjectValue = "ODataJsonEntityReferenceLinkDeserializer_EntityReferenceLinkMustBeObjectValue";

		// Token: 0x04000AF7 RID: 2807
		internal const string ODataJsonEntityReferenceLinkDeserializer_MultipleUriPropertiesInEntityReferenceLink = "ODataJsonEntityReferenceLinkDeserializer_MultipleUriPropertiesInEntityReferenceLink";

		// Token: 0x04000AF8 RID: 2808
		internal const string ODataJsonEntityReferenceLinkDeserializer_EntityReferenceLinkUriCannotBeNull = "ODataJsonEntityReferenceLinkDeserializer_EntityReferenceLinkUriCannotBeNull";

		// Token: 0x04000AF9 RID: 2809
		internal const string ODataJsonEntryAndFeedDeserializer_ExpectedFeedResultsPropertyNotFound = "ODataJsonEntryAndFeedDeserializer_ExpectedFeedResultsPropertyNotFound";

		// Token: 0x04000AFA RID: 2810
		internal const string ODataJsonEntryAndFeedDeserializer_CannotReadFeedContentStart = "ODataJsonEntryAndFeedDeserializer_CannotReadFeedContentStart";

		// Token: 0x04000AFB RID: 2811
		internal const string ODataJsonEntryAndFeedDeserializer_MultipleMetadataPropertiesInEntryValue = "ODataJsonEntryAndFeedDeserializer_MultipleMetadataPropertiesInEntryValue";

		// Token: 0x04000AFC RID: 2812
		internal const string ODataJsonEntryAndFeedDeserializer_MultipleUriPropertiesInDeferredLink = "ODataJsonEntryAndFeedDeserializer_MultipleUriPropertiesInDeferredLink";

		// Token: 0x04000AFD RID: 2813
		internal const string ODataJsonEntryAndFeedDeserializer_DeferredLinkUriCannotBeNull = "ODataJsonEntryAndFeedDeserializer_DeferredLinkUriCannotBeNull";

		// Token: 0x04000AFE RID: 2814
		internal const string ODataJsonEntryAndFeedDeserializer_DeferredLinkMissingUri = "ODataJsonEntryAndFeedDeserializer_DeferredLinkMissingUri";

		// Token: 0x04000AFF RID: 2815
		internal const string ODataJsonEntryAndFeedDeserializer_CannotReadNavigationPropertyValue = "ODataJsonEntryAndFeedDeserializer_CannotReadNavigationPropertyValue";

		// Token: 0x04000B00 RID: 2816
		internal const string ODataJsonEntryAndFeedDeserializer_MultipleFeedResultsPropertiesFound = "ODataJsonEntryAndFeedDeserializer_MultipleFeedResultsPropertiesFound";

		// Token: 0x04000B01 RID: 2817
		internal const string ODataJsonEntryAndFeedDeserializer_MultipleMetadataPropertiesForStreamProperty = "ODataJsonEntryAndFeedDeserializer_MultipleMetadataPropertiesForStreamProperty";

		// Token: 0x04000B02 RID: 2818
		internal const string ODataJsonEntryAndFeedDeserializer_CannotParseStreamReference = "ODataJsonEntryAndFeedDeserializer_CannotParseStreamReference";

		// Token: 0x04000B03 RID: 2819
		internal const string ODataJsonEntryAndFeedDeserializer_PropertyInEntryMustHaveObjectValue = "ODataJsonEntryAndFeedDeserializer_PropertyInEntryMustHaveObjectValue";

		// Token: 0x04000B04 RID: 2820
		internal const string ODataJsonEntryAndFeedDeserializer_CannotReadSingletonNavigationPropertyValue = "ODataJsonEntryAndFeedDeserializer_CannotReadSingletonNavigationPropertyValue";

		// Token: 0x04000B05 RID: 2821
		internal const string ODataJsonEntryAndFeedDeserializer_CannotReadCollectionNavigationPropertyValue = "ODataJsonEntryAndFeedDeserializer_CannotReadCollectionNavigationPropertyValue";

		// Token: 0x04000B06 RID: 2822
		internal const string ODataJsonEntryAndFeedDeserializer_StreamPropertyInRequest = "ODataJsonEntryAndFeedDeserializer_StreamPropertyInRequest";

		// Token: 0x04000B07 RID: 2823
		internal const string ODataJsonLightEntryAndFeedSerializer_AnnotationGroupWithoutName = "ODataJsonLightEntryAndFeedSerializer_AnnotationGroupWithoutName";

		// Token: 0x04000B08 RID: 2824
		internal const string ODataJsonLightEntryAndFeedSerializer_AnnotationGroupMemberWithoutName = "ODataJsonLightEntryAndFeedSerializer_AnnotationGroupMemberWithoutName";

		// Token: 0x04000B09 RID: 2825
		internal const string ODataJsonLightEntryAndFeedSerializer_AnnotationGroupMemberWithInvalidValue = "ODataJsonLightEntryAndFeedSerializer_AnnotationGroupMemberWithInvalidValue";

		// Token: 0x04000B0A RID: 2826
		internal const string ODataJsonLightEntryAndFeedSerializer_AnnotationGroupInRequest = "ODataJsonLightEntryAndFeedSerializer_AnnotationGroupInRequest";

		// Token: 0x04000B0B RID: 2827
		internal const string ODataJsonLightEntryAndFeedSerializer_AnnotationGroupMemberMustBeAnnotation = "ODataJsonLightEntryAndFeedSerializer_AnnotationGroupMemberMustBeAnnotation";

		// Token: 0x04000B0C RID: 2828
		internal const string ODataJsonLightEntryAndFeedSerializer_DuplicateAnnotationGroup = "ODataJsonLightEntryAndFeedSerializer_DuplicateAnnotationGroup";

		// Token: 0x04000B0D RID: 2829
		internal const string ODataJsonLightEntryAndFeedSerializer_ActionsAndFunctionsGroupMustSpecifyTarget = "ODataJsonLightEntryAndFeedSerializer_ActionsAndFunctionsGroupMustSpecifyTarget";

		// Token: 0x04000B0E RID: 2830
		internal const string ODataJsonLightEntryAndFeedSerializer_ActionsAndFunctionsGroupMustNotHaveDuplicateTarget = "ODataJsonLightEntryAndFeedSerializer_ActionsAndFunctionsGroupMustNotHaveDuplicateTarget";

		// Token: 0x04000B0F RID: 2831
		internal const string ODataJsonErrorDeserializer_TopLevelErrorWithInvalidProperty = "ODataJsonErrorDeserializer_TopLevelErrorWithInvalidProperty";

		// Token: 0x04000B10 RID: 2832
		internal const string ODataJsonErrorDeserializer_TopLevelErrorMessageValueWithInvalidProperty = "ODataJsonErrorDeserializer_TopLevelErrorMessageValueWithInvalidProperty";

		// Token: 0x04000B11 RID: 2833
		internal const string ODataVerboseJsonErrorDeserializer_TopLevelErrorValueWithInvalidProperty = "ODataVerboseJsonErrorDeserializer_TopLevelErrorValueWithInvalidProperty";

		// Token: 0x04000B12 RID: 2834
		internal const string ODataJsonPropertyAndValueDeserializer_TopLevelPropertyWithoutMetadata = "ODataJsonPropertyAndValueDeserializer_TopLevelPropertyWithoutMetadata";

		// Token: 0x04000B13 RID: 2835
		internal const string ODataJsonPropertyAndValueDeserializer_InvalidTopLevelPropertyPayload = "ODataJsonPropertyAndValueDeserializer_InvalidTopLevelPropertyPayload";

		// Token: 0x04000B14 RID: 2836
		internal const string ODataJsonPropertyAndValueDeserializer_CannotReadPropertyValue = "ODataJsonPropertyAndValueDeserializer_CannotReadPropertyValue";

		// Token: 0x04000B15 RID: 2837
		internal const string ODataJsonPropertyAndValueDeserializer_MultipleMetadataPropertiesInComplexValue = "ODataJsonPropertyAndValueDeserializer_MultipleMetadataPropertiesInComplexValue";

		// Token: 0x04000B16 RID: 2838
		internal const string ODataJsonPropertyAndValueDeserializer_MultiplePropertiesInCollectionWrapper = "ODataJsonPropertyAndValueDeserializer_MultiplePropertiesInCollectionWrapper";

		// Token: 0x04000B17 RID: 2839
		internal const string ODataJsonPropertyAndValueDeserializer_CollectionWithoutResults = "ODataJsonPropertyAndValueDeserializer_CollectionWithoutResults";

		// Token: 0x04000B18 RID: 2840
		internal const string ODataJsonPropertyAndValueDeserializer_InvalidTypeName = "ODataJsonPropertyAndValueDeserializer_InvalidTypeName";

		// Token: 0x04000B19 RID: 2841
		internal const string ODataJsonPropertyAndValueDeserializer_InvalidPrimitiveTypeName = "ODataJsonPropertyAndValueDeserializer_InvalidPrimitiveTypeName";

		// Token: 0x04000B1A RID: 2842
		internal const string ODataJsonPropertyAndValueDeserializer_MetadataPropertyMustHaveObjectValue = "ODataJsonPropertyAndValueDeserializer_MetadataPropertyMustHaveObjectValue";

		// Token: 0x04000B1B RID: 2843
		internal const string ODataJsonServiceDocumentDeserializer_MultipleEntitySetsPropertiesForServiceDocument = "ODataJsonServiceDocumentDeserializer_MultipleEntitySetsPropertiesForServiceDocument";

		// Token: 0x04000B1C RID: 2844
		internal const string ODataJsonServiceDocumentDeserializer_NoEntitySetsPropertyForServiceDocument = "ODataJsonServiceDocumentDeserializer_NoEntitySetsPropertyForServiceDocument";

		// Token: 0x04000B1D RID: 2845
		internal const string ODataCollectionReaderCore_ReadOrReadAsyncCalledInInvalidState = "ODataCollectionReaderCore_ReadOrReadAsyncCalledInInvalidState";

		// Token: 0x04000B1E RID: 2846
		internal const string ODataCollectionReaderCore_SyncCallOnAsyncReader = "ODataCollectionReaderCore_SyncCallOnAsyncReader";

		// Token: 0x04000B1F RID: 2847
		internal const string ODataCollectionReaderCore_AsyncCallOnSyncReader = "ODataCollectionReaderCore_AsyncCallOnSyncReader";

		// Token: 0x04000B20 RID: 2848
		internal const string ODataCollectionReaderCore_ExpectedItemTypeSetInInvalidState = "ODataCollectionReaderCore_ExpectedItemTypeSetInInvalidState";

		// Token: 0x04000B21 RID: 2849
		internal const string ODataParameterReaderCore_ReadOrReadAsyncCalledInInvalidState = "ODataParameterReaderCore_ReadOrReadAsyncCalledInInvalidState";

		// Token: 0x04000B22 RID: 2850
		internal const string ODataParameterReaderCore_SyncCallOnAsyncReader = "ODataParameterReaderCore_SyncCallOnAsyncReader";

		// Token: 0x04000B23 RID: 2851
		internal const string ODataParameterReaderCore_AsyncCallOnSyncReader = "ODataParameterReaderCore_AsyncCallOnSyncReader";

		// Token: 0x04000B24 RID: 2852
		internal const string ODataParameterReaderCore_SubReaderMustBeCreatedAndReadToCompletionBeforeTheNextReadOrReadAsyncCall = "ODataParameterReaderCore_SubReaderMustBeCreatedAndReadToCompletionBeforeTheNextReadOrReadAsyncCall";

		// Token: 0x04000B25 RID: 2853
		internal const string ODataParameterReaderCore_SubReaderMustBeInCompletedStateBeforeTheNextReadOrReadAsyncCall = "ODataParameterReaderCore_SubReaderMustBeInCompletedStateBeforeTheNextReadOrReadAsyncCall";

		// Token: 0x04000B26 RID: 2854
		internal const string ODataParameterReaderCore_InvalidCreateReaderMethodCalledForState = "ODataParameterReaderCore_InvalidCreateReaderMethodCalledForState";

		// Token: 0x04000B27 RID: 2855
		internal const string ODataParameterReaderCore_CreateReaderAlreadyCalled = "ODataParameterReaderCore_CreateReaderAlreadyCalled";

		// Token: 0x04000B28 RID: 2856
		internal const string ODataParameterReaderCore_ParameterNameNotInMetadata = "ODataParameterReaderCore_ParameterNameNotInMetadata";

		// Token: 0x04000B29 RID: 2857
		internal const string ODataParameterReaderCore_DuplicateParametersInPayload = "ODataParameterReaderCore_DuplicateParametersInPayload";

		// Token: 0x04000B2A RID: 2858
		internal const string ODataParameterReaderCore_ParametersMissingInPayload = "ODataParameterReaderCore_ParametersMissingInPayload";

		// Token: 0x04000B2B RID: 2859
		internal const string ODataJsonParameterReader_UnsupportedPrimitiveParameterType = "ODataJsonParameterReader_UnsupportedPrimitiveParameterType";

		// Token: 0x04000B2C RID: 2860
		internal const string ODataJsonParameterReader_UnsupportedParameterTypeKind = "ODataJsonParameterReader_UnsupportedParameterTypeKind";

		// Token: 0x04000B2D RID: 2861
		internal const string ODataJsonParameterReader_NullCollectionExpected = "ODataJsonParameterReader_NullCollectionExpected";

		// Token: 0x04000B2E RID: 2862
		internal const string ODataJsonInputContext_FunctionImportCannotBeNullForCreateParameterReader = "ODataJsonInputContext_FunctionImportCannotBeNullForCreateParameterReader";

		// Token: 0x04000B2F RID: 2863
		internal const string ODataJsonCollectionReader_CannotReadWrappedCollectionStart = "ODataJsonCollectionReader_CannotReadWrappedCollectionStart";

		// Token: 0x04000B30 RID: 2864
		internal const string ODataJsonCollectionReader_CannotReadCollectionStart = "ODataJsonCollectionReader_CannotReadCollectionStart";

		// Token: 0x04000B31 RID: 2865
		internal const string ODataJsonCollectionReader_ParsingWithoutMetadata = "ODataJsonCollectionReader_ParsingWithoutMetadata";

		// Token: 0x04000B32 RID: 2866
		internal const string ValidationUtils_ActionsAndFunctionsMustSpecifyMetadata = "ValidationUtils_ActionsAndFunctionsMustSpecifyMetadata";

		// Token: 0x04000B33 RID: 2867
		internal const string ValidationUtils_ActionsAndFunctionsMustSpecifyTarget = "ValidationUtils_ActionsAndFunctionsMustSpecifyTarget";

		// Token: 0x04000B34 RID: 2868
		internal const string ValidationUtils_EnumerableContainsANullItem = "ValidationUtils_EnumerableContainsANullItem";

		// Token: 0x04000B35 RID: 2869
		internal const string ValidationUtils_AssociationLinkMustSpecifyName = "ValidationUtils_AssociationLinkMustSpecifyName";

		// Token: 0x04000B36 RID: 2870
		internal const string ValidationUtils_AssociationLinkMustSpecifyUrl = "ValidationUtils_AssociationLinkMustSpecifyUrl";

		// Token: 0x04000B37 RID: 2871
		internal const string ValidationUtils_TypeNameMustNotBeEmpty = "ValidationUtils_TypeNameMustNotBeEmpty";

		// Token: 0x04000B38 RID: 2872
		internal const string ValidationUtils_PropertyDoesNotExistOnType = "ValidationUtils_PropertyDoesNotExistOnType";

		// Token: 0x04000B39 RID: 2873
		internal const string ValidationUtils_ResourceCollectionMustSpecifyUrl = "ValidationUtils_ResourceCollectionMustSpecifyUrl";

		// Token: 0x04000B3A RID: 2874
		internal const string ValidationUtils_ResourceCollectionUrlMustNotBeNull = "ValidationUtils_ResourceCollectionUrlMustNotBeNull";

		// Token: 0x04000B3B RID: 2875
		internal const string ValidationUtils_NonPrimitiveTypeForPrimitiveValue = "ValidationUtils_NonPrimitiveTypeForPrimitiveValue";

		// Token: 0x04000B3C RID: 2876
		internal const string ValidationUtils_UnsupportedPrimitiveType = "ValidationUtils_UnsupportedPrimitiveType";

		// Token: 0x04000B3D RID: 2877
		internal const string ValidationUtils_IncompatiblePrimitiveItemType = "ValidationUtils_IncompatiblePrimitiveItemType";

		// Token: 0x04000B3E RID: 2878
		internal const string ValidationUtils_NonStreamingCollectionElementsMustNotBeNull = "ValidationUtils_NonStreamingCollectionElementsMustNotBeNull";

		// Token: 0x04000B3F RID: 2879
		internal const string ValidationUtils_InvalidCollectionTypeName = "ValidationUtils_InvalidCollectionTypeName";

		// Token: 0x04000B40 RID: 2880
		internal const string ValidationUtils_UnrecognizedTypeName = "ValidationUtils_UnrecognizedTypeName";

		// Token: 0x04000B41 RID: 2881
		internal const string ValidationUtils_IncorrectTypeKind = "ValidationUtils_IncorrectTypeKind";

		// Token: 0x04000B42 RID: 2882
		internal const string ValidationUtils_IncorrectTypeKindNoTypeName = "ValidationUtils_IncorrectTypeKindNoTypeName";

		// Token: 0x04000B43 RID: 2883
		internal const string ValidationUtils_IncorrectValueTypeKind = "ValidationUtils_IncorrectValueTypeKind";

		// Token: 0x04000B44 RID: 2884
		internal const string ValidationUtils_LinkMustSpecifyName = "ValidationUtils_LinkMustSpecifyName";

		// Token: 0x04000B45 RID: 2885
		internal const string ValidationUtils_MismatchPropertyKindForStreamProperty = "ValidationUtils_MismatchPropertyKindForStreamProperty";

		// Token: 0x04000B46 RID: 2886
		internal const string ValidationUtils_InvalidEtagValue = "ValidationUtils_InvalidEtagValue";

		// Token: 0x04000B47 RID: 2887
		internal const string ValidationUtils_NestedCollectionsAreNotSupported = "ValidationUtils_NestedCollectionsAreNotSupported";

		// Token: 0x04000B48 RID: 2888
		internal const string ValidationUtils_StreamReferenceValuesNotSupportedInCollections = "ValidationUtils_StreamReferenceValuesNotSupportedInCollections";

		// Token: 0x04000B49 RID: 2889
		internal const string ValidationUtils_IncompatibleType = "ValidationUtils_IncompatibleType";

		// Token: 0x04000B4A RID: 2890
		internal const string ValidationUtils_OpenCollectionProperty = "ValidationUtils_OpenCollectionProperty";

		// Token: 0x04000B4B RID: 2891
		internal const string ValidationUtils_OpenStreamProperty = "ValidationUtils_OpenStreamProperty";

		// Token: 0x04000B4C RID: 2892
		internal const string ValidationUtils_InvalidCollectionTypeReference = "ValidationUtils_InvalidCollectionTypeReference";

		// Token: 0x04000B4D RID: 2893
		internal const string ValidationUtils_EntryWithMediaResourceAndNonMLEType = "ValidationUtils_EntryWithMediaResourceAndNonMLEType";

		// Token: 0x04000B4E RID: 2894
		internal const string ValidationUtils_EntryWithoutMediaResourceAndMLEType = "ValidationUtils_EntryWithoutMediaResourceAndMLEType";

		// Token: 0x04000B4F RID: 2895
		internal const string ValidationUtils_EntryTypeNotAssignableToExpectedType = "ValidationUtils_EntryTypeNotAssignableToExpectedType";

		// Token: 0x04000B50 RID: 2896
		internal const string ValidationUtils_OpenNavigationProperty = "ValidationUtils_OpenNavigationProperty";

		// Token: 0x04000B51 RID: 2897
		internal const string ValidationUtils_NavigationPropertyExpected = "ValidationUtils_NavigationPropertyExpected";

		// Token: 0x04000B52 RID: 2898
		internal const string ValidationUtils_InvalidBatchBoundaryDelimiterLength = "ValidationUtils_InvalidBatchBoundaryDelimiterLength";

		// Token: 0x04000B53 RID: 2899
		internal const string ValidationUtils_RecursionDepthLimitReached = "ValidationUtils_RecursionDepthLimitReached";

		// Token: 0x04000B54 RID: 2900
		internal const string ValidationUtils_MaxDepthOfNestedEntriesExceeded = "ValidationUtils_MaxDepthOfNestedEntriesExceeded";

		// Token: 0x04000B55 RID: 2901
		internal const string ValidationUtils_NullCollectionItemForNonNullableType = "ValidationUtils_NullCollectionItemForNonNullableType";

		// Token: 0x04000B56 RID: 2902
		internal const string ValidationUtils_PropertiesMustNotContainReservedChars = "ValidationUtils_PropertiesMustNotContainReservedChars";

		// Token: 0x04000B57 RID: 2903
		internal const string ValidationUtils_MaxNumberOfEntityPropertyMappingsExceeded = "ValidationUtils_MaxNumberOfEntityPropertyMappingsExceeded";

		// Token: 0x04000B58 RID: 2904
		internal const string ValidationUtils_WorkspaceCollectionsMustNotContainNullItem = "ValidationUtils_WorkspaceCollectionsMustNotContainNullItem";

		// Token: 0x04000B59 RID: 2905
		internal const string ValidationUtils_InvalidMetadataReferenceProperty = "ValidationUtils_InvalidMetadataReferenceProperty";

		// Token: 0x04000B5A RID: 2906
		internal const string ODataAtomWriter_FeedsMustHaveNonEmptyId = "ODataAtomWriter_FeedsMustHaveNonEmptyId";

		// Token: 0x04000B5B RID: 2907
		internal const string WriterValidationUtils_PropertyMustNotBeNull = "WriterValidationUtils_PropertyMustNotBeNull";

		// Token: 0x04000B5C RID: 2908
		internal const string WriterValidationUtils_PropertiesMustHaveNonEmptyName = "WriterValidationUtils_PropertiesMustHaveNonEmptyName";

		// Token: 0x04000B5D RID: 2909
		internal const string WriterValidationUtils_PropertyNameDoesntMatchFunctionImportName = "WriterValidationUtils_PropertyNameDoesntMatchFunctionImportName";

		// Token: 0x04000B5E RID: 2910
		internal const string WriterValidationUtils_MissingTypeNameWithMetadata = "WriterValidationUtils_MissingTypeNameWithMetadata";

		// Token: 0x04000B5F RID: 2911
		internal const string WriterValidationUtils_NextPageLinkInRequest = "WriterValidationUtils_NextPageLinkInRequest";

		// Token: 0x04000B60 RID: 2912
		internal const string WriterValidationUtils_ResourceCollectionMustHaveUniqueName = "WriterValidationUtils_ResourceCollectionMustHaveUniqueName";

		// Token: 0x04000B61 RID: 2913
		internal const string WriterValidationUtils_DefaultStreamWithContentTypeWithoutReadLink = "WriterValidationUtils_DefaultStreamWithContentTypeWithoutReadLink";

		// Token: 0x04000B62 RID: 2914
		internal const string WriterValidationUtils_DefaultStreamWithReadLinkWithoutContentType = "WriterValidationUtils_DefaultStreamWithReadLinkWithoutContentType";

		// Token: 0x04000B63 RID: 2915
		internal const string WriterValidationUtils_StreamReferenceValueMustHaveEditLinkOrReadLink = "WriterValidationUtils_StreamReferenceValueMustHaveEditLinkOrReadLink";

		// Token: 0x04000B64 RID: 2916
		internal const string WriterValidationUtils_StreamReferenceValueMustHaveEditLinkToHaveETag = "WriterValidationUtils_StreamReferenceValueMustHaveEditLinkToHaveETag";

		// Token: 0x04000B65 RID: 2917
		internal const string WriterValidationUtils_StreamReferenceValueEmptyContentType = "WriterValidationUtils_StreamReferenceValueEmptyContentType";

		// Token: 0x04000B66 RID: 2918
		internal const string WriterValidationUtils_EntriesMustHaveNonEmptyId = "WriterValidationUtils_EntriesMustHaveNonEmptyId";

		// Token: 0x04000B67 RID: 2919
		internal const string WriterValidationUtils_MessageWriterSettingsBaseUriMustBeNullOrAbsolute = "WriterValidationUtils_MessageWriterSettingsBaseUriMustBeNullOrAbsolute";

		// Token: 0x04000B68 RID: 2920
		internal const string WriterValidationUtils_EntityReferenceLinkUrlMustNotBeNull = "WriterValidationUtils_EntityReferenceLinkUrlMustNotBeNull";

		// Token: 0x04000B69 RID: 2921
		internal const string WriterValidationUtils_EntityReferenceLinksLinkMustNotBeNull = "WriterValidationUtils_EntityReferenceLinksLinkMustNotBeNull";

		// Token: 0x04000B6A RID: 2922
		internal const string WriterValidationUtils_EntryTypeInExpandedLinkNotCompatibleWithNavigationPropertyType = "WriterValidationUtils_EntryTypeInExpandedLinkNotCompatibleWithNavigationPropertyType";

		// Token: 0x04000B6B RID: 2923
		internal const string WriterValidationUtils_ExpandedLinkIsCollectionTrueWithEntryContent = "WriterValidationUtils_ExpandedLinkIsCollectionTrueWithEntryContent";

		// Token: 0x04000B6C RID: 2924
		internal const string WriterValidationUtils_ExpandedLinkIsCollectionFalseWithFeedContent = "WriterValidationUtils_ExpandedLinkIsCollectionFalseWithFeedContent";

		// Token: 0x04000B6D RID: 2925
		internal const string WriterValidationUtils_ExpandedLinkIsCollectionTrueWithEntryMetadata = "WriterValidationUtils_ExpandedLinkIsCollectionTrueWithEntryMetadata";

		// Token: 0x04000B6E RID: 2926
		internal const string WriterValidationUtils_ExpandedLinkIsCollectionFalseWithFeedMetadata = "WriterValidationUtils_ExpandedLinkIsCollectionFalseWithFeedMetadata";

		// Token: 0x04000B6F RID: 2927
		internal const string WriterValidationUtils_ExpandedLinkWithFeedPayloadAndEntryMetadata = "WriterValidationUtils_ExpandedLinkWithFeedPayloadAndEntryMetadata";

		// Token: 0x04000B70 RID: 2928
		internal const string WriterValidationUtils_ExpandedLinkWithEntryPayloadAndFeedMetadata = "WriterValidationUtils_ExpandedLinkWithEntryPayloadAndFeedMetadata";

		// Token: 0x04000B71 RID: 2929
		internal const string WriterValidationUtils_CollectionPropertiesMustNotHaveNullValue = "WriterValidationUtils_CollectionPropertiesMustNotHaveNullValue";

		// Token: 0x04000B72 RID: 2930
		internal const string WriterValidationUtils_NonNullablePropertiesMustNotHaveNullValue = "WriterValidationUtils_NonNullablePropertiesMustNotHaveNullValue";

		// Token: 0x04000B73 RID: 2931
		internal const string WriterValidationUtils_StreamPropertiesMustNotHaveNullValue = "WriterValidationUtils_StreamPropertiesMustNotHaveNullValue";

		// Token: 0x04000B74 RID: 2932
		internal const string WriterValidationUtils_OperationInRequest = "WriterValidationUtils_OperationInRequest";

		// Token: 0x04000B75 RID: 2933
		internal const string WriterValidationUtils_AssociationLinkInRequest = "WriterValidationUtils_AssociationLinkInRequest";

		// Token: 0x04000B76 RID: 2934
		internal const string WriterValidationUtils_StreamPropertyInRequest = "WriterValidationUtils_StreamPropertyInRequest";

		// Token: 0x04000B77 RID: 2935
		internal const string WriterValidationUtils_MessageWriterSettingsMetadataDocumentUriMustBeNullOrAbsolute = "WriterValidationUtils_MessageWriterSettingsMetadataDocumentUriMustBeNullOrAbsolute";

		// Token: 0x04000B78 RID: 2936
		internal const string WriterValidationUtils_NavigationLinkMustSpecifyUrl = "WriterValidationUtils_NavigationLinkMustSpecifyUrl";

		// Token: 0x04000B79 RID: 2937
		internal const string WriterValidationUtils_NavigationLinkMustSpecifyIsCollection = "WriterValidationUtils_NavigationLinkMustSpecifyIsCollection";

		// Token: 0x04000B7A RID: 2938
		internal const string WriterValidationUtils_MessageWriterSettingsJsonPaddingOnRequestMessage = "WriterValidationUtils_MessageWriterSettingsJsonPaddingOnRequestMessage";

		// Token: 0x04000B7B RID: 2939
		internal const string XmlReaderExtension_InvalidNodeInStringValue = "XmlReaderExtension_InvalidNodeInStringValue";

		// Token: 0x04000B7C RID: 2940
		internal const string XmlReaderExtension_InvalidRootNode = "XmlReaderExtension_InvalidRootNode";

		// Token: 0x04000B7D RID: 2941
		internal const string ODataAtomInputContext_NonEmptyElementWithNullAttribute = "ODataAtomInputContext_NonEmptyElementWithNullAttribute";

		// Token: 0x04000B7E RID: 2942
		internal const string ODataMetadataInputContext_ErrorReadingMetadata = "ODataMetadataInputContext_ErrorReadingMetadata";

		// Token: 0x04000B7F RID: 2943
		internal const string ODataMetadataOutputContext_ErrorWritingMetadata = "ODataMetadataOutputContext_ErrorWritingMetadata";

		// Token: 0x04000B80 RID: 2944
		internal const string EpmExtensionMethods_InvalidKeepInContentOnType = "EpmExtensionMethods_InvalidKeepInContentOnType";

		// Token: 0x04000B81 RID: 2945
		internal const string EpmExtensionMethods_InvalidKeepInContentOnProperty = "EpmExtensionMethods_InvalidKeepInContentOnProperty";

		// Token: 0x04000B82 RID: 2946
		internal const string EpmExtensionMethods_InvalidTargetTextContentKindOnType = "EpmExtensionMethods_InvalidTargetTextContentKindOnType";

		// Token: 0x04000B83 RID: 2947
		internal const string EpmExtensionMethods_InvalidTargetTextContentKindOnProperty = "EpmExtensionMethods_InvalidTargetTextContentKindOnProperty";

		// Token: 0x04000B84 RID: 2948
		internal const string EpmExtensionMethods_MissingAttributeOnType = "EpmExtensionMethods_MissingAttributeOnType";

		// Token: 0x04000B85 RID: 2949
		internal const string EpmExtensionMethods_MissingAttributeOnProperty = "EpmExtensionMethods_MissingAttributeOnProperty";

		// Token: 0x04000B86 RID: 2950
		internal const string EpmExtensionMethods_AttributeNotAllowedForCustomMappingOnType = "EpmExtensionMethods_AttributeNotAllowedForCustomMappingOnType";

		// Token: 0x04000B87 RID: 2951
		internal const string EpmExtensionMethods_AttributeNotAllowedForCustomMappingOnProperty = "EpmExtensionMethods_AttributeNotAllowedForCustomMappingOnProperty";

		// Token: 0x04000B88 RID: 2952
		internal const string EpmExtensionMethods_AttributeNotAllowedForAtomPubMappingOnType = "EpmExtensionMethods_AttributeNotAllowedForAtomPubMappingOnType";

		// Token: 0x04000B89 RID: 2953
		internal const string EpmExtensionMethods_AttributeNotAllowedForAtomPubMappingOnProperty = "EpmExtensionMethods_AttributeNotAllowedForAtomPubMappingOnProperty";

		// Token: 0x04000B8A RID: 2954
		internal const string EpmExtensionMethods_CannotConvertEdmAnnotationValue = "EpmExtensionMethods_CannotConvertEdmAnnotationValue";

		// Token: 0x04000B8B RID: 2955
		internal const string ODataAtomReader_MediaLinkEntryMismatch = "ODataAtomReader_MediaLinkEntryMismatch";

		// Token: 0x04000B8C RID: 2956
		internal const string ODataAtomReader_FeedNavigationLinkForResourceReferenceProperty = "ODataAtomReader_FeedNavigationLinkForResourceReferenceProperty";

		// Token: 0x04000B8D RID: 2957
		internal const string ODataAtomReader_ExpandedFeedInEntryNavigationLink = "ODataAtomReader_ExpandedFeedInEntryNavigationLink";

		// Token: 0x04000B8E RID: 2958
		internal const string ODataAtomReader_ExpandedEntryInFeedNavigationLink = "ODataAtomReader_ExpandedEntryInFeedNavigationLink";

		// Token: 0x04000B8F RID: 2959
		internal const string ODataAtomReader_DeferredEntryInFeedNavigationLink = "ODataAtomReader_DeferredEntryInFeedNavigationLink";

		// Token: 0x04000B90 RID: 2960
		internal const string ODataAtomReader_EntryXmlCustomizationCallbackReturnedSameInstance = "ODataAtomReader_EntryXmlCustomizationCallbackReturnedSameInstance";

		// Token: 0x04000B91 RID: 2961
		internal const string ODataAtomReaderUtils_InvalidTypeName = "ODataAtomReaderUtils_InvalidTypeName";

		// Token: 0x04000B92 RID: 2962
		internal const string ODataAtomDeserializer_RelativeUriUsedWithoutBaseUriSpecified = "ODataAtomDeserializer_RelativeUriUsedWithoutBaseUriSpecified";

		// Token: 0x04000B93 RID: 2963
		internal const string ODataAtomCollectionDeserializer_TypeOrNullAttributeNotAllowed = "ODataAtomCollectionDeserializer_TypeOrNullAttributeNotAllowed";

		// Token: 0x04000B94 RID: 2964
		internal const string ODataAtomCollectionDeserializer_WrongCollectionItemElementName = "ODataAtomCollectionDeserializer_WrongCollectionItemElementName";

		// Token: 0x04000B95 RID: 2965
		internal const string ODataAtomCollectionDeserializer_TopLevelCollectionElementWrongNamespace = "ODataAtomCollectionDeserializer_TopLevelCollectionElementWrongNamespace";

		// Token: 0x04000B96 RID: 2966
		internal const string ODataAtomPropertyAndValueDeserializer_TopLevelPropertyElementWrongNamespace = "ODataAtomPropertyAndValueDeserializer_TopLevelPropertyElementWrongNamespace";

		// Token: 0x04000B97 RID: 2967
		internal const string ODataAtomPropertyAndValueDeserializer_NonEmptyElementWithNullAttribute = "ODataAtomPropertyAndValueDeserializer_NonEmptyElementWithNullAttribute";

		// Token: 0x04000B98 RID: 2968
		internal const string ODataAtomPropertyAndValueDeserializer_InvalidCollectionElement = "ODataAtomPropertyAndValueDeserializer_InvalidCollectionElement";

		// Token: 0x04000B99 RID: 2969
		internal const string ODataAtomPropertyAndValueDeserializer_NavigationPropertyInProperties = "ODataAtomPropertyAndValueDeserializer_NavigationPropertyInProperties";

		// Token: 0x04000B9A RID: 2970
		internal const string ODataAtomPropertyAndValueSerializer_NullValueNotAllowedForInstanceAnnotation = "ODataAtomPropertyAndValueSerializer_NullValueNotAllowedForInstanceAnnotation";

		// Token: 0x04000B9B RID: 2971
		internal const string EdmLibraryExtensions_CollectionItemCanBeOnlyPrimitiveOrComplex = "EdmLibraryExtensions_CollectionItemCanBeOnlyPrimitiveOrComplex";

		// Token: 0x04000B9C RID: 2972
		internal const string ODataAtomEntryAndFeedDeserializer_ElementExpected = "ODataAtomEntryAndFeedDeserializer_ElementExpected";

		// Token: 0x04000B9D RID: 2973
		internal const string ODataAtomEntryAndFeedDeserializer_EntryElementWrongName = "ODataAtomEntryAndFeedDeserializer_EntryElementWrongName";

		// Token: 0x04000B9E RID: 2974
		internal const string ODataAtomEntryAndFeedDeserializer_ContentWithSourceLinkIsNotEmpty = "ODataAtomEntryAndFeedDeserializer_ContentWithSourceLinkIsNotEmpty";

		// Token: 0x04000B9F RID: 2975
		internal const string ODataAtomEntryAndFeedDeserializer_ContentWithWrongType = "ODataAtomEntryAndFeedDeserializer_ContentWithWrongType";

		// Token: 0x04000BA0 RID: 2976
		internal const string ODataAtomEntryAndFeedDeserializer_ContentWithInvalidNode = "ODataAtomEntryAndFeedDeserializer_ContentWithInvalidNode";

		// Token: 0x04000BA1 RID: 2977
		internal const string ODataAtomEntryAndFeedDeserializer_FeedElementWrongName = "ODataAtomEntryAndFeedDeserializer_FeedElementWrongName";

		// Token: 0x04000BA2 RID: 2978
		internal const string ODataAtomEntryAndFeedDeserializer_UnknownElementInInline = "ODataAtomEntryAndFeedDeserializer_UnknownElementInInline";

		// Token: 0x04000BA3 RID: 2979
		internal const string ODataAtomEntryAndFeedDeserializer_MultipleExpansionsInInline = "ODataAtomEntryAndFeedDeserializer_MultipleExpansionsInInline";

		// Token: 0x04000BA4 RID: 2980
		internal const string ODataAtomEntryAndFeedDeserializer_MultipleInlineElementsInLink = "ODataAtomEntryAndFeedDeserializer_MultipleInlineElementsInLink";

		// Token: 0x04000BA5 RID: 2981
		internal const string ODataAtomEntryAndFeedDeserializer_StreamPropertyWithMultipleEditLinks = "ODataAtomEntryAndFeedDeserializer_StreamPropertyWithMultipleEditLinks";

		// Token: 0x04000BA6 RID: 2982
		internal const string ODataAtomEntryAndFeedDeserializer_StreamPropertyWithMultipleReadLinks = "ODataAtomEntryAndFeedDeserializer_StreamPropertyWithMultipleReadLinks";

		// Token: 0x04000BA7 RID: 2983
		internal const string ODataAtomEntryAndFeedDeserializer_StreamPropertyWithMultipleContentTypes = "ODataAtomEntryAndFeedDeserializer_StreamPropertyWithMultipleContentTypes";

		// Token: 0x04000BA8 RID: 2984
		internal const string ODataAtomEntryAndFeedDeserializer_StreamPropertyDuplicatePropertyName = "ODataAtomEntryAndFeedDeserializer_StreamPropertyDuplicatePropertyName";

		// Token: 0x04000BA9 RID: 2985
		internal const string ODataAtomEntryAndFeedDeserializer_StreamPropertyWithEmptyName = "ODataAtomEntryAndFeedDeserializer_StreamPropertyWithEmptyName";

		// Token: 0x04000BAA RID: 2986
		internal const string ODataAtomEntryAndFeedDeserializer_OperationMissingMetadataAttribute = "ODataAtomEntryAndFeedDeserializer_OperationMissingMetadataAttribute";

		// Token: 0x04000BAB RID: 2987
		internal const string ODataAtomEntryAndFeedDeserializer_OperationMissingTargetAttribute = "ODataAtomEntryAndFeedDeserializer_OperationMissingTargetAttribute";

		// Token: 0x04000BAC RID: 2988
		internal const string ODataAtomEntryAndFeedDeserializer_MultipleLinksInEntry = "ODataAtomEntryAndFeedDeserializer_MultipleLinksInEntry";

		// Token: 0x04000BAD RID: 2989
		internal const string ODataAtomEntryAndFeedDeserializer_MultipleLinksInFeed = "ODataAtomEntryAndFeedDeserializer_MultipleLinksInFeed";

		// Token: 0x04000BAE RID: 2990
		internal const string ODataAtomEntryAndFeedDeserializer_DuplicateElements = "ODataAtomEntryAndFeedDeserializer_DuplicateElements";

		// Token: 0x04000BAF RID: 2991
		internal const string ODataAtomEntryAndFeedDeserializer_InvalidTypeAttributeOnAssociationLink = "ODataAtomEntryAndFeedDeserializer_InvalidTypeAttributeOnAssociationLink";

		// Token: 0x04000BB0 RID: 2992
		internal const string ODataAtomEntryAndFeedDeserializer_EncounteredAnnotationInNestedFeed = "ODataAtomEntryAndFeedDeserializer_EncounteredAnnotationInNestedFeed";

		// Token: 0x04000BB1 RID: 2993
		internal const string ODataAtomEntryAndFeedDeserializer_EncounteredDeltaLinkInNestedFeed = "ODataAtomEntryAndFeedDeserializer_EncounteredDeltaLinkInNestedFeed";

		// Token: 0x04000BB2 RID: 2994
		internal const string ODataAtomEntryAndFeedDeserializer_AnnotationWithNonDotTarget = "ODataAtomEntryAndFeedDeserializer_AnnotationWithNonDotTarget";

		// Token: 0x04000BB3 RID: 2995
		internal const string ODataAtomServiceDocumentDeserializer_ServiceDocumentRootElementWrongNameOrNamespace = "ODataAtomServiceDocumentDeserializer_ServiceDocumentRootElementWrongNameOrNamespace";

		// Token: 0x04000BB4 RID: 2996
		internal const string ODataAtomServiceDocumentDeserializer_MissingWorkspaceElement = "ODataAtomServiceDocumentDeserializer_MissingWorkspaceElement";

		// Token: 0x04000BB5 RID: 2997
		internal const string ODataAtomServiceDocumentDeserializer_MultipleWorkspaceElementsFound = "ODataAtomServiceDocumentDeserializer_MultipleWorkspaceElementsFound";

		// Token: 0x04000BB6 RID: 2998
		internal const string ODataAtomServiceDocumentDeserializer_UnexpectedElementInServiceDocument = "ODataAtomServiceDocumentDeserializer_UnexpectedElementInServiceDocument";

		// Token: 0x04000BB7 RID: 2999
		internal const string ODataAtomServiceDocumentDeserializer_UnexpectedElementInWorkspace = "ODataAtomServiceDocumentDeserializer_UnexpectedElementInWorkspace";

		// Token: 0x04000BB8 RID: 3000
		internal const string ODataAtomServiceDocumentDeserializer_UnexpectedElementInResourceCollection = "ODataAtomServiceDocumentDeserializer_UnexpectedElementInResourceCollection";

		// Token: 0x04000BB9 RID: 3001
		internal const string ODataAtomEntryMetadataDeserializer_InvalidTextConstructKind = "ODataAtomEntryMetadataDeserializer_InvalidTextConstructKind";

		// Token: 0x04000BBA RID: 3002
		internal const string ODataAtomMetadataDeserializer_MultipleSingletonMetadataElements = "ODataAtomMetadataDeserializer_MultipleSingletonMetadataElements";

		// Token: 0x04000BBB RID: 3003
		internal const string ODataAtomErrorDeserializer_InvalidRootElement = "ODataAtomErrorDeserializer_InvalidRootElement";

		// Token: 0x04000BBC RID: 3004
		internal const string ODataAtomErrorDeserializer_MultipleErrorElementsWithSameName = "ODataAtomErrorDeserializer_MultipleErrorElementsWithSameName";

		// Token: 0x04000BBD RID: 3005
		internal const string ODataAtomErrorDeserializer_MultipleInnerErrorElementsWithSameName = "ODataAtomErrorDeserializer_MultipleInnerErrorElementsWithSameName";

		// Token: 0x04000BBE RID: 3006
		internal const string ODataAtomEntityReferenceLinkDeserializer_InvalidEntityReferenceLinkStartElement = "ODataAtomEntityReferenceLinkDeserializer_InvalidEntityReferenceLinkStartElement";

		// Token: 0x04000BBF RID: 3007
		internal const string ODataAtomEntityReferenceLinkDeserializer_InvalidEntityReferenceLinksStartElement = "ODataAtomEntityReferenceLinkDeserializer_InvalidEntityReferenceLinksStartElement";

		// Token: 0x04000BC0 RID: 3008
		internal const string ODataAtomEntityReferenceLinkDeserializer_MultipleEntityReferenceLinksElementsWithSameName = "ODataAtomEntityReferenceLinkDeserializer_MultipleEntityReferenceLinksElementsWithSameName";

		// Token: 0x04000BC1 RID: 3009
		internal const string EpmReader_OpenComplexOrCollectionEpmProperty = "EpmReader_OpenComplexOrCollectionEpmProperty";

		// Token: 0x04000BC2 RID: 3010
		internal const string EpmSyndicationReader_MultipleValuesForNonCollectionProperty = "EpmSyndicationReader_MultipleValuesForNonCollectionProperty";

		// Token: 0x04000BC3 RID: 3011
		internal const string ODataAtomServiceDocumentMetadataDeserializer_InvalidFixedAttributeValue = "ODataAtomServiceDocumentMetadataDeserializer_InvalidFixedAttributeValue";

		// Token: 0x04000BC4 RID: 3012
		internal const string ODataAtomServiceDocumentMetadataDeserializer_MultipleTitleElementsFound = "ODataAtomServiceDocumentMetadataDeserializer_MultipleTitleElementsFound";

		// Token: 0x04000BC5 RID: 3013
		internal const string ODataAtomServiceDocumentMetadataDeserializer_MultipleAcceptElementsFoundInCollection = "ODataAtomServiceDocumentMetadataDeserializer_MultipleAcceptElementsFoundInCollection";

		// Token: 0x04000BC6 RID: 3014
		internal const string ODataAtomServiceDocumentMetadataSerializer_ResourceCollectionNameAndTitleMismatch = "ODataAtomServiceDocumentMetadataSerializer_ResourceCollectionNameAndTitleMismatch";

		// Token: 0x04000BC7 RID: 3015
		internal const string CollectionWithoutExpectedTypeValidator_InvalidItemTypeKind = "CollectionWithoutExpectedTypeValidator_InvalidItemTypeKind";

		// Token: 0x04000BC8 RID: 3016
		internal const string CollectionWithoutExpectedTypeValidator_IncompatibleItemTypeKind = "CollectionWithoutExpectedTypeValidator_IncompatibleItemTypeKind";

		// Token: 0x04000BC9 RID: 3017
		internal const string CollectionWithoutExpectedTypeValidator_IncompatibleItemTypeName = "CollectionWithoutExpectedTypeValidator_IncompatibleItemTypeName";

		// Token: 0x04000BCA RID: 3018
		internal const string FeedWithoutExpectedTypeValidator_IncompatibleTypes = "FeedWithoutExpectedTypeValidator_IncompatibleTypes";

		// Token: 0x04000BCB RID: 3019
		internal const string MessageStreamWrappingStream_ByteLimitExceeded = "MessageStreamWrappingStream_ByteLimitExceeded";

		// Token: 0x04000BCC RID: 3020
		internal const string MetadataUtils_ResolveTypeName = "MetadataUtils_ResolveTypeName";

		// Token: 0x04000BCD RID: 3021
		internal const string EdmValueUtils_UnsupportedPrimitiveType = "EdmValueUtils_UnsupportedPrimitiveType";

		// Token: 0x04000BCE RID: 3022
		internal const string EdmValueUtils_IncorrectPrimitiveTypeKind = "EdmValueUtils_IncorrectPrimitiveTypeKind";

		// Token: 0x04000BCF RID: 3023
		internal const string EdmValueUtils_IncorrectPrimitiveTypeKindNoTypeName = "EdmValueUtils_IncorrectPrimitiveTypeKindNoTypeName";

		// Token: 0x04000BD0 RID: 3024
		internal const string EdmValueUtils_CannotConvertTypeToClrValue = "EdmValueUtils_CannotConvertTypeToClrValue";

		// Token: 0x04000BD1 RID: 3025
		internal const string ODataEdmStructuredValue_UndeclaredProperty = "ODataEdmStructuredValue_UndeclaredProperty";

		// Token: 0x04000BD2 RID: 3026
		internal const string ODataModelAnnotationEvaluator_AmbiguousAnnotationTerm = "ODataModelAnnotationEvaluator_AmbiguousAnnotationTerm";

		// Token: 0x04000BD3 RID: 3027
		internal const string ODataModelAnnotationEvaluator_AmbiguousAnnotationTermWithQualifier = "ODataModelAnnotationEvaluator_AmbiguousAnnotationTermWithQualifier";

		// Token: 0x04000BD4 RID: 3028
		internal const string ODataModelAnnotationEvaluator_AnnotationTermWithInvalidQualifier = "ODataModelAnnotationEvaluator_AnnotationTermWithInvalidQualifier";

		// Token: 0x04000BD5 RID: 3029
		internal const string ODataModelAnnotationEvaluator_AnnotationTermWithUnsupportedQualifier = "ODataModelAnnotationEvaluator_AnnotationTermWithUnsupportedQualifier";

		// Token: 0x04000BD6 RID: 3030
		internal const string ODataMetadataBuilder_MissingEntitySetUri = "ODataMetadataBuilder_MissingEntitySetUri";

		// Token: 0x04000BD7 RID: 3031
		internal const string ODataMetadataBuilder_MissingSegmentForEntitySetUriSuffix = "ODataMetadataBuilder_MissingSegmentForEntitySetUriSuffix";

		// Token: 0x04000BD8 RID: 3032
		internal const string ODataMetadataBuilder_MissingEntityInstanceUri = "ODataMetadataBuilder_MissingEntityInstanceUri";

		// Token: 0x04000BD9 RID: 3033
		internal const string ODataJsonLightInputContext_EntityTypeMustBeCompatibleWithEntitySetBaseType = "ODataJsonLightInputContext_EntityTypeMustBeCompatibleWithEntitySetBaseType";

		// Token: 0x04000BDA RID: 3034
		internal const string ODataJsonLightInputContext_MetadataDocumentUriMissing = "ODataJsonLightInputContext_MetadataDocumentUriMissing";

		// Token: 0x04000BDB RID: 3035
		internal const string ODataJsonLightInputContext_PayloadKindDetectionForRequest = "ODataJsonLightInputContext_PayloadKindDetectionForRequest";

		// Token: 0x04000BDC RID: 3036
		internal const string ODataJsonLightInputContext_FunctionImportCannotBeNullForCreateParameterReader = "ODataJsonLightInputContext_FunctionImportCannotBeNullForCreateParameterReader";

		// Token: 0x04000BDD RID: 3037
		internal const string ODataJsonLightInputContext_NoEntitySetForRequest = "ODataJsonLightInputContext_NoEntitySetForRequest";

		// Token: 0x04000BDE RID: 3038
		internal const string ODataJsonLightInputContext_FunctionImportOrItemTypeRequiredForCollectionReaderInRequests = "ODataJsonLightInputContext_FunctionImportOrItemTypeRequiredForCollectionReaderInRequests";

		// Token: 0x04000BDF RID: 3039
		internal const string ODataJsonLightInputContext_NavigationPropertyRequiredForReadEntityReferenceLinkInRequests = "ODataJsonLightInputContext_NavigationPropertyRequiredForReadEntityReferenceLinkInRequests";

		// Token: 0x04000BE0 RID: 3040
		internal const string ODataJsonLightInputContext_ModelRequiredForReading = "ODataJsonLightInputContext_ModelRequiredForReading";

		// Token: 0x04000BE1 RID: 3041
		internal const string ODataJsonLightInputContext_BaseUriMustBeNonNullAndAbsolute = "ODataJsonLightInputContext_BaseUriMustBeNonNullAndAbsolute";

		// Token: 0x04000BE2 RID: 3042
		internal const string ODataJsonLightInputContext_ItemTypeRequiredForCollectionReaderInRequests = "ODataJsonLightInputContext_ItemTypeRequiredForCollectionReaderInRequests";

		// Token: 0x04000BE3 RID: 3043
		internal const string ODataJsonLightInputContext_NoItemTypeSpecified = "ODataJsonLightInputContext_NoItemTypeSpecified";

		// Token: 0x04000BE4 RID: 3044
		internal const string ODataJsonLightDeserializer_MetadataLinkNotFoundAsFirstProperty = "ODataJsonLightDeserializer_MetadataLinkNotFoundAsFirstProperty";

		// Token: 0x04000BE5 RID: 3045
		internal const string ODataJsonLightDeserializer_RequiredPropertyNotFound = "ODataJsonLightDeserializer_RequiredPropertyNotFound";

		// Token: 0x04000BE6 RID: 3046
		internal const string ODataJsonLightDeserializer_OnlyODataTypeAnnotationCanTargetInstanceAnnotation = "ODataJsonLightDeserializer_OnlyODataTypeAnnotationCanTargetInstanceAnnotation";

		// Token: 0x04000BE7 RID: 3047
		internal const string ODataJsonLightDeserializer_AnnotationTargetingInstanceAnnotationWithoutValue = "ODataJsonLightDeserializer_AnnotationTargetingInstanceAnnotationWithoutValue";

		// Token: 0x04000BE8 RID: 3048
		internal const string ODataJsonLightWriter_EntityReferenceLinkAfterFeedInRequest = "ODataJsonLightWriter_EntityReferenceLinkAfterFeedInRequest";

		// Token: 0x04000BE9 RID: 3049
		internal const string ODataJsonLightWriter_InstanceAnnotationNotSupportedOnExpandedFeed = "ODataJsonLightWriter_InstanceAnnotationNotSupportedOnExpandedFeed";

		// Token: 0x04000BEA RID: 3050
		internal const string ODataJsonLightOutputContext_MetadataDocumentUriMissing = "ODataJsonLightOutputContext_MetadataDocumentUriMissing";

		// Token: 0x04000BEB RID: 3051
		internal const string ODataJsonLightPropertyAndValueSerializer_NoExpectedTypeOrTypeNameSpecifiedForComplexValueRequest = "ODataJsonLightPropertyAndValueSerializer_NoExpectedTypeOrTypeNameSpecifiedForComplexValueRequest";

		// Token: 0x04000BEC RID: 3052
		internal const string ODataJsonLightPropertyAndValueSerializer_NoExpectedTypeOrTypeNameSpecifiedForCollectionValueInRequest = "ODataJsonLightPropertyAndValueSerializer_NoExpectedTypeOrTypeNameSpecifiedForCollectionValueInRequest";

		// Token: 0x04000BED RID: 3053
		internal const string ODataJsonLightServiceDocumentSerializer_ResourceCollectionMustSpecifyName = "ODataJsonLightServiceDocumentSerializer_ResourceCollectionMustSpecifyName";

		// Token: 0x04000BEE RID: 3054
		internal const string ODataFeedAndEntryTypeContext_MetadataOrSerializationInfoMissing = "ODataFeedAndEntryTypeContext_MetadataOrSerializationInfoMissing";

		// Token: 0x04000BEF RID: 3055
		internal const string ODataFeedAndEntryTypeContext_ODataEntryTypeNameMissing = "ODataFeedAndEntryTypeContext_ODataEntryTypeNameMissing";

		// Token: 0x04000BF0 RID: 3056
		internal const string ODataJsonLightMetadataUriBuilder_ValidateDerivedType = "ODataJsonLightMetadataUriBuilder_ValidateDerivedType";

		// Token: 0x04000BF1 RID: 3057
		internal const string ODataJsonLightMetadataUriBuilder_TypeNameMissingForTopLevelCollectionWhenWritingResponsePayload = "ODataJsonLightMetadataUriBuilder_TypeNameMissingForTopLevelCollectionWhenWritingResponsePayload";

		// Token: 0x04000BF2 RID: 3058
		internal const string ODataJsonLightMetadataUriBuilder_EntitySetOrNavigationPropertyMissingForTopLevelEntityReferenceLinkResponse = "ODataJsonLightMetadataUriBuilder_EntitySetOrNavigationPropertyMissingForTopLevelEntityReferenceLinkResponse";

		// Token: 0x04000BF3 RID: 3059
		internal const string ODataJsonLightMetadataUriBuilder_EntitySetOrNavigationPropertyMissingForTopLevelEntityReferenceLinksResponse = "ODataJsonLightMetadataUriBuilder_EntitySetOrNavigationPropertyMissingForTopLevelEntityReferenceLinksResponse";

		// Token: 0x04000BF4 RID: 3060
		internal const string ODataJsonLightPropertyAndValueDeserializer_UnexpectedAnnotationProperties = "ODataJsonLightPropertyAndValueDeserializer_UnexpectedAnnotationProperties";

		// Token: 0x04000BF5 RID: 3061
		internal const string ODataJsonLightPropertyAndValueDeserializer_UnexpectedPropertyAnnotation = "ODataJsonLightPropertyAndValueDeserializer_UnexpectedPropertyAnnotation";

		// Token: 0x04000BF6 RID: 3062
		internal const string ODataJsonLightPropertyAndValueDeserializer_UnexpectedODataPropertyAnnotation = "ODataJsonLightPropertyAndValueDeserializer_UnexpectedODataPropertyAnnotation";

		// Token: 0x04000BF7 RID: 3063
		internal const string ODataJsonLightPropertyAndValueDeserializer_UnexpectedProperty = "ODataJsonLightPropertyAndValueDeserializer_UnexpectedProperty";

		// Token: 0x04000BF8 RID: 3064
		internal const string ODataJsonLightPropertyAndValueDeserializer_InvalidTopLevelPropertyPayload = "ODataJsonLightPropertyAndValueDeserializer_InvalidTopLevelPropertyPayload";

		// Token: 0x04000BF9 RID: 3065
		internal const string ODataJsonLightPropertyAndValueDeserializer_InvalidTopLevelPropertyName = "ODataJsonLightPropertyAndValueDeserializer_InvalidTopLevelPropertyName";

		// Token: 0x04000BFA RID: 3066
		internal const string ODataJsonLightPropertyAndValueDeserializer_InvalidTypeName = "ODataJsonLightPropertyAndValueDeserializer_InvalidTypeName";

		// Token: 0x04000BFB RID: 3067
		internal const string ODataJsonLightPropertyAndValueDeserializer_InvalidPrimitiveTypeName = "ODataJsonLightPropertyAndValueDeserializer_InvalidPrimitiveTypeName";

		// Token: 0x04000BFC RID: 3068
		internal const string ODataJsonLightPropertyAndValueDeserializer_TopLevelPropertyAnnotationWithoutProperty = "ODataJsonLightPropertyAndValueDeserializer_TopLevelPropertyAnnotationWithoutProperty";

		// Token: 0x04000BFD RID: 3069
		internal const string ODataJsonLightPropertyAndValueDeserializer_ComplexValuePropertyAnnotationWithoutProperty = "ODataJsonLightPropertyAndValueDeserializer_ComplexValuePropertyAnnotationWithoutProperty";

		// Token: 0x04000BFE RID: 3070
		internal const string ODataJsonLightPropertyAndValueDeserializer_ComplexValueWithPropertyTypeAnnotation = "ODataJsonLightPropertyAndValueDeserializer_ComplexValueWithPropertyTypeAnnotation";

		// Token: 0x04000BFF RID: 3071
		internal const string ODataJsonLightPropertyAndValueDeserializer_ComplexTypeAnnotationNotFirst = "ODataJsonLightPropertyAndValueDeserializer_ComplexTypeAnnotationNotFirst";

		// Token: 0x04000C00 RID: 3072
		internal const string ODataJsonLightPropertyAndValueDeserializer_UnexpectedDataPropertyAnnotation = "ODataJsonLightPropertyAndValueDeserializer_UnexpectedDataPropertyAnnotation";

		// Token: 0x04000C01 RID: 3073
		internal const string ODataJsonLightPropertyAndValueDeserializer_TypePropertyAfterValueProperty = "ODataJsonLightPropertyAndValueDeserializer_TypePropertyAfterValueProperty";

		// Token: 0x04000C02 RID: 3074
		internal const string ODataJsonLightPropertyAndValueDeserializer_ODataTypeAnnotationInPrimitiveValue = "ODataJsonLightPropertyAndValueDeserializer_ODataTypeAnnotationInPrimitiveValue";

		// Token: 0x04000C03 RID: 3075
		internal const string ODataJsonLightPropertyAndValueDeserializer_TopLevelPropertyWithPrimitiveNullValue = "ODataJsonLightPropertyAndValueDeserializer_TopLevelPropertyWithPrimitiveNullValue";

		// Token: 0x04000C04 RID: 3076
		internal const string ODataJsonLightPropertyAndValueDeserializer_UnexpectedMetadataReferenceProperty = "ODataJsonLightPropertyAndValueDeserializer_UnexpectedMetadataReferenceProperty";

		// Token: 0x04000C05 RID: 3077
		internal const string ODataJsonLightPropertyAndValueDeserializer_NoPropertyAndAnnotationAllowedInNullPayload = "ODataJsonLightPropertyAndValueDeserializer_NoPropertyAndAnnotationAllowedInNullPayload";

		// Token: 0x04000C06 RID: 3078
		internal const string ODataJsonLightPropertyAndValueDeserializer_EdmNullInMetadataUriWithoutNullValueInPayload = "ODataJsonLightPropertyAndValueDeserializer_EdmNullInMetadataUriWithoutNullValueInPayload";

		// Token: 0x04000C07 RID: 3079
		internal const string ODataJsonReaderCoreUtils_CannotReadSpatialPropertyValue = "ODataJsonReaderCoreUtils_CannotReadSpatialPropertyValue";

		// Token: 0x04000C08 RID: 3080
		internal const string ODataJsonLightReaderUtils_AnnotationWithNullValue = "ODataJsonLightReaderUtils_AnnotationWithNullValue";

		// Token: 0x04000C09 RID: 3081
		internal const string ODataJsonLightReaderUtils_InvalidValueForODataNullAnnotation = "ODataJsonLightReaderUtils_InvalidValueForODataNullAnnotation";

		// Token: 0x04000C0A RID: 3082
		internal const string JsonLightInstanceAnnotationWriter_DuplicateAnnotationNameInCollection = "JsonLightInstanceAnnotationWriter_DuplicateAnnotationNameInCollection";

		// Token: 0x04000C0B RID: 3083
		internal const string ODataJsonLightMetadataUriParser_ServiceDocumentUriMustNotHaveFragment = "ODataJsonLightMetadataUriParser_ServiceDocumentUriMustNotHaveFragment";

		// Token: 0x04000C0C RID: 3084
		internal const string ODataJsonLightMetadataUriParser_NullMetadataDocumentUri = "ODataJsonLightMetadataUriParser_NullMetadataDocumentUri";

		// Token: 0x04000C0D RID: 3085
		internal const string ODataJsonLightMetadataUriParser_MetadataUriDoesNotMatchExpectedPayloadKind = "ODataJsonLightMetadataUriParser_MetadataUriDoesNotMatchExpectedPayloadKind";

		// Token: 0x04000C0E RID: 3086
		internal const string ODataJsonLightMetadataUriParser_InvalidEntitySetNameOrTypeName = "ODataJsonLightMetadataUriParser_InvalidEntitySetNameOrTypeName";

		// Token: 0x04000C0F RID: 3087
		internal const string ODataJsonLightMetadataUriParser_InvalidPropertyName = "ODataJsonLightMetadataUriParser_InvalidPropertyName";

		// Token: 0x04000C10 RID: 3088
		internal const string ODataJsonLightMetadataUriParser_InvalidEntityWithTypeCastUriSuffix = "ODataJsonLightMetadataUriParser_InvalidEntityWithTypeCastUriSuffix";

		// Token: 0x04000C11 RID: 3089
		internal const string ODataJsonLightMetadataUriParser_InvalidEntityTypeInTypeCast = "ODataJsonLightMetadataUriParser_InvalidEntityTypeInTypeCast";

		// Token: 0x04000C12 RID: 3090
		internal const string ODataJsonLightMetadataUriParser_IncompatibleEntityTypeInTypeCast = "ODataJsonLightMetadataUriParser_IncompatibleEntityTypeInTypeCast";

		// Token: 0x04000C13 RID: 3091
		internal const string ODataJsonLightMetadataUriParser_InvalidEntityReferenceLinkUriSuffix = "ODataJsonLightMetadataUriParser_InvalidEntityReferenceLinkUriSuffix";

		// Token: 0x04000C14 RID: 3092
		internal const string ODataJsonLightMetadataUriParser_InvalidPropertyForEntityReferenceLinkUri = "ODataJsonLightMetadataUriParser_InvalidPropertyForEntityReferenceLinkUri";

		// Token: 0x04000C15 RID: 3093
		internal const string ODataJsonLightMetadataUriParser_InvalidSingletonNavPropertyForEntityReferenceLinkUri = "ODataJsonLightMetadataUriParser_InvalidSingletonNavPropertyForEntityReferenceLinkUri";

		// Token: 0x04000C16 RID: 3094
		internal const string ODataJsonLightMetadataUriParser_FragmentWithInvalidNumberOfParts = "ODataJsonLightMetadataUriParser_FragmentWithInvalidNumberOfParts";

		// Token: 0x04000C17 RID: 3095
		internal const string ODataJsonLightMetadataUriParser_InvalidEntitySetOrFunctionImportName = "ODataJsonLightMetadataUriParser_InvalidEntitySetOrFunctionImportName";

		// Token: 0x04000C18 RID: 3096
		internal const string ODataJsonLightMetadataUriParser_InvalidPayloadKindWithSelectQueryOption = "ODataJsonLightMetadataUriParser_InvalidPayloadKindWithSelectQueryOption";

		// Token: 0x04000C19 RID: 3097
		internal const string ODataJsonLightMetadataUriParser_NoModel = "ODataJsonLightMetadataUriParser_NoModel";

		// Token: 0x04000C1A RID: 3098
		internal const string ODataJsonLightMetadataUriParser_ModelResolverReturnedNull = "ODataJsonLightMetadataUriParser_ModelResolverReturnedNull";

		// Token: 0x04000C1B RID: 3099
		internal const string ODataJsonLightMetadataUriParser_InvalidAssociationLink = "ODataJsonLightMetadataUriParser_InvalidAssociationLink";

		// Token: 0x04000C1C RID: 3100
		internal const string ODataJsonLightMetadataUriParser_InvalidEntitySetName = "ODataJsonLightMetadataUriParser_InvalidEntitySetName";

		// Token: 0x04000C1D RID: 3101
		internal const string ODataJsonLightEntryAndFeedDeserializer_EntryTypeAnnotationNotFirst = "ODataJsonLightEntryAndFeedDeserializer_EntryTypeAnnotationNotFirst";

		// Token: 0x04000C1E RID: 3102
		internal const string ODataJsonLightEntryAndFeedDeserializer_EntryInstanceAnnotationPrecededByProperty = "ODataJsonLightEntryAndFeedDeserializer_EntryInstanceAnnotationPrecededByProperty";

		// Token: 0x04000C1F RID: 3103
		internal const string ODataJsonLightEntryAndFeedDeserializer_CannotReadFeedContentStart = "ODataJsonLightEntryAndFeedDeserializer_CannotReadFeedContentStart";

		// Token: 0x04000C20 RID: 3104
		internal const string ODataJsonLightEntryAndFeedDeserializer_ExpectedFeedPropertyNotFound = "ODataJsonLightEntryAndFeedDeserializer_ExpectedFeedPropertyNotFound";

		// Token: 0x04000C21 RID: 3105
		internal const string ODataJsonLightEntryAndFeedDeserializer_InvalidNodeTypeForItemsInFeed = "ODataJsonLightEntryAndFeedDeserializer_InvalidNodeTypeForItemsInFeed";

		// Token: 0x04000C22 RID: 3106
		internal const string ODataJsonLightEntryAndFeedDeserializer_InvalidPropertyAnnotationInTopLevelFeed = "ODataJsonLightEntryAndFeedDeserializer_InvalidPropertyAnnotationInTopLevelFeed";

		// Token: 0x04000C23 RID: 3107
		internal const string ODataJsonLightEntryAndFeedDeserializer_InvalidPropertyInTopLevelFeed = "ODataJsonLightEntryAndFeedDeserializer_InvalidPropertyInTopLevelFeed";

		// Token: 0x04000C24 RID: 3108
		internal const string ODataJsonLightEntryAndFeedDeserializer_FeedPropertyAnnotationForTopLevelFeed = "ODataJsonLightEntryAndFeedDeserializer_FeedPropertyAnnotationForTopLevelFeed";

		// Token: 0x04000C25 RID: 3109
		internal const string ODataJsonLightEntryAndFeedDeserializer_PropertyWithoutValueWithWrongType = "ODataJsonLightEntryAndFeedDeserializer_PropertyWithoutValueWithWrongType";

		// Token: 0x04000C26 RID: 3110
		internal const string ODataJsonLightEntryAndFeedDeserializer_OpenPropertyWithoutValue = "ODataJsonLightEntryAndFeedDeserializer_OpenPropertyWithoutValue";

		// Token: 0x04000C27 RID: 3111
		internal const string ODataJsonLightEntryAndFeedDeserializer_StreamPropertyInRequest = "ODataJsonLightEntryAndFeedDeserializer_StreamPropertyInRequest";

		// Token: 0x04000C28 RID: 3112
		internal const string ODataJsonLightEntryAndFeedDeserializer_UnexpectedStreamPropertyAnnotation = "ODataJsonLightEntryAndFeedDeserializer_UnexpectedStreamPropertyAnnotation";

		// Token: 0x04000C29 RID: 3113
		internal const string ODataJsonLightEntryAndFeedDeserializer_StreamPropertyWithValue = "ODataJsonLightEntryAndFeedDeserializer_StreamPropertyWithValue";

		// Token: 0x04000C2A RID: 3114
		internal const string ODataJsonLightEntryAndFeedDeserializer_UnexpectedDeferredLinkPropertyAnnotation = "ODataJsonLightEntryAndFeedDeserializer_UnexpectedDeferredLinkPropertyAnnotation";

		// Token: 0x04000C2B RID: 3115
		internal const string ODataJsonLightEntryAndFeedDeserializer_CannotReadSingletonNavigationPropertyValue = "ODataJsonLightEntryAndFeedDeserializer_CannotReadSingletonNavigationPropertyValue";

		// Token: 0x04000C2C RID: 3116
		internal const string ODataJsonLightEntryAndFeedDeserializer_CannotReadCollectionNavigationPropertyValue = "ODataJsonLightEntryAndFeedDeserializer_CannotReadCollectionNavigationPropertyValue";

		// Token: 0x04000C2D RID: 3117
		internal const string ODataJsonLightEntryAndFeedDeserializer_CannotReadNavigationPropertyValue = "ODataJsonLightEntryAndFeedDeserializer_CannotReadNavigationPropertyValue";

		// Token: 0x04000C2E RID: 3118
		internal const string ODataJsonLightEntryAndFeedDeserializer_UnexpectedExpandedSingletonNavigationLinkPropertyAnnotation = "ODataJsonLightEntryAndFeedDeserializer_UnexpectedExpandedSingletonNavigationLinkPropertyAnnotation";

		// Token: 0x04000C2F RID: 3119
		internal const string ODataJsonLightEntryAndFeedDeserializer_UnexpectedExpandedCollectionNavigationLinkPropertyAnnotation = "ODataJsonLightEntryAndFeedDeserializer_UnexpectedExpandedCollectionNavigationLinkPropertyAnnotation";

		// Token: 0x04000C30 RID: 3120
		internal const string ODataJsonLightEntryAndFeedDeserializer_DuplicateExpandedFeedAnnotation = "ODataJsonLightEntryAndFeedDeserializer_DuplicateExpandedFeedAnnotation";

		// Token: 0x04000C31 RID: 3121
		internal const string ODataJsonLightEntryAndFeedDeserializer_UnexpectedPropertyAnnotationAfterExpandedFeed = "ODataJsonLightEntryAndFeedDeserializer_UnexpectedPropertyAnnotationAfterExpandedFeed";

		// Token: 0x04000C32 RID: 3122
		internal const string ODataJsonLightEntryAndFeedDeserializer_AnnotationGroupWithoutName = "ODataJsonLightEntryAndFeedDeserializer_AnnotationGroupWithoutName";

		// Token: 0x04000C33 RID: 3123
		internal const string ODataJsonLightEntryAndFeedDeserializer_AnnotationGroupMemberWithoutName = "ODataJsonLightEntryAndFeedDeserializer_AnnotationGroupMemberWithoutName";

		// Token: 0x04000C34 RID: 3124
		internal const string ODataJsonLightEntryAndFeedDeserializer_AnnotationGroupMemberWithInvalidValue = "ODataJsonLightEntryAndFeedDeserializer_AnnotationGroupMemberWithInvalidValue";

		// Token: 0x04000C35 RID: 3125
		internal const string ODataJsonLightEntryAndFeedDeserializer_AnnotationGroupInRequest = "ODataJsonLightEntryAndFeedDeserializer_AnnotationGroupInRequest";

		// Token: 0x04000C36 RID: 3126
		internal const string ODataJsonLightEntryAndFeedDeserializer_UnexpectedNavigationLinkInRequestPropertyAnnotation = "ODataJsonLightEntryAndFeedDeserializer_UnexpectedNavigationLinkInRequestPropertyAnnotation";

		// Token: 0x04000C37 RID: 3127
		internal const string ODataJsonLightEntryAndFeedDeserializer_ArrayValueForSingletonBindPropertyAnnotation = "ODataJsonLightEntryAndFeedDeserializer_ArrayValueForSingletonBindPropertyAnnotation";

		// Token: 0x04000C38 RID: 3128
		internal const string ODataJsonLightEntryAndFeedDeserializer_StringValueForCollectionBindPropertyAnnotation = "ODataJsonLightEntryAndFeedDeserializer_StringValueForCollectionBindPropertyAnnotation";

		// Token: 0x04000C39 RID: 3129
		internal const string ODataJsonLightEntryAndFeedDeserializer_EmptyBindArray = "ODataJsonLightEntryAndFeedDeserializer_EmptyBindArray";

		// Token: 0x04000C3A RID: 3130
		internal const string ODataJsonLightEntryAndFeedDeserializer_NavigationPropertyWithoutValueAndEntityReferenceLink = "ODataJsonLightEntryAndFeedDeserializer_NavigationPropertyWithoutValueAndEntityReferenceLink";

		// Token: 0x04000C3B RID: 3131
		internal const string ODataJsonLightEntryAndFeedDeserializer_SingletonNavigationPropertyWithBindingAndValue = "ODataJsonLightEntryAndFeedDeserializer_SingletonNavigationPropertyWithBindingAndValue";

		// Token: 0x04000C3C RID: 3132
		internal const string ODataJsonLightEntryAndFeedDeserializer_PropertyWithoutValueWithUnknownType = "ODataJsonLightEntryAndFeedDeserializer_PropertyWithoutValueWithUnknownType";

		// Token: 0x04000C3D RID: 3133
		internal const string ODataJsonLightEntryAndFeedDeserializer_FunctionImportIsNotActionOrFunction = "ODataJsonLightEntryAndFeedDeserializer_FunctionImportIsNotActionOrFunction";

		// Token: 0x04000C3E RID: 3134
		internal const string ODataJsonLightEntryAndFeedDeserializer_MultipleOptionalPropertiesInOperation = "ODataJsonLightEntryAndFeedDeserializer_MultipleOptionalPropertiesInOperation";

		// Token: 0x04000C3F RID: 3135
		internal const string ODataJsonLightEntryAndFeedDeserializer_MultipleTargetPropertiesInOperation = "ODataJsonLightEntryAndFeedDeserializer_MultipleTargetPropertiesInOperation";

		// Token: 0x04000C40 RID: 3136
		internal const string ODataJsonLightEntryAndFeedDeserializer_OperationMissingTargetProperty = "ODataJsonLightEntryAndFeedDeserializer_OperationMissingTargetProperty";

		// Token: 0x04000C41 RID: 3137
		internal const string ODataJsonLightEntryAndFeedDeserializer_MetadataReferencePropertyInRequest = "ODataJsonLightEntryAndFeedDeserializer_MetadataReferencePropertyInRequest";

		// Token: 0x04000C42 RID: 3138
		internal const string ODataJsonLightEntryAndFeedDeserializer_EncounteredAnnotationGroupInUnexpectedPosition = "ODataJsonLightEntryAndFeedDeserializer_EncounteredAnnotationGroupInUnexpectedPosition";

		// Token: 0x04000C43 RID: 3139
		internal const string ODataJsonLightEntryAndFeedDeserializer_EntryTypeAlreadySpecified = "ODataJsonLightEntryAndFeedDeserializer_EntryTypeAlreadySpecified";

		// Token: 0x04000C44 RID: 3140
		internal const string ODataJsonLightValidationUtils_OperationPropertyCannotBeNull = "ODataJsonLightValidationUtils_OperationPropertyCannotBeNull";

		// Token: 0x04000C45 RID: 3141
		internal const string ODataJsonLightValidationUtils_OpenMetadataReferencePropertyNotSupported = "ODataJsonLightValidationUtils_OpenMetadataReferencePropertyNotSupported";

		// Token: 0x04000C46 RID: 3142
		internal const string ODataJsonLightDeserializer_RelativeUriUsedWithouODataMetadataAnnotation = "ODataJsonLightDeserializer_RelativeUriUsedWithouODataMetadataAnnotation";

		// Token: 0x04000C47 RID: 3143
		internal const string ODataJsonLightEntryMetadataContext_MetadataAnnotationMustBeInPayload = "ODataJsonLightEntryMetadataContext_MetadataAnnotationMustBeInPayload";

		// Token: 0x04000C48 RID: 3144
		internal const string ODataJsonLightCollectionDeserializer_ExpectedCollectionPropertyNotFound = "ODataJsonLightCollectionDeserializer_ExpectedCollectionPropertyNotFound";

		// Token: 0x04000C49 RID: 3145
		internal const string ODataJsonLightCollectionDeserializer_CannotReadCollectionContentStart = "ODataJsonLightCollectionDeserializer_CannotReadCollectionContentStart";

		// Token: 0x04000C4A RID: 3146
		internal const string ODataJsonLightCollectionDeserializer_CannotReadCollectionEnd = "ODataJsonLightCollectionDeserializer_CannotReadCollectionEnd";

		// Token: 0x04000C4B RID: 3147
		internal const string ODataJsonLightCollectionDeserializer_InvalidCollectionTypeName = "ODataJsonLightCollectionDeserializer_InvalidCollectionTypeName";

		// Token: 0x04000C4C RID: 3148
		internal const string ODataJsonLightEntityReferenceLinkDeserializer_EntityReferenceLinkMustBeObjectValue = "ODataJsonLightEntityReferenceLinkDeserializer_EntityReferenceLinkMustBeObjectValue";

		// Token: 0x04000C4D RID: 3149
		internal const string ODataJsonLightEntityReferenceLinkDeserializer_PropertyAnnotationForEntityReferenceLink = "ODataJsonLightEntityReferenceLinkDeserializer_PropertyAnnotationForEntityReferenceLink";

		// Token: 0x04000C4E RID: 3150
		internal const string ODataJsonLightEntityReferenceLinkDeserializer_InvalidAnnotationInEntityReferenceLink = "ODataJsonLightEntityReferenceLinkDeserializer_InvalidAnnotationInEntityReferenceLink";

		// Token: 0x04000C4F RID: 3151
		internal const string ODataJsonLightEntityReferenceLinkDeserializer_InvalidPropertyInEntityReferenceLink = "ODataJsonLightEntityReferenceLinkDeserializer_InvalidPropertyInEntityReferenceLink";

		// Token: 0x04000C50 RID: 3152
		internal const string ODataJsonLightEntityReferenceLinkDeserializer_MissingEntityReferenceLinkProperty = "ODataJsonLightEntityReferenceLinkDeserializer_MissingEntityReferenceLinkProperty";

		// Token: 0x04000C51 RID: 3153
		internal const string ODataJsonLightEntityReferenceLinkDeserializer_MultipleUriPropertiesInEntityReferenceLink = "ODataJsonLightEntityReferenceLinkDeserializer_MultipleUriPropertiesInEntityReferenceLink";

		// Token: 0x04000C52 RID: 3154
		internal const string ODataJsonLightEntityReferenceLinkDeserializer_EntityReferenceLinkUrlCannotBeNull = "ODataJsonLightEntityReferenceLinkDeserializer_EntityReferenceLinkUrlCannotBeNull";

		// Token: 0x04000C53 RID: 3155
		internal const string ODataJsonLightEntityReferenceLinkDeserializer_PropertyAnnotationForEntityReferenceLinks = "ODataJsonLightEntityReferenceLinkDeserializer_PropertyAnnotationForEntityReferenceLinks";

		// Token: 0x04000C54 RID: 3156
		internal const string ODataJsonLightEntityReferenceLinkDeserializer_InvalidEntityReferenceLinksPropertyFound = "ODataJsonLightEntityReferenceLinkDeserializer_InvalidEntityReferenceLinksPropertyFound";

		// Token: 0x04000C55 RID: 3157
		internal const string ODataJsonLightEntityReferenceLinkDeserializer_InvalidPropertyAnnotationInEntityReferenceLinks = "ODataJsonLightEntityReferenceLinkDeserializer_InvalidPropertyAnnotationInEntityReferenceLinks";

		// Token: 0x04000C56 RID: 3158
		internal const string ODataJsonLightEntityReferenceLinkDeserializer_ExpectedEntityReferenceLinksPropertyNotFound = "ODataJsonLightEntityReferenceLinkDeserializer_ExpectedEntityReferenceLinksPropertyNotFound";

		// Token: 0x04000C57 RID: 3159
		internal const string ODataJsonOperationsDeserializerUtils_OperationPropertyCannotBeNull = "ODataJsonOperationsDeserializerUtils_OperationPropertyCannotBeNull";

		// Token: 0x04000C58 RID: 3160
		internal const string ODataJsonOperationsDeserializerUtils_OperationsPropertyMustHaveObjectValue = "ODataJsonOperationsDeserializerUtils_OperationsPropertyMustHaveObjectValue";

		// Token: 0x04000C59 RID: 3161
		internal const string ODataJsonOperationsDeserializerUtils_RepeatMetadataValue = "ODataJsonOperationsDeserializerUtils_RepeatMetadataValue";

		// Token: 0x04000C5A RID: 3162
		internal const string ODataJsonOperationsDeserializerUtils_MetadataMustHaveArrayValue = "ODataJsonOperationsDeserializerUtils_MetadataMustHaveArrayValue";

		// Token: 0x04000C5B RID: 3163
		internal const string ODataJsonOperationsDeserializerUtils_OperationMetadataArrayExpectedAnObject = "ODataJsonOperationsDeserializerUtils_OperationMetadataArrayExpectedAnObject";

		// Token: 0x04000C5C RID: 3164
		internal const string ODataJsonOperationsDeserializerUtils_MultipleOptionalPropertiesInOperation = "ODataJsonOperationsDeserializerUtils_MultipleOptionalPropertiesInOperation";

		// Token: 0x04000C5D RID: 3165
		internal const string ODataJsonOperationsDeserializerUtils_MultipleTargetPropertiesInOperation = "ODataJsonOperationsDeserializerUtils_MultipleTargetPropertiesInOperation";

		// Token: 0x04000C5E RID: 3166
		internal const string ODataJsonOperationsDeserializerUtils_OperationMissingTargetProperty = "ODataJsonOperationsDeserializerUtils_OperationMissingTargetProperty";

		// Token: 0x04000C5F RID: 3167
		internal const string ODataJsonLightServiceDocumentDeserializer_DuplicatePropertiesInServiceDocument = "ODataJsonLightServiceDocumentDeserializer_DuplicatePropertiesInServiceDocument";

		// Token: 0x04000C60 RID: 3168
		internal const string ODataJsonLightServiceDocumentDeserializer_DuplicatePropertiesInResourceCollection = "ODataJsonLightServiceDocumentDeserializer_DuplicatePropertiesInResourceCollection";

		// Token: 0x04000C61 RID: 3169
		internal const string ODataJsonLightServiceDocumentDeserializer_MissingValuePropertyInServiceDocument = "ODataJsonLightServiceDocumentDeserializer_MissingValuePropertyInServiceDocument";

		// Token: 0x04000C62 RID: 3170
		internal const string ODataJsonLightServiceDocumentDeserializer_MissingRequiredPropertyInResourceCollection = "ODataJsonLightServiceDocumentDeserializer_MissingRequiredPropertyInResourceCollection";

		// Token: 0x04000C63 RID: 3171
		internal const string ODataJsonLightServiceDocumentDeserializer_PropertyAnnotationInServiceDocument = "ODataJsonLightServiceDocumentDeserializer_PropertyAnnotationInServiceDocument";

		// Token: 0x04000C64 RID: 3172
		internal const string ODataJsonLightServiceDocumentDeserializer_InstanceAnnotationInServiceDocument = "ODataJsonLightServiceDocumentDeserializer_InstanceAnnotationInServiceDocument";

		// Token: 0x04000C65 RID: 3173
		internal const string ODataJsonLightServiceDocumentDeserializer_PropertyAnnotationInResourceCollection = "ODataJsonLightServiceDocumentDeserializer_PropertyAnnotationInResourceCollection";

		// Token: 0x04000C66 RID: 3174
		internal const string ODataJsonLightServiceDocumentDeserializer_InstanceAnnotationInResourceCollection = "ODataJsonLightServiceDocumentDeserializer_InstanceAnnotationInResourceCollection";

		// Token: 0x04000C67 RID: 3175
		internal const string ODataJsonLightServiceDocumentDeserializer_UnexpectedPropertyInResourceCollection = "ODataJsonLightServiceDocumentDeserializer_UnexpectedPropertyInResourceCollection";

		// Token: 0x04000C68 RID: 3176
		internal const string ODataJsonLightServiceDocumentDeserializer_UnexpectedPropertyInServiceDocument = "ODataJsonLightServiceDocumentDeserializer_UnexpectedPropertyInServiceDocument";

		// Token: 0x04000C69 RID: 3177
		internal const string ODataJsonLightServiceDocumentDeserializer_PropertyAnnotationWithoutProperty = "ODataJsonLightServiceDocumentDeserializer_PropertyAnnotationWithoutProperty";

		// Token: 0x04000C6A RID: 3178
		internal const string ODataJsonLightParameterDeserializer_PropertyAnnotationForParameters = "ODataJsonLightParameterDeserializer_PropertyAnnotationForParameters";

		// Token: 0x04000C6B RID: 3179
		internal const string ODataJsonLightParameterDeserializer_PropertyAnnotationWithoutPropertyForParameters = "ODataJsonLightParameterDeserializer_PropertyAnnotationWithoutPropertyForParameters";

		// Token: 0x04000C6C RID: 3180
		internal const string ODataJsonLightParameterDeserializer_UnsupportedPrimitiveParameterType = "ODataJsonLightParameterDeserializer_UnsupportedPrimitiveParameterType";

		// Token: 0x04000C6D RID: 3181
		internal const string ODataJsonLightParameterDeserializer_NullCollectionExpected = "ODataJsonLightParameterDeserializer_NullCollectionExpected";

		// Token: 0x04000C6E RID: 3182
		internal const string ODataJsonLightParameterDeserializer_UnsupportedParameterTypeKind = "ODataJsonLightParameterDeserializer_UnsupportedParameterTypeKind";

		// Token: 0x04000C6F RID: 3183
		internal const string SelectedPropertiesNode_StarSegmentNotLastSegment = "SelectedPropertiesNode_StarSegmentNotLastSegment";

		// Token: 0x04000C70 RID: 3184
		internal const string SelectedPropertiesNode_StarSegmentAfterTypeSegment = "SelectedPropertiesNode_StarSegmentAfterTypeSegment";

		// Token: 0x04000C71 RID: 3185
		internal const string ODataJsonLightErrorDeserializer_PropertyAnnotationNotAllowedInErrorPayload = "ODataJsonLightErrorDeserializer_PropertyAnnotationNotAllowedInErrorPayload";

		// Token: 0x04000C72 RID: 3186
		internal const string ODataJsonLightErrorDeserializer_InstanceAnnotationNotAllowedInErrorPayload = "ODataJsonLightErrorDeserializer_InstanceAnnotationNotAllowedInErrorPayload";

		// Token: 0x04000C73 RID: 3187
		internal const string ODataJsonLightErrorDeserializer_PropertyAnnotationWithoutPropertyForError = "ODataJsonLightErrorDeserializer_PropertyAnnotationWithoutPropertyForError";

		// Token: 0x04000C74 RID: 3188
		internal const string ODataJsonLightErrorDeserializer_TopLevelErrorValueWithInvalidProperty = "ODataJsonLightErrorDeserializer_TopLevelErrorValueWithInvalidProperty";

		// Token: 0x04000C75 RID: 3189
		internal const string ODataConventionalUriBuilder_EntityTypeWithNoKeyProperties = "ODataConventionalUriBuilder_EntityTypeWithNoKeyProperties";

		// Token: 0x04000C76 RID: 3190
		internal const string ODataConventionalUriBuilder_NullKeyValue = "ODataConventionalUriBuilder_NullKeyValue";

		// Token: 0x04000C77 RID: 3191
		internal const string ODataEntryMetadataContext_EntityTypeWithNoKeyProperties = "ODataEntryMetadataContext_EntityTypeWithNoKeyProperties";

		// Token: 0x04000C78 RID: 3192
		internal const string ODataEntryMetadataContext_NullKeyValue = "ODataEntryMetadataContext_NullKeyValue";

		// Token: 0x04000C79 RID: 3193
		internal const string ODataEntryMetadataContext_KeyOrETagValuesMustBePrimitiveValues = "ODataEntryMetadataContext_KeyOrETagValuesMustBePrimitiveValues";

		// Token: 0x04000C7A RID: 3194
		internal const string EdmValueUtils_NonPrimitiveValue = "EdmValueUtils_NonPrimitiveValue";

		// Token: 0x04000C7B RID: 3195
		internal const string EdmValueUtils_PropertyDoesntExist = "EdmValueUtils_PropertyDoesntExist";

		// Token: 0x04000C7C RID: 3196
		internal const string JsonLightAnnotationGroupDeserializer_AnnotationGroupDeclarationWithoutName = "JsonLightAnnotationGroupDeserializer_AnnotationGroupDeclarationWithoutName";

		// Token: 0x04000C7D RID: 3197
		internal const string JsonLightAnnotationGroupDeserializer_InvalidAnnotationFoundInsideAnnotationGroup = "JsonLightAnnotationGroupDeserializer_InvalidAnnotationFoundInsideAnnotationGroup";

		// Token: 0x04000C7E RID: 3198
		internal const string JsonLightAnnotationGroupDeserializer_InvalidAnnotationFoundInsideNamedAnnotationGroup = "JsonLightAnnotationGroupDeserializer_InvalidAnnotationFoundInsideNamedAnnotationGroup";

		// Token: 0x04000C7F RID: 3199
		internal const string JsonLightAnnotationGroupDeserializer_EncounteredMultipleNameProperties = "JsonLightAnnotationGroupDeserializer_EncounteredMultipleNameProperties";

		// Token: 0x04000C80 RID: 3200
		internal const string JsonLightAnnotationGroupDeserializer_UndefinedAnnotationGroupReference = "JsonLightAnnotationGroupDeserializer_UndefinedAnnotationGroupReference";

		// Token: 0x04000C81 RID: 3201
		internal const string JsonLightAnnotationGroupDeserializer_MultipleAnnotationGroupsWithSameName = "JsonLightAnnotationGroupDeserializer_MultipleAnnotationGroupsWithSameName";

		// Token: 0x04000C82 RID: 3202
		internal const string ODataPrimitiveValue_CannotCreateODataPrimitiveValueFromNull = "ODataPrimitiveValue_CannotCreateODataPrimitiveValueFromNull";

		// Token: 0x04000C83 RID: 3203
		internal const string ODataPrimitiveValue_CannotCreateODataPrimitiveValueFromUnsupportedValueType = "ODataPrimitiveValue_CannotCreateODataPrimitiveValueFromUnsupportedValueType";

		// Token: 0x04000C84 RID: 3204
		internal const string ODataAnnotatable_InstanceAnnotationsOnlyOnODataError = "ODataAnnotatable_InstanceAnnotationsOnlyOnODataError";

		// Token: 0x04000C85 RID: 3205
		internal const string ODataInstanceAnnotation_NeedPeriodInName = "ODataInstanceAnnotation_NeedPeriodInName";

		// Token: 0x04000C86 RID: 3206
		internal const string ODataInstanceAnnotation_ReservedNamesNotAllowed = "ODataInstanceAnnotation_ReservedNamesNotAllowed";

		// Token: 0x04000C87 RID: 3207
		internal const string ODataInstanceAnnotation_BadTermName = "ODataInstanceAnnotation_BadTermName";

		// Token: 0x04000C88 RID: 3208
		internal const string ODataInstanceAnnotation_ValueCannotBeODataStreamReferenceValue = "ODataInstanceAnnotation_ValueCannotBeODataStreamReferenceValue";

		// Token: 0x04000C89 RID: 3209
		internal const string ODataJsonLightValueSerializer_MissingTypeNameOnComplex = "ODataJsonLightValueSerializer_MissingTypeNameOnComplex";

		// Token: 0x04000C8A RID: 3210
		internal const string ODataJsonLightValueSerializer_MissingTypeNameOnCollection = "ODataJsonLightValueSerializer_MissingTypeNameOnCollection";

		// Token: 0x04000C8B RID: 3211
		internal const string AtomInstanceAnnotation_MissingTermAttributeOnAnnotationElement = "AtomInstanceAnnotation_MissingTermAttributeOnAnnotationElement";

		// Token: 0x04000C8C RID: 3212
		internal const string AtomInstanceAnnotation_AttributeValueNotationUsedWithIncompatibleType = "AtomInstanceAnnotation_AttributeValueNotationUsedWithIncompatibleType";

		// Token: 0x04000C8D RID: 3213
		internal const string AtomInstanceAnnotation_AttributeValueNotationUsedOnNonEmptyElement = "AtomInstanceAnnotation_AttributeValueNotationUsedOnNonEmptyElement";

		// Token: 0x04000C8E RID: 3214
		internal const string AtomInstanceAnnotation_MultipleAttributeValueNotationAttributes = "AtomInstanceAnnotation_MultipleAttributeValueNotationAttributes";

		// Token: 0x04000C8F RID: 3215
		internal const string AnnotationFilterPattern_InvalidPatternMissingDot = "AnnotationFilterPattern_InvalidPatternMissingDot";

		// Token: 0x04000C90 RID: 3216
		internal const string AnnotationFilterPattern_InvalidPatternEmptySegment = "AnnotationFilterPattern_InvalidPatternEmptySegment";

		// Token: 0x04000C91 RID: 3217
		internal const string AnnotationFilterPattern_InvalidPatternWildCardInSegment = "AnnotationFilterPattern_InvalidPatternWildCardInSegment";

		// Token: 0x04000C92 RID: 3218
		internal const string AnnotationFilterPattern_InvalidPatternWildCardMustBeInLastSegment = "AnnotationFilterPattern_InvalidPatternWildCardMustBeInLastSegment";

		// Token: 0x04000C93 RID: 3219
		internal const string JsonFullMetadataLevel_MissingEntitySet = "JsonFullMetadataLevel_MissingEntitySet";

		// Token: 0x04000C94 RID: 3220
		internal const string ODataQueryUtils_DidNotFindServiceOperation = "ODataQueryUtils_DidNotFindServiceOperation";

		// Token: 0x04000C95 RID: 3221
		internal const string ODataQueryUtils_FoundMultipleServiceOperations = "ODataQueryUtils_FoundMultipleServiceOperations";

		// Token: 0x04000C96 RID: 3222
		internal const string ODataQueryUtils_CannotSetMetadataAnnotationOnPrimitiveType = "ODataQueryUtils_CannotSetMetadataAnnotationOnPrimitiveType";

		// Token: 0x04000C97 RID: 3223
		internal const string ODataQueryUtils_DidNotFindEntitySet = "ODataQueryUtils_DidNotFindEntitySet";

		// Token: 0x04000C98 RID: 3224
		internal const string BinaryOperatorQueryNode_InvalidOperandType = "BinaryOperatorQueryNode_InvalidOperandType";

		// Token: 0x04000C99 RID: 3225
		internal const string BinaryOperatorQueryNode_OperandsMustHaveSameTypes = "BinaryOperatorQueryNode_OperandsMustHaveSameTypes";

		// Token: 0x04000C9A RID: 3226
		internal const string SyntacticTree_UriMustBeAbsolute = "SyntacticTree_UriMustBeAbsolute";

		// Token: 0x04000C9B RID: 3227
		internal const string SyntacticTree_MaxDepthInvalid = "SyntacticTree_MaxDepthInvalid";

		// Token: 0x04000C9C RID: 3228
		internal const string SyntacticTree_InvalidSkipQueryOptionValue = "SyntacticTree_InvalidSkipQueryOptionValue";

		// Token: 0x04000C9D RID: 3229
		internal const string SyntacticTree_InvalidTopQueryOptionValue = "SyntacticTree_InvalidTopQueryOptionValue";

		// Token: 0x04000C9E RID: 3230
		internal const string SyntacticTree_InvalidInlineCountQueryOptionValue = "SyntacticTree_InvalidInlineCountQueryOptionValue";

		// Token: 0x04000C9F RID: 3231
		internal const string QueryOptionUtils_QueryParameterMustBeSpecifiedOnce = "QueryOptionUtils_QueryParameterMustBeSpecifiedOnce";

		// Token: 0x04000CA0 RID: 3232
		internal const string UriBuilder_NotSupportedClrLiteral = "UriBuilder_NotSupportedClrLiteral";

		// Token: 0x04000CA1 RID: 3233
		internal const string UriBuilder_NotSupportedQueryToken = "UriBuilder_NotSupportedQueryToken";

		// Token: 0x04000CA2 RID: 3234
		internal const string UriQueryExpressionParser_TooDeep = "UriQueryExpressionParser_TooDeep";

		// Token: 0x04000CA3 RID: 3235
		internal const string UriQueryExpressionParser_ExpressionExpected = "UriQueryExpressionParser_ExpressionExpected";

		// Token: 0x04000CA4 RID: 3236
		internal const string UriQueryExpressionParser_OpenParenExpected = "UriQueryExpressionParser_OpenParenExpected";

		// Token: 0x04000CA5 RID: 3237
		internal const string UriQueryExpressionParser_CloseParenOrCommaExpected = "UriQueryExpressionParser_CloseParenOrCommaExpected";

		// Token: 0x04000CA6 RID: 3238
		internal const string UriQueryExpressionParser_CloseParenOrOperatorExpected = "UriQueryExpressionParser_CloseParenOrOperatorExpected";

		// Token: 0x04000CA7 RID: 3239
		internal const string UriQueryExpressionParser_RepeatedVisitor = "UriQueryExpressionParser_RepeatedVisitor";

		// Token: 0x04000CA8 RID: 3240
		internal const string UriQueryExpressionParser_CannotCreateStarTokenFromNonStar = "UriQueryExpressionParser_CannotCreateStarTokenFromNonStar";

		// Token: 0x04000CA9 RID: 3241
		internal const string UriQueryExpressionParser_RangeVariableAlreadyDeclared = "UriQueryExpressionParser_RangeVariableAlreadyDeclared";

		// Token: 0x04000CAA RID: 3242
		internal const string UriQueryPathParser_RequestUriDoesNotHaveTheCorrectBaseUri = "UriQueryPathParser_RequestUriDoesNotHaveTheCorrectBaseUri";

		// Token: 0x04000CAB RID: 3243
		internal const string UriQueryPathParser_SyntaxError = "UriQueryPathParser_SyntaxError";

		// Token: 0x04000CAC RID: 3244
		internal const string UriQueryPathParser_TooManySegments = "UriQueryPathParser_TooManySegments";

		// Token: 0x04000CAD RID: 3245
		internal const string UriQueryPathParser_InvalidKeyValueLiteral = "UriQueryPathParser_InvalidKeyValueLiteral";

		// Token: 0x04000CAE RID: 3246
		internal const string PropertyInfoTypeAnnotation_CannotFindProperty = "PropertyInfoTypeAnnotation_CannotFindProperty";

		// Token: 0x04000CAF RID: 3247
		internal const string SelectionItemBinder_NonNavigationPathToken = "SelectionItemBinder_NonNavigationPathToken";

		// Token: 0x04000CB0 RID: 3248
		internal const string SelectTreeNormalizer_NonPathProperty = "SelectTreeNormalizer_NonPathProperty";

		// Token: 0x04000CB1 RID: 3249
		internal const string ExpandItem_NonEntityNavProp = "ExpandItem_NonEntityNavProp";

		// Token: 0x04000CB2 RID: 3250
		internal const string MetadataBinder_UnsupportedQueryTokenKind = "MetadataBinder_UnsupportedQueryTokenKind";

		// Token: 0x04000CB3 RID: 3251
		internal const string MetadataBinder_UnsupportedExtensionToken = "MetadataBinder_UnsupportedExtensionToken";

		// Token: 0x04000CB4 RID: 3252
		internal const string MetadataBinder_RootSegmentResourceNotFound = "MetadataBinder_RootSegmentResourceNotFound";

		// Token: 0x04000CB5 RID: 3253
		internal const string MetadataBinder_KeyValueApplicableOnlyToEntityType = "MetadataBinder_KeyValueApplicableOnlyToEntityType";

		// Token: 0x04000CB6 RID: 3254
		internal const string MetadataBinder_PropertyNotDeclared = "MetadataBinder_PropertyNotDeclared";

		// Token: 0x04000CB7 RID: 3255
		internal const string MetadataBinder_PropertyNotDeclaredOrNotKeyInKeyValue = "MetadataBinder_PropertyNotDeclaredOrNotKeyInKeyValue";

		// Token: 0x04000CB8 RID: 3256
		internal const string MetadataBinder_UnnamedKeyValueOnTypeWithMultipleKeyProperties = "MetadataBinder_UnnamedKeyValueOnTypeWithMultipleKeyProperties";

		// Token: 0x04000CB9 RID: 3257
		internal const string MetadataBinder_DuplicitKeyPropertyInKeyValues = "MetadataBinder_DuplicitKeyPropertyInKeyValues";

		// Token: 0x04000CBA RID: 3258
		internal const string MetadataBinder_NotAllKeyPropertiesSpecifiedInKeyValues = "MetadataBinder_NotAllKeyPropertiesSpecifiedInKeyValues";

		// Token: 0x04000CBB RID: 3259
		internal const string MetadataBinder_CannotConvertToType = "MetadataBinder_CannotConvertToType";

		// Token: 0x04000CBC RID: 3260
		internal const string MetadataBinder_NonQueryableServiceOperationWithKeyLookup = "MetadataBinder_NonQueryableServiceOperationWithKeyLookup";

		// Token: 0x04000CBD RID: 3261
		internal const string MetadataBinder_QueryServiceOperationOfNonEntityType = "MetadataBinder_QueryServiceOperationOfNonEntityType";

		// Token: 0x04000CBE RID: 3262
		internal const string MetadataBinder_ServiceOperationParameterMissing = "MetadataBinder_ServiceOperationParameterMissing";

		// Token: 0x04000CBF RID: 3263
		internal const string MetadataBinder_ServiceOperationParameterInvalidType = "MetadataBinder_ServiceOperationParameterInvalidType";

		// Token: 0x04000CC0 RID: 3264
		internal const string MetadataBinder_FilterExpressionNotSingleValue = "MetadataBinder_FilterExpressionNotSingleValue";

		// Token: 0x04000CC1 RID: 3265
		internal const string MetadataBinder_OrderByExpressionNotSingleValue = "MetadataBinder_OrderByExpressionNotSingleValue";

		// Token: 0x04000CC2 RID: 3266
		internal const string MetadataBinder_PropertyAccessWithoutParentParameter = "MetadataBinder_PropertyAccessWithoutParentParameter";

		// Token: 0x04000CC3 RID: 3267
		internal const string MetadataBinder_MultiValuePropertyNotSupportedInExpression = "MetadataBinder_MultiValuePropertyNotSupportedInExpression";

		// Token: 0x04000CC4 RID: 3268
		internal const string MetadataBinder_BinaryOperatorOperandNotSingleValue = "MetadataBinder_BinaryOperatorOperandNotSingleValue";

		// Token: 0x04000CC5 RID: 3269
		internal const string MetadataBinder_UnaryOperatorOperandNotSingleValue = "MetadataBinder_UnaryOperatorOperandNotSingleValue";

		// Token: 0x04000CC6 RID: 3270
		internal const string MetadataBinder_PropertyAccessSourceNotSingleValue = "MetadataBinder_PropertyAccessSourceNotSingleValue";

		// Token: 0x04000CC7 RID: 3271
		internal const string MetadataBinder_IncompatibleOperandsError = "MetadataBinder_IncompatibleOperandsError";

		// Token: 0x04000CC8 RID: 3272
		internal const string MetadataBinder_IncompatibleOperandError = "MetadataBinder_IncompatibleOperandError";

		// Token: 0x04000CC9 RID: 3273
		internal const string MetadataBinder_UnknownFunction = "MetadataBinder_UnknownFunction";

		// Token: 0x04000CCA RID: 3274
		internal const string MetadataBinder_FunctionArgumentNotSingleValue = "MetadataBinder_FunctionArgumentNotSingleValue";

		// Token: 0x04000CCB RID: 3275
		internal const string MetadataBinder_NoApplicableFunctionFound = "MetadataBinder_NoApplicableFunctionFound";

		// Token: 0x04000CCC RID: 3276
		internal const string MetadataBinder_BuiltInFunctionSignatureWithoutAReturnType = "MetadataBinder_BuiltInFunctionSignatureWithoutAReturnType";

		// Token: 0x04000CCD RID: 3277
		internal const string MetadataBinder_UnsupportedSystemQueryOption = "MetadataBinder_UnsupportedSystemQueryOption";

		// Token: 0x04000CCE RID: 3278
		internal const string MetadataBinder_BoundNodeCannotBeNull = "MetadataBinder_BoundNodeCannotBeNull";

		// Token: 0x04000CCF RID: 3279
		internal const string MetadataBinder_TopRequiresNonNegativeInteger = "MetadataBinder_TopRequiresNonNegativeInteger";

		// Token: 0x04000CD0 RID: 3280
		internal const string MetadataBinder_SkipRequiresNonNegativeInteger = "MetadataBinder_SkipRequiresNonNegativeInteger";

		// Token: 0x04000CD1 RID: 3281
		internal const string MetadataBinder_ServiceOperationWithoutResultKind = "MetadataBinder_ServiceOperationWithoutResultKind";

		// Token: 0x04000CD2 RID: 3282
		internal const string MetadataBinder_HierarchyNotFollowed = "MetadataBinder_HierarchyNotFollowed";

		// Token: 0x04000CD3 RID: 3283
		internal const string MetadataBinder_MustBeCalledOnRoot = "MetadataBinder_MustBeCalledOnRoot";

		// Token: 0x04000CD4 RID: 3284
		internal const string MetadataBinder_NoTypeSupported = "MetadataBinder_NoTypeSupported";

		// Token: 0x04000CD5 RID: 3285
		internal const string MetadataBinder_LambdaParentMustBeCollection = "MetadataBinder_LambdaParentMustBeCollection";

		// Token: 0x04000CD6 RID: 3286
		internal const string MetadataBinder_ParameterNotInScope = "MetadataBinder_ParameterNotInScope";

		// Token: 0x04000CD7 RID: 3287
		internal const string MetadataBinder_NullNavigationProperty = "MetadataBinder_NullNavigationProperty";

		// Token: 0x04000CD8 RID: 3288
		internal const string MetadataBinder_NavigationPropertyNotFollowingSingleEntityType = "MetadataBinder_NavigationPropertyNotFollowingSingleEntityType";

		// Token: 0x04000CD9 RID: 3289
		internal const string MetadataBinder_AnyAllExpressionNotSingleValue = "MetadataBinder_AnyAllExpressionNotSingleValue";

		// Token: 0x04000CDA RID: 3290
		internal const string MetadataBinder_CastOrIsOfExpressionWithWrongNumberOfOperands = "MetadataBinder_CastOrIsOfExpressionWithWrongNumberOfOperands";

		// Token: 0x04000CDB RID: 3291
		internal const string MetadataBinder_CastOrIsOfFunctionWithoutATypeArgument = "MetadataBinder_CastOrIsOfFunctionWithoutATypeArgument";

		// Token: 0x04000CDC RID: 3292
		internal const string MetadataBinder_CastOrIsOfCollectionsNotSupported = "MetadataBinder_CastOrIsOfCollectionsNotSupported";

		// Token: 0x04000CDD RID: 3293
		internal const string MetadataBinder_SpatialLengthFunctionWithInvalidArgs = "MetadataBinder_SpatialLengthFunctionWithInvalidArgs";

		// Token: 0x04000CDE RID: 3294
		internal const string MetadataBinder_SpatialLengthFunctionWithoutASingleValueArg = "MetadataBinder_SpatialLengthFunctionWithoutASingleValueArg";

		// Token: 0x04000CDF RID: 3295
		internal const string MetadataBinder_SpatialLengthFunctionWithOutLineStringArg = "MetadataBinder_SpatialLengthFunctionWithOutLineStringArg";

		// Token: 0x04000CE0 RID: 3296
		internal const string MetadataBinder_SpatialIntersectsFunctionWithInvalidArgs = "MetadataBinder_SpatialIntersectsFunctionWithInvalidArgs";

		// Token: 0x04000CE1 RID: 3297
		internal const string MetadataBinder_SpatialIntersectsFunctionWithoutASingleValueArg = "MetadataBinder_SpatialIntersectsFunctionWithoutASingleValueArg";

		// Token: 0x04000CE2 RID: 3298
		internal const string MetadataBinder_SpatialIntersectsFunctionWithInvalidArgTypes = "MetadataBinder_SpatialIntersectsFunctionWithInvalidArgTypes";

		// Token: 0x04000CE3 RID: 3299
		internal const string MetadataBinder_NonValidTypeArgument = "MetadataBinder_NonValidTypeArgument";

		// Token: 0x04000CE4 RID: 3300
		internal const string MetadataBinder_OperatorNotSupportedInThisVersion = "MetadataBinder_OperatorNotSupportedInThisVersion";

		// Token: 0x04000CE5 RID: 3301
		internal const string MetadataBinder_KeywordNotSupportedInThisRelease = "MetadataBinder_KeywordNotSupportedInThisRelease";

		// Token: 0x04000CE6 RID: 3302
		internal const string MetadataBinder_CollectionOpenPropertiesNotSupportedInThisRelease = "MetadataBinder_CollectionOpenPropertiesNotSupportedInThisRelease";

		// Token: 0x04000CE7 RID: 3303
		internal const string MetadataBinder_IllegalSegmentType = "MetadataBinder_IllegalSegmentType";

		// Token: 0x04000CE8 RID: 3304
		internal const string MetadataBinder_QueryOptionNotApplicable = "MetadataBinder_QueryOptionNotApplicable";

		// Token: 0x04000CE9 RID: 3305
		internal const string FunctionCallBinder_CannotFindASuitableOverload = "FunctionCallBinder_CannotFindASuitableOverload";

		// Token: 0x04000CEA RID: 3306
		internal const string FunctionCallBinder_NonSingleValueParent = "FunctionCallBinder_NonSingleValueParent";

		// Token: 0x04000CEB RID: 3307
		internal const string FunctionCallBinder_FoundInvalidFunctionImports = "FunctionCallBinder_FoundInvalidFunctionImports";

		// Token: 0x04000CEC RID: 3308
		internal const string FunctionCallBinder_BuiltInFunctionMustHaveHaveNullParent = "FunctionCallBinder_BuiltInFunctionMustHaveHaveNullParent";

		// Token: 0x04000CED RID: 3309
		internal const string FunctionCallBinder_CallingFunctionOnOpenProperty = "FunctionCallBinder_CallingFunctionOnOpenProperty";

		// Token: 0x04000CEE RID: 3310
		internal const string FunctionCallParser_DuplicateParameterName = "FunctionCallParser_DuplicateParameterName";

		// Token: 0x04000CEF RID: 3311
		internal const string ODataUriParser_InvalidInlineCount = "ODataUriParser_InvalidInlineCount";

		// Token: 0x04000CF0 RID: 3312
		internal const string CastBinder_ChildTypeIsNotEntity = "CastBinder_ChildTypeIsNotEntity";

		// Token: 0x04000CF1 RID: 3313
		internal const string BatchReferenceSegment_InvalidContentID = "BatchReferenceSegment_InvalidContentID";

		// Token: 0x04000CF2 RID: 3314
		internal const string SelectExpandBinder_UnknownPropertyType = "SelectExpandBinder_UnknownPropertyType";

		// Token: 0x04000CF3 RID: 3315
		internal const string SelectExpandBinder_CantFindProperty = "SelectExpandBinder_CantFindProperty";

		// Token: 0x04000CF4 RID: 3316
		internal const string SelectionItemBinder_NoExpandForSelectedProperty = "SelectionItemBinder_NoExpandForSelectedProperty";

		// Token: 0x04000CF5 RID: 3317
		internal const string SelectionItemBinder_NonPathSelectToken = "SelectionItemBinder_NonPathSelectToken";

		// Token: 0x04000CF6 RID: 3318
		internal const string SelectionItemBinder_NonEntityTypeSegment = "SelectionItemBinder_NonEntityTypeSegment";

		// Token: 0x04000CF7 RID: 3319
		internal const string SelectExpandPathBinder_FollowNonTypeSegment = "SelectExpandPathBinder_FollowNonTypeSegment";

		// Token: 0x04000CF8 RID: 3320
		internal const string SelectPropertyVisitor_SystemTokenInSelect = "SelectPropertyVisitor_SystemTokenInSelect";

		// Token: 0x04000CF9 RID: 3321
		internal const string SelectPropertyVisitor_InvalidSegmentInSelectPath = "SelectPropertyVisitor_InvalidSegmentInSelectPath";

		// Token: 0x04000CFA RID: 3322
		internal const string SelectPropertyVisitor_DisparateTypeSegmentsInSelectExpand = "SelectPropertyVisitor_DisparateTypeSegmentsInSelectExpand";

		// Token: 0x04000CFB RID: 3323
		internal const string SelectExpandClause_CannotDeleteFromAllSelection = "SelectExpandClause_CannotDeleteFromAllSelection";

		// Token: 0x04000CFC RID: 3324
		internal const string SegmentFactory_LinksSegmentNotFollowedByNavProp = "SegmentFactory_LinksSegmentNotFollowedByNavProp";

		// Token: 0x04000CFD RID: 3325
		internal const string ExpandItemBinder_TraversingANonNormalizedTree = "ExpandItemBinder_TraversingANonNormalizedTree";

		// Token: 0x04000CFE RID: 3326
		internal const string ExpandItemBinder_CannotFindType = "ExpandItemBinder_CannotFindType";

		// Token: 0x04000CFF RID: 3327
		internal const string ExpandItemBinder_PropertyIsNotANavigationProperty = "ExpandItemBinder_PropertyIsNotANavigationProperty";

		// Token: 0x04000D00 RID: 3328
		internal const string ExpandItemBinder_TypeSegmentNotFollowedByPath = "ExpandItemBinder_TypeSegmentNotFollowedByPath";

		// Token: 0x04000D01 RID: 3329
		internal const string ExpandItemBinder_PathTooDeep = "ExpandItemBinder_PathTooDeep";

		// Token: 0x04000D02 RID: 3330
		internal const string Nodes_CollectionNavigationNode_MustHaveSingleMultiplicity = "Nodes_CollectionNavigationNode_MustHaveSingleMultiplicity";

		// Token: 0x04000D03 RID: 3331
		internal const string Nodes_NonentityParameterQueryNodeWithEntityType = "Nodes_NonentityParameterQueryNodeWithEntityType";

		// Token: 0x04000D04 RID: 3332
		internal const string Nodes_EntityCollectionServiceOperationRequiresEntityReturnType = "Nodes_EntityCollectionServiceOperationRequiresEntityReturnType";

		// Token: 0x04000D05 RID: 3333
		internal const string Nodes_CollectionNavigationNode_MustHaveManyMultiplicity = "Nodes_CollectionNavigationNode_MustHaveManyMultiplicity";

		// Token: 0x04000D06 RID: 3334
		internal const string Nodes_PropertyAccessShouldBeNonEntityProperty = "Nodes_PropertyAccessShouldBeNonEntityProperty";

		// Token: 0x04000D07 RID: 3335
		internal const string Nodes_PropertyAccessTypeShouldNotBeCollection = "Nodes_PropertyAccessTypeShouldNotBeCollection";

		// Token: 0x04000D08 RID: 3336
		internal const string Nodes_PropertyAccessTypeMustBeCollection = "Nodes_PropertyAccessTypeMustBeCollection";

		// Token: 0x04000D09 RID: 3337
		internal const string Nodes_NonStaticEntitySetExpressionsAreNotSupportedInThisRelease = "Nodes_NonStaticEntitySetExpressionsAreNotSupportedInThisRelease";

		// Token: 0x04000D0A RID: 3338
		internal const string Nodes_CollectionFunctionCallNode_ItemTypeMustBePrimitiveOrComplex = "Nodes_CollectionFunctionCallNode_ItemTypeMustBePrimitiveOrComplex";

		// Token: 0x04000D0B RID: 3339
		internal const string Nodes_EntityCollectionFunctionCallNode_ItemTypeMustBeAnEntity = "Nodes_EntityCollectionFunctionCallNode_ItemTypeMustBeAnEntity";

		// Token: 0x04000D0C RID: 3340
		internal const string ExpandTreeNormalizer_CallAddTermsOnUnexpandedTerms = "ExpandTreeNormalizer_CallAddTermsOnUnexpandedTerms";

		// Token: 0x04000D0D RID: 3341
		internal const string ExpandTreeNormalizer_NonPathInPropertyChain = "ExpandTreeNormalizer_NonPathInPropertyChain";

		// Token: 0x04000D0E RID: 3342
		internal const string UriSelectParser_TermIsNotValid = "UriSelectParser_TermIsNotValid";

		// Token: 0x04000D0F RID: 3343
		internal const string UriSelectParser_FunctionsAreNotAllowed = "UriSelectParser_FunctionsAreNotAllowed";

		// Token: 0x04000D10 RID: 3344
		internal const string UriSelectParser_InvalidTopOption = "UriSelectParser_InvalidTopOption";

		// Token: 0x04000D11 RID: 3345
		internal const string UriSelectParser_InvalidSkipOption = "UriSelectParser_InvalidSkipOption";

		// Token: 0x04000D12 RID: 3346
		internal const string UriSelectParser_SystemTokenInSelectExpand = "UriSelectParser_SystemTokenInSelectExpand";

		// Token: 0x04000D13 RID: 3347
		internal const string UriParser_NeedServiceRootForThisOverload = "UriParser_NeedServiceRootForThisOverload";

		// Token: 0x04000D14 RID: 3348
		internal const string UriParser_UriMustBeAbsolute = "UriParser_UriMustBeAbsolute";

		// Token: 0x04000D15 RID: 3349
		internal const string UriParser_NegativeLimit = "UriParser_NegativeLimit";

		// Token: 0x04000D16 RID: 3350
		internal const string UriParser_ExpandCountExceeded = "UriParser_ExpandCountExceeded";

		// Token: 0x04000D17 RID: 3351
		internal const string UriParser_ExpandDepthExceeded = "UriParser_ExpandDepthExceeded";

		// Token: 0x04000D18 RID: 3352
		internal const string PathParser_ServiceOperationWithoutResultKindAttribute = "PathParser_ServiceOperationWithoutResultKindAttribute";

		// Token: 0x04000D19 RID: 3353
		internal const string PathParser_FunctionsAreNotSupported = "PathParser_FunctionsAreNotSupported";

		// Token: 0x04000D1A RID: 3354
		internal const string PathParser_ServiceOperationsWithSameName = "PathParser_ServiceOperationsWithSameName";

		// Token: 0x04000D1B RID: 3355
		internal const string PathParser_LinksNotSupported = "PathParser_LinksNotSupported";

		// Token: 0x04000D1C RID: 3356
		internal const string PathParser_CannotUseValueOnCollection = "PathParser_CannotUseValueOnCollection";

		// Token: 0x04000D1D RID: 3357
		internal const string PathParser_TypeMustBeRelatedToSet = "PathParser_TypeMustBeRelatedToSet";

		// Token: 0x04000D1E RID: 3358
		internal const string ODataFeed_MustNotContainBothNextPageLinkAndDeltaLink = "ODataFeed_MustNotContainBothNextPageLinkAndDeltaLink";

		// Token: 0x04000D1F RID: 3359
		internal const string ODataExpandPath_OnlyLastSegmentMustBeNavigationProperty = "ODataExpandPath_OnlyLastSegmentMustBeNavigationProperty";

		// Token: 0x04000D20 RID: 3360
		internal const string ODataExpandPath_InvalidExpandPathSegment = "ODataExpandPath_InvalidExpandPathSegment";

		// Token: 0x04000D21 RID: 3361
		internal const string ODataSelectPath_CannotEndInTypeSegment = "ODataSelectPath_CannotEndInTypeSegment";

		// Token: 0x04000D22 RID: 3362
		internal const string ODataSelectPath_InvalidSelectPathSegmentType = "ODataSelectPath_InvalidSelectPathSegmentType";

		// Token: 0x04000D23 RID: 3363
		internal const string ODataSelectPath_OperationSegmentCanOnlyBeLastSegment = "ODataSelectPath_OperationSegmentCanOnlyBeLastSegment";

		// Token: 0x04000D24 RID: 3364
		internal const string ODataSelectPath_NavPropSegmentCanOnlyBeLastSegment = "ODataSelectPath_NavPropSegmentCanOnlyBeLastSegment";

		// Token: 0x04000D25 RID: 3365
		internal const string RequestUriProcessor_EntitySetNotSpecified = "RequestUriProcessor_EntitySetNotSpecified";

		// Token: 0x04000D26 RID: 3366
		internal const string RequestUriProcessor_TargetEntitySetNotFound = "RequestUriProcessor_TargetEntitySetNotFound";

		// Token: 0x04000D27 RID: 3367
		internal const string RequestUriProcessor_FoundInvalidFunctionImport = "RequestUriProcessor_FoundInvalidFunctionImport";

		// Token: 0x04000D28 RID: 3368
		internal const string OperationSegment_ReturnTypeForMultipleOverloads = "OperationSegment_ReturnTypeForMultipleOverloads";

		// Token: 0x04000D29 RID: 3369
		internal const string OperationSegment_CannotReturnNull = "OperationSegment_CannotReturnNull";

		// Token: 0x04000D2A RID: 3370
		internal const string SingleValueFunctionCallNode_FunctionImportsWithLegacyConstructor = "SingleValueFunctionCallNode_FunctionImportsWithLegacyConstructor";

		// Token: 0x04000D2B RID: 3371
		internal const string SingleEntityFunctionCallNode_CallFunctionImportsUsingLegacyConstructor = "SingleEntityFunctionCallNode_CallFunctionImportsUsingLegacyConstructor";

		// Token: 0x04000D2C RID: 3372
		internal const string SegmentArgumentParser_TryConvertValuesForNamedValues = "SegmentArgumentParser_TryConvertValuesForNamedValues";

		// Token: 0x04000D2D RID: 3373
		internal const string SegmentArgumentParser_TryConvertValuesToNonPrimitive = "SegmentArgumentParser_TryConvertValuesToNonPrimitive";

		// Token: 0x04000D2E RID: 3374
		internal const string SegmentArgumentParser_TryConvertValuesForPositionalValues = "SegmentArgumentParser_TryConvertValuesForPositionalValues";

		// Token: 0x04000D2F RID: 3375
		internal const string FunctionOverloadResolver_NoSingleMatchFound = "FunctionOverloadResolver_NoSingleMatchFound";

		// Token: 0x04000D30 RID: 3376
		internal const string FunctionOverloadResolver_MultipleActionOverloads = "FunctionOverloadResolver_MultipleActionOverloads";

		// Token: 0x04000D31 RID: 3377
		internal const string RequestUriProcessor_EmptySegmentInRequestUrl = "RequestUriProcessor_EmptySegmentInRequestUrl";

		// Token: 0x04000D32 RID: 3378
		internal const string RequestUriProcessor_SyntaxError = "RequestUriProcessor_SyntaxError";

		// Token: 0x04000D33 RID: 3379
		internal const string RequestUriProcessor_CannotSpecifyAfterPostLinkSegment = "RequestUriProcessor_CannotSpecifyAfterPostLinkSegment";

		// Token: 0x04000D34 RID: 3380
		internal const string RequestUriProcessor_CountOnRoot = "RequestUriProcessor_CountOnRoot";

		// Token: 0x04000D35 RID: 3381
		internal const string RequestUriProcessor_MustBeLeafSegment = "RequestUriProcessor_MustBeLeafSegment";

		// Token: 0x04000D36 RID: 3382
		internal const string RequestUriProcessor_LinkSegmentMustBeFollowedByEntitySegment = "RequestUriProcessor_LinkSegmentMustBeFollowedByEntitySegment";

		// Token: 0x04000D37 RID: 3383
		internal const string RequestUriProcessor_MissingSegmentAfterLink = "RequestUriProcessor_MissingSegmentAfterLink";

		// Token: 0x04000D38 RID: 3384
		internal const string RequestUriProcessor_CountNotSupported = "RequestUriProcessor_CountNotSupported";

		// Token: 0x04000D39 RID: 3385
		internal const string RequestUriProcessor_CannotQuerySingletons = "RequestUriProcessor_CannotQuerySingletons";

		// Token: 0x04000D3A RID: 3386
		internal const string RequestUriProcessor_CannotQueryCollections = "RequestUriProcessor_CannotQueryCollections";

		// Token: 0x04000D3B RID: 3387
		internal const string RequestUriProcessor_SegmentDoesNotSupportKeyPredicates = "RequestUriProcessor_SegmentDoesNotSupportKeyPredicates";

		// Token: 0x04000D3C RID: 3388
		internal const string RequestUriProcessor_ValueSegmentAfterScalarPropertySegment = "RequestUriProcessor_ValueSegmentAfterScalarPropertySegment";

		// Token: 0x04000D3D RID: 3389
		internal const string RequestUriProcessor_InvalidTypeIdentifier_UnrelatedType = "RequestUriProcessor_InvalidTypeIdentifier_UnrelatedType";

		// Token: 0x04000D3E RID: 3390
		internal const string ResourceType_ComplexTypeCannotBeOpen = "ResourceType_ComplexTypeCannotBeOpen";

		// Token: 0x04000D3F RID: 3391
		internal const string BadRequest_ValuesCannotBeReturnedForSpatialTypes = "BadRequest_ValuesCannotBeReturnedForSpatialTypes";

		// Token: 0x04000D40 RID: 3392
		internal const string OpenNavigationPropertiesNotSupportedOnOpenTypes = "OpenNavigationPropertiesNotSupportedOnOpenTypes";

		// Token: 0x04000D41 RID: 3393
		internal const string BadRequest_ResourceCanBeCrossReferencedOnlyForBindOperation = "BadRequest_ResourceCanBeCrossReferencedOnlyForBindOperation";

		// Token: 0x04000D42 RID: 3394
		internal const string DataServiceConfiguration_ResponseVersionIsBiggerThanProtocolVersion = "DataServiceConfiguration_ResponseVersionIsBiggerThanProtocolVersion";

		// Token: 0x04000D43 RID: 3395
		internal const string BadRequest_KeyCountMismatch = "BadRequest_KeyCountMismatch";

		// Token: 0x04000D44 RID: 3396
		internal const string RequestUriProcessor_KeysMustBeNamed = "RequestUriProcessor_KeysMustBeNamed";

		// Token: 0x04000D45 RID: 3397
		internal const string RequestUriProcessor_ResourceNotFound = "RequestUriProcessor_ResourceNotFound";

		// Token: 0x04000D46 RID: 3398
		internal const string RequestUriProcessor_BatchedActionOnEntityCreatedInSameChangeset = "RequestUriProcessor_BatchedActionOnEntityCreatedInSameChangeset";

		// Token: 0x04000D47 RID: 3399
		internal const string RequestUriProcessor_IEnumerableServiceOperationsCannotBeFurtherComposed = "RequestUriProcessor_IEnumerableServiceOperationsCannotBeFurtherComposed";

		// Token: 0x04000D48 RID: 3400
		internal const string RequestUriProcessor_Forbidden = "RequestUriProcessor_Forbidden";

		// Token: 0x04000D49 RID: 3401
		internal const string RequestUriProcessor_OperationSegmentBoundToANonEntityType = "RequestUriProcessor_OperationSegmentBoundToANonEntityType";

		// Token: 0x04000D4A RID: 3402
		internal const string General_InternalError = "General_InternalError";

		// Token: 0x04000D4B RID: 3403
		internal const string ExceptionUtils_CheckIntegerNotNegative = "ExceptionUtils_CheckIntegerNotNegative";

		// Token: 0x04000D4C RID: 3404
		internal const string ExceptionUtils_CheckIntegerPositive = "ExceptionUtils_CheckIntegerPositive";

		// Token: 0x04000D4D RID: 3405
		internal const string ExceptionUtils_CheckLongPositive = "ExceptionUtils_CheckLongPositive";

		// Token: 0x04000D4E RID: 3406
		internal const string ExceptionUtils_ArgumentStringNullOrEmpty = "ExceptionUtils_ArgumentStringNullOrEmpty";

		// Token: 0x04000D4F RID: 3407
		internal const string ExpressionToken_IdentifierExpected = "ExpressionToken_IdentifierExpected";

		// Token: 0x04000D50 RID: 3408
		internal const string ExpressionLexer_UnterminatedStringLiteral = "ExpressionLexer_UnterminatedStringLiteral";

		// Token: 0x04000D51 RID: 3409
		internal const string ExpressionLexer_InvalidCharacter = "ExpressionLexer_InvalidCharacter";

		// Token: 0x04000D52 RID: 3410
		internal const string ExpressionLexer_SyntaxError = "ExpressionLexer_SyntaxError";

		// Token: 0x04000D53 RID: 3411
		internal const string ExpressionLexer_UnterminatedLiteral = "ExpressionLexer_UnterminatedLiteral";

		// Token: 0x04000D54 RID: 3412
		internal const string ExpressionLexer_DigitExpected = "ExpressionLexer_DigitExpected";

		// Token: 0x04000D55 RID: 3413
		internal const string ExpressionLexer_UnbalancedBracketExpression = "ExpressionLexer_UnbalancedBracketExpression";

		// Token: 0x04000D56 RID: 3414
		internal const string UriQueryExpressionParser_UnrecognizedLiteral = "UriQueryExpressionParser_UnrecognizedLiteral";

		// Token: 0x04000D57 RID: 3415
		internal const string JsonReader_UnexpectedComma = "JsonReader_UnexpectedComma";

		// Token: 0x04000D58 RID: 3416
		internal const string JsonReader_MultipleTopLevelValues = "JsonReader_MultipleTopLevelValues";

		// Token: 0x04000D59 RID: 3417
		internal const string JsonReader_EndOfInputWithOpenScope = "JsonReader_EndOfInputWithOpenScope";

		// Token: 0x04000D5A RID: 3418
		internal const string JsonReader_UnexpectedToken = "JsonReader_UnexpectedToken";

		// Token: 0x04000D5B RID: 3419
		internal const string JsonReader_UnrecognizedToken = "JsonReader_UnrecognizedToken";

		// Token: 0x04000D5C RID: 3420
		internal const string JsonReader_MissingColon = "JsonReader_MissingColon";

		// Token: 0x04000D5D RID: 3421
		internal const string JsonReader_UnrecognizedEscapeSequence = "JsonReader_UnrecognizedEscapeSequence";

		// Token: 0x04000D5E RID: 3422
		internal const string JsonReader_UnexpectedEndOfString = "JsonReader_UnexpectedEndOfString";

		// Token: 0x04000D5F RID: 3423
		internal const string JsonReader_InvalidNumberFormat = "JsonReader_InvalidNumberFormat";

		// Token: 0x04000D60 RID: 3424
		internal const string JsonReader_MissingComma = "JsonReader_MissingComma";

		// Token: 0x04000D61 RID: 3425
		internal const string JsonReader_InvalidPropertyNameOrUnexpectedComma = "JsonReader_InvalidPropertyNameOrUnexpectedComma";

		// Token: 0x04000D62 RID: 3426
		internal const string JsonReaderExtensions_UnexpectedNodeDetected = "JsonReaderExtensions_UnexpectedNodeDetected";

		// Token: 0x04000D63 RID: 3427
		internal const string JsonReaderExtensions_CannotReadPropertyValueAsString = "JsonReaderExtensions_CannotReadPropertyValueAsString";

		// Token: 0x04000D64 RID: 3428
		internal const string JsonReaderExtensions_CannotReadValueAsString = "JsonReaderExtensions_CannotReadValueAsString";

		// Token: 0x04000D65 RID: 3429
		internal const string JsonReaderExtensions_CannotReadValueAsDouble = "JsonReaderExtensions_CannotReadValueAsDouble";

		// Token: 0x04000D66 RID: 3430
		private static TextRes loader;

		// Token: 0x04000D67 RID: 3431
		private ResourceManager resources;
	}
}
