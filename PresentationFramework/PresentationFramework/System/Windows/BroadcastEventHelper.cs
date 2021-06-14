using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.PresentationFramework;

namespace System.Windows
{
	// Token: 0x020000A0 RID: 160
	internal static class BroadcastEventHelper
	{
		// Token: 0x06000303 RID: 771 RVA: 0x0000852C File Offset: 0x0000672C
		internal static void AddLoadedCallback(DependencyObject d, DependencyObject logicalParent)
		{
			DispatcherOperationCallback dispatcherOperationCallback = new DispatcherOperationCallback(BroadcastEventHelper.BroadcastLoadedEvent);
			LoadedOrUnloadedOperation loadedOrUnloadedOperation = MediaContext.From(d.Dispatcher).AddLoadedOrUnloadedCallback(dispatcherOperationCallback, d);
			DispatcherOperation dispatcherOperation = d.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, dispatcherOperationCallback, d);
			d.SetValue(FrameworkElement.LoadedPendingPropertyKey, new object[]
			{
				loadedOrUnloadedOperation,
				dispatcherOperation,
				logicalParent
			});
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00008588 File Offset: 0x00006788
		internal static void RemoveLoadedCallback(DependencyObject d, object[] loadedPending)
		{
			if (loadedPending != null)
			{
				d.ClearValue(FrameworkElement.LoadedPendingPropertyKey);
				DispatcherOperation dispatcherOperation = (DispatcherOperation)loadedPending[1];
				if (dispatcherOperation.Status == DispatcherOperationStatus.Pending)
				{
					dispatcherOperation.Abort();
				}
				MediaContext.From(d.Dispatcher).RemoveLoadedOrUnloadedCallback((LoadedOrUnloadedOperation)loadedPending[0]);
			}
		}

