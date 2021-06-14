using System;
using System.Xaml;
using MS.Internal.Xaml.Context;

namespace System.Windows.Markup
{
	// Token: 0x0200026F RID: 623
	internal class WpfXamlFrame : XamlFrame
	{
		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x060023AF RID: 9135 RVA: 0x000AE82C File Offset: 0x000ACA2C
		// (set) Token: 0x060023B0 RID: 9136 RVA: 0x000AE834 File Offset: 0x000ACA34
		public bool FreezeFreezable { get; set; }

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x060023B1 RID: 9137 RVA: 0x000AE83D File Offset: 0x000ACA3D
		// (set) Token: 0x060023B2 RID: 9138 RVA: 0x000AE845 File Offset: 0x000ACA45
		public XamlMember Property { get; set; }

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x060023B3 RID: 9139 RVA: 0x000AE84E File Offset: 0x000ACA4E
		// (set) Token: 0x060023B4 RID: 9140 RVA: 0x000AE856 File Offset: 0x000ACA56
		public XamlType Type { get; set; }

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x060023B5 RID: 9141 RVA: 0x000AE85F File Offset: 0x000ACA5F
		// (set) Token: 0x060023B6 RID: 9142 RVA: 0x000AE867 File Offset: 0x000ACA67
		public object Instance { get; set; }

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x060023B7 RID: 9143 RVA: 0x000AE870 File Offset: 0x000ACA70
		// (set) Token: 0x060023B8 RID: 9144 RVA: 0x000AE878 File Offset: 0x000ACA78
		public XmlnsDictionary XmlnsDictionary { get; set; }

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x060023B9 RID: 9145 RVA: 0x000AE881 File Offset: 0x000ACA81
		// (set) Token: 0x060023BA RID: 9146 RVA: 0x000AE889 File Offset: 0x000ACA89
		public bool? XmlSpace { get; set; }

		// Token: 0x060023BB RID: 9147 RVA: 0x000AE894 File Offset: 0x000ACA94
		public override void Reset()
		{
			this.Type = null;
			this.Property = null;
			this.Instance = null;
			this.XmlnsDictionary = null;
			this.XmlSpace = null;
			if (this.FreezeFreezable)
			{
				this.FreezeFreezable = false;
			}
		}
	}
}
