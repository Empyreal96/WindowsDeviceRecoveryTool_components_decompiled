using System;
using System.Collections.Specialized;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.ItemContainerGenerator.ItemsChanged" /> event.</summary>
	// Token: 0x02000594 RID: 1428
	public class ItemsChangedEventArgs : EventArgs
	{
		// Token: 0x06005E54 RID: 24148 RVA: 0x001A7660 File Offset: 0x001A5860
		internal ItemsChangedEventArgs(NotifyCollectionChangedAction action, GeneratorPosition position, GeneratorPosition oldPosition, int itemCount, int itemUICount)
		{
			this._action = action;
			this._position = position;
			this._oldPosition = oldPosition;
			this._itemCount = itemCount;
			this._itemUICount = itemUICount;
		}

		// Token: 0x06005E55 RID: 24149 RVA: 0x001A768D File Offset: 0x001A588D
		internal ItemsChangedEventArgs(NotifyCollectionChangedAction action, GeneratorPosition position, int itemCount, int itemUICount) : this(action, position, new GeneratorPosition(-1, 0), itemCount, itemUICount)
		{
		}

		/// <summary>Gets the action that occurred on the items collection.</summary>
		/// <returns>Returns the action that occurred.</returns>
		// Token: 0x170016C4 RID: 5828
		// (get) Token: 0x06005E56 RID: 24150 RVA: 0x001A76A1 File Offset: 0x001A58A1
		public NotifyCollectionChangedAction Action
		{
			get
			{
				return this._action;
			}
		}

		/// <summary>Gets the position in the collection where the change occurred.</summary>
		/// <returns>Returns a <see cref="T:System.Windows.Controls.Primitives.GeneratorPosition" />.</returns>
		// Token: 0x170016C5 RID: 5829
		// (get) Token: 0x06005E57 RID: 24151 RVA: 0x001A76A9 File Offset: 0x001A58A9
		public GeneratorPosition Position
		{
			get
			{
				return this._position;
			}
		}

		/// <summary>Gets the position in the collection before the change occurred.</summary>
		/// <returns>Returns a <see cref="T:System.Windows.Controls.Primitives.GeneratorPosition" />.</returns>
		// Token: 0x170016C6 RID: 5830
		// (get) Token: 0x06005E58 RID: 24152 RVA: 0x001A76B1 File Offset: 0x001A58B1
		public GeneratorPosition OldPosition
		{
			get
			{
				return this._oldPosition;
			}
		}

		/// <summary>Gets the number of items that were involved in the change.</summary>
		/// <returns>Integer that represents the number of items involved in the change.</returns>
		// Token: 0x170016C7 RID: 5831
		// (get) Token: 0x06005E59 RID: 24153 RVA: 0x001A76B9 File Offset: 0x001A58B9
		public int ItemCount
		{
			get
			{
				return this._itemCount;
			}
		}

		/// <summary>Gets the number of user interface (UI) elements involved in the change.</summary>
		/// <returns>Integer that represents the number of UI elements involved in the change.</returns>
		// Token: 0x170016C8 RID: 5832
		// (get) Token: 0x06005E5A RID: 24154 RVA: 0x001A76C1 File Offset: 0x001A58C1
		public int ItemUICount
		{
			get
			{
				return this._itemUICount;
			}
		}

		// Token: 0x04003052 RID: 12370
		private NotifyCollectionChangedAction _action;

		// Token: 0x04003053 RID: 12371
		private GeneratorPosition _position;

		// Token: 0x04003054 RID: 12372
		private GeneratorPosition _oldPosition;

		// Token: 0x04003055 RID: 12373
		private int _itemCount;

		// Token: 0x04003056 RID: 12374
		private int _itemUICount;
	}
}
