using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	/// <summary>Contains information about a WMI method.          </summary>
	// Token: 0x0200004C RID: 76
	public class MethodData
	{
		// Token: 0x060002C8 RID: 712 RVA: 0x0000FD47 File Offset: 0x0000DF47
		internal MethodData(ManagementObject parent, string methodName)
		{
			this.parent = parent;
			this.methodName = methodName;
			this.RefreshMethodInfo();
			this.qualifiers = null;
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000FD6C File Offset: 0x0000DF6C
		private void RefreshMethodInfo()
		{
			int num = -2147217407;
			try
			{
				num = this.parent.wbemObject.GetMethod_(this.methodName, 0, out this.wmiInParams, out this.wmiOutParams);
			}
			catch (COMException e)
			{
				ManagementException.ThrowWithExtendedInfo(e);
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

		/// <summary>Gets the name of the method.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the name of the method.</returns>
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060002CA RID: 714 RVA: 0x0000FDF4 File Offset: 0x0000DFF4
		public string Name
		{
			get
			{
				if (this.methodName == null)
				{
					return "";
				}
				return this.methodName;
			}
		}

		/// <summary>Gets the input parameters to the method. Each parameter is described as a property in the object. If a parameter is both in and out, it appears in both the <see cref="P:System.Management.MethodData.InParameters" /> and <see cref="P:System.Management.MethodData.OutParameters" /> properties.          </summary>
		/// <returns>Returns a <see cref="T:System.Management.ManagementBaseObject" /> containing the input parameters to the method.</returns>
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0000FE0A File Offset: 0x0000E00A
		public ManagementBaseObject InParameters
		{
			get
			{
				this.RefreshMethodInfo();
				if (this.wmiInParams != null)
				{
					return new ManagementBaseObject(this.wmiInParams);
				}
				return null;
			}
		}

		/// <summary>Gets the output parameters to the method. Each parameter is described as a property in the object. If a parameter is both in and out, it will appear in both the <see cref="P:System.Management.MethodData.InParameters" /> and <see cref="P:System.Management.MethodData.OutParameters" /> properties.          </summary>
		/// <returns>Returns a <see cref="T:System.Management.ManagementBaseObject" /> containing the output parameters for the method.</returns>
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060002CC RID: 716 RVA: 0x0000FE27 File Offset: 0x0000E027
		public ManagementBaseObject OutParameters
		{
			get
			{
				this.RefreshMethodInfo();
				if (this.wmiOutParams != null)
				{
					return new ManagementBaseObject(this.wmiOutParams);
				}
				return null;
			}
		}

		/// <summary>Gets the name of the management class in which the method was first introduced in the class inheritance hierarchy.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the name of the class in which the method was first introduced in the class inheritance hierarchy.</returns>
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060002CD RID: 717 RVA: 0x0000FE44 File Offset: 0x0000E044
		public string Origin
		{
			get
			{
				string result = null;
				int methodOrigin_ = this.parent.wbemObject.GetMethodOrigin_(this.methodName, out result);
				if (methodOrigin_ < 0)
				{
					if (methodOrigin_ == -2147217393)
					{
						result = string.Empty;
					}
					else if (((long)methodOrigin_ & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)methodOrigin_);
					}
					else
					{
						Marshal.ThrowExceptionForHR(methodOrigin_, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
				return result;
			}
		}

		/// <summary>Gets a collection of qualifiers defined in the method. Each element is of type <see cref="T:System.Management.QualifierData" /> and contains information such as the qualifier name, value, and flavor.          </summary>
		/// <returns>Returns a <see cref="T:System.Management.QualifierDataCollection" /> containing the qualifiers for the method.</returns>
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060002CE RID: 718 RVA: 0x0000FEAA File Offset: 0x0000E0AA
		public QualifierDataCollection Qualifiers
		{
			get
			{
				if (this.qualifiers == null)
				{
					this.qualifiers = new QualifierDataCollection(this.parent, this.methodName, QualifierType.MethodQualifier);
				}
				return this.qualifiers;
			}
		}

		// Token: 0x040001D8 RID: 472
		private ManagementObject parent;

		// Token: 0x040001D9 RID: 473
		private string methodName;

		// Token: 0x040001DA RID: 474
		private IWbemClassObjectFreeThreaded wmiInParams;

		// Token: 0x040001DB RID: 475
		private IWbemClassObjectFreeThreaded wmiOutParams;

		// Token: 0x040001DC RID: 476
		private QualifierDataCollection qualifiers;
	}
}
