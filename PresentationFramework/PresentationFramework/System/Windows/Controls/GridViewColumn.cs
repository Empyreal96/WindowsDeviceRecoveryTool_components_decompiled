using System;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Markup;
using MS.Internal;

namespace System.Windows.Controls
{
	/// <summary>Represents a column that displays data.</summary>
	// Token: 0x020004D6 RID: 1238
	[ContentProperty("Header")]
	[StyleTypedProperty(Property = "HeaderContainerStyle", StyleTargetType = typeof(GridViewColumnHeader))]
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public class GridViewColumn : DependencyObject, INotifyPropertyChanged
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.GridViewColumn" /> class.</summary>
		// Token: 0x06004CD1 RID: 19665 RVA: 0x0015AD5D File Offset: 0x00158F5D
		public GridViewColumn()
		{
			this.ResetPrivateData();
			this._state = (double.IsNaN(this.Width) ? ColumnMeasureState.Init : ColumnMeasureState.SpecificWidth);
		}

		/// <summary>Creates a string representation of the <see cref="T:System.Windows.Controls.GridViewColumn" />.</summary>
		/// <returns>A string that identifies the object as a <see cref="T:System.Windows.Controls.GridViewColumn" /> object and displays the value of the <see cref="P:System.Windows.Controls.GridViewColumn.Header" /> property.</returns>
		// Token: 0x06004CD2 RID: 19666 RVA: 0x0015AD82 File Offset: 0x00158F82
		public override string ToString()
		{
			return SR.Get("ToStringFormatString_GridViewColumn", new object[]
			{
				base.GetType(),
				this.Header
			});
		}

		/// <summary>Gets or sets the content of the header of a <see cref="T:System.Windows.Controls.GridViewColumn" />. </summary>
		/// <returns>The object to use for the column header. The default is <see langword="null" />.</returns>
		// Token: 0x170012BC RID: 4796
		// (get) Token: 0x06004CD3 RID: 19667 RVA: 0x0015ADA6 File Offset: 0x00158FA6
		// (set) Token: 0x06004CD4 RID: 19668 RVA: 0x0015ADB3 File Offset: 0x00158FB3
		public object Header
		{
			get
			{
				return base.GetValue(GridViewColumn.HeaderProperty);
			}
			set
			{
				base.SetValue(GridViewColumn.HeaderProperty, value);
			}
		}

