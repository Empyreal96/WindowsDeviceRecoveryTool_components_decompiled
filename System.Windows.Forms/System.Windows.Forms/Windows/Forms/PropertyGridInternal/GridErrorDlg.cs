using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x02000486 RID: 1158
	internal partial class GridErrorDlg : Form
	{
		// Token: 0x17001359 RID: 4953
		// (get) Token: 0x06004E2A RID: 20010 RVA: 0x0014030C File Offset: 0x0013E50C
		public bool DetailsButtonExpanded
		{
			get
			{
				return this.detailsButtonExpanded;
			}
		}

		// Token: 0x1700135A RID: 4954
		// (set) Token: 0x06004E2B RID: 20011 RVA: 0x00140314 File Offset: 0x0013E514
		public string Details
		{
			set
			{
				this.details.Text = value;
			}
		}

		// Token: 0x1700135B RID: 4955
		// (set) Token: 0x06004E2C RID: 20012 RVA: 0x00140322 File Offset: 0x0013E522
		public string Message
		{
			set
			{
				this.lblMessage.Text = value;
			}
		}

		// Token: 0x06004E2D RID: 20013 RVA: 0x00140330 File Offset: 0x0013E530
		public GridErrorDlg(PropertyGrid owner)
		{
			this.ownerGrid = owner;
			this.expandImage = new Bitmap(typeof(ThreadExceptionDialog), "down.bmp");
			this.expandImage.MakeTransparent();
			if (DpiHelper.IsScalingRequired)
			{
				DpiHelper.ScaleBitmapLogicalToDevice(ref this.expandImage, 0);
			}
			this.collapseImage = new Bitmap(typeof(ThreadExceptionDialog), "up.bmp");
			this.collapseImage.MakeTransparent();
			if (DpiHelper.IsScalingRequired)
			{
				DpiHelper.ScaleBitmapLogicalToDevice(ref this.collapseImage, 0);
			}
			this.InitializeComponent();
			foreach (object obj in base.Controls)
			{
				Control control = (Control)obj;
				if (control.SupportsUseCompatibleTextRendering)
				{
					control.UseCompatibleTextRenderingInt = this.ownerGrid.UseCompatibleTextRendering;
				}
			}
			this.pictureBox.Image = SystemIcons.Warning.ToBitmap();
			this.detailsBtn.Text = " " + SR.GetString("ExDlgShowDetails");
			this.details.AccessibleName = SR.GetString("ExDlgDetailsText");
			this.okBtn.Text = SR.GetString("ExDlgOk");
			this.cancelBtn.Text = SR.GetString("ExDlgCancel");
			this.detailsBtn.Image = this.expandImage;
		}

		// Token: 0x06004E2E RID: 20014 RVA: 0x001404A4 File Offset: 0x0013E6A4
		private void DetailsClick(object sender, EventArgs devent)
		{
			int num = this.details.Height + 8;
			if (this.details.Visible)
			{
				this.detailsBtn.Image = this.expandImage;
				this.detailsButtonExpanded = false;
				base.Height -= num;
			}
			else
			{
				this.detailsBtn.Image = this.collapseImage;
				this.detailsButtonExpanded = true;
				this.details.Width = this.overarchingTableLayoutPanel.Width - this.details.Margin.Horizontal;
				base.Height += num;
			}
			this.details.Visible = !this.details.Visible;
			if (AccessibilityImprovements.Level1)
			{
				base.AccessibilityNotifyClients(AccessibleEvents.StateChange, -1);
				base.AccessibilityNotifyClients(AccessibleEvents.NameChange, -1);
				this.details.TabStop = !this.details.TabStop;
				if (this.details.Visible)
				{
					this.details.Focus();
				}
			}
		}

		// Token: 0x1700135C RID: 4956
		// (get) Token: 0x06004E2F RID: 20015 RVA: 0x000EEE11 File Offset: 0x000ED011
		private static bool IsRTLResources
		{
			get
			{
				return SR.GetString("RTL") != "RTL_False";
			}
		}

		// Token: 0x06004E31 RID: 20017 RVA: 0x00140D4F File Offset: 0x0013EF4F
		private void OnButtonClick(object s, EventArgs e)
		{
			base.DialogResult = ((Button)s).DialogResult;
			base.Close();
		}

		// Token: 0x06004E32 RID: 20018 RVA: 0x00140D68 File Offset: 0x0013EF68
		protected override void OnVisibleChanged(EventArgs e)
		{
			if (base.Visible)
			{
				using (Graphics graphics = base.CreateGraphics())
				{
					int num = (int)Math.Ceiling((double)PropertyGrid.MeasureTextHelper.MeasureText(this.ownerGrid, graphics, this.detailsBtn.Text, this.detailsBtn.Font).Width);
					num += this.detailsBtn.Image.Width;
					this.detailsBtn.Width = (int)Math.Ceiling((double)((float)num * (this.ownerGrid.UseCompatibleTextRendering ? 1.15f : 1.4f)));
					this.detailsBtn.Height = this.okBtn.Height;
				}
				int x = this.details.Location.X;
				int num2 = this.detailsBtn.Location.Y + this.detailsBtn.Height + this.detailsBtn.Margin.Bottom;
				Control parent = this.detailsBtn.Parent;
				while (parent != null && !(parent is Form))
				{
					num2 += parent.Location.Y;
					parent = parent.Parent;
				}
				this.details.Location = new Point(x, num2);
				if (this.details.Visible)
				{
					this.DetailsClick(this.details, EventArgs.Empty);
				}
			}
			this.okBtn.Focus();
		}

		// Token: 0x0400333B RID: 13115
		private Bitmap expandImage;

		// Token: 0x0400333C RID: 13116
		private Bitmap collapseImage;

		// Token: 0x0400333D RID: 13117
		private PropertyGrid ownerGrid;

		// Token: 0x0400333E RID: 13118
		private bool detailsButtonExpanded;
	}
}
