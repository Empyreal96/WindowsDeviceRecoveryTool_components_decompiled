using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml;
using MS.Internal;
using MS.Internal.PresentationFramework.Markup;

namespace System.Windows.Documents
{
	/// <summary>Represents a selection of content between two <see cref="T:System.Windows.Documents.TextPointer" /> positions.</summary>
	// Token: 0x0200040B RID: 1035
	public class TextRange : ITextRange
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.TextRange" /> class, taking two specified <see cref="T:System.Windows.Documents.TextPointer" /> positions as the beginning and end positions for the new range.</summary>
		/// <param name="position1">A fixed anchor position that marks one end of the selection used to form the new <see cref="T:System.Windows.Documents.TextRange" />.</param>
		/// <param name="position2">A movable position that marks the other end of the selection used to form the new <see cref="T:System.Windows.Documents.TextRange" />.</param>
		/// <exception cref="T:System.ArgumentException">Occurs when <paramref name="position1" /> and <paramref name="position2" /> are not positioned within the same document.</exception>
		/// <exception cref="T:System.ArgumentNullException">Occurs when <paramref name="position1" /> or <paramref name="position2" /> is <see langword="null" />.</exception>
		// Token: 0x06003AA3 RID: 15011 RVA: 0x00109FEC File Offset: 0x001081EC
		public TextRange(TextPointer position1, TextPointer position2) : this(position1, position2)
		{
		}

		// Token: 0x06003AA4 RID: 15012 RVA: 0x00109FF6 File Offset: 0x001081F6
		internal TextRange(ITextPointer position1, ITextPointer position2) : this(position1, position2, false)
		{
		}

		// Token: 0x06003AA5 RID: 15013 RVA: 0x0010A004 File Offset: 0x00108204
		internal TextRange(ITextPointer position1, ITextPointer position2, bool ignoreTextUnitBoundaries)
		{
			if (position1 == null)
			{
				throw new ArgumentNullException("position1");
			}
			if (position2 == null)
			{
				throw new ArgumentNullException("position2");
			}
			this.SetFlags(ignoreTextUnitBoundaries, TextRange.Flags.IgnoreTextUnitBoundaries);
			ValidationHelper.VerifyPosition(position1.TextContainer, position1, "position1");
			ValidationHelper.VerifyPosition(position1.TextContainer, position2, "position2");
			TextRangeBase.Select(this, position1, position2);
		}

		// Token: 0x06003AA6 RID: 15014 RVA: 0x0010A065 File Offset: 0x00108265
		internal TextRange(TextPointer position1, TextPointer position2, bool useRestrictiveXamlXmlReader) : this(position1, position2)
		{
			this._useRestrictiveXamlXmlReader = useRestrictiveXamlXmlReader;
		}

		// Token: 0x06003AA7 RID: 15015 RVA: 0x0010A076 File Offset: 0x00108276
		bool ITextRange.Contains(ITextPointer position)
		{
			return TextRangeBase.Contains(this, position);
		}

		// Token: 0x06003AA8 RID: 15016 RVA: 0x0010A07F File Offset: 0x0010827F
		void ITextRange.Select(ITextPointer position1, ITextPointer position2)
		{
			TextRangeBase.Select(this, position1, position2);
		}

		// Token: 0x06003AA9 RID: 15017 RVA: 0x0010A089 File Offset: 0x00108289
		void ITextRange.SelectWord(ITextPointer position)
		{
			TextRangeBase.SelectWord(this, position);
		}

		// Token: 0x06003AAA RID: 15018 RVA: 0x0010A092 File Offset: 0x00108292
		void ITextRange.SelectParagraph(ITextPointer position)
		{
			TextRangeBase.SelectParagraph(this, position);
		}

		// Token: 0x06003AAB RID: 15019 RVA: 0x0010A09B File Offset: 0x0010829B
		void ITextRange.ApplyTypingHeuristics(bool overType)
		{
			TextRangeBase.ApplyTypingHeuristics(this, overType);
		}

		// Token: 0x06003AAC RID: 15020 RVA: 0x0010A0A4 File Offset: 0x001082A4
		object ITextRange.GetPropertyValue(DependencyProperty formattingProperty)
		{
			return TextRangeBase.GetPropertyValue(this, formattingProperty);
		}

		// Token: 0x06003AAD RID: 15021 RVA: 0x0010A0AD File Offset: 0x001082AD
		UIElement ITextRange.GetUIElementSelected()
		{
			return TextRangeBase.GetUIElementSelected(this);
		}

		// Token: 0x06003AAE RID: 15022 RVA: 0x0010A0B5 File Offset: 0x001082B5
		bool ITextRange.CanSave(string dataFormat)
		{
			return TextRangeBase.CanSave(this, dataFormat);
		}

		// Token: 0x06003AAF RID: 15023 RVA: 0x0010A0BE File Offset: 0x001082BE
		void ITextRange.Save(Stream stream, string dataFormat)
		{
			TextRangeBase.Save(this, stream, dataFormat, false);
		}

		// Token: 0x06003AB0 RID: 15024 RVA: 0x0010A0C9 File Offset: 0x001082C9
		void ITextRange.Save(Stream stream, string dataFormat, bool preserveTextElements)
		{
			TextRangeBase.Save(this, stream, dataFormat, preserveTextElements);
		}

		// Token: 0x06003AB1 RID: 15025 RVA: 0x0010A0D4 File Offset: 0x001082D4
		void ITextRange.BeginChange()
		{
			TextRangeBase.BeginChange(this);
		}

