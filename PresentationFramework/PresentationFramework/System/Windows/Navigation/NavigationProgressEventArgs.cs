using System;

namespace System.Windows.Navigation
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Application.NavigationProgress" /> and <see cref="E:System.Windows.Navigation.NavigationWindow.NavigationProgress" /> events. </summary>
	// Token: 0x0200030D RID: 781
	public class NavigationProgressEventArgs : EventArgs
	{
		// Token: 0x0600293C RID: 10556 RVA: 0x000BE42C File Offset: 0x000BC62C
		internal NavigationProgressEventArgs(Uri uri, long bytesRead, long maxBytes, object Navigator)
		{
			this._uri = uri;
			this._bytesRead = bytesRead;
			this._maxBytes = maxBytes;
			this._navigator = Navigator;
		}

		/// <summary>Gets the uniform resource identifier (URI) of the target page. </summary>
		/// <returns>The URI of the target page.</returns>
		// Token: 0x17000A01 RID: 2561
		// (get) Token: 0x0600293D RID: 10557 RVA: 0x000BE451 File Offset: 0x000BC651
		public Uri Uri
		{
			get
			{
				return this._uri;
			}
		}

		/// <summary>Gets the number of bytes that have been read. </summary>
		/// <returns>The number of bytes that have been read.</returns>
		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x0600293E RID: 10558 RVA: 0x000BE459 File Offset: 0x000BC659
		public long BytesRead
		{
			get
			{
				return this._bytesRead;
			}
		}

		/// <summary>Gets the maximum number of bytes. </summary>
		/// <returns>The maximum number of bytes.</returns>
		// Token: 0x17000A03 RID: 2563
		// (get) Token: 0x0600293F RID: 10559 RVA: 0x000BE461 File Offset: 0x000BC661
		public long MaxBytes
		{
			get
			{
				return this._maxBytes;
			}
		}

		/// <summary>Gets the navigator that raised the event </summary>
		/// <returns>The navigator that raised the event</returns>
		// Token: 0x17000A04 RID: 2564
		// (get) Token: 0x06002940 RID: 10560 RVA: 0x000BE469 File Offset: 0x000BC669
		public object Navigator
		{
			get
			{
				return this._navigator;
			}
		}

		// Token: 0x04001BEB RID: 7147
		private Uri _uri;

		// Token: 0x04001BEC RID: 7148
		private long _bytesRead;

		// Token: 0x04001BED RID: 7149
		private long _maxBytes;

		// Token: 0x04001BEE RID: 7150
		private object _navigator;
	}
}
