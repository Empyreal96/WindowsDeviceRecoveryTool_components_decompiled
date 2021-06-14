using System;
using MS.Internal;

namespace System.Windows
{
	/// <summary>Provides a <see cref="T:System.Windows.WeakEventManager" /> implementation so that you can use the "weak event listener" pattern to attach listeners for the <see cref="E:System.Windows.UIElement.LostFocus" /> or <see cref="E:System.Windows.ContentElement.LostFocus" /> events.</summary>
	// Token: 0x020000D7 RID: 215
	public class LostFocusEventManager : WeakEventManager
	{
		// Token: 0x0600076E RID: 1902 RVA: 0x0001737C File Offset: 0x0001557C
		private LostFocusEventManager()
		{
		}

		/// <summary>Adds the provided listener to the list of listeners on the provided source.</summary>
		/// <param name="source">The object with the event.</param>
		/// <param name="listener">The object to add as a listener.</param>
		// Token: 0x0600076F RID: 1903 RVA: 0x00017384 File Offset: 0x00015584
		public static void AddListener(DependencyObject source, IWeakEventListener listener)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			LostFocusEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		/// <summary>Removes the specified listener from the list of listeners on the provided source.</summary>
		/// <param name="source">The object to remove the listener from.</param>
		/// <param name="listener">The listener to remove.</param>
		// Token: 0x06000770 RID: 1904 RVA: 0x000173AE File Offset: 0x000155AE
		public static void RemoveListener(DependencyObject source, IWeakEventListener listener)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			LostFocusEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		/// <summary>Adds the specified event handler, which is called when specified source raises the <see cref="E:System.Windows.UIElement.LostFocus" /> or <see cref="E:System.Windows.ContentElement.LostFocus" /> event.</summary>
		/// <param name="source">The source object that the raises the <see cref="E:System.Windows.UIElement.LostFocus" /> or <see cref="E:System.Windows.ContentElement.LostFocus" /> event. </param>
		/// <param name="handler">The delegate that handles the <see cref="E:System.Windows.UIElement.LostFocus" /> or <see cref="E:System.Windows.ContentElement.LostFocus" /> event.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="handler" /> is <see langword="null" />.</exception>
		// Token: 0x06000771 RID: 1905 RVA: 0x000173D8 File Offset: 0x000155D8
		public static void AddHandler(DependencyObject source, EventHandler<RoutedEventArgs> handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			LostFocusEventManager.CurrentManager.ProtectedAddHandler(source, handler);
		}

		/// <summary>Removes the specified event handler from the specified source.</summary>
		/// <param name="source">The source object that the raises the <see cref="E:System.Windows.UIElement.LostFocus" /> or <see cref="E:System.Windows.ContentElement.LostFocus" /> event.</param>
		/// <param name="handler">The delegate that handles the <see cref="E:System.Windows.UIElement.LostFocus" /> or <see cref="E:System.Windows.ContentElement.LostFocus" /> event.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="handler" /> is <see langword="null" />.</exception>
		// Token: 0x06000772 RID: 1906 RVA: 0x000173F4 File Offset: 0x000155F4
		public static void RemoveHandler(DependencyObject source, EventHandler<RoutedEventArgs> handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			LostFocusEventManager.CurrentManager.ProtectedRemoveHandler(source, handler);
		}

		/// <summary>Returns a new object to contain listeners to the <see cref="E:System.Windows.UIElement.LostFocus" /> or <see cref="E:System.Windows.ContentElement.LostFocus" /> event.</summary>
		/// <returns>A new object to contain listeners to the <see cref="E:System.Windows.UIElement.LostFocus" /> or <see cref="E:System.Windows.ContentElement.LostFocus" /> event.</returns>
		// Token: 0x06000773 RID: 1907 RVA: 0x00017410 File Offset: 0x00015610
		protected override WeakEventManager.ListenerList NewListenerList()
		{
			return new WeakEventManager.ListenerList<RoutedEventArgs>();
		}

		/// <summary>Begins listening for the <see cref="E:System.Windows.UIElement.LostFocus" /> event on the given source, attaching an internal class handler to that source.</summary>
		/// <param name="source">The object on which to start listening for the pertinent <see cref="E:System.Windows.UIElement.LostFocus" /> event.</param>
		// Token: 0x06000774 RID: 1908 RVA: 0x00017418 File Offset: 0x00015618
		protected override void StartListening(object source)
		{
			DependencyObject d = (DependencyObject)source;
			FrameworkElement frameworkElement;
			FrameworkContentElement frameworkContentElement;
			Helper.DowncastToFEorFCE(d, out frameworkElement, out frameworkContentElement, true);
			if (frameworkElement != null)
			{
				frameworkElement.LostFocus += this.OnLostFocus;
				return;
			}
			if (frameworkContentElement != null)
			{
				frameworkContentElement.LostFocus += this.OnLostFocus;
			}
		}

		/// <summary>Stops listening for the <see cref="E:System.Windows.UIElement.LostFocus" /> event on the given source.</summary>
		/// <param name="source">The source object on which to stop listening for <see cref="E:System.Windows.UIElement.LostFocus" />.</param>
		// Token: 0x06000775 RID: 1909 RVA: 0x00017464 File Offset: 0x00015664
		protected override void StopListening(object source)
		{
			DependencyObject d = (DependencyObject)source;
			FrameworkElement frameworkElement;
			FrameworkContentElement frameworkContentElement;
			Helper.DowncastToFEorFCE(d, out frameworkElement, out frameworkContentElement, true);
			if (frameworkElement != null)
			{
				frameworkElement.LostFocus -= this.OnLostFocus;
				return;
			}
			if (frameworkContentElement != null)
			{
				frameworkContentElement.LostFocus -= this.OnLostFocus;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000776 RID: 1910 RVA: 0x000174B0 File Offset: 0x000156B0
		private static LostFocusEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(LostFocusEventManager);
				LostFocusEventManager lostFocusEventManager = (LostFocusEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
				if (lostFocusEventManager == null)
				{
					lostFocusEventManager = new LostFocusEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, lostFocusEventManager);
				}
				return lostFocusEventManager;
			}
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x000174E5 File Offset: 0x000156E5
		private void OnLostFocus(object sender, RoutedEventArgs args)
		{
			base.DeliverEvent(sender, args);
		}
	}
}