		// Token: 0x06003AB2 RID: 15026 RVA: 0x0010A0DC File Offset: 0x001082DC
		void ITextRange.BeginChangeNoUndo()
		{
			TextRangeBase.BeginChangeNoUndo(this);
		}

		// Token: 0x06003AB3 RID: 15027 RVA: 0x0010A0E4 File Offset: 0x001082E4
		void ITextRange.EndChange()
		{
			TextRangeBase.EndChange(this, false, false);
		}

		// Token: 0x06003AB4 RID: 15028 RVA: 0x0010A0EE File Offset: 0x001082EE
		void ITextRange.EndChange(bool disableScroll, bool skipEvents)
		{
			TextRangeBase.EndChange(this, disableScroll, skipEvents);
		}

		// Token: 0x06003AB5 RID: 15029 RVA: 0x0010A0F8 File Offset: 0x001082F8
		IDisposable ITextRange.DeclareChangeBlock()
		{
			return new TextRange.ChangeBlock(this, false);
		}

		// Token: 0x06003AB6 RID: 15030 RVA: 0x0010A101 File Offset: 0x00108301
		IDisposable ITextRange.DeclareChangeBlock(bool disableScroll)
		{
			return new TextRange.ChangeBlock(this, disableScroll);
		}

		// Token: 0x06003AB7 RID: 15031 RVA: 0x0010A10A File Offset: 0x0010830A
		void ITextRange.NotifyChanged(bool disableScroll, bool skipEvents)
		{
			TextRangeBase.NotifyChanged(this, disableScroll);
		}

		// Token: 0x17000EC6 RID: 3782
		// (get) Token: 0x06003AB8 RID: 15032 RVA: 0x0010A113 File Offset: 0x00108313
		bool ITextRange.IgnoreTextUnitBoundaries
		{
			get
			{
				return this.CheckFlags(TextRange.Flags.IgnoreTextUnitBoundaries);
			}
		}

		// Token: 0x17000EC7 RID: 3783
		// (get) Token: 0x06003AB9 RID: 15033 RVA: 0x0010A11C File Offset: 0x0010831C
		ITextPointer ITextRange.Start
		{
			get
			{
				return TextRangeBase.GetStart(this);
			}
		}

		// Token: 0x17000EC8 RID: 3784
		// (get) Token: 0x06003ABA RID: 15034 RVA: 0x0010A124 File Offset: 0x00108324
		ITextPointer ITextRange.End
		{
			get
			{
				return TextRangeBase.GetEnd(this);
			}
		}

		// Token: 0x17000EC9 RID: 3785
		// (get) Token: 0x06003ABB RID: 15035 RVA: 0x0010A12C File Offset: 0x0010832C
		bool ITextRange.IsEmpty
		{
			get
			{
				return TextRangeBase.GetIsEmpty(this);
			}
		}

		// Token: 0x17000ECA RID: 3786
		// (get) Token: 0x06003ABC RID: 15036 RVA: 0x0010A134 File Offset: 0x00108334
		List<TextSegment> ITextRange.TextSegments
		{
			get
			{
				return TextRangeBase.GetTextSegments(this);
			}
		}

		// Token: 0x17000ECB RID: 3787
		// (get) Token: 0x06003ABD RID: 15037 RVA: 0x0010A13C File Offset: 0x0010833C
		bool ITextRange.HasConcreteTextContainer
		{
			get
			{
				Invariant.Assert(this._textSegments != null, "_textSegments must not be null");
				Invariant.Assert(this._textSegments.Count > 0, "_textSegments.Count must be > 0");
				return this._textSegments[0].Start is TextPointer;
			}
		}

		// Token: 0x17000ECC RID: 3788
		// (get) Token: 0x06003ABE RID: 15038 RVA: 0x0010A190 File Offset: 0x00108390
		// (set) Token: 0x06003ABF RID: 15039 RVA: 0x0010A198 File Offset: 0x00108398
		string ITextRange.Text
		{
			get
			{
				return TextRangeBase.GetText(this);
			}
			set
			{
				TextRangeBase.SetText(this, value);
			}
		}

		// Token: 0x17000ECD RID: 3789
		// (get) Token: 0x06003AC0 RID: 15040 RVA: 0x0010A1A1 File Offset: 0x001083A1
		string ITextRange.Xml
		{
			get
			{
				return TextRangeBase.GetXml(this);
			}
		}

		// Token: 0x17000ECE RID: 3790
		// (get) Token: 0x06003AC1 RID: 15041 RVA: 0x0010A1A9 File Offset: 0x001083A9
		int ITextRange.ChangeBlockLevel
		{
			get
			{
				return TextRangeBase.GetChangeBlockLevel(this);
			}
		}

		// Token: 0x17000ECF RID: 3791
		// (get) Token: 0x06003AC2 RID: 15042 RVA: 0x0010A1B1 File Offset: 0x001083B1
		bool ITextRange.IsTableCellRange
		{
			get
			{
				return TextRangeBase.GetIsTableCellRange(this);
			}
		}

		// Token: 0x14000090 RID: 144
		// (add) Token: 0x06003AC3 RID: 15043 RVA: 0x0010A1B9 File Offset: 0x001083B9
		// (remove) Token: 0x06003AC4 RID: 15044 RVA: 0x0010A1C2 File Offset: 0x001083C2
		event EventHandler ITextRange.Changed
		{
			add
			{
				this.Changed += value;
			}
			remove
			{
				this.Changed -= value;
			}
		}

		// Token: 0x06003AC5 RID: 15045 RVA: 0x0010A1CB File Offset: 0x001083CB
		void ITextRange.FireChanged()
		{
			if (this.Changed != null)
			{
				this.Changed(this, EventArgs.Empty);
			}
		}

