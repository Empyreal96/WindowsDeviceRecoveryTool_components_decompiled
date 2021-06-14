using System;
using System.Text;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x02000168 RID: 360
	internal abstract class ODataJsonLightMetadataUriBuilder
	{
		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000A09 RID: 2569
		internal abstract Uri BaseUri { get; }

		// Token: 0x06000A0A RID: 2570 RVA: 0x00020C2C File Offset: 0x0001EE2C
		internal static ODataJsonLightMetadataUriBuilder CreateFromSettings(JsonLightMetadataLevel metadataLevel, bool writingResponse, ODataMessageWriterSettings writerSettings, IEdmModel model)
		{
			if (!metadataLevel.ShouldWriteODataMetadataUri())
			{
				return ODataJsonLightMetadataUriBuilder.NullMetadataUriBuilder.Instance;
			}
			ODataMetadataDocumentUri metadataDocumentUri = writerSettings.MetadataDocumentUri;
			if (metadataDocumentUri != null)
			{
				return ODataJsonLightMetadataUriBuilder.CreateDirectlyFromUri(metadataDocumentUri, model, writingResponse);
			}
			if (writingResponse)
			{
				throw new ODataException(Strings.ODataJsonLightOutputContext_MetadataDocumentUriMissing);
			}
			return ODataJsonLightMetadataUriBuilder.NullMetadataUriBuilder.Instance;
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x00020C6D File Offset: 0x0001EE6D
		internal static ODataJsonLightMetadataUriBuilder CreateDirectlyFromUri(ODataMetadataDocumentUri metadataDocumentUri, IEdmModel model, bool writingResponse)
		{
			return new ODataJsonLightMetadataUriBuilder.DefaultMetadataUriBuilder(metadataDocumentUri, model, writingResponse);
		}

		// Token: 0x06000A0C RID: 2572
		internal abstract bool TryBuildFeedMetadataUri(ODataFeedAndEntryTypeContext typeContext, out Uri metadataUri);

		// Token: 0x06000A0D RID: 2573
		internal abstract bool TryBuildEntryMetadataUri(ODataFeedAndEntryTypeContext typeContext, out Uri metadataUri);

		// Token: 0x06000A0E RID: 2574
		internal abstract bool TryBuildMetadataUriForValue(ODataProperty property, out Uri metadataUri);

		// Token: 0x06000A0F RID: 2575
		internal abstract bool TryBuildEntityReferenceLinkMetadataUri(ODataEntityReferenceLinkSerializationInfo serializationInfo, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty, out Uri metadataUri);

		// Token: 0x06000A10 RID: 2576
		internal abstract bool TryBuildEntityReferenceLinksMetadataUri(ODataEntityReferenceLinksSerializationInfo serializationInfo, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty, out Uri metadataUri);

		// Token: 0x06000A11 RID: 2577
		internal abstract bool TryBuildCollectionMetadataUri(ODataCollectionStartSerializationInfo serializationInfo, IEdmTypeReference itemTypeReference, out Uri metadataUri);

		// Token: 0x06000A12 RID: 2578
		internal abstract bool TryBuildServiceDocumentMetadataUri(out Uri metadataUri);

		// Token: 0x02000169 RID: 361
		private sealed class DefaultMetadataUriBuilder : ODataJsonLightMetadataUriBuilder
		{
			// Token: 0x06000A14 RID: 2580 RVA: 0x00020C7F File Offset: 0x0001EE7F
			internal DefaultMetadataUriBuilder(ODataMetadataDocumentUri metadataDocumentUri, IEdmModel model, bool writingResponse)
			{
				this.metadataDocumentUri = metadataDocumentUri;
				this.model = model;
				this.writingResponse = writingResponse;
			}

			// Token: 0x17000260 RID: 608
			// (get) Token: 0x06000A15 RID: 2581 RVA: 0x00020C9C File Offset: 0x0001EE9C
			internal override Uri BaseUri
			{
				get
				{
					return this.metadataDocumentUri.BaseUri;
				}
			}

			// Token: 0x06000A16 RID: 2582 RVA: 0x00020CA9 File Offset: 0x0001EEA9
			internal override bool TryBuildFeedMetadataUri(ODataFeedAndEntryTypeContext typeContext, out Uri metadataUri)
			{
				metadataUri = ODataJsonLightMetadataUriBuilder.DefaultMetadataUriBuilder.CreateFeedOrEntryMetadataUri(this.metadataDocumentUri, this.model, typeContext, false, this.writingResponse);
				return metadataUri != null;
			}

			// Token: 0x06000A17 RID: 2583 RVA: 0x00020CCE File Offset: 0x0001EECE
			internal override bool TryBuildEntryMetadataUri(ODataFeedAndEntryTypeContext typeContext, out Uri metadataUri)
			{
				metadataUri = ODataJsonLightMetadataUriBuilder.DefaultMetadataUriBuilder.CreateFeedOrEntryMetadataUri(this.metadataDocumentUri, this.model, typeContext, true, this.writingResponse);
				return metadataUri != null;
			}

			// Token: 0x06000A18 RID: 2584 RVA: 0x00020CF4 File Offset: 0x0001EEF4
			internal override bool TryBuildMetadataUriForValue(ODataProperty property, out Uri metadataUri)
			{
				string metadataUriTypeNameForValue = ODataJsonLightMetadataUriBuilder.DefaultMetadataUriBuilder.GetMetadataUriTypeNameForValue(property);
				if (string.IsNullOrEmpty(metadataUriTypeNameForValue))
				{
					throw new ODataException(Strings.WriterValidationUtils_MissingTypeNameWithMetadata);
				}
				metadataUri = ODataJsonLightMetadataUriBuilder.DefaultMetadataUriBuilder.CreateTypeMetadataUri(this.metadataDocumentUri, metadataUriTypeNameForValue);
				return metadataUri != null;
			}

			// Token: 0x06000A19 RID: 2585 RVA: 0x00020D34 File Offset: 0x0001EF34
			internal override bool TryBuildEntityReferenceLinkMetadataUri(ODataEntityReferenceLinkSerializationInfo serializationInfo, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty, out Uri metadataUri)
			{
				string entitySetName = null;
				string typecast = null;
				string text = null;
				bool appendItemSelector = false;
				if (serializationInfo != null)
				{
					entitySetName = serializationInfo.SourceEntitySetName;
					typecast = serializationInfo.Typecast;
					text = serializationInfo.NavigationPropertyName;
					appendItemSelector = serializationInfo.IsCollectionNavigationProperty;
				}
				else if (navigationProperty != null)
				{
					entitySetName = ODataJsonLightMetadataUriBuilder.DefaultMetadataUriBuilder.GetEntitySetName(entitySet, this.model);
					typecast = ODataJsonLightMetadataUriBuilder.DefaultMetadataUriBuilder.GetTypecast(entitySet, navigationProperty.DeclaringEntityType());
					text = navigationProperty.Name;
					appendItemSelector = navigationProperty.Type.IsCollection();
				}
				metadataUri = ((text == null) ? null : ODataJsonLightMetadataUriBuilder.DefaultMetadataUriBuilder.CreateEntityContainerElementMetadataUri(this.metadataDocumentUri, entitySetName, typecast, text, appendItemSelector));
				if (this.writingResponse && metadataUri == null)
				{
					throw new ODataException(Strings.ODataJsonLightMetadataUriBuilder_EntitySetOrNavigationPropertyMissingForTopLevelEntityReferenceLinkResponse);
				}
				return metadataUri != null;
			}

			// Token: 0x06000A1A RID: 2586 RVA: 0x00020DDC File Offset: 0x0001EFDC
			internal override bool TryBuildEntityReferenceLinksMetadataUri(ODataEntityReferenceLinksSerializationInfo serializationInfo, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty, out Uri metadataUri)
			{
				string entitySetName = null;
				string typecast = null;
				string text = null;
				if (serializationInfo != null)
				{
					entitySetName = serializationInfo.SourceEntitySetName;
					typecast = serializationInfo.Typecast;
					text = serializationInfo.NavigationPropertyName;
				}
				else if (navigationProperty != null)
				{
					entitySetName = ODataJsonLightMetadataUriBuilder.DefaultMetadataUriBuilder.GetEntitySetName(entitySet, this.model);
					typecast = ODataJsonLightMetadataUriBuilder.DefaultMetadataUriBuilder.GetTypecast(entitySet, navigationProperty.DeclaringEntityType());
					text = navigationProperty.Name;
				}
				metadataUri = ((text == null) ? null : ODataJsonLightMetadataUriBuilder.DefaultMetadataUriBuilder.CreateEntityContainerElementMetadataUri(this.metadataDocumentUri, entitySetName, typecast, text, false));
				if (this.writingResponse && metadataUri == null)
				{
					throw new ODataException(Strings.ODataJsonLightMetadataUriBuilder_EntitySetOrNavigationPropertyMissingForTopLevelEntityReferenceLinksResponse);
				}
				return metadataUri != null;
			}

			// Token: 0x06000A1B RID: 2587 RVA: 0x00020E6C File Offset: 0x0001F06C
			internal override bool TryBuildCollectionMetadataUri(ODataCollectionStartSerializationInfo serializationInfo, IEdmTypeReference itemTypeReference, out Uri metadataUri)
			{
				string fullTypeName = null;
				if (serializationInfo != null)
				{
					fullTypeName = serializationInfo.CollectionTypeName;
				}
				else if (itemTypeReference != null)
				{
					fullTypeName = EdmLibraryExtensions.GetCollectionTypeName(itemTypeReference.ODataFullName());
				}
				metadataUri = ODataJsonLightMetadataUriBuilder.DefaultMetadataUriBuilder.CreateTypeMetadataUri(this.metadataDocumentUri, fullTypeName);
				if (this.writingResponse && metadataUri == null)
				{
					throw new ODataException(Strings.ODataJsonLightMetadataUriBuilder_TypeNameMissingForTopLevelCollectionWhenWritingResponsePayload);
				}
				return metadataUri != null;
			}

			// Token: 0x06000A1C RID: 2588 RVA: 0x00020EC9 File Offset: 0x0001F0C9
			internal override bool TryBuildServiceDocumentMetadataUri(out Uri metadataUri)
			{
				metadataUri = this.metadataDocumentUri.BaseUri;
				return true;
			}

			// Token: 0x06000A1D RID: 2589 RVA: 0x00020EDC File Offset: 0x0001F0DC
			private static string GetMetadataUriTypeNameForValue(ODataProperty property)
			{
				ODataValue odataValue = property.ODataValue;
				if (odataValue.IsNullValue)
				{
					return "Edm.Null";
				}
				SerializationTypeNameAnnotation annotation = odataValue.GetAnnotation<SerializationTypeNameAnnotation>();
				if (annotation != null && !string.IsNullOrEmpty(annotation.TypeName))
				{
					return annotation.TypeName;
				}
				ODataComplexValue odataComplexValue = odataValue as ODataComplexValue;
				if (odataComplexValue != null)
				{
					return odataComplexValue.TypeName;
				}
				ODataCollectionValue odataCollectionValue = odataValue as ODataCollectionValue;
				if (odataCollectionValue != null)
				{
					return odataCollectionValue.TypeName;
				}
				ODataPrimitiveValue odataPrimitiveValue = odataValue as ODataPrimitiveValue;
				if (odataPrimitiveValue == null)
				{
					throw new ODataException(Strings.ODataWriter_StreamPropertiesMustBePropertiesOfODataEntry(property.Name));
				}
				return EdmLibraryExtensions.GetPrimitiveTypeReference(odataPrimitiveValue.Value.GetType()).ODataFullName();
			}

			// Token: 0x06000A1E RID: 2590 RVA: 0x00020F74 File Offset: 0x0001F174
			private static string GetEntitySetName(IEdmEntitySet entitySet, IEdmModel edmModel)
			{
				if (entitySet == null)
				{
					return null;
				}
				IEdmEntityContainer container = entitySet.Container;
				string result;
				if (edmModel.IsDefaultEntityContainer(container))
				{
					result = entitySet.Name;
				}
				else
				{
					result = string.Concat(new string[]
					{
						container.Namespace,
						".",
						container.Name,
						".",
						entitySet.Name
					});
				}
				return result;
			}

			// Token: 0x06000A1F RID: 2591 RVA: 0x00020FDC File Offset: 0x0001F1DC
			private static string GetTypecast(IEdmEntitySet entitySet, IEdmEntityType entityType)
			{
				if (entitySet == null || entityType == null)
				{
					return null;
				}
				IEdmEntityType elementType = EdmTypeWriterResolver.Instance.GetElementType(entitySet);
				if (elementType.IsEquivalentTo(entityType))
				{
					return null;
				}
				if (!elementType.IsAssignableFrom(entityType))
				{
					throw new ODataException(Strings.ODataJsonLightMetadataUriBuilder_ValidateDerivedType(elementType.FullName(), entityType.FullName()));
				}
				return entityType.ODataFullName();
			}

			// Token: 0x06000A20 RID: 2592 RVA: 0x0002102E File Offset: 0x0001F22E
			private static Uri CreateTypeMetadataUri(ODataMetadataDocumentUri metadataDocumentUri, string fullTypeName)
			{
				if (fullTypeName != null)
				{
					return new Uri(metadataDocumentUri.BaseUri, '#' + fullTypeName);
				}
				return null;
			}

			// Token: 0x06000A21 RID: 2593 RVA: 0x00021050 File Offset: 0x0001F250
			private static Uri CreateFeedOrEntryMetadataUri(ODataMetadataDocumentUri metadataDocumentUri, IEdmModel model, ODataFeedAndEntryTypeContext typeContext, bool isEntry, bool writingResponse)
			{
				string typecast = (typeContext.EntitySetElementTypeName == typeContext.ExpectedEntityTypeName) ? null : typeContext.ExpectedEntityTypeName;
				return ODataJsonLightMetadataUriBuilder.DefaultMetadataUriBuilder.CreateEntityContainerElementMetadataUri(metadataDocumentUri, typeContext.EntitySetName, typecast, null, isEntry);
			}

			// Token: 0x06000A22 RID: 2594 RVA: 0x0002108C File Offset: 0x0001F28C
			private static Uri CreateEntityContainerElementMetadataUri(ODataMetadataDocumentUri metadataDocumentUri, string entitySetName, string typecast, string navigationPropertyName, bool appendItemSelector)
			{
				if (entitySetName == null)
				{
					return null;
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append('#');
				stringBuilder.Append(entitySetName);
				if (typecast != null)
				{
					stringBuilder.Append('/');
					stringBuilder.Append(typecast);
				}
				if (navigationPropertyName != null)
				{
					stringBuilder.Append('/');
					stringBuilder.Append("$links");
					stringBuilder.Append('/');
					stringBuilder.Append(navigationPropertyName);
				}
				if (appendItemSelector)
				{
					stringBuilder.Append('/');
					stringBuilder.Append("@Element");
				}
				string selectClause = metadataDocumentUri.SelectClause;
				if (selectClause != null)
				{
					stringBuilder.Append('&');
					stringBuilder.Append("$select");
					stringBuilder.Append('=');
					stringBuilder.Append(selectClause);
				}
				return new Uri(metadataDocumentUri.BaseUri, stringBuilder.ToString());
			}

			// Token: 0x040003B8 RID: 952
			private readonly ODataMetadataDocumentUri metadataDocumentUri;

			// Token: 0x040003B9 RID: 953
			private readonly IEdmModel model;

			// Token: 0x040003BA RID: 954
			private readonly bool writingResponse;
		}

		// Token: 0x0200016A RID: 362
		private sealed class NullMetadataUriBuilder : ODataJsonLightMetadataUriBuilder
		{
			// Token: 0x06000A23 RID: 2595 RVA: 0x0002114C File Offset: 0x0001F34C
			private NullMetadataUriBuilder()
			{
			}

			// Token: 0x17000261 RID: 609
			// (get) Token: 0x06000A24 RID: 2596 RVA: 0x00021154 File Offset: 0x0001F354
			internal override Uri BaseUri
			{
				get
				{
					return null;
				}
			}

			// Token: 0x06000A25 RID: 2597 RVA: 0x00021157 File Offset: 0x0001F357
			internal override bool TryBuildFeedMetadataUri(ODataFeedAndEntryTypeContext typeContext, out Uri metadataUri)
			{
				metadataUri = null;
				return false;
			}

			// Token: 0x06000A26 RID: 2598 RVA: 0x0002115D File Offset: 0x0001F35D
			internal override bool TryBuildEntryMetadataUri(ODataFeedAndEntryTypeContext typeContext, out Uri metadataUri)
			{
				metadataUri = null;
				return false;
			}

			// Token: 0x06000A27 RID: 2599 RVA: 0x00021163 File Offset: 0x0001F363
			internal override bool TryBuildMetadataUriForValue(ODataProperty property, out Uri metadataUri)
			{
				metadataUri = null;
				return false;
			}

			// Token: 0x06000A28 RID: 2600 RVA: 0x00021169 File Offset: 0x0001F369
			internal override bool TryBuildEntityReferenceLinkMetadataUri(ODataEntityReferenceLinkSerializationInfo serializationInfo, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty, out Uri metadataUri)
			{
				metadataUri = null;
				return false;
			}

			// Token: 0x06000A29 RID: 2601 RVA: 0x00021170 File Offset: 0x0001F370
			internal override bool TryBuildEntityReferenceLinksMetadataUri(ODataEntityReferenceLinksSerializationInfo serializationInfo, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty, out Uri metadataUri)
			{
				metadataUri = null;
				return false;
			}

			// Token: 0x06000A2A RID: 2602 RVA: 0x00021177 File Offset: 0x0001F377
			internal override bool TryBuildCollectionMetadataUri(ODataCollectionStartSerializationInfo serializationInfo, IEdmTypeReference itemTypeReference, out Uri metadataUri)
			{
				metadataUri = null;
				return false;
			}

			// Token: 0x06000A2B RID: 2603 RVA: 0x0002117D File Offset: 0x0001F37D
			internal override bool TryBuildServiceDocumentMetadataUri(out Uri metadataUri)
			{
				metadataUri = null;
				return false;
			}

			// Token: 0x040003BB RID: 955
			internal static readonly ODataJsonLightMetadataUriBuilder.NullMetadataUriBuilder Instance = new ODataJsonLightMetadataUriBuilder.NullMetadataUriBuilder();
		}
	}
}
