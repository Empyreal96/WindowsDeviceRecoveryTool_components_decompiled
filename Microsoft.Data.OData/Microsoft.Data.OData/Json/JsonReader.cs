using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace Microsoft.Data.OData.Json
{
	// Token: 0x0200016C RID: 364
	[DebuggerDisplay("{NodeType}: {Value}")]
	internal class JsonReader
	{
		// Token: 0x06000A3F RID: 2623 RVA: 0x00021230 File Offset: 0x0001F430
		public JsonReader(TextReader reader, ODataFormat jsonFormat)
		{
			this.nodeType = JsonNodeType.None;
			this.nodeValue = null;
			this.reader = reader;
			this.characterBuffer = new char[2040];
			this.storedCharacterCount = 0;
			this.tokenStartIndex = 0;
			this.endOfInputReached = false;
			this.allowAnnotations = (jsonFormat == ODataFormat.Json);
			this.supportAspNetDateTimeFormat = (jsonFormat == ODataFormat.VerboseJson);
			this.scopes = new Stack<JsonReader.Scope>();
			this.scopes.Push(new JsonReader.Scope(JsonReader.ScopeType.Root));
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000A40 RID: 2624 RVA: 0x000212B5 File Offset: 0x0001F4B5
		public virtual object Value
		{
			get
			{
				return this.nodeValue;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000A41 RID: 2625 RVA: 0x000212BD File Offset: 0x0001F4BD
		public virtual JsonNodeType NodeType
		{
			get
			{
				return this.nodeType;
			}
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x000212C8 File Offset: 0x0001F4C8
		public virtual bool Read()
		{
			this.nodeValue = null;
			if (!this.SkipWhitespaces())
			{
				return this.EndOfInput();
			}
			JsonReader.Scope scope = this.scopes.Peek();
			bool flag = false;
			if (this.characterBuffer[this.tokenStartIndex] == ',')
			{
				flag = true;
				this.tokenStartIndex++;
				if (!this.SkipWhitespaces())
				{
					return this.EndOfInput();
				}
			}
			switch (scope.Type)
			{
			case JsonReader.ScopeType.Root:
				if (flag)
				{
					throw JsonReaderExtensions.CreateException(Strings.JsonReader_UnexpectedComma(JsonReader.ScopeType.Root));
				}
				if (scope.ValueCount > 0)
				{
					throw JsonReaderExtensions.CreateException(Strings.JsonReader_MultipleTopLevelValues);
				}
				this.nodeType = this.ParseValue();
				break;
			case JsonReader.ScopeType.Array:
				if (flag && scope.ValueCount == 0)
				{
					throw JsonReaderExtensions.CreateException(Strings.JsonReader_UnexpectedComma(JsonReader.ScopeType.Array));
				}
				if (this.characterBuffer[this.tokenStartIndex] == ']')
				{
					this.tokenStartIndex++;
					if (flag)
					{
						throw JsonReaderExtensions.CreateException(Strings.JsonReader_UnexpectedComma(JsonReader.ScopeType.Array));
					}
					this.PopScope();
					this.nodeType = JsonNodeType.EndArray;
				}
				else
				{
					if (!flag && scope.ValueCount > 0)
					{
						throw JsonReaderExtensions.CreateException(Strings.JsonReader_MissingComma(JsonReader.ScopeType.Array));
					}
					this.nodeType = this.ParseValue();
				}
				break;
			case JsonReader.ScopeType.Object:
				if (flag && scope.ValueCount == 0)
				{
					throw JsonReaderExtensions.CreateException(Strings.JsonReader_UnexpectedComma(JsonReader.ScopeType.Object));
				}
				if (this.characterBuffer[this.tokenStartIndex] == '}')
				{
					this.tokenStartIndex++;
					if (flag)
					{
						throw JsonReaderExtensions.CreateException(Strings.JsonReader_UnexpectedComma(JsonReader.ScopeType.Object));
					}
					this.PopScope();
					this.nodeType = JsonNodeType.EndObject;
				}
				else
				{
					if (!flag && scope.ValueCount > 0)
					{
						throw JsonReaderExtensions.CreateException(Strings.JsonReader_MissingComma(JsonReader.ScopeType.Object));
					}
					this.nodeType = this.ParseProperty();
				}
				break;
			case JsonReader.ScopeType.Property:
				if (flag)
				{
					throw JsonReaderExtensions.CreateException(Strings.JsonReader_UnexpectedComma(JsonReader.ScopeType.Property));
				}
				this.nodeType = this.ParseValue();
				break;
			default:
				throw JsonReaderExtensions.CreateException(Strings.General_InternalError(InternalErrorCodes.JsonReader_Read));
			}
			return true;
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x000214D1 File Offset: 0x0001F6D1
		private static bool IsWhitespaceCharacter(char character)
		{
			return character <= ' ' && (character == ' ' || character == '\t' || character == '\n' || character == '\r');
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x000214F0 File Offset: 0x0001F6F0
		private static object TryParseDateTimePrimitiveValue(string stringValue)
		{
			if (!stringValue.StartsWith("/Date(", StringComparison.Ordinal) || !stringValue.EndsWith(")/", StringComparison.Ordinal))
			{
				return null;
			}
			string text = stringValue.Substring("/Date(".Length, stringValue.Length - ("/Date(".Length + ")/".Length));
			int num = text.IndexOfAny(new char[]
			{
				'+',
				'-'
			}, 1);
			if (num == -1)
			{
				long ticks;
				if (long.TryParse(text, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out ticks))
				{
					return new DateTime(JsonValueUtils.JsonTicksToDateTimeTicks(ticks), DateTimeKind.Utc);
				}
			}
			else
			{
				string s = text.Substring(num);
				text = text.Substring(0, num);
				long ticks2;
				int minutes;
				if (long.TryParse(text, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out ticks2) && int.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out minutes))
				{
					return new DateTimeOffset(JsonValueUtils.JsonTicksToDateTimeTicks(ticks2), new TimeSpan(0, minutes, 0));
				}
			}
			return null;
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x000215DC File Offset: 0x0001F7DC
		private JsonNodeType ParseValue()
		{
			this.scopes.Peek().ValueCount++;
			char c = this.characterBuffer[this.tokenStartIndex];
			char c2 = c;
			if (c2 > '[')
			{
				if (c2 <= 'n')
				{
					if (c2 != 'f')
					{
						if (c2 != 'n')
						{
							goto IL_E5;
						}
						this.nodeValue = this.ParseNullPrimitiveValue();
						goto IL_110;
					}
				}
				else if (c2 != 't')
				{
					if (c2 == '{')
					{
						this.PushScope(JsonReader.ScopeType.Object);
						this.tokenStartIndex++;
						return JsonNodeType.StartObject;
					}
					goto IL_E5;
				}
				this.nodeValue = this.ParseBooleanPrimitiveValue();
				goto IL_110;
			}
			if (c2 != '"' && c2 != '\'')
			{
				if (c2 == '[')
				{
					this.PushScope(JsonReader.ScopeType.Array);
					this.tokenStartIndex++;
					return JsonNodeType.StartArray;
				}
			}
			else
			{
				bool flag;
				this.nodeValue = this.ParseStringPrimitiveValue(out flag);
				if (!flag || !this.supportAspNetDateTimeFormat)
				{
					goto IL_110;
				}
				object obj = JsonReader.TryParseDateTimePrimitiveValue((string)this.nodeValue);
				if (obj != null)
				{
					this.nodeValue = obj;
					goto IL_110;
				}
				goto IL_110;
			}
			IL_E5:
			if (!char.IsDigit(c) && c != '-' && c != '.')
			{
				throw JsonReaderExtensions.CreateException(Strings.JsonReader_UnrecognizedToken);
			}
			this.nodeValue = this.ParseNumberPrimitiveValue();
			IL_110:
			this.TryPopPropertyScope();
			return JsonNodeType.PrimitiveValue;
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x00021700 File Offset: 0x0001F900
		private JsonNodeType ParseProperty()
		{
			this.scopes.Peek().ValueCount++;
			this.PushScope(JsonReader.ScopeType.Property);
			this.nodeValue = this.ParseName();
			if (string.IsNullOrEmpty((string)this.nodeValue))
			{
				throw JsonReaderExtensions.CreateException(Strings.JsonReader_InvalidPropertyNameOrUnexpectedComma((string)this.nodeValue));
			}
			if (!this.SkipWhitespaces() || this.characterBuffer[this.tokenStartIndex] != ':')
			{
				throw JsonReaderExtensions.CreateException(Strings.JsonReader_MissingColon((string)this.nodeValue));
			}
			this.tokenStartIndex++;
			return JsonNodeType.Property;
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x000217A0 File Offset: 0x0001F9A0
		private string ParseStringPrimitiveValue()
		{
			bool flag;
			return this.ParseStringPrimitiveValue(out flag);
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x000217B8 File Offset: 0x0001F9B8
		private string ParseStringPrimitiveValue(out bool hasLeadingBackslash)
		{
			hasLeadingBackslash = false;
			char c = this.characterBuffer[this.tokenStartIndex];
			this.tokenStartIndex++;
			StringBuilder stringBuilder = null;
			int num = 0;
			while (this.tokenStartIndex + num < this.storedCharacterCount || this.ReadInput())
			{
				char c2 = this.characterBuffer[this.tokenStartIndex + num];
				if (c2 == '\\')
				{
					if (num == 0 && stringBuilder == null)
					{
						hasLeadingBackslash = true;
					}
					if (stringBuilder == null)
					{
						if (this.stringValueBuilder == null)
						{
							this.stringValueBuilder = new StringBuilder();
						}
						else
						{
							this.stringValueBuilder.Length = 0;
						}
						stringBuilder = this.stringValueBuilder;
					}
					stringBuilder.Append(this.ConsumeTokenToString(num));
					num = 0;
					if (!this.EnsureAvailableCharacters(2))
					{
						throw JsonReaderExtensions.CreateException(Strings.JsonReader_UnrecognizedEscapeSequence("\\"));
					}
					c2 = this.characterBuffer[this.tokenStartIndex + 1];
					this.tokenStartIndex += 2;
					char c3 = c2;
					if (c3 <= '\\')
					{
						if (c3 <= '\'')
						{
							if (c3 != '"' && c3 != '\'')
							{
								goto IL_1E0;
							}
						}
						else if (c3 != '/' && c3 != '\\')
						{
							goto IL_1E0;
						}
						stringBuilder.Append(c2);
						continue;
					}
					if (c3 <= 'f')
					{
						if (c3 == 'b')
						{
							stringBuilder.Append('\b');
							continue;
						}
						if (c3 == 'f')
						{
							stringBuilder.Append('\f');
							continue;
						}
					}
					else
					{
						if (c3 == 'n')
						{
							stringBuilder.Append('\n');
							continue;
						}
						switch (c3)
						{
						case 'r':
							stringBuilder.Append('\r');
							continue;
						case 't':
							stringBuilder.Append('\t');
							continue;
						case 'u':
						{
							if (!this.EnsureAvailableCharacters(4))
							{
								throw JsonReaderExtensions.CreateException(Strings.JsonReader_UnrecognizedEscapeSequence("\\uXXXX"));
							}
							string text = this.ConsumeTokenToString(4);
							int num2;
							if (!int.TryParse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num2))
							{
								throw JsonReaderExtensions.CreateException(Strings.JsonReader_UnrecognizedEscapeSequence("\\u" + text));
							}
							stringBuilder.Append((char)num2);
							continue;
						}
						}
					}
					IL_1E0:
					throw JsonReaderExtensions.CreateException(Strings.JsonReader_UnrecognizedEscapeSequence("\\" + c2));
				}
				else
				{
					if (c2 == c)
					{
						string text2 = this.ConsumeTokenToString(num);
						this.tokenStartIndex++;
						if (stringBuilder != null)
						{
							stringBuilder.Append(text2);
							text2 = stringBuilder.ToString();
						}
						return text2;
					}
					num++;
				}
			}
			throw JsonReaderExtensions.CreateException(Strings.JsonReader_UnexpectedEndOfString);
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x00021A20 File Offset: 0x0001FC20
		private object ParseNullPrimitiveValue()
		{
			string text = this.ParseName();
			if (!string.Equals(text, "null"))
			{
				throw JsonReaderExtensions.CreateException(Strings.JsonReader_UnexpectedToken(text));
			}
			return null;
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x00021A50 File Offset: 0x0001FC50
		private object ParseBooleanPrimitiveValue()
		{
			string text = this.ParseName();
			if (string.Equals(text, "false"))
			{
				return false;
			}
			if (string.Equals(text, "true"))
			{
				return true;
			}
			throw JsonReaderExtensions.CreateException(Strings.JsonReader_UnexpectedToken(text));
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x00021A98 File Offset: 0x0001FC98
		private object ParseNumberPrimitiveValue()
		{
			int num = 1;
			while (this.tokenStartIndex + num < this.storedCharacterCount || this.ReadInput())
			{
				char c = this.characterBuffer[this.tokenStartIndex + num];
				if (!char.IsDigit(c) && c != '.' && c != 'E' && c != 'e' && c != '-' && c != '+')
				{
					break;
				}
				num++;
			}
			string text = this.ConsumeTokenToString(num);
			int num2;
			if (int.TryParse(text, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out num2))
			{
				return num2;
			}
			double num3;
			if (double.TryParse(text, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out num3))
			{
				return num3;
			}
			throw JsonReaderExtensions.CreateException(Strings.JsonReader_InvalidNumberFormat(text));
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x00021B3C File Offset: 0x0001FD3C
		private string ParseName()
		{
			char c = this.characterBuffer[this.tokenStartIndex];
			if (c == '"' || c == '\'')
			{
				return this.ParseStringPrimitiveValue();
			}
			int num = 0;
			do
			{
				char c2 = this.characterBuffer[this.tokenStartIndex + num];
				if (c2 != '_' && !char.IsLetterOrDigit(c2) && c2 != '$' && (!this.allowAnnotations || (c2 != '.' && c2 != '@')))
				{
					break;
				}
				num++;
			}
			while (this.tokenStartIndex + num < this.storedCharacterCount || this.ReadInput());
			return this.ConsumeTokenToString(num);
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x00021BC1 File Offset: 0x0001FDC1
		private bool EndOfInput()
		{
			if (this.scopes.Count > 1)
			{
				throw JsonReaderExtensions.CreateException(Strings.JsonReader_EndOfInputWithOpenScope);
			}
			this.nodeType = JsonNodeType.EndOfInput;
			return false;
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x00021BE4 File Offset: 0x0001FDE4
		private void PushScope(JsonReader.ScopeType newScopeType)
		{
			this.scopes.Push(new JsonReader.Scope(newScopeType));
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x00021BF7 File Offset: 0x0001FDF7
		private void PopScope()
		{
			this.scopes.Pop();
			this.TryPopPropertyScope();
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x00021C0B File Offset: 0x0001FE0B
		private void TryPopPropertyScope()
		{
			if (this.scopes.Peek().Type == JsonReader.ScopeType.Property)
			{
				this.scopes.Pop();
			}
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x00021C2C File Offset: 0x0001FE2C
		private bool SkipWhitespaces()
		{
			for (;;)
			{
				if (this.tokenStartIndex >= this.storedCharacterCount)
				{
					if (!this.ReadInput())
					{
						return false;
					}
				}
				else
				{
					if (!JsonReader.IsWhitespaceCharacter(this.characterBuffer[this.tokenStartIndex]))
					{
						break;
					}
					this.tokenStartIndex++;
				}
			}
			return true;
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x00021C6B File Offset: 0x0001FE6B
		private bool EnsureAvailableCharacters(int characterCountAfterTokenStart)
		{
			while (this.tokenStartIndex + characterCountAfterTokenStart > this.storedCharacterCount)
			{
				if (!this.ReadInput())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x00021C8C File Offset: 0x0001FE8C
		private string ConsumeTokenToString(int characterCount)
		{
			string result = new string(this.characterBuffer, this.tokenStartIndex, characterCount);
			this.tokenStartIndex += characterCount;
			return result;
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x00021CBC File Offset: 0x0001FEBC
		private bool ReadInput()
		{
			if (this.endOfInputReached)
			{
				return false;
			}
			if (this.storedCharacterCount == this.characterBuffer.Length)
			{
				if (this.tokenStartIndex == this.storedCharacterCount)
				{
					this.tokenStartIndex = 0;
					this.storedCharacterCount = 0;
				}
				else if (this.tokenStartIndex > this.characterBuffer.Length - 64)
				{
					Array.Copy(this.characterBuffer, this.tokenStartIndex, this.characterBuffer, 0, this.storedCharacterCount - this.tokenStartIndex);
					this.storedCharacterCount -= this.tokenStartIndex;
					this.tokenStartIndex = 0;
				}
				else
				{
					int num = this.characterBuffer.Length * 2;
					char[] destinationArray = new char[num];
					Array.Copy(this.characterBuffer, 0, destinationArray, 0, this.characterBuffer.Length);
					this.characterBuffer = destinationArray;
				}
			}
			int num2 = this.reader.Read(this.characterBuffer, this.storedCharacterCount, this.characterBuffer.Length - this.storedCharacterCount);
			if (num2 == 0)
			{
				this.endOfInputReached = true;
				return false;
			}
			this.storedCharacterCount += num2;
			return true;
		}

		// Token: 0x040003C5 RID: 965
		private const int InitialCharacterBufferSize = 2040;

		// Token: 0x040003C6 RID: 966
		private const int MaxCharacterCountToMove = 64;

		// Token: 0x040003C7 RID: 967
		private const string DateTimeFormatPrefix = "/Date(";

		// Token: 0x040003C8 RID: 968
		private const string DateTimeFormatSuffix = ")/";

		// Token: 0x040003C9 RID: 969
		private readonly TextReader reader;

		// Token: 0x040003CA RID: 970
		private readonly Stack<JsonReader.Scope> scopes;

		// Token: 0x040003CB RID: 971
		private readonly bool allowAnnotations;

		// Token: 0x040003CC RID: 972
		private readonly bool supportAspNetDateTimeFormat;

		// Token: 0x040003CD RID: 973
		private bool endOfInputReached;

		// Token: 0x040003CE RID: 974
		private char[] characterBuffer;

		// Token: 0x040003CF RID: 975
		private int storedCharacterCount;

		// Token: 0x040003D0 RID: 976
		private int tokenStartIndex;

		// Token: 0x040003D1 RID: 977
		private JsonNodeType nodeType;

		// Token: 0x040003D2 RID: 978
		private object nodeValue;

		// Token: 0x040003D3 RID: 979
		private StringBuilder stringValueBuilder;

		// Token: 0x0200016D RID: 365
		private enum ScopeType
		{
			// Token: 0x040003D5 RID: 981
			Root,
			// Token: 0x040003D6 RID: 982
			Array,
			// Token: 0x040003D7 RID: 983
			Object,
			// Token: 0x040003D8 RID: 984
			Property
		}

		// Token: 0x0200016E RID: 366
		private sealed class Scope
		{
			// Token: 0x06000A55 RID: 2645 RVA: 0x00021DCC File Offset: 0x0001FFCC
			public Scope(JsonReader.ScopeType type)
			{
				this.type = type;
			}

			// Token: 0x1700026D RID: 621
			// (get) Token: 0x06000A56 RID: 2646 RVA: 0x00021DDB File Offset: 0x0001FFDB
			// (set) Token: 0x06000A57 RID: 2647 RVA: 0x00021DE3 File Offset: 0x0001FFE3
			public int ValueCount { get; set; }

			// Token: 0x1700026E RID: 622
			// (get) Token: 0x06000A58 RID: 2648 RVA: 0x00021DEC File Offset: 0x0001FFEC
			public JsonReader.ScopeType Type
			{
				get
				{
					return this.type;
				}
			}

			// Token: 0x040003D9 RID: 985
			private readonly JsonReader.ScopeType type;
		}
	}
}