		// Token: 0x17000ED0 RID: 3792
		// (get) Token: 0x06003AC6 RID: 15046 RVA: 0x0010A1E6 File Offset: 0x001083E6
		// (set) Token: 0x06003AC7 RID: 15047 RVA: 0x0010A1EF File Offset: 0x001083EF
		bool ITextRange._IsTableCellRange
		{
			get
			{
				return this.CheckFlags(TextRange.Flags.IsTableCellRange);
			}
			set
			{
				this.SetFlags(value, TextRange.Flags.IsTableCellRange);
			}
		}

		// Token: 0x17000ED1 RID: 3793
		// (get) Token: 0x06003AC8 RID: 15048 RVA: 0x0010A1F9 File Offset: 0x001083F9
		// (set) Token: 0x06003AC9 RID: 15049 RVA: 0x0010A201 File Offset: 0x00108401
		List<TextSegment> ITextRange._TextSegments
		{
			get
			{
				return this._textSegments;
			}
			set
			{
				this._textSegments = value;
			}
		}

		// Token: 0x17000ED2 RID: 3794
		// (get) Token: 0x06003ACA RID: 15050 RVA: 0x0010A20A File Offset: 0x0010840A
		// (set) Token: 0x06003ACB RID: 15051 RVA: 0x0010A212 File Offset: 0x00108412
		int ITextRange._ChangeBlockLevel
		{
			get
			{
				return this._changeBlockLevel;
			}
			set
			{
				this._changeBlockLevel = value;
			}
		}

		// Token: 0x17000ED3 RID: 3795
		// (get) Token: 0x06003ACC RID: 15052 RVA: 0x0010A21B File Offset: 0x0010841B
		// (set) Token: 0x06003ACD RID: 15053 RVA: 0x0010A223 File Offset: 0x00108423
		ChangeBlockUndoRecord ITextRange._ChangeBlockUndoRecord
		{
			get
			{
				return this._changeBlockUndoRecord;
			}
			set
			{
				this._changeBlockUndoRecord = value;
			}
		}

		// Token: 0x17000ED4 RID: 3796
		// (get) Token: 0x06003ACE RID: 15054 RVA: 0x0010A22C File Offset: 0x0010842C
		// (set) Token: 0x06003ACF RID: 15055 RVA: 0x0010A234 File Offset: 0x00108434
		bool ITextRange._IsChanged
		{
			get
			{
				return this._IsChanged;
			}
			set
			{
				this._IsChanged = value;
			}
		}

		// Token: 0x17000ED5 RID: 3797
		// (get) Token: 0x06003AD0 RID: 15056 RVA: 0x0010A23D File Offset: 0x0010843D
		// (set) Token: 0x06003AD1 RID: 15057 RVA: 0x0010A245 File Offset: 0x00108445
		uint ITextRange._ContentGeneration
		{
			get
			{
				return this._ContentGeneration;
			}
			set
			{
				this._ContentGeneration = value;
			}
		}

		/// <summary>Checks whether a position (specified by a <see cref="T:System.Windows.Documents.TextPointer" />) is located within the current selection.</summary>
		/// <param name="textPointer">A position to test for inclusion in the current selection.</param>
		/// <returns>
		///     <see langword="true" /> if the specified position is located within the current selection; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentException">Occurs when textPointer is not in the same document as the current selection.</exception>
		// Token: 0x06003AD2 RID: 15058 RVA: 0x0010A24E File Offset: 0x0010844E
		public bool Contains(TextPointer textPointer)
		{
			return ((ITextRange)this).Contains(textPointer);
		}

		/// <summary>Updates the current selection, taking two <see cref="T:System.Windows.Documents.TextPointer" /> positions to indicate the updated selection.</summary>
		/// <param name="position1">A fixed anchor position that marks one end of the updated selection.</param>
		/// <param name="position2">A movable position that marks the other end of the updated selection.</param>
		/// <exception cref="T:System.ArgumentException">Occurs when <paramref name="position1" /> and <paramref name="position2" /> are not positioned within the same document.</exception>
		/// <exception cref="T:System.ArgumentNullException">Occurs when <paramref name="position1" /> or <paramref name="position2" /> is <see langword="null" />.</exception>
		// Token: 0x06003AD3 RID: 15059 RVA: 0x0010A257 File Offset: 0x00108457
		public void Select(TextPointer position1, TextPointer position2)
		{
			((ITextRange)this).Select(position1, position2);
		}

		// Token: 0x06003AD4 RID: 15060 RVA: 0x0010A261 File Offset: 0x00108461
		internal void SelectWord(TextPointer textPointer)
		{
			((ITextRange)this).SelectWord(textPointer);
		}

		// Token: 0x06003AD5 RID: 15061 RVA: 0x0010A26A File Offset: 0x0010846A
		internal void SelectParagraph(ITextPointer position)
		{
			((ITextRange)this).SelectParagraph(position);
		}

		/// <summary>Applies a specified formatting property and value to the current selection.</summary>
		/// <param name="formattingProperty">A formatting property to apply.</param>
		/// <param name="value">The value for the formatting property.</param>
		/// <exception cref="T:System.ArgumentException">Occurs when <paramref name="formattingProperty" /> does not specify a valid formatting property, or <paramref name="value" /> specifies an invalid value for <paramref name="formattingProperty" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">Occurs when <paramref name="formattingProperty" /> is <see langword="null" />.</exception>
		// Token: 0x06003AD6 RID: 15062 RVA: 0x0010A273 File Offset: 0x00108473
		public void ApplyPropertyValue(DependencyProperty formattingProperty, object value)
		{
			this.ApplyPropertyValue(formattingProperty, value, false, PropertyValueAction.SetValue);
		}

