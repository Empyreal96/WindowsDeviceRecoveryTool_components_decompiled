using System;
using System.Collections.Generic;

namespace System.Windows
{
	// Token: 0x020000E9 RID: 233
	internal struct ResourcesChangeInfo
	{
		// Token: 0x0600083C RID: 2108 RVA: 0x0001AD6E File Offset: 0x00018F6E
		internal ResourcesChangeInfo(object key)
		{
			this._oldDictionaries = null;
			this._newDictionaries = null;
			this._key = key;
			this._container = null;
			this._flags = (ResourcesChangeInfo.PrivateFlags)0;
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0001AD94 File Offset: 0x00018F94
		internal ResourcesChangeInfo(ResourceDictionary oldDictionary, ResourceDictionary newDictionary)
		{
			this._oldDictionaries = null;
			if (oldDictionary != null)
			{
				this._oldDictionaries = new List<ResourceDictionary>(1);
				this._oldDictionaries.Add(oldDictionary);
			}
			this._newDictionaries = null;
			if (newDictionary != null)
			{
				this._newDictionaries = new List<ResourceDictionary>(1);
				this._newDictionaries.Add(newDictionary);
			}
			this._key = null;
			this._container = null;
			this._flags = (ResourcesChangeInfo.PrivateFlags)0;
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0001ADFA File Offset: 0x00018FFA
		internal ResourcesChangeInfo(List<ResourceDictionary> oldDictionaries, List<ResourceDictionary> newDictionaries, bool isStyleResourcesChange, bool isTemplateResourcesChange, DependencyObject container)
		{
			this._oldDictionaries = oldDictionaries;
			this._newDictionaries = newDictionaries;
			this._key = null;
			this._container = container;
			this._flags = (ResourcesChangeInfo.PrivateFlags)0;
			this.IsStyleResourcesChange = isStyleResourcesChange;
			this.IsTemplateResourcesChange = isTemplateResourcesChange;
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x0001AE30 File Offset: 0x00019030
		internal static ResourcesChangeInfo ThemeChangeInfo
		{
			get
			{
				return new ResourcesChangeInfo
				{
					IsThemeChange = true
				};
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000840 RID: 2112 RVA: 0x0001AE50 File Offset: 0x00019050
		internal static ResourcesChangeInfo TreeChangeInfo
		{
			get
			{
				return new ResourcesChangeInfo
				{
					IsTreeChange = true
				};
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000841 RID: 2113 RVA: 0x0001AE70 File Offset: 0x00019070
		internal static ResourcesChangeInfo SysColorsOrSettingsChangeInfo
		{
			get
			{
				return new ResourcesChangeInfo
				{
					IsSysColorsOrSettingsChange = true
				};
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000842 RID: 2114 RVA: 0x0001AE90 File Offset: 0x00019090
		internal static ResourcesChangeInfo CatastrophicDictionaryChangeInfo
		{
			get
			{
				return new ResourcesChangeInfo
				{
					IsCatastrophicDictionaryChange = true
				};
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000843 RID: 2115 RVA: 0x0001AEAE File Offset: 0x000190AE
		// (set) Token: 0x06000844 RID: 2116 RVA: 0x0001AEB7 File Offset: 0x000190B7
		internal bool IsThemeChange
		{
			get
			{
				return this.ReadPrivateFlag(ResourcesChangeInfo.PrivateFlags.IsThemeChange);
			}
			set
			{
				this.WritePrivateFlag(ResourcesChangeInfo.PrivateFlags.IsThemeChange, value);
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000845 RID: 2117 RVA: 0x0001AEC1 File Offset: 0x000190C1
		// (set) Token: 0x06000846 RID: 2118 RVA: 0x0001AECA File Offset: 0x000190CA
		internal bool IsTreeChange
		{
			get
			{
				return this.ReadPrivateFlag(ResourcesChangeInfo.PrivateFlags.IsTreeChange);
			}
			set
			{
				this.WritePrivateFlag(ResourcesChangeInfo.PrivateFlags.IsTreeChange, value);
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000847 RID: 2119 RVA: 0x0001AED4 File Offset: 0x000190D4
		// (set) Token: 0x06000848 RID: 2120 RVA: 0x0001AEDD File Offset: 0x000190DD
		internal bool IsStyleResourcesChange
		{
			get
			{
				return this.ReadPrivateFlag(ResourcesChangeInfo.PrivateFlags.IsStyleResourceChange);
			}
			set
			{
				this.WritePrivateFlag(ResourcesChangeInfo.PrivateFlags.IsStyleResourceChange, value);
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000849 RID: 2121 RVA: 0x0001AEE7 File Offset: 0x000190E7
		// (set) Token: 0x0600084A RID: 2122 RVA: 0x0001AEF0 File Offset: 0x000190F0
		internal bool IsTemplateResourcesChange
		{
			get
			{
				return this.ReadPrivateFlag(ResourcesChangeInfo.PrivateFlags.IsTemplateResourceChange);
			}
			set
			{
				this.WritePrivateFlag(ResourcesChangeInfo.PrivateFlags.IsTemplateResourceChange, value);
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x0600084B RID: 2123 RVA: 0x0001AEFA File Offset: 0x000190FA
		// (set) Token: 0x0600084C RID: 2124 RVA: 0x0001AF04 File Offset: 0x00019104
		internal bool IsSysColorsOrSettingsChange
		{
			get
			{
				return this.ReadPrivateFlag(ResourcesChangeInfo.PrivateFlags.IsSysColorsOrSettingsChange);
			}
			set
			{
				this.WritePrivateFlag(ResourcesChangeInfo.PrivateFlags.IsSysColorsOrSettingsChange, value);
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x0600084D RID: 2125 RVA: 0x0001AF0F File Offset: 0x0001910F
		// (set) Token: 0x0600084E RID: 2126 RVA: 0x0001AF19 File Offset: 0x00019119
		internal bool IsCatastrophicDictionaryChange
		{
			get
			{
				return this.ReadPrivateFlag(ResourcesChangeInfo.PrivateFlags.IsCatastrophicDictionaryChange);
			}
			set
			{
				this.WritePrivateFlag(ResourcesChangeInfo.PrivateFlags.IsCatastrophicDictionaryChange, value);
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x0600084F RID: 2127 RVA: 0x0001AF24 File Offset: 0x00019124
		// (set) Token: 0x06000850 RID: 2128 RVA: 0x0001AF2E File Offset: 0x0001912E
		internal bool IsImplicitDataTemplateChange
		{
			get
			{
				return this.ReadPrivateFlag(ResourcesChangeInfo.PrivateFlags.IsImplicitDataTemplateChange);
			}
			set
			{
				this.WritePrivateFlag(ResourcesChangeInfo.PrivateFlags.IsImplicitDataTemplateChange, value);
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000851 RID: 2129 RVA: 0x0001AF39 File Offset: 0x00019139
		internal bool IsResourceAddOperation
		{
			get
			{
				return this._key != null || (this._newDictionaries != null && this._newDictionaries.Count > 0);
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000852 RID: 2130 RVA: 0x0001AF5D File Offset: 0x0001915D
		internal DependencyObject Container
		{
			get
			{
				return this._container;
			}
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0001AF68 File Offset: 0x00019168
		internal bool Contains(object key, bool isImplicitStyleKey)
		{
			if (this.IsTreeChange || this.IsCatastrophicDictionaryChange)
			{
				return true;
			}
			if (this.IsThemeChange || this.IsSysColorsOrSettingsChange)
			{
				return !isImplicitStyleKey;
			}
			if (this._key != null && object.Equals(this._key, key))
			{
				return true;
			}
			if (this._oldDictionaries != null)
			{
				for (int i = 0; i < this._oldDictionaries.Count; i++)
				{
					if (this._oldDictionaries[i].Contains(key))
					{
						return true;
					}
				}
			}
			if (this._newDictionaries != null)
			{
				for (int j = 0; j < this._newDictionaries.Count; j++)
				{
					if (this._newDictionaries[j].Contains(key))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0001B020 File Offset: 0x00019220
		internal void SetIsImplicitDataTemplateChange()
		{
			bool flag = this.IsCatastrophicDictionaryChange || this._key is DataTemplateKey;
			if (!flag && this._oldDictionaries != null)
			{
				foreach (ResourceDictionary resourceDictionary in this._oldDictionaries)
				{
					if (resourceDictionary.HasImplicitDataTemplates)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag && this._newDictionaries != null)
			{
				foreach (ResourceDictionary resourceDictionary2 in this._newDictionaries)
				{
					if (resourceDictionary2.HasImplicitDataTemplates)
					{
						flag = true;
						break;
					}
				}
			}
			this.IsImplicitDataTemplateChange = flag;
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x0001B0F8 File Offset: 0x000192F8
		private void WritePrivateFlag(ResourcesChangeInfo.PrivateFlags bit, bool value)
		{
			if (value)
			{
				this._flags |= bit;
				return;
			}
			this._flags &= ~bit;
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0001B11C File Offset: 0x0001931C
		private bool ReadPrivateFlag(ResourcesChangeInfo.PrivateFlags bit)
		{
			return (this._flags & bit) > (ResourcesChangeInfo.PrivateFlags)0;
		}

		// Token: 0x04000794 RID: 1940
		private List<ResourceDictionary> _oldDictionaries;

		// Token: 0x04000795 RID: 1941
		private List<ResourceDictionary> _newDictionaries;

		// Token: 0x04000796 RID: 1942
		private object _key;

		// Token: 0x04000797 RID: 1943
		private DependencyObject _container;

		// Token: 0x04000798 RID: 1944
		private ResourcesChangeInfo.PrivateFlags _flags;

		// Token: 0x02000826 RID: 2086
		private enum PrivateFlags : byte
		{
			// Token: 0x04003BE4 RID: 15332
			IsThemeChange = 1,
			// Token: 0x04003BE5 RID: 15333
			IsTreeChange,
			// Token: 0x04003BE6 RID: 15334
			IsStyleResourceChange = 4,
			// Token: 0x04003BE7 RID: 15335
			IsTemplateResourceChange = 8,
			// Token: 0x04003BE8 RID: 15336
			IsSysColorsOrSettingsChange = 16,
			// Token: 0x04003BE9 RID: 15337
			IsCatastrophicDictionaryChange = 32,
			// Token: 0x04003BEA RID: 15338
			IsImplicitDataTemplateChange = 64
		}
	}
}
