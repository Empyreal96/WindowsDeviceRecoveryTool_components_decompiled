using System;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace System.Windows.Documents
{
	// Token: 0x0200034C RID: 844
	internal sealed class FixedElement : DependencyObject
	{
		// Token: 0x06002D1D RID: 11549 RVA: 0x000CBB1B File Offset: 0x000C9D1B
		internal FixedElement(FixedElement.ElementType type, FixedTextPointer start, FixedTextPointer end, int pageIndex)
		{
			this._type = type;
			this._start = start;
			this._end = end;
			this._pageIndex = pageIndex;
		}

		// Token: 0x06002D1E RID: 11550 RVA: 0x000CBB40 File Offset: 0x000C9D40
		internal void Append(FixedElement e)
		{
			if (this._type == FixedElement.ElementType.InlineUIContainer)
			{
				this._object = e._object;
			}
		}

		// Token: 0x06002D1F RID: 11551 RVA: 0x000CBB58 File Offset: 0x000C9D58
		internal object GetObject()
		{
			if (this._type == FixedElement.ElementType.Hyperlink || this._type == FixedElement.ElementType.Paragraph || (this._type >= FixedElement.ElementType.Table && this._type <= FixedElement.ElementType.TableCell))
			{
				if (this._object == null)
				{
					this._object = this.BuildObjectTree();
				}
				return this._object;
			}
			if (this._type != FixedElement.ElementType.Object && this._type != FixedElement.ElementType.InlineUIContainer)
			{
				return null;
			}
			Image image = this.GetImage();
			object result = image;
			if (this._type == FixedElement.ElementType.InlineUIContainer)
			{
				result = new InlineUIContainer
				{
					Child = image
				};
			}
			return result;
		}

		// Token: 0x06002D20 RID: 11552 RVA: 0x000CBBE0 File Offset: 0x000C9DE0
		internal object BuildObjectTree()
		{
			FixedElement.ElementType type = this._type;
			IAddChild addChild;
			if (type != FixedElement.ElementType.Paragraph)
			{
				switch (type)
				{
				case FixedElement.ElementType.Table:
					addChild = new Table();
					goto IL_C7;
				case FixedElement.ElementType.TableRowGroup:
					addChild = new TableRowGroup();
					goto IL_C7;
				case FixedElement.ElementType.TableRow:
					addChild = new TableRow();
					goto IL_C7;
				case FixedElement.ElementType.TableCell:
					addChild = new TableCell();
					goto IL_C7;
				case FixedElement.ElementType.Hyperlink:
				{
					Hyperlink hyperlink = new Hyperlink();
					hyperlink.NavigateUri = (base.GetValue(FixedElement.NavigateUriProperty) as Uri);
					hyperlink.RequestNavigate += this.ClickHyperlink;
					AutomationProperties.SetHelpText(hyperlink, (string)base.GetValue(FixedElement.HelpTextProperty));
					AutomationProperties.SetName(hyperlink, (string)base.GetValue(FixedElement.NameProperty));
					addChild = hyperlink;
					goto IL_C7;
				}
				}
				addChild = null;
			}
			else
			{
				addChild = new Paragraph();
			}
			IL_C7:
			ITextPointer textPointer = ((ITextPointer)this._start).CreatePointer();
			while (textPointer.CompareTo(this._end) < 0)
			{
				TextPointerContext pointerContext = textPointer.GetPointerContext(LogicalDirection.Forward);
				if (pointerContext == TextPointerContext.Text)
				{
					addChild.AddText(textPointer.GetTextInRun(LogicalDirection.Forward));
				}
				else if (pointerContext == TextPointerContext.EmbeddedElement)
				{
					addChild.AddChild(textPointer.GetAdjacentElement(LogicalDirection.Forward));
				}
				else if (pointerContext == TextPointerContext.ElementStart)
				{
					object adjacentElement = textPointer.GetAdjacentElement(LogicalDirection.Forward);
					if (adjacentElement != null)
					{
						addChild.AddChild(adjacentElement);
						textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
						textPointer.MoveToElementEdge(ElementEdge.BeforeEnd);
					}
				}
				textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
			}
			return addChild;
		}

		// Token: 0x06002D21 RID: 11553 RVA: 0x000CBD34 File Offset: 0x000C9F34
		private Image GetImage()
		{
			Image image = null;
			Uri uri = this._object as Uri;
			if (uri != null)
			{
				image = new Image();
				image.Source = new BitmapImage(uri);
				image.Width = image.Source.Width;
				image.Height = image.Source.Height;
				AutomationProperties.SetName(image, (string)base.GetValue(FixedElement.NameProperty));
				AutomationProperties.SetHelpText(image, (string)base.GetValue(FixedElement.HelpTextProperty));
			}
			return image;
		}

		// Token: 0x06002D22 RID: 11554 RVA: 0x000CBDBC File Offset: 0x000C9FBC
		private void ClickHyperlink(object sender, RequestNavigateEventArgs args)
		{
			FixedDocument fixedDocument = this._start.FixedTextContainer.FixedDocument;
			int pageNumber = fixedDocument.GetPageNumber(this._start);
			FixedPage element = fixedDocument.SyncGetPage(pageNumber, false);
			Hyperlink.RaiseNavigate(element, args.Uri, null);
		}

		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x06002D23 RID: 11555 RVA: 0x000CBDFD File Offset: 0x000C9FFD
		internal bool IsTextElement
		{
			get
			{
				return this._type != FixedElement.ElementType.Object && this._type != FixedElement.ElementType.Container;
			}
		}

		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x06002D24 RID: 11556 RVA: 0x000CBE18 File Offset: 0x000CA018
		internal Type Type
		{
			get
			{
				switch (this._type)
				{
				case FixedElement.ElementType.Paragraph:
					return typeof(Paragraph);
				case FixedElement.ElementType.Inline:
					return typeof(Inline);
				case FixedElement.ElementType.Run:
					return typeof(Run);
				case FixedElement.ElementType.Span:
					return typeof(Span);
				case FixedElement.ElementType.Bold:
					return typeof(Bold);
				case FixedElement.ElementType.Italic:
					return typeof(Italic);
				case FixedElement.ElementType.Underline:
					return typeof(Underline);
				case FixedElement.ElementType.Object:
					return typeof(object);
				case FixedElement.ElementType.Section:
					return typeof(Section);
				case FixedElement.ElementType.Figure:
					return typeof(Figure);
				case FixedElement.ElementType.Table:
					return typeof(Table);
				case FixedElement.ElementType.TableRowGroup:
					return typeof(TableRowGroup);
				case FixedElement.ElementType.TableRow:
					return typeof(TableRow);
				case FixedElement.ElementType.TableCell:
					return typeof(TableCell);
				case FixedElement.ElementType.List:
					return typeof(List);
				case FixedElement.ElementType.ListItem:
					return typeof(ListItem);
				case FixedElement.ElementType.Hyperlink:
					return typeof(Hyperlink);
				case FixedElement.ElementType.InlineUIContainer:
					return typeof(InlineUIContainer);
				}
				return typeof(object);
			}
		}

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x06002D25 RID: 11557 RVA: 0x000CBF5B File Offset: 0x000CA15B
		internal FixedTextPointer Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x06002D26 RID: 11558 RVA: 0x000CBF63 File Offset: 0x000CA163
		internal FixedTextPointer End
		{
			get
			{
				return this._end;
			}
		}

		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x06002D27 RID: 11559 RVA: 0x000CBF6B File Offset: 0x000CA16B
		internal int PageIndex
		{
			get
			{
				return this._pageIndex;
			}
		}

		// Token: 0x17000B3B RID: 2875
		// (set) Token: 0x06002D28 RID: 11560 RVA: 0x000CBF73 File Offset: 0x000CA173
		internal object Object
		{
			set
			{
				this._object = value;
			}
		}

		// Token: 0x04001D6E RID: 7534
		public static readonly DependencyProperty LanguageProperty = FrameworkElement.LanguageProperty.AddOwner(typeof(FixedElement));

		// Token: 0x04001D6F RID: 7535
		public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(FixedElement));

		// Token: 0x04001D70 RID: 7536
		public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(FixedElement));

		// Token: 0x04001D71 RID: 7537
		public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof(FixedElement));

		// Token: 0x04001D72 RID: 7538
		public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(typeof(FixedElement));

		// Token: 0x04001D73 RID: 7539
		public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(FixedElement));

		// Token: 0x04001D74 RID: 7540
		public static readonly DependencyProperty ForegroundProperty = TextElement.ForegroundProperty.AddOwner(typeof(FixedElement));

		// Token: 0x04001D75 RID: 7541
		public static readonly DependencyProperty FlowDirectionProperty = FrameworkElement.FlowDirectionProperty.AddOwner(typeof(FixedElement));

		// Token: 0x04001D76 RID: 7542
		public static readonly DependencyProperty CellSpacingProperty = Table.CellSpacingProperty.AddOwner(typeof(FixedElement));

		// Token: 0x04001D77 RID: 7543
		public static readonly DependencyProperty BorderThicknessProperty = Block.BorderThicknessProperty.AddOwner(typeof(FixedElement));

		// Token: 0x04001D78 RID: 7544
		public static readonly DependencyProperty BorderBrushProperty = Block.BorderBrushProperty.AddOwner(typeof(FixedElement));

		// Token: 0x04001D79 RID: 7545
		public static readonly DependencyProperty ColumnSpanProperty = TableCell.ColumnSpanProperty.AddOwner(typeof(FixedElement));

		// Token: 0x04001D7A RID: 7546
		public static readonly DependencyProperty NavigateUriProperty = Hyperlink.NavigateUriProperty.AddOwner(typeof(FixedElement));

		// Token: 0x04001D7B RID: 7547
		public static readonly DependencyProperty NameProperty = AutomationProperties.NameProperty.AddOwner(typeof(FixedElement));

		// Token: 0x04001D7C RID: 7548
		public static readonly DependencyProperty HelpTextProperty = AutomationProperties.HelpTextProperty.AddOwner(typeof(FixedElement));

		// Token: 0x04001D7D RID: 7549
		private FixedElement.ElementType _type;

		// Token: 0x04001D7E RID: 7550
		private FixedTextPointer _start;

		// Token: 0x04001D7F RID: 7551
		private FixedTextPointer _end;

		// Token: 0x04001D80 RID: 7552
		private object _object;

		// Token: 0x04001D81 RID: 7553
		private int _pageIndex;

		// Token: 0x020008D1 RID: 2257
		internal enum ElementType
		{
			// Token: 0x04004243 RID: 16963
			Paragraph,
			// Token: 0x04004244 RID: 16964
			Inline,
			// Token: 0x04004245 RID: 16965
			Run,
			// Token: 0x04004246 RID: 16966
			Span,
			// Token: 0x04004247 RID: 16967
			Bold,
			// Token: 0x04004248 RID: 16968
			Italic,
			// Token: 0x04004249 RID: 16969
			Underline,
			// Token: 0x0400424A RID: 16970
			Object,
			// Token: 0x0400424B RID: 16971
			Container,
			// Token: 0x0400424C RID: 16972
			Section,
			// Token: 0x0400424D RID: 16973
			Figure,
			// Token: 0x0400424E RID: 16974
			Table,
			// Token: 0x0400424F RID: 16975
			TableRowGroup,
			// Token: 0x04004250 RID: 16976
			TableRow,
			// Token: 0x04004251 RID: 16977
			TableCell,
			// Token: 0x04004252 RID: 16978
			List,
			// Token: 0x04004253 RID: 16979
			ListItem,
			// Token: 0x04004254 RID: 16980
			Header,
			// Token: 0x04004255 RID: 16981
			Footer,
			// Token: 0x04004256 RID: 16982
			Hyperlink,
			// Token: 0x04004257 RID: 16983
			InlineUIContainer
		}
	}
}
