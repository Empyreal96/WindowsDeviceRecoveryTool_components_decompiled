using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Microsoft.WindowsDeviceRecoveryTool.Localization
{
	// Token: 0x02000005 RID: 5
	public class ResourceLocalizationProvider
	{
		// Token: 0x0600001E RID: 30 RVA: 0x0000278C File Offset: 0x0000098C
		public ResourceLocalizationProvider(string baseName, Assembly assembly)
		{
			this.resourceManager = new ResourceManager(baseName, assembly);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000027A4 File Offset: 0x000009A4
		public object Translate(string key, CultureInfo cultureInfo)
		{
			return this.resourceManager.GetString(key, cultureInfo);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00003004 File Offset: 0x00001204
		public IEnumerable<CultureInfo> Languages()
		{
			yield return CultureInfo.GetCultureInfo("ar-SA");
			yield return CultureInfo.GetCultureInfo("cs-CZ");
			yield return CultureInfo.GetCultureInfo("da-DK");
			yield return CultureInfo.GetCultureInfo("de-DE");
			yield return CultureInfo.GetCultureInfo("el-GR");
			yield return CultureInfo.GetCultureInfo("en-US");
			yield return CultureInfo.GetCultureInfo("en-GB");
			yield return CultureInfo.GetCultureInfo("es-US");
			yield return CultureInfo.GetCultureInfo("es-ES");
			yield return CultureInfo.GetCultureInfo("et-EE");
			yield return CultureInfo.GetCultureInfo("fi-FI");
			yield return CultureInfo.GetCultureInfo("fil-PH");
			yield return CultureInfo.GetCultureInfo("fr-FR");
			yield return CultureInfo.GetCultureInfo("fr-CA");
			yield return CultureInfo.GetCultureInfo("he-IL");
			yield return CultureInfo.GetCultureInfo("hr-HR");
			yield return CultureInfo.GetCultureInfo("hu-HU");
			yield return CultureInfo.GetCultureInfo("id-ID");
			yield return CultureInfo.GetCultureInfo("it-IT");
			yield return CultureInfo.GetCultureInfo("ja-JP");
			yield return CultureInfo.GetCultureInfo("ko-KR");
			yield return CultureInfo.GetCultureInfo("lv-LV");
			yield return CultureInfo.GetCultureInfo("lt-LT");
			yield return CultureInfo.GetCultureInfo("ms-MY");
			yield return CultureInfo.GetCultureInfo("nb-NO");
			yield return CultureInfo.GetCultureInfo("nl-NL");
			yield return CultureInfo.GetCultureInfo("pl-PL");
			yield return CultureInfo.GetCultureInfo("pt-BR");
			yield return CultureInfo.GetCultureInfo("pt-PT");
			yield return CultureInfo.GetCultureInfo("ro-RO");
			yield return CultureInfo.GetCultureInfo("ru-RU");
			yield return CultureInfo.GetCultureInfo("sk-SK");
			yield return CultureInfo.GetCultureInfo("sl-SI");
			yield return CultureInfo.GetCultureInfo("sv-SE");
			yield return CultureInfo.GetCultureInfo("th-TH");
			yield return CultureInfo.GetCultureInfo("tr-TR");
			yield return CultureInfo.GetCultureInfo("vi-VN");
			yield return CultureInfo.GetCultureInfo("zh-CN");
			yield return CultureInfo.GetCultureInfo("zh-HK");
			yield return CultureInfo.GetCultureInfo("zh-TW");
			yield break;
		}

		// Token: 0x04000011 RID: 17
		private readonly ResourceManager resourceManager;
	}
}
