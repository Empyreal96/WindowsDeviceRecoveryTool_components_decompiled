using System;
using System.Collections;
using System.ComponentModel;
using System.IO.Packaging;
using System.Text;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;
using MS.Internal;
using MS.Internal.Utility;

namespace System.Windows.Documents
{
	/// <summary>Provides the content for a high fidelity, fixed-format page. </summary>
	// Token: 0x02000355 RID: 853
	[ContentProperty("Children")]
	public sealed class FixedPage : FrameworkElement, IAddChildInternal, IAddChild, IFixedNavigate, IUriContext
	{
		// Token: 0x06002D62 RID: 11618 RVA: 0x000CCA70 File Offset: 0x000CAC70
		static FixedPage()
		{
			FrameworkPropertyMetadata frameworkPropertyMetadata = new FrameworkPropertyMetadata(FlowDirection.LeftToRight, FrameworkPropertyMetadataOptions.AffectsParentArrange);
			frameworkPropertyMetadata.CoerceValueCallback = new CoerceValueCallback(FixedPage.CoerceFlowDirection);
			FrameworkElement.FlowDirectionProperty.OverrideMetadata(typeof(FixedPage), frameworkPropertyMetadata);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.FixedPage" /> class. </summary>
		// Token: 0x06002D63 RID: 11619 RVA: 0x000CCC80 File Offset: 0x000CAE80
		public FixedPage()
		{
			this.Init();
		}

		// Token: 0x06002D64 RID: 11620 RVA: 0x000CCC8E File Offset: 0x000CAE8E
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new FixedPageAutomationPeer(this);
		}

		// Token: 0x06002D65 RID: 11621 RVA: 0x00002137 File Offset: 0x00000337
		protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
		{
		}

		// Token: 0x06002D66 RID: 11622 RVA: 0x000CCC98 File Offset: 0x000CAE98
		protected override void OnRender(DrawingContext dc)
		{
			Brush background = this.Background;
			if (background != null)
			{
				dc.DrawRectangle(background, null, new Rect(0.0, 0.0, base.RenderSize.Width, base.RenderSize.Height));
			}
		}

