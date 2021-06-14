using System;
using System.Runtime.CompilerServices;
using MS.Internal.Globalization;

namespace System.Windows
{
	/// <summary>The <see cref="T:System.Windows.Localization" /> class defines attached properties for localization attributes and comments.</summary>
	// Token: 0x020000D5 RID: 213
	public static class Localization
	{
		/// <summary>Gets the value of the <see cref="F:System.Windows.Localization.CommentsProperty" /> attached property from a specified element.</summary>
		/// <param name="element">A <see cref="T:System.Object" /> that represents the element whose attached property you want to retrieve.</param>
		/// <returns>A <see cref="T:System.String" /> value that represents the localization comment.</returns>
		// Token: 0x0600075C RID: 1884 RVA: 0x00016F76 File Offset: 0x00015176
		[AttachedPropertyBrowsableForType(typeof(object))]
		public static string GetComments(object element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return Localization.GetValue(element, Localization.CommentsProperty);
		}

		/// <summary>Sets the <see cref="F:System.Windows.Localization.CommentsProperty" /> attached property to the specified element.</summary>
		/// <param name="element">A <see cref="T:System.Object" /> that represents the element whose attached property you want to set.</param>
		/// <param name="comments">A <see cref="T:System.String" /> that specifies the localization comments.</param>
		// Token: 0x0600075D RID: 1885 RVA: 0x00016F91 File Offset: 0x00015191
		public static void SetComments(object element, string comments)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			LocComments.ParsePropertyComments(comments);
			Localization.SetValue(element, Localization.CommentsProperty, comments);
		}

		/// <summary>Gets the value of the <see cref="F:System.Windows.Localization.AttributesProperty" /> attached property from a specified element.</summary>
		/// <param name="element">A <see cref="T:System.Object" /> that represents the element whose attached property you want to retrieve.</param>
		/// <returns>A <see cref="T:System.String" /> value that represents the localization attribute.</returns>
		// Token: 0x0600075E RID: 1886 RVA: 0x00016FB4 File Offset: 0x000151B4
		[AttachedPropertyBrowsableForType(typeof(object))]
		public static string GetAttributes(object element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return Localization.GetValue(element, Localization.AttributesProperty);
		}

		/// <summary>Sets the <see cref="F:System.Windows.Localization.AttributesProperty" /> attached property for the specified element.</summary>
		/// <param name="element">A <see cref="T:System.Object" /> that represents the element whose attached property you want to set.</param>
		/// <param name="attributes">A <see cref="T:System.String" /> that specifies the localization attributes.</param>
		// Token: 0x0600075F RID: 1887 RVA: 0x00016FCF File Offset: 0x000151CF
		public static void SetAttributes(object element, string attributes)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			LocComments.ParsePropertyLocalizabilityAttributes(attributes);
			Localization.SetValue(element, Localization.AttributesProperty, attributes);
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x00016FF4 File Offset: 0x000151F4
		private static string GetValue(object element, DependencyProperty property)
		{
			DependencyObject dependencyObject = element as DependencyObject;
			if (dependencyObject != null)
			{
				return (string)dependencyObject.GetValue(property);
			}
			string result;
			if (property == Localization.CommentsProperty)
			{
				Localization._commentsOnObjects.TryGetValue(element, out result);
			}
			else
			{
				Localization._attributesOnObjects.TryGetValue(element, out result);
			}
			return result;
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x00017040 File Offset: 0x00015240
		private static void SetValue(object element, DependencyProperty property, string value)
		{
			DependencyObject dependencyObject = element as DependencyObject;
			if (dependencyObject != null)
			{
				dependencyObject.SetValue(property, value);
				return;
			}
			if (property == Localization.CommentsProperty)
			{
				Localization._commentsOnObjects.Remove(element);
				Localization._commentsOnObjects.Add(element, value);
				return;
			}
			Localization._attributesOnObjects.Remove(element);
			Localization._attributesOnObjects.Add(element, value);
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Localization.Comments" /> attached property. </summary>
		/// <returns>The <see cref="P:System.Windows.Localization.Comments" /> attached property identifier.</returns>
		// Token: 0x04000737 RID: 1847
		public static readonly DependencyProperty CommentsProperty = DependencyProperty.RegisterAttached("Comments", typeof(string), typeof(Localization));

		/// <summary>Identifies the <see cref="P:System.Windows.Localization.Attributes" /> attached property. </summary>
		/// <returns>The <see cref="P:System.Windows.Localization.Attributes" /> attached property identifier.</returns>
		// Token: 0x04000738 RID: 1848
		public static readonly DependencyProperty AttributesProperty = DependencyProperty.RegisterAttached("Attributes", typeof(string), typeof(Localization));

		// Token: 0x04000739 RID: 1849
		private static ConditionalWeakTable<object, string> _commentsOnObjects = new ConditionalWeakTable<object, string>();

		// Token: 0x0400073A RID: 1850
		private static ConditionalWeakTable<object, string> _attributesOnObjects = new ConditionalWeakTable<object, string>();
	}
}
