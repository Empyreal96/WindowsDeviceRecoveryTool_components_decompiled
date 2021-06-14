using System;

namespace System.Drawing.Design
{
	/// <summary>Exposes a collection of toolbox items.</summary>
	// Token: 0x02000074 RID: 116
	public interface IToolboxItemProvider
	{
		/// <summary>Gets a collection of <see cref="T:System.Drawing.Design.ToolboxItem" /> objects.</summary>
		/// <returns>A collection of <see cref="T:System.Drawing.Design.ToolboxItem" /> objects.</returns>
		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000836 RID: 2102
		ToolboxItemCollection Items { get; }
	}
}
