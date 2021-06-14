using System;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000712 RID: 1810
	internal class WeakDependencySource
	{
		// Token: 0x060074A0 RID: 29856 RVA: 0x002160E1 File Offset: 0x002142E1
		internal WeakDependencySource(DependencyObject item, DependencyProperty dp)
		{
			this._item = BindingExpressionBase.CreateReference(item);
			this._dp = dp;
		}

		// Token: 0x060074A1 RID: 29857 RVA: 0x002160FC File Offset: 0x002142FC
		internal WeakDependencySource(WeakReference wr, DependencyProperty dp)
		{
			this._item = wr;
			this._dp = dp;
		}

		// Token: 0x17001BBE RID: 7102
		// (get) Token: 0x060074A2 RID: 29858 RVA: 0x00216112 File Offset: 0x00214312
		internal DependencyObject DependencyObject
		{
			get
			{
				return (DependencyObject)BindingExpressionBase.GetReference(this._item);
			}
		}

		// Token: 0x17001BBF RID: 7103
		// (get) Token: 0x060074A3 RID: 29859 RVA: 0x00216124 File Offset: 0x00214324
		internal DependencyProperty DependencyProperty
		{
			get
			{
				return this._dp;
			}
		}

		// Token: 0x040037DC RID: 14300
		private object _item;

		// Token: 0x040037DD RID: 14301
		private DependencyProperty _dp;
	}
}
