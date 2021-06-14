using System;

namespace System.Windows.Markup
{
	/// <summary>Provides methods used internally by the WPF XAML parser to attach events and event setters in compiled XAML. </summary>
	// Token: 0x02000217 RID: 535
	public interface IStyleConnector
	{
		/// <summary>Attaches events on event setters and templates in compiled content. </summary>
		/// <param name="connectionId">The unique connection ID for event wiring purposes. </param>
		/// <param name="target">The target for event wiring.</param>
		// Token: 0x06002126 RID: 8486
		void Connect(int connectionId, object target);
	}
}
