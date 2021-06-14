using System;

namespace System.Management
{
	/// <summary>Holds event data for the <see cref="E:System.Management.ManagementEventWatcher.EventArrived" /> event.          </summary>
	// Token: 0x02000013 RID: 19
	public class EventArrivedEventArgs : ManagementEventArgs
	{
		// Token: 0x0600005D RID: 93 RVA: 0x00003EDA File Offset: 0x000020DA
		internal EventArrivedEventArgs(object context, ManagementBaseObject eventObject) : base(context)
		{
			this.eventObject = eventObject;
		}

		/// <summary>Gets the WMI event that was delivered.      </summary>
		/// <returns>Returns a <see cref="T:System.Management.ManagementBaseObject" /> that contains the delivered WMI event.</returns>
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00003EEA File Offset: 0x000020EA
		public ManagementBaseObject NewEvent
		{
			get
			{
				return this.eventObject;
			}
		}

		// Token: 0x04000084 RID: 132
		private ManagementBaseObject eventObject;
	}
}
