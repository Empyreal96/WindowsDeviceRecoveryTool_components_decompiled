using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace System.Windows.Controls
{
	// Token: 0x020004A4 RID: 1188
	internal class DataGridColumnHeaderCollection : IEnumerable, INotifyCollectionChanged, IDisposable
	{
		// Token: 0x06004887 RID: 18567 RVA: 0x00149E08 File Offset: 0x00148008
		public DataGridColumnHeaderCollection(ObservableCollection<DataGridColumn> columns)
		{
			this._columns = columns;
			if (this._columns != null)
			{
				this._columns.CollectionChanged += this.OnColumnsChanged;
			}
		}

		// Token: 0x06004888 RID: 18568 RVA: 0x00149E36 File Offset: 0x00148036
		public DataGridColumn ColumnFromIndex(int index)
		{
			if (index >= 0 && index < this._columns.Count)
			{
				return this._columns[index];
			}
			return null;
		}

		// Token: 0x06004889 RID: 18569 RVA: 0x00149E58 File Offset: 0x00148058
		internal void NotifyHeaderPropertyChanged(DataGridColumn column, DependencyPropertyChangedEventArgs e)
		{
			NotifyCollectionChangedEventArgs args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, e.NewValue, e.OldValue, this._columns.IndexOf(column));
			this.FireCollectionChanged(args);
		}

		// Token: 0x0600488A RID: 18570 RVA: 0x00149E8D File Offset: 0x0014808D
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			if (this._columns != null)
			{
				this._columns.CollectionChanged -= this.OnColumnsChanged;
			}
		}

		// Token: 0x0600488B RID: 18571 RVA: 0x00149EB4 File Offset: 0x001480B4
		public IEnumerator GetEnumerator()
		{
			return new DataGridColumnHeaderCollection.ColumnHeaderCollectionEnumerator(this._columns);
		}

		// Token: 0x140000C8 RID: 200
		// (add) Token: 0x0600488C RID: 18572 RVA: 0x00149EC4 File Offset: 0x001480C4
		// (remove) Token: 0x0600488D RID: 18573 RVA: 0x00149EFC File Offset: 0x001480FC
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x0600488E RID: 18574 RVA: 0x00149F34 File Offset: 0x00148134
		private void OnColumnsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			NotifyCollectionChangedEventArgs args;
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				args = new NotifyCollectionChangedEventArgs(e.Action, DataGridColumnHeaderCollection.HeadersFromColumns(e.NewItems), e.NewStartingIndex);
				break;
			case NotifyCollectionChangedAction.Remove:
				args = new NotifyCollectionChangedEventArgs(e.Action, DataGridColumnHeaderCollection.HeadersFromColumns(e.OldItems), e.OldStartingIndex);
				break;
			case NotifyCollectionChangedAction.Replace:
				args = new NotifyCollectionChangedEventArgs(e.Action, DataGridColumnHeaderCollection.HeadersFromColumns(e.NewItems), DataGridColumnHeaderCollection.HeadersFromColumns(e.OldItems), e.OldStartingIndex);
				break;
			case NotifyCollectionChangedAction.Move:
				args = new NotifyCollectionChangedEventArgs(e.Action, DataGridColumnHeaderCollection.HeadersFromColumns(e.OldItems), e.NewStartingIndex, e.OldStartingIndex);
				break;
			default:
				args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
				break;
			}
			this.FireCollectionChanged(args);
		}

		// Token: 0x0600488F RID: 18575 RVA: 0x00149FFE File Offset: 0x001481FE
		private void FireCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, args);
			}
		}

		// Token: 0x06004890 RID: 18576 RVA: 0x0014A018 File Offset: 0x00148218
		private static object[] HeadersFromColumns(IList columns)
		{
			object[] array = new object[columns.Count];
			for (int i = 0; i < columns.Count; i++)
			{
				DataGridColumn dataGridColumn = columns[i] as DataGridColumn;
				if (dataGridColumn != null)
				{
					array[i] = dataGridColumn.Header;
				}
				else
				{
					array[i] = null;
				}
			}
			return array;
		}

		// Token: 0x0400299F RID: 10655
		private ObservableCollection<DataGridColumn> _columns;

		// Token: 0x02000969 RID: 2409
		private class ColumnHeaderCollectionEnumerator : IEnumerator, IDisposable
		{
			// Token: 0x0600875D RID: 34653 RVA: 0x0024FA8B File Offset: 0x0024DC8B
			public ColumnHeaderCollectionEnumerator(ObservableCollection<DataGridColumn> columns)
			{
				if (columns != null)
				{
					this._columns = columns;
					this._columns.CollectionChanged += this.OnColumnsChanged;
				}
				this._current = -1;
			}

			// Token: 0x17001E97 RID: 7831
			// (get) Token: 0x0600875E RID: 34654 RVA: 0x0024FABC File Offset: 0x0024DCBC
			public object Current
			{
				get
				{
					if (!this.IsValid)
					{
						throw new InvalidOperationException();
					}
					DataGridColumn dataGridColumn = this._columns[this._current];
					if (dataGridColumn != null)
					{
						return dataGridColumn.Header;
					}
					return null;
				}
			}

			// Token: 0x0600875F RID: 34655 RVA: 0x0024FAF4 File Offset: 0x0024DCF4
			public bool MoveNext()
			{
				if (this.HasChanged)
				{
					throw new InvalidOperationException();
				}
				if (this._columns != null && this._current < this._columns.Count - 1)
				{
					this._current++;
					return true;
				}
				return false;
			}

			// Token: 0x06008760 RID: 34656 RVA: 0x0024FB32 File Offset: 0x0024DD32
			public void Reset()
			{
				if (this.HasChanged)
				{
					throw new InvalidOperationException();
				}
				this._current = -1;
			}

			// Token: 0x06008761 RID: 34657 RVA: 0x0024FB49 File Offset: 0x0024DD49
			public void Dispose()
			{
				GC.SuppressFinalize(this);
				if (this._columns != null)
				{
					this._columns.CollectionChanged -= this.OnColumnsChanged;
				}
			}

			// Token: 0x17001E98 RID: 7832
			// (get) Token: 0x06008762 RID: 34658 RVA: 0x0024FB70 File Offset: 0x0024DD70
			private bool HasChanged
			{
				get
				{
					return this._columnsChanged;
				}
			}

			// Token: 0x17001E99 RID: 7833
			// (get) Token: 0x06008763 RID: 34659 RVA: 0x0024FB78 File Offset: 0x0024DD78
			private bool IsValid
			{
				get
				{
					return this._columns != null && this._current >= 0 && this._current < this._columns.Count && !this.HasChanged;
				}
			}

			// Token: 0x06008764 RID: 34660 RVA: 0x0024FBA9 File Offset: 0x0024DDA9
			private void OnColumnsChanged(object sender, NotifyCollectionChangedEventArgs e)
			{
				this._columnsChanged = true;
			}

			// Token: 0x04004434 RID: 17460
			private int _current;

			// Token: 0x04004435 RID: 17461
			private bool _columnsChanged;

			// Token: 0x04004436 RID: 17462
			private ObservableCollection<DataGridColumn> _columns;
		}
	}
}
