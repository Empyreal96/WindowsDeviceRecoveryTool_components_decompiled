using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Documents;
using MS.Internal.FontCache;
using MS.Internal.PtsHost;
using MS.Internal.Telemetry.PresentationFramework;
using MS.Internal.Text;

namespace System.Windows.Documents
{
	/// <summary>Hosts and formats flow content with advanced document features, such as pagination and columns.</summary>
	// Token: 0x0200036F RID: 879
	[Localizability(LocalizationCategory.Inherit, Readability = Readability.Unreadable)]
	[ContentProperty("Blocks")]
	public class FlowDocument : FrameworkContentElement, IDocumentPaginatorSource, IServiceProvider, IAddChild
	{
		// Token: 0x06002F12 RID: 12050 RVA: 0x000D4BC0 File Offset: 0x000D2DC0
		static FlowDocument()
		{
			PropertyChangedCallback propertyChangedCallback = new PropertyChangedCallback(FlowDocument.OnTypographyChanged);
			DependencyProperty[] typographyPropertiesList = Typography.TypographyPropertiesList;
			for (int i = 0; i < typographyPropertiesList.Length; i++)
			{
				typographyPropertiesList[i].OverrideMetadata(FlowDocument._typeofThis, new FrameworkPropertyMetadata(propertyChangedCallback));
			}
			FrameworkContentElement.DefaultStyleKeyProperty.OverrideMetadata(FlowDocument._typeofThis, new FrameworkPropertyMetadata(FlowDocument._typeofThis));
			ContentElement.FocusableProperty.OverrideMetadata(FlowDocument._typeofThis, new FrameworkPropertyMetadata(true));
			ControlsTraceLogger.AddControl(TelemetryControls.FlowDocument);
		}

