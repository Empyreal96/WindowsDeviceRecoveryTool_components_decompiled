using System;

namespace System.Drawing
{
	/// <summary>Specifies the units of measure for a text string.</summary>
	// Token: 0x0200004A RID: 74
	public enum StringUnit
	{
		/// <summary>Specifies world units as the unit of measure.</summary>
		// Token: 0x04000595 RID: 1429
		World,
		/// <summary>Specifies the device unit as the unit of measure.</summary>
		// Token: 0x04000596 RID: 1430
		Display,
		/// <summary>Specifies a pixel as the unit of measure.</summary>
		// Token: 0x04000597 RID: 1431
		Pixel,
		/// <summary>Specifies a printer's point (1/72 inch) as the unit of measure.</summary>
		// Token: 0x04000598 RID: 1432
		Point,
		/// <summary>Specifies an inch as the unit of measure.</summary>
		// Token: 0x04000599 RID: 1433
		Inch,
		/// <summary>Specifies 1/300 of an inch as the unit of measure.</summary>
		// Token: 0x0400059A RID: 1434
		Document,
		/// <summary>Specifies a millimeter as the unit of measure </summary>
		// Token: 0x0400059B RID: 1435
		Millimeter,
		/// <summary>Specifies a printer's em size of 32 as the unit of measure.</summary>
		// Token: 0x0400059C RID: 1436
		Em = 32
	}
}
