using System;
using System.Windows;

namespace MS.Internal
{
	// Token: 0x020005ED RID: 1517
	internal class PrePostDescendentsWalker<T> : DescendentsWalker<T>
	{
		// Token: 0x06006533 RID: 25907 RVA: 0x001C6934 File Offset: 0x001C4B34
		public PrePostDescendentsWalker(TreeWalkPriority priority, VisitedCallback<T> preCallback, VisitedCallback<T> postCallback, T data) : base(priority, preCallback, data)
		{
			this._postCallback = postCallback;
		}

		// Token: 0x06006534 RID: 25908 RVA: 0x001C6948 File Offset: 0x001C4B48
		public override void StartWalk(DependencyObject startNode, bool skipStartNode)
		{
			try
			{
				base.StartWalk(startNode, skipStartNode);
			}
			finally
			{
				if (!skipStartNode && this._postCallback != null && (FrameworkElement.DType.IsInstanceOfType(startNode) || FrameworkContentElement.DType.IsInstanceOfType(startNode)))
				{
					this._postCallback(startNode, base.Data, this._priority == TreeWalkPriority.VisualTree);
				}
			}
		}

		// Token: 0x06006535 RID: 25909 RVA: 0x001C69B4 File Offset: 0x001C4BB4
		protected override void _VisitNode(DependencyObject d, bool visitedViaVisualTree)
		{
			try
			{
				base._VisitNode(d, visitedViaVisualTree);
			}
			finally
			{
				if (this._postCallback != null)
				{
					this._postCallback(d, base.Data, visitedViaVisualTree);
				}
			}
		}

		// Token: 0x040032BD RID: 12989
		private VisitedCallback<T> _postCallback;
	}
}
