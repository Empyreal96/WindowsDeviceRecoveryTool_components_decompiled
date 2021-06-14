using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.VerboseJson
{
	// Token: 0x020001C5 RID: 453
	internal sealed class ODataVerboseJsonEntryAndFeedSerializer : ODataVerboseJsonPropertyAndValueSerializer
	{
		// Token: 0x06000DFB RID: 3579 RVA: 0x00031020 File Offset: 0x0002F220
		internal ODataVerboseJsonEntryAndFeedSerializer(ODataVerboseJsonOutputContext verboseJsonOutputContext) : base(verboseJsonOutputContext)
		{
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x0003102C File Offset: 0x0002F22C
		internal void WriteEntryMetadata(ODataEntry entry, ProjectedPropertiesAnnotation projectedProperties, IEdmEntityType entryEntityType, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker)
		{
			base.JsonWriter.WriteName("__metadata");
			base.JsonWriter.StartObjectScope();
			string id = entry.Id;
			if (id != null)
			{
				base.JsonWriter.WriteName("id");
				base.JsonWriter.WriteValue(id);
			}
			Uri uri = entry.EditLink ?? entry.ReadLink;
			if (uri != null)
			{
				base.JsonWriter.WriteName("uri");
				base.JsonWriter.WriteValue(base.UriToAbsoluteUriString(uri));
			}
			string etag = entry.ETag;
			if (etag != null)
			{
				base.WriteETag("etag", etag);
			}
			string entryTypeNameForWriting = base.VerboseJsonOutputContext.TypeNameOracle.GetEntryTypeNameForWriting(entry);
			if (entryTypeNameForWriting != null)
			{
				base.JsonWriter.WriteName("type");
				base.JsonWriter.WriteValue(entryTypeNameForWriting);
			}
			ODataStreamReferenceValue mediaResource = entry.MediaResource;
			if (mediaResource != null)
			{
				WriterValidationUtils.ValidateStreamReferenceValue(mediaResource, true);
				base.WriteStreamReferenceValueContent(mediaResource);
			}
			IEnumerable<ODataAction> actions = entry.Actions;
			if (actions != null)
			{
				this.WriteOperations(actions.Cast<ODataOperation>(), "actions", true, false);
			}
			IEnumerable<ODataFunction> functions = entry.Functions;
			if (functions != null)
			{
				this.WriteOperations(functions.Cast<ODataOperation>(), "functions", false, false);
			}
			IEnumerable<ODataAssociationLink> associationLinks = entry.AssociationLinks;
			if (associationLinks != null)
			{
				bool flag = true;
				foreach (ODataAssociationLink odataAssociationLink in associationLinks)
				{
					ValidationUtils.ValidateAssociationLinkNotNull(odataAssociationLink);
					if (!projectedProperties.ShouldSkipProperty(odataAssociationLink.Name))
					{
						if (flag)
						{
							base.JsonWriter.WriteName("properties");
							base.JsonWriter.StartObjectScope();
							flag = false;
						}
						base.ValidateAssociationLink(odataAssociationLink, entryEntityType);
						this.WriteAssociationLink(odataAssociationLink, duplicatePropertyNamesChecker);
					}
				}
				if (!flag)
				{
					base.JsonWriter.EndObjectScope();
				}
			}
			base.JsonWriter.EndObjectScope();
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0003126C File Offset: 0x0002F46C
		internal void WriteOperations(IEnumerable<ODataOperation> operations, string operationName, bool isAction, bool writingJsonLight)
		{
			bool flag = true;
			IEnumerable<IGrouping<string, ODataOperation>> enumerable = operations.GroupBy(delegate(ODataOperation o)
			{
				ValidationUtils.ValidateOperationNotNull(o, isAction);
				WriterValidationUtils.ValidateCanWriteOperation(o, this.VerboseJsonOutputContext.WritingResponse);
				ValidationUtils.ValidateOperationMetadataNotNull(o);
				if (!writingJsonLight)
				{
					ValidationUtils.ValidateOperationTargetNotNull(o);
				}
				return this.UriToUriString(o.Metadata, false);
			});
			foreach (IGrouping<string, ODataOperation> operations2 in enumerable)
			{
				if (flag)
				{
					base.VerboseJsonOutputContext.JsonWriter.WriteName(operationName);
					base.VerboseJsonOutputContext.JsonWriter.StartObjectScope();
					flag = false;
				}
				this.WriteOperationMetadataGroup(operations2);
			}
			if (!flag)
			{
				base.VerboseJsonOutputContext.JsonWriter.EndObjectScope();
			}
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x00031324 File Offset: 0x0002F524
		private void WriteAssociationLink(ODataAssociationLink associationLink, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker)
		{
			duplicatePropertyNamesChecker.CheckForDuplicateAssociationLinkNames(associationLink);
			base.JsonWriter.WriteName(associationLink.Name);
			base.JsonWriter.StartObjectScope();
			base.JsonWriter.WriteName("associationuri");
			base.JsonWriter.WriteValue(base.UriToAbsoluteUriString(associationLink.Url));
			base.JsonWriter.EndObjectScope();
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x00031388 File Offset: 0x0002F588
		private void WriteOperationMetadataGroup(IGrouping<string, ODataOperation> operations)
		{
			bool flag = true;
			foreach (ODataOperation operation in operations)
			{
				if (flag)
				{
					base.VerboseJsonOutputContext.JsonWriter.WriteName(operations.Key);
					base.VerboseJsonOutputContext.JsonWriter.StartArrayScope();
					flag = false;
				}
				this.WriteOperation(operation);
			}
			base.VerboseJsonOutputContext.JsonWriter.EndArrayScope();
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x00031410 File Offset: 0x0002F610
		private void WriteOperation(ODataOperation operation)
		{
			base.VerboseJsonOutputContext.JsonWriter.StartObjectScope();
			if (operation.Title != null)
			{
				base.VerboseJsonOutputContext.JsonWriter.WriteName("title");
				base.VerboseJsonOutputContext.JsonWriter.WriteValue(operation.Title);
			}
			if (operation.Target != null)
			{
				string value = base.UriToAbsoluteUriString(operation.Target);
				base.VerboseJsonOutputContext.JsonWriter.WriteName("target");
				base.VerboseJsonOutputContext.JsonWriter.WriteValue(value);
			}
			base.VerboseJsonOutputContext.JsonWriter.EndObjectScope();
		}
	}
}
