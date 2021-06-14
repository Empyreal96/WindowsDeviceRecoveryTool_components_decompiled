using System;
using System.Globalization;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000B9 RID: 185
	internal class JsonFormatterConverter : IFormatterConverter
	{
		// Token: 0x060008DF RID: 2271 RVA: 0x00021BC2 File Offset: 0x0001FDC2
		public JsonFormatterConverter(JsonSerializerInternalReader reader, JsonISerializableContract contract, JsonProperty member)
		{
			ValidationUtils.ArgumentNotNull(reader, "serializer");
			ValidationUtils.ArgumentNotNull(contract, "contract");
			this._reader = reader;
			this._contract = contract;
			this._member = member;
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x00021BF8 File Offset: 0x0001FDF8
		private T GetTokenValue<T>(object value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			JValue jvalue = (JValue)value;
			return (T)((object)System.Convert.ChangeType(jvalue.Value, typeof(T), CultureInfo.InvariantCulture));
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x00021C38 File Offset: 0x0001FE38
		public object Convert(object value, Type type)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			JToken jtoken = value as JToken;
			if (jtoken == null)
			{
				throw new ArgumentException("Value is not a JToken.", "value");
			}
			return this._reader.CreateISerializableItem(jtoken, type, this._contract, this._member);
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x00021C83 File Offset: 0x0001FE83
		public object Convert(object value, TypeCode typeCode)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			if (value is JValue)
			{
				value = ((JValue)value).Value;
			}
			return System.Convert.ChangeType(value, typeCode, CultureInfo.InvariantCulture);
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x00021CB1 File Offset: 0x0001FEB1
		public bool ToBoolean(object value)
		{
			return this.GetTokenValue<bool>(value);
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x00021CBA File Offset: 0x0001FEBA
		public byte ToByte(object value)
		{
			return this.GetTokenValue<byte>(value);
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x00021CC3 File Offset: 0x0001FEC3
		public char ToChar(object value)
		{
			return this.GetTokenValue<char>(value);
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x00021CCC File Offset: 0x0001FECC
		public DateTime ToDateTime(object value)
		{
			return this.GetTokenValue<DateTime>(value);
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x00021CD5 File Offset: 0x0001FED5
		public decimal ToDecimal(object value)
		{
			return this.GetTokenValue<decimal>(value);
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x00021CDE File Offset: 0x0001FEDE
		public double ToDouble(object value)
		{
			return this.GetTokenValue<double>(value);
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x00021CE7 File Offset: 0x0001FEE7
		public short ToInt16(object value)
		{
			return this.GetTokenValue<short>(value);
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x00021CF0 File Offset: 0x0001FEF0
		public int ToInt32(object value)
		{
			return this.GetTokenValue<int>(value);
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x00021CF9 File Offset: 0x0001FEF9
		public long ToInt64(object value)
		{
			return this.GetTokenValue<long>(value);
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x00021D02 File Offset: 0x0001FF02
		public sbyte ToSByte(object value)
		{
			return this.GetTokenValue<sbyte>(value);
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x00021D0B File Offset: 0x0001FF0B
		public float ToSingle(object value)
		{
			return this.GetTokenValue<float>(value);
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x00021D14 File Offset: 0x0001FF14
		public string ToString(object value)
		{
			return this.GetTokenValue<string>(value);
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x00021D1D File Offset: 0x0001FF1D
		public ushort ToUInt16(object value)
		{
			return this.GetTokenValue<ushort>(value);
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x00021D26 File Offset: 0x0001FF26
		public uint ToUInt32(object value)
		{
			return this.GetTokenValue<uint>(value);
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x00021D2F File Offset: 0x0001FF2F
		public ulong ToUInt64(object value)
		{
			return this.GetTokenValue<ulong>(value);
		}

		// Token: 0x04000318 RID: 792
		private readonly JsonSerializerInternalReader _reader;

		// Token: 0x04000319 RID: 793
		private readonly JsonISerializableContract _contract;

		// Token: 0x0400031A RID: 794
		private readonly JsonProperty _member;
	}
}
