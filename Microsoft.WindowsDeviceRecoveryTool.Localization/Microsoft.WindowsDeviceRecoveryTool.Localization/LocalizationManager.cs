using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Microsoft.WindowsDeviceRecoveryTool.Localization
{
	// Token: 0x02000003 RID: 3
	public class LocalizationManager
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000011 RID: 17 RVA: 0x000024AC File Offset: 0x000006AC
		// (remove) Token: 0x06000012 RID: 18 RVA: 0x000024E8 File Offset: 0x000006E8
		public event EventHandler LanguageChanged;

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002524 File Offset: 0x00000724
		// (set) Token: 0x06000014 RID: 20 RVA: 0x0000254C File Offset: 0x0000074C
		public CultureInfo CurrentLanguage
		{
			get
			{
				return Application.Current.Dispatcher.Thread.CurrentUICulture;
			}
			set
			{
				if (value != Application.Current.Dispatcher.Thread.CurrentUICulture)
				{
					Application.Current.Dispatcher.Thread.CurrentUICulture = value;
					this.OnLanguageChanged();
				}
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002594 File Offset: 0x00000794
		// (set) Token: 0x06000016 RID: 22 RVA: 0x000025AB File Offset: 0x000007AB
		private ResourceLocalizationProvider TranslationProvider { get; set; }

		// Token: 0x06000017 RID: 23 RVA: 0x000025B4 File Offset: 0x000007B4
		public static string GetTranslation(string key)
		{
			return LocalizationManager.Instance().Translate(key).ToString();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000025D8 File Offset: 0x000007D8
		public static LocalizationManager Instance()
		{
			if (LocalizationManager.localizationManager == null)
			{
				LocalizationManager.localizationManager = new LocalizationManager
				{
					TranslationProvider = new ResourceLocalizationProvider("Microsoft.WindowsDeviceRecoveryTool.Localization.Resources.Resources", Assembly.GetExecutingAssembly())
				};
			}
			return LocalizationManager.localizationManager;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002624 File Offset: 0x00000824
		public IEnumerable<CultureInfo> Languages()
		{
			IEnumerable<CultureInfo> result;
			if (this.TranslationProvider != null)
			{
				result = this.TranslationProvider.Languages();
			}
			else
			{
				result = Enumerable.Empty<CultureInfo>();
			}
			return result;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002658 File Offset: 0x00000858
		public object Translate(string key)
		{
			if (this.TranslationProvider != null)
			{
				object obj = this.TranslationProvider.Translate(key, this.CurrentLanguage);
				if (obj != null)
				{
					return obj.ToString();
				}
				string arg = key.Split(new char[]
				{
					'_'
				}).FirstOrDefault<string>();
				obj = this.TranslationProvider.Translate(string.Format("{0}_UnknownError", arg), this.CurrentLanguage);
				if (obj != null)
				{
					return obj.ToString();
				}
			}
			return "KEY: " + key + " NOT FOUND";
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000026FC File Offset: 0x000008FC
		public object EnglishResource(string key)
		{
			if (this.TranslationProvider != null)
			{
				object obj = this.TranslationProvider.Translate(key, new CultureInfo("en-US"));
				if (obj != null)
				{
					return obj.ToString();
				}
			}
			return "KEY: " + key + " NOT FOUND";
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002758 File Offset: 0x00000958
		private void OnLanguageChanged()
		{
			EventHandler languageChanged = this.LanguageChanged;
			if (languageChanged != null)
			{
				languageChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x04000009 RID: 9
		private static LocalizationManager localizationManager;
	}
}
