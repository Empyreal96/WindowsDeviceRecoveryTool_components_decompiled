using System;
using System.IO;
using ComponentAce.Compression.Archiver;

namespace ComponentAce.Compression.GZip
{
	// Token: 0x0200003A RID: 58
	internal class GzipCompressionItemsArray : CompressionItemsArray
	{
		// Token: 0x0600022E RID: 558 RVA: 0x0001683C File Offset: 0x0001583C
		protected internal override void RemoveAt(int index)
		{
			if (this._dirItemsArray[index] is GzipItem)
			{
				GzipItem gzipItem = this._dirItemsArray[index] as GzipItem;
				if (gzipItem.NeedToDestroyDestinationStream)
				{
					string text = string.Empty;
					if (gzipItem.DestinationStream != null && gzipItem.DestinationStream is FileStream)
					{
						text = (gzipItem.DestinationStream as FileStream).Name;
					}
					if (gzipItem.DestinationStream != null)
					{
						gzipItem.DestinationStream.Close();
					}
					if (!CompressionUtils.IsNullOrEmpty(text) && File.Exists(text))
					{
						try
						{
							File.Delete(text);
						}
						catch
						{
						}
					}
				}
			}
			base.RemoveAt(index);
		}
	}
}
