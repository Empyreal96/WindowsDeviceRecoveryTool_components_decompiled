using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData
{
	// Token: 0x02000246 RID: 582
	internal static class WriterValidationUtils
	{
		// Token: 0x06001299 RID: 4761 RVA: 0x00045C20 File Offset: 0x00043E20
		internal static void ValidateMessageWriterSettings(ODataMessageWriterSettings messageWriterSettings, bool writingResponse)
		{
			if (messageWriterSettings.BaseUri != null && !messageWriterSettings.BaseUri.IsAbsoluteUri)
			{
				throw new ODataException(Strings.WriterValidationUtils_MessageWriterSettingsBaseUriMustBeNullOrAbsolute(UriUtilsCommon.UriToString(messageWriterSettings.BaseUri)));
			}
			if (messageWriterSettings.HasJsonPaddingFunction() && !writingResponse)
			{
				throw new ODataException(Strings.WriterValidationUtils_MessageWriterSettingsJsonPaddingOnRequestMessage);
			}
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x00045C74 File Offset: 0x00043E74
		internal static void ValidatePropertyNotNull(ODataProperty property)
		{
			if (property == null)
			{
				throw new ODataException(Strings.WriterValidationUtils_PropertyMustNotBeNull);
			}
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x00045C84 File Offset: 0x00043E84
		internal static void ValidatePropertyName(string propertyName)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				throw new ODataException(Strings.WriterValidationUtils_PropertiesMustHaveNonEmptyName);
			}
			ValidationUtils.ValidatePropertyName(propertyName);
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x00045CA0 File Offset: 0x00043EA0
		internal static IEdmProperty ValidatePropertyDefined(string propertyName, IEdmStructuredType owningStructuredType)
		{
			if (owningStructuredType == null)
			{
				return null;
			}
			return owningStructuredType.FindProperty(propertyName);
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x00045CBC File Offset: 0x00043EBC
		internal static IEdmNavigationProperty ValidateNavigationPropertyDefined(string propertyName, IEdmEntityType owningEntityType)
		{
			if (owningEntityType == null)
			{
				return null;
			}
			IEdmProperty edmProperty = WriterValidationUtils.ValidatePropertyDefined(propertyName, owningEntityType);
			if (edmProperty == null)
			{
				throw new ODataException(Strings.ValidationUtils_OpenNavigationProperty(propertyName, owningEntityType.ODataFullName()));
			}
			if (edmProperty.PropertyKind != EdmPropertyKind.Navigation)
			{
				throw new ODataException(Strings.ValidationUtils_NavigationPropertyExpected(propertyName, owningEntityType.ODataFullName(), edmProperty.PropertyKind.ToString()));
			}
			return (IEdmNavigationProperty)edmProperty;
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x00045D1C File Offset: 0x00043F1C
		internal static void ValidateEntryInExpandedLink(IEdmEntityType entryEntityType, IEdmEntityType parentNavigationPropertyType)
		{
			if (parentNavigationPropertyType == null)
			{
				return;
			}
			if (!parentNavigationPropertyType.IsAssignableFrom(entryEntityType))
			{
				throw new ODataException(Strings.WriterValidationUtils_EntryTypeInExpandedLinkNotCompatibleWithNavigationPropertyType(entryEntityType.ODataFullName(), parentNavigationPropertyType.ODataFullName()));
			}
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x00045D42 File Offset: 0x00043F42
		internal static void ValidateAssociationLink(ODataAssociationLink associationLink, ODataVersion version, bool writingResponse)
		{
			ODataVersionChecker.CheckAssociationLinks(version);
			if (!writingResponse)
			{
				throw new ODataException(Strings.WriterValidationUtils_AssociationLinkInRequest(associationLink.Name));
			}
			ValidationUtils.ValidateAssociationLink(associationLink);
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x00045D64 File Offset: 0x00043F64
		internal static void ValidateCanWriteOperation(ODataOperation operation, bool writingResponse)
		{
			if (!writingResponse)
			{
				throw new ODataException(Strings.WriterValidationUtils_OperationInRequest(operation.Metadata));
			}
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x00045D7A File Offset: 0x00043F7A
		internal static void ValidateFeedAtEnd(ODataFeed feed, bool writingRequest, ODataVersion version)
		{
			if (feed.NextPageLink != null)
			{
				if (writingRequest)
				{
					throw new ODataException(Strings.WriterValidationUtils_NextPageLinkInRequest);
				}
				ODataVersionChecker.CheckNextLink(version);
			}
			if (feed.DeltaLink != null)
			{
				ODataVersionChecker.CheckDeltaLink(version);
			}
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x00045DB2 File Offset: 0x00043FB2
		internal static void ValidateEntryAtStart(ODataEntry entry)
		{
			WriterValidationUtils.ValidateEntryId(entry.Id);
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x00045DBF File Offset: 0x00043FBF
		internal static void ValidateEntryAtEnd(ODataEntry entry)
		{
			WriterValidationUtils.ValidateEntryId(entry.Id);
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x00045DCC File Offset: 0x00043FCC
		internal static void ValidateStreamReferenceValue(ODataStreamReferenceValue streamReference, bool isDefaultStream)
		{
			if (streamReference.ContentType != null && streamReference.ContentType.Length == 0)
			{
				throw new ODataException(Strings.WriterValidationUtils_StreamReferenceValueEmptyContentType);
			}
			if (isDefaultStream && streamReference.ReadLink == null && streamReference.ContentType != null)
			{
				throw new ODataException(Strings.WriterValidationUtils_DefaultStreamWithContentTypeWithoutReadLink);
			}
			if (isDefaultStream && streamReference.ReadLink != null && streamReference.ContentType == null)
			{
				throw new ODataException(Strings.WriterValidationUtils_DefaultStreamWithReadLinkWithoutContentType);
			}
			if (streamReference.EditLink == null && streamReference.ReadLink == null && !isDefaultStream)
			{
				throw new ODataException(Strings.WriterValidationUtils_StreamReferenceValueMustHaveEditLinkOrReadLink);
			}
			if (streamReference.EditLink == null && streamReference.ETag != null)
			{
				throw new ODataException(Strings.WriterValidationUtils_StreamReferenceValueMustHaveEditLinkToHaveETag);
			}
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x00045E8C File Offset: 0x0004408C
		internal static void ValidateStreamReferenceProperty(ODataProperty streamProperty, IEdmProperty edmProperty, ODataVersion version, bool writingResponse)
		{
			ODataVersionChecker.CheckStreamReferenceProperty(version);
			ValidationUtils.ValidateStreamReferenceProperty(streamProperty, edmProperty);
			if (!writingResponse)
			{
				throw new ODataException(Strings.WriterValidationUtils_StreamPropertyInRequest(streamProperty.Name));
			}
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x00045EAF File Offset: 0x000440AF
		internal static void ValidateEntityReferenceLinkNotNull(ODataEntityReferenceLink entityReferenceLink)
		{
			if (entityReferenceLink == null)
			{
				throw new ODataException(Strings.WriterValidationUtils_EntityReferenceLinksLinkMustNotBeNull);
			}
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x00045EBF File Offset: 0x000440BF
		internal static void ValidateEntityReferenceLink(ODataEntityReferenceLink entityReferenceLink)
		{
			if (entityReferenceLink.Url == null)
			{
				throw new ODataException(Strings.WriterValidationUtils_EntityReferenceLinkUrlMustNotBeNull);
			}
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x00045EDC File Offset: 0x000440DC
		internal static IEdmNavigationProperty ValidateNavigationLink(ODataNavigationLink navigationLink, IEdmEntityType declaringEntityType, ODataPayloadKind? expandedPayloadKind)
		{
			if (string.IsNullOrEmpty(navigationLink.Name))
			{
				throw new ODataException(Strings.ValidationUtils_LinkMustSpecifyName);
			}
			bool flag = expandedPayloadKind == ODataPayloadKind.EntityReferenceLink;
			bool flag2 = expandedPayloadKind == ODataPayloadKind.Feed;
			Func<object, string> func = null;
			if (!flag && navigationLink.IsCollection != null && expandedPayloadKind != null && flag2 != navigationLink.IsCollection.Value)
			{
				func = ((expandedPayloadKind.Value == ODataPayloadKind.Feed) ? new Func<object, string>(Strings.WriterValidationUtils_ExpandedLinkIsCollectionFalseWithFeedContent) : new Func<object, string>(Strings.WriterValidationUtils_ExpandedLinkIsCollectionTrueWithEntryContent));
			}
			IEdmNavigationProperty edmNavigationProperty = null;
			if (func == null && declaringEntityType != null)
			{
				edmNavigationProperty = WriterValidationUtils.ValidateNavigationPropertyDefined(navigationLink.Name, declaringEntityType);
				bool flag3 = edmNavigationProperty.Type.TypeKind() == EdmTypeKind.Collection;
				if (navigationLink.IsCollection != null && flag3 != navigationLink.IsCollection && (!(navigationLink.IsCollection == false) || !flag))
				{
					func = (flag3 ? new Func<object, string>(Strings.WriterValidationUtils_ExpandedLinkIsCollectionFalseWithFeedMetadata) : new Func<object, string>(Strings.WriterValidationUtils_ExpandedLinkIsCollectionTrueWithEntryMetadata));
				}
				if (!flag && expandedPayloadKind != null && flag3 != flag2)
				{
					func = (flag3 ? new Func<object, string>(Strings.WriterValidationUtils_ExpandedLinkWithEntryPayloadAndFeedMetadata) : new Func<object, string>(Strings.WriterValidationUtils_ExpandedLinkWithFeedPayloadAndEntryMetadata));
				}
			}
			if (func != null)
			{
				string arg = (navigationLink.Url == null) ? "null" : UriUtilsCommon.UriToString(navigationLink.Url);
				throw new ODataException(func(arg));
			}
			return edmNavigationProperty;
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x0004608A File Offset: 0x0004428A
		internal static void ValidateNavigationLinkUrlPresent(ODataNavigationLink navigationLink)
		{
			if (navigationLink.Url == null)
			{
				throw new ODataException(Strings.WriterValidationUtils_NavigationLinkMustSpecifyUrl(navigationLink.Name));
			}
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x000460AC File Offset: 0x000442AC
		internal static void ValidateNavigationLinkHasCardinality(ODataNavigationLink navigationLink)
		{
			if (navigationLink.IsCollection == null)
			{
				throw new ODataException(Strings.WriterValidationUtils_NavigationLinkMustSpecifyIsCollection(navigationLink.Name));
			}
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x000460DC File Offset: 0x000442DC
		internal static void ValidateNullPropertyValue(IEdmTypeReference expectedPropertyTypeReference, string propertyName, ODataWriterBehavior writerBehavior, IEdmModel model)
		{
			if (expectedPropertyTypeReference != null)
			{
				if (expectedPropertyTypeReference.IsNonEntityCollectionType())
				{
					throw new ODataException(Strings.WriterValidationUtils_CollectionPropertiesMustNotHaveNullValue(propertyName));
				}
				if (expectedPropertyTypeReference.IsODataPrimitiveTypeKind())
				{
					if (!expectedPropertyTypeReference.IsNullable && !writerBehavior.AllowNullValuesForNonNullablePrimitiveTypes)
					{
						throw new ODataException(Strings.WriterValidationUtils_NonNullablePropertiesMustNotHaveNullValue(propertyName, expectedPropertyTypeReference.ODataFullName()));
					}
				}
				else
				{
					if (expectedPropertyTypeReference.IsStream())
					{
						throw new ODataException(Strings.WriterValidationUtils_StreamPropertiesMustNotHaveNullValue(propertyName));
					}
					if (expectedPropertyTypeReference.IsODataComplexTypeKind() && ValidationUtils.ShouldValidateComplexPropertyNullValue(model))
					{
						IEdmComplexTypeReference edmComplexTypeReference = expectedPropertyTypeReference.AsComplex();
						if (!edmComplexTypeReference.IsNullable)
						{
							throw new ODataException(Strings.WriterValidationUtils_NonNullablePropertiesMustNotHaveNullValue(propertyName, expectedPropertyTypeReference.ODataFullName()));
						}
					}
				}
			}
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x00046172 File Offset: 0x00044372
		private static void ValidateEntryId(string id)
		{
			if (id != null && id.Length == 0)
			{
				throw new ODataException(Strings.WriterValidationUtils_EntriesMustHaveNonEmptyId);
			}
		}
	}
}
