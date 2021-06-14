using System;
using System.Collections.Generic;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.JsonLight;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Evaluation
{
	// Token: 0x02000107 RID: 263
	internal sealed class ODataMetadataContext : IODataMetadataContext
	{
		// Token: 0x06000728 RID: 1832 RVA: 0x00018944 File Offset: 0x00016B44
		public ODataMetadataContext(bool isResponse, IEdmModel model, Uri metadataDocumentUri) : this(isResponse, null, EdmTypeWriterResolver.Instance, model, metadataDocumentUri)
		{
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00018958 File Offset: 0x00016B58
		public ODataMetadataContext(bool isResponse, Func<IEdmEntityType, bool> operationsBoundToEntityTypeMustBeContainerQualified, EdmTypeResolver edmTypeResolver, IEdmModel model, Uri metadataDocumentUri)
		{
			this.isResponse = isResponse;
			this.operationsBoundToEntityTypeMustBeContainerQualified = (operationsBoundToEntityTypeMustBeContainerQualified ?? new Func<IEdmEntityType, bool>(EdmLibraryExtensions.OperationsBoundToEntityTypeMustBeContainerQualified));
			this.edmTypeResolver = edmTypeResolver;
			this.model = model;
			this.metadataDocumentUri = metadataDocumentUri;
			this.alwaysBindableOperationsCache = new Dictionary<IEdmType, IEdmFunctionImport[]>(ReferenceEqualityComparer<IEdmType>.Instance);
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x0600072A RID: 1834 RVA: 0x000189B0 File Offset: 0x00016BB0
		public IEdmModel Model
		{
			get
			{
				return this.model;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x000189B8 File Offset: 0x00016BB8
		public Uri ServiceBaseUri
		{
			get
			{
				Uri result;
				if ((result = this.serviceBaseUri) == null)
				{
					result = (this.serviceBaseUri = new Uri(this.MetadataDocumentUri, "./"));
				}
				return result;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x000189E8 File Offset: 0x00016BE8
		public Uri MetadataDocumentUri
		{
			get
			{
				if (this.metadataDocumentUri == null)
				{
					throw new ODataException(Strings.ODataJsonLightEntryMetadataContext_MetadataAnnotationMustBeInPayload("odata.metadata"));
				}
				return this.metadataDocumentUri;
			}
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00018A10 File Offset: 0x00016C10
		public ODataEntityMetadataBuilder GetEntityMetadataBuilderForReader(IODataJsonLightReaderEntryState entryState)
		{
			if (entryState.MetadataBuilder == null)
			{
				ODataEntry entry = entryState.Entry;
				if (this.isResponse)
				{
					ODataTypeAnnotation annotation = entry.GetAnnotation<ODataTypeAnnotation>();
					IEdmEntitySet entitySet = annotation.EntitySet;
					IEdmEntityType elementType = this.edmTypeResolver.GetElementType(entitySet);
					IODataFeedAndEntryTypeContext typeContext = ODataFeedAndEntryTypeContext.Create(null, entitySet, elementType, entryState.EntityType, this.model, true);
					IODataEntryMetadataContext entryMetadataContext = ODataEntryMetadataContext.Create(entry, typeContext, null, (IEdmEntityType)entry.GetEdmType().Definition, this, entryState.SelectedProperties);
					UrlConvention urlConvention = UrlConvention.ForUserSettingAndTypeContext(null, typeContext);
					ODataConventionalUriBuilder uriBuilder = new ODataConventionalUriBuilder(this.ServiceBaseUri, urlConvention);
					entryState.MetadataBuilder = new ODataConventionalEntityMetadataBuilder(entryMetadataContext, this, uriBuilder);
				}
				else
				{
					entryState.MetadataBuilder = new NoOpEntityMetadataBuilder(entry);
				}
			}
			return entryState.MetadataBuilder;
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00018AD4 File Offset: 0x00016CD4
		public IEdmFunctionImport[] GetAlwaysBindableOperationsForType(IEdmType bindingType)
		{
			IEdmFunctionImport[] array;
			if (!this.alwaysBindableOperationsCache.TryGetValue(bindingType, out array))
			{
				array = MetadataUtils.CalculateAlwaysBindableOperationsForType(bindingType, this.model, this.edmTypeResolver);
				this.alwaysBindableOperationsCache.Add(bindingType, array);
			}
			return array;
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00018B12 File Offset: 0x00016D12
		public bool OperationsBoundToEntityTypeMustBeContainerQualified(IEdmEntityType entityType)
		{
			return this.operationsBoundToEntityTypeMustBeContainerQualified(entityType);
		}

		// Token: 0x040002B5 RID: 693
		private readonly IEdmModel model;

		// Token: 0x040002B6 RID: 694
		private readonly EdmTypeResolver edmTypeResolver;

		// Token: 0x040002B7 RID: 695
		private readonly Dictionary<IEdmType, IEdmFunctionImport[]> alwaysBindableOperationsCache;

		// Token: 0x040002B8 RID: 696
		private readonly bool isResponse;

		// Token: 0x040002B9 RID: 697
		private readonly Func<IEdmEntityType, bool> operationsBoundToEntityTypeMustBeContainerQualified;

		// Token: 0x040002BA RID: 698
		private readonly Uri metadataDocumentUri;

		// Token: 0x040002BB RID: 699
		private Uri serviceBaseUri;
	}
}
