using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Annotations;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.Edm.Library.Annotations;
using Microsoft.Data.Edm.Library.Values;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.OData.Metadata
{
	// Token: 0x0200020D RID: 525
	internal static class EpmExtensionMethods
	{
		// Token: 0x06001013 RID: 4115 RVA: 0x0003A6B0 File Offset: 0x000388B0
		internal static ODataEntityPropertyMappingCache EnsureEpmCache(this IEdmModel model, IEdmEntityType entityType, int maxMappingCount)
		{
			bool flag;
			return EpmExtensionMethods.EnsureEpmCacheInternal(model, entityType, maxMappingCount, out flag);
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x0003A6C8 File Offset: 0x000388C8
		internal static bool HasEntityPropertyMappings(this IEdmModel model, IEdmEntityType entityType)
		{
			for (IEdmEntityType edmEntityType = entityType; edmEntityType != null; edmEntityType = edmEntityType.BaseEntityType())
			{
				if (model.GetEntityPropertyMappings(edmEntityType) != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x0003A6EF File Offset: 0x000388EF
		internal static ODataEntityPropertyMappingCollection GetEntityPropertyMappings(this IEdmModel model, IEdmEntityType entityType)
		{
			return model.GetAnnotationValue(entityType);
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x0003A6F8 File Offset: 0x000388F8
		internal static ODataEntityPropertyMappingCache GetEpmCache(this IEdmModel model, IEdmEntityType entityType)
		{
			return model.GetAnnotationValue(entityType);
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x0003A704 File Offset: 0x00038904
		internal static Dictionary<string, IEdmDirectValueAnnotationBinding> GetAnnotationBindingsToRemoveSerializableEpmAnnotations(this IEdmModel model, IEdmElement annotatable)
		{
			Dictionary<string, IEdmDirectValueAnnotationBinding> dictionary = new Dictionary<string, IEdmDirectValueAnnotationBinding>(StringComparer.Ordinal);
			IEnumerable<IEdmDirectValueAnnotation> odataAnnotations = model.GetODataAnnotations(annotatable);
			if (odataAnnotations != null)
			{
				foreach (IEdmDirectValueAnnotation edmDirectValueAnnotation in odataAnnotations)
				{
					if (edmDirectValueAnnotation.IsEpmAnnotation())
					{
						dictionary.Add(edmDirectValueAnnotation.Name, new EdmDirectValueAnnotationBinding(annotatable, edmDirectValueAnnotation.NamespaceUri, edmDirectValueAnnotation.Name, null));
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x0003A784 File Offset: 0x00038984
		internal static void ClearInMemoryEpmAnnotations(this IEdmModel model, IEdmElement annotatable)
		{
			model.SetAnnotationValues(new IEdmDirectValueAnnotationBinding[]
			{
				new EdmTypedDirectValueAnnotationBinding<ODataEntityPropertyMappingCollection>(annotatable, null),
				new EdmTypedDirectValueAnnotationBinding<ODataEntityPropertyMappingCache>(annotatable, null)
			});
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x0003A828 File Offset: 0x00038A28
		internal static void SaveEpmAnnotationsForProperty(this IEdmModel model, IEdmProperty property, ODataEntityPropertyMappingCache epmCache)
		{
			string propertyName = property.Name;
			IEnumerable<EntityPropertyMappingAttribute> enumerable = from m in epmCache.MappingsForDeclaredProperties
			where m.SourcePath.StartsWith(propertyName, StringComparison.Ordinal) && (m.SourcePath.Length == propertyName.Length || m.SourcePath[propertyName.Length] == '/')
			select m;
			bool skipSourcePath;
			bool removePrefix;
			if (property.Type.IsODataPrimitiveTypeKind())
			{
				skipSourcePath = true;
				removePrefix = false;
			}
			else
			{
				removePrefix = true;
				skipSourcePath = enumerable.Any((EntityPropertyMappingAttribute m) => m.SourcePath == propertyName);
			}
			model.SaveEpmAnnotations(property, enumerable, skipSourcePath, removePrefix);
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0003A89C File Offset: 0x00038A9C
		internal static void SaveEpmAnnotations(this IEdmModel model, IEdmElement annotatable, IEnumerable<EntityPropertyMappingAttribute> mappings, bool skipSourcePath, bool removePrefix)
		{
			EpmAttributeNameBuilder epmAttributeNameBuilder = new EpmAttributeNameBuilder();
			Dictionary<string, IEdmDirectValueAnnotationBinding> annotationBindingsToRemoveSerializableEpmAnnotations = model.GetAnnotationBindingsToRemoveSerializableEpmAnnotations(annotatable);
			foreach (EntityPropertyMappingAttribute entityPropertyMappingAttribute in mappings)
			{
				string text;
				if (entityPropertyMappingAttribute.TargetSyndicationItem == SyndicationItemProperty.CustomProperty)
				{
					text = epmAttributeNameBuilder.EpmTargetPath;
					annotationBindingsToRemoveSerializableEpmAnnotations[text] = EpmExtensionMethods.GetODataAnnotationBinding(annotatable, text, entityPropertyMappingAttribute.TargetPath);
					text = epmAttributeNameBuilder.EpmNsUri;
					annotationBindingsToRemoveSerializableEpmAnnotations[text] = EpmExtensionMethods.GetODataAnnotationBinding(annotatable, text, entityPropertyMappingAttribute.TargetNamespaceUri);
					string targetNamespacePrefix = entityPropertyMappingAttribute.TargetNamespacePrefix;
					if (!string.IsNullOrEmpty(targetNamespacePrefix))
					{
						text = epmAttributeNameBuilder.EpmNsPrefix;
						annotationBindingsToRemoveSerializableEpmAnnotations[text] = EpmExtensionMethods.GetODataAnnotationBinding(annotatable, text, targetNamespacePrefix);
					}
				}
				else
				{
					text = epmAttributeNameBuilder.EpmTargetPath;
					annotationBindingsToRemoveSerializableEpmAnnotations[text] = EpmExtensionMethods.GetODataAnnotationBinding(annotatable, text, entityPropertyMappingAttribute.TargetSyndicationItem.ToAttributeValue());
					text = epmAttributeNameBuilder.EpmContentKind;
					annotationBindingsToRemoveSerializableEpmAnnotations[text] = EpmExtensionMethods.GetODataAnnotationBinding(annotatable, text, entityPropertyMappingAttribute.TargetTextContentKind.ToAttributeValue());
				}
				if (!skipSourcePath)
				{
					string text2 = entityPropertyMappingAttribute.SourcePath;
					if (removePrefix)
					{
						text2 = text2.Substring(text2.IndexOf('/') + 1);
					}
					text = epmAttributeNameBuilder.EpmSourcePath;
					annotationBindingsToRemoveSerializableEpmAnnotations[text] = EpmExtensionMethods.GetODataAnnotationBinding(annotatable, text, text2);
				}
				string value = entityPropertyMappingAttribute.KeepInContent ? "true" : "false";
				text = epmAttributeNameBuilder.EpmKeepInContent;
				annotationBindingsToRemoveSerializableEpmAnnotations[text] = EpmExtensionMethods.GetODataAnnotationBinding(annotatable, text, value);
				epmAttributeNameBuilder.MoveNext();
			}
			model.SetAnnotationValues(annotationBindingsToRemoveSerializableEpmAnnotations.Values);
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x0003AA24 File Offset: 0x00038C24
		internal static CachedPrimitiveKeepInContentAnnotation EpmCachedKeepPrimitiveInContent(this IEdmModel model, IEdmComplexType complexType)
		{
			return model.GetAnnotationValue(complexType);
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x0003AA30 File Offset: 0x00038C30
		internal static string ToTargetPath(this SyndicationItemProperty targetSyndicationItem)
		{
			switch (targetSyndicationItem)
			{
			case SyndicationItemProperty.AuthorEmail:
				return "author/email";
			case SyndicationItemProperty.AuthorName:
				return "author/name";
			case SyndicationItemProperty.AuthorUri:
				return "author/uri";
			case SyndicationItemProperty.ContributorEmail:
				return "contributor/email";
			case SyndicationItemProperty.ContributorName:
				return "contributor/name";
			case SyndicationItemProperty.ContributorUri:
				return "contributor/uri";
			case SyndicationItemProperty.Updated:
				return "updated";
			case SyndicationItemProperty.Published:
				return "published";
			case SyndicationItemProperty.Rights:
				return "rights";
			case SyndicationItemProperty.Summary:
				return "summary";
			case SyndicationItemProperty.Title:
				return "title";
			default:
				throw new ArgumentException(Strings.EntityPropertyMapping_EpmAttribute("targetSyndicationItem"));
			}
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x0003AAC8 File Offset: 0x00038CC8
		private static void LoadEpmAnnotations(IEdmModel model, IEdmEntityType entityType)
		{
			string typeName = entityType.ODataFullName();
			ODataEntityPropertyMappingCollection odataEntityPropertyMappingCollection = new ODataEntityPropertyMappingCollection();
			model.LoadEpmAnnotations(entityType, odataEntityPropertyMappingCollection, typeName, null);
			IEnumerable<IEdmProperty> declaredProperties = entityType.DeclaredProperties;
			if (declaredProperties != null)
			{
				foreach (IEdmProperty edmProperty in declaredProperties)
				{
					model.LoadEpmAnnotations(edmProperty, odataEntityPropertyMappingCollection, typeName, edmProperty);
				}
			}
			model.SetAnnotationValue(entityType, odataEntityPropertyMappingCollection);
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x0003AB44 File Offset: 0x00038D44
		private static void LoadEpmAnnotations(this IEdmModel model, IEdmElement annotatable, ODataEntityPropertyMappingCollection mappings, string typeName, IEdmProperty property)
		{
			IEnumerable<EpmExtensionMethods.EpmAnnotationValues> enumerable = model.ParseSerializableEpmAnnotations(annotatable, typeName, property);
			if (enumerable != null)
			{
				foreach (EpmExtensionMethods.EpmAnnotationValues annotationValues in enumerable)
				{
					EntityPropertyMappingAttribute mapping = EpmExtensionMethods.ValidateAnnotationValues(annotationValues, typeName, property);
					mappings.Add(mapping);
				}
			}
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x0003ABA4 File Offset: 0x00038DA4
		private static SyndicationItemProperty MapTargetPathToSyndicationProperty(string targetPath)
		{
			SyndicationItemProperty result;
			if (!EpmExtensionMethods.TargetPathToSyndicationItemMap.TryGetValue(targetPath, out result))
			{
				return SyndicationItemProperty.CustomProperty;
			}
			return result;
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x0003ABC4 File Offset: 0x00038DC4
		private static string ToAttributeValue(this SyndicationTextContentKind contentKind)
		{
			switch (contentKind)
			{
			case SyndicationTextContentKind.Html:
				return "html";
			case SyndicationTextContentKind.Xhtml:
				return "xhtml";
			default:
				return "text";
			}
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x0003ABF8 File Offset: 0x00038DF8
		private static string ToAttributeValue(this SyndicationItemProperty syndicationItemProperty)
		{
			switch (syndicationItemProperty)
			{
			case SyndicationItemProperty.AuthorEmail:
				return "SyndicationAuthorEmail";
			case SyndicationItemProperty.AuthorName:
				return "SyndicationAuthorName";
			case SyndicationItemProperty.AuthorUri:
				return "SyndicationAuthorUri";
			case SyndicationItemProperty.ContributorEmail:
				return "SyndicationContributorEmail";
			case SyndicationItemProperty.ContributorName:
				return "SyndicationContributorName";
			case SyndicationItemProperty.ContributorUri:
				return "SyndicationContributorUri";
			case SyndicationItemProperty.Updated:
				return "SyndicationUpdated";
			case SyndicationItemProperty.Published:
				return "SyndicationPublished";
			case SyndicationItemProperty.Rights:
				return "SyndicationRights";
			case SyndicationItemProperty.Summary:
				return "SyndicationSummary";
			case SyndicationItemProperty.Title:
				return "SyndicationTitle";
			}
			throw new ODataException(Strings.General_InternalError(InternalErrorCodes.EpmExtensionMethods_ToAttributeValue_SyndicationItemProperty));
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0003AC94 File Offset: 0x00038E94
		private static SyndicationTextContentKind MapContentKindToSyndicationTextContentKind(string contentKind, string attributeSuffix, string typeName, string propertyName)
		{
			if (contentKind != null)
			{
				if (contentKind == "text")
				{
					return SyndicationTextContentKind.Plaintext;
				}
				if (contentKind == "html")
				{
					return SyndicationTextContentKind.Html;
				}
				if (contentKind == "xhtml")
				{
					return SyndicationTextContentKind.Xhtml;
				}
			}
			string message = (propertyName == null) ? Strings.EpmExtensionMethods_InvalidTargetTextContentKindOnType("FC_ContentKind" + attributeSuffix, typeName) : Strings.EpmExtensionMethods_InvalidTargetTextContentKindOnProperty("FC_ContentKind" + attributeSuffix, propertyName, typeName);
			throw new ODataException(message);
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x0003AD04 File Offset: 0x00038F04
		private static IEnumerable<EpmExtensionMethods.EpmAnnotationValues> ParseSerializableEpmAnnotations(this IEdmModel model, IEdmElement annotatable, string typeName, IEdmProperty property)
		{
			Dictionary<string, EpmExtensionMethods.EpmAnnotationValues> dictionary = null;
			IEnumerable<IEdmDirectValueAnnotation> odataAnnotations = model.GetODataAnnotations(annotatable);
			if (odataAnnotations != null)
			{
				foreach (IEdmDirectValueAnnotation annotation in odataAnnotations)
				{
					string second;
					string text;
					if (annotation.IsEpmAnnotation(out second, out text))
					{
						string text2 = EpmExtensionMethods.ConvertEdmAnnotationValue(annotation);
						if (dictionary == null)
						{
							dictionary = new Dictionary<string, EpmExtensionMethods.EpmAnnotationValues>(StringComparer.Ordinal);
						}
						EpmExtensionMethods.EpmAnnotationValues epmAnnotationValues;
						if (!dictionary.TryGetValue(text, out epmAnnotationValues))
						{
							epmAnnotationValues = new EpmExtensionMethods.EpmAnnotationValues
							{
								AttributeSuffix = text
							};
							dictionary[text] = epmAnnotationValues;
						}
						if (EpmExtensionMethods.NamesMatchByReference("FC_TargetPath", second))
						{
							epmAnnotationValues.TargetPath = text2;
						}
						else if (EpmExtensionMethods.NamesMatchByReference("FC_SourcePath", second))
						{
							epmAnnotationValues.SourcePath = text2;
						}
						else if (EpmExtensionMethods.NamesMatchByReference("FC_KeepInContent", second))
						{
							epmAnnotationValues.KeepInContent = text2;
						}
						else if (EpmExtensionMethods.NamesMatchByReference("FC_ContentKind", second))
						{
							epmAnnotationValues.ContentKind = text2;
						}
						else if (EpmExtensionMethods.NamesMatchByReference("FC_NsUri", second))
						{
							epmAnnotationValues.NamespaceUri = text2;
						}
						else
						{
							if (!EpmExtensionMethods.NamesMatchByReference("FC_NsPrefix", second))
							{
								throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataUtils_ParseSerializableEpmAnnotations_UnreachableCodePath));
							}
							epmAnnotationValues.NamespacePrefix = text2;
						}
					}
				}
				if (dictionary != null)
				{
					foreach (EpmExtensionMethods.EpmAnnotationValues epmAnnotationValues2 in dictionary.Values)
					{
						string sourcePath = epmAnnotationValues2.SourcePath;
						if (sourcePath == null)
						{
							if (property == null)
							{
								string p = "FC_SourcePath" + epmAnnotationValues2.AttributeSuffix;
								throw new ODataException(Strings.EpmExtensionMethods_MissingAttributeOnType(p, typeName));
							}
							epmAnnotationValues2.SourcePath = property.Name;
						}
						else if (property != null && !property.Type.IsODataPrimitiveTypeKind())
						{
							epmAnnotationValues2.SourcePath = property.Name + "/" + sourcePath;
						}
					}
				}
			}
			if (dictionary != null)
			{
				return dictionary.Values;
			}
			return null;
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x0003AF28 File Offset: 0x00039128
		private static EntityPropertyMappingAttribute ValidateAnnotationValues(EpmExtensionMethods.EpmAnnotationValues annotationValues, string typeName, IEdmProperty property)
		{
			if (annotationValues.TargetPath == null)
			{
				string p = "FC_TargetPath" + annotationValues.AttributeSuffix;
				string message = (property == null) ? Strings.EpmExtensionMethods_MissingAttributeOnType(p, typeName) : Strings.EpmExtensionMethods_MissingAttributeOnProperty(p, property.Name, typeName);
				throw new ODataException(message);
			}
			bool keepInContent = true;
			if (annotationValues.KeepInContent != null && !bool.TryParse(annotationValues.KeepInContent, out keepInContent))
			{
				string p2 = "FC_KeepInContent" + annotationValues.AttributeSuffix;
				throw new InvalidOperationException((property == null) ? Strings.EpmExtensionMethods_InvalidKeepInContentOnType(p2, typeName) : Strings.EpmExtensionMethods_InvalidKeepInContentOnProperty(p2, property.Name, typeName));
			}
			SyndicationItemProperty syndicationItemProperty = EpmExtensionMethods.MapTargetPathToSyndicationProperty(annotationValues.TargetPath);
			EntityPropertyMappingAttribute result;
			if (syndicationItemProperty == SyndicationItemProperty.CustomProperty)
			{
				if (annotationValues.ContentKind != null)
				{
					string p3 = "FC_ContentKind" + annotationValues.AttributeSuffix;
					string message2 = (property == null) ? Strings.EpmExtensionMethods_AttributeNotAllowedForCustomMappingOnType(p3, typeName) : Strings.EpmExtensionMethods_AttributeNotAllowedForCustomMappingOnProperty(p3, property.Name, typeName);
					throw new ODataException(message2);
				}
				result = new EntityPropertyMappingAttribute(annotationValues.SourcePath, annotationValues.TargetPath, annotationValues.NamespacePrefix, annotationValues.NamespaceUri, keepInContent);
			}
			else
			{
				if (annotationValues.NamespaceUri != null)
				{
					string p4 = "FC_NsUri" + annotationValues.AttributeSuffix;
					string message3 = (property == null) ? Strings.EpmExtensionMethods_AttributeNotAllowedForAtomPubMappingOnType(p4, typeName) : Strings.EpmExtensionMethods_AttributeNotAllowedForAtomPubMappingOnProperty(p4, property.Name, typeName);
					throw new ODataException(message3);
				}
				if (annotationValues.NamespacePrefix != null)
				{
					string p5 = "FC_NsPrefix" + annotationValues.AttributeSuffix;
					string message4 = (property == null) ? Strings.EpmExtensionMethods_AttributeNotAllowedForAtomPubMappingOnType(p5, typeName) : Strings.EpmExtensionMethods_AttributeNotAllowedForAtomPubMappingOnProperty(p5, property.Name, typeName);
					throw new ODataException(message4);
				}
				SyndicationTextContentKind targetTextContentKind = SyndicationTextContentKind.Plaintext;
				if (annotationValues.ContentKind != null)
				{
					targetTextContentKind = EpmExtensionMethods.MapContentKindToSyndicationTextContentKind(annotationValues.ContentKind, annotationValues.AttributeSuffix, typeName, (property == null) ? null : property.Name);
				}
				result = new EntityPropertyMappingAttribute(annotationValues.SourcePath, syndicationItemProperty, targetTextContentKind, keepInContent);
			}
			return result;
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x0003B0EA File Offset: 0x000392EA
		private static void RemoveEpmCache(this IEdmModel model, IEdmEntityType entityType)
		{
			model.SetAnnotationValue(entityType, null);
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x0003B0F4 File Offset: 0x000392F4
		private static bool IsEpmAnnotation(this IEdmDirectValueAnnotation annotation)
		{
			string text;
			string text2;
			return annotation.IsEpmAnnotation(out text, out text2);
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x0003B10C File Offset: 0x0003930C
		private static bool IsEpmAnnotation(this IEdmDirectValueAnnotation annotation, out string baseName, out string suffix)
		{
			string name = annotation.Name;
			for (int i = 0; i < EpmExtensionMethods.EpmAnnotationBaseNames.Length; i++)
			{
				string text = EpmExtensionMethods.EpmAnnotationBaseNames[i];
				if (name.StartsWith(text, StringComparison.Ordinal))
				{
					baseName = text;
					suffix = name.Substring(text.Length);
					return true;
				}
			}
			baseName = null;
			suffix = null;
			return false;
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x0003B160 File Offset: 0x00039360
		private static string ConvertEdmAnnotationValue(IEdmDirectValueAnnotation annotation)
		{
			object value = annotation.Value;
			if (value == null)
			{
				return null;
			}
			IEdmStringValue edmStringValue = value as IEdmStringValue;
			if (edmStringValue != null)
			{
				return edmStringValue.Value;
			}
			throw new ODataException(Strings.EpmExtensionMethods_CannotConvertEdmAnnotationValue(annotation.NamespaceUri, annotation.Name, annotation.GetType().FullName));
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x0003B1AB File Offset: 0x000393AB
		private static bool NamesMatchByReference(string first, string second)
		{
			return object.ReferenceEquals(first, second);
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x0003B1B4 File Offset: 0x000393B4
		private static bool HasOwnOrInheritedEpm(this IEdmModel model, IEdmEntityType entityType)
		{
			if (entityType == null)
			{
				return false;
			}
			if (model.GetAnnotationValue(entityType) != null)
			{
				return true;
			}
			EpmExtensionMethods.LoadEpmAnnotations(model, entityType);
			return model.GetAnnotationValue(entityType) != null || model.HasOwnOrInheritedEpm(entityType.BaseEntityType());
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x0003B1E4 File Offset: 0x000393E4
		private static IEdmDirectValueAnnotationBinding GetODataAnnotationBinding(IEdmElement annotatable, string localName, string value)
		{
			IEdmStringValue value2 = null;
			if (value != null)
			{
				IEdmStringTypeReference @string = EdmCoreModel.Instance.GetString(true);
				value2 = new EdmStringConstant(@string, value);
			}
			return new EdmDirectValueAnnotationBinding(annotatable, "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata", localName, value2);
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x0003B218 File Offset: 0x00039418
		private static ODataEntityPropertyMappingCache EnsureEpmCacheInternal(IEdmModel model, IEdmEntityType entityType, int maxMappingCount, out bool cacheModified)
		{
			cacheModified = false;
			if (entityType == null)
			{
				return null;
			}
			IEdmEntityType edmEntityType = entityType.BaseEntityType();
			ODataEntityPropertyMappingCache baseCache = null;
			if (edmEntityType != null)
			{
				baseCache = EpmExtensionMethods.EnsureEpmCacheInternal(model, edmEntityType, maxMappingCount, out cacheModified);
			}
			ODataEntityPropertyMappingCache odataEntityPropertyMappingCache = model.GetEpmCache(entityType);
			if (model.HasOwnOrInheritedEpm(entityType))
			{
				ODataEntityPropertyMappingCollection entityPropertyMappings = model.GetEntityPropertyMappings(entityType);
				bool flag = odataEntityPropertyMappingCache == null || cacheModified || odataEntityPropertyMappingCache.IsDirty(entityPropertyMappings);
				if (!flag)
				{
					return odataEntityPropertyMappingCache;
				}
				cacheModified = true;
				int totalMappingCount = ValidationUtils.ValidateTotalEntityPropertyMappingCount(baseCache, entityPropertyMappings, maxMappingCount);
				odataEntityPropertyMappingCache = new ODataEntityPropertyMappingCache(entityPropertyMappings, model, totalMappingCount);
				try
				{
					odataEntityPropertyMappingCache.BuildEpmForType(entityType, entityType);
					odataEntityPropertyMappingCache.EpmSourceTree.Validate(entityType);
					model.SetAnnotationValue(entityType, odataEntityPropertyMappingCache);
					return odataEntityPropertyMappingCache;
				}
				catch
				{
					model.RemoveEpmCache(entityType);
					throw;
				}
			}
			if (odataEntityPropertyMappingCache != null)
			{
				cacheModified = true;
				model.RemoveEpmCache(entityType);
			}
			return odataEntityPropertyMappingCache;
		}

		// Token: 0x040005E9 RID: 1513
		private static readonly string[] EpmAnnotationBaseNames = new string[]
		{
			"FC_TargetPath",
			"FC_SourcePath",
			"FC_KeepInContent",
			"FC_ContentKind",
			"FC_NsUri",
			"FC_NsPrefix"
		};

		// Token: 0x040005EA RID: 1514
		private static readonly Dictionary<string, SyndicationItemProperty> TargetPathToSyndicationItemMap = new Dictionary<string, SyndicationItemProperty>(StringComparer.OrdinalIgnoreCase)
		{
			{
				"SyndicationAuthorEmail",
				SyndicationItemProperty.AuthorEmail
			},
			{
				"SyndicationAuthorName",
				SyndicationItemProperty.AuthorName
			},
			{
				"SyndicationAuthorUri",
				SyndicationItemProperty.AuthorUri
			},
			{
				"SyndicationContributorEmail",
				SyndicationItemProperty.ContributorEmail
			},
			{
				"SyndicationContributorName",
				SyndicationItemProperty.ContributorName
			},
			{
				"SyndicationContributorUri",
				SyndicationItemProperty.ContributorUri
			},
			{
				"SyndicationUpdated",
				SyndicationItemProperty.Updated
			},
			{
				"SyndicationPublished",
				SyndicationItemProperty.Published
			},
			{
				"SyndicationRights",
				SyndicationItemProperty.Rights
			},
			{
				"SyndicationSummary",
				SyndicationItemProperty.Summary
			},
			{
				"SyndicationTitle",
				SyndicationItemProperty.Title
			}
		};

		// Token: 0x0200020E RID: 526
		private sealed class EpmAnnotationValues
		{
			// Token: 0x1700035F RID: 863
			// (get) Token: 0x0600102E RID: 4142 RVA: 0x0003B3B2 File Offset: 0x000395B2
			// (set) Token: 0x0600102F RID: 4143 RVA: 0x0003B3BA File Offset: 0x000395BA
			internal string SourcePath { get; set; }

			// Token: 0x17000360 RID: 864
			// (get) Token: 0x06001030 RID: 4144 RVA: 0x0003B3C3 File Offset: 0x000395C3
			// (set) Token: 0x06001031 RID: 4145 RVA: 0x0003B3CB File Offset: 0x000395CB
			internal string TargetPath { get; set; }

			// Token: 0x17000361 RID: 865
			// (get) Token: 0x06001032 RID: 4146 RVA: 0x0003B3D4 File Offset: 0x000395D4
			// (set) Token: 0x06001033 RID: 4147 RVA: 0x0003B3DC File Offset: 0x000395DC
			internal string KeepInContent { get; set; }

			// Token: 0x17000362 RID: 866
			// (get) Token: 0x06001034 RID: 4148 RVA: 0x0003B3E5 File Offset: 0x000395E5
			// (set) Token: 0x06001035 RID: 4149 RVA: 0x0003B3ED File Offset: 0x000395ED
			internal string ContentKind { get; set; }

			// Token: 0x17000363 RID: 867
			// (get) Token: 0x06001036 RID: 4150 RVA: 0x0003B3F6 File Offset: 0x000395F6
			// (set) Token: 0x06001037 RID: 4151 RVA: 0x0003B3FE File Offset: 0x000395FE
			internal string NamespaceUri { get; set; }

			// Token: 0x17000364 RID: 868
			// (get) Token: 0x06001038 RID: 4152 RVA: 0x0003B407 File Offset: 0x00039607
			// (set) Token: 0x06001039 RID: 4153 RVA: 0x0003B40F File Offset: 0x0003960F
			internal string NamespacePrefix { get; set; }

			// Token: 0x17000365 RID: 869
			// (get) Token: 0x0600103A RID: 4154 RVA: 0x0003B418 File Offset: 0x00039618
			// (set) Token: 0x0600103B RID: 4155 RVA: 0x0003B420 File Offset: 0x00039620
			internal string AttributeSuffix { get; set; }
		}
	}
}
