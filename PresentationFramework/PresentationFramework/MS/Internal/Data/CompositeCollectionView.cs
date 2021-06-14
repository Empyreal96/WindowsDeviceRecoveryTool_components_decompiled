using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MS.Internal.Hashing.PresentationFramework;
using MS.Internal.Utility;

namespace MS.Internal.Data
{
	// Token: 0x02000710 RID: 1808
	internal sealed class CompositeCollectionView : CollectionView
	{
		// Token: 0x06007440 RID: 29760 RVA: 0x00213AB4 File Offset: 0x00211CB4
		internal CompositeCollectionView(CompositeCollection collection) : base(collection, -1)
		{
			this._collection = collection;
			this._collection.ContainedCollectionChanged += this.OnContainedCollectionChanged;
			int num = this.PrivateIsEmpty ? -1 : 0;
			int count = this.PrivateIsEmpty ? 0 : 1;
			base.SetCurrent(this.GetItem(num, out this._currentPositionX, out this._currentPositionY), num, count);
		}

		// Token: 0x17001BAE RID: 7086
		// (get) Token: 0x06007441 RID: 29761 RVA: 0x00213B2A File Offset: 0x00211D2A
		public override int Count
		{
			get
			{
				if (this._count == -1)
				{
					this._count = this.CountDeep(this._collection.Count);
				}
				return this._count;
			}
		}

		// Token: 0x17001BAF RID: 7087
		// (get) Token: 0x06007442 RID: 29762 RVA: 0x00213B52 File Offset: 0x00211D52
		public override bool IsEmpty
		{
			get
			{
				return this.PrivateIsEmpty;
			}
		}

		// Token: 0x17001BB0 RID: 7088
		// (get) Token: 0x06007443 RID: 29763 RVA: 0x00213B5C File Offset: 0x00211D5C
		private bool PrivateIsEmpty
		{
			get
			{
				if (this._count < 0)
				{
					for (int i = 0; i < this._collection.Count; i++)
					{
						CollectionContainer collectionContainer = this._collection[i] as CollectionContainer;
						if (collectionContainer == null || collectionContainer.ViewCount != 0)
						{
							return false;
						}
					}
					this.CacheCount(0);
				}
				return this._count == 0;
			}
		}

		// Token: 0x17001BB1 RID: 7089
		// (get) Token: 0x06007444 RID: 29764 RVA: 0x00213BB7 File Offset: 0x00211DB7
		public override bool IsCurrentAfterLast
		{
			get
			{
				return this.IsEmpty || this._currentPositionX >= this._collection.Count;
			}
		}

		// Token: 0x17001BB2 RID: 7090
		// (get) Token: 0x06007445 RID: 29765 RVA: 0x00213BD9 File Offset: 0x00211DD9
		public override bool IsCurrentBeforeFirst
		{
			get
			{
				return this.IsEmpty || this._currentPositionX < 0;
			}
		}

		// Token: 0x17001BB3 RID: 7091
		// (get) Token: 0x06007446 RID: 29766 RVA: 0x0000B02A File Offset: 0x0000922A
		public override bool CanFilter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06007447 RID: 29767 RVA: 0x00213BEE File Offset: 0x00211DEE
		public override bool Contains(object item)
		{
			return this.FindItem(item, false) >= 0;
		}

		// Token: 0x06007448 RID: 29768 RVA: 0x00213BFE File Offset: 0x00211DFE
		public override int IndexOf(object item)
		{
			return this.FindItem(item, false);
		}

		// Token: 0x06007449 RID: 29769 RVA: 0x00213C08 File Offset: 0x00211E08
		public override object GetItemAt(int index)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			int num;
			int num2;
			object item = this.GetItem(index, out num, out num2);
			if (item == CompositeCollectionView.s_afterLast)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return item;
		}

		// Token: 0x0600744A RID: 29770 RVA: 0x00213C46 File Offset: 0x00211E46
		public override bool MoveCurrentTo(object item)
		{
			if (ItemsControl.EqualsEx(this.CurrentItem, item) && (item != null || this.IsCurrentInView))
			{
				return this.IsCurrentInView;
			}
			if (!this.IsEmpty)
			{
				this.FindItem(item, true);
			}
			return this.IsCurrentInView;
		}

		// Token: 0x0600744B RID: 29771 RVA: 0x00213C7F File Offset: 0x00211E7F
		public override bool MoveCurrentToFirst()
		{
			return !this.IsEmpty && this._MoveTo(0);
		}

		// Token: 0x0600744C RID: 29772 RVA: 0x00213C94 File Offset: 0x00211E94
		public override bool MoveCurrentToLast()
		{
			bool isCurrentAfterLast = this.IsCurrentAfterLast;
			bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
			int num = this.Count - 1;
			int currentPositionX;
			int currentPositionY;
			object lastItem = this.GetLastItem(out currentPositionX, out currentPositionY);
			if ((this.CurrentPosition != num || this.CurrentItem != lastItem) && base.OKToChangeCurrent())
			{
				this._currentPositionX = currentPositionX;
				this._currentPositionY = currentPositionY;
				base.SetCurrent(lastItem, num);
				this.OnCurrentChanged();
				if (this.IsCurrentAfterLast != isCurrentAfterLast)
				{
					this.OnPropertyChanged("IsCurrentAfterLast");
				}
				if (this.IsCurrentBeforeFirst != isCurrentBeforeFirst)
				{
					this.OnPropertyChanged("IsCurrentBeforeFirst");
				}
				this.OnPropertyChanged("CurrentPosition");
				this.OnPropertyChanged("CurrentItem");
			}
			return this.IsCurrentInView;
		}

