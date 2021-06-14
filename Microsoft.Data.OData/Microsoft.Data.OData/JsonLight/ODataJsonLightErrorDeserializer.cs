using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x0200018F RID: 399
	internal sealed class ODataJsonLightErrorDeserializer : ODataJsonLightDeserializer
	{
		// Token: 0x06000B75 RID: 2933 RVA: 0x00027FD0 File Offset: 0x000261D0
		internal ODataJsonLightErrorDeserializer(ODataJsonLightInputContext jsonLightInputContext) : base(jsonLightInputContext)
		{
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x00027FDC File Offset: 0x000261DC
		internal ODataError ReadTopLevelError()
		{
			base.JsonReader.DisableInStreamErrorDetection = true;
			DuplicatePropertyNamesChecker duplicatePropertyNamesChecker = base.CreateDuplicatePropertyNamesChecker();
			ODataError result;
			try
			{
				base.ReadPayloadStart(ODataPayloadKind.Error, duplicatePropertyNamesChecker, false, false);
				ODataError odataError = this.ReadTopLevelErrorImplementation();
				result = odataError;
			}
			finally
			{
				base.JsonReader.DisableInStreamErrorDetection = false;
			}
			return result;
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x00028054 File Offset: 0x00026254
		internal Task<ODataError> ReadTopLevelErrorAsync()
		{
			base.JsonReader.DisableInStreamErrorDetection = true;
			DuplicatePropertyNamesChecker duplicatePropertyNamesChecker = base.CreateDuplicatePropertyNamesChecker();
			return base.ReadPayloadStartAsync(ODataPayloadKind.Error, duplicatePropertyNamesChecker, false, false).FollowOnSuccessWith((Task t) => this.ReadTopLevelErrorImplementation()).FollowAlwaysWith(delegate(Task<ODataError> t)
			{
				base.JsonReader.DisableInStreamErrorDetection = false;
			});
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x000280A4 File Offset: 0x000262A4
		private ODataError ReadTopLevelErrorImplementation()
		{
			ODataError odataError = null;
			while (base.JsonReader.NodeType == JsonNodeType.Property)
			{
				string text = base.JsonReader.ReadPropertyName();
				if (string.CompareOrdinal("odata.error", text) != 0)
				{
					throw new ODataException(Strings.ODataJsonErrorDeserializer_TopLevelErrorWithInvalidProperty(text));
				}
				if (odataError != null)
				{
					throw new ODataException(Strings.ODataJsonReaderUtils_MultipleErrorPropertiesWithSameName("odata.error"));
				}
				odataError = new ODataError();
				this.ReadODataErrorObject(odataError);
			}
			base.JsonReader.ReadEndObject();
			base.ReadPayloadEnd(false);
			return odataError;
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x0002819C File Offset: 0x0002639C
		private void ReadJsonObjectInErrorPayload(Action<string, DuplicatePropertyNamesChecker> readPropertyWithValue)
		{
			DuplicatePropertyNamesChecker duplicatePropertyNamesChecker = base.CreateDuplicatePropertyNamesChecker();
			base.JsonReader.ReadStartObject();
			while (base.JsonReader.NodeType == JsonNodeType.Property)
			{
				base.ProcessProperty(duplicatePropertyNamesChecker, new Func<string, object>(this.ReadErrorPropertyAnnotationValue), delegate(ODataJsonLightDeserializer.PropertyParsingResult propertyParsingResult, string propertyName)
				{
					switch (propertyParsingResult)
					{
					case ODataJsonLightDeserializer.PropertyParsingResult.EndOfObject:
						return;
					case ODataJsonLightDeserializer.PropertyParsingResult.PropertyWithValue:
						readPropertyWithValue(propertyName, duplicatePropertyNamesChecker);
						return;
					case ODataJsonLightDeserializer.PropertyParsingResult.PropertyWithoutValue:
						throw new ODataException(Strings.ODataJsonLightErrorDeserializer_PropertyAnnotationWithoutPropertyForError(propertyName));
					case ODataJsonLightDeserializer.PropertyParsingResult.ODataInstanceAnnotation:
						throw new ODataException(Strings.ODataJsonLightErrorDeserializer_InstanceAnnotationNotAllowedInErrorPayload(propertyName));
					case ODataJsonLightDeserializer.PropertyParsingResult.CustomInstanceAnnotation:
						readPropertyWithValue(propertyName, duplicatePropertyNamesChecker);
						return;
					case ODataJsonLightDeserializer.PropertyParsingResult.MetadataReferenceProperty:
						throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_UnexpectedMetadataReferenceProperty(propertyName));
					default:
						return;
					}
				});
			}
			base.JsonReader.ReadEndObject();
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x00028214 File Offset: 0x00026414
		private object ReadErrorPropertyAnnotationValue(string propertyAnnotationName)
		{
			if (string.CompareOrdinal(propertyAnnotationName, "odata.type") != 0)
			{
				throw new ODataException(Strings.ODataJsonLightErrorDeserializer_PropertyAnnotationNotAllowedInErrorPayload(propertyAnnotationName));
			}
			string text = base.JsonReader.ReadStringValue();
			if (text == null)
			{
				throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_InvalidTypeName(propertyAnnotationName));
			}
			return text;
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x00028274 File Offset: 0x00026474
		private void ReadODataErrorObject(ODataError error)
		{
			this.ReadJsonObjectInErrorPayload(delegate(string propertyName, DuplicatePropertyNamesChecker duplicationPropertyNameChecker)
			{
				this.ReadPropertyValueInODataErrorObject(error, propertyName, duplicationPropertyNameChecker);
			});
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x000282C4 File Offset: 0x000264C4
		private void ReadErrorMessageObject(ODataError error)
		{
			this.ReadJsonObjectInErrorPayload(delegate(string propertyName, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker)
			{
				this.ReadPropertyValueInMessageObject(error, propertyName);
			});
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x0002831C File Offset: 0x0002651C
		private ODataInnerError ReadInnerError(int recursionDepth)
		{
			ValidationUtils.IncreaseAndValidateRecursionDepth(ref recursionDepth, base.MessageReaderSettings.MessageQuotas.MaxNestingDepth);
			ODataInnerError innerError = new ODataInnerError();
			this.ReadJsonObjectInErrorPayload(delegate(string propertyName, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker)
			{
				this.ReadPropertyValueInInnerError(recursionDepth, innerError, propertyName);
			});
			return innerError;
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x0002837C File Offset: 0x0002657C
		private void ReadPropertyValueInInnerError(int recursionDepth, ODataInnerError innerError, string propertyName)
		{
			if (propertyName != null)
			{
				if (propertyName == "message")
				{
					innerError.Message = base.JsonReader.ReadStringValue("message");
					return;
				}
				if (propertyName == "type")
				{
					innerError.TypeName = base.JsonReader.ReadStringValue("type");
					return;
				}
				if (propertyName == "stacktrace")
				{
					innerError.StackTrace = base.JsonReader.ReadStringValue("stacktrace");
					return;
				}
				if (propertyName == "internalexception")
				{
					innerError.InnerError = this.ReadInnerError(recursionDepth);
					return;
				}
			}
			base.JsonReader.SkipValue();
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x00028428 File Offset: 0x00026628
		private void ReadPropertyValueInODataErrorObject(ODataError error, string propertyName, DuplicatePropertyNamesChecker duplicationPropertyNameChecker)
		{
			if (propertyName != null)
			{
				if (propertyName == "code")
				{
					error.ErrorCode = base.JsonReader.ReadStringValue("code");
					return;
				}
				if (propertyName == "message")
				{
					this.ReadErrorMessageObject(error);
					return;
				}
				if (propertyName == "innererror")
				{
					error.InnerError = this.ReadInnerError(0);
					return;
				}
			}
			if (ODataJsonLightReaderUtils.IsAnnotationProperty(propertyName))
			{
				ODataJsonLightPropertyAndValueDeserializer odataJsonLightPropertyAndValueDeserializer = new ODataJsonLightPropertyAndValueDeserializer(base.JsonLightInputContext);
				object obj = null;
				Dictionary<string, object> odataPropertyAnnotations = duplicationPropertyNameChecker.GetODataPropertyAnnotations(propertyName);
				if (odataPropertyAnnotations != null)
				{
					odataPropertyAnnotations.TryGetValue("odata.type", out obj);
				}
				object instanceAnnotationValue = odataJsonLightPropertyAndValueDeserializer.ReadNonEntityValue(obj as string, null, null, null, false, false, false, propertyName, true);
				error.AddInstanceAnnotationForReading(propertyName, instanceAnnotationValue);
				return;
			}
			throw new ODataException(Strings.ODataJsonLightErrorDeserializer_TopLevelErrorValueWithInvalidProperty(propertyName));
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x000284EC File Offset: 0x000266EC
		private void ReadPropertyValueInMessageObject(ODataError error, string propertyName)
		{
			if (propertyName != null)
			{
				if (propertyName == "lang")
				{
					error.MessageLanguage = base.JsonReader.ReadStringValue("lang");
					return;
				}
				if (propertyName == "value")
				{
					error.Message = base.JsonReader.ReadStringValue("value");
					return;
				}
			}
			if (ODataJsonLightReaderUtils.IsAnnotationProperty(propertyName))
			{
				base.JsonReader.SkipValue();
				return;
			}
			throw new ODataException(Strings.ODataJsonErrorDeserializer_TopLevelErrorMessageValueWithInvalidProperty(propertyName));
		}
	}
}
