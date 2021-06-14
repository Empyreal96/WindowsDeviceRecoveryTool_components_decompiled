using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Annotations;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Validation;
using Microsoft.Data.Edm.Validation.Internal;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.Edm.Csdl
{
	// Token: 0x020000AA RID: 170
	public static class SerializationExtensionMethods
	{
		// Token: 0x060002CE RID: 718 RVA: 0x0000720E File Offset: 0x0000540E
		public static Version GetEdmxVersion(this IEdmModel model)
		{
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			return model.GetAnnotationValue(model, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "EdmxVersion");
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000722D File Offset: 0x0000542D
		public static void SetEdmxVersion(this IEdmModel model, Version version)
		{
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			model.SetAnnotationValue(model, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "EdmxVersion", version);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000724D File Offset: 0x0000544D
		public static void SetNamespacePrefixMappings(this IEdmModel model, IEnumerable<KeyValuePair<string, string>> mappings)
		{
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			model.SetAnnotationValue(model, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "NamespacePrefix", mappings);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000726D File Offset: 0x0000546D
		public static IEnumerable<KeyValuePair<string, string>> GetNamespacePrefixMappings(this IEdmModel model)
		{
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			return model.GetAnnotationValue(model, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "NamespacePrefix");
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000728C File Offset: 0x0000548C
		public static void SetDataServiceVersion(this IEdmModel model, Version version)
		{
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			model.SetAnnotationValue(model, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "DataServiceVersion", version);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x000072AC File Offset: 0x000054AC
		public static Version GetDataServiceVersion(this IEdmModel model)
		{
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			return model.GetAnnotationValue(model, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "DataServiceVersion");
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x000072CB File Offset: 0x000054CB
		public static void SetMaxDataServiceVersion(this IEdmModel model, Version version)
		{
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			model.SetAnnotationValue(model, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "MaxDataServiceVersion", version);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x000072EB File Offset: 0x000054EB
		public static Version GetMaxDataServiceVersion(this IEdmModel model)
		{
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			return model.GetAnnotationValue(model, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "MaxDataServiceVersion");
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000730A File Offset: 0x0000550A
		public static void SetSerializationLocation(this IEdmVocabularyAnnotation annotation, IEdmModel model, EdmVocabularyAnnotationSerializationLocation? location)
		{
			EdmUtil.CheckArgumentNull<IEdmVocabularyAnnotation>(annotation, "annotation");
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			model.SetAnnotationValue(annotation, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "AnnotationSerializationLocation", location);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000733B File Offset: 0x0000553B
		public static EdmVocabularyAnnotationSerializationLocation? GetSerializationLocation(this IEdmVocabularyAnnotation annotation, IEdmModel model)
		{
			EdmUtil.CheckArgumentNull<IEdmVocabularyAnnotation>(annotation, "annotation");
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			return model.GetAnnotationValue(annotation, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "AnnotationSerializationLocation") as EdmVocabularyAnnotationSerializationLocation?;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00007370 File Offset: 0x00005570
		public static void SetSchemaNamespace(this IEdmVocabularyAnnotation annotation, IEdmModel model, string schemaNamespace)
		{
			EdmUtil.CheckArgumentNull<IEdmVocabularyAnnotation>(annotation, "annotation");
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			model.SetAnnotationValue(annotation, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "SchemaNamespace", schemaNamespace);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000739C File Offset: 0x0000559C
		public static string GetSchemaNamespace(this IEdmVocabularyAnnotation annotation, IEdmModel model)
		{
			EdmUtil.CheckArgumentNull<IEdmVocabularyAnnotation>(annotation, "annotation");
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			return model.GetAnnotationValue(annotation, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "SchemaNamespace");
		}

		// Token: 0x060002DA RID: 730 RVA: 0x000073C7 File Offset: 0x000055C7
		public static void SetAssociationName(this IEdmModel model, IEdmNavigationProperty property, string associationName)
		{
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			EdmUtil.CheckArgumentNull<IEdmNavigationProperty>(property, "property");
			EdmUtil.CheckArgumentNull<string>(associationName, "associationName");
			model.SetAnnotationValue(property, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "AssociationName", associationName);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00007400 File Offset: 0x00005600
		public static string GetAssociationName(this IEdmModel model, IEdmNavigationProperty property)
		{
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			EdmUtil.CheckArgumentNull<IEdmNavigationProperty>(property, "property");
			property.PopulateCaches();
			string text = model.GetAnnotationValue(property, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "AssociationName");
			if (text == null)
			{
				IEdmNavigationProperty primary = property.GetPrimary();
				IEdmNavigationProperty partner = primary.Partner;
				text = SerializationExtensionMethods.GetQualifiedAndEscapedPropertyName(partner) + '_' + SerializationExtensionMethods.GetQualifiedAndEscapedPropertyName(primary);
			}
			return text;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00007468 File Offset: 0x00005668
		public static void SetAssociationNamespace(this IEdmModel model, IEdmNavigationProperty property, string associationNamespace)
		{
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			EdmUtil.CheckArgumentNull<IEdmNavigationProperty>(property, "property");
			EdmUtil.CheckArgumentNull<string>(associationNamespace, "associationNamespace");
			model.SetAnnotationValue(property, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "AssociationNamespace", associationNamespace);
		}

		// Token: 0x060002DD RID: 733 RVA: 0x000074A0 File Offset: 0x000056A0
		public static string GetAssociationNamespace(this IEdmModel model, IEdmNavigationProperty property)
		{
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			EdmUtil.CheckArgumentNull<IEdmNavigationProperty>(property, "property");
			property.PopulateCaches();
			string text = model.GetAnnotationValue(property, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "AssociationNamespace");
			if (text == null)
			{
				text = property.GetPrimary().DeclaringEntityType().Namespace;
			}
			return text;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x000074F2 File Offset: 0x000056F2
		public static string GetAssociationFullName(this IEdmModel model, IEdmNavigationProperty property)
		{
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			EdmUtil.CheckArgumentNull<IEdmNavigationProperty>(property, "property");
			property.PopulateCaches();
			return model.GetAssociationNamespace(property) + "." + model.GetAssociationName(property);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000752C File Offset: 0x0000572C
		public static void SetAssociationAnnotations(this IEdmModel model, IEdmNavigationProperty property, IEnumerable<IEdmDirectValueAnnotation> annotations, IEnumerable<IEdmDirectValueAnnotation> end1Annotations, IEnumerable<IEdmDirectValueAnnotation> end2Annotations, IEnumerable<IEdmDirectValueAnnotation> constraintAnnotations)
		{
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			EdmUtil.CheckArgumentNull<IEdmNavigationProperty>(property, "property");
			if ((annotations != null && annotations.FirstOrDefault<IEdmDirectValueAnnotation>() != null) || (end1Annotations != null && end1Annotations.FirstOrDefault<IEdmDirectValueAnnotation>() != null) || (end2Annotations != null && end2Annotations.FirstOrDefault<IEdmDirectValueAnnotation>() != null) || (constraintAnnotations != null && constraintAnnotations.FirstOrDefault<IEdmDirectValueAnnotation>() != null))
			{
				model.SetAnnotationValue(property, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "AssociationAnnotations", new SerializationExtensionMethods.AssociationAnnotations
				{
					Annotations = annotations,
					End1Annotations = end1Annotations,
					End2Annotations = end2Annotations,
					ConstraintAnnotations = constraintAnnotations
				});
			}
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x000075B8 File Offset: 0x000057B8
		public static void GetAssociationAnnotations(this IEdmModel model, IEdmNavigationProperty property, out IEnumerable<IEdmDirectValueAnnotation> annotations, out IEnumerable<IEdmDirectValueAnnotation> end1Annotations, out IEnumerable<IEdmDirectValueAnnotation> end2Annotations, out IEnumerable<IEdmDirectValueAnnotation> constraintAnnotations)
		{
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			EdmUtil.CheckArgumentNull<IEdmNavigationProperty>(property, "property");
			property.PopulateCaches();
			SerializationExtensionMethods.AssociationAnnotations annotationValue = model.GetAnnotationValue(property, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "AssociationAnnotations");
			if (annotationValue != null)
			{
				annotations = (annotationValue.Annotations ?? Enumerable.Empty<IEdmDirectValueAnnotation>());
				end1Annotations = (annotationValue.End1Annotations ?? Enumerable.Empty<IEdmDirectValueAnnotation>());
				end2Annotations = (annotationValue.End2Annotations ?? Enumerable.Empty<IEdmDirectValueAnnotation>());
				constraintAnnotations = (annotationValue.ConstraintAnnotations ?? Enumerable.Empty<IEdmDirectValueAnnotation>());
				return;
			}
			IEnumerable<IEdmDirectValueAnnotation> enumerable = Enumerable.Empty<IEdmDirectValueAnnotation>();
			annotations = enumerable;
			end1Annotations = enumerable;
			end2Annotations = enumerable;
			constraintAnnotations = enumerable;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00007653 File Offset: 0x00005853
		public static void SetAssociationEndName(this IEdmModel model, IEdmNavigationProperty property, string association)
		{
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			EdmUtil.CheckArgumentNull<IEdmNavigationProperty>(property, "property");
			model.SetAnnotationValue(property, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "AssociationEndName", association);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000767F File Offset: 0x0000587F
		public static string GetAssociationEndName(this IEdmModel model, IEdmNavigationProperty property)
		{
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			EdmUtil.CheckArgumentNull<IEdmNavigationProperty>(property, "property");
			property.PopulateCaches();
			return model.GetAnnotationValue(property, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "AssociationEndName") ?? property.Partner.Name;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x000076C0 File Offset: 0x000058C0
		public static void SetAssociationSetName(this IEdmModel model, IEdmEntitySet entitySet, IEdmNavigationProperty property, string associationSet)
		{
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			EdmUtil.CheckArgumentNull<IEdmEntitySet>(entitySet, "entitySet");
			EdmUtil.CheckArgumentNull<IEdmNavigationProperty>(property, "property");
			Dictionary<IEdmNavigationProperty, string> dictionary = model.GetAnnotationValue(entitySet, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "AssociationSetName");
			if (dictionary == null)
			{
				dictionary = new Dictionary<IEdmNavigationProperty, string>(SerializationExtensionMethods.EdmNavigationPropertyHashComparer.Instance);
				model.SetAnnotationValue(entitySet, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "AssociationSetName", dictionary);
			}
			dictionary[property] = associationSet;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000772C File Offset: 0x0000592C
		public static string GetAssociationSetName(this IEdmModel model, IEdmEntitySet entitySet, IEdmNavigationProperty property)
		{
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			EdmUtil.CheckArgumentNull<IEdmEntitySet>(entitySet, "entitySet");
			EdmUtil.CheckArgumentNull<IEdmNavigationProperty>(property, "property");
			Dictionary<IEdmNavigationProperty, string> annotationValue = model.GetAnnotationValue(entitySet, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "AssociationSetName");
			string result;
			if (annotationValue == null || !annotationValue.TryGetValue(property, out result))
			{
				result = model.GetAssociationName(property) + "Set";
			}
			return result;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x00007790 File Offset: 0x00005990
		public static void SetAssociationSetAnnotations(this IEdmModel model, IEdmEntitySet entitySet, IEdmNavigationProperty property, IEnumerable<IEdmDirectValueAnnotation> annotations, IEnumerable<IEdmDirectValueAnnotation> end1Annotations, IEnumerable<IEdmDirectValueAnnotation> end2Annotations)
		{
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			EdmUtil.CheckArgumentNull<IEdmEntitySet>(entitySet, "property");
			EdmUtil.CheckArgumentNull<IEdmNavigationProperty>(property, "property");
			if ((annotations != null && annotations.FirstOrDefault<IEdmDirectValueAnnotation>() != null) || (end1Annotations != null && end1Annotations.FirstOrDefault<IEdmDirectValueAnnotation>() != null) || (end2Annotations != null && end2Annotations.FirstOrDefault<IEdmDirectValueAnnotation>() != null))
			{
				Dictionary<IEdmNavigationProperty, SerializationExtensionMethods.AssociationSetAnnotations> dictionary = model.GetAnnotationValue(entitySet, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "AssociationSetAnnotations");
				if (dictionary == null)
				{
					dictionary = new Dictionary<IEdmNavigationProperty, SerializationExtensionMethods.AssociationSetAnnotations>(SerializationExtensionMethods.EdmNavigationPropertyHashComparer.Instance);
					model.SetAnnotationValue(entitySet, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "AssociationSetAnnotations", dictionary);
				}
				dictionary[property] = new SerializationExtensionMethods.AssociationSetAnnotations
				{
					Annotations = annotations,
					End1Annotations = end1Annotations,
					End2Annotations = end2Annotations
				};
			}
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00007840 File Offset: 0x00005A40
		public static void GetAssociationSetAnnotations(this IEdmModel model, IEdmEntitySet entitySet, IEdmNavigationProperty property, out IEnumerable<IEdmDirectValueAnnotation> annotations, out IEnumerable<IEdmDirectValueAnnotation> end1Annotations, out IEnumerable<IEdmDirectValueAnnotation> end2Annotations)
		{
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			EdmUtil.CheckArgumentNull<IEdmEntitySet>(entitySet, "entitySet");
			EdmUtil.CheckArgumentNull<IEdmNavigationProperty>(property, "property");
			Dictionary<IEdmNavigationProperty, SerializationExtensionMethods.AssociationSetAnnotations> annotationValue = model.GetAnnotationValue(entitySet, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "AssociationSetAnnotations");
			SerializationExtensionMethods.AssociationSetAnnotations associationSetAnnotations;
			if (annotationValue != null && annotationValue.TryGetValue(property, out associationSetAnnotations))
			{
				annotations = (associationSetAnnotations.Annotations ?? Enumerable.Empty<IEdmDirectValueAnnotation>());
				end1Annotations = (associationSetAnnotations.End1Annotations ?? Enumerable.Empty<IEdmDirectValueAnnotation>());
				end2Annotations = (associationSetAnnotations.End2Annotations ?? Enumerable.Empty<IEdmDirectValueAnnotation>());
				return;
			}
			IEnumerable<IEdmDirectValueAnnotation> enumerable = Enumerable.Empty<IEdmDirectValueAnnotation>();
			annotations = enumerable;
			end1Annotations = enumerable;
			end2Annotations = enumerable;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x000078D8 File Offset: 0x00005AD8
		public static IEdmNavigationProperty GetPrimary(this IEdmNavigationProperty property)
		{
			if (property.IsPrincipal)
			{
				return property;
			}
			IEdmNavigationProperty partner = property.Partner;
			if (partner.IsPrincipal)
			{
				return partner;
			}
			int num = string.Compare(property.Name, partner.Name, StringComparison.Ordinal);
			if (num == 0)
			{
				num = string.Compare(property.DeclaringEntityType().FullName(), partner.DeclaringEntityType().FullName(), StringComparison.Ordinal);
			}
			if (num <= 0)
			{
				return partner;
			}
			return property;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000793A File Offset: 0x00005B3A
		public static void SetIsValueExplicit(this IEdmEnumMember member, IEdmModel model, bool? isExplicit)
		{
			EdmUtil.CheckArgumentNull<IEdmEnumMember>(member, "member");
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			model.SetAnnotationValue(member, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "IsEnumMemberValueExplicit", isExplicit);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000796B File Offset: 0x00005B6B
		public static bool? IsValueExplicit(this IEdmEnumMember member, IEdmModel model)
		{
			EdmUtil.CheckArgumentNull<IEdmEnumMember>(member, "member");
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			return model.GetAnnotationValue(member, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "IsEnumMemberValueExplicit") as bool?;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x000079A0 File Offset: 0x00005BA0
		public static void SetIsSerializedAsElement(this IEdmValue value, IEdmModel model, bool isSerializedAsElement)
		{
			EdmUtil.CheckArgumentNull<IEdmValue>(value, "value");
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			EdmError edmError;
			if (isSerializedAsElement && !ValidationHelper.ValidateValueCanBeWrittenAsXmlElementAnnotation(value, null, null, out edmError))
			{
				throw new InvalidOperationException(edmError.ToString());
			}
			model.SetAnnotationValue(value, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "IsSerializedAsElement", isSerializedAsElement);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x000079F8 File Offset: 0x00005BF8
		public static bool IsSerializedAsElement(this IEdmValue value, IEdmModel model)
		{
			EdmUtil.CheckArgumentNull<IEdmValue>(value, "value");
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			return (model.GetAnnotationValue(value, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "IsSerializedAsElement") as bool?) ?? false;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00007A4C File Offset: 0x00005C4C
		public static void SetNamespaceAlias(this IEdmModel model, string namespaceName, string alias)
		{
			VersioningDictionary<string, string> versioningDictionary = model.GetAnnotationValue(model, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "NamespaceAlias");
			if (versioningDictionary == null)
			{
				versioningDictionary = VersioningDictionary<string, string>.Create(new Func<string, string, int>(string.CompareOrdinal));
			}
			if (alias == null)
			{
				versioningDictionary = versioningDictionary.Remove(namespaceName);
			}
			else
			{
				versioningDictionary = versioningDictionary.Set(namespaceName, alias);
			}
			model.SetAnnotationValue(model, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "NamespaceAlias", versioningDictionary);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x00007AA8 File Offset: 0x00005CA8
		public static string GetNamespaceAlias(this IEdmModel model, string namespaceName)
		{
			VersioningDictionary<string, string> annotationValue = model.GetAnnotationValue(model, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "NamespaceAlias");
			return annotationValue.Get(namespaceName);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00007ACE File Offset: 0x00005CCE
		internal static VersioningDictionary<string, string> GetNamespaceAliases(this IEdmModel model)
		{
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			return model.GetAnnotationValue(model, "http://schemas.microsoft.com/ado/2011/04/edm/internal", "NamespaceAlias");
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00007AF0 File Offset: 0x00005CF0
		internal static bool IsInline(this IEdmVocabularyAnnotation annotation, IEdmModel model)
		{
			return annotation.GetSerializationLocation(model) == EdmVocabularyAnnotationSerializationLocation.Inline || annotation.TargetString() == null;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00007B25 File Offset: 0x00005D25
		internal static string TargetString(this IEdmVocabularyAnnotation annotation)
		{
			return EdmUtil.FullyQualifiedName(annotation.Target);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00007B32 File Offset: 0x00005D32
		private static void PopulateCaches(this IEdmNavigationProperty property)
		{
			IEdmNavigationProperty partner = property.Partner;
			bool isPrincipal = property.IsPrincipal;
			IEnumerable<IEdmStructuralProperty> dependentProperties = property.DependentProperties;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00007B4C File Offset: 0x00005D4C
		private static string GetQualifiedAndEscapedPropertyName(IEdmNavigationProperty property)
		{
			return string.Concat(new object[]
			{
				SerializationExtensionMethods.EscapeName(property.DeclaringEntityType().Namespace).Replace('.', '_'),
				'_',
				SerializationExtensionMethods.EscapeName(property.DeclaringEntityType().Name),
				'_',
				SerializationExtensionMethods.EscapeName(property.Name)
			});
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00007BB7 File Offset: 0x00005DB7
		private static string EscapeName(string name)
		{
			return name.Replace("_", "__");
		}

		// Token: 0x0400014C RID: 332
		private const char AssociationNameEscapeChar = '_';

		// Token: 0x0400014D RID: 333
		private const string AssociationNameEscapeString = "_";

		// Token: 0x0400014E RID: 334
		private const string AssociationNameEscapeStringEscaped = "__";

		// Token: 0x020000AB RID: 171
		private class AssociationAnnotations
		{
			// Token: 0x17000183 RID: 387
			// (get) Token: 0x060002F4 RID: 756 RVA: 0x00007BC9 File Offset: 0x00005DC9
			// (set) Token: 0x060002F5 RID: 757 RVA: 0x00007BD1 File Offset: 0x00005DD1
			public IEnumerable<IEdmDirectValueAnnotation> Annotations { get; set; }

			// Token: 0x17000184 RID: 388
			// (get) Token: 0x060002F6 RID: 758 RVA: 0x00007BDA File Offset: 0x00005DDA
			// (set) Token: 0x060002F7 RID: 759 RVA: 0x00007BE2 File Offset: 0x00005DE2
			public IEnumerable<IEdmDirectValueAnnotation> End1Annotations { get; set; }

			// Token: 0x17000185 RID: 389
			// (get) Token: 0x060002F8 RID: 760 RVA: 0x00007BEB File Offset: 0x00005DEB
			// (set) Token: 0x060002F9 RID: 761 RVA: 0x00007BF3 File Offset: 0x00005DF3
			public IEnumerable<IEdmDirectValueAnnotation> End2Annotations { get; set; }

			// Token: 0x17000186 RID: 390
			// (get) Token: 0x060002FA RID: 762 RVA: 0x00007BFC File Offset: 0x00005DFC
			// (set) Token: 0x060002FB RID: 763 RVA: 0x00007C04 File Offset: 0x00005E04
			public IEnumerable<IEdmDirectValueAnnotation> ConstraintAnnotations { get; set; }
		}

		// Token: 0x020000AC RID: 172
		private class AssociationSetAnnotations
		{
			// Token: 0x17000187 RID: 391
			// (get) Token: 0x060002FD RID: 765 RVA: 0x00007C15 File Offset: 0x00005E15
			// (set) Token: 0x060002FE RID: 766 RVA: 0x00007C1D File Offset: 0x00005E1D
			public IEnumerable<IEdmDirectValueAnnotation> Annotations { get; set; }

			// Token: 0x17000188 RID: 392
			// (get) Token: 0x060002FF RID: 767 RVA: 0x00007C26 File Offset: 0x00005E26
			// (set) Token: 0x06000300 RID: 768 RVA: 0x00007C2E File Offset: 0x00005E2E
			public IEnumerable<IEdmDirectValueAnnotation> End1Annotations { get; set; }

			// Token: 0x17000189 RID: 393
			// (get) Token: 0x06000301 RID: 769 RVA: 0x00007C37 File Offset: 0x00005E37
			// (set) Token: 0x06000302 RID: 770 RVA: 0x00007C3F File Offset: 0x00005E3F
			public IEnumerable<IEdmDirectValueAnnotation> End2Annotations { get; set; }
		}

		// Token: 0x020000AD RID: 173
		private class EdmNavigationPropertyHashComparer : EqualityComparer<IEdmNavigationProperty>
		{
			// Token: 0x06000304 RID: 772 RVA: 0x00007C50 File Offset: 0x00005E50
			private EdmNavigationPropertyHashComparer()
			{
			}

			// Token: 0x1700018A RID: 394
			// (get) Token: 0x06000305 RID: 773 RVA: 0x00007C58 File Offset: 0x00005E58
			internal static SerializationExtensionMethods.EdmNavigationPropertyHashComparer Instance
			{
				get
				{
					return SerializationExtensionMethods.EdmNavigationPropertyHashComparer.instance;
				}
			}

			// Token: 0x06000306 RID: 774 RVA: 0x00007C60 File Offset: 0x00005E60
			public override bool Equals(IEdmNavigationProperty left, IEdmNavigationProperty right)
			{
				string a = SerializationExtensionMethods.EdmNavigationPropertyHashComparer.GenerateHash(right);
				string b = SerializationExtensionMethods.EdmNavigationPropertyHashComparer.GenerateHash(left);
				return a == b;
			}

			// Token: 0x06000307 RID: 775 RVA: 0x00007C84 File Offset: 0x00005E84
			public override int GetHashCode(IEdmNavigationProperty obj)
			{
				string text = SerializationExtensionMethods.EdmNavigationPropertyHashComparer.GenerateHash(obj);
				return text.GetHashCode();
			}

			// Token: 0x06000308 RID: 776 RVA: 0x00007C9E File Offset: 0x00005E9E
			private static string GenerateHash(IEdmNavigationProperty prop)
			{
				return prop.Name + "_" + prop.DeclaringEntityType().FullName();
			}

			// Token: 0x04000156 RID: 342
			private static SerializationExtensionMethods.EdmNavigationPropertyHashComparer instance = new SerializationExtensionMethods.EdmNavigationPropertyHashComparer();
		}
	}
}
