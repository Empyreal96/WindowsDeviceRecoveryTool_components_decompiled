using System;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x02000485 RID: 1157
	internal class GridEntryCollection : GridItemCollection
	{
		// Token: 0x06004E21 RID: 20001 RVA: 0x001401B2 File Offset: 0x0013E3B2
		public GridEntryCollection(GridEntry owner, GridEntry[] entries) : base(entries)
		{
			this.owner = owner;
		}

		// Token: 0x06004E22 RID: 20002 RVA: 0x001401C4 File Offset: 0x0013E3C4
		public void AddRange(GridEntry[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this.entries != null)
			{
				GridEntry[] array = new GridEntry[this.entries.Length + value.Length];
				this.entries.CopyTo(array, 0);
				value.CopyTo(array, this.entries.Length);
				this.entries = array;
				return;
			}
			this.entries = (GridEntry[])value.Clone();
		}

		// Token: 0x06004E23 RID: 20003 RVA: 0x0014022E File Offset: 0x0013E42E
		public void Clear()
		{
			this.entries = new GridEntry[0];
		}

		// Token: 0x06004E24 RID: 20004 RVA: 0x0014023C File Offset: 0x0013E43C
		public void CopyTo(Array dest, int index)
		{
			this.entries.CopyTo(dest, index);
		}

		// Token: 0x06004E25 RID: 20005 RVA: 0x0014024B File Offset: 0x0013E44B
		internal GridEntry GetEntry(int index)
		{
			return (GridEntry)this.entries[index];
		}

		// Token: 0x06004E26 RID: 20006 RVA: 0x0014025A File Offset: 0x0013E45A
		internal int GetEntry(GridEntry child)
		{
			return Array.IndexOf<GridItem>(this.entries, child);
		}

		// Token: 0x06004E27 RID: 20007 RVA: 0x00140268 File Offset: 0x0013E468
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06004E28 RID: 20008 RVA: 0x00140278 File Offset: 0x0013E478
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.owner != null && this.entries != null)
			{
				for (int i = 0; i < this.entries.Length; i++)
				{
					if (this.entries[i] != null)
					{
						((GridEntry)this.entries[i]).Dispose();
						this.entries[i] = null;
					}
				}
				this.entries = new GridEntry[0];
			}
		}

		// Token: 0x06004E29 RID: 20009 RVA: 0x001402DC File Offset: 0x0013E4DC
		~GridEntryCollection()
		{
			this.Dispose(false);
		}

		// Token: 0x04003331 RID: 13105
		private GridEntry owner;
	}
}
