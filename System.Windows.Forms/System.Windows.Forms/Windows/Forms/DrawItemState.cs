using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the state of an item that is being drawn.</summary>
	// Token: 0x0200022B RID: 555
	[Flags]
	public enum DrawItemState
	{
		/// <summary>The item is checked. Only menu controls use this value.</summary>
		// Token: 0x04000E74 RID: 3700
		Checked = 8,
		/// <summary>The item is the editing portion of a <see cref="T:System.Windows.Forms.ComboBox" />.</summary>
		// Token: 0x04000E75 RID: 3701
		ComboBoxEdit = 4096,
		/// <summary>The item is in its default visual state.</summary>
		// Token: 0x04000E76 RID: 3702
		Default = 32,
		/// <summary>The item is unavailable.</summary>
		// Token: 0x04000E77 RID: 3703
		Disabled = 4,
		/// <summary>The item has focus.</summary>
		// Token: 0x04000E78 RID: 3704
		Focus = 16,
		/// <summary>The item is grayed. Only menu controls use this value.</summary>
		// Token: 0x04000E79 RID: 3705
		Grayed = 2,
		/// <summary>The item is being hot-tracked, that is, the item is highlighted as the mouse pointer passes over it.</summary>
		// Token: 0x04000E7A RID: 3706
		HotLight = 64,
		/// <summary>The item is inactive.</summary>
		// Token: 0x04000E7B RID: 3707
		Inactive = 128,
		/// <summary>The item displays without a keyboard accelerator.</summary>
		// Token: 0x04000E7C RID: 3708
		NoAccelerator = 256,
		/// <summary>The item displays without the visual cue that indicates it has focus.</summary>
		// Token: 0x04000E7D RID: 3709
		NoFocusRect = 512,
		/// <summary>The item is selected.</summary>
		// Token: 0x04000E7E RID: 3710
		Selected = 1,
		/// <summary>The item currently has no state.</summary>
		// Token: 0x04000E7F RID: 3711
		None = 0
	}
}
