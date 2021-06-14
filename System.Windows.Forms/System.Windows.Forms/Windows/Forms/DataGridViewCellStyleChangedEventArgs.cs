using System;

namespace System.Windows.Forms
{
	// Token: 0x020001A3 RID: 419
	internal class DataGridViewCellStyleChangedEventArgs : EventArgs
	{
		// Token: 0x06001B58 RID: 7000 RVA: 0x00088683 File Offset: 0x00086883
		internal DataGridViewCellStyleChangedEventArgs()
		{
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06001B59 RID: 7001 RVA: 0x0008868B File Offset: 0x0008688B
		// (set) Token: 0x06001B5A RID: 7002 RVA: 0x00088693 File Offset: 0x00086893
		internal bool ChangeAffectsPreferredSize
		{
			get
			{
				return this.changeAffectsPreferredSize;
			}
			set
			{
				this.changeAffectsPreferredSize = value;
			}
		}

		// Token: 0x04000C3E RID: 3134
		private bool changeAffectsPreferredSize;
	}
}
