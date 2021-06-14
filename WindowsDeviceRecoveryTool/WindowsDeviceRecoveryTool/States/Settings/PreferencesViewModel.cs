using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Properties;
using Microsoft.WindowsDeviceRecoveryTool.Styles.Assets;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Settings
{
	// Token: 0x020000D9 RID: 217
	[Export]
	public class PreferencesViewModel : BaseViewModel, ICanHandle<ThemeColorChangedMessage>, ICanHandle<LanguageChangedMessage>, ICanHandle
	{
		// Token: 0x060006B2 RID: 1714 RVA: 0x00022F00 File Offset: 0x00021100
		[ImportingConstructor]
		public PreferencesViewModel()
		{
			try
			{
				Tracer<PreferencesViewModel>.LogEntry(".ctor");
				this.styles = StyleLogic.Styles;
				base.RaisePropertyChanged<ReadOnlyCollection<DictionaryStyle>>(() => this.Styles);
				this.FillStyles();
				IEnumerable<CultureInfo> baseList = LocalizationManager.Instance().Languages();
				Collection<ExtendedCultureInfo> source = ExtendedCultureInfo.CreateLanguagesList(baseList);
				this.Languages = CollectionViewSource.GetDefaultView(source);
				this.Languages.SortDescriptions.Add(new SortDescription("ExtendedDisplayName", ListSortDirection.Ascending));
				this.Languages.MoveCurrentTo(LocalizationManager.Instance().CurrentLanguage);
			}
			catch (Exception error)
			{
				Tracer<PreferencesViewModel>.WriteError(error);
				throw;
			}
			finally
			{
				Tracer<PreferencesViewModel>.LogExit(".ctor");
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x00023044 File Offset: 0x00021244
		// (set) Token: 0x060006B4 RID: 1716 RVA: 0x00023068 File Offset: 0x00021268
		public CultureInfo SelectedLanguage
		{
			get
			{
				return this.Languages.CurrentItem as CultureInfo;
			}
			set
			{
				this.Languages.MoveCurrentTo(value);
				if (!LocalizationManager.Instance().CurrentLanguage.Equals(value))
				{
					LocalizationManager.Instance().CurrentLanguage = value;
					ApplicationInfo.CurrentLanguageInRegistry = LocalizationManager.Instance().CurrentLanguage;
					this.StylesView.Refresh();
				}
				this.UpdatePageHeaders();
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060006B5 RID: 1717 RVA: 0x000230C8 File Offset: 0x000212C8
		// (set) Token: 0x060006B6 RID: 1718 RVA: 0x000230EC File Offset: 0x000212EC
		public ThemeStyle SelectedTheme
		{
			get
			{
				return this.GetTheme(Settings.Default.Theme);
			}
			set
			{
				if (value.Name != Settings.Default.Theme)
				{
					Settings.Default.Theme = value.Name;
					base.RaisePropertyChanged<ThemeStyle>(() => this.SelectedTheme);
					if (this.reloadTheme)
					{
						StyleLogic.LoadTheme(Settings.GetSelectedThemeFileName());
					}
				}
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060006B7 RID: 1719 RVA: 0x00023180 File Offset: 0x00021380
		// (set) Token: 0x060006B8 RID: 1720 RVA: 0x0002319C File Offset: 0x0002139C
		public string SelectedColor
		{
			get
			{
				return Settings.Default.Style;
			}
			set
			{
				if (value != Settings.Default.Style)
				{
					Settings.Default.Style = value;
					base.RaisePropertyChanged<string>(() => this.SelectedColor);
					if (this.reloadTheme)
					{
						StyleLogic.LoadTheme(Settings.GetSelectedThemeFileName());
					}
				}
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060006B9 RID: 1721 RVA: 0x00023224 File Offset: 0x00021424
		// (set) Token: 0x060006BA RID: 1722 RVA: 0x0002323C File Offset: 0x0002143C
		public List<ThemeStyle> ThemeList
		{
			get
			{
				return this.themeList;
			}
			set
			{
				base.SetValue<List<ThemeStyle>>(() => this.ThemeList, ref this.themeList, value);
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060006BB RID: 1723 RVA: 0x0002328C File Offset: 0x0002148C
		// (set) Token: 0x060006BC RID: 1724 RVA: 0x000232A4 File Offset: 0x000214A4
		public ICollectionView Languages
		{
			get
			{
				return this.languages;
			}
			set
			{
				base.SetValue<ICollectionView>(() => this.Languages, ref this.languages, value);
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060006BD RID: 1725 RVA: 0x000232F4 File Offset: 0x000214F4
		// (set) Token: 0x060006BE RID: 1726 RVA: 0x0002330C File Offset: 0x0002150C
		public ICollectionView StylesView
		{
			get
			{
				return this.stylesView;
			}
			set
			{
				base.SetValue<ICollectionView>(() => this.StylesView, ref this.stylesView, value);
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060006BF RID: 1727 RVA: 0x0002335C File Offset: 0x0002155C
		public ReadOnlyCollection<DictionaryStyle> Styles
		{
			get
			{
				return new ReadOnlyCollection<DictionaryStyle>(this.styles);
			}
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0002337C File Offset: 0x0002157C
		private void FillStyles()
		{
			this.StylesView = CollectionViewSource.GetDefaultView(this.Styles);
			this.StylesView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
			this.StylesView.MoveCurrentTo(StyleLogic.CurrentStyle);
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x000233C9 File Offset: 0x000215C9
		public override void OnStarted()
		{
			this.UpdatePageHeaders();
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x000233D3 File Offset: 0x000215D3
		public override void OnStopped()
		{
			Settings.Default.Save();
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x000233E4 File Offset: 0x000215E4
		public void Handle(ThemeColorChangedMessage message)
		{
			bool flag = false;
			try
			{
				this.reloadTheme = false;
				if (!string.IsNullOrEmpty(message.Color) && message.Color != Settings.Default.Style)
				{
					this.SelectedColor = message.Color;
					flag = true;
				}
				if (!string.IsNullOrEmpty(message.Theme) && message.Theme != Settings.Default.Theme)
				{
					this.SelectedTheme = this.GetTheme(message.Theme);
					flag = true;
				}
			}
			finally
			{
				this.reloadTheme = true;
				if (flag)
				{
					StyleLogic.LoadTheme(Settings.GetSelectedThemeFileName());
				}
			}
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x000234B4 File Offset: 0x000216B4
		public void Handle(LanguageChangedMessage message)
		{
			if (message.Language != null && !object.Equals(message.Language, LocalizationManager.Instance().CurrentLanguage))
			{
				this.SelectedLanguage = message.Language;
			}
			this.UpdatePageHeaders();
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x0002352C File Offset: 0x0002172C
		private ThemeStyle GetTheme(string name)
		{
			return this.themeList.FirstOrDefault((ThemeStyle t) => t.Name.Equals(name));
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00023562 File Offset: 0x00021762
		private void UpdatePageHeaders()
		{
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("Settings"), LocalizationManager.GetTranslation("Preferences")));
		}

		// Token: 0x040002CE RID: 718
		private readonly IList<DictionaryStyle> styles;

		// Token: 0x040002CF RID: 719
		private ICollectionView languages;

		// Token: 0x040002D0 RID: 720
		private ICollectionView stylesView;

		// Token: 0x040002D1 RID: 721
		private List<ThemeStyle> themeList = new List<ThemeStyle>
		{
			new ThemeStyle("ThemeDark"),
			new ThemeStyle("ThemeLight"),
			new ThemeStyle("ThemeHighContrast")
		};

		// Token: 0x040002D2 RID: 722
		private bool reloadTheme = true;
	}
}
