using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace System.Deployment.Internal.CodeSigning
{
	// Token: 0x02000016 RID: 22
	internal class CmiManifestSigner2
	{
		// Token: 0x0600009F RID: 159 RVA: 0x000027DB File Offset: 0x000009DB
		private CmiManifestSigner2()
		{
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000685F File Offset: 0x00004A5F
		internal CmiManifestSigner2(AsymmetricAlgorithm strongNameKey) : this(strongNameKey, null, false)
		{
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000686C File Offset: 0x00004A6C
		internal CmiManifestSigner2(AsymmetricAlgorithm strongNameKey, X509Certificate2 certificate, bool useSha256)
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
			this.m_useSha256 = useSha256;
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x000068D0 File Offset: 0x00004AD0
		internal bool UseSha256
		{
			get
			{
				return this.m_useSha256;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x000068D8 File Offset: 0x00004AD8
		internal AsymmetricAlgorithm StrongNameKey
		{
			get
			{
				return this.m_strongNameKey;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x000068E0 File Offset: 0x00004AE0
		internal X509Certificate2 Certificate
		{
			get
			{
				return this.m_certificate;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x000068E8 File Offset: 0x00004AE8
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x000068F0 File Offset: 0x00004AF0
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

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x000068F9 File Offset: 0x00004AF9
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x00006901 File Offset: 0x00004B01
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

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x0000690A File Offset: 0x00004B0A
		internal X509Certificate2Collection ExtraStore
		{
			get
			{
				return this.m_certificates;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00006912 File Offset: 0x00004B12
		// (set) Token: 0x060000AB RID: 171 RVA: 0x0000691A File Offset: 0x00004B1A
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

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00006944 File Offset: 0x00004B44
		// (set) Token: 0x060000AD RID: 173 RVA: 0x0000694C File Offset: 0x00004B4C
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

		// Token: 0x040000F1 RID: 241
		private AsymmetricAlgorithm m_strongNameKey;

		// Token: 0x040000F2 RID: 242
		private X509Certificate2 m_certificate;

		// Token: 0x040000F3 RID: 243
		private string m_description;

		// Token: 0x040000F4 RID: 244
		private string m_url;

		// Token: 0x040000F5 RID: 245
		private X509Certificate2Collection m_certificates;

		// Token: 0x040000F6 RID: 246
		private X509IncludeOption m_includeOption;

		// Token: 0x040000F7 RID: 247
		private CmiManifestSignerFlag m_signerFlag;

		// Token: 0x040000F8 RID: 248
		private bool m_useSha256;

		// Token: 0x040000F9 RID: 249
		internal const uint CimManifestSignerFlagMask = 1U;
	}
}
