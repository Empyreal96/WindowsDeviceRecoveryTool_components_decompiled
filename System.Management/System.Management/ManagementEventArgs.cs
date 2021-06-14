using System;

namespace System.Management
{
	/// <summary>Represents the virtual base class to hold event data for WMI events.          </summary>
	// Token: 0x0200000E RID: 14
	public abstract class ManagementEventArgs : EventArgs
	{
		// Token: 0x06000050 RID: 80 RVA: 0x00003E27 File Offset: 0x00002027
		internal ManagementEventArgs(object context)
		{
			this.context = context;
		}

		/// <summary>Gets the operation context echoed back                   from the operation that triggered the event.          </summary>
		/// <returns>Returns an <see cref="T:System.Object" /> value for an operation context.</returns>
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00003E36 File Offset: 0x00002036
		public object Context
		{
			get
			{
				return this.context;
			}
		}

		// Token: 0x0400007C RID: 124
		private object context;
	}
}
