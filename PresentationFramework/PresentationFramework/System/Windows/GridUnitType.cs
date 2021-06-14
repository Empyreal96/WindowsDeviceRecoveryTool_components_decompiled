using System;

namespace System.Windows
{
	/// <summary>Describes the kind of value that a <see cref="T:System.Windows.GridLength" /> object is holding. </summary>
	// Token: 0x020000CC RID: 204
	public enum GridUnitType
	{
		/// <summary>The size is determined by the size properties of the content object. </summary>
		// Token: 0x04000714 RID: 1812
		Auto,
		/// <summary>The value is expressed as a pixel. </summary>
		// Token: 0x04000715 RID: 1813
		Pixel,
		/// <summary>The value is expressed as a weighted proportion of available space. </summary>
		// Token: 0x04000716 RID: 1814
		Star
	}
}
