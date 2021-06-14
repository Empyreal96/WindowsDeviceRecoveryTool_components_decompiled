using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.WindowsDeviceRecoveryTool.Localization;

namespace Microsoft.WindowsDeviceRecoveryTool.Styles.Assets
{
	// Token: 0x02000005 RID: 5
	public class ExtendedCultureInfo : CultureInfo, INotifyPropertyChanged
	{
		// Token: 0x06000012 RID: 18 RVA: 0x000021E7 File Offset: 0x000003E7
		public ExtendedCultureInfo(CultureInfo cultureInfo) : base(cultureInfo.LCID)
		{
			ExtendedCultureInfo.AllInstance.Add(this);
			LocalizationManager.Instance().LanguageChanged += this.OnLanguageChanged;
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000013 RID: 19 RVA: 0x0000221C File Offset: 0x0000041C
		// (remove) Token: 0x06000014 RID: 20 RVA: 0x00002258 File Offset: 0x00000458
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002294 File Offset: 0x00000494
		public string ExtendedDisplayName
		{
			get
			{
				CultureInfo cultureInfo = this.IsCountryCreated(this) ? this : this.Parent;
				string text = cultureInfo.DisplayName.Replace(" (", ", ").Replace(")", string.Empty);
				string arg = cultureInfo.NativeName.Replace("(", ", ").Replace(")", string.Empty);
				string result;
				if (string.CompareOrdinal(cultureInfo.DisplayName, cultureInfo.NativeName) == 0)
				{
					result = text;
				}
				else if (LocalizationManager.Instance().CurrentLanguage.TextInfo.IsRightToLeft && !this.TextInfo.IsRightToLeft)
				{
					result = string.Format("({0} ({1}", arg, text);
				}
				else
				{
					result = string.Format("{0} ({1})", arg, text);
				}
				return result;
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000023B0 File Offset: 0x000005B0
		private bool IsCountryCreated(CultureInfo language)
		{
			return ExtendedCultureInfo.AllInstance.Any((ExtendedCultureInfo i) => i != language && string.CompareOrdinal(i.TwoLetterISOLanguageName, language.TwoLetterISOLanguageName) == 0);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002400 File Offset: 0x00000600
		public static Collection<ExtendedCultureInfo> CreateLanguagesList(IEnumerable<CultureInfo> baseList)
		{
			List<ExtendedCultureInfo> list = (from language in baseList
			select new ExtendedCultureInfo(language)).ToList<ExtendedCultureInfo>();
			return new Collection<ExtendedCultureInfo>(list);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002443 File Offset: 0x00000643
		private void OnLanguageChanged(object sender, EventArgs eventArgs)
		{
			this.OnPropertyChanged("ExtendedDisplayName");
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002454 File Offset: 0x00000654
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x04000005 RID: 5
		private static readonly List<ExtendedCultureInfo> AllInstance = new List<ExtendedCultureInfo>();
	}
}
