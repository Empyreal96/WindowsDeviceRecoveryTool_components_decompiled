using System;
using System.Collections;

namespace MS.Internal.Controls
{
	// Token: 0x0200075D RID: 1885
	internal class EmptyEnumerator : IEnumerator
	{
		// Token: 0x060077FD RID: 30717 RVA: 0x0000326D File Offset: 0x0000146D
		private EmptyEnumerator()
		{
		}

		// Token: 0x17001C65 RID: 7269
		// (get) Token: 0x060077FE RID: 30718 RVA: 0x002237D4 File Offset: 0x002219D4
		public static IEnumerator Instance
		{
			get
			{
				if (EmptyEnumerator._instance == null)
				{
					EmptyEnumerator._instance = new EmptyEnumerator();
				}
				return EmptyEnumerator._instance;
			}
		}

		// Token: 0x060077FF RID: 30719 RVA: 0x00002137 File Offset: 0x00000337
		public void Reset()
		{
		}

		// Token: 0x06007800 RID: 30720 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool MoveNext()
		{
			return false;
		}

		// Token: 0x17001C66 RID: 7270
		// (get) Token: 0x06007801 RID: 30721 RVA: 0x00170A42 File Offset: 0x0016EC42
		public object Current
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x040038E2 RID: 14562
		private static IEnumerator _instance;
	}
}
