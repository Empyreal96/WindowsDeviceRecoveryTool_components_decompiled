using System;

namespace System.Management
{
	/// <summary>Describes the possible CIM types for properties, qualifiers, or method parameters.          </summary>
	// Token: 0x02000006 RID: 6
	public enum CimType
	{
		/// <summary>A null value.</summary>
		// Token: 0x04000055 RID: 85
		None,
		/// <summary>A signed 8-bit integer. This value maps to the <see cref="T:System.SByte" /> type.</summary>
		// Token: 0x04000056 RID: 86
		SInt8 = 16,
		/// <summary>An unsigned 8-bit integer. This value maps to the <see cref="T:System.Byte" /> type.</summary>
		// Token: 0x04000057 RID: 87
		UInt8,
		/// <summary>A signed 16-bit integer. This value maps to the <see cref="T:System.Int16" /> type.</summary>
		// Token: 0x04000058 RID: 88
		SInt16 = 2,
		/// <summary>An unsigned 16-bit integer. This value maps to the <see cref="T:System.UInt16" /> type.</summary>
		// Token: 0x04000059 RID: 89
		UInt16 = 18,
		/// <summary>A signed 32-bit integer. This value maps to the <see cref="T:System.Int32" /> type.</summary>
		// Token: 0x0400005A RID: 90
		SInt32 = 3,
		/// <summary>An unsigned 32-bit integer. This value maps to the <see cref="T:System.UInt32" /> type.</summary>
		// Token: 0x0400005B RID: 91
		UInt32 = 19,
		/// <summary>A signed 64-bit integer. This value maps to the <see cref="T:System.Int64" /> type.</summary>
		// Token: 0x0400005C RID: 92
		SInt64,
		/// <summary>An unsigned 64-bit integer. This value maps to the <see cref="T:System.UInt64" /> type.</summary>
		// Token: 0x0400005D RID: 93
		UInt64,
		/// <summary>A floating-point 32-bit number. This value maps to the <see cref="T:System.Single" /> type.</summary>
		// Token: 0x0400005E RID: 94
		Real32 = 4,
		/// <summary>A floating point 64-bit number. This value maps to the <see cref="T:System.Double" /> type.</summary>
		// Token: 0x0400005F RID: 95
		Real64,
		/// <summary>A Boolean. This value maps to the <see cref="T:System.Boolean" /> type.</summary>
		// Token: 0x04000060 RID: 96
		Boolean = 11,
		/// <summary>A string. This value maps to the <see cref="T:System.String" /> type.</summary>
		// Token: 0x04000061 RID: 97
		String = 8,
		/// <summary>A date or time value, represented in a string in DMTF date/time format: yyyymmddHHMMSS.mmmmmmsUUU, where yyyymmdd is the date in year/month/day; HHMMSS is the time in hours/minutes/seconds; mmmmmm is the number of microseconds in 6 digits; and sUUU is a sign (+ or -) and a 3-digit UTC offset. This value maps to the <see cref="T:System.DateTime" /> type.</summary>
		// Token: 0x04000062 RID: 98
		DateTime = 101,
		/// <summary>A reference to another object. This is represented by a string containing the path to the referenced object. This value maps to the <see cref="T:System.Int16" /> type.</summary>
		// Token: 0x04000063 RID: 99
		Reference,
		/// <summary>A 16-bit character. This value maps to the <see cref="T:System.Char" /> type.</summary>
		// Token: 0x04000064 RID: 100
		Char16,
		/// <summary>An embedded object. Note that embedded objects differ from references in that the embedded object does not have a path and its lifetime is identical to the lifetime of the containing object. This value maps to the <see cref="T:System.Object" /> type.</summary>
		// Token: 0x04000065 RID: 101
		Object = 13
	}
}
