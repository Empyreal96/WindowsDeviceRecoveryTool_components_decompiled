using System;
using System.Collections;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Media;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020007D6 RID: 2006
	internal sealed class PathNode
	{
		// Token: 0x06007C1B RID: 31771 RVA: 0x0022E9AE File Offset: 0x0022CBAE
		internal PathNode(DependencyObject node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			this._node = node;
		}

		// Token: 0x06007C1C RID: 31772 RVA: 0x0022E9D8 File Offset: 0x0022CBD8
		public override bool Equals(object obj)
		{
			PathNode pathNode = obj as PathNode;
			return pathNode != null && this._node.Equals(pathNode.Node);
		}

		// Token: 0x06007C1D RID: 31773 RVA: 0x0022EA02 File Offset: 0x0022CC02
		public override int GetHashCode()
		{
			if (this._node == null)
			{
				return base.GetHashCode();
			}
			return this._node.GetHashCode();
		}

		// Token: 0x17001CE8 RID: 7400
		// (get) Token: 0x06007C1E RID: 31774 RVA: 0x0022EA1E File Offset: 0x0022CC1E
		public DependencyObject Node
		{
			get
			{
				return this._node;
			}
		}

		// Token: 0x17001CE9 RID: 7401
		// (get) Token: 0x06007C1F RID: 31775 RVA: 0x0022EA26 File Offset: 0x0022CC26
		public IList Children
		{
			get
			{
				return this._children;
			}
		}

		// Token: 0x06007C20 RID: 31776 RVA: 0x0022EA30 File Offset: 0x0022CC30
		internal static PathNode BuildPathForElements(ICollection nodes)
		{
			if (nodes == null)
			{
				throw new ArgumentNullException("nodes");
			}
			PathNode pathNode = null;
			foreach (object obj in nodes)
			{
				DependencyObject node = (DependencyObject)obj;
				PathNode pathNode2 = PathNode.BuildPathForElement(node);
				if (pathNode == null)
				{
					pathNode = pathNode2;
				}
				else
				{
					PathNode.AddBranchToPath(pathNode, pathNode2);
				}
			}
			if (pathNode != null)
			{
				pathNode.FreezeChildren();
			}
			return pathNode;
		}

		// Token: 0x06007C21 RID: 31777 RVA: 0x0022EAB0 File Offset: 0x0022CCB0
		internal static DependencyObject GetParent(DependencyObject node)
		{
			DependencyObject dependencyObject = node;
			DependencyObject dependencyObject2;
			for (;;)
			{
				dependencyObject2 = (DependencyObject)dependencyObject.GetValue(PathNode.HiddenParentProperty);
				if (dependencyObject2 == null)
				{
					Visual visual = dependencyObject as Visual;
					if (visual != null)
					{
						dependencyObject2 = VisualTreeHelper.GetParent(visual);
					}
				}
				if (dependencyObject2 == null)
				{
					dependencyObject2 = LogicalTreeHelper.GetParent(dependencyObject);
				}
				if (dependencyObject2 == null || FrameworkElement.DType.IsInstanceOfType(dependencyObject2) || FrameworkContentElement.DType.IsInstanceOfType(dependencyObject2))
				{
					break;
				}
				dependencyObject = dependencyObject2;
			}
			return dependencyObject2;
		}

		// Token: 0x06007C22 RID: 31778 RVA: 0x0022EB14 File Offset: 0x0022CD14
		private static PathNode BuildPathForElement(DependencyObject node)
		{
			PathNode pathNode = null;
			while (node != null)
			{
				PathNode pathNode2 = new PathNode(node);
				if (pathNode != null)
				{
					pathNode2.AddChild(pathNode);
				}
				pathNode = pathNode2;
				if (node.ReadLocalValue(AnnotationService.ServiceProperty) != DependencyProperty.UnsetValue)
				{
					break;
				}
				node = PathNode.GetParent(node);
			}
			return pathNode;
		}

		// Token: 0x06007C23 RID: 31779 RVA: 0x0022EB58 File Offset: 0x0022CD58
		private static PathNode AddBranchToPath(PathNode path, PathNode branch)
		{
			PathNode pathNode = path;
			PathNode pathNode2 = branch;
			while (pathNode.Node.Equals(pathNode2.Node) && pathNode2._children.Count > 0)
			{
				bool flag = false;
				PathNode pathNode3 = (PathNode)pathNode2._children[0];
				foreach (object obj in pathNode._children)
				{
					PathNode pathNode4 = (PathNode)obj;
					if (pathNode4.Equals(pathNode3))
					{
						flag = true;
						pathNode2 = pathNode3;
						pathNode = pathNode4;
						break;
					}
				}
				if (!flag)
				{
					pathNode.AddChild(pathNode3);
					break;
				}
			}
			return path;
		}

		// Token: 0x06007C24 RID: 31780 RVA: 0x0022EC10 File Offset: 0x0022CE10
		private void AddChild(object child)
		{
			this._children.Add(child);
		}

		// Token: 0x06007C25 RID: 31781 RVA: 0x0022EC20 File Offset: 0x0022CE20
		private void FreezeChildren()
		{
			foreach (object obj in this._children)
			{
				PathNode pathNode = (PathNode)obj;
				pathNode.FreezeChildren();
			}
			this._children = ArrayList.ReadOnly(this._children);
		}

		// Token: 0x04003A56 RID: 14934
		internal static readonly DependencyProperty HiddenParentProperty = DependencyProperty.RegisterAttached("HiddenParent", typeof(DependencyObject), typeof(PathNode));

		// Token: 0x04003A57 RID: 14935
		private DependencyObject _node;

		// Token: 0x04003A58 RID: 14936
		private ArrayList _children = new ArrayList(1);
	}
}
