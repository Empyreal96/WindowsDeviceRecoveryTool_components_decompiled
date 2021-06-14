using System;
using System.IO;
using ComponentAce.Compression.Archiver;
using ComponentAce.Compression.Exception;

namespace ComponentAce.Compression.Tar
{
	// Token: 0x0200005B RID: 91
	internal class LegacyTarWriter
	{
		// Token: 0x060003C5 RID: 965 RVA: 0x0001E42C File Offset: 0x0001D42C
		public LegacyTarWriter(Stream writeStream, int codepage, DoOnStreamOperationFailureDelegate writeToStreamFailureDelegate, DoOnStreamOperationFailureDelegate readFromStreamFailureDelegate)
		{
			if (writeStream == null)
			{
				throw new ArgumentNullException("writeStream");
			}
			if (!writeStream.CanWrite)
			{
				throw ExceptionBuilder.Exception(ErrorCode.StreamDoesNotSupportWriting, new object[]
				{
					"writeStream"
				});
			}
			this._codePage = codepage;
			this._outStream = writeStream;
			this._readFromStreamFailureDelegate = readFromStreamFailureDelegate;
			this._writeToStreamFailureDelegate = writeToStreamFailureDelegate;
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x0001E49A File Offset: 0x0001D49A
		protected virtual Stream OutStream
		{
			get
			{
				return this._outStream;
			}
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0001E4A4 File Offset: 0x0001D4A4
		public virtual void Write(Stream data, long dataSizeInBytes, string name, int userId, int groupId, int mode, DateTime lastModificationTime, char typeFlag)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (!data.CanRead)
			{
				throw ExceptionBuilder.Exception(ErrorCode.StreamDoesNotSupportReading, new object[]
				{
					"data"
				});
			}
			this.WriteHeader(name, lastModificationTime, dataSizeInBytes, userId, groupId, mode, typeFlag);
			this.WriteContent(dataSizeInBytes, data);
			this.AlignTo512(dataSizeInBytes, false);
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0001E504 File Offset: 0x0001D504
		public void WriteContent(long count, Stream data)
		{
			while (count > 0L && count > (long)this._buffer.Length)
			{
				int num;
				if (!ReadWriteHelper.ReadFromStream(this._buffer, 0, this._buffer.Length, out num, data, this._readFromStreamFailureDelegate))
				{
					throw ExceptionBuilder.Exception(ErrorCode.ReadFromStreamFailed);
				}
				if (num == 0)
				{
					break;
				}
				if (!ReadWriteHelper.WriteToStream(this._buffer, 0, num, this.OutStream, this._writeToStreamFailureDelegate))
				{
					throw ExceptionBuilder.Exception(ErrorCode.WriteToTheStreamFailed);
				}
				count -= (long)num;
			}
			if (count > 0L)
			{
				int num2;
				if (!ReadWriteHelper.ReadFromStream(this._buffer, 0, (int)count, out num2, data, this._readFromStreamFailureDelegate))
				{
					throw ExceptionBuilder.Exception(ErrorCode.ReadFromStreamFailed);
				}
				if (num2 == 0)
				{
					while (count > 0L)
					{
						byte[] buffer = new byte[1];
						if (!ReadWriteHelper.WriteToStream(buffer, 0, 1, this.OutStream, this._writeToStreamFailureDelegate))
						{
							throw ExceptionBuilder.Exception(ErrorCode.WriteToTheStreamFailed);
						}
						count -= 1L;
					}
					return;
				}
				if (!ReadWriteHelper.WriteToStream(this._buffer, 0, num2, this.OutStream, this._writeToStreamFailureDelegate))
				{
					throw ExceptionBuilder.Exception(ErrorCode.WriteToTheStreamFailed);
				}
			}
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0001E60C File Offset: 0x0001D60C
		internal virtual void WriteHeader(string name, DateTime lastModificationTime, long count, int userId, int groupId, int mode, char typeFlag)
		{
			OldStyleHeader oldStyleHeader = new OldStyleHeader(this._codePage);
			oldStyleHeader.FileName = name;
			oldStyleHeader.LastModification = lastModificationTime;
			oldStyleHeader.SizeInBytes = count;
			oldStyleHeader.UserId = userId;
			oldStyleHeader.GroupId = groupId;
			oldStyleHeader.Mode = mode;
			oldStyleHeader.TypeFlag = typeFlag;
			if (!ReadWriteHelper.WriteToStream(oldStyleHeader.GetHeaderValue(), 0, oldStyleHeader.HeaderSize, this.OutStream, this._writeToStreamFailureDelegate))
			{
				throw ExceptionBuilder.Exception(ErrorCode.WriteToTheStreamFailed);
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0001E684 File Offset: 0x0001D684
		public void AlignTo512(long size, bool acceptZero)
		{
			size %= 512L;
			if (size == 0L && !acceptZero)
			{
				return;
			}
			while (size < 512L)
			{
				byte[] buffer = new byte[1];
				if (!ReadWriteHelper.WriteToStream(buffer, 0, 1, this.OutStream, this._writeToStreamFailureDelegate))
				{
					throw ExceptionBuilder.Exception(ErrorCode.WriteToTheStreamFailed);
				}
				size += 1L;
			}
		}

		// Token: 0x04000276 RID: 630
		public const int DefaultUserId = 61;

		// Token: 0x04000277 RID: 631
		public const int DefaultGroupId = 61;

		// Token: 0x04000278 RID: 632
		public const int DefaultMode = 511;

		// Token: 0x04000279 RID: 633
		protected readonly int _codePage;

		// Token: 0x0400027A RID: 634
		private readonly Stream _outStream;

		// Token: 0x0400027B RID: 635
		protected byte[] _buffer = new byte[1024];

		// Token: 0x0400027C RID: 636
		protected readonly DoOnStreamOperationFailureDelegate _writeToStreamFailureDelegate;

		// Token: 0x0400027D RID: 637
		protected readonly DoOnStreamOperationFailureDelegate _readFromStreamFailureDelegate;
	}
}
