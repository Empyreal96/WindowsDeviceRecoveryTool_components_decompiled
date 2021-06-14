using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using MS.Internal.Data;

namespace System.Windows.Data
{
	/// <summary>Provides static methods to manipulate bindings, including <see cref="T:System.Windows.Data.Binding" />, <see cref="T:System.Windows.Data.MultiBinding" />, and <see cref="T:System.Windows.Data.PriorityBinding" /> objects.</summary>
	// Token: 0x020001A3 RID: 419
	public static class BindingOperations
	{
		/// <summary>Gets an object that replaces the <see cref="P:System.Windows.FrameworkElement.DataContext" /> when an item container is removed from the visual tree.</summary>
		/// <returns>An object that replaces the <see cref="P:System.Windows.FrameworkElement.DataContext" /> when an item container is removed from the visual tree.</returns>
		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06001A2A RID: 6698 RVA: 0x0007D1AD File Offset: 0x0007B3AD
		public static object DisconnectedSource
		{
			get
			{
				return BindingExpressionBase.DisconnectedItem;
			}
		}

		/// <summary>Creates and associates a new instance of <see cref="T:System.Windows.Data.BindingExpressionBase" /> with the specified binding target property.</summary>
		/// <param name="target">The binding target of the binding.</param>
		/// <param name="dp">The target property of the binding.</param>
		/// <param name="binding">The <see cref="T:System.Windows.Data.BindingBase" /> object that describes the binding.</param>
		/// <returns>The instance of <see cref="T:System.Windows.Data.BindingExpressionBase" /> created for and associated with the specified property. The <see cref="T:System.Windows.Data.BindingExpressionBase" /> class is the base class of <see cref="T:System.Windows.Data.BindingExpression" />, <see cref="T:System.Windows.Data.MultiBindingExpression" />, and <see cref="T:System.Windows.Data.PriorityBindingExpression" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="target" /> parameter cannot be <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="dp" /> parameter cannot be <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="binding" /> parameter cannot be <see langword="null" />.</exception>
		// Token: 0x06001A2B RID: 6699 RVA: 0x0007D1B4 File Offset: 0x0007B3B4
		public static BindingExpressionBase SetBinding(DependencyObject target, DependencyProperty dp, BindingBase binding)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (dp == null)
			{
				throw new ArgumentNullException("dp");
			}
			if (binding == null)
			{
				throw new ArgumentNullException("binding");
			}
			BindingExpressionBase bindingExpressionBase = binding.CreateBindingExpression(target, dp);
			target.SetValue(dp, bindingExpressionBase);
			return bindingExpressionBase;
		}

		/// <summary>Retrieves the <see cref="T:System.Windows.Data.BindingBase" /> object that is set on the specified property.</summary>
		/// <param name="target">The object where <paramref name="dp" /> is.</param>
		/// <param name="dp">The binding target property from which to retrieve the <see cref="T:System.Windows.Data.BindingBase" /> object.</param>
		/// <returns>The <see cref="T:System.Windows.Data.BindingBase" /> object that is set on the given property or <see langword="null" /> if no binding object has been set.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="target" /> and <paramref name="dp" /> parameters cannot be <see langword="null" />.</exception>
		// Token: 0x06001A2C RID: 6700 RVA: 0x0007D200 File Offset: 0x0007B400
		public static BindingBase GetBindingBase(DependencyObject target, DependencyProperty dp)
		{
			BindingExpressionBase bindingExpressionBase = BindingOperations.GetBindingExpressionBase(target, dp);
			if (bindingExpressionBase == null)
			{
				return null;
			}
			return bindingExpressionBase.ParentBindingBase;
		}

