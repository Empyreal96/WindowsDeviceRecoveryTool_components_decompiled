using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies values for navigating among accessible objects.</summary>
	// Token: 0x02000107 RID: 263
	public enum AccessibleNavigation
	{
		/// <summary>Navigation to a sibling object located below the starting object.</summary>
		// Token: 0x0400047D RID: 1149
		Down = 2,
		/// <summary>Navigation to the first child of the object.</summary>
		// Token: 0x0400047E RID: 1150
		FirstChild = 7,
		/// <summary>Navigation to the last child of the object.</summary>
		// Token: 0x0400047F RID: 1151
		LastChild,
		/// <summary>Navigation to the sibling object located to the left of the starting object.</summary>
		// Token: 0x04000480 RID: 1152
		Left = 3,
		/// <summary>Navigation to the next logical object, typically from a sibling object to the starting object.</summary>
		// Token: 0x04000481 RID: 1153
		Next = 5,
		/// <summary>Navigation to the previous logical object, typically from a sibling object to the starting object.</summary>
		// Token: 0x04000482 RID: 1154
		Previous,
		/// <summary>Navigation to the sibling object located to the right of the starting object.</summary>
		// Token: 0x04000483 RID: 1155
		Right = 4,
		/// <summary>Navigation to a sibling object located above the starting object.</summary>
		// Token: 0x04000484 RID: 1156
		Up = 1
	}
}