		// Token: 0x0600744D RID: 29773 RVA: 0x00213D43 File Offset: 0x00211F43
		public override bool MoveCurrentToNext()
		{
			return !this.IsCurrentAfterLast && this._MoveTo(this.CurrentPosition + 1);
		}

		// Token: 0x0600744E RID: 29774 RVA: 0x00213D5D File Offset: 0x00211F5D
		public override bool MoveCurrentToPrevious()
		{
			return !this.IsCurrentBeforeFirst && this._MoveTo(this.CurrentPosition - 1);
		}

		// Token: 0x0600744F RID: 29775 RVA: 0x00213D78 File Offset: 0x00211F78
		public override bool MoveCurrentToPosition(int position)
		{
			if (position < -1)
			{
				throw new ArgumentOutOfRangeException("position");
			}
			int currentPositionX;
			int currentPositionY;
			object obj = this.GetItem(position, out currentPositionX, out currentPositionY);
			if (position != this.CurrentPosition || obj != this.CurrentItem)
			{
				if (obj == CompositeCollectionView.s_afterLast)
				{
					obj = null;
					if (position > this.Count)
					{
						throw new ArgumentOutOfRangeException("position");
					}
				}
				if (base.OKToChangeCurrent())
				{
					bool isCurrentAfterLast = this.IsCurrentAfterLast;
					bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
					this._currentPositionX = currentPositionX;
					this._currentPositionY = currentPositionY;
					base.SetCurrent(obj, position);
					this.OnCurrentChanged();
					if (this.IsCurrentAfterLast != isCurrentAfterLast)
					{
						this.OnPropertyChanged("IsCurrentAfterLast");
					}
					if (this.IsCurrentBeforeFirst != isCurrentBeforeFirst)
					{
						this.OnPropertyChanged("IsCurrentBeforeFirst");
					}
					this.OnPropertyChanged("CurrentPosition");
					this.OnPropertyChanged("CurrentItem");
				}
			}
			return this.IsCurrentInView;
		}