		/// <summary>Retrieves the <see cref="T:System.Windows.Data.Binding" /> object that is set on the specified property.</summary>
		/// <param name="target">The object where <paramref name="dp" /> is.</param>
		/// <param name="dp">The binding target property from which to retrieve the binding.</param>
		/// <returns>The <see cref="T:System.Windows.Data.Binding" /> object set on the given property or <see langword="null" /> if no <see cref="T:System.Windows.Data.Binding" /> object has been set.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="target" /> and <paramref name="dp" /> parameters cannot be null.</exception>
		// Token: 0x06001A2D RID: 6701 RVA: 0x0007D220 File Offset: 0x0007B420
		public static Binding GetBinding(DependencyObject target, DependencyProperty dp)
		{
			return BindingOperations.GetBindingBase(target, dp) as Binding;
		}

		/// <summary>Retrieves the <see cref="T:System.Windows.Data.PriorityBinding" /> object that is set on the specified property.</summary>
		/// <param name="target">The object where <paramref name="dp" /> is.</param>
		/// <param name="dp">The binding target property from which to retrieve the binding.</param>
		/// <returns>The <see cref="T:System.Windows.Data.PriorityBinding" /> object set on the given property or <see langword="null" /> if no <see cref="T:System.Windows.Data.PriorityBinding" /> object has been set.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="target" /> and <paramref name="dp" /> parameters cannot be null.</exception>
		// Token: 0x06001A2E RID: 6702 RVA: 0x0007D22E File Offset: 0x0007B42E
		public static PriorityBinding GetPriorityBinding(DependencyObject target, DependencyProperty dp)
		{
			return BindingOperations.GetBindingBase(target, dp) as PriorityBinding;
		}

		/// <summary>Retrieves the <see cref="T:System.Windows.Data.MultiBinding" /> object that is set on the specified property.</summary>
		/// <param name="target">The object where <paramref name="dp" /> is.</param>
		/// <param name="dp">The binding target property from which to retrieve the binding.</param>
		/// <returns>The <see cref="T:System.Windows.Data.MultiBinding" /> object set on the given property or <see langword="null" /> if no <see cref="T:System.Windows.Data.MultiBinding" /> object has been set.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="target" /> and <paramref name="dp" /> parameters cannot be null.</exception>
		// Token: 0x06001A2F RID: 6703 RVA: 0x0007D23C File Offset: 0x0007B43C
		public static MultiBinding GetMultiBinding(DependencyObject target, DependencyProperty dp)
		{
			return BindingOperations.GetBindingBase(target, dp) as MultiBinding;
		}

