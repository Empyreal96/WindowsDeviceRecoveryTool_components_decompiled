using System;
using System.Security.Cryptography.X509Certificates;

namespace System.Deployment.Internal.CodeSigning
{
	// Token: 0x02000012 RID: 18
	internal class CmiAuthenticodeTimestamperInfo
	{
		// Token: 0x0600006B RID: 107 RVA: 0x000027DB File Offset: 0x000009DB
		private CmiAuthenticodeTimestamperInfo()
		{
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003E9C File Offset: 0x0000209C
		internal CmiAuthenticodeTimestamperInfo(Win32.AXL_TIMESTAMPER_INFO timestamperInfo)
		{
			this.m_error = (int)timestamperInfo.dwError;
			this.m_algHash = timestamperInfo.algHash;
			long fileTime = (long)((ulong)timestamperInfo.ftTimestamp.dwHighDateTime << 32 | (ulong)timestamperInfo.ftTimestamp.dwLowDateTime);
			this.m_timestampTime = DateTime.FromFileTime(fileTime);
			if (timestamperInfo.pChainContext != IntPtr.Zero)
			{
				this.m_timestamperChain = new X509Chain(timestamperInfo.pChainContext);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00003F13 File Offset: 0x00002113
		internal int ErrorCode
		{
			get
			{
				return this.m_error;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00003F1B File Offset: 0x0000211B
		internal uint HashAlgId
		{
			get
			{
				return this.m_algHash;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00003F23 File Offset: 0x00002123
		internal DateTime TimestampTime
		{
			get
			{
				return this.m_timestampTime;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00003F2B File Offset: 0x0000212B
		internal X509Chain TimestamperChain
		{
			get
			{
				return this.m_timestamperChain;
			}
		}

		// Token: 0x040000D7 RID: 215
		private int m_error;

		// Token: 0x040000D8 RID: 216
		private X509Chain m_timestamperChain;

		// Token: 0x040000D9 RID: 217
		private DateTime m_timestampTime;

		// Token: 0x040000DA RID: 218
		private uint m_algHash;
	}
}
