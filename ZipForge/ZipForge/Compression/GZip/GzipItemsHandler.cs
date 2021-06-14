using System;
using System.Globalization;
using System.IO;
using ComponentAce.Compression.Archiver;
using ComponentAce.Compression.Exception;
using ComponentAce.Compression.Interfaces;
using ComponentAce.Compression.Tar;

namespace ComponentAce.Compression.GZip
{
	// Token: 0x02000045 RID: 69
	internal class GzipItemsHandler : IItemsHandler
	{
		// Token: 0x060002E3 RID: 739 RVA: 0x00018A90 File Offset: 0x00017A90
		public GzipItemsHandler(Stream gzipStream, DoOnStreamOperationFailureDelegate writeToStreamFailureDelegate, DoOnStreamOperationFailureDelegate readFromStreamFailureDelegate)
		{
			this._codePage = CultureInfo.CurrentCulture.TextInfo.OEMCodePage;
			this._gzipReader = new GzipReader(gzipStream, this._codePage);
			this.ItemsArray = new GzipCompressionItemsArray();
			this.ItemsArrayBackup = new GzipCompressionItemsArray();
			this._writeToStreamFailureDelegate = writeToStreamFailureDelegate;
			this._readFromStreamFailureDelegate = readFromStreamFailureDelegate;
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x00018AEE File Offset: 0x00017AEE
		// (set) Token: 0x060002E5 RID: 741 RVA: 0x00018AF6 File Offset: 0x00017AF6
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

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x00018AFF File Offset: 0x00017AFF
		// (set) Token: 0x060002E7 RID: 743 RVA: 0x00018B07 File Offset: 0x00017B07
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

		// Token: 0x060002E8 RID: 744 RVA: 0x00018B10 File Offset: 0x00017B10
		public void LoadItemsArray()
		{
			GzipItem item = new GzipItem(this._writeToStreamFailureDelegate, this._readFromStreamFailureDelegate);
			this._gzipReader.ReadItem(ref item);
			this.ItemsArray.AddItem(item);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x00018B48 File Offset: 0x00017B48
		public void SaveItemsArray()
		{
			for (int i = 0; i < this.ItemsArray.Count; i++)
			{
				this.SaveItemToStream((this.ItemsArray[i] as GzipItem).DestinationStream, i);
			}
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00018B88 File Offset: 0x00017B88
		public void SaveItemsArray(Stream stream)
		{
			for (int i = 0; i < this.ItemsArray.Count; i++)
			{
				this.SaveItemToStream((this.ItemsArray[i] as GzipItem).DestinationStream, i);
			}
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00018BC8 File Offset: 0x00017BC8
		private void SaveItemToStream(Stream stream, int itemNo)
		{
			stream.Seek(this.ItemsArray[itemNo].RelativeLocalHeaderOffset, SeekOrigin.Begin);
			this.ItemsArray[itemNo].WriteLocalHeaderToStream(stream, 0);
			stream.Seek(this.ItemsArray[itemNo].RelativeLocalHeaderOffset + (long)this.ItemsArray[itemNo].GetLocalHeaderSize() + (this.ItemsArray[itemNo] as GzipItem).CompressedSize, SeekOrigin.Begin);
			byte[] array = new byte[8];
			BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream(array));
			binaryWriter.Write((this.ItemsArray[itemNo] as GzipItem).Crc32);
			binaryWriter.Write((uint)(this.ItemsArray[itemNo].UncompressedSize % 4294967296L));
			if (!ReadWriteHelper.WriteToStream(array, 0, array.Length, stream, null))
			{
				throw ExceptionBuilder.Exception(ErrorCode.WriteToTheStreamFailed);
			}
		}

		// Token: 0x040001EB RID: 491
		private readonly GzipReader _gzipReader;

		// Token: 0x040001EC RID: 492
		private readonly int _codePage;

		// Token: 0x040001ED RID: 493
		private readonly DoOnStreamOperationFailureDelegate _writeToStreamFailureDelegate;

		// Token: 0x040001EE RID: 494
		private readonly DoOnStreamOperationFailureDelegate _readFromStreamFailureDelegate;

		// Token: 0x040001EF RID: 495
		private IItemsArray _itemsArray;

		// Token: 0x040001F0 RID: 496
		private IItemsArray _itemsArrayBackup;
	}
}
