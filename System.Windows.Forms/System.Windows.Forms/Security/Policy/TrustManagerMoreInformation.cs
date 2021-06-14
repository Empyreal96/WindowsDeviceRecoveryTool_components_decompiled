using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Win32;

namespace System.Security.Policy
{
	// Token: 0x0200050A RID: 1290
	internal partial class TrustManagerMoreInformation : Form
	{
		// Token: 0x06005477 RID: 21623 RVA: 0x00161F94 File Offset: 0x00160194
		internal TrustManagerMoreInformation(TrustManagerPromptOptions options, string publisherName)
		{
			this.InitializeComponent();
			this.Font = SystemFonts.MessageBoxFont;
			this.lblMachineAccess.Font = (this.lblPublisher.Font = (this.lblInstallation.Font = (this.lblLocation.Font = new Font(this.lblMachineAccess.Font, FontStyle.Bold))));
			this.FillContent(options, publisherName);
			if (AccessibilityImprovements.Level2)
			{
				base.AccessibleName = this.Text;
				this.tableLayoutPanel.AccessibleName = string.Empty;
				this.pictureBoxPublisher.AccessibleName = this.pictureBoxPublisher.AccessibleDescription;
				this.pictureBoxPublisher.AccessibleRole = AccessibleRole.Graphic;
				this.lblPublisher.AccessibleName = this.lblPublisher.Text;
				this.lblPublisherContent.AccessibleName = this.lblPublisherContent.Text;
				this.pictureBoxMachineAccess.AccessibleName = this.pictureBoxMachineAccess.AccessibleDescription;
				this.pictureBoxMachineAccess.AccessibleRole = AccessibleRole.Graphic;
				this.lblMachineAccess.AccessibleName = this.lblMachineAccess.Text;
				this.lblMachineAccessContent.AccessibleName = this.lblMachineAccessContent.Text;
				this.pictureBoxInstallation.AccessibleName = this.pictureBoxInstallation.AccessibleDescription;
				this.pictureBoxInstallation.AccessibleRole = AccessibleRole.Graphic;
				this.lblInstallation.AccessibleName = this.lblInstallation.Text;
				this.lblInstallationContent.AccessibleName = this.lblInstallationContent.Text;
				this.pictureBoxLocation.AccessibleName = this.pictureBoxLocation.AccessibleDescription;
				this.pictureBoxLocation.AccessibleRole = AccessibleRole.Graphic;
				this.lblLocation.AccessibleName = this.lblLocation.Text;
				this.lblLocationContent.AccessibleName = this.lblLocationContent.Text;
				this.btnClose.AccessibleName = this.btnClose.Text;
				this.tableLayoutPanel.Controls.SetChildIndex(this.pictureBoxPublisher, 0);
				this.tableLayoutPanel.Controls.SetChildIndex(this.lblPublisher, 1);
				this.tableLayoutPanel.Controls.SetChildIndex(this.lblPublisherContent, 2);
				this.tableLayoutPanel.Controls.SetChildIndex(this.pictureBoxMachineAccess, 3);
				this.tableLayoutPanel.Controls.SetChildIndex(this.lblMachineAccess, 4);
				this.tableLayoutPanel.Controls.SetChildIndex(this.lblMachineAccessContent, 5);
				this.tableLayoutPanel.Controls.SetChildIndex(this.pictureBoxInstallation, 6);
				this.tableLayoutPanel.Controls.SetChildIndex(this.lblInstallation, 7);
				this.tableLayoutPanel.Controls.SetChildIndex(this.lblInstallationContent, 8);
				this.tableLayoutPanel.Controls.SetChildIndex(this.pictureBoxLocation, 9);
				this.tableLayoutPanel.Controls.SetChildIndex(this.lblLocation, 10);
				this.tableLayoutPanel.Controls.SetChildIndex(this.lblLocationContent, 11);
				this.tableLayoutPanel.Controls.SetChildIndex(this.btnClose, 12);
			}
		}

