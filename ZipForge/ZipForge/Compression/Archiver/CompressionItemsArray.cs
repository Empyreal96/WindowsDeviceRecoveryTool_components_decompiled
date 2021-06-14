using System;
using System.Collections;
using ComponentAce.Compression.Exception;
using ComponentAce.Compression.Interfaces;

namespace ComponentAce.Compression.Archiver
{
	// Token: 0x02000022 RID: 34
	internal class CompressionItemsArray : IItemsArray, ICloneable
	{
		// Token: 0x06000157 RID: 343 RVA: 0x00010694 File Offset: 0x0000F694
		public CompressionItemsArray()
		{
			this._dirItemsArray = new ArrayList();
			this._dirItemsHashtable = new Hashtable();
		}

		// Token: 0x06000158 RID: 344 RVA: 0x000106B4 File Offset: 0x0000F6B4
		protected internal virtual void RemoveAt(int index)
		{
			string name = this[index].Name;
			this._dirItemsHashtable.Remove(name);
			this._dirItemsArray.RemoveAt(index);
			this.UpdateHashTable();
		}

		// Token: 0x06000159 RID: 345 RVA: 0x000106EC File Offset: 0x0000F6EC
		protected internal virtual void RemoveAll()
		{
			this._dirItemsArray.Clear();
			this._dirItemsHashtable.Clear();
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00010704 File Offset: 0x0000F704
		public object Clone()
		{
			CompressionItemsArray compressionItemsArray = new CompressionItemsArray();
			foreach (object obj in this._dirItemsArray)
			{
				IItem item = (IItem)obj;
				compressionItemsArray._dirItemsArray.Add(item.Clone());
			}
			compressionItemsArray._dirItemsHashtable = (Hashtable)this._dirItemsHashtable.Clone();
			return compressionItemsArray;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00010788 File Offset: 0x0000F788
		private void CopyFrom(CompressionItemsArray from)
		{
			this._dirItemsArray = (ArrayList)from._dirItemsArray.Clone();
			this.UpdateHashTable();
			foreach (object obj in this._dirItemsArray)
			{
				IItem item = (IItem)obj;
				item.ItemNameChanged += this.ItemNameChangedHandler;
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00010808 File Offset: 0x0000F808
		private void UpdateHashTable()
		{
			this._dirItemsHashtable.Clear();
			int num = 0;
			foreach (object obj in this._dirItemsArray)
			{
				IItem item = (IItem)obj;
				this._dirItemsHashtable.Add(item.Name, num++);
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00010884 File Offset: 0x0000F884
		public int Length
		{
			get
			{
				return this._dirItemsArray.Count;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00010891 File Offset: 0x0000F891
		// (set) Token: 0x0600015F RID: 351 RVA: 0x0001089E File Offset: 0x0000F89E
		private int Capacity
		{
			get
			{
				return this._dirItemsArray.Capacity;
			}
			set
			{
				if (value == 0)
				{
					this._dirItemsArray.Clear();
					this._dirItemsHashtable.Clear();
				}
				this._dirItemsArray.Capacity = value;
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x000108C8 File Offset: 0x0000F8C8
		public int GetDirItemIndexByName(string name)
		{
			object obj = this._dirItemsHashtable[name];
			if (obj == null)
			{
				return -1;
			}
			return (int)obj;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x000108F0 File Offset: 0x0000F8F0
		private void ItemNameChangedHandler(object sender, EventArgs e)
		{
			IItem item = (IItem)sender;
			int num = 0;
			object obj = this._dirItemsHashtable[item.OldName];
			if (obj != null)
			{
				num = (int)obj;
				this._dirItemsHashtable.Remove(item.OldName);
			}
			this._dirItemsHashtable.Add(item.Name, num);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0001094C File Offset: 0x0000F94C
		internal void SetItem(int index, IItem value)
		{
			if (index < 0 || index >= this.Count)
			{
				throw ExceptionBuilder.Exception(ErrorCode.IndexOutOfBounds);
			}
			if (index >= this._dirItemsArray.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			string name = value.Name;
			if (name == "")
			{
				throw new ArgumentException("The directory item name is empty");
			}
			value.ItemNameChanged += this.ItemNameChangedHandler;
			string name2 = (this._dirItemsArray[index] as IItem).Name;
			this._dirItemsHashtable.Remove(name2);
			this._dirItemsArray[index] = value;
			this._dirItemsHashtable.Add(name, index);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x000109F4 File Offset: 0x0000F9F4
		internal IItem GetItem(int index)
		{
			if (index < 0 || index >= this.Count)
			{
				throw ExceptionBuilder.Exception(ErrorCode.IndexOutOfBounds);
			}
			return this._dirItemsArray[index] as IItem;
		}

		// Token: 0x1700002C RID: 44
		public IItem this[int index]
		{
			get
			{
				return this.GetItem(index);
			}
			set
			{
				this.SetItem(index, value);
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00010A30 File Offset: 0x0000FA30
		public void AddItem(IItem item)
		{
			this._dirItemsArray.Add(item);
			this._dirItemsHashtable.Add(item.Name, this._dirItemsArray.Count - 1);
			item.ItemNameChanged += this.ItemNameChangedHandler;
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00010A7F File Offset: 0x0000FA7F
		public int Count
		{
			get
			{
				return this.GetCount();
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00010A88 File Offset: 0x0000FA88
		~CompressionItemsArray()
		{
			this._dirItemsArray = null;
			this._dirItemsHashtable = null;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00010ABC File Offset: 0x0000FABC
		internal int GetCount()
		{
			return this.Length;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00010AC4 File Offset: 0x0000FAC4
		public void DeleteItem(int index)
		{
			if (index < 0 || index >= this.Count)
			{
				throw ExceptionBuilder.Exception(ErrorCode.IndexOutOfBounds);
			}
			this.RemoveAt(index);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00010AE1 File Offset: 0x0000FAE1
		public void Clear()
		{
			this.RemoveAll();
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00010AEC File Offset: 0x0000FAEC
		public void ClearTags()
		{
			for (int i = 0; i < this.Count; i++)
			{
				this[i].IsTagged = false;
			}
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00010B18 File Offset: 0x0000FB18
		public bool FileExists(string fileName, ref int itemNo)
		{
			int dirItemIndexByName;
			if ((dirItemIndexByName = this.GetDirItemIndexByName(fileName)) != -1)
			{
				itemNo = dirItemIndexByName;
				return true;
			}
			return false;
		}

		// Token: 0x040000C2 RID: 194
		protected internal ArrayList _dirItemsArray;

		// Token: 0x040000C3 RID: 195
		protected internal Hashtable _dirItemsHashtable;
	}
}
