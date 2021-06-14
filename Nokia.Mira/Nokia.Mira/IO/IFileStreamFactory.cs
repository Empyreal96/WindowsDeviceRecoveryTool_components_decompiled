using System;

namespace Nokia.Mira.IO
{
	// Token: 0x0200000E RID: 14
	internal interface IFileStreamFactory
	{
		// Token: 0x0600002F RID: 47
		IFileStream Create(long initialPosition);
	}
}
