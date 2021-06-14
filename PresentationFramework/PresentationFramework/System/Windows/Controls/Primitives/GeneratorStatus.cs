using System;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Used by <see cref="T:System.Windows.Controls.ItemContainerGenerator" /> to indicate the status of its item generation.</summary>
	// Token: 0x02000591 RID: 1425
	public enum GeneratorStatus
	{
		/// <summary>The generator has not tried to generate content.</summary>
		// Token: 0x0400304E RID: 12366
		NotStarted,
		/// <summary>The generator is generating containers.</summary>
		// Token: 0x0400304F RID: 12367
		GeneratingContainers,
		/// <summary>The generator has finished generating containers.</summary>
		// Token: 0x04003050 RID: 12368
		ContainersGenerated,
		/// <summary>The generator has finished generating containers, but encountered one or more errors.</summary>
		// Token: 0x04003051 RID: 12369
		Error
	}
}
