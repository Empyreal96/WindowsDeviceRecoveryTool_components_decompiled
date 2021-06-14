using System;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000044 RID: 68
	internal sealed class ReferenceAppId
	{
		// Token: 0x06000155 RID: 341 RVA: 0x00006FBA File Offset: 0x000051BA
		internal ReferenceAppId(IReferenceAppId id)
		{
			if (id == null)
			{
				throw new ArgumentNullException();
			}
			this._id = id;
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00006FD2 File Offset: 0x000051D2
		// (set) Token: 0x06000157 RID: 343 RVA: 0x00006FDF File Offset: 0x000051DF
		public string SubscriptionId
		{
			get
			{
				return this._id.get_SubscriptionId();
			}
			set
			{
				this._id.put_SubscriptionId(value);
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00006FED File Offset: 0x000051ED
		// (set) Token: 0x06000159 RID: 345 RVA: 0x00006FFA File Offset: 0x000051FA
		public string Codebase
		{
			get
			{
				return this._id.get_Codebase();
			}
			set
			{
				this._id.put_Codebase(value);
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00007008 File Offset: 0x00005208
		public EnumReferenceIdentity AppPath
		{
			get
			{
				return new EnumReferenceIdentity(this._id.EnumAppPath());
			}
		}

		// Token: 0x04000134 RID: 308
		internal IReferenceAppId _id;
	}
}
