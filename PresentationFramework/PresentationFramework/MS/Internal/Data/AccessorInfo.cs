using System;

namespace MS.Internal.Data
{
	// Token: 0x020006FF RID: 1791
	internal sealed class AccessorInfo
	{
		// Token: 0x06007343 RID: 29507 RVA: 0x00211002 File Offset: 0x0020F202
		internal AccessorInfo(object accessor, Type propertyType, object[] args)
		{
			this._accessor = accessor;
			this._propertyType = propertyType;
			this._args = args;
		}

		// Token: 0x17001B5A RID: 7002
		// (get) Token: 0x06007344 RID: 29508 RVA: 0x0021101F File Offset: 0x0020F21F
		internal object Accessor
		{
			get
			{
				return this._accessor;
			}
		}

		// Token: 0x17001B5B RID: 7003
		// (get) Token: 0x06007345 RID: 29509 RVA: 0x00211027 File Offset: 0x0020F227
		internal Type PropertyType
		{
			get
			{
				return this._propertyType;
			}
		}

		// Token: 0x17001B5C RID: 7004
		// (get) Token: 0x06007346 RID: 29510 RVA: 0x0021102F File Offset: 0x0020F22F
		internal object[] Args
		{
			get
			{
				return this._args;
			}
		}

		// Token: 0x17001B5D RID: 7005
		// (get) Token: 0x06007347 RID: 29511 RVA: 0x00211037 File Offset: 0x0020F237
		// (set) Token: 0x06007348 RID: 29512 RVA: 0x0021103F File Offset: 0x0020F23F
		internal int Generation
		{
			get
			{
				return this._generation;
			}
			set
			{
				this._generation = value;
			}
		}

		// Token: 0x0400378C RID: 14220
		private object _accessor;

		// Token: 0x0400378D RID: 14221
		private Type _propertyType;

		// Token: 0x0400378E RID: 14222
		private object[] _args;

		// Token: 0x0400378F RID: 14223
		private int _generation;
	}
}
