using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x02000058 RID: 88
	public class JsonTextWriter : JsonWriter
	{
		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x0000E819 File Offset: 0x0000CA19
		private Base64Encoder Base64Encoder
		{
			get
			{
				if (this._base64Encoder == null)
				{
					this._base64Encoder = new Base64Encoder(this._writer);
				}
				return this._base64Encoder;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x0000E83A File Offset: 0x0000CA3A
		// (set) Token: 0x060003D2 RID: 978 RVA: 0x0000E842 File Offset: 0x0000CA42
		public int Indentation
		{
			get
			{
				return this._indentation;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException("Indentation value must be greater than 0.");
				}
				this._indentation = value;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x0000E85A File Offset: 0x0000CA5A
		// (set) Token: 0x060003D4 RID: 980 RVA: 0x0000E862 File Offset: 0x0000CA62
		public char QuoteChar
		{
			get
			{
				return this._quoteChar;
			}
			set
			{
				if (value != '"' && value != '\'')
				{
					throw new ArgumentException("Invalid JavaScript string quote character. Valid quote characters are ' and \".");
				}
				this._quoteChar = value;
				this.UpdateCharEscapeFlags();
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x0000E886 File Offset: 0x0000CA86
		// (set) Token: 0x060003D6 RID: 982 RVA: 0x0000E88E File Offset: 0x0000CA8E
		public char IndentChar
		{
			get
			{
				return this._indentChar;
			}
			set
			{
				if (value != this._indentChar)
				{
					this._indentChar = value;
					this._indentChars = null;
				}
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x0000E8A7 File Offset: 0x0000CAA7
		// (set) Token: 0x060003D8 RID: 984 RVA: 0x0000E8AF File Offset: 0x0000CAAF
		public bool QuoteName
		{
			get
			{
				return this._quoteName;
			}
			set
			{
				this._quoteName = value;
			}
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000E8B8 File Offset: 0x0000CAB8
		public JsonTextWriter(TextWriter textWriter)
		{
			if (textWriter == null)
			{
				throw new ArgumentNullException("textWriter");
			}
			this._writer = textWriter;
			this._quoteChar = '"';
			this._quoteName = true;
			this._indentChar = ' ';
			this._indentation = 2;
			this.UpdateCharEscapeFlags();
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000E904 File Offset: 0x0000CB04
		public override void Flush()
		{
			this._writer.Flush();
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000E911 File Offset: 0x0000CB11
		public override void Close()
		{
			base.Close();
			if (base.CloseOutput && this._writer != null)
			{
				this._writer.Close();
			}
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000E934 File Offset: 0x0000CB34
		public override void WriteStartObject()
		{
			base.InternalWriteStart(JsonToken.StartObject, JsonContainerType.Object);
			this._writer.Write('{');
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000E94B File Offset: 0x0000CB4B
		public override void WriteStartArray()
		{
			base.InternalWriteStart(JsonToken.StartArray, JsonContainerType.Array);
			this._writer.Write('[');
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000E962 File Offset: 0x0000CB62
		public override void WriteStartConstructor(string name)
		{
			base.InternalWriteStart(JsonToken.StartConstructor, JsonContainerType.Constructor);
			this._writer.Write("new ");
			this._writer.Write(name);
			this._writer.Write('(');
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000E998 File Offset: 0x0000CB98
		protected override void WriteEnd(JsonToken token)
		{
			switch (token)
			{
			case JsonToken.EndObject:
				this._writer.Write('}');
				return;
			case JsonToken.EndArray:
				this._writer.Write(']');
				return;
			case JsonToken.EndConstructor:
				this._writer.Write(')');
				return;
			default:
				throw JsonWriterException.Create(this, "Invalid JsonToken: " + token, null);
			}
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000E9FF File Offset: 0x0000CBFF
		public override void WritePropertyName(string name)
		{
			base.InternalWritePropertyName(name);
			this.WriteEscapedString(name, this._quoteName);
			this._writer.Write(':');
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000EA24 File Offset: 0x0000CC24
		public override void WritePropertyName(string name, bool escape)
		{
			base.InternalWritePropertyName(name);
			if (escape)
			{
				this.WriteEscapedString(name, this._quoteName);
			}
			else
			{
				if (this._quoteName)
				{
					this._writer.Write(this._quoteChar);
				}
				this._writer.Write(name);
				if (this._quoteName)
				{
					this._writer.Write(this._quoteChar);
				}
			}
			this._writer.Write(':');
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000EA95 File Offset: 0x0000CC95
		internal override void OnStringEscapeHandlingChanged()
		{
			this.UpdateCharEscapeFlags();
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000EA9D File Offset: 0x0000CC9D
		private void UpdateCharEscapeFlags()
		{
			this._charEscapeFlags = JavaScriptUtils.GetCharEscapeFlags(base.StringEscapeHandling, this._quoteChar);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000EAB8 File Offset: 0x0000CCB8
		protected override void WriteIndent()
		{
			this._writer.WriteLine();
			int i = base.Top * this._indentation;
			if (i > 0)
			{
				if (this._indentChars == null)
				{
					this._indentChars = new string(this._indentChar, 10).ToCharArray();
				}
				while (i > 0)
				{
					int num = Math.Min(i, 10);
					this._writer.Write(this._indentChars, 0, num);
					i -= num;
				}
			}
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000EB28 File Offset: 0x0000CD28
		protected override void WriteValueDelimiter()
		{
			this._writer.Write(',');
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000EB37 File Offset: 0x0000CD37
		protected override void WriteIndentSpace()
		{
			this._writer.Write(' ');
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000EB46 File Offset: 0x0000CD46
		private void WriteValueInternal(string value, JsonToken token)
		{
			this._writer.Write(value);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000EB54 File Offset: 0x0000CD54
		public override void WriteValue(object value)
		{
			if (value is BigInteger)
			{
				base.InternalWriteValue(JsonToken.Integer);
				this.WriteValueInternal(((BigInteger)value).ToString(CultureInfo.InvariantCulture), JsonToken.String);
				return;
			}
			base.WriteValue(value);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000EB93 File Offset: 0x0000CD93
		public override void WriteNull()
		{
			base.InternalWriteValue(JsonToken.Null);
			this.WriteValueInternal(JsonConvert.Null, JsonToken.Null);
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000EBAA File Offset: 0x0000CDAA
		public override void WriteUndefined()
		{
			base.InternalWriteValue(JsonToken.Undefined);
			this.WriteValueInternal(JsonConvert.Undefined, JsonToken.Undefined);
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000EBC1 File Offset: 0x0000CDC1
		public override void WriteRaw(string json)
		{
			base.InternalWriteRaw();
			this._writer.Write(json);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000EBD5 File Offset: 0x0000CDD5
		public override void WriteValue(string value)
		{
			base.InternalWriteValue(JsonToken.String);
			if (value == null)
			{
				this.WriteValueInternal(JsonConvert.Null, JsonToken.Null);
				return;
			}
			this.WriteEscapedString(value, true);
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0000EBF8 File Offset: 0x0000CDF8
		private void WriteEscapedString(string value, bool quote)
		{
			this.EnsureWriteBuffer();
			JavaScriptUtils.WriteEscapedJavaScriptString(this._writer, value, this._quoteChar, quote, this._charEscapeFlags, base.StringEscapeHandling, ref this._writeBuffer);
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000EC25 File Offset: 0x0000CE25
		public override void WriteValue(int value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue((long)value);
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000EC36 File Offset: 0x0000CE36
		[CLSCompliant(false)]
		public override void WriteValue(uint value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue((long)((ulong)value));
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000EC47 File Offset: 0x0000CE47
		public override void WriteValue(long value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue(value);
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000EC57 File Offset: 0x0000CE57
		[CLSCompliant(false)]
		public override void WriteValue(ulong value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue(value);
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000EC67 File Offset: 0x0000CE67
		public override void WriteValue(float value)
		{
			base.InternalWriteValue(JsonToken.Float);
			this.WriteValueInternal(JsonConvert.ToString(value, base.FloatFormatHandling, this.QuoteChar, false), JsonToken.Float);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000EC8A File Offset: 0x0000CE8A
		public override void WriteValue(float? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			base.InternalWriteValue(JsonToken.Float);
			this.WriteValueInternal(JsonConvert.ToString(value.Value, base.FloatFormatHandling, this.QuoteChar, true), JsonToken.Float);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000ECC3 File Offset: 0x0000CEC3
		public override void WriteValue(double value)
		{
			base.InternalWriteValue(JsonToken.Float);
			this.WriteValueInternal(JsonConvert.ToString(value, base.FloatFormatHandling, this.QuoteChar, false), JsonToken.Float);
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000ECE6 File Offset: 0x0000CEE6
		public override void WriteValue(double? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			base.InternalWriteValue(JsonToken.Float);
			this.WriteValueInternal(JsonConvert.ToString(value.Value, base.FloatFormatHandling, this.QuoteChar, true), JsonToken.Float);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000ED1F File Offset: 0x0000CF1F
		public override void WriteValue(bool value)
		{
			base.InternalWriteValue(JsonToken.Boolean);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Boolean);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000ED37 File Offset: 0x0000CF37
		public override void WriteValue(short value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue((long)value);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000ED48 File Offset: 0x0000CF48
		[CLSCompliant(false)]
		public override void WriteValue(ushort value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue((long)((ulong)value));
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000ED59 File Offset: 0x0000CF59
		public override void WriteValue(char value)
		{
			base.InternalWriteValue(JsonToken.String);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.String);
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000ED71 File Offset: 0x0000CF71
		public override void WriteValue(byte value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue((long)((ulong)value));
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000ED82 File Offset: 0x0000CF82
		[CLSCompliant(false)]
		public override void WriteValue(sbyte value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue((long)value);
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000ED93 File Offset: 0x0000CF93
		public override void WriteValue(decimal value)
		{
			base.InternalWriteValue(JsonToken.Float);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Float);
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000EDAC File Offset: 0x0000CFAC
		public override void WriteValue(DateTime value)
		{
			base.InternalWriteValue(JsonToken.Date);
			value = DateTimeUtils.EnsureDateTime(value, base.DateTimeZoneHandling);
			if (string.IsNullOrEmpty(base.DateFormatString))
			{
				this.EnsureWriteBuffer();
				int num = 0;
				this._writeBuffer[num++] = this._quoteChar;
				num = DateTimeUtils.WriteDateTimeString(this._writeBuffer, num, value, null, value.Kind, base.DateFormatHandling);
				this._writeBuffer[num++] = this._quoteChar;
				this._writer.Write(this._writeBuffer, 0, num);
				return;
			}
			this._writer.Write(this._quoteChar);
			this._writer.Write(value.ToString(base.DateFormatString, base.Culture));
			this._writer.Write(this._quoteChar);
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000EE80 File Offset: 0x0000D080
		public override void WriteValue(byte[] value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			base.InternalWriteValue(JsonToken.Bytes);
			this._writer.Write(this._quoteChar);
			this.Base64Encoder.Encode(value, 0, value.Length);
			this.Base64Encoder.Flush();
			this._writer.Write(this._quoteChar);
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000EEDC File Offset: 0x0000D0DC
		public override void WriteValue(DateTimeOffset value)
		{
			base.InternalWriteValue(JsonToken.Date);
			if (string.IsNullOrEmpty(base.DateFormatString))
			{
				this.EnsureWriteBuffer();
				int num = 0;
				this._writeBuffer[num++] = this._quoteChar;
				num = DateTimeUtils.WriteDateTimeString(this._writeBuffer, num, (base.DateFormatHandling == DateFormatHandling.IsoDateFormat) ? value.DateTime : value.UtcDateTime, new TimeSpan?(value.Offset), DateTimeKind.Local, base.DateFormatHandling);
				this._writeBuffer[num++] = this._quoteChar;
				this._writer.Write(this._writeBuffer, 0, num);
				return;
			}
			this._writer.Write(this._quoteChar);
			this._writer.Write(value.ToString(base.DateFormatString, base.Culture));
			this._writer.Write(this._quoteChar);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000EFB8 File Offset: 0x0000D1B8
		public override void WriteValue(Guid value)
		{
			base.InternalWriteValue(JsonToken.String);
			string value2 = value.ToString("D", CultureInfo.InvariantCulture);
			this._writer.Write(this._quoteChar);
			this._writer.Write(value2);
			this._writer.Write(this._quoteChar);
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0000F010 File Offset: 0x0000D210
		public override void WriteValue(TimeSpan value)
		{
			base.InternalWriteValue(JsonToken.String);
			string value2 = value.ToString(null, CultureInfo.InvariantCulture);
			this._writer.Write(this._quoteChar);
			this._writer.Write(value2);
			this._writer.Write(this._quoteChar);
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0000F061 File Offset: 0x0000D261
		public override void WriteValue(Uri value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			base.InternalWriteValue(JsonToken.String);
			this.WriteEscapedString(value.OriginalString, true);
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000F088 File Offset: 0x0000D288
		public override void WriteComment(string text)
		{
			base.InternalWriteComment();
			this._writer.Write("/*");
			this._writer.Write(text);
			this._writer.Write("*/");
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0000F0BC File Offset: 0x0000D2BC
		public override void WriteWhitespace(string ws)
		{
			base.InternalWriteWhitespace(ws);
			this._writer.Write(ws);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0000F0D1 File Offset: 0x0000D2D1
		private void EnsureWriteBuffer()
		{
			if (this._writeBuffer == null)
			{
				this._writeBuffer = new char[35];
			}
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0000F0E8 File Offset: 0x0000D2E8
		private void WriteIntegerValue(long value)
		{
			if (value >= 0L && value <= 9L)
			{
				this._writer.Write((char)(48L + value));
				return;
			}
			ulong uvalue = (ulong)((value < 0L) ? (-(ulong)value) : value);
			if (value < 0L)
			{
				this._writer.Write('-');
			}
			this.WriteIntegerValue(uvalue);
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0000F138 File Offset: 0x0000D338
		private void WriteIntegerValue(ulong uvalue)
		{
			if (uvalue <= 9UL)
			{
				this._writer.Write((char)(48UL + uvalue));
				return;
			}
			this.EnsureWriteBuffer();
			int num = MathUtils.IntLength(uvalue);
			int num2 = 0;
			do
			{
				this._writeBuffer[num - ++num2] = (char)(48UL + uvalue % 10UL);
				uvalue /= 10UL;
			}
			while (uvalue != 0UL);
			this._writer.Write(this._writeBuffer, 0, num2);
		}

		// Token: 0x04000169 RID: 361
		private readonly TextWriter _writer;

		// Token: 0x0400016A RID: 362
		private Base64Encoder _base64Encoder;

		// Token: 0x0400016B RID: 363
		private char _indentChar;

		// Token: 0x0400016C RID: 364
		private int _indentation;

		// Token: 0x0400016D RID: 365
		private char _quoteChar;

		// Token: 0x0400016E RID: 366
		private bool _quoteName;

		// Token: 0x0400016F RID: 367
		private bool[] _charEscapeFlags;

		// Token: 0x04000170 RID: 368
		private char[] _writeBuffer;

		// Token: 0x04000171 RID: 369
		private char[] _indentChars;
	}
}
