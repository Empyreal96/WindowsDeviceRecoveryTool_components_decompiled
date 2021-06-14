using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System.Windows.Forms
{
	/// <summary>Implements a dialog box that is displayed when an unhandled exception occurs in a thread.</summary>
	// Token: 0x02000395 RID: 917
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	[UIPermission(SecurityAction.Assert, Window = UIPermissionWindow.AllWindows)]
	public partial class ThreadExceptionDialog : Form
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ThreadExceptionDialog" /> class.</summary>
		/// <param name="t">The <see cref="T:System.Exception" /> that represents the exception that occurred. </param>
		// Token: 0x06003A0E RID: 14862 RVA: 0x00101A78 File Offset: 0x000FFC78
		public ThreadExceptionDialog(Exception t)
		{
			if (DpiHelper.EnableThreadExceptionDialogHighDpiImprovements)
			{
				this.scaledMaxWidth = base.LogicalToDeviceUnits(440);
				this.scaledMaxHeight = base.LogicalToDeviceUnits(325);
				this.scaledPaddingWidth = base.LogicalToDeviceUnits(84);
				this.scaledPaddingHeight = base.LogicalToDeviceUnits(26);
				this.scaledMaxTextWidth = base.LogicalToDeviceUnits(180);
				this.scaledMaxTextHeight = base.LogicalToDeviceUnits(40);
				this.scaledButtonTopPadding = base.LogicalToDeviceUnits(31);
				this.scaledButtonDetailsLeftPadding = base.LogicalToDeviceUnits(8);
				this.scaledMessageTopPadding = base.LogicalToDeviceUnits(8);
				this.scaledHeightPadding = base.LogicalToDeviceUnits(8);
				this.scaledButtonWidth = base.LogicalToDeviceUnits(100);
				this.scaledButtonHeight = base.LogicalToDeviceUnits(23);
				this.scaledButtonAlignmentWidth = base.LogicalToDeviceUnits(105);
				this.scaledButtonAlignmentPadding = base.LogicalToDeviceUnits(5);
				this.scaledDetailsWidthPadding = base.LogicalToDeviceUnits(16);
				this.scaledDetailsHeight = base.LogicalToDeviceUnits(154);
				this.scaledPictureWidth = base.LogicalToDeviceUnits(64);
				this.scaledPictureHeight = base.LogicalToDeviceUnits(64);
				this.scaledExceptionMessageVerticalPadding = base.LogicalToDeviceUnits(4);
			}
			bool flag = false;
			WarningException ex = t as WarningException;
			string name;
			string text;
			Button[] array;
			if (ex != null)
			{
				name = "ExDlgWarningText";
				text = ex.Message;
				if (ex.HelpUrl == null)
				{
					array = new Button[]
					{
						this.continueButton
					};
				}
				else
				{
					array = new Button[]
					{
						this.continueButton,
						this.helpButton
					};
				}
			}
			else
			{
				text = t.Message;
				flag = true;
				if (Application.AllowQuit)
				{
					if (t is SecurityException)
					{
						name = "ExDlgSecurityErrorText";
					}
					else
					{
						name = "ExDlgErrorText";
					}
					array = new Button[]
					{
						this.detailsButton,
						this.continueButton,
						this.quitButton
					};
				}
				else
				{
					if (t is SecurityException)
					{
						name = "ExDlgSecurityContinueErrorText";
					}
					else
					{
						name = "ExDlgContinueErrorText";
					}
					array = new Button[]
					{
						this.detailsButton,
						this.continueButton
					};
				}
			}
			if (text.Length == 0)
			{
				text = t.GetType().Name;
			}
			if (t is SecurityException)
			{
				text = SR.GetString(name, new object[]
				{
					t.GetType().Name,
					ThreadExceptionDialog.Trim(text)
				});
			}
			else
			{
				text = SR.GetString(name, new object[]
				{
					ThreadExceptionDialog.Trim(text)
				});
			}
			StringBuilder stringBuilder = new StringBuilder();
			string value = "\r\n";
			string @string = SR.GetString("ExDlgMsgSeperator");
			string string2 = SR.GetString("ExDlgMsgSectionSeperator");
			if (Application.CustomThreadExceptionHandlerAttached)
			{
				stringBuilder.Append(SR.GetString("ExDlgMsgHeaderNonSwitchable"));
			}
			else
			{
				stringBuilder.Append(SR.GetString("ExDlgMsgHeaderSwitchable"));
			}
			stringBuilder.Append(string.Format(CultureInfo.CurrentCulture, string2, new object[]
			{
				SR.GetString("ExDlgMsgExceptionSection")
			}));
			stringBuilder.Append(t.ToString());
			stringBuilder.Append(value);
			stringBuilder.Append(value);
			stringBuilder.Append(string.Format(CultureInfo.CurrentCulture, string2, new object[]
			{
				SR.GetString("ExDlgMsgLoadedAssembliesSection")
			}));
			new FileIOPermission(PermissionState.Unrestricted).Assert();
			try
			{
				foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					AssemblyName name2 = assembly.GetName();
					string text2 = SR.GetString("NotAvailable");
					try
					{
						if (name2.EscapedCodeBase != null && name2.EscapedCodeBase.Length > 0)
						{
							Uri uri = new Uri(name2.EscapedCodeBase);
							if (uri.Scheme == "file")
							{
								text2 = FileVersionInfo.GetVersionInfo(NativeMethods.GetLocalPath(name2.EscapedCodeBase)).FileVersion;
							}
						}
					}
					catch (FileNotFoundException)
					{
					}
					stringBuilder.Append(SR.GetString("ExDlgMsgLoadedAssembliesEntry", new object[]
					{
						name2.Name,
						name2.Version,
						text2,
						name2.EscapedCodeBase
					}));
					stringBuilder.Append(@string);
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			stringBuilder.Append(string.Format(CultureInfo.CurrentCulture, string2, new object[]
			{
				SR.GetString("ExDlgMsgJITDebuggingSection")
			}));
			if (Application.CustomThreadExceptionHandlerAttached)
			{
				stringBuilder.Append(SR.GetString("ExDlgMsgFooterNonSwitchable"));
			}
			else
			{
				stringBuilder.Append(SR.GetString("ExDlgMsgFooterSwitchable"));
			}
			stringBuilder.Append(value);
			stringBuilder.Append(value);
			string text3 = stringBuilder.ToString();
			Graphics graphics = this.message.CreateGraphicsInternal();
			Size proposedSize = new Size(this.scaledMaxWidth - this.scaledPaddingWidth, int.MaxValue);
			if (DpiHelper.EnableThreadExceptionDialogHighDpiImprovements && !Control.UseCompatibleTextRenderingDefault)
			{
				proposedSize = Size.Ceiling(TextRenderer.MeasureText(text, this.Font, proposedSize, TextFormatFlags.WordBreak));
			}
			else
			{
				proposedSize = Size.Ceiling(graphics.MeasureString(text, this.Font, proposedSize.Width));
			}
			proposedSize.Height += this.scaledExceptionMessageVerticalPadding;
			graphics.Dispose();
			if (proposedSize.Width < this.scaledMaxTextWidth)
			{
				proposedSize.Width = this.scaledMaxTextWidth;
			}
			if (proposedSize.Height > this.scaledMaxHeight)
			{
				proposedSize.Height = this.scaledMaxHeight;
			}
			int num = proposedSize.Width + this.scaledPaddingWidth;
			int num2 = Math.Max(proposedSize.Height, this.scaledMaxTextHeight) + this.scaledPaddingHeight;
			IntSecurity.GetParent.Assert();
			try
			{
				Form activeForm = Form.ActiveForm;
				if (activeForm == null || activeForm.Text.Length == 0)
				{
					this.Text = SR.GetString("ExDlgCaption");
				}
				else
				{
					this.Text = SR.GetString("ExDlgCaption2", new object[]
					{
						activeForm.Text
					});
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			base.AcceptButton = this.continueButton;
			base.CancelButton = this.continueButton;
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			base.Icon = null;
			base.ClientSize = new Size(num, num2 + this.scaledButtonTopPadding);
			base.TopMost = true;
			this.pictureBox.Location = new Point(this.scaledPictureWidth / 8, this.scaledPictureHeight / 8);
			this.pictureBox.Size = new Size(this.scaledPictureWidth * 3 / 4, this.scaledPictureHeight * 3 / 4);
			this.pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
			if (t is SecurityException)
			{
				this.pictureBox.Image = SystemIcons.Information.ToBitmap();
			}
			else
			{
				this.pictureBox.Image = SystemIcons.Error.ToBitmap();
			}
			base.Controls.Add(this.pictureBox);
			this.message.SetBounds(this.scaledPictureWidth, this.scaledMessageTopPadding + (this.scaledMaxTextHeight - Math.Min(proposedSize.Height, this.scaledMaxTextHeight)) / 2, proposedSize.Width, proposedSize.Height);
			this.message.Text = text;
			base.Controls.Add(this.message);
			this.continueButton.Text = SR.GetString("ExDlgContinue");
			this.continueButton.FlatStyle = FlatStyle.Standard;
			this.continueButton.DialogResult = DialogResult.Cancel;
			this.quitButton.Text = SR.GetString("ExDlgQuit");
			this.quitButton.FlatStyle = FlatStyle.Standard;
			this.quitButton.DialogResult = DialogResult.Abort;
			this.helpButton.Text = SR.GetString("ExDlgHelp");
			this.helpButton.FlatStyle = FlatStyle.Standard;
			this.helpButton.DialogResult = DialogResult.Yes;
			this.detailsButton.Text = SR.GetString("ExDlgShowDetails");
			this.detailsButton.FlatStyle = FlatStyle.Standard;
			this.detailsButton.Click += this.DetailsClick;
			int num3 = 0;
			if (flag)
			{
				Button button = this.detailsButton;
				this.expandImage = new Bitmap(base.GetType(), "down.bmp");
				this.expandImage.MakeTransparent();
				this.collapseImage = new Bitmap(base.GetType(), "up.bmp");
				this.collapseImage.MakeTransparent();
				if (DpiHelper.EnableThreadExceptionDialogHighDpiImprovements)
				{
					base.ScaleBitmapLogicalToDevice(ref this.expandImage);
					base.ScaleBitmapLogicalToDevice(ref this.collapseImage);
				}
				button.SetBounds(this.scaledButtonDetailsLeftPadding, num2, this.scaledButtonWidth, this.scaledButtonHeight);
				button.Image = this.expandImage;
				button.ImageAlign = ContentAlignment.MiddleLeft;
				base.Controls.Add(button);
				num3 = 1;
			}
			int num4 = num - this.scaledButtonDetailsLeftPadding - ((array.Length - num3) * this.scaledButtonAlignmentWidth - this.scaledButtonAlignmentPadding);
			for (int j = num3; j < array.Length; j++)
			{
				Button button = array[j];
				button.SetBounds(num4, num2, this.scaledButtonWidth, this.scaledButtonHeight);
				base.Controls.Add(button);
				num4 += this.scaledButtonAlignmentWidth;
			}
			this.details.Text = text3;
			this.details.ScrollBars = ScrollBars.Both;
			this.details.Multiline = true;
			this.details.ReadOnly = true;
			this.details.WordWrap = false;
			this.details.TabStop = false;
			this.details.AcceptsReturn = false;
			this.details.SetBounds(this.scaledButtonDetailsLeftPadding, num2 + this.scaledButtonTopPadding, num - this.scaledDetailsWidthPadding, this.scaledDetailsHeight);
			this.details.Visible = this.detailsVisible;
			base.Controls.Add(this.details);
			if (DpiHelper.EnableThreadExceptionDialogHighDpiImprovements)
			{
				base.DpiChanged += this.ThreadExceptionDialog_DpiChanged;
			}
		}

		// Token: 0x06003A0F RID: 14863 RVA: 0x00102530 File Offset: 0x00100730
		private void ThreadExceptionDialog_DpiChanged(object sender, DpiChangedEventArgs e)
		{
			if (this.expandImage != null)
			{
				this.expandImage.Dispose();
			}
			this.expandImage = new Bitmap(base.GetType(), "down.bmp");
			this.expandImage.MakeTransparent();
			if (this.collapseImage != null)
			{
				this.collapseImage.Dispose();
			}
			this.collapseImage = new Bitmap(base.GetType(), "up.bmp");
			this.collapseImage.MakeTransparent();
			base.ScaleBitmapLogicalToDevice(ref this.expandImage);
			base.ScaleBitmapLogicalToDevice(ref this.collapseImage);
			this.detailsButton.Image = (this.detailsVisible ? this.collapseImage : this.expandImage);
		}

		/// <summary>Gets or sets a value indicating whether the dialog box automatically sizes to its content.</summary>
		/// <returns>
		///     <see langword="true" /> if the dialog box automatically sizes; otherwise, <see langword="false" />. </returns>
		// Token: 0x17000E7E RID: 3710
		// (get) Token: 0x06003A10 RID: 14864 RVA: 0x001025DE File Offset: 0x001007DE
		// (set) Token: 0x06003A11 RID: 14865 RVA: 0x001025E6 File Offset: 0x001007E6
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ThreadExceptionDialog.AutoSize" /> property changes.</summary>
		// Token: 0x140002D8 RID: 728
		// (add) Token: 0x06003A12 RID: 14866 RVA: 0x001025EF File Offset: 0x001007EF
		// (remove) Token: 0x06003A13 RID: 14867 RVA: 0x001025F8 File Offset: 0x001007F8
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler AutoSizeChanged
		{
			add
			{
				base.AutoSizeChanged += value;
			}
			remove
			{
				base.AutoSizeChanged -= value;
			}
		}

		// Token: 0x06003A14 RID: 14868 RVA: 0x00102604 File Offset: 0x00100804
		private void DetailsClick(object sender, EventArgs eventargs)
		{
			int num = this.details.Height + this.scaledHeightPadding;
			if (this.detailsVisible)
			{
				num = -num;
			}
			base.Height += num;
			this.detailsVisible = !this.detailsVisible;
			this.details.Visible = this.detailsVisible;
			this.detailsButton.Image = (this.detailsVisible ? this.collapseImage : this.expandImage);
		}

		// Token: 0x06003A15 RID: 14869 RVA: 0x00102680 File Offset: 0x00100880
		private static string Trim(string s)
		{
			if (s == null)
			{
				return s;
			}
			int num = s.Length;
			while (num > 0 && s[num - 1] == '.')
			{
				num--;
			}
			return s.Substring(0, num);
		}

		// Token: 0x040022BC RID: 8892
		private const string DownBitmapName = "down.bmp";

		// Token: 0x040022BD RID: 8893
		private const string UpBitmapName = "up.bmp";

		// Token: 0x040022BE RID: 8894
		private const int MAXWIDTH = 440;

		// Token: 0x040022BF RID: 8895
		private const int MAXHEIGHT = 325;

		// Token: 0x040022C0 RID: 8896
		private const int PADDINGWIDTH = 84;

		// Token: 0x040022C1 RID: 8897
		private const int PADDINGHEIGHT = 26;

		// Token: 0x040022C2 RID: 8898
		private const int MAXTEXTWIDTH = 180;

		// Token: 0x040022C3 RID: 8899
		private const int MAXTEXTHEIGHT = 40;

		// Token: 0x040022C4 RID: 8900
		private const int BUTTONTOPPADDING = 31;

		// Token: 0x040022C5 RID: 8901
		private const int BUTTONDETAILS_LEFTPADDING = 8;

		// Token: 0x040022C6 RID: 8902
		private const int MESSAGE_TOPPADDING = 8;

		// Token: 0x040022C7 RID: 8903
		private const int HEIGHTPADDING = 8;

		// Token: 0x040022C8 RID: 8904
		private const int BUTTONWIDTH = 100;

		// Token: 0x040022C9 RID: 8905
		private const int BUTTONHEIGHT = 23;

		// Token: 0x040022CA RID: 8906
		private const int BUTTONALIGNMENTWIDTH = 105;

		// Token: 0x040022CB RID: 8907
		private const int BUTTONALIGNMENTPADDING = 5;

		// Token: 0x040022CC RID: 8908
		private const int DETAILSWIDTHPADDING = 16;

		// Token: 0x040022CD RID: 8909
		private const int DETAILSHEIGHT = 154;

		// Token: 0x040022CE RID: 8910
		private const int PICTUREWIDTH = 64;

		// Token: 0x040022CF RID: 8911
		private const int PICTUREHEIGHT = 64;

		// Token: 0x040022D0 RID: 8912
		private const int EXCEPTIONMESSAGEVERTICALPADDING = 4;

		// Token: 0x040022D1 RID: 8913
		private int scaledMaxWidth = 440;

		// Token: 0x040022D2 RID: 8914
		private int scaledMaxHeight = 325;

		// Token: 0x040022D3 RID: 8915
		private int scaledPaddingWidth = 84;

		// Token: 0x040022D4 RID: 8916
		private int scaledPaddingHeight = 26;

		// Token: 0x040022D5 RID: 8917
		private int scaledMaxTextWidth = 180;

		// Token: 0x040022D6 RID: 8918
		private int scaledMaxTextHeight = 40;

		// Token: 0x040022D7 RID: 8919
		private int scaledButtonTopPadding = 31;

		// Token: 0x040022D8 RID: 8920
		private int scaledButtonDetailsLeftPadding = 8;

		// Token: 0x040022D9 RID: 8921
		private int scaledMessageTopPadding = 8;

		// Token: 0x040022DA RID: 8922
		private int scaledHeightPadding = 8;

		// Token: 0x040022DB RID: 8923
		private int scaledButtonWidth = 100;

		// Token: 0x040022DC RID: 8924
		private int scaledButtonHeight = 23;

		// Token: 0x040022DD RID: 8925
		private int scaledButtonAlignmentWidth = 105;

		// Token: 0x040022DE RID: 8926
		private int scaledButtonAlignmentPadding = 5;

		// Token: 0x040022DF RID: 8927
		private int scaledDetailsWidthPadding = 16;

		// Token: 0x040022E0 RID: 8928
		private int scaledDetailsHeight = 154;

		// Token: 0x040022E1 RID: 8929
		private int scaledPictureWidth = 64;

		// Token: 0x040022E2 RID: 8930
		private int scaledPictureHeight = 64;

		// Token: 0x040022E3 RID: 8931
		private int scaledExceptionMessageVerticalPadding = 4;

		// Token: 0x040022E4 RID: 8932
		private PictureBox pictureBox = new PictureBox();

		// Token: 0x040022E5 RID: 8933
		private Label message = new Label();

		// Token: 0x040022E6 RID: 8934
		private Button continueButton = new Button();

		// Token: 0x040022E7 RID: 8935
		private Button quitButton = new Button();

		// Token: 0x040022E8 RID: 8936
		private Button detailsButton = new Button();

		// Token: 0x040022E9 RID: 8937
		private Button helpButton = new Button();

		// Token: 0x040022EA RID: 8938
		private TextBox details = new TextBox();

		// Token: 0x040022EB RID: 8939
		private Bitmap expandImage;

		// Token: 0x040022EC RID: 8940
		private Bitmap collapseImage;

		// Token: 0x040022ED RID: 8941
		private bool detailsVisible;
	}
}
