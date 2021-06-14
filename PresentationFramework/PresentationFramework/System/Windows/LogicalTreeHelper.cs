using System;
using System.Collections;
using MS.Internal.Controls;

namespace System.Windows
{
	/// <summary>Provides static helper methods for querying objects in the logical tree.</summary>
	// Token: 0x020000D6 RID: 214
	public static class LogicalTreeHelper
	{
		/// <summary>Attempts to find and return an object that has the specified name. The search starts from the specified object and continues into subnodes of the logical tree. </summary>
		/// <param name="logicalTreeNode">The object to start searching from. This object must be either a <see cref="T:System.Windows.FrameworkElement" /> or a <see cref="T:System.Windows.FrameworkContentElement" />.</param>
		/// <param name="elementName">The name of the object to find.</param>
		/// <returns>The object with the matching name, if one is found; returns <see langword="null" /> if no matching name was found in the logical tree.</returns>
		// Token: 0x06000763 RID: 1891 RVA: 0x00017104 File Offset: 0x00015304
		public static DependencyObject FindLogicalNode(DependencyObject logicalTreeNode, string elementName)
		{
			if (logicalTreeNode == null)
			{
				throw new ArgumentNullException("logicalTreeNode");
			}
			if (elementName == null)
			{
				throw new ArgumentNullException("elementName");
			}
			if (elementName == string.Empty)
			{
				throw new ArgumentException(SR.Get("StringEmpty"), "elementName");
			}
			DependencyObject dependencyObject = null;
			IFrameworkInputElement frameworkInputElement = logicalTreeNode as IFrameworkInputElement;
			if (frameworkInputElement != null && frameworkInputElement.Name == elementName)
			{
				dependencyObject = logicalTreeNode;
			}
			if (dependencyObject == null)
			{
				IEnumerator logicalChildren = LogicalTreeHelper.GetLogicalChildren(logicalTreeNode);
				if (logicalChildren != null)
				{
					logicalChildren.Reset();
					while (dependencyObject == null && logicalChildren.MoveNext())
					{
						DependencyObject dependencyObject2 = logicalChildren.Current as DependencyObject;
						if (dependencyObject2 != null)
						{
							dependencyObject = LogicalTreeHelper.FindLogicalNode(dependencyObject2, elementName);
						}
					}
				}
			}
			return dependencyObject;
		}

		/// <summary>Returns the parent object of the specified object by processing the logical tree.</summary>
		/// <param name="current">The object to find the parent object for. This is expected to be either a <see cref="T:System.Windows.FrameworkElement" /> or a <see cref="T:System.Windows.FrameworkContentElement" />.</param>
		/// <returns>The requested parent object.</returns>
		// Token: 0x06000764 RID: 1892 RVA: 0x000171A8 File Offset: 0x000153A8
		public static DependencyObject GetParent(DependencyObject current)
		{
			if (current == null)
			{
				throw new ArgumentNullException("current");
			}
			FrameworkElement frameworkElement = current as FrameworkElement;
			if (frameworkElement != null)
			{
				return frameworkElement.Parent;
			}
			FrameworkContentElement frameworkContentElement = current as FrameworkContentElement;
			if (frameworkContentElement != null)
			{
				return frameworkContentElement.Parent;
			}
			return null;
		}

		/// <summary>Returns the collection of immediate child objects of the specified object, by processing the logical tree.</summary>
		/// <param name="current">The object from which to start processing the logical tree. This is expected to be either a <see cref="T:System.Windows.FrameworkElement" /> or <see cref="T:System.Windows.FrameworkContentElement" />.</param>
		/// <returns>The enumerable collection of immediate child objects from the logical tree of the specified object.</returns>
		// Token: 0x06000765 RID: 1893 RVA: 0x000171E8 File Offset: 0x000153E8
		public static IEnumerable GetChildren(DependencyObject current)
		{
			if (current == null)
			{
				throw new ArgumentNullException("current");
			}
			FrameworkElement frameworkElement = current as FrameworkElement;
			if (frameworkElement != null)
			{
				return new LogicalTreeHelper.EnumeratorWrapper(frameworkElement.LogicalChildren);
			}
			FrameworkContentElement frameworkContentElement = current as FrameworkContentElement;
			if (frameworkContentElement != null)
			{
				return new LogicalTreeHelper.EnumeratorWrapper(frameworkContentElement.LogicalChildren);
			}
			return LogicalTreeHelper.EnumeratorWrapper.Empty;
		}

		/// <summary>Returns the collection of immediate child objects of the specified <see cref="T:System.Windows.FrameworkElement" /> by processing the logical tree. </summary>
		/// <param name="current">The object from which to start processing the logical tree.</param>
		/// <returns>The enumerable collection of immediate child objects starting from <paramref name="current" /> in the logical tree.</returns>
		// Token: 0x06000766 RID: 1894 RVA: 0x00017234 File Offset: 0x00015434
		public static IEnumerable GetChildren(FrameworkElement current)
		{
			if (current == null)
			{
				throw new ArgumentNullException("current");
			}
			return new LogicalTreeHelper.EnumeratorWrapper(current.LogicalChildren);
		}

