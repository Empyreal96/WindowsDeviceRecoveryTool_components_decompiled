using System;
using System.Collections.Generic;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x0200013E RID: 318
	internal sealed class JsonLightAnnotationGroupDeserializer : ODataJsonLightDeserializer
	{
		// Token: 0x06000882 RID: 2178 RVA: 0x0001B9CB File Offset: 0x00019BCB
		internal JsonLightAnnotationGroupDeserializer(ODataJsonLightInputContext inputContext) : base(inputContext)
		{
			this.annotationGroups = new Dictionary<string, ODataJsonLightAnnotationGroup>(EqualityComparer<string>.Default);
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x0001B9E4 File Offset: 0x00019BE4
		internal ODataJsonLightAnnotationGroup ReadAnnotationGroup(Func<string, object> readPropertyAnnotationValue, Func<string, DuplicatePropertyNamesChecker, object> readInstanceAnnotationValue)
		{
			string propertyName = base.JsonReader.GetPropertyName();
			if (string.CompareOrdinal(propertyName, "odata.annotationGroup") == 0)
			{
				base.JsonReader.ReadPropertyName();
				return this.ReadAnnotationGroupDeclaration(readPropertyAnnotationValue, readInstanceAnnotationValue);
			}
			if (string.CompareOrdinal(propertyName, "odata.annotationGroupReference") == 0)
			{
				base.JsonReader.ReadPropertyName();
				return this.ReadAnnotationGroupReference();
			}
			return null;
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x0001BA40 File Offset: 0x00019C40
		internal void AddAnnotationGroup(ODataJsonLightAnnotationGroup annotationGroup)
		{
			if (this.annotationGroups.ContainsKey(annotationGroup.Name))
			{
				throw new ODataException(Strings.JsonLightAnnotationGroupDeserializer_MultipleAnnotationGroupsWithSameName(annotationGroup.Name));
			}
			this.annotationGroups.Add(annotationGroup.Name, annotationGroup);
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0001BA78 File Offset: 0x00019C78
		private static void VerifyAnnotationGroupNameNotYetFound(ODataJsonLightAnnotationGroup annotationGroup)
		{
			if (!string.IsNullOrEmpty(annotationGroup.Name))
			{
				throw new ODataException(Strings.JsonLightAnnotationGroupDeserializer_EncounteredMultipleNameProperties);
			}
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x0001BA92 File Offset: 0x00019C92
		private static bool IsAnnotationGroupName(string propertyName)
		{
			return string.CompareOrdinal(propertyName, "name") == 0;
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x0001BAA2 File Offset: 0x00019CA2
		private static void VerifyAnnotationGroupNameFound(ODataJsonLightAnnotationGroup annotationGroup)
		{
			if (string.IsNullOrEmpty(annotationGroup.Name))
			{
				throw new ODataException(Strings.JsonLightAnnotationGroupDeserializer_AnnotationGroupDeclarationWithoutName);
			}
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0001BABC File Offset: 0x00019CBC
		private static void VerifyDataPropertyIsAnnotationName(string propertyName, ODataJsonLightAnnotationGroup annotationGroup)
		{
			if (!JsonLightAnnotationGroupDeserializer.IsAnnotationGroupName(propertyName))
			{
				throw JsonLightAnnotationGroupDeserializer.CreateExceptionForInvalidAnnotationInsideAnnotationGroup(propertyName, annotationGroup);
			}
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x0001BACE File Offset: 0x00019CCE
		private static ODataException CreateExceptionForInvalidAnnotationInsideAnnotationGroup(string propertyName, ODataJsonLightAnnotationGroup annotationGroup)
		{
			if (string.IsNullOrEmpty(annotationGroup.Name))
			{
				return new ODataException(Strings.JsonLightAnnotationGroupDeserializer_InvalidAnnotationFoundInsideAnnotationGroup(propertyName));
			}
			return new ODataException(Strings.JsonLightAnnotationGroupDeserializer_InvalidAnnotationFoundInsideNamedAnnotationGroup(annotationGroup.Name, propertyName));
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x0001BAFC File Offset: 0x00019CFC
		private ODataJsonLightAnnotationGroup ReadAnnotationGroupReference()
		{
			string text = base.JsonReader.ReadStringValue("odata.annotationGroupReference");
			ODataJsonLightAnnotationGroup result;
			if (this.annotationGroups.TryGetValue(text, out result))
			{
				return result;
			}
			throw new ODataException(Strings.JsonLightAnnotationGroupDeserializer_UndefinedAnnotationGroupReference(text));
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x0001BC74 File Offset: 0x00019E74
		private ODataJsonLightAnnotationGroup ReadAnnotationGroupDeclaration(Func<string, object> readPropertyAnnotationValue, Func<string, DuplicatePropertyNamesChecker, object> readInstanceAnnotationValue)
		{
			ODataJsonLightAnnotationGroup annotationGroup = new ODataJsonLightAnnotationGroup
			{
				Annotations = new Dictionary<string, object>(EqualityComparer<string>.Default)
			};
			base.JsonReader.ReadStartObject();
			DuplicatePropertyNamesChecker duplicatePropertyNamesChecker = base.CreateDuplicatePropertyNamesChecker();
			while (base.JsonReader.NodeType == JsonNodeType.Property)
			{
				base.ProcessProperty(duplicatePropertyNamesChecker, readPropertyAnnotationValue, delegate(ODataJsonLightDeserializer.PropertyParsingResult propertyParsingResult, string propertyName)
				{
					switch (propertyParsingResult)
					{
					case ODataJsonLightDeserializer.PropertyParsingResult.EndOfObject:
						return;
					case ODataJsonLightDeserializer.PropertyParsingResult.PropertyWithValue:
						JsonLightAnnotationGroupDeserializer.VerifyDataPropertyIsAnnotationName(propertyName, annotationGroup);
						JsonLightAnnotationGroupDeserializer.VerifyAnnotationGroupNameNotYetFound(annotationGroup);
						annotationGroup.Name = this.JsonReader.ReadStringValue(propertyName);
						return;
					case ODataJsonLightDeserializer.PropertyParsingResult.PropertyWithoutValue:
					{
						Dictionary<string, object> odataPropertyAnnotations = duplicatePropertyNamesChecker.GetODataPropertyAnnotations(propertyName);
						if (odataPropertyAnnotations == null)
						{
							return;
						}
						using (Dictionary<string, object>.Enumerator enumerator = odataPropertyAnnotations.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<string, object> keyValuePair = enumerator.Current;
								annotationGroup.Annotations.Add(propertyName + '@' + keyValuePair.Key, keyValuePair.Value);
							}
							return;
						}
						break;
					}
					case ODataJsonLightDeserializer.PropertyParsingResult.ODataInstanceAnnotation:
						break;
					case ODataJsonLightDeserializer.PropertyParsingResult.CustomInstanceAnnotation:
						this.JsonReader.SkipValue();
						return;
					case ODataJsonLightDeserializer.PropertyParsingResult.MetadataReferenceProperty:
						throw JsonLightAnnotationGroupDeserializer.CreateExceptionForInvalidAnnotationInsideAnnotationGroup(propertyName, annotationGroup);
					default:
						throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataJsonLightAnnotationGroupDeserializer_ReadAnnotationGroupDeclaration));
					}
					annotationGroup.Annotations.Add(propertyName, readInstanceAnnotationValue(propertyName, duplicatePropertyNamesChecker));
				});
			}
			JsonLightAnnotationGroupDeserializer.VerifyAnnotationGroupNameFound(annotationGroup);
			base.JsonReader.ReadEndObject();
			this.AddAnnotationGroup(annotationGroup);
			return annotationGroup;
		}

		// Token: 0x04000349 RID: 841
		private readonly Dictionary<string, ODataJsonLightAnnotationGroup> annotationGroups;
	}
}
