using System;

namespace System.Drawing
{
	/// <summary>The <see cref="T:System.Drawing.StringDigitSubstitute" /> enumeration specifies how to substitute digits in a string according to a user's locale or language.</summary>
	// Token: 0x02000045 RID: 69
	public enum StringDigitSubstitute
	{
		/// <summary>Specifies a user-defined substitution scheme.</summary>
		// Token: 0x0400057C RID: 1404
		User,
		/// <summary>Specifies to disable substitutions.</summary>
		// Token: 0x0400057D RID: 1405
		None,
		/// <summary>Specifies substitution digits that correspond with the official national language of the user's locale.</summary>
		// Token: 0x0400057E RID: 1406
		National,
		/// <summary>Specifies substitution digits that correspond with the user's native script or language, which may be different from the official national language of the user's locale.</summary>
		// Token: 0x0400057F RID: 1407
		Traditional
	}
}
