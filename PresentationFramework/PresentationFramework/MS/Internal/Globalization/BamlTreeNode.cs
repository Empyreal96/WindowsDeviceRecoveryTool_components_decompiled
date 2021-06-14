using System;
using System.Collections.Generic;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006A0 RID: 1696
	internal abstract class BamlTreeNode
	{
		// Token: 0x06006E44 RID: 28228 RVA: 0x001FBF9A File Offset: 0x001FA19A
		internal BamlTreeNode(BamlNodeType type)
		{
			this.NodeType = type;
		}

		// Token: 0x06006E45 RID: 28229 RVA: 0x001FBFA9 File Offset: 0x001FA1A9
		internal void AddChild(BamlTreeNode child)
		{
			if (this._children == null)
			{
				this._children = new List<BamlTreeNode>();
			}
			this._children.Add(child);
			child.Parent = this;
		}

		// Token: 0x06006E46 RID: 28230
		internal abstract BamlTreeNode Copy();

		// Token: 0x06006E47 RID: 28231
		internal abstract void Serialize(BamlWriter writer);

		// Token: 0x17001A2D RID: 6701
		// (get) Token: 0x06006E48 RID: 28232 RVA: 0x001FBFD1 File Offset: 0x001FA1D1
		// (set) Token: 0x06006E49 RID: 28233 RVA: 0x001FBFD9 File Offset: 0x001FA1D9
		internal BamlNodeType NodeType
		{
			get
			{
				return this._nodeType;
			}
			set
			{
				this._nodeType = value;
			}
		}

		// Token: 0x17001A2E RID: 6702
		// (get) Token: 0x06006E4A RID: 28234 RVA: 0x001FBFE2 File Offset: 0x001FA1E2
		// (set) Token: 0x06006E4B RID: 28235 RVA: 0x001FBFEA File Offset: 0x001FA1EA
		internal List<BamlTreeNode> Children
		{
			get
			{
				return this._children;
			}
			set
			{
				this._children = value;
			}
		}

		// Token: 0x17001A2F RID: 6703
		// (get) Token: 0x06006E4C RID: 28236 RVA: 0x001FBFF3 File Offset: 0x001FA1F3
		// (set) Token: 0x06006E4D RID: 28237 RVA: 0x001FBFFB File Offset: 0x001FA1FB
		internal BamlTreeNode Parent
		{
			get
			{
				return this._parent;
			}
			set
			{
				this._parent = value;
			}
		}

		// Token: 0x17001A30 RID: 6704
		// (get) Token: 0x06006E4E RID: 28238 RVA: 0x001FC004 File Offset: 0x001FA204
		// (set) Token: 0x06006E4F RID: 28239 RVA: 0x001FC011 File Offset: 0x001FA211
		internal bool Formatted
		{
			get
			{
				return (this._state & BamlTreeNode.BamlTreeNodeState.ContentFormatted) > BamlTreeNode.BamlTreeNodeState.None;
			}
			set
			{
				if (value)
				{
					this._state |= BamlTreeNode.BamlTreeNodeState.ContentFormatted;
					return;
				}
				this._state &= ~BamlTreeNode.BamlTreeNodeState.ContentFormatted;
			}
		}

		// Token: 0x17001A31 RID: 6705
		// (get) Token: 0x06006E50 RID: 28240 RVA: 0x001FC037 File Offset: 0x001FA237
		// (set) Token: 0x06006E51 RID: 28241 RVA: 0x001FC044 File Offset: 0x001FA244
		internal bool Visited
		{
			get
			{
				return (this._state & BamlTreeNode.BamlTreeNodeState.NodeVisited) > BamlTreeNode.BamlTreeNodeState.None;
			}
			set
			{
				if (value)
				{
					this._state |= BamlTreeNode.BamlTreeNodeState.NodeVisited;
					return;
				}
				this._state &= ~BamlTreeNode.BamlTreeNodeState.NodeVisited;
			}
		}

		// Token: 0x17001A32 RID: 6706
		// (get) Token: 0x06006E52 RID: 28242 RVA: 0x001FC06A File Offset: 0x001FA26A
		// (set) Token: 0x06006E53 RID: 28243 RVA: 0x001FC077 File Offset: 0x001FA277
		internal bool Unidentifiable
		{
			get
			{
				return (this._state & BamlTreeNode.BamlTreeNodeState.Unidentifiable) > BamlTreeNode.BamlTreeNodeState.None;
			}
			set
			{
				if (value)
				{
					this._state |= BamlTreeNode.BamlTreeNodeState.Unidentifiable;
					return;
				}
				this._state &= ~BamlTreeNode.BamlTreeNodeState.Unidentifiable;
			}
		}

		// Token: 0x04003645 RID: 13893
		protected BamlNodeType _nodeType;

		// Token: 0x04003646 RID: 13894
		protected List<BamlTreeNode> _children;

		// Token: 0x04003647 RID: 13895
		protected BamlTreeNode _parent;

		// Token: 0x04003648 RID: 13896
		private BamlTreeNode.BamlTreeNodeState _state;

		// Token: 0x02000B2A RID: 2858
		[Flags]
		private enum BamlTreeNodeState : byte
		{
			// Token: 0x04004A7A RID: 19066
			None = 0,
			// Token: 0x04004A7B RID: 19067
			ContentFormatted = 1,
			// Token: 0x04004A7C RID: 19068
			NodeVisited = 2,
			// Token: 0x04004A7D RID: 19069
			Unidentifiable = 4
		}
	}
}
