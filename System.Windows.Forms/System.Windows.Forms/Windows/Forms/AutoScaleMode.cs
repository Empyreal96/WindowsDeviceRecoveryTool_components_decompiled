using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the different types of automatic scaling modes supported by Windows Forms.</summary>
	// Token: 0x0200011B RID: 283
	public enum AutoScaleMode
	{
		/// <summary>Automatic scaling is disabled.</summary>
		// Token: 0x0400056A RID: 1386
		None,
		/// <summary>Controls scale relative to the dimensions of the font the classes are using, which is typically the system font.</summary>
		// Token: 0x0400056B RID: 1387
		Font,
		/// <summary>Controls scale relative to the display resolution. Common resolutions are 96 and 120 DPI.</summary>
		// Token: 0x0400056C RID: 1388
		Dpi,
		/// <summary>Controls scale according to the classes' parent's scaling mode. If there is no parent, automatic scaling is disabled.</summary>
		// Token: 0x0400056D RID: 1389
		Inherit
	}
}
