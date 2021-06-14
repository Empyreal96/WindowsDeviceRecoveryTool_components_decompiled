using System;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200002D RID: 45
	internal interface IXmlDeclaration : IXmlNode
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001CF RID: 463
		string Version { get; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001D0 RID: 464
		// (set) Token: 0x060001D1 RID: 465
		string Encoding { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001D2 RID: 466
		// (set) Token: 0x060001D3 RID: 467
		string Standalone { get; set; }
	}
}
