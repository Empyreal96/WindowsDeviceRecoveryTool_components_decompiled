using System;
using System.IO;
using System.Text;

namespace System.Windows.Markup
{
	// Token: 0x020001C8 RID: 456
	internal class BamlBinaryReader : BinaryReader
	{
		// Token: 0x06001D25 RID: 7461 RVA: 0x00087F97 File Offset: 0x00086197
		public BamlBinaryReader(Stream stream, Encoding code) : base(stream, code)
		{
		}

		// Token: 0x06001D26 RID: 7462 RVA: 0x0003CA6D File Offset: 0x0003AC6D
		public new int Read7BitEncodedInt()
		{
			return base.Read7BitEncodedInt();
		}
	}
}
