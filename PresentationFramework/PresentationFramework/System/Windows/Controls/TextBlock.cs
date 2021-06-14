using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.Documents;
using MS.Internal.PresentationFramework;
using MS.Internal.Telemetry.PresentationFramework;
using MS.Internal.Text;

namespace System.Windows.Controls
{
	/// <summary>Provides a lightweight control for displaying small amounts of flow content.</summary>
	// Token: 0x0200053C RID: 1340
	[ContentProperty("Inlines")]
	[Localizability(LocalizationCategory.Text)]
	public class TextBlock : FrameworkElement, IContentHost, IAddChildInternal, IAddChild, IServiceProvider
	{
		/// <summary>This method supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.  Use the type-safe <see cref="M:System.Windows.Controls.TextBlock.InputHitTestCore(System.Windows.Point)" /> method instead.</summary>
		/// <param name="point">Mouse coordinates relative to the content host. </param>
		/// <returns>The element that has been hit. </returns>
		// Token: 0x060056EC RID: 22252 RVA: 0x00180B60 File Offset: 0x0017ED60
		IInputElement IContentHost.InputHitTest(Point point)
		{
			return this.InputHitTestCore(point);
		}

		/// <summary>This method supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.  Use the type-safe <see cref="M:System.Windows.Controls.TextBlock.GetRectanglesCore(System.Windows.ContentElement)" /> method instead.</summary>
		/// <param name="child"> A <see cref="T:System.Windows.ContentElement" /> for which to generate and return a collection of bounding rectangles.</param>
		/// <returns> A read-only collection of bounding rectangles for the specified <see cref="T:System.Windows.ContentElement" />.</returns>
		// Token: 0x060056ED RID: 22253 RVA: 0x00180B69 File Offset: 0x0017ED69
		ReadOnlyCollection<Rect> IContentHost.GetRectangles(ContentElement child)
		{
			return this.GetRectanglesCore(child);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.  Use the type-safe <see cref="P:System.Windows.Controls.TextBlock.HostedElementsCore" /> property instead.</summary>
		/// <returns>Elements hosted by the content host. </returns>
		// Token: 0x17001528 RID: 5416
		// (get) Token: 0x060056EE RID: 22254 RVA: 0x00180B72 File Offset: 0x0017ED72
		IEnumerator<IInputElement> IContentHost.HostedElements
		{
			get
			{
				return this.HostedElementsCore;
			}
		}

		/// <summary>This method supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.  Use the type-safe <see cref="M:System.Windows.Controls.TextBlock.OnChildDesiredSizeChangedCore(System.Windows.UIElement)" /> method instead.</summary>
		/// <param name="child"> The child <see cref="T:System.Windows.UIElement" /> element whose <see cref="P:System.Windows.UIElement.DesiredSize" /> has changed.</param>
		// Token: 0x060056EF RID: 22255 RVA: 0x00180B7A File Offset: 0x0017ED7A
		void IContentHost.OnChildDesiredSizeChanged(UIElement child)
		{
			this.OnChildDesiredSizeChangedCore(child);
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="value">An object to add as a child. </param>
		// Token: 0x060056F0 RID: 22256 RVA: 0x00180B84 File Offset: 0x0017ED84
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.EnsureComplexContent();
			if (!(this._complexContent.TextContainer is TextContainer))
			{
				throw new ArgumentException(SR.Get("TextPanelIllegalParaTypeForIAddChild", new object[]
				{
					"value",
					value.GetType()
				}));
			}
			Type type = this._complexContent.TextContainer.Parent.GetType();
			Type type2 = value.GetType();
			if (!TextSchema.IsValidChildOfContainer(type, type2))
			{
				if (!(value is UIElement))
				{
					throw new ArgumentException(SR.Get("TextSchema_ChildTypeIsInvalid", new object[]
					{
						type.Name,
						type2.Name
					}));
				}
				value = new InlineUIContainer((UIElement)value);
			}
			Invariant.Assert(value is Inline, "Schema validation helper must guarantee that invalid element is not passed here");
			TextContainer textContainer = (TextContainer)this._complexContent.TextContainer;
			textContainer.BeginChange();
			try
			{
				TextPointer end = textContainer.End;
				textContainer.InsertElementInternal(end, end, (Inline)value);
			}
			finally
			{
				textContainer.EndChange();
			}
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="text">A string to add to the object. </param>
		// Token: 0x060056F1 RID: 22257 RVA: 0x00180C9C File Offset: 0x0017EE9C
		void IAddChild.AddText(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			if (this._complexContent == null)
			{
				this.Text += text;
				return;
			}
			TextContainer textContainer = (TextContainer)this._complexContent.TextContainer;
			textContainer.BeginChange();
			try
			{
				TextPointer end = textContainer.End;
				Run run = Inline.CreateImplicitRun(this);
				textContainer.InsertElementInternal(end, end, run);
				run.Text = text;
			}
			finally
			{
				textContainer.EndChange();
			}
		}

		/// <summary>Gets an enumerator that can iterate the logical children of the <see cref="T:System.Windows.Controls.TextBlock" />.</summary>
		/// <returns>An enumerator for the logical children.</returns>
		// Token: 0x17001529 RID: 5417
		// (get) Token: 0x060056F2 RID: 22258 RVA: 0x00180D20 File Offset: 0x0017EF20
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				if (this.IsContentPresenterContainer)
				{
					return EmptyEnumerator.Instance;
				}
				if (this._complexContent == null)
				{
					return new TextBlock.SimpleContentEnumerator(this.Text);
				}
				if (!this._complexContent.ForeignTextContainer)
				{
					return new RangeContentEnumerator(this.ContentStart, this.ContentEnd);
				}
				return EmptyEnumerator.Instance;
			}
		}

		/// <summary>This method supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="serviceType">An object that specifies the type of service object to get. </param>
		/// <returns>A service object of type <paramref name="serviceType" />, or <see langword="null" /> if there is no service object of type <paramref name="serviceType" />. </returns>
		// Token: 0x060056F3 RID: 22259 RVA: 0x00180D74 File Offset: 0x0017EF74
		object IServiceProvider.GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			if (serviceType == typeof(ITextView))
			{
				this.EnsureComplexContent();
				return this._complexContent.TextView;
			}
			if (serviceType == typeof(ITextContainer))
			{
				this.EnsureComplexContent();
				return this._complexContent.TextContainer;
			}
			if (serviceType == typeof(TextContainer))
			{
				this.EnsureComplexContent();
				return this._complexContent.TextContainer as TextContainer;
			}
			return null;
		}

