using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using MS.Internal;

namespace System.Windows.Controls
{
	/// <summary>Specifies with an underscore the character that is used as the access key.</summary>
	// Token: 0x0200046A RID: 1130
	[ContentProperty("Text")]
	public class AccessText : FrameworkElement, IAddChild
	{
		/// <summary>For a description of this member, see <see cref="M:System.Windows.Markup.IAddChild.AddChild(System.Object)" />.</summary>
		/// <param name="value">The object to add to the <see cref="T:System.Windows.Controls.AccessText" />.</param>
		// Token: 0x060041E5 RID: 16869 RVA: 0x0012E054 File Offset: 0x0012C254
		void IAddChild.AddChild(object value)
		{
			((IAddChild)this.TextBlock).AddChild(value);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Windows.Markup.IAddChild.AddText(System.String)" />.</summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x060041E6 RID: 16870 RVA: 0x0012E062 File Offset: 0x0012C262
		void IAddChild.AddText(string text)
		{
			((IAddChild)this.TextBlock).AddText(text);
		}

		/// <summary>Gets an enumerator that iterates the logical child elements of the <see cref="T:System.Windows.Controls.AccessText" /> element. </summary>
		/// <returns>An enumerator that iterates the logical child elements of this element.</returns>
		// Token: 0x1700102B RID: 4139
		// (get) Token: 0x060041E7 RID: 16871 RVA: 0x0012E070 File Offset: 0x0012C270
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				return new RangeContentEnumerator(this.TextContainer.Start, this.TextContainer.End);
			}
		}

		/// <summary>Provides read-only access to the character that follows the first underline character.</summary>
		/// <returns>The character to return.</returns>
		// Token: 0x1700102C RID: 4140
		// (get) Token: 0x060041E9 RID: 16873 RVA: 0x0012E08D File Offset: 0x0012C28D
		public char AccessKey
		{
			get
			{
				if (this._accessKey == null || this._accessKey.Text.Length <= 0)
				{
					return '\0';
				}
				return this._accessKey.Text[0];
			}
		}

		/// <summary>Gets or sets the text that is displayed by the <see cref="T:System.Windows.Controls.AccessText" /> element. </summary>
		/// <returns>The text without the first underscore character. The default is an empty string.</returns>
		// Token: 0x1700102D RID: 4141
		// (get) Token: 0x060041EA RID: 16874 RVA: 0x0012E0BD File Offset: 0x0012C2BD
		// (set) Token: 0x060041EB RID: 16875 RVA: 0x0012E0CF File Offset: 0x0012C2CF
		[DefaultValue("")]
		public string Text
		{
			get
			{
				return (string)base.GetValue(AccessText.TextProperty);
			}
			set
			{
				base.SetValue(AccessText.TextProperty, value);
			}
		}

		/// <summary>Gets or sets the font family to use with the <see cref="T:System.Windows.Controls.AccessText" /> element.  </summary>
		/// <returns>The font family to use. The default is the font that is determined by the <see cref="P:System.Windows.SystemFonts.MessageFontFamily" /> metric.</returns>
		// Token: 0x1700102E RID: 4142
		// (get) Token: 0x060041EC RID: 16876 RVA: 0x0012E0DD File Offset: 0x0012C2DD
		// (set) Token: 0x060041ED RID: 16877 RVA: 0x0012E0EF File Offset: 0x0012C2EF
		[Localizability(LocalizationCategory.Font, Modifiability = Modifiability.Unmodifiable)]
		public FontFamily FontFamily
		{
			get
			{
				return (FontFamily)base.GetValue(AccessText.FontFamilyProperty);
			}
			set
			{
				base.SetValue(AccessText.FontFamilyProperty, value);
			}
		}

