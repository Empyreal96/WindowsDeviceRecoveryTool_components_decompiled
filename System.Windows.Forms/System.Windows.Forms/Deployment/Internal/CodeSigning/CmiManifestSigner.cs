using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace System.Deployment.Internal.CodeSigning
{
	// Token: 0x0200000F RID: 15
	internal class CmiManifestSigner
	{
		// Token: 0x06000045 RID: 69 RVA: 0x000027DB File Offset: 0x000009DB
		private CmiManifestSigner()
		{
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003C0C File Offset: 0x00001E0C
		internal CmiManifestSigner(AsymmetricAlgorithm strongNameKey) : this(strongNameKey, null)
		{
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003C18 File Offset: 0x00001E18
		internal CmiManifestSigner(AsymmetricAlgorithm strongNameKey, X509Certificate2 certificate)
		{
			if (strongNameKey == null)
			{
				throw new ArgumentNullException("strongNameKey");
			}
			if (!(strongNameKey is RSA))
			{
				throw new ArgumentNullException("strongNameKey");
			}
			this.m_strongNameKey = strongNameKey;
			this.m_certificate = certificate;
			this.m_certificates = new X509Certificate2Collection();
			this.m_includeOption = X509IncludeOption.ExcludeRoot;
			this.m_signerFlag = CmiManifestSignerFlag.None;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00003C75 File Offset: 0x00001E75
		internal AsymmetricAlgorithm StrongNameKey
		{
			get
			{
				return this.m_strongNameKey;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00003C7D File Offset: 0x00001E7D
		internal X509Certificate2 Certificate
		{
			get
			{
				return this.m_certificate;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00003C85 File Offset: 0x00001E85
		// (set) Token: 0x0600004B RID: 75 RVA: 0x00003C8D File Offset: 0x00001E8D
		internal string Description
		{
			get
			{
				return this.m_description;
			}
			set
			{
				this.m_description = value;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00003C96 File Offset: 0x00001E96
		// (set) Token: 0x0600004D RID: 77 RVA: 0x00003C9E File Offset: 0x00001E9E
		internal string DescriptionUrl
		{
			get
			{
				return this.m_url;
			}
			set
			{
				this.m_url = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00003CA7 File Offset: 0x00001EA7
		internal X509Certificate2Collection ExtraStore
		{
			get
			{
				return this.m_certificates;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00003CAF File Offset: 0x00001EAF
		// (set) Token: 0x06000050 RID: 80 RVA: 0x00003CB7 File Offset: 0x00001EB7
		internal X509IncludeOption IncludeOption
		{
			get
			{
				return this.m_includeOption;
			}
			set
			{
				if (value < X509IncludeOption.None || value > X509IncludeOption.WholeChain)
				{
					throw new ArgumentException("value");
				}
				if (this.m_includeOption == X509IncludeOption.None)
				{
					throw new NotSupportedException();
				}
				this.m_includeOption = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00003CE1 File Offset: 0x00001EE1
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00003CE9 File Offset: 0x00001EE9
		internal CmiManifestSignerFlag Flag
		{
			get
			{
				return this.m_signerFlag;
			}
			set
			{
				if ((value & ~CmiManifestSignerFlag.DontReplacePublicKeyToken) != CmiManifestSignerFlag.None)
				{
					throw new ArgumentException("value");
				}
				this.m_signerFlag = value;
			}
		}

		// Token: 0x040000C5 RID: 197
		private AsymmetricAlgorithm m_strongNameKey;

		// Token: 0x040000C6 RID: 198
		private X509Certificate2 m_certificate;

		// Token: 0x040000C7 RID: 199
		private string m_description;

		// Token: 0x040000C8 RID: 200
		private string m_url;

		// Token: 0x040000C9 RID: 201
		private X509Certificate2Collection m_certificates;

		// Token: 0x040000CA RID: 202
		private X509IncludeOption m_includeOption;

		// Token: 0x040000CB RID: 203
		private CmiManifestSignerFlag m_signerFlag;

		// Token: 0x040000CC RID: 204
		internal const uint CimManifestSignerFlagMask = 1U;
	}
}
