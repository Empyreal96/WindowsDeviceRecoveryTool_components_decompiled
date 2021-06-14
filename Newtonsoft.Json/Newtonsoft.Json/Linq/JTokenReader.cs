using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x0200007F RID: 127
	public class JTokenReader : JsonReader, IJsonLineInfo
	{
		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060006C6 RID: 1734 RVA: 0x0001AA8E File Offset: 0x00018C8E
		public JToken CurrentToken
		{
			get
			{
				return this._current;
			}
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x0001AA96 File Offset: 0x00018C96
		public JTokenReader(JToken token)
		{
			ValidationUtils.ArgumentNotNull(token, "token");
			this._root = token;
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x0001AAB0 File Offset: 0x00018CB0
		internal JTokenReader(JToken token, string initialPath) : this(token)
		{
			this._initialPath = initialPath;
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0001AAC0 File Offset: 0x00018CC0
		public override byte[] ReadAsBytes()
		{
			return base.ReadAsBytesInternal();
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0001AAC8 File Offset: 0x00018CC8
		public override decimal? ReadAsDecimal()
		{
			return base.ReadAsDecimalInternal();
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0001AAD0 File Offset: 0x00018CD0
		public override int? ReadAsInt32()
		{
			return base.ReadAsInt32Internal();
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0001AAD8 File Offset: 0x00018CD8
		public override string ReadAsString()
		{
			return base.ReadAsStringInternal();
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0001AAE0 File Offset: 0x00018CE0
		public override DateTime? ReadAsDateTime()
		{
			return base.ReadAsDateTimeInternal();
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0001AAE8 File Offset: 0x00018CE8
		public override DateTimeOffset? ReadAsDateTimeOffset()
		{
			return base.ReadAsDateTimeOffsetInternal();
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x0001AAF0 File Offset: 0x00018CF0
		internal override bool ReadInternal()
		{
			if (base.CurrentState == JsonReader.State.Start)
			{
				this._current = this._root;
				this.SetToken(this._current);
				return true;
			}
			if (this._current == null)
			{
				return false;
			}
			JContainer jcontainer = this._current as JContainer;
			if (jcontainer != null && this._parent != jcontainer)
			{
				return this.ReadInto(jcontainer);
			}
			return this.ReadOver(this._current);
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0001AB55 File Offset: 0x00018D55
		public override bool Read()
		{
			this._readType = ReadType.Read;
			return this.ReadInternal();
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0001AB64 File Offset: 0x00018D64
		private bool ReadOver(JToken t)
		{
			if (t == this._root)
			{
				return this.ReadToEnd();
			}
			JToken next = t.Next;
			if (next != null && next != t && t != t.Parent.Last)
			{
				this._current = next;
				this.SetToken(this._current);
				return true;
			}
			if (t.Parent == null)
			{
				return this.ReadToEnd();
			}
			return this.SetEnd(t.Parent);
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0001ABCD File Offset: 0x00018DCD
		private bool ReadToEnd()
		{
			this._current = null;
			base.SetToken(JsonToken.None);
			return false;
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0001ABE0 File Offset: 0x00018DE0
		private JsonToken? GetEndToken(JContainer c)
		{
			switch (c.Type)
			{
			case JTokenType.Object:
				return new JsonToken?(JsonToken.EndObject);
			case JTokenType.Array:
				return new JsonToken?(JsonToken.EndArray);
			case JTokenType.Constructor:
				return new JsonToken?(JsonToken.EndConstructor);
			case JTokenType.Property:
				return null;
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", c.Type, "Unexpected JContainer type.");
			}
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0001AC4C File Offset: 0x00018E4C
		private bool ReadInto(JContainer c)
		{
			JToken first = c.First;
			if (first == null)
			{
				return this.SetEnd(c);
			}
			this.SetToken(first);
			this._current = first;
			this._parent = c;
			return true;
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0001AC84 File Offset: 0x00018E84
		private bool SetEnd(JContainer c)
		{
			JsonToken? endToken = this.GetEndToken(c);
			if (endToken != null)
			{
				base.SetToken(endToken.Value);
				this._current = c;
				this._parent = c;
				return true;
			}
			return this.ReadOver(c);
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0001ACC8 File Offset: 0x00018EC8
		private void SetToken(JToken token)
		{
			switch (token.Type)
			{
			case JTokenType.Object:
				base.SetToken(JsonToken.StartObject);
				return;
			case JTokenType.Array:
				base.SetToken(JsonToken.StartArray);
				return;
			case JTokenType.Constructor:
				base.SetToken(JsonToken.StartConstructor, ((JConstructor)token).Name);
				return;
			case JTokenType.Property:
				base.SetToken(JsonToken.PropertyName, ((JProperty)token).Name);
				return;
			case JTokenType.Comment:
				base.SetToken(JsonToken.Comment, ((JValue)token).Value);
				return;
			case JTokenType.Integer:
				base.SetToken(JsonToken.Integer, ((JValue)token).Value);
				return;
			case JTokenType.Float:
				base.SetToken(JsonToken.Float, ((JValue)token).Value);
				return;
			case JTokenType.String:
				base.SetToken(JsonToken.String, ((JValue)token).Value);
				return;
			case JTokenType.Boolean:
				base.SetToken(JsonToken.Boolean, ((JValue)token).Value);
				return;
			case JTokenType.Null:
				base.SetToken(JsonToken.Null, ((JValue)token).Value);
				return;
			case JTokenType.Undefined:
				base.SetToken(JsonToken.Undefined, ((JValue)token).Value);
				return;
			case JTokenType.Date:
				base.SetToken(JsonToken.Date, ((JValue)token).Value);
				return;
			case JTokenType.Raw:
				base.SetToken(JsonToken.Raw, ((JValue)token).Value);
				return;
			case JTokenType.Bytes:
				base.SetToken(JsonToken.Bytes, ((JValue)token).Value);
				return;
			case JTokenType.Guid:
				base.SetToken(JsonToken.String, this.SafeToString(((JValue)token).Value));
				return;
			case JTokenType.Uri:
				base.SetToken(JsonToken.String, this.SafeToString(((JValue)token).Value));
				return;
			case JTokenType.TimeSpan:
				base.SetToken(JsonToken.String, this.SafeToString(((JValue)token).Value));
				return;
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", token.Type, "Unexpected JTokenType.");
			}
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0001AE8F File Offset: 0x0001908F
		private string SafeToString(object value)
		{
			if (value == null)
			{
				return null;
			}
			return value.ToString();
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0001AE9C File Offset: 0x0001909C
		bool IJsonLineInfo.HasLineInfo()
		{
			if (base.CurrentState == JsonReader.State.Start)
			{
				return false;
			}
			IJsonLineInfo current = this._current;
			return current != null && current.HasLineInfo();
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x0001AEC8 File Offset: 0x000190C8
		int IJsonLineInfo.LineNumber
		{
			get
			{
				if (base.CurrentState == JsonReader.State.Start)
				{
					return 0;
				}
				IJsonLineInfo current = this._current;
				if (current != null)
				{
					return current.LineNumber;
				}
				return 0;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x0001AEF4 File Offset: 0x000190F4
		int IJsonLineInfo.LinePosition
		{
			get
			{
				if (base.CurrentState == JsonReader.State.Start)
				{
					return 0;
				}
				IJsonLineInfo current = this._current;
				if (current != null)
				{
					return current.LinePosition;
				}
				return 0;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x0001AF20 File Offset: 0x00019120
		public override string Path
		{
			get
			{
				string text = base.Path;
				if (!string.IsNullOrEmpty(this._initialPath))
				{
					if (string.IsNullOrEmpty(text))
					{
						return this._initialPath;
					}
					if (this._initialPath.EndsWith(']') || text.StartsWith('['))
					{
						text = this._initialPath + text;
					}
					else
					{
						text = this._initialPath + "." + text;
					}
				}
				return text;
			}
		}

		// Token: 0x040001E5 RID: 485
		private readonly string _initialPath;

		// Token: 0x040001E6 RID: 486
		private readonly JToken _root;

		// Token: 0x040001E7 RID: 487
		private JToken _parent;

		// Token: 0x040001E8 RID: 488
		private JToken _current;
	}
}
