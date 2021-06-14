using System;

namespace System.Windows.Markup
{
	/// <summary>Abstract class that provides a means to store parser records for later instantiation. </summary>
	// Token: 0x0200023C RID: 572
	public abstract class XamlInstanceCreator
	{
		/// <summary>When overridden in a derived class, creates a new object to store parser records.</summary>
		/// <returns>The created object.</returns>
		// Token: 0x06002299 RID: 8857
		public abstract object CreateObject();
	}
}
