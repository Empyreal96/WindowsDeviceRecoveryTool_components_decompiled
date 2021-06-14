using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Common
{
	// Token: 0x02000154 RID: 340
	internal class XmlElementInfo : IXmlElementAttributes
	{
		// Token: 0x06000699 RID: 1689 RVA: 0x00010DEC File Offset: 0x0000EFEC
		internal XmlElementInfo(string elementName, CsdlLocation elementLocation, IList<XmlAttributeInfo> attributes, List<XmlAnnotationInfo> annotations)
		{
			this.Name = elementName;
			this.Location = elementLocation;
			if (attributes != null && attributes.Count > 0)
			{
				this.attributes = new Dictionary<string, XmlAttributeInfo>();
				foreach (XmlAttributeInfo xmlAttributeInfo in attributes)
				{
					this.attributes.Add(xmlAttributeInfo.Name, xmlAttributeInfo);
				}
			}
			this.annotations = annotations;
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x0600069A RID: 1690 RVA: 0x0001104C File Offset: 0x0000F24C
		IEnumerable<XmlAttributeInfo> IXmlElementAttributes.Unused
		{
			get
			{
				if (this.attributes != null)
				{
					foreach (XmlAttributeInfo attr2 in from attr in this.attributes.Values
					where !attr.IsUsed
					select attr)
					{
						yield return attr2;
					}
				}
				yield break;
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x0600069B RID: 1691 RVA: 0x00011069 File Offset: 0x0000F269
		// (set) Token: 0x0600069C RID: 1692 RVA: 0x00011071 File Offset: 0x0000F271
		internal string Name { get; private set; }

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x0001107A File Offset: 0x0000F27A
		// (set) Token: 0x0600069E RID: 1694 RVA: 0x00011082 File Offset: 0x0000F282
		internal CsdlLocation Location { get; private set; }

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x0001108B File Offset: 0x0000F28B
		internal IXmlElementAttributes Attributes
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x060006A0 RID: 1696 RVA: 0x0001108E File Offset: 0x0000F28E
		internal IList<XmlAnnotationInfo> Annotations
		{
			get
			{
				return this.annotations ?? ((IList<XmlAnnotationInfo>)new XmlAnnotationInfo[0]);
			}
		}

		// Token: 0x170002C1 RID: 705
		XmlAttributeInfo IXmlElementAttributes.this[string attributeName]
		{
			get
			{
				XmlAttributeInfo xmlAttributeInfo;
				if (this.attributes != null && this.attributes.TryGetValue(attributeName, out xmlAttributeInfo))
				{
					xmlAttributeInfo.IsUsed = true;
					return xmlAttributeInfo;
				}
				return XmlAttributeInfo.Missing;
			}
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x000110DB File Offset: 0x0000F2DB
		internal void AddAnnotation(XmlAnnotationInfo annotation)
		{
			if (this.annotations == null)
			{
				this.annotations = new List<XmlAnnotationInfo>();
			}
			this.annotations.Add(annotation);
		}

		// Token: 0x04000374 RID: 884
		private readonly Dictionary<string, XmlAttributeInfo> attributes;

		// Token: 0x04000375 RID: 885
		private List<XmlAnnotationInfo> annotations;
	}
}
