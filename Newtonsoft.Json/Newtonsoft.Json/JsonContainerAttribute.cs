using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000044 RID: 68
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
	public abstract class JsonContainerAttribute : Attribute
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000258 RID: 600 RVA: 0x00009FD1 File Offset: 0x000081D1
		// (set) Token: 0x06000259 RID: 601 RVA: 0x00009FD9 File Offset: 0x000081D9
		public string Id { get; set; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600025A RID: 602 RVA: 0x00009FE2 File Offset: 0x000081E2
		// (set) Token: 0x0600025B RID: 603 RVA: 0x00009FEA File Offset: 0x000081EA
		public string Title { get; set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600025C RID: 604 RVA: 0x00009FF3 File Offset: 0x000081F3
		// (set) Token: 0x0600025D RID: 605 RVA: 0x00009FFB File Offset: 0x000081FB
		public string Description { get; set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0000A004 File Offset: 0x00008204
		// (set) Token: 0x0600025F RID: 607 RVA: 0x0000A00C File Offset: 0x0000820C
		public Type ItemConverterType { get; set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000260 RID: 608 RVA: 0x0000A015 File Offset: 0x00008215
		// (set) Token: 0x06000261 RID: 609 RVA: 0x0000A01D File Offset: 0x0000821D
		public object[] ItemConverterParameters { get; set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000262 RID: 610 RVA: 0x0000A028 File Offset: 0x00008228
		// (set) Token: 0x06000263 RID: 611 RVA: 0x0000A04E File Offset: 0x0000824E
		public bool IsReference
		{
			get
			{
				return this._isReference ?? false;
			}
			set
			{
				this._isReference = new bool?(value);
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000264 RID: 612 RVA: 0x0000A05C File Offset: 0x0000825C
		// (set) Token: 0x06000265 RID: 613 RVA: 0x0000A082 File Offset: 0x00008282
		public bool ItemIsReference
		{
			get
			{
				return this._itemIsReference ?? false;
			}
			set
			{
				this._itemIsReference = new bool?(value);
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000266 RID: 614 RVA: 0x0000A090 File Offset: 0x00008290
		// (set) Token: 0x06000267 RID: 615 RVA: 0x0000A0B6 File Offset: 0x000082B6
		public ReferenceLoopHandling ItemReferenceLoopHandling
		{
			get
			{
				ReferenceLoopHandling? itemReferenceLoopHandling = this._itemReferenceLoopHandling;
				if (itemReferenceLoopHandling == null)
				{
					return ReferenceLoopHandling.Error;
				}
				return itemReferenceLoopHandling.GetValueOrDefault();
			}
			set
			{
				this._itemReferenceLoopHandling = new ReferenceLoopHandling?(value);
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000268 RID: 616 RVA: 0x0000A0C4 File Offset: 0x000082C4
		// (set) Token: 0x06000269 RID: 617 RVA: 0x0000A0EA File Offset: 0x000082EA
		public TypeNameHandling ItemTypeNameHandling
		{
			get
			{
				TypeNameHandling? itemTypeNameHandling = this._itemTypeNameHandling;
				if (itemTypeNameHandling == null)
				{
					return TypeNameHandling.None;
				}
				return itemTypeNameHandling.GetValueOrDefault();
			}
			set
			{
				this._itemTypeNameHandling = new TypeNameHandling?(value);
			}
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000A0F8 File Offset: 0x000082F8
		protected JsonContainerAttribute()
		{
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000A100 File Offset: 0x00008300
		protected JsonContainerAttribute(string id)
		{
			this.Id = id;
		}

		// Token: 0x040000D0 RID: 208
		internal bool? _isReference;

		// Token: 0x040000D1 RID: 209
		internal bool? _itemIsReference;

		// Token: 0x040000D2 RID: 210
		internal ReferenceLoopHandling? _itemReferenceLoopHandling;

		// Token: 0x040000D3 RID: 211
		internal TypeNameHandling? _itemTypeNameHandling;
	}
}
