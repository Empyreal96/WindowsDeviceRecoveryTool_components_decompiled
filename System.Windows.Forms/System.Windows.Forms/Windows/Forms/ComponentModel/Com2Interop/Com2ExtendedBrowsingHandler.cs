using System;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x020004A6 RID: 1190
	internal abstract class Com2ExtendedBrowsingHandler
	{
		// Token: 0x170013D9 RID: 5081
		// (get) Token: 0x06005063 RID: 20579
		public abstract Type Interface { get; }

		// Token: 0x06005064 RID: 20580 RVA: 0x0014D009 File Offset: 0x0014B209
		public virtual void SetupPropertyHandlers(Com2PropertyDescriptor propDesc)
		{
			this.SetupPropertyHandlers(new Com2PropertyDescriptor[]
			{
				propDesc
			});
		}

		// Token: 0x06005065 RID: 20581
		public abstract void SetupPropertyHandlers(Com2PropertyDescriptor[] propDesc);
	}
}
