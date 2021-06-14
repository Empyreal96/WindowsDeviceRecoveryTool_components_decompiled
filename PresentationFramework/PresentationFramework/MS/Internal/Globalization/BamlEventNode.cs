using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006AD RID: 1709
	internal sealed class BamlEventNode : BamlTreeNode
	{
		// Token: 0x06006E9A RID: 28314 RVA: 0x001FC582 File Offset: 0x001FA782
		internal BamlEventNode(string eventName, string handlerName) : base(BamlNodeType.Event)
		{
			this._eventName = eventName;
			this._handlerName = handlerName;
		}

		// Token: 0x06006E9B RID: 28315 RVA: 0x001FC59A File Offset: 0x001FA79A
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteEvent(this._eventName, this._handlerName);
		}

		// Token: 0x06006E9C RID: 28316 RVA: 0x001FC5AE File Offset: 0x001FA7AE
		internal override BamlTreeNode Copy()
		{
			return new BamlEventNode(this._eventName, this._handlerName);
		}

		// Token: 0x04003666 RID: 13926
		private string _eventName;

		// Token: 0x04003667 RID: 13927
		private string _handlerName;
	}
}
