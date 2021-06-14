using System;
using System.Collections;

namespace System.Data.Services.Client
{
	// Token: 0x020000B1 RID: 177
	internal class ReferenceEqualityComparer : IEqualityComparer
	{
		// Token: 0x060005AA RID: 1450 RVA: 0x00015995 File Offset: 0x00013B95
		protected ReferenceEqualityComparer()
		{
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0001599D File Offset: 0x00013B9D
		bool IEqualityComparer.Equals(object x, object y)
		{
			return object.ReferenceEquals(x, y);
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x000159A6 File Offset: 0x00013BA6
		int IEqualityComparer.GetHashCode(object obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.GetHashCode();
		}
	}
}
