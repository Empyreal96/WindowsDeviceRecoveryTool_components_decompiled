using System;
using System.Windows.Controls;
using System.Windows.Media;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	/// <summary>A flow content element that defines a column within a <see cref="T:System.Windows.Documents.Table" />.</summary>
	// Token: 0x020003E5 RID: 997
	public class TableColumn : FrameworkContentElement, IIndexedChild<Table>
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.TableColumn" /> class. </summary>
		// Token: 0x06003671 RID: 13937 RVA: 0x000F5855 File Offset: 0x000F3A55
		public TableColumn()
		{
			this._parentIndex = -1;
		}

		/// <summary>Gets or sets the width of a <see cref="T:System.Windows.Documents.TableColumn" /> element. The <see cref="P:System.Windows.Documents.TableColumn.Width" /> property measures the sum of the <see cref="T:System.Windows.Documents.TableColumn" /> content, padding, and border from side to side.  </summary>
		/// <returns>The width of the <see cref="T:System.Windows.Documents.TableColumn" /> element, as a <see cref="T:System.Windows.GridLength" />.</returns>
		// Token: 0x17000DEB RID: 3563
		// (get) Token: 0x06003672 RID: 13938 RVA: 0x000F5864 File Offset: 0x000F3A64
		// (set) Token: 0x06003673 RID: 13939 RVA: 0x000F5876 File Offset: 0x000F3A76
		public GridLength Width
		{
			get
			{
				return (GridLength)base.GetValue(TableColumn.WidthProperty);
			}
			set
			{
				base.SetValue(TableColumn.WidthProperty, value);
			}
		}

		/// <summary>Gets or sets the background <see cref="T:System.Windows.Media.Brush" /> used to fill the content of the <see cref="T:System.Windows.Documents.TableColumn" />.  </summary>
		/// <returns>The background <see cref="T:System.Windows.Media.Brush" /> used to fill the content of the <see cref="T:System.Windows.Documents.TableColumn" />.</returns>
		// Token: 0x17000DEC RID: 3564
		// (get) Token: 0x06003674 RID: 13940 RVA: 0x000F5889 File Offset: 0x000F3A89
		// (set) Token: 0x06003675 RID: 13941 RVA: 0x000F589B File Offset: 0x000F3A9B
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(TableColumn.BackgroundProperty);
			}
			set
			{
				base.SetValue(TableColumn.BackgroundProperty, value);
			}
		}

		// Token: 0x06003676 RID: 13942 RVA: 0x000F58A9 File Offset: 0x000F3AA9
		void IIndexedChild<Table>.OnEnterParentTree()
		{
			this.OnEnterParentTree();
		}

		// Token: 0x06003677 RID: 13943 RVA: 0x000F58B1 File Offset: 0x000F3AB1
		void IIndexedChild<Table>.OnExitParentTree()
		{
			this.OnExitParentTree();
		}

		// Token: 0x06003678 RID: 13944 RVA: 0x00002137 File Offset: 0x00000337
		void IIndexedChild<Table>.OnAfterExitParentTree(Table parent)
		{
		}

		// Token: 0x17000DED RID: 3565
		// (get) Token: 0x06003679 RID: 13945 RVA: 0x000F58B9 File Offset: 0x000F3AB9
		// (set) Token: 0x0600367A RID: 13946 RVA: 0x000F58C1 File Offset: 0x000F3AC1
		int IIndexedChild<Table>.Index
		{
			get
			{
				return this.Index;
			}
			set
			{
				this.Index = value;
			}
		}

		// Token: 0x0600367B RID: 13947 RVA: 0x000F58CA File Offset: 0x000F3ACA
		internal void OnEnterParentTree()
		{
			this.Table.InvalidateColumns();
		}

		// Token: 0x0600367C RID: 13948 RVA: 0x000F58CA File Offset: 0x000F3ACA
		internal void OnExitParentTree()
		{
			this.Table.InvalidateColumns();
		}

		// Token: 0x17000DEE RID: 3566
		// (get) Token: 0x0600367D RID: 13949 RVA: 0x000F58D7 File Offset: 0x000F3AD7
		internal Table Table
		{
			get
			{
				return base.Parent as Table;
			}
		}

		// Token: 0x17000DEF RID: 3567
		// (get) Token: 0x0600367E RID: 13950 RVA: 0x000F58E4 File Offset: 0x000F3AE4
		// (set) Token: 0x0600367F RID: 13951 RVA: 0x000F58EC File Offset: 0x000F3AEC
		internal int Index
		{
			get
			{
				return this._parentIndex;
			}
			set
			{
				this._parentIndex = value;
			}
		}

		// Token: 0x17000DF0 RID: 3568
		// (get) Token: 0x06003680 RID: 13952 RVA: 0x000F58F5 File Offset: 0x000F3AF5
		internal static GridLength DefaultWidth
		{
			get
			{
				return new GridLength(0.0, GridUnitType.Auto);
			}
		}

		// Token: 0x06003681 RID: 13953 RVA: 0x000F5908 File Offset: 0x000F3B08
		private static bool IsValidWidth(object value)
		{
			GridLength gridLength = (GridLength)value;
			if ((gridLength.GridUnitType == GridUnitType.Pixel || gridLength.GridUnitType == GridUnitType.Star) && gridLength.Value < 0.0)
			{
				return false;
			}
			double num = (double)Math.Min(1000000, 3500000);
			return gridLength.GridUnitType != GridUnitType.Pixel || gridLength.Value <= num;
		}

		// Token: 0x06003682 RID: 13954 RVA: 0x000F596C File Offset: 0x000F3B6C
		private static void OnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Table table = ((TableColumn)d).Table;
			if (table != null)
			{
				table.InvalidateColumns();
			}
		}

		// Token: 0x06003683 RID: 13955 RVA: 0x000F5990 File Offset: 0x000F3B90
		private static void OnBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Table table = ((TableColumn)d).Table;
			if (table != null)
			{
				table.InvalidateColumns();
			}
		}

		// Token: 0x04002547 RID: 9543
		private int _parentIndex;

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.TableColumn.Width" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.TableColumn.Width" /> dependency property.</returns>
		// Token: 0x04002548 RID: 9544
		public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(GridLength), typeof(TableColumn), new FrameworkPropertyMetadata(new GridLength(0.0, GridUnitType.Auto), FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(TableColumn.OnWidthChanged)), new ValidateValueCallback(TableColumn.IsValidWidth));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.TableColumn.Background" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.TableColumn.Background" /> dependency property.</returns>
		// Token: 0x04002549 RID: 9545
		public static readonly DependencyProperty BackgroundProperty = Panel.BackgroundProperty.AddOwner(typeof(TableColumn), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(TableColumn.OnBackgroundChanged)));
	}
}
