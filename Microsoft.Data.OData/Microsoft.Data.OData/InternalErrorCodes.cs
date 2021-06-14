using System;

namespace Microsoft.Data.OData
{
	// Token: 0x02000290 RID: 656
	internal enum InternalErrorCodes
	{
		// Token: 0x0400086A RID: 2154
		ODataWriterCore_WriteEnd_UnreachableCodePath,
		// Token: 0x0400086B RID: 2155
		ODataWriterCore_ValidateTransition_UnreachableCodePath,
		// Token: 0x0400086C RID: 2156
		ODataWriterCore_Scope_Create_UnreachableCodePath,
		// Token: 0x0400086D RID: 2157
		ODataWriterCore_DuplicatePropertyNamesChecker,
		// Token: 0x0400086E RID: 2158
		ODataWriterCore_ParentNavigationLinkScope,
		// Token: 0x0400086F RID: 2159
		ODataUtils_VersionString_UnreachableCodePath,
		// Token: 0x04000870 RID: 2160
		ODataUtilsInternal_ToDataServiceVersion_UnreachableCodePath,
		// Token: 0x04000871 RID: 2161
		ODataUtilsInternal_IsPayloadKindSupported_UnreachableCodePath,
		// Token: 0x04000872 RID: 2162
		ODataUtils_GetDefaultEncoding_UnreachableCodePath,
		// Token: 0x04000873 RID: 2163
		ODataUtils_ParseSerializableEpmAnnotations_UnreachableCodePath,
		// Token: 0x04000874 RID: 2164
		ODataMessageWriter_WriteProperty,
		// Token: 0x04000875 RID: 2165
		ODataMessageWriter_WriteEntityReferenceLink,
		// Token: 0x04000876 RID: 2166
		ODataMessageWriter_WriteEntityReferenceLinks,
		// Token: 0x04000877 RID: 2167
		ODataMessageWriter_WriteError,
		// Token: 0x04000878 RID: 2168
		ODataMessageWriter_WriteServiceDocument,
		// Token: 0x04000879 RID: 2169
		ODataMessageWriter_WriteMetadataDocument,
		// Token: 0x0400087A RID: 2170
		EpmSyndicationWriter_WriteEntryEpm_ContentTarget,
		// Token: 0x0400087B RID: 2171
		EpmSyndicationWriter_CreateAtomTextConstruct,
		// Token: 0x0400087C RID: 2172
		EpmSyndicationWriter_WritePersonEpm,
		// Token: 0x0400087D RID: 2173
		EpmSyndicationWriter_WriteParentSegment_TargetSegmentName,
		// Token: 0x0400087E RID: 2174
		ODataAtomConvert_ToString,
		// Token: 0x0400087F RID: 2175
		ODataCollectionWriter_CreateCollectionWriter_UnreachableCodePath,
		// Token: 0x04000880 RID: 2176
		ODataCollectionWriterCore_ValidateTransition_UnreachableCodePath,
		// Token: 0x04000881 RID: 2177
		ODataCollectionWriterCore_WriteEnd_UnreachableCodePath,
		// Token: 0x04000882 RID: 2178
		ODataParameterWriter_CannotCreateParameterWriterForFormat,
		// Token: 0x04000883 RID: 2179
		ODataParameterWriterCore_ValidateTransition_InvalidTransitionFromStart,
		// Token: 0x04000884 RID: 2180
		ODataParameterWriterCore_ValidateTransition_InvalidTransitionFromCanWriteParameter,
		// Token: 0x04000885 RID: 2181
		ODataParameterWriterCore_ValidateTransition_InvalidTransitionFromActiveSubWriter,
		// Token: 0x04000886 RID: 2182
		ODataParameterWriterCore_ValidateTransition_InvalidTransitionFromCompleted,
		// Token: 0x04000887 RID: 2183
		ODataParameterWriterCore_ValidateTransition_InvalidTransitionFromError,
		// Token: 0x04000888 RID: 2184
		ODataParameterWriterCore_ValidateTransition_UnreachableCodePath,
		// Token: 0x04000889 RID: 2185
		ODataParameterWriterCore_WriteEndImplementation_UnreachableCodePath,
		// Token: 0x0400088A RID: 2186
		QueryPathValidator_ValidateSegment_Root,
		// Token: 0x0400088B RID: 2187
		QueryPathValidator_ValidateSegment_NonRoot,
		// Token: 0x0400088C RID: 2188
		ODataBatchWriter_ValidateTransition_UnreachableCodePath,
		// Token: 0x0400088D RID: 2189
		ODataBatchWriterUtils_HttpMethod_ToText_UnreachableCodePath,
		// Token: 0x0400088E RID: 2190
		ODataBatchReader_ReadImplementation,
		// Token: 0x0400088F RID: 2191
		ODataBatchReader_GetEndBoundary_Completed,
		// Token: 0x04000890 RID: 2192
		ODataBatchReader_GetEndBoundary_Exception,
		// Token: 0x04000891 RID: 2193
		ODataBatchReader_GetEndBoundary_UnknownValue,
		// Token: 0x04000892 RID: 2194
		ODataBatchReaderStream_SkipToBoundary,
		// Token: 0x04000893 RID: 2195
		ODataBatchReaderStream_ReadLine,
		// Token: 0x04000894 RID: 2196
		ODataBatchReaderStream_ReadWithDelimiter,
		// Token: 0x04000895 RID: 2197
		ODataBatchReaderStreamBuffer_ScanForBoundary,
		// Token: 0x04000896 RID: 2198
		ODataBatchReaderStreamBuffer_ReadWithLength,
		// Token: 0x04000897 RID: 2199
		JsonReader_Read,
		// Token: 0x04000898 RID: 2200
		ODataReader_CreateReader_UnreachableCodePath,
		// Token: 0x04000899 RID: 2201
		ODataReaderCore_ReadImplementation,
		// Token: 0x0400089A RID: 2202
		ODataReaderCoreAsync_ReadAsynchronously,
		// Token: 0x0400089B RID: 2203
		ODataVerboseJsonEntryAndFeedDeserializer_ReadFeedProperty,
		// Token: 0x0400089C RID: 2204
		ODataVerboseJsonReader_ReadEntryStart,
		// Token: 0x0400089D RID: 2205
		ODataVerboseJsonPropertyAndValueDeserializer_ReadPropertyValue,
		// Token: 0x0400089E RID: 2206
		ODataCollectionReader_CreateReader_UnreachableCodePath,
		// Token: 0x0400089F RID: 2207
		ODataCollectionReaderCore_ReadImplementation,
		// Token: 0x040008A0 RID: 2208
		ODataCollectionReaderCoreAsync_ReadAsynchronously,
		// Token: 0x040008A1 RID: 2209
		ODataParameterReaderCore_ReadImplementation,
		// Token: 0x040008A2 RID: 2210
		ODataParameterReaderCoreAsync_ReadAsynchronously,
		// Token: 0x040008A3 RID: 2211
		ODataParameterReaderCore_ValueMustBePrimitiveOrComplexOrNull,
		// Token: 0x040008A4 RID: 2212
		ODataAtomReader_ReadAtNavigationLinkStartImplementation,
		// Token: 0x040008A5 RID: 2213
		ODataAtomPropertyAndValueDeserializer_ReadNonEntityValue,
		// Token: 0x040008A6 RID: 2214
		AtomValueUtils_ConvertStringToPrimitive,
		// Token: 0x040008A7 RID: 2215
		EdmCoreModel_PrimitiveType,
		// Token: 0x040008A8 RID: 2216
		EpmSyndicationReader_ReadEntryEpm_ContentTarget,
		// Token: 0x040008A9 RID: 2217
		EpmSyndicationReader_ReadParentSegment_TargetSegmentName,
		// Token: 0x040008AA RID: 2218
		EpmSyndicationReader_ReadPersonEpm,
		// Token: 0x040008AB RID: 2219
		EpmReader_SetEpmValueForSegment_TypeKind,
		// Token: 0x040008AC RID: 2220
		EpmReader_SetEpmValueForSegment_StreamProperty,
		// Token: 0x040008AD RID: 2221
		ReaderValidationUtils_ResolveAndValidateTypeName_Strict_TypeKind,
		// Token: 0x040008AE RID: 2222
		ReaderValidationUtils_ResolveAndValidateTypeName_Lax_TypeKind,
		// Token: 0x040008AF RID: 2223
		EpmExtensionMethods_ToAttributeValue_SyndicationItemProperty,
		// Token: 0x040008B0 RID: 2224
		ODataMetadataFormat_CreateOutputContextAsync,
		// Token: 0x040008B1 RID: 2225
		ODataMetadataFormat_CreateInputContextAsync,
		// Token: 0x040008B2 RID: 2226
		ODataModelFunctions_UnsupportedMethodOrProperty,
		// Token: 0x040008B3 RID: 2227
		ODataJsonLightPropertyAndValueDeserializer_ReadPropertyValue,
		// Token: 0x040008B4 RID: 2228
		ODataJsonLightPropertyAndValueDeserializer_GetNonEntityValueKind,
		// Token: 0x040008B5 RID: 2229
		ODataJsonLightEntryAndFeedDeserializer_ReadFeedProperty,
		// Token: 0x040008B6 RID: 2230
		ODataJsonLightReader_ReadEntryStart,
		// Token: 0x040008B7 RID: 2231
		ODataJsonLightEntryAndFeedDeserializer_ReadTopLevelFeedAnnotations,
		// Token: 0x040008B8 RID: 2232
		ODataJsonLightReader_ReadFeedEnd,
		// Token: 0x040008B9 RID: 2233
		ODataJsonLightCollectionDeserializer_ReadCollectionStart,
		// Token: 0x040008BA RID: 2234
		ODataJsonLightCollectionDeserializer_ReadCollectionStart_TypeKindFromPayloadFunc,
		// Token: 0x040008BB RID: 2235
		ODataJsonLightCollectionDeserializer_ReadCollectionEnd,
		// Token: 0x040008BC RID: 2236
		ODataJsonLightEntityReferenceLinkDeserializer_ReadSingleEntityReferenceLink,
		// Token: 0x040008BD RID: 2237
		ODataJsonLightEntityReferenceLinkDeserializer_ReadEntityReferenceLinksAnnotations,
		// Token: 0x040008BE RID: 2238
		ODataJsonLightParameterDeserializer_ReadNextParameter,
		// Token: 0x040008BF RID: 2239
		ODataJsonLightAnnotationGroupDeserializer_ReadAnnotationGroupDeclaration,
		// Token: 0x040008C0 RID: 2240
		EdmTypeWriterResolver_GetReturnTypeForFunctionImportGroup,
		// Token: 0x040008C1 RID: 2241
		ODataVersionCache_UnknownVersion
	}
}
