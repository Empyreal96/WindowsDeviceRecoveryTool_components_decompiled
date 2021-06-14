using System;

namespace Nokia.Mira.IO
{
	// Token: 0x02000010 RID: 16
	internal interface IFileStream : IDisposable
	{
		// Token: 0x06000032 RID: 50
		void Flush();

		// Token: 0x06000033 RID: 51
		void Write(byte[] buffer, int offset, int count);
	}
}
