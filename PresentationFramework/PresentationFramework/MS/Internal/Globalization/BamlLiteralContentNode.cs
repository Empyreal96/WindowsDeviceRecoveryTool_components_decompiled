using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006AA RID: 1706
	internal sealed class BamlLiteralContentNode : BamlTreeNode
	{
		// Token: 0x06006E8D RID: 28301 RVA: 0x001FC47A File Offset: 0x001FA67A
		internal BamlLiteralContentNode(string literalContent) : base(BamlNodeType.LiteralContent)
		{
			this._literalContent = literalContent;
		}

		// Token: 0x06006E8E RID: 28302 RVA: 0x001FC48B File Offset: 0x001FA68B
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteLiteralContent(this._literalContent);
		}

		// Token: 0x06006E8F RID: 28303 RVA: 0x001FC499 File Offset: 0x001FA699
		internal override BamlTreeNode Copy()
		{
			return new BamlLiteralContentNode(this._literalContent);
		}

		// Token: 0x17001A45 RID: 6725
		// (get) Token: 0x06006E90 RID: 28304 RVA: 0x001FC4A6 File Offset: 0x001FA6A6
		// (set) Token: 0x06006E91 RID: 28305 RVA: 0x001FC4AE File Offset: 0x001FA6AE
		internal string Content
		{
			get
			{
				return this._literalContent;
			}
			set
			{
				this._literalContent = value;
			}
		}

		// Token: 0x0400365E RID: 13918
		private string _literalContent;
	}
}
