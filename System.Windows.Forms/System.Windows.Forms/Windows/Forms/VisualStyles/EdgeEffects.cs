using System;

namespace System.Windows.Forms.VisualStyles
{
	/// <summary>Specifies the visual effects that can be applied to the edges of a visual style element.</summary>
	// Token: 0x02000471 RID: 1137
	[Flags]
	public enum EdgeEffects
	{
		/// <summary>The border is drawn without any effects.</summary>
		// Token: 0x0400328E RID: 12942
		None = 0,
		/// <summary>The area within the element borders is filled.</summary>
		// Token: 0x0400328F RID: 12943
		FillInterior = 2048,
		/// <summary>The border is flat.</summary>
		// Token: 0x04003290 RID: 12944
		Flat = 4096,
		/// <summary>The border is soft.</summary>
		// Token: 0x04003291 RID: 12945
		Soft = 16384,
		/// <summary>The border is one-dimensional.</summary>
		// Token: 0x04003292 RID: 12946
		Mono = 32768
	}
}
