using System;

namespace System.Windows.Documents
{
	/// <summary>Represents a collection of <see cref="T:System.Windows.Documents.Block" /> elements. <see cref="T:System.Windows.Documents.BlockCollection" /> defines the allowable child content of the <see cref="T:System.Windows.Documents.FlowDocument" />, <see cref="T:System.Windows.Documents.Section" />, <see cref="T:System.Windows.Documents.ListItem" />, <see cref="T:System.Windows.Documents.TableCell" />, <see cref="T:System.Windows.Documents.Floater" />, and <see cref="T:System.Windows.Documents.Figure" /> elements.</summary>
	// Token: 0x0200032E RID: 814
	public class BlockCollection : TextElementCollection<Block>
	{
		// Token: 0x06002B09 RID: 11017 RVA: 0x000C4533 File Offset: 0x000C2733
		internal BlockCollection(DependencyObject owner, bool isOwnerParent) : base(owner, isOwnerParent)
		{
		}

		/// <summary>Gets the first <see cref="T:System.Windows.Documents.Block" /> element within this instance of <see cref="T:System.Windows.Documents.BlockCollection" />.</summary>
		/// <returns>The first <see cref="T:System.Windows.Documents.Block" /> element in the <see cref="T:System.Windows.Documents.BlockCollection" />.</returns>
		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x06002B0A RID: 11018 RVA: 0x000C453D File Offset: 0x000C273D
		public Block FirstBlock
		{
			get
			{
				return base.FirstChild;
			}
		}

		/// <summary>Gets the last <see cref="T:System.Windows.Documents.Block" /> element within this instance of <see cref="T:System.Windows.Documents.BlockCollection" />.</summary>
		/// <returns>The last <see cref="T:System.Windows.Documents.Block" /> element in the <see cref="T:System.Windows.Documents.BlockCollection" />.</returns>
		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x06002B0B RID: 11019 RVA: 0x000C4545 File Offset: 0x000C2745
		public Block LastBlock
		{
			get
			{
				return base.LastChild;
			}
		}
	}
}
