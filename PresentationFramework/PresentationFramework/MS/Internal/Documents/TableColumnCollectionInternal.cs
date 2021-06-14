using System;
using System.Windows;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020006F1 RID: 1777
	internal class TableColumnCollectionInternal : ContentElementCollection<Table, TableColumn>
	{
		// Token: 0x06007225 RID: 29221 RVA: 0x00209B17 File Offset: 0x00207D17
		internal TableColumnCollectionInternal(Table owner) : base(owner)
		{
		}

		// Token: 0x06007226 RID: 29222 RVA: 0x00209B20 File Offset: 0x00207D20
		public override void Add(TableColumn item)
		{
			int num = base.Version;
			base.Version = num + 1;
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (base.Size == base.Items.Length)
			{
				base.EnsureCapacity(base.Size + 1);
			}
			num = base.Size;
			base.Size = num + 1;
			int index = num;
			this.PrivateConnectChild(index, item);
		}

		// Token: 0x06007227 RID: 29223 RVA: 0x00209B84 File Offset: 0x00207D84
		public override void Clear()
		{
			int version = base.Version;
			base.Version = version + 1;
			for (int i = 0; i < base.Size; i++)
			{
				this.PrivateDisconnectChild(base.Items[i]);
				base.Items[i] = null;
			}
			base.Size = 0;
		}

		// Token: 0x06007228 RID: 29224 RVA: 0x00209BD0 File Offset: 0x00207DD0
		public override void Insert(int index, TableColumn item)
		{
			int num = base.Version;
			base.Version = num + 1;
			if (index < 0 || index > base.Size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (base.Size == base.Items.Length)
			{
				base.EnsureCapacity(base.Size + 1);
			}
			for (int i = base.Size - 1; i >= index; i--)
			{
				base.Items[i + 1] = base.Items[i];
				base.Items[i].Index = i + 1;
			}
			base.Items[index] = null;
			num = base.Size;
			base.Size = num + 1;
			this.PrivateConnectChild(index, item);
		}

		// Token: 0x06007229 RID: 29225 RVA: 0x00209C8C File Offset: 0x00207E8C
		internal override void PrivateConnectChild(int index, TableColumn item)
		{
			if (item.Parent is ContentElementCollection<Table, TableColumn>.DummyProxy)
			{
				if (LogicalTreeHelper.GetParent(item.Parent) != base.Owner)
				{
					throw new ArgumentException(SR.Get("TableCollectionWrongProxyParent"));
				}
			}
			else
			{
				if (item.Parent != null)
				{
					throw new ArgumentException(SR.Get("TableCollectionInOtherCollection"));
				}
				base.Owner.AddLogicalChild(item);
			}
			base.Items[index] = item;
			item.Index = index;
			item.OnEnterParentTree();
		}

		// Token: 0x0600722A RID: 29226 RVA: 0x00209D04 File Offset: 0x00207F04
		internal override void PrivateDisconnectChild(TableColumn item)
		{
			item.OnExitParentTree();
			base.Items[item.Index] = null;
			item.Index = -1;
			if (!(item.Parent is ContentElementCollection<Table, TableColumn>.DummyProxy))
			{
				base.Owner.RemoveLogicalChild(item);
			}
		}

		// Token: 0x0600722B RID: 29227 RVA: 0x00209D48 File Offset: 0x00207F48
		public override bool Remove(TableColumn item)
		{
			int version = base.Version;
			base.Version = version + 1;
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (!base.BelongsToOwner(item))
			{
				return false;
			}
			base.PrivateRemove(item);
			return true;
		}

		// Token: 0x0600722C RID: 29228 RVA: 0x00209D88 File Offset: 0x00207F88
		public override void RemoveAt(int index)
		{
			int version = base.Version;
			base.Version = version + 1;
			if (index < 0 || index >= base.Size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			base.PrivateRemove(base.Items[index]);
		}

		// Token: 0x0600722D RID: 29229 RVA: 0x00209DD0 File Offset: 0x00207FD0
		public override void RemoveRange(int index, int count)
		{
			int version = base.Version;
			base.Version = version + 1;
			if (index < 0 || index >= base.Size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionCountNeedNonNegNum"));
			}
			if (base.Size - index < count)
			{
				throw new ArgumentException(SR.Get("TableCollectionRangeOutOfRange"));
			}
			if (count > 0)
			{
				for (int i = index + count - 1; i >= index; i--)
				{
					this.PrivateDisconnectChild(base.Items[i]);
				}
				base.Size -= count;
				for (int j = index; j < base.Size; j++)
				{
					base.Items[j] = base.Items[j + count];
					base.Items[j].Index = j;
					base.Items[j + count] = null;
				}
			}
		}

		// Token: 0x17001B2C RID: 6956
		public override TableColumn this[int index]
		{
			get
			{
				if (index < 0 || index >= base.Size)
				{
					throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
				}
				return base.Items[index];
			}
			set
			{
				if (index < 0 || index >= base.Size)
				{
					throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.PrivateDisconnectChild(base.Items[index]);
				this.PrivateConnectChild(index, value);
			}
		}
	}
}
