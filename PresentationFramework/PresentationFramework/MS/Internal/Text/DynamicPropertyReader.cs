using System;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

namespace MS.Internal.Text
{
	// Token: 0x020005FE RID: 1534
	internal static class DynamicPropertyReader
	{
		// Token: 0x06006611 RID: 26129 RVA: 0x001CB4D8 File Offset: 0x001C96D8
		internal static Typeface GetTypeface(DependencyObject element)
		{
			FontFamily fontFamily = (FontFamily)element.GetValue(TextElement.FontFamilyProperty);
			FontStyle style = (FontStyle)element.GetValue(TextElement.FontStyleProperty);
			FontWeight weight = (FontWeight)element.GetValue(TextElement.FontWeightProperty);
			FontStretch stretch = (FontStretch)element.GetValue(TextElement.FontStretchProperty);
			return new Typeface(fontFamily, style, weight, stretch);
		}

		// Token: 0x06006612 RID: 26130 RVA: 0x001CB534 File Offset: 0x001C9734
		internal static Typeface GetModifiedTypeface(DependencyObject element, FontFamily fontFamily)
		{
			FontStyle style = (FontStyle)element.GetValue(TextElement.FontStyleProperty);
			FontWeight weight = (FontWeight)element.GetValue(TextElement.FontWeightProperty);
			FontStretch stretch = (FontStretch)element.GetValue(TextElement.FontStretchProperty);
			return new Typeface(fontFamily, style, weight, stretch);
		}

		// Token: 0x06006613 RID: 26131 RVA: 0x001CB580 File Offset: 0x001C9780
		internal static TextDecorationCollection GetTextDecorationsForInlineObject(DependencyObject element, TextDecorationCollection textDecorations)
		{
			DependencyObject parent = LogicalTreeHelper.GetParent(element);
			TextDecorationCollection textDecorationCollection = null;
			if (parent != null)
			{
				textDecorationCollection = DynamicPropertyReader.GetTextDecorations(parent);
			}
			if (!((textDecorations == null) ? (textDecorationCollection == null) : textDecorations.ValueEquals(textDecorationCollection)))
			{
				if (textDecorationCollection == null)
				{
					textDecorations = null;
				}
				else
				{
					textDecorations = new TextDecorationCollection();
					int count = textDecorationCollection.Count;
					for (int i = 0; i < count; i++)
					{
						textDecorations.Add(textDecorationCollection[i]);
					}
				}
			}
			return textDecorations;
		}

		// Token: 0x06006614 RID: 26132 RVA: 0x001CB5E9 File Offset: 0x001C97E9
		internal static TextDecorationCollection GetTextDecorations(DependencyObject element)
		{
			return DynamicPropertyReader.GetCollectionValue(element, Inline.TextDecorationsProperty) as TextDecorationCollection;
		}

		// Token: 0x06006615 RID: 26133 RVA: 0x001CB5FB File Offset: 0x001C97FB
		internal static TextEffectCollection GetTextEffects(DependencyObject element)
		{
			return DynamicPropertyReader.GetCollectionValue(element, TextElement.TextEffectsProperty) as TextEffectCollection;
		}

		// Token: 0x06006616 RID: 26134 RVA: 0x001CB610 File Offset: 0x001C9810
		private static object GetCollectionValue(DependencyObject element, DependencyProperty property)
		{
			bool flag;
			if (element.GetValueSource(property, null, out flag) != BaseValueSourceInternal.Default || flag)
			{
				return element.GetValue(property);
			}
			return null;
		}

		// Token: 0x06006617 RID: 26135 RVA: 0x001CB63C File Offset: 0x001C983C
		internal static bool GetKeepTogether(DependencyObject element)
		{
			Paragraph paragraph = element as Paragraph;
			return paragraph != null && paragraph.KeepTogether;
		}

		// Token: 0x06006618 RID: 26136 RVA: 0x001CB65C File Offset: 0x001C985C
		internal static bool GetKeepWithNext(DependencyObject element)
		{
			Paragraph paragraph = element as Paragraph;
			return paragraph != null && paragraph.KeepWithNext;
		}

		// Token: 0x06006619 RID: 26137 RVA: 0x001CB67C File Offset: 0x001C987C
		internal static int GetMinWidowLines(DependencyObject element)
		{
			Paragraph paragraph = element as Paragraph;
			if (paragraph == null)
			{
				return 0;
			}
			return paragraph.MinWidowLines;
		}

		// Token: 0x0600661A RID: 26138 RVA: 0x001CB69C File Offset: 0x001C989C
		internal static int GetMinOrphanLines(DependencyObject element)
		{
			Paragraph paragraph = element as Paragraph;
			if (paragraph == null)
			{
				return 0;
			}
			return paragraph.MinOrphanLines;
		}

