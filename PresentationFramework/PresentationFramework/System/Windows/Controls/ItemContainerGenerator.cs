using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Controls;
using MS.Utility;

namespace System.Windows.Controls
{
	/// <summary>Generates the user interface (UI) on behalf of its host, such as an <see cref="T:System.Windows.Controls.ItemsControl" />.</summary>
	// Token: 0x020004F0 RID: 1264
	public sealed class ItemContainerGenerator : IRecyclingItemContainerGenerator, IItemContainerGenerator, IWeakEventListener
	{
		// Token: 0x06004FE1 RID: 20449 RVA: 0x00165E50 File Offset: 0x00164050
		internal ItemContainerGenerator(IGeneratorHost host) : this(null, host, host as DependencyObject, 0)
		{
			CollectionChangedEventManager.AddHandler(host.View, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnCollectionChanged));
		}

		// Token: 0x06004FE2 RID: 20450 RVA: 0x00165E78 File Offset: 0x00164078
		private ItemContainerGenerator(ItemContainerGenerator parent, GroupItem groupItem) : this(parent, parent.Host, groupItem, parent.Level + 1)
		{
		}

		// Token: 0x06004FE3 RID: 20451 RVA: 0x00165E90 File Offset: 0x00164090
		private ItemContainerGenerator(ItemContainerGenerator parent, IGeneratorHost host, DependencyObject peer, int level)
		{
			this._parent = parent;
			this._host = host;
			this._peer = peer;
			this._level = level;
			this.OnRefresh();
		}

		/// <summary>The generation status of the <see cref="T:System.Windows.Controls.ItemContainerGenerator" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Controls.Primitives.GeneratorStatus" /> value that represents the generation status of the <see cref="T:System.Windows.Controls.ItemContainerGenerator" />.</returns>
		// Token: 0x17001381 RID: 4993
		// (get) Token: 0x06004FE4 RID: 20452 RVA: 0x00165EC6 File Offset: 0x001640C6
		public GeneratorStatus Status
		{
			get
			{
				return this._status;
			}
		}

