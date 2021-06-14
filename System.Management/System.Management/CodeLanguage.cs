using System;

namespace System.Management
{
	/// <summary>Defines the languages supported by the code generator.          </summary>
	// Token: 0x02000050 RID: 80
	public enum CodeLanguage
	{
		/// <summary>A value for generating C# code.             </summary>
		// Token: 0x040001F5 RID: 501
		CSharp,
		/// <summary>A value for generating JScript code.</summary>
		// Token: 0x040001F6 RID: 502
		JScript,
		/// <summary>A value for generating Visual Basic code.</summary>
		// Token: 0x040001F7 RID: 503
		VB,
		/// <summary>A value for generating Visual J# code.</summary>
		// Token: 0x040001F8 RID: 504
		VJSharp,
		/// <summary>A value for generating managed C++ code.             </summary>
		// Token: 0x040001F9 RID: 505
		Mcpp
	}
}
