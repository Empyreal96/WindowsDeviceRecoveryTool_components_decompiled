using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006A8 RID: 1704
	internal sealed class BamlEndComplexPropertyNode : BamlTreeNode
	{
		// Token: 0x06006E83 RID: 28291 RVA: 0x001FC39F File Offset: 0x001FA59F
		internal BamlEndComplexPropertyNode() : base(BamlNodeType.EndComplexProperty)
		{
		}

		// Token: 0x06006E84 RID: 28292 RVA: 0x001FC3A9 File Offset: 0x001FA5A9
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteEndComplexProperty();
		}

		// Token: 0x06006E85 RID: 28293 RVA: 0x001FC3B1 File Offset: 0x001FA5B1
		internal override BamlTreeNode Copy()
		{
			return new BamlEndComplexPropertyNode();
		}
	}
}
