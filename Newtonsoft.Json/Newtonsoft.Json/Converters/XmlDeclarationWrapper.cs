using System;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200002E RID: 46
	internal class XmlDeclarationWrapper : XmlNodeWrapper, IXmlDeclaration, IXmlNode
	{
		// Token: 0x060001D4 RID: 468 RVA: 0x00008236 File Offset: 0x00006436
		public XmlDeclarationWrapper(XmlDeclaration declaration) : base(declaration)
		{
			this._declaration = declaration;
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x00008246 File Offset: 0x00006446
		public string Version
		{
			get
			{
				return this._declaration.Version;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x00008253 File Offset: 0x00006453
		// (set) Token: 0x060001D7 RID: 471 RVA: 0x00008260 File Offset: 0x00006460
		public string Encoding
		{
			get
			{
				return this._declaration.Encoding;
			}
			set
			{
				this._declaration.Encoding = value;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x0000826E File Offset: 0x0000646E
		// (set) Token: 0x060001D9 RID: 473 RVA: 0x0000827B File Offset: 0x0000647B
		public string Standalone
		{
			get
			{
				return this._declaration.Standalone;
			}
			set
			{
				this._declaration.Standalone = value;
			}
		}

		// Token: 0x040000A2 RID: 162
		private readonly XmlDeclaration _declaration;
	}
}
