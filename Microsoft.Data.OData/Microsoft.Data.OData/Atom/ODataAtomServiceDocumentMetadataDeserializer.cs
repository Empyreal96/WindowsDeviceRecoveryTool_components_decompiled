using System;
using System.Collections.Generic;
using System.Xml;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x020001E2 RID: 482
	internal sealed class ODataAtomServiceDocumentMetadataDeserializer : ODataAtomMetadataDeserializer
	{
		// Token: 0x06000EEE RID: 3822 RVA: 0x00034E80 File Offset: 0x00033080
		internal ODataAtomServiceDocumentMetadataDeserializer(ODataAtomInputContext atomInputContext) : base(atomInputContext)
		{
			XmlNameTable nameTable = base.XmlReader.NameTable;
			this.AtomNamespace = nameTable.Add("http://www.w3.org/2005/Atom");
			this.AtomCategoryElementName = nameTable.Add("category");
			this.AtomHRefAttributeName = nameTable.Add("href");
			this.AtomPublishingFixedAttributeName = nameTable.Add("fixed");
			this.AtomCategorySchemeAttributeName = nameTable.Add("scheme");
			this.AtomCategoryTermAttributeName = nameTable.Add("term");
			this.AtomCategoryLabelAttributeName = nameTable.Add("label");
			this.EmptyNamespace = nameTable.Add(string.Empty);
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x00034F28 File Offset: 0x00033128
		internal void ReadTitleElementInWorkspace(AtomWorkspaceMetadata workspaceMetadata)
		{
			if (workspaceMetadata.Title != null)
			{
				throw new ODataException(Strings.ODataAtomServiceDocumentMetadataDeserializer_MultipleTitleElementsFound("workspace"));
			}
			workspaceMetadata.Title = base.ReadTitleElement();
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x00034F50 File Offset: 0x00033150
		internal void ReadTitleElementInCollection(AtomResourceCollectionMetadata collectionMetadata, ODataResourceCollectionInfo collectionInfo)
		{
			if (collectionInfo.Name != null)
			{
				throw new ODataException(Strings.ODataAtomServiceDocumentMetadataDeserializer_MultipleTitleElementsFound("collection"));
			}
			AtomTextConstruct atomTextConstruct = base.ReadTitleElement();
			collectionInfo.Name = atomTextConstruct.Text;
			if (base.ReadAtomMetadata)
			{
				collectionMetadata.Title = atomTextConstruct;
			}
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x00034F98 File Offset: 0x00033198
		internal void ReadCategoriesElementInCollection(AtomResourceCollectionMetadata collectionMetadata)
		{
			AtomCategoriesMetadata atomCategoriesMetadata = new AtomCategoriesMetadata();
			List<AtomCategoryMetadata> list = new List<AtomCategoryMetadata>();
			while (base.XmlReader.MoveToNextAttribute())
			{
				string value = base.XmlReader.Value;
				if (base.XmlReader.NamespaceEquals(this.EmptyNamespace))
				{
					if (base.XmlReader.LocalNameEquals(this.AtomHRefAttributeName))
					{
						atomCategoriesMetadata.Href = base.ProcessUriFromPayload(value, base.XmlReader.XmlBaseUri);
					}
					else if (base.XmlReader.LocalNameEquals(this.AtomPublishingFixedAttributeName))
					{
						if (string.CompareOrdinal(value, "yes") == 0)
						{
							atomCategoriesMetadata.Fixed = new bool?(true);
						}
						else
						{
							if (string.CompareOrdinal(value, "no") != 0)
							{
								throw new ODataException(Strings.ODataAtomServiceDocumentMetadataDeserializer_InvalidFixedAttributeValue(value));
							}
							atomCategoriesMetadata.Fixed = new bool?(false);
						}
					}
					else if (base.XmlReader.LocalNameEquals(this.AtomCategorySchemeAttributeName))
					{
						atomCategoriesMetadata.Scheme = value;
					}
				}
			}
			base.XmlReader.MoveToElement();
			if (!base.XmlReader.IsEmptyElement)
			{
				base.XmlReader.ReadStartElement();
				do
				{
					XmlNodeType nodeType = base.XmlReader.NodeType;
					if (nodeType != XmlNodeType.Element)
					{
						if (nodeType != XmlNodeType.EndElement)
						{
							base.XmlReader.Skip();
						}
					}
					else if (base.XmlReader.NamespaceEquals(this.AtomNamespace) && base.XmlReader.LocalNameEquals(this.AtomCategoryElementName))
					{
						list.Add(this.ReadCategoryElementInCollection());
					}
				}
				while (base.XmlReader.NodeType != XmlNodeType.EndElement);
			}
			base.XmlReader.Read();
			atomCategoriesMetadata.Categories = new ReadOnlyEnumerable<AtomCategoryMetadata>(list);
			collectionMetadata.Categories = atomCategoriesMetadata;
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x0003512C File Offset: 0x0003332C
		internal void ReadAcceptElementInCollection(AtomResourceCollectionMetadata collectionMetadata)
		{
			if (collectionMetadata.Accept != null)
			{
				throw new ODataException(Strings.ODataAtomServiceDocumentMetadataDeserializer_MultipleAcceptElementsFoundInCollection);
			}
			collectionMetadata.Accept = base.XmlReader.ReadElementValue();
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x00035154 File Offset: 0x00033354
		private AtomCategoryMetadata ReadCategoryElementInCollection()
		{
			AtomCategoryMetadata atomCategoryMetadata = new AtomCategoryMetadata();
			while (base.XmlReader.MoveToNextAttribute())
			{
				string value = base.XmlReader.Value;
				if (base.XmlReader.NamespaceEquals(this.EmptyNamespace))
				{
					if (base.XmlReader.LocalNameEquals(this.AtomCategoryTermAttributeName))
					{
						atomCategoryMetadata.Term = value;
					}
					else if (base.XmlReader.LocalNameEquals(this.AtomCategorySchemeAttributeName))
					{
						atomCategoryMetadata.Scheme = value;
					}
					else if (base.XmlReader.LocalNameEquals(this.AtomCategoryLabelAttributeName))
					{
						atomCategoryMetadata.Label = value;
					}
				}
			}
			return atomCategoryMetadata;
		}

		// Token: 0x04000522 RID: 1314
		private readonly string AtomNamespace;

		// Token: 0x04000523 RID: 1315
		private readonly string AtomCategoryElementName;

		// Token: 0x04000524 RID: 1316
		private readonly string AtomHRefAttributeName;

		// Token: 0x04000525 RID: 1317
		private readonly string AtomPublishingFixedAttributeName;

		// Token: 0x04000526 RID: 1318
		private readonly string AtomCategorySchemeAttributeName;

		// Token: 0x04000527 RID: 1319
		private readonly string AtomCategoryTermAttributeName;

		// Token: 0x04000528 RID: 1320
		private readonly string AtomCategoryLabelAttributeName;

		// Token: 0x04000529 RID: 1321
		private readonly string EmptyNamespace;
	}
}
