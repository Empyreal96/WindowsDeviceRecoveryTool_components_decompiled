using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq.Expressions;
using System.Numerics;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000075 RID: 117
	public class JValue : JToken, IEquatable<JValue>, IFormattable, IComparable, IComparable<JValue>, IConvertible
	{
		// Token: 0x06000669 RID: 1641 RVA: 0x00019078 File Offset: 0x00017278
		internal JValue(object value, JTokenType type)
		{
			this._value = value;
			this._valueType = type;
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x0001908E File Offset: 0x0001728E
		public JValue(JValue other) : this(other.Value, other.Type)
		{
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x000190A2 File Offset: 0x000172A2
		public JValue(long value) : this(value, JTokenType.Integer)
		{
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x000190B1 File Offset: 0x000172B1
		public JValue(decimal value) : this(value, JTokenType.Float)
		{
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x000190C0 File Offset: 0x000172C0
		public JValue(char value) : this(value, JTokenType.String)
		{
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x000190CF File Offset: 0x000172CF
		[CLSCompliant(false)]
		public JValue(ulong value) : this(value, JTokenType.Integer)
		{
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x000190DE File Offset: 0x000172DE
		public JValue(double value) : this(value, JTokenType.Float)
		{
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x000190ED File Offset: 0x000172ED
		public JValue(float value) : this(value, JTokenType.Float)
		{
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x000190FC File Offset: 0x000172FC
		public JValue(DateTime value) : this(value, JTokenType.Date)
		{
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0001910C File Offset: 0x0001730C
		public JValue(DateTimeOffset value) : this(value, JTokenType.Date)
		{
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0001911C File Offset: 0x0001731C
		public JValue(bool value) : this(value, JTokenType.Boolean)
		{
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0001912C File Offset: 0x0001732C
		public JValue(string value) : this(value, JTokenType.String)
		{
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00019136 File Offset: 0x00017336
		public JValue(Guid value) : this(value, JTokenType.Guid)
		{
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00019146 File Offset: 0x00017346
		public JValue(Uri value) : this(value, (value != null) ? JTokenType.Uri : JTokenType.Null)
		{
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0001915E File Offset: 0x0001735E
		public JValue(TimeSpan value) : this(value, JTokenType.TimeSpan)
		{
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00019170 File Offset: 0x00017370
		public JValue(object value) : this(value, JValue.GetValueType(null, value))
		{
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x00019194 File Offset: 0x00017394
		internal override bool DeepEquals(JToken node)
		{
			JValue jvalue = node as JValue;
			return jvalue != null && (jvalue == this || JValue.ValuesEquals(this, jvalue));
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x0600067A RID: 1658 RVA: 0x000191BA File Offset: 0x000173BA
		public override bool HasValues
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x000191C0 File Offset: 0x000173C0
		private static int CompareBigInteger(BigInteger i1, object i2)
		{
			int num = i1.CompareTo(ConvertUtils.ToBigInteger(i2));
			if (num != 0)
			{
				return num;
			}
			if (i2 is decimal)
			{
				decimal num2 = (decimal)i2;
				return 0m.CompareTo(Math.Abs(num2 - Math.Truncate(num2)));
			}
			if (i2 is double || i2 is float)
			{
				double num3 = Convert.ToDouble(i2, CultureInfo.InvariantCulture);
				return 0.0.CompareTo(Math.Abs(num3 - Math.Truncate(num3)));
			}
			return num;
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0001924C File Offset: 0x0001744C
		internal static int Compare(JTokenType valueType, object objA, object objB)
		{
			if (objA == null && objB == null)
			{
				return 0;
			}
			if (objA != null && objB == null)
			{
				return 1;
			}
			if (objA == null && objB != null)
			{
				return -1;
			}
			switch (valueType)
			{
			case JTokenType.Comment:
			case JTokenType.String:
			case JTokenType.Raw:
			{
				string strA = Convert.ToString(objA, CultureInfo.InvariantCulture);
				string strB = Convert.ToString(objB, CultureInfo.InvariantCulture);
				return string.CompareOrdinal(strA, strB);
			}
			case JTokenType.Integer:
				if (objA is BigInteger)
				{
					return JValue.CompareBigInteger((BigInteger)objA, objB);
				}
				if (objB is BigInteger)
				{
					return -JValue.CompareBigInteger((BigInteger)objB, objA);
				}
				if (objA is ulong || objB is ulong || objA is decimal || objB is decimal)
				{
					return Convert.ToDecimal(objA, CultureInfo.InvariantCulture).CompareTo(Convert.ToDecimal(objB, CultureInfo.InvariantCulture));
				}
				if (objA is float || objB is float || objA is double || objB is double)
				{
					return JValue.CompareFloat(objA, objB);
				}
				return Convert.ToInt64(objA, CultureInfo.InvariantCulture).CompareTo(Convert.ToInt64(objB, CultureInfo.InvariantCulture));
			case JTokenType.Float:
				if (objA is BigInteger)
				{
					return JValue.CompareBigInteger((BigInteger)objA, objB);
				}
				if (objB is BigInteger)
				{
					return -JValue.CompareBigInteger((BigInteger)objB, objA);
				}
				return JValue.CompareFloat(objA, objB);
			case JTokenType.Boolean:
			{
				bool flag = Convert.ToBoolean(objA, CultureInfo.InvariantCulture);
				bool value = Convert.ToBoolean(objB, CultureInfo.InvariantCulture);
				return flag.CompareTo(value);
			}
			case JTokenType.Date:
			{
				if (objA is DateTime)
				{
					DateTime dateTime = (DateTime)objA;
					DateTime value2;
					if (objB is DateTimeOffset)
					{
						value2 = ((DateTimeOffset)objB).DateTime;
					}
					else
					{
						value2 = Convert.ToDateTime(objB, CultureInfo.InvariantCulture);
					}
					return dateTime.CompareTo(value2);
				}
				DateTimeOffset dateTimeOffset = (DateTimeOffset)objA;
				DateTimeOffset other;
				if (objB is DateTimeOffset)
				{
					other = (DateTimeOffset)objB;
				}
				else
				{
					other = new DateTimeOffset(Convert.ToDateTime(objB, CultureInfo.InvariantCulture));
				}
				return dateTimeOffset.CompareTo(other);
			}
			case JTokenType.Bytes:
			{
				if (!(objB is byte[]))
				{
					throw new ArgumentException("Object must be of type byte[].");
				}
				byte[] array = objA as byte[];
				byte[] array2 = objB as byte[];
				if (array == null)
				{
					return -1;
				}
				if (array2 == null)
				{
					return 1;
				}
				return MiscellaneousUtils.ByteArrayCompare(array, array2);
			}
			case JTokenType.Guid:
			{
				if (!(objB is Guid))
				{
					throw new ArgumentException("Object must be of type Guid.");
				}
				Guid guid = (Guid)objA;
				Guid value3 = (Guid)objB;
				return guid.CompareTo(value3);
			}
			case JTokenType.Uri:
			{
				if (!(objB is Uri))
				{
					throw new ArgumentException("Object must be of type Uri.");
				}
				Uri uri = (Uri)objA;
				Uri uri2 = (Uri)objB;
				return Comparer<string>.Default.Compare(uri.ToString(), uri2.ToString());
			}
			case JTokenType.TimeSpan:
			{
				if (!(objB is TimeSpan))
				{
					throw new ArgumentException("Object must be of type TimeSpan.");
				}
				TimeSpan timeSpan = (TimeSpan)objA;
				TimeSpan value4 = (TimeSpan)objB;
				return timeSpan.CompareTo(value4);
			}
			}
			throw MiscellaneousUtils.CreateArgumentOutOfRangeException("valueType", valueType, "Unexpected value type: {0}".FormatWith(CultureInfo.InvariantCulture, valueType));
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0001954C File Offset: 0x0001774C
		private static int CompareFloat(object objA, object objB)
		{
			double d = Convert.ToDouble(objA, CultureInfo.InvariantCulture);
			double num = Convert.ToDouble(objB, CultureInfo.InvariantCulture);
			if (MathUtils.ApproxEquals(d, num))
			{
				return 0;
			}
			return d.CompareTo(num);
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00019584 File Offset: 0x00017784
		private static bool Operation(ExpressionType operation, object objA, object objB, out object result)
		{
			if ((objA is string || objB is string) && (operation == ExpressionType.Add || operation == ExpressionType.AddAssign))
			{
				result = ((objA != null) ? objA.ToString() : null) + ((objB != null) ? objB.ToString() : null);
				return true;
			}
			if (objA is BigInteger || objB is BigInteger)
			{
				if (objA == null || objB == null)
				{
					result = null;
					return true;
				}
				BigInteger bigInteger = ConvertUtils.ToBigInteger(objA);
				BigInteger bigInteger2 = ConvertUtils.ToBigInteger(objB);
				if (operation > ExpressionType.Multiply)
				{
					if (operation <= ExpressionType.DivideAssign)
					{
						if (operation != ExpressionType.Subtract)
						{
							switch (operation)
							{
							case ExpressionType.AddAssign:
								goto IL_BC;
							case ExpressionType.AndAssign:
								goto IL_3CE;
							case ExpressionType.DivideAssign:
								goto IL_EC;
							default:
								goto IL_3CE;
							}
						}
					}
					else
					{
						if (operation == ExpressionType.MultiplyAssign)
						{
							goto IL_DC;
						}
						if (operation != ExpressionType.SubtractAssign)
						{
							goto IL_3CE;
						}
					}
					result = bigInteger - bigInteger2;
					return true;
				}
				if (operation != ExpressionType.Add)
				{
					if (operation == ExpressionType.Divide)
					{
						goto IL_EC;
					}
					if (operation != ExpressionType.Multiply)
					{
						goto IL_3CE;
					}
					goto IL_DC;
				}
				IL_BC:
				result = bigInteger + bigInteger2;
				return true;
				IL_DC:
				result = bigInteger * bigInteger2;
				return true;
				IL_EC:
				result = bigInteger / bigInteger2;
				return true;
			}
			else if (objA is ulong || objB is ulong || objA is decimal || objB is decimal)
			{
				if (objA == null || objB == null)
				{
					result = null;
					return true;
				}
				decimal d = Convert.ToDecimal(objA, CultureInfo.InvariantCulture);
				decimal d2 = Convert.ToDecimal(objB, CultureInfo.InvariantCulture);
				if (operation > ExpressionType.Multiply)
				{
					if (operation <= ExpressionType.DivideAssign)
					{
						if (operation != ExpressionType.Subtract)
						{
							switch (operation)
							{
							case ExpressionType.AddAssign:
								goto IL_199;
							case ExpressionType.AndAssign:
								goto IL_3CE;
							case ExpressionType.DivideAssign:
								goto IL_1C9;
							default:
								goto IL_3CE;
							}
						}
					}
					else
					{
						if (operation == ExpressionType.MultiplyAssign)
						{
							goto IL_1B9;
						}
						if (operation != ExpressionType.SubtractAssign)
						{
							goto IL_3CE;
						}
					}
					result = d - d2;
					return true;
				}
				if (operation != ExpressionType.Add)
				{
					if (operation == ExpressionType.Divide)
					{
						goto IL_1C9;
					}
					if (operation != ExpressionType.Multiply)
					{
						goto IL_3CE;
					}
					goto IL_1B9;
				}
				IL_199:
				result = d + d2;
				return true;
				IL_1B9:
				result = d * d2;
				return true;
				IL_1C9:
				result = d / d2;
				return true;
			}
			else if (objA is float || objB is float || objA is double || objB is double)
			{
				if (objA == null || objB == null)
				{
					result = null;
					return true;
				}
				double num = Convert.ToDouble(objA, CultureInfo.InvariantCulture);
				double num2 = Convert.ToDouble(objB, CultureInfo.InvariantCulture);
				if (operation > ExpressionType.Multiply)
				{
					if (operation <= ExpressionType.DivideAssign)
					{
						if (operation != ExpressionType.Subtract)
						{
							switch (operation)
							{
							case ExpressionType.AddAssign:
								goto IL_278;
							case ExpressionType.AndAssign:
								goto IL_3CE;
							case ExpressionType.DivideAssign:
								goto IL_2A2;
							default:
								goto IL_3CE;
							}
						}
					}
					else
					{
						if (operation == ExpressionType.MultiplyAssign)
						{
							goto IL_294;
						}
						if (operation != ExpressionType.SubtractAssign)
						{
							goto IL_3CE;
						}
					}
					result = num - num2;
					return true;
				}
				if (operation != ExpressionType.Add)
				{
					if (operation == ExpressionType.Divide)
					{
						goto IL_2A2;
					}
					if (operation != ExpressionType.Multiply)
					{
						goto IL_3CE;
					}
					goto IL_294;
				}
				IL_278:
				result = num + num2;
				return true;
				IL_294:
				result = num * num2;
				return true;
				IL_2A2:
				result = num / num2;
				return true;
			}
			else if (objA is int || objA is uint || objA is long || objA is short || objA is ushort || objA is sbyte || objA is byte || objB is int || objB is uint || objB is long || objB is short || objB is ushort || objB is sbyte || objB is byte)
			{
				if (objA == null || objB == null)
				{
					result = null;
					return true;
				}
				long num3 = Convert.ToInt64(objA, CultureInfo.InvariantCulture);
				long num4 = Convert.ToInt64(objB, CultureInfo.InvariantCulture);
				if (operation > ExpressionType.Multiply)
				{
					if (operation <= ExpressionType.DivideAssign)
					{
						if (operation != ExpressionType.Subtract)
						{
							switch (operation)
							{
							case ExpressionType.AddAssign:
								goto IL_396;
							case ExpressionType.AndAssign:
								goto IL_3CE;
							case ExpressionType.DivideAssign:
								goto IL_3C0;
							default:
								goto IL_3CE;
							}
						}
					}
					else
					{
						if (operation == ExpressionType.MultiplyAssign)
						{
							goto IL_3B2;
						}
						if (operation != ExpressionType.SubtractAssign)
						{
							goto IL_3CE;
						}
					}
					result = num3 - num4;
					return true;
				}
				if (operation != ExpressionType.Add)
				{
					if (operation == ExpressionType.Divide)
					{
						goto IL_3C0;
					}
					if (operation != ExpressionType.Multiply)
					{
						goto IL_3CE;
					}
					goto IL_3B2;
				}
				IL_396:
				result = num3 + num4;
				return true;
				IL_3B2:
				result = num3 * num4;
				return true;
				IL_3C0:
				result = num3 / num4;
				return true;
			}
			IL_3CE:
			result = null;
			return false;
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00019963 File Offset: 0x00017B63
		internal override JToken CloneToken()
		{
			return new JValue(this);
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x0001996B File Offset: 0x00017B6B
		public static JValue CreateComment(string value)
		{
			return new JValue(value, JTokenType.Comment);
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00019974 File Offset: 0x00017B74
		public static JValue CreateString(string value)
		{
			return new JValue(value, JTokenType.String);
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0001997D File Offset: 0x00017B7D
		public static JValue CreateNull()
		{
			return new JValue(null, JTokenType.Null);
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x00019987 File Offset: 0x00017B87
		public static JValue CreateUndefined()
		{
			return new JValue(null, JTokenType.Undefined);
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x00019994 File Offset: 0x00017B94
		private static JTokenType GetValueType(JTokenType? current, object value)
		{
			if (value == null)
			{
				return JTokenType.Null;
			}
			if (value == DBNull.Value)
			{
				return JTokenType.Null;
			}
			if (value is string)
			{
				return JValue.GetStringValueType(current);
			}
			if (value is long || value is int || value is short || value is sbyte || value is ulong || value is uint || value is ushort || value is byte)
			{
				return JTokenType.Integer;
			}
			if (value is Enum)
			{
				return JTokenType.Integer;
			}
			if (value is BigInteger)
			{
				return JTokenType.Integer;
			}
			if (value is double || value is float || value is decimal)
			{
				return JTokenType.Float;
			}
			if (value is DateTime)
			{
				return JTokenType.Date;
			}
			if (value is DateTimeOffset)
			{
				return JTokenType.Date;
			}
			if (value is byte[])
			{
				return JTokenType.Bytes;
			}
			if (value is bool)
			{
				return JTokenType.Boolean;
			}
			if (value is Guid)
			{
				return JTokenType.Guid;
			}
			if (value is Uri)
			{
				return JTokenType.Uri;
			}
			if (value is TimeSpan)
			{
				return JTokenType.TimeSpan;
			}
			throw new ArgumentException("Could not determine JSON object type for type {0}.".FormatWith(CultureInfo.InvariantCulture, value.GetType()));
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x00019A98 File Offset: 0x00017C98
		private static JTokenType GetStringValueType(JTokenType? current)
		{
			if (current == null)
			{
				return JTokenType.String;
			}
			JTokenType value = current.Value;
			if (value == JTokenType.Comment || value == JTokenType.String || value == JTokenType.Raw)
			{
				return current.Value;
			}
			return JTokenType.String;
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000686 RID: 1670 RVA: 0x00019ACE File Offset: 0x00017CCE
		public override JTokenType Type
		{
			get
			{
				return this._valueType;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000687 RID: 1671 RVA: 0x00019AD6 File Offset: 0x00017CD6
		// (set) Token: 0x06000688 RID: 1672 RVA: 0x00019AE0 File Offset: 0x00017CE0
		public new object Value
		{
			get
			{
				return this._value;
			}
			set
			{
				Type left = (this._value != null) ? this._value.GetType() : null;
				Type right = (value != null) ? value.GetType() : null;
				if (left != right)
				{
					this._valueType = JValue.GetValueType(new JTokenType?(this._valueType), value);
				}
				this._value = value;
			}
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x00019B38 File Offset: 0x00017D38
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			if (converters != null && converters.Length > 0 && this._value != null)
			{
				JsonConverter matchingConverter = JsonSerializer.GetMatchingConverter(converters, this._value.GetType());
				if (matchingConverter != null && matchingConverter.CanWrite)
				{
					matchingConverter.WriteJson(writer, this._value, JsonSerializer.CreateDefault());
					return;
				}
			}
			switch (this._valueType)
			{
			case JTokenType.Comment:
				writer.WriteComment((this._value != null) ? this._value.ToString() : null);
				return;
			case JTokenType.Integer:
				if (this._value is BigInteger)
				{
					writer.WriteValue((BigInteger)this._value);
					return;
				}
				writer.WriteValue(Convert.ToInt64(this._value, CultureInfo.InvariantCulture));
				return;
			case JTokenType.Float:
				if (this._value is decimal)
				{
					writer.WriteValue((decimal)this._value);
					return;
				}
				if (this._value is double)
				{
					writer.WriteValue((double)this._value);
					return;
				}
				if (this._value is float)
				{
					writer.WriteValue((float)this._value);
					return;
				}
				writer.WriteValue(Convert.ToDouble(this._value, CultureInfo.InvariantCulture));
				return;
			case JTokenType.String:
				writer.WriteValue((this._value != null) ? this._value.ToString() : null);
				return;
			case JTokenType.Boolean:
				writer.WriteValue(Convert.ToBoolean(this._value, CultureInfo.InvariantCulture));
				return;
			case JTokenType.Null:
				writer.WriteNull();
				return;
			case JTokenType.Undefined:
				writer.WriteUndefined();
				return;
			case JTokenType.Date:
				if (this._value is DateTimeOffset)
				{
					writer.WriteValue((DateTimeOffset)this._value);
					return;
				}
				writer.WriteValue(Convert.ToDateTime(this._value, CultureInfo.InvariantCulture));
				return;
			case JTokenType.Raw:
				writer.WriteRawValue((this._value != null) ? this._value.ToString() : null);
				return;
			case JTokenType.Bytes:
				writer.WriteValue((byte[])this._value);
				return;
			case JTokenType.Guid:
			case JTokenType.Uri:
			case JTokenType.TimeSpan:
				writer.WriteValue((this._value != null) ? this._value.ToString() : null);
				return;
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("TokenType", this._valueType, "Unexpected token type.");
			}
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00019D78 File Offset: 0x00017F78
		internal override int GetDeepHashCode()
		{
			int num = (this._value != null) ? this._value.GetHashCode() : 0;
			int valueType = (int)this._valueType;
			return valueType.GetHashCode() ^ num;
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00019DAC File Offset: 0x00017FAC
		private static bool ValuesEquals(JValue v1, JValue v2)
		{
			return v1 == v2 || (v1._valueType == v2._valueType && JValue.Compare(v1._valueType, v1._value, v2._value) == 0);
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00019DDE File Offset: 0x00017FDE
		public bool Equals(JValue other)
		{
			return other != null && JValue.ValuesEquals(this, other);
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x00019DEC File Offset: 0x00017FEC
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			JValue jvalue = obj as JValue;
			if (jvalue != null)
			{
				return this.Equals(jvalue);
			}
			return base.Equals(obj);
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x00019E17 File Offset: 0x00018017
		public override int GetHashCode()
		{
			if (this._value == null)
			{
				return 0;
			}
			return this._value.GetHashCode();
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00019E2E File Offset: 0x0001802E
		public override string ToString()
		{
			if (this._value == null)
			{
				return string.Empty;
			}
			return this._value.ToString();
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00019E49 File Offset: 0x00018049
		public string ToString(string format)
		{
			return this.ToString(format, CultureInfo.CurrentCulture);
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00019E57 File Offset: 0x00018057
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00019E64 File Offset: 0x00018064
		public string ToString(string format, IFormatProvider formatProvider)
		{
			if (this._value == null)
			{
				return string.Empty;
			}
			IFormattable formattable = this._value as IFormattable;
			if (formattable != null)
			{
				return formattable.ToString(format, formatProvider);
			}
			return this._value.ToString();
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x00019EA2 File Offset: 0x000180A2
		protected override DynamicMetaObject GetMetaObject(Expression parameter)
		{
			return new DynamicProxyMetaObject<JValue>(parameter, this, new JValue.JValueDynamicProxy(), true);
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x00019EB4 File Offset: 0x000180B4
		int IComparable.CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			object objB = (obj is JValue) ? ((JValue)obj).Value : obj;
			return JValue.Compare(this._valueType, this._value, objB);
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00019EEF File Offset: 0x000180EF
		public int CompareTo(JValue obj)
		{
			if (obj == null)
			{
				return 1;
			}
			return JValue.Compare(this._valueType, this._value, obj._value);
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00019F0D File Offset: 0x0001810D
		TypeCode IConvertible.GetTypeCode()
		{
			if (this._value == null)
			{
				return TypeCode.Empty;
			}
			if (this._value is DateTimeOffset)
			{
				return TypeCode.DateTime;
			}
			if (this._value is BigInteger)
			{
				return TypeCode.Object;
			}
			return System.Type.GetTypeCode(this._value.GetType());
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00019F48 File Offset: 0x00018148
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return (bool)this;
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x00019F50 File Offset: 0x00018150
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return (char)this;
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00019F58 File Offset: 0x00018158
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return (sbyte)this;
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00019F60 File Offset: 0x00018160
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return (byte)this;
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00019F68 File Offset: 0x00018168
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return (short)this;
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00019F70 File Offset: 0x00018170
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return (ushort)this;
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00019F78 File Offset: 0x00018178
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return (int)this;
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00019F80 File Offset: 0x00018180
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return (uint)this;
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00019F88 File Offset: 0x00018188
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return (long)this;
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x00019F90 File Offset: 0x00018190
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return (ulong)this;
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x00019F98 File Offset: 0x00018198
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return (float)this;
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00019FA1 File Offset: 0x000181A1
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return (double)this;
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x00019FAA File Offset: 0x000181AA
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return (decimal)this;
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x00019FB2 File Offset: 0x000181B2
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			return (DateTime)this;
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00019FBA File Offset: 0x000181BA
		object IConvertible.ToType(Type conversionType, IFormatProvider provider)
		{
			return base.ToObject(conversionType);
		}

		// Token: 0x040001D2 RID: 466
		private JTokenType _valueType;

		// Token: 0x040001D3 RID: 467
		private object _value;

		// Token: 0x02000076 RID: 118
		private class JValueDynamicProxy : DynamicProxy<JValue>
		{
			// Token: 0x060006A6 RID: 1702 RVA: 0x00019FC4 File Offset: 0x000181C4
			public override bool TryConvert(JValue instance, ConvertBinder binder, out object result)
			{
				if (binder.Type == typeof(JValue))
				{
					result = instance;
					return true;
				}
				object value = instance.Value;
				if (value == null)
				{
					result = null;
					return ReflectionUtils.IsNullable(binder.Type);
				}
				result = ConvertUtils.Convert(value, CultureInfo.InvariantCulture, binder.Type);
				return true;
			}

			// Token: 0x060006A7 RID: 1703 RVA: 0x0001A01C File Offset: 0x0001821C
			public override bool TryBinaryOperation(JValue instance, BinaryOperationBinder binder, object arg, out object result)
			{
				object objB = (arg is JValue) ? ((JValue)arg).Value : arg;
				ExpressionType operation = binder.Operation;
				if (operation <= ExpressionType.NotEqual)
				{
					if (operation <= ExpressionType.LessThanOrEqual)
					{
						if (operation != ExpressionType.Add)
						{
							switch (operation)
							{
							case ExpressionType.Divide:
								break;
							case ExpressionType.Equal:
								result = (JValue.Compare(instance.Type, instance.Value, objB) == 0);
								return true;
							case ExpressionType.ExclusiveOr:
							case ExpressionType.Invoke:
							case ExpressionType.Lambda:
							case ExpressionType.LeftShift:
								goto IL_199;
							case ExpressionType.GreaterThan:
								result = (JValue.Compare(instance.Type, instance.Value, objB) > 0);
								return true;
							case ExpressionType.GreaterThanOrEqual:
								result = (JValue.Compare(instance.Type, instance.Value, objB) >= 0);
								return true;
							case ExpressionType.LessThan:
								result = (JValue.Compare(instance.Type, instance.Value, objB) < 0);
								return true;
							case ExpressionType.LessThanOrEqual:
								result = (JValue.Compare(instance.Type, instance.Value, objB) <= 0);
								return true;
							default:
								goto IL_199;
							}
						}
					}
					else if (operation != ExpressionType.Multiply)
					{
						if (operation != ExpressionType.NotEqual)
						{
							goto IL_199;
						}
						result = (JValue.Compare(instance.Type, instance.Value, objB) != 0);
						return true;
					}
				}
				else if (operation <= ExpressionType.DivideAssign)
				{
					if (operation != ExpressionType.Subtract)
					{
						switch (operation)
						{
						case ExpressionType.AddAssign:
						case ExpressionType.DivideAssign:
							break;
						case ExpressionType.AndAssign:
							goto IL_199;
						default:
							goto IL_199;
						}
					}
				}
				else if (operation != ExpressionType.MultiplyAssign && operation != ExpressionType.SubtractAssign)
				{
					goto IL_199;
				}
				if (JValue.Operation(binder.Operation, instance.Value, objB, out result))
				{
					result = new JValue(result);
					return true;
				}
				IL_199:
				result = null;
				return false;
			}
		}
	}
}
