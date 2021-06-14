using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x02000165 RID: 357
	internal sealed class ODataJsonLightMetadataUriParser
	{
		// Token: 0x060009E8 RID: 2536 RVA: 0x000201EE File Offset: 0x0001E3EE
		private ODataJsonLightMetadataUriParser(IEdmModel model, Uri metadataUriFromPayload)
		{
			if (!model.IsUserModel())
			{
				throw new ODataException(Strings.ODataJsonLightMetadataUriParser_NoModel);
			}
			this.model = model;
			this.parseResult = new ODataJsonLightMetadataUriParseResult(metadataUriFromPayload);
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x0002021C File Offset: 0x0001E41C
		internal static ODataJsonLightMetadataUriParseResult Parse(IEdmModel model, string metadataUriFromPayload, ODataPayloadKind payloadKind, ODataVersion version, ODataReaderBehavior readerBehavior)
		{
			if (metadataUriFromPayload == null)
			{
				throw new ODataException(Strings.ODataJsonLightMetadataUriParser_NullMetadataDocumentUri);
			}
			Uri metadataUriFromPayload2 = new Uri(metadataUriFromPayload, UriKind.Absolute);
			ODataJsonLightMetadataUriParser odataJsonLightMetadataUriParser = new ODataJsonLightMetadataUriParser(model, metadataUriFromPayload2);
			odataJsonLightMetadataUriParser.TokenizeMetadataUri();
			odataJsonLightMetadataUriParser.ParseMetadataUri(payloadKind, readerBehavior, version);
			return odataJsonLightMetadataUriParser.parseResult;
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x00020260 File Offset: 0x0001E460
		private static string ExtractSelectQueryOption(string fragment)
		{
			int num = fragment.IndexOf(ODataJsonLightMetadataUriParser.SelectQueryOptionStart, StringComparison.Ordinal);
			if (num < 0)
			{
				return null;
			}
			int num2 = num + ODataJsonLightMetadataUriParser.SelectQueryOptionStart.Length;
			int num3 = fragment.IndexOf('&', num2);
			string text;
			if (num3 < 0)
			{
				text = fragment.Substring(num2);
			}
			else
			{
				text = fragment.Substring(num2, num3 - num2);
			}
			return text.Trim();
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x000202B8 File Offset: 0x0001E4B8
		private void TokenizeMetadataUri()
		{
			Uri metadataUri = this.parseResult.MetadataUri;
			UriBuilder uriBuilder = new UriBuilder(metadataUri)
			{
				Fragment = null
			};
			this.parseResult.MetadataDocumentUri = uriBuilder.Uri;
			this.parseResult.Fragment = metadataUri.GetComponents(UriComponents.Fragment, UriFormat.Unescaped);
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x00020308 File Offset: 0x0001E508
		private void ParseMetadataUri(ODataPayloadKind expectedPayloadKind, ODataReaderBehavior readerBehavior, ODataVersion version)
		{
			ODataPayloadKind odataPayloadKind = this.ParseMetadataUriFragment(this.parseResult.Fragment, readerBehavior, version);
			bool flag = odataPayloadKind == expectedPayloadKind || expectedPayloadKind == ODataPayloadKind.Unsupported;
			if (odataPayloadKind == ODataPayloadKind.Collection)
			{
				this.parseResult.DetectedPayloadKinds = new ODataPayloadKind[]
				{
					ODataPayloadKind.Collection,
					ODataPayloadKind.Property
				};
				if (expectedPayloadKind == ODataPayloadKind.Property)
				{
					flag = true;
				}
			}
			else
			{
				this.parseResult.DetectedPayloadKinds = new ODataPayloadKind[]
				{
					odataPayloadKind
				};
			}
			if (!flag)
			{
				throw new ODataException(Strings.ODataJsonLightMetadataUriParser_MetadataUriDoesNotMatchExpectedPayloadKind(UriUtilsCommon.UriToString(this.parseResult.MetadataUri), expectedPayloadKind.ToString()));
			}
			string selectQueryOption = this.parseResult.SelectQueryOption;
			if (selectQueryOption != null && odataPayloadKind != ODataPayloadKind.Feed && odataPayloadKind != ODataPayloadKind.Entry)
			{
				throw new ODataException(Strings.ODataJsonLightMetadataUriParser_InvalidPayloadKindWithSelectQueryOption(expectedPayloadKind.ToString()));
			}
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x000204AC File Offset: 0x0001E6AC
		private ODataPayloadKind ParseMetadataUriFragment(string fragment, ODataReaderBehavior readerBehavior, ODataVersion version)
		{
			int num = fragment.IndexOf('&');
			if (num > 0)
			{
				string fragment2 = fragment.Substring(num);
				this.parseResult.SelectQueryOption = ODataJsonLightMetadataUriParser.ExtractSelectQueryOption(fragment2);
				fragment = fragment.Substring(0, num);
			}
			string[] parts = fragment.Split(new char[]
			{
				'/'
			});
			int num2 = parts.Length;
			EdmTypeResolver edmTypeResolver = new EdmTypeReaderResolver(this.model, readerBehavior, version);
			int num3 = fragment.IndexOf("$links", StringComparison.Ordinal);
			ODataPayloadKind result;
			if (num3 > -1)
			{
				result = this.ParseAssociationLinks(edmTypeResolver, num2, parts, readerBehavior, version);
			}
			else
			{
				switch (num2)
				{
				case 1:
					if (fragment.Length == 0)
					{
						result = ODataPayloadKind.ServiceDocument;
					}
					else if (parts[0].Equals("Edm.Null", StringComparison.OrdinalIgnoreCase))
					{
						result = ODataPayloadKind.Property;
						this.parseResult.IsNullProperty = true;
					}
					else
					{
						IEdmEntitySet edmEntitySet = this.model.ResolveEntitySet(parts[0]);
						if (edmEntitySet != null)
						{
							this.parseResult.EntitySet = edmEntitySet;
							this.parseResult.EdmType = edmTypeResolver.GetElementType(edmEntitySet);
							result = ODataPayloadKind.Feed;
						}
						else
						{
							this.parseResult.EdmType = this.ResolveType(parts[0], readerBehavior, version);
							result = ((this.parseResult.EdmType is IEdmCollectionType) ? ODataPayloadKind.Collection : ODataPayloadKind.Property);
						}
					}
					break;
				case 2:
					result = this.ResolveEntitySet(parts[0], delegate(IEdmEntitySet resolvedEntitySet)
					{
						IEdmEntityType elementType = edmTypeResolver.GetElementType(resolvedEntitySet);
						if (string.CompareOrdinal("@Element", parts[1]) == 0)
						{
							this.parseResult.EdmType = elementType;
							return ODataPayloadKind.Entry;
						}
						this.parseResult.EdmType = this.ResolveTypeCast(resolvedEntitySet, parts[1], readerBehavior, version, elementType);
						return ODataPayloadKind.Feed;
					});
					break;
				case 3:
					result = this.ResolveEntitySet(parts[0], delegate(IEdmEntitySet resolvedEntitySet)
					{
						IEdmEntityType elementType = edmTypeResolver.GetElementType(resolvedEntitySet);
						this.parseResult.EdmType = this.ResolveTypeCast(resolvedEntitySet, parts[1], readerBehavior, version, elementType);
						this.ValidateMetadataUriFragmentItemSelector(parts[2]);
						return ODataPayloadKind.Entry;
					});
					break;
				default:
					throw new ODataException(Strings.ODataJsonLightMetadataUriParser_FragmentWithInvalidNumberOfParts(UriUtilsCommon.UriToString(this.parseResult.MetadataUri), num2, 3));
				}
			}
			return result;
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x00020924 File Offset: 0x0001EB24
		private ODataPayloadKind ParseAssociationLinks(EdmTypeResolver edmTypeResolver, int partCount, string[] parts, ODataReaderBehavior readerBehavior, ODataVersion version)
		{
			return this.ResolveEntitySet(parts[0], delegate(IEdmEntitySet resolvedEntitySet)
			{
				ODataPayloadKind result;
				switch (partCount)
				{
				case 3:
				{
					if (string.CompareOrdinal("$links", parts[1]) != 0)
					{
						throw new ODataException(Strings.ODataJsonLightMetadataUriParser_InvalidAssociationLink(UriUtilsCommon.UriToString(this.parseResult.MetadataUri)));
					}
					IEdmNavigationProperty navigationProperty = this.ResolveEntityReferenceLinkMetadataFragment(edmTypeResolver, resolvedEntitySet, null, parts[2], readerBehavior, version);
					result = this.SetEntityLinkParseResults(navigationProperty, null);
					break;
				}
				case 4:
					if (string.CompareOrdinal("$links", parts[1]) == 0)
					{
						IEdmNavigationProperty navigationProperty = this.ResolveEntityReferenceLinkMetadataFragment(edmTypeResolver, resolvedEntitySet, null, parts[2], readerBehavior, version);
						this.ValidateLinkMetadataUriFragmentItemSelector(parts[3]);
						result = this.SetEntityLinkParseResults(navigationProperty, parts[3]);
					}
					else
					{
						if (string.CompareOrdinal("$links", parts[2]) != 0)
						{
							throw new ODataException(Strings.ODataJsonLightMetadataUriParser_InvalidAssociationLink(UriUtilsCommon.UriToString(this.parseResult.MetadataUri)));
						}
						IEdmNavigationProperty navigationProperty = this.ResolveEntityReferenceLinkMetadataFragment(edmTypeResolver, resolvedEntitySet, parts[1], parts[3], readerBehavior, version);
						result = this.SetEntityLinkParseResults(navigationProperty, null);
					}
					break;
				case 5:
				{
					if (string.CompareOrdinal("$links", parts[2]) != 0)
					{
						throw new ODataException(Strings.ODataJsonLightMetadataUriParser_InvalidAssociationLink(UriUtilsCommon.UriToString(this.parseResult.MetadataUri)));
					}
					IEdmNavigationProperty navigationProperty = this.ResolveEntityReferenceLinkMetadataFragment(edmTypeResolver, resolvedEntitySet, parts[1], parts[3], readerBehavior, version);
					this.ValidateLinkMetadataUriFragmentItemSelector(parts[2]);
					result = this.SetEntityLinkParseResults(navigationProperty, parts[4]);
					break;
				}
				default:
					throw new ODataException(Strings.ODataJsonLightMetadataUriParser_InvalidAssociationLink(UriUtilsCommon.UriToString(this.parseResult.MetadataUri)));
				}
				return result;
			});
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x00020980 File Offset: 0x0001EB80
		private ODataPayloadKind SetEntityLinkParseResults(IEdmNavigationProperty navigationProperty, string singleElement)
		{
			this.parseResult.NavigationProperty = navigationProperty;
			ODataPayloadKind result = navigationProperty.Type.IsCollection() ? ODataPayloadKind.EntityReferenceLinks : ODataPayloadKind.EntityReferenceLink;
			if (singleElement != null && string.CompareOrdinal("@Element", singleElement) == 0)
			{
				if (!navigationProperty.Type.IsCollection())
				{
					throw new ODataException(Strings.ODataJsonLightMetadataUriParser_InvalidSingletonNavPropertyForEntityReferenceLinkUri(UriUtilsCommon.UriToString(this.parseResult.MetadataUri), navigationProperty.Name, singleElement));
				}
				result = ODataPayloadKind.EntityReferenceLink;
			}
			return result;
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x000209F8 File Offset: 0x0001EBF8
		private IEdmNavigationProperty ResolveEntityReferenceLinkMetadataFragment(EdmTypeResolver edmTypeResolver, IEdmEntitySet entitySet, string typeName, string propertyName, ODataReaderBehavior readerBehavior, ODataVersion version)
		{
			IEdmEntityType edmEntityType = edmTypeResolver.GetElementType(entitySet);
			if (typeName != null)
			{
				edmEntityType = this.ResolveTypeCast(entitySet, typeName, readerBehavior, version, edmEntityType);
			}
			return this.ResolveNavigationProperty(edmEntityType, propertyName);
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x00020A29 File Offset: 0x0001EC29
		private void ValidateLinkMetadataUriFragmentItemSelector(string elementSelector)
		{
			if (string.CompareOrdinal("@Element", elementSelector) != 0)
			{
				throw new ODataException(Strings.ODataJsonLightMetadataUriParser_InvalidEntityReferenceLinkUriSuffix(UriUtilsCommon.UriToString(this.parseResult.MetadataUri), elementSelector, "@Element"));
			}
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x00020A59 File Offset: 0x0001EC59
		private void ValidateMetadataUriFragmentItemSelector(string elementSelector)
		{
			if (string.CompareOrdinal("@Element", elementSelector) != 0)
			{
				throw new ODataException(Strings.ODataJsonLightMetadataUriParser_InvalidEntityWithTypeCastUriSuffix(UriUtilsCommon.UriToString(this.parseResult.MetadataUri), elementSelector, "@Element"));
			}
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x00020A8C File Offset: 0x0001EC8C
		private IEdmNavigationProperty ResolveNavigationProperty(IEdmEntityType entityType, string navigationPropertyName)
		{
			IEdmProperty edmProperty = entityType.FindProperty(navigationPropertyName);
			IEdmNavigationProperty edmNavigationProperty = edmProperty as IEdmNavigationProperty;
			if (edmNavigationProperty == null)
			{
				throw new ODataException(Strings.ODataJsonLightMetadataUriParser_InvalidPropertyForEntityReferenceLinkUri(UriUtilsCommon.UriToString(this.parseResult.MetadataUri), navigationPropertyName));
			}
			return edmNavigationProperty;
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x00020ACC File Offset: 0x0001ECCC
		private ODataPayloadKind ResolveEntitySet(string entitySetPart, Func<IEdmEntitySet, ODataPayloadKind> resolvedEntitySet)
		{
			IEdmEntitySet edmEntitySet = this.model.ResolveEntitySet(entitySetPart);
			if (edmEntitySet != null)
			{
				this.parseResult.EntitySet = edmEntitySet;
				return resolvedEntitySet(edmEntitySet);
			}
			throw new ODataException(Strings.ODataJsonLightMetadataUriParser_InvalidEntitySetName(UriUtilsCommon.UriToString(this.parseResult.MetadataUri), entitySetPart));
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x00020B18 File Offset: 0x0001ED18
		private IEdmEntityType ResolveTypeCast(IEdmEntitySet entitySet, string typeCast, ODataReaderBehavior readerBehavior, ODataVersion version, IEdmEntityType entitySetElementType)
		{
			IEdmEntityType edmEntityType = entitySetElementType;
			if (!string.IsNullOrEmpty(typeCast))
			{
				EdmTypeKind edmTypeKind;
				edmEntityType = (MetadataUtils.ResolveTypeNameForRead(this.model, null, typeCast, readerBehavior, version, out edmTypeKind) as IEdmEntityType);
				if (edmEntityType == null)
				{
					throw new ODataException(Strings.ODataJsonLightMetadataUriParser_InvalidEntityTypeInTypeCast(UriUtilsCommon.UriToString(this.parseResult.MetadataUri), typeCast));
				}
				if (!entitySetElementType.IsAssignableFrom(edmEntityType))
				{
					throw new ODataException(Strings.ODataJsonLightMetadataUriParser_IncompatibleEntityTypeInTypeCast(UriUtilsCommon.UriToString(this.parseResult.MetadataUri), typeCast, entitySetElementType.FullName(), entitySet.FullName()));
				}
			}
			return edmEntityType;
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x00020B9C File Offset: 0x0001ED9C
		private IEdmType ResolveType(string typeName, ODataReaderBehavior readerBehavior, ODataVersion version)
		{
			string text = EdmLibraryExtensions.GetCollectionItemTypeName(typeName) ?? typeName;
			EdmTypeKind edmTypeKind;
			IEdmType edmType = MetadataUtils.ResolveTypeNameForRead(this.model, null, text, readerBehavior, version, out edmTypeKind);
			if (edmType == null || (edmType.TypeKind != EdmTypeKind.Primitive && edmType.TypeKind != EdmTypeKind.Complex))
			{
				throw new ODataException(Strings.ODataJsonLightMetadataUriParser_InvalidEntitySetNameOrTypeName(UriUtilsCommon.UriToString(this.parseResult.MetadataUri), typeName));
			}
			return (text == typeName) ? edmType : EdmLibraryExtensions.GetCollectionType(edmType.ToTypeReference(true));
		}

		// Token: 0x040003A3 RID: 931
		private static readonly string SelectQueryOptionStart = "$select" + '=';

		// Token: 0x040003A4 RID: 932
		private readonly IEdmModel model;

		// Token: 0x040003A5 RID: 933
		private readonly ODataJsonLightMetadataUriParseResult parseResult;
	}
}
