using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Security.Permissions;
using System.Threading;

namespace System.Windows.Forms
{
	/// <summary>Controls how a document is printed from a Windows Forms application.</summary>
	// Token: 0x0200043C RID: 1084
	public class PrintControllerWithStatusDialog : PrintController
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.PrintControllerWithStatusDialog" /> class, wrapping the supplied <see cref="T:System.Drawing.Printing.PrintController" />.</summary>
		/// <param name="underlyingController">A <see cref="T:System.Drawing.Printing.PrintController" /> to encapsulate. </param>
		// Token: 0x06004B6D RID: 19309 RVA: 0x00136EBC File Offset: 0x001350BC
		public PrintControllerWithStatusDialog(PrintController underlyingController) : this(underlyingController, SR.GetString("PrintControllerWithStatusDialog_DialogTitlePrint"))
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.PrintControllerWithStatusDialog" /> class, wrapping the supplied <see cref="T:System.Drawing.Printing.PrintController" /> and specifying a title for the dialog box.</summary>
		/// <param name="underlyingController">A <see cref="T:System.Drawing.Printing.PrintController" /> to encapsulate. </param>
		/// <param name="dialogTitle">A <see cref="T:System.String" /> containing a title for the status dialog box. </param>
		// Token: 0x06004B6E RID: 19310 RVA: 0x00136ECF File Offset: 0x001350CF
		public PrintControllerWithStatusDialog(PrintController underlyingController, string dialogTitle)
		{
			this.underlyingController = underlyingController;
			this.dialogTitle = dialogTitle;
		}

		/// <summary>Gets a value indicating this <see cref="T:System.Windows.Forms.PrintControllerWithStatusDialog" /> is used for print preview.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.PrintControllerWithStatusDialog" /> is used for print preview, otherwise, <see langword="false" />.</returns>
		// Token: 0x1700127E RID: 4734
		// (get) Token: 0x06004B6F RID: 19311 RVA: 0x00136EE5 File Offset: 0x001350E5
		public override bool IsPreview
		{
			get
			{
				return this.underlyingController != null && this.underlyingController.IsPreview;
			}
		}

		/// <summary>Begins the control sequence that determines when and how to print a document.</summary>
		/// <param name="document">A <see cref="T:System.Drawing.Printing.PrintDocument" /> that represents the document currently being printed.</param>
		/// <param name="e">A <see cref="T:System.Drawing.Printing.PrintEventArgs" /> that contains the event data.</param>
		// Token: 0x06004B70 RID: 19312 RVA: 0x00136EFC File Offset: 0x001350FC
		public override void OnStartPrint(PrintDocument document, PrintEventArgs e)
		{
			base.OnStartPrint(document, e);
			this.document = document;
			this.pageNumber = 1;
			if (SystemInformation.UserInteractive)
			{
				this.backgroundThread = new PrintControllerWithStatusDialog.BackgroundThread(this);
			}
			try
			{
				this.underlyingController.OnStartPrint(document, e);
			}
			catch
			{
				if (this.backgroundThread != null)
				{
					this.backgroundThread.Stop();
				}
				throw;
			}
			finally
			{
				if (this.backgroundThread != null && this.backgroundThread.canceled)
				{
					e.Cancel = true;
				}
			}
		}

		/// <summary>Begins the control sequence that determines when and how to print a page of a document.</summary>
		/// <param name="document">A <see cref="T:System.Drawing.Printing.PrintDocument" /> that represents the document currently being printed.</param>
		/// <param name="e">A <see cref="T:System.Drawing.Printing.PrintPageEventArgs" /> that contains the event data.</param>
		/// <returns>A <see cref="T:System.Drawing.Graphics" /> object that represents a page from a <see cref="T:System.Drawing.Printing.PrintDocument" />.</returns>
		// Token: 0x06004B71 RID: 19313 RVA: 0x00136F94 File Offset: 0x00135194
		public override Graphics OnStartPage(PrintDocument document, PrintPageEventArgs e)
		{
			base.OnStartPage(document, e);
			if (this.backgroundThread != null)
			{
				this.backgroundThread.UpdateLabel();
			}
			Graphics result = this.underlyingController.OnStartPage(document, e);
			if (this.backgroundThread != null && this.backgroundThread.canceled)
			{
				e.Cancel = true;
			}
			return result;
		}

