using System;
using System.Collections;
using System.Globalization;

namespace System
{
	// Token: 0x02000007 RID: 7
	[Serializable]
	internal class InvariantComparer : IComparer
	{
		// Token: 0x06000007 RID: 7 RVA: 0x0000226E File Offset: 0x0000046E
		internal InvariantComparer()
		{
			this.m_compareInfo = CultureInfo.InvariantCulture.CompareInfo;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002288 File Offset: 0x00000488
		public int Compare(object a, object b)
		{
			string text = a as string;
			string text2 = b as string;
			if (text != null && text2 != null)
			{
				return this.m_compareInfo.Compare(text, text2);
			}
			return Comparer.Default.Compare(a, b);
		}

		// Token: 0x04000083 RID: 131
		private CompareInfo m_compareInfo;

		// Token: 0x04000084 RID: 132
		internal static readonly InvariantComparer Default = new InvariantComparer();
	}
}
