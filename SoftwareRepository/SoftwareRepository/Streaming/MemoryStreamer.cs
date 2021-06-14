using System;
using System.IO;

namespace SoftwareRepository.Streaming
{
	// Token: 0x02000019 RID: 25
	public class MemoryStreamer : Streamer
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x00003895 File Offset: 0x00001A95
		protected override Stream GetStreamInternal()
		{
			return new MemoryStream();
		}
	}
}
