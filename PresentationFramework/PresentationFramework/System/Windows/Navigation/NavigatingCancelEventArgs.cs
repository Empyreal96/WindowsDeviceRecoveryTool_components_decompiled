using System;
using System.ComponentModel;
using System.Net;

namespace System.Windows.Navigation
{
	/// <summary>Provides data for the Navigating event.</summary>
	// Token: 0x02000309 RID: 777
	public class NavigatingCancelEventArgs : CancelEventArgs
	{
		// Token: 0x06002921 RID: 10529 RVA: 0x000BE2B0 File Offset: 0x000BC4B0
		internal NavigatingCancelEventArgs(Uri uri, object content, CustomContentState customContentState, object extraData, NavigationMode navigationMode, WebRequest request, object Navigator, bool isNavInitiator)
		{
			this._uri = uri;
			this._content = content;
			this._targetContentState = customContentState;
			this._navigationMode = navigationMode;
			this._extraData = extraData;
			this._webRequest = request;
			this._isNavInitiator = isNavInitiator;
			this._navigator = Navigator;
		}

		/// <summary>Gets the uniform resource identifier (URI) for the content being navigated to.</summary>
		/// <returns>The <see cref="T:System.Uri" /> for the content being navigated to. If navigating to an object, <see cref="P:System.Windows.Navigation.NavigatingCancelEventArgs.Uri" /> is <see langword="null" />.</returns>
		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x06002922 RID: 10530 RVA: 0x000BE300 File Offset: 0x000BC500
		public Uri Uri
		{
			get
			{
				return this._uri;
			}
		}

		/// <summary>Gets a reference to the content object that is being navigated to.</summary>
		/// <returns>A <see cref="T:System.Object" /> reference to the content object that is being navigated to; otherwise, <see langword="null" />.</returns>
		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x06002923 RID: 10531 RVA: 0x000BE308 File Offset: 0x000BC508
		public object Content
		{
			get
			{
				return this._content;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Navigation.CustomContentState" /> object that is to be applied to the content being navigated to.</summary>
		/// <returns>The <see cref="T:System.Windows.Navigation.CustomContentState" /> object that is to be applied to the content being navigated to.</returns>
		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x06002924 RID: 10532 RVA: 0x000BE310 File Offset: 0x000BC510
		public CustomContentState TargetContentState
		{
			get
			{
				return this._targetContentState;
			}
		}

		/// <summary>Gets and sets the <see cref="T:System.Windows.Navigation.CustomContentState" /> object that is associated with the back navigation history entry for the page being navigated from.</summary>
		/// <returns>The <see cref="T:System.Windows.Navigation.CustomContentState" /> object that is associated with the back navigation history entry for the page being navigated from.</returns>
		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x06002926 RID: 10534 RVA: 0x000BE321 File Offset: 0x000BC521
		// (set) Token: 0x06002925 RID: 10533 RVA: 0x000BE318 File Offset: 0x000BC518
		public CustomContentState ContentStateToSave
		{
			get
			{
				return this._contentStateToSave;
			}
			set
			{
				this._contentStateToSave = value;
			}
		}

		/// <summary>Gets the optional data <see cref="T:System.Object" /> that was passed when navigation started.</summary>
		/// <returns>The optional data <see cref="T:System.Object" /> that was passed when navigation started.</returns>
		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x06002927 RID: 10535 RVA: 0x000BE329 File Offset: 0x000BC529
		public object ExtraData
		{
			get
			{
				return this._extraData;
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Navigation.NavigationMode" /> value that indicates the type of navigation that is occurring.</summary>
		/// <returns>A <see cref="T:System.Windows.Navigation.NavigationMode" /> value that indicates the type of navigation that is occurring.</returns>
		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x06002928 RID: 10536 RVA: 0x000BE331 File Offset: 0x000BC531
		public NavigationMode NavigationMode
		{
			get
			{
				return this._navigationMode;
			}
		}

		/// <summary>Gets the <see cref="T:System.Net.WebRequest" /> object that is used to request the specified content.</summary>
		/// <returns>Gets the <see cref="T:System.Net.WebRequest" /> object that is used to request the specified content. </returns>
		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x06002929 RID: 10537 RVA: 0x000BE339 File Offset: 0x000BC539
		public WebRequest WebRequest
		{
			get
			{
				return this._webRequest;
			}
		}

		/// <summary>Indicates whether the navigator (<see cref="T:System.Windows.Navigation.NavigationWindow" />, <see cref="T:System.Windows.Controls.Frame" />) that is specified by <see cref="P:System.Windows.Navigation.NavigatingCancelEventArgs.Navigator" /> is servicing this navigation, or whether a parent navigator is doing so.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value that is <see langword="true" /> if the navigator that is specified by <see cref="P:System.Windows.Navigation.NavigatingCancelEventArgs.Navigator" /> is servicing this navigation. Otherwise, <see langword="false" /> is returned, such as during a nested <see cref="T:System.Windows.Controls.Frame" /> navigation.</returns>
		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x0600292A RID: 10538 RVA: 0x000BE341 File Offset: 0x000BC541
		public bool IsNavigationInitiator
		{
			get
			{
				return this._isNavInitiator;
			}
		}

		/// <summary>The navigator that raised this event.</summary>
		/// <returns>An <see cref="T:System.Object" /> that is the navigator that raised this event</returns>
		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x0600292B RID: 10539 RVA: 0x000BE349 File Offset: 0x000BC549
		public object Navigator
		{
			get
			{
				return this._navigator;
			}
		}

		// Token: 0x04001BD0 RID: 7120
		private Uri _uri;

		// Token: 0x04001BD1 RID: 7121
		private object _content;

		// Token: 0x04001BD2 RID: 7122
		private CustomContentState _targetContentState;

		// Token: 0x04001BD3 RID: 7123
		private CustomContentState _contentStateToSave;

		// Token: 0x04001BD4 RID: 7124
		private object _extraData;

		// Token: 0x04001BD5 RID: 7125
		private NavigationMode _navigationMode;

		// Token: 0x04001BD6 RID: 7126
		private WebRequest _webRequest;

		// Token: 0x04001BD7 RID: 7127
		private bool _isNavInitiator;

		// Token: 0x04001BD8 RID: 7128
		private object _navigator;
	}
}
