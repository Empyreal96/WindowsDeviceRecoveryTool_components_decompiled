using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006A6 RID: 1702
	internal sealed class BamlXmlnsPropertyNode : BamlTreeNode
	{
		// Token: 0x06006E74 RID: 28276 RVA: 0x001FC2C4 File Offset: 0x001FA4C4
		internal BamlXmlnsPropertyNode(string prefix, string xmlns) : base(BamlNodeType.XmlnsProperty)
		{
			this._prefix = prefix;
			this._xmlns = xmlns;
		}

		// Token: 0x06006E75 RID: 28277 RVA: 0x001FC2DB File Offset: 0x001FA4DB
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteXmlnsProperty(this._prefix, this._xmlns);
		}

		// Token: 0x06006E76 RID: 28278 RVA: 0x001FC2EF File Offset: 0x001FA4EF
		internal override BamlTreeNode Copy()
		{
			return new BamlXmlnsPropertyNode(this._prefix, this._xmlns);
		}

		// Token: 0x04003653 RID: 13907
		private string _prefix;

		// Token: 0x04003654 RID: 13908
		private string _xmlns;
	}
}
