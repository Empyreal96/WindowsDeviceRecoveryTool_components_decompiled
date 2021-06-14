using System;

namespace System.Management
{
	/// <summary>Holds event data for the <see cref="E:System.Management.ManagementOperationObserver.ObjectPut" /> event.          </summary>
	// Token: 0x02000011 RID: 17
	public class ObjectPutEventArgs : ManagementEventArgs
	{
		// Token: 0x06000057 RID: 87 RVA: 0x00003E7D File Offset: 0x0000207D
		internal ObjectPutEventArgs(object context, ManagementPath path) : base(context)
		{
			this.wmiPath = path;
		}

		/// <summary>Gets the identity of the object that has been put.          </summary>
		/// <returns>Returns a <see cref="T:System.Management.ManagementPath" /> containing the path of the object that has been put.</returns>
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00003E8D File Offset: 0x0000208D
		public ManagementPath Path
		{
			get
			{
				return this.wmiPath;
			}
		}

		// Token: 0x04000080 RID: 128
		private ManagementPath wmiPath;
	}
}
