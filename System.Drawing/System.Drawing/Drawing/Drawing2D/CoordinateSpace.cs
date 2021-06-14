using System;

namespace System.Drawing.Drawing2D
{
	/// <summary>Specifies the system to use when evaluating coordinates.</summary>
	// Token: 0x020000B9 RID: 185
	public enum CoordinateSpace
	{
		/// <summary>Specifies that coordinates are in the world coordinate context. World coordinates are used in a nonphysical environment, such as a modeling environment.</summary>
		// Token: 0x04000979 RID: 2425
		World,
		/// <summary>Specifies that coordinates are in the page coordinate context. Their units are defined by the <see cref="P:System.Drawing.Graphics.PageUnit" /> property, and must be one of the elements of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration.</summary>
		// Token: 0x0400097A RID: 2426
		Page,
		/// <summary>Specifies that coordinates are in the device coordinate context. On a computer screen the device coordinates are usually measured in pixels.</summary>
		// Token: 0x0400097B RID: 2427
		Device
	}
}
