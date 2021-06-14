using System;
using System.ComponentModel;
using System.Windows.Markup;
using System.Windows.Media;

namespace System.Windows.Documents
{
	/// <summary>A flow content element that represents a particular content item in an ordered or unordered <see cref="T:System.Windows.Documents.List" />.</summary>
	// Token: 0x02000395 RID: 917
	[ContentProperty("Blocks")]
	public class ListItem : TextElement
	{
		/// <summary>Initializes a new, empty instance of the <see cref="T:System.Windows.Documents.ListItem" /> class.</summary>
		// Token: 0x060031BF RID: 12735 RVA: 0x000C428C File Offset: 0x000C248C
		public ListItem()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.ListItem" /> class, taking a specified <see cref="T:System.Windows.Documents.Paragraph" /> object as the initial contents of the new <see cref="T:System.Windows.Documents.ListItem" />.</summary>
		/// <param name="paragraph">A <see cref="T:System.Windows.Documents.Paragraph" /> object specifying the initial contents of the new <see cref="T:System.Windows.Documents.ListItem" />.</param>
		// Token: 0x060031C0 RID: 12736 RVA: 0x000DBF05 File Offset: 0x000DA105
		public ListItem(Paragraph paragraph)
		{
			if (paragraph == null)
			{
				throw new ArgumentNullException("paragraph");
			}
			this.Blocks.Add(paragraph);
		}

		/// <summary>Gets the <see cref="T:System.Windows.Documents.List" /> that contains the <see cref="T:System.Windows.Documents.ListItem" />.</summary>
		/// <returns>The list that contains the <see cref="T:System.Windows.Documents.ListItem" />.</returns>
		// Token: 0x17000C82 RID: 3202
		// (get) Token: 0x060031C1 RID: 12737 RVA: 0x000DBF27 File Offset: 0x000DA127
		public List List
		{
			get
			{
				return base.Parent as List;
			}
		}

