using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace System.Deployment.Internal.CodeSigning
{
	// Token: 0x02000011 RID: 17
	internal class CmiAuthenticodeSignerInfo
	{
		// Token: 0x0600005B RID: 91 RVA: 0x000027DB File Offset: 0x000009DB
		internal CmiAuthenticodeSignerInfo()
		{
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003D4C File Offset: 0x00001F4C
		internal CmiAuthenticodeSignerInfo(int errorCode)
		{
			this.m_error = errorCode;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003D5C File Offset: 0x00001F5C
		internal CmiAuthenticodeSignerInfo(Win32.AXL_SIGNER_INFO signerInfo, Win32.AXL_TIMESTAMPER_INFO timestamperInfo)
		{
			this.m_error = (int)signerInfo.dwError;
			if (signerInfo.pChainContext != IntPtr.Zero)
			{
				this.m_signerChain = new X509Chain(signerInfo.pChainContext);
			}
			this.m_algHash = signerInfo.algHash;
			if (signerInfo.pwszHash != IntPtr.Zero)
			{
				this.m_hash = Marshal.PtrToStringUni(signerInfo.pwszHash);
			}
			if (signerInfo.pwszDescription != IntPtr.Zero)
			{
				this.m_description = Marshal.PtrToStringUni(signerInfo.pwszDescription);
			}
			if (signerInfo.pwszDescriptionUrl != IntPtr.Zero)
			{
				this.m_descriptionUrl = Marshal.PtrToStringUni(signerInfo.pwszDescriptionUrl);
			}
			if (timestamperInfo.dwError != 2148204800U)
			{
				this.m_timestamperInfo = new CmiAuthenticodeTimestamperInfo(timestamperInfo);
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00003E2C File Offset: 0x0000202C
		// (set) Token: 0x0600005F RID: 95 RVA: 0x00003E34 File Offset: 0x00002034
		internal int ErrorCode
		{
			get
			{
				return this.m_error;
			}
			set
			{
				this.m_error = value;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00003E3D File Offset: 0x0000203D
		// (set) Token: 0x06000061 RID: 97 RVA: 0x00003E45 File Offset: 0x00002045
		internal uint HashAlgId
		{
			get
			{
				return this.m_algHash;
			}
			set
			{
				this.m_algHash = value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00003E4E File Offset: 0x0000204E
		// (set) Token: 0x06000063 RID: 99 RVA: 0x00003E56 File Offset: 0x00002056
		internal string Hash
		{
			get
			{
				return this.m_hash;
			}
			set
			{
				this.m_hash = value;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00003E5F File Offset: 0x0000205F
		// (set) Token: 0x06000065 RID: 101 RVA: 0x00003E67 File Offset: 0x00002067
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

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00003E70 File Offset: 0x00002070
		// (set) Token: 0x06000067 RID: 103 RVA: 0x00003E78 File Offset: 0x00002078
		internal string DescriptionUrl
		{
			get
			{
				return this.m_descriptionUrl;
			}
			set
			{
				this.m_descriptionUrl = value;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00003E81 File Offset: 0x00002081
		internal CmiAuthenticodeTimestamperInfo TimestamperInfo
		{
			get
			{
				return this.m_timestamperInfo;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00003E89 File Offset: 0x00002089
		// (set) Token: 0x0600006A RID: 106 RVA: 0x00003E91 File Offset: 0x00002091
		internal X509Chain SignerChain
		{
			get
			{
				return this.m_signerChain;
			}
			set
			{
				this.m_signerChain = value;
			}
		}

		// Token: 0x040000D0 RID: 208
		private int m_error;

		// Token: 0x040000D1 RID: 209
		private X509Chain m_signerChain;

		// Token: 0x040000D2 RID: 210
		private uint m_algHash;

		// Token: 0x040000D3 RID: 211
		private string m_hash;

		// Token: 0x040000D4 RID: 212
		private string m_description;

		// Token: 0x040000D5 RID: 213
		private string m_descriptionUrl;

		// Token: 0x040000D6 RID: 214
		private CmiAuthenticodeTimestamperInfo m_timestamperInfo;
	}
}
