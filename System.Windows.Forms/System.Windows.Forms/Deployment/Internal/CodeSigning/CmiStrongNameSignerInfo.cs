using System;
using System.Security.Cryptography;

namespace System.Deployment.Internal.CodeSigning
{
	// Token: 0x02000010 RID: 16
	internal class CmiStrongNameSignerInfo
	{
		// Token: 0x06000053 RID: 83 RVA: 0x000027DB File Offset: 0x000009DB
		internal CmiStrongNameSignerInfo()
		{
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003D03 File Offset: 0x00001F03
		internal CmiStrongNameSignerInfo(int errorCode, string publicKeyToken)
		{
			this.m_error = errorCode;
			this.m_publicKeyToken = publicKeyToken;
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00003D19 File Offset: 0x00001F19
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00003D21 File Offset: 0x00001F21
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

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00003D2A File Offset: 0x00001F2A
		// (set) Token: 0x06000058 RID: 88 RVA: 0x00003D32 File Offset: 0x00001F32
		internal string PublicKeyToken
		{
			get
			{
				return this.m_publicKeyToken;
			}
			set
			{
				this.m_publicKeyToken = value;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00003D3B File Offset: 0x00001F3B
		// (set) Token: 0x0600005A RID: 90 RVA: 0x00003D43 File Offset: 0x00001F43
		internal AsymmetricAlgorithm PublicKey
		{
			get
			{
				return this.m_snKey;
			}
			set
			{
				this.m_snKey = value;
			}
		}

		// Token: 0x040000CD RID: 205
		private int m_error;

		// Token: 0x040000CE RID: 206
		private string m_publicKeyToken;

		// Token: 0x040000CF RID: 207
		private AsymmetricAlgorithm m_snKey;
	}
}
