using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management
{
	/// <summary>Represents management exceptions.          </summary>
	// Token: 0x0200001A RID: 26
	[Serializable]
	public class ManagementException : SystemException
	{
		// Token: 0x0600008C RID: 140 RVA: 0x00004A80 File Offset: 0x00002C80
		internal static void ThrowWithExtendedInfo(ManagementStatus errorCode)
		{
			ManagementBaseObject managementBaseObject = null;
			string msg = null;
			IWbemClassObjectFreeThreaded errorInfo = WbemErrorInfo.GetErrorInfo();
			if (errorInfo != null)
			{
				managementBaseObject = new ManagementBaseObject(errorInfo);
			}
			if ((msg = ManagementException.GetMessage(errorCode)) == null && managementBaseObject != null)
			{
				try
				{
					msg = (string)managementBaseObject["Description"];
				}
				catch
				{
				}
			}
			throw new ManagementException(errorCode, msg, managementBaseObject);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00004ADC File Offset: 0x00002CDC
		internal static void ThrowWithExtendedInfo(Exception e)
		{
			ManagementBaseObject managementBaseObject = null;
			string msg = null;
			IWbemClassObjectFreeThreaded errorInfo = WbemErrorInfo.GetErrorInfo();
			if (errorInfo != null)
			{
				managementBaseObject = new ManagementBaseObject(errorInfo);
			}
			if ((msg = ManagementException.GetMessage(e)) == null && managementBaseObject != null)
			{
				try
				{
					msg = (string)managementBaseObject["Description"];
				}
				catch
				{
				}
			}
			throw new ManagementException(e, msg, managementBaseObject);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00004B38 File Offset: 0x00002D38
		internal ManagementException(ManagementStatus errorCode, string msg, ManagementBaseObject errObj) : base(msg)
		{
			this.errorCode = errorCode;
			this.errorObject = errObj;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00004B50 File Offset: 0x00002D50
		internal ManagementException(Exception e, string msg, ManagementBaseObject errObj) : base(msg, e)
		{
			try
			{
				if (e is ManagementException)
				{
					this.errorCode = ((ManagementException)e).ErrorCode;
					if (this.errorObject != null)
					{
						this.errorObject = (ManagementBaseObject)((ManagementException)e).errorObject.Clone();
					}
					else
					{
						this.errorObject = null;
					}
				}
				else if (e is COMException)
				{
					this.errorCode = (ManagementStatus)((COMException)e).ErrorCode;
				}
				else
				{
					this.errorCode = (ManagementStatus)base.HResult;
				}
			}
			catch
			{
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementException" /> class that is serializable.          </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> destination for this serialization.</param>
		// Token: 0x06000090 RID: 144 RVA: 0x00004BE8 File Offset: 0x00002DE8
		protected ManagementException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.errorCode = (ManagementStatus)info.GetValue("errorCode", typeof(ManagementStatus));
			this.errorObject = (info.GetValue("errorObject", typeof(ManagementBaseObject)) as ManagementBaseObject);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementException" /> class.          </summary>
		// Token: 0x06000091 RID: 145 RVA: 0x00004C3D File Offset: 0x00002E3D
		public ManagementException() : this(ManagementStatus.Failed, "", null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementException" />              class with a specified error message.          </summary>
		/// <param name="message">The message that describes the error. </param>
		// Token: 0x06000092 RID: 146 RVA: 0x00004C50 File Offset: 0x00002E50
		public ManagementException(string message) : this(ManagementStatus.Failed, message, null)
		{
		}

		/// <summary>Initializes an empty new instance of the <see cref="T:System.Management.ManagementException" /> class. If the <paramref name="innerException" /> parameter is not <see langword="null" />, the current exception is raised in a catch block that handles the inner exception.</summary>
		/// <param name="message">The message that describes the error. </param>
		/// <param name="innerException">The exception that is the cause of the current exception.</param>
		// Token: 0x06000093 RID: 147 RVA: 0x00004C5F File Offset: 0x00002E5F
		public ManagementException(string message, Exception innerException) : this(innerException, message, null)
		{
			if (!(innerException is ManagementException))
			{
				this.errorCode = ManagementStatus.Failed;
			}
		}

		/// <summary>Populates the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the <see cref="T:System.Management.ManagementException" />.          </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> destination for this serialization.</param>
		// Token: 0x06000094 RID: 148 RVA: 0x00004C7D File Offset: 0x00002E7D
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("errorCode", this.errorCode);
			info.AddValue("errorObject", this.errorObject);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004CB0 File Offset: 0x00002EB0
		private static string GetMessage(Exception e)
		{
			string text = null;
			if (e is COMException)
			{
				text = ManagementException.GetMessage((ManagementStatus)((COMException)e).ErrorCode);
			}
			if (text == null)
			{
				text = e.Message;
			}
			return text;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00004CE4 File Offset: 0x00002EE4
		private static string GetMessage(ManagementStatus errorCode)
		{
			string result = null;
			IWbemStatusCodeText wbemStatusCodeText = (IWbemStatusCodeText)new WbemStatusCodeText();
			if (wbemStatusCodeText != null)
			{
				try
				{
					int errorCodeText_ = wbemStatusCodeText.GetErrorCodeText_((int)errorCode, 0U, 1, out result);
					if (errorCodeText_ != 0)
					{
						errorCodeText_ = wbemStatusCodeText.GetErrorCodeText_((int)errorCode, 0U, 0, out result);
					}
				}
				catch
				{
				}
			}
			return result;
		}

		/// <summary>Gets the extended error object provided by WMI.          </summary>
		/// <returns>Returns a <see cref="T:System.Management.ManagementBaseObject" /> that contains extended error information.</returns>
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00004D34 File Offset: 0x00002F34
		public ManagementBaseObject ErrorInformation
		{
			get
			{
				return this.errorObject;
			}
		}

		/// <summary>Gets the error code reported by WMI, which caused this exception.          </summary>
		/// <returns>Returns a <see cref="T:System.Management.ManagementStatus" /> enumeration value that contains the error code.</returns>
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00004D3C File Offset: 0x00002F3C
		public ManagementStatus ErrorCode
		{
			get
			{
				return this.errorCode;
			}
		}

		// Token: 0x04000104 RID: 260
		private ManagementBaseObject errorObject;

		// Token: 0x04000105 RID: 261
		private ManagementStatus errorCode;
	}
}
