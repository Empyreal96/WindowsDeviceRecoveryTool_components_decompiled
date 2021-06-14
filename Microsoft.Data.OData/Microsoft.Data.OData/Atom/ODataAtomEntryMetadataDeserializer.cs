using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x0200021B RID: 539
	internal sealed class ODataAtomEntryMetadataDeserializer : ODataAtomEpmDeserializer
	{
		// Token: 0x060010C7 RID: 4295 RVA: 0x0003E458 File Offset: 0x0003C658
		internal ODataAtomEntryMetadataDeserializer(ODataAtomInputContext atomInputContext) : base(atomInputContext)
		{
			XmlNameTable nameTable = base.XmlReader.NameTable;
			this.EmptyNamespace = nameTable.Add(string.Empty);
			this.AtomNamespace = nameTable.Add("http://www.w3.org/2005/Atom");
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x060010C8 RID: 4296 RVA: 0x0003E49C File Offset: 0x0003C69C
		private ODataAtomFeedMetadataDeserializer SourceMetadataDeserializer
		{
			get
			{
				ODataAtomFeedMetadataDeserializer result;
				if ((result = this.sourceMetadataDeserializer) == null)
				{
					result = (this.sourceMetadataDeserializer = new ODataAtomFeedMetadataDeserializer(base.AtomInputContext, true));
				}
				return result;
			}
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x0003E4C8 File Offset: 0x0003C6C8
		internal void ReadAtomElementInEntryContent(IODataAtomReaderEntryState entryState)
		{
			ODataEntityPropertyMappingCache cachedEpm = entryState.CachedEpm;
			EpmTargetPathSegment parentSegment = null;
			if (cachedEpm != null)
			{
				parentSegment = cachedEpm.EpmTargetTree.SyndicationRoot;
			}
			EpmTargetPathSegment epmTargetPathSegment;
			string localName;
			if (base.ShouldReadElement(parentSegment, base.XmlReader.LocalName, out epmTargetPathSegment) && (localName = base.XmlReader.LocalName) != null)
			{
				if (<PrivateImplementationDetails>{D7F3BFF1-6690-4BAC-BAB5-77DD89A1B7E1}.$$method0x6001036-1 == null)
				{
					<PrivateImplementationDetails>{D7F3BFF1-6690-4BAC-BAB5-77DD89A1B7E1}.$$method0x6001036-1 = new Dictionary<string, int>(8)
					{
						{
							"author",
							0
						},
						{
							"contributor",
							1
						},
						{
							"updated",
							2
						},
						{
							"published",
							3
						},
						{
							"rights",
							4
						},
						{
							"source",
							5
						},
						{
							"summary",
							6
						},
						{
							"title",
							7
						}
					};
				}
				int num;
				if (<PrivateImplementationDetails>{D7F3BFF1-6690-4BAC-BAB5-77DD89A1B7E1}.$$method0x6001036-1.TryGetValue(localName, out num))
				{
					switch (num)
					{
					case 0:
						this.ReadAuthorElement(entryState, epmTargetPathSegment);
						return;
					case 1:
						this.ReadContributorElement(entryState, epmTargetPathSegment);
						return;
					case 2:
					{
						AtomEntryMetadata atomEntryMetadata = entryState.AtomEntryMetadata;
						if (base.UseClientFormatBehavior)
						{
							if (this.ShouldReadSingletonElement(atomEntryMetadata.UpdatedString != null))
							{
								atomEntryMetadata.UpdatedString = base.ReadAtomDateConstructAsString();
								return;
							}
						}
						else if (this.ShouldReadSingletonElement(atomEntryMetadata.Updated != null))
						{
							atomEntryMetadata.Updated = base.ReadAtomDateConstruct();
							return;
						}
						break;
					}
					case 3:
					{
						AtomEntryMetadata atomEntryMetadata2 = entryState.AtomEntryMetadata;
						if (base.UseClientFormatBehavior)
						{
							if (this.ShouldReadSingletonElement(atomEntryMetadata2.PublishedString != null))
							{
								atomEntryMetadata2.PublishedString = base.ReadAtomDateConstructAsString();
								return;
							}
						}
						else if (this.ShouldReadSingletonElement(atomEntryMetadata2.Published != null))
						{
							atomEntryMetadata2.Published = base.ReadAtomDateConstruct();
							return;
						}
						break;
					}
					case 4:
						if (this.ShouldReadSingletonElement(entryState.AtomEntryMetadata.Rights != null))
						{
							entryState.AtomEntryMetadata.Rights = base.ReadAtomTextConstruct();
							return;
						}
						break;
					case 5:
						if (this.ShouldReadSingletonElement(entryState.AtomEntryMetadata.Source != null))
						{
							entryState.AtomEntryMetadata.Source = this.ReadAtomSourceInEntryContent();
							return;
						}
						break;
					case 6:
						if (this.ShouldReadSingletonElement(entryState.AtomEntryMetadata.Summary != null))
						{
							entryState.AtomEntryMetadata.Summary = base.ReadAtomTextConstruct();
							return;
						}
						break;
					case 7:
						if (this.ShouldReadSingletonElement(entryState.AtomEntryMetadata.Title != null))
						{
							entryState.AtomEntryMetadata.Title = base.ReadAtomTextConstruct();
							return;
						}
						break;
					}
				}
			}
			base.XmlReader.Skip();
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x0003E758 File Offset: 0x0003C958
		internal AtomLinkMetadata ReadAtomLinkElementInEntryContent(string relation, string hrefStringValue)
		{
			AtomLinkMetadata atomLinkMetadata = null;
			if (base.ReadAtomMetadata)
			{
				atomLinkMetadata = new AtomLinkMetadata();
				atomLinkMetadata.Relation = relation;
				if (base.ReadAtomMetadata)
				{
					atomLinkMetadata.Href = ((hrefStringValue == null) ? null : base.ProcessUriFromPayload(hrefStringValue, base.XmlReader.XmlBaseUri));
				}
				while (base.XmlReader.MoveToNextAttribute())
				{
					string localName;
					if (base.XmlReader.NamespaceEquals(this.EmptyNamespace) && (localName = base.XmlReader.LocalName) != null)
					{
						if (!(localName == "type"))
						{
							if (!(localName == "hreflang"))
							{
								if (!(localName == "title"))
								{
									if (localName == "length")
									{
										if (base.ReadAtomMetadata)
										{
											string value = base.XmlReader.Value;
											int value2;
											if (!int.TryParse(value, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out value2))
											{
												throw new ODataException(Strings.EpmSyndicationWriter_InvalidLinkLengthValue(value));
											}
											atomLinkMetadata.Length = new int?(value2);
										}
									}
								}
								else
								{
									atomLinkMetadata.Title = base.XmlReader.Value;
								}
							}
							else
							{
								atomLinkMetadata.HrefLang = base.XmlReader.Value;
							}
						}
						else
						{
							atomLinkMetadata.MediaType = base.XmlReader.Value;
						}
					}
				}
			}
			base.XmlReader.MoveToElement();
			return atomLinkMetadata;
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x0003E8B8 File Offset: 0x0003CAB8
		internal void ReadAtomCategoryElementInEntryContent(IODataAtomReaderEntryState entryState)
		{
			ODataEntityPropertyMappingCache cachedEpm = entryState.CachedEpm;
			EpmTargetPathSegment epmTargetPathSegment = null;
			if (cachedEpm != null)
			{
				epmTargetPathSegment = cachedEpm.EpmTargetTree.SyndicationRoot;
			}
			bool flag;
			if (epmTargetPathSegment != null)
			{
				flag = epmTargetPathSegment.SubSegments.Any((EpmTargetPathSegment segment) => string.CompareOrdinal(segment.SegmentName, "category") == 0);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (base.ReadAtomMetadata || flag2)
			{
				AtomCategoryMetadata categoryMetadata = this.ReadAtomCategoryElement();
				entryState.AtomEntryMetadata.AddCategory(categoryMetadata);
				return;
			}
			base.XmlReader.Skip();
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x0003E938 File Offset: 0x0003CB38
		internal AtomCategoryMetadata ReadAtomCategoryElement()
		{
			AtomCategoryMetadata atomCategoryMetadata = new AtomCategoryMetadata();
			while (base.XmlReader.MoveToNextAttribute())
			{
				string localName2;
				if (base.XmlReader.NamespaceEquals(this.EmptyNamespace))
				{
					string localName;
					if ((localName = base.XmlReader.LocalName) != null)
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
								atomCategoryMetadata.Term = (atomCategoryMetadata.Term ?? base.XmlReader.Value);
							}
						}
						else
						{
							atomCategoryMetadata.Scheme = (atomCategoryMetadata.Scheme ?? base.XmlReader.Value);
						}
					}
				}
				else if (base.UseClientFormatBehavior && base.XmlReader.NamespaceEquals(this.AtomNamespace) && (localName2 = base.XmlReader.LocalName) != null)
				{
					if (!(localName2 == "scheme"))
					{
						if (localName2 == "term")
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
			base.XmlReader.Skip();
			return atomCategoryMetadata;
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x0003EA7C File Offset: 0x0003CC7C
		internal AtomFeedMetadata ReadAtomSourceInEntryContent()
		{
			AtomFeedMetadata atomFeedMetadata = AtomMetadataReaderUtils.CreateNewAtomFeedMetadata();
			if (base.XmlReader.IsEmptyElement)
			{
				base.XmlReader.Read();
				return atomFeedMetadata;
			}
			base.XmlReader.Read();
			while (base.XmlReader.NodeType != XmlNodeType.EndElement)
			{
				if (base.XmlReader.NodeType != XmlNodeType.Element)
				{
					base.XmlReader.Skip();
				}
				else if (base.XmlReader.NamespaceEquals(this.AtomNamespace))
				{
					this.SourceMetadataDeserializer.ReadAtomElementAsFeedMetadata(atomFeedMetadata);
				}
				else
				{
					base.XmlReader.Skip();
				}
			}
			base.XmlReader.Read();
			return atomFeedMetadata;
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x0003EB1B File Offset: 0x0003CD1B
		private void ReadAuthorElement(IODataAtomReaderEntryState entryState, EpmTargetPathSegment epmTargetPathSegment)
		{
			if (this.ShouldReadCollectionElement(entryState.AtomEntryMetadata.Authors.Any<AtomPersonMetadata>()))
			{
				entryState.AtomEntryMetadata.AddAuthor(base.ReadAtomPersonConstruct(epmTargetPathSegment));
				return;
			}
			base.XmlReader.Skip();
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x0003EB53 File Offset: 0x0003CD53
		private void ReadContributorElement(IODataAtomReaderEntryState entryState, EpmTargetPathSegment epmTargetPathSegment)
		{
			if (this.ShouldReadCollectionElement(entryState.AtomEntryMetadata.Contributors.Any<AtomPersonMetadata>()))
			{
				entryState.AtomEntryMetadata.AddContributor(base.ReadAtomPersonConstruct(epmTargetPathSegment));
				return;
			}
			base.XmlReader.Skip();
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x0003EB8B File Offset: 0x0003CD8B
		private bool ShouldReadCollectionElement(bool someAlreadyExist)
		{
			return base.ReadAtomMetadata || !someAlreadyExist;
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x0003EB9B File Offset: 0x0003CD9B
		private bool ShouldReadSingletonElement(bool alreadyExists)
		{
			if (!alreadyExists)
			{
				return true;
			}
			if (base.ReadAtomMetadata || base.AtomInputContext.UseDefaultFormatBehavior)
			{
				throw new ODataException(Strings.ODataAtomMetadataDeserializer_MultipleSingletonMetadataElements(base.XmlReader.LocalName, "entry"));
			}
			return false;
		}

		// Token: 0x04000621 RID: 1569
		private readonly string EmptyNamespace;

		// Token: 0x04000622 RID: 1570
		private readonly string AtomNamespace;

		// Token: 0x04000623 RID: 1571
		private ODataAtomFeedMetadataDeserializer sourceMetadataDeserializer;
	}
}
