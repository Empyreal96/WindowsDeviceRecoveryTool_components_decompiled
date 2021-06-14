using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Implements the basic functionality that represents the appearance and behavior of a table layout.</summary>
	// Token: 0x02000386 RID: 902
	[TypeConverter(typeof(TableLayoutSettings.StyleConverter))]
	public abstract class TableLayoutStyle
	{
		/// <summary>Gets or sets a flag indicating how a row or column should be sized relative to its containing table.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.SizeType" /> values that specifies how rows or columns of user interface (UI) elements should be sized relative to their container. The default is <see cref="F:System.Windows.Forms.SizeType.AutoSize" />.</returns>
		// Token: 0x17000E26 RID: 3622
		// (get) Token: 0x060038B8 RID: 14520 RVA: 0x000FE3F8 File Offset: 0x000FC5F8
		// (set) Token: 0x060038B9 RID: 14521 RVA: 0x000FE400 File Offset: 0x000FC600
		[DefaultValue(SizeType.AutoSize)]
		public SizeType SizeType
		{
			get
			{
				return this._sizeType;
			}
			set
			{
				if (this._sizeType != value)
				{
					this._sizeType = value;
					if (this.Owner != null)
					{
						LayoutTransaction.DoLayout(this.Owner, this.Owner, PropertyNames.Style);
						Control control = this.Owner as Control;
						if (control != null)
						{
							control.Invalidate();
						}
					}
				}
			}
		}

		// Token: 0x17000E27 RID: 3623
		// (get) Token: 0x060038BA RID: 14522 RVA: 0x000FE450 File Offset: 0x000FC650
		// (set) Token: 0x060038BB RID: 14523 RVA: 0x000FE458 File Offset: 0x000FC658
		internal float Size
		{
			get
			{
				return this._size;
			}
			set
			{
				if (value < 0f)
				{
					throw new ArgumentOutOfRangeException("Size", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"Size",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this._size != value)
				{
					this._size = value;
					if (this.Owner != null)
					{
						LayoutTransaction.DoLayout(this.Owner, this.Owner, PropertyNames.Style);
						Control control = this.Owner as Control;
						if (control != null)
						{
							control.Invalidate();
						}
					}
				}
			}
		}

		// Token: 0x060038BC RID: 14524 RVA: 0x000FE4F3 File Offset: 0x000FC6F3
		private bool ShouldSerializeSize()
		{
			return this.SizeType > SizeType.AutoSize;
		}

		// Token: 0x17000E28 RID: 3624
		// (get) Token: 0x060038BD RID: 14525 RVA: 0x000FE4FE File Offset: 0x000FC6FE
		// (set) Token: 0x060038BE RID: 14526 RVA: 0x000FE506 File Offset: 0x000FC706
		internal IArrangedElement Owner
		{
			get
			{
				return this._owner;
			}
			set
			{
				this._owner = value;
			}
		}

		// Token: 0x060038BF RID: 14527 RVA: 0x000FE50F File Offset: 0x000FC70F
		internal void SetSize(float size)
		{
			this._size = size;
		}

		// Token: 0x04002274 RID: 8820
		private IArrangedElement _owner;

		// Token: 0x04002275 RID: 8821
		private SizeType _sizeType;

		// Token: 0x04002276 RID: 8822
		private float _size;
	}
}
