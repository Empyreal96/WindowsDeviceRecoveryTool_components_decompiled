using System;
using System.ComponentModel;
using MS.Internal.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Defines row-specific properties that apply to <see cref="T:System.Windows.Controls.Grid" /> elements.</summary>
	// Token: 0x02000573 RID: 1395
	public class RowDefinition : DefinitionBase
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.RowDefinition" /> class.</summary>
		// Token: 0x06005BEA RID: 23530 RVA: 0x0019D88B File Offset: 0x0019BA8B
		public RowDefinition() : base(false)
		{
		}

		/// <summary>Gets the calculated height of a <see cref="T:System.Windows.Controls.RowDefinition" /> element, or sets the <see cref="T:System.Windows.GridLength" /> value of a row that is defined by the <see cref="T:System.Windows.Controls.RowDefinition" />.   </summary>
		/// <returns>The <see cref="T:System.Windows.GridLength" /> that represents the height of the row. The default value is 1.0.</returns>
		// Token: 0x17001644 RID: 5700
		// (get) Token: 0x06005BEB RID: 23531 RVA: 0x0019CEDC File Offset: 0x0019B0DC
		// (set) Token: 0x06005BEC RID: 23532 RVA: 0x0019D894 File Offset: 0x0019BA94
		public GridLength Height
		{
			get
			{
				return base.UserSizeValueCache;
			}
			set
			{
				base.SetValue(RowDefinition.HeightProperty, value);
			}
		}

		/// <summary>Gets or sets a value that represents the minimum allowable height of a <see cref="T:System.Windows.Controls.RowDefinition" />.  </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the minimum allowable height. The default value is 0.</returns>
		// Token: 0x17001645 RID: 5701
		// (get) Token: 0x06005BED RID: 23533 RVA: 0x0014FE9C File Offset: 0x0014E09C
		// (set) Token: 0x06005BEE RID: 23534 RVA: 0x0019D8A7 File Offset: 0x0019BAA7
		[TypeConverter(typeof(LengthConverter))]
		public double MinHeight
		{
			get
			{
				return base.UserMinSizeValueCache;
			}
			set
			{
				base.SetValue(RowDefinition.MinHeightProperty, value);
			}
		}

		/// <summary>Gets or sets a value that represents the maximum height of a <see cref="T:System.Windows.Controls.RowDefinition" />.  </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the maximum height. </returns>
		// Token: 0x17001646 RID: 5702
		// (get) Token: 0x06005BEF RID: 23535 RVA: 0x0014FEA4 File Offset: 0x0014E0A4
		// (set) Token: 0x06005BF0 RID: 23536 RVA: 0x0019D8BA File Offset: 0x0019BABA
		[TypeConverter(typeof(LengthConverter))]
		public double MaxHeight
		{
			get
			{
				return base.UserMaxSizeValueCache;
			}
			set
			{
				base.SetValue(RowDefinition.MaxHeightProperty, value);
			}
		}

		/// <summary>Gets a value that represents the calculated height of the <see cref="T:System.Windows.Controls.RowDefinition" />.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the calculated height in device independent pixels. The default value is 0.0.</returns>
		// Token: 0x17001647 RID: 5703
		// (get) Token: 0x06005BF1 RID: 23537 RVA: 0x0019D8D0 File Offset: 0x0019BAD0
		public double ActualHeight
		{
			get
			{
				double result = 0.0;
				if (base.InParentLogicalTree)
				{
					result = ((Grid)base.Parent).GetFinalRowDefinitionHeight(base.Index);
				}
				return result;
			}
		}

		/// <summary>Gets a value that represents the offset value of this <see cref="T:System.Windows.Controls.RowDefinition" />.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the offset of the row. The default value is 0.0.</returns>
		// Token: 0x17001648 RID: 5704
		// (get) Token: 0x06005BF2 RID: 23538 RVA: 0x0019D908 File Offset: 0x0019BB08
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

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.RowDefinition.Height" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.RowDefinition.Height" /> dependency property.</returns>
		// Token: 0x04002F9A RID: 12186
		[CommonDependencyProperty]
		public static readonly DependencyProperty HeightProperty = DependencyProperty.Register("Height", typeof(GridLength), typeof(RowDefinition), new FrameworkPropertyMetadata(new GridLength(1.0, GridUnitType.Star), new PropertyChangedCallback(DefinitionBase.OnUserSizePropertyChanged)), new ValidateValueCallback(DefinitionBase.IsUserSizePropertyValueValid));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.RowDefinition.MinHeight" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.RowDefinition.MinHeight" /> dependency property.</returns>
		// Token: 0x04002F9B RID: 12187
		[CommonDependencyProperty]
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		public static readonly DependencyProperty MinHeightProperty = DependencyProperty.Register("MinHeight", typeof(double), typeof(RowDefinition), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(DefinitionBase.OnUserMinSizePropertyChanged)), new ValidateValueCallback(DefinitionBase.IsUserMinSizePropertyValueValid));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.RowDefinition.MaxHeight" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.RowDefinition.MaxHeight" /> dependency property.</returns>
		// Token: 0x04002F9C RID: 12188
		[CommonDependencyProperty]
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		public static readonly DependencyProperty MaxHeightProperty = DependencyProperty.Register("MaxHeight", typeof(double), typeof(RowDefinition), new FrameworkPropertyMetadata(double.PositiveInfinity, new PropertyChangedCallback(DefinitionBase.OnUserMaxSizePropertyChanged)), new ValidateValueCallback(DefinitionBase.IsUserMaxSizePropertyValueValid));
	}
}
