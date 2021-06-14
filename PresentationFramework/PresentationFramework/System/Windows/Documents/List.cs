using System;
using System.ComponentModel;
using System.Windows.Markup;
using MS.Internal;

namespace System.Windows.Documents
{
	/// <summary>A block-level flow content element that provides facilities for presenting content in an ordered or unordered list.</summary>
	// Token: 0x02000394 RID: 916
	[ContentProperty("ListItems")]
	public class List : Block
	{
		// Token: 0x060031B0 RID: 12720 RVA: 0x000DBAFC File Offset: 0x000D9CFC
		static List()
		{
			FrameworkContentElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(List), new FrameworkPropertyMetadata(typeof(List)));
		}

		/// <summary>Initializes a new, empty instance of the <see cref="T:System.Windows.Documents.List" /> class. </summary>
		// Token: 0x060031B1 RID: 12721 RVA: 0x000C454D File Offset: 0x000C274D
		public List()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.List" /> class, taking a specified <see cref="T:System.Windows.Documents.ListItem" /> object as the initial contents of the new <see cref="T:System.Windows.Documents.List" />.</summary>
		/// <param name="listItem">A <see cref="T:System.Windows.Documents.ListItem" /> object specifying the initial contents of the new <see cref="T:System.Windows.Documents.List" />.</param>
		// Token: 0x060031B2 RID: 12722 RVA: 0x000DBBE8 File Offset: 0x000D9DE8
		public List(ListItem listItem)
		{
			if (listItem == null)
			{
				throw new ArgumentNullException("listItem");
			}
			this.ListItems.Add(listItem);
		}

		/// <summary>Gets a <see cref="T:System.Windows.Documents.ListItemCollection" /> containing the <see cref="T:System.Windows.Documents.ListItem" /> elements that comprise the contents of the <see cref="T:System.Windows.Documents.List" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.ListItemCollection" /> containing the <see cref="T:System.Windows.Documents.ListItem" /> elements that comprise the contents of the <see cref="T:System.Windows.Documents.List" />.This property has no default value.</returns>
		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x060031B3 RID: 12723 RVA: 0x000DBC0A File Offset: 0x000D9E0A
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ListItemCollection ListItems
		{
			get
			{
				return new ListItemCollection(this, true);
			}
		}

		/// <summary>Gets or sets the marker style for the <see cref="T:System.Windows.Documents.List" />.  </summary>
		/// <returns>A member of the <see cref="T:System.Windows.TextMarkerStyle" /> enumeration specifying the marker style to use.The default value is <see cref="F:System.Windows.TextMarkerStyle.Disc" />.</returns>
		// Token: 0x17000C7F RID: 3199
		// (get) Token: 0x060031B4 RID: 12724 RVA: 0x000DBC13 File Offset: 0x000D9E13
		// (set) Token: 0x060031B5 RID: 12725 RVA: 0x000DBC25 File Offset: 0x000D9E25
		public TextMarkerStyle MarkerStyle
		{
			get
			{
				return (TextMarkerStyle)base.GetValue(List.MarkerStyleProperty);
			}
			set
			{
				base.SetValue(List.MarkerStyleProperty, value);
			}
		}

		/// <summary>Gets or sets the desired distance between the contents of each <see cref="T:System.Windows.Documents.ListItem" /> element, and the near edge of the list marker.  </summary>
		/// <returns>A double value specifying the desired distance between list content and the near edge of list markers, in device independent pixels.  A value of <see cref="F:System.Double.NaN" /> (equivalent to an attribute value of "Auto") causes the marker offset to be determined automatically.The default value is <see cref="F:System.Double.NaN" />.</returns>
		// Token: 0x17000C80 RID: 3200
		// (get) Token: 0x060031B6 RID: 12726 RVA: 0x000DBC38 File Offset: 0x000D9E38
		// (set) Token: 0x060031B7 RID: 12727 RVA: 0x000DBC4A File Offset: 0x000D9E4A
		[TypeConverter(typeof(LengthConverter))]
		public double MarkerOffset
		{
			get
			{
				return (double)base.GetValue(List.MarkerOffsetProperty);
			}
			set
			{
				base.SetValue(List.MarkerOffsetProperty, value);
			}
		}

		/// <summary>Gets or sets the starting index for labeling the items in an ordered list.  </summary>
		/// <returns>The starting index for labeling items in an ordered list.The default value is 1.</returns>
		// Token: 0x17000C81 RID: 3201
		// (get) Token: 0x060031B8 RID: 12728 RVA: 0x000DBC5D File Offset: 0x000D9E5D
		// (set) Token: 0x060031B9 RID: 12729 RVA: 0x000DBC6F File Offset: 0x000D9E6F
		public int StartIndex
		{
			get
			{
				return (int)base.GetValue(List.StartIndexProperty);
			}
			set
			{
				base.SetValue(List.StartIndexProperty, value);
			}
		}

