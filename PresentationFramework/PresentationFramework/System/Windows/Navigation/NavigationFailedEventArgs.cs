using System;
using System.Net;

namespace System.Windows.Navigation
{
	/// <summary>Provides data for the NavigationFailed event.</summary>
	// Token: 0x0200030B RID: 779
	public class NavigationFailedEventArgs : EventArgs
	{
		// Token: 0x06002933 RID: 10547 RVA: 0x000BE3B6 File Offset: 0x000BC5B6
		internal NavigationFailedEventArgs(Uri uri, object extraData, object navigator, WebRequest request, WebResponse response, Exception e)
		{
			this._uri = uri;
			this._extraData = extraData;
			this._navigator = navigator;
			this._request = request;
			this._response = response;
			this._exception = e;
		}

		/// <summary>Gets the uniform resource identifier (URI) for the content that could not be navigated to.</summary>
		/// <returns>The <see cref="T:System.Uri" /> for the content that could not be navigated to.</returns>
		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x06002934 RID: 10548 RVA: 0x000BE3EB File Offset: 0x000BC5EB
		public Uri Uri
		{
			get
			{
				return this._uri;
			}
		}

		/// <summary>Gets the optional data <see cref="T:System.Object" /> that was passed when navigation commenced.</summary>
		/// <returns>The optional data <see cref="T:System.Object" /> that was passed when navigation commenced.</returns>
		// Token: 0x170009FB RID: 2555
		// (get) Token: 0x06002935 RID: 10549 RVA: 0x000BE3F3 File Offset: 0x000BC5F3
		public object ExtraData
		{
			get
			{
				return this._extraData;
			}
		}

		/// <summary>The navigator that raised this event.</summary>
		/// <returns>An <see cref="T:System.Object" /> that is the navigator that raised this event</returns>
		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x06002936 RID: 10550 RVA: 0x000BE3FB File Offset: 0x000BC5FB
		public object Navigator
		{
			get
			{
				return this._navigator;
			}
		}

		/// <summary>Gets the web request that was used to request the specified content.</summary>
		/// <returns>Gets the <see cref="T:System.Net.WebRequest" /> object that was used to request the specified content. If navigating to an object, <see cref="P:System.Windows.Navigation.NavigationFailedEventArgs.WebRequest" /> is <see langword="null" />.</returns>
		// Token: 0x170009FD RID: 2557
		// (get) Token: 0x06002937 RID: 10551 RVA: 0x000BE403 File Offset: 0x000BC603
		public WebRequest WebRequest
		{
			get
			{
				return this._request;
			}
		}

		/// <summary>Gets the web response that was returned after attempting to download the requested the specified content.</summary>
		/// <returns>The <see cref="T:System.Net.WebResponse" /> that was returned after attempting to download the requested the specified content. If the navigation failed, <see cref="P:System.Windows.Navigation.NavigationFailedEventArgs.WebResponse" /> is <see langword="null" />.</returns>
		// Token: 0x170009FE RID: 2558
		// (get) Token: 0x06002938 RID: 10552 RVA: 0x000BE40B File Offset: 0x000BC60B
		public WebResponse WebResponse
		{
			get
			{
				return this._response;
			}
		}

		/// <summary>Gets the <see cref="T:System.Exception" /> that was raised as the result of a failed navigation.</summary>
		/// <returns>The <see cref="T:System.Exception" /> that was raised as the result of a failed navigation.</returns>
		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x06002939 RID: 10553 RVA: 0x000BE413 File Offset: 0x000BC613
		public Exception Exception
		{
			get
			{
				return this._exception;
			}
		}

		/// <summary>Gets or sets whether the failed navigation exception has been handled.</summary>
		/// <returns>
		///     <see langword="true" /> if the exception is handled; otherwise, <see langword="false" /> (default).</returns>
		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x0600293A RID: 10554 RVA: 0x000BE41B File Offset: 0x000BC61B
		// (set) Token: 0x0600293B RID: 10555 RVA: 0x000BE423 File Offset: 0x000BC623
		public bool Handled
		{
			get
			{
				return this._handled;
			}
			set
			{
				this._handled = value;
			}
		}

		// Token: 0x04001BDF RID: 7135
		private Uri _uri;

		// Token: 0x04001BE0 RID: 7136
		private object _extraData;

		// Token: 0x04001BE1 RID: 7137
		private object _navigator;

		// Token: 0x04001BE2 RID: 7138
		private WebRequest _request;

		// Token: 0x04001BE3 RID: 7139
		private WebResponse _response;

		// Token: 0x04001BE4 RID: 7140
		private Exception _exception;

		// Token: 0x04001BE5 RID: 7141
		private bool _handled;
	}
}
