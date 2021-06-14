using System;
using System.Xml;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x02000218 RID: 536
	internal sealed class ODataAtomCollectionDeserializer : ODataAtomPropertyAndValueDeserializer
	{
		// Token: 0x0600109C RID: 4252 RVA: 0x0003C53B File Offset: 0x0003A73B
		internal ODataAtomCollectionDeserializer(ODataAtomInputContext atomInputContext) : base(atomInputContext)
		{
			this.duplicatePropertyNamesChecker = base.CreateDuplicatePropertyNamesChecker();
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x0003C550 File Offset: 0x0003A750
		internal ODataCollectionStart ReadCollectionStart(out bool isCollectionElementEmpty)
		{
			if (!base.XmlReader.NamespaceEquals(base.XmlReader.ODataNamespace))
			{
				throw new ODataException(Strings.ODataAtomCollectionDeserializer_TopLevelCollectionElementWrongNamespace(base.XmlReader.NamespaceURI, base.XmlReader.ODataNamespace));
			}
			while (base.XmlReader.MoveToNextAttribute())
			{
				if (base.XmlReader.NamespaceEquals(base.XmlReader.ODataMetadataNamespace) && (base.XmlReader.LocalNameEquals(this.AtomTypeAttributeName) || base.XmlReader.LocalNameEquals(this.ODataNullAttributeName)))
				{
					throw new ODataException(Strings.ODataAtomCollectionDeserializer_TypeOrNullAttributeNotAllowed);
				}
			}
			base.XmlReader.MoveToElement();
			ODataCollectionStart odataCollectionStart = new ODataCollectionStart();
			odataCollectionStart.Name = base.XmlReader.LocalName;
			isCollectionElementEmpty = base.XmlReader.IsEmptyElement;
			if (!isCollectionElementEmpty)
			{
				base.XmlReader.Read();
			}
			return odataCollectionStart;
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x0003C62D File Offset: 0x0003A82D
		internal void ReadCollectionEnd()
		{
			base.XmlReader.Read();
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x0003C63C File Offset: 0x0003A83C
		internal object ReadCollectionItem(IEdmTypeReference expectedItemType, CollectionWithoutExpectedTypeValidator collectionValidator)
		{
			if (!base.XmlReader.LocalNameEquals(this.ODataCollectionItemElementName))
			{
				throw new ODataException(Strings.ODataAtomCollectionDeserializer_WrongCollectionItemElementName(base.XmlReader.LocalName, base.XmlReader.ODataNamespace));
			}
			object result = base.ReadNonEntityValue(expectedItemType, this.duplicatePropertyNamesChecker, collectionValidator, true, false);
			base.XmlReader.Read();
			return result;
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x0003C69C File Offset: 0x0003A89C
		internal void SkipToElementInODataNamespace()
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
					if (base.XmlReader.NamespaceEquals(base.XmlReader.ODataNamespace))
					{
						break;
					}
					base.XmlReader.Skip();
				}
				if (base.XmlReader.EOF)
				{
					return;
				}
			}
		}

		// Token: 0x04000603 RID: 1539
		private readonly DuplicatePropertyNamesChecker duplicatePropertyNamesChecker;
	}
}
