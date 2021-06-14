using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006A5 RID: 1701
	internal sealed class BamlEndElementNode : BamlTreeNode
	{
		// Token: 0x06006E71 RID: 28273 RVA: 0x001FC2AC File Offset: 0x001FA4AC
		internal BamlEndElementNode() : base(BamlNodeType.EndElement)
		{
		}

		// Token: 0x06006E72 RID: 28274 RVA: 0x001FC2B5 File Offset: 0x001FA4B5
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteEndElement();
		}

		// Token: 0x06006E73 RID: 28275 RVA: 0x001FC2BD File Offset: 0x001FA4BD
		internal override BamlTreeNode Copy()
		{
			return new BamlEndElementNode();
		}
	}
}
