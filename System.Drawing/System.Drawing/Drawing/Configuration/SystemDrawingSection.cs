using System;
using System.Configuration;

namespace System.Drawing.Configuration
{
	/// <summary>Represents the configuration section used by classes in the <see cref="N:System.Drawing" /> namespace.</summary>
	// Token: 0x02000084 RID: 132
	public sealed class SystemDrawingSection : ConfigurationSection
	{
		// Token: 0x060008C1 RID: 2241 RVA: 0x00021FB8 File Offset: 0x000201B8
		static SystemDrawingSection()
		{
			SystemDrawingSection.properties.Add(SystemDrawingSection.bitmapSuffix);
		}

		/// <summary>Gets or sets the suffix to append to a file name indicated by a <see cref="T:System.Drawing.ToolboxBitmapAttribute" /> when an assembly is declared with a <see cref="T:System.Drawing.BitmapSuffixInSameAssemblyAttribute" /> or a <see cref="T:System.Drawing.BitmapSuffixInSatelliteAssemblyAttribute" />.</summary>
		/// <returns>The bitmap suffix.</returns>
		// Token: 0x17000335 RID: 821
		// (get) Token: 0x060008C2 RID: 2242 RVA: 0x00021FEE File Offset: 0x000201EE
		// (set) Token: 0x060008C3 RID: 2243 RVA: 0x00022000 File Offset: 0x00020200
		[ConfigurationProperty("bitmapSuffix")]
		public string BitmapSuffix
		{
			get
			{
				return (string)base[SystemDrawingSection.bitmapSuffix];
			}
			set
			{
				base[SystemDrawingSection.bitmapSuffix] = value;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x060008C4 RID: 2244 RVA: 0x0002200E File Offset: 0x0002020E
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return SystemDrawingSection.properties;
			}
		}

		// Token: 0x0400071B RID: 1819
		private const string BitmapSuffixSectionName = "bitmapSuffix";

		// Token: 0x0400071C RID: 1820
		private static readonly ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x0400071D RID: 1821
		private static readonly ConfigurationProperty bitmapSuffix = new ConfigurationProperty("bitmapSuffix", typeof(string), null, ConfigurationPropertyOptions.None);
	}
}
