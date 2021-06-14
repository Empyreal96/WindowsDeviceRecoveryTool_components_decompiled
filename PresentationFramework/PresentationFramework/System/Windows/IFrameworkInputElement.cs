using System;

namespace System.Windows
{
	/// <summary>Declares a namescope contract for framework elements.</summary>
	// Token: 0x020000D2 RID: 210
	public interface IFrameworkInputElement : IInputElement
	{
		/// <summary>Gets or sets the name of an element. </summary>
		/// <returns>The element name, which is unique in the namescope and can be used as an identifier for certain operations.</returns>
		// Token: 0x17000184 RID: 388
		// (get) Token: 0x0600074B RID: 1867
		// (set) Token: 0x0600074C RID: 1868
		string Name { get; set; }
	}
}
