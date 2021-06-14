using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000C5 RID: 197
	internal class JsonSerializerProxy : JsonSerializer
	{
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060009B6 RID: 2486 RVA: 0x000274BD File Offset: 0x000256BD
		// (remove) Token: 0x060009B7 RID: 2487 RVA: 0x000274CB File Offset: 0x000256CB
		public override event EventHandler<ErrorEventArgs> Error
		{
			add
			{
				this._serializer.Error += value;
			}
			remove
			{
				this._serializer.Error -= value;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x060009B8 RID: 2488 RVA: 0x000274D9 File Offset: 0x000256D9
		// (set) Token: 0x060009B9 RID: 2489 RVA: 0x000274E6 File Offset: 0x000256E6
		public override IReferenceResolver ReferenceResolver
		{
			get
			{
				return this._serializer.ReferenceResolver;
			}
			set
			{
				this._serializer.ReferenceResolver = value;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x060009BA RID: 2490 RVA: 0x000274F4 File Offset: 0x000256F4
		// (set) Token: 0x060009BB RID: 2491 RVA: 0x00027501 File Offset: 0x00025701
		public override ITraceWriter TraceWriter
		{
			get
			{
				return this._serializer.TraceWriter;
			}
			set
			{
				this._serializer.TraceWriter = value;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x060009BC RID: 2492 RVA: 0x0002750F File Offset: 0x0002570F
		public override JsonConverterCollection Converters
		{
			get
			{
				return this._serializer.Converters;
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x060009BD RID: 2493 RVA: 0x0002751C File Offset: 0x0002571C
		// (set) Token: 0x060009BE RID: 2494 RVA: 0x00027529 File Offset: 0x00025729
		public override DefaultValueHandling DefaultValueHandling
		{
			get
			{
				return this._serializer.DefaultValueHandling;
			}
			set
			{
				this._serializer.DefaultValueHandling = value;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x060009BF RID: 2495 RVA: 0x00027537 File Offset: 0x00025737
		// (set) Token: 0x060009C0 RID: 2496 RVA: 0x00027544 File Offset: 0x00025744
		public override IContractResolver ContractResolver
		{
			get
			{
				return this._serializer.ContractResolver;
			}
			set
			{
				this._serializer.ContractResolver = value;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x060009C1 RID: 2497 RVA: 0x00027552 File Offset: 0x00025752
		// (set) Token: 0x060009C2 RID: 2498 RVA: 0x0002755F File Offset: 0x0002575F
		public override MissingMemberHandling MissingMemberHandling
		{
			get
			{
				return this._serializer.MissingMemberHandling;
			}
			set
			{
				this._serializer.MissingMemberHandling = value;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x060009C3 RID: 2499 RVA: 0x0002756D File Offset: 0x0002576D
		// (set) Token: 0x060009C4 RID: 2500 RVA: 0x0002757A File Offset: 0x0002577A
		public override NullValueHandling NullValueHandling
		{
			get
			{
				return this._serializer.NullValueHandling;
			}
			set
			{
				this._serializer.NullValueHandling = value;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x060009C5 RID: 2501 RVA: 0x00027588 File Offset: 0x00025788
		// (set) Token: 0x060009C6 RID: 2502 RVA: 0x00027595 File Offset: 0x00025795
		public override ObjectCreationHandling ObjectCreationHandling
		{
			get
			{
				return this._serializer.ObjectCreationHandling;
			}
			set
			{
				this._serializer.ObjectCreationHandling = value;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060009C7 RID: 2503 RVA: 0x000275A3 File Offset: 0x000257A3
		// (set) Token: 0x060009C8 RID: 2504 RVA: 0x000275B0 File Offset: 0x000257B0
		public override ReferenceLoopHandling ReferenceLoopHandling
		{
			get
			{
				return this._serializer.ReferenceLoopHandling;
			}
			set
			{
				this._serializer.ReferenceLoopHandling = value;
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060009C9 RID: 2505 RVA: 0x000275BE File Offset: 0x000257BE
		// (set) Token: 0x060009CA RID: 2506 RVA: 0x000275CB File Offset: 0x000257CB
		public override PreserveReferencesHandling PreserveReferencesHandling
		{
			get
			{
				return this._serializer.PreserveReferencesHandling;
			}
			set
			{
				this._serializer.PreserveReferencesHandling = value;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060009CB RID: 2507 RVA: 0x000275D9 File Offset: 0x000257D9
		// (set) Token: 0x060009CC RID: 2508 RVA: 0x000275E6 File Offset: 0x000257E6
		public override TypeNameHandling TypeNameHandling
		{
			get
			{
				return this._serializer.TypeNameHandling;
			}
			set
			{
				this._serializer.TypeNameHandling = value;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x060009CD RID: 2509 RVA: 0x000275F4 File Offset: 0x000257F4
		// (set) Token: 0x060009CE RID: 2510 RVA: 0x00027601 File Offset: 0x00025801
		public override MetadataPropertyHandling MetadataPropertyHandling
		{
			get
			{
				return this._serializer.MetadataPropertyHandling;
			}
			set
			{
				this._serializer.MetadataPropertyHandling = value;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060009CF RID: 2511 RVA: 0x0002760F File Offset: 0x0002580F
		// (set) Token: 0x060009D0 RID: 2512 RVA: 0x0002761C File Offset: 0x0002581C
		public override FormatterAssemblyStyle TypeNameAssemblyFormat
		{
			get
			{
				return this._serializer.TypeNameAssemblyFormat;
			}
			set
			{
				this._serializer.TypeNameAssemblyFormat = value;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x060009D1 RID: 2513 RVA: 0x0002762A File Offset: 0x0002582A
		// (set) Token: 0x060009D2 RID: 2514 RVA: 0x00027637 File Offset: 0x00025837
		public override ConstructorHandling ConstructorHandling
		{
			get
			{
				return this._serializer.ConstructorHandling;
			}
			set
			{
				this._serializer.ConstructorHandling = value;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x060009D3 RID: 2515 RVA: 0x00027645 File Offset: 0x00025845
		// (set) Token: 0x060009D4 RID: 2516 RVA: 0x00027652 File Offset: 0x00025852
		public override SerializationBinder Binder
		{
			get
			{
				return this._serializer.Binder;
			}
			set
			{
				this._serializer.Binder = value;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x060009D5 RID: 2517 RVA: 0x00027660 File Offset: 0x00025860
		// (set) Token: 0x060009D6 RID: 2518 RVA: 0x0002766D File Offset: 0x0002586D
		public override StreamingContext Context
		{
			get
			{
				return this._serializer.Context;
			}
			set
			{
				this._serializer.Context = value;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x060009D7 RID: 2519 RVA: 0x0002767B File Offset: 0x0002587B
		// (set) Token: 0x060009D8 RID: 2520 RVA: 0x00027688 File Offset: 0x00025888
		public override Formatting Formatting
		{
			get
			{
				return this._serializer.Formatting;
			}
			set
			{
				this._serializer.Formatting = value;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x060009D9 RID: 2521 RVA: 0x00027696 File Offset: 0x00025896
		// (set) Token: 0x060009DA RID: 2522 RVA: 0x000276A3 File Offset: 0x000258A3
		public override DateFormatHandling DateFormatHandling
		{
			get
			{
				return this._serializer.DateFormatHandling;
			}
			set
			{
				this._serializer.DateFormatHandling = value;
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x060009DB RID: 2523 RVA: 0x000276B1 File Offset: 0x000258B1
		// (set) Token: 0x060009DC RID: 2524 RVA: 0x000276BE File Offset: 0x000258BE
		public override DateTimeZoneHandling DateTimeZoneHandling
		{
			get
			{
				return this._serializer.DateTimeZoneHandling;
			}
			set
			{
				this._serializer.DateTimeZoneHandling = value;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x060009DD RID: 2525 RVA: 0x000276CC File Offset: 0x000258CC
		// (set) Token: 0x060009DE RID: 2526 RVA: 0x000276D9 File Offset: 0x000258D9
		public override DateParseHandling DateParseHandling
		{
			get
			{
				return this._serializer.DateParseHandling;
			}
			set
			{
				this._serializer.DateParseHandling = value;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x060009DF RID: 2527 RVA: 0x000276E7 File Offset: 0x000258E7
		// (set) Token: 0x060009E0 RID: 2528 RVA: 0x000276F4 File Offset: 0x000258F4
		public override FloatFormatHandling FloatFormatHandling
		{
			get
			{
				return this._serializer.FloatFormatHandling;
			}
			set
			{
				this._serializer.FloatFormatHandling = value;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060009E1 RID: 2529 RVA: 0x00027702 File Offset: 0x00025902
		// (set) Token: 0x060009E2 RID: 2530 RVA: 0x0002770F File Offset: 0x0002590F
		public override FloatParseHandling FloatParseHandling
		{
			get
			{
				return this._serializer.FloatParseHandling;
			}
			set
			{
				this._serializer.FloatParseHandling = value;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x060009E3 RID: 2531 RVA: 0x0002771D File Offset: 0x0002591D
		// (set) Token: 0x060009E4 RID: 2532 RVA: 0x0002772A File Offset: 0x0002592A
		public override StringEscapeHandling StringEscapeHandling
		{
			get
			{
				return this._serializer.StringEscapeHandling;
			}
			set
			{
				this._serializer.StringEscapeHandling = value;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060009E5 RID: 2533 RVA: 0x00027738 File Offset: 0x00025938
		// (set) Token: 0x060009E6 RID: 2534 RVA: 0x00027745 File Offset: 0x00025945
		public override string DateFormatString
		{
			get
			{
				return this._serializer.DateFormatString;
			}
			set
			{
				this._serializer.DateFormatString = value;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x060009E7 RID: 2535 RVA: 0x00027753 File Offset: 0x00025953
		// (set) Token: 0x060009E8 RID: 2536 RVA: 0x00027760 File Offset: 0x00025960
		public override CultureInfo Culture
		{
			get
			{
				return this._serializer.Culture;
			}
			set
			{
				this._serializer.Culture = value;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x060009E9 RID: 2537 RVA: 0x0002776E File Offset: 0x0002596E
		// (set) Token: 0x060009EA RID: 2538 RVA: 0x0002777B File Offset: 0x0002597B
		public override int? MaxDepth
		{
			get
			{
				return this._serializer.MaxDepth;
			}
			set
			{
				this._serializer.MaxDepth = value;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x060009EB RID: 2539 RVA: 0x00027789 File Offset: 0x00025989
		// (set) Token: 0x060009EC RID: 2540 RVA: 0x00027796 File Offset: 0x00025996
		public override bool CheckAdditionalContent
		{
			get
			{
				return this._serializer.CheckAdditionalContent;
			}
			set
			{
				this._serializer.CheckAdditionalContent = value;
			}
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x000277A4 File Offset: 0x000259A4
		internal JsonSerializerInternalBase GetInternalSerializer()
		{
			if (this._serializerReader != null)
			{
				return this._serializerReader;
			}
			return this._serializerWriter;
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x000277BB File Offset: 0x000259BB
		public JsonSerializerProxy(JsonSerializerInternalReader serializerReader)
		{
			ValidationUtils.ArgumentNotNull(serializerReader, "serializerReader");
			this._serializerReader = serializerReader;
			this._serializer = serializerReader.Serializer;
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x000277E1 File Offset: 0x000259E1
		public JsonSerializerProxy(JsonSerializerInternalWriter serializerWriter)
		{
			ValidationUtils.ArgumentNotNull(serializerWriter, "serializerWriter");
			this._serializerWriter = serializerWriter;
			this._serializer = serializerWriter.Serializer;
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x00027807 File Offset: 0x00025A07
		internal override object DeserializeInternal(JsonReader reader, Type objectType)
		{
			if (this._serializerReader != null)
			{
				return this._serializerReader.Deserialize(reader, objectType, false);
			}
			return this._serializer.Deserialize(reader, objectType);
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x0002782D File Offset: 0x00025A2D
		internal override void PopulateInternal(JsonReader reader, object target)
		{
			if (this._serializerReader != null)
			{
				this._serializerReader.Populate(reader, target);
				return;
			}
			this._serializer.Populate(reader, target);
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x00027852 File Offset: 0x00025A52
		internal override void SerializeInternal(JsonWriter jsonWriter, object value, Type rootType)
		{
			if (this._serializerWriter != null)
			{
				this._serializerWriter.Serialize(jsonWriter, value, rootType);
				return;
			}
			this._serializer.Serialize(jsonWriter, value);
		}

		// Token: 0x0400035D RID: 861
		private readonly JsonSerializerInternalReader _serializerReader;

		// Token: 0x0400035E RID: 862
		private readonly JsonSerializerInternalWriter _serializerWriter;

		// Token: 0x0400035F RID: 863
		private readonly JsonSerializer _serializer;
	}
}
