using System;
using System.IO;

namespace System.Windows.Baml2006
{
	// Token: 0x0200015E RID: 350
	internal class BamlBinaryReader : BinaryReader
	{
		// Token: 0x06000FA4 RID: 4004 RVA: 0x0003CA64 File Offset: 0x0003AC64
		public BamlBinaryReader(Stream stream) : base(stream)
		{
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x0003CA6D File Offset: 0x0003AC6D
		public new int Read7BitEncodedInt()
		{
			return base.Read7BitEncodedInt();
		}
	}
}
