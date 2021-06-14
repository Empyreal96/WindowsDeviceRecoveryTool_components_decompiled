using System;

namespace System.Windows.Forms
{
	/// <summary>The site for a <see cref="T:System.Windows.Forms.Design.ComponentEditorPage" />.</summary>
	// Token: 0x02000439 RID: 1081
	public interface IComponentEditorPageSite
	{
		/// <summary>Returns the parent control for the page window.</summary>
		/// <returns>The parent control for the page window.</returns>
		// Token: 0x06004B4A RID: 19274
		Control GetControl();

		/// <summary>Notifies the site that the editor is in a modified state.</summary>
		// Token: 0x06004B4B RID: 19275
		void SetDirty();
	}
}
