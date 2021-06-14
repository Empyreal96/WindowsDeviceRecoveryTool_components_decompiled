using System;
using System.Security;
using System.Windows;

namespace MS.Internal.Ink
{
	// Token: 0x02000680 RID: 1664
	internal abstract class ClipboardData
	{
		// Token: 0x06006D09 RID: 27913 RVA: 0x001F5481 File Offset: 0x001F3681
		[SecurityCritical]
		internal bool CopyToDataObject(IDataObject dataObject)
		{
			if (this.CanCopy())
			{
				this.DoCopy(dataObject);
				return true;
			}
			return false;
		}

		// Token: 0x06006D0A RID: 27914 RVA: 0x001F5495 File Offset: 0x001F3695
		internal void PasteFromDataObject(IDataObject dataObject)
		{
			if (this.CanPaste(dataObject))
			{
				this.DoPaste(dataObject);
			}
		}

		// Token: 0x06006D0B RID: 27915
		internal abstract bool CanPaste(IDataObject dataObject);

		// Token: 0x06006D0C RID: 27916
		protected abstract bool CanCopy();

		// Token: 0x06006D0D RID: 27917
		protected abstract void DoCopy(IDataObject dataObject);

		// Token: 0x06006D0E RID: 27918
		protected abstract void DoPaste(IDataObject dataObject);
	}
}