		// Token: 0x060031BA RID: 12730 RVA: 0x000DBC84 File Offset: 0x000D9E84
		internal int GetListItemIndex(ListItem item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (item.Parent != this)
			{
				throw new InvalidOperationException(SR.Get("ListElementItemNotAChildOfList"));
			}
			int num = this.StartIndex;
			TextPointer textPointer = new TextPointer(base.ContentStart);
			while (textPointer.CompareTo(base.ContentEnd) != 0)
			{
				if (textPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart)
				{
					DependencyObject adjacentElementFromOuterPosition = textPointer.GetAdjacentElementFromOuterPosition(LogicalDirection.Forward);
					if (adjacentElementFromOuterPosition is ListItem)
					{
						if (adjacentElementFromOuterPosition == item)
						{
							break;
						}
						if (num < 2147483647)
						{
							num++;
						}
					}
					textPointer.MoveToPosition(((TextElement)adjacentElementFromOuterPosition).ElementEnd);
				}
				else
				{
					textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
				}
			}
			return num;
		}

		// Token: 0x060031BB RID: 12731 RVA: 0x000DBD24 File Offset: 0x000D9F24
		internal void Apply(Block firstBlock, Block lastBlock)
		{
			Invariant.Assert(base.Parent == null, "Cannot Apply List Because It Is Inserted In The Tree Already.");
			Invariant.Assert(base.IsEmpty, "Cannot Apply List Because It Is Not Empty.");
			Invariant.Assert(firstBlock.Parent == lastBlock.Parent, "Cannot Apply List Because Block Are Not Siblings.");
			TextContainer textContainer = base.TextContainer;
			textContainer.BeginChange();
			try
			{
				base.Reposition(firstBlock.ElementStart, lastBlock.ElementEnd);
				ListItem listItem;
				for (Block block = firstBlock; block != null; block = ((block == lastBlock) ? null : ((Block)listItem.ElementEnd.GetAdjacentElement(LogicalDirection.Forward))))
				{
					if (block is List)
					{
						listItem = (block.ElementStart.GetAdjacentElement(LogicalDirection.Backward) as ListItem);
						if (listItem != null)
						{
							listItem.Reposition(listItem.ContentStart, block.ElementEnd);
						}
						else
						{
							listItem = new ListItem();
							listItem.Reposition(block.ElementStart, block.ElementEnd);
						}
					}
					else
					{
						listItem = new ListItem();
						listItem.Reposition(block.ElementStart, block.ElementEnd);
						block.ClearValue(Block.MarginProperty);
						block.ClearValue(Block.PaddingProperty);
						block.ClearValue(Paragraph.TextIndentProperty);
					}
				}
				TextRangeEdit.SetParagraphProperty(base.ElementStart, base.ElementEnd, Block.FlowDirectionProperty, firstBlock.GetValue(Block.FlowDirectionProperty));
			}
			finally
			{
				textContainer.EndChange();
			}
		}

		// Token: 0x060031BC RID: 12732 RVA: 0x000DBE70 File Offset: 0x000DA070
		private static bool IsValidMarkerStyle(object o)
		{
			TextMarkerStyle textMarkerStyle = (TextMarkerStyle)o;
			return textMarkerStyle == TextMarkerStyle.None || textMarkerStyle == TextMarkerStyle.Disc || textMarkerStyle == TextMarkerStyle.Circle || textMarkerStyle == TextMarkerStyle.Square || textMarkerStyle == TextMarkerStyle.Box || textMarkerStyle == TextMarkerStyle.LowerRoman || textMarkerStyle == TextMarkerStyle.UpperRoman || textMarkerStyle == TextMarkerStyle.LowerLatin || textMarkerStyle == TextMarkerStyle.UpperLatin || textMarkerStyle == TextMarkerStyle.Decimal;
		}

		// Token: 0x060031BD RID: 12733 RVA: 0x000DBEB0 File Offset: 0x000DA0B0
		private static bool IsValidStartIndex(object o)
		{
			int num = (int)o;
			return num > 0;
		}

		// Token: 0x060031BE RID: 12734 RVA: 0x000DBEC8 File Offset: 0x000DA0C8
		private static bool IsValidMarkerOffset(object o)
		{
			double num = (double)o;
			double num2 = (double)Math.Min(1000000, 3500000);
			double num3 = -num2;
			return double.IsNaN(num) || (num >= num3 && num <= num2);
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.List.MarkerStyle" />  dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.List.MarkerStyle" /> dependency property.</returns>
		// Token: 0x04001E98 RID: 7832
		public static readonly DependencyProperty MarkerStyleProperty = DependencyProperty.Register("MarkerStyle", typeof(TextMarkerStyle), typeof(List), new FrameworkPropertyMetadata(TextMarkerStyle.Disc, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(List.IsValidMarkerStyle));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.List.MarkerOffset" />  dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.List.MarkerOffset" /> dependency property.</returns>
		// Token: 0x04001E99 RID: 7833
		public static readonly DependencyProperty MarkerOffsetProperty = DependencyProperty.Register("MarkerOffset", typeof(double), typeof(List), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(List.IsValidMarkerOffset));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.List.StartIndex" />  dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.List.StartIndex" /> dependency property.</returns>
		// Token: 0x04001E9A RID: 7834
		public static readonly DependencyProperty StartIndexProperty = DependencyProperty.Register("StartIndex", typeof(int), typeof(List), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(List.IsValidStartIndex));
	}
}
