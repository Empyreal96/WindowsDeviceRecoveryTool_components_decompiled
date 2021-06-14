using System;
using System.IO;
using System.Text;
using ComponentAce.Compression.Archiver;
using ComponentAce.Compression.Exception;
using ComponentAce.Compression.Tar;

namespace ComponentAce.Compression.GZip
{
	// Token: 0x02000046 RID: 70
	internal class GzipReader
	{
		// Token: 0x060002EC RID: 748 RVA: 0x00018CAD File Offset: 0x00017CAD
		public GzipReader(Stream compressedStream, int codepage)
		{
			this._compressedStream = compressedStream;
			this._codepage = codepage;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x00018CC4 File Offset: 0x00017CC4
		public void ReadItem(ref GzipItem item)
		{
			byte[] array = new byte[GzipHeader.SizeOf()];
			int num;
			if (!ReadWriteHelper.ReadFromStream(array, 0, array.Length, out num, this._compressedStream, new DoOnStreamOperationFailureDelegate(this.DoOnReadFromStreamFailure)))
			{
				throw ExceptionBuilder.Exception(ErrorCode.ReadFromStreamFailed);
			}
			GzipHeader gzipHeader = new GzipHeader();
			gzipHeader.LoadFromByteArray(array);
			item.Id1 = gzipHeader.Id1;
			item.Id2 = gzipHeader.Id2;
			item.CompressionMethod = gzipHeader.CompressionMethod;
			item.Flag = gzipHeader.Flag;
			item.ExtraFlags = gzipHeader.ExtraFlags;
			item.LastFileModificationTime = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(gzipHeader.ModificationTime);
			item.OperationSystem = gzipHeader.OperationSystem;
			if (item.IsFlagBitSet(2))
			{
				this.ReadExtraField(ref item);
			}
			if (item.IsFlagBitSet(3))
			{
				this.ReadFileName(ref item);
			}
			if (item.IsFlagBitSet(4))
			{
				this.ReadComment(ref item);
			}
			if (item.IsFlagBitSet(1))
			{
				this.ReadCrc(ref item);
			}
			item.DestinationStream = this._compressedStream;
			item.NeedToDestroyDestinationStream = false;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00018DE4 File Offset: 0x00017DE4
		private void ReadExtraField(ref GzipItem item)
		{
			byte[] array = new byte[2];
			int num;
			if (!ReadWriteHelper.ReadFromStream(array, 0, array.Length, out num, this._compressedStream, new DoOnStreamOperationFailureDelegate(this.DoOnReadFromStreamFailure)))
			{
				throw ExceptionBuilder.Exception(ErrorCode.ReadFromStreamFailed);
			}
			MemoryStream input = new MemoryStream(array);
			BinaryReader binaryReader = new BinaryReader(input);
			item.ExtraFieldLenToRead = binaryReader.ReadUInt16();
			byte[] array2 = new byte[(int)item.ExtraFieldLenToRead];
			item.ExtraFieldData = (byte[])array2.Clone();
			if (!ReadWriteHelper.ReadFromStream(array2, 0, array2.Length, out num, this._compressedStream, new DoOnStreamOperationFailureDelegate(this.DoOnReadFromStreamFailure)))
			{
				throw ExceptionBuilder.Exception(ErrorCode.ReadFromStreamFailed);
			}
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00018E8C File Offset: 0x00017E8C
		private void ReadFileName(ref GzipItem item)
		{
			byte[] array = new byte[1];
			int num;
			while (ReadWriteHelper.ReadFromStream(array, 0, array.Length, out num, this._compressedStream, new DoOnStreamOperationFailureDelegate(this.DoOnReadFromStreamFailure)))
			{
				if (array[0] != 0)
				{
					GzipItem gzipItem = item;
					gzipItem.Name += Encoding.GetEncoding(this._codepage).GetString(array);
				}
				if (array[0] == 0)
				{
					return;
				}
			}
			throw ExceptionBuilder.Exception(ErrorCode.ReadFromStreamFailed);
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00018EF8 File Offset: 0x00017EF8
		private void ReadComment(ref GzipItem item)
		{
			byte[] array = new byte[1];
			item.Comment = string.Empty;
			int num;
			while (ReadWriteHelper.ReadFromStream(array, 0, array.Length, out num, this._compressedStream, new DoOnStreamOperationFailureDelegate(this.DoOnReadFromStreamFailure)))
			{
				if (array[0] != 0)
				{
					GzipItem gzipItem = item;
					gzipItem.Comment += Encoding.GetEncoding(this._codepage).GetString(array);
				}
				if (array[0] == 0)
				{
					return;
				}
			}
			throw ExceptionBuilder.Exception(ErrorCode.ReadFromStreamFailed);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00018F70 File Offset: 0x00017F70
		private void ReadCrc(ref GzipItem item)
		{
			byte[] array = new byte[2];
			int num;
			if (!ReadWriteHelper.ReadFromStream(array, 0, array.Length, out num, this._compressedStream, new DoOnStreamOperationFailureDelegate(this.DoOnReadFromStreamFailure)))
			{
				throw ExceptionBuilder.Exception(ErrorCode.ReadFromStreamFailed);
			}
			MemoryStream input = new MemoryStream(array);
			BinaryReader binaryReader = new BinaryReader(input);
			item.HeaderCrc = binaryReader.ReadUInt16();
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00018FCB File Offset: 0x00017FCB
		protected void DoOnReadFromStreamFailure(Exception innerException, ref bool cancel)
		{
			throw innerException;
		}

		// Token: 0x040001F1 RID: 497
		private readonly Stream _compressedStream;

		// Token: 0x040001F2 RID: 498
		private readonly int _codepage;
	}
}
