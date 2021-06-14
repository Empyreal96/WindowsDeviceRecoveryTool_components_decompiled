using System;
using System.IO;
using System.Text;

namespace System.Windows.Markup
{
	// Token: 0x020001C9 RID: 457
	internal class BamlBinaryWriter : BinaryWriter
	{
		// Token: 0x06001D27 RID: 7463 RVA: 0x00087FA1 File Offset: 0x000861A1
		public BamlBinaryWriter(Stream stream, Encoding code) : base(stream, code)
		{
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x00087FAB File Offset: 0x000861AB
		public new void Write7BitEncodedInt(int value)
		{
			base.Write7BitEncodedInt(value);
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x00087FB4 File Offset: 0x000861B4
		public static int SizeOf7bitEncodedSize(int size)
		{
			if ((size & -128) == 0)
			{
				return 1;
			}
			if ((size & -16384) == 0)
			{
				return 2;
			}
			if ((size & -2097152) == 0)
			{
				return 3;
			}
			if ((size & -268435456) == 0)
			{
				return 4;
			}
			return 5;
		}
	}
}
