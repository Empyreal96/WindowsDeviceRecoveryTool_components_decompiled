using System;
using System.Data.Services.Common;
using System.Linq;
using System.Xml;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x02000214 RID: 532
	internal sealed class EpmSyndicationReader : EpmReader
	{
		// Token: 0x06001069 RID: 4201 RVA: 0x0003BEE8 File Offset: 0x0003A0E8
		private EpmSyndicationReader(IODataAtomReaderEntryState entryState, ODataAtomInputContext inputContext) : base(entryState, inputContext)
		{
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x0003BEF4 File Offset: 0x0003A0F4
		internal static void ReadEntryEpm(IODataAtomReaderEntryState entryState, ODataAtomInputContext inputContext)
		{
			EpmSyndicationReader epmSyndicationReader = new EpmSyndicationReader(entryState, inputContext);
			epmSyndicationReader.ReadEntryEpm();
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x0003BF10 File Offset: 0x0003A110
		private void ReadEntryEpm()
		{
			AtomEntryMetadata atomEntryMetadata = base.EntryState.AtomEntryMetadata;
			EpmTargetPathSegment syndicationRoot = base.EntryState.CachedEpm.EpmTargetTree.SyndicationRoot;
			if (syndicationRoot.SubSegments.Count == 0)
			{
				return;
			}
			foreach (EpmTargetPathSegment epmTargetPathSegment in syndicationRoot.SubSegments)
			{
				if (epmTargetPathSegment.HasContent)
				{
					this.ReadPropertyValueSegment(epmTargetPathSegment, atomEntryMetadata);
				}
				else
				{
					this.ReadParentSegment(epmTargetPathSegment, atomEntryMetadata);
				}
			}
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x0003BFA8 File Offset: 0x0003A1A8
		private void ReadPropertyValueSegment(EpmTargetPathSegment targetSegment, AtomEntryMetadata entryMetadata)
		{
			EntityPropertyMappingInfo epmInfo = targetSegment.EpmInfo;
			switch (epmInfo.Attribute.TargetSyndicationItem)
			{
			case SyndicationItemProperty.Updated:
				if (base.MessageReaderSettings.ReaderBehavior.FormatBehaviorKind == ODataBehaviorKind.WcfDataServicesClient)
				{
					if (entryMetadata.UpdatedString != null)
					{
						base.SetEntryEpmValue(targetSegment.EpmInfo, entryMetadata.UpdatedString);
						return;
					}
				}
				else if (entryMetadata.Updated != null)
				{
					base.SetEntryEpmValue(targetSegment.EpmInfo, XmlConvert.ToString(entryMetadata.Updated.Value));
					return;
				}
				break;
			case SyndicationItemProperty.Published:
				if (base.MessageReaderSettings.ReaderBehavior.FormatBehaviorKind == ODataBehaviorKind.WcfDataServicesClient)
				{
					if (entryMetadata.PublishedString != null)
					{
						base.SetEntryEpmValue(targetSegment.EpmInfo, entryMetadata.PublishedString);
						return;
					}
				}
				else if (entryMetadata.Published != null)
				{
					base.SetEntryEpmValue(targetSegment.EpmInfo, XmlConvert.ToString(entryMetadata.Published.Value));
					return;
				}
				break;
			case SyndicationItemProperty.Rights:
				this.ReadTextConstructEpm(targetSegment, entryMetadata.Rights);
				return;
			case SyndicationItemProperty.Summary:
				this.ReadTextConstructEpm(targetSegment, entryMetadata.Summary);
				return;
			case SyndicationItemProperty.Title:
				this.ReadTextConstructEpm(targetSegment, entryMetadata.Title);
				return;
			default:
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.EpmSyndicationReader_ReadEntryEpm_ContentTarget));
			}
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x0003C0EC File Offset: 0x0003A2EC
		private void ReadParentSegment(EpmTargetPathSegment targetSegment, AtomEntryMetadata entryMetadata)
		{
			string segmentName;
			if ((segmentName = targetSegment.SegmentName) != null)
			{
				if (!(segmentName == "author"))
				{
					if (!(segmentName == "contributor"))
					{
						goto IL_B3;
					}
					AtomPersonMetadata atomPersonMetadata = entryMetadata.Contributors.FirstOrDefault<AtomPersonMetadata>();
					if (atomPersonMetadata != null)
					{
						this.ReadPersonEpm(base.EntryState.Entry.Properties.ToReadOnlyEnumerable("Properties"), base.EntryState.EntityType.ToTypeReference(), targetSegment, atomPersonMetadata);
						return;
					}
				}
				else
				{
					AtomPersonMetadata atomPersonMetadata2 = entryMetadata.Authors.FirstOrDefault<AtomPersonMetadata>();
					if (atomPersonMetadata2 != null)
					{
						this.ReadPersonEpm(base.EntryState.Entry.Properties.ToReadOnlyEnumerable("Properties"), base.EntryState.EntityType.ToTypeReference(), targetSegment, atomPersonMetadata2);
						return;
					}
				}
				return;
			}
			IL_B3:
			throw new ODataException(Strings.General_InternalError(InternalErrorCodes.EpmSyndicationReader_ReadParentSegment_TargetSegmentName));
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x0003C1C0 File Offset: 0x0003A3C0
		private void ReadPersonEpm(ReadOnlyEnumerable<ODataProperty> targetList, IEdmTypeReference targetTypeReference, EpmTargetPathSegment targetSegment, AtomPersonMetadata personMetadata)
		{
			foreach (EpmTargetPathSegment epmTargetPathSegment in targetSegment.SubSegments)
			{
				switch (epmTargetPathSegment.EpmInfo.Attribute.TargetSyndicationItem)
				{
				case SyndicationItemProperty.AuthorEmail:
				case SyndicationItemProperty.ContributorEmail:
				{
					string email = personMetadata.Email;
					if (email != null)
					{
						base.SetEpmValue(targetList, targetTypeReference, epmTargetPathSegment.EpmInfo, email);
					}
					break;
				}
				case SyndicationItemProperty.AuthorName:
				case SyndicationItemProperty.ContributorName:
				{
					string name = personMetadata.Name;
					if (name != null)
					{
						base.SetEpmValue(targetList, targetTypeReference, epmTargetPathSegment.EpmInfo, name);
					}
					break;
				}
				case SyndicationItemProperty.AuthorUri:
				case SyndicationItemProperty.ContributorUri:
				{
					string uriFromEpm = personMetadata.UriFromEpm;
					if (uriFromEpm != null)
					{
						base.SetEpmValue(targetList, targetTypeReference, epmTargetPathSegment.EpmInfo, uriFromEpm);
					}
					break;
				}
				default:
					throw new ODataException(Strings.General_InternalError(InternalErrorCodes.EpmSyndicationReader_ReadPersonEpm));
				}
			}
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x0003C2B0 File Offset: 0x0003A4B0
		private void ReadTextConstructEpm(EpmTargetPathSegment targetSegment, AtomTextConstruct textConstruct)
		{
			if (textConstruct != null && textConstruct.Text != null)
			{
				base.SetEntryEpmValue(targetSegment.EpmInfo, textConstruct.Text);
			}
		}
	}
}
