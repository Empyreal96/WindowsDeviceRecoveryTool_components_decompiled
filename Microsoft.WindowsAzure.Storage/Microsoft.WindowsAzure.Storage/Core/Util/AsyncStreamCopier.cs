using System;
using System.IO;
using System.Threading;
using Microsoft.WindowsAzure.Storage.Core.Executor;

namespace Microsoft.WindowsAzure.Storage.Core.Util
{
	// Token: 0x02000061 RID: 97
	internal class AsyncStreamCopier<T> : IDisposable
	{
		// Token: 0x06000D8F RID: 3471 RVA: 0x00032064 File Offset: 0x00030264
		public AsyncStreamCopier(Stream src, Stream dest, ExecutionState<T> state, int buffSize, bool calculateMd5, StreamDescriptor streamCopyState)
		{
			this.src = src;
			this.dest = dest;
			this.state = state;
			this.currentReadBuff = new byte[buffSize];
			this.currentWriteBuff = new byte[buffSize];
			this.streamCopyState = streamCopyState;
			if (streamCopyState != null && calculateMd5 && streamCopyState.Md5HashRef == null)
			{
				streamCopyState.Md5HashRef = new MD5Wrapper();
			}
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x0003210C File Offset: 0x0003030C
		public void StartCopyStream(Action<ExecutionState<T>> completedDelegate, long? copyLength, long? maxLength)
		{
			if (copyLength != null && maxLength != null)
			{
				throw new ArgumentException("Cannot specify both copyLength and maxLength.");
			}
			if (this.src.CanSeek && maxLength != null && this.src.Length - this.src.Position > maxLength)
			{
				throw new InvalidOperationException("The length of the stream exceeds the permitted length.");
			}
			if (this.src.CanSeek && copyLength != null && this.src.Length - this.src.Position < copyLength)
			{
				throw new ArgumentOutOfRangeException("copyLength", "The requested number of bytes exceeds the length of the stream remaining from the specified position.");
			}
			this.copyLen = copyLength;
			this.maximumLen = maxLength;
			this.completedDel = completedDelegate;
			if (this.state.OperationExpiryTime != null)
			{
				this.waitHandle = ThreadPool.RegisterWaitForSingleObject(this.completedEvent, new WaitOrTimerCallback(AsyncStreamCopier<T>.MaximumCopyTimeCallback), this, this.state.RemainingTimeout, true);
			}
			lock (this.state.CancellationLockerObject)
			{
				this.previousCancellationDelegate = this.state.CancelDelegate;
				this.state.CancelDelegate = new Action(this.Abort);
			}
			this.EndOpWithCatch(null);
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x00032298 File Offset: 0x00030498
		public void Abort()
		{
			this.cancelRequested = true;
			AsyncStreamCopier<T>.ForceAbort(this, false);
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x000322AA File Offset: 0x000304AA
		public void Dispose()
		{
			if (this.waitHandle != null)
			{
				this.waitHandle.Unregister(null);
				this.waitHandle = null;
			}
			if (this.completedEvent != null)
			{
				this.completedEvent.Close();
				this.completedEvent = null;
			}
			this.state = null;
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x000322EC File Offset: 0x000304EC
		private void EndOpWithCatch(IAsyncResult res)
		{
			if (res != null && res.CompletedSynchronously)
			{
				return;
			}
			lock (this.lockerObj)
			{
				try
				{
					this.EndOperation(res);
				}
				catch (Exception inner)
				{
					if (this.state.ReqTimedOut)
					{
						this.exceptionRef = Exceptions.GenerateTimeoutException((this.state.Cmd != null) ? this.state.Cmd.CurrentResult : null, inner);
					}
					else if (this.cancelRequested)
					{
						this.exceptionRef = Exceptions.GenerateCancellationException((this.state.Cmd != null) ? this.state.Cmd.CurrentResult : null, inner);
					}
					else
					{
						this.exceptionRef = inner;
					}
					if (this.readRes == null && this.writeRes == null)
					{
						this.SignalCompletion();
					}
				}
			}
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x000323E4 File Offset: 0x000305E4
		private void EndOperation(IAsyncResult res)
		{
			if (res != null)
			{
				if ((bool)res.AsyncState)
				{
					this.ProcessEndRead();
				}
				else
				{
					this.ProcessEndWrite();
				}
			}
			while (!this.ReachedEndOfSrc() && this.readRes == null && this.writeRes == null)
			{
				if (!this.ShouldDispatchNextOperation())
				{
					this.SignalCompletion();
					return;
				}
				if (this.ConsumeReadBuffer() > 0)
				{
					this.writeRes = this.dest.BeginWrite(this.currentWriteBuff, 0, this.currentWriteCount, new AsyncCallback(this.EndOpWithCatch), false);
					if (this.writeRes.CompletedSynchronously)
					{
						this.ProcessEndWrite();
					}
				}
				int num = this.NextReadLength();
				if (num != 0)
				{
					this.readRes = this.src.BeginRead(this.currentReadBuff, 0, num, new AsyncCallback(this.EndOpWithCatch), true);
					if (this.readRes.CompletedSynchronously)
					{
						this.ProcessEndRead();
					}
				}
				else
				{
					this.lastReadCount = 0;
				}
			}
			if (this.ReachedEndOfSrc() && this.writeRes == null)
			{
				if (this.exceptionRef == null && this.copyLen != null && this.NextReadLength() != 0)
				{
					this.exceptionRef = new ArgumentOutOfRangeException("copyLength", "The requested number of bytes exceeds the length of the stream remaining from the specified position.");
				}
				this.SignalCompletion();
			}
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x0003253C File Offset: 0x0003073C
		private static void MaximumCopyTimeCallback(object copier, bool timedOut)
		{
			if (timedOut)
			{
				AsyncStreamCopier<T> copier2 = (AsyncStreamCopier<T>)copier;
				AsyncStreamCopier<T>.ForceAbort(copier2, true);
			}
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x0003255C File Offset: 0x0003075C
		private static void ForceAbort(AsyncStreamCopier<T> copier, bool timedOut)
		{
			ExecutionState<T> executionState = copier.state;
			if (executionState != null)
			{
				if (executionState.Req != null)
				{
					try
					{
						executionState.ReqTimedOut = timedOut;
						executionState.Req.Abort();
					}
					catch (Exception)
					{
					}
				}
				copier.exceptionRef = (timedOut ? Exceptions.GenerateTimeoutException((executionState.Cmd != null) ? executionState.Cmd.CurrentResult : null, null) : Exceptions.GenerateCancellationException((executionState.Cmd != null) ? executionState.Cmd.CurrentResult : null, null));
			}
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x000325E8 File Offset: 0x000307E8
		private void SignalCompletion()
		{
			if (Interlocked.CompareExchange(ref this.completionProcessed, 1, 0) == 0)
			{
				this.completedEvent.Set();
				this.ProcessCompletion();
			}
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x0003260C File Offset: 0x0003080C
		private void ProcessCompletion()
		{
			this.state.CancelDelegate = this.previousCancellationDelegate;
			this.src = null;
			this.dest = null;
			this.currentReadBuff = null;
			this.currentWriteBuff = null;
			if (this.exceptionRef == null && this.streamCopyState != null && this.streamCopyState.Md5HashRef != null)
			{
				try
				{
					this.streamCopyState.Md5 = this.streamCopyState.Md5HashRef.ComputeHash();
				}
				catch (Exception)
				{
				}
				finally
				{
					this.streamCopyState.Md5HashRef = null;
				}
			}
			if (this.exceptionRef != null)
			{
				this.state.ExceptionRef = this.exceptionRef;
			}
			Action<ExecutionState<T>> action = this.completedDel;
			this.completedDel = null;
			if (action != null)
			{
				try
				{
					action(this.state);
				}
				catch (Exception ex)
				{
					this.state.ExceptionRef = ex;
				}
			}
			this.Dispose();
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x00032708 File Offset: 0x00030908
		private bool ShouldDispatchNextOperation()
		{
			if (this.maximumLen != null && Interlocked.Read(ref this.currentBytesReadFromSource) > this.maximumLen)
			{
				this.exceptionRef = new InvalidOperationException("The length of the stream exceeds the permitted length.");
			}
			else if (this.state.OperationExpiryTime != null && DateTime.Now >= this.state.OperationExpiryTime.Value)
			{
				this.exceptionRef = Exceptions.GenerateTimeoutException((this.state.Cmd != null) ? this.state.Cmd.CurrentResult : null, null);
			}
			else if (this.state.CancelRequested)
			{
				this.exceptionRef = Exceptions.GenerateCancellationException((this.state.Cmd != null) ? this.state.Cmd.CurrentResult : null, null);
			}
			return !this.cancelRequested && this.exceptionRef == null;
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x00032810 File Offset: 0x00030A10
		private void ProcessEndRead()
		{
			IAsyncResult asyncResult = this.readRes;
			this.readRes = null;
			this.lastReadCount = this.src.EndRead(asyncResult);
			Interlocked.Add(ref this.currentBytesReadFromSource, (long)this.lastReadCount);
			this.state.UpdateCompletedSynchronously(asyncResult.CompletedSynchronously);
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x0003286C File Offset: 0x00030A6C
		private void ProcessEndWrite()
		{
			IAsyncResult asyncResult = this.writeRes;
			this.writeRes = null;
			this.dest.EndWrite(asyncResult);
			this.state.UpdateCompletedSynchronously(asyncResult.CompletedSynchronously);
			if (this.streamCopyState != null)
			{
				this.streamCopyState.Length += (long)this.currentWriteCount;
				if (this.streamCopyState.Md5HashRef != null)
				{
					this.streamCopyState.Md5HashRef.UpdateHash(this.currentWriteBuff, 0, this.currentWriteCount);
				}
			}
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x000328F8 File Offset: 0x00030AF8
		private int ConsumeReadBuffer()
		{
			if (!this.ReadBufferFull())
			{
				return this.lastReadCount;
			}
			this.currentWriteCount = this.lastReadCount;
			this.lastReadCount = -1;
			byte[] array = this.currentReadBuff;
			this.currentReadBuff = this.currentWriteBuff;
			this.currentWriteBuff = array;
			return this.currentWriteCount;
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x00032954 File Offset: 0x00030B54
		private int NextReadLength()
		{
			if (this.copyLen != null)
			{
				long val = this.copyLen.Value - Interlocked.Read(ref this.currentBytesReadFromSource);
				return (int)Math.Min(val, (long)this.currentReadBuff.Length);
			}
			return this.currentReadBuff.Length;
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x0003299F File Offset: 0x00030B9F
		private bool ReachedEndOfSrc()
		{
			return this.lastReadCount == 0;
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x000329AC File Offset: 0x00030BAC
		private bool ReadBufferFull()
		{
			return this.lastReadCount > 0;
		}

		// Token: 0x040001BE RID: 446
		private byte[] currentReadBuff;

		// Token: 0x040001BF RID: 447
		private byte[] currentWriteBuff;

		// Token: 0x040001C0 RID: 448
		private volatile int lastReadCount = -1;

		// Token: 0x040001C1 RID: 449
		private volatile int currentWriteCount = -1;

		// Token: 0x040001C2 RID: 450
		private StreamDescriptor streamCopyState;

		// Token: 0x040001C3 RID: 451
		private Exception exceptionRef;

		// Token: 0x040001C4 RID: 452
		private long currentBytesReadFromSource;

		// Token: 0x040001C5 RID: 453
		private long? copyLen = null;

		// Token: 0x040001C6 RID: 454
		private long? maximumLen = null;

		// Token: 0x040001C7 RID: 455
		private Stream src;

		// Token: 0x040001C8 RID: 456
		private Stream dest;

		// Token: 0x040001C9 RID: 457
		private Action<ExecutionState<T>> completedDel;

		// Token: 0x040001CA RID: 458
		private volatile IAsyncResult readRes;

		// Token: 0x040001CB RID: 459
		private volatile IAsyncResult writeRes;

		// Token: 0x040001CC RID: 460
		private object lockerObj = new object();

		// Token: 0x040001CD RID: 461
		private volatile bool cancelRequested;

		// Token: 0x040001CE RID: 462
		private ExecutionState<T> state;

		// Token: 0x040001CF RID: 463
		private Action previousCancellationDelegate;

		// Token: 0x040001D0 RID: 464
		private RegisteredWaitHandle waitHandle;

		// Token: 0x040001D1 RID: 465
		private ManualResetEvent completedEvent = new ManualResetEvent(false);

		// Token: 0x040001D2 RID: 466
		private int completionProcessed;
	}
}
