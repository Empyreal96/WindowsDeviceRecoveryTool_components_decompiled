using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000BC RID: 188
	public class JsonObjectContract : JsonContainerContract
	{
		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x060008F6 RID: 2294 RVA: 0x00021D69 File Offset: 0x0001FF69
		// (set) Token: 0x060008F7 RID: 2295 RVA: 0x00021D71 File Offset: 0x0001FF71
		public MemberSerialization MemberSerialization { get; set; }

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060008F8 RID: 2296 RVA: 0x00021D7A File Offset: 0x0001FF7A
		// (set) Token: 0x060008F9 RID: 2297 RVA: 0x00021D82 File Offset: 0x0001FF82
		public Required? ItemRequired { get; set; }

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060008FA RID: 2298 RVA: 0x00021D8B File Offset: 0x0001FF8B
		// (set) Token: 0x060008FB RID: 2299 RVA: 0x00021D93 File Offset: 0x0001FF93
		public JsonPropertyCollection Properties { get; private set; }

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x060008FC RID: 2300 RVA: 0x00021D9C File Offset: 0x0001FF9C
		[Obsolete("ConstructorParameters is obsolete. Use CreatorParameters instead.")]
		public JsonPropertyCollection ConstructorParameters
		{
			get
			{
				return this.CreatorParameters;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x060008FD RID: 2301 RVA: 0x00021DA4 File Offset: 0x0001FFA4
		// (set) Token: 0x060008FE RID: 2302 RVA: 0x00021DAC File Offset: 0x0001FFAC
		public JsonPropertyCollection CreatorParameters { get; private set; }

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x060008FF RID: 2303 RVA: 0x00021DB5 File Offset: 0x0001FFB5
		// (set) Token: 0x06000900 RID: 2304 RVA: 0x00021DBD File Offset: 0x0001FFBD
		[Obsolete("OverrideConstructor is obsolete. Use OverrideCreator instead.")]
		public ConstructorInfo OverrideConstructor
		{
			get
			{
				return this._overrideConstructor;
			}
			set
			{
				this._overrideConstructor = value;
				this._overrideCreator = ((value != null) ? JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor(value) : null);
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000901 RID: 2305 RVA: 0x00021DE3 File Offset: 0x0001FFE3
		// (set) Token: 0x06000902 RID: 2306 RVA: 0x00021DEB File Offset: 0x0001FFEB
		[Obsolete("ParametrizedConstructor is obsolete. Use OverrideCreator instead.")]
		public ConstructorInfo ParametrizedConstructor
		{
			get
			{
				return this._parametrizedConstructor;
			}
			set
			{
				this._parametrizedConstructor = value;
				this._parametrizedCreator = ((value != null) ? JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor(value) : null);
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x00021E11 File Offset: 0x00020011
		// (set) Token: 0x06000904 RID: 2308 RVA: 0x00021E19 File Offset: 0x00020019
		public ObjectConstructor<object> OverrideCreator
		{
			get
			{
				return this._overrideCreator;
			}
			set
			{
				this._overrideCreator = value;
				this._overrideConstructor = null;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000905 RID: 2309 RVA: 0x00021E29 File Offset: 0x00020029
		internal ObjectConstructor<object> ParametrizedCreator
		{
			get
			{
				return this._parametrizedCreator;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000906 RID: 2310 RVA: 0x00021E31 File Offset: 0x00020031
		// (set) Token: 0x06000907 RID: 2311 RVA: 0x00021E39 File Offset: 0x00020039
		public ExtensionDataSetter ExtensionDataSetter { get; set; }

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000908 RID: 2312 RVA: 0x00021E42 File Offset: 0x00020042
		// (set) Token: 0x06000909 RID: 2313 RVA: 0x00021E4A File Offset: 0x0002004A
		public ExtensionDataGetter ExtensionDataGetter { get; set; }

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x0600090A RID: 2314 RVA: 0x00021E54 File Offset: 0x00020054
		internal bool HasRequiredOrDefaultValueProperties
		{
			get
			{
				if (this._hasRequiredOrDefaultValueProperties == null)
				{
					this._hasRequiredOrDefaultValueProperties = new bool?(false);
					if (this.ItemRequired.GetValueOrDefault(Required.Default) != Required.Default)
					{
						this._hasRequiredOrDefaultValueProperties = new bool?(true);
					}
					else
					{
						foreach (JsonProperty jsonProperty in this.Properties)
						{
							if (jsonProperty.Required != Required.Default || ((jsonProperty.DefaultValueHandling & DefaultValueHandling.Populate) == DefaultValueHandling.Populate && jsonProperty.Writable))
							{
								this._hasRequiredOrDefaultValueProperties = new bool?(true);
								break;
							}
						}
					}
				}
				return this._hasRequiredOrDefaultValueProperties.Value;
			}
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x00021F44 File Offset: 0x00020144
		public JsonObjectContract(Type underlyingType) : base(underlyingType)
		{
			this.ContractType = JsonContractType.Object;
			this.Properties = new JsonPropertyCollection(base.UnderlyingType);
			this.CreatorParameters = new JsonPropertyCollection(base.UnderlyingType);
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x00021F76 File Offset: 0x00020176
		[SecuritySafeCritical]
		internal object GetUninitializedObject()
		{
			if (!JsonTypeReflector.FullyTrusted)
			{
				throw new JsonException("Insufficient permissions. Creating an uninitialized '{0}' type requires full trust.".FormatWith(CultureInfo.InvariantCulture, this.NonNullableUnderlyingType));
			}
			return FormatterServices.GetUninitializedObject(this.NonNullableUnderlyingType);
		}

		// Token: 0x0400031C RID: 796
		private bool? _hasRequiredOrDefaultValueProperties;

		// Token: 0x0400031D RID: 797
		private ConstructorInfo _parametrizedConstructor;

		// Token: 0x0400031E RID: 798
		private ConstructorInfo _overrideConstructor;

		// Token: 0x0400031F RID: 799
		private ObjectConstructor<object> _overrideCreator;

		// Token: 0x04000320 RID: 800
		private ObjectConstructor<object> _parametrizedCreator;
	}
}
