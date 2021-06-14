using System;

namespace System.Windows.Controls
{
	/// <summary>Provides a way to apply styles based on custom logic.</summary>
	// Token: 0x02000528 RID: 1320
	public class StyleSelector
	{
		/// <summary>When overridden in a derived class, returns a <see cref="T:System.Windows.Style" /> based on custom logic.</summary>
		/// <param name="item">The content.</param>
		/// <param name="container">The element to which the style will be applied.</param>
		/// <returns>Returns an application-specific style to apply; otherwise, <see langword="null" />.</returns>
		// Token: 0x060055BC RID: 21948 RVA: 0x0000C238 File Offset: 0x0000A438
		public virtual Style SelectStyle(object item, DependencyObject container)
		{
			return null;
		}
	}
}