		// Token: 0x06004FE5 RID: 20453 RVA: 0x00165ED0 File Offset: 0x001640D0
		private void SetStatus(GeneratorStatus value)
		{
			if (value != this._status)
			{
				this._status = value;
				GeneratorStatus status = this._status;
				if (status != GeneratorStatus.GeneratingContainers)
				{
					if (status == GeneratorStatus.ContainersGenerated)
					{
						string text = null;
						if (this._itemsGenerated >= 0)
						{
							DependencyObject dependencyObject = this.Host as DependencyObject;
							if (dependencyObject != null)
							{
								text = (string)dependencyObject.GetValue(FrameworkElement.NameProperty);
							}
							if (text == null || text.Length == 0)
							{
								text = this.Host.GetHashCode().ToString(CultureInfo.InvariantCulture);
							}
							EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringEnd, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, string.Format(CultureInfo.InvariantCulture, "ItemContainerGenerator for {0} {1} - {2} items", new object[]
							{
								this.Host.GetType().Name,
								text,
								this._itemsGenerated
							}));
						}
					}
				}
				else if (EventTrace.IsEnabled(EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info))
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringBegin, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "ItemsControl.Generator");
					this._itemsGenerated = 0;
				}
				else
				{
					this._itemsGenerated = int.MinValue;
				}
				if (this.StatusChanged != null)
				{
					this.StatusChanged(this, EventArgs.Empty);
				}
			}
		}

		/// <summary>Gets the collection of items that belong to this <see cref="T:System.Windows.Controls.ItemContainerGenerator" />.</summary>
		/// <returns>The collection of items that belong to this <see cref="T:System.Windows.Controls.ItemContainerGenerator" />.</returns>
		// Token: 0x17001382 RID: 4994
		// (get) Token: 0x06004FE6 RID: 20454 RVA: 0x00165FEF File Offset: 0x001641EF
		public ReadOnlyCollection<object> Items
		{
			get
			{
				if (this._itemsReadOnly == null && this._items != null)
				{
					this._itemsReadOnly = new ReadOnlyCollection<object>(new ListOfObject(this._items));
				}
				return this._itemsReadOnly;
			}
		}

		/// <summary>Returns the ItemContainerGenerator appropriate for use by the specified panel.</summary>
		/// <param name="panel">The panel for which to return an appropriate ItemContainerGenerator.</param>
		/// <returns>An ItemContainerGenerator appropriate for use by the specified panel.</returns>
		// Token: 0x06004FE7 RID: 20455 RVA: 0x00166020 File Offset: 0x00164220
		ItemContainerGenerator IItemContainerGenerator.GetItemContainerGeneratorForPanel(Panel panel)
		{
			if (!panel.IsItemsHost)
			{
				throw new ArgumentException(SR.Get("PanelIsNotItemsHost"), "panel");
			}
			ItemsPresenter itemsPresenter = ItemsPresenter.FromPanel(panel);
			if (itemsPresenter != null)
			{
				return itemsPresenter.Generator;
			}
			if (panel.TemplatedParent != null)
			{
				return this;
			}
			return null;
		}

		/// <summary>Prepares the generator to generate items, starting at the specified GeneratorPosition, and in the specified GeneratorDirection.</summary>
		/// <param name="position">A GeneratorPosition that specifies the position of the item to start generating items at.</param>
		/// <param name="direction">A GeneratorDirection that specifies the direction which to generate items.</param>
		/// <returns>An IDisposable object that tracks the lifetime of the generation process.</returns>
		// Token: 0x06004FE8 RID: 20456 RVA: 0x00166066 File Offset: 0x00164266
		IDisposable IItemContainerGenerator.StartAt(GeneratorPosition position, GeneratorDirection direction)
		{
			return ((IItemContainerGenerator)this).StartAt(position, direction, false);
		}

		/// <summary>Prepares the generator to generate items, starting at the specified GeneratorPosition, and in the specified GeneratorDirection, and controlling whether or not to start at a generated (realized) item.</summary>
		/// <param name="position">A GeneratorPosition that specifies the position of the item to start generating items at.</param>
		/// <param name="direction">A GeneratorDirection that specifies the direction which to generate items.</param>
		/// <param name="allowStartAtRealizedItem">A Boolean that specifies whether to start at a generated (realized) item.</param>
		/// <returns>An IDisposable object that tracks the lifetime of the generation process.</returns>
		// Token: 0x06004FE9 RID: 20457 RVA: 0x00166071 File Offset: 0x00164271
		IDisposable IItemContainerGenerator.StartAt(GeneratorPosition position, GeneratorDirection direction, bool allowStartAtRealizedItem)
		{
			if (this._generator != null)
			{
				throw new InvalidOperationException(SR.Get("GenerationInProgress"));
			}
			this._generator = new ItemContainerGenerator.Generator(this, position, direction, allowStartAtRealizedItem);
			return this._generator;
		}

		/// <summary>Returns an object that manages the <see cref="P:System.Windows.Controls.ItemContainerGenerator.Status" /> property.</summary>
		/// <returns>An object that manages the <see cref="P:System.Windows.Controls.ItemContainerGenerator.Status" /> property.</returns>
		// Token: 0x06004FEA RID: 20458 RVA: 0x001660A0 File Offset: 0x001642A0
		public IDisposable GenerateBatches()
		{
			if (this._isGeneratingBatches)
			{
				throw new InvalidOperationException(SR.Get("GenerationInProgress"));
			}
			return new ItemContainerGenerator.BatchGenerator(this);
		}

		/// <summary>Returns the container element used to display the next item.</summary>
		/// <returns>A DependencyObject that is the container element which is used to display the next item.</returns>
		// Token: 0x06004FEB RID: 20459 RVA: 0x001660C0 File Offset: 0x001642C0
		DependencyObject IItemContainerGenerator.GenerateNext()
		{
			if (this._generator == null)
			{
				throw new InvalidOperationException(SR.Get("GenerationNotInProgress"));
			}
			bool flag;
			return this._generator.GenerateNext(true, out flag);
		}

		/// <summary>Returns the container element used to display the next item, and whether the container element has been newly generated (realized).</summary>
		/// <param name="isNewlyRealized">Is true if the returned DependencyObject is newly generated (realized); otherwise, false.</param>
		/// <returns>A DependencyObject that is the container element which is used to display the next item.</returns>
		// Token: 0x06004FEC RID: 20460 RVA: 0x001660F3 File Offset: 0x001642F3
		DependencyObject IItemContainerGenerator.GenerateNext(out bool isNewlyRealized)
		{
			if (this._generator == null)
			{
				throw new InvalidOperationException(SR.Get("GenerationNotInProgress"));
			}
			return this._generator.GenerateNext(false, out isNewlyRealized);
		}

		/// <summary>Prepares the specified element as the container for the corresponding item.</summary>
		/// <param name="container">The container to prepare. Normally, container is the result of the previous call to GenerateNext.</param>
		// Token: 0x06004FED RID: 20461 RVA: 0x0016611C File Offset: 0x0016431C
		void IItemContainerGenerator.PrepareItemContainer(DependencyObject container)
		{
			object item = container.ReadLocalValue(ItemContainerGenerator.ItemForItemContainerProperty);
			this.Host.PrepareItemContainer(container, item);
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="position">Removes one or more generated (realized) items.</param>
		/// <param name="count">The Int32 number of elements to remove, starting at <paramref name="position" />.</param>
		// Token: 0x06004FEE RID: 20462 RVA: 0x00166142 File Offset: 0x00164342
		void IItemContainerGenerator.Remove(GeneratorPosition position, int count)
		{
			this.Remove(position, count, false);
		}

		// Token: 0x06004FEF RID: 20463 RVA: 0x00166150 File Offset: 0x00164350
		private void Remove(GeneratorPosition position, int count, bool isRecycling)
		{
			if (position.Offset != 0)
			{
				throw new ArgumentException(SR.Get("RemoveRequiresOffsetZero", new object[]
				{
					position.Index,
					position.Offset
				}), "position");
			}
			if (count <= 0)
			{
				throw new ArgumentException(SR.Get("RemoveRequiresPositiveCount", new object[]
				{
					count
				}), "count");
			}
			if (this._itemMap == null)
			{
				return;
			}
			int index = position.Index;
			int num = index;
			ItemContainerGenerator.ItemBlock itemBlock = this._itemMap.Next;
			while (itemBlock != this._itemMap && num >= itemBlock.ContainerCount)
			{
				num -= itemBlock.ContainerCount;
				itemBlock = itemBlock.Next;
			}
			ItemContainerGenerator.RealizedItemBlock realizedItemBlock = itemBlock as ItemContainerGenerator.RealizedItemBlock;
			int num2 = num + count - 1;
			while (itemBlock != this._itemMap)
			{
				if (!(itemBlock is ItemContainerGenerator.RealizedItemBlock))
				{
					throw new InvalidOperationException(SR.Get("CannotRemoveUnrealizedItems", new object[]
					{
						index,
						count
					}));
				}
				if (num2 < itemBlock.ContainerCount)
				{
					break;
				}
				num2 -= itemBlock.ContainerCount;
				itemBlock = itemBlock.Next;
			}
			ItemContainerGenerator.RealizedItemBlock realizedItemBlock2 = itemBlock as ItemContainerGenerator.RealizedItemBlock;
			ItemContainerGenerator.RealizedItemBlock realizedItemBlock3 = realizedItemBlock;
			int num3 = num;
			while (realizedItemBlock3 != realizedItemBlock2 || num3 <= num2)
			{
				DependencyObject dependencyObject = realizedItemBlock3.ContainerAt(num3);
				this.UnlinkContainerFromItem(dependencyObject, realizedItemBlock3.ItemAt(num3));
				bool flag = this._generatesGroupItems && !(dependencyObject is GroupItem);
				if (isRecycling && !flag)
				{
					if (this._containerType == null)
					{
						this._containerType = dependencyObject.GetType();
					}
					else if (this._containerType != dependencyObject.GetType())
					{
						throw new InvalidOperationException(SR.Get("CannotRecyleHeterogeneousTypes"));
					}
					this._recyclableContainers.Enqueue(dependencyObject);
				}
				if (++num3 >= realizedItemBlock3.ContainerCount && realizedItemBlock3 != realizedItemBlock2)
				{
					realizedItemBlock3 = (realizedItemBlock3.Next as ItemContainerGenerator.RealizedItemBlock);
					num3 = 0;
				}
			}
			bool flag2 = num == 0;
			bool flag3 = num2 == realizedItemBlock2.ItemCount - 1;
			bool flag4 = flag2 && realizedItemBlock.Prev is ItemContainerGenerator.UnrealizedItemBlock;
			bool flag5 = flag3 && realizedItemBlock2.Next is ItemContainerGenerator.UnrealizedItemBlock;
			ItemContainerGenerator.ItemBlock itemBlock2 = null;
			ItemContainerGenerator.UnrealizedItemBlock unrealizedItemBlock;
			int num4;
			int num5;
			if (flag4)
			{
				unrealizedItemBlock = (ItemContainerGenerator.UnrealizedItemBlock)realizedItemBlock.Prev;
				num4 = unrealizedItemBlock.ItemCount;
				num5 = -unrealizedItemBlock.ItemCount;
			}
			else if (flag5)
			{
				unrealizedItemBlock = (ItemContainerGenerator.UnrealizedItemBlock)realizedItemBlock2.Next;
				num4 = 0;
				num5 = num;
			}
			else
			{
				unrealizedItemBlock = new ItemContainerGenerator.UnrealizedItemBlock();
				num4 = 0;
				num5 = num;
				itemBlock2 = (flag2 ? realizedItemBlock.Prev : realizedItemBlock);
			}
			for (itemBlock = realizedItemBlock; itemBlock != realizedItemBlock2; itemBlock = itemBlock.Next)
			{
				int itemCount = itemBlock.ItemCount;
				this.MoveItems(itemBlock, num, itemCount - num, unrealizedItemBlock, num4, num5);
				num4 += itemCount - num;
				num = 0;
				num5 -= itemCount;
				if (itemBlock.ItemCount == 0)
				{
					itemBlock.Remove();
				}
			}
			int count2 = itemBlock.ItemCount - 1 - num2;
			this.MoveItems(itemBlock, num, num2 - num + 1, unrealizedItemBlock, num4, num5);
			ItemContainerGenerator.RealizedItemBlock realizedItemBlock4 = realizedItemBlock2;
			if (!flag3)
			{
				if (realizedItemBlock == realizedItemBlock2 && !flag2)
				{
					realizedItemBlock4 = new ItemContainerGenerator.RealizedItemBlock();
				}
				this.MoveItems(itemBlock, num2 + 1, count2, realizedItemBlock4, 0, num2 + 1);
			}
			if (itemBlock2 != null)
			{
				unrealizedItemBlock.InsertAfter(itemBlock2);
			}
			if (realizedItemBlock4 != realizedItemBlock2)
			{
				realizedItemBlock4.InsertAfter(unrealizedItemBlock);
			}
			this.RemoveAndCoalesceBlocksIfNeeded(itemBlock);
		}

		/// <summary>Removes all generated (realized) items.</summary>
		// Token: 0x06004FF0 RID: 20464 RVA: 0x001664A3 File Offset: 0x001646A3
		void IItemContainerGenerator.RemoveAll()
		{
			this.RemoveAllInternal(false);
		}

		// Token: 0x06004FF1 RID: 20465 RVA: 0x001664AC File Offset: 0x001646AC
		internal void RemoveAllInternal(bool saveRecycleQueue)
		{
			ItemContainerGenerator.ItemBlock itemMap = this._itemMap;
			this._itemMap = null;
			try
			{
				if (itemMap != null)
				{
					for (ItemContainerGenerator.ItemBlock next = itemMap.Next; next != itemMap; next = next.Next)
					{
						ItemContainerGenerator.RealizedItemBlock realizedItemBlock = next as ItemContainerGenerator.RealizedItemBlock;
						if (realizedItemBlock != null)
						{
							for (int i = 0; i < realizedItemBlock.ContainerCount; i++)
							{
								this.UnlinkContainerFromItem(realizedItemBlock.ContainerAt(i), realizedItemBlock.ItemAt(i));
							}
						}
					}
				}
			}
			finally
			{
				this.PrepareGrouping();
				this._itemMap = new ItemContainerGenerator.ItemBlock();
				this._itemMap.Prev = (this._itemMap.Next = this._itemMap);
				ItemContainerGenerator.UnrealizedItemBlock unrealizedItemBlock = new ItemContainerGenerator.UnrealizedItemBlock();
				unrealizedItemBlock.InsertAfter(this._itemMap);
				unrealizedItemBlock.ItemCount = this.ItemsInternal.Count;
				if (!saveRecycleQueue)
				{
					this.ResetRecyclableContainers();
				}
				this.SetAlternationCount();
				if (this.MapChanged != null)
				{
					this.MapChanged(null, -1, 0, unrealizedItemBlock, 0, 0);
				}
			}
		}

		// Token: 0x06004FF2 RID: 20466 RVA: 0x001665A4 File Offset: 0x001647A4
		private void ResetRecyclableContainers()
		{
			this._recyclableContainers = new Queue<DependencyObject>();
			this._containerType = null;
			this._generatesGroupItems = false;
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="position">The index of the first element to reuse. <paramref name="position" /> must refer to a previously generated (realized) item.</param>
		/// <param name="count">The number of elements to reuse, starting at <paramref name="position" />.</param>
		// Token: 0x06004FF3 RID: 20467 RVA: 0x001665BF File Offset: 0x001647BF
		void IRecyclingItemContainerGenerator.Recycle(GeneratorPosition position, int count)
		{
			this.Remove(position, count, true);
		}

		/// <summary>Returns the GeneratorPosition object that maps to the item at the specified index.</summary>
		/// <param name="itemIndex">The index of desired item. </param>
		/// <returns>A GeneratorPosition object that maps to the item at the specified index.</returns>
		// Token: 0x06004FF4 RID: 20468 RVA: 0x001665CC File Offset: 0x001647CC
		GeneratorPosition IItemContainerGenerator.GeneratorPositionFromIndex(int itemIndex)
		{
			GeneratorPosition result;
			ItemContainerGenerator.ItemBlock itemBlock;
			int num;
			this.GetBlockAndPosition(itemIndex, out result, out itemBlock, out num);
			if (itemBlock == this._itemMap && result.Index == -1)
			{
				int offset = result.Offset + 1;
				result.Offset = offset;
			}
			return result;
		}

		/// <summary>Returns the index that maps to the specified GeneratorPosition.</summary>
		/// <param name="position">The index of desired item.</param>
		/// <returns>An Int32 that is the index which maps to the specified GeneratorPosition.</returns>
		// Token: 0x06004FF5 RID: 20469 RVA: 0x0016660C File Offset: 0x0016480C
		int IItemContainerGenerator.IndexFromGeneratorPosition(GeneratorPosition position)
		{
			int num = position.Index;
			if (num != -1)
			{
				if (this._itemMap != null)
				{
					int num2 = 0;
					for (ItemContainerGenerator.ItemBlock next = this._itemMap.Next; next != this._itemMap; next = next.Next)
					{
						if (num < next.ContainerCount)
						{
							return num2 + num + position.Offset;
						}
						num2 += next.ItemCount;
						num -= next.ContainerCount;
					}
				}
				return -1;
			}
			if (position.Offset >= 0)
			{
				return position.Offset - 1;
			}
			return this.ItemsInternal.Count + position.Offset;
		}

		/// <summary>Returns the item that corresponds to the specified, generated <see cref="T:System.Windows.UIElement" />.</summary>
		/// <param name="container">The <see cref="T:System.Windows.DependencyObject" /> that corresponds to the item to be returned.</param>
		/// <returns>A <see cref="T:System.Windows.DependencyObject" /> that is the item which corresponds to the specified, generated <see cref="T:System.Windows.UIElement" />. If the <see cref="T:System.Windows.UIElement" /> has not been generated, <see cref="F:System.Windows.DependencyProperty.UnsetValue" /> is returned. </returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="container" /> is <see langword="null" />.</exception>
		// Token: 0x06004FF6 RID: 20470 RVA: 0x001666A0 File Offset: 0x001648A0
		public object ItemFromContainer(DependencyObject container)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			object obj = container.ReadLocalValue(ItemContainerGenerator.ItemForItemContainerProperty);
			if (obj != DependencyProperty.UnsetValue && !this.Host.IsHostForItemContainer(container))
			{
				obj = DependencyProperty.UnsetValue;
			}
			return obj;
		}

		/// <summary>Returns the <see cref="T:System.Windows.UIElement" /> corresponding to the given item.</summary>
		/// <param name="item">The <see cref="T:System.Object" /> item to find the <see cref="T:System.Windows.UIElement" /> for.</param>
		/// <returns>A <see cref="T:System.Windows.UIElement" /> that corresponds to the given item. Returns <see langword="null" /> if the item does not belong to the item collection, or if a <see cref="T:System.Windows.UIElement" /> has not been generated for it.</returns>
		// Token: 0x06004FF7 RID: 20471 RVA: 0x001666E4 File Offset: 0x001648E4
		public DependencyObject ContainerFromItem(object item)
		{
			object obj;
			DependencyObject result;
			int num;
			this.DoLinearSearch((object o, DependencyObject d) => ItemsControl.EqualsEx(o, item), out obj, out result, out num, false);
			return result;
		}

		/// <summary>Returns the index to an item that corresponds to the specified, generated <see cref="T:System.Windows.UIElement" />. </summary>
		/// <param name="container">The <see cref="T:System.Windows.DependencyObject" /> that corresponds to the item to the index to be returned.</param>
		/// <returns>An <see cref="T:System.Int32" /> index to an item that corresponds to the specified, generated <see cref="T:System.Windows.UIElement" /> or -1 if <paramref name="container" /> is not found. </returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="container" /> is <see langword="null" />.</exception>
		// Token: 0x06004FF8 RID: 20472 RVA: 0x00166719 File Offset: 0x00164919
		public int IndexFromContainer(DependencyObject container)
		{
			return this.IndexFromContainer(container, false);
		}

		/// <summary>Returns the index to an item that corresponds to the specified, generated <see cref="T:System.Windows.UIElement" />, optionally recursively searching hierarchical items.</summary>
		/// <param name="container">The <see cref="T:System.Windows.DependencyObject" /> that corresponds to the item to the index to be returned.</param>
		/// <param name="returnLocalIndex">
		///       <see langword="true" /> to search the current level of hierarchical items; <see langword="false" /> to recursively search hierarchical items.</param>
		/// <returns>An <see cref="T:System.Int32" /> index to an item that corresponds to the specified, generated <see cref="T:System.Windows.UIElement" /> or -1 if <paramref name="container" /> is not found.</returns>
		// Token: 0x06004FF9 RID: 20473 RVA: 0x00166724 File Offset: 0x00164924
		public int IndexFromContainer(DependencyObject container, bool returnLocalIndex)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			object obj;
			DependencyObject dependencyObject;
			int result;
			this.DoLinearSearch((object o, DependencyObject d) => d == container, out obj, out dependencyObject, out result, returnLocalIndex);
			return result;
		}

		// Token: 0x06004FFA RID: 20474 RVA: 0x0016676C File Offset: 0x0016496C
		internal bool FindItem(Func<object, DependencyObject, bool> match, out DependencyObject container, out int itemIndex)
		{
			object obj;
			return this.DoLinearSearch(match, out obj, out container, out itemIndex, false);
		}

		// Token: 0x06004FFB RID: 20475 RVA: 0x00166788 File Offset: 0x00164988
		private bool DoLinearSearch(Func<object, DependencyObject, bool> match, out object item, out DependencyObject container, out int itemIndex, bool returnLocalIndex)
		{
			item = null;
			container = null;
			itemIndex = 0;
			if (this._itemMap != null)
			{
				ItemContainerGenerator.ItemBlock itemBlock = this._itemMap.Next;
				int num = 0;
				while (num <= this._startIndexForUIFromItem && itemBlock != this._itemMap)
				{
					num += itemBlock.ItemCount;
					itemBlock = itemBlock.Next;
				}
				itemBlock = itemBlock.Prev;
				num -= itemBlock.ItemCount;
				ItemContainerGenerator.RealizedItemBlock realizedItemBlock = itemBlock as ItemContainerGenerator.RealizedItemBlock;
				int num2;
				if (realizedItemBlock != null)
				{
					num2 = this._startIndexForUIFromItem - num;
					if (num2 >= realizedItemBlock.ItemCount)
					{
						num2 = 0;
					}
				}
				else
				{
					num2 = 0;
				}
				ItemContainerGenerator.ItemBlock itemBlock2 = itemBlock;
				int i = num2;
				int num3 = itemBlock.ItemCount;
				for (;;)
				{
					if (realizedItemBlock != null)
					{
						while (i < num3)
						{
							bool flag = match(realizedItemBlock.ItemAt(i), realizedItemBlock.ContainerAt(i));
							if (flag)
							{
								item = realizedItemBlock.ItemAt(i);
								container = realizedItemBlock.ContainerAt(i);
							}
							else if (!returnLocalIndex && this.IsGrouping && realizedItemBlock.ItemAt(i) is CollectionViewGroup)
							{
								GroupItem groupItem = (GroupItem)realizedItemBlock.ContainerAt(i);
								int num4;
								flag = groupItem.Generator.DoLinearSearch(match, out item, out container, out num4, false);
								if (flag)
								{
									itemIndex = num4;
								}
							}
							if (flag)
							{
								goto Block_11;
							}
							i++;
						}
						if (itemBlock2 == itemBlock && i == num2)
						{
							goto IL_1A3;
						}
					}
					num += itemBlock2.ItemCount;
					i = 0;
					itemBlock2 = itemBlock2.Next;
					if (itemBlock2 == this._itemMap)
					{
						itemBlock2 = itemBlock2.Next;
						num = 0;
					}
					num3 = itemBlock2.ItemCount;
					realizedItemBlock = (itemBlock2 as ItemContainerGenerator.RealizedItemBlock);
					if (itemBlock2 == itemBlock)
					{
						if (realizedItemBlock == null)
						{
							goto IL_1A3;
						}
						num3 = num2;
					}
				}
				Block_11:
				this._startIndexForUIFromItem = num + i;
				itemIndex += this.GetRealizedItemBlockCount(realizedItemBlock, i, returnLocalIndex) + this.GetCount(itemBlock2, returnLocalIndex);
				return true;
			}
			IL_1A3:
			itemIndex = -1;
			item = null;
			container = null;
			return false;
		}

		// Token: 0x06004FFC RID: 20476 RVA: 0x00166943 File Offset: 0x00164B43
		private int GetCount()
		{
			return this.GetCount(this._itemMap);
		}

		// Token: 0x06004FFD RID: 20477 RVA: 0x00166951 File Offset: 0x00164B51
		private int GetCount(ItemContainerGenerator.ItemBlock stop)
		{
			return this.GetCount(stop, false);
		}

		// Token: 0x06004FFE RID: 20478 RVA: 0x0016695C File Offset: 0x00164B5C
		private int GetCount(ItemContainerGenerator.ItemBlock stop, bool returnLocalIndex)
		{
			if (this._itemMap == null)
			{
				return 0;
			}
			int num = 0;
			ItemContainerGenerator.ItemBlock itemMap = this._itemMap;
			for (ItemContainerGenerator.ItemBlock next = itemMap.Next; next != stop; next = next.Next)
			{
				num += next.ItemCount;
			}
			if (!returnLocalIndex && this.IsGrouping)
			{
				int num2 = num;
				num = 0;
				for (int i = 0; i < num2; i++)
				{
					CollectionViewGroup collectionViewGroup = this.Items[i] as CollectionViewGroup;
					num += ((collectionViewGroup == null) ? 1 : collectionViewGroup.ItemCount);
				}
			}
			return num;
		}

		// Token: 0x06004FFF RID: 20479 RVA: 0x001669E0 File Offset: 0x00164BE0
		private int GetRealizedItemBlockCount(ItemContainerGenerator.RealizedItemBlock rib, int end, bool returnLocalIndex)
		{
			if (!this.IsGrouping || returnLocalIndex)
			{
				return end;
			}
			int num = 0;
			for (int i = 0; i < end; i++)
			{
				CollectionViewGroup collectionViewGroup;
				if ((collectionViewGroup = (rib.ItemAt(i) as CollectionViewGroup)) != null)
				{
					num += collectionViewGroup.ItemCount;
				}
				else
				{
					num++;
				}
			}
			return num;
		}

		/// <summary>Returns the element corresponding to the item at the given index within the <see cref="T:System.Windows.Controls.ItemCollection" />.</summary>
		/// <param name="index">The index of the desired item.</param>
		/// <returns>Returns the element corresponding to the item at the given index within the <see cref="T:System.Windows.Controls.ItemCollection" /> or returns <see langword="null" /> if the item is not realized.</returns>
		// Token: 0x06005000 RID: 20480 RVA: 0x00166A2C File Offset: 0x00164C2C
		public DependencyObject ContainerFromIndex(int index)
		{
			if (this._itemMap == null)
			{
				return null;
			}
			int num = 0;
			if (this.IsGrouping)
			{
				num = index;
				index = 0;
				int count = this.ItemsInternal.Count;
				while (index < count)
				{
					CollectionViewGroup collectionViewGroup = this.ItemsInternal[index] as CollectionViewGroup;
					int num2 = (collectionViewGroup == null) ? 1 : collectionViewGroup.ItemCount;
					if (num < num2)
					{
						break;
					}
					num -= num2;
					index++;
				}
			}
			for (ItemContainerGenerator.ItemBlock next = this._itemMap.Next; next != this._itemMap; next = next.Next)
			{
				if (index < next.ItemCount)
				{
					DependencyObject dependencyObject = next.ContainerAt(index);
					GroupItem groupItem = dependencyObject as GroupItem;
					if (groupItem != null)
					{
						dependencyObject = groupItem.Generator.ContainerFromIndex(num);
					}
					return dependencyObject;
				}
				index -= next.ItemCount;
			}
			return null;
		}

		/// <summary>The <see cref="E:System.Windows.Controls.ItemContainerGenerator.ItemsChanged" /> event is raised by a <see cref="T:System.Windows.Controls.ItemContainerGenerator" /> to inform layouts that the items collection has changed.</summary>
		// Token: 0x140000F0 RID: 240
		// (add) Token: 0x06005001 RID: 20481 RVA: 0x00166AF4 File Offset: 0x00164CF4
		// (remove) Token: 0x06005002 RID: 20482 RVA: 0x00166B2C File Offset: 0x00164D2C
		public event ItemsChangedEventHandler ItemsChanged;

		/// <summary>The <see cref="E:System.Windows.Controls.ItemContainerGenerator.StatusChanged" /> event is raised by a <see cref="T:System.Windows.Controls.ItemContainerGenerator" /> to inform controls that its status has changed. </summary>
		// Token: 0x140000F1 RID: 241
		// (add) Token: 0x06005003 RID: 20483 RVA: 0x00166B64 File Offset: 0x00164D64
		// (remove) Token: 0x06005004 RID: 20484 RVA: 0x00166B9C File Offset: 0x00164D9C
		public event EventHandler StatusChanged;

		// Token: 0x17001383 RID: 4995
		// (get) Token: 0x06005005 RID: 20485 RVA: 0x00166BD1 File Offset: 0x00164DD1
		internal IEnumerable RecyclableContainers
		{
			get
			{
				return this._recyclableContainers;
			}
		}

		// Token: 0x06005006 RID: 20486 RVA: 0x00166BD9 File Offset: 0x00164DD9
		internal void Refresh()
		{
			this.OnRefresh();
		}

		// Token: 0x06005007 RID: 20487 RVA: 0x00166BE1 File Offset: 0x00164DE1
		internal void Release()
		{
			((IItemContainerGenerator)this).RemoveAll();
		}

		// Token: 0x06005008 RID: 20488 RVA: 0x00166BEC File Offset: 0x00164DEC
		internal void Verify()
		{
			if (this._itemMap == null)
			{
				return;
			}
			List<string> list = new List<string>();
			int num = 0;
			for (ItemContainerGenerator.ItemBlock next = this._itemMap.Next; next != this._itemMap; next = next.Next)
			{
				num += next.ItemCount;
			}
			if (num != this._items.Count)
			{
				list.Add(SR.Get("Generator_CountIsWrong", new object[]
				{
					num,
					this._items.Count
				}));
			}
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			for (ItemContainerGenerator.ItemBlock next2 = this._itemMap.Next; next2 != this._itemMap; next2 = next2.Next)
			{
				ItemContainerGenerator.RealizedItemBlock realizedItemBlock = next2 as ItemContainerGenerator.RealizedItemBlock;
				if (realizedItemBlock != null)
				{
					for (int i = 0; i < realizedItemBlock.ItemCount; i++)
					{
						int num5 = num4 + i;
						object obj = realizedItemBlock.ItemAt(i);
						object obj2 = (num5 < this._items.Count) ? this._items[num5] : null;
						if (!ItemsControl.EqualsEx(obj, obj2))
						{
							if (num3 < 3)
							{
								list.Add(SR.Get("Generator_ItemIsWrong", new object[]
								{
									num5,
									obj,
									obj2
								}));
								num3++;
							}
							num2++;
						}
					}
				}
				num4 += next2.ItemCount;
			}
			if (num2 > num3)
			{
				list.Add(SR.Get("Generator_MoreErrors", new object[]
				{
					num2 - num3
				}));
			}
			if (list.Count > 0)
			{
				CultureInfo invariantEnglishUS = TypeConverterHelper.InvariantEnglishUS;
				DependencyObject peer = this.Peer;
				string text = (string)peer.GetValue(FrameworkElement.NameProperty);
				if (string.IsNullOrWhiteSpace(text))
				{
					text = SR.Get("Generator_Unnamed");
				}
				List<string> list2 = new List<string>();
				this.GetCollectionChangedSources(0, new Action<int, object, bool?, List<string>>(this.FormatCollectionChangedSource), list2);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(SR.Get("Generator_Readme0"));
				stringBuilder.Append(SR.Get("Generator_Readme1", new object[]
				{
					peer,
					text
				}));
				stringBuilder.Append("  ");
				stringBuilder.AppendLine(SR.Get("Generator_Readme2"));
				foreach (string text2 in list)
				{
					stringBuilder.AppendFormat(invariantEnglishUS, "  {0}", new object[]
					{
						text2
					});
					stringBuilder.AppendLine();
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(SR.Get("Generator_Readme3"));
				foreach (string text3 in list2)
				{
					stringBuilder.AppendFormat(invariantEnglishUS, "  {0}", new object[]
					{
						text3
					});
					stringBuilder.AppendLine();
				}
				stringBuilder.AppendLine(SR.Get("Generator_Readme4"));
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(SR.Get("Generator_Readme5"));
				stringBuilder.AppendLine();
				stringBuilder.Append(SR.Get("Generator_Readme6"));
				stringBuilder.Append("  ");
				stringBuilder.Append(SR.Get("Generator_Readme7", new object[]
				{
					"PresentationTraceSources.TraceLevel",
					"High"
				}));
				stringBuilder.Append("  ");
				stringBuilder.AppendLine(SR.Get("Generator_Readme8", new object[]
				{
					"System.Diagnostics.PresentationTraceSources.SetTraceLevel(myItemsControl.ItemContainerGenerator, System.Diagnostics.PresentationTraceLevel.High)"
				}));
				stringBuilder.AppendLine(SR.Get("Generator_Readme9"));
				Exception innerException = new Exception(stringBuilder.ToString());
				throw new InvalidOperationException(SR.Get("Generator_Inconsistent"), innerException);
			}
		}

		// Token: 0x06005009 RID: 20489 RVA: 0x00166FDC File Offset: 0x001651DC
		private void FormatCollectionChangedSource(int level, object source, bool? isLikely, List<string> sources)
		{
			Type type = source.GetType();
			if (isLikely == null)
			{
				isLikely = new bool?(true);
				string assemblyQualifiedName = type.AssemblyQualifiedName;
				int num = assemblyQualifiedName.LastIndexOf("PublicKeyToken=");
				if (num >= 0)
				{
					string strA = assemblyQualifiedName.Substring(num + "PublicKeyToken=".Length);
					if (string.Compare(strA, "31bf3856ad364e35", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(strA, "b77a5c561934e089", StringComparison.OrdinalIgnoreCase) == 0)
					{
						isLikely = new bool?(false);
					}
				}
			}
			char c = (isLikely == true) ? '*' : ' ';
			string text = new string(' ', level);
			sources.Add(string.Format(TypeConverterHelper.InvariantEnglishUS, "{0} {1} {2}", new object[]
			{
				c,
				text,
				type.FullName
			}));
		}

		// Token: 0x0600500A RID: 20490 RVA: 0x001670B4 File Offset: 0x001652B4
		private void GetCollectionChangedSources(int level, Action<int, object, bool?, List<string>> format, List<string> sources)
		{
			format(level, this, new bool?(false), sources);
			this.Host.View.GetCollectionChangedSources(level + 1, format, sources);
		}

		// Token: 0x0600500B RID: 20491 RVA: 0x001670DC File Offset: 0x001652DC
		internal void ChangeAlternationCount()
		{
			if (this._itemMap == null)
			{
				return;
			}
			this.SetAlternationCount();
			if (this.IsGrouping && this.GroupStyle != null)
			{
				for (ItemContainerGenerator.ItemBlock next = this._itemMap.Next; next != this._itemMap; next = next.Next)
				{
					for (int i = 0; i < next.ContainerCount; i++)
					{
						GroupItem groupItem = ((ItemContainerGenerator.RealizedItemBlock)next).ContainerAt(i) as GroupItem;
						if (groupItem != null)
						{
							groupItem.Generator.ChangeAlternationCount();
						}
					}
				}
			}
		}

		// Token: 0x0600500C RID: 20492 RVA: 0x00167158 File Offset: 0x00165358
		private void ChangeAlternationCount(int newAlternationCount)
		{
			if (this._alternationCount == newAlternationCount)
			{
				return;
			}
			ItemContainerGenerator.ItemBlock next = this._itemMap.Next;
			int i = 0;
			while (i == next.ContainerCount)
			{
				next = next.Next;
			}
			if (next != this._itemMap)
			{
				if (newAlternationCount > 0)
				{
					this._alternationCount = newAlternationCount;
					this.SetAlternationIndex((ItemContainerGenerator.RealizedItemBlock)next, i, GeneratorDirection.Forward);
				}
				else if (this._alternationCount > 0)
				{
					while (next != this._itemMap)
					{
						for (i = 0; i < next.ContainerCount; i++)
						{
							ItemsControl.ClearAlternationIndex(((ItemContainerGenerator.RealizedItemBlock)next).ContainerAt(i));
						}
						next = next.Next;
					}
				}
			}
			this._alternationCount = newAlternationCount;
		}

		// Token: 0x17001384 RID: 4996
		// (get) Token: 0x0600500D RID: 20493 RVA: 0x001671F7 File Offset: 0x001653F7
		internal ItemContainerGenerator Parent
		{
			get
			{
				return this._parent;
			}
		}

		// Token: 0x17001385 RID: 4997
		// (get) Token: 0x0600500E RID: 20494 RVA: 0x001671FF File Offset: 0x001653FF
		internal int Level
		{
			get
			{
				return this._level;
			}
		}

		// Token: 0x17001386 RID: 4998
		// (get) Token: 0x0600500F RID: 20495 RVA: 0x00167207 File Offset: 0x00165407
		// (set) Token: 0x06005010 RID: 20496 RVA: 0x00167210 File Offset: 0x00165410
		internal GroupStyle GroupStyle
		{
			get
			{
				return this._groupStyle;
			}
			set
			{
				if (this._groupStyle != value)
				{
					if (this._groupStyle != null)
					{
						PropertyChangedEventManager.RemoveHandler(this._groupStyle, new EventHandler<PropertyChangedEventArgs>(this.OnGroupStylePropertyChanged), string.Empty);
					}
					this._groupStyle = value;
					if (this._groupStyle != null)
					{
						PropertyChangedEventManager.AddHandler(this._groupStyle, new EventHandler<PropertyChangedEventArgs>(this.OnGroupStylePropertyChanged), string.Empty);
					}
				}
			}
		}

		// Token: 0x17001387 RID: 4999
		// (get) Token: 0x06005011 RID: 20497 RVA: 0x00167275 File Offset: 0x00165475
		// (set) Token: 0x06005012 RID: 20498 RVA: 0x00167280 File Offset: 0x00165480
		internal IList ItemsInternal
		{
			get
			{
				return this._items;
			}
			set
			{
				if (this._items != value)
				{
					INotifyCollectionChanged notifyCollectionChanged = this._items as INotifyCollectionChanged;
					if (this._items != this.Host.View && notifyCollectionChanged != null)
					{
						CollectionChangedEventManager.RemoveHandler(notifyCollectionChanged, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnCollectionChanged));
					}
					this._items = value;
					this._itemsReadOnly = null;
					notifyCollectionChanged = (this._items as INotifyCollectionChanged);
					if (this._items != this.Host.View && notifyCollectionChanged != null)
					{
						CollectionChangedEventManager.AddHandler(notifyCollectionChanged, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnCollectionChanged));
					}
				}
			}
		}

		// Token: 0x140000F2 RID: 242
		// (add) Token: 0x06005013 RID: 20499 RVA: 0x0016730C File Offset: 0x0016550C
		// (remove) Token: 0x06005014 RID: 20500 RVA: 0x00167344 File Offset: 0x00165544
		internal event EventHandler PanelChanged;

		// Token: 0x06005015 RID: 20501 RVA: 0x00167379 File Offset: 0x00165579
		internal void OnPanelChanged()
		{
			if (this.PanelChanged != null)
			{
				this.PanelChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x17001388 RID: 5000
		// (get) Token: 0x06005016 RID: 20502 RVA: 0x00167394 File Offset: 0x00165594
		private IGeneratorHost Host
		{
			get
			{
				return this._host;
			}
		}

		// Token: 0x17001389 RID: 5001
		// (get) Token: 0x06005017 RID: 20503 RVA: 0x0016739C File Offset: 0x0016559C
		private DependencyObject Peer
		{
			get
			{
				return this._peer;
			}
		}

		// Token: 0x1700138A RID: 5002
		// (get) Token: 0x06005018 RID: 20504 RVA: 0x001673A4 File Offset: 0x001655A4
		private bool IsGrouping
		{
			get
			{
				return this.ItemsInternal != this.Host.View;
			}
		}

		// Token: 0x06005019 RID: 20505 RVA: 0x001673BC File Offset: 0x001655BC
		private void MoveToPosition(GeneratorPosition position, GeneratorDirection direction, bool allowStartAtRealizedItem, ref ItemContainerGenerator.GeneratorState state)
		{
			ItemContainerGenerator.ItemBlock itemBlock = this._itemMap;
			if (itemBlock == null)
			{
				return;
			}
			int num = 0;
			if (position.Index != -1)
			{
				int num2 = 0;
				int i = position.Index;
				itemBlock = itemBlock.Next;
				while (i >= itemBlock.ContainerCount)
				{
					num2 += itemBlock.ItemCount;
					i -= itemBlock.ContainerCount;
					num += itemBlock.ItemCount;
					itemBlock = itemBlock.Next;
				}
				state.Block = itemBlock;
				state.Offset = i;
				state.Count = num2;
				state.ItemIndex = num + i;
			}
			else
			{
				state.Block = itemBlock;
				state.Offset = 0;
				state.Count = 0;
				state.ItemIndex = num - 1;
			}
			int j = position.Offset;
			if (j == 0 && (!allowStartAtRealizedItem || state.Block == this._itemMap))
			{
				j = ((direction == GeneratorDirection.Forward) ? 1 : -1);
			}
			if (j > 0)
			{
				state.Block.MoveForward(ref state, true);
				for (j--; j > 0; j -= state.Block.MoveForward(ref state, allowStartAtRealizedItem, j))
				{
				}
				return;
			}
			if (j < 0)
			{
				if (state.Block == this._itemMap)
				{
					state.ItemIndex = (state.Count = this.ItemsInternal.Count);
				}
				state.Block.MoveBackward(ref state, true);
				for (j++; j < 0; j += state.Block.MoveBackward(ref state, allowStartAtRealizedItem, -j))
				{
				}
			}
		}

		// Token: 0x0600501A RID: 20506 RVA: 0x00167520 File Offset: 0x00165720
		private void Realize(ItemContainerGenerator.UnrealizedItemBlock block, int offset, object item, DependencyObject container)
		{
			ItemContainerGenerator.RealizedItemBlock realizedItemBlock;
			ItemContainerGenerator.RealizedItemBlock realizedItemBlock2;
			int num;
			ItemContainerGenerator.RealizedItemBlock realizedItemBlock3;
			if (offset == 0 && (realizedItemBlock = (block.Prev as ItemContainerGenerator.RealizedItemBlock)) != null && realizedItemBlock.ItemCount < 16)
			{
				realizedItemBlock2 = realizedItemBlock;
				num = realizedItemBlock.ItemCount;
				this.MoveItems(block, offset, 1, realizedItemBlock2, num, -realizedItemBlock.ItemCount);
				this.MoveItems(block, 1, block.ItemCount, block, 0, 1);
			}
			else if (offset == block.ItemCount - 1 && (realizedItemBlock3 = (block.Next as ItemContainerGenerator.RealizedItemBlock)) != null && realizedItemBlock3.ItemCount < 16)
			{
				realizedItemBlock2 = realizedItemBlock3;
				num = 0;
				this.MoveItems(realizedItemBlock2, 0, realizedItemBlock2.ItemCount, realizedItemBlock2, 1, -1);
				this.MoveItems(block, offset, 1, realizedItemBlock2, num, offset);
			}
			else
			{
				realizedItemBlock2 = new ItemContainerGenerator.RealizedItemBlock();
				num = 0;
				if (offset == 0)
				{
					realizedItemBlock2.InsertBefore(block);
					this.MoveItems(block, offset, 1, realizedItemBlock2, num, 0);
					this.MoveItems(block, 1, block.ItemCount, block, 0, 1);
				}
				else if (offset == block.ItemCount - 1)
				{
					realizedItemBlock2.InsertAfter(block);
					this.MoveItems(block, offset, 1, realizedItemBlock2, num, offset);
				}
				else
				{
					ItemContainerGenerator.UnrealizedItemBlock unrealizedItemBlock = new ItemContainerGenerator.UnrealizedItemBlock();
					unrealizedItemBlock.InsertAfter(block);
					realizedItemBlock2.InsertAfter(block);
					this.MoveItems(block, offset + 1, block.ItemCount - offset - 1, unrealizedItemBlock, 0, offset + 1);
					this.MoveItems(block, offset, 1, realizedItemBlock2, 0, offset);
				}
			}
			this.RemoveAndCoalesceBlocksIfNeeded(block);
			realizedItemBlock2.RealizeItem(num, item, container);
		}

		// Token: 0x0600501B RID: 20507 RVA: 0x00167668 File Offset: 0x00165868
		private void RemoveAndCoalesceBlocksIfNeeded(ItemContainerGenerator.ItemBlock block)
		{
			if (block != null && block != this._itemMap && block.ItemCount == 0)
			{
				block.Remove();
				if (block.Prev is ItemContainerGenerator.UnrealizedItemBlock && block.Next is ItemContainerGenerator.UnrealizedItemBlock)
				{
					this.MoveItems(block.Next, 0, block.Next.ItemCount, block.Prev, block.Prev.ItemCount, -block.Prev.ItemCount - 1);
					block.Next.Remove();
				}
			}
		}

		// Token: 0x0600501C RID: 20508 RVA: 0x001676EC File Offset: 0x001658EC
		private void MoveItems(ItemContainerGenerator.ItemBlock block, int offset, int count, ItemContainerGenerator.ItemBlock newBlock, int newOffset, int deltaCount)
		{
			ItemContainerGenerator.RealizedItemBlock realizedItemBlock = block as ItemContainerGenerator.RealizedItemBlock;
			ItemContainerGenerator.RealizedItemBlock realizedItemBlock2 = newBlock as ItemContainerGenerator.RealizedItemBlock;
			if (realizedItemBlock != null && realizedItemBlock2 != null)
			{
				realizedItemBlock2.CopyEntries(realizedItemBlock, offset, count, newOffset);
			}
			else if (realizedItemBlock != null && realizedItemBlock.ItemCount > count)
			{
				realizedItemBlock.ClearEntries(offset, count);
			}
			block.ItemCount -= count;
			newBlock.ItemCount += count;
			if (this.MapChanged != null)
			{
				this.MapChanged(block, offset, count, newBlock, newOffset, deltaCount);
			}
		}

		// Token: 0x0600501D RID: 20509 RVA: 0x00167768 File Offset: 0x00165968
		private void SetAlternationIndex(ItemContainerGenerator.ItemBlock block, int offset, GeneratorDirection direction)
		{
			if (this._alternationCount <= 0)
			{
				return;
			}
			if (direction != GeneratorDirection.Backward)
			{
				offset--;
				while (offset < 0 || block is ItemContainerGenerator.UnrealizedItemBlock)
				{
					block = block.Prev;
					offset = block.ContainerCount - 1;
				}
				ItemContainerGenerator.RealizedItemBlock realizedItemBlock = block as ItemContainerGenerator.RealizedItemBlock;
				int num = (block == this._itemMap) ? -1 : ItemsControl.GetAlternationIndex(realizedItemBlock.ContainerAt(offset));
				for (;;)
				{
					for (offset++; offset == block.ContainerCount; offset = 0)
					{
						block = block.Next;
					}
					if (block == this._itemMap)
					{
						break;
					}
					num = (num + 1) % this._alternationCount;
					realizedItemBlock = (block as ItemContainerGenerator.RealizedItemBlock);
					ItemsControl.SetAlternationIndex(realizedItemBlock.ContainerAt(offset), num);
				}
			}
			else
			{
				offset++;
				while (offset >= block.ContainerCount || block is ItemContainerGenerator.UnrealizedItemBlock)
				{
					block = block.Next;
					offset = 0;
				}
				ItemContainerGenerator.RealizedItemBlock realizedItemBlock = block as ItemContainerGenerator.RealizedItemBlock;
				int num = (block == this._itemMap) ? 1 : ItemsControl.GetAlternationIndex(realizedItemBlock.ContainerAt(offset));
				for (;;)
				{
					for (offset--; offset < 0; offset = block.ContainerCount - 1)
					{
						block = block.Prev;
					}
					if (block == this._itemMap)
					{
						break;
					}
					num = (this._alternationCount + num - 1) % this._alternationCount;
					realizedItemBlock = (block as ItemContainerGenerator.RealizedItemBlock);
					ItemsControl.SetAlternationIndex(realizedItemBlock.ContainerAt(offset), num);
				}
			}
		}

		// Token: 0x0600501E RID: 20510 RVA: 0x001678A4 File Offset: 0x00165AA4
		private DependencyObject ContainerForGroup(CollectionViewGroup group)
		{
			this._generatesGroupItems = true;
			if (!this.ShouldHide(group))
			{
				GroupItem groupItem = new GroupItem();
				ItemContainerGenerator.LinkContainerToItem(groupItem, group);
				groupItem.Generator = new ItemContainerGenerator(this, groupItem);
				return groupItem;
			}
			this.AddEmptyGroupItem(group);
			return null;
		}

		// Token: 0x0600501F RID: 20511 RVA: 0x001678E8 File Offset: 0x00165AE8
		private void PrepareGrouping()
		{
			GroupStyle groupStyle;
			IList list;
			if (this.Level == 0)
			{
				groupStyle = this.Host.GetGroupStyle(null, 0);
				if (groupStyle == null)
				{
					list = this.Host.View;
				}
				else
				{
					CollectionView collectionView = this.Host.View.CollectionView;
					list = ((collectionView == null) ? null : collectionView.Groups);
					if (list == null)
					{
						list = this.Host.View;
						if (list.Count > 0)
						{
							groupStyle = null;
						}
					}
				}
			}
			else
			{
				GroupItem groupItem = (GroupItem)this.Peer;
				CollectionViewGroup collectionViewGroup = groupItem.ReadLocalValue(ItemContainerGenerator.ItemForItemContainerProperty) as CollectionViewGroup;
				if (collectionViewGroup != null)
				{
					if (collectionViewGroup.IsBottomLevel)
					{
						groupStyle = null;
					}
					else
					{
						groupStyle = this.Host.GetGroupStyle(collectionViewGroup, this.Level);
					}
					list = collectionViewGroup.Items;
				}
				else
				{
					groupStyle = null;
					list = this.Host.View;
				}
			}
			this.GroupStyle = groupStyle;
			this.ItemsInternal = list;
			if (this.Level == 0 && this.Host != null)
			{
				this.Host.SetIsGrouping(this.IsGrouping);
			}
		}

		// Token: 0x06005020 RID: 20512 RVA: 0x001679E4 File Offset: 0x00165BE4
		private void SetAlternationCount()
		{
			int alternationCount;
			if (this.IsGrouping && this.GroupStyle != null)
			{
				if (this.GroupStyle.IsAlternationCountSet)
				{
					alternationCount = this.GroupStyle.AlternationCount;
				}
				else if (this._parent != null)
				{
					alternationCount = this._parent._alternationCount;
				}
				else
				{
					alternationCount = this.Host.AlternationCount;
				}
			}
			else
			{
				alternationCount = this.Host.AlternationCount;
			}
			this.ChangeAlternationCount(alternationCount);
		}

		// Token: 0x06005021 RID: 20513 RVA: 0x00167A53 File Offset: 0x00165C53
		private bool ShouldHide(CollectionViewGroup group)
		{
			return this.GroupStyle.HidesIfEmpty && group.ItemCount == 0;
		}

		// Token: 0x06005022 RID: 20514 RVA: 0x00167A70 File Offset: 0x00165C70
		private void AddEmptyGroupItem(CollectionViewGroup group)
		{
			ItemContainerGenerator.EmptyGroupItem emptyGroupItem = new ItemContainerGenerator.EmptyGroupItem();
			ItemContainerGenerator.LinkContainerToItem(emptyGroupItem, group);
			emptyGroupItem.SetGenerator(new ItemContainerGenerator(this, emptyGroupItem));
			if (this._emptyGroupItems == null)
			{
				this._emptyGroupItems = new ArrayList();
			}
			this._emptyGroupItems.Add(emptyGroupItem);
		}

		// Token: 0x06005023 RID: 20515 RVA: 0x00167AB8 File Offset: 0x00165CB8
		private void OnSubgroupBecameNonEmpty(ItemContainerGenerator.EmptyGroupItem groupItem, CollectionViewGroup group)
		{
			this.UnlinkContainerFromItem(groupItem, group);
			if (this._emptyGroupItems != null)
			{
				this._emptyGroupItems.Remove(groupItem);
			}
			if (this.ItemsChanged != null)
			{
				GeneratorPosition position = this.PositionFromIndex(this.ItemsInternal.IndexOf(group));
				this.ItemsChanged(this, new ItemsChangedEventArgs(NotifyCollectionChangedAction.Add, position, 1, 0));
			}
		}

		// Token: 0x06005024 RID: 20516 RVA: 0x00167B14 File Offset: 0x00165D14
		private void OnSubgroupBecameEmpty(CollectionViewGroup group)
		{
			if (this.ShouldHide(group))
			{
				GeneratorPosition position = this.PositionFromIndex(this.ItemsInternal.IndexOf(group));
				if (position.Offset == 0 && position.Index >= 0)
				{
					((IItemContainerGenerator)this).Remove(position, 1);
					if (this.ItemsChanged != null)
					{
						this.ItemsChanged(this, new ItemsChangedEventArgs(NotifyCollectionChangedAction.Remove, position, 1, 1));
					}
					this.AddEmptyGroupItem(group);
				}
			}
		}

		// Token: 0x06005025 RID: 20517 RVA: 0x00167B7C File Offset: 0x00165D7C
		private GeneratorPosition PositionFromIndex(int itemIndex)
		{
			GeneratorPosition result;
			ItemContainerGenerator.ItemBlock itemBlock;
			int num;
			this.GetBlockAndPosition(itemIndex, out result, out itemBlock, out num);
			return result;
		}

		// Token: 0x06005026 RID: 20518 RVA: 0x00167B97 File Offset: 0x00165D97
		private void GetBlockAndPosition(object item, int itemIndex, bool deletedFromItems, out GeneratorPosition position, out ItemContainerGenerator.ItemBlock block, out int offsetFromBlockStart, out int correctIndex)
		{
			if (itemIndex >= 0)
			{
				this.GetBlockAndPosition(itemIndex, out position, out block, out offsetFromBlockStart);
				correctIndex = itemIndex;
				return;
			}
			this.GetBlockAndPosition(item, deletedFromItems, out position, out block, out offsetFromBlockStart, out correctIndex);
		}

		// Token: 0x06005027 RID: 20519 RVA: 0x00167BC0 File Offset: 0x00165DC0
		private void GetBlockAndPosition(int itemIndex, out GeneratorPosition position, out ItemContainerGenerator.ItemBlock block, out int offsetFromBlockStart)
		{
			position = new GeneratorPosition(-1, 0);
			block = null;
			offsetFromBlockStart = itemIndex;
			if (this._itemMap == null || itemIndex < 0)
			{
				return;
			}
			int num = 0;
			block = this._itemMap.Next;
			while (block != this._itemMap)
			{
				if (offsetFromBlockStart >= block.ItemCount)
				{
					num += block.ContainerCount;
					offsetFromBlockStart -= block.ItemCount;
					block = block.Next;
				}
				else
				{
					if (block.ContainerCount > 0)
					{
						position = new GeneratorPosition(num + offsetFromBlockStart, 0);
						return;
					}
					position = new GeneratorPosition(num - 1, offsetFromBlockStart + 1);
					return;
				}
			}
		}

		// Token: 0x06005028 RID: 20520 RVA: 0x00167C68 File Offset: 0x00165E68
		private void GetBlockAndPosition(object item, bool deletedFromItems, out GeneratorPosition position, out ItemContainerGenerator.ItemBlock block, out int offsetFromBlockStart, out int correctIndex)
		{
			correctIndex = 0;
			int num = 0;
			offsetFromBlockStart = 0;
			int num2 = deletedFromItems ? 1 : 0;
			position = new GeneratorPosition(-1, 0);
			if (this._itemMap == null)
			{
				block = null;
				return;
			}
			for (block = this._itemMap.Next; block != this._itemMap; block = block.Next)
			{
				ItemContainerGenerator.RealizedItemBlock realizedItemBlock = block as ItemContainerGenerator.RealizedItemBlock;
				if (realizedItemBlock != null)
				{
					offsetFromBlockStart = realizedItemBlock.OffsetOfItem(item);
					if (offsetFromBlockStart >= 0)
					{
						position = new GeneratorPosition(num + offsetFromBlockStart, 0);
						correctIndex += offsetFromBlockStart;
						break;
					}
				}
				else if (block is ItemContainerGenerator.UnrealizedItemBlock)
				{
					bool flag = false;
					realizedItemBlock = (block.Next as ItemContainerGenerator.RealizedItemBlock);
					if (realizedItemBlock != null && realizedItemBlock.ContainerCount > 0)
					{
						flag = ItemsControl.EqualsEx(realizedItemBlock.ItemAt(0), this.ItemsInternal[correctIndex + block.ItemCount - num2]);
					}
					else if (block.Next == this._itemMap)
					{
						flag = (block.Prev == this._itemMap || this.ItemsInternal.Count == correctIndex + block.ItemCount - num2);
					}
					if (flag)
					{
						offsetFromBlockStart = 0;
						position = new GeneratorPosition(num - 1, 1);
						break;
					}
				}
				correctIndex += block.ItemCount;
				num += block.ContainerCount;
			}
			if (block == this._itemMap)
			{
				throw new InvalidOperationException(SR.Get("CannotFindRemovedItem"));
			}
		}

		// Token: 0x06005029 RID: 20521 RVA: 0x00167DEF File Offset: 0x00165FEF
		internal static void LinkContainerToItem(DependencyObject container, object item)
		{
			container.ClearValue(ItemContainerGenerator.ItemForItemContainerProperty);
			container.SetValue(ItemContainerGenerator.ItemForItemContainerProperty, item);
			if (container != item)
			{
				container.SetValue(FrameworkElement.DataContextProperty, item);
			}
		}

		// Token: 0x0600502A RID: 20522 RVA: 0x00167E18 File Offset: 0x00166018
		private void UnlinkContainerFromItem(DependencyObject container, object item)
		{
			ItemContainerGenerator.UnlinkContainerFromItem(container, item, this._host);
		}

		// Token: 0x0600502B RID: 20523 RVA: 0x00167E28 File Offset: 0x00166028
		internal static void UnlinkContainerFromItem(DependencyObject container, object item, IGeneratorHost host)
		{
			container.ClearValue(ItemContainerGenerator.ItemForItemContainerProperty);
			host.ClearContainerForItem(container, item);
			if (container != item)
			{
				DependencyProperty dataContextProperty = FrameworkElement.DataContextProperty;
				container.SetValue(dataContextProperty, BindingExpressionBase.DisconnectedItem);
			}
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="managerType">The type of the <see cref="T:System.Windows.WeakEventManager" /> calling this method.</param>
		/// <param name="sender">Object that originated the event.</param>
		/// <param name="e">Event data.</param>
		/// <returns>
		///     <see langword="true" /> if the listener handled the event.</returns>
		// Token: 0x0600502C RID: 20524 RVA: 0x0000B02A File Offset: 0x0000922A
		bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return false;
		}

		// Token: 0x0600502D RID: 20525 RVA: 0x00167E5E File Offset: 0x0016605E
		private void OnGroupStylePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Panel")
			{
				this.OnPanelChanged();
				return;
			}
			this.OnRefresh();
		}

		// Token: 0x0600502E RID: 20526 RVA: 0x00167E7F File Offset: 0x0016607F
		private void ValidateAndCorrectIndex(object item, ref int index)
		{
			if (index < 0)
			{
				index = this.ItemsInternal.IndexOf(item);
				if (index < 0)
				{
					throw new InvalidOperationException(SR.Get("CollectionAddEventMissingItem", new object[]
					{
						item
					}));
				}
			}
		}

		// Token: 0x0600502F RID: 20527 RVA: 0x00167EB4 File Offset: 0x001660B4
		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			if (sender != this.ItemsInternal && args.Action != NotifyCollectionChangedAction.Reset)
			{
				return;
			}
			switch (args.Action)
			{
			case NotifyCollectionChangedAction.Add:
				if (args.NewItems.Count != 1)
				{
					throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
				}
				this.OnItemAdded(args.NewItems[0], args.NewStartingIndex);
				break;
			case NotifyCollectionChangedAction.Remove:
				if (args.OldItems.Count != 1)
				{
					throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
				}
				this.OnItemRemoved(args.OldItems[0], args.OldStartingIndex);
				break;
			case NotifyCollectionChangedAction.Replace:
				if (!FrameworkCompatibilityPreferences.TargetsDesktop_V4_0 && args.OldItems.Count != 1)
				{
					throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
				}
				this.OnItemReplaced(args.OldItems[0], args.NewItems[0], args.NewStartingIndex);
				break;
			case NotifyCollectionChangedAction.Move:
				if (!FrameworkCompatibilityPreferences.TargetsDesktop_V4_0 && args.OldItems.Count != 1)
				{
					throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
				}
				this.OnItemMoved(args.OldItems[0], args.OldStartingIndex, args.NewStartingIndex);
				break;
			case NotifyCollectionChangedAction.Reset:
				this.OnRefresh();
				break;
			default:
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					args.Action
				}));
			}
			PresentationTraceLevel traceLevel = PresentationTraceSources.GetTraceLevel(this);
			if (traceLevel >= PresentationTraceLevel.High)
			{
				this.Verify();
			}
		}

		// Token: 0x06005030 RID: 20528 RVA: 0x00168040 File Offset: 0x00166240
		private void OnItemAdded(object item, int index)
		{
			if (this._itemMap == null)
			{
				return;
			}
			this.ValidateAndCorrectIndex(item, ref index);
			GeneratorPosition position = new GeneratorPosition(-1, 0);
			ItemContainerGenerator.ItemBlock itemBlock = this._itemMap.Next;
			int num = index;
			int num2 = 0;
			while (itemBlock != this._itemMap && num >= itemBlock.ItemCount)
			{
				num -= itemBlock.ItemCount;
				position.Index += itemBlock.ContainerCount;
				num2 = ((itemBlock.ContainerCount > 0) ? 0 : (num2 + itemBlock.ItemCount));
				itemBlock = itemBlock.Next;
			}
			position.Offset = num2 + num + 1;
			ItemContainerGenerator.UnrealizedItemBlock unrealizedItemBlock = itemBlock as ItemContainerGenerator.UnrealizedItemBlock;
			if (unrealizedItemBlock != null)
			{
				this.MoveItems(unrealizedItemBlock, num, 1, unrealizedItemBlock, num + 1, 0);
				ItemContainerGenerator.UnrealizedItemBlock unrealizedItemBlock2 = unrealizedItemBlock;
				int itemCount = unrealizedItemBlock2.ItemCount + 1;
				unrealizedItemBlock2.ItemCount = itemCount;
			}
			else if ((num == 0 || itemBlock == this._itemMap) && (unrealizedItemBlock = (itemBlock.Prev as ItemContainerGenerator.UnrealizedItemBlock)) != null)
			{
				ItemContainerGenerator.UnrealizedItemBlock unrealizedItemBlock3 = unrealizedItemBlock;
				int itemCount = unrealizedItemBlock3.ItemCount + 1;
				unrealizedItemBlock3.ItemCount = itemCount;
			}
			else
			{
				unrealizedItemBlock = new ItemContainerGenerator.UnrealizedItemBlock();
				unrealizedItemBlock.ItemCount = 1;
				ItemContainerGenerator.RealizedItemBlock realizedItemBlock;
				if (num > 0 && (realizedItemBlock = (itemBlock as ItemContainerGenerator.RealizedItemBlock)) != null)
				{
					ItemContainerGenerator.RealizedItemBlock realizedItemBlock2 = new ItemContainerGenerator.RealizedItemBlock();
					this.MoveItems(realizedItemBlock, num, realizedItemBlock.ItemCount - num, realizedItemBlock2, 0, num);
					realizedItemBlock2.InsertAfter(realizedItemBlock);
					position.Index += itemBlock.ContainerCount;
					position.Offset = 1;
					itemBlock = realizedItemBlock2;
				}
				unrealizedItemBlock.InsertBefore(itemBlock);
			}
			if (this.MapChanged != null)
			{
				this.MapChanged(null, index, 1, unrealizedItemBlock, 0, 0);
			}
			if (this.ItemsChanged != null)
			{
				this.ItemsChanged(this, new ItemsChangedEventArgs(NotifyCollectionChangedAction.Add, position, 1, 0));
			}
		}

		// Token: 0x06005031 RID: 20529 RVA: 0x001681D8 File Offset: 0x001663D8
		private void OnItemRemoved(object item, int itemIndex)
		{
			DependencyObject dependencyObject = null;
			int itemUICount = 0;
			GeneratorPosition position;
			ItemContainerGenerator.ItemBlock itemBlock;
			int num;
			int num2;
			this.GetBlockAndPosition(item, itemIndex, true, out position, out itemBlock, out num, out num2);
			ItemContainerGenerator.RealizedItemBlock realizedItemBlock = itemBlock as ItemContainerGenerator.RealizedItemBlock;
			if (realizedItemBlock != null)
			{
				itemUICount = 1;
				dependencyObject = realizedItemBlock.ContainerAt(num);
			}
			this.MoveItems(itemBlock, num + 1, itemBlock.ItemCount - num - 1, itemBlock, num, 0);
			ItemContainerGenerator.ItemBlock itemBlock2 = itemBlock;
			int itemCount = itemBlock2.ItemCount - 1;
			itemBlock2.ItemCount = itemCount;
			if (realizedItemBlock != null)
			{
				this.SetAlternationIndex(itemBlock, num, GeneratorDirection.Forward);
			}
			this.RemoveAndCoalesceBlocksIfNeeded(itemBlock);
			if (this.MapChanged != null)
			{
				this.MapChanged(null, itemIndex, -1, null, 0, 0);
			}
			if (this.ItemsChanged != null)
			{
				this.ItemsChanged(this, new ItemsChangedEventArgs(NotifyCollectionChangedAction.Remove, position, 1, itemUICount));
			}
			if (dependencyObject != null)
			{
				this.UnlinkContainerFromItem(dependencyObject, item);
			}
			if (this.Level > 0 && this.ItemsInternal.Count == 0)
			{
				GroupItem groupItem = (GroupItem)this.Peer;
				CollectionViewGroup collectionViewGroup = groupItem.ReadLocalValue(ItemContainerGenerator.ItemForItemContainerProperty) as CollectionViewGroup;
				if (collectionViewGroup != null)
				{
					this.Parent.OnSubgroupBecameEmpty(collectionViewGroup);
				}
			}
		}

		// Token: 0x06005032 RID: 20530 RVA: 0x001682DC File Offset: 0x001664DC
		private void OnItemReplaced(object oldItem, object newItem, int index)
		{
			GeneratorPosition position;
			ItemContainerGenerator.ItemBlock itemBlock;
			int index2;
			int num;
			this.GetBlockAndPosition(oldItem, index, false, out position, out itemBlock, out index2, out num);
			ItemContainerGenerator.RealizedItemBlock realizedItemBlock = itemBlock as ItemContainerGenerator.RealizedItemBlock;
			if (realizedItemBlock != null)
			{
				DependencyObject dependencyObject = realizedItemBlock.ContainerAt(index2);
				if (oldItem != dependencyObject && !this._host.IsItemItsOwnContainer(newItem))
				{
					realizedItemBlock.RealizeItem(index2, newItem, dependencyObject);
					ItemContainerGenerator.LinkContainerToItem(dependencyObject, newItem);
					this._host.PrepareItemContainer(dependencyObject, newItem);
					return;
				}
				DependencyObject containerForItem = this._host.GetContainerForItem(newItem);
				realizedItemBlock.RealizeItem(index2, newItem, containerForItem);
				ItemContainerGenerator.LinkContainerToItem(containerForItem, newItem);
				if (this.ItemsChanged != null)
				{
					this.ItemsChanged(this, new ItemsChangedEventArgs(NotifyCollectionChangedAction.Replace, position, 1, 1));
				}
				this.UnlinkContainerFromItem(dependencyObject, oldItem);
			}
		}

		// Token: 0x06005033 RID: 20531 RVA: 0x00168390 File Offset: 0x00166590
		private void OnItemMoved(object item, int oldIndex, int newIndex)
		{
			if (this._itemMap == null)
			{
				return;
			}
			DependencyObject dependencyObject = null;
			int itemUICount = 0;
			GeneratorPosition generatorPosition;
			ItemContainerGenerator.ItemBlock itemBlock;
			int num;
			int num2;
			this.GetBlockAndPosition(item, oldIndex, true, out generatorPosition, out itemBlock, out num, out num2);
			GeneratorPosition oldPosition = generatorPosition;
			ItemContainerGenerator.RealizedItemBlock realizedItemBlock = itemBlock as ItemContainerGenerator.RealizedItemBlock;
			if (realizedItemBlock != null)
			{
				itemUICount = 1;
				dependencyObject = realizedItemBlock.ContainerAt(num);
			}
			this.MoveItems(itemBlock, num + 1, itemBlock.ItemCount - num - 1, itemBlock, num, 0);
			ItemContainerGenerator.ItemBlock itemBlock2 = itemBlock;
			int itemCount = itemBlock2.ItemCount - 1;
			itemBlock2.ItemCount = itemCount;
			this.RemoveAndCoalesceBlocksIfNeeded(itemBlock);
			generatorPosition = new GeneratorPosition(-1, 0);
			itemBlock = this._itemMap.Next;
			num = newIndex;
			while (itemBlock != this._itemMap && num >= itemBlock.ItemCount)
			{
				num -= itemBlock.ItemCount;
				if (itemBlock.ContainerCount > 0)
				{
					generatorPosition.Index += itemBlock.ContainerCount;
					generatorPosition.Offset = 0;
				}
				else
				{
					generatorPosition.Offset += itemBlock.ItemCount;
				}
				itemBlock = itemBlock.Next;
			}
			generatorPosition.Offset += num + 1;
			ItemContainerGenerator.UnrealizedItemBlock unrealizedItemBlock = itemBlock as ItemContainerGenerator.UnrealizedItemBlock;
			if (unrealizedItemBlock != null)
			{
				this.MoveItems(unrealizedItemBlock, num, 1, unrealizedItemBlock, num + 1, 0);
				ItemContainerGenerator.UnrealizedItemBlock unrealizedItemBlock2 = unrealizedItemBlock;
				itemCount = unrealizedItemBlock2.ItemCount + 1;
				unrealizedItemBlock2.ItemCount = itemCount;
			}
			else if ((num == 0 || itemBlock == this._itemMap) && (unrealizedItemBlock = (itemBlock.Prev as ItemContainerGenerator.UnrealizedItemBlock)) != null)
			{
				ItemContainerGenerator.UnrealizedItemBlock unrealizedItemBlock3 = unrealizedItemBlock;
				itemCount = unrealizedItemBlock3.ItemCount + 1;
				unrealizedItemBlock3.ItemCount = itemCount;
			}
			else
			{
				unrealizedItemBlock = new ItemContainerGenerator.UnrealizedItemBlock();
				unrealizedItemBlock.ItemCount = 1;
				if (num > 0 && (realizedItemBlock = (itemBlock as ItemContainerGenerator.RealizedItemBlock)) != null)
				{
					ItemContainerGenerator.RealizedItemBlock realizedItemBlock2 = new ItemContainerGenerator.RealizedItemBlock();
					this.MoveItems(realizedItemBlock, num, realizedItemBlock.ItemCount - num, realizedItemBlock2, 0, num);
					realizedItemBlock2.InsertAfter(realizedItemBlock);
					generatorPosition.Index += itemBlock.ContainerCount;
					generatorPosition.Offset = 1;
					num = 0;
					itemBlock = realizedItemBlock2;
				}
				unrealizedItemBlock.InsertBefore(itemBlock);
			}
			DependencyObject parentInternal = VisualTreeHelper.GetParentInternal(dependencyObject);
			if (this.ItemsChanged != null)
			{
				this.ItemsChanged(this, new ItemsChangedEventArgs(NotifyCollectionChangedAction.Move, generatorPosition, oldPosition, 1, itemUICount));
			}
			if (dependencyObject != null)
			{
				if (parentInternal == null || VisualTreeHelper.GetParentInternal(dependencyObject) != parentInternal)
				{
					this.UnlinkContainerFromItem(dependencyObject, item);
				}
				else
				{
					this.Realize(unrealizedItemBlock, num, item, dependencyObject);
				}
			}
			if (this._alternationCount > 0)
			{
				int itemIndex = Math.Min(oldIndex, newIndex);
				this.GetBlockAndPosition(itemIndex, out generatorPosition, out itemBlock, out num);
				this.SetAlternationIndex(itemBlock, num, GeneratorDirection.Forward);
			}
		}

		// Token: 0x06005034 RID: 20532 RVA: 0x001685F4 File Offset: 0x001667F4
		private void OnRefresh()
		{
			((IItemContainerGenerator)this).RemoveAll();
			if (this.ItemsChanged != null)
			{
				GeneratorPosition position = new GeneratorPosition(0, 0);
				this.ItemsChanged(this, new ItemsChangedEventArgs(NotifyCollectionChangedAction.Reset, position, 0, 0));
			}
		}

		// Token: 0x140000F3 RID: 243
		// (add) Token: 0x06005035 RID: 20533 RVA: 0x00168630 File Offset: 0x00166830
		// (remove) Token: 0x06005036 RID: 20534 RVA: 0x00168668 File Offset: 0x00166868
		private event ItemContainerGenerator.MapChangedHandler MapChanged;

		// Token: 0x04002C19 RID: 11289
		internal static readonly DependencyProperty ItemForItemContainerProperty = DependencyProperty.RegisterAttached("ItemForItemContainer", typeof(object), typeof(ItemContainerGenerator), new FrameworkPropertyMetadata(null));

		// Token: 0x04002C1B RID: 11291
		private ItemContainerGenerator.Generator _generator;

		// Token: 0x04002C1C RID: 11292
		private IGeneratorHost _host;

		// Token: 0x04002C1D RID: 11293
		private ItemContainerGenerator.ItemBlock _itemMap;

		// Token: 0x04002C1E RID: 11294
		private GeneratorStatus _status;

		// Token: 0x04002C1F RID: 11295
		private int _itemsGenerated;

		// Token: 0x04002C20 RID: 11296
		private int _startIndexForUIFromItem;

		// Token: 0x04002C21 RID: 11297
		private DependencyObject _peer;

		// Token: 0x04002C22 RID: 11298
		private int _level;

		// Token: 0x04002C23 RID: 11299
		private IList _items;

		// Token: 0x04002C24 RID: 11300
		private ReadOnlyCollection<object> _itemsReadOnly;

		// Token: 0x04002C25 RID: 11301
		private GroupStyle _groupStyle;

		// Token: 0x04002C26 RID: 11302
		private ItemContainerGenerator _parent;

		// Token: 0x04002C27 RID: 11303
		private ArrayList _emptyGroupItems;

		// Token: 0x04002C28 RID: 11304
		private int _alternationCount;

		// Token: 0x04002C29 RID: 11305
		private Type _containerType;

		// Token: 0x04002C2A RID: 11306
		private Queue<DependencyObject> _recyclableContainers = new Queue<DependencyObject>();

		// Token: 0x04002C2B RID: 11307
		private bool _generatesGroupItems;

		// Token: 0x04002C2C RID: 11308
		private bool _isGeneratingBatches;

		// Token: 0x02000999 RID: 2457
		private class Generator : IDisposable
		{
			// Token: 0x060087E9 RID: 34793 RVA: 0x00251124 File Offset: 0x0024F324
			internal Generator(ItemContainerGenerator factory, GeneratorPosition position, GeneratorDirection direction, bool allowStartAtRealizedItem)
			{
				this._factory = factory;
				this._direction = direction;
				this._factory.MapChanged += this.OnMapChanged;
				this._factory.MoveToPosition(position, direction, allowStartAtRealizedItem, ref this._cachedState);
				this._done = (this._factory.ItemsInternal.Count == 0);
				this._factory.SetStatus(GeneratorStatus.GeneratingContainers);
			}

			// Token: 0x060087EA RID: 34794 RVA: 0x00251198 File Offset: 0x0024F398
			public DependencyObject GenerateNext(bool stopAtRealized, out bool isNewlyRealized)
			{
				DependencyObject dependencyObject = null;
				isNewlyRealized = false;
				while (dependencyObject == null)
				{
					ItemContainerGenerator.UnrealizedItemBlock unrealizedItemBlock = this._cachedState.Block as ItemContainerGenerator.UnrealizedItemBlock;
					IList itemsInternal = this._factory.ItemsInternal;
					int itemIndex = this._cachedState.ItemIndex;
					int num = (this._direction == GeneratorDirection.Forward) ? 1 : -1;
					if (this._cachedState.Block == this._factory._itemMap)
					{
						this._done = true;
					}
					if (unrealizedItemBlock == null && stopAtRealized)
					{
						this._done = true;
					}
					if (0 > itemIndex || itemIndex >= itemsInternal.Count)
					{
						this._done = true;
					}
					if (this._done)
					{
						isNewlyRealized = false;
						return null;
					}
					object obj = itemsInternal[itemIndex];
					if (unrealizedItemBlock != null)
					{
						isNewlyRealized = true;
						CollectionViewGroup collectionViewGroup = obj as CollectionViewGroup;
						bool flag = this._factory._generatesGroupItems && collectionViewGroup == null;
						if (this._factory._recyclableContainers.Count > 0 && !this._factory.Host.IsItemItsOwnContainer(obj) && !flag)
						{
							dependencyObject = this._factory._recyclableContainers.Dequeue();
							isNewlyRealized = false;
						}
						else if (collectionViewGroup == null || !this._factory.IsGrouping)
						{
							dependencyObject = this._factory.Host.GetContainerForItem(obj);
						}
						else
						{
							dependencyObject = this._factory.ContainerForGroup(collectionViewGroup);
						}
						if (dependencyObject != null)
						{
							ItemContainerGenerator.LinkContainerToItem(dependencyObject, obj);
							this._factory.Realize(unrealizedItemBlock, this._cachedState.Offset, obj, dependencyObject);
							this._factory.SetAlternationIndex(this._cachedState.Block, this._cachedState.Offset, this._direction);
						}
					}
					else
					{
						isNewlyRealized = false;
						ItemContainerGenerator.RealizedItemBlock realizedItemBlock = (ItemContainerGenerator.RealizedItemBlock)this._cachedState.Block;
						dependencyObject = realizedItemBlock.ContainerAt(this._cachedState.Offset);
					}
					this._cachedState.ItemIndex = itemIndex;
					if (this._direction == GeneratorDirection.Forward)
					{
						this._cachedState.Block.MoveForward(ref this._cachedState, true);
					}
					else
					{
						this._cachedState.Block.MoveBackward(ref this._cachedState, true);
					}
				}
				return dependencyObject;
			}

			// Token: 0x060087EB RID: 34795 RVA: 0x002513A0 File Offset: 0x0024F5A0
			void IDisposable.Dispose()
			{
				if (this._factory != null)
				{
					this._factory.MapChanged -= this.OnMapChanged;
					this._done = true;
					if (!this._factory._isGeneratingBatches)
					{
						this._factory.SetStatus(GeneratorStatus.ContainersGenerated);
					}
					this._factory._generator = null;
					this._factory = null;
				}
				GC.SuppressFinalize(this);
			}

			// Token: 0x060087EC RID: 34796 RVA: 0x00251408 File Offset: 0x0024F608
			private void OnMapChanged(ItemContainerGenerator.ItemBlock block, int offset, int count, ItemContainerGenerator.ItemBlock newBlock, int newOffset, int deltaCount)
			{
				if (block != null)
				{
					if (block == this._cachedState.Block && offset <= this._cachedState.Offset && this._cachedState.Offset < offset + count)
					{
						this._cachedState.Block = newBlock;
						this._cachedState.Offset = this._cachedState.Offset + (newOffset - offset);
						this._cachedState.Count = this._cachedState.Count + deltaCount;
						return;
					}
				}
				else if (offset >= 0)
				{
					if (offset < this._cachedState.Count || (offset == this._cachedState.Count && newBlock != null && newBlock != this._cachedState.Block))
					{
						this._cachedState.Count = this._cachedState.Count + count;
						this._cachedState.ItemIndex = this._cachedState.ItemIndex + count;
						return;
					}
					if (offset < this._cachedState.Count + this._cachedState.Offset)
					{
						this._cachedState.Offset = this._cachedState.Offset + count;
						this._cachedState.ItemIndex = this._cachedState.ItemIndex + count;
						return;
					}
					if (offset == this._cachedState.Count + this._cachedState.Offset)
					{
						if (count > 0)
						{
							this._cachedState.Offset = this._cachedState.Offset + count;
							this._cachedState.ItemIndex = this._cachedState.ItemIndex + count;
							return;
						}
						if (this._cachedState.Offset == this._cachedState.Block.ItemCount)
						{
							this._cachedState.Block = this._cachedState.Block.Next;
							this._cachedState.Offset = 0;
							return;
						}
					}
				}
				else
				{
					this._cachedState.Block = newBlock;
					this._cachedState.Offset = this._cachedState.Offset + this._cachedState.Count;
					this._cachedState.Count = 0;
				}
			}

			// Token: 0x040044E3 RID: 17635
			private ItemContainerGenerator _factory;

			// Token: 0x040044E4 RID: 17636
			private GeneratorDirection _direction;

			// Token: 0x040044E5 RID: 17637
			private bool _done;

			// Token: 0x040044E6 RID: 17638
			private ItemContainerGenerator.GeneratorState _cachedState;
		}

		// Token: 0x0200099A RID: 2458
		private class BatchGenerator : IDisposable
		{
			// Token: 0x060087ED RID: 34797 RVA: 0x002515E6 File Offset: 0x0024F7E6
			public BatchGenerator(ItemContainerGenerator factory)
			{
				this._factory = factory;
				this._factory._isGeneratingBatches = true;
				this._factory.SetStatus(GeneratorStatus.GeneratingContainers);
			}

			// Token: 0x060087EE RID: 34798 RVA: 0x0025160D File Offset: 0x0024F80D
			void IDisposable.Dispose()
			{
				if (this._factory != null)
				{
					this._factory._isGeneratingBatches = false;
					this._factory.SetStatus(GeneratorStatus.ContainersGenerated);
					this._factory = null;
				}
				GC.SuppressFinalize(this);
			}

			// Token: 0x040044E7 RID: 17639
			private ItemContainerGenerator _factory;
		}

		// Token: 0x0200099B RID: 2459
		// (Invoke) Token: 0x060087F0 RID: 34800
		private delegate void MapChangedHandler(ItemContainerGenerator.ItemBlock block, int offset, int count, ItemContainerGenerator.ItemBlock newBlock, int newOffset, int deltaCount);

		// Token: 0x0200099C RID: 2460
		private class ItemBlock
		{
			// Token: 0x17001EAC RID: 7852
			// (get) Token: 0x060087F3 RID: 34803 RVA: 0x0025163C File Offset: 0x0024F83C
			// (set) Token: 0x060087F4 RID: 34804 RVA: 0x00251644 File Offset: 0x0024F844
			public int ItemCount
			{
				get
				{
					return this._count;
				}
				set
				{
					this._count = value;
				}
			}

			// Token: 0x17001EAD RID: 7853
			// (get) Token: 0x060087F5 RID: 34805 RVA: 0x0025164D File Offset: 0x0024F84D
			// (set) Token: 0x060087F6 RID: 34806 RVA: 0x00251655 File Offset: 0x0024F855
			public ItemContainerGenerator.ItemBlock Prev
			{
				get
				{
					return this._prev;
				}
				set
				{
					this._prev = value;
				}
			}

			// Token: 0x17001EAE RID: 7854
			// (get) Token: 0x060087F7 RID: 34807 RVA: 0x0025165E File Offset: 0x0024F85E
			// (set) Token: 0x060087F8 RID: 34808 RVA: 0x00251666 File Offset: 0x0024F866
			public ItemContainerGenerator.ItemBlock Next
			{
				get
				{
					return this._next;
				}
				set
				{
					this._next = value;
				}
			}

			// Token: 0x17001EAF RID: 7855
			// (get) Token: 0x060087F9 RID: 34809 RVA: 0x0025166F File Offset: 0x0024F86F
			public virtual int ContainerCount
			{
				get
				{
					return int.MaxValue;
				}
			}

			// Token: 0x060087FA RID: 34810 RVA: 0x0000C238 File Offset: 0x0000A438
			public virtual DependencyObject ContainerAt(int index)
			{
				return null;
			}

			// Token: 0x060087FB RID: 34811 RVA: 0x0000C238 File Offset: 0x0000A438
			public virtual object ItemAt(int index)
			{
				return null;
			}

			// Token: 0x060087FC RID: 34812 RVA: 0x00251676 File Offset: 0x0024F876
			public void InsertAfter(ItemContainerGenerator.ItemBlock prev)
			{
				this.Next = prev.Next;
				this.Prev = prev;
				this.Prev.Next = this;
				this.Next.Prev = this;
			}

			// Token: 0x060087FD RID: 34813 RVA: 0x002516A3 File Offset: 0x0024F8A3
			public void InsertBefore(ItemContainerGenerator.ItemBlock next)
			{
				this.InsertAfter(next.Prev);
			}

			// Token: 0x060087FE RID: 34814 RVA: 0x002516B1 File Offset: 0x0024F8B1
			public void Remove()
			{
				this.Prev.Next = this.Next;
				this.Next.Prev = this.Prev;
			}

			// Token: 0x060087FF RID: 34815 RVA: 0x002516D8 File Offset: 0x0024F8D8
			public void MoveForward(ref ItemContainerGenerator.GeneratorState state, bool allowMovePastRealizedItem)
			{
				if (this.IsMoveAllowed(allowMovePastRealizedItem))
				{
					state.ItemIndex++;
					int num = state.Offset + 1;
					state.Offset = num;
					if (num >= this.ItemCount)
					{
						state.Block = this.Next;
						state.Offset = 0;
						state.Count += this.ItemCount;
					}
				}
			}

			// Token: 0x06008800 RID: 34816 RVA: 0x0025173C File Offset: 0x0024F93C
			public void MoveBackward(ref ItemContainerGenerator.GeneratorState state, bool allowMovePastRealizedItem)
			{
				if (this.IsMoveAllowed(allowMovePastRealizedItem))
				{
					int num = state.Offset - 1;
					state.Offset = num;
					if (num < 0)
					{
						state.Block = this.Prev;
						state.Offset = state.Block.ItemCount - 1;
						state.Count -= state.Block.ItemCount;
					}
					state.ItemIndex--;
				}
			}

			// Token: 0x06008801 RID: 34817 RVA: 0x002517AC File Offset: 0x0024F9AC
			public int MoveForward(ref ItemContainerGenerator.GeneratorState state, bool allowMovePastRealizedItem, int count)
			{
				if (this.IsMoveAllowed(allowMovePastRealizedItem))
				{
					if (count < this.ItemCount - state.Offset)
					{
						state.Offset += count;
					}
					else
					{
						count = this.ItemCount - state.Offset;
						state.Block = this.Next;
						state.Offset = 0;
						state.Count += this.ItemCount;
					}
					state.ItemIndex += count;
				}
				return count;
			}

			// Token: 0x06008802 RID: 34818 RVA: 0x00251828 File Offset: 0x0024FA28
			public int MoveBackward(ref ItemContainerGenerator.GeneratorState state, bool allowMovePastRealizedItem, int count)
			{
				if (this.IsMoveAllowed(allowMovePastRealizedItem))
				{
					if (count <= state.Offset)
					{
						state.Offset -= count;
					}
					else
					{
						count = state.Offset + 1;
						state.Block = this.Prev;
						state.Offset = state.Block.ItemCount - 1;
						state.Count -= state.Block.ItemCount;
					}
					state.ItemIndex -= count;
				}
				return count;
			}

			// Token: 0x06008803 RID: 34819 RVA: 0x00012630 File Offset: 0x00010830
			protected virtual bool IsMoveAllowed(bool allowMovePastRealizedItem)
			{
				return allowMovePastRealizedItem;
			}

			// Token: 0x040044E8 RID: 17640
			public const int BlockSize = 16;

			// Token: 0x040044E9 RID: 17641
			private int _count;

			// Token: 0x040044EA RID: 17642
			private ItemContainerGenerator.ItemBlock _prev;

			// Token: 0x040044EB RID: 17643
			private ItemContainerGenerator.ItemBlock _next;
		}

		// Token: 0x0200099D RID: 2461
		private class UnrealizedItemBlock : ItemContainerGenerator.ItemBlock
		{
			// Token: 0x17001EB0 RID: 7856
			// (get) Token: 0x06008805 RID: 34821 RVA: 0x0000B02A File Offset: 0x0000922A
			public override int ContainerCount
			{
				get
				{
					return 0;
				}
			}

			// Token: 0x06008806 RID: 34822 RVA: 0x00016748 File Offset: 0x00014948
			protected override bool IsMoveAllowed(bool allowMovePastRealizedItem)
			{
				return true;
			}
		}

		// Token: 0x0200099E RID: 2462
		private class RealizedItemBlock : ItemContainerGenerator.ItemBlock
		{
			// Token: 0x17001EB1 RID: 7857
			// (get) Token: 0x06008808 RID: 34824 RVA: 0x002518AF File Offset: 0x0024FAAF
			public override int ContainerCount
			{
				get
				{
					return base.ItemCount;
				}
			}

			// Token: 0x06008809 RID: 34825 RVA: 0x002518B7 File Offset: 0x0024FAB7
			public override DependencyObject ContainerAt(int index)
			{
				return this._entry[index].Container;
			}

			// Token: 0x0600880A RID: 34826 RVA: 0x002518CA File Offset: 0x0024FACA
			public override object ItemAt(int index)
			{
				return this._entry[index].Item;
			}

			// Token: 0x0600880B RID: 34827 RVA: 0x002518E0 File Offset: 0x0024FAE0
			public void CopyEntries(ItemContainerGenerator.RealizedItemBlock src, int offset, int count, int newOffset)
			{
				if (offset < newOffset)
				{
					for (int i = count - 1; i >= 0; i--)
					{
						this._entry[newOffset + i] = src._entry[offset + i];
					}
					if (src != this)
					{
						src.ClearEntries(offset, count);
						return;
					}
					src.ClearEntries(offset, newOffset - offset);
					return;
				}
				else
				{
					for (int i = 0; i < count; i++)
					{
						this._entry[newOffset + i] = src._entry[offset + i];
					}
					if (src != this)
					{
						src.ClearEntries(offset, count);
						return;
					}
					src.ClearEntries(newOffset + count, offset - newOffset);
					return;
				}
			}

			// Token: 0x0600880C RID: 34828 RVA: 0x0025197C File Offset: 0x0024FB7C
			public void ClearEntries(int offset, int count)
			{
				for (int i = 0; i < count; i++)
				{
					this._entry[offset + i].Item = null;
					this._entry[offset + i].Container = null;
				}
			}

			// Token: 0x0600880D RID: 34829 RVA: 0x002519BD File Offset: 0x0024FBBD
			public void RealizeItem(int index, object item, DependencyObject container)
			{
				this._entry[index].Item = item;
				this._entry[index].Container = container;
			}

			// Token: 0x0600880E RID: 34830 RVA: 0x002519E4 File Offset: 0x0024FBE4
			public int OffsetOfItem(object item)
			{
				for (int i = 0; i < base.ItemCount; i++)
				{
					if (ItemsControl.EqualsEx(this._entry[i].Item, item))
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x040044EC RID: 17644
			private ItemContainerGenerator.BlockEntry[] _entry = new ItemContainerGenerator.BlockEntry[16];
		}

		// Token: 0x0200099F RID: 2463
		private struct BlockEntry
		{
			// Token: 0x17001EB2 RID: 7858
			// (get) Token: 0x06008810 RID: 34832 RVA: 0x00251A33 File Offset: 0x0024FC33
			// (set) Token: 0x06008811 RID: 34833 RVA: 0x00251A3B File Offset: 0x0024FC3B
			public object Item
			{
				get
				{
					return this._item;
				}
				set
				{
					this._item = value;
				}
			}

			// Token: 0x17001EB3 RID: 7859
			// (get) Token: 0x06008812 RID: 34834 RVA: 0x00251A44 File Offset: 0x0024FC44
			// (set) Token: 0x06008813 RID: 34835 RVA: 0x00251A4C File Offset: 0x0024FC4C
			public DependencyObject Container
			{
				get
				{
					return this._container;
				}
				set
				{
					this._container = value;
				}
			}

			// Token: 0x040044ED RID: 17645
			private object _item;

			// Token: 0x040044EE RID: 17646
			private DependencyObject _container;
		}

		// Token: 0x020009A0 RID: 2464
		private struct GeneratorState
		{
			// Token: 0x17001EB4 RID: 7860
			// (get) Token: 0x06008814 RID: 34836 RVA: 0x00251A55 File Offset: 0x0024FC55
			// (set) Token: 0x06008815 RID: 34837 RVA: 0x00251A5D File Offset: 0x0024FC5D
			public ItemContainerGenerator.ItemBlock Block
			{
				get
				{
					return this._block;
				}
				set
				{
					this._block = value;
				}
			}

			// Token: 0x17001EB5 RID: 7861
			// (get) Token: 0x06008816 RID: 34838 RVA: 0x00251A66 File Offset: 0x0024FC66
			// (set) Token: 0x06008817 RID: 34839 RVA: 0x00251A6E File Offset: 0x0024FC6E
			public int Offset
			{
				get
				{
					return this._offset;
				}
				set
				{
					this._offset = value;
				}
			}

			// Token: 0x17001EB6 RID: 7862
			// (get) Token: 0x06008818 RID: 34840 RVA: 0x00251A77 File Offset: 0x0024FC77
			// (set) Token: 0x06008819 RID: 34841 RVA: 0x00251A7F File Offset: 0x0024FC7F
			public int Count
			{
				get
				{
					return this._count;
				}
				set
				{
					this._count = value;
				}
			}

			// Token: 0x17001EB7 RID: 7863
			// (get) Token: 0x0600881A RID: 34842 RVA: 0x00251A88 File Offset: 0x0024FC88
			// (set) Token: 0x0600881B RID: 34843 RVA: 0x00251A90 File Offset: 0x0024FC90
			public int ItemIndex
			{
				get
				{
					return this._itemIndex;
				}
				set
				{
					this._itemIndex = value;
				}
			}

			// Token: 0x040044EF RID: 17647
			private ItemContainerGenerator.ItemBlock _block;

			// Token: 0x040044F0 RID: 17648
			private int _offset;

			// Token: 0x040044F1 RID: 17649
			private int _count;

			// Token: 0x040044F2 RID: 17650
			private int _itemIndex;
		}

		// Token: 0x020009A1 RID: 2465
		private class EmptyGroupItem : GroupItem
		{
			// Token: 0x0600881C RID: 34844 RVA: 0x00251A99 File Offset: 0x0024FC99
			public void SetGenerator(ItemContainerGenerator generator)
			{
				base.Generator = generator;
				generator.ItemsChanged += this.OnItemsChanged;
			}

			// Token: 0x0600881D RID: 34845 RVA: 0x00251AB4 File Offset: 0x0024FCB4
			private void OnItemsChanged(object sender, ItemsChangedEventArgs e)
			{
				CollectionViewGroup collectionViewGroup = (CollectionViewGroup)base.GetValue(ItemContainerGenerator.ItemForItemContainerProperty);
				if (collectionViewGroup.ItemCount > 0)
				{
					ItemContainerGenerator generator = base.Generator;
					generator.ItemsChanged -= this.OnItemsChanged;
					generator.Parent.OnSubgroupBecameNonEmpty(this, collectionViewGroup);
				}
			}
		}
	}
}