		/// <summary>Initializes a new, empty instance of the <see cref="T:System.Windows.Documents.FlowDocument" /> class.</summary>
		// Token: 0x06002F13 RID: 12051 RVA: 0x000D50D1 File Offset: 0x000D32D1
		public FlowDocument()
		{
			this.Initialize(null);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.FlowDocument" /> class, adding a specified <see cref="T:System.Windows.Documents.Block" /> element as the initial content. </summary>
		/// <param name="block">An object deriving from the abstract <see cref="T:System.Windows.Documents.Block" /> class, to be added as the initial content.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="block" /> is <see langword="null" />.</exception>
		// Token: 0x06002F14 RID: 12052 RVA: 0x000D50F3 File Offset: 0x000D32F3
		public FlowDocument(Block block)
		{
			this.Initialize(null);
			if (block == null)
			{
				throw new ArgumentNullException("block");
			}
			this.Blocks.Add(block);
		}

		// Token: 0x06002F15 RID: 12053 RVA: 0x000D512F File Offset: 0x000D332F
		internal FlowDocument(TextContainer textContainer)
		{
			this.Initialize(textContainer);
		}

		/// <summary>Gets the top-level <see cref="T:System.Windows.Documents.Block" /> elements of the contents of the <see cref="T:System.Windows.Documents.FlowDocument" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.BlockCollection" /> containing the <see cref="T:System.Windows.Documents.Block" /> elements that make up the contents of the <see cref="T:System.Windows.Documents.FlowDocument" />.  </returns>
		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x06002F16 RID: 12054 RVA: 0x000C3B80 File Offset: 0x000C1D80
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public BlockCollection Blocks
		{
			get
			{
				return new BlockCollection(this, true);
			}
		}

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x06002F17 RID: 12055 RVA: 0x000D5151 File Offset: 0x000D3351
		internal TextRange TextRange
		{
			get
			{
				return new TextRange(this.ContentStart, this.ContentEnd);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Documents.TextPointer" /> that represents the start of content within a <see cref="T:System.Windows.Documents.FlowDocument" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.TextPointerContext" /> representing the start of the contents in the <see cref="T:System.Windows.Documents.FlowDocument" />.</returns>
		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x06002F18 RID: 12056 RVA: 0x000D5164 File Offset: 0x000D3364
		public TextPointer ContentStart
		{
			get
			{
				return this._structuralCache.TextContainer.Start;
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Documents.TextPointer" /> that represents the end of the content in a <see cref="T:System.Windows.Documents.FlowDocument" />. </summary>
		/// <returns>A <see cref="T:System.Windows.Documents.TextPointer" /> representing the end of the contents in the <see cref="T:System.Windows.Documents.FlowDocument" />.</returns>
		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x06002F19 RID: 12057 RVA: 0x000D5176 File Offset: 0x000D3376
		public TextPointer ContentEnd
		{
			get
			{
				return this._structuralCache.TextContainer.End;
			}
		}

		/// <summary>Gets or sets the preferred top-level font family for the <see cref="T:System.Windows.Documents.FlowDocument" />.  </summary>
		/// <returns>A <see cref="T:System.Windows.Media.FontFamily" /> object specifying the preferred font family, or a primary preferred font family with one or more fallback font families. The default is the font determined by the <see cref="P:System.Windows.SystemFonts.MessageFontFamily" /> value.</returns>
		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x06002F1A RID: 12058 RVA: 0x000D5188 File Offset: 0x000D3388
		// (set) Token: 0x06002F1B RID: 12059 RVA: 0x000D519A File Offset: 0x000D339A
		[Localizability(LocalizationCategory.Font, Modifiability = Modifiability.Unmodifiable)]
		public FontFamily FontFamily
		{
			get
			{
				return (FontFamily)base.GetValue(FlowDocument.FontFamilyProperty);
			}
			set
			{
				base.SetValue(FlowDocument.FontFamilyProperty, value);
			}
		}

		/// <summary>Gets or sets the top-level font style for the <see cref="T:System.Windows.Documents.FlowDocument" />.  </summary>
		/// <returns>A member of the <see cref="T:System.Windows.FontStyles" /> class that specifies the desired font style. The default is determined by the <see cref="P:System.Windows.SystemFonts.MessageFontStyle" /> value.</returns>
		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x06002F1C RID: 12060 RVA: 0x000D51A8 File Offset: 0x000D33A8
		// (set) Token: 0x06002F1D RID: 12061 RVA: 0x000D51BA File Offset: 0x000D33BA
		public FontStyle FontStyle
		{
			get
			{
				return (FontStyle)base.GetValue(FlowDocument.FontStyleProperty);
			}
			set
			{
				base.SetValue(FlowDocument.FontStyleProperty, value);
			}
		}

		/// <summary>Gets or sets the top-level font weight for the <see cref="T:System.Windows.Documents.FlowDocument" />.  </summary>
		/// <returns>A member of the <see cref="T:System.Windows.FontWeights" /> class that specifies the desired font weight. The default is determined by the <see cref="P:System.Windows.SystemFonts.MessageFontWeight" /> value.</returns>
		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x06002F1E RID: 12062 RVA: 0x000D51CD File Offset: 0x000D33CD
		// (set) Token: 0x06002F1F RID: 12063 RVA: 0x000D51DF File Offset: 0x000D33DF
		public FontWeight FontWeight
		{
			get
			{
				return (FontWeight)base.GetValue(FlowDocument.FontWeightProperty);
			}
			set
			{
				base.SetValue(FlowDocument.FontWeightProperty, value);
			}
		}

		/// <summary>Gets or sets the top-level font-stretching characteristics for the <see cref="T:System.Windows.Documents.FlowDocument" />.  </summary>
		/// <returns>A member of the <see cref="T:System.Windows.FontStretch" /> class that specifies the desired font-stretching characteristics to use. The default is <see cref="P:System.Windows.FontStretches.Normal" />.</returns>
		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x06002F20 RID: 12064 RVA: 0x000D51F2 File Offset: 0x000D33F2
		// (set) Token: 0x06002F21 RID: 12065 RVA: 0x000D5204 File Offset: 0x000D3404
		public FontStretch FontStretch
		{
			get
			{
				return (FontStretch)base.GetValue(FlowDocument.FontStretchProperty);
			}
			set
			{
				base.SetValue(FlowDocument.FontStretchProperty, value);
			}
		}

		/// <summary>Gets or sets the top-level font size for the <see cref="T:System.Windows.Documents.FlowDocument" />.  </summary>
		/// <returns>The desired font size to use, in device independent pixels).   The default is determined by the <see cref="P:System.Windows.SystemFonts.MessageFontSize" /> value.</returns>
		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x06002F22 RID: 12066 RVA: 0x000D5217 File Offset: 0x000D3417
		// (set) Token: 0x06002F23 RID: 12067 RVA: 0x000D5229 File Offset: 0x000D3429
		[TypeConverter(typeof(FontSizeConverter))]
		[Localizability(LocalizationCategory.None)]
		public double FontSize
		{
			get
			{
				return (double)base.GetValue(FlowDocument.FontSizeProperty);
			}
			set
			{
				base.SetValue(FlowDocument.FontSizeProperty, value);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Media.Brush" /> to apply to the text contents of the <see cref="T:System.Windows.Documents.FlowDocument" />.  </summary>
		/// <returns>The brush used to apply to the text contents. The default is <see cref="P:System.Windows.Media.Brushes.Black" />.</returns>
		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x06002F24 RID: 12068 RVA: 0x000D523C File Offset: 0x000D343C
		// (set) Token: 0x06002F25 RID: 12069 RVA: 0x000D524E File Offset: 0x000D344E
		public Brush Foreground
		{
			get
			{
				return (Brush)base.GetValue(FlowDocument.ForegroundProperty);
			}
			set
			{
				base.SetValue(FlowDocument.ForegroundProperty, value);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Media.Brush" /> used to fill the background of content area.  </summary>
		/// <returns>The brush used to fill the background of the content area, or <see langword="null" /> to not use a background brush. The default is <see langword="null" />.</returns>
		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x06002F26 RID: 12070 RVA: 0x000D525C File Offset: 0x000D345C
		// (set) Token: 0x06002F27 RID: 12071 RVA: 0x000D526E File Offset: 0x000D346E
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(FlowDocument.BackgroundProperty);
			}
			set
			{
				base.SetValue(FlowDocument.BackgroundProperty, value);
			}
		}

		/// <summary>Gets or sets the effects to apply to the text of a <see cref="T:System.Windows.Documents.FlowDocument" />.  </summary>
		/// <returns>A <see cref="T:System.Windows.Media.TextEffectCollection" /> containing one or more <see cref="T:System.Windows.Media.TextEffect" /> objects that define effects to apply to the text of the <see cref="T:System.Windows.Documents.FlowDocument" />. The default is <see langword="null" /> (no effects applied).</returns>
		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x06002F28 RID: 12072 RVA: 0x000D527C File Offset: 0x000D347C
		// (set) Token: 0x06002F29 RID: 12073 RVA: 0x000D528E File Offset: 0x000D348E
		public TextEffectCollection TextEffects
		{
			get
			{
				return (TextEffectCollection)base.GetValue(FlowDocument.TextEffectsProperty);
			}
			set
			{
				base.SetValue(FlowDocument.TextEffectsProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates the horizontal alignment of text content.  </summary>
		/// <returns>One of the <see cref="T:System.Windows.TextAlignment" /> values that specifies the desired alignment. The default is <see cref="F:System.Windows.TextAlignment.Left" />.</returns>
		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x06002F2A RID: 12074 RVA: 0x000D529C File Offset: 0x000D349C
		// (set) Token: 0x06002F2B RID: 12075 RVA: 0x000D52AE File Offset: 0x000D34AE
		public TextAlignment TextAlignment
		{
			get
			{
				return (TextAlignment)base.GetValue(FlowDocument.TextAlignmentProperty);
			}
			set
			{
				base.SetValue(FlowDocument.TextAlignmentProperty, value);
			}
		}

		/// <summary>Gets or sets the relative direction for flow of content in a <see cref="T:System.Windows.Documents.FlowDocument" />.  </summary>
		/// <returns>One of the <see cref="T:System.Windows.FlowDirection" /> values that specifies the relative flow direction. The default is <see cref="F:System.Windows.FlowDirection.LeftToRight" />.</returns>
		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x06002F2C RID: 12076 RVA: 0x000D52C1 File Offset: 0x000D34C1
		// (set) Token: 0x06002F2D RID: 12077 RVA: 0x000D52D3 File Offset: 0x000D34D3
		public FlowDirection FlowDirection
		{
			get
			{
				return (FlowDirection)base.GetValue(FlowDocument.FlowDirectionProperty);
			}
			set
			{
				base.SetValue(FlowDocument.FlowDirectionProperty, value);
			}
		}

		/// <summary>Gets or sets the height of each line of content.  </summary>
		/// <returns>The height of each line, in device independent pixels, in the range 0.0034 to 160000. A value of <see cref="F:System.Double.NaN" /> (equivalent to an attribute value of "Auto") causes the line height to be determined automatically from the current font characteristics. The default is <see cref="F:System.Double.NaN" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <see cref="P:System.Windows.Controls.TextBlock.LineHeight" /> is set to a non-positive value.</exception>
		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x06002F2E RID: 12078 RVA: 0x000D52E6 File Offset: 0x000D34E6
		// (set) Token: 0x06002F2F RID: 12079 RVA: 0x000D52F8 File Offset: 0x000D34F8
		[TypeConverter(typeof(LengthConverter))]
		public double LineHeight
		{
			get
			{
				return (double)base.GetValue(FlowDocument.LineHeightProperty);
			}
			set
			{
				base.SetValue(FlowDocument.LineHeightProperty, value);
			}
		}

		/// <summary>Gets or sets the mechanism by which a line box is determined for each line of text within the <see cref="T:System.Windows.Documents.FlowDocument" />.  </summary>
		/// <returns>One of the <see cref="T:System.Windows.LineStackingStrategy" /> values that specifies the mechanism by which a line box is determined for each line of text in the <see cref="T:System.Windows.Documents.FlowDocument" />. The default is <see cref="F:System.Windows.LineStackingStrategy.MaxHeight" />.</returns>
		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x06002F30 RID: 12080 RVA: 0x000D530B File Offset: 0x000D350B
		// (set) Token: 0x06002F31 RID: 12081 RVA: 0x000D531D File Offset: 0x000D351D
		public LineStackingStrategy LineStackingStrategy
		{
			get
			{
				return (LineStackingStrategy)base.GetValue(FlowDocument.LineStackingStrategyProperty);
			}
			set
			{
				base.SetValue(FlowDocument.LineStackingStrategyProperty, value);
			}
		}

		/// <summary>Gets or sets the minimum desired width of the columns in a <see cref="T:System.Windows.Documents.FlowDocument" />.  </summary>
		/// <returns>The minimum desired column width, in device independent pixels. A value of <see cref="F:System.Double.NaN" /> causes only one column to be displayed, regardless of the page width.  The default is <see cref="F:System.Double.NaN" />.</returns>
		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x06002F32 RID: 12082 RVA: 0x000D5330 File Offset: 0x000D3530
		// (set) Token: 0x06002F33 RID: 12083 RVA: 0x000D5342 File Offset: 0x000D3542
		[TypeConverter(typeof(LengthConverter))]
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
		public double ColumnWidth
		{
			get
			{
				return (double)base.GetValue(FlowDocument.ColumnWidthProperty);
			}
			set
			{
				base.SetValue(FlowDocument.ColumnWidthProperty, value);
			}
		}

		/// <summary>Gets or sets the column gap value, which indicates the spacing between columns in a <see cref="T:System.Windows.Documents.FlowDocument" />.  </summary>
		/// <returns>The column gap, in device independent pixels.  A value of <see cref="F:System.Double.NaN" /> (equivalent to an attribute value of "Auto") indicates that the column gap is equal to the <see cref="P:System.Windows.Documents.FlowDocument.LineHeight" /> property. The default is <see cref="F:System.Double.NaN" />.</returns>
		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x06002F34 RID: 12084 RVA: 0x000D5355 File Offset: 0x000D3555
		// (set) Token: 0x06002F35 RID: 12085 RVA: 0x000D5367 File Offset: 0x000D3567
		[TypeConverter(typeof(LengthConverter))]
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
		public double ColumnGap
		{
			get
			{
				return (double)base.GetValue(FlowDocument.ColumnGapProperty);
			}
			set
			{
				base.SetValue(FlowDocument.ColumnGapProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="P:System.Windows.Documents.FlowDocument.ColumnWidth" /> value is flexible or fixed.  </summary>
		/// <returns>
		///     <see langword="true" /> if the column width is flexible; <see langword="false" /> if the column width is fixed. The default is <see langword="true" />.</returns>
		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x06002F36 RID: 12086 RVA: 0x000D537A File Offset: 0x000D357A
		// (set) Token: 0x06002F37 RID: 12087 RVA: 0x000D538C File Offset: 0x000D358C
		public bool IsColumnWidthFlexible
		{
			get
			{
				return (bool)base.GetValue(FlowDocument.IsColumnWidthFlexibleProperty);
			}
			set
			{
				base.SetValue(FlowDocument.IsColumnWidthFlexibleProperty, value);
			}
		}

		/// <summary>Gets or sets the column rule width.  </summary>
		/// <returns>The column rule width, in device independent pixels. The default is 0.0.</returns>
		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x06002F38 RID: 12088 RVA: 0x000D539A File Offset: 0x000D359A
		// (set) Token: 0x06002F39 RID: 12089 RVA: 0x000D53AC File Offset: 0x000D35AC
		[TypeConverter(typeof(LengthConverter))]
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
		public double ColumnRuleWidth
		{
			get
			{
				return (double)base.GetValue(FlowDocument.ColumnRuleWidthProperty);
			}
			set
			{
				base.SetValue(FlowDocument.ColumnRuleWidthProperty, value);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Media.Brush" /> used to draw the rule between columns.  </summary>
		/// <returns>A <see cref="T:System.Windows.Media.Brush" /> to use when drawing the rule line between columns, or <see langword="null" /> to not use a background brush. The default is <see langword="null" />.</returns>
		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x06002F3A RID: 12090 RVA: 0x000D53BF File Offset: 0x000D35BF
		// (set) Token: 0x06002F3B RID: 12091 RVA: 0x000D53D1 File Offset: 0x000D35D1
		public Brush ColumnRuleBrush
		{
			get
			{
				return (Brush)base.GetValue(FlowDocument.ColumnRuleBrushProperty);
			}
			set
			{
				base.SetValue(FlowDocument.ColumnRuleBrushProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether optimal paragraph layout is enabled or disabled.  </summary>
		/// <returns>
		///     <see langword="true" /> if optimal paragraph layout is enabled; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000BDF RID: 3039
		// (get) Token: 0x06002F3C RID: 12092 RVA: 0x000D53DF File Offset: 0x000D35DF
		// (set) Token: 0x06002F3D RID: 12093 RVA: 0x000D53F1 File Offset: 0x000D35F1
		public bool IsOptimalParagraphEnabled
		{
			get
			{
				return (bool)base.GetValue(FlowDocument.IsOptimalParagraphEnabledProperty);
			}
			set
			{
				base.SetValue(FlowDocument.IsOptimalParagraphEnabledProperty, value);
			}
		}

		/// <summary>Gets or sets the preferred width for pages in a <see cref="T:System.Windows.Documents.FlowDocument" />.  </summary>
		/// <returns>The preferred width, in device independent pixels, for pages in the <see cref="T:System.Windows.Documents.FlowDocument" />. A value of <see cref="F:System.Double.NaN" /> (equivalent to an attribute value of "Auto") causes the page width to be determined automatically. The default is <see cref="F:System.Double.NaN" />.</returns>
		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x06002F3E RID: 12094 RVA: 0x000D53FF File Offset: 0x000D35FF
		// (set) Token: 0x06002F3F RID: 12095 RVA: 0x000D5411 File Offset: 0x000D3611
		[TypeConverter(typeof(LengthConverter))]
		public double PageWidth
		{
			get
			{
				return (double)base.GetValue(FlowDocument.PageWidthProperty);
			}
			set
			{
				base.SetValue(FlowDocument.PageWidthProperty, value);
			}
		}

		/// <summary>Gets or sets the minimum width for pages in a <see cref="T:System.Windows.Documents.FlowDocument" />.  </summary>
		/// <returns>The minimum width, in device independent pixels, for pages in the <see cref="T:System.Windows.Documents.FlowDocument" />. The default is 0.0.</returns>
		// Token: 0x17000BE1 RID: 3041
		// (get) Token: 0x06002F40 RID: 12096 RVA: 0x000D5424 File Offset: 0x000D3624
		// (set) Token: 0x06002F41 RID: 12097 RVA: 0x000D5436 File Offset: 0x000D3636
		[TypeConverter(typeof(LengthConverter))]
		public double MinPageWidth
		{
			get
			{
				return (double)base.GetValue(FlowDocument.MinPageWidthProperty);
			}
			set
			{
				base.SetValue(FlowDocument.MinPageWidthProperty, value);
			}
		}

		/// <summary>Gets or sets the maximum width for pages in a <see cref="T:System.Windows.Documents.FlowDocument" />.  </summary>
		/// <returns>The maximum width, in device independent pixels, for pages in the <see cref="T:System.Windows.Documents.FlowDocument" />. The default is <see cref="F:System.Double.PositiveInfinity" /> (no maximum page width).</returns>
		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x06002F42 RID: 12098 RVA: 0x000D5449 File Offset: 0x000D3649
		// (set) Token: 0x06002F43 RID: 12099 RVA: 0x000D545B File Offset: 0x000D365B
		[TypeConverter(typeof(LengthConverter))]
		public double MaxPageWidth
		{
			get
			{
				return (double)base.GetValue(FlowDocument.MaxPageWidthProperty);
			}
			set
			{
				base.SetValue(FlowDocument.MaxPageWidthProperty, value);
			}
		}

		/// <summary>Gets or sets the preferred height for pages in a <see cref="T:System.Windows.Documents.FlowDocument" />.  </summary>
		/// <returns>The preferred height, in device independent pixels, for pages in the <see cref="T:System.Windows.Documents.FlowDocument" />. A value of <see cref="F:System.Double.NaN" /> (equivalent to an attribute value of "Auto") causes the page height to be determined automatically. The default is <see cref="F:System.Double.NaN" />.</returns>
		// Token: 0x17000BE3 RID: 3043
		// (get) Token: 0x06002F44 RID: 12100 RVA: 0x000D546E File Offset: 0x000D366E
		// (set) Token: 0x06002F45 RID: 12101 RVA: 0x000D5480 File Offset: 0x000D3680
		[TypeConverter(typeof(LengthConverter))]
		public double PageHeight
		{
			get
			{
				return (double)base.GetValue(FlowDocument.PageHeightProperty);
			}
			set
			{
				base.SetValue(FlowDocument.PageHeightProperty, value);
			}
		}

		/// <summary>Gets or sets the minimum height for pages in a <see cref="T:System.Windows.Documents.FlowDocument" />.  </summary>
		/// <returns>The minimum height, in device independent pixels, for pages in the <see cref="T:System.Windows.Documents.FlowDocument" />. The default is 0.0.</returns>
		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x06002F46 RID: 12102 RVA: 0x000D5493 File Offset: 0x000D3693
		// (set) Token: 0x06002F47 RID: 12103 RVA: 0x000D54A5 File Offset: 0x000D36A5
		[TypeConverter(typeof(LengthConverter))]
		public double MinPageHeight
		{
			get
			{
				return (double)base.GetValue(FlowDocument.MinPageHeightProperty);
			}
			set
			{
				base.SetValue(FlowDocument.MinPageHeightProperty, value);
			}
		}

		/// <summary>Gets or sets the maximum height for pages in a <see cref="T:System.Windows.Documents.FlowDocument" />.  </summary>
		/// <returns>The maximum height, in device independent pixels, for pages in the <see cref="T:System.Windows.Documents.FlowDocument" />. The default is <see cref="F:System.Double.PositiveInfinity" /> (no maximum page height).</returns>
		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x06002F48 RID: 12104 RVA: 0x000D54B8 File Offset: 0x000D36B8
		// (set) Token: 0x06002F49 RID: 12105 RVA: 0x000D54CA File Offset: 0x000D36CA
		[TypeConverter(typeof(LengthConverter))]
		public double MaxPageHeight
		{
			get
			{
				return (double)base.GetValue(FlowDocument.MaxPageHeightProperty);
			}
			set
			{
				base.SetValue(FlowDocument.MaxPageHeightProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates the thickness of padding space between the boundaries of a page and the page's content.  </summary>
		/// <returns>A <see cref="T:System.Windows.Thickness" /> structure that specifies the amount of padding to apply, in device independent pixels. The default is a uniform thickness of zero (0.0).</returns>
		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x06002F4A RID: 12106 RVA: 0x000D54DD File Offset: 0x000D36DD
		// (set) Token: 0x06002F4B RID: 12107 RVA: 0x000D54EF File Offset: 0x000D36EF
		public Thickness PagePadding
		{
			get
			{
				return (Thickness)base.GetValue(FlowDocument.PagePaddingProperty);
			}
			set
			{
				base.SetValue(FlowDocument.PagePaddingProperty, value);
			}
		}

		/// <summary>Gets the currently effective typography variations for the text contents of the <see cref="T:System.Windows.Documents.FlowDocument" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.Typography" /> object that specifies the currently effective typography variations. For a list of default typography values, see <see cref="T:System.Windows.Documents.Typography" />.</returns>
		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x06002F4C RID: 12108 RVA: 0x000D5502 File Offset: 0x000D3702
		public Typography Typography
		{
			get
			{
				return new Typography(this);
			}
		}

		/// <summary>Gets or sets a value that indicates whether automatic hyphenation of words is enabled or disabled.  </summary>
		/// <returns>
		///     <see langword="true" /> if automatic breaking and hyphenation of words is enabled; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000BE8 RID: 3048
		// (get) Token: 0x06002F4D RID: 12109 RVA: 0x000D550A File Offset: 0x000D370A
		// (set) Token: 0x06002F4E RID: 12110 RVA: 0x000D551C File Offset: 0x000D371C
		public bool IsHyphenationEnabled
		{
			get
			{
				return (bool)base.GetValue(FlowDocument.IsHyphenationEnabledProperty);
			}
			set
			{
				base.SetValue(FlowDocument.IsHyphenationEnabledProperty, value);
			}
		}

		/// <summary>Sets the DPI for the FlowDocument, causing it to be remeasured and rerendered.</summary>
		/// <param name="dpiInfo">The DPI setting, from which a <see cref="T:System.Windows.Media.Visual" /> or <see cref="T:System.Windows.UIElement" /> is rendered.</param>
		// Token: 0x06002F4F RID: 12111 RVA: 0x000D552C File Offset: 0x000D372C
		public void SetDpi(DpiScale dpiInfo)
		{
			if (dpiInfo.PixelsPerDip != this._pixelsPerDip)
			{
				this._pixelsPerDip = dpiInfo.PixelsPerDip;
				if (this.StructuralCache.HasPtsContext())
				{
					this.StructuralCache.TextFormatterHost.PixelsPerDip = this._pixelsPerDip;
				}
				IFlowDocumentFormatter formatter = this._formatter;
				if (formatter == null)
				{
					return;
				}
				formatter.OnContentInvalidated(true);
			}
		}

		/// <summary>Called when one or more of the dependency properties that exist on the element have had their effective values changed.</summary>
		/// <param name="e">Arguments for the associated event.</param>
		// Token: 0x06002F50 RID: 12112 RVA: 0x000D558C File Offset: 0x000D378C
		protected sealed override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if ((e.IsAValueChange || e.IsASubPropertyChange) && this._structuralCache != null && this._structuralCache.IsFormattedOnce)
			{
				FrameworkPropertyMetadata frameworkPropertyMetadata = e.Metadata as FrameworkPropertyMetadata;
				if (frameworkPropertyMetadata != null)
				{
					bool flag = frameworkPropertyMetadata.AffectsRender && (e.IsAValueChange || !frameworkPropertyMetadata.SubPropertiesDoNotAffectRender);
					if (frameworkPropertyMetadata.AffectsMeasure || frameworkPropertyMetadata.AffectsArrange || flag || frameworkPropertyMetadata.AffectsParentMeasure || frameworkPropertyMetadata.AffectsParentArrange)
					{
						if (this._structuralCache.IsFormattingInProgress)
						{
							this._structuralCache.OnInvalidOperationDetected();
							throw new InvalidOperationException(SR.Get("FlowDocumentInvalidContnetChange"));
						}
						this._structuralCache.InvalidateFormatCache(!flag);
						if (this._formatter != null)
						{
							this._formatter.OnContentInvalidated(!flag);
						}
					}
				}
			}
		}

		/// <summary>When overridden in a derived class, provides specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> implementations to the Windows Presentation Foundation (WPF) infrastructure.</summary>
		/// <returns>The type-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> implementation.</returns>
		// Token: 0x06002F51 RID: 12113 RVA: 0x000C6AA4 File Offset: 0x000C4CA4
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DocumentAutomationPeer(this);
		}

		/// <summary>Gets an enumerator that can iterate the logical children of the <see cref="T:System.Windows.Documents.FlowDocument" />. </summary>
		/// <returns>An enumerator for the logical children.</returns>
		// Token: 0x17000BE9 RID: 3049
		// (get) Token: 0x06002F52 RID: 12114 RVA: 0x000D5677 File Offset: 0x000D3877
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				return new RangeContentEnumerator(this._structuralCache.TextContainer.Start, this._structuralCache.TextContainer.End);
			}
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.ContentElement.IsEnabled" /> property for the <see cref="T:System.Windows.Documents.FlowDocument" />.</summary>
		/// <returns>The value of the <see cref="P:System.Windows.ContentElement.IsEnabled" /> property for the <see cref="T:System.Windows.Documents.FlowDocument" />.</returns>
		// Token: 0x17000BEA RID: 3050
		// (get) Token: 0x06002F53 RID: 12115 RVA: 0x000D56A0 File Offset: 0x000D38A0
		protected override bool IsEnabledCore
		{
			get
			{
				if (!base.IsEnabledCore)
				{
					return false;
				}
				RichTextBox richTextBox = base.Parent as RichTextBox;
				return richTextBox == null || richTextBox.IsDocumentEnabled;
			}
		}

		// Token: 0x06002F54 RID: 12116 RVA: 0x000D56D0 File Offset: 0x000D38D0
		internal ContentPosition GetObjectPosition(object element)
		{
			ITextPointer textPointer = null;
			if (element == this)
			{
				textPointer = this.ContentStart;
			}
			else if (element is TextElement)
			{
				textPointer = ((TextElement)element).ContentStart;
			}
			else if (element is FrameworkElement)
			{
				DependencyObject dependencyObject = null;
				while (element is FrameworkElement)
				{
					dependencyObject = LogicalTreeHelper.GetParent((DependencyObject)element);
					if (dependencyObject == null)
					{
						dependencyObject = VisualTreeHelper.GetParent((Visual)element);
					}
					if (!(dependencyObject is FrameworkElement))
					{
						break;
					}
					element = dependencyObject;
				}
				if (dependencyObject is BlockUIContainer || dependencyObject is InlineUIContainer)
				{
					textPointer = TextContainerHelper.GetTextPointerForEmbeddedObject((FrameworkElement)element);
				}
			}
			if (textPointer != null && textPointer.TextContainer != this._structuralCache.TextContainer)
			{
				textPointer = null;
			}
			TextPointer textPointer2 = textPointer as TextPointer;
			if (textPointer2 == null)
			{
				return ContentPosition.Missing;
			}
			return textPointer2;
		}

		// Token: 0x06002F55 RID: 12117 RVA: 0x000D5784 File Offset: 0x000D3984
		internal void OnChildDesiredSizeChanged(UIElement child)
		{
			if (this._structuralCache != null && this._structuralCache.IsFormattedOnce && !this._structuralCache.ForceReformat)
			{
				if (this._structuralCache.IsFormattingInProgress)
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.OnChildDesiredSizeChangedAsync), child);
					return;
				}
				int cpfromEmbeddedObject = TextContainerHelper.GetCPFromEmbeddedObject(child, ElementEdge.BeforeStart);
				if (cpfromEmbeddedObject < 0)
				{
					return;
				}
				TextPointer textPointer = new TextPointer(this._structuralCache.TextContainer.Start);
				textPointer.MoveByOffset(cpfromEmbeddedObject);
				TextPointer textPointer2 = new TextPointer(textPointer);
				textPointer2.MoveByOffset(TextContainerHelper.EmbeddedObjectLength);
				DirtyTextRange dtr = new DirtyTextRange(cpfromEmbeddedObject, TextContainerHelper.EmbeddedObjectLength, TextContainerHelper.EmbeddedObjectLength, false);
				this._structuralCache.AddDirtyTextRange(dtr);
				if (this._formatter != null)
				{
					this._formatter.OnContentInvalidated(true, textPointer, textPointer2);
				}
			}
		}

		// Token: 0x06002F56 RID: 12118 RVA: 0x000D5858 File Offset: 0x000D3A58
		internal void InitializeForFirstFormatting()
		{
			this._structuralCache.TextContainer.Changing += this.OnTextContainerChanging;
			this._structuralCache.TextContainer.Change += this.OnTextContainerChange;
			this._structuralCache.TextContainer.Highlights.Changed += this.OnHighlightChanged;
		}

		// Token: 0x06002F57 RID: 12119 RVA: 0x000D58C0 File Offset: 0x000D3AC0
		internal void Uninitialize()
		{
			this._structuralCache.TextContainer.Changing -= this.OnTextContainerChanging;
			this._structuralCache.TextContainer.Change -= this.OnTextContainerChange;
			this._structuralCache.TextContainer.Highlights.Changed -= this.OnHighlightChanged;
			this._structuralCache.IsFormattedOnce = false;
		}

		// Token: 0x06002F58 RID: 12120 RVA: 0x000D5934 File Offset: 0x000D3B34
		internal Thickness ComputePageMargin()
		{
			double lineHeightValue = DynamicPropertyReader.GetLineHeightValue(this);
			Thickness pagePadding = this.PagePadding;
			if (DoubleUtil.IsNaN(pagePadding.Left))
			{
				pagePadding.Left = lineHeightValue;
			}
			if (DoubleUtil.IsNaN(pagePadding.Top))
			{
				pagePadding.Top = lineHeightValue;
			}
			if (DoubleUtil.IsNaN(pagePadding.Right))
			{
				pagePadding.Right = lineHeightValue;
			}
			if (DoubleUtil.IsNaN(pagePadding.Bottom))
			{
				pagePadding.Bottom = lineHeightValue;
			}
			return pagePadding;
		}

		// Token: 0x06002F59 RID: 12121 RVA: 0x000D59A8 File Offset: 0x000D3BA8
		internal override void OnNewParent(DependencyObject newParent)
		{
			DependencyObject parent = base.Parent;
			base.OnNewParent(newParent);
			if (newParent is RichTextBox || parent is RichTextBox)
			{
				base.CoerceValue(ContentElement.IsEnabledProperty);
			}
		}

		// Token: 0x17000BEB RID: 3051
		// (get) Token: 0x06002F5A RID: 12122 RVA: 0x000D59E0 File Offset: 0x000D3BE0
		internal FlowDocumentFormatter BottomlessFormatter
		{
			get
			{
				if (this._formatter != null && !(this._formatter is FlowDocumentFormatter))
				{
					this._formatter.Suspend();
					this._formatter = null;
				}
				if (this._formatter == null)
				{
					this._formatter = new FlowDocumentFormatter(this);
				}
				return (FlowDocumentFormatter)this._formatter;
			}
		}

		// Token: 0x17000BEC RID: 3052
		// (get) Token: 0x06002F5B RID: 12123 RVA: 0x000D5A33 File Offset: 0x000D3C33
		internal StructuralCache StructuralCache
		{
			get
			{
				return this._structuralCache;
			}
		}

		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x06002F5C RID: 12124 RVA: 0x000D5A3B File Offset: 0x000D3C3B
		internal TypographyProperties TypographyPropertiesGroup
		{
			get
			{
				if (this._typographyPropertiesGroup == null)
				{
					this._typographyPropertiesGroup = TextElement.GetTypographyProperties(this);
				}
				return this._typographyPropertiesGroup;
			}
		}

		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x06002F5D RID: 12125 RVA: 0x000D5A5D File Offset: 0x000D3C5D
		// (set) Token: 0x06002F5E RID: 12126 RVA: 0x000D5A65 File Offset: 0x000D3C65
		internal TextWrapping TextWrapping
		{
			get
			{
				return this._textWrapping;
			}
			set
			{
				this._textWrapping = value;
			}
		}

		// Token: 0x17000BEF RID: 3055
		// (get) Token: 0x06002F5F RID: 12127 RVA: 0x000D5A6E File Offset: 0x000D3C6E
		internal IFlowDocumentFormatter Formatter
		{
			get
			{
				return this._formatter;
			}
		}

		// Token: 0x17000BF0 RID: 3056
		// (get) Token: 0x06002F60 RID: 12128 RVA: 0x000D5A76 File Offset: 0x000D3C76
		internal bool IsLayoutDataValid
		{
			get
			{
				return this._formatter != null && this._formatter.IsLayoutDataValid;
			}
		}

		// Token: 0x17000BF1 RID: 3057
		// (get) Token: 0x06002F61 RID: 12129 RVA: 0x000D5A8D File Offset: 0x000D3C8D
		internal TextContainer TextContainer
		{
			get
			{
				return this._structuralCache.TextContainer;
			}
		}

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x06002F62 RID: 12130 RVA: 0x000D5A9A File Offset: 0x000D3C9A
		// (set) Token: 0x06002F63 RID: 12131 RVA: 0x000D5AA2 File Offset: 0x000D3CA2
		internal double PixelsPerDip
		{
			get
			{
				return this._pixelsPerDip;
			}
			set
			{
				this._pixelsPerDip = value;
			}
		}

		// Token: 0x14000076 RID: 118
		// (add) Token: 0x06002F64 RID: 12132 RVA: 0x000D5AAC File Offset: 0x000D3CAC
		// (remove) Token: 0x06002F65 RID: 12133 RVA: 0x000D5AE4 File Offset: 0x000D3CE4
		internal event EventHandler PageSizeChanged;

		// Token: 0x06002F66 RID: 12134 RVA: 0x000D5B19 File Offset: 0x000D3D19
		private static void OnTypographyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((FlowDocument)d)._typographyPropertiesGroup = null;
		}

		// Token: 0x06002F67 RID: 12135 RVA: 0x000D5B27 File Offset: 0x000D3D27
		private object OnChildDesiredSizeChangedAsync(object arg)
		{
			this.OnChildDesiredSizeChanged(arg as UIElement);
			return null;
		}

		// Token: 0x06002F68 RID: 12136 RVA: 0x000D5B36 File Offset: 0x000D3D36
		private void Initialize(TextContainer textContainer)
		{
			if (textContainer == null)
			{
				textContainer = new TextContainer(this, false);
			}
			this._structuralCache = new StructuralCache(this, textContainer);
			if (this._formatter != null)
			{
				this._formatter.Suspend();
				this._formatter = null;
			}
		}

		// Token: 0x06002F69 RID: 12137 RVA: 0x000D5B6C File Offset: 0x000D3D6C
		private static void OnPageMetricsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FlowDocument flowDocument = (FlowDocument)d;
			if (flowDocument._structuralCache != null && flowDocument._structuralCache.IsFormattedOnce)
			{
				if (flowDocument._formatter != null)
				{
					flowDocument._formatter.OnContentInvalidated(true);
				}
				if (flowDocument.PageSizeChanged != null)
				{
					flowDocument.PageSizeChanged(flowDocument, EventArgs.Empty);
				}
			}
		}

		// Token: 0x06002F6A RID: 12138 RVA: 0x000D5BC2 File Offset: 0x000D3DC2
		private static void OnMinPageWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.CoerceValue(FlowDocument.MaxPageWidthProperty);
			d.CoerceValue(FlowDocument.PageWidthProperty);
		}

		// Token: 0x06002F6B RID: 12139 RVA: 0x000D5BDA File Offset: 0x000D3DDA
		private static void OnMinPageHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.CoerceValue(FlowDocument.MaxPageHeightProperty);
			d.CoerceValue(FlowDocument.PageHeightProperty);
		}

		// Token: 0x06002F6C RID: 12140 RVA: 0x000D5BF2 File Offset: 0x000D3DF2
		private static void OnMaxPageWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.CoerceValue(FlowDocument.PageWidthProperty);
		}

		// Token: 0x06002F6D RID: 12141 RVA: 0x000D5BFF File Offset: 0x000D3DFF
		private static void OnMaxPageHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.CoerceValue(FlowDocument.PageHeightProperty);
		}

		// Token: 0x06002F6E RID: 12142 RVA: 0x000D5C0C File Offset: 0x000D3E0C
		private static object CoerceMaxPageWidth(DependencyObject d, object value)
		{
			FlowDocument flowDocument = (FlowDocument)d;
			double num = (double)value;
			double minPageWidth = flowDocument.MinPageWidth;
			if (num < minPageWidth)
			{
				return minPageWidth;
			}
			return value;
		}

		// Token: 0x06002F6F RID: 12143 RVA: 0x000D5C3C File Offset: 0x000D3E3C
		private static object CoerceMaxPageHeight(DependencyObject d, object value)
		{
			FlowDocument flowDocument = (FlowDocument)d;
			double num = (double)value;
			double minPageHeight = flowDocument.MinPageHeight;
			if (num < minPageHeight)
			{
				return minPageHeight;
			}
			return value;
		}

		// Token: 0x06002F70 RID: 12144 RVA: 0x000D5C6C File Offset: 0x000D3E6C
		private static object CoercePageWidth(DependencyObject d, object value)
		{
			FlowDocument flowDocument = (FlowDocument)d;
			double num = (double)value;
			if (!DoubleUtil.IsNaN(num))
			{
				double maxPageWidth = flowDocument.MaxPageWidth;
				if (num > maxPageWidth)
				{
					num = maxPageWidth;
				}
				double minPageWidth = flowDocument.MinPageWidth;
				if (num < minPageWidth)
				{
				}
			}
			return value;
		}

		// Token: 0x06002F71 RID: 12145 RVA: 0x000D5CAC File Offset: 0x000D3EAC
		private static object CoercePageHeight(DependencyObject d, object value)
		{
			FlowDocument flowDocument = (FlowDocument)d;
			double num = (double)value;
			if (!DoubleUtil.IsNaN(num))
			{
				double maxPageHeight = flowDocument.MaxPageHeight;
				if (num > maxPageHeight)
				{
					num = maxPageHeight;
				}
				double minPageHeight = flowDocument.MinPageHeight;
				if (num < minPageHeight)
				{
				}
			}
			return value;
		}

		// Token: 0x06002F72 RID: 12146 RVA: 0x000D5CEC File Offset: 0x000D3EEC
		private void OnHighlightChanged(object sender, HighlightChangedEventArgs args)
		{
			Invariant.Assert(args != null);
			Invariant.Assert(args.Ranges != null);
			Invariant.Assert(this._structuralCache != null && this._structuralCache.IsFormattedOnce, "Unexpected Highlights.Changed callback before first format!");
			if (this._structuralCache.IsFormattingInProgress)
			{
				this._structuralCache.OnInvalidOperationDetected();
				throw new InvalidOperationException(SR.Get("FlowDocumentInvalidContnetChange"));
			}
			if (args.OwnerType != typeof(SpellerHighlightLayer))
			{
				return;
			}
			if (args.Ranges.Count > 0)
			{
				if (this._formatter == null || !(this._formatter is FlowDocumentFormatter))
				{
					this._structuralCache.InvalidateFormatCache(false);
				}
				if (this._formatter != null)
				{
					for (int i = 0; i < args.Ranges.Count; i++)
					{
						TextSegment textSegment = (TextSegment)args.Ranges[i];
						this._formatter.OnContentInvalidated(false, textSegment.Start, textSegment.End);
						if (this._formatter is FlowDocumentFormatter)
						{
							DirtyTextRange dtr = new DirtyTextRange(textSegment.Start.Offset, textSegment.Start.GetOffsetToPosition(textSegment.End), textSegment.Start.GetOffsetToPosition(textSegment.End), false);
							this._structuralCache.AddDirtyTextRange(dtr);
						}
					}
				}
			}
		}

		// Token: 0x06002F73 RID: 12147 RVA: 0x000D5E48 File Offset: 0x000D4048
		private void OnTextContainerChanging(object sender, EventArgs args)
		{
			Invariant.Assert(sender == this._structuralCache.TextContainer, "Received text change for foreign TextContainer.");
			Invariant.Assert(this._structuralCache != null && this._structuralCache.IsFormattedOnce, "Unexpected TextContainer.Changing callback before first format!");
			if (this._structuralCache.IsFormattingInProgress)
			{
				this._structuralCache.OnInvalidOperationDetected();
				throw new InvalidOperationException(SR.Get("FlowDocumentInvalidContnetChange"));
			}
			this._structuralCache.IsContentChangeInProgress = true;
		}

		// Token: 0x06002F74 RID: 12148 RVA: 0x000D5EC4 File Offset: 0x000D40C4
		private void OnTextContainerChange(object sender, TextContainerChangeEventArgs args)
		{
			Invariant.Assert(args != null);
			Invariant.Assert(sender == this._structuralCache.TextContainer);
			Invariant.Assert(this._structuralCache != null && this._structuralCache.IsFormattedOnce, "Unexpected TextContainer.Change callback before first format!");
			if (args.Count == 0)
			{
				return;
			}
			try
			{
				if (this._structuralCache.IsFormattingInProgress)
				{
					this._structuralCache.OnInvalidOperationDetected();
					throw new InvalidOperationException(SR.Get("FlowDocumentInvalidContnetChange"));
				}
				ITextPointer end;
				if (args.TextChange != TextChangeType.ContentRemoved)
				{
					end = args.ITextPosition.CreatePointer(args.Count, LogicalDirection.Forward);
				}
				else
				{
					end = args.ITextPosition;
				}
				if (!args.AffectsRenderOnly || (this._formatter != null && this._formatter is FlowDocumentFormatter))
				{
					DirtyTextRange dtr = new DirtyTextRange(args);
					this._structuralCache.AddDirtyTextRange(dtr);
				}
				else
				{
					this._structuralCache.InvalidateFormatCache(false);
				}
				if (this._formatter != null)
				{
					this._formatter.OnContentInvalidated(!args.AffectsRenderOnly, args.ITextPosition, end);
				}
			}
			finally
			{
				this._structuralCache.IsContentChangeInProgress = false;
			}
		}

		// Token: 0x06002F75 RID: 12149 RVA: 0x000D5FE4 File Offset: 0x000D41E4
		private static bool IsValidPageSize(object o)
		{
			double num = (double)o;
			double num2 = (double)Math.Min(1000000, 3500000);
			return double.IsNaN(num) || (num >= 0.0 && num <= num2);
		}

		// Token: 0x06002F76 RID: 12150 RVA: 0x000D6028 File Offset: 0x000D4228
		private static bool IsValidMinPageSize(object o)
		{
			double num = (double)o;
			double num2 = (double)Math.Min(1000000, 3500000);
			return !double.IsNaN(num) && (double.IsNegativeInfinity(num) || (num >= 0.0 && num <= num2));
		}

		// Token: 0x06002F77 RID: 12151 RVA: 0x000D6074 File Offset: 0x000D4274
		private static bool IsValidMaxPageSize(object o)
		{
			double num = (double)o;
			double num2 = (double)Math.Min(1000000, 3500000);
			return !double.IsNaN(num) && (double.IsPositiveInfinity(num) || (num >= 0.0 && num <= num2));
		}

		// Token: 0x06002F78 RID: 12152 RVA: 0x000D60C0 File Offset: 0x000D42C0
		private static bool IsValidPagePadding(object o)
		{
			Thickness t = (Thickness)o;
			return Block.IsValidThickness(t, true);
		}

		// Token: 0x06002F79 RID: 12153 RVA: 0x000D60DC File Offset: 0x000D42DC
		private static bool IsValidColumnRuleWidth(object o)
		{
			double num = (double)o;
			double num2 = (double)Math.Min(1000000, 3500000);
			return !double.IsNaN(num) && num >= 0.0 && num <= num2;
		}

		// Token: 0x06002F7A RID: 12154 RVA: 0x000D611C File Offset: 0x000D431C
		private static bool IsValidColumnGap(object o)
		{
			double num = (double)o;
			double num2 = (double)Math.Min(1000000, 3500000);
			return double.IsNaN(num) || (num >= 0.0 && num <= num2);
		}

		/// <summary>Adds a child object. </summary>
		/// <param name="value">The child object to add.</param>
		// Token: 0x06002F7B RID: 12155 RVA: 0x000D6160 File Offset: 0x000D4360
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!TextSchema.IsValidChildOfContainer(FlowDocument._typeofThis, value.GetType()))
			{
				throw new ArgumentException(SR.Get("TextSchema_ChildTypeIsInvalid", new object[]
				{
					FlowDocument._typeofThis.Name,
					value.GetType().Name
				}));
			}
			if (value is TextElement && ((TextElement)value).Parent != null)
			{
				throw new ArgumentException(SR.Get("TextSchema_TheChildElementBelongsToAnotherTreeAlready", new object[]
				{
					value.GetType().Name
				}));
			}
			if (value is Block)
			{
				TextContainer textContainer = this._structuralCache.TextContainer;
				((Block)value).RepositionWithContent(textContainer.End);
				return;
			}
			Invariant.Assert(false);
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x06002F7C RID: 12156 RVA: 0x0000B31C File Offset: 0x0000951C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		/// <summary>Gets the service object of the specified type.</summary>
		/// <param name="serviceType">An object that specifies the type of service object to get. </param>
		/// <returns>A service object of type <paramref name="serviceType." />-or-
		///
		///     <see langword="null" /> if there is no service object of type <paramref name="serviceType" />.</returns>
		// Token: 0x06002F7D RID: 12157 RVA: 0x000D6224 File Offset: 0x000D4424
		object IServiceProvider.GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			if (serviceType == typeof(ITextContainer))
			{
				return this._structuralCache.TextContainer;
			}
			if (serviceType == typeof(TextContainer))
			{
				return this._structuralCache.TextContainer;
			}
			return null;
		}

		/// <summary>Defines the source object that performs actual content pagination.</summary>
		/// <returns>The object that performs the actual content pagination.</returns>
		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x06002F7E RID: 12158 RVA: 0x000D6284 File Offset: 0x000D4484
		DocumentPaginator IDocumentPaginatorSource.DocumentPaginator
		{
			get
			{
				if (this._formatter != null && !(this._formatter is FlowDocumentPaginator))
				{
					this._formatter.Suspend();
					this._formatter = null;
				}
				if (this._formatter == null)
				{
					this._formatter = new FlowDocumentPaginator(this);
				}
				return (FlowDocumentPaginator)this._formatter;
			}
		}

		// Token: 0x04001E21 RID: 7713
		private static readonly Type _typeofThis = typeof(FlowDocument);

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FlowDocument.FontFamily" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FlowDocument.FontFamily" /> dependency property.</returns>
		// Token: 0x04001E22 RID: 7714
		public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(FlowDocument._typeofThis);

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FlowDocument.FontStyle" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FlowDocument.FontStyle" /> dependency property.</returns>
		// Token: 0x04001E23 RID: 7715
		public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(FlowDocument._typeofThis);

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FlowDocument.FontWeight" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FlowDocument.FontWeight" /> dependency property.</returns>
		// Token: 0x04001E24 RID: 7716
		public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(FlowDocument._typeofThis);

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FlowDocument.FontStretch" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FlowDocument.FontStretch" /> dependency property.</returns>
		// Token: 0x04001E25 RID: 7717
		public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(FlowDocument._typeofThis);

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FlowDocument.FontSize" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FlowDocument.FontSize" /> dependency property.</returns>
		// Token: 0x04001E26 RID: 7718
		public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(FlowDocument._typeofThis);

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FlowDocument.Foreground" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FlowDocument.Foreground" /> dependency property.</returns>
		// Token: 0x04001E27 RID: 7719
		public static readonly DependencyProperty ForegroundProperty = TextElement.ForegroundProperty.AddOwner(FlowDocument._typeofThis);

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FlowDocument.Background" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FlowDocument.Background" /> dependency property.</returns>
		// Token: 0x04001E28 RID: 7720
		public static readonly DependencyProperty BackgroundProperty = TextElement.BackgroundProperty.AddOwner(FlowDocument._typeofThis, new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FlowDocument.TextEffects" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FlowDocument.TextEffects" /> dependency property.</returns>
		// Token: 0x04001E29 RID: 7721
		public static readonly DependencyProperty TextEffectsProperty = TextElement.TextEffectsProperty.AddOwner(FlowDocument._typeofThis, new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(TextEffectCollection.Empty), FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FlowDocument.TextAlignment" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FlowDocument.TextAlignment" /> dependency property.</returns>
		// Token: 0x04001E2A RID: 7722
		public static readonly DependencyProperty TextAlignmentProperty = Block.TextAlignmentProperty.AddOwner(FlowDocument._typeofThis);

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FlowDocument.FlowDirection" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FlowDocument.FlowDirection" /> dependency property.</returns>
		// Token: 0x04001E2B RID: 7723
		public static readonly DependencyProperty FlowDirectionProperty = Block.FlowDirectionProperty.AddOwner(FlowDocument._typeofThis);

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FlowDocument.LineHeight" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FlowDocument.LineHeight" /> dependency property.</returns>
		// Token: 0x04001E2C RID: 7724
		public static readonly DependencyProperty LineHeightProperty = Block.LineHeightProperty.AddOwner(FlowDocument._typeofThis);

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FlowDocument.LineStackingStrategy" />  dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FlowDocument.LineStackingStrategy" /> dependency property.</returns>
		// Token: 0x04001E2D RID: 7725
		public static readonly DependencyProperty LineStackingStrategyProperty = Block.LineStackingStrategyProperty.AddOwner(FlowDocument._typeofThis);

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FlowDocument.ColumnWidth" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FlowDocument.ColumnWidth" /> dependency property.</returns>
		// Token: 0x04001E2E RID: 7726
		public static readonly DependencyProperty ColumnWidthProperty = DependencyProperty.Register("ColumnWidth", typeof(double), FlowDocument._typeofThis, new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FlowDocument.ColumnGap" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FlowDocument.ColumnGap" /> dependency property.</returns>
		// Token: 0x04001E2F RID: 7727
		public static readonly DependencyProperty ColumnGapProperty = DependencyProperty.Register("ColumnGap", typeof(double), FlowDocument._typeofThis, new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(FlowDocument.IsValidColumnGap));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FlowDocument.IsColumnWidthFlexible" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FlowDocument.IsColumnWidthFlexible" /> dependency property.</returns>
		// Token: 0x04001E30 RID: 7728
		public static readonly DependencyProperty IsColumnWidthFlexibleProperty = DependencyProperty.Register("IsColumnWidthFlexible", typeof(bool), FlowDocument._typeofThis, new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FlowDocument.ColumnRuleWidth" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FlowDocument.ColumnRuleWidth" /> dependency property.</returns>
		// Token: 0x04001E31 RID: 7729
		public static readonly DependencyProperty ColumnRuleWidthProperty = DependencyProperty.Register("ColumnRuleWidth", typeof(double), FlowDocument._typeofThis, new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(FlowDocument.IsValidColumnRuleWidth));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FlowDocument.ColumnRuleBrush" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FlowDocument.ColumnRuleBrush" /> dependency property.</returns>
		// Token: 0x04001E32 RID: 7730
		public static readonly DependencyProperty ColumnRuleBrushProperty = DependencyProperty.Register("ColumnRuleBrush", typeof(Brush), FlowDocument._typeofThis, new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FlowDocument.IsOptimalParagraphEnabled" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FlowDocument.IsOptimalParagraphEnabled" /> dependency property.</returns>
		// Token: 0x04001E33 RID: 7731
		public static readonly DependencyProperty IsOptimalParagraphEnabledProperty = DependencyProperty.Register("IsOptimalParagraphEnabled", typeof(bool), FlowDocument._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FlowDocument.PageWidth" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FlowDocument.PageWidth" /> dependency property.</returns>
		// Token: 0x04001E34 RID: 7732
		public static readonly DependencyProperty PageWidthProperty = DependencyProperty.Register("PageWidth", typeof(double), FlowDocument._typeofThis, new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FlowDocument.OnPageMetricsChanged), new CoerceValueCallback(FlowDocument.CoercePageWidth)), new ValidateValueCallback(FlowDocument.IsValidPageSize));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FlowDocument.MinPageWidth" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FlowDocument.MinPageWidth" /> dependency property.</returns>
		// Token: 0x04001E35 RID: 7733
		public static readonly DependencyProperty MinPageWidthProperty = DependencyProperty.Register("MinPageWidth", typeof(double), FlowDocument._typeofThis, new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FlowDocument.OnMinPageWidthChanged)), new ValidateValueCallback(FlowDocument.IsValidMinPageSize));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FlowDocument.MaxPageWidth" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FlowDocument.MaxPageWidth" /> dependency property.</returns>
		// Token: 0x04001E36 RID: 7734
		public static readonly DependencyProperty MaxPageWidthProperty = DependencyProperty.Register("MaxPageWidth", typeof(double), FlowDocument._typeofThis, new FrameworkPropertyMetadata(double.PositiveInfinity, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FlowDocument.OnMaxPageWidthChanged), new CoerceValueCallback(FlowDocument.CoerceMaxPageWidth)), new ValidateValueCallback(FlowDocument.IsValidMaxPageSize));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FlowDocument.PageHeight" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FlowDocument.PageHeight" /> dependency property.</returns>
		// Token: 0x04001E37 RID: 7735
		public static readonly DependencyProperty PageHeightProperty = DependencyProperty.Register("PageHeight", typeof(double), FlowDocument._typeofThis, new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FlowDocument.OnPageMetricsChanged), new CoerceValueCallback(FlowDocument.CoercePageHeight)), new ValidateValueCallback(FlowDocument.IsValidPageSize));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FlowDocument.MinPageHeight" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FlowDocument.MinPageHeight" /> dependency property.</returns>
		// Token: 0x04001E38 RID: 7736
		public static readonly DependencyProperty MinPageHeightProperty = DependencyProperty.Register("MinPageHeight", typeof(double), FlowDocument._typeofThis, new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FlowDocument.OnMinPageHeightChanged)), new ValidateValueCallback(FlowDocument.IsValidMinPageSize));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FlowDocument.MaxPageHeight" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FlowDocument.MaxPageHeight" /> dependency property.</returns>
		// Token: 0x04001E39 RID: 7737
		public static readonly DependencyProperty MaxPageHeightProperty = DependencyProperty.Register("MaxPageHeight", typeof(double), FlowDocument._typeofThis, new FrameworkPropertyMetadata(double.PositiveInfinity, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FlowDocument.OnMaxPageHeightChanged), new CoerceValueCallback(FlowDocument.CoerceMaxPageHeight)), new ValidateValueCallback(FlowDocument.IsValidMaxPageSize));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FlowDocument.PagePadding" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FlowDocument.PagePadding" /> dependency property.</returns>
		// Token: 0x04001E3A RID: 7738
		public static readonly DependencyProperty PagePaddingProperty = DependencyProperty.Register("PagePadding", typeof(Thickness), FlowDocument._typeofThis, new FrameworkPropertyMetadata(new Thickness(double.NaN), FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FlowDocument.OnPageMetricsChanged)), new ValidateValueCallback(FlowDocument.IsValidPagePadding));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FlowDocument.IsHyphenationEnabled" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FlowDocument.IsHyphenationEnabled" /> dependency property.</returns>
		// Token: 0x04001E3B RID: 7739
		public static readonly DependencyProperty IsHyphenationEnabledProperty = Block.IsHyphenationEnabledProperty.AddOwner(FlowDocument._typeofThis);

		// Token: 0x04001E3D RID: 7741
		private StructuralCache _structuralCache;

		// Token: 0x04001E3E RID: 7742
		private TypographyProperties _typographyPropertiesGroup;

		// Token: 0x04001E3F RID: 7743
		private IFlowDocumentFormatter _formatter;

		// Token: 0x04001E40 RID: 7744
		private TextWrapping _textWrapping = TextWrapping.Wrap;

		// Token: 0x04001E41 RID: 7745
		private double _pixelsPerDip = (double)Util.PixelsPerDip;
	}
}
