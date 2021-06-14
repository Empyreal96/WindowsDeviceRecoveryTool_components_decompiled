using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x02000013 RID: 19
	public abstract class JsonWriter : IDisposable
	{
		// Token: 0x0600009A RID: 154 RVA: 0x0000455C File Offset: 0x0000275C
		internal static JsonWriter.State[][] BuildStateArray()
		{
			List<JsonWriter.State[]> list = JsonWriter.StateArrayTempate.ToList<JsonWriter.State[]>();
			JsonWriter.State[] item = JsonWriter.StateArrayTempate[0];
			JsonWriter.State[] item2 = JsonWriter.StateArrayTempate[7];
			foreach (object obj in EnumUtils.GetValues(typeof(JsonToken)))
			{
				JsonToken jsonToken = (JsonToken)obj;
				if (list.Count <= (int)jsonToken)
				{
					switch (jsonToken)
					{
					case JsonToken.Integer:
					case JsonToken.Float:
					case JsonToken.String:
					case JsonToken.Boolean:
					case JsonToken.Null:
					case JsonToken.Undefined:
					case JsonToken.Date:
					case JsonToken.Bytes:
						list.Add(item2);
						continue;
					}
					list.Add(item);
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600009B RID: 155 RVA: 0x0000462C File Offset: 0x0000282C
		static JsonWriter()
		{
			JsonWriter.StateArray = JsonWriter.BuildStateArray();
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600009C RID: 156 RVA: 0x0000484A File Offset: 0x00002A4A
		// (set) Token: 0x0600009D RID: 157 RVA: 0x00004852 File Offset: 0x00002A52
		public bool CloseOutput { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600009E RID: 158 RVA: 0x0000485C File Offset: 0x00002A5C
		protected internal int Top
		{
			get
			{
				int num = this._stack.Count;
				if (this.Peek() != JsonContainerType.None)
				{
					num++;
				}
				return num;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00004884 File Offset: 0x00002A84
		public WriteState WriteState
		{
			get
			{
				switch (this._currentState)
				{
				case JsonWriter.State.Start:
					return WriteState.Start;
				case JsonWriter.State.Property:
					return WriteState.Property;
				case JsonWriter.State.ObjectStart:
				case JsonWriter.State.Object:
					return WriteState.Object;
				case JsonWriter.State.ArrayStart:
				case JsonWriter.State.Array:
					return WriteState.Array;
				case JsonWriter.State.ConstructorStart:
				case JsonWriter.State.Constructor:
					return WriteState.Constructor;
				case JsonWriter.State.Closed:
					return WriteState.Closed;
				case JsonWriter.State.Error:
					return WriteState.Error;
				default:
					throw JsonWriterException.Create(this, "Invalid state: " + this._currentState, null);
				}
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x000048F2 File Offset: 0x00002AF2
		internal string ContainerPath
		{
			get
			{
				if (this._currentPosition.Type == JsonContainerType.None)
				{
					return string.Empty;
				}
				return JsonPosition.BuildPath(this._stack);
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00004914 File Offset: 0x00002B14
		public string Path
		{
			get
			{
				if (this._currentPosition.Type == JsonContainerType.None)
				{
					return string.Empty;
				}
				bool flag = this._currentState != JsonWriter.State.ArrayStart && this._currentState != JsonWriter.State.ConstructorStart && this._currentState != JsonWriter.State.ObjectStart;
				IEnumerable<JsonPosition> positions = (!flag) ? this._stack : this._stack.Concat(new JsonPosition[]
				{
					this._currentPosition
				});
				return JsonPosition.BuildPath(positions);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x0000498D File Offset: 0x00002B8D
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x00004995 File Offset: 0x00002B95
		public Formatting Formatting
		{
			get
			{
				return this._formatting;
			}
			set
			{
				this._formatting = value;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x0000499E File Offset: 0x00002B9E
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x000049A6 File Offset: 0x00002BA6
		public DateFormatHandling DateFormatHandling
		{
			get
			{
				return this._dateFormatHandling;
			}
			set
			{
				this._dateFormatHandling = value;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x000049AF File Offset: 0x00002BAF
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x000049B7 File Offset: 0x00002BB7
		public DateTimeZoneHandling DateTimeZoneHandling
		{
			get
			{
				return this._dateTimeZoneHandling;
			}
			set
			{
				this._dateTimeZoneHandling = value;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x000049C0 File Offset: 0x00002BC0
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x000049C8 File Offset: 0x00002BC8
		public StringEscapeHandling StringEscapeHandling
		{
			get
			{
				return this._stringEscapeHandling;
			}
			set
			{
				this._stringEscapeHandling = value;
				this.OnStringEscapeHandlingChanged();
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000049D7 File Offset: 0x00002BD7
		internal virtual void OnStringEscapeHandlingChanged()
		{
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000AB RID: 171 RVA: 0x000049D9 File Offset: 0x00002BD9
		// (set) Token: 0x060000AC RID: 172 RVA: 0x000049E1 File Offset: 0x00002BE1
		public FloatFormatHandling FloatFormatHandling
		{
			get
			{
				return this._floatFormatHandling;
			}
			set
			{
				this._floatFormatHandling = value;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000AD RID: 173 RVA: 0x000049EA File Offset: 0x00002BEA
		// (set) Token: 0x060000AE RID: 174 RVA: 0x000049F2 File Offset: 0x00002BF2
		public string DateFormatString
		{
			get
			{
				return this._dateFormatString;
			}
			set
			{
				this._dateFormatString = value;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000AF RID: 175 RVA: 0x000049FB File Offset: 0x00002BFB
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x00004A0C File Offset: 0x00002C0C
		public CultureInfo Culture
		{
			get
			{
				return this._culture ?? CultureInfo.InvariantCulture;
			}
			set
			{
				this._culture = value;
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004A15 File Offset: 0x00002C15
		protected JsonWriter()
		{
			this._stack = new List<JsonPosition>(4);
			this._currentState = JsonWriter.State.Start;
			this._formatting = Formatting.None;
			this._dateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
			this.CloseOutput = true;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00004A45 File Offset: 0x00002C45
		internal void UpdateScopeWithFinishedValue()
		{
			if (this._currentPosition.HasIndex)
			{
				this._currentPosition.Position = this._currentPosition.Position + 1;
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00004A67 File Offset: 0x00002C67
		private void Push(JsonContainerType value)
		{
			if (this._currentPosition.Type != JsonContainerType.None)
			{
				this._stack.Add(this._currentPosition);
			}
			this._currentPosition = new JsonPosition(value);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00004A94 File Offset: 0x00002C94
		private JsonContainerType Pop()
		{
			JsonPosition currentPosition = this._currentPosition;
			if (this._stack.Count > 0)
			{
				this._currentPosition = this._stack[this._stack.Count - 1];
				this._stack.RemoveAt(this._stack.Count - 1);
			}
			else
			{
				this._currentPosition = default(JsonPosition);
			}
			return currentPosition.Type;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00004B01 File Offset: 0x00002D01
		private JsonContainerType Peek()
		{
			return this._currentPosition.Type;
		}

		// Token: 0x060000B6 RID: 182
		public abstract void Flush();

		// Token: 0x060000B7 RID: 183 RVA: 0x00004B0E File Offset: 0x00002D0E
		public virtual void Close()
		{
			this.AutoCompleteAll();
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004B16 File Offset: 0x00002D16
		public virtual void WriteStartObject()
		{
			this.InternalWriteStart(JsonToken.StartObject, JsonContainerType.Object);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00004B20 File Offset: 0x00002D20
		public virtual void WriteEndObject()
		{
			this.InternalWriteEnd(JsonContainerType.Object);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00004B29 File Offset: 0x00002D29
		public virtual void WriteStartArray()
		{
			this.InternalWriteStart(JsonToken.StartArray, JsonContainerType.Array);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004B33 File Offset: 0x00002D33
		public virtual void WriteEndArray()
		{
			this.InternalWriteEnd(JsonContainerType.Array);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00004B3C File Offset: 0x00002D3C
		public virtual void WriteStartConstructor(string name)
		{
			this.InternalWriteStart(JsonToken.StartConstructor, JsonContainerType.Constructor);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00004B46 File Offset: 0x00002D46
		public virtual void WriteEndConstructor()
		{
			this.InternalWriteEnd(JsonContainerType.Constructor);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00004B4F File Offset: 0x00002D4F
		public virtual void WritePropertyName(string name)
		{
			this.InternalWritePropertyName(name);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00004B58 File Offset: 0x00002D58
		public virtual void WritePropertyName(string name, bool escape)
		{
			this.WritePropertyName(name);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004B61 File Offset: 0x00002D61
		public virtual void WriteEnd()
		{
			this.WriteEnd(this.Peek());
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00004B6F File Offset: 0x00002D6F
		public void WriteToken(JsonReader reader)
		{
			this.WriteToken(reader, true, true);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00004B7A File Offset: 0x00002D7A
		public void WriteToken(JsonReader reader, bool writeChildren)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			this.WriteToken(reader, writeChildren, true);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00004B90 File Offset: 0x00002D90
		public void WriteToken(JsonToken token, object value)
		{
			this.WriteTokenInternal(token, value);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00004B9A File Offset: 0x00002D9A
		public void WriteToken(JsonToken token)
		{
			this.WriteTokenInternal(token, null);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004BA4 File Offset: 0x00002DA4
		internal void WriteToken(JsonReader reader, bool writeChildren, bool writeDateConstructorAsDate)
		{
			int initialDepth;
			if (reader.TokenType == JsonToken.None)
			{
				initialDepth = -1;
			}
			else if (!JsonTokenUtils.IsStartToken(reader.TokenType))
			{
				initialDepth = reader.Depth + 1;
			}
			else
			{
				initialDepth = reader.Depth;
			}
			this.WriteToken(reader, initialDepth, writeChildren, writeDateConstructorAsDate);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00004BE8 File Offset: 0x00002DE8
		internal void WriteToken(JsonReader reader, int initialDepth, bool writeChildren, bool writeDateConstructorAsDate)
		{
			do
			{
				if (writeDateConstructorAsDate && reader.TokenType == JsonToken.StartConstructor && string.Equals(reader.Value.ToString(), "Date", StringComparison.Ordinal))
				{
					this.WriteConstructorDate(reader);
				}
				else
				{
					this.WriteTokenInternal(reader.TokenType, reader.Value);
				}
			}
			while (initialDepth - 1 < reader.Depth - (JsonTokenUtils.IsEndToken(reader.TokenType) ? 1 : 0) && writeChildren && reader.Read());
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00004C60 File Offset: 0x00002E60
		private void WriteTokenInternal(JsonToken tokenType, object value)
		{
			switch (tokenType)
			{
			case JsonToken.None:
				return;
			case JsonToken.StartObject:
				this.WriteStartObject();
				return;
			case JsonToken.StartArray:
				this.WriteStartArray();
				return;
			case JsonToken.StartConstructor:
				ValidationUtils.ArgumentNotNull(value, "value");
				this.WriteStartConstructor(value.ToString());
				return;
			case JsonToken.PropertyName:
				ValidationUtils.ArgumentNotNull(value, "value");
				this.WritePropertyName(value.ToString());
				return;
			case JsonToken.Comment:
				this.WriteComment((value != null) ? value.ToString() : null);
				return;
			case JsonToken.Raw:
				this.WriteRawValue((value != null) ? value.ToString() : null);
				return;
			case JsonToken.Integer:
				ValidationUtils.ArgumentNotNull(value, "value");
				if (value is BigInteger)
				{
					this.WriteValue((BigInteger)value);
					return;
				}
				this.WriteValue(Convert.ToInt64(value, CultureInfo.InvariantCulture));
				return;
			case JsonToken.Float:
				ValidationUtils.ArgumentNotNull(value, "value");
				if (value is decimal)
				{
					this.WriteValue((decimal)value);
					return;
				}
				if (value is double)
				{
					this.WriteValue((double)value);
					return;
				}
				if (value is float)
				{
					this.WriteValue((float)value);
					return;
				}
				this.WriteValue(Convert.ToDouble(value, CultureInfo.InvariantCulture));
				return;
			case JsonToken.String:
				ValidationUtils.ArgumentNotNull(value, "value");
				this.WriteValue(value.ToString());
				return;
			case JsonToken.Boolean:
				ValidationUtils.ArgumentNotNull(value, "value");
				this.WriteValue(Convert.ToBoolean(value, CultureInfo.InvariantCulture));
				return;
			case JsonToken.Null:
				this.WriteNull();
				return;
			case JsonToken.Undefined:
				this.WriteUndefined();
				return;
			case JsonToken.EndObject:
				this.WriteEndObject();
				return;
			case JsonToken.EndArray:
				this.WriteEndArray();
				return;
			case JsonToken.EndConstructor:
				this.WriteEndConstructor();
				return;
			case JsonToken.Date:
				ValidationUtils.ArgumentNotNull(value, "value");
				if (value is DateTimeOffset)
				{
					this.WriteValue((DateTimeOffset)value);
					return;
				}
				this.WriteValue(Convert.ToDateTime(value, CultureInfo.InvariantCulture));
				return;
			case JsonToken.Bytes:
				ValidationUtils.ArgumentNotNull(value, "value");
				if (value is Guid)
				{
					this.WriteValue((Guid)value);
					return;
				}
				this.WriteValue((byte[])value);
				return;
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("TokenType", tokenType, "Unexpected token type.");
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00004E88 File Offset: 0x00003088
		private void WriteConstructorDate(JsonReader reader)
		{
			if (!reader.Read())
			{
				throw JsonWriterException.Create(this, "Unexpected end when reading date constructor.", null);
			}
			if (reader.TokenType != JsonToken.Integer)
			{
				throw JsonWriterException.Create(this, "Unexpected token when reading date constructor. Expected Integer, got " + reader.TokenType, null);
			}
			long javaScriptTicks = (long)reader.Value;
			DateTime value = DateTimeUtils.ConvertJavaScriptTicksToDateTime(javaScriptTicks);
			if (!reader.Read())
			{
				throw JsonWriterException.Create(this, "Unexpected end when reading date constructor.", null);
			}
			if (reader.TokenType != JsonToken.EndConstructor)
			{
				throw JsonWriterException.Create(this, "Unexpected token when reading date constructor. Expected EndConstructor, got " + reader.TokenType, null);
			}
			this.WriteValue(value);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00004F28 File Offset: 0x00003128
		private void WriteEnd(JsonContainerType type)
		{
			switch (type)
			{
			case JsonContainerType.Object:
				this.WriteEndObject();
				return;
			case JsonContainerType.Array:
				this.WriteEndArray();
				return;
			case JsonContainerType.Constructor:
				this.WriteEndConstructor();
				return;
			default:
				throw JsonWriterException.Create(this, "Unexpected type when writing end: " + type, null);
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004F79 File Offset: 0x00003179
		private void AutoCompleteAll()
		{
			while (this.Top > 0)
			{
				this.WriteEnd();
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004F8C File Offset: 0x0000318C
		private JsonToken GetCloseTokenForType(JsonContainerType type)
		{
			switch (type)
			{
			case JsonContainerType.Object:
				return JsonToken.EndObject;
			case JsonContainerType.Array:
				return JsonToken.EndArray;
			case JsonContainerType.Constructor:
				return JsonToken.EndConstructor;
			default:
				throw JsonWriterException.Create(this, "No close token for type: " + type, null);
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00004FD4 File Offset: 0x000031D4
		private void AutoCompleteClose(JsonContainerType type)
		{
			int num = 0;
			if (this._currentPosition.Type == type)
			{
				num = 1;
			}
			else
			{
				int num2 = this.Top - 2;
				for (int i = num2; i >= 0; i--)
				{
					int index = num2 - i;
					if (this._stack[index].Type == type)
					{
						num = i + 2;
						break;
					}
				}
			}
			if (num == 0)
			{
				throw JsonWriterException.Create(this, "No token to close.", null);
			}
			for (int j = 0; j < num; j++)
			{
				JsonToken closeTokenForType = this.GetCloseTokenForType(this.Pop());
				if (this._currentState == JsonWriter.State.Property)
				{
					this.WriteNull();
				}
				if (this._formatting == Formatting.Indented && this._currentState != JsonWriter.State.ObjectStart && this._currentState != JsonWriter.State.ArrayStart)
				{
					this.WriteIndent();
				}
				this.WriteEnd(closeTokenForType);
				JsonContainerType jsonContainerType = this.Peek();
				switch (jsonContainerType)
				{
				case JsonContainerType.None:
					this._currentState = JsonWriter.State.Start;
					break;
				case JsonContainerType.Object:
					this._currentState = JsonWriter.State.Object;
					break;
				case JsonContainerType.Array:
					this._currentState = JsonWriter.State.Array;
					break;
				case JsonContainerType.Constructor:
					this._currentState = JsonWriter.State.Array;
					break;
				default:
					throw JsonWriterException.Create(this, "Unknown JsonType: " + jsonContainerType, null);
				}
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000050F6 File Offset: 0x000032F6
		protected virtual void WriteEnd(JsonToken token)
		{
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000050F8 File Offset: 0x000032F8
		protected virtual void WriteIndent()
		{
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000050FA File Offset: 0x000032FA
		protected virtual void WriteValueDelimiter()
		{
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x000050FC File Offset: 0x000032FC
		protected virtual void WriteIndentSpace()
		{
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00005100 File Offset: 0x00003300
		internal void AutoComplete(JsonToken tokenBeingWritten)
		{
			JsonWriter.State state = JsonWriter.StateArray[(int)tokenBeingWritten][(int)this._currentState];
			if (state == JsonWriter.State.Error)
			{
				throw JsonWriterException.Create(this, "Token {0} in state {1} would result in an invalid JSON object.".FormatWith(CultureInfo.InvariantCulture, tokenBeingWritten.ToString(), this._currentState.ToString()), null);
			}
			if ((this._currentState == JsonWriter.State.Object || this._currentState == JsonWriter.State.Array || this._currentState == JsonWriter.State.Constructor) && tokenBeingWritten != JsonToken.Comment)
			{
				this.WriteValueDelimiter();
			}
			if (this._formatting == Formatting.Indented)
			{
				if (this._currentState == JsonWriter.State.Property)
				{
					this.WriteIndentSpace();
				}
				if (this._currentState == JsonWriter.State.Array || this._currentState == JsonWriter.State.ArrayStart || this._currentState == JsonWriter.State.Constructor || this._currentState == JsonWriter.State.ConstructorStart || (tokenBeingWritten == JsonToken.PropertyName && this._currentState != JsonWriter.State.Start))
				{
					this.WriteIndent();
				}
			}
			this._currentState = state;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000051CD File Offset: 0x000033CD
		public virtual void WriteNull()
		{
			this.InternalWriteValue(JsonToken.Null);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000051D7 File Offset: 0x000033D7
		public virtual void WriteUndefined()
		{
			this.InternalWriteValue(JsonToken.Undefined);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000051E1 File Offset: 0x000033E1
		public virtual void WriteRaw(string json)
		{
			this.InternalWriteRaw();
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000051E9 File Offset: 0x000033E9
		public virtual void WriteRawValue(string json)
		{
			this.UpdateScopeWithFinishedValue();
			this.AutoComplete(JsonToken.Undefined);
			this.WriteRaw(json);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00005200 File Offset: 0x00003400
		public virtual void WriteValue(string value)
		{
			this.InternalWriteValue(JsonToken.String);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000520A File Offset: 0x0000340A
		public virtual void WriteValue(int value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00005213 File Offset: 0x00003413
		[CLSCompliant(false)]
		public virtual void WriteValue(uint value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000521C File Offset: 0x0000341C
		public virtual void WriteValue(long value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00005225 File Offset: 0x00003425
		[CLSCompliant(false)]
		public virtual void WriteValue(ulong value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000522E File Offset: 0x0000342E
		public virtual void WriteValue(float value)
		{
			this.InternalWriteValue(JsonToken.Float);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00005237 File Offset: 0x00003437
		public virtual void WriteValue(double value)
		{
			this.InternalWriteValue(JsonToken.Float);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00005240 File Offset: 0x00003440
		public virtual void WriteValue(bool value)
		{
			this.InternalWriteValue(JsonToken.Boolean);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000524A File Offset: 0x0000344A
		public virtual void WriteValue(short value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00005253 File Offset: 0x00003453
		[CLSCompliant(false)]
		public virtual void WriteValue(ushort value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000525C File Offset: 0x0000345C
		public virtual void WriteValue(char value)
		{
			this.InternalWriteValue(JsonToken.String);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00005266 File Offset: 0x00003466
		public virtual void WriteValue(byte value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0000526F File Offset: 0x0000346F
		[CLSCompliant(false)]
		public virtual void WriteValue(sbyte value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00005278 File Offset: 0x00003478
		public virtual void WriteValue(decimal value)
		{
			this.InternalWriteValue(JsonToken.Float);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00005281 File Offset: 0x00003481
		public virtual void WriteValue(DateTime value)
		{
			this.InternalWriteValue(JsonToken.Date);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000528B File Offset: 0x0000348B
		public virtual void WriteValue(DateTimeOffset value)
		{
			this.InternalWriteValue(JsonToken.Date);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00005295 File Offset: 0x00003495
		public virtual void WriteValue(Guid value)
		{
			this.InternalWriteValue(JsonToken.String);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0000529F File Offset: 0x0000349F
		public virtual void WriteValue(TimeSpan value)
		{
			this.InternalWriteValue(JsonToken.String);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000052A9 File Offset: 0x000034A9
		public virtual void WriteValue(int? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000052C8 File Offset: 0x000034C8
		[CLSCompliant(false)]
		public virtual void WriteValue(uint? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000052E7 File Offset: 0x000034E7
		public virtual void WriteValue(long? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00005306 File Offset: 0x00003506
		[CLSCompliant(false)]
		public virtual void WriteValue(ulong? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00005325 File Offset: 0x00003525
		public virtual void WriteValue(float? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005344 File Offset: 0x00003544
		public virtual void WriteValue(double? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00005363 File Offset: 0x00003563
		public virtual void WriteValue(bool? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00005384 File Offset: 0x00003584
		public virtual void WriteValue(short? value)
		{
			short? num = value;
			int? num2 = (num != null) ? new int?((int)num.GetValueOrDefault()) : null;
			if (num2 == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000053D4 File Offset: 0x000035D4
		[CLSCompliant(false)]
		public virtual void WriteValue(ushort? value)
		{
			ushort? num = value;
			int? num2 = (num != null) ? new int?((int)num.GetValueOrDefault()) : null;
			if (num2 == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00005424 File Offset: 0x00003624
		public virtual void WriteValue(char? value)
		{
			char? c = value;
			int? num = (c != null) ? new int?((int)c.GetValueOrDefault()) : null;
			if (num == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00005474 File Offset: 0x00003674
		public virtual void WriteValue(byte? value)
		{
			byte? b = value;
			int? num = (b != null) ? new int?((int)b.GetValueOrDefault()) : null;
			if (num == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x000054C4 File Offset: 0x000036C4
		[CLSCompliant(false)]
		public virtual void WriteValue(sbyte? value)
		{
			sbyte? b = value;
			int? num = (b != null) ? new int?((int)b.GetValueOrDefault()) : null;
			if (num == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00005511 File Offset: 0x00003711
		public virtual void WriteValue(decimal? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00005530 File Offset: 0x00003730
		public virtual void WriteValue(DateTime? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0000554F File Offset: 0x0000374F
		public virtual void WriteValue(DateTimeOffset? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0000556E File Offset: 0x0000376E
		public virtual void WriteValue(Guid? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000558D File Offset: 0x0000378D
		public virtual void WriteValue(TimeSpan? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000055AC File Offset: 0x000037AC
		public virtual void WriteValue(byte[] value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.InternalWriteValue(JsonToken.Bytes);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x000055C0 File Offset: 0x000037C0
		public virtual void WriteValue(Uri value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.InternalWriteValue(JsonToken.String);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000055DA File Offset: 0x000037DA
		public virtual void WriteValue(object value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			if (value is BigInteger)
			{
				throw JsonWriter.CreateUnsupportedTypeException(this, value);
			}
			JsonWriter.WriteValue(this, ConvertUtils.GetTypeCode(value.GetType()), value);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00005608 File Offset: 0x00003808
		public virtual void WriteComment(string text)
		{
			this.InternalWriteComment();
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00005610 File Offset: 0x00003810
		public virtual void WriteWhitespace(string ws)
		{
			this.InternalWriteWhitespace(ws);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00005619 File Offset: 0x00003819
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00005622 File Offset: 0x00003822
		private void Dispose(bool disposing)
		{
			if (this._currentState != JsonWriter.State.Closed)
			{
				this.Close();
			}
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00005634 File Offset: 0x00003834
		internal static void WriteValue(JsonWriter writer, PrimitiveTypeCode typeCode, object value)
		{
			switch (typeCode)
			{
			case PrimitiveTypeCode.Char:
				writer.WriteValue((char)value);
				return;
			case PrimitiveTypeCode.CharNullable:
				writer.WriteValue((value == null) ? null : new char?((char)value));
				return;
			case PrimitiveTypeCode.Boolean:
				writer.WriteValue((bool)value);
				return;
			case PrimitiveTypeCode.BooleanNullable:
				writer.WriteValue((value == null) ? null : new bool?((bool)value));
				return;
			case PrimitiveTypeCode.SByte:
				writer.WriteValue((sbyte)value);
				return;
			case PrimitiveTypeCode.SByteNullable:
				writer.WriteValue((value == null) ? null : new sbyte?((sbyte)value));
				return;
			case PrimitiveTypeCode.Int16:
				writer.WriteValue((short)value);
				return;
			case PrimitiveTypeCode.Int16Nullable:
				writer.WriteValue((value == null) ? null : new short?((short)value));
				return;
			case PrimitiveTypeCode.UInt16:
				writer.WriteValue((ushort)value);
				return;
			case PrimitiveTypeCode.UInt16Nullable:
				writer.WriteValue((value == null) ? null : new ushort?((ushort)value));
				return;
			case PrimitiveTypeCode.Int32:
				writer.WriteValue((int)value);
				return;
			case PrimitiveTypeCode.Int32Nullable:
				writer.WriteValue((value == null) ? null : new int?((int)value));
				return;
			case PrimitiveTypeCode.Byte:
				writer.WriteValue((byte)value);
				return;
			case PrimitiveTypeCode.ByteNullable:
				writer.WriteValue((value == null) ? null : new byte?((byte)value));
				return;
			case PrimitiveTypeCode.UInt32:
				writer.WriteValue((uint)value);
				return;
			case PrimitiveTypeCode.UInt32Nullable:
				writer.WriteValue((value == null) ? null : new uint?((uint)value));
				return;
			case PrimitiveTypeCode.Int64:
				writer.WriteValue((long)value);
				return;
			case PrimitiveTypeCode.Int64Nullable:
				writer.WriteValue((value == null) ? null : new long?((long)value));
				return;
			case PrimitiveTypeCode.UInt64:
				writer.WriteValue((ulong)value);
				return;
			case PrimitiveTypeCode.UInt64Nullable:
				writer.WriteValue((value == null) ? null : new ulong?((ulong)value));
				return;
			case PrimitiveTypeCode.Single:
				writer.WriteValue((float)value);
				return;
			case PrimitiveTypeCode.SingleNullable:
				writer.WriteValue((value == null) ? null : new float?((float)value));
				return;
			case PrimitiveTypeCode.Double:
				writer.WriteValue((double)value);
				return;
			case PrimitiveTypeCode.DoubleNullable:
				writer.WriteValue((value == null) ? null : new double?((double)value));
				return;
			case PrimitiveTypeCode.DateTime:
				writer.WriteValue((DateTime)value);
				return;
			case PrimitiveTypeCode.DateTimeNullable:
				writer.WriteValue((value == null) ? null : new DateTime?((DateTime)value));
				return;
			case PrimitiveTypeCode.DateTimeOffset:
				writer.WriteValue((DateTimeOffset)value);
				return;
			case PrimitiveTypeCode.DateTimeOffsetNullable:
				writer.WriteValue((value == null) ? null : new DateTimeOffset?((DateTimeOffset)value));
				return;
			case PrimitiveTypeCode.Decimal:
				writer.WriteValue((decimal)value);
				return;
			case PrimitiveTypeCode.DecimalNullable:
				writer.WriteValue((value == null) ? null : new decimal?((decimal)value));
				return;
			case PrimitiveTypeCode.Guid:
				writer.WriteValue((Guid)value);
				return;
			case PrimitiveTypeCode.GuidNullable:
				writer.WriteValue((value == null) ? null : new Guid?((Guid)value));
				return;
			case PrimitiveTypeCode.TimeSpan:
				writer.WriteValue((TimeSpan)value);
				return;
			case PrimitiveTypeCode.TimeSpanNullable:
				writer.WriteValue((value == null) ? null : new TimeSpan?((TimeSpan)value));
				return;
			case PrimitiveTypeCode.BigInteger:
				writer.WriteValue((BigInteger)value);
				return;
			case PrimitiveTypeCode.BigIntegerNullable:
				writer.WriteValue((value == null) ? null : new BigInteger?((BigInteger)value));
				return;
			case PrimitiveTypeCode.Uri:
				writer.WriteValue((Uri)value);
				return;
			case PrimitiveTypeCode.String:
				writer.WriteValue((string)value);
				return;
			case PrimitiveTypeCode.Bytes:
				writer.WriteValue((byte[])value);
				return;
			case PrimitiveTypeCode.DBNull:
				writer.WriteNull();
				return;
			default:
				if (value is IConvertible)
				{
					IConvertible convertible = (IConvertible)value;
					TypeInformation typeInformation = ConvertUtils.GetTypeInformation(convertible);
					PrimitiveTypeCode typeCode2 = (typeInformation.TypeCode == PrimitiveTypeCode.Object) ? PrimitiveTypeCode.String : typeInformation.TypeCode;
					Type conversionType = (typeInformation.TypeCode == PrimitiveTypeCode.Object) ? typeof(string) : typeInformation.Type;
					object value2 = convertible.ToType(conversionType, CultureInfo.InvariantCulture);
					JsonWriter.WriteValue(writer, typeCode2, value2);
					return;
				}
				throw JsonWriter.CreateUnsupportedTypeException(writer, value);
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00005ACB File Offset: 0x00003CCB
		private static JsonWriterException CreateUnsupportedTypeException(JsonWriter writer, object value)
		{
			return JsonWriterException.Create(writer, "Unsupported type: {0}. Use the JsonSerializer class to get the object's JSON representation.".FormatWith(CultureInfo.InvariantCulture, value.GetType()), null);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00005AEC File Offset: 0x00003CEC
		protected void SetWriteState(JsonToken token, object value)
		{
			switch (token)
			{
			case JsonToken.StartObject:
				this.InternalWriteStart(token, JsonContainerType.Object);
				return;
			case JsonToken.StartArray:
				this.InternalWriteStart(token, JsonContainerType.Array);
				return;
			case JsonToken.StartConstructor:
				this.InternalWriteStart(token, JsonContainerType.Constructor);
				return;
			case JsonToken.PropertyName:
				if (!(value is string))
				{
					throw new ArgumentException("A name is required when setting property name state.", "value");
				}
				this.InternalWritePropertyName((string)value);
				return;
			case JsonToken.Comment:
				this.InternalWriteComment();
				return;
			case JsonToken.Raw:
				this.InternalWriteRaw();
				return;
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Null:
			case JsonToken.Undefined:
			case JsonToken.Date:
			case JsonToken.Bytes:
				this.InternalWriteValue(token);
				return;
			case JsonToken.EndObject:
				this.InternalWriteEnd(JsonContainerType.Object);
				return;
			case JsonToken.EndArray:
				this.InternalWriteEnd(JsonContainerType.Array);
				return;
			case JsonToken.EndConstructor:
				this.InternalWriteEnd(JsonContainerType.Constructor);
				return;
			default:
				throw new ArgumentOutOfRangeException("token");
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00005BC1 File Offset: 0x00003DC1
		internal void InternalWriteEnd(JsonContainerType container)
		{
			this.AutoCompleteClose(container);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00005BCA File Offset: 0x00003DCA
		internal void InternalWritePropertyName(string name)
		{
			this._currentPosition.PropertyName = name;
			this.AutoComplete(JsonToken.PropertyName);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00005BDF File Offset: 0x00003DDF
		internal void InternalWriteRaw()
		{
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00005BE1 File Offset: 0x00003DE1
		internal void InternalWriteStart(JsonToken token, JsonContainerType container)
		{
			this.UpdateScopeWithFinishedValue();
			this.AutoComplete(token);
			this.Push(container);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00005BF7 File Offset: 0x00003DF7
		internal void InternalWriteValue(JsonToken token)
		{
			this.UpdateScopeWithFinishedValue();
			this.AutoComplete(token);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00005C06 File Offset: 0x00003E06
		internal void InternalWriteWhitespace(string ws)
		{
			if (ws != null && !StringUtils.IsWhiteSpace(ws))
			{
				throw JsonWriterException.Create(this, "Only white space characters should be used.", null);
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00005C20 File Offset: 0x00003E20
		internal void InternalWriteComment()
		{
			this.AutoComplete(JsonToken.Comment);
		}

		// Token: 0x0400006A RID: 106
		private static readonly JsonWriter.State[][] StateArray;

		// Token: 0x0400006B RID: 107
		internal static readonly JsonWriter.State[][] StateArrayTempate = new JsonWriter.State[][]
		{
			new JsonWriter.State[]
			{
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.ObjectStart,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.ArrayStart,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.Property,
				JsonWriter.State.Error,
				JsonWriter.State.Property,
				JsonWriter.State.Property,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.Start,
				JsonWriter.State.Property,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.Object,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.Array,
				JsonWriter.State.Constructor,
				JsonWriter.State.Constructor,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.Start,
				JsonWriter.State.Property,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.Object,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.Array,
				JsonWriter.State.Constructor,
				JsonWriter.State.Constructor,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.Start,
				JsonWriter.State.Object,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Array,
				JsonWriter.State.Array,
				JsonWriter.State.Constructor,
				JsonWriter.State.Constructor,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			}
		};

		// Token: 0x0400006C RID: 108
		private readonly List<JsonPosition> _stack;

		// Token: 0x0400006D RID: 109
		private JsonPosition _currentPosition;

		// Token: 0x0400006E RID: 110
		private JsonWriter.State _currentState;

		// Token: 0x0400006F RID: 111
		private Formatting _formatting;

		// Token: 0x04000070 RID: 112
		private DateFormatHandling _dateFormatHandling;

		// Token: 0x04000071 RID: 113
		private DateTimeZoneHandling _dateTimeZoneHandling;

		// Token: 0x04000072 RID: 114
		private StringEscapeHandling _stringEscapeHandling;

		// Token: 0x04000073 RID: 115
		private FloatFormatHandling _floatFormatHandling;

		// Token: 0x04000074 RID: 116
		private string _dateFormatString;

		// Token: 0x04000075 RID: 117
		private CultureInfo _culture;

		// Token: 0x02000014 RID: 20
		internal enum State
		{
			// Token: 0x04000078 RID: 120
			Start,
			// Token: 0x04000079 RID: 121
			Property,
			// Token: 0x0400007A RID: 122
			ObjectStart,
			// Token: 0x0400007B RID: 123
			Object,
			// Token: 0x0400007C RID: 124
			ArrayStart,
			// Token: 0x0400007D RID: 125
			Array,
			// Token: 0x0400007E RID: 126
			ConstructorStart,
			// Token: 0x0400007F RID: 127
			Constructor,
			// Token: 0x04000080 RID: 128
			Closed,
			// Token: 0x04000081 RID: 129
			Error
		}
	}
}
