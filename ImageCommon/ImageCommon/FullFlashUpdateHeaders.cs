using System;
using System.Text;

namespace Microsoft.WindowsPhone.Imaging
{
	// Token: 0x02000003 RID: 3
	public static class FullFlashUpdateHeaders
	{
		// Token: 0x06000008 RID: 8 RVA: 0x0000216A File Offset: 0x0000036A
		public static byte[] GetSecuritySignature()
		{
			return Encoding.ASCII.GetBytes("SignedImage ");
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000217B File Offset: 0x0000037B
		public static byte[] GetImageSignature()
		{
			return Encoding.ASCII.GetBytes("ImageFlash  ");
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000A RID: 10 RVA: 0x0000218C File Offset: 0x0000038C
		[CLSCompliant(false)]
		public static uint SecurityHeaderSize
		{
			get
			{
				return (uint)(FullFlashUpdateImage.SecureHeaderSize + FullFlashUpdateHeaders.GetSecuritySignature().Length);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000B RID: 11 RVA: 0x0000219B File Offset: 0x0000039B
		[CLSCompliant(false)]
		public static uint ImageHeaderSize
		{
			get
			{
				return (uint)(FullFlashUpdateImage.ImageHeaderSize + FullFlashUpdateHeaders.GetImageSignature().Length);
			}
		}
	}
}
