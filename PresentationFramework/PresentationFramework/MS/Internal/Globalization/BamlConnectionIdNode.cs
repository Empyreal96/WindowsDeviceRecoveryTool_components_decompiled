using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006A3 RID: 1699
	internal sealed class BamlConnectionIdNode : BamlTreeNode
	{
		// Token: 0x06006E5F RID: 28255 RVA: 0x001FC0F3 File Offset: 0x001FA2F3
		internal BamlConnectionIdNode(int connectionId) : base(BamlNodeType.ConnectionId)
		{
			this._connectionId = connectionId;
		}

		// Token: 0x06006E60 RID: 28256 RVA: 0x001FC103 File Offset: 0x001FA303
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteConnectionId(this._connectionId);
		}

		// Token: 0x06006E61 RID: 28257 RVA: 0x001FC111 File Offset: 0x001FA311
		internal override BamlTreeNode Copy()
		{
			return new BamlConnectionIdNode(this._connectionId);
		}

		// Token: 0x04003649 RID: 13897
		private int _connectionId;
	}
}
