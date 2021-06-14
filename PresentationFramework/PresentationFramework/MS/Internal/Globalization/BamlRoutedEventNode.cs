using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006AC RID: 1708
	internal sealed class BamlRoutedEventNode : BamlTreeNode
	{
		// Token: 0x06006E97 RID: 28311 RVA: 0x001FC51C File Offset: 0x001FA71C
		internal BamlRoutedEventNode(string assemblyName, string ownerTypeFullName, string eventIdName, string handlerName) : base(BamlNodeType.RoutedEvent)
		{
			this._assemblyName = assemblyName;
			this._ownerTypeFullName = ownerTypeFullName;
			this._eventIdName = eventIdName;
			this._handlerName = handlerName;
		}

		// Token: 0x06006E98 RID: 28312 RVA: 0x001FC543 File Offset: 0x001FA743
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteRoutedEvent(this._assemblyName, this._ownerTypeFullName, this._eventIdName, this._handlerName);
		}

		// Token: 0x06006E99 RID: 28313 RVA: 0x001FC563 File Offset: 0x001FA763
		internal override BamlTreeNode Copy()
		{
			return new BamlRoutedEventNode(this._assemblyName, this._ownerTypeFullName, this._eventIdName, this._handlerName);
		}

		// Token: 0x04003662 RID: 13922
		private string _assemblyName;

		// Token: 0x04003663 RID: 13923
		private string _ownerTypeFullName;

		// Token: 0x04003664 RID: 13924
		private string _eventIdName;

		// Token: 0x04003665 RID: 13925
		private string _handlerName;
	}
}
