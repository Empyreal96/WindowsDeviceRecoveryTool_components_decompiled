using System;

namespace Newtonsoft.Json
{
	// Token: 0x0200004E RID: 78
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false)]
	public sealed class JsonObjectAttribute : JsonContainerAttribute
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x0000AD57 File Offset: 0x00008F57
		// (set) Token: 0x060002D4 RID: 724 RVA: 0x0000AD5F File Offset: 0x00008F5F
		public MemberSerialization MemberSerialization
		{
			get
			{
				return this._memberSerialization;
			}
			set
			{
				this._memberSerialization = value;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x0000AD68 File Offset: 0x00008F68
		// (set) Token: 0x060002D6 RID: 726 RVA: 0x0000AD8E File Offset: 0x00008F8E
		public Required ItemRequired
		{
			get
			{
				Required? itemRequired = this._itemRequired;
				if (itemRequired == null)
				{
					return Required.Default;
				}
				return itemRequired.GetValueOrDefault();
			}
			set
			{
				this._itemRequired = new Required?(value);
			}
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000AD9C File Offset: 0x00008F9C
		public JsonObjectAttribute()
		{
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000ADA4 File Offset: 0x00008FA4
		public JsonObjectAttribute(MemberSerialization memberSerialization)
		{
			this.MemberSerialization = memberSerialization;
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000ADB3 File Offset: 0x00008FB3
		public JsonObjectAttribute(string id) : base(id)
		{
		}

		// Token: 0x040000E6 RID: 230
		private MemberSerialization _memberSerialization;

		// Token: 0x040000E7 RID: 231
		internal Required? _itemRequired;
	}
}
