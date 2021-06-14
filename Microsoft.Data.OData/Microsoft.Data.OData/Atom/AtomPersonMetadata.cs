using System;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x0200027E RID: 638
	public sealed class AtomPersonMetadata : ODataAnnotatable
	{
		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06001534 RID: 5428 RVA: 0x0004DA44 File Offset: 0x0004BC44
		// (set) Token: 0x06001535 RID: 5429 RVA: 0x0004DA4C File Offset: 0x0004BC4C
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06001536 RID: 5430 RVA: 0x0004DA55 File Offset: 0x0004BC55
		// (set) Token: 0x06001537 RID: 5431 RVA: 0x0004DA5D File Offset: 0x0004BC5D
		public Uri Uri { get; set; }

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06001538 RID: 5432 RVA: 0x0004DA66 File Offset: 0x0004BC66
		// (set) Token: 0x06001539 RID: 5433 RVA: 0x0004DA6E File Offset: 0x0004BC6E
		public string Email
		{
			get
			{
				return this.email;
			}
			set
			{
				this.email = value;
			}
		}

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x0600153A RID: 5434 RVA: 0x0004DA77 File Offset: 0x0004BC77
		// (set) Token: 0x0600153B RID: 5435 RVA: 0x0004DA7F File Offset: 0x0004BC7F
		internal string UriFromEpm
		{
			get
			{
				return this.uriFromEpm;
			}
			set
			{
				this.uriFromEpm = value;
			}
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x0004DA88 File Offset: 0x0004BC88
		public static AtomPersonMetadata ToAtomPersonMetadata(string name)
		{
			return new AtomPersonMetadata
			{
				Name = name
			};
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x0004DAA3 File Offset: 0x0004BCA3
		public static implicit operator AtomPersonMetadata(string name)
		{
			return AtomPersonMetadata.ToAtomPersonMetadata(name);
		}

		// Token: 0x040007BD RID: 1981
		private string name;

		// Token: 0x040007BE RID: 1982
		private string email;

		// Token: 0x040007BF RID: 1983
		private string uriFromEpm;
	}
}
