using System;

namespace Microsoft.WindowsPhone.Imaging
{
	// Token: 0x02000018 RID: 24
	public interface IPayloadWrapper
	{
		// Token: 0x06000146 RID: 326
		void InitializeWrapper(long payloadSize);

		// Token: 0x06000147 RID: 327
		void ResetPosition();

		// Token: 0x06000148 RID: 328
		void Write(byte[] data);

		// Token: 0x06000149 RID: 329
		void FinalizeWrapper();
	}
}
