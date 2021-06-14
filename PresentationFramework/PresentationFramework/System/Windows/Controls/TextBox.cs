using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Documents;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Represents a control that can be used to display or edit unformatted text.</summary>
	// Token: 0x0200053D RID: 1341
	[Localizability(LocalizationCategory.Text)]
	[ContentProperty("Text")]
	public class TextBox : TextBoxBase, IAddChild, ITextBoxViewHost
	{
		// Token: 0x0600578A RID: 22410 RVA: 0x001843FC File Offset: 0x001825FC
		static TextBox()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(typeof(TextBox)));
			TextBox._dType = DependencyObjectType.FromSystemTypeInternal(typeof(TextBox));
			PropertyChangedCallback propertyChangedCallback = new PropertyChangedCallback(TextBox.OnMinMaxChanged);
			FrameworkElement.HeightProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(propertyChangedCallback));
			FrameworkElement.MinHeightProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(propertyChangedCallback));
			FrameworkElement.MaxHeightProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(propertyChangedCallback));
			Control.FontFamilyProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(propertyChangedCallback));
			Control.FontSizeProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(propertyChangedCallback));
			PropertyChangedCallback propertyChangedCallback2 = new PropertyChangedCallback(TextBox.OnTypographyChanged);
			DependencyProperty[] typographyPropertiesList = Typography.TypographyPropertiesList;
			for (int i = 0; i < typographyPropertiesList.Length; i++)
			{
				typographyPropertiesList[i].OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(propertyChangedCallback2));
			}
			TextBoxBase.HorizontalScrollBarVisibilityProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(ScrollBarVisibility.Hidden, new PropertyChangedCallback(TextBoxBase.OnScrollViewerPropertyChanged), new CoerceValueCallback(TextBox.CoerceHorizontalScrollBarVisibility)));
			ControlsTraceLogger.AddControl(TelemetryControls.TextBox);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.TextBox" /> class.</summary>
		// Token: 0x0600578B RID: 22411 RVA: 0x00184714 File Offset: 0x00182914
		public TextBox()
		{
			TextEditor.RegisterCommandHandlers(typeof(TextBox), false, false, false);
			base.InitializeTextContainer(new TextContainer(this, true)
			{
				CollectTextChanges = true
			});
			base.TextEditor.AcceptsRichContent = false;
		}

		/// <summary>Throws an exception in all cases.</summary>
		/// <param name="value">An object to add as a child.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">In all other cases.</exception>
		// Token: 0x0600578C RID: 22412 RVA: 0x00184766 File Offset: 0x00182966
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			throw new InvalidOperationException(SR.Get("TextBoxInvalidChild", new object[]
			{
				value.ToString()
			}));
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">A string to add to the object.</param>
		// Token: 0x0600578D RID: 22413 RVA: 0x00184794 File Offset: 0x00182994
		void IAddChild.AddText(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			base.TextContainer.End.InsertTextInRun(text);
		}

		/// <summary>Selects a range of text in the text box.</summary>
		/// <param name="start">The zero-based character index of the first character in the selection.</param>
		/// <param name="length">The length of the selection, in characters.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="start" /> or <paramref name="length" /> is negative.</exception>
		// Token: 0x0600578E RID: 22414 RVA: 0x001847B8 File Offset: 0x001829B8
		public void Select(int start, int length)
		{
			if (start < 0)
			{
				throw new ArgumentOutOfRangeException("start", SR.Get("ParameterCannotBeNegative"));
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", SR.Get("ParameterCannotBeNegative"));
			}
			int symbolCount = base.TextContainer.SymbolCount;
			if (start > symbolCount)
			{
				start = symbolCount;
			}
			TextPointer textPointer = base.TextContainer.CreatePointerAtOffset(start, LogicalDirection.Forward);
			textPointer = textPointer.GetInsertionPosition(LogicalDirection.Forward);
			int offsetToPosition = textPointer.GetOffsetToPosition(base.TextContainer.End);
			if (length > offsetToPosition)
			{
				length = offsetToPosition;
			}
			TextPointer textPointer2 = new TextPointer(textPointer, length, LogicalDirection.Forward);
			textPointer2 = textPointer2.GetInsertionPosition(LogicalDirection.Forward);
			base.TextSelectionInternal.Select(textPointer, textPointer2);
		}

		/// <summary>Clears all the content from the text box.</summary>
		// Token: 0x0600578F RID: 22415 RVA: 0x00184858 File Offset: 0x00182A58
		public void Clear()
		{
			using (base.TextSelectionInternal.DeclareChangeBlock())
			{
				base.TextContainer.DeleteContentInternal(base.TextContainer.Start, base.TextContainer.End);
				base.TextSelectionInternal.Select(base.TextContainer.Start, base.TextContainer.Start);
			}
		}

		/// <summary>Returns the zero-based index of the character that is closest to the specified point.</summary>
		/// <param name="point">A point in <see cref="T:System.Windows.Controls.TextBox" /> coordinate-space for which to return an index.</param>
		/// <param name="snapToText">
		///       <see langword="true" /> to return the nearest index if there is no character at the specified point; <see langword="false" /> to return –1 if there is no character at the specified point.</param>
		/// <returns>The index of the character that is closest to the specified point, or –1 if no valid index can be found.</returns>
		// Token: 0x06005790 RID: 22416 RVA: 0x001848D0 File Offset: 0x00182AD0
		public int GetCharacterIndexFromPoint(Point point, bool snapToText)
		{
			if (base.RenderScope == null)
			{
				return -1;
			}
			TextPointer textPositionFromPointInternal = base.GetTextPositionFromPointInternal(point, snapToText);
			if (textPositionFromPointInternal == null)
			{
				return -1;
			}
			int offset = textPositionFromPointInternal.Offset;
			if (textPositionFromPointInternal.LogicalDirection != LogicalDirection.Backward)
			{
				return offset;
			}
			return offset - 1;
		}

		/// <summary>Returns the zero-based character index for the first character in the specified line.</summary>
		/// <param name="lineIndex">The zero-based index of the line to retrieve the initial character index for.</param>
		/// <returns>The zero-based index for the first character in the specified line.</returns>
		// Token: 0x06005791 RID: 22417 RVA: 0x0018490C File Offset: 0x00182B0C
		public int GetCharacterIndexFromLineIndex(int lineIndex)
		{
			if (base.RenderScope == null)
			{
				return -1;
			}
			if (lineIndex < 0 || lineIndex >= this.LineCount)
			{
				throw new ArgumentOutOfRangeException("lineIndex");
			}
			TextPointer startPositionOfLine = this.GetStartPositionOfLine(lineIndex);
			if (startPositionOfLine != null)
			{
				return startPositionOfLine.Offset;
			}
			return 0;
		}

		/// <summary>Returns the zero-based line index for the line that contains the specified character index.</summary>
		/// <param name="charIndex">The zero-based character index for which to retrieve the associated line index.</param>
		/// <returns>The zero-based index for the line that contains the specified character index.</returns>
		// Token: 0x06005792 RID: 22418 RVA: 0x00184950 File Offset: 0x00182B50
		public int GetLineIndexFromCharacterIndex(int charIndex)
		{
			if (base.RenderScope == null)
			{
				return -1;
			}
			if (charIndex < 0 || charIndex > base.TextContainer.SymbolCount)
			{
				throw new ArgumentOutOfRangeException("charIndex");
			}
			TextPointer textPointer = base.TextContainer.CreatePointerAtOffset(charIndex, LogicalDirection.Forward);
			int result;
			if (textPointer.ValidateLayout())
			{
				TextBoxView textBoxView = (TextBoxView)base.RenderScope;
				result = textBoxView.GetLineIndexFromOffset(charIndex);
			}
			else
			{
				result = -1;
			}
			return result;
		}

		/// <summary>Returns the number of characters in the specified line.</summary>
		/// <param name="lineIndex">The zero-based line index for which to return a character count.</param>
		/// <returns>The number of characters in the specified line.</returns>
		// Token: 0x06005793 RID: 22419 RVA: 0x001849B4 File Offset: 0x00182BB4
		public int GetLineLength(int lineIndex)
		{
			if (base.RenderScope == null)
			{
				return -1;
			}
			if (lineIndex < 0 || lineIndex >= this.LineCount)
			{
				throw new ArgumentOutOfRangeException("lineIndex");
			}
			TextPointer startPositionOfLine = this.GetStartPositionOfLine(lineIndex);
			TextPointer endPositionOfLine = this.GetEndPositionOfLine(lineIndex);
			int result;
			if (startPositionOfLine == null || endPositionOfLine == null)
			{
				result = -1;
			}
			else
			{
				result = startPositionOfLine.GetOffsetToPosition(endPositionOfLine);
			}
			return result;
		}

		/// <summary>Returns the line index for the first line that is currently visible in the text box.</summary>
		/// <returns>The zero-based index for the first visible line in the text box.</returns>
		// Token: 0x06005794 RID: 22420 RVA: 0x00184A08 File Offset: 0x00182C08
		public int GetFirstVisibleLineIndex()
		{
			if (base.RenderScope == null)
			{
				return -1;
			}
			double lineHeight = this.GetLineHeight();
			return (int)Math.Floor(base.VerticalOffset / lineHeight + 0.0001);
		}

		/// <summary>Returns the line index for the last line that is currently visible in the text box.</summary>
		/// <returns>The zero-based index for the last visible line in the text box.</returns>
		// Token: 0x06005795 RID: 22421 RVA: 0x00184A40 File Offset: 0x00182C40
		public int GetLastVisibleLineIndex()
		{
			if (base.RenderScope == null)
			{
				return -1;
			}
			double extentHeight = ((IScrollInfo)base.RenderScope).ExtentHeight;
			if (base.VerticalOffset + base.ViewportHeight >= extentHeight)
			{
				return this.LineCount - 1;
			}
			return (int)Math.Floor((base.VerticalOffset + base.ViewportHeight - 1.0) / this.GetLineHeight());
		}

		/// <summary>Scrolls the line at the specified line index into view.</summary>
		/// <param name="lineIndex">The zero-based line index of the line to scroll into view.</param>
		// Token: 0x06005796 RID: 22422 RVA: 0x00184AA8 File Offset: 0x00182CA8
		public void ScrollToLine(int lineIndex)
		{
			if (base.RenderScope == null)
			{
				return;
			}
			if (lineIndex < 0 || lineIndex >= this.LineCount)
			{
				throw new ArgumentOutOfRangeException("lineIndex");
			}
			TextPointer startPositionOfLine = this.GetStartPositionOfLine(lineIndex);
			Rect targetRectangle;
			if (this.GetRectangleFromTextPositionInternal(startPositionOfLine, false, out targetRectangle))
			{
				base.RenderScope.BringIntoView(targetRectangle);
			}
		}

		/// <summary>Returns the text that is currently displayed on the specified line.</summary>
		/// <param name="lineIndex">The zero-based line index for which to retrieve the currently displayed text.</param>
		/// <returns>A string containing a copy of the text currently visible on the specified line.</returns>
		// Token: 0x06005797 RID: 22423 RVA: 0x00184AF8 File Offset: 0x00182CF8
		public string GetLineText(int lineIndex)
		{
			if (base.RenderScope == null)
			{
				return null;
			}
			if (lineIndex < 0 || lineIndex >= this.LineCount)
			{
				throw new ArgumentOutOfRangeException("lineIndex");
			}
			TextPointer startPositionOfLine = this.GetStartPositionOfLine(lineIndex);
			TextPointer endPositionOfLine = this.GetEndPositionOfLine(lineIndex);
			string result;
			if (startPositionOfLine != null && endPositionOfLine != null)
			{
				result = TextRangeBase.GetTextInternal(startPositionOfLine, endPositionOfLine);
			}
			else
			{
				result = this.Text;
			}
			return result;
		}

		/// <summary>Returns the rectangle for the leading edge of the character at the specified index.</summary>
		/// <param name="charIndex">The zero-based character index of the character for which to retrieve the rectangle.</param>
		/// <returns>A rectangle for the leading edge of the character at the specified character index, or <see cref="P:System.Windows.Rect.Empty" /> if a bounding rectangle cannot be determined.</returns>
		// Token: 0x06005798 RID: 22424 RVA: 0x00184B4F File Offset: 0x00182D4F
		public Rect GetRectFromCharacterIndex(int charIndex)
		{
			return this.GetRectFromCharacterIndex(charIndex, false);
		}

		/// <summary>Returns the rectangle for the leading or trailing edge of the character at the specified index.</summary>
		/// <param name="charIndex">The zero-based character index of the character for which to retrieve the rectangle.</param>
		/// <param name="trailingEdge">
		///       <see langword="true" /> to get the trailing edge of the character; <see langword="false" /> to get the leading edge of the character.</param>
		/// <returns>A rectangle for an edge of the character at the specified character index, or <see cref="P:System.Windows.Rect.Empty" /> if a bounding rectangle cannot be determined.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="charIndex" /> is negative or is greater than the length of the content.</exception>
		// Token: 0x06005799 RID: 22425 RVA: 0x00184B5C File Offset: 0x00182D5C
		public Rect GetRectFromCharacterIndex(int charIndex, bool trailingEdge)
		{
			if (charIndex < 0 || charIndex > base.TextContainer.SymbolCount)
			{
				throw new ArgumentOutOfRangeException("charIndex");
			}
			TextPointer textPointer = base.TextContainer.CreatePointerAtOffset(charIndex, LogicalDirection.Backward);
			textPointer = textPointer.GetInsertionPosition(LogicalDirection.Backward);
			if (trailingEdge && charIndex < base.TextContainer.SymbolCount)
			{
				textPointer = textPointer.GetNextInsertionPosition(LogicalDirection.Forward);
				Invariant.Assert(textPointer != null);
				textPointer = textPointer.GetPositionAtOffset(0, LogicalDirection.Backward);
			}
			else
			{
				textPointer = textPointer.GetPositionAtOffset(0, LogicalDirection.Forward);
			}
			Rect result;
			this.GetRectangleFromTextPositionInternal(textPointer, true, out result);
			return result;
		}

		/// <summary>Returns a <see cref="T:System.Windows.Controls.SpellingError" /> object associated with any spelling error at the specified character index.</summary>
		/// <param name="charIndex">The zero-based character index of a position in content to examine for a spelling error.</param>
		/// <returns>A <see cref="T:System.Windows.Controls.SpellingError" /> object containing the details of the spelling error found at the character indicated by <paramref name="charIndex" />, or <see langword="null" /> if no spelling error exists at the specified character.</returns>
		// Token: 0x0600579A RID: 22426 RVA: 0x00184BE0 File Offset: 0x00182DE0
		public SpellingError GetSpellingError(int charIndex)
		{
			if (charIndex < 0 || charIndex > base.TextContainer.SymbolCount)
			{
				throw new ArgumentOutOfRangeException("charIndex");
			}
			TextPointer position = base.TextContainer.CreatePointerAtOffset(charIndex, LogicalDirection.Forward);
			SpellingError spellingErrorAtPosition = base.TextEditor.GetSpellingErrorAtPosition(position, LogicalDirection.Forward);
			if (spellingErrorAtPosition == null && charIndex < base.TextContainer.SymbolCount - 1)
			{
				position = base.TextContainer.CreatePointerAtOffset(charIndex + 1, LogicalDirection.Forward);
				spellingErrorAtPosition = base.TextEditor.GetSpellingErrorAtPosition(position, LogicalDirection.Backward);
			}
			return spellingErrorAtPosition;
		}

		/// <summary>Returns the beginning character index for any spelling error that includes the specified character.</summary>
		/// <param name="charIndex">The zero-based character index of a position in content to examine for a spelling error.</param>
		/// <returns>The beginning character index for any spelling error that includes the character specified by <paramref name="charIndex" />, or –1 if the specified character is not part of a spelling error.</returns>
		// Token: 0x0600579B RID: 22427 RVA: 0x00184C58 File Offset: 0x00182E58
		public int GetSpellingErrorStart(int charIndex)
		{
			SpellingError spellingError = this.GetSpellingError(charIndex);
			if (spellingError != null)
			{
				return spellingError.Start.Offset;
			}
			return -1;
		}

		/// <summary>Returns the length of any spelling error that includes the specified character.</summary>
		/// <param name="charIndex">The zero-based character index of a position in content to examine for a spelling error.</param>
		/// <returns>The length of any spelling error that includes the character specified by charIndex, or 0 if the specified character is not part of a spelling error.</returns>
		// Token: 0x0600579C RID: 22428 RVA: 0x00184C80 File Offset: 0x00182E80
		public int GetSpellingErrorLength(int charIndex)
		{
			SpellingError spellingError = this.GetSpellingError(charIndex);
			if (spellingError != null)
			{
				return spellingError.End.Offset - spellingError.Start.Offset;
			}
			return 0;
		}

		/// <summary>Returns the beginning character index for the next spelling error in the contents of the text box.</summary>
		/// <param name="charIndex">The zero-based character index indicating a position from which to search for the next spelling error.</param>
		/// <param name="direction">One of the <see cref="T:System.Windows.Documents.LogicalDirection" /> values that specifies the direction in which to search for the next spelling error, starting at the specified <paramref name="charIndex" />.</param>
		/// <returns>The character index for the beginning of the next spelling error in the contents of the text box, or –1 if no next spelling error exists.</returns>
		// Token: 0x0600579D RID: 22429 RVA: 0x00184CB4 File Offset: 0x00182EB4
		public int GetNextSpellingErrorCharacterIndex(int charIndex, LogicalDirection direction)
		{
			if (charIndex < 0 || charIndex > base.TextContainer.SymbolCount)
			{
				throw new ArgumentOutOfRangeException("charIndex");
			}
			if (base.TextContainer.SymbolCount == 0)
			{
				return -1;
			}
			ITextPointer textPointer = base.TextContainer.CreatePointerAtOffset(charIndex, direction);
			textPointer = base.TextEditor.GetNextSpellingErrorPosition(textPointer, direction);
			if (textPointer != null)
			{
				return textPointer.Offset;
			}
			return -1;
		}

		/// <summary>Gets or sets how the text box should wrap text.</summary>
		/// <returns>One of the <see cref="T:System.Windows.TextWrapping" /> values that indicates how the text box should wrap text. The default is <see cref="F:System.Windows.TextWrapping.NoWrap" />.</returns>
		// Token: 0x17001551 RID: 5457
		// (get) Token: 0x0600579E RID: 22430 RVA: 0x00184D14 File Offset: 0x00182F14
		// (set) Token: 0x0600579F RID: 22431 RVA: 0x00184D26 File Offset: 0x00182F26
		public TextWrapping TextWrapping
		{
			get
			{
				return (TextWrapping)base.GetValue(TextBox.TextWrappingProperty);
			}
			set
			{
				base.SetValue(TextBox.TextWrappingProperty, value);
			}
		}

		/// <summary>Gets or sets the minimum number of visible lines.</summary>
		/// <returns>The minimum number of visible lines. The default is 1.</returns>
		/// <exception cref="T:System.Exception">
		///         <see cref="P:System.Windows.Controls.TextBox.MinLines" /> is greater than <see cref="P:System.Windows.Controls.TextBox.MaxLines" />.</exception>
		// Token: 0x17001552 RID: 5458
		// (get) Token: 0x060057A0 RID: 22432 RVA: 0x00184D39 File Offset: 0x00182F39
		// (set) Token: 0x060057A1 RID: 22433 RVA: 0x00184D4B File Offset: 0x00182F4B
		[DefaultValue(1)]
		public int MinLines
		{
			get
			{
				return (int)base.GetValue(TextBox.MinLinesProperty);
			}
			set
			{
				base.SetValue(TextBox.MinLinesProperty, value);
			}
		}

		/// <summary>Gets or sets the maximum number of visible lines.</summary>
		/// <returns>The maximum number of visible lines. The default is <see cref="F:System.Int32.MaxValue" />.</returns>
		/// <exception cref="T:System.Exception">
		///         <see cref="P:System.Windows.Controls.TextBox.MaxLines" /> is less than <see cref="P:System.Windows.Controls.TextBox.MinLines" />.</exception>
		// Token: 0x17001553 RID: 5459
		// (get) Token: 0x060057A2 RID: 22434 RVA: 0x00184D5E File Offset: 0x00182F5E
		// (set) Token: 0x060057A3 RID: 22435 RVA: 0x00184D70 File Offset: 0x00182F70
		[DefaultValue(2147483647)]
		public int MaxLines
		{
			get
			{
				return (int)base.GetValue(TextBox.MaxLinesProperty);
			}
			set
			{
				base.SetValue(TextBox.MaxLinesProperty, value);
			}
		}

		/// <summary>Gets or sets the text contents of the text box.</summary>
		/// <returns>A string containing the text contents of the text box. The default is an empty string ("").</returns>
		// Token: 0x17001554 RID: 5460
		// (get) Token: 0x060057A4 RID: 22436 RVA: 0x00184D83 File Offset: 0x00182F83
		// (set) Token: 0x060057A5 RID: 22437 RVA: 0x00184D95 File Offset: 0x00182F95
		[DefaultValue("")]
		[Localizability(LocalizationCategory.Text)]
		public string Text
		{
			get
			{
				return (string)base.GetValue(TextBox.TextProperty);
			}
			set
			{
				base.SetValue(TextBox.TextProperty, value);
			}
		}

		/// <summary>Gets or sets how characters are cased when they are manually entered into the text box.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Controls.CharacterCasing" /> values that specifies how manually entered characters are cased. The default is <see cref="F:System.Windows.Controls.CharacterCasing.Normal" />.</returns>
		// Token: 0x17001555 RID: 5461
		// (get) Token: 0x060057A6 RID: 22438 RVA: 0x00184DA3 File Offset: 0x00182FA3
		// (set) Token: 0x060057A7 RID: 22439 RVA: 0x00184DB5 File Offset: 0x00182FB5
		public CharacterCasing CharacterCasing
		{
			get
			{
				return (CharacterCasing)base.GetValue(TextBox.CharacterCasingProperty);
			}
			set
			{
				base.SetValue(TextBox.CharacterCasingProperty, value);
			}
		}

		/// <summary>Gets or sets the maximum number of characters that can be manually entered into the text box.</summary>
		/// <returns>The maximum number of characters that can be manually entered into the text box. The default is 0, which indicates no limit.</returns>
		// Token: 0x17001556 RID: 5462
		// (get) Token: 0x060057A8 RID: 22440 RVA: 0x00184DC8 File Offset: 0x00182FC8
		// (set) Token: 0x060057A9 RID: 22441 RVA: 0x00184DDA File Offset: 0x00182FDA
		[DefaultValue(0)]
		[Localizability(LocalizationCategory.None, Modifiability = Modifiability.Unmodifiable)]
		public int MaxLength
		{
			get
			{
				return (int)base.GetValue(TextBox.MaxLengthProperty);
			}
			set
			{
				base.SetValue(TextBox.MaxLengthProperty, value);
			}
		}

		/// <summary>Gets or sets the horizontal alignment of the contents of the text box. </summary>
		/// <returns>One of the <see cref="T:System.Windows.TextAlignment" /> values that specifies the horizontal alignment of the contents of the text box. The default is <see cref="F:System.Windows.TextAlignment.Left" />.</returns>
		// Token: 0x17001557 RID: 5463
		// (get) Token: 0x060057AA RID: 22442 RVA: 0x00184DED File Offset: 0x00182FED
		// (set) Token: 0x060057AB RID: 22443 RVA: 0x00184DFF File Offset: 0x00182FFF
		public TextAlignment TextAlignment
		{
			get
			{
				return (TextAlignment)base.GetValue(TextBox.TextAlignmentProperty);
			}
			set
			{
				base.SetValue(TextBox.TextAlignmentProperty, value);
			}
		}

		/// <summary>Gets or sets the content of the current selection in the text box.</summary>
		/// <returns>The currently selected text in the text box.</returns>
		// Token: 0x17001558 RID: 5464
		// (get) Token: 0x060057AC RID: 22444 RVA: 0x00184E12 File Offset: 0x00183012
		// (set) Token: 0x060057AD RID: 22445 RVA: 0x00184E20 File Offset: 0x00183020
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string SelectedText
		{
			get
			{
				return base.TextSelectionInternal.Text;
			}
			set
			{
				using (base.TextSelectionInternal.DeclareChangeBlock())
				{
					base.TextSelectionInternal.Text = value;
				}
			}
		}

		/// <summary>Gets or sets a value indicating the number of characters in the current selection in the text box.</summary>
		/// <returns>The number of characters in the current selection in the text box. The default is 0.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <see cref="P:System.Windows.Controls.TextBox.SelectionLength" /> is set to a negative value.</exception>
		// Token: 0x17001559 RID: 5465
		// (get) Token: 0x060057AE RID: 22446 RVA: 0x00184E64 File Offset: 0x00183064
		// (set) Token: 0x060057AF RID: 22447 RVA: 0x00184E84 File Offset: 0x00183084
		[DefaultValue(0)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SelectionLength
		{
			get
			{
				return base.TextSelectionInternal.Start.GetOffsetToPosition(base.TextSelectionInternal.End);
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value", SR.Get("ParameterCannotBeNegative"));
				}
				int offsetToPosition = base.TextSelectionInternal.Start.GetOffsetToPosition(base.TextContainer.End);
				if (value > offsetToPosition)
				{
					value = offsetToPosition;
				}
				TextPointer textPointer = new TextPointer(base.TextSelectionInternal.Start, value, LogicalDirection.Forward);
				textPointer = textPointer.GetInsertionPosition(LogicalDirection.Forward);
				base.TextSelectionInternal.Select(base.TextSelectionInternal.Start, textPointer);
			}
		}

		/// <summary>Gets or sets a character index for the beginning of the current selection.</summary>
		/// <returns>The character index for the beginning of the current selection.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <see cref="P:System.Windows.Controls.TextBox.SelectionStart" /> is set to a negative value.</exception>
		// Token: 0x1700155A RID: 5466
		// (get) Token: 0x060057B0 RID: 22448 RVA: 0x00184EFF File Offset: 0x001830FF
		// (set) Token: 0x060057B1 RID: 22449 RVA: 0x00184F14 File Offset: 0x00183114
		[DefaultValue(0)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SelectionStart
		{
			get
			{
				return base.TextSelectionInternal.Start.Offset;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value", SR.Get("ParameterCannotBeNegative"));
				}
				int num = base.TextSelectionInternal.Start.GetOffsetToPosition(base.TextSelectionInternal.End);
				int symbolCount = base.TextContainer.SymbolCount;
				if (value > symbolCount)
				{
					value = symbolCount;
				}
				TextPointer textPointer = base.TextContainer.CreatePointerAtOffset(value, LogicalDirection.Forward);
				textPointer = textPointer.GetInsertionPosition(LogicalDirection.Forward);
				int offsetToPosition = textPointer.GetOffsetToPosition(base.TextContainer.End);
				if (num > offsetToPosition)
				{
					num = offsetToPosition;
				}
				TextPointer textPointer2 = new TextPointer(textPointer, num, LogicalDirection.Forward);
				textPointer2 = textPointer2.GetInsertionPosition(LogicalDirection.Forward);
				base.TextSelectionInternal.Select(textPointer, textPointer2);
			}
		}

		/// <summary>Gets or sets the insertion position index of the caret.</summary>
		/// <returns>The zero-based insertion position index of the caret. </returns>
		// Token: 0x1700155B RID: 5467
		// (get) Token: 0x060057B2 RID: 22450 RVA: 0x00184FB9 File Offset: 0x001831B9
		// (set) Token: 0x060057B3 RID: 22451 RVA: 0x00184FC1 File Offset: 0x001831C1
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int CaretIndex
		{
			get
			{
				return this.SelectionStart;
			}
			set
			{
				this.Select(value, 0);
			}
		}

		/// <summary>Gets the total number of lines in the text box.</summary>
		/// <returns>The total number of lines in the text box, or –1 if layout information is not available.</returns>
		// Token: 0x1700155C RID: 5468
		// (get) Token: 0x060057B4 RID: 22452 RVA: 0x00184FCB File Offset: 0x001831CB
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int LineCount
		{
			get
			{
				if (base.RenderScope == null)
				{
					return -1;
				}
				return this.GetLineIndexFromCharacterIndex(base.TextContainer.SymbolCount) + 1;
			}
		}

		/// <summary>Gets the text decorations to apply to the text box.</summary>
		/// <returns>A <see cref="T:System.Windows.TextDecorationCollection" /> collection that contains text decorations to apply to the text box. The default is <see langword="null" /> (no text decorations applied).</returns>
		// Token: 0x1700155D RID: 5469
		// (get) Token: 0x060057B5 RID: 22453 RVA: 0x00184FEA File Offset: 0x001831EA
		// (set) Token: 0x060057B6 RID: 22454 RVA: 0x00184FFC File Offset: 0x001831FC
		public TextDecorationCollection TextDecorations
		{
			get
			{
				return (TextDecorationCollection)base.GetValue(TextBox.TextDecorationsProperty);
			}
			set
			{
				base.SetValue(TextBox.TextDecorationsProperty, value);
			}
		}

		/// <summary>Gets the currently effective typography variations for the text contents of the text box.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.Typography" /> object that specifies the currently effective typography variations. For a list of default typography values, see <see cref="T:System.Windows.Documents.Typography" />.</returns>
		// Token: 0x1700155E RID: 5470
		// (get) Token: 0x060057B7 RID: 22455 RVA: 0x000D5502 File Offset: 0x000D3702
		public Typography Typography
		{
			get
			{
				return new Typography(this);
			}
		}

		/// <summary>Creates and returns an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> object for the text box.</summary>
		/// <returns>An <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> object for the text box.</returns>
		// Token: 0x060057B8 RID: 22456 RVA: 0x0018500A File Offset: 0x0018320A
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new TextBoxAutomationPeer(this);
		}

		/// <summary>Called when one or more of the dependency properties that exist on the element have had their effective values changed.</summary>
		/// <param name="e">Arguments for the associated event.</param>
		// Token: 0x060057B9 RID: 22457 RVA: 0x00185014 File Offset: 0x00183214
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (base.RenderScope != null)
			{
				FrameworkPropertyMetadata frameworkPropertyMetadata = e.Property.GetMetadata(typeof(TextBox)) as FrameworkPropertyMetadata;
				if (frameworkPropertyMetadata != null && (e.IsAValueChange || e.IsASubPropertyChange || e.Property == TextBox.TextAlignmentProperty))
				{
					if (frameworkPropertyMetadata.AffectsMeasure || frameworkPropertyMetadata.AffectsArrange || frameworkPropertyMetadata.AffectsParentMeasure || frameworkPropertyMetadata.AffectsParentArrange || e.Property == Control.HorizontalContentAlignmentProperty || e.Property == Control.VerticalContentAlignmentProperty)
					{
						((TextBoxView)base.RenderScope).Remeasure();
					}
					else if (frameworkPropertyMetadata.AffectsRender && (e.IsAValueChange || !frameworkPropertyMetadata.SubPropertiesDoNotAffectRender))
					{
						((TextBoxView)base.RenderScope).Rerender();
					}
					if (Speller.IsSpellerAffectingProperty(e.Property) && base.TextEditor.Speller != null)
					{
						base.TextEditor.Speller.ResetErrors();
					}
				}
			}
			TextBoxAutomationPeer textBoxAutomationPeer = UIElementAutomationPeer.FromElement(this) as TextBoxAutomationPeer;
			if (textBoxAutomationPeer != null)
			{
				if (e.Property == TextBox.TextProperty)
				{
					textBoxAutomationPeer.RaiseValuePropertyChangedEvent((string)e.OldValue, (string)e.NewValue);
				}
				if (e.Property == TextBoxBase.IsReadOnlyProperty)
				{
					textBoxAutomationPeer.RaiseIsReadOnlyPropertyChangedEvent((bool)e.OldValue, (bool)e.NewValue);
				}
			}
		}

		/// <summary>Gets an enumerator for the logical child elements of the <see cref="T:System.Windows.Controls.TextBox" />.</summary>
		/// <returns>An enumerator for the logical child elements of the <see cref="T:System.Windows.Controls.TextBox" />.</returns>
		// Token: 0x1700155F RID: 5471
		// (get) Token: 0x060057BA RID: 22458 RVA: 0x00185181 File Offset: 0x00183381
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				return new RangeContentEnumerator(base.TextContainer.Start, base.TextContainer.End);
			}
		}

		/// <summary>Sizes the text box to its content.</summary>
		/// <param name="constraint">A <see cref="T:System.Windows.Size" /> structure that specifies the constraints on the size of the text box.</param>
		/// <returns>A <see cref="T:System.Windows.Size" /> structure indicating the new size of the text box.</returns>
		// Token: 0x060057BB RID: 22459 RVA: 0x001851A0 File Offset: 0x001833A0
		protected override Size MeasureOverride(Size constraint)
		{
			if (this.MinLines > 1 && this.MaxLines < this.MinLines)
			{
				throw new Exception(SR.Get("TextBoxMinMaxLinesMismatch"));
			}
			Size result = base.MeasureOverride(constraint);
			if (this._minmaxChanged)
			{
				if (base.ScrollViewer == null)
				{
					this.SetRenderScopeMinMaxHeight();
				}
				else
				{
					this.SetScrollViewerMinMaxHeight();
				}
				this._minmaxChanged = false;
			}
			return result;
		}

		// Token: 0x060057BC RID: 22460 RVA: 0x00185202 File Offset: 0x00183402
		internal void OnTextWrappingChanged()
		{
			base.CoerceValue(TextBoxBase.HorizontalScrollBarVisibilityProperty);
		}

		// Token: 0x060057BD RID: 22461 RVA: 0x0018520F File Offset: 0x0018340F
		internal override FrameworkElement CreateRenderScope()
		{
			return new TextBoxView(this);
		}

		// Token: 0x060057BE RID: 22462 RVA: 0x00185217 File Offset: 0x00183417
		internal override void AttachToVisualTree()
		{
			base.AttachToVisualTree();
			if (base.RenderScope == null)
			{
				return;
			}
			this.OnTextWrappingChanged();
			this._minmaxChanged = true;
		}

		// Token: 0x060057BF RID: 22463 RVA: 0x00185235 File Offset: 0x00183435
		internal override string GetPlainText()
		{
			return this.Text;
		}

		// Token: 0x17001560 RID: 5472
		// (get) Token: 0x060057C0 RID: 22464 RVA: 0x0018523D File Offset: 0x0018343D
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return TextBox._dType;
			}
		}

		// Token: 0x060057C1 RID: 22465 RVA: 0x00185244 File Offset: 0x00183444
		internal override void DoLineUp()
		{
			if (base.ScrollViewer != null)
			{
				base.ScrollViewer.ScrollToVerticalOffset(base.VerticalOffset - this.GetLineHeight());
			}
		}

		// Token: 0x060057C2 RID: 22466 RVA: 0x00185266 File Offset: 0x00183466
		internal override void DoLineDown()
		{
			if (base.ScrollViewer != null)
			{
				base.ScrollViewer.ScrollToVerticalOffset(base.VerticalOffset + this.GetLineHeight());
			}
		}

		// Token: 0x060057C3 RID: 22467 RVA: 0x00185288 File Offset: 0x00183488
		internal override void OnTextContainerChanged(object sender, TextContainerChangedEventArgs e)
		{
			bool flag = false;
			string text = null;
			try
			{
				this._changeEventNestingCount++;
				if (!this._isInsideTextContentChange)
				{
					this._isInsideTextContentChange = true;
					DeferredTextReference deferredTextReference = new DeferredTextReference(base.TextContainer);
					this._newTextValue = deferredTextReference;
					base.SetCurrentDeferredValue(TextBox.TextProperty, deferredTextReference);
				}
			}
			finally
			{
				this._changeEventNestingCount--;
				if (this._changeEventNestingCount == 0)
				{
					if (FrameworkCompatibilityPreferences.GetKeepTextBoxDisplaySynchronizedWithTextProperty())
					{
						text = (this._newTextValue as string);
						flag = (text != null && text != this.Text);
					}
					this._isInsideTextContentChange = false;
					this._newTextValue = DependencyProperty.UnsetValue;
				}
			}
			if (flag)
			{
				try
				{
					this._newTextValue = text;
					this._isInsideTextContentChange = true;
					this._changeEventNestingCount++;
					this.OnTextPropertyChanged(text, this.Text);
				}
				finally
				{
					this._changeEventNestingCount--;
					this._isInsideTextContentChange = false;
					this._newTextValue = DependencyProperty.UnsetValue;
				}
			}
			if (this._changeEventNestingCount == 0)
			{
				base.OnTextContainerChanged(sender, e);
			}
		}

		// Token: 0x060057C4 RID: 22468 RVA: 0x001853A4 File Offset: 0x001835A4
		internal void OnDeferredTextReferenceResolved(DeferredTextReference dtr, string s)
		{
			if (dtr == this._newTextValue)
			{
				this._newTextValue = s;
			}
		}

		// Token: 0x060057C5 RID: 22469 RVA: 0x001853B6 File Offset: 0x001835B6
		internal override void OnScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			base.OnScrollChanged(sender, e);
			if (e.ViewportHeightChange != 0.0)
			{
				this.SetScrollViewerMinMaxHeight();
			}
		}

		// Token: 0x060057C6 RID: 22470 RVA: 0x001853D7 File Offset: 0x001835D7
		internal void RaiseCourtesyTextChangedEvent()
		{
			this.OnTextChanged(new TextChangedEventArgs(TextBoxBase.TextChangedEvent, UndoAction.None));
		}

		// Token: 0x17001561 RID: 5473
		// (get) Token: 0x060057C7 RID: 22471 RVA: 0x000957A4 File Offset: 0x000939A4
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 42;
			}
		}

		// Token: 0x17001562 RID: 5474
		// (get) Token: 0x060057C8 RID: 22472 RVA: 0x00177A26 File Offset: 0x00175C26
		internal TextSelection Selection
		{
			get
			{
				return base.TextSelectionInternal;
			}
		}

		// Token: 0x17001563 RID: 5475
		// (get) Token: 0x060057C9 RID: 22473 RVA: 0x001853EA File Offset: 0x001835EA
		internal TextPointer StartPosition
		{
			get
			{
				return base.TextContainer.Start;
			}
		}

		// Token: 0x17001564 RID: 5476
		// (get) Token: 0x060057CA RID: 22474 RVA: 0x001853F7 File Offset: 0x001835F7
		internal TextPointer EndPosition
		{
			get
			{
				return base.TextContainer.End;
			}
		}

		// Token: 0x17001565 RID: 5477
		// (get) Token: 0x060057CB RID: 22475 RVA: 0x00185404 File Offset: 0x00183604
		internal bool IsTypographyDefaultValue
		{
			get
			{
				return !this._isTypographySet;
			}
		}

		// Token: 0x17001566 RID: 5478
		// (get) Token: 0x060057CC RID: 22476 RVA: 0x0018540F File Offset: 0x0018360F
		ITextContainer ITextBoxViewHost.TextContainer
		{
			get
			{
				return base.TextContainer;
			}
		}

		// Token: 0x17001567 RID: 5479
		// (get) Token: 0x060057CD RID: 22477 RVA: 0x00185417 File Offset: 0x00183617
		bool ITextBoxViewHost.IsTypographyDefaultValue
		{
			get
			{
				return this.IsTypographyDefaultValue;
			}
		}

		// Token: 0x060057CE RID: 22478 RVA: 0x00185420 File Offset: 0x00183620
		private bool GetRectangleFromTextPositionInternal(TextPointer position, bool relativeToTextBox, out Rect rect)
		{
			if (base.RenderScope == null)
			{
				rect = Rect.Empty;
				return false;
			}
			if (position.ValidateLayout())
			{
				rect = TextPointerBase.GetCharacterRect(position, position.LogicalDirection, relativeToTextBox);
			}
			else
			{
				rect = Rect.Empty;
			}
			return rect != Rect.Empty;
		}

		// Token: 0x060057CF RID: 22479 RVA: 0x0018547C File Offset: 0x0018367C
		private TextPointer GetStartPositionOfLine(int lineIndex)
		{
			if (base.RenderScope == null)
			{
				return null;
			}
			Point point = default(Point);
			double lineHeight = this.GetLineHeight();
			point.Y = lineHeight * (double)lineIndex + lineHeight / 2.0 - base.VerticalOffset;
			point.X = -base.HorizontalOffset;
			TextPointer textPointer;
			if (TextEditor.GetTextView(base.RenderScope).Validate(point))
			{
				textPointer = (TextPointer)TextEditor.GetTextView(base.RenderScope).GetTextPositionFromPoint(point, true);
				textPointer = (TextPointer)TextEditor.GetTextView(base.RenderScope).GetLineRange(textPointer).Start.CreatePointer(textPointer.LogicalDirection);
			}
			else
			{
				textPointer = null;
			}
			return textPointer;
		}

		// Token: 0x060057D0 RID: 22480 RVA: 0x00185528 File Offset: 0x00183728
		private TextPointer GetEndPositionOfLine(int lineIndex)
		{
			if (base.RenderScope == null)
			{
				return null;
			}
			Point point = default(Point);
			double lineHeight = this.GetLineHeight();
			point.Y = lineHeight * (double)lineIndex + lineHeight / 2.0 - base.VerticalOffset;
			point.X = 0.0;
			TextPointer textPointer;
			if (TextEditor.GetTextView(base.RenderScope).Validate(point))
			{
				textPointer = (TextPointer)TextEditor.GetTextView(base.RenderScope).GetTextPositionFromPoint(point, true);
				textPointer = (TextPointer)TextEditor.GetTextView(base.RenderScope).GetLineRange(textPointer).End.CreatePointer(textPointer.LogicalDirection);
				if (TextPointerBase.IsNextToPlainLineBreak(textPointer, LogicalDirection.Forward))
				{
					textPointer.MoveToNextInsertionPosition(LogicalDirection.Forward);
				}
			}
			else
			{
				textPointer = null;
			}
			return textPointer;
		}

		// Token: 0x060057D1 RID: 22481 RVA: 0x001855E8 File Offset: 0x001837E8
		private static object CoerceHorizontalScrollBarVisibility(DependencyObject d, object value)
		{
			TextBox textBox = d as TextBox;
			if (textBox != null && (textBox.TextWrapping == TextWrapping.Wrap || textBox.TextWrapping == TextWrapping.WrapWithOverflow))
			{
				return ScrollBarVisibility.Disabled;
			}
			return value;
		}

		// Token: 0x060057D2 RID: 22482 RVA: 0x0015A58B File Offset: 0x0015878B
		private static bool MaxLengthValidateValue(object value)
		{
			return (int)value >= 0;
		}

		// Token: 0x060057D3 RID: 22483 RVA: 0x00185618 File Offset: 0x00183818
		private static bool CharacterCasingValidateValue(object value)
		{
			return CharacterCasing.Normal <= (CharacterCasing)value && (CharacterCasing)value <= CharacterCasing.Upper;
		}

		// Token: 0x060057D4 RID: 22484 RVA: 0x0015A599 File Offset: 0x00158799
		private static bool MinLinesValidateValue(object value)
		{
			return (int)value > 0;
		}

		// Token: 0x060057D5 RID: 22485 RVA: 0x0015A599 File Offset: 0x00158799
		private static bool MaxLinesValidateValue(object value)
		{
			return (int)value > 0;
		}

		// Token: 0x060057D6 RID: 22486 RVA: 0x00185634 File Offset: 0x00183834
		private static void OnMinMaxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBox textBox = (TextBox)d;
			textBox._minmaxChanged = true;
		}

		// Token: 0x060057D7 RID: 22487 RVA: 0x00185650 File Offset: 0x00183850
		private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBox textBox = (TextBox)d;
			if (textBox._isInsideTextContentChange && (textBox._newTextValue == DependencyProperty.UnsetValue || textBox._newTextValue is DeferredTextReference))
			{
				return;
			}
			textBox.OnTextPropertyChanged((string)e.OldValue, (string)e.NewValue);
		}

		// Token: 0x060057D8 RID: 22488 RVA: 0x001856A8 File Offset: 0x001838A8
		private void OnTextPropertyChanged(string oldText, string newText)
		{
			bool flag = false;
			int start = 0;
			bool flag2 = false;
			if (this._isInsideTextContentChange)
			{
				if (object.Equals(this._newTextValue, newText))
				{
					return;
				}
				flag = true;
			}
			if (newText == null)
			{
				newText = string.Empty;
			}
			bool flag3 = base.HasExpression(base.LookupEntry(TextBox.TextProperty.GlobalIndex), TextBox.TextProperty);
			string oldText2 = oldText;
			if (flag)
			{
				flag2 = true;
				oldText2 = (string)this._newTextValue;
			}
			else if (flag3)
			{
				BindingExpressionBase bindingExpression = BindingOperations.GetBindingExpression(this, TextBox.TextProperty);
				flag2 = (bindingExpression != null && bindingExpression.IsInUpdate && bindingExpression.IsInTransfer);
			}
			if (flag2)
			{
				start = this.ChooseCaretIndex(this.CaretIndex, oldText2, newText);
			}
			if (flag)
			{
				this._newTextValue = newText;
			}
			this._isInsideTextContentChange = true;
			try
			{
				using (base.TextSelectionInternal.DeclareChangeBlock())
				{
					base.TextContainer.DeleteContentInternal(base.TextContainer.Start, base.TextContainer.End);
					base.TextContainer.End.InsertTextInRun(newText);
					this.Select(start, 0);
				}
			}
			finally
			{
				if (!flag)
				{
					this._isInsideTextContentChange = false;
				}
			}
			if (flag3)
			{
				UndoManager undoManager = base.TextEditor._GetUndoManager();
				if (undoManager != null && undoManager.IsEnabled)
				{
					undoManager.Clear();
				}
			}
		}

		// Token: 0x060057D9 RID: 22489 RVA: 0x00185800 File Offset: 0x00183A00
		private int ChooseCaretIndex(int oldIndex, string oldText, string newText)
		{
			int num = newText.IndexOf(oldText, StringComparison.Ordinal);
			if (oldText.Length > 0 && num >= 0)
			{
				return num + oldIndex;
			}
			if (oldIndex == 0)
			{
				return 0;
			}
			if (oldIndex == oldText.Length)
			{
				return newText.Length;
			}
			int num2 = 0;
			while (num2 < oldText.Length && num2 < newText.Length && oldText[num2] == newText[num2])
			{
				num2++;
			}
			int num3 = 0;
			while (num3 < oldText.Length && num3 < newText.Length && oldText[oldText.Length - 1 - num3] == newText[newText.Length - 1 - num3])
			{
				num3++;
			}
			if (2 * (num2 + num3) >= Math.Min(oldText.Length, newText.Length))
			{
				if (oldIndex <= num2)
				{
					return oldIndex;
				}
				if (oldIndex >= oldText.Length - num3)
				{
					return newText.Length - (oldText.Length - oldIndex);
				}
			}
			char value = oldText[oldIndex - 1];
			int i = newText.IndexOf(value);
			int num4 = -1;
			int num5 = 1;
			while (i >= 0)
			{
				int num6 = 1;
				num = i - 1;
				while (num >= 0 && oldIndex - (i - num) >= 0 && newText[num] == oldText[oldIndex - (i - num)])
				{
					num6++;
					num--;
				}
				num = i + 1;
				while (num < newText.Length && oldIndex + (num - i) < oldText.Length && newText[num] == oldText[oldIndex + (num - i)])
				{
					num6++;
					num++;
				}
				if (num6 > num5)
				{
					num4 = i + 1;
					num5 = num6;
				}
				i = newText.IndexOf(value, i + 1);
			}
			if (num4 >= 0)
			{
				return num4;
			}
			return newText.Length;
		}

		// Token: 0x060057DA RID: 22490 RVA: 0x0018599F File Offset: 0x00183B9F
		private static void OnTextWrappingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is TextBox)
			{
				((TextBox)d).OnTextWrappingChanged();
			}
		}

		// Token: 0x060057DB RID: 22491 RVA: 0x001859B4 File Offset: 0x00183BB4
		private void SetScrollViewerMinMaxHeight()
		{
			if (base.RenderScope == null)
			{
				return;
			}
			if (base.ReadLocalValue(FrameworkElement.HeightProperty) != DependencyProperty.UnsetValue || base.ReadLocalValue(FrameworkElement.MaxHeightProperty) != DependencyProperty.UnsetValue || base.ReadLocalValue(FrameworkElement.MinHeightProperty) != DependencyProperty.UnsetValue)
			{
				base.ScrollViewer.ClearValue(FrameworkElement.MinHeightProperty);
				base.ScrollViewer.ClearValue(FrameworkElement.MaxHeightProperty);
				return;
			}
			double num = base.ScrollViewer.ActualHeight - base.ViewportHeight;
			double lineHeight = this.GetLineHeight();
			double num2 = num + lineHeight * (double)this.MinLines;
			if (this.MinLines > 1 && base.ScrollViewer.MinHeight != num2)
			{
				base.ScrollViewer.MinHeight = num2;
			}
			num2 = num + lineHeight * (double)this.MaxLines;
			if (this.MaxLines < 2147483647 && base.ScrollViewer.MaxHeight != num2)
			{
				base.ScrollViewer.MaxHeight = num2;
			}
		}

		// Token: 0x060057DC RID: 22492 RVA: 0x00185AA0 File Offset: 0x00183CA0
		private void SetRenderScopeMinMaxHeight()
		{
			if (base.RenderScope == null)
			{
				return;
			}
			if (base.ReadLocalValue(FrameworkElement.HeightProperty) != DependencyProperty.UnsetValue || base.ReadLocalValue(FrameworkElement.MaxHeightProperty) != DependencyProperty.UnsetValue || base.ReadLocalValue(FrameworkElement.MinHeightProperty) != DependencyProperty.UnsetValue)
			{
				base.RenderScope.ClearValue(FrameworkElement.MinHeightProperty);
				base.RenderScope.ClearValue(FrameworkElement.MaxHeightProperty);
				return;
			}
			double lineHeight = this.GetLineHeight();
			double num = lineHeight * (double)this.MinLines;
			if (this.MinLines > 1 && base.RenderScope.MinHeight != num)
			{
				base.RenderScope.MinHeight = num;
			}
			num = lineHeight * (double)this.MaxLines;
			if (this.MaxLines < 2147483647 && base.RenderScope.MaxHeight != num)
			{
				base.RenderScope.MaxHeight = num;
			}
		}

		// Token: 0x060057DD RID: 22493 RVA: 0x00185B74 File Offset: 0x00183D74
		private double GetLineHeight()
		{
			FontFamily fontFamily = (FontFamily)base.GetValue(Control.FontFamilyProperty);
			double num = (double)base.GetValue(TextElement.FontSizeProperty);
			double result;
			if (TextOptions.GetTextFormattingMode(this) == TextFormattingMode.Ideal)
			{
				result = fontFamily.LineSpacing * num;
			}
			else
			{
				result = fontFamily.GetLineSpacingForDisplayMode(num, base.GetDpi().DpiScaleY);
			}
			return result;
		}

		/// <summary>Returns a value that indicates whether the effective value of the <see cref="P:System.Windows.Controls.TextBox.Text" /> property should be serialized during serialization of the <see cref="T:System.Windows.Controls.TextBox" /> object.</summary>
		/// <param name="manager">A serialization service manager object for this object.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.TextBox.Text" /> property should be serialized; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.NullReferenceException">
		///         <paramref name="manager" /> is <see langword="null" />.</exception>
		// Token: 0x060057DE RID: 22494 RVA: 0x00185BCD File Offset: 0x00183DCD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeText(XamlDesignerSerializationManager manager)
		{
			return manager.XmlWriter == null;
		}

		// Token: 0x060057DF RID: 22495 RVA: 0x000C2612 File Offset: 0x000C0812
		private static void OnQueryScrollCommand(object target, CanExecuteRoutedEventArgs args)
		{
			args.CanExecute = true;
		}

		// Token: 0x060057E0 RID: 22496 RVA: 0x00185BD8 File Offset: 0x00183DD8
		private static object CoerceText(DependencyObject d, object value)
		{
			if (value == null)
			{
				return string.Empty;
			}
			return value;
		}

		// Token: 0x060057E1 RID: 22497 RVA: 0x00185BE4 File Offset: 0x00183DE4
		private static void OnTypographyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBox textBox = (TextBox)d;
			textBox._isTypographySet = true;
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBox.TextWrapping" /> dependency property.</summary>
		// Token: 0x04002E82 RID: 11906
		public static readonly DependencyProperty TextWrappingProperty = TextBlock.TextWrappingProperty.AddOwner(typeof(TextBox), new FrameworkPropertyMetadata(TextWrapping.NoWrap, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(TextBox.OnTextWrappingChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBox.MinLines" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBox.MinLines" /> dependency property.</returns>
		// Token: 0x04002E83 RID: 11907
		public static readonly DependencyProperty MinLinesProperty = DependencyProperty.Register("MinLines", typeof(int), typeof(TextBox), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(TextBox.OnMinMaxChanged)), new ValidateValueCallback(TextBox.MinLinesValidateValue));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBox.MaxLines" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBox.MaxLines" /> dependency property.</returns>
		// Token: 0x04002E84 RID: 11908
		public static readonly DependencyProperty MaxLinesProperty = DependencyProperty.Register("MaxLines", typeof(int), typeof(TextBox), new FrameworkPropertyMetadata(int.MaxValue, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(TextBox.OnMinMaxChanged)), new ValidateValueCallback(TextBox.MaxLinesValidateValue));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBox.Text" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBox.Text" /> dependency property.</returns>
		// Token: 0x04002E85 RID: 11909
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TextBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(TextBox.OnTextPropertyChanged), new CoerceValueCallback(TextBox.CoerceText), true, UpdateSourceTrigger.LostFocus));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBox.CharacterCasing" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBox.CharacterCasing" /> dependency property.</returns>
		// Token: 0x04002E86 RID: 11910
		public static readonly DependencyProperty CharacterCasingProperty = DependencyProperty.Register("CharacterCasing", typeof(CharacterCasing), typeof(TextBox), new FrameworkPropertyMetadata(CharacterCasing.Normal), new ValidateValueCallback(TextBox.CharacterCasingValidateValue));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBox.MaxLength" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBox.MaxLength" /> dependency property.</returns>
		// Token: 0x04002E87 RID: 11911
		public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(int), typeof(TextBox), new FrameworkPropertyMetadata(0), new ValidateValueCallback(TextBox.MaxLengthValidateValue));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBox.TextAlignment" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBox.TextAlignment" /> dependency property.</returns>
		// Token: 0x04002E88 RID: 11912
		public static readonly DependencyProperty TextAlignmentProperty = Block.TextAlignmentProperty.AddOwner(typeof(TextBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBox.TextDecorations" /> dependency property.</summary>
		// Token: 0x04002E89 RID: 11913
		public static readonly DependencyProperty TextDecorationsProperty = Inline.TextDecorationsProperty.AddOwner(typeof(TextBox), new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(TextDecorationCollection.Empty), FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x04002E8A RID: 11914
		private static DependencyObjectType _dType;

		// Token: 0x04002E8B RID: 11915
		private bool _minmaxChanged;

		// Token: 0x04002E8C RID: 11916
		private bool _isInsideTextContentChange;

		// Token: 0x04002E8D RID: 11917
		private object _newTextValue = DependencyProperty.UnsetValue;

		// Token: 0x04002E8E RID: 11918
		private bool _isTypographySet;

		// Token: 0x04002E8F RID: 11919
		private int _changeEventNestingCount;
	}
}
