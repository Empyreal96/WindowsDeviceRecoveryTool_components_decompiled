using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x0200018E RID: 398
	internal sealed class ODataJsonLightEntryAndFeedSerializer : ODataJsonLightPropertySerializer
	{
		// Token: 0x06000B64 RID: 2916 RVA: 0x000276E7 File Offset: 0x000258E7
		internal ODataJsonLightEntryAndFeedSerializer(ODataJsonLightOutputContext jsonLightOutputContext) : base(jsonLightOutputContext)
		{
			this.annotationGroups = new Dictionary<string, ODataJsonLightAnnotationGroup>(StringComparer.Ordinal);
			this.metadataUriBuilder = jsonLightOutputContext.CreateMetadataUriBuilder();
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000B65 RID: 2917 RVA: 0x0002770C File Offset: 0x0002590C
		private Uri MetadataDocumentBaseUri
		{
			get
			{
				return this.metadataUriBuilder.BaseUri;
			}
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x0002771C File Offset: 0x0002591C
		internal void WriteAnnotationGroup(ODataEntry entry)
		{
			ODataJsonLightAnnotationGroup annotation = entry.GetAnnotation<ODataJsonLightAnnotationGroup>();
			if (annotation == null)
			{
				return;
			}
			if (!base.JsonLightOutputContext.WritingResponse)
			{
				throw new ODataException(Strings.ODataJsonLightEntryAndFeedSerializer_AnnotationGroupInRequest);
			}
			string name = annotation.Name;
			if (string.IsNullOrEmpty(name))
			{
				throw new ODataException(Strings.ODataJsonLightEntryAndFeedSerializer_AnnotationGroupWithoutName);
			}
			ODataJsonLightAnnotationGroup objA;
			if (!this.annotationGroups.TryGetValue(name, out objA))
			{
				base.JsonWriter.WriteName("odata.annotationGroup");
				base.JsonWriter.StartObjectScope();
				base.JsonWriter.WriteName("name");
				base.JsonWriter.WritePrimitiveValue(name, base.JsonLightOutputContext.Version);
				if (annotation.Annotations != null)
				{
					foreach (KeyValuePair<string, object> keyValuePair in annotation.Annotations)
					{
						string key = keyValuePair.Key;
						if (key.Length == 0)
						{
							throw new ODataException(Strings.ODataJsonLightEntryAndFeedSerializer_AnnotationGroupMemberWithoutName(annotation.Name));
						}
						if (!ODataJsonLightReaderUtils.IsAnnotationProperty(key))
						{
							throw new ODataException(Strings.ODataJsonLightEntryAndFeedSerializer_AnnotationGroupMemberMustBeAnnotation(key, annotation.Name));
						}
						base.JsonWriter.WriteName(key);
						object value = keyValuePair.Value;
						string text = value as string;
						if (text == null && value != null)
						{
							throw new ODataException(Strings.ODataJsonLightEntryAndFeedSerializer_AnnotationGroupMemberWithInvalidValue(key, annotation.Name, value.GetType().FullName));
						}
						base.JsonWriter.WritePrimitiveValue(text, base.JsonLightOutputContext.Version);
					}
				}
				base.JsonWriter.EndObjectScope();
				this.annotationGroups.Add(name, annotation);
				return;
			}
			if (!object.ReferenceEquals(objA, annotation))
			{
				throw new ODataException(Strings.ODataJsonLightEntryAndFeedSerializer_DuplicateAnnotationGroup(name));
			}
			base.JsonWriter.WriteName("odata.annotationGroupReference");
			base.JsonWriter.WritePrimitiveValue(name, base.JsonLightOutputContext.Version);
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x000278FC File Offset: 0x00025AFC
		internal void WriteEntryStartMetadataProperties(IODataJsonLightWriterEntryState entryState)
		{
			ODataEntry entry = entryState.Entry;
			string entryTypeNameForWriting = base.JsonLightOutputContext.TypeNameOracle.GetEntryTypeNameForWriting(entryState.GetOrCreateTypeContext(base.Model, base.WritingResponse).ExpectedEntityTypeName, entry);
			if (entryTypeNameForWriting != null)
			{
				base.JsonWriter.WriteName("odata.type");
				base.JsonWriter.WriteValue(entryTypeNameForWriting);
			}
			string id = entry.Id;
			if (id != null)
			{
				base.JsonWriter.WriteName("odata.id");
				base.JsonWriter.WriteValue(id);
			}
			string etag = entry.ETag;
			if (etag != null)
			{
				base.JsonWriter.WriteName("odata.etag");
				base.JsonWriter.WriteValue(etag);
			}
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x000279A4 File Offset: 0x00025BA4
		internal void WriteEntryMetadataProperties(IODataJsonLightWriterEntryState entryState)
		{
			ODataEntry entry = entryState.Entry;
			Uri editLink = entry.EditLink;
			if (editLink != null && !entryState.EditLinkWritten)
			{
				base.JsonWriter.WriteName("odata.editLink");
				base.JsonWriter.WriteValue(base.UriToString(editLink));
				entryState.EditLinkWritten = true;
			}
			Uri readLink = entry.ReadLink;
			if (readLink != null && !entryState.ReadLinkWritten)
			{
				base.JsonWriter.WriteName("odata.readLink");
				base.JsonWriter.WriteValue(base.UriToString(readLink));
				entryState.ReadLinkWritten = true;
			}
			ODataStreamReferenceValue mediaResource = entry.MediaResource;
			if (mediaResource != null)
			{
				Uri editLink2 = mediaResource.EditLink;
				if (editLink2 != null && !entryState.MediaEditLinkWritten)
				{
					base.JsonWriter.WriteName("odata.mediaEditLink");
					base.JsonWriter.WriteValue(base.UriToString(editLink2));
					entryState.MediaEditLinkWritten = true;
				}
				Uri readLink2 = mediaResource.ReadLink;
				if (readLink2 != null && !entryState.MediaReadLinkWritten)
				{
					base.JsonWriter.WriteName("odata.mediaReadLink");
					base.JsonWriter.WriteValue(base.UriToString(readLink2));
					entryState.MediaReadLinkWritten = true;
				}
				string contentType = mediaResource.ContentType;
				if (contentType != null && !entryState.MediaContentTypeWritten)
				{
					base.JsonWriter.WriteName("odata.mediaContentType");
					base.JsonWriter.WriteValue(contentType);
					entryState.MediaContentTypeWritten = true;
				}
				string etag = mediaResource.ETag;
				if (etag != null && !entryState.MediaETagWritten)
				{
					base.JsonWriter.WriteName("odata.mediaETag");
					base.JsonWriter.WriteValue(etag);
					entryState.MediaETagWritten = true;
				}
			}
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x00027B40 File Offset: 0x00025D40
		internal void WriteEntryEndMetadataProperties(IODataJsonLightWriterEntryState entryState, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker)
		{
			ODataEntry entry = entryState.Entry;
			for (ODataJsonLightReaderNavigationLinkInfo nextUnprocessedNavigationLink = entry.MetadataBuilder.GetNextUnprocessedNavigationLink(); nextUnprocessedNavigationLink != null; nextUnprocessedNavigationLink = entry.MetadataBuilder.GetNextUnprocessedNavigationLink())
			{
				nextUnprocessedNavigationLink.NavigationLink.SetMetadataBuilder(entry.MetadataBuilder);
				this.WriteNavigationLinkMetadata(nextUnprocessedNavigationLink.NavigationLink, duplicatePropertyNamesChecker);
			}
			IEnumerable<ODataAction> actions = entry.Actions;
			if (actions != null)
			{
				this.WriteOperations(actions.Cast<ODataOperation>(), true);
			}
			IEnumerable<ODataFunction> functions = entry.Functions;
			if (functions != null)
			{
				this.WriteOperations(functions.Cast<ODataOperation>(), false);
			}
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x00027BC0 File Offset: 0x00025DC0
		internal void WriteNavigationLinkMetadata(ODataNavigationLink navigationLink, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker)
		{
			Uri url = navigationLink.Url;
			string name = navigationLink.Name;
			if (url != null)
			{
				base.JsonWriter.WritePropertyAnnotationName(name, "odata.navigationLinkUrl");
				base.JsonWriter.WriteValue(base.UriToString(url));
			}
			Uri associationLinkUrl = navigationLink.AssociationLinkUrl;
			if (associationLinkUrl != null)
			{
				duplicatePropertyNamesChecker.CheckForDuplicateAssociationLinkNames(new ODataAssociationLink
				{
					Name = name
				});
				this.WriteAssociationLink(navigationLink.Name, associationLinkUrl);
			}
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x00027C90 File Offset: 0x00025E90
		internal void WriteOperations(IEnumerable<ODataOperation> operations, bool isAction)
		{
			IEnumerable<IGrouping<string, ODataOperation>> enumerable = operations.GroupBy(delegate(ODataOperation o)
			{
				ValidationUtils.ValidateOperationNotNull(o, isAction);
				WriterValidationUtils.ValidateCanWriteOperation(o, this.JsonLightOutputContext.WritingResponse);
				ODataJsonLightValidationUtils.ValidateOperation(this.MetadataDocumentBaseUri, o);
				return this.GetOperationMetadataString(o);
			});
			foreach (IGrouping<string, ODataOperation> operations2 in enumerable)
			{
				this.WriteOperationMetadataGroup(operations2);
			}
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x00027D00 File Offset: 0x00025F00
		internal void TryWriteEntryMetadataUri(ODataFeedAndEntryTypeContext typeContext)
		{
			Uri metadataUri;
			if (this.metadataUriBuilder.TryBuildEntryMetadataUri(typeContext, out metadataUri))
			{
				base.WriteMetadataUriProperty(metadataUri);
			}
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x00027D24 File Offset: 0x00025F24
		internal void TryWriteFeedMetadataUri(ODataFeedAndEntryTypeContext typeContext)
		{
			Uri metadataUri;
			if (this.metadataUriBuilder.TryBuildFeedMetadataUri(typeContext, out metadataUri))
			{
				base.WriteMetadataUriProperty(metadataUri);
			}
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x00027D48 File Offset: 0x00025F48
		private void WriteAssociationLink(string propertyName, Uri associationLinkUrl)
		{
			base.JsonWriter.WritePropertyAnnotationName(propertyName, "odata.associationLinkUrl");
			base.JsonWriter.WriteValue(base.UriToString(associationLinkUrl));
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x00027D70 File Offset: 0x00025F70
		private string GetOperationMetadataString(ODataOperation operation)
		{
			string propertyName = UriUtilsCommon.UriToString(operation.Metadata);
			if (this.MetadataDocumentBaseUri == null)
			{
				return operation.Metadata.Fragment;
			}
			return '#' + ODataJsonLightUtils.GetUriFragmentFromMetadataReferencePropertyName(this.MetadataDocumentBaseUri, propertyName);
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x00027DBB File Offset: 0x00025FBB
		private string GetOperationTargetUriString(ODataOperation operation)
		{
			if (!(operation.Target == null))
			{
				return base.UriToString(operation.Target);
			}
			return null;
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x00027DE8 File Offset: 0x00025FE8
		private void ValidateOperationMetadataGroup(IGrouping<string, ODataOperation> operations)
		{
			if (operations.Count<ODataOperation>() > 1)
			{
				if (operations.Any((ODataOperation o) => o.Target == null))
				{
					throw new ODataException(Strings.ODataJsonLightEntryAndFeedSerializer_ActionsAndFunctionsGroupMustSpecifyTarget(operations.Key));
				}
			}
			foreach (IGrouping<string, ODataOperation> grouping in operations.GroupBy(new Func<ODataOperation, string>(this.GetOperationTargetUriString)))
			{
				if (grouping.Count<ODataOperation>() > 1)
				{
					throw new ODataException(Strings.ODataJsonLightEntryAndFeedSerializer_ActionsAndFunctionsGroupMustNotHaveDuplicateTarget(operations.Key, grouping.Key));
				}
			}
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x00027E9C File Offset: 0x0002609C
		private void WriteOperationMetadataGroup(IGrouping<string, ODataOperation> operations)
		{
			this.ValidateOperationMetadataGroup(operations);
			base.JsonLightOutputContext.JsonWriter.WriteName(operations.Key);
			bool flag = operations.Count<ODataOperation>() > 1;
			if (flag)
			{
				base.JsonLightOutputContext.JsonWriter.StartArrayScope();
			}
			foreach (ODataOperation operation in operations)
			{
				this.WriteOperation(operation);
			}
			if (flag)
			{
				base.JsonLightOutputContext.JsonWriter.EndArrayScope();
			}
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x00027F34 File Offset: 0x00026134
		private void WriteOperation(ODataOperation operation)
		{
			base.JsonLightOutputContext.JsonWriter.StartObjectScope();
			if (operation.Title != null)
			{
				base.JsonLightOutputContext.JsonWriter.WriteName("title");
				base.JsonLightOutputContext.JsonWriter.WriteValue(operation.Title);
			}
			if (operation.Target != null)
			{
				string operationTargetUriString = this.GetOperationTargetUriString(operation);
				base.JsonLightOutputContext.JsonWriter.WriteName("target");
				base.JsonLightOutputContext.JsonWriter.WriteValue(operationTargetUriString);
			}
			base.JsonLightOutputContext.JsonWriter.EndObjectScope();
		}

		// Token: 0x04000419 RID: 1049
		private readonly Dictionary<string, ODataJsonLightAnnotationGroup> annotationGroups;

		// Token: 0x0400041A RID: 1050
		private readonly ODataJsonLightMetadataUriBuilder metadataUriBuilder;
	}
}
