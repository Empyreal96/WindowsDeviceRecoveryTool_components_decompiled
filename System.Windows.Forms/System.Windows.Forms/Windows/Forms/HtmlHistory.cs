using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Manages the list of documents and Web sites the user has visited within the current session.</summary>
	// Token: 0x02000271 RID: 625
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class HtmlHistory : IDisposable
	{
		// Token: 0x060025BB RID: 9659 RVA: 0x000B482E File Offset: 0x000B2A2E
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		internal HtmlHistory(UnsafeNativeMethods.IOmHistory history)
		{
			this.htmlHistory = history;
		}

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x060025BC RID: 9660 RVA: 0x000B483D File Offset: 0x000B2A3D
		private UnsafeNativeMethods.IOmHistory NativeOmHistory
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				return this.htmlHistory;
			}
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Windows.Forms.HtmlHistory" />. </summary>
		// Token: 0x060025BD RID: 9661 RVA: 0x000B485E File Offset: 0x000B2A5E
		public void Dispose()
		{
			this.htmlHistory = null;
			this.disposed = true;
			GC.SuppressFinalize(this);
		}

		/// <summary>Gets the size of the history stack.</summary>
		/// <returns>The current number of entries in the Uniform Resource Locator (URL) history. </returns>
		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x060025BE RID: 9662 RVA: 0x000B4874 File Offset: 0x000B2A74
		public int Length
		{
			get
			{
				return (int)this.NativeOmHistory.GetLength();
			}
		}

		/// <summary>Navigates backward in the navigation stack by the specified number of entries.</summary>
		/// <param name="numberBack">The number of entries to navigate backward in the navigation stack. This number must be a positive integer.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Argument is not a positive 32-bit integer. </exception>
		// Token: 0x060025BF RID: 9663 RVA: 0x000B4884 File Offset: 0x000B2A84
		public void Back(int numberBack)
		{
			if (numberBack < 0)
			{
				throw new ArgumentOutOfRangeException("numberBack", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"numberBack",
					numberBack.ToString(CultureInfo.CurrentCulture),
					0.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (numberBack > 0)
			{
				object obj = -numberBack;
				this.NativeOmHistory.Go(ref obj);
			}
		}

		/// <summary>Navigates forward in the navigation stack by the specified number of entries. </summary>
		/// <param name="numberForward">The number of entries to navigate forward in the navigation stack. This number must be a positive integer.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Argument is not a positive 32-bit integer. </exception>
		// Token: 0x060025C0 RID: 9664 RVA: 0x000B48F4 File Offset: 0x000B2AF4
		public void Forward(int numberForward)
		{
			if (numberForward < 0)
			{
				throw new ArgumentOutOfRangeException("numberForward", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"numberForward",
					numberForward.ToString(CultureInfo.CurrentCulture),
					0.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (numberForward > 0)
			{
				object obj = numberForward;
				this.NativeOmHistory.Go(ref obj);
			}
		}

		/// <summary>Navigates to the specified Uniform Resource Locator (URL). </summary>
		/// <param name="url">The URL as a <see cref="T:System.Uri" /> object.</param>
		// Token: 0x060025C1 RID: 9665 RVA: 0x000B4960 File Offset: 0x000B2B60
		public void Go(Uri url)
		{
			this.Go(url.ToString());
		}

		/// <summary>Navigates to the specified Uniform Resource Locator (URL). </summary>
		/// <param name="urlString">The URL you want to display. This may be a relative or virtual URL (for example, page.html, path/page.html, or /path/to/page.html), in which case the current Web page's URL is used as a base. </param>
		// Token: 0x060025C2 RID: 9666 RVA: 0x000B4970 File Offset: 0x000B2B70
		public void Go(string urlString)
		{
			object obj = urlString;
			this.NativeOmHistory.Go(ref obj);
		}

		/// <summary>Navigates to the specified relative position in the browser's history. </summary>
		/// <param name="relativePosition">The entry in the navigation stack you want to display.</param>
		// Token: 0x060025C3 RID: 9667 RVA: 0x000B498C File Offset: 0x000B2B8C
		public void Go(int relativePosition)
		{
			object obj = relativePosition;
			this.NativeOmHistory.Go(ref obj);
		}

		/// <summary>Gets the unmanaged interface wrapped by this class. </summary>
		/// <returns>An <see cref="T:System.Object" /> that can be cast into an <see langword="IOmHistory" /> interface pointer.</returns>
		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x060025C4 RID: 9668 RVA: 0x000B49AD File Offset: 0x000B2BAD
		public object DomHistory
		{
			get
			{
				return this.NativeOmHistory;
			}
		}

		// Token: 0x0400101C RID: 4124
		private UnsafeNativeMethods.IOmHistory htmlHistory;

		// Token: 0x0400101D RID: 4125
		private bool disposed;
	}
}
