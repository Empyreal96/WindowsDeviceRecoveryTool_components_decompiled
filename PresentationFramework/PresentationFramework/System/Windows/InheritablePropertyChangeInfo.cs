using System;

namespace System.Windows
{
	// Token: 0x020000D0 RID: 208
	internal struct InheritablePropertyChangeInfo
	{
		// Token: 0x06000746 RID: 1862 RVA: 0x00016CED File Offset: 0x00014EED
		internal InheritablePropertyChangeInfo(DependencyObject rootElement, DependencyProperty property, EffectiveValueEntry oldEntry, EffectiveValueEntry newEntry)
		{
			this._rootElement = rootElement;
			this._property = property;
			this._oldEntry = oldEntry;
			this._newEntry = newEntry;
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000747 RID: 1863 RVA: 0x00016D0C File Offset: 0x00014F0C
		internal DependencyObject RootElement
		{
			get
			{
				return this._rootElement;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000748 RID: 1864 RVA: 0x00016D14 File Offset: 0x00014F14
		internal DependencyProperty Property
		{
			get
			{
				return this._property;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000749 RID: 1865 RVA: 0x00016D1C File Offset: 0x00014F1C
		internal EffectiveValueEntry OldEntry
		{
			get
			{
				return this._oldEntry;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x0600074A RID: 1866 RVA: 0x00016D24 File Offset: 0x00014F24
		internal EffectiveValueEntry NewEntry
		{
			get
			{
				return this._newEntry;
			}
		}

		// Token: 0x04000729 RID: 1833
		private DependencyObject _rootElement;

		// Token: 0x0400072A RID: 1834
		private DependencyProperty _property;

		// Token: 0x0400072B RID: 1835
		private EffectiveValueEntry _oldEntry;

		// Token: 0x0400072C RID: 1836
		private EffectiveValueEntry _newEntry;
	}
}
