using System;

namespace Microsoft.WindowsPhone.Imaging
{
	// Token: 0x02000019 RID: 25
	public class ManifestWrapper : IPayloadWrapper
	{
		// Token: 0x0600014A RID: 330 RVA: 0x00006DB8 File Offset: 0x00004FB8
		public ManifestWrapper(FullFlashUpdateImage ffuImage, IPayloadWrapper innerWrapper)
		{
			this.ffuImage = ffuImage;
			this.innerWrapper = innerWrapper;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00006DD0 File Offset: 0x00004FD0
		public void InitializeWrapper(long payloadSize)
		{
			byte[] manifestRegion = this.ffuImage.GetManifestRegion();
			long payloadSize2 = payloadSize + (long)manifestRegion.Length;
			this.innerWrapper.InitializeWrapper(payloadSize2);
			this.innerWrapper.Write(manifestRegion);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00006E08 File Offset: 0x00005008
		public void ResetPosition()
		{
			this.innerWrapper.ResetPosition();
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00006E15 File Offset: 0x00005015
		public void Write(byte[] data)
		{
			this.innerWrapper.Write(data);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00006E23 File Offset: 0x00005023
		public void FinalizeWrapper()
		{
			this.innerWrapper.FinalizeWrapper();
		}

		// Token: 0x0400009A RID: 154
		private FullFlashUpdateImage ffuImage;

		// Token: 0x0400009B RID: 155
		private IPayloadWrapper innerWrapper;
	}
}
