using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006B0 RID: 1712
	internal sealed class BamlStartConstructorNode : BamlTreeNode
	{
		// Token: 0x06006EA3 RID: 28323 RVA: 0x001FC652 File Offset: 0x001FA852
		internal BamlStartConstructorNode() : base(BamlNodeType.StartConstructor)
		{
		}

		// Token: 0x06006EA4 RID: 28324 RVA: 0x001FC65C File Offset: 0x001FA85C
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteStartConstructor();
		}

		// Token: 0x06006EA5 RID: 28325 RVA: 0x001FC664 File Offset: 0x001FA864
		internal override BamlTreeNode Copy()
		{
			return new BamlStartConstructorNode();
		}
	}
}