		// Token: 0x06004CD5 RID: 19669 RVA: 0x0015ADC4 File Offset: 0x00158FC4
		private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GridViewColumn gridViewColumn = (GridViewColumn)d;
			gridViewColumn.OnPropertyChanged(GridViewColumn.HeaderProperty.Name);
		}

		/// <summary>Gets or sets the style to use for the header of the <see cref="T:System.Windows.Controls.GridViewColumn" />. </summary>
		/// <returns>The <see cref="T:System.Windows.Style" /> that defines the display properties for the column header. The default is <see langword="null" />.</returns>
		// Token: 0x170012BD RID: 4797
		// (get) Token: 0x06004CD6 RID: 19670 RVA: 0x0015ADE8 File Offset: 0x00158FE8
		// (set) Token: 0x06004CD7 RID: 19671 RVA: 0x0015ADFA File Offset: 0x00158FFA
		public Style HeaderContainerStyle
		{
			get
			{
				return (Style)base.GetValue(GridViewColumn.HeaderContainerStyleProperty);
			}
			set
			{
				base.SetValue(GridViewColumn.HeaderContainerStyleProperty, value);
			}
		}

		// Token: 0x06004CD8 RID: 19672 RVA: 0x0015AE08 File Offset: 0x00159008
		private static void OnHeaderContainerStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GridViewColumn gridViewColumn = (GridViewColumn)d;
			gridViewColumn.OnPropertyChanged(GridViewColumn.HeaderContainerStyleProperty.Name);
		}

		/// <summary>Gets or sets the template to use to display the content of the column header. </summary>
		/// <returns>A <see cref="T:System.Windows.DataTemplate" /> to use to display the column header. The default is <see langword="null" />.</returns>
		// Token: 0x170012BE RID: 4798
		// (get) Token: 0x06004CD9 RID: 19673 RVA: 0x0015AE2C File Offset: 0x0015902C
		// (set) Token: 0x06004CDA RID: 19674 RVA: 0x0015AE3E File Offset: 0x0015903E
		public DataTemplate HeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(GridViewColumn.HeaderTemplateProperty);
			}
			set
			{
				base.SetValue(GridViewColumn.HeaderTemplateProperty, value);
			}
		}

		// Token: 0x06004CDB RID: 19675 RVA: 0x0015AE4C File Offset: 0x0015904C
		private static void OnHeaderTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GridViewColumn gridViewColumn = (GridViewColumn)d;
			Helper.CheckTemplateAndTemplateSelector("Header", GridViewColumn.HeaderTemplateProperty, GridViewColumn.HeaderTemplateSelectorProperty, gridViewColumn);
			gridViewColumn.OnPropertyChanged(GridViewColumn.HeaderTemplateProperty.Name);
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Controls.DataTemplateSelector" /> that provides logic to select the template to use to display the column header. </summary>
		/// <returns>The <see cref="T:System.Windows.Controls.DataTemplateSelector" /> object that provides data template selection for each <see cref="T:System.Windows.Controls.GridViewColumn" />. The default is <see langword="null" />.</returns>
		// Token: 0x170012BF RID: 4799
		// (get) Token: 0x06004CDC RID: 19676 RVA: 0x0015AE85 File Offset: 0x00159085
		// (set) Token: 0x06004CDD RID: 19677 RVA: 0x0015AE97 File Offset: 0x00159097
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DataTemplateSelector HeaderTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(GridViewColumn.HeaderTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(GridViewColumn.HeaderTemplateSelectorProperty, value);
			}
		}

		// Token: 0x06004CDE RID: 19678 RVA: 0x0015AEA8 File Offset: 0x001590A8
		private static void OnHeaderTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GridViewColumn gridViewColumn = (GridViewColumn)d;
			Helper.CheckTemplateAndTemplateSelector("Header", GridViewColumn.HeaderTemplateProperty, GridViewColumn.HeaderTemplateSelectorProperty, gridViewColumn);
			gridViewColumn.OnPropertyChanged(GridViewColumn.HeaderTemplateSelectorProperty.Name);
		}

		/// <summary>Gets or sets a composite string that specifies how to format the <see cref="P:System.Windows.Controls.GridViewColumn.Header" /> property if it is displayed as a string.</summary>
		/// <returns>A composite string that specifies how to format the <see cref="P:System.Windows.Controls.GridViewColumn.Header" /> property if it is displayed as a string. The default is <see langword="null" />.</returns>
		// Token: 0x170012C0 RID: 4800
		// (get) Token: 0x06004CDF RID: 19679 RVA: 0x0015AEE1 File Offset: 0x001590E1
		// (set) Token: 0x06004CE0 RID: 19680 RVA: 0x0015AEF3 File Offset: 0x001590F3
		public string HeaderStringFormat
		{
			get
			{
				return (string)base.GetValue(GridViewColumn.HeaderStringFormatProperty);
			}
			set
			{
				base.SetValue(GridViewColumn.HeaderStringFormatProperty, value);
			}
		}

		// Token: 0x06004CE1 RID: 19681 RVA: 0x0015AF04 File Offset: 0x00159104
		private static void OnHeaderStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GridViewColumn gridViewColumn = (GridViewColumn)d;
			gridViewColumn.OnHeaderStringFormatChanged((string)e.OldValue, (string)e.NewValue);
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Controls.GridViewColumn.HeaderStringFormat" /> property changes.</summary>
		/// <param name="oldHeaderStringFormat">The old value of the <see cref="P:System.Windows.Controls.GridViewColumn.HeaderStringFormat" /> property.</param>
		/// <param name="newHeaderStringFormat">The new value of the <see cref="P:System.Windows.Controls.GridViewColumn.HeaderStringFormat" /> property.</param>
		// Token: 0x06004CE2 RID: 19682 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnHeaderStringFormatChanged(string oldHeaderStringFormat, string newHeaderStringFormat)
		{
		}

		/// <summary>Gets or sets the data item to bind to for this column.</summary>
		/// <returns>The specified data item type that displays in the column. The default is <see langword="null" />.</returns>
		// Token: 0x170012C1 RID: 4801
		// (get) Token: 0x06004CE3 RID: 19683 RVA: 0x0015AF36 File Offset: 0x00159136
		// (set) Token: 0x06004CE4 RID: 19684 RVA: 0x0015AF3E File Offset: 0x0015913E
		public BindingBase DisplayMemberBinding
		{
			get
			{
				return this._displayMemberBinding;
			}
			set
			{
				if (this._displayMemberBinding != value)
				{
					this._displayMemberBinding = value;
					this.OnDisplayMemberBindingChanged();
				}
			}
		}

		// Token: 0x06004CE5 RID: 19685 RVA: 0x0015AF56 File Offset: 0x00159156
		private void OnDisplayMemberBindingChanged()
		{
			this.OnPropertyChanged("DisplayMemberBinding");
		}

		/// <summary>Gets or sets the template to use to display the contents of a column cell. </summary>
		/// <returns>A <see cref="T:System.Windows.DataTemplate" /> that is used to format a column cell. The default is <see langword="null" />.</returns>
		// Token: 0x170012C2 RID: 4802
		// (get) Token: 0x06004CE6 RID: 19686 RVA: 0x0015AF63 File Offset: 0x00159163
		// (set) Token: 0x06004CE7 RID: 19687 RVA: 0x0015AF75 File Offset: 0x00159175
		public DataTemplate CellTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(GridViewColumn.CellTemplateProperty);
			}
			set
			{
				base.SetValue(GridViewColumn.CellTemplateProperty, value);
			}
		}

		// Token: 0x06004CE8 RID: 19688 RVA: 0x0015AF84 File Offset: 0x00159184
		private static void OnCellTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GridViewColumn gridViewColumn = (GridViewColumn)d;
			gridViewColumn.OnPropertyChanged(GridViewColumn.CellTemplateProperty.Name);
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Controls.DataTemplateSelector" /> that determines the template to use to display cells in a column. </summary>
		/// <returns>A <see cref="T:System.Windows.Controls.DataTemplateSelector" /> that provides <see cref="T:System.Windows.DataTemplate" /> selection for column cells. The default is <see langword="null" />.</returns>
		// Token: 0x170012C3 RID: 4803
		// (get) Token: 0x06004CE9 RID: 19689 RVA: 0x0015AFA8 File Offset: 0x001591A8
		// (set) Token: 0x06004CEA RID: 19690 RVA: 0x0015AFBA File Offset: 0x001591BA
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DataTemplateSelector CellTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(GridViewColumn.CellTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(GridViewColumn.CellTemplateSelectorProperty, value);
			}
		}

		// Token: 0x06004CEB RID: 19691 RVA: 0x0015AFC8 File Offset: 0x001591C8
		private static void OnCellTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GridViewColumn gridViewColumn = (GridViewColumn)d;
			gridViewColumn.OnPropertyChanged(GridViewColumn.CellTemplateSelectorProperty.Name);
		}

		/// <summary>Gets or sets the width of the column. </summary>
		/// <returns>The width of the column. The default is <see cref="F:System.Double.NaN" />, which automatically sizes to the largest column item that is not the column header.</returns>
		// Token: 0x170012C4 RID: 4804
		// (get) Token: 0x06004CEC RID: 19692 RVA: 0x0015AFEC File Offset: 0x001591EC
		// (set) Token: 0x06004CED RID: 19693 RVA: 0x0015AFFE File Offset: 0x001591FE
		[TypeConverter(typeof(LengthConverter))]
		public double Width
		{
			get
			{
				return (double)base.GetValue(GridViewColumn.WidthProperty);
			}
			set
			{
				base.SetValue(GridViewColumn.WidthProperty, value);
			}
		}

		// Token: 0x06004CEE RID: 19694 RVA: 0x0015B014 File Offset: 0x00159214
		private static void OnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GridViewColumn gridViewColumn = (GridViewColumn)d;
			double d2 = (double)e.NewValue;
			gridViewColumn.State = (double.IsNaN(d2) ? ColumnMeasureState.Init : ColumnMeasureState.SpecificWidth);
			gridViewColumn.OnPropertyChanged(GridViewColumn.WidthProperty.Name);
		}

		/// <summary>Gets the actual width of a <see cref="T:System.Windows.Controls.GridViewColumn" />.</summary>
		/// <returns>The current width of the column. The default is zero (0.0).</returns>
		// Token: 0x170012C5 RID: 4805
		// (get) Token: 0x06004CEF RID: 19695 RVA: 0x0015B057 File Offset: 0x00159257
		// (set) Token: 0x06004CF0 RID: 19696 RVA: 0x0015B05F File Offset: 0x0015925F
		public double ActualWidth
		{
			get
			{
				return this._actualWidth;
			}
			private set
			{
				if (!double.IsNaN(value) && !double.IsInfinity(value) && value >= 0.0 && this._actualWidth != value)
				{
					this._actualWidth = value;
					this.OnPropertyChanged("ActualWidth");
				}
			}
		}

		/// <summary>Occurs when the value of any <see cref="T:System.Windows.Controls.GridViewColumn" /> property changes.</summary>
		// Token: 0x140000D9 RID: 217
		// (add) Token: 0x06004CF1 RID: 19697 RVA: 0x0015B098 File Offset: 0x00159298
		// (remove) Token: 0x06004CF2 RID: 19698 RVA: 0x0015B0A1 File Offset: 0x001592A1
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this._propertyChanged += value;
			}
			remove
			{
				this._propertyChanged -= value;
			}
		}

		// Token: 0x140000DA RID: 218
		// (add) Token: 0x06004CF3 RID: 19699 RVA: 0x0015B0AC File Offset: 0x001592AC
		// (remove) Token: 0x06004CF4 RID: 19700 RVA: 0x0015B0E4 File Offset: 0x001592E4
		private event PropertyChangedEventHandler _propertyChanged;

		/// <summary>Raises the <see cref="E:System.Windows.Controls.GridViewColumn.System#ComponentModel#INotifyPropertyChanged#PropertyChanged" /> event.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06004CF5 RID: 19701 RVA: 0x0015B119 File Offset: 0x00159319
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (this._propertyChanged != null)
			{
				this._propertyChanged(this, e);
			}
		}

		// Token: 0x06004CF6 RID: 19702 RVA: 0x0015B130 File Offset: 0x00159330
		internal void OnThemeChanged()
		{
			if (this.Header != null)
			{
				DependencyObject dependencyObject = this.Header as DependencyObject;
				if (dependencyObject != null)
				{
					FrameworkElement frameworkElement;
					FrameworkContentElement frameworkContentElement;
					Helper.DowncastToFEorFCE(dependencyObject, out frameworkElement, out frameworkContentElement, false);
					if (frameworkElement != null || frameworkContentElement != null)
					{
						TreeWalkHelper.InvalidateOnResourcesChange(frameworkElement, frameworkContentElement, ResourcesChangeInfo.ThemeChangeInfo);
					}
				}
			}
		}

		// Token: 0x06004CF7 RID: 19703 RVA: 0x0015B171 File Offset: 0x00159371
		internal double EnsureWidth(double width)
		{
			if (width > this.DesiredWidth)
			{
				this.DesiredWidth = width;
			}
			return this.DesiredWidth;
		}

		// Token: 0x06004CF8 RID: 19704 RVA: 0x0015B189 File Offset: 0x00159389
		internal void ResetPrivateData()
		{
			this._actualIndex = -1;
			this._desiredWidth = 0.0;
			this._state = (double.IsNaN(this.Width) ? ColumnMeasureState.Init : ColumnMeasureState.SpecificWidth);
		}

		// Token: 0x170012C6 RID: 4806
		// (get) Token: 0x06004CF9 RID: 19705 RVA: 0x0015B1B8 File Offset: 0x001593B8
		// (set) Token: 0x06004CFA RID: 19706 RVA: 0x0015B1C0 File Offset: 0x001593C0
		internal ColumnMeasureState State
		{
			get
			{
				return this._state;
			}
			set
			{
				if (this._state == value)
				{
					if (value == ColumnMeasureState.SpecificWidth)
					{
						this.UpdateActualWidth();
					}
					return;
				}
				this._state = value;
				if (value != ColumnMeasureState.Init)
				{
					this.UpdateActualWidth();
					return;
				}
				this.DesiredWidth = 0.0;
			}
		}

		// Token: 0x170012C7 RID: 4807
		// (get) Token: 0x06004CFB RID: 19707 RVA: 0x0015B1F6 File Offset: 0x001593F6
		// (set) Token: 0x06004CFC RID: 19708 RVA: 0x0015B1FE File Offset: 0x001593FE
		internal int ActualIndex
		{
			get
			{
				return this._actualIndex;
			}
			set
			{
				this._actualIndex = value;
			}
		}

		// Token: 0x170012C8 RID: 4808
		// (get) Token: 0x06004CFD RID: 19709 RVA: 0x0015B207 File Offset: 0x00159407
		// (set) Token: 0x06004CFE RID: 19710 RVA: 0x0015B20F File Offset: 0x0015940F
		internal double DesiredWidth
		{
			get
			{
				return this._desiredWidth;
			}
			private set
			{
				this._desiredWidth = value;
			}
		}

		// Token: 0x170012C9 RID: 4809
		// (get) Token: 0x06004CFF RID: 19711 RVA: 0x0015B218 File Offset: 0x00159418
		internal override DependencyObject InheritanceContext
		{
			get
			{
				return this._inheritanceContext;
			}
		}

		// Token: 0x06004D00 RID: 19712 RVA: 0x0015B220 File Offset: 0x00159420
		internal override void AddInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			if (this._inheritanceContext == null && context != null)
			{
				this._inheritanceContext = context;
				base.OnInheritanceContextChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06004D01 RID: 19713 RVA: 0x0015B23F File Offset: 0x0015943F
		internal override void RemoveInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			if (this._inheritanceContext == context)
			{
				this._inheritanceContext = null;
				base.OnInheritanceContextChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06004D02 RID: 19714 RVA: 0x0015B25C File Offset: 0x0015945C
		private void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x06004D03 RID: 19715 RVA: 0x0015B26A File Offset: 0x0015946A
		private void UpdateActualWidth()
		{
			this.ActualWidth = ((this.State == ColumnMeasureState.SpecificWidth) ? this.Width : this.DesiredWidth);
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridViewColumn.Header" /> dependency property. </summary>
		// Token: 0x04002B34 RID: 11060
		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(GridViewColumn), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewColumn.OnHeaderChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridViewColumn.HeaderContainerStyle" /> dependency property. </summary>
		// Token: 0x04002B35 RID: 11061
		public static readonly DependencyProperty HeaderContainerStyleProperty = DependencyProperty.Register("HeaderContainerStyle", typeof(Style), typeof(GridViewColumn), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewColumn.OnHeaderContainerStyleChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridViewColumn.HeaderTemplate" /> dependency property. </summary>
		// Token: 0x04002B36 RID: 11062
		public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(GridViewColumn), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewColumn.OnHeaderTemplateChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridViewColumn.HeaderTemplateSelector" /> dependency property. </summary>
		// Token: 0x04002B37 RID: 11063
		public static readonly DependencyProperty HeaderTemplateSelectorProperty = DependencyProperty.Register("HeaderTemplateSelector", typeof(DataTemplateSelector), typeof(GridViewColumn), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewColumn.OnHeaderTemplateSelectorChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridViewColumn.HeaderStringFormat" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.GridViewColumn.HeaderStringFormat" /> dependency property.</returns>
		// Token: 0x04002B38 RID: 11064
		public static readonly DependencyProperty HeaderStringFormatProperty = DependencyProperty.Register("HeaderStringFormat", typeof(string), typeof(GridViewColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(GridViewColumn.OnHeaderStringFormatChanged)));

		// Token: 0x04002B39 RID: 11065
		private BindingBase _displayMemberBinding;

		// Token: 0x04002B3A RID: 11066
		internal const string c_DisplayMemberBindingName = "DisplayMemberBinding";

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridViewColumn.CellTemplate" /> dependency property. </summary>
		// Token: 0x04002B3B RID: 11067
		public static readonly DependencyProperty CellTemplateProperty = DependencyProperty.Register("CellTemplate", typeof(DataTemplate), typeof(GridViewColumn), new PropertyMetadata(new PropertyChangedCallback(GridViewColumn.OnCellTemplateChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridViewColumn.CellTemplateSelector" /> dependency property. </summary>
		// Token: 0x04002B3C RID: 11068
		public static readonly DependencyProperty CellTemplateSelectorProperty = DependencyProperty.Register("CellTemplateSelector", typeof(DataTemplateSelector), typeof(GridViewColumn), new PropertyMetadata(new PropertyChangedCallback(GridViewColumn.OnCellTemplateSelectorChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridViewColumn.Width" /> dependency property. </summary>
		// Token: 0x04002B3D RID: 11069
		public static readonly DependencyProperty WidthProperty = FrameworkElement.WidthProperty.AddOwner(typeof(GridViewColumn), new PropertyMetadata(double.NaN, new PropertyChangedCallback(GridViewColumn.OnWidthChanged)));

		// Token: 0x04002B3F RID: 11071
		internal const string c_ActualWidthName = "ActualWidth";

		// Token: 0x04002B40 RID: 11072
		private DependencyObject _inheritanceContext;

		// Token: 0x04002B41 RID: 11073
		private double _desiredWidth;

		// Token: 0x04002B42 RID: 11074
		private int _actualIndex;

		// Token: 0x04002B43 RID: 11075
		private double _actualWidth;

		// Token: 0x04002B44 RID: 11076
		private ColumnMeasureState _state;
	}
}
