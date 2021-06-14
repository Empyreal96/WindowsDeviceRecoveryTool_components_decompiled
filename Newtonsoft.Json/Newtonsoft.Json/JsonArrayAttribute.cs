using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000045 RID: 69
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
	public sealed class JsonArrayAttribute : JsonContainerAttribute
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600026C RID: 620 RVA: 0x0000A10F File Offset: 0x0000830F
		// (set) Token: 0x0600026D RID: 621 RVA: 0x0000A117 File Offset: 0x00008317
		public bool AllowNullItems
		{
			get
			{
				return this._allowNullItems;
			}
			set
			{
				this._allowNullItems = value;
			}
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000A120 File Offset: 0x00008320
		public JsonArrayAttribute()
		{
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000A128 File Offset: 0x00008328
		public JsonArrayAttribute(bool allowNullItems)
		{
			this._allowNullItems = allowNullItems;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000A137 File Offset: 0x00008337
		public JsonArrayAttribute(string id) : base(id)
		{
		}

		// Token: 0x040000D9 RID: 217
		private bool _allowNullItems;
	}
}
