using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x020001E0 RID: 480
	internal sealed class ODataAtomFeedMetadataDeserializer : ODataAtomMetadataDeserializer
	{
		// Token: 0x06000ED4 RID: 3796 RVA: 0x00034584 File Offset: 0x00032784
		internal ODataAtomFeedMetadataDeserializer(ODataAtomInputContext atomInputContext, bool inSourceElement) : base(atomInputContext)
		{
			XmlNameTable nameTable = base.XmlReader.NameTable;
			this.EmptyNamespace = nameTable.Add(string.Empty);
			this.InSourceElement = inSourceElement;
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000ED5 RID: 3797 RVA: 0x000345BC File Offset: 0x000327BC
		// (set) Token: 0x06000ED6 RID: 3798 RVA: 0x000345C4 File Offset: 0x000327C4
		private bool InSourceElement { get; set; }

		// Token: 0x06000ED7 RID: 3799 RVA: 0x000345D0 File Offset: 0x000327D0
		internal void ReadAtomElementAsFeedMetadata(AtomFeedMetadata atomFeedMetadata)
		{
			string localName;
			switch (localName = base.XmlReader.LocalName)
			{
			case "author":
				this.ReadAuthorElement(atomFeedMetadata);
				return;
			case "category":
				this.ReadCategoryElement(atomFeedMetadata);
				return;
			case "contributor":
				this.ReadContributorElement(atomFeedMetadata);
				return;
			case "generator":
				this.ReadGeneratorElement(atomFeedMetadata);
				return;
			case "icon":
				this.ReadIconElement(atomFeedMetadata);
				return;
			case "id":
				if (this.InSourceElement)
				{
					this.ReadIdElementAsSourceId(atomFeedMetadata);
					return;
				}
				base.XmlReader.Skip();
				return;
			case "link":
				this.ReadLinkElementIntoLinksCollection(atomFeedMetadata);
				return;
			case "logo":
				this.ReadLogoElement(atomFeedMetadata);
				return;
			case "rights":
				this.ReadRightsElement(atomFeedMetadata);
				return;
			case "subtitle":
				this.ReadSubtitleElement(atomFeedMetadata);
				return;
			case "title":
				this.ReadTitleElement(atomFeedMetadata);
				return;
			case "updated":
				this.ReadUpdatedElement(atomFeedMetadata);
				return;
			}
			base.XmlReader.Skip();
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x00034768 File Offset: 0x00032968
		internal AtomLinkMetadata ReadAtomLinkElementInFeed(string relation, string hrefStringValue)
		{
			AtomLinkMetadata atomLinkMetadata = new AtomLinkMetadata
			{
				Relation = relation,
				Href = ((hrefStringValue == null) ? null : base.ProcessUriFromPayload(hrefStringValue, base.XmlReader.XmlBaseUri))
			};
			while (base.XmlReader.MoveToNextAttribute())
			{
				string localName;
				if (base.XmlReader.NamespaceEquals(this.EmptyNamespace) && (localName = base.XmlReader.LocalName) != null)
				{
					if (<PrivateImplementationDetails>{D7F3BFF1-6690-4BAC-BAB5-77DD89A1B7E1}.$$method0x6000e4c-1 == null)
					{
						<PrivateImplementationDetails>{D7F3BFF1-6690-4BAC-BAB5-77DD89A1B7E1}.$$method0x6000e4c-1 = new Dictionary<string, int>(6)
						{
							{
								"type",
								0
							},
							{
								"hreflang",
								1
							},
							{
								"title",
								2
							},
							{
								"length",
								3
							},
							{
								"rel",
								4
							},
							{
								"href",
								5
							}
						};
					}
					int num;
					if (<PrivateImplementationDetails>{D7F3BFF1-6690-4BAC-BAB5-77DD89A1B7E1}.$$method0x6000e4c-1.TryGetValue(localName, out num))
					{
						switch (num)
						{
						case 0:
							atomLinkMetadata.MediaType = base.XmlReader.Value;
							break;
						case 1:
							atomLinkMetadata.HrefLang = base.XmlReader.Value;
							break;
						case 2:
							atomLinkMetadata.Title = base.XmlReader.Value;
							break;
						case 3:
						{
							string value = base.XmlReader.Value;
							int value2;
							if (!int.TryParse(value, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out value2))
							{
								throw new ODataException(Strings.EpmSyndicationWriter_InvalidLinkLengthValue(value));
							}
							atomLinkMetadata.Length = new int?(value2);
							break;
						}
						case 4:
							if (atomLinkMetadata.Relation == null)
							{
								atomLinkMetadata.Relation = base.XmlReader.Value;
							}
							break;
						case 5:
							if (atomLinkMetadata.Href == null)
							{
								atomLinkMetadata.Href = base.ProcessUriFromPayload(base.XmlReader.Value, base.XmlReader.XmlBaseUri);
							}
							break;
						}
					}
				}
			}
			base.XmlReader.Skip();
			return atomLinkMetadata;
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x0003494A File Offset: 0x00032B4A
		private void ReadAuthorElement(AtomFeedMetadata atomFeedMetadata)
		{
			atomFeedMetadata.AddAuthor(base.ReadAtomPersonConstruct(null));
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x0003495C File Offset: 0x00032B5C
		private void ReadCategoryElement(AtomFeedMetadata atomFeedMetadata)
		{
			AtomCategoryMetadata atomCategoryMetadata = new AtomCategoryMetadata();
			while (base.XmlReader.MoveToNextAttribute())
			{
				string localName;
				if (base.XmlReader.NamespaceEquals(this.EmptyNamespace) && (localName = base.XmlReader.LocalName) != null)
				{
					if (!(localName == "scheme"))
					{
						if (!(localName == "term"))
						{
							if (localName == "label")
							{
								atomCategoryMetadata.Label = base.XmlReader.Value;
							}
						}
						else
						{
							atomCategoryMetadata.Term = base.XmlReader.Value;
						}
					}
					else
					{
						atomCategoryMetadata.Scheme = base.XmlReader.Value;
					}
				}
			}
			atomFeedMetadata.AddCategory(atomCategoryMetadata);
			base.XmlReader.Skip();
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x00034A18 File Offset: 0x00032C18
		private void ReadContributorElement(AtomFeedMetadata atomFeedMetadata)
		{
			atomFeedMetadata.AddContributor(base.ReadAtomPersonConstruct(null));
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x00034A28 File Offset: 0x00032C28
		private void ReadGeneratorElement(AtomFeedMetadata atomFeedMetadata)
		{
			this.VerifyNotPreviouslyDefined(atomFeedMetadata.Generator);
			AtomGeneratorMetadata atomGeneratorMetadata = new AtomGeneratorMetadata();
			while (base.XmlReader.MoveToNextAttribute())
			{
				string localName;
				if (base.XmlReader.NamespaceEquals(this.EmptyNamespace) && (localName = base.XmlReader.LocalName) != null)
				{
					if (!(localName == "uri"))
					{
						if (localName == "version")
						{
							atomGeneratorMetadata.Version = base.XmlReader.Value;
						}
					}
					else
					{
						atomGeneratorMetadata.Uri = base.ProcessUriFromPayload(base.XmlReader.Value, base.XmlReader.XmlBaseUri);
					}
				}
			}
			base.XmlReader.MoveToElement();
			if (base.XmlReader.IsEmptyElement)
			{
				base.XmlReader.Skip();
			}
			else
			{
				atomGeneratorMetadata.Name = base.XmlReader.ReadElementValue();
			}
			atomFeedMetadata.Generator = atomGeneratorMetadata;
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x00034B07 File Offset: 0x00032D07
		private void ReadIconElement(AtomFeedMetadata atomFeedMetadata)
		{
			this.VerifyNotPreviouslyDefined(atomFeedMetadata.Icon);
			atomFeedMetadata.Icon = this.ReadUriValuedElement();
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x00034B21 File Offset: 0x00032D21
		private void ReadIdElementAsSourceId(AtomFeedMetadata atomFeedMetadata)
		{
			this.VerifyNotPreviouslyDefined(atomFeedMetadata.SourceId);
			atomFeedMetadata.SourceId = base.XmlReader.ReadElementValue();
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x00034B40 File Offset: 0x00032D40
		private void ReadLinkElementIntoLinksCollection(AtomFeedMetadata atomFeedMetadata)
		{
			AtomLinkMetadata linkMetadata = this.ReadAtomLinkElementInFeed(null, null);
			atomFeedMetadata.AddLink(linkMetadata);
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x00034B5D File Offset: 0x00032D5D
		private void ReadLogoElement(AtomFeedMetadata atomFeedMetadata)
		{
			this.VerifyNotPreviouslyDefined(atomFeedMetadata.Logo);
			atomFeedMetadata.Logo = this.ReadUriValuedElement();
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x00034B77 File Offset: 0x00032D77
		private void ReadRightsElement(AtomFeedMetadata atomFeedMetadata)
		{
			this.VerifyNotPreviouslyDefined(atomFeedMetadata.Rights);
			atomFeedMetadata.Rights = base.ReadAtomTextConstruct();
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x00034B91 File Offset: 0x00032D91
		private void ReadSubtitleElement(AtomFeedMetadata atomFeedMetadata)
		{
			this.VerifyNotPreviouslyDefined(atomFeedMetadata.Subtitle);
			atomFeedMetadata.Subtitle = base.ReadAtomTextConstruct();
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x00034BAB File Offset: 0x00032DAB
		private void ReadTitleElement(AtomFeedMetadata atomFeedMetadata)
		{
			this.VerifyNotPreviouslyDefined(atomFeedMetadata.Title);
			atomFeedMetadata.Title = base.ReadAtomTextConstruct();
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x00034BC5 File Offset: 0x00032DC5
		private void ReadUpdatedElement(AtomFeedMetadata atomFeedMetadata)
		{
			this.VerifyNotPreviouslyDefined(atomFeedMetadata.Updated);
			atomFeedMetadata.Updated = base.ReadAtomDateConstruct();
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x00034BE4 File Offset: 0x00032DE4
		private Uri ReadUriValuedElement()
		{
			string uriFromPayload = base.XmlReader.ReadElementValue();
			return base.ProcessUriFromPayload(uriFromPayload, base.XmlReader.XmlBaseUri);
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x00034C10 File Offset: 0x00032E10
		private void VerifyNotPreviouslyDefined(object metadataValue)
		{
			if (metadataValue != null)
			{
				string p = this.InSourceElement ? "source" : "feed";
				throw new ODataException(Strings.ODataAtomMetadataDeserializer_MultipleSingletonMetadataElements(base.XmlReader.LocalName, p));
			}
		}

		// Token: 0x0400051E RID: 1310
		private readonly string EmptyNamespace;
	}
}