		/// <summary>Gets or sets the font style to use with the <see cref="T:System.Windows.Controls.AccessText" /> element. </summary>
		/// <returns>The font style to use; for example, normal, italic, or oblique. The default is determined by the <see cref="P:System.Windows.SystemFonts.MessageFontStyle" /> metric.</returns>
		// Token: 0x1700102F RID: 4143
		// (get) Token: 0x060041EE RID: 16878 RVA: 0x0012E0FD File Offset: 0x0012C2FD
		// (set) Token: 0x060041EF RID: 16879 RVA: 0x0012E10F File Offset: 0x0012C30F
		public FontStyle FontStyle
		{
			get
			{
				return (FontStyle)base.GetValue(AccessText.FontStyleProperty);
			}
			set
			{
				base.SetValue(AccessText.FontStyleProperty, value);
			}
		}

		/// <summary>Gets or sets the font weight to use with the <see cref="T:System.Windows.Controls.AccessText" /> element. </summary>
		/// <returns>The font weight to use. The default is determined by the <see cref="P:System.Windows.SystemFonts.MessageFontWeight" /> metric.</returns>
		// Token: 0x17001030 RID: 4144
		// (get) Token: 0x060041F0 RID: 16880 RVA: 0x0012E122 File Offset: 0x0012C322
		// (set) Token: 0x060041F1 RID: 16881 RVA: 0x0012E134 File Offset: 0x0012C334
		public FontWeight FontWeight
		{
			get
			{
				return (FontWeight)base.GetValue(AccessText.FontWeightProperty);
			}
			set
			{
				base.SetValue(AccessText.FontWeightProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.FontStretch" /> property that selects a normal, condensed, or expanded font from a <see cref="T:System.Windows.Media.FontFamily" />. </summary>
		/// <returns>The relative degree that the font is stretched. The default is <see cref="P:System.Windows.FontStretches.Normal" />.</returns>
		// Token: 0x17001031 RID: 4145
		// (get) Token: 0x060041F2 RID: 16882 RVA: 0x0012E147 File Offset: 0x0012C347
		// (set) Token: 0x060041F3 RID: 16883 RVA: 0x0012E159 File Offset: 0x0012C359
		public FontStretch FontStretch
		{
			get
			{
				return (FontStretch)base.GetValue(AccessText.FontStretchProperty);
			}
			set
			{
				base.SetValue(AccessText.FontStretchProperty, value);
			}
		}

		/// <summary>Gets or sets the font size to use with the <see cref="T:System.Windows.Controls.AccessText" /> element.  </summary>
		/// <returns>The font size to use. The default is the font size that is determined by the <see cref="P:System.Windows.SystemFonts.MessageFontSize" /> metric.</returns>
		// Token: 0x17001032 RID: 4146
		// (get) Token: 0x060041F4 RID: 16884 RVA: 0x0012E16C File Offset: 0x0012C36C
		// (set) Token: 0x060041F5 RID: 16885 RVA: 0x0012E17E File Offset: 0x0012C37E
		[TypeConverter(typeof(FontSizeConverter))]
		[Localizability(LocalizationCategory.None)]
		public double FontSize
		{
			get
			{
				return (double)base.GetValue(AccessText.FontSizeProperty);
			}
			set
			{
				base.SetValue(AccessText.FontSizeProperty, value);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Media.Brush" /> that draws the text content of the element. </summary>
		/// <returns>The <see cref="T:System.Windows.Media.Brush" /> that draws the text. The default is <see cref="P:System.Windows.Media.Brushes.Black" />.</returns>
		// Token: 0x17001033 RID: 4147
		// (get) Token: 0x060041F6 RID: 16886 RVA: 0x0012E191 File Offset: 0x0012C391
		// (set) Token: 0x060041F7 RID: 16887 RVA: 0x0012E1A3 File Offset: 0x0012C3A3
		public Brush Foreground
		{
			get
			{
				return (Brush)base.GetValue(AccessText.ForegroundProperty);
			}
			set
			{
				base.SetValue(AccessText.ForegroundProperty, value);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Media.Brush" /> that fills the content area.  </summary>
		/// <returns>The <see cref="T:System.Windows.Media.Brush" /> that fills the content area. The default is <see langword="null" />.</returns>
		// Token: 0x17001034 RID: 4148
		// (get) Token: 0x060041F8 RID: 16888 RVA: 0x0012E1B1 File Offset: 0x0012C3B1
		// (set) Token: 0x060041F9 RID: 16889 RVA: 0x0012E1C3 File Offset: 0x0012C3C3
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(AccessText.BackgroundProperty);
			}
			set
			{
				base.SetValue(AccessText.BackgroundProperty, value);
			}
		}

		/// <summary>Gets or sets the decorations that are added to the text of an <see cref="T:System.Windows.Controls.AccessText" /> element. </summary>
		/// <returns>The <see cref="T:System.Windows.TextDecorations" /> applied to the text of an <see cref="T:System.Windows.Controls.AccessText" />. The default is <see langword="null" />.</returns>
		// Token: 0x17001035 RID: 4149
		// (get) Token: 0x060041FA RID: 16890 RVA: 0x0012E1D1 File Offset: 0x0012C3D1
		// (set) Token: 0x060041FB RID: 16891 RVA: 0x0012E1E3 File Offset: 0x0012C3E3
		public TextDecorationCollection TextDecorations
		{
			get
			{
				return (TextDecorationCollection)base.GetValue(AccessText.TextDecorationsProperty);
			}
			set
			{
				base.SetValue(AccessText.TextDecorationsProperty, value);
			}
		}

		/// <summary>Gets or sets the effects that are added to the text of an <see cref="T:System.Windows.Controls.AccessText" /> element. </summary>
		/// <returns>The <see cref="T:System.Windows.Media.TextEffectCollection" />. The default is <see langword="null" />.</returns>
		// Token: 0x17001036 RID: 4150
		// (get) Token: 0x060041FC RID: 16892 RVA: 0x0012E1F1 File Offset: 0x0012C3F1
		// (set) Token: 0x060041FD RID: 16893 RVA: 0x0012E203 File Offset: 0x0012C403
		public TextEffectCollection TextEffects
		{
			get
			{
				return (TextEffectCollection)base.GetValue(AccessText.TextEffectsProperty);
			}
			set
			{
				base.SetValue(AccessText.TextEffectsProperty, value);
			}
		}

		/// <summary>Gets or sets the height of each line box. </summary>
		/// <returns>A double that specifies the height of each line box. This value must be equal to or greater than 0.0034 and equal to or less then 160000. A value of <see cref="F:System.Double.NaN" /> (equivalent to an attribute value of Auto) causes the line height to be determined automatically from the current font characteristics. The default is <see cref="F:System.Double.NaN" />.</returns>
		// Token: 0x17001037 RID: 4151
		// (get) Token: 0x060041FE RID: 16894 RVA: 0x0012E211 File Offset: 0x0012C411
		// (set) Token: 0x060041FF RID: 16895 RVA: 0x0012E223 File Offset: 0x0012C423
		[TypeConverter(typeof(LengthConverter))]
		public double LineHeight
		{
			get
			{
				return (double)base.GetValue(AccessText.LineHeightProperty);
			}
			set
			{
				base.SetValue(AccessText.LineHeightProperty, value);
			}
		}

		/// <summary>Gets or sets how the <see cref="P:System.Windows.Controls.AccessText.LineHeight" /> property is enforced. </summary>
		/// <returns>A <see cref="T:System.Windows.LineStackingStrategy" /> value that determines the behavior of the <see cref="P:System.Windows.Controls.AccessText.LineHeight" /> property.</returns>
		// Token: 0x17001038 RID: 4152
		// (get) Token: 0x06004200 RID: 16896 RVA: 0x0012E236 File Offset: 0x0012C436
		// (set) Token: 0x06004201 RID: 16897 RVA: 0x0012E248 File Offset: 0x0012C448
		public LineStackingStrategy LineStackingStrategy
		{
			get
			{
				return (LineStackingStrategy)base.GetValue(AccessText.LineStackingStrategyProperty);
			}
			set
			{
				base.SetValue(AccessText.LineStackingStrategyProperty, value);
			}
		}

		/// <summary>Gets or sets the horizontal alignment of the content.  </summary>
		/// <returns>The horizontal alignment of the text.</returns>
		// Token: 0x17001039 RID: 4153
		// (get) Token: 0x06004202 RID: 16898 RVA: 0x0012E25B File Offset: 0x0012C45B
		// (set) Token: 0x06004203 RID: 16899 RVA: 0x0012E26D File Offset: 0x0012C46D
		public TextAlignment TextAlignment
		{
			get
			{
				return (TextAlignment)base.GetValue(AccessText.TextAlignmentProperty);
			}
			set
			{
				base.SetValue(AccessText.TextAlignmentProperty, value);
			}
		}

		/// <summary>Gets or sets how the textual content of an <see cref="T:System.Windows.Controls.AccessText" /> element is clipped if it overflows the line box. </summary>
		/// <returns>The trimming behavior to use. The default is <see cref="F:System.Windows.TextTrimming.None" /></returns>
		// Token: 0x1700103A RID: 4154
		// (get) Token: 0x06004204 RID: 16900 RVA: 0x0012E280 File Offset: 0x0012C480
		// (set) Token: 0x06004205 RID: 16901 RVA: 0x0012E292 File Offset: 0x0012C492
		public TextTrimming TextTrimming
		{
			get
			{
				return (TextTrimming)base.GetValue(AccessText.TextTrimmingProperty);
			}
			set
			{
				base.SetValue(AccessText.TextTrimmingProperty, value);
			}
		}

		/// <summary>Gets or sets whether the textual content of an <see cref="T:System.Windows.Controls.AccessText" /> element is wrapped if it overflows the line box. </summary>
		/// <returns>The wrapping behavior to use. The default is <see cref="F:System.Windows.TextWrapping.NoWrap" />.</returns>
		// Token: 0x1700103B RID: 4155
		// (get) Token: 0x06004206 RID: 16902 RVA: 0x0012E2A5 File Offset: 0x0012C4A5
		// (set) Token: 0x06004207 RID: 16903 RVA: 0x0012E2B7 File Offset: 0x0012C4B7
		public TextWrapping TextWrapping
		{
			get
			{
				return (TextWrapping)base.GetValue(AccessText.TextWrappingProperty);
			}
			set
			{
				base.SetValue(AccessText.TextWrappingProperty, value);
			}
		}

		/// <summary>Gets or sets a value that adjusts the baseline offset position of text in an <see cref="T:System.Windows.Controls.AccessText" /> element.  </summary>
		/// <returns>The amount to adjust the baseline offset position.</returns>
		// Token: 0x1700103C RID: 4156
		// (get) Token: 0x06004208 RID: 16904 RVA: 0x0012E2CA File Offset: 0x0012C4CA
		// (set) Token: 0x06004209 RID: 16905 RVA: 0x0012E2DC File Offset: 0x0012C4DC
		public double BaselineOffset
		{
			get
			{
				return (double)base.GetValue(AccessText.BaselineOffsetProperty);
			}
			set
			{
				base.SetValue(AccessText.BaselineOffsetProperty, value);
			}
		}

		/// <summary>Remeasures the control. </summary>
		/// <param name="constraint">The maximum size limit for the control. The return value cannot exceed this size.</param>
		/// <returns>The size of the control. Cannot exceed the maximum size limit for the control.</returns>
		// Token: 0x0600420A RID: 16906 RVA: 0x0012E2EF File Offset: 0x0012C4EF
		protected sealed override Size MeasureOverride(Size constraint)
		{
			this.TextBlock.Measure(constraint);
			return this.TextBlock.DesiredSize;
		}

		/// <summary>Arranges and sizes the content of an <see cref="T:System.Windows.Controls.AccessText" /> object. </summary>
		/// <param name="arrangeSize">The computed size that is used to arrange the content.</param>
		/// <returns>The size of the content.</returns>
		// Token: 0x0600420B RID: 16907 RVA: 0x0012E308 File Offset: 0x0012C508
		protected sealed override Size ArrangeOverride(Size arrangeSize)
		{
			this.TextBlock.Arrange(new Rect(arrangeSize));
			return arrangeSize;
		}

		// Token: 0x0600420C RID: 16908 RVA: 0x0012E31C File Offset: 0x0012C51C
		internal static bool HasCustomSerialization(object o)
		{
			Run run = o as Run;
			return run != null && AccessText.HasCustomSerializationStorage.GetValue(run);
		}

		// Token: 0x1700103D RID: 4157
		// (get) Token: 0x0600420D RID: 16909 RVA: 0x0012E340 File Offset: 0x0012C540
		internal TextBlock TextBlock
		{
			get
			{
				if (this._textBlock == null)
				{
					this.CreateTextBlock();
				}
				return this._textBlock;
			}
		}

		// Token: 0x1700103E RID: 4158
		// (get) Token: 0x0600420E RID: 16910 RVA: 0x0012E356 File Offset: 0x0012C556
		internal static char AccessKeyMarker
		{
			get
			{
				return '_';
			}
		}

		// Token: 0x1700103F RID: 4159
		// (get) Token: 0x0600420F RID: 16911 RVA: 0x0012E35A File Offset: 0x0012C55A
		private TextContainer TextContainer
		{
			get
			{
				if (this._textContainer == null)
				{
					this.CreateTextBlock();
				}
				return this._textContainer;
			}
		}

		// Token: 0x06004210 RID: 16912 RVA: 0x0012E370 File Offset: 0x0012C570
		private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((AccessText)d).TextBlock.SetValue(e.Property, e.NewValue);
		}

		// Token: 0x06004211 RID: 16913 RVA: 0x0012E390 File Offset: 0x0012C590
		private void CreateTextBlock()
		{
			this._textContainer = new TextContainer(this, false);
			this._textBlock = new TextBlock();
			base.AddVisualChild(this._textBlock);
			this._textBlock.IsContentPresenterContainer = true;
			this._textBlock.SetTextContainer(this._textContainer);
			this.InitializeTextContainerListener();
		}

		/// <summary>Gets the number of child elements that are visual.</summary>
		/// <returns>Returns an integer that represents the number of child elements that are visible.</returns>
		// Token: 0x17001040 RID: 4160
		// (get) Token: 0x06004212 RID: 16914 RVA: 0x00016748 File Offset: 0x00014948
		protected override int VisualChildrenCount
		{
			get
			{
				return 1;
			}
		}

		/// <summary>Gets the index of a visual child element.</summary>
		/// <param name="index">The index of the visual child element to return.</param>
		/// <returns>Returns an integer that represents the index of a visual child element.</returns>
		// Token: 0x06004213 RID: 16915 RVA: 0x0012E3E4 File Offset: 0x0012C5E4
		protected override Visual GetVisualChild(int index)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this.TextBlock;
		}

		// Token: 0x06004214 RID: 16916 RVA: 0x0012E40C File Offset: 0x0012C60C
		internal static void SerializeCustom(XmlWriter xmlWriter, object o)
		{
			Run run = o as Run;
			if (run != null)
			{
				xmlWriter.WriteString(AccessText.AccessKeyMarker.ToString() + run.Text);
			}
		}

		// Token: 0x17001041 RID: 4161
		// (get) Token: 0x06004215 RID: 16917 RVA: 0x0012E444 File Offset: 0x0012C644
		private static Style AccessKeyStyle
		{
			get
			{
				if (AccessText._accessKeyStyle == null)
				{
					Style style = new Style(typeof(Run));
					Trigger trigger = new Trigger();
					trigger.Property = KeyboardNavigation.ShowKeyboardCuesProperty;
					trigger.Value = true;
					trigger.Setters.Add(new Setter(AccessText.TextDecorationsProperty, System.Windows.TextDecorations.Underline));
					style.Triggers.Add(trigger);
					style.Seal();
					AccessText._accessKeyStyle = style;
				}
				return AccessText._accessKeyStyle;
			}
		}

		// Token: 0x06004216 RID: 16918 RVA: 0x0012E4BC File Offset: 0x0012C6BC
		private void UpdateAccessKey()
		{
			TextPointer textPointer = new TextPointer(this.TextContainer.Start);
			while (!this._accessKeyLocated && textPointer.CompareTo(this.TextContainer.End) < 0)
			{
				TextPointerContext pointerContext = textPointer.GetPointerContext(LogicalDirection.Forward);
				if (pointerContext == TextPointerContext.Text)
				{
					string textInRun = textPointer.GetTextInRun(LogicalDirection.Forward);
					int num = AccessText.FindAccessKeyMarker(textInRun);
					if (num != -1 && num < textInRun.Length - 1)
					{
						string nextTextElement = StringInfo.GetNextTextElement(textInRun, num + 1);
						TextPointer positionAtOffset = textPointer.GetPositionAtOffset(num + 1 + nextTextElement.Length);
						this._accessKey = new Run(nextTextElement);
						this._accessKey.Style = AccessText.AccessKeyStyle;
						this.RegisterAccessKey();
						AccessText.HasCustomSerializationStorage.SetValue(this._accessKey, true);
						this._accessKeyLocated = true;
						this.UninitializeTextContainerListener();
						this.TextContainer.BeginChange();
						try
						{
							TextPointer textPointer2 = new TextPointer(textPointer, num);
							TextRangeEdit.DeleteInlineContent(textPointer2, positionAtOffset);
							this._accessKey.RepositionWithContent(textPointer2);
						}
						finally
						{
							this.TextContainer.EndChange();
							this.InitializeTextContainerListener();
						}
					}
				}
				textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
			}
			textPointer = new TextPointer(this.TextContainer.Start);
			string text = AccessText.AccessKeyMarker.ToString();
			string oldValue = text + text;
			while (textPointer.CompareTo(this.TextContainer.End) < 0)
			{
				TextPointerContext pointerContext2 = textPointer.GetPointerContext(LogicalDirection.Forward);
				if (pointerContext2 == TextPointerContext.Text)
				{
					string textInRun2 = textPointer.GetTextInRun(LogicalDirection.Forward);
					string text2 = textInRun2.Replace(oldValue, text);
					if (textInRun2 != text2)
					{
						TextPointer start = new TextPointer(textPointer, 0);
						TextPointer textPointer3 = new TextPointer(textPointer, textInRun2.Length);
						this.UninitializeTextContainerListener();
						this.TextContainer.BeginChange();
						try
						{
							textPointer3.InsertTextInRun(text2);
							TextRangeEdit.DeleteInlineContent(start, textPointer3);
						}
						finally
						{
							this.TextContainer.EndChange();
							this.InitializeTextContainerListener();
						}
					}
				}
				textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
			}
		}

		// Token: 0x06004217 RID: 16919 RVA: 0x0012E6C4 File Offset: 0x0012C8C4
		private static int FindAccessKeyMarker(string text)
		{
			int length = text.Length;
			int num;
			for (int i = 0; i < length; i = num + 2)
			{
				num = text.IndexOf(AccessText.AccessKeyMarker, i);
				if (num == -1)
				{
					return -1;
				}
				if (num + 1 < length && text[num + 1] != AccessText.AccessKeyMarker)
				{
					return num;
				}
			}
			return -1;
		}

		// Token: 0x06004218 RID: 16920 RVA: 0x0012E710 File Offset: 0x0012C910
		internal static string RemoveAccessKeyMarker(string text)
		{
			if (!string.IsNullOrEmpty(text))
			{
				string text2 = AccessText.AccessKeyMarker.ToString();
				string oldValue = text2 + text2;
				int num = AccessText.FindAccessKeyMarker(text);
				if (num >= 0 && num < text.Length - 1)
				{
					text = text.Remove(num, 1);
				}
				text = text.Replace(oldValue, text2);
			}
			return text;
		}

		// Token: 0x06004219 RID: 16921 RVA: 0x0012E768 File Offset: 0x0012C968
		private void RegisterAccessKey()
		{
			if (this._currentlyRegistered != null)
			{
				AccessKeyManager.Unregister(this._currentlyRegistered, this);
				this._currentlyRegistered = null;
			}
			string text = this._accessKey.Text;
			if (!string.IsNullOrEmpty(text))
			{
				AccessKeyManager.Register(text, this);
				this._currentlyRegistered = text;
			}
		}

		// Token: 0x0600421A RID: 16922 RVA: 0x0012E7B2 File Offset: 0x0012C9B2
		private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((AccessText)d).UpdateText((string)e.NewValue);
		}

		// Token: 0x0600421B RID: 16923 RVA: 0x0012E7CC File Offset: 0x0012C9CC
		private void UpdateText(string text)
		{
			if (text == null)
			{
				text = string.Empty;
			}
			this._accessKeyLocated = false;
			this._accessKey = null;
			this.TextContainer.BeginChange();
			try
			{
				this.TextContainer.DeleteContentInternal(this.TextContainer.Start, this.TextContainer.End);
				Run run = Inline.CreateImplicitRun(this);
				this.TextContainer.End.InsertTextElement(run);
				run.Text = text;
			}
			finally
			{
				this.TextContainer.EndChange();
			}
		}

		// Token: 0x0600421C RID: 16924 RVA: 0x0012E85C File Offset: 0x0012CA5C
		private void InitializeTextContainerListener()
		{
			this.TextContainer.Changed += this.OnTextContainerChanged;
		}

		// Token: 0x0600421D RID: 16925 RVA: 0x0012E875 File Offset: 0x0012CA75
		private void UninitializeTextContainerListener()
		{
			this.TextContainer.Changed -= this.OnTextContainerChanged;
		}

		// Token: 0x0600421E RID: 16926 RVA: 0x0012E88E File Offset: 0x0012CA8E
		private void OnTextContainerChanged(object sender, TextContainerChangedEventArgs args)
		{
			if (args.HasContentAddedOrRemoved)
			{
				this.UpdateAccessKey();
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.AccessText.Text" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.AccessText.Text" /> dependency property.</returns>
		// Token: 0x040027BD RID: 10173
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(AccessText), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(AccessText.OnTextChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.AccessText.FontFamily" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.AccessText.FontFamily" /> dependency property.</returns>
		// Token: 0x040027BE RID: 10174
		public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(AccessText));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.AccessText.FontStyle" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.AccessText.FontStyle" /> dependency property.</returns>
		// Token: 0x040027BF RID: 10175
		public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(AccessText));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.AccessText.FontWeight" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.AccessText.FontWeight" /> dependency property.</returns>
		// Token: 0x040027C0 RID: 10176
		public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof(AccessText));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.AccessText.FontStretch" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.AccessText.FontStretch" /> dependency property.</returns>
		// Token: 0x040027C1 RID: 10177
		public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(typeof(AccessText));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.AccessText.FontSize" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.AccessText.FontSize" /> dependency property.</returns>
		// Token: 0x040027C2 RID: 10178
		public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(AccessText));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.AccessText.Foreground" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.AccessText.Foreground" /> dependency property.</returns>
		// Token: 0x040027C3 RID: 10179
		public static readonly DependencyProperty ForegroundProperty = TextElement.ForegroundProperty.AddOwner(typeof(AccessText));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.AccessText.Background" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.AccessText.Background" /> dependency property.</returns>
		// Token: 0x040027C4 RID: 10180
		public static readonly DependencyProperty BackgroundProperty = TextElement.BackgroundProperty.AddOwner(typeof(AccessText), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(AccessText.OnPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.AccessText.TextDecorations" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.AccessText.TextDecorations" /> dependency property.</returns>
		// Token: 0x040027C5 RID: 10181
		public static readonly DependencyProperty TextDecorationsProperty = Inline.TextDecorationsProperty.AddOwner(typeof(AccessText), new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(TextDecorationCollection.Empty), FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(AccessText.OnPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.AccessText.TextEffects" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.AccessText.TextEffects" /> dependency property.</returns>
		// Token: 0x040027C6 RID: 10182
		public static readonly DependencyProperty TextEffectsProperty = TextElement.TextEffectsProperty.AddOwner(typeof(AccessText), new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(TextEffectCollection.Empty), FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(AccessText.OnPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.AccessText.LineHeight" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.AccessText.LineHeight" /> dependency property.</returns>
		// Token: 0x040027C7 RID: 10183
		public static readonly DependencyProperty LineHeightProperty = Block.LineHeightProperty.AddOwner(typeof(AccessText));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.AccessText.LineStackingStrategy" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.AccessText.LineStackingStrategy" /> dependency property.</returns>
		// Token: 0x040027C8 RID: 10184
		public static readonly DependencyProperty LineStackingStrategyProperty = Block.LineStackingStrategyProperty.AddOwner(typeof(AccessText));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.AccessText.TextAlignment" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.AccessText.TextAlignment" /> dependency property.</returns>
		// Token: 0x040027C9 RID: 10185
		public static readonly DependencyProperty TextAlignmentProperty = Block.TextAlignmentProperty.AddOwner(typeof(AccessText));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.AccessText.TextTrimming" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.AccessText.TextTrimming" /> dependency property.</returns>
		// Token: 0x040027CA RID: 10186
		public static readonly DependencyProperty TextTrimmingProperty = TextBlock.TextTrimmingProperty.AddOwner(typeof(AccessText), new FrameworkPropertyMetadata(TextTrimming.None, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(AccessText.OnPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.AccessText.TextWrapping" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.AccessText.TextWrapping" /> dependency property.</returns>
		// Token: 0x040027CB RID: 10187
		public static readonly DependencyProperty TextWrappingProperty = TextBlock.TextWrappingProperty.AddOwner(typeof(AccessText), new FrameworkPropertyMetadata(TextWrapping.NoWrap, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(AccessText.OnPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.AccessText.BaselineOffset" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.AccessText.BaselineOffset" /> dependency property.</returns>
		// Token: 0x040027CC RID: 10188
		public static readonly DependencyProperty BaselineOffsetProperty = TextBlock.BaselineOffsetProperty.AddOwner(typeof(AccessText), new FrameworkPropertyMetadata(new PropertyChangedCallback(AccessText.OnPropertyChanged)));

		// Token: 0x040027CD RID: 10189
		private TextContainer _textContainer;

		// Token: 0x040027CE RID: 10190
		private TextBlock _textBlock;

		// Token: 0x040027CF RID: 10191
		private Run _accessKey;

		// Token: 0x040027D0 RID: 10192
		private bool _accessKeyLocated;

		// Token: 0x040027D1 RID: 10193
		private const char _accessKeyMarker = '_';

		// Token: 0x040027D2 RID: 10194
		private static Style _accessKeyStyle;

		// Token: 0x040027D3 RID: 10195
		private string _currentlyRegistered;

		// Token: 0x040027D4 RID: 10196
		private static readonly UncommonField<bool> HasCustomSerializationStorage = new UncommonField<bool>();
	}
}
