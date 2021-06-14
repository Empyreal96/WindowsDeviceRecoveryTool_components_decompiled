using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System.Windows.Forms
{
	/// <summary>Displays ADO.NET data in a scrollable grid. The <see cref="T:System.Windows.Forms.DataGridView" /> control replaces and adds functionality to the <see cref="T:System.Windows.Forms.DataGrid" /> control; however, the <see cref="T:System.Windows.Forms.DataGrid" /> control is retained for both backward compatibility and future use, if you choose. </summary>
	// Token: 0x0200016A RID: 362
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Designer("System.Windows.Forms.Design.DataGridDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultProperty("DataSource")]
	[DefaultEvent("Navigate")]
	[ComplexBindingProperties("DataSource", "DataMember")]
	public class DataGrid : Control, ISupportInitialize, IDataGridEditingService
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGrid" /> class.</summary>
		// Token: 0x060010DE RID: 4318 RVA: 0x0003BE4C File Offset: 0x0003A04C
		public DataGrid()
		{
			base.SetStyle(ControlStyles.UserPaint, true);
			base.SetStyle(ControlStyles.Opaque, false);
			base.SetStyle(ControlStyles.SupportsTransparentBackColor, false);
			base.SetStyle(ControlStyles.UserMouse, true);
			this.gridState = new BitVector32(272423);
			this.dataGridTables = new GridTableStylesCollection(this);
			this.layout = this.CreateInitialLayoutState();
			this.parentRows = new DataGridParentRows(this);
			this.horizScrollBar.Top = base.ClientRectangle.Height - this.horizScrollBar.Height;
			this.horizScrollBar.Left = 0;
			this.horizScrollBar.Visible = false;
			this.horizScrollBar.Scroll += this.GridHScrolled;
			base.Controls.Add(this.horizScrollBar);
			this.vertScrollBar.Top = 0;
			this.vertScrollBar.Left = base.ClientRectangle.Width - this.vertScrollBar.Width;
			this.vertScrollBar.Visible = false;
			this.vertScrollBar.Scroll += this.GridVScrolled;
			base.Controls.Add(this.vertScrollBar);
			this.BackColor = DataGrid.DefaultBackBrush.Color;
			this.ForeColor = DataGrid.DefaultForeBrush.Color;
			this.borderStyle = BorderStyle.Fixed3D;
			this.currentChangedHandler = new EventHandler(this.DataSource_RowChanged);
			this.positionChangedHandler = new EventHandler(this.DataSource_PositionChanged);
			this.itemChangedHandler = new ItemChangedEventHandler(this.DataSource_ItemChanged);
			this.metaDataChangedHandler = new EventHandler(this.DataSource_MetaDataChanged);
			this.dataGridTableStylesCollectionChanged = new CollectionChangeEventHandler(this.TableStylesCollectionChanged);
			this.dataGridTables.CollectionChanged += this.dataGridTableStylesCollectionChanged;
			this.SetDataGridTable(this.defaultTableStyle, true);
			this.backButtonHandler = new EventHandler(this.OnBackButtonClicked);
			this.downButtonHandler = new EventHandler(this.OnShowParentDetailsButtonClicked);
			this.caption = new DataGridCaption(this);
			this.caption.BackwardClicked += this.backButtonHandler;
			this.caption.DownClicked += this.downButtonHandler;
			this.RecalculateFonts();
			base.Size = new Size(130, 80);
			base.Invalidate();
			base.PerformLayout();
		}

		/// <summary>Gets or sets a value indicating whether the grid can be resorted by clicking on a column header.</summary>
		/// <returns>
		///     <see langword="true" /> if columns can be sorted; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x060010DF RID: 4319 RVA: 0x0003C1CE File Offset: 0x0003A3CE
		// (set) Token: 0x060010E0 RID: 4320 RVA: 0x0003C1DC File Offset: 0x0003A3DC
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("DataGridAllowSortingDescr")]
		public bool AllowSorting
		{
			get
			{
				return this.gridState[1];
			}
			set
			{
				if (this.AllowSorting != value)
				{
					this.gridState[1] = value;
					if (!value && this.listManager != null)
					{
						IList list = this.listManager.List;
						if (list is IBindingList)
						{
							((IBindingList)list).RemoveSort();
						}
					}
				}
			}
		}

		/// <summary>Gets or sets the background color of odd-numbered rows of the grid.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the alternating background color. The default is the system color for windows (<see cref="P:System.Drawing.SystemColors.Window" />).</returns>
		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x060010E1 RID: 4321 RVA: 0x0003C229 File Offset: 0x0003A429
		// (set) Token: 0x060010E2 RID: 4322 RVA: 0x0003C238 File Offset: 0x0003A438
		[SRCategory("CatColors")]
		[SRDescription("DataGridAlternatingBackColorDescr")]
		public Color AlternatingBackColor
		{
			get
			{
				return this.alternatingBackBrush.Color;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[]
					{
						"AlternatingBackColor"
					}));
				}
				if (DataGrid.IsTransparentColor(value))
				{
					throw new ArgumentException(SR.GetString("DataGridTransparentAlternatingBackColorNotAllowed"));
				}
				if (!this.alternatingBackBrush.Color.Equals(value))
				{
					this.alternatingBackBrush = new SolidBrush(value);
					this.InvalidateInside();
				}
			}
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.DataGrid.AlternatingBackColor" /> property to its default color.</summary>
		// Token: 0x060010E3 RID: 4323 RVA: 0x0003C2B7 File Offset: 0x0003A4B7
		public void ResetAlternatingBackColor()
		{
			if (this.ShouldSerializeAlternatingBackColor())
			{
				this.AlternatingBackColor = DataGrid.DefaultAlternatingBackBrush.Color;
				this.InvalidateInside();
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Forms.DataGrid.AlternatingBackColor" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x060010E4 RID: 4324 RVA: 0x0003C2D7 File Offset: 0x0003A4D7
		protected virtual bool ShouldSerializeAlternatingBackColor()
		{
			return !this.AlternatingBackBrush.Equals(DataGrid.DefaultAlternatingBackBrush);
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x060010E5 RID: 4325 RVA: 0x0003C2EC File Offset: 0x0003A4EC
		internal Brush AlternatingBackBrush
		{
			get
			{
				return this.alternatingBackBrush;
			}
		}

		/// <summary>Gets or sets the background color of even-numbered rows of the grid.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the color of rows in the grid. The default is the system color for windows (<see cref="P:System.Drawing.SystemColors.Window" />).</returns>
		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x060010E6 RID: 4326 RVA: 0x00011FB1 File Offset: 0x000101B1
		// (set) Token: 0x060010E7 RID: 4327 RVA: 0x0003C2F4 File Offset: 0x0003A4F4
		[SRCategory("CatColors")]
		[SRDescription("ControlBackColorDescr")]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				if (DataGrid.IsTransparentColor(value))
				{
					throw new ArgumentException(SR.GetString("DataGridTransparentBackColorNotAllowed"));
				}
				base.BackColor = value;
			}
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.DataGrid.BackColor" /> property to its default value.</summary>
		// Token: 0x060010E8 RID: 4328 RVA: 0x0003C318 File Offset: 0x0003A518
		public override void ResetBackColor()
		{
			if (!this.BackColor.Equals(DataGrid.DefaultBackBrush.Color))
			{
				this.BackColor = DataGrid.DefaultBackBrush.Color;
			}
		}

		/// <summary>Gets or sets the foreground color (typically the color of the text) property of the <see cref="T:System.Windows.Forms.DataGrid" /> control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the foreground color. The default is <see cref="P:System.Drawing.SystemBrushes.WindowText" /> color.</returns>
		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x060010E9 RID: 4329 RVA: 0x00012082 File Offset: 0x00010282
		// (set) Token: 0x060010EA RID: 4330 RVA: 0x0001208A File Offset: 0x0001028A
		[SRCategory("CatColors")]
		[SRDescription("ControlForeColorDescr")]
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.DataGrid.ForeColor" /> property to its default value.</summary>
		// Token: 0x060010EB RID: 4331 RVA: 0x0003C35C File Offset: 0x0003A55C
		public override void ResetForeColor()
		{
			if (!this.ForeColor.Equals(DataGrid.DefaultForeBrush.Color))
			{
				this.ForeColor = DataGrid.DefaultForeBrush.Color;
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x060010EC RID: 4332 RVA: 0x0003C39E File Offset: 0x0003A59E
		internal SolidBrush BackBrush
		{
			get
			{
				return this.backBrush;
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x060010ED RID: 4333 RVA: 0x0003C3A6 File Offset: 0x0003A5A6
		internal SolidBrush ForeBrush
		{
			get
			{
				return this.foreBrush;
			}
		}

		/// <summary>Gets or sets the grid's border style.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.BorderStyle" /> enumeration values. The default is <see langword="FixedSingle" />.</returns>
		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x060010EE RID: 4334 RVA: 0x0003C3AE File Offset: 0x0003A5AE
		// (set) Token: 0x060010EF RID: 4335 RVA: 0x0003C3B8 File Offset: 0x0003A5B8
		[SRCategory("CatAppearance")]
		[DefaultValue(BorderStyle.Fixed3D)]
		[DispId(-504)]
		[SRDescription("DataGridBorderStyleDescr")]
		public BorderStyle BorderStyle
		{
			get
			{
				return this.borderStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(BorderStyle));
				}
				if (this.borderStyle != value)
				{
					this.borderStyle = value;
					base.PerformLayout();
					base.Invalidate();
					this.OnBorderStyleChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGrid.BorderStyle" /> has changed.</summary>
		// Token: 0x140000B2 RID: 178
		// (add) Token: 0x060010F0 RID: 4336 RVA: 0x0003C412 File Offset: 0x0003A612
		// (remove) Token: 0x060010F1 RID: 4337 RVA: 0x0003C425 File Offset: 0x0003A625
		[SRCategory("CatPropertyChanged")]
		[SRDescription("DataGridOnBorderStyleChangedDescr")]
		public event EventHandler BorderStyleChanged
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_BORDERSTYLECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_BORDERSTYLECHANGED, value);
			}
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x060010F2 RID: 4338 RVA: 0x0003C438 File Offset: 0x0003A638
		private int BorderWidth
		{
			get
			{
				if (this.BorderStyle == BorderStyle.Fixed3D)
				{
					return SystemInformation.Border3DSize.Width;
				}
				if (this.BorderStyle == BorderStyle.FixedSingle)
				{
					return 2;
				}
				return 0;
			}
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>The default size of the control.</returns>
		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x060010F3 RID: 4339 RVA: 0x0003C468 File Offset: 0x0003A668
		protected override Size DefaultSize
		{
			get
			{
				return new Size(130, 80);
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x060010F4 RID: 4340 RVA: 0x0003C476 File Offset: 0x0003A676
		private static SolidBrush DefaultSelectionBackBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.ActiveCaption;
			}
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x060010F5 RID: 4341 RVA: 0x0003C482 File Offset: 0x0003A682
		private static SolidBrush DefaultSelectionForeBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.ActiveCaptionText;
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x060010F6 RID: 4342 RVA: 0x0003C48E File Offset: 0x0003A68E
		internal static SolidBrush DefaultBackBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.Window;
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x060010F7 RID: 4343 RVA: 0x0003C49A File Offset: 0x0003A69A
		internal static SolidBrush DefaultForeBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.WindowText;
			}
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x060010F8 RID: 4344 RVA: 0x0003C4A6 File Offset: 0x0003A6A6
		private static SolidBrush DefaultBackgroundBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.AppWorkspace;
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x060010F9 RID: 4345 RVA: 0x0003C49A File Offset: 0x0003A69A
		internal static SolidBrush DefaultParentRowsForeBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.WindowText;
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x060010FA RID: 4346 RVA: 0x0003C4B2 File Offset: 0x0003A6B2
		internal static SolidBrush DefaultParentRowsBackBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.Control;
			}
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x060010FB RID: 4347 RVA: 0x0003C48E File Offset: 0x0003A68E
		internal static SolidBrush DefaultAlternatingBackBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.Window;
			}
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x060010FC RID: 4348 RVA: 0x0003C4B2 File Offset: 0x0003A6B2
		private static SolidBrush DefaultGridLineBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.Control;
			}
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x060010FD RID: 4349 RVA: 0x0003C4B2 File Offset: 0x0003A6B2
		private static SolidBrush DefaultHeaderBackBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.Control;
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x060010FE RID: 4350 RVA: 0x0003C4BE File Offset: 0x0003A6BE
		private static SolidBrush DefaultHeaderForeBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.ControlText;
			}
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x060010FF RID: 4351 RVA: 0x0003C4CA File Offset: 0x0003A6CA
		private static Pen DefaultHeaderForePen
		{
			get
			{
				return new Pen(SystemColors.ControlText);
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06001100 RID: 4352 RVA: 0x0003C4D6 File Offset: 0x0003A6D6
		private static SolidBrush DefaultLinkBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.HotTrack;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06001101 RID: 4353 RVA: 0x0003C4E2 File Offset: 0x0003A6E2
		// (set) Token: 0x06001102 RID: 4354 RVA: 0x0003C4F4 File Offset: 0x0003A6F4
		private bool ListHasErrors
		{
			get
			{
				return this.gridState[128];
			}
			set
			{
				if (this.ListHasErrors != value)
				{
					this.gridState[128] = value;
					this.ComputeMinimumRowHeaderWidth();
					if (!this.layout.RowHeadersVisible)
					{
						return;
					}
					if (value)
					{
						if (this.myGridTable.IsDefault)
						{
							this.RowHeaderWidth += 15;
							return;
						}
						this.myGridTable.RowHeaderWidth += 15;
						return;
					}
					else
					{
						if (this.myGridTable.IsDefault)
						{
							this.RowHeaderWidth -= 15;
							return;
						}
						this.myGridTable.RowHeaderWidth -= 15;
					}
				}
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06001103 RID: 4355 RVA: 0x0003C598 File Offset: 0x0003A798
		private bool Bound
		{
			get
			{
				return this.listManager != null && this.myGridTable != null;
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06001104 RID: 4356 RVA: 0x0003C5AD File Offset: 0x0003A7AD
		internal DataGridCaption Caption
		{
			get
			{
				return this.caption;
			}
		}

		/// <summary>Gets or sets the background color of the caption area.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the caption's background color. The default is <see cref="P:System.Drawing.SystemColors.ActiveCaption" /> color.</returns>
		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06001105 RID: 4357 RVA: 0x0003C5B5 File Offset: 0x0003A7B5
		// (set) Token: 0x06001106 RID: 4358 RVA: 0x0003C5C2 File Offset: 0x0003A7C2
		[SRCategory("CatColors")]
		[SRDescription("DataGridCaptionBackColorDescr")]
		public Color CaptionBackColor
		{
			get
			{
				return this.Caption.BackColor;
			}
			set
			{
				if (DataGrid.IsTransparentColor(value))
				{
					throw new ArgumentException(SR.GetString("DataGridTransparentCaptionBackColorNotAllowed"));
				}
				this.Caption.BackColor = value;
			}
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x0003C5E8 File Offset: 0x0003A7E8
		private void ResetCaptionBackColor()
		{
			this.Caption.ResetBackColor();
		}

		/// <summary>Gets a value indicating whether the <see cref="P:System.Windows.Forms.DataGrid.CaptionBackColor" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has been changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001108 RID: 4360 RVA: 0x0003C5F5 File Offset: 0x0003A7F5
		protected virtual bool ShouldSerializeCaptionBackColor()
		{
			return this.Caption.ShouldSerializeBackColor();
		}

		/// <summary>Gets or sets the foreground color of the caption area.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the foreground color of the caption area. The default is <see cref="P:System.Drawing.SystemColors.ActiveCaptionText" />.</returns>
		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06001109 RID: 4361 RVA: 0x0003C602 File Offset: 0x0003A802
		// (set) Token: 0x0600110A RID: 4362 RVA: 0x0003C60F File Offset: 0x0003A80F
		[SRCategory("CatColors")]
		[SRDescription("DataGridCaptionForeColorDescr")]
		public Color CaptionForeColor
		{
			get
			{
				return this.Caption.ForeColor;
			}
			set
			{
				this.Caption.ForeColor = value;
			}
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x0003C61D File Offset: 0x0003A81D
		private void ResetCaptionForeColor()
		{
			this.Caption.ResetForeColor();
		}

		/// <summary>Gets a value indicating whether the <see cref="P:System.Windows.Forms.DataGrid.CaptionForeColor" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has been changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600110C RID: 4364 RVA: 0x0003C62A File Offset: 0x0003A82A
		protected virtual bool ShouldSerializeCaptionForeColor()
		{
			return this.Caption.ShouldSerializeForeColor();
		}

		/// <summary>Gets or sets the font of the grid's caption.</summary>
		/// <returns>A <see cref="T:System.Drawing.Font" /> that represents the caption's font.</returns>
		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x0600110D RID: 4365 RVA: 0x0003C637 File Offset: 0x0003A837
		// (set) Token: 0x0600110E RID: 4366 RVA: 0x0003C644 File Offset: 0x0003A844
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[AmbientValue(null)]
		[SRDescription("DataGridCaptionFontDescr")]
		public Font CaptionFont
		{
			get
			{
				return this.Caption.Font;
			}
			set
			{
				this.Caption.Font = value;
			}
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x0003C652 File Offset: 0x0003A852
		private bool ShouldSerializeCaptionFont()
		{
			return this.Caption.ShouldSerializeFont();
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x0003C65F File Offset: 0x0003A85F
		private void ResetCaptionFont()
		{
			this.Caption.ResetFont();
		}

		/// <summary>Gets or sets the text of the grid's window caption.</summary>
		/// <returns>A string to be displayed as the window caption of the grid. The default is an empty string ("").</returns>
		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x06001111 RID: 4369 RVA: 0x0003C66C File Offset: 0x0003A86C
		// (set) Token: 0x06001112 RID: 4370 RVA: 0x0003C679 File Offset: 0x0003A879
		[SRCategory("CatAppearance")]
		[DefaultValue("")]
		[Localizable(true)]
		[SRDescription("DataGridCaptionTextDescr")]
		public string CaptionText
		{
			get
			{
				return this.Caption.Text;
			}
			set
			{
				this.Caption.Text = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the grid's caption is visible.</summary>
		/// <returns>
		///     <see langword="true" /> if the caption is visible; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06001113 RID: 4371 RVA: 0x0003C687 File Offset: 0x0003A887
		// (set) Token: 0x06001114 RID: 4372 RVA: 0x0003C694 File Offset: 0x0003A894
		[DefaultValue(true)]
		[SRCategory("CatDisplay")]
		[SRDescription("DataGridCaptionVisibleDescr")]
		public bool CaptionVisible
		{
			get
			{
				return this.layout.CaptionVisible;
			}
			set
			{
				if (this.layout.CaptionVisible != value)
				{
					this.layout.CaptionVisible = value;
					base.PerformLayout();
					base.Invalidate();
					this.OnCaptionVisibleChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGrid.CaptionVisible" /> property has changed.</summary>
		// Token: 0x140000B3 RID: 179
		// (add) Token: 0x06001115 RID: 4373 RVA: 0x0003C6C7 File Offset: 0x0003A8C7
		// (remove) Token: 0x06001116 RID: 4374 RVA: 0x0003C6DA File Offset: 0x0003A8DA
		[SRCategory("CatPropertyChanged")]
		[SRDescription("DataGridOnCaptionVisibleChangedDescr")]
		public event EventHandler CaptionVisibleChanged
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_CAPTIONVISIBLECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_CAPTIONVISIBLECHANGED, value);
			}
		}

		/// <summary>Gets or sets which cell has the focus. Not available at design time.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridCell" /> with the focus.</returns>
		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06001117 RID: 4375 RVA: 0x0003C6ED File Offset: 0x0003A8ED
		// (set) Token: 0x06001118 RID: 4376 RVA: 0x0003C700 File Offset: 0x0003A900
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("DataGridCurrentCellDescr")]
		public DataGridCell CurrentCell
		{
			get
			{
				return new DataGridCell(this.currentRow, this.currentCol);
			}
			set
			{
				if (this.layout.dirty)
				{
					throw new ArgumentException(SR.GetString("DataGridSettingCurrentCellNotGood"));
				}
				if (value.RowNumber == this.currentRow && value.ColumnNumber == this.currentCol)
				{
					return;
				}
				if (this.DataGridRowsLength == 0 || this.myGridTable.GridColumnStyles == null || this.myGridTable.GridColumnStyles.Count == 0)
				{
					return;
				}
				this.EnsureBound();
				int num = this.currentRow;
				int num2 = this.currentCol;
				bool flag = this.gridState[32768];
				bool flag2 = false;
				bool flag3 = false;
				int num3 = value.ColumnNumber;
				int num4 = value.RowNumber;
				string text = null;
				try
				{
					int count = this.myGridTable.GridColumnStyles.Count;
					if (num3 < 0)
					{
						num3 = 0;
					}
					if (num3 >= count)
					{
						num3 = count - 1;
					}
					int num5 = this.DataGridRowsLength;
					DataGridRow[] array = this.DataGridRows;
					if (num4 < 0)
					{
						num4 = 0;
					}
					if (num4 >= num5)
					{
						num4 = num5 - 1;
					}
					if (this.currentCol != num3)
					{
						flag2 = true;
						int position = this.ListManager.Position;
						int count2 = this.ListManager.List.Count;
						this.EndEdit();
						if (this.ListManager.Position != position || count2 != this.ListManager.List.Count)
						{
							this.RecreateDataGridRows();
							if (this.ListManager.List.Count > 0)
							{
								this.currentRow = this.ListManager.Position;
								this.Edit();
							}
							else
							{
								this.currentRow = -1;
							}
							return;
						}
						this.currentCol = num3;
						this.InvalidateRow(this.currentRow);
					}
					if (this.currentRow != num4)
					{
						flag2 = true;
						int position2 = this.ListManager.Position;
						int count3 = this.ListManager.List.Count;
						this.EndEdit();
						if (this.ListManager.Position != position2 || count3 != this.ListManager.List.Count)
						{
							this.RecreateDataGridRows();
							if (this.ListManager.List.Count > 0)
							{
								this.currentRow = this.ListManager.Position;
								this.Edit();
							}
							else
							{
								this.currentRow = -1;
							}
							return;
						}
						if (this.currentRow < num5)
						{
							array[this.currentRow].OnRowLeave();
						}
						array[num4].OnRowEnter();
						this.currentRow = num4;
						if (num < num5)
						{
							this.InvalidateRow(num);
						}
						this.InvalidateRow(this.currentRow);
						if (num != this.listManager.Position)
						{
							flag3 = true;
							if (this.gridState[32768])
							{
								this.AbortEdit();
							}
						}
						else if (this.gridState[1048576])
						{
							this.ListManager.PositionChanged -= this.positionChangedHandler;
							this.ListManager.CancelCurrentEdit();
							this.ListManager.Position = this.currentRow;
							this.ListManager.PositionChanged += this.positionChangedHandler;
							array[this.DataGridRowsLength - 1] = new DataGridAddNewRow(this, this.myGridTable, this.DataGridRowsLength - 1);
							this.SetDataGridRows(array, this.DataGridRowsLength);
							this.gridState[1048576] = false;
						}
						else
						{
							this.ListManager.EndCurrentEdit();
							if (num5 != this.DataGridRowsLength)
							{
								this.currentRow = ((this.currentRow == num5 - 1) ? (this.DataGridRowsLength - 1) : this.currentRow);
							}
							if (this.currentRow == this.dataGridRowsLength - 1 && this.policy.AllowAdd)
							{
								this.AddNewRow();
							}
							else
							{
								this.ListManager.Position = this.currentRow;
							}
						}
					}
				}
				catch (Exception ex)
				{
					text = ex.Message;
				}
				if (text != null)
				{
					DialogResult dialogResult = RTLAwareMessageBox.Show(null, SR.GetString("DataGridPushedIncorrectValueIntoColumn", new object[]
					{
						text
					}), SR.GetString("DataGridErrorMessageBoxCaption"), MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
					if (dialogResult == DialogResult.Yes)
					{
						this.currentRow = num;
						this.currentCol = num2;
						this.InvalidateRowHeader(num4);
						this.InvalidateRowHeader(this.currentRow);
						if (flag)
						{
							this.Edit();
						}
					}
					else
					{
						if (this.currentRow == this.DataGridRowsLength - 1 && num == this.DataGridRowsLength - 2 && this.DataGridRows[this.currentRow] is DataGridAddNewRow)
						{
							num4 = num;
						}
						this.currentRow = num4;
						this.listManager.PositionChanged -= this.positionChangedHandler;
						this.listManager.CancelCurrentEdit();
						this.listManager.Position = num4;
						this.listManager.PositionChanged += this.positionChangedHandler;
						this.currentRow = num4;
						this.currentCol = num3;
						if (flag)
						{
							this.Edit();
						}
					}
				}
				if (flag2)
				{
					this.EnsureVisible(this.currentRow, this.currentCol);
					this.OnCurrentCellChanged(EventArgs.Empty);
					if (!flag3)
					{
						this.Edit();
					}
					base.AccessibilityNotifyClients(AccessibleEvents.Focus, this.CurrentCellAccIndex);
					base.AccessibilityNotifyClients(AccessibleEvents.Selection, this.CurrentCellAccIndex);
				}
			}
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06001119 RID: 4377 RVA: 0x0003CC18 File Offset: 0x0003AE18
		internal int CurrentCellAccIndex
		{
			get
			{
				int num = 0;
				num++;
				num += this.myGridTable.GridColumnStyles.Count;
				num += this.DataGridRows.Length;
				if (this.horizScrollBar.Visible)
				{
					num++;
				}
				if (this.vertScrollBar.Visible)
				{
					num++;
				}
				return num + (this.currentRow * this.myGridTable.GridColumnStyles.Count + this.currentCol);
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGrid.CurrentCell" /> property has changed.</summary>
		// Token: 0x140000B4 RID: 180
		// (add) Token: 0x0600111A RID: 4378 RVA: 0x0003CC8D File Offset: 0x0003AE8D
		// (remove) Token: 0x0600111B RID: 4379 RVA: 0x0003CCA0 File Offset: 0x0003AEA0
		[SRCategory("CatPropertyChanged")]
		[SRDescription("DataGridOnCurrentCellChangedDescr")]
		public event EventHandler CurrentCellChanged
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_CURRENTCELLCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_CURRENTCELLCHANGED, value);
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x0600111C RID: 4380 RVA: 0x0003CCB4 File Offset: 0x0003AEB4
		// (set) Token: 0x0600111D RID: 4381 RVA: 0x0003CCCF File Offset: 0x0003AECF
		private int CurrentColumn
		{
			get
			{
				return this.CurrentCell.ColumnNumber;
			}
			set
			{
				this.CurrentCell = new DataGridCell(this.currentRow, value);
			}
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x0600111E RID: 4382 RVA: 0x0003CCE4 File Offset: 0x0003AEE4
		// (set) Token: 0x0600111F RID: 4383 RVA: 0x0003CCFF File Offset: 0x0003AEFF
		private int CurrentRow
		{
			get
			{
				return this.CurrentCell.RowNumber;
			}
			set
			{
				this.CurrentCell = new DataGridCell(value, this.currentCol);
			}
		}

		/// <summary>Gets or sets the background color of selected rows.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background color of selected rows. The default is the <see cref="P:System.Drawing.SystemBrushes.ActiveCaption" /> color.</returns>
		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06001120 RID: 4384 RVA: 0x0003CD13 File Offset: 0x0003AF13
		// (set) Token: 0x06001121 RID: 4385 RVA: 0x0003CD20 File Offset: 0x0003AF20
		[SRCategory("CatColors")]
		[SRDescription("DataGridSelectionBackColorDescr")]
		public Color SelectionBackColor
		{
			get
			{
				return this.selectionBackBrush.Color;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[]
					{
						"SelectionBackColor"
					}));
				}
				if (DataGrid.IsTransparentColor(value))
				{
					throw new ArgumentException(SR.GetString("DataGridTransparentSelectionBackColorNotAllowed"));
				}
				if (!value.Equals(this.selectionBackBrush.Color))
				{
					this.selectionBackBrush = new SolidBrush(value);
					this.InvalidateInside();
				}
			}
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06001122 RID: 4386 RVA: 0x0003CD9D File Offset: 0x0003AF9D
		internal SolidBrush SelectionBackBrush
		{
			get
			{
				return this.selectionBackBrush;
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06001123 RID: 4387 RVA: 0x0003CDA5 File Offset: 0x0003AFA5
		internal SolidBrush SelectionForeBrush
		{
			get
			{
				return this.selectionForeBrush;
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Forms.DataGrid.SelectionBackColor" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001124 RID: 4388 RVA: 0x0003CDAD File Offset: 0x0003AFAD
		protected bool ShouldSerializeSelectionBackColor()
		{
			return !DataGrid.DefaultSelectionBackBrush.Equals(this.selectionBackBrush);
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.DataGrid.SelectionBackColor" /> property to its default value.</summary>
		// Token: 0x06001125 RID: 4389 RVA: 0x0003CDC2 File Offset: 0x0003AFC2
		public void ResetSelectionBackColor()
		{
			if (this.ShouldSerializeSelectionBackColor())
			{
				this.SelectionBackColor = DataGrid.DefaultSelectionBackBrush.Color;
			}
		}

		/// <summary>Gets or set the foreground color of selected rows.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing the foreground color of selected rows. The default is the <see cref="P:System.Drawing.SystemBrushes.ActiveCaptionText" /> color.</returns>
		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06001126 RID: 4390 RVA: 0x0003CDDC File Offset: 0x0003AFDC
		// (set) Token: 0x06001127 RID: 4391 RVA: 0x0003CDEC File Offset: 0x0003AFEC
		[SRCategory("CatColors")]
		[SRDescription("DataGridSelectionForeColorDescr")]
		public Color SelectionForeColor
		{
			get
			{
				return this.selectionForeBrush.Color;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[]
					{
						"SelectionForeColor"
					}));
				}
				if (!value.Equals(this.selectionForeBrush.Color))
				{
					this.selectionForeBrush = new SolidBrush(value);
					this.InvalidateInside();
				}
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Forms.DataGrid.SelectionForeColor" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001128 RID: 4392 RVA: 0x0003CE51 File Offset: 0x0003B051
		protected virtual bool ShouldSerializeSelectionForeColor()
		{
			return !this.SelectionForeBrush.Equals(DataGrid.DefaultSelectionForeBrush);
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.DataGrid.SelectionForeColor" /> property to its default value.</summary>
		// Token: 0x06001129 RID: 4393 RVA: 0x0003CE66 File Offset: 0x0003B066
		public void ResetSelectionForeColor()
		{
			if (this.ShouldSerializeSelectionForeColor())
			{
				this.SelectionForeColor = DataGrid.DefaultSelectionForeBrush.Color;
			}
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x0003CE80 File Offset: 0x0003B080
		internal override bool ShouldSerializeForeColor()
		{
			return !DataGrid.DefaultForeBrush.Color.Equals(this.ForeColor);
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x0003CEB4 File Offset: 0x0003B0B4
		internal override bool ShouldSerializeBackColor()
		{
			return !DataGrid.DefaultBackBrush.Color.Equals(this.BackColor);
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x0600112C RID: 4396 RVA: 0x0003CEE7 File Offset: 0x0003B0E7
		internal DataGridRow[] DataGridRows
		{
			get
			{
				if (this.dataGridRows == null)
				{
					this.CreateDataGridRows();
				}
				return this.dataGridRows;
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x0600112D RID: 4397 RVA: 0x0003CEFD File Offset: 0x0003B0FD
		internal DataGridToolTip ToolTipProvider
		{
			get
			{
				return this.toolTipProvider;
			}
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x0600112E RID: 4398 RVA: 0x0003CF05 File Offset: 0x0003B105
		// (set) Token: 0x0600112F RID: 4399 RVA: 0x0003CF0D File Offset: 0x0003B10D
		internal int ToolTipId
		{
			get
			{
				return this.toolTipId;
			}
			set
			{
				this.toolTipId = value;
			}
		}

		// Token: 0x06001130 RID: 4400 RVA: 0x0003CF18 File Offset: 0x0003B118
		private void ResetToolTip()
		{
			for (int i = 0; i < this.ToolTipId; i++)
			{
				this.ToolTipProvider.RemoveToolTip(new IntPtr(i));
			}
			if (!this.parentRows.IsEmpty())
			{
				bool alignRight = this.isRightToLeft();
				int detailsButtonWidth = this.Caption.GetDetailsButtonWidth();
				Rectangle backButtonRect = this.Caption.GetBackButtonRect(this.layout.Caption, alignRight, detailsButtonWidth);
				Rectangle detailsButtonRect = this.Caption.GetDetailsButtonRect(this.layout.Caption, alignRight);
				backButtonRect.X = this.MirrorRectangle(backButtonRect, this.layout.Inside, this.isRightToLeft());
				detailsButtonRect.X = this.MirrorRectangle(detailsButtonRect, this.layout.Inside, this.isRightToLeft());
				this.ToolTipProvider.AddToolTip(SR.GetString("DataGridCaptionBackButtonToolTip"), new IntPtr(0), backButtonRect);
				this.ToolTipProvider.AddToolTip(SR.GetString("DataGridCaptionDetailsButtonToolTip"), new IntPtr(1), detailsButtonRect);
				this.ToolTipId = 2;
				return;
			}
			this.ToolTipId = 0;
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x0003D024 File Offset: 0x0003B224
		private void CreateDataGridRows()
		{
			CurrencyManager currencyManager = this.ListManager;
			DataGridTableStyle dataGridTableStyle = this.myGridTable;
			this.InitializeColumnWidths();
			if (currencyManager == null)
			{
				this.SetDataGridRows(new DataGridRow[0], 0);
				return;
			}
			int num = currencyManager.Count;
			if (this.policy.AllowAdd)
			{
				num++;
			}
			DataGridRow[] array = new DataGridRow[num];
			for (int i = 0; i < currencyManager.Count; i++)
			{
				array[i] = new DataGridRelationshipRow(this, dataGridTableStyle, i);
			}
			if (this.policy.AllowAdd)
			{
				this.addNewRow = new DataGridAddNewRow(this, dataGridTableStyle, num - 1);
				array[num - 1] = this.addNewRow;
			}
			else
			{
				this.addNewRow = null;
			}
			this.SetDataGridRows(array, num);
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x0003D0D0 File Offset: 0x0003B2D0
		private void RecreateDataGridRows()
		{
			int num = 0;
			CurrencyManager currencyManager = this.ListManager;
			if (currencyManager != null)
			{
				num = currencyManager.Count;
				if (this.policy.AllowAdd)
				{
					num++;
				}
			}
			this.SetDataGridRows(null, num);
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x0003D10C File Offset: 0x0003B30C
		internal void SetDataGridRows(DataGridRow[] newRows, int newRowsLength)
		{
			this.dataGridRows = newRows;
			this.dataGridRowsLength = newRowsLength;
			this.vertScrollBar.Maximum = Math.Max(0, this.DataGridRowsLength - 1);
			if (this.firstVisibleRow > newRowsLength)
			{
				this.vertScrollBar.Value = 0;
				this.firstVisibleRow = 0;
			}
			this.ResetUIState();
		}

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06001134 RID: 4404 RVA: 0x0003D162 File Offset: 0x0003B362
		internal int DataGridRowsLength
		{
			get
			{
				return this.dataGridRowsLength;
			}
		}

		/// <summary>Gets or sets the data source that the grid is displaying data for.</summary>
		/// <returns>An object that functions as a data source.</returns>
		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06001135 RID: 4405 RVA: 0x0003D16A File Offset: 0x0003B36A
		// (set) Token: 0x06001136 RID: 4406 RVA: 0x0003D174 File Offset: 0x0003B374
		[DefaultValue(null)]
		[SRCategory("CatData")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[AttributeProvider(typeof(IListSource))]
		[SRDescription("DataGridDataSourceDescr")]
		public object DataSource
		{
			get
			{
				return this.dataSource;
			}
			set
			{
				if (value != null && !(value is IList) && !(value is IListSource))
				{
					throw new ArgumentException(SR.GetString("BadDataSourceForComplexBinding"));
				}
				if (this.dataSource != null && this.dataSource.Equals(value))
				{
					return;
				}
				if ((value == null || value == Convert.DBNull) && this.DataMember != null && this.DataMember.Length != 0)
				{
					this.dataSource = null;
					this.DataMember = "";
					return;
				}
				if (value != null)
				{
					this.EnforceValidDataMember(value);
				}
				this.ResetParentRows();
				this.Set_ListManager(value, this.DataMember, false);
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGrid.DataSource" /> property value has changed.</summary>
		// Token: 0x140000B5 RID: 181
		// (add) Token: 0x06001137 RID: 4407 RVA: 0x0003D20C File Offset: 0x0003B40C
		// (remove) Token: 0x06001138 RID: 4408 RVA: 0x0003D21F File Offset: 0x0003B41F
		[SRCategory("CatPropertyChanged")]
		[SRDescription("DataGridOnDataSourceChangedDescr")]
		public event EventHandler DataSourceChanged
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_DATASOURCECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_DATASOURCECHANGED, value);
			}
		}

		/// <summary>Gets or sets the specific list in a <see cref="P:System.Windows.Forms.DataGrid.DataSource" /> for which the <see cref="T:System.Windows.Forms.DataGrid" /> control displays a grid.</summary>
		/// <returns>A list in a <see cref="P:System.Windows.Forms.DataGrid.DataSource" />. The default is an empty string ("").</returns>
		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06001139 RID: 4409 RVA: 0x0003D232 File Offset: 0x0003B432
		// (set) Token: 0x0600113A RID: 4410 RVA: 0x0003D23A File Offset: 0x0003B43A
		[DefaultValue(null)]
		[SRCategory("CatData")]
		[Editor("System.Windows.Forms.Design.DataMemberListEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("DataGridDataMemberDescr")]
		public string DataMember
		{
			get
			{
				return this.dataMember;
			}
			set
			{
				if (this.dataMember != null && this.dataMember.Equals(value))
				{
					return;
				}
				this.ResetParentRows();
				this.Set_ListManager(this.DataSource, value, false);
			}
		}

		/// <summary>Sets the <see cref="P:System.Windows.Forms.DataGrid.DataSource" /> and <see cref="P:System.Windows.Forms.DataGrid.DataMember" /> properties at run time.</summary>
		/// <param name="dataSource">The data source for the <see cref="T:System.Windows.Forms.DataGrid" /> control. </param>
		/// <param name="dataMember">The <see cref="P:System.Windows.Forms.DataGrid.DataMember" /> string that specifies the table to bind to within the object returned by the <see cref="P:System.Windows.Forms.DataGrid.DataSource" /> property. </param>
		/// <exception cref="T:System.ArgumentException">One or more of the arguments are invalid. </exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="dataSource" /> argument is <see langword="null" />. </exception>
		// Token: 0x0600113B RID: 4411 RVA: 0x0003D268 File Offset: 0x0003B468
		public void SetDataBinding(object dataSource, string dataMember)
		{
			this.parentRows.Clear();
			this.originalState = null;
			this.caption.BackButtonActive = (this.caption.DownButtonActive = (this.caption.BackButtonVisible = false));
			this.caption.SetDownButtonDirection(!this.layout.ParentRowsVisible);
			this.Set_ListManager(dataSource, dataMember, false);
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.CurrencyManager" /> for this <see cref="T:System.Windows.Forms.DataGrid" /> control.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.CurrencyManager" /> for this <see cref="T:System.Windows.Forms.DataGrid" /> control.</returns>
		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x0600113C RID: 4412 RVA: 0x0003D2D1 File Offset: 0x0003B4D1
		// (set) Token: 0x0600113D RID: 4413 RVA: 0x0003D30E File Offset: 0x0003B50E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRDescription("DataGridListManagerDescr")]
		protected internal CurrencyManager ListManager
		{
			get
			{
				if (this.listManager == null && this.BindingContext != null && this.DataSource != null)
				{
					return (CurrencyManager)this.BindingContext[this.DataSource, this.DataMember];
				}
				return this.listManager;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("DataGridSetListManager"));
			}
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x0003D31F File Offset: 0x0003B51F
		internal void Set_ListManager(object newDataSource, string newDataMember, bool force)
		{
			this.Set_ListManager(newDataSource, newDataMember, force, true);
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x0003D32C File Offset: 0x0003B52C
		internal void Set_ListManager(object newDataSource, string newDataMember, bool force, bool forceColumnCreation)
		{
			bool flag = this.DataSource != newDataSource;
			bool flag2 = this.DataMember != newDataMember;
			if (!force && !flag && !flag2 && this.gridState[2097152])
			{
				return;
			}
			this.gridState[2097152] = true;
			if (this.toBeDisposedEditingControl != null)
			{
				base.Controls.Remove(this.toBeDisposedEditingControl);
				this.toBeDisposedEditingControl = null;
			}
			bool flag3 = true;
			try
			{
				this.UpdateListManager();
				if (this.listManager != null)
				{
					this.UnWireDataSource();
				}
				CurrencyManager currencyManager = this.listManager;
				if (newDataSource != null && this.BindingContext != null && newDataSource != Convert.DBNull)
				{
					this.listManager = (CurrencyManager)this.BindingContext[newDataSource, newDataMember];
				}
				else
				{
					this.listManager = null;
				}
				this.dataSource = newDataSource;
				this.dataMember = ((newDataMember == null) ? "" : newDataMember);
				bool flag4 = this.listManager != currencyManager;
				if (this.listManager != null)
				{
					this.WireDataSource();
					this.policy.UpdatePolicy(this.listManager, this.ReadOnly);
				}
				if (!this.Initializing && this.listManager == null)
				{
					if (base.ContainsFocus && this.ParentInternal == null)
					{
						for (int i = 0; i < base.Controls.Count; i++)
						{
							if (base.Controls[i].Focused)
							{
								this.toBeDisposedEditingControl = base.Controls[i];
								break;
							}
						}
						if (this.toBeDisposedEditingControl == this.horizScrollBar || this.toBeDisposedEditingControl == this.vertScrollBar)
						{
							this.toBeDisposedEditingControl = null;
						}
					}
					this.SetDataGridRows(null, 0);
					this.defaultTableStyle.GridColumnStyles.Clear();
					this.SetDataGridTable(this.defaultTableStyle, forceColumnCreation);
					if (this.toBeDisposedEditingControl != null)
					{
						base.Controls.Add(this.toBeDisposedEditingControl);
					}
				}
				if (flag4 || this.gridState[4194304])
				{
					if (base.Visible)
					{
						base.BeginUpdateInternal();
					}
					if (this.listManager != null)
					{
						this.defaultTableStyle.GridColumnStyles.ResetDefaultColumnCollection();
						DataGridTableStyle dataGridTableStyle = this.dataGridTables[this.listManager.GetListName()];
						if (dataGridTableStyle == null)
						{
							this.SetDataGridTable(this.defaultTableStyle, forceColumnCreation);
						}
						else
						{
							this.SetDataGridTable(dataGridTableStyle, forceColumnCreation);
						}
						this.currentRow = ((this.listManager.Position == -1) ? 0 : this.listManager.Position);
					}
					this.RecreateDataGridRows();
					if (base.Visible)
					{
						base.EndUpdateInternal();
					}
					flag3 = false;
					this.ComputeMinimumRowHeaderWidth();
					if (this.myGridTable.IsDefault)
					{
						this.RowHeaderWidth = Math.Max(this.minRowHeaderWidth, this.RowHeaderWidth);
					}
					else
					{
						this.myGridTable.RowHeaderWidth = Math.Max(this.minRowHeaderWidth, this.RowHeaderWidth);
					}
					this.ListHasErrors = this.DataGridSourceHasErrors();
					this.ResetUIState();
					this.OnDataSourceChanged(EventArgs.Empty);
				}
			}
			finally
			{
				this.gridState[2097152] = false;
				if (flag3 && base.Visible)
				{
					base.EndUpdateInternal();
				}
			}
		}

		/// <summary>Gets or sets index of the row that currently has focus.</summary>
		/// <returns>The zero-based index of the current row.</returns>
		/// <exception cref="T:System.Exception">There is no <see cref="T:System.Windows.Forms.CurrencyManager" />. </exception>
		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06001140 RID: 4416 RVA: 0x0003D660 File Offset: 0x0003B860
		// (set) Token: 0x06001141 RID: 4417 RVA: 0x0003D6C4 File Offset: 0x0003B8C4
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[SRDescription("DataGridSelectedIndexDescr")]
		public int CurrentRowIndex
		{
			get
			{
				if (this.originalState == null)
				{
					if (this.listManager != null)
					{
						return this.listManager.Position;
					}
					return -1;
				}
				else
				{
					if (this.BindingContext == null)
					{
						return -1;
					}
					CurrencyManager currencyManager = (CurrencyManager)this.BindingContext[this.originalState.DataSource, this.originalState.DataMember];
					return currencyManager.Position;
				}
			}
			set
			{
				if (this.listManager == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridSetSelectIndex"));
				}
				if (this.originalState == null)
				{
					this.listManager.Position = value;
					this.currentRow = value;
					return;
				}
				CurrencyManager currencyManager = (CurrencyManager)this.BindingContext[this.originalState.DataSource, this.originalState.DataMember];
				currencyManager.Position = value;
				this.originalState.LinkingRow = this.originalState.DataGridRows[value];
				base.Invalidate();
			}
		}

		/// <summary>Gets the collection of <see cref="T:System.Windows.Forms.DataGridTableStyle" /> objects for the grid.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.GridTableStylesCollection" /> that represents the collection of <see cref="T:System.Windows.Forms.DataGridTableStyle" /> objects.</returns>
		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06001142 RID: 4418 RVA: 0x0003D751 File Offset: 0x0003B951
		[SRCategory("CatData")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(true)]
		[SRDescription("DataGridGridTablesDescr")]
		public GridTableStylesCollection TableStyles
		{
			get
			{
				return this.dataGridTables;
			}
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06001143 RID: 4419 RVA: 0x0003D759 File Offset: 0x0003B959
		internal new int FontHeight
		{
			get
			{
				return this.fontHeight;
			}
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06001144 RID: 4420 RVA: 0x0003D761 File Offset: 0x0003B961
		internal AccessibleObject ParentRowsAccessibleObject
		{
			get
			{
				return this.parentRows.AccessibleObject;
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06001145 RID: 4421 RVA: 0x0003D76E File Offset: 0x0003B96E
		internal Rectangle ParentRowsBounds
		{
			get
			{
				return this.layout.ParentRows;
			}
		}

		/// <summary>Gets or sets the color of the grid lines.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the color of the grid lines. The default is the system color for controls (<see cref="P:System.Drawing.SystemColors.Control" />).</returns>
		/// <exception cref="T:System.ArgumentException">The value is not set. </exception>
		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06001146 RID: 4422 RVA: 0x0003D77B File Offset: 0x0003B97B
		// (set) Token: 0x06001147 RID: 4423 RVA: 0x0003D788 File Offset: 0x0003B988
		[SRCategory("CatColors")]
		[SRDescription("DataGridGridLineColorDescr")]
		public Color GridLineColor
		{
			get
			{
				return this.gridLineBrush.Color;
			}
			set
			{
				if (this.gridLineBrush.Color != value)
				{
					if (value.IsEmpty)
					{
						throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[]
						{
							"GridLineColor"
						}));
					}
					this.gridLineBrush = new SolidBrush(value);
					base.Invalidate(this.layout.Data);
				}
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Forms.DataGrid.GridLineColor" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001148 RID: 4424 RVA: 0x0003D7EC File Offset: 0x0003B9EC
		protected virtual bool ShouldSerializeGridLineColor()
		{
			return !this.GridLineBrush.Equals(DataGrid.DefaultGridLineBrush);
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.DataGrid.GridLineColor" /> property to its default value.</summary>
		// Token: 0x06001149 RID: 4425 RVA: 0x0003D801 File Offset: 0x0003BA01
		public void ResetGridLineColor()
		{
			if (this.ShouldSerializeGridLineColor())
			{
				this.GridLineColor = DataGrid.DefaultGridLineBrush.Color;
			}
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x0600114A RID: 4426 RVA: 0x0003D81B File Offset: 0x0003BA1B
		internal SolidBrush GridLineBrush
		{
			get
			{
				return this.gridLineBrush;
			}
		}

		/// <summary>Gets or sets the line style of the grid.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DataGridLineStyle" /> values. The default is <see langword="Solid" />.</returns>
		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x0600114B RID: 4427 RVA: 0x0003D823 File Offset: 0x0003BA23
		// (set) Token: 0x0600114C RID: 4428 RVA: 0x0003D82C File Offset: 0x0003BA2C
		[SRCategory("CatAppearance")]
		[DefaultValue(DataGridLineStyle.Solid)]
		[SRDescription("DataGridGridLineStyleDescr")]
		public DataGridLineStyle GridLineStyle
		{
			get
			{
				return this.gridLineStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridLineStyle));
				}
				if (this.gridLineStyle != value)
				{
					this.gridLineStyle = value;
					this.myGridTable.ResetRelationsUI();
					base.Invalidate(this.layout.Data);
				}
			}
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x0600114D RID: 4429 RVA: 0x0003D88B File Offset: 0x0003BA8B
		internal int GridLineWidth
		{
			get
			{
				if (this.GridLineStyle != DataGridLineStyle.Solid)
				{
					return 0;
				}
				return 1;
			}
		}

		/// <summary>Gets or sets the way parent row labels are displayed.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DataGridParentRowsLabelStyle" /> values. The default is <see langword="Both" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The enumerator was not valid. </exception>
		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x0600114E RID: 4430 RVA: 0x0003D899 File Offset: 0x0003BA99
		// (set) Token: 0x0600114F RID: 4431 RVA: 0x0003D8A4 File Offset: 0x0003BAA4
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue(DataGridParentRowsLabelStyle.Both)]
		[SRCategory("CatDisplay")]
		[SRDescription("DataGridParentRowsLabelStyleDescr")]
		public DataGridParentRowsLabelStyle ParentRowsLabelStyle
		{
			get
			{
				return this.parentRowsLabels;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridParentRowsLabelStyle));
				}
				if (this.parentRowsLabels != value)
				{
					this.parentRowsLabels = value;
					base.Invalidate(this.layout.ParentRows);
					this.OnParentRowsLabelStyleChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the label style of the parent row is changed.</summary>
		// Token: 0x140000B6 RID: 182
		// (add) Token: 0x06001150 RID: 4432 RVA: 0x0003D903 File Offset: 0x0003BB03
		// (remove) Token: 0x06001151 RID: 4433 RVA: 0x0003D916 File Offset: 0x0003BB16
		[SRCategory("CatPropertyChanged")]
		[SRDescription("DataGridOnParentRowsLabelStyleChangedDescr")]
		public event EventHandler ParentRowsLabelStyleChanged
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_PARENTROWSLABELSTYLECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_PARENTROWSLABELSTYLECHANGED, value);
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06001152 RID: 4434 RVA: 0x0003D929 File Offset: 0x0003BB29
		internal bool Initializing
		{
			get
			{
				return this.inInit;
			}
		}

		/// <summary>Gets the index of the first visible column in a grid.</summary>
		/// <returns>The index of a <see cref="T:System.Windows.Forms.DataGridColumnStyle" />.</returns>
		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06001153 RID: 4435 RVA: 0x0003D931 File Offset: 0x0003BB31
		[Browsable(false)]
		[SRDescription("DataGridFirstVisibleColumnDescr")]
		public int FirstVisibleColumn
		{
			get
			{
				return this.firstVisibleCol;
			}
		}

		/// <summary>Gets or sets a value indicating whether the grid displays in flat mode.</summary>
		/// <returns>
		///     <see langword="true" /> if the grid is displayed flat; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06001154 RID: 4436 RVA: 0x0003D939 File Offset: 0x0003BB39
		// (set) Token: 0x06001155 RID: 4437 RVA: 0x0003D948 File Offset: 0x0003BB48
		[DefaultValue(false)]
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridFlatModeDescr")]
		public bool FlatMode
		{
			get
			{
				return this.gridState[64];
			}
			set
			{
				if (value != this.FlatMode)
				{
					this.gridState[64] = value;
					base.Invalidate(this.layout.Inside);
					this.OnFlatModeChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGrid.FlatMode" /> has changed.</summary>
		// Token: 0x140000B7 RID: 183
		// (add) Token: 0x06001156 RID: 4438 RVA: 0x0003D97D File Offset: 0x0003BB7D
		// (remove) Token: 0x06001157 RID: 4439 RVA: 0x0003D990 File Offset: 0x0003BB90
		[SRCategory("CatPropertyChanged")]
		[SRDescription("DataGridOnFlatModeChangedDescr")]
		public event EventHandler FlatModeChanged
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_FLATMODECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_FLATMODECHANGED, value);
			}
		}

		/// <summary>Gets or sets the background color of all row and column headers.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background color of row and column headers. The default is the system color for controls, <see cref="P:System.Drawing.SystemColors.Control" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">While trying to set the property, a <see langword="Color.Empty" /> was passed. </exception>
		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06001158 RID: 4440 RVA: 0x0003D9A3 File Offset: 0x0003BBA3
		// (set) Token: 0x06001159 RID: 4441 RVA: 0x0003D9B0 File Offset: 0x0003BBB0
		[SRCategory("CatColors")]
		[SRDescription("DataGridHeaderBackColorDescr")]
		public Color HeaderBackColor
		{
			get
			{
				return this.headerBackBrush.Color;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[]
					{
						"HeaderBackColor"
					}));
				}
				if (DataGrid.IsTransparentColor(value))
				{
					throw new ArgumentException(SR.GetString("DataGridTransparentHeaderBackColorNotAllowed"));
				}
				if (!value.Equals(this.headerBackBrush.Color))
				{
					this.headerBackBrush = new SolidBrush(value);
					if (this.layout.RowHeadersVisible)
					{
						base.Invalidate(this.layout.RowHeaders);
					}
					if (this.layout.ColumnHeadersVisible)
					{
						base.Invalidate(this.layout.ColumnHeaders);
					}
					base.Invalidate(this.layout.TopLeftHeader);
				}
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x0600115A RID: 4442 RVA: 0x0003DA74 File Offset: 0x0003BC74
		internal SolidBrush HeaderBackBrush
		{
			get
			{
				return this.headerBackBrush;
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Forms.DataGrid.HeaderBackColor" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600115B RID: 4443 RVA: 0x0003DA7C File Offset: 0x0003BC7C
		protected virtual bool ShouldSerializeHeaderBackColor()
		{
			return !this.HeaderBackBrush.Equals(DataGrid.DefaultHeaderBackBrush);
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.DataGrid.HeaderBackColor" /> property to its default value.</summary>
		// Token: 0x0600115C RID: 4444 RVA: 0x0003DA91 File Offset: 0x0003BC91
		public void ResetHeaderBackColor()
		{
			if (this.ShouldSerializeHeaderBackColor())
			{
				this.HeaderBackColor = DataGrid.DefaultHeaderBackBrush.Color;
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x0600115D RID: 4445 RVA: 0x0003DAAB File Offset: 0x0003BCAB
		internal SolidBrush BackgroundBrush
		{
			get
			{
				return this.backgroundBrush;
			}
		}

		// Token: 0x0600115E RID: 4446 RVA: 0x0003DAB3 File Offset: 0x0003BCB3
		private void ResetBackgroundColor()
		{
			if (this.backgroundBrush != null && this.BackgroundBrush != DataGrid.DefaultBackgroundBrush)
			{
				this.backgroundBrush.Dispose();
				this.backgroundBrush = null;
			}
			this.backgroundBrush = DataGrid.DefaultBackgroundBrush;
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Forms.DataGrid.BackgroundColor" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600115F RID: 4447 RVA: 0x0003DAE7 File Offset: 0x0003BCE7
		protected virtual bool ShouldSerializeBackgroundColor()
		{
			return !this.BackgroundBrush.Equals(DataGrid.DefaultBackgroundBrush);
		}

		/// <summary>Gets or sets the color of the non-row area of the grid.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the color of the grid's background. The default is the <see cref="P:System.Drawing.SystemColors.AppWorkspace" /> color.</returns>
		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06001160 RID: 4448 RVA: 0x0003DAFC File Offset: 0x0003BCFC
		// (set) Token: 0x06001161 RID: 4449 RVA: 0x0003DB0C File Offset: 0x0003BD0C
		[SRCategory("CatColors")]
		[SRDescription("DataGridBackgroundColorDescr")]
		public Color BackgroundColor
		{
			get
			{
				return this.backgroundBrush.Color;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[]
					{
						"BackgroundColor"
					}));
				}
				if (!value.Equals(this.backgroundBrush.Color))
				{
					if (this.backgroundBrush != null && this.BackgroundBrush != DataGrid.DefaultBackgroundBrush)
					{
						this.backgroundBrush.Dispose();
						this.backgroundBrush = null;
					}
					this.backgroundBrush = new SolidBrush(value);
					base.Invalidate(this.layout.Inside);
					this.OnBackgroundColorChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGrid.BackgroundColor" /> has changed.</summary>
		// Token: 0x140000B8 RID: 184
		// (add) Token: 0x06001162 RID: 4450 RVA: 0x0003DBAE File Offset: 0x0003BDAE
		// (remove) Token: 0x06001163 RID: 4451 RVA: 0x0003DBC1 File Offset: 0x0003BDC1
		[SRCategory("CatPropertyChanged")]
		[SRDescription("DataGridOnBackgroundColorChangedDescr")]
		public event EventHandler BackgroundColorChanged
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_BACKGROUNDCOLORCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_BACKGROUNDCOLORCHANGED, value);
			}
		}

		/// <summary>Gets or sets the font used for column headers.</summary>
		/// <returns>The <see cref="T:System.Drawing.Font" /> that represents the header text.</returns>
		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06001164 RID: 4452 RVA: 0x0003DBD4 File Offset: 0x0003BDD4
		// (set) Token: 0x06001165 RID: 4453 RVA: 0x0003DBEC File Offset: 0x0003BDEC
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridHeaderFontDescr")]
		public Font HeaderFont
		{
			get
			{
				if (this.headerFont != null)
				{
					return this.headerFont;
				}
				return this.Font;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("HeaderFont");
				}
				if (!value.Equals(this.headerFont))
				{
					this.headerFont = value;
					this.RecalculateFonts();
					base.PerformLayout();
					base.Invalidate(this.layout.Inside);
				}
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Forms.DataGrid.HeaderFont" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001166 RID: 4454 RVA: 0x0003DC39 File Offset: 0x0003BE39
		protected bool ShouldSerializeHeaderFont()
		{
			return this.headerFont != null;
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.DataGrid.HeaderFont" /> property to its default value.</summary>
		// Token: 0x06001167 RID: 4455 RVA: 0x0003DC44 File Offset: 0x0003BE44
		public void ResetHeaderFont()
		{
			if (this.headerFont != null)
			{
				this.headerFont = null;
				this.RecalculateFonts();
				base.PerformLayout();
				base.Invalidate(this.layout.Inside);
			}
		}

		/// <summary>Gets or sets the foreground color of headers.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the foreground color of the grid's column headers, including the column header text and the plus/minus glyphs. The default is <see cref="P:System.Drawing.SystemColors.ControlText" /> color.</returns>
		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06001168 RID: 4456 RVA: 0x0003DC72 File Offset: 0x0003BE72
		// (set) Token: 0x06001169 RID: 4457 RVA: 0x0003DC80 File Offset: 0x0003BE80
		[SRCategory("CatColors")]
		[SRDescription("DataGridHeaderForeColorDescr")]
		public Color HeaderForeColor
		{
			get
			{
				return this.headerForePen.Color;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[]
					{
						"HeaderForeColor"
					}));
				}
				if (!value.Equals(this.headerForePen.Color))
				{
					this.headerForePen = new Pen(value);
					this.headerForeBrush = new SolidBrush(value);
					if (this.layout.RowHeadersVisible)
					{
						base.Invalidate(this.layout.RowHeaders);
					}
					if (this.layout.ColumnHeadersVisible)
					{
						base.Invalidate(this.layout.ColumnHeaders);
					}
					base.Invalidate(this.layout.TopLeftHeader);
				}
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Forms.DataGrid.HeaderForeColor" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600116A RID: 4458 RVA: 0x0003DD38 File Offset: 0x0003BF38
		protected virtual bool ShouldSerializeHeaderForeColor()
		{
			return !this.HeaderForePen.Equals(DataGrid.DefaultHeaderForePen);
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.DataGrid.HeaderForeColor" /> property to its default value.</summary>
		// Token: 0x0600116B RID: 4459 RVA: 0x0003DD4D File Offset: 0x0003BF4D
		public void ResetHeaderForeColor()
		{
			if (this.ShouldSerializeHeaderForeColor())
			{
				this.HeaderForeColor = DataGrid.DefaultHeaderForeBrush.Color;
			}
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x0600116C RID: 4460 RVA: 0x0003DD67 File Offset: 0x0003BF67
		internal SolidBrush HeaderForeBrush
		{
			get
			{
				return this.headerForeBrush;
			}
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x0600116D RID: 4461 RVA: 0x0003DD6F File Offset: 0x0003BF6F
		internal Pen HeaderForePen
		{
			get
			{
				return this.headerForePen;
			}
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x0003DD77 File Offset: 0x0003BF77
		private void ResetHorizontalOffset()
		{
			this.horizontalOffset = 0;
			this.negOffset = 0;
			this.firstVisibleCol = 0;
			this.numVisibleCols = 0;
			this.lastTotallyVisibleCol = -1;
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x0600116F RID: 4463 RVA: 0x0003DD9C File Offset: 0x0003BF9C
		// (set) Token: 0x06001170 RID: 4464 RVA: 0x0003DDA4 File Offset: 0x0003BFA4
		internal int HorizontalOffset
		{
			get
			{
				return this.horizontalOffset;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				int columnWidthSum = this.GetColumnWidthSum();
				int num = columnWidthSum - this.layout.Data.Width;
				if (value > num && num > 0)
				{
					value = num;
				}
				if (value == this.horizontalOffset)
				{
					return;
				}
				int change = this.horizontalOffset - value;
				this.horizScrollBar.Value = value;
				Rectangle rectangle = this.layout.Data;
				if (this.layout.ColumnHeadersVisible)
				{
					rectangle = Rectangle.Union(rectangle, this.layout.ColumnHeaders);
				}
				this.horizontalOffset = value;
				this.firstVisibleCol = this.ComputeFirstVisibleColumn();
				this.ComputeVisibleColumns();
				if (this.gridState[131072])
				{
					if (this.currentCol >= this.firstVisibleCol && this.currentCol < this.firstVisibleCol + this.numVisibleCols - 1 && (this.gridState[32768] || this.gridState[16384]))
					{
						this.Edit();
					}
					else
					{
						this.EndEdit();
					}
					this.gridState[131072] = false;
				}
				else
				{
					this.EndEdit();
				}
				NativeMethods.RECT[] rects = this.CreateScrollableRegion(rectangle);
				this.ScrollRectangles(rects, change);
				this.OnScroll(EventArgs.Empty);
			}
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x0003DEE0 File Offset: 0x0003C0E0
		private void ScrollRectangles(NativeMethods.RECT[] rects, int change)
		{
			if (rects != null)
			{
				if (this.isRightToLeft())
				{
					change = -change;
				}
				foreach (NativeMethods.RECT rect in rects)
				{
					SafeNativeMethods.ScrollWindow(new HandleRef(this, base.Handle), change, 0, ref rect, ref rect);
				}
			}
		}

		/// <summary>Gets the horizontal scroll bar for the grid.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ScrollBar" /> for the grid.</returns>
		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06001172 RID: 4466 RVA: 0x0003DF2A File Offset: 0x0003C12A
		[SRDescription("DataGridHorizScrollBarDescr")]
		protected ScrollBar HorizScrollBar
		{
			get
			{
				return this.horizScrollBar;
			}
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06001173 RID: 4467 RVA: 0x0003DF32 File Offset: 0x0003C132
		internal bool LedgerStyle
		{
			get
			{
				return this.gridState[32];
			}
		}

		/// <summary>Gets or sets the color of the text that you can click to navigate to a child table.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the color of text that is clicked to navigate to a child table. The default is <see cref="P:System.Drawing.SystemColors.HotTrack" />.</returns>
		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06001174 RID: 4468 RVA: 0x0003DF41 File Offset: 0x0003C141
		// (set) Token: 0x06001175 RID: 4469 RVA: 0x0003DF50 File Offset: 0x0003C150
		[SRCategory("CatColors")]
		[SRDescription("DataGridLinkColorDescr")]
		public Color LinkColor
		{
			get
			{
				return this.linkBrush.Color;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[]
					{
						"LinkColor"
					}));
				}
				if (!this.linkBrush.Color.Equals(value))
				{
					this.linkBrush = new SolidBrush(value);
					base.Invalidate(this.layout.Data);
				}
			}
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x0003DFC2 File Offset: 0x0003C1C2
		internal virtual bool ShouldSerializeLinkColor()
		{
			return !this.LinkBrush.Equals(DataGrid.DefaultLinkBrush);
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.DataGrid.LinkColor" /> property to its default value.</summary>
		// Token: 0x06001177 RID: 4471 RVA: 0x0003DFD7 File Offset: 0x0003C1D7
		public void ResetLinkColor()
		{
			if (this.ShouldSerializeLinkColor())
			{
				this.LinkColor = DataGrid.DefaultLinkBrush.Color;
			}
		}

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06001178 RID: 4472 RVA: 0x0003DFF1 File Offset: 0x0003C1F1
		internal Brush LinkBrush
		{
			get
			{
				return this.linkBrush;
			}
		}

		/// <summary>This member is not meaningful for this control.</summary>
		/// <returns>The color displayed when hovering over link text.</returns>
		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06001179 RID: 4473 RVA: 0x0003DFF9 File Offset: 0x0003C1F9
		// (set) Token: 0x0600117A RID: 4474 RVA: 0x0000701A File Offset: 0x0000521A
		[SRDescription("DataGridLinkHoverColorDescr")]
		[SRCategory("CatColors")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public Color LinkHoverColor
		{
			get
			{
				return this.LinkColor;
			}
			set
			{
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Forms.DataGrid.LinkHoverColor" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600117B RID: 4475 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected virtual bool ShouldSerializeLinkHoverColor()
		{
			return false;
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.DataGrid.LinkHoverColor" /> property to its default value.</summary>
		// Token: 0x0600117C RID: 4476 RVA: 0x0000701A File Offset: 0x0000521A
		public void ResetLinkHoverColor()
		{
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x0600117D RID: 4477 RVA: 0x0003E001 File Offset: 0x0003C201
		internal Font LinkFont
		{
			get
			{
				return this.linkFont;
			}
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x0600117E RID: 4478 RVA: 0x0003E009 File Offset: 0x0003C209
		internal int LinkFontHeight
		{
			get
			{
				return this.linkFontHeight;
			}
		}

		/// <summary>Gets or sets a value indicating whether navigation is allowed.</summary>
		/// <returns>
		///     <see langword="true" /> if navigation is allowed; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x0600117F RID: 4479 RVA: 0x0003E011 File Offset: 0x0003C211
		// (set) Token: 0x06001180 RID: 4480 RVA: 0x0003E024 File Offset: 0x0003C224
		[DefaultValue(true)]
		[SRDescription("DataGridNavigationModeDescr")]
		[SRCategory("CatBehavior")]
		public bool AllowNavigation
		{
			get
			{
				return this.gridState[8192];
			}
			set
			{
				if (this.AllowNavigation != value)
				{
					this.gridState[8192] = value;
					this.Caption.BackButtonActive = (!this.parentRows.IsEmpty() && value);
					this.Caption.BackButtonVisible = this.Caption.BackButtonActive;
					this.RecreateDataGridRows();
					this.OnAllowNavigationChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGrid.AllowNavigation" /> property has changed.</summary>
		// Token: 0x140000B9 RID: 185
		// (add) Token: 0x06001181 RID: 4481 RVA: 0x0003E08D File Offset: 0x0003C28D
		// (remove) Token: 0x06001182 RID: 4482 RVA: 0x0003E0A0 File Offset: 0x0003C2A0
		[SRCategory("CatPropertyChanged")]
		[SRDescription("DataGridOnNavigationModeChangedDescr")]
		public event EventHandler AllowNavigationChanged
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_ALLOWNAVIGATIONCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_ALLOWNAVIGATIONCHANGED, value);
			}
		}

		/// <summary>This member is not meaningful for this control.</summary>
		/// <returns>The type of cursor to display as the mouse pointer moves over the object.</returns>
		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001183 RID: 4483 RVA: 0x00012033 File Offset: 0x00010233
		// (set) Token: 0x06001184 RID: 4484 RVA: 0x0001203B File Offset: 0x0001023B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Cursor Cursor
		{
			get
			{
				return base.Cursor;
			}
			set
			{
				base.Cursor = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.DataGrid.Cursor" /> property changes.</summary>
		// Token: 0x140000BA RID: 186
		// (add) Token: 0x06001185 RID: 4485 RVA: 0x0003E0B3 File Offset: 0x0003C2B3
		// (remove) Token: 0x06001186 RID: 4486 RVA: 0x0003E0BC File Offset: 0x0003C2BC
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler CursorChanged
		{
			add
			{
				base.CursorChanged += value;
			}
			remove
			{
				base.CursorChanged -= value;
			}
		}

		/// <summary>This member is not meaningful for this control.</summary>
		/// <returns>The background image associated with the control.</returns>
		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06001187 RID: 4487 RVA: 0x00011FC2 File Offset: 0x000101C2
		// (set) Token: 0x06001188 RID: 4488 RVA: 0x00011FCA File Offset: 0x000101CA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		/// <summary>This member is not meaningful for this control.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.ImageLayout" /> value.</returns>
		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001189 RID: 4489 RVA: 0x00011FD3 File Offset: 0x000101D3
		// (set) Token: 0x0600118A RID: 4490 RVA: 0x00011FDB File Offset: 0x000101DB
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
			set
			{
				base.BackgroundImageLayout = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.DataGrid.BackgroundImage" /> property changes.</summary>
		// Token: 0x140000BB RID: 187
		// (add) Token: 0x0600118B RID: 4491 RVA: 0x0001FD81 File Offset: 0x0001DF81
		// (remove) Token: 0x0600118C RID: 4492 RVA: 0x0001FD8A File Offset: 0x0001DF8A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageChanged
		{
			add
			{
				base.BackgroundImageChanged += value;
			}
			remove
			{
				base.BackgroundImageChanged -= value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.DataGrid.BackgroundImageLayout" /> property changes.</summary>
		// Token: 0x140000BC RID: 188
		// (add) Token: 0x0600118D RID: 4493 RVA: 0x0001FD93 File Offset: 0x0001DF93
		// (remove) Token: 0x0600118E RID: 4494 RVA: 0x0001FD9C File Offset: 0x0001DF9C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageLayoutChanged
		{
			add
			{
				base.BackgroundImageLayoutChanged += value;
			}
			remove
			{
				base.BackgroundImageLayoutChanged -= value;
			}
		}

		/// <summary>Gets or sets the background color of parent rows.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the color of parent rows. The default is the <see cref="P:System.Drawing.SystemColors.Control" /> color.</returns>
		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x0600118F RID: 4495 RVA: 0x0003E0C5 File Offset: 0x0003C2C5
		// (set) Token: 0x06001190 RID: 4496 RVA: 0x0003E0D2 File Offset: 0x0003C2D2
		[SRCategory("CatColors")]
		[SRDescription("DataGridParentRowsBackColorDescr")]
		public Color ParentRowsBackColor
		{
			get
			{
				return this.parentRows.BackColor;
			}
			set
			{
				if (DataGrid.IsTransparentColor(value))
				{
					throw new ArgumentException(SR.GetString("DataGridTransparentParentRowsBackColorNotAllowed"));
				}
				this.parentRows.BackColor = value;
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06001191 RID: 4497 RVA: 0x0003E0F8 File Offset: 0x0003C2F8
		internal SolidBrush ParentRowsBackBrush
		{
			get
			{
				return this.parentRows.BackBrush;
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Forms.DataGrid.ParentRowsBackColor" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has been changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001192 RID: 4498 RVA: 0x0003E105 File Offset: 0x0003C305
		protected virtual bool ShouldSerializeParentRowsBackColor()
		{
			return !this.ParentRowsBackBrush.Equals(DataGrid.DefaultParentRowsBackBrush);
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x0003E11A File Offset: 0x0003C31A
		private void ResetParentRowsBackColor()
		{
			if (this.ShouldSerializeParentRowsBackColor())
			{
				this.parentRows.BackBrush = DataGrid.DefaultParentRowsBackBrush;
			}
		}

		/// <summary>Gets or sets the foreground color of parent rows.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the foreground color of parent rows. The default is the <see cref="P:System.Drawing.SystemColors.WindowText" /> color.</returns>
		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06001194 RID: 4500 RVA: 0x0003E134 File Offset: 0x0003C334
		// (set) Token: 0x06001195 RID: 4501 RVA: 0x0003E141 File Offset: 0x0003C341
		[SRCategory("CatColors")]
		[SRDescription("DataGridParentRowsForeColorDescr")]
		public Color ParentRowsForeColor
		{
			get
			{
				return this.parentRows.ForeColor;
			}
			set
			{
				this.parentRows.ForeColor = value;
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06001196 RID: 4502 RVA: 0x0003E14F File Offset: 0x0003C34F
		internal SolidBrush ParentRowsForeBrush
		{
			get
			{
				return this.parentRows.ForeBrush;
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Forms.DataGrid.ParentRowsForeColor" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has been changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001197 RID: 4503 RVA: 0x0003E15C File Offset: 0x0003C35C
		protected virtual bool ShouldSerializeParentRowsForeColor()
		{
			return !this.ParentRowsForeBrush.Equals(DataGrid.DefaultParentRowsForeBrush);
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x0003E171 File Offset: 0x0003C371
		private void ResetParentRowsForeColor()
		{
			if (this.ShouldSerializeParentRowsForeColor())
			{
				this.parentRows.ForeBrush = DataGrid.DefaultParentRowsForeBrush;
			}
		}

		/// <summary>Gets or sets the default width of the grid columns in pixels.</summary>
		/// <returns>The default width (in pixels) of columns in the grid.</returns>
		/// <exception cref="T:System.ArgumentException">The property value is less than 0. </exception>
		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06001199 RID: 4505 RVA: 0x0003E18B File Offset: 0x0003C38B
		// (set) Token: 0x0600119A RID: 4506 RVA: 0x0003E193 File Offset: 0x0003C393
		[DefaultValue(75)]
		[SRCategory("CatLayout")]
		[SRDescription("DataGridPreferredColumnWidthDescr")]
		[TypeConverter(typeof(DataGridPreferredColumnWidthTypeConverter))]
		public int PreferredColumnWidth
		{
			get
			{
				return this.preferredColumnWidth;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(SR.GetString("DataGridColumnWidth"), "PreferredColumnWidth");
				}
				if (this.preferredColumnWidth != value)
				{
					this.preferredColumnWidth = value;
				}
			}
		}

		/// <summary>Gets or sets the preferred row height for the <see cref="T:System.Windows.Forms.DataGrid" /> control.</summary>
		/// <returns>The height of a row.</returns>
		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x0600119B RID: 4507 RVA: 0x0003E1BE File Offset: 0x0003C3BE
		// (set) Token: 0x0600119C RID: 4508 RVA: 0x0003E1C6 File Offset: 0x0003C3C6
		[SRCategory("CatLayout")]
		[SRDescription("DataGridPreferredRowHeightDescr")]
		public int PreferredRowHeight
		{
			get
			{
				return this.prefferedRowHeight;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(SR.GetString("DataGridRowRowHeight"));
				}
				this.prefferedRowHeight = value;
			}
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x0003E1E3 File Offset: 0x0003C3E3
		private void ResetPreferredRowHeight()
		{
			this.prefferedRowHeight = DataGrid.defaultFontHeight + 3;
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Forms.DataGrid.PreferredRowHeight" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600119E RID: 4510 RVA: 0x0003E1F2 File Offset: 0x0003C3F2
		protected bool ShouldSerializePreferredRowHeight()
		{
			return this.prefferedRowHeight != DataGrid.defaultFontHeight + 3;
		}

		/// <summary>Gets or sets a value indicating whether the grid is in read-only mode.</summary>
		/// <returns>
		///     <see langword="true" /> if the grid is in read-only mode; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x0600119F RID: 4511 RVA: 0x0003E206 File Offset: 0x0003C406
		// (set) Token: 0x060011A0 RID: 4512 RVA: 0x0003E218 File Offset: 0x0003C418
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("DataGridReadOnlyDescr")]
		public bool ReadOnly
		{
			get
			{
				return this.gridState[4096];
			}
			set
			{
				if (this.ReadOnly != value)
				{
					bool flag = false;
					if (value)
					{
						flag = this.policy.AllowAdd;
						this.policy.AllowRemove = false;
						this.policy.AllowEdit = false;
						this.policy.AllowAdd = false;
					}
					else
					{
						flag |= this.policy.UpdatePolicy(this.listManager, value);
					}
					this.gridState[4096] = value;
					DataGridRow[] array = this.DataGridRows;
					if (flag)
					{
						this.RecreateDataGridRows();
						DataGridRow[] array2 = this.DataGridRows;
						int num = Math.Min(array2.Length, array.Length);
						for (int i = 0; i < num; i++)
						{
							if (array[i].Selected)
							{
								array2[i].Selected = true;
							}
						}
					}
					base.PerformLayout();
					this.InvalidateInside();
					this.OnReadOnlyChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGrid.ReadOnly" /> property value changes.</summary>
		// Token: 0x140000BD RID: 189
		// (add) Token: 0x060011A1 RID: 4513 RVA: 0x0003E2ED File Offset: 0x0003C4ED
		// (remove) Token: 0x060011A2 RID: 4514 RVA: 0x0003E300 File Offset: 0x0003C500
		[SRCategory("CatPropertyChanged")]
		[SRDescription("DataGridOnReadOnlyChangedDescr")]
		public event EventHandler ReadOnlyChanged
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_READONLYCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_READONLYCHANGED, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the column headers of a table are visible.</summary>
		/// <returns>
		///     <see langword="true" /> if the column headers are visible; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x060011A3 RID: 4515 RVA: 0x0003E313 File Offset: 0x0003C513
		// (set) Token: 0x060011A4 RID: 4516 RVA: 0x0003E321 File Offset: 0x0003C521
		[SRCategory("CatDisplay")]
		[DefaultValue(true)]
		[SRDescription("DataGridColumnHeadersVisibleDescr")]
		public bool ColumnHeadersVisible
		{
			get
			{
				return this.gridState[2];
			}
			set
			{
				if (this.ColumnHeadersVisible != value)
				{
					this.gridState[2] = value;
					this.layout.ColumnHeadersVisible = value;
					base.PerformLayout();
					this.InvalidateInside();
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the parent rows of a table are visible.</summary>
		/// <returns>
		///     <see langword="true" /> if the parent rows are visible; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x060011A5 RID: 4517 RVA: 0x0003E351 File Offset: 0x0003C551
		// (set) Token: 0x060011A6 RID: 4518 RVA: 0x0003E35E File Offset: 0x0003C55E
		[SRCategory("CatDisplay")]
		[DefaultValue(true)]
		[SRDescription("DataGridParentRowsVisibleDescr")]
		public bool ParentRowsVisible
		{
			get
			{
				return this.layout.ParentRowsVisible;
			}
			set
			{
				if (this.layout.ParentRowsVisible != value)
				{
					this.SetParentRowsVisibility(value);
					this.caption.SetDownButtonDirection(!value);
					this.OnParentRowsVisibleChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGrid.ParentRowsVisible" /> property value changes.</summary>
		// Token: 0x140000BE RID: 190
		// (add) Token: 0x060011A7 RID: 4519 RVA: 0x0003E38F File Offset: 0x0003C58F
		// (remove) Token: 0x060011A8 RID: 4520 RVA: 0x0003E3A2 File Offset: 0x0003C5A2
		[SRCategory("CatPropertyChanged")]
		[SRDescription("DataGridOnParentRowsVisibleChangedDescr")]
		public event EventHandler ParentRowsVisibleChanged
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_PARENTROWSVISIBLECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_PARENTROWSVISIBLECHANGED, value);
			}
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x0003E3B5 File Offset: 0x0003C5B5
		internal bool ParentRowsIsEmpty()
		{
			return this.parentRows.IsEmpty();
		}

		/// <summary>Gets or sets a value that specifies whether row headers are visible.</summary>
		/// <returns>
		///     <see langword="true" /> if row headers are visible; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x060011AA RID: 4522 RVA: 0x0003E3C2 File Offset: 0x0003C5C2
		// (set) Token: 0x060011AB RID: 4523 RVA: 0x0003E3D0 File Offset: 0x0003C5D0
		[SRCategory("CatDisplay")]
		[DefaultValue(true)]
		[SRDescription("DataGridRowHeadersVisibleDescr")]
		public bool RowHeadersVisible
		{
			get
			{
				return this.gridState[4];
			}
			set
			{
				if (this.RowHeadersVisible != value)
				{
					this.gridState[4] = value;
					base.PerformLayout();
					this.InvalidateInside();
				}
			}
		}

		/// <summary>Gets or sets the width of row headers.</summary>
		/// <returns>The width of row headers in the <see cref="T:System.Windows.Forms.DataGrid" />. The default is 35.</returns>
		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x060011AC RID: 4524 RVA: 0x0003E3F4 File Offset: 0x0003C5F4
		// (set) Token: 0x060011AD RID: 4525 RVA: 0x0003E3FC File Offset: 0x0003C5FC
		[SRCategory("CatLayout")]
		[DefaultValue(35)]
		[SRDescription("DataGridRowHeaderWidthDescr")]
		public int RowHeaderWidth
		{
			get
			{
				return this.rowHeaderWidth;
			}
			set
			{
				value = Math.Max(this.minRowHeaderWidth, value);
				if (this.rowHeaderWidth != value)
				{
					this.rowHeaderWidth = value;
					if (this.layout.RowHeadersVisible)
					{
						base.PerformLayout();
						this.InvalidateInside();
					}
				}
			}
		}

		/// <summary>This member is not meaningful for this control.</summary>
		/// <returns>The text associated with this control.</returns>
		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x060011AE RID: 4526 RVA: 0x0001BFA5 File Offset: 0x0001A1A5
		// (set) Token: 0x060011AF RID: 4527 RVA: 0x0001BFAD File Offset: 0x0001A1AD
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Bindable(false)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.DataGrid.Text" /> property changes.</summary>
		// Token: 0x140000BF RID: 191
		// (add) Token: 0x060011B0 RID: 4528 RVA: 0x0003E435 File Offset: 0x0003C635
		// (remove) Token: 0x060011B1 RID: 4529 RVA: 0x0003E43E File Offset: 0x0003C63E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler TextChanged
		{
			add
			{
				base.TextChanged += value;
			}
			remove
			{
				base.TextChanged -= value;
			}
		}

		/// <summary>Gets the vertical scroll bar of the control.</summary>
		/// <returns>The vertical <see cref="T:System.Windows.Forms.ScrollBar" /> of the grid.</returns>
		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x060011B2 RID: 4530 RVA: 0x0003E447 File Offset: 0x0003C647
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRDescription("DataGridVertScrollBarDescr")]
		protected ScrollBar VertScrollBar
		{
			get
			{
				return this.vertScrollBar;
			}
		}

		/// <summary>Gets the number of visible columns.</summary>
		/// <returns>The number of columns visible in the viewport. The viewport is the rectangular area through which the grid is visible. The size of the viewport depends on the size of the <see cref="T:System.Windows.Forms.DataGrid" /> control; if you allow users to resize the control, the viewport will also be affected.</returns>
		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x060011B3 RID: 4531 RVA: 0x0003E44F File Offset: 0x0003C64F
		[Browsable(false)]
		[SRDescription("DataGridVisibleColumnCountDescr")]
		public int VisibleColumnCount
		{
			get
			{
				return Math.Min(this.numVisibleCols, (this.myGridTable == null) ? 0 : this.myGridTable.GridColumnStyles.Count);
			}
		}

		/// <summary>Gets the number of rows visible.</summary>
		/// <returns>The number of rows visible in the viewport. The viewport is the rectangular area through which the grid is visible. The size of the viewport depends on the size of the <see cref="T:System.Windows.Forms.DataGrid" /> control; if you allow users to resize the control, the viewport will also be affected.</returns>
		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x060011B4 RID: 4532 RVA: 0x0003E477 File Offset: 0x0003C677
		[Browsable(false)]
		[SRDescription("DataGridVisibleRowCountDescr")]
		public int VisibleRowCount
		{
			get
			{
				return this.numVisibleRows;
			}
		}

		/// <summary>Gets or sets the value of the cell at the specified the row and column.</summary>
		/// <param name="rowIndex">The zero-based index of the row containing the value. </param>
		/// <param name="columnIndex">The zero-based index of the column containing the value. </param>
		/// <returns>The value, typed as <see cref="T:System.Object" />, of the cell.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">While getting or setting, the <paramref name="rowIndex" /> is out of range.While getting or setting, the <paramref name="columnIndex" /> is out of range. </exception>
		// Token: 0x17000485 RID: 1157
		public object this[int rowIndex, int columnIndex]
		{
			get
			{
				this.EnsureBound();
				if (rowIndex < 0 || rowIndex >= this.DataGridRowsLength)
				{
					throw new ArgumentOutOfRangeException("rowIndex");
				}
				if (columnIndex < 0 || columnIndex >= this.myGridTable.GridColumnStyles.Count)
				{
					throw new ArgumentOutOfRangeException("columnIndex");
				}
				CurrencyManager source = this.listManager;
				DataGridColumnStyle dataGridColumnStyle = this.myGridTable.GridColumnStyles[columnIndex];
				return dataGridColumnStyle.GetColumnValueAtRow(source, rowIndex);
			}
			set
			{
				this.EnsureBound();
				if (rowIndex < 0 || rowIndex >= this.DataGridRowsLength)
				{
					throw new ArgumentOutOfRangeException("rowIndex");
				}
				if (columnIndex < 0 || columnIndex >= this.myGridTable.GridColumnStyles.Count)
				{
					throw new ArgumentOutOfRangeException("columnIndex");
				}
				CurrencyManager currencyManager = this.listManager;
				if (currencyManager.Position != rowIndex)
				{
					currencyManager.Position = rowIndex;
				}
				DataGridColumnStyle dataGridColumnStyle = this.myGridTable.GridColumnStyles[columnIndex];
				dataGridColumnStyle.SetColumnValueAtRow(currencyManager, rowIndex, value);
				if (columnIndex >= this.firstVisibleCol && columnIndex <= this.firstVisibleCol + this.numVisibleCols - 1 && rowIndex >= this.firstVisibleRow && rowIndex <= this.firstVisibleRow + this.numVisibleRows)
				{
					Rectangle cellBounds = this.GetCellBounds(rowIndex, columnIndex);
					base.Invalidate(cellBounds);
				}
			}
		}

		/// <summary>Gets or sets the value of a specified <see cref="T:System.Windows.Forms.DataGridCell" />.</summary>
		/// <param name="cell">A <see cref="T:System.Windows.Forms.DataGridCell" /> that represents a cell in the grid. </param>
		/// <returns>The value, typed as <see cref="T:System.Object" />, of the cell.</returns>
		// Token: 0x17000486 RID: 1158
		public object this[DataGridCell cell]
		{
			get
			{
				return this[cell.RowNumber, cell.ColumnNumber];
			}
			set
			{
				this[cell.RowNumber, cell.ColumnNumber] = value;
			}
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x0003E5E0 File Offset: 0x0003C7E0
		private void WireTableStylePropChanged(DataGridTableStyle gridTable)
		{
			gridTable.GridLineColorChanged += this.GridLineColorChanged;
			gridTable.GridLineStyleChanged += this.GridLineStyleChanged;
			gridTable.HeaderBackColorChanged += this.HeaderBackColorChanged;
			gridTable.HeaderFontChanged += this.HeaderFontChanged;
			gridTable.HeaderForeColorChanged += this.HeaderForeColorChanged;
			gridTable.LinkColorChanged += this.LinkColorChanged;
			gridTable.LinkHoverColorChanged += this.LinkHoverColorChanged;
			gridTable.PreferredColumnWidthChanged += this.PreferredColumnWidthChanged;
			gridTable.RowHeadersVisibleChanged += this.RowHeadersVisibleChanged;
			gridTable.ColumnHeadersVisibleChanged += this.ColumnHeadersVisibleChanged;
			gridTable.RowHeaderWidthChanged += this.RowHeaderWidthChanged;
			gridTable.AllowSortingChanged += this.AllowSortingChanged;
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x0003E6C8 File Offset: 0x0003C8C8
		private void UnWireTableStylePropChanged(DataGridTableStyle gridTable)
		{
			gridTable.GridLineColorChanged -= this.GridLineColorChanged;
			gridTable.GridLineStyleChanged -= this.GridLineStyleChanged;
			gridTable.HeaderBackColorChanged -= this.HeaderBackColorChanged;
			gridTable.HeaderFontChanged -= this.HeaderFontChanged;
			gridTable.HeaderForeColorChanged -= this.HeaderForeColorChanged;
			gridTable.LinkColorChanged -= this.LinkColorChanged;
			gridTable.LinkHoverColorChanged -= this.LinkHoverColorChanged;
			gridTable.PreferredColumnWidthChanged -= this.PreferredColumnWidthChanged;
			gridTable.RowHeadersVisibleChanged -= this.RowHeadersVisibleChanged;
			gridTable.ColumnHeadersVisibleChanged -= this.ColumnHeadersVisibleChanged;
			gridTable.RowHeaderWidthChanged -= this.RowHeaderWidthChanged;
			gridTable.AllowSortingChanged -= this.AllowSortingChanged;
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x0003E7B0 File Offset: 0x0003C9B0
		private void WireDataSource()
		{
			this.listManager.CurrentChanged += this.currentChangedHandler;
			this.listManager.PositionChanged += this.positionChangedHandler;
			this.listManager.ItemChanged += this.itemChangedHandler;
			this.listManager.MetaDataChanged += this.metaDataChangedHandler;
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x0003E804 File Offset: 0x0003CA04
		private void UnWireDataSource()
		{
			this.listManager.CurrentChanged -= this.currentChangedHandler;
			this.listManager.PositionChanged -= this.positionChangedHandler;
			this.listManager.ItemChanged -= this.itemChangedHandler;
			this.listManager.MetaDataChanged -= this.metaDataChangedHandler;
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x0003E858 File Offset: 0x0003CA58
		private void DataSource_Changed(object sender, EventArgs ea)
		{
			this.policy.UpdatePolicy(this.ListManager, this.ReadOnly);
			if (this.gridState[512])
			{
				DataGridRow[] array = this.DataGridRows;
				int num = this.DataGridRowsLength;
				array[num - 1] = new DataGridRelationshipRow(this, this.myGridTable, num - 1);
				this.SetDataGridRows(array, num);
			}
			else if (this.gridState[1048576] && !this.gridState[1024])
			{
				this.listManager.CancelCurrentEdit();
				this.gridState[1048576] = false;
				this.RecreateDataGridRows();
			}
			else if (!this.gridState[1024])
			{
				this.RecreateDataGridRows();
				this.currentRow = Math.Min(this.currentRow, this.listManager.Count);
			}
			bool listHasErrors = this.ListHasErrors;
			this.ListHasErrors = this.DataGridSourceHasErrors();
			if (listHasErrors == this.ListHasErrors)
			{
				this.InvalidateInside();
			}
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x0003E959 File Offset: 0x0003CB59
		private void GridLineColorChanged(object sender, EventArgs e)
		{
			base.Invalidate(this.layout.Data);
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x0003E96C File Offset: 0x0003CB6C
		private void GridLineStyleChanged(object sender, EventArgs e)
		{
			this.myGridTable.ResetRelationsUI();
			base.Invalidate(this.layout.Data);
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x0003E98C File Offset: 0x0003CB8C
		private void HeaderBackColorChanged(object sender, EventArgs e)
		{
			if (this.layout.RowHeadersVisible)
			{
				base.Invalidate(this.layout.RowHeaders);
			}
			if (this.layout.ColumnHeadersVisible)
			{
				base.Invalidate(this.layout.ColumnHeaders);
			}
			base.Invalidate(this.layout.TopLeftHeader);
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x0003E9E6 File Offset: 0x0003CBE6
		private void HeaderFontChanged(object sender, EventArgs e)
		{
			this.RecalculateFonts();
			base.PerformLayout();
			base.Invalidate(this.layout.Inside);
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x0003EA08 File Offset: 0x0003CC08
		private void HeaderForeColorChanged(object sender, EventArgs e)
		{
			if (this.layout.RowHeadersVisible)
			{
				base.Invalidate(this.layout.RowHeaders);
			}
			if (this.layout.ColumnHeadersVisible)
			{
				base.Invalidate(this.layout.ColumnHeaders);
			}
			base.Invalidate(this.layout.TopLeftHeader);
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x0003E959 File Offset: 0x0003CB59
		private void LinkColorChanged(object sender, EventArgs e)
		{
			base.Invalidate(this.layout.Data);
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x0003E959 File Offset: 0x0003CB59
		private void LinkHoverColorChanged(object sender, EventArgs e)
		{
			base.Invalidate(this.layout.Data);
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x0003EA62 File Offset: 0x0003CC62
		private void PreferredColumnWidthChanged(object sender, EventArgs e)
		{
			this.SetDataGridRows(null, this.DataGridRowsLength);
			base.PerformLayout();
			base.Invalidate();
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x0003EA7D File Offset: 0x0003CC7D
		private void RowHeadersVisibleChanged(object sender, EventArgs e)
		{
			this.layout.RowHeadersVisible = (this.myGridTable != null && this.myGridTable.RowHeadersVisible);
			base.PerformLayout();
			this.InvalidateInside();
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x0003EAAC File Offset: 0x0003CCAC
		private void ColumnHeadersVisibleChanged(object sender, EventArgs e)
		{
			this.layout.ColumnHeadersVisible = (this.myGridTable != null && this.myGridTable.ColumnHeadersVisible);
			base.PerformLayout();
			this.InvalidateInside();
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x0003EADB File Offset: 0x0003CCDB
		private void RowHeaderWidthChanged(object sender, EventArgs e)
		{
			if (this.layout.RowHeadersVisible)
			{
				base.PerformLayout();
				this.InvalidateInside();
			}
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x0003EAF8 File Offset: 0x0003CCF8
		private void AllowSortingChanged(object sender, EventArgs e)
		{
			if (!this.myGridTable.AllowSorting && this.listManager != null)
			{
				IList list = this.listManager.List;
				if (list is IBindingList)
				{
					((IBindingList)list).RemoveSort();
				}
			}
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x0003EB3C File Offset: 0x0003CD3C
		private void DataSource_RowChanged(object sender, EventArgs ea)
		{
			DataGridRow[] array = this.DataGridRows;
			if (this.currentRow < this.DataGridRowsLength)
			{
				this.InvalidateRow(this.currentRow);
			}
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x0003EB6C File Offset: 0x0003CD6C
		private void DataSource_PositionChanged(object sender, EventArgs ea)
		{
			if (this.DataGridRowsLength > this.listManager.Count + (this.policy.AllowAdd ? 1 : 0) && !this.gridState[1024])
			{
				this.RecreateDataGridRows();
			}
			if (this.ListManager.Position != this.currentRow)
			{
				this.CurrentCell = new DataGridCell(this.listManager.Position, this.currentCol);
			}
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x0003EBE5 File Offset: 0x0003CDE5
		internal void DataSource_MetaDataChanged(object sender, EventArgs e)
		{
			this.MetaDataChanged();
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x0003EBF0 File Offset: 0x0003CDF0
		private bool DataGridSourceHasErrors()
		{
			if (this.listManager == null)
			{
				return false;
			}
			for (int i = 0; i < this.listManager.Count; i++)
			{
				object obj = this.listManager[i];
				if (obj is IDataErrorInfo)
				{
					string error = ((IDataErrorInfo)obj).Error;
					if (error != null && error.Length != 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x0003EC4C File Offset: 0x0003CE4C
		private void TableStylesCollectionChanged(object sender, CollectionChangeEventArgs ccea)
		{
			if (sender != this.dataGridTables)
			{
				return;
			}
			if (this.listManager == null)
			{
				return;
			}
			if (ccea.Action == CollectionChangeAction.Add)
			{
				DataGridTableStyle dataGridTableStyle = (DataGridTableStyle)ccea.Element;
				if (this.listManager.GetListName().Equals(dataGridTableStyle.MappingName))
				{
					this.SetDataGridTable(dataGridTableStyle, true);
					this.SetDataGridRows(null, 0);
					return;
				}
			}
			else if (ccea.Action == CollectionChangeAction.Remove)
			{
				DataGridTableStyle dataGridTableStyle2 = (DataGridTableStyle)ccea.Element;
				if (this.myGridTable.MappingName.Equals(dataGridTableStyle2.MappingName))
				{
					this.defaultTableStyle.GridColumnStyles.ResetDefaultColumnCollection();
					this.SetDataGridTable(this.defaultTableStyle, true);
					this.SetDataGridRows(null, 0);
					return;
				}
			}
			else
			{
				DataGridTableStyle dataGridTableStyle3 = this.dataGridTables[this.listManager.GetListName()];
				if (dataGridTableStyle3 == null)
				{
					if (!this.myGridTable.IsDefault)
					{
						this.defaultTableStyle.GridColumnStyles.ResetDefaultColumnCollection();
						this.SetDataGridTable(this.defaultTableStyle, true);
						this.SetDataGridRows(null, 0);
						return;
					}
				}
				else
				{
					this.SetDataGridTable(dataGridTableStyle3, true);
					this.SetDataGridRows(null, 0);
				}
			}
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x0003ED60 File Offset: 0x0003CF60
		private void DataSource_ItemChanged(object sender, ItemChangedEventArgs ea)
		{
			if (ea.Index == -1)
			{
				this.DataSource_Changed(sender, EventArgs.Empty);
				return;
			}
			object obj = this.listManager[ea.Index];
			bool listHasErrors = this.ListHasErrors;
			if (obj is IDataErrorInfo)
			{
				if (((IDataErrorInfo)obj).Error.Length != 0)
				{
					this.ListHasErrors = true;
				}
				else if (this.ListHasErrors)
				{
					this.ListHasErrors = this.DataGridSourceHasErrors();
				}
			}
			if (listHasErrors == this.ListHasErrors)
			{
				this.InvalidateRow(ea.Index);
			}
			if (this.editColumn != null && ea.Index == this.currentRow)
			{
				this.editColumn.UpdateUI(this.ListManager, ea.Index, null);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGrid.BorderStyleChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060011D0 RID: 4560 RVA: 0x0003EE18 File Offset: 0x0003D018
		protected virtual void OnBorderStyleChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGrid.EVENT_BORDERSTYLECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGrid.CaptionVisibleChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060011D1 RID: 4561 RVA: 0x0003EE48 File Offset: 0x0003D048
		protected virtual void OnCaptionVisibleChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGrid.EVENT_CAPTIONVISIBLECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGrid.CurrentCellChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060011D2 RID: 4562 RVA: 0x0003EE78 File Offset: 0x0003D078
		protected virtual void OnCurrentCellChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGrid.EVENT_CURRENTCELLCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGrid.FlatModeChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060011D3 RID: 4563 RVA: 0x0003EEA8 File Offset: 0x0003D0A8
		protected virtual void OnFlatModeChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGrid.EVENT_FLATMODECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGrid.BackgroundColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060011D4 RID: 4564 RVA: 0x0003EED8 File Offset: 0x0003D0D8
		protected virtual void OnBackgroundColorChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGrid.EVENT_BACKGROUNDCOLORCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGrid.AllowNavigationChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060011D5 RID: 4565 RVA: 0x0003EF08 File Offset: 0x0003D108
		protected virtual void OnAllowNavigationChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGrid.EVENT_ALLOWNAVIGATIONCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGrid.ParentRowsVisibleChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060011D6 RID: 4566 RVA: 0x0003EF38 File Offset: 0x0003D138
		protected virtual void OnParentRowsVisibleChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGrid.EVENT_PARENTROWSVISIBLECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGrid.ParentRowsLabelStyleChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060011D7 RID: 4567 RVA: 0x0003EF68 File Offset: 0x0003D168
		protected virtual void OnParentRowsLabelStyleChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGrid.EVENT_PARENTROWSLABELSTYLECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGrid.ReadOnlyChanged" /> event </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060011D8 RID: 4568 RVA: 0x0003EF98 File Offset: 0x0003D198
		protected virtual void OnReadOnlyChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGrid.EVENT_READONLYCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGrid.Navigate" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.NavigateEventArgs" /> that contains the event data. </param>
		// Token: 0x060011D9 RID: 4569 RVA: 0x0003EFC6 File Offset: 0x0003D1C6
		protected void OnNavigate(NavigateEventArgs e)
		{
			if (this.onNavigate != null)
			{
				this.onNavigate(this, e);
			}
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x0003EFE0 File Offset: 0x0003D1E0
		internal void OnNodeClick(EventArgs e)
		{
			base.PerformLayout();
			GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
			if (this.firstVisibleCol > -1 && this.firstVisibleCol < gridColumnStyles.Count && gridColumnStyles[this.firstVisibleCol] == this.editColumn)
			{
				this.Edit();
			}
			EventHandler eventHandler = (EventHandler)base.Events[DataGrid.EVENT_NODECLICKED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGrid.RowHeaderClick" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060011DB RID: 4571 RVA: 0x0003F051 File Offset: 0x0003D251
		protected void OnRowHeaderClick(EventArgs e)
		{
			if (this.onRowHeaderClick != null)
			{
				this.onRowHeaderClick(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGrid.Scroll" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060011DC RID: 4572 RVA: 0x0003F068 File Offset: 0x0003D268
		protected void OnScroll(EventArgs e)
		{
			if (this.ToolTipProvider != null)
			{
				this.ResetToolTip();
			}
			EventHandler eventHandler = (EventHandler)base.Events[DataGrid.EVENT_SCROLL];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Listens for the scroll event of the horizontal scroll bar.</summary>
		/// <param name="sender">An <see cref="T:System.Object" /> that contains data about the control. </param>
		/// <param name="se">A <see cref="T:System.Windows.Forms.ScrollEventArgs" /> that contains the event data. </param>
		// Token: 0x060011DD RID: 4573 RVA: 0x0003F0A4 File Offset: 0x0003D2A4
		protected virtual void GridHScrolled(object sender, ScrollEventArgs se)
		{
			if (!base.Enabled)
			{
				return;
			}
			if (this.DataSource == null)
			{
				return;
			}
			this.gridState[131072] = true;
			if (se.Type == ScrollEventType.SmallIncrement || se.Type == ScrollEventType.SmallDecrement)
			{
				int num = (se.Type == ScrollEventType.SmallIncrement) ? 1 : -1;
				if (se.Type == ScrollEventType.SmallDecrement && this.negOffset == 0)
				{
					GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
					int num2 = this.firstVisibleCol - 1;
					while (num2 >= 0 && gridColumnStyles[num2].Width == 0)
					{
						num--;
						num2--;
					}
				}
				if (se.Type == ScrollEventType.SmallIncrement && this.negOffset == 0)
				{
					GridColumnStylesCollection gridColumnStyles2 = this.myGridTable.GridColumnStyles;
					int num3 = this.firstVisibleCol;
					while (num3 > -1 && num3 < gridColumnStyles2.Count && gridColumnStyles2[num3].Width == 0)
					{
						num++;
						num3++;
					}
				}
				this.ScrollRight(num);
				se.NewValue = this.HorizontalOffset;
			}
			else if (se.Type != ScrollEventType.EndScroll)
			{
				this.HorizontalOffset = se.NewValue;
			}
			this.gridState[131072] = false;
		}

		/// <summary>Listens for the scroll event of the vertical scroll bar.</summary>
		/// <param name="sender">An <see cref="T:System.Object" /> that contains data about the control. </param>
		/// <param name="se">A <see cref="T:System.Windows.Forms.ScrollEventArgs" /> that contains the event data. </param>
		// Token: 0x060011DE RID: 4574 RVA: 0x0003F1C4 File Offset: 0x0003D3C4
		protected virtual void GridVScrolled(object sender, ScrollEventArgs se)
		{
			if (!base.Enabled)
			{
				return;
			}
			if (this.DataSource == null)
			{
				return;
			}
			this.gridState[131072] = true;
			try
			{
				se.NewValue = Math.Min(se.NewValue, this.DataGridRowsLength - this.numTotallyVisibleRows);
				int rows = se.NewValue - this.firstVisibleRow;
				this.ScrollDown(rows);
			}
			finally
			{
				this.gridState[131072] = false;
			}
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x0003F24C File Offset: 0x0003D44C
		private void HandleEndCurrentEdit()
		{
			int num = this.currentRow;
			int num2 = this.currentCol;
			string text = null;
			try
			{
				this.listManager.EndCurrentEdit();
			}
			catch (Exception ex)
			{
				text = ex.Message;
			}
			if (text != null)
			{
				DialogResult dialogResult = RTLAwareMessageBox.Show(null, SR.GetString("DataGridPushedIncorrectValueIntoColumn", new object[]
				{
					text
				}), SR.GetString("DataGridErrorMessageBoxCaption"), MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
				if (dialogResult == DialogResult.Yes)
				{
					this.currentRow = num;
					this.currentCol = num2;
					this.InvalidateRowHeader(this.currentRow);
					this.Edit();
					return;
				}
				this.listManager.PositionChanged -= this.positionChangedHandler;
				this.listManager.CancelCurrentEdit();
				this.listManager.Position = this.currentRow;
				this.listManager.PositionChanged += this.positionChangedHandler;
			}
		}

		/// <summary>Listens for the caption's back button clicked event.</summary>
		/// <param name="sender">An <see cref="T:System.Object" /> that contains data about the control. </param>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains data about the event. </param>
		// Token: 0x060011E0 RID: 4576 RVA: 0x0003F328 File Offset: 0x0003D528
		protected void OnBackButtonClicked(object sender, EventArgs e)
		{
			this.NavigateBack();
			EventHandler eventHandler = (EventHandler)base.Events[DataGrid.EVENT_BACKBUTTONCLICK];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.BackColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060011E1 RID: 4577 RVA: 0x0003F35C File Offset: 0x0003D55C
		protected override void OnBackColorChanged(EventArgs e)
		{
			this.backBrush = new SolidBrush(this.BackColor);
			base.Invalidate();
			base.OnBackColorChanged(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.BindingContextChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060011E2 RID: 4578 RVA: 0x0003F37C File Offset: 0x0003D57C
		protected override void OnBindingContextChanged(EventArgs e)
		{
			if (this.DataSource != null && !this.gridState[2097152])
			{
				try
				{
					this.Set_ListManager(this.DataSource, this.DataMember, true, false);
				}
				catch
				{
					if (this.Site == null || !this.Site.DesignMode)
					{
						throw;
					}
					RTLAwareMessageBox.Show(null, SR.GetString("DataGridExceptionInPaint"), null, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
					if (base.Visible)
					{
						base.BeginUpdateInternal();
					}
					this.ResetParentRows();
					this.Set_ListManager(null, string.Empty, true);
					if (base.Visible)
					{
						base.EndUpdateInternal();
					}
				}
			}
			base.OnBindingContextChanged(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGrid.DataSourceChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060011E3 RID: 4579 RVA: 0x0003F434 File Offset: 0x0003D634
		protected virtual void OnDataSourceChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGrid.EVENT_DATASOURCECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGrid.ShowParentDetailsButtonClick" /> event.</summary>
		/// <param name="sender">The source of the event. </param>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060011E4 RID: 4580 RVA: 0x0003F464 File Offset: 0x0003D664
		protected void OnShowParentDetailsButtonClicked(object sender, EventArgs e)
		{
			this.ParentRowsVisible = !this.caption.ToggleDownButtonDirection();
			EventHandler eventHandler = (EventHandler)base.Events[DataGrid.EVENT_DOWNBUTTONCLICK];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ForeColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060011E5 RID: 4581 RVA: 0x0003F4A6 File Offset: 0x0003D6A6
		protected override void OnForeColorChanged(EventArgs e)
		{
			this.foreBrush = new SolidBrush(this.ForeColor);
			base.Invalidate();
			base.OnForeColorChanged(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060011E6 RID: 4582 RVA: 0x0003F4C8 File Offset: 0x0003D6C8
		protected override void OnFontChanged(EventArgs e)
		{
			this.Caption.OnGridFontChanged();
			this.RecalculateFonts();
			this.RecreateDataGridRows();
			if (this.originalState != null)
			{
				Stack stack = new Stack();
				while (!this.parentRows.IsEmpty())
				{
					DataGridState dataGridState = this.parentRows.PopTop();
					int num = dataGridState.DataGridRowsLength;
					for (int i = 0; i < num; i++)
					{
						dataGridState.DataGridRows[i].Height = dataGridState.DataGridRows[i].MinimumRowHeight(dataGridState.GridColumnStyles);
					}
					stack.Push(dataGridState);
				}
				while (stack.Count != 0)
				{
					this.parentRows.AddParent((DataGridState)stack.Pop());
				}
			}
			base.OnFontChanged(e);
		}

		/// <summary>Overrides <see cref="M:System.Windows.Forms.Control.OnPaintBackground(System.Windows.Forms.PaintEventArgs)" /> to prevent painting the background of the <see cref="T:System.Windows.Forms.DataGrid" /> control.</summary>
		/// <param name="ebe">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains information about the control to paint. </param>
		// Token: 0x060011E7 RID: 4583 RVA: 0x0000701A File Offset: 0x0000521A
		protected override void OnPaintBackground(PaintEventArgs ebe)
		{
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Layout" /> event, which repositions controls and updates scroll bars.</summary>
		/// <param name="levent">A <see cref="T:System.Windows.Forms.LayoutEventArgs" /> that contains the event data. </param>
		// Token: 0x060011E8 RID: 4584 RVA: 0x0003F578 File Offset: 0x0003D778
		protected override void OnLayout(LayoutEventArgs levent)
		{
			if (this.gridState[65536])
			{
				return;
			}
			base.OnLayout(levent);
			if (this.gridState[16777216])
			{
				return;
			}
			this.gridState[2048] = false;
			try
			{
				if (base.IsHandleCreated)
				{
					if (this.layout.ParentRowsVisible)
					{
						this.parentRows.OnLayout();
					}
					if (this.ToolTipProvider != null)
					{
						this.ResetToolTip();
					}
					this.ComputeLayout();
				}
			}
			finally
			{
				this.gridState[2048] = true;
			}
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.Control.CreateHandle" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060011E9 RID: 4585 RVA: 0x0003F61C File Offset: 0x0003D81C
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			this.toolTipProvider = new DataGridToolTip(this);
			this.toolTipProvider.CreateToolTipHandle();
			this.toolTipId = 0;
			base.PerformLayout();
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.Control.DestroyHandle" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> containing the event data. </param>
		// Token: 0x060011EA RID: 4586 RVA: 0x0003F649 File Offset: 0x0003D849
		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);
			if (this.toolTipProvider != null)
			{
				this.toolTipProvider.Destroy();
				this.toolTipProvider = null;
			}
			this.toolTipId = 0;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Enter" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060011EB RID: 4587 RVA: 0x0003F673 File Offset: 0x0003D873
		protected override void OnEnter(EventArgs e)
		{
			if (this.gridState[2048] && !this.gridState[65536])
			{
				if (this.Bound)
				{
					this.Edit();
				}
				base.OnEnter(e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Leave" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060011EC RID: 4588 RVA: 0x0003F6AE File Offset: 0x0003D8AE
		protected override void OnLeave(EventArgs e)
		{
			this.OnLeave_Grid();
			base.OnLeave(e);
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x0003F6C0 File Offset: 0x0003D8C0
		private void OnLeave_Grid()
		{
			this.gridState[2048] = false;
			try
			{
				this.EndEdit();
				if (this.listManager != null && !this.gridState[65536])
				{
					if (this.gridState[1048576])
					{
						this.listManager.CancelCurrentEdit();
						DataGridRow[] array = this.DataGridRows;
						array[this.DataGridRowsLength - 1] = new DataGridAddNewRow(this, this.myGridTable, this.DataGridRowsLength - 1);
						this.SetDataGridRows(array, this.DataGridRowsLength);
					}
					else
					{
						this.HandleEndCurrentEdit();
					}
				}
			}
			finally
			{
				this.gridState[2048] = true;
				if (!this.gridState[65536])
				{
					this.gridState[1048576] = false;
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.KeyDown" /> event.</summary>
		/// <param name="ke">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that provides data about the <see cref="M:System.Windows.Forms.Control.OnKeyDown(System.Windows.Forms.KeyEventArgs)" /> event. </param>
		// Token: 0x060011EE RID: 4590 RVA: 0x0003F79C File Offset: 0x0003D99C
		protected override void OnKeyDown(KeyEventArgs ke)
		{
			base.OnKeyDown(ke);
			this.ProcessGridKey(ke);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.KeyPress" /> event.</summary>
		/// <param name="kpe">A <see cref="T:System.Windows.Forms.KeyPressEventArgs" /> that contains data about the <see cref="M:System.Windows.Forms.Control.OnKeyPress(System.Windows.Forms.KeyPressEventArgs)" /> event </param>
		// Token: 0x060011EF RID: 4591 RVA: 0x0003F7B0 File Offset: 0x0003D9B0
		protected override void OnKeyPress(KeyPressEventArgs kpe)
		{
			base.OnKeyPress(kpe);
			GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
			if (gridColumnStyles != null && this.currentCol > 0 && this.currentCol < gridColumnStyles.Count && !gridColumnStyles[this.currentCol].ReadOnly && kpe.KeyChar > ' ')
			{
				this.Edit(new string(new char[]
				{
					kpe.KeyChar
				}));
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains data about the <see cref="M:System.Windows.Forms.Control.OnMouseDown(System.Windows.Forms.MouseEventArgs)" /> event. </param>
		// Token: 0x060011F0 RID: 4592 RVA: 0x0003F824 File Offset: 0x0003DA24
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			this.gridState[524288] = false;
			this.gridState[256] = false;
			if (this.listManager == null)
			{
				return;
			}
			DataGrid.HitTestInfo hitTestInfo = this.HitTest(e.X, e.Y);
			Keys modifierKeys = Control.ModifierKeys;
			bool flag = (modifierKeys & Keys.Control) == Keys.Control && (modifierKeys & Keys.Alt) == Keys.None;
			bool flag2 = (modifierKeys & Keys.Shift) == Keys.Shift;
			if (e.Button != MouseButtons.Left)
			{
				return;
			}
			if (hitTestInfo.type == DataGrid.HitTestType.ColumnResize)
			{
				if (e.Clicks > 1)
				{
					this.ColAutoResize(hitTestInfo.col);
					return;
				}
				this.ColResizeBegin(e, hitTestInfo.col);
				return;
			}
			else if (hitTestInfo.type == DataGrid.HitTestType.RowResize)
			{
				if (e.Clicks > 1)
				{
					this.RowAutoResize(hitTestInfo.row);
					return;
				}
				this.RowResizeBegin(e, hitTestInfo.row);
				return;
			}
			else
			{
				if (hitTestInfo.type == DataGrid.HitTestType.ColumnHeader)
				{
					this.trackColumnHeader = this.myGridTable.GridColumnStyles[hitTestInfo.col].PropertyDescriptor;
					return;
				}
				if (hitTestInfo.type == DataGrid.HitTestType.Caption)
				{
					Rectangle rectangle = this.layout.Caption;
					this.caption.MouseDown(e.X - rectangle.X, e.Y - rectangle.Y);
					return;
				}
				if (this.layout.Data.Contains(e.X, e.Y) || this.layout.RowHeaders.Contains(e.X, e.Y))
				{
					int rowFromY = this.GetRowFromY(e.Y);
					if (rowFromY > -1)
					{
						Point point = this.NormalizeToRow(e.X, e.Y, rowFromY);
						DataGridRow[] array = this.DataGridRows;
						if (array[rowFromY].OnMouseDown(point.X, point.Y, this.layout.RowHeaders, this.isRightToLeft()))
						{
							this.CommitEdit();
							array = this.DataGridRows;
							if (rowFromY < this.DataGridRowsLength && array[rowFromY] is DataGridRelationshipRow && ((DataGridRelationshipRow)array[rowFromY]).Expanded)
							{
								this.EnsureVisible(rowFromY, 0);
							}
							this.Edit();
							return;
						}
					}
				}
				if (hitTestInfo.type == DataGrid.HitTestType.RowHeader)
				{
					this.EndEdit();
					if (!(this.DataGridRows[hitTestInfo.row] is DataGridAddNewRow))
					{
						int num = this.currentRow;
						this.CurrentCell = new DataGridCell(hitTestInfo.row, this.currentCol);
						if (hitTestInfo.row != num && this.currentRow != hitTestInfo.row && this.currentRow == num)
						{
							return;
						}
					}
					if (flag)
					{
						if (this.IsSelected(hitTestInfo.row))
						{
							this.UnSelect(hitTestInfo.row);
						}
						else
						{
							this.Select(hitTestInfo.row);
						}
					}
					else
					{
						if (this.lastRowSelected != -1 && flag2)
						{
							int num2 = Math.Min(this.lastRowSelected, hitTestInfo.row);
							int num3 = Math.Max(this.lastRowSelected, hitTestInfo.row);
							int num4 = this.lastRowSelected;
							this.ResetSelection();
							this.lastRowSelected = num4;
							DataGridRow[] array2 = this.DataGridRows;
							for (int i = num2; i <= num3; i++)
							{
								array2[i].Selected = true;
								this.numSelectedRows++;
							}
							this.EndEdit();
							return;
						}
						this.ResetSelection();
						this.Select(hitTestInfo.row);
					}
					this.lastRowSelected = hitTestInfo.row;
					return;
				}
				if (hitTestInfo.type == DataGrid.HitTestType.ParentRows)
				{
					this.EndEdit();
					this.parentRows.OnMouseDown(e.X, e.Y, this.isRightToLeft());
				}
				if (hitTestInfo.type == DataGrid.HitTestType.Cell)
				{
					if (this.myGridTable.GridColumnStyles[hitTestInfo.col].MouseDown(hitTestInfo.row, e.X, e.Y))
					{
						return;
					}
					DataGridCell dataGridCell = new DataGridCell(hitTestInfo.row, hitTestInfo.col);
					if (this.policy.AllowEdit && this.CurrentCell.Equals(dataGridCell))
					{
						this.ResetSelection();
						this.EnsureVisible(this.currentRow, this.currentCol);
						this.Edit();
						return;
					}
					this.ResetSelection();
					this.CurrentCell = dataGridCell;
				}
				return;
			}
		}

		/// <summary>Creates the <see cref="E:System.Windows.Forms.Control.MouseLeave" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains data about the <see cref="M:System.Windows.Forms.Control.OnMouseLeave(System.EventArgs)" /> event. </param>
		// Token: 0x060011F1 RID: 4593 RVA: 0x0003FC70 File Offset: 0x0003DE70
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			if (this.oldRow != -1)
			{
				DataGridRow[] array = this.DataGridRows;
				array[this.oldRow].OnMouseLeft(this.layout.RowHeaders, this.isRightToLeft());
			}
			if (this.gridState[262144])
			{
				this.caption.MouseLeft();
			}
			this.Cursor = null;
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x0003FCD6 File Offset: 0x0003DED6
		internal void TextBoxOnMouseWheel(MouseEventArgs e)
		{
			this.OnMouseWheel(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseMove" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains data about the <see cref="M:System.Windows.Forms.Control.OnMouseMove(System.Windows.Forms.MouseEventArgs)" /> event. </param>
		// Token: 0x060011F3 RID: 4595 RVA: 0x0003FCE0 File Offset: 0x0003DEE0
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (this.listManager == null)
			{
				return;
			}
			DataGrid.HitTestInfo hitTestInfo = this.HitTest(e.X, e.Y);
			bool alignToRight = this.isRightToLeft();
			if (this.gridState[8])
			{
				this.ColResizeMove(e);
			}
			if (this.gridState[16])
			{
				this.RowResizeMove(e);
			}
			if (this.gridState[8] || hitTestInfo.type == DataGrid.HitTestType.ColumnResize)
			{
				this.Cursor = Cursors.SizeWE;
				return;
			}
			if (this.gridState[16] || hitTestInfo.type == DataGrid.HitTestType.RowResize)
			{
				this.Cursor = Cursors.SizeNS;
				return;
			}
			this.Cursor = null;
			if (this.layout.Data.Contains(e.X, e.Y) || (this.layout.RowHeadersVisible && this.layout.RowHeaders.Contains(e.X, e.Y)))
			{
				DataGridRow[] array = this.DataGridRows;
				int rowFromY = this.GetRowFromY(e.Y);
				if (this.lastRowSelected != -1 && !this.gridState[256])
				{
					int rowTop = this.GetRowTop(this.lastRowSelected);
					int num = rowTop + array[this.lastRowSelected].Height;
					int height = SystemInformation.DragSize.Height;
					this.gridState[256] = ((e.Y - rowTop < height && rowTop - e.Y < height) || (e.Y - num < height && num - e.Y < height));
				}
				if (rowFromY > -1)
				{
					Point point = this.NormalizeToRow(e.X, e.Y, rowFromY);
					if (!array[rowFromY].OnMouseMove(point.X, point.Y, this.layout.RowHeaders, alignToRight) && this.gridState[256])
					{
						MouseButtons mouseButtons = Control.MouseButtons;
						if (this.lastRowSelected != -1 && (mouseButtons & MouseButtons.Left) == MouseButtons.Left && ((Control.ModifierKeys & Keys.Control) != Keys.Control || (Control.ModifierKeys & Keys.Alt) != Keys.None))
						{
							int num2 = this.lastRowSelected;
							this.ResetSelection();
							this.lastRowSelected = num2;
							int num3 = Math.Min(this.lastRowSelected, rowFromY);
							int num4 = Math.Max(this.lastRowSelected, rowFromY);
							DataGridRow[] array2 = this.DataGridRows;
							for (int i = num3; i <= num4; i++)
							{
								array2[i].Selected = true;
								this.numSelectedRows++;
							}
						}
					}
				}
				if (this.oldRow != rowFromY && this.oldRow != -1)
				{
					array[this.oldRow].OnMouseLeft(this.layout.RowHeaders, alignToRight);
				}
				this.oldRow = rowFromY;
			}
			if (hitTestInfo.type == DataGrid.HitTestType.ParentRows && this.parentRows != null)
			{
				this.parentRows.OnMouseMove(e.X, e.Y);
			}
			if (hitTestInfo.type == DataGrid.HitTestType.Caption)
			{
				this.gridState[262144] = true;
				Rectangle rectangle = this.layout.Caption;
				this.caption.MouseOver(e.X - rectangle.X, e.Y - rectangle.Y);
				return;
			}
			if (this.gridState[262144])
			{
				this.gridState[262144] = false;
				this.caption.MouseLeft();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseUp" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains data about the <see cref="M:System.Windows.Forms.Control.OnMouseUp(System.Windows.Forms.MouseEventArgs)" /> event. </param>
		// Token: 0x060011F4 RID: 4596 RVA: 0x00040068 File Offset: 0x0003E268
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			this.gridState[256] = false;
			if (this.listManager == null || this.myGridTable == null)
			{
				return;
			}
			if (this.gridState[8])
			{
				this.ColResizeEnd(e);
			}
			if (this.gridState[16])
			{
				this.RowResizeEnd(e);
			}
			this.gridState[8] = false;
			this.gridState[16] = false;
			DataGrid.HitTestInfo hitTestInfo = this.HitTest(e.X, e.Y);
			if ((hitTestInfo.type & DataGrid.HitTestType.Caption) == DataGrid.HitTestType.Caption)
			{
				this.caption.MouseUp(e.X, e.Y);
			}
			if (hitTestInfo.type == DataGrid.HitTestType.ColumnHeader)
			{
				PropertyDescriptor propertyDescriptor = this.myGridTable.GridColumnStyles[hitTestInfo.col].PropertyDescriptor;
				if (propertyDescriptor == this.trackColumnHeader)
				{
					this.ColumnHeaderClicked(this.trackColumnHeader);
				}
			}
			this.trackColumnHeader = null;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseWheel" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains data about the <see cref="M:System.Windows.Forms.Control.OnMouseUp(System.Windows.Forms.MouseEventArgs)" /> event. </param>
		// Token: 0x060011F5 RID: 4597 RVA: 0x0004015C File Offset: 0x0003E35C
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			if (e is HandledMouseEventArgs)
			{
				if (((HandledMouseEventArgs)e).Handled)
				{
					return;
				}
				((HandledMouseEventArgs)e).Handled = true;
			}
			bool flag = true;
			if ((Control.ModifierKeys & Keys.Control) != Keys.None)
			{
				flag = false;
			}
			if (this.listManager == null || this.myGridTable == null)
			{
				return;
			}
			ScrollBar scrollBar = flag ? this.vertScrollBar : this.horizScrollBar;
			if (!scrollBar.Visible)
			{
				return;
			}
			this.gridState[131072] = true;
			this.wheelDelta += e.Delta;
			float num = (float)this.wheelDelta / 120f;
			int num2 = (int)((float)SystemInformation.MouseWheelScrollLines * num);
			if (num2 != 0)
			{
				this.wheelDelta = 0;
				if (flag)
				{
					int num3 = this.firstVisibleRow - num2;
					num3 = Math.Max(0, Math.Min(num3, this.DataGridRowsLength - this.numTotallyVisibleRows));
					this.ScrollDown(num3 - this.firstVisibleRow);
				}
				else
				{
					int num4 = this.horizScrollBar.Value + ((num2 < 0) ? 1 : -1) * this.horizScrollBar.LargeChange;
					this.HorizontalOffset = num4;
				}
			}
			this.gridState[131072] = false;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.</summary>
		/// <param name="pe">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> which contains data about the event. </param>
		// Token: 0x060011F6 RID: 4598 RVA: 0x0004028C File Offset: 0x0003E48C
		protected override void OnPaint(PaintEventArgs pe)
		{
			try
			{
				this.CheckHierarchyState();
				if (this.layout.dirty)
				{
					this.ComputeLayout();
				}
				Graphics graphics = pe.Graphics;
				Region clip = graphics.Clip;
				if (this.layout.CaptionVisible)
				{
					this.caption.Paint(graphics, this.layout.Caption, this.isRightToLeft());
				}
				if (this.layout.ParentRowsVisible)
				{
					graphics.FillRectangle(SystemBrushes.AppWorkspace, this.layout.ParentRows);
					this.parentRows.Paint(graphics, this.layout.ParentRows, this.isRightToLeft());
				}
				Rectangle rectangle = this.layout.Data;
				if (this.layout.RowHeadersVisible)
				{
					rectangle = Rectangle.Union(rectangle, this.layout.RowHeaders);
				}
				if (this.layout.ColumnHeadersVisible)
				{
					rectangle = Rectangle.Union(rectangle, this.layout.ColumnHeaders);
				}
				graphics.SetClip(rectangle);
				this.PaintGrid(graphics, rectangle);
				graphics.Clip = clip;
				clip.Dispose();
				this.PaintBorder(graphics, this.layout.ClientRectangle);
				graphics.FillRectangle(DataGrid.DefaultHeaderBackBrush, this.layout.ResizeBoxRect);
				base.OnPaint(pe);
			}
			catch
			{
				if (this.Site == null || !this.Site.DesignMode)
				{
					throw;
				}
				this.gridState[8388608] = true;
				try
				{
					RTLAwareMessageBox.Show(null, SR.GetString("DataGridExceptionInPaint"), null, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
					if (base.Visible)
					{
						base.BeginUpdateInternal();
					}
					this.ResetParentRows();
					this.Set_ListManager(null, string.Empty, true);
				}
				finally
				{
					this.gridState[8388608] = false;
					if (base.Visible)
					{
						base.EndUpdateInternal();
					}
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060011F7 RID: 4599 RVA: 0x00040480 File Offset: 0x0003E680
		protected override void OnResize(EventArgs e)
		{
			if (this.layout.CaptionVisible)
			{
				base.Invalidate(this.layout.Caption);
			}
			if (this.layout.ParentRowsVisible)
			{
				this.parentRows.OnResize(this.layout.ParentRows);
			}
			int borderWidth = this.BorderWidth;
			Rectangle clientRectangle = this.layout.ClientRectangle;
			Rectangle rc = new Rectangle(clientRectangle.X + clientRectangle.Width - borderWidth, clientRectangle.Y, borderWidth, clientRectangle.Height);
			Rectangle rc2 = new Rectangle(clientRectangle.X, clientRectangle.Y + clientRectangle.Height - borderWidth, clientRectangle.Width, borderWidth);
			Rectangle clientRectangle2 = base.ClientRectangle;
			if (clientRectangle2.Width != clientRectangle.Width)
			{
				base.Invalidate(rc);
				rc = new Rectangle(clientRectangle2.X + clientRectangle2.Width - borderWidth, clientRectangle2.Y, borderWidth, clientRectangle2.Height);
				base.Invalidate(rc);
			}
			if (clientRectangle2.Height != clientRectangle.Height)
			{
				base.Invalidate(rc2);
				rc2 = new Rectangle(clientRectangle2.X, clientRectangle2.Y + clientRectangle2.Height - borderWidth, clientRectangle2.Width, borderWidth);
				base.Invalidate(rc2);
			}
			if (!this.layout.ResizeBoxRect.IsEmpty)
			{
				base.Invalidate(this.layout.ResizeBoxRect);
			}
			this.layout.ClientRectangle = clientRectangle2;
			int num = this.firstVisibleRow;
			base.OnResize(e);
			if (this.isRightToLeft() || num != this.firstVisibleRow)
			{
				base.Invalidate();
			}
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x00040618 File Offset: 0x0003E818
		internal void OnRowHeightChanged(DataGridRow row)
		{
			this.ClearRegionCache();
			int rowTop = this.GetRowTop(row.RowNumber);
			if (rowTop > 0)
			{
				base.Invalidate(new Rectangle
				{
					Y = rowTop,
					X = this.layout.Inside.X,
					Width = this.layout.Inside.Width,
					Height = this.layout.Inside.Bottom - rowTop
				});
			}
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x0004069C File Offset: 0x0003E89C
		internal void ParentRowsDataChanged()
		{
			this.parentRows.Clear();
			this.caption.BackButtonActive = (this.caption.DownButtonActive = (this.caption.BackButtonVisible = false));
			this.caption.SetDownButtonDirection(!this.layout.ParentRowsVisible);
			object newDataSource = this.originalState.DataSource;
			string newDataMember = this.originalState.DataMember;
			this.originalState = null;
			this.Set_ListManager(newDataSource, newDataMember, true);
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x00040720 File Offset: 0x0003E920
		private void AbortEdit()
		{
			this.gridState[65536] = true;
			this.editColumn.Abort(this.editRow.RowNumber);
			this.gridState[65536] = false;
			this.gridState[32768] = false;
			this.editRow = null;
			this.editColumn = null;
		}

		/// <summary>Occurs when the user navigates to a new table.</summary>
		// Token: 0x140000C0 RID: 192
		// (add) Token: 0x060011FB RID: 4603 RVA: 0x00040784 File Offset: 0x0003E984
		// (remove) Token: 0x060011FC RID: 4604 RVA: 0x0004079D File Offset: 0x0003E99D
		[SRCategory("CatAction")]
		[SRDescription("DataGridNavigateEventDescr")]
		public event NavigateEventHandler Navigate
		{
			add
			{
				this.onNavigate = (NavigateEventHandler)Delegate.Combine(this.onNavigate, value);
			}
			remove
			{
				this.onNavigate = (NavigateEventHandler)Delegate.Remove(this.onNavigate, value);
			}
		}

		/// <summary>Occurs when a row header is clicked.</summary>
		// Token: 0x140000C1 RID: 193
		// (add) Token: 0x060011FD RID: 4605 RVA: 0x000407B6 File Offset: 0x0003E9B6
		// (remove) Token: 0x060011FE RID: 4606 RVA: 0x000407CF File Offset: 0x0003E9CF
		protected event EventHandler RowHeaderClick
		{
			add
			{
				this.onRowHeaderClick = (EventHandler)Delegate.Combine(this.onRowHeaderClick, value);
			}
			remove
			{
				this.onRowHeaderClick = (EventHandler)Delegate.Remove(this.onRowHeaderClick, value);
			}
		}

		// Token: 0x140000C2 RID: 194
		// (add) Token: 0x060011FF RID: 4607 RVA: 0x000407E8 File Offset: 0x0003E9E8
		// (remove) Token: 0x06001200 RID: 4608 RVA: 0x000407FB File Offset: 0x0003E9FB
		[SRCategory("CatAction")]
		[SRDescription("DataGridNodeClickEventDescr")]
		internal event EventHandler NodeClick
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_NODECLICKED, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_NODECLICKED, value);
			}
		}

		/// <summary>Occurs when the user scrolls the <see cref="T:System.Windows.Forms.DataGrid" /> control.</summary>
		// Token: 0x140000C3 RID: 195
		// (add) Token: 0x06001201 RID: 4609 RVA: 0x0004080E File Offset: 0x0003EA0E
		// (remove) Token: 0x06001202 RID: 4610 RVA: 0x00040821 File Offset: 0x0003EA21
		[SRCategory("CatAction")]
		[SRDescription("DataGridScrollEventDescr")]
		public event EventHandler Scroll
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_SCROLL, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_SCROLL, value);
			}
		}

		/// <summary>Gets or sets the site of the control.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.ISite" /> associated with the Control, if any.</returns>
		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06001203 RID: 4611 RVA: 0x00040834 File Offset: 0x0003EA34
		// (set) Token: 0x06001204 RID: 4612 RVA: 0x0004083C File Offset: 0x0003EA3C
		public override ISite Site
		{
			get
			{
				return base.Site;
			}
			set
			{
				ISite site = this.Site;
				base.Site = value;
				if (value != site && !base.Disposing)
				{
					this.SubObjectsSiteChange(false);
					this.SubObjectsSiteChange(true);
				}
			}
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x00040874 File Offset: 0x0003EA74
		internal void AddNewRow()
		{
			this.EnsureBound();
			this.ResetSelection();
			this.UpdateListManager();
			this.gridState[512] = true;
			this.gridState[1048576] = true;
			try
			{
				this.ListManager.AddNew();
			}
			catch
			{
				this.gridState[512] = false;
				this.gridState[1048576] = false;
				base.PerformLayout();
				this.InvalidateInside();
				throw;
			}
			this.gridState[512] = false;
		}

		/// <summary>Attempts to put the grid into a state where editing is allowed.</summary>
		/// <param name="gridColumn">A <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> to edit. </param>
		/// <param name="rowNumber">The number of the row to edit. </param>
		/// <returns>
		///     <see langword="true" /> if the method is successful; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001206 RID: 4614 RVA: 0x00040914 File Offset: 0x0003EB14
		public bool BeginEdit(DataGridColumnStyle gridColumn, int rowNumber)
		{
			if (this.DataSource == null || this.myGridTable == null)
			{
				return false;
			}
			if (this.gridState[32768])
			{
				return false;
			}
			int c;
			if ((c = this.myGridTable.GridColumnStyles.IndexOf(gridColumn)) < 0)
			{
				return false;
			}
			this.CurrentCell = new DataGridCell(rowNumber, c);
			this.ResetSelection();
			this.Edit();
			return true;
		}

		/// <summary>Begins the initialization of a <see cref="T:System.Windows.Forms.DataGrid" /> that is used on a form or used by another component. The initialization occurs at run time.</summary>
		// Token: 0x06001207 RID: 4615 RVA: 0x0004097B File Offset: 0x0003EB7B
		public void BeginInit()
		{
			if (this.inInit)
			{
				throw new InvalidOperationException(SR.GetString("DataGridBeginInit"));
			}
			this.inInit = true;
		}

		// Token: 0x06001208 RID: 4616 RVA: 0x0004099C File Offset: 0x0003EB9C
		private Rectangle CalcRowResizeFeedbackRect(MouseEventArgs e)
		{
			Rectangle data = this.layout.Data;
			Rectangle result = new Rectangle(data.X, e.Y, data.Width, 3);
			result.Y = Math.Min(data.Bottom - 3, result.Y);
			result.Y = Math.Max(result.Y, 0);
			return result;
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x00040A04 File Offset: 0x0003EC04
		private Rectangle CalcColResizeFeedbackRect(MouseEventArgs e)
		{
			Rectangle data = this.layout.Data;
			Rectangle result = new Rectangle(e.X, data.Y, 3, data.Height);
			result.X = Math.Min(data.Right - 3, result.X);
			result.X = Math.Max(result.X, 0);
			return result;
		}

		// Token: 0x0600120A RID: 4618 RVA: 0x00040A6A File Offset: 0x0003EC6A
		private void CancelCursorUpdate()
		{
			if (this.listManager != null)
			{
				this.EndEdit();
				this.listManager.CancelCurrentEdit();
			}
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x00040A88 File Offset: 0x0003EC88
		private void CheckHierarchyState()
		{
			if (this.checkHierarchy && this.listManager != null && this.myGridTable != null)
			{
				if (this.myGridTable == null)
				{
					return;
				}
				for (int i = 0; i < this.myGridTable.GridColumnStyles.Count; i++)
				{
					DataGridColumnStyle dataGridColumnStyle = this.myGridTable.GridColumnStyles[i];
				}
				this.checkHierarchy = false;
			}
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x00040AEA File Offset: 0x0003ECEA
		private void ClearRegionCache()
		{
			this.cachedScrollableRegion = null;
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x00040AF4 File Offset: 0x0003ECF4
		private void ColAutoResize(int col)
		{
			this.EndEdit();
			CurrencyManager currencyManager = this.listManager;
			if (currencyManager == null)
			{
				return;
			}
			Graphics graphics = base.CreateGraphicsInternal();
			try
			{
				DataGridColumnStyle dataGridColumnStyle = this.myGridTable.GridColumnStyles[col];
				string headerText = dataGridColumnStyle.HeaderText;
				Font font;
				if (this.myGridTable.IsDefault)
				{
					font = this.HeaderFont;
				}
				else
				{
					font = this.myGridTable.HeaderFont;
				}
				int num = (int)graphics.MeasureString(headerText, font).Width + this.layout.ColumnHeaders.Height + 1;
				int count = currencyManager.Count;
				for (int i = 0; i < count; i++)
				{
					object columnValueAtRow = dataGridColumnStyle.GetColumnValueAtRow(currencyManager, i);
					int width = dataGridColumnStyle.GetPreferredSize(graphics, columnValueAtRow).Width;
					if (width > num)
					{
						num = width;
					}
				}
				if (dataGridColumnStyle.Width != num)
				{
					dataGridColumnStyle.width = num;
					this.ComputeVisibleColumns();
					bool flag = true;
					if (this.lastTotallyVisibleCol != -1)
					{
						for (int j = this.lastTotallyVisibleCol + 1; j < this.myGridTable.GridColumnStyles.Count; j++)
						{
							if (this.myGridTable.GridColumnStyles[j].PropertyDescriptor != null)
							{
								flag = false;
								break;
							}
						}
					}
					else
					{
						flag = false;
					}
					if (flag && (this.negOffset != 0 || this.horizontalOffset != 0))
					{
						dataGridColumnStyle.width = num;
						int num2 = 0;
						int count2 = this.myGridTable.GridColumnStyles.Count;
						int width2 = this.layout.Data.Width;
						GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
						this.negOffset = 0;
						this.horizontalOffset = 0;
						this.firstVisibleCol = 0;
						for (int k = count2 - 1; k >= 0; k--)
						{
							if (gridColumnStyles[k].PropertyDescriptor != null)
							{
								num2 += gridColumnStyles[k].Width;
								if (num2 > width2)
								{
									if (this.negOffset == 0)
									{
										this.firstVisibleCol = k;
										this.negOffset = num2 - width2;
										this.horizontalOffset = this.negOffset;
										this.numVisibleCols++;
									}
									else
									{
										this.horizontalOffset += gridColumnStyles[k].Width;
									}
								}
								else
								{
									this.numVisibleCols++;
								}
							}
						}
						base.PerformLayout();
						base.Invalidate(Rectangle.Union(this.layout.Data, this.layout.ColumnHeaders));
					}
					else
					{
						base.PerformLayout();
						Rectangle rectangle = this.layout.Data;
						if (this.layout.ColumnHeadersVisible)
						{
							rectangle = Rectangle.Union(rectangle, this.layout.ColumnHeaders);
						}
						int colBeg = this.GetColBeg(col);
						if (!this.isRightToLeft())
						{
							rectangle.Width -= colBeg - rectangle.X;
							rectangle.X = colBeg;
						}
						else
						{
							rectangle.Width -= colBeg;
						}
						base.Invalidate(rectangle);
					}
				}
			}
			finally
			{
				graphics.Dispose();
			}
			if (this.horizScrollBar.Visible)
			{
				this.horizScrollBar.Value = this.HorizontalOffset;
			}
		}

		/// <summary>Collapses child relations, if any exist for all rows, or for a specified row.</summary>
		/// <param name="row">The number of the row to collapse. If set to -1, all rows are collapsed. </param>
		// Token: 0x0600120E RID: 4622 RVA: 0x00040E2C File Offset: 0x0003F02C
		public void Collapse(int row)
		{
			this.SetRowExpansionState(row, false);
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x00040E38 File Offset: 0x0003F038
		private void ColResizeBegin(MouseEventArgs e, int col)
		{
			int x = e.X;
			this.EndEdit();
			Rectangle r = Rectangle.Union(this.layout.ColumnHeaders, this.layout.Data);
			if (this.isRightToLeft())
			{
				r.Width = this.GetColBeg(col) - this.layout.Data.X - 2;
			}
			else
			{
				int colBeg = this.GetColBeg(col);
				r.X = colBeg + 3;
				r.Width = this.layout.Data.X + this.layout.Data.Width - colBeg - 2;
			}
			base.CaptureInternal = true;
			Cursor.ClipInternal = base.RectangleToScreen(r);
			this.gridState[8] = true;
			this.trackColAnchor = x;
			this.trackColumn = col;
			this.DrawColSplitBar(e);
			this.lastSplitBar = e;
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x00040F13 File Offset: 0x0003F113
		private void ColResizeMove(MouseEventArgs e)
		{
			if (this.lastSplitBar != null)
			{
				this.DrawColSplitBar(this.lastSplitBar);
				this.lastSplitBar = e;
			}
			this.DrawColSplitBar(e);
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x00040F38 File Offset: 0x0003F138
		private void ColResizeEnd(MouseEventArgs e)
		{
			this.gridState[16777216] = true;
			try
			{
				if (this.lastSplitBar != null)
				{
					this.DrawColSplitBar(this.lastSplitBar);
					this.lastSplitBar = null;
				}
				bool flag = this.isRightToLeft();
				int num = flag ? Math.Max(e.X, this.layout.Data.X) : Math.Min(e.X, this.layout.Data.Right + 1);
				int num2 = num - this.GetColEnd(this.trackColumn);
				if (flag)
				{
					num2 = -num2;
				}
				if (this.trackColAnchor != num && num2 != 0)
				{
					DataGridColumnStyle dataGridColumnStyle = this.myGridTable.GridColumnStyles[this.trackColumn];
					int num3 = dataGridColumnStyle.Width + num2;
					num3 = Math.Max(num3, 3);
					dataGridColumnStyle.Width = num3;
					this.ComputeVisibleColumns();
					bool flag2 = true;
					for (int i = this.lastTotallyVisibleCol + 1; i < this.myGridTable.GridColumnStyles.Count; i++)
					{
						if (this.myGridTable.GridColumnStyles[i].PropertyDescriptor != null)
						{
							flag2 = false;
							break;
						}
					}
					if (flag2 && (this.negOffset != 0 || this.horizontalOffset != 0))
					{
						int num4 = 0;
						int count = this.myGridTable.GridColumnStyles.Count;
						int width = this.layout.Data.Width;
						GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
						this.negOffset = 0;
						this.horizontalOffset = 0;
						this.firstVisibleCol = 0;
						for (int j = count - 1; j > -1; j--)
						{
							if (gridColumnStyles[j].PropertyDescriptor != null)
							{
								num4 += gridColumnStyles[j].Width;
								if (num4 > width)
								{
									if (this.negOffset == 0)
									{
										this.negOffset = num4 - width;
										this.firstVisibleCol = j;
										this.horizontalOffset = this.negOffset;
										this.numVisibleCols++;
									}
									else
									{
										this.horizontalOffset += gridColumnStyles[j].Width;
									}
								}
								else
								{
									this.numVisibleCols++;
								}
							}
						}
						base.Invalidate(Rectangle.Union(this.layout.Data, this.layout.ColumnHeaders));
					}
					else
					{
						Rectangle rc = Rectangle.Union(this.layout.ColumnHeaders, this.layout.Data);
						int colBeg = this.GetColBeg(this.trackColumn);
						rc.Width -= (flag ? (rc.Right - colBeg) : (colBeg - rc.X));
						rc.X = (flag ? this.layout.Data.X : colBeg);
						base.Invalidate(rc);
					}
				}
			}
			finally
			{
				Cursor.ClipInternal = Rectangle.Empty;
				base.CaptureInternal = false;
				this.gridState[16777216] = false;
			}
			base.PerformLayout();
			if (this.horizScrollBar.Visible)
			{
				this.horizScrollBar.Value = this.HorizontalOffset;
			}
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x00041268 File Offset: 0x0003F468
		private void MetaDataChanged()
		{
			this.parentRows.Clear();
			this.caption.BackButtonActive = (this.caption.DownButtonActive = (this.caption.BackButtonVisible = false));
			this.caption.SetDownButtonDirection(!this.layout.ParentRowsVisible);
			this.gridState[4194304] = true;
			try
			{
				if (this.originalState != null)
				{
					this.Set_ListManager(this.originalState.DataSource, this.originalState.DataMember, true);
					this.originalState = null;
				}
				else
				{
					this.Set_ListManager(this.DataSource, this.DataMember, true);
				}
			}
			finally
			{
				this.gridState[4194304] = false;
			}
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x00041338 File Offset: 0x0003F538
		private void RowAutoResize(int row)
		{
			this.EndEdit();
			CurrencyManager currencyManager = this.ListManager;
			if (currencyManager == null)
			{
				return;
			}
			Graphics graphics = base.CreateGraphicsInternal();
			try
			{
				GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
				DataGridRow dataGridRow = this.DataGridRows[row];
				int count = currencyManager.Count;
				int num = 0;
				int count2 = gridColumnStyles.Count;
				for (int i = 0; i < count2; i++)
				{
					object columnValueAtRow = gridColumnStyles[i].GetColumnValueAtRow(currencyManager, row);
					num = Math.Max(num, gridColumnStyles[i].GetPreferredHeight(graphics, columnValueAtRow));
				}
				if (dataGridRow.Height != num)
				{
					dataGridRow.Height = num;
					base.PerformLayout();
					Rectangle rectangle = this.layout.Data;
					if (this.layout.RowHeadersVisible)
					{
						rectangle = Rectangle.Union(rectangle, this.layout.RowHeaders);
					}
					int rowTop = this.GetRowTop(row);
					rectangle.Height -= rectangle.Y - rowTop;
					rectangle.Y = rowTop;
					base.Invalidate(rectangle);
				}
			}
			finally
			{
				graphics.Dispose();
			}
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x00041454 File Offset: 0x0003F654
		private void RowResizeBegin(MouseEventArgs e, int row)
		{
			int y = e.Y;
			this.EndEdit();
			Rectangle r = Rectangle.Union(this.layout.RowHeaders, this.layout.Data);
			int rowTop = this.GetRowTop(row);
			r.Y = rowTop + 3;
			r.Height = this.layout.Data.Y + this.layout.Data.Height - rowTop - 2;
			base.CaptureInternal = true;
			Cursor.ClipInternal = base.RectangleToScreen(r);
			this.gridState[16] = true;
			this.trackRowAnchor = y;
			this.trackRow = row;
			this.DrawRowSplitBar(e);
			this.lastSplitBar = e;
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x00041505 File Offset: 0x0003F705
		private void RowResizeMove(MouseEventArgs e)
		{
			if (this.lastSplitBar != null)
			{
				this.DrawRowSplitBar(this.lastSplitBar);
				this.lastSplitBar = e;
			}
			this.DrawRowSplitBar(e);
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x0004152C File Offset: 0x0003F72C
		private void RowResizeEnd(MouseEventArgs e)
		{
			try
			{
				if (this.lastSplitBar != null)
				{
					this.DrawRowSplitBar(this.lastSplitBar);
					this.lastSplitBar = null;
				}
				int num = Math.Min(e.Y, this.layout.Data.Y + this.layout.Data.Height + 1);
				int num2 = num - this.GetRowBottom(this.trackRow);
				if (this.trackRowAnchor != num && num2 != 0)
				{
					DataGridRow dataGridRow = this.DataGridRows[this.trackRow];
					int num3 = dataGridRow.Height + num2;
					num3 = Math.Max(num3, 3);
					dataGridRow.Height = num3;
					base.PerformLayout();
					Rectangle rc = Rectangle.Union(this.layout.RowHeaders, this.layout.Data);
					int rowTop = this.GetRowTop(this.trackRow);
					rc.Height -= rc.Y - rowTop;
					rc.Y = rowTop;
					base.Invalidate(rc);
				}
			}
			finally
			{
				Cursor.ClipInternal = Rectangle.Empty;
				base.CaptureInternal = false;
			}
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x00041648 File Offset: 0x0003F848
		private void ColumnHeaderClicked(PropertyDescriptor prop)
		{
			if (!this.CommitEdit())
			{
				return;
			}
			bool allowSorting;
			if (this.myGridTable.IsDefault)
			{
				allowSorting = this.AllowSorting;
			}
			else
			{
				allowSorting = this.myGridTable.AllowSorting;
			}
			if (!allowSorting)
			{
				return;
			}
			ListSortDirection listSortDirection = this.ListManager.GetSortDirection();
			PropertyDescriptor sortProperty = this.ListManager.GetSortProperty();
			if (sortProperty != null && sortProperty.Equals(prop))
			{
				listSortDirection = ((listSortDirection == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
			}
			else
			{
				listSortDirection = ListSortDirection.Ascending;
			}
			if (this.listManager.Count == 0)
			{
				return;
			}
			this.ListManager.SetSort(prop, listSortDirection);
			this.ResetSelection();
			this.InvalidateInside();
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x000416DC File Offset: 0x0003F8DC
		private bool CommitEdit()
		{
			if ((!this.gridState[32768] && !this.gridState[16384]) || (this.gridState[65536] && !this.gridState[131072]))
			{
				return true;
			}
			this.gridState[65536] = true;
			if (this.editColumn.ReadOnly || this.gridState[1048576])
			{
				bool flag = false;
				if (base.ContainsFocus)
				{
					flag = true;
				}
				if (flag && this.gridState[2048])
				{
					this.FocusInternal();
				}
				this.editColumn.ConcedeFocus();
				if (flag && this.gridState[2048] && base.CanFocus && !this.Focused)
				{
					this.FocusInternal();
				}
				this.gridState[65536] = false;
				return true;
			}
			bool flag2 = this.editColumn.Commit(this.ListManager, this.currentRow);
			this.gridState[65536] = false;
			if (flag2)
			{
				this.gridState[32768] = false;
			}
			return flag2;
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x00041814 File Offset: 0x0003FA14
		private int ComputeDeltaRows(int targetRow)
		{
			if (this.firstVisibleRow == targetRow)
			{
				return 0;
			}
			int num = -1;
			int num2 = -1;
			int num3 = this.DataGridRowsLength;
			int num4 = 0;
			DataGridRow[] array = this.DataGridRows;
			for (int i = 0; i < num3; i++)
			{
				if (i == this.firstVisibleRow)
				{
					num = num4;
				}
				if (i == targetRow)
				{
					num2 = num4;
				}
				if (num2 != -1 && num != -1)
				{
					break;
				}
				num4 += array[i].Height;
			}
			int num5 = num2 + array[targetRow].Height;
			int num6 = this.layout.Data.Height + num;
			if (num5 > num6)
			{
				int num7 = num5 - num6;
				num += num7;
			}
			else
			{
				if (num < num2)
				{
					return 0;
				}
				int num8 = num - num2;
				num -= num8;
			}
			int num9 = this.ComputeFirstVisibleRow(num);
			return num9 - this.firstVisibleRow;
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x000418DC File Offset: 0x0003FADC
		private int ComputeFirstVisibleRow(int firstVisibleRowLogicalTop)
		{
			int num = this.DataGridRowsLength;
			int num2 = 0;
			DataGridRow[] array = this.DataGridRows;
			int num3 = 0;
			while (num3 < num && num2 < firstVisibleRowLogicalTop)
			{
				num2 += array[num3].Height;
				num3++;
			}
			return num3;
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x00041918 File Offset: 0x0003FB18
		private void ComputeLayout()
		{
			bool flag = !this.isRightToLeft();
			Rectangle resizeBoxRect = this.layout.ResizeBoxRect;
			this.EndEdit();
			this.ClearRegionCache();
			DataGrid.LayoutData layoutData = new DataGrid.LayoutData(this.layout);
			layoutData.Inside = base.ClientRectangle;
			Rectangle inside = layoutData.Inside;
			int borderWidth = this.BorderWidth;
			inside.Inflate(-borderWidth, -borderWidth);
			Rectangle rectangle = inside;
			if (this.layout.CaptionVisible)
			{
				int num = this.captionFontHeight + 6;
				Rectangle rectangle2 = layoutData.Caption;
				rectangle2 = rectangle;
				rectangle2.Height = num;
				rectangle.Y += num;
				rectangle.Height -= num;
				layoutData.Caption = rectangle2;
			}
			else
			{
				layoutData.Caption = Rectangle.Empty;
			}
			if (this.layout.ParentRowsVisible)
			{
				Rectangle rectangle3 = layoutData.ParentRows;
				int height = this.parentRows.Height;
				rectangle3 = rectangle;
				rectangle3.Height = height;
				rectangle.Y += height;
				rectangle.Height -= height;
				layoutData.ParentRows = rectangle3;
			}
			else
			{
				layoutData.ParentRows = Rectangle.Empty;
			}
			int num2 = this.headerFontHeight + 6;
			if (this.layout.ColumnHeadersVisible)
			{
				Rectangle columnHeaders = layoutData.ColumnHeaders;
				columnHeaders = rectangle;
				columnHeaders.Height = num2;
				rectangle.Y += num2;
				rectangle.Height -= num2;
				layoutData.ColumnHeaders = columnHeaders;
			}
			else
			{
				layoutData.ColumnHeaders = Rectangle.Empty;
			}
			bool flag2 = this.myGridTable.IsDefault ? this.RowHeadersVisible : this.myGridTable.RowHeadersVisible;
			int num3 = this.myGridTable.IsDefault ? this.RowHeaderWidth : this.myGridTable.RowHeaderWidth;
			layoutData.RowHeadersVisible = flag2;
			if (this.myGridTable != null && flag2)
			{
				Rectangle rowHeaders = layoutData.RowHeaders;
				if (flag)
				{
					rowHeaders = rectangle;
					rowHeaders.Width = num3;
					rectangle.X += num3;
					rectangle.Width -= num3;
				}
				else
				{
					rowHeaders = rectangle;
					rowHeaders.Width = num3;
					rowHeaders.X = rectangle.Right - num3;
					rectangle.Width -= num3;
				}
				layoutData.RowHeaders = rowHeaders;
				if (this.layout.ColumnHeadersVisible)
				{
					Rectangle topLeftHeader = layoutData.TopLeftHeader;
					Rectangle columnHeaders2 = layoutData.ColumnHeaders;
					if (flag)
					{
						topLeftHeader = columnHeaders2;
						topLeftHeader.Width = num3;
						columnHeaders2.Width -= num3;
						columnHeaders2.X += num3;
					}
					else
					{
						topLeftHeader = columnHeaders2;
						topLeftHeader.Width = num3;
						topLeftHeader.X = columnHeaders2.Right - num3;
						columnHeaders2.Width -= num3;
					}
					layoutData.TopLeftHeader = topLeftHeader;
					layoutData.ColumnHeaders = columnHeaders2;
				}
				else
				{
					layoutData.TopLeftHeader = Rectangle.Empty;
				}
			}
			else
			{
				layoutData.RowHeaders = Rectangle.Empty;
				layoutData.TopLeftHeader = Rectangle.Empty;
			}
			layoutData.Data = rectangle;
			layoutData.Inside = inside;
			this.layout = layoutData;
			this.LayoutScrollBars();
			if (!resizeBoxRect.Equals(this.layout.ResizeBoxRect) && !this.layout.ResizeBoxRect.IsEmpty)
			{
				base.Invalidate(this.layout.ResizeBoxRect);
			}
			this.layout.dirty = false;
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x00041C90 File Offset: 0x0003FE90
		private int ComputeRowDelta(int from, int to)
		{
			int num = from;
			int num2 = to;
			int num3 = -1;
			if (num > num2)
			{
				num = to;
				num2 = from;
				num3 = 1;
			}
			DataGridRow[] array = this.DataGridRows;
			int num4 = 0;
			for (int i = num; i < num2; i++)
			{
				num4 += array[i].Height;
			}
			return num3 * num4;
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x00041CD9 File Offset: 0x0003FED9
		internal int MinimumRowHeaderWidth()
		{
			return this.minRowHeaderWidth;
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x00041CE4 File Offset: 0x0003FEE4
		internal void ComputeMinimumRowHeaderWidth()
		{
			this.minRowHeaderWidth = 15;
			if (this.ListHasErrors)
			{
				this.minRowHeaderWidth += 15;
			}
			if (this.myGridTable != null && this.myGridTable.RelationsList.Count != 0)
			{
				this.minRowHeaderWidth += 15;
			}
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x00041D3C File Offset: 0x0003FF3C
		private void ComputeVisibleColumns()
		{
			this.EnsureBound();
			GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
			int count = gridColumnStyles.Count;
			int num = -this.negOffset;
			int num2 = 0;
			int width = this.layout.Data.Width;
			int num3 = this.firstVisibleCol;
			if (width < 0 || gridColumnStyles.Count == 0)
			{
				this.numVisibleCols = (this.firstVisibleCol = 0);
				this.lastTotallyVisibleCol = -1;
				return;
			}
			while (num < width && num3 < count)
			{
				if (gridColumnStyles[num3].PropertyDescriptor != null)
				{
					num += gridColumnStyles[num3].Width;
				}
				num3++;
				num2++;
			}
			this.numVisibleCols = num2;
			if (num < width)
			{
				int num4 = this.firstVisibleCol - 1;
				while (num4 > 0 && num + gridColumnStyles[num4].Width <= width)
				{
					if (gridColumnStyles[num4].PropertyDescriptor != null)
					{
						num += gridColumnStyles[num4].Width;
					}
					num2++;
					this.firstVisibleCol--;
					num4--;
				}
				if (this.numVisibleCols != num2)
				{
					base.Invalidate(this.layout.Data);
					base.Invalidate(this.layout.ColumnHeaders);
					this.numVisibleCols = num2;
				}
			}
			this.lastTotallyVisibleCol = this.firstVisibleCol + this.numVisibleCols - 1;
			if (num > width)
			{
				if (this.numVisibleCols <= 1 || (this.numVisibleCols == 2 && this.negOffset != 0))
				{
					this.lastTotallyVisibleCol = -1;
					return;
				}
				this.lastTotallyVisibleCol--;
			}
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x00041EC8 File Offset: 0x000400C8
		private int ComputeFirstVisibleColumn()
		{
			int i = 0;
			if (this.horizontalOffset == 0)
			{
				this.negOffset = 0;
				return 0;
			}
			if (this.myGridTable != null && this.myGridTable.GridColumnStyles != null && this.myGridTable.GridColumnStyles.Count != 0)
			{
				GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
				int num = 0;
				int count = gridColumnStyles.Count;
				if (gridColumnStyles[0].Width == -1)
				{
					this.negOffset = 0;
					return 0;
				}
				for (i = 0; i < count; i++)
				{
					if (gridColumnStyles[i].PropertyDescriptor != null)
					{
						num += gridColumnStyles[i].Width;
					}
					if (num > this.horizontalOffset)
					{
						break;
					}
				}
				if (i == count)
				{
					this.negOffset = 0;
					return 0;
				}
				this.negOffset = gridColumnStyles[i].Width - (num - this.horizontalOffset);
			}
			return i;
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x00041FA0 File Offset: 0x000401A0
		private void ComputeVisibleRows()
		{
			this.EnsureBound();
			Rectangle data = this.layout.Data;
			int height = data.Height;
			int num = 0;
			int num2 = 0;
			DataGridRow[] array = this.DataGridRows;
			int num3 = this.DataGridRowsLength;
			if (height < 0)
			{
				this.numVisibleRows = (this.numTotallyVisibleRows = 0);
				return;
			}
			int num4 = this.firstVisibleRow;
			while (num4 < num3 && num <= height)
			{
				num += array[num4].Height;
				num2++;
				num4++;
			}
			if (num < height)
			{
				for (int i = this.firstVisibleRow - 1; i >= 0; i--)
				{
					int height2 = array[i].Height;
					if (num + height2 > height)
					{
						break;
					}
					num += height2;
					this.firstVisibleRow--;
					num2++;
				}
			}
			this.numVisibleRows = (this.numTotallyVisibleRows = num2);
			if (num > height)
			{
				this.numTotallyVisibleRows--;
			}
		}

		/// <summary>Constructs a new instance of the accessibility object for this control.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Control.ControlAccessibleObject" /> for this control.</returns>
		// Token: 0x06001222 RID: 4642 RVA: 0x00042086 File Offset: 0x00040286
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new DataGrid.DataGridAccessibleObject(this);
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x00042090 File Offset: 0x00040290
		private DataGridState CreateChildState(string relationName, DataGridRow source)
		{
			DataGridState dataGridState = new DataGridState();
			string text;
			if (string.IsNullOrEmpty(this.DataMember))
			{
				text = relationName;
			}
			else
			{
				text = this.DataMember + "." + relationName;
			}
			CurrencyManager currencyManager = (CurrencyManager)this.BindingContext[this.DataSource, text];
			dataGridState.DataSource = this.DataSource;
			dataGridState.DataMember = text;
			dataGridState.ListManager = currencyManager;
			dataGridState.DataGridRows = null;
			dataGridState.DataGridRowsLength = currencyManager.Count + (this.policy.AllowAdd ? 1 : 0);
			return dataGridState;
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x00042120 File Offset: 0x00040320
		private DataGrid.LayoutData CreateInitialLayoutState()
		{
			return new DataGrid.LayoutData
			{
				Inside = default(Rectangle),
				TopLeftHeader = default(Rectangle),
				ColumnHeaders = default(Rectangle),
				RowHeaders = default(Rectangle),
				Data = default(Rectangle),
				Caption = default(Rectangle),
				ParentRows = default(Rectangle),
				ResizeBoxRect = default(Rectangle),
				ColumnHeadersVisible = true,
				RowHeadersVisible = true,
				CaptionVisible = true,
				ParentRowsVisible = true,
				ClientRectangle = base.ClientRectangle
			};
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x000421BC File Offset: 0x000403BC
		private NativeMethods.RECT[] CreateScrollableRegion(Rectangle scroll)
		{
			if (this.cachedScrollableRegion != null)
			{
				return this.cachedScrollableRegion;
			}
			bool rightToLeft = this.isRightToLeft();
			using (Region region = new Region(scroll))
			{
				int num = this.numVisibleRows;
				int num2 = this.layout.Data.Y;
				int x = this.layout.Data.X;
				DataGridRow[] array = this.DataGridRows;
				for (int i = this.firstVisibleRow; i < num; i++)
				{
					int height = array[i].Height;
					Rectangle nonScrollableArea = array[i].GetNonScrollableArea();
					nonScrollableArea.X += x;
					nonScrollableArea.X = this.MirrorRectangle(nonScrollableArea, this.layout.Data, rightToLeft);
					if (!nonScrollableArea.IsEmpty)
					{
						region.Exclude(new Rectangle(nonScrollableArea.X, nonScrollableArea.Y + num2, nonScrollableArea.Width, nonScrollableArea.Height));
					}
					num2 += height;
				}
				using (Graphics graphics = base.CreateGraphicsInternal())
				{
					IntPtr hrgn = region.GetHrgn(graphics);
					if (hrgn != IntPtr.Zero)
					{
						this.cachedScrollableRegion = UnsafeNativeMethods.GetRectsFromRegion(hrgn);
						IntSecurity.ObjectFromWin32Handle.Assert();
						try
						{
							region.ReleaseHrgn(hrgn);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
				}
			}
			return this.cachedScrollableRegion;
		}

		/// <summary>Disposes of the resources (other than memory) used by the <see cref="T:System.Windows.Forms.DataGrid" />.</summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x06001226 RID: 4646 RVA: 0x00042360 File Offset: 0x00040560
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.vertScrollBar != null)
				{
					this.vertScrollBar.Dispose();
				}
				if (this.horizScrollBar != null)
				{
					this.horizScrollBar.Dispose();
				}
				if (this.toBeDisposedEditingControl != null)
				{
					this.toBeDisposedEditingControl.Dispose();
					this.toBeDisposedEditingControl = null;
				}
				GridTableStylesCollection tableStyles = this.TableStyles;
				if (tableStyles != null)
				{
					for (int i = 0; i < tableStyles.Count; i++)
					{
						tableStyles[i].Dispose();
					}
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x000423E0 File Offset: 0x000405E0
		private void DrawColSplitBar(MouseEventArgs e)
		{
			Rectangle r = this.CalcColResizeFeedbackRect(e);
			this.DrawSplitBar(r);
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x000423FC File Offset: 0x000405FC
		private void DrawRowSplitBar(MouseEventArgs e)
		{
			Rectangle r = this.CalcRowResizeFeedbackRect(e);
			this.DrawSplitBar(r);
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x00042418 File Offset: 0x00040618
		private void DrawSplitBar(Rectangle r)
		{
			IntPtr handle = base.Handle;
			IntPtr dcex = UnsafeNativeMethods.GetDCEx(new HandleRef(this, handle), NativeMethods.NullHandleRef, 1026);
			IntPtr handle2 = ControlPaint.CreateHalftoneHBRUSH();
			IntPtr handle3 = SafeNativeMethods.SelectObject(new HandleRef(this, dcex), new HandleRef(null, handle2));
			SafeNativeMethods.PatBlt(new HandleRef(this, dcex), r.X, r.Y, r.Width, r.Height, 5898313);
			SafeNativeMethods.SelectObject(new HandleRef(this, dcex), new HandleRef(null, handle3));
			SafeNativeMethods.DeleteObject(new HandleRef(null, handle2));
			UnsafeNativeMethods.ReleaseDC(new HandleRef(this, handle), new HandleRef(this, dcex));
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x000424C0 File Offset: 0x000406C0
		private void Edit()
		{
			this.Edit(null);
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x000424CC File Offset: 0x000406CC
		private void Edit(string displayText)
		{
			this.EnsureBound();
			bool cellIsVisible = true;
			this.EndEdit();
			DataGridRow[] array = this.DataGridRows;
			if (this.DataGridRowsLength == 0)
			{
				return;
			}
			array[this.currentRow].OnEdit();
			this.editRow = array[this.currentRow];
			if (this.myGridTable.GridColumnStyles.Count == 0)
			{
				return;
			}
			this.editColumn = this.myGridTable.GridColumnStyles[this.currentCol];
			if (this.editColumn.PropertyDescriptor == null)
			{
				return;
			}
			Rectangle bounds = Rectangle.Empty;
			if (this.currentRow < this.firstVisibleRow || this.currentRow > this.firstVisibleRow + this.numVisibleRows || this.currentCol < this.firstVisibleCol || this.currentCol > this.firstVisibleCol + this.numVisibleCols - 1 || (this.currentCol == this.firstVisibleCol && this.negOffset != 0))
			{
				cellIsVisible = false;
			}
			else
			{
				bounds = this.GetCellBounds(this.currentRow, this.currentCol);
			}
			this.gridState[16384] = true;
			this.gridState[32768] = false;
			this.gridState[65536] = true;
			this.editColumn.Edit(this.ListManager, this.currentRow, bounds, this.myGridTable.ReadOnly || this.ReadOnly || !this.policy.AllowEdit, displayText, cellIsVisible);
			this.gridState[65536] = false;
		}

		/// <summary>Requests an end to an edit operation taking place on the <see cref="T:System.Windows.Forms.DataGrid" /> control.</summary>
		/// <param name="gridColumn">The <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> to cease editing. </param>
		/// <param name="rowNumber">The number of the row to cease editing. </param>
		/// <param name="shouldAbort">Set to <see langword="true" /> if the current operation should be stopped. </param>
		/// <returns>
		///     <see langword="true" /> if the editing operation ceases; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600122C RID: 4652 RVA: 0x00042650 File Offset: 0x00040850
		public bool EndEdit(DataGridColumnStyle gridColumn, int rowNumber, bool shouldAbort)
		{
			bool result = false;
			if (this.gridState[32768])
			{
				DataGridColumnStyle dataGridColumnStyle = this.editColumn;
				int rowNumber2 = this.editRow.RowNumber;
				if (shouldAbort)
				{
					this.AbortEdit();
					result = true;
				}
				else
				{
					result = this.CommitEdit();
				}
			}
			return result;
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x0004269D File Offset: 0x0004089D
		private void EndEdit()
		{
			if (!this.gridState[32768] && !this.gridState[16384])
			{
				return;
			}
			if (!this.CommitEdit())
			{
				this.AbortEdit();
			}
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x000426D4 File Offset: 0x000408D4
		private void EnforceValidDataMember(object value)
		{
			if (this.DataMember == null || this.DataMember.Length == 0)
			{
				return;
			}
			if (this.BindingContext == null)
			{
				return;
			}
			try
			{
				BindingManagerBase bindingManagerBase = this.BindingContext[value, this.dataMember];
			}
			catch
			{
				this.dataMember = "";
			}
		}

		/// <summary>Informs the <see cref="T:System.Windows.Forms.DataGrid" /> control when the user begins to edit the column at the specified location.</summary>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that defines the location of the edited column. </param>
		// Token: 0x0600122F RID: 4655 RVA: 0x00042734 File Offset: 0x00040934
		protected internal virtual void ColumnStartedEditing(Rectangle bounds)
		{
			DataGridRow[] array = this.DataGridRows;
			if (bounds.IsEmpty && this.editColumn is DataGridTextBoxColumn && this.currentRow != -1 && this.currentCol != -1)
			{
				DataGridTextBoxColumn dataGridTextBoxColumn = this.editColumn as DataGridTextBoxColumn;
				Rectangle cellBounds = this.GetCellBounds(this.currentRow, this.currentCol);
				this.gridState[65536] = true;
				try
				{
					dataGridTextBoxColumn.TextBox.Bounds = cellBounds;
				}
				finally
				{
					this.gridState[65536] = false;
				}
			}
			if (this.gridState[1048576])
			{
				int num = this.DataGridRowsLength;
				DataGridRow[] array2 = new DataGridRow[num + 1];
				for (int i = 0; i < num; i++)
				{
					array2[i] = array[i];
				}
				array2[num] = new DataGridAddNewRow(this, this.myGridTable, num);
				this.SetDataGridRows(array2, num + 1);
				this.Edit();
				this.gridState[1048576] = false;
				this.gridState[32768] = true;
				this.gridState[16384] = false;
				return;
			}
			this.gridState[32768] = true;
			this.gridState[16384] = false;
			this.InvalidateRowHeader(this.currentRow);
			array[this.currentRow].LoseChildFocus(this.layout.RowHeaders, this.isRightToLeft());
		}

		/// <summary>Informs the <see cref="T:System.Windows.Forms.DataGrid" /> control when the user begins to edit a column using the specified control.</summary>
		/// <param name="editingControl">The <see cref="T:System.Windows.Forms.Control" /> used to edit the column. </param>
		// Token: 0x06001230 RID: 4656 RVA: 0x000428B4 File Offset: 0x00040AB4
		protected internal virtual void ColumnStartedEditing(Control editingControl)
		{
			this.ColumnStartedEditing(editingControl.Bounds);
		}

		/// <summary>Displays child relations, if any exist, for all rows or a specific row.</summary>
		/// <param name="row">The number of the row to expand. If set to -1, all rows are expanded. </param>
		// Token: 0x06001231 RID: 4657 RVA: 0x000428C2 File Offset: 0x00040AC2
		public void Expand(int row)
		{
			this.SetRowExpansionState(row, true);
		}

		/// <summary>Creates a <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> using the specified <see cref="T:System.ComponentModel.PropertyDescriptor" />.</summary>
		/// <param name="prop">The <see cref="T:System.ComponentModel.PropertyDescriptor" /> to use for creating the grid column style. </param>
		/// <param name="isDefault">
		///       <see langword="true" /> to set the column style as the default; otherwise, <see langword="false" />. </param>
		/// <returns>The new <see cref="T:System.Windows.Forms.DataGridColumnStyle" />.</returns>
		// Token: 0x06001232 RID: 4658 RVA: 0x000428CC File Offset: 0x00040ACC
		protected virtual DataGridColumnStyle CreateGridColumn(PropertyDescriptor prop, bool isDefault)
		{
			if (this.myGridTable != null)
			{
				return this.myGridTable.CreateGridColumn(prop, isDefault);
			}
			return null;
		}

		/// <summary>Creates a new <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> with the specified <see cref="T:System.ComponentModel.PropertyDescriptor" />.</summary>
		/// <param name="prop">The <see cref="T:System.ComponentModel.PropertyDescriptor" /> to use for creating the grid column style. </param>
		/// <returns>The new <see cref="T:System.Windows.Forms.DataGridColumnStyle" />.</returns>
		// Token: 0x06001233 RID: 4659 RVA: 0x000428E5 File Offset: 0x00040AE5
		protected virtual DataGridColumnStyle CreateGridColumn(PropertyDescriptor prop)
		{
			if (this.myGridTable != null)
			{
				return this.myGridTable.CreateGridColumn(prop);
			}
			return null;
		}

		/// <summary>Ends the initialization of a <see cref="T:System.Windows.Forms.DataGrid" /> that is used on a form or used by another component. The initialization occurs at run time.</summary>
		// Token: 0x06001234 RID: 4660 RVA: 0x00042900 File Offset: 0x00040B00
		public void EndInit()
		{
			this.inInit = false;
			if (this.myGridTable == null && this.ListManager != null)
			{
				this.SetDataGridTable(this.TableStyles[this.ListManager.GetListName()], true);
			}
			if (this.myGridTable != null)
			{
				this.myGridTable.DataGrid = this;
			}
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x00042958 File Offset: 0x00040B58
		private int GetColFromX(int x)
		{
			if (this.myGridTable == null)
			{
				return -1;
			}
			Rectangle data = this.layout.Data;
			x = this.MirrorPoint(x, data, this.isRightToLeft());
			GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
			int count = gridColumnStyles.Count;
			int num = data.X - this.negOffset;
			int num2 = this.firstVisibleCol;
			while (num < data.Width + data.X && num2 < count)
			{
				if (gridColumnStyles[num2].PropertyDescriptor != null)
				{
					num += gridColumnStyles[num2].Width;
				}
				if (num > x)
				{
					return num2;
				}
				num2++;
			}
			return -1;
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x000429FC File Offset: 0x00040BFC
		internal int GetColBeg(int col)
		{
			int num = this.layout.Data.X - this.negOffset;
			GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
			int num2 = Math.Min(col, gridColumnStyles.Count);
			for (int i = this.firstVisibleCol; i < num2; i++)
			{
				if (gridColumnStyles[i].PropertyDescriptor != null)
				{
					num += gridColumnStyles[i].Width;
				}
			}
			return this.MirrorPoint(num, this.layout.Data, this.isRightToLeft());
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x00042A80 File Offset: 0x00040C80
		internal int GetColEnd(int col)
		{
			int colBeg = this.GetColBeg(col);
			int width = this.myGridTable.GridColumnStyles[col].Width;
			if (!this.isRightToLeft())
			{
				return colBeg + width;
			}
			return colBeg - width;
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x00042ABC File Offset: 0x00040CBC
		private int GetColumnWidthSum()
		{
			int num = 0;
			if (this.myGridTable != null && this.myGridTable.GridColumnStyles != null)
			{
				GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
				int count = gridColumnStyles.Count;
				for (int i = 0; i < count; i++)
				{
					if (gridColumnStyles[i].PropertyDescriptor != null)
					{
						num += gridColumnStyles[i].Width;
					}
				}
			}
			return num;
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x00042B20 File Offset: 0x00040D20
		private DataGridRelationshipRow[] GetExpandableRows()
		{
			int num = this.DataGridRowsLength;
			DataGridRow[] array = this.DataGridRows;
			if (this.policy.AllowAdd)
			{
				num = Math.Max(num - 1, 0);
			}
			DataGridRelationshipRow[] array2 = new DataGridRelationshipRow[num];
			for (int i = 0; i < num; i++)
			{
				array2[i] = (DataGridRelationshipRow)array[i];
			}
			return array2;
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x00042B74 File Offset: 0x00040D74
		private int GetRowFromY(int y)
		{
			Rectangle data = this.layout.Data;
			int num = data.Y;
			int num2 = this.firstVisibleRow;
			int num3 = this.DataGridRowsLength;
			DataGridRow[] array = this.DataGridRows;
			int bottom = data.Bottom;
			while (num < bottom && num2 < num3)
			{
				num += array[num2].Height;
				if (num > y)
				{
					return num2;
				}
				num2++;
			}
			return -1;
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x00042BD6 File Offset: 0x00040DD6
		internal Rectangle GetRowHeaderRect()
		{
			return this.layout.RowHeaders;
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x00042BE3 File Offset: 0x00040DE3
		internal Rectangle GetColumnHeadersRect()
		{
			return this.layout.ColumnHeaders;
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x00042BF0 File Offset: 0x00040DF0
		private Rectangle GetRowRect(int rowNumber)
		{
			Rectangle data = this.layout.Data;
			int num = data.Y;
			DataGridRow[] array = this.DataGridRows;
			int num2 = this.firstVisibleRow;
			while (num2 <= rowNumber && num <= data.Bottom)
			{
				if (num2 == rowNumber)
				{
					Rectangle result = new Rectangle(data.X, num, data.Width, array[num2].Height);
					if (this.layout.RowHeadersVisible)
					{
						result.Width += this.layout.RowHeaders.Width;
						result.X -= (this.isRightToLeft() ? 0 : this.layout.RowHeaders.Width);
					}
					return result;
				}
				num += array[num2].Height;
				num2++;
			}
			return Rectangle.Empty;
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x00042CC8 File Offset: 0x00040EC8
		private int GetRowTop(int row)
		{
			DataGridRow[] array = this.DataGridRows;
			int num = this.layout.Data.Y;
			int num2 = Math.Min(row, this.DataGridRowsLength);
			for (int i = this.firstVisibleRow; i < num2; i++)
			{
				num += array[i].Height;
			}
			for (int j = this.firstVisibleRow; j > num2; j--)
			{
				num -= array[j].Height;
			}
			return num;
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x00042D38 File Offset: 0x00040F38
		private int GetRowBottom(int row)
		{
			DataGridRow[] array = this.DataGridRows;
			return this.GetRowTop(row) + array[row].Height;
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x00042D5C File Offset: 0x00040F5C
		private void EnsureBound()
		{
			if (!this.Bound)
			{
				throw new InvalidOperationException(SR.GetString("DataGridUnbound"));
			}
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x00042D78 File Offset: 0x00040F78
		private void EnsureVisible(int row, int col)
		{
			if (row < this.firstVisibleRow || row >= this.firstVisibleRow + this.numTotallyVisibleRows)
			{
				int rows = this.ComputeDeltaRows(row);
				this.ScrollDown(rows);
			}
			if (this.firstVisibleCol == 0 && this.numVisibleCols == 0 && this.lastTotallyVisibleCol == -1)
			{
				return;
			}
			int num = this.firstVisibleCol;
			int num2 = this.negOffset;
			int num3 = this.lastTotallyVisibleCol;
			while (col < this.firstVisibleCol || (col == this.firstVisibleCol && this.negOffset != 0) || (this.lastTotallyVisibleCol == -1 && col > this.firstVisibleCol) || (this.lastTotallyVisibleCol > -1 && col > this.lastTotallyVisibleCol))
			{
				this.ScrollToColumn(col);
				if (num == this.firstVisibleCol && num2 == this.negOffset && num3 == this.lastTotallyVisibleCol)
				{
					break;
				}
				num = this.firstVisibleCol;
				num2 = this.negOffset;
				num3 = this.lastTotallyVisibleCol;
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Rectangle" /> that specifies the four corners of the selected cell.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that defines the current cell's corners.</returns>
		// Token: 0x06001242 RID: 4674 RVA: 0x00042E54 File Offset: 0x00041054
		public Rectangle GetCurrentCellBounds()
		{
			DataGridCell currentCell = this.CurrentCell;
			return this.GetCellBounds(currentCell.RowNumber, currentCell.ColumnNumber);
		}

		/// <summary>Gets the <see cref="T:System.Drawing.Rectangle" /> of the cell specified by row and column number.</summary>
		/// <param name="row">The number of the cell's row. </param>
		/// <param name="col">The number of the cell's column. </param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that defines the current cell's corners.</returns>
		// Token: 0x06001243 RID: 4675 RVA: 0x00042E7C File Offset: 0x0004107C
		public Rectangle GetCellBounds(int row, int col)
		{
			DataGridRow[] array = this.DataGridRows;
			Rectangle cellBounds = array[row].GetCellBounds(col);
			cellBounds.Y += this.GetRowTop(row);
			cellBounds.X += this.layout.Data.X - this.negOffset;
			cellBounds.X = this.MirrorRectangle(cellBounds, this.layout.Data, this.isRightToLeft());
			return cellBounds;
		}

		/// <summary>Gets the <see cref="T:System.Drawing.Rectangle" /> of the cell specified by <see cref="T:System.Windows.Forms.DataGridCell" />.</summary>
		/// <param name="dgc">The <see cref="T:System.Windows.Forms.DataGridCell" /> to look up. </param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that defines the current cell's corners.</returns>
		// Token: 0x06001244 RID: 4676 RVA: 0x00042EF4 File Offset: 0x000410F4
		public Rectangle GetCellBounds(DataGridCell dgc)
		{
			return this.GetCellBounds(dgc.RowNumber, dgc.ColumnNumber);
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x00042F0C File Offset: 0x0004110C
		internal Rectangle GetRowBounds(DataGridRow row)
		{
			return new Rectangle
			{
				Y = this.GetRowTop(row.RowNumber),
				X = this.layout.Data.X,
				Height = row.Height,
				Width = this.layout.Data.Width
			};
		}

		/// <summary>Gets information, such as row and column number of a clicked point on the grid, using the x and y coordinate passed to the method.</summary>
		/// <param name="x">The horizontal position of the coordinate. </param>
		/// <param name="y">The vertical position of the coordinate. </param>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGrid.HitTestInfo" /> that contains information about the clicked part of the grid.</returns>
		// Token: 0x06001246 RID: 4678 RVA: 0x00042F70 File Offset: 0x00041170
		public DataGrid.HitTestInfo HitTest(int x, int y)
		{
			int y2 = this.layout.Data.Y;
			DataGrid.HitTestInfo hitTestInfo = new DataGrid.HitTestInfo();
			if (this.layout.CaptionVisible && this.layout.Caption.Contains(x, y))
			{
				hitTestInfo.type = DataGrid.HitTestType.Caption;
				return hitTestInfo;
			}
			if (this.layout.ParentRowsVisible && this.layout.ParentRows.Contains(x, y))
			{
				hitTestInfo.type = DataGrid.HitTestType.ParentRows;
				return hitTestInfo;
			}
			if (!this.layout.Inside.Contains(x, y))
			{
				return hitTestInfo;
			}
			if (this.layout.TopLeftHeader.Contains(x, y))
			{
				return hitTestInfo;
			}
			if (this.layout.ColumnHeaders.Contains(x, y))
			{
				hitTestInfo.type = DataGrid.HitTestType.ColumnHeader;
				hitTestInfo.col = this.GetColFromX(x);
				if (hitTestInfo.col < 0)
				{
					return DataGrid.HitTestInfo.Nowhere;
				}
				int colBeg = this.GetColBeg(hitTestInfo.col + 1);
				bool flag = this.isRightToLeft();
				if ((flag && x - colBeg < 8) || (!flag && colBeg - x < 8))
				{
					hitTestInfo.type = DataGrid.HitTestType.ColumnResize;
				}
				if (!this.allowColumnResize)
				{
					return DataGrid.HitTestInfo.Nowhere;
				}
				return hitTestInfo;
			}
			else if (this.layout.RowHeaders.Contains(x, y))
			{
				hitTestInfo.type = DataGrid.HitTestType.RowHeader;
				hitTestInfo.row = this.GetRowFromY(y);
				if (hitTestInfo.row < 0)
				{
					return DataGrid.HitTestInfo.Nowhere;
				}
				DataGridRow[] array = this.DataGridRows;
				int num = this.GetRowTop(hitTestInfo.row) + array[hitTestInfo.row].Height;
				if (num - y - this.BorderWidth < 2 && !(array[hitTestInfo.row] is DataGridAddNewRow))
				{
					hitTestInfo.type = DataGrid.HitTestType.RowResize;
				}
				if (!this.allowRowResize)
				{
					return DataGrid.HitTestInfo.Nowhere;
				}
				return hitTestInfo;
			}
			else
			{
				if (!this.layout.Data.Contains(x, y))
				{
					return hitTestInfo;
				}
				hitTestInfo.type = DataGrid.HitTestType.Cell;
				hitTestInfo.col = this.GetColFromX(x);
				hitTestInfo.row = this.GetRowFromY(y);
				if (hitTestInfo.col < 0 || hitTestInfo.row < 0)
				{
					return DataGrid.HitTestInfo.Nowhere;
				}
				return hitTestInfo;
			}
		}

		/// <summary>Gets information, such as row and column number of a clicked point on the grid, about the grid using a specific <see cref="T:System.Drawing.Point" />.</summary>
		/// <param name="position">A <see cref="T:System.Drawing.Point" /> that represents single x,y coordinate. </param>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGrid.HitTestInfo" /> that contains specific information about the grid.</returns>
		// Token: 0x06001247 RID: 4679 RVA: 0x0004316E File Offset: 0x0004136E
		public DataGrid.HitTestInfo HitTest(Point position)
		{
			return this.HitTest(position.X, position.Y);
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x00043184 File Offset: 0x00041384
		private void InitializeColumnWidths()
		{
			if (this.myGridTable == null)
			{
				return;
			}
			GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
			int count = gridColumnStyles.Count;
			int width = this.myGridTable.IsDefault ? this.PreferredColumnWidth : this.myGridTable.PreferredColumnWidth;
			for (int i = 0; i < count; i++)
			{
				if (gridColumnStyles[i].width == -1)
				{
					gridColumnStyles[i].width = width;
				}
			}
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x000431F6 File Offset: 0x000413F6
		internal void InvalidateInside()
		{
			base.Invalidate(this.layout.Inside);
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x00043209 File Offset: 0x00041409
		internal void InvalidateCaption()
		{
			if (this.layout.CaptionVisible)
			{
				base.Invalidate(this.layout.Caption);
			}
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x00043229 File Offset: 0x00041429
		internal void InvalidateCaptionRect(Rectangle r)
		{
			if (this.layout.CaptionVisible)
			{
				base.Invalidate(r);
			}
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x00043240 File Offset: 0x00041440
		internal void InvalidateColumn(int column)
		{
			GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
			if (column < 0 || gridColumnStyles == null || gridColumnStyles.Count <= column)
			{
				return;
			}
			if (column < this.firstVisibleCol || column > this.firstVisibleCol + this.numVisibleCols - 1)
			{
				return;
			}
			Rectangle rectangle = default(Rectangle);
			rectangle.Height = this.layout.Data.Height;
			rectangle.Width = gridColumnStyles[column].Width;
			rectangle.Y = this.layout.Data.Y;
			int num = this.layout.Data.X - this.negOffset;
			int count = gridColumnStyles.Count;
			int num2 = this.firstVisibleCol;
			while (num2 < count && num2 != column)
			{
				num += gridColumnStyles[num2].Width;
				num2++;
			}
			rectangle.X = num;
			rectangle.X = this.MirrorRectangle(rectangle, this.layout.Data, this.isRightToLeft());
			base.Invalidate(rectangle);
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x00043346 File Offset: 0x00041546
		internal void InvalidateParentRows()
		{
			if (this.layout.ParentRowsVisible)
			{
				base.Invalidate(this.layout.ParentRows);
			}
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x00043368 File Offset: 0x00041568
		internal void InvalidateParentRowsRect(Rectangle r)
		{
			Rectangle rectangle = this.layout.ParentRows;
			base.Invalidate(r);
			bool isEmpty = rectangle.IsEmpty;
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x00043390 File Offset: 0x00041590
		internal void InvalidateRow(int rowNumber)
		{
			Rectangle rowRect = this.GetRowRect(rowNumber);
			if (!rowRect.IsEmpty)
			{
				base.Invalidate(rowRect);
			}
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x000433B8 File Offset: 0x000415B8
		private void InvalidateRowHeader(int rowNumber)
		{
			if (rowNumber >= this.firstVisibleRow && rowNumber < this.firstVisibleRow + this.numVisibleRows)
			{
				if (!this.layout.RowHeadersVisible)
				{
					return;
				}
				base.Invalidate(new Rectangle
				{
					Y = this.GetRowTop(rowNumber),
					X = this.layout.RowHeaders.X,
					Width = this.layout.RowHeaders.Width,
					Height = this.DataGridRows[rowNumber].Height
				});
			}
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x0004344C File Offset: 0x0004164C
		internal void InvalidateRowRect(int rowNumber, Rectangle r)
		{
			Rectangle rowRect = this.GetRowRect(rowNumber);
			if (!rowRect.IsEmpty)
			{
				Rectangle rc = new Rectangle(rowRect.X + r.X, rowRect.Y + r.Y, r.Width, r.Height);
				if (this.vertScrollBar.Visible && this.isRightToLeft())
				{
					rc.X -= this.vertScrollBar.Width;
				}
				base.Invalidate(rc);
			}
		}

		/// <summary>Gets a value that indicates whether the node of a specified row is expanded or collapsed.</summary>
		/// <param name="rowNumber">The number of the row in question. </param>
		/// <returns>
		///     <see langword="true" /> if the node is expanded; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001252 RID: 4690 RVA: 0x000434D4 File Offset: 0x000416D4
		public bool IsExpanded(int rowNumber)
		{
			if (rowNumber < 0 || rowNumber > this.DataGridRowsLength)
			{
				throw new ArgumentOutOfRangeException("rowNumber");
			}
			DataGridRow[] array = this.DataGridRows;
			DataGridRow dataGridRow = array[rowNumber];
			if (dataGridRow is DataGridRelationshipRow)
			{
				DataGridRelationshipRow dataGridRelationshipRow = (DataGridRelationshipRow)dataGridRow;
				return dataGridRelationshipRow.Expanded;
			}
			return false;
		}

		/// <summary>Gets a value indicating whether a specified row is selected.</summary>
		/// <param name="row">The number of the row you are interested in. </param>
		/// <returns>
		///     <see langword="true" /> if the row is selected; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001253 RID: 4691 RVA: 0x0004351C File Offset: 0x0004171C
		public bool IsSelected(int row)
		{
			DataGridRow[] array = this.DataGridRows;
			return array[row].Selected;
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x00043538 File Offset: 0x00041738
		internal static bool IsTransparentColor(Color color)
		{
			return color.A < byte.MaxValue;
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x00043548 File Offset: 0x00041748
		private void LayoutScrollBars()
		{
			if (this.listManager == null || this.myGridTable == null)
			{
				this.horizScrollBar.Visible = false;
				this.vertScrollBar.Visible = false;
				return;
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = this.isRightToLeft();
			int count = this.myGridTable.GridColumnStyles.Count;
			DataGridRow[] array = this.DataGridRows;
			int num = Math.Max(0, this.GetColumnWidthSum());
			if (num > this.layout.Data.Width && !flag)
			{
				int height = this.horizScrollBar.Height;
				DataGrid.LayoutData layoutData = this.layout;
				layoutData.Data.Height = layoutData.Data.Height - height;
				if (this.layout.RowHeadersVisible)
				{
					DataGrid.LayoutData layoutData2 = this.layout;
					layoutData2.RowHeaders.Height = layoutData2.RowHeaders.Height - height;
				}
				flag = true;
			}
			int num2 = this.firstVisibleRow;
			this.ComputeVisibleRows();
			if (this.numTotallyVisibleRows != this.DataGridRowsLength && !flag2)
			{
				int width = this.vertScrollBar.Width;
				DataGrid.LayoutData layoutData3 = this.layout;
				layoutData3.Data.Width = layoutData3.Data.Width - width;
				if (this.layout.ColumnHeadersVisible)
				{
					if (flag4)
					{
						DataGrid.LayoutData layoutData4 = this.layout;
						layoutData4.ColumnHeaders.X = layoutData4.ColumnHeaders.X + width;
					}
					DataGrid.LayoutData layoutData5 = this.layout;
					layoutData5.ColumnHeaders.Width = layoutData5.ColumnHeaders.Width - width;
				}
				flag2 = true;
			}
			this.firstVisibleCol = this.ComputeFirstVisibleColumn();
			this.ComputeVisibleColumns();
			if (flag2 && num > this.layout.Data.Width && !flag)
			{
				this.firstVisibleRow = num2;
				int height2 = this.horizScrollBar.Height;
				DataGrid.LayoutData layoutData6 = this.layout;
				layoutData6.Data.Height = layoutData6.Data.Height - height2;
				if (this.layout.RowHeadersVisible)
				{
					DataGrid.LayoutData layoutData7 = this.layout;
					layoutData7.RowHeaders.Height = layoutData7.RowHeaders.Height - height2;
				}
				flag = true;
				flag3 = true;
			}
			if (flag3)
			{
				this.ComputeVisibleRows();
				if (this.numTotallyVisibleRows != this.DataGridRowsLength && !flag2)
				{
					int width2 = this.vertScrollBar.Width;
					DataGrid.LayoutData layoutData8 = this.layout;
					layoutData8.Data.Width = layoutData8.Data.Width - width2;
					if (this.layout.ColumnHeadersVisible)
					{
						if (flag4)
						{
							DataGrid.LayoutData layoutData9 = this.layout;
							layoutData9.ColumnHeaders.X = layoutData9.ColumnHeaders.X + width2;
						}
						DataGrid.LayoutData layoutData10 = this.layout;
						layoutData10.ColumnHeaders.Width = layoutData10.ColumnHeaders.Width - width2;
					}
					flag2 = true;
				}
			}
			this.layout.ResizeBoxRect = default(Rectangle);
			if (flag2 && flag)
			{
				Rectangle data = this.layout.Data;
				this.layout.ResizeBoxRect = new Rectangle(flag4 ? data.X : data.Right, data.Bottom, this.vertScrollBar.Width, this.horizScrollBar.Height);
			}
			if (flag && count > 0)
			{
				int num3 = num - this.layout.Data.Width;
				this.horizScrollBar.Minimum = 0;
				this.horizScrollBar.Maximum = num;
				this.horizScrollBar.SmallChange = 1;
				this.horizScrollBar.LargeChange = Math.Max(num - num3, 0);
				this.horizScrollBar.Enabled = base.Enabled;
				this.horizScrollBar.RightToLeft = this.RightToLeft;
				this.horizScrollBar.Bounds = new Rectangle(flag4 ? (this.layout.Inside.X + this.layout.ResizeBoxRect.Width) : this.layout.Inside.X, this.layout.Data.Bottom, this.layout.Inside.Width - this.layout.ResizeBoxRect.Width, this.horizScrollBar.Height);
				this.horizScrollBar.Visible = true;
			}
			else
			{
				this.HorizontalOffset = 0;
				this.horizScrollBar.Visible = false;
			}
			if (flag2)
			{
				int y = this.layout.Data.Y;
				if (this.layout.ColumnHeadersVisible)
				{
					y = this.layout.ColumnHeaders.Y;
				}
				this.vertScrollBar.LargeChange = ((this.numTotallyVisibleRows != 0) ? this.numTotallyVisibleRows : 1);
				this.vertScrollBar.Bounds = new Rectangle(flag4 ? this.layout.Data.X : this.layout.Data.Right, y, this.vertScrollBar.Width, this.layout.Data.Height + this.layout.ColumnHeaders.Height);
				this.vertScrollBar.Enabled = base.Enabled;
				this.vertScrollBar.Visible = true;
				if (flag4)
				{
					DataGrid.LayoutData layoutData11 = this.layout;
					layoutData11.Data.X = layoutData11.Data.X + this.vertScrollBar.Width;
					return;
				}
			}
			else
			{
				this.vertScrollBar.Visible = false;
			}
		}

		/// <summary>Navigates back to the table previously displayed in the grid.</summary>
		// Token: 0x06001256 RID: 4694 RVA: 0x00043A38 File Offset: 0x00041C38
		public void NavigateBack()
		{
			if (!this.CommitEdit() || this.parentRows.IsEmpty())
			{
				return;
			}
			if (this.gridState[1048576])
			{
				this.gridState[1048576] = false;
				try
				{
					this.listManager.CancelCurrentEdit();
					goto IL_4F;
				}
				catch
				{
					goto IL_4F;
				}
			}
			this.UpdateListManager();
			IL_4F:
			DataGridState dataGridState = this.parentRows.PopTop();
			this.ResetMouseState();
			dataGridState.PullState(this, false);
			if (this.parentRows.GetTopParent() == null)
			{
				this.originalState = null;
			}
			DataGridRow[] array = this.DataGridRows;
			if ((this.ReadOnly || !this.policy.AllowAdd) == array[this.DataGridRowsLength - 1] is DataGridAddNewRow)
			{
				int num = (this.ReadOnly || !this.policy.AllowAdd) ? (this.DataGridRowsLength - 1) : (this.DataGridRowsLength + 1);
				DataGridRow[] array2 = new DataGridRow[num];
				for (int i = 0; i < Math.Min(num, this.DataGridRowsLength); i++)
				{
					array2[i] = this.DataGridRows[i];
				}
				if (!this.ReadOnly && this.policy.AllowAdd)
				{
					array2[num - 1] = new DataGridAddNewRow(this, this.myGridTable, num - 1);
				}
				this.SetDataGridRows(array2, num);
			}
			array = this.DataGridRows;
			if (array != null && array.Length != 0)
			{
				DataGridTableStyle dataGridTableStyle = array[0].DataGridTableStyle;
				if (dataGridTableStyle != this.myGridTable)
				{
					for (int j = 0; j < array.Length; j++)
					{
						array[j].DataGridTableStyle = this.myGridTable;
					}
				}
			}
			if (this.myGridTable.GridColumnStyles.Count > 0 && this.myGridTable.GridColumnStyles[0].Width == -1)
			{
				this.InitializeColumnWidths();
			}
			this.currentRow = ((this.ListManager.Position == -1) ? 0 : this.ListManager.Position);
			if (!this.AllowNavigation)
			{
				this.RecreateDataGridRows();
			}
			this.caption.BackButtonActive = (this.parentRows.GetTopParent() != null && this.AllowNavigation);
			this.caption.BackButtonVisible = this.caption.BackButtonActive;
			this.caption.DownButtonActive = (this.parentRows.GetTopParent() != null);
			base.PerformLayout();
			base.Invalidate();
			if (this.vertScrollBar.Visible)
			{
				this.vertScrollBar.Value = this.firstVisibleRow;
			}
			if (this.horizScrollBar.Visible)
			{
				this.horizScrollBar.Value = this.HorizontalOffset + this.negOffset;
			}
			this.Edit();
			this.OnNavigate(new NavigateEventArgs(false));
		}

		/// <summary>Navigates to the table specified by row and relation name.</summary>
		/// <param name="rowNumber">The number of the row to navigate to. </param>
		/// <param name="relationName">The name of the child relation to navigate to. </param>
		// Token: 0x06001257 RID: 4695 RVA: 0x00043CE8 File Offset: 0x00041EE8
		public void NavigateTo(int rowNumber, string relationName)
		{
			if (!this.AllowNavigation)
			{
				return;
			}
			DataGridRow[] array = this.DataGridRows;
			if (rowNumber < 0 || rowNumber > this.DataGridRowsLength - (this.policy.AllowAdd ? 2 : 1))
			{
				throw new ArgumentOutOfRangeException("rowNumber");
			}
			this.EnsureBound();
			DataGridRow source = array[rowNumber];
			this.NavigateTo(relationName, source, false);
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x00043D44 File Offset: 0x00041F44
		internal void NavigateTo(string relationName, DataGridRow source, bool fromRow)
		{
			if (!this.AllowNavigation)
			{
				return;
			}
			if (!this.CommitEdit())
			{
				return;
			}
			DataGridState childState;
			try
			{
				childState = this.CreateChildState(relationName, source);
			}
			catch
			{
				this.NavigateBack();
				return;
			}
			try
			{
				this.listManager.EndCurrentEdit();
			}
			catch
			{
				return;
			}
			DataGridState dataGridState = new DataGridState(this);
			dataGridState.LinkingRow = source;
			if (source.RowNumber != this.CurrentRow)
			{
				this.listManager.Position = source.RowNumber;
			}
			if (this.parentRows.GetTopParent() == null)
			{
				this.originalState = dataGridState;
			}
			this.parentRows.AddParent(dataGridState);
			this.NavigateTo(childState);
			this.OnNavigate(new NavigateEventArgs(true));
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x00043E08 File Offset: 0x00042008
		private void NavigateTo(DataGridState childState)
		{
			this.EndEdit();
			this.gridState[16384] = false;
			this.ResetMouseState();
			childState.PullState(this, true);
			if (this.listManager.Position != this.currentRow)
			{
				this.currentRow = ((this.listManager.Position == -1) ? 0 : this.listManager.Position);
			}
			if (this.parentRows.GetTopParent() != null)
			{
				this.caption.BackButtonActive = this.AllowNavigation;
				this.caption.BackButtonVisible = this.caption.BackButtonActive;
				this.caption.DownButtonActive = true;
			}
			this.HorizontalOffset = 0;
			base.PerformLayout();
			base.Invalidate();
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x00043EC4 File Offset: 0x000420C4
		private Point NormalizeToRow(int x, int y, int row)
		{
			Point point = new Point(0, this.layout.Data.Y);
			DataGridRow[] array = this.DataGridRows;
			for (int i = this.firstVisibleRow; i < row; i++)
			{
				point.Y += array[i].Height;
			}
			return new Point(x, y - point.Y);
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x00043F28 File Offset: 0x00042128
		internal void OnColumnCollectionChanged(object sender, CollectionChangeEventArgs e)
		{
			DataGridTableStyle dataGridTableStyle = (DataGridTableStyle)sender;
			if (dataGridTableStyle.Equals(this.myGridTable))
			{
				if (!this.myGridTable.IsDefault && (e.Action != CollectionChangeAction.Refresh || e.Element == null))
				{
					this.PairTableStylesAndGridColumns(this.listManager, this.myGridTable, false);
				}
				base.Invalidate();
				base.PerformLayout();
			}
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x00043F88 File Offset: 0x00042188
		private void PaintColumnHeaders(Graphics g)
		{
			bool flag = this.isRightToLeft();
			Rectangle columnHeaders = this.layout.ColumnHeaders;
			if (!flag)
			{
				columnHeaders.X -= this.negOffset;
			}
			columnHeaders.Width += this.negOffset;
			int num = this.PaintColumnHeaderText(g, columnHeaders);
			if (flag)
			{
				columnHeaders.X = columnHeaders.Right - num;
			}
			columnHeaders.Width = num;
			if (!this.FlatMode)
			{
				ControlPaint.DrawBorder3D(g, columnHeaders, Border3DStyle.RaisedInner);
				columnHeaders.Inflate(-1, -1);
				int num2 = columnHeaders.Width;
				columnHeaders.Width = num2 - 1;
				num2 = columnHeaders.Height;
				columnHeaders.Height = num2 - 1;
				g.DrawRectangle(SystemPens.Control, columnHeaders);
			}
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x00044040 File Offset: 0x00042240
		private int PaintColumnHeaderText(Graphics g, Rectangle boundingRect)
		{
			int num = 0;
			Rectangle rectangle = boundingRect;
			GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
			bool flag = this.isRightToLeft();
			int count = gridColumnStyles.Count;
			PropertyDescriptor sortProperty = this.ListManager.GetSortProperty();
			for (int i = this.firstVisibleCol; i < count; i++)
			{
				if (gridColumnStyles[i].PropertyDescriptor != null)
				{
					if (num > boundingRect.Width)
					{
						break;
					}
					bool flag2 = sortProperty != null && sortProperty.Equals(gridColumnStyles[i].PropertyDescriptor);
					TriangleDirection dir = TriangleDirection.Up;
					if (flag2)
					{
						ListSortDirection sortDirection = this.ListManager.GetSortDirection();
						if (sortDirection == ListSortDirection.Descending)
						{
							dir = TriangleDirection.Down;
						}
					}
					if (flag)
					{
						rectangle.Width = gridColumnStyles[i].Width - (flag2 ? rectangle.Height : 0);
						rectangle.X = boundingRect.Right - num - rectangle.Width;
					}
					else
					{
						rectangle.X = boundingRect.X + num;
						rectangle.Width = gridColumnStyles[i].Width - (flag2 ? rectangle.Height : 0);
					}
					Brush brush;
					if (this.myGridTable.IsDefault)
					{
						brush = this.HeaderBackBrush;
					}
					else
					{
						brush = this.myGridTable.HeaderBackBrush;
					}
					g.FillRectangle(brush, rectangle);
					if (flag)
					{
						rectangle.X -= 2;
						rectangle.Y += 2;
					}
					else
					{
						rectangle.X += 2;
						rectangle.Y += 2;
					}
					StringFormat stringFormat = new StringFormat();
					HorizontalAlignment alignment = gridColumnStyles[i].Alignment;
					stringFormat.Alignment = ((alignment == HorizontalAlignment.Right) ? StringAlignment.Far : ((alignment == HorizontalAlignment.Center) ? StringAlignment.Center : StringAlignment.Near));
					stringFormat.FormatFlags |= StringFormatFlags.NoWrap;
					if (flag)
					{
						stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
						stringFormat.Alignment = StringAlignment.Near;
					}
					g.DrawString(gridColumnStyles[i].HeaderText, this.myGridTable.IsDefault ? this.HeaderFont : this.myGridTable.HeaderFont, this.myGridTable.IsDefault ? this.HeaderForeBrush : this.myGridTable.HeaderForeBrush, rectangle, stringFormat);
					stringFormat.Dispose();
					if (flag)
					{
						rectangle.X += 2;
						rectangle.Y -= 2;
					}
					else
					{
						rectangle.X -= 2;
						rectangle.Y -= 2;
					}
					if (flag2)
					{
						Rectangle rectangle2 = new Rectangle(flag ? (rectangle.X - rectangle.Height) : rectangle.Right, rectangle.Y, rectangle.Height, rectangle.Height);
						g.FillRectangle(brush, rectangle2);
						int num2 = Math.Max(0, (rectangle.Height - 5) / 2);
						rectangle2.Inflate(-num2, -num2);
						Pen pen = new Pen(this.BackgroundBrush);
						Pen pen2 = new Pen(this.myGridTable.BackBrush);
						Triangle.Paint(g, rectangle2, dir, brush, pen, pen2, pen, true);
						pen.Dispose();
						pen2.Dispose();
					}
					int num3 = rectangle.Width + (flag2 ? rectangle.Height : 0);
					if (!this.FlatMode)
					{
						if (flag && flag2)
						{
							rectangle.X -= rectangle.Height;
						}
						rectangle.Width = num3;
						ControlPaint.DrawBorder3D(g, rectangle, Border3DStyle.RaisedInner);
					}
					num += num3;
				}
			}
			if (num < boundingRect.Width)
			{
				rectangle = boundingRect;
				if (!flag)
				{
					rectangle.X += num;
				}
				rectangle.Width -= num;
				g.FillRectangle(this.backgroundBrush, rectangle);
			}
			return num;
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x00044404 File Offset: 0x00042604
		private void PaintBorder(Graphics g, Rectangle bounds)
		{
			if (this.BorderStyle == BorderStyle.None)
			{
				return;
			}
			if (this.BorderStyle == BorderStyle.Fixed3D)
			{
				Border3DStyle style = Border3DStyle.Sunken;
				ControlPaint.DrawBorder3D(g, bounds, style);
				return;
			}
			if (this.BorderStyle == BorderStyle.FixedSingle)
			{
				Brush brush;
				if (this.myGridTable.IsDefault)
				{
					brush = this.HeaderForeBrush;
				}
				else
				{
					brush = this.myGridTable.HeaderForeBrush;
				}
				g.FillRectangle(brush, bounds.X, bounds.Y, bounds.Width + 2, 2);
				g.FillRectangle(brush, bounds.Right - 2, bounds.Y, 2, bounds.Height + 2);
				g.FillRectangle(brush, bounds.X, bounds.Bottom - 2, bounds.Width + 2, 2);
				g.FillRectangle(brush, bounds.X, bounds.Y, 2, bounds.Height + 2);
				return;
			}
			Pen windowFrame = SystemPens.WindowFrame;
			int num = bounds.Width;
			bounds.Width = num - 1;
			num = bounds.Height;
			bounds.Height = num - 1;
			g.DrawRectangle(windowFrame, bounds);
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x00044510 File Offset: 0x00042710
		private void PaintGrid(Graphics g, Rectangle gridBounds)
		{
			Rectangle rect = gridBounds;
			if (this.listManager != null)
			{
				if (this.layout.ColumnHeadersVisible)
				{
					Region clip = g.Clip;
					g.SetClip(this.layout.ColumnHeaders);
					this.PaintColumnHeaders(g);
					g.Clip = clip;
					clip.Dispose();
					int height = this.layout.ColumnHeaders.Height;
					rect.Y += height;
					rect.Height -= height;
				}
				if (this.layout.TopLeftHeader.Width > 0)
				{
					if (this.myGridTable.IsDefault)
					{
						g.FillRectangle(this.HeaderBackBrush, this.layout.TopLeftHeader);
					}
					else
					{
						g.FillRectangle(this.myGridTable.HeaderBackBrush, this.layout.TopLeftHeader);
					}
					if (!this.FlatMode)
					{
						ControlPaint.DrawBorder3D(g, this.layout.TopLeftHeader, Border3DStyle.RaisedInner);
					}
				}
				this.PaintRows(g, ref rect);
			}
			if (rect.Height > 0)
			{
				g.FillRectangle(this.backgroundBrush, rect);
			}
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x00044624 File Offset: 0x00042824
		private void DeleteDataGridRows(int deletedRows)
		{
			if (deletedRows == 0)
			{
				return;
			}
			int num = this.DataGridRowsLength;
			int num2 = num - deletedRows + (this.gridState[1048576] ? 1 : 0);
			DataGridRow[] array = new DataGridRow[num2];
			DataGridRow[] array2 = this.DataGridRows;
			int num3 = 0;
			for (int i = 0; i < num; i++)
			{
				if (array2[i].Selected)
				{
					num3++;
				}
				else
				{
					array[i - num3] = array2[i];
					array[i - num3].number = i - num3;
				}
			}
			if (this.gridState[1048576])
			{
				array[num - num3] = new DataGridAddNewRow(this, this.myGridTable, num - num3);
				this.gridState[1048576] = false;
			}
			this.SetDataGridRows(array, num2);
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x000446E8 File Offset: 0x000428E8
		private void PaintRows(Graphics g, ref Rectangle boundingRect)
		{
			int num = 0;
			bool flag = this.isRightToLeft();
			Rectangle rectangle = boundingRect;
			Rectangle dataBounds = Rectangle.Empty;
			bool rowHeadersVisible = this.layout.RowHeadersVisible;
			Rectangle rectangle2 = Rectangle.Empty;
			int num2 = this.DataGridRowsLength;
			DataGridRow[] array = this.DataGridRows;
			int numVisibleColumns = this.myGridTable.GridColumnStyles.Count - this.firstVisibleCol;
			int num3 = this.firstVisibleRow;
			while (num3 < num2 && num <= boundingRect.Height)
			{
				rectangle = boundingRect;
				rectangle.Height = array[num3].Height;
				rectangle.Y = boundingRect.Y + num;
				if (rowHeadersVisible)
				{
					rectangle2 = rectangle;
					rectangle2.Width = this.layout.RowHeaders.Width;
					if (flag)
					{
						rectangle2.X = rectangle.Right - rectangle2.Width;
					}
					if (g.IsVisible(rectangle2))
					{
						array[num3].PaintHeader(g, rectangle2, flag, this.gridState[32768]);
						g.ExcludeClip(rectangle2);
					}
					if (!flag)
					{
						rectangle.X += rectangle2.Width;
					}
					rectangle.Width -= rectangle2.Width;
				}
				if (g.IsVisible(rectangle))
				{
					dataBounds = rectangle;
					if (!flag)
					{
						dataBounds.X -= this.negOffset;
					}
					dataBounds.Width += this.negOffset;
					array[num3].Paint(g, dataBounds, rectangle, this.firstVisibleCol, numVisibleColumns, flag);
				}
				num += rectangle.Height;
				num3++;
			}
			boundingRect.Y += num;
			boundingRect.Height -= num;
		}

		/// <summary>Gets or sets a value that indicates whether a key should be processed further.</summary>
		/// <param name="keyData">A <see cref="T:System.Windows.Forms.Keys" /> that contains data about the pressed key. </param>
		/// <returns>
		///     <see langword="true" />, the key should be processed; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001262 RID: 4706 RVA: 0x000448A4 File Offset: 0x00042AA4
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			DataGridRow[] array = this.DataGridRows;
			if (this.listManager != null && this.DataGridRowsLength > 0 && array[this.currentRow].OnKeyPress(keyData))
			{
				return true;
			}
			Keys keys = keyData & Keys.KeyCode;
			if (keys <= Keys.A)
			{
				if (keys <= Keys.Return)
				{
					if (keys != Keys.Tab && keys != Keys.Return)
					{
						goto IL_243;
					}
				}
				else
				{
					switch (keys)
					{
					case Keys.Escape:
					case Keys.Space:
					case Keys.Prior:
					case Keys.Next:
					case Keys.Left:
					case Keys.Up:
					case Keys.Right:
					case Keys.Down:
						break;
					case Keys.IMEConvert:
					case Keys.IMENonconvert:
					case Keys.IMEAccept:
					case Keys.IMEModeChange:
					case Keys.End:
					case Keys.Home:
						goto IL_243;
					default:
						if (keys != Keys.Delete && keys != Keys.A)
						{
							goto IL_243;
						}
						break;
					}
				}
			}
			else if (keys <= Keys.Add)
			{
				if (keys != Keys.C)
				{
					if (keys != Keys.Add)
					{
						goto IL_243;
					}
				}
				else
				{
					if ((keyData & Keys.Control) == Keys.None || (keyData & Keys.Alt) != Keys.None || !this.Bound)
					{
						goto IL_243;
					}
					if (this.numSelectedRows != 0)
					{
						int num = 0;
						string text = "";
						for (int i = 0; i < this.DataGridRowsLength; i++)
						{
							if (array[i].Selected)
							{
								GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
								int count = gridColumnStyles.Count;
								for (int j = 0; j < count; j++)
								{
									DataGridColumnStyle dataGridColumnStyle = gridColumnStyles[j];
									text += dataGridColumnStyle.GetDisplayText(dataGridColumnStyle.GetColumnValueAtRow(this.ListManager, i));
									if (j < count - 1)
									{
										text += this.GetOutputTextDelimiter();
									}
								}
								if (num < this.numSelectedRows - 1)
								{
									text += "\r\n";
								}
								num++;
							}
						}
						Clipboard.SetDataObject(text);
						return true;
					}
					if (this.currentRow >= this.ListManager.Count)
					{
						goto IL_243;
					}
					GridColumnStylesCollection gridColumnStyles2 = this.myGridTable.GridColumnStyles;
					if (this.currentCol >= 0 && this.currentCol < gridColumnStyles2.Count)
					{
						DataGridColumnStyle dataGridColumnStyle2 = gridColumnStyles2[this.currentCol];
						string displayText = dataGridColumnStyle2.GetDisplayText(dataGridColumnStyle2.GetColumnValueAtRow(this.ListManager, this.currentRow));
						Clipboard.SetDataObject(displayText);
						return true;
					}
					goto IL_243;
				}
			}
			else if (keys != Keys.Subtract && keys != Keys.Oemplus && keys != Keys.OemMinus)
			{
				goto IL_243;
			}
			KeyEventArgs ke = new KeyEventArgs(keyData);
			if (this.ProcessGridKey(ke))
			{
				return true;
			}
			IL_243:
			return base.ProcessDialogKey(keyData);
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x00044AFC File Offset: 0x00042CFC
		private void DeleteRows(DataGridRow[] localGridRows)
		{
			int num = 0;
			int num2 = (this.listManager == null) ? 0 : this.listManager.Count;
			if (base.Visible)
			{
				base.BeginUpdateInternal();
			}
			try
			{
				if (this.ListManager != null)
				{
					for (int i = 0; i < this.DataGridRowsLength; i++)
					{
						if (localGridRows[i].Selected)
						{
							if (localGridRows[i] is DataGridAddNewRow)
							{
								localGridRows[i].Selected = false;
							}
							else
							{
								this.ListManager.RemoveAt(i - num);
								num++;
							}
						}
					}
				}
			}
			catch
			{
				this.RecreateDataGridRows();
				this.gridState[1024] = false;
				if (base.Visible)
				{
					base.EndUpdateInternal();
				}
				throw;
			}
			if (this.listManager != null && num2 == this.listManager.Count + num)
			{
				this.DeleteDataGridRows(num);
			}
			else
			{
				this.RecreateDataGridRows();
			}
			this.gridState[1024] = false;
			if (base.Visible)
			{
				base.EndUpdateInternal();
			}
			if (this.listManager != null && num2 != this.listManager.Count + num)
			{
				base.Invalidate();
			}
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x00044C1C File Offset: 0x00042E1C
		private int MoveLeftRight(GridColumnStylesCollection cols, int startCol, bool goRight)
		{
			int i;
			if (goRight)
			{
				for (i = startCol + 1; i < cols.Count; i++)
				{
					if (cols[i].PropertyDescriptor != null)
					{
						return i;
					}
				}
				return i;
			}
			for (i = startCol - 1; i >= 0; i--)
			{
				if (cols[i].PropertyDescriptor != null)
				{
					return i;
				}
			}
			return i;
		}

		/// <summary>Processes keys for grid navigation.</summary>
		/// <param name="ke">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains data about the key up or key down event. </param>
		/// <returns>
		///     <see langword="true" />, if the key was processed; otherwise <see langword="false" />.</returns>
		// Token: 0x06001265 RID: 4709 RVA: 0x00044C70 File Offset: 0x00042E70
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected bool ProcessGridKey(KeyEventArgs ke)
		{
			if (this.listManager == null || this.myGridTable == null)
			{
				return false;
			}
			DataGridRow[] localGridRows = this.DataGridRows;
			KeyEventArgs keyEventArgs = ke;
			Keys keyCode;
			if (this.isRightToLeft())
			{
				keyCode = ke.KeyCode;
				if (keyCode != Keys.Left)
				{
					if (keyCode == Keys.Right)
					{
						keyEventArgs = new KeyEventArgs(Keys.Left | ke.Modifiers);
					}
				}
				else
				{
					keyEventArgs = new KeyEventArgs(Keys.Right | ke.Modifiers);
				}
			}
			GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
			int num = 0;
			int num2 = gridColumnStyles.Count;
			for (int i = 0; i < gridColumnStyles.Count; i++)
			{
				if (gridColumnStyles[i].PropertyDescriptor != null)
				{
					num = i;
					break;
				}
			}
			for (int j = gridColumnStyles.Count - 1; j >= 0; j--)
			{
				if (gridColumnStyles[j].PropertyDescriptor != null)
				{
					num2 = j;
					break;
				}
			}
			keyCode = keyEventArgs.KeyCode;
			if (keyCode <= Keys.A)
			{
				if (keyCode <= Keys.Return)
				{
					if (keyCode == Keys.Tab)
					{
						return this.ProcessTabKey(keyEventArgs.KeyData);
					}
					if (keyCode == Keys.Return)
					{
						this.gridState[524288] = false;
						this.ResetSelection();
						if (!this.gridState[32768])
						{
							return false;
						}
						if ((keyEventArgs.Modifiers & Keys.Control) != Keys.None && !keyEventArgs.Alt)
						{
							this.EndEdit();
							this.HandleEndCurrentEdit();
							this.Edit();
						}
						else
						{
							this.CurrentRow = this.currentRow + 1;
						}
					}
				}
				else
				{
					switch (keyCode)
					{
					case Keys.Escape:
						this.gridState[524288] = false;
						this.ResetSelection();
						if (!this.gridState[32768])
						{
							this.CancelEditing();
							this.Edit();
							return false;
						}
						this.AbortEdit();
						if (this.layout.RowHeadersVisible && this.currentRow > -1)
						{
							Rectangle rowRect = this.GetRowRect(this.currentRow);
							rowRect.Width = this.layout.RowHeaders.Width;
							base.Invalidate(rowRect);
						}
						this.Edit();
						break;
					case Keys.IMEConvert:
					case Keys.IMENonconvert:
					case Keys.IMEAccept:
					case Keys.IMEModeChange:
					case Keys.Select:
					case Keys.Print:
					case Keys.Execute:
					case Keys.Snapshot:
					case Keys.Insert:
						break;
					case Keys.Space:
						this.gridState[524288] = false;
						if (this.dataGridRowsLength == 0)
						{
							return true;
						}
						if (keyEventArgs.Shift)
						{
							this.ResetSelection();
							this.EndEdit();
							DataGridRow[] array = this.DataGridRows;
							array[this.currentRow].Selected = true;
							this.numSelectedRows = 1;
							return true;
						}
						return false;
					case Keys.Prior:
						if (this.dataGridRowsLength == 0)
						{
							return true;
						}
						this.gridState[524288] = false;
						if (keyEventArgs.Shift)
						{
							int num3 = this.currentRow;
							this.CurrentRow = Math.Max(0, this.CurrentRow - this.numTotallyVisibleRows);
							DataGridRow[] array2 = this.DataGridRows;
							for (int k = num3; k >= this.currentRow; k--)
							{
								if (!array2[k].Selected)
								{
									array2[k].Selected = true;
									this.numSelectedRows++;
								}
							}
							this.EndEdit();
						}
						else if (keyEventArgs.Control && !keyEventArgs.Alt)
						{
							this.ParentRowsVisible = false;
						}
						else
						{
							this.ResetSelection();
							this.CurrentRow = Math.Max(0, this.CurrentRow - this.numTotallyVisibleRows);
						}
						break;
					case Keys.Next:
						this.gridState[524288] = false;
						if (this.dataGridRowsLength == 0)
						{
							return true;
						}
						if (keyEventArgs.Shift)
						{
							int num4 = this.currentRow;
							this.CurrentRow = Math.Min(this.DataGridRowsLength - (this.policy.AllowAdd ? 2 : 1), this.currentRow + this.numTotallyVisibleRows);
							DataGridRow[] array3 = this.DataGridRows;
							for (int l = num4; l <= this.currentRow; l++)
							{
								if (!array3[l].Selected)
								{
									array3[l].Selected = true;
									this.numSelectedRows++;
								}
							}
							this.EndEdit();
						}
						else if (keyEventArgs.Control && !keyEventArgs.Alt)
						{
							this.ParentRowsVisible = true;
						}
						else
						{
							this.ResetSelection();
							this.CurrentRow = Math.Min(this.DataGridRowsLength - (this.policy.AllowAdd ? 2 : 1), this.CurrentRow + this.numTotallyVisibleRows);
						}
						break;
					case Keys.End:
						this.gridState[524288] = false;
						if (this.dataGridRowsLength == 0)
						{
							return true;
						}
						this.ResetSelection();
						this.CurrentColumn = num2;
						if (keyEventArgs.Control && !keyEventArgs.Alt)
						{
							int num5 = this.currentRow;
							this.CurrentRow = Math.Max(0, this.DataGridRowsLength - (this.policy.AllowAdd ? 2 : 1));
							if (keyEventArgs.Shift)
							{
								DataGridRow[] array4 = this.DataGridRows;
								for (int m = num5; m <= this.currentRow; m++)
								{
									array4[m].Selected = true;
								}
								this.numSelectedRows = this.currentRow - num5 + 1;
								this.EndEdit();
							}
							return true;
						}
						break;
					case Keys.Home:
						this.gridState[524288] = false;
						if (this.dataGridRowsLength == 0)
						{
							return true;
						}
						this.ResetSelection();
						this.CurrentColumn = 0;
						if (keyEventArgs.Control && !keyEventArgs.Alt)
						{
							int num6 = this.currentRow;
							this.CurrentRow = 0;
							if (keyEventArgs.Shift)
							{
								DataGridRow[] array5 = this.DataGridRows;
								for (int n = 0; n <= num6; n++)
								{
									array5[n].Selected = true;
									this.numSelectedRows++;
								}
								this.EndEdit();
							}
							return true;
						}
						break;
					case Keys.Left:
						this.gridState[524288] = false;
						this.ResetSelection();
						if ((keyEventArgs.Modifiers & Keys.Modifiers) == Keys.Alt)
						{
							if (this.Caption.BackButtonVisible)
							{
								this.NavigateBack();
							}
							return true;
						}
						if ((keyEventArgs.Modifiers & Keys.Control) == Keys.Control)
						{
							this.CurrentColumn = num;
						}
						else if (this.currentCol == num && this.currentRow != 0)
						{
							this.CurrentRow--;
							int currentColumn = this.MoveLeftRight(this.myGridTable.GridColumnStyles, this.myGridTable.GridColumnStyles.Count, false);
							this.CurrentColumn = currentColumn;
						}
						else
						{
							int num7 = this.MoveLeftRight(this.myGridTable.GridColumnStyles, this.currentCol, false);
							if (num7 == -1)
							{
								if (this.currentRow == 0)
								{
									return true;
								}
								this.CurrentRow--;
								this.CurrentColumn = num2;
							}
							else
							{
								this.CurrentColumn = num7;
							}
						}
						break;
					case Keys.Up:
						this.gridState[524288] = false;
						if (this.dataGridRowsLength == 0)
						{
							return true;
						}
						if (keyEventArgs.Control && !keyEventArgs.Alt)
						{
							if (keyEventArgs.Shift)
							{
								DataGridRow[] array6 = this.DataGridRows;
								int num8 = this.currentRow;
								this.CurrentRow = 0;
								this.ResetSelection();
								for (int num9 = 0; num9 <= num8; num9++)
								{
									array6[num9].Selected = true;
								}
								this.numSelectedRows = num8 + 1;
								this.EndEdit();
								return true;
							}
							this.ResetSelection();
							this.CurrentRow = 0;
							return true;
						}
						else
						{
							if (keyEventArgs.Shift)
							{
								DataGridRow[] array7 = this.DataGridRows;
								if (array7[this.currentRow].Selected)
								{
									if (this.currentRow >= 1)
									{
										if (array7[this.currentRow - 1].Selected)
										{
											if (this.currentRow >= this.DataGridRowsLength - 1 || !array7[this.currentRow + 1].Selected)
											{
												this.numSelectedRows--;
												array7[this.currentRow].Selected = false;
											}
										}
										else
										{
											this.numSelectedRows += (array7[this.currentRow - 1].Selected ? 0 : 1);
											array7[this.currentRow - 1].Selected = true;
										}
										int num10 = this.CurrentRow;
										this.CurrentRow = num10 - 1;
									}
								}
								else
								{
									this.numSelectedRows++;
									array7[this.currentRow].Selected = true;
									if (this.currentRow >= 1)
									{
										this.numSelectedRows += (array7[this.currentRow - 1].Selected ? 0 : 1);
										array7[this.currentRow - 1].Selected = true;
										int num10 = this.CurrentRow;
										this.CurrentRow = num10 - 1;
									}
								}
								this.EndEdit();
								return true;
							}
							if (keyEventArgs.Alt)
							{
								this.SetRowExpansionState(-1, false);
								return true;
							}
							this.ResetSelection();
							this.CurrentRow--;
							this.Edit();
						}
						break;
					case Keys.Right:
						this.gridState[524288] = false;
						this.ResetSelection();
						if ((keyEventArgs.Modifiers & Keys.Control) == Keys.Control && !keyEventArgs.Alt)
						{
							this.CurrentColumn = num2;
						}
						else if (this.currentCol == num2 && this.currentRow != this.DataGridRowsLength - 1)
						{
							this.CurrentRow++;
							this.CurrentColumn = num;
						}
						else
						{
							int num11 = this.MoveLeftRight(this.myGridTable.GridColumnStyles, this.currentCol, true);
							if (num11 == gridColumnStyles.Count + 1)
							{
								this.CurrentColumn = num;
								int num10 = this.CurrentRow;
								this.CurrentRow = num10 + 1;
							}
							else
							{
								this.CurrentColumn = num11;
							}
						}
						break;
					case Keys.Down:
						this.gridState[524288] = false;
						if (this.dataGridRowsLength == 0)
						{
							return true;
						}
						if (keyEventArgs.Control && !keyEventArgs.Alt)
						{
							if (keyEventArgs.Shift)
							{
								int num12 = this.currentRow;
								this.CurrentRow = Math.Max(0, this.DataGridRowsLength - (this.policy.AllowAdd ? 2 : 1));
								DataGridRow[] array8 = this.DataGridRows;
								this.ResetSelection();
								for (int num13 = num12; num13 <= this.currentRow; num13++)
								{
									array8[num13].Selected = true;
								}
								this.numSelectedRows = this.currentRow - num12 + 1;
								this.EndEdit();
								return true;
							}
							this.ResetSelection();
							this.CurrentRow = Math.Max(0, this.DataGridRowsLength - (this.policy.AllowAdd ? 2 : 1));
							return true;
						}
						else
						{
							if (keyEventArgs.Shift)
							{
								DataGridRow[] array9 = this.DataGridRows;
								if (array9[this.currentRow].Selected)
								{
									if (this.currentRow < this.DataGridRowsLength - (this.policy.AllowAdd ? 1 : 0) - 1)
									{
										if (array9[this.currentRow + 1].Selected)
										{
											if (this.currentRow == 0 || !array9[this.currentRow - 1].Selected)
											{
												this.numSelectedRows--;
												array9[this.currentRow].Selected = false;
											}
										}
										else
										{
											this.numSelectedRows += (array9[this.currentRow + 1].Selected ? 0 : 1);
											array9[this.currentRow + 1].Selected = true;
										}
										int num10 = this.CurrentRow;
										this.CurrentRow = num10 + 1;
									}
								}
								else
								{
									this.numSelectedRows++;
									array9[this.currentRow].Selected = true;
									if (this.currentRow < this.DataGridRowsLength - (this.policy.AllowAdd ? 1 : 0) - 1)
									{
										int num10 = this.CurrentRow;
										this.CurrentRow = num10 + 1;
										this.numSelectedRows += (array9[this.currentRow].Selected ? 0 : 1);
										array9[this.currentRow].Selected = true;
									}
								}
								this.EndEdit();
								return true;
							}
							if (keyEventArgs.Alt)
							{
								this.SetRowExpansionState(-1, true);
								return true;
							}
							this.ResetSelection();
							this.Edit();
							this.CurrentRow++;
						}
						break;
					case Keys.Delete:
						this.gridState[524288] = false;
						if (!this.policy.AllowRemove || this.numSelectedRows <= 0)
						{
							return false;
						}
						this.gridState[1024] = true;
						this.DeleteRows(localGridRows);
						this.currentRow = ((this.listManager.Count == 0) ? 0 : this.listManager.Position);
						this.numSelectedRows = 0;
						break;
					default:
						if (keyCode == Keys.A)
						{
							this.gridState[524288] = false;
							if (keyEventArgs.Control && !keyEventArgs.Alt)
							{
								DataGridRow[] array10 = this.DataGridRows;
								for (int num14 = 0; num14 < this.DataGridRowsLength; num14++)
								{
									if (array10[num14] is DataGridRelationshipRow)
									{
										array10[num14].Selected = true;
									}
								}
								this.numSelectedRows = this.DataGridRowsLength - (this.policy.AllowAdd ? 1 : 0);
								this.EndEdit();
								return true;
							}
							return false;
						}
						break;
					}
				}
			}
			else
			{
				if (keyCode <= Keys.Subtract)
				{
					if (keyCode == Keys.Add)
					{
						goto IL_64A;
					}
					if (keyCode != Keys.Subtract)
					{
						return true;
					}
				}
				else
				{
					if (keyCode == Keys.F2)
					{
						this.gridState[524288] = false;
						this.ResetSelection();
						this.Edit();
						return true;
					}
					if (keyCode == Keys.Oemplus)
					{
						goto IL_64A;
					}
					if (keyCode != Keys.OemMinus)
					{
						return true;
					}
				}
				this.gridState[524288] = false;
				if (keyEventArgs.Control && !keyEventArgs.Alt)
				{
					this.SetRowExpansionState(-1, false);
					return true;
				}
				return false;
				IL_64A:
				this.gridState[524288] = false;
				if (keyEventArgs.Control)
				{
					this.SetRowExpansionState(-1, true);
					this.EndEdit();
					return true;
				}
				return false;
			}
			return true;
		}

		/// <summary>Previews a keyboard message and returns a value indicating if the key was consumed.</summary>
		/// <param name="m">A <see cref="T:System.Windows.Forms.Message" /> that contains data about the event. The parameter is passed by reference. </param>
		/// <returns>
		///     <see langword="true" />, if the key was consumed; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001266 RID: 4710 RVA: 0x00045A20 File Offset: 0x00043C20
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override bool ProcessKeyPreview(ref Message m)
		{
			if (m.Msg == 256)
			{
				KeyEventArgs keyEventArgs = new KeyEventArgs((Keys)((long)m.WParam) | Control.ModifierKeys);
				Keys keyCode = keyEventArgs.KeyCode;
				if (keyCode <= Keys.A)
				{
					if (keyCode <= Keys.Return)
					{
						if (keyCode != Keys.Tab && keyCode != Keys.Return)
						{
							goto IL_113;
						}
					}
					else
					{
						switch (keyCode)
						{
						case Keys.Escape:
						case Keys.Space:
						case Keys.Prior:
						case Keys.Next:
						case Keys.End:
						case Keys.Home:
						case Keys.Left:
						case Keys.Up:
						case Keys.Right:
						case Keys.Down:
						case Keys.Delete:
							break;
						case Keys.IMEConvert:
						case Keys.IMENonconvert:
						case Keys.IMEAccept:
						case Keys.IMEModeChange:
						case Keys.Select:
						case Keys.Print:
						case Keys.Execute:
						case Keys.Snapshot:
						case Keys.Insert:
							goto IL_113;
						default:
							if (keyCode != Keys.A)
							{
								goto IL_113;
							}
							break;
						}
					}
				}
				else if (keyCode <= Keys.Subtract)
				{
					if (keyCode != Keys.Add && keyCode != Keys.Subtract)
					{
						goto IL_113;
					}
				}
				else if (keyCode != Keys.F2 && keyCode != Keys.Oemplus && keyCode != Keys.OemMinus)
				{
					goto IL_113;
				}
				return this.ProcessGridKey(keyEventArgs);
			}
			if (m.Msg == 257)
			{
				KeyEventArgs keyEventArgs2 = new KeyEventArgs((Keys)((long)m.WParam) | Control.ModifierKeys);
				if (keyEventArgs2.KeyCode == Keys.Tab)
				{
					return this.ProcessGridKey(keyEventArgs2);
				}
			}
			IL_113:
			return base.ProcessKeyPreview(ref m);
		}

		/// <summary>Gets a value indicating whether the Tab key should be processed.</summary>
		/// <param name="keyData">A <see cref="T:System.Windows.Forms.Keys" /> that contains data about which the pressed key. </param>
		/// <returns>
		///     <see langword="true" /> if the TAB key should be processed; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001267 RID: 4711 RVA: 0x00045B48 File Offset: 0x00043D48
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected bool ProcessTabKey(Keys keyData)
		{
			if (this.listManager == null || this.myGridTable == null)
			{
				return false;
			}
			bool flag = false;
			int count = this.myGridTable.GridColumnStyles.Count;
			bool flag2 = this.isRightToLeft();
			this.ResetSelection();
			if (this.gridState[32768])
			{
				flag = true;
				if (!this.CommitEdit())
				{
					this.Edit();
					return true;
				}
			}
			if ((keyData & Keys.Control) == Keys.Control)
			{
				if ((keyData & Keys.Alt) == Keys.Alt)
				{
					return true;
				}
				Keys keyData2 = keyData & ~Keys.Control;
				this.EndEdit();
				this.gridState[65536] = true;
				try
				{
					this.FocusInternal();
				}
				finally
				{
					this.gridState[65536] = false;
				}
				bool result = false;
				IntSecurity.ModifyFocus.Assert();
				try
				{
					result = base.ProcessDialogKey(keyData2);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				return result;
			}
			else
			{
				DataGridRow[] array = this.DataGridRows;
				GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
				int num = 0;
				int num2 = gridColumnStyles.Count - 1;
				if (array.Length == 0)
				{
					this.EndEdit();
					bool result2 = false;
					IntSecurity.ModifyFocus.Assert();
					try
					{
						result2 = base.ProcessDialogKey(keyData);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					return result2;
				}
				for (int i = 0; i < gridColumnStyles.Count; i++)
				{
					if (gridColumnStyles[i].PropertyDescriptor != null)
					{
						num2 = i;
						break;
					}
				}
				for (int j = gridColumnStyles.Count - 1; j >= 0; j--)
				{
					if (gridColumnStyles[j].PropertyDescriptor != null)
					{
						num = j;
						break;
					}
				}
				if (this.CurrentColumn == num)
				{
					if ((this.gridState[524288] || (!this.gridState[524288] && (keyData & Keys.Shift) != Keys.Shift)) && array[this.CurrentRow].ProcessTabKey(keyData, this.layout.RowHeaders, this.isRightToLeft()))
					{
						if (gridColumnStyles.Count > 0)
						{
							gridColumnStyles[this.CurrentColumn].ConcedeFocus();
						}
						this.gridState[524288] = true;
						if (this.gridState[2048] && base.CanFocus && !this.Focused)
						{
							this.FocusInternal();
						}
						return true;
					}
					if (this.currentRow == this.DataGridRowsLength - 1 && (keyData & Keys.Shift) == Keys.None)
					{
						this.EndEdit();
						bool result3 = false;
						IntSecurity.ModifyFocus.Assert();
						try
						{
							result3 = base.ProcessDialogKey(keyData);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
						return result3;
					}
				}
				if (this.CurrentColumn == num2)
				{
					if (!this.gridState[524288])
					{
						if (this.CurrentRow != 0 && (keyData & Keys.Shift) == Keys.Shift && array[this.CurrentRow - 1].ProcessTabKey(keyData, this.layout.RowHeaders, this.isRightToLeft()))
						{
							int num3 = this.CurrentRow;
							this.CurrentRow = num3 - 1;
							if (gridColumnStyles.Count > 0)
							{
								gridColumnStyles[this.CurrentColumn].ConcedeFocus();
							}
							this.gridState[524288] = true;
							if (this.gridState[2048] && base.CanFocus && !this.Focused)
							{
								this.FocusInternal();
							}
							return true;
						}
						if (this.currentRow == 0 && (keyData & Keys.Shift) == Keys.Shift)
						{
							this.EndEdit();
							bool result4 = false;
							IntSecurity.ModifyFocus.Assert();
							try
							{
								result4 = base.ProcessDialogKey(keyData);
							}
							finally
							{
								CodeAccessPermission.RevertAssert();
							}
							return result4;
						}
					}
					else
					{
						if (array[this.CurrentRow].ProcessTabKey(keyData, this.layout.RowHeaders, this.isRightToLeft()))
						{
							return true;
						}
						this.gridState[524288] = false;
						this.CurrentColumn = num;
						return true;
					}
				}
				if ((keyData & Keys.Shift) != Keys.Shift)
				{
					if (this.CurrentColumn == num)
					{
						if (this.CurrentRow != this.DataGridRowsLength - 1)
						{
							this.CurrentColumn = num2;
						}
						this.CurrentRow++;
					}
					else
					{
						int currentColumn = this.MoveLeftRight(gridColumnStyles, this.currentCol, true);
						this.CurrentColumn = currentColumn;
					}
				}
				else if (this.CurrentColumn == num2)
				{
					if (this.CurrentRow != 0)
					{
						this.CurrentColumn = num;
					}
					if (!this.gridState[524288])
					{
						int num3 = this.CurrentRow;
						this.CurrentRow = num3 - 1;
					}
				}
				else if (this.gridState[524288] && this.CurrentColumn == num)
				{
					this.InvalidateRow(this.currentRow);
					this.Edit();
				}
				else
				{
					int currentColumn2 = this.MoveLeftRight(gridColumnStyles, this.currentCol, false);
					this.CurrentColumn = currentColumn2;
				}
				this.gridState[524288] = false;
				if (flag)
				{
					this.ResetSelection();
					this.Edit();
				}
				return true;
			}
		}

		/// <summary>Cancels the current edit operation and rolls back all changes.</summary>
		// Token: 0x06001268 RID: 4712 RVA: 0x0004605C File Offset: 0x0004425C
		protected virtual void CancelEditing()
		{
			this.CancelCursorUpdate();
			if (this.gridState[1048576])
			{
				this.gridState[1048576] = false;
				DataGridRow[] array = this.DataGridRows;
				array[this.DataGridRowsLength - 1] = new DataGridAddNewRow(this, this.myGridTable, this.DataGridRowsLength - 1);
				this.SetDataGridRows(array, this.DataGridRowsLength);
			}
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x000460C4 File Offset: 0x000442C4
		internal void RecalculateFonts()
		{
			try
			{
				this.linkFont = new Font(this.Font, FontStyle.Underline);
			}
			catch
			{
			}
			this.fontHeight = this.Font.Height;
			this.linkFontHeight = this.LinkFont.Height;
			this.captionFontHeight = this.CaptionFont.Height;
			if (this.myGridTable == null || this.myGridTable.IsDefault)
			{
				this.headerFontHeight = this.HeaderFont.Height;
				return;
			}
			this.headerFontHeight = this.myGridTable.HeaderFont.Height;
		}

		/// <summary>Occurs when the <see langword="Back" /> button on a child table is clicked.</summary>
		// Token: 0x140000C4 RID: 196
		// (add) Token: 0x0600126A RID: 4714 RVA: 0x00046168 File Offset: 0x00044368
		// (remove) Token: 0x0600126B RID: 4715 RVA: 0x0004617B File Offset: 0x0004437B
		[SRCategory("CatAction")]
		[SRDescription("DataGridBackButtonClickDescr")]
		public event EventHandler BackButtonClick
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_BACKBUTTONCLICK, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_BACKBUTTONCLICK, value);
			}
		}

		/// <summary>Occurs when the <see langword="ShowParentDetails" /> button is clicked.</summary>
		// Token: 0x140000C5 RID: 197
		// (add) Token: 0x0600126C RID: 4716 RVA: 0x0004618E File Offset: 0x0004438E
		// (remove) Token: 0x0600126D RID: 4717 RVA: 0x000461A1 File Offset: 0x000443A1
		[SRCategory("CatAction")]
		[SRDescription("DataGridDownButtonClickDescr")]
		public event EventHandler ShowParentDetailsButtonClick
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_DOWNBUTTONCLICK, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_DOWNBUTTONCLICK, value);
			}
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x000461B4 File Offset: 0x000443B4
		private void ResetMouseState()
		{
			this.oldRow = -1;
			this.gridState[262144] = true;
		}

		/// <summary>Turns off selection for all rows that are selected.</summary>
		// Token: 0x0600126F RID: 4719 RVA: 0x000461D0 File Offset: 0x000443D0
		protected void ResetSelection()
		{
			if (this.numSelectedRows > 0)
			{
				DataGridRow[] array = this.DataGridRows;
				for (int i = 0; i < this.DataGridRowsLength; i++)
				{
					if (array[i].Selected)
					{
						array[i].Selected = false;
					}
				}
			}
			this.numSelectedRows = 0;
			this.lastRowSelected = -1;
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x00046220 File Offset: 0x00044420
		private void ResetParentRows()
		{
			this.parentRows.Clear();
			this.originalState = null;
			this.caption.BackButtonActive = (this.caption.DownButtonActive = (this.caption.BackButtonVisible = false));
			this.caption.SetDownButtonDirection(!this.layout.ParentRowsVisible);
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x00046280 File Offset: 0x00044480
		private void ResetUIState()
		{
			this.gridState[524288] = false;
			this.ResetSelection();
			this.ResetMouseState();
			base.PerformLayout();
			base.Invalidate();
			if (this.horizScrollBar.Visible)
			{
				this.horizScrollBar.Invalidate();
			}
			if (this.vertScrollBar.Visible)
			{
				this.vertScrollBar.Invalidate();
			}
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x000462E8 File Offset: 0x000444E8
		private void ScrollDown(int rows)
		{
			if (rows != 0)
			{
				this.ClearRegionCache();
				int num = Math.Max(0, Math.Min(this.firstVisibleRow + rows, this.DataGridRowsLength - 1));
				int from = this.firstVisibleRow;
				this.firstVisibleRow = num;
				this.vertScrollBar.Value = num;
				bool flag = this.gridState[32768];
				this.ComputeVisibleRows();
				if (this.gridState[131072])
				{
					this.Edit();
					this.gridState[131072] = false;
				}
				else
				{
					this.EndEdit();
				}
				int nYAmount = this.ComputeRowDelta(from, num);
				Rectangle a = this.layout.Data;
				if (this.layout.RowHeadersVisible)
				{
					a = Rectangle.Union(a, this.layout.RowHeaders);
				}
				NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(a.X, a.Y, a.Width, a.Height);
				SafeNativeMethods.ScrollWindow(new HandleRef(this, base.Handle), 0, nYAmount, ref rect, ref rect);
				this.OnScroll(EventArgs.Empty);
				if (flag)
				{
					this.InvalidateRowHeader(this.currentRow);
				}
			}
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x0004640C File Offset: 0x0004460C
		private void ScrollRight(int columns)
		{
			int num = this.firstVisibleCol + columns;
			GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
			int num2 = 0;
			int count = gridColumnStyles.Count;
			int num3 = 0;
			if (this.myGridTable.IsDefault)
			{
				num3 = count;
			}
			else
			{
				for (int i = 0; i < count; i++)
				{
					if (gridColumnStyles[i].PropertyDescriptor != null)
					{
						num3++;
					}
				}
			}
			if ((this.lastTotallyVisibleCol == num3 - 1 && columns > 0) || (this.firstVisibleCol == 0 && columns < 0 && this.negOffset == 0))
			{
				return;
			}
			num = Math.Min(num, count - 1);
			for (int j = 0; j < num; j++)
			{
				if (gridColumnStyles[j].PropertyDescriptor != null)
				{
					num2 += gridColumnStyles[j].Width;
				}
			}
			this.HorizontalOffset = num2;
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x000464D8 File Offset: 0x000446D8
		private void ScrollToColumn(int targetCol)
		{
			int num = targetCol - this.firstVisibleCol;
			if (targetCol > this.lastTotallyVisibleCol && this.lastTotallyVisibleCol != -1)
			{
				num = targetCol - this.lastTotallyVisibleCol;
			}
			if (num != 0 || this.negOffset != 0)
			{
				this.ScrollRight(num);
			}
		}

		/// <summary>Selects a specified row.</summary>
		/// <param name="row">The index of the row to select. </param>
		// Token: 0x06001275 RID: 4725 RVA: 0x0004651C File Offset: 0x0004471C
		public void Select(int row)
		{
			DataGridRow[] array = this.DataGridRows;
			if (!array[row].Selected)
			{
				array[row].Selected = true;
				this.numSelectedRows++;
			}
			this.EndEdit();
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x00046558 File Offset: 0x00044758
		private void PairTableStylesAndGridColumns(CurrencyManager lm, DataGridTableStyle gridTable, bool forceColumnCreation)
		{
			PropertyDescriptorCollection itemProperties = lm.GetItemProperties();
			GridColumnStylesCollection gridColumnStyles = gridTable.GridColumnStyles;
			if (gridTable.IsDefault || string.Compare(lm.GetListName(), gridTable.MappingName, true, CultureInfo.InvariantCulture) != 0)
			{
				gridTable.SetGridColumnStylesCollection(lm);
				if (gridTable.GridColumnStyles.Count > 0 && gridTable.GridColumnStyles[0].Width == -1)
				{
					this.InitializeColumnWidths();
				}
				return;
			}
			if (gridTable.GridColumnStyles.Count != 0 || base.DesignMode)
			{
				for (int i = 0; i < gridColumnStyles.Count; i++)
				{
					gridColumnStyles[i].PropertyDescriptor = null;
				}
				for (int j = 0; j < itemProperties.Count; j++)
				{
					DataGridColumnStyle dataGridColumnStyle = gridColumnStyles.MapColumnStyleToPropertyName(itemProperties[j].Name);
					if (dataGridColumnStyle != null)
					{
						dataGridColumnStyle.PropertyDescriptor = itemProperties[j];
					}
				}
				gridTable.SetRelationsList(lm);
				return;
			}
			if (forceColumnCreation)
			{
				gridTable.SetGridColumnStylesCollection(lm);
				return;
			}
			gridTable.SetRelationsList(lm);
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x00046650 File Offset: 0x00044850
		internal void SetDataGridTable(DataGridTableStyle newTable, bool forceColumnCreation)
		{
			if (this.myGridTable != null)
			{
				this.UnWireTableStylePropChanged(this.myGridTable);
				if (this.myGridTable.IsDefault)
				{
					this.myGridTable.GridColumnStyles.ResetPropertyDescriptors();
					this.myGridTable.ResetRelationsList();
				}
			}
			this.myGridTable = newTable;
			this.WireTableStylePropChanged(this.myGridTable);
			this.layout.RowHeadersVisible = (newTable.IsDefault ? this.RowHeadersVisible : newTable.RowHeadersVisible);
			if (newTable != null)
			{
				newTable.DataGrid = this;
			}
			if (this.listManager != null)
			{
				this.PairTableStylesAndGridColumns(this.listManager, this.myGridTable, forceColumnCreation);
			}
			if (newTable != null)
			{
				newTable.ResetRelationsUI();
			}
			this.gridState[16384] = false;
			this.horizScrollBar.Value = 0;
			this.firstVisibleRow = 0;
			this.currentCol = 0;
			if (this.listManager == null)
			{
				this.currentRow = 0;
			}
			else
			{
				this.currentRow = ((this.listManager.Position == -1) ? 0 : this.listManager.Position);
			}
			this.ResetHorizontalOffset();
			this.negOffset = 0;
			this.ResetUIState();
			this.checkHierarchy = true;
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x00046774 File Offset: 0x00044974
		internal void SetParentRowsVisibility(bool visible)
		{
			Rectangle rectangle = this.layout.ParentRows;
			Rectangle data = this.layout.Data;
			if (this.layout.RowHeadersVisible)
			{
				data.X -= (this.isRightToLeft() ? 0 : this.layout.RowHeaders.Width);
				data.Width += this.layout.RowHeaders.Width;
			}
			if (this.layout.ColumnHeadersVisible)
			{
				data.Y -= this.layout.ColumnHeaders.Height;
				data.Height += this.layout.ColumnHeaders.Height;
			}
			this.EndEdit();
			if (visible)
			{
				this.layout.ParentRowsVisible = true;
				base.PerformLayout();
				base.Invalidate();
				return;
			}
			NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(data.X, data.Y - this.layout.ParentRows.Height, data.Width, data.Height + this.layout.ParentRows.Height);
			SafeNativeMethods.ScrollWindow(new HandleRef(this, base.Handle), 0, -rectangle.Height, ref rect, ref rect);
			if (this.vertScrollBar.Visible)
			{
				Rectangle bounds = this.vertScrollBar.Bounds;
				bounds.Y -= rectangle.Height;
				bounds.Height += rectangle.Height;
				base.Invalidate(bounds);
			}
			this.layout.ParentRowsVisible = false;
			base.PerformLayout();
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x00046918 File Offset: 0x00044B18
		private void SetRowExpansionState(int row, bool expanded)
		{
			if (row < -1 || row > this.DataGridRowsLength - (this.policy.AllowAdd ? 2 : 1))
			{
				throw new ArgumentOutOfRangeException("row");
			}
			DataGridRow[] array = this.DataGridRows;
			if (row == -1)
			{
				DataGridRelationshipRow[] expandableRows = this.GetExpandableRows();
				bool flag = false;
				for (int i = 0; i < expandableRows.Length; i++)
				{
					if (expandableRows[i].Expanded != expanded)
					{
						expandableRows[i].Expanded = expanded;
						flag = true;
					}
				}
				if (flag && (this.gridState[16384] || this.gridState[32768]))
				{
					this.ResetSelection();
					this.Edit();
					return;
				}
			}
			else if (array[row] is DataGridRelationshipRow)
			{
				DataGridRelationshipRow dataGridRelationshipRow = (DataGridRelationshipRow)array[row];
				if (dataGridRelationshipRow.Expanded != expanded)
				{
					if (this.gridState[16384] || this.gridState[32768])
					{
						this.ResetSelection();
						this.Edit();
					}
					dataGridRelationshipRow.Expanded = expanded;
				}
			}
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x00046A14 File Offset: 0x00044C14
		private void ObjectSiteChange(IContainer container, IComponent component, bool site)
		{
			if (site)
			{
				if (component.Site == null)
				{
					container.Add(component);
					return;
				}
			}
			else if (component.Site != null && component.Site.Container == container)
			{
				container.Remove(component);
			}
		}

		/// <summary>Adds or removes the <see cref="T:System.Windows.Forms.DataGridTableStyle" /> objects from the container that is associated with the <see cref="T:System.Windows.Forms.DataGrid" />.</summary>
		/// <param name="site">
		///       <see langword="true" /> to add the <see cref="T:System.Windows.Forms.DataGridTableStyle" /> objects to a container; <see langword="false" /> to remove them.</param>
		// Token: 0x0600127B RID: 4731 RVA: 0x00046A48 File Offset: 0x00044C48
		public void SubObjectsSiteChange(bool site)
		{
			if (this.DesignMode && this.Site != null)
			{
				IDesignerHost designerHost = (IDesignerHost)this.Site.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					DesignerTransaction designerTransaction = designerHost.CreateTransaction();
					try
					{
						IContainer container = this.Site.Container;
						DataGridTableStyle[] array = new DataGridTableStyle[this.TableStyles.Count];
						this.TableStyles.CopyTo(array, 0);
						foreach (DataGridTableStyle dataGridTableStyle in array)
						{
							this.ObjectSiteChange(container, dataGridTableStyle, site);
							DataGridColumnStyle[] array2 = new DataGridColumnStyle[dataGridTableStyle.GridColumnStyles.Count];
							dataGridTableStyle.GridColumnStyles.CopyTo(array2, 0);
							foreach (DataGridColumnStyle component in array2)
							{
								this.ObjectSiteChange(container, component, site);
							}
						}
					}
					finally
					{
						designerTransaction.Commit();
					}
				}
			}
		}

		/// <summary>Unselects a specified row.</summary>
		/// <param name="row">The index of the row to deselect. </param>
		// Token: 0x0600127C RID: 4732 RVA: 0x00046B44 File Offset: 0x00044D44
		public void UnSelect(int row)
		{
			DataGridRow[] array = this.DataGridRows;
			if (array[row].Selected)
			{
				array[row].Selected = false;
				this.numSelectedRows--;
			}
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x00046B7C File Offset: 0x00044D7C
		private void UpdateListManager()
		{
			try
			{
				if (this.listManager != null)
				{
					this.EndEdit();
					this.listManager.EndCurrentEdit();
				}
			}
			catch
			{
			}
		}

		/// <summary>Gets the string that is the delimiter between columns when row contents are copied to the Clipboard.</summary>
		/// <returns>The string value "\t", which represents a tab used to separate columns in a row. </returns>
		// Token: 0x0600127E RID: 4734 RVA: 0x00046BB8 File Offset: 0x00044DB8
		protected virtual string GetOutputTextDelimiter()
		{
			return "\t";
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x00046BBF File Offset: 0x00044DBF
		private int MirrorRectangle(Rectangle R1, Rectangle rect, bool rightToLeft)
		{
			if (rightToLeft)
			{
				return rect.Right + rect.X - R1.Right;
			}
			return R1.X;
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x00046BE3 File Offset: 0x00044DE3
		private int MirrorPoint(int x, Rectangle rect, bool rightToLeft)
		{
			if (rightToLeft)
			{
				return rect.Right + rect.X - x;
			}
			return x;
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x00046BFB File Offset: 0x00044DFB
		private bool isRightToLeft()
		{
			return this.RightToLeft == RightToLeft.Yes;
		}

		// Token: 0x040008D4 RID: 2260
		internal TraceSwitch DataGridAcc;

		// Token: 0x040008D5 RID: 2261
		private const int GRIDSTATE_allowSorting = 1;

		// Token: 0x040008D6 RID: 2262
		private const int GRIDSTATE_columnHeadersVisible = 2;

		// Token: 0x040008D7 RID: 2263
		private const int GRIDSTATE_rowHeadersVisible = 4;

		// Token: 0x040008D8 RID: 2264
		private const int GRIDSTATE_trackColResize = 8;

		// Token: 0x040008D9 RID: 2265
		private const int GRIDSTATE_trackRowResize = 16;

		// Token: 0x040008DA RID: 2266
		private const int GRIDSTATE_isLedgerStyle = 32;

		// Token: 0x040008DB RID: 2267
		private const int GRIDSTATE_isFlatMode = 64;

		// Token: 0x040008DC RID: 2268
		private const int GRIDSTATE_listHasErrors = 128;

		// Token: 0x040008DD RID: 2269
		private const int GRIDSTATE_dragging = 256;

		// Token: 0x040008DE RID: 2270
		private const int GRIDSTATE_inListAddNew = 512;

		// Token: 0x040008DF RID: 2271
		private const int GRIDSTATE_inDeleteRow = 1024;

		// Token: 0x040008E0 RID: 2272
		private const int GRIDSTATE_canFocus = 2048;

		// Token: 0x040008E1 RID: 2273
		private const int GRIDSTATE_readOnlyMode = 4096;

		// Token: 0x040008E2 RID: 2274
		private const int GRIDSTATE_allowNavigation = 8192;

		// Token: 0x040008E3 RID: 2275
		private const int GRIDSTATE_isNavigating = 16384;

		// Token: 0x040008E4 RID: 2276
		private const int GRIDSTATE_isEditing = 32768;

		// Token: 0x040008E5 RID: 2277
		private const int GRIDSTATE_editControlChanging = 65536;

		// Token: 0x040008E6 RID: 2278
		private const int GRIDSTATE_isScrolling = 131072;

		// Token: 0x040008E7 RID: 2279
		private const int GRIDSTATE_overCaption = 262144;

		// Token: 0x040008E8 RID: 2280
		private const int GRIDSTATE_childLinkFocused = 524288;

		// Token: 0x040008E9 RID: 2281
		private const int GRIDSTATE_inAddNewRow = 1048576;

		// Token: 0x040008EA RID: 2282
		private const int GRIDSTATE_inSetListManager = 2097152;

		// Token: 0x040008EB RID: 2283
		private const int GRIDSTATE_metaDataChanged = 4194304;

		// Token: 0x040008EC RID: 2284
		private const int GRIDSTATE_exceptionInPaint = 8388608;

		// Token: 0x040008ED RID: 2285
		private const int GRIDSTATE_layoutSuspended = 16777216;

		// Token: 0x040008EE RID: 2286
		private BitVector32 gridState;

		// Token: 0x040008EF RID: 2287
		private const int NumRowsForAutoResize = 10;

		// Token: 0x040008F0 RID: 2288
		private const int errorRowBitmapWidth = 15;

		// Token: 0x040008F1 RID: 2289
		private const DataGridParentRowsLabelStyle defaultParentRowsLabelStyle = DataGridParentRowsLabelStyle.Both;

		// Token: 0x040008F2 RID: 2290
		private const BorderStyle defaultBorderStyle = BorderStyle.Fixed3D;

		// Token: 0x040008F3 RID: 2291
		private const bool defaultCaptionVisible = true;

		// Token: 0x040008F4 RID: 2292
		private const bool defaultParentRowsVisible = true;

		// Token: 0x040008F5 RID: 2293
		private DataGridTableStyle defaultTableStyle = new DataGridTableStyle(true);

		// Token: 0x040008F6 RID: 2294
		private SolidBrush alternatingBackBrush = DataGrid.DefaultAlternatingBackBrush;

		// Token: 0x040008F7 RID: 2295
		private SolidBrush gridLineBrush = DataGrid.DefaultGridLineBrush;

		// Token: 0x040008F8 RID: 2296
		private const DataGridLineStyle defaultGridLineStyle = DataGridLineStyle.Solid;

		// Token: 0x040008F9 RID: 2297
		private DataGridLineStyle gridLineStyle = DataGridLineStyle.Solid;

		// Token: 0x040008FA RID: 2298
		private SolidBrush headerBackBrush = DataGrid.DefaultHeaderBackBrush;

		// Token: 0x040008FB RID: 2299
		private Font headerFont;

		// Token: 0x040008FC RID: 2300
		private SolidBrush headerForeBrush = DataGrid.DefaultHeaderForeBrush;

		// Token: 0x040008FD RID: 2301
		private Pen headerForePen = DataGrid.DefaultHeaderForePen;

		// Token: 0x040008FE RID: 2302
		private SolidBrush linkBrush = DataGrid.DefaultLinkBrush;

		// Token: 0x040008FF RID: 2303
		private const int defaultPreferredColumnWidth = 75;

		// Token: 0x04000900 RID: 2304
		private int preferredColumnWidth = 75;

		// Token: 0x04000901 RID: 2305
		private static int defaultFontHeight = Control.DefaultFont.Height;

		// Token: 0x04000902 RID: 2306
		private int prefferedRowHeight = DataGrid.defaultFontHeight + 3;

		// Token: 0x04000903 RID: 2307
		private const int defaultRowHeaderWidth = 35;

		// Token: 0x04000904 RID: 2308
		private int rowHeaderWidth = 35;

		// Token: 0x04000905 RID: 2309
		private int minRowHeaderWidth;

		// Token: 0x04000906 RID: 2310
		private SolidBrush selectionBackBrush = DataGrid.DefaultSelectionBackBrush;

		// Token: 0x04000907 RID: 2311
		private SolidBrush selectionForeBrush = DataGrid.DefaultSelectionForeBrush;

		// Token: 0x04000908 RID: 2312
		private DataGridParentRows parentRows;

		// Token: 0x04000909 RID: 2313
		private DataGridState originalState;

		// Token: 0x0400090A RID: 2314
		private DataGridRow[] dataGridRows = new DataGridRow[0];

		// Token: 0x0400090B RID: 2315
		private int dataGridRowsLength;

		// Token: 0x0400090C RID: 2316
		private int toolTipId;

		// Token: 0x0400090D RID: 2317
		private DataGridToolTip toolTipProvider;

		// Token: 0x0400090E RID: 2318
		private DataGridAddNewRow addNewRow;

		// Token: 0x0400090F RID: 2319
		private DataGrid.LayoutData layout = new DataGrid.LayoutData();

		// Token: 0x04000910 RID: 2320
		private NativeMethods.RECT[] cachedScrollableRegion;

		// Token: 0x04000911 RID: 2321
		internal bool allowColumnResize = true;

		// Token: 0x04000912 RID: 2322
		internal bool allowRowResize = true;

		// Token: 0x04000913 RID: 2323
		internal DataGridParentRowsLabelStyle parentRowsLabels = DataGridParentRowsLabelStyle.Both;

		// Token: 0x04000914 RID: 2324
		private int trackColAnchor;

		// Token: 0x04000915 RID: 2325
		private int trackColumn;

		// Token: 0x04000916 RID: 2326
		private int trackRowAnchor;

		// Token: 0x04000917 RID: 2327
		private int trackRow;

		// Token: 0x04000918 RID: 2328
		private PropertyDescriptor trackColumnHeader;

		// Token: 0x04000919 RID: 2329
		private MouseEventArgs lastSplitBar;

		// Token: 0x0400091A RID: 2330
		private Font linkFont;

		// Token: 0x0400091B RID: 2331
		private SolidBrush backBrush = DataGrid.DefaultBackBrush;

		// Token: 0x0400091C RID: 2332
		private SolidBrush foreBrush = DataGrid.DefaultForeBrush;

		// Token: 0x0400091D RID: 2333
		private SolidBrush backgroundBrush = DataGrid.DefaultBackgroundBrush;

		// Token: 0x0400091E RID: 2334
		private int fontHeight = -1;

		// Token: 0x0400091F RID: 2335
		private int linkFontHeight = -1;

		// Token: 0x04000920 RID: 2336
		private int captionFontHeight = -1;

		// Token: 0x04000921 RID: 2337
		private int headerFontHeight = -1;

		// Token: 0x04000922 RID: 2338
		private DataGridCaption caption;

		// Token: 0x04000923 RID: 2339
		private BorderStyle borderStyle;

		// Token: 0x04000924 RID: 2340
		private object dataSource;

		// Token: 0x04000925 RID: 2341
		private string dataMember = "";

		// Token: 0x04000926 RID: 2342
		private CurrencyManager listManager;

		// Token: 0x04000927 RID: 2343
		private Control toBeDisposedEditingControl;

		// Token: 0x04000928 RID: 2344
		internal GridTableStylesCollection dataGridTables;

		// Token: 0x04000929 RID: 2345
		internal DataGridTableStyle myGridTable;

		// Token: 0x0400092A RID: 2346
		internal bool checkHierarchy = true;

		// Token: 0x0400092B RID: 2347
		internal bool inInit;

		// Token: 0x0400092C RID: 2348
		internal int currentRow;

		// Token: 0x0400092D RID: 2349
		internal int currentCol;

		// Token: 0x0400092E RID: 2350
		private int numSelectedRows;

		// Token: 0x0400092F RID: 2351
		private int lastRowSelected = -1;

		// Token: 0x04000930 RID: 2352
		private DataGrid.Policy policy = new DataGrid.Policy();

		// Token: 0x04000931 RID: 2353
		private DataGridColumnStyle editColumn;

		// Token: 0x04000932 RID: 2354
		private DataGridRow editRow;

		// Token: 0x04000933 RID: 2355
		private ScrollBar horizScrollBar = new HScrollBar();

		// Token: 0x04000934 RID: 2356
		private ScrollBar vertScrollBar = new VScrollBar();

		// Token: 0x04000935 RID: 2357
		private int horizontalOffset;

		// Token: 0x04000936 RID: 2358
		private int negOffset;

		// Token: 0x04000937 RID: 2359
		private int wheelDelta;

		// Token: 0x04000938 RID: 2360
		internal int firstVisibleRow;

		// Token: 0x04000939 RID: 2361
		internal int firstVisibleCol;

		// Token: 0x0400093A RID: 2362
		private int numVisibleRows;

		// Token: 0x0400093B RID: 2363
		private int numVisibleCols;

		// Token: 0x0400093C RID: 2364
		private int numTotallyVisibleRows;

		// Token: 0x0400093D RID: 2365
		private int lastTotallyVisibleCol;

		// Token: 0x0400093E RID: 2366
		private int oldRow = -1;

		// Token: 0x0400093F RID: 2367
		private static readonly object EVENT_CURRENTCELLCHANGED = new object();

		// Token: 0x04000940 RID: 2368
		private static readonly object EVENT_NODECLICKED = new object();

		// Token: 0x04000941 RID: 2369
		private static readonly object EVENT_SCROLL = new object();

		// Token: 0x04000942 RID: 2370
		private static readonly object EVENT_BACKBUTTONCLICK = new object();

		// Token: 0x04000943 RID: 2371
		private static readonly object EVENT_DOWNBUTTONCLICK = new object();

		// Token: 0x04000944 RID: 2372
		private ItemChangedEventHandler itemChangedHandler;

		// Token: 0x04000945 RID: 2373
		private EventHandler positionChangedHandler;

		// Token: 0x04000946 RID: 2374
		private EventHandler currentChangedHandler;

		// Token: 0x04000947 RID: 2375
		private EventHandler metaDataChangedHandler;

		// Token: 0x04000948 RID: 2376
		private CollectionChangeEventHandler dataGridTableStylesCollectionChanged;

		// Token: 0x04000949 RID: 2377
		private EventHandler backButtonHandler;

		// Token: 0x0400094A RID: 2378
		private EventHandler downButtonHandler;

		// Token: 0x0400094B RID: 2379
		private NavigateEventHandler onNavigate;

		// Token: 0x0400094C RID: 2380
		private EventHandler onRowHeaderClick;

		// Token: 0x0400094D RID: 2381
		private static readonly object EVENT_BORDERSTYLECHANGED = new object();

		// Token: 0x0400094E RID: 2382
		private static readonly object EVENT_CAPTIONVISIBLECHANGED = new object();

		// Token: 0x0400094F RID: 2383
		private static readonly object EVENT_DATASOURCECHANGED = new object();

		// Token: 0x04000950 RID: 2384
		private static readonly object EVENT_PARENTROWSLABELSTYLECHANGED = new object();

		// Token: 0x04000951 RID: 2385
		private static readonly object EVENT_FLATMODECHANGED = new object();

		// Token: 0x04000952 RID: 2386
		private static readonly object EVENT_BACKGROUNDCOLORCHANGED = new object();

		// Token: 0x04000953 RID: 2387
		private static readonly object EVENT_ALLOWNAVIGATIONCHANGED = new object();

		// Token: 0x04000954 RID: 2388
		private static readonly object EVENT_READONLYCHANGED = new object();

		// Token: 0x04000955 RID: 2389
		private static readonly object EVENT_PARENTROWSVISIBLECHANGED = new object();

		// Token: 0x0200058A RID: 1418
		[ComVisible(true)]
		internal class DataGridAccessibleObject : Control.ControlAccessibleObject
		{
			// Token: 0x060057D5 RID: 22485 RVA: 0x00093572 File Offset: 0x00091772
			public DataGridAccessibleObject(DataGrid owner) : base(owner)
			{
			}

			// Token: 0x170014F5 RID: 5365
			// (get) Token: 0x060057D6 RID: 22486 RVA: 0x00171A58 File Offset: 0x0016FC58
			internal DataGrid DataGrid
			{
				get
				{
					return (DataGrid)base.Owner;
				}
			}

			// Token: 0x170014F6 RID: 5366
			// (get) Token: 0x060057D7 RID: 22487 RVA: 0x00171A65 File Offset: 0x0016FC65
			private int ColumnCountPrivate
			{
				get
				{
					return ((DataGrid)base.Owner).myGridTable.GridColumnStyles.Count;
				}
			}

			// Token: 0x170014F7 RID: 5367
			// (get) Token: 0x060057D8 RID: 22488 RVA: 0x00171A81 File Offset: 0x0016FC81
			private int RowCountPrivate
			{
				get
				{
					return ((DataGrid)base.Owner).dataGridRows.Length;
				}
			}

			// Token: 0x170014F8 RID: 5368
			// (get) Token: 0x060057D9 RID: 22489 RVA: 0x00171A98 File Offset: 0x0016FC98
			// (set) Token: 0x060057DA RID: 22490 RVA: 0x00171ABB File Offset: 0x0016FCBB
			public override string Name
			{
				get
				{
					string accessibleName = base.Owner.AccessibleName;
					if (accessibleName != null)
					{
						return accessibleName;
					}
					return "DataGrid";
				}
				set
				{
					base.Owner.AccessibleName = value;
				}
			}

			// Token: 0x170014F9 RID: 5369
			// (get) Token: 0x060057DB RID: 22491 RVA: 0x00171ACC File Offset: 0x0016FCCC
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.Table;
				}
			}

			// Token: 0x060057DC RID: 22492 RVA: 0x00171AF0 File Offset: 0x0016FCF0
			public override AccessibleObject GetChild(int index)
			{
				DataGrid dataGrid = (DataGrid)base.Owner;
				int columnCountPrivate = this.ColumnCountPrivate;
				int rowCountPrivate = this.RowCountPrivate;
				if (dataGrid.dataGridRows == null)
				{
					dataGrid.CreateDataGridRows();
				}
				if (index < 1)
				{
					return dataGrid.ParentRowsAccessibleObject;
				}
				index--;
				if (index < columnCountPrivate)
				{
					return dataGrid.myGridTable.GridColumnStyles[index].HeaderAccessibleObject;
				}
				index -= columnCountPrivate;
				if (index < rowCountPrivate)
				{
					return dataGrid.dataGridRows[index].AccessibleObject;
				}
				index -= rowCountPrivate;
				if (dataGrid.horizScrollBar.Visible)
				{
					if (index == 0)
					{
						return dataGrid.horizScrollBar.AccessibilityObject;
					}
					index--;
				}
				if (dataGrid.vertScrollBar.Visible)
				{
					if (index == 0)
					{
						return dataGrid.vertScrollBar.AccessibilityObject;
					}
					index--;
				}
				int count = dataGrid.myGridTable.GridColumnStyles.Count;
				int num = dataGrid.dataGridRows.Length;
				int num2 = index / count;
				int num3 = index % count;
				if (num2 < dataGrid.dataGridRows.Length && num3 < dataGrid.myGridTable.GridColumnStyles.Count)
				{
					return dataGrid.dataGridRows[num2].AccessibleObject.GetChild(num3);
				}
				return null;
			}

			// Token: 0x060057DD RID: 22493 RVA: 0x00171C0C File Offset: 0x0016FE0C
			public override int GetChildCount()
			{
				int num = 1 + this.ColumnCountPrivate + ((DataGrid)base.Owner).DataGridRowsLength;
				if (this.DataGrid.horizScrollBar.Visible)
				{
					num++;
				}
				if (this.DataGrid.vertScrollBar.Visible)
				{
					num++;
				}
				return num + this.DataGrid.DataGridRows.Length * this.DataGrid.myGridTable.GridColumnStyles.Count;
			}

			// Token: 0x060057DE RID: 22494 RVA: 0x00171C86 File Offset: 0x0016FE86
			public override AccessibleObject GetFocused()
			{
				if (this.DataGrid.Focused)
				{
					return this.GetSelected();
				}
				return null;
			}

			// Token: 0x060057DF RID: 22495 RVA: 0x00171CA0 File Offset: 0x0016FEA0
			public override AccessibleObject GetSelected()
			{
				if (this.DataGrid.DataGridRows.Length == 0 || this.DataGrid.myGridTable.GridColumnStyles.Count == 0)
				{
					return null;
				}
				DataGridCell currentCell = this.DataGrid.CurrentCell;
				return this.GetChild(1 + this.ColumnCountPrivate + currentCell.RowNumber).GetChild(currentCell.ColumnNumber);
			}

			// Token: 0x060057E0 RID: 22496 RVA: 0x00171D04 File Offset: 0x0016FF04
			public override AccessibleObject HitTest(int x, int y)
			{
				Point point = this.DataGrid.PointToClient(new Point(x, y));
				DataGrid.HitTestInfo hitTestInfo = this.DataGrid.HitTest(point.X, point.Y);
				DataGrid.HitTestType type = hitTestInfo.Type;
				if (type <= DataGrid.HitTestType.RowResize)
				{
					switch (type)
					{
					case DataGrid.HitTestType.None:
					case DataGrid.HitTestType.Cell | DataGrid.HitTestType.ColumnHeader:
					case DataGrid.HitTestType.Cell | DataGrid.HitTestType.RowHeader:
					case DataGrid.HitTestType.ColumnHeader | DataGrid.HitTestType.RowHeader:
					case DataGrid.HitTestType.Cell | DataGrid.HitTestType.ColumnHeader | DataGrid.HitTestType.RowHeader:
					case DataGrid.HitTestType.ColumnResize:
						break;
					case DataGrid.HitTestType.Cell:
						return this.GetChild(1 + this.ColumnCountPrivate + hitTestInfo.Row).GetChild(hitTestInfo.Column);
					case DataGrid.HitTestType.ColumnHeader:
						return this.GetChild(1 + hitTestInfo.Column);
					case DataGrid.HitTestType.RowHeader:
						return this.GetChild(1 + this.ColumnCountPrivate + hitTestInfo.Row);
					default:
						if (type != DataGrid.HitTestType.RowResize)
						{
						}
						break;
					}
				}
				else if (type != DataGrid.HitTestType.Caption)
				{
					if (type == DataGrid.HitTestType.ParentRows)
					{
						return this.DataGrid.ParentRowsAccessibleObject;
					}
				}
				return null;
			}

			// Token: 0x060057E1 RID: 22497 RVA: 0x00171DDA File Offset: 0x0016FFDA
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navdir)
			{
				if (this.GetChildCount() > 0)
				{
					if (navdir == AccessibleNavigation.FirstChild)
					{
						return this.GetChild(0);
					}
					if (navdir == AccessibleNavigation.LastChild)
					{
						return this.GetChild(this.GetChildCount() - 1);
					}
				}
				return null;
			}
		}

		// Token: 0x0200058B RID: 1419
		internal class LayoutData
		{
			// Token: 0x060057E2 RID: 22498 RVA: 0x00171E08 File Offset: 0x00170008
			public LayoutData()
			{
			}

			// Token: 0x060057E3 RID: 22499 RVA: 0x00171E88 File Offset: 0x00170088
			public LayoutData(DataGrid.LayoutData src)
			{
				this.GrabLayout(src);
			}

			// Token: 0x060057E4 RID: 22500 RVA: 0x00171F0C File Offset: 0x0017010C
			private void GrabLayout(DataGrid.LayoutData src)
			{
				this.Inside = src.Inside;
				this.TopLeftHeader = src.TopLeftHeader;
				this.ColumnHeaders = src.ColumnHeaders;
				this.RowHeaders = src.RowHeaders;
				this.Data = src.Data;
				this.Caption = src.Caption;
				this.ParentRows = src.ParentRows;
				this.ResizeBoxRect = src.ResizeBoxRect;
				this.ColumnHeadersVisible = src.ColumnHeadersVisible;
				this.RowHeadersVisible = src.RowHeadersVisible;
				this.CaptionVisible = src.CaptionVisible;
				this.ParentRowsVisible = src.ParentRowsVisible;
				this.ClientRectangle = src.ClientRectangle;
			}

			// Token: 0x060057E5 RID: 22501 RVA: 0x00171FB8 File Offset: 0x001701B8
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder(200);
				stringBuilder.Append(base.ToString());
				stringBuilder.Append(" { \n");
				stringBuilder.Append("Inside = ");
				stringBuilder.Append(this.Inside.ToString());
				stringBuilder.Append('\n');
				stringBuilder.Append("TopLeftHeader = ");
				stringBuilder.Append(this.TopLeftHeader.ToString());
				stringBuilder.Append('\n');
				stringBuilder.Append("ColumnHeaders = ");
				stringBuilder.Append(this.ColumnHeaders.ToString());
				stringBuilder.Append('\n');
				stringBuilder.Append("RowHeaders = ");
				stringBuilder.Append(this.RowHeaders.ToString());
				stringBuilder.Append('\n');
				stringBuilder.Append("Data = ");
				stringBuilder.Append(this.Data.ToString());
				stringBuilder.Append('\n');
				stringBuilder.Append("Caption = ");
				stringBuilder.Append(this.Caption.ToString());
				stringBuilder.Append('\n');
				stringBuilder.Append("ParentRows = ");
				stringBuilder.Append(this.ParentRows.ToString());
				stringBuilder.Append('\n');
				stringBuilder.Append("ResizeBoxRect = ");
				stringBuilder.Append(this.ResizeBoxRect.ToString());
				stringBuilder.Append('\n');
				stringBuilder.Append("ColumnHeadersVisible = ");
				stringBuilder.Append(this.ColumnHeadersVisible.ToString());
				stringBuilder.Append('\n');
				stringBuilder.Append("RowHeadersVisible = ");
				stringBuilder.Append(this.RowHeadersVisible.ToString());
				stringBuilder.Append('\n');
				stringBuilder.Append("CaptionVisible = ");
				stringBuilder.Append(this.CaptionVisible.ToString());
				stringBuilder.Append('\n');
				stringBuilder.Append("ParentRowsVisible = ");
				stringBuilder.Append(this.ParentRowsVisible.ToString());
				stringBuilder.Append('\n');
				stringBuilder.Append("ClientRectangle = ");
				stringBuilder.Append(this.ClientRectangle.ToString());
				stringBuilder.Append(" } ");
				return stringBuilder.ToString();
			}

			// Token: 0x04003879 RID: 14457
			internal bool dirty = true;

			// Token: 0x0400387A RID: 14458
			public Rectangle Inside = Rectangle.Empty;

			// Token: 0x0400387B RID: 14459
			public Rectangle RowHeaders = Rectangle.Empty;

			// Token: 0x0400387C RID: 14460
			public Rectangle TopLeftHeader = Rectangle.Empty;

			// Token: 0x0400387D RID: 14461
			public Rectangle ColumnHeaders = Rectangle.Empty;

			// Token: 0x0400387E RID: 14462
			public Rectangle Data = Rectangle.Empty;

			// Token: 0x0400387F RID: 14463
			public Rectangle Caption = Rectangle.Empty;

			// Token: 0x04003880 RID: 14464
			public Rectangle ParentRows = Rectangle.Empty;

			// Token: 0x04003881 RID: 14465
			public Rectangle ResizeBoxRect = Rectangle.Empty;

			// Token: 0x04003882 RID: 14466
			public bool ColumnHeadersVisible;

			// Token: 0x04003883 RID: 14467
			public bool RowHeadersVisible;

			// Token: 0x04003884 RID: 14468
			public bool CaptionVisible;

			// Token: 0x04003885 RID: 14469
			public bool ParentRowsVisible;

			// Token: 0x04003886 RID: 14470
			public Rectangle ClientRectangle = Rectangle.Empty;
		}

		/// <summary>Contains information about a part of the <see cref="T:System.Windows.Forms.DataGrid" /> at a specified coordinate. This class cannot be inherited.</summary>
		// Token: 0x0200058C RID: 1420
		public sealed class HitTestInfo
		{
			// Token: 0x060057E6 RID: 22502 RVA: 0x00172224 File Offset: 0x00170424
			internal HitTestInfo()
			{
				this.type = DataGrid.HitTestType.None;
				this.row = (this.col = -1);
			}

			// Token: 0x060057E7 RID: 22503 RVA: 0x00172250 File Offset: 0x00170450
			internal HitTestInfo(DataGrid.HitTestType type)
			{
				this.type = type;
				this.row = (this.col = -1);
			}

			/// <summary>Gets the number of the column the user has clicked.</summary>
			/// <returns>The number of the column.</returns>
			// Token: 0x170014FA RID: 5370
			// (get) Token: 0x060057E8 RID: 22504 RVA: 0x0017227A File Offset: 0x0017047A
			public int Column
			{
				get
				{
					return this.col;
				}
			}

			/// <summary>Gets the number of the row the user has clicked.</summary>
			/// <returns>The number of the clicked row.</returns>
			// Token: 0x170014FB RID: 5371
			// (get) Token: 0x060057E9 RID: 22505 RVA: 0x00172282 File Offset: 0x00170482
			public int Row
			{
				get
				{
					return this.row;
				}
			}

			/// <summary>Gets the part of the <see cref="T:System.Windows.Forms.DataGrid" /> control, other than the row or column, that was clicked.</summary>
			/// <returns>One of the <see cref="T:System.Windows.Forms.DataGrid.HitTestType" /> enumerations.</returns>
			// Token: 0x170014FC RID: 5372
			// (get) Token: 0x060057EA RID: 22506 RVA: 0x0017228A File Offset: 0x0017048A
			public DataGrid.HitTestType Type
			{
				get
				{
					return this.type;
				}
			}

			/// <summary>Indicates whether two objects are identical.</summary>
			/// <param name="value">The second object to compare, typed as <see cref="T:System.Object" />. </param>
			/// <returns>
			///     <see langword="true" /> if the objects are equal; otherwise, <see langword="false" />.</returns>
			// Token: 0x060057EB RID: 22507 RVA: 0x00172294 File Offset: 0x00170494
			public override bool Equals(object value)
			{
				if (value is DataGrid.HitTestInfo)
				{
					DataGrid.HitTestInfo hitTestInfo = (DataGrid.HitTestInfo)value;
					return this.type == hitTestInfo.type && this.row == hitTestInfo.row && this.col == hitTestInfo.col;
				}
				return false;
			}

			/// <summary>Gets the hash code for the <see cref="T:System.Windows.Forms.DataGrid.HitTestInfo" /> instance.</summary>
			/// <returns>The hash code for this instance.</returns>
			// Token: 0x060057EC RID: 22508 RVA: 0x001722DE File Offset: 0x001704DE
			public override int GetHashCode()
			{
				return (int)(this.type + (this.row << 8) + (this.col << 16));
			}

			/// <summary>Gets the type, row number, and column number.</summary>
			/// <returns>The type, row number, and column number.</returns>
			// Token: 0x060057ED RID: 22509 RVA: 0x001722FC File Offset: 0x001704FC
			public override string ToString()
			{
				return string.Concat(new string[]
				{
					"{ ",
					this.type.ToString(),
					",",
					this.row.ToString(CultureInfo.InvariantCulture),
					",",
					this.col.ToString(CultureInfo.InvariantCulture),
					"}"
				});
			}

			// Token: 0x04003887 RID: 14471
			internal DataGrid.HitTestType type;

			// Token: 0x04003888 RID: 14472
			internal int row;

			// Token: 0x04003889 RID: 14473
			internal int col;

			/// <summary>Indicates that a coordinate corresponds to part of the <see cref="T:System.Windows.Forms.DataGrid" /> control that is not functioning.</summary>
			// Token: 0x0400388A RID: 14474
			public static readonly DataGrid.HitTestInfo Nowhere = new DataGrid.HitTestInfo();
		}

		/// <summary>Specifies the part of the <see cref="T:System.Windows.Forms.DataGrid" /> control the user has clicked.</summary>
		// Token: 0x0200058D RID: 1421
		[Flags]
		public enum HitTestType
		{
			/// <summary>The background area, visible when the control contains no table, few rows, or when a table is scrolled to its bottom.</summary>
			// Token: 0x0400388C RID: 14476
			None = 0,
			/// <summary>A cell in the <see cref="T:System.Windows.Forms.DataGrid" /> control.</summary>
			// Token: 0x0400388D RID: 14477
			Cell = 1,
			/// <summary>A column header in the <see cref="T:System.Windows.Forms.DataGrid" /> control.</summary>
			// Token: 0x0400388E RID: 14478
			ColumnHeader = 2,
			/// <summary>A row header in the <see cref="T:System.Windows.Forms.DataGrid" /> control.</summary>
			// Token: 0x0400388F RID: 14479
			RowHeader = 4,
			/// <summary>The column border, which is the line between column headers. It can be dragged to resize a column's width.</summary>
			// Token: 0x04003890 RID: 14480
			ColumnResize = 8,
			/// <summary>The row border, which is the line between grid row headers. It can be dragged to resize a row's height.</summary>
			// Token: 0x04003891 RID: 14481
			RowResize = 16,
			/// <summary>The caption of the <see cref="T:System.Windows.Forms.DataGrid" /> control.</summary>
			// Token: 0x04003892 RID: 14482
			Caption = 32,
			/// <summary>The parent row section of the <see cref="T:System.Windows.Forms.DataGrid" /> control. The parent row displays information from or about the parent table of the currently displayed child table, such as the name of the parent table, column names and values of the parent record.</summary>
			// Token: 0x04003893 RID: 14483
			ParentRows = 64
		}

		// Token: 0x0200058E RID: 1422
		private class Policy
		{
			// Token: 0x170014FD RID: 5373
			// (get) Token: 0x060057F0 RID: 22512 RVA: 0x00172397 File Offset: 0x00170597
			// (set) Token: 0x060057F1 RID: 22513 RVA: 0x0017239F File Offset: 0x0017059F
			public bool AllowAdd
			{
				get
				{
					return this.allowAdd;
				}
				set
				{
					if (this.allowAdd != value)
					{
						this.allowAdd = value;
					}
				}
			}

			// Token: 0x170014FE RID: 5374
			// (get) Token: 0x060057F2 RID: 22514 RVA: 0x001723B1 File Offset: 0x001705B1
			// (set) Token: 0x060057F3 RID: 22515 RVA: 0x001723B9 File Offset: 0x001705B9
			public bool AllowEdit
			{
				get
				{
					return this.allowEdit;
				}
				set
				{
					if (this.allowEdit != value)
					{
						this.allowEdit = value;
					}
				}
			}

			// Token: 0x170014FF RID: 5375
			// (get) Token: 0x060057F4 RID: 22516 RVA: 0x001723CB File Offset: 0x001705CB
			// (set) Token: 0x060057F5 RID: 22517 RVA: 0x001723D3 File Offset: 0x001705D3
			public bool AllowRemove
			{
				get
				{
					return this.allowRemove;
				}
				set
				{
					if (this.allowRemove != value)
					{
						this.allowRemove = value;
					}
				}
			}

			// Token: 0x060057F6 RID: 22518 RVA: 0x001723E8 File Offset: 0x001705E8
			public bool UpdatePolicy(CurrencyManager listManager, bool gridReadOnly)
			{
				bool result = false;
				IBindingList bindingList = (listManager == null) ? null : (listManager.List as IBindingList);
				if (listManager == null)
				{
					if (!this.allowAdd)
					{
						result = true;
					}
					this.allowAdd = (this.allowEdit = (this.allowRemove = true));
				}
				else
				{
					if (this.AllowAdd != listManager.AllowAdd && !gridReadOnly)
					{
						result = true;
					}
					this.AllowAdd = (listManager.AllowAdd && !gridReadOnly && bindingList != null && bindingList.SupportsChangeNotification);
					this.AllowEdit = (listManager.AllowEdit && !gridReadOnly);
					this.AllowRemove = (listManager.AllowRemove && !gridReadOnly && bindingList != null && bindingList.SupportsChangeNotification);
				}
				return result;
			}

			// Token: 0x04003894 RID: 14484
			private bool allowAdd = true;

			// Token: 0x04003895 RID: 14485
			private bool allowEdit = true;

			// Token: 0x04003896 RID: 14486
			private bool allowRemove = true;
		}
	}
}
