using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Core
{
	// Token: 0x02000087 RID: 135
	public class MultiBufferMemoryStream : Stream
	{
		// Token: 0x06000F3E RID: 3902 RVA: 0x0003A0E8 File Offset: 0x000382E8
		public MultiBufferMemoryStream(IBufferManager bufferManager, int bufferSize = 65536)
		{
			this.bufferBlocks = new List<byte[]>();
			this.bufferManager = bufferManager;
			this.bufferSize = ((this.bufferManager == null) ? bufferSize : this.bufferManager.GetDefaultBufferSize());
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", "Buffer size must be a positive, non-zero value");
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000F3F RID: 3903 RVA: 0x0003A13D File Offset: 0x0003833D
		public override bool CanRead
		{
			get
			{
				return !this.disposed;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000F40 RID: 3904 RVA: 0x0003A14A File Offset: 0x0003834A
		public override bool CanSeek
		{
			get
			{
				return !this.disposed;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000F41 RID: 3905 RVA: 0x0003A157 File Offset: 0x00038357
		public override bool CanWrite
		{
			get
			{
				return !this.disposed;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000F42 RID: 3906 RVA: 0x0003A164 File Offset: 0x00038364
		public override long Length
		{
			get
			{
				return this.length;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000F43 RID: 3907 RVA: 0x0003A16C File Offset: 0x0003836C
		// (set) Token: 0x06000F44 RID: 3908 RVA: 0x0003A174 File Offset: 0x00038374
		public override long Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.Seek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x0003A17F File Offset: 0x0003837F
		public override int Read(byte[] buffer, int offset, int count)
		{
			CommonUtility.AssertNotNull("buffer", buffer);
			CommonUtility.AssertInBounds<int>("offset", offset, 0, buffer.Length);
			CommonUtility.AssertInBounds<int>("count", count, 0, buffer.Length - offset);
			return this.ReadInternal(buffer, offset, count);
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x0003A1B8 File Offset: 0x000383B8
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("buffer", buffer);
			CommonUtility.AssertInBounds<int>("offset", offset, 0, buffer.Length);
			CommonUtility.AssertInBounds<int>("count", count, 0, buffer.Length - offset);
			StorageAsyncResult<int> storageAsyncResult = new StorageAsyncResult<int>(callback, state);
			try
			{
				storageAsyncResult.Result = this.Read(buffer, offset, count);
				storageAsyncResult.OnComplete();
			}
			catch (Exception exception)
			{
				storageAsyncResult.OnComplete(exception);
			}
			return storageAsyncResult;
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x0003A22C File Offset: 0x0003842C
		public override int EndRead(IAsyncResult asyncResult)
		{
			StorageAsyncResult<int> storageAsyncResult = (StorageAsyncResult<int>)asyncResult;
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x0003A24C File Offset: 0x0003844C
		public override long Seek(long offset, SeekOrigin origin)
		{
			long val;
			switch (origin)
			{
			case SeekOrigin.Begin:
				val = offset;
				break;
			case SeekOrigin.Current:
				val = this.position + offset;
				break;
			case SeekOrigin.End:
				val = this.Length + offset;
				break;
			default:
				CommonUtility.ArgumentOutOfRange("origin", origin);
				throw new ArgumentOutOfRangeException("origin");
			}
			CommonUtility.AssertInBounds<long>("offset", val, 0L, this.Length);
			this.position = val;
			return this.position;
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x0003A2C4 File Offset: 0x000384C4
		public override void SetLength(long value)
		{
			this.Reserve(value);
			this.length = value;
			this.position = Math.Min(this.position, this.length);
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x0003A2EC File Offset: 0x000384EC
		public override void Write(byte[] buffer, int offset, int count)
		{
			CommonUtility.AssertNotNull("buffer", buffer);
			CommonUtility.AssertInBounds<int>("offset", offset, 0, buffer.Length);
			CommonUtility.AssertInBounds<int>("count", count, 0, buffer.Length - offset);
			if (this.position + (long)count > this.capacity)
			{
				this.Reserve(this.position + (long)count);
			}
			this.WriteInternal(buffer, offset, count);
			this.length = Math.Max(this.length, this.position);
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x0003A364 File Offset: 0x00038564
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("buffer", buffer);
			CommonUtility.AssertInBounds<int>("offset", offset, 0, buffer.Length);
			CommonUtility.AssertInBounds<int>("count", count, 0, buffer.Length - offset);
			StorageAsyncResult<NullType> storageAsyncResult = new StorageAsyncResult<NullType>(callback, state);
			try
			{
				this.Write(buffer, offset, count);
				storageAsyncResult.OnComplete();
			}
			catch (Exception exception)
			{
				storageAsyncResult.OnComplete(exception);
			}
			return storageAsyncResult;
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x0003A3D4 File Offset: 0x000385D4
		public override void EndWrite(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult;
			storageAsyncResult.End();
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x0003A3EE File Offset: 0x000385EE
		public override void Flush()
		{
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x0003A3F0 File Offset: 0x000385F0
		public void FastCopyTo(Stream destination, DateTime? expiryTime)
		{
			CommonUtility.AssertNotNull("destination", destination);
			long num = this.Length - this.Position;
			try
			{
				while (num != 0L)
				{
					if (expiryTime != null && DateTime.Now.CompareTo(expiryTime.Value) > 0)
					{
						throw new TimeoutException();
					}
					ArraySegment<byte> currentBlock = this.GetCurrentBlock();
					int num2 = (int)Math.Min(num, (long)currentBlock.Count);
					destination.Write(currentBlock.Array, currentBlock.Offset, num2);
					this.AdvancePosition(ref num, num2);
				}
			}
			catch (Exception)
			{
				if (expiryTime != null && DateTime.Now.CompareTo(expiryTime.Value) > 0)
				{
					throw new TimeoutException();
				}
				throw;
			}
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x0003A4B4 File Offset: 0x000386B4
		public IAsyncResult BeginFastCopyTo(Stream destination, DateTime? expiryTime, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("destination", destination);
			StorageAsyncResult<NullType> storageAsyncResult = new StorageAsyncResult<NullType>(callback, state);
			storageAsyncResult.OperationState = new MultiBufferMemoryStream.CopyState
			{
				Destination = destination,
				ExpiryTime = expiryTime
			};
			this.FastCopyToInternal(storageAsyncResult);
			return storageAsyncResult;
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x0003A4F8 File Offset: 0x000386F8
		private void FastCopyToInternal(StorageAsyncResult<NullType> result)
		{
			MultiBufferMemoryStream.CopyState copyState = (MultiBufferMemoryStream.CopyState)result.OperationState;
			long num = this.Length - this.Position;
			try
			{
				while (num != 0L)
				{
					if (copyState.ExpiryTime != null && DateTime.Now.CompareTo(copyState.ExpiryTime.Value) > 0)
					{
						throw new TimeoutException();
					}
					ArraySegment<byte> currentBlock = this.GetCurrentBlock();
					int num2 = (int)Math.Min(num, (long)currentBlock.Count);
					this.AdvancePosition(ref num, num2);
					IAsyncResult asyncResult = copyState.Destination.BeginWrite(currentBlock.Array, currentBlock.Offset, num2, new AsyncCallback(this.FastCopyToCallback), result);
					if (!asyncResult.CompletedSynchronously)
					{
						return;
					}
					copyState.Destination.EndWrite(asyncResult);
				}
				result.OnComplete();
			}
			catch (Exception exception)
			{
				if (copyState.ExpiryTime != null && DateTime.Now.CompareTo(copyState.ExpiryTime.Value) > 0)
				{
					result.OnComplete(new TimeoutException());
				}
				else
				{
					result.OnComplete(exception);
				}
			}
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x0003A628 File Offset: 0x00038828
		private void FastCopyToCallback(IAsyncResult asyncResult)
		{
			if (asyncResult.CompletedSynchronously)
			{
				return;
			}
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult.AsyncState;
			storageAsyncResult.UpdateCompletedSynchronously(asyncResult.CompletedSynchronously);
			MultiBufferMemoryStream.CopyState copyState = (MultiBufferMemoryStream.CopyState)storageAsyncResult.OperationState;
			try
			{
				copyState.Destination.EndWrite(asyncResult);
				this.FastCopyToInternal(storageAsyncResult);
			}
			catch (Exception exception)
			{
				if (copyState.ExpiryTime != null && DateTime.Now.CompareTo(copyState.ExpiryTime.Value) > 0)
				{
					storageAsyncResult.OnComplete(new TimeoutException());
				}
				else
				{
					storageAsyncResult.OnComplete(exception);
				}
			}
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x0003A6D0 File Offset: 0x000388D0
		public void EndFastCopyTo(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult;
			storageAsyncResult.End();
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x0003A6EC File Offset: 0x000388EC
		public string ComputeMD5Hash()
		{
			string result;
			using (MD5Wrapper md5Wrapper = new MD5Wrapper())
			{
				long num = this.Length - this.Position;
				while (num != 0L)
				{
					ArraySegment<byte> currentBlock = this.GetCurrentBlock();
					int num2 = (int)Math.Min(num, (long)currentBlock.Count);
					md5Wrapper.UpdateHash(currentBlock.Array, currentBlock.Offset, num2);
					this.AdvancePosition(ref num, num2);
				}
				result = md5Wrapper.ComputeHash();
			}
			return result;
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x0003A770 File Offset: 0x00038970
		private void Reserve(long requiredSize)
		{
			if (requiredSize < 0L)
			{
				throw new ArgumentOutOfRangeException("requiredSize", "The size must be positive");
			}
			while (requiredSize > this.capacity)
			{
				this.AddBlock();
			}
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x0003A798 File Offset: 0x00038998
		private void AddBlock()
		{
			byte[] array = (this.bufferManager == null) ? new byte[this.bufferSize] : this.bufferManager.TakeBuffer(this.bufferSize);
			if (array.Length != this.bufferSize)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The IBufferManager provided an incorrect length buffer to the stream, Expected {0}, received {1}. Buffer length should equal the value returned by IBufferManager.GetDefaultBufferSize().", new object[]
				{
					this.bufferSize,
					array.Length
				}));
			}
			this.bufferBlocks.Add(array);
			this.capacity += (long)this.bufferSize;
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x0003A830 File Offset: 0x00038A30
		private int ReadInternal(byte[] buffer, int offset, int count)
		{
			int num = (int)Math.Min(this.Length - this.Position, (long)count);
			int val = num;
			while (val != 0)
			{
				ArraySegment<byte> currentBlock = this.GetCurrentBlock();
				int num2 = Math.Min(val, currentBlock.Count);
				Buffer.BlockCopy(currentBlock.Array, currentBlock.Offset, buffer, offset, num2);
				this.AdvancePosition(ref offset, ref val, num2);
			}
			return num;
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x0003A894 File Offset: 0x00038A94
		private void WriteInternal(byte[] buffer, int offset, int count)
		{
			while (count != 0)
			{
				ArraySegment<byte> currentBlock = this.GetCurrentBlock();
				int num = Math.Min(count, currentBlock.Count);
				Buffer.BlockCopy(buffer, offset, currentBlock.Array, currentBlock.Offset, num);
				this.AdvancePosition(ref offset, ref count, num);
			}
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x0003A8DC File Offset: 0x00038ADC
		private void AdvancePosition(ref int offset, ref int leftToProcess, int amountProcessed)
		{
			this.position += (long)amountProcessed;
			offset += amountProcessed;
			leftToProcess -= amountProcessed;
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x0003A8F9 File Offset: 0x00038AF9
		private void AdvancePosition(ref long leftToProcess, int amountProcessed)
		{
			this.position += (long)amountProcessed;
			leftToProcess -= (long)amountProcessed;
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x0003A914 File Offset: 0x00038B14
		private ArraySegment<byte> GetCurrentBlock()
		{
			int index = (int)(this.position / (long)this.bufferSize);
			int num = (int)(this.position % (long)this.bufferSize);
			byte[] array = this.bufferBlocks[index];
			return new ArraySegment<byte>(array, num, array.Length - num);
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x0003A95C File Offset: 0x00038B5C
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				this.disposed = true;
				if (disposing)
				{
					if (this.bufferManager != null)
					{
						foreach (byte[] buffer in this.bufferBlocks)
						{
							this.bufferManager.ReturnBuffer(buffer);
						}
					}
					this.bufferBlocks.Clear();
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x04000287 RID: 647
		private const int DefaultSmallBufferSize = 65536;

		// Token: 0x04000288 RID: 648
		private readonly int bufferSize;

		// Token: 0x04000289 RID: 649
		private List<byte[]> bufferBlocks;

		// Token: 0x0400028A RID: 650
		private long length;

		// Token: 0x0400028B RID: 651
		private long capacity;

		// Token: 0x0400028C RID: 652
		private long position;

		// Token: 0x0400028D RID: 653
		private IBufferManager bufferManager;

		// Token: 0x0400028E RID: 654
		private volatile bool disposed;

		// Token: 0x02000088 RID: 136
		private class CopyState
		{
			// Token: 0x170001D5 RID: 469
			// (get) Token: 0x06000F5C RID: 3932 RVA: 0x0003A9E4 File Offset: 0x00038BE4
			// (set) Token: 0x06000F5D RID: 3933 RVA: 0x0003A9EC File Offset: 0x00038BEC
			public Stream Destination { get; set; }

			// Token: 0x170001D6 RID: 470
			// (get) Token: 0x06000F5E RID: 3934 RVA: 0x0003A9F5 File Offset: 0x00038BF5
			// (set) Token: 0x06000F5F RID: 3935 RVA: 0x0003A9FD File Offset: 0x00038BFD
			public DateTime? ExpiryTime { get; set; }
		}
	}
}
