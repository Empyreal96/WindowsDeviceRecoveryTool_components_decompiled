using System;
using System.ComponentModel;
using System.Windows.Markup;
using MS.Internal;

namespace System.Windows.Documents
{
	/// <summary>A block-level flow content element used to group content into a paragraph.</summary>
	// Token: 0x020003A0 RID: 928
	[ContentProperty("Inlines")]
	public class Paragraph : Block
	{
		// Token: 0x06003270 RID: 12912 RVA: 0x000DCE34 File Offset: 0x000DB034
		static Paragraph()
		{
			FrameworkContentElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Paragraph), new FrameworkPropertyMetadata(typeof(Paragraph)));
		}

		/// <summary>Initializes a new, empty instance of the <see cref="T:System.Windows.Documents.Paragraph" /> class.</summary>
		// Token: 0x06003271 RID: 12913 RVA: 0x000C454D File Offset: 0x000C274D
		public Paragraph()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Paragraph" /> class, taking a specified <see cref="T:System.Windows.Documents.Inline" /> object as its initial contents.</summary>
		/// <param name="inline">An <see cref="T:System.Windows.Documents.Inline" /> object specifying the initial contents of the new <see cref="T:System.Windows.Documents.Paragraph" />.</param>
		// Token: 0x06003272 RID: 12914 RVA: 0x000DCFA6 File Offset: 0x000DB1A6
		public Paragraph(Inline inline)
		{
			if (inline == null)
			{
				throw new ArgumentNullException("inline");
			}
			this.Inlines.Add(inline);
		}

		/// <summary>Gets an <see cref="T:System.Windows.Documents.InlineCollection" /> containing the top-level <see cref="T:System.Windows.Documents.Inline" /> elements that comprise the contents of the <see cref="T:System.Windows.Documents.Paragraph" />.</summary>
		/// <returns>An <see cref="T:System.Windows.Documents.InlineCollection" /> containing the <see cref="T:System.Windows.Documents.Inline" /> elements that comprise the contents of the <see cref="T:System.Windows.Documents.Paragraph" />.This property has no default value.</returns>
		// Token: 0x17000CB8 RID: 3256
		// (get) Token: 0x06003273 RID: 12915 RVA: 0x000DCFC8 File Offset: 0x000DB1C8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public InlineCollection Inlines
		{
			get
			{
				return new InlineCollection(this, true);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.TextDecorationCollection" /> that contains text decorations to apply to this element.  </summary>
		/// <returns>A <see cref="T:System.Windows.TextDecorationCollection" /> collection that contains text decorations to apply to this element. A value of <see langword="null" /> means no text decorations will be applied. The default value is <see langword="null" />.</returns>
		// Token: 0x17000CB9 RID: 3257
		// (get) Token: 0x06003274 RID: 12916 RVA: 0x000DCFD1 File Offset: 0x000DB1D1
		// (set) Token: 0x06003275 RID: 12917 RVA: 0x000DCFE3 File Offset: 0x000DB1E3
		public TextDecorationCollection TextDecorations
		{
			get
			{
				return (TextDecorationCollection)base.GetValue(Paragraph.TextDecorationsProperty);
			}
			set
			{
				base.SetValue(Paragraph.TextDecorationsProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates how far to indent the first line of a <see cref="T:System.Windows.Documents.Paragraph" />.  </summary>
		/// <returns>A double value specifying the amount to indent the first line of the paragraph, in device independent pixels. The default value is 0.</returns>
		// Token: 0x17000CBA RID: 3258
		// (get) Token: 0x06003276 RID: 12918 RVA: 0x000DCFF1 File Offset: 0x000DB1F1
		// (set) Token: 0x06003277 RID: 12919 RVA: 0x000DD003 File Offset: 0x000DB203
		[TypeConverter(typeof(LengthConverter))]
		public double TextIndent
		{
			get
			{
				return (double)base.GetValue(Paragraph.TextIndentProperty);
			}
			set
			{
				base.SetValue(Paragraph.TextIndentProperty, value);
			}
		}

		/// <summary>Gets or sets a value that specifies the minimum number of lines that can be left before the break when a <see cref="T:System.Windows.Documents.Paragraph" /> is broken by a page break or column break.  </summary>
		/// <returns>An integer specifying the minimum number of lines that can be left before the break when a <see cref="T:System.Windows.Documents.Paragraph" /> is broken by a page break or column break. A value of 0 indicates no minimum.The default value is 0.</returns>
		// Token: 0x17000CBB RID: 3259
		// (get) Token: 0x06003278 RID: 12920 RVA: 0x000DD016 File Offset: 0x000DB216
		// (set) Token: 0x06003279 RID: 12921 RVA: 0x000DD028 File Offset: 0x000DB228
		public int MinOrphanLines
		{
			get
			{
				return (int)base.GetValue(Paragraph.MinOrphanLinesProperty);
			}
			set
			{
				base.SetValue(Paragraph.MinOrphanLinesProperty, value);
			}
		}

		/// <summary>Gets or sets a value that specifies the minimum number of lines that can be placed after the break when a <see cref="T:System.Windows.Documents.Paragraph" /> is broken by a page break or column break.  </summary>
		/// <returns>An integer specifying the minimum number of lines that can be placed after the break when a <see cref="T:System.Windows.Documents.Paragraph" /> is broken by a page break or column break.  A value of 0 indicates no minimum.The default value is 0.</returns>
		// Token: 0x17000CBC RID: 3260
		// (get) Token: 0x0600327A RID: 12922 RVA: 0x000DD03B File Offset: 0x000DB23B
		// (set) Token: 0x0600327B RID: 12923 RVA: 0x000DD04D File Offset: 0x000DB24D
		public int MinWidowLines
		{
			get
			{
				return (int)base.GetValue(Paragraph.MinWidowLinesProperty);
			}
			set
			{
				base.SetValue(Paragraph.MinWidowLinesProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a break may occur between this paragraph and the next paragraph.  </summary>
		/// <returns>
		///     <see langword="true" /> to prevent a break from occurring between this paragraph and the next paragraph; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000CBD RID: 3261
		// (get) Token: 0x0600327C RID: 12924 RVA: 0x000DD060 File Offset: 0x000DB260
		// (set) Token: 0x0600327D RID: 12925 RVA: 0x000DD072 File Offset: 0x000DB272
		public bool KeepWithNext
		{
			get
			{
				return (bool)base.GetValue(Paragraph.KeepWithNextProperty);
			}
			set
			{
				base.SetValue(Paragraph.KeepWithNextProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the text of the paragraph may be broken by a page break or column break.  </summary>
		/// <returns>
		///     <see langword="true" /> to prevent the text of the paragraph from being broken; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000CBE RID: 3262
		// (get) Token: 0x0600327E RID: 12926 RVA: 0x000DD080 File Offset: 0x000DB280
		// (set) Token: 0x0600327F RID: 12927 RVA: 0x000DD092 File Offset: 0x000DB292
		public bool KeepTogether
		{
			get
			{
				return (bool)base.GetValue(Paragraph.KeepTogetherProperty);
			}
			set
			{
				base.SetValue(Paragraph.KeepTogetherProperty, value);
			}
		}

		// Token: 0x06003280 RID: 12928 RVA: 0x000DD0A0 File Offset: 0x000DB2A0
		internal void GetDefaultMarginValue(ref Thickness margin)
		{
			double num = base.LineHeight;
			if (Paragraph.IsLineHeightAuto(num))
			{
				num = base.FontFamily.LineSpacing * base.FontSize;
			}
			margin = new Thickness(0.0, num, 0.0, num);
		}

		// Token: 0x06003281 RID: 12929 RVA: 0x000DD0EE File Offset: 0x000DB2EE
		internal static bool IsMarginAuto(Thickness margin)
		{
			return double.IsNaN(margin.Left) && double.IsNaN(margin.Right) && double.IsNaN(margin.Top) && double.IsNaN(margin.Bottom);
		}

		// Token: 0x06003282 RID: 12930 RVA: 0x000DD128 File Offset: 0x000DB328
		internal static bool IsLineHeightAuto(double lineHeight)
		{
			return double.IsNaN(lineHeight);
		}

		// Token: 0x06003283 RID: 12931 RVA: 0x000DD130 File Offset: 0x000DB330
		internal static bool HasNoTextContent(Paragraph paragraph)
		{
			ITextPointer textPointer = paragraph.ContentStart.CreatePointer();
			ITextPointer contentEnd = paragraph.ContentEnd;
			while (textPointer.CompareTo(contentEnd) < 0)
			{
				TextPointerContext pointerContext = textPointer.GetPointerContext(LogicalDirection.Forward);
				if (pointerContext == TextPointerContext.Text || pointerContext == TextPointerContext.EmbeddedElement || typeof(LineBreak).IsAssignableFrom(textPointer.ParentType) || typeof(AnchoredBlock).IsAssignableFrom(textPointer.ParentType))
				{
					return false;
				}
				textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
			}
			return true;
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Documents.Paragraph.Inlines" /> property should be persisted.</summary>
		/// <param name="manager">A serialization service manager object for this object.</param>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.NullReferenceException">
		///         <paramref name="manager" /> is <see langword="null" />.</exception>
		// Token: 0x06003284 RID: 12932 RVA: 0x000C3C87 File Offset: 0x000C1E87
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeInlines(XamlDesignerSerializationManager manager)
		{
			return manager != null && manager.XmlWriter == null;
		}

		// Token: 0x06003285 RID: 12933 RVA: 0x000DD1A8 File Offset: 0x000DB3A8
		private static bool IsValidMinOrphanLines(object o)
		{
			int num = (int)o;
			return num >= 0 && num <= 1000000;
		}

		// Token: 0x06003286 RID: 12934 RVA: 0x000DD1D0 File Offset: 0x000DB3D0
		private static bool IsValidMinWidowLines(object o)
		{
			int num = (int)o;
			return num >= 0 && num <= 1000000;
		}

		// Token: 0x06003287 RID: 12935 RVA: 0x000DD1F8 File Offset: 0x000DB3F8
		private static bool IsValidTextIndent(object o)
		{
			double num = (double)o;
			double num2 = (double)Math.Min(1000000, 3500000);
			double num3 = -num2;
			return !double.IsNaN(num) && num >= num3 && num <= num2;
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Paragraph.TextDecorations" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Paragraph.TextDecorations" /> dependency property.</returns>
		// Token: 0x04001EC4 RID: 7876
		public static readonly DependencyProperty TextDecorationsProperty = Inline.TextDecorationsProperty.AddOwner(typeof(Paragraph), new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(TextDecorationCollection.Empty), FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Paragraph.TextIndent" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Paragraph.TextIndent" /> dependency property.</returns>
		// Token: 0x04001EC5 RID: 7877
		public static readonly DependencyProperty TextIndentProperty = DependencyProperty.Register("TextIndent", typeof(double), typeof(Paragraph), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(Paragraph.IsValidTextIndent));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Paragraph.MinOrphanLines" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Paragraph.MinOrphanLines" /> dependency property.</returns>
		// Token: 0x04001EC6 RID: 7878
		public static readonly DependencyProperty MinOrphanLinesProperty = DependencyProperty.Register("MinOrphanLines", typeof(int), typeof(Paragraph), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure), new ValidateValueCallback(Paragraph.IsValidMinOrphanLines));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Paragraph.MinWidowLines" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Paragraph.MinWidowLines" /> dependency property.</returns>
		// Token: 0x04001EC7 RID: 7879
		public static readonly DependencyProperty MinWidowLinesProperty = DependencyProperty.Register("MinWidowLines", typeof(int), typeof(Paragraph), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure), new ValidateValueCallback(Paragraph.IsValidMinWidowLines));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Paragraph.KeepWithNext" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Paragraph.KeepWithNext" /> dependency property.</returns>
		// Token: 0x04001EC8 RID: 7880
		public static readonly DependencyProperty KeepWithNextProperty = DependencyProperty.Register("KeepWithNext", typeof(bool), typeof(Paragraph), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Paragraph.KeepTogether" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Paragraph.KeepTogether" /> dependency property.</returns>
		// Token: 0x04001EC9 RID: 7881
		public static readonly DependencyProperty KeepTogetherProperty = DependencyProperty.Register("KeepTogether", typeof(bool), typeof(Paragraph), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsParentMeasure));
	}
}
