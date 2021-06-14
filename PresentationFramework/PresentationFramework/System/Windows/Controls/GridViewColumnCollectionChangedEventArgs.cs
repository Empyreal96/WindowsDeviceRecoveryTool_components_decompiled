using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace System.Windows.Controls
{
	// Token: 0x020004D9 RID: 1241
	internal class GridViewColumnCollectionChangedEventArgs : NotifyCollectionChangedEventArgs
	{
		// Token: 0x06004D26 RID: 19750 RVA: 0x0015BABF File Offset: 0x00159CBF
		internal GridViewColumnCollectionChangedEventArgs(GridViewColumn column, string propertyName) : base(NotifyCollectionChangedAction.Reset)
		{
			this._column = column;
			this._propertyName = propertyName;
		}

		// Token: 0x06004D27 RID: 19751 RVA: 0x0015BADD File Offset: 0x00159CDD
		internal GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction action, GridViewColumn[] clearedColumns) : base(action)
		{
			this._clearedColumns = Array.AsReadOnly<GridViewColumn>(clearedColumns);
		}

		// Token: 0x06004D28 RID: 19752 RVA: 0x0015BAF9 File Offset: 0x00159CF9
		internal GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction action, GridViewColumn changedItem, int index, int actualIndex) : base(action, changedItem, index)
		{
			this._actualIndex = actualIndex;
		}

		// Token: 0x06004D29 RID: 19753 RVA: 0x0015BB13 File Offset: 0x00159D13
		internal GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction action, GridViewColumn newItem, GridViewColumn oldItem, int index, int actualIndex) : base(action, newItem, oldItem, index)
		{
			this._actualIndex = actualIndex;
		}

		// Token: 0x06004D2A RID: 19754 RVA: 0x0015BB2F File Offset: 0x00159D2F
		internal GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction action, GridViewColumn changedItem, int index, int oldIndex, int actualIndex) : base(action, changedItem, index, oldIndex)
		{
			this._actualIndex = actualIndex;
		}

		// Token: 0x170012CF RID: 4815
		// (get) Token: 0x06004D2B RID: 19755 RVA: 0x0015BB4B File Offset: 0x00159D4B
		internal int ActualIndex
		{
			get
			{
				return this._actualIndex;
			}
		}

		// Token: 0x170012D0 RID: 4816
		// (get) Token: 0x06004D2C RID: 19756 RVA: 0x0015BB53 File Offset: 0x00159D53
		internal ReadOnlyCollection<GridViewColumn> ClearedColumns
		{
			get
			{
				return this._clearedColumns;
			}
		}

		// Token: 0x170012D1 RID: 4817
		// (get) Token: 0x06004D2D RID: 19757 RVA: 0x0015BB5B File Offset: 0x00159D5B
		internal GridViewColumn Column
		{
			get
			{
				return this._column;
			}
		}

		// Token: 0x170012D2 RID: 4818
		// (get) Token: 0x06004D2E RID: 19758 RVA: 0x0015BB63 File Offset: 0x00159D63
		internal string PropertyName
		{
			get
			{
				return this._propertyName;
			}
		}

		// Token: 0x04002B51 RID: 11089
		private int _actualIndex = -1;

		// Token: 0x04002B52 RID: 11090
		private ReadOnlyCollection<GridViewColumn> _clearedColumns;

		// Token: 0x04002B53 RID: 11091
		private GridViewColumn _column;

		// Token: 0x04002B54 RID: 11092
		private string _propertyName;
	}
}
