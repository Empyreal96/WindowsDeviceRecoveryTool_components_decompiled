using System;
using System.ComponentModel;
using System.Windows.Markup;
using System.Windows.Media;

namespace System.Windows.Documents
{
	/// <summary>An abstract class that provides a base for <see cref="T:System.Windows.Documents.Inline" /> elements that are used to anchor <see cref="T:System.Windows.Documents.Block" /> elements to flow content.</summary>
	// Token: 0x0200032C RID: 812
	[ContentProperty("Blocks")]
	public abstract class AnchoredBlock : Inline
	{
		/// <summary>Initializes base class values when called by a derived class, taking a specified <see cref="T:System.Windows.Documents.Block" /> object as the initial contents of the new descendant of <see cref="T:System.Windows.Documents.AnchoredBlock" />, and a <see cref="T:System.Windows.Documents.TextPointer" /> specifying an insertion position for the new <see cref="T:System.Windows.Documents.AnchoredBlock" /> descendant.</summary>
		/// <param name="block">A <see cref="T:System.Windows.Documents.Block" /> object specifying the initial contents of the new element.  This parameter may be <see langword="null" />, in which case no <see cref="T:System.Windows.Documents.Block" /> is inserted.</param>
		/// <param name="insertionPosition">A <see cref="T:System.Windows.Documents.TextPointer" /> specifying an insertion position at which to insert the element after it is created, or <see langword="null" /> for no automatic insertion.</param>
		// Token: 0x06002AC8 RID: 10952 RVA: 0x000C3B24 File Offset: 0x000C1D24
		protected AnchoredBlock(Block block, TextPointer insertionPosition)
		{
			if (insertionPosition != null)
			{
				insertionPosition.TextContainer.BeginChange();
			}
			try
			{
				if (insertionPosition != null)
				{
					insertionPosition.InsertInline(this);
				}
				if (block != null)
				{
					this.Blocks.Add(block);
				}
			}
			finally
			{
				if (insertionPosition != null)
				{
					insertionPosition.TextContainer.EndChange();
				}
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Documents.BlockCollection" /> containing the top-level <see cref="T:System.Windows.Documents.Block" /> elements that comprise the contents of the element.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.BlockCollection" /> containing the <see cref="T:System.Windows.Documents.Block" /> elements that comprise the contents of the element. This property has no default value.</returns>
		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x06002AC9 RID: 10953 RVA: 0x000C3B80 File Offset: 0x000C1D80
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public BlockCollection Blocks
		{
			get
			{
				return new BlockCollection(this, true);
			}
		}

		/// <summary>Gets or sets the margin thickness for the element. </summary>
		/// <returns>A <see cref="T:System.Windows.Thickness" /> structure specifying the amount of margin to apply, in device independent pixels.The default value is a uniform thickness of zero (0.0).</returns>
		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x06002ACA RID: 10954 RVA: 0x000C3B89 File Offset: 0x000C1D89
		// (set) Token: 0x06002ACB RID: 10955 RVA: 0x000C3B9B File Offset: 0x000C1D9B
		public Thickness Margin
		{
			get
			{
				return (Thickness)base.GetValue(AnchoredBlock.MarginProperty);
			}
			set
			{
				base.SetValue(AnchoredBlock.MarginProperty, value);
			}
		}

		/// <summary>Gets or sets the padding thickness for the element. </summary>
		/// <returns>A <see cref="T:System.Windows.Thickness" /> structure specifying the amount of padding to apply, in device independent pixels.The default value is a uniform thickness of zero (0.0).</returns>
		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x06002ACC RID: 10956 RVA: 0x000C3BAE File Offset: 0x000C1DAE
		// (set) Token: 0x06002ACD RID: 10957 RVA: 0x000C3BC0 File Offset: 0x000C1DC0
		public Thickness Padding
		{
			get
			{
				return (Thickness)base.GetValue(AnchoredBlock.PaddingProperty);
			}
			set
			{
				base.SetValue(AnchoredBlock.PaddingProperty, value);
			}
		}

		/// <summary>Gets or sets the border thickness for the element. </summary>
		/// <returns>A <see cref="T:System.Windows.Thickness" /> structure specifying the amount of border to apply, in device independent pixels.The default value is a uniform thickness of zero (0.0).</returns>
		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x06002ACE RID: 10958 RVA: 0x000C3BD3 File Offset: 0x000C1DD3
		// (set) Token: 0x06002ACF RID: 10959 RVA: 0x000C3BE5 File Offset: 0x000C1DE5
		public Thickness BorderThickness
		{
			get
			{
				return (Thickness)base.GetValue(AnchoredBlock.BorderThicknessProperty);
			}
			set
			{
				base.SetValue(AnchoredBlock.BorderThicknessProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Media.Brush" /> to use when painting the element's border. </summary>
		/// <returns>The brush used to apply to the element's border.The default value is a null brush.</returns>
		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x06002AD0 RID: 10960 RVA: 0x000C3BF8 File Offset: 0x000C1DF8
		// (set) Token: 0x06002AD1 RID: 10961 RVA: 0x000C3C0A File Offset: 0x000C1E0A
		public Brush BorderBrush
		{
			get
			{
				return (Brush)base.GetValue(AnchoredBlock.BorderBrushProperty);
			}
			set
			{
				base.SetValue(AnchoredBlock.BorderBrushProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates the horizontal alignment of text content. </summary>
		/// <returns>A member of the <see cref="T:System.Windows.TextAlignment" /> enumerations specifying the desired alignment.The default value is <see cref="F:System.Windows.TextAlignment.Left" />.</returns>
		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x06002AD2 RID: 10962 RVA: 0x000C3C18 File Offset: 0x000C1E18
		// (set) Token: 0x06002AD3 RID: 10963 RVA: 0x000C3C2A File Offset: 0x000C1E2A
		public TextAlignment TextAlignment
		{
			get
			{
				return (TextAlignment)base.GetValue(AnchoredBlock.TextAlignmentProperty);
			}
			set
			{
				base.SetValue(AnchoredBlock.TextAlignmentProperty, value);
			}
		}

		/// <summary>Gets or sets the height of each line of content. </summary>
		/// <returns>A double value specifying the height of line in device independent pixels.  <see cref="P:System.Windows.Documents.AnchoredBlock.LineHeight" /> must be equal to or greater than 0.0034 and equal to or less then 160000.A value of <see cref="F:System.Double.NaN" /> (equivalent to an attribute value of "Auto") causes the line height is determined automatically from the current font characteristics.  The default value is <see cref="F:System.Double.NaN" />.</returns>
		/// <exception cref="T:System.ArgumentException">Raised if an attempt is made to set <see cref="P:System.Windows.Controls.TextBlock.LineHeight" /> to a non-positive value.</exception>
		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x06002AD4 RID: 10964 RVA: 0x000C3C3D File Offset: 0x000C1E3D
		// (set) Token: 0x06002AD5 RID: 10965 RVA: 0x000C3C4F File Offset: 0x000C1E4F
		[TypeConverter(typeof(LengthConverter))]
		public double LineHeight
		{
			get
			{
				return (double)base.GetValue(AnchoredBlock.LineHeightProperty);
			}
			set
			{
				base.SetValue(AnchoredBlock.LineHeightProperty, value);
			}
		}

		/// <summary>Gets or sets the mechanism by which a line box is determined for each line of text within the text element. </summary>
		/// <returns>The mechanism by which a line box is determined for each line of text within the text element. The default value is <see cref="F:System.Windows.LineStackingStrategy.MaxHeight" />.</returns>
		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x06002AD6 RID: 10966 RVA: 0x000C3C62 File Offset: 0x000C1E62
		// (set) Token: 0x06002AD7 RID: 10967 RVA: 0x000C3C74 File Offset: 0x000C1E74
		public LineStackingStrategy LineStackingStrategy
		{
			get
			{
				return (LineStackingStrategy)base.GetValue(AnchoredBlock.LineStackingStrategyProperty);
			}
			set
			{
				base.SetValue(AnchoredBlock.LineStackingStrategyProperty, value);
			}
		}

		/// <summary>Returns a value that indicates whether or not the effective value of the <see cref="P:System.Windows.Documents.AnchoredBlock.Blocks" /> property should be serialized during serialization of an object deriving from <see cref="T:System.Windows.Documents.AnchoredBlock" />.</summary>
		/// <param name="manager">A serialization service manager object for this object.</param>
		/// <returns>
		///     true if the <see cref="P:System.Windows.Documents.AnchoredBlock.Blocks" /> property should be serialized; otherwise, false.</returns>
		/// <exception cref="T:System.NullReferenceException">Raised when <paramref name="manager" /> is null.</exception>
		// Token: 0x06002AD8 RID: 10968 RVA: 0x000C3C87 File Offset: 0x000C1E87
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBlocks(XamlDesignerSerializationManager manager)
		{
			return manager != null && manager.XmlWriter == null;
		}

		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x06002AD9 RID: 10969 RVA: 0x00016748 File Offset: 0x00014948
		internal override bool IsIMEStructuralElement
		{
			get
			{
				return true;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.AnchoredBlock.Margin" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.AnchoredBlock.Margin" /> dependency property.</returns>
		// Token: 0x04001C4F RID: 7247
		public static readonly DependencyProperty MarginProperty = Block.MarginProperty.AddOwner(typeof(AnchoredBlock), new FrameworkPropertyMetadata(new Thickness(double.NaN), FrameworkPropertyMetadataOptions.AffectsMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.AnchoredBlock.Padding" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.AnchoredBlock.Padding" /> dependency property.</returns>
		// Token: 0x04001C50 RID: 7248
		public static readonly DependencyProperty PaddingProperty = Block.PaddingProperty.AddOwner(typeof(AnchoredBlock), new FrameworkPropertyMetadata(new Thickness(double.NaN), FrameworkPropertyMetadataOptions.AffectsMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.AnchoredBlock.BorderThickness" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.AnchoredBlock.BorderThickness" /> dependency property.</returns>
		// Token: 0x04001C51 RID: 7249
		public static readonly DependencyProperty BorderThicknessProperty = Block.BorderThicknessProperty.AddOwner(typeof(AnchoredBlock), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.AnchoredBlock.BorderBrush" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.AnchoredBlock.BorderBrush" /> dependency property.</returns>
		// Token: 0x04001C52 RID: 7250
		public static readonly DependencyProperty BorderBrushProperty = Block.BorderBrushProperty.AddOwner(typeof(AnchoredBlock), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.AnchoredBlock.TextAlignment" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.AnchoredBlock.TextAlignment" /> dependency property.</returns>
		// Token: 0x04001C53 RID: 7251
		public static readonly DependencyProperty TextAlignmentProperty = Block.TextAlignmentProperty.AddOwner(typeof(AnchoredBlock));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.AnchoredBlock.LineHeight" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.AnchoredBlock.LineHeight" /> dependency property.</returns>
		// Token: 0x04001C54 RID: 7252
		public static readonly DependencyProperty LineHeightProperty = Block.LineHeightProperty.AddOwner(typeof(AnchoredBlock));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.AnchoredBlock.LineStackingStrategy" />  dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.AnchoredBlock.LineStackingStrategy" /> dependency property.</returns>
		// Token: 0x04001C55 RID: 7253
		public static readonly DependencyProperty LineStackingStrategyProperty = Block.LineStackingStrategyProperty.AddOwner(typeof(AnchoredBlock));
	}
}
