using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000BE RID: 190
	public class JsonProperty
	{
		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000910 RID: 2320 RVA: 0x00021FD9 File Offset: 0x000201D9
		// (set) Token: 0x06000911 RID: 2321 RVA: 0x00021FE1 File Offset: 0x000201E1
		internal JsonContract PropertyContract { get; set; }

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000912 RID: 2322 RVA: 0x00021FEA File Offset: 0x000201EA
		// (set) Token: 0x06000913 RID: 2323 RVA: 0x00021FF2 File Offset: 0x000201F2
		public string PropertyName
		{
			get
			{
				return this._propertyName;
			}
			set
			{
				this._propertyName = value;
				this._skipPropertyNameEscape = !JavaScriptUtils.ShouldEscapeJavaScriptString(this._propertyName, JavaScriptUtils.HtmlCharEscapeFlags);
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000914 RID: 2324 RVA: 0x00022014 File Offset: 0x00020214
		// (set) Token: 0x06000915 RID: 2325 RVA: 0x0002201C File Offset: 0x0002021C
		public Type DeclaringType { get; set; }

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000916 RID: 2326 RVA: 0x00022025 File Offset: 0x00020225
		// (set) Token: 0x06000917 RID: 2327 RVA: 0x0002202D File Offset: 0x0002022D
		public int? Order { get; set; }

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000918 RID: 2328 RVA: 0x00022036 File Offset: 0x00020236
		// (set) Token: 0x06000919 RID: 2329 RVA: 0x0002203E File Offset: 0x0002023E
		public string UnderlyingName { get; set; }

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x0600091A RID: 2330 RVA: 0x00022047 File Offset: 0x00020247
		// (set) Token: 0x0600091B RID: 2331 RVA: 0x0002204F File Offset: 0x0002024F
		public IValueProvider ValueProvider { get; set; }

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x0600091C RID: 2332 RVA: 0x00022058 File Offset: 0x00020258
		// (set) Token: 0x0600091D RID: 2333 RVA: 0x00022060 File Offset: 0x00020260
		public IAttributeProvider AttributeProvider { get; set; }

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x0600091E RID: 2334 RVA: 0x00022069 File Offset: 0x00020269
		// (set) Token: 0x0600091F RID: 2335 RVA: 0x00022071 File Offset: 0x00020271
		public Type PropertyType
		{
			get
			{
				return this._propertyType;
			}
			set
			{
				if (this._propertyType != value)
				{
					this._propertyType = value;
					this._hasGeneratedDefaultValue = false;
				}
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000920 RID: 2336 RVA: 0x0002208F File Offset: 0x0002028F
		// (set) Token: 0x06000921 RID: 2337 RVA: 0x00022097 File Offset: 0x00020297
		public JsonConverter Converter { get; set; }

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000922 RID: 2338 RVA: 0x000220A0 File Offset: 0x000202A0
		// (set) Token: 0x06000923 RID: 2339 RVA: 0x000220A8 File Offset: 0x000202A8
		public JsonConverter MemberConverter { get; set; }

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000924 RID: 2340 RVA: 0x000220B1 File Offset: 0x000202B1
		// (set) Token: 0x06000925 RID: 2341 RVA: 0x000220B9 File Offset: 0x000202B9
		public bool Ignored { get; set; }

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000926 RID: 2342 RVA: 0x000220C2 File Offset: 0x000202C2
		// (set) Token: 0x06000927 RID: 2343 RVA: 0x000220CA File Offset: 0x000202CA
		public bool Readable { get; set; }

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000928 RID: 2344 RVA: 0x000220D3 File Offset: 0x000202D3
		// (set) Token: 0x06000929 RID: 2345 RVA: 0x000220DB File Offset: 0x000202DB
		public bool Writable { get; set; }

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600092A RID: 2346 RVA: 0x000220E4 File Offset: 0x000202E4
		// (set) Token: 0x0600092B RID: 2347 RVA: 0x000220EC File Offset: 0x000202EC
		public bool HasMemberAttribute { get; set; }

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x0600092C RID: 2348 RVA: 0x000220F5 File Offset: 0x000202F5
		// (set) Token: 0x0600092D RID: 2349 RVA: 0x00022107 File Offset: 0x00020307
		public object DefaultValue
		{
			get
			{
				if (!this._hasExplicitDefaultValue)
				{
					return null;
				}
				return this._defaultValue;
			}
			set
			{
				this._hasExplicitDefaultValue = true;
				this._defaultValue = value;
			}
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x00022117 File Offset: 0x00020317
		internal object GetResolvedDefaultValue()
		{
			if (this._propertyType == null)
			{
				return null;
			}
			if (!this._hasExplicitDefaultValue && !this._hasGeneratedDefaultValue)
			{
				this._defaultValue = ReflectionUtils.GetDefaultValue(this.PropertyType);
				this._hasGeneratedDefaultValue = true;
			}
			return this._defaultValue;
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x0600092F RID: 2351 RVA: 0x00022158 File Offset: 0x00020358
		// (set) Token: 0x06000930 RID: 2352 RVA: 0x0002217E File Offset: 0x0002037E
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

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000931 RID: 2353 RVA: 0x0002218C File Offset: 0x0002038C
		// (set) Token: 0x06000932 RID: 2354 RVA: 0x00022194 File Offset: 0x00020394
		public bool? IsReference { get; set; }

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000933 RID: 2355 RVA: 0x0002219D File Offset: 0x0002039D
		// (set) Token: 0x06000934 RID: 2356 RVA: 0x000221A5 File Offset: 0x000203A5
		public NullValueHandling? NullValueHandling { get; set; }

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000935 RID: 2357 RVA: 0x000221AE File Offset: 0x000203AE
		// (set) Token: 0x06000936 RID: 2358 RVA: 0x000221B6 File Offset: 0x000203B6
		public DefaultValueHandling? DefaultValueHandling { get; set; }

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000937 RID: 2359 RVA: 0x000221BF File Offset: 0x000203BF
		// (set) Token: 0x06000938 RID: 2360 RVA: 0x000221C7 File Offset: 0x000203C7
		public ReferenceLoopHandling? ReferenceLoopHandling { get; set; }

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000939 RID: 2361 RVA: 0x000221D0 File Offset: 0x000203D0
		// (set) Token: 0x0600093A RID: 2362 RVA: 0x000221D8 File Offset: 0x000203D8
		public ObjectCreationHandling? ObjectCreationHandling { get; set; }

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x0600093B RID: 2363 RVA: 0x000221E1 File Offset: 0x000203E1
		// (set) Token: 0x0600093C RID: 2364 RVA: 0x000221E9 File Offset: 0x000203E9
		public TypeNameHandling? TypeNameHandling { get; set; }

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x0600093D RID: 2365 RVA: 0x000221F2 File Offset: 0x000203F2
		// (set) Token: 0x0600093E RID: 2366 RVA: 0x000221FA File Offset: 0x000203FA
		public Predicate<object> ShouldSerialize { get; set; }

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x0600093F RID: 2367 RVA: 0x00022203 File Offset: 0x00020403
		// (set) Token: 0x06000940 RID: 2368 RVA: 0x0002220B File Offset: 0x0002040B
		public Predicate<object> GetIsSpecified { get; set; }

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000941 RID: 2369 RVA: 0x00022214 File Offset: 0x00020414
		// (set) Token: 0x06000942 RID: 2370 RVA: 0x0002221C File Offset: 0x0002041C
		public Action<object, object> SetIsSpecified { get; set; }

		// Token: 0x06000943 RID: 2371 RVA: 0x00022225 File Offset: 0x00020425
		public override string ToString()
		{
			return this.PropertyName;
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000944 RID: 2372 RVA: 0x0002222D File Offset: 0x0002042D
		// (set) Token: 0x06000945 RID: 2373 RVA: 0x00022235 File Offset: 0x00020435
		public JsonConverter ItemConverter { get; set; }

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000946 RID: 2374 RVA: 0x0002223E File Offset: 0x0002043E
		// (set) Token: 0x06000947 RID: 2375 RVA: 0x00022246 File Offset: 0x00020446
		public bool? ItemIsReference { get; set; }

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000948 RID: 2376 RVA: 0x0002224F File Offset: 0x0002044F
		// (set) Token: 0x06000949 RID: 2377 RVA: 0x00022257 File Offset: 0x00020457
		public TypeNameHandling? ItemTypeNameHandling { get; set; }

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x0600094A RID: 2378 RVA: 0x00022260 File Offset: 0x00020460
		// (set) Token: 0x0600094B RID: 2379 RVA: 0x00022268 File Offset: 0x00020468
		public ReferenceLoopHandling? ItemReferenceLoopHandling { get; set; }

		// Token: 0x0600094C RID: 2380 RVA: 0x00022271 File Offset: 0x00020471
		internal void WritePropertyName(JsonWriter writer)
		{
			if (this._skipPropertyNameEscape)
			{
				writer.WritePropertyName(this.PropertyName, false);
				return;
			}
			writer.WritePropertyName(this.PropertyName);
		}

		// Token: 0x04000328 RID: 808
		internal Required? _required;

		// Token: 0x04000329 RID: 809
		internal bool _hasExplicitDefaultValue;

		// Token: 0x0400032A RID: 810
		private object _defaultValue;

		// Token: 0x0400032B RID: 811
		private bool _hasGeneratedDefaultValue;

		// Token: 0x0400032C RID: 812
		private string _propertyName;

		// Token: 0x0400032D RID: 813
		internal bool _skipPropertyNameEscape;

		// Token: 0x0400032E RID: 814
		private Type _propertyType;
	}
}
