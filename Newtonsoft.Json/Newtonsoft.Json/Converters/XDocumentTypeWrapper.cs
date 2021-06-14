using System;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000033 RID: 51
	internal class XDocumentTypeWrapper : XObjectWrapper, IXmlDocumentType, IXmlNode
	{
		// Token: 0x060001F8 RID: 504 RVA: 0x00008384 File Offset: 0x00006584
		public XDocumentTypeWrapper(XDocumentType documentType) : base(documentType)
		{
			this._documentType = documentType;
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x00008394 File Offset: 0x00006594
		public string Name
		{
			get
			{
				return this._documentType.Name;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001FA RID: 506 RVA: 0x000083A1 File Offset: 0x000065A1
		public string System
		{
			get
			{
				return this._documentType.SystemId;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001FB RID: 507 RVA: 0x000083AE File Offset: 0x000065AE
		public string Public
		{
			get
			{
				return this._documentType.PublicId;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001FC RID: 508 RVA: 0x000083BB File Offset: 0x000065BB
		public string InternalSubset
		{
			get
			{
				return this._documentType.InternalSubset;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001FD RID: 509 RVA: 0x000083C8 File Offset: 0x000065C8
		public override string LocalName
		{
			get
			{
				return "DOCTYPE";
			}
		}

		// Token: 0x040000A6 RID: 166
		private readonly XDocumentType _documentType;
	}
}
