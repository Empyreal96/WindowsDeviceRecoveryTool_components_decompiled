using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x02000057 RID: 87
	public class JsonTextReader : JsonReader, IJsonLineInfo
	{
		// Token: 0x0600039E RID: 926 RVA: 0x0000C922 File Offset: 0x0000AB22
		public JsonTextReader(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this._reader = reader;
			this._lineNumber = 1;
			this._chars = new char[1025];
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000C956 File Offset: 0x0000AB56
		private StringBuffer GetBuffer()
		{
			if (this._buffer == null)
			{
				this._buffer = new StringBuffer(1025);
			}
			else
			{
				this._buffer.Position = 0;
			}
			return this._buffer;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000C984 File Offset: 0x0000AB84
		private void OnNewLine(int pos)
		{
			this._lineNumber++;
			this._lineStartPos = pos - 1;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000C9A0 File Offset: 0x0000ABA0
		private void ParseString(char quote)
		{
			this._charPos++;
			this.ShiftBufferIfNeeded();
			this.ReadStringIntoBuffer(quote);
			base.SetPostValueState(true);
			if (this._readType == ReadType.ReadAsBytes)
			{
				byte[] value;
				if (this._stringReference.Length == 0)
				{
					value = new byte[0];
				}
				else
				{
					value = Convert.FromBase64CharArray(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length);
				}
				base.SetToken(JsonToken.Bytes, value, false);
				return;
			}
			if (this._readType == ReadType.ReadAsString)
			{
				string value2 = this._stringReference.ToString();
				base.SetToken(JsonToken.String, value2, false);
				this._quoteChar = quote;
				return;
			}
			string text = this._stringReference.ToString();
			if (this._dateParseHandling != DateParseHandling.None)
			{
				DateParseHandling dateParseHandling;
				if (this._readType == ReadType.ReadAsDateTime)
				{
					dateParseHandling = DateParseHandling.DateTime;
				}
				else if (this._readType == ReadType.ReadAsDateTimeOffset)
				{
					dateParseHandling = DateParseHandling.DateTimeOffset;
				}
				else
				{
					dateParseHandling = this._dateParseHandling;
				}
				object value3;
				if (DateTimeUtils.TryParseDateTime(text, dateParseHandling, base.DateTimeZoneHandling, base.DateFormatString, base.Culture, out value3))
				{
					base.SetToken(JsonToken.Date, value3, false);
					return;
				}
			}
			base.SetToken(JsonToken.String, text, false);
			this._quoteChar = quote;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000CAC2 File Offset: 0x0000ACC2
		private static void BlockCopyChars(char[] src, int srcOffset, char[] dst, int dstOffset, int count)
		{
			Buffer.BlockCopy(src, srcOffset * 2, dst, dstOffset * 2, count * 2);
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000CAD8 File Offset: 0x0000ACD8
		private void ShiftBufferIfNeeded()
		{
			int num = this._chars.Length;
			if ((double)(num - this._charPos) <= (double)num * 0.1)
			{
				int num2 = this._charsUsed - this._charPos;
				if (num2 > 0)
				{
					JsonTextReader.BlockCopyChars(this._chars, this._charPos, this._chars, 0, num2);
				}
				this._lineStartPos -= this._charPos;
				this._charPos = 0;
				this._charsUsed = num2;
				this._chars[this._charsUsed] = '\0';
			}
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000CB5F File Offset: 0x0000AD5F
		private int ReadData(bool append)
		{
			return this.ReadData(append, 0);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000CB6C File Offset: 0x0000AD6C
		private int ReadData(bool append, int charsRequired)
		{
			if (this._isEndOfFile)
			{
				return 0;
			}
			if (this._charsUsed + charsRequired >= this._chars.Length - 1)
			{
				if (append)
				{
					int num = Math.Max(this._chars.Length * 2, this._charsUsed + charsRequired + 1);
					char[] array = new char[num];
					JsonTextReader.BlockCopyChars(this._chars, 0, array, 0, this._chars.Length);
					this._chars = array;
				}
				else
				{
					int num2 = this._charsUsed - this._charPos;
					if (num2 + charsRequired + 1 >= this._chars.Length)
					{
						char[] array2 = new char[num2 + charsRequired + 1];
						if (num2 > 0)
						{
							JsonTextReader.BlockCopyChars(this._chars, this._charPos, array2, 0, num2);
						}
						this._chars = array2;
					}
					else if (num2 > 0)
					{
						JsonTextReader.BlockCopyChars(this._chars, this._charPos, this._chars, 0, num2);
					}
					this._lineStartPos -= this._charPos;
					this._charPos = 0;
					this._charsUsed = num2;
				}
			}
			int count = this._chars.Length - this._charsUsed - 1;
			int num3 = this._reader.Read(this._chars, this._charsUsed, count);
			this._charsUsed += num3;
			if (num3 == 0)
			{
				this._isEndOfFile = true;
			}
			this._chars[this._charsUsed] = '\0';
			return num3;
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000CCBF File Offset: 0x0000AEBF
		private bool EnsureChars(int relativePosition, bool append)
		{
			return this._charPos + relativePosition < this._charsUsed || this.ReadChars(relativePosition, append);
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000CCDC File Offset: 0x0000AEDC
		private bool ReadChars(int relativePosition, bool append)
		{
			if (this._isEndOfFile)
			{
				return false;
			}
			int num = this._charPos + relativePosition - this._charsUsed + 1;
			int num2 = 0;
			do
			{
				int num3 = this.ReadData(append, num - num2);
				if (num3 == 0)
				{
					break;
				}
				num2 += num3;
			}
			while (num2 < num);
			return num2 >= num;
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000CD24 File Offset: 0x0000AF24
		[DebuggerStepThrough]
		public override bool Read()
		{
			this._readType = ReadType.Read;
			if (!this.ReadInternal())
			{
				base.SetToken(JsonToken.None);
				return false;
			}
			return true;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000CD3F File Offset: 0x0000AF3F
		public override byte[] ReadAsBytes()
		{
			return base.ReadAsBytesInternal();
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000CD47 File Offset: 0x0000AF47
		public override decimal? ReadAsDecimal()
		{
			return base.ReadAsDecimalInternal();
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000CD4F File Offset: 0x0000AF4F
		public override int? ReadAsInt32()
		{
			return base.ReadAsInt32Internal();
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000CD57 File Offset: 0x0000AF57
		public override string ReadAsString()
		{
			return base.ReadAsStringInternal();
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000CD5F File Offset: 0x0000AF5F
		public override DateTime? ReadAsDateTime()
		{
			return base.ReadAsDateTimeInternal();
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000CD67 File Offset: 0x0000AF67
		public override DateTimeOffset? ReadAsDateTimeOffset()
		{
			return base.ReadAsDateTimeOffsetInternal();
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000CD70 File Offset: 0x0000AF70
		internal override bool ReadInternal()
		{
			for (;;)
			{
				switch (this._currentState)
				{
				case JsonReader.State.Start:
				case JsonReader.State.Property:
				case JsonReader.State.ArrayStart:
				case JsonReader.State.Array:
				case JsonReader.State.ConstructorStart:
				case JsonReader.State.Constructor:
					goto IL_43;
				case JsonReader.State.ObjectStart:
				case JsonReader.State.Object:
					goto IL_4A;
				case JsonReader.State.PostValue:
					if (this.ParsePostValue())
					{
						return true;
					}
					continue;
				case JsonReader.State.Finished:
					goto IL_5B;
				}
				break;
			}
			goto IL_BA;
			IL_43:
			return this.ParseValue();
			IL_4A:
			return this.ParseObject();
			IL_5B:
			if (!this.EnsureChars(0, false))
			{
				return false;
			}
			this.EatWhitespace(false);
			if (this._isEndOfFile)
			{
				return false;
			}
			if (this._chars[this._charPos] == '/')
			{
				this.ParseComment();
				return true;
			}
			throw JsonReaderException.Create(this, "Additional text encountered after finished reading JSON content: {0}.".FormatWith(CultureInfo.InvariantCulture, this._chars[this._charPos]));
			IL_BA:
			throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, base.CurrentState));
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000CE58 File Offset: 0x0000B058
		private void ReadStringIntoBuffer(char quote)
		{
			int num = this._charPos;
			int charPos = this._charPos;
			int num2 = this._charPos;
			StringBuffer stringBuffer = null;
			char c2;
			for (;;)
			{
				char c = this._chars[num++];
				if (c <= '\r')
				{
					if (c != '\0')
					{
						if (c != '\n')
						{
							if (c == '\r')
							{
								this._charPos = num - 1;
								this.ProcessCarriageReturn(true);
								num = this._charPos;
							}
						}
						else
						{
							this._charPos = num - 1;
							this.ProcessLineFeed();
							num = this._charPos;
						}
					}
					else if (this._charsUsed == num - 1)
					{
						num--;
						if (this.ReadData(true) == 0)
						{
							break;
						}
					}
				}
				else if (c != '"' && c != '\'')
				{
					if (c == '\\')
					{
						this._charPos = num;
						if (!this.EnsureChars(0, true))
						{
							goto Block_10;
						}
						int writeToPosition = num - 1;
						c2 = this._chars[num];
						char c3 = c2;
						char c4;
						if (c3 <= '\\')
						{
							if (c3 <= '\'')
							{
								if (c3 != '"' && c3 != '\'')
								{
									goto Block_14;
								}
							}
							else if (c3 != '/')
							{
								if (c3 != '\\')
								{
									goto Block_16;
								}
								num++;
								c4 = '\\';
								goto IL_2BF;
							}
							c4 = c2;
							num++;
						}
						else if (c3 <= 'f')
						{
							if (c3 != 'b')
							{
								if (c3 != 'f')
								{
									goto Block_19;
								}
								num++;
								c4 = '\f';
							}
							else
							{
								num++;
								c4 = '\b';
							}
						}
						else
						{
							if (c3 != 'n')
							{
								switch (c3)
								{
								case 'r':
									num++;
									c4 = '\r';
									goto IL_2BF;
								case 't':
									num++;
									c4 = '\t';
									goto IL_2BF;
								case 'u':
									num++;
									this._charPos = num;
									c4 = this.ParseUnicode();
									if (StringUtils.IsLowSurrogate(c4))
									{
										c4 = '�';
									}
									else if (StringUtils.IsHighSurrogate(c4))
									{
										bool flag;
										do
										{
											flag = false;
											if (this.EnsureChars(2, true) && this._chars[this._charPos] == '\\' && this._chars[this._charPos + 1] == 'u')
											{
												char writeChar = c4;
												this._charPos += 2;
												c4 = this.ParseUnicode();
												if (!StringUtils.IsLowSurrogate(c4))
												{
													if (StringUtils.IsHighSurrogate(c4))
													{
														writeChar = '�';
														flag = true;
													}
													else
													{
														writeChar = '�';
													}
												}
												if (stringBuffer == null)
												{
													stringBuffer = this.GetBuffer();
												}
												this.WriteCharToBuffer(stringBuffer, writeChar, num2, writeToPosition);
												num2 = this._charPos;
											}
											else
											{
												c4 = '�';
											}
										}
										while (flag);
									}
									num = this._charPos;
									goto IL_2BF;
								}
								goto Block_21;
							}
							num++;
							c4 = '\n';
						}
						IL_2BF:
						if (stringBuffer == null)
						{
							stringBuffer = this.GetBuffer();
						}
						this.WriteCharToBuffer(stringBuffer, c4, num2, writeToPosition);
						num2 = num;
					}
				}
				else if (this._chars[num - 1] == quote)
				{
					goto Block_30;
				}
			}
			this._charPos = num;
			throw JsonReaderException.Create(this, "Unterminated string. Expected delimiter: {0}.".FormatWith(CultureInfo.InvariantCulture, quote));
			Block_10:
			this._charPos = num;
			throw JsonReaderException.Create(this, "Unterminated string. Expected delimiter: {0}.".FormatWith(CultureInfo.InvariantCulture, quote));
			Block_14:
			Block_16:
			Block_19:
			Block_21:
			num++;
			this._charPos = num;
			throw JsonReaderException.Create(this, "Bad JSON escape sequence: {0}.".FormatWith(CultureInfo.InvariantCulture, "\\" + c2));
			Block_30:
			num--;
			if (charPos == num2)
			{
				this._stringReference = new StringReference(this._chars, charPos, num - charPos);
			}
			else
			{
				if (stringBuffer == null)
				{
					stringBuffer = this.GetBuffer();
				}
				if (num > num2)
				{
					stringBuffer.Append(this._chars, num2, num - num2);
				}
				this._stringReference = new StringReference(stringBuffer.GetInternalBuffer(), 0, stringBuffer.Position);
			}
			num++;
			this._charPos = num;
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000D1E8 File Offset: 0x0000B3E8
		private void WriteCharToBuffer(StringBuffer buffer, char writeChar, int lastWritePosition, int writeToPosition)
		{
			if (writeToPosition > lastWritePosition)
			{
				buffer.Append(this._chars, lastWritePosition, writeToPosition - lastWritePosition);
			}
			buffer.Append(writeChar);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000D208 File Offset: 0x0000B408
		private char ParseUnicode()
		{
			if (this.EnsureChars(4, true))
			{
				string s = new string(this._chars, this._charPos, 4);
				char c = Convert.ToChar(int.Parse(s, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo));
				char result = c;
				this._charPos += 4;
				return result;
			}
			throw JsonReaderException.Create(this, "Unexpected end while parsing unicode character.");
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000D268 File Offset: 0x0000B468
		private void ReadNumberIntoBuffer()
		{
			int num = this._charPos;
			for (;;)
			{
				char c = this._chars[num];
				if (c <= 'F')
				{
					if (c != '\0')
					{
						switch (c)
						{
						case '+':
						case '-':
						case '.':
						case '0':
						case '1':
						case '2':
						case '3':
						case '4':
						case '5':
						case '6':
						case '7':
						case '8':
						case '9':
						case 'A':
						case 'B':
						case 'C':
						case 'D':
						case 'E':
						case 'F':
							goto IL_E4;
						}
						break;
					}
					this._charPos = num;
					if (this._charsUsed != num || this.ReadData(true) == 0)
					{
						return;
					}
					continue;
				}
				else if (c != 'X')
				{
					switch (c)
					{
					case 'a':
					case 'b':
					case 'c':
					case 'd':
					case 'e':
					case 'f':
						break;
					default:
						if (c != 'x')
						{
							goto Block_6;
						}
						break;
					}
				}
				IL_E4:
				num++;
			}
			Block_6:
			this._charPos = num;
			char c2 = this._chars[this._charPos];
			if (char.IsWhiteSpace(c2) || c2 == ',' || c2 == '}' || c2 == ']' || c2 == ')' || c2 == '/')
			{
				return;
			}
			throw JsonReaderException.Create(this, "Unexpected character encountered while parsing number: {0}.".FormatWith(CultureInfo.InvariantCulture, c2));
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0000D3B4 File Offset: 0x0000B5B4
		private void ClearRecentString()
		{
			if (this._buffer != null)
			{
				this._buffer.Position = 0;
			}
			this._stringReference = default(StringReference);
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0000D3D8 File Offset: 0x0000B5D8
		private bool ParsePostValue()
		{
			char c;
			for (;;)
			{
				c = this._chars[this._charPos];
				char c2 = c;
				if (c2 <= ')')
				{
					if (c2 <= '\r')
					{
						if (c2 != '\0')
						{
							switch (c2)
							{
							case '\t':
								break;
							case '\n':
								this.ProcessLineFeed();
								continue;
							case '\v':
							case '\f':
								goto IL_145;
							case '\r':
								this.ProcessCarriageReturn(false);
								continue;
							default:
								goto IL_145;
							}
						}
						else
						{
							if (this._charsUsed != this._charPos)
							{
								this._charPos++;
								continue;
							}
							if (this.ReadData(false) == 0)
							{
								break;
							}
							continue;
						}
					}
					else if (c2 != ' ')
					{
						if (c2 != ')')
						{
							goto IL_145;
						}
						goto IL_E5;
					}
					this._charPos++;
					continue;
				}
				if (c2 <= '/')
				{
					if (c2 == ',')
					{
						goto IL_105;
					}
					if (c2 == '/')
					{
						goto IL_FD;
					}
				}
				else
				{
					if (c2 == ']')
					{
						goto IL_CD;
					}
					if (c2 == '}')
					{
						goto IL_B5;
					}
				}
				IL_145:
				if (!char.IsWhiteSpace(c))
				{
					goto IL_160;
				}
				this._charPos++;
			}
			this._currentState = JsonReader.State.Finished;
			return false;
			IL_B5:
			this._charPos++;
			base.SetToken(JsonToken.EndObject);
			return true;
			IL_CD:
			this._charPos++;
			base.SetToken(JsonToken.EndArray);
			return true;
			IL_E5:
			this._charPos++;
			base.SetToken(JsonToken.EndConstructor);
			return true;
			IL_FD:
			this.ParseComment();
			return true;
			IL_105:
			this._charPos++;
			base.SetStateBasedOnCurrent();
			return false;
			IL_160:
			throw JsonReaderException.Create(this, "After parsing a value an unexpected character was encountered: {0}.".FormatWith(CultureInfo.InvariantCulture, c));
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000D560 File Offset: 0x0000B760
		private bool ParseObject()
		{
			for (;;)
			{
				char c = this._chars[this._charPos];
				char c2 = c;
				if (c2 <= '\r')
				{
					if (c2 != '\0')
					{
						switch (c2)
						{
						case '\t':
							break;
						case '\n':
							this.ProcessLineFeed();
							continue;
						case '\v':
						case '\f':
							goto IL_BF;
						case '\r':
							this.ProcessCarriageReturn(false);
							continue;
						default:
							goto IL_BF;
						}
					}
					else
					{
						if (this._charsUsed != this._charPos)
						{
							this._charPos++;
							continue;
						}
						if (this.ReadData(false) == 0)
						{
							break;
						}
						continue;
					}
				}
				else if (c2 != ' ')
				{
					if (c2 == '/')
					{
						goto IL_8D;
					}
					if (c2 != '}')
					{
						goto IL_BF;
					}
					goto IL_75;
				}
				this._charPos++;
				continue;
				IL_BF:
				if (!char.IsWhiteSpace(c))
				{
					goto IL_DA;
				}
				this._charPos++;
			}
			return false;
			IL_75:
			base.SetToken(JsonToken.EndObject);
			this._charPos++;
			return true;
			IL_8D:
			this.ParseComment();
			return true;
			IL_DA:
			return this.ParseProperty();
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000D650 File Offset: 0x0000B850
		private bool ParseProperty()
		{
			char c = this._chars[this._charPos];
			char c2;
			if (c == '"' || c == '\'')
			{
				this._charPos++;
				c2 = c;
				this.ShiftBufferIfNeeded();
				this.ReadStringIntoBuffer(c2);
			}
			else
			{
				if (!this.ValidIdentifierChar(c))
				{
					throw JsonReaderException.Create(this, "Invalid property identifier character: {0}.".FormatWith(CultureInfo.InvariantCulture, this._chars[this._charPos]));
				}
				c2 = '\0';
				this.ShiftBufferIfNeeded();
				this.ParseUnquotedProperty();
			}
			string text;
			if (this.NameTable != null)
			{
				text = this.NameTable.Get(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length);
				if (text == null)
				{
					text = this._stringReference.ToString();
				}
			}
			else
			{
				text = this._stringReference.ToString();
			}
			this.EatWhitespace(false);
			if (this._chars[this._charPos] != ':')
			{
				throw JsonReaderException.Create(this, "Invalid character after parsing property name. Expected ':' but got: {0}.".FormatWith(CultureInfo.InvariantCulture, this._chars[this._charPos]));
			}
			this._charPos++;
			base.SetToken(JsonToken.PropertyName, text);
			this._quoteChar = c2;
			this.ClearRecentString();
			return true;
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0000D798 File Offset: 0x0000B998
		private bool ValidIdentifierChar(char value)
		{
			return char.IsLetterOrDigit(value) || value == '_' || value == '$';
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000D7B0 File Offset: 0x0000B9B0
		private void ParseUnquotedProperty()
		{
			int charPos = this._charPos;
			char c2;
			for (;;)
			{
				char c = this._chars[this._charPos];
				if (c == '\0')
				{
					if (this._charsUsed != this._charPos)
					{
						goto IL_3C;
					}
					if (this.ReadData(true) == 0)
					{
						break;
					}
				}
				else
				{
					c2 = this._chars[this._charPos];
					if (!this.ValidIdentifierChar(c2))
					{
						goto IL_7E;
					}
					this._charPos++;
				}
			}
			throw JsonReaderException.Create(this, "Unexpected end while parsing unquoted property name.");
			IL_3C:
			this._stringReference = new StringReference(this._chars, charPos, this._charPos - charPos);
			return;
			IL_7E:
			if (char.IsWhiteSpace(c2) || c2 == ':')
			{
				this._stringReference = new StringReference(this._chars, charPos, this._charPos - charPos);
				return;
			}
			throw JsonReaderException.Create(this, "Invalid JavaScript property identifier character: {0}.".FormatWith(CultureInfo.InvariantCulture, c2));
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000D880 File Offset: 0x0000BA80
		private bool ParseValue()
		{
			char c;
			for (;;)
			{
				c = this._chars[this._charPos];
				char c2 = c;
				if (c2 <= 'I')
				{
					if (c2 <= '\r')
					{
						if (c2 != '\0')
						{
							switch (c2)
							{
							case '\t':
								break;
							case '\n':
								this.ProcessLineFeed();
								continue;
							case '\v':
							case '\f':
								goto IL_272;
							case '\r':
								this.ProcessCarriageReturn(false);
								continue;
							default:
								goto IL_272;
							}
						}
						else
						{
							if (this._charsUsed != this._charPos)
							{
								this._charPos++;
								continue;
							}
							if (this.ReadData(false) == 0)
							{
								break;
							}
							continue;
						}
					}
					else
					{
						switch (c2)
						{
						case ' ':
							break;
						case '!':
							goto IL_272;
						case '"':
							goto IL_110;
						default:
							switch (c2)
							{
							case '\'':
								goto IL_110;
							case '(':
							case '*':
							case '+':
							case '.':
								goto IL_272;
							case ')':
								goto IL_230;
							case ',':
								goto IL_226;
							case '-':
								goto IL_1A3;
							case '/':
								goto IL_1D0;
							default:
								if (c2 != 'I')
								{
									goto IL_272;
								}
								goto IL_19B;
							}
							break;
						}
					}
					this._charPos++;
					continue;
				}
				if (c2 <= 'f')
				{
					if (c2 == 'N')
					{
						goto IL_193;
					}
					switch (c2)
					{
					case '[':
						goto IL_1F7;
					case '\\':
						break;
					case ']':
						goto IL_20E;
					default:
						if (c2 == 'f')
						{
							goto IL_121;
						}
						break;
					}
				}
				else
				{
					if (c2 == 'n')
					{
						goto IL_129;
					}
					switch (c2)
					{
					case 't':
						goto IL_119;
					case 'u':
						goto IL_1D8;
					default:
						if (c2 == '{')
						{
							goto IL_1E0;
						}
						break;
					}
				}
				IL_272:
				if (!char.IsWhiteSpace(c))
				{
					goto IL_28D;
				}
				this._charPos++;
			}
			return false;
			IL_110:
			this.ParseString(c);
			return true;
			IL_119:
			this.ParseTrue();
			return true;
			IL_121:
			this.ParseFalse();
			return true;
			IL_129:
			if (this.EnsureChars(1, true))
			{
				char c3 = this._chars[this._charPos + 1];
				if (c3 == 'u')
				{
					this.ParseNull();
				}
				else
				{
					if (c3 != 'e')
					{
						throw JsonReaderException.Create(this, "Unexpected character encountered while parsing value: {0}.".FormatWith(CultureInfo.InvariantCulture, this._chars[this._charPos]));
					}
					this.ParseConstructor();
				}
				return true;
			}
			throw JsonReaderException.Create(this, "Unexpected end.");
			IL_193:
			this.ParseNumberNaN();
			return true;
			IL_19B:
			this.ParseNumberPositiveInfinity();
			return true;
			IL_1A3:
			if (this.EnsureChars(1, true) && this._chars[this._charPos + 1] == 'I')
			{
				this.ParseNumberNegativeInfinity();
			}
			else
			{
				this.ParseNumber();
			}
			return true;
			IL_1D0:
			this.ParseComment();
			return true;
			IL_1D8:
			this.ParseUndefined();
			return true;
			IL_1E0:
			this._charPos++;
			base.SetToken(JsonToken.StartObject);
			return true;
			IL_1F7:
			this._charPos++;
			base.SetToken(JsonToken.StartArray);
			return true;
			IL_20E:
			this._charPos++;
			base.SetToken(JsonToken.EndArray);
			return true;
			IL_226:
			base.SetToken(JsonToken.Undefined);
			return true;
			IL_230:
			this._charPos++;
			base.SetToken(JsonToken.EndConstructor);
			return true;
			IL_28D:
			if (char.IsNumber(c) || c == '-' || c == '.')
			{
				this.ParseNumber();
				return true;
			}
			throw JsonReaderException.Create(this, "Unexpected character encountered while parsing value: {0}.".FormatWith(CultureInfo.InvariantCulture, c));
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000DB4F File Offset: 0x0000BD4F
		private void ProcessLineFeed()
		{
			this._charPos++;
			this.OnNewLine(this._charPos);
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000DB6C File Offset: 0x0000BD6C
		private void ProcessCarriageReturn(bool append)
		{
			this._charPos++;
			if (this.EnsureChars(1, append) && this._chars[this._charPos] == '\n')
			{
				this._charPos++;
			}
			this.OnNewLine(this._charPos);
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000DBBC File Offset: 0x0000BDBC
		private bool EatWhitespace(bool oneOrMore)
		{
			bool flag = false;
			bool flag2 = false;
			while (!flag)
			{
				char c = this._chars[this._charPos];
				char c2 = c;
				if (c2 != '\0')
				{
					if (c2 != '\n')
					{
						if (c2 != '\r')
						{
							if (c == ' ' || char.IsWhiteSpace(c))
							{
								flag2 = true;
								this._charPos++;
							}
							else
							{
								flag = true;
							}
						}
						else
						{
							this.ProcessCarriageReturn(false);
						}
					}
					else
					{
						this.ProcessLineFeed();
					}
				}
				else if (this._charsUsed == this._charPos)
				{
					if (this.ReadData(false) == 0)
					{
						flag = true;
					}
				}
				else
				{
					this._charPos++;
				}
			}
			return !oneOrMore || flag2;
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000DC58 File Offset: 0x0000BE58
		private void ParseConstructor()
		{
			if (!this.MatchValueWithTrailingSeparator("new"))
			{
				throw JsonReaderException.Create(this, "Unexpected content while parsing JSON.");
			}
			this.EatWhitespace(false);
			int charPos = this._charPos;
			char c;
			for (;;)
			{
				c = this._chars[this._charPos];
				if (c == '\0')
				{
					if (this._charsUsed != this._charPos)
					{
						goto IL_53;
					}
					if (this.ReadData(true) == 0)
					{
						break;
					}
				}
				else
				{
					if (!char.IsLetterOrDigit(c))
					{
						goto IL_85;
					}
					this._charPos++;
				}
			}
			throw JsonReaderException.Create(this, "Unexpected end while parsing constructor.");
			IL_53:
			int charPos2 = this._charPos;
			this._charPos++;
			goto IL_F7;
			IL_85:
			if (c == '\r')
			{
				charPos2 = this._charPos;
				this.ProcessCarriageReturn(true);
			}
			else if (c == '\n')
			{
				charPos2 = this._charPos;
				this.ProcessLineFeed();
			}
			else if (char.IsWhiteSpace(c))
			{
				charPos2 = this._charPos;
				this._charPos++;
			}
			else
			{
				if (c != '(')
				{
					throw JsonReaderException.Create(this, "Unexpected character while parsing constructor: {0}.".FormatWith(CultureInfo.InvariantCulture, c));
				}
				charPos2 = this._charPos;
			}
			IL_F7:
			this._stringReference = new StringReference(this._chars, charPos, charPos2 - charPos);
			string value = this._stringReference.ToString();
			this.EatWhitespace(false);
			if (this._chars[this._charPos] != '(')
			{
				throw JsonReaderException.Create(this, "Unexpected character while parsing constructor: {0}.".FormatWith(CultureInfo.InvariantCulture, this._chars[this._charPos]));
			}
			this._charPos++;
			this.ClearRecentString();
			base.SetToken(JsonToken.StartConstructor, value);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000DDEC File Offset: 0x0000BFEC
		private void ParseNumber()
		{
			this.ShiftBufferIfNeeded();
			char c = this._chars[this._charPos];
			int charPos = this._charPos;
			this.ReadNumberIntoBuffer();
			base.SetPostValueState(true);
			this._stringReference = new StringReference(this._chars, charPos, this._charPos - charPos);
			bool flag = char.IsDigit(c) && this._stringReference.Length == 1;
			bool flag2 = c == '0' && this._stringReference.Length > 1 && this._stringReference.Chars[this._stringReference.StartIndex + 1] != '.' && this._stringReference.Chars[this._stringReference.StartIndex + 1] != 'e' && this._stringReference.Chars[this._stringReference.StartIndex + 1] != 'E';
			object value;
			JsonToken newToken;
			if (this._readType == ReadType.ReadAsInt32)
			{
				if (flag)
				{
					value = (int)(c - '0');
				}
				else
				{
					if (flag2)
					{
						string text = this._stringReference.ToString();
						try
						{
							int num = text.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt32(text, 16) : Convert.ToInt32(text, 8);
							value = num;
							goto IL_1DE;
						}
						catch (Exception ex)
						{
							throw JsonReaderException.Create(this, "Input string '{0}' is not a valid integer.".FormatWith(CultureInfo.InvariantCulture, text), ex);
						}
					}
					int num2;
					ParseResult parseResult = ConvertUtils.Int32TryParse(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length, out num2);
					if (parseResult == ParseResult.Success)
					{
						value = num2;
					}
					else
					{
						if (parseResult == ParseResult.Overflow)
						{
							throw JsonReaderException.Create(this, "JSON integer {0} is too large or small for an Int32.".FormatWith(CultureInfo.InvariantCulture, this._stringReference.ToString()));
						}
						throw JsonReaderException.Create(this, "Input string '{0}' is not a valid integer.".FormatWith(CultureInfo.InvariantCulture, this._stringReference.ToString()));
					}
				}
				IL_1DE:
				newToken = JsonToken.Integer;
			}
			else if (this._readType == ReadType.ReadAsDecimal)
			{
				if (flag)
				{
					value = c - 48m;
				}
				else
				{
					if (flag2)
					{
						string text2 = this._stringReference.ToString();
						try
						{
							long value2 = text2.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(text2, 16) : Convert.ToInt64(text2, 8);
							value = Convert.ToDecimal(value2);
							goto IL_2D1;
						}
						catch (Exception ex2)
						{
							throw JsonReaderException.Create(this, "Input string '{0}' is not a valid decimal.".FormatWith(CultureInfo.InvariantCulture, text2), ex2);
						}
					}
					string s = this._stringReference.ToString();
					decimal num3;
					if (!decimal.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out num3))
					{
						throw JsonReaderException.Create(this, "Input string '{0}' is not a valid decimal.".FormatWith(CultureInfo.InvariantCulture, this._stringReference.ToString()));
					}
					value = num3;
				}
				IL_2D1:
				newToken = JsonToken.Float;
			}
			else if (flag)
			{
				value = (long)((ulong)c - 48UL);
				newToken = JsonToken.Integer;
			}
			else if (flag2)
			{
				string text3 = this._stringReference.ToString();
				try
				{
					value = (text3.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(text3, 16) : Convert.ToInt64(text3, 8));
				}
				catch (Exception ex3)
				{
					throw JsonReaderException.Create(this, "Input string '{0}' is not a valid number.".FormatWith(CultureInfo.InvariantCulture, text3), ex3);
				}
				newToken = JsonToken.Integer;
			}
			else
			{
				long num4;
				ParseResult parseResult2 = ConvertUtils.Int64TryParse(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length, out num4);
				if (parseResult2 == ParseResult.Success)
				{
					value = num4;
					newToken = JsonToken.Integer;
				}
				else if (parseResult2 == ParseResult.Overflow)
				{
					string text4 = this._stringReference.ToString();
					if (text4.Length > 380)
					{
						throw JsonReaderException.Create(this, "JSON integer {0} is too large to parse.".FormatWith(CultureInfo.InvariantCulture, this._stringReference.ToString()));
					}
					value = JsonTextReader.BigIntegerParse(text4, CultureInfo.InvariantCulture);
					newToken = JsonToken.Integer;
				}
				else
				{
					string text5 = this._stringReference.ToString();
					if (this._floatParseHandling == FloatParseHandling.Decimal)
					{
						decimal num5;
						if (!decimal.TryParse(text5, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out num5))
						{
							throw JsonReaderException.Create(this, "Input string '{0}' is not a valid decimal.".FormatWith(CultureInfo.InvariantCulture, text5));
						}
						value = num5;
					}
					else
					{
						double num6;
						if (!double.TryParse(text5, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out num6))
						{
							throw JsonReaderException.Create(this, "Input string '{0}' is not a valid number.".FormatWith(CultureInfo.InvariantCulture, text5));
						}
						value = num6;
					}
					newToken = JsonToken.Float;
				}
			}
			this.ClearRecentString();
			base.SetToken(newToken, value, false);
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000E2B0 File Offset: 0x0000C4B0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static object BigIntegerParse(string number, CultureInfo culture)
		{
			return BigInteger.Parse(number, culture);
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000E2C0 File Offset: 0x0000C4C0
		private void ParseComment()
		{
			this._charPos++;
			if (!this.EnsureChars(1, false))
			{
				throw JsonReaderException.Create(this, "Unexpected end while parsing comment.");
			}
			bool flag;
			if (this._chars[this._charPos] == '*')
			{
				flag = false;
			}
			else
			{
				if (this._chars[this._charPos] != '/')
				{
					throw JsonReaderException.Create(this, "Error parsing comment. Expected: *, got {0}.".FormatWith(CultureInfo.InvariantCulture, this._chars[this._charPos]));
				}
				flag = true;
			}
			this._charPos++;
			int charPos = this._charPos;
			bool flag2 = false;
			while (!flag2)
			{
				char c = this._chars[this._charPos];
				if (c <= '\n')
				{
					if (c != '\0')
					{
						if (c == '\n')
						{
							if (flag)
							{
								this._stringReference = new StringReference(this._chars, charPos, this._charPos - charPos);
								flag2 = true;
							}
							this.ProcessLineFeed();
							continue;
						}
					}
					else
					{
						if (this._charsUsed != this._charPos)
						{
							this._charPos++;
							continue;
						}
						if (this.ReadData(true) != 0)
						{
							continue;
						}
						if (!flag)
						{
							throw JsonReaderException.Create(this, "Unexpected end while parsing comment.");
						}
						this._stringReference = new StringReference(this._chars, charPos, this._charPos - charPos);
						flag2 = true;
						continue;
					}
				}
				else
				{
					if (c == '\r')
					{
						if (flag)
						{
							this._stringReference = new StringReference(this._chars, charPos, this._charPos - charPos);
							flag2 = true;
						}
						this.ProcessCarriageReturn(true);
						continue;
					}
					if (c == '*')
					{
						this._charPos++;
						if (!flag && this.EnsureChars(0, true) && this._chars[this._charPos] == '/')
						{
							this._stringReference = new StringReference(this._chars, charPos, this._charPos - charPos - 1);
							this._charPos++;
							flag2 = true;
							continue;
						}
						continue;
					}
				}
				this._charPos++;
			}
			base.SetToken(JsonToken.Comment, this._stringReference.ToString());
			this.ClearRecentString();
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000E4D8 File Offset: 0x0000C6D8
		private bool MatchValue(string value)
		{
			if (!this.EnsureChars(value.Length - 1, true))
			{
				return false;
			}
			for (int i = 0; i < value.Length; i++)
			{
				if (this._chars[this._charPos + i] != value[i])
				{
					return false;
				}
			}
			this._charPos += value.Length;
			return true;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000E538 File Offset: 0x0000C738
		private bool MatchValueWithTrailingSeparator(string value)
		{
			return this.MatchValue(value) && (!this.EnsureChars(0, false) || this.IsSeparator(this._chars[this._charPos]) || this._chars[this._charPos] == '\0');
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000E588 File Offset: 0x0000C788
		private bool IsSeparator(char c)
		{
			if (c <= ')')
			{
				switch (c)
				{
				case '\t':
				case '\n':
				case '\r':
					break;
				case '\v':
				case '\f':
					goto IL_8E;
				default:
					if (c != ' ')
					{
						if (c != ')')
						{
							goto IL_8E;
						}
						if (base.CurrentState == JsonReader.State.Constructor || base.CurrentState == JsonReader.State.ConstructorStart)
						{
							return true;
						}
						return false;
					}
					break;
				}
				return true;
			}
			if (c <= '/')
			{
				if (c != ',')
				{
					if (c != '/')
					{
						goto IL_8E;
					}
					if (!this.EnsureChars(1, false))
					{
						return false;
					}
					char c2 = this._chars[this._charPos + 1];
					return c2 == '*' || c2 == '/';
				}
			}
			else if (c != ']' && c != '}')
			{
				goto IL_8E;
			}
			return true;
			IL_8E:
			if (char.IsWhiteSpace(c))
			{
				return true;
			}
			return false;
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000E62E File Offset: 0x0000C82E
		private void ParseTrue()
		{
			if (this.MatchValueWithTrailingSeparator(JsonConvert.True))
			{
				base.SetToken(JsonToken.Boolean, true);
				return;
			}
			throw JsonReaderException.Create(this, "Error parsing boolean value.");
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000E657 File Offset: 0x0000C857
		private void ParseNull()
		{
			if (this.MatchValueWithTrailingSeparator(JsonConvert.Null))
			{
				base.SetToken(JsonToken.Null);
				return;
			}
			throw JsonReaderException.Create(this, "Error parsing null value.");
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000E67A File Offset: 0x0000C87A
		private void ParseUndefined()
		{
			if (this.MatchValueWithTrailingSeparator(JsonConvert.Undefined))
			{
				base.SetToken(JsonToken.Undefined);
				return;
			}
			throw JsonReaderException.Create(this, "Error parsing undefined value.");
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000E69D File Offset: 0x0000C89D
		private void ParseFalse()
		{
			if (this.MatchValueWithTrailingSeparator(JsonConvert.False))
			{
				base.SetToken(JsonToken.Boolean, false);
				return;
			}
			throw JsonReaderException.Create(this, "Error parsing boolean value.");
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000E6C8 File Offset: 0x0000C8C8
		private void ParseNumberNegativeInfinity()
		{
			if (!this.MatchValueWithTrailingSeparator(JsonConvert.NegativeInfinity))
			{
				throw JsonReaderException.Create(this, "Error parsing negative infinity value.");
			}
			if (this._floatParseHandling == FloatParseHandling.Decimal)
			{
				throw new JsonReaderException("Cannot read -Infinity as a decimal.");
			}
			base.SetToken(JsonToken.Float, double.NegativeInfinity);
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000E718 File Offset: 0x0000C918
		private void ParseNumberPositiveInfinity()
		{
			if (!this.MatchValueWithTrailingSeparator(JsonConvert.PositiveInfinity))
			{
				throw JsonReaderException.Create(this, "Error parsing positive infinity value.");
			}
			if (this._floatParseHandling == FloatParseHandling.Decimal)
			{
				throw new JsonReaderException("Cannot read Infinity as a decimal.");
			}
			base.SetToken(JsonToken.Float, double.PositiveInfinity);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000E768 File Offset: 0x0000C968
		private void ParseNumberNaN()
		{
			if (!this.MatchValueWithTrailingSeparator(JsonConvert.NaN))
			{
				throw JsonReaderException.Create(this, "Error parsing NaN value.");
			}
			if (this._floatParseHandling == FloatParseHandling.Decimal)
			{
				throw new JsonReaderException("Cannot read NaN as a decimal.");
			}
			base.SetToken(JsonToken.Float, double.NaN);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000E7B7 File Offset: 0x0000C9B7
		public override void Close()
		{
			base.Close();
			if (base.CloseInput && this._reader != null)
			{
				this._reader.Close();
			}
			if (this._buffer != null)
			{
				this._buffer.Clear();
			}
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000E7ED File Offset: 0x0000C9ED
		public bool HasLineInfo()
		{
			return true;
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060003CE RID: 974 RVA: 0x0000E7F0 File Offset: 0x0000C9F0
		public int LineNumber
		{
			get
			{
				if (base.CurrentState == JsonReader.State.Start && this.LinePosition == 0)
				{
					return 0;
				}
				return this._lineNumber;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060003CF RID: 975 RVA: 0x0000E80A File Offset: 0x0000CA0A
		public int LinePosition
		{
			get
			{
				return this._charPos - this._lineStartPos;
			}
		}

		// Token: 0x0400015D RID: 349
		private const char UnicodeReplacementChar = '�';

		// Token: 0x0400015E RID: 350
		private const int MaximumJavascriptIntegerCharacterLength = 380;

		// Token: 0x0400015F RID: 351
		private readonly TextReader _reader;

		// Token: 0x04000160 RID: 352
		private char[] _chars;

		// Token: 0x04000161 RID: 353
		private int _charsUsed;

		// Token: 0x04000162 RID: 354
		private int _charPos;

		// Token: 0x04000163 RID: 355
		private int _lineStartPos;

		// Token: 0x04000164 RID: 356
		private int _lineNumber;

		// Token: 0x04000165 RID: 357
		private bool _isEndOfFile;

		// Token: 0x04000166 RID: 358
		private StringBuffer _buffer;

		// Token: 0x04000167 RID: 359
		private StringReference _stringReference;

		// Token: 0x04000168 RID: 360
		internal PropertyNameTable NameTable;
	}
}
