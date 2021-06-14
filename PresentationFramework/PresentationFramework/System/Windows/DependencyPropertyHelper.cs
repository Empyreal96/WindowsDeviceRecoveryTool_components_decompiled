using System;
using MS.Internal;

namespace System.Windows
{
	/// <summary>Provides a single helper method (<see cref="M:System.Windows.DependencyPropertyHelper.GetValueSource(System.Windows.DependencyObject,System.Windows.DependencyProperty)" />) that reports the property system source for the effective value of a dependency property.</summary>
	// Token: 0x020000B0 RID: 176
	public static class DependencyPropertyHelper
	{
		/// <summary>Returns a structure that reports various metadata and property system characteristics of a specified dependency property on a particular <see cref="T:System.Windows.DependencyObject" />.</summary>
		/// <param name="dependencyObject">The element that contains the <paramref name="dependencyProperty" /> to report information for.</param>
		/// <param name="dependencyProperty">The identifier for the dependency property to report information for.</param>
		/// <returns>A <see cref="T:System.Windows.ValueSource" /> structure that reports the specific information.</returns>
		// Token: 0x060003B9 RID: 953 RVA: 0x0000A968 File Offset: 0x00008B68
		public static ValueSource GetValueSource(DependencyObject dependencyObject, DependencyProperty dependencyProperty)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			if (dependencyProperty == null)
			{
				throw new ArgumentNullException("dependencyProperty");
			}
			dependencyObject.VerifyAccess();
			bool flag;
			bool isExpression;
			bool isAnimated;
			bool isCoerced;
			bool isCurrent;
			BaseValueSourceInternal valueSource = dependencyObject.GetValueSource(dependencyProperty, null, out flag, out isExpression, out isAnimated, out isCoerced, out isCurrent);
			return new ValueSource(valueSource, isExpression, isAnimated, isCoerced, isCurrent);
		}

		/// <summary>Indicates whether a specified element belongs to an instance of a template that defines a value for the specified property that may change at runtime based on changes elsewhere. </summary>
		/// <param name="elementInTemplate">An element that belongs to a template instance. </param>
		/// <param name="dependencyProperty">A dependency property. </param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="elementInTemplate" /> belongs to an instance of a template that defines a value for the specified property that may change at runtime based on changes elsewhere; otherwise, <see langword="false" />. </returns>
		// Token: 0x060003BA RID: 954 RVA: 0x0000A9B8 File Offset: 0x00008BB8
		public static bool IsTemplatedValueDynamic(DependencyObject elementInTemplate, DependencyProperty dependencyProperty)
		{
			if (elementInTemplate == null)
			{
				throw new ArgumentNullException("elementInTemplate");
			}
			if (dependencyProperty == null)
			{
				throw new ArgumentNullException("dependencyProperty");
			}
			FrameworkObject frameworkObject = new FrameworkObject(elementInTemplate);
			DependencyObject templatedParent = frameworkObject.TemplatedParent;
			if (templatedParent == null)
			{
				throw new ArgumentException(SR.Get("ElementMustBelongToTemplate"), "elementInTemplate");
			}
			int templateChildIndex = frameworkObject.TemplateChildIndex;
			return StyleHelper.IsValueDynamic(templatedParent, templateChildIndex, dependencyProperty);
		}
	}
}
