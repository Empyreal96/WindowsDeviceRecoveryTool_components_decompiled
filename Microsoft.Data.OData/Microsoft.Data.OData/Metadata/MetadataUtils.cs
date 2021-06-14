using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Annotations;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.Edm.Library.Values;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.OData.Metadata
{
	// Token: 0x02000254 RID: 596
	internal static class MetadataUtils
	{
		// Token: 0x0600138C RID: 5004 RVA: 0x00049508 File Offset: 0x00047708
		internal static bool TryGetODataAnnotation(this IEdmModel model, IEdmElement annotatable, string localName, out string value)
		{
			object annotationValue = model.GetAnnotationValue(annotatable, "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata", localName);
			if (annotationValue == null)
			{
				value = null;
				return false;
			}
			IEdmStringValue edmStringValue = annotationValue as IEdmStringValue;
			if (edmStringValue == null)
			{
				throw new ODataException(Strings.ODataAtomWriterMetadataUtils_InvalidAnnotationValue(localName, annotationValue.GetType().FullName));
			}
			value = edmStringValue.Value;
			return true;
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x00049558 File Offset: 0x00047758
		internal static void SetODataAnnotation(this IEdmModel model, IEdmElement annotatable, string localName, string value)
		{
			IEdmStringValue value2 = null;
			if (value != null)
			{
				IEdmStringTypeReference @string = EdmCoreModel.Instance.GetString(true);
				value2 = new EdmStringConstant(@string, value);
			}
			model.SetAnnotationValue(annotatable, "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata", localName, value2);
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x000495A0 File Offset: 0x000477A0
		internal static IEnumerable<IEdmDirectValueAnnotation> GetODataAnnotations(this IEdmModel model, IEdmElement annotatable)
		{
			IEnumerable<IEdmDirectValueAnnotation> enumerable = model.DirectValueAnnotations(annotatable);
			if (enumerable == null)
			{
				return null;
			}
			return from a in enumerable
			where a.NamespaceUri == "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata"
			select a;
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x000495E0 File Offset: 0x000477E0
		internal static IEdmTypeReference GetEdmType(this ODataAnnotatable annotatable)
		{
			if (annotatable == null)
			{
				return null;
			}
			ODataTypeAnnotation annotation = annotatable.GetAnnotation<ODataTypeAnnotation>();
			if (annotation != null)
			{
				return annotation.Type;
			}
			return null;
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x00049604 File Offset: 0x00047804
		internal static IEdmType ResolveTypeNameForWrite(IEdmModel model, string typeName)
		{
			EdmTypeKind edmTypeKind;
			return MetadataUtils.ResolveTypeName(model, null, typeName, null, ODataVersion.V3, out edmTypeKind);
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x00049620 File Offset: 0x00047820
		internal static IEdmType ResolveTypeNameForRead(IEdmModel model, IEdmType expectedType, string typeName, ODataReaderBehavior readerBehavior, ODataVersion version, out EdmTypeKind typeKind)
		{
			Func<IEdmType, string, IEdmType> customTypeResolver = (readerBehavior == null) ? null : readerBehavior.TypeResolver;
			return MetadataUtils.ResolveTypeName(model, expectedType, typeName, customTypeResolver, version, out typeKind);
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x00049648 File Offset: 0x00047848
		internal static IEdmType ResolveTypeName(IEdmModel model, IEdmType expectedType, string typeName, Func<IEdmType, string, IEdmType> customTypeResolver, ODataVersion version, out EdmTypeKind typeKind)
		{
			IEdmType edmType = null;
			string text = (version >= ODataVersion.V3) ? EdmLibraryExtensions.GetCollectionItemTypeName(typeName) : null;
			if (text == null)
			{
				if (customTypeResolver != null && model.IsUserModel())
				{
					edmType = customTypeResolver(expectedType, typeName);
					if (edmType == null)
					{
						throw new ODataException(Strings.MetadataUtils_ResolveTypeName(typeName));
					}
				}
				else
				{
					edmType = model.FindType(typeName);
				}
				if (version < ODataVersion.V3 && edmType != null && edmType.IsSpatial())
				{
					edmType = null;
				}
				typeKind = ((edmType == null) ? EdmTypeKind.None : edmType.TypeKind);
			}
			else
			{
				typeKind = EdmTypeKind.Collection;
				IEdmType expectedType2 = null;
				if (customTypeResolver != null && expectedType != null && expectedType.TypeKind == EdmTypeKind.Collection)
				{
					expectedType2 = ((IEdmCollectionType)expectedType).ElementType.Definition;
				}
				EdmTypeKind edmTypeKind;
				IEdmType edmType2 = MetadataUtils.ResolveTypeName(model, expectedType2, text, customTypeResolver, version, out edmTypeKind);
				if (edmType2 != null)
				{
					edmType = EdmLibraryExtensions.GetCollectionType(edmType2);
				}
			}
			return edmType;
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x000496FC File Offset: 0x000478FC
		internal static IEdmFunctionImport[] CalculateAlwaysBindableOperationsForType(IEdmType bindingType, IEdmModel model, EdmTypeResolver edmTypeResolver)
		{
			List<IEdmFunctionImport> list = new List<IEdmFunctionImport>();
			foreach (IEdmEntityContainer container in model.EntityContainers())
			{
				foreach (IEdmFunctionImport edmFunctionImport in container.FunctionImports())
				{
					if (edmFunctionImport.IsBindable && model.IsAlwaysBindable(edmFunctionImport))
					{
						IEdmFunctionParameter edmFunctionParameter = edmFunctionImport.Parameters.FirstOrDefault<IEdmFunctionParameter>();
						if (edmFunctionParameter != null)
						{
							IEdmType definition = edmTypeResolver.GetParameterType(edmFunctionParameter).Definition;
							if (definition.IsAssignableFrom(bindingType))
							{
								list.Add(edmFunctionImport);
							}
						}
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x000497CC File Offset: 0x000479CC
		internal static IEdmTypeReference LookupTypeOfValueTerm(string qualifiedTermName, IEdmModel model)
		{
			IEdmTypeReference result = null;
			IEdmValueTerm edmValueTerm = model.FindValueTerm(qualifiedTermName);
			if (edmValueTerm != null)
			{
				result = edmValueTerm.Type;
			}
			return result;
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x000497EE File Offset: 0x000479EE
		internal static IEdmTypeReference GetNullablePayloadTypeReference(IEdmType payloadType)
		{
			if (payloadType != null)
			{
				return payloadType.ToTypeReference(true);
			}
			return null;
		}
	}
}
