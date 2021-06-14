using System;
using System.Collections;

namespace MS.Internal
{
	// Token: 0x020005EA RID: 1514
	internal interface IWeakHashtable
	{
		// Token: 0x17001841 RID: 6209
		object this[object key]
		{
			get;
		}

		// Token: 0x17001842 RID: 6210
		// (get) Token: 0x060064F7 RID: 25847
		ICollection Keys { get; }

		// Token: 0x17001843 RID: 6211
		// (get) Token: 0x060064F8 RID: 25848
		int Count { get; }

		// Token: 0x060064F9 RID: 25849
		bool ContainsKey(object key);

		// Token: 0x060064FA RID: 25850
		void Remove(object key);

		// Token: 0x060064FB RID: 25851
		void Clear();

		// Token: 0x060064FC RID: 25852
		void SetWeak(object key, object value);

		// Token: 0x060064FD RID: 25853
		object UnwrapKey(object key);
	}
}
