using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006A2 RID: 1698
	internal sealed class BamlEndDocumentNode : BamlTreeNode
	{
		// Token: 0x06006E5C RID: 28252 RVA: 0x001FC0DB File Offset: 0x001FA2DB
		internal BamlEndDocumentNode() : base(BamlNodeType.EndDocument)
		{
		}

		// Token: 0x06006E5D RID: 28253 RVA: 0x001FC0E4 File Offset: 0x001FA2E4
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteEndDocument();
		}

		// Token: 0x06006E5E RID: 28254 RVA: 0x001FC0EC File Offset: 0x001FA2EC
		internal override BamlTreeNode Copy()
		{
			return new BamlEndDocumentNode();
		}
	}
}
