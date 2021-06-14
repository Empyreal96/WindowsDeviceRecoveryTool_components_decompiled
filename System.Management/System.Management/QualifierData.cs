using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Management
{
	/// <summary>Contains information about a WMI qualifier.          </summary>
	// Token: 0x0200004A RID: 74
	public class QualifierData
	{
		// Token: 0x060002AA RID: 682 RVA: 0x0000F3C6 File Offset: 0x0000D5C6
		internal QualifierData(ManagementBaseObject parent, string propName, string qualName, QualifierType type)
		{
			this.parent = parent;
			this.propertyOrMethodName = propName;
			this.qualifierName = qualName;
			this.qualifierType = type;
			this.RefreshQualifierInfo();
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000F3F4 File Offset: 0x0000D5F4
		private void RefreshQualifierInfo()
		{
			this.qualifierSet = null;
			int num;
			switch (this.qualifierType)
			{
			case QualifierType.ObjectQualifier:
				num = this.parent.wbemObject.GetQualifierSet_(out this.qualifierSet);
				break;
			case QualifierType.PropertyQualifier:
				num = this.parent.wbemObject.GetPropertyQualifierSet_(this.propertyOrMethodName, out this.qualifierSet);
				break;
			case QualifierType.MethodQualifier:
				num = this.parent.wbemObject.GetMethodQualifierSet_(this.propertyOrMethodName, out this.qualifierSet);
				break;
			default:
				throw new ManagementException(ManagementStatus.Unexpected, null, null);
			}
			if (((long)num & (long)((ulong)-2147483648)) == 0L)
			{
				this.qualifierValue = null;
				if (this.qualifierSet != null)
				{
					num = this.qualifierSet.Get_(this.qualifierName, 0, ref this.qualifierValue, ref this.qualifierFlavor);
				}
			}
			if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
			{
				ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
				return;
			}
			if (((long)num & (long)((ulong)-2147483648)) != 0L)
			{
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000F4FC File Offset: 0x0000D6FC
		private static object MapQualValueToWmiValue(object qualVal)
		{
			object obj = DBNull.Value;
			if (qualVal != null)
			{
				if (qualVal is Array)
				{
					if (qualVal is int[] || qualVal is double[] || qualVal is string[] || qualVal is bool[])
					{
						obj = qualVal;
					}
					else
					{
						Array array = (Array)qualVal;
						int length = array.Length;
						Type left = (length > 0) ? array.GetValue(0).GetType() : null;
						if (left == typeof(int))
						{
							obj = new int[length];
							for (int i = 0; i < length; i++)
							{
								((int[])obj)[i] = Convert.ToInt32(array.GetValue(i), (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(int)));
							}
						}
						else if (left == typeof(double))
						{
							obj = new double[length];
							for (int j = 0; j < length; j++)
							{
								((double[])obj)[j] = Convert.ToDouble(array.GetValue(j), (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(double)));
							}
						}
						else if (left == typeof(string))
						{
							obj = new string[length];
							for (int k = 0; k < length; k++)
							{
								((string[])obj)[k] = array.GetValue(k).ToString();
							}
						}
						else if (left == typeof(bool))
						{
							obj = new bool[length];
							for (int l = 0; l < length; l++)
							{
								((bool[])obj)[l] = Convert.ToBoolean(array.GetValue(l), (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(bool)));
							}
						}
						else
						{
							obj = array;
						}
					}
				}
				else
				{
					obj = qualVal;
				}
			}
			return obj;
		}

		/// <summary>Represents the name of the qualifier.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the name of the qualifier.</returns>
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060002AD RID: 685 RVA: 0x0000F6C5 File Offset: 0x0000D8C5
		public string Name
		{
			get
			{
				if (this.qualifierName == null)
				{
					return "";
				}
				return this.qualifierName;
			}
		}

		/// <summary>Gets or sets the value of the qualifier.          </summary>
		/// <returns>Returns an <see cref="T:System.Object" /> value containing the value of the qualifier.</returns>
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060002AE RID: 686 RVA: 0x0000F6DB File Offset: 0x0000D8DB
		// (set) Token: 0x060002AF RID: 687 RVA: 0x0000F6F0 File Offset: 0x0000D8F0
		public object Value
		{
			get
			{
				this.RefreshQualifierInfo();
				return ValueTypeSafety.GetSafeObject(this.qualifierValue);
			}
			set
			{
				this.RefreshQualifierInfo();
				object obj = QualifierData.MapQualValueToWmiValue(value);
				int num = this.qualifierSet.Put_(this.qualifierName, ref obj, this.qualifierFlavor & -97);
				if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					return;
				}
				if (((long)num & (long)((ulong)-2147483648)) != 0L)
				{
					Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the qualifier is amended.          </summary>
		/// <returns>Returns a <see cref="T:System.Boolean" /> value indicating whether the qualifier is amended.</returns>
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x0000F75C File Offset: 0x0000D95C
		// (set) Token: 0x060002B1 RID: 689 RVA: 0x0000F778 File Offset: 0x0000D978
		public bool IsAmended
		{
			get
			{
				this.RefreshQualifierInfo();
				return 128 == (this.qualifierFlavor & 128);
			}
			set
			{
				this.RefreshQualifierInfo();
				int num = this.qualifierFlavor & -97;
				if (value)
				{
					num |= 128;
				}
				else
				{
					num &= -129;
				}
				int num2 = this.qualifierSet.Put_(this.qualifierName, ref this.qualifierValue, num);
				if (((long)num2 & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num2);
					return;
				}
				if (((long)num2 & (long)((ulong)-2147483648)) != 0L)
				{
					Marshal.ThrowExceptionForHR(num2, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
		}

		/// <summary>Gets a value indicating whether the qualifier has been defined locally on this class or has been propagated from a base class.          </summary>
		/// <returns>Returns a <see cref="T:System.Boolean" /> value indicating whether the qualifier has been defined locally on this class or has been propagated from a base class.</returns>
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x0000F7F8 File Offset: 0x0000D9F8
		public bool IsLocal
		{
			get
			{
				this.RefreshQualifierInfo();
				return (this.qualifierFlavor & 96) == 0;
			}
		}

		/// <summary>Gets or sets a value indicating whether the qualifier should be propagated to instances of the class.          </summary>
		/// <returns>Returns a <see cref="T:System.Boolean" /> value indicating whether the qualifier should be propagated to instances of the class.</returns>
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000F80C File Offset: 0x0000DA0C
		// (set) Token: 0x060002B4 RID: 692 RVA: 0x0000F820 File Offset: 0x0000DA20
		public bool PropagatesToInstance
		{
			get
			{
				this.RefreshQualifierInfo();
				return 1 == (this.qualifierFlavor & 1);
			}
			set
			{
				this.RefreshQualifierInfo();
				int num = this.qualifierFlavor & -97;
				if (value)
				{
					num |= 1;
				}
				else
				{
					num &= -2;
				}
				int num2 = this.qualifierSet.Put_(this.qualifierName, ref this.qualifierValue, num);
				if (((long)num2 & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num2);
					return;
				}
				if (((long)num2 & (long)((ulong)-2147483648)) != 0L)
				{
					Marshal.ThrowExceptionForHR(num2, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the qualifier should be propagated to                    subclasses of the class.          </summary>
		/// <returns>Returns a <see cref="T:System.Boolean" /> value indicating whether the qualifier should be propagated to subclasses of the class.</returns>
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x0000F899 File Offset: 0x0000DA99
		// (set) Token: 0x060002B6 RID: 694 RVA: 0x0000F8AC File Offset: 0x0000DAAC
		public bool PropagatesToSubclass
		{
			get
			{
				this.RefreshQualifierInfo();
				return 2 == (this.qualifierFlavor & 2);
			}
			set
			{
				this.RefreshQualifierInfo();
				int num = this.qualifierFlavor & -97;
				if (value)
				{
					num |= 2;
				}
				else
				{
					num &= -3;
				}
				int num2 = this.qualifierSet.Put_(this.qualifierName, ref this.qualifierValue, num);
				if (((long)num2 & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num2);
					return;
				}
				if (((long)num2 & (long)((ulong)-2147483648)) != 0L)
				{
					Marshal.ThrowExceptionForHR(num2, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the value of the qualifier can be overridden when propagated.          </summary>
		/// <returns>Returns a <see cref="T:System.Boolean" /> value indicating whether the value of the qualifier can be overridden when propagated.</returns>
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x0000F925 File Offset: 0x0000DB25
		// (set) Token: 0x060002B8 RID: 696 RVA: 0x0000F93C File Offset: 0x0000DB3C
		public bool IsOverridable
		{
			get
			{
				this.RefreshQualifierInfo();
				return (this.qualifierFlavor & 16) == 0;
			}
			set
			{
				this.RefreshQualifierInfo();
				int num = this.qualifierFlavor & -97;
				if (value)
				{
					num &= -17;
				}
				else
				{
					num |= 16;
				}
				int num2 = this.qualifierSet.Put_(this.qualifierName, ref this.qualifierValue, num);
				if (((long)num2 & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num2);
					return;
				}
				if (((long)num2 & (long)((ulong)-2147483648)) != 0L)
				{
					Marshal.ThrowExceptionForHR(num2, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
		}

		// Token: 0x040001CE RID: 462
		private ManagementBaseObject parent;

		// Token: 0x040001CF RID: 463
		private string propertyOrMethodName;

		// Token: 0x040001D0 RID: 464
		private string qualifierName;

		// Token: 0x040001D1 RID: 465
		private QualifierType qualifierType;

		// Token: 0x040001D2 RID: 466
		private object qualifierValue;

		// Token: 0x040001D3 RID: 467
		private int qualifierFlavor;

		// Token: 0x040001D4 RID: 468
		private IWbemQualifierSetFreeThreaded qualifierSet;
	}
}
