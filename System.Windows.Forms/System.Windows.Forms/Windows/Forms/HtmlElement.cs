using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Represents an HTML element inside of a Web page. </summary>
	// Token: 0x0200026A RID: 618
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class HtmlElement
	{
		// Token: 0x06002531 RID: 9521 RVA: 0x000B3723 File Offset: 0x000B1923
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		internal HtmlElement(HtmlShimManager shimManager, UnsafeNativeMethods.IHTMLElement element)
		{
			this.htmlElement = element;
			this.shimManager = shimManager;
		}

		/// <summary>Gets an <see cref="T:System.Windows.Forms.HtmlElementCollection" /> of all elements underneath the current element. </summary>
		/// <returns>A collection of all elements that are direct or indirect children of the current element. If the current element is a TABLE, for example, <see cref="P:System.Windows.Forms.HtmlElement.All" /> will return every TH, TR, and TD element within the table, as well as any other elements, such as DIV and SPAN elements, contained within the cells. </returns>
		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06002532 RID: 9522 RVA: 0x000B373C File Offset: 0x000B193C
		public HtmlElementCollection All
		{
			get
			{
				UnsafeNativeMethods.IHTMLElementCollection ihtmlelementCollection = this.NativeHtmlElement.GetAll() as UnsafeNativeMethods.IHTMLElementCollection;
				if (ihtmlelementCollection == null)
				{
					return new HtmlElementCollection(this.shimManager);
				}
				return new HtmlElementCollection(this.shimManager, ihtmlelementCollection);
			}
		}

		/// <summary>Gets an <see cref="T:System.Windows.Forms.HtmlElementCollection" /> of all children of the current element.</summary>
		/// <returns>A collection of all <see cref="T:System.Windows.Forms.HtmlElement" /> objects that have the current element as a parent.</returns>
		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06002533 RID: 9523 RVA: 0x000B3778 File Offset: 0x000B1978
		public HtmlElementCollection Children
		{
			get
			{
				UnsafeNativeMethods.IHTMLElementCollection ihtmlelementCollection = this.NativeHtmlElement.GetChildren() as UnsafeNativeMethods.IHTMLElementCollection;
				if (ihtmlelementCollection == null)
				{
					return new HtmlElementCollection(this.shimManager);
				}
				return new HtmlElementCollection(this.shimManager, ihtmlelementCollection);
			}
		}

		/// <summary>Gets a value indicating whether this element can have child elements.</summary>
		/// <returns>
		///     <see langword="true" /> if element can have child elements; otherwise, <see langword="false" />.</returns>
		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06002534 RID: 9524 RVA: 0x000B37B1 File Offset: 0x000B19B1
		public bool CanHaveChildren
		{
			get
			{
				return ((UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement).CanHaveChildren();
			}
		}

		/// <summary>Gets the bounds of the client area of the element in the HTML document.</summary>
		/// <returns>The client area occupied by the element, minus any area taken by borders and scroll bars. To obtain the position and dimensions of the element inclusive of its adornments, use <see cref="P:System.Windows.Forms.HtmlElement.OffsetRectangle" /> instead.</returns>
		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x06002535 RID: 9525 RVA: 0x000B37C4 File Offset: 0x000B19C4
		public Rectangle ClientRectangle
		{
			get
			{
				UnsafeNativeMethods.IHTMLElement2 ihtmlelement = (UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement;
				return new Rectangle(ihtmlelement.ClientLeft(), ihtmlelement.ClientTop(), ihtmlelement.ClientWidth(), ihtmlelement.ClientHeight());
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.HtmlDocument" /> to which this element belongs.</summary>
		/// <returns>The parent document of this element.</returns>
		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x06002536 RID: 9526 RVA: 0x000B37FC File Offset: 0x000B19FC
		public HtmlDocument Document
		{
			get
			{
				UnsafeNativeMethods.IHTMLDocument ihtmldocument = this.NativeHtmlElement.GetDocument() as UnsafeNativeMethods.IHTMLDocument;
				if (ihtmldocument == null)
				{
					return null;
				}
				return new HtmlDocument(this.shimManager, ihtmldocument);
			}
		}

		/// <summary>Gets or sets whether the user can input data into this element.</summary>
		/// <returns>
		///     <see langword="true" /> if the element allows user input; otherwise, <see langword="false" />.</returns>
		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x06002537 RID: 9527 RVA: 0x000B382B File Offset: 0x000B1A2B
		// (set) Token: 0x06002538 RID: 9528 RVA: 0x000B3840 File Offset: 0x000B1A40
		public bool Enabled
		{
			get
			{
				return !((UnsafeNativeMethods.IHTMLElement3)this.NativeHtmlElement).GetDisabled();
			}
			set
			{
				((UnsafeNativeMethods.IHTMLElement3)this.NativeHtmlElement).SetDisabled(!value);
			}
		}

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06002539 RID: 9529 RVA: 0x000B3858 File Offset: 0x000B1A58
		private HtmlElement.HtmlElementShim ElementShim
		{
			get
			{
				if (this.ShimManager != null)
				{
					HtmlElement.HtmlElementShim elementShim = this.ShimManager.GetElementShim(this);
					if (elementShim == null)
					{
						this.shimManager.AddElementShim(this);
						elementShim = this.ShimManager.GetElementShim(this);
					}
					return elementShim;
				}
				return null;
			}
		}

		/// <summary>Gets the next element below this element in the document tree. </summary>
		/// <returns>An <see cref="T:System.Windows.Forms.HtmlElement" /> representing the first element contained underneath the current element, in source order.</returns>
		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x0600253A RID: 9530 RVA: 0x000B389C File Offset: 0x000B1A9C
		public HtmlElement FirstChild
		{
			get
			{
				UnsafeNativeMethods.IHTMLElement ihtmlelement = null;
				UnsafeNativeMethods.IHTMLDOMNode ihtmldomnode = this.NativeHtmlElement as UnsafeNativeMethods.IHTMLDOMNode;
				if (ihtmldomnode != null)
				{
					ihtmlelement = (ihtmldomnode.FirstChild() as UnsafeNativeMethods.IHTMLElement);
				}
				if (ihtmlelement == null)
				{
					return null;
				}
				return new HtmlElement(this.shimManager, ihtmlelement);
			}
		}

		/// <summary>Gets or sets a label by which to identify the element.</summary>
		/// <returns>The unique identifier for the element. </returns>
		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x0600253B RID: 9531 RVA: 0x000B38D7 File Offset: 0x000B1AD7
		// (set) Token: 0x0600253C RID: 9532 RVA: 0x000B38E4 File Offset: 0x000B1AE4
		public string Id
		{
			get
			{
				return this.NativeHtmlElement.GetId();
			}
			set
			{
				this.NativeHtmlElement.SetId(value);
			}
		}

		/// <summary>Gets or sets the HTML markup underneath this element.</summary>
		/// <returns>The HTML markup that defines the child elements of the current element.</returns>
		/// <exception cref="T:System.NotSupportedException">Creating child elements on this element is not allowed. </exception>
		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x0600253D RID: 9533 RVA: 0x000B38F2 File Offset: 0x000B1AF2
		// (set) Token: 0x0600253E RID: 9534 RVA: 0x000B3900 File Offset: 0x000B1B00
		public string InnerHtml
		{
			get
			{
				return this.NativeHtmlElement.GetInnerHTML();
			}
			set
			{
				try
				{
					this.NativeHtmlElement.SetInnerHTML(value);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode == -2146827688)
					{
						throw new NotSupportedException(SR.GetString("HtmlElementPropertyNotSupported"));
					}
					throw;
				}
			}
		}

		/// <summary>Gets or sets the text assigned to the element.</summary>
		/// <returns>The element's text, absent any HTML markup. If the element contains child elements, only the text in those child elements will be preserved. </returns>
		/// <exception cref="T:System.NotSupportedException">The specified element cannot contain text (for example, an IMG element). </exception>
		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x0600253F RID: 9535 RVA: 0x000B394C File Offset: 0x000B1B4C
		// (set) Token: 0x06002540 RID: 9536 RVA: 0x000B395C File Offset: 0x000B1B5C
		public string InnerText
		{
			get
			{
				return this.NativeHtmlElement.GetInnerText();
			}
			set
			{
				try
				{
					this.NativeHtmlElement.SetInnerText(value);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode == -2146827688)
					{
						throw new NotSupportedException(SR.GetString("HtmlElementPropertyNotSupported"));
					}
					throw;
				}
			}
		}

		/// <summary>Gets or sets the name of the element. </summary>
		/// <returns>A <see cref="T:System.String" /> representing the element's name.</returns>
		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x06002541 RID: 9537 RVA: 0x000B39A8 File Offset: 0x000B1BA8
		// (set) Token: 0x06002542 RID: 9538 RVA: 0x000B39B5 File Offset: 0x000B1BB5
		public string Name
		{
			get
			{
				return this.GetAttribute("Name");
			}
			set
			{
				this.SetAttribute("Name", value);
			}
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x06002543 RID: 9539 RVA: 0x000B39C3 File Offset: 0x000B1BC3
		private UnsafeNativeMethods.IHTMLElement NativeHtmlElement
		{
			get
			{
				return this.htmlElement;
			}
		}

		/// <summary>Gets the next element at the same level as this element in the document tree. </summary>
		/// <returns>An <see cref="T:System.Windows.Forms.HtmlElement" /> representing the element to the right of the current element. </returns>
		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x06002544 RID: 9540 RVA: 0x000B39CC File Offset: 0x000B1BCC
		public HtmlElement NextSibling
		{
			get
			{
				UnsafeNativeMethods.IHTMLElement ihtmlelement = null;
				UnsafeNativeMethods.IHTMLDOMNode ihtmldomnode = this.NativeHtmlElement as UnsafeNativeMethods.IHTMLDOMNode;
				if (ihtmldomnode != null)
				{
					ihtmlelement = (ihtmldomnode.NextSibling() as UnsafeNativeMethods.IHTMLElement);
				}
				if (ihtmlelement == null)
				{
					return null;
				}
				return new HtmlElement(this.shimManager, ihtmlelement);
			}
		}

		/// <summary>Gets the location of an element relative to its parent.</summary>
		/// <returns>The x- and y-coordinate positions of the element, and its width and its height, in relation to its parent. If an element's parent is relatively or absolutely positioned, <see cref="P:System.Windows.Forms.HtmlElement.OffsetRectangle" /> will return the offset of the parent element. If the element itself is relatively positioned with respect to its parent, <see cref="P:System.Windows.Forms.HtmlElement.OffsetRectangle" /> will return the offset from its parent.</returns>
		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x06002545 RID: 9541 RVA: 0x000B3A07 File Offset: 0x000B1C07
		public Rectangle OffsetRectangle
		{
			get
			{
				return new Rectangle(this.NativeHtmlElement.GetOffsetLeft(), this.NativeHtmlElement.GetOffsetTop(), this.NativeHtmlElement.GetOffsetWidth(), this.NativeHtmlElement.GetOffsetHeight());
			}
		}

		/// <summary>Gets the element from which <see cref="P:System.Windows.Forms.HtmlElement.OffsetRectangle" /> is calculated.</summary>
		/// <returns>The element from which the offsets are calculated.If an element's parent or another element in the element's hierarchy uses relative or absolute positioning, <see langword="OffsetParent" /> will be the first relatively or absolutely positioned element in which the current element is nested. If none of the elements above the current element are absolutely or relatively positioned, <see langword="OffsetParent" /> will be the BODY tag of the document. </returns>
		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x06002546 RID: 9542 RVA: 0x000B3A3C File Offset: 0x000B1C3C
		public HtmlElement OffsetParent
		{
			get
			{
				UnsafeNativeMethods.IHTMLElement offsetParent = this.NativeHtmlElement.GetOffsetParent();
				if (offsetParent == null)
				{
					return null;
				}
				return new HtmlElement(this.shimManager, offsetParent);
			}
		}

		/// <summary>Gets or sets the current element's HTML code. </summary>
		/// <returns>The HTML code for the current element and its children.</returns>
		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06002547 RID: 9543 RVA: 0x000B3A66 File Offset: 0x000B1C66
		// (set) Token: 0x06002548 RID: 9544 RVA: 0x000B3A74 File Offset: 0x000B1C74
		public string OuterHtml
		{
			get
			{
				return this.NativeHtmlElement.GetOuterHTML();
			}
			set
			{
				try
				{
					this.NativeHtmlElement.SetOuterHTML(value);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode == -2146827688)
					{
						throw new NotSupportedException(SR.GetString("HtmlElementPropertyNotSupported"));
					}
					throw;
				}
			}
		}

		/// <summary>Gets or sets the current element's text. </summary>
		/// <returns>The text inside the current element, and in the element's children. </returns>
		/// <exception cref="T:System.NotSupportedException">You cannot set text outside of this element.</exception>
		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x06002549 RID: 9545 RVA: 0x000B3AC0 File Offset: 0x000B1CC0
		// (set) Token: 0x0600254A RID: 9546 RVA: 0x000B3AD0 File Offset: 0x000B1CD0
		public string OuterText
		{
			get
			{
				return this.NativeHtmlElement.GetOuterText();
			}
			set
			{
				try
				{
					this.NativeHtmlElement.SetOuterText(value);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode == -2146827688)
					{
						throw new NotSupportedException(SR.GetString("HtmlElementPropertyNotSupported"));
					}
					throw;
				}
			}
		}

		/// <summary>Gets the current element's parent element.</summary>
		/// <returns>The element above the current element in the HTML document's hierarchy.</returns>
		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x0600254B RID: 9547 RVA: 0x000B3B1C File Offset: 0x000B1D1C
		public HtmlElement Parent
		{
			get
			{
				UnsafeNativeMethods.IHTMLElement parentElement = this.NativeHtmlElement.GetParentElement();
				if (parentElement == null)
				{
					return null;
				}
				return new HtmlElement(this.shimManager, parentElement);
			}
		}

		/// <summary>Gets the dimensions of an element's scrollable region.</summary>
		/// <returns>The size and coordinate location of the scrollable area of an element.</returns>
		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x0600254C RID: 9548 RVA: 0x000B3B48 File Offset: 0x000B1D48
		public Rectangle ScrollRectangle
		{
			get
			{
				UnsafeNativeMethods.IHTMLElement2 ihtmlelement = (UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement;
				return new Rectangle(ihtmlelement.GetScrollLeft(), ihtmlelement.GetScrollTop(), ihtmlelement.GetScrollWidth(), ihtmlelement.GetScrollHeight());
			}
		}

		/// <summary>Gets or sets the distance between the edge of the element and the left edge of its content.</summary>
		/// <returns>The distance, in pixels, between the left edge of the element and the left edge of its content.</returns>
		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x0600254D RID: 9549 RVA: 0x000B3B7E File Offset: 0x000B1D7E
		// (set) Token: 0x0600254E RID: 9550 RVA: 0x000B3B90 File Offset: 0x000B1D90
		public int ScrollLeft
		{
			get
			{
				return ((UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement).GetScrollLeft();
			}
			set
			{
				((UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement).SetScrollLeft(value);
			}
		}

		/// <summary>Gets or sets the distance between the edge of the element and the top edge of its content.</summary>
		/// <returns>The distance, in pixels, between the top edge of the element and the top edge of its content.</returns>
		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x0600254F RID: 9551 RVA: 0x000B3BA3 File Offset: 0x000B1DA3
		// (set) Token: 0x06002550 RID: 9552 RVA: 0x000B3BB5 File Offset: 0x000B1DB5
		public int ScrollTop
		{
			get
			{
				return ((UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement).GetScrollTop();
			}
			set
			{
				((UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement).SetScrollTop(value);
			}
		}

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x06002551 RID: 9553 RVA: 0x000B3BC8 File Offset: 0x000B1DC8
		private HtmlShimManager ShimManager
		{
			get
			{
				return this.shimManager;
			}
		}

		/// <summary>Gets or sets a semicolon-delimited list of styles for the current element. </summary>
		/// <returns>A string consisting of all of the element's styles</returns>
		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x06002552 RID: 9554 RVA: 0x000B3BD0 File Offset: 0x000B1DD0
		// (set) Token: 0x06002553 RID: 9555 RVA: 0x000B3BE2 File Offset: 0x000B1DE2
		public string Style
		{
			get
			{
				return this.NativeHtmlElement.GetStyle().GetCssText();
			}
			set
			{
				this.NativeHtmlElement.GetStyle().SetCssText(value);
			}
		}

		/// <summary>Gets the name of the HTML tag.</summary>
		/// <returns>The name used to create this element using HTML markup.</returns>
		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06002554 RID: 9556 RVA: 0x000B3BF5 File Offset: 0x000B1DF5
		public string TagName
		{
			get
			{
				return this.NativeHtmlElement.GetTagName();
			}
		}

		/// <summary>Gets or sets the location of this element in the tab order.</summary>
		/// <returns>The numeric index of the element in the tab order.</returns>
		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x06002555 RID: 9557 RVA: 0x000B3C02 File Offset: 0x000B1E02
		// (set) Token: 0x06002556 RID: 9558 RVA: 0x000B3C14 File Offset: 0x000B1E14
		public short TabIndex
		{
			get
			{
				return ((UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement).GetTabIndex();
			}
			set
			{
				((UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement).SetTabIndex((int)value);
			}
		}

		/// <summary>Gets an unmanaged interface pointer for this element.</summary>
		/// <returns>The COM IUnknown pointer for the element, which you can cast to one of the HTML element interfaces, such as IHTMLElement.</returns>
		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x06002557 RID: 9559 RVA: 0x000B3C27 File Offset: 0x000B1E27
		public object DomElement
		{
			get
			{
				return this.NativeHtmlElement;
			}
		}

		/// <summary>Adds an element to another element's subtree.</summary>
		/// <param name="newElement">The <see cref="T:System.Windows.Forms.HtmlElement" /> to append to this location in the tree. </param>
		/// <returns>The element after it has been added to the tree. </returns>
		// Token: 0x06002558 RID: 9560 RVA: 0x000B3C2F File Offset: 0x000B1E2F
		public HtmlElement AppendChild(HtmlElement newElement)
		{
			return this.InsertAdjacentElement(HtmlElementInsertionOrientation.BeforeEnd, newElement);
		}

		/// <summary>Adds an event handler for a named event on the HTML Document Object Model (DOM).</summary>
		/// <param name="eventName">The name of the event you want to handle.</param>
		/// <param name="eventHandler">The managed code that handles the event.</param>
		// Token: 0x06002559 RID: 9561 RVA: 0x000B3C39 File Offset: 0x000B1E39
		public void AttachEventHandler(string eventName, EventHandler eventHandler)
		{
			this.ElementShim.AttachEventHandler(eventName, eventHandler);
		}

		/// <summary>Removes an event handler from a named event on the HTML Document Object Model (DOM).</summary>
		/// <param name="eventName">The name of the event you want to handle.</param>
		/// <param name="eventHandler">The managed code that handles the event.</param>
		// Token: 0x0600255A RID: 9562 RVA: 0x000B3C48 File Offset: 0x000B1E48
		public void DetachEventHandler(string eventName, EventHandler eventHandler)
		{
			this.ElementShim.DetachEventHandler(eventName, eventHandler);
		}

		/// <summary>Puts user input focus on the current element.</summary>
		// Token: 0x0600255B RID: 9563 RVA: 0x000B3C58 File Offset: 0x000B1E58
		public void Focus()
		{
			try
			{
				((UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement).Focus();
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode == -2146826178)
				{
					throw new NotSupportedException(SR.GetString("HtmlElementMethodNotSupported"));
				}
				throw;
			}
		}

		/// <summary>Retrieves the value of the named attribute on the element.</summary>
		/// <param name="attributeName">The name of the attribute. This argument is case-insensitive.</param>
		/// <returns>The value of this attribute on the element, as a <see cref="T:System.String" /> value. If the specified attribute does not exist on this element, returns an empty string.</returns>
		// Token: 0x0600255C RID: 9564 RVA: 0x000B3CA8 File Offset: 0x000B1EA8
		public string GetAttribute(string attributeName)
		{
			object attribute = this.NativeHtmlElement.GetAttribute(attributeName, 0);
			if (attribute != null)
			{
				return attribute.ToString();
			}
			return "";
		}

		/// <summary>Retrieves a collection of elements represented in HTML by the specified HTML tag.</summary>
		/// <param name="tagName">The name of the tag whose <see cref="T:System.Windows.Forms.HtmlElement" /> objects you wish to retrieve.</param>
		/// <returns>An <see cref="T:System.Windows.Forms.HtmlElementCollection" /> containing all elements whose HTML tag name is equal to <paramref name="tagName" />.</returns>
		// Token: 0x0600255D RID: 9565 RVA: 0x000B3CD4 File Offset: 0x000B1ED4
		public HtmlElementCollection GetElementsByTagName(string tagName)
		{
			UnsafeNativeMethods.IHTMLElementCollection elementsByTagName = ((UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement).GetElementsByTagName(tagName);
			if (elementsByTagName == null)
			{
				return new HtmlElementCollection(this.shimManager);
			}
			return new HtmlElementCollection(this.shimManager, elementsByTagName);
		}

		/// <summary>Insert a new element into the Document Object Model (DOM).</summary>
		/// <param name="orient">Where to insert this element in relation to the current element.</param>
		/// <param name="newElement">The new element to insert.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.HtmlElement" /> that was just inserted. If insertion failed, this will return <see langword="null" />.</returns>
		// Token: 0x0600255E RID: 9566 RVA: 0x000B3D10 File Offset: 0x000B1F10
		public HtmlElement InsertAdjacentElement(HtmlElementInsertionOrientation orient, HtmlElement newElement)
		{
			UnsafeNativeMethods.IHTMLElement ihtmlelement = ((UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement).InsertAdjacentElement(orient.ToString(), (UnsafeNativeMethods.IHTMLElement)newElement.DomElement);
			if (ihtmlelement == null)
			{
				return null;
			}
			return new HtmlElement(this.shimManager, ihtmlelement);
		}

		/// <summary>Executes an unexposed method on the underlying DOM element of this element.</summary>
		/// <param name="methodName">The name of the property or method to invoke. </param>
		/// <returns>The element returned by this method, represented as an <see cref="T:System.Object" />. If this <see cref="T:System.Object" /> is another HTML element, and you have a reference to the unmanaged MSHTML library added to your project, you can cast it to its appropriate unmanaged interface.</returns>
		// Token: 0x0600255F RID: 9567 RVA: 0x000B3D57 File Offset: 0x000B1F57
		public object InvokeMember(string methodName)
		{
			return this.InvokeMember(methodName, null);
		}

		/// <summary>Executes a function defined in the current HTML page by a scripting language.</summary>
		/// <param name="methodName">The name of the property or method to invoke.</param>
		/// <param name="parameter">A list of parameters to pass. </param>
		/// <returns>The element returned by the function, represented as an <see cref="T:System.Object" />. If this <see cref="T:System.Object" /> is another HTML element, and you have a reference to the unmanaged MSHTML library added to your project, you can cast it to its appropriate unmanaged interface.</returns>
		// Token: 0x06002560 RID: 9568 RVA: 0x000B3D64 File Offset: 0x000B1F64
		public object InvokeMember(string methodName, params object[] parameter)
		{
			object result = null;
			NativeMethods.tagDISPPARAMS tagDISPPARAMS = new NativeMethods.tagDISPPARAMS();
			tagDISPPARAMS.rgvarg = IntPtr.Zero;
			try
			{
				UnsafeNativeMethods.IDispatch dispatch = this.NativeHtmlElement as UnsafeNativeMethods.IDispatch;
				if (dispatch != null)
				{
					Guid empty = Guid.Empty;
					string[] rgszNames = new string[]
					{
						methodName
					};
					int[] array = new int[]
					{
						-1
					};
					int idsOfNames = dispatch.GetIDsOfNames(ref empty, rgszNames, 1, SafeNativeMethods.GetThreadLCID(), array);
					if (NativeMethods.Succeeded(idsOfNames) && array[0] != -1)
					{
						if (parameter != null)
						{
							Array.Reverse(parameter);
						}
						tagDISPPARAMS.rgvarg = ((parameter == null) ? IntPtr.Zero : HtmlDocument.ArrayToVARIANTVector(parameter));
						tagDISPPARAMS.cArgs = ((parameter == null) ? 0 : parameter.Length);
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
					HtmlDocument.FreeVARIANTVector(tagDISPPARAMS.rgvarg, parameter.Length);
				}
			}
			return result;
		}

		/// <summary>Removes focus from the current element, if that element has focus. </summary>
		// Token: 0x06002561 RID: 9569 RVA: 0x000B3E98 File Offset: 0x000B2098
		public void RemoveFocus()
		{
			((UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement).Blur();
		}

		/// <summary>Causes the named event to call all registered event handlers. </summary>
		/// <param name="eventName">The name of the event to raise. </param>
		// Token: 0x06002562 RID: 9570 RVA: 0x000B3EAA File Offset: 0x000B20AA
		public void RaiseEvent(string eventName)
		{
			((UnsafeNativeMethods.IHTMLElement3)this.NativeHtmlElement).FireEvent(eventName, IntPtr.Zero);
		}

		/// <summary>Scrolls through the document containing this element until the top or bottom edge of this element is aligned with the document's window. </summary>
		/// <param name="alignWithTop">If <see langword="true" />, the top of the object will be displayed at the top of the window. If <see langword="false" />, the bottom of the object will be displayed at the bottom of the window.</param>
		// Token: 0x06002563 RID: 9571 RVA: 0x000B3EC3 File Offset: 0x000B20C3
		public void ScrollIntoView(bool alignWithTop)
		{
			this.NativeHtmlElement.ScrollIntoView(alignWithTop);
		}

		/// <summary>Sets the value of the named attribute on the element.</summary>
		/// <param name="attributeName">The name of the attribute to set.</param>
		/// <param name="value">The new value of this attribute. </param>
		// Token: 0x06002564 RID: 9572 RVA: 0x000B3ED8 File Offset: 0x000B20D8
		public void SetAttribute(string attributeName, string value)
		{
			try
			{
				this.NativeHtmlElement.SetAttribute(attributeName, value, 0);
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode == -2147352567)
				{
					throw new NotSupportedException(SR.GetString("HtmlElementAttributeNotSupported"));
				}
				throw;
			}
		}

		/// <summary>Occurs when the user clicks on the element with the left mouse button. </summary>
		// Token: 0x140001B4 RID: 436
		// (add) Token: 0x06002565 RID: 9573 RVA: 0x000B3F28 File Offset: 0x000B2128
		// (remove) Token: 0x06002566 RID: 9574 RVA: 0x000B3F3B File Offset: 0x000B213B
		public event HtmlElementEventHandler Click
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventClick, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventClick, value);
			}
		}

		/// <summary>Occurs when the user clicks the left mouse button over an element twice, in rapid succession.</summary>
		// Token: 0x140001B5 RID: 437
		// (add) Token: 0x06002567 RID: 9575 RVA: 0x000B3F4E File Offset: 0x000B214E
		// (remove) Token: 0x06002568 RID: 9576 RVA: 0x000B3F61 File Offset: 0x000B2161
		public event HtmlElementEventHandler DoubleClick
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventDoubleClick, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventDoubleClick, value);
			}
		}

		/// <summary>Occurs when the user drags text to various locations. </summary>
		// Token: 0x140001B6 RID: 438
		// (add) Token: 0x06002569 RID: 9577 RVA: 0x000B3F74 File Offset: 0x000B2174
		// (remove) Token: 0x0600256A RID: 9578 RVA: 0x000B3F87 File Offset: 0x000B2187
		public event HtmlElementEventHandler Drag
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventDrag, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventDrag, value);
			}
		}

		/// <summary>Occurs when a user finishes a drag operation.</summary>
		// Token: 0x140001B7 RID: 439
		// (add) Token: 0x0600256B RID: 9579 RVA: 0x000B3F9A File Offset: 0x000B219A
		// (remove) Token: 0x0600256C RID: 9580 RVA: 0x000B3FAD File Offset: 0x000B21AD
		public event HtmlElementEventHandler DragEnd
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventDragEnd, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventDragEnd, value);
			}
		}

		/// <summary>Occurs when the user is no longer dragging an item over this element. </summary>
		// Token: 0x140001B8 RID: 440
		// (add) Token: 0x0600256D RID: 9581 RVA: 0x000B3FC0 File Offset: 0x000B21C0
		// (remove) Token: 0x0600256E RID: 9582 RVA: 0x000B3FD3 File Offset: 0x000B21D3
		public event HtmlElementEventHandler DragLeave
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventDragLeave, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventDragLeave, value);
			}
		}

		/// <summary>Occurs when the user drags text over the element.</summary>
		// Token: 0x140001B9 RID: 441
		// (add) Token: 0x0600256F RID: 9583 RVA: 0x000B3FE6 File Offset: 0x000B21E6
		// (remove) Token: 0x06002570 RID: 9584 RVA: 0x000B3FF9 File Offset: 0x000B21F9
		public event HtmlElementEventHandler DragOver
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventDragOver, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventDragOver, value);
			}
		}

		/// <summary>Occurs when the element first receives user input focus. </summary>
		// Token: 0x140001BA RID: 442
		// (add) Token: 0x06002571 RID: 9585 RVA: 0x000B400C File Offset: 0x000B220C
		// (remove) Token: 0x06002572 RID: 9586 RVA: 0x000B401F File Offset: 0x000B221F
		public event HtmlElementEventHandler Focusing
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventFocusing, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventFocusing, value);
			}
		}

		/// <summary>Occurs when the element has received user input focus.</summary>
		// Token: 0x140001BB RID: 443
		// (add) Token: 0x06002573 RID: 9587 RVA: 0x000B4032 File Offset: 0x000B2232
		// (remove) Token: 0x06002574 RID: 9588 RVA: 0x000B4045 File Offset: 0x000B2245
		public event HtmlElementEventHandler GotFocus
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventGotFocus, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventGotFocus, value);
			}
		}

		/// <summary>Occurs when the element is losing user input focus. </summary>
		// Token: 0x140001BC RID: 444
		// (add) Token: 0x06002575 RID: 9589 RVA: 0x000B4058 File Offset: 0x000B2258
		// (remove) Token: 0x06002576 RID: 9590 RVA: 0x000B406B File Offset: 0x000B226B
		public event HtmlElementEventHandler LosingFocus
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventLosingFocus, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventLosingFocus, value);
			}
		}

		/// <summary>Occurs when the element has lost user input focus. </summary>
		// Token: 0x140001BD RID: 445
		// (add) Token: 0x06002577 RID: 9591 RVA: 0x000B407E File Offset: 0x000B227E
		// (remove) Token: 0x06002578 RID: 9592 RVA: 0x000B4091 File Offset: 0x000B2291
		public event HtmlElementEventHandler LostFocus
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventLostFocus, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventLostFocus, value);
			}
		}

		/// <summary>Occurs when the user presses a key on the keyboard.</summary>
		// Token: 0x140001BE RID: 446
		// (add) Token: 0x06002579 RID: 9593 RVA: 0x000B40A4 File Offset: 0x000B22A4
		// (remove) Token: 0x0600257A RID: 9594 RVA: 0x000B40B7 File Offset: 0x000B22B7
		public event HtmlElementEventHandler KeyDown
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventKeyDown, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventKeyDown, value);
			}
		}

		/// <summary>Occurs when the user presses and releases a key on the keyboard.</summary>
		// Token: 0x140001BF RID: 447
		// (add) Token: 0x0600257B RID: 9595 RVA: 0x000B40CA File Offset: 0x000B22CA
		// (remove) Token: 0x0600257C RID: 9596 RVA: 0x000B40DD File Offset: 0x000B22DD
		public event HtmlElementEventHandler KeyPress
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventKeyPress, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventKeyPress, value);
			}
		}

		/// <summary>Occurs when the user releases a key on the keyboard.</summary>
		// Token: 0x140001C0 RID: 448
		// (add) Token: 0x0600257D RID: 9597 RVA: 0x000B40F0 File Offset: 0x000B22F0
		// (remove) Token: 0x0600257E RID: 9598 RVA: 0x000B4103 File Offset: 0x000B2303
		public event HtmlElementEventHandler KeyUp
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventKeyUp, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventKeyUp, value);
			}
		}

		/// <summary>Occurs when the user moves the mouse cursor across the element.</summary>
		// Token: 0x140001C1 RID: 449
		// (add) Token: 0x0600257F RID: 9599 RVA: 0x000B4116 File Offset: 0x000B2316
		// (remove) Token: 0x06002580 RID: 9600 RVA: 0x000B4129 File Offset: 0x000B2329
		public event HtmlElementEventHandler MouseMove
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventMouseMove, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventMouseMove, value);
			}
		}

		/// <summary>Occurs when the user presses a mouse button.</summary>
		// Token: 0x140001C2 RID: 450
		// (add) Token: 0x06002581 RID: 9601 RVA: 0x000B413C File Offset: 0x000B233C
		// (remove) Token: 0x06002582 RID: 9602 RVA: 0x000B414F File Offset: 0x000B234F
		public event HtmlElementEventHandler MouseDown
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventMouseDown, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventMouseDown, value);
			}
		}

		/// <summary>Occurs when the mouse cursor enters the bounds of the element.</summary>
		// Token: 0x140001C3 RID: 451
		// (add) Token: 0x06002583 RID: 9603 RVA: 0x000B4162 File Offset: 0x000B2362
		// (remove) Token: 0x06002584 RID: 9604 RVA: 0x000B4175 File Offset: 0x000B2375
		public event HtmlElementEventHandler MouseOver
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventMouseOver, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventMouseOver, value);
			}
		}

		/// <summary>Occurs when the user releases a mouse button.</summary>
		// Token: 0x140001C4 RID: 452
		// (add) Token: 0x06002585 RID: 9605 RVA: 0x000B4188 File Offset: 0x000B2388
		// (remove) Token: 0x06002586 RID: 9606 RVA: 0x000B419B File Offset: 0x000B239B
		public event HtmlElementEventHandler MouseUp
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventMouseUp, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventMouseUp, value);
			}
		}

		/// <summary>Occurs when the user first moves the mouse cursor over the current element. </summary>
		// Token: 0x140001C5 RID: 453
		// (add) Token: 0x06002587 RID: 9607 RVA: 0x000B41AE File Offset: 0x000B23AE
		// (remove) Token: 0x06002588 RID: 9608 RVA: 0x000B41C1 File Offset: 0x000B23C1
		public event HtmlElementEventHandler MouseEnter
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventMouseEnter, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventMouseEnter, value);
			}
		}

		/// <summary>Occurs when the user moves the mouse cursor off of the current element. </summary>
		// Token: 0x140001C6 RID: 454
		// (add) Token: 0x06002589 RID: 9609 RVA: 0x000B41D4 File Offset: 0x000B23D4
		// (remove) Token: 0x0600258A RID: 9610 RVA: 0x000B41E7 File Offset: 0x000B23E7
		public event HtmlElementEventHandler MouseLeave
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventMouseLeave, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventMouseLeave, value);
			}
		}

		/// <summary>Compares two elements for equality.</summary>
		/// <param name="left">The first <see cref="T:System.Windows.Forms.HtmlElement" />.</param>
		/// <param name="right">The second <see cref="T:System.Windows.Forms.HtmlElement" />.</param>
		/// <returns>
		///     <see langword="true" /> if both parameters are <see langword="null" />, or if both elements have the same underlying COM interface; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600258B RID: 9611 RVA: 0x000B41FC File Offset: 0x000B23FC
		public static bool operator ==(HtmlElement left, HtmlElement right)
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
				intPtr = Marshal.GetIUnknownForObject(left.NativeHtmlElement);
				intPtr2 = Marshal.GetIUnknownForObject(right.NativeHtmlElement);
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

		/// <summary>Compares two <see cref="T:System.Windows.Forms.HtmlElement" /> objects for inequality.</summary>
		/// <param name="left">The first <see cref="T:System.Windows.Forms.HtmlElement" />.</param>
		/// <param name="right">The second <see cref="T:System.Windows.Forms.HtmlElement" />.</param>
		/// <returns>
		///     <see langword="true" /> is only one element is <see langword="null" />, or the two objects are not equal; otherwise, <see langword="false" />. </returns>
		// Token: 0x0600258C RID: 9612 RVA: 0x000B4284 File Offset: 0x000B2484
		public static bool operator !=(HtmlElement left, HtmlElement right)
		{
			return !(left == right);
		}

		/// <summary>Serves as a hash function for a particular type.</summary>
		/// <returns>A hash code for the current <see cref="T:System.Object" />.</returns>
		// Token: 0x0600258D RID: 9613 RVA: 0x000B4290 File Offset: 0x000B2490
		public override int GetHashCode()
		{
			if (this.htmlElement != null)
			{
				return this.htmlElement.GetHashCode();
			}
			return 0;
		}

		/// <summary>Tests if the supplied object is equal to the current element.</summary>
		/// <param name="obj">The object to test for equality.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="obj" /> is an <see cref="T:System.Windows.Forms.HtmlElement" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600258E RID: 9614 RVA: 0x000B42A7 File Offset: 0x000B24A7
		public override bool Equals(object obj)
		{
			return this == obj as HtmlElement;
		}

		// Token: 0x04000FF8 RID: 4088
		internal static readonly object EventClick = new object();

		// Token: 0x04000FF9 RID: 4089
		internal static readonly object EventDoubleClick = new object();

		// Token: 0x04000FFA RID: 4090
		internal static readonly object EventDrag = new object();

		// Token: 0x04000FFB RID: 4091
		internal static readonly object EventDragEnd = new object();

		// Token: 0x04000FFC RID: 4092
		internal static readonly object EventDragLeave = new object();

		// Token: 0x04000FFD RID: 4093
		internal static readonly object EventDragOver = new object();

		// Token: 0x04000FFE RID: 4094
		internal static readonly object EventFocusing = new object();

		// Token: 0x04000FFF RID: 4095
		internal static readonly object EventGotFocus = new object();

		// Token: 0x04001000 RID: 4096
		internal static readonly object EventLosingFocus = new object();

		// Token: 0x04001001 RID: 4097
		internal static readonly object EventLostFocus = new object();

		// Token: 0x04001002 RID: 4098
		internal static readonly object EventKeyDown = new object();

		// Token: 0x04001003 RID: 4099
		internal static readonly object EventKeyPress = new object();

		// Token: 0x04001004 RID: 4100
		internal static readonly object EventKeyUp = new object();

		// Token: 0x04001005 RID: 4101
		internal static readonly object EventMouseDown = new object();

		// Token: 0x04001006 RID: 4102
		internal static readonly object EventMouseEnter = new object();

		// Token: 0x04001007 RID: 4103
		internal static readonly object EventMouseLeave = new object();

		// Token: 0x04001008 RID: 4104
		internal static readonly object EventMouseMove = new object();

		// Token: 0x04001009 RID: 4105
		internal static readonly object EventMouseOver = new object();

		// Token: 0x0400100A RID: 4106
		internal static readonly object EventMouseUp = new object();

		// Token: 0x0400100B RID: 4107
		private UnsafeNativeMethods.IHTMLElement htmlElement;

		// Token: 0x0400100C RID: 4108
		private HtmlShimManager shimManager;

		// Token: 0x020005EC RID: 1516
		[ClassInterface(ClassInterfaceType.None)]
		private class HTMLElementEvents2 : StandardOleMarshalObject, UnsafeNativeMethods.DHTMLElementEvents2, UnsafeNativeMethods.DHTMLAnchorEvents2, UnsafeNativeMethods.DHTMLAreaEvents2, UnsafeNativeMethods.DHTMLButtonElementEvents2, UnsafeNativeMethods.DHTMLControlElementEvents2, UnsafeNativeMethods.DHTMLFormElementEvents2, UnsafeNativeMethods.DHTMLFrameSiteEvents2, UnsafeNativeMethods.DHTMLImgEvents2, UnsafeNativeMethods.DHTMLInputFileElementEvents2, UnsafeNativeMethods.DHTMLInputImageEvents2, UnsafeNativeMethods.DHTMLInputTextElementEvents2, UnsafeNativeMethods.DHTMLLabelEvents2, UnsafeNativeMethods.DHTMLLinkElementEvents2, UnsafeNativeMethods.DHTMLMapEvents2, UnsafeNativeMethods.DHTMLMarqueeElementEvents2, UnsafeNativeMethods.DHTMLOptionButtonElementEvents2, UnsafeNativeMethods.DHTMLSelectElementEvents2, UnsafeNativeMethods.DHTMLStyleElementEvents2, UnsafeNativeMethods.DHTMLTableEvents2, UnsafeNativeMethods.DHTMLTextContainerEvents2, UnsafeNativeMethods.DHTMLScriptEvents2
		{
			// Token: 0x06005B2B RID: 23339 RVA: 0x0017E2BD File Offset: 0x0017C4BD
			public HTMLElementEvents2(HtmlElement htmlElement)
			{
				this.parent = htmlElement;
			}

			// Token: 0x06005B2C RID: 23340 RVA: 0x0017E2CC File Offset: 0x0017C4CC
			private void FireEvent(object key, EventArgs e)
			{
				if (this.parent != null)
				{
					this.parent.ElementShim.FireEvent(key, e);
				}
			}

			// Token: 0x06005B2D RID: 23341 RVA: 0x0017E2F0 File Offset: 0x0017C4F0
			public bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventClick, htmlElementEventArgs);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B2E RID: 23342 RVA: 0x0017E324 File Offset: 0x0017C524
			public bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventDoubleClick, htmlElementEventArgs);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B2F RID: 23343 RVA: 0x0017E358 File Offset: 0x0017C558
			public bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventKeyPress, htmlElementEventArgs);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B30 RID: 23344 RVA: 0x0017E38C File Offset: 0x0017C58C
			public void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs e = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventKeyDown, e);
			}

			// Token: 0x06005B31 RID: 23345 RVA: 0x0017E3B8 File Offset: 0x0017C5B8
			public void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs e = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventKeyUp, e);
			}

			// Token: 0x06005B32 RID: 23346 RVA: 0x0017E3E4 File Offset: 0x0017C5E4
			public void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs e = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventMouseOver, e);
			}

			// Token: 0x06005B33 RID: 23347 RVA: 0x0017E410 File Offset: 0x0017C610
			public void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs e = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventMouseMove, e);
			}

			// Token: 0x06005B34 RID: 23348 RVA: 0x0017E43C File Offset: 0x0017C63C
			public void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs e = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventMouseDown, e);
			}

			// Token: 0x06005B35 RID: 23349 RVA: 0x0017E468 File Offset: 0x0017C668
			public void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs e = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventMouseUp, e);
			}

			// Token: 0x06005B36 RID: 23350 RVA: 0x0017E494 File Offset: 0x0017C694
			public void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs e = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventMouseEnter, e);
			}

			// Token: 0x06005B37 RID: 23351 RVA: 0x0017E4C0 File Offset: 0x0017C6C0
			public void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs e = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventMouseLeave, e);
			}

			// Token: 0x06005B38 RID: 23352 RVA: 0x0017E4EC File Offset: 0x0017C6EC
			public bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B39 RID: 23353 RVA: 0x0017E514 File Offset: 0x0017C714
			public void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs e = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventGotFocus, e);
			}

			// Token: 0x06005B3A RID: 23354 RVA: 0x0017E540 File Offset: 0x0017C740
			public bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventDrag, htmlElementEventArgs);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B3B RID: 23355 RVA: 0x0017E574 File Offset: 0x0017C774
			public void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs e = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventDragEnd, e);
			}

			// Token: 0x06005B3C RID: 23356 RVA: 0x0017E5A0 File Offset: 0x0017C7A0
			public void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs e = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventDragLeave, e);
			}

			// Token: 0x06005B3D RID: 23357 RVA: 0x0017E5CC File Offset: 0x0017C7CC
			public bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventDragOver, htmlElementEventArgs);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B3E RID: 23358 RVA: 0x0017E600 File Offset: 0x0017C800
			public void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs e = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventFocusing, e);
			}

			// Token: 0x06005B3F RID: 23359 RVA: 0x0017E62C File Offset: 0x0017C82C
			public void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs e = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventLosingFocus, e);
			}

			// Token: 0x06005B40 RID: 23360 RVA: 0x0017E658 File Offset: 0x0017C858
			public void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs e = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventLostFocus, e);
			}

			// Token: 0x06005B41 RID: 23361 RVA: 0x0000701A File Offset: 0x0000521A
			public void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B42 RID: 23362 RVA: 0x0017E684 File Offset: 0x0017C884
			public bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B43 RID: 23363 RVA: 0x0017E6AC File Offset: 0x0017C8AC
			public bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B44 RID: 23364 RVA: 0x0000701A File Offset: 0x0000521A
			public void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B45 RID: 23365 RVA: 0x0017E6D4 File Offset: 0x0017C8D4
			public bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B46 RID: 23366 RVA: 0x0000701A File Offset: 0x0000521A
			public void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B47 RID: 23367 RVA: 0x0017E6FC File Offset: 0x0017C8FC
			public bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B48 RID: 23368 RVA: 0x0017E724 File Offset: 0x0017C924
			public bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B49 RID: 23369 RVA: 0x0000701A File Offset: 0x0000521A
			public void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B4A RID: 23370 RVA: 0x0017E74C File Offset: 0x0017C94C
			public bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B4B RID: 23371 RVA: 0x0000701A File Offset: 0x0000521A
			public void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B4C RID: 23372 RVA: 0x0000701A File Offset: 0x0000521A
			public void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B4D RID: 23373 RVA: 0x0000701A File Offset: 0x0000521A
			public void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B4E RID: 23374 RVA: 0x0000701A File Offset: 0x0000521A
			public void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B4F RID: 23375 RVA: 0x0000701A File Offset: 0x0000521A
			public void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B50 RID: 23376 RVA: 0x0000701A File Offset: 0x0000521A
			public void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B51 RID: 23377 RVA: 0x0000701A File Offset: 0x0000521A
			public void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B52 RID: 23378 RVA: 0x0000701A File Offset: 0x0000521A
			public void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B53 RID: 23379 RVA: 0x0017E774 File Offset: 0x0017C974
			public bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B54 RID: 23380 RVA: 0x0017E79C File Offset: 0x0017C99C
			public bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B55 RID: 23381 RVA: 0x0017E7C4 File Offset: 0x0017C9C4
			public bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B56 RID: 23382 RVA: 0x0017E7EC File Offset: 0x0017C9EC
			public bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B57 RID: 23383 RVA: 0x0017E814 File Offset: 0x0017CA14
			public bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B58 RID: 23384 RVA: 0x0017E83C File Offset: 0x0017CA3C
			public bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B59 RID: 23385 RVA: 0x0017E864 File Offset: 0x0017CA64
			public bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B5A RID: 23386 RVA: 0x0017E88C File Offset: 0x0017CA8C
			public bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B5B RID: 23387 RVA: 0x0017E8B4 File Offset: 0x0017CAB4
			public bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B5C RID: 23388 RVA: 0x0000701A File Offset: 0x0000521A
			public void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B5D RID: 23389 RVA: 0x0000701A File Offset: 0x0000521A
			public void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B5E RID: 23390 RVA: 0x0000701A File Offset: 0x0000521A
			public void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B5F RID: 23391 RVA: 0x0000701A File Offset: 0x0000521A
			public void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B60 RID: 23392 RVA: 0x0000701A File Offset: 0x0000521A
			public void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B61 RID: 23393 RVA: 0x0000701A File Offset: 0x0000521A
			public void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B62 RID: 23394 RVA: 0x0000701A File Offset: 0x0000521A
			public void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B63 RID: 23395 RVA: 0x0000701A File Offset: 0x0000521A
			public void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B64 RID: 23396 RVA: 0x0017E8DC File Offset: 0x0017CADC
			public bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B65 RID: 23397 RVA: 0x0017E904 File Offset: 0x0017CB04
			public bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B66 RID: 23398 RVA: 0x0000701A File Offset: 0x0000521A
			public void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B67 RID: 23399 RVA: 0x0017E92C File Offset: 0x0017CB2C
			public bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B68 RID: 23400 RVA: 0x0017E954 File Offset: 0x0017CB54
			public bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B69 RID: 23401 RVA: 0x0000701A File Offset: 0x0000521A
			public void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B6A RID: 23402 RVA: 0x0017E97C File Offset: 0x0017CB7C
			public bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B6B RID: 23403 RVA: 0x0017E9A4 File Offset: 0x0017CBA4
			public bool onchange(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B6C RID: 23404 RVA: 0x0000701A File Offset: 0x0000521A
			public void onselect(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B6D RID: 23405 RVA: 0x0000701A File Offset: 0x0000521A
			public void onload(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B6E RID: 23406 RVA: 0x0000701A File Offset: 0x0000521A
			public void onerror(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B6F RID: 23407 RVA: 0x0000701A File Offset: 0x0000521A
			public void onabort(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B70 RID: 23408 RVA: 0x0017E9CC File Offset: 0x0017CBCC
			public bool onsubmit(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B71 RID: 23409 RVA: 0x0017E9F4 File Offset: 0x0017CBF4
			public bool onreset(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06005B72 RID: 23410 RVA: 0x0000701A File Offset: 0x0000521A
			public void onchange_void(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B73 RID: 23411 RVA: 0x0000701A File Offset: 0x0000521A
			public void onbounce(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B74 RID: 23412 RVA: 0x0000701A File Offset: 0x0000521A
			public void onfinish(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06005B75 RID: 23413 RVA: 0x0000701A File Offset: 0x0000521A
			public void onstart(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x040039B1 RID: 14769
			private HtmlElement parent;
		}

		// Token: 0x020005ED RID: 1517
		internal class HtmlElementShim : HtmlShim
		{
			// Token: 0x06005B76 RID: 23414 RVA: 0x0017EA1C File Offset: 0x0017CC1C
			public HtmlElementShim(HtmlElement element)
			{
				this.htmlElement = element;
				if (this.htmlElement != null)
				{
					HtmlDocument document = this.htmlElement.Document;
					if (document != null)
					{
						HtmlWindow window = document.Window;
						if (window != null)
						{
							this.associatedWindow = window.NativeHtmlWindow;
						}
					}
				}
			}

			// Token: 0x170015EA RID: 5610
			// (get) Token: 0x06005B77 RID: 23415 RVA: 0x0017EA75 File Offset: 0x0017CC75
			public UnsafeNativeMethods.IHTMLElement NativeHtmlElement
			{
				get
				{
					return this.htmlElement.NativeHtmlElement;
				}
			}

			// Token: 0x170015EB RID: 5611
			// (get) Token: 0x06005B78 RID: 23416 RVA: 0x0017EA82 File Offset: 0x0017CC82
			internal HtmlElement Element
			{
				get
				{
					return this.htmlElement;
				}
			}

			// Token: 0x170015EC RID: 5612
			// (get) Token: 0x06005B79 RID: 23417 RVA: 0x0017EA8A File Offset: 0x0017CC8A
			public override UnsafeNativeMethods.IHTMLWindow2 AssociatedWindow
			{
				get
				{
					return this.associatedWindow;
				}
			}

			// Token: 0x06005B7A RID: 23418 RVA: 0x0017EA94 File Offset: 0x0017CC94
			public override void AttachEventHandler(string eventName, EventHandler eventHandler)
			{
				HtmlToClrEventProxy pdisp = base.AddEventProxy(eventName, eventHandler);
				bool flag = ((UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement).AttachEvent(eventName, pdisp);
			}

			// Token: 0x06005B7B RID: 23419 RVA: 0x0017EAC0 File Offset: 0x0017CCC0
			public override void ConnectToEvents()
			{
				if (this.cookie == null || !this.cookie.Connected)
				{
					int num = 0;
					while (num < HtmlElement.HtmlElementShim.dispInterfaceTypes.Length && this.cookie == null)
					{
						this.cookie = new AxHost.ConnectionPointCookie(this.NativeHtmlElement, new HtmlElement.HTMLElementEvents2(this.htmlElement), HtmlElement.HtmlElementShim.dispInterfaceTypes[num], false);
						if (!this.cookie.Connected)
						{
							this.cookie = null;
						}
						num++;
					}
				}
			}

			// Token: 0x06005B7C RID: 23420 RVA: 0x0017EB34 File Offset: 0x0017CD34
			public override void DetachEventHandler(string eventName, EventHandler eventHandler)
			{
				HtmlToClrEventProxy htmlToClrEventProxy = base.RemoveEventProxy(eventHandler);
				if (htmlToClrEventProxy != null)
				{
					((UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement).DetachEvent(eventName, htmlToClrEventProxy);
				}
			}

			// Token: 0x06005B7D RID: 23421 RVA: 0x0017EB5E File Offset: 0x0017CD5E
			public override void DisconnectFromEvents()
			{
				if (this.cookie != null)
				{
					this.cookie.Disconnect();
					this.cookie = null;
				}
			}

			// Token: 0x06005B7E RID: 23422 RVA: 0x0017EB7A File Offset: 0x0017CD7A
			protected override void Dispose(bool disposing)
			{
				base.Dispose(disposing);
				if (this.htmlElement != null)
				{
					Marshal.FinalReleaseComObject(this.htmlElement.NativeHtmlElement);
				}
				this.htmlElement = null;
			}

			// Token: 0x06005B7F RID: 23423 RVA: 0x0017EA82 File Offset: 0x0017CC82
			protected override object GetEventSender()
			{
				return this.htmlElement;
			}

			// Token: 0x040039B2 RID: 14770
			private static Type[] dispInterfaceTypes = new Type[]
			{
				typeof(UnsafeNativeMethods.DHTMLElementEvents2),
				typeof(UnsafeNativeMethods.DHTMLAnchorEvents2),
				typeof(UnsafeNativeMethods.DHTMLAreaEvents2),
				typeof(UnsafeNativeMethods.DHTMLButtonElementEvents2),
				typeof(UnsafeNativeMethods.DHTMLControlElementEvents2),
				typeof(UnsafeNativeMethods.DHTMLFormElementEvents2),
				typeof(UnsafeNativeMethods.DHTMLFrameSiteEvents2),
				typeof(UnsafeNativeMethods.DHTMLImgEvents2),
				typeof(UnsafeNativeMethods.DHTMLInputFileElementEvents2),
				typeof(UnsafeNativeMethods.DHTMLInputImageEvents2),
				typeof(UnsafeNativeMethods.DHTMLInputTextElementEvents2),
				typeof(UnsafeNativeMethods.DHTMLLabelEvents2),
				typeof(UnsafeNativeMethods.DHTMLLinkElementEvents2),
				typeof(UnsafeNativeMethods.DHTMLMapEvents2),
				typeof(UnsafeNativeMethods.DHTMLMarqueeElementEvents2),
				typeof(UnsafeNativeMethods.DHTMLOptionButtonElementEvents2),
				typeof(UnsafeNativeMethods.DHTMLSelectElementEvents2),
				typeof(UnsafeNativeMethods.DHTMLStyleElementEvents2),
				typeof(UnsafeNativeMethods.DHTMLTableEvents2),
				typeof(UnsafeNativeMethods.DHTMLTextContainerEvents2),
				typeof(UnsafeNativeMethods.DHTMLScriptEvents2)
			};

			// Token: 0x040039B3 RID: 14771
			private AxHost.ConnectionPointCookie cookie;

			// Token: 0x040039B4 RID: 14772
			private HtmlElement htmlElement;

			// Token: 0x040039B5 RID: 14773
			private UnsafeNativeMethods.IHTMLWindow2 associatedWindow;
		}
	}
}
