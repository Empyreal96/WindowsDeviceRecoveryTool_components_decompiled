using System;

namespace System.Management
{
	/// <summary>Holds event data for the <see cref="E:System.Management.ManagementOperationObserver.ObjectReady" /> event.          </summary>
	// Token: 0x0200000F RID: 15
	public class ObjectReadyEventArgs : ManagementEventArgs
	{
		// Token: 0x06000052 RID: 82 RVA: 0x00003E3E File Offset: 0x0000203E
		internal ObjectReadyEventArgs(object context, ManagementBaseObject wmiObject) : base(context)
		{
			this.wmiObject = wmiObject;
		}

		/// <summary>Gets the newly-returned object.          </summary>
		/// <returns>Returns a <see cref="T:System.Management.ManagementBaseObject" /> containing the newly-returned object.</returns>
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00003E4E File Offset: 0x0000204E
		public ManagementBaseObject NewObject
		{
			get
			{
				return this.wmiObject;
			}
		}

		// Token: 0x0400007D RID: 125
		private ManagementBaseObject wmiObject;
	}
}
