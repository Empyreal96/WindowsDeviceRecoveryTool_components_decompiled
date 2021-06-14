using System;
using System.Drawing;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x020003B5 RID: 949
	internal class ToolStripGrip : ToolStripButton
	{
		// Token: 0x06003E9C RID: 16028 RVA: 0x00111A04 File Offset: 0x0010FC04
		internal ToolStripGrip()
		{
			if (DpiHelper.EnableToolStripHighDpiImprovements)
			{
				this.scaledDefaultPadding = DpiHelper.LogicalToDeviceUnits(ToolStripGrip.defaultPadding, 0);
				this.scaledGripThickness = DpiHelper.LogicalToDeviceUnitsX(ToolStripGrip.gripThicknessDefault);
				this.scaledGripThicknessVisualStylesEnabled = DpiHelper.LogicalToDeviceUnitsX(ToolStripGrip.gripThicknessVisualStylesEnabled);
			}
			this.gripThickness = (ToolStripManager.VisualStylesEnabled ? this.scaledGripThicknessVisualStylesEnabled : this.scaledGripThickness);
			base.SupportsItemClick = false;
		}

		// Token: 0x17000F9B RID: 3995
		// (get) Token: 0x06003E9D RID: 16029 RVA: 0x00111AA8 File Offset: 0x0010FCA8
		protected internal override Padding DefaultMargin
		{
			get
			{
				return this.scaledDefaultPadding;
			}
		}

		// Token: 0x17000F9C RID: 3996
		// (get) Token: 0x06003E9E RID: 16030 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		public override bool CanSelect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000F9D RID: 3997
		// (get) Token: 0x06003E9F RID: 16031 RVA: 0x00111AB0 File Offset: 0x0010FCB0
		internal int GripThickness
		{
			get
			{
				return this.gripThickness;
			}
		}

		// Token: 0x17000F9E RID: 3998
		// (get) Token: 0x06003EA0 RID: 16032 RVA: 0x00111AB8 File Offset: 0x0010FCB8
		// (set) Token: 0x06003EA1 RID: 16033 RVA: 0x00111ACC File Offset: 0x0010FCCC
		internal bool MovingToolStrip
		{
			get
			{
				return this.ToolStripPanelRow != null && this.movingToolStrip;
			}
			set
			{
				if (this.movingToolStrip != value && base.ParentInternal != null)
				{
					if (value && base.ParentInternal.ToolStripPanelRow == null)
					{
						return;
					}
					this.movingToolStrip = value;
					this.lastEndLocation = ToolStrip.InvalidMouseEnter;
					if (this.movingToolStrip)
					{
						((ISupportToolStripPanel)base.ParentInternal).BeginDrag();
						return;
					}
					((ISupportToolStripPanel)base.ParentInternal).EndDrag();
				}
			}
		}

		// Token: 0x17000F9F RID: 3999
		// (get) Token: 0x06003EA2 RID: 16034 RVA: 0x00111B2C File Offset: 0x0010FD2C
		private ToolStripPanelRow ToolStripPanelRow
		{
			get
			{
				if (base.ParentInternal != null)
				{
					return ((ISupportToolStripPanel)base.ParentInternal).ToolStripPanelRow;
				}
				return null;
			}
		}

		// Token: 0x06003EA3 RID: 16035 RVA: 0x00111B43 File Offset: 0x0010FD43
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new ToolStripGrip.ToolStripGripAccessibleObject(this);
		}

		// Token: 0x06003EA4 RID: 16036 RVA: 0x00111B4C File Offset: 0x0010FD4C
		public override Size GetPreferredSize(Size constrainingSize)
		{
			Size empty = Size.Empty;
			if (base.ParentInternal != null)
			{
				if (base.ParentInternal.LayoutStyle == ToolStripLayoutStyle.VerticalStackWithOverflow)
				{
					empty = new Size(base.ParentInternal.Width, this.gripThickness);
				}
				else
				{
					empty = new Size(this.gripThickness, base.ParentInternal.Height);
				}
			}
			if (empty.Width > constrainingSize.Width)
			{
				empty.Width = constrainingSize.Width;
			}
			if (empty.Height > constrainingSize.Height)
			{
				empty.Height = constrainingSize.Height;
			}
			return empty;
		}

		// Token: 0x06003EA5 RID: 16037 RVA: 0x00111BE4 File Offset: 0x0010FDE4
		private bool LeftMouseButtonIsDown()
		{
			return Control.MouseButtons == MouseButtons.Left && Control.ModifierKeys == Keys.None;
		}

		// Token: 0x06003EA6 RID: 16038 RVA: 0x00111BFC File Offset: 0x0010FDFC
		protected override void OnPaint(PaintEventArgs e)
		{
			if (base.ParentInternal != null)
			{
				base.ParentInternal.OnPaintGrip(e);
			}
		}

		// Token: 0x06003EA7 RID: 16039 RVA: 0x00111C12 File Offset: 0x0010FE12
		protected override void OnMouseDown(MouseEventArgs mea)
		{
			this.startLocation = base.TranslatePoint(new Point(mea.X, mea.Y), ToolStripPointType.ToolStripItemCoords, ToolStripPointType.ScreenCoords);
			base.OnMouseDown(mea);
		}

		// Token: 0x06003EA8 RID: 16040 RVA: 0x00111C3C File Offset: 0x0010FE3C
		protected override void OnMouseMove(MouseEventArgs mea)
		{
			bool flag = this.LeftMouseButtonIsDown();
			if (!this.MovingToolStrip && flag)
			{
				Point point = base.TranslatePoint(mea.Location, ToolStripPointType.ToolStripItemCoords, ToolStripPointType.ScreenCoords);
				int num = point.X - this.startLocation.X;
				num = ((num < 0) ? (num * -1) : num);
				if (ToolStripGrip.DragSize == LayoutUtils.MaxSize)
				{
					ToolStripGrip.DragSize = SystemInformation.DragSize;
				}
				if (num >= ToolStripGrip.DragSize.Width)
				{
					this.MovingToolStrip = true;
				}
				else
				{
					int num2 = point.Y - this.startLocation.Y;
					num2 = ((num2 < 0) ? (num2 * -1) : num2);
					if (num2 >= ToolStripGrip.DragSize.Height)
					{
						this.MovingToolStrip = true;
					}
				}
			}
			if (this.MovingToolStrip)
			{
				if (flag)
				{
					Point point2 = base.TranslatePoint(new Point(mea.X, mea.Y), ToolStripPointType.ToolStripItemCoords, ToolStripPointType.ScreenCoords);
					if (point2 != this.lastEndLocation)
					{
						this.ToolStripPanelRow.ToolStripPanel.MoveControl(base.ParentInternal, point2);
						this.lastEndLocation = point2;
					}
					this.startLocation = point2;
				}
				else
				{
					this.MovingToolStrip = false;
				}
			}
			base.OnMouseMove(mea);
		}

		// Token: 0x06003EA9 RID: 16041 RVA: 0x00111D60 File Offset: 0x0010FF60
		protected override void OnMouseEnter(EventArgs e)
		{
			if (base.ParentInternal != null && this.ToolStripPanelRow != null && !base.ParentInternal.IsInDesignMode)
			{
				this.oldCursor = base.ParentInternal.Cursor;
				ToolStripGrip.SetCursor(base.ParentInternal, Cursors.SizeAll);
			}
			else
			{
				this.oldCursor = null;
			}
			base.OnMouseEnter(e);
		}

		// Token: 0x06003EAA RID: 16042 RVA: 0x00111DBC File Offset: 0x0010FFBC
		protected override void OnMouseLeave(EventArgs e)
		{
			if (this.oldCursor != null && !base.ParentInternal.IsInDesignMode)
			{
				ToolStripGrip.SetCursor(base.ParentInternal, this.oldCursor);
			}
			if (!this.MovingToolStrip && this.LeftMouseButtonIsDown())
			{
				this.MovingToolStrip = true;
			}
			base.OnMouseLeave(e);
		}

		// Token: 0x06003EAB RID: 16043 RVA: 0x00111E14 File Offset: 0x00110014
		protected override void OnMouseUp(MouseEventArgs mea)
		{
			if (this.MovingToolStrip)
			{
				Point screenLocation = base.TranslatePoint(new Point(mea.X, mea.Y), ToolStripPointType.ToolStripItemCoords, ToolStripPointType.ScreenCoords);
				this.ToolStripPanelRow.ToolStripPanel.MoveControl(base.ParentInternal, screenLocation);
			}
			if (!base.ParentInternal.IsInDesignMode)
			{
				ToolStripGrip.SetCursor(base.ParentInternal, this.oldCursor);
			}
			ToolStripPanel.ClearDragFeedback();
			this.MovingToolStrip = false;
			base.OnMouseUp(mea);
		}

		// Token: 0x06003EAC RID: 16044 RVA: 0x00111E8C File Offset: 0x0011008C
		internal override void ToolStrip_RescaleConstants(int oldDpi, int newDpi)
		{
			base.RescaleConstantsInternal(newDpi);
			this.scaledDefaultPadding = DpiHelper.LogicalToDeviceUnits(ToolStripGrip.defaultPadding, newDpi);
			this.scaledGripThickness = DpiHelper.LogicalToDeviceUnits(ToolStripGrip.gripThicknessDefault, newDpi);
			this.scaledGripThicknessVisualStylesEnabled = DpiHelper.LogicalToDeviceUnits(ToolStripGrip.gripThicknessVisualStylesEnabled, newDpi);
			base.Margin = this.DefaultMargin;
			this.gripThickness = (ToolStripManager.VisualStylesEnabled ? this.scaledGripThicknessVisualStylesEnabled : this.scaledGripThickness);
			this.OnFontChanged(EventArgs.Empty);
		}

		// Token: 0x06003EAD RID: 16045 RVA: 0x00111F05 File Offset: 0x00110105
		private static void SetCursor(Control control, Cursor cursor)
		{
			IntSecurity.ModifyCursor.Assert();
			control.Cursor = cursor;
		}

		// Token: 0x04002411 RID: 9233
		private Cursor oldCursor;

		// Token: 0x04002412 RID: 9234
		private int gripThickness;

		// Token: 0x04002413 RID: 9235
		private Point startLocation = Point.Empty;

		// Token: 0x04002414 RID: 9236
		private bool movingToolStrip;

		// Token: 0x04002415 RID: 9237
		private Point lastEndLocation = ToolStrip.InvalidMouseEnter;

		// Token: 0x04002416 RID: 9238
		private static Size DragSize = LayoutUtils.MaxSize;

		// Token: 0x04002417 RID: 9239
		private static readonly Padding defaultPadding = new Padding(2);

		// Token: 0x04002418 RID: 9240
		private static readonly int gripThicknessDefault = 3;

		// Token: 0x04002419 RID: 9241
		private static readonly int gripThicknessVisualStylesEnabled = 5;

		// Token: 0x0400241A RID: 9242
		private Padding scaledDefaultPadding = ToolStripGrip.defaultPadding;

		// Token: 0x0400241B RID: 9243
		private int scaledGripThickness = ToolStripGrip.gripThicknessDefault;

		// Token: 0x0400241C RID: 9244
		private int scaledGripThicknessVisualStylesEnabled = ToolStripGrip.gripThicknessVisualStylesEnabled;

		// Token: 0x02000738 RID: 1848
		internal class ToolStripGripAccessibleObject : ToolStripButton.ToolStripButtonAccessibleObject
		{
			// Token: 0x06006112 RID: 24850 RVA: 0x0018D459 File Offset: 0x0018B659
			public ToolStripGripAccessibleObject(ToolStripGrip owner) : base(owner)
			{
			}

			// Token: 0x1700172D RID: 5933
			// (get) Token: 0x06006113 RID: 24851 RVA: 0x0018D464 File Offset: 0x0018B664
			// (set) Token: 0x06006114 RID: 24852 RVA: 0x0018D4A5 File Offset: 0x0018B6A5
			public override string Name
			{
				get
				{
					string accessibleName = base.Owner.AccessibleName;
					if (accessibleName != null)
					{
						return accessibleName;
					}
					if (string.IsNullOrEmpty(this.stockName))
					{
						this.stockName = SR.GetString("ToolStripGripAccessibleName");
					}
					return this.stockName;
				}
				set
				{
					base.Name = value;
				}
			}

			// Token: 0x1700172E RID: 5934
			// (get) Token: 0x06006115 RID: 24853 RVA: 0x0018D4B0 File Offset: 0x0018B6B0
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.Grip;
				}
			}

			// Token: 0x06006116 RID: 24854 RVA: 0x0018D4D0 File Offset: 0x0018B6D0
			internal override object GetPropertyValue(int propertyID)
			{
				if (AccessibilityImprovements.Level3)
				{
					if (propertyID == 30003)
					{
						return 50027;
					}
					if (propertyID == 30022)
					{
						return false;
					}
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x0400417D RID: 16765
			private string stockName;
		}
	}
}
