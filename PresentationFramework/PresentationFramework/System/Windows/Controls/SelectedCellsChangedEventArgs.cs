using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGrid.SelectedCellsChanged" /> event. </summary>
	// Token: 0x02000529 RID: 1321
	public class SelectedCellsChangedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.SelectedCellsChangedEventArgs" /> class with the specified cells added to and removed from the selection. </summary>
		/// <param name="addedCells">The cells added to the selection.</param>
		/// <param name="removedCells">The cells removed from the selection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="addedCells" /> or <paramref name="removedCells" /> is <see langword="null" />.</exception>
		// Token: 0x060055BE RID: 21950 RVA: 0x0017C38F File Offset: 0x0017A58F
		public SelectedCellsChangedEventArgs(List<DataGridCellInfo> addedCells, List<DataGridCellInfo> removedCells)
		{
			if (addedCells == null)
			{
				throw new ArgumentNullException("addedCells");
			}
			if (removedCells == null)
			{
				throw new ArgumentNullException("removedCells");
			}
			this._addedCells = addedCells.AsReadOnly();
			this._removedCells = removedCells.AsReadOnly();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.SelectedCellsChangedEventArgs" /> class with the specified cells added to and removed from the selection.</summary>
		/// <param name="addedCells">The cells added to the selection.</param>
		/// <param name="removedCells">The cells removed from the selection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="addedCells" /> or <paramref name="removedCells" /> is <see langword="null" />.</exception>
		// Token: 0x060055BF RID: 21951 RVA: 0x0017C3CB File Offset: 0x0017A5CB
		public SelectedCellsChangedEventArgs(ReadOnlyCollection<DataGridCellInfo> addedCells, ReadOnlyCollection<DataGridCellInfo> removedCells)
		{
			if (addedCells == null)
			{
				throw new ArgumentNullException("addedCells");
			}
			if (removedCells == null)
			{
				throw new ArgumentNullException("removedCells");
			}
			this._addedCells = addedCells;
			this._removedCells = removedCells;
		}

		// Token: 0x060055C0 RID: 21952 RVA: 0x0017C3FD File Offset: 0x0017A5FD
		internal SelectedCellsChangedEventArgs(DataGrid owner, VirtualizedCellInfoCollection addedCells, VirtualizedCellInfoCollection removedCells)
		{
			this._addedCells = ((addedCells != null) ? addedCells : VirtualizedCellInfoCollection.MakeEmptyCollection(owner));
			this._removedCells = ((removedCells != null) ? removedCells : VirtualizedCellInfoCollection.MakeEmptyCollection(owner));
		}

		/// <summary>Gets the cells that were added to the selection.</summary>
		/// <returns>The added cells.</returns>
		// Token: 0x170014DA RID: 5338
		// (get) Token: 0x060055C1 RID: 21953 RVA: 0x0017C429 File Offset: 0x0017A629
		public IList<DataGridCellInfo> AddedCells
		{
			get
			{
				return this._addedCells;
			}
		}

		/// <summary>Gets the list of cells removed from the selection.</summary>
		/// <returns>The list of cells removed.</returns>
		// Token: 0x170014DB RID: 5339
		// (get) Token: 0x060055C2 RID: 21954 RVA: 0x0017C431 File Offset: 0x0017A631
		public IList<DataGridCellInfo> RemovedCells
		{
			get
			{
				return this._removedCells;
			}
		}

		// Token: 0x04002E0C RID: 11788
		private IList<DataGridCellInfo> _addedCells;

		// Token: 0x04002E0D RID: 11789
		private IList<DataGridCellInfo> _removedCells;
	}
}
