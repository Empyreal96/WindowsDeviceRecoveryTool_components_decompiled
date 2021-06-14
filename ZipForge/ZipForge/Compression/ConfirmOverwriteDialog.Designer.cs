namespace ComponentAce.Compression
{
	// Token: 0x02000033 RID: 51
	internal partial class ConfirmOverwriteDialog : global::System.Windows.Forms.Form
	{
		// Token: 0x06000218 RID: 536 RVA: 0x0001630F File Offset: 0x0001530F
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00016330 File Offset: 0x00015330
		private void InitializeComponent()
		{
			this._cancelButton = new global::System.Windows.Forms.Button();
			this._noButton = new global::System.Windows.Forms.Button();
			this._yesToAllButton = new global::System.Windows.Forms.Button();
			this._yesButton = new global::System.Windows.Forms.Button();
			this._dialogTextLabel = new global::System.Windows.Forms.Label();
			base.SuspendLayout();
			this._cancelButton.Location = new global::System.Drawing.Point(347, 60);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new global::System.Drawing.Size(78, 27);
			this._cancelButton.TabIndex = 0;
			this._cancelButton.Text = "&Cancel";
			this._cancelButton.Click += new global::System.EventHandler(this.CancelButtonClick);
			this._noButton.Location = new global::System.Drawing.Point(260, 60);
			this._noButton.Name = "_noButton";
			this._noButton.Size = new global::System.Drawing.Size(81, 27);
			this._noButton.TabIndex = 0;
			this._noButton.Text = "&No";
			this._noButton.Click += new global::System.EventHandler(this.NoButtonClick);
			this._yesToAllButton.Location = new global::System.Drawing.Point(173, 60);
			this._yesToAllButton.Name = "_yesToAllButton";
			this._yesToAllButton.Size = new global::System.Drawing.Size(81, 27);
			this._yesToAllButton.TabIndex = 0;
			this._yesToAllButton.Text = "Yes to &All";
			this._yesToAllButton.Click += new global::System.EventHandler(this.YesToAllButtonClick);
			this._yesButton.Location = new global::System.Drawing.Point(86, 60);
			this._yesButton.Name = "_yesButton";
			this._yesButton.Size = new global::System.Drawing.Size(81, 27);
			this._yesButton.TabIndex = 0;
			this._yesButton.Text = "&Yes";
			this._yesButton.Click += new global::System.EventHandler(this.YesButtonClick);
			this._dialogTextLabel.AutoSize = true;
			this._dialogTextLabel.Location = new global::System.Drawing.Point(13, 13);
			this._dialogTextLabel.Name = "_dialogTextLabel";
			this._dialogTextLabel.Size = new global::System.Drawing.Size(24, 13);
			this._dialogTextLabel.TabIndex = 1;
			this._dialogTextLabel.Text = "test";
			base.ClientSize = new global::System.Drawing.Size(437, 101);
			base.Controls.Add(this._dialogTextLabel);
			base.Controls.Add(this._yesButton);
			base.Controls.Add(this._yesToAllButton);
			base.Controls.Add(this._noButton);
			base.Controls.Add(this._cancelButton);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.Name = "ConfirmOverwriteDialog";
			this.Text = "Confirm file overwrite";
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400014A RID: 330
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400014B RID: 331
		private global::System.Windows.Forms.Button _cancelButton;

		// Token: 0x0400014C RID: 332
		private global::System.Windows.Forms.Button _noButton;

		// Token: 0x0400014D RID: 333
		private global::System.Windows.Forms.Button _yesToAllButton;

		// Token: 0x0400014E RID: 334
		private global::System.Windows.Forms.Button _yesButton;

		// Token: 0x0400014F RID: 335
		private global::System.Windows.Forms.Label _dialogTextLabel;
	}
}
