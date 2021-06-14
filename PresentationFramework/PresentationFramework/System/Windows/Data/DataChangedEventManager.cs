using System;

namespace System.Windows.Data
{
	/// <summary>Provides a <see cref="T:System.Windows.WeakEventManager" /> implementation so that you can use the "weak event listener" pattern to attach listeners for the <see cref="E:System.Windows.Data.DataSourceProvider.DataChanged" /> event.</summary>
	// Token: 0x020001AC RID: 428
	public class DataChangedEventManager : WeakEventManager
	{
		// Token: 0x06001B4F RID: 6991 RVA: 0x0001737C File Offset: 0x0001557C
		private DataChangedEventManager()
		{
		}

		/// <summary>Adds the specified listener to the <see cref="E:System.Windows.Data.DataSourceProvider.DataChanged" /> event of the specified source.</summary>
		/// <param name="source">The object with the event.</param>
		/// <param name="listener">The object to add as a listener.</param>
		// Token: 0x06001B50 RID: 6992 RVA: 0x0008088C File Offset: 0x0007EA8C
		public static void AddListener(DataSourceProvider source, IWeakEventListener listener)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			DataChangedEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		/// <summary>Removes the specified listener from the <see cref="E:System.Windows.Data.DataSourceProvider.DataChanged" /> event of the specified source.</summary>
		/// <param name="source">The object with the event.</param>
		/// <param name="listener">The listener to remove.</param>
		// Token: 0x06001B51 RID: 6993 RVA: 0x000808B6 File Offset: 0x0007EAB6
		public static void RemoveListener(DataSourceProvider source, IWeakEventListener listener)
		{
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			DataChangedEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		/// <summary>Adds the specified event handler, which is called when specified source raises the <see cref="E:System.ComponentModel.ICollectionView.CurrentChanging" /> event.</summary>
		/// <param name="source">The source object that the raises the <see cref="E:System.Windows.Data.DataSourceProvider.DataChanged" /> event.</param>
		/// <param name="handler">The delegate that handles the <see cref="E:System.Windows.Data.DataSourceProvider.DataChanged" /> event.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="handler" /> is <see langword="null" />.</exception>
		// Token: 0x06001B52 RID: 6994 RVA: 0x000808D2 File Offset: 0x0007EAD2
		public static void AddHandler(DataSourceProvider source, EventHandler<EventArgs> handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			DataChangedEventManager.CurrentManager.ProtectedAddHandler(source, handler);
		}

		/// <summary>Removes the specified event handler from the specified source.</summary>
		/// <param name="source">The source object that the raises the <see cref="E:System.Windows.Data.DataSourceProvider.DataChanged" /> event.</param>
		/// <param name="handler">The delegate that handles the <see cref="E:System.Windows.Data.DataSourceProvider.DataChanged" /> event.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="handler" /> is <see langword="null" />.</exception>
		// Token: 0x06001B53 RID: 6995 RVA: 0x000808EE File Offset: 0x0007EAEE
		public static void RemoveHandler(DataSourceProvider source, EventHandler<EventArgs> handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			DataChangedEventManager.CurrentManager.ProtectedRemoveHandler(source, handler);
		}

		/// <summary>Returns a new object to contain listeners to the <see cref="E:System.Windows.Data.DataSourceProvider.DataChanged" /> event.</summary>
		/// <returns>A new object to contain listeners to the <see cref="E:System.Windows.Data.DataSourceProvider.DataChanged" /> event.</returns>
		// Token: 0x06001B54 RID: 6996 RVA: 0x0008090A File Offset: 0x0007EB0A
		protected override WeakEventManager.ListenerList NewListenerList()
		{
			return new WeakEventManager.ListenerList<EventArgs>();
		}

		/// <summary>Begins listening for the <see cref="E:System.Windows.Data.DataSourceProvider.DataChanged" /> event on the specified source.</summary>
		/// <param name="source">The object with the event.</param>
		// Token: 0x06001B55 RID: 6997 RVA: 0x00080914 File Offset: 0x0007EB14
		protected override void StartListening(object source)
		{
			DataSourceProvider dataSourceProvider = (DataSourceProvider)source;
			dataSourceProvider.DataChanged += this.OnDataChanged;
		}

		/// <summary>Stops listening for the <see cref="E:System.Windows.Data.DataSourceProvider.DataChanged" /> event on the specified source.</summary>
		/// <param name="source">The source object to stop listening for.</param>
		// Token: 0x06001B56 RID: 6998 RVA: 0x0008093C File Offset: 0x0007EB3C
		protected override void StopListening(object source)
		{
			DataSourceProvider dataSourceProvider = (DataSourceProvider)source;
			dataSourceProvider.DataChanged -= this.OnDataChanged;
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06001B57 RID: 6999 RVA: 0x00080964 File Offset: 0x0007EB64
		private static DataChangedEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(DataChangedEventManager);
				DataChangedEventManager dataChangedEventManager = (DataChangedEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
				if (dataChangedEventManager == null)
				{
					dataChangedEventManager = new DataChangedEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, dataChangedEventManager);
				}
				return dataChangedEventManager;
			}
		}

		// Token: 0x06001B58 RID: 7000 RVA: 0x000174E5 File Offset: 0x000156E5
		private void OnDataChanged(object sender, EventArgs args)
		{
			base.DeliverEvent(sender, args);
		}
	}
}