		// Token: 0x06003AD7 RID: 15063 RVA: 0x0010A27F File Offset: 0x0010847F
		internal void ApplyPropertyValue(DependencyProperty formattingProperty, object value, bool applyToParagraphs)
		{
			this.ApplyPropertyValue(formattingProperty, value, applyToParagraphs, PropertyValueAction.SetValue);
		}

		// Token: 0x06003AD8 RID: 15064 RVA: 0x0010A28C File Offset: 0x0010848C
		internal void ApplyPropertyValue(DependencyProperty formattingProperty, object value, bool applyToParagraphs, PropertyValueAction propertyValueAction)
		{
			Invariant.Assert(this.HasConcreteTextContainer, "Can't apply property to non-TextContainer range!");
			if (formattingProperty == null)
			{
				throw new ArgumentNullException("formattingProperty");
			}
			if (!TextSchema.IsCharacterProperty(formattingProperty) && !TextSchema.IsParagraphProperty(formattingProperty))
			{
				throw new ArgumentException(SR.Get("TextEditorPropertyIsNotApplicableForTextFormatting", new object[]
				{
					formattingProperty.Name
				}));
			}
			if (value is string && formattingProperty.PropertyType != typeof(string))
			{
				TypeConverter converter = TypeDescriptor.GetConverter(formattingProperty.PropertyType);
				Invariant.Assert(converter != null);
				value = converter.ConvertFromString((string)value);
			}
			if (!formattingProperty.IsValidValue(value) && (!(formattingProperty.PropertyType == typeof(Thickness)) || !(value is Thickness)))
			{
				throw new ArgumentException(SR.Get("TextEditorTypeOfParameterIsNotAppropriateForFormattingProperty", new object[]
				{
					(value == null) ? "null" : value.GetType().Name,
					formattingProperty.Name
				}), "value");
			}
			if (propertyValueAction != PropertyValueAction.SetValue && propertyValueAction != PropertyValueAction.IncreaseByAbsoluteValue && propertyValueAction != PropertyValueAction.DecreaseByAbsoluteValue && propertyValueAction != PropertyValueAction.IncreaseByPercentageValue && propertyValueAction != PropertyValueAction.DecreaseByPercentageValue)
			{
				throw new ArgumentException(SR.Get("TextRange_InvalidParameterValue"), "propertyValueAction");
			}
			if (propertyValueAction != PropertyValueAction.SetValue && !TextSchema.IsPropertyIncremental(formattingProperty))
			{
				throw new ArgumentException(SR.Get("TextRange_PropertyCannotBeIncrementedOrDecremented", new object[]
				{
					formattingProperty.Name
				}), "propertyValueAction");
			}
			this.ApplyPropertyToTextVirtual(formattingProperty, value, applyToParagraphs, propertyValueAction);
		}

		/// <summary>Removes all formatting properties (represented by <see cref="T:System.Windows.Documents.Inline" /> elements) from the current selection.</summary>
		// Token: 0x06003AD9 RID: 15065 RVA: 0x0010A3F4 File Offset: 0x001085F4
		public void ClearAllProperties()
		{
			Invariant.Assert(this.HasConcreteTextContainer, "Can't clear properties in non-TextContainer range");
			this.ClearAllPropertiesVirtual();
		}

		/// <summary>Returns the effective value of a specified formatting property on the current selection.</summary>
		/// <param name="formattingProperty">A formatting property to get the value of with respect to the current selection.</param>
		/// <returns>An object specifying the value of the specified formatting property.</returns>
		/// <exception cref="T:System.ArgumentException">Occurs when <paramref name="formattingProperty" /> does not specify a valid formatting property, or <paramref name="value" /> specifies an invalid value for <paramref name="formattingProperty" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">Occurs when <paramref name="formattingProperty" /> is <see langword="null" />.</exception>
		// Token: 0x06003ADA RID: 15066 RVA: 0x0010A40C File Offset: 0x0010860C
		public object GetPropertyValue(DependencyProperty formattingProperty)
		{
			if (formattingProperty == null)
			{
				throw new ArgumentNullException("formattingProperty");
			}
			if (!TextSchema.IsCharacterProperty(formattingProperty) && !TextSchema.IsParagraphProperty(formattingProperty))
			{
				throw new ArgumentException(SR.Get("TextEditorPropertyIsNotApplicableForTextFormatting", new object[]
				{
					formattingProperty.Name
				}));
			}
			return ((ITextRange)this).GetPropertyValue(formattingProperty);
		}

		// Token: 0x06003ADB RID: 15067 RVA: 0x0010A45D File Offset: 0x0010865D
		internal UIElement GetUIElementSelected()
		{
			return ((ITextRange)this).GetUIElementSelected();
		}

		/// <summary>Checks whether the current selection can be saved as a specified data format.</summary>
		/// <param name="dataFormat">A data format to check for save compatibility with the current selection.  See <see cref="T:System.Windows.DataFormats" /> for a list of predefined data formats.</param>
		/// <returns>
		///     <see langword="true" /> if the current selection can be saved as the specified data format; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003ADC RID: 15068 RVA: 0x0010A465 File Offset: 0x00108665
		public bool CanSave(string dataFormat)
		{
			return ((ITextRange)this).CanSave(dataFormat);
		}

