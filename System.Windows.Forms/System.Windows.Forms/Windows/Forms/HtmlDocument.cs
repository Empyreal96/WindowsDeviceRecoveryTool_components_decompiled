using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Provides top-level programmatic access to an HTML document hosted by the <see cref="T:System.Windows.Forms.WebBrowser" /> control.</summary>
	// Token: 0x02000269 RID: 617
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class HtmlDocument
	{
		// Token: 0x060024E6 RID: 9446 RVA: 0x000B2AA0 File Offset: 0x000B0CA0
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		internal HtmlDocument(HtmlShimManager shimManager, UnsafeNativeMethods.IHTMLDocument doc)
		{
			this.htmlDocument2 = (UnsafeNativeMethods.IHTMLDocument2)doc;
			this.shimManager = shimManager;
		}

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x060024E7 RID: 9447 RVA: 0x000B2ABB File Offset: 0x000B0CBB
		internal UnsafeNativeMethods.IHTMLDocument2 NativeHtmlDocument2
		{
			get
			{
				return this.htmlDocument2;
			}
		}

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x060024E8 RID: 9448 RVA: 0x000B2AC4 File Offset: 0x000B0CC4
		private HtmlDocument.HtmlDocumentShim DocumentShim
		{
			get
			{
				if (this.ShimManager != null)
				{
					HtmlDocument.HtmlDocumentShim documentShim = this.ShimManager.GetDocumentShim(this);
					if (documentShim == null)
					{
						this.shimManager.AddDocumentShim(this);
						documentShim = this.ShimManager.GetDocumentShim(this);
					}
					return documentShim;
				}
				return null;
			}
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x060024E9 RID: 9449 RVA: 0x000B2B05 File Offset: 0x000B0D05
		private HtmlShimManager ShimManager
		{
			get
			{
				return this.shimManager;
			}
		}

		/// <summary>Provides the <see cref="T:System.Windows.Forms.HtmlElement" /> which currently has user input focus. </summary>
		/// <returns>The <see cref="T:System.Windows.Forms.HtmlElement" /> which currently has user input focus.</returns>
		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x060024EA RID: 9450 RVA: 0x000B2B10 File Offset: 0x000B0D10
		public HtmlElement ActiveElement
		{
			get
			{
				UnsafeNativeMethods.IHTMLElement activeElement = this.NativeHtmlDocument2.GetActiveElement();
				if (activeElement == null)
				{
					return null;
				}
				return new HtmlElement(this.ShimManager, activeElement);
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.HtmlElement" /> for the BODY tag. </summary>
		/// <returns>The <see cref="T:System.Windows.Forms.HtmlElement" /> object for the BODY tag.</returns>
		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x060024EB RID: 9451 RVA: 0x000B2B3C File Offset: 0x000B0D3C
		public HtmlElement Body
		{
			get
			{
				UnsafeNativeMethods.IHTMLElement body = this.NativeHtmlDocument2.GetBody();
				if (body == null)
				{
					return null;
				}
				return new HtmlElement(this.ShimManager, body);
			}
		}

		/// <summary>Gets or sets the string describing the domain of this document for security purposes.</summary>
		/// <returns>A valid domain. </returns>
		/// <exception cref="T:System.ArgumentException">The argument for the Domain property must be a valid domain name using Domain Name System (DNS) conventions.</exception>
		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x060024EC RID: 9452 RVA: 0x000B2B66 File Offset: 0x000B0D66
		// (set) Token: 0x060024ED RID: 9453 RVA: 0x000B2B74 File Offset: 0x000B0D74
		public string Domain
		{
			get
			{
				return this.NativeHtmlDocument2.GetDomain();
			}
			set
			{
				try
				{
					this.NativeHtmlDocument2.SetDomain(value);
				}
				catch (ArgumentException)
				{
					throw new ArgumentException(SR.GetString("HtmlDocumentInvalidDomain"));
				}
			}
		}

		/// <summary>Gets or sets the text value of the &lt;TITLE&gt; tag in the current HTML document. </summary>
		/// <returns>The title of the current document.</returns>
		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x060024EE RID: 9454 RVA: 0x000B2BB0 File Offset: 0x000B0DB0
		// (set) Token: 0x060024EF RID: 9455 RVA: 0x000B2BBD File Offset: 0x000B0DBD
		public string Title
		{
			get
			{
				return this.NativeHtmlDocument2.GetTitle();
			}
			set
			{
				this.NativeHtmlDocument2.SetTitle(value);
			}
		}

		/// <summary>Gets the URL describing the location of this document. </summary>
		/// <returns>A <see cref="T:System.Uri" /> representing this document's URL. </returns>
		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x060024F0 RID: 9456 RVA: 0x000B2BCC File Offset: 0x000B0DCC
		public Uri Url
		{
			get
			{
				UnsafeNativeMethods.IHTMLLocation location = this.NativeHtmlDocument2.GetLocation();
				string text = (location == null) ? "" : location.GetHref();
				if (!string.IsNullOrEmpty(text))
				{
					return new Uri(text);
				}
				return null;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.HtmlWindow" /> associated with this document.</summary>
		/// <returns>The window for this document. </returns>
		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x060024F1 RID: 9457 RVA: 0x000B2C08 File Offset: 0x000B0E08
		public HtmlWindow Window
		{
			get
			{
				UnsafeNativeMethods.IHTMLWindow2 parentWindow = this.NativeHtmlDocument2.GetParentWindow();
				if (parentWindow == null)
				{
					return null;
				}
				return new HtmlWindow(this.ShimManager, parentWindow);
			}
		}

		/// <summary>Gets or sets the background color of the HTML document.</summary>
		/// <returns>The <see cref="T:System.Drawing.Color" /> of the document's background.</returns>
		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x060024F2 RID: 9458 RVA: 0x000B2C34 File Offset: 0x000B0E34
		// (set) Token: 0x060024F3 RID: 9459 RVA: 0x000B2C7C File Offset: 0x000B0E7C
		public Color BackColor
		{
			get
			{
				Color result = Color.Empty;
				try
				{
					result = this.ColorFromObject(this.NativeHtmlDocument2.GetBgColor());
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				return result;
			}
			set
			{
				int num = (int)value.R << 16 | (int)value.G << 8 | (int)value.B;
				this.NativeHtmlDocument2.SetBgColor(num);
			}
		}

		/// <summary>Gets or sets the text color for the document.</summary>
		/// <returns>The color of the text in the document. </returns>
		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x060024F4 RID: 9460 RVA: 0x000B2CB8 File Offset: 0x000B0EB8
		// (set) Token: 0x060024F5 RID: 9461 RVA: 0x000B2D00 File Offset: 0x000B0F00
		public Color ForeColor
		{
			get
			{
				Color result = Color.Empty;
				try
				{
					result = this.ColorFromObject(this.NativeHtmlDocument2.GetFgColor());
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				return result;
			}
			set
			{
				int num = (int)value.R << 16 | (int)value.G << 8 | (int)value.B;
				this.NativeHtmlDocument2.SetFgColor(num);
			}
		}

		/// <summary>Gets or sets the color of hyperlinks.</summary>
		/// <returns>The color for hyperlinks in the current document.</returns>
		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x060024F6 RID: 9462 RVA: 0x000B2D3C File Offset: 0x000B0F3C
		// (set) Token: 0x060024F7 RID: 9463 RVA: 0x000B2D84 File Offset: 0x000B0F84
		public Color LinkColor
		{
			get
			{
				Color result = Color.Empty;
				try
				{
					result = this.ColorFromObject(this.NativeHtmlDocument2.GetLinkColor());
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				return result;
			}
			set
			{
				int num = (int)value.R << 16 | (int)value.G << 8 | (int)value.B;
				this.NativeHtmlDocument2.SetLinkColor(num);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Drawing.Color" /> of a hyperlink when clicked by a user. </summary>
		/// <returns>The <see cref="T:System.Drawing.Color" /> for active links. </returns>
		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x060024F8 RID: 9464 RVA: 0x000B2DC0 File Offset: 0x000B0FC0
		// (set) Token: 0x060024F9 RID: 9465 RVA: 0x000B2E08 File Offset: 0x000B1008
		public Color ActiveLinkColor
		{
			get
			{
				Color result = Color.Empty;
				try
				{
					result = this.ColorFromObject(this.NativeHtmlDocument2.GetAlinkColor());
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				return result;
			}
			set
			{
				int num = (int)value.R << 16 | (int)value.G << 8 | (int)value.B;
				this.NativeHtmlDocument2.SetAlinkColor(num);
			}
		}

		/// <summary>Gets or sets the Color of links to HTML pages that the user has already visited. </summary>
		/// <returns>The color of visited links. </returns>
		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x060024FA RID: 9466 RVA: 0x000B2E44 File Offset: 0x000B1044
		// (set) Token: 0x060024FB RID: 9467 RVA: 0x000B2E8C File Offset: 0x000B108C
		public Color VisitedLinkColor
		{
			get
			{
				Color result = Color.Empty;
				try
				{
					result = this.ColorFromObject(this.NativeHtmlDocument2.GetVlinkColor());
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				return result;
			}
			set
			{
				int num = (int)value.R << 16 | (int)value.G << 8 | (int)value.B;
				this.NativeHtmlDocument2.SetVlinkColor(num);
			}
		}

		/// <summary>Gets a value indicating whether the document has user input focus. </summary>
		/// <returns>
		///     <see langword="true" /> if the document has focus; otherwise, <see langword="false" />.</returns>
		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x060024FC RID: 9468 RVA: 0x000B2EC7 File Offset: 0x000B10C7
		public bool Focused
		{
			get
			{
				return ((UnsafeNativeMethods.IHTMLDocument4)this.NativeHtmlDocument2).HasFocus();
			}
		}

		/// <summary>Gets the unmanaged interface pointer for this <see cref="T:System.Windows.Forms.HtmlDocument" />.</summary>
		/// <returns>An <see cref="T:System.Object" /> representing an IDispatch pointer to the unmanaged document. </returns>
		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x060024FD RID: 9469 RVA: 0x000B2ED9 File Offset: 0x000B10D9
		public object DomDocument
		{
			get
			{
				return this.NativeHtmlDocument2;
			}
		}

		/// <summary>Gets or sets the HTTP cookies associated with this document.</summary>
		/// <returns>A <see cref="T:System.String" /> containing a list of cookies, with each cookie separated by a semicolon.</returns>
		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x060024FE RID: 9470 RVA: 0x000B2EE1 File Offset: 0x000B10E1
		// (set) Token: 0x060024FF RID: 9471 RVA: 0x000B2EEE File Offset: 0x000B10EE
		public string Cookie
		{
			get
			{
				return this.NativeHtmlDocument2.GetCookie();
			}
			set
			{
				this.NativeHtmlDocument2.SetCookie(value);
			}
		}

		/// <summary>Gets or sets the direction of text in the current document.</summary>
		/// <returns>
		///     <see langword="true" /> if text renders from right to left; otherwise, <see langword="false" />.</returns>
		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06002500 RID: 9472 RVA: 0x000B2EFC File Offset: 0x000B10FC
		// (set) Token: 0x06002501 RID: 9473 RVA: 0x000B2F18 File Offset: 0x000B1118
		public bool RightToLeft
		{
			get
			{
				return ((UnsafeNativeMethods.IHTMLDocument3)this.NativeHtmlDocument2).GetDir() == "rtl";
			}
			set
			{
				((UnsafeNativeMethods.IHTMLDocument3)this.NativeHtmlDocument2).SetDir(value ? "rtl" : "ltr");
			}
		}

		/// <summary>Gets or sets the character encoding for this document.</summary>
		/// <returns>The <see cref="T:System.String" /> representing the current character encoding.</returns>
		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x06002502 RID: 9474 RVA: 0x000B2F39 File Offset: 0x000B1139
		// (set) Token: 0x06002503 RID: 9475 RVA: 0x000B2F46 File Offset: 0x000B1146
		public string Encoding
		{
			get
			{
				return this.NativeHtmlDocument2.GetCharset();
			}
			set
			{
				this.NativeHtmlDocument2.SetCharset(value);
			}
		}

		/// <summary>Gets the encoding used by default for the current document. </summary>
		/// <returns>The <see cref="T:System.String" /> representing the encoding that the browser uses when the page is first displayed.</returns>
		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06002504 RID: 9476 RVA: 0x000B2F54 File Offset: 0x000B1154
		public string DefaultEncoding
		{
			get
			{
				return this.NativeHtmlDocument2.GetDefaultCharset();
			}
		}

		/// <summary>Gets an instance of <see cref="T:System.Windows.Forms.HtmlElementCollection" />, which stores all <see cref="T:System.Windows.Forms.HtmlElement" /> objects for the document. </summary>
		/// <returns>The <see cref="T:System.Windows.Forms.HtmlElementCollection" /> of all elements in the document.</returns>
		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06002505 RID: 9477 RVA: 0x000B2F64 File Offset: 0x000B1164
		public HtmlElementCollection All
		{
			get
			{
				UnsafeNativeMethods.IHTMLElementCollection all = this.NativeHtmlDocument2.GetAll();
				if (all == null)
				{
					return new HtmlElementCollection(this.ShimManager);
				}
				return new HtmlElementCollection(this.ShimManager, all);
			}
		}

		/// <summary>Gets a list of all the hyperlinks within this HTML document.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.HtmlElementCollection" /> of <see cref="T:System.Windows.Forms.HtmlElement" /> objects.</returns>
		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06002506 RID: 9478 RVA: 0x000B2F98 File Offset: 0x000B1198
		public HtmlElementCollection Links
		{
			get
			{
				UnsafeNativeMethods.IHTMLElementCollection links = this.NativeHtmlDocument2.GetLinks();
				if (links == null)
				{
					return new HtmlElementCollection(this.ShimManager);
				}
				return new HtmlElementCollection(this.ShimManager, links);
			}
		}

		/// <summary>Gets a collection of all image tags in the document. </summary>
		/// <returns>A collection of <see cref="T:System.Windows.Forms.HtmlElement" /> objects, one for each IMG tag in the document. Elements are returned from the collection in source order. </returns>
		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06002507 RID: 9479 RVA: 0x000B2FCC File Offset: 0x000B11CC
		public HtmlElementCollection Images
		{
			get
			{
				UnsafeNativeMethods.IHTMLElementCollection images = this.NativeHtmlDocument2.GetImages();
				if (images == null)
				{
					return new HtmlElementCollection(this.ShimManager);
				}
				return new HtmlElementCollection(this.ShimManager, images);
			}
		}

		/// <summary>Gets a collection of all of the &lt;FORM&gt; elements in the document. </summary>
		/// <returns>An <see cref="T:System.Windows.Forms.HtmlElementCollection" /> of the &lt;FORM&gt; elements within the document.</returns>
		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06002508 RID: 9480 RVA: 0x000B3000 File Offset: 0x000B1200
		public HtmlElementCollection Forms
		{
			get
			{
				UnsafeNativeMethods.IHTMLElementCollection forms = this.NativeHtmlDocument2.GetForms();
				if (forms == null)
				{
					return new HtmlElementCollection(this.ShimManager);
				}
				return new HtmlElementCollection(this.ShimManager, forms);
			}
		}

		/// <summary>Writes a new HTML page.</summary>
		/// <param name="text">The HTML text to write into the document.</param>
		// Token: 0x06002509 RID: 9481 RVA: 0x000B3034 File Offset: 0x000B1234
		public void Write(string text)
		{
			object[] psarray = new object[]
			{
				text
			};
			this.NativeHtmlDocument2.Write(psarray);
		}

		/// <summary>Executes the specified command against the document. </summary>
		/// <param name="command">The name of the command to execute.</param>
		/// <param name="showUI">Whether or not to show command-specific dialog boxes or message boxes to the user. </param>
		/// <param name="value">The value to assign using the command. Not applicable for all commands.</param>
		// Token: 0x0600250A RID: 9482 RVA: 0x000B3059 File Offset: 0x000B1259
		public void ExecCommand(string command, bool showUI, object value)
		{
			this.NativeHtmlDocument2.ExecCommand(command, showUI, value);
		}

		/// <summary>Sets user input focus on the current document.</summary>
		// Token: 0x0600250B RID: 9483 RVA: 0x000B306A File Offset: 0x000B126A
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void Focus()
		{
			((UnsafeNativeMethods.IHTMLDocument4)this.NativeHtmlDocument2).Focus();
			((UnsafeNativeMethods.IHTMLDocument4)this.NativeHtmlDocument2).Focus();
		}

		/// <summary>Retrieves a single <see cref="T:System.Windows.Forms.HtmlElement" /> using the element's ID attribute as a search key.</summary>
		/// <param name="id">The ID attribute of the element to retrieve.</param>
		/// <returns>Returns the first object with the same ID attribute as the specified value, or <see langword="null" /> if the <paramref name="id" /> cannot be found. </returns>
		// Token: 0x0600250C RID: 9484 RVA: 0x000B308C File Offset: 0x000B128C
		public HtmlElement GetElementById(string id)
		{
			UnsafeNativeMethods.IHTMLElement elementById = ((UnsafeNativeMethods.IHTMLDocument3)this.NativeHtmlDocument2).GetElementById(id);
			if (elementById == null)
			{
				return null;
			}
			return new HtmlElement(this.ShimManager, elementById);
		}

		/// <summary>Retrieves the HTML element located at the specified client coordinates.</summary>
		/// <param name="point">The x,y position of the element on the screen, relative to the top-left corner of the document. </param>
		/// <returns>The <see cref="T:System.Windows.Forms.HtmlElement" /> at the specified screen location in the document.</returns>
		// Token: 0x0600250D RID: 9485 RVA: 0x000B30BC File Offset: 0x000B12BC
		public HtmlElement GetElementFromPoint(Point point)
		{
			UnsafeNativeMethods.IHTMLElement ihtmlelement = this.NativeHtmlDocument2.ElementFromPoint(point.X, point.Y);
			if (ihtmlelement == null)
			{
				return null;
			}
			return new HtmlElement(this.ShimManager, ihtmlelement);
		}

		/// <summary>Retrieve a collection of elements with the specified HTML tag.</summary>
		/// <param name="tagName">The name of the HTML tag for the <see cref="T:System.Windows.Forms.HtmlElement" /> objects you want to retrieve.</param>
		/// <returns>The collection of elements who tag name is equal to the <paramref name="tagName" /> argument.</returns>
		// Token: 0x0600250E RID: 9486 RVA: 0x000B30F4 File Offset: 0x000B12F4
		public HtmlElementCollection GetElementsByTagName(string tagName)
		{
			UnsafeNativeMethods.IHTMLElementCollection elementsByTagName = ((UnsafeNativeMethods.IHTMLDocument3)this.NativeHtmlDocument2).GetElementsByTagName(tagName);
			if (elementsByTagName == null)
			{
				return new HtmlElementCollection(this.ShimManager);
			}
			return new HtmlElementCollection(this.ShimManager, elementsByTagName);
		}

		/// <summary>Gets a new <see cref="T:System.Windows.Forms.HtmlDocument" /> to use with the <see cref="M:System.Windows.Forms.HtmlDocument.Write(System.String)" /> method.</summary>
		/// <param name="replaceInHistory">Whether the new window's navigation should replace the previous element in the navigation history of the DOM. </param>
		/// <returns>A new document for writing.</returns>
		// Token: 0x0600250F RID: 9487 RVA: 0x000B3130 File Offset: 0x000B1330
		public HtmlDocument OpenNew(bool replaceInHistory)
		{
			object name = replaceInHistory ? "replace" : "";
			object obj = null;
			object obj2 = this.NativeHtmlDocument2.Open("text/html", name, obj, obj);
			UnsafeNativeMethods.IHTMLDocument ihtmldocument = obj2 as UnsafeNativeMethods.IHTMLDocument;
			if (ihtmldocument == null)
			{
				return null;
			}
			return new HtmlDocument(this.ShimManager, ihtmldocument);
		}

		/// <summary>Creates a new <see langword="HtmlElement" /> of the specified HTML tag type. </summary>
		/// <param name="elementTag">The name of the HTML element to create. </param>
		/// <returns>A new element of the specified tag type. </returns>
		// Token: 0x06002510 RID: 9488 RVA: 0x000B317C File Offset: 0x000B137C
		public HtmlElement CreateElement(string elementTag)
		{
			UnsafeNativeMethods.IHTMLElement ihtmlelement = this.NativeHtmlDocument2.CreateElement(elementTag);
			if (ihtmlelement == null)
			{
				return null;
			}
			return new HtmlElement(this.ShimManager, ihtmlelement);
		}

		/// <summary>Executes an Active Scripting function defined in an HTML page.</summary>
		/// <param name="scriptName">The name of the script method to invoke.</param>
		/// <param name="args">The arguments to pass to the script method. </param>
		/// <returns>The object returned by the Active Scripting call. </returns>
		// Token: 0x06002511 RID: 9489 RVA: 0x000B31A8 File Offset: 0x000B13A8
		public object InvokeScript(string scriptName, object[] args)
		{
			object result = null;
			NativeMethods.tagDISPPARAMS tagDISPPARAMS = new NativeMethods.tagDISPPARAMS();
			tagDISPPARAMS.rgvarg = IntPtr.Zero;
			try
			{
				UnsafeNativeMethods.IDispatch dispatch = this.NativeHtmlDocument2.GetScript() as UnsafeNativeMethods.IDispatch;
				if (dispatch != null)
				{
					Guid empty = Guid.Empty;
					string[] rgszNames = new string[]
					{
						scriptName
					};
					int[] array = new int[]
					{
						-1
					};
					int idsOfNames = dispatch.GetIDsOfNames(ref empty, rgszNames, 1, SafeNativeMethods.GetThreadLCID(), array);
					if (NativeMethods.Succeeded(idsOfNames) && array[0] != -1)
					{
						if (args != null)
						{
							Array.Reverse(args);
						}
						tagDISPPARAMS.rgvarg = ((args == null) ? IntPtr.Zero : HtmlDocument.ArrayToVARIANTVector(args));
						tagDISPPARAMS.cArgs = ((args == null) ? 0 : args.Length);
						tagDISPPARAMS.rgdispidNamedArgs = IntPtr.Zero;
						tagDISPPARAMS.cNamedArgs = 0;
						object[] array2 = new object[1];
						if (dispatch.Invoke(array[0], ref empty, SafeNativeMethods.GetThreadLCID(), 1, tagDISPPARAMS, array2, new NativeMethods.tagEXCEPINFO(), null) == 0)
						{
							result = array2[0];
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
			finally
			{
				if (tagDISPPARAMS.rgvarg != IntPtr.Zero)
				{
					HtmlDocument.FreeVARIANTVector(tagDISPPARAMS.rgvarg, args.Length);
				}
			}
			return result;
		}

		/// <summary>Executes an Active Scripting function defined in an HTML page.</summary>
		/// <param name="scriptName">The name of the script method to invoke.</param>
		/// <returns>The object returned by the Active Scripting call. </returns>
		// Token: 0x06002512 RID: 9490 RVA: 0x000B32E0 File Offset: 0x000B14E0
		public object InvokeScript(string scriptName)
		{
			return this.InvokeScript(scriptName, null);
		}

		/// <summary>Adds an event handler for the named HTML DOM event.</summary>
		/// <param name="eventName">The name of the event you want to handle.</param>
		/// <param name="eventHandler">The managed code that handles the event. </param>
		// Token: 0x06002513 RID: 9491 RVA: 0x000B32EC File Offset: 0x000B14EC
		public void AttachEventHandler(string eventName, EventHandler eventHandler)
		{
			HtmlDocument.HtmlDocumentShim documentShim = this.DocumentShim;
			if (documentShim != null)
			{
				documentShim.AttachEventHandler(eventName, eventHandler);
			}
		}

		/// <summary>Removes an event handler from a named event on the HTML DOM. </summary>
		/// <param name="eventName">The name of the event you want to cease handling.</param>
		/// <param name="eventHandler">The managed code that handles the event.</param>
		// Token: 0x06002514 RID: 9492 RVA: 0x000B330C File Offset: 0x000B150C
		public void DetachEventHandler(string eventName, EventHandler eventHandler)
		{
			HtmlDocument.HtmlDocumentShim documentShim = this.DocumentShim;
			if (documentShim != null)
			{
				documentShim.DetachEventHandler(eventName, eventHandler);
			}
		}

		/// <summary>Occurs when the user clicks anywhere on the document.</summary>
		// Token: 0x140001AA RID: 426
		// (add) Token: 0x06002515 RID: 9493 RVA: 0x000B332B File Offset: 0x000B152B
		// (remove) Token: 0x06002516 RID: 9494 RVA: 0x000B333E File Offset: 0x000B153E
		public event HtmlElementEventHandler Click
		{
			add
			{
				this.DocumentShim.AddHandler(HtmlDocument.EventClick, value);
			}
			remove
			{
				this.DocumentShim.RemoveHandler(HtmlDocument.EventClick, value);
			}
		}

		/// <summary>Occurs when the user requests to display the document's context menu. </summary>
		// Token: 0x140001AB RID: 427
		// (add) Token: 0x06002517 RID: 9495 RVA: 0x000B3351 File Offset: 0x000B1551
		// (remove) Token: 0x06002518 RID: 9496 RVA: 0x000B3364 File Offset: 0x000B1564
		public event HtmlElementEventHandler ContextMenuShowing
		{
			add
			{
				this.DocumentShim.AddHandler(HtmlDocument.EventContextMenuShowing, value);
			}
			remove
			{
				this.DocumentShim.RemoveHandler(HtmlDocument.EventContextMenuShowing, value);
			}
		}

		/// <summary>Occurs before focus is given to the document.</summary>
		// Token: 0x140001AC RID: 428
		// (add) Token: 0x06002519 RID: 9497 RVA: 0x000B3377 File Offset: 0x000B1577
		// (remove) Token: 0x0600251A RID: 9498 RVA: 0x000B338A File Offset: 0x000B158A
		public event HtmlElementEventHandler Focusing
		{
			add
			{
				this.DocumentShim.AddHandler(HtmlDocument.EventFocusing, value);
			}
			remove
			{
				this.DocumentShim.RemoveHandler(HtmlDocument.EventFocusing, value);
			}
		}

		/// <summary>Occurs while focus is leaving a control.</summary>
		// Token: 0x140001AD RID: 429
		// (add) Token: 0x0600251B RID: 9499 RVA: 0x000B339D File Offset: 0x000B159D
		// (remove) Token: 0x0600251C RID: 9500 RVA: 0x000B33B0 File Offset: 0x000B15B0
		public event HtmlElementEventHandler LosingFocus
		{
			add
			{
				this.DocumentShim.AddHandler(HtmlDocument.EventLosingFocus, value);
			}
			remove
			{
				this.DocumentShim.RemoveHandler(HtmlDocument.EventLosingFocus, value);
			}
		}

		/// <summary>Occurs when the user clicks the left mouse button.</summary>
		// Token: 0x140001AE RID: 430
		// (add) Token: 0x0600251D RID: 9501 RVA: 0x000B33C3 File Offset: 0x000B15C3
		// (remove) Token: 0x0600251E RID: 9502 RVA: 0x000B33D6 File Offset: 0x000B15D6
		public event HtmlElementEventHandler MouseDown
		{
			add
			{
				this.DocumentShim.AddHandler(HtmlDocument.EventMouseDown, value);
			}
			remove
			{
				this.DocumentShim.RemoveHandler(HtmlDocument.EventMouseDown, value);
			}
		}

		/// <summary>Occurs when the mouse is no longer hovering over the document. </summary>
		// Token: 0x140001AF RID: 431
		// (add) Token: 0x0600251F RID: 9503 RVA: 0x000B33E9 File Offset: 0x000B15E9
		// (remove) Token: 0x06002520 RID: 9504 RVA: 0x000B33FC File Offset: 0x000B15FC
		public event HtmlElementEventHandler MouseLeave
		{
			add
			{
				this.DocumentShim.AddHandler(HtmlDocument.EventMouseLeave, value);
			}
			remove
			{
				this.DocumentShim.RemoveHandler(HtmlDocument.EventMouseLeave, value);
			}
		}

		/// <summary>Occurs when the mouse is moved over the document.</summary>
		// Token: 0x140001B0 RID: 432
		// (add) Token: 0x06002521 RID: 9505 RVA: 0x000B340F File Offset: 0x000B160F
		// (remove) Token: 0x06002522 RID: 9506 RVA: 0x000B3422 File Offset: 0x000B1622
		public event HtmlElementEventHandler MouseMove
		{
			add
			{
				this.DocumentShim.AddHandler(HtmlDocument.EventMouseMove, value);
			}
			remove
			{
				this.DocumentShim.RemoveHandler(HtmlDocument.EventMouseMove, value);
			}
		}

		/// <summary>Occurs when the mouse is moved over the document. </summary>
		// Token: 0x140001B1 RID: 433
		// (add) Token: 0x06002523 RID: 9507 RVA: 0x000B3435 File Offset: 0x000B1635
		// (remove) Token: 0x06002524 RID: 9508 RVA: 0x000B3448 File Offset: 0x000B1648
		public event HtmlElementEventHandler MouseOver
		{
			add
			{
				this.DocumentShim.AddHandler(HtmlDocument.EventMouseOver, value);
			}
			remove
			{
				this.DocumentShim.RemoveHandler(HtmlDocument.EventMouseOver, value);
			}
		}

		/// <summary>Occurs when the user releases the left mouse button.</summary>
		// Token: 0x140001B2 RID: 434
		// (add) Token: 0x06002525 RID: 9509 RVA: 0x000B345B File Offset: 0x000B165B
		// (remove) Token: 0x06002526 RID: 9510 RVA: 0x000B346E File Offset: 0x000B166E
		public event HtmlElementEventHandler MouseUp
		{
			add
			{
				this.DocumentShim.AddHandler(HtmlDocument.EventMouseUp, value);
			}
			remove
			{
				this.DocumentShim.RemoveHandler(HtmlDocument.EventMouseUp, value);
			}
		}

		/// <summary>Occurs when navigation to another Web page is halted.</summary>
		// Token: 0x140001B3 RID: 435
		// (add) Token: 0x06002527 RID: 9511 RVA: 0x000B3481 File Offset: 0x000B1681
		// (remove) Token: 0x06002528 RID: 9512 RVA: 0x000B3494 File Offset: 0x000B1694
		public event HtmlElementEventHandler Stop
		{
			add
			{
				this.DocumentShim.AddHandler(HtmlDocument.EventStop, value);
			}
			remove
			{
				this.DocumentShim.RemoveHandler(HtmlDocument.EventStop, value);
			}
		}

		// Token: 0x06002529 RID: 9513 RVA: 0x000B34A8 File Offset: 0x000B16A8
		internal unsafe static IntPtr ArrayToVARIANTVector(object[] args)
		{
			int num = args.Length;
			IntPtr intPtr = Marshal.AllocCoTaskMem(num * HtmlDocument.VariantSize);
			byte* ptr = (byte*)((void*)intPtr);
			for (int i = 0; i < num; i++)
			{
				Marshal.GetNativeVariantForObject(args[i], (IntPtr)((void*)(ptr + HtmlDocument.VariantSize * i)));
			}
			return intPtr;
		}

		// Token: 0x0600252A RID: 9514 RVA: 0x000B34F0 File Offset: 0x000B16F0
		internal unsafe static void FreeVARIANTVector(IntPtr mem, int len)
		{
			byte* ptr = (byte*)((void*)mem);
			for (int i = 0; i < len; i++)
			{
				SafeNativeMethods.VariantClear(new HandleRef(null, (IntPtr)((void*)(ptr + HtmlDocument.VariantSize * i))));
			}
			Marshal.FreeCoTaskMem(mem);
		}

		// Token: 0x0600252B RID: 9515 RVA: 0x000B3530 File Offset: 0x000B1730
		private Color ColorFromObject(object oColor)
		{
			try
			{
				if (oColor is string)
				{
					string text = oColor as string;
					int num = text.IndexOf('#');
					if (num >= 0)
					{
						string s = text.Substring(num + 1);
						return Color.FromArgb(255, Color.FromArgb(int.Parse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture)));
					}
					return Color.FromName(text);
				}
				else if (oColor is int)
				{
					return Color.FromArgb(255, Color.FromArgb((int)oColor));
				}
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
			return Color.Empty;
		}

		/// <summary>Returns a value that indicates whether the specified <see cref="T:System.Windows.Forms.HtmlDocument" /> instances represent the same value. </summary>
		/// <param name="left">The first instance to compare.</param>
		/// <param name="right">The second instance to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the specified instances are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600252C RID: 9516 RVA: 0x000B35D8 File Offset: 0x000B17D8
		public static bool operator ==(HtmlDocument left, HtmlDocument right)
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
				intPtr = Marshal.GetIUnknownForObject(left.NativeHtmlDocument2);
				intPtr2 = Marshal.GetIUnknownForObject(right.NativeHtmlDocument2);
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

		/// <summary>Returns a value that indicates whether the specified <see cref="T:System.Windows.Forms.HtmlDocument" /> instances do not represent the same value. </summary>
		/// <param name="left">The first instance to compare.</param>
		/// <param name="right">The second instance to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the specified instances are not equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600252D RID: 9517 RVA: 0x000B3660 File Offset: 0x000B1860
		public static bool operator !=(HtmlDocument left, HtmlDocument right)
		{
			return !(left == right);
		}

		/// <summary>Retrieves the hash code for this object.</summary>
		/// <returns>An <see cref="T:System.Int32" /> representing an in-memory hash of this object.</returns>
		// Token: 0x0600252E RID: 9518 RVA: 0x000B366C File Offset: 0x000B186C
		public override int GetHashCode()
		{
			if (this.htmlDocument2 != null)
			{
				return this.htmlDocument2.GetHashCode();
			}
			return 0;
		}

		/// <summary>Tests the object for equality against the current object.</summary>
		/// <param name="obj">The object to test.</param>
		/// <returns>
		///     <see langword="true" /> if the objects are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600252F RID: 9519 RVA: 0x000B3683 File Offset: 0x000B1883
		public override bool Equals(object obj)
		{
			return this == (HtmlDocument)obj;
		}

		// Token: 0x04000FEB RID: 4075
		internal static object EventClick = new object();

		// Token: 0x04000FEC RID: 4076
		internal static object EventContextMenuShowing = new object();

		// Token: 0x04000FED RID: 4077
		internal static object EventFocusing = new object();

		// Token: 0x04000FEE RID: 4078
		internal static object EventLosingFocus = new object();

		// Token: 0x04000FEF RID: 4079
		internal static object EventMouseDown = new object();

		// Token: 0x04000FF0 RID: 4080
		internal static object EventMouseLeave = new object();

		// Token: 0x04000FF1 RID: 4081
		internal static object EventMouseMove = new object();

		// Token: 0x04000FF2 RID: 4082
		internal static object EventMouseOver = new object();

		// Token: 0x04000FF3 RID: 4083
		internal static object EventMouseUp = new object();

		// Token: 0x04000FF4 RID: 4084
		internal static object EventStop = new object();

		// Token: 0x04000FF5 RID: 4085
		private UnsafeNativeMethods.IHTMLDocument2 htmlDocument2;

		// Token: 0x04000FF6 RID: 4086
		private HtmlShimManager shimManager;

		// Token: 0x04000FF7 RID: 4087
		private static readonly int VariantSize = (int)Marshal.OffsetOf(typeof(HtmlDocument.FindSizeOfVariant), "b");

		// Token: 0x020005E9 RID: 1513
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct FindSizeOfVariant
		{
			// Token: 0x040039AB RID: 14763
			[MarshalAs(UnmanagedType.Struct)]
			public object var;

			// Token: 0x040039AC RID: 14764
			public byte b;
		}

		// Token: 0x020005EA RID: 1514
		internal class HtmlDocumentShim : HtmlShim
		{
			// Token: 0x06005AF9 RID: 23289 RVA: 0x0017DD78 File Offset: 0x0017BF78
			internal HtmlDocumentShim(HtmlDocument htmlDocument)
			{
				this.htmlDocument = htmlDocument;
				if (this.htmlDocument != null)
				{
					HtmlWindow window = htmlDocument.Window;
					if (window != null)
					{
						this.associatedWindow = window.NativeHtmlWindow;
					}
				}
			}

			// Token: 0x170015E7 RID: 5607
			// (get) Token: 0x06005AFA RID: 23290 RVA: 0x0017DDBC File Offset: 0x0017BFBC
			public override UnsafeNativeMethods.IHTMLWindow2 AssociatedWindow
			{
				get
				{
					return this.associatedWindow;
				}
			}

			// Token: 0x170015E8 RID: 5608
			// (get) Token: 0x06005AFB RID: 23291 RVA: 0x0017DDC4 File Offset: 0x0017BFC4
			public UnsafeNativeMethods.IHTMLDocument2 NativeHtmlDocument2
			{
				get
				{
					return this.htmlDocument.NativeHtmlDocument2;
				}
			}

			// Token: 0x170015E9 RID: 5609
			// (get) Token: 0x06005AFC RID: 23292 RVA: 0x0017DDD1 File Offset: 0x0017BFD1
			internal HtmlDocument Document
			{
				get
				{
					return this.htmlDocument;
				}
			}

			// Token: 0x06005AFD RID: 23293 RVA: 0x0017DDDC File Offset: 0x0017BFDC
			public override void AttachEventHandler(string eventName, EventHandler eventHandler)
			{
				HtmlToClrEventProxy pdisp = base.AddEventProxy(eventName, eventHandler);
				bool flag = ((UnsafeNativeMethods.IHTMLDocument3)this.NativeHtmlDocument2).AttachEvent(eventName, pdisp);
			}

			// Token: 0x06005AFE RID: 23294 RVA: 0x0017DE08 File Offset: 0x0017C008
			public override void DetachEventHandler(string eventName, EventHandler eventHandler)
			{
				HtmlToClrEventProxy htmlToClrEventProxy = base.RemoveEventProxy(eventHandler);
				if (htmlToClrEventProxy != null)
				{
					((UnsafeNativeMethods.IHTMLDocument3)this.NativeHtmlDocument2).DetachEvent(eventName, htmlToClrEventProxy);
				}
			}

			// Token: 0x06005AFF RID: 23295 RVA: 0x0017DE34 File Offset: 0x0017C034
			public override void ConnectToEvents()
			{
				if (this.cookie == null || !this.cookie.Connected)
				{
					this.cookie = new AxHost.ConnectionPointCookie(this.NativeHtmlDocument2, new HtmlDocument.HTMLDocumentEvents2(this.htmlDocument), typeof(UnsafeNativeMethods.DHTMLDocumentEvents2), false);
					if (!this.cookie.Connected)
					{
						this.cookie = null;
					}
				}
			}

			// Token: 0x06005B00 RID: 23296 RVA: 0x0017DE91 File Offset: 0x0017C091
			public override void DisconnectFromEvents()
			{
				if (this.cookie != null)
				{
					this.cookie.Disconnect();
					this.cookie = null;
				}
			}

			// Token: 0x06005B01 RID: 23297 RVA: 0x0017DEAD File Offset: 0x0017C0AD
			protected override void Dispose(bool disposing)
			{
				base.Dispose(disposing);
				if (disposing)
				{
					if (this.htmlDocument != null)
					{
						Marshal.FinalReleaseComObject(this.htmlDocument.NativeHtmlDocument2);
					}
					this.htmlDocument = null;
				}
			}

			// Token: 0x06005B02 RID: 23298 RVA: 0x0017DDD1 File Offset: 0x0017BFD1
			protected override object GetEventSender()
			{
				return this.htmlDocument;
			}

			// Token: 0x040039AD RID: 14765
			private AxHost.ConnectionPointCookie cookie;

			// Token: 0x040039AE RID: 14766
			private HtmlDocument htmlDocument;

			// Token: 0x040039AF RID: 14767
			private UnsafeNativeMethods.IHTMLWindow2 associatedWindow;
		}

		// Token: 0x020005EB RID: 1515
		[ClassInterface(ClassInterfaceType.None)]
		private class HTMLDocumentEvents2 : StandardOleMarshalObject, UnsafeNativeMethods.DHTMLDocumentEvents2
		{
			// Token: 0x06005B03 RID: 23299 RVA: 0x0017DEDF File Offset: 0x0017C0DF
			public HTMLDocumentEvents2(HtmlDocument htmlDocument)
			{
				this.parent = htmlDocument;
			}

			// Token: 0x06005B04 RID: 23300 RVA: 0x0017DEEE File Offset: 0x0017C0EE
			private void FireEvent(object key, EventArgs e)
			{
				if (this.parent != null)
				{
					this.parent.DocumentShim.FireEvent(key, e);
				}
			}

			// Token: 0x06005B05 RID: 23301 RVA: 0x0017DF10 File Offset: 0x0017C110
			public bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlDocument.EventClick, htmlElementEventArgs);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B06 RID: 23302 RVA: 0x0017DF44 File Offset: 0x0017C144
			public bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlDocument.EventContextMenuShowing, htmlElementEventArgs);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B07 RID: 23303 RVA: 0x0017DF78 File Offset: 0x0017C178
			public void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs e = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlDocument.EventFocusing, e);
			}

			// Token: 0x06005B08 RID: 23304 RVA: 0x0017DFA4 File Offset: 0x0017C1A4
			public void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs e = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlDocument.EventLosingFocus, e);
			}

			// Token: 0x06005B09 RID: 23305 RVA: 0x0017DFD0 File Offset: 0x0017C1D0
			public void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs e = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlDocument.EventMouseMove, e);
			}

			// Token: 0x06005B0A RID: 23306 RVA: 0x0017DFFC File Offset: 0x0017C1FC
			public void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs e = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlDocument.EventMouseDown, e);
			}

			// Token: 0x06005B0B RID: 23307 RVA: 0x0017E028 File Offset: 0x0017C228
			public void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs e = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlDocument.EventMouseLeave, e);
			}

			// Token: 0x06005B0C RID: 23308 RVA: 0x0017E054 File Offset: 0x0017C254
			public void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs e = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlDocument.EventMouseOver, e);
			}

			// Token: 0x06005B0D RID: 23309 RVA: 0x0017E080 File Offset: 0x0017C280
			public void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs e = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlDocument.EventMouseUp, e);
			}

			// Token: 0x06005B0E RID: 23310 RVA: 0x0017E0AC File Offset: 0x0017C2AC
			public bool onstop(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlDocument.EventStop, htmlElementEventArgs);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B0F RID: 23311 RVA: 0x0017E0E0 File Offset: 0x0017C2E0
			public bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B10 RID: 23312 RVA: 0x0017E108 File Offset: 0x0017C308
			public bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B11 RID: 23313 RVA: 0x0000701A File Offset: 0x0000521A
			public void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B12 RID: 23314 RVA: 0x0000701A File Offset: 0x0000521A
			public void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B13 RID: 23315 RVA: 0x0017E130 File Offset: 0x0017C330
			public bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B14 RID: 23316 RVA: 0x0000701A File Offset: 0x0000521A
			public void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B15 RID: 23317 RVA: 0x0017E158 File Offset: 0x0017C358
			public bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B16 RID: 23318 RVA: 0x0000701A File Offset: 0x0000521A
			public void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B17 RID: 23319 RVA: 0x0017E180 File Offset: 0x0017C380
			public bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B18 RID: 23320 RVA: 0x0000701A File Offset: 0x0000521A
			public void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B19 RID: 23321 RVA: 0x0017E1A8 File Offset: 0x0017C3A8
			public bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B1A RID: 23322 RVA: 0x0017E1D0 File Offset: 0x0017C3D0
			public bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B1B RID: 23323 RVA: 0x0017E1F8 File Offset: 0x0017C3F8
			public bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B1C RID: 23324 RVA: 0x0000701A File Offset: 0x0000521A
			public void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B1D RID: 23325 RVA: 0x0000701A File Offset: 0x0000521A
			public void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B1E RID: 23326 RVA: 0x0000701A File Offset: 0x0000521A
			public void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B1F RID: 23327 RVA: 0x0000701A File Offset: 0x0000521A
			public void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B20 RID: 23328 RVA: 0x0000701A File Offset: 0x0000521A
			public void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B21 RID: 23329 RVA: 0x0000701A File Offset: 0x0000521A
			public void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B22 RID: 23330 RVA: 0x0000701A File Offset: 0x0000521A
			public void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B23 RID: 23331 RVA: 0x0000701A File Offset: 0x0000521A
			public void onbeforeeditfocus(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B24 RID: 23332 RVA: 0x0000701A File Offset: 0x0000521A
			public void onselectionchange(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B25 RID: 23333 RVA: 0x0017E220 File Offset: 0x0017C420
			public bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B26 RID: 23334 RVA: 0x0017E248 File Offset: 0x0017C448
			public bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B27 RID: 23335 RVA: 0x0000701A File Offset: 0x0000521A
			public void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B28 RID: 23336 RVA: 0x0000701A File Offset: 0x0000521A
			public void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B29 RID: 23337 RVA: 0x0017E270 File Offset: 0x0017C470
			public bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B2A RID: 23338 RVA: 0x0017E298 File Offset: 0x0017C498
			public bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x040039B0 RID: 14768
			private HtmlDocument parent;
		}
	}
}
