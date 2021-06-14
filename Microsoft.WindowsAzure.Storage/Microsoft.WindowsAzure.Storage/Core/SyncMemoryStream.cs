using System;
using System.IO;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Core
{
	// Token: 0x0200008C RID: 140
	public class SyncMemoryStream : MemoryStream
	{
		// Token: 0x06000F7A RID: 3962 RVA: 0x0003AB66 File Offset: 0x00038D66
		public SyncMemoryStream()
		{
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x0003AB6E File Offset: 0x00038D6E
		public SyncMemoryStream(byte[] buffer) : base(buffer)
		{
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x0003AB77 File Offset: 0x00038D77
		public SyncMemoryStream(byte[] buffer, int index) : base(buffer, index, buffer.Length - index)
		{
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x0003AB86 File Offset: 0x00038D86
		public SyncMemoryStream(byte[] buffer, int index, int count) : base(buffer, index, count)
		{
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x0003AB94 File Offset: 0x00038D94
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

		// Token: 0x06000F7F RID: 3967 RVA: 0x0003AC08 File Offset: 0x00038E08
		public override int EndRead(IAsyncResult asyncResult)
		{
			StorageAsyncResult<int> storageAsyncResult = (StorageAsyncResult<int>)asyncResult;
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x0003AC28 File Offset: 0x00038E28
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

		// Token: 0x06000F81 RID: 3969 RVA: 0x0003AC98 File Offset: 0x00038E98
		public override void EndWrite(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult;
			storageAsyncResult.End();
		}
	}
}
