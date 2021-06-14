using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Styles.Assets
{
	// Token: 0x02000006 RID: 6
	public class Location
	{
		// Token: 0x0600001C RID: 28 RVA: 0x0000248D File Offset: 0x0000068D
		public Location(string countryNativeName, string countryEnglishName, string ietfLanguageTag, int geoId)
		{
			this.CountryNativeName = countryNativeName;
			this.CountryEnglishName = countryEnglishName;
			this.IetfLanguageTag = ietfLanguageTag;
			this.GeoId = geoId;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000024BC File Offset: 0x000006BC
		// (set) Token: 0x0600001E RID: 30 RVA: 0x0000250B File Offset: 0x0000070B
		public string CountryNativeName
		{
			get
			{
				string result;
				if (this.countryNativeName == this.CountryEnglishName)
				{
					result = this.countryNativeName;
				}
				else
				{
					result = this.countryNativeName + " (" + this.CountryEnglishName + ")";
				}
				return result;
			}
			private set
			{
				this.countryNativeName = value;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00002518 File Offset: 0x00000718
		// (set) Token: 0x06000020 RID: 32 RVA: 0x0000252F File Offset: 0x0000072F
		public string CountryEnglishName { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002538 File Offset: 0x00000738
		// (set) Token: 0x06000022 RID: 34 RVA: 0x0000254F File Offset: 0x0000074F
		public string IetfLanguageTag { get; private set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002558 File Offset: 0x00000758
		// (set) Token: 0x06000024 RID: 36 RVA: 0x0000256F File Offset: 0x0000076F
		public int GeoId { get; private set; }

		// Token: 0x04000008 RID: 8
		private string countryNativeName;
	}
}
