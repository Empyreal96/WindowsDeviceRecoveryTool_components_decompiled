using System;
using System.Drawing;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x0200047C RID: 1148
	internal class DocComment : PropertyGrid.SnappableControl
	{
		// Token: 0x06004D44 RID: 19780 RVA: 0x0013CC7C File Offset: 0x0013AE7C
		internal DocComment(PropertyGrid owner) : base(owner)
		{
			base.SuspendLayout();
			this.m_labelTitle = new Label();
			this.m_labelTitle.UseMnemonic = false;
			this.m_labelTitle.Cursor = Cursors.Default;
			this.m_labelDesc = new Label();
			this.m_labelDesc.AutoEllipsis = true;
			this.m_labelDesc.Cursor = Cursors.Default;
			this.UpdateTextRenderingEngine();
			base.Controls.Add(this.m_labelTitle);
			base.Controls.Add(this.m_labelDesc);
			if (DpiHelper.EnableDpiChangedHighDpiImprovements)
			{
				this.cBorder = base.LogicalToDeviceUnits(3);
				this.cydef = base.LogicalToDeviceUnits(59);
			}
			base.Size = new Size(0, this.cydef);
			this.Text = SR.GetString("PBRSDocCommentPaneTitle");
			base.SetStyle(ControlStyles.Selectable, false);
			base.ResumeLayout(false);
		}

		// Token: 0x17001315 RID: 4885
		// (get) Token: 0x06004D45 RID: 19781 RVA: 0x0013CD84 File Offset: 0x0013AF84
		// (set) Token: 0x06004D46 RID: 19782 RVA: 0x0013CD99 File Offset: 0x0013AF99
		public virtual int Lines
		{
			get
			{
				this.UpdateUIWithFont();
				return base.Height / this.lineHeight;
			}
			set
			{
				this.UpdateUIWithFont();
				base.Size = new Size(base.Width, 1 + value * this.lineHeight);
			}
		}

		// Token: 0x06004D47 RID: 19783 RVA: 0x0013CDBC File Offset: 0x0013AFBC
		public override int GetOptimalHeight(int width)
		{
			this.UpdateUIWithFont();
			int num = this.m_labelTitle.Size.Height;
			if (this.ownerGrid.IsHandleCreated && !base.IsHandleCreated)
			{
				base.CreateControl();
			}
			Graphics graphics = this.m_labelDesc.CreateGraphicsInternal();
			SizeF value = PropertyGrid.MeasureTextHelper.MeasureText(this.ownerGrid, graphics, this.m_labelTitle.Text, this.Font, width);
			Size size = Size.Ceiling(value);
			graphics.Dispose();
			int num2 = DpiHelper.EnableDpiChangedHighDpiImprovements ? base.LogicalToDeviceUnits(2) : 2;
			num += size.Height * 2 + num2;
			return Math.Max(num + 2 * num2, DpiHelper.EnableDpiChangedHighDpiImprovements ? base.LogicalToDeviceUnits(59) : 59);
		}

		// Token: 0x06004D48 RID: 19784 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void LayoutWindow()
		{
		}

		// Token: 0x06004D49 RID: 19785 RVA: 0x0013CE77 File Offset: 0x0013B077
		protected override void OnFontChanged(EventArgs e)
		{
			this.needUpdateUIWithFont = true;
			base.PerformLayout();
			base.OnFontChanged(e);
		}

		// Token: 0x06004D4A RID: 19786 RVA: 0x0013CE8D File Offset: 0x0013B08D
		protected override void OnLayout(LayoutEventArgs e)
		{
			this.UpdateUIWithFont();
			this.SetChildLabelsBounds();
			this.m_labelDesc.Text = this.fullDesc;
			this.m_labelDesc.AccessibleName = this.fullDesc;
			base.OnLayout(e);
		}

		// Token: 0x06004D4B RID: 19787 RVA: 0x0013CEC4 File Offset: 0x0013B0C4
		protected override void OnResize(EventArgs e)
		{
			Rectangle clientRectangle = base.ClientRectangle;
			if (!this.rect.IsEmpty && clientRectangle.Width > this.rect.Width)
			{
				Rectangle rc = new Rectangle(this.rect.Width - 1, 0, clientRectangle.Width - this.rect.Width + 1, this.rect.Height);
				base.Invalidate(rc);
			}
			if (DpiHelper.EnableDpiChangedHighDpiImprovements)
			{
				this.lineHeight = this.Font.Height + base.LogicalToDeviceUnits(2);
				if (base.ClientRectangle.Width != this.rect.Width || base.ClientRectangle.Height != this.rect.Height)
				{
					this.m_labelTitle.Location = new Point(this.cBorder, this.cBorder);
					this.m_labelDesc.Location = new Point(this.cBorder, this.cBorder + this.lineHeight);
					this.SetChildLabelsBounds();
				}
			}
			this.rect = clientRectangle;
			base.OnResize(e);
		}

		// Token: 0x06004D4C RID: 19788 RVA: 0x0013CFE4 File Offset: 0x0013B1E4
		private void SetChildLabelsBounds()
		{
			Size clientSize = base.ClientSize;
			clientSize.Width = Math.Max(0, clientSize.Width - 2 * this.cBorder);
			clientSize.Height = Math.Max(0, clientSize.Height - 2 * this.cBorder);
			this.m_labelTitle.SetBounds(this.m_labelTitle.Top, this.m_labelTitle.Left, clientSize.Width, Math.Min(this.lineHeight, clientSize.Height), BoundsSpecified.Size);
			this.m_labelDesc.SetBounds(this.m_labelDesc.Top, this.m_labelDesc.Left, clientSize.Width, Math.Max(0, clientSize.Height - this.lineHeight - (DpiHelper.EnableDpiChangedHighDpiImprovements ? base.LogicalToDeviceUnits(1) : 1)), BoundsSpecified.Size);
		}

		// Token: 0x06004D4D RID: 19789 RVA: 0x0013D0BE File Offset: 0x0013B2BE
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			this.UpdateUIWithFont();
		}

		// Token: 0x06004D4E RID: 19790 RVA: 0x0013D0CD File Offset: 0x0013B2CD
		protected override void RescaleConstantsForDpi(int deviceDpiOld, int deviceDpiNew)
		{
			base.RescaleConstantsForDpi(deviceDpiOld, deviceDpiNew);
			if (DpiHelper.EnableDpiChangedHighDpiImprovements)
			{
				this.cBorder = base.LogicalToDeviceUnits(3);
				this.cydef = base.LogicalToDeviceUnits(59);
			}
		}

		// Token: 0x06004D4F RID: 19791 RVA: 0x0013D0FC File Offset: 0x0013B2FC
		public virtual void SetComment(string title, string desc)
		{
			if (this.m_labelDesc.Text != title)
			{
				this.m_labelTitle.Text = title;
			}
			if (desc != this.fullDesc)
			{
				this.fullDesc = desc;
				this.m_labelDesc.Text = this.fullDesc;
				this.m_labelDesc.AccessibleName = this.fullDesc;
			}
		}

		// Token: 0x06004D50 RID: 19792 RVA: 0x0013D160 File Offset: 0x0013B360
		public override int SnapHeightRequest(int cyNew)
		{
			this.UpdateUIWithFont();
			int num = Math.Max(2, cyNew / this.lineHeight);
			return 1 + num * this.lineHeight;
		}

		// Token: 0x06004D51 RID: 19793 RVA: 0x0013D18C File Offset: 0x0013B38C
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			if (AccessibilityImprovements.Level3)
			{
				return new DocCommentAccessibleObject(this, this.ownerGrid);
			}
			return base.CreateAccessibilityInstance();
		}

		// Token: 0x17001316 RID: 4886
		// (get) Token: 0x06004D52 RID: 19794 RVA: 0x000A010F File Offset: 0x0009E30F
		internal override bool SupportsUiaProviders
		{
			get
			{
				return AccessibilityImprovements.Level3;
			}
		}

		// Token: 0x06004D53 RID: 19795 RVA: 0x0013D1A8 File Offset: 0x0013B3A8
		internal void UpdateTextRenderingEngine()
		{
			this.m_labelTitle.UseCompatibleTextRendering = this.ownerGrid.UseCompatibleTextRendering;
			this.m_labelDesc.UseCompatibleTextRendering = this.ownerGrid.UseCompatibleTextRendering;
		}

		// Token: 0x06004D54 RID: 19796 RVA: 0x0013D1D8 File Offset: 0x0013B3D8
		private void UpdateUIWithFont()
		{
			if (base.IsHandleCreated && this.needUpdateUIWithFont)
			{
				try
				{
					this.m_labelTitle.Font = new Font(this.Font, FontStyle.Bold);
				}
				catch
				{
				}
				this.lineHeight = this.Font.Height + 2;
				this.m_labelTitle.Location = new Point(this.cBorder, this.cBorder);
				this.m_labelDesc.Location = new Point(this.cBorder, this.cBorder + this.lineHeight);
				this.needUpdateUIWithFont = false;
				base.PerformLayout();
			}
		}

		// Token: 0x040032E7 RID: 13031
		private Label m_labelTitle;

		// Token: 0x040032E8 RID: 13032
		private Label m_labelDesc;

		// Token: 0x040032E9 RID: 13033
		private string fullDesc;

		// Token: 0x040032EA RID: 13034
		protected int lineHeight;

		// Token: 0x040032EB RID: 13035
		private bool needUpdateUIWithFont = true;

		// Token: 0x040032EC RID: 13036
		protected const int CBORDER = 3;

		// Token: 0x040032ED RID: 13037
		protected const int CXDEF = 0;

		// Token: 0x040032EE RID: 13038
		protected const int CYDEF = 59;

		// Token: 0x040032EF RID: 13039
		protected const int MIN_LINES = 2;

		// Token: 0x040032F0 RID: 13040
		private int cydef = 59;

		// Token: 0x040032F1 RID: 13041
		private int cBorder = 3;

		// Token: 0x040032F2 RID: 13042
		internal Rectangle rect = Rectangle.Empty;
	}
}
