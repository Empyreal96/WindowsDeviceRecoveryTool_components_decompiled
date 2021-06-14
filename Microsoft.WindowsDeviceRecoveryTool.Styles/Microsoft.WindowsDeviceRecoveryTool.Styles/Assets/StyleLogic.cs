using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows;

namespace Microsoft.WindowsDeviceRecoveryTool.Styles.Assets
{
	// Token: 0x02000007 RID: 7
	public static class StyleLogic
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002578 File Offset: 0x00000778
		public static ReadOnlyCollection<DictionaryStyle> Styles
		{
			get
			{
				return StyleLogic.MetroStyles;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002590 File Offset: 0x00000790
		// (set) Token: 0x06000027 RID: 39 RVA: 0x000025C4 File Offset: 0x000007C4
		public static DictionaryStyle CurrentStyle
		{
			get
			{
				DictionaryStyle result;
				if (StyleLogic.currentStyle == null)
				{
					result = StyleLogic.MetroStyles.FirstOrDefault<DictionaryStyle>();
				}
				else
				{
					result = StyleLogic.currentStyle;
				}
				return result;
			}
			set
			{
				if (value == null || !StyleLogic.MetroStyles.Contains(value))
				{
					StyleLogic.currentStyle = StyleLogic.MetroStyles.FirstOrDefault<DictionaryStyle>();
				}
				else
				{
					StyleLogic.currentStyle = value;
				}
				if (StyleLogic.currentStyle != null)
				{
					Uri resourceLocator = new Uri("Microsoft.WindowsDeviceRecoveryTool.Styles;component/Colors/" + StyleLogic.currentStyle.FileName, UriKind.Relative);
					ResourceDictionary item = Application.LoadComponent(resourceLocator) as ResourceDictionary;
					Application.Current.Resources.MergedDictionaries.Insert(0, item);
					Application.Current.Resources.MergedDictionaries.Remove(Application.Current.Resources.MergedDictionaries[1]);
				}
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000026AC File Offset: 0x000008AC
		public static DictionaryStyle GetStyle(string styleName)
		{
			string value = styleName.ToLower();
			return StyleLogic.MetroStyles.First((DictionaryStyle style) => style.Name.ToLower().Equals(value));
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000026E8 File Offset: 0x000008E8
		public static void RestoreStyle(string styleName)
		{
			DictionaryStyle dictionaryStyle = StyleLogic.FindStyleByName(styleName);
			StyleLogic.CurrentStyle = dictionaryStyle;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002704 File Offset: 0x00000904
		public static DictionaryStyle FindStyleByName(string name)
		{
			foreach (DictionaryStyle dictionaryStyle in StyleLogic.Styles)
			{
				if (string.CompareOrdinal(dictionaryStyle.Name.ToLower(), name.ToLower()) == 0)
				{
					return dictionaryStyle;
				}
			}
			return StyleLogic.CurrentStyle;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002788 File Offset: 0x00000988
		public static void LoadTheme(string name)
		{
			Uri resourceLocator = new Uri("Microsoft.WindowsDeviceRecoveryTool.Styles;component/" + name, UriKind.Relative);
			ResourceDictionary item = Application.LoadComponent(resourceLocator) as ResourceDictionary;
			Application.Current.Resources.MergedDictionaries.Insert(1, item);
			Application.Current.Resources.MergedDictionaries.Remove(Application.Current.Resources.MergedDictionaries[2]);
			StyleLogic.ResetStyles();
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000027FC File Offset: 0x000009FC
		private static void ResetStyles()
		{
			ResourceDictionary item = Application.LoadComponent(new Uri("/Microsoft.WindowsDeviceRecoveryTool.Styles;component/SystemStyles.xaml", UriKind.Relative)) as ResourceDictionary;
			Application.Current.Resources.MergedDictionaries.Insert(2, item);
			Application.Current.Resources.MergedDictionaries.Remove(Application.Current.Resources.MergedDictionaries[3]);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002894 File Offset: 0x00000A94
		public static bool IfStyleExists(string name)
		{
			return StyleLogic.MetroStyles.Any((DictionaryStyle s) => string.Compare(s.Name, name, StringComparison.Ordinal) == 0);
		}

		// Token: 0x0400000C RID: 12
		private static readonly ReadOnlyCollection<DictionaryStyle> MetroStyles = new ReadOnlyCollection<DictionaryStyle>(new List<DictionaryStyle>
		{
			new DictionaryStyle("AccentColorEmerald", "EmeraldDictionary.xaml", Color.FromArgb(0, 138, 0)),
			new DictionaryStyle("AccentColorCobalt", "CobaltDictionary.xaml", Color.FromArgb(0, 80, 239)),
			new DictionaryStyle("AccentColorCrimson", "CrimsonDictionary.xaml", Color.FromArgb(162, 0, 37)),
			new DictionaryStyle("AccentColorMauve", "MauveDictionary.xaml", Color.FromArgb(118, 96, 138)),
			new DictionaryStyle("AccentColorSienna", "SiennaDictionary.xaml", Color.FromArgb(160, 82, 45)),
			new DictionaryStyle("AccentColorIndigo", "IndigoDictionary.xaml", Color.FromArgb(106, 0, 255)),
			new DictionaryStyle("AccentColorBlue", "BlueDictionary.xaml", Color.FromArgb(255, 0, 66, 127)),
			new DictionaryStyle("AccentColorBlack", "BlackDictionary.xaml", Color.Black)
		});

		// Token: 0x0400000D RID: 13
		private static DictionaryStyle currentStyle;
	}
}
