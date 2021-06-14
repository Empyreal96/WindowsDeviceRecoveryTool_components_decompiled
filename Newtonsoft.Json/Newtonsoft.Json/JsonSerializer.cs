using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x02000054 RID: 84
	public class JsonSerializer
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000313 RID: 787 RVA: 0x0000B368 File Offset: 0x00009568
		// (remove) Token: 0x06000314 RID: 788 RVA: 0x0000B3A0 File Offset: 0x000095A0
		public virtual event EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs> Error;

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000315 RID: 789 RVA: 0x0000B3D5 File Offset: 0x000095D5
		// (set) Token: 0x06000316 RID: 790 RVA: 0x0000B3DD File Offset: 0x000095DD
		public virtual IReferenceResolver ReferenceResolver
		{
			get
			{
				return this.GetReferenceResolver();
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value", "Reference resolver cannot be null.");
				}
				this._referenceResolver = value;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000317 RID: 791 RVA: 0x0000B3F9 File Offset: 0x000095F9
		// (set) Token: 0x06000318 RID: 792 RVA: 0x0000B401 File Offset: 0x00009601
		public virtual SerializationBinder Binder
		{
			get
			{
				return this._binder;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value", "Serialization binder cannot be null.");
				}
				this._binder = value;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000319 RID: 793 RVA: 0x0000B41D File Offset: 0x0000961D
		// (set) Token: 0x0600031A RID: 794 RVA: 0x0000B425 File Offset: 0x00009625
		public virtual ITraceWriter TraceWriter
		{
			get
			{
				return this._traceWriter;
			}
			set
			{
				this._traceWriter = value;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600031B RID: 795 RVA: 0x0000B42E File Offset: 0x0000962E
		// (set) Token: 0x0600031C RID: 796 RVA: 0x0000B436 File Offset: 0x00009636
		public virtual TypeNameHandling TypeNameHandling
		{
			get
			{
				return this._typeNameHandling;
			}
			set
			{
				if (value < TypeNameHandling.None || value > TypeNameHandling.Auto)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._typeNameHandling = value;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600031D RID: 797 RVA: 0x0000B452 File Offset: 0x00009652
		// (set) Token: 0x0600031E RID: 798 RVA: 0x0000B45A File Offset: 0x0000965A
		public virtual FormatterAssemblyStyle TypeNameAssemblyFormat
		{
			get
			{
				return this._typeNameAssemblyFormat;
			}
			set
			{
				if (value < FormatterAssemblyStyle.Simple || value > FormatterAssemblyStyle.Full)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._typeNameAssemblyFormat = value;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600031F RID: 799 RVA: 0x0000B476 File Offset: 0x00009676
		// (set) Token: 0x06000320 RID: 800 RVA: 0x0000B47E File Offset: 0x0000967E
		public virtual PreserveReferencesHandling PreserveReferencesHandling
		{
			get
			{
				return this._preserveReferencesHandling;
			}
			set
			{
				if (value < PreserveReferencesHandling.None || value > PreserveReferencesHandling.All)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._preserveReferencesHandling = value;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000321 RID: 801 RVA: 0x0000B49A File Offset: 0x0000969A
		// (set) Token: 0x06000322 RID: 802 RVA: 0x0000B4A2 File Offset: 0x000096A2
		public virtual ReferenceLoopHandling ReferenceLoopHandling
		{
			get
			{
				return this._referenceLoopHandling;
			}
			set
			{
				if (value < ReferenceLoopHandling.Error || value > ReferenceLoopHandling.Serialize)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._referenceLoopHandling = value;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000323 RID: 803 RVA: 0x0000B4BE File Offset: 0x000096BE
		// (set) Token: 0x06000324 RID: 804 RVA: 0x0000B4C6 File Offset: 0x000096C6
		public virtual MissingMemberHandling MissingMemberHandling
		{
			get
			{
				return this._missingMemberHandling;
			}
			set
			{
				if (value < MissingMemberHandling.Ignore || value > MissingMemberHandling.Error)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._missingMemberHandling = value;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000325 RID: 805 RVA: 0x0000B4E2 File Offset: 0x000096E2
		// (set) Token: 0x06000326 RID: 806 RVA: 0x0000B4EA File Offset: 0x000096EA
		public virtual NullValueHandling NullValueHandling
		{
			get
			{
				return this._nullValueHandling;
			}
			set
			{
				if (value < NullValueHandling.Include || value > NullValueHandling.Ignore)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._nullValueHandling = value;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000327 RID: 807 RVA: 0x0000B506 File Offset: 0x00009706
		// (set) Token: 0x06000328 RID: 808 RVA: 0x0000B50E File Offset: 0x0000970E
		public virtual DefaultValueHandling DefaultValueHandling
		{
			get
			{
				return this._defaultValueHandling;
			}
			set
			{
				if (value < DefaultValueHandling.Include || value > DefaultValueHandling.IgnoreAndPopulate)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._defaultValueHandling = value;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000329 RID: 809 RVA: 0x0000B52A File Offset: 0x0000972A
		// (set) Token: 0x0600032A RID: 810 RVA: 0x0000B532 File Offset: 0x00009732
		public virtual ObjectCreationHandling ObjectCreationHandling
		{
			get
			{
				return this._objectCreationHandling;
			}
			set
			{
				if (value < ObjectCreationHandling.Auto || value > ObjectCreationHandling.Replace)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._objectCreationHandling = value;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600032B RID: 811 RVA: 0x0000B54E File Offset: 0x0000974E
		// (set) Token: 0x0600032C RID: 812 RVA: 0x0000B556 File Offset: 0x00009756
		public virtual ConstructorHandling ConstructorHandling
		{
			get
			{
				return this._constructorHandling;
			}
			set
			{
				if (value < ConstructorHandling.Default || value > ConstructorHandling.AllowNonPublicDefaultConstructor)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._constructorHandling = value;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600032D RID: 813 RVA: 0x0000B572 File Offset: 0x00009772
		// (set) Token: 0x0600032E RID: 814 RVA: 0x0000B57A File Offset: 0x0000977A
		public virtual MetadataPropertyHandling MetadataPropertyHandling
		{
			get
			{
				return this._metadataPropertyHandling;
			}
			set
			{
				if (value < MetadataPropertyHandling.Default || value > MetadataPropertyHandling.Ignore)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._metadataPropertyHandling = value;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600032F RID: 815 RVA: 0x0000B596 File Offset: 0x00009796
		public virtual JsonConverterCollection Converters
		{
			get
			{
				if (this._converters == null)
				{
					this._converters = new JsonConverterCollection();
				}
				return this._converters;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000330 RID: 816 RVA: 0x0000B5B1 File Offset: 0x000097B1
		// (set) Token: 0x06000331 RID: 817 RVA: 0x0000B5B9 File Offset: 0x000097B9
		public virtual IContractResolver ContractResolver
		{
			get
			{
				return this._contractResolver;
			}
			set
			{
				this._contractResolver = (value ?? DefaultContractResolver.Instance);
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000332 RID: 818 RVA: 0x0000B5CB File Offset: 0x000097CB
		// (set) Token: 0x06000333 RID: 819 RVA: 0x0000B5D3 File Offset: 0x000097D3
		public virtual StreamingContext Context
		{
			get
			{
				return this._context;
			}
			set
			{
				this._context = value;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000334 RID: 820 RVA: 0x0000B5DC File Offset: 0x000097DC
		// (set) Token: 0x06000335 RID: 821 RVA: 0x0000B602 File Offset: 0x00009802
		public virtual Formatting Formatting
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

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000336 RID: 822 RVA: 0x0000B610 File Offset: 0x00009810
		// (set) Token: 0x06000337 RID: 823 RVA: 0x0000B636 File Offset: 0x00009836
		public virtual DateFormatHandling DateFormatHandling
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

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000338 RID: 824 RVA: 0x0000B644 File Offset: 0x00009844
		// (set) Token: 0x06000339 RID: 825 RVA: 0x0000B66A File Offset: 0x0000986A
		public virtual DateTimeZoneHandling DateTimeZoneHandling
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

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600033A RID: 826 RVA: 0x0000B678 File Offset: 0x00009878
		// (set) Token: 0x0600033B RID: 827 RVA: 0x0000B69E File Offset: 0x0000989E
		public virtual DateParseHandling DateParseHandling
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

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600033C RID: 828 RVA: 0x0000B6AC File Offset: 0x000098AC
		// (set) Token: 0x0600033D RID: 829 RVA: 0x0000B6D2 File Offset: 0x000098D2
		public virtual FloatParseHandling FloatParseHandling
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

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600033E RID: 830 RVA: 0x0000B6E0 File Offset: 0x000098E0
		// (set) Token: 0x0600033F RID: 831 RVA: 0x0000B706 File Offset: 0x00009906
		public virtual FloatFormatHandling FloatFormatHandling
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

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000340 RID: 832 RVA: 0x0000B714 File Offset: 0x00009914
		// (set) Token: 0x06000341 RID: 833 RVA: 0x0000B73A File Offset: 0x0000993A
		public virtual StringEscapeHandling StringEscapeHandling
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

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000342 RID: 834 RVA: 0x0000B748 File Offset: 0x00009948
		// (set) Token: 0x06000343 RID: 835 RVA: 0x0000B759 File Offset: 0x00009959
		public virtual string DateFormatString
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

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000344 RID: 836 RVA: 0x0000B769 File Offset: 0x00009969
		// (set) Token: 0x06000345 RID: 837 RVA: 0x0000B77A File Offset: 0x0000997A
		public virtual CultureInfo Culture
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

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000346 RID: 838 RVA: 0x0000B783 File Offset: 0x00009983
		// (set) Token: 0x06000347 RID: 839 RVA: 0x0000B78C File Offset: 0x0000998C
		public virtual int? MaxDepth
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

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000348 RID: 840 RVA: 0x0000B7D0 File Offset: 0x000099D0
		// (set) Token: 0x06000349 RID: 841 RVA: 0x0000B7F6 File Offset: 0x000099F6
		public virtual bool CheckAdditionalContent
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

		// Token: 0x0600034A RID: 842 RVA: 0x0000B804 File Offset: 0x00009A04
		internal bool IsCheckAdditionalContentSet()
		{
			return this._checkAdditionalContent != null;
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000B814 File Offset: 0x00009A14
		public JsonSerializer()
		{
			this._referenceLoopHandling = ReferenceLoopHandling.Error;
			this._missingMemberHandling = MissingMemberHandling.Ignore;
			this._nullValueHandling = NullValueHandling.Include;
			this._defaultValueHandling = DefaultValueHandling.Include;
			this._objectCreationHandling = ObjectCreationHandling.Auto;
			this._preserveReferencesHandling = PreserveReferencesHandling.None;
			this._constructorHandling = ConstructorHandling.Default;
			this._typeNameHandling = TypeNameHandling.None;
			this._metadataPropertyHandling = MetadataPropertyHandling.Default;
			this._context = JsonSerializerSettings.DefaultContext;
			this._binder = DefaultSerializationBinder.Instance;
			this._culture = JsonSerializerSettings.DefaultCulture;
			this._contractResolver = DefaultContractResolver.Instance;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000B892 File Offset: 0x00009A92
		public static JsonSerializer Create()
		{
			return new JsonSerializer();
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000B89C File Offset: 0x00009A9C
		public static JsonSerializer Create(JsonSerializerSettings settings)
		{
			JsonSerializer jsonSerializer = JsonSerializer.Create();
			if (settings != null)
			{
				JsonSerializer.ApplySerializerSettings(jsonSerializer, settings);
			}
			return jsonSerializer;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000B8BC File Offset: 0x00009ABC
		public static JsonSerializer CreateDefault()
		{
			Func<JsonSerializerSettings> defaultSettings = JsonConvert.DefaultSettings;
			JsonSerializerSettings settings = (defaultSettings != null) ? defaultSettings() : null;
			return JsonSerializer.Create(settings);
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000B8E4 File Offset: 0x00009AE4
		public static JsonSerializer CreateDefault(JsonSerializerSettings settings)
		{
			JsonSerializer jsonSerializer = JsonSerializer.CreateDefault();
			if (settings != null)
			{
				JsonSerializer.ApplySerializerSettings(jsonSerializer, settings);
			}
			return jsonSerializer;
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000B904 File Offset: 0x00009B04
		private static void ApplySerializerSettings(JsonSerializer serializer, JsonSerializerSettings settings)
		{
			if (!CollectionUtils.IsNullOrEmpty<JsonConverter>(settings.Converters))
			{
				for (int i = 0; i < settings.Converters.Count; i++)
				{
					serializer.Converters.Insert(i, settings.Converters[i]);
				}
			}
			if (settings._typeNameHandling != null)
			{
				serializer.TypeNameHandling = settings.TypeNameHandling;
			}
			if (settings._metadataPropertyHandling != null)
			{
				serializer.MetadataPropertyHandling = settings.MetadataPropertyHandling;
			}
			if (settings._typeNameAssemblyFormat != null)
			{
				serializer.TypeNameAssemblyFormat = settings.TypeNameAssemblyFormat;
			}
			if (settings._preserveReferencesHandling != null)
			{
				serializer.PreserveReferencesHandling = settings.PreserveReferencesHandling;
			}
			if (settings._referenceLoopHandling != null)
			{
				serializer.ReferenceLoopHandling = settings.ReferenceLoopHandling;
			}
			if (settings._missingMemberHandling != null)
			{
				serializer.MissingMemberHandling = settings.MissingMemberHandling;
			}
			if (settings._objectCreationHandling != null)
			{
				serializer.ObjectCreationHandling = settings.ObjectCreationHandling;
			}
			if (settings._nullValueHandling != null)
			{
				serializer.NullValueHandling = settings.NullValueHandling;
			}
			if (settings._defaultValueHandling != null)
			{
				serializer.DefaultValueHandling = settings.DefaultValueHandling;
			}
			if (settings._constructorHandling != null)
			{
				serializer.ConstructorHandling = settings.ConstructorHandling;
			}
			if (settings._context != null)
			{
				serializer.Context = settings.Context;
			}
			if (settings._checkAdditionalContent != null)
			{
				serializer._checkAdditionalContent = settings._checkAdditionalContent;
			}
			if (settings.Error != null)
			{
				serializer.Error += settings.Error;
			}
			if (settings.ContractResolver != null)
			{
				serializer.ContractResolver = settings.ContractResolver;
			}
			if (settings.ReferenceResolver != null)
			{
				serializer.ReferenceResolver = settings.ReferenceResolver;
			}
			if (settings.TraceWriter != null)
			{
				serializer.TraceWriter = settings.TraceWriter;
			}
			if (settings.Binder != null)
			{
				serializer.Binder = settings.Binder;
			}
			if (settings._formatting != null)
			{
				serializer._formatting = settings._formatting;
			}
			if (settings._dateFormatHandling != null)
			{
				serializer._dateFormatHandling = settings._dateFormatHandling;
			}
			if (settings._dateTimeZoneHandling != null)
			{
				serializer._dateTimeZoneHandling = settings._dateTimeZoneHandling;
			}
			if (settings._dateParseHandling != null)
			{
				serializer._dateParseHandling = settings._dateParseHandling;
			}
			if (settings._dateFormatStringSet)
			{
				serializer._dateFormatString = settings._dateFormatString;
				serializer._dateFormatStringSet = settings._dateFormatStringSet;
			}
			if (settings._floatFormatHandling != null)
			{
				serializer._floatFormatHandling = settings._floatFormatHandling;
			}
			if (settings._floatParseHandling != null)
			{
				serializer._floatParseHandling = settings._floatParseHandling;
			}
			if (settings._stringEscapeHandling != null)
			{
				serializer._stringEscapeHandling = settings._stringEscapeHandling;
			}
			if (settings._culture != null)
			{
				serializer._culture = settings._culture;
			}
			if (settings._maxDepthSet)
			{
				serializer._maxDepth = settings._maxDepth;
				serializer._maxDepthSet = settings._maxDepthSet;
			}
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000BBDF File Offset: 0x00009DDF
		public void Populate(TextReader reader, object target)
		{
			this.Populate(new JsonTextReader(reader), target);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000BBEE File Offset: 0x00009DEE
		public void Populate(JsonReader reader, object target)
		{
			this.PopulateInternal(reader, target);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000BBF8 File Offset: 0x00009DF8
		internal virtual void PopulateInternal(JsonReader reader, object target)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			ValidationUtils.ArgumentNotNull(target, "target");
			CultureInfo previousCulture;
			DateTimeZoneHandling? previousDateTimeZoneHandling;
			DateParseHandling? previousDateParseHandling;
			FloatParseHandling? previousFloatParseHandling;
			int? previousMaxDepth;
			string previousDateFormatString;
			this.SetupReader(reader, out previousCulture, out previousDateTimeZoneHandling, out previousDateParseHandling, out previousFloatParseHandling, out previousMaxDepth, out previousDateFormatString);
			TraceJsonReader traceJsonReader = (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Verbose) ? new TraceJsonReader(reader) : null;
			JsonSerializerInternalReader jsonSerializerInternalReader = new JsonSerializerInternalReader(this);
			jsonSerializerInternalReader.Populate(traceJsonReader ?? reader, target);
			if (traceJsonReader != null)
			{
				this.TraceWriter.Trace(TraceLevel.Verbose, "Deserialized JSON: " + Environment.NewLine + traceJsonReader.GetJson(), null);
			}
			this.ResetReader(reader, previousCulture, previousDateTimeZoneHandling, previousDateParseHandling, previousFloatParseHandling, previousMaxDepth, previousDateFormatString);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000BC9C File Offset: 0x00009E9C
		public object Deserialize(JsonReader reader)
		{
			return this.Deserialize(reader, null);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000BCA6 File Offset: 0x00009EA6
		public object Deserialize(TextReader reader, Type objectType)
		{
			return this.Deserialize(new JsonTextReader(reader), objectType);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000BCB5 File Offset: 0x00009EB5
		public T Deserialize<T>(JsonReader reader)
		{
			return (T)((object)this.Deserialize(reader, typeof(T)));
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000BCCD File Offset: 0x00009ECD
		public object Deserialize(JsonReader reader, Type objectType)
		{
			return this.DeserializeInternal(reader, objectType);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000BCD8 File Offset: 0x00009ED8
		internal virtual object DeserializeInternal(JsonReader reader, Type objectType)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			CultureInfo previousCulture;
			DateTimeZoneHandling? previousDateTimeZoneHandling;
			DateParseHandling? previousDateParseHandling;
			FloatParseHandling? previousFloatParseHandling;
			int? previousMaxDepth;
			string previousDateFormatString;
			this.SetupReader(reader, out previousCulture, out previousDateTimeZoneHandling, out previousDateParseHandling, out previousFloatParseHandling, out previousMaxDepth, out previousDateFormatString);
			TraceJsonReader traceJsonReader = (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Verbose) ? new TraceJsonReader(reader) : null;
			JsonSerializerInternalReader jsonSerializerInternalReader = new JsonSerializerInternalReader(this);
			object result = jsonSerializerInternalReader.Deserialize(traceJsonReader ?? reader, objectType, this.CheckAdditionalContent);
			if (traceJsonReader != null)
			{
				this.TraceWriter.Trace(TraceLevel.Verbose, "Deserialized JSON: " + Environment.NewLine + traceJsonReader.GetJson(), null);
			}
			this.ResetReader(reader, previousCulture, previousDateTimeZoneHandling, previousDateParseHandling, previousFloatParseHandling, previousMaxDepth, previousDateFormatString);
			return result;
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000BD7C File Offset: 0x00009F7C
		private void SetupReader(JsonReader reader, out CultureInfo previousCulture, out DateTimeZoneHandling? previousDateTimeZoneHandling, out DateParseHandling? previousDateParseHandling, out FloatParseHandling? previousFloatParseHandling, out int? previousMaxDepth, out string previousDateFormatString)
		{
			if (this._culture != null && !this._culture.Equals(reader.Culture))
			{
				previousCulture = reader.Culture;
				reader.Culture = this._culture;
			}
			else
			{
				previousCulture = null;
			}
			if (this._dateTimeZoneHandling != null && reader.DateTimeZoneHandling != this._dateTimeZoneHandling)
			{
				previousDateTimeZoneHandling = new DateTimeZoneHandling?(reader.DateTimeZoneHandling);
				reader.DateTimeZoneHandling = this._dateTimeZoneHandling.Value;
			}
			else
			{
				previousDateTimeZoneHandling = null;
			}
			if (this._dateParseHandling != null && reader.DateParseHandling != this._dateParseHandling)
			{
				previousDateParseHandling = new DateParseHandling?(reader.DateParseHandling);
				reader.DateParseHandling = this._dateParseHandling.Value;
			}
			else
			{
				previousDateParseHandling = null;
			}
			if (this._floatParseHandling != null && reader.FloatParseHandling != this._floatParseHandling)
			{
				previousFloatParseHandling = new FloatParseHandling?(reader.FloatParseHandling);
				reader.FloatParseHandling = this._floatParseHandling.Value;
			}
			else
			{
				previousFloatParseHandling = null;
			}
			if (this._maxDepthSet && reader.MaxDepth != this._maxDepth)
			{
				previousMaxDepth = reader.MaxDepth;
				reader.MaxDepth = this._maxDepth;
			}
			else
			{
				previousMaxDepth = null;
			}
			if (this._dateFormatStringSet && reader.DateFormatString != this._dateFormatString)
			{
				previousDateFormatString = reader.DateFormatString;
				reader.DateFormatString = this._dateFormatString;
			}
			else
			{
				previousDateFormatString = null;
			}
			JsonTextReader jsonTextReader = reader as JsonTextReader;
			if (jsonTextReader != null)
			{
				DefaultContractResolver defaultContractResolver = this._contractResolver as DefaultContractResolver;
				if (defaultContractResolver != null)
				{
					jsonTextReader.NameTable = defaultContractResolver.GetState().NameTable;
				}
			}
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000BFA8 File Offset: 0x0000A1A8
		private void ResetReader(JsonReader reader, CultureInfo previousCulture, DateTimeZoneHandling? previousDateTimeZoneHandling, DateParseHandling? previousDateParseHandling, FloatParseHandling? previousFloatParseHandling, int? previousMaxDepth, string previousDateFormatString)
		{
			if (previousCulture != null)
			{
				reader.Culture = previousCulture;
			}
			if (previousDateTimeZoneHandling != null)
			{
				reader.DateTimeZoneHandling = previousDateTimeZoneHandling.Value;
			}
			if (previousDateParseHandling != null)
			{
				reader.DateParseHandling = previousDateParseHandling.Value;
			}
			if (previousFloatParseHandling != null)
			{
				reader.FloatParseHandling = previousFloatParseHandling.Value;
			}
			if (this._maxDepthSet)
			{
				reader.MaxDepth = previousMaxDepth;
			}
			if (this._dateFormatStringSet)
			{
				reader.DateFormatString = previousDateFormatString;
			}
			JsonTextReader jsonTextReader = reader as JsonTextReader;
			if (jsonTextReader != null)
			{
				jsonTextReader.NameTable = null;
			}
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000C032 File Offset: 0x0000A232
		public void Serialize(TextWriter textWriter, object value)
		{
			this.Serialize(new JsonTextWriter(textWriter), value);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000C041 File Offset: 0x0000A241
		public void Serialize(JsonWriter jsonWriter, object value, Type objectType)
		{
			this.SerializeInternal(jsonWriter, value, objectType);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000C04C File Offset: 0x0000A24C
		public void Serialize(TextWriter textWriter, object value, Type objectType)
		{
			this.Serialize(new JsonTextWriter(textWriter), value, objectType);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000C05C File Offset: 0x0000A25C
		public void Serialize(JsonWriter jsonWriter, object value)
		{
			this.SerializeInternal(jsonWriter, value, null);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000C068 File Offset: 0x0000A268
		internal virtual void SerializeInternal(JsonWriter jsonWriter, object value, Type objectType)
		{
			ValidationUtils.ArgumentNotNull(jsonWriter, "jsonWriter");
			Formatting? formatting = null;
			if (this._formatting != null && jsonWriter.Formatting != this._formatting)
			{
				formatting = new Formatting?(jsonWriter.Formatting);
				jsonWriter.Formatting = this._formatting.Value;
			}
			DateFormatHandling? dateFormatHandling = null;
			if (this._dateFormatHandling != null && jsonWriter.DateFormatHandling != this._dateFormatHandling)
			{
				dateFormatHandling = new DateFormatHandling?(jsonWriter.DateFormatHandling);
				jsonWriter.DateFormatHandling = this._dateFormatHandling.Value;
			}
			DateTimeZoneHandling? dateTimeZoneHandling = null;
			if (this._dateTimeZoneHandling != null && jsonWriter.DateTimeZoneHandling != this._dateTimeZoneHandling)
			{
				dateTimeZoneHandling = new DateTimeZoneHandling?(jsonWriter.DateTimeZoneHandling);
				jsonWriter.DateTimeZoneHandling = this._dateTimeZoneHandling.Value;
			}
			FloatFormatHandling? floatFormatHandling = null;
			if (this._floatFormatHandling != null && jsonWriter.FloatFormatHandling != this._floatFormatHandling)
			{
				floatFormatHandling = new FloatFormatHandling?(jsonWriter.FloatFormatHandling);
				jsonWriter.FloatFormatHandling = this._floatFormatHandling.Value;
			}
			StringEscapeHandling? stringEscapeHandling = null;
			if (this._stringEscapeHandling != null && jsonWriter.StringEscapeHandling != this._stringEscapeHandling)
			{
				stringEscapeHandling = new StringEscapeHandling?(jsonWriter.StringEscapeHandling);
				jsonWriter.StringEscapeHandling = this._stringEscapeHandling.Value;
			}
			CultureInfo cultureInfo = null;
			if (this._culture != null && !this._culture.Equals(jsonWriter.Culture))
			{
				cultureInfo = jsonWriter.Culture;
				jsonWriter.Culture = this._culture;
			}
			string dateFormatString = null;
			if (this._dateFormatStringSet && jsonWriter.DateFormatString != this._dateFormatString)
			{
				dateFormatString = jsonWriter.DateFormatString;
				jsonWriter.DateFormatString = this._dateFormatString;
			}
			TraceJsonWriter traceJsonWriter = (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Verbose) ? new TraceJsonWriter(jsonWriter) : null;
			JsonSerializerInternalWriter jsonSerializerInternalWriter = new JsonSerializerInternalWriter(this);
			jsonSerializerInternalWriter.Serialize(traceJsonWriter ?? jsonWriter, value, objectType);
			if (traceJsonWriter != null)
			{
				this.TraceWriter.Trace(TraceLevel.Verbose, "Serialized JSON: " + Environment.NewLine + traceJsonWriter.GetJson(), null);
			}
			if (formatting != null)
			{
				jsonWriter.Formatting = formatting.Value;
			}
			if (dateFormatHandling != null)
			{
				jsonWriter.DateFormatHandling = dateFormatHandling.Value;
			}
			if (dateTimeZoneHandling != null)
			{
				jsonWriter.DateTimeZoneHandling = dateTimeZoneHandling.Value;
			}
			if (floatFormatHandling != null)
			{
				jsonWriter.FloatFormatHandling = floatFormatHandling.Value;
			}
			if (stringEscapeHandling != null)
			{
				jsonWriter.StringEscapeHandling = stringEscapeHandling.Value;
			}
			if (this._dateFormatStringSet)
			{
				jsonWriter.DateFormatString = dateFormatString;
			}
			if (cultureInfo != null)
			{
				jsonWriter.Culture = cultureInfo;
			}
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000C39F File Offset: 0x0000A59F
		internal IReferenceResolver GetReferenceResolver()
		{
			if (this._referenceResolver == null)
			{
				this._referenceResolver = new DefaultReferenceResolver();
			}
			return this._referenceResolver;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000C3BA File Offset: 0x0000A5BA
		internal JsonConverter GetMatchingConverter(Type type)
		{
			return JsonSerializer.GetMatchingConverter(this._converters, type);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000C3C8 File Offset: 0x0000A5C8
		internal static JsonConverter GetMatchingConverter(IList<JsonConverter> converters, Type objectType)
		{
			if (converters != null)
			{
				for (int i = 0; i < converters.Count; i++)
				{
					JsonConverter jsonConverter = converters[i];
					if (jsonConverter.CanConvert(objectType))
					{
						return jsonConverter;
					}
				}
			}
			return null;
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000C400 File Offset: 0x0000A600
		internal void OnError(Newtonsoft.Json.Serialization.ErrorEventArgs e)
		{
			EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs> error = this.Error;
			if (error != null)
			{
				error(this, e);
			}
		}

		// Token: 0x04000103 RID: 259
		internal TypeNameHandling _typeNameHandling;

		// Token: 0x04000104 RID: 260
		internal FormatterAssemblyStyle _typeNameAssemblyFormat;

		// Token: 0x04000105 RID: 261
		internal PreserveReferencesHandling _preserveReferencesHandling;

		// Token: 0x04000106 RID: 262
		internal ReferenceLoopHandling _referenceLoopHandling;

		// Token: 0x04000107 RID: 263
		internal MissingMemberHandling _missingMemberHandling;

		// Token: 0x04000108 RID: 264
		internal ObjectCreationHandling _objectCreationHandling;

		// Token: 0x04000109 RID: 265
		internal NullValueHandling _nullValueHandling;

		// Token: 0x0400010A RID: 266
		internal DefaultValueHandling _defaultValueHandling;

		// Token: 0x0400010B RID: 267
		internal ConstructorHandling _constructorHandling;

		// Token: 0x0400010C RID: 268
		internal MetadataPropertyHandling _metadataPropertyHandling;

		// Token: 0x0400010D RID: 269
		internal JsonConverterCollection _converters;

		// Token: 0x0400010E RID: 270
		internal IContractResolver _contractResolver;

		// Token: 0x0400010F RID: 271
		internal ITraceWriter _traceWriter;

		// Token: 0x04000110 RID: 272
		internal SerializationBinder _binder;

		// Token: 0x04000111 RID: 273
		internal StreamingContext _context;

		// Token: 0x04000112 RID: 274
		private IReferenceResolver _referenceResolver;

		// Token: 0x04000113 RID: 275
		private Formatting? _formatting;

		// Token: 0x04000114 RID: 276
		private DateFormatHandling? _dateFormatHandling;

		// Token: 0x04000115 RID: 277
		private DateTimeZoneHandling? _dateTimeZoneHandling;

		// Token: 0x04000116 RID: 278
		private DateParseHandling? _dateParseHandling;

		// Token: 0x04000117 RID: 279
		private FloatFormatHandling? _floatFormatHandling;

		// Token: 0x04000118 RID: 280
		private FloatParseHandling? _floatParseHandling;

		// Token: 0x04000119 RID: 281
		private StringEscapeHandling? _stringEscapeHandling;

		// Token: 0x0400011A RID: 282
		private CultureInfo _culture;

		// Token: 0x0400011B RID: 283
		private int? _maxDepth;

		// Token: 0x0400011C RID: 284
		private bool _maxDepthSet;

		// Token: 0x0400011D RID: 285
		private bool? _checkAdditionalContent;

		// Token: 0x0400011E RID: 286
		private string _dateFormatString;

		// Token: 0x0400011F RID: 287
		private bool _dateFormatStringSet;
	}
}
