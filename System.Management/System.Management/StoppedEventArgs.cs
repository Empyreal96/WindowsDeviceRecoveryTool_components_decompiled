using System;

namespace System.Management
{
	/// <summary>Holds event data for the <see cref="E:System.Management.ManagementEventWatcher.Stopped" /> event.          </summary>
	// Token: 0x02000014 RID: 20
	public class StoppedEventArgs : ManagementEventArgs
	{
		// Token: 0x0600005F RID: 95 RVA: 0x00003EF2 File Offset: 0x000020F2
		internal StoppedEventArgs(object context, int status) : base(context)
		{
			this.status = status;
		}

		/// <summary>Gets the completion status of the operation.          </summary>
		/// <returns>Returns a <see cref="T:System.Management.ManagementStatus" /> containing the status of the operation.</returns>
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00003F02 File Offset: 0x00002102
		public ManagementStatus Status
		{
			get
			{
				return (ManagementStatus)this.status;
			}
		}

		// Token: 0x04000085 RID: 133
		private int status;
	}
}
