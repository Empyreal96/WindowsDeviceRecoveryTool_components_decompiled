using System;
using System.ComponentModel;
using System.Windows.Media;
using MS.Internal.Text;

namespace System.Windows.Documents
{
	/// <summary>An abstract class that provides a base for all block-level flow content elements.</summary>
	// Token: 0x0200032D RID: 813
	public abstract class Block : TextElement
	{
		/// <summary>Gets a collection of <see cref="T:System.Windows.Documents.Block" /> elements that are siblings to the current <see cref="T:System.Windows.Documents.Block" /> element.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.BlockCollection" /> that contains the child <see cref="T:System.Windows.Documents.Block" /> elements that are directly hosted by the parent of the current <see cref="T:System.Windows.Documents.Block" /> element, or <see langword="null" /> if the current <see cref="T:System.Windows.Documents.Block" /> element has no parent.</returns>
		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x06002ADB RID: 10971 RVA: 0x000C3DA2 File Offset: 0x000C1FA2
		public BlockCollection SiblingBlocks
		{
			get
			{
				if (base.Parent == null)
				{
					return null;
				}
				return new BlockCollection(this, false);
			}
		}

		/// <summary>Gets the sibling <see cref="T:System.Windows.Documents.Block" /> element that directly follows the current <see cref="T:System.Windows.Documents.Block" /> element.</summary>
		/// <returns>The sibling <see cref="T:System.Windows.Documents.Block" /> element that directly follows the current <see cref="T:System.Windows.Documents.Block" /> element, or <see langword="null" /> if no such element exists.</returns>
		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x06002ADC RID: 10972 RVA: 0x000C3DB5 File Offset: 0x000C1FB5
		public Block NextBlock
		{
			get
			{
				return base.NextElement as Block;
			}
		}

		/// <summary>Gets the sibling <see cref="T:System.Windows.Documents.Block" /> element that directly precedes the current <see cref="T:System.Windows.Documents.Block" /> element.</summary>
		/// <returns>The sibling <see cref="T:System.Windows.Documents.Block" /> element that directly precedes the current <see cref="T:System.Windows.Documents.Block" /> element, or <see langword="null" /> if no such element exists.</returns>
		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x06002ADD RID: 10973 RVA: 0x000C3DC2 File Offset: 0x000C1FC2
		public Block PreviousBlock
		{
			get
			{
				return base.PreviousElement as Block;
			}
		}

