using System;
using System.Globalization;
using System.IO;
using ComponentAce.Compression.Archiver;
using ComponentAce.Compression.Interfaces;

namespace ComponentAce.Compression.Tar
{
	// Token: 0x02000066 RID: 102
	internal class TarItemsHandler : IItemsHandler
	{
		// Token: 0x0600046A RID: 1130 RVA: 0x0001FE50 File Offset: 0x0001EE50
		public TarItemsHandler(Stream tarredStream, DoOnStreamOperationFailureDelegate writeToStreamFailureDelegate, DoOnStreamOperationFailureDelegate readFromStreamFailureDelegate)
		{
			this._codePage = CultureInfo.CurrentCulture.TextInfo.OEMCodePage;
			this._compressedStream = tarredStream;
			this._tarReader = new TarReader(tarredStream, this._codePage);
			this.ItemsArray = new CompressionItemsArray();
			this.ItemsArrayBackup = new CompressionItemsArray();
			this._writeToStreamFailureDelegate = writeToStreamFailureDelegate;
			this._readFromStreamFailureDelegate = readFromStreamFailureDelegate;
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x0001FEB5 File Offset: 0x0001EEB5
		// (set) Token: 0x0600046C RID: 1132 RVA: 0x0001FEBD File Offset: 0x0001EEBD
		public IItemsArray ItemsArray
		{
			get
			{
				return this._itemsArray;
			}
			set
			{
				this._itemsArray = value;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600046D RID: 1133 RVA: 0x0001FEC6 File Offset: 0x0001EEC6
		// (set) Token: 0x0600046E RID: 1134 RVA: 0x0001FECE File Offset: 0x0001EECE
		public IItemsArray ItemsArrayBackup
		{
			get
			{
				return this._itemsArrayBackup;
			}
			set
			{
				this._itemsArrayBackup = value;
			}
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0001FED8 File Offset: 0x0001EED8
		public void LoadItemsArray()
		{
			while (this._tarReader.MoveNext(true))
			{
				TarItem tarItem = new TarItem(this._writeToStreamFailureDelegate, this._readFromStreamFailureDelegate);
				tarItem.SrcFileName = (tarItem.Name = this._tarReader.FileInfo.FileName);
				tarItem.LastFileModificationTime = this._tarReader.FileInfo.LastModification;
				tarItem.ExternalAttributes = (FileAttributes)this._tarReader.FileInfo.Mode;
				tarItem.RelativeLocalHeaderOffset = this._compressedStream.Position - (long)tarItem.GetLocalHeaderSize();
				tarItem.GroupId = this._tarReader.FileInfo.GroupId;
				tarItem.GroupName = this._tarReader.FileInfo.GroupName;
				tarItem.UncompressedSize = this._tarReader.FileInfo.SizeInBytes;
				tarItem.UserId = this._tarReader.FileInfo.UserId;
				tarItem.UserName = this._tarReader.FileInfo.UserName;
				tarItem.TypeFlag = this._tarReader.FileInfo.TypeFlag;
				this.ItemsArray.AddItem(tarItem);
			}
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00020001 File Offset: 0x0001F001
		public void SaveItemsArray()
		{
			this.SaveItemsArray(this._compressedStream);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00020010 File Offset: 0x0001F010
		public void SaveItemsArray(Stream stream)
		{
			TarWriter tarWriter = new TarWriter(stream, this._codePage, this._writeToStreamFailureDelegate, this._readFromStreamFailureDelegate);
			long length = stream.Length;
			for (int i = 0; i < this.ItemsArray.Count; i++)
			{
				stream.Seek(this.ItemsArray[i].RelativeLocalHeaderOffset, SeekOrigin.Begin);
				this.ItemsArray[i].WriteLocalHeaderToStream(stream, 0);
			}
			stream.Seek(length, SeekOrigin.Begin);
			tarWriter.AlignTo512(0L, true);
			tarWriter.AlignTo512(0L, true);
		}

		// Token: 0x040002B3 RID: 691
		private readonly Stream _compressedStream;

		// Token: 0x040002B4 RID: 692
		private readonly TarReader _tarReader;

		// Token: 0x040002B5 RID: 693
		private readonly int _codePage;

		// Token: 0x040002B6 RID: 694
		private readonly DoOnStreamOperationFailureDelegate _writeToStreamFailureDelegate;

		// Token: 0x040002B7 RID: 695
		private readonly DoOnStreamOperationFailureDelegate _readFromStreamFailureDelegate;

		// Token: 0x040002B8 RID: 696
		internal IItemsArray _itemsArray;

		// Token: 0x040002B9 RID: 697
		internal IItemsArray _itemsArrayBackup;
	}
}
