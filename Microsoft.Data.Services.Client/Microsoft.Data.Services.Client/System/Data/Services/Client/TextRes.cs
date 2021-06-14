using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace System.Data.Services.Client
{
	// Token: 0x02000139 RID: 313
	internal sealed class TextRes
	{
		// Token: 0x06000B3A RID: 2874 RVA: 0x0002C764 File Offset: 0x0002A964
		internal TextRes()
		{
			this.resources = new ResourceManager("System.Data.Services.Client", base.GetType().Assembly);
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x0002C788 File Offset: 0x0002A988
		private static TextRes GetLoader()
		{
			if (TextRes.loader == null)
			{
				TextRes value = new TextRes();
				Interlocked.CompareExchange<TextRes>(ref TextRes.loader, value, null);
			}
			return TextRes.loader;
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000B3C RID: 2876 RVA: 0x0002C7B4 File Offset: 0x0002A9B4
		private static CultureInfo Culture
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000B3D RID: 2877 RVA: 0x0002C7B7 File Offset: 0x0002A9B7
		public static ResourceManager Resources
		{
			get
			{
				return TextRes.GetLoader().resources;
			}
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x0002C7C4 File Offset: 0x0002A9C4
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

		// Token: 0x06000B3F RID: 2879 RVA: 0x0002C848 File Offset: 0x0002AA48
		public static string GetString(string name)
		{
			TextRes textRes = TextRes.GetLoader();
			if (textRes == null)
			{
				return null;
			}
			return textRes.resources.GetString(name, TextRes.Culture);
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x0002C871 File Offset: 0x0002AA71
		public static string GetString(string name, out bool usedFallback)
		{
			usedFallback = false;
			return TextRes.GetString(name);
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x0002C87C File Offset: 0x0002AA7C
		public static object GetObject(string name)
		{
			TextRes textRes = TextRes.GetLoader();
			if (textRes == null)
			{
				return null;
			}
			return textRes.resources.GetObject(name, TextRes.Culture);
		}

		// Token: 0x0400060C RID: 1548
		internal const string Batch_ExpectedContentType = "Batch_ExpectedContentType";

		// Token: 0x0400060D RID: 1549
		internal const string Batch_ExpectedResponse = "Batch_ExpectedResponse";

		// Token: 0x0400060E RID: 1550
		internal const string Batch_IncompleteResponseCount = "Batch_IncompleteResponseCount";

		// Token: 0x0400060F RID: 1551
		internal const string Batch_UnexpectedContent = "Batch_UnexpectedContent";

		// Token: 0x04000610 RID: 1552
		internal const string Context_BaseUri = "Context_BaseUri";

		// Token: 0x04000611 RID: 1553
		internal const string Context_BaseUriRequired = "Context_BaseUriRequired";

		// Token: 0x04000612 RID: 1554
		internal const string Context_ResolveReturnedInvalidUri = "Context_ResolveReturnedInvalidUri";

		// Token: 0x04000613 RID: 1555
		internal const string Context_RequestUriIsRelativeBaseUriRequired = "Context_RequestUriIsRelativeBaseUriRequired";

		// Token: 0x04000614 RID: 1556
		internal const string Context_ResolveEntitySetOrBaseUriRequired = "Context_ResolveEntitySetOrBaseUriRequired";

		// Token: 0x04000615 RID: 1557
		internal const string Context_CannotConvertKey = "Context_CannotConvertKey";

		// Token: 0x04000616 RID: 1558
		internal const string Context_TrackingExpectsAbsoluteUri = "Context_TrackingExpectsAbsoluteUri";

		// Token: 0x04000617 RID: 1559
		internal const string Context_LocationHeaderExpectsAbsoluteUri = "Context_LocationHeaderExpectsAbsoluteUri";

		// Token: 0x04000618 RID: 1560
		internal const string Context_LinkResourceInsertFailure = "Context_LinkResourceInsertFailure";

		// Token: 0x04000619 RID: 1561
		internal const string Context_InternalError = "Context_InternalError";

		// Token: 0x0400061A RID: 1562
		internal const string Context_BatchExecuteError = "Context_BatchExecuteError";

		// Token: 0x0400061B RID: 1563
		internal const string Context_EntitySetName = "Context_EntitySetName";

		// Token: 0x0400061C RID: 1564
		internal const string Context_BatchNotSupportedForNamedStreams = "Context_BatchNotSupportedForNamedStreams";

		// Token: 0x0400061D RID: 1565
		internal const string Context_SetSaveStreamWithoutNamedStreamEditLink = "Context_SetSaveStreamWithoutNamedStreamEditLink";

		// Token: 0x0400061E RID: 1566
		internal const string Content_EntityWithoutKey = "Content_EntityWithoutKey";

		// Token: 0x0400061F RID: 1567
		internal const string Content_EntityIsNotEntityType = "Content_EntityIsNotEntityType";

		// Token: 0x04000620 RID: 1568
		internal const string Context_EntityNotContained = "Context_EntityNotContained";

		// Token: 0x04000621 RID: 1569
		internal const string Context_EntityAlreadyContained = "Context_EntityAlreadyContained";

		// Token: 0x04000622 RID: 1570
		internal const string Context_DifferentEntityAlreadyContained = "Context_DifferentEntityAlreadyContained";

		// Token: 0x04000623 RID: 1571
		internal const string Context_DidNotOriginateAsync = "Context_DidNotOriginateAsync";

		// Token: 0x04000624 RID: 1572
		internal const string Context_AsyncAlreadyDone = "Context_AsyncAlreadyDone";

		// Token: 0x04000625 RID: 1573
		internal const string Context_OperationCanceled = "Context_OperationCanceled";

		// Token: 0x04000626 RID: 1574
		internal const string Context_PropertyNotSupportedForMaxDataServiceVersionGreaterThanX = "Context_PropertyNotSupportedForMaxDataServiceVersionGreaterThanX";

		// Token: 0x04000627 RID: 1575
		internal const string Context_NoLoadWithInsertEnd = "Context_NoLoadWithInsertEnd";

		// Token: 0x04000628 RID: 1576
		internal const string Context_NoRelationWithInsertEnd = "Context_NoRelationWithInsertEnd";

		// Token: 0x04000629 RID: 1577
		internal const string Context_NoRelationWithDeleteEnd = "Context_NoRelationWithDeleteEnd";

		// Token: 0x0400062A RID: 1578
		internal const string Context_RelationAlreadyContained = "Context_RelationAlreadyContained";

		// Token: 0x0400062B RID: 1579
		internal const string Context_RelationNotRefOrCollection = "Context_RelationNotRefOrCollection";

		// Token: 0x0400062C RID: 1580
		internal const string Context_AddLinkCollectionOnly = "Context_AddLinkCollectionOnly";

		// Token: 0x0400062D RID: 1581
		internal const string Context_AddRelatedObjectCollectionOnly = "Context_AddRelatedObjectCollectionOnly";

		// Token: 0x0400062E RID: 1582
		internal const string Context_AddRelatedObjectSourceDeleted = "Context_AddRelatedObjectSourceDeleted";

		// Token: 0x0400062F RID: 1583
		internal const string Context_SetLinkReferenceOnly = "Context_SetLinkReferenceOnly";

		// Token: 0x04000630 RID: 1584
		internal const string Context_NoContentTypeForMediaLink = "Context_NoContentTypeForMediaLink";

		// Token: 0x04000631 RID: 1585
		internal const string Context_BatchNotSupportedForMediaLink = "Context_BatchNotSupportedForMediaLink";

		// Token: 0x04000632 RID: 1586
		internal const string Context_UnexpectedZeroRawRead = "Context_UnexpectedZeroRawRead";

		// Token: 0x04000633 RID: 1587
		internal const string Context_VersionNotSupported = "Context_VersionNotSupported";

		// Token: 0x04000634 RID: 1588
		internal const string Context_ResponseVersionIsBiggerThanProtocolVersion = "Context_ResponseVersionIsBiggerThanProtocolVersion";

		// Token: 0x04000635 RID: 1589
		internal const string Context_RequestVersionIsBiggerThanProtocolVersion = "Context_RequestVersionIsBiggerThanProtocolVersion";

		// Token: 0x04000636 RID: 1590
		internal const string Context_ChildResourceExists = "Context_ChildResourceExists";

		// Token: 0x04000637 RID: 1591
		internal const string Context_ContentTypeRequiredForNamedStream = "Context_ContentTypeRequiredForNamedStream";

		// Token: 0x04000638 RID: 1592
		internal const string Context_EntityNotMediaLinkEntry = "Context_EntityNotMediaLinkEntry";

		// Token: 0x04000639 RID: 1593
		internal const string Context_MLEWithoutSaveStream = "Context_MLEWithoutSaveStream";

		// Token: 0x0400063A RID: 1594
		internal const string Context_SetSaveStreamOnMediaEntryProperty = "Context_SetSaveStreamOnMediaEntryProperty";

		// Token: 0x0400063B RID: 1595
		internal const string Context_SetSaveStreamWithoutEditMediaLink = "Context_SetSaveStreamWithoutEditMediaLink";

		// Token: 0x0400063C RID: 1596
		internal const string Context_SetSaveStreamOnInvalidEntityState = "Context_SetSaveStreamOnInvalidEntityState";

		// Token: 0x0400063D RID: 1597
		internal const string Context_EntityDoesNotContainNamedStream = "Context_EntityDoesNotContainNamedStream";

		// Token: 0x0400063E RID: 1598
		internal const string Context_MissingSelfAndEditLinkForNamedStream = "Context_MissingSelfAndEditLinkForNamedStream";

		// Token: 0x0400063F RID: 1599
		internal const string Context_BothLocationAndIdMustBeSpecified = "Context_BothLocationAndIdMustBeSpecified";

		// Token: 0x04000640 RID: 1600
		internal const string Context_BodyOperationParametersNotAllowedWithGet = "Context_BodyOperationParametersNotAllowedWithGet";

		// Token: 0x04000641 RID: 1601
		internal const string Context_MissingOperationParameterName = "Context_MissingOperationParameterName";

		// Token: 0x04000642 RID: 1602
		internal const string Context_DuplicateUriOperationParameterName = "Context_DuplicateUriOperationParameterName";

		// Token: 0x04000643 RID: 1603
		internal const string Context_DuplicateBodyOperationParameterName = "Context_DuplicateBodyOperationParameterName";

		// Token: 0x04000644 RID: 1604
		internal const string Context_NullKeysAreNotSupported = "Context_NullKeysAreNotSupported";

		// Token: 0x04000645 RID: 1605
		internal const string Context_ExecuteExpectsGetOrPost = "Context_ExecuteExpectsGetOrPost";

		// Token: 0x04000646 RID: 1606
		internal const string Context_EndExecuteExpectedVoidResponse = "Context_EndExecuteExpectedVoidResponse";

		// Token: 0x04000647 RID: 1607
		internal const string Context_NullElementInOperationParameterArray = "Context_NullElementInOperationParameterArray";

		// Token: 0x04000648 RID: 1608
		internal const string Context_EntityMetadataBuilderIsRequired = "Context_EntityMetadataBuilderIsRequired";

		// Token: 0x04000649 RID: 1609
		internal const string Context_BuildingRequestAndSendingRequestCannotBeUsedTogether = "Context_BuildingRequestAndSendingRequestCannotBeUsedTogether";

		// Token: 0x0400064A RID: 1610
		internal const string Context_CannotChangeStateToAdded = "Context_CannotChangeStateToAdded";

		// Token: 0x0400064B RID: 1611
		internal const string Context_CannotChangeStateToModifiedIfNotUnchanged = "Context_CannotChangeStateToModifiedIfNotUnchanged";

		// Token: 0x0400064C RID: 1612
		internal const string Context_CannotChangeStateIfAdded = "Context_CannotChangeStateIfAdded";

		// Token: 0x0400064D RID: 1613
		internal const string Context_OnMessageCreatingReturningNull = "Context_OnMessageCreatingReturningNull";

		// Token: 0x0400064E RID: 1614
		internal const string Context_SendingRequest_InvalidWhenUsingOnMessageCreating = "Context_SendingRequest_InvalidWhenUsingOnMessageCreating";

		// Token: 0x0400064F RID: 1615
		internal const string DataServiceClientFormat_AtomEventsOnlySupportedWithAtomFormat = "DataServiceClientFormat_AtomEventsOnlySupportedWithAtomFormat";

		// Token: 0x04000650 RID: 1616
		internal const string DataServiceClientFormat_LoadServiceModelRequired = "DataServiceClientFormat_LoadServiceModelRequired";

		// Token: 0x04000651 RID: 1617
		internal const string DataServiceClientFormat_JsonUnsupportedForLessThanV3 = "DataServiceClientFormat_JsonUnsupportedForLessThanV3";

		// Token: 0x04000652 RID: 1618
		internal const string DataServiceClientFormat_ValidServiceModelRequiredForJson = "DataServiceClientFormat_ValidServiceModelRequiredForJson";

		// Token: 0x04000653 RID: 1619
		internal const string DataServiceClientFormat_JsonVerboseUnsupported = "DataServiceClientFormat_JsonVerboseUnsupported";

		// Token: 0x04000654 RID: 1620
		internal const string Collection_NullCollectionReference = "Collection_NullCollectionReference";

		// Token: 0x04000655 RID: 1621
		internal const string ClientType_MissingOpenProperty = "ClientType_MissingOpenProperty";

		// Token: 0x04000656 RID: 1622
		internal const string Clienttype_MultipleOpenProperty = "Clienttype_MultipleOpenProperty";

		// Token: 0x04000657 RID: 1623
		internal const string ClientType_MissingProperty = "ClientType_MissingProperty";

		// Token: 0x04000658 RID: 1624
		internal const string ClientType_KeysMustBeSimpleTypes = "ClientType_KeysMustBeSimpleTypes";

		// Token: 0x04000659 RID: 1625
		internal const string ClientType_KeysOnDifferentDeclaredType = "ClientType_KeysOnDifferentDeclaredType";

		// Token: 0x0400065A RID: 1626
		internal const string ClientType_MissingMimeTypeProperty = "ClientType_MissingMimeTypeProperty";

		// Token: 0x0400065B RID: 1627
		internal const string ClientType_MissingMimeTypeDataProperty = "ClientType_MissingMimeTypeDataProperty";

		// Token: 0x0400065C RID: 1628
		internal const string ClientType_MissingMediaEntryProperty = "ClientType_MissingMediaEntryProperty";

		// Token: 0x0400065D RID: 1629
		internal const string ClientType_NoSettableFields = "ClientType_NoSettableFields";

		// Token: 0x0400065E RID: 1630
		internal const string ClientType_MultipleImplementationNotSupported = "ClientType_MultipleImplementationNotSupported";

		// Token: 0x0400065F RID: 1631
		internal const string ClientType_NullOpenProperties = "ClientType_NullOpenProperties";

		// Token: 0x04000660 RID: 1632
		internal const string ClientType_Ambiguous = "ClientType_Ambiguous";

		// Token: 0x04000661 RID: 1633
		internal const string ClientType_UnsupportedType = "ClientType_UnsupportedType";

		// Token: 0x04000662 RID: 1634
		internal const string ClientType_CollectionOfCollectionNotSupported = "ClientType_CollectionOfCollectionNotSupported";

		// Token: 0x04000663 RID: 1635
		internal const string ClientType_CollectionPropertyNotSupportedInV2AndBelow = "ClientType_CollectionPropertyNotSupportedInV2AndBelow";

		// Token: 0x04000664 RID: 1636
		internal const string ClientType_MultipleTypesWithSameName = "ClientType_MultipleTypesWithSameName";

		// Token: 0x04000665 RID: 1637
		internal const string WebUtil_CollectionTypeNotSupportedInV2OrBelow = "WebUtil_CollectionTypeNotSupportedInV2OrBelow";

		// Token: 0x04000666 RID: 1638
		internal const string WebUtil_TypeMismatchInCollection = "WebUtil_TypeMismatchInCollection";

		// Token: 0x04000667 RID: 1639
		internal const string WebUtil_TypeMismatchInNonPropertyCollection = "WebUtil_TypeMismatchInNonPropertyCollection";

		// Token: 0x04000668 RID: 1640
		internal const string ClientTypeCache_NonEntityTypeCannotContainEntityProperties = "ClientTypeCache_NonEntityTypeCannotContainEntityProperties";

		// Token: 0x04000669 RID: 1641
		internal const string DataServiceException_GeneralError = "DataServiceException_GeneralError";

		// Token: 0x0400066A RID: 1642
		internal const string Deserialize_GetEnumerator = "Deserialize_GetEnumerator";

		// Token: 0x0400066B RID: 1643
		internal const string Deserialize_Current = "Deserialize_Current";

		// Token: 0x0400066C RID: 1644
		internal const string Deserialize_MixedTextWithComment = "Deserialize_MixedTextWithComment";

		// Token: 0x0400066D RID: 1645
		internal const string Deserialize_ExpectingSimpleValue = "Deserialize_ExpectingSimpleValue";

		// Token: 0x0400066E RID: 1646
		internal const string Deserialize_MismatchAtomLinkLocalSimple = "Deserialize_MismatchAtomLinkLocalSimple";

		// Token: 0x0400066F RID: 1647
		internal const string Deserialize_MismatchAtomLinkFeedPropertyNotCollection = "Deserialize_MismatchAtomLinkFeedPropertyNotCollection";

		// Token: 0x04000670 RID: 1648
		internal const string Deserialize_MismatchAtomLinkEntryPropertyIsCollection = "Deserialize_MismatchAtomLinkEntryPropertyIsCollection";

		// Token: 0x04000671 RID: 1649
		internal const string Deserialize_NoLocationHeader = "Deserialize_NoLocationHeader";

		// Token: 0x04000672 RID: 1650
		internal const string Deserialize_ServerException = "Deserialize_ServerException";

		// Token: 0x04000673 RID: 1651
		internal const string Deserialize_MissingIdElement = "Deserialize_MissingIdElement";

		// Token: 0x04000674 RID: 1652
		internal const string Collection_NullCollectionNotSupported = "Collection_NullCollectionNotSupported";

		// Token: 0x04000675 RID: 1653
		internal const string Collection_NullNonPropertyCollectionNotSupported = "Collection_NullNonPropertyCollectionNotSupported";

		// Token: 0x04000676 RID: 1654
		internal const string Collection_NullCollectionItemsNotSupported = "Collection_NullCollectionItemsNotSupported";

		// Token: 0x04000677 RID: 1655
		internal const string Collection_ComplexTypesInCollectionOfPrimitiveTypesNotAllowed = "Collection_ComplexTypesInCollectionOfPrimitiveTypesNotAllowed";

		// Token: 0x04000678 RID: 1656
		internal const string Collection_PrimitiveTypesInCollectionOfComplexTypesNotAllowed = "Collection_PrimitiveTypesInCollectionOfComplexTypesNotAllowed";

		// Token: 0x04000679 RID: 1657
		internal const string EntityDescriptor_MissingSelfEditLink = "EntityDescriptor_MissingSelfEditLink";

		// Token: 0x0400067A RID: 1658
		internal const string EpmSourceTree_InvalidSourcePath = "EpmSourceTree_InvalidSourcePath";

		// Token: 0x0400067B RID: 1659
		internal const string EpmSourceTree_DuplicateEpmAttrsWithSameSourceName = "EpmSourceTree_DuplicateEpmAttrsWithSameSourceName";

		// Token: 0x0400067C RID: 1660
		internal const string EpmSourceTree_EndsWithNonPrimitiveType = "EpmSourceTree_EndsWithNonPrimitiveType";

		// Token: 0x0400067D RID: 1661
		internal const string EpmSourceTree_InaccessiblePropertyOnType = "EpmSourceTree_InaccessiblePropertyOnType";

		// Token: 0x0400067E RID: 1662
		internal const string EpmSourceTree_NamedStreamCannotBeMapped = "EpmSourceTree_NamedStreamCannotBeMapped";

		// Token: 0x0400067F RID: 1663
		internal const string EpmSourceTree_SpatialTypeCannotBeMapped = "EpmSourceTree_SpatialTypeCannotBeMapped";

		// Token: 0x04000680 RID: 1664
		internal const string EpmSourceTree_CollectionPropertyCannotBeMapped = "EpmSourceTree_CollectionPropertyCannotBeMapped";

		// Token: 0x04000681 RID: 1665
		internal const string EpmTargetTree_InvalidTargetPath = "EpmTargetTree_InvalidTargetPath";

		// Token: 0x04000682 RID: 1666
		internal const string EpmTargetTree_AttributeInMiddle = "EpmTargetTree_AttributeInMiddle";

		// Token: 0x04000683 RID: 1667
		internal const string EpmTargetTree_DuplicateEpmAttrsWithSameTargetName = "EpmTargetTree_DuplicateEpmAttrsWithSameTargetName";

		// Token: 0x04000684 RID: 1668
		internal const string HttpProcessUtility_ContentTypeMissing = "HttpProcessUtility_ContentTypeMissing";

		// Token: 0x04000685 RID: 1669
		internal const string HttpProcessUtility_MediaTypeMissingValue = "HttpProcessUtility_MediaTypeMissingValue";

		// Token: 0x04000686 RID: 1670
		internal const string HttpProcessUtility_MediaTypeRequiresSemicolonBeforeParameter = "HttpProcessUtility_MediaTypeRequiresSemicolonBeforeParameter";

		// Token: 0x04000687 RID: 1671
		internal const string HttpProcessUtility_MediaTypeRequiresSlash = "HttpProcessUtility_MediaTypeRequiresSlash";

		// Token: 0x04000688 RID: 1672
		internal const string HttpProcessUtility_MediaTypeRequiresSubType = "HttpProcessUtility_MediaTypeRequiresSubType";

		// Token: 0x04000689 RID: 1673
		internal const string HttpProcessUtility_MediaTypeUnspecified = "HttpProcessUtility_MediaTypeUnspecified";

		// Token: 0x0400068A RID: 1674
		internal const string HttpProcessUtility_EncodingNotSupported = "HttpProcessUtility_EncodingNotSupported";

		// Token: 0x0400068B RID: 1675
		internal const string HttpProcessUtility_EscapeCharWithoutQuotes = "HttpProcessUtility_EscapeCharWithoutQuotes";

		// Token: 0x0400068C RID: 1676
		internal const string HttpProcessUtility_EscapeCharAtEnd = "HttpProcessUtility_EscapeCharAtEnd";

		// Token: 0x0400068D RID: 1677
		internal const string HttpProcessUtility_ClosingQuoteNotFound = "HttpProcessUtility_ClosingQuoteNotFound";

		// Token: 0x0400068E RID: 1678
		internal const string MaterializeFromAtom_CountNotPresent = "MaterializeFromAtom_CountNotPresent";

		// Token: 0x0400068F RID: 1679
		internal const string MaterializeFromAtom_TopLevelLinkNotAvailable = "MaterializeFromAtom_TopLevelLinkNotAvailable";

		// Token: 0x04000690 RID: 1680
		internal const string MaterializeFromAtom_CollectionKeyNotPresentInLinkTable = "MaterializeFromAtom_CollectionKeyNotPresentInLinkTable";

		// Token: 0x04000691 RID: 1681
		internal const string MaterializeFromAtom_GetNestLinkForFlatCollection = "MaterializeFromAtom_GetNestLinkForFlatCollection";

		// Token: 0x04000692 RID: 1682
		internal const string ODataRequestMessage_GetStreamMethodNotSupported = "ODataRequestMessage_GetStreamMethodNotSupported";

		// Token: 0x04000693 RID: 1683
		internal const string Util_EmptyString = "Util_EmptyString";

		// Token: 0x04000694 RID: 1684
		internal const string Util_EmptyArray = "Util_EmptyArray";

		// Token: 0x04000695 RID: 1685
		internal const string Util_NullArrayElement = "Util_NullArrayElement";

		// Token: 0x04000696 RID: 1686
		internal const string ALinq_UnsupportedExpression = "ALinq_UnsupportedExpression";

		// Token: 0x04000697 RID: 1687
		internal const string ALinq_CouldNotConvert = "ALinq_CouldNotConvert";

		// Token: 0x04000698 RID: 1688
		internal const string ALinq_MethodNotSupported = "ALinq_MethodNotSupported";

		// Token: 0x04000699 RID: 1689
		internal const string ALinq_UnaryNotSupported = "ALinq_UnaryNotSupported";

		// Token: 0x0400069A RID: 1690
		internal const string ALinq_BinaryNotSupported = "ALinq_BinaryNotSupported";

		// Token: 0x0400069B RID: 1691
		internal const string ALinq_ConstantNotSupported = "ALinq_ConstantNotSupported";

		// Token: 0x0400069C RID: 1692
		internal const string ALinq_TypeBinaryNotSupported = "ALinq_TypeBinaryNotSupported";

		// Token: 0x0400069D RID: 1693
		internal const string ALinq_ConditionalNotSupported = "ALinq_ConditionalNotSupported";

		// Token: 0x0400069E RID: 1694
		internal const string ALinq_ParameterNotSupported = "ALinq_ParameterNotSupported";

		// Token: 0x0400069F RID: 1695
		internal const string ALinq_MemberAccessNotSupported = "ALinq_MemberAccessNotSupported";

		// Token: 0x040006A0 RID: 1696
		internal const string ALinq_LambdaNotSupported = "ALinq_LambdaNotSupported";

		// Token: 0x040006A1 RID: 1697
		internal const string ALinq_NewNotSupported = "ALinq_NewNotSupported";

		// Token: 0x040006A2 RID: 1698
		internal const string ALinq_MemberInitNotSupported = "ALinq_MemberInitNotSupported";

		// Token: 0x040006A3 RID: 1699
		internal const string ALinq_ListInitNotSupported = "ALinq_ListInitNotSupported";

		// Token: 0x040006A4 RID: 1700
		internal const string ALinq_NewArrayNotSupported = "ALinq_NewArrayNotSupported";

		// Token: 0x040006A5 RID: 1701
		internal const string ALinq_InvocationNotSupported = "ALinq_InvocationNotSupported";

		// Token: 0x040006A6 RID: 1702
		internal const string ALinq_QueryOptionsOnlyAllowedOnLeafNodes = "ALinq_QueryOptionsOnlyAllowedOnLeafNodes";

		// Token: 0x040006A7 RID: 1703
		internal const string ALinq_CantExpand = "ALinq_CantExpand";

		// Token: 0x040006A8 RID: 1704
		internal const string ALinq_CantCastToUnsupportedPrimitive = "ALinq_CantCastToUnsupportedPrimitive";

		// Token: 0x040006A9 RID: 1705
		internal const string ALinq_CantNavigateWithoutKeyPredicate = "ALinq_CantNavigateWithoutKeyPredicate";

		// Token: 0x040006AA RID: 1706
		internal const string ALinq_CanOnlyApplyOneKeyPredicate = "ALinq_CanOnlyApplyOneKeyPredicate";

		// Token: 0x040006AB RID: 1707
		internal const string ALinq_CantTranslateExpression = "ALinq_CantTranslateExpression";

		// Token: 0x040006AC RID: 1708
		internal const string ALinq_TranslationError = "ALinq_TranslationError";

		// Token: 0x040006AD RID: 1709
		internal const string ALinq_CantAddQueryOption = "ALinq_CantAddQueryOption";

		// Token: 0x040006AE RID: 1710
		internal const string ALinq_CantAddDuplicateQueryOption = "ALinq_CantAddDuplicateQueryOption";

		// Token: 0x040006AF RID: 1711
		internal const string ALinq_CantAddAstoriaQueryOption = "ALinq_CantAddAstoriaQueryOption";

		// Token: 0x040006B0 RID: 1712
		internal const string ALinq_CantAddQueryOptionStartingWithDollarSign = "ALinq_CantAddQueryOptionStartingWithDollarSign";

		// Token: 0x040006B1 RID: 1713
		internal const string ALinq_CantReferToPublicField = "ALinq_CantReferToPublicField";

		// Token: 0x040006B2 RID: 1714
		internal const string ALinq_QueryOptionsOnlyAllowedOnSingletons = "ALinq_QueryOptionsOnlyAllowedOnSingletons";

		// Token: 0x040006B3 RID: 1715
		internal const string ALinq_QueryOptionOutOfOrder = "ALinq_QueryOptionOutOfOrder";

		// Token: 0x040006B4 RID: 1716
		internal const string ALinq_CannotAddCountOption = "ALinq_CannotAddCountOption";

		// Token: 0x040006B5 RID: 1717
		internal const string ALinq_CannotAddCountOptionConflict = "ALinq_CannotAddCountOptionConflict";

		// Token: 0x040006B6 RID: 1718
		internal const string ALinq_ProjectionOnlyAllowedOnLeafNodes = "ALinq_ProjectionOnlyAllowedOnLeafNodes";

		// Token: 0x040006B7 RID: 1719
		internal const string ALinq_ProjectionCanOnlyHaveOneProjection = "ALinq_ProjectionCanOnlyHaveOneProjection";

		// Token: 0x040006B8 RID: 1720
		internal const string ALinq_ProjectionMemberAssignmentMismatch = "ALinq_ProjectionMemberAssignmentMismatch";

		// Token: 0x040006B9 RID: 1721
		internal const string ALinq_InvalidExpressionInNavigationPath = "ALinq_InvalidExpressionInNavigationPath";

		// Token: 0x040006BA RID: 1722
		internal const string ALinq_ExpressionNotSupportedInProjectionToEntity = "ALinq_ExpressionNotSupportedInProjectionToEntity";

		// Token: 0x040006BB RID: 1723
		internal const string ALinq_ExpressionNotSupportedInProjection = "ALinq_ExpressionNotSupportedInProjection";

		// Token: 0x040006BC RID: 1724
		internal const string ALinq_CannotConstructKnownEntityTypes = "ALinq_CannotConstructKnownEntityTypes";

		// Token: 0x040006BD RID: 1725
		internal const string ALinq_CannotCreateConstantEntity = "ALinq_CannotCreateConstantEntity";

		// Token: 0x040006BE RID: 1726
		internal const string ALinq_PropertyNamesMustMatchInProjections = "ALinq_PropertyNamesMustMatchInProjections";

		// Token: 0x040006BF RID: 1727
		internal const string ALinq_CanOnlyProjectTheLeaf = "ALinq_CanOnlyProjectTheLeaf";

		// Token: 0x040006C0 RID: 1728
		internal const string ALinq_CannotProjectWithExplicitExpansion = "ALinq_CannotProjectWithExplicitExpansion";

		// Token: 0x040006C1 RID: 1729
		internal const string ALinq_CollectionPropertyNotSupportedInOrderBy = "ALinq_CollectionPropertyNotSupportedInOrderBy";

		// Token: 0x040006C2 RID: 1730
		internal const string ALinq_CollectionPropertyNotSupportedInWhere = "ALinq_CollectionPropertyNotSupportedInWhere";

		// Token: 0x040006C3 RID: 1731
		internal const string ALinq_CollectionMemberAccessNotSupportedInNavigation = "ALinq_CollectionMemberAccessNotSupportedInNavigation";

		// Token: 0x040006C4 RID: 1732
		internal const string ALinq_LinkPropertyNotSupportedInExpression = "ALinq_LinkPropertyNotSupportedInExpression";

		// Token: 0x040006C5 RID: 1733
		internal const string ALinq_OfTypeArgumentNotAvailable = "ALinq_OfTypeArgumentNotAvailable";

		// Token: 0x040006C6 RID: 1734
		internal const string ALinq_CannotUseTypeFiltersMultipleTimes = "ALinq_CannotUseTypeFiltersMultipleTimes";

		// Token: 0x040006C7 RID: 1735
		internal const string ALinq_ExpressionCannotEndWithTypeAs = "ALinq_ExpressionCannotEndWithTypeAs";

		// Token: 0x040006C8 RID: 1736
		internal const string ALinq_TypeAsNotSupportedForMaxDataServiceVersionLessThan3 = "ALinq_TypeAsNotSupportedForMaxDataServiceVersionLessThan3";

		// Token: 0x040006C9 RID: 1737
		internal const string ALinq_MethodNotSupportedForMaxDataServiceVersionLessThanX = "ALinq_MethodNotSupportedForMaxDataServiceVersionLessThanX";

		// Token: 0x040006CA RID: 1738
		internal const string ALinq_TypeAsArgumentNotEntityType = "ALinq_TypeAsArgumentNotEntityType";

		// Token: 0x040006CB RID: 1739
		internal const string ALinq_InvalidSourceForAnyAll = "ALinq_InvalidSourceForAnyAll";

		// Token: 0x040006CC RID: 1740
		internal const string ALinq_AnyAllNotSupportedInOrderBy = "ALinq_AnyAllNotSupportedInOrderBy";

		// Token: 0x040006CD RID: 1741
		internal const string ALinq_FormatQueryOptionNotSupported = "ALinq_FormatQueryOptionNotSupported";

		// Token: 0x040006CE RID: 1742
		internal const string DSKAttribute_MustSpecifyAtleastOnePropertyName = "DSKAttribute_MustSpecifyAtleastOnePropertyName";

		// Token: 0x040006CF RID: 1743
		internal const string DataServiceCollection_LoadRequiresTargetCollectionObserved = "DataServiceCollection_LoadRequiresTargetCollectionObserved";

		// Token: 0x040006D0 RID: 1744
		internal const string DataServiceCollection_CannotStopTrackingChildCollection = "DataServiceCollection_CannotStopTrackingChildCollection";

		// Token: 0x040006D1 RID: 1745
		internal const string DataServiceCollection_OperationForTrackedOnly = "DataServiceCollection_OperationForTrackedOnly";

		// Token: 0x040006D2 RID: 1746
		internal const string DataServiceCollection_CannotDetermineContextFromItems = "DataServiceCollection_CannotDetermineContextFromItems";

		// Token: 0x040006D3 RID: 1747
		internal const string DataServiceCollection_InsertIntoTrackedButNotLoadedCollection = "DataServiceCollection_InsertIntoTrackedButNotLoadedCollection";

		// Token: 0x040006D4 RID: 1748
		internal const string DataServiceCollection_MultipleLoadAsyncOperationsAtTheSameTime = "DataServiceCollection_MultipleLoadAsyncOperationsAtTheSameTime";

		// Token: 0x040006D5 RID: 1749
		internal const string DataServiceCollection_LoadAsyncNoParamsWithoutParentEntity = "DataServiceCollection_LoadAsyncNoParamsWithoutParentEntity";

		// Token: 0x040006D6 RID: 1750
		internal const string DataServiceCollection_LoadAsyncRequiresDataServiceQuery = "DataServiceCollection_LoadAsyncRequiresDataServiceQuery";

		// Token: 0x040006D7 RID: 1751
		internal const string DataBinding_DataServiceCollectionArgumentMustHaveEntityType = "DataBinding_DataServiceCollectionArgumentMustHaveEntityType";

		// Token: 0x040006D8 RID: 1752
		internal const string DataBinding_CollectionPropertySetterValueHasObserver = "DataBinding_CollectionPropertySetterValueHasObserver";

		// Token: 0x040006D9 RID: 1753
		internal const string DataBinding_DataServiceCollectionChangedUnknownActionCollection = "DataBinding_DataServiceCollectionChangedUnknownActionCollection";

		// Token: 0x040006DA RID: 1754
		internal const string DataBinding_CollectionChangedUnknownActionCollection = "DataBinding_CollectionChangedUnknownActionCollection";

		// Token: 0x040006DB RID: 1755
		internal const string DataBinding_BindingOperation_DetachedSource = "DataBinding_BindingOperation_DetachedSource";

		// Token: 0x040006DC RID: 1756
		internal const string DataBinding_BindingOperation_ArrayItemNull = "DataBinding_BindingOperation_ArrayItemNull";

		// Token: 0x040006DD RID: 1757
		internal const string DataBinding_BindingOperation_ArrayItemNotEntity = "DataBinding_BindingOperation_ArrayItemNotEntity";

		// Token: 0x040006DE RID: 1758
		internal const string DataBinding_Util_UnknownEntitySetName = "DataBinding_Util_UnknownEntitySetName";

		// Token: 0x040006DF RID: 1759
		internal const string DataBinding_EntityAlreadyInCollection = "DataBinding_EntityAlreadyInCollection";

		// Token: 0x040006E0 RID: 1760
		internal const string DataBinding_NotifyPropertyChangedNotImpl = "DataBinding_NotifyPropertyChangedNotImpl";

		// Token: 0x040006E1 RID: 1761
		internal const string DataBinding_NotifyCollectionChangedNotImpl = "DataBinding_NotifyCollectionChangedNotImpl";

		// Token: 0x040006E2 RID: 1762
		internal const string DataBinding_ComplexObjectAssociatedWithMultipleEntities = "DataBinding_ComplexObjectAssociatedWithMultipleEntities";

		// Token: 0x040006E3 RID: 1763
		internal const string DataBinding_CollectionAssociatedWithMultipleEntities = "DataBinding_CollectionAssociatedWithMultipleEntities";

		// Token: 0x040006E4 RID: 1764
		internal const string AtomParser_SingleEntry_NoneFound = "AtomParser_SingleEntry_NoneFound";

		// Token: 0x040006E5 RID: 1765
		internal const string AtomParser_SingleEntry_MultipleFound = "AtomParser_SingleEntry_MultipleFound";

		// Token: 0x040006E6 RID: 1766
		internal const string AtomParser_SingleEntry_ExpectedFeedOrEntry = "AtomParser_SingleEntry_ExpectedFeedOrEntry";

		// Token: 0x040006E7 RID: 1767
		internal const string AtomMaterializer_CannotAssignNull = "AtomMaterializer_CannotAssignNull";

		// Token: 0x040006E8 RID: 1768
		internal const string AtomMaterializer_EntryIntoCollectionMismatch = "AtomMaterializer_EntryIntoCollectionMismatch";

		// Token: 0x040006E9 RID: 1769
		internal const string AtomMaterializer_EntryToAccessIsNull = "AtomMaterializer_EntryToAccessIsNull";

		// Token: 0x040006EA RID: 1770
		internal const string AtomMaterializer_EntryToInitializeIsNull = "AtomMaterializer_EntryToInitializeIsNull";

		// Token: 0x040006EB RID: 1771
		internal const string AtomMaterializer_ProjectEntityTypeMismatch = "AtomMaterializer_ProjectEntityTypeMismatch";

		// Token: 0x040006EC RID: 1772
		internal const string AtomMaterializer_PropertyMissing = "AtomMaterializer_PropertyMissing";

		// Token: 0x040006ED RID: 1773
		internal const string AtomMaterializer_PropertyNotExpectedEntry = "AtomMaterializer_PropertyNotExpectedEntry";

		// Token: 0x040006EE RID: 1774
		internal const string AtomMaterializer_DataServiceCollectionNotSupportedForNonEntities = "AtomMaterializer_DataServiceCollectionNotSupportedForNonEntities";

		// Token: 0x040006EF RID: 1775
		internal const string AtomMaterializer_NoParameterlessCtorForCollectionProperty = "AtomMaterializer_NoParameterlessCtorForCollectionProperty";

		// Token: 0x040006F0 RID: 1776
		internal const string AtomMaterializer_InvalidCollectionItem = "AtomMaterializer_InvalidCollectionItem";

		// Token: 0x040006F1 RID: 1777
		internal const string AtomMaterializer_InvalidEntityType = "AtomMaterializer_InvalidEntityType";

		// Token: 0x040006F2 RID: 1778
		internal const string AtomMaterializer_InvalidNonEntityType = "AtomMaterializer_InvalidNonEntityType";

		// Token: 0x040006F3 RID: 1779
		internal const string AtomMaterializer_CollectionExpectedCollection = "AtomMaterializer_CollectionExpectedCollection";

		// Token: 0x040006F4 RID: 1780
		internal const string AtomMaterializer_InvalidResponsePayload = "AtomMaterializer_InvalidResponsePayload";

		// Token: 0x040006F5 RID: 1781
		internal const string AtomMaterializer_InvalidContentTypeEncountered = "AtomMaterializer_InvalidContentTypeEncountered";

		// Token: 0x040006F6 RID: 1782
		internal const string AtomMaterializer_MaterializationTypeError = "AtomMaterializer_MaterializationTypeError";

		// Token: 0x040006F7 RID: 1783
		internal const string AtomMaterializer_ResetAfterEnumeratorCreationError = "AtomMaterializer_ResetAfterEnumeratorCreationError";

		// Token: 0x040006F8 RID: 1784
		internal const string AtomMaterializer_TypeShouldBeCollectionError = "AtomMaterializer_TypeShouldBeCollectionError";

		// Token: 0x040006F9 RID: 1785
		internal const string Serializer_LoopsNotAllowedInComplexTypes = "Serializer_LoopsNotAllowedInComplexTypes";

		// Token: 0x040006FA RID: 1786
		internal const string Serializer_LoopsNotAllowedInNonPropertyComplexTypes = "Serializer_LoopsNotAllowedInNonPropertyComplexTypes";

		// Token: 0x040006FB RID: 1787
		internal const string Serializer_InvalidCollectionParamterItemType = "Serializer_InvalidCollectionParamterItemType";

		// Token: 0x040006FC RID: 1788
		internal const string Serializer_NullCollectionParamterItemValue = "Serializer_NullCollectionParamterItemValue";

		// Token: 0x040006FD RID: 1789
		internal const string Serializer_InvalidParameterType = "Serializer_InvalidParameterType";

		// Token: 0x040006FE RID: 1790
		internal const string Serializer_UriDoesNotContainParameterAlias = "Serializer_UriDoesNotContainParameterAlias";

		// Token: 0x040006FF RID: 1791
		internal const string DataServiceQuery_EnumerationNotSupported = "DataServiceQuery_EnumerationNotSupported";

		// Token: 0x04000700 RID: 1792
		internal const string Context_SendingRequestEventArgsNotHttp = "Context_SendingRequestEventArgsNotHttp";

		// Token: 0x04000701 RID: 1793
		internal const string General_InternalError = "General_InternalError";

		// Token: 0x04000702 RID: 1794
		internal const string ODataMetadataBuilder_MissingEntitySetUri = "ODataMetadataBuilder_MissingEntitySetUri";

		// Token: 0x04000703 RID: 1795
		internal const string ODataMetadataBuilder_MissingSegmentForEntitySetUriSuffix = "ODataMetadataBuilder_MissingSegmentForEntitySetUriSuffix";

		// Token: 0x04000704 RID: 1796
		internal const string ODataMetadataBuilder_MissingEntityInstanceUri = "ODataMetadataBuilder_MissingEntityInstanceUri";

		// Token: 0x04000705 RID: 1797
		internal const string EdmValueUtils_UnsupportedPrimitiveType = "EdmValueUtils_UnsupportedPrimitiveType";

		// Token: 0x04000706 RID: 1798
		internal const string EdmValueUtils_IncorrectPrimitiveTypeKind = "EdmValueUtils_IncorrectPrimitiveTypeKind";

		// Token: 0x04000707 RID: 1799
		internal const string EdmValueUtils_IncorrectPrimitiveTypeKindNoTypeName = "EdmValueUtils_IncorrectPrimitiveTypeKindNoTypeName";

		// Token: 0x04000708 RID: 1800
		internal const string EdmValueUtils_CannotConvertTypeToClrValue = "EdmValueUtils_CannotConvertTypeToClrValue";

		// Token: 0x04000709 RID: 1801
		internal const string DataServiceRequest_FailGetCount = "DataServiceRequest_FailGetCount";

		// Token: 0x0400070A RID: 1802
		internal const string Context_ExecuteExpectedVoidResponse = "Context_ExecuteExpectedVoidResponse";

		// Token: 0x0400070B RID: 1803
		private static TextRes loader;

		// Token: 0x0400070C RID: 1804
		private ResourceManager resources;
	}
}
