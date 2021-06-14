using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000051 RID: 81
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public sealed class JsonPropertyAttribute : Attribute
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000AFAC File Offset: 0x000091AC
		// (set) Token: 0x060002E1 RID: 737 RVA: 0x0000AFB4 File Offset: 0x000091B4
		public Type ItemConverterType { get; set; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x0000AFBD File Offset: 0x000091BD
		// (set) Token: 0x060002E3 RID: 739 RVA: 0x0000AFC5 File Offset: 0x000091C5
		public object[] ItemConverterParameters { get; set; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x0000AFD0 File Offset: 0x000091D0
		// (set) Token: 0x060002E5 RID: 741 RVA: 0x0000AFF6 File Offset: 0x000091F6
		public NullValueHandling NullValueHandling
		{
			get
			{
				NullValueHandling? nullValueHandling = this._nullValueHandling;
				if (nullValueHandling == null)
				{
					return NullValueHandling.Include;
				}
				return nullValueHandling.GetValueOrDefault();
			}
			set
			{
				this._nullValueHandling = new NullValueHandling?(value);
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x0000B004 File Offset: 0x00009204
		// (set) Token: 0x060002E7 RID: 743 RVA: 0x0000B02A File Offset: 0x0000922A
		public DefaultValueHandling DefaultValueHandling
		{
			get
			{
				DefaultValueHandling? defaultValueHandling = this._defaultValueHandling;
				if (defaultValueHandling == null)
				{
					return DefaultValueHandling.Include;
				}
				return defaultValueHandling.GetValueOrDefault();
			}
			set
			{
				this._defaultValueHandling = new DefaultValueHandling?(value);
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x0000B038 File Offset: 0x00009238
		// (set) Token: 0x060002E9 RID: 745 RVA: 0x0000B05E File Offset: 0x0000925E
		public ReferenceLoopHandling ReferenceLoopHandling
		{
			get
			{
				ReferenceLoopHandling? referenceLoopHandling = this._referenceLoopHandling;
				if (referenceLoopHandling == null)
				{
					return ReferenceLoopHandling.Error;
				}
				return referenceLoopHandling.GetValueOrDefault();
			}
			set
			{
				this._referenceLoopHandling = new ReferenceLoopHandling?(value);
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060002EA RID: 746 RVA: 0x0000B06C File Offset: 0x0000926C
		// (set) Token: 0x060002EB RID: 747 RVA: 0x0000B092 File Offset: 0x00009292
		public ObjectCreationHandling ObjectCreationHandling
		{
			get
			{
				ObjectCreationHandling? objectCreationHandling = this._objectCreationHandling;
				if (objectCreationHandling == null)
				{
					return ObjectCreationHandling.Auto;
				}
				return objectCreationHandling.GetValueOrDefault();
			}
			set
			{
				this._objectCreationHandling = new ObjectCreationHandling?(value);
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060002EC RID: 748 RVA: 0x0000B0A0 File Offset: 0x000092A0
		// (set) Token: 0x060002ED RID: 749 RVA: 0x0000B0C6 File Offset: 0x000092C6
		public TypeNameHandling TypeNameHandling
		{
			get
			{
				TypeNameHandling? typeNameHandling = this._typeNameHandling;
				if (typeNameHandling == null)
				{
					return TypeNameHandling.None;
				}
				return typeNameHandling.GetValueOrDefault();
			}
			set
			{
				this._typeNameHandling = new TypeNameHandling?(value);
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0000B0D4 File Offset: 0x000092D4
		// (set) Token: 0x060002EF RID: 751 RVA: 0x0000B0FA File Offset: 0x000092FA
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

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x0000B108 File Offset: 0x00009308
		// (set) Token: 0x060002F1 RID: 753 RVA: 0x0000B12E File Offset: 0x0000932E
		public int Order
		{
			get
			{
				int? order = this._order;
				if (order == null)
				{
					return 0;
				}
				return order.GetValueOrDefault();
			}
			set
			{
				this._order = new int?(value);
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x0000B13C File Offset: 0x0000933C
		// (set) Token: 0x060002F3 RID: 755 RVA: 0x0000B162 File Offset: 0x00009362
		public Required Required
		{
			get
			{
				Required? required = this._required;
				if (required == null)
				{
					return Required.Default;
				}
				return required.GetValueOrDefault();
			}
			set
			{
				this._required = new Required?(value);
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x0000B170 File Offset: 0x00009370
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x0000B178 File Offset: 0x00009378
		public string PropertyName { get; set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x0000B184 File Offset: 0x00009384
		// (set) Token: 0x060002F7 RID: 759 RVA: 0x0000B1AA File Offset: 0x000093AA
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

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x0000B1B8 File Offset: 0x000093B8
		// (set) Token: 0x060002F9 RID: 761 RVA: 0x0000B1DE File Offset: 0x000093DE
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

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060002FA RID: 762 RVA: 0x0000B1EC File Offset: 0x000093EC
		// (set) Token: 0x060002FB RID: 763 RVA: 0x0000B212 File Offset: 0x00009412
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

		// Token: 0x060002FC RID: 764 RVA: 0x0000B220 File Offset: 0x00009420
		public JsonPropertyAttribute()
		{
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000B228 File Offset: 0x00009428
		public JsonPropertyAttribute(string propertyName)
		{
			this.PropertyName = propertyName;
		}

		// Token: 0x040000F2 RID: 242
		internal NullValueHandling? _nullValueHandling;

		// Token: 0x040000F3 RID: 243
		internal DefaultValueHandling? _defaultValueHandling;

		// Token: 0x040000F4 RID: 244
		internal ReferenceLoopHandling? _referenceLoopHandling;

		// Token: 0x040000F5 RID: 245
		internal ObjectCreationHandling? _objectCreationHandling;

		// Token: 0x040000F6 RID: 246
		internal TypeNameHandling? _typeNameHandling;

		// Token: 0x040000F7 RID: 247
		internal bool? _isReference;

		// Token: 0x040000F8 RID: 248
		internal int? _order;

		// Token: 0x040000F9 RID: 249
		internal Required? _required;

		// Token: 0x040000FA RID: 250
		internal bool? _itemIsReference;

		// Token: 0x040000FB RID: 251
		internal ReferenceLoopHandling? _itemReferenceLoopHandling;

		// Token: 0x040000FC RID: 252
		internal TypeNameHandling? _itemTypeNameHandling;
	}
}
