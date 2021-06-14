using System;
using System.Collections.Generic;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData.Json;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.VerboseJson
{
	// Token: 0x02000232 RID: 562
	internal sealed class ODataVerboseJsonEntryAndFeedDeserializer : ODataVerboseJsonPropertyAndValueDeserializer
	{
		// Token: 0x060011DA RID: 4570 RVA: 0x000424F8 File Offset: 0x000406F8
		internal ODataVerboseJsonEntryAndFeedDeserializer(ODataVerboseJsonInputContext verboseJsonInputContext) : base(verboseJsonInputContext)
		{
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x00042504 File Offset: 0x00040704
		internal void ReadFeedStart(ODataFeed feed, bool isResultsWrapperExpected, bool isExpandedLinkContent)
		{
			if (isResultsWrapperExpected)
			{
				base.JsonReader.ReadNext();
				while (base.JsonReader.NodeType == JsonNodeType.Property)
				{
					string text = base.JsonReader.ReadPropertyName();
					if (string.CompareOrdinal("results", text) == 0)
					{
						goto IL_4C;
					}
					this.ReadFeedProperty(feed, text, isExpandedLinkContent);
				}
				throw new ODataException(Strings.ODataJsonEntryAndFeedDeserializer_ExpectedFeedResultsPropertyNotFound);
			}
			IL_4C:
			if (base.JsonReader.NodeType != JsonNodeType.StartArray)
			{
				throw new ODataException(Strings.ODataJsonEntryAndFeedDeserializer_CannotReadFeedContentStart(base.JsonReader.NodeType));
			}
			base.JsonReader.ReadStartArray();
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x00042594 File Offset: 0x00040794
		internal void ReadFeedEnd(ODataFeed feed, bool readResultsWrapper, bool isExpandedLinkContent)
		{
			if (readResultsWrapper)
			{
				base.JsonReader.ReadEndArray();
				while (base.JsonReader.NodeType == JsonNodeType.Property)
				{
					string propertyName = base.JsonReader.ReadPropertyName();
					this.ReadFeedProperty(feed, propertyName, isExpandedLinkContent);
				}
			}
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x000425D4 File Offset: 0x000407D4
		internal void ReadEntryStart()
		{
			if (base.JsonReader.NodeType != JsonNodeType.StartObject)
			{
				throw new ODataException(Strings.ODataJsonReader_CannotReadEntryStart(base.JsonReader.NodeType));
			}
			base.JsonReader.ReadNext();
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x0004260C File Offset: 0x0004080C
		internal void ReadEntryMetadataPropertyValue(IODataVerboseJsonReaderEntryState entryState)
		{
			ODataEntry entry = entryState.Entry;
			base.JsonReader.ReadStartObject();
			ODataStreamReferenceValue mediaResource = null;
			ODataVerboseJsonReaderUtils.MetadataPropertyBitMask metadataPropertyBitMask = ODataVerboseJsonReaderUtils.MetadataPropertyBitMask.None;
			while (base.JsonReader.NodeType == JsonNodeType.Property)
			{
				string text = base.JsonReader.ReadPropertyName();
				string key;
				switch (key = text)
				{
				case "uri":
					this.ReadUriMetadataProperty(entry, ref metadataPropertyBitMask);
					continue;
				case "id":
					this.ReadIdMetadataProperty(entry, ref metadataPropertyBitMask);
					continue;
				case "etag":
					this.ReadETagMetadataProperty(entry, ref metadataPropertyBitMask);
					continue;
				case "type":
					base.JsonReader.SkipValue();
					continue;
				case "media_src":
					this.ReadMediaSourceMetadataProperty(ref metadataPropertyBitMask, ref mediaResource);
					continue;
				case "edit_media":
					this.ReadEditMediaMetadataProperty(ref metadataPropertyBitMask, ref mediaResource);
					continue;
				case "content_type":
					this.ReadContentTypeMetadataProperty(ref metadataPropertyBitMask, ref mediaResource);
					continue;
				case "media_etag":
					this.ReadMediaETagMetadataProperty(ref metadataPropertyBitMask, ref mediaResource);
					continue;
				case "actions":
					this.ReadActionsMetadataProperty(entry, ref metadataPropertyBitMask);
					continue;
				case "functions":
					this.ReadFunctionsMetadataProperty(entry, ref metadataPropertyBitMask);
					continue;
				case "properties":
					this.ReadPropertiesMetadataProperty(entryState, ref metadataPropertyBitMask);
					continue;
				}
				base.JsonReader.SkipValue();
			}
			entry.MediaResource = mediaResource;
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x000427D8 File Offset: 0x000409D8
		internal void ValidateEntryMetadata(IODataVerboseJsonReaderEntryState entryState)
		{
			ODataEntry entry = entryState.Entry;
			IEdmEntityType entityType = entryState.EntityType;
			if (base.Model.HasDefaultStream(entityType) && entry.MediaResource == null)
			{
				ODataStreamReferenceValue mediaResource = null;
				ODataVerboseJsonReaderUtils.EnsureInstance<ODataStreamReferenceValue>(ref mediaResource);
				entry.MediaResource = mediaResource;
			}
			bool useDefaultFormatBehavior = base.UseDefaultFormatBehavior;
			ValidationUtils.ValidateEntryMetadataResource(entry, entityType, base.Model, useDefaultFormatBehavior);
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x00042830 File Offset: 0x00040A30
		internal ODataNavigationLink ReadEntryContent(IODataVerboseJsonReaderEntryState entryState, out IEdmNavigationProperty navigationProperty)
		{
			ODataNavigationLink odataNavigationLink = null;
			navigationProperty = null;
			IEdmEntityType entityType = entryState.EntityType;
			while (base.JsonReader.NodeType == JsonNodeType.Property)
			{
				string text = base.JsonReader.ReadPropertyName();
				if (string.CompareOrdinal("__metadata", text) == 0)
				{
					if (entryState.MetadataPropertyFound)
					{
						throw new ODataException(Strings.ODataJsonEntryAndFeedDeserializer_MultipleMetadataPropertiesInEntryValue);
					}
					entryState.MetadataPropertyFound = true;
					base.JsonReader.SkipValue();
				}
				else
				{
					if (!ValidationUtils.IsValidPropertyName(text))
					{
						base.JsonReader.SkipValue();
						continue;
					}
					IEdmProperty edmProperty = ReaderValidationUtils.FindDefinedProperty(text, entityType);
					if (edmProperty != null)
					{
						navigationProperty = (edmProperty as IEdmNavigationProperty);
						if (navigationProperty != null)
						{
							if (this.ShouldEntryPropertyBeSkipped())
							{
								base.JsonReader.SkipValue();
							}
							else
							{
								bool flag = navigationProperty.Type.IsCollection();
								odataNavigationLink = new ODataNavigationLink
								{
									Name = text,
									IsCollection = new bool?(flag)
								};
								this.ValidateNavigationLinkPropertyValue(flag);
								entryState.DuplicatePropertyNamesChecker.CheckForDuplicatePropertyNamesOnNavigationLinkStart(odataNavigationLink);
							}
						}
						else
						{
							this.ReadEntryProperty(entryState, edmProperty);
						}
					}
					else
					{
						odataNavigationLink = this.ReadUndeclaredProperty(entryState, text);
					}
				}
				if (odataNavigationLink != null)
				{
					break;
				}
			}
			return odataNavigationLink;
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x00042944 File Offset: 0x00040B44
		internal void ReadDeferredNavigationLink(ODataNavigationLink navigationLink)
		{
			base.JsonReader.ReadStartObject();
			base.JsonReader.ReadPropertyName();
			base.JsonReader.ReadStartObject();
			while (base.JsonReader.NodeType == JsonNodeType.Property)
			{
				string strB = base.JsonReader.ReadPropertyName();
				if (string.CompareOrdinal("uri", strB) == 0)
				{
					if (navigationLink.Url != null)
					{
						throw new ODataException(Strings.ODataJsonEntryAndFeedDeserializer_MultipleUriPropertiesInDeferredLink);
					}
					string text = base.JsonReader.ReadStringValue("uri");
					if (text == null)
					{
						throw new ODataException(Strings.ODataJsonEntryAndFeedDeserializer_DeferredLinkUriCannotBeNull);
					}
					navigationLink.Url = base.ProcessUriFromPayload(text);
				}
				else
				{
					base.JsonReader.SkipValue();
				}
			}
			if (navigationLink.Url == null)
			{
				throw new ODataException(Strings.ODataJsonEntryAndFeedDeserializer_DeferredLinkMissingUri);
			}
			base.JsonReader.ReadEndObject();
			base.JsonReader.ReadEndObject();
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x00042A20 File Offset: 0x00040C20
		internal ODataEntityReferenceLink ReadEntityReferenceLink()
		{
			base.JsonReader.ReadStartObject();
			base.JsonReader.ReadPropertyName();
			base.JsonReader.ReadStartObject();
			ODataEntityReferenceLink odataEntityReferenceLink = new ODataEntityReferenceLink();
			ODataVerboseJsonReaderUtils.MetadataPropertyBitMask metadataPropertyBitMask = ODataVerboseJsonReaderUtils.MetadataPropertyBitMask.None;
			while (base.JsonReader.NodeType == JsonNodeType.Property)
			{
				string strB = base.JsonReader.ReadPropertyName();
				if (string.CompareOrdinal("uri", strB) == 0)
				{
					ODataVerboseJsonReaderUtils.VerifyMetadataPropertyNotFound(ref metadataPropertyBitMask, ODataVerboseJsonReaderUtils.MetadataPropertyBitMask.Uri, "uri");
					string text = base.JsonReader.ReadStringValue("uri");
					ODataVerboseJsonReaderUtils.ValidateMetadataStringProperty(text, "uri");
					odataEntityReferenceLink.Url = base.ProcessUriFromPayload(text);
				}
				else
				{
					base.JsonReader.SkipValue();
				}
			}
			base.JsonReader.ReadEndObject();
			base.JsonReader.ReadEndObject();
			return odataEntityReferenceLink;
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x00042ADC File Offset: 0x00040CDC
		internal bool IsDeferredLink(bool navigationLinkFound)
		{
			JsonNodeType nodeType = base.JsonReader.NodeType;
			if (nodeType == JsonNodeType.PrimitiveValue)
			{
				if (base.JsonReader.Value == null)
				{
					return false;
				}
				if (navigationLinkFound)
				{
					throw new ODataException(Strings.ODataJsonEntryAndFeedDeserializer_CannotReadNavigationPropertyValue);
				}
				return false;
			}
			else
			{
				if (nodeType == JsonNodeType.StartArray)
				{
					return false;
				}
				base.JsonReader.StartBuffering();
				bool result;
				try
				{
					base.JsonReader.ReadStartObject();
					nodeType = base.JsonReader.NodeType;
					if (nodeType == JsonNodeType.EndObject)
					{
						result = false;
					}
					else
					{
						string strB = base.JsonReader.ReadPropertyName();
						if (string.CompareOrdinal("__deferred", strB) != 0)
						{
							result = false;
						}
						else
						{
							base.JsonReader.SkipValue();
							result = (base.JsonReader.NodeType == JsonNodeType.EndObject);
						}
					}
				}
				finally
				{
					base.JsonReader.StopBuffering();
				}
				return result;
			}
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x00042BA0 File Offset: 0x00040DA0
		internal bool IsEntityReferenceLink()
		{
			JsonNodeType nodeType = base.JsonReader.NodeType;
			if (nodeType != JsonNodeType.StartObject)
			{
				return false;
			}
			base.JsonReader.StartBuffering();
			bool result;
			try
			{
				base.JsonReader.ReadStartObject();
				nodeType = base.JsonReader.NodeType;
				if (nodeType == JsonNodeType.EndObject)
				{
					result = false;
				}
				else
				{
					bool flag = false;
					while (base.JsonReader.NodeType == JsonNodeType.Property)
					{
						string strB = base.JsonReader.ReadPropertyName();
						if (string.CompareOrdinal("__metadata", strB) != 0)
						{
							return false;
						}
						if (base.JsonReader.NodeType != JsonNodeType.StartObject)
						{
							return false;
						}
						base.JsonReader.ReadStartObject();
						while (base.JsonReader.NodeType == JsonNodeType.Property)
						{
							string strB2 = base.JsonReader.ReadPropertyName();
							if (string.CompareOrdinal("uri", strB2) == 0)
							{
								flag = true;
							}
							base.JsonReader.SkipValue();
						}
						base.JsonReader.ReadEndObject();
					}
					result = flag;
				}
			}
			finally
			{
				base.JsonReader.StopBuffering();
			}
			return result;
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x00042CA8 File Offset: 0x00040EA8
		private static void AddEntryProperty(IODataVerboseJsonReaderEntryState entryState, string propertyName, object propertyValue)
		{
			ODataProperty odataProperty = new ODataProperty
			{
				Name = propertyName,
				Value = propertyValue
			};
			entryState.DuplicatePropertyNamesChecker.CheckForDuplicatePropertyNames(odataProperty);
			ODataEntry entry = entryState.Entry;
			entry.Properties = entry.Properties.ConcatToReadOnlyEnumerable("Properties", odataProperty);
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x00042CF8 File Offset: 0x00040EF8
		private void ReadFeedProperty(ODataFeed feed, string propertyName, bool isExpandedLinkContent)
		{
			switch (ODataVerboseJsonReaderUtils.DetermineFeedPropertyKind(propertyName))
			{
			case ODataVerboseJsonReaderUtils.FeedPropertyKind.Unsupported:
				base.JsonReader.SkipValue();
				return;
			case ODataVerboseJsonReaderUtils.FeedPropertyKind.Count:
				if (base.ReadingResponse && base.Version >= ODataVersion.V2 && !isExpandedLinkContent)
				{
					string text = base.JsonReader.ReadStringValue("__count");
					ODataVerboseJsonReaderUtils.ValidateFeedProperty(text, "__count");
					long value = (long)ODataVerboseJsonReaderUtils.ConvertValue(text, EdmCoreModel.Instance.GetInt64(false), base.MessageReaderSettings, base.Version, true, propertyName);
					feed.Count = new long?(value);
					return;
				}
				base.JsonReader.SkipValue();
				return;
			case ODataVerboseJsonReaderUtils.FeedPropertyKind.Results:
				throw new ODataException(Strings.ODataJsonEntryAndFeedDeserializer_MultipleFeedResultsPropertiesFound);
			case ODataVerboseJsonReaderUtils.FeedPropertyKind.NextPageLink:
				if (base.ReadingResponse && base.Version >= ODataVersion.V2)
				{
					string text2 = base.JsonReader.ReadStringValue("__next");
					ODataVerboseJsonReaderUtils.ValidateFeedProperty(text2, "__next");
					feed.NextPageLink = base.ProcessUriFromPayload(text2);
					return;
				}
				base.JsonReader.SkipValue();
				return;
			default:
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataVerboseJsonEntryAndFeedDeserializer_ReadFeedProperty));
			}
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x00042E08 File Offset: 0x00041008
		private void ReadEntryProperty(IODataVerboseJsonReaderEntryState entryState, IEdmProperty edmProperty)
		{
			ODataNullValueBehaviorKind odataNullValueBehaviorKind = base.ReadingResponse ? ODataNullValueBehaviorKind.Default : base.Model.NullValueReadBehaviorKind(edmProperty);
			IEdmTypeReference type = edmProperty.Type;
			object obj = type.IsStream() ? this.ReadStreamPropertyValue() : base.ReadNonEntityValue(type, null, null, odataNullValueBehaviorKind == ODataNullValueBehaviorKind.Default, edmProperty.Name);
			if (odataNullValueBehaviorKind != ODataNullValueBehaviorKind.IgnoreValue || obj != null)
			{
				ODataVerboseJsonEntryAndFeedDeserializer.AddEntryProperty(entryState, edmProperty.Name, obj);
			}
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x00042E6C File Offset: 0x0004106C
		private void ReadOpenProperty(IODataVerboseJsonReaderEntryState entryState, string propertyName)
		{
			object obj = base.ReadNonEntityValue(null, null, null, true, propertyName);
			ValidationUtils.ValidateOpenPropertyValue(propertyName, obj);
			ODataVerboseJsonEntryAndFeedDeserializer.AddEntryProperty(entryState, propertyName, obj);
		}

		// Token: 0x060011E9 RID: 4585 RVA: 0x00042E94 File Offset: 0x00041094
		private ODataNavigationLink ReadUndeclaredProperty(IODataVerboseJsonReaderEntryState entryState, string propertyName)
		{
			if (entryState.EntityType.IsOpen && this.ShouldEntryPropertyBeSkipped())
			{
				base.JsonReader.SkipValue();
				return null;
			}
			bool flag = false;
			bool flag2 = false;
			if (base.JsonReader.NodeType == JsonNodeType.StartObject)
			{
				base.JsonReader.StartBuffering();
				base.JsonReader.Read();
				if (base.JsonReader.NodeType == JsonNodeType.Property)
				{
					string strA = base.JsonReader.ReadPropertyName();
					if (string.CompareOrdinal(strA, "__deferred") == 0)
					{
						flag2 = true;
					}
					else if (string.CompareOrdinal(strA, "__mediaresource") == 0)
					{
						flag = true;
					}
					base.JsonReader.SkipValue();
					if (base.JsonReader.NodeType != JsonNodeType.EndObject)
					{
						flag = false;
						flag2 = false;
					}
				}
				base.JsonReader.StopBuffering();
			}
			if (flag || flag2)
			{
				if (!base.MessageReaderSettings.ReportUndeclaredLinkProperties)
				{
					throw new ODataException(Strings.ValidationUtils_PropertyDoesNotExistOnType(propertyName, entryState.EntityType.ODataFullName()));
				}
			}
			else
			{
				if (entryState.EntityType.IsOpen)
				{
					this.ReadOpenProperty(entryState, propertyName);
					return null;
				}
				if (!base.MessageReaderSettings.IgnoreUndeclaredValueProperties)
				{
					throw new ODataException(Strings.ValidationUtils_PropertyDoesNotExistOnType(propertyName, entryState.EntityType.ODataFullName()));
				}
			}
			if (flag2)
			{
				ODataNavigationLink odataNavigationLink = new ODataNavigationLink
				{
					Name = propertyName,
					IsCollection = null
				};
				entryState.DuplicatePropertyNamesChecker.CheckForDuplicatePropertyNamesOnNavigationLinkStart(odataNavigationLink);
				return odataNavigationLink;
			}
			if (flag)
			{
				object propertyValue = this.ReadStreamPropertyValue();
				ODataVerboseJsonEntryAndFeedDeserializer.AddEntryProperty(entryState, propertyName, propertyValue);
				return null;
			}
			base.JsonReader.SkipValue();
			return null;
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x0004300C File Offset: 0x0004120C
		private ODataStreamReferenceValue ReadStreamPropertyValue()
		{
			if (!base.ReadingResponse)
			{
				throw new ODataException(Strings.ODataJsonEntryAndFeedDeserializer_StreamPropertyInRequest);
			}
			ODataVersionChecker.CheckStreamReferenceProperty(base.Version);
			base.JsonReader.ReadStartObject();
			if (base.JsonReader.NodeType != JsonNodeType.Property)
			{
				throw new ODataException(Strings.ODataJsonEntryAndFeedDeserializer_CannotParseStreamReference);
			}
			string strB = base.JsonReader.ReadPropertyName();
			if (string.CompareOrdinal("__mediaresource", strB) != 0)
			{
				throw new ODataException(Strings.ODataJsonEntryAndFeedDeserializer_CannotParseStreamReference);
			}
			ODataStreamReferenceValue result = this.ReadStreamReferenceValue();
			if (base.JsonReader.NodeType != JsonNodeType.EndObject)
			{
				throw new ODataException(Strings.ODataJsonEntryAndFeedDeserializer_CannotParseStreamReference);
			}
			base.JsonReader.Read();
			return result;
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x000430B0 File Offset: 0x000412B0
		private void ReadUriMetadataProperty(ODataEntry entry, ref ODataVerboseJsonReaderUtils.MetadataPropertyBitMask metadataPropertiesFoundBitField)
		{
			ODataVerboseJsonReaderUtils.VerifyMetadataPropertyNotFound(ref metadataPropertiesFoundBitField, ODataVerboseJsonReaderUtils.MetadataPropertyBitMask.Uri, "uri");
			string text = base.JsonReader.ReadStringValue("uri");
			if (text != null)
			{
				ODataVerboseJsonReaderUtils.ValidateMetadataStringProperty(text, "uri");
				entry.EditLink = base.ProcessUriFromPayload(text);
			}
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x000430F8 File Offset: 0x000412F8
		private void ReadIdMetadataProperty(ODataEntry entry, ref ODataVerboseJsonReaderUtils.MetadataPropertyBitMask metadataPropertiesFoundBitField)
		{
			if (base.UseServerFormatBehavior)
			{
				base.JsonReader.SkipValue();
				return;
			}
			ODataVerboseJsonReaderUtils.VerifyMetadataPropertyNotFound(ref metadataPropertiesFoundBitField, ODataVerboseJsonReaderUtils.MetadataPropertyBitMask.Id, "id");
			string text = base.JsonReader.ReadStringValue("id");
			ODataVerboseJsonReaderUtils.ValidateMetadataStringProperty(text, "id");
			entry.Id = text;
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x0004314C File Offset: 0x0004134C
		private void ReadETagMetadataProperty(ODataEntry entry, ref ODataVerboseJsonReaderUtils.MetadataPropertyBitMask metadataPropertiesFoundBitField)
		{
			if (base.UseServerFormatBehavior)
			{
				base.JsonReader.SkipValue();
				return;
			}
			ODataVerboseJsonReaderUtils.VerifyMetadataPropertyNotFound(ref metadataPropertiesFoundBitField, ODataVerboseJsonReaderUtils.MetadataPropertyBitMask.ETag, "etag");
			string text = base.JsonReader.ReadStringValue("etag");
			ODataVerboseJsonReaderUtils.ValidateMetadataStringProperty(text, "etag");
			entry.ETag = text;
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x0004319C File Offset: 0x0004139C
		private void ReadMediaSourceMetadataProperty(ref ODataVerboseJsonReaderUtils.MetadataPropertyBitMask metadataPropertiesFoundBitField, ref ODataStreamReferenceValue mediaResource)
		{
			if (base.UseServerFormatBehavior)
			{
				base.JsonReader.SkipValue();
				return;
			}
			ODataVerboseJsonReaderUtils.VerifyMetadataPropertyNotFound(ref metadataPropertiesFoundBitField, ODataVerboseJsonReaderUtils.MetadataPropertyBitMask.MediaUri, "media_src");
			ODataVerboseJsonReaderUtils.EnsureInstance<ODataStreamReferenceValue>(ref mediaResource);
			string text = base.JsonReader.ReadStringValue("media_src");
			ODataVerboseJsonReaderUtils.ValidateMetadataStringProperty(text, "media_src");
			mediaResource.ReadLink = base.ProcessUriFromPayload(text);
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x000431FC File Offset: 0x000413FC
		private void ReadEditMediaMetadataProperty(ref ODataVerboseJsonReaderUtils.MetadataPropertyBitMask metadataPropertiesFoundBitField, ref ODataStreamReferenceValue mediaResource)
		{
			if (base.UseServerFormatBehavior)
			{
				base.JsonReader.SkipValue();
				return;
			}
			ODataVerboseJsonReaderUtils.VerifyMetadataPropertyNotFound(ref metadataPropertiesFoundBitField, ODataVerboseJsonReaderUtils.MetadataPropertyBitMask.EditMedia, "edit_media");
			ODataVerboseJsonReaderUtils.EnsureInstance<ODataStreamReferenceValue>(ref mediaResource);
			string text = base.JsonReader.ReadStringValue("edit_media");
			ODataVerboseJsonReaderUtils.ValidateMetadataStringProperty(text, "edit_media");
			mediaResource.EditLink = base.ProcessUriFromPayload(text);
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x0004325C File Offset: 0x0004145C
		private void ReadContentTypeMetadataProperty(ref ODataVerboseJsonReaderUtils.MetadataPropertyBitMask metadataPropertiesFoundBitField, ref ODataStreamReferenceValue mediaResource)
		{
			if (base.UseServerFormatBehavior)
			{
				base.JsonReader.SkipValue();
				return;
			}
			ODataVerboseJsonReaderUtils.VerifyMetadataPropertyNotFound(ref metadataPropertiesFoundBitField, ODataVerboseJsonReaderUtils.MetadataPropertyBitMask.ContentType, "content_type");
			ODataVerboseJsonReaderUtils.EnsureInstance<ODataStreamReferenceValue>(ref mediaResource);
			string text = base.JsonReader.ReadStringValue("content_type");
			ODataVerboseJsonReaderUtils.ValidateMetadataStringProperty(text, "content_type");
			mediaResource.ContentType = text;
		}

		// Token: 0x060011F1 RID: 4593 RVA: 0x000432B4 File Offset: 0x000414B4
		private void ReadMediaETagMetadataProperty(ref ODataVerboseJsonReaderUtils.MetadataPropertyBitMask metadataPropertiesFoundBitField, ref ODataStreamReferenceValue mediaResource)
		{
			if (base.UseServerFormatBehavior)
			{
				base.JsonReader.SkipValue();
				return;
			}
			ODataVerboseJsonReaderUtils.VerifyMetadataPropertyNotFound(ref metadataPropertiesFoundBitField, ODataVerboseJsonReaderUtils.MetadataPropertyBitMask.MediaETag, "media_etag");
			ODataVerboseJsonReaderUtils.EnsureInstance<ODataStreamReferenceValue>(ref mediaResource);
			string text = base.JsonReader.ReadStringValue("media_etag");
			ODataVerboseJsonReaderUtils.ValidateMetadataStringProperty(text, "media_etag");
			mediaResource.ETag = text;
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x0004330C File Offset: 0x0004150C
		private void ReadActionsMetadataProperty(ODataEntry entry, ref ODataVerboseJsonReaderUtils.MetadataPropertyBitMask metadataPropertiesFoundBitField)
		{
			if (base.MessageReaderSettings.MaxProtocolVersion >= ODataVersion.V3 && base.ReadingResponse)
			{
				ODataVerboseJsonReaderUtils.VerifyMetadataPropertyNotFound(ref metadataPropertiesFoundBitField, ODataVerboseJsonReaderUtils.MetadataPropertyBitMask.Actions, "actions");
				this.ReadOperationsMetadata(entry, true);
				return;
			}
			base.JsonReader.SkipValue();
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x00043348 File Offset: 0x00041548
		private void ReadFunctionsMetadataProperty(ODataEntry entry, ref ODataVerboseJsonReaderUtils.MetadataPropertyBitMask metadataPropertiesFoundBitField)
		{
			if (base.MessageReaderSettings.MaxProtocolVersion >= ODataVersion.V3 && base.ReadingResponse)
			{
				ODataVerboseJsonReaderUtils.VerifyMetadataPropertyNotFound(ref metadataPropertiesFoundBitField, ODataVerboseJsonReaderUtils.MetadataPropertyBitMask.Functions, "functions");
				this.ReadOperationsMetadata(entry, false);
				return;
			}
			base.JsonReader.SkipValue();
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x00043384 File Offset: 0x00041584
		private void ReadPropertiesMetadataProperty(IODataVerboseJsonReaderEntryState entryState, ref ODataVerboseJsonReaderUtils.MetadataPropertyBitMask metadataPropertiesFoundBitField)
		{
			if (!base.ReadingResponse || base.MessageReaderSettings.MaxProtocolVersion < ODataVersion.V3)
			{
				base.JsonReader.SkipValue();
				return;
			}
			ODataVerboseJsonReaderUtils.VerifyMetadataPropertyNotFound(ref metadataPropertiesFoundBitField, ODataVerboseJsonReaderUtils.MetadataPropertyBitMask.Properties, "properties");
			if (base.JsonReader.NodeType != JsonNodeType.StartObject)
			{
				throw new ODataException(Strings.ODataJsonEntryAndFeedDeserializer_PropertyInEntryMustHaveObjectValue("properties", base.JsonReader.NodeType));
			}
			base.JsonReader.ReadStartObject();
			while (base.JsonReader.NodeType == JsonNodeType.Property)
			{
				string text = base.JsonReader.ReadPropertyName();
				ValidationUtils.ValidateAssociationLinkName(text);
				ReaderValidationUtils.ValidateNavigationPropertyDefined(text, entryState.EntityType, base.MessageReaderSettings);
				base.JsonReader.ReadStartObject();
				while (base.JsonReader.NodeType == JsonNodeType.Property)
				{
					string strA = base.JsonReader.ReadPropertyName();
					if (string.CompareOrdinal(strA, "associationuri") == 0)
					{
						string text2 = base.JsonReader.ReadStringValue("associationuri");
						ODataVerboseJsonReaderUtils.ValidateMetadataStringProperty(text2, "associationuri");
						ODataAssociationLink associationLink = new ODataAssociationLink
						{
							Name = text,
							Url = base.ProcessUriFromPayload(text2)
						};
						ValidationUtils.ValidateAssociationLink(associationLink);
						ReaderUtils.CheckForDuplicateAssociationLinkAndUpdateNavigationLink(entryState.DuplicatePropertyNamesChecker, associationLink);
						entryState.Entry.AddAssociationLink(associationLink);
					}
					else
					{
						base.JsonReader.SkipValue();
					}
				}
				base.JsonReader.ReadEndObject();
			}
			base.JsonReader.ReadEndObject();
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x000434F0 File Offset: 0x000416F0
		private void ReadOperationsMetadata(ODataEntry entry, bool isActions)
		{
			IODataJsonOperationsDeserializerContext iodataJsonOperationsDeserializerContext = new ODataVerboseJsonEntryAndFeedDeserializer.OperationsDeserializerContext(entry, this);
			string text = isActions ? "actions" : "functions";
			if (iodataJsonOperationsDeserializerContext.JsonReader.NodeType != JsonNodeType.StartObject)
			{
				throw new ODataException(Strings.ODataJsonOperationsDeserializerUtils_OperationsPropertyMustHaveObjectValue(text, iodataJsonOperationsDeserializerContext.JsonReader.NodeType));
			}
			iodataJsonOperationsDeserializerContext.JsonReader.ReadStartObject();
			HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
			while (iodataJsonOperationsDeserializerContext.JsonReader.NodeType == JsonNodeType.Property)
			{
				string text2 = iodataJsonOperationsDeserializerContext.JsonReader.ReadPropertyName();
				if (hashSet.Contains(text2))
				{
					throw new ODataException(Strings.ODataJsonOperationsDeserializerUtils_RepeatMetadataValue(text, text2));
				}
				hashSet.Add(text2);
				if (iodataJsonOperationsDeserializerContext.JsonReader.NodeType != JsonNodeType.StartArray)
				{
					throw new ODataException(Strings.ODataJsonOperationsDeserializerUtils_MetadataMustHaveArrayValue(text2, iodataJsonOperationsDeserializerContext.JsonReader.NodeType, text));
				}
				iodataJsonOperationsDeserializerContext.JsonReader.ReadStartArray();
				if (iodataJsonOperationsDeserializerContext.JsonReader.NodeType != JsonNodeType.StartObject)
				{
					throw new ODataException(Strings.ODataJsonOperationsDeserializerUtils_OperationMetadataArrayExpectedAnObject(text2, iodataJsonOperationsDeserializerContext.JsonReader.NodeType, text));
				}
				Uri metadata = this.ResolveUri(text2);
				while (iodataJsonOperationsDeserializerContext.JsonReader.NodeType == JsonNodeType.StartObject)
				{
					iodataJsonOperationsDeserializerContext.JsonReader.ReadStartObject();
					ODataOperation odataOperation;
					if (isActions)
					{
						odataOperation = new ODataAction();
					}
					else
					{
						odataOperation = new ODataFunction();
					}
					odataOperation.Metadata = metadata;
					while (iodataJsonOperationsDeserializerContext.JsonReader.NodeType == JsonNodeType.Property)
					{
						string text3 = iodataJsonOperationsDeserializerContext.JsonReader.ReadPropertyName();
						string a;
						if ((a = text3) != null)
						{
							if (!(a == "title"))
							{
								if (a == "target")
								{
									if (odataOperation.Target != null)
									{
										throw new ODataException(Strings.ODataJsonOperationsDeserializerUtils_MultipleTargetPropertiesInOperation(text2, text));
									}
									string text4 = iodataJsonOperationsDeserializerContext.JsonReader.ReadStringValue("target");
									ReaderValidationUtils.ValidateOperationProperty(text4, text3, text2, text);
									odataOperation.Target = iodataJsonOperationsDeserializerContext.ProcessUriFromPayload(text4);
									continue;
								}
							}
							else
							{
								if (odataOperation.Title != null)
								{
									throw new ODataException(Strings.ODataJsonOperationsDeserializerUtils_MultipleOptionalPropertiesInOperation(text3, text2, text));
								}
								string text5 = iodataJsonOperationsDeserializerContext.JsonReader.ReadStringValue("title");
								ReaderValidationUtils.ValidateOperationProperty(text5, text3, text2, text);
								odataOperation.Title = text5;
								continue;
							}
						}
						iodataJsonOperationsDeserializerContext.JsonReader.SkipValue();
					}
					if (odataOperation.Target == null)
					{
						throw new ODataException(Strings.ODataJsonOperationsDeserializerUtils_OperationMissingTargetProperty(text2, text));
					}
					iodataJsonOperationsDeserializerContext.JsonReader.ReadEndObject();
					if (isActions)
					{
						iodataJsonOperationsDeserializerContext.AddActionToEntry((ODataAction)odataOperation);
					}
					else
					{
						iodataJsonOperationsDeserializerContext.AddFunctionToEntry((ODataFunction)odataOperation);
					}
				}
				iodataJsonOperationsDeserializerContext.JsonReader.ReadEndArray();
			}
			iodataJsonOperationsDeserializerContext.JsonReader.ReadEndObject();
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x0004377C File Offset: 0x0004197C
		private ODataStreamReferenceValue ReadStreamReferenceValue()
		{
			base.JsonReader.ReadStartObject();
			ODataStreamReferenceValue odataStreamReferenceValue = new ODataStreamReferenceValue();
			while (base.JsonReader.NodeType == JsonNodeType.Property)
			{
				string text = base.JsonReader.ReadPropertyName();
				string a;
				if ((a = text) != null)
				{
					if (!(a == "edit_media"))
					{
						if (!(a == "media_src"))
						{
							if (!(a == "content_type"))
							{
								if (a == "media_etag")
								{
									if (odataStreamReferenceValue.ETag != null)
									{
										throw new ODataException(Strings.ODataJsonEntryAndFeedDeserializer_MultipleMetadataPropertiesForStreamProperty("media_etag"));
									}
									string text2 = base.JsonReader.ReadStringValue("media_etag");
									ODataVerboseJsonReaderUtils.ValidateMediaResourceStringProperty(text2, "media_etag");
									odataStreamReferenceValue.ETag = text2;
									continue;
								}
							}
							else
							{
								if (odataStreamReferenceValue.ContentType != null)
								{
									throw new ODataException(Strings.ODataJsonEntryAndFeedDeserializer_MultipleMetadataPropertiesForStreamProperty("content_type"));
								}
								string text3 = base.JsonReader.ReadStringValue("content_type");
								ODataVerboseJsonReaderUtils.ValidateMediaResourceStringProperty(text3, "content_type");
								odataStreamReferenceValue.ContentType = text3;
								continue;
							}
						}
						else
						{
							if (odataStreamReferenceValue.ReadLink != null)
							{
								throw new ODataException(Strings.ODataJsonEntryAndFeedDeserializer_MultipleMetadataPropertiesForStreamProperty("media_src"));
							}
							string text4 = base.JsonReader.ReadStringValue("media_src");
							ODataVerboseJsonReaderUtils.ValidateMediaResourceStringProperty(text4, "media_src");
							odataStreamReferenceValue.ReadLink = base.ProcessUriFromPayload(text4);
							continue;
						}
					}
					else
					{
						if (odataStreamReferenceValue.EditLink != null)
						{
							throw new ODataException(Strings.ODataJsonEntryAndFeedDeserializer_MultipleMetadataPropertiesForStreamProperty("edit_media"));
						}
						string text5 = base.JsonReader.ReadStringValue("edit_media");
						ODataVerboseJsonReaderUtils.ValidateMediaResourceStringProperty(text5, "edit_media");
						odataStreamReferenceValue.EditLink = base.ProcessUriFromPayload(text5);
						continue;
					}
				}
				base.JsonReader.SkipValue();
			}
			base.JsonReader.ReadEndObject();
			return odataStreamReferenceValue;
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x00043938 File Offset: 0x00041B38
		private Uri ResolveUri(string uriFromPayload)
		{
			Uri uri = new Uri(uriFromPayload, UriKind.RelativeOrAbsolute);
			Uri uri2 = base.VerboseJsonInputContext.ResolveUri(base.MessageReaderSettings.BaseUri, uri);
			if (uri2 != null)
			{
				return uri2;
			}
			return uri;
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x00043974 File Offset: 0x00041B74
		private void ValidateNavigationLinkPropertyValue(bool isCollection)
		{
			JsonNodeType nodeType = base.JsonReader.NodeType;
			if (nodeType == JsonNodeType.StartArray)
			{
				if (!isCollection)
				{
					throw new ODataException(Strings.ODataJsonEntryAndFeedDeserializer_CannotReadSingletonNavigationPropertyValue(nodeType));
				}
			}
			else if (nodeType == JsonNodeType.PrimitiveValue && base.JsonReader.Value == null)
			{
				if (isCollection)
				{
					throw new ODataException(Strings.ODataJsonEntryAndFeedDeserializer_CannotReadCollectionNavigationPropertyValue(nodeType));
				}
			}
			else if (nodeType != JsonNodeType.StartObject)
			{
				throw new ODataException(Strings.ODataJsonEntryAndFeedDeserializer_CannotReadNavigationPropertyValue);
			}
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x000439D9 File Offset: 0x00041BD9
		private bool ShouldEntryPropertyBeSkipped()
		{
			return !base.ReadingResponse && base.UseServerFormatBehavior && this.IsDeferredLink(false);
		}

		// Token: 0x02000233 RID: 563
		private sealed class OperationsDeserializerContext : IODataJsonOperationsDeserializerContext
		{
			// Token: 0x060011FA RID: 4602 RVA: 0x000439F4 File Offset: 0x00041BF4
			public OperationsDeserializerContext(ODataEntry entry, ODataVerboseJsonEntryAndFeedDeserializer verboseJsonEntryAndFeedDeserializer)
			{
				this.entry = entry;
				this.verboseJsonEntryAndFeedDeserializer = verboseJsonEntryAndFeedDeserializer;
			}

			// Token: 0x170003D6 RID: 982
			// (get) Token: 0x060011FB RID: 4603 RVA: 0x00043A0A File Offset: 0x00041C0A
			public JsonReader JsonReader
			{
				get
				{
					return this.verboseJsonEntryAndFeedDeserializer.JsonReader;
				}
			}

			// Token: 0x060011FC RID: 4604 RVA: 0x00043A17 File Offset: 0x00041C17
			public Uri ProcessUriFromPayload(string uriFromPayload)
			{
				return this.verboseJsonEntryAndFeedDeserializer.ProcessUriFromPayload(uriFromPayload);
			}

			// Token: 0x060011FD RID: 4605 RVA: 0x00043A25 File Offset: 0x00041C25
			public void AddActionToEntry(ODataAction action)
			{
				this.entry.AddAction(action);
			}

			// Token: 0x060011FE RID: 4606 RVA: 0x00043A33 File Offset: 0x00041C33
			public void AddFunctionToEntry(ODataFunction function)
			{
				this.entry.AddFunction(function);
			}

			// Token: 0x04000688 RID: 1672
			private ODataEntry entry;

			// Token: 0x04000689 RID: 1673
			private ODataVerboseJsonEntryAndFeedDeserializer verboseJsonEntryAndFeedDeserializer;
		}
	}
}
