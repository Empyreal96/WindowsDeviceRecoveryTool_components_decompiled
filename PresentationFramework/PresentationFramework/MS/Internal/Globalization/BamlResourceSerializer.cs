using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Markup;
using System.Windows.Markup.Localizer;

namespace MS.Internal.Globalization
{
	// Token: 0x0200069B RID: 1691
	internal sealed class BamlResourceSerializer
	{
		// Token: 0x06006E1C RID: 28188 RVA: 0x001FB2BA File Offset: 0x001F94BA
		internal static void Serialize(BamlLocalizer localizer, BamlTree tree, Stream output)
		{
			new BamlResourceSerializer().SerializeImp(localizer, tree, output);
		}

		// Token: 0x06006E1D RID: 28189 RVA: 0x0000326D File Offset: 0x0000146D
		private BamlResourceSerializer()
		{
		}

		// Token: 0x06006E1E RID: 28190 RVA: 0x001FB2CC File Offset: 0x001F94CC
		private void SerializeImp(BamlLocalizer localizer, BamlTree tree, Stream output)
		{
			this._writer = new BamlWriter(output);
			this._bamlTreeStack = new Stack<BamlTreeNode>();
			this._bamlTreeStack.Push(tree.Root);
			while (this._bamlTreeStack.Count > 0)
			{
				BamlTreeNode bamlTreeNode = this._bamlTreeStack.Pop();
				if (!bamlTreeNode.Visited)
				{
					bamlTreeNode.Visited = true;
					bamlTreeNode.Serialize(this._writer);
					this.PushChildrenToStack(bamlTreeNode.Children);
				}
				else
				{
					BamlStartElementNode bamlStartElementNode = bamlTreeNode as BamlStartElementNode;
					if (bamlStartElementNode != null)
					{
						localizer.RaiseErrorNotifyEvent(new BamlLocalizerErrorNotifyEventArgs(BamlTreeMap.GetKey(bamlStartElementNode), BamlLocalizerError.DuplicateElement));
					}
				}
			}
		}

		// Token: 0x06006E1F RID: 28191 RVA: 0x001FB364 File Offset: 0x001F9564
		private void PushChildrenToStack(List<BamlTreeNode> children)
		{
			if (children == null)
			{
				return;
			}
			for (int i = children.Count - 1; i >= 0; i--)
			{
				this._bamlTreeStack.Push(children[i]);
			}
		}

		// Token: 0x04003631 RID: 13873
		private BamlWriter _writer;

		// Token: 0x04003632 RID: 13874
		private Stack<BamlTreeNode> _bamlTreeStack;
	}
}
