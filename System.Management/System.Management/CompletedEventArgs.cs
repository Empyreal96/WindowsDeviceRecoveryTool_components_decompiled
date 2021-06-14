using System;

namespace System.Management
{
	/// <summary>Holds event data for the <see cref="E:System.Management.ManagementOperationObserver.Completed" /> event.          </summary>
	// Token: 0x02000010 RID: 16
	public class CompletedEventArgs : ManagementEventArgs
	{
		// Token: 0x06000054 RID: 84 RVA: 0x00003E56 File Offset: 0x00002056
		internal CompletedEventArgs(object context, int status, ManagementBaseObject wmiStatusObject) : base(context)
		{
			this.wmiObject = wmiStatusObject;
			this.status = status;
		}

		/// <summary>Gets additional status information within a WMI object. This may be null.          </summary>
		/// <returns>Returns a <see cref="T:System.Management.ManagementBaseObject" /> that contains status information about the completion of an operation.</returns>
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00003E6D File Offset: 0x0000206D
		public ManagementBaseObject StatusObject
		{
			get
			{
				return this.wmiObject;
			}
		}

		/// <summary>Gets the completion status of the operation.          </summary>
		/// <returns>Returns a <see cref="T:System.Management.ManagementStatus" /> enumeration value.</returns>
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00003E75 File Offset: 0x00002075
		public ManagementStatus Status
		{
			get
			{
				return (ManagementStatus)this.status;
			}
		}

		// Token: 0x0400007E RID: 126
		private readonly int status;

		// Token: 0x0400007F RID: 127
		private readonly ManagementBaseObject wmiObject;
	}
}
