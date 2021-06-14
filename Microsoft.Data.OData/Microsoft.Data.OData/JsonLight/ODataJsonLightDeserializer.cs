using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData.Evaluation;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x02000112 RID: 274
	internal abstract class ODataJsonLightDeserializer : ODataDeserializer
	{
		// Token: 0x0600075C RID: 1884 RVA: 0x000190FC File Offset: 0x000172FC
		protected ODataJsonLightDeserializer(ODataJsonLightInputContext jsonLightInputContext) : base(jsonLightInputContext)
		{
			this.jsonLightInputContext = jsonLightInputContext;
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x0600075D RID: 1885 RVA: 0x0001910C File Offset: 0x0001730C
		internal IODataMetadataContext MetadataContext
		{
			get
			{
				Func<IEdmEntityType, bool> operationsBoundToEntityTypeMustBeContainerQualified = base.MessageReaderSettings.ReaderBehavior.OperationsBoundToEntityTypeMustBeContainerQualified;
				IODataMetadataContext result;
				if ((result = this.metadataContext) == null)
				{
					result = (this.metadataContext = new ODataMetadataContext(base.ReadingResponse, operationsBoundToEntityTypeMustBeContainerQualified, this.JsonLightInputContext.EdmTypeResolver, base.Model, this.MetadataDocumentUri));
				}
				return result;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x0600075E RID: 1886 RVA: 0x00019160 File Offset: 0x00017360
		internal BufferingJsonReader JsonReader
		{
			get
			{
				return this.jsonLightInputContext.JsonReader;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x0600075F RID: 1887 RVA: 0x0001916D File Offset: 0x0001736D
		internal ODataJsonLightMetadataUriParseResult MetadataUriParseResult
		{
			get
			{
				return this.metadataUriParseResult;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x00019175 File Offset: 0x00017375
		protected ODataJsonLightInputContext JsonLightInputContext
		{
			get
			{
				return this.jsonLightInputContext;
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000761 RID: 1889 RVA: 0x00019180 File Offset: 0x00017380
		private Uri MetadataDocumentUri
		{
			get
			{
				return (this.MetadataUriParseResult != null && this.MetadataUriParseResult.MetadataDocumentUri != null) ? this.MetadataUriParseResult.MetadataDocumentUri : null;
			}
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x000191B8 File Offset: 0x000173B8
		internal static bool TryParsePropertyAnnotation(string propertyAnnotationName, out string propertyName, out string annotationName)
		{
			int num = propertyAnnotationName.IndexOf('@');
			if (num <= 0 || num == propertyAnnotationName.Length - 1)
			{
				propertyName = null;
				annotationName = null;
				return false;
			}
			propertyName = propertyAnnotationName.Substring(0, num);
			annotationName = propertyAnnotationName.Substring(num + 1);
			return true;
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x000191FC File Offset: 0x000173FC
		internal void ReadPayloadStart(ODataPayloadKind payloadKind, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, bool isReadingNestedPayload, bool allowEmptyPayload)
		{
			string text = this.ReadPayloadStartImplementation(payloadKind, duplicatePropertyNamesChecker, isReadingNestedPayload, allowEmptyPayload);
			ODataJsonLightMetadataUriParseResult odataJsonLightMetadataUriParseResult = null;
			if (!isReadingNestedPayload && payloadKind != ODataPayloadKind.Error)
			{
				odataJsonLightMetadataUriParseResult = ((this.jsonLightInputContext.PayloadKindDetectionState == null) ? null : this.jsonLightInputContext.PayloadKindDetectionState.MetadataUriParseResult);
				if (odataJsonLightMetadataUriParseResult == null && text != null)
				{
					odataJsonLightMetadataUriParseResult = ODataJsonLightMetadataUriParser.Parse(base.Model, text, payloadKind, base.Version, base.MessageReaderSettings.ReaderBehavior);
				}
			}
			this.metadataUriParseResult = odataJsonLightMetadataUriParseResult;
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x00019340 File Offset: 0x00017540
		internal Task ReadPayloadStartAsync(ODataPayloadKind payloadKind, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, bool isReadingNestedPayload, bool allowEmptyPayload)
		{
			return TaskUtils.GetTaskForSynchronousOperation(delegate()
			{
				string text = this.ReadPayloadStartImplementation(payloadKind, duplicatePropertyNamesChecker, isReadingNestedPayload, allowEmptyPayload);
				if (!isReadingNestedPayload && payloadKind != ODataPayloadKind.Error)
				{
					this.metadataUriParseResult = ((this.jsonLightInputContext.PayloadKindDetectionState == null) ? null : this.jsonLightInputContext.PayloadKindDetectionState.MetadataUriParseResult);
					if (this.metadataUriParseResult == null && text != null)
					{
						this.metadataUriParseResult = ODataJsonLightMetadataUriParser.Parse(this.Model, text, payloadKind, this.Version, this.MessageReaderSettings.ReaderBehavior);
					}
				}
			});
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x00019388 File Offset: 0x00017588
		internal void ReadPayloadEnd(bool isReadingNestedPayload)
		{
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0001938C File Offset: 0x0001758C
		internal string ReadAndValidateAnnotationStringValue(string annotationName)
		{
			string text = this.JsonReader.ReadStringValue(annotationName);
			ODataJsonLightReaderUtils.ValidateAnnotationStringValue(text, annotationName);
			return text;
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x000193B0 File Offset: 0x000175B0
		internal Uri ReadAndValidateAnnotationStringValueAsUri(string annotationName)
		{
			string uriFromPayload = this.ReadAndValidateAnnotationStringValue(annotationName);
			return this.ProcessUriFromPayload(uriFromPayload);
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x000193CC File Offset: 0x000175CC
		internal long ReadAndValidateAnnotationStringValueAsLong(string annotationName)
		{
			string value = this.ReadAndValidateAnnotationStringValue(annotationName);
			return (long)ODataJsonLightReaderUtils.ConvertValue(value, EdmCoreModel.Instance.GetInt64(false), base.MessageReaderSettings, base.Version, true, annotationName);
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x00019408 File Offset: 0x00017608
		internal Uri ProcessUriFromPayload(string uriFromPayload)
		{
			Uri uri = new Uri(uriFromPayload, UriKind.RelativeOrAbsolute);
			Uri metadataDocumentUri = this.MetadataDocumentUri;
			Uri uri2 = this.JsonLightInputContext.ResolveUri(metadataDocumentUri, uri);
			if (uri2 != null)
			{
				return uri2;
			}
			if (!uri.IsAbsoluteUri)
			{
				if (metadataDocumentUri == null)
				{
					throw new ODataException(Strings.ODataJsonLightDeserializer_RelativeUriUsedWithouODataMetadataAnnotation(uriFromPayload, "odata.metadata"));
				}
				uri = UriUtils.UriToAbsoluteUri(metadataDocumentUri, uri);
			}
			return uri;
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x00019468 File Offset: 0x00017668
		internal void ProcessProperty(DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, Func<string, object> readPropertyAnnotationValue, Action<ODataJsonLightDeserializer.PropertyParsingResult, string> handleProperty)
		{
			string text;
			ODataJsonLightDeserializer.PropertyParsingResult propertyParsingResult = this.ParseProperty(duplicatePropertyNamesChecker, readPropertyAnnotationValue, out text);
			while (propertyParsingResult == ODataJsonLightDeserializer.PropertyParsingResult.CustomInstanceAnnotation && this.ShouldSkipCustomInstanceAnnotation(text))
			{
				duplicatePropertyNamesChecker.MarkPropertyAsProcessed(text);
				this.JsonReader.SkipValue();
				propertyParsingResult = this.ParseProperty(duplicatePropertyNamesChecker, readPropertyAnnotationValue, out text);
			}
			handleProperty(propertyParsingResult, text);
			if (propertyParsingResult != ODataJsonLightDeserializer.PropertyParsingResult.EndOfObject)
			{
				duplicatePropertyNamesChecker.MarkPropertyAsProcessed(text);
			}
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x000194BE File Offset: 0x000176BE
		[Conditional("DEBUG")]
		internal void AssertJsonCondition(params JsonNodeType[] allowedNodeTypes)
		{
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x000194C0 File Offset: 0x000176C0
		private bool ShouldSkipCustomInstanceAnnotation(string annotationName)
		{
			return (!(this is ODataJsonLightErrorDeserializer) || base.MessageReaderSettings.ShouldIncludeAnnotation != null) && base.MessageReaderSettings.ShouldSkipAnnotation(annotationName);
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x000194E5 File Offset: 0x000176E5
		private bool SkippedOverUnknownODataAnnotation(string annotationName)
		{
			if (ODataAnnotationNames.IsUnknownODataAnnotationName(annotationName))
			{
				this.JsonReader.Read();
				this.JsonReader.SkipValue();
				return true;
			}
			return false;
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0001950C File Offset: 0x0001770C
		private ODataJsonLightDeserializer.PropertyParsingResult ParseProperty(DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, Func<string, object> readPropertyAnnotationValue, out string parsedPropertyName)
		{
			string p = null;
			parsedPropertyName = null;
			while (this.JsonReader.NodeType == JsonNodeType.Property)
			{
				string propertyName = this.JsonReader.GetPropertyName();
				string text;
				string text2;
				bool flag = ODataJsonLightDeserializer.TryParsePropertyAnnotation(propertyName, out text, out text2);
				text = (text ?? propertyName);
				if (parsedPropertyName != null && string.CompareOrdinal(parsedPropertyName, text) != 0)
				{
					if (ODataJsonLightReaderUtils.IsAnnotationProperty(parsedPropertyName))
					{
						throw new ODataException(Strings.ODataJsonLightDeserializer_AnnotationTargetingInstanceAnnotationWithoutValue(p, parsedPropertyName));
					}
					return ODataJsonLightDeserializer.PropertyParsingResult.PropertyWithoutValue;
				}
				else if (flag)
				{
					if (ODataJsonLightReaderUtils.IsAnnotationProperty(text) || !this.SkippedOverUnknownODataAnnotation(text2))
					{
						parsedPropertyName = text;
						p = text2;
						this.ProcessPropertyAnnotation(text, text2, duplicatePropertyNamesChecker, readPropertyAnnotationValue);
					}
				}
				else if (!this.SkippedOverUnknownODataAnnotation(text))
				{
					this.JsonReader.Read();
					parsedPropertyName = text;
					if (ODataJsonLightUtils.IsMetadataReferenceProperty(text))
					{
						return ODataJsonLightDeserializer.PropertyParsingResult.MetadataReferenceProperty;
					}
					if (!ODataJsonLightReaderUtils.IsAnnotationProperty(text))
					{
						return ODataJsonLightDeserializer.PropertyParsingResult.PropertyWithValue;
					}
					if (ODataJsonLightReaderUtils.IsODataAnnotationName(text))
					{
						return ODataJsonLightDeserializer.PropertyParsingResult.ODataInstanceAnnotation;
					}
					return ODataJsonLightDeserializer.PropertyParsingResult.CustomInstanceAnnotation;
				}
			}
			if (parsedPropertyName == null)
			{
				return ODataJsonLightDeserializer.PropertyParsingResult.EndOfObject;
			}
			if (ODataJsonLightReaderUtils.IsAnnotationProperty(parsedPropertyName))
			{
				throw new ODataException(Strings.ODataJsonLightDeserializer_AnnotationTargetingInstanceAnnotationWithoutValue(p, parsedPropertyName));
			}
			return ODataJsonLightDeserializer.PropertyParsingResult.PropertyWithoutValue;
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x000195F8 File Offset: 0x000177F8
		private void ProcessPropertyAnnotation(string annotatedPropertyName, string annotationName, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, Func<string, object> readPropertyAnnotationValue)
		{
			if (ODataJsonLightReaderUtils.IsAnnotationProperty(annotatedPropertyName) && string.CompareOrdinal(annotationName, "odata.type") != 0)
			{
				throw new ODataException(Strings.ODataJsonLightDeserializer_OnlyODataTypeAnnotationCanTargetInstanceAnnotation(annotationName, annotatedPropertyName, "odata.type"));
			}
			this.JsonReader.Read();
			if (ODataJsonLightReaderUtils.IsODataAnnotationName(annotationName))
			{
				duplicatePropertyNamesChecker.AddODataPropertyAnnotation(annotatedPropertyName, annotationName, readPropertyAnnotationValue(annotationName));
				return;
			}
			duplicatePropertyNamesChecker.AddCustomPropertyAnnotation(annotatedPropertyName, annotationName);
			this.JsonReader.SkipValue();
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x00019664 File Offset: 0x00017864
		private string ReadPayloadStartImplementation(ODataPayloadKind payloadKind, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, bool isReadingNestedPayload, bool allowEmptyPayload)
		{
			if (!isReadingNestedPayload)
			{
				this.JsonReader.Read();
				if (allowEmptyPayload && this.JsonReader.NodeType == JsonNodeType.EndOfInput)
				{
					return null;
				}
				this.JsonReader.ReadStartObject();
				if (payloadKind != ODataPayloadKind.Error)
				{
					bool failOnMissingMetadataUriAnnotation = this.jsonLightInputContext.ReadingResponse && (this.jsonLightInputContext.PayloadKindDetectionState == null || this.jsonLightInputContext.PayloadKindDetectionState.MetadataUriParseResult == null);
					return this.ReadMetadataUriAnnotation(payloadKind, duplicatePropertyNamesChecker, failOnMissingMetadataUriAnnotation);
				}
			}
			return null;
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x000196E4 File Offset: 0x000178E4
		private string ReadMetadataUriAnnotation(ODataPayloadKind payloadKind, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, bool failOnMissingMetadataUriAnnotation)
		{
			if (this.JsonReader.NodeType != JsonNodeType.Property)
			{
				if (!failOnMissingMetadataUriAnnotation || payloadKind == ODataPayloadKind.Unsupported)
				{
					return null;
				}
				throw new ODataException(Strings.ODataJsonLightDeserializer_MetadataLinkNotFoundAsFirstProperty);
			}
			else
			{
				string propertyName = this.JsonReader.GetPropertyName();
				if (string.CompareOrdinal("odata.metadata", propertyName) == 0)
				{
					if (duplicatePropertyNamesChecker != null)
					{
						duplicatePropertyNamesChecker.MarkPropertyAsProcessed(propertyName);
					}
					this.JsonReader.ReadNext();
					return this.JsonReader.ReadStringValue();
				}
				if (!failOnMissingMetadataUriAnnotation || payloadKind == ODataPayloadKind.Unsupported)
				{
					return null;
				}
				throw new ODataException(Strings.ODataJsonLightDeserializer_MetadataLinkNotFoundAsFirstProperty);
			}
		}

		// Token: 0x040002C2 RID: 706
		private readonly ODataJsonLightInputContext jsonLightInputContext;

		// Token: 0x040002C3 RID: 707
		private IODataMetadataContext metadataContext;

		// Token: 0x040002C4 RID: 708
		private ODataJsonLightMetadataUriParseResult metadataUriParseResult;

		// Token: 0x02000113 RID: 275
		internal enum PropertyParsingResult
		{
			// Token: 0x040002C6 RID: 710
			EndOfObject,
			// Token: 0x040002C7 RID: 711
			PropertyWithValue,
			// Token: 0x040002C8 RID: 712
			PropertyWithoutValue,
			// Token: 0x040002C9 RID: 713
			ODataInstanceAnnotation,
			// Token: 0x040002CA RID: 714
			CustomInstanceAnnotation,
			// Token: 0x040002CB RID: 715
			MetadataReferenceProperty
		}
	}
}
