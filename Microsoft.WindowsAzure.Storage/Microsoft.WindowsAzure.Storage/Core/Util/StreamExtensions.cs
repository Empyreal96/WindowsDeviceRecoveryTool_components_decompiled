using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.WindowsAzure.Storage.Core.Executor;

namespace Microsoft.WindowsAzure.Storage.Core.Util
{
	// Token: 0x0200009D RID: 157
	internal static class StreamExtensions
	{
		// Token: 0x0600102E RID: 4142 RVA: 0x0003D8EE File Offset: 0x0003BAEE
		[DebuggerNonUserCode]
		internal static int GetBufferSize(Stream inStream)
		{
			if (inStream.CanSeek && inStream.Length - inStream.Position > 0L)
			{
				return (int)Math.Min(inStream.Length - inStream.Position, 65536L);
			}
			return 65536;
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x0003D928 File Offset: 0x0003BB28
		[DebuggerNonUserCode]
		internal static void WriteToSync<T>(this Stream stream, Stream toStream, long? copyLength, long? maxLength, bool calculateMd5, bool syncRead, ExecutionState<T> executionState, StreamDescriptor streamCopyState)
		{
			if (copyLength != null && maxLength != null)
			{
				throw new ArgumentException("Cannot specify both copyLength and maxLength.");
			}
			if (stream.CanSeek && maxLength != null && stream.Length - stream.Position > maxLength)
			{
				throw new InvalidOperationException("The length of the stream exceeds the permitted length.");
			}
			if (stream.CanSeek && copyLength != null && stream.Length - stream.Position < copyLength)
			{
				throw new ArgumentOutOfRangeException("copyLength", "The requested number of bytes exceeds the length of the stream remaining from the specified position.");
			}
			byte[] array = new byte[StreamExtensions.GetBufferSize(stream)];
			if (streamCopyState != null && calculateMd5 && streamCopyState.Md5HashRef == null)
			{
				streamCopyState.Md5HashRef = new MD5Wrapper();
			}
			RegisteredWaitHandle registeredWaitHandle = null;
			ManualResetEvent manualResetEvent = null;
			if (!syncRead && executionState.OperationExpiryTime != null)
			{
				manualResetEvent = new ManualResetEvent(false);
				registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(manualResetEvent, new WaitOrTimerCallback(StreamExtensions.MaximumCopyTimeCallback<T>), executionState, executionState.RemainingTimeout, true);
			}
			try
			{
				try
				{
					long? num = copyLength;
					while (executionState.OperationExpiryTime == null || DateTime.Now.CompareTo(executionState.OperationExpiryTime.Value) <= 0)
					{
						int num2 = StreamExtensions.MinBytesToRead(num, array.Length);
						if (num2 != 0)
						{
							int num3 = syncRead ? stream.Read(array, 0, num2) : stream.EndRead(stream.BeginRead(array, 0, num2, null, null));
							if (num != null)
							{
								num -= (long)num3;
							}
							if (num3 > 0)
							{
								toStream.Write(array, 0, num3);
								if (streamCopyState != null)
								{
									streamCopyState.Length += (long)num3;
									if (maxLength != null && streamCopyState.Length > maxLength.Value)
									{
										throw new InvalidOperationException("The length of the stream exceeds the permitted length.");
									}
									if (streamCopyState.Md5HashRef != null)
									{
										streamCopyState.Md5HashRef.UpdateHash(array, 0, num3);
									}
								}
							}
							if (num3 != 0)
							{
								continue;
							}
						}
						if (num != null && num != 0L)
						{
							throw new ArgumentOutOfRangeException("copyLength", "The requested number of bytes exceeds the length of the stream remaining from the specified position.");
						}
						goto IL_2C8;
					}
					throw Exceptions.GenerateTimeoutException((executionState.Cmd != null) ? executionState.Cmd.CurrentResult : null, null);
				}
				catch (Exception)
				{
					if (executionState.OperationExpiryTime != null && DateTime.Now.CompareTo(executionState.OperationExpiryTime.Value) > 0)
					{
						throw Exceptions.GenerateTimeoutException((executionState.Cmd != null) ? executionState.Cmd.CurrentResult : null, null);
					}
					throw;
				}
				IL_2C8:;
			}
			finally
			{
				if (registeredWaitHandle != null)
				{
					registeredWaitHandle.Unregister(null);
				}
				if (manualResetEvent != null)
				{
					manualResetEvent.Close();
				}
			}
			if (streamCopyState != null && streamCopyState.Md5HashRef != null)
			{
				streamCopyState.Md5 = streamCopyState.Md5HashRef.ComputeHash();
				streamCopyState.Md5HashRef = null;
			}
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x0003DC70 File Offset: 0x0003BE70
		private static void MaximumCopyTimeCallback<T>(object state, bool timedOut)
		{
			ExecutionState<T> executionState = (ExecutionState<T>)state;
			if (timedOut && executionState.Req != null)
			{
				try
				{
					executionState.ReqTimedOut = true;
					executionState.Req.Abort();
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x0003DCB8 File Offset: 0x0003BEB8
		private static int MinBytesToRead(long? val1, int val2)
		{
			if (val1 != null && val1 < (long)val2)
			{
				return (int)val1.Value;
			}
			return val2;
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x0003DCF3 File Offset: 0x0003BEF3
		[DebuggerNonUserCode]
		internal static Stream WrapWithByteCountingStream(this Stream stream, RequestResult result)
		{
			return new ByteCountingStream(stream, result);
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x0003DCFC File Offset: 0x0003BEFC
		[DebuggerNonUserCode]
		internal static void WriteToAsync<T>(this Stream stream, Stream toStream, long? copyLength, long? maxLength, bool calculateMd5, ExecutionState<T> executionState, StreamDescriptor streamCopyState, Action<ExecutionState<T>> completed)
		{
			AsyncStreamCopier<T> asyncStreamCopier = new AsyncStreamCopier<T>(stream, toStream, executionState, StreamExtensions.GetBufferSize(stream), calculateMd5, streamCopyState);
			asyncStreamCopier.StartCopyStream(completed, copyLength, maxLength);
		}
	}
}
