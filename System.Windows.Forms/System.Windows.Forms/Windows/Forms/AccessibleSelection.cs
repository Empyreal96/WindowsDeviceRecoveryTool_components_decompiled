using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies how an accessible object is selected or receives focus.</summary>
	// Token: 0x0200010B RID: 267
	[Flags]
	public enum AccessibleSelection
	{
		/// <summary>The selection or focus of an object is unchanged.</summary>
		// Token: 0x040004E8 RID: 1256
		None = 0,
		/// <summary>Assigns focus to an object and makes it the anchor, which is the starting point for the selection. Can be combined with <see langword="TakeSelection" />, <see langword="ExtendSelection" />, <see langword="AddSelection" />, or <see langword="RemoveSelection" />.</summary>
		// Token: 0x040004E9 RID: 1257
		TakeFocus = 1,
		/// <summary>Selects the object and deselects all other objects in the container.</summary>
		// Token: 0x040004EA RID: 1258
		TakeSelection = 2,
		/// <summary>Selects all objects between the anchor and the selected object.</summary>
		// Token: 0x040004EB RID: 1259
		ExtendSelection = 4,
		/// <summary>Adds the object to the selection.</summary>
		// Token: 0x040004EC RID: 1260
		AddSelection = 8,
		/// <summary>Removes the object from the selection.</summary>
		// Token: 0x040004ED RID: 1261
		RemoveSelection = 16
	}
}
