using System;
using System.Globalization;
using System.Numerics;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000081 RID: 129
	public class JTokenWriter : JsonWriter
	{
		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060006DC RID: 1756 RVA: 0x0001AF8B File Offset: 0x0001918B
		public JToken CurrentToken
		{
			get
			{
				return this._current;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x0001AF93 File Offset: 0x00019193
		public JToken Token
		{
			get
			{
				if (this._token != null)
				{
					return this._token;
				}
				return this._value;
			}
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0001AFAA File Offset: 0x000191AA
		public JTokenWriter(JContainer container)
		{
			ValidationUtils.ArgumentNotNull(container, "container");
			this._token = container;
			this._parent = container;
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0001AFCB File Offset: 0x000191CB
		public JTokenWriter()
		{
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0001AFD3 File Offset: 0x000191D3
		public override void Flush()
		{
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0001AFD5 File Offset: 0x000191D5
		public override void Close()
		{
			base.Close();
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0001AFDD File Offset: 0x000191DD
		public override void WriteStartObject()
		{
			base.WriteStartObject();
			this.AddParent(new JObject());
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0001AFF0 File Offset: 0x000191F0
		private void AddParent(JContainer container)
		{
			if (this._parent == null)
			{
				this._token = container;
			}
			else
			{
				this._parent.AddAndSkipParentCheck(container);
			}
			this._parent = container;
			this._current = container;
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0001B020 File Offset: 0x00019220
		private void RemoveParent()
		{
			this._current = this._parent;
			this._parent = this._parent.Parent;
			if (this._parent != null && this._parent.Type == JTokenType.Property)
			{
				this._parent = this._parent.Parent;
			}
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0001B071 File Offset: 0x00019271
		public override void WriteStartArray()
		{
			base.WriteStartArray();
			this.AddParent(new JArray());
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x0001B084 File Offset: 0x00019284
		public override void WriteStartConstructor(string name)
		{
			base.WriteStartConstructor(name);
			this.AddParent(new JConstructor(name));
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x0001B099 File Offset: 0x00019299
		protected override void WriteEnd(JsonToken token)
		{
			this.RemoveParent();
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0001B0A1 File Offset: 0x000192A1
		public override void WritePropertyName(string name)
		{
			this.AddParent(new JProperty(name));
			base.WritePropertyName(name);
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x0001B0B6 File Offset: 0x000192B6
		private void AddValue(object value, JsonToken token)
		{
			this.AddValue(new JValue(value), token);
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x0001B0C8 File Offset: 0x000192C8
		internal void AddValue(JValue value, JsonToken token)
		{
			if (this._parent != null)
			{
				this._parent.Add(value);
				this._current = this._parent.Last;
				if (this._parent.Type == JTokenType.Property)
				{
					this._parent = this._parent.Parent;
					return;
				}
			}
			else
			{
				this._value = (value ?? JValue.CreateNull());
				this._current = this._value;
			}
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0001B136 File Offset: 0x00019336
		public override void WriteValue(object value)
		{
			if (value is BigInteger)
			{
				base.InternalWriteValue(JsonToken.Integer);
				this.AddValue(value, JsonToken.Integer);
				return;
			}
			base.WriteValue(value);
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0001B157 File Offset: 0x00019357
		public override void WriteNull()
		{
			base.WriteNull();
			this.AddValue(null, JsonToken.Null);
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0001B168 File Offset: 0x00019368
		public override void WriteUndefined()
		{
			base.WriteUndefined();
			this.AddValue(null, JsonToken.Undefined);
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0001B179 File Offset: 0x00019379
		public override void WriteRaw(string json)
		{
			base.WriteRaw(json);
			this.AddValue(new JRaw(json), JsonToken.Raw);
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0001B18F File Offset: 0x0001938F
		public override void WriteComment(string text)
		{
			base.WriteComment(text);
			this.AddValue(JValue.CreateComment(text), JsonToken.Comment);
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0001B1A5 File Offset: 0x000193A5
		public override void WriteValue(string value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.String);
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x0001B1B7 File Offset: 0x000193B7
		public override void WriteValue(int value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x0001B1CD File Offset: 0x000193CD
		[CLSCompliant(false)]
		public override void WriteValue(uint value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0001B1E3 File Offset: 0x000193E3
		public override void WriteValue(long value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0001B1F9 File Offset: 0x000193F9
		[CLSCompliant(false)]
		public override void WriteValue(ulong value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0001B20F File Offset: 0x0001940F
		public override void WriteValue(float value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Float);
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0001B225 File Offset: 0x00019425
		public override void WriteValue(double value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Float);
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0001B23B File Offset: 0x0001943B
		public override void WriteValue(bool value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Boolean);
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0001B252 File Offset: 0x00019452
		public override void WriteValue(short value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0001B268 File Offset: 0x00019468
		[CLSCompliant(false)]
		public override void WriteValue(ushort value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0001B280 File Offset: 0x00019480
		public override void WriteValue(char value)
		{
			base.WriteValue(value);
			string value2 = value.ToString(CultureInfo.InvariantCulture);
			this.AddValue(value2, JsonToken.String);
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0001B2AC File Offset: 0x000194AC
		public override void WriteValue(byte value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0001B2C2 File Offset: 0x000194C2
		[CLSCompliant(false)]
		public override void WriteValue(sbyte value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0001B2D8 File Offset: 0x000194D8
		public override void WriteValue(decimal value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Float);
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0001B2EE File Offset: 0x000194EE
		public override void WriteValue(DateTime value)
		{
			base.WriteValue(value);
			value = DateTimeUtils.EnsureDateTime(value, base.DateTimeZoneHandling);
			this.AddValue(value, JsonToken.Date);
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0001B313 File Offset: 0x00019513
		public override void WriteValue(DateTimeOffset value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Date);
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0001B32A File Offset: 0x0001952A
		public override void WriteValue(byte[] value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Bytes);
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0001B33C File Offset: 0x0001953C
		public override void WriteValue(TimeSpan value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.String);
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0001B353 File Offset: 0x00019553
		public override void WriteValue(Guid value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.String);
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0001B36A File Offset: 0x0001956A
		public override void WriteValue(Uri value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.String);
		}

		// Token: 0x040001FC RID: 508
		private JContainer _token;

		// Token: 0x040001FD RID: 509
		private JContainer _parent;

		// Token: 0x040001FE RID: 510
		private JValue _value;

		// Token: 0x040001FF RID: 511
		private JToken _current;
	}
}
