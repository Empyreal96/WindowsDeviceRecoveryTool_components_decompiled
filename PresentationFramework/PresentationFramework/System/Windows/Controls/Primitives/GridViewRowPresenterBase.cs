using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Media;
using MS.Internal.Controls;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents the base class for classes that define the layout for a row of data where different data items are displayed in different columns.</summary>
	// Token: 0x0200058B RID: 1419
	public abstract class GridViewRowPresenterBase : FrameworkElement, IWeakEventListener
	{
		/// <summary>Returns a string representation of a <see cref="T:System.Windows.Controls.Primitives.GridViewRowPresenterBase" /> object.</summary>
		/// <returns>A string that contains the type of the object and the number of columns.</returns>
		// Token: 0x06005E00 RID: 24064 RVA: 0x001A7120 File Offset: 0x001A5320
		public override string ToString()
		{
			return SR.Get("ToStringFormatString_GridViewRowPresenterBase", new object[]
			{
				base.GetType(),
				(this.Columns != null) ? this.Columns.Count : 0
			});
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Controls.GridViewColumnCollection" />. </summary>
		/// <returns>A collection of <see cref="T:System.Windows.Controls.GridViewColumn" /> objects that display data. The default is <see langword="null" />.</returns>
		// Token: 0x170016B1 RID: 5809
		// (get) Token: 0x06005E01 RID: 24065 RVA: 0x001A7159 File Offset: 0x001A5359
		// (set) Token: 0x06005E02 RID: 24066 RVA: 0x001A716B File Offset: 0x001A536B
		public GridViewColumnCollection Columns
		{
			get
			{
				return (GridViewColumnCollection)base.GetValue(GridViewRowPresenterBase.ColumnsProperty);
			}
			set
			{
				base.SetValue(GridViewRowPresenterBase.ColumnsProperty, value);
			}
		}

		/// <summary>Gets an enumerator for the logical children of a row.</summary>
		/// <returns>The <see cref="T:System.Collections.IEnumerator" /> for the logical children of this row. </returns>
		// Token: 0x170016B2 RID: 5810
		// (get) Token: 0x06005E03 RID: 24067 RVA: 0x001A7179 File Offset: 0x001A5379
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				if (this.InternalChildren.Count == 0)
				{
					return EmptyEnumerator.Instance;
				}
				return this.InternalChildren.GetEnumerator();
			}
		}

		/// <summary>Gets the number of visual children for a row. </summary>
		/// <returns>The number of visual children for the current row. </returns>
		// Token: 0x170016B3 RID: 5811
		// (get) Token: 0x06005E04 RID: 24068 RVA: 0x001A7199 File Offset: 0x001A5399
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._uiElementCollection == null)
				{
					return 0;
				}
				return this._uiElementCollection.Count;
			}
		}

		/// <summary>Gets the visual child in the row item at the specified index.</summary>
		/// <param name="index">The index of the child.</param>
		/// <returns>A <see cref="T:System.Windows.Media.Visual" /> object that contains the child at the specified index.</returns>
		// Token: 0x06005E05 RID: 24069 RVA: 0x001A71B0 File Offset: 0x001A53B0
		protected override Visual GetVisualChild(int index)
		{
			if (this._uiElementCollection == null)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._uiElementCollection[index];
		}

		// Token: 0x06005E06 RID: 24070 RVA: 0x001A71E4 File Offset: 0x001A53E4
		internal virtual void OnColumnCollectionChanged(GridViewColumnCollectionChangedEventArgs e)
		{
			if (this.DesiredWidthList != null)
			{
				if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
				{
					if (this.DesiredWidthList.Count > e.ActualIndex)
					{
						this.DesiredWidthList.RemoveAt(e.ActualIndex);
						return;
					}
				}
				else if (e.Action == NotifyCollectionChangedAction.Reset)
				{
					this.DesiredWidthList = null;
				}
			}
		}

		// Token: 0x06005E07 RID: 24071
		internal abstract void OnColumnPropertyChanged(GridViewColumn column, string propertyName);

		// Token: 0x06005E08 RID: 24072 RVA: 0x001A7240 File Offset: 0x001A5440
		internal void EnsureDesiredWidthList()
		{
			GridViewColumnCollection columns = this.Columns;
			if (columns != null)
			{
				int count = columns.Count;
				if (this.DesiredWidthList == null)
				{
					this.DesiredWidthList = new List<double>(count);
				}
				int num = count - this.DesiredWidthList.Count;
				for (int i = 0; i < num; i++)
				{
					this.DesiredWidthList.Add(double.NaN);
				}
			}
		}

		// Token: 0x170016B4 RID: 5812
		// (get) Token: 0x06005E09 RID: 24073 RVA: 0x001A72A0 File Offset: 0x001A54A0
		// (set) Token: 0x06005E0A RID: 24074 RVA: 0x001A72A8 File Offset: 0x001A54A8
		internal List<double> DesiredWidthList
		{
			get
			{
				return this._desiredWidthList;
			}
			private set
			{
				this._desiredWidthList = value;
			}
		}

		// Token: 0x170016B5 RID: 5813
		// (get) Token: 0x06005E0B RID: 24075 RVA: 0x001A72B1 File Offset: 0x001A54B1
		// (set) Token: 0x06005E0C RID: 24076 RVA: 0x001A72B9 File Offset: 0x001A54B9
		internal bool NeedUpdateVisualTree
		{
			get
			{
				return this._needUpdateVisualTree;
			}
			set
			{
				this._needUpdateVisualTree = value;
			}
		}

		// Token: 0x170016B6 RID: 5814
		// (get) Token: 0x06005E0D RID: 24077 RVA: 0x001A72C2 File Offset: 0x001A54C2
		internal UIElementCollection InternalChildren
		{
			get
			{
				if (this._uiElementCollection == null)
				{
					this._uiElementCollection = new UIElementCollection(this, this);
				}
				return this._uiElementCollection;
			}
		}

		// Token: 0x06005E0E RID: 24078 RVA: 0x001A72E0 File Offset: 0x001A54E0
		private static void ColumnsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GridViewRowPresenterBase gridViewRowPresenterBase = (GridViewRowPresenterBase)d;
			GridViewColumnCollection gridViewColumnCollection = (GridViewColumnCollection)e.OldValue;
			if (gridViewColumnCollection != null)
			{
				InternalCollectionChangedEventManager.RemoveHandler(gridViewColumnCollection, new EventHandler<NotifyCollectionChangedEventArgs>(gridViewRowPresenterBase.ColumnCollectionChanged));
				if (!gridViewColumnCollection.InViewMode && gridViewColumnCollection.Owner == gridViewRowPresenterBase.GetStableAncester())
				{
					gridViewColumnCollection.Owner = null;
				}
			}
			GridViewColumnCollection gridViewColumnCollection2 = (GridViewColumnCollection)e.NewValue;
			if (gridViewColumnCollection2 != null)
			{
				InternalCollectionChangedEventManager.AddHandler(gridViewColumnCollection2, new EventHandler<NotifyCollectionChangedEventArgs>(gridViewRowPresenterBase.ColumnCollectionChanged));
				if (!gridViewColumnCollection2.InViewMode && gridViewColumnCollection2.Owner == null)
				{
					gridViewColumnCollection2.Owner = gridViewRowPresenterBase.GetStableAncester();
				}
			}
			gridViewRowPresenterBase.NeedUpdateVisualTree = true;
			gridViewRowPresenterBase.InvalidateMeasure();
		}

		// Token: 0x06005E0F RID: 24079 RVA: 0x001A7380 File Offset: 0x001A5580
		private FrameworkElement GetStableAncester()
		{
			ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(base.TemplatedParent);
			if (itemsControl == null)
			{
				return this;
			}
			return itemsControl;
		}

		// Token: 0x170016B7 RID: 5815
		// (get) Token: 0x06005E10 RID: 24080 RVA: 0x001A739F File Offset: 0x001A559F
		private bool IsPresenterVisualReady
		{
			get
			{
				return base.IsInitialized && !this.NeedUpdateVisualTree;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="managerType"> The type of the <see cref="T:System.Windows.WeakEventManager" /> calling this method.</param>
		/// <param name="sender"> Object that originated the event.</param>
		/// <param name="args"> Event data.</param>
		/// <returns>
		///     <see langword="true" /> if the listener handled the event.</returns>
		// Token: 0x06005E11 RID: 24081 RVA: 0x0000B02A File Offset: 0x0000922A
		bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs args)
		{
			return false;
		}

		// Token: 0x06005E12 RID: 24082 RVA: 0x001A73B4 File Offset: 0x001A55B4
		private void ColumnCollectionChanged(object sender, NotifyCollectionChangedEventArgs arg)
		{
			GridViewColumnCollectionChangedEventArgs gridViewColumnCollectionChangedEventArgs = arg as GridViewColumnCollectionChangedEventArgs;
			if (gridViewColumnCollectionChangedEventArgs != null && this.IsPresenterVisualReady)
			{
				if (gridViewColumnCollectionChangedEventArgs.Column != null)
				{
					this.OnColumnPropertyChanged(gridViewColumnCollectionChangedEventArgs.Column, gridViewColumnCollectionChangedEventArgs.PropertyName);
					return;
				}
				this.OnColumnCollectionChanged(gridViewColumnCollectionChangedEventArgs);
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.GridViewRowPresenterBase.Columns" /> dependency property. </summary>
		// Token: 0x04003043 RID: 12355
		public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns", typeof(GridViewColumnCollection), typeof(GridViewRowPresenterBase), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(GridViewRowPresenterBase.ColumnsPropertyChanged)));

		// Token: 0x04003044 RID: 12356
		internal const double c_PaddingHeaderMinWidth = 2.0;

		// Token: 0x04003045 RID: 12357
		private UIElementCollection _uiElementCollection;

		// Token: 0x04003046 RID: 12358
		private bool _needUpdateVisualTree = true;

		// Token: 0x04003047 RID: 12359
		private List<double> _desiredWidthList;
	}
}
