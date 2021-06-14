using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Specifies how properties are sorted in the <see cref="T:System.Windows.Forms.PropertyGrid" />.</summary>
	// Token: 0x02000320 RID: 800
	[ComVisible(true)]
	public enum PropertySort
	{
		/// <summary>Properties are displayed in the order in which they are retrieved from the <see cref="T:System.ComponentModel.TypeDescriptor" />.</summary>
		// Token: 0x04001E24 RID: 7716
		NoSort,
		/// <summary>Properties are sorted in an alphabetical list.</summary>
		// Token: 0x04001E25 RID: 7717
		Alphabetical,
		/// <summary>Properties are displayed according to their category in a group. The categories are defined by the properties themselves.</summary>
		// Token: 0x04001E26 RID: 7718
		Categorized,
		/// <summary>Properties are displayed according to their category in a group. The properties are further sorted alphabetically within the group. The categories are defined by the properties themselves.</summary>
		// Token: 0x04001E27 RID: 7719
		CategorizedAlphabetical
	}
}
