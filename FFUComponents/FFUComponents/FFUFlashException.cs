using System;
using System.Runtime.Serialization;

namespace FFUComponents
{
	// Token: 0x02000010 RID: 16
	[Serializable]
	public class FFUFlashException : FFUException
	{
		// Token: 0x06000050 RID: 80 RVA: 0x00003071 File Offset: 0x00001271
		public FFUFlashException()
		{
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003079 File Offset: 0x00001279
		public FFUFlashException(string message) : base(message)
		{
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003082 File Offset: 0x00001282
		public FFUFlashException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000308C File Offset: 0x0000128C
		public FFUFlashException(string deviceName, Guid deviceId, FFUFlashException.ErrorCode error, string message) : base(deviceName, deviceId, message)
		{
			this.Error = error;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000309F File Offset: 0x0000129F
		protected FFUFlashException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			info.AddValue("Error", this.Error);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000030BF File Offset: 0x000012BF
		protected new virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Error", this.Error);
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000056 RID: 86 RVA: 0x000030DF File Offset: 0x000012DF
		// (set) Token: 0x06000057 RID: 87 RVA: 0x000030E7 File Offset: 0x000012E7
		public FFUFlashException.ErrorCode Error { get; private set; }

		// Token: 0x02000011 RID: 17
		public enum ErrorCode
		{
			// Token: 0x0400001A RID: 26
			None,
			// Token: 0x0400001B RID: 27
			FlashError = 2,
			// Token: 0x0400001C RID: 28
			InvalidStoreHeader = 8,
			// Token: 0x0400001D RID: 29
			DescriptorAllocationFailed,
			// Token: 0x0400001E RID: 30
			DescriptorReadFailed = 11,
			// Token: 0x0400001F RID: 31
			BlockReadFailed,
			// Token: 0x04000020 RID: 32
			BlockWriteFailed,
			// Token: 0x04000021 RID: 33
			CrcError,
			// Token: 0x04000022 RID: 34
			SecureHeaderReadFailed,
			// Token: 0x04000023 RID: 35
			InvalidSecureHeader,
			// Token: 0x04000024 RID: 36
			InsufficientSecurityPadding,
			// Token: 0x04000025 RID: 37
			InvalidImageHeader,
			// Token: 0x04000026 RID: 38
			InsufficientImagePadding,
			// Token: 0x04000027 RID: 39
			BufferingFailed,
			// Token: 0x04000028 RID: 40
			ExcessBlocks,
			// Token: 0x04000029 RID: 41
			InvalidPlatformId,
			// Token: 0x0400002A RID: 42
			HashCheckFailed,
			// Token: 0x0400002B RID: 43
			SignatureCheckFailed,
			// Token: 0x0400002C RID: 44
			DesyncFailed = 26,
			// Token: 0x0400002D RID: 45
			FailedBcdQuery,
			// Token: 0x0400002E RID: 46
			InvalidWriteDescriptors,
			// Token: 0x0400002F RID: 47
			AntiTheftCheckFailed,
			// Token: 0x04000030 RID: 48
			RemoveableMediaCheckFailed = 32,
			// Token: 0x04000031 RID: 49
			UseOptimizedSettingsFailed
		}
	}
}
