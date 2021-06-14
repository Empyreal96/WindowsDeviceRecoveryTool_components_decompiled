using System;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x0200027C RID: 636
	public sealed class AtomLinkMetadata : ODataAnnotatable
	{
		// Token: 0x0600151E RID: 5406 RVA: 0x0004D8FF File Offset: 0x0004BAFF
		public AtomLinkMetadata()
		{
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x0004D908 File Offset: 0x0004BB08
		internal AtomLinkMetadata(AtomLinkMetadata other)
		{
			if (other == null)
			{
				return;
			}
			this.Relation = other.Relation;
			this.Href = other.Href;
			this.HrefLang = other.HrefLang;
			this.Title = other.Title;
			this.MediaType = other.MediaType;
			this.Length = other.Length;
			this.hrefFromEpm = other.hrefFromEpm;
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06001520 RID: 5408 RVA: 0x0004D973 File Offset: 0x0004BB73
		// (set) Token: 0x06001521 RID: 5409 RVA: 0x0004D97B File Offset: 0x0004BB7B
		public Uri Href { get; set; }

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06001522 RID: 5410 RVA: 0x0004D984 File Offset: 0x0004BB84
		// (set) Token: 0x06001523 RID: 5411 RVA: 0x0004D98C File Offset: 0x0004BB8C
		public string Relation { get; set; }

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06001524 RID: 5412 RVA: 0x0004D995 File Offset: 0x0004BB95
		// (set) Token: 0x06001525 RID: 5413 RVA: 0x0004D99D File Offset: 0x0004BB9D
		public string MediaType { get; set; }

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06001526 RID: 5414 RVA: 0x0004D9A6 File Offset: 0x0004BBA6
		// (set) Token: 0x06001527 RID: 5415 RVA: 0x0004D9AE File Offset: 0x0004BBAE
		public string HrefLang { get; set; }

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06001528 RID: 5416 RVA: 0x0004D9B7 File Offset: 0x0004BBB7
		// (set) Token: 0x06001529 RID: 5417 RVA: 0x0004D9BF File Offset: 0x0004BBBF
		public string Title { get; set; }

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x0600152A RID: 5418 RVA: 0x0004D9C8 File Offset: 0x0004BBC8
		// (set) Token: 0x0600152B RID: 5419 RVA: 0x0004D9D0 File Offset: 0x0004BBD0
		public int? Length { get; set; }

		// Token: 0x040007B3 RID: 1971
		private string hrefFromEpm;
	}
}