		/// <summary>Returns the collection of immediate child objects of the specified <see cref="T:System.Windows.FrameworkContentElement" /> by processing the logical tree. </summary>
		/// <param name="current">The object from which to start processing the logical tree.</param>
		/// <returns>The enumerable collection of immediate child objects starting from <paramref name="current" /> in the logical tree.</returns>
		// Token: 0x06000767 RID: 1895 RVA: 0x0001724F File Offset: 0x0001544F
		public static IEnumerable GetChildren(FrameworkContentElement current)
		{
			if (current == null)
			{
				throw new ArgumentNullException("current");
			}
			return new LogicalTreeHelper.EnumeratorWrapper(current.LogicalChildren);
		}

		/// <summary>Attempts to bring the requested UI element into view and raises the <see cref="E:System.Windows.FrameworkElement.RequestBringIntoView" /> event on the target in order to report the results.</summary>
		/// <param name="current">The UI element to bring into view.</param>
		// Token: 0x06000768 RID: 1896 RVA: 0x0001726C File Offset: 0x0001546C
		public static void BringIntoView(DependencyObject current)
		{
			if (current == null)
			{
				throw new ArgumentNullException("current");
			}
			FrameworkElement frameworkElement = current as FrameworkElement;
			if (frameworkElement != null)
			{
				frameworkElement.BringIntoView();
			}
			FrameworkContentElement frameworkContentElement = current as FrameworkContentElement;
			if (frameworkContentElement != null)
			{
				frameworkContentElement.BringIntoView();
			}
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x000172A8 File Offset: 0x000154A8
		internal static void AddLogicalChild(DependencyObject parent, object child)
		{
			if (child != null && parent != null)
			{
				FrameworkElement frameworkElement = parent as FrameworkElement;
				if (frameworkElement != null)
				{
					frameworkElement.AddLogicalChild(child);
					return;
				}
				FrameworkContentElement frameworkContentElement = parent as FrameworkContentElement;
				if (frameworkContentElement != null)
				{
					frameworkContentElement.AddLogicalChild(child);
				}
			}
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x000172DE File Offset: 0x000154DE
		internal static void AddLogicalChild(FrameworkElement parentFE, FrameworkContentElement parentFCE, object child)
		{
			if (child != null)
			{
				if (parentFE != null)
				{
					parentFE.AddLogicalChild(child);
					return;
				}
				if (parentFCE != null)
				{
					parentFCE.AddLogicalChild(child);
				}
			}
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x000172F8 File Offset: 0x000154F8
		internal static void RemoveLogicalChild(DependencyObject parent, object child)
		{
			if (child != null && parent != null)
			{
				FrameworkElement frameworkElement = parent as FrameworkElement;
				if (frameworkElement != null)
				{
					frameworkElement.RemoveLogicalChild(child);
					return;
				}
				FrameworkContentElement frameworkContentElement = parent as FrameworkContentElement;
				if (frameworkContentElement != null)
				{
					frameworkContentElement.RemoveLogicalChild(child);
				}
			}
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0001732E File Offset: 0x0001552E
		internal static void RemoveLogicalChild(FrameworkElement parentFE, FrameworkContentElement parentFCE, object child)
		{
			if (child != null)
			{
				if (parentFE != null)
				{
					parentFE.RemoveLogicalChild(child);
					return;
				}
				parentFCE.RemoveLogicalChild(child);
			}
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x00017348 File Offset: 0x00015548
		internal static IEnumerator GetLogicalChildren(DependencyObject current)
		{
			FrameworkElement frameworkElement = current as FrameworkElement;
			if (frameworkElement != null)
			{
				return frameworkElement.LogicalChildren;
			}
			FrameworkContentElement frameworkContentElement = current as FrameworkContentElement;
			if (frameworkContentElement != null)
			{
				return frameworkContentElement.LogicalChildren;
			}
			return EmptyEnumerator.Instance;
		}

		// Token: 0x0200081E RID: 2078
		private class EnumeratorWrapper : IEnumerable
		{
			// Token: 0x06007E53 RID: 32339 RVA: 0x0023596C File Offset: 0x00233B6C
			public EnumeratorWrapper(IEnumerator enumerator)
			{
				if (enumerator != null)
				{
					this._enumerator = enumerator;
					return;
				}
				this._enumerator = EmptyEnumerator.Instance;
			}

			// Token: 0x06007E54 RID: 32340 RVA: 0x0023598A File Offset: 0x00233B8A
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this._enumerator;
			}

			// Token: 0x17001D50 RID: 7504
			// (get) Token: 0x06007E55 RID: 32341 RVA: 0x00235992 File Offset: 0x00233B92
			internal static LogicalTreeHelper.EnumeratorWrapper Empty
			{
				get
				{
					if (LogicalTreeHelper.EnumeratorWrapper._emptyInstance == null)
					{
						LogicalTreeHelper.EnumeratorWrapper._emptyInstance = new LogicalTreeHelper.EnumeratorWrapper(null);
					}
					return LogicalTreeHelper.EnumeratorWrapper._emptyInstance;
				}
			}

			// Token: 0x04003BCD RID: 15309
			private IEnumerator _enumerator;

			// Token: 0x04003BCE RID: 15310
			private static LogicalTreeHelper.EnumeratorWrapper _emptyInstance;
		}
	}
}
