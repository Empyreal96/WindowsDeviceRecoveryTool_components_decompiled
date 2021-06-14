using System;

namespace System.Windows.Documents
{
	/// <summary>Represents an element on a page that can be linked to from other documents or other places in the same document.</summary>
	// Token: 0x02000392 RID: 914
	public sealed class LinkTarget
	{
		/// <summary>Gets or sets the name of the element that this <see cref="T:System.Windows.Documents.LinkTarget" /> identifies as a linkable element.</summary>
		/// <returns>A <see cref="T:System.String" /> that is identical to the <see cref="P:System.Windows.FrameworkElement.Name" /> property of the markup element that corresponds to this <see cref="T:System.Windows.Documents.LinkTarget" /> element.</returns>
		// Token: 0x17000C7C RID: 3196
		// (get) Token: 0x060031A4 RID: 12708 RVA: 0x000DBA90 File Offset: 0x000D9C90
		// (set) Token: 0x060031A5 RID: 12709 RVA: 0x000DBA98 File Offset: 0x000D9C98
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x04001E97 RID: 7831
		private string _name;
	}
}
