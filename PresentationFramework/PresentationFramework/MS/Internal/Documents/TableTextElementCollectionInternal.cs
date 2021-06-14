using System;
using System.Collections;
using System.Windows;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020006F2 RID: 1778
	internal class TableTextElementCollectionInternal<TParent, TElementType> : ContentElementCollection<TParent, TElementType> where TParent : TextElement, IAcceptInsertion where TElementType : TextElement, IIndexedChild<TParent>
	{
		// Token: 0x06007230 RID: 29232 RVA: 0x00209F1A File Offset: 0x0020811A
		internal TableTextElementCollectionInternal(TParent owner) : base(owner)
		{
		}

		// Token: 0x06007231 RID: 29233 RVA: 0x00209F24 File Offset: 0x00208124
		public override void Add(TElementType item)
		{
			int version = base.Version;
			base.Version = version + 1;
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (item.Parent != null)
			{
				throw new ArgumentException(SR.Get("TableCollectionInOtherCollection"));
			}
			base.Owner.InsertionIndex = base.Size;
			item.RepositionWithContent(base.Owner.ContentEnd);
			base.Owner.InsertionIndex = -1;
		}

		// Token: 0x06007232 RID: 29234 RVA: 0x00209FB4 File Offset: 0x002081B4
		public override void Clear()
		{
			int version = base.Version;
			base.Version = version + 1;
			for (int i = base.Size - 1; i >= 0; i--)
			{
				this.Remove(base.Items[i]);
			}
			base.Size = 0;
		}

		// Token: 0x06007233 RID: 29235 RVA: 0x0020A000 File Offset: 0x00208200
		public override void Insert(int index, TElementType item)
		{
			int version = base.Version;
			base.Version = version + 1;
			if (index < 0 || index > base.Size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (item.Parent != null)
			{
				throw new ArgumentException(SR.Get("TableCollectionInOtherCollection"));
			}
			base.Owner.InsertionIndex = index;
			if (index == base.Size)
			{
				item.RepositionWithContent(base.Owner.ContentEnd);
			}
			else
			{
				TElementType telementType = base.Items[index];
				TextPointer textPosition = new TextPointer(telementType.ContentStart, -1);
				item.RepositionWithContent(textPosition);
			}
			base.Owner.InsertionIndex = -1;
		}

		// Token: 0x06007234 RID: 29236 RVA: 0x0020A0E0 File Offset: 0x002082E0
		public override bool Remove(TElementType item)
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
			TextPointer startPosition = new TextPointer(item.TextContainer, item.TextElementNode, ElementEdge.BeforeStart, LogicalDirection.Backward);
			TextPointer endPosition = new TextPointer(item.TextContainer, item.TextElementNode, ElementEdge.AfterEnd, LogicalDirection.Backward);
			base.Owner.TextContainer.BeginChange();
			try
			{
				base.Owner.TextContainer.DeleteContentInternal(startPosition, endPosition);
			}
			finally
			{
				base.Owner.TextContainer.EndChange();
			}
			return true;
		}

		// Token: 0x06007235 RID: 29237 RVA: 0x0020A1AC File Offset: 0x002083AC
		public override void RemoveAt(int index)
		{
			int version = base.Version;
			base.Version = version + 1;
			if (index < 0 || index >= base.Size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			this.Remove(base.Items[index]);
		}

		// Token: 0x06007236 RID: 29238 RVA: 0x0020A1FC File Offset: 0x002083FC
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
					this.Remove(base.Items[i]);
				}
			}
		}

		// Token: 0x06007237 RID: 29239 RVA: 0x0020A28C File Offset: 0x0020848C
		internal override void PrivateConnectChild(int index, TElementType item)
		{
			if (item.Parent is ContentElementCollection<TParent, TElementType>.DummyProxy && LogicalTreeHelper.GetParent(item.Parent) != base.Owner)
			{
				throw new ArgumentException(SR.Get("TableCollectionWrongProxyParent"));
			}
			base.Items[index] = item;
			item.Index = index;
			item.OnEnterParentTree();
		}

		// Token: 0x06007238 RID: 29240 RVA: 0x0020A2FC File Offset: 0x002084FC
		internal override void PrivateDisconnectChild(TElementType item)
		{
			int index = item.Index;
			item.OnExitParentTree();
			base.Items[item.Index] = default(TElementType);
			item.Index = -1;
			int size = base.Size - 1;
			base.Size = size;
			for (int i = index; i < base.Size; i++)
			{
				base.Items[i] = base.Items[i + 1];
				base.Items[i].Index = i;
			}
			base.Items[base.Size] = default(TElementType);
			item.OnAfterExitParentTree(base.Owner);
		}

		// Token: 0x06007239 RID: 29241 RVA: 0x0020A3C8 File Offset: 0x002085C8
		internal int FindInsertionIndex(TElementType item)
		{
			int num = 0;
			object obj = item;
			if (item.Parent is ContentElementCollection<TParent, TElementType>.DummyProxy)
			{
				obj = item.Parent;
			}
			IEnumerator enumerator = base.Owner.IsEmpty ? new RangeContentEnumerator(null, null) : new RangeContentEnumerator(base.Owner.ContentStart, base.Owner.ContentEnd);
			while (enumerator.MoveNext())
			{
				if (obj == enumerator.Current)
				{
					return num;
				}
				if (enumerator.Current is TElementType || enumerator.Current is ContentElementCollection<TParent, TElementType>.DummyProxy)
				{
					num++;
				}
			}
			Invariant.Assert(false);
			return -1;
		}

		// Token: 0x0600723A RID: 29242 RVA: 0x0020A478 File Offset: 0x00208678
		internal void InternalAdd(TElementType item)
		{
			if (base.Size == base.Items.Length)
			{
				base.EnsureCapacity(base.Size + 1);
			}
			int num = base.Owner.InsertionIndex;
			if (num == -1)
			{
				num = this.FindInsertionIndex(item);
			}
			for (int i = base.Size - 1; i >= num; i--)
			{
				base.Items[i + 1] = base.Items[i];
				base.Items[i].Index = i + 1;
			}
			base.Items[num] = default(TElementType);
			int size = base.Size;
			base.Size = size + 1;
			this.PrivateConnectChild(num, item);
		}

		// Token: 0x0600723B RID: 29243 RVA: 0x0020A533 File Offset: 0x00208733
		internal void InternalRemove(TElementType item)
		{
			this.PrivateDisconnectChild(item);
		}

		// Token: 0x17001B2D RID: 6957
		public override TElementType this[int index]
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
				if (value.Parent != null)
				{
					throw new ArgumentException(SR.Get("TableCollectionInOtherCollection"));
				}
				this.RemoveAt(index);
				this.Insert(index, value);
			}
		}
	}
}