		/// <summary>Adds a child object. </summary>
		/// <param name="value">The child object to add.</param>
		// Token: 0x06002D67 RID: 11623 RVA: 0x000CCCEC File Offset: 0x000CAEEC
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			UIElement uielement = value as UIElement;
			if (uielement == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(UIElement)
				}), "value");
			}
			this.Children.Add(uielement);
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x06002D68 RID: 11624 RVA: 0x0000B31C File Offset: 0x0000951C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		/// <summary>Returns the distance between the left side of an element and the left side of its parent <see cref="T:System.Windows.Controls.Canvas" />.</summary>
		/// <param name="element">The element from which to get the left offset.</param>
		/// <returns>The distance between the right side of an element and the right side of its parent canvas.</returns>
		// Token: 0x06002D69 RID: 11625 RVA: 0x000CCD4F File Offset: 0x000CAF4F
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		[AttachedPropertyBrowsableForChildren]
		public static double GetLeft(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(FixedPage.LeftProperty);
		}

		/// <summary>Sets the distance between the left side of an element and the left side of its parent <see cref="T:System.Windows.Controls.Canvas" />.</summary>
		/// <param name="element">The element on which to set the left offset.</param>
		/// <param name="length">The new distance between the left side of the element and the left side of its parent canvas.</param>
		// Token: 0x06002D6A RID: 11626 RVA: 0x000CCD6F File Offset: 0x000CAF6F
		public static void SetLeft(UIElement element, double length)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(FixedPage.LeftProperty, length);
		}

		/// <summary>Returns the distance between the top of an element and the top of its parent <see cref="T:System.Windows.Controls.Canvas" />.</summary>
		/// <param name="element">The element from which to get the top offset.</param>
		/// <returns>The distance between the top of an element and the top of its parent canvas.</returns>
		// Token: 0x06002D6B RID: 11627 RVA: 0x000CCD90 File Offset: 0x000CAF90
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		[AttachedPropertyBrowsableForChildren]
		public static double GetTop(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(FixedPage.TopProperty);
		}

		/// <summary>Sets the distance between the top of an element and the top of its parent <see cref="T:System.Windows.Controls.Canvas" />.</summary>
		/// <param name="element">The element on which to set the top offset.</param>
		/// <param name="length">The new distance between the top side of the element and the top side of its parent canvas.</param>
		// Token: 0x06002D6C RID: 11628 RVA: 0x000CCDB0 File Offset: 0x000CAFB0
		public static void SetTop(UIElement element, double length)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(FixedPage.TopProperty, length);
		}

		/// <summary>Returns the distance between the right side of an element and the right side of its parent <see cref="T:System.Windows.Controls.Canvas" />.</summary>
		/// <param name="element">The element from which to get the right offset.</param>
		/// <returns>The distance between the right side of an element and the right side of its parent canvas.</returns>
		// Token: 0x06002D6D RID: 11629 RVA: 0x000CCDD1 File Offset: 0x000CAFD1
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		[AttachedPropertyBrowsableForChildren]
		public static double GetRight(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(FixedPage.RightProperty);
		}

		/// <summary>Sets the distance between the right side of an element and the right side of its parent <see cref="T:System.Windows.Controls.Canvas" />.</summary>
		/// <param name="element">The element on which to set the right offset.</param>
		/// <param name="length">The new distance between the right side of the element and the right side of its parent canvas.</param>
		// Token: 0x06002D6E RID: 11630 RVA: 0x000CCDF1 File Offset: 0x000CAFF1
		public static void SetRight(UIElement element, double length)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(FixedPage.RightProperty, length);
		}

		/// <summary>Returns the distance between the bottom of an element and the bottom of its parent <see cref="T:System.Windows.Controls.Canvas" />.</summary>
		/// <param name="element">The element from which to get the bottom offset.</param>
		/// <returns>The distance between the bottom of an element and the bottom of its parent canvas.</returns>
		// Token: 0x06002D6F RID: 11631 RVA: 0x000CCE12 File Offset: 0x000CB012
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		[AttachedPropertyBrowsableForChildren]
		public static double GetBottom(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(FixedPage.BottomProperty);
		}

		/// <summary>Sets the distance between the bottom of an element and the bottom of its parent <see cref="T:System.Windows.Controls.Canvas" />.</summary>
		/// <param name="element">The element on which to set the bottom offset.</param>
		/// <param name="length">The new distance between the bottom side of the element and the bottom side of its parent canvas.</param>
		// Token: 0x06002D70 RID: 11632 RVA: 0x000CCE32 File Offset: 0x000CB032
		public static void SetBottom(UIElement element, double length)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(FixedPage.BottomProperty, length);
		}

		/// <summary>Returns the <see cref="P:System.Windows.Documents.FixedPage.NavigateUri" /> property for a given element.</summary>
		/// <param name="element">The element from which to get the property.</param>
		/// <returns>The <see cref="T:System.Uri" /> of <paramref name="element" />.</returns>
		// Token: 0x06002D71 RID: 11633 RVA: 0x000CCE53 File Offset: 0x000CB053
		[AttachedPropertyBrowsableForChildren]
		public static Uri GetNavigateUri(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (Uri)element.GetValue(FixedPage.NavigateUriProperty);
		}

		/// <summary>Sets the uniform resource identifier (URI) to navigate to when a hyperlink is clicked.</summary>
		/// <param name="element">The element on which to set the URI offset.</param>
		/// <param name="uri">The URI to navigate to when a hyperlink is clicked.</param>
		// Token: 0x06002D72 RID: 11634 RVA: 0x000CCE73 File Offset: 0x000CB073
		public static void SetNavigateUri(UIElement element, Uri uri)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(FixedPage.NavigateUriProperty, uri);
		}

		/// <summary>Gets or sets the base URI of the current application context. </summary>
		/// <returns>The base URI of the application context.</returns>
		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x06002D73 RID: 11635 RVA: 0x000C216F File Offset: 0x000C036F
		// (set) Token: 0x06002D74 RID: 11636 RVA: 0x000C2181 File Offset: 0x000C0381
		Uri IUriContext.BaseUri
		{
			get
			{
				return (Uri)base.GetValue(BaseUriHelper.BaseUriProperty);
			}
			set
			{
				base.SetValue(BaseUriHelper.BaseUriProperty, value);
			}
		}

		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x06002D75 RID: 11637 RVA: 0x000CCE8F File Offset: 0x000CB08F
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				return this.Children.GetEnumerator();
			}
		}

		/// <summary>Gets a collection of the <see cref="T:System.Windows.Documents.FixedPage" /> child elements. </summary>
		/// <returns>The <see cref="T:System.Windows.Controls.UIElementCollection" /> of the child elements.</returns>
		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x06002D76 RID: 11638 RVA: 0x000CCE9C File Offset: 0x000CB09C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public UIElementCollection Children
		{
			get
			{
				if (this._uiElementCollection == null)
				{
					this._uiElementCollection = this.CreateUIElementCollection(this);
				}
				return this._uiElementCollection;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Printing.PrintTicket" /> that is associated with the page.  </summary>
		/// <returns>The <see cref="T:System.Printing.PrintTicket" /> for the page.</returns>
		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x06002D77 RID: 11639 RVA: 0x000CCEB9 File Offset: 0x000CB0B9
		// (set) Token: 0x06002D78 RID: 11640 RVA: 0x000CCEC6 File Offset: 0x000CB0C6
		public object PrintTicket
		{
			get
			{
				return base.GetValue(FixedPage.PrintTicketProperty);
			}
			set
			{
				base.SetValue(FixedPage.PrintTicketProperty, value);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Media.Brush" /> used for rendering the page background.   </summary>
		/// <returns>The brush for rendering the page background.</returns>
		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x06002D79 RID: 11641 RVA: 0x000CCED4 File Offset: 0x000CB0D4
		// (set) Token: 0x06002D7A RID: 11642 RVA: 0x000CCEE6 File Offset: 0x000CB0E6
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(FixedPage.BackgroundProperty);
			}
			set
			{
				base.SetValue(FixedPage.BackgroundProperty, value);
			}
		}

		/// <summary>Gets or sets the bounding rectangle of the content area; that is, the area of the page within the margins, if any.  </summary>
		/// <returns>The <see cref="T:System.Windows.Rect" /> that defines the content area.</returns>
		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x06002D7B RID: 11643 RVA: 0x000CCEF4 File Offset: 0x000CB0F4
		// (set) Token: 0x06002D7C RID: 11644 RVA: 0x000CCF06 File Offset: 0x000CB106
		public Rect ContentBox
		{
			get
			{
				return (Rect)base.GetValue(FixedPage.ContentBoxProperty);
			}
			set
			{
				base.SetValue(FixedPage.ContentBoxProperty, value);
			}
		}

		/// <summary>Gets or sets a rectangle defining the overflow area for bleeds, registration marks, and crop marks.  </summary>
		/// <returns>The <see cref="T:System.Windows.Rect" /> defining the overflow area.</returns>
		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x06002D7D RID: 11645 RVA: 0x000CCF19 File Offset: 0x000CB119
		// (set) Token: 0x06002D7E RID: 11646 RVA: 0x000CCF2B File Offset: 0x000CB12B
		public Rect BleedBox
		{
			get
			{
				return (Rect)base.GetValue(FixedPage.BleedBoxProperty);
			}
			set
			{
				base.SetValue(FixedPage.BleedBoxProperty, value);
			}
		}

		// Token: 0x06002D7F RID: 11647 RVA: 0x000CCF40 File Offset: 0x000CB140
		protected internal override void OnVisualParentChanged(DependencyObject oldParent)
		{
			base.OnVisualParentChanged(oldParent);
			if (oldParent == null)
			{
				HighlightVisual highlightVisual = HighlightVisual.GetHighlightVisual(this);
				AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
				if (highlightVisual == null && adornerLayer != null)
				{
					PageContent pageContent = LogicalTreeHelper.GetParent(this) as PageContent;
					if (pageContent != null)
					{
						FixedDocument fixedDocument = LogicalTreeHelper.GetParent(pageContent) as FixedDocument;
						if (fixedDocument != null && adornerLayer != null)
						{
							int zOrder = 1073741823;
							adornerLayer.Add(new HighlightVisual(fixedDocument, this), zOrder);
						}
					}
				}
			}
		}

		// Token: 0x06002D80 RID: 11648 RVA: 0x000CCFA2 File Offset: 0x000CB1A2
		private static object CoerceFlowDirection(DependencyObject page, object flowDirection)
		{
			return FlowDirection.LeftToRight;
		}

		// Token: 0x06002D81 RID: 11649 RVA: 0x000CCFAC File Offset: 0x000CB1AC
		internal static bool CanNavigateToUri(Uri uri)
		{
			return !uri.IsAbsoluteUri || uri.IsUnc || uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeMailto || (uri.Scheme == PackUriHelper.UriSchemePack && !string.IsNullOrEmpty(uri.Fragment)) || SecurityHelper.CallerHasWebPermission(uri);
		}

		// Token: 0x06002D82 RID: 11650 RVA: 0x000CD028 File Offset: 0x000CB228
		internal static Uri GetLinkUri(IInputElement element, Uri inputUri)
		{
			DependencyObject dependencyObject = element as DependencyObject;
			if (inputUri != null && (FixedPage.CanNavigateToUri(inputUri) || (inputUri.Scheme == PackUriHelper.UriSchemePack && !string.IsNullOrEmpty(inputUri.Fragment))))
			{
				Uri uri = inputUri;
				if (!inputUri.IsAbsoluteUri)
				{
					uri = new Uri(new Uri("http://microsoft.com/"), inputUri);
				}
				string fragment = uri.Fragment;
				int num = (fragment == null) ? 0 : fragment.Length;
				if (num != 0)
				{
					string text = inputUri.ToString();
					string uriString = text.Substring(0, text.IndexOf('#'));
					inputUri = new Uri(uriString, UriKind.RelativeOrAbsolute);
					if (!inputUri.IsAbsoluteUri)
					{
						string startPartUriString = FixedPage.GetStartPartUriString(dependencyObject);
						if (startPartUriString != null)
						{
							inputUri = new Uri(startPartUriString, UriKind.RelativeOrAbsolute);
						}
					}
				}
				Uri baseUri = BaseUriHelper.GetBaseUri(dependencyObject);
				Uri uri2 = BindUriHelper.GetUriToNavigate(dependencyObject, baseUri, inputUri);
				if (num != 0)
				{
					StringBuilder stringBuilder = new StringBuilder(uri2.ToString());
					stringBuilder.Append(fragment);
					uri2 = new Uri(stringBuilder.ToString(), UriKind.RelativeOrAbsolute);
				}
				return uri2;
			}
			return null;
		}

		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x06002D83 RID: 11651 RVA: 0x000CD12A File Offset: 0x000CB32A
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._uiElementCollection == null)
				{
					return 0;
				}
				return this._uiElementCollection.Count;
			}
		}

		// Token: 0x06002D84 RID: 11652 RVA: 0x000CD141 File Offset: 0x000CB341
		protected override Visual GetVisualChild(int index)
		{
			if (this._uiElementCollection == null)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._uiElementCollection[index];
		}

		// Token: 0x06002D85 RID: 11653 RVA: 0x000CD172 File Offset: 0x000CB372
		private UIElementCollection CreateUIElementCollection(FrameworkElement logicalParent)
		{
			return new UIElementCollection(this, logicalParent);
		}

		// Token: 0x06002D86 RID: 11654 RVA: 0x000CD17C File Offset: 0x000CB37C
		protected override Size MeasureOverride(Size constraint)
		{
			Size availableSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
			foreach (object obj in this.Children)
			{
				UIElement uielement = (UIElement)obj;
				uielement.Measure(availableSize);
			}
			return default(Size);
		}

		// Token: 0x06002D87 RID: 11655 RVA: 0x000CD1F8 File Offset: 0x000CB3F8
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			foreach (object obj in this.Children)
			{
				UIElement uielement = (UIElement)obj;
				double x = 0.0;
				double y = 0.0;
				double left = FixedPage.GetLeft(uielement);
				if (!DoubleUtil.IsNaN(left))
				{
					x = left;
				}
				else
				{
					double right = FixedPage.GetRight(uielement);
					if (!DoubleUtil.IsNaN(right))
					{
						x = arrangeSize.Width - uielement.DesiredSize.Width - right;
					}
				}
				double top = FixedPage.GetTop(uielement);
				if (!DoubleUtil.IsNaN(top))
				{
					y = top;
				}
				else
				{
					double bottom = FixedPage.GetBottom(uielement);
					if (!DoubleUtil.IsNaN(bottom))
					{
						y = arrangeSize.Height - uielement.DesiredSize.Height - bottom;
					}
				}
				uielement.Arrange(new Rect(new Point(x, y), uielement.DesiredSize));
			}
			return arrangeSize;
		}

		// Token: 0x06002D88 RID: 11656 RVA: 0x000CD304 File Offset: 0x000CB504
		void IFixedNavigate.NavigateAsync(string elementID)
		{
			FixedHyperLink.NavigateToElement(this, elementID);
		}

		// Token: 0x06002D89 RID: 11657 RVA: 0x000CD310 File Offset: 0x000CB510
		UIElement IFixedNavigate.FindElementByID(string elementID, out FixedPage rootFixedPage)
		{
			UIElement result = null;
			rootFixedPage = this;
			UIElementCollection children = this.Children;
			int i = 0;
			int count = children.Count;
			while (i < count)
			{
				UIElement logicalTreeNode = children[i];
				DependencyObject dependencyObject = LogicalTreeHelper.FindLogicalNode(logicalTreeNode, elementID);
				if (dependencyObject != null)
				{
					result = (dependencyObject as UIElement);
					break;
				}
				i++;
			}
			return result;
		}

		// Token: 0x06002D8A RID: 11658 RVA: 0x000CD360 File Offset: 0x000CB560
		internal FixedNode CreateFixedNode(int pageIndex, UIElement e)
		{
			return this._CreateFixedNode(pageIndex, e);
		}

		// Token: 0x06002D8B RID: 11659 RVA: 0x000CD36A File Offset: 0x000CB56A
		internal Glyphs GetGlyphsElement(FixedNode node)
		{
			return this.GetElement(node) as Glyphs;
		}

		// Token: 0x06002D8C RID: 11660 RVA: 0x000CD378 File Offset: 0x000CB578
		internal DependencyObject GetElement(FixedNode node)
		{
			int num = node[1];
			if (num < 0 || num > this.Children.Count)
			{
				return null;
			}
			DependencyObject dependencyObject = this.Children[num];
			for (int i = 2; i <= node.ChildLevels; i++)
			{
				num = node[i];
				if (dependencyObject is Canvas)
				{
					dependencyObject = ((Canvas)dependencyObject).Children[num];
				}
				else
				{
					IEnumerable children = LogicalTreeHelper.GetChildren(dependencyObject);
					if (children == null)
					{
						return null;
					}
					int num2 = -1;
					IEnumerator enumerator = children.GetEnumerator();
					while (enumerator.MoveNext())
					{
						num2++;
						if (num2 == num)
						{
							dependencyObject = (DependencyObject)enumerator.Current;
							break;
						}
					}
				}
			}
			return dependencyObject;
		}

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x06002D8D RID: 11661 RVA: 0x000CD423 File Offset: 0x000CB623
		// (set) Token: 0x06002D8E RID: 11662 RVA: 0x000CD42B File Offset: 0x000CB62B
		internal string StartPartUriString
		{
			get
			{
				return this._startPartUriString;
			}
			set
			{
				this._startPartUriString = value;
			}
		}

		// Token: 0x06002D8F RID: 11663 RVA: 0x000CD434 File Offset: 0x000CB634
		private void Init()
		{
			if (XpsValidatingLoader.DocumentMode)
			{
				base.InheritanceBehavior = InheritanceBehavior.SkipAllNext;
			}
		}

		// Token: 0x06002D90 RID: 11664 RVA: 0x000CD444 File Offset: 0x000CB644
		internal StoryFragments GetPageStructure()
		{
			return FixedDocument.GetStoryFragments(this);
		}

		// Token: 0x06002D91 RID: 11665 RVA: 0x000CD45C File Offset: 0x000CB65C
		internal int[] _CreateChildIndex(DependencyObject e)
		{
			ArrayList arrayList = new ArrayList();
			while (e != this)
			{
				DependencyObject parent = LogicalTreeHelper.GetParent(e);
				int num = -1;
				if (parent is FixedPage)
				{
					num = ((FixedPage)parent).Children.IndexOf((UIElement)e);
				}
				else if (parent is Canvas)
				{
					num = ((Canvas)parent).Children.IndexOf((UIElement)e);
				}
				else
				{
					IEnumerable children = LogicalTreeHelper.GetChildren(parent);
					IEnumerator enumerator = children.GetEnumerator();
					while (enumerator.MoveNext())
					{
						num++;
						if (enumerator.Current == e)
						{
							break;
						}
					}
				}
				arrayList.Insert(0, num);
				e = parent;
			}
			while (e != this)
			{
			}
			return (int[])arrayList.ToArray(typeof(int));
		}

		// Token: 0x06002D92 RID: 11666 RVA: 0x000CD517 File Offset: 0x000CB717
		private FixedNode _CreateFixedNode(int pageIndex, UIElement e)
		{
			return FixedNode.Create(pageIndex, this._CreateChildIndex(e));
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x000CD528 File Offset: 0x000CB728
		private static string GetStartPartUriString(DependencyObject current)
		{
			DependencyObject dependencyObject = current;
			FixedPage fixedPage = current as FixedPage;
			while (fixedPage == null && dependencyObject != null)
			{
				dependencyObject = dependencyObject.InheritanceParent;
				fixedPage = (dependencyObject as FixedPage);
			}
			if (fixedPage == null)
			{
				return null;
			}
			if (fixedPage.StartPartUriString == null)
			{
				DependencyObject parent = LogicalTreeHelper.GetParent(current);
				while (parent != null)
				{
					FixedDocumentSequence fixedDocumentSequence = parent as FixedDocumentSequence;
					if (fixedDocumentSequence != null)
					{
						Uri baseUri = ((IUriContext)fixedDocumentSequence).BaseUri;
						if (!(baseUri != null))
						{
							break;
						}
						string text = baseUri.ToString();
						string fragment = baseUri.Fragment;
						int num = (fragment == null) ? 0 : fragment.Length;
						if (num != 0)
						{
							fixedPage.StartPartUriString = text.Substring(0, text.IndexOf('#'));
							break;
						}
						fixedPage.StartPartUriString = baseUri.ToString();
						break;
					}
					else
					{
						parent = LogicalTreeHelper.GetParent(parent);
					}
				}
				if (fixedPage.StartPartUriString == null)
				{
					fixedPage.StartPartUriString = string.Empty;
				}
			}
			if (fixedPage.StartPartUriString == string.Empty)
			{
				return null;
			}
			return fixedPage.StartPartUriString;
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FixedPage.PrintTicket" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FixedPage.PrintTicket" /> dependency property.</returns>
		// Token: 0x04001DA6 RID: 7590
		public static readonly DependencyProperty PrintTicketProperty = DependencyProperty.RegisterAttached("PrintTicket", typeof(object), typeof(FixedPage), new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FixedPage.Background" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FixedPage.Background" /> dependency property.</returns>
		// Token: 0x04001DA7 RID: 7591
		public static readonly DependencyProperty BackgroundProperty = Panel.BackgroundProperty.AddOwner(typeof(FixedPage), new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FixedPage.Left" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FixedPage.Left" /> attached property.</returns>
		// Token: 0x04001DA8 RID: 7592
		public static readonly DependencyProperty LeftProperty = DependencyProperty.RegisterAttached("Left", typeof(double), typeof(FixedPage), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsParentArrange));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FixedPage.Top" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FixedPage.Top" /> attached property.</returns>
		// Token: 0x04001DA9 RID: 7593
		public static readonly DependencyProperty TopProperty = DependencyProperty.RegisterAttached("Top", typeof(double), typeof(FixedPage), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsParentArrange));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FixedPage.Right" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FixedPage.Right" /> dependency property.</returns>
		// Token: 0x04001DAA RID: 7594
		public static readonly DependencyProperty RightProperty = DependencyProperty.RegisterAttached("Right", typeof(double), typeof(FixedPage), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsParentArrange));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FixedPage.Bottom" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FixedPage.Bottom" /> attached property.</returns>
		// Token: 0x04001DAB RID: 7595
		public static readonly DependencyProperty BottomProperty = DependencyProperty.RegisterAttached("Bottom", typeof(double), typeof(FixedPage), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsParentArrange));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FixedPage.ContentBox" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FixedPage.ContentBox" /> dependency property.</returns>
		// Token: 0x04001DAC RID: 7596
		public static readonly DependencyProperty ContentBoxProperty = DependencyProperty.Register("ContentBox", typeof(Rect), typeof(FixedPage), new FrameworkPropertyMetadata(Rect.Empty));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FixedPage.BleedBox" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.DocumentPage.BleedBox" /> dependency property.</returns>
		// Token: 0x04001DAD RID: 7597
		public static readonly DependencyProperty BleedBoxProperty = DependencyProperty.Register("BleedBox", typeof(Rect), typeof(FixedPage), new FrameworkPropertyMetadata(Rect.Empty));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FixedPage.NavigateUri" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FixedPage.NavigateUri" /> attached property.</returns>
		// Token: 0x04001DAE RID: 7598
		public static readonly DependencyProperty NavigateUriProperty = DependencyProperty.RegisterAttached("NavigateUri", typeof(Uri), typeof(FixedPage), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Hyperlink.OnNavigateUriChanged), new CoerceValueCallback(Hyperlink.CoerceNavigateUri)));

		// Token: 0x04001DAF RID: 7599
		private string _startPartUriString;

		// Token: 0x04001DB0 RID: 7600
		private UIElementCollection _uiElementCollection;
	}
}