		/// <summary>Checks whether the current selection can be loaded with content in a specified data format.</summary>
		/// <param name="dataFormat">A data format to check for load-compatibility into the current selection.  See <see cref="T:System.Windows.DataFormats" /> for a list of predefined data formats.</param>
		/// <returns>
		///     <see langword="true" /> if the current selection can be loaded with content in the specified data format; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003ADD RID: 15069 RVA: 0x0010A46E File Offset: 0x0010866E
		public bool CanLoad(string dataFormat)
		{
			return TextRangeBase.CanLoad(this, dataFormat);
		}

		/// <summary>Saves the current selection to a specified stream in a specified data format.</summary>
		/// <param name="stream">An empty, writable stream to save the current selection to.</param>
		/// <param name="dataFormat">A data format to save the current selection as.  Currently supported data formats are <see cref="F:System.Windows.DataFormats.Rtf" />, <see cref="F:System.Windows.DataFormats.Text" />, <see cref="F:System.Windows.DataFormats.Xaml" />, and <see cref="F:System.Windows.DataFormats.XamlPackage" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="stream" /> or <paramref name="dataFormat" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The specified data format is unsupported.-orContent loaded from <paramref name="stream" /> does not match the specified data format.</exception>
		// Token: 0x06003ADE RID: 15070 RVA: 0x0010A477 File Offset: 0x00108677
		public void Save(Stream stream, string dataFormat)
		{
			((ITextRange)this).Save(stream, dataFormat);
		}

		/// <summary>Saves the current selection to a specified stream in a specified data format, with the option of preserving custom <see cref="T:System.Windows.Documents.TextElement" /> objects.</summary>
		/// <param name="stream">An empty, writable stream to save the current selection to.</param>
		/// <param name="dataFormat">A data format to save the current selection as.  Currently supported data formats are <see cref="F:System.Windows.DataFormats.Rtf" />, <see cref="F:System.Windows.DataFormats.Text" />, <see cref="F:System.Windows.DataFormats.Xaml" />, and <see cref="F:System.Windows.DataFormats.XamlPackage" />.</param>
		/// <param name="preserveTextElements">
		///       <see langword="true" /> to preserve custom <see cref="T:System.Windows.Documents.TextElement" /> objects; otherwise, <see langword="false" />.</param>
		/// <exception cref="T:System.ArgumentNullException">Occurs when <paramref name="stream" /> or <paramref name="dataFormat" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">Occurs when the specified data format is unsupported.  May also be raised if content loaded from <paramref name="stream" /> does not match the specified data format.</exception>
		// Token: 0x06003ADF RID: 15071 RVA: 0x0010A481 File Offset: 0x00108681
		public void Save(Stream stream, string dataFormat, bool preserveTextElements)
		{
			((ITextRange)this).Save(stream, dataFormat, preserveTextElements);
		}

		/// <summary>Loads the current selection in a specified data format from a specified stream.</summary>
		/// <param name="stream">A readable stream that contains data to load into the current selection.</param>
		/// <param name="dataFormat">A data format to load the data as.  Currently supported data formats are <see cref="F:System.Windows.DataFormats.Rtf" />, <see cref="F:System.Windows.DataFormats.Text" />, <see cref="F:System.Windows.DataFormats.Xaml" />, and <see cref="F:System.Windows.DataFormats.XamlPackage" />.</param>
		/// <exception cref="T:System.ArgumentNullException">Occurs when <paramref name="stream" /> or <paramref name="dataFormat" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">Occurs when the specified data format is unsupported.  May also be raised if content loaded from <paramref name="stream" /> does not match the specified data format.</exception>
		// Token: 0x06003AE0 RID: 15072 RVA: 0x0010A48C File Offset: 0x0010868C
		public void Load(Stream stream, string dataFormat)
		{
			this.LoadVirtual(stream, dataFormat);
		}

		// Token: 0x06003AE1 RID: 15073 RVA: 0x0010A496 File Offset: 0x00108696
		internal void InsertEmbeddedUIElement(FrameworkElement embeddedElement)
		{
			Invariant.Assert(embeddedElement != null);
			this.InsertEmbeddedUIElementVirtual(embeddedElement);
		}

		// Token: 0x06003AE2 RID: 15074 RVA: 0x0010A4A8 File Offset: 0x001086A8
		internal void InsertImage(Image image)
		{
			BitmapSource bitmapSource = (BitmapSource)image.Source;
			Invariant.Assert(bitmapSource != null);
			if (double.IsNaN(image.Height))
			{
				if ((double)bitmapSource.PixelHeight < 300.0)
				{
					image.Height = (double)bitmapSource.PixelHeight;
				}
				else
				{
					image.Height = 300.0;
				}
			}
			if (double.IsNaN(image.Width))
			{
				if ((double)bitmapSource.PixelHeight < 300.0)
				{
					image.Width = (double)bitmapSource.PixelWidth;
				}
				else
				{
					image.Width = 300.0 / (double)bitmapSource.PixelHeight * (double)bitmapSource.PixelWidth;
				}
			}
			this.InsertEmbeddedUIElement(image);
		}

		// Token: 0x06003AE3 RID: 15075 RVA: 0x0010A55B File Offset: 0x0010875B
		internal virtual void SetXmlVirtual(TextElement fragment)
		{
			if (!this.IsTableCellRange)
			{
				TextRangeSerialization.PasteXml(this, fragment);
			}
		}

		// Token: 0x06003AE4 RID: 15076 RVA: 0x0010A56C File Offset: 0x0010876C
		internal virtual void LoadVirtual(Stream stream, string dataFormat)
		{
			TextRangeBase.Load(this, stream, dataFormat);
		}

		// Token: 0x06003AE5 RID: 15077 RVA: 0x0010A576 File Offset: 0x00108776
		internal Table InsertTable(int rowCount, int columnCount)
		{
			Invariant.Assert(this.HasConcreteTextContainer, "InsertTable: TextRange must belong to non-abstract TextContainer");
			return this.InsertTableVirtual(rowCount, columnCount);
		}

