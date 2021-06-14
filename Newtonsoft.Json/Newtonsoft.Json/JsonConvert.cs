using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x02000047 RID: 71
	public static class JsonConvert
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000272 RID: 626 RVA: 0x0000A148 File Offset: 0x00008348
		// (set) Token: 0x06000273 RID: 627 RVA: 0x0000A14F File Offset: 0x0000834F
		public static Func<JsonSerializerSettings> DefaultSettings { get; set; }

		// Token: 0x06000274 RID: 628 RVA: 0x0000A157 File Offset: 0x00008357
		public static string ToString(DateTime value)
		{
			return JsonConvert.ToString(value, DateFormatHandling.IsoDateFormat, DateTimeZoneHandling.RoundtripKind);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000A164 File Offset: 0x00008364
		public static string ToString(DateTime value, DateFormatHandling format, DateTimeZoneHandling timeZoneHandling)
		{
			DateTime value2 = DateTimeUtils.EnsureDateTime(value, timeZoneHandling);
			string result;
			using (StringWriter stringWriter = StringUtils.CreateStringWriter(64))
			{
				stringWriter.Write('"');
				DateTimeUtils.WriteDateTimeString(stringWriter, value2, format, null, CultureInfo.InvariantCulture);
				stringWriter.Write('"');
				result = stringWriter.ToString();
			}
			return result;
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000A1C4 File Offset: 0x000083C4
		public static string ToString(DateTimeOffset value)
		{
			return JsonConvert.ToString(value, DateFormatHandling.IsoDateFormat);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000A1D0 File Offset: 0x000083D0
		public static string ToString(DateTimeOffset value, DateFormatHandling format)
		{
			string result;
			using (StringWriter stringWriter = StringUtils.CreateStringWriter(64))
			{
				stringWriter.Write('"');
				DateTimeUtils.WriteDateTimeOffsetString(stringWriter, value, format, null, CultureInfo.InvariantCulture);
				stringWriter.Write('"');
				result = stringWriter.ToString();
			}
			return result;
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000A228 File Offset: 0x00008428
		public static string ToString(bool value)
		{
			if (!value)
			{
				return JsonConvert.False;
			}
			return JsonConvert.True;
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000A238 File Offset: 0x00008438
		public static string ToString(char value)
		{
			return JsonConvert.ToString(char.ToString(value));
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000A245 File Offset: 0x00008445
		public static string ToString(Enum value)
		{
			return value.ToString("D");
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000A252 File Offset: 0x00008452
		public static string ToString(int value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000A261 File Offset: 0x00008461
		public static string ToString(short value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000A270 File Offset: 0x00008470
		[CLSCompliant(false)]
		public static string ToString(ushort value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000A27F File Offset: 0x0000847F
		[CLSCompliant(false)]
		public static string ToString(uint value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000A28E File Offset: 0x0000848E
		public static string ToString(long value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000A29D File Offset: 0x0000849D
		private static string ToStringInternal(BigInteger value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000A2AC File Offset: 0x000084AC
		[CLSCompliant(false)]
		public static string ToString(ulong value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000A2BB File Offset: 0x000084BB
		public static string ToString(float value)
		{
			return JsonConvert.EnsureDecimalPlace((double)value, value.ToString("R", CultureInfo.InvariantCulture));
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000A2D5 File Offset: 0x000084D5
		internal static string ToString(float value, FloatFormatHandling floatFormatHandling, char quoteChar, bool nullable)
		{
			return JsonConvert.EnsureFloatFormat((double)value, JsonConvert.EnsureDecimalPlace((double)value, value.ToString("R", CultureInfo.InvariantCulture)), floatFormatHandling, quoteChar, nullable);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000A2F9 File Offset: 0x000084F9
		private static string EnsureFloatFormat(double value, string text, FloatFormatHandling floatFormatHandling, char quoteChar, bool nullable)
		{
			if (floatFormatHandling == FloatFormatHandling.Symbol || (!double.IsInfinity(value) && !double.IsNaN(value)))
			{
				return text;
			}
			if (floatFormatHandling != FloatFormatHandling.DefaultValue)
			{
				return quoteChar + text + quoteChar;
			}
			if (nullable)
			{
				return JsonConvert.Null;
			}
			return "0.0";
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000A337 File Offset: 0x00008537
		public static string ToString(double value)
		{
			return JsonConvert.EnsureDecimalPlace(value, value.ToString("R", CultureInfo.InvariantCulture));
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000A350 File Offset: 0x00008550
		internal static string ToString(double value, FloatFormatHandling floatFormatHandling, char quoteChar, bool nullable)
		{
			return JsonConvert.EnsureFloatFormat(value, JsonConvert.EnsureDecimalPlace(value, value.ToString("R", CultureInfo.InvariantCulture)), floatFormatHandling, quoteChar, nullable);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000A372 File Offset: 0x00008572
		private static string EnsureDecimalPlace(double value, string text)
		{
			if (double.IsNaN(value) || double.IsInfinity(value) || text.IndexOf('.') != -1 || text.IndexOf('E') != -1 || text.IndexOf('e') != -1)
			{
				return text;
			}
			return text + ".0";
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000A3B2 File Offset: 0x000085B2
		private static string EnsureDecimalPlace(string text)
		{
			if (text.IndexOf('.') != -1)
			{
				return text;
			}
			return text + ".0";
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000A3CC File Offset: 0x000085CC
		public static string ToString(byte value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000A3DB File Offset: 0x000085DB
		[CLSCompliant(false)]
		public static string ToString(sbyte value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000A3EA File Offset: 0x000085EA
		public static string ToString(decimal value)
		{
			return JsonConvert.EnsureDecimalPlace(value.ToString(null, CultureInfo.InvariantCulture));
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000A3FE File Offset: 0x000085FE
		public static string ToString(Guid value)
		{
			return JsonConvert.ToString(value, '"');
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000A408 File Offset: 0x00008608
		internal static string ToString(Guid value, char quoteChar)
		{
			string str = value.ToString("D", CultureInfo.InvariantCulture);
			string text = quoteChar.ToString(CultureInfo.InvariantCulture);
			return text + str + text;
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000A43C File Offset: 0x0000863C
		public static string ToString(TimeSpan value)
		{
			return JsonConvert.ToString(value, '"');
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000A446 File Offset: 0x00008646
		internal static string ToString(TimeSpan value, char quoteChar)
		{
			return JsonConvert.ToString(value.ToString(), quoteChar);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000A45B File Offset: 0x0000865B
		public static string ToString(Uri value)
		{
			if (value == null)
			{
				return JsonConvert.Null;
			}
			return JsonConvert.ToString(value, '"');
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000A474 File Offset: 0x00008674
		internal static string ToString(Uri value, char quoteChar)
		{
			return JsonConvert.ToString(value.OriginalString, quoteChar);
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000A482 File Offset: 0x00008682
		public static string ToString(string value)
		{
			return JsonConvert.ToString(value, '"');
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000A48C File Offset: 0x0000868C
		public static string ToString(string value, char delimiter)
		{
			return JsonConvert.ToString(value, delimiter, StringEscapeHandling.Default);
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000A496 File Offset: 0x00008696
		public static string ToString(string value, char delimiter, StringEscapeHandling stringEscapeHandling)
		{
			if (delimiter != '"' && delimiter != '\'')
			{
				throw new ArgumentException("Delimiter must be a single or double quote.", "delimiter");
			}
			return JavaScriptUtils.ToEscapedJavaScriptString(value, delimiter, true, stringEscapeHandling);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000A4BC File Offset: 0x000086BC
		public static string ToString(object value)
		{
			if (value == null)
			{
				return JsonConvert.Null;
			}
			switch (ConvertUtils.GetTypeCode(value.GetType()))
			{
			case PrimitiveTypeCode.Char:
				return JsonConvert.ToString((char)value);
			case PrimitiveTypeCode.Boolean:
				return JsonConvert.ToString((bool)value);
			case PrimitiveTypeCode.SByte:
				return JsonConvert.ToString((sbyte)value);
			case PrimitiveTypeCode.Int16:
				return JsonConvert.ToString((short)value);
			case PrimitiveTypeCode.UInt16:
				return JsonConvert.ToString((ushort)value);
			case PrimitiveTypeCode.Int32:
				return JsonConvert.ToString((int)value);
			case PrimitiveTypeCode.Byte:
				return JsonConvert.ToString((byte)value);
			case PrimitiveTypeCode.UInt32:
				return JsonConvert.ToString((uint)value);
			case PrimitiveTypeCode.Int64:
				return JsonConvert.ToString((long)value);
			case PrimitiveTypeCode.UInt64:
				return JsonConvert.ToString((ulong)value);
			case PrimitiveTypeCode.Single:
				return JsonConvert.ToString((float)value);
			case PrimitiveTypeCode.Double:
				return JsonConvert.ToString((double)value);
			case PrimitiveTypeCode.DateTime:
				return JsonConvert.ToString((DateTime)value);
			case PrimitiveTypeCode.DateTimeOffset:
				return JsonConvert.ToString((DateTimeOffset)value);
			case PrimitiveTypeCode.Decimal:
				return JsonConvert.ToString((decimal)value);
			case PrimitiveTypeCode.Guid:
				return JsonConvert.ToString((Guid)value);
			case PrimitiveTypeCode.TimeSpan:
				return JsonConvert.ToString((TimeSpan)value);
			case PrimitiveTypeCode.BigInteger:
				return JsonConvert.ToStringInternal((BigInteger)value);
			case PrimitiveTypeCode.Uri:
				return JsonConvert.ToString((Uri)value);
			case PrimitiveTypeCode.String:
				return JsonConvert.ToString((string)value);
			case PrimitiveTypeCode.DBNull:
				return JsonConvert.Null;
			}
			throw new ArgumentException("Unsupported type: {0}. Use the JsonSerializer class to get the object's JSON representation.".FormatWith(CultureInfo.InvariantCulture, value.GetType()));
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000A69D File Offset: 0x0000889D
		public static string SerializeObject(object value)
		{
			return JsonConvert.SerializeObject(value, null, null);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000A6A7 File Offset: 0x000088A7
		public static string SerializeObject(object value, Formatting formatting)
		{
			return JsonConvert.SerializeObject(value, formatting, null);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000A6B4 File Offset: 0x000088B4
		public static string SerializeObject(object value, params JsonConverter[] converters)
		{
			JsonSerializerSettings settings = (converters != null && converters.Length > 0) ? new JsonSerializerSettings
			{
				Converters = converters
			} : null;
			return JsonConvert.SerializeObject(value, null, settings);
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000A6E4 File Offset: 0x000088E4
		public static string SerializeObject(object value, Formatting formatting, params JsonConverter[] converters)
		{
			JsonSerializerSettings settings = (converters != null && converters.Length > 0) ? new JsonSerializerSettings
			{
				Converters = converters
			} : null;
			return JsonConvert.SerializeObject(value, null, formatting, settings);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000A715 File Offset: 0x00008915
		public static string SerializeObject(object value, JsonSerializerSettings settings)
		{
			return JsonConvert.SerializeObject(value, null, settings);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000A720 File Offset: 0x00008920
		public static string SerializeObject(object value, Type type, JsonSerializerSettings settings)
		{
			JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
			return JsonConvert.SerializeObjectInternal(value, type, jsonSerializer);
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000A73C File Offset: 0x0000893C
		public static string SerializeObject(object value, Formatting formatting, JsonSerializerSettings settings)
		{
			return JsonConvert.SerializeObject(value, null, formatting, settings);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000A748 File Offset: 0x00008948
		public static string SerializeObject(object value, Type type, Formatting formatting, JsonSerializerSettings settings)
		{
			JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
			jsonSerializer.Formatting = formatting;
			return JsonConvert.SerializeObjectInternal(value, type, jsonSerializer);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000A76C File Offset: 0x0000896C
		private static string SerializeObjectInternal(object value, Type type, JsonSerializer jsonSerializer)
		{
			StringBuilder sb = new StringBuilder(256);
			StringWriter stringWriter = new StringWriter(sb, CultureInfo.InvariantCulture);
			using (JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter))
			{
				jsonTextWriter.Formatting = jsonSerializer.Formatting;
				jsonSerializer.Serialize(jsonTextWriter, value, type);
			}
			return stringWriter.ToString();
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000A7D0 File Offset: 0x000089D0
		[Obsolete("SerializeObjectAsync is obsolete. Use the Task.Factory.StartNew method to serialize JSON asynchronously: Task.Factory.StartNew(() => JsonConvert.SerializeObject(value))")]
		public static Task<string> SerializeObjectAsync(object value)
		{
			return JsonConvert.SerializeObjectAsync(value, Formatting.None, null);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000A7DA File Offset: 0x000089DA
		[Obsolete("SerializeObjectAsync is obsolete. Use the Task.Factory.StartNew method to serialize JSON asynchronously: Task.Factory.StartNew(() => JsonConvert.SerializeObject(value, formatting))")]
		public static Task<string> SerializeObjectAsync(object value, Formatting formatting)
		{
			return JsonConvert.SerializeObjectAsync(value, formatting, null);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000A808 File Offset: 0x00008A08
		[Obsolete("SerializeObjectAsync is obsolete. Use the Task.Factory.StartNew method to serialize JSON asynchronously: Task.Factory.StartNew(() => JsonConvert.SerializeObject(value, formatting, settings))")]
		public static Task<string> SerializeObjectAsync(object value, Formatting formatting, JsonSerializerSettings settings)
		{
			return Task.Factory.StartNew<string>(() => JsonConvert.SerializeObject(value, formatting, settings));
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000A846 File Offset: 0x00008A46
		public static object DeserializeObject(string value)
		{
			return JsonConvert.DeserializeObject(value, null, null);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000A850 File Offset: 0x00008A50
		public static object DeserializeObject(string value, JsonSerializerSettings settings)
		{
			return JsonConvert.DeserializeObject(value, null, settings);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000A85A File Offset: 0x00008A5A
		public static object DeserializeObject(string value, Type type)
		{
			return JsonConvert.DeserializeObject(value, type, null);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000A864 File Offset: 0x00008A64
		public static T DeserializeObject<T>(string value)
		{
			return JsonConvert.DeserializeObject<T>(value, null);
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000A86D File Offset: 0x00008A6D
		public static T DeserializeAnonymousType<T>(string value, T anonymousTypeObject)
		{
			return JsonConvert.DeserializeObject<T>(value);
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000A875 File Offset: 0x00008A75
		public static T DeserializeAnonymousType<T>(string value, T anonymousTypeObject, JsonSerializerSettings settings)
		{
			return JsonConvert.DeserializeObject<T>(value, settings);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000A87E File Offset: 0x00008A7E
		public static T DeserializeObject<T>(string value, params JsonConverter[] converters)
		{
			return (T)((object)JsonConvert.DeserializeObject(value, typeof(T), converters));
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000A896 File Offset: 0x00008A96
		public static T DeserializeObject<T>(string value, JsonSerializerSettings settings)
		{
			return (T)((object)JsonConvert.DeserializeObject(value, typeof(T), settings));
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000A8B0 File Offset: 0x00008AB0
		public static object DeserializeObject(string value, Type type, params JsonConverter[] converters)
		{
			JsonSerializerSettings settings = (converters != null && converters.Length > 0) ? new JsonSerializerSettings
			{
				Converters = converters
			} : null;
			return JsonConvert.DeserializeObject(value, type, settings);
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000A8E0 File Offset: 0x00008AE0
		public static object DeserializeObject(string value, Type type, JsonSerializerSettings settings)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
			if (!jsonSerializer.IsCheckAdditionalContentSet())
			{
				jsonSerializer.CheckAdditionalContent = true;
			}
			object result;
			using (JsonTextReader jsonTextReader = new JsonTextReader(new StringReader(value)))
			{
				result = jsonSerializer.Deserialize(jsonTextReader, type);
			}
			return result;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000A940 File Offset: 0x00008B40
		[Obsolete("DeserializeObjectAsync is obsolete. Use the Task.Factory.StartNew method to deserialize JSON asynchronously: Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(value))")]
		public static Task<T> DeserializeObjectAsync<T>(string value)
		{
			return JsonConvert.DeserializeObjectAsync<T>(value, null);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000A964 File Offset: 0x00008B64
		[Obsolete("DeserializeObjectAsync is obsolete. Use the Task.Factory.StartNew method to deserialize JSON asynchronously: Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(value, settings))")]
		public static Task<T> DeserializeObjectAsync<T>(string value, JsonSerializerSettings settings)
		{
			return Task.Factory.StartNew<T>(() => JsonConvert.DeserializeObject<T>(value, settings));
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000A99B File Offset: 0x00008B9B
		[Obsolete("DeserializeObjectAsync is obsolete. Use the Task.Factory.StartNew method to deserialize JSON asynchronously: Task.Factory.StartNew(() => JsonConvert.DeserializeObject(value))")]
		public static Task<object> DeserializeObjectAsync(string value)
		{
			return JsonConvert.DeserializeObjectAsync(value, null, null);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000A9C8 File Offset: 0x00008BC8
		[Obsolete("DeserializeObjectAsync is obsolete. Use the Task.Factory.StartNew method to deserialize JSON asynchronously: Task.Factory.StartNew(() => JsonConvert.DeserializeObject(value, type, settings))")]
		public static Task<object> DeserializeObjectAsync(string value, Type type, JsonSerializerSettings settings)
		{
			return Task.Factory.StartNew<object>(() => JsonConvert.DeserializeObject(value, type, settings));
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000AA06 File Offset: 0x00008C06
		public static void PopulateObject(string value, object target)
		{
			JsonConvert.PopulateObject(value, target, null);
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000AA10 File Offset: 0x00008C10
		public static void PopulateObject(string value, object target, JsonSerializerSettings settings)
		{
			JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
			using (JsonReader jsonReader = new JsonTextReader(new StringReader(value)))
			{
				jsonSerializer.Populate(jsonReader, target);
				if (jsonReader.Read() && jsonReader.TokenType != JsonToken.Comment)
				{
					throw new JsonSerializationException("Additional text found in JSON string after finishing deserializing object.");
				}
			}
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000AA94 File Offset: 0x00008C94
		[Obsolete("PopulateObjectAsync is obsolete. Use the Task.Factory.StartNew method to populate an object with JSON values asynchronously: Task.Factory.StartNew(() => JsonConvert.PopulateObject(value, target, settings))")]
		public static Task PopulateObjectAsync(string value, object target, JsonSerializerSettings settings)
		{
			return Task.Factory.StartNew(delegate()
			{
				JsonConvert.PopulateObject(value, target, settings);
			});
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000AAD2 File Offset: 0x00008CD2
		public static string SerializeXmlNode(XmlNode node)
		{
			return JsonConvert.SerializeXmlNode(node, Formatting.None);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000AADC File Offset: 0x00008CDC
		public static string SerializeXmlNode(XmlNode node, Formatting formatting)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter();
			return JsonConvert.SerializeObject(node, formatting, new JsonConverter[]
			{
				xmlNodeConverter
			});
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000AB04 File Offset: 0x00008D04
		public static string SerializeXmlNode(XmlNode node, Formatting formatting, bool omitRootObject)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter
			{
				OmitRootObject = omitRootObject
			};
			return JsonConvert.SerializeObject(node, formatting, new JsonConverter[]
			{
				xmlNodeConverter
			});
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000AB33 File Offset: 0x00008D33
		public static XmlDocument DeserializeXmlNode(string value)
		{
			return JsonConvert.DeserializeXmlNode(value, null);
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000AB3C File Offset: 0x00008D3C
		public static XmlDocument DeserializeXmlNode(string value, string deserializeRootElementName)
		{
			return JsonConvert.DeserializeXmlNode(value, deserializeRootElementName, false);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000AB48 File Offset: 0x00008D48
		public static XmlDocument DeserializeXmlNode(string value, string deserializeRootElementName, bool writeArrayAttribute)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter();
			xmlNodeConverter.DeserializeRootElementName = deserializeRootElementName;
			xmlNodeConverter.WriteArrayAttribute = writeArrayAttribute;
			return (XmlDocument)JsonConvert.DeserializeObject(value, typeof(XmlDocument), new JsonConverter[]
			{
				xmlNodeConverter
			});
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000AB8A File Offset: 0x00008D8A
		public static string SerializeXNode(XObject node)
		{
			return JsonConvert.SerializeXNode(node, Formatting.None);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000AB93 File Offset: 0x00008D93
		public static string SerializeXNode(XObject node, Formatting formatting)
		{
			return JsonConvert.SerializeXNode(node, formatting, false);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000ABA0 File Offset: 0x00008DA0
		public static string SerializeXNode(XObject node, Formatting formatting, bool omitRootObject)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter
			{
				OmitRootObject = omitRootObject
			};
			return JsonConvert.SerializeObject(node, formatting, new JsonConverter[]
			{
				xmlNodeConverter
			});
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000ABCF File Offset: 0x00008DCF
		public static XDocument DeserializeXNode(string value)
		{
			return JsonConvert.DeserializeXNode(value, null);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000ABD8 File Offset: 0x00008DD8
		public static XDocument DeserializeXNode(string value, string deserializeRootElementName)
		{
			return JsonConvert.DeserializeXNode(value, deserializeRootElementName, false);
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000ABE4 File Offset: 0x00008DE4
		public static XDocument DeserializeXNode(string value, string deserializeRootElementName, bool writeArrayAttribute)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter();
			xmlNodeConverter.DeserializeRootElementName = deserializeRootElementName;
			xmlNodeConverter.WriteArrayAttribute = writeArrayAttribute;
			return (XDocument)JsonConvert.DeserializeObject(value, typeof(XDocument), new JsonConverter[]
			{
				xmlNodeConverter
			});
		}

		// Token: 0x040000DA RID: 218
		public static readonly string True = "true";

		// Token: 0x040000DB RID: 219
		public static readonly string False = "false";

		// Token: 0x040000DC RID: 220
		public static readonly string Null = "null";

		// Token: 0x040000DD RID: 221
		public static readonly string Undefined = "undefined";

		// Token: 0x040000DE RID: 222
		public static readonly string PositiveInfinity = "Infinity";

		// Token: 0x040000DF RID: 223
		public static readonly string NegativeInfinity = "-Infinity";

		// Token: 0x040000E0 RID: 224
		public static readonly string NaN = "NaN";
	}
}
