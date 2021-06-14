using System;
using System.ComponentModel;

namespace System.Windows.Documents
{
	/// <summary>An inline-level flow content element used to host a figure.  A figure is a portion of flow content with placement properties that can be customized independently from the primary content flow within a <see cref="T:System.Windows.Documents.FlowDocument" />.</summary>
	// Token: 0x02000342 RID: 834
	public class Figure : AnchoredBlock
	{
		/// <summary>Initializes a new, empty instance of the <see cref="T:System.Windows.Documents.Figure" /> class.</summary>
		// Token: 0x06002C99 RID: 11417 RVA: 0x000C95BC File Offset: 0x000C77BC
		public Figure() : this(null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Figure" /> class, taking a specified <see cref="T:System.Windows.Documents.Block" /> object as the initial contents of the new <see cref="T:System.Windows.Documents.Figure" />.</summary>
		/// <param name="childBlock">A <see cref="T:System.Windows.Documents.Block" /> object specifying the initial contents of the new <see cref="T:System.Windows.Documents.Figure" />.</param>
		// Token: 0x06002C9A RID: 11418 RVA: 0x000C95C5 File Offset: 0x000C77C5
		public Figure(Block childBlock) : this(childBlock, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Figure" /> class, taking a specified <see cref="T:System.Windows.Documents.Block" /> object as the initial contents of the new <see cref="T:System.Windows.Documents.Figure" />, and a <see cref="T:System.Windows.Documents.TextPointer" /> specifying an insertion position for the new <see cref="T:System.Windows.Documents.Figure" /> element.</summary>
		/// <param name="childBlock">A <see cref="T:System.Windows.Documents.Block" /> object specifying the initial contents of the new <see cref="T:System.Windows.Documents.Figure" />.  This parameter may be <see langword="null" />, in which case no <see cref="T:System.Windows.Documents.Block" /> is inserted.</param>
		/// <param name="insertionPosition">A <see cref="T:System.Windows.Documents.TextPointer" /> specifying an insertion position at which to insert the <see cref="T:System.Windows.Documents.Figure" /> element after it is created, or <see langword="null" /> for no automatic insertion.</param>
		// Token: 0x06002C9B RID: 11419 RVA: 0x000C95CF File Offset: 0x000C77CF
		public Figure(Block childBlock, TextPointer insertionPosition) : base(childBlock, insertionPosition)
		{
		}

		/// <summary>Gets or sets a value that indicates the position that content is anchored to in the horizontal direction.  </summary>
		/// <returns>A member of the <see cref="T:System.Windows.FigureHorizontalAnchor" /> enumeration specifying a horizontal anchor location for the <see cref="T:System.Windows.Documents.Figure" />.The default value is <see cref="F:System.Windows.FigureHorizontalAnchor.ColumnRight" />.</returns>
		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x06002C9C RID: 11420 RVA: 0x000C95D9 File Offset: 0x000C77D9
		// (set) Token: 0x06002C9D RID: 11421 RVA: 0x000C95EB File Offset: 0x000C77EB
		public FigureHorizontalAnchor HorizontalAnchor
		{
			get
			{
				return (FigureHorizontalAnchor)base.GetValue(Figure.HorizontalAnchorProperty);
			}
			set
			{
				base.SetValue(Figure.HorizontalAnchorProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates the position that content is anchored to in the vertical direction.  </summary>
		/// <returns>A member of the <see cref="T:System.Windows.FigureVerticalAnchor" /> enumeration specifying a vertical anchor location for the <see cref="T:System.Windows.Documents.Figure" />.The default value is <see cref="F:System.Windows.FigureVerticalAnchor.ParagraphTop" />.</returns>
		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x06002C9E RID: 11422 RVA: 0x000C95FE File Offset: 0x000C77FE
		// (set) Token: 0x06002C9F RID: 11423 RVA: 0x000C9610 File Offset: 0x000C7810
		public FigureVerticalAnchor VerticalAnchor
		{
			get
			{
				return (FigureVerticalAnchor)base.GetValue(Figure.VerticalAnchorProperty);
			}
			set
			{
				base.SetValue(Figure.VerticalAnchorProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates the distance that a <see cref="T:System.Windows.Documents.Figure" /> is offset from its baseline in the horizontal direction.  </summary>
		/// <returns>The distance that a <see cref="T:System.Windows.Documents.Figure" /> is offset from its baseline in the horizontal direction, in device independent pixels.The default value is 0.0.</returns>
		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x06002CA0 RID: 11424 RVA: 0x000C9623 File Offset: 0x000C7823
		// (set) Token: 0x06002CA1 RID: 11425 RVA: 0x000C9635 File Offset: 0x000C7835
		[TypeConverter(typeof(LengthConverter))]
		public double HorizontalOffset
		{
			get
			{
				return (double)base.GetValue(Figure.HorizontalOffsetProperty);
			}
			set
			{
				base.SetValue(Figure.HorizontalOffsetProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates the distance that a <see cref="T:System.Windows.Documents.Figure" /> is offset from its baseline in the vertical direction.  </summary>
		/// <returns>The distance that a <see cref="T:System.Windows.Documents.Figure" /> is offset from its baseline in the vertical direction, in device independent pixels.The default value is 0.0.</returns>
		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x06002CA2 RID: 11426 RVA: 0x000C9648 File Offset: 0x000C7848
		// (set) Token: 0x06002CA3 RID: 11427 RVA: 0x000C965A File Offset: 0x000C785A
		[TypeConverter(typeof(LengthConverter))]
		public double VerticalOffset
		{
			get
			{
				return (double)base.GetValue(Figure.VerticalOffsetProperty);
			}
			set
			{
				base.SetValue(Figure.VerticalOffsetProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether this figure can delay its placement in the flow of content.  </summary>
		/// <returns>
		///     true if this figure can delay placement; otherwise, false.The default value is true.</returns>
		// Token: 0x17000B16 RID: 2838
		// (get) Token: 0x06002CA4 RID: 11428 RVA: 0x000C966D File Offset: 0x000C786D
		// (set) Token: 0x06002CA5 RID: 11429 RVA: 0x000C967F File Offset: 0x000C787F
		public bool CanDelayPlacement
		{
			get
			{
				return (bool)base.GetValue(Figure.CanDelayPlacementProperty);
			}
			set
			{
				base.SetValue(Figure.CanDelayPlacementProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates the allowable ways in which content can flow around a <see cref="T:System.Windows.Documents.Figure" />.   </summary>
		/// <returns>A member of the <see cref="T:System.Windows.WrapDirection" /> enumeration specifying the allowable ways in which content can flow around a <see cref="T:System.Windows.Documents.Figure" />.The default value is <see cref="F:System.Windows.WrapDirection.Both" />.</returns>
		// Token: 0x17000B17 RID: 2839
		// (get) Token: 0x06002CA6 RID: 11430 RVA: 0x000C968D File Offset: 0x000C788D
		// (set) Token: 0x06002CA7 RID: 11431 RVA: 0x000C969F File Offset: 0x000C789F
		public WrapDirection WrapDirection
		{
			get
			{
				return (WrapDirection)base.GetValue(Figure.WrapDirectionProperty);
			}
			set
			{
				base.SetValue(Figure.WrapDirectionProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates the width of a <see cref="T:System.Windows.Documents.Figure" /> element.  </summary>
		/// <returns>A <see cref="T:System.Windows.FigureLength" /> structure specifying the width characteristics for the <see cref="T:System.Windows.Documents.Figure" />.The default value is <see cref="T:System.Windows.FigureLength" />.<see cref="P:System.Windows.FigureLength.Value" /> = 1.0 and <see cref="T:System.Windows.FigureLength" />.<see cref="P:System.Windows.FigureLength.FigureUnitType" /> = <see cref="F:System.Windows.FigureUnitType.Auto" />.</returns>
		// Token: 0x17000B18 RID: 2840
		// (get) Token: 0x06002CA8 RID: 11432 RVA: 0x000C96B2 File Offset: 0x000C78B2
		// (set) Token: 0x06002CA9 RID: 11433 RVA: 0x000C96C4 File Offset: 0x000C78C4
		public FigureLength Width
		{
			get
			{
				return (FigureLength)base.GetValue(Figure.WidthProperty);
			}
			set
			{
				base.SetValue(Figure.WidthProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates the height of a <see cref="T:System.Windows.Documents.Figure" /> element.  </summary>
		/// <returns>A <see cref="T:System.Windows.FigureLength" /> structure specifying the height characteristics for the <see cref="T:System.Windows.Documents.Figure" />.The default value is <see cref="T:System.Windows.FigureLength" />.<see cref="P:System.Windows.FigureLength.Value" /> = 1.0 and <see cref="T:System.Windows.FigureLength" />.<see cref="P:System.Windows.FigureLength.FigureUnitType" /> = <see cref="F:System.Windows.FigureUnitType.Auto" />.</returns>
		// Token: 0x17000B19 RID: 2841
		// (get) Token: 0x06002CAA RID: 11434 RVA: 0x000C96D7 File Offset: 0x000C78D7
		// (set) Token: 0x06002CAB RID: 11435 RVA: 0x000C96E9 File Offset: 0x000C78E9
		public FigureLength Height
		{
			get
			{
				return (FigureLength)base.GetValue(Figure.HeightProperty);
			}
			set
			{
				base.SetValue(Figure.HeightProperty, value);
			}
		}

		// Token: 0x06002CAC RID: 11436 RVA: 0x000C96FC File Offset: 0x000C78FC
		private static bool IsValidHorizontalAnchor(object o)
		{
			FigureHorizontalAnchor figureHorizontalAnchor = (FigureHorizontalAnchor)o;
			return figureHorizontalAnchor == FigureHorizontalAnchor.ContentCenter || figureHorizontalAnchor == FigureHorizontalAnchor.ContentLeft || figureHorizontalAnchor == FigureHorizontalAnchor.ContentRight || figureHorizontalAnchor == FigureHorizontalAnchor.PageCenter || figureHorizontalAnchor == FigureHorizontalAnchor.PageLeft || figureHorizontalAnchor == FigureHorizontalAnchor.PageRight || figureHorizontalAnchor == FigureHorizontalAnchor.ColumnCenter || figureHorizontalAnchor == FigureHorizontalAnchor.ColumnLeft || figureHorizontalAnchor == FigureHorizontalAnchor.ColumnRight;
		}

		// Token: 0x06002CAD RID: 11437 RVA: 0x000C9738 File Offset: 0x000C7938
		private static bool IsValidVerticalAnchor(object o)
		{
			FigureVerticalAnchor figureVerticalAnchor = (FigureVerticalAnchor)o;
			return figureVerticalAnchor == FigureVerticalAnchor.ContentBottom || figureVerticalAnchor == FigureVerticalAnchor.ContentCenter || figureVerticalAnchor == FigureVerticalAnchor.ContentTop || figureVerticalAnchor == FigureVerticalAnchor.PageBottom || figureVerticalAnchor == FigureVerticalAnchor.PageCenter || figureVerticalAnchor == FigureVerticalAnchor.PageTop || figureVerticalAnchor == FigureVerticalAnchor.ParagraphTop;
		}

		// Token: 0x06002CAE RID: 11438 RVA: 0x000C976C File Offset: 0x000C796C
		private static bool IsValidWrapDirection(object o)
		{
			WrapDirection wrapDirection = (WrapDirection)o;
			return wrapDirection == WrapDirection.Both || wrapDirection == WrapDirection.None || wrapDirection == WrapDirection.Left || wrapDirection == WrapDirection.Right;
		}

		// Token: 0x06002CAF RID: 11439 RVA: 0x000C9794 File Offset: 0x000C7994
		private static bool IsValidOffset(object o)
		{
			double num = (double)o;
			double num2 = (double)Math.Min(1000000, 3500000);
			double num3 = -num2;
			return !double.IsNaN(num) && num >= num3 && num <= num2;
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Figure.HorizontalAnchor" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Figure.HorizontalAnchor" /> dependency property.</returns>
		// Token: 0x04001D2A RID: 7466
		public static readonly DependencyProperty HorizontalAnchorProperty = DependencyProperty.Register("HorizontalAnchor", typeof(FigureHorizontalAnchor), typeof(Figure), new FrameworkPropertyMetadata(FigureHorizontalAnchor.ColumnRight, FrameworkPropertyMetadataOptions.AffectsParentMeasure), new ValidateValueCallback(Figure.IsValidHorizontalAnchor));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Figure.VerticalAnchor" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Figure.VerticalAnchor" /> dependency property.</returns>
		// Token: 0x04001D2B RID: 7467
		public static readonly DependencyProperty VerticalAnchorProperty = DependencyProperty.Register("VerticalAnchor", typeof(FigureVerticalAnchor), typeof(Figure), new FrameworkPropertyMetadata(FigureVerticalAnchor.ParagraphTop, FrameworkPropertyMetadataOptions.AffectsParentMeasure), new ValidateValueCallback(Figure.IsValidVerticalAnchor));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Figure.HorizontalOffset" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Figure.HorizontalOffset" /> dependency property.</returns>
		// Token: 0x04001D2C RID: 7468
		public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register("HorizontalOffset", typeof(double), typeof(Figure), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsParentMeasure), new ValidateValueCallback(Figure.IsValidOffset));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Figure.VerticalOffset" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Figure.VerticalOffset" /> dependency property.</returns>
		// Token: 0x04001D2D RID: 7469
		public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register("VerticalOffset", typeof(double), typeof(Figure), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsParentMeasure), new ValidateValueCallback(Figure.IsValidOffset));

		/// <summary> Identifies the <see cref="P:System.Windows.Documents.Figure.CanDelayPlacement" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Figure.CanDelayPlacement" /> dependency property.</returns>
		// Token: 0x04001D2E RID: 7470
		public static readonly DependencyProperty CanDelayPlacementProperty = DependencyProperty.Register("CanDelayPlacement", typeof(bool), typeof(Figure), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Figure.WrapDirection" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Figure.WrapDirection" /> dependency property.</returns>
		// Token: 0x04001D2F RID: 7471
		public static readonly DependencyProperty WrapDirectionProperty = DependencyProperty.Register("WrapDirection", typeof(WrapDirection), typeof(Figure), new FrameworkPropertyMetadata(WrapDirection.Both, FrameworkPropertyMetadataOptions.AffectsParentMeasure), new ValidateValueCallback(Figure.IsValidWrapDirection));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Figure.Width" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Figure.Width" /> dependency property.</returns>
		// Token: 0x04001D30 RID: 7472
		public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(FigureLength), typeof(Figure), new FrameworkPropertyMetadata(new FigureLength(1.0, FigureUnitType.Auto), FrameworkPropertyMetadataOptions.AffectsMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Figure.Height" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Figure.Height" /> dependency property.</returns>
		// Token: 0x04001D31 RID: 7473
		public static readonly DependencyProperty HeightProperty = DependencyProperty.Register("Height", typeof(FigureLength), typeof(Figure), new FrameworkPropertyMetadata(new FigureLength(1.0, FigureUnitType.Auto), FrameworkPropertyMetadataOptions.AffectsMeasure));
	}
}
