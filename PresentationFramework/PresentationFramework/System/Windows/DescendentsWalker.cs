using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace System.Windows
{
	// Token: 0x020000B1 RID: 177
	internal class DescendentsWalker<T> : DescendentsWalkerBase
	{
		// Token: 0x060003BB RID: 955 RVA: 0x0000AA1C File Offset: 0x00008C1C
		public DescendentsWalker(TreeWalkPriority priority, VisitedCallback<T> callback) : this(priority, callback, default(T))
		{
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000AA3A File Offset: 0x00008C3A
		public DescendentsWalker(TreeWalkPriority priority, VisitedCallback<T> callback, T data) : base(priority)
		{
			this._callback = callback;
			this._data = data;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000AA51 File Offset: 0x00008C51
		public void StartWalk(DependencyObject startNode)
		{
			this.StartWalk(startNode, false);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000AA5C File Offset: 0x00008C5C
		public virtual void StartWalk(DependencyObject startNode, bool skipStartNode)
		{
			this._startNode = startNode;
			bool flag = true;
			if (!skipStartNode && (FrameworkElement.DType.IsInstanceOfType(this._startNode) || FrameworkContentElement.DType.IsInstanceOfType(this._startNode)))
			{
				flag = this._callback(this._startNode, this._data, this._priority == TreeWalkPriority.VisualTree);
			}
			if (flag)
			{
				this.IterateChildren(this._startNode);
			}
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000AACC File Offset: 0x00008CCC
		private void IterateChildren(DependencyObject d)
		{
			this._recursionDepth++;
			if (FrameworkElement.DType.IsInstanceOfType(d))
			{
				FrameworkElement frameworkElement = (FrameworkElement)d;
				bool hasLogicalChildren = frameworkElement.HasLogicalChildren;
				if (this._priority == TreeWalkPriority.VisualTree)
				{
					this.WalkFrameworkElementVisualThenLogicalChildren(frameworkElement, hasLogicalChildren);
				}
				else if (this._priority == TreeWalkPriority.LogicalTree)
				{
					this.WalkFrameworkElementLogicalThenVisualChildren(frameworkElement, hasLogicalChildren);
				}
			}
			else if (FrameworkContentElement.DType.IsInstanceOfType(d))
			{
				FrameworkContentElement frameworkContentElement = d as FrameworkContentElement;
				if (frameworkContentElement.HasLogicalChildren)
				{
					this.WalkLogicalChildren(null, frameworkContentElement, frameworkContentElement.LogicalChildren);
				}
			}
			else
			{
				Visual visual = d as Visual;
				if (visual != null)
				{
					this.WalkVisualChildren(visual);
				}
				else
				{
					Visual3D visual3D = d as Visual3D;
					if (visual3D != null)
					{
						this.WalkVisualChildren(visual3D);
					}
				}
			}
			this._recursionDepth--;
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000AB88 File Offset: 0x00008D88
		private void WalkVisualChildren(Visual v)
		{
			v.IsVisualChildrenIterationInProgress = true;
			try
			{
				int internalVisual2DOr3DChildrenCount = v.InternalVisual2DOr3DChildrenCount;
				for (int i = 0; i < internalVisual2DOr3DChildrenCount; i++)
				{
					DependencyObject dependencyObject = v.InternalGet2DOr3DVisualChild(i);
					if (dependencyObject != null)
					{
						bool visitedViaVisualTree = true;
						this.VisitNode(dependencyObject, visitedViaVisualTree);
					}
				}
			}
			finally
			{
				v.IsVisualChildrenIterationInProgress = false;
			}
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000ABE0 File Offset: 0x00008DE0
		private void WalkVisualChildren(Visual3D v)
		{
			v.IsVisualChildrenIterationInProgress = true;
			try
			{
				int internalVisual2DOr3DChildrenCount = v.InternalVisual2DOr3DChildrenCount;
				for (int i = 0; i < internalVisual2DOr3DChildrenCount; i++)
				{
					DependencyObject dependencyObject = v.InternalGet2DOr3DVisualChild(i);
					if (dependencyObject != null)
					{
						bool visitedViaVisualTree = true;
						this.VisitNode(dependencyObject, visitedViaVisualTree);
					}
				}
			}
			finally
			{
				v.IsVisualChildrenIterationInProgress = false;
			}
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000AC38 File Offset: 0x00008E38
		private void WalkLogicalChildren(FrameworkElement feParent, FrameworkContentElement fceParent, IEnumerator logicalChildren)
		{
			if (feParent != null)
			{
				feParent.IsLogicalChildrenIterationInProgress = true;
			}
			else
			{
				fceParent.IsLogicalChildrenIterationInProgress = true;
			}
			try
			{
				if (logicalChildren != null)
				{
					while (logicalChildren.MoveNext())
					{
						object obj = logicalChildren.Current;
						DependencyObject dependencyObject = obj as DependencyObject;
						if (dependencyObject != null)
						{
							bool visitedViaVisualTree = false;
							this.VisitNode(dependencyObject, visitedViaVisualTree);
						}
					}
				}
			}
			finally
			{
				if (feParent != null)
				{
					feParent.IsLogicalChildrenIterationInProgress = false;
				}
				else
				{
					fceParent.IsLogicalChildrenIterationInProgress = false;
				}
			}
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000ACA8 File Offset: 0x00008EA8
		private void WalkFrameworkElementVisualThenLogicalChildren(FrameworkElement feParent, bool hasLogicalChildren)
		{
			this.WalkVisualChildren(feParent);
			List<Popup> value = Popup.RegisteredPopupsField.GetValue(feParent);
			if (value != null)
			{
				foreach (Popup fe in value)
				{
					bool visitedViaVisualTree = false;
					this.VisitNode(fe, visitedViaVisualTree);
				}
			}
			feParent.IsLogicalChildrenIterationInProgress = true;
			try
			{
				if (hasLogicalChildren)
				{
					IEnumerator logicalChildren = feParent.LogicalChildren;
					if (logicalChildren != null)
					{
						while (logicalChildren.MoveNext())
						{
							object obj = logicalChildren.Current;
							FrameworkElement frameworkElement = obj as FrameworkElement;
							if (frameworkElement != null)
							{
								if (VisualTreeHelper.GetParent(frameworkElement) != frameworkElement.Parent)
								{
									bool visitedViaVisualTree2 = false;
									this.VisitNode(frameworkElement, visitedViaVisualTree2);
								}
							}
							else
							{
								FrameworkContentElement frameworkContentElement = obj as FrameworkContentElement;
								if (frameworkContentElement != null)
								{
									bool visitedViaVisualTree3 = false;
									this.VisitNode(frameworkContentElement, visitedViaVisualTree3);
								}
							}
						}
					}
				}
			}
			finally
			{
				feParent.IsLogicalChildrenIterationInProgress = false;
			}
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000AD98 File Offset: 0x00008F98
		private void WalkFrameworkElementLogicalThenVisualChildren(FrameworkElement feParent, bool hasLogicalChildren)
		{
			if (hasLogicalChildren)
			{
				this.WalkLogicalChildren(feParent, null, feParent.LogicalChildren);
			}
			feParent.IsVisualChildrenIterationInProgress = true;
			try
			{
				int internalVisualChildrenCount = feParent.InternalVisualChildrenCount;
				for (int i = 0; i < internalVisualChildrenCount; i++)
				{
					Visual visual = feParent.InternalGetVisualChild(i);
					if (visual != null && FrameworkElement.DType.IsInstanceOfType(visual) && VisualTreeHelper.GetParent(visual) != ((FrameworkElement)visual).Parent)
					{
						bool visitedViaVisualTree = true;
						this.VisitNode(visual, visitedViaVisualTree);
					}
				}
			}
			finally
			{
				feParent.IsVisualChildrenIterationInProgress = false;
			}
			List<Popup> value = Popup.RegisteredPopupsField.GetValue(feParent);
			if (value != null)
			{
				foreach (Popup fe in value)
				{
					bool visitedViaVisualTree2 = false;
					this.VisitNode(fe, visitedViaVisualTree2);
				}
			}
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000AE78 File Offset: 0x00009078
		private void VisitNode(FrameworkElement fe, bool visitedViaVisualTree)
		{
			if (this._recursionDepth > ContextLayoutManager.s_LayoutRecursionLimit)
			{
				throw new InvalidOperationException(SR.Get("LogicalTreeLoop"));
			}
			int num = this._nodes.IndexOf(fe);
			if (num != -1)
			{
				this._nodes.RemoveAt(num);
				return;
			}
			DependencyObject parent = VisualTreeHelper.GetParent(fe);
			DependencyObject parent2 = fe.Parent;
			if (parent != null && parent2 != null && parent != parent2)
			{
				this._nodes.Add(fe);
			}
			this._VisitNode(fe, visitedViaVisualTree);
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000AEF0 File Offset: 0x000090F0
		private void VisitNode(DependencyObject d, bool visitedViaVisualTree)
		{
			if (this._recursionDepth > ContextLayoutManager.s_LayoutRecursionLimit)
			{
				throw new InvalidOperationException(SR.Get("LogicalTreeLoop"));
			}
			if (FrameworkElement.DType.IsInstanceOfType(d))
			{
				this.VisitNode(d as FrameworkElement, visitedViaVisualTree);
				return;
			}
			if (FrameworkContentElement.DType.IsInstanceOfType(d))
			{
				this._VisitNode(d, visitedViaVisualTree);
				return;
			}
			this.IterateChildren(d);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000AF54 File Offset: 0x00009154
		protected virtual void _VisitNode(DependencyObject d, bool visitedViaVisualTree)
		{
			bool flag = this._callback(d, this._data, visitedViaVisualTree);
			if (flag)
			{
				this.IterateChildren(d);
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x0000AF7F File Offset: 0x0000917F
		protected T Data
		{
			get
			{
				return this._data;
			}
		}

		// Token: 0x0400060C RID: 1548
		private VisitedCallback<T> _callback;

		// Token: 0x0400060D RID: 1549
		private T _data;
	}
}