		/// <summary>Completes the control sequence that determines when and how to print a page of a document.</summary>
		/// <param name="document">A <see cref="T:System.Drawing.Printing.PrintDocument" /> that represents the document currently being printed.</param>
		/// <param name="e">A <see cref="T:System.Drawing.Printing.PrintPageEventArgs" /> that contains the event data.</param>
		// Token: 0x06004B72 RID: 19314 RVA: 0x00136FE8 File Offset: 0x001351E8
		public override void OnEndPage(PrintDocument document, PrintPageEventArgs e)
		{
			this.underlyingController.OnEndPage(document, e);
			if (this.backgroundThread != null && this.backgroundThread.canceled)
			{
				e.Cancel = true;
			}
			this.pageNumber++;
			base.OnEndPage(document, e);
		}

		/// <summary>Completes the control sequence that determines when and how to print a document.</summary>
		/// <param name="document">A <see cref="T:System.Drawing.Printing.PrintDocument" /> that represents the document currently being printed.</param>
		/// <param name="e">A <see cref="T:System.Drawing.Printing.PrintPageEventArgs" /> that contains the event data.</param>
		// Token: 0x06004B73 RID: 19315 RVA: 0x00137034 File Offset: 0x00135234
		public override void OnEndPrint(PrintDocument document, PrintEventArgs e)
		{
			this.underlyingController.OnEndPrint(document, e);
			if (this.backgroundThread != null && this.backgroundThread.canceled)
			{
				e.Cancel = true;
			}
			if (this.backgroundThread != null)
			{
				this.backgroundThread.Stop();
			}
			base.OnEndPrint(document, e);
		}

		// Token: 0x04002791 RID: 10129
		private PrintController underlyingController;

		// Token: 0x04002792 RID: 10130
		private PrintDocument document;

		// Token: 0x04002793 RID: 10131
		private PrintControllerWithStatusDialog.BackgroundThread backgroundThread;

		// Token: 0x04002794 RID: 10132
		private int pageNumber;

		// Token: 0x04002795 RID: 10133
		private string dialogTitle;

		// Token: 0x02000808 RID: 2056
		private class BackgroundThread
		{
			// Token: 0x06006E49 RID: 28233 RVA: 0x00193AC9 File Offset: 0x00191CC9
			internal BackgroundThread(PrintControllerWithStatusDialog parent)
			{
				this.parent = parent;
				this.thread = new Thread(new ThreadStart(this.Run));
				this.thread.SetApartmentState(ApartmentState.STA);
				this.thread.Start();
			}

			// Token: 0x06006E4A RID: 28234 RVA: 0x00193B08 File Offset: 0x00191D08
			[UIPermission(SecurityAction.Assert, Window = UIPermissionWindow.AllWindows)]
			[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
			private void Run()
			{
				try
				{
					lock (this)
					{
						if (this.alreadyStopped)
						{
							return;
						}
						this.dialog = new PrintControllerWithStatusDialog.StatusDialog(this, this.parent.dialogTitle);
						this.ThreadUnsafeUpdateLabel();
						this.dialog.Visible = true;
					}
					if (!this.alreadyStopped)
					{
						Application.Run(this.dialog);
					}
				}
				finally
				{
					lock (this)
					{
						if (this.dialog != null)
						{
							this.dialog.Dispose();
							this.dialog = null;
						}
					}
				}
			}

			// Token: 0x06006E4B RID: 28235 RVA: 0x00193BD0 File Offset: 0x00191DD0
			internal void Stop()
			{
				lock (this)
				{
					if (this.dialog != null && this.dialog.IsHandleCreated)
					{
						this.dialog.BeginInvoke(new MethodInvoker(this.dialog.Close));
					}
					else
					{
						this.alreadyStopped = true;
					}
				}
			}

			// Token: 0x06006E4C RID: 28236 RVA: 0x00193C40 File Offset: 0x00191E40
			private void ThreadUnsafeUpdateLabel()
			{
				this.dialog.label1.Text = SR.GetString("PrintControllerWithStatusDialog_NowPrinting", new object[]
				{
					this.parent.pageNumber,
					this.parent.document.DocumentName
				});
			}

			// Token: 0x06006E4D RID: 28237 RVA: 0x00193C93 File Offset: 0x00191E93
			internal void UpdateLabel()
			{
				if (this.dialog != null && this.dialog.IsHandleCreated)
				{
					this.dialog.BeginInvoke(new MethodInvoker(this.ThreadUnsafeUpdateLabel));
				}
			}

			// Token: 0x04004243 RID: 16963
			private PrintControllerWithStatusDialog parent;

			// Token: 0x04004244 RID: 16964
			private PrintControllerWithStatusDialog.StatusDialog dialog;

			// Token: 0x04004245 RID: 16965
			private Thread thread;

			// Token: 0x04004246 RID: 16966
			internal bool canceled;

			// Token: 0x04004247 RID: 16967
			private bool alreadyStopped;
		}

