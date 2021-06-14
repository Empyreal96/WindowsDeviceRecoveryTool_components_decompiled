using System;
using System.IO;

namespace Nokia.Mira.Primitives
{
	// Token: 0x02000019 RID: 25
	internal interface IStreamContainer
	{
		// Token: 0x06000052 RID: 82
		IStreamReservationContext ReserveStream(out Stream stream);
	}
}