		// Token: 0x060056F4 RID: 22260 RVA: 0x00180E08 File Offset: 0x0017F008
		static TextBlock()
		{
			TextBlock.BaselineOffsetProperty.OverrideMetadata(typeof(TextBlock), new FrameworkPropertyMetadata(null, new CoerceValueCallback(TextBlock.CoerceBaselineOffset)));
			PropertyChangedCallback propertyChangedCallback = new PropertyChangedCallback(TextBlock.OnTypographyChanged);
			DependencyProperty[] typographyPropertiesList = Typography.TypographyPropertiesList;
			for (int i = 0; i < typographyPropertiesList.Length; i++)
			{
				typographyPropertiesList[i].OverrideMetadata(typeof(TextBlock), new FrameworkPropertyMetadata(propertyChangedCallback));
			}
			EventManager.RegisterClassHandler(typeof(TextBlock), FrameworkElement.RequestBringIntoViewEvent, new RequestBringIntoViewEventHandler(TextBlock.OnRequestBringIntoView));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBlock), new FrameworkPropertyMetadata(typeof(TextBlock)));
			ControlsTraceLogger.AddControl(TelemetryControls.TextBlock);
		}

		/// <summary> Initializes a new instance of the <see cref="T:System.Windows.Controls.TextBlock" /> class.</summary>
		// Token: 0x060056F5 RID: 22261 RVA: 0x0018116C File Offset: 0x0017F36C
		public TextBlock()
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.TextBlock" /> class, adding a specified <see cref="T:System.Windows.Documents.Inline" /> element as the initial display content.</summary>
		/// <param name="inline">An object deriving from the abstract <see cref="T:System.Windows.Documents.Inline" /> class, to be added as the initial content.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="inline" /> is <see langword="null" />.</exception>
		// Token: 0x060056F6 RID: 22262 RVA: 0x0018117A File Offset: 0x0017F37A
		public TextBlock(Inline inline)
		{
			this.Initialize();
			if (inline == null)
			{
				throw new ArgumentNullException("inline");
			}
			this.Inlines.Add(inline);
		}

		// Token: 0x060056F7 RID: 22263 RVA: 0x00002137 File Offset: 0x00000337
		private void Initialize()
		{
		}

		/// <summary>Returns a <see cref="T:System.Windows.Documents.TextPointer" /> to the position closest to a specified <see cref="T:System.Windows.Point" />.</summary>
		/// <param name="point">A <see cref="T:System.Windows.Point" /> in the coordinate space of the <see cref="T:System.Windows.Controls.TextBlock" /> for which to return a <see cref="T:System.Windows.Documents.TextPointer" />.</param>
		/// <param name="snapToText">
		///       <see langword="true" /> to return a <see cref="T:System.Windows.Documents.TextPointer" /> to the insertion point closest to <paramref name="point" />, whether or not <paramref name="point" /> is inside a character's bounding box; <see langword="false" /> to return <see langword="null" /> if <paramref name="point" /> is not inside a character's bounding box.</param>
		/// <returns>A <see cref="T:System.Windows.Documents.TextPointer" /> to the specified point, or <see langword="null" /> if <paramref name="snapToText" /> is <see langword="false" /> and the specified point does not fall within a character bounding box in the <see cref="T:System.Windows.Controls.TextBlock" /> content area.</returns>
		/// <exception cref="T:System.InvalidOperationException">Current, valid layout information for the control is unavailable.</exception>
		// Token: 0x060056F8 RID: 22264 RVA: 0x001811A4 File Offset: 0x0017F3A4
		public TextPointer GetPositionFromPoint(Point point, bool snapToText)
		{
			if (this.CheckFlags(TextBlock.Flags.ContentChangeInProgress))
			{
				throw new InvalidOperationException(SR.Get("TextContainerChangingReentrancyInvalid"));
			}
			this.EnsureComplexContent();
			TextPointer result;
			if (((ITextView)this._complexContent.TextView).Validate(point))
			{
				result = (TextPointer)this._complexContent.TextView.GetTextPositionFromPoint(point, snapToText);
			}
			else
			{
				result = (snapToText ? new TextPointer((TextPointer)this._complexContent.TextContainer.Start) : null);
			}
			return result;
		}

		/// <summary>Gets an <see cref="T:System.Windows.Documents.InlineCollection" /> containing the top-level <see cref="T:System.Windows.Documents.Inline" /> elements that comprise the contents of the <see cref="T:System.Windows.Controls.TextBlock" />.</summary>
		/// <returns>An <see cref="T:System.Windows.Documents.InlineCollection" /> containing the <see cref="T:System.Windows.Documents.Inline" /> elements that comprise the contents of the <see cref="T:System.Windows.Controls.TextBlock" />.</returns>
		// Token: 0x1700152A RID: 5418
		// (get) Token: 0x060056F9 RID: 22265 RVA: 0x000DCFC8 File Offset: 0x000DB1C8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public InlineCollection Inlines
		{
			get
			{
				return new InlineCollection(this, true);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Documents.TextPointer" /> to the beginning of content in the <see cref="T:System.Windows.Controls.TextBlock" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.TextPointer" /> to the beginning of content in the <see cref="T:System.Windows.Controls.TextBlock" />.</returns>
		// Token: 0x1700152B RID: 5419
		// (get) Token: 0x060056FA RID: 22266 RVA: 0x00181220 File Offset: 0x0017F420
		public TextPointer ContentStart
		{
			get
			{
				this.EnsureComplexContent();
				return (TextPointer)this._complexContent.TextContainer.Start;
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Documents.TextPointer" /> to the end of content in the <see cref="T:System.Windows.Controls.TextBlock" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.TextPointer" /> to the end of content in the <see cref="T:System.Windows.Controls.TextBlock" />.</returns>
		// Token: 0x1700152C RID: 5420
		// (get) Token: 0x060056FB RID: 22267 RVA: 0x0018123D File Offset: 0x0017F43D
		public TextPointer ContentEnd
		{
			get
			{
				this.EnsureComplexContent();
				return (TextPointer)this._complexContent.TextContainer.End;
			}
		}

		// Token: 0x1700152D RID: 5421
		// (get) Token: 0x060056FC RID: 22268 RVA: 0x0018125A File Offset: 0x0017F45A
		internal TextRange TextRange
		{
			get
			{
				return new TextRange(this.ContentStart, this.ContentEnd);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.LineBreakCondition" /> that indicates how content should break before the current element. </summary>
		/// <returns>The conditions for breaking content after the current element.</returns>
		// Token: 0x1700152E RID: 5422
		// (get) Token: 0x060056FD RID: 22269 RVA: 0x0000B02A File Offset: 0x0000922A
		public LineBreakCondition BreakBefore
		{
			get
			{
				return LineBreakCondition.BreakDesired;
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.LineBreakCondition" /> that indicates how content should break after the current element.</summary>
		/// <returns>The conditions for breaking content after the current element.</returns>
		// Token: 0x1700152F RID: 5423
		// (get) Token: 0x060056FE RID: 22270 RVA: 0x0000B02A File Offset: 0x0000922A
		public LineBreakCondition BreakAfter
		{
			get
			{
				return LineBreakCondition.BreakDesired;
			}
		}

		/// <summary>Gets the currently effective typography variations for the contents of this element.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.Typography" /> object that specifies the currently effective typography variations. For a list of default typography values, see <see cref="T:System.Windows.Documents.Typography" />.</returns>
		// Token: 0x17001530 RID: 5424
		// (get) Token: 0x060056FF RID: 22271 RVA: 0x000D5502 File Offset: 0x000D3702
		public Typography Typography
		{
			get
			{
				return new Typography(this);
			}
		}

		/// <summary>Gets or sets the amount by which each line of text is offset from the baseline.  </summary>
		/// <returns>The amount by which each line of text is offset from the baseline, in device independent pixels. <see cref="F:System.Double.NaN" /> indicates that an optimal baseline offset is automatically calculated from the current font characteristics. The default is <see cref="F:System.Double.NaN" />.</returns>
		// Token: 0x17001531 RID: 5425
		// (get) Token: 0x06005700 RID: 22272 RVA: 0x0018126D File Offset: 0x0017F46D
		// (set) Token: 0x06005701 RID: 22273 RVA: 0x0018127F File Offset: 0x0017F47F
		public double BaselineOffset
		{
			get
			{
				return (double)base.GetValue(TextBlock.BaselineOffsetProperty);
			}
			set
			{
				base.SetValue(TextBlock.BaselineOffsetProperty, value);
			}
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.TextBlock.BaselineOffset" /> attached property on a specified dependency object.</summary>
		/// <param name="element">The dependency object on which to set the value of the <see cref="P:System.Windows.Controls.TextBlock.BaselineOffset" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06005702 RID: 22274 RVA: 0x00181292 File Offset: 0x0017F492
		public static void SetBaselineOffset(DependencyObject element, double value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextBlock.BaselineOffsetProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Controls.TextBlock.BaselineOffset" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object from which to retrieve the value of the <see cref="P:System.Windows.Controls.TextBlock.BaselineOffset" /> attached property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Controls.TextBlock.BaselineOffset" /> attached property on the specified dependency object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06005703 RID: 22275 RVA: 0x001812B3 File Offset: 0x0017F4B3
		public static double GetBaselineOffset(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(TextBlock.BaselineOffsetProperty);
		}

		/// <summary>Gets or sets the text contents of a <see cref="T:System.Windows.Controls.TextBlock" />.  </summary>
		/// <returns>The text contents of this <see cref="T:System.Windows.Controls.TextBlock" />. Note that all non-text content is stripped out, resulting in a plain text representation of the <see cref="T:System.Windows.Controls.TextBlock" /> contents. The default is <see cref="F:System.String.Empty" />.</returns>
		// Token: 0x17001532 RID: 5426
		// (get) Token: 0x06005704 RID: 22276 RVA: 0x001812D3 File Offset: 0x0017F4D3
		// (set) Token: 0x06005705 RID: 22277 RVA: 0x001812E5 File Offset: 0x0017F4E5
		[Localizability(LocalizationCategory.Text)]
		public string Text
		{
			get
			{
				return (string)base.GetValue(TextBlock.TextProperty);
			}
			set
			{
				base.SetValue(TextBlock.TextProperty, value);
			}
		}

		// Token: 0x06005706 RID: 22278 RVA: 0x001812F4 File Offset: 0x0017F4F4
		private static object CoerceText(DependencyObject d, object value)
		{
			TextBlock textBlock = (TextBlock)d;
			if (value == null)
			{
				value = string.Empty;
			}
			if (textBlock._complexContent != null && !textBlock.CheckFlags(TextBlock.Flags.TextContentChanging) && (string)value == (string)textBlock.GetValue(TextBlock.TextProperty))
			{
				TextBlock.OnTextChanged(d, (string)value);
			}
			return value;
		}

		/// <summary>Gets or sets the preferred top-level font family for the <see cref="T:System.Windows.Controls.TextBlock" />.  </summary>
		/// <returns>A <see cref="T:System.Windows.Media.FontFamily" /> object specifying the preferred font family, or a primary preferred font family with one or more fallback font families. The default is the font determined by the <see cref="P:System.Windows.SystemFonts.MessageFontFamily" /> value.</returns>
		// Token: 0x17001533 RID: 5427
		// (get) Token: 0x06005707 RID: 22279 RVA: 0x00181351 File Offset: 0x0017F551
		// (set) Token: 0x06005708 RID: 22280 RVA: 0x00181363 File Offset: 0x0017F563
		[Localizability(LocalizationCategory.Font)]
		public FontFamily FontFamily
		{
			get
			{
				return (FontFamily)base.GetValue(TextBlock.FontFamilyProperty);
			}
			set
			{
				base.SetValue(TextBlock.FontFamilyProperty, value);
			}
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.TextBlock.FontFamily" /> attached property on a specified dependency object.</summary>
		/// <param name="element">The dependency object on which to set the value of the <see cref="P:System.Windows.Controls.TextBlock.FontFamily" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06005709 RID: 22281 RVA: 0x00181371 File Offset: 0x0017F571
		public static void SetFontFamily(DependencyObject element, FontFamily value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextBlock.FontFamilyProperty, value);
		}

		/// <summary>Returns the value of the <see cref="F:System.Windows.Controls.TextBlock.FontFamilyProperty" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object from which to retrieve the value of the <see cref="P:System.Windows.Controls.TextBlock.FontFamily" /> attached property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Controls.TextBlock.FontFamily" /> attached property on the specified dependency object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x0600570A RID: 22282 RVA: 0x0018138D File Offset: 0x0017F58D
		public static FontFamily GetFontFamily(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontFamily)element.GetValue(TextBlock.FontFamilyProperty);
		}

		/// <summary>Gets or sets the top-level font style for the <see cref="T:System.Windows.Controls.TextBlock" />.  </summary>
		/// <returns>A member of the <see cref="T:System.Windows.FontStyles" /> class specifying the desired font style. The default is determined by the <see cref="P:System.Windows.SystemFonts.MessageFontStyle" /> value.</returns>
		// Token: 0x17001534 RID: 5428
		// (get) Token: 0x0600570B RID: 22283 RVA: 0x001813AD File Offset: 0x0017F5AD
		// (set) Token: 0x0600570C RID: 22284 RVA: 0x001813BF File Offset: 0x0017F5BF
		public FontStyle FontStyle
		{
			get
			{
				return (FontStyle)base.GetValue(TextBlock.FontStyleProperty);
			}
			set
			{
				base.SetValue(TextBlock.FontStyleProperty, value);
			}
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.TextBlock.FontStyle" /> attached property on a specified dependency object.</summary>
		/// <param name="element">The dependency object on which to set the value of the <see cref="P:System.Windows.Controls.TextBlock.FontStyle" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x0600570D RID: 22285 RVA: 0x001813D2 File Offset: 0x0017F5D2
		public static void SetFontStyle(DependencyObject element, FontStyle value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextBlock.FontStyleProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Controls.TextBlock.FontStyle" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object from which to retrieve the value of the <see cref="P:System.Windows.Controls.TextBlock.FontStyle" /> attached property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Controls.TextBlock.FontStyle" /> attached property on the specified dependency object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x0600570E RID: 22286 RVA: 0x001813F3 File Offset: 0x0017F5F3
		public static FontStyle GetFontStyle(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontStyle)element.GetValue(TextBlock.FontStyleProperty);
		}

		/// <summary>Gets or sets the top-level font weight for the <see cref="T:System.Windows.Controls.TextBlock" />.  </summary>
		/// <returns>A member of the <see cref="T:System.Windows.FontWeights" /> class specifying the desired font weight. The default is determined by the <see cref="P:System.Windows.SystemFonts.MessageFontWeight" /> value.</returns>
		// Token: 0x17001535 RID: 5429
		// (get) Token: 0x0600570F RID: 22287 RVA: 0x00181413 File Offset: 0x0017F613
		// (set) Token: 0x06005710 RID: 22288 RVA: 0x00181425 File Offset: 0x0017F625
		public FontWeight FontWeight
		{
			get
			{
				return (FontWeight)base.GetValue(TextBlock.FontWeightProperty);
			}
			set
			{
				base.SetValue(TextBlock.FontWeightProperty, value);
			}
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.TextBlock.FontWeight" /> attached property on a specified dependency object.</summary>
		/// <param name="element">The dependency object on which to set the value of the <see cref="P:System.Windows.Controls.TextBlock.FontWeight" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06005711 RID: 22289 RVA: 0x00181438 File Offset: 0x0017F638
		public static void SetFontWeight(DependencyObject element, FontWeight value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextBlock.FontWeightProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Controls.TextBlock.FontWeight" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object from which to retrieve the value of the <see cref="P:System.Windows.Controls.TextBlock.FontWeight" /> attached property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Controls.TextBlock.FontWeight" /> attached property on the specified dependency object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06005712 RID: 22290 RVA: 0x00181459 File Offset: 0x0017F659
		public static FontWeight GetFontWeight(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontWeight)element.GetValue(TextBlock.FontWeightProperty);
		}

		/// <summary>Gets or sets the top-level font-stretching characteristics for the <see cref="T:System.Windows.Controls.TextBlock" />.  </summary>
		/// <returns>A member of the <see cref="T:System.Windows.FontStretch" /> class specifying the desired font-stretching characteristics to use. The default is <see cref="P:System.Windows.FontStretches.Normal" />.</returns>
		// Token: 0x17001536 RID: 5430
		// (get) Token: 0x06005713 RID: 22291 RVA: 0x00181479 File Offset: 0x0017F679
		// (set) Token: 0x06005714 RID: 22292 RVA: 0x0018148B File Offset: 0x0017F68B
		public FontStretch FontStretch
		{
			get
			{
				return (FontStretch)base.GetValue(TextBlock.FontStretchProperty);
			}
			set
			{
				base.SetValue(TextBlock.FontStretchProperty, value);
			}
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.TextBlock.FontStretch" /> attached property on a specified dependency object.</summary>
		/// <param name="element">The dependency object on which to set the value of the <see cref="P:System.Windows.Controls.TextBlock.FontStretch" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06005715 RID: 22293 RVA: 0x0018149E File Offset: 0x0017F69E
		public static void SetFontStretch(DependencyObject element, FontStretch value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextBlock.FontStretchProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Controls.TextBlock.FontStretch" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object from which to retrieve the value of the <see cref="P:System.Windows.Controls.TextBlock.FontStretch" /> attached property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Controls.TextBlock.FontStretch" /> attached property on the specified dependency object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06005716 RID: 22294 RVA: 0x001814BF File Offset: 0x0017F6BF
		public static FontStretch GetFontStretch(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontStretch)element.GetValue(TextBlock.FontStretchProperty);
		}

		/// <summary>Gets or sets the top-level font size for the <see cref="T:System.Windows.Controls.TextBlock" />.   </summary>
		/// <returns>The desired font size to use in device independent pixels). The default is determined by the <see cref="P:System.Windows.SystemFonts.MessageFontSize" /> value.</returns>
		// Token: 0x17001537 RID: 5431
		// (get) Token: 0x06005717 RID: 22295 RVA: 0x001814DF File Offset: 0x0017F6DF
		// (set) Token: 0x06005718 RID: 22296 RVA: 0x001814F1 File Offset: 0x0017F6F1
		[TypeConverter(typeof(FontSizeConverter))]
		[Localizability(LocalizationCategory.None)]
		public double FontSize
		{
			get
			{
				return (double)base.GetValue(TextBlock.FontSizeProperty);
			}
			set
			{
				base.SetValue(TextBlock.FontSizeProperty, value);
			}
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.TextBlock.FontSize" /> attached property on a specified dependency object.</summary>
		/// <param name="element">The dependency object on which to set the value of the <see cref="P:System.Windows.Controls.TextBlock.FontSize" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06005719 RID: 22297 RVA: 0x00181504 File Offset: 0x0017F704
		public static void SetFontSize(DependencyObject element, double value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextBlock.FontSizeProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Controls.TextBlock.FontSize" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object from which to retrieve the value of the <see cref="P:System.Windows.Controls.TextBlock.FontSize" /> attached property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Controls.TextBlock.FontSize" /> attached property on the specified dependency object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x0600571A RID: 22298 RVA: 0x00181525 File Offset: 0x0017F725
		[TypeConverter(typeof(FontSizeConverter))]
		public static double GetFontSize(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(TextBlock.FontSizeProperty);
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Media.Brush" /> to apply to the text contents of the <see cref="T:System.Windows.Controls.TextBlock" />.  </summary>
		/// <returns>The brush used to apply to the text contents. The default is <see cref="P:System.Windows.Media.Brushes.Black" />.</returns>
		// Token: 0x17001538 RID: 5432
		// (get) Token: 0x0600571B RID: 22299 RVA: 0x00181545 File Offset: 0x0017F745
		// (set) Token: 0x0600571C RID: 22300 RVA: 0x00181557 File Offset: 0x0017F757
		public Brush Foreground
		{
			get
			{
				return (Brush)base.GetValue(TextBlock.ForegroundProperty);
			}
			set
			{
				base.SetValue(TextBlock.ForegroundProperty, value);
			}
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.TextBlock.Foreground" /> attached property on a specified dependency object.</summary>
		/// <param name="element">The dependency object on which to set the value of the <see cref="P:System.Windows.Controls.TextBlock.Foreground" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x0600571D RID: 22301 RVA: 0x00181565 File Offset: 0x0017F765
		public static void SetForeground(DependencyObject element, Brush value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextBlock.ForegroundProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Controls.TextBlock.Foreground" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object from which to retrieve the value of the <see cref="P:System.Windows.Controls.TextBlock.Foreground" /> attached property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Controls.TextBlock.Foreground" /> attached property on the specified dependency object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x0600571E RID: 22302 RVA: 0x00181581 File Offset: 0x0017F781
		public static Brush GetForeground(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (Brush)element.GetValue(TextBlock.ForegroundProperty);
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Media.Brush" /> used to fill the background of content area.  </summary>
		/// <returns>The brush used to fill the background of the content area, or <see langword="null" /> to not use a background brush. The default is <see langword="null" />.</returns>
		// Token: 0x17001539 RID: 5433
		// (get) Token: 0x0600571F RID: 22303 RVA: 0x001815A1 File Offset: 0x0017F7A1
		// (set) Token: 0x06005720 RID: 22304 RVA: 0x001815B3 File Offset: 0x0017F7B3
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(TextBlock.BackgroundProperty);
			}
			set
			{
				base.SetValue(TextBlock.BackgroundProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.TextDecorationCollection" /> that contains the effects to apply to the text of a <see cref="T:System.Windows.Controls.TextBlock" />.  </summary>
		/// <returns>A <see cref="T:System.Windows.TextDecorationCollection" /> collection that contains text decorations to apply to this element. The default is <see langword="null" /> (no text decorations applied).</returns>
		// Token: 0x1700153A RID: 5434
		// (get) Token: 0x06005721 RID: 22305 RVA: 0x001815C1 File Offset: 0x0017F7C1
		// (set) Token: 0x06005722 RID: 22306 RVA: 0x001815D3 File Offset: 0x0017F7D3
		public TextDecorationCollection TextDecorations
		{
			get
			{
				return (TextDecorationCollection)base.GetValue(TextBlock.TextDecorationsProperty);
			}
			set
			{
				base.SetValue(TextBlock.TextDecorationsProperty, value);
			}
		}

		/// <summary>Gets or sets the effects to apply to the text content in this element.  </summary>
		/// <returns>A <see cref="T:System.Windows.Media.TextEffectCollection" /> containing one or more <see cref="T:System.Windows.Media.TextEffect" /> objects that define effects to apply to the text of the <see cref="T:System.Windows.Controls.TextBlock" />. The default is <see langword="null" /> (no effects applied).</returns>
		// Token: 0x1700153B RID: 5435
		// (get) Token: 0x06005723 RID: 22307 RVA: 0x001815E1 File Offset: 0x0017F7E1
		// (set) Token: 0x06005724 RID: 22308 RVA: 0x001815F3 File Offset: 0x0017F7F3
		public TextEffectCollection TextEffects
		{
			get
			{
				return (TextEffectCollection)base.GetValue(TextBlock.TextEffectsProperty);
			}
			set
			{
				base.SetValue(TextBlock.TextEffectsProperty, value);
			}
		}

		/// <summary>Gets or sets the height of each line of content.  </summary>
		/// <returns>The height of line, in device independent pixels, in the range of 0.0034 to 160000. A value of <see cref="F:System.Double.NaN" /> (equivalent to an attribute value of "Auto") indicates that the line height is determined automatically from the current font characteristics. The default is <see cref="F:System.Double.NaN" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <see cref="P:System.Windows.Controls.TextBlock.LineHeight" /> is set to a non-positive value.</exception>
		// Token: 0x1700153C RID: 5436
		// (get) Token: 0x06005725 RID: 22309 RVA: 0x00181601 File Offset: 0x0017F801
		// (set) Token: 0x06005726 RID: 22310 RVA: 0x00181613 File Offset: 0x0017F813
		[TypeConverter(typeof(LengthConverter))]
		public double LineHeight
		{
			get
			{
				return (double)base.GetValue(TextBlock.LineHeightProperty);
			}
			set
			{
				base.SetValue(TextBlock.LineHeightProperty, value);
			}
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.TextBlock.LineHeight" /> attached property on a specified dependency object.</summary>
		/// <param name="element">The dependency object on which to set the value of the <see cref="P:System.Windows.Controls.TextBlock.LineHeight" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <see cref="P:System.Windows.Controls.TextBlock.LineHeight" />is set to a non-positive value.</exception>
		// Token: 0x06005727 RID: 22311 RVA: 0x00181626 File Offset: 0x0017F826
		public static void SetLineHeight(DependencyObject element, double value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextBlock.LineHeightProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Controls.TextBlock.LineHeight" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object from which to retrieve the value of the <see cref="P:System.Windows.Controls.TextBlock.LineHeight" /> attached property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Controls.TextBlock.LineHeight" /> attached property on the specified dependency object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06005728 RID: 22312 RVA: 0x00181647 File Offset: 0x0017F847
		[TypeConverter(typeof(LengthConverter))]
		public static double GetLineHeight(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(TextBlock.LineHeightProperty);
		}

		/// <summary>Gets or sets the mechanism by which a line box is determined for each line of text within the <see cref="T:System.Windows.Controls.TextBlock" />.  </summary>
		/// <returns>The mechanism by which a line box is determined for each line of text within the <see cref="T:System.Windows.Controls.TextBlock" />. The default is <see cref="F:System.Windows.LineStackingStrategy.MaxHeight" />.</returns>
		// Token: 0x1700153D RID: 5437
		// (get) Token: 0x06005729 RID: 22313 RVA: 0x00181667 File Offset: 0x0017F867
		// (set) Token: 0x0600572A RID: 22314 RVA: 0x00181679 File Offset: 0x0017F879
		public LineStackingStrategy LineStackingStrategy
		{
			get
			{
				return (LineStackingStrategy)base.GetValue(TextBlock.LineStackingStrategyProperty);
			}
			set
			{
				base.SetValue(TextBlock.LineStackingStrategyProperty, value);
			}
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.TextBlock.LineStackingStrategy" /> attached property on a specified dependency object.</summary>
		/// <param name="element">The dependency object on which to set the value of the <see cref="P:System.Windows.Controls.TextBlock.LineStackingStrategy" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x0600572B RID: 22315 RVA: 0x0018168C File Offset: 0x0017F88C
		public static void SetLineStackingStrategy(DependencyObject element, LineStackingStrategy value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextBlock.LineStackingStrategyProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Controls.TextBlock.LineStackingStrategy" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object from which to retrieve the value of the <see cref="P:System.Windows.Controls.TextBlock.LineStackingStrategy" /> attached property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Controls.TextBlock.LineStackingStrategy" /> attached property on the specified dependency object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x0600572C RID: 22316 RVA: 0x001816AD File Offset: 0x0017F8AD
		public static LineStackingStrategy GetLineStackingStrategy(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (LineStackingStrategy)element.GetValue(TextBlock.LineStackingStrategyProperty);
		}

		/// <summary>Gets or sets a value that indicates the thickness of padding space between the boundaries of the content area, and the content displayed by a <see cref="T:System.Windows.Controls.TextBlock" />.  </summary>
		/// <returns>A <see cref="T:System.Windows.Thickness" /> structure specifying the amount of padding to apply, in device independent pixels. The default is <see cref="F:System.Double.NaN" />.</returns>
		// Token: 0x1700153E RID: 5438
		// (get) Token: 0x0600572D RID: 22317 RVA: 0x001816CD File Offset: 0x0017F8CD
		// (set) Token: 0x0600572E RID: 22318 RVA: 0x001816DF File Offset: 0x0017F8DF
		public Thickness Padding
		{
			get
			{
				return (Thickness)base.GetValue(TextBlock.PaddingProperty);
			}
			set
			{
				base.SetValue(TextBlock.PaddingProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates the horizontal alignment of text content.  </summary>
		/// <returns>One of the <see cref="T:System.Windows.TextAlignment" /> values that specifies the desired alignment. The default is <see cref="F:System.Windows.TextAlignment.Left" />.</returns>
		// Token: 0x1700153F RID: 5439
		// (get) Token: 0x0600572F RID: 22319 RVA: 0x001816F2 File Offset: 0x0017F8F2
		// (set) Token: 0x06005730 RID: 22320 RVA: 0x00181704 File Offset: 0x0017F904
		public TextAlignment TextAlignment
		{
			get
			{
				return (TextAlignment)base.GetValue(TextBlock.TextAlignmentProperty);
			}
			set
			{
				base.SetValue(TextBlock.TextAlignmentProperty, value);
			}
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.TextBlock.TextAlignment" /> attached property on a specified dependency object.</summary>
		/// <param name="element">The dependency object on which to set the value of the <see cref="P:System.Windows.Controls.TextBlock.TextAlignment" /> property.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06005731 RID: 22321 RVA: 0x00181717 File Offset: 0x0017F917
		public static void SetTextAlignment(DependencyObject element, TextAlignment value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextBlock.TextAlignmentProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Controls.TextBlock.TextAlignment" /> attached property for a specified dependency object.</summary>
		/// <param name="element">The dependency object from which to retrieve the value of the <see cref="P:System.Windows.Controls.TextBlock.TextAlignment" /> attached property.</param>
		/// <returns>The current value of the <see cref="P:System.Windows.Controls.TextBlock.TextAlignment" /> attached property on the specified dependency object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06005732 RID: 22322 RVA: 0x00181738 File Offset: 0x0017F938
		public static TextAlignment GetTextAlignment(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (TextAlignment)element.GetValue(TextBlock.TextAlignmentProperty);
		}

		/// <summary>Gets or sets the text trimming behavior to employ when content overflows the content area.  </summary>
		/// <returns>One of the <see cref="T:System.Windows.TextTrimming" /> values that specifies the text trimming behavior to employ. The default is <see cref="F:System.Windows.TextTrimming.None" />.</returns>
		// Token: 0x17001540 RID: 5440
		// (get) Token: 0x06005733 RID: 22323 RVA: 0x00181758 File Offset: 0x0017F958
		// (set) Token: 0x06005734 RID: 22324 RVA: 0x0018176A File Offset: 0x0017F96A
		public TextTrimming TextTrimming
		{
			get
			{
				return (TextTrimming)base.GetValue(TextBlock.TextTrimmingProperty);
			}
			set
			{
				base.SetValue(TextBlock.TextTrimmingProperty, value);
			}
		}

		/// <summary>Gets or sets how the <see cref="T:System.Windows.Controls.TextBlock" /> should wrap text.  </summary>
		/// <returns>One of the <see cref="T:System.Windows.TextWrapping" /> values. The default is <see cref="F:System.Windows.TextWrapping.NoWrap" />.</returns>
		// Token: 0x17001541 RID: 5441
		// (get) Token: 0x06005735 RID: 22325 RVA: 0x0018177D File Offset: 0x0017F97D
		// (set) Token: 0x06005736 RID: 22326 RVA: 0x0018178F File Offset: 0x0017F98F
		public TextWrapping TextWrapping
		{
			get
			{
				return (TextWrapping)base.GetValue(TextBlock.TextWrappingProperty);
			}
			set
			{
				base.SetValue(TextBlock.TextWrappingProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether automatic hyphenation of words is enabled or disabled.  </summary>
		/// <returns>
		///     <see langword="true" /> to indicate that automatic breaking and hyphenation of words is enabled; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001542 RID: 5442
		// (get) Token: 0x06005737 RID: 22327 RVA: 0x001817A2 File Offset: 0x0017F9A2
		// (set) Token: 0x06005738 RID: 22328 RVA: 0x001817B4 File Offset: 0x0017F9B4
		public bool IsHyphenationEnabled
		{
			get
			{
				return (bool)base.GetValue(TextBlock.IsHyphenationEnabledProperty);
			}
			set
			{
				base.SetValue(TextBlock.IsHyphenationEnabledProperty, value);
			}
		}

		/// <summary>Gets the number of <see cref="T:System.Windows.Media.Visual" /> children hosted by the <see cref="T:System.Windows.Controls.TextBlock" />.</summary>
		/// <returns>The number of visual children hosted by the <see cref="T:System.Windows.Controls.TextBlock" />.</returns>
		// Token: 0x17001543 RID: 5443
		// (get) Token: 0x06005739 RID: 22329 RVA: 0x001817C2 File Offset: 0x0017F9C2
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._complexContent != null)
				{
					return this._complexContent.VisualChildren.Count;
				}
				return 0;
			}
		}

		/// <summary>Returns the <see cref="T:System.Windows.Media.Visual" /> child at a specified index.</summary>
		/// <param name="index">A zero-based index specifying the <see cref="T:System.Windows.Media.Visual" /> child to return.  This value must be between 0 and (<see cref="P:System.Windows.Controls.TextBlock.VisualChildrenCount" /> minus 1)</param>
		/// <returns>The <see cref="T:System.Windows.Media.Visual" /> child at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is not between 0 and (<see cref="P:System.Windows.Controls.TextBlock.VisualChildrenCount" /> minus 1)</exception>
		// Token: 0x0600573A RID: 22330 RVA: 0x001817DE File Offset: 0x0017F9DE
		protected override Visual GetVisualChild(int index)
		{
			if (this._complexContent == null)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return this._complexContent.VisualChildren[index];
		}

		/// <summary>Called to re-measure the <see cref="T:System.Windows.Controls.TextBlock" />.</summary>
		/// <param name="constraint">A <see cref="T:System.Windows.Size" /> structure specifying any constraints on the size of the <see cref="T:System.Windows.Controls.TextBlock" />.</param>
		/// <returns>A <see cref="T:System.Windows.Size" /> structure indicating the new size of the <see cref="T:System.Windows.Controls.TextBlock" />.</returns>
		// Token: 0x0600573B RID: 22331 RVA: 0x00181804 File Offset: 0x0017FA04
		protected sealed override Size MeasureOverride(Size constraint)
		{
			this.VerifyReentrancy();
			this._textBlockCache = null;
			this.EnsureTextBlockCache();
			LineProperties lineProperties = this._textBlockCache._lineProperties;
			if (this.CheckFlags(TextBlock.Flags.PendingTextContainerEventInit))
			{
				Invariant.Assert(this._complexContent != null);
				this.InitializeTextContainerListeners();
				this.SetFlags(false, TextBlock.Flags.PendingTextContainerEventInit);
			}
			int lineCount = this.LineCount;
			if (lineCount > 0 && base.IsMeasureValid && this.InlineObjects == null)
			{
				bool flag;
				if (lineProperties.TextTrimming == TextTrimming.None)
				{
					flag = (DoubleUtil.AreClose(constraint.Width, this._referenceSize.Width) || lineProperties.TextWrapping == TextWrapping.NoWrap);
				}
				else
				{
					flag = (DoubleUtil.AreClose(constraint.Width, this._referenceSize.Width) && lineProperties.TextWrapping == TextWrapping.NoWrap && (DoubleUtil.AreClose(constraint.Height, this._referenceSize.Height) || lineCount == 1));
				}
				if (flag)
				{
					this._referenceSize = constraint;
					return this._previousDesiredSize;
				}
			}
			this._referenceSize = constraint;
			bool flag2 = this.CheckFlags(TextBlock.Flags.FormattedOnce);
			double baselineOffset = this._baselineOffset;
			this.InlineObjects = null;
			int capacity = (this._subsequentLines == null) ? 1 : this._subsequentLines.Count;
			this.ClearLineMetrics();
			if (this._complexContent != null)
			{
				this._complexContent.TextView.Invalidate();
			}
			lineProperties.IgnoreTextAlignment = true;
			this.SetFlags(true, TextBlock.Flags.RequiresAlignment);
			this.SetFlags(true, TextBlock.Flags.FormattedOnce);
			this.SetFlags(false, TextBlock.Flags.HasParagraphEllipses);
			this.SetFlags(true, TextBlock.Flags.MeasureInProgress | TextBlock.Flags.TreeInReadOnlyMode);
			Size size = default(Size);
			bool flag3 = true;
			try
			{
				Line line = this.CreateLine(lineProperties);
				bool flag4 = false;
				int num = 0;
				TextLineBreak textLineBreak = null;
				Thickness padding = this.Padding;
				Size size2 = new Size(Math.Max(0.0, constraint.Width - (padding.Left + padding.Right)), Math.Max(0.0, constraint.Height - (padding.Top + padding.Bottom)));
				TextDpi.EnsureValidLineWidth(ref size2);
				while (!flag4)
				{
					using (line)
					{
						line.Format(num, size2.Width, this.GetLineProperties(num == 0, lineProperties), textLineBreak, this._textBlockCache._textRunCache, false);
						double num2 = this.CalcLineAdvance(line.Height, lineProperties);
						LineMetrics lineMetrics = new LineMetrics(line.Length, line.Width, num2, line.BaselineOffset, line.HasInlineObjects(), textLineBreak);
						if (!this.CheckFlags(TextBlock.Flags.HasFirstLine))
						{
							this.SetFlags(true, TextBlock.Flags.HasFirstLine);
							this._firstLine = lineMetrics;
						}
						else
						{
							if (this._subsequentLines == null)
							{
								this._subsequentLines = new List<LineMetrics>(capacity);
							}
							this._subsequentLines.Add(lineMetrics);
						}
						size.Width = Math.Max(size.Width, line.GetCollapsedWidth());
						if (lineProperties.TextTrimming == TextTrimming.None || size2.Height >= size.Height + num2 || num == 0)
						{
							this._baselineOffset = size.Height + line.BaselineOffset;
							size.Height += num2;
						}
						else
						{
							this.SetFlags(true, TextBlock.Flags.HasParagraphEllipses);
						}
						textLineBreak = line.GetTextLineBreak();
						flag4 = line.EndOfParagraph;
						num += line.Length;
						if (!flag4 && lineProperties.TextWrapping == TextWrapping.NoWrap && line.Length == 9600)
						{
							flag4 = true;
						}
					}
				}
				size.Width += padding.Left + padding.Right;
				size.Height += padding.Top + padding.Bottom;
				Invariant.Assert(textLineBreak == null);
				flag3 = false;
			}
			finally
			{
				lineProperties.IgnoreTextAlignment = false;
				this.SetFlags(false, TextBlock.Flags.MeasureInProgress | TextBlock.Flags.TreeInReadOnlyMode);
				if (flag3)
				{
					this._textBlockCache._textRunCache = null;
					this.ClearLineMetrics();
				}
			}
			if (!DoubleUtil.AreClose(baselineOffset, this._baselineOffset))
			{
				base.CoerceValue(TextBlock.BaselineOffsetProperty);
			}
			this._previousDesiredSize = size;
			return size;
		}

		/// <summary>Positions child elements and determines a size for the <see cref="T:System.Windows.Controls.TextBlock" />.</summary>
		/// <param name="arrangeSize">A <see cref="T:System.Windows.Size" /> within the hosting parent element that the <see cref="T:System.Windows.Controls.TextBlock" /> should use to arrange itself and its child elements. Sizing constraints may affect this requested size.</param>
		/// <returns>The actual <see cref="T:System.Windows.Size" /> used to arrange the element.</returns>
		// Token: 0x0600573C RID: 22332 RVA: 0x00181C34 File Offset: 0x0017FE34
		protected sealed override Size ArrangeOverride(Size arrangeSize)
		{
			this.VerifyReentrancy();
			if (this._complexContent != null)
			{
				this._complexContent.VisualChildren.Clear();
			}
			ArrayList inlineObjects = this.InlineObjects;
			int lineCount = this.LineCount;
			if (inlineObjects != null && lineCount > 0)
			{
				bool flag = true;
				this.SetFlags(true, TextBlock.Flags.TreeInReadOnlyMode);
				this.SetFlags(true, TextBlock.Flags.ArrangeInProgress);
				try
				{
					this.EnsureTextBlockCache();
					LineProperties lineProperties = this._textBlockCache._lineProperties;
					double wrappingWidth = this.CalcWrappingWidth(arrangeSize.Width);
					Vector vector = this.CalcContentOffset(arrangeSize, wrappingWidth);
					Line line = this.CreateLine(lineProperties);
					int num = 0;
					Vector lineOffset = vector;
					for (int i = 0; i < lineCount; i++)
					{
						LineMetrics line2 = this.GetLine(i);
						if (line2.HasInlineObjects)
						{
							using (line)
							{
								bool ellipsis = this.ParagraphEllipsisShownOnLine(i, lineOffset.Y - vector.Y);
								this.Format(line, line2.Length, num, wrappingWidth, this.GetLineProperties(num == 0, lineProperties), line2.TextLineBreak, this._textBlockCache._textRunCache, ellipsis);
								line.Arrange(this._complexContent.VisualChildren, lineOffset);
							}
						}
						lineOffset.Y += line2.Height;
						num += line2.Length;
					}
					flag = false;
				}
				finally
				{
					this.SetFlags(false, TextBlock.Flags.TreeInReadOnlyMode);
					this.SetFlags(false, TextBlock.Flags.ArrangeInProgress);
					if (flag)
					{
						this._textBlockCache._textRunCache = null;
						this.ClearLineMetrics();
					}
				}
			}
			if (this._complexContent != null)
			{
				base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.OnValidateTextView), EventArgs.Empty);
			}
			base.InvalidateVisual();
			return arrangeSize;
		}

		/// <summary>Renders the contents of a <see cref="T:System.Windows.Controls.TextBlock" />.</summary>
		/// <param name="ctx">The <see cref="T:System.Windows.Media.DrawingContext" /> to render the control on.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="ctx" /> is <see langword="null" />.</exception>
		// Token: 0x0600573D RID: 22333 RVA: 0x00181E00 File Offset: 0x00180000
		protected sealed override void OnRender(DrawingContext ctx)
		{
			this.VerifyReentrancy();
			if (ctx == null)
			{
				throw new ArgumentNullException("ctx");
			}
			if (!this.IsLayoutDataValid)
			{
				return;
			}
			Brush background = this.Background;
			if (background != null)
			{
				ctx.DrawRectangle(background, null, new Rect(0.0, 0.0, base.RenderSize.Width, base.RenderSize.Height));
			}
			this.SetFlags(false, TextBlock.Flags.RequiresAlignment);
			this.SetFlags(true, TextBlock.Flags.TreeInReadOnlyMode);
			try
			{
				this.EnsureTextBlockCache();
				LineProperties lineProperties = this._textBlockCache._lineProperties;
				double wrappingWidth = this.CalcWrappingWidth(base.RenderSize.Width);
				Vector vector = this.CalcContentOffset(base.RenderSize, wrappingWidth);
				Point lineOffset = new Point(vector.X, vector.Y);
				Line line = this.CreateLine(lineProperties);
				int num = 0;
				bool flag = false;
				this.SetFlags(this.CheckFlags(TextBlock.Flags.HasParagraphEllipses), TextBlock.Flags.RequiresAlignment);
				int lineCount = this.LineCount;
				for (int i = 0; i < lineCount; i++)
				{
					LineMetrics metrics = this.GetLine(i);
					double value = Math.Max(0.0, base.RenderSize.Height - this.Padding.Bottom);
					if (this.CheckFlags(TextBlock.Flags.HasParagraphEllipses) && i + 1 < lineCount)
					{
						double value2 = this.GetLine(i + 1).Height + metrics.Height + lineOffset.Y;
						flag = (DoubleUtil.GreaterThan(value2, value) && !DoubleUtil.AreClose(value2, value));
					}
					if (!this.CheckFlags(TextBlock.Flags.HasParagraphEllipses) || DoubleUtil.LessThanOrClose(metrics.Height + lineOffset.Y, value) || i == 0)
					{
						using (line)
						{
							this.Format(line, metrics.Length, num, wrappingWidth, this.GetLineProperties(num == 0, flag, lineProperties), metrics.TextLineBreak, this._textBlockCache._textRunCache, flag);
							if (!this.CheckFlags(TextBlock.Flags.HasParagraphEllipses))
							{
								metrics = this.UpdateLine(i, metrics, line.Start, line.Width);
							}
							line.Render(ctx, lineOffset);
							lineOffset.Y += metrics.Height;
							num += metrics.Length;
						}
					}
				}
			}
			finally
			{
				this.SetFlags(false, TextBlock.Flags.TreeInReadOnlyMode);
				this._textBlockCache = null;
			}
		}

		/// <summary>Called when the value one or more hosted dependency properties changes.</summary>
		/// <param name="e">Arguments for the associated event.</param>
		// Token: 0x0600573E RID: 22334 RVA: 0x00182098 File Offset: 0x00180298
		protected sealed override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if ((e.IsAValueChange || e.IsASubPropertyChange) && this.CheckFlags(TextBlock.Flags.FormattedOnce))
			{
				FrameworkPropertyMetadata frameworkPropertyMetadata = e.Metadata as FrameworkPropertyMetadata;
				if (frameworkPropertyMetadata != null)
				{
					bool flag = frameworkPropertyMetadata.AffectsRender && (e.IsAValueChange || !frameworkPropertyMetadata.SubPropertiesDoNotAffectRender);
					if (frameworkPropertyMetadata.AffectsMeasure || frameworkPropertyMetadata.AffectsArrange || flag)
					{
						this.VerifyTreeIsUnlocked();
						this._textBlockCache = null;
					}
				}
			}
		}

		/// <summary>Returns a <see cref="T:System.Windows.Media.PointHitTestResult" /> for specified <see cref="T:System.Windows.Media.PointHitTestParameters" />.</summary>
		/// <param name="hitTestParameters">A <see cref="T:System.Windows.Media.PointHitTestParameters" /> object specifying the parameters to hit test for.</param>
		/// <returns>A <see cref="T:System.Windows.Media.PointHitTestResult" /> for the specified hit test parameters.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="hitTestParameters" /> is <see langword="null" />.</exception>
		// Token: 0x0600573F RID: 22335 RVA: 0x0018211C File Offset: 0x0018031C
		protected sealed override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
		{
			this.VerifyReentrancy();
			if (hitTestParameters == null)
			{
				throw new ArgumentNullException("hitTestParameters");
			}
			Rect rect = new Rect(default(Point), base.RenderSize);
			if (rect.Contains(hitTestParameters.HitPoint))
			{
				return new PointHitTestResult(this, hitTestParameters.HitPoint);
			}
			return null;
		}

		/// <summary>Returns the <see cref="T:System.Windows.IInputElement" /> at a specified <see cref="T:System.Windows.Point" /> within the <see cref="T:System.Windows.Controls.TextBlock" />.</summary>
		/// <param name="point">A <see cref="T:System.Windows.Point" />, in the coordinate space of the <see cref="T:System.Windows.Controls.TextBlock" />, for which to return the corresponding <see cref="T:System.Windows.IInputElement" />.</param>
		/// <returns>The <see cref="T:System.Windows.IInputElement" /> found at the specified Point, or <see langword="null" /> if no such <see cref="T:System.Windows.IInputElement" /> can be found.</returns>
		// Token: 0x06005740 RID: 22336 RVA: 0x00182170 File Offset: 0x00180370
		protected virtual IInputElement InputHitTestCore(Point point)
		{
			if (!this.IsLayoutDataValid)
			{
				return this;
			}
			LineProperties lineProperties = this.GetLineProperties();
			IInputElement inputElement = null;
			double wrappingWidth = this.CalcWrappingWidth(base.RenderSize.Width);
			Vector vector = this.CalcContentOffset(base.RenderSize, wrappingWidth);
			point -= vector;
			if (point.X < 0.0 || point.Y < 0.0)
			{
				return this;
			}
			inputElement = null;
			int num = 0;
			double num2 = 0.0;
			TextRunCache textRunCache = new TextRunCache();
			int lineCount = this.LineCount;
			for (int i = 0; i < lineCount; i++)
			{
				LineMetrics line = this.GetLine(i);
				if (num2 + line.Height > point.Y)
				{
					Line line2 = this.CreateLine(lineProperties);
					using (line2)
					{
						bool ellipsis = this.ParagraphEllipsisShownOnLine(i, num2);
						this.Format(line2, line.Length, num, wrappingWidth, this.GetLineProperties(num == 0, lineProperties), line.TextLineBreak, textRunCache, ellipsis);
						if (line2.Start <= point.X && line2.Start + line2.Width >= point.X)
						{
							inputElement = line2.InputHitTest(point.X);
						}
						break;
					}
				}
				num += line.Length;
				num2 += line.Height;
			}
			if (inputElement == null)
			{
				return this;
			}
			return inputElement;
		}

		/// <summary>Returns a read-only collection of bounding rectangles for a specified <see cref="T:System.Windows.ContentElement" />.</summary>
		/// <param name="child">A <see cref="T:System.Windows.ContentElement" /> for which to generate and return a collection of bounding rectangles.</param>
		/// <returns>A read-only collection of bounding rectangles for the specified <see cref="T:System.Windows.ContentElement" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="child" /> is <see langword="null" />.</exception>
		// Token: 0x06005741 RID: 22337 RVA: 0x001822F4 File Offset: 0x001804F4
		protected virtual ReadOnlyCollection<Rect> GetRectanglesCore(ContentElement child)
		{
			if (child == null)
			{
				throw new ArgumentNullException("child");
			}
			if (!this.IsLayoutDataValid)
			{
				return new ReadOnlyCollection<Rect>(new List<Rect>(0));
			}
			LineProperties lineProperties = this.GetLineProperties();
			if (this._complexContent == null || !(this._complexContent.TextContainer is TextContainer))
			{
				return new ReadOnlyCollection<Rect>(new List<Rect>(0));
			}
			TextPointer textPointer = this.FindElementPosition(child);
			if (textPointer == null)
			{
				return new ReadOnlyCollection<Rect>(new List<Rect>(0));
			}
			TextPointer textPointer2 = null;
			if (child is TextElement)
			{
				textPointer2 = new TextPointer(((TextElement)child).ElementEnd);
			}
			else if (child is FrameworkContentElement)
			{
				textPointer2 = new TextPointer(textPointer);
				textPointer2.MoveByOffset(1);
			}
			if (textPointer2 == null)
			{
				return new ReadOnlyCollection<Rect>(new List<Rect>(0));
			}
			int offsetToPosition = this._complexContent.TextContainer.Start.GetOffsetToPosition(textPointer);
			int offsetToPosition2 = this._complexContent.TextContainer.Start.GetOffsetToPosition(textPointer2);
			int num = 0;
			int num2 = 0;
			double num3 = 0.0;
			int lineCount = this.LineCount;
			while (offsetToPosition >= num2 + this.GetLine(num).Length && num < lineCount)
			{
				num2 += this.GetLine(num).Length;
				num++;
				num3 += this.GetLine(num).Height;
			}
			int num4 = num2;
			List<Rect> list = new List<Rect>();
			double wrappingWidth = this.CalcWrappingWidth(base.RenderSize.Width);
			TextRunCache textRunCache = new TextRunCache();
			Vector vector = this.CalcContentOffset(base.RenderSize, wrappingWidth);
			do
			{
				LineMetrics line = this.GetLine(num);
				Line line2 = this.CreateLine(lineProperties);
				using (line2)
				{
					bool ellipsis = this.ParagraphEllipsisShownOnLine(num, (double)num2);
					this.Format(line2, line.Length, num4, wrappingWidth, this.GetLineProperties(num == 0, lineProperties), line.TextLineBreak, textRunCache, ellipsis);
					if (line.Length == line2.Length)
					{
						int num5 = (offsetToPosition >= num4) ? offsetToPosition : num4;
						int num6 = (offsetToPosition2 < num4 + line.Length) ? offsetToPosition2 : (num4 + line.Length);
						double x = vector.X;
						double yOffset = vector.Y + num3;
						List<Rect> rangeBounds = line2.GetRangeBounds(num5, num6 - num5, x, yOffset);
						list.AddRange(rangeBounds);
					}
				}
				num4 += line.Length;
				num3 += line.Height;
				num++;
			}
			while (offsetToPosition2 > num4);
			Invariant.Assert(list != null);
			return new ReadOnlyCollection<Rect>(list);
		}

		/// <summary>Gets an enumerator that can be used iterate the elements hosted by this <see cref="T:System.Windows.Controls.TextBlock" />.</summary>
		/// <returns>An enumerator that can iterate elements hosted by this <see cref="T:System.Windows.Controls.TextBlock" />.</returns>
		// Token: 0x17001544 RID: 5444
		// (get) Token: 0x06005742 RID: 22338 RVA: 0x0018258C File Offset: 0x0018078C
		protected virtual IEnumerator<IInputElement> HostedElementsCore
		{
			get
			{
				if (this.CheckFlags(TextBlock.Flags.ContentChangeInProgress))
				{
					throw new InvalidOperationException(SR.Get("TextContainerChangingReentrancyInvalid"));
				}
				if (this._complexContent == null || !(this._complexContent.TextContainer is TextContainer))
				{
					return new HostedElements(new ReadOnlyCollection<TextSegment>(new List<TextSegment>(0)));
				}
				List<TextSegment> list = new List<TextSegment>(1);
				TextSegment item = new TextSegment(this._complexContent.TextContainer.Start, this._complexContent.TextContainer.End);
				list.Insert(0, item);
				ReadOnlyCollection<TextSegment> textSegments = new ReadOnlyCollection<TextSegment>(list);
				return new HostedElements(textSegments);
			}
		}

		/// <summary>Called when a child element deriving from <see cref="T:System.Windows.UIElement" /> changes its <see cref="P:System.Windows.UIElement.DesiredSize" />.</summary>
		/// <param name="child">The child <see cref="T:System.Windows.UIElement" /> element whose <see cref="P:System.Windows.UIElement.DesiredSize" /> has changed.</param>
		// Token: 0x06005743 RID: 22339 RVA: 0x00182621 File Offset: 0x00180821
		protected virtual void OnChildDesiredSizeChangedCore(UIElement child)
		{
			this.OnChildDesiredSizeChanged(child);
		}

		/// <summary>Creates and returns an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> object for this <see cref="T:System.Windows.Controls.TextBlock" />.</summary>
		/// <returns>An <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> object for this <see cref="T:System.Windows.Controls.TextBlock" />.</returns>
		// Token: 0x06005744 RID: 22340 RVA: 0x0018262A File Offset: 0x0018082A
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new TextBlockAutomationPeer(this);
		}

		// Token: 0x06005745 RID: 22341 RVA: 0x00182632 File Offset: 0x00180832
		internal void RemoveChild(Visual child)
		{
			if (this._complexContent != null)
			{
				this._complexContent.VisualChildren.Remove(child);
			}
		}

		// Token: 0x06005746 RID: 22342 RVA: 0x00182650 File Offset: 0x00180850
		internal void SetTextContainer(ITextContainer textContainer)
		{
			if (this._complexContent != null)
			{
				this._complexContent.Detach(this);
				this._complexContent = null;
				this.SetFlags(false, TextBlock.Flags.PendingTextContainerEventInit);
			}
			if (textContainer != null)
			{
				this._complexContent = null;
				this.EnsureComplexContent(textContainer);
			}
			this.SetFlags(false, TextBlock.Flags.ContentChangeInProgress);
			base.InvalidateMeasure();
			base.InvalidateVisual();
		}

		// Token: 0x06005747 RID: 22343 RVA: 0x001826AC File Offset: 0x001808AC
		internal Size MeasureChild(InlineObject inlineObject)
		{
			Size desiredSize;
			if (this.CheckFlags(TextBlock.Flags.MeasureInProgress))
			{
				Thickness padding = this.Padding;
				Size availableSize = new Size(Math.Max(0.0, this._referenceSize.Width - (padding.Left + padding.Right)), Math.Max(0.0, this._referenceSize.Height - (padding.Top + padding.Bottom)));
				inlineObject.Element.Measure(availableSize);
				desiredSize = inlineObject.Element.DesiredSize;
				ArrayList arrayList = this.InlineObjects;
				bool flag = false;
				if (arrayList == null)
				{
					arrayList = (this.InlineObjects = new ArrayList(1));
				}
				else
				{
					for (int i = 0; i < arrayList.Count; i++)
					{
						if (((InlineObject)arrayList[i]).Dcp == inlineObject.Dcp)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					arrayList.Add(inlineObject);
				}
			}
			else
			{
				desiredSize = inlineObject.Element.DesiredSize;
			}
			return desiredSize;
		}

		// Token: 0x06005748 RID: 22344 RVA: 0x001827AC File Offset: 0x001809AC
		internal override string GetPlainText()
		{
			if (this._complexContent != null)
			{
				return TextRangeBase.GetTextInternal(this._complexContent.TextContainer.Start, this._complexContent.TextContainer.End);
			}
			if (this._contentCache != null)
			{
				return this._contentCache;
			}
			return string.Empty;
		}

		// Token: 0x06005749 RID: 22345 RVA: 0x001827FC File Offset: 0x001809FC
		internal ReadOnlyCollection<LineResult> GetLineResults()
		{
			Invariant.Assert(this.IsLayoutDataValid);
			if (this.CheckFlags(TextBlock.Flags.RequiresAlignment))
			{
				this.AlignContent();
			}
			double wrappingWidth = this.CalcWrappingWidth(base.RenderSize.Width);
			Vector vector = this.CalcContentOffset(base.RenderSize, wrappingWidth);
			int lineCount = this.LineCount;
			List<LineResult> list = new List<LineResult>(lineCount);
			int num = 0;
			double num2 = 0.0;
			for (int i = 0; i < lineCount; i++)
			{
				LineMetrics line = this.GetLine(i);
				Rect layoutBox = new Rect(vector.X + line.Start, vector.Y + num2, line.Width, line.Height);
				list.Add(new TextLineResult(this, num, line.Length, layoutBox, line.Baseline, i));
				num2 += line.Height;
				num += line.Length;
			}
			return new ReadOnlyCollection<LineResult>(list);
		}

		// Token: 0x0600574A RID: 22346 RVA: 0x001828EC File Offset: 0x00180AEC
		internal void GetLineDetails(int dcp, int index, double lineVOffset, out int cchContent, out int cchEllipses)
		{
			Invariant.Assert(this.IsLayoutDataValid);
			Invariant.Assert(index >= 0 && index < this.LineCount);
			LineProperties lineProperties = this.GetLineProperties();
			double wrappingWidth = this.CalcWrappingWidth(base.RenderSize.Width);
			TextRunCache textRunCache = new TextRunCache();
			LineMetrics line = this.GetLine(index);
			using (Line line2 = this.CreateLine(lineProperties))
			{
				TextLineBreak textLineBreak = this.GetLine(index).TextLineBreak;
				bool ellipsis = this.ParagraphEllipsisShownOnLine(index, lineVOffset);
				this.Format(line2, line.Length, dcp, wrappingWidth, this.GetLineProperties(dcp == 0, lineProperties), textLineBreak, textRunCache, ellipsis);
				Invariant.Assert(line.Length == line2.Length, "Line length is out of sync");
				cchContent = line2.ContentLength;
				cchEllipses = line2.GetEllipsesLength();
			}
		}

		// Token: 0x0600574B RID: 22347 RVA: 0x001829D8 File Offset: 0x00180BD8
		internal ITextPointer GetTextPositionFromDistance(int dcp, double distance, double lineVOffset, int index)
		{
			Invariant.Assert(this.IsLayoutDataValid);
			LineProperties lineProperties = this.GetLineProperties();
			this.EnsureComplexContent();
			double wrappingWidth = this.CalcWrappingWidth(base.RenderSize.Width);
			Vector vector = this.CalcContentOffset(base.RenderSize, wrappingWidth);
			distance -= vector.X;
			lineVOffset -= vector.Y;
			TextRunCache textRunCache = new TextRunCache();
			LineMetrics line = this.GetLine(index);
			ITextPointer result;
			using (Line line2 = this.CreateLine(lineProperties))
			{
				Invariant.Assert(index >= 0 && index < this.LineCount);
				TextLineBreak textLineBreak = this.GetLine(index).TextLineBreak;
				bool ellipsis = this.ParagraphEllipsisShownOnLine(index, lineVOffset);
				this.Format(line2, line.Length, dcp, wrappingWidth, this.GetLineProperties(dcp == 0, lineProperties), textLineBreak, textRunCache, ellipsis);
				Invariant.Assert(line.Length == line2.Length, "Line length is out of sync");
				CharacterHit textPositionFromDistance = line2.GetTextPositionFromDistance(distance);
				LogicalDirection gravity = (textPositionFromDistance.TrailingLength > 0) ? LogicalDirection.Backward : LogicalDirection.Forward;
				result = this._complexContent.TextContainer.Start.CreatePointer(textPositionFromDistance.FirstCharacterIndex + textPositionFromDistance.TrailingLength, gravity);
			}
			return result;
		}

		// Token: 0x0600574C RID: 22348 RVA: 0x00182B24 File Offset: 0x00180D24
		internal Rect GetRectangleFromTextPosition(ITextPointer orientedPosition)
		{
			Invariant.Assert(this.IsLayoutDataValid);
			Invariant.Assert(orientedPosition != null);
			LineProperties lineProperties = this.GetLineProperties();
			this.EnsureComplexContent();
			int num = this._complexContent.TextContainer.Start.GetOffsetToPosition(orientedPosition);
			int num2 = num;
			if (orientedPosition.LogicalDirection == LogicalDirection.Backward && num > 0)
			{
				num--;
			}
			double wrappingWidth = this.CalcWrappingWidth(base.RenderSize.Width);
			Vector vector = this.CalcContentOffset(base.RenderSize, wrappingWidth);
			double num3 = 0.0;
			int num4 = 0;
			TextRunCache textRunCache = new TextRunCache();
			Rect result = Rect.Empty;
			FlowDirection flowDirection = FlowDirection.LeftToRight;
			int lineCount = this.LineCount;
			for (int i = 0; i < lineCount; i++)
			{
				LineMetrics line = this.GetLine(i);
				if (num4 + line.Length > num || (num4 + line.Length == num && i == lineCount - 1))
				{
					using (Line line2 = this.CreateLine(lineProperties))
					{
						bool ellipsis = this.ParagraphEllipsisShownOnLine(i, num3);
						this.Format(line2, line.Length, num4, wrappingWidth, this.GetLineProperties(num4 == 0, lineProperties), line.TextLineBreak, textRunCache, ellipsis);
						Invariant.Assert(line.Length == line2.Length, "Line length is out of sync");
						result = line2.GetBoundsFromTextPosition(num, out flowDirection);
						break;
					}
				}
				num4 += line.Length;
				num3 += line.Height;
			}
			if (!result.IsEmpty)
			{
				result.X += vector.X;
				result.Y += vector.Y + num3;
				if (lineProperties.FlowDirection != flowDirection)
				{
					if (orientedPosition.LogicalDirection == LogicalDirection.Forward || num2 == 0)
					{
						result.X = result.Right;
					}
				}
				else if (orientedPosition.LogicalDirection == LogicalDirection.Backward && num2 > 0)
				{
					result.X = result.Right;
				}
				result.Width = 0.0;
			}
			return result;
		}

		// Token: 0x0600574D RID: 22349 RVA: 0x00182D2C File Offset: 0x00180F2C
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition)
		{
			Invariant.Assert(this.IsLayoutDataValid);
			Invariant.Assert(startPosition != null);
			Invariant.Assert(endPosition != null);
			Invariant.Assert(startPosition.CompareTo(endPosition) <= 0);
			Geometry result = null;
			LineProperties lineProperties = this.GetLineProperties();
			this.EnsureComplexContent();
			int offsetToPosition = this._complexContent.TextContainer.Start.GetOffsetToPosition(startPosition);
			int offsetToPosition2 = this._complexContent.TextContainer.Start.GetOffsetToPosition(endPosition);
			double wrappingWidth = this.CalcWrappingWidth(base.RenderSize.Width);
			Vector vector = this.CalcContentOffset(base.RenderSize, wrappingWidth);
			TextRunCache textRunCache = new TextRunCache();
			Line line = this.CreateLine(lineProperties);
			int num = 0;
			ITextPointer textPointer = this._complexContent.TextContainer.Start.CreatePointer(0);
			double num2 = 0.0;
			int lineCount = this.LineCount;
			int i = 0;
			int num3 = lineCount;
			while (i < num3)
			{
				LineMetrics line2 = this.GetLine(i);
				if (offsetToPosition2 <= num)
				{
					break;
				}
				int num4 = num + line2.Length;
				textPointer.MoveByOffset(line2.Length);
				if (offsetToPosition < num4)
				{
					using (line)
					{
						bool ellipsis = this.ParagraphEllipsisShownOnLine(i, num2);
						this.Format(line, line2.Length, num, wrappingWidth, this.GetLineProperties(num == 0, lineProperties), line2.TextLineBreak, textRunCache, ellipsis);
						if (Invariant.Strict)
						{
							Invariant.Assert(this.GetLine(i).Length == line.Length, "Line length is out of sync");
						}
						int num5 = Math.Max(num, offsetToPosition);
						int num6 = Math.Min(num4, offsetToPosition2);
						if (num5 != num6)
						{
							IList<Rect> rangeBounds = line.GetRangeBounds(num5, num6 - num5, vector.X, vector.Y + num2);
							if (rangeBounds.Count > 0)
							{
								int num7 = 0;
								int count = rangeBounds.Count;
								do
								{
									Rect rect = rangeBounds[num7];
									if (num7 == count - 1 && offsetToPosition2 >= num4 && TextPointerBase.IsNextToAnyBreak(textPointer, LogicalDirection.Backward))
									{
										double num8 = this.FontSize * 0.5;
										rect.Width += num8;
									}
									RectangleGeometry addedGeometry = new RectangleGeometry(rect);
									CaretElement.AddGeometry(ref result, addedGeometry);
								}
								while (++num7 < count);
							}
						}
					}
				}
				num += line2.Length;
				num2 += line2.Height;
				i++;
			}
			return result;
		}

		// Token: 0x0600574E RID: 22350 RVA: 0x00182FB8 File Offset: 0x001811B8
		internal bool IsAtCaretUnitBoundary(ITextPointer position, int dcp, int lineIndex)
		{
			Invariant.Assert(this.IsLayoutDataValid);
			LineProperties lineProperties = this.GetLineProperties();
			this.EnsureComplexContent();
			TextRunCache textRunCache = new TextRunCache();
			bool result = false;
			int offsetToPosition = this._complexContent.TextContainer.Start.GetOffsetToPosition(position);
			CharacterHit charHit = default(CharacterHit);
			if (position.LogicalDirection == LogicalDirection.Backward)
			{
				if (offsetToPosition <= dcp)
				{
					return false;
				}
				charHit = new CharacterHit(offsetToPosition - 1, 1);
			}
			else if (position.LogicalDirection == LogicalDirection.Forward)
			{
				charHit = new CharacterHit(offsetToPosition, 0);
			}
			LineMetrics line = this.GetLine(lineIndex);
			double wrappingWidth = this.CalcWrappingWidth(base.RenderSize.Width);
			using (Line line2 = this.CreateLine(lineProperties))
			{
				this.Format(line2, line.Length, dcp, wrappingWidth, this.GetLineProperties(lineIndex == 0, lineProperties), line.TextLineBreak, textRunCache, false);
				Invariant.Assert(line.Length == line2.Length, "Line length is out of sync");
				result = line2.IsAtCaretCharacterHit(charHit);
			}
			return result;
		}

		// Token: 0x0600574F RID: 22351 RVA: 0x001830C4 File Offset: 0x001812C4
		internal ITextPointer GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction, int dcp, int lineIndex)
		{
			Invariant.Assert(this.IsLayoutDataValid);
			LineProperties lineProperties = this.GetLineProperties();
			this.EnsureComplexContent();
			int offsetToPosition = this._complexContent.TextContainer.Start.GetOffsetToPosition(position);
			if (offsetToPosition == dcp && direction == LogicalDirection.Backward)
			{
				if (lineIndex == 0)
				{
					return position;
				}
				lineIndex--;
				dcp -= this.GetLine(lineIndex).Length;
			}
			else if (offsetToPosition == dcp + this.GetLine(lineIndex).Length && direction == LogicalDirection.Forward)
			{
				int lineCount = this.LineCount;
				if (lineIndex == lineCount - 1)
				{
					return position;
				}
				dcp += this.GetLine(lineIndex).Length;
				lineIndex++;
			}
			TextRunCache textRunCache = new TextRunCache();
			double wrappingWidth = this.CalcWrappingWidth(base.RenderSize.Width);
			CharacterHit index = new CharacterHit(offsetToPosition, 0);
			LineMetrics line = this.GetLine(lineIndex);
			CharacterHit characterHit;
			using (Line line2 = this.CreateLine(lineProperties))
			{
				this.Format(line2, line.Length, dcp, wrappingWidth, this.GetLineProperties(lineIndex == 0, lineProperties), line.TextLineBreak, textRunCache, false);
				Invariant.Assert(line.Length == line2.Length, "Line length is out of sync");
				if (direction == LogicalDirection.Forward)
				{
					characterHit = line2.GetNextCaretCharacterHit(index);
				}
				else
				{
					characterHit = line2.GetPreviousCaretCharacterHit(index);
				}
			}
			LogicalDirection gravity;
			if (characterHit.FirstCharacterIndex + characterHit.TrailingLength == dcp + this.GetLine(lineIndex).Length && direction == LogicalDirection.Forward)
			{
				if (lineIndex == this.LineCount - 1)
				{
					gravity = LogicalDirection.Backward;
				}
				else
				{
					gravity = LogicalDirection.Forward;
				}
			}
			else if (characterHit.FirstCharacterIndex + characterHit.TrailingLength == dcp && direction == LogicalDirection.Backward)
			{
				if (dcp == 0)
				{
					gravity = LogicalDirection.Forward;
				}
				else
				{
					gravity = LogicalDirection.Backward;
				}
			}
			else
			{
				gravity = ((characterHit.TrailingLength > 0) ? LogicalDirection.Backward : LogicalDirection.Forward);
			}
			return this._complexContent.TextContainer.Start.CreatePointer(characterHit.FirstCharacterIndex + characterHit.TrailingLength, gravity);
		}

		// Token: 0x06005750 RID: 22352 RVA: 0x001832C0 File Offset: 0x001814C0
		internal ITextPointer GetBackspaceCaretUnitPosition(ITextPointer position, int dcp, int lineIndex)
		{
			Invariant.Assert(this.IsLayoutDataValid);
			LineProperties lineProperties = this.GetLineProperties();
			this.EnsureComplexContent();
			int offsetToPosition = this._complexContent.TextContainer.Start.GetOffsetToPosition(position);
			if (offsetToPosition == dcp)
			{
				if (lineIndex == 0)
				{
					return position;
				}
				lineIndex--;
				dcp -= this.GetLine(lineIndex).Length;
			}
			double wrappingWidth = this.CalcWrappingWidth(base.RenderSize.Width);
			CharacterHit index = new CharacterHit(offsetToPosition, 0);
			LineMetrics line = this.GetLine(lineIndex);
			TextRunCache textRunCache = new TextRunCache();
			CharacterHit backspaceCaretCharacterHit;
			using (Line line2 = this.CreateLine(lineProperties))
			{
				this.Format(line2, line.Length, dcp, wrappingWidth, this.GetLineProperties(lineIndex == 0, lineProperties), line.TextLineBreak, textRunCache, false);
				Invariant.Assert(line.Length == line2.Length, "Line length is out of sync");
				backspaceCaretCharacterHit = line2.GetBackspaceCaretCharacterHit(index);
			}
			LogicalDirection gravity;
			if (backspaceCaretCharacterHit.FirstCharacterIndex + backspaceCaretCharacterHit.TrailingLength == dcp)
			{
				if (dcp == 0)
				{
					gravity = LogicalDirection.Forward;
				}
				else
				{
					gravity = LogicalDirection.Backward;
				}
			}
			else
			{
				gravity = ((backspaceCaretCharacterHit.TrailingLength > 0) ? LogicalDirection.Backward : LogicalDirection.Forward);
			}
			return this._complexContent.TextContainer.Start.CreatePointer(backspaceCaretCharacterHit.FirstCharacterIndex + backspaceCaretCharacterHit.TrailingLength, gravity);
		}

		// Token: 0x17001545 RID: 5445
		// (get) Token: 0x06005751 RID: 22353 RVA: 0x00183418 File Offset: 0x00181618
		internal TextFormatter TextFormatter
		{
			get
			{
				TextFormattingMode textFormattingMode = TextOptions.GetTextFormattingMode(this);
				if (TextFormattingMode.Display == textFormattingMode)
				{
					if (this._textFormatterDisplay == null)
					{
						this._textFormatterDisplay = TextFormatter.FromCurrentDispatcher(textFormattingMode);
					}
					return this._textFormatterDisplay;
				}
				if (this._textFormatterIdeal == null)
				{
					this._textFormatterIdeal = TextFormatter.FromCurrentDispatcher(textFormattingMode);
				}
				return this._textFormatterIdeal;
			}
		}

		// Token: 0x17001546 RID: 5446
		// (get) Token: 0x06005752 RID: 22354 RVA: 0x00183465 File Offset: 0x00181665
		internal ITextContainer TextContainer
		{
			get
			{
				this.EnsureComplexContent();
				return this._complexContent.TextContainer;
			}
		}

		// Token: 0x17001547 RID: 5447
		// (get) Token: 0x06005753 RID: 22355 RVA: 0x00183478 File Offset: 0x00181678
		internal ITextView TextView
		{
			get
			{
				this.EnsureComplexContent();
				return this._complexContent.TextView;
			}
		}

		// Token: 0x17001548 RID: 5448
		// (get) Token: 0x06005754 RID: 22356 RVA: 0x0018348B File Offset: 0x0018168B
		internal Highlights Highlights
		{
			get
			{
				this.EnsureComplexContent();
				return this._complexContent.Highlights;
			}
		}

		// Token: 0x17001549 RID: 5449
		// (get) Token: 0x06005755 RID: 22357 RVA: 0x001834A0 File Offset: 0x001816A0
		internal LineProperties ParagraphProperties
		{
			get
			{
				return this.GetLineProperties();
			}
		}

		// Token: 0x1700154A RID: 5450
		// (get) Token: 0x06005756 RID: 22358 RVA: 0x001834B8 File Offset: 0x001816B8
		internal bool IsLayoutDataValid
		{
			get
			{
				return base.IsMeasureValid && base.IsArrangeValid && this.CheckFlags(TextBlock.Flags.HasFirstLine) && !this.CheckFlags(TextBlock.Flags.ContentChangeInProgress) && !this.CheckFlags(TextBlock.Flags.MeasureInProgress) && !this.CheckFlags(TextBlock.Flags.ArrangeInProgress);
			}
		}

		// Token: 0x1700154B RID: 5451
		// (get) Token: 0x06005757 RID: 22359 RVA: 0x00183505 File Offset: 0x00181705
		internal bool HasComplexContent
		{
			get
			{
				return this._complexContent != null;
			}
		}

		// Token: 0x1700154C RID: 5452
		// (get) Token: 0x06005758 RID: 22360 RVA: 0x00183510 File Offset: 0x00181710
		internal bool IsTypographyDefaultValue
		{
			get
			{
				return !this.CheckFlags(TextBlock.Flags.IsTypographySet);
			}
		}

		// Token: 0x1700154D RID: 5453
		// (get) Token: 0x06005759 RID: 22361 RVA: 0x00183520 File Offset: 0x00181720
		// (set) Token: 0x0600575A RID: 22362 RVA: 0x00183537 File Offset: 0x00181737
		private ArrayList InlineObjects
		{
			get
			{
				if (this._complexContent != null)
				{
					return this._complexContent.InlineObjects;
				}
				return null;
			}
			set
			{
				if (this._complexContent != null)
				{
					this._complexContent.InlineObjects = value;
				}
			}
		}

		// Token: 0x1700154E RID: 5454
		// (get) Token: 0x0600575B RID: 22363 RVA: 0x0018354D File Offset: 0x0018174D
		// (set) Token: 0x0600575C RID: 22364 RVA: 0x00183557 File Offset: 0x00181757
		internal bool IsContentPresenterContainer
		{
			get
			{
				return this.CheckFlags(TextBlock.Flags.IsContentPresenterContainer);
			}
			set
			{
				this.SetFlags(value, TextBlock.Flags.IsContentPresenterContainer);
			}
		}

		// Token: 0x0600575D RID: 22365 RVA: 0x00183562 File Offset: 0x00181762
		private static void OnTypographyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TextBlock)d).SetFlags(true, TextBlock.Flags.IsTypographySet);
		}

		// Token: 0x0600575E RID: 22366 RVA: 0x00183575 File Offset: 0x00181775
		private object OnValidateTextView(object arg)
		{
			if (this.IsLayoutDataValid && this._complexContent != null)
			{
				this._complexContent.TextView.OnUpdated();
			}
			return null;
		}

		// Token: 0x0600575F RID: 22367 RVA: 0x00183598 File Offset: 0x00181798
		private static void InsertTextRun(ITextPointer position, string text, bool whitespacesIgnorable)
		{
			if (!(position is TextPointer) || ((TextPointer)position).Parent == null || ((TextPointer)position).Parent is TextBox)
			{
				position.InsertTextInRun(text);
				return;
			}
			if (!whitespacesIgnorable || text.Trim().Length > 0)
			{
				Run run = Inline.CreateImplicitRun(((TextPointer)position).Parent);
				((TextPointer)position).InsertTextElement(run);
				run.Text = text;
			}
		}

		// Token: 0x06005760 RID: 22368 RVA: 0x0018360C File Offset: 0x0018180C
		private Line CreateLine(LineProperties lineProperties)
		{
			Line result;
			if (this._complexContent == null)
			{
				result = new SimpleLine(this, this.Text, lineProperties.DefaultTextRunProperties);
			}
			else
			{
				result = new ComplexLine(this);
			}
			return result;
		}

		// Token: 0x06005761 RID: 22369 RVA: 0x0018363E File Offset: 0x0018183E
		private void EnsureComplexContent()
		{
			this.EnsureComplexContent(null);
		}

		// Token: 0x06005762 RID: 22370 RVA: 0x00183648 File Offset: 0x00181848
		private void EnsureComplexContent(ITextContainer textContainer)
		{
			if (this._complexContent == null)
			{
				if (textContainer == null)
				{
					textContainer = new TextContainer(this.IsContentPresenterContainer ? null : this, false);
				}
				this._complexContent = new TextBlock.ComplexContent(this, textContainer, false, this.Text);
				this._contentCache = null;
				if (this.CheckFlags(TextBlock.Flags.FormattedOnce))
				{
					Invariant.Assert(!this.CheckFlags(TextBlock.Flags.PendingTextContainerEventInit));
					this.InitializeTextContainerListeners();
					bool flag = base.IsMeasureValid && base.IsArrangeValid;
					base.InvalidateMeasure();
					base.InvalidateVisual();
					if (flag)
					{
						base.UpdateLayout();
						return;
					}
				}
				else
				{
					this.SetFlags(true, TextBlock.Flags.PendingTextContainerEventInit);
				}
			}
		}

		// Token: 0x06005763 RID: 22371 RVA: 0x001836E8 File Offset: 0x001818E8
		private void ClearComplexContent()
		{
			if (this._complexContent != null)
			{
				this._complexContent.Detach(this);
				this._complexContent = null;
				Invariant.Assert(this._contentCache == null, "Content cache should be null when complex content exists.");
			}
		}

		// Token: 0x06005764 RID: 22372 RVA: 0x00183718 File Offset: 0x00181918
		private void OnHighlightChanged(object sender, HighlightChangedEventArgs args)
		{
			Invariant.Assert(args != null);
			Invariant.Assert(args.Ranges != null);
			Invariant.Assert(this.CheckFlags(TextBlock.Flags.FormattedOnce), "Unexpected Highlights.Changed callback before first format!");
			if (args.OwnerType != typeof(SpellerHighlightLayer))
			{
				return;
			}
			base.InvalidateVisual();
		}

		// Token: 0x06005765 RID: 22373 RVA: 0x0018376B File Offset: 0x0018196B
		private void OnTextContainerChanging(object sender, EventArgs args)
		{
			if (this.CheckFlags(TextBlock.Flags.FormattedOnce))
			{
				this.VerifyTreeIsUnlocked();
				this.SetFlags(true, TextBlock.Flags.ContentChangeInProgress);
			}
		}

		// Token: 0x06005766 RID: 22374 RVA: 0x00183788 File Offset: 0x00181988
		private void OnTextContainerChange(object sender, TextContainerChangeEventArgs args)
		{
			Invariant.Assert(args != null);
			if (this._complexContent == null)
			{
				return;
			}
			Invariant.Assert(sender == this._complexContent.TextContainer, "Received text change for foreign TextContainer.");
			if (args.Count == 0)
			{
				return;
			}
			if (this.CheckFlags(TextBlock.Flags.FormattedOnce))
			{
				this.VerifyTreeIsUnlocked();
				this.SetFlags(false, TextBlock.Flags.ContentChangeInProgress);
				base.InvalidateMeasure();
			}
			if (!this.CheckFlags(TextBlock.Flags.TextContentChanging) && args.TextChange != TextChangeType.PropertyModified)
			{
				this.SetFlags(true, TextBlock.Flags.TextContentChanging);
				try
				{
					base.SetDeferredValue(TextBlock.TextProperty, new DeferredTextReference(this.TextContainer));
				}
				finally
				{
					this.SetFlags(false, TextBlock.Flags.TextContentChanging);
				}
			}
		}

		// Token: 0x06005767 RID: 22375 RVA: 0x00183840 File Offset: 0x00181A40
		private void EnsureTextBlockCache()
		{
			if (this._textBlockCache == null)
			{
				this._textBlockCache = new TextBlockCache();
				this._textBlockCache._lineProperties = this.GetLineProperties();
				this._textBlockCache._textRunCache = new TextRunCache();
			}
		}

		// Token: 0x06005768 RID: 22376 RVA: 0x00183878 File Offset: 0x00181A78
		private LineProperties GetLineProperties()
		{
			TextProperties defaultTextProperties = new TextProperties(this, this.IsTypographyDefaultValue);
			LineProperties lineProperties = new LineProperties(this, this, defaultTextProperties, null);
			bool flag = (bool)base.GetValue(TextBlock.IsHyphenationEnabledProperty);
			if (flag)
			{
				lineProperties.Hyphenator = this.EnsureHyphenator();
			}
			return lineProperties;
		}

		// Token: 0x06005769 RID: 22377 RVA: 0x001838BD File Offset: 0x00181ABD
		private TextParagraphProperties GetLineProperties(bool firstLine, LineProperties lineProperties)
		{
			return this.GetLineProperties(firstLine, false, lineProperties);
		}

		// Token: 0x0600576A RID: 22378 RVA: 0x001838C8 File Offset: 0x00181AC8
		private TextParagraphProperties GetLineProperties(bool firstLine, bool showParagraphEllipsis, LineProperties lineProperties)
		{
			this.GetLineProperties();
			firstLine = (firstLine && lineProperties.HasFirstLineProperties);
			if (showParagraphEllipsis)
			{
				return lineProperties.GetParaEllipsisLineProps(firstLine);
			}
			if (!firstLine)
			{
				return lineProperties;
			}
			return lineProperties.FirstLineProps;
		}

		// Token: 0x0600576B RID: 22379 RVA: 0x001838F5 File Offset: 0x00181AF5
		private double CalcLineAdvance(double lineHeight, LineProperties lineProperties)
		{
			return lineProperties.CalcLineAdvance(lineHeight);
		}

		// Token: 0x0600576C RID: 22380 RVA: 0x00183900 File Offset: 0x00181B00
		private Vector CalcContentOffset(Size computedSize, double wrappingWidth)
		{
			Vector result = default(Vector);
			Thickness padding = this.Padding;
			Size size = new Size(Math.Max(0.0, computedSize.Width - (padding.Left + padding.Right)), Math.Max(0.0, computedSize.Height - (padding.Top + padding.Bottom)));
			TextAlignment textAlignment = this.TextAlignment;
			if (textAlignment != TextAlignment.Right)
			{
				if (textAlignment == TextAlignment.Center)
				{
					result.X = (size.Width - wrappingWidth) / 2.0;
				}
			}
			else
			{
				result.X = size.Width - wrappingWidth;
			}
			result.X += padding.Left;
			result.Y += padding.Top;
			return result;
		}

		// Token: 0x0600576D RID: 22381 RVA: 0x001839D8 File Offset: 0x00181BD8
		private bool ParagraphEllipsisShownOnLine(int lineIndex, double lineVOffset)
		{
			if (lineIndex >= this.LineCount - 1)
			{
				return false;
			}
			if (!this.CheckFlags(TextBlock.Flags.HasParagraphEllipses))
			{
				return false;
			}
			double value = this.GetLine(lineIndex + 1).Height + this.GetLine(lineIndex).Height + lineVOffset;
			double value2 = Math.Max(0.0, base.RenderSize.Height - this.Padding.Bottom);
			return DoubleUtil.GreaterThan(value, value2) && !DoubleUtil.AreClose(value, value2);
		}

		// Token: 0x0600576E RID: 22382 RVA: 0x00183A64 File Offset: 0x00181C64
		private double CalcWrappingWidth(double width)
		{
			if (width < this._previousDesiredSize.Width)
			{
				width = this._previousDesiredSize.Width;
			}
			if (width > this._referenceSize.Width)
			{
				width = this._referenceSize.Width;
			}
			bool flag = DoubleUtil.AreClose(width, this._referenceSize.Width);
			double num = this.Padding.Left + this.Padding.Right;
			width = Math.Max(0.0, width - num);
			if (!flag && width != 0.0)
			{
				TextFormattingMode textFormattingMode = TextOptions.GetTextFormattingMode(this);
				if (textFormattingMode == TextFormattingMode.Display)
				{
					width += 0.5 / base.GetDpi().DpiScaleY;
				}
				if (num != 0.0)
				{
					width += 1E-11;
				}
			}
			TextDpi.EnsureValidLineWidth(ref width);
			return width;
		}

		// Token: 0x0600576F RID: 22383 RVA: 0x00183B44 File Offset: 0x00181D44
		private void Format(Line line, int length, int dcp, double wrappingWidth, TextParagraphProperties paragraphProperties, TextLineBreak textLineBreak, TextRunCache textRunCache, bool ellipsis)
		{
			line.Format(dcp, wrappingWidth, paragraphProperties, textLineBreak, textRunCache, ellipsis);
			if (line.Length < length && !FrameworkAppContextSwitches.TextBlockReflowsAfterMeasure)
			{
				double num = this._referenceSize.Width;
				double num2 = wrappingWidth;
				TextDpi.EnsureValidLineWidth(ref num);
				double num3 = 0.01;
				double num4;
				for (;;)
				{
					num4 = num2 + num3;
					if (num4 > num)
					{
						goto IL_7E;
					}
					line.Format(dcp, num4, paragraphProperties, textLineBreak, textRunCache, ellipsis);
					if (line.Length >= length)
					{
						break;
					}
					num2 = num4;
					num3 *= 2.0;
				}
				num = num4;
				IL_7E:
				for (double num5 = (num - num2) / 2.0; num5 > 0.01; num5 /= 2.0)
				{
					double num6 = num2 + num5;
					line.Format(dcp, num6, paragraphProperties, textLineBreak, textRunCache, ellipsis);
					if (line.Length < length)
					{
						num2 = num6;
					}
					else
					{
						num = num6;
					}
				}
				line.Format(dcp, num, paragraphProperties, textLineBreak, textRunCache, ellipsis);
			}
		}

		// Token: 0x06005770 RID: 22384 RVA: 0x00183C33 File Offset: 0x00181E33
		private void VerifyTreeIsUnlocked()
		{
			if (this.CheckFlags(TextBlock.Flags.TreeInReadOnlyMode))
			{
				throw new InvalidOperationException(SR.Get("IllegalTreeChangeDetected"));
			}
		}

		/// <summary>Returns a value that indicates whether the effective value of the <see cref="P:System.Windows.Controls.TextBlock.Text" /> property should be serialized during serialization of a <see cref="T:System.Windows.Controls.TextBlock" /> object.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.TextBlock.Text" /> property should be serialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005771 RID: 22385 RVA: 0x00183C50 File Offset: 0x00181E50
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeText()
		{
			bool result = false;
			if (this._complexContent == null)
			{
				object obj = base.ReadLocalValue(TextBlock.TextProperty);
				if (obj != null && obj != DependencyProperty.UnsetValue && obj as string != string.Empty)
				{
					result = true;
				}
			}
			return result;
		}

		/// <summary>Returns a value that indicates whether the effective value of the <see cref="P:System.Windows.Controls.TextBlock.Inlines" /> property should be serialized during serialization of a <see cref="T:System.Windows.Controls.TextBlock" /> object.</summary>
		/// <param name="manager">A serialization service manager object for this object.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.TextBlock.Inlines" /> property should be serialized; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="manager" /> is <see langword="null" />.</exception>
		// Token: 0x06005772 RID: 22386 RVA: 0x00183C93 File Offset: 0x00181E93
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeInlines(XamlDesignerSerializationManager manager)
		{
			return this._complexContent != null && manager != null && manager.XmlWriter == null;
		}

		// Token: 0x06005773 RID: 22387 RVA: 0x00183CAC File Offset: 0x00181EAC
		private void AlignContent()
		{
			LineProperties lineProperties = this.GetLineProperties();
			double wrappingWidth = this.CalcWrappingWidth(base.RenderSize.Width);
			Vector vector = this.CalcContentOffset(base.RenderSize, wrappingWidth);
			Line line = this.CreateLine(lineProperties);
			TextRunCache textRunCache = new TextRunCache();
			int num = 0;
			double num2 = 0.0;
			int lineCount = this.LineCount;
			for (int i = 0; i < lineCount; i++)
			{
				LineMetrics line2 = this.GetLine(i);
				using (line)
				{
					bool ellipsis = this.ParagraphEllipsisShownOnLine(i, num2);
					this.Format(line, line2.Length, num, wrappingWidth, this.GetLineProperties(num == 0, lineProperties), line2.TextLineBreak, textRunCache, ellipsis);
					double num3 = this.CalcLineAdvance(line.Height, lineProperties);
					Invariant.Assert(line2.Length == line.Length, "Line length is out of sync");
					num += this.UpdateLine(i, line2, line.Start, line.Width).Length;
					num2 += num3;
				}
			}
			this.SetFlags(false, TextBlock.Flags.RequiresAlignment);
		}

		// Token: 0x06005774 RID: 22388 RVA: 0x00183DDC File Offset: 0x00181FDC
		private static void OnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs args)
		{
			TextBlock textBlock = sender as TextBlock;
			ContentElement contentElement = args.TargetObject as ContentElement;
			if (textBlock != null && contentElement != null && TextBlock.ContainsContentElement(textBlock, contentElement))
			{
				args.Handled = true;
				ReadOnlyCollection<Rect> rectanglesCore = textBlock.GetRectanglesCore(contentElement);
				Invariant.Assert(rectanglesCore != null, "Rect collection cannot be null.");
				if (rectanglesCore.Count > 0)
				{
					textBlock.BringIntoView(rectanglesCore[0]);
					return;
				}
				textBlock.BringIntoView();
			}
		}

		// Token: 0x06005775 RID: 22389 RVA: 0x00183E48 File Offset: 0x00182048
		private static bool ContainsContentElement(TextBlock textBlock, ContentElement element)
		{
			return textBlock._complexContent != null && textBlock._complexContent.TextContainer is TextContainer && element is TextElement && textBlock._complexContent.TextContainer == ((TextElement)element).TextContainer;
		}

		// Token: 0x1700154F RID: 5455
		// (get) Token: 0x06005776 RID: 22390 RVA: 0x00183E96 File Offset: 0x00182096
		private int LineCount
		{
			get
			{
				if (!this.CheckFlags(TextBlock.Flags.HasFirstLine))
				{
					return 0;
				}
				if (this._subsequentLines != null)
				{
					return this._subsequentLines.Count + 1;
				}
				return 1;
			}
		}

		// Token: 0x06005777 RID: 22391 RVA: 0x00183EBE File Offset: 0x001820BE
		private LineMetrics GetLine(int index)
		{
			if (index != 0)
			{
				return this._subsequentLines[index - 1];
			}
			return this._firstLine;
		}

		// Token: 0x06005778 RID: 22392 RVA: 0x00183ED8 File Offset: 0x001820D8
		private LineMetrics UpdateLine(int index, LineMetrics metrics, double start, double width)
		{
			metrics = new LineMetrics(metrics, start, width);
			if (index == 0)
			{
				this._firstLine = metrics;
			}
			else
			{
				this._subsequentLines[index - 1] = metrics;
			}
			return metrics;
		}

		// Token: 0x06005779 RID: 22393 RVA: 0x00183F01 File Offset: 0x00182101
		private void SetFlags(bool value, TextBlock.Flags flags)
		{
			this._flags = (value ? (this._flags | flags) : (this._flags & ~flags));
		}

		// Token: 0x0600577A RID: 22394 RVA: 0x00183F1F File Offset: 0x0018211F
		private bool CheckFlags(TextBlock.Flags flags)
		{
			return (this._flags & flags) == flags;
		}

		// Token: 0x0600577B RID: 22395 RVA: 0x00183F2C File Offset: 0x0018212C
		private void VerifyReentrancy()
		{
			if (this.CheckFlags(TextBlock.Flags.MeasureInProgress))
			{
				throw new InvalidOperationException(SR.Get("MeasureReentrancyInvalid"));
			}
			if (this.CheckFlags(TextBlock.Flags.ArrangeInProgress))
			{
				throw new InvalidOperationException(SR.Get("ArrangeReentrancyInvalid"));
			}
			if (this.CheckFlags(TextBlock.Flags.ContentChangeInProgress))
			{
				throw new InvalidOperationException(SR.Get("TextContainerChangingReentrancyInvalid"));
			}
		}

		// Token: 0x0600577C RID: 22396 RVA: 0x00183F8C File Offset: 0x0018218C
		private int GetLineIndexFromDcp(int dcpLine)
		{
			Invariant.Assert(dcpLine >= 0);
			int i = 0;
			int num = 0;
			int lineCount = this.LineCount;
			while (i < lineCount)
			{
				if (num == dcpLine)
				{
					return i;
				}
				num += this.GetLine(i).Length;
				i++;
			}
			Invariant.Assert(false, "Dcp passed is not at start of any line in TextBlock");
			return -1;
		}

		// Token: 0x0600577D RID: 22397 RVA: 0x00183FE0 File Offset: 0x001821E0
		private TextPointer FindElementPosition(IInputElement e)
		{
			if (e is TextElement && (e as TextElement).TextContainer == this._complexContent.TextContainer)
			{
				return new TextPointer((e as TextElement).ElementStart);
			}
			TextPointer textPointer = new TextPointer((TextPointer)this._complexContent.TextContainer.Start);
			while (textPointer.CompareTo((TextPointer)this._complexContent.TextContainer.End) < 0)
			{
				TextPointerContext pointerContext = textPointer.GetPointerContext(LogicalDirection.Forward);
				if (pointerContext == TextPointerContext.EmbeddedElement)
				{
					DependencyObject adjacentElement = textPointer.GetAdjacentElement(LogicalDirection.Forward);
					if ((adjacentElement is ContentElement || adjacentElement is UIElement) && (adjacentElement == e as ContentElement || adjacentElement == e as UIElement))
					{
						return textPointer;
					}
				}
				textPointer.MoveByOffset(1);
			}
			return null;
		}

		// Token: 0x0600577E RID: 22398 RVA: 0x0018409C File Offset: 0x0018229C
		internal void OnChildBaselineOffsetChanged(DependencyObject source)
		{
			if (!this.CheckFlags(TextBlock.Flags.MeasureInProgress))
			{
				base.InvalidateMeasure();
				base.InvalidateVisual();
			}
		}

		// Token: 0x0600577F RID: 22399 RVA: 0x001840B4 File Offset: 0x001822B4
		private static void OnBaselineOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextElement value = TextElement.ContainerTextElementField.GetValue(d);
			if (value != null)
			{
				DependencyObject parent = value.TextContainer.Parent;
				TextBlock textBlock = parent as TextBlock;
				if (textBlock != null)
				{
					textBlock.OnChildBaselineOffsetChanged(d);
					return;
				}
				FlowDocument flowDocument = parent as FlowDocument;
				if (flowDocument != null && d is UIElement)
				{
					flowDocument.OnChildDesiredSizeChanged((UIElement)d);
				}
			}
		}

		// Token: 0x06005780 RID: 22400 RVA: 0x0018410C File Offset: 0x0018230C
		private void InitializeTextContainerListeners()
		{
			this._complexContent.TextContainer.Changing += this.OnTextContainerChanging;
			this._complexContent.TextContainer.Change += this.OnTextContainerChange;
			this._complexContent.Highlights.Changed += this.OnHighlightChanged;
		}

		// Token: 0x06005781 RID: 22401 RVA: 0x00184170 File Offset: 0x00182370
		private void ClearLineMetrics()
		{
			if (this.CheckFlags(TextBlock.Flags.HasFirstLine))
			{
				if (this._subsequentLines != null)
				{
					int count = this._subsequentLines.Count;
					for (int i = 0; i < count; i++)
					{
						this._subsequentLines[i].Dispose(false);
					}
					this._subsequentLines = null;
				}
				this._firstLine = this._firstLine.Dispose(true);
				this.SetFlags(false, TextBlock.Flags.HasFirstLine);
			}
		}

		// Token: 0x06005782 RID: 22402 RVA: 0x001841E8 File Offset: 0x001823E8
		private NaturalLanguageHyphenator EnsureHyphenator()
		{
			if (this.CheckFlags(TextBlock.Flags.IsHyphenatorSet))
			{
				return TextBlock.HyphenatorField.GetValue(this);
			}
			NaturalLanguageHyphenator naturalLanguageHyphenator = new NaturalLanguageHyphenator();
			TextBlock.HyphenatorField.SetValue(this, naturalLanguageHyphenator);
			this.SetFlags(true, TextBlock.Flags.IsHyphenatorSet);
			return naturalLanguageHyphenator;
		}

		// Token: 0x06005783 RID: 22403 RVA: 0x00184230 File Offset: 0x00182430
		private static bool IsValidTextTrimming(object o)
		{
			TextTrimming textTrimming = (TextTrimming)o;
			return textTrimming == TextTrimming.CharacterEllipsis || textTrimming == TextTrimming.None || textTrimming == TextTrimming.WordEllipsis;
		}

		// Token: 0x06005784 RID: 22404 RVA: 0x00184254 File Offset: 0x00182454
		private static bool IsValidTextWrap(object o)
		{
			TextWrapping textWrapping = (TextWrapping)o;
			return textWrapping == TextWrapping.Wrap || textWrapping == TextWrapping.NoWrap || textWrapping == TextWrapping.WrapWithOverflow;
		}

		// Token: 0x06005785 RID: 22405 RVA: 0x00184278 File Offset: 0x00182478
		private static object CoerceBaselineOffset(DependencyObject d, object value)
		{
			TextBlock textBlock = (TextBlock)d;
			if (DoubleUtil.IsNaN((double)value))
			{
				return textBlock._baselineOffset;
			}
			return value;
		}

		/// <summary>Returns a value that indicates whether the effective value of the <see cref="P:System.Windows.Controls.TextBlock.BaselineOffset" /> property should be serialized during serialization of a <see cref="T:System.Windows.Controls.TextBlock" /> object.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.TextBlock.BaselineOffset" /> property should be serialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005786 RID: 22406 RVA: 0x001842A8 File Offset: 0x001824A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBaselineOffset()
		{
			object obj = base.ReadLocalValue(TextBlock.BaselineOffsetProperty);
			return obj != DependencyProperty.UnsetValue && !DoubleUtil.IsNaN((double)obj);
		}

		// Token: 0x06005787 RID: 22407 RVA: 0x001842D9 File Offset: 0x001824D9
		private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBlock.OnTextChanged(d, (string)e.NewValue);
		}

		// Token: 0x06005788 RID: 22408 RVA: 0x001842F0 File Offset: 0x001824F0
		private static void OnTextChanged(DependencyObject d, string newText)
		{
			TextBlock textBlock = (TextBlock)d;
			if (textBlock.CheckFlags(TextBlock.Flags.TextContentChanging))
			{
				return;
			}
			if (textBlock._complexContent == null)
			{
				textBlock._contentCache = ((newText != null) ? newText : string.Empty);
				return;
			}
			textBlock.SetFlags(true, TextBlock.Flags.TextContentChanging);
			try
			{
				bool flag = true;
				Invariant.Assert(textBlock._contentCache == null, "Content cache should be null when complex content exists.");
				textBlock._complexContent.TextContainer.BeginChange();
				try
				{
					((TextContainer)textBlock._complexContent.TextContainer).DeleteContentInternal((TextPointer)textBlock._complexContent.TextContainer.Start, (TextPointer)textBlock._complexContent.TextContainer.End);
					TextBlock.InsertTextRun(textBlock._complexContent.TextContainer.End, newText, true);
					flag = false;
				}
				finally
				{
					textBlock._complexContent.TextContainer.EndChange();
					if (flag)
					{
						textBlock.ClearLineMetrics();
					}
				}
			}
			finally
			{
				textBlock.SetFlags(false, TextBlock.Flags.TextContentChanging);
			}
		}

		// Token: 0x17001550 RID: 5456
		// (get) Token: 0x06005789 RID: 22409 RVA: 0x000962DF File Offset: 0x000944DF
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 28;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.BaselineOffset" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.BaselineOffset" /> dependency property.</returns>
		// Token: 0x04002E64 RID: 11876
		public static readonly DependencyProperty BaselineOffsetProperty = DependencyProperty.RegisterAttached("BaselineOffset", typeof(double), typeof(TextBlock), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(TextBlock.OnBaselineOffsetChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.Text" /> dependency property.</summary>
		/// <returns>The identifier of the <see cref="P:System.Windows.Controls.TextBlock.Text" /> dependency property.</returns>
		// Token: 0x04002E65 RID: 11877
		[CommonDependencyProperty]
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TextBlock), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(TextBlock.OnTextChanged), new CoerceValueCallback(TextBlock.CoerceText)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.FontFamily" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.FontFamily" /> dependency property.</returns>
		// Token: 0x04002E66 RID: 11878
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(TextBlock));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.FontStyle" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.FontStyle" /> dependency property.</returns>
		// Token: 0x04002E67 RID: 11879
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(TextBlock));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.FontWeight" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.FontWeight" /> dependency property.</returns>
		// Token: 0x04002E68 RID: 11880
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof(TextBlock));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.FontStretch" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.FontStretch" /> dependency property.</returns>
		// Token: 0x04002E69 RID: 11881
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(typeof(TextBlock));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.FontSize" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.FontSize" /> dependency property.</returns>
		// Token: 0x04002E6A RID: 11882
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(TextBlock));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.Foreground" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.Foreground" /> dependency property.</returns>
		// Token: 0x04002E6B RID: 11883
		[CommonDependencyProperty]
		public static readonly DependencyProperty ForegroundProperty = TextElement.ForegroundProperty.AddOwner(typeof(TextBlock));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.Background" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.Background" /> dependency property.</returns>
		// Token: 0x04002E6C RID: 11884
		[CommonDependencyProperty]
		public static readonly DependencyProperty BackgroundProperty = TextElement.BackgroundProperty.AddOwner(typeof(TextBlock), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.TextDecorations" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.TextDecorations" /> dependency property.</returns>
		// Token: 0x04002E6D RID: 11885
		[CommonDependencyProperty]
		public static readonly DependencyProperty TextDecorationsProperty = Inline.TextDecorationsProperty.AddOwner(typeof(TextBlock), new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(TextDecorationCollection.Empty), FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.TextEffects" /> dependency property.</summary>
		/// <returns>The identifier of the <see cref="P:System.Windows.Controls.TextBlock.TextEffects" /> dependency property.</returns>
		// Token: 0x04002E6E RID: 11886
		public static readonly DependencyProperty TextEffectsProperty = TextElement.TextEffectsProperty.AddOwner(typeof(TextBlock), new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(TextEffectCollection.Empty), FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.LineHeight" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.LineHeight" /> dependency property.</returns>
		// Token: 0x04002E6F RID: 11887
		public static readonly DependencyProperty LineHeightProperty = Block.LineHeightProperty.AddOwner(typeof(TextBlock));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.LineStackingStrategy" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.LineStackingStrategy" /> dependency property.</returns>
		// Token: 0x04002E70 RID: 11888
		public static readonly DependencyProperty LineStackingStrategyProperty = Block.LineStackingStrategyProperty.AddOwner(typeof(TextBlock));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.Padding" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.Padding" /> dependency property.</returns>
		// Token: 0x04002E71 RID: 11889
		public static readonly DependencyProperty PaddingProperty = Block.PaddingProperty.AddOwner(typeof(TextBlock), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.TextAlignment" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.TextAlignment" /> dependency property.</returns>
		// Token: 0x04002E72 RID: 11890
		public static readonly DependencyProperty TextAlignmentProperty = Block.TextAlignmentProperty.AddOwner(typeof(TextBlock));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.TextTrimming" /> dependency property. </summary>
		/// <returns>The identifier of the <see cref="P:System.Windows.Controls.TextBlock.TextTrimming" /> dependency property.</returns>
		// Token: 0x04002E73 RID: 11891
		[CommonDependencyProperty]
		public static readonly DependencyProperty TextTrimmingProperty = DependencyProperty.Register("TextTrimming", typeof(TextTrimming), typeof(TextBlock), new FrameworkPropertyMetadata(TextTrimming.None, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(TextBlock.IsValidTextTrimming));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.TextWrapping" /> dependency property. </summary>
		/// <returns>The identifier of the <see cref="P:System.Windows.Controls.TextBlock.TextWrapping" /> dependency property.</returns>
		// Token: 0x04002E74 RID: 11892
		[CommonDependencyProperty]
		public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(TextBlock), new FrameworkPropertyMetadata(TextWrapping.NoWrap, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(TextBlock.IsValidTextWrap));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.IsHyphenationEnabled" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.IsHyphenationEnabled" /> dependency property.</returns>
		// Token: 0x04002E75 RID: 11893
		public static readonly DependencyProperty IsHyphenationEnabledProperty = Block.IsHyphenationEnabledProperty.AddOwner(typeof(TextBlock));

		// Token: 0x04002E76 RID: 11894
		private TextBlockCache _textBlockCache;

		// Token: 0x04002E77 RID: 11895
		private string _contentCache;

		// Token: 0x04002E78 RID: 11896
		private TextBlock.ComplexContent _complexContent;

		// Token: 0x04002E79 RID: 11897
		private TextFormatter _textFormatterIdeal;

		// Token: 0x04002E7A RID: 11898
		private TextFormatter _textFormatterDisplay;

		// Token: 0x04002E7B RID: 11899
		private Size _referenceSize;

		// Token: 0x04002E7C RID: 11900
		private Size _previousDesiredSize;

		// Token: 0x04002E7D RID: 11901
		private double _baselineOffset;

		// Token: 0x04002E7E RID: 11902
		private static readonly UncommonField<NaturalLanguageHyphenator> HyphenatorField = new UncommonField<NaturalLanguageHyphenator>();

		// Token: 0x04002E7F RID: 11903
		private LineMetrics _firstLine;

		// Token: 0x04002E80 RID: 11904
		private List<LineMetrics> _subsequentLines;

		// Token: 0x04002E81 RID: 11905
		private TextBlock.Flags _flags;

		// Token: 0x020009C0 RID: 2496
		[Flags]
		private enum Flags
		{
			// Token: 0x04004571 RID: 17777
			FormattedOnce = 1,
			// Token: 0x04004572 RID: 17778
			MeasureInProgress = 2,
			// Token: 0x04004573 RID: 17779
			TreeInReadOnlyMode = 4,
			// Token: 0x04004574 RID: 17780
			RequiresAlignment = 8,
			// Token: 0x04004575 RID: 17781
			ContentChangeInProgress = 16,
			// Token: 0x04004576 RID: 17782
			IsContentPresenterContainer = 32,
			// Token: 0x04004577 RID: 17783
			HasParagraphEllipses = 64,
			// Token: 0x04004578 RID: 17784
			PendingTextContainerEventInit = 128,
			// Token: 0x04004579 RID: 17785
			ArrangeInProgress = 256,
			// Token: 0x0400457A RID: 17786
			IsTypographySet = 512,
			// Token: 0x0400457B RID: 17787
			TextContentChanging = 1024,
			// Token: 0x0400457C RID: 17788
			IsHyphenatorSet = 2048,
			// Token: 0x0400457D RID: 17789
			HasFirstLine = 4096
		}

		// Token: 0x020009C1 RID: 2497
		private class ComplexContent
		{
			// Token: 0x0600888B RID: 34955 RVA: 0x00252594 File Offset: 0x00250794
			internal ComplexContent(TextBlock owner, ITextContainer textContainer, bool foreignTextContianer, string content)
			{
				this.VisualChildren = new VisualCollection(owner);
				this.TextContainer = textContainer;
				this.ForeignTextContainer = foreignTextContianer;
				if (content != null && content.Length > 0)
				{
					TextBlock.InsertTextRun(this.TextContainer.End, content, false);
				}
				this.TextView = new TextParagraphView(owner, this.TextContainer);
				this.TextContainer.TextView = this.TextView;
			}

			// Token: 0x0600888C RID: 34956 RVA: 0x00252608 File Offset: 0x00250808
			internal void Detach(TextBlock owner)
			{
				this.Highlights.Changed -= owner.OnHighlightChanged;
				this.TextContainer.Changing -= owner.OnTextContainerChanging;
				this.TextContainer.Change -= owner.OnTextContainerChange;
			}

			// Token: 0x17001ED3 RID: 7891
			// (get) Token: 0x0600888D RID: 34957 RVA: 0x0025265A File Offset: 0x0025085A
			internal Highlights Highlights
			{
				get
				{
					return this.TextContainer.Highlights;
				}
			}

			// Token: 0x0400457E RID: 17790
			internal VisualCollection VisualChildren;

			// Token: 0x0400457F RID: 17791
			internal readonly ITextContainer TextContainer;

			// Token: 0x04004580 RID: 17792
			internal readonly bool ForeignTextContainer;

			// Token: 0x04004581 RID: 17793
			internal readonly TextParagraphView TextView;

			// Token: 0x04004582 RID: 17794
			internal ArrayList InlineObjects;
		}

		// Token: 0x020009C2 RID: 2498
		private class SimpleContentEnumerator : IEnumerator
		{
			// Token: 0x0600888E RID: 34958 RVA: 0x00252667 File Offset: 0x00250867
			internal SimpleContentEnumerator(string content)
			{
				this._content = content;
				this._initialized = false;
				this._invalidPosition = false;
			}

			// Token: 0x0600888F RID: 34959 RVA: 0x00252684 File Offset: 0x00250884
			void IEnumerator.Reset()
			{
				this._initialized = false;
				this._invalidPosition = false;
			}

			// Token: 0x06008890 RID: 34960 RVA: 0x00252694 File Offset: 0x00250894
			bool IEnumerator.MoveNext()
			{
				if (!this._initialized)
				{
					this._initialized = true;
					return true;
				}
				this._invalidPosition = true;
				return false;
			}

			// Token: 0x17001ED4 RID: 7892
			// (get) Token: 0x06008891 RID: 34961 RVA: 0x002526AF File Offset: 0x002508AF
			object IEnumerator.Current
			{
				get
				{
					if (!this._initialized || this._invalidPosition)
					{
						throw new InvalidOperationException();
					}
					return this._content;
				}
			}

			// Token: 0x04004583 RID: 17795
			private readonly string _content;

			// Token: 0x04004584 RID: 17796
			private bool _initialized;

			// Token: 0x04004585 RID: 17797
			private bool _invalidPosition;
		}
	}
}