		/// <summary>Gets a block collection that contains the top-level <see cref="T:System.Windows.Documents.Block" /> elements of the <see cref="T:System.Windows.Documents.ListItem" />.</summary>
		/// <returns>A block collection that contains the <see cref="T:System.Windows.Documents.Block" /> elements of the <see cref="T:System.Windows.Documents.ListItem" /></returns>
		// Token: 0x17000C83 RID: 3203
		// (get) Token: 0x060031C2 RID: 12738 RVA: 0x000C3B80 File Offset: 0x000C1D80
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public BlockCollection Blocks
		{
			get
			{
				return new BlockCollection(this, true);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Documents.ListItemCollection" /> that contains the <see cref="T:System.Windows.Documents.ListItem" /> elements that are siblings of the current <see cref="T:System.Windows.Documents.ListItem" /> element.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.ListItemCollection" /> that contains the child <see cref="T:System.Windows.Documents.ListItem" /> elements that are directly hosted by the parent of the current <see cref="T:System.Windows.Documents.ListItem" /> element, or <see langword="null" /> if the current <see cref="T:System.Windows.Documents.ListItem" /> element has no parent.</returns>
		// Token: 0x17000C84 RID: 3204
		// (get) Token: 0x060031C3 RID: 12739 RVA: 0x000DBF34 File Offset: 0x000DA134
		public ListItemCollection SiblingListItems
		{
			get
			{
				if (base.Parent == null)
				{
					return null;
				}
				return new ListItemCollection(this, false);
			}
		}

		/// <summary>Gets the next <see cref="T:System.Windows.Documents.ListItem" /> in the containing <see cref="T:System.Windows.Documents.List" />.</summary>
		/// <returns>The next <see cref="T:System.Windows.Documents.ListItem" /> in the <see cref="T:System.Windows.Documents.List" />, or <see langword="null" /> if there is no next <see cref="T:System.Windows.Documents.ListItem" />.</returns>
		// Token: 0x17000C85 RID: 3205
		// (get) Token: 0x060031C4 RID: 12740 RVA: 0x000DBF47 File Offset: 0x000DA147
		public ListItem NextListItem
		{
			get
			{
				return base.NextElement as ListItem;
			}
		}

		/// <summary>Gets the previous <see cref="T:System.Windows.Documents.ListItem" /> in the containing <see cref="T:System.Windows.Documents.List" />.</summary>
		/// <returns>The previous <see cref="T:System.Windows.Documents.ListItem" /> in the <see cref="T:System.Windows.Documents.List" />, or <see langword="null" /> if there is no previous <see cref="T:System.Windows.Documents.ListItem" />.</returns>
		// Token: 0x17000C86 RID: 3206
		// (get) Token: 0x060031C5 RID: 12741 RVA: 0x000DBF54 File Offset: 0x000DA154
		public ListItem PreviousListItem
		{
			get
			{
				return base.PreviousElement as ListItem;
			}
		}

		/// <summary>Gets or sets the margin thickness for the element.  </summary>
		/// <returns>A <see cref="T:System.Windows.Thickness" /> structure that specifies the amount of margin to apply, in device independent pixels. The default is a uniform thickness of zero (0.0).</returns>
		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x060031C6 RID: 12742 RVA: 0x000DBF61 File Offset: 0x000DA161
		// (set) Token: 0x060031C7 RID: 12743 RVA: 0x000DBF73 File Offset: 0x000DA173
		public Thickness Margin
		{
			get
			{
				return (Thickness)base.GetValue(ListItem.MarginProperty);
			}
			set
			{
				base.SetValue(ListItem.MarginProperty, value);
			}
		}

		/// <summary>Gets or sets the padding thickness for the element.  </summary>
		/// <returns>A <see cref="T:System.Windows.Thickness" /> structure that specifies the amount of padding to apply, in device independent pixels. The default is a uniform thickness of zero (0.0).</returns>
		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x060031C8 RID: 12744 RVA: 0x000DBF86 File Offset: 0x000DA186
		// (set) Token: 0x060031C9 RID: 12745 RVA: 0x000DBF98 File Offset: 0x000DA198
		public Thickness Padding
		{
			get
			{
				return (Thickness)base.GetValue(ListItem.PaddingProperty);
			}
			set
			{
				base.SetValue(ListItem.PaddingProperty, value);
			}
		}

		/// <summary>Gets or sets the border thickness for the element.  </summary>
		/// <returns>A <see cref="T:System.Windows.Thickness" /> structure that specifies the amount of border to apply, in device independent pixels. The default is a uniform thickness of zero (0.0).</returns>
		// Token: 0x17000C89 RID: 3209
		// (get) Token: 0x060031CA RID: 12746 RVA: 0x000DBFAB File Offset: 0x000DA1AB
		// (set) Token: 0x060031CB RID: 12747 RVA: 0x000DBFBD File Offset: 0x000DA1BD
		public Thickness BorderThickness
		{
			get
			{
				return (Thickness)base.GetValue(ListItem.BorderThicknessProperty);
			}
			set
			{
				base.SetValue(ListItem.BorderThicknessProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Media.Brush" /> to use when painting the element's border.  </summary>
		/// <returns>The brush used to apply to the element's border. The default is <see langword="null" />.</returns>
		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x060031CC RID: 12748 RVA: 0x000DBFD0 File Offset: 0x000DA1D0
		// (set) Token: 0x060031CD RID: 12749 RVA: 0x000DBFE2 File Offset: 0x000DA1E2
		public Brush BorderBrush
		{
			get
			{
				return (Brush)base.GetValue(ListItem.BorderBrushProperty);
			}
			set
			{
				base.SetValue(ListItem.BorderBrushProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates the horizontal alignment of text content.  </summary>
		/// <returns>One of the <see cref="T:System.Windows.TextAlignment" /> values that specifies the desired alignment. The default is <see cref="F:System.Windows.TextAlignment.Left" />.</returns>
		// Token: 0x17000C8B RID: 3211
		// (get) Token: 0x060031CE RID: 12750 RVA: 0x000DBFF0 File Offset: 0x000DA1F0
		// (set) Token: 0x060031CF RID: 12751 RVA: 0x000DC002 File Offset: 0x000DA202
		public TextAlignment TextAlignment
		{
			get
			{
				return (TextAlignment)base.GetValue(ListItem.TextAlignmentProperty);
			}
			set
			{
				base.SetValue(ListItem.TextAlignmentProperty, value);
			}
		}

		/// <summary>Gets or sets the relative direction for flow of content within a <see cref="T:System.Windows.Documents.ListItem" /> element.  </summary>
		/// <returns>One of the <see cref="T:System.Windows.FlowDirection" /> values that specifies the relative flow direction.  The default is <see cref="F:System.Windows.FlowDirection.LeftToRight" />.</returns>
		// Token: 0x17000C8C RID: 3212
		// (get) Token: 0x060031D0 RID: 12752 RVA: 0x000DC015 File Offset: 0x000DA215
		// (set) Token: 0x060031D1 RID: 12753 RVA: 0x000DC027 File Offset: 0x000DA227
		public FlowDirection FlowDirection
		{
			get
			{
				return (FlowDirection)base.GetValue(ListItem.FlowDirectionProperty);
			}
			set
			{
				base.SetValue(ListItem.FlowDirectionProperty, value);
			}
		}

		/// <summary>Gets or sets the height of each line of content.  </summary>
		/// <returns>The height of each line in device independent pixels with a value range of 0.0034 to 160000.  A value of <see cref="F:System.Double.NaN" /> (equivalent to an attribute value of "Auto") causes the line height to be determined automatically from the current font characteristics.  The default is <see cref="F:System.Double.NaN" />.</returns>
		// Token: 0x17000C8D RID: 3213
		// (get) Token: 0x060031D2 RID: 12754 RVA: 0x000DC03A File Offset: 0x000DA23A
		// (set) Token: 0x060031D3 RID: 12755 RVA: 0x000DC04C File Offset: 0x000DA24C
		[TypeConverter(typeof(LengthConverter))]
		public double LineHeight
		{
			get
			{
				return (double)base.GetValue(ListItem.LineHeightProperty);
			}
			set
			{
				base.SetValue(ListItem.LineHeightProperty, value);
			}
		}

		/// <summary>Gets or sets the mechanism by which a line box is determined for each line of text within the <see cref="T:System.Windows.Documents.ListItem" />.  </summary>
		/// <returns>One of the <see cref="T:System.Windows.LineStackingStrategy" /> values that specifies the mechanism by which a line box is determined for each line of text within the <see cref="T:System.Windows.Documents.ListItem" />. The default is <see cref="F:System.Windows.LineStackingStrategy.MaxHeight" />.</returns>
		// Token: 0x17000C8E RID: 3214
		// (get) Token: 0x060031D4 RID: 12756 RVA: 0x000DC05F File Offset: 0x000DA25F
		// (set) Token: 0x060031D5 RID: 12757 RVA: 0x000DC071 File Offset: 0x000DA271
		public LineStackingStrategy LineStackingStrategy
		{
			get
			{
				return (LineStackingStrategy)base.GetValue(ListItem.LineStackingStrategyProperty);
			}
			set
			{
				base.SetValue(ListItem.LineStackingStrategyProperty, value);
			}
		}

		/// <summary>Returns a value that indicates whether the effective value of the <see cref="P:System.Windows.Documents.ListItem.Blocks" /> property should be serialized during serialization of the <see cref="T:System.Windows.Documents.ListItem" /> object.</summary>
		/// <param name="manager">A serialization service manager object for the object.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Documents.ListItem.Blocks" /> property should be serialized; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.NullReferenceException">
		///         <paramref name="manager" /> is <see langword="null" />.</exception>
		// Token: 0x060031D6 RID: 12758 RVA: 0x000C3C87 File Offset: 0x000C1E87
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBlocks(XamlDesignerSerializationManager manager)
		{
			return manager != null && manager.XmlWriter == null;
		}

		// Token: 0x17000C8F RID: 3215
		// (get) Token: 0x060031D7 RID: 12759 RVA: 0x00016748 File Offset: 0x00014948
		internal override bool IsIMEStructuralElement
		{
			get
			{
				return true;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.ListItem.Margin" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.ListItem.Margin" /> dependency property.</returns>
		// Token: 0x04001E9B RID: 7835
		public static readonly DependencyProperty MarginProperty = Block.MarginProperty.AddOwner(typeof(ListItem), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.ListItem.Padding" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.ListItem.Padding" /> dependency property.</returns>
		// Token: 0x04001E9C RID: 7836
		public static readonly DependencyProperty PaddingProperty = Block.PaddingProperty.AddOwner(typeof(ListItem), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.ListItem.BorderThickness" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.ListItem.BorderThickness" /> dependency property.</returns>
		// Token: 0x04001E9D RID: 7837
		public static readonly DependencyProperty BorderThicknessProperty = Block.BorderThicknessProperty.AddOwner(typeof(ListItem), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.ListItem.BorderBrush" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.ListItem.BorderBrush" /> dependency property.</returns>
		// Token: 0x04001E9E RID: 7838
		public static readonly DependencyProperty BorderBrushProperty = Block.BorderBrushProperty.AddOwner(typeof(ListItem), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.ListItem.TextAlignment" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.ListItem.TextAlignment" /> dependency property.</returns>
		// Token: 0x04001E9F RID: 7839
		public static readonly DependencyProperty TextAlignmentProperty = Block.TextAlignmentProperty.AddOwner(typeof(ListItem));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.ListItem.FlowDirection" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.ListItem.FlowDirection" /> dependency property.</returns>
		// Token: 0x04001EA0 RID: 7840
		public static readonly DependencyProperty FlowDirectionProperty = Block.FlowDirectionProperty.AddOwner(typeof(ListItem));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.ListItem.LineHeight" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.ListItem.LineHeight" /> dependency property.</returns>
		// Token: 0x04001EA1 RID: 7841
		public static readonly DependencyProperty LineHeightProperty = Block.LineHeightProperty.AddOwner(typeof(ListItem));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.ListItem.LineStackingStrategy" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.ListItem.LineStackingStrategy" /> dependency property.</returns>
		// Token: 0x04001EA2 RID: 7842
		public static readonly DependencyProperty LineStackingStrategyProperty = Block.LineStackingStrategyProperty.AddOwner(typeof(ListItem));
	}
}