		// Token: 0x06000305 RID: 773 RVA: 0x000085D4 File Offset: 0x000067D4
		internal static void AddUnloadedCallback(DependencyObject d, DependencyObject logicalParent)
		{
			DispatcherOperationCallback dispatcherOperationCallback = new DispatcherOperationCallback(BroadcastEventHelper.BroadcastUnloadedEvent);
			LoadedOrUnloadedOperation loadedOrUnloadedOperation = MediaContext.From(d.Dispatcher).AddLoadedOrUnloadedCallback(dispatcherOperationCallback, d);
			DispatcherOperation dispatcherOperation = d.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, dispatcherOperationCallback, d);
			d.SetValue(FrameworkElement.UnloadedPendingPropertyKey, new object[]
			{
				loadedOrUnloadedOperation,
				dispatcherOperation,
				logicalParent
			});
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00008630 File Offset: 0x00006830
		internal static void RemoveUnloadedCallback(DependencyObject d, object[] unloadedPending)
		{
			if (unloadedPending != null)
			{
				d.ClearValue(FrameworkElement.UnloadedPendingPropertyKey);
				DispatcherOperation dispatcherOperation = (DispatcherOperation)unloadedPending[1];
				if (dispatcherOperation.Status == DispatcherOperationStatus.Pending)
				{
					dispatcherOperation.Abort();
				}
				MediaContext.From(d.Dispatcher).RemoveLoadedOrUnloadedCallback((LoadedOrUnloadedOperation)unloadedPending[0]);
			}
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000867B File Offset: 0x0000687B
		internal static void BroadcastLoadedOrUnloadedEvent(DependencyObject d, DependencyObject oldParent, DependencyObject newParent)
		{
			if (oldParent == null && newParent != null)
			{
				if (BroadcastEventHelper.IsLoadedHelper(newParent))
				{
					BroadcastEventHelper.FireLoadedOnDescendentsHelper(d);
					return;
				}
			}
			else if (oldParent != null && newParent == null && BroadcastEventHelper.IsLoadedHelper(oldParent))
			{
				BroadcastEventHelper.FireUnloadedOnDescendentsHelper(d);
			}
		}

		// Token: 0x06000308 RID: 776 RVA: 0x000086A8 File Offset: 0x000068A8
		internal static object BroadcastLoadedEvent(object root)
		{
			DependencyObject dependencyObject = (DependencyObject)root;
			object[] loadedPending = (object[])dependencyObject.GetValue(FrameworkElement.LoadedPendingProperty);
			bool isLoaded = BroadcastEventHelper.IsLoadedHelper(dependencyObject);
			BroadcastEventHelper.RemoveLoadedCallback(dependencyObject, loadedPending);
			BroadcastEventHelper.BroadcastLoadedSynchronously(dependencyObject, isLoaded);
			return null;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x000086E3 File Offset: 0x000068E3
		internal static void BroadcastLoadedSynchronously(DependencyObject rootDO, bool isLoaded)
		{
			if (!isLoaded)
			{
				BroadcastEventHelper.BroadcastEvent(rootDO, FrameworkElement.LoadedEvent);
			}
		}

		// Token: 0x0600030A RID: 778 RVA: 0x000086F4 File Offset: 0x000068F4
		internal static object BroadcastUnloadedEvent(object root)
		{
			DependencyObject dependencyObject = (DependencyObject)root;
			object[] unloadedPending = (object[])dependencyObject.GetValue(FrameworkElement.UnloadedPendingProperty);
			bool isLoaded = BroadcastEventHelper.IsLoadedHelper(dependencyObject);
			BroadcastEventHelper.RemoveUnloadedCallback(dependencyObject, unloadedPending);
			BroadcastEventHelper.BroadcastUnloadedSynchronously(dependencyObject, isLoaded);
			return null;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000872F File Offset: 0x0000692F
		internal static void BroadcastUnloadedSynchronously(DependencyObject rootDO, bool isLoaded)
		{
			if (isLoaded)
			{
				BroadcastEventHelper.BroadcastEvent(rootDO, FrameworkElement.UnloadedEvent);
			}
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00008740 File Offset: 0x00006940
		private static void BroadcastEvent(DependencyObject root, RoutedEvent routedEvent)
		{
			List<DependencyObject> list = new List<DependencyObject>();
			DescendentsWalker<BroadcastEventHelper.BroadcastEventData> descendentsWalker = new DescendentsWalker<BroadcastEventHelper.BroadcastEventData>(TreeWalkPriority.VisualTree, BroadcastEventHelper.BroadcastDelegate, new BroadcastEventHelper.BroadcastEventData(root, routedEvent, list));
			descendentsWalker.StartWalk(root);
			for (int i = 0; i < list.Count; i++)
			{
				DependencyObject dependencyObject = list[i];
				RoutedEventArgs args = new RoutedEventArgs(routedEvent, dependencyObject);
				FrameworkObject frameworkObject = new FrameworkObject(dependencyObject, true);
				if (routedEvent == FrameworkElement.LoadedEvent)
				{
					frameworkObject.OnLoaded(args);
				}
				else
				{
					frameworkObject.OnUnloaded(args);
				}
			}
		}

		// Token: 0x0600030D RID: 781 RVA: 0x000087B8 File Offset: 0x000069B8
		private static bool OnBroadcastCallback(DependencyObject d, BroadcastEventHelper.BroadcastEventData data, bool visitedViaVisualTree)
		{
			DependencyObject root = data.Root;
			RoutedEvent routedEvent = data.RoutedEvent;
			List<DependencyObject> eventRoute = data.EventRoute;
			if (FrameworkElement.DType.IsInstanceOfType(d))
			{
				FrameworkElement frameworkElement = (FrameworkElement)d;
				if (frameworkElement != root && routedEvent == FrameworkElement.LoadedEvent && frameworkElement.UnloadedPending != null)
				{
					frameworkElement.FireLoadedOnDescendentsInternal();
				}
				else if (frameworkElement != root && routedEvent == FrameworkElement.UnloadedEvent && frameworkElement.LoadedPending != null)
				{
					BroadcastEventHelper.RemoveLoadedCallback(frameworkElement, frameworkElement.LoadedPending);
				}
				else
				{
					if (frameworkElement != root)
					{
						if (routedEvent == FrameworkElement.LoadedEvent && frameworkElement.LoadedPending != null)
						{
							BroadcastEventHelper.RemoveLoadedCallback(frameworkElement, frameworkElement.LoadedPending);
						}
						else if (routedEvent == FrameworkElement.UnloadedEvent && frameworkElement.UnloadedPending != null)
						{
							BroadcastEventHelper.RemoveUnloadedCallback(frameworkElement, frameworkElement.UnloadedPending);
						}
					}
					if (frameworkElement.SubtreeHasLoadedChangeHandler)
					{
						frameworkElement.IsLoadedCache = (routedEvent == FrameworkElement.LoadedEvent);
						eventRoute.Add(frameworkElement);
						return true;
					}
				}
			}
			else
			{
				FrameworkContentElement frameworkContentElement = (FrameworkContentElement)d;
				if (frameworkContentElement != root && routedEvent == FrameworkElement.LoadedEvent && frameworkContentElement.UnloadedPending != null)
				{
					frameworkContentElement.FireLoadedOnDescendentsInternal();
				}
				else if (frameworkContentElement != root && routedEvent == FrameworkElement.UnloadedEvent && frameworkContentElement.LoadedPending != null)
				{
					BroadcastEventHelper.RemoveLoadedCallback(frameworkContentElement, frameworkContentElement.LoadedPending);
				}
				else
				{
					if (frameworkContentElement != root)
					{
						if (routedEvent == FrameworkElement.LoadedEvent && frameworkContentElement.LoadedPending != null)
						{
							BroadcastEventHelper.RemoveLoadedCallback(frameworkContentElement, frameworkContentElement.LoadedPending);
						}
						else if (routedEvent == FrameworkElement.UnloadedEvent && frameworkContentElement.UnloadedPending != null)
						{
							BroadcastEventHelper.RemoveUnloadedCallback(frameworkContentElement, frameworkContentElement.UnloadedPending);
						}
					}
					if (frameworkContentElement.SubtreeHasLoadedChangeHandler)
					{
						frameworkContentElement.IsLoadedCache = (routedEvent == FrameworkElement.LoadedEvent);
						eventRoute.Add(frameworkContentElement);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000894D File Offset: 0x00006B4D
		private static bool SubtreeHasLoadedChangeHandlerHelper(DependencyObject d)
		{
			if (FrameworkElement.DType.IsInstanceOfType(d))
			{
				return ((FrameworkElement)d).SubtreeHasLoadedChangeHandler;
			}
			return FrameworkContentElement.DType.IsInstanceOfType(d) && ((FrameworkContentElement)d).SubtreeHasLoadedChangeHandler;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00008982 File Offset: 0x00006B82
		private static void FireLoadedOnDescendentsHelper(DependencyObject d)
		{
			if (FrameworkElement.DType.IsInstanceOfType(d))
			{
				((FrameworkElement)d).FireLoadedOnDescendentsInternal();
				return;
			}
			((FrameworkContentElement)d).FireLoadedOnDescendentsInternal();
		}

		// Token: 0x06000310 RID: 784 RVA: 0x000089A8 File Offset: 0x00006BA8
		private static void FireUnloadedOnDescendentsHelper(DependencyObject d)
		{
			if (FrameworkElement.DType.IsInstanceOfType(d))
			{
				((FrameworkElement)d).FireUnloadedOnDescendentsInternal();
				return;
			}
			((FrameworkContentElement)d).FireUnloadedOnDescendentsInternal();
		}

		// Token: 0x06000311 RID: 785 RVA: 0x000089D0 File Offset: 0x00006BD0
		private static bool IsLoadedHelper(DependencyObject d)
		{
			FrameworkObject frameworkObject = new FrameworkObject(d);
			return frameworkObject.IsLoaded;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x000089EC File Offset: 0x00006BEC
		internal static bool IsParentLoaded(DependencyObject d)
		{
			FrameworkObject frameworkObject = new FrameworkObject(d);
			DependencyObject effectiveParent = frameworkObject.EffectiveParent;
			if (effectiveParent != null)
			{
				return BroadcastEventHelper.IsLoadedHelper(effectiveParent);
			}
			Visual visual;
			if ((visual = (d as Visual)) != null)
			{
				return SafeSecurityHelper.IsConnectedToPresentationSource(visual);
			}
			Visual3D reference;
			if ((reference = (d as Visual3D)) != null)
			{
				visual = VisualTreeHelper.GetContainingVisual2D(reference);
				return visual != null && SafeSecurityHelper.IsConnectedToPresentationSource(visual);
			}
			return false;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00008A44 File Offset: 0x00006C44
		internal static FrameworkElementFactory GetFEFTreeRoot(DependencyObject templatedParent)
		{
			FrameworkObject frameworkObject = new FrameworkObject(templatedParent, true);
			FrameworkTemplate templateInternal = frameworkObject.FE.TemplateInternal;
			return templateInternal.VisualTree;
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00008A70 File Offset: 0x00006C70
		internal static void AddOrRemoveHasLoadedChangeHandlerFlag(DependencyObject d, DependencyObject oldParent, DependencyObject newParent)
		{
			bool flag = BroadcastEventHelper.SubtreeHasLoadedChangeHandlerHelper(d);
			if (flag)
			{
				if (oldParent == null && newParent != null)
				{
					BroadcastEventHelper.AddHasLoadedChangeHandlerFlagInAncestry(newParent);
					return;
				}
				if (oldParent != null && newParent == null)
				{
					BroadcastEventHelper.RemoveHasLoadedChangeHandlerFlagInAncestry(oldParent);
				}
			}
		}

		// Token: 0x06000315 RID: 789 RVA: 0x00008AA0 File Offset: 0x00006CA0
		internal static void AddHasLoadedChangeHandlerFlagInAncestry(DependencyObject d)
		{
			BroadcastEventHelper.UpdateHasLoadedChangeHandlerFlagInAncestry(d, true);
		}

		// Token: 0x06000316 RID: 790 RVA: 0x00008AA9 File Offset: 0x00006CA9
		internal static void RemoveHasLoadedChangeHandlerFlagInAncestry(DependencyObject d)
		{
			BroadcastEventHelper.UpdateHasLoadedChangeHandlerFlagInAncestry(d, false);
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00008AB4 File Offset: 0x00006CB4
		private static bool AreThereLoadedChangeHandlersInSubtree(ref FrameworkObject fo)
		{
			if (!fo.IsValid)
			{
				return false;
			}
			if (fo.ThisHasLoadedChangeEventHandler)
			{
				return true;
			}
			if (fo.IsFE)
			{
				Visual fe = fo.FE;
				int childrenCount = VisualTreeHelper.GetChildrenCount(fe);
				for (int i = 0; i < childrenCount; i++)
				{
					FrameworkElement frameworkElement = VisualTreeHelper.GetChild(fe, i) as FrameworkElement;
					if (frameworkElement != null && frameworkElement.SubtreeHasLoadedChangeHandler)
					{
						return true;
					}
				}
			}
			foreach (object obj in LogicalTreeHelper.GetChildren(fo.DO))
			{
				DependencyObject dependencyObject = obj as DependencyObject;
				if (dependencyObject != null && BroadcastEventHelper.SubtreeHasLoadedChangeHandlerHelper(dependencyObject))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00008B80 File Offset: 0x00006D80
		private static void UpdateHasLoadedChangeHandlerFlagInAncestry(DependencyObject d, bool addHandler)
		{
			FrameworkObject frameworkObject = new FrameworkObject(d);
			if (!addHandler && BroadcastEventHelper.AreThereLoadedChangeHandlersInSubtree(ref frameworkObject))
			{
				return;
			}
			if (frameworkObject.IsValid)
			{
				if (frameworkObject.SubtreeHasLoadedChangeHandler != addHandler)
				{
					DependencyObject dependencyObject = frameworkObject.IsFE ? VisualTreeHelper.GetParent(frameworkObject.FE) : null;
					DependencyObject parent = frameworkObject.Parent;
					DependencyObject dependencyObject2 = null;
					frameworkObject.SubtreeHasLoadedChangeHandler = addHandler;
					if (dependencyObject != null)
					{
						BroadcastEventHelper.UpdateHasLoadedChangeHandlerFlagInAncestry(dependencyObject, addHandler);
						dependencyObject2 = dependencyObject;
					}
					if (parent != null && parent != dependencyObject)
					{
						BroadcastEventHelper.UpdateHasLoadedChangeHandlerFlagInAncestry(parent, addHandler);
						if (frameworkObject.IsFCE)
						{
							dependencyObject2 = parent;
						}
					}
					if (parent == null && dependencyObject == null)
					{
						dependencyObject2 = Helper.FindMentor(frameworkObject.DO.InheritanceContext);
						if (dependencyObject2 != null)
						{
							frameworkObject.ChangeSubtreeHasLoadedChangedHandler(dependencyObject2);
						}
					}
					if (addHandler)
					{
						if (frameworkObject.IsFE)
						{
							BroadcastEventHelper.UpdateIsLoadedCache(frameworkObject.FE, dependencyObject2);
							return;
						}
						BroadcastEventHelper.UpdateIsLoadedCache(frameworkObject.FCE, dependencyObject2);
						return;
					}
				}
			}
			else
			{
				DependencyObject dependencyObject3 = null;
				Visual reference;
				ContentElement reference2;
				Visual3D reference3;
				if ((reference = (d as Visual)) != null)
				{
					dependencyObject3 = VisualTreeHelper.GetParent(reference);
				}
				else if ((reference2 = (d as ContentElement)) != null)
				{
					dependencyObject3 = ContentOperations.GetParent(reference2);
				}
				else if ((reference3 = (d as Visual3D)) != null)
				{
					dependencyObject3 = VisualTreeHelper.GetParent(reference3);
				}
				if (dependencyObject3 != null)
				{
					BroadcastEventHelper.UpdateHasLoadedChangeHandlerFlagInAncestry(dependencyObject3, addHandler);
				}
			}
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00008CA5 File Offset: 0x00006EA5
		private static void UpdateIsLoadedCache(FrameworkElement fe, DependencyObject parent)
		{
			if (fe.GetValue(FrameworkElement.LoadedPendingProperty) != null)
			{
				fe.IsLoadedCache = false;
				return;
			}
			if (parent != null)
			{
				fe.IsLoadedCache = BroadcastEventHelper.IsLoadedHelper(parent);
				return;
			}
			if (SafeSecurityHelper.IsConnectedToPresentationSource(fe))
			{
				fe.IsLoadedCache = true;
				return;
			}
			fe.IsLoadedCache = false;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00008CE3 File Offset: 0x00006EE3
		private static void UpdateIsLoadedCache(FrameworkContentElement fce, DependencyObject parent)
		{
			if (fce.GetValue(FrameworkElement.LoadedPendingProperty) == null)
			{
				fce.IsLoadedCache = BroadcastEventHelper.IsLoadedHelper(parent);
				return;
			}
			fce.IsLoadedCache = false;
		}

		// Token: 0x040005C8 RID: 1480
		private static VisitedCallback<BroadcastEventHelper.BroadcastEventData> BroadcastDelegate = new VisitedCallback<BroadcastEventHelper.BroadcastEventData>(BroadcastEventHelper.OnBroadcastCallback);

		// Token: 0x02000812 RID: 2066
		private struct BroadcastEventData
		{
			// Token: 0x06007E39 RID: 32313 RVA: 0x0023566D File Offset: 0x0023386D
			internal BroadcastEventData(DependencyObject root, RoutedEvent routedEvent, List<DependencyObject> eventRoute)
			{
				this.Root = root;
				this.RoutedEvent = routedEvent;
				this.EventRoute = eventRoute;
			}

			// Token: 0x04003BA6 RID: 15270
			internal DependencyObject Root;

			// Token: 0x04003BA7 RID: 15271
			internal RoutedEvent RoutedEvent;

			// Token: 0x04003BA8 RID: 15272
			internal List<DependencyObject> EventRoute;
		}
	}
}
