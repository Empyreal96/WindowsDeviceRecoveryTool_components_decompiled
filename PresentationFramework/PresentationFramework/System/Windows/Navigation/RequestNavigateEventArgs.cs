using System;
using System.Windows.Documents;

namespace System.Windows.Navigation
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Documents.Hyperlink.RequestNavigate" /> event. </summary>
	// Token: 0x02000324 RID: 804
	public class RequestNavigateEventArgs : RoutedEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Navigation.RequestNavigateEventArgs" /> class. This constructor is protected.</summary>
		// Token: 0x06002A7E RID: 10878 RVA: 0x000C288A File Offset: 0x000C0A8A
		protected RequestNavigateEventArgs()
		{
			base.RoutedEvent = Hyperlink.RequestNavigateEvent;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Navigation.RequestNavigateEventArgs" /> class with a uniform resource identifier (URI) and target name.</summary>
		/// <param name="uri">The target URI.</param>
		/// <param name="target">The target name.</param>
		// Token: 0x06002A7F RID: 10879 RVA: 0x000C289D File Offset: 0x000C0A9D
		public RequestNavigateEventArgs(Uri uri, string target)
		{
			this._uri = uri;
			this._target = target;
			base.RoutedEvent = Hyperlink.RequestNavigateEvent;
		}

		/// <summary>The uniform resource identifier (URI) for the content that is being navigated to.</summary>
		/// <returns>The URI for the content that is being navigated to.</returns>
		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x06002A80 RID: 10880 RVA: 0x000C28BE File Offset: 0x000C0ABE
		public Uri Uri
		{
			get
			{
				return this._uri;
			}
		}

		/// <summary>The navigator that will host the content that is navigated to.</summary>
		/// <returns>The navigator (<see cref="T:System.Windows.Navigation.NavigationWindow" /> or <see cref="T:System.Windows.Controls.Frame" />) that will host the content that is navigated to.</returns>
		// Token: 0x17000A46 RID: 2630
		// (get) Token: 0x06002A81 RID: 10881 RVA: 0x000C28C6 File Offset: 0x000C0AC6
		public string Target
		{
			get
			{
				return this._target;
			}
		}

		/// <summary>Invokes a specified event handler from a specified sender. </summary>
		/// <param name="genericHandler">The name of the handler.</param>
		/// <param name="genericTarget">The object that is raising the event.</param>
		// Token: 0x06002A82 RID: 10882 RVA: 0x000C28D0 File Offset: 0x000C0AD0
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			if (base.RoutedEvent == null)
			{
				throw new InvalidOperationException(SR.Get("RequestNavigateEventMustHaveRoutedEvent"));
			}
			RequestNavigateEventHandler requestNavigateEventHandler = (RequestNavigateEventHandler)genericHandler;
			requestNavigateEventHandler(genericTarget, this);
		}

		// Token: 0x04001C42 RID: 7234
		private Uri _uri;

		// Token: 0x04001C43 RID: 7235
		private string _target;
	}
}
