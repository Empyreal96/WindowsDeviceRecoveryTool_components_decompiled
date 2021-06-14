using System;

namespace System.Windows.Controls
{
	/// <summary>Provides a way to choose a <see cref="T:System.Windows.DataTemplate" /> based on the data object and the data-bound element.</summary>
	// Token: 0x020004C0 RID: 1216
	public class DataTemplateSelector
	{
		/// <summary>When overridden in a derived class, returns a <see cref="T:System.Windows.DataTemplate" /> based on custom logic.</summary>
		/// <param name="item">The data object for which to select the template.</param>
		/// <param name="container">The data-bound object.</param>
		/// <returns>Returns a <see cref="T:System.Windows.DataTemplate" /> or <see langword="null" />. The default value is <see langword="null" />.</returns>
		// Token: 0x060049CD RID: 18893 RVA: 0x0000C238 File Offset: 0x0000A438
		public virtual DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			return null;
		}
	}
}
