using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData
{
	// Token: 0x0200024F RID: 591
	public static class ODataUtils
	{
		// Token: 0x06001354 RID: 4948 RVA: 0x0004890D File Offset: 0x00046B0D
		public static ODataFormat SetHeadersForPayload(ODataMessageWriter messageWriter, ODataPayloadKind payloadKind)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataMessageWriter>(messageWriter, "messageWriter");
			if (payloadKind == ODataPayloadKind.Unsupported)
			{
				throw new ArgumentException(Strings.ODataMessageWriter_CannotSetHeadersWithInvalidPayloadKind(payloadKind), "payloadKind");
			}
			return messageWriter.SetHeaders(payloadKind);
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x0004893F File Offset: 0x00046B3F
		public static ODataFormat GetReadFormat(ODataMessageReader messageReader)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataMessageReader>(messageReader, "messageReader");
			return messageReader.GetFormat();
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x00048952 File Offset: 0x00046B52
		public static void LoadODataAnnotations(this IEdmModel model)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmModel>(model, "model");
			model.LoadODataAnnotations(100);
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x00048968 File Offset: 0x00046B68
		public static void LoadODataAnnotations(this IEdmModel model, int maxEntityPropertyMappingsPerType)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmModel>(model, "model");
			foreach (IEdmEntityType entityType in model.EntityTypes())
			{
				model.LoadODataAnnotations(entityType, maxEntityPropertyMappingsPerType);
			}
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x000489C4 File Offset: 0x00046BC4
		public static void LoadODataAnnotations(this IEdmModel model, IEdmEntityType entityType)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmModel>(model, "model");
			ExceptionUtils.CheckArgumentNotNull<IEdmEntityType>(entityType, "entityType");
			model.LoadODataAnnotations(entityType, 100);
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x000489E5 File Offset: 0x00046BE5
		public static void LoadODataAnnotations(this IEdmModel model, IEdmEntityType entityType, int maxEntityPropertyMappingsPerType)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmModel>(model, "model");
			ExceptionUtils.CheckArgumentNotNull<IEdmEntityType>(entityType, "entityType");
			model.ClearInMemoryEpmAnnotations(entityType);
			model.EnsureEpmCache(entityType, maxEntityPropertyMappingsPerType);
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x00048A10 File Offset: 0x00046C10
		public static void SaveODataAnnotations(this IEdmModel model)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmModel>(model, "model");
			if (!model.IsUserModel())
			{
				throw new ODataException(Strings.ODataUtils_CannotSaveAnnotationsToBuiltInModel);
			}
			foreach (IEdmEntityType entityType in model.EntityTypes())
			{
				ODataUtils.SaveODataAnnotationsImplementation(model, entityType);
			}
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x00048A7C File Offset: 0x00046C7C
		public static void SaveODataAnnotations(this IEdmModel model, IEdmEntityType entityType)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmModel>(model, "model");
			ExceptionUtils.CheckArgumentNotNull<IEdmEntityType>(entityType, "entityType");
			ODataUtils.SaveODataAnnotationsImplementation(model, entityType);
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x00048A9C File Offset: 0x00046C9C
		public static bool HasDefaultStream(this IEdmModel model, IEdmEntityType entityType)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmModel>(model, "model");
			ExceptionUtils.CheckArgumentNotNull<IEdmEntityType>(entityType, "entityType");
			bool flag;
			return ODataUtils.TryGetBooleanAnnotation(model, entityType, "HasStream", true, out flag) && flag;
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x00048AD3 File Offset: 0x00046CD3
		public static void SetHasDefaultStream(this IEdmModel model, IEdmEntityType entityType, bool hasStream)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmModel>(model, "model");
			ExceptionUtils.CheckArgumentNotNull<IEdmEntityType>(entityType, "entityType");
			ODataUtils.SetBooleanAnnotation(model, entityType, "HasStream", hasStream);
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x00048AF8 File Offset: 0x00046CF8
		public static bool IsDefaultEntityContainer(this IEdmModel model, IEdmEntityContainer entityContainer)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmModel>(model, "model");
			ExceptionUtils.CheckArgumentNotNull<IEdmEntityContainer>(entityContainer, "entityContainer");
			bool flag;
			return ODataUtils.TryGetBooleanAnnotation(model, entityContainer, "IsDefaultEntityContainer", out flag) && flag;
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x00048B2E File Offset: 0x00046D2E
		public static void SetIsDefaultEntityContainer(this IEdmModel model, IEdmEntityContainer entityContainer, bool isDefaultContainer)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmModel>(model, "model");
			ExceptionUtils.CheckArgumentNotNull<IEdmEntityContainer>(entityContainer, "entityContainer");
			ODataUtils.SetBooleanAnnotation(model, entityContainer, "IsDefaultEntityContainer", isDefaultContainer);
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x00048B54 File Offset: 0x00046D54
		public static string GetMimeType(this IEdmModel model, IEdmElement annotatable)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmModel>(model, "model");
			ExceptionUtils.CheckArgumentNotNull<IEdmElement>(annotatable, "annotatable");
			string text;
			if (!model.TryGetODataAnnotation(annotatable, "MimeType", out text))
			{
				return null;
			}
			if (text == null)
			{
				throw new ODataException(Strings.ODataUtils_NullValueForMimeTypeAnnotation);
			}
			return text;
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x00048B98 File Offset: 0x00046D98
		public static void SetMimeType(this IEdmModel model, IEdmElement annotatable, string mimeType)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmModel>(model, "model");
			ExceptionUtils.CheckArgumentNotNull<IEdmElement>(annotatable, "annotatable");
			model.SetODataAnnotation(annotatable, "MimeType", mimeType);
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x00048BC0 File Offset: 0x00046DC0
		public static string GetHttpMethod(this IEdmModel model, IEdmElement annotatable)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmModel>(model, "model");
			ExceptionUtils.CheckArgumentNotNull<IEdmElement>(annotatable, "annotatable");
			string text;
			if (!model.TryGetODataAnnotation(annotatable, "HttpMethod", out text))
			{
				return null;
			}
			if (text == null)
			{
				throw new ODataException(Strings.ODataUtils_NullValueForHttpMethodAnnotation);
			}
			return text;
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x00048C04 File Offset: 0x00046E04
		public static void SetHttpMethod(this IEdmModel model, IEdmElement annotatable, string httpMethod)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmModel>(model, "model");
			ExceptionUtils.CheckArgumentNotNull<IEdmElement>(annotatable, "annotatable");
			model.SetODataAnnotation(annotatable, "HttpMethod", httpMethod);
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x00048C2C File Offset: 0x00046E2C
		public static bool IsAlwaysBindable(this IEdmModel model, IEdmFunctionImport functionImport)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmModel>(model, "model");
			ExceptionUtils.CheckArgumentNotNull<IEdmFunctionImport>(functionImport, "functionImport");
			bool flag;
			if (!ODataUtils.TryGetBooleanAnnotation(model, functionImport, "IsAlwaysBindable", out flag))
			{
				return false;
			}
			if (!functionImport.IsBindable && flag)
			{
				throw new ODataException(Strings.ODataUtils_UnexpectedIsAlwaysBindableAnnotationInANonBindableFunctionImport);
			}
			return flag;
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x00048C78 File Offset: 0x00046E78
		public static void SetIsAlwaysBindable(this IEdmModel model, IEdmFunctionImport functionImport, bool isAlwaysBindable)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmModel>(model, "model");
			ExceptionUtils.CheckArgumentNotNull<IEdmFunctionImport>(functionImport, "functionImport");
			if (!functionImport.IsBindable && isAlwaysBindable)
			{
				throw new ODataException(Strings.ODataUtils_IsAlwaysBindableAnnotationSetForANonBindableFunctionImport);
			}
			ODataUtils.SetBooleanAnnotation(model, functionImport, "IsAlwaysBindable", isAlwaysBindable);
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x00048CB4 File Offset: 0x00046EB4
		public static ODataNullValueBehaviorKind NullValueReadBehaviorKind(this IEdmModel model, IEdmProperty property)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmModel>(model, "model");
			ExceptionUtils.CheckArgumentNotNull<IEdmProperty>(property, "property");
			ODataEdmPropertyAnnotation annotationValue = model.GetAnnotationValue(property);
			if (annotationValue != null)
			{
				return annotationValue.NullValueReadBehaviorKind;
			}
			return ODataNullValueBehaviorKind.Default;
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x00048CEC File Offset: 0x00046EEC
		public static void SetNullValueReaderBehavior(this IEdmModel model, IEdmProperty property, ODataNullValueBehaviorKind nullValueReadBehaviorKind)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmModel>(model, "model");
			ExceptionUtils.CheckArgumentNotNull<IEdmProperty>(property, "property");
			ODataEdmPropertyAnnotation odataEdmPropertyAnnotation = model.GetAnnotationValue(property);
			if (odataEdmPropertyAnnotation == null)
			{
				if (nullValueReadBehaviorKind != ODataNullValueBehaviorKind.Default)
				{
					odataEdmPropertyAnnotation = new ODataEdmPropertyAnnotation
					{
						NullValueReadBehaviorKind = nullValueReadBehaviorKind
					};
					model.SetAnnotationValue(property, odataEdmPropertyAnnotation);
					return;
				}
			}
			else
			{
				odataEdmPropertyAnnotation.NullValueReadBehaviorKind = nullValueReadBehaviorKind;
			}
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x00048D3C File Offset: 0x00046F3C
		public static string ODataVersionToString(ODataVersion version)
		{
			switch (version)
			{
			case ODataVersion.V1:
				return "1.0";
			case ODataVersion.V2:
				return "2.0";
			case ODataVersion.V3:
				return "3.0";
			default:
				throw new ODataException(Strings.ODataUtils_UnsupportedVersionNumber);
			}
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x00048D7C File Offset: 0x00046F7C
		public static ODataVersion StringToODataVersion(string version)
		{
			string text = version;
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(version, "version");
			int num = text.IndexOf(';');
			if (num >= 0)
			{
				text = text.Substring(0, num);
			}
			string a;
			if ((a = text.Trim()) != null)
			{
				if (a == "1.0")
				{
					return ODataVersion.V1;
				}
				if (a == "2.0")
				{
					return ODataVersion.V2;
				}
				if (a == "3.0")
				{
					return ODataVersion.V3;
				}
			}
			throw new ODataException(Strings.ODataUtils_UnsupportedVersionHeader(version));
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x00048DF0 File Offset: 0x00046FF0
		public static Func<string, bool> CreateAnnotationFilter(string annotationFilter)
		{
			AnnotationFilter @object = AnnotationFilter.Create(annotationFilter);
			return new Func<string, bool>(@object.Matches);
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x00048E14 File Offset: 0x00047014
		private static void SaveODataAnnotationsImplementation(IEdmModel model, IEdmEntityType entityType)
		{
			ODataEntityPropertyMappingCache odataEntityPropertyMappingCache = model.EnsureEpmCache(entityType, int.MaxValue);
			if (odataEntityPropertyMappingCache != null)
			{
				model.SaveEpmAnnotations(entityType, odataEntityPropertyMappingCache.MappingsForInheritedProperties, false, false);
				IEnumerable<IEdmProperty> declaredProperties = entityType.DeclaredProperties;
				if (declaredProperties != null)
				{
					foreach (IEdmProperty edmProperty in declaredProperties)
					{
						if (edmProperty.Type.IsODataPrimitiveTypeKind() || edmProperty.Type.IsODataComplexTypeKind())
						{
							model.SaveEpmAnnotationsForProperty(edmProperty, odataEntityPropertyMappingCache);
						}
						else if (edmProperty.Type.IsNonEntityCollectionType())
						{
							model.SaveEpmAnnotationsForProperty(edmProperty, odataEntityPropertyMappingCache);
						}
					}
				}
			}
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x00048EB8 File Offset: 0x000470B8
		private static bool TryGetBooleanAnnotation(IEdmModel model, IEdmStructuredType structuredType, string annotationLocalName, bool recursive, out bool boolValue)
		{
			string s = null;
			bool flag;
			do
			{
				flag = model.TryGetODataAnnotation(structuredType, annotationLocalName, out s);
				if (flag)
				{
					break;
				}
				structuredType = structuredType.BaseType;
			}
			while (recursive && structuredType != null);
			if (!flag)
			{
				boolValue = false;
				return false;
			}
			boolValue = XmlConvert.ToBoolean(s);
			return true;
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x00048EF8 File Offset: 0x000470F8
		private static bool TryGetBooleanAnnotation(IEdmModel model, IEdmElement annotatable, string annotationLocalName, out bool boolValue)
		{
			string s;
			if (model.TryGetODataAnnotation(annotatable, annotationLocalName, out s))
			{
				boolValue = XmlConvert.ToBoolean(s);
				return true;
			}
			boolValue = false;
			return false;
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x00048F1F File Offset: 0x0004711F
		private static void SetBooleanAnnotation(IEdmModel model, IEdmElement annotatable, string annotationLocalName, bool boolValue)
		{
			model.SetODataAnnotation(annotatable, annotationLocalName, boolValue ? "true" : null);
		}

		// Token: 0x040006F2 RID: 1778
		private const string Version1NumberString = "1.0";

		// Token: 0x040006F3 RID: 1779
		private const string Version2NumberString = "2.0";

		// Token: 0x040006F4 RID: 1780
		private const string Version3NumberString = "3.0";
	}
}
