using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Represents the logical window that contains one or more instances of <see cref="T:System.Windows.Forms.HtmlDocument" />.</summary>
	// Token: 0x02000275 RID: 629
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class HtmlWindow
	{
		// Token: 0x060025F3 RID: 9715 RVA: 0x000B5170 File Offset: 0x000B3370
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		internal HtmlWindow(HtmlShimManager shimManager, UnsafeNativeMethods.IHTMLWindow2 win)
		{
			this.htmlWindow2 = win;
			this.shimManager = shimManager;
		}

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x060025F4 RID: 9716 RVA: 0x000B5186 File Offset: 0x000B3386
		internal UnsafeNativeMethods.IHTMLWindow2 NativeHtmlWindow
		{
			get
			{
				return this.htmlWindow2;
			}
		}

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x060025F5 RID: 9717 RVA: 0x000B518E File Offset: 0x000B338E
		private HtmlShimManager ShimManager
		{
			get
			{
				return this.shimManager;
			}
		}

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x060025F6 RID: 9718 RVA: 0x000B5198 File Offset: 0x000B3398
		private HtmlWindow.HtmlWindowShim WindowShim
		{
			get
			{
				if (this.ShimManager != null)
				{
					HtmlWindow.HtmlWindowShim windowShim = this.ShimManager.GetWindowShim(this);
					if (windowShim == null)
					{
						this.shimManager.AddWindowShim(this);
						windowShim = this.ShimManager.GetWindowShim(this);
					}
					return windowShim;
				}
				return null;
			}
		}

		/// <summary>Gets the HTML document contained within the window.</summary>
		/// <returns>A valid instance of <see cref="T:System.Windows.Forms.HtmlDocument" />, if a document is loaded. If this window contains a FRAMESET, or no document is currently loaded, it will return <see langword="null" />.</returns>
		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x060025F7 RID: 9719 RVA: 0x000B51DC File Offset: 0x000B33DC
		public HtmlDocument Document
		{
			get
			{
				UnsafeNativeMethods.IHTMLDocument ihtmldocument = this.NativeHtmlWindow.GetDocument() as UnsafeNativeMethods.IHTMLDocument;
				if (ihtmldocument == null)
				{
					return null;
				}
				return new HtmlDocument(this.ShimManager, ihtmldocument);
			}
		}

		/// <summary>Gets the unmanaged interface wrapped by this class. </summary>
		/// <returns>An object that can be cast to an <see langword="IHTMLWindow2" />, <see langword="IHTMLWindow3" />, or <see langword="IHTMLWindow4 " />pointer.</returns>
		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x060025F8 RID: 9720 RVA: 0x000B520B File Offset: 0x000B340B
		public object DomWindow
		{
			get
			{
				return this.NativeHtmlWindow;
			}
		}

		/// <summary>Gets a reference to each of the FRAME elements defined within the Web page.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.HtmlWindowCollection" /> of a document's FRAME and IFRAME objects.</returns>
		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x060025F9 RID: 9721 RVA: 0x000B5214 File Offset: 0x000B3414
		public HtmlWindowCollection Frames
		{
			get
			{
				UnsafeNativeMethods.IHTMLFramesCollection2 frames = this.NativeHtmlWindow.GetFrames();
				if (frames == null)
				{
					return null;
				}
				return new HtmlWindowCollection(this.ShimManager, frames);
			}
		}

		/// <summary>Gets an object containing the user's most recently visited URLs. </summary>
		/// <returns>The <see cref="T:System.Windows.Forms.HtmlHistory" />  for the current window.</returns>
		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x060025FA RID: 9722 RVA: 0x000B5240 File Offset: 0x000B3440
		public HtmlHistory History
		{
			get
			{
				UnsafeNativeMethods.IOmHistory history = this.NativeHtmlWindow.GetHistory();
				if (history == null)
				{
					return null;
				}
				return new HtmlHistory(history);
			}
		}

		/// <summary>Gets a value indicating whether this window is open or closed.</summary>
		/// <returns>
		///     <see langword="true" /> if the window is still open on the screen; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x060025FB RID: 9723 RVA: 0x000B5264 File Offset: 0x000B3464
		public bool IsClosed
		{
			get
			{
				return this.NativeHtmlWindow.GetClosed();
			}
		}

		/// <summary>Gets or sets the name of the window. </summary>
		/// <returns>A <see cref="T:System.String" /> representing the name. </returns>
		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x060025FC RID: 9724 RVA: 0x000B5271 File Offset: 0x000B3471
		// (set) Token: 0x060025FD RID: 9725 RVA: 0x000B527E File Offset: 0x000B347E
		public string Name
		{
			get
			{
				return this.NativeHtmlWindow.GetName();
			}
			set
			{
				this.NativeHtmlWindow.SetName(value);
			}
		}

		/// <summary>Gets a reference to the window that opened the current window. </summary>
		/// <returns>The <see cref="T:System.Windows.Forms.HtmlWindow" /> that was created by a call to the <see cref="M:System.Windows.Forms.HtmlWindow.Open(System.String,System.String,System.String,System.Boolean)" /> or <see cref="M:System.Windows.Forms.HtmlWindow.OpenNew(System.String,System.String)" /> methods. If the window was not created using one of these methods, this property returns <see langword="null" />.</returns>
		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x060025FE RID: 9726 RVA: 0x000B528C File Offset: 0x000B348C
		public HtmlWindow Opener
		{
			get
			{
				UnsafeNativeMethods.IHTMLWindow2 ihtmlwindow = this.NativeHtmlWindow.GetOpener() as UnsafeNativeMethods.IHTMLWindow2;
				if (ihtmlwindow == null)
				{
					return null;
				}
				return new HtmlWindow(this.ShimManager, ihtmlwindow);
			}
		}

		/// <summary>Gets the window which resides above the current one in a page containing frames.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.HtmlWindow" /> that owns the current window. If the current window is not a FRAME, or is not embedded inside of a FRAME, it returns <see langword="null" />.</returns>
		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x060025FF RID: 9727 RVA: 0x000B52BC File Offset: 0x000B34BC
		public HtmlWindow Parent
		{
			get
			{
				UnsafeNativeMethods.IHTMLWindow2 parent = this.NativeHtmlWindow.GetParent();
				if (parent == null)
				{
					return null;
				}
				return new HtmlWindow(this.ShimManager, parent);
			}
		}

		/// <summary>Gets the position of the window's client area on the screen. </summary>
		/// <returns>A <see cref="T:System.Drawing.Point" /> describing the x -and y-coordinates of the top-left corner of the screen, in pixels. </returns>
		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06002600 RID: 9728 RVA: 0x000B52E6 File Offset: 0x000B34E6
		public Point Position
		{
			get
			{
				return new Point(((UnsafeNativeMethods.IHTMLWindow3)this.NativeHtmlWindow).GetScreenLeft(), ((UnsafeNativeMethods.IHTMLWindow3)this.NativeHtmlWindow).GetScreenTop());
			}
		}

		/// <summary>Gets or sets the size of the current window.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> describing the size of the window in pixels. </returns>
		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x06002601 RID: 9729 RVA: 0x000B5310 File Offset: 0x000B3510
		// (set) Token: 0x06002602 RID: 9730 RVA: 0x000B533F File Offset: 0x000B353F
		public Size Size
		{
			get
			{
				UnsafeNativeMethods.IHTMLElement body = this.NativeHtmlWindow.GetDocument().GetBody();
				return new Size(body.GetOffsetWidth(), body.GetOffsetHeight());
			}
			set
			{
				this.ResizeTo(value.Width, value.Height);
			}
		}

		/// <summary>Gets or sets the text displayed in the status bar of a window.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the current status text.</returns>
		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x06002603 RID: 9731 RVA: 0x000B5355 File Offset: 0x000B3555
		// (set) Token: 0x06002604 RID: 9732 RVA: 0x000B5362 File Offset: 0x000B3562
		public string StatusBarText
		{
			get
			{
				return this.NativeHtmlWindow.GetStatus();
			}
			set
			{
				this.NativeHtmlWindow.SetStatus(value);
			}
		}

		/// <summary>Gets the URL corresponding to the current item displayed in the window. </summary>
		/// <returns>A <see cref="T:System.Uri" /> describing the URL.</returns>
		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x06002605 RID: 9733 RVA: 0x000B5370 File Offset: 0x000B3570
		public Uri Url
		{
			get
			{
				UnsafeNativeMethods.IHTMLLocation location = this.NativeHtmlWindow.GetLocation();
				string text = (location == null) ? "" : location.GetHref();
				if (!string.IsNullOrEmpty(text))
				{
					return new Uri(text);
				}
				return null;
			}
		}

		/// <summary>Gets the frame element corresponding to this window.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.HtmlElement" /> corresponding to this window's FRAME element. If this window is not a frame, it returns <see langword="null" />. </returns>
		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x06002606 RID: 9734 RVA: 0x000B53AC File Offset: 0x000B35AC
		public HtmlElement WindowFrameElement
		{
			get
			{
				UnsafeNativeMethods.IHTMLElement ihtmlelement = ((UnsafeNativeMethods.IHTMLWindow4)this.NativeHtmlWindow).frameElement() as UnsafeNativeMethods.IHTMLElement;
				if (ihtmlelement == null)
				{
					return null;
				}
				return new HtmlElement(this.ShimManager, ihtmlelement);
			}
		}

		/// <summary>Displays a message box. </summary>
		/// <param name="message">The <see cref="T:System.String" /> to display in the message box.</param>
		// Token: 0x06002607 RID: 9735 RVA: 0x000B53E0 File Offset: 0x000B35E0
		public void Alert(string message)
		{
			this.NativeHtmlWindow.Alert(message);
		}

		/// <summary>Adds an event handler for the named HTML DOM event.</summary>
		/// <param name="eventName">The name of the event you want to handle.</param>
		/// <param name="eventHandler">A reference to the managed code that handles the event.</param>
		// Token: 0x06002608 RID: 9736 RVA: 0x000B53EE File Offset: 0x000B35EE
		public void AttachEventHandler(string eventName, EventHandler eventHandler)
		{
			this.WindowShim.AttachEventHandler(eventName, eventHandler);
		}

		/// <summary>Closes the window.</summary>
		// Token: 0x06002609 RID: 9737 RVA: 0x000B53FD File Offset: 0x000B35FD
		public void Close()
		{
			this.NativeHtmlWindow.Close();
		}

		/// <summary>Displays a dialog box with a message and buttons to solicit a yes/no response.</summary>
		/// <param name="message">The text to display to the user.</param>
		/// <returns>
		///     <see langword="true" /> if the user clicked Yes; <see langword="false" /> if the user clicked No or closed the dialog box.</returns>
		// Token: 0x0600260A RID: 9738 RVA: 0x000B540A File Offset: 0x000B360A
		public bool Confirm(string message)
		{
			return this.NativeHtmlWindow.Confirm(message);
		}

		/// <summary>Removes the named event handler.</summary>
		/// <param name="eventName">The name of the event you want to handle.</param>
		/// <param name="eventHandler">A reference to the managed code that handles the event.</param>
		// Token: 0x0600260B RID: 9739 RVA: 0x000B5418 File Offset: 0x000B3618
		public void DetachEventHandler(string eventName, EventHandler eventHandler)
		{
			this.WindowShim.DetachEventHandler(eventName, eventHandler);
		}

		/// <summary>Puts the focus on the current window.</summary>
		// Token: 0x0600260C RID: 9740 RVA: 0x000B5427 File Offset: 0x000B3627
		public void Focus()
		{
			this.NativeHtmlWindow.Focus();
		}

		/// <summary>Moves the window to the specified coordinates on the screen. </summary>
		/// <param name="x">The x-coordinate of the window's upper-left corner.</param>
		/// <param name="y">The y-coordinate of the window's upper-left corner.</param>
		/// <exception cref="T:System.UnauthorizedAccessException">The code trying to execute this operation does not have permission to manipulate this window. See the Remarks section for details.</exception>
		// Token: 0x0600260D RID: 9741 RVA: 0x000B5434 File Offset: 0x000B3634
		public void MoveTo(int x, int y)
		{
			this.NativeHtmlWindow.MoveTo(x, y);
		}

		/// <summary>Moves the window to the specified coordinates on the screen. </summary>
		/// <param name="point">The x- and y-coordinates of the window's upper-left corner. </param>
		/// <exception cref="T:System.UnauthorizedAccessException">The code trying to execute this operation does not have permission to manipulate this window. See the Remarks section for details.</exception>
		// Token: 0x0600260E RID: 9742 RVA: 0x000B5443 File Offset: 0x000B3643
		public void MoveTo(Point point)
		{
			this.NativeHtmlWindow.MoveTo(point.X, point.Y);
		}

		/// <summary>Displays a new document in the current window. </summary>
		/// <param name="url">The location, specified as a <see cref="T:System.Uri" />, of the document or object to display in the current window.</param>
		// Token: 0x0600260F RID: 9743 RVA: 0x000B545E File Offset: 0x000B365E
		public void Navigate(Uri url)
		{
			this.NativeHtmlWindow.Navigate(url.ToString());
		}

		/// <summary>Displays or downloads the new content located at the specified URL. </summary>
		/// <param name="urlString">The resource to display, described by a Uniform Resource Locator. </param>
		// Token: 0x06002610 RID: 9744 RVA: 0x000B5471 File Offset: 0x000B3671
		public void Navigate(string urlString)
		{
			this.NativeHtmlWindow.Navigate(urlString);
		}

		/// <summary>Displays a file in the named window.</summary>
		/// <param name="urlString">The Uniform Resource Locator that describes the location of the file to load.</param>
		/// <param name="target">The name of the window in which to open the resource. This may be a developer-supplied name, or one of the following special values:
		///       _blank: Opens <paramref name="url" /> in a new window. Works the same as a call to <see cref="M:System.Windows.Forms.HtmlWindow.OpenNew(System.String,System.String)" />.
		///       _media: Opens <paramref name="url" /> in the Media bar. 
		///       _parent: Opens <paramref name="url" /> in the window that created the current window.
		///       _search: Opens <paramref name="url" /> in the Search bar.
		///       _self: Opens <paramref name="url" /> in the current window. 
		///       _top: If called against a window belonging to a FRAME element, opens <paramref name="url" /> in the window hosting its FRAMESET. Otherwise, acts the same as _self.</param>
		/// <param name="windowOptions">A comma-delimited string consisting of zero or more of the following options in the form <paramref name="name=value" />. Except for the left, top, height, and width options, which take arbitrary integers, each option accepts yes or <see langword="1" />, and no or <see langword="0" />, as valid values.
		///       channelmode: Used with the deprecated channels technology of Internet Explorer 4.0. Default is no.
		///       directories: Whether the window should display directory navigation buttons. Default is yes. 
		///       height: The height of the window's client area, in pixels. The minimum is 100; attempts to open a window smaller than this will cause the window to open according to the Internet Explorer defaults. 
		///       left: The left (x-coordinate) position of the window, relative to the upper-left corner of the user's screen, in pixels. Must be a positive integer.
		///       location: Whether to display the Address bar, which enables users to navigate the window to a new URL. Default is yes. 
		///       menubar: Whether to display menus on the new window. Default is yes.
		///       resizable: Whether the window can be resized by the user. Default is yes.
		///       scrollbars: Whether the window has horizontal and vertical scroll bars. Default is yes.
		///       status: Whether the window has a status bar at the bottom. Default is yes.
		///       titlebar: Whether the title of the current page is displayed. Setting this option to no has no effect within a managed application; the title bar will always appear.
		///       toolbar: Whether toolbar buttons such as Back, Forward, and Stop are visible. Default is yes.
		///       top: The top (y-coordinate) position of the window, relative to the upper-left corner of the user's screen, in pixels. Must be a positive integer.
		///       width: The width of the window's client area, in pixels. The minimum is 100; attempts to open a window smaller than this will cause the window to open according to the Internet Explorer defaults.</param>
		/// <param name="replaceEntry">Whether <paramref name="url" /> replaces the current window's URL in the navigation history. This will effect the operation of methods on the <see cref="T:System.Windows.Forms.HtmlHistory" /> class.</param>
		/// <returns>An <see cref="T:System.Windows.Forms.HtmlWindow" /> representing the new window, or the previously created window named by the <paramref name="target" /> parameter.</returns>
		// Token: 0x06002611 RID: 9745 RVA: 0x000B5480 File Offset: 0x000B3680
		public HtmlWindow Open(string urlString, string target, string windowOptions, bool replaceEntry)
		{
			UnsafeNativeMethods.IHTMLWindow2 ihtmlwindow = this.NativeHtmlWindow.Open(urlString, target, windowOptions, replaceEntry);
			if (ihtmlwindow == null)
			{
				return null;
			}
			return new HtmlWindow(this.ShimManager, ihtmlwindow);
		}

		/// <summary>Displays a file in the named window.</summary>
		/// <param name="url">The Uniform Resource Locator that describes the location of the file to load.</param>
		/// <param name="target">The name of the window in which to open the resource. This can be a developer-supplied name, or one of the following special values:
		///       _blank: Opens <paramref name="url" /> in a new window. Works the same as a call to <see cref="M:System.Windows.Forms.HtmlWindow.OpenNew(System.String,System.String)" />.
		///       _media: Opens <paramref name="url" /> in the Media bar. 
		///       _parent: Opens <paramref name="url" /> in the window that created the current window.
		///       _search: Opens <paramref name="url" /> in the Search bar.
		///       _self: Opens <paramref name="url" /> in the current window. 
		///       _top: If called against a window belonging to a FRAME element, opens <paramref name="url" /> in the window hosting its FRAMESET. Otherwise, acts the same as _self.</param>
		/// <param name="windowOptions">A comma-delimited string consisting of zero or more of the following options in the form <paramref name="name=value" />. Except for the left, top, height, and width options, which take arbitrary integers, each option accepts yes or <see langword="1" />, and no or <see langword="0" />, as valid values.
		///       channelmode: Used with the deprecated channels technology of Internet Explorer 4.0. Default is no.
		///       directories: Whether the window should display directory navigation buttons. Default is yes. 
		///       height: The height of the window's client area, in pixels. The minimum is 100; attempts to open a window smaller than this will cause the window to open according to The Internet Explorer defaults. 
		///       left: The left (x-coordinate) position of the window, relative to the upper-left corner of the user's screen, in pixels. Must be a positive integer.
		///       location: Whether to display the Address bar, which enables users to navigate the window to a new URL. Default is yes. 
		///       menubar: Whether to display menus on the new window. Default is yes.
		///       resizable: Whether the window can be resized by the user. Default is yes.
		///       scrollbars: Whether the window has horizontal and vertical scroll bars. Default is yes.
		///       status: Whether the window has a status bar at the bottom. Default is yes.
		///       titlebar: Whether the title of the current page is displayed. Setting this option to no has no effect within a managed application; the title bar will always appear.
		///       toolbar: Whether toolbar buttons such as Back, Forward, and Stop are visible. Default is yes.
		///       top: The top (y-coordinate) position of the window, relative to the upper-left corner of the user's screen, in pixels. Must be a positive integer.
		///       width: The width of the window's client area, in pixels. The minimum is 100; attempts to open a window smaller than this will cause the window to open according to The Internet Explorer defaults.</param>
		/// <param name="replaceEntry">Whether <paramref name="url" /> replaces the current window's URL in the navigation history. This will effect the operation of methods on the <see cref="T:System.Windows.Forms.HtmlHistory" /> class. </param>
		/// <returns>An <see cref="T:System.Windows.Forms.HtmlWindow" /> representing the new window, or the previously created window named by the <paramref name="target" /> parameter.</returns>
		// Token: 0x06002612 RID: 9746 RVA: 0x000B54AF File Offset: 0x000B36AF
		public HtmlWindow Open(Uri url, string target, string windowOptions, bool replaceEntry)
		{
			return this.Open(url.ToString(), target, windowOptions, replaceEntry);
		}

		/// <summary>Displays a file in a new window.</summary>
		/// <param name="urlString">The Uniform Resource Locator that describes the location of the file to load.</param>
		/// <param name="windowOptions">A comma-delimited string consisting of zero or more of the following options in the form <paramref name="name=value" />. See <see cref="M:System.Windows.Forms.HtmlWindow.Open(System.String,System.String,System.String,System.Boolean)" /> for a full description of the valid options. </param>
		/// <returns>An <see cref="T:System.Windows.Forms.HtmlWindow" /> representing the new window. </returns>
		// Token: 0x06002613 RID: 9747 RVA: 0x000B54C4 File Offset: 0x000B36C4
		public HtmlWindow OpenNew(string urlString, string windowOptions)
		{
			UnsafeNativeMethods.IHTMLWindow2 ihtmlwindow = this.NativeHtmlWindow.Open(urlString, "_blank", windowOptions, true);
			if (ihtmlwindow == null)
			{
				return null;
			}
			return new HtmlWindow(this.ShimManager, ihtmlwindow);
		}

		/// <summary>Displays a file in a new window.</summary>
		/// <param name="url">The Uniform Resource Locator that describes the location of the file to load.</param>
		/// <param name="windowOptions">A comma-delimited string consisting of zero or more of the following options in the form <paramref name="name=value" />. See <see cref="M:System.Windows.Forms.HtmlWindow.Open(System.String,System.String,System.String,System.Boolean)" /> for a full description of the valid options. </param>
		/// <returns>An <see cref="T:System.Windows.Forms.HtmlWindow" /> representing the new window. </returns>
		// Token: 0x06002614 RID: 9748 RVA: 0x000B54F6 File Offset: 0x000B36F6
		public HtmlWindow OpenNew(Uri url, string windowOptions)
		{
			return this.OpenNew(url.ToString(), windowOptions);
		}

		/// <summary>Shows a dialog box that displays a message and a text box to the user. </summary>
		/// <param name="message">The message to display to the user.</param>
		/// <param name="defaultInputValue">The default value displayed in the text box.</param>
		/// <returns>A <see cref="T:System.String" /> representing the text entered by the user.</returns>
		// Token: 0x06002615 RID: 9749 RVA: 0x000B5505 File Offset: 0x000B3705
		public string Prompt(string message, string defaultInputValue)
		{
			return this.NativeHtmlWindow.Prompt(message, defaultInputValue).ToString();
		}

		/// <summary>Takes focus off of the current window. </summary>
		// Token: 0x06002616 RID: 9750 RVA: 0x000B5519 File Offset: 0x000B3719
		public void RemoveFocus()
		{
			this.NativeHtmlWindow.Blur();
		}

		/// <summary>Changes the size of the window to the specified dimensions. </summary>
		/// <param name="width">Describes the desired width of the window, in pixels. Must be 100 pixels or more.</param>
		/// <param name="height">Describes the desired height of the window, in pixels. Must be 100 pixels or more.</param>
		/// <exception cref="T:System.UnauthorizedAccessException">The window you are trying to resize is in a different domain than its parent window. This restriction is part of cross-frame scripting security; for more information, see About Cross-Frame Scripting and Security.</exception>
		// Token: 0x06002617 RID: 9751 RVA: 0x000B5526 File Offset: 0x000B3726
		public void ResizeTo(int width, int height)
		{
			this.NativeHtmlWindow.ResizeTo(width, height);
		}

		/// <summary>Changes the size of the window to the specified dimensions. </summary>
		/// <param name="size">A <see cref="T:System.Drawing.Size" /> describing the desired width and height of the window, in pixels. Must be 100 pixels or more in both dimensions. </param>
		/// <exception cref="T:System.UnauthorizedAccessException">The window you are trying to resize is in a different domain than its parent window. This restriction is part of cross-frame scripting security; for more information, see About Cross-Frame Scripting and Security.</exception>
		// Token: 0x06002618 RID: 9752 RVA: 0x000B5535 File Offset: 0x000B3735
		public void ResizeTo(Size size)
		{
			this.NativeHtmlWindow.ResizeTo(size.Width, size.Height);
		}

		/// <summary>Scrolls the window to the designated position.</summary>
		/// <param name="x">The x-coordinate, relative to the top-left corner of the current window, toward which the page should scroll.</param>
		/// <param name="y">The y-coordinate, relative to the top-left corner of the current window, toward which the page should scroll.</param>
		// Token: 0x06002619 RID: 9753 RVA: 0x000B5550 File Offset: 0x000B3750
		public void ScrollTo(int x, int y)
		{
			this.NativeHtmlWindow.ScrollTo(x, y);
		}

		/// <summary>Moves the window to the specified coordinates. </summary>
		/// <param name="point">The x- and y-coordinates, relative to the top-left corner of the current window, toward which the page should scroll. </param>
		// Token: 0x0600261A RID: 9754 RVA: 0x000B555F File Offset: 0x000B375F
		public void ScrollTo(Point point)
		{
			this.NativeHtmlWindow.ScrollTo(point.X, point.Y);
		}

		/// <summary>Occurs when script running inside of the window encounters a run-time error.</summary>
		// Token: 0x140001C7 RID: 455
		// (add) Token: 0x0600261B RID: 9755 RVA: 0x000B557A File Offset: 0x000B377A
		// (remove) Token: 0x0600261C RID: 9756 RVA: 0x000B558D File Offset: 0x000B378D
		public event HtmlElementErrorEventHandler Error
		{
			add
			{
				this.WindowShim.AddHandler(HtmlWindow.EventError, value);
			}
			remove
			{
				this.WindowShim.RemoveHandler(HtmlWindow.EventError, value);
			}
		}

		/// <summary>Occurs when the current window obtains user input focus.</summary>
		// Token: 0x140001C8 RID: 456
		// (add) Token: 0x0600261D RID: 9757 RVA: 0x000B55A0 File Offset: 0x000B37A0
		// (remove) Token: 0x0600261E RID: 9758 RVA: 0x000B55B3 File Offset: 0x000B37B3
		public event HtmlElementEventHandler GotFocus
		{
			add
			{
				this.WindowShim.AddHandler(HtmlWindow.EventGotFocus, value);
			}
			remove
			{
				this.WindowShim.RemoveHandler(HtmlWindow.EventGotFocus, value);
			}
		}

		/// <summary>Occurs when the window's document and all of its elements have finished initializing.</summary>
		// Token: 0x140001C9 RID: 457
		// (add) Token: 0x0600261F RID: 9759 RVA: 0x000B55C6 File Offset: 0x000B37C6
		// (remove) Token: 0x06002620 RID: 9760 RVA: 0x000B55D9 File Offset: 0x000B37D9
		public event HtmlElementEventHandler Load
		{
			add
			{
				this.WindowShim.AddHandler(HtmlWindow.EventLoad, value);
			}
			remove
			{
				this.WindowShim.RemoveHandler(HtmlWindow.EventLoad, value);
			}
		}

		/// <summary>Occurs when user input focus has left the window.</summary>
		// Token: 0x140001CA RID: 458
		// (add) Token: 0x06002621 RID: 9761 RVA: 0x000B55EC File Offset: 0x000B37EC
		// (remove) Token: 0x06002622 RID: 9762 RVA: 0x000B55FF File Offset: 0x000B37FF
		public event HtmlElementEventHandler LostFocus
		{
			add
			{
				this.WindowShim.AddHandler(HtmlWindow.EventLostFocus, value);
			}
			remove
			{
				this.WindowShim.RemoveHandler(HtmlWindow.EventLostFocus, value);
			}
		}

		/// <summary>Occurs when the user uses the mouse to change the dimensions of the window.</summary>
		// Token: 0x140001CB RID: 459
		// (add) Token: 0x06002623 RID: 9763 RVA: 0x000B5612 File Offset: 0x000B3812
		// (remove) Token: 0x06002624 RID: 9764 RVA: 0x000B5625 File Offset: 0x000B3825
		public event HtmlElementEventHandler Resize
		{
			add
			{
				this.WindowShim.AddHandler(HtmlWindow.EventResize, value);
			}
			remove
			{
				this.WindowShim.RemoveHandler(HtmlWindow.EventResize, value);
			}
		}

		/// <summary>Occurs when the user scrolls through the window to view off-screen text. </summary>
		// Token: 0x140001CC RID: 460
		// (add) Token: 0x06002625 RID: 9765 RVA: 0x000B5638 File Offset: 0x000B3838
		// (remove) Token: 0x06002626 RID: 9766 RVA: 0x000B564B File Offset: 0x000B384B
		public event HtmlElementEventHandler Scroll
		{
			add
			{
				this.WindowShim.AddHandler(HtmlWindow.EventScroll, value);
			}
			remove
			{
				this.WindowShim.RemoveHandler(HtmlWindow.EventScroll, value);
			}
		}

		/// <summary>Occurs when the current page is unloading, and a new page is about to be displayed. </summary>
		// Token: 0x140001CD RID: 461
		// (add) Token: 0x06002627 RID: 9767 RVA: 0x000B565E File Offset: 0x000B385E
		// (remove) Token: 0x06002628 RID: 9768 RVA: 0x000B5671 File Offset: 0x000B3871
		public event HtmlElementEventHandler Unload
		{
			add
			{
				this.WindowShim.AddHandler(HtmlWindow.EventUnload, value);
			}
			remove
			{
				this.WindowShim.RemoveHandler(HtmlWindow.EventUnload, value);
			}
		}

		/// <summary>Tests the two <see cref="T:System.Windows.Forms.HtmlWindow" /> objects for equality.</summary>
		/// <param name="left">The first <see cref="T:System.Windows.Forms.HtmlWindow" /> object.</param>
		/// <param name="right">The second <see cref="T:System.Windows.Forms.HtmlWindow" /> object.</param>
		/// <returns>
		///     <see langword="true" /> if both parameters are <see langword="null" />, or if both elements have the same underlying COM interface; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002629 RID: 9769 RVA: 0x000B5684 File Offset: 0x000B3884
		public static bool operator ==(HtmlWindow left, HtmlWindow right)
		{
			if (left == null != (right == null))
			{
				return false;
			}
			if (left == null)
			{
				return true;
			}
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			bool result;
			try
			{
				intPtr = Marshal.GetIUnknownForObject(left.NativeHtmlWindow);
				intPtr2 = Marshal.GetIUnknownForObject(right.NativeHtmlWindow);
				result = (intPtr == intPtr2);
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.Release(intPtr);
				}
				if (intPtr2 != IntPtr.Zero)
				{
					Marshal.Release(intPtr2);
				}
			}
			return result;
		}

		/// <summary>Tests two <see langword="HtmlWindow" /> objects for inequality.</summary>
		/// <param name="left">The first <see cref="T:System.Windows.Forms.HtmlWindow" /> object.</param>
		/// <param name="right">The second <see cref="T:System.Windows.Forms.HtmlWindow" /> object.</param>
		/// <returns>
		///     <see langword="true" /> if one but not both of the objects is <see langword="null" />, or the underlying COM pointers do not match; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600262A RID: 9770 RVA: 0x000B570C File Offset: 0x000B390C
		public static bool operator !=(HtmlWindow left, HtmlWindow right)
		{
			return !(left == right);
		}

		/// <summary>Serves as a hash function for a particular type.</summary>
		/// <returns>A hash code for the current <see cref="T:System.Windows.Forms.HtmlWindow" />.</returns>
		// Token: 0x0600262B RID: 9771 RVA: 0x000B5718 File Offset: 0x000B3918
		public override int GetHashCode()
		{
			if (this.htmlWindow2 != null)
			{
				return this.htmlWindow2.GetHashCode();
			}
			return 0;
		}

		/// <summary>Tests the object for equality against the current object.</summary>
		/// <param name="obj">The object to test.</param>
		/// <returns>
		///     <see langword="true" /> if the objects are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600262C RID: 9772 RVA: 0x000B572F File Offset: 0x000B392F
		public override bool Equals(object obj)
		{
			return this == (HtmlWindow)obj;
		}

		// Token: 0x04001028 RID: 4136
		internal static readonly object EventError = new object();

		// Token: 0x04001029 RID: 4137
		internal static readonly object EventGotFocus = new object();

		// Token: 0x0400102A RID: 4138
		internal static readonly object EventLoad = new object();

		// Token: 0x0400102B RID: 4139
		internal static readonly object EventLostFocus = new object();

		// Token: 0x0400102C RID: 4140
		internal static readonly object EventResize = new object();

		// Token: 0x0400102D RID: 4141
		internal static readonly object EventScroll = new object();

		// Token: 0x0400102E RID: 4142
		internal static readonly object EventUnload = new object();

		// Token: 0x0400102F RID: 4143
		private HtmlShimManager shimManager;

		// Token: 0x04001030 RID: 4144
		private UnsafeNativeMethods.IHTMLWindow2 htmlWindow2;

		// Token: 0x020005EE RID: 1518
		[ClassInterface(ClassInterfaceType.None)]
		private class HTMLWindowEvents2 : StandardOleMarshalObject, UnsafeNativeMethods.DHTMLWindowEvents2
		{
			// Token: 0x06005B81 RID: 23425 RVA: 0x0017ECE2 File Offset: 0x0017CEE2
			public HTMLWindowEvents2(HtmlWindow htmlWindow)
			{
				this.parent = htmlWindow;
			}

			// Token: 0x06005B82 RID: 23426 RVA: 0x0017ECF1 File Offset: 0x0017CEF1
			private void FireEvent(object key, EventArgs e)
			{
				if (this.parent != null)
				{
					this.parent.WindowShim.FireEvent(key, e);
				}
			}

			// Token: 0x06005B83 RID: 23427 RVA: 0x0017ED14 File Offset: 0x0017CF14
			public void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs e = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlWindow.EventGotFocus, e);
			}

			// Token: 0x06005B84 RID: 23428 RVA: 0x0017ED40 File Offset: 0x0017CF40
			public void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs e = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlWindow.EventLostFocus, e);
			}

			// Token: 0x06005B85 RID: 23429 RVA: 0x0017ED6C File Offset: 0x0017CF6C
			public bool onerror(string description, string urlString, int line)
			{
				HtmlElementErrorEventArgs htmlElementErrorEventArgs = new HtmlElementErrorEventArgs(description, urlString, line);
				this.FireEvent(HtmlWindow.EventError, htmlElementErrorEventArgs);
				return htmlElementErrorEventArgs.Handled;
			}

			// Token: 0x06005B86 RID: 23430 RVA: 0x0017ED94 File Offset: 0x0017CF94
			public void onload(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs e = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlWindow.EventLoad, e);
			}

			// Token: 0x06005B87 RID: 23431 RVA: 0x0017EDC0 File Offset: 0x0017CFC0
			public void onunload(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs e = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlWindow.EventUnload, e);
				if (this.parent != null)
				{
					this.parent.WindowShim.OnWindowUnload();
				}
			}

			// Token: 0x06005B88 RID: 23432 RVA: 0x0017EE0C File Offset: 0x0017D00C
			public void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs e = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlWindow.EventScroll, e);
			}

			// Token: 0x06005B89 RID: 23433 RVA: 0x0017EE38 File Offset: 0x0017D038
			public void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs e = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlWindow.EventResize, e);
			}

			// Token: 0x06005B8A RID: 23434 RVA: 0x0017EE64 File Offset: 0x0017D064
			public bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B8B RID: 23435 RVA: 0x0000701A File Offset: 0x0000521A
			public void onbeforeunload(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B8C RID: 23436 RVA: 0x0000701A File Offset: 0x0000521A
			public void onbeforeprint(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B8D RID: 23437 RVA: 0x0000701A File Offset: 0x0000521A
			public void onafterprint(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x040039B6 RID: 14774
			private HtmlWindow parent;
		}

		// Token: 0x020005EF RID: 1519
		internal class HtmlWindowShim : HtmlShim
		{
			// Token: 0x06005B8E RID: 23438 RVA: 0x0017EE89 File Offset: 0x0017D089
			public HtmlWindowShim(HtmlWindow window)
			{
				this.htmlWindow = window;
			}

			// Token: 0x170015ED RID: 5613
			// (get) Token: 0x06005B8F RID: 23439 RVA: 0x0017EE98 File Offset: 0x0017D098
			public UnsafeNativeMethods.IHTMLWindow2 NativeHtmlWindow
			{
				get
				{
					return this.htmlWindow.NativeHtmlWindow;
				}
			}

			// Token: 0x170015EE RID: 5614
			// (get) Token: 0x06005B90 RID: 23440 RVA: 0x0017EE98 File Offset: 0x0017D098
			public override UnsafeNativeMethods.IHTMLWindow2 AssociatedWindow
			{
				get
				{
					return this.htmlWindow.NativeHtmlWindow;
				}
			}

			// Token: 0x06005B91 RID: 23441 RVA: 0x0017EEA8 File Offset: 0x0017D0A8
			public override void AttachEventHandler(string eventName, EventHandler eventHandler)
			{
				HtmlToClrEventProxy pdisp = base.AddEventProxy(eventName, eventHandler);
				bool flag = ((UnsafeNativeMethods.IHTMLWindow3)this.NativeHtmlWindow).AttachEvent(eventName, pdisp);
			}

			// Token: 0x06005B92 RID: 23442 RVA: 0x0017EED4 File Offset: 0x0017D0D4
			public override void ConnectToEvents()
			{
				if (this.cookie == null || !this.cookie.Connected)
				{
					this.cookie = new AxHost.ConnectionPointCookie(this.NativeHtmlWindow, new HtmlWindow.HTMLWindowEvents2(this.htmlWindow), typeof(UnsafeNativeMethods.DHTMLWindowEvents2), false);
					if (!this.cookie.Connected)
					{
						this.cookie = null;
					}
				}
			}

			// Token: 0x06005B93 RID: 23443 RVA: 0x0017EF34 File Offset: 0x0017D134
			public override void DetachEventHandler(string eventName, EventHandler eventHandler)
			{
				HtmlToClrEventProxy htmlToClrEventProxy = base.RemoveEventProxy(eventHandler);
				if (htmlToClrEventProxy != null)
				{
					((UnsafeNativeMethods.IHTMLWindow3)this.NativeHtmlWindow).DetachEvent(eventName, htmlToClrEventProxy);
				}
			}

			// Token: 0x06005B94 RID: 23444 RVA: 0x0017EF5E File Offset: 0x0017D15E
			public override void DisconnectFromEvents()
			{
				if (this.cookie != null)
				{
					this.cookie.Disconnect();
					this.cookie = null;
				}
			}

			// Token: 0x06005B95 RID: 23445 RVA: 0x0017EF7A File Offset: 0x0017D17A
			protected override void Dispose(bool disposing)
			{
				base.Dispose(disposing);
				if (disposing)
				{
					if (this.htmlWindow != null && this.htmlWindow.NativeHtmlWindow != null)
					{
						Marshal.FinalReleaseComObject(this.htmlWindow.NativeHtmlWindow);
					}
					this.htmlWindow = null;
				}
			}

			// Token: 0x06005B96 RID: 23446 RVA: 0x0017EFB9 File Offset: 0x0017D1B9
			protected override object GetEventSender()
			{
				return this.htmlWindow;
			}

			// Token: 0x06005B97 RID: 23447 RVA: 0x0017EFC1 File Offset: 0x0017D1C1
			public void OnWindowUnload()
			{
				if (this.htmlWindow != null)
				{
					this.htmlWindow.ShimManager.OnWindowUnloaded(this.htmlWindow);
				}
			}

			// Token: 0x040039B7 RID: 14775
			private AxHost.ConnectionPointCookie cookie;

			// Token: 0x040039B8 RID: 14776
			private HtmlWindow htmlWindow;
		}
	}
}
