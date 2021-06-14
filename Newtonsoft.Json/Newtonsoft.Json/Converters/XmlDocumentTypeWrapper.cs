using System;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000030 RID: 48
	internal class XmlDocumentTypeWrapper : XmlNodeWrapper, IXmlDocumentType, IXmlNode
	{
		// Token: 0x060001DE RID: 478 RVA: 0x00008289 File Offset: 0x00006489
		public XmlDocumentTypeWrapper(XmlDocumentType documentType) : base(documentType)
		{
			this._documentType = documentType;
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001DF RID: 479 RVA: 0x00008299 File Offset: 0x00006499
		public string Name
		{
			get
			{
				return this._documentType.Name;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x000082A6 File Offset: 0x000064A6
		public string System
		{
			get
			{
				return this._documentType.SystemId;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x000082B3 File Offset: 0x000064B3
		public string Public
		{
			get
			{
				return this._documentType.PublicId;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x000082C0 File Offset: 0x000064C0
		public string InternalSubset
		{
			get
			{
				return this._documentType.InternalSubset;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x000082CD File Offset: 0x000064CD
		public override string LocalName
		{
			get
			{
				return "DOCTYPE";
			}
		}

		// Token: 0x040000A3 RID: 163
		private readonly XmlDocumentType _documentType;
	}
}
