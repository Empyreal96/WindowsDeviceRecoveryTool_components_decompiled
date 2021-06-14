using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Contains border styles for the cells in a <see cref="T:System.Windows.Forms.DataGridView" /> control.</summary>
	// Token: 0x02000180 RID: 384
	public sealed class DataGridViewAdvancedBorderStyle : ICloneable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewAdvancedBorderStyle" /> class. </summary>
		// Token: 0x06001951 RID: 6481 RVA: 0x0007E07F File Offset: 0x0007C27F
		public DataGridViewAdvancedBorderStyle() : this(null, DataGridViewAdvancedCellBorderStyle.NotSet, DataGridViewAdvancedCellBorderStyle.NotSet, DataGridViewAdvancedCellBorderStyle.NotSet)
		{
		}

		// Token: 0x06001952 RID: 6482 RVA: 0x0007E08B File Offset: 0x0007C28B
		internal DataGridViewAdvancedBorderStyle(DataGridView owner) : this(owner, DataGridViewAdvancedCellBorderStyle.NotSet, DataGridViewAdvancedCellBorderStyle.NotSet, DataGridViewAdvancedCellBorderStyle.NotSet)
		{
		}

		// Token: 0x06001953 RID: 6483 RVA: 0x0007E098 File Offset: 0x0007C298
		internal DataGridViewAdvancedBorderStyle(DataGridView owner, DataGridViewAdvancedCellBorderStyle banned1, DataGridViewAdvancedCellBorderStyle banned2, DataGridViewAdvancedCellBorderStyle banned3)
		{
			this.owner = owner;
			this.banned1 = banned1;
			this.banned2 = banned2;
			this.banned3 = banned3;
		}

		/// <summary>Gets or sets the border style for all of the borders of a cell.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DataGridViewAdvancedCellBorderStyle" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value when setting this property is not a valid <see cref="T:System.Windows.Forms.DataGridViewAdvancedCellBorderStyle" /> values.</exception>
		/// <exception cref="T:System.ArgumentException">The specified value when setting this property is <see cref="F:System.Windows.Forms.DataGridViewAdvancedCellBorderStyle.NotSet" />.-or-The specified value when setting this property is <see cref="F:System.Windows.Forms.DataGridViewAdvancedCellBorderStyle.OutsetDouble" />, <see cref="F:System.Windows.Forms.DataGridViewAdvancedCellBorderStyle.OutsetPartial" />, or <see cref="F:System.Windows.Forms.DataGridViewAdvancedCellBorderStyle.InsetDouble" /> and this <see cref="T:System.Windows.Forms.DataGridViewAdvancedBorderStyle" /> instance was retrieved through the <see cref="P:System.Windows.Forms.DataGridView.AdvancedCellBorderStyle" /> property.</exception>
		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06001954 RID: 6484 RVA: 0x0007E0EB File Offset: 0x0007C2EB
		// (set) Token: 0x06001955 RID: 6485 RVA: 0x0007E100 File Offset: 0x0007C300
		public DataGridViewAdvancedCellBorderStyle All
		{
			get
			{
				if (!this.all)
				{
					return DataGridViewAdvancedCellBorderStyle.NotSet;
				}
				return this.top;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 7))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridViewAdvancedCellBorderStyle));
				}
				if (value == DataGridViewAdvancedCellBorderStyle.NotSet || value == this.banned1 || value == this.banned2 || value == this.banned3)
				{
					throw new ArgumentException(SR.GetString("DataGridView_AdvancedCellBorderStyleInvalid", new object[]
					{
						"All"
					}));
				}
				if (!this.all || this.top != value)
				{
					this.all = true;
					this.bottom = value;
					this.right = value;
					this.left = value;
					this.top = value;
					if (this.owner != null)
					{
						this.owner.OnAdvancedBorderStyleChanged(this);
					}
				}
			}
		}

		/// <summary>Gets or sets the style for the bottom border of a cell.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DataGridViewAdvancedCellBorderStyle" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value when setting this property is not a valid <see cref="T:System.Windows.Forms.DataGridViewAdvancedCellBorderStyle" /> values.</exception>
		/// <exception cref="T:System.ArgumentException">The specified value when setting this property is <see cref="F:System.Windows.Forms.DataGridViewAdvancedCellBorderStyle.NotSet" />.</exception>
		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06001956 RID: 6486 RVA: 0x0007E1BD File Offset: 0x0007C3BD
		// (set) Token: 0x06001957 RID: 6487 RVA: 0x0007E1D4 File Offset: 0x0007C3D4
		public DataGridViewAdvancedCellBorderStyle Bottom
		{
			get
			{
				if (this.all)
				{
					return this.top;
				}
				return this.bottom;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 7))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridViewAdvancedCellBorderStyle));
				}
				if (value == DataGridViewAdvancedCellBorderStyle.NotSet)
				{
					throw new ArgumentException(SR.GetString("DataGridView_AdvancedCellBorderStyleInvalid", new object[]
					{
						"Bottom"
					}));
				}
				this.BottomInternal = value;
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (set) Token: 0x06001958 RID: 6488 RVA: 0x0007E230 File Offset: 0x0007C430
		internal DataGridViewAdvancedCellBorderStyle BottomInternal
		{
			set
			{
				if ((this.all && this.top != value) || (!this.all && this.bottom != value))
				{
					if (this.all && this.right == DataGridViewAdvancedCellBorderStyle.OutsetDouble)
					{
						this.right = DataGridViewAdvancedCellBorderStyle.Outset;
					}
					this.all = false;
					this.bottom = value;
					if (this.owner != null)
					{
						this.owner.OnAdvancedBorderStyleChanged(this);
					}
				}
			}
		}

		/// <summary>Gets the style for the left border of a cell.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DataGridViewAdvancedCellBorderStyle" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value when setting this property is not a valid <see cref="T:System.Windows.Forms.DataGridViewAdvancedCellBorderStyle" />.</exception>
		/// <exception cref="T:System.ArgumentException">The specified value when setting this property is <see cref="F:System.Windows.Forms.DataGridViewAdvancedCellBorderStyle.NotSet" />.-or-The specified value when setting this property is <see cref="F:System.Windows.Forms.DataGridViewAdvancedCellBorderStyle.InsetDouble" /> or <see cref="F:System.Windows.Forms.DataGridViewAdvancedCellBorderStyle.OutsetDouble" /> and this <see cref="T:System.Windows.Forms.DataGridViewAdvancedBorderStyle" /> instance has an associated <see cref="T:System.Windows.Forms.DataGridView" /> control with a <see cref="P:System.Windows.Forms.Control.RightToLeft" /> property value of <see langword="true" />.</exception>
		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06001959 RID: 6489 RVA: 0x0007E299 File Offset: 0x0007C499
		// (set) Token: 0x0600195A RID: 6490 RVA: 0x0007E2B0 File Offset: 0x0007C4B0
		public DataGridViewAdvancedCellBorderStyle Left
		{
			get
			{
				if (this.all)
				{
					return this.top;
				}
				return this.left;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 7))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridViewAdvancedCellBorderStyle));
				}
				if (value == DataGridViewAdvancedCellBorderStyle.NotSet)
				{
					throw new ArgumentException(SR.GetString("DataGridView_AdvancedCellBorderStyleInvalid", new object[]
					{
						"Left"
					}));
				}
				this.LeftInternal = value;
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (set) Token: 0x0600195B RID: 6491 RVA: 0x0007E30C File Offset: 0x0007C50C
		internal DataGridViewAdvancedCellBorderStyle LeftInternal
		{
			set
			{
				if ((this.all && this.top != value) || (!this.all && this.left != value))
				{
					if (this.owner != null && this.owner.RightToLeftInternal && (value == DataGridViewAdvancedCellBorderStyle.InsetDouble || value == DataGridViewAdvancedCellBorderStyle.OutsetDouble))
					{
						throw new ArgumentException(SR.GetString("DataGridView_AdvancedCellBorderStyleInvalid", new object[]
						{
							"Left"
						}));
					}
					if (this.all)
					{
						if (this.right == DataGridViewAdvancedCellBorderStyle.OutsetDouble)
						{
							this.right = DataGridViewAdvancedCellBorderStyle.Outset;
						}
						if (this.bottom == DataGridViewAdvancedCellBorderStyle.OutsetDouble)
						{
							this.bottom = DataGridViewAdvancedCellBorderStyle.Outset;
						}
					}
					this.all = false;
					this.left = value;
					if (this.owner != null)
					{
						this.owner.OnAdvancedBorderStyleChanged(this);
					}
				}
			}
		}

		/// <summary>Gets the style for the right border of a cell.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DataGridViewAdvancedCellBorderStyle" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value when setting this property is not a valid <see cref="T:System.Windows.Forms.DataGridViewAdvancedCellBorderStyle" />.</exception>
		/// <exception cref="T:System.ArgumentException">The specified value when setting this property is <see cref="F:System.Windows.Forms.DataGridViewAdvancedCellBorderStyle.NotSet" />.-or-The specified value when setting this property is <see cref="F:System.Windows.Forms.DataGridViewAdvancedCellBorderStyle.InsetDouble" /> or <see cref="F:System.Windows.Forms.DataGridViewAdvancedCellBorderStyle.OutsetDouble" /> and this <see cref="T:System.Windows.Forms.DataGridViewAdvancedBorderStyle" /> instance has an associated <see cref="T:System.Windows.Forms.DataGridView" /> control with a <see cref="P:System.Windows.Forms.Control.RightToLeft" /> property value of <see langword="false" />.</exception>
		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x0600195C RID: 6492 RVA: 0x0007E3C6 File Offset: 0x0007C5C6
		// (set) Token: 0x0600195D RID: 6493 RVA: 0x0007E3E0 File Offset: 0x0007C5E0
		public DataGridViewAdvancedCellBorderStyle Right
		{
			get
			{
				if (this.all)
				{
					return this.top;
				}
				return this.right;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 7))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridViewAdvancedCellBorderStyle));
				}
				if (value == DataGridViewAdvancedCellBorderStyle.NotSet)
				{
					throw new ArgumentException(SR.GetString("DataGridView_AdvancedCellBorderStyleInvalid", new object[]
					{
						"Right"
					}));
				}
				this.RightInternal = value;
			}
		}

		// Token: 0x170005A9 RID: 1449
		// (set) Token: 0x0600195E RID: 6494 RVA: 0x0007E43C File Offset: 0x0007C63C
		internal DataGridViewAdvancedCellBorderStyle RightInternal
		{
			set
			{
				if ((this.all && this.top != value) || (!this.all && this.right != value))
				{
					if (this.owner != null && !this.owner.RightToLeftInternal && (value == DataGridViewAdvancedCellBorderStyle.InsetDouble || value == DataGridViewAdvancedCellBorderStyle.OutsetDouble))
					{
						throw new ArgumentException(SR.GetString("DataGridView_AdvancedCellBorderStyleInvalid", new object[]
						{
							"Right"
						}));
					}
					if (this.all && this.bottom == DataGridViewAdvancedCellBorderStyle.OutsetDouble)
					{
						this.bottom = DataGridViewAdvancedCellBorderStyle.Outset;
					}
					this.all = false;
					this.right = value;
					if (this.owner != null)
					{
						this.owner.OnAdvancedBorderStyleChanged(this);
					}
				}
			}
		}

		/// <summary>Gets the style for the top border of a cell.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DataGridViewAdvancedCellBorderStyle" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value when setting this property is not a valid <see cref="T:System.Windows.Forms.DataGridViewAdvancedCellBorderStyle" />.</exception>
		/// <exception cref="T:System.ArgumentException">The specified value when setting this property is <see cref="F:System.Windows.Forms.DataGridViewAdvancedCellBorderStyle.NotSet" />.</exception>
		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x0600195F RID: 6495 RVA: 0x0007E4E0 File Offset: 0x0007C6E0
		// (set) Token: 0x06001960 RID: 6496 RVA: 0x0007E4E8 File Offset: 0x0007C6E8
		public DataGridViewAdvancedCellBorderStyle Top
		{
			get
			{
				return this.top;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 7))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridViewAdvancedCellBorderStyle));
				}
				if (value == DataGridViewAdvancedCellBorderStyle.NotSet)
				{
					throw new ArgumentException(SR.GetString("DataGridView_AdvancedCellBorderStyleInvalid", new object[]
					{
						"Top"
					}));
				}
				this.TopInternal = value;
			}
		}

		// Token: 0x170005AB RID: 1451
		// (set) Token: 0x06001961 RID: 6497 RVA: 0x0007E544 File Offset: 0x0007C744
		internal DataGridViewAdvancedCellBorderStyle TopInternal
		{
			set
			{
				if ((this.all && this.top != value) || (!this.all && this.top != value))
				{
					if (this.all)
					{
						if (this.right == DataGridViewAdvancedCellBorderStyle.OutsetDouble)
						{
							this.right = DataGridViewAdvancedCellBorderStyle.Outset;
						}
						if (this.bottom == DataGridViewAdvancedCellBorderStyle.OutsetDouble)
						{
							this.bottom = DataGridViewAdvancedCellBorderStyle.Outset;
						}
					}
					this.all = false;
					this.top = value;
					if (this.owner != null)
					{
						this.owner.OnAdvancedBorderStyleChanged(this);
					}
				}
			}
		}

		/// <summary>Determines whether the specified object is equal to the current <see cref="T:System.Windows.Forms.DataGridViewAdvancedBorderStyle" />.</summary>
		/// <param name="other">An <see cref="T:System.Object" /> to be compared.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="other" /> is a <see cref="T:System.Windows.Forms.DataGridViewAdvancedBorderStyle" /> and the values for the <see cref="P:System.Windows.Forms.DataGridViewAdvancedBorderStyle.Top" />, <see cref="P:System.Windows.Forms.DataGridViewAdvancedBorderStyle.Bottom" />, <see cref="P:System.Windows.Forms.DataGridViewAdvancedBorderStyle.Left" />, and <see cref="P:System.Windows.Forms.DataGridViewAdvancedBorderStyle.Right" /> properties are equal to their counterpart in the current <see cref="T:System.Windows.Forms.DataGridViewAdvancedBorderStyle" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001962 RID: 6498 RVA: 0x0007E5C0 File Offset: 0x0007C7C0
		public override bool Equals(object other)
		{
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle = other as DataGridViewAdvancedBorderStyle;
			return dataGridViewAdvancedBorderStyle != null && (dataGridViewAdvancedBorderStyle.all == this.all && dataGridViewAdvancedBorderStyle.top == this.top && dataGridViewAdvancedBorderStyle.left == this.left && dataGridViewAdvancedBorderStyle.bottom == this.bottom) && dataGridViewAdvancedBorderStyle.right == this.right;
		}

		/// <summary>Serves as a hash function for a particular type.</summary>
		/// <returns>A hash code for the current <see cref="T:System.Object" />.</returns>
		// Token: 0x06001963 RID: 6499 RVA: 0x0007E621 File Offset: 0x0007C821
		public override int GetHashCode()
		{
			return WindowsFormsUtils.GetCombinedHashCodes(new int[]
			{
				(int)this.top,
				(int)this.left,
				(int)this.bottom,
				(int)this.right
			});
		}

		/// <summary>Returns a string that represents the <see cref="T:System.Windows.Forms.DataGridViewAdvancedBorderStyle" />.</summary>
		/// <returns>A string that represents the <see cref="T:System.Windows.Forms.DataGridViewAdvancedBorderStyle" />.</returns>
		// Token: 0x06001964 RID: 6500 RVA: 0x0007E654 File Offset: 0x0007C854
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"DataGridViewAdvancedBorderStyle { All=",
				this.All.ToString(),
				", Left=",
				this.Left.ToString(),
				", Right=",
				this.Right.ToString(),
				", Top=",
				this.Top.ToString(),
				", Bottom=",
				this.Bottom.ToString(),
				" }"
			});
		}

		/// <summary>Creates a new object that is a copy of the current instance.</summary>
		/// <returns>A copy of the current instance.</returns>
		// Token: 0x06001965 RID: 6501 RVA: 0x0007E714 File Offset: 0x0007C914
		object ICloneable.Clone()
		{
			return new DataGridViewAdvancedBorderStyle(this.owner, this.banned1, this.banned2, this.banned3)
			{
				all = this.all,
				top = this.top,
				right = this.right,
				bottom = this.bottom,
				left = this.left
			};
		}

		// Token: 0x04000B6C RID: 2924
		private DataGridView owner;

		// Token: 0x04000B6D RID: 2925
		private bool all = true;

		// Token: 0x04000B6E RID: 2926
		private DataGridViewAdvancedCellBorderStyle banned1;

		// Token: 0x04000B6F RID: 2927
		private DataGridViewAdvancedCellBorderStyle banned2;

		// Token: 0x04000B70 RID: 2928
		private DataGridViewAdvancedCellBorderStyle banned3;

		// Token: 0x04000B71 RID: 2929
		private DataGridViewAdvancedCellBorderStyle top = DataGridViewAdvancedCellBorderStyle.None;

		// Token: 0x04000B72 RID: 2930
		private DataGridViewAdvancedCellBorderStyle left = DataGridViewAdvancedCellBorderStyle.None;

		// Token: 0x04000B73 RID: 2931
		private DataGridViewAdvancedCellBorderStyle right = DataGridViewAdvancedCellBorderStyle.None;

		// Token: 0x04000B74 RID: 2932
		private DataGridViewAdvancedCellBorderStyle bottom = DataGridViewAdvancedCellBorderStyle.None;
	}
}
