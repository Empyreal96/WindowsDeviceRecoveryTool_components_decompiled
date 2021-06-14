using System;

namespace System.Windows.Markup.Localizer
{
	/// <summary>Resolves localizable settings for classes and properties in binary XAML (BAML). </summary>
	// Token: 0x0200028C RID: 652
	public abstract class BamlLocalizabilityResolver
	{
		/// <summary>Returns a value that indicates whether a specified type of element can be localized and, if so, whether it can be formatted inline. </summary>
		/// <param name="assembly">The full name of the assembly that contains BAML to be localized.</param>
		/// <param name="className">The full class name of the element that you want to retrieve localizability information for.</param>
		/// <returns>An object that contains the localizability information for the specified assembly and element.</returns>
		// Token: 0x060024C1 RID: 9409
		public abstract ElementLocalizability GetElementLocalizability(string assembly, string className);

		/// <summary>Returns a value that indicates whether a specified property of a specified type of element can be localized. </summary>
		/// <param name="assembly">The full name of the assembly that contains BAML to be localized.</param>
		/// <param name="className">The full class name of the element that you want to retrieve localizability information for.</param>
		/// <param name="property">The name of the property that you want to retrieve localizability information for.</param>
		/// <returns>An object that specifies whether and how the property can be localized.</returns>
		// Token: 0x060024C2 RID: 9410
		public abstract LocalizabilityAttribute GetPropertyLocalizability(string assembly, string className, string property);

		/// <summary>Returns the full class name of a XAML tag that has not been encountered in BAML.</summary>
		/// <param name="formattingTag">The name of the tag.</param>
		/// <returns>The full class name associated with the tag.</returns>
		// Token: 0x060024C3 RID: 9411
		public abstract string ResolveFormattingTagToClass(string formattingTag);

		/// <summary>Returns the full name of the assembly that contains the specified class.</summary>
		/// <param name="className">The full class name.</param>
		/// <returns>The full name of the assembly that contains the class.</returns>
		// Token: 0x060024C4 RID: 9412
		public abstract string ResolveAssemblyFromClass(string className);
	}
}
