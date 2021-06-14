using System;
using System.ComponentModel;

namespace System.Windows.Forms.PropertyGridInternal
{
	/// <summary>Defines methods and a property that allow filtering on specific attributes.</summary>
	// Token: 0x0200048D RID: 1165
	public interface IRootGridEntry
	{
		/// <summary>Gets or sets the attributes on which the property browser filters.</summary>
		/// <returns>The attributes on which the property browser filters.</returns>
		// Token: 0x1700136A RID: 4970
		// (get) Token: 0x06004E65 RID: 20069
		// (set) Token: 0x06004E66 RID: 20070
		AttributeCollection BrowsableAttributes { get; set; }

		/// <summary>Resets the <see cref="P:System.Windows.Forms.PropertyGridInternal.IRootGridEntry.BrowsableAttributes" /> property to the default value.</summary>
		// Token: 0x06004E67 RID: 20071
		void ResetBrowsableAttributes();

		/// <summary>Sorts the properties in the property browser.</summary>
		/// <param name="showCategories">
		///       <see langword="true" /> to group the properties by category; otherwise, <see langword="false" />.</param>
		// Token: 0x06004E68 RID: 20072
		void ShowCategories(bool showCategories);
	}
}
