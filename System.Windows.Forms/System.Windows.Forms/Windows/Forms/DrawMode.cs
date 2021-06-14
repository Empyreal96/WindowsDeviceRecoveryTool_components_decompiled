using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies how the elements of a control are drawn.</summary>
	// Token: 0x02000232 RID: 562
	public enum DrawMode
	{
		/// <summary>All the elements in a control are drawn by the operating system and are of the same size.</summary>
		// Token: 0x04000E99 RID: 3737
		Normal,
		/// <summary>All the elements in the control are drawn manually and are of the same size.</summary>
		// Token: 0x04000E9A RID: 3738
		OwnerDrawFixed,
		/// <summary>All the elements in the control are drawn manually and can differ in size.</summary>
		// Token: 0x04000E9B RID: 3739
		OwnerDrawVariable
	}
}
