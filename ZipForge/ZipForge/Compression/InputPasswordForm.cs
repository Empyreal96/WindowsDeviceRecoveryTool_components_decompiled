using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ComponentAce.Compression
{
	// Token: 0x02000047 RID: 71
	internal partial class InputPasswordForm : Form
	{
		// Token: 0x060002F3 RID: 755 RVA: 0x00018FCE File Offset: 0x00017FCE
		public InputPasswordForm()
		{
			this.InitializeComponent();
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00018FFC File Offset: 0x00017FFC
		internal void InitializeComponent()
		{
			this.passwordBox1 = new TextBox();
			this.visiblePasswordCheckBox = new CheckBox();
			this.passwordBox2 = new TextBox();
			this.acceptButton = new Button();
			this.cancelButton = new Button();
			this.promptLabel = new Label();
			base.SuspendLayout();
			this.passwordBox1.Location = new Point(8, 96);
			this.passwordBox1.Name = "passwordBox1";
			this.passwordBox1.Size = new Size(176, 24);
			this.passwordBox1.TabIndex = 0;
			this.visiblePasswordCheckBox.Location = new Point(8, 64);
			this.visiblePasswordCheckBox.Name = "visiblePasswordCheckBox";
			this.visiblePasswordCheckBox.Size = new Size(176, 24);
			this.visiblePasswordCheckBox.TabIndex = 1;
			this.visiblePasswordCheckBox.Text = "Visible password";
			this.visiblePasswordCheckBox.CheckedChanged += this.VisiblePasswordCheckBoxCheckedChanged;
			this.passwordBox2.Location = new Point(8, 136);
			this.passwordBox2.Name = "passwordBox2";
			this.passwordBox2.Size = new Size(176, 24);
			this.passwordBox2.TabIndex = 0;
			this.acceptButton.DialogResult = DialogResult.OK;
			this.acceptButton.Location = new Point(8, 176);
			this.acceptButton.Name = "acceptButton";
			this.acceptButton.Size = new Size(80, 24);
			this.acceptButton.TabIndex = 2;
			this.acceptButton.Text = "Accept";
			this.acceptButton.Click += this.AcceptButtonClick;
			this.cancelButton.DialogResult = DialogResult.Cancel;
			this.cancelButton.Location = new Point(104, 176);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new Size(80, 24);
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "Cancel";
			this.promptLabel.Location = new Point(8, 8);
			this.promptLabel.Name = "promptLabel";
			this.promptLabel.Size = new Size(176, 40);
			this.promptLabel.TabIndex = 3;
			this.promptLabel.Text = "Type password";
			base.AcceptButton = this.acceptButton;
			this.AutoScaleBaseSize = new Size(5, 13);
			base.CancelButton = this.cancelButton;
			base.ClientSize = new Size(202, 223);
			base.Controls.Add(this.promptLabel);
			base.Controls.Add(this.acceptButton);
			base.Controls.Add(this.visiblePasswordCheckBox);
			base.Controls.Add(this.passwordBox1);
			base.Controls.Add(this.passwordBox2);
			base.Controls.Add(this.cancelButton);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			this.MaximumSize = new Size(210, 250);
			this.MinimumSize = new Size(210, 250);
			base.Name = "InputPasswordForm";
			this.Text = "Input password";
			base.Load += this.InputPasswordForm_Load;
			base.ResumeLayout(false);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00019388 File Offset: 0x00018388
		internal void VisiblePasswordCheckBoxCheckedChanged(object sender, EventArgs e)
		{
			if (this.visiblePasswordCheckBox.Checked)
			{
				this.passwordBox2.Text = "";
				this.passwordBox2.Visible = false;
				this.passwordBox1.PasswordChar = '\0';
				return;
			}
			this.passwordBox1.PasswordChar = '*';
			this.passwordBox2.Visible = true;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x000193E4 File Offset: 0x000183E4
		internal void InputPasswordForm_Load(object sender, EventArgs e)
		{
			this.visiblePasswordCheckBox.Checked = false;
			this.Password = "";
			this.passwordBox1.Text = "";
			this.passwordBox2.Text = "";
			this.passwordBox1.PasswordChar = '*';
			this.passwordBox2.PasswordChar = '*';
			base.ActiveControl = this.passwordBox1;
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00019450 File Offset: 0x00018450
		internal void AcceptButtonClick(object sender, EventArgs e)
		{
			if (this.visiblePasswordCheckBox.Checked)
			{
				this.Password = this.passwordBox1.Text;
				base.DialogResult = DialogResult.OK;
				return;
			}
			if (this.passwordBox1.Text.Equals(this.passwordBox2.Text))
			{
				this.Password = this.passwordBox1.Text;
				base.DialogResult = DialogResult.OK;
				return;
			}
			base.DialogResult = DialogResult.None;
			MessageBox.Show(this, "Type same password in both input fields!", "Invalid password!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			base.ActiveControl = this.passwordBox1;
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060002FA RID: 762 RVA: 0x000194EE File Offset: 0x000184EE
		// (set) Token: 0x060002F9 RID: 761 RVA: 0x000194E0 File Offset: 0x000184E0
		public string Prompt
		{
			get
			{
				return this.promptLabel.Text;
			}
			set
			{
				this.promptLabel.Text = value;
			}
		}

		// Token: 0x040001F3 RID: 499
		internal TextBox passwordBox1;

		// Token: 0x040001F4 RID: 500
		internal TextBox passwordBox2;

		// Token: 0x040001F5 RID: 501
		internal Button acceptButton;

		// Token: 0x040001F6 RID: 502
		internal Button cancelButton;

		// Token: 0x040001F7 RID: 503
		internal CheckBox visiblePasswordCheckBox;

		// Token: 0x040001F8 RID: 504
		internal Label promptLabel;

		// Token: 0x040001FA RID: 506
		public string Password;
	}
}
