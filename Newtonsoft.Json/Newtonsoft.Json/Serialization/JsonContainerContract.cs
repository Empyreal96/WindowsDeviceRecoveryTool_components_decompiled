using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000B0 RID: 176
	public class JsonContainerContract : JsonContract
	{
		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x00020F7A File Offset: 0x0001F17A
		// (set) Token: 0x0600089D RID: 2205 RVA: 0x00020F82 File Offset: 0x0001F182
		internal JsonContract ItemContract
		{
			get
			{
				return this._itemContract;
			}
			set
			{
				this._itemContract = value;
				if (this._itemContract != null)
				{
					this._finalItemContract = (this._itemContract.UnderlyingType.IsSealed() ? this._itemContract : null);
					return;
				}
				this._finalItemContract = null;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x0600089E RID: 2206 RVA: 0x00020FBC File Offset: 0x0001F1BC
		internal JsonContract FinalItemContract
		{
			get
			{
				return this._finalItemContract;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x0600089F RID: 2207 RVA: 0x00020FC4 File Offset: 0x0001F1C4
		// (set) Token: 0x060008A0 RID: 2208 RVA: 0x00020FCC File Offset: 0x0001F1CC
		public JsonConverter ItemConverter { get; set; }

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060008A1 RID: 2209 RVA: 0x00020FD5 File Offset: 0x0001F1D5
		// (set) Token: 0x060008A2 RID: 2210 RVA: 0x00020FDD File Offset: 0x0001F1DD
		public bool? ItemIsReference { get; set; }

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060008A3 RID: 2211 RVA: 0x00020FE6 File Offset: 0x0001F1E6
		// (set) Token: 0x060008A4 RID: 2212 RVA: 0x00020FEE File Offset: 0x0001F1EE
		public ReferenceLoopHandling? ItemReferenceLoopHandling { get; set; }

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x060008A5 RID: 2213 RVA: 0x00020FF7 File Offset: 0x0001F1F7
		// (set) Token: 0x060008A6 RID: 2214 RVA: 0x00020FFF File Offset: 0x0001F1FF
		public TypeNameHandling? ItemTypeNameHandling { get; set; }

		// Token: 0x060008A7 RID: 2215 RVA: 0x00021008 File Offset: 0x0001F208
		internal JsonContainerContract(Type underlyingType) : base(underlyingType)
		{
			JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(underlyingType);
			if (cachedAttribute != null)
			{
				if (cachedAttribute.ItemConverterType != null)
				{
					this.ItemConverter = JsonTypeReflector.CreateJsonConverterInstance(cachedAttribute.ItemConverterType, cachedAttribute.ItemConverterParameters);
				}
				this.ItemIsReference = cachedAttribute._itemIsReference;
				this.ItemReferenceLoopHandling = cachedAttribute._itemReferenceLoopHandling;
				this.ItemTypeNameHandling = cachedAttribute._itemTypeNameHandling;
			}
		}

		// Token: 0x040002EE RID: 750
		private JsonContract _itemContract;

		// Token: 0x040002EF RID: 751
		private JsonContract _finalItemContract;
	}
}
