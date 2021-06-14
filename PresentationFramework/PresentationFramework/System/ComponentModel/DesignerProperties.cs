using System;
using System.Windows;
using MS.Internal.KnownBoxes;

namespace System.ComponentModel
{
	/// <summary>Provides attached properties used to communicate with a designer.</summary>
	// Token: 0x02000096 RID: 150
	public static class DesignerProperties
	{
		/// <summary>Gets the value of the <see cref="P:System.ComponentModel.DesignerProperties.IsInDesignMode" /> attached property for the specified <see cref="T:System.Windows.UIElement" />.</summary>
		/// <param name="element">The element from which the property value is read.</param>
		/// <returns>The <see cref="P:System.ComponentModel.DesignerProperties.IsInDesignMode" /> property value for the element.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06000263 RID: 611 RVA: 0x000062AE File Offset: 0x000044AE
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public static bool GetIsInDesignMode(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(DesignerProperties.IsInDesignModeProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.ComponentModel.DesignerProperties.IsInDesignMode" /> attached property to a specified element. </summary>
		/// <param name="element">The element to which the attached property is written.</param>
		/// <param name="value">The needed <see cref="T:System.Boolean" /> value.</param>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06000264 RID: 612 RVA: 0x000062CE File Offset: 0x000044CE
		public static void SetIsInDesignMode(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(DesignerProperties.IsInDesignModeProperty, value);
		}

		/// <summary>Identifies the <see cref="P:System.ComponentModel.DesignerProperties.IsInDesignMode" /> attached property.</summary>
		// Token: 0x04000591 RID: 1425
		public static readonly DependencyProperty IsInDesignModeProperty = DependencyProperty.RegisterAttached("IsInDesignMode", typeof(bool), typeof(DesignerProperties), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.OverridesInheritanceBehavior));
	}
}
