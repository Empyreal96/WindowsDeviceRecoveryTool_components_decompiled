using System;
using System.Collections;

namespace MS.Internal.Controls
{
	// Token: 0x0200075C RID: 1884
	internal class EmptyEnumerable : IEnumerable
	{
		// Token: 0x060077FA RID: 30714 RVA: 0x0000326D File Offset: 0x0000146D
		private EmptyEnumerable()
		{
		}

		// Token: 0x060077FB RID: 30715 RVA: 0x00222AA5 File Offset: 0x00220CA5
		IEnumerator IEnumerable.GetEnumerator()
		{
			return EmptyEnumerator.Instance;
		}

		// Token: 0x17001C64 RID: 7268
		// (get) Token: 0x060077FC RID: 30716 RVA: 0x002237BC File Offset: 0x002219BC
		public static IEnumerable Instance
		{
			get
			{
				if (EmptyEnumerable._instance == null)
				{
					EmptyEnumerable._instance = new EmptyEnumerable();
				}
				return EmptyEnumerable._instance;
			}
		}

		// Token: 0x040038E1 RID: 14561
		private static IEnumerable _instance;
	}
}
