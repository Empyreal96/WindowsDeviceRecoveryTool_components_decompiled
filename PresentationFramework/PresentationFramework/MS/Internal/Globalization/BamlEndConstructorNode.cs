using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006B1 RID: 1713
	internal sealed class BamlEndConstructorNode : BamlTreeNode
	{
		// Token: 0x06006EA6 RID: 28326 RVA: 0x001FC66B File Offset: 0x001FA86B
		internal BamlEndConstructorNode() : base(BamlNodeType.EndConstructor)
		{
		}

		// Token: 0x06006EA7 RID: 28327 RVA: 0x001FC675 File Offset: 0x001FA875
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteEndConstructor();
		}

		// Token: 0x06006EA8 RID: 28328 RVA: 0x001FC67D File Offset: 0x001FA87D
		internal override BamlTreeNode Copy()
		{
			return new BamlEndConstructorNode();
		}
	}
}
