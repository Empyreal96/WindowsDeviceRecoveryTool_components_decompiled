using System;
using System.IO;
using System.Text;
using ComponentAce.Compression.Archiver;
using ComponentAce.Compression.Exception;

namespace ComponentAce.Compression.Tar
{
	// Token: 0x02000067 RID: 103
	internal class TarReader
	{
		// Token: 0x06000472 RID: 1138 RVA: 0x0002009A File Offset: 0x0001F09A
		public TarReader(Stream tarredData, int codepage)
		{
			this._codepage = codepage;
			this._inStream = tarredData;
			this._header = new UnixStandartHeader(this._codepage);
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000473 RID: 1139 RVA: 0x000200D1 File Offset: 0x0001F0D1
		public ITarHeader FileInfo
		{
			get
			{
				return this._header;
			}
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x000200DC File Offset: 0x0001F0DC
		public void Read(Stream dataDestanation)
		{
			byte[] buffer;
			int count;
			while ((count = this.Read(out buffer)) != -1)
			{
				dataDestanation.Write(buffer, 0, count);
			}
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00020104 File Offset: 0x0001F104
		protected int Read(out byte[] buffer)
		{
			if (this._remainingBytesInFile == 0L)
			{
				buffer = null;
				return -1;
			}
			int i = -1;
			long num = this._remainingBytesInFile - 512L;
			if (num > 0L)
			{
				num = 512L;
			}
			else
			{
				i = 512 - (int)this._remainingBytesInFile;
				num = this._remainingBytesInFile;
			}
			int num2;
			if (!ReadWriteHelper.ReadFromStream(this._dataBuffer, 0, (int)num, out num2, this._inStream, new DoOnStreamOperationFailureDelegate(this.DoOnReadFromStreamFailure)))
			{
				throw ExceptionBuilder.Exception(ErrorCode.ReadFromStreamFailed);
			}
			this._remainingBytesInFile -= (long)num2;
			if (this._inStream.CanSeek && i > 0)
			{
				this._inStream.Seek((long)i, SeekOrigin.Current);
			}
			else
			{
				while (i > 0)
				{
					byte[] array = new byte[1];
					int num3;
					if (!ReadWriteHelper.ReadFromStream(array, 0, array.Length, out num3, this._inStream, new DoOnStreamOperationFailureDelegate(this.DoOnReadFromStreamFailure)))
					{
						throw ExceptionBuilder.Exception(ErrorCode.ReadFromStreamFailed);
					}
					i--;
				}
			}
			buffer = this._dataBuffer;
			return num2;
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x000201F8 File Offset: 0x0001F1F8
		private static bool IsEmpty(Array buffer)
		{
			foreach (object obj in buffer)
			{
				byte b = (byte)obj;
				if (b != 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00020250 File Offset: 0x0001F250
		public bool MoveNext(bool skipData)
		{
			if (this._remainingBytesInFile > 0L)
			{
				if (!skipData)
				{
					throw ExceptionBuilder.Exception(ErrorCode.NotAllPrevisiousDataWasRead);
				}
				if (this._inStream.CanSeek)
				{
					long num = this._remainingBytesInFile % 512L;
					this._inStream.Seek(this._remainingBytesInFile + (512L - ((num == 0L) ? 512L : num)), SeekOrigin.Current);
				}
				else
				{
					byte[] array;
					while (this.Read(out array) != -1)
					{
					}
				}
			}
			byte[] bytes = this._header.GetBytes();
			int num2;
			if (!ReadWriteHelper.ReadFromStream(bytes, 0, this._header.HeaderSize, out num2, this._inStream, new DoOnStreamOperationFailureDelegate(this.DoOnReadFromStreamFailure)))
			{
				throw ExceptionBuilder.Exception(ErrorCode.ReadFromStreamFailed);
			}
			if (num2 < 512)
			{
				throw ExceptionBuilder.Exception(ErrorCode.NotAllHeaderBytesWasRead, new object[]
				{
					num2
				});
			}
			if (TarReader.IsEmpty(bytes))
			{
				num2 = this._inStream.Read(bytes, 0, this._header.HeaderSize);
				return num2 == 512 && TarReader.IsEmpty(bytes) && false;
			}
			if (!this._header.UpdateHeaderFromBytes())
			{
				throw ExceptionBuilder.Exception(ErrorCode.InvalidCheckSum);
			}
			if (this._header.TypeFlag == 'L')
			{
				this._remainingBytesInFile = this._header.SizeInBytes;
				MemoryStream memoryStream = new MemoryStream();
				this.Read(memoryStream);
				this.MoveNext(false);
				this._header.UnsafeSetFileName(Encoding.GetEncoding(this._codepage).GetString(memoryStream.ToArray()));
			}
			this._remainingBytesInFile = this._header.SizeInBytes;
			return true;
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x000203DB File Offset: 0x0001F3DB
		protected void DoOnReadFromStreamFailure(Exception innerException, ref bool cancel)
		{
			throw innerException;
		}

		// Token: 0x040002BA RID: 698
		private readonly byte[] _dataBuffer = new byte[512];

		// Token: 0x040002BB RID: 699
		private readonly UnixStandartHeader _header;

		// Token: 0x040002BC RID: 700
		private readonly Stream _inStream;

		// Token: 0x040002BD RID: 701
		private long _remainingBytesInFile;

		// Token: 0x040002BE RID: 702
		private readonly int _codepage;
	}
}
