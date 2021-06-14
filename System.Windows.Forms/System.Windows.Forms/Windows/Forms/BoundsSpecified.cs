using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the bounds of the control to use when defining a control's size and position.</summary>
	// Token: 0x02000131 RID: 305
	[Flags]
	public enum BoundsSpecified
	{
		/// <summary>The left edge of the control is defined.</summary>
		// Token: 0x0400066B RID: 1643
		X = 1,
		/// <summary>The top edge of the control is defined.</summary>
		// Token: 0x0400066C RID: 1644
		Y = 2,
		/// <summary>The width of the control is defined.</summary>
		// Token: 0x0400066D RID: 1645
		Width = 4,
		/// <summary>The height of the control is defined.</summary>
		// Token: 0x0400066E RID: 1646
		Height = 8,
		/// <summary>Both <see langword="X" /> and <see langword="Y" /> coordinates of the control are defined.</summary>
		// Token: 0x0400066F RID: 1647
		Location = 3,
		/// <summary>Both <see cref="P:System.Windows.Forms.Control.Width" /> and <see cref="P:System.Windows.Forms.Control.Height" /> property values of the control are defined.</summary>
		// Token: 0x04000670 RID: 1648
		Size = 12,
		/// <summary>Both <see cref="P:System.Windows.Forms.Control.Location" /> and <see cref="P:System.Windows.Forms.Control.Size" /> property values are defined.</summary>
		// Token: 0x04000671 RID: 1649
		All = 15,
		/// <summary>No bounds are specified.</summary>
		// Token: 0x04000672 RID: 1650
		None = 0
	}
}
