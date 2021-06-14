using System;
using System.IO;

namespace ComponentAce.Compression.ZipForge.Encryption
{
	// Token: 0x0200008B RID: 139
	internal abstract class BaseZipForgeCryptoTransform
	{
		// Token: 0x06000679 RID: 1657
		public abstract void GenerateKey(string password);

		// Token: 0x0600067A RID: 1658
		public abstract bool CheckPassword(string password, DirItem item);

		// Token: 0x0600067B RID: 1659
		public abstract byte[] GetKey();

		// Token: 0x0600067C RID: 1660
		public abstract string GetPassword();

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600067D RID: 1661 RVA: 0x00029FAC File Offset: 0x00028FAC
		public byte[] Key
		{
			get
			{
				return this.GetKey();
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600067E RID: 1662 RVA: 0x00029FB4 File Offset: 0x00028FB4
		public string Password
		{
			get
			{
				return this.GetPassword();
			}
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00029FBC File Offset: 0x00028FBC
		public virtual void Initialize(CryptoTransformMode mode, DirItem item)
		{
			this.transformMode = mode;
		}

		// Token: 0x06000680 RID: 1664
		public abstract void Reset();

		// Token: 0x06000681 RID: 1665
		public abstract int EncryptBuffer(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset);

		// Token: 0x06000682 RID: 1666
		public abstract int DecryptBuffer(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset);

		// Token: 0x06000683 RID: 1667
		public abstract byte[] GetFileStorageStartBlock();

		// Token: 0x06000684 RID: 1668
		public abstract void LoadFileStorageStartBlock(Stream stream, long offset);

		// Token: 0x06000685 RID: 1669
		public abstract int GetFileStorageStartBlockSize();

		// Token: 0x06000686 RID: 1670
		public abstract byte[] GetFileStorageEndBlock();

		// Token: 0x06000687 RID: 1671
		public abstract void LoadFileStorageEndBlock(Stream stream, long offset);

		// Token: 0x06000688 RID: 1672
		public abstract int GetFileStorageEndBlockSize();

		// Token: 0x06000689 RID: 1673
		public abstract bool IsDirItemCorrupted(DirItem item, uint crc32);

		// Token: 0x0600068A RID: 1674
		public abstract ExtraFieldData GetExtraFieldData();

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x00029FC5 File Offset: 0x00028FC5
		public CryptoTransformMode TransformMode
		{
			get
			{
				return this.transformMode;
			}
		}

		// Token: 0x04000370 RID: 880
		private CryptoTransformMode transformMode;
	}
}