		// Token: 0x02000809 RID: 2057
		private class StatusDialog : Form
		{
			// Token: 0x06006E4E RID: 28238 RVA: 0x00193CC2 File Offset: 0x00191EC2
			internal StatusDialog(PrintControllerWithStatusDialog.BackgroundThread backgroundThread, string dialogTitle)
			{
				this.InitializeComponent();
				this.backgroundThread = backgroundThread;
				this.Text = dialogTitle;
				this.MinimumSize = base.Size;
			}

			// Token: 0x170017D3 RID: 6099
			// (get) Token: 0x06006E4F RID: 28239 RVA: 0x000EEE11 File Offset: 0x000ED011
			private static bool IsRTLResources
			{
				get
				{
					return SR.GetString("RTL") != "RTL_False";
				}
			}

			// Token: 0x06006E50 RID: 28240 RVA: 0x00193CEC File Offset: 0x00191EEC
			private void InitializeComponent()
			{
				if (PrintControllerWithStatusDialog.StatusDialog.IsRTLResources)
				{
					this.RightToLeft = RightToLeft.Yes;
				}
				this.tableLayoutPanel1 = new TableLayoutPanel();
				this.label1 = new Label();
				this.button1 = new Button();
				this.label1.AutoSize = true;
				this.label1.Location = new Point(8, 16);
				this.label1.TextAlign = ContentAlignment.MiddleCenter;
				this.label1.Size = new Size(240, 64);
				this.label1.TabIndex = 1;
				this.label1.Anchor = AnchorStyles.None;
				this.button1.AutoSize = true;
				this.button1.Size = new Size(75, 23);
				this.button1.TabIndex = 0;
				this.button1.Text = SR.GetString("PrintControllerWithStatusDialog_Cancel");
				this.button1.Location = new Point(88, 88);
				this.button1.Anchor = AnchorStyles.None;
				this.button1.Click += this.button1_Click;
				this.tableLayoutPanel1.AutoSize = true;
				this.tableLayoutPanel1.ColumnCount = 1;
				this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
				this.tableLayoutPanel1.Dock = DockStyle.Fill;
				this.tableLayoutPanel1.Location = new Point(0, 0);
				this.tableLayoutPanel1.RowCount = 2;
				this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
				this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
				this.tableLayoutPanel1.TabIndex = 0;
				this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
				this.tableLayoutPanel1.Controls.Add(this.button1, 0, 1);
				base.AutoScaleDimensions = new Size(6, 13);
				base.AutoScaleMode = AutoScaleMode.Font;
				base.MaximizeBox = false;
				base.ControlBox = false;
				base.MinimizeBox = false;
				Size size = new Size(256, 122);
				if (DpiHelper.IsScalingRequired)
				{
					base.ClientSize = DpiHelper.LogicalToDeviceUnits(size, 0);
				}
				else
				{
					base.ClientSize = size;
				}
				base.CancelButton = this.button1;
				base.SizeGripStyle = SizeGripStyle.Hide;
				base.Controls.Add(this.tableLayoutPanel1);
			}

			// Token: 0x06006E51 RID: 28241 RVA: 0x00193F4B File Offset: 0x0019214B
			private void button1_Click(object sender, EventArgs e)
			{
				this.button1.Enabled = false;
				this.label1.Text = SR.GetString("PrintControllerWithStatusDialog_Canceling");
				this.backgroundThread.canceled = true;
			}

			// Token: 0x04004248 RID: 16968
			internal Label label1;

			// Token: 0x04004249 RID: 16969
			private Button button1;

			// Token: 0x0400424A RID: 16970
			private TableLayoutPanel tableLayoutPanel1;

			// Token: 0x0400424B RID: 16971
			private PrintControllerWithStatusDialog.BackgroundThread backgroundThread;
		}
	}
}
