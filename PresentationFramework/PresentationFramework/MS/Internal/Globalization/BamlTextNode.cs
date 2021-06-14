using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006AB RID: 1707
	internal sealed class BamlTextNode : BamlTreeNode
	{
		// Token: 0x06006E92 RID: 28306 RVA: 0x001FC4B7 File Offset: 0x001FA6B7
		internal BamlTextNode(string text) : this(text, null, null)
		{
		}

		// Token: 0x06006E93 RID: 28307 RVA: 0x001FC4C2 File Offset: 0x001FA6C2
		internal BamlTextNode(string text, string typeConverterAssemblyName, string typeConverterName) : base(BamlNodeType.Text)
		{
			this._content = text;
			this._typeConverterAssemblyName = typeConverterAssemblyName;
			this._typeConverterName = typeConverterName;
		}

		// Token: 0x06006E94 RID: 28308 RVA: 0x001FC4E1 File Offset: 0x001FA6E1
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteText(this._content, this._typeConverterAssemblyName, this._typeConverterName);
		}

		// Token: 0x06006E95 RID: 28309 RVA: 0x001FC4FB File Offset: 0x001FA6FB
		internal override BamlTreeNode Copy()
		{
			return new BamlTextNode(this._content, this._typeConverterAssemblyName, this._typeConverterName);
		}

		// Token: 0x17001A46 RID: 6726
		// (get) Token: 0x06006E96 RID: 28310 RVA: 0x001FC514 File Offset: 0x001FA714
		internal string Content
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x0400365F RID: 13919
		private string _content;

		// Token: 0x04003660 RID: 13920
		private string _typeConverterAssemblyName;

		// Token: 0x04003661 RID: 13921
		private string _typeConverterName;
	}
}
