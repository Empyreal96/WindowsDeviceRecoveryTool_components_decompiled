using System;
using System.ComponentModel;
using MS.Internal.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Defines column-specific properties that apply to <see cref="T:System.Windows.Controls.Grid" /> elements. </summary>
	// Token: 0x02000571 RID: 1393
	public class ColumnDefinition : DefinitionBase
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.ColumnDefinition" /> class.</summary>
		// Token: 0x06005BB9 RID: 23481 RVA: 0x0019CED3 File Offset: 0x0019B0D3
		public ColumnDefinition() : base(true)
		{
		}

		/// <summary>Gets the calculated width of a <see cref="T:System.Windows.Controls.ColumnDefinition" /> element, or sets the <see cref="T:System.Windows.GridLength" /> value of a column that is defined by the <see cref="T:System.Windows.Controls.ColumnDefinition" />.   </summary>
		/// <returns>The <see cref="T:System.Windows.GridLength" /> that represents the width of the Column. The default value is 1.0.</returns>
		// Token: 0x17001636 RID: 5686
		// (get) Token: 0x06005BBA RID: 23482 RVA: 0x0019CEDC File Offset: 0x0019B0DC
		// (set) Token: 0x06005BBB RID: 23483 RVA: 0x0019CEE4 File Offset: 0x0019B0E4
		public GridLength Width
		{
			get
			{
				return base.UserSizeValueCache;
			}
			set
			{
				base.SetValue(ColumnDefinition.WidthProperty, value);
			}
		}

		/// <summary>Gets or sets a value that represents the minimum width of a <see cref="T:System.Windows.Controls.ColumnDefinition" />.   </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the minimum width. The default value is 0.</returns>
		// Token: 0x17001637 RID: 5687
		// (get) Token: 0x06005BBC RID: 23484 RVA: 0x0014FE9C File Offset: 0x0014E09C
		// (set) Token: 0x06005BBD RID: 23485 RVA: 0x0019CEF7 File Offset: 0x0019B0F7
		[TypeConverter(typeof(LengthConverter))]
		public double MinWidth
		{
			get
			{
				return base.UserMinSizeValueCache;
			}
			set
			{
				base.SetValue(ColumnDefinition.MinWidthProperty, value);
			}
		}

		/// <summary>Gets or sets a value that represents the maximum width of a <see cref="T:System.Windows.Controls.ColumnDefinition" />.   </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the maximum width. The default value is <see cref="F:System.Double.PositiveInfinity" />.</returns>
		// Token: 0x17001638 RID: 5688
		// (get) Token: 0x06005BBE RID: 23486 RVA: 0x0014FEA4 File Offset: 0x0014E0A4
		// (set) Token: 0x06005BBF RID: 23487 RVA: 0x0019CF0A File Offset: 0x0019B10A
		[TypeConverter(typeof(LengthConverter))]
		public double MaxWidth
		{
			get
			{
				return base.UserMaxSizeValueCache;
			}
			set
			{
				base.SetValue(ColumnDefinition.MaxWidthProperty, value);
			}
		}

		/// <summary>Gets a value that represents the actual calculated width of a <see cref="T:System.Windows.Controls.ColumnDefinition" />. </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the actual calculated width in device independent pixels. The default value is 0.0.</returns>
		// Token: 0x17001639 RID: 5689
		// (get) Token: 0x06005BC0 RID: 23488 RVA: 0x0019CF20 File Offset: 0x0019B120
		public double ActualWidth
		{
			get
			{
				double result = 0.0;
				if (base.InParentLogicalTree)
				{
					result = ((Grid)base.Parent).GetFinalColumnDefinitionWidth(base.Index);
				}
				return result;
			}
		}

		/// <summary>Gets a value that represents the offset value of this <see cref="T:System.Windows.Controls.ColumnDefinition" />. </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the offset of the column. The default value is 0.0.</returns>
		// Token: 0x1700163A RID: 5690
		// (get) Token: 0x06005BC1 RID: 23489 RVA: 0x0019CF58 File Offset: 0x0019B158
		public double Offset
		{
			get
			{
				double result = 0.0;
				if (base.Index != 0)
				{
					result = base.FinalOffset;
				}
				return result;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ColumnDefinition.Width" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ColumnDefinition.Width" /> dependency property.</returns>
		// Token: 0x04002F92 RID: 12178
		[CommonDependencyProperty]
		public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(GridLength), typeof(ColumnDefinition), new FrameworkPropertyMetadata(new GridLength(1.0, GridUnitType.Star), new PropertyChangedCallback(DefinitionBase.OnUserSizePropertyChanged)), new ValidateValueCallback(DefinitionBase.IsUserSizePropertyValueValid));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ColumnDefinition.MinWidth" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ColumnDefinition.MinWidth" /> dependency property.</returns>
		// Token: 0x04002F93 RID: 12179
		[CommonDependencyProperty]
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		public static readonly DependencyProperty MinWidthProperty = DependencyProperty.Register("MinWidth", typeof(double), typeof(ColumnDefinition), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(DefinitionBase.OnUserMinSizePropertyChanged)), new ValidateValueCallback(DefinitionBase.IsUserMinSizePropertyValueValid));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ColumnDefinition.MaxWidth" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ColumnDefinition.MaxWidth" /> dependency property.</returns>
		// Token: 0x04002F94 RID: 12180
		[CommonDependencyProperty]
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		public static readonly DependencyProperty MaxWidthProperty = DependencyProperty.Register("MaxWidth", typeof(double), typeof(ColumnDefinition), new FrameworkPropertyMetadata(double.PositiveInfinity, new PropertyChangedCallback(DefinitionBase.OnUserMaxSizePropertyChanged)), new ValidateValueCallback(DefinitionBase.IsUserMaxSizePropertyValueValid));
	}
}