		// Token: 0x06003AE6 RID: 15078 RVA: 0x0010A590 File Offset: 0x00108790
		internal TextRange InsertRows(int rowCount)
		{
			Invariant.Assert(this.HasConcreteTextContainer, "InsertRows: TextRange must belong to non-abstract TextContainer");
			return this.InsertRowsVirtual(rowCount);
		}

		// Token: 0x06003AE7 RID: 15079 RVA: 0x0010A5A9 File Offset: 0x001087A9
		internal bool DeleteRows()
		{
			Invariant.Assert(this.HasConcreteTextContainer, "DeleteRows: TextRange must belong to non-abstract TextContainer");
			return this.DeleteRowsVirtual();
		}

		// Token: 0x06003AE8 RID: 15080 RVA: 0x0010A5C1 File Offset: 0x001087C1
		internal TextRange InsertColumns(int columnCount)
		{
			Invariant.Assert(this.HasConcreteTextContainer, "InsertColumns: TextRange must belong to non-abstract TextContainer");
			return this.InsertColumnsVirtual(columnCount);
		}

		// Token: 0x06003AE9 RID: 15081 RVA: 0x0010A5DA File Offset: 0x001087DA
		internal bool DeleteColumns()
		{
			Invariant.Assert(this.HasConcreteTextContainer, "DeleteColumns: TextRange must belong to non-abstract TextContainer");
			return this.DeleteColumnsVirtual();
		}

		// Token: 0x06003AEA RID: 15082 RVA: 0x0010A5F2 File Offset: 0x001087F2
		internal TextRange MergeCells()
		{
			Invariant.Assert(this.HasConcreteTextContainer, "MergeCells: TextRange must belong to non-abstract TextContainer");
			return this.MergeCellsVirtual();
		}

		// Token: 0x06003AEB RID: 15083 RVA: 0x0010A60A File Offset: 0x0010880A
		internal TextRange SplitCell(int splitCountHorizontal, int splitCountVertical)
		{
			Invariant.Assert(this.HasConcreteTextContainer, "SplitCells: TextRange must belong to non-abstract TextContainer");
			return this.SplitCellVirtual(splitCountHorizontal, splitCountVertical);
		}

		/// <summary>Gets the position that marks the beginning of the current selection.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.TextPointer" /> that points to the beginning of the current selection.</returns>
		// Token: 0x17000ED6 RID: 3798
		// (get) Token: 0x06003AEC RID: 15084 RVA: 0x0010A624 File Offset: 0x00108824
		public TextPointer Start
		{
			get
			{
				return (TextPointer)((ITextRange)this).Start;
			}
		}

		/// <summary>Get the position that marks the end of the current selection.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.TextPointer" /> that points to the end of the current selection.</returns>
		// Token: 0x17000ED7 RID: 3799
		// (get) Token: 0x06003AED RID: 15085 RVA: 0x0010A631 File Offset: 0x00108831
		public TextPointer End
		{
			get
			{
				return (TextPointer)((ITextRange)this).End;
			}
		}

		/// <summary>Gets a value indicating whether or not the current selection is empty.</summary>
		/// <returns>
		///     <see langword="true" /> if the current selection is empty; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000ED8 RID: 3800
		// (get) Token: 0x06003AEE RID: 15086 RVA: 0x0010A63E File Offset: 0x0010883E
		public bool IsEmpty
		{
			get
			{
				return ((ITextRange)this).IsEmpty;
			}
		}

		// Token: 0x17000ED9 RID: 3801
		// (get) Token: 0x06003AEF RID: 15087 RVA: 0x0010A646 File Offset: 0x00108846
		internal bool HasConcreteTextContainer
		{
			get
			{
				return ((ITextRange)this).HasConcreteTextContainer;
			}
		}

		// Token: 0x17000EDA RID: 3802
		// (get) Token: 0x06003AF0 RID: 15088 RVA: 0x0010A64E File Offset: 0x0010884E
		internal FrameworkElement ContainingFrameworkElement
		{
			get
			{
				if (this.HasConcreteTextContainer)
				{
					return this.Start.ContainingFrameworkElement;
				}
				return null;
			}
		}

		/// <summary>Gets or sets the plain text contents of the current selection.</summary>
		/// <returns>A string containing the plain text contents of the current selection.</returns>
		/// <exception cref="T:System.ArgumentNullException">Occurs when an attempt is made to set this property to <see langword="null" />.</exception>
		// Token: 0x17000EDB RID: 3803
		// (get) Token: 0x06003AF1 RID: 15089 RVA: 0x0010A665 File Offset: 0x00108865
		// (set) Token: 0x06003AF2 RID: 15090 RVA: 0x0010A66D File Offset: 0x0010886D
		public string Text
		{
			get
			{
				return ((ITextRange)this).Text;
			}
			set
			{
				((ITextRange)this).Text = value;
			}
		}

		// Token: 0x17000EDC RID: 3804
		// (get) Token: 0x06003AF3 RID: 15091 RVA: 0x0010A676 File Offset: 0x00108876
		// (set) Token: 0x06003AF4 RID: 15092 RVA: 0x0010A680 File Offset: 0x00108880
		internal string Xml
		{
			get
			{
				return ((ITextRange)this).Xml;
			}
			set
			{
				TextRangeBase.BeginChange(this);
				try
				{
					object obj = XamlReaderProxy.Load(new XmlTextReader(new StringReader(value)), this._useRestrictiveXamlXmlReader);
					TextElement textElement = obj as TextElement;
					if (textElement != null)
					{
						this.SetXmlVirtual(textElement);
					}
				}
				finally
				{
					TextRangeBase.EndChange(this);
				}
			}
		}

