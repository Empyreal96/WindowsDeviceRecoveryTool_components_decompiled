using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x02000055 RID: 85
	public class JsonSerializerSettings
	{
		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000364 RID: 868 RVA: 0x0000C420 File Offset: 0x0000A620
		// (set) Token: 0x06000365 RID: 869 RVA: 0x0000C446 File Offset: 0x0000A646
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

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000366 RID: 870 RVA: 0x0000C454 File Offset: 0x0000A654
		// (set) Token: 0x06000367 RID: 871 RVA: 0x0000C47A File Offset: 0x0000A67A
		public MissingMemberHandling MissingMemberHandling
		{
			get
			{
				MissingMemberHandling? missingMemberHandling = this._missingMemberHandling;
				if (missingMemberHandling == null)
				{
					return MissingMemberHandling.Ignore;
				}
				return missingMemberHandling.GetValueOrDefault();
			}
			set
			{
				this._missingMemberHandling = new MissingMemberHandling?(value);
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000368 RID: 872 RVA: 0x0000C488 File Offset: 0x0000A688
		// (set) Token: 0x06000369 RID: 873 RVA: 0x0000C4AE File Offset: 0x0000A6AE
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

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600036A RID: 874 RVA: 0x0000C4BC File Offset: 0x0000A6BC
		// (set) Token: 0x0600036B RID: 875 RVA: 0x0000C4E2 File Offset: 0x0000A6E2
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

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600036C RID: 876 RVA: 0x0000C4F0 File Offset: 0x0000A6F0
		// (set) Token: 0x0600036D RID: 877 RVA: 0x0000C516 File Offset: 0x0000A716
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

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600036E RID: 878 RVA: 0x0000C524 File Offset: 0x0000A724
		// (set) Token: 0x0600036F RID: 879 RVA: 0x0000C52C File Offset: 0x0000A72C
		public IList<JsonConverter> Converters { get; set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000370 RID: 880 RVA: 0x0000C538 File Offset: 0x0000A738
		// (set) Token: 0x06000371 RID: 881 RVA: 0x0000C55E File Offset: 0x0000A75E
		public PreserveReferencesHandling PreserveReferencesHandling
		{
			get
			{
				PreserveReferencesHandling? preserveReferencesHandling = this._preserveReferencesHandling;
				if (preserveReferencesHandling == null)
				{
					return PreserveReferencesHandling.None;
				}
				return preserveReferencesHandling.GetValueOrDefault();
			}
			set
			{
				this._preserveReferencesHandling = new PreserveReferencesHandling?(value);
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000372 RID: 882 RVA: 0x0000C56C File Offset: 0x0000A76C
		// (set) Token: 0x06000373 RID: 883 RVA: 0x0000C592 File Offset: 0x0000A792
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

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000374 RID: 884 RVA: 0x0000C5A0 File Offset: 0x0000A7A0
		// (set) Token: 0x06000375 RID: 885 RVA: 0x0000C5C6 File Offset: 0x0000A7C6
		public MetadataPropertyHandling MetadataPropertyHandling
		{
			get
			{
				MetadataPropertyHandling? metadataPropertyHandling = this._metadataPropertyHandling;
				if (metadataPropertyHandling == null)
				{
					return MetadataPropertyHandling.Default;
				}
				return metadataPropertyHandling.GetValueOrDefault();
			}
			set
			{
				this._metadataPropertyHandling = new MetadataPropertyHandling?(value);
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000376 RID: 886 RVA: 0x0000C5D4 File Offset: 0x0000A7D4
		// (set) Token: 0x06000377 RID: 887 RVA: 0x0000C5FA File Offset: 0x0000A7FA
		public FormatterAssemblyStyle TypeNameAssemblyFormat
		{
			get
			{
				FormatterAssemblyStyle? typeNameAssemblyFormat = this._typeNameAssemblyFormat;
				if (typeNameAssemblyFormat == null)
				{
					return FormatterAssemblyStyle.Simple;
				}
				return typeNameAssemblyFormat.GetValueOrDefault();
			}
			set
			{
				this._typeNameAssemblyFormat = new FormatterAssemblyStyle?(value);
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000378 RID: 888 RVA: 0x0000C608 File Offset: 0x0000A808
		// (set) Token: 0x06000379 RID: 889 RVA: 0x0000C62E File Offset: 0x0000A82E
		public ConstructorHandling ConstructorHandling
		{
			get
			{
				ConstructorHandling? constructorHandling = this._constructorHandling;
				if (constructorHandling == null)
				{
					return ConstructorHandling.Default;
				}
				return constructorHandling.GetValueOrDefault();
			}
			set
			{
				this._constructorHandling = new ConstructorHandling?(value);
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600037A RID: 890 RVA: 0x0000C63C File Offset: 0x0000A83C
		// (set) Token: 0x0600037B RID: 891 RVA: 0x0000C644 File Offset: 0x0000A844
		public IContractResolver ContractResolver { get; set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600037C RID: 892 RVA: 0x0000C64D File Offset: 0x0000A84D
		// (set) Token: 0x0600037D RID: 893 RVA: 0x0000C655 File Offset: 0x0000A855
		public IReferenceResolver ReferenceResolver { get; set; }

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600037E RID: 894 RVA: 0x0000C65E File Offset: 0x0000A85E
		// (set) Token: 0x0600037F RID: 895 RVA: 0x0000C666 File Offset: 0x0000A866
		public ITraceWriter TraceWriter { get; set; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000380 RID: 896 RVA: 0x0000C66F File Offset: 0x0000A86F
		// (set) Token: 0x06000381 RID: 897 RVA: 0x0000C677 File Offset: 0x0000A877
		public SerializationBinder Binder { get; set; }

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000382 RID: 898 RVA: 0x0000C680 File Offset: 0x0000A880
		// (set) Token: 0x06000383 RID: 899 RVA: 0x0000C688 File Offset: 0x0000A888
		public EventHandler<ErrorEventArgs> Error { get; set; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000384 RID: 900 RVA: 0x0000C694 File Offset: 0x0000A894
		// (set) Token: 0x06000385 RID: 901 RVA: 0x0000C6BE File Offset: 0x0000A8BE
		public StreamingContext Context
		{
			get
			{
				StreamingContext? context = this._context;
				if (context == null)
				{
					return JsonSerializerSettings.DefaultContext;
				}
				return context.GetValueOrDefault();
			}
			set
			{
				this._context = new StreamingContext?(value);
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000386 RID: 902 RVA: 0x0000C6CC File Offset: 0x0000A8CC
		// (set) Token: 0x06000387 RID: 903 RVA: 0x0000C6DD File Offset: 0x0000A8DD
		public string DateFormatString
		{
			get
			{
				return this._dateFormatString ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";
			}
			set
			{
				this._dateFormatString = value;
				this._dateFormatStringSet = true;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000388 RID: 904 RVA: 0x0000C6ED File Offset: 0x0000A8ED
		// (set) Token: 0x06000389 RID: 905 RVA: 0x0000C6F8 File Offset: 0x0000A8F8
		public int? MaxDepth
		{
			get
			{
				return this._maxDepth;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentException("Value must be positive.", "value");
				}
				this._maxDepth = value;
				this._maxDepthSet = true;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600038A RID: 906 RVA: 0x0000C73C File Offset: 0x0000A93C
		// (set) Token: 0x0600038B RID: 907 RVA: 0x0000C762 File Offset: 0x0000A962
		public Formatting Formatting
		{
			get
			{
				Formatting? formatting = this._formatting;
				if (formatting == null)
				{
					return Formatting.None;
				}
				return formatting.GetValueOrDefault();
			}
			set
			{
				this._formatting = new Formatting?(value);
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600038C RID: 908 RVA: 0x0000C770 File Offset: 0x0000A970
		// (set) Token: 0x0600038D RID: 909 RVA: 0x0000C796 File Offset: 0x0000A996
		public DateFormatHandling DateFormatHandling
		{
			get
			{
				DateFormatHandling? dateFormatHandling = this._dateFormatHandling;
				if (dateFormatHandling == null)
				{
					return DateFormatHandling.IsoDateFormat;
				}
				return dateFormatHandling.GetValueOrDefault();
			}
			set
			{
				this._dateFormatHandling = new DateFormatHandling?(value);
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600038E RID: 910 RVA: 0x0000C7A4 File Offset: 0x0000A9A4
		// (set) Token: 0x0600038F RID: 911 RVA: 0x0000C7CA File Offset: 0x0000A9CA
		public DateTimeZoneHandling DateTimeZoneHandling
		{
			get
			{
				DateTimeZoneHandling? dateTimeZoneHandling = this._dateTimeZoneHandling;
				if (dateTimeZoneHandling == null)
				{
					return DateTimeZoneHandling.RoundtripKind;
				}
				return dateTimeZoneHandling.GetValueOrDefault();
			}
			set
			{
				this._dateTimeZoneHandling = new DateTimeZoneHandling?(value);
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000390 RID: 912 RVA: 0x0000C7D8 File Offset: 0x0000A9D8
		// (set) Token: 0x06000391 RID: 913 RVA: 0x0000C7FE File Offset: 0x0000A9FE
		public DateParseHandling DateParseHandling
		{
			get
			{
				DateParseHandling? dateParseHandling = this._dateParseHandling;
				if (dateParseHandling == null)
				{
					return DateParseHandling.DateTime;
				}
				return dateParseHandling.GetValueOrDefault();
			}
			set
			{
				this._dateParseHandling = new DateParseHandling?(value);
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000392 RID: 914 RVA: 0x0000C80C File Offset: 0x0000AA0C
		// (set) Token: 0x06000393 RID: 915 RVA: 0x0000C832 File Offset: 0x0000AA32
		public FloatFormatHandling FloatFormatHandling
		{
			get
			{
				FloatFormatHandling? floatFormatHandling = this._floatFormatHandling;
				if (floatFormatHandling == null)
				{
					return FloatFormatHandling.String;
				}
				return floatFormatHandling.GetValueOrDefault();
			}
			set
			{
				this._floatFormatHandling = new FloatFormatHandling?(value);
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000394 RID: 916 RVA: 0x0000C840 File Offset: 0x0000AA40
		// (set) Token: 0x06000395 RID: 917 RVA: 0x0000C866 File Offset: 0x0000AA66
		public FloatParseHandling FloatParseHandling
		{
			get
			{
				FloatParseHandling? floatParseHandling = this._floatParseHandling;
				if (floatParseHandling == null)
				{
					return FloatParseHandling.Double;
				}
				return floatParseHandling.GetValueOrDefault();
			}
			set
			{
				this._floatParseHandling = new FloatParseHandling?(value);
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000396 RID: 918 RVA: 0x0000C874 File Offset: 0x0000AA74
		// (set) Token: 0x06000397 RID: 919 RVA: 0x0000C89A File Offset: 0x0000AA9A
		public StringEscapeHandling StringEscapeHandling
		{
			get
			{
				StringEscapeHandling? stringEscapeHandling = this._stringEscapeHandling;
				if (stringEscapeHandling == null)
				{
					return StringEscapeHandling.Default;
				}
				return stringEscapeHandling.GetValueOrDefault();
			}
			set
			{
				this._stringEscapeHandling = new StringEscapeHandling?(value);
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000398 RID: 920 RVA: 0x0000C8A8 File Offset: 0x0000AAA8
		// (set) Token: 0x06000399 RID: 921 RVA: 0x0000C8B9 File Offset: 0x0000AAB9
		public CultureInfo Culture
		{
			get
			{
				return this._culture ?? JsonSerializerSettings.DefaultCulture;
			}
			set
			{
				this._culture = value;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600039A RID: 922 RVA: 0x0000C8C4 File Offset: 0x0000AAC4
		// (set) Token: 0x0600039B RID: 923 RVA: 0x0000C8EA File Offset: 0x0000AAEA
		public bool CheckAdditionalContent
		{
			get
			{
				return this._checkAdditionalContent ?? false;
			}
			set
			{
				this._checkAdditionalContent = new bool?(value);
			}
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000C90F File Offset: 0x0000AB0F
		public JsonSerializerSettings()
		{
			this.Converters = new List<JsonConverter>();
		}

		// Token: 0x04000121 RID: 289
		internal const ReferenceLoopHandling DefaultReferenceLoopHandling = ReferenceLoopHandling.Error;

		// Token: 0x04000122 RID: 290
		internal const MissingMemberHandling DefaultMissingMemberHandling = MissingMemberHandling.Ignore;

		// Token: 0x04000123 RID: 291
		internal const NullValueHandling DefaultNullValueHandling = NullValueHandling.Include;

		// Token: 0x04000124 RID: 292
		internal const DefaultValueHandling DefaultDefaultValueHandling = DefaultValueHandling.Include;

		// Token: 0x04000125 RID: 293
		internal const ObjectCreationHandling DefaultObjectCreationHandling = ObjectCreationHandling.Auto;

		// Token: 0x04000126 RID: 294
		internal const PreserveReferencesHandling DefaultPreserveReferencesHandling = PreserveReferencesHandling.None;

		// Token: 0x04000127 RID: 295
		internal const ConstructorHandling DefaultConstructorHandling = ConstructorHandling.Default;

		// Token: 0x04000128 RID: 296
		internal const TypeNameHandling DefaultTypeNameHandling = TypeNameHandling.None;

		// Token: 0x04000129 RID: 297
		internal const MetadataPropertyHandling DefaultMetadataPropertyHandling = MetadataPropertyHandling.Default;

		// Token: 0x0400012A RID: 298
		internal const FormatterAssemblyStyle DefaultTypeNameAssemblyFormat = FormatterAssemblyStyle.Simple;

		// Token: 0x0400012B RID: 299
		internal const Formatting DefaultFormatting = Formatting.None;

		// Token: 0x0400012C RID: 300
		internal const DateFormatHandling DefaultDateFormatHandling = DateFormatHandling.IsoDateFormat;

		// Token: 0x0400012D RID: 301
		internal const DateTimeZoneHandling DefaultDateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;

		// Token: 0x0400012E RID: 302
		internal const DateParseHandling DefaultDateParseHandling = DateParseHandling.DateTime;

		// Token: 0x0400012F RID: 303
		internal const FloatParseHandling DefaultFloatParseHandling = FloatParseHandling.Double;

		// Token: 0x04000130 RID: 304
		internal const FloatFormatHandling DefaultFloatFormatHandling = FloatFormatHandling.String;

		// Token: 0x04000131 RID: 305
		internal const StringEscapeHandling DefaultStringEscapeHandling = StringEscapeHandling.Default;

		// Token: 0x04000132 RID: 306
		internal const FormatterAssemblyStyle DefaultFormatterAssemblyStyle = FormatterAssemblyStyle.Simple;

		// Token: 0x04000133 RID: 307
		internal const bool DefaultCheckAdditionalContent = false;

		// Token: 0x04000134 RID: 308
		internal const string DefaultDateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

		// Token: 0x04000135 RID: 309
		internal static readonly StreamingContext DefaultContext = default(StreamingContext);

		// Token: 0x04000136 RID: 310
		internal static readonly CultureInfo DefaultCulture = CultureInfo.InvariantCulture;

		// Token: 0x04000137 RID: 311
		internal Formatting? _formatting;

		// Token: 0x04000138 RID: 312
		internal DateFormatHandling? _dateFormatHandling;

		// Token: 0x04000139 RID: 313
		internal DateTimeZoneHandling? _dateTimeZoneHandling;

		// Token: 0x0400013A RID: 314
		internal DateParseHandling? _dateParseHandling;

		// Token: 0x0400013B RID: 315
		internal FloatFormatHandling? _floatFormatHandling;

		// Token: 0x0400013C RID: 316
		internal FloatParseHandling? _floatParseHandling;

		// Token: 0x0400013D RID: 317
		internal StringEscapeHandling? _stringEscapeHandling;

		// Token: 0x0400013E RID: 318
		internal CultureInfo _culture;

		// Token: 0x0400013F RID: 319
		internal bool? _checkAdditionalContent;

		// Token: 0x04000140 RID: 320
		internal int? _maxDepth;

		// Token: 0x04000141 RID: 321
		internal bool _maxDepthSet;

		// Token: 0x04000142 RID: 322
		internal string _dateFormatString;

		// Token: 0x04000143 RID: 323
		internal bool _dateFormatStringSet;

		// Token: 0x04000144 RID: 324
		internal FormatterAssemblyStyle? _typeNameAssemblyFormat;

		// Token: 0x04000145 RID: 325
		internal DefaultValueHandling? _defaultValueHandling;

		// Token: 0x04000146 RID: 326
		internal PreserveReferencesHandling? _preserveReferencesHandling;

		// Token: 0x04000147 RID: 327
		internal NullValueHandling? _nullValueHandling;

		// Token: 0x04000148 RID: 328
		internal ObjectCreationHandling? _objectCreationHandling;

		// Token: 0x04000149 RID: 329
		internal MissingMemberHandling? _missingMemberHandling;

		// Token: 0x0400014A RID: 330
		internal ReferenceLoopHandling? _referenceLoopHandling;

		// Token: 0x0400014B RID: 331
		internal StreamingContext? _context;

		// Token: 0x0400014C RID: 332
		internal ConstructorHandling? _constructorHandling;

		// Token: 0x0400014D RID: 333
		internal TypeNameHandling? _typeNameHandling;

		// Token: 0x0400014E RID: 334
		internal MetadataPropertyHandling? _metadataPropertyHandling;
	}
}
