using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Microsoft.WindowsPhone.Imaging
{
	// Token: 0x0200002A RID: 42
	public class SecurityWrapper : IPayloadWrapper
	{
		// Token: 0x060001DB RID: 475 RVA: 0x0000B5C7 File Offset: 0x000097C7
		public SecurityWrapper(FullFlashUpdateImage ffuImage, IPayloadWrapper innerWrapper)
		{
			this.ffuImage = ffuImage;
			this.innerWrapper = innerWrapper;
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001DC RID: 476 RVA: 0x0000B5DD File Offset: 0x000097DD
		// (set) Token: 0x060001DD RID: 477 RVA: 0x0000B5E5 File Offset: 0x000097E5
		public byte[] CatalogData { get; private set; }

		// Token: 0x060001DE RID: 478 RVA: 0x0000B5F0 File Offset: 0x000097F0
		public void InitializeWrapper(long payloadSize)
		{
			if (payloadSize % (long)((ulong)this.ffuImage.ChunkSizeInBytes) != 0L)
			{
				throw new ImageCommonException("Data size not aligned with hash chunk size.");
			}
			this.sha = new SHA256CryptoServiceProvider();
			this.sha.Initialize();
			this.bytesHashed = 0;
			this.hashOffset = 0;
			uint num = (uint)(payloadSize / (long)((ulong)this.ffuImage.ChunkSizeInBytes));
			uint num2 = num * (uint)(this.sha.HashSize / 8);
			this.hashData = new byte[num2];
			this.CatalogData = ImageSigner.GenerateCatalogFile(this.hashData);
			byte[] securityHeader = this.ffuImage.GetSecurityHeader(this.CatalogData, this.hashData);
			this.innerWrapper.InitializeWrapper(payloadSize + (long)securityHeader.Length);
			this.innerWrapper.Write(securityHeader);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000B6B1 File Offset: 0x000098B1
		public void ResetPosition()
		{
			this.innerWrapper.ResetPosition();
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000B6BE File Offset: 0x000098BE
		public void Write(byte[] data)
		{
			this.HashBufferAsync(data);
			this.innerWrapper.Write(data);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000B6D4 File Offset: 0x000098D4
		public void FinalizeWrapper()
		{
			this.hashTask.Wait();
			this.hashTask = null;
			if (this.hashOffset != this.hashData.Length)
			{
				string message = string.Format("Failed to hash all data in the stream. hashOffset = {0}, hashData.Length = {1}, bytesHashed = {2}.", this.hashOffset, this.hashData.Length, this.bytesHashed);
				throw new ImageCommonException(message);
			}
			this.CatalogData = ImageSigner.GenerateCatalogFile(this.hashData);
			byte[] securityHeader = this.ffuImage.GetSecurityHeader(this.CatalogData, this.hashData);
			this.innerWrapper.ResetPosition();
			this.innerWrapper.Write(securityHeader);
			this.ffuImage.CatalogData = this.CatalogData;
			this.ffuImage.HashTableData = this.hashData;
			this.innerWrapper.FinalizeWrapper();
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000B7C0 File Offset: 0x000099C0
		private void HashBufferAsync(byte[] data)
		{
			if (this.hashTask != null)
			{
				this.hashTask.Wait();
			}
			this.hashTask = Task.Factory.StartNew(delegate()
			{
				this.HashBuffer(data);
			});
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000B810 File Offset: 0x00009A10
		private void HashBuffer(byte[] data)
		{
			int chunkSizeInBytes = (int)this.ffuImage.ChunkSizeInBytes;
			int num = chunkSizeInBytes - this.bytesHashed;
			for (int i = 0; i < data.Length; i += chunkSizeInBytes)
			{
				int num2 = num;
				if (data.Length - i < num)
				{
					num2 = data.Length;
				}
				byte[] hash = this.sha.ComputeHash(data, i, num2);
				this.bytesHashed += num2;
				this.bytesHashed %= chunkSizeInBytes;
				if (this.bytesHashed == 0)
				{
					this.CommitHashToTable(hash);
				}
				num = chunkSizeInBytes;
			}
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000B88C File Offset: 0x00009A8C
		private void CommitHashToTable(byte[] hash)
		{
			hash.CopyTo(this.hashData, this.hashOffset);
			this.hashOffset += hash.Length;
		}

		// Token: 0x04000133 RID: 307
		private IPayloadWrapper innerWrapper;

		// Token: 0x04000134 RID: 308
		private FullFlashUpdateImage ffuImage;

		// Token: 0x04000135 RID: 309
		private Task hashTask;

		// Token: 0x04000136 RID: 310
		private SHA256 sha;

		// Token: 0x04000137 RID: 311
		private byte[] hashData;

		// Token: 0x04000138 RID: 312
		private int bytesHashed;

		// Token: 0x04000139 RID: 313
		private int hashOffset;
	}
}