		// Token: 0x06005479 RID: 21625 RVA: 0x001622CC File Offset: 0x001604CC
		private void FillContent(TrustManagerPromptOptions options, string publisherName)
		{
			TrustManagerMoreInformation.LoadWarningBitmap((publisherName == null) ? TrustManagerWarningLevel.Red : TrustManagerWarningLevel.Green, this.pictureBoxPublisher);
			TrustManagerMoreInformation.LoadWarningBitmap(((options & (TrustManagerPromptOptions.RequiresPermissions | TrustManagerPromptOptions.WillHaveFullTrust)) != TrustManagerPromptOptions.None) ? TrustManagerWarningLevel.Red : TrustManagerWarningLevel.Green, this.pictureBoxMachineAccess);
			TrustManagerMoreInformation.LoadWarningBitmap(((options & TrustManagerPromptOptions.AddsShortcut) != TrustManagerPromptOptions.None) ? TrustManagerWarningLevel.Yellow : TrustManagerWarningLevel.Green, this.pictureBoxInstallation);
			TrustManagerWarningLevel warningLevel;
			if ((options & (TrustManagerPromptOptions.LocalNetworkSource | TrustManagerPromptOptions.LocalComputerSource | TrustManagerPromptOptions.TrustedSitesSource)) != TrustManagerPromptOptions.None)
			{
				warningLevel = TrustManagerWarningLevel.Green;
			}
			else if ((options & TrustManagerPromptOptions.UntrustedSitesSource) != TrustManagerPromptOptions.None)
			{
				warningLevel = TrustManagerWarningLevel.Red;
			}
			else
			{
				warningLevel = TrustManagerWarningLevel.Yellow;
			}
			TrustManagerMoreInformation.LoadWarningBitmap(warningLevel, this.pictureBoxLocation);
			if (publisherName == null)
			{
				this.lblPublisherContent.Text = SR.GetString("TrustManagerMoreInfo_UnknownPublisher");
			}
			else
			{
				this.lblPublisherContent.Text = SR.GetString("TrustManagerMoreInfo_KnownPublisher", new object[]
				{
					publisherName
				});
			}
			if ((options & (TrustManagerPromptOptions.RequiresPermissions | TrustManagerPromptOptions.WillHaveFullTrust)) != TrustManagerPromptOptions.None)
			{
				this.lblMachineAccessContent.Text = SR.GetString("TrustManagerMoreInfo_UnsafeAccess");
			}
			else
			{
				this.lblMachineAccessContent.Text = SR.GetString("TrustManagerMoreInfo_SafeAccess");
			}
			if ((options & TrustManagerPromptOptions.AddsShortcut) != TrustManagerPromptOptions.None)
			{
				this.Text = SR.GetString("TrustManagerMoreInfo_InstallTitle");
				this.lblInstallationContent.Text = SR.GetString("TrustManagerMoreInfo_WithShortcut");
			}
			else
			{
				this.Text = SR.GetString("TrustManagerMoreInfo_RunTitle");
				this.lblInstallationContent.Text = SR.GetString("TrustManagerMoreInfo_WithoutShortcut");
			}
			string @string;
			if ((options & TrustManagerPromptOptions.LocalNetworkSource) != TrustManagerPromptOptions.None)
			{
				@string = SR.GetString("TrustManagerMoreInfo_LocalNetworkSource");
			}
			else if ((options & TrustManagerPromptOptions.LocalComputerSource) != TrustManagerPromptOptions.None)
			{
				@string = SR.GetString("TrustManagerMoreInfo_LocalComputerSource");
			}
			else if ((options & TrustManagerPromptOptions.InternetSource) != TrustManagerPromptOptions.None)
			{
				@string = SR.GetString("TrustManagerMoreInfo_InternetSource");
			}
			else if ((options & TrustManagerPromptOptions.TrustedSitesSource) != TrustManagerPromptOptions.None)
			{
				@string = SR.GetString("TrustManagerMoreInfo_TrustedSitesSource");
			}
			else
			{
				@string = SR.GetString("TrustManagerMoreInfo_UntrustedSitesSource");
			}
			this.lblLocationContent.Text = SR.GetString("TrustManagerMoreInfo_Location", new object[]
			{
				@string
			});
		}

