using System;

namespace System.Windows
{
	/// <summary>Specifies the locations where theme resource dictionaries are located.</summary>
	// Token: 0x020000E8 RID: 232
	public enum ResourceDictionaryLocation
	{
		/// <summary>No theme dictionaries exist.</summary>
		// Token: 0x04000791 RID: 1937
		None,
		/// <summary>Theme dictionaries exist in the assembly that defines the types being themed.</summary>
		// Token: 0x04000792 RID: 1938
		SourceAssembly,
		/// <summary>Theme dictionaries exist in assemblies external to the one defining the types being themed.</summary>
		// Token: 0x04000793 RID: 1939
		ExternalAssembly
	}
}
