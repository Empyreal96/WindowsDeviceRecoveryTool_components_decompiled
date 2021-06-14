using System;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Ink;

namespace MS.Internal.Ink
{
	// Token: 0x0200068C RID: 1676
	internal class ISFClipboardData : ClipboardData
	{
		// Token: 0x06006DA9 RID: 28073 RVA: 0x001F68DE File Offset: 0x001F4ADE
		internal ISFClipboardData()
		{
		}

		// Token: 0x06006DAA RID: 28074 RVA: 0x001F809D File Offset: 0x001F629D
		internal ISFClipboardData(StrokeCollection strokes)
		{
			this._strokes = strokes;
		}

		// Token: 0x06006DAB RID: 28075 RVA: 0x001F80AC File Offset: 0x001F62AC
		internal override bool CanPaste(IDataObject dataObject)
		{
			return dataObject.GetDataPresent(StrokeCollection.InkSerializedFormat, false);
		}

		// Token: 0x06006DAC RID: 28076 RVA: 0x001F80BA File Offset: 0x001F62BA
		protected override bool CanCopy()
		{
			return this.Strokes != null && this.Strokes.Count != 0;
		}

		// Token: 0x06006DAD RID: 28077 RVA: 0x001F80D4 File Offset: 0x001F62D4
		[SecurityCritical]
		protected override void DoCopy(IDataObject dataObject)
		{
			MemoryStream memoryStream = new MemoryStream();
			this.Strokes.Save(memoryStream);
			memoryStream.Position = 0L;
			new UIPermission(UIPermissionClipboard.AllClipboard).Assert();
			try
			{
				dataObject.SetData(StrokeCollection.InkSerializedFormat, memoryStream);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x06006DAE RID: 28078 RVA: 0x001F812C File Offset: 0x001F632C
		protected override void DoPaste(IDataObject dataObject)
		{
			MemoryStream memoryStream = dataObject.GetData(StrokeCollection.InkSerializedFormat) as MemoryStream;
			StrokeCollection strokeCollection = null;
			bool flag = false;
			if (memoryStream != null && memoryStream != Stream.Null)
			{
				try
				{
					strokeCollection = new StrokeCollection(memoryStream);
					flag = true;
				}
				catch (ArgumentException)
				{
					flag = false;
				}
			}
			this._strokes = (flag ? strokeCollection : new StrokeCollection());
		}

		// Token: 0x17001A21 RID: 6689
		// (get) Token: 0x06006DAF RID: 28079 RVA: 0x001F818C File Offset: 0x001F638C
		internal StrokeCollection Strokes
		{
			get
			{
				return this._strokes;
			}
		}

		// Token: 0x040035FC RID: 13820
		private StrokeCollection _strokes;
	}
}
