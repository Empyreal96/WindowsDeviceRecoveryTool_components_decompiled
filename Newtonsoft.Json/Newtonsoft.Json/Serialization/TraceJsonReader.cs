using System;
using System.Globalization;
using System.IO;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000CD RID: 205
	internal class TraceJsonReader : JsonReader, IJsonLineInfo
	{
		// Token: 0x06000A16 RID: 2582 RVA: 0x00028090 File Offset: 0x00026290
		public TraceJsonReader(JsonReader innerReader)
		{
			this._innerReader = innerReader;
			this._sw = new StringWriter(CultureInfo.InvariantCulture);
			this._textWriter = new JsonTextWriter(this._sw);
			this._textWriter.Formatting = Formatting.Indented;
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x000280CC File Offset: 0x000262CC
		public string GetJson()
		{
			return this._sw.ToString();
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x000280DC File Offset: 0x000262DC
		public override bool Read()
		{
			bool result = this._innerReader.Read();
			this._textWriter.WriteToken(this._innerReader, false, false);
			return result;
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x0002810C File Offset: 0x0002630C
		public override int? ReadAsInt32()
		{
			int? result = this._innerReader.ReadAsInt32();
			this._textWriter.WriteToken(this._innerReader, false, false);
			return result;
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0002813C File Offset: 0x0002633C
		public override string ReadAsString()
		{
			string result = this._innerReader.ReadAsString();
			this._textWriter.WriteToken(this._innerReader, false, false);
			return result;
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x0002816C File Offset: 0x0002636C
		public override byte[] ReadAsBytes()
		{
			byte[] result = this._innerReader.ReadAsBytes();
			this._textWriter.WriteToken(this._innerReader, false, false);
			return result;
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x0002819C File Offset: 0x0002639C
		public override decimal? ReadAsDecimal()
		{
			decimal? result = this._innerReader.ReadAsDecimal();
			this._textWriter.WriteToken(this._innerReader, false, false);
			return result;
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x000281CC File Offset: 0x000263CC
		public override DateTime? ReadAsDateTime()
		{
			DateTime? result = this._innerReader.ReadAsDateTime();
			this._textWriter.WriteToken(this._innerReader, false, false);
			return result;
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x000281FC File Offset: 0x000263FC
		public override DateTimeOffset? ReadAsDateTimeOffset()
		{
			DateTimeOffset? result = this._innerReader.ReadAsDateTimeOffset();
			this._textWriter.WriteToken(this._innerReader, false, false);
			return result;
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000A1F RID: 2591 RVA: 0x00028229 File Offset: 0x00026429
		public override int Depth
		{
			get
			{
				return this._innerReader.Depth;
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000A20 RID: 2592 RVA: 0x00028236 File Offset: 0x00026436
		public override string Path
		{
			get
			{
				return this._innerReader.Path;
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000A21 RID: 2593 RVA: 0x00028243 File Offset: 0x00026443
		// (set) Token: 0x06000A22 RID: 2594 RVA: 0x00028250 File Offset: 0x00026450
		public override char QuoteChar
		{
			get
			{
				return this._innerReader.QuoteChar;
			}
			protected internal set
			{
				this._innerReader.QuoteChar = value;
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000A23 RID: 2595 RVA: 0x0002825E File Offset: 0x0002645E
		public override JsonToken TokenType
		{
			get
			{
				return this._innerReader.TokenType;
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000A24 RID: 2596 RVA: 0x0002826B File Offset: 0x0002646B
		public override object Value
		{
			get
			{
				return this._innerReader.Value;
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000A25 RID: 2597 RVA: 0x00028278 File Offset: 0x00026478
		public override Type ValueType
		{
			get
			{
				return this._innerReader.ValueType;
			}
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x00028285 File Offset: 0x00026485
		public override void Close()
		{
			this._innerReader.Close();
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x00028294 File Offset: 0x00026494
		bool IJsonLineInfo.HasLineInfo()
		{
			IJsonLineInfo jsonLineInfo = this._innerReader as IJsonLineInfo;
			return jsonLineInfo != null && jsonLineInfo.HasLineInfo();
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000A28 RID: 2600 RVA: 0x000282B8 File Offset: 0x000264B8
		int IJsonLineInfo.LineNumber
		{
			get
			{
				IJsonLineInfo jsonLineInfo = this._innerReader as IJsonLineInfo;
				if (jsonLineInfo == null)
				{
					return 0;
				}
				return jsonLineInfo.LineNumber;
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000A29 RID: 2601 RVA: 0x000282DC File Offset: 0x000264DC
		int IJsonLineInfo.LinePosition
		{
			get
			{
				IJsonLineInfo jsonLineInfo = this._innerReader as IJsonLineInfo;
				if (jsonLineInfo == null)
				{
					return 0;
				}
				return jsonLineInfo.LinePosition;
			}
		}

		// Token: 0x04000370 RID: 880
		private readonly JsonReader _innerReader;

		// Token: 0x04000371 RID: 881
		private readonly JsonTextWriter _textWriter;

		// Token: 0x04000372 RID: 882
		private readonly StringWriter _sw;
	}
}
