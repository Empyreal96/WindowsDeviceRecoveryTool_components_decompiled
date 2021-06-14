using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows.Forms;

namespace System.Security.Policy
{
	// Token: 0x02000504 RID: 1284
	internal class TrustManagerPromptUIThread
	{
		// Token: 0x0600544E RID: 21582 RVA: 0x00160490 File Offset: 0x0015E690
		public TrustManagerPromptUIThread(string appName, string defaultBrowserExePath, string supportUrl, string deploymentUrl, string publisherName, X509Certificate2 certificate, TrustManagerPromptOptions options)
		{
			this.m_appName = appName;
			this.m_defaultBrowserExePath = defaultBrowserExePath;
			this.m_supportUrl = supportUrl;
			this.m_deploymentUrl = deploymentUrl;
			this.m_publisherName = publisherName;
			this.m_certificate = certificate;
			this.m_options = options;
		}

		// Token: 0x0600544F RID: 21583 RVA: 0x001604E0 File Offset: 0x0015E6E0
		public DialogResult ShowDialog()
		{
			Thread thread = new Thread(new ThreadStart(this.ShowDialogWork));
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			thread.Join();
			return this.m_ret;
		}

		// Token: 0x06005450 RID: 21584 RVA: 0x00160518 File Offset: 0x0015E718
		private void ShowDialogWork()
		{
			try
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				using (TrustManagerPromptUI trustManagerPromptUI = new TrustManagerPromptUI(this.m_appName, this.m_defaultBrowserExePath, this.m_supportUrl, this.m_deploymentUrl, this.m_publisherName, this.m_certificate, this.m_options))
				{
					this.m_ret = trustManagerPromptUI.ShowDialog();
				}
			}
			catch
			{
			}
			finally
			{
				Application.ExitThread();
			}
		}

		// Token: 0x0400363D RID: 13885
		private string m_appName;

		// Token: 0x0400363E RID: 13886
		private string m_defaultBrowserExePath;

		// Token: 0x0400363F RID: 13887
		private string m_supportUrl;

		// Token: 0x04003640 RID: 13888
		private string m_deploymentUrl;

		// Token: 0x04003641 RID: 13889
		private string m_publisherName;

		// Token: 0x04003642 RID: 13890
		private X509Certificate2 m_certificate;

		// Token: 0x04003643 RID: 13891
		private TrustManagerPromptOptions m_options;

		// Token: 0x04003644 RID: 13892
		private DialogResult m_ret = DialogResult.No;
	}
}
