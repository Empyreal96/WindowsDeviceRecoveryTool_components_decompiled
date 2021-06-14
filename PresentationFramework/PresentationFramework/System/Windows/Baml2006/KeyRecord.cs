using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xaml;

namespace System.Windows.Baml2006
{
	// Token: 0x0200015B RID: 347
	[DebuggerDisplay("{DebuggerString}")]
	internal class KeyRecord
	{
		// Token: 0x06000F87 RID: 3975 RVA: 0x0003C8B6 File Offset: 0x0003AAB6
		public KeyRecord(bool shared, bool sharedSet, int valuePosition, Type keyType) : this(shared, sharedSet, valuePosition)
		{
			this._data = keyType;
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x0003C8B6 File Offset: 0x0003AAB6
		public KeyRecord(bool shared, bool sharedSet, int valuePosition, string keyString) : this(shared, sharedSet, valuePosition)
		{
			this._data = keyString;
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x0003C8C9 File Offset: 0x0003AAC9
		public KeyRecord(bool shared, bool sharedSet, int valuePosition, XamlSchemaContext context) : this(shared, sharedSet, valuePosition)
		{
			this._data = new XamlNodeList(context, 8);
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x0003C8E2 File Offset: 0x0003AAE2
		private KeyRecord(bool shared, bool sharedSet, int valuePosition)
		{
			this._shared = shared;
			this._sharedSet = sharedSet;
			this.ValuePosition = (long)valuePosition;
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06000F8B RID: 3979 RVA: 0x0003C900 File Offset: 0x0003AB00
		public bool Shared
		{
			get
			{
				return this._shared;
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06000F8C RID: 3980 RVA: 0x0003C908 File Offset: 0x0003AB08
		public bool SharedSet
		{
			get
			{
				return this._sharedSet;
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06000F8D RID: 3981 RVA: 0x0003C910 File Offset: 0x0003AB10
		// (set) Token: 0x06000F8E RID: 3982 RVA: 0x0003C918 File Offset: 0x0003AB18
		public long ValuePosition { get; set; }

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06000F8F RID: 3983 RVA: 0x0003C921 File Offset: 0x0003AB21
		// (set) Token: 0x06000F90 RID: 3984 RVA: 0x0003C929 File Offset: 0x0003AB29
		public int ValueSize { get; set; }

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06000F91 RID: 3985 RVA: 0x0003C932 File Offset: 0x0003AB32
		// (set) Token: 0x06000F92 RID: 3986 RVA: 0x0003C93A File Offset: 0x0003AB3A
		public byte Flags { get; set; }

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06000F93 RID: 3987 RVA: 0x0003C943 File Offset: 0x0003AB43
		public List<object> StaticResources
		{
			get
			{
				if (this._resources == null)
				{
					this._resources = new List<object>();
				}
				return this._resources;
			}
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06000F94 RID: 3988 RVA: 0x0003C95E File Offset: 0x0003AB5E
		public bool HasStaticResources
		{
			get
			{
				return this._resources != null && this._resources.Count > 0;
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06000F95 RID: 3989 RVA: 0x0003C978 File Offset: 0x0003AB78
		public StaticResource LastStaticResource
		{
			get
			{
				return this.StaticResources[this.StaticResources.Count - 1] as StaticResource;
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06000F96 RID: 3990 RVA: 0x0003C997 File Offset: 0x0003AB97
		public string KeyString
		{
			get
			{
				return this._data as string;
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06000F97 RID: 3991 RVA: 0x0003C9A4 File Offset: 0x0003ABA4
		public Type KeyType
		{
			get
			{
				return this._data as Type;
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06000F98 RID: 3992 RVA: 0x0003C9B1 File Offset: 0x0003ABB1
		public XamlNodeList KeyNodeList
		{
			get
			{
				return this._data as XamlNodeList;
			}
		}

		// Token: 0x0400119F RID: 4511
		private List<object> _resources;

		// Token: 0x040011A0 RID: 4512
		private object _data;

		// Token: 0x040011A1 RID: 4513
		private bool _shared;

		// Token: 0x040011A2 RID: 4514
		private bool _sharedSet;
	}
}