		// Token: 0x0600547B RID: 21627 RVA: 0x00162C10 File Offset: 0x00160E10
		private static void LoadWarningBitmap(TrustManagerWarningLevel warningLevel, PictureBox pictureBox)
		{
			Bitmap bitmap;
			if (warningLevel != TrustManagerWarningLevel.Green)
			{
				if (warningLevel != TrustManagerWarningLevel.Yellow)
				{
					if (!LocalAppContextSwitches.UseLegacyImages)
					{
						bitmap = TrustManagerMoreInformation.QueryDPiMatchedSmallBitmap("TrustManagerHighRisk.ico");
					}
					else
					{
						bitmap = new Bitmap(typeof(Form), "TrustManagerHighRiskSm.bmp");
					}
					pictureBox.AccessibleDescription = string.Format(CultureInfo.CurrentCulture, SR.GetString("TrustManager_WarningIconAccessibleDescription_HighRisk"), new object[]
					{
						pictureBox.AccessibleDescription
					});
				}
				else
				{
					if (!LocalAppContextSwitches.UseLegacyImages)
					{
						bitmap = TrustManagerMoreInformation.QueryDPiMatchedSmallBitmap("TrustManagerWarning.ico");
					}
					else
					{
						bitmap = new Bitmap(typeof(Form), "TrustManagerWarningSm.bmp");
					}
					pictureBox.AccessibleDescription = string.Format(CultureInfo.CurrentCulture, SR.GetString("TrustManager_WarningIconAccessibleDescription_MediumRisk"), new object[]
					{
						pictureBox.AccessibleDescription
					});
				}
			}
			else
			{
				if (!LocalAppContextSwitches.UseLegacyImages)
				{
					bitmap = TrustManagerMoreInformation.QueryDPiMatchedSmallBitmap("TrustManagerOK.ico");
				}
				else
				{
					bitmap = new Bitmap(typeof(Form), "TrustManagerOKSm.bmp");
				}
				pictureBox.AccessibleDescription = string.Format(CultureInfo.CurrentCulture, SR.GetString("TrustManager_WarningIconAccessibleDescription_LowRisk"), new object[]
				{
					pictureBox.AccessibleDescription
				});
			}
			if (bitmap != null)
			{
				bitmap.MakeTransparent();
				pictureBox.Image = bitmap;
			}
		}

		// Token: 0x0600547C RID: 21628 RVA: 0x00162D38 File Offset: 0x00160F38
		internal static Bitmap QueryDPiMatchedSmallBitmap(string iconName)
		{
			Icon icon = new Icon(typeof(Form), iconName);
			icon = new Icon(icon, icon.Width / 2, icon.Height / 2);
			if (icon != null)
			{
				return icon.ToBitmap();
			}
			return null;
		}

		// Token: 0x0600547D RID: 21629 RVA: 0x00162D78 File Offset: 0x00160F78
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			SystemEvents.UserPreferenceChanged += this.OnUserPreferenceChanged;
		}

		// Token: 0x0600547E RID: 21630 RVA: 0x00162D92 File Offset: 0x00160F92
		protected override void OnHandleDestroyed(EventArgs e)
		{
			SystemEvents.UserPreferenceChanged -= this.OnUserPreferenceChanged;
			base.OnHandleDestroyed(e);
		}

		// Token: 0x0600547F RID: 21631 RVA: 0x00162DAC File Offset: 0x00160FAC
		private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			if (e.Category == UserPreferenceCategory.Window)
			{
				this.Font = SystemFonts.MessageBoxFont;
				this.lblLocation.Font = (this.lblInstallation.Font = (this.lblMachineAccess.Font = (this.lblPublisher.Font = new Font(this.Font, FontStyle.Bold))));
			}
			base.Invalidate();
		}
	}
}
