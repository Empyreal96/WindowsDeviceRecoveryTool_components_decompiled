using System;
using System.ComponentModel;

namespace System.Windows.Documents
{
	/// <summary>Provides an inline-level flow content element used to host a floater. A floater displays images and other content parallel to the main content flow in a <see cref="T:System.Windows.Documents.FlowDocument" />.</summary>
	// Token: 0x0200036D RID: 877
	public class Floater : AnchoredBlock
	{
		// Token: 0x06002F03 RID: 12035 RVA: 0x000D49B4 File Offset: 0x000D2BB4
		static Floater()
		{
			FrameworkContentElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Floater), new FrameworkPropertyMetadata(typeof(Floater)));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Floater" /> class.</summary>
		// Token: 0x06002F04 RID: 12036 RVA: 0x000D4A4C File Offset: 0x000D2C4C
		public Floater() : this(null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Floater" /> class with the specified <see cref="T:System.Windows.Documents.Block" /> object as its initial content.</summary>
		/// <param name="childBlock">The initial content of the new <see cref="T:System.Windows.Documents.Floater" />.</param>
		// Token: 0x06002F05 RID: 12037 RVA: 0x000D4A56 File Offset: 0x000D2C56
		public Floater(Block childBlock) : this(childBlock, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Floater" /> class with the specified <see cref="T:System.Windows.Documents.Block" /> object as its initial content, and a <see cref="T:System.Windows.Documents.TextPointer" /> that specifies an insertion position for the new <see cref="T:System.Windows.Documents.Floater" />.</summary>
		/// <param name="childBlock">The initial content of the new <see cref="T:System.Windows.Documents.Floater" />. This parameter can be <see langword="null" />, in which case no <see cref="T:System.Windows.Documents.Block" /> is inserted.</param>
		/// <param name="insertionPosition">The position at which to insert the <see cref="T:System.Windows.Documents.Floater" /> element after it is created.</param>
		// Token: 0x06002F06 RID: 12038 RVA: 0x000C95CF File Offset: 0x000C77CF
		public Floater(Block childBlock, TextPointer insertionPosition) : base(childBlock, insertionPosition)
		{
		}

		/// <summary>Gets or sets a value that indicates the horizontal alignment for a <see cref="T:System.Windows.Documents.Floater" /> object. </summary>
		/// <returns>A member of the <see cref="T:System.Windows.HorizontalAlignment" /> enumeration specifying the horizontal alignment for the <see cref="T:System.Windows.Documents.Floater" />. The default is <see cref="F:System.Windows.HorizontalAlignment.Stretch" />.</returns>
		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x06002F07 RID: 12039 RVA: 0x000D4A60 File Offset: 0x000D2C60
		// (set) Token: 0x06002F08 RID: 12040 RVA: 0x000D4A72 File Offset: 0x000D2C72
		public HorizontalAlignment HorizontalAlignment
		{
			get
			{
				return (HorizontalAlignment)base.GetValue(Floater.HorizontalAlignmentProperty);
			}
			set
			{
				base.SetValue(Floater.HorizontalAlignmentProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates the width of a <see cref="T:System.Windows.Documents.Floater" /> object. </summary>
		/// <returns>The width of the <see cref="T:System.Windows.Documents.Floater" />, in device independent pixels. The default value is <see cref="F:System.Double.NaN" /> (equivalent to an attribute value of Auto), which indicates that the line height is determined automatically. </returns>
		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x06002F09 RID: 12041 RVA: 0x000D4A85 File Offset: 0x000D2C85
		// (set) Token: 0x06002F0A RID: 12042 RVA: 0x000D4A97 File Offset: 0x000D2C97
		[TypeConverter(typeof(LengthConverter))]
		public double Width
		{
			get
			{
				return (double)base.GetValue(Floater.WidthProperty);
			}
			set
			{
				base.SetValue(Floater.WidthProperty, value);
			}
		}

		// Token: 0x06002F0B RID: 12043 RVA: 0x000D4AAC File Offset: 0x000D2CAC
		private static bool IsValidWidth(object o)
		{
			double num = (double)o;
			double num2 = (double)Math.Min(1000000, 3500000);
			return double.IsNaN(num) || (num >= 0.0 && num <= num2);
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Floater.HorizontalAlignment" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Floater.HorizontalAlignment" /> dependency property.</returns>
		// Token: 0x04001E1C RID: 7708
		public static readonly DependencyProperty HorizontalAlignmentProperty = FrameworkElement.HorizontalAlignmentProperty.AddOwner(typeof(Floater), new FrameworkPropertyMetadata(HorizontalAlignment.Stretch, FrameworkPropertyMetadataOptions.AffectsMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Floater.Width" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Floater.Width" /> dependency property.</returns>
		// Token: 0x04001E1D RID: 7709
		public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(double), typeof(Floater), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(Floater.IsValidWidth));
	}
}
