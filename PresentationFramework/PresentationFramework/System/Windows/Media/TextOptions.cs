using System;
using MS.Internal.Media;

namespace System.Windows.Media
{
	/// <summary>Defines a set of attached properties that affect the way text is displayed in an element.</summary>
	// Token: 0x0200017F RID: 383
	public static class TextOptions
	{
		// Token: 0x0600167C RID: 5756 RVA: 0x000703B4 File Offset: 0x0006E5B4
		internal static bool IsTextFormattingModeValid(object valueObject)
		{
			TextFormattingMode textFormattingMode = (TextFormattingMode)valueObject;
			return textFormattingMode == TextFormattingMode.Ideal || textFormattingMode == TextFormattingMode.Display;
		}

		/// <summary>Sets the <see cref="T:System.Windows.Media.TextFormattingMode" /> for the specified element. </summary>
		/// <param name="element">The element to set the <see cref="T:System.Windows.Media.TextFormattingMode" /> for.</param>
		/// <param name="value">The <see cref="T:System.Windows.Media.TextFormattingMode" /> to set on <paramref name="element" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />. </exception>
		// Token: 0x0600167D RID: 5757 RVA: 0x000703D1 File Offset: 0x0006E5D1
		public static void SetTextFormattingMode(DependencyObject element, TextFormattingMode value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextOptions.TextFormattingModeProperty, value);
		}

		/// <summary>Gets the <see cref="T:System.Windows.Media.TextFormattingMode" /> for the specified element.</summary>
		/// <param name="element">The element to get the <see cref="T:System.Windows.Media.TextFormattingMode" /> for.</param>
		/// <returns>The <see cref="T:System.Windows.Media.TextFormattingMode" /> for <paramref name="element" />.</returns>
		// Token: 0x0600167E RID: 5758 RVA: 0x000703F2 File Offset: 0x0006E5F2
		public static TextFormattingMode GetTextFormattingMode(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (TextFormattingMode)element.GetValue(TextOptions.TextFormattingModeProperty);
		}

		/// <summary>Sets the <see cref="T:System.Windows.Media.TextRenderingMode" /> for the specified element.</summary>
		/// <param name="element">The element to set the <see cref="T:System.Windows.Media.TextRenderingMode" /> for.</param>
		/// <param name="value">The <see cref="T:System.Windows.Media.TextRenderingMode" /> to set on <paramref name="element" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />. </exception>
		// Token: 0x0600167F RID: 5759 RVA: 0x00070412 File Offset: 0x0006E612
		public static void SetTextRenderingMode(DependencyObject element, TextRenderingMode value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextOptions.TextRenderingModeProperty, value);
		}

		/// <summary>Gets the <see cref="T:System.Windows.Media.TextRenderingMode" /> for the specified element.</summary>
		/// <param name="element">The element to get the <see cref="T:System.Windows.Media.TextRenderingMode" /> for.</param>
		/// <returns>The <see cref="T:System.Windows.Media.TextRenderingMode" /> for <paramref name="element" />.</returns>
		// Token: 0x06001680 RID: 5760 RVA: 0x00070433 File Offset: 0x0006E633
		public static TextRenderingMode GetTextRenderingMode(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (TextRenderingMode)element.GetValue(TextOptions.TextRenderingModeProperty);
		}

		/// <summary>Sets the <see cref="T:System.Windows.Media.TextHintingMode" /> for the specified element.</summary>
		/// <param name="element">The element to set the <see cref="T:System.Windows.Media.TextHintingMode" /> for.</param>
		/// <param name="value">The <see cref="T:System.Windows.Media.TextHintingMode" /> to set on <paramref name="element" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />. </exception>
		// Token: 0x06001681 RID: 5761 RVA: 0x00070453 File Offset: 0x0006E653
		public static void SetTextHintingMode(DependencyObject element, TextHintingMode value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextOptions.TextHintingModeProperty, value);
		}

		/// <summary>Gets the <see cref="T:System.Windows.Media.TextHintingMode" /> for the specified element.</summary>
		/// <param name="element">The element to get the <see cref="T:System.Windows.Media.TextHintingMode" />  for.</param>
		/// <returns>The <see cref="T:System.Windows.Media.TextHintingMode" /> for <paramref name="element" />.</returns>
		// Token: 0x06001682 RID: 5762 RVA: 0x00070474 File Offset: 0x0006E674
		public static TextHintingMode GetTextHintingMode(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (TextHintingMode)element.GetValue(TextOptions.TextHintingModeProperty);
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Media.TextOptions.TextFormattingMode" /> attached property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Media.TextOptions.TextFormattingMode" /> attached property.</returns>
		// Token: 0x040012A0 RID: 4768
		public static readonly DependencyProperty TextFormattingModeProperty = DependencyProperty.RegisterAttached("TextFormattingMode", typeof(TextFormattingMode), typeof(TextOptions), new FrameworkPropertyMetadata(TextFormattingMode.Ideal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits), new ValidateValueCallback(TextOptions.IsTextFormattingModeValid));

		/// <summary>Identifies the <see cref="P:System.Windows.Media.TextOptions.TextRenderingMode" /> attached property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Media.TextOptions.TextRenderingMode" /> attached property.</returns>
		// Token: 0x040012A1 RID: 4769
		public static readonly DependencyProperty TextRenderingModeProperty = DependencyProperty.RegisterAttached("TextRenderingMode", typeof(TextRenderingMode), typeof(TextOptions), new FrameworkPropertyMetadata(TextRenderingMode.Auto, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits), new ValidateValueCallback(ValidateEnums.IsTextRenderingModeValid));

		/// <summary>Identifies the <see cref="P:System.Windows.Media.TextOptions.TextHintingMode" /> attached property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Media.TextOptions.TextHintingMode" /> attached property.</returns>
		// Token: 0x040012A2 RID: 4770
		public static readonly DependencyProperty TextHintingModeProperty = TextOptionsInternal.TextHintingModeProperty.AddOwner(typeof(TextOptions));
	}
}
