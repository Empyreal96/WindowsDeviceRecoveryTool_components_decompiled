using System;
using System.Net;

namespace System.Windows.Navigation
{
	/// <summary>Provides data for non-cancelable navigation events, including <see cref="E:System.Windows.Navigation.NavigationWindow.LoadCompleted" />, <see cref="E:System.Windows.Navigation.NavigationWindow.Navigated" />, and <see cref="E:System.Windows.Navigation.NavigationWindow.NavigationStopped" />.  </summary>
	// Token: 0x0200030A RID: 778
	public class NavigationEventArgs : EventArgs
	{
		// Token: 0x0600292C RID: 10540 RVA: 0x000BE351 File Offset: 0x000BC551
		internal NavigationEventArgs(Uri uri, object content, object extraData, WebResponse response, object Navigator, bool isNavigationInitiator)
		{
			this._uri = uri;
			this._content = content;
			this._extraData = extraData;
			this._webResponse = response;
			this._isNavigationInitiator = isNavigationInitiator;
			this._navigator = Navigator;
		}

		/// <summary>Gets the uniform resource identifier (URI) of the target page.</summary>
		/// <returns>The URI of the target page.</returns>
		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x0600292D RID: 10541 RVA: 0x000BE386 File Offset: 0x000BC586
		public Uri Uri
		{
			get
			{
				return this._uri;
			}
		}

		/// <summary>Gets the root node of the target page's content. </summary>
		/// <returns>The root element of the target page's content.</returns>
		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x0600292E RID: 10542 RVA: 0x000BE38E File Offset: 0x000BC58E
		public object Content
		{
			get
			{
				return this._content;
			}
		}

		/// <summary>Gets a value that indicates whether the current navigator initiated the navigation.</summary>
		/// <returns>
		///     <see langword="true" /> if the navigation was initiated inside the current frame; <see langword="false" /> if the parent navigator is also navigating.</returns>
		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x0600292F RID: 10543 RVA: 0x000BE396 File Offset: 0x000BC596
		public bool IsNavigationInitiator
		{
			get
			{
				return this._isNavigationInitiator;
			}
		}

		/// <summary>Gets an optional user-defined data object. </summary>
		/// <returns>The data object.</returns>
		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x06002930 RID: 10544 RVA: 0x000BE39E File Offset: 0x000BC59E
		public object ExtraData
		{
			get
			{
				return this._extraData;
			}
		}

		/// <summary>Gets the Web response to allow access to HTTP headers and other properties. </summary>
		/// <returns>The Web response.</returns>
		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x06002931 RID: 10545 RVA: 0x000BE3A6 File Offset: 0x000BC5A6
		public WebResponse WebResponse
		{
			get
			{
				return this._webResponse;
			}
		}

		/// <summary>Gets the navigator that raised the event </summary>
		/// <returns>The navigator that raised the event.</returns>
		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x06002932 RID: 10546 RVA: 0x000BE3AE File Offset: 0x000BC5AE
		public object Navigator
		{
			get
			{
				return this._navigator;
			}
		}

		// Token: 0x04001BD9 RID: 7129
		private Uri _uri;

		// Token: 0x04001BDA RID: 7130
		private object _content;

		// Token: 0x04001BDB RID: 7131
		private object _extraData;

		// Token: 0x04001BDC RID: 7132
		private WebResponse _webResponse;

		// Token: 0x04001BDD RID: 7133
		private bool _isNavigationInitiator;

		// Token: 0x04001BDE RID: 7134
		private object _navigator;
	}
}