		// Token: 0x06007450 RID: 29776 RVA: 0x00213E4B File Offset: 0x0021204B
		protected override void RefreshOverride()
		{
			this._version++;
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		// Token: 0x06007451 RID: 29777 RVA: 0x00213E67 File Offset: 0x00212067
		protected override IEnumerator GetEnumerator()
		{
			return new CompositeCollectionView.FlatteningEnumerator(this._collection, this);
		}

		// Token: 0x06007452 RID: 29778 RVA: 0x00213E78 File Offset: 0x00212078
		protected override void ProcessCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			this.ValidateCollectionChangedEventArgs(args);
			bool flag = false;
			switch (args.Action)
			{
			case NotifyCollectionChangedAction.Add:
			case NotifyCollectionChangedAction.Remove:
			{
				object obj;
				int num;
				if (args.Action == NotifyCollectionChangedAction.Add)
				{
					obj = args.NewItems[0];
					num = args.NewStartingIndex;
				}
				else
				{
					obj = args.OldItems[0];
					num = args.OldStartingIndex;
				}
				int num2 = num;
				if (this._traceLog != null)
				{
					this._traceLog.Add("ProcessCollectionChanged  action = {0}  item = {1}", new object[]
					{
						args.Action,
						TraceLog.IdFor(obj)
					});
				}
				CollectionContainer collectionContainer = obj as CollectionContainer;
				if (collectionContainer == null)
				{
					for (int i = num2 - 1; i >= 0; i--)
					{
						collectionContainer = (this._collection[i] as CollectionContainer);
						if (collectionContainer != null)
						{
							num2 += collectionContainer.ViewCount - 1;
						}
					}
					if (args.Action == NotifyCollectionChangedAction.Add)
					{
						if (this._count >= 0)
						{
							this._count++;
						}
						this.UpdateCurrencyAfterAdd(num2, args.NewStartingIndex, true);
					}
					else if (args.Action == NotifyCollectionChangedAction.Remove)
					{
						if (this._count >= 0)
						{
							this._count--;
						}
						this.UpdateCurrencyAfterRemove(num2, args.OldStartingIndex, true);
					}
					args = new NotifyCollectionChangedEventArgs(args.Action, obj, num2);
				}
				else
				{
					if (args.Action == NotifyCollectionChangedAction.Add)
					{
						if (this._count >= 0)
						{
							this._count += collectionContainer.ViewCount;
						}
					}
					else if (this._count >= 0)
					{
						this._count -= collectionContainer.ViewCount;
					}
					if (num <= this._currentPositionX)
					{
						if (args.Action == NotifyCollectionChangedAction.Add)
						{
							this._currentPositionX++;
							this.SetCurrentPositionFromXY(this._currentPositionX, this._currentPositionY);
						}
						else
						{
							Invariant.Assert(args.Action == NotifyCollectionChangedAction.Remove);
							if (num == this._currentPositionX)
							{
								flag = true;
							}
							else
							{
								this._currentPositionX--;
								this.SetCurrentPositionFromXY(this._currentPositionX, this._currentPositionY);
							}
						}
					}
					args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
				}
				break;
			}
			case NotifyCollectionChangedAction.Replace:
			{
				CollectionContainer collectionContainer2 = args.NewItems[0] as CollectionContainer;
				CollectionContainer collectionContainer3 = args.OldItems[0] as CollectionContainer;
				int num3 = args.OldStartingIndex;
				if (collectionContainer2 == null && collectionContainer3 == null)
				{
					for (int j = num3 - 1; j >= 0; j--)
					{
						CollectionContainer collectionContainer4 = this._collection[j] as CollectionContainer;
						if (collectionContainer4 != null)
						{
							num3 += collectionContainer4.ViewCount - 1;
						}
					}
					if (num3 == this.CurrentPosition)
					{
						flag = true;
					}
					args = new NotifyCollectionChangedEventArgs(args.Action, args.NewItems, args.OldItems, num3);
				}
				else
				{
					if (this._count >= 0)
					{
						this._count -= ((collectionContainer3 == null) ? 1 : collectionContainer3.ViewCount);
						this._count += ((collectionContainer2 == null) ? 1 : collectionContainer2.ViewCount);
					}
					if (num3 < this._currentPositionX)
					{
						this.SetCurrentPositionFromXY(this._currentPositionX, this._currentPositionY);
					}
					else if (num3 == this._currentPositionX)
					{
						flag = true;
					}
					args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
				}
				break;
			}
			case NotifyCollectionChangedAction.Move:
			{
				CollectionContainer collectionContainer5 = args.OldItems[0] as CollectionContainer;
				int num4 = args.OldStartingIndex;
				int num5 = args.NewStartingIndex;
				if (collectionContainer5 == null)
				{
					for (int k = num4 - 1; k >= 0; k--)
					{
						CollectionContainer collectionContainer6 = this._collection[k] as CollectionContainer;
						if (collectionContainer6 != null)
						{
							num4 += collectionContainer6.ViewCount - 1;
						}
					}
					for (int l = num5 - 1; l >= 0; l--)
					{
						CollectionContainer collectionContainer7 = this._collection[l] as CollectionContainer;
						if (collectionContainer7 != null)
						{
							num5 += collectionContainer7.ViewCount - 1;
						}
					}
					if (num4 == this.CurrentPosition)
					{
						flag = true;
					}
					else if (num5 <= this.CurrentPosition && num4 > this.CurrentPosition)
					{
						this.UpdateCurrencyAfterAdd(num5, args.NewStartingIndex, true);
					}
					else if (num4 < this.CurrentPosition && num5 >= this.CurrentPosition)
					{
						this.UpdateCurrencyAfterRemove(num4, args.OldStartingIndex, true);
					}
					args = new NotifyCollectionChangedEventArgs(args.Action, args.OldItems, num5, num4);
				}
				else
				{
					if (num4 == this._currentPositionX)
					{
						flag = true;
					}
					else if (num5 <= this._currentPositionX && num4 > this._currentPositionX)
					{
						this._currentPositionX++;
						this.SetCurrentPositionFromXY(this._currentPositionX, this._currentPositionY);
					}
					else if (num4 < this._currentPositionX && num5 >= this._currentPositionX)
					{
						this._currentPositionX--;
						this.SetCurrentPositionFromXY(this._currentPositionX, this._currentPositionY);
					}
					args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
				}
				break;
			}
			case NotifyCollectionChangedAction.Reset:
				if (this._traceLog != null)
				{
					this._traceLog.Add("ProcessCollectionChanged  action = {0}", new object[]
					{
						args.Action
					});
				}
				if (this._collection.Count != 0)
				{
					throw new InvalidOperationException(SR.Get("CompositeCollectionResetOnlyOnClear"));
				}
				this._count = 0;
				if (this._currentPositionX >= 0)
				{
					base.OnCurrentChanging();
					this.SetCurrentBeforeFirst();
					this.OnCurrentChanged();
					this.OnPropertyChanged("IsCurrentBeforeFirst");
					this.OnPropertyChanged("CurrentPosition");
					this.OnPropertyChanged("CurrentItem");
				}
				break;
			default:
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					args.Action
				}));
			}
			this._version++;
			this.OnCollectionChanged(args);
			if (flag)
			{
				this._currentPositionY = 0;
				this.MoveCurrencyOffDeletedElement();
			}
		}

		// Token: 0x06007453 RID: 29779 RVA: 0x00214418 File Offset: 0x00212618
		internal void OnContainedCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			this.ValidateCollectionChangedEventArgs(args);
			this._count = -1;
			int num = args.OldStartingIndex;
			int num2 = args.NewStartingIndex;
			int num3 = 0;
			int i;
			for (i = 0; i < this._collection.Count; i++)
			{
				CollectionContainer collectionContainer = this._collection[i] as CollectionContainer;
				if (collectionContainer != null)
				{
					if (sender == collectionContainer)
					{
						break;
					}
					num3 += collectionContainer.ViewCount;
				}
				else
				{
					num3++;
				}
			}
			if (args.OldStartingIndex >= 0)
			{
				num += num3;
			}
			if (args.NewStartingIndex >= 0)
			{
				num2 += num3;
			}
			if (i >= this._collection.Count)
			{
				if (this._traceLog != null)
				{
					this._traceLog.Add("Received ContainerCollectionChange from unknown sender {0}  action = {1} old item = {2}, new item = {3}", new object[]
					{
						TraceLog.IdFor(sender),
						args.Action,
						TraceLog.IdFor(args.OldItems[0]),
						TraceLog.IdFor(args.NewItems[0])
					});
					this._traceLog.Add("Unhook CollectionChanged event handler from unknown sender.", new object[0]);
				}
				this.CacheCount(num3);
				return;
			}
			switch (args.Action)
			{
			case NotifyCollectionChangedAction.Add:
				this.TraceContainerCollectionChange(sender, args.Action, null, args.NewItems[0]);
				if (num2 < 0)
				{
					num2 = this.DeduceFlatIndexForAdd((CollectionContainer)sender, i);
				}
				this.UpdateCurrencyAfterAdd(num2, i, false);
				args = new NotifyCollectionChangedEventArgs(args.Action, args.NewItems[0], num2);
				break;
			case NotifyCollectionChangedAction.Remove:
				this.TraceContainerCollectionChange(sender, args.Action, args.OldItems[0], null);
				if (num < 0)
				{
					num = this.DeduceFlatIndexForRemove((CollectionContainer)sender, i, args.OldItems[0]);
				}
				this.UpdateCurrencyAfterRemove(num, i, false);
				args = new NotifyCollectionChangedEventArgs(args.Action, args.OldItems[0], num);
				break;
			case NotifyCollectionChangedAction.Replace:
				this.TraceContainerCollectionChange(sender, args.Action, args.OldItems[0], args.NewItems[0]);
				if (num == this.CurrentPosition)
				{
					this.MoveCurrencyOffDeletedElement();
				}
				args = new NotifyCollectionChangedEventArgs(args.Action, args.NewItems[0], args.OldItems[0], num);
				break;
			case NotifyCollectionChangedAction.Move:
				this.TraceContainerCollectionChange(sender, args.Action, args.OldItems[0], args.NewItems[0]);
				if (num < 0)
				{
					num = this.DeduceFlatIndexForRemove((CollectionContainer)sender, i, args.NewItems[0]);
				}
				if (num2 < 0)
				{
					num2 = this.DeduceFlatIndexForAdd((CollectionContainer)sender, i);
				}
				this.UpdateCurrencyAfterMove(num, num2, i, false);
				args = new NotifyCollectionChangedEventArgs(args.Action, args.OldItems[0], num2, num);
				break;
			case NotifyCollectionChangedAction.Reset:
				if (this._traceLog != null)
				{
					this._traceLog.Add("ContainerCollectionChange from {0}  action = {1}", new object[]
					{
						TraceLog.IdFor(sender),
						args.Action
					});
				}
				this.UpdateCurrencyAfterRefresh(sender);
				break;
			default:
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					args.Action
				}));
			}
			this._version++;
			this.OnCollectionChanged(args);
		}

		// Token: 0x06007454 RID: 29780 RVA: 0x00214758 File Offset: 0x00212958
		internal override bool HasReliableHashCodes()
		{
			int i = 0;
			int count = this._collection.Count;
			while (i < count)
			{
				CollectionContainer collectionContainer = this._collection[i] as CollectionContainer;
				if (collectionContainer != null)
				{
					CollectionView collectionView = collectionContainer.View as CollectionView;
					if (collectionView != null && !collectionView.HasReliableHashCodes())
					{
						return false;
					}
				}
				else if (!HashHelper.HasReliableHashCode(this._collection[i]))
				{
					return false;
				}
				i++;
			}
			return true;
		}

		// Token: 0x06007455 RID: 29781 RVA: 0x002147C1 File Offset: 0x002129C1
		internal override void GetCollectionChangedSources(int level, Action<int, object, bool?, List<string>> format, List<string> sources)
		{
			format(level, this, new bool?(false), sources);
			if (this._collection != null)
			{
				this._collection.GetCollectionChangedSources(level + 1, format, sources);
			}
		}

		// Token: 0x17001BB4 RID: 7092
		// (get) Token: 0x06007456 RID: 29782 RVA: 0x002147EA File Offset: 0x002129EA
		private bool IsCurrentInView
		{
			get
			{
				return 0 <= this._currentPositionX && this._currentPositionX < this._collection.Count;
			}
		}

		// Token: 0x06007457 RID: 29783 RVA: 0x0021480C File Offset: 0x00212A0C
		private int FindItem(object item, bool changeCurrent)
		{
			int i = 0;
			int num = 0;
			int num2 = 0;
			while (i < this._collection.Count)
			{
				CollectionContainer collectionContainer = this._collection[i] as CollectionContainer;
				if (collectionContainer == null)
				{
					if (ItemsControl.EqualsEx(this._collection[i], item))
					{
						break;
					}
					num2++;
				}
				else
				{
					num = collectionContainer.ViewIndexOf(item);
					if (num >= 0)
					{
						num2 += num;
						break;
					}
					num = 0;
					num2 += collectionContainer.ViewCount;
				}
				i++;
			}
			if (i >= this._collection.Count)
			{
				this.CacheCount(num2);
				num2 = -1;
				item = null;
				i = -1;
				num = 0;
			}
			if (changeCurrent && this.CurrentPosition != num2 && base.OKToChangeCurrent())
			{
				object currentItem = this.CurrentItem;
				int currentPosition = this.CurrentPosition;
				bool isCurrentAfterLast = this.IsCurrentAfterLast;
				bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
				base.SetCurrent(item, num2);
				this._currentPositionX = i;
				this._currentPositionY = num;
				this.OnCurrentChanged();
				if (this.IsCurrentAfterLast != isCurrentAfterLast)
				{
					this.OnPropertyChanged("IsCurrentAfterLast");
				}
				if (this.IsCurrentBeforeFirst != isCurrentBeforeFirst)
				{
					this.OnPropertyChanged("IsCurrentBeforeFirst");
				}
				if (currentPosition != this.CurrentPosition)
				{
					this.OnPropertyChanged("CurrentPosition");
				}
				if (currentItem != this.CurrentItem)
				{
					this.OnPropertyChanged("CurrentItem");
				}
			}
			return num2;
		}

		// Token: 0x06007458 RID: 29784 RVA: 0x0021494C File Offset: 0x00212B4C
		private object GetItem(int flatIndex, out int positionX, out int positionY)
		{
			positionY = 0;
			if (flatIndex == -1)
			{
				positionX = -1;
				return null;
			}
			if (this._count >= 0 && flatIndex >= this._count)
			{
				positionX = this._collection.Count;
				return CompositeCollectionView.s_afterLast;
			}
			int num = 0;
			for (int i = 0; i < this._collection.Count; i++)
			{
				CollectionContainer collectionContainer = this._collection[i] as CollectionContainer;
				if (collectionContainer == null)
				{
					if (num == flatIndex)
					{
						positionX = i;
						return this._collection[i];
					}
					num++;
				}
				else if (collectionContainer.Collection != null)
				{
					int num2 = flatIndex - num;
					int viewCount = collectionContainer.ViewCount;
					if (num2 < viewCount)
					{
						positionX = i;
						positionY = num2;
						return collectionContainer.ViewItem(num2);
					}
					num += viewCount;
				}
			}
			this.CacheCount(num);
			positionX = this._collection.Count;
			return CompositeCollectionView.s_afterLast;
		}

		// Token: 0x06007459 RID: 29785 RVA: 0x00214A18 File Offset: 0x00212C18
		private object GetNextItemFromXY(int positionX, int positionY)
		{
			Invariant.Assert(positionY >= 0);
			object result = null;
			while (positionX < this._collection.Count)
			{
				CollectionContainer collectionContainer = this._collection[positionX] as CollectionContainer;
				if (collectionContainer == null)
				{
					result = this._collection[positionX];
					positionY = 0;
					break;
				}
				if (positionY < collectionContainer.ViewCount)
				{
					result = collectionContainer.ViewItem(positionY);
					break;
				}
				positionY = 0;
				positionX++;
			}
			if (positionX < this._collection.Count)
			{
				this._currentPositionX = positionX;
				this._currentPositionY = positionY;
			}
			else
			{
				this._currentPositionX = this._collection.Count;
				this._currentPositionY = 0;
			}
			return result;
		}

		// Token: 0x0600745A RID: 29786 RVA: 0x00214ABC File Offset: 0x00212CBC
		private int CountDeep(int end)
		{
			if (Invariant.Strict)
			{
				Invariant.Assert(end <= this._collection.Count);
			}
			int num = 0;
			for (int i = 0; i < end; i++)
			{
				CollectionContainer collectionContainer = this._collection[i] as CollectionContainer;
				if (collectionContainer == null)
				{
					num++;
				}
				else
				{
					num += collectionContainer.ViewCount;
				}
			}
			return num;
		}

		// Token: 0x0600745B RID: 29787 RVA: 0x00214B1C File Offset: 0x00212D1C
		private void CacheCount(int count)
		{
			bool flag = this._count != count && this._count >= 0;
			this._count = count;
			if (flag)
			{
				this.OnPropertyChanged("Count");
			}
		}

		// Token: 0x0600745C RID: 29788 RVA: 0x00214B58 File Offset: 0x00212D58
		private bool _MoveTo(int proposed)
		{
			int currentPositionX;
			int currentPositionY;
			object item = this.GetItem(proposed, out currentPositionX, out currentPositionY);
			if (proposed != this.CurrentPosition || item != this.CurrentItem)
			{
				Invariant.Assert(this._count < 0 || proposed <= this._count);
				if (base.OKToChangeCurrent())
				{
					object currentItem = this.CurrentItem;
					int currentPosition = this.CurrentPosition;
					bool isCurrentAfterLast = this.IsCurrentAfterLast;
					bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
					this._currentPositionX = currentPositionX;
					this._currentPositionY = currentPositionY;
					if (item == CompositeCollectionView.s_afterLast)
					{
						base.SetCurrent(null, this.Count);
					}
					else
					{
						base.SetCurrent(item, proposed);
					}
					this.OnCurrentChanged();
					if (this.IsCurrentAfterLast != isCurrentAfterLast)
					{
						this.OnPropertyChanged("IsCurrentAfterLast");
					}
					if (this.IsCurrentBeforeFirst != isCurrentBeforeFirst)
					{
						this.OnPropertyChanged("IsCurrentBeforeFirst");
					}
					if (currentPosition != this.CurrentPosition)
					{
						this.OnPropertyChanged("CurrentPosition");
					}
					if (currentItem != this.CurrentItem)
					{
						this.OnPropertyChanged("CurrentItem");
					}
				}
			}
			return this.IsCurrentInView;
		}

		// Token: 0x0600745D RID: 29789 RVA: 0x00214C5C File Offset: 0x00212E5C
		private int DeduceFlatIndexForAdd(CollectionContainer sender, int x)
		{
			int result;
			if (this._currentPositionX > x)
			{
				result = 0;
			}
			else if (this._currentPositionX < x)
			{
				result = this.CurrentPosition + 1;
			}
			else
			{
				object o = sender.ViewItem(this._currentPositionY);
				if (ItemsControl.EqualsEx(this.CurrentItem, o))
				{
					result = this.CurrentPosition + 1;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x0600745E RID: 29790 RVA: 0x00214CB4 File Offset: 0x00212EB4
		private int DeduceFlatIndexForRemove(CollectionContainer sender, int x, object item)
		{
			int result;
			if (this._currentPositionX > x)
			{
				result = 0;
			}
			else if (this._currentPositionX < x)
			{
				result = this.CurrentPosition + 1;
			}
			else if (ItemsControl.EqualsEx(item, this.CurrentItem))
			{
				result = this.CurrentPosition;
			}
			else
			{
				object o = sender.ViewItem(this._currentPositionY);
				if (ItemsControl.EqualsEx(item, o))
				{
					result = this.CurrentPosition + 1;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x0600745F RID: 29791 RVA: 0x00214D20 File Offset: 0x00212F20
		private void UpdateCurrencyAfterAdd(int flatIndex, int positionX, bool isCompositeItem)
		{
			if (flatIndex < 0)
			{
				return;
			}
			if (flatIndex <= this.CurrentPosition)
			{
				int newPosition = this.CurrentPosition + 1;
				if (isCompositeItem)
				{
					this._currentPositionX++;
				}
				else if (positionX == this._currentPositionX)
				{
					this._currentPositionY++;
				}
				base.SetCurrent(this.GetNextItemFromXY(this._currentPositionX, this._currentPositionY), newPosition);
			}
		}

		// Token: 0x06007460 RID: 29792 RVA: 0x00214D88 File Offset: 0x00212F88
		private void UpdateCurrencyAfterRemove(int flatIndex, int positionX, bool isCompositeItem)
		{
			if (flatIndex < 0)
			{
				return;
			}
			if (flatIndex < this.CurrentPosition)
			{
				base.SetCurrent(this.CurrentItem, this.CurrentPosition - 1);
				if (isCompositeItem)
				{
					this._currentPositionX--;
					return;
				}
				if (positionX == this._currentPositionX)
				{
					this._currentPositionY--;
					return;
				}
			}
			else if (flatIndex == this.CurrentPosition)
			{
				this.MoveCurrencyOffDeletedElement();
			}
		}

		// Token: 0x06007461 RID: 29793 RVA: 0x00214DF0 File Offset: 0x00212FF0
		private void UpdateCurrencyAfterMove(int oldIndex, int newIndex, int positionX, bool isCompositeItem)
		{
			if ((oldIndex < this.CurrentPosition && newIndex < this.CurrentPosition) || (oldIndex > this.CurrentPosition && newIndex > this.CurrentPosition))
			{
				return;
			}
			if (newIndex <= this.CurrentPosition)
			{
				this.UpdateCurrencyAfterAdd(newIndex, positionX, isCompositeItem);
			}
			if (oldIndex <= this.CurrentPosition)
			{
				this.UpdateCurrencyAfterRemove(oldIndex, positionX, isCompositeItem);
			}
		}

		// Token: 0x06007462 RID: 29794 RVA: 0x00214E48 File Offset: 0x00213048
		private void UpdateCurrencyAfterRefresh(object refreshedObject)
		{
			Invariant.Assert(refreshedObject is CollectionContainer);
			object currentItem = this.CurrentItem;
			int currentPosition = this.CurrentPosition;
			bool isCurrentAfterLast = this.IsCurrentAfterLast;
			bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
			if (this.IsCurrentInView && refreshedObject == this._collection[this._currentPositionX])
			{
				CollectionContainer collectionContainer = refreshedObject as CollectionContainer;
				if (collectionContainer.ViewCount == 0)
				{
					this._currentPositionY = 0;
					this.MoveCurrencyOffDeletedElement();
				}
				else
				{
					int num = collectionContainer.ViewIndexOf(this.CurrentItem);
					if (num >= 0)
					{
						this._currentPositionY = num;
						this.SetCurrentPositionFromXY(this._currentPositionX, this._currentPositionY);
					}
					else
					{
						base.OnCurrentChanging();
						this.SetCurrentBeforeFirst();
						this.OnCurrentChanged();
					}
				}
			}
			else
			{
				for (int i = 0; i < this._currentPositionX; i++)
				{
					if (this._collection[i] == refreshedObject)
					{
						this.SetCurrentPositionFromXY(this._currentPositionX, this._currentPositionY);
						break;
					}
				}
			}
			if (this.IsCurrentAfterLast != isCurrentAfterLast)
			{
				this.OnPropertyChanged("IsCurrentAfterLast");
			}
			if (this.IsCurrentBeforeFirst != isCurrentBeforeFirst)
			{
				this.OnPropertyChanged("IsCurrentBeforeFirst");
			}
			if (currentPosition != this.CurrentPosition)
			{
				this.OnPropertyChanged("CurrentPosition");
			}
			if (currentItem != this.CurrentItem)
			{
				this.OnPropertyChanged("CurrentItem");
			}
		}

		// Token: 0x06007463 RID: 29795 RVA: 0x00214F88 File Offset: 0x00213188
		private void MoveCurrencyOffDeletedElement()
		{
			int currentPosition = this.CurrentPosition;
			base.OnCurrentChanging();
			object newItem = this.GetNextItemFromXY(this._currentPositionX, this._currentPositionY);
			if (this._currentPositionX >= this._collection.Count)
			{
				newItem = this.GetLastItem(out this._currentPositionX, out this._currentPositionY);
				base.SetCurrent(newItem, this.Count - 1);
			}
			else
			{
				this.SetCurrentPositionFromXY(this._currentPositionX, this._currentPositionY);
				base.SetCurrent(newItem, this.CurrentPosition);
			}
			this.OnCurrentChanged();
			this.OnPropertyChanged("Count");
			this.OnPropertyChanged("CurrentItem");
			if (this.IsCurrentAfterLast)
			{
				this.OnPropertyChanged("IsCurrentAfterLast");
			}
			if (this.IsCurrentBeforeFirst)
			{
				this.OnPropertyChanged("IsCurrentBeforeFirst");
			}
			if (this.CurrentPosition != currentPosition)
			{
				this.OnPropertyChanged("CurrentPosition");
			}
		}

		// Token: 0x06007464 RID: 29796 RVA: 0x00215064 File Offset: 0x00213264
		private object GetLastItem(out int positionX, out int positionY)
		{
			object result = null;
			positionX = -1;
			positionY = 0;
			if (this._count != 0)
			{
				for (positionX = this._collection.Count - 1; positionX >= 0; positionX--)
				{
					CollectionContainer collectionContainer = this._collection[positionX] as CollectionContainer;
					if (collectionContainer == null)
					{
						result = this._collection[positionX];
						break;
					}
					if (collectionContainer.ViewCount > 0)
					{
						positionY = collectionContainer.ViewCount - 1;
						result = collectionContainer.ViewItem(positionY);
						break;
					}
				}
				if (positionX < 0)
				{
					this.CacheCount(0);
				}
			}
			return result;
		}

		// Token: 0x06007465 RID: 29797 RVA: 0x002150EE File Offset: 0x002132EE
		private void SetCurrentBeforeFirst()
		{
			this._currentPositionX = -1;
			this._currentPositionY = 0;
			base.SetCurrent(null, -1);
		}

		// Token: 0x06007466 RID: 29798 RVA: 0x00215106 File Offset: 0x00213306
		private void SetCurrentPositionFromXY(int x, int y)
		{
			if (this.IsCurrentBeforeFirst)
			{
				base.SetCurrent(null, -1);
				return;
			}
			if (this.IsCurrentAfterLast)
			{
				base.SetCurrent(null, this.Count);
				return;
			}
			base.SetCurrent(this.CurrentItem, this.CountDeep(x) + y);
		}

		// Token: 0x06007467 RID: 29799 RVA: 0x00215144 File Offset: 0x00213344
		private void InitializeTraceLog()
		{
			this._traceLog = new TraceLog(20);
		}

		// Token: 0x06007468 RID: 29800 RVA: 0x00215154 File Offset: 0x00213354
		private void TraceContainerCollectionChange(object sender, NotifyCollectionChangedAction action, object oldItem, object newItem)
		{
			if (this._traceLog != null)
			{
				this._traceLog.Add("ContainerCollectionChange from {0}  action = {1} oldItem = {2} newItem = {3}", new object[]
				{
					TraceLog.IdFor(sender),
					action,
					TraceLog.IdFor(oldItem),
					TraceLog.IdFor(newItem)
				});
			}
		}

		// Token: 0x06007469 RID: 29801 RVA: 0x002151A4 File Offset: 0x002133A4
		private void ValidateCollectionChangedEventArgs(NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				if (e.NewItems.Count != 1)
				{
					throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
				}
				break;
			case NotifyCollectionChangedAction.Remove:
				if (e.OldItems.Count != 1)
				{
					throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
				}
				break;
			case NotifyCollectionChangedAction.Replace:
				if (e.NewItems.Count != 1 || e.OldItems.Count != 1)
				{
					throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
				}
				break;
			case NotifyCollectionChangedAction.Move:
				if (e.NewItems.Count != 1)
				{
					throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
				}
				if (e.NewStartingIndex < 0)
				{
					throw new InvalidOperationException(SR.Get("CannotMoveToUnknownPosition"));
				}
				break;
			case NotifyCollectionChangedAction.Reset:
				break;
			default:
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					e.Action
				}));
			}
		}

		// Token: 0x0600746A RID: 29802 RVA: 0x0007D0A8 File Offset: 0x0007B2A8
		private void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x040037CE RID: 14286
		private TraceLog _traceLog;

		// Token: 0x040037CF RID: 14287
		private CompositeCollection _collection;

		// Token: 0x040037D0 RID: 14288
		private int _count = -1;

		// Token: 0x040037D1 RID: 14289
		private int _version;

		// Token: 0x040037D2 RID: 14290
		private int _currentPositionX = -1;

		// Token: 0x040037D3 RID: 14291
		private int _currentPositionY;

		// Token: 0x040037D4 RID: 14292
		private static readonly object s_afterLast = new object();

		// Token: 0x02000B4A RID: 2890
		private class FlatteningEnumerator : IEnumerator, IDisposable
		{
			// Token: 0x06008DA9 RID: 36265 RVA: 0x00259E01 File Offset: 0x00258001
			internal FlatteningEnumerator(CompositeCollection collection, CompositeCollectionView view)
			{
				Invariant.Assert(collection != null && view != null);
				this._collection = collection;
				this._view = view;
				this._version = view._version;
				this.Reset();
			}

			// Token: 0x06008DAA RID: 36266 RVA: 0x00259E38 File Offset: 0x00258038
			public bool MoveNext()
			{
				this.CheckVersion();
				bool result = true;
				object obj;
				for (;;)
				{
					if (this._containerEnumerator != null)
					{
						if (this._containerEnumerator.MoveNext())
						{
							break;
						}
						this.DisposeContainerEnumerator();
					}
					int num = this._index + 1;
					this._index = num;
					if (num >= this._collection.Count)
					{
						goto IL_9A;
					}
					obj = this._collection[this._index];
					CollectionContainer collectionContainer = obj as CollectionContainer;
					if (collectionContainer == null)
					{
						goto IL_91;
					}
					IEnumerable view = collectionContainer.View;
					this._containerEnumerator = ((view != null) ? view.GetEnumerator() : null);
				}
				this._current = this._containerEnumerator.Current;
				return result;
				IL_91:
				this._current = obj;
				return result;
				IL_9A:
				this._current = null;
				this._done = true;
				result = false;
				return result;
			}

			// Token: 0x17001F80 RID: 8064
			// (get) Token: 0x06008DAB RID: 36267 RVA: 0x00259EF0 File Offset: 0x002580F0
			public object Current
			{
				get
				{
					if (this._index < 0)
					{
						throw new InvalidOperationException(SR.Get("EnumeratorNotStarted"));
					}
					if (this._done)
					{
						throw new InvalidOperationException(SR.Get("EnumeratorReachedEnd"));
					}
					return this._current;
				}
			}

			// Token: 0x06008DAC RID: 36268 RVA: 0x00259F29 File Offset: 0x00258129
			public void Reset()
			{
				this.CheckVersion();
				this._index = -1;
				this._current = null;
				this.DisposeContainerEnumerator();
				this._done = false;
			}

			// Token: 0x06008DAD RID: 36269 RVA: 0x00259F4C File Offset: 0x0025814C
			public void Dispose()
			{
				this.DisposeContainerEnumerator();
			}

			// Token: 0x06008DAE RID: 36270 RVA: 0x00259F54 File Offset: 0x00258154
			private void DisposeContainerEnumerator()
			{
				IDisposable disposable = this._containerEnumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
				this._containerEnumerator = null;
			}

			// Token: 0x06008DAF RID: 36271 RVA: 0x00259F80 File Offset: 0x00258180
			private void CheckVersion()
			{
				if (this._isInvalidated || (this._isInvalidated = (this._version != this._view._version)))
				{
					throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
				}
			}

			// Token: 0x04004AD5 RID: 19157
			private CompositeCollection _collection;

			// Token: 0x04004AD6 RID: 19158
			private CompositeCollectionView _view;

			// Token: 0x04004AD7 RID: 19159
			private int _index;

			// Token: 0x04004AD8 RID: 19160
			private object _current;

			// Token: 0x04004AD9 RID: 19161
			private IEnumerator _containerEnumerator;

			// Token: 0x04004ADA RID: 19162
			private bool _done;

			// Token: 0x04004ADB RID: 19163
			private bool _isInvalidated;

			// Token: 0x04004ADC RID: 19164
			private int _version;
		}
	}
}
