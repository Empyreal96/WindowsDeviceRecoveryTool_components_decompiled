using System;
using System.Security.Cryptography.X509Certificates;

namespace System.Security.Policy
{
	// Token: 0x02000505 RID: 1285
	internal class ParsedData
	{
		// Token: 0x1700144B RID: 5195
		// (get) Token: 0x06005452 RID: 21586 RVA: 0x001605AC File Offset: 0x0015E7AC
		// (set) Token: 0x06005453 RID: 21587 RVA: 0x001605B4 File Offset: 0x0015E7B4
		public bool RequestsShellIntegration
		{
			get
			{
				return this.requestsShellIntegration;
			}
			set
			{
				this.requestsShellIntegration = value;
			}
		}

		// Token: 0x1700144C RID: 5196
		// (get) Token: 0x06005454 RID: 21588 RVA: 0x001605BD File Offset: 0x0015E7BD
		// (set) Token: 0x06005455 RID: 21589 RVA: 0x001605C5 File Offset: 0x0015E7C5
		public X509Certificate2 Certificate
		{
			get
			{
				return this.certificate;
			}
			set
			{
				this.certificate = value;
			}
		}

		// Token: 0x1700144D RID: 5197
		// (get) Token: 0x06005456 RID: 21590 RVA: 0x001605CE File Offset: 0x0015E7CE
		// (set) Token: 0x06005457 RID: 21591 RVA: 0x001605D6 File Offset: 0x0015E7D6
		public string AppName
		{
			get
			{
				return this.appName;
			}
			set
			{
				this.appName = value;
			}
		}

		// Token: 0x1700144E RID: 5198
		// (get) Token: 0x06005458 RID: 21592 RVA: 0x001605DF File Offset: 0x0015E7DF
		// (set) Token: 0x06005459 RID: 21593 RVA: 0x001605E7 File Offset: 0x0015E7E7
		public string AppPublisher
		{
			get
			{
				return this.appPublisher;
			}
			set
			{
				this.appPublisher = value;
			}
		}

		// Token: 0x1700144F RID: 5199
		// (get) Token: 0x0600545A RID: 21594 RVA: 0x001605F0 File Offset: 0x0015E7F0
		// (set) Token: 0x0600545B RID: 21595 RVA: 0x001605F8 File Offset: 0x0015E7F8
		public string AuthenticodedPublisher
		{
			get
			{
				return this.authenticodedPublisher;
			}
			set
			{
				this.authenticodedPublisher = value;
			}
		}

		// Token: 0x17001450 RID: 5200
		// (get) Token: 0x0600545C RID: 21596 RVA: 0x00160601 File Offset: 0x0015E801
		// (set) Token: 0x0600545D RID: 21597 RVA: 0x00160609 File Offset: 0x0015E809
		public bool UseManifestForTrust
		{
			get
			{
				return this.disallowTrustOverride;
			}
			set
			{
				this.disallowTrustOverride = value;
			}
		}

		// Token: 0x17001451 RID: 5201
		// (get) Token: 0x0600545E RID: 21598 RVA: 0x00160612 File Offset: 0x0015E812
		// (set) Token: 0x0600545F RID: 21599 RVA: 0x0016061A File Offset: 0x0015E81A
		public string SupportUrl
		{
			get
			{
				return this.supportUrl;
			}
			set
			{
				this.supportUrl = value;
			}
		}

		// Token: 0x04003645 RID: 13893
		private bool requestsShellIntegration;

		// Token: 0x04003646 RID: 13894
		private string appName;

		// Token: 0x04003647 RID: 13895
		private string appPublisher;

		// Token: 0x04003648 RID: 13896
		private string supportUrl;

		// Token: 0x04003649 RID: 13897
		private string authenticodedPublisher;

		// Token: 0x0400364A RID: 13898
		private bool disallowTrustOverride;

		// Token: 0x0400364B RID: 13899
		private X509Certificate2 certificate;
	}
}