		// Token: 0x0600661B RID: 26139 RVA: 0x001CB6BC File Offset: 0x001C98BC
		internal static double GetLineHeightValue(DependencyObject d)
		{
			double num = (double)d.GetValue(Block.LineHeightProperty);
			if (DoubleUtil.IsNaN(num))
			{
				FontFamily fontFamily = (FontFamily)d.GetValue(TextElement.FontFamilyProperty);
				double num2 = (double)d.GetValue(TextElement.FontSizeProperty);
				num = fontFamily.LineSpacing * num2;
			}
			return Math.Max(TextDpi.MinWidth, Math.Min(TextDpi.MaxWidth, num));
		}

		// Token: 0x0600661C RID: 26140 RVA: 0x001CB724 File Offset: 0x001C9924
		internal static Brush GetBackgroundBrush(DependencyObject element)
		{
			Brush brush = null;
			while (brush == null && DynamicPropertyReader.CanApplyBackgroundBrush(element))
			{
				brush = (Brush)element.GetValue(TextElement.BackgroundProperty);
				Invariant.Assert(element is FrameworkContentElement);
				element = ((FrameworkContentElement)element).Parent;
			}
			return brush;
		}

		// Token: 0x0600661D RID: 26141 RVA: 0x001CB770 File Offset: 0x001C9970
		internal static Brush GetBackgroundBrushForInlineObject(StaticTextPointer position)
		{
			object highlightValue = position.TextContainer.Highlights.GetHighlightValue(position, LogicalDirection.Forward, typeof(TextSelection));
			Brush result;
			if (highlightValue == DependencyProperty.UnsetValue)
			{
				result = (Brush)position.GetValue(TextElement.BackgroundProperty);
			}
			else
			{
				result = SelectionHighlightInfo.BackgroundBrush;
			}
			return result;
		}

		// Token: 0x0600661E RID: 26142 RVA: 0x001CB7C0 File Offset: 0x001C99C0
		internal static BaselineAlignment GetBaselineAlignment(DependencyObject element)
		{
			Inline inline = element as Inline;
			BaselineAlignment result = (inline != null) ? inline.BaselineAlignment : BaselineAlignment.Baseline;
			while (inline != null && DynamicPropertyReader.BaselineAlignmentIsDefault(inline))
			{
				inline = (inline.Parent as Inline);
			}
			if (inline != null)
			{
				result = inline.BaselineAlignment;
			}
			return result;
		}

		// Token: 0x0600661F RID: 26143 RVA: 0x001CB805 File Offset: 0x001C9A05
		internal static BaselineAlignment GetBaselineAlignmentForInlineObject(DependencyObject element)
		{
			return DynamicPropertyReader.GetBaselineAlignment(LogicalTreeHelper.GetParent(element));
		}

		// Token: 0x06006620 RID: 26144 RVA: 0x001CB814 File Offset: 0x001C9A14
		internal static CultureInfo GetCultureInfo(DependencyObject element)
		{
			XmlLanguage xmlLanguage = (XmlLanguage)element.GetValue(FrameworkElement.LanguageProperty);
			CultureInfo result;
			try
			{
				result = xmlLanguage.GetSpecificCulture();
			}
			catch (InvalidOperationException)
			{
				result = TypeConverterHelper.InvariantEnglishUS;
			}
			return result;
		}

		// Token: 0x06006621 RID: 26145 RVA: 0x001CB858 File Offset: 0x001C9A58
		internal static NumberSubstitution GetNumberSubstitution(DependencyObject element)
		{
			return new NumberSubstitution
			{
				CultureSource = (NumberCultureSource)element.GetValue(NumberSubstitution.CultureSourceProperty),
				CultureOverride = (CultureInfo)element.GetValue(NumberSubstitution.CultureOverrideProperty),
				Substitution = (NumberSubstitutionMethod)element.GetValue(NumberSubstitution.SubstitutionProperty)
			};
		}

		// Token: 0x06006622 RID: 26146 RVA: 0x001CB8AE File Offset: 0x001C9AAE
		private static bool CanApplyBackgroundBrush(DependencyObject element)
		{
			return element is Inline && !(element is AnchoredBlock);
		}

		// Token: 0x06006623 RID: 26147 RVA: 0x001CB8C4 File Offset: 0x001C9AC4
		private static bool BaselineAlignmentIsDefault(DependencyObject element)
		{
			Invariant.Assert(element != null);
			bool flag;
			return element.GetValueSource(Inline.BaselineAlignmentProperty, null, out flag) == BaseValueSourceInternal.Default && !flag;
		}
	}
}
