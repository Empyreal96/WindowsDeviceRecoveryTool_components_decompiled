using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x02000178 RID: 376
	internal sealed class DataGridState : ICloneable
	{
		// Token: 0x06001415 RID: 5141 RVA: 0x0004C905 File Offset: 0x0004AB05
		public DataGridState()
		{
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x0004C919 File Offset: 0x0004AB19
		public DataGridState(DataGrid dataGrid)
		{
			this.PushState(dataGrid);
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06001417 RID: 5143 RVA: 0x0004C934 File Offset: 0x0004AB34
		internal AccessibleObject ParentRowAccessibleObject
		{
			get
			{
				if (this.parentRowAccessibleObject == null)
				{
					this.parentRowAccessibleObject = new DataGridState.DataGridStateParentRowAccessibleObject(this);
				}
				return this.parentRowAccessibleObject;
			}
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x0004C950 File Offset: 0x0004AB50
		public object Clone()
		{
			return new DataGridState
			{
				DataGridRows = this.DataGridRows,
				DataSource = this.DataSource,
				DataMember = this.DataMember,
				FirstVisibleRow = this.FirstVisibleRow,
				FirstVisibleCol = this.FirstVisibleCol,
				CurrentRow = this.CurrentRow,
				CurrentCol = this.CurrentCol,
				GridColumnStyles = this.GridColumnStyles,
				ListManager = this.ListManager,
				DataGrid = this.DataGrid
			};
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x0004C9DC File Offset: 0x0004ABDC
		public void PushState(DataGrid dataGrid)
		{
			this.DataSource = dataGrid.DataSource;
			this.DataMember = dataGrid.DataMember;
			this.DataGrid = dataGrid;
			this.DataGridRows = dataGrid.DataGridRows;
			this.DataGridRowsLength = dataGrid.DataGridRowsLength;
			this.FirstVisibleRow = dataGrid.firstVisibleRow;
			this.FirstVisibleCol = dataGrid.firstVisibleCol;
			this.CurrentRow = dataGrid.currentRow;
			this.GridColumnStyles = new GridColumnStylesCollection(dataGrid.myGridTable);
			this.GridColumnStyles.Clear();
			foreach (object obj in dataGrid.myGridTable.GridColumnStyles)
			{
				DataGridColumnStyle column = (DataGridColumnStyle)obj;
				this.GridColumnStyles.Add(column);
			}
			this.ListManager = dataGrid.ListManager;
			this.ListManager.ItemChanged += this.DataSource_Changed;
			this.ListManager.MetaDataChanged += this.DataSource_MetaDataChanged;
			this.CurrentCol = dataGrid.currentCol;
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x0004CB00 File Offset: 0x0004AD00
		public void RemoveChangeNotification()
		{
			this.ListManager.ItemChanged -= this.DataSource_Changed;
			this.ListManager.MetaDataChanged -= this.DataSource_MetaDataChanged;
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x0004CB30 File Offset: 0x0004AD30
		public void PullState(DataGrid dataGrid, bool createColumn)
		{
			dataGrid.Set_ListManager(this.DataSource, this.DataMember, true, createColumn);
			dataGrid.firstVisibleRow = this.FirstVisibleRow;
			dataGrid.firstVisibleCol = this.FirstVisibleCol;
			dataGrid.currentRow = this.CurrentRow;
			dataGrid.currentCol = this.CurrentCol;
			dataGrid.SetDataGridRows(this.DataGridRows, this.DataGridRowsLength);
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x0004CB93 File Offset: 0x0004AD93
		private void DataSource_Changed(object sender, ItemChangedEventArgs e)
		{
			if (this.DataGrid != null && this.ListManager.Position == e.Index)
			{
				this.DataGrid.InvalidateParentRows();
				return;
			}
			if (this.DataGrid != null)
			{
				this.DataGrid.ParentRowsDataChanged();
			}
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x0004CBCF File Offset: 0x0004ADCF
		private void DataSource_MetaDataChanged(object sender, EventArgs e)
		{
			if (this.DataGrid != null)
			{
				this.DataGrid.ParentRowsDataChanged();
			}
		}

		// Token: 0x040009CE RID: 2510
		public object DataSource;

		// Token: 0x040009CF RID: 2511
		public string DataMember;

		// Token: 0x040009D0 RID: 2512
		public CurrencyManager ListManager;

		// Token: 0x040009D1 RID: 2513
		public DataGridRow[] DataGridRows = new DataGridRow[0];

		// Token: 0x040009D2 RID: 2514
		public DataGrid DataGrid;

		// Token: 0x040009D3 RID: 2515
		public int DataGridRowsLength;

		// Token: 0x040009D4 RID: 2516
		public GridColumnStylesCollection GridColumnStyles;

		// Token: 0x040009D5 RID: 2517
		public int FirstVisibleRow;

		// Token: 0x040009D6 RID: 2518
		public int FirstVisibleCol;

		// Token: 0x040009D7 RID: 2519
		public int CurrentRow;

		// Token: 0x040009D8 RID: 2520
		public int CurrentCol;

		// Token: 0x040009D9 RID: 2521
		public DataGridRow LinkingRow;

		// Token: 0x040009DA RID: 2522
		private AccessibleObject parentRowAccessibleObject;

		// Token: 0x02000599 RID: 1433
		[ComVisible(true)]
		internal class DataGridStateParentRowAccessibleObject : AccessibleObject
		{
			// Token: 0x0600584D RID: 22605 RVA: 0x0017353C File Offset: 0x0017173C
			public DataGridStateParentRowAccessibleObject(DataGridState owner)
			{
				this.owner = owner;
			}

			// Token: 0x1700152B RID: 5419
			// (get) Token: 0x0600584E RID: 22606 RVA: 0x0017354C File Offset: 0x0017174C
			public override Rectangle Bounds
			{
				get
				{
					DataGridParentRows dataGridParentRows = ((DataGridParentRows.DataGridParentRowsAccessibleObject)this.Parent).Owner;
					DataGrid dataGrid = this.owner.LinkingRow.DataGrid;
					Rectangle boundsForDataGridStateAccesibility = dataGridParentRows.GetBoundsForDataGridStateAccesibility(this.owner);
					boundsForDataGridStateAccesibility.Y += dataGrid.ParentRowsBounds.Y;
					return dataGrid.RectangleToScreen(boundsForDataGridStateAccesibility);
				}
			}

			// Token: 0x1700152C RID: 5420
			// (get) Token: 0x0600584F RID: 22607 RVA: 0x001735AB File Offset: 0x001717AB
			public override string Name
			{
				get
				{
					return SR.GetString("AccDGParentRow");
				}
			}

			// Token: 0x1700152D RID: 5421
			// (get) Token: 0x06005850 RID: 22608 RVA: 0x001735B7 File Offset: 0x001717B7
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.owner.LinkingRow.DataGrid.ParentRowsAccessibleObject;
				}
			}

			// Token: 0x1700152E RID: 5422
			// (get) Token: 0x06005851 RID: 22609 RVA: 0x0000E0C0 File Offset: 0x0000C2C0
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.ListItem;
				}
			}

			// Token: 0x1700152F RID: 5423
			// (get) Token: 0x06005852 RID: 22610 RVA: 0x001735D0 File Offset: 0x001717D0
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					StringBuilder stringBuilder = new StringBuilder();
					CurrencyManager currencyManager = (CurrencyManager)this.owner.LinkingRow.DataGrid.BindingContext[this.owner.DataSource, this.owner.DataMember];
					stringBuilder.Append(this.owner.ListManager.GetListName());
					stringBuilder.Append(": ");
					bool flag = false;
					foreach (object obj in this.owner.GridColumnStyles)
					{
						DataGridColumnStyle dataGridColumnStyle = (DataGridColumnStyle)obj;
						if (flag)
						{
							stringBuilder.Append(", ");
						}
						string headerText = dataGridColumnStyle.HeaderText;
						string value = dataGridColumnStyle.PropertyDescriptor.Converter.ConvertToString(dataGridColumnStyle.PropertyDescriptor.GetValue(currencyManager.Current));
						stringBuilder.Append(headerText);
						stringBuilder.Append(": ");
						stringBuilder.Append(value);
						flag = true;
					}
					return stringBuilder.ToString();
				}
			}

			// Token: 0x06005853 RID: 22611 RVA: 0x001736F0 File Offset: 0x001718F0
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navdir)
			{
				DataGridParentRows.DataGridParentRowsAccessibleObject dataGridParentRowsAccessibleObject = (DataGridParentRows.DataGridParentRowsAccessibleObject)this.Parent;
				switch (navdir)
				{
				case AccessibleNavigation.Up:
				case AccessibleNavigation.Left:
				case AccessibleNavigation.Previous:
					return dataGridParentRowsAccessibleObject.GetPrev(this);
				case AccessibleNavigation.Down:
				case AccessibleNavigation.Right:
				case AccessibleNavigation.Next:
					return dataGridParentRowsAccessibleObject.GetNext(this);
				default:
					return null;
				}
			}

			// Token: 0x040038AB RID: 14507
			private DataGridState owner;
		}
	}
}
