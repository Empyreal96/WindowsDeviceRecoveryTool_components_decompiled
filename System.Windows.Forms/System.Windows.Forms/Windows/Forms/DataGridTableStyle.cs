using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace System.Windows.Forms
{
	/// <summary>Represents the table drawn by the <see cref="T:System.Windows.Forms.DataGrid" /> control at run time.</summary>
	// Token: 0x02000179 RID: 377
	[ToolboxItem(false)]
	[DesignTimeVisible(false)]
	public class DataGridTableStyle : Component, IDataGridEditingService
	{
		/// <summary>Indicates whether sorting is allowed on the grid table when this <see cref="T:System.Windows.Forms.DataGridTableStyle" /> is used.</summary>
		/// <returns>
		///     <see langword="true" /> if sorting is allowed; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x0600141E RID: 5150 RVA: 0x0004CBE4 File Offset: 0x0004ADE4
		// (set) Token: 0x0600141F RID: 5151 RVA: 0x0004CBEC File Offset: 0x0004ADEC
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("DataGridAllowSortingDescr")]
		public bool AllowSorting
		{
			get
			{
				return this.allowSorting;
			}
			set
			{
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[]
					{
						"AllowSorting"
					}));
				}
				if (this.allowSorting != value)
				{
					this.allowSorting = value;
					this.OnAllowSortingChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridTableStyle.AllowSorting" /> property value changes.</summary>
		// Token: 0x140000D5 RID: 213
		// (add) Token: 0x06001420 RID: 5152 RVA: 0x0004CC3A File Offset: 0x0004AE3A
		// (remove) Token: 0x06001421 RID: 5153 RVA: 0x0004CC4D File Offset: 0x0004AE4D
		public event EventHandler AllowSortingChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventAllowSorting, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventAllowSorting, value);
			}
		}

		/// <summary>Gets or sets the background color of odd-numbered rows of the grid.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background color of odd-numbered rows. The default is <see cref="P:System.Drawing.SystemBrushes.Window" /></returns>
		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06001422 RID: 5154 RVA: 0x0004CC60 File Offset: 0x0004AE60
		// (set) Token: 0x06001423 RID: 5155 RVA: 0x0004CC70 File Offset: 0x0004AE70
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
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[]
					{
						"AlternatingBackColor"
					}));
				}
				if (DataGrid.IsTransparentColor(value))
				{
					throw new ArgumentException(SR.GetString("DataGridTableStyleTransparentAlternatingBackColorNotAllowed"));
				}
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[]
					{
						"AlternatingBackColor"
					}));
				}
				if (!this.alternatingBackBrush.Color.Equals(value))
				{
					this.alternatingBackBrush = new SolidBrush(value);
					this.InvalidateInside();
					this.OnAlternatingBackColorChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridTableStyle.AlternatingBackColor" /> value changes.</summary>
		// Token: 0x140000D6 RID: 214
		// (add) Token: 0x06001424 RID: 5156 RVA: 0x0004CD20 File Offset: 0x0004AF20
		// (remove) Token: 0x06001425 RID: 5157 RVA: 0x0004CD33 File Offset: 0x0004AF33
		public event EventHandler AlternatingBackColorChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventAlternatingBackColor, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventAlternatingBackColor, value);
			}
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.DataGridTableStyle.AlternatingBackColor" /> property to its default value.</summary>
		// Token: 0x06001426 RID: 5158 RVA: 0x0004CD46 File Offset: 0x0004AF46
		public void ResetAlternatingBackColor()
		{
			if (this.ShouldSerializeAlternatingBackColor())
			{
				this.AlternatingBackColor = DataGridTableStyle.DefaultAlternatingBackBrush.Color;
				this.InvalidateInside();
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Forms.DataGridTableStyle.AlternatingBackColor" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001427 RID: 5159 RVA: 0x0004CD66 File Offset: 0x0004AF66
		protected virtual bool ShouldSerializeAlternatingBackColor()
		{
			return !this.AlternatingBackBrush.Equals(DataGridTableStyle.DefaultAlternatingBackBrush);
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06001428 RID: 5160 RVA: 0x0004CD7B File Offset: 0x0004AF7B
		internal SolidBrush AlternatingBackBrush
		{
			get
			{
				return this.alternatingBackBrush;
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Forms.DataGridTableStyle.BackColor" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001429 RID: 5161 RVA: 0x0004CD83 File Offset: 0x0004AF83
		protected bool ShouldSerializeBackColor()
		{
			return !DataGridTableStyle.DefaultBackBrush.Equals(this.backBrush);
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Forms.DataGridTableStyle.ForeColor" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600142A RID: 5162 RVA: 0x0004CD98 File Offset: 0x0004AF98
		protected bool ShouldSerializeForeColor()
		{
			return !DataGridTableStyle.DefaultForeBrush.Equals(this.foreBrush);
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x0600142B RID: 5163 RVA: 0x0004CDAD File Offset: 0x0004AFAD
		internal SolidBrush BackBrush
		{
			get
			{
				return this.backBrush;
			}
		}

		/// <summary>Gets or sets the background color of even-numbered rows of the grid.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background color of odd-numbered rows.</returns>
		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x0600142C RID: 5164 RVA: 0x0004CDB5 File Offset: 0x0004AFB5
		// (set) Token: 0x0600142D RID: 5165 RVA: 0x0004CDC4 File Offset: 0x0004AFC4
		[SRCategory("CatColors")]
		[SRDescription("ControlBackColorDescr")]
		public Color BackColor
		{
			get
			{
				return this.backBrush.Color;
			}
			set
			{
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[]
					{
						"BackColor"
					}));
				}
				if (DataGrid.IsTransparentColor(value))
				{
					throw new ArgumentException(SR.GetString("DataGridTableStyleTransparentBackColorNotAllowed"));
				}
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[]
					{
						"BackColor"
					}));
				}
				if (!this.backBrush.Color.Equals(value))
				{
					this.backBrush = new SolidBrush(value);
					this.InvalidateInside();
					this.OnBackColorChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridTableStyle.BackColor" /> value changes.</summary>
		// Token: 0x140000D7 RID: 215
		// (add) Token: 0x0600142E RID: 5166 RVA: 0x0004CE74 File Offset: 0x0004B074
		// (remove) Token: 0x0600142F RID: 5167 RVA: 0x0004CE87 File Offset: 0x0004B087
		public event EventHandler BackColorChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventBackColor, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventBackColor, value);
			}
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.DataGridTableStyle.BackColor" /> property to its default value.</summary>
		// Token: 0x06001430 RID: 5168 RVA: 0x0004CE9A File Offset: 0x0004B09A
		public void ResetBackColor()
		{
			if (!this.backBrush.Equals(DataGridTableStyle.DefaultBackBrush))
			{
				this.BackColor = DataGridTableStyle.DefaultBackBrush.Color;
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06001431 RID: 5169 RVA: 0x0004CEC0 File Offset: 0x0004B0C0
		internal int BorderWidth
		{
			get
			{
				if (this.DataGrid == null)
				{
					return 0;
				}
				DataGridLineStyle dataGridLineStyle;
				int gridLineWidth;
				if (this.IsDefault)
				{
					dataGridLineStyle = this.DataGrid.GridLineStyle;
					gridLineWidth = this.DataGrid.GridLineWidth;
				}
				else
				{
					dataGridLineStyle = this.GridLineStyle;
					gridLineWidth = this.GridLineWidth;
				}
				if (dataGridLineStyle == DataGridLineStyle.None)
				{
					return 0;
				}
				return gridLineWidth;
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06001432 RID: 5170 RVA: 0x0003C48E File Offset: 0x0003A68E
		internal static SolidBrush DefaultAlternatingBackBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.Window;
			}
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06001433 RID: 5171 RVA: 0x0003C48E File Offset: 0x0003A68E
		internal static SolidBrush DefaultBackBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.Window;
			}
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06001434 RID: 5172 RVA: 0x0003C49A File Offset: 0x0003A69A
		internal static SolidBrush DefaultForeBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.WindowText;
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06001435 RID: 5173 RVA: 0x0003C4B2 File Offset: 0x0003A6B2
		private static SolidBrush DefaultGridLineBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.Control;
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06001436 RID: 5174 RVA: 0x0003C4B2 File Offset: 0x0003A6B2
		private static SolidBrush DefaultHeaderBackBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.Control;
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06001437 RID: 5175 RVA: 0x0003C4BE File Offset: 0x0003A6BE
		private static SolidBrush DefaultHeaderForeBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.ControlText;
			}
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06001438 RID: 5176 RVA: 0x0003C4CA File Offset: 0x0003A6CA
		private static Pen DefaultHeaderForePen
		{
			get
			{
				return new Pen(SystemColors.ControlText);
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06001439 RID: 5177 RVA: 0x0003C4D6 File Offset: 0x0003A6D6
		private static SolidBrush DefaultLinkBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.HotTrack;
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x0600143A RID: 5178 RVA: 0x0003C476 File Offset: 0x0003A676
		private static SolidBrush DefaultSelectionBackBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.ActiveCaption;
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x0600143B RID: 5179 RVA: 0x0003C482 File Offset: 0x0003A682
		private static SolidBrush DefaultSelectionForeBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.ActiveCaptionText;
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x0600143C RID: 5180 RVA: 0x0004CF0F File Offset: 0x0004B10F
		// (set) Token: 0x0600143D RID: 5181 RVA: 0x0004CF18 File Offset: 0x0004B118
		internal int FocusedRelation
		{
			get
			{
				return this.focusedRelation;
			}
			set
			{
				if (this.focusedRelation != value)
				{
					this.focusedRelation = value;
					if (this.focusedRelation == -1)
					{
						this.focusedTextWidth = 0;
						return;
					}
					Graphics graphics = this.DataGrid.CreateGraphicsInternal();
					this.focusedTextWidth = (int)Math.Ceiling((double)graphics.MeasureString((string)this.RelationsList[this.focusedRelation], this.DataGrid.LinkFont).Width);
					graphics.Dispose();
				}
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x0600143E RID: 5182 RVA: 0x0004CF94 File Offset: 0x0004B194
		internal int FocusedTextWidth
		{
			get
			{
				return this.focusedTextWidth;
			}
		}

		/// <summary>Gets or sets the foreground color of the grid table.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the foreground color of the grid table.</returns>
		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x0600143F RID: 5183 RVA: 0x0004CF9C File Offset: 0x0004B19C
		// (set) Token: 0x06001440 RID: 5184 RVA: 0x0004CFAC File Offset: 0x0004B1AC
		[SRCategory("CatColors")]
		[SRDescription("ControlForeColorDescr")]
		public Color ForeColor
		{
			get
			{
				return this.foreBrush.Color;
			}
			set
			{
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[]
					{
						"ForeColor"
					}));
				}
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[]
					{
						"BackColor"
					}));
				}
				if (!this.foreBrush.Color.Equals(value))
				{
					this.foreBrush = new SolidBrush(value);
					this.InvalidateInside();
					this.OnForeColorChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridTableStyle.ForeColor" /> value changes.</summary>
		// Token: 0x140000D8 RID: 216
		// (add) Token: 0x06001441 RID: 5185 RVA: 0x0004D044 File Offset: 0x0004B244
		// (remove) Token: 0x06001442 RID: 5186 RVA: 0x0004D057 File Offset: 0x0004B257
		public event EventHandler ForeColorChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventForeColor, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventForeColor, value);
			}
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06001443 RID: 5187 RVA: 0x0004D06A File Offset: 0x0004B26A
		internal SolidBrush ForeBrush
		{
			get
			{
				return this.foreBrush;
			}
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.DataGridTableStyle.ForeColor" /> property to its default value.</summary>
		// Token: 0x06001444 RID: 5188 RVA: 0x0004D072 File Offset: 0x0004B272
		public void ResetForeColor()
		{
			if (!this.foreBrush.Equals(DataGridTableStyle.DefaultForeBrush))
			{
				this.ForeColor = DataGridTableStyle.DefaultForeBrush.Color;
			}
		}

		/// <summary>Gets or sets the color of grid lines.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the grid line color.</returns>
		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06001445 RID: 5189 RVA: 0x0004D096 File Offset: 0x0004B296
		// (set) Token: 0x06001446 RID: 5190 RVA: 0x0004D0A4 File Offset: 0x0004B2A4
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
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[]
					{
						"GridLineColor"
					}));
				}
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
					this.OnGridLineColorChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridTableStyle.GridLineColor" /> value changes.</summary>
		// Token: 0x140000D9 RID: 217
		// (add) Token: 0x06001447 RID: 5191 RVA: 0x0004D128 File Offset: 0x0004B328
		// (remove) Token: 0x06001448 RID: 5192 RVA: 0x0004D13B File Offset: 0x0004B33B
		public event EventHandler GridLineColorChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventGridLineColor, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventGridLineColor, value);
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Forms.DataGridTableStyle.GridLineColor" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001449 RID: 5193 RVA: 0x0004D14E File Offset: 0x0004B34E
		protected virtual bool ShouldSerializeGridLineColor()
		{
			return !this.GridLineBrush.Equals(DataGridTableStyle.DefaultGridLineBrush);
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.DataGridTableStyle.GridLineColor" /> property to its default value.</summary>
		// Token: 0x0600144A RID: 5194 RVA: 0x0004D163 File Offset: 0x0004B363
		public void ResetGridLineColor()
		{
			if (this.ShouldSerializeGridLineColor())
			{
				this.GridLineColor = DataGridTableStyle.DefaultGridLineBrush.Color;
			}
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x0600144B RID: 5195 RVA: 0x0004D17D File Offset: 0x0004B37D
		internal SolidBrush GridLineBrush
		{
			get
			{
				return this.gridLineBrush;
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x0600144C RID: 5196 RVA: 0x0004D185 File Offset: 0x0004B385
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

		/// <summary>Gets or sets the style of grid lines.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DataGridLineStyle" /> values. The default is <see langword="DataGridLineStyle.Solid" />.</returns>
		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x0600144D RID: 5197 RVA: 0x0004D193 File Offset: 0x0004B393
		// (set) Token: 0x0600144E RID: 5198 RVA: 0x0004D19C File Offset: 0x0004B39C
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
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[]
					{
						"GridLineStyle"
					}));
				}
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridLineStyle));
				}
				if (this.gridLineStyle != value)
				{
					this.gridLineStyle = value;
					this.OnGridLineStyleChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridTableStyle.GridLineStyle" /> value changes.</summary>
		// Token: 0x140000DA RID: 218
		// (add) Token: 0x0600144F RID: 5199 RVA: 0x0004D210 File Offset: 0x0004B410
		// (remove) Token: 0x06001450 RID: 5200 RVA: 0x0004D223 File Offset: 0x0004B423
		public event EventHandler GridLineStyleChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventGridLineStyle, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventGridLineStyle, value);
			}
		}

		/// <summary>Gets or sets the background color of headers.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background color of headers.</returns>
		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001451 RID: 5201 RVA: 0x0004D236 File Offset: 0x0004B436
		// (set) Token: 0x06001452 RID: 5202 RVA: 0x0004D244 File Offset: 0x0004B444
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
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[]
					{
						"HeaderBackColor"
					}));
				}
				if (DataGrid.IsTransparentColor(value))
				{
					throw new ArgumentException(SR.GetString("DataGridTableStyleTransparentHeaderBackColorNotAllowed"));
				}
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[]
					{
						"HeaderBackColor"
					}));
				}
				if (!value.Equals(this.headerBackBrush.Color))
				{
					this.headerBackBrush = new SolidBrush(value);
					this.OnHeaderBackColorChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridTableStyle.HeaderBackColor" /> value changes.</summary>
		// Token: 0x140000DB RID: 219
		// (add) Token: 0x06001453 RID: 5203 RVA: 0x0004D2EC File Offset: 0x0004B4EC
		// (remove) Token: 0x06001454 RID: 5204 RVA: 0x0004D2FF File Offset: 0x0004B4FF
		public event EventHandler HeaderBackColorChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventHeaderBackColor, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventHeaderBackColor, value);
			}
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06001455 RID: 5205 RVA: 0x0004D312 File Offset: 0x0004B512
		internal SolidBrush HeaderBackBrush
		{
			get
			{
				return this.headerBackBrush;
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Forms.DataGridTableStyle.HeaderBackColor" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001456 RID: 5206 RVA: 0x0004D31A File Offset: 0x0004B51A
		protected virtual bool ShouldSerializeHeaderBackColor()
		{
			return !this.HeaderBackBrush.Equals(DataGridTableStyle.DefaultHeaderBackBrush);
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.DataGridTableStyle.HeaderBackColor" /> property to its default value.</summary>
		// Token: 0x06001457 RID: 5207 RVA: 0x0004D32F File Offset: 0x0004B52F
		public void ResetHeaderBackColor()
		{
			if (this.ShouldSerializeHeaderBackColor())
			{
				this.HeaderBackColor = DataGridTableStyle.DefaultHeaderBackBrush.Color;
			}
		}

		/// <summary>Gets or sets the font used for header captions.</summary>
		/// <returns>The <see cref="T:System.Drawing.Font" /> used for captions.</returns>
		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06001458 RID: 5208 RVA: 0x0004D349 File Offset: 0x0004B549
		// (set) Token: 0x06001459 RID: 5209 RVA: 0x0004D374 File Offset: 0x0004B574
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[AmbientValue(null)]
		[SRDescription("DataGridHeaderFontDescr")]
		public Font HeaderFont
		{
			get
			{
				if (this.headerFont != null)
				{
					return this.headerFont;
				}
				if (this.DataGrid != null)
				{
					return this.DataGrid.Font;
				}
				return Control.DefaultFont;
			}
			set
			{
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[]
					{
						"HeaderFont"
					}));
				}
				if ((value == null && this.headerFont != null) || (value != null && !value.Equals(this.headerFont)))
				{
					this.headerFont = value;
					this.OnHeaderFontChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridTableStyle.HeaderFont" /> value changes.</summary>
		// Token: 0x140000DC RID: 220
		// (add) Token: 0x0600145A RID: 5210 RVA: 0x0004D3D5 File Offset: 0x0004B5D5
		// (remove) Token: 0x0600145B RID: 5211 RVA: 0x0004D3E8 File Offset: 0x0004B5E8
		public event EventHandler HeaderFontChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventHeaderFont, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventHeaderFont, value);
			}
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x0004D3FB File Offset: 0x0004B5FB
		private bool ShouldSerializeHeaderFont()
		{
			return this.headerFont != null;
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.DataGridTableStyle.HeaderFont" /> property to its default value.</summary>
		// Token: 0x0600145D RID: 5213 RVA: 0x0004D406 File Offset: 0x0004B606
		public void ResetHeaderFont()
		{
			if (this.headerFont != null)
			{
				this.headerFont = null;
				this.OnHeaderFontChanged(EventArgs.Empty);
			}
		}

		/// <summary>Gets or sets the foreground color of headers.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the foreground color of headers.</returns>
		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x0600145E RID: 5214 RVA: 0x0004D422 File Offset: 0x0004B622
		// (set) Token: 0x0600145F RID: 5215 RVA: 0x0004D430 File Offset: 0x0004B630
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
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[]
					{
						"HeaderForeColor"
					}));
				}
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
					this.OnHeaderForeColorChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridTableStyle.HeaderForeColor" /> value changes.</summary>
		// Token: 0x140000DD RID: 221
		// (add) Token: 0x06001460 RID: 5216 RVA: 0x0004D4CC File Offset: 0x0004B6CC
		// (remove) Token: 0x06001461 RID: 5217 RVA: 0x0004D4DF File Offset: 0x0004B6DF
		public event EventHandler HeaderForeColorChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventHeaderForeColor, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventHeaderForeColor, value);
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Forms.DataGridTableStyle.HeaderForeColor" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001462 RID: 5218 RVA: 0x0004D4F2 File Offset: 0x0004B6F2
		protected virtual bool ShouldSerializeHeaderForeColor()
		{
			return !this.HeaderForePen.Equals(DataGridTableStyle.DefaultHeaderForePen);
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.DataGridTableStyle.HeaderForeColor" /> property to its default value.</summary>
		// Token: 0x06001463 RID: 5219 RVA: 0x0004D507 File Offset: 0x0004B707
		public void ResetHeaderForeColor()
		{
			if (this.ShouldSerializeHeaderForeColor())
			{
				this.HeaderForeColor = DataGridTableStyle.DefaultHeaderForeBrush.Color;
			}
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06001464 RID: 5220 RVA: 0x0004D521 File Offset: 0x0004B721
		internal SolidBrush HeaderForeBrush
		{
			get
			{
				return this.headerForeBrush;
			}
		}

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06001465 RID: 5221 RVA: 0x0004D529 File Offset: 0x0004B729
		internal Pen HeaderForePen
		{
			get
			{
				return this.headerForePen;
			}
		}

		/// <summary>Gets or sets the color of link text.</summary>
		/// <returns>The <see cref="T:System.Drawing.Color" /> of link text.</returns>
		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06001466 RID: 5222 RVA: 0x0004D531 File Offset: 0x0004B731
		// (set) Token: 0x06001467 RID: 5223 RVA: 0x0004D540 File Offset: 0x0004B740
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
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[]
					{
						"LinkColor"
					}));
				}
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
					this.OnLinkColorChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridTableStyle.LinkColor" /> value changes.</summary>
		// Token: 0x140000DE RID: 222
		// (add) Token: 0x06001468 RID: 5224 RVA: 0x0004D5D2 File Offset: 0x0004B7D2
		// (remove) Token: 0x06001469 RID: 5225 RVA: 0x0004D5E5 File Offset: 0x0004B7E5
		public event EventHandler LinkColorChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventLinkColor, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventLinkColor, value);
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Forms.DataGridTableStyle.LinkColor" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600146A RID: 5226 RVA: 0x0004D5F8 File Offset: 0x0004B7F8
		protected virtual bool ShouldSerializeLinkColor()
		{
			return !this.LinkBrush.Equals(DataGridTableStyle.DefaultLinkBrush);
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.DataGridTableStyle.LinkColor" /> property to its default value.</summary>
		// Token: 0x0600146B RID: 5227 RVA: 0x0004D60D File Offset: 0x0004B80D
		public void ResetLinkColor()
		{
			if (this.ShouldSerializeLinkColor())
			{
				this.LinkColor = DataGridTableStyle.DefaultLinkBrush.Color;
			}
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x0600146C RID: 5228 RVA: 0x0004D627 File Offset: 0x0004B827
		internal Brush LinkBrush
		{
			get
			{
				return this.linkBrush;
			}
		}

		/// <summary>Gets or sets the color displayed when hovering over link text.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the hover color.</returns>
		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x0600146D RID: 5229 RVA: 0x0004D62F File Offset: 0x0004B82F
		// (set) Token: 0x0600146E RID: 5230 RVA: 0x0000701A File Offset: 0x0000521A
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridTableStyle.LinkHoverColor" /> value changes.</summary>
		// Token: 0x140000DF RID: 223
		// (add) Token: 0x0600146F RID: 5231 RVA: 0x0004D637 File Offset: 0x0004B837
		// (remove) Token: 0x06001470 RID: 5232 RVA: 0x0004D64A File Offset: 0x0004B84A
		public event EventHandler LinkHoverColorChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventLinkHoverColor, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventLinkHoverColor, value);
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Forms.DataGridTableStyle.LinkHoverColor" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001471 RID: 5233 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected virtual bool ShouldSerializeLinkHoverColor()
		{
			return false;
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06001472 RID: 5234 RVA: 0x0004D65D File Offset: 0x0004B85D
		internal Rectangle RelationshipRect
		{
			get
			{
				if (this.relationshipRect.IsEmpty)
				{
					this.ComputeRelationshipRect();
				}
				return this.relationshipRect;
			}
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x0004D67C File Offset: 0x0004B87C
		private Rectangle ComputeRelationshipRect()
		{
			if (this.relationshipRect.IsEmpty && this.DataGrid.AllowNavigation)
			{
				Graphics graphics = this.DataGrid.CreateGraphicsInternal();
				this.relationshipRect = default(Rectangle);
				this.relationshipRect.X = 0;
				int num = 0;
				for (int i = 0; i < this.RelationsList.Count; i++)
				{
					int num2 = (int)Math.Ceiling((double)graphics.MeasureString((string)this.RelationsList[i], this.DataGrid.LinkFont).Width);
					if (num2 > num)
					{
						num = num2;
					}
				}
				graphics.Dispose();
				this.relationshipRect.Width = num + 5;
				this.relationshipRect.Width = this.relationshipRect.Width + 2;
				this.relationshipRect.Height = this.BorderWidth + this.relationshipHeight * this.RelationsList.Count;
				this.relationshipRect.Height = this.relationshipRect.Height + 2;
				if (this.RelationsList.Count > 0)
				{
					this.relationshipRect.Height = this.relationshipRect.Height + 2;
				}
			}
			return this.relationshipRect;
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x0004D7A3 File Offset: 0x0004B9A3
		internal void ResetRelationsUI()
		{
			this.relationshipRect = Rectangle.Empty;
			this.focusedRelation = -1;
			this.relationshipHeight = this.dataGrid.LinkFontHeight + 1;
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06001475 RID: 5237 RVA: 0x0004D7CA File Offset: 0x0004B9CA
		internal int RelationshipHeight
		{
			get
			{
				return this.relationshipHeight;
			}
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.DataGridTableStyle.LinkHoverColor" /> property to its default value.</summary>
		// Token: 0x06001476 RID: 5238 RVA: 0x0000701A File Offset: 0x0000521A
		public void ResetLinkHoverColor()
		{
		}

		/// <summary>Gets or sets the width used to create columns when a new grid is displayed.</summary>
		/// <returns>The width used to create columns when a new grid is displayed.</returns>
		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06001477 RID: 5239 RVA: 0x0004D7D2 File Offset: 0x0004B9D2
		// (set) Token: 0x06001478 RID: 5240 RVA: 0x0004D7DC File Offset: 0x0004B9DC
		[DefaultValue(75)]
		[SRCategory("CatLayout")]
		[Localizable(true)]
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
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[]
					{
						"PreferredColumnWidth"
					}));
				}
				if (value < 0)
				{
					throw new ArgumentException(SR.GetString("DataGridColumnWidth"), "PreferredColumnWidth");
				}
				if (this.preferredColumnWidth != value)
				{
					this.preferredColumnWidth = value;
					this.OnPreferredColumnWidthChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridTableStyle.PreferredColumnWidth" /> property value changes.</summary>
		// Token: 0x140000E0 RID: 224
		// (add) Token: 0x06001479 RID: 5241 RVA: 0x0004D843 File Offset: 0x0004BA43
		// (remove) Token: 0x0600147A RID: 5242 RVA: 0x0004D856 File Offset: 0x0004BA56
		public event EventHandler PreferredColumnWidthChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventPreferredColumnWidth, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventPreferredColumnWidth, value);
			}
		}

		/// <summary>Gets or sets the height used to create a row when a new grid is displayed.</summary>
		/// <returns>The height of a row, in pixels.</returns>
		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x0600147B RID: 5243 RVA: 0x0004D869 File Offset: 0x0004BA69
		// (set) Token: 0x0600147C RID: 5244 RVA: 0x0004D874 File Offset: 0x0004BA74
		[SRCategory("CatLayout")]
		[Localizable(true)]
		[SRDescription("DataGridPreferredRowHeightDescr")]
		public int PreferredRowHeight
		{
			get
			{
				return this.prefferedRowHeight;
			}
			set
			{
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[]
					{
						"PrefferedRowHeight"
					}));
				}
				if (value < 0)
				{
					throw new ArgumentException(SR.GetString("DataGridRowRowHeight"));
				}
				this.prefferedRowHeight = value;
				this.OnPreferredRowHeightChanged(EventArgs.Empty);
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridTableStyle.PreferredRowHeight" /> value changes.</summary>
		// Token: 0x140000E1 RID: 225
		// (add) Token: 0x0600147D RID: 5245 RVA: 0x0004D8CD File Offset: 0x0004BACD
		// (remove) Token: 0x0600147E RID: 5246 RVA: 0x0004D8E0 File Offset: 0x0004BAE0
		public event EventHandler PreferredRowHeightChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventPreferredRowHeight, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventPreferredRowHeight, value);
			}
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x0004D8F3 File Offset: 0x0004BAF3
		private void ResetPreferredRowHeight()
		{
			this.PreferredRowHeight = DataGridTableStyle.defaultFontHeight + 3;
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Forms.DataGridTableStyle.PreferredRowHeight" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001480 RID: 5248 RVA: 0x0004D902 File Offset: 0x0004BB02
		protected bool ShouldSerializePreferredRowHeight()
		{
			return this.prefferedRowHeight != DataGridTableStyle.defaultFontHeight + 3;
		}

		/// <summary>Gets or sets a value indicating whether column headers are visible.</summary>
		/// <returns>
		///     <see langword="true" /> if column headers are visible; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06001481 RID: 5249 RVA: 0x0004D916 File Offset: 0x0004BB16
		// (set) Token: 0x06001482 RID: 5250 RVA: 0x0004D91E File Offset: 0x0004BB1E
		[SRCategory("CatDisplay")]
		[DefaultValue(true)]
		[SRDescription("DataGridColumnHeadersVisibleDescr")]
		public bool ColumnHeadersVisible
		{
			get
			{
				return this.columnHeadersVisible;
			}
			set
			{
				if (this.columnHeadersVisible != value)
				{
					this.columnHeadersVisible = value;
					this.OnColumnHeadersVisibleChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridTableStyle.ColumnHeadersVisible" /> value changes.</summary>
		// Token: 0x140000E2 RID: 226
		// (add) Token: 0x06001483 RID: 5251 RVA: 0x0004D93B File Offset: 0x0004BB3B
		// (remove) Token: 0x06001484 RID: 5252 RVA: 0x0004D94E File Offset: 0x0004BB4E
		public event EventHandler ColumnHeadersVisibleChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventColumnHeadersVisible, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventColumnHeadersVisible, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether row headers are visible.</summary>
		/// <returns>
		///     <see langword="true" /> if row headers are visible; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06001485 RID: 5253 RVA: 0x0004D961 File Offset: 0x0004BB61
		// (set) Token: 0x06001486 RID: 5254 RVA: 0x0004D969 File Offset: 0x0004BB69
		[SRCategory("CatDisplay")]
		[DefaultValue(true)]
		[SRDescription("DataGridRowHeadersVisibleDescr")]
		public bool RowHeadersVisible
		{
			get
			{
				return this.rowHeadersVisible;
			}
			set
			{
				if (this.rowHeadersVisible != value)
				{
					this.rowHeadersVisible = value;
					this.OnRowHeadersVisibleChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridTableStyle.RowHeadersVisible" /> value changes.</summary>
		// Token: 0x140000E3 RID: 227
		// (add) Token: 0x06001487 RID: 5255 RVA: 0x0004D986 File Offset: 0x0004BB86
		// (remove) Token: 0x06001488 RID: 5256 RVA: 0x0004D999 File Offset: 0x0004BB99
		public event EventHandler RowHeadersVisibleChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventRowHeadersVisible, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventRowHeadersVisible, value);
			}
		}

		/// <summary>Gets or sets the width of row headers.</summary>
		/// <returns>The width of row headers, in pixels.</returns>
		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06001489 RID: 5257 RVA: 0x0004D9AC File Offset: 0x0004BBAC
		// (set) Token: 0x0600148A RID: 5258 RVA: 0x0004D9B4 File Offset: 0x0004BBB4
		[SRCategory("CatLayout")]
		[DefaultValue(35)]
		[Localizable(true)]
		[SRDescription("DataGridRowHeaderWidthDescr")]
		public int RowHeaderWidth
		{
			get
			{
				return this.rowHeaderWidth;
			}
			set
			{
				if (this.DataGrid != null)
				{
					value = Math.Max(this.DataGrid.MinimumRowHeaderWidth(), value);
				}
				if (this.rowHeaderWidth != value)
				{
					this.rowHeaderWidth = value;
					this.OnRowHeaderWidthChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridTableStyle.RowHeaderWidth" /> value changes.</summary>
		// Token: 0x140000E4 RID: 228
		// (add) Token: 0x0600148B RID: 5259 RVA: 0x0004D9EC File Offset: 0x0004BBEC
		// (remove) Token: 0x0600148C RID: 5260 RVA: 0x0004D9FF File Offset: 0x0004BBFF
		public event EventHandler RowHeaderWidthChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventRowHeaderWidth, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventRowHeaderWidth, value);
			}
		}

		/// <summary>Gets or sets the background color of selected cells.</summary>
		/// <returns>The <see cref="T:System.Drawing.Color" /> that represents the background color of selected cells.</returns>
		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x0600148D RID: 5261 RVA: 0x0004DA12 File Offset: 0x0004BC12
		// (set) Token: 0x0600148E RID: 5262 RVA: 0x0004DA20 File Offset: 0x0004BC20
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
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[]
					{
						"SelectionBackColor"
					}));
				}
				if (DataGrid.IsTransparentColor(value))
				{
					throw new ArgumentException(SR.GetString("DataGridTableStyleTransparentSelectionBackColorNotAllowed"));
				}
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[]
					{
						"SelectionBackColor"
					}));
				}
				if (!value.Equals(this.selectionBackBrush.Color))
				{
					this.selectionBackBrush = new SolidBrush(value);
					this.InvalidateInside();
					this.OnSelectionBackColorChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridTableStyle.SelectionBackColor" /> value changes.</summary>
		// Token: 0x140000E5 RID: 229
		// (add) Token: 0x0600148F RID: 5263 RVA: 0x0004DACE File Offset: 0x0004BCCE
		// (remove) Token: 0x06001490 RID: 5264 RVA: 0x0004DAE1 File Offset: 0x0004BCE1
		public event EventHandler SelectionBackColorChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventSelectionBackColor, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventSelectionBackColor, value);
			}
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06001491 RID: 5265 RVA: 0x0004DAF4 File Offset: 0x0004BCF4
		internal SolidBrush SelectionBackBrush
		{
			get
			{
				return this.selectionBackBrush;
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06001492 RID: 5266 RVA: 0x0004DAFC File Offset: 0x0004BCFC
		internal SolidBrush SelectionForeBrush
		{
			get
			{
				return this.selectionForeBrush;
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Forms.DataGridTableStyle.SelectionBackColor" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001493 RID: 5267 RVA: 0x0004DB04 File Offset: 0x0004BD04
		protected bool ShouldSerializeSelectionBackColor()
		{
			return !DataGridTableStyle.DefaultSelectionBackBrush.Equals(this.selectionBackBrush);
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.DataGridTableStyle.SelectionBackColor" /> property to its default value.</summary>
		// Token: 0x06001494 RID: 5268 RVA: 0x0004DB19 File Offset: 0x0004BD19
		public void ResetSelectionBackColor()
		{
			if (this.ShouldSerializeSelectionBackColor())
			{
				this.SelectionBackColor = DataGridTableStyle.DefaultSelectionBackBrush.Color;
			}
		}

		/// <summary>Gets or sets the foreground color of selected cells.</summary>
		/// <returns>The <see cref="T:System.Drawing.Color" /> that represents the foreground color of selected cells.</returns>
		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06001495 RID: 5269 RVA: 0x0004DB33 File Offset: 0x0004BD33
		// (set) Token: 0x06001496 RID: 5270 RVA: 0x0004DB40 File Offset: 0x0004BD40
		[Description("The foreground color for the current data grid row")]
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
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[]
					{
						"SelectionForeColor"
					}));
				}
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
					this.OnSelectionForeColorChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridTableStyle.SelectionForeColor" /> value changes.</summary>
		// Token: 0x140000E6 RID: 230
		// (add) Token: 0x06001497 RID: 5271 RVA: 0x0004DBD6 File Offset: 0x0004BDD6
		// (remove) Token: 0x06001498 RID: 5272 RVA: 0x0004DBE9 File Offset: 0x0004BDE9
		public event EventHandler SelectionForeColorChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventSelectionForeColor, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventSelectionForeColor, value);
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Forms.DataGridTableStyle.SelectionForeColor" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001499 RID: 5273 RVA: 0x0004DBFC File Offset: 0x0004BDFC
		protected virtual bool ShouldSerializeSelectionForeColor()
		{
			return !this.SelectionForeBrush.Equals(DataGridTableStyle.DefaultSelectionForeBrush);
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.DataGridTableStyle.SelectionForeColor" /> property to its default value.</summary>
		// Token: 0x0600149A RID: 5274 RVA: 0x0004DC11 File Offset: 0x0004BE11
		public void ResetSelectionForeColor()
		{
			if (this.ShouldSerializeSelectionForeColor())
			{
				this.SelectionForeColor = DataGridTableStyle.DefaultSelectionForeBrush.Color;
			}
		}

		// Token: 0x0600149B RID: 5275 RVA: 0x0004DC2B File Offset: 0x0004BE2B
		private void InvalidateInside()
		{
			if (this.DataGrid != null)
			{
				this.DataGrid.InvalidateInside();
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridTableStyle" /> class using the specified value to determine whether the grid table is the default style.</summary>
		/// <param name="isDefaultTableStyle">
		///       <see langword="true" /> to specify the table as the default; otherwise, <see langword="false" />. </param>
		// Token: 0x0600149C RID: 5276 RVA: 0x0004DC40 File Offset: 0x0004BE40
		public DataGridTableStyle(bool isDefaultTableStyle)
		{
			this.gridColumns = new GridColumnStylesCollection(this, isDefaultTableStyle);
			this.gridColumns.CollectionChanged += this.OnColumnCollectionChanged;
			this.isDefaultTableStyle = isDefaultTableStyle;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridTableStyle" /> class.</summary>
		// Token: 0x0600149D RID: 5277 RVA: 0x0004DD4E File Offset: 0x0004BF4E
		public DataGridTableStyle() : this(false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridTableStyle" /> class with the specified <see cref="T:System.Windows.Forms.CurrencyManager" />.</summary>
		/// <param name="listManager">The <see cref="T:System.Windows.Forms.CurrencyManager" /> to use. </param>
		// Token: 0x0600149E RID: 5278 RVA: 0x0004DD57 File Offset: 0x0004BF57
		public DataGridTableStyle(CurrencyManager listManager) : this()
		{
			this.mappingName = listManager.GetListName();
			this.SetGridColumnStylesCollection(listManager);
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x0004DD74 File Offset: 0x0004BF74
		internal void SetRelationsList(CurrencyManager listManager)
		{
			PropertyDescriptorCollection itemProperties = listManager.GetItemProperties();
			int count = itemProperties.Count;
			if (this.relationsList.Count > 0)
			{
				this.relationsList.Clear();
			}
			for (int i = 0; i < count; i++)
			{
				PropertyDescriptor propertyDescriptor = itemProperties[i];
				if (DataGridTableStyle.PropertyDescriptorIsARelation(propertyDescriptor))
				{
					this.relationsList.Add(propertyDescriptor.Name);
				}
			}
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x0004DDD8 File Offset: 0x0004BFD8
		internal void SetGridColumnStylesCollection(CurrencyManager listManager)
		{
			this.gridColumns.CollectionChanged -= this.OnColumnCollectionChanged;
			PropertyDescriptorCollection itemProperties = listManager.GetItemProperties();
			if (this.relationsList.Count > 0)
			{
				this.relationsList.Clear();
			}
			int count = itemProperties.Count;
			for (int i = 0; i < count; i++)
			{
				PropertyDescriptor propertyDescriptor = itemProperties[i];
				if (propertyDescriptor.IsBrowsable)
				{
					if (DataGridTableStyle.PropertyDescriptorIsARelation(propertyDescriptor))
					{
						this.relationsList.Add(propertyDescriptor.Name);
					}
					else
					{
						DataGridColumnStyle dataGridColumnStyle = this.CreateGridColumn(propertyDescriptor, this.isDefaultTableStyle);
						if (this.isDefaultTableStyle)
						{
							this.gridColumns.AddDefaultColumn(dataGridColumnStyle);
						}
						else
						{
							dataGridColumnStyle.MappingName = propertyDescriptor.Name;
							dataGridColumnStyle.HeaderText = propertyDescriptor.Name;
							this.gridColumns.Add(dataGridColumnStyle);
						}
					}
				}
			}
			this.gridColumns.CollectionChanged += this.OnColumnCollectionChanged;
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x0004DEC3 File Offset: 0x0004C0C3
		private static bool PropertyDescriptorIsARelation(PropertyDescriptor prop)
		{
			return typeof(IList).IsAssignableFrom(prop.PropertyType) && !typeof(Array).IsAssignableFrom(prop.PropertyType);
		}

		/// <summary>Creates a <see cref="T:System.Windows.Forms.DataGridColumnStyle" />, using the specified property descriptor.</summary>
		/// <param name="prop">The <see cref="T:System.ComponentModel.PropertyDescriptor" /> used to create the column style object. </param>
		/// <returns>The newly created <see cref="T:System.Windows.Forms.DataGridColumnStyle" />.</returns>
		// Token: 0x060014A2 RID: 5282 RVA: 0x0004DEF6 File Offset: 0x0004C0F6
		protected internal virtual DataGridColumnStyle CreateGridColumn(PropertyDescriptor prop)
		{
			return this.CreateGridColumn(prop, false);
		}

		/// <summary>Creates a <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> using the specified property descriptor. Specifies whether the <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> is a default column style.</summary>
		/// <param name="prop">The <see cref="T:System.ComponentModel.PropertyDescriptor" /> used to create the column style object. </param>
		/// <param name="isDefault">Specifies whether the <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> is a default column style. This parameter is read-only. </param>
		/// <returns>The newly created <see cref="T:System.Windows.Forms.DataGridColumnStyle" />.</returns>
		// Token: 0x060014A3 RID: 5283 RVA: 0x0004DF00 File Offset: 0x0004C100
		protected internal virtual DataGridColumnStyle CreateGridColumn(PropertyDescriptor prop, bool isDefault)
		{
			Type propertyType = prop.PropertyType;
			DataGridColumnStyle result;
			if (propertyType.Equals(typeof(bool)))
			{
				result = new DataGridBoolColumn(prop, isDefault);
			}
			else if (propertyType.Equals(typeof(string)))
			{
				result = new DataGridTextBoxColumn(prop, isDefault);
			}
			else if (propertyType.Equals(typeof(DateTime)))
			{
				result = new DataGridTextBoxColumn(prop, "d", isDefault);
			}
			else if (propertyType.Equals(typeof(short)) || propertyType.Equals(typeof(int)) || propertyType.Equals(typeof(long)) || propertyType.Equals(typeof(ushort)) || propertyType.Equals(typeof(uint)) || propertyType.Equals(typeof(ulong)) || propertyType.Equals(typeof(decimal)) || propertyType.Equals(typeof(double)) || propertyType.Equals(typeof(float)) || propertyType.Equals(typeof(byte)) || propertyType.Equals(typeof(sbyte)))
			{
				result = new DataGridTextBoxColumn(prop, "G", isDefault);
			}
			else
			{
				result = new DataGridTextBoxColumn(prop, isDefault);
			}
			return result;
		}

		// Token: 0x060014A4 RID: 5284 RVA: 0x0004E05F File Offset: 0x0004C25F
		internal void ResetRelationsList()
		{
			if (this.isDefaultTableStyle)
			{
				this.relationsList.Clear();
			}
		}

		/// <summary>Gets or sets the name used to map this table to a specific data source.</summary>
		/// <returns>The name used to map this grid to a specific data source.</returns>
		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x060014A5 RID: 5285 RVA: 0x0004E074 File Offset: 0x0004C274
		// (set) Token: 0x060014A6 RID: 5286 RVA: 0x0004E07C File Offset: 0x0004C27C
		[Editor("System.Windows.Forms.Design.DataGridTableStyleMappingNameEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DefaultValue("")]
		public string MappingName
		{
			get
			{
				return this.mappingName;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (value.Equals(this.mappingName))
				{
					return;
				}
				string text = this.MappingName;
				this.mappingName = value;
				try
				{
					if (this.DataGrid != null)
					{
						this.DataGrid.TableStyles.CheckForMappingNameDuplicates(this);
					}
				}
				catch
				{
					this.mappingName = text;
					throw;
				}
				this.OnMappingNameChanged(EventArgs.Empty);
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridTableStyle.MappingName" /> value changes.</summary>
		// Token: 0x140000E7 RID: 231
		// (add) Token: 0x060014A7 RID: 5287 RVA: 0x0004E0F0 File Offset: 0x0004C2F0
		// (remove) Token: 0x060014A8 RID: 5288 RVA: 0x0004E103 File Offset: 0x0004C303
		public event EventHandler MappingNameChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventMappingName, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventMappingName, value);
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x060014A9 RID: 5289 RVA: 0x0004E116 File Offset: 0x0004C316
		internal ArrayList RelationsList
		{
			get
			{
				return this.relationsList;
			}
		}

		/// <summary>Gets the collection of columns drawn for this table.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.GridColumnStylesCollection" /> that contains all <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> objects for the table.</returns>
		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x060014AA RID: 5290 RVA: 0x0004E11E File Offset: 0x0004C31E
		[Localizable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public virtual GridColumnStylesCollection GridColumnStyles
		{
			get
			{
				return this.gridColumns;
			}
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x0004E128 File Offset: 0x0004C328
		internal void SetInternalDataGrid(DataGrid dG, bool force)
		{
			if (this.dataGrid != null && this.dataGrid.Equals(dG) && !force)
			{
				return;
			}
			this.dataGrid = dG;
			if (dG != null && dG.Initializing)
			{
				return;
			}
			int count = this.gridColumns.Count;
			for (int i = 0; i < count; i++)
			{
				this.gridColumns[i].SetDataGridInternalInColumn(dG);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.DataGrid" /> control for the drawn table.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGrid" /> control that displays the table.</returns>
		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x060014AC RID: 5292 RVA: 0x0004E18C File Offset: 0x0004C38C
		// (set) Token: 0x060014AD RID: 5293 RVA: 0x0004E194 File Offset: 0x0004C394
		[Browsable(false)]
		public virtual DataGrid DataGrid
		{
			get
			{
				return this.dataGrid;
			}
			set
			{
				this.SetInternalDataGrid(value, true);
			}
		}

		/// <summary>Gets or sets a value indicating whether columns can be edited.</summary>
		/// <returns>
		///     <see langword="true" />, if columns cannot be edited; otherwise, <see langword="false" />.</returns>
		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x060014AE RID: 5294 RVA: 0x0004E19E File Offset: 0x0004C39E
		// (set) Token: 0x060014AF RID: 5295 RVA: 0x0004E1A6 File Offset: 0x0004C3A6
		[DefaultValue(false)]
		public virtual bool ReadOnly
		{
			get
			{
				return this.readOnly;
			}
			set
			{
				if (this.readOnly != value)
				{
					this.readOnly = value;
					this.OnReadOnlyChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridTableStyle.ReadOnly" /> value changes.</summary>
		// Token: 0x140000E8 RID: 232
		// (add) Token: 0x060014B0 RID: 5296 RVA: 0x0004E1C3 File Offset: 0x0004C3C3
		// (remove) Token: 0x060014B1 RID: 5297 RVA: 0x0004E1D6 File Offset: 0x0004C3D6
		public event EventHandler ReadOnlyChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventReadOnly, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventReadOnly, value);
			}
		}

		/// <summary>Requests an edit operation.</summary>
		/// <param name="gridColumn">The <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> to edit. </param>
		/// <param name="rowNumber">The number of the edited row. </param>
		/// <returns>
		///     <see langword="true" />, if the operation succeeds; otherwise, <see langword="false" />.</returns>
		// Token: 0x060014B2 RID: 5298 RVA: 0x0004E1EC File Offset: 0x0004C3EC
		public bool BeginEdit(DataGridColumnStyle gridColumn, int rowNumber)
		{
			DataGrid dataGrid = this.DataGrid;
			return dataGrid != null && dataGrid.BeginEdit(gridColumn, rowNumber);
		}

		/// <summary>Requests an end to an edit operation.</summary>
		/// <param name="gridColumn">The <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> to edit. </param>
		/// <param name="rowNumber">The number of the edited row. </param>
		/// <param name="shouldAbort">A value indicating whether the operation should be stopped; <see langword="true" /> if it should stop; otherwise, <see langword="false" />. </param>
		/// <returns>
		///     <see langword="true" /> if the edit operation ends successfully; otherwise, <see langword="false" />.</returns>
		// Token: 0x060014B3 RID: 5299 RVA: 0x0004E210 File Offset: 0x0004C410
		public bool EndEdit(DataGridColumnStyle gridColumn, int rowNumber, bool shouldAbort)
		{
			DataGrid dataGrid = this.DataGrid;
			return dataGrid != null && dataGrid.EndEdit(gridColumn, rowNumber, shouldAbort);
		}

		// Token: 0x060014B4 RID: 5300 RVA: 0x0004E234 File Offset: 0x0004C434
		internal void InvalidateColumn(DataGridColumnStyle column)
		{
			int num = this.GridColumnStyles.IndexOf(column);
			if (num >= 0 && this.DataGrid != null)
			{
				this.DataGrid.InvalidateColumn(num);
			}
		}

		// Token: 0x060014B5 RID: 5301 RVA: 0x0004E268 File Offset: 0x0004C468
		private void OnColumnCollectionChanged(object sender, CollectionChangeEventArgs e)
		{
			this.gridColumns.CollectionChanged -= this.OnColumnCollectionChanged;
			try
			{
				DataGrid dataGrid = this.DataGrid;
				DataGridColumnStyle dataGridColumnStyle = e.Element as DataGridColumnStyle;
				if (e.Action == CollectionChangeAction.Add)
				{
					if (dataGridColumnStyle != null)
					{
						dataGridColumnStyle.SetDataGridInternalInColumn(dataGrid);
					}
				}
				else if (e.Action == CollectionChangeAction.Remove)
				{
					if (dataGridColumnStyle != null)
					{
						dataGridColumnStyle.SetDataGridInternalInColumn(null);
					}
				}
				else if (e.Element != null)
				{
					for (int i = 0; i < this.gridColumns.Count; i++)
					{
						this.gridColumns[i].SetDataGridInternalInColumn(null);
					}
				}
				if (dataGrid != null)
				{
					dataGrid.OnColumnCollectionChanged(this, e);
				}
			}
			finally
			{
				this.gridColumns.CollectionChanged += this.OnColumnCollectionChanged;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGridTableStyle.ReadOnlyChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060014B6 RID: 5302 RVA: 0x0004E330 File Offset: 0x0004C530
		protected virtual void OnReadOnlyChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventReadOnly] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGridTableStyle.MappingNameChanged" /> event </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060014B7 RID: 5303 RVA: 0x0004E360 File Offset: 0x0004C560
		protected virtual void OnMappingNameChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventMappingName] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGridTableStyle.AlternatingBackColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060014B8 RID: 5304 RVA: 0x0004E390 File Offset: 0x0004C590
		protected virtual void OnAlternatingBackColorChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventAlternatingBackColor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGridTableStyle.ForeColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060014B9 RID: 5305 RVA: 0x0004E3C0 File Offset: 0x0004C5C0
		protected virtual void OnForeColorChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventBackColor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGridTableStyle.BackColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060014BA RID: 5306 RVA: 0x0004E3F0 File Offset: 0x0004C5F0
		protected virtual void OnBackColorChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventForeColor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGridTableStyle.AllowSortingChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060014BB RID: 5307 RVA: 0x0004E420 File Offset: 0x0004C620
		protected virtual void OnAllowSortingChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventAllowSorting] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGridTableStyle.GridLineColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060014BC RID: 5308 RVA: 0x0004E450 File Offset: 0x0004C650
		protected virtual void OnGridLineColorChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventGridLineColor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGridTableStyle.GridLineStyleChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060014BD RID: 5309 RVA: 0x0004E480 File Offset: 0x0004C680
		protected virtual void OnGridLineStyleChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventGridLineStyle] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGridTableStyle.HeaderBackColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060014BE RID: 5310 RVA: 0x0004E4B0 File Offset: 0x0004C6B0
		protected virtual void OnHeaderBackColorChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventHeaderBackColor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGridTableStyle.HeaderFontChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060014BF RID: 5311 RVA: 0x0004E4E0 File Offset: 0x0004C6E0
		protected virtual void OnHeaderFontChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventHeaderFont] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGridTableStyle.HeaderForeColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060014C0 RID: 5312 RVA: 0x0004E510 File Offset: 0x0004C710
		protected virtual void OnHeaderForeColorChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventHeaderForeColor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGridTableStyle.LinkColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060014C1 RID: 5313 RVA: 0x0004E540 File Offset: 0x0004C740
		protected virtual void OnLinkColorChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventLinkColor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see langword="LinkHoverColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060014C2 RID: 5314 RVA: 0x0004E570 File Offset: 0x0004C770
		protected virtual void OnLinkHoverColorChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventLinkHoverColor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGridTableStyle.PreferredRowHeightChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060014C3 RID: 5315 RVA: 0x0004E5A0 File Offset: 0x0004C7A0
		protected virtual void OnPreferredRowHeightChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventPreferredRowHeight] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGridTableStyle.PreferredColumnWidthChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060014C4 RID: 5316 RVA: 0x0004E5D0 File Offset: 0x0004C7D0
		protected virtual void OnPreferredColumnWidthChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventPreferredColumnWidth] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGridTableStyle.ColumnHeadersVisibleChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060014C5 RID: 5317 RVA: 0x0004E600 File Offset: 0x0004C800
		protected virtual void OnColumnHeadersVisibleChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventColumnHeadersVisible] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGridTableStyle.RowHeadersVisibleChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060014C6 RID: 5318 RVA: 0x0004E630 File Offset: 0x0004C830
		protected virtual void OnRowHeadersVisibleChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventRowHeadersVisible] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGridTableStyle.RowHeaderWidthChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060014C7 RID: 5319 RVA: 0x0004E660 File Offset: 0x0004C860
		protected virtual void OnRowHeaderWidthChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventRowHeaderWidth] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGridTableStyle.SelectionForeColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060014C8 RID: 5320 RVA: 0x0004E690 File Offset: 0x0004C890
		protected virtual void OnSelectionForeColorChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventSelectionForeColor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGridTableStyle.SelectionBackColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060014C9 RID: 5321 RVA: 0x0004E6C0 File Offset: 0x0004C8C0
		protected virtual void OnSelectionBackColorChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventSelectionBackColor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Disposes of the resources (other than memory) used by the <see cref="T:System.Windows.Forms.DataGridTableStyle" />.</summary>
		/// <param name="disposing">
		///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		// Token: 0x060014CA RID: 5322 RVA: 0x0004E6F0 File Offset: 0x0004C8F0
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				GridColumnStylesCollection gridColumnStyles = this.GridColumnStyles;
				if (gridColumnStyles != null)
				{
					for (int i = 0; i < gridColumnStyles.Count; i++)
					{
						gridColumnStyles[i].Dispose();
					}
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x060014CB RID: 5323 RVA: 0x0004E72E File Offset: 0x0004C92E
		internal bool IsDefault
		{
			get
			{
				return this.isDefaultTableStyle;
			}
		}

		// Token: 0x040009DB RID: 2523
		internal DataGrid dataGrid;

		// Token: 0x040009DC RID: 2524
		private int relationshipHeight;

		// Token: 0x040009DD RID: 2525
		internal const int relationshipSpacing = 1;

		// Token: 0x040009DE RID: 2526
		private Rectangle relationshipRect = Rectangle.Empty;

		// Token: 0x040009DF RID: 2527
		private int focusedRelation = -1;

		// Token: 0x040009E0 RID: 2528
		private int focusedTextWidth;

		// Token: 0x040009E1 RID: 2529
		private ArrayList relationsList = new ArrayList(2);

		// Token: 0x040009E2 RID: 2530
		private string mappingName = "";

		// Token: 0x040009E3 RID: 2531
		private GridColumnStylesCollection gridColumns;

		// Token: 0x040009E4 RID: 2532
		private bool readOnly;

		// Token: 0x040009E5 RID: 2533
		private bool isDefaultTableStyle;

		// Token: 0x040009E6 RID: 2534
		private static readonly object EventAllowSorting = new object();

		// Token: 0x040009E7 RID: 2535
		private static readonly object EventGridLineColor = new object();

		// Token: 0x040009E8 RID: 2536
		private static readonly object EventGridLineStyle = new object();

		// Token: 0x040009E9 RID: 2537
		private static readonly object EventHeaderBackColor = new object();

		// Token: 0x040009EA RID: 2538
		private static readonly object EventHeaderForeColor = new object();

		// Token: 0x040009EB RID: 2539
		private static readonly object EventHeaderFont = new object();

		// Token: 0x040009EC RID: 2540
		private static readonly object EventLinkColor = new object();

		// Token: 0x040009ED RID: 2541
		private static readonly object EventLinkHoverColor = new object();

		// Token: 0x040009EE RID: 2542
		private static readonly object EventPreferredColumnWidth = new object();

		// Token: 0x040009EF RID: 2543
		private static readonly object EventPreferredRowHeight = new object();

		// Token: 0x040009F0 RID: 2544
		private static readonly object EventColumnHeadersVisible = new object();

		// Token: 0x040009F1 RID: 2545
		private static readonly object EventRowHeaderWidth = new object();

		// Token: 0x040009F2 RID: 2546
		private static readonly object EventSelectionBackColor = new object();

		// Token: 0x040009F3 RID: 2547
		private static readonly object EventSelectionForeColor = new object();

		// Token: 0x040009F4 RID: 2548
		private static readonly object EventMappingName = new object();

		// Token: 0x040009F5 RID: 2549
		private static readonly object EventAlternatingBackColor = new object();

		// Token: 0x040009F6 RID: 2550
		private static readonly object EventBackColor = new object();

		// Token: 0x040009F7 RID: 2551
		private static readonly object EventForeColor = new object();

		// Token: 0x040009F8 RID: 2552
		private static readonly object EventReadOnly = new object();

		// Token: 0x040009F9 RID: 2553
		private static readonly object EventRowHeadersVisible = new object();

		// Token: 0x040009FA RID: 2554
		private const bool defaultAllowSorting = true;

		// Token: 0x040009FB RID: 2555
		private const DataGridLineStyle defaultGridLineStyle = DataGridLineStyle.Solid;

		// Token: 0x040009FC RID: 2556
		private const int defaultPreferredColumnWidth = 75;

		// Token: 0x040009FD RID: 2557
		private const int defaultRowHeaderWidth = 35;

		// Token: 0x040009FE RID: 2558
		internal static readonly Font defaultFont = Control.DefaultFont;

		// Token: 0x040009FF RID: 2559
		internal static readonly int defaultFontHeight = DataGridTableStyle.defaultFont.Height;

		// Token: 0x04000A00 RID: 2560
		private bool allowSorting = true;

		// Token: 0x04000A01 RID: 2561
		private SolidBrush alternatingBackBrush = DataGridTableStyle.DefaultAlternatingBackBrush;

		// Token: 0x04000A02 RID: 2562
		private SolidBrush backBrush = DataGridTableStyle.DefaultBackBrush;

		// Token: 0x04000A03 RID: 2563
		private SolidBrush foreBrush = DataGridTableStyle.DefaultForeBrush;

		// Token: 0x04000A04 RID: 2564
		private SolidBrush gridLineBrush = DataGridTableStyle.DefaultGridLineBrush;

		// Token: 0x04000A05 RID: 2565
		private DataGridLineStyle gridLineStyle = DataGridLineStyle.Solid;

		// Token: 0x04000A06 RID: 2566
		internal SolidBrush headerBackBrush = DataGridTableStyle.DefaultHeaderBackBrush;

		// Token: 0x04000A07 RID: 2567
		internal Font headerFont;

		// Token: 0x04000A08 RID: 2568
		internal SolidBrush headerForeBrush = DataGridTableStyle.DefaultHeaderForeBrush;

		// Token: 0x04000A09 RID: 2569
		internal Pen headerForePen = DataGridTableStyle.DefaultHeaderForePen;

		// Token: 0x04000A0A RID: 2570
		private SolidBrush linkBrush = DataGridTableStyle.DefaultLinkBrush;

		// Token: 0x04000A0B RID: 2571
		internal int preferredColumnWidth = 75;

		// Token: 0x04000A0C RID: 2572
		private int prefferedRowHeight = DataGridTableStyle.defaultFontHeight + 3;

		// Token: 0x04000A0D RID: 2573
		private SolidBrush selectionBackBrush = DataGridTableStyle.DefaultSelectionBackBrush;

		// Token: 0x04000A0E RID: 2574
		private SolidBrush selectionForeBrush = DataGridTableStyle.DefaultSelectionForeBrush;

		// Token: 0x04000A0F RID: 2575
		private int rowHeaderWidth = 35;

		// Token: 0x04000A10 RID: 2576
		private bool rowHeadersVisible = true;

		// Token: 0x04000A11 RID: 2577
		private bool columnHeadersVisible = true;

		/// <summary>Gets the default table style.</summary>
		// Token: 0x04000A12 RID: 2578
		public static readonly DataGridTableStyle DefaultTableStyle = new DataGridTableStyle(true);
	}
}
