using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using Newtonsoft.Json.Linq.JsonPath;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x0200005F RID: 95
	public abstract class JToken : IJEnumerable<JToken>, IEnumerable<JToken>, IEnumerable, IJsonLineInfo, ICloneable, IDynamicMetaObjectProvider
	{
		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000474 RID: 1140 RVA: 0x00011295 File Offset: 0x0000F495
		public static JTokenEqualityComparer EqualityComparer
		{
			get
			{
				if (JToken._equalityComparer == null)
				{
					JToken._equalityComparer = new JTokenEqualityComparer();
				}
				return JToken._equalityComparer;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000475 RID: 1141 RVA: 0x000112AD File Offset: 0x0000F4AD
		// (set) Token: 0x06000476 RID: 1142 RVA: 0x000112B5 File Offset: 0x0000F4B5
		public JContainer Parent
		{
			[DebuggerStepThrough]
			get
			{
				return this._parent;
			}
			internal set
			{
				this._parent = value;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x000112C0 File Offset: 0x0000F4C0
		public JToken Root
		{
			get
			{
				JContainer parent = this.Parent;
				if (parent == null)
				{
					return this;
				}
				while (parent.Parent != null)
				{
					parent = parent.Parent;
				}
				return parent;
			}
		}

		// Token: 0x06000478 RID: 1144
		internal abstract JToken CloneToken();

		// Token: 0x06000479 RID: 1145
		internal abstract bool DeepEquals(JToken node);

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600047A RID: 1146
		public abstract JTokenType Type { get; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600047B RID: 1147
		public abstract bool HasValues { get; }

		// Token: 0x0600047C RID: 1148 RVA: 0x000112E9 File Offset: 0x0000F4E9
		public static bool DeepEquals(JToken t1, JToken t2)
		{
			return t1 == t2 || (t1 != null && t2 != null && t1.DeepEquals(t2));
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600047D RID: 1149 RVA: 0x00011300 File Offset: 0x0000F500
		// (set) Token: 0x0600047E RID: 1150 RVA: 0x00011308 File Offset: 0x0000F508
		public JToken Next
		{
			get
			{
				return this._next;
			}
			internal set
			{
				this._next = value;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x00011311 File Offset: 0x0000F511
		// (set) Token: 0x06000480 RID: 1152 RVA: 0x00011319 File Offset: 0x0000F519
		public JToken Previous
		{
			get
			{
				return this._previous;
			}
			internal set
			{
				this._previous = value;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000481 RID: 1153 RVA: 0x00011324 File Offset: 0x0000F524
		public string Path
		{
			get
			{
				if (this.Parent == null)
				{
					return string.Empty;
				}
				IList<JToken> list = this.AncestorsAndSelf().Reverse<JToken>().ToList<JToken>();
				IList<JsonPosition> list2 = new List<JsonPosition>();
				for (int i = 0; i < list.Count; i++)
				{
					JToken jtoken = list[i];
					JToken jtoken2 = null;
					if (i + 1 < list.Count)
					{
						jtoken2 = list[i + 1];
					}
					else if (list[i].Type == JTokenType.Property)
					{
						jtoken2 = list[i];
					}
					if (jtoken2 != null)
					{
						switch (jtoken.Type)
						{
						case JTokenType.Array:
						case JTokenType.Constructor:
						{
							int position = ((IList<JToken>)jtoken).IndexOf(jtoken2);
							list2.Add(new JsonPosition(JsonContainerType.Array)
							{
								Position = position
							});
							break;
						}
						case JTokenType.Property:
						{
							JProperty jproperty = (JProperty)jtoken;
							list2.Add(new JsonPosition(JsonContainerType.Object)
							{
								PropertyName = jproperty.Name
							});
							break;
						}
						}
					}
				}
				return JsonPosition.BuildPath(list2);
			}
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00011421 File Offset: 0x0000F621
		internal JToken()
		{
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0001142C File Offset: 0x0000F62C
		public void AddAfterSelf(object content)
		{
			if (this._parent == null)
			{
				throw new InvalidOperationException("The parent is missing.");
			}
			int num = this._parent.IndexOfItem(this);
			this._parent.AddInternal(num + 1, content, false);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0001146C File Offset: 0x0000F66C
		public void AddBeforeSelf(object content)
		{
			if (this._parent == null)
			{
				throw new InvalidOperationException("The parent is missing.");
			}
			int index = this._parent.IndexOfItem(this);
			this._parent.AddInternal(index, content, false);
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x000114A7 File Offset: 0x0000F6A7
		public IEnumerable<JToken> Ancestors()
		{
			return this.GetAncestors(false);
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x000114B0 File Offset: 0x0000F6B0
		public IEnumerable<JToken> AncestorsAndSelf()
		{
			return this.GetAncestors(true);
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x000115CC File Offset: 0x0000F7CC
		internal IEnumerable<JToken> GetAncestors(bool self)
		{
			for (JToken current = self ? this : this.Parent; current != null; current = current.Parent)
			{
				yield return current;
			}
			yield break;
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x000116F0 File Offset: 0x0000F8F0
		public IEnumerable<JToken> AfterSelf()
		{
			if (this.Parent != null)
			{
				for (JToken o = this.Next; o != null; o = o.Next)
				{
					yield return o;
				}
			}
			yield break;
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00011810 File Offset: 0x0000FA10
		public IEnumerable<JToken> BeforeSelf()
		{
			for (JToken o = this.Parent.First; o != this; o = o.Next)
			{
				yield return o;
			}
			yield break;
		}

		// Token: 0x1700010C RID: 268
		public virtual JToken this[object key]
		{
			get
			{
				throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, base.GetType()));
			}
			set
			{
				throw new InvalidOperationException("Cannot set child value on {0}.".FormatWith(CultureInfo.InvariantCulture, base.GetType()));
			}
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00011868 File Offset: 0x0000FA68
		public virtual T Value<T>(object key)
		{
			JToken jtoken = this[key];
			if (jtoken != null)
			{
				return jtoken.Convert<JToken, T>();
			}
			return default(T);
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x00011890 File Offset: 0x0000FA90
		public virtual JToken First
		{
			get
			{
				throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, base.GetType()));
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x000118AC File Offset: 0x0000FAAC
		public virtual JToken Last
		{
			get
			{
				throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, base.GetType()));
			}
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x000118C8 File Offset: 0x0000FAC8
		public virtual JEnumerable<JToken> Children()
		{
			return JEnumerable<JToken>.Empty;
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x000118CF File Offset: 0x0000FACF
		public JEnumerable<T> Children<T>() where T : JToken
		{
			return new JEnumerable<T>(this.Children().OfType<T>());
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x000118E6 File Offset: 0x0000FAE6
		public virtual IEnumerable<T> Values<T>()
		{
			throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, base.GetType()));
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00011902 File Offset: 0x0000FB02
		public void Remove()
		{
			if (this._parent == null)
			{
				throw new InvalidOperationException("The parent is missing.");
			}
			this._parent.RemoveItem(this);
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00011924 File Offset: 0x0000FB24
		public void Replace(JToken value)
		{
			if (this._parent == null)
			{
				throw new InvalidOperationException("The parent is missing.");
			}
			this._parent.ReplaceItem(this, value);
		}

		// Token: 0x06000494 RID: 1172
		public abstract void WriteTo(JsonWriter writer, params JsonConverter[] converters);

		// Token: 0x06000495 RID: 1173 RVA: 0x00011946 File Offset: 0x0000FB46
		public override string ToString()
		{
			return this.ToString(Formatting.Indented, new JsonConverter[0]);
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00011958 File Offset: 0x0000FB58
		public string ToString(Formatting formatting, params JsonConverter[] converters)
		{
			string result;
			using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
			{
				this.WriteTo(new JsonTextWriter(stringWriter)
				{
					Formatting = formatting
				}, converters);
				result = stringWriter.ToString();
			}
			return result;
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x000119AC File Offset: 0x0000FBAC
		private static JValue EnsureValue(JToken value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (value is JProperty)
			{
				value = ((JProperty)value).Value;
			}
			return value as JValue;
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x000119E4 File Offset: 0x0000FBE4
		private static string GetType(JToken token)
		{
			ValidationUtils.ArgumentNotNull(token, "token");
			if (token is JProperty)
			{
				token = ((JProperty)token).Value;
			}
			return token.Type.ToString();
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00011A16 File Offset: 0x0000FC16
		private static bool ValidateToken(JToken o, JTokenType[] validTypes, bool nullable)
		{
			return Array.IndexOf<JTokenType>(validTypes, o.Type) != -1 || (nullable && (o.Type == JTokenType.Null || o.Type == JTokenType.Undefined));
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00011A44 File Offset: 0x0000FC44
		public static explicit operator bool(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.BooleanTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Boolean.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is BigInteger)
			{
				return Convert.ToBoolean((int)((BigInteger)jvalue.Value));
			}
			return Convert.ToBoolean(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00011AB8 File Offset: 0x0000FCB8
		public static explicit operator DateTimeOffset(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.DateTimeTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to DateTimeOffset.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is DateTimeOffset)
			{
				return (DateTimeOffset)jvalue.Value;
			}
			if (jvalue.Value is string)
			{
				return DateTimeOffset.Parse((string)jvalue.Value, CultureInfo.InvariantCulture);
			}
			return new DateTimeOffset(Convert.ToDateTime(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x00011B4C File Offset: 0x0000FD4C
		public static explicit operator bool?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.BooleanTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Boolean.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is BigInteger)
			{
				return new bool?(Convert.ToBoolean((int)((BigInteger)jvalue.Value)));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new bool?(Convert.ToBoolean(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00011BE8 File Offset: 0x0000FDE8
		public static explicit operator long(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Int64.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is BigInteger)
			{
				return (long)((BigInteger)jvalue.Value);
			}
			return Convert.ToInt64(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00011C58 File Offset: 0x0000FE58
		public static explicit operator DateTime?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.DateTimeTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to DateTime.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is DateTimeOffset)
			{
				return new DateTime?(((DateTimeOffset)jvalue.Value).DateTime);
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new DateTime?(Convert.ToDateTime(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00011CF4 File Offset: 0x0000FEF4
		public static explicit operator DateTimeOffset?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.DateTimeTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to DateTimeOffset.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			if (jvalue.Value is DateTimeOffset)
			{
				return (DateTimeOffset?)jvalue.Value;
			}
			if (jvalue.Value is string)
			{
				return new DateTimeOffset?(DateTimeOffset.Parse((string)jvalue.Value, CultureInfo.InvariantCulture));
			}
			return new DateTimeOffset?(new DateTimeOffset(Convert.ToDateTime(jvalue.Value, CultureInfo.InvariantCulture)));
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00011DB0 File Offset: 0x0000FFB0
		public static explicit operator decimal?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Decimal.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is BigInteger)
			{
				return new decimal?((decimal)((BigInteger)jvalue.Value));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new decimal?(Convert.ToDecimal(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00011E48 File Offset: 0x00010048
		public static explicit operator double?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Double.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is BigInteger)
			{
				return new double?((double)((BigInteger)jvalue.Value));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new double?(Convert.ToDouble(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00011EE0 File Offset: 0x000100E0
		public static explicit operator char?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.CharTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Char.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is BigInteger)
			{
				return new char?((char)((ushort)((BigInteger)jvalue.Value)));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new char?(Convert.ToChar(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00011F78 File Offset: 0x00010178
		public static explicit operator int(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Int32.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is BigInteger)
			{
				return (int)((BigInteger)jvalue.Value);
			}
			return Convert.ToInt32(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00011FE8 File Offset: 0x000101E8
		public static explicit operator short(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Int16.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is BigInteger)
			{
				return (short)((BigInteger)jvalue.Value);
			}
			return Convert.ToInt16(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00012058 File Offset: 0x00010258
		[CLSCompliant(false)]
		public static explicit operator ushort(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to UInt16.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is BigInteger)
			{
				return (ushort)((BigInteger)jvalue.Value);
			}
			return Convert.ToUInt16(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x000120C8 File Offset: 0x000102C8
		[CLSCompliant(false)]
		public static explicit operator char(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.CharTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Char.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is BigInteger)
			{
				return (char)((ushort)((BigInteger)jvalue.Value));
			}
			return Convert.ToChar(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00012138 File Offset: 0x00010338
		public static explicit operator byte(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Byte.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is BigInteger)
			{
				return (byte)((BigInteger)jvalue.Value);
			}
			return Convert.ToByte(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x000121A8 File Offset: 0x000103A8
		[CLSCompliant(false)]
		public static explicit operator sbyte(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to SByte.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is BigInteger)
			{
				return (sbyte)((BigInteger)jvalue.Value);
			}
			return Convert.ToSByte(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00012218 File Offset: 0x00010418
		public static explicit operator int?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Int32.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is BigInteger)
			{
				return new int?((int)((BigInteger)jvalue.Value));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new int?(Convert.ToInt32(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x000122B0 File Offset: 0x000104B0
		public static explicit operator short?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Int16.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is BigInteger)
			{
				return new short?((short)((BigInteger)jvalue.Value));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new short?(Convert.ToInt16(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00012348 File Offset: 0x00010548
		[CLSCompliant(false)]
		public static explicit operator ushort?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to UInt16.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is BigInteger)
			{
				return new ushort?((ushort)((BigInteger)jvalue.Value));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new ushort?(Convert.ToUInt16(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x000123E0 File Offset: 0x000105E0
		public static explicit operator byte?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Byte.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is BigInteger)
			{
				return new byte?((byte)((BigInteger)jvalue.Value));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new byte?(Convert.ToByte(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x00012478 File Offset: 0x00010678
		[CLSCompliant(false)]
		public static explicit operator sbyte?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to SByte.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is BigInteger)
			{
				return new sbyte?((sbyte)((BigInteger)jvalue.Value));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new sbyte?(Convert.ToSByte(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00012510 File Offset: 0x00010710
		public static explicit operator DateTime(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.DateTimeTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to DateTime.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is DateTimeOffset)
			{
				return ((DateTimeOffset)jvalue.Value).DateTime;
			}
			return Convert.ToDateTime(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00012584 File Offset: 0x00010784
		public static explicit operator long?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Int64.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is BigInteger)
			{
				return new long?((long)((BigInteger)jvalue.Value));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new long?(Convert.ToInt64(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0001261C File Offset: 0x0001081C
		public static explicit operator float?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Single.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is BigInteger)
			{
				return new float?((float)((BigInteger)jvalue.Value));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new float?(Convert.ToSingle(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x000126B4 File Offset: 0x000108B4
		public static explicit operator decimal(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Decimal.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is BigInteger)
			{
				return (decimal)((BigInteger)jvalue.Value);
			}
			return Convert.ToDecimal(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00012724 File Offset: 0x00010924
		[CLSCompliant(false)]
		public static explicit operator uint?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to UInt32.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is BigInteger)
			{
				return new uint?((uint)((BigInteger)jvalue.Value));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new uint?(Convert.ToUInt32(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x000127BC File Offset: 0x000109BC
		[CLSCompliant(false)]
		public static explicit operator ulong?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to UInt64.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is BigInteger)
			{
				return new ulong?((ulong)((BigInteger)jvalue.Value));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new ulong?(Convert.ToUInt64(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00012854 File Offset: 0x00010A54
		public static explicit operator double(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Double.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is BigInteger)
			{
				return (double)((BigInteger)jvalue.Value);
			}
			return Convert.ToDouble(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x000128C4 File Offset: 0x00010AC4
		public static explicit operator float(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Single.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is BigInteger)
			{
				return (float)((BigInteger)jvalue.Value);
			}
			return Convert.ToSingle(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00012934 File Offset: 0x00010B34
		public static explicit operator string(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.StringTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to String.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			if (jvalue.Value is byte[])
			{
				return Convert.ToBase64String((byte[])jvalue.Value);
			}
			if (jvalue.Value is BigInteger)
			{
				return ((BigInteger)jvalue.Value).ToString(CultureInfo.InvariantCulture);
			}
			return Convert.ToString(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x000129D8 File Offset: 0x00010BD8
		[CLSCompliant(false)]
		public static explicit operator uint(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to UInt32.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is BigInteger)
			{
				return (uint)((BigInteger)jvalue.Value);
			}
			return Convert.ToUInt32(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00012A48 File Offset: 0x00010C48
		[CLSCompliant(false)]
		public static explicit operator ulong(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to UInt64.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is BigInteger)
			{
				return (ulong)((BigInteger)jvalue.Value);
			}
			return Convert.ToUInt64(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00012AB8 File Offset: 0x00010CB8
		public static explicit operator byte[](JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.BytesTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to byte array.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is string)
			{
				return Convert.FromBase64String(Convert.ToString(jvalue.Value, CultureInfo.InvariantCulture));
			}
			if (jvalue.Value is BigInteger)
			{
				return ((BigInteger)jvalue.Value).ToByteArray();
			}
			if (jvalue.Value is byte[])
			{
				return (byte[])jvalue.Value;
			}
			throw new ArgumentException("Can not convert {0} to byte array.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00012B74 File Offset: 0x00010D74
		public static explicit operator Guid(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.GuidTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Guid.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is byte[])
			{
				return new Guid((byte[])jvalue.Value);
			}
			if (!(jvalue.Value is Guid))
			{
				return new Guid(Convert.ToString(jvalue.Value, CultureInfo.InvariantCulture));
			}
			return (Guid)jvalue.Value;
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00012C00 File Offset: 0x00010E00
		public static explicit operator Guid?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.GuidTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Guid.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			if (jvalue.Value is byte[])
			{
				return new Guid?(new Guid((byte[])jvalue.Value));
			}
			return new Guid?((jvalue.Value is Guid) ? ((Guid)jvalue.Value) : new Guid(Convert.ToString(jvalue.Value, CultureInfo.InvariantCulture)));
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00012CB8 File Offset: 0x00010EB8
		public static explicit operator TimeSpan(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.TimeSpanTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to TimeSpan.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (!(jvalue.Value is TimeSpan))
			{
				return ConvertUtils.ParseTimeSpan(Convert.ToString(jvalue.Value, CultureInfo.InvariantCulture));
			}
			return (TimeSpan)jvalue.Value;
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x00012D28 File Offset: 0x00010F28
		public static explicit operator TimeSpan?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.TimeSpanTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to TimeSpan.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new TimeSpan?((jvalue.Value is TimeSpan) ? ((TimeSpan)jvalue.Value) : ConvertUtils.ParseTimeSpan(Convert.ToString(jvalue.Value, CultureInfo.InvariantCulture)));
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00012DBC File Offset: 0x00010FBC
		public static explicit operator Uri(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.UriTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Uri.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			if (!(jvalue.Value is Uri))
			{
				return new Uri(Convert.ToString(jvalue.Value, CultureInfo.InvariantCulture));
			}
			return (Uri)jvalue.Value;
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00012E3C File Offset: 0x0001103C
		private static BigInteger ToBigInteger(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.BigIntegerTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to BigInteger.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			return ConvertUtils.ToBigInteger(jvalue.Value);
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00012E88 File Offset: 0x00011088
		private static BigInteger? ToBigIntegerNullable(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.BigIntegerTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to BigInteger.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new BigInteger?(ConvertUtils.ToBigInteger(jvalue.Value));
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00012EEA File Offset: 0x000110EA
		public static implicit operator JToken(bool value)
		{
			return new JValue(value);
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00012EF2 File Offset: 0x000110F2
		public static implicit operator JToken(DateTimeOffset value)
		{
			return new JValue(value);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x00012EFA File Offset: 0x000110FA
		public static implicit operator JToken(byte value)
		{
			return new JValue((long)((ulong)value));
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00012F03 File Offset: 0x00011103
		public static implicit operator JToken(byte? value)
		{
			return new JValue(value);
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00012F10 File Offset: 0x00011110
		[CLSCompliant(false)]
		public static implicit operator JToken(sbyte value)
		{
			return new JValue((long)value);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00012F19 File Offset: 0x00011119
		[CLSCompliant(false)]
		public static implicit operator JToken(sbyte? value)
		{
			return new JValue(value);
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00012F26 File Offset: 0x00011126
		public static implicit operator JToken(bool? value)
		{
			return new JValue(value);
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00012F33 File Offset: 0x00011133
		public static implicit operator JToken(long value)
		{
			return new JValue(value);
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x00012F3B File Offset: 0x0001113B
		public static implicit operator JToken(DateTime? value)
		{
			return new JValue(value);
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x00012F48 File Offset: 0x00011148
		public static implicit operator JToken(DateTimeOffset? value)
		{
			return new JValue(value);
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x00012F55 File Offset: 0x00011155
		public static implicit operator JToken(decimal? value)
		{
			return new JValue(value);
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x00012F62 File Offset: 0x00011162
		public static implicit operator JToken(double? value)
		{
			return new JValue(value);
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00012F6F File Offset: 0x0001116F
		[CLSCompliant(false)]
		public static implicit operator JToken(short value)
		{
			return new JValue((long)value);
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00012F78 File Offset: 0x00011178
		[CLSCompliant(false)]
		public static implicit operator JToken(ushort value)
		{
			return new JValue((long)((ulong)value));
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00012F81 File Offset: 0x00011181
		public static implicit operator JToken(int value)
		{
			return new JValue((long)value);
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00012F8A File Offset: 0x0001118A
		public static implicit operator JToken(int? value)
		{
			return new JValue(value);
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00012F97 File Offset: 0x00011197
		public static implicit operator JToken(DateTime value)
		{
			return new JValue(value);
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00012F9F File Offset: 0x0001119F
		public static implicit operator JToken(long? value)
		{
			return new JValue(value);
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00012FAC File Offset: 0x000111AC
		public static implicit operator JToken(float? value)
		{
			return new JValue(value);
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00012FB9 File Offset: 0x000111B9
		public static implicit operator JToken(decimal value)
		{
			return new JValue(value);
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00012FC1 File Offset: 0x000111C1
		[CLSCompliant(false)]
		public static implicit operator JToken(short? value)
		{
			return new JValue(value);
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00012FCE File Offset: 0x000111CE
		[CLSCompliant(false)]
		public static implicit operator JToken(ushort? value)
		{
			return new JValue(value);
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00012FDB File Offset: 0x000111DB
		[CLSCompliant(false)]
		public static implicit operator JToken(uint? value)
		{
			return new JValue(value);
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00012FE8 File Offset: 0x000111E8
		[CLSCompliant(false)]
		public static implicit operator JToken(ulong? value)
		{
			return new JValue(value);
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x00012FF5 File Offset: 0x000111F5
		public static implicit operator JToken(double value)
		{
			return new JValue(value);
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00012FFD File Offset: 0x000111FD
		public static implicit operator JToken(float value)
		{
			return new JValue(value);
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00013005 File Offset: 0x00011205
		public static implicit operator JToken(string value)
		{
			return new JValue(value);
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0001300D File Offset: 0x0001120D
		[CLSCompliant(false)]
		public static implicit operator JToken(uint value)
		{
			return new JValue((long)((ulong)value));
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x00013016 File Offset: 0x00011216
		[CLSCompliant(false)]
		public static implicit operator JToken(ulong value)
		{
			return new JValue(value);
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0001301E File Offset: 0x0001121E
		public static implicit operator JToken(byte[] value)
		{
			return new JValue(value);
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x00013026 File Offset: 0x00011226
		public static implicit operator JToken(Uri value)
		{
			return new JValue(value);
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0001302E File Offset: 0x0001122E
		public static implicit operator JToken(TimeSpan value)
		{
			return new JValue(value);
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00013036 File Offset: 0x00011236
		public static implicit operator JToken(TimeSpan? value)
		{
			return new JValue(value);
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00013043 File Offset: 0x00011243
		public static implicit operator JToken(Guid value)
		{
			return new JValue(value);
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0001304B File Offset: 0x0001124B
		public static implicit operator JToken(Guid? value)
		{
			return new JValue(value);
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00013058 File Offset: 0x00011258
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<JToken>)this).GetEnumerator();
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00013060 File Offset: 0x00011260
		IEnumerator<JToken> IEnumerable<JToken>.GetEnumerator()
		{
			return this.Children().GetEnumerator();
		}

		// Token: 0x060004E6 RID: 1254
		internal abstract int GetDeepHashCode();

		// Token: 0x1700010F RID: 271
		IJEnumerable<JToken> IJEnumerable<JToken>.this[object key]
		{
			get
			{
				return this[key];
			}
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00013084 File Offset: 0x00011284
		public JsonReader CreateReader()
		{
			return new JTokenReader(this, this.Path);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00013094 File Offset: 0x00011294
		internal static JToken FromObjectInternal(object o, JsonSerializer jsonSerializer)
		{
			ValidationUtils.ArgumentNotNull(o, "o");
			ValidationUtils.ArgumentNotNull(jsonSerializer, "jsonSerializer");
			JToken token;
			using (JTokenWriter jtokenWriter = new JTokenWriter())
			{
				jsonSerializer.Serialize(jtokenWriter, o);
				token = jtokenWriter.Token;
			}
			return token;
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x000130EC File Offset: 0x000112EC
		public static JToken FromObject(object o)
		{
			return JToken.FromObjectInternal(o, JsonSerializer.CreateDefault());
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x000130F9 File Offset: 0x000112F9
		public static JToken FromObject(object o, JsonSerializer jsonSerializer)
		{
			return JToken.FromObjectInternal(o, jsonSerializer);
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00013102 File Offset: 0x00011302
		public T ToObject<T>()
		{
			return (T)((object)this.ToObject(typeof(T)));
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0001311C File Offset: 0x0001131C
		public object ToObject(Type objectType)
		{
			if (JsonConvert.DefaultSettings == null)
			{
				bool flag;
				PrimitiveTypeCode typeCode = ConvertUtils.GetTypeCode(objectType, out flag);
				if (flag && this.Type == JTokenType.String)
				{
					Type type = objectType.IsEnum() ? objectType : Nullable.GetUnderlyingType(objectType);
					try
					{
						return Enum.Parse(type, (string)this, true);
					}
					catch (Exception innerException)
					{
						throw new ArgumentException("Could not convert '{0}' to {1}.".FormatWith(CultureInfo.InvariantCulture, (string)this, type.Name), innerException);
					}
				}
				switch (typeCode)
				{
				case PrimitiveTypeCode.Char:
					return (char)this;
				case PrimitiveTypeCode.CharNullable:
					return (char?)this;
				case PrimitiveTypeCode.Boolean:
					return (bool)this;
				case PrimitiveTypeCode.BooleanNullable:
					return (bool?)this;
				case PrimitiveTypeCode.SByte:
					return (sbyte?)this;
				case PrimitiveTypeCode.SByteNullable:
					return (sbyte)this;
				case PrimitiveTypeCode.Int16:
					return (short)this;
				case PrimitiveTypeCode.Int16Nullable:
					return (short?)this;
				case PrimitiveTypeCode.UInt16:
					return (ushort)this;
				case PrimitiveTypeCode.UInt16Nullable:
					return (ushort?)this;
				case PrimitiveTypeCode.Int32:
					return (int)this;
				case PrimitiveTypeCode.Int32Nullable:
					return (int?)this;
				case PrimitiveTypeCode.Byte:
					return (byte)this;
				case PrimitiveTypeCode.ByteNullable:
					return (byte?)this;
				case PrimitiveTypeCode.UInt32:
					return (uint)this;
				case PrimitiveTypeCode.UInt32Nullable:
					return (uint?)this;
				case PrimitiveTypeCode.Int64:
					return (long)this;
				case PrimitiveTypeCode.Int64Nullable:
					return (long?)this;
				case PrimitiveTypeCode.UInt64:
					return (ulong)this;
				case PrimitiveTypeCode.UInt64Nullable:
					return (ulong?)this;
				case PrimitiveTypeCode.Single:
					return (float)this;
				case PrimitiveTypeCode.SingleNullable:
					return (float?)this;
				case PrimitiveTypeCode.Double:
					return (double)this;
				case PrimitiveTypeCode.DoubleNullable:
					return (double?)this;
				case PrimitiveTypeCode.DateTime:
					return (DateTime)this;
				case PrimitiveTypeCode.DateTimeNullable:
					return (DateTime?)this;
				case PrimitiveTypeCode.DateTimeOffset:
					return (DateTimeOffset)this;
				case PrimitiveTypeCode.DateTimeOffsetNullable:
					return (DateTimeOffset?)this;
				case PrimitiveTypeCode.Decimal:
					return (decimal)this;
				case PrimitiveTypeCode.DecimalNullable:
					return (decimal?)this;
				case PrimitiveTypeCode.Guid:
					return (Guid)this;
				case PrimitiveTypeCode.GuidNullable:
					return (Guid?)this;
				case PrimitiveTypeCode.TimeSpan:
					return (TimeSpan)this;
				case PrimitiveTypeCode.TimeSpanNullable:
					return (TimeSpan?)this;
				case PrimitiveTypeCode.BigInteger:
					return JToken.ToBigInteger(this);
				case PrimitiveTypeCode.BigIntegerNullable:
					return JToken.ToBigIntegerNullable(this);
				case PrimitiveTypeCode.Uri:
					return (Uri)this;
				case PrimitiveTypeCode.String:
					return (string)this;
				}
			}
			return this.ToObject(objectType, JsonSerializer.CreateDefault());
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0001341C File Offset: 0x0001161C
		public T ToObject<T>(JsonSerializer jsonSerializer)
		{
			return (T)((object)this.ToObject(typeof(T), jsonSerializer));
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00013434 File Offset: 0x00011634
		public object ToObject(Type objectType, JsonSerializer jsonSerializer)
		{
			ValidationUtils.ArgumentNotNull(jsonSerializer, "jsonSerializer");
			object result;
			using (JTokenReader jtokenReader = new JTokenReader(this))
			{
				result = jsonSerializer.Deserialize(jtokenReader, objectType);
			}
			return result;
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0001347C File Offset: 0x0001167C
		public static JToken ReadFrom(JsonReader reader)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JToken from JsonReader.");
			}
			IJsonLineInfo lineInfo = reader as IJsonLineInfo;
			switch (reader.TokenType)
			{
			case JsonToken.StartObject:
				return JObject.Load(reader);
			case JsonToken.StartArray:
				return JArray.Load(reader);
			case JsonToken.StartConstructor:
				return JConstructor.Load(reader);
			case JsonToken.PropertyName:
				return JProperty.Load(reader);
			case JsonToken.Comment:
			{
				JValue jvalue = JValue.CreateComment(reader.Value.ToString());
				jvalue.SetLineInfo(lineInfo);
				return jvalue;
			}
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Date:
			case JsonToken.Bytes:
			{
				JValue jvalue = new JValue(reader.Value);
				jvalue.SetLineInfo(lineInfo);
				return jvalue;
			}
			case JsonToken.Null:
			{
				JValue jvalue = JValue.CreateNull();
				jvalue.SetLineInfo(lineInfo);
				return jvalue;
			}
			case JsonToken.Undefined:
			{
				JValue jvalue = JValue.CreateUndefined();
				jvalue.SetLineInfo(lineInfo);
				return jvalue;
			}
			}
			throw JsonReaderException.Create(reader, "Error reading JToken from JsonReader. Unexpected token: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00013598 File Offset: 0x00011798
		public static JToken Parse(string json)
		{
			JToken result;
			using (JsonReader jsonReader = new JsonTextReader(new StringReader(json)))
			{
				JToken jtoken = JToken.Load(jsonReader);
				if (jsonReader.Read() && jsonReader.TokenType != JsonToken.Comment)
				{
					throw JsonReaderException.Create(jsonReader, "Additional text found in JSON string after parsing content.");
				}
				result = jtoken;
			}
			return result;
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x000135F4 File Offset: 0x000117F4
		public static JToken Load(JsonReader reader)
		{
			return JToken.ReadFrom(reader);
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x000135FC File Offset: 0x000117FC
		internal void SetLineInfo(IJsonLineInfo lineInfo)
		{
			if (lineInfo == null || !lineInfo.HasLineInfo())
			{
				return;
			}
			this.SetLineInfo(lineInfo.LineNumber, lineInfo.LinePosition);
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0001361C File Offset: 0x0001181C
		internal void SetLineInfo(int lineNumber, int linePosition)
		{
			this.AddAnnotation(new JToken.LineInfoAnnotation(lineNumber, linePosition));
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0001362B File Offset: 0x0001182B
		bool IJsonLineInfo.HasLineInfo()
		{
			return this.Annotation<JToken.LineInfoAnnotation>() != null;
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060004F6 RID: 1270 RVA: 0x0001363C File Offset: 0x0001183C
		int IJsonLineInfo.LineNumber
		{
			get
			{
				JToken.LineInfoAnnotation lineInfoAnnotation = this.Annotation<JToken.LineInfoAnnotation>();
				if (lineInfoAnnotation != null)
				{
					return lineInfoAnnotation.LineNumber;
				}
				return 0;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x0001365C File Offset: 0x0001185C
		int IJsonLineInfo.LinePosition
		{
			get
			{
				JToken.LineInfoAnnotation lineInfoAnnotation = this.Annotation<JToken.LineInfoAnnotation>();
				if (lineInfoAnnotation != null)
				{
					return lineInfoAnnotation.LinePosition;
				}
				return 0;
			}
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0001367B File Offset: 0x0001187B
		public JToken SelectToken(string path)
		{
			return this.SelectToken(path, false);
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00013688 File Offset: 0x00011888
		public JToken SelectToken(string path, bool errorWhenNoMatch)
		{
			JPath jpath = new JPath(path);
			JToken jtoken = null;
			foreach (JToken jtoken2 in jpath.Evaluate(this, errorWhenNoMatch))
			{
				if (jtoken != null)
				{
					throw new JsonException("Path returned multiple tokens.");
				}
				jtoken = jtoken2;
			}
			return jtoken;
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x000136EC File Offset: 0x000118EC
		public IEnumerable<JToken> SelectTokens(string path)
		{
			return this.SelectTokens(path, false);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x000136F8 File Offset: 0x000118F8
		public IEnumerable<JToken> SelectTokens(string path, bool errorWhenNoMatch)
		{
			JPath jpath = new JPath(path);
			return jpath.Evaluate(this, errorWhenNoMatch);
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00013714 File Offset: 0x00011914
		protected virtual DynamicMetaObject GetMetaObject(Expression parameter)
		{
			return new DynamicProxyMetaObject<JToken>(parameter, this, new DynamicProxy<JToken>(), true);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00013723 File Offset: 0x00011923
		DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Expression parameter)
		{
			return this.GetMetaObject(parameter);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0001372C File Offset: 0x0001192C
		object ICloneable.Clone()
		{
			return this.DeepClone();
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00013734 File Offset: 0x00011934
		public JToken DeepClone()
		{
			return this.CloneToken();
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0001373C File Offset: 0x0001193C
		public void AddAnnotation(object annotation)
		{
			if (annotation == null)
			{
				throw new ArgumentNullException("annotation");
			}
			if (this._annotations == null)
			{
				this._annotations = ((annotation is object[]) ? new object[]
				{
					annotation
				} : annotation);
				return;
			}
			object[] array = this._annotations as object[];
			if (array == null)
			{
				this._annotations = new object[]
				{
					this._annotations,
					annotation
				};
				return;
			}
			int num = 0;
			while (num < array.Length && array[num] != null)
			{
				num++;
			}
			if (num == array.Length)
			{
				Array.Resize<object>(ref array, num * 2);
				this._annotations = array;
			}
			array[num] = annotation;
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x000137D8 File Offset: 0x000119D8
		public T Annotation<T>() where T : class
		{
			if (this._annotations != null)
			{
				object[] array = this._annotations as object[];
				if (array == null)
				{
					return this._annotations as T;
				}
				foreach (object obj in array)
				{
					if (obj == null)
					{
						break;
					}
					T t = obj as T;
					if (t != null)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00013844 File Offset: 0x00011A44
		public object Annotation(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (this._annotations != null)
			{
				object[] array = this._annotations as object[];
				if (array == null)
				{
					if (type.IsInstanceOfType(this._annotations))
					{
						return this._annotations;
					}
				}
				else
				{
					foreach (object obj in array)
					{
						if (obj == null)
						{
							break;
						}
						if (type.IsInstanceOfType(obj))
						{
							return obj;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00013A68 File Offset: 0x00011C68
		public IEnumerable<T> Annotations<T>() where T : class
		{
			if (this._annotations != null)
			{
				object[] annotations = this._annotations as object[];
				if (annotations != null)
				{
					foreach (object o in annotations)
					{
						if (o == null)
						{
							break;
						}
						T casted = o as T;
						if (casted != null)
						{
							yield return casted;
						}
					}
				}
				else
				{
					T annotation = this._annotations as T;
					if (annotation != null)
					{
						yield return annotation;
					}
				}
			}
			yield break;
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00013C3C File Offset: 0x00011E3C
		public IEnumerable<object> Annotations(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (this._annotations != null)
			{
				object[] annotations = this._annotations as object[];
				if (annotations != null)
				{
					foreach (object o in annotations)
					{
						if (o == null)
						{
							break;
						}
						if (type.IsInstanceOfType(o))
						{
							yield return o;
						}
					}
				}
				else if (type.IsInstanceOfType(this._annotations))
				{
					yield return this._annotations;
				}
			}
			yield break;
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00013C60 File Offset: 0x00011E60
		public void RemoveAnnotations<T>() where T : class
		{
			if (this._annotations != null)
			{
				object[] array = this._annotations as object[];
				if (array == null)
				{
					if (this._annotations is T)
					{
						this._annotations = null;
						return;
					}
				}
				else
				{
					int i = 0;
					int j = 0;
					while (i < array.Length)
					{
						object obj = array[i];
						if (obj == null)
						{
							break;
						}
						if (!(obj is T))
						{
							array[j++] = obj;
						}
						i++;
					}
					if (j != 0)
					{
						while (j < i)
						{
							array[j++] = null;
						}
						return;
					}
					this._annotations = null;
				}
			}
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00013CDC File Offset: 0x00011EDC
		public void RemoveAnnotations(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (this._annotations != null)
			{
				object[] array = this._annotations as object[];
				if (array == null)
				{
					if (type.IsInstanceOfType(this._annotations))
					{
						this._annotations = null;
						return;
					}
				}
				else
				{
					int i = 0;
					int j = 0;
					while (i < array.Length)
					{
						object obj = array[i];
						if (obj == null)
						{
							break;
						}
						if (!type.IsInstanceOfType(obj))
						{
							array[j++] = obj;
						}
						i++;
					}
					if (j != 0)
					{
						while (j < i)
						{
							array[j++] = null;
						}
						return;
					}
					this._annotations = null;
				}
			}
		}

		// Token: 0x040001A0 RID: 416
		private static JTokenEqualityComparer _equalityComparer;

		// Token: 0x040001A1 RID: 417
		private JContainer _parent;

		// Token: 0x040001A2 RID: 418
		private JToken _previous;

		// Token: 0x040001A3 RID: 419
		private JToken _next;

		// Token: 0x040001A4 RID: 420
		private object _annotations;

		// Token: 0x040001A5 RID: 421
		private static readonly JTokenType[] BooleanTypes = new JTokenType[]
		{
			JTokenType.Integer,
			JTokenType.Float,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Boolean
		};

		// Token: 0x040001A6 RID: 422
		private static readonly JTokenType[] NumberTypes = new JTokenType[]
		{
			JTokenType.Integer,
			JTokenType.Float,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Boolean
		};

		// Token: 0x040001A7 RID: 423
		private static readonly JTokenType[] BigIntegerTypes = new JTokenType[]
		{
			JTokenType.Integer,
			JTokenType.Float,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Boolean,
			JTokenType.Bytes
		};

		// Token: 0x040001A8 RID: 424
		private static readonly JTokenType[] StringTypes = new JTokenType[]
		{
			JTokenType.Date,
			JTokenType.Integer,
			JTokenType.Float,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Boolean,
			JTokenType.Bytes,
			JTokenType.Guid,
			JTokenType.TimeSpan,
			JTokenType.Uri
		};

		// Token: 0x040001A9 RID: 425
		private static readonly JTokenType[] GuidTypes = new JTokenType[]
		{
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Guid,
			JTokenType.Bytes
		};

		// Token: 0x040001AA RID: 426
		private static readonly JTokenType[] TimeSpanTypes = new JTokenType[]
		{
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.TimeSpan
		};

		// Token: 0x040001AB RID: 427
		private static readonly JTokenType[] UriTypes = new JTokenType[]
		{
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Uri
		};

		// Token: 0x040001AC RID: 428
		private static readonly JTokenType[] CharTypes = new JTokenType[]
		{
			JTokenType.Integer,
			JTokenType.Float,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw
		};

		// Token: 0x040001AD RID: 429
		private static readonly JTokenType[] DateTimeTypes = new JTokenType[]
		{
			JTokenType.Date,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw
		};

		// Token: 0x040001AE RID: 430
		private static readonly JTokenType[] BytesTypes = new JTokenType[]
		{
			JTokenType.Bytes,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Integer
		};

		// Token: 0x02000060 RID: 96
		private class LineInfoAnnotation
		{
			// Token: 0x06000508 RID: 1288 RVA: 0x00013F23 File Offset: 0x00012123
			public LineInfoAnnotation(int lineNumber, int linePosition)
			{
				this.LineNumber = lineNumber;
				this.LinePosition = linePosition;
			}

			// Token: 0x040001AF RID: 431
			internal readonly int LineNumber;

			// Token: 0x040001B0 RID: 432
			internal readonly int LinePosition;
		}
	}
}
