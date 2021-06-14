using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Globalization;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x02000267 RID: 615
	internal sealed class EpmSyndicationWriter : EpmWriter
	{
		// Token: 0x0600144B RID: 5195 RVA: 0x0004B6EB File Offset: 0x000498EB
		private EpmSyndicationWriter(EpmTargetTree epmTargetTree, ODataAtomOutputContext atomOutputContext) : base(atomOutputContext)
		{
			this.epmTargetTree = epmTargetTree;
			this.entryMetadata = new AtomEntryMetadata();
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x0004B708 File Offset: 0x00049908
		internal static AtomEntryMetadata WriteEntryEpm(EpmTargetTree epmTargetTree, EntryPropertiesValueCache epmValueCache, IEdmEntityTypeReference type, ODataAtomOutputContext atomOutputContext)
		{
			EpmSyndicationWriter epmSyndicationWriter = new EpmSyndicationWriter(epmTargetTree, atomOutputContext);
			return epmSyndicationWriter.WriteEntryEpm(epmValueCache, type);
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x0004B728 File Offset: 0x00049928
		private static AtomTextConstruct CreateAtomTextConstruct(string textValue, SyndicationTextContentKind contentKind)
		{
			AtomTextConstructKind kind;
			switch (contentKind)
			{
			case SyndicationTextContentKind.Plaintext:
				kind = AtomTextConstructKind.Text;
				break;
			case SyndicationTextContentKind.Html:
				kind = AtomTextConstructKind.Html;
				break;
			case SyndicationTextContentKind.Xhtml:
				kind = AtomTextConstructKind.Xhtml;
				break;
			default:
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.EpmSyndicationWriter_CreateAtomTextConstruct));
			}
			return new AtomTextConstruct
			{
				Kind = kind,
				Text = textValue
			};
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x0004B780 File Offset: 0x00049980
		private static DateTimeOffset CreateDateTimeValue(object propertyValue, SyndicationItemProperty targetProperty, ODataWriterBehavior writerBehavior)
		{
			if (propertyValue == null)
			{
				return DateTimeOffset.Now;
			}
			if (propertyValue is DateTimeOffset)
			{
				return (DateTimeOffset)propertyValue;
			}
			if (propertyValue is DateTime)
			{
				return new DateTimeOffset((DateTime)propertyValue);
			}
			string text = propertyValue as string;
			if (text == null)
			{
				DateTimeOffset result;
				try
				{
					result = new DateTimeOffset(Convert.ToDateTime(propertyValue, CultureInfo.InvariantCulture));
				}
				catch (Exception e)
				{
					if (!ExceptionUtils.IsCatchableExceptionType(e))
					{
						throw;
					}
					throw new ODataException(Strings.EpmSyndicationWriter_DateTimePropertyCanNotBeConverted(targetProperty.ToString()));
				}
				return result;
			}
			DateTimeOffset result2;
			if (DateTimeOffset.TryParse(text, out result2))
			{
				return result2;
			}
			DateTime dateTime;
			if (!DateTime.TryParse(text, out dateTime))
			{
				throw new ODataException(Strings.EpmSyndicationWriter_DateTimePropertyCanNotBeConverted(targetProperty.ToString()));
			}
			return new DateTimeOffset(dateTime);
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x0004B840 File Offset: 0x00049A40
		private static string CreateDateTimeStringValue(object propertyValue, ODataWriterBehavior writerBehavior)
		{
			if (propertyValue == null)
			{
				propertyValue = DateTimeOffset.Now;
			}
			if (propertyValue is DateTime)
			{
				propertyValue = new DateTimeOffset((DateTime)propertyValue);
			}
			if (propertyValue is DateTimeOffset)
			{
				return ODataAtomConvert.ToAtomString((DateTimeOffset)propertyValue);
			}
			return EpmWriterUtils.GetPropertyValueAsText(propertyValue);
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x0004B890 File Offset: 0x00049A90
		private AtomEntryMetadata WriteEntryEpm(EntryPropertiesValueCache epmValueCache, IEdmEntityTypeReference entityType)
		{
			EpmTargetPathSegment syndicationRoot = this.epmTargetTree.SyndicationRoot;
			if (syndicationRoot.SubSegments.Count == 0)
			{
				return null;
			}
			foreach (EpmTargetPathSegment epmTargetPathSegment in syndicationRoot.SubSegments)
			{
				if (epmTargetPathSegment.HasContent)
				{
					EntityPropertyMappingInfo epmInfo = epmTargetPathSegment.EpmInfo;
					object propertyValue = base.ReadEntryPropertyValue(epmInfo, epmValueCache, entityType);
					string propertyValueAsText = EpmWriterUtils.GetPropertyValueAsText(propertyValue);
					switch (epmInfo.Attribute.TargetSyndicationItem)
					{
					case SyndicationItemProperty.Updated:
						if (base.WriterBehavior.FormatBehaviorKind == ODataBehaviorKind.WcfDataServicesClient)
						{
							this.entryMetadata.UpdatedString = EpmSyndicationWriter.CreateDateTimeStringValue(propertyValue, base.WriterBehavior);
						}
						else
						{
							this.entryMetadata.Updated = new DateTimeOffset?(EpmSyndicationWriter.CreateDateTimeValue(propertyValue, SyndicationItemProperty.Updated, base.WriterBehavior));
						}
						break;
					case SyndicationItemProperty.Published:
						if (base.WriterBehavior.FormatBehaviorKind == ODataBehaviorKind.WcfDataServicesClient)
						{
							this.entryMetadata.PublishedString = EpmSyndicationWriter.CreateDateTimeStringValue(propertyValue, base.WriterBehavior);
						}
						else
						{
							this.entryMetadata.Published = new DateTimeOffset?(EpmSyndicationWriter.CreateDateTimeValue(propertyValue, SyndicationItemProperty.Published, base.WriterBehavior));
						}
						break;
					case SyndicationItemProperty.Rights:
						this.entryMetadata.Rights = EpmSyndicationWriter.CreateAtomTextConstruct(propertyValueAsText, epmInfo.Attribute.TargetTextContentKind);
						break;
					case SyndicationItemProperty.Summary:
						this.entryMetadata.Summary = EpmSyndicationWriter.CreateAtomTextConstruct(propertyValueAsText, epmInfo.Attribute.TargetTextContentKind);
						break;
					case SyndicationItemProperty.Title:
						this.entryMetadata.Title = EpmSyndicationWriter.CreateAtomTextConstruct(propertyValueAsText, epmInfo.Attribute.TargetTextContentKind);
						break;
					default:
						throw new ODataException(Strings.General_InternalError(InternalErrorCodes.EpmSyndicationWriter_WriteEntryEpm_ContentTarget));
					}
				}
				else
				{
					this.WriteParentSegment(epmTargetPathSegment, epmValueCache, entityType);
				}
			}
			return this.entryMetadata;
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x0004BA70 File Offset: 0x00049C70
		private void WriteParentSegment(EpmTargetPathSegment targetSegment, object epmValueCache, IEdmTypeReference typeReference)
		{
			if (targetSegment.SegmentName == "author")
			{
				AtomPersonMetadata atomPersonMetadata = this.WritePersonEpm(targetSegment, epmValueCache, typeReference);
				if (atomPersonMetadata != null)
				{
					List<AtomPersonMetadata> list = (List<AtomPersonMetadata>)this.entryMetadata.Authors;
					if (list == null)
					{
						list = new List<AtomPersonMetadata>();
						this.entryMetadata.Authors = list;
					}
					list.Add(atomPersonMetadata);
					return;
				}
			}
			else
			{
				if (!(targetSegment.SegmentName == "contributor"))
				{
					throw new ODataException(Strings.General_InternalError(InternalErrorCodes.EpmSyndicationWriter_WriteParentSegment_TargetSegmentName));
				}
				AtomPersonMetadata atomPersonMetadata2 = this.WritePersonEpm(targetSegment, epmValueCache, typeReference);
				if (atomPersonMetadata2 != null)
				{
					List<AtomPersonMetadata> list2 = (List<AtomPersonMetadata>)this.entryMetadata.Contributors;
					if (list2 == null)
					{
						list2 = new List<AtomPersonMetadata>();
						this.entryMetadata.Contributors = list2;
					}
					list2.Add(atomPersonMetadata2);
					return;
				}
			}
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x0004BB2C File Offset: 0x00049D2C
		private AtomPersonMetadata WritePersonEpm(EpmTargetPathSegment targetSegment, object epmValueCache, IEdmTypeReference typeReference)
		{
			AtomPersonMetadata atomPersonMetadata = null;
			foreach (EpmTargetPathSegment epmTargetPathSegment in targetSegment.SubSegments)
			{
				string propertyValueAsText = this.GetPropertyValueAsText(epmTargetPathSegment, epmValueCache, typeReference);
				if (propertyValueAsText != null)
				{
					switch (epmTargetPathSegment.EpmInfo.Attribute.TargetSyndicationItem)
					{
					case SyndicationItemProperty.AuthorEmail:
					case SyndicationItemProperty.ContributorEmail:
						if (propertyValueAsText != null && propertyValueAsText.Length > 0)
						{
							if (atomPersonMetadata == null)
							{
								atomPersonMetadata = new AtomPersonMetadata();
							}
							atomPersonMetadata.Email = propertyValueAsText;
						}
						break;
					case SyndicationItemProperty.AuthorName:
					case SyndicationItemProperty.ContributorName:
						if (propertyValueAsText != null)
						{
							if (atomPersonMetadata == null)
							{
								atomPersonMetadata = new AtomPersonMetadata();
							}
							atomPersonMetadata.Name = propertyValueAsText;
						}
						break;
					case SyndicationItemProperty.AuthorUri:
					case SyndicationItemProperty.ContributorUri:
						if (propertyValueAsText != null && propertyValueAsText.Length > 0)
						{
							if (atomPersonMetadata == null)
							{
								atomPersonMetadata = new AtomPersonMetadata();
							}
							atomPersonMetadata.UriFromEpm = propertyValueAsText;
						}
						break;
					default:
						throw new ODataException(Strings.General_InternalError(InternalErrorCodes.EpmSyndicationWriter_WritePersonEpm));
					}
				}
			}
			return atomPersonMetadata;
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x0004BC2C File Offset: 0x00049E2C
		private string GetPropertyValueAsText(EpmTargetPathSegment targetSegment, object epmValueCache, IEdmTypeReference typeReference)
		{
			EntryPropertiesValueCache entryPropertiesValueCache = epmValueCache as EntryPropertiesValueCache;
			object obj;
			if (entryPropertiesValueCache != null)
			{
				obj = base.ReadEntryPropertyValue(targetSegment.EpmInfo, entryPropertiesValueCache, typeReference.AsEntity());
			}
			else
			{
				obj = epmValueCache;
				ValidationUtils.ValidateIsExpectedPrimitiveType(obj, typeReference);
			}
			return EpmWriterUtils.GetPropertyValueAsText(obj);
		}

		// Token: 0x0400072D RID: 1837
		private readonly EpmTargetTree epmTargetTree;

		// Token: 0x0400072E RID: 1838
		private readonly AtomEntryMetadata entryMetadata;
	}
}
