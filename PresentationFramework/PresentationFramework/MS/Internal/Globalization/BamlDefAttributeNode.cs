using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006AE RID: 1710
	internal sealed class BamlDefAttributeNode : BamlTreeNode
	{
		// Token: 0x06006E9D RID: 28317 RVA: 0x001FC5C1 File Offset: 0x001FA7C1
		internal BamlDefAttributeNode(string name, string value) : base(BamlNodeType.DefAttribute)
		{
			this._name = name;
			this._value = value;
		}

		// Token: 0x06006E9E RID: 28318 RVA: 0x001FC5D9 File Offset: 0x001FA7D9
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteDefAttribute(this._name, this._value);
		}

		// Token: 0x06006E9F RID: 28319 RVA: 0x001FC5ED File Offset: 0x001FA7ED
		internal override BamlTreeNode Copy()
		{
			return new BamlDefAttributeNode(this._name, this._value);
		}

		// Token: 0x04003668 RID: 13928
		private string _name;

		// Token: 0x04003669 RID: 13929
		private string _value;
	}
}
