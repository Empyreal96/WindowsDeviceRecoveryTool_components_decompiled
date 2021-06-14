using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006B3 RID: 1715
	internal sealed class BamlPresentationOptionsAttributeNode : BamlTreeNode
	{
		// Token: 0x06006EAC RID: 28332 RVA: 0x001FC6D5 File Offset: 0x001FA8D5
		internal BamlPresentationOptionsAttributeNode(string name, string value) : base(BamlNodeType.PresentationOptionsAttribute)
		{
			this._name = name;
			this._value = value;
		}

		// Token: 0x06006EAD RID: 28333 RVA: 0x001FC6ED File Offset: 0x001FA8ED
		internal override void Serialize(BamlWriter writer)
		{
			writer.WritePresentationOptionsAttribute(this._name, this._value);
		}

		// Token: 0x06006EAE RID: 28334 RVA: 0x001FC701 File Offset: 0x001FA901
		internal override BamlTreeNode Copy()
		{
			return new BamlPresentationOptionsAttributeNode(this._name, this._value);
		}

		// Token: 0x04003670 RID: 13936
		private string _name;

		// Token: 0x04003671 RID: 13937
		private string _value;
	}
}
