using System;

namespace System.Drawing
{
	/// <summary>Specifies the unit of measure for the given data.</summary>
	// Token: 0x0200004C RID: 76
	public enum GraphicsUnit
	{
		/// <summary>Specifies the world coordinate system unit as the unit of measure.</summary>
		// Token: 0x0400059E RID: 1438
		World,
		/// <summary>Specifies the unit of measure of the display device. Typically pixels for video displays, and 1/100 inch for printers.</summary>
		// Token: 0x0400059F RID: 1439
		Display,
		/// <summary>Specifies a device pixel as the unit of measure.</summary>
		// Token: 0x040005A0 RID: 1440
		Pixel,
		/// <summary>Specifies a printer's point (1/72 inch) as the unit of measure.</summary>
		// Token: 0x040005A1 RID: 1441
		Point,
		/// <summary>Specifies the inch as the unit of measure.</summary>
		// Token: 0x040005A2 RID: 1442
		Inch,
		/// <summary>Specifies the document unit (1/300 inch) as the unit of measure.</summary>
		// Token: 0x040005A3 RID: 1443
		Document,
		/// <summary>Specifies the millimeter as the unit of measure.</summary>
		// Token: 0x040005A4 RID: 1444
		Millimeter
	}
}
