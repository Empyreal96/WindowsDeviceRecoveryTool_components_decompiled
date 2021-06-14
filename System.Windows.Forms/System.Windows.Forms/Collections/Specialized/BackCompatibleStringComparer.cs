using System;
using System.Runtime.CompilerServices;

namespace System.Collections.Specialized
{
	// Token: 0x020000F3 RID: 243
	internal class BackCompatibleStringComparer : IEqualityComparer
	{
		// Token: 0x060003B8 RID: 952 RVA: 0x000027DB File Offset: 0x000009DB
		internal BackCompatibleStringComparer()
		{
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000BE00 File Offset: 0x0000A000
		public unsafe static int GetHashCode(string obj)
		{
			char* ptr = obj;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			int num = 5381;
			char* ptr2 = ptr;
			int num2;
			while ((num2 = (int)(*ptr2)) != 0)
			{
				num = ((num << 5) + num ^ num2);
				ptr2++;
			}
			return num;
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000BE3E File Offset: 0x0000A03E
		bool IEqualityComparer.Equals(object a, object b)
		{
			return object.Equals(a, b);
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000BE48 File Offset: 0x0000A048
		public virtual int GetHashCode(object o)
		{
			string text = o as string;
			if (text == null)
			{
				return o.GetHashCode();
			}
			return BackCompatibleStringComparer.GetHashCode(text);
		}

		// Token: 0x040003F7 RID: 1015
		internal static IEqualityComparer Default = new BackCompatibleStringComparer();
	}
}
