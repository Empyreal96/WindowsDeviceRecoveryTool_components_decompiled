using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ComponentAce.Compression
{
	// Token: 0x02000033 RID: 51
	internal partial class ConfirmOverwriteDialog : Form
	{
		// Token: 0x0600021A RID: 538 RVA: 0x00016627 File Offset: 0x00015627
		public ConfirmOverwriteDialog()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00016635 File Offset: 0x00015635
		private void YesButtonClick(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Yes;
			base.Close();
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00016644 File Offset: 0x00015644
		private void YesToAllButtonClick(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00016653 File Offset: 0x00015653
		private void NoButtonClick(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.No;
			base.Close();
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00016662 File Offset: 0x00015662
		private void CancelButtonClick(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600021F RID: 543 RVA: 0x00016671 File Offset: 0x00015671
		// (set) Token: 0x06000220 RID: 544 RVA: 0x0001667E File Offset: 0x0001567E
		public string DialogText
		{
			get
			{
				return this._dialogTextLabel.Text;
			}
			set
			{
				this._dialogTextLabel.Text = value;
			}
		}
	}
}
