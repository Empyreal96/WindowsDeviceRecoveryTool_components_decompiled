using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Management
{
	/// <summary>Represents information about a WMI property.          </summary>
	// Token: 0x02000048 RID: 72
	public class PropertyData
	{
		// Token: 0x0600028E RID: 654 RVA: 0x0000DC0A File Offset: 0x0000BE0A
		internal PropertyData(ManagementBaseObject parent, string propName)
		{
			this.parent = parent;
			this.propertyName = propName;
			this.qualifiers = null;
			this.RefreshPropertyInfo();
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000DC30 File Offset: 0x0000BE30
		private void RefreshPropertyInfo()
		{
			this.propertyValue = null;
			int num = this.parent.wbemObject.Get_(this.propertyName, 0, ref this.propertyValue, ref this.propertyType, ref this.propertyFlavor);
			if (num < 0)
			{
				if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					return;
				}
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		/// <summary>Gets the name of the property.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the property name.</returns>
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000290 RID: 656 RVA: 0x0000DC9A File Offset: 0x0000BE9A
		public string Name
		{
			get
			{
				if (this.propertyName == null)
				{
					return "";
				}
				return this.propertyName;
			}
		}

		/// <summary>Gets or sets the current value of the property.          </summary>
		/// <returns>Returns an <see cref="T:System.Object" /> value representing the value of the property.</returns>
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000291 RID: 657 RVA: 0x0000DCB0 File Offset: 0x0000BEB0
		// (set) Token: 0x06000292 RID: 658 RVA: 0x0000DCE4 File Offset: 0x0000BEE4
		public object Value
		{
			get
			{
				this.RefreshPropertyInfo();
				return ValueTypeSafety.GetSafeObject(PropertyData.MapWmiValueToValue(this.propertyValue, (CimType)(this.propertyType & -8193), (this.propertyType & 8192) != 0));
			}
			set
			{
				this.RefreshPropertyInfo();
				object obj = PropertyData.MapValueToWmiValue(value, (CimType)(this.propertyType & -8193), (this.propertyType & 8192) != 0);
				int num = this.parent.wbemObject.Put_(this.propertyName, 0, ref obj, 0);
				if (num >= 0)
				{
					if (this.parent.GetType() == typeof(ManagementObject))
					{
						((ManagementObject)this.parent).Path.UpdateRelativePath((string)this.parent["__RELPATH"]);
					}
					return;
				}
				if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					return;
				}
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		/// <summary>Gets the CIM type of the property.          </summary>
		/// <returns>Returns a <see cref="T:System.Management.CimType" /> enumeration value representing the CIM type of the property.</returns>
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000293 RID: 659 RVA: 0x0000DDA7 File Offset: 0x0000BFA7
		public CimType Type
		{
			get
			{
				this.RefreshPropertyInfo();
				return (CimType)(this.propertyType & -8193);
			}
		}

		/// <summary>Gets a value indicating whether the property has been defined in the current WMI class.          </summary>
		/// <returns>Returns a <see cref="T:System.Boolean" /> value indicating whether the property has been defined in the current WMI class.</returns>
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000294 RID: 660 RVA: 0x0000DDBB File Offset: 0x0000BFBB
		public bool IsLocal
		{
			get
			{
				this.RefreshPropertyInfo();
				return (this.propertyFlavor & 32) == 0;
			}
		}

		/// <summary>Gets a value indicating whether the property is an array.          </summary>
		/// <returns>Returns a <see cref="T:System.Boolean" /> value indicating whether the property is an array.</returns>
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000295 RID: 661 RVA: 0x0000DDD1 File Offset: 0x0000BFD1
		public bool IsArray
		{
			get
			{
				this.RefreshPropertyInfo();
				return (this.propertyType & 8192) != 0;
			}
		}

		/// <summary>Gets the name of the WMI class in the hierarchy in which the property was introduced.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the name of the WMI class in the hierarchy in which the property was introduced.</returns>
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000296 RID: 662 RVA: 0x0000DDE8 File Offset: 0x0000BFE8
		public string Origin
		{
			get
			{
				string result = null;
				int propertyOrigin_ = this.parent.wbemObject.GetPropertyOrigin_(this.propertyName, out result);
				if (propertyOrigin_ < 0)
				{
					if (propertyOrigin_ == -2147217393)
					{
						result = string.Empty;
					}
					else if (((long)propertyOrigin_ & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)propertyOrigin_);
					}
					else
					{
						Marshal.ThrowExceptionForHR(propertyOrigin_, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
				return result;
			}
		}

		/// <summary>Gets the set of qualifiers defined on the property.          </summary>
		/// <returns>Returns a <see cref="T:System.Management.QualifierDataCollection" /> containing the set of qualifiers defined on the property.</returns>
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000297 RID: 663 RVA: 0x0000DE4E File Offset: 0x0000C04E
		public QualifierDataCollection Qualifiers
		{
			get
			{
				if (this.qualifiers == null)
				{
					this.qualifiers = new QualifierDataCollection(this.parent, this.propertyName, QualifierType.PropertyQualifier);
				}
				return this.qualifiers;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000298 RID: 664 RVA: 0x0000DE76 File Offset: 0x0000C076
		// (set) Token: 0x06000299 RID: 665 RVA: 0x0000DE7E File Offset: 0x0000C07E
		internal long NullEnumValue
		{
			get
			{
				return this.propertyNullEnumValue;
			}
			set
			{
				this.propertyNullEnumValue = value;
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000DE88 File Offset: 0x0000C088
		internal static object MapWmiValueToValue(object wmiValue, CimType type, bool isArray)
		{
			object obj = null;
			if (DBNull.Value != wmiValue && wmiValue != null)
			{
				if (isArray)
				{
					Array array = (Array)wmiValue;
					int length = array.Length;
					switch (type)
					{
					case CimType.Object:
						obj = new ManagementBaseObject[length];
						for (int i = 0; i < length; i++)
						{
							((ManagementBaseObject[])obj)[i] = new ManagementBaseObject(new IWbemClassObjectFreeThreaded(Marshal.GetIUnknownForObject(array.GetValue(i))));
						}
						return obj;
					case (CimType)14:
					case (CimType)15:
					case CimType.UInt8:
						break;
					case CimType.SInt8:
						obj = new sbyte[length];
						for (int j = 0; j < length; j++)
						{
							((sbyte[])obj)[j] = (sbyte)((short)array.GetValue(j));
						}
						return obj;
					case CimType.UInt16:
						obj = new ushort[length];
						for (int k = 0; k < length; k++)
						{
							((ushort[])obj)[k] = (ushort)((int)array.GetValue(k));
						}
						return obj;
					case CimType.UInt32:
						obj = new uint[length];
						for (int l = 0; l < length; l++)
						{
							((uint[])obj)[l] = (uint)((int)array.GetValue(l));
						}
						return obj;
					case CimType.SInt64:
						obj = new long[length];
						for (int m = 0; m < length; m++)
						{
							((long[])obj)[m] = Convert.ToInt64((string)array.GetValue(m), (IFormatProvider)CultureInfo.CurrentCulture.GetFormat(typeof(long)));
						}
						return obj;
					case CimType.UInt64:
						obj = new ulong[length];
						for (int n = 0; n < length; n++)
						{
							((ulong[])obj)[n] = Convert.ToUInt64((string)array.GetValue(n), (IFormatProvider)CultureInfo.CurrentCulture.GetFormat(typeof(ulong)));
						}
						return obj;
					default:
						if (type == CimType.Char16)
						{
							obj = new char[length];
							for (int num = 0; num < length; num++)
							{
								((char[])obj)[num] = (char)((short)array.GetValue(num));
							}
							return obj;
						}
						break;
					}
					obj = wmiValue;
				}
				else
				{
					switch (type)
					{
					case CimType.Object:
						return new ManagementBaseObject(new IWbemClassObjectFreeThreaded(Marshal.GetIUnknownForObject(wmiValue)));
					case (CimType)14:
					case (CimType)15:
					case CimType.UInt8:
						break;
					case CimType.SInt8:
						return (sbyte)((short)wmiValue);
					case CimType.UInt16:
						return (ushort)((int)wmiValue);
					case CimType.UInt32:
						return (uint)((int)wmiValue);
					case CimType.SInt64:
						return Convert.ToInt64((string)wmiValue, (IFormatProvider)CultureInfo.CurrentCulture.GetFormat(typeof(long)));
					case CimType.UInt64:
						return Convert.ToUInt64((string)wmiValue, (IFormatProvider)CultureInfo.CurrentCulture.GetFormat(typeof(ulong)));
					default:
						if (type == CimType.Char16)
						{
							return (char)((short)wmiValue);
						}
						break;
					}
					obj = wmiValue;
				}
			}
			return obj;
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000E188 File Offset: 0x0000C388
		internal static object MapValueToWmiValue(object val, CimType type, bool isArray)
		{
			object obj = DBNull.Value;
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			if (val != null)
			{
				if (isArray)
				{
					Array array = (Array)val;
					int length = array.Length;
					switch (type)
					{
					case CimType.SInt16:
						if (val is short[])
						{
							return val;
						}
						obj = new short[length];
						for (int i = 0; i < length; i++)
						{
							((short[])obj)[i] = Convert.ToInt16(array.GetValue(i), (IFormatProvider)invariantCulture.GetFormat(typeof(short)));
						}
						return obj;
					case CimType.SInt32:
						if (val is int[])
						{
							return val;
						}
						obj = new int[length];
						for (int j = 0; j < length; j++)
						{
							((int[])obj)[j] = Convert.ToInt32(array.GetValue(j), (IFormatProvider)invariantCulture.GetFormat(typeof(int)));
						}
						return obj;
					case CimType.Real32:
						if (val is float[])
						{
							return val;
						}
						obj = new float[length];
						for (int k = 0; k < length; k++)
						{
							((float[])obj)[k] = Convert.ToSingle(array.GetValue(k), (IFormatProvider)invariantCulture.GetFormat(typeof(float)));
						}
						return obj;
					case CimType.Real64:
						if (val is double[])
						{
							return val;
						}
						obj = new double[length];
						for (int l = 0; l < length; l++)
						{
							((double[])obj)[l] = Convert.ToDouble(array.GetValue(l), (IFormatProvider)invariantCulture.GetFormat(typeof(double)));
						}
						return obj;
					case (CimType)6:
					case (CimType)7:
					case (CimType)9:
					case (CimType)10:
					case (CimType)12:
					case (CimType)14:
					case (CimType)15:
						goto IL_507;
					case CimType.String:
						break;
					case CimType.Boolean:
						if (val is bool[])
						{
							return val;
						}
						obj = new bool[length];
						for (int m = 0; m < length; m++)
						{
							((bool[])obj)[m] = Convert.ToBoolean(array.GetValue(m), (IFormatProvider)invariantCulture.GetFormat(typeof(bool)));
						}
						return obj;
					case CimType.Object:
						obj = new IWbemClassObject_DoNotMarshal[length];
						for (int n = 0; n < length; n++)
						{
							((IWbemClassObject_DoNotMarshal[])obj)[n] = (IWbemClassObject_DoNotMarshal)Marshal.GetObjectForIUnknown(((ManagementBaseObject)array.GetValue(n)).wbemObject);
						}
						return obj;
					case CimType.SInt8:
						obj = new short[length];
						for (int num = 0; num < length; num++)
						{
							((short[])obj)[num] = (short)Convert.ToSByte(array.GetValue(num), (IFormatProvider)invariantCulture.GetFormat(typeof(sbyte)));
						}
						return obj;
					case CimType.UInt8:
						if (val is byte[])
						{
							return val;
						}
						obj = new byte[length];
						for (int num2 = 0; num2 < length; num2++)
						{
							((byte[])obj)[num2] = Convert.ToByte(array.GetValue(num2), (IFormatProvider)invariantCulture.GetFormat(typeof(byte)));
						}
						return obj;
					case CimType.UInt16:
						obj = new int[length];
						for (int num3 = 0; num3 < length; num3++)
						{
							((int[])obj)[num3] = (int)Convert.ToUInt16(array.GetValue(num3), (IFormatProvider)invariantCulture.GetFormat(typeof(ushort)));
						}
						return obj;
					case CimType.UInt32:
						obj = new int[length];
						for (int num4 = 0; num4 < length; num4++)
						{
							((int[])obj)[num4] = (int)Convert.ToUInt32(array.GetValue(num4), (IFormatProvider)invariantCulture.GetFormat(typeof(uint)));
						}
						return obj;
					case CimType.SInt64:
						obj = new string[length];
						for (int num5 = 0; num5 < length; num5++)
						{
							((string[])obj)[num5] = Convert.ToInt64(array.GetValue(num5), (IFormatProvider)invariantCulture.GetFormat(typeof(long))).ToString((IFormatProvider)invariantCulture.GetFormat(typeof(long)));
						}
						return obj;
					case CimType.UInt64:
						obj = new string[length];
						for (int num6 = 0; num6 < length; num6++)
						{
							((string[])obj)[num6] = Convert.ToUInt64(array.GetValue(num6), (IFormatProvider)invariantCulture.GetFormat(typeof(ulong))).ToString((IFormatProvider)invariantCulture.GetFormat(typeof(ulong)));
						}
						return obj;
					default:
						if (type - CimType.DateTime > 1)
						{
							if (type != CimType.Char16)
							{
								goto IL_507;
							}
							obj = new short[length];
							for (int num7 = 0; num7 < length; num7++)
							{
								((short[])obj)[num7] = (short)Convert.ToChar(array.GetValue(num7), (IFormatProvider)invariantCulture.GetFormat(typeof(char)));
							}
							return obj;
						}
						break;
					}
					if (val is string[])
					{
						return val;
					}
					obj = new string[length];
					for (int num8 = 0; num8 < length; num8++)
					{
						((string[])obj)[num8] = array.GetValue(num8).ToString();
					}
					return obj;
					IL_507:
					obj = val;
				}
				else
				{
					switch (type)
					{
					case CimType.SInt16:
						return Convert.ToInt16(val, (IFormatProvider)invariantCulture.GetFormat(typeof(short)));
					case CimType.SInt32:
						return Convert.ToInt32(val, (IFormatProvider)invariantCulture.GetFormat(typeof(int)));
					case CimType.Real32:
						return Convert.ToSingle(val, (IFormatProvider)invariantCulture.GetFormat(typeof(float)));
					case CimType.Real64:
						return Convert.ToDouble(val, (IFormatProvider)invariantCulture.GetFormat(typeof(double)));
					case (CimType)6:
					case (CimType)7:
					case (CimType)9:
					case (CimType)10:
					case (CimType)12:
					case (CimType)14:
					case (CimType)15:
						goto IL_79C;
					case CimType.String:
						break;
					case CimType.Boolean:
						return Convert.ToBoolean(val, (IFormatProvider)invariantCulture.GetFormat(typeof(bool)));
					case CimType.Object:
						if (val is ManagementBaseObject)
						{
							return Marshal.GetObjectForIUnknown(((ManagementBaseObject)val).wbemObject);
						}
						return val;
					case CimType.SInt8:
						return (short)Convert.ToSByte(val, (IFormatProvider)invariantCulture.GetFormat(typeof(short)));
					case CimType.UInt8:
						return Convert.ToByte(val, (IFormatProvider)invariantCulture.GetFormat(typeof(byte)));
					case CimType.UInt16:
						return (int)Convert.ToUInt16(val, (IFormatProvider)invariantCulture.GetFormat(typeof(ushort)));
					case CimType.UInt32:
						return (int)Convert.ToUInt32(val, (IFormatProvider)invariantCulture.GetFormat(typeof(uint)));
					case CimType.SInt64:
						return Convert.ToInt64(val, (IFormatProvider)invariantCulture.GetFormat(typeof(long))).ToString((IFormatProvider)invariantCulture.GetFormat(typeof(long)));
					case CimType.UInt64:
						return Convert.ToUInt64(val, (IFormatProvider)invariantCulture.GetFormat(typeof(ulong))).ToString((IFormatProvider)invariantCulture.GetFormat(typeof(ulong)));
					default:
						if (type - CimType.DateTime > 1)
						{
							if (type != CimType.Char16)
							{
								goto IL_79C;
							}
							return (short)Convert.ToChar(val, (IFormatProvider)invariantCulture.GetFormat(typeof(char)));
						}
						break;
					}
					return val.ToString();
					IL_79C:
					obj = val;
				}
			}
			return obj;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000E934 File Offset: 0x0000CB34
		internal static object MapValueToWmiValue(object val, out bool isArray, out CimType type)
		{
			object obj = DBNull.Value;
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			isArray = false;
			type = CimType.None;
			if (val != null)
			{
				isArray = val.GetType().IsArray;
				Type type2 = val.GetType();
				if (isArray)
				{
					Type elementType = type2.GetElementType();
					if (elementType.IsPrimitive)
					{
						if (elementType == typeof(byte))
						{
							byte[] array = (byte[])val;
							int num = array.Length;
							type = CimType.UInt8;
							obj = new short[num];
							for (int i = 0; i < num; i++)
							{
								((short[])obj)[i] = ((IConvertible)array[i]).ToInt16(null);
							}
						}
						else if (elementType == typeof(sbyte))
						{
							sbyte[] array2 = (sbyte[])val;
							int num2 = array2.Length;
							type = CimType.SInt8;
							obj = new short[num2];
							for (int j = 0; j < num2; j++)
							{
								((short[])obj)[j] = ((IConvertible)array2[j]).ToInt16(null);
							}
						}
						else if (elementType == typeof(bool))
						{
							type = CimType.Boolean;
							obj = (bool[])val;
						}
						else if (elementType == typeof(ushort))
						{
							ushort[] array3 = (ushort[])val;
							int num3 = array3.Length;
							type = CimType.UInt16;
							obj = new int[num3];
							for (int k = 0; k < num3; k++)
							{
								((int[])obj)[k] = ((IConvertible)array3[k]).ToInt32(null);
							}
						}
						else if (elementType == typeof(short))
						{
							type = CimType.SInt16;
							obj = (short[])val;
						}
						else if (elementType == typeof(int))
						{
							type = CimType.SInt32;
							obj = (int[])val;
						}
						else if (elementType == typeof(uint))
						{
							uint[] array4 = (uint[])val;
							int num4 = array4.Length;
							type = CimType.UInt32;
							obj = new string[num4];
							for (int l = 0; l < num4; l++)
							{
								((string[])obj)[l] = array4[l].ToString((IFormatProvider)invariantCulture.GetFormat(typeof(uint)));
							}
						}
						else if (elementType == typeof(ulong))
						{
							ulong[] array5 = (ulong[])val;
							int num5 = array5.Length;
							type = CimType.UInt64;
							obj = new string[num5];
							for (int m = 0; m < num5; m++)
							{
								((string[])obj)[m] = array5[m].ToString((IFormatProvider)invariantCulture.GetFormat(typeof(ulong)));
							}
						}
						else if (elementType == typeof(long))
						{
							long[] array6 = (long[])val;
							int num6 = array6.Length;
							type = CimType.SInt64;
							obj = new string[num6];
							for (int n = 0; n < num6; n++)
							{
								((string[])obj)[n] = array6[n].ToString((IFormatProvider)invariantCulture.GetFormat(typeof(long)));
							}
						}
						else if (elementType == typeof(float))
						{
							type = CimType.Real32;
							obj = (float[])val;
						}
						else if (elementType == typeof(double))
						{
							type = CimType.Real64;
							obj = (double[])val;
						}
						else if (elementType == typeof(char))
						{
							char[] array7 = (char[])val;
							int num7 = array7.Length;
							type = CimType.Char16;
							obj = new short[num7];
							for (int num8 = 0; num8 < num7; num8++)
							{
								((short[])obj)[num8] = ((IConvertible)array7[num8]).ToInt16(null);
							}
						}
					}
					else if (elementType == typeof(string))
					{
						type = CimType.String;
						obj = (string[])val;
					}
					else if (val is ManagementBaseObject[])
					{
						Array array8 = (Array)val;
						int length = array8.Length;
						type = CimType.Object;
						obj = new IWbemClassObject_DoNotMarshal[length];
						for (int num9 = 0; num9 < length; num9++)
						{
							((IWbemClassObject_DoNotMarshal[])obj)[num9] = (IWbemClassObject_DoNotMarshal)Marshal.GetObjectForIUnknown(((ManagementBaseObject)array8.GetValue(num9)).wbemObject);
						}
					}
				}
				else if (type2 == typeof(ushort))
				{
					type = CimType.UInt16;
					obj = ((IConvertible)((ushort)val)).ToInt32(null);
				}
				else if (type2 == typeof(uint))
				{
					type = CimType.UInt32;
					if (((uint)val & 2147483648U) != 0U)
					{
						obj = Convert.ToString(val, (IFormatProvider)invariantCulture.GetFormat(typeof(uint)));
					}
					else
					{
						obj = Convert.ToInt32(val, (IFormatProvider)invariantCulture.GetFormat(typeof(int)));
					}
				}
				else if (type2 == typeof(ulong))
				{
					type = CimType.UInt64;
					obj = ((ulong)val).ToString((IFormatProvider)invariantCulture.GetFormat(typeof(ulong)));
				}
				else if (type2 == typeof(sbyte))
				{
					type = CimType.SInt8;
					obj = ((IConvertible)((sbyte)val)).ToInt16(null);
				}
				else if (type2 == typeof(byte))
				{
					type = CimType.UInt8;
					obj = val;
				}
				else if (type2 == typeof(short))
				{
					type = CimType.SInt16;
					obj = val;
				}
				else if (type2 == typeof(int))
				{
					type = CimType.SInt32;
					obj = val;
				}
				else if (type2 == typeof(long))
				{
					type = CimType.SInt64;
					obj = val.ToString();
				}
				else if (type2 == typeof(bool))
				{
					type = CimType.Boolean;
					obj = val;
				}
				else if (type2 == typeof(float))
				{
					type = CimType.Real32;
					obj = val;
				}
				else if (type2 == typeof(double))
				{
					type = CimType.Real64;
					obj = val;
				}
				else if (type2 == typeof(char))
				{
					type = CimType.Char16;
					obj = ((IConvertible)((char)val)).ToInt16(null);
				}
				else if (type2 == typeof(string))
				{
					type = CimType.String;
					obj = val;
				}
				else if (val is ManagementBaseObject)
				{
					type = CimType.Object;
					obj = Marshal.GetObjectForIUnknown(((ManagementBaseObject)val).wbemObject);
				}
			}
			return obj;
		}

		// Token: 0x040001C5 RID: 453
		private ManagementBaseObject parent;

		// Token: 0x040001C6 RID: 454
		private string propertyName;

		// Token: 0x040001C7 RID: 455
		private object propertyValue;

		// Token: 0x040001C8 RID: 456
		private long propertyNullEnumValue;

		// Token: 0x040001C9 RID: 457
		private int propertyType;

		// Token: 0x040001CA RID: 458
		private int propertyFlavor;

		// Token: 0x040001CB RID: 459
		private QualifierDataCollection qualifiers;
	}
}
