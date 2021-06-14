using System;
using System.Collections.Generic;
using System.Xml;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x0200021C RID: 540
	internal sealed class ODataAtomServiceDocumentDeserializer : ODataAtomDeserializer
	{
		// Token: 0x060010D3 RID: 4307 RVA: 0x0003EBD4 File Offset: 0x0003CDD4
		internal ODataAtomServiceDocumentDeserializer(ODataAtomInputContext atomInputContext) : base(atomInputContext)
		{
			XmlNameTable nameTable = base.XmlReader.NameTable;
			this.AtomPublishingServiceElementName = nameTable.Add("service");
			this.AtomPublishingWorkspaceElementName = nameTable.Add("workspace");
			this.AtomPublishingCollectionElementName = nameTable.Add("collection");
			this.AtomPublishingAcceptElementName = nameTable.Add("accept");
			this.AtomPublishingCategoriesElementName = nameTable.Add("categories");
			this.AtomHRefAttributeName = nameTable.Add("href");
			this.AtomPublishingNamespace = nameTable.Add("http://www.w3.org/2007/app");
			this.AtomNamespace = nameTable.Add("http://www.w3.org/2005/Atom");
			this.AtomTitleElementName = nameTable.Add("title");
			this.EmptyNamespace = nameTable.Add(string.Empty);
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x060010D4 RID: 4308 RVA: 0x0003EC9E File Offset: 0x0003CE9E
		private ODataAtomServiceDocumentMetadataDeserializer ServiceDocumentMetadataDeserializer
		{
			get
			{
				if (this.serviceDocumentMetadataDeserializer == null)
				{
					this.serviceDocumentMetadataDeserializer = new ODataAtomServiceDocumentMetadataDeserializer(base.AtomInputContext);
				}
				return this.serviceDocumentMetadataDeserializer;
			}
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x0003ECC0 File Offset: 0x0003CEC0
		internal ODataWorkspace ReadServiceDocument()
		{
			base.ReadPayloadStart();
			if (!base.XmlReader.NamespaceEquals(this.AtomPublishingNamespace) || !base.XmlReader.LocalNameEquals(this.AtomPublishingServiceElementName))
			{
				throw new ODataException(Strings.ODataAtomServiceDocumentDeserializer_ServiceDocumentRootElementWrongNameOrNamespace(base.XmlReader.LocalName, base.XmlReader.NamespaceURI));
			}
			ODataWorkspace odataWorkspace = null;
			if (!base.XmlReader.IsEmptyElement)
			{
				base.XmlReader.Read();
				odataWorkspace = this.ReadWorkspace();
			}
			if (odataWorkspace == null)
			{
				throw new ODataException(Strings.ODataAtomServiceDocumentDeserializer_MissingWorkspaceElement);
			}
			this.SkipToElementInAtomPublishingNamespace();
			if (base.XmlReader.NodeType != XmlNodeType.Element)
			{
				base.XmlReader.Read();
				base.ReadPayloadEnd();
				return odataWorkspace;
			}
			if (base.XmlReader.LocalNameEquals(this.AtomPublishingWorkspaceElementName))
			{
				throw new ODataException(Strings.ODataAtomServiceDocumentDeserializer_MultipleWorkspaceElementsFound);
			}
			throw new ODataException(Strings.ODataAtomServiceDocumentDeserializer_UnexpectedElementInServiceDocument(base.XmlReader.LocalName));
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x0003EDA8 File Offset: 0x0003CFA8
		private ODataWorkspace ReadWorkspace()
		{
			bool enableAtomMetadataReading = base.AtomInputContext.MessageReaderSettings.EnableAtomMetadataReading;
			this.SkipToElementInAtomPublishingNamespace();
			if (base.XmlReader.NodeType == XmlNodeType.EndElement)
			{
				return null;
			}
			if (!base.XmlReader.LocalNameEquals(this.AtomPublishingWorkspaceElementName))
			{
				throw new ODataException(Strings.ODataAtomServiceDocumentDeserializer_UnexpectedElementInServiceDocument(base.XmlReader.LocalName));
			}
			List<ODataResourceCollectionInfo> list = new List<ODataResourceCollectionInfo>();
			AtomWorkspaceMetadata atomWorkspaceMetadata = null;
			if (enableAtomMetadataReading)
			{
				atomWorkspaceMetadata = new AtomWorkspaceMetadata();
			}
			if (!base.XmlReader.IsEmptyElement)
			{
				base.XmlReader.ReadStartElement();
				for (;;)
				{
					base.XmlReader.SkipInsignificantNodes();
					XmlNodeType nodeType = base.XmlReader.NodeType;
					if (nodeType != XmlNodeType.Element)
					{
						if (nodeType != XmlNodeType.EndElement)
						{
							base.XmlReader.Skip();
						}
					}
					else if (base.XmlReader.NamespaceEquals(this.AtomPublishingNamespace))
					{
						if (!base.XmlReader.LocalNameEquals(this.AtomPublishingCollectionElementName))
						{
							break;
						}
						ODataResourceCollectionInfo item = this.ReadCollectionElement();
						list.Add(item);
					}
					else if (enableAtomMetadataReading && base.XmlReader.NamespaceEquals(this.AtomNamespace))
					{
						if (base.XmlReader.LocalNameEquals(this.AtomTitleElementName))
						{
							this.ServiceDocumentMetadataDeserializer.ReadTitleElementInWorkspace(atomWorkspaceMetadata);
						}
						else
						{
							base.XmlReader.Skip();
						}
					}
					else
					{
						base.XmlReader.Skip();
					}
					if (base.XmlReader.NodeType == XmlNodeType.EndElement)
					{
						goto IL_162;
					}
				}
				throw new ODataException(Strings.ODataAtomServiceDocumentDeserializer_UnexpectedElementInWorkspace(base.XmlReader.LocalName));
			}
			IL_162:
			base.XmlReader.Read();
			ODataWorkspace odataWorkspace = new ODataWorkspace
			{
				Collections = new ReadOnlyEnumerable<ODataResourceCollectionInfo>(list)
			};
			if (enableAtomMetadataReading)
			{
				odataWorkspace.SetAnnotation<AtomWorkspaceMetadata>(atomWorkspaceMetadata);
			}
			return odataWorkspace;
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x0003EF48 File Offset: 0x0003D148
		private ODataResourceCollectionInfo ReadCollectionElement()
		{
			ODataResourceCollectionInfo odataResourceCollectionInfo = new ODataResourceCollectionInfo();
			string attribute = base.XmlReader.GetAttribute(this.AtomHRefAttributeName, this.EmptyNamespace);
			ValidationUtils.ValidateResourceCollectionInfoUrl(attribute);
			odataResourceCollectionInfo.Url = base.ProcessUriFromPayload(attribute, base.XmlReader.XmlBaseUri);
			bool enableAtomMetadataReading = base.MessageReaderSettings.EnableAtomMetadataReading;
			AtomResourceCollectionMetadata atomResourceCollectionMetadata = null;
			if (enableAtomMetadataReading)
			{
				atomResourceCollectionMetadata = new AtomResourceCollectionMetadata();
			}
			if (!base.XmlReader.IsEmptyElement)
			{
				base.XmlReader.ReadStartElement();
				for (;;)
				{
					XmlNodeType nodeType = base.XmlReader.NodeType;
					if (nodeType != XmlNodeType.Element)
					{
						if (nodeType != XmlNodeType.EndElement)
						{
							base.XmlReader.Skip();
						}
					}
					else if (base.XmlReader.NamespaceEquals(this.AtomPublishingNamespace))
					{
						if (base.XmlReader.LocalNameEquals(this.AtomPublishingCategoriesElementName))
						{
							if (enableAtomMetadataReading)
							{
								this.ServiceDocumentMetadataDeserializer.ReadCategoriesElementInCollection(atomResourceCollectionMetadata);
							}
							else
							{
								base.XmlReader.Skip();
							}
						}
						else
						{
							if (!base.XmlReader.LocalNameEquals(this.AtomPublishingAcceptElementName))
							{
								break;
							}
							if (enableAtomMetadataReading)
							{
								this.ServiceDocumentMetadataDeserializer.ReadAcceptElementInCollection(atomResourceCollectionMetadata);
							}
							else
							{
								base.XmlReader.Skip();
							}
						}
					}
					else if (base.XmlReader.NamespaceEquals(this.AtomNamespace))
					{
						if (base.XmlReader.LocalNameEquals(this.AtomTitleElementName))
						{
							this.ServiceDocumentMetadataDeserializer.ReadTitleElementInCollection(atomResourceCollectionMetadata, odataResourceCollectionInfo);
						}
						else
						{
							base.XmlReader.Skip();
						}
					}
					else
					{
						base.XmlReader.Skip();
					}
					if (base.XmlReader.NodeType == XmlNodeType.EndElement)
					{
						goto IL_18B;
					}
				}
				throw new ODataException(Strings.ODataAtomServiceDocumentDeserializer_UnexpectedElementInResourceCollection(base.XmlReader.LocalName));
			}
			IL_18B:
			base.XmlReader.Read();
			if (enableAtomMetadataReading)
			{
				odataResourceCollectionInfo.SetAnnotation<AtomResourceCollectionMetadata>(atomResourceCollectionMetadata);
			}
			return odataResourceCollectionInfo;
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x0003F0F8 File Offset: 0x0003D2F8
		private void SkipToElementInAtomPublishingNamespace()
		{
			for (;;)
			{
				XmlNodeType nodeType = base.XmlReader.NodeType;
				if (nodeType != XmlNodeType.Element)
				{
					if (nodeType == XmlNodeType.EndElement)
					{
						return;
					}
					base.XmlReader.Skip();
				}
				else
				{
					if (base.XmlReader.NamespaceEquals(this.AtomPublishingNamespace))
					{
						break;
					}
					base.XmlReader.Skip();
				}
			}
		}

		// Token: 0x04000625 RID: 1573
		private readonly string AtomPublishingServiceElementName;

		// Token: 0x04000626 RID: 1574
		private readonly string AtomPublishingWorkspaceElementName;

		// Token: 0x04000627 RID: 1575
		private readonly string AtomHRefAttributeName;

		// Token: 0x04000628 RID: 1576
		private readonly string AtomPublishingCollectionElementName;

		// Token: 0x04000629 RID: 1577
		private readonly string AtomPublishingCategoriesElementName;

		// Token: 0x0400062A RID: 1578
		private readonly string AtomPublishingAcceptElementName;

		// Token: 0x0400062B RID: 1579
		private readonly string AtomPublishingNamespace;

		// Token: 0x0400062C RID: 1580
		private readonly string AtomNamespace;

		// Token: 0x0400062D RID: 1581
		private readonly string AtomTitleElementName;

		// Token: 0x0400062E RID: 1582
		private readonly string EmptyNamespace;

		// Token: 0x0400062F RID: 1583
		private ODataAtomServiceDocumentMetadataDeserializer serviceDocumentMetadataDeserializer;
	}
}
