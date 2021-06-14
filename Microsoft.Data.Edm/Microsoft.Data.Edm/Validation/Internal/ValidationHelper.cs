using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.Edm.Validation.Internal
{
	// Token: 0x02000237 RID: 567
	internal static class ValidationHelper
	{
		// Token: 0x06000C86 RID: 3206 RVA: 0x00024DD8 File Offset: 0x00022FD8
		internal static bool IsEdmSystemNamespace(string namespaceName)
		{
			return namespaceName == "Transient" || namespaceName == "Edm";
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x00024DF4 File Offset: 0x00022FF4
		internal static bool AddMemberNameToHashSet(IEdmNamedElement item, HashSetInternal<string> memberNameList, ValidationContext context, EdmErrorCode errorCode, string errorString, bool suppressError)
		{
			IEdmSchemaElement edmSchemaElement = item as IEdmSchemaElement;
			string thingToAdd = (edmSchemaElement != null) ? edmSchemaElement.FullName() : item.Name;
			if (!memberNameList.Add(thingToAdd))
			{
				if (!suppressError)
				{
					context.AddError(item.Location(), errorCode, errorString);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x00024E49 File Offset: 0x00023049
		internal static bool AllPropertiesAreNullable(IEnumerable<IEdmStructuralProperty> properties)
		{
			return (from p in properties
			where !p.Type.IsNullable
			select p).Count<IEdmStructuralProperty>() == 0;
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x00024E83 File Offset: 0x00023083
		internal static bool HasNullableProperty(IEnumerable<IEdmStructuralProperty> properties)
		{
			return (from p in properties
			where p.Type.IsNullable
			select p).Count<IEdmStructuralProperty>() > 0;
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x00024EB0 File Offset: 0x000230B0
		internal static bool PropertySetIsSubset(IEnumerable<IEdmStructuralProperty> set, IEnumerable<IEdmStructuralProperty> subset)
		{
			return subset.Except(set).Count<IEdmStructuralProperty>() <= 0;
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x00024EC4 File Offset: 0x000230C4
		internal static bool PropertySetsAreEquivalent(IEnumerable<IEdmStructuralProperty> set1, IEnumerable<IEdmStructuralProperty> set2)
		{
			if (set1.Count<IEdmStructuralProperty>() != set2.Count<IEdmStructuralProperty>())
			{
				return false;
			}
			IEnumerator<IEdmStructuralProperty> enumerator = set2.GetEnumerator();
			foreach (IEdmStructuralProperty edmStructuralProperty in set1)
			{
				enumerator.MoveNext();
				if (edmStructuralProperty != enumerator.Current)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x00024F34 File Offset: 0x00023134
		internal static bool ValidateValueCanBeWrittenAsXmlElementAnnotation(IEdmValue value, string annotationNamespace, string annotationName, out EdmError error)
		{
			IEdmStringValue edmStringValue = value as IEdmStringValue;
			if (edmStringValue == null)
			{
				error = new EdmError(value.Location(), EdmErrorCode.InvalidElementAnnotation, Strings.EdmModel_Validator_Semantic_InvalidElementAnnotationNotIEdmStringValue);
				return false;
			}
			string value2 = edmStringValue.Value;
			XmlReader xmlReader = XmlReader.Create(new StringReader(value2));
			bool result;
			try
			{
				if (xmlReader.NodeType != XmlNodeType.Element)
				{
					while (xmlReader.Read() && xmlReader.NodeType != XmlNodeType.Element)
					{
					}
				}
				if (xmlReader.EOF)
				{
					error = new EdmError(value.Location(), EdmErrorCode.InvalidElementAnnotation, Strings.EdmModel_Validator_Semantic_InvalidElementAnnotationValueInvalidXml);
					result = false;
				}
				else
				{
					string namespaceURI = xmlReader.NamespaceURI;
					string localName = xmlReader.LocalName;
					if (EdmUtil.IsNullOrWhiteSpaceInternal(namespaceURI) || EdmUtil.IsNullOrWhiteSpaceInternal(localName))
					{
						error = new EdmError(value.Location(), EdmErrorCode.InvalidElementAnnotation, Strings.EdmModel_Validator_Semantic_InvalidElementAnnotationNullNamespaceOrName);
						result = false;
					}
					else if ((annotationNamespace != null && !(namespaceURI == annotationNamespace)) || (annotationName != null && !(localName == annotationName)))
					{
						error = new EdmError(value.Location(), EdmErrorCode.InvalidElementAnnotation, Strings.EdmModel_Validator_Semantic_InvalidElementAnnotationMismatchedTerm);
						result = false;
					}
					else
					{
						while (xmlReader.Read())
						{
						}
						error = null;
						result = true;
					}
				}
			}
			catch (Exception)
			{
				error = new EdmError(value.Location(), EdmErrorCode.InvalidElementAnnotation, Strings.EdmModel_Validator_Semantic_InvalidElementAnnotationValueInvalidXml);
				result = false;
			}
			return result;
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x00025068 File Offset: 0x00023268
		internal static bool IsInterfaceCritical(EdmError error)
		{
			return error.ErrorCode >= EdmErrorCode.InterfaceCriticalPropertyValueMustNotBeNull && error.ErrorCode <= EdmErrorCode.InterfaceCriticalCycleInTypeHierarchy;
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x00025084 File Offset: 0x00023284
		internal static bool ItemExistsInReferencedModel(this IEdmModel model, string fullName, bool checkEntityContainer)
		{
			foreach (IEdmModel edmModel in model.ReferencedModels)
			{
				if (edmModel.FindDeclaredType(fullName) != null || edmModel.FindDeclaredValueTerm(fullName) != null || (checkEntityContainer && edmModel.FindDeclaredEntityContainer(fullName) != null) || (edmModel.FindDeclaredFunctions(fullName) ?? Enumerable.Empty<IEdmFunction>()).FirstOrDefault<IEdmFunction>() != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x00025120 File Offset: 0x00023320
		internal static bool FunctionOrNameExistsInReferencedModel(this IEdmModel model, IEdmFunction function, string functionFullName, bool checkEntityContainer)
		{
			foreach (IEdmModel edmModel in model.ReferencedModels)
			{
				if (edmModel.FindDeclaredType(functionFullName) != null || edmModel.FindDeclaredValueTerm(functionFullName) != null || (checkEntityContainer && edmModel.FindDeclaredEntityContainer(functionFullName) != null))
				{
					return true;
				}
				IEnumerable<IEdmFunction> source = edmModel.FindDeclaredFunctions(functionFullName) ?? Enumerable.Empty<IEdmFunction>();
				if (source.Any((IEdmFunction existingFunction) => function.IsFunctionSignatureEquivalentTo(existingFunction)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x000251D0 File Offset: 0x000233D0
		internal static bool TypeIndirectlyContainsTarget(IEdmEntityType source, IEdmEntityType target, HashSetInternal<IEdmEntityType> visited, IEdmModel context)
		{
			if (visited.Add(source))
			{
				if (source.IsOrInheritsFrom(target))
				{
					return true;
				}
				foreach (IEdmNavigationProperty edmNavigationProperty in source.NavigationProperties())
				{
					if (edmNavigationProperty.ContainsTarget && ValidationHelper.TypeIndirectlyContainsTarget(edmNavigationProperty.ToEntityType(), target, visited, context))
					{
						return true;
					}
				}
				foreach (IEdmStructuredType edmStructuredType in context.FindAllDerivedTypes(source))
				{
					IEdmEntityType edmEntityType = edmStructuredType as IEdmEntityType;
					if (edmEntityType != null && ValidationHelper.TypeIndirectlyContainsTarget(edmEntityType, target, visited, context))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}
	}
}