		/// <summary>Gets or sets a value that indicates whether automatic hyphenation of words is enabled or disabled.  </summary>
		/// <returns>
		///     <see langword="true" /> if automatic breaking and hyphenation of words is enabled; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x06002ADE RID: 10974 RVA: 0x000C3DCF File Offset: 0x000C1FCF
		// (set) Token: 0x06002ADF RID: 10975 RVA: 0x000C3DE1 File Offset: 0x000C1FE1
		public bool IsHyphenationEnabled
		{
			get
			{
				return (bool)base.GetValue(Block.IsHyphenationEnabledProperty);
			}
			set
			{
				base.SetValue(Block.IsHyphenationEnabledProperty, value);
			}
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Block.IsHyphenationEnabled" /> attached property on a specified dependency object.</summary>
		/// <param name="element">The dependency object on which to set the value of the <see cref="P:System.Windows.Documents.Block.IsHyphenationEnabled" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06002AE0 RID: 10976 RVA: 0x000C3DEF File Offset: 0x000C1FEF
		public static void SetIsHyphenationEnabled(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Block.IsHyphenationEnabledProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Block.IsHyphenationEnabled" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object from which to retrieve the value of the <see cref="P:System.Windows.Documents.Block.IsHyphenationEnabled" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Block.IsHyphenationEnabled" /> attached property on the specified dependency object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06002AE1 RID: 10977 RVA: 0x000C3E0B File Offset: 0x000C200B
		public static bool GetIsHyphenationEnabled(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Block.IsHyphenationEnabledProperty);
		}

		/// <summary>Gets or sets the margin thickness for the element.  </summary>
		/// <returns>A <see cref="T:System.Windows.Thickness" /> structure that specifies the amount of margin to apply, in device independent pixels. The default is a uniform thickness of zero (0.0).</returns>
		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x06002AE2 RID: 10978 RVA: 0x000C3E2B File Offset: 0x000C202B
		// (set) Token: 0x06002AE3 RID: 10979 RVA: 0x000C3E3D File Offset: 0x000C203D
		public Thickness Margin
		{
			get
			{
				return (Thickness)base.GetValue(Block.MarginProperty);
			}
			set
			{
				base.SetValue(Block.MarginProperty, value);
			}
		}

		/// <summary>Gets or sets the padding thickness for the element.  </summary>
		/// <returns>A <see cref="T:System.Windows.Thickness" /> structure that specifies the amount of padding to apply, in device independent pixels. The default is a uniform thickness of zero (0.0).</returns>
		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x06002AE4 RID: 10980 RVA: 0x000C3E50 File Offset: 0x000C2050
		// (set) Token: 0x06002AE5 RID: 10981 RVA: 0x000C3E62 File Offset: 0x000C2062
		public Thickness Padding
		{
			get
			{
				return (Thickness)base.GetValue(Block.PaddingProperty);
			}
			set
			{
				base.SetValue(Block.PaddingProperty, value);
			}
		}

		/// <summary>Gets or sets the border thickness for the element.  </summary>
		/// <returns>A <see cref="T:System.Windows.Thickness" /> structure specifying the amount of border to apply, in device independent pixels. The default is a uniform thickness of zero (0.0).</returns>
		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x06002AE6 RID: 10982 RVA: 0x000C3E75 File Offset: 0x000C2075
		// (set) Token: 0x06002AE7 RID: 10983 RVA: 0x000C3E87 File Offset: 0x000C2087
		public Thickness BorderThickness
		{
			get
			{
				return (Thickness)base.GetValue(Block.BorderThicknessProperty);
			}
			set
			{
				base.SetValue(Block.BorderThicknessProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Media.Brush" /> to use when painting the element's border.  </summary>
		/// <returns>The brush used to apply to the element's border. The default is <see langword="null" />.</returns>
		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x06002AE8 RID: 10984 RVA: 0x000C3E9A File Offset: 0x000C209A
		// (set) Token: 0x06002AE9 RID: 10985 RVA: 0x000C3EAC File Offset: 0x000C20AC
		public Brush BorderBrush
		{
			get
			{
				return (Brush)base.GetValue(Block.BorderBrushProperty);
			}
			set
			{
				base.SetValue(Block.BorderBrushProperty, value);
			}
		}

		/// <summary>Gets or sets the horizontal alignment of text content.  </summary>
		/// <returns>One of the <see cref="T:System.Windows.TextAlignment" /> values that specifies the desired alignment. The default is <see cref="F:System.Windows.TextAlignment.Left" />.</returns>
		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x06002AEA RID: 10986 RVA: 0x000C3EBA File Offset: 0x000C20BA
		// (set) Token: 0x06002AEB RID: 10987 RVA: 0x000C3ECC File Offset: 0x000C20CC
		public TextAlignment TextAlignment
		{
			get
			{
				return (TextAlignment)base.GetValue(Block.TextAlignmentProperty);
			}
			set
			{
				base.SetValue(Block.TextAlignmentProperty, value);
			}
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Block.TextAlignment" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object on which to set the value of the <see cref="P:System.Windows.Documents.Block.TextAlignment" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06002AEC RID: 10988 RVA: 0x000C3EDF File Offset: 0x000C20DF
		public static void SetTextAlignment(DependencyObject element, TextAlignment value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Block.TextAlignmentProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Block.TextAlignment" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object from which to retrieve the value of the <see cref="P:System.Windows.Documents.Block.TextAlignment" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Block.TextAlignment" /> attached property on the specified dependency object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06002AED RID: 10989 RVA: 0x000C3F00 File Offset: 0x000C2100
		public static TextAlignment GetTextAlignment(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (TextAlignment)element.GetValue(Block.TextAlignmentProperty);
		}

		/// <summary>Gets or sets the relative direction for flow of content within a <see cref="T:System.Windows.Documents.Block" /> element.  </summary>
		/// <returns>One of the <see cref="T:System.Windows.FlowDirection" /> values that specifies the relative flow direction. The default is <see cref="F:System.Windows.FlowDirection.LeftToRight" />.</returns>
		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x06002AEE RID: 10990 RVA: 0x000C3F20 File Offset: 0x000C2120
		// (set) Token: 0x06002AEF RID: 10991 RVA: 0x000C3F32 File Offset: 0x000C2132
		public FlowDirection FlowDirection
		{
			get
			{
				return (FlowDirection)base.GetValue(Block.FlowDirectionProperty);
			}
			set
			{
				base.SetValue(Block.FlowDirectionProperty, value);
			}
		}

		/// <summary>Gets or sets the height of each line of content.  </summary>
		/// <returns>The height of each line in device independent pixels, in the range of 0.0034 to 160000, or <see cref="F:System.Double.NaN" /> to determine the height automatically. The default is <see cref="F:System.Double.NaN" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <see cref="P:System.Windows.Controls.TextBlock.LineHeight" /> is set to a non-positive value.</exception>
		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x06002AF0 RID: 10992 RVA: 0x000C3F45 File Offset: 0x000C2145
		// (set) Token: 0x06002AF1 RID: 10993 RVA: 0x000C3F57 File Offset: 0x000C2157
		[TypeConverter(typeof(LengthConverter))]
		public double LineHeight
		{
			get
			{
				return (double)base.GetValue(Block.LineHeightProperty);
			}
			set
			{
				base.SetValue(Block.LineHeightProperty, value);
			}
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Block.LineHeight" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object on which to set the value of the <see cref="P:System.Windows.Documents.Block.LineHeight" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="value" /> is negative.</exception>
		// Token: 0x06002AF2 RID: 10994 RVA: 0x000C3F6A File Offset: 0x000C216A
		public static void SetLineHeight(DependencyObject element, double value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Block.LineHeightProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Block.LineHeight" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object from which to retrieve the value of the <see cref="P:System.Windows.Documents.Block.LineHeight" /> property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Block.LineHeight" /> attached property on the specified dependency object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06002AF3 RID: 10995 RVA: 0x000C3F8B File Offset: 0x000C218B
		[TypeConverter(typeof(LengthConverter))]
		public static double GetLineHeight(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(Block.LineHeightProperty);
		}

		/// <summary>Gets or sets how a line box is determined for each line of text within the block-level flow content element.  </summary>
		/// <returns>One of the <see cref="T:System.Windows.LineStackingStrategy" /> values that specifies how a line box is determined for each line of text within the block-level flow content element. The default value is <see cref="F:System.Windows.LineStackingStrategy.MaxHeight" />.</returns>
		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x06002AF4 RID: 10996 RVA: 0x000C3FAB File Offset: 0x000C21AB
		// (set) Token: 0x06002AF5 RID: 10997 RVA: 0x000C3FBD File Offset: 0x000C21BD
		public LineStackingStrategy LineStackingStrategy
		{
			get
			{
				return (LineStackingStrategy)base.GetValue(Block.LineStackingStrategyProperty);
			}
			set
			{
				base.SetValue(Block.LineStackingStrategyProperty, value);
			}
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Documents.Block.LineStackingStrategy" /> attached property on a specified dependency object.</summary>
		/// <param name="element">The dependency object on which to set the value of the <see cref="P:System.Windows.Documents.Block.LineStackingStrategy" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06002AF6 RID: 10998 RVA: 0x000C3FD0 File Offset: 0x000C21D0
		public static void SetLineStackingStrategy(DependencyObject element, LineStackingStrategy value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Block.LineStackingStrategyProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Documents.Block.LineStackingStrategy" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object from which to retrieve the value of the <see cref="P:System.Windows.Documents.Block.LineStackingStrategy" /> attached property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Documents.Block.LineStackingStrategy" /> attached property on the specified dependency object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06002AF7 RID: 10999 RVA: 0x000C3FF1 File Offset: 0x000C21F1
		public static LineStackingStrategy GetLineStackingStrategy(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (LineStackingStrategy)element.GetValue(Block.LineStackingStrategyProperty);
		}

		/// <summary>Gets or sets a value that indicates whether to automatically insert a page-break before this element.  </summary>
		/// <returns>
		///     <see langword="true" /> to automatically insert a page-break before this element; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x06002AF8 RID: 11000 RVA: 0x000C4011 File Offset: 0x000C2211
		// (set) Token: 0x06002AF9 RID: 11001 RVA: 0x000C4023 File Offset: 0x000C2223
		public bool BreakPageBefore
		{
			get
			{
				return (bool)base.GetValue(Block.BreakPageBeforeProperty);
			}
			set
			{
				base.SetValue(Block.BreakPageBeforeProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether to automatically insert a column-break before this element in cases where the element participates in a column-based presentation.  </summary>
		/// <returns>
		///     <see langword="true" /> to automatically insert a column-break before this element; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x06002AFA RID: 11002 RVA: 0x000C4031 File Offset: 0x000C2231
		// (set) Token: 0x06002AFB RID: 11003 RVA: 0x000C4043 File Offset: 0x000C2243
		public bool BreakColumnBefore
		{
			get
			{
				return (bool)base.GetValue(Block.BreakColumnBeforeProperty);
			}
			set
			{
				base.SetValue(Block.BreakColumnBeforeProperty, value);
			}
		}

		/// <summary>Gets or sets the direction in which any <see cref="T:System.Windows.Documents.Floater" /> elements contained by a <see cref="T:System.Windows.Documents.Block" /> element should be repositioned.  </summary>
		/// <returns>One of the <see cref="T:System.Windows.WrapDirection" /> values that specifies the direction in which to separate any <see cref="T:System.Windows.Documents.Floater" /> elements from other content contained in the <see cref="T:System.Windows.Documents.Block" /> element. The default is <see cref="F:System.Windows.WrapDirection.None" />, which indicates that floaters should be rendered in place.</returns>
		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x06002AFC RID: 11004 RVA: 0x000C4051 File Offset: 0x000C2251
		// (set) Token: 0x06002AFD RID: 11005 RVA: 0x000C4063 File Offset: 0x000C2263
		public WrapDirection ClearFloaters
		{
			get
			{
				return (WrapDirection)base.GetValue(Block.ClearFloatersProperty);
			}
			set
			{
				base.SetValue(Block.ClearFloatersProperty, value);
			}
		}

		// Token: 0x06002AFE RID: 11006 RVA: 0x000C4078 File Offset: 0x000C2278
		internal static bool IsValidMargin(object o)
		{
			Thickness t = (Thickness)o;
			return Block.IsValidThickness(t, true);
		}

		// Token: 0x06002AFF RID: 11007 RVA: 0x000C4094 File Offset: 0x000C2294
		internal static bool IsValidPadding(object o)
		{
			Thickness t = (Thickness)o;
			return Block.IsValidThickness(t, true);
		}

		// Token: 0x06002B00 RID: 11008 RVA: 0x000C40B0 File Offset: 0x000C22B0
		internal static bool IsValidBorderThickness(object o)
		{
			Thickness t = (Thickness)o;
			return Block.IsValidThickness(t, false);
		}

		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x06002B01 RID: 11009 RVA: 0x00016748 File Offset: 0x00014948
		internal override bool IsIMEStructuralElement
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002B02 RID: 11010 RVA: 0x000C40CC File Offset: 0x000C22CC
		private static bool IsValidLineHeight(object o)
		{
			double num = (double)o;
			double minWidth = TextDpi.MinWidth;
			double num2 = (double)Math.Min(1000000, 160000);
			return double.IsNaN(num) || (num >= minWidth && num <= num2);
		}

		// Token: 0x06002B03 RID: 11011 RVA: 0x000C4110 File Offset: 0x000C2310
		private static bool IsValidLineStackingStrategy(object o)
		{
			LineStackingStrategy lineStackingStrategy = (LineStackingStrategy)o;
			return lineStackingStrategy == LineStackingStrategy.MaxHeight || lineStackingStrategy == LineStackingStrategy.BlockLineHeight;
		}

		// Token: 0x06002B04 RID: 11012 RVA: 0x000C4130 File Offset: 0x000C2330
		private static bool IsValidTextAlignment(object o)
		{
			TextAlignment textAlignment = (TextAlignment)o;
			return textAlignment == TextAlignment.Center || textAlignment == TextAlignment.Justify || textAlignment == TextAlignment.Left || textAlignment == TextAlignment.Right;
		}

		// Token: 0x06002B05 RID: 11013 RVA: 0x000C4158 File Offset: 0x000C2358
		private static bool IsValidWrapDirection(object o)
		{
			WrapDirection wrapDirection = (WrapDirection)o;
			return wrapDirection == WrapDirection.None || wrapDirection == WrapDirection.Left || wrapDirection == WrapDirection.Right || wrapDirection == WrapDirection.Both;
		}

		// Token: 0x06002B06 RID: 11014 RVA: 0x000C4180 File Offset: 0x000C2380
		internal static bool IsValidThickness(Thickness t, bool allowNaN)
		{
			double num = (double)Math.Min(1000000, 3500000);
			return (allowNaN || (!double.IsNaN(t.Left) && !double.IsNaN(t.Right) && !double.IsNaN(t.Top) && !double.IsNaN(t.Bottom))) && (double.IsNaN(t.Left) || (t.Left >= 0.0 && t.Left <= num)) && (double.IsNaN(t.Right) || (t.Right >= 0.0 && t.Right <= num)) && (double.IsNaN(t.Top) || (t.Top >= 0.0 && t.Top <= num)) && (double.IsNaN(t.Bottom) || (t.Bottom >= 0.0 && t.Bottom <= num));
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Block.IsHyphenationEnabled" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Block.IsHyphenationEnabled" /> dependency property.</returns>
		// Token: 0x04001C56 RID: 7254
		public static readonly DependencyProperty IsHyphenationEnabledProperty = DependencyProperty.RegisterAttached("IsHyphenationEnabled", typeof(bool), typeof(Block), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Block.Margin" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Block.Margin" /> dependency property.</returns>
		// Token: 0x04001C57 RID: 7255
		public static readonly DependencyProperty MarginProperty = DependencyProperty.Register("Margin", typeof(Thickness), typeof(Block), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(Block.IsValidMargin));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Block.Padding" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Block.Padding" /> dependency property.</returns>
		// Token: 0x04001C58 RID: 7256
		public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register("Padding", typeof(Thickness), typeof(Block), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(Block.IsValidPadding));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Block.BorderThickness" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Block.BorderThickness" /> dependency property.</returns>
		// Token: 0x04001C59 RID: 7257
		public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(Block), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(Block.IsValidBorderThickness));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Block.BorderBrush" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Block.BorderBrush" /> dependency property.</returns>
		// Token: 0x04001C5A RID: 7258
		public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(Block), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Block.TextAlignment" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Block.TextAlignment" /> dependency property.</returns>
		// Token: 0x04001C5B RID: 7259
		public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.RegisterAttached("TextAlignment", typeof(TextAlignment), typeof(Block), new FrameworkPropertyMetadata(TextAlignment.Left, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits), new ValidateValueCallback(Block.IsValidTextAlignment));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Block.FlowDirection" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Block.FlowDirection" /> dependency property.</returns>
		// Token: 0x04001C5C RID: 7260
		public static readonly DependencyProperty FlowDirectionProperty = FrameworkElement.FlowDirectionProperty.AddOwner(typeof(Block));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Block.LineHeight" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Block.LineHeight" /> dependency property.</returns>
		// Token: 0x04001C5D RID: 7261
		public static readonly DependencyProperty LineHeightProperty = DependencyProperty.RegisterAttached("LineHeight", typeof(double), typeof(Block), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits), new ValidateValueCallback(Block.IsValidLineHeight));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Block.LineStackingStrategy" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Block.LineStackingStrategy" /> dependency property.</returns>
		// Token: 0x04001C5E RID: 7262
		public static readonly DependencyProperty LineStackingStrategyProperty = DependencyProperty.RegisterAttached("LineStackingStrategy", typeof(LineStackingStrategy), typeof(Block), new FrameworkPropertyMetadata(LineStackingStrategy.MaxHeight, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits), new ValidateValueCallback(Block.IsValidLineStackingStrategy));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Block.BreakPageBefore" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Block.BreakPageBefore" /> dependency property.</returns>
		// Token: 0x04001C5F RID: 7263
		public static readonly DependencyProperty BreakPageBeforeProperty = DependencyProperty.Register("BreakPageBefore", typeof(bool), typeof(Block), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Block.BreakColumnBefore" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Block.BreakColumnBefore" /> dependency property.</returns>
		// Token: 0x04001C60 RID: 7264
		public static readonly DependencyProperty BreakColumnBeforeProperty = DependencyProperty.Register("BreakColumnBefore", typeof(bool), typeof(Block), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Block.ClearFloaters" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Block.ClearFloaters" /> dependency property.</returns>
		// Token: 0x04001C61 RID: 7265
		public static readonly DependencyProperty ClearFloatersProperty = DependencyProperty.Register("ClearFloaters", typeof(WrapDirection), typeof(Block), new FrameworkPropertyMetadata(WrapDirection.None, FrameworkPropertyMetadataOptions.AffectsParentMeasure), new ValidateValueCallback(Block.IsValidWrapDirection));
	}
}