		// Token: 0x17000EDD RID: 3805
		// (get) Token: 0x06003AF5 RID: 15093 RVA: 0x0010A6D4 File Offset: 0x001088D4
		internal bool IsTableCellRange
		{
			get
			{
				return ((ITextRange)this).IsTableCellRange;
			}
		}

		/// <summary>Occurs when the range is repositioned to cover a new span of content.</summary>
		// Token: 0x14000091 RID: 145
		// (add) Token: 0x06003AF6 RID: 15094 RVA: 0x0010A6DC File Offset: 0x001088DC
		// (remove) Token: 0x06003AF7 RID: 15095 RVA: 0x0010A714 File Offset: 0x00108914
		public event EventHandler Changed;

		// Token: 0x06003AF8 RID: 15096 RVA: 0x0010A749 File Offset: 0x00108949
		internal void BeginChange()
		{
			((ITextRange)this).BeginChange();
		}

		// Token: 0x06003AF9 RID: 15097 RVA: 0x0010A751 File Offset: 0x00108951
		internal void EndChange()
		{
			((ITextRange)this).EndChange();
		}

		// Token: 0x06003AFA RID: 15098 RVA: 0x0010A759 File Offset: 0x00108959
		internal IDisposable DeclareChangeBlock()
		{
			return ((ITextRange)this).DeclareChangeBlock();
		}

		// Token: 0x06003AFB RID: 15099 RVA: 0x0010A761 File Offset: 0x00108961
		internal IDisposable DeclareChangeBlock(bool disableScroll)
		{
			return ((ITextRange)this).DeclareChangeBlock(disableScroll);
		}

		// Token: 0x17000EDE RID: 3806
		// (get) Token: 0x06003AFC RID: 15100 RVA: 0x0010A76A File Offset: 0x0010896A
		// (set) Token: 0x06003AFD RID: 15101 RVA: 0x0010A773 File Offset: 0x00108973
		internal bool _IsChanged
		{
			get
			{
				return this.CheckFlags(TextRange.Flags.IsChanged);
			}
			set
			{
				this.SetFlags(value, TextRange.Flags.IsChanged);
			}
		}

