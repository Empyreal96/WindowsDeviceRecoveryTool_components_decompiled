using System;

namespace Microsoft.WindowsAzure.Storage
{
	// Token: 0x02000075 RID: 117
	public interface IBufferManager
	{
		// Token: 0x06000E80 RID: 3712
		void ReturnBuffer(byte[] buffer);

		// Token: 0x06000E81 RID: 3713
		byte[] TakeBuffer(int bufferSize);

		// Token: 0x06000E82 RID: 3714
		int GetDefaultBufferSize();
	}
}
