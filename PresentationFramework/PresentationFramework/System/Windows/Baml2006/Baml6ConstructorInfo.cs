using System;
using System.Collections.Generic;

namespace System.Windows.Baml2006
{
	// Token: 0x02000168 RID: 360
	internal struct Baml6ConstructorInfo
	{
		// Token: 0x06001077 RID: 4215 RVA: 0x000419A0 File Offset: 0x0003FBA0
		public Baml6ConstructorInfo(List<Type> types, Func<object[], object> ctor)
		{
			this._types = types;
			this._constructor = ctor;
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06001078 RID: 4216 RVA: 0x000419B0 File Offset: 0x0003FBB0
		public List<Type> Types
		{
			get
			{
				return this._types;
			}
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06001079 RID: 4217 RVA: 0x000419B8 File Offset: 0x0003FBB8
		public Func<object[], object> Constructor
		{
			get
			{
				return this._constructor;
			}
		}

		// Token: 0x0400122B RID: 4651
		private List<Type> _types;

		// Token: 0x0400122C RID: 4652
		private Func<object[], object> _constructor;
	}
}
