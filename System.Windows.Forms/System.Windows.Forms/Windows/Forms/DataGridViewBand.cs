using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace System.Windows.Forms
{
	/// <summary>Represents a linear collection of elements in a <see cref="T:System.Windows.Forms.DataGridView" /> control.</summary>
	// Token: 0x0200018E RID: 398
	public class DataGridViewBand : DataGridViewElement, ICloneable, IDisposable
	{
		// Token: 0x0600196D RID: 6509 RVA: 0x0007E7D0 File Offset: 0x0007C9D0
		internal DataGridViewBand()
		{
			this.propertyStore = new PropertyStore();
			this.bandIndex = -1;
		}

		/// <summary>Releases the resources associated with the band.</summary>
		// Token: 0x0600196E RID: 6510 RVA: 0x0007E7EC File Offset: 0x0007C9EC
		~DataGridViewBand()
		{
			this.Dispose(false);
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x0600196F RID: 6511 RVA: 0x0007E81C File Offset: 0x0007CA1C
		// (set) Token: 0x06001970 RID: 6512 RVA: 0x0007E824 File Offset: 0x0007CA24
		internal int CachedThickness
		{
			get
			{
				return this.cachedThickness;
			}
			set
			{
				this.cachedThickness = value;
			}
		}

		/// <summary>Gets or sets the shortcut menu for the band.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ContextMenuStrip" /> associated with the current <see cref="T:System.Windows.Forms.DataGridViewBand" />. The default is <see langword="null" />.</returns>
		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06001971 RID: 6513 RVA: 0x0007E82D File Offset: 0x0007CA2D
		// (set) Token: 0x06001972 RID: 6514 RVA: 0x0007E84F File Offset: 0x0007CA4F
		[DefaultValue(null)]
		public virtual ContextMenuStrip ContextMenuStrip
		{
			get
			{
				if (this.bandIsRow)
				{
					return ((DataGridViewRow)this).GetContextMenuStrip(this.Index);
				}
				return this.ContextMenuStripInternal;
			}
			set
			{
				this.ContextMenuStripInternal = value;
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06001973 RID: 6515 RVA: 0x0007E858 File Offset: 0x0007CA58
		// (set) Token: 0x06001974 RID: 6516 RVA: 0x0007E870 File Offset: 0x0007CA70
		internal ContextMenuStrip ContextMenuStripInternal
		{
			get
			{
				return (ContextMenuStrip)this.Properties.GetObject(DataGridViewBand.PropContextMenuStrip);
			}
			set
			{
				ContextMenuStrip contextMenuStrip = (ContextMenuStrip)this.Properties.GetObject(DataGridViewBand.PropContextMenuStrip);
				if (contextMenuStrip != value)
				{
					EventHandler value2 = new EventHandler(this.DetachContextMenuStrip);
					if (contextMenuStrip != null)
					{
						contextMenuStrip.Disposed -= value2;
					}
					this.Properties.SetObject(DataGridViewBand.PropContextMenuStrip, value);
					if (value != null)
					{
						value.Disposed += value2;
					}
					if (base.DataGridView != null)
					{
						base.DataGridView.OnBandContextMenuStripChanged(this);
					}
				}
			}
		}

		/// <summary>Gets or sets the default cell style of the band.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> associated with the <see cref="T:System.Windows.Forms.DataGridViewBand" />.</returns>
		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06001975 RID: 6517 RVA: 0x0007E8E0 File Offset: 0x0007CAE0
		// (set) Token: 0x06001976 RID: 6518 RVA: 0x0007E938 File Offset: 0x0007CB38
		[Browsable(false)]
		public virtual DataGridViewCellStyle DefaultCellStyle
		{
			get
			{
				DataGridViewCellStyle dataGridViewCellStyle = (DataGridViewCellStyle)this.Properties.GetObject(DataGridViewBand.PropDefaultCellStyle);
				if (dataGridViewCellStyle == null)
				{
					dataGridViewCellStyle = new DataGridViewCellStyle();
					dataGridViewCellStyle.AddScope(base.DataGridView, this.bandIsRow ? DataGridViewCellStyleScopes.Row : DataGridViewCellStyleScopes.Column);
					this.Properties.SetObject(DataGridViewBand.PropDefaultCellStyle, dataGridViewCellStyle);
				}
				return dataGridViewCellStyle;
			}
			set
			{
				DataGridViewCellStyle dataGridViewCellStyle = null;
				if (this.HasDefaultCellStyle)
				{
					dataGridViewCellStyle = this.DefaultCellStyle;
					dataGridViewCellStyle.RemoveScope(this.bandIsRow ? DataGridViewCellStyleScopes.Row : DataGridViewCellStyleScopes.Column);
				}
				if (value != null || this.Properties.ContainsObject(DataGridViewBand.PropDefaultCellStyle))
				{
					if (value != null)
					{
						value.AddScope(base.DataGridView, this.bandIsRow ? DataGridViewCellStyleScopes.Row : DataGridViewCellStyleScopes.Column);
					}
					this.Properties.SetObject(DataGridViewBand.PropDefaultCellStyle, value);
				}
				if (((dataGridViewCellStyle != null && value == null) || (dataGridViewCellStyle == null && value != null) || (dataGridViewCellStyle != null && value != null && !dataGridViewCellStyle.Equals(this.DefaultCellStyle))) && base.DataGridView != null)
				{
					base.DataGridView.OnBandDefaultCellStyleChanged(this);
				}
			}
		}

		/// <summary>Gets or sets the run-time type of the default header cell.</summary>
		/// <returns>A <see cref="T:System.Type" /> that describes the run-time class of the object used as the default header cell.</returns>
		/// <exception cref="T:System.ArgumentException">The specified value when setting this property is not a <see cref="T:System.Type" /> representing <see cref="T:System.Windows.Forms.DataGridViewHeaderCell" /> or a derived type. </exception>
		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06001977 RID: 6519 RVA: 0x0007E9E0 File Offset: 0x0007CBE0
		// (set) Token: 0x06001978 RID: 6520 RVA: 0x0007EA30 File Offset: 0x0007CC30
		[Browsable(false)]
		public Type DefaultHeaderCellType
		{
			get
			{
				Type type = (Type)this.Properties.GetObject(DataGridViewBand.PropDefaultHeaderCellType);
				if (type == null)
				{
					if (this.bandIsRow)
					{
						type = typeof(DataGridViewRowHeaderCell);
					}
					else
					{
						type = typeof(DataGridViewColumnHeaderCell);
					}
				}
				return type;
			}
			set
			{
				if (!(value != null) && !this.Properties.ContainsObject(DataGridViewBand.PropDefaultHeaderCellType))
				{
					return;
				}
				if (Type.GetType("System.Windows.Forms.DataGridViewHeaderCell").IsAssignableFrom(value))
				{
					this.Properties.SetObject(DataGridViewBand.PropDefaultHeaderCellType, value);
					return;
				}
				throw new ArgumentException(SR.GetString("DataGridView_WrongType", new object[]
				{
					"DefaultHeaderCellType",
					"System.Windows.Forms.DataGridViewHeaderCell"
				}));
			}
		}

		/// <summary>Gets a value indicating whether the band is currently displayed onscreen. </summary>
		/// <returns>
		///     <see langword="true" /> if the band is currently onscreen; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06001979 RID: 6521 RVA: 0x0007EAA4 File Offset: 0x0007CCA4
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool Displayed
		{
			get
			{
				return (this.State & DataGridViewElementStates.Displayed) > DataGridViewElementStates.None;
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (set) Token: 0x0600197A RID: 6522 RVA: 0x0007EABE File Offset: 0x0007CCBE
		internal bool DisplayedInternal
		{
			set
			{
				if (value)
				{
					base.StateInternal = (this.State | DataGridViewElementStates.Displayed);
				}
				else
				{
					base.StateInternal = (this.State & ~DataGridViewElementStates.Displayed);
				}
				if (base.DataGridView != null)
				{
					this.OnStateChanged(DataGridViewElementStates.Displayed);
				}
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x0600197B RID: 6523 RVA: 0x0007EAF4 File Offset: 0x0007CCF4
		// (set) Token: 0x0600197C RID: 6524 RVA: 0x0007EB1C File Offset: 0x0007CD1C
		internal int DividerThickness
		{
			get
			{
				bool flag;
				int integer = this.Properties.GetInteger(DataGridViewBand.PropDividerThickness, out flag);
				if (!flag)
				{
					return 0;
				}
				return integer;
			}
			set
			{
				if (value < 0)
				{
					if (this.bandIsRow)
					{
						throw new ArgumentOutOfRangeException("DividerHeight", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"DividerHeight",
							value.ToString(CultureInfo.CurrentCulture),
							0.ToString(CultureInfo.CurrentCulture)
						}));
					}
					throw new ArgumentOutOfRangeException("DividerWidth", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"DividerWidth",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				else
				{
					if (value <= 65536)
					{
						if (value != this.DividerThickness)
						{
							this.Properties.SetInteger(DataGridViewBand.PropDividerThickness, value);
							if (base.DataGridView != null)
							{
								base.DataGridView.OnBandDividerThicknessChanged(this);
							}
						}
						return;
					}
					if (this.bandIsRow)
					{
						throw new ArgumentOutOfRangeException("DividerHeight", SR.GetString("InvalidHighBoundArgumentEx", new object[]
						{
							"DividerHeight",
							value.ToString(CultureInfo.CurrentCulture),
							65536.ToString(CultureInfo.CurrentCulture)
						}));
					}
					throw new ArgumentOutOfRangeException("DividerWidth", SR.GetString("InvalidHighBoundArgumentEx", new object[]
					{
						"DividerWidth",
						value.ToString(CultureInfo.CurrentCulture),
						65536.ToString(CultureInfo.CurrentCulture)
					}));
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the band will move when a user scrolls through the <see cref="T:System.Windows.Forms.DataGridView" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the band cannot be scrolled from view; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x0600197D RID: 6525 RVA: 0x0007EC8D File Offset: 0x0007CE8D
		// (set) Token: 0x0600197E RID: 6526 RVA: 0x0007EC9A File Offset: 0x0007CE9A
		[DefaultValue(false)]
		public virtual bool Frozen
		{
			get
			{
				return (this.State & DataGridViewElementStates.Frozen) > DataGridViewElementStates.None;
			}
			set
			{
				if ((this.State & DataGridViewElementStates.Frozen) > DataGridViewElementStates.None != value)
				{
					this.OnStateChanging(DataGridViewElementStates.Frozen);
					if (value)
					{
						base.StateInternal = (this.State | DataGridViewElementStates.Frozen);
					}
					else
					{
						base.StateInternal = (this.State & ~DataGridViewElementStates.Frozen);
					}
					this.OnStateChanged(DataGridViewElementStates.Frozen);
				}
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="P:System.Windows.Forms.DataGridViewBand.DefaultCellStyle" /> property has been set. </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Forms.DataGridViewBand.DefaultCellStyle" /> property has been set; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x0600197F RID: 6527 RVA: 0x0007ECDA File Offset: 0x0007CEDA
		[Browsable(false)]
		public bool HasDefaultCellStyle
		{
			get
			{
				return this.Properties.ContainsObject(DataGridViewBand.PropDefaultCellStyle) && this.Properties.GetObject(DataGridViewBand.PropDefaultCellStyle) != null;
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06001980 RID: 6528 RVA: 0x0007ED03 File Offset: 0x0007CF03
		internal bool HasDefaultHeaderCellType
		{
			get
			{
				return this.Properties.ContainsObject(DataGridViewBand.PropDefaultHeaderCellType) && this.Properties.GetObject(DataGridViewBand.PropDefaultHeaderCellType) != null;
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06001981 RID: 6529 RVA: 0x0007ED2C File Offset: 0x0007CF2C
		internal bool HasHeaderCell
		{
			get
			{
				return this.Properties.ContainsObject(DataGridViewBand.PropHeaderCell) && this.Properties.GetObject(DataGridViewBand.PropHeaderCell) != null;
			}
		}

		/// <summary>Gets or sets the header cell of the <see cref="T:System.Windows.Forms.DataGridViewBand" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewHeaderCell" /> representing the header cell of the <see cref="T:System.Windows.Forms.DataGridViewBand" />.</returns>
		/// <exception cref="T:System.ArgumentException">The specified value when setting this property is not a <see cref="T:System.Windows.Forms.DataGridViewRowHeaderCell" /> and this <see cref="T:System.Windows.Forms.DataGridViewBand" /> instance is of type <see cref="T:System.Windows.Forms.DataGridViewRow" />.-or-The specified value when setting this property is not a <see cref="T:System.Windows.Forms.DataGridViewColumnHeaderCell" /> and this <see cref="T:System.Windows.Forms.DataGridViewBand" /> instance is of type <see cref="T:System.Windows.Forms.DataGridViewColumn" />.</exception>
		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06001982 RID: 6530 RVA: 0x0007ED58 File Offset: 0x0007CF58
		// (set) Token: 0x06001983 RID: 6531 RVA: 0x0007EE18 File Offset: 0x0007D018
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected DataGridViewHeaderCell HeaderCellCore
		{
			get
			{
				DataGridViewHeaderCell dataGridViewHeaderCell = (DataGridViewHeaderCell)this.Properties.GetObject(DataGridViewBand.PropHeaderCell);
				if (dataGridViewHeaderCell == null)
				{
					Type defaultHeaderCellType = this.DefaultHeaderCellType;
					dataGridViewHeaderCell = (DataGridViewHeaderCell)SecurityUtils.SecureCreateInstance(defaultHeaderCellType);
					dataGridViewHeaderCell.DataGridViewInternal = base.DataGridView;
					if (this.bandIsRow)
					{
						dataGridViewHeaderCell.OwningRowInternal = (DataGridViewRow)this;
						this.Properties.SetObject(DataGridViewBand.PropHeaderCell, dataGridViewHeaderCell);
					}
					else
					{
						DataGridViewColumn dataGridViewColumn = this as DataGridViewColumn;
						dataGridViewHeaderCell.OwningColumnInternal = dataGridViewColumn;
						this.Properties.SetObject(DataGridViewBand.PropHeaderCell, dataGridViewHeaderCell);
						if (base.DataGridView != null && base.DataGridView.SortedColumn == dataGridViewColumn)
						{
							DataGridViewColumnHeaderCell dataGridViewColumnHeaderCell = dataGridViewHeaderCell as DataGridViewColumnHeaderCell;
							dataGridViewColumnHeaderCell.SortGlyphDirection = base.DataGridView.SortOrder;
						}
					}
				}
				return dataGridViewHeaderCell;
			}
			set
			{
				DataGridViewHeaderCell dataGridViewHeaderCell = (DataGridViewHeaderCell)this.Properties.GetObject(DataGridViewBand.PropHeaderCell);
				if (value != null || this.Properties.ContainsObject(DataGridViewBand.PropHeaderCell))
				{
					if (dataGridViewHeaderCell != null)
					{
						dataGridViewHeaderCell.DataGridViewInternal = null;
						if (this.bandIsRow)
						{
							dataGridViewHeaderCell.OwningRowInternal = null;
						}
						else
						{
							dataGridViewHeaderCell.OwningColumnInternal = null;
							((DataGridViewColumnHeaderCell)dataGridViewHeaderCell).SortGlyphDirectionInternal = SortOrder.None;
						}
					}
					if (value != null)
					{
						if (this.bandIsRow)
						{
							if (!(value is DataGridViewRowHeaderCell))
							{
								throw new ArgumentException(SR.GetString("DataGridView_WrongType", new object[]
								{
									"HeaderCell",
									"System.Windows.Forms.DataGridViewRowHeaderCell"
								}));
							}
							if (value.OwningRow != null)
							{
								value.OwningRow.HeaderCell = null;
							}
							value.OwningRowInternal = (DataGridViewRow)this;
						}
						else
						{
							if (!(value is DataGridViewColumnHeaderCell))
							{
								throw new ArgumentException(SR.GetString("DataGridView_WrongType", new object[]
								{
									"HeaderCell",
									"System.Windows.Forms.DataGridViewColumnHeaderCell"
								}));
							}
							if (value.OwningColumn != null)
							{
								value.OwningColumn.HeaderCell = null;
							}
							value.OwningColumnInternal = (DataGridViewColumn)this;
						}
						value.DataGridViewInternal = base.DataGridView;
					}
					this.Properties.SetObject(DataGridViewBand.PropHeaderCell, value);
				}
				if (((value == null && dataGridViewHeaderCell != null) || (value != null && dataGridViewHeaderCell == null) || (value != null && dataGridViewHeaderCell != null && !dataGridViewHeaderCell.Equals(value))) && base.DataGridView != null)
				{
					base.DataGridView.OnBandHeaderCellChanged(this);
				}
			}
		}

		/// <summary>Gets the relative position of the band within the <see cref="T:System.Windows.Forms.DataGridView" /> control.</summary>
		/// <returns>The zero-based position of the band in the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" /> or <see cref="T:System.Windows.Forms.DataGridViewColumnCollection" /> that it is contained within. The default is -1, indicating that there is no associated <see cref="T:System.Windows.Forms.DataGridView" /> control.</returns>
		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06001984 RID: 6532 RVA: 0x0007EF7B File Offset: 0x0007D17B
		[Browsable(false)]
		public int Index
		{
			get
			{
				return this.bandIndex;
			}
		}

		// Token: 0x170005BE RID: 1470
		// (set) Token: 0x06001985 RID: 6533 RVA: 0x0007EF83 File Offset: 0x0007D183
		internal int IndexInternal
		{
			set
			{
				this.bandIndex = value;
			}
		}

		/// <summary>Gets the cell style in effect for the current band, taking into account style inheritance.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> associated with the <see cref="T:System.Windows.Forms.DataGridViewBand" />. The default is <see langword="null" />.</returns>
		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06001986 RID: 6534 RVA: 0x0000DE5C File Offset: 0x0000C05C
		[Browsable(false)]
		public virtual DataGridViewCellStyle InheritedStyle
		{
			get
			{
				return null;
			}
		}

		/// <summary>Gets a value indicating whether the band represents a row.</summary>
		/// <returns>
		///     <see langword="true" /> if the band represents a <see cref="T:System.Windows.Forms.DataGridViewRow" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06001987 RID: 6535 RVA: 0x0007EF8C File Offset: 0x0007D18C
		protected bool IsRow
		{
			get
			{
				return this.bandIsRow;
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06001988 RID: 6536 RVA: 0x0007EF94 File Offset: 0x0007D194
		// (set) Token: 0x06001989 RID: 6537 RVA: 0x0007EFCC File Offset: 0x0007D1CC
		internal int MinimumThickness
		{
			get
			{
				if (this.bandIsRow && this.bandIndex > -1)
				{
					int num;
					int result;
					this.GetHeightInfo(this.bandIndex, out num, out result);
					return result;
				}
				return this.minimumThickness;
			}
			set
			{
				if (this.minimumThickness != value)
				{
					if (value < 2)
					{
						if (this.bandIsRow)
						{
							throw new ArgumentOutOfRangeException("MinimumHeight", value, SR.GetString("DataGridViewBand_MinimumHeightSmallerThanOne", new object[]
							{
								2.ToString(CultureInfo.CurrentCulture)
							}));
						}
						throw new ArgumentOutOfRangeException("MinimumWidth", value, SR.GetString("DataGridViewBand_MinimumWidthSmallerThanOne", new object[]
						{
							2.ToString(CultureInfo.CurrentCulture)
						}));
					}
					else
					{
						if (this.Thickness < value)
						{
							if (base.DataGridView != null && !this.bandIsRow)
							{
								base.DataGridView.OnColumnMinimumWidthChanging((DataGridViewColumn)this, value);
							}
							this.Thickness = value;
						}
						this.minimumThickness = value;
						if (base.DataGridView != null)
						{
							base.DataGridView.OnBandMinimumThicknessChanged(this);
						}
					}
				}
			}
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x0600198A RID: 6538 RVA: 0x0007F0A2 File Offset: 0x0007D2A2
		internal PropertyStore Properties
		{
			get
			{
				return this.propertyStore;
			}
		}

		/// <summary>Gets or sets a value indicating whether the user can edit the band's cells.</summary>
		/// <returns>
		///     <see langword="true" /> if the user cannot edit the band's cells; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">When setting this property, this <see cref="T:System.Windows.Forms.DataGridViewBand" /> instance is a shared <see cref="T:System.Windows.Forms.DataGridViewRow" />.</exception>
		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x0600198B RID: 6539 RVA: 0x0007F0AA File Offset: 0x0007D2AA
		// (set) Token: 0x0600198C RID: 6540 RVA: 0x0007F0D0 File Offset: 0x0007D2D0
		[DefaultValue(false)]
		public virtual bool ReadOnly
		{
			get
			{
				return (this.State & DataGridViewElementStates.ReadOnly) != DataGridViewElementStates.None || (base.DataGridView != null && base.DataGridView.ReadOnly);
			}
			set
			{
				if (base.DataGridView == null)
				{
					if ((this.State & DataGridViewElementStates.ReadOnly) > DataGridViewElementStates.None != value)
					{
						if (value)
						{
							if (this.bandIsRow)
							{
								foreach (object obj in ((DataGridViewRow)this).Cells)
								{
									DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
									if (dataGridViewCell.ReadOnly)
									{
										dataGridViewCell.ReadOnlyInternal = false;
									}
								}
							}
							base.StateInternal = (this.State | DataGridViewElementStates.ReadOnly);
							return;
						}
						base.StateInternal = (this.State & ~DataGridViewElementStates.ReadOnly);
					}
					return;
				}
				if (base.DataGridView.ReadOnly)
				{
					return;
				}
				if (!this.bandIsRow)
				{
					this.OnStateChanging(DataGridViewElementStates.ReadOnly);
					base.DataGridView.SetReadOnlyColumnCore(this.bandIndex, value);
					return;
				}
				if (this.bandIndex == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidPropertySetOnSharedRow", new object[]
					{
						"ReadOnly"
					}));
				}
				this.OnStateChanging(DataGridViewElementStates.ReadOnly);
				base.DataGridView.SetReadOnlyRowCore(this.bandIndex, value);
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (set) Token: 0x0600198D RID: 6541 RVA: 0x0007F1E8 File Offset: 0x0007D3E8
		internal bool ReadOnlyInternal
		{
			set
			{
				if (value)
				{
					base.StateInternal = (this.State | DataGridViewElementStates.ReadOnly);
				}
				else
				{
					base.StateInternal = (this.State & ~DataGridViewElementStates.ReadOnly);
				}
				this.OnStateChanged(DataGridViewElementStates.ReadOnly);
			}
		}

		/// <summary>Gets or sets a value indicating whether the band can be resized in the user interface (UI).</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DataGridViewTriState" /> values. The default is <see cref="F:System.Windows.Forms.DataGridViewTriState.True" />.</returns>
		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x0600198E RID: 6542 RVA: 0x0007F213 File Offset: 0x0007D413
		// (set) Token: 0x0600198F RID: 6543 RVA: 0x0007F248 File Offset: 0x0007D448
		[Browsable(true)]
		public virtual DataGridViewTriState Resizable
		{
			get
			{
				if ((this.State & DataGridViewElementStates.ResizableSet) != DataGridViewElementStates.None)
				{
					if ((this.State & DataGridViewElementStates.Resizable) == DataGridViewElementStates.None)
					{
						return DataGridViewTriState.False;
					}
					return DataGridViewTriState.True;
				}
				else
				{
					if (base.DataGridView == null)
					{
						return DataGridViewTriState.NotSet;
					}
					if (!base.DataGridView.AllowUserToResizeColumns)
					{
						return DataGridViewTriState.False;
					}
					return DataGridViewTriState.True;
				}
			}
			set
			{
				DataGridViewTriState resizable = this.Resizable;
				if (value == DataGridViewTriState.NotSet)
				{
					base.StateInternal = (this.State & ~DataGridViewElementStates.ResizableSet);
				}
				else
				{
					base.StateInternal = (this.State | DataGridViewElementStates.ResizableSet);
					if ((this.State & DataGridViewElementStates.Resizable) > DataGridViewElementStates.None != (value == DataGridViewTriState.True))
					{
						if (value == DataGridViewTriState.True)
						{
							base.StateInternal = (this.State | DataGridViewElementStates.Resizable);
						}
						else
						{
							base.StateInternal = (this.State & ~DataGridViewElementStates.Resizable);
						}
					}
				}
				if (resizable != this.Resizable)
				{
					this.OnStateChanged(DataGridViewElementStates.Resizable);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the band is in a selected user interface (UI) state.</summary>
		/// <returns>
		///     <see langword="true" /> if the band is selected; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The specified value when setting this property is <see langword="true" />, but the band has not been added to a <see cref="T:System.Windows.Forms.DataGridView" /> control. -or-This property is being set on a shared <see cref="T:System.Windows.Forms.DataGridViewRow" />.</exception>
		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06001990 RID: 6544 RVA: 0x0007F2C3 File Offset: 0x0007D4C3
		// (set) Token: 0x06001991 RID: 6545 RVA: 0x0007F2D4 File Offset: 0x0007D4D4
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool Selected
		{
			get
			{
				return (this.State & DataGridViewElementStates.Selected) > DataGridViewElementStates.None;
			}
			set
			{
				if (base.DataGridView != null)
				{
					if (this.bandIsRow)
					{
						if (this.bandIndex == -1)
						{
							throw new InvalidOperationException(SR.GetString("DataGridView_InvalidPropertySetOnSharedRow", new object[]
							{
								"Selected"
							}));
						}
						if (base.DataGridView.SelectionMode == DataGridViewSelectionMode.FullRowSelect || base.DataGridView.SelectionMode == DataGridViewSelectionMode.RowHeaderSelect)
						{
							base.DataGridView.SetSelectedRowCoreInternal(this.bandIndex, value);
							return;
						}
					}
					else if (base.DataGridView.SelectionMode == DataGridViewSelectionMode.FullColumnSelect || base.DataGridView.SelectionMode == DataGridViewSelectionMode.ColumnHeaderSelect)
					{
						base.DataGridView.SetSelectedColumnCoreInternal(this.bandIndex, value);
						return;
					}
				}
				else if (value)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewBand_CannotSelect"));
				}
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (set) Token: 0x06001992 RID: 6546 RVA: 0x0007F38C File Offset: 0x0007D58C
		internal bool SelectedInternal
		{
			set
			{
				if (value)
				{
					base.StateInternal = (this.State | DataGridViewElementStates.Selected);
				}
				else
				{
					base.StateInternal = (this.State & ~DataGridViewElementStates.Selected);
				}
				if (base.DataGridView != null)
				{
					this.OnStateChanged(DataGridViewElementStates.Selected);
				}
			}
		}

		/// <summary>Gets or sets the object that contains data to associate with the band.</summary>
		/// <returns>An <see cref="T:System.Object" /> that contains information associated with the band. The default is <see langword="null" />.</returns>
		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06001993 RID: 6547 RVA: 0x0007F3C1 File Offset: 0x0007D5C1
		// (set) Token: 0x06001994 RID: 6548 RVA: 0x0007F3D3 File Offset: 0x0007D5D3
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object Tag
		{
			get
			{
				return this.Properties.GetObject(DataGridViewBand.PropUserData);
			}
			set
			{
				if (value != null || this.Properties.ContainsObject(DataGridViewBand.PropUserData))
				{
					this.Properties.SetObject(DataGridViewBand.PropUserData, value);
				}
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001995 RID: 6549 RVA: 0x0007F3FC File Offset: 0x0007D5FC
		// (set) Token: 0x06001996 RID: 6550 RVA: 0x0007F434 File Offset: 0x0007D634
		internal int Thickness
		{
			get
			{
				if (this.bandIsRow && this.bandIndex > -1)
				{
					int result;
					int num;
					this.GetHeightInfo(this.bandIndex, out result, out num);
					return result;
				}
				return this.thickness;
			}
			set
			{
				int num = this.MinimumThickness;
				if (value < num)
				{
					value = num;
				}
				if (value <= 65536)
				{
					bool flag = true;
					if (this.bandIsRow)
					{
						if (base.DataGridView != null && base.DataGridView.AutoSizeRowsMode != DataGridViewAutoSizeRowsMode.None)
						{
							this.cachedThickness = value;
							flag = false;
						}
					}
					else
					{
						DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)this;
						DataGridViewAutoSizeColumnMode inheritedAutoSizeMode = dataGridViewColumn.InheritedAutoSizeMode;
						if (inheritedAutoSizeMode != DataGridViewAutoSizeColumnMode.Fill && inheritedAutoSizeMode != DataGridViewAutoSizeColumnMode.None && inheritedAutoSizeMode != DataGridViewAutoSizeColumnMode.NotSet)
						{
							this.cachedThickness = value;
							flag = false;
						}
						else if (inheritedAutoSizeMode == DataGridViewAutoSizeColumnMode.Fill && base.DataGridView != null && dataGridViewColumn.Visible)
						{
							IntPtr handle = base.DataGridView.Handle;
							base.DataGridView.AdjustFillingColumn(dataGridViewColumn, value);
							flag = false;
						}
					}
					if (flag && this.thickness != value)
					{
						if (base.DataGridView != null)
						{
							base.DataGridView.OnBandThicknessChanging();
						}
						this.ThicknessInternal = value;
					}
					return;
				}
				if (this.bandIsRow)
				{
					throw new ArgumentOutOfRangeException("Height", SR.GetString("InvalidHighBoundArgumentEx", new object[]
					{
						"Height",
						value.ToString(CultureInfo.CurrentCulture),
						65536.ToString(CultureInfo.CurrentCulture)
					}));
				}
				throw new ArgumentOutOfRangeException("Width", SR.GetString("InvalidHighBoundArgumentEx", new object[]
				{
					"Width",
					value.ToString(CultureInfo.CurrentCulture),
					65536.ToString(CultureInfo.CurrentCulture)
				}));
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06001997 RID: 6551 RVA: 0x0007F59B File Offset: 0x0007D79B
		// (set) Token: 0x06001998 RID: 6552 RVA: 0x0007F5A3 File Offset: 0x0007D7A3
		internal int ThicknessInternal
		{
			get
			{
				return this.thickness;
			}
			set
			{
				this.thickness = value;
				if (base.DataGridView != null)
				{
					base.DataGridView.OnBandThicknessChanged(this);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the band is visible to the user.</summary>
		/// <returns>
		///     <see langword="true" /> if the band is visible; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The specified value when setting this property is <see langword="false" /> and the band is the row for new records.</exception>
		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06001999 RID: 6553 RVA: 0x0007F5C0 File Offset: 0x0007D7C0
		// (set) Token: 0x0600199A RID: 6554 RVA: 0x0007F5D0 File Offset: 0x0007D7D0
		[DefaultValue(true)]
		public virtual bool Visible
		{
			get
			{
				return (this.State & DataGridViewElementStates.Visible) > DataGridViewElementStates.None;
			}
			set
			{
				if ((this.State & DataGridViewElementStates.Visible) > DataGridViewElementStates.None != value)
				{
					if (base.DataGridView != null && this.bandIsRow && base.DataGridView.NewRowIndex != -1 && base.DataGridView.NewRowIndex == this.bandIndex && !value)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewBand_NewRowCannotBeInvisible"));
					}
					this.OnStateChanging(DataGridViewElementStates.Visible);
					if (value)
					{
						base.StateInternal = (this.State | DataGridViewElementStates.Visible);
					}
					else
					{
						base.StateInternal = (this.State & ~DataGridViewElementStates.Visible);
					}
					this.OnStateChanged(DataGridViewElementStates.Visible);
				}
			}
		}

		/// <summary>Creates an exact copy of this band.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the cloned <see cref="T:System.Windows.Forms.DataGridViewBand" />.</returns>
		// Token: 0x0600199B RID: 6555 RVA: 0x0007F664 File Offset: 0x0007D864
		public virtual object Clone()
		{
			DataGridViewBand dataGridViewBand = (DataGridViewBand)Activator.CreateInstance(base.GetType());
			if (dataGridViewBand != null)
			{
				this.CloneInternal(dataGridViewBand);
			}
			return dataGridViewBand;
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x0007F690 File Offset: 0x0007D890
		internal void CloneInternal(DataGridViewBand dataGridViewBand)
		{
			dataGridViewBand.propertyStore = new PropertyStore();
			dataGridViewBand.bandIndex = -1;
			dataGridViewBand.bandIsRow = this.bandIsRow;
			if (!this.bandIsRow || this.bandIndex >= 0 || base.DataGridView == null)
			{
				dataGridViewBand.StateInternal = (this.State & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Selected));
			}
			dataGridViewBand.thickness = this.Thickness;
			dataGridViewBand.MinimumThickness = this.MinimumThickness;
			dataGridViewBand.cachedThickness = this.CachedThickness;
			dataGridViewBand.DividerThickness = this.DividerThickness;
			dataGridViewBand.Tag = this.Tag;
			if (this.HasDefaultCellStyle)
			{
				dataGridViewBand.DefaultCellStyle = new DataGridViewCellStyle(this.DefaultCellStyle);
			}
			if (this.HasDefaultHeaderCellType)
			{
				dataGridViewBand.DefaultHeaderCellType = this.DefaultHeaderCellType;
			}
			if (this.ContextMenuStripInternal != null)
			{
				dataGridViewBand.ContextMenuStrip = this.ContextMenuStripInternal.Clone();
			}
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x0007F765 File Offset: 0x0007D965
		private void DetachContextMenuStrip(object sender, EventArgs e)
		{
			this.ContextMenuStripInternal = null;
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Windows.Forms.DataGridViewBand" />.  </summary>
		// Token: 0x0600199E RID: 6558 RVA: 0x0007F76E File Offset: 0x0007D96E
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.DataGridViewBand" /> and optionally releases the managed resources.  </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		// Token: 0x0600199F RID: 6559 RVA: 0x0007F780 File Offset: 0x0007D980
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				ContextMenuStrip contextMenuStripInternal = this.ContextMenuStripInternal;
				if (contextMenuStripInternal != null)
				{
					contextMenuStripInternal.Disposed -= this.DetachContextMenuStrip;
				}
			}
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x0007F7AC File Offset: 0x0007D9AC
		internal void GetHeightInfo(int rowIndex, out int height, out int minimumHeight)
		{
			if (base.DataGridView != null && (base.DataGridView.VirtualMode || base.DataGridView.DataSource != null) && base.DataGridView.AutoSizeRowsMode == DataGridViewAutoSizeRowsMode.None)
			{
				DataGridViewRowHeightInfoNeededEventArgs dataGridViewRowHeightInfoNeededEventArgs = base.DataGridView.OnRowHeightInfoNeeded(rowIndex, this.thickness, this.minimumThickness);
				height = dataGridViewRowHeightInfoNeededEventArgs.Height;
				minimumHeight = dataGridViewRowHeightInfoNeededEventArgs.MinimumHeight;
				return;
			}
			height = this.thickness;
			minimumHeight = this.minimumThickness;
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x0007F824 File Offset: 0x0007DA24
		internal void OnStateChanged(DataGridViewElementStates elementState)
		{
			if (base.DataGridView != null)
			{
				if (this.bandIsRow)
				{
					base.DataGridView.Rows.InvalidateCachedRowCount(elementState);
					base.DataGridView.Rows.InvalidateCachedRowsHeight(elementState);
					if (this.bandIndex != -1)
					{
						base.DataGridView.OnDataGridViewElementStateChanged(this, -1, elementState);
						return;
					}
				}
				else
				{
					base.DataGridView.Columns.InvalidateCachedColumnCount(elementState);
					base.DataGridView.Columns.InvalidateCachedColumnsWidth(elementState);
					base.DataGridView.OnDataGridViewElementStateChanged(this, -1, elementState);
				}
			}
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x0007F8AB File Offset: 0x0007DAAB
		private void OnStateChanging(DataGridViewElementStates elementState)
		{
			if (base.DataGridView != null)
			{
				if (this.bandIsRow)
				{
					if (this.bandIndex != -1)
					{
						base.DataGridView.OnDataGridViewElementStateChanging(this, -1, elementState);
						return;
					}
				}
				else
				{
					base.DataGridView.OnDataGridViewElementStateChanging(this, -1, elementState);
				}
			}
		}

		/// <summary>Called when the band is associated with a different <see cref="T:System.Windows.Forms.DataGridView" />.</summary>
		// Token: 0x060019A3 RID: 6563 RVA: 0x0007F8E4 File Offset: 0x0007DAE4
		protected override void OnDataGridViewChanged()
		{
			if (this.HasDefaultCellStyle)
			{
				if (base.DataGridView == null)
				{
					this.DefaultCellStyle.RemoveScope(this.bandIsRow ? DataGridViewCellStyleScopes.Row : DataGridViewCellStyleScopes.Column);
				}
				else
				{
					this.DefaultCellStyle.AddScope(base.DataGridView, this.bandIsRow ? DataGridViewCellStyleScopes.Row : DataGridViewCellStyleScopes.Column);
				}
			}
			base.OnDataGridViewChanged();
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x0007F940 File Offset: 0x0007DB40
		private bool ShouldSerializeDefaultHeaderCellType()
		{
			Type left = (Type)this.Properties.GetObject(DataGridViewBand.PropDefaultHeaderCellType);
			return left != null;
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x0007F96A File Offset: 0x0007DB6A
		internal bool ShouldSerializeResizable()
		{
			return (this.State & DataGridViewElementStates.ResizableSet) > DataGridViewElementStates.None;
		}

		/// <summary>Returns a string that represents the current band.</summary>
		/// <returns>A <see cref="T:System.String" /> that represents the current <see cref="T:System.Windows.Forms.DataGridViewBand" />.</returns>
		// Token: 0x060019A6 RID: 6566 RVA: 0x0007F978 File Offset: 0x0007DB78
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(36);
			stringBuilder.Append("DataGridViewBand { Index=");
			stringBuilder.Append(this.Index.ToString(CultureInfo.CurrentCulture));
			stringBuilder.Append(" }");
			return stringBuilder.ToString();
		}

		// Token: 0x04000BB9 RID: 3001
		private static readonly int PropContextMenuStrip = PropertyStore.CreateKey();

		// Token: 0x04000BBA RID: 3002
		private static readonly int PropDefaultCellStyle = PropertyStore.CreateKey();

		// Token: 0x04000BBB RID: 3003
		private static readonly int PropDefaultHeaderCellType = PropertyStore.CreateKey();

		// Token: 0x04000BBC RID: 3004
		private static readonly int PropDividerThickness = PropertyStore.CreateKey();

		// Token: 0x04000BBD RID: 3005
		private static readonly int PropHeaderCell = PropertyStore.CreateKey();

		// Token: 0x04000BBE RID: 3006
		private static readonly int PropUserData = PropertyStore.CreateKey();

		// Token: 0x04000BBF RID: 3007
		internal const int minBandThickness = 2;

		// Token: 0x04000BC0 RID: 3008
		internal const int maxBandThickness = 65536;

		// Token: 0x04000BC1 RID: 3009
		private PropertyStore propertyStore;

		// Token: 0x04000BC2 RID: 3010
		private int thickness;

		// Token: 0x04000BC3 RID: 3011
		private int cachedThickness;

		// Token: 0x04000BC4 RID: 3012
		private int minimumThickness;

		// Token: 0x04000BC5 RID: 3013
		private int bandIndex;

		// Token: 0x04000BC6 RID: 3014
		internal bool bandIsRow;
	}
}
