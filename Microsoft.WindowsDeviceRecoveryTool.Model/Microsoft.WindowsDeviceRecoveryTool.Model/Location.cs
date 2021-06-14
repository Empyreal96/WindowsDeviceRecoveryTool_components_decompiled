using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Model
{
	// Token: 0x0200004F RID: 79
	internal class Location
	{
		// Token: 0x06000275 RID: 629 RVA: 0x000079CC File Offset: 0x00005BCC
		public Location(string countryEnglishName, string ietfLanguageTag, int geoId)
		{
			this.CountryEnglishName = countryEnglishName;
			this.IetfLanguageTag = ietfLanguageTag;
			this.GeoId = geoId;
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000276 RID: 630 RVA: 0x000079F0 File Offset: 0x00005BF0
		// (set) Token: 0x06000277 RID: 631 RVA: 0x00007A07 File Offset: 0x00005C07
		public string CountryEnglishName { get; set; }

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000278 RID: 632 RVA: 0x00007A10 File Offset: 0x00005C10
		// (set) Token: 0x06000279 RID: 633 RVA: 0x00007A27 File Offset: 0x00005C27
		public string IetfLanguageTag { get; set; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600027A RID: 634 RVA: 0x00007A30 File Offset: 0x00005C30
		// (set) Token: 0x0600027B RID: 635 RVA: 0x00007A47 File Offset: 0x00005C47
		public int GeoId { get; set; }
	}
}
