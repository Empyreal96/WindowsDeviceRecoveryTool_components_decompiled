using System;
using System.IO;

namespace ComponentAce.Compression.Libs.ZLib
{
	// Token: 0x020000C4 RID: 196
	public class ZStreamException : IOException
	{
		// Token: 0x06000879 RID: 2169 RVA: 0x00036501 File Offset: 0x00035501
		public ZStreamException()
		{
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x00036509 File Offset: 0x00035509
		public ZStreamException(string s) : base(s)
		{
		}
	}
}
