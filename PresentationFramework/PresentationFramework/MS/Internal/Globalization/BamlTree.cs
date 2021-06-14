using System;
using System.Collections.Generic;

namespace MS.Internal.Globalization
{
	// Token: 0x0200069E RID: 1694
	internal sealed class BamlTree
	{
		// Token: 0x06006E37 RID: 28215 RVA: 0x0000326D File Offset: 0x0000146D
		internal BamlTree()
		{
		}

		// Token: 0x06006E38 RID: 28216 RVA: 0x001FBE78 File Offset: 0x001FA078
		internal BamlTree(BamlTreeNode root, int size)
		{
			this._root = root;
			this._nodeList = new List<BamlTreeNode>(size);
			this.CreateInternalIndex(ref this._root, ref this._nodeList, false);
		}

		// Token: 0x17001A27 RID: 6695
		// (get) Token: 0x06006E39 RID: 28217 RVA: 0x001FBEA6 File Offset: 0x001FA0A6
		internal BamlTreeNode Root
		{
			get
			{
				return this._root;
			}
		}

		// Token: 0x17001A28 RID: 6696
		// (get) Token: 0x06006E3A RID: 28218 RVA: 0x001FBEAE File Offset: 0x001FA0AE
		internal int Size
		{
			get
			{
				return this._nodeList.Count;
			}
		}

		// Token: 0x17001A29 RID: 6697
		internal BamlTreeNode this[int i]
		{
			get
			{
				return this._nodeList[i];
			}
		}

		// Token: 0x06006E3C RID: 28220 RVA: 0x001FBECC File Offset: 0x001FA0CC
		internal BamlTree Copy()
		{
			BamlTreeNode root = this._root;
			List<BamlTreeNode> nodeList = new List<BamlTreeNode>(this.Size);
			this.CreateInternalIndex(ref root, ref nodeList, true);
			return new BamlTree
			{
				_root = root,
				_nodeList = nodeList
			};
		}

		// Token: 0x06006E3D RID: 28221 RVA: 0x001FBF0C File Offset: 0x001FA10C
		internal void AddTreeNode(BamlTreeNode node)
		{
			this._nodeList.Add(node);
		}

		// Token: 0x06006E3E RID: 28222 RVA: 0x001FBF1C File Offset: 0x001FA11C
		private void CreateInternalIndex(ref BamlTreeNode parent, ref List<BamlTreeNode> nodeList, bool toCopy)
		{
			List<BamlTreeNode> children = parent.Children;
			if (toCopy)
			{
				parent = parent.Copy();
				if (children != null)
				{
					parent.Children = new List<BamlTreeNode>(children.Count);
				}
			}
			nodeList.Add(parent);
			if (children == null)
			{
				return;
			}
			for (int i = 0; i < children.Count; i++)
			{
				BamlTreeNode bamlTreeNode = children[i];
				this.CreateInternalIndex(ref bamlTreeNode, ref nodeList, toCopy);
				if (toCopy)
				{
					bamlTreeNode.Parent = parent;
					parent.Children.Add(bamlTreeNode);
				}
			}
		}

		// Token: 0x04003643 RID: 13891
		private BamlTreeNode _root;

		// Token: 0x04003644 RID: 13892
		private List<BamlTreeNode> _nodeList;
	}
}
