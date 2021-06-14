using System;
using System.IO;
using ComponentAce.Compression.Archiver;
using ComponentAce.Compression.Exception;

namespace ComponentAce.Compression.Tar
{
	// Token: 0x02000060 RID: 96
	internal class ReadWriteHelper
	{
		// Token: 0x06000419 RID: 1049 RVA: 0x0001F1B1 File Offset: 0x0001E1B1
		private ReadWriteHelper()
		{
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0001F1B9 File Offset: 0x0001E1B9
		public static ReadWriteHelper GetInstance()
		{
			if (ReadWriteHelper._instance == null)
			{
				ReadWriteHelper._instance = new ReadWriteHelper();
			}
			return ReadWriteHelper._instance;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0001F1D4 File Offset: 0x0001E1D4
		public static bool WriteToStream(byte[] buffer, int offset, int count, Stream stream, DoOnStreamOperationFailureDelegate writeToStreamFailureDelegate)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (buffer.Length < offset)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (buffer.Length < offset + count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (!stream.CanWrite)
			{
				throw ExceptionBuilder.Exception(ErrorCode.StreamDoesNotSupportWriting, new object[]
				{
					"stream"
				});
			}
			bool flag = false;
			bool flag2;
			do
			{
				try
				{
					stream.Write(buffer, offset, count);
					flag2 = false;
				}
				catch (ObjectDisposedException)
				{
					flag2 = true;
					break;
				}
				catch (IOException innerException)
				{
					if (writeToStreamFailureDelegate == null)
					{
						throw;
					}
					writeToStreamFailureDelegate(innerException, ref flag);
					flag2 = true;
				}
			}
			while (flag2 && !flag);
			return !flag2;
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0001F2B0 File Offset: 0x0001E2B0
		public static bool ReadFromStream(byte[] buffer, int offset, int count, out int readedBytesCount, Stream stream, DoOnStreamOperationFailureDelegate readFromStreamFailureDelegate)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (buffer.Length < offset)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (buffer.Length < offset + count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (!stream.CanRead)
			{
				throw ExceptionBuilder.Exception(ErrorCode.StreamDoesNotSupportReading, new object[]
				{
					"stream"
				});
			}
			readedBytesCount = 0;
			MemoryStream memoryStream = new MemoryStream();
			byte[] buffer2 = new byte[count];
			bool flag = false;
			bool flag2;
			do
			{
				try
				{
					readedBytesCount = stream.Read(buffer2, 0, count);
					if (readedBytesCount > 0)
					{
						memoryStream.Write(buffer2, 0, readedBytesCount);
						while (count - readedBytesCount > 0)
						{
							int num = stream.Read(buffer2, 0, count - readedBytesCount);
							if (num == 0)
							{
								break;
							}
							memoryStream.Write(buffer2, 0, num);
							readedBytesCount += num;
						}
					}
					flag2 = false;
				}
				catch (ObjectDisposedException)
				{
					flag2 = true;
					break;
				}
				catch (IOException innerException)
				{
					if (readFromStreamFailureDelegate == null)
					{
						throw;
					}
					readFromStreamFailureDelegate(innerException, ref flag);
					flag2 = true;
				}
			}
			while (flag2 && !flag);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			if (memoryStream.Length != (long)count)
			{
				throw ExceptionBuilder.Exception(ErrorCode.InvalidReadedBytesCount, new object[]
				{
					count,
					memoryStream.Length
				});
			}
			Buffer.BlockCopy(memoryStream.ToArray(), 0, buffer, offset, (int)memoryStream.Length);
			return !flag2;
		}

		// Token: 0x04000297 RID: 663
		private static ReadWriteHelper _instance;
	}
}
