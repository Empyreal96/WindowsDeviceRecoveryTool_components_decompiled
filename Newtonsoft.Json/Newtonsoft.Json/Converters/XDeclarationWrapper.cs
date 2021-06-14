using System;
using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000032 RID: 50
	internal class XDeclarationWrapper : XObjectWrapper, IXmlDeclaration, IXmlNode
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001EF RID: 495 RVA: 0x0000831C File Offset: 0x0000651C
		// (set) Token: 0x060001F0 RID: 496 RVA: 0x00008324 File Offset: 0x00006524
		internal XDeclaration Declaration { get; private set; }

		// Token: 0x060001F1 RID: 497 RVA: 0x0000832D File Offset: 0x0000652D
		public XDeclarationWrapper(XDeclaration declaration) : base(null)
		{
			this.Declaration = declaration;
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x0000833D File Offset: 0x0000653D
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.XmlDeclaration;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x00008341 File Offset: 0x00006541
		public string Version
		{
			get
			{
				return this.Declaration.Version;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x0000834E File Offset: 0x0000654E
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x0000835B File Offset: 0x0000655B
		public string Encoding
		{
			get
			{
				return this.Declaration.Encoding;
			}
			set
			{
				this.Declaration.Encoding = value;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x00008369 File Offset: 0x00006569
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x00008376 File Offset: 0x00006576
		public string Standalone
		{
			get
			{
				return this.Declaration.Standalone;
			}
			set
			{
				this.Declaration.Standalone = value;
			}
		}
	}
}