		// Token: 0x06003AFE RID: 15102 RVA: 0x0010A780 File Offset: 0x00108980
		internal virtual void InsertEmbeddedUIElementVirtual(FrameworkElement embeddedElement)
		{
			Invariant.Assert(this.HasConcreteTextContainer, "Can't insert embedded object to non-TextContainer range!");
			Invariant.Assert(embeddedElement != null);
			TextRangeBase.BeginChange(this);
			try
			{
				this.Text = string.Empty;
				TextPointer textPointer = TextRangeEditTables.EnsureInsertionPosition(this.Start);
				Paragraph paragraph = textPointer.Paragraph;
				if (paragraph != null)
				{
					if (Paragraph.HasNoTextContent(paragraph))
					{
						BlockUIContainer blockUIContainer = new BlockUIContainer(embeddedElement);
						blockUIContainer.TextAlignment = TextRangeEdit.GetTextAlignmentFromHorizontalAlignment(embeddedElement.HorizontalAlignment);
						paragraph.SiblingBlocks.InsertAfter(paragraph, blockUIContainer);
						paragraph.SiblingBlocks.Remove(paragraph);
						this.Select(blockUIContainer.ContentStart, blockUIContainer.ContentEnd);
					}
					else
					{
						InlineUIContainer inlineUIContainer = new InlineUIContainer(embeddedElement);
						TextPointer textPointer2 = TextRangeEdit.SplitFormattingElements(this.Start, false);
						textPointer2.InsertTextElement(inlineUIContainer);
						this.Select(inlineUIContainer.ElementStart, inlineUIContainer.ElementEnd);
					}
				}
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x06003AFF RID: 15103 RVA: 0x0010A864 File Offset: 0x00108A64
		internal virtual void ApplyPropertyToTextVirtual(DependencyProperty formattingProperty, object value, bool applyToParagraphs, PropertyValueAction propertyValueAction)
		{
			TextRangeBase.BeginChange(this);
			try
			{
				for (int i = 0; i < this._textSegments.Count; i++)
				{
					TextSegment textSegment = this._textSegments[i];
					if (formattingProperty == FrameworkElement.FlowDirectionProperty)
					{
						if (applyToParagraphs || this.IsEmpty || TextRangeBase.IsParagraphBoundaryCrossed(this))
						{
							TextRangeEdit.SetParagraphProperty((TextPointer)textSegment.Start, (TextPointer)textSegment.End, formattingProperty, value, propertyValueAction);
						}
						else
						{
							TextRangeEdit.SetInlineProperty((TextPointer)textSegment.Start, (TextPointer)textSegment.End, formattingProperty, value, propertyValueAction);
						}
					}
					else if (TextSchema.IsCharacterProperty(formattingProperty))
					{
						TextRangeEdit.SetInlineProperty((TextPointer)textSegment.Start, (TextPointer)textSegment.End, formattingProperty, value, propertyValueAction);
					}
					else if (TextSchema.IsParagraphProperty(formattingProperty))
					{
						if (formattingProperty.PropertyType == typeof(Thickness) && (FlowDirection)textSegment.Start.GetValue(Block.FlowDirectionProperty) == FlowDirection.RightToLeft)
						{
							value = new Thickness(((Thickness)value).Right, ((Thickness)value).Top, ((Thickness)value).Left, ((Thickness)value).Bottom);
						}
						TextRangeEdit.SetParagraphProperty((TextPointer)textSegment.Start, (TextPointer)textSegment.End, formattingProperty, value, propertyValueAction);
					}
				}
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x06003B00 RID: 15104 RVA: 0x0010A9FC File Offset: 0x00108BFC
		internal virtual void ClearAllPropertiesVirtual()
		{
			TextRangeBase.BeginChange(this);
			try
			{
				TextRangeEdit.CharacterResetFormatting(this.Start, this.End);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x06003B01 RID: 15105 RVA: 0x0010AA3C File Offset: 0x00108C3C
		internal virtual Table InsertTableVirtual(int rowCount, int columnCount)
		{
			TextRangeBase.BeginChange(this);
			Table result;
			try
			{
				result = TextRangeEditTables.InsertTable(this.End, rowCount, columnCount);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
			return result;
		}

		// Token: 0x06003B02 RID: 15106 RVA: 0x0010AA78 File Offset: 0x00108C78
		internal virtual TextRange InsertRowsVirtual(int rowCount)
		{
			TextRangeBase.BeginChange(this);
			TextRange result;
			try
			{
				result = TextRangeEditTables.InsertRows(this, rowCount);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
			return result;
		}

		// Token: 0x06003B03 RID: 15107 RVA: 0x0010AAB0 File Offset: 0x00108CB0
		internal virtual bool DeleteRowsVirtual()
		{
			TextRangeBase.BeginChange(this);
			bool result;
			try
			{
				result = TextRangeEditTables.DeleteRows(this);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
			return result;
		}

		// Token: 0x06003B04 RID: 15108 RVA: 0x0010AAE4 File Offset: 0x00108CE4
		internal virtual TextRange InsertColumnsVirtual(int columnCount)
		{
			TextRangeBase.BeginChange(this);
			TextRange result;
			try
			{
				result = TextRangeEditTables.InsertColumns(this, columnCount);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
			return result;
		}

		// Token: 0x06003B05 RID: 15109 RVA: 0x0010AB1C File Offset: 0x00108D1C
		internal virtual bool DeleteColumnsVirtual()
		{
			TextRangeBase.BeginChange(this);
			bool result;
			try
			{
				result = TextRangeEditTables.DeleteColumns(this);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
			return result;
		}

		// Token: 0x06003B06 RID: 15110 RVA: 0x0010AB50 File Offset: 0x00108D50
		internal virtual TextRange MergeCellsVirtual()
		{
			TextRangeBase.BeginChange(this);
			TextRange result;
			try
			{
				result = TextRangeEditTables.MergeCells(this);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
			return result;
		}

		// Token: 0x06003B07 RID: 15111 RVA: 0x0010AB84 File Offset: 0x00108D84
		internal virtual TextRange SplitCellVirtual(int splitCountHorizontal, int splitCountVertical)
		{
			TextRangeBase.BeginChange(this);
			TextRange result;
			try
			{
				result = TextRangeEditTables.SplitCell(this, splitCountHorizontal, splitCountVertical);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
			return result;
		}

		// Token: 0x17000EDF RID: 3807
		// (get) Token: 0x06003B08 RID: 15112 RVA: 0x0010A20A File Offset: 0x0010840A
		internal int ChangeBlockLevel
		{
			get
			{
				return this._changeBlockLevel;
			}
		}

		// Token: 0x06003B09 RID: 15113 RVA: 0x0010ABBC File Offset: 0x00108DBC
		private void SetFlags(bool value, TextRange.Flags flags)
		{
			this._flags = (value ? (this._flags | flags) : (this._flags & ~flags));
		}

		// Token: 0x06003B0A RID: 15114 RVA: 0x0010ABDA File Offset: 0x00108DDA
		private bool CheckFlags(TextRange.Flags flags)
		{
			return (this._flags & flags) == flags;
		}

		// Token: 0x040025E0 RID: 9696
		private bool _useRestrictiveXamlXmlReader;

		// Token: 0x040025E1 RID: 9697
		private List<TextSegment> _textSegments;

		// Token: 0x040025E2 RID: 9698
		private int _changeBlockLevel;

		// Token: 0x040025E3 RID: 9699
		private ChangeBlockUndoRecord _changeBlockUndoRecord;

		// Token: 0x040025E4 RID: 9700
		private uint _ContentGeneration;

		// Token: 0x040025E5 RID: 9701
		private TextRange.Flags _flags;

		// Token: 0x02000908 RID: 2312
		private class ChangeBlock : IDisposable
		{
			// Token: 0x060085D7 RID: 34263 RVA: 0x0024B260 File Offset: 0x00249460
			internal ChangeBlock(ITextRange range, bool disableScroll)
			{
				this._range = range;
				this._disableScroll = disableScroll;
				this._range.BeginChange();
			}

			// Token: 0x060085D8 RID: 34264 RVA: 0x0024B281 File Offset: 0x00249481
			void IDisposable.Dispose()
			{
				this._range.EndChange(this._disableScroll, false);
				GC.SuppressFinalize(this);
			}

			// Token: 0x0400430F RID: 17167
			private readonly ITextRange _range;

			// Token: 0x04004310 RID: 17168
			private readonly bool _disableScroll;
		}

		// Token: 0x02000909 RID: 2313
		[Flags]
		private enum Flags
		{
			// Token: 0x04004312 RID: 17170
			IgnoreTextUnitBoundaries = 1,
			// Token: 0x04004313 RID: 17171
			IsChanged = 2,
			// Token: 0x04004314 RID: 17172
			IsTableCellRange = 4
		}
	}
}
