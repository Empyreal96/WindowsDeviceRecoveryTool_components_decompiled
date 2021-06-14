using System;

namespace System.Data.Services.Client
{
	// Token: 0x02000067 RID: 103
	public sealed class LinkInfo
	{
		// Token: 0x06000366 RID: 870 RVA: 0x0000EC09 File Offset: 0x0000CE09
		internal LinkInfo(string propertyName)
		{
			this.name = propertyName;
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000367 RID: 871 RVA: 0x0000EC18 File Offset: 0x0000CE18
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000368 RID: 872 RVA: 0x0000EC20 File Offset: 0x0000CE20
		// (set) Token: 0x06000369 RID: 873 RVA: 0x0000EC28 File Offset: 0x0000CE28
		public Uri NavigationLink
		{
			get
			{
				return this.navigationLink;
			}
			internal set
			{
				this.navigationLink = value;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600036A RID: 874 RVA: 0x0000EC31 File Offset: 0x0000CE31
		// (set) Token: 0x0600036B RID: 875 RVA: 0x0000EC39 File Offset: 0x0000CE39
		public Uri AssociationLink
		{
			get
			{
				return this.associationLink;
			}
			internal set
			{
				this.associationLink = value;
			}
		}

		// Token: 0x04000297 RID: 663
		private Uri navigationLink;

		// Token: 0x04000298 RID: 664
		private Uri associationLink;

		// Token: 0x04000299 RID: 665
		private string name;
	}
}
