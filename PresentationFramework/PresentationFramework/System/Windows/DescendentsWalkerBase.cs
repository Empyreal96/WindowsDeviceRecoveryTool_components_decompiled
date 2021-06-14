using System;
using System.Windows.Media;
using MS.Utility;

namespace System.Windows
{
	// Token: 0x020000B3 RID: 179
	internal class DescendentsWalkerBase
	{
		// Token: 0x060003CD RID: 973 RVA: 0x0000AF87 File Offset: 0x00009187
		protected DescendentsWalkerBase(TreeWalkPriority priority)
		{
			this._startNode = null;
			this._priority = priority;
			this._recursionDepth = 0;
			this._nodes = default(FrugalStructList<DependencyObject>);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000AFB0 File Offset: 0x000091B0
		internal bool WasVisited(DependencyObject d)
		{
			DependencyObject dependencyObject = d;
			while (dependencyObject != this._startNode && dependencyObject != null)
			{
				DependencyObject dependencyObject2;
				if (FrameworkElement.DType.IsInstanceOfType(dependencyObject))
				{
					FrameworkElement frameworkElement = dependencyObject as FrameworkElement;
					dependencyObject2 = frameworkElement.Parent;
					DependencyObject parent = VisualTreeHelper.GetParent(frameworkElement);
					if (parent != null && dependencyObject2 != null && parent != dependencyObject2)
					{
						return this._nodes.Contains(dependencyObject);
					}
					if (parent != null)
					{
						dependencyObject = parent;
						continue;
					}
				}
				else
				{
					FrameworkContentElement frameworkContentElement = dependencyObject as FrameworkContentElement;
					dependencyObject2 = ((frameworkContentElement != null) ? frameworkContentElement.Parent : null);
				}
				dependencyObject = dependencyObject2;
			}
			return dependencyObject != null;
		}

		// Token: 0x0400060E RID: 1550
		internal DependencyObject _startNode;

		// Token: 0x0400060F RID: 1551
		internal TreeWalkPriority _priority;

		// Token: 0x04000610 RID: 1552
		internal FrugalStructList<DependencyObject> _nodes;

		// Token: 0x04000611 RID: 1553
		internal int _recursionDepth;
	}
}
