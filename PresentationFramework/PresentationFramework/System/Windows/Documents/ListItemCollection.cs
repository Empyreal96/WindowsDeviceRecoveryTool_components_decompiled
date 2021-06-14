using System;

namespace System.Windows.Documents
{
	/// <summary>Represents a collection of <see cref="T:System.Windows.Documents.ListItem" /> elements. <see cref="T:System.Windows.Documents.ListItemCollection" /> defines the allowable child content of a <see cref="T:System.Windows.Documents.List" /> element.</summary>
	// Token: 0x02000396 RID: 918
	public class ListItemCollection : TextElementCollection<ListItem>
	{
		// Token: 0x060031D9 RID: 12761 RVA: 0x000DC19E File Offset: 0x000DA39E
		internal ListItemCollection(DependencyObject owner, bool isOwnerParent) : base(owner, isOwnerParent)
		{
		}

		/// <summary>Gets the first <see cref="T:System.Windows.Documents.ListItem" /> element within this instance of <see cref="T:System.Windows.Documents.ListItemCollection" />.</summary>
		/// <returns>The first <see cref="T:System.Windows.Documents.ListItem" /> element within this instance of <see cref="T:System.Windows.Documents.ListItemCollection" />.</returns>
		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x060031DA RID: 12762 RVA: 0x000DC1A8 File Offset: 0x000DA3A8
		public ListItem FirstListItem
		{
			get
			{
				return base.FirstChild;
			}
		}

		/// <summary>Gets the last <see cref="T:System.Windows.Documents.ListItem" /> element within this instance of <see cref="T:System.Windows.Documents.ListItemCollection" />.</summary>
		/// <returns>The last <see cref="T:System.Windows.Documents.ListItem" /> element within this instance of <see cref="T:System.Windows.Documents.ListItemCollection" />.</returns>
		// Token: 0x17000C91 RID: 3217
		// (get) Token: 0x060031DB RID: 12763 RVA: 0x000DC1B0 File Offset: 0x000DA3B0
		public ListItem LastListItem
		{
			get
			{
				return base.LastChild;
			}
		}
	}
}
