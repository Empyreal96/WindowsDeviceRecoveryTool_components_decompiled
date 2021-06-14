using System;

namespace System.Windows.Forms
{
	/// <summary>Represents the look and feel of a row in a table layout.</summary>
	// Token: 0x02000385 RID: 901
	public class RowStyle : TableLayoutStyle
	{
		/// <summary>Initializes an instance of the <see cref="T:System.Windows.Forms.RowStyle" /> class to its default state.</summary>
		// Token: 0x060038B3 RID: 14515 RVA: 0x000FE3A4 File Offset: 0x000FC5A4
		public RowStyle()
		{
		}

		/// <summary>Initializes an instance of the <see cref="T:System.Windows.Forms.RowStyle" /> class using the supplied <see cref="T:System.Windows.Forms.SizeType" /> value.</summary>
		/// <param name="sizeType">A <see cref="P:System.Windows.Forms.TableLayoutStyle.SizeType" /> indicating how the row should be should be sized relative to its containing table.</param>
		// Token: 0x060038B4 RID: 14516 RVA: 0x000FE3AC File Offset: 0x000FC5AC
		public RowStyle(SizeType sizeType)
		{
			base.SizeType = sizeType;
		}

		/// <summary>Initializes an instance of the <see cref="T:System.Windows.Forms.RowStyle" /> class using the supplied <see cref="T:System.Windows.Forms.SizeType" /> and height values.</summary>
		/// <param name="sizeType">A <see cref="P:System.Windows.Forms.TableLayoutStyle.SizeType" /> indicating how the row should be should be sized relative to its containing table.</param>
		/// <param name="height">The preferred height in pixels or percentage of the <see cref="T:System.Windows.Forms.TableLayoutPanel" />, depending on <paramref name="sizeType" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="height" /> is less than 0.</exception>
		// Token: 0x060038B5 RID: 14517 RVA: 0x000FE3E2 File Offset: 0x000FC5E2
		public RowStyle(SizeType sizeType, float height)
		{
			base.SizeType = sizeType;
			this.Height = height;
		}

		/// <summary>Gets or sets the height of a row.</summary>
		/// <returns>The preferred height of a row in pixels or percentage of the <see cref="T:System.Windows.Forms.TableLayoutPanel" />, depending on the <see cref="P:System.Windows.Forms.TableLayoutStyle.SizeType" /> property.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value is less than 0 when setting this property.</exception>
		// Token: 0x17000E25 RID: 3621
		// (get) Token: 0x060038B6 RID: 14518 RVA: 0x000FE3D1 File Offset: 0x000FC5D1
		// (set) Token: 0x060038B7 RID: 14519 RVA: 0x000FE3D9 File Offset: 0x000FC5D9
		public float Height
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