		/// <summary>Retrieves the <see cref="T:System.Windows.Data.BindingExpressionBase" /> object that is set on the specified property.</summary>
		/// <param name="target">The object where <paramref name="dp" /> is.</param>
		/// <param name="dp">The binding target property from which to retrieve the <see cref="T:System.Windows.Data.BindingExpressionBase" /> object.</param>
		/// <returns>The <see cref="T:System.Windows.Data.BindingExpressionBase" /> object that is set on the given property or <see langword="null" /> if no binding object has been set.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="target" /> and <paramref name="dp" /> parameters cannot be null.</exception>
		// Token: 0x06001A30 RID: 6704 RVA: 0x0007D24C File Offset: 0x0007B44C
		public static BindingExpressionBase GetBindingExpressionBase(DependencyObject target, DependencyProperty dp)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (dp == null)
			{
				throw new ArgumentNullException("dp");
			}
			Expression expression = StyleHelper.GetExpression(target, dp);
			return expression as BindingExpressionBase;
		}

		/// <summary>Returns the <see cref="T:System.Windows.Data.BindingExpression" /> object associated with the specified binding target property on the specified object.</summary>
		/// <param name="target">The binding target object where <paramref name="dp" /> is.</param>
		/// <param name="dp">The binding target property from which to retrieve the <see cref="T:System.Windows.Data.BindingExpression" /> object.</param>
		/// <returns>The <see cref="T:System.Windows.Data.BindingExpression" /> object associated with the given property or <see langword="null" /> if none exists. If a <see cref="T:System.Windows.Data.PriorityBindingExpression" /> object is set on the property, the <see cref="P:System.Windows.Data.PriorityBindingExpression.ActiveBindingExpression" /> is returned.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="target" /> and <paramref name="dp" /> parameters cannot be <see langword="null" />.</exception>
		// Token: 0x06001A31 RID: 6705 RVA: 0x0007D284 File Offset: 0x0007B484
		public static BindingExpression GetBindingExpression(DependencyObject target, DependencyProperty dp)
		{
			BindingExpressionBase bindingExpressionBase = BindingOperations.GetBindingExpressionBase(target, dp);
			PriorityBindingExpression priorityBindingExpression = bindingExpressionBase as PriorityBindingExpression;
			if (priorityBindingExpression != null)
			{
				bindingExpressionBase = priorityBindingExpression.ActiveBindingExpression;
			}
			return bindingExpressionBase as BindingExpression;
		}

		/// <summary>Returns the <see cref="T:System.Windows.Data.MultiBindingExpression" /> object associated with the specified binding target property on the specified object.</summary>
		/// <param name="target">The binding target object where <paramref name="dp" /> is.</param>
		/// <param name="dp">The binding target property from which to retrieve the <see cref="T:System.Windows.Data.MultiBindingExpression" /> object.</param>
		/// <returns>The <see cref="T:System.Windows.Data.MultiBindingExpression" /> object associated with the given property or <see langword="null" /> if none exists.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="target" /> and <paramref name="dp" /> parameters cannot be <see langword="null" />.</exception>
		// Token: 0x06001A32 RID: 6706 RVA: 0x0007D2B0 File Offset: 0x0007B4B0
		public static MultiBindingExpression GetMultiBindingExpression(DependencyObject target, DependencyProperty dp)
		{
			return BindingOperations.GetBindingExpressionBase(target, dp) as MultiBindingExpression;
		}

		/// <summary>Returns the <see cref="T:System.Windows.Data.PriorityBindingExpression" /> object associated with the specified binding target property on the specified object.</summary>
		/// <param name="target">The binding target object where <paramref name="dp" /> is.</param>
		/// <param name="dp">The binding target property from which to retrieve the <see cref="T:System.Windows.Data.PriorityBindingExpression" /> object.</param>
		/// <returns>The <see cref="T:System.Windows.Data.PriorityBindingExpression" /> object associated with the given property or <see langword="null" /> if none exists.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="target" /> and <paramref name="dp" /> parameters cannot be <see langword="null" />.</exception>
		// Token: 0x06001A33 RID: 6707 RVA: 0x0007D2BE File Offset: 0x0007B4BE
		public static PriorityBindingExpression GetPriorityBindingExpression(DependencyObject target, DependencyProperty dp)
		{
			return BindingOperations.GetBindingExpressionBase(target, dp) as PriorityBindingExpression;
		}

		/// <summary>Removes the binding from a property if there is one.</summary>
		/// <param name="target">The object from which to remove the binding.</param>
		/// <param name="dp">The dependency property from which to remove the binding.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="target" /> and <paramref name="dp" /> parameters cannot be <see langword="null" />.</exception>
		// Token: 0x06001A34 RID: 6708 RVA: 0x0007D2CC File Offset: 0x0007B4CC
		public static void ClearBinding(DependencyObject target, DependencyProperty dp)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (dp == null)
			{
				throw new ArgumentNullException("dp");
			}
			if (BindingOperations.IsDataBound(target, dp))
			{
				target.ClearValue(dp);
			}
		}

		/// <summary>Removes all bindings, including bindings of type <see cref="T:System.Windows.Data.Binding" />, <see cref="T:System.Windows.Data.MultiBinding" />, and <see cref="T:System.Windows.Data.PriorityBinding" />, from the specified <see cref="T:System.Windows.DependencyObject" />.</summary>
		/// <param name="target">The object from which to remove bindings.</param>
		/// <exception cref="T:System.ArgumentNullException">If <paramref name="target" /> is <see langword="null" />.</exception>
		// Token: 0x06001A35 RID: 6709 RVA: 0x0007D2FC File Offset: 0x0007B4FC
		public static void ClearAllBindings(DependencyObject target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			LocalValueEnumerator localValueEnumerator = target.GetLocalValueEnumerator();
			ArrayList arrayList = new ArrayList(8);
			while (localValueEnumerator.MoveNext())
			{
				LocalValueEntry localValueEntry = localValueEnumerator.Current;
				if (BindingOperations.IsDataBound(target, localValueEntry.Property))
				{
					arrayList.Add(localValueEntry.Property);
				}
			}
			for (int i = 0; i < arrayList.Count; i++)
			{
				target.ClearValue((DependencyProperty)arrayList[i]);
			}
		}

		/// <summary>Returns a value that indicates whether the specified property is currently data-bound.</summary>
		/// <param name="target">The object where <paramref name="dp" /> is.</param>
		/// <param name="dp">The dependency property to check.</param>
		/// <returns>
		///     <see langword="true" /> if the specified property is data-bound; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">If <paramref name="target" /> is <see langword="null" />.</exception>
		// Token: 0x06001A36 RID: 6710 RVA: 0x0007D378 File Offset: 0x0007B578
		public static bool IsDataBound(DependencyObject target, DependencyProperty dp)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (dp == null)
			{
				throw new ArgumentNullException("dp");
			}
			object expression = StyleHelper.GetExpression(target, dp);
			return expression is BindingExpressionBase;
		}

		/// <summary>Enables a <see cref="T:System.Windows.Data.CollectionView" /> object to participate in synchronized access to a collection used on multiple threads by using a mechanism other than a simple lock.</summary>
		/// <param name="collection">The collection that needs synchronized access.</param>
		/// <param name="context">An object that is passed to the callback.</param>
		/// <param name="synchronizationCallback">The callback that is invoked whenever access to the collection is required. </param>
		// Token: 0x06001A37 RID: 6711 RVA: 0x0007D3B2 File Offset: 0x0007B5B2
		public static void EnableCollectionSynchronization(IEnumerable collection, object context, CollectionSynchronizationCallback synchronizationCallback)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (synchronizationCallback == null)
			{
				throw new ArgumentNullException("synchronizationCallback");
			}
			ViewManager.Current.RegisterCollectionSynchronizationCallback(collection, context, synchronizationCallback);
		}

		/// <summary>Enables a <see cref="T:System.Windows.Data.CollectionView" />object to participate in synchronized access to a collection used on multiple threads by a simple locking mechanism. </summary>
		/// <param name="collection">The collection that needs synchronized access.</param>
		/// <param name="lockObject">The object to lock when accessing the collection.</param>
		// Token: 0x06001A38 RID: 6712 RVA: 0x0007D3DD File Offset: 0x0007B5DD
		public static void EnableCollectionSynchronization(IEnumerable collection, object lockObject)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (lockObject == null)
			{
				throw new ArgumentNullException("lockObject");
			}
			ViewManager.Current.RegisterCollectionSynchronizationCallback(collection, lockObject, null);
		}

		/// <summary>Remove the synchronization registered for the specified collection.</summary>
		/// <param name="collection">The collection to remove synchronized access from.</param>
		// Token: 0x06001A39 RID: 6713 RVA: 0x0007D408 File Offset: 0x0007B608
		public static void DisableCollectionSynchronization(IEnumerable collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			ViewManager.Current.RegisterCollectionSynchronizationCallback(collection, null, null);
		}

		/// <summary>Provides access to a collection by using the synchronization mechanism that the application specified when it called EnableCollectionSynchronization.</summary>
		/// <param name="collection">The collection to access.</param>
		/// <param name="accessMethod">The action to perform on the collection.</param>
		/// <param name="writeAccess">
		///       <see langword="true" /> if <paramref name="accessMethod" /> will write to the collection; otherwise, <see langword="false" />.</param>
		// Token: 0x06001A3A RID: 6714 RVA: 0x0007D428 File Offset: 0x0007B628
		public static void AccessCollection(IEnumerable collection, Action accessMethod, bool writeAccess)
		{
			ViewManager viewManager = ViewManager.Current;
			if (viewManager == null)
			{
				throw new InvalidOperationException(SR.Get("AccessCollectionAfterShutDown", new object[]
				{
					collection
				}));
			}
			viewManager.AccessCollection(collection, accessMethod, writeAccess);
		}

		/// <summary>Gets all <see cref="T:System.Windows.Data.BindingExpressionBase" /> objects that have invalid values or target values have not been updated the source. </summary>
		/// <param name="root">The root <see cref="T:System.Windows.UIElement" /> to get binding groups for.  This method returns <see cref="T:System.Windows.Data.BindingExpressionBase" /> objects that are associated with this element or its descendant elements. </param>
		/// <returns>A collection of <see cref="T:System.Windows.Data.BindingExpressionBase" /> objects that are associated with the specified element and have invalid values or target values have not been updated the source. </returns>
		// Token: 0x06001A3B RID: 6715 RVA: 0x0007D464 File Offset: 0x0007B664
		public static ReadOnlyCollection<BindingExpressionBase> GetSourceUpdatingBindings(DependencyObject root)
		{
			List<BindingExpressionBase> bindingsInScope = DataBindEngine.CurrentDataBindEngine.CommitManager.GetBindingsInScope(root);
			return new ReadOnlyCollection<BindingExpressionBase>(bindingsInScope);
		}

		/// <summary>Gets all <see cref="T:System.Windows.Data.BindingGroup" /> objects that have invalid values or target values have not been updated the source. </summary>
		/// <param name="root">The root <see cref="T:System.Windows.UIElement" /> to get binding groups for.  This method returns <see cref="T:System.Windows.Data.BindingGroup" /> objects that are associated with this element or its descendant elements.</param>
		/// <returns>A collection of <see cref="T:System.Windows.Data.BindingGroup" /> objects that are associated with the specified element and have invalid values or target values have not been updated the source.</returns>
		// Token: 0x06001A3C RID: 6716 RVA: 0x0007D488 File Offset: 0x0007B688
		public static ReadOnlyCollection<BindingGroup> GetSourceUpdatingBindingGroups(DependencyObject root)
		{
			List<BindingGroup> bindingGroupsInScope = DataBindEngine.CurrentDataBindEngine.CommitManager.GetBindingGroupsInScope(root);
			return new ReadOnlyCollection<BindingGroup>(bindingGroupsInScope);
		}

		/// <summary>Occurs when the data-binding system notices a collection.</summary>
		// Token: 0x14000046 RID: 70
		// (add) Token: 0x06001A3D RID: 6717 RVA: 0x0007D4AC File Offset: 0x0007B6AC
		// (remove) Token: 0x06001A3E RID: 6718 RVA: 0x0007D4E0 File Offset: 0x0007B6E0
		public static event EventHandler<CollectionRegisteringEventArgs> CollectionRegistering;

		/// <summary>Occurs when the data-binding system notices a collection view.</summary>
		// Token: 0x14000047 RID: 71
		// (add) Token: 0x06001A3F RID: 6719 RVA: 0x0007D514 File Offset: 0x0007B714
		// (remove) Token: 0x06001A40 RID: 6720 RVA: 0x0007D548 File Offset: 0x0007B748
		public static event EventHandler<CollectionViewRegisteringEventArgs> CollectionViewRegistering;

		// Token: 0x06001A41 RID: 6721 RVA: 0x0007D57B File Offset: 0x0007B77B
		internal static bool IsValidUpdateSourceTrigger(UpdateSourceTrigger value)
		{
			return value <= UpdateSourceTrigger.Explicit;
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06001A42 RID: 6722 RVA: 0x0007D584 File Offset: 0x0007B784
		// (set) Token: 0x06001A43 RID: 6723 RVA: 0x0007D590 File Offset: 0x0007B790
		internal static bool IsCleanupEnabled
		{
			get
			{
				return DataBindEngine.CurrentDataBindEngine.CleanupEnabled;
			}
			set
			{
				DataBindEngine.CurrentDataBindEngine.CleanupEnabled = value;
			}
		}

		// Token: 0x06001A44 RID: 6724 RVA: 0x0007D59D File Offset: 0x0007B79D
		internal static bool Cleanup()
		{
			return DataBindEngine.CurrentDataBindEngine.Cleanup();
		}

		// Token: 0x06001A45 RID: 6725 RVA: 0x0007D5A9 File Offset: 0x0007B7A9
		internal static void PrintStats()
		{
			DataBindEngine.CurrentDataBindEngine.AccessorTable.PrintStats();
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06001A46 RID: 6726 RVA: 0x0007D5BA File Offset: 0x0007B7BA
		// (set) Token: 0x06001A47 RID: 6727 RVA: 0x0007D5CB File Offset: 0x0007B7CB
		internal static bool TraceAccessorTableSize
		{
			get
			{
				return DataBindEngine.CurrentDataBindEngine.AccessorTable.TraceSize;
			}
			set
			{
				DataBindEngine.CurrentDataBindEngine.AccessorTable.TraceSize = value;
			}
		}

		// Token: 0x06001A48 RID: 6728 RVA: 0x0007D5DD File Offset: 0x0007B7DD
		internal static void OnCollectionRegistering(IEnumerable collection, object parent)
		{
			if (BindingOperations.CollectionRegistering != null)
			{
				BindingOperations.CollectionRegistering(null, new CollectionRegisteringEventArgs(collection, parent));
			}
		}

		// Token: 0x06001A49 RID: 6729 RVA: 0x0007D5F8 File Offset: 0x0007B7F8
		internal static void OnCollectionViewRegistering(CollectionView view)
		{
			if (BindingOperations.CollectionViewRegistering != null)
			{
				BindingOperations.CollectionViewRegistering(null, new CollectionViewRegisteringEventArgs(view));
			}
		}

		// Token: 0x06001A4A RID: 6730 RVA: 0x0007D614 File Offset: 0x0007B814
		internal static IDisposable EnableExceptionLogging()
		{
			BindingOperations.ExceptionLogger value = new BindingOperations.ExceptionLogger();
			Interlocked.CompareExchange<BindingOperations.ExceptionLogger>(ref BindingOperations._exceptionLogger, value, null);
			return BindingOperations._exceptionLogger;
		}

		// Token: 0x06001A4B RID: 6731 RVA: 0x0007D63C File Offset: 0x0007B83C
		internal static void LogException(Exception ex)
		{
			BindingOperations.ExceptionLogger exceptionLogger = BindingOperations._exceptionLogger;
			if (exceptionLogger != null)
			{
				exceptionLogger.LogException(ex);
			}
		}

		// Token: 0x0400134B RID: 4939
		private static BindingOperations.ExceptionLogger _exceptionLogger;

		// Token: 0x02000877 RID: 2167
		internal class ExceptionLogger : IDisposable
		{
			// Token: 0x06008318 RID: 33560 RVA: 0x0024490B File Offset: 0x00242B0B
			internal void LogException(Exception ex)
			{
				this._log.Add(ex);
			}

			// Token: 0x06008319 RID: 33561 RVA: 0x00244919 File Offset: 0x00242B19
			void IDisposable.Dispose()
			{
				Interlocked.CompareExchange<BindingOperations.ExceptionLogger>(ref BindingOperations._exceptionLogger, null, this);
			}

			// Token: 0x17001DB2 RID: 7602
			// (get) Token: 0x0600831A RID: 33562 RVA: 0x00244928 File Offset: 0x00242B28
			internal List<Exception> Log
			{
				get
				{
					return this._log;
				}
			}

			// Token: 0x04004137 RID: 16695
			private List<Exception> _log = new List<Exception>();
		}
	}
}
