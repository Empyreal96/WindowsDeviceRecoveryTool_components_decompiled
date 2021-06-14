using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x02000005 RID: 5
	public abstract class JsonReader : IDisposable
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000011 RID: 17 RVA: 0x00002883 File Offset: 0x00000A83
		protected JsonReader.State CurrentState
		{
			get
			{
				return this._currentState;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000012 RID: 18 RVA: 0x0000288B File Offset: 0x00000A8B
		// (set) Token: 0x06000013 RID: 19 RVA: 0x00002893 File Offset: 0x00000A93
		public bool CloseInput { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000014 RID: 20 RVA: 0x0000289C File Offset: 0x00000A9C
		// (set) Token: 0x06000015 RID: 21 RVA: 0x000028A4 File Offset: 0x00000AA4
		public bool SupportMultipleContent { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000016 RID: 22 RVA: 0x000028AD File Offset: 0x00000AAD
		// (set) Token: 0x06000017 RID: 23 RVA: 0x000028B5 File Offset: 0x00000AB5
		public virtual char QuoteChar
		{
			get
			{
				return this._quoteChar;
			}
			protected internal set
			{
				this._quoteChar = value;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000028BE File Offset: 0x00000ABE
		// (set) Token: 0x06000019 RID: 25 RVA: 0x000028C6 File Offset: 0x00000AC6
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

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000028CF File Offset: 0x00000ACF
		// (set) Token: 0x0600001B RID: 27 RVA: 0x000028D7 File Offset: 0x00000AD7
		public DateParseHandling DateParseHandling
		{
			get
			{
				return this._dateParseHandling;
			}
			set
			{
				this._dateParseHandling = value;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001C RID: 28 RVA: 0x000028E0 File Offset: 0x00000AE0
		// (set) Token: 0x0600001D RID: 29 RVA: 0x000028E8 File Offset: 0x00000AE8
		public FloatParseHandling FloatParseHandling
		{
			get
			{
				return this._floatParseHandling;
			}
			set
			{
				this._floatParseHandling = value;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000028F1 File Offset: 0x00000AF1
		// (set) Token: 0x0600001F RID: 31 RVA: 0x000028F9 File Offset: 0x00000AF9
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

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00002902 File Offset: 0x00000B02
		// (set) Token: 0x06000021 RID: 33 RVA: 0x0000290C File Offset: 0x00000B0C
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
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000022 RID: 34 RVA: 0x00002948 File Offset: 0x00000B48
		public virtual JsonToken TokenType
		{
			get
			{
				return this._tokenType;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002950 File Offset: 0x00000B50
		public virtual object Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00002958 File Offset: 0x00000B58
		public virtual Type ValueType
		{
			get
			{
				if (this._value == null)
				{
					return null;
				}
				return this._value.GetType();
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002970 File Offset: 0x00000B70
		public virtual int Depth
		{
			get
			{
				int count = this._stack.Count;
				if (JsonTokenUtils.IsStartToken(this.TokenType) || this._currentPosition.Type == JsonContainerType.None)
				{
					return count;
				}
				return count + 1;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000026 RID: 38 RVA: 0x000029A8 File Offset: 0x00000BA8
		public virtual string Path
		{
			get
			{
				if (this._currentPosition.Type == JsonContainerType.None)
				{
					return string.Empty;
				}
				bool flag = this._currentState != JsonReader.State.ArrayStart && this._currentState != JsonReader.State.ConstructorStart && this._currentState != JsonReader.State.ObjectStart;
				IEnumerable<JsonPosition> positions = (!flag) ? this._stack : this._stack.Concat(new JsonPosition[]
				{
					this._currentPosition
				});
				return JsonPosition.BuildPath(positions);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002A22 File Offset: 0x00000C22
		// (set) Token: 0x06000028 RID: 40 RVA: 0x00002A33 File Offset: 0x00000C33
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

		// Token: 0x06000029 RID: 41 RVA: 0x00002A3C File Offset: 0x00000C3C
		internal JsonPosition GetPosition(int depth)
		{
			if (depth < this._stack.Count)
			{
				return this._stack[depth];
			}
			return this._currentPosition;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002A5F File Offset: 0x00000C5F
		protected JsonReader()
		{
			this._currentState = JsonReader.State.Start;
			this._stack = new List<JsonPosition>(4);
			this._dateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
			this._dateParseHandling = DateParseHandling.DateTime;
			this._floatParseHandling = FloatParseHandling.Double;
			this.CloseInput = true;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002A98 File Offset: 0x00000C98
		private void Push(JsonContainerType value)
		{
			this.UpdateScopeWithFinishedValue();
			if (this._currentPosition.Type == JsonContainerType.None)
			{
				this._currentPosition = new JsonPosition(value);
				return;
			}
			this._stack.Add(this._currentPosition);
			this._currentPosition = new JsonPosition(value);
			if (this._maxDepth != null && this.Depth + 1 > this._maxDepth && !this._hasExceededMaxDepth)
			{
				this._hasExceededMaxDepth = true;
				throw JsonReaderException.Create(this, "The reader's MaxDepth of {0} has been exceeded.".FormatWith(CultureInfo.InvariantCulture, this._maxDepth));
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002B48 File Offset: 0x00000D48
		private JsonContainerType Pop()
		{
			JsonPosition currentPosition;
			if (this._stack.Count > 0)
			{
				currentPosition = this._currentPosition;
				this._currentPosition = this._stack[this._stack.Count - 1];
				this._stack.RemoveAt(this._stack.Count - 1);
			}
			else
			{
				currentPosition = this._currentPosition;
				this._currentPosition = default(JsonPosition);
			}
			if (this._maxDepth != null && this.Depth <= this._maxDepth)
			{
				this._hasExceededMaxDepth = false;
			}
			return currentPosition.Type;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002BF4 File Offset: 0x00000DF4
		private JsonContainerType Peek()
		{
			return this._currentPosition.Type;
		}

		// Token: 0x0600002E RID: 46
		public abstract bool Read();

		// Token: 0x0600002F RID: 47
		public abstract int? ReadAsInt32();

		// Token: 0x06000030 RID: 48
		public abstract string ReadAsString();

		// Token: 0x06000031 RID: 49
		public abstract byte[] ReadAsBytes();

		// Token: 0x06000032 RID: 50
		public abstract decimal? ReadAsDecimal();

		// Token: 0x06000033 RID: 51
		public abstract DateTime? ReadAsDateTime();

		// Token: 0x06000034 RID: 52
		public abstract DateTimeOffset? ReadAsDateTimeOffset();

		// Token: 0x06000035 RID: 53 RVA: 0x00002C01 File Offset: 0x00000E01
		internal virtual bool ReadInternal()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002C08 File Offset: 0x00000E08
		internal DateTimeOffset? ReadAsDateTimeOffsetInternal()
		{
			this._readType = ReadType.ReadAsDateTimeOffset;
			while (this.ReadInternal())
			{
				JsonToken tokenType = this.TokenType;
				if (tokenType != JsonToken.Comment)
				{
					if (tokenType == JsonToken.Date)
					{
						if (this.Value is DateTime)
						{
							this.SetToken(JsonToken.Date, new DateTimeOffset((DateTime)this.Value), false);
						}
						return new DateTimeOffset?((DateTimeOffset)this.Value);
					}
					if (tokenType == JsonToken.Null)
					{
						return null;
					}
					if (tokenType == JsonToken.String)
					{
						string text = (string)this.Value;
						if (string.IsNullOrEmpty(text))
						{
							this.SetToken(JsonToken.Null);
							return null;
						}
						object obj;
						DateTimeOffset dateTimeOffset;
						if (DateTimeUtils.TryParseDateTime(text, DateParseHandling.DateTimeOffset, this.DateTimeZoneHandling, this._dateFormatString, this.Culture, out obj))
						{
							dateTimeOffset = (DateTimeOffset)obj;
							this.SetToken(JsonToken.Date, dateTimeOffset, false);
							return new DateTimeOffset?(dateTimeOffset);
						}
						if (DateTimeOffset.TryParse(text, this.Culture, DateTimeStyles.RoundtripKind, out dateTimeOffset))
						{
							this.SetToken(JsonToken.Date, dateTimeOffset, false);
							return new DateTimeOffset?(dateTimeOffset);
						}
						throw JsonReaderException.Create(this, "Could not convert string to DateTimeOffset: {0}.".FormatWith(CultureInfo.InvariantCulture, this.Value));
					}
					else
					{
						if (tokenType == JsonToken.EndArray)
						{
							return null;
						}
						throw JsonReaderException.Create(this, "Error reading date. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, tokenType));
					}
				}
			}
			this.SetToken(JsonToken.None);
			return null;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002D70 File Offset: 0x00000F70
		internal byte[] ReadAsBytesInternal()
		{
			this._readType = ReadType.ReadAsBytes;
			while (this.ReadInternal())
			{
				JsonToken tokenType = this.TokenType;
				if (tokenType != JsonToken.Comment)
				{
					if (this.IsWrappedInTypeObject())
					{
						byte[] array = this.ReadAsBytes();
						this.ReadInternal();
						this.SetToken(JsonToken.Bytes, array, false);
						return array;
					}
					if (tokenType == JsonToken.String)
					{
						string text = (string)this.Value;
						byte[] array2;
						Guid guid;
						if (text.Length == 0)
						{
							array2 = new byte[0];
						}
						else if (ConvertUtils.TryConvertGuid(text, out guid))
						{
							array2 = guid.ToByteArray();
						}
						else
						{
							array2 = Convert.FromBase64String(text);
						}
						this.SetToken(JsonToken.Bytes, array2, false);
						return array2;
					}
					if (tokenType == JsonToken.Null)
					{
						return null;
					}
					if (tokenType == JsonToken.Bytes)
					{
						if (this.ValueType == typeof(Guid))
						{
							byte[] array3 = ((Guid)this.Value).ToByteArray();
							this.SetToken(JsonToken.Bytes, array3, false);
							return array3;
						}
						return (byte[])this.Value;
					}
					else
					{
						if (tokenType == JsonToken.StartArray)
						{
							List<byte> list = new List<byte>();
							while (this.ReadInternal())
							{
								tokenType = this.TokenType;
								JsonToken jsonToken = tokenType;
								switch (jsonToken)
								{
								case JsonToken.Comment:
									continue;
								case JsonToken.Raw:
									break;
								case JsonToken.Integer:
									list.Add(Convert.ToByte(this.Value, CultureInfo.InvariantCulture));
									continue;
								default:
									if (jsonToken == JsonToken.EndArray)
									{
										byte[] array4 = list.ToArray();
										this.SetToken(JsonToken.Bytes, array4, false);
										return array4;
									}
									break;
								}
								throw JsonReaderException.Create(this, "Unexpected token when reading bytes: {0}.".FormatWith(CultureInfo.InvariantCulture, tokenType));
							}
							throw JsonReaderException.Create(this, "Unexpected end when reading bytes.");
						}
						if (tokenType == JsonToken.EndArray)
						{
							return null;
						}
						throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, tokenType));
					}
				}
			}
			this.SetToken(JsonToken.None);
			return null;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002F18 File Offset: 0x00001118
		internal decimal? ReadAsDecimalInternal()
		{
			this._readType = ReadType.ReadAsDecimal;
			while (this.ReadInternal())
			{
				JsonToken tokenType = this.TokenType;
				if (tokenType != JsonToken.Comment)
				{
					if (tokenType == JsonToken.Integer || tokenType == JsonToken.Float)
					{
						if (!(this.Value is decimal))
						{
							this.SetToken(JsonToken.Float, Convert.ToDecimal(this.Value, CultureInfo.InvariantCulture), false);
						}
						return new decimal?((decimal)this.Value);
					}
					if (tokenType == JsonToken.Null)
					{
						return null;
					}
					if (tokenType == JsonToken.String)
					{
						string text = (string)this.Value;
						if (string.IsNullOrEmpty(text))
						{
							this.SetToken(JsonToken.Null);
							return null;
						}
						decimal num;
						if (decimal.TryParse(text, NumberStyles.Number, this.Culture, out num))
						{
							this.SetToken(JsonToken.Float, num, false);
							return new decimal?(num);
						}
						throw JsonReaderException.Create(this, "Could not convert string to decimal: {0}.".FormatWith(CultureInfo.InvariantCulture, this.Value));
					}
					else
					{
						if (tokenType == JsonToken.EndArray)
						{
							return null;
						}
						throw JsonReaderException.Create(this, "Error reading decimal. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, tokenType));
					}
				}
			}
			this.SetToken(JsonToken.None);
			return null;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003040 File Offset: 0x00001240
		internal int? ReadAsInt32Internal()
		{
			this._readType = ReadType.ReadAsInt32;
			while (this.ReadInternal())
			{
				JsonToken tokenType = this.TokenType;
				if (tokenType != JsonToken.Comment)
				{
					if (tokenType == JsonToken.Integer || tokenType == JsonToken.Float)
					{
						if (!(this.Value is int))
						{
							this.SetToken(JsonToken.Integer, Convert.ToInt32(this.Value, CultureInfo.InvariantCulture), false);
						}
						return new int?((int)this.Value);
					}
					if (tokenType == JsonToken.Null)
					{
						return null;
					}
					if (tokenType == JsonToken.String)
					{
						string text = (string)this.Value;
						if (string.IsNullOrEmpty(text))
						{
							this.SetToken(JsonToken.Null);
							return null;
						}
						int num;
						if (int.TryParse(text, NumberStyles.Integer, this.Culture, out num))
						{
							this.SetToken(JsonToken.Integer, num, false);
							return new int?(num);
						}
						throw JsonReaderException.Create(this, "Could not convert string to integer: {0}.".FormatWith(CultureInfo.InvariantCulture, this.Value));
					}
					else
					{
						if (tokenType == JsonToken.EndArray)
						{
							return null;
						}
						throw JsonReaderException.Create(this, "Error reading integer. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, this.TokenType));
					}
				}
			}
			this.SetToken(JsonToken.None);
			return null;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x0000316C File Offset: 0x0000136C
		internal string ReadAsStringInternal()
		{
			this._readType = ReadType.ReadAsString;
			while (this.ReadInternal())
			{
				JsonToken tokenType = this.TokenType;
				if (tokenType != JsonToken.Comment)
				{
					if (tokenType == JsonToken.String)
					{
						return (string)this.Value;
					}
					if (tokenType == JsonToken.Null)
					{
						return null;
					}
					if (JsonTokenUtils.IsPrimitiveToken(tokenType) && this.Value != null)
					{
						string text;
						if (this.Value is IFormattable)
						{
							text = ((IFormattable)this.Value).ToString(null, this.Culture);
						}
						else
						{
							text = this.Value.ToString();
						}
						this.SetToken(JsonToken.String, text, false);
						return text;
					}
					if (tokenType == JsonToken.EndArray)
					{
						return null;
					}
					throw JsonReaderException.Create(this, "Error reading string. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, tokenType));
				}
			}
			this.SetToken(JsonToken.None);
			return null;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003228 File Offset: 0x00001428
		internal DateTime? ReadAsDateTimeInternal()
		{
			this._readType = ReadType.ReadAsDateTime;
			while (this.ReadInternal())
			{
				if (this.TokenType != JsonToken.Comment)
				{
					if (this.TokenType == JsonToken.Date)
					{
						return new DateTime?((DateTime)this.Value);
					}
					if (this.TokenType == JsonToken.Null)
					{
						return null;
					}
					if (this.TokenType == JsonToken.String)
					{
						string text = (string)this.Value;
						if (string.IsNullOrEmpty(text))
						{
							this.SetToken(JsonToken.Null);
							return null;
						}
						object obj;
						DateTime dateTime;
						if (DateTimeUtils.TryParseDateTime(text, DateParseHandling.DateTime, this.DateTimeZoneHandling, this._dateFormatString, this.Culture, out obj))
						{
							dateTime = (DateTime)obj;
							dateTime = DateTimeUtils.EnsureDateTime(dateTime, this.DateTimeZoneHandling);
							this.SetToken(JsonToken.Date, dateTime, false);
							return new DateTime?(dateTime);
						}
						if (DateTime.TryParse(text, this.Culture, DateTimeStyles.RoundtripKind, out dateTime))
						{
							dateTime = DateTimeUtils.EnsureDateTime(dateTime, this.DateTimeZoneHandling);
							this.SetToken(JsonToken.Date, dateTime, false);
							return new DateTime?(dateTime);
						}
						throw JsonReaderException.Create(this, "Could not convert string to DateTime: {0}.".FormatWith(CultureInfo.InvariantCulture, this.Value));
					}
					else
					{
						if (this.TokenType == JsonToken.EndArray)
						{
							return null;
						}
						throw JsonReaderException.Create(this, "Error reading date. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, this.TokenType));
					}
				}
			}
			this.SetToken(JsonToken.None);
			return null;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003394 File Offset: 0x00001594
		private bool IsWrappedInTypeObject()
		{
			this._readType = ReadType.Read;
			if (this.TokenType != JsonToken.StartObject)
			{
				return false;
			}
			if (!this.ReadInternal())
			{
				throw JsonReaderException.Create(this, "Unexpected end when reading bytes.");
			}
			if (this.Value.ToString() == "$type")
			{
				this.ReadInternal();
				if (this.Value != null && this.Value.ToString().StartsWith("System.Byte[]", StringComparison.Ordinal))
				{
					this.ReadInternal();
					if (this.Value.ToString() == "$value")
					{
						return true;
					}
				}
			}
			throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, JsonToken.StartObject));
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003444 File Offset: 0x00001644
		public void Skip()
		{
			if (this.TokenType == JsonToken.PropertyName)
			{
				this.Read();
			}
			if (JsonTokenUtils.IsStartToken(this.TokenType))
			{
				int depth = this.Depth;
				while (this.Read() && depth < this.Depth)
				{
				}
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003486 File Offset: 0x00001686
		protected void SetToken(JsonToken newToken)
		{
			this.SetToken(newToken, null, true);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003491 File Offset: 0x00001691
		protected void SetToken(JsonToken newToken, object value)
		{
			this.SetToken(newToken, value, true);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x0000349C File Offset: 0x0000169C
		internal void SetToken(JsonToken newToken, object value, bool updateIndex)
		{
			this._tokenType = newToken;
			this._value = value;
			switch (newToken)
			{
			case JsonToken.StartObject:
				this._currentState = JsonReader.State.ObjectStart;
				this.Push(JsonContainerType.Object);
				return;
			case JsonToken.StartArray:
				this._currentState = JsonReader.State.ArrayStart;
				this.Push(JsonContainerType.Array);
				return;
			case JsonToken.StartConstructor:
				this._currentState = JsonReader.State.ConstructorStart;
				this.Push(JsonContainerType.Constructor);
				return;
			case JsonToken.PropertyName:
				this._currentState = JsonReader.State.Property;
				this._currentPosition.PropertyName = (string)value;
				return;
			case JsonToken.Comment:
				break;
			case JsonToken.Raw:
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Null:
			case JsonToken.Undefined:
			case JsonToken.Date:
			case JsonToken.Bytes:
				this.SetPostValueState(updateIndex);
				break;
			case JsonToken.EndObject:
				this.ValidateEnd(JsonToken.EndObject);
				return;
			case JsonToken.EndArray:
				this.ValidateEnd(JsonToken.EndArray);
				return;
			case JsonToken.EndConstructor:
				this.ValidateEnd(JsonToken.EndConstructor);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000356F File Offset: 0x0000176F
		internal void SetPostValueState(bool updateIndex)
		{
			if (this.Peek() != JsonContainerType.None)
			{
				this._currentState = JsonReader.State.PostValue;
			}
			else
			{
				this.SetFinished();
			}
			if (updateIndex)
			{
				this.UpdateScopeWithFinishedValue();
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003591 File Offset: 0x00001791
		private void UpdateScopeWithFinishedValue()
		{
			if (this._currentPosition.HasIndex)
			{
				this._currentPosition.Position = this._currentPosition.Position + 1;
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000035B4 File Offset: 0x000017B4
		private void ValidateEnd(JsonToken endToken)
		{
			JsonContainerType jsonContainerType = this.Pop();
			if (this.GetTypeForCloseToken(endToken) != jsonContainerType)
			{
				throw JsonReaderException.Create(this, "JsonToken {0} is not valid for closing JsonType {1}.".FormatWith(CultureInfo.InvariantCulture, endToken, jsonContainerType));
			}
			if (this.Peek() != JsonContainerType.None)
			{
				this._currentState = JsonReader.State.PostValue;
				return;
			}
			this.SetFinished();
		}

		// Token: 0x06000044 RID: 68 RVA: 0x0000360C File Offset: 0x0000180C
		protected void SetStateBasedOnCurrent()
		{
			JsonContainerType jsonContainerType = this.Peek();
			switch (jsonContainerType)
			{
			case JsonContainerType.None:
				this.SetFinished();
				return;
			case JsonContainerType.Object:
				this._currentState = JsonReader.State.Object;
				return;
			case JsonContainerType.Array:
				this._currentState = JsonReader.State.Array;
				return;
			case JsonContainerType.Constructor:
				this._currentState = JsonReader.State.Constructor;
				return;
			default:
				throw JsonReaderException.Create(this, "While setting the reader state back to current object an unexpected JsonType was encountered: {0}".FormatWith(CultureInfo.InvariantCulture, jsonContainerType));
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003675 File Offset: 0x00001875
		private void SetFinished()
		{
			if (this.SupportMultipleContent)
			{
				this._currentState = JsonReader.State.Start;
				return;
			}
			this._currentState = JsonReader.State.Finished;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003690 File Offset: 0x00001890
		private JsonContainerType GetTypeForCloseToken(JsonToken token)
		{
			switch (token)
			{
			case JsonToken.EndObject:
				return JsonContainerType.Object;
			case JsonToken.EndArray:
				return JsonContainerType.Array;
			case JsonToken.EndConstructor:
				return JsonContainerType.Constructor;
			default:
				throw JsonReaderException.Create(this, "Not a valid close JsonToken: {0}".FormatWith(CultureInfo.InvariantCulture, token));
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000036D7 File Offset: 0x000018D7
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000036E0 File Offset: 0x000018E0
		protected virtual void Dispose(bool disposing)
		{
			if (this._currentState != JsonReader.State.Closed && disposing)
			{
				this.Close();
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000036F4 File Offset: 0x000018F4
		public virtual void Close()
		{
			this._currentState = JsonReader.State.Closed;
			this._tokenType = JsonToken.None;
			this._value = null;
		}

		// Token: 0x0400000E RID: 14
		private JsonToken _tokenType;

		// Token: 0x0400000F RID: 15
		private object _value;

		// Token: 0x04000010 RID: 16
		internal char _quoteChar;

		// Token: 0x04000011 RID: 17
		internal JsonReader.State _currentState;

		// Token: 0x04000012 RID: 18
		internal ReadType _readType;

		// Token: 0x04000013 RID: 19
		private JsonPosition _currentPosition;

		// Token: 0x04000014 RID: 20
		private CultureInfo _culture;

		// Token: 0x04000015 RID: 21
		private DateTimeZoneHandling _dateTimeZoneHandling;

		// Token: 0x04000016 RID: 22
		private int? _maxDepth;

		// Token: 0x04000017 RID: 23
		private bool _hasExceededMaxDepth;

		// Token: 0x04000018 RID: 24
		internal DateParseHandling _dateParseHandling;

		// Token: 0x04000019 RID: 25
		internal FloatParseHandling _floatParseHandling;

		// Token: 0x0400001A RID: 26
		private string _dateFormatString;

		// Token: 0x0400001B RID: 27
		private readonly List<JsonPosition> _stack;

		// Token: 0x02000006 RID: 6
		protected internal enum State
		{
			// Token: 0x0400001F RID: 31
			Start,
			// Token: 0x04000020 RID: 32
			Complete,
			// Token: 0x04000021 RID: 33
			Property,
			// Token: 0x04000022 RID: 34
			ObjectStart,
			// Token: 0x04000023 RID: 35
			Object,
			// Token: 0x04000024 RID: 36
			ArrayStart,
			// Token: 0x04000025 RID: 37
			Array,
			// Token: 0x04000026 RID: 38
			Closed,
			// Token: 0x04000027 RID: 39
			PostValue,
			// Token: 0x04000028 RID: 40
			ConstructorStart,
			// Token: 0x04000029 RID: 41
			Constructor,
			// Token: 0x0400002A RID: 42
			Error,
			// Token: 0x0400002B RID: 43
			Finished
		}
	}
}
