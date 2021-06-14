using System;

namespace System.Data.Services.Common
{
	// Token: 0x020000F6 RID: 246
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class EntitySetAttribute : Attribute
	{
		// Token: 0x0600082C RID: 2092 RVA: 0x00022E87 File Offset: 0x00021087
		public EntitySetAttribute(string entitySet)
		{
			this.entitySet = entitySet;
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600082D RID: 2093 RVA: 0x00022E96 File Offset: 0x00021096
		public string EntitySet
		{
			get
			{
				return this.entitySet;
			}
		}

		// Token: 0x040004E0 RID: 1248
		private readonly string entitySet;
	}
}
