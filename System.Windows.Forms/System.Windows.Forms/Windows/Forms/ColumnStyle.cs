using System;

namespace System.Windows.Forms
{
	/// <summary>Represents the look and feel of a column in a table layout.</summary>
	// Token: 0x02000384 RID: 900
	public class ColumnStyle : TableLayoutStyle
	{
		/// <summary>Initializes and instance of the <see cref="T:System.Windows.Forms.ColumnStyle" /> class to its default state.</summary>
		// Token: 0x060038AE RID: 14510 RVA: 0x000FE3A4 File Offset: 0x000FC5A4
		public ColumnStyle()
		{
		}

		/// <summary>Initializes an instance of the <see cref="T:System.Windows.Forms.ColumnStyle" /> class using the supplied <see cref="T:System.Windows.Forms.SizeType" /> value.</summary>
		/// <param name="sizeType">A <see cref="P:System.Windows.Forms.TableLayoutStyle.SizeType" /> indicating how the column should be should be sized relative to its containing table.</param>
		// Token: 0x060038AF RID: 14511 RVA: 0x000FE3AC File Offset: 0x000FC5AC
		public ColumnStyle(SizeType sizeType)
		{
			base.SizeType = sizeType;
		}

		/// <summary>Initializes and instance of the <see cref="T:System.Windows.Forms.ColumnStyle" /> class using the supplied <see cref="T:System.Windows.Forms.SizeType" /> and width values.</summary>
		/// <param name="sizeType">A <see cref="P:System.Windows.Forms.TableLayoutStyle.SizeType" /> indicating how the column should be should be sized relative to its containing table.</param>
		/// <param name="width">The preferred width, in pixels or percentage, depending on the <paramref name="sizeType" /> parameter.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="width" /> is less than 0.</exception>
		// Token: 0x060038B0 RID: 14512 RVA: 0x000FE3BB File Offset: 0x000FC5BB
		public ColumnStyle(SizeType sizeType, float width)
		{
			base.SizeType = sizeType;
			this.Width = width;
		}

		/// <summary>Gets or sets the width value for a column.</summary>
		/// <returns>The preferred width, in pixels or percentage, depending on the <see cref="P:System.Windows.Forms.TableLayoutStyle.SizeType" /> property.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value is less than 0 when setting this property.</exception>
		// Token: 0x17000E24 RID: 3620
		// (get) Token: 0x060038B1 RID: 14513 RVA: 0x000FE3D1 File Offset: 0x000FC5D1
		// (set) Token: 0x060038B2 RID: 14514 RVA: 0x000FE3D9 File Offset: 0x000FC5D9
		public float Width
		{
			get
			{
				return base.Size;
			}
			set
			{
				base.Size = value;
			}
		}
	}
}
