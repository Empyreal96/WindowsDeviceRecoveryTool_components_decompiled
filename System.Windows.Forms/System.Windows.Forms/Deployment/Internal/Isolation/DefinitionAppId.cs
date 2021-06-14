using System;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000042 RID: 66
	internal sealed class DefinitionAppId
	{
		// Token: 0x06000149 RID: 329 RVA: 0x00006F49 File Offset: 0x00005149
		internal DefinitionAppId(IDefinitionAppId id)
		{
			if (id == null)
			{
				throw new ArgumentNullException();
			}
			this._id = id;
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600014A RID: 330 RVA: 0x00006F61 File Offset: 0x00005161
		// (set) Token: 0x0600014B RID: 331 RVA: 0x00006F6E File Offset: 0x0000516E
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

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00006F7C File Offset: 0x0000517C
		// (set) Token: 0x0600014D RID: 333 RVA: 0x00006F89 File Offset: 0x00005189
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

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00006F97 File Offset: 0x00005197
		public EnumDefinitionIdentity AppPath
		{
			get
			{
				return new EnumDefinitionIdentity(this._id.EnumAppPath());
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00006FA9 File Offset: 0x000051A9
		private void SetAppPath(IDefinitionIdentity[] Ids)
		{
			this._id.SetAppPath((uint)Ids.Length, Ids);
		}

		// Token: 0x04000133 RID: 307
		internal IDefinitionAppId _id;
	}
}
