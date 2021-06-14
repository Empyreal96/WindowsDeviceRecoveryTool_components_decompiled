using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace FFUComponents
{
	// Token: 0x0200005A RID: 90
	internal class DTSFUsbStream : Stream
	{
		// Token: 0x0600019D RID: 413 RVA: 0x00008534 File Offset: 0x00006734
		public DTSFUsbStream(string deviceName, FileShare shareMode, TimeSpan transferTimeout)
		{
			if (string.IsNullOrEmpty(deviceName))
			{
				throw new ArgumentException("Invalid Argument", "deviceName");
			}
			this.isDisposed = false;
			this.deviceName = deviceName;
			try
			{
				int num = 0;
				this.deviceHandle = DTSFUsbStream.CreateDeviceHandle(this.deviceName, shareMode, ref num);
				if (this.deviceHandle.IsInvalid)
				{
					throw new IOException(string.Format(CultureInfo.CurrentCulture, Resources.ERROR_INVALID_HANDLE, new object[]
					{
						deviceName,
						num
					}));
				}
				this.InitializeDevice();
				this.SetTransferTimeout(transferTimeout);
				if (!ThreadPool.BindHandle(this.deviceHandle))
				{
					throw new IOException(string.Format(CultureInfo.CurrentCulture, Resources.ERROR_BINDHANDLE, new object[]
					{
						deviceName
					}));
				}
				this.Connect();
			}
			catch (Exception)
			{
				this.CloseDeviceHandle();
				throw;
			}
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000862C File Offset: 0x0000682C
		public DTSFUsbStream(string deviceName, TimeSpan transferTimeout) : this(deviceName, FileShare.None, transferTimeout)
		{
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600019F RID: 415 RVA: 0x00008637 File Offset: 0x00006837
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x0000863A File Offset: 0x0000683A
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x0000863D File Offset: 0x0000683D
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00008640 File Offset: 0x00006840
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x00008647 File Offset: 0x00006847
		// (set) Token: 0x060001A4 RID: 420 RVA: 0x0000864E File Offset: 0x0000684E
		public override long Position
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00008655 File Offset: 0x00006855
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000865C File Offset: 0x0000685C
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00008663 File Offset: 0x00006863
		public override void Flush()
		{
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00008665 File Offset: 0x00006865
		private void HandleAsyncTimeout(IAsyncResult asyncResult)
		{
			if (NativeMethods.CancelIo(this.deviceHandle))
			{
				asyncResult.AsyncWaitHandle.WaitOne(this.completionTimeout, false);
			}
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00008688 File Offset: 0x00006888
		public override int Read(byte[] buffer, int offset, int count)
		{
			IAsyncResult asyncResult = this.BeginRead(buffer, offset, count, null, null);
			int result;
			try
			{
				result = this.EndRead(asyncResult);
			}
			catch (TimeoutException innerException)
			{
				this.HandleAsyncTimeout(asyncResult);
				throw new Win32Exception(Resources.ERROR_CALLBACK_TIMEOUT, innerException);
			}
			return result;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x000086D0 File Offset: 0x000068D0
		public override void Write(byte[] buffer, int offset, int count)
		{
			IAsyncResult asyncResult = this.BeginWrite(buffer, offset, count, null, null);
			try
			{
				this.EndWrite(asyncResult);
			}
			catch (TimeoutException innerException)
			{
				this.HandleAsyncTimeout(asyncResult);
				throw new Win32Exception(Resources.ERROR_CALLBACK_TIMEOUT, innerException);
			}
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00008718 File Offset: 0x00006918
		private void RetryRead(uint errorCode, DTSFUsbStreamReadAsyncResult asyncResult, out Exception exception)
		{
			exception = null;
			if (this.IsDeviceDisconnected(errorCode))
			{
				exception = new Win32Exception((int)errorCode);
				return;
			}
			if (asyncResult.RetryCount > 10)
			{
				exception = new Win32Exception((int)errorCode);
				return;
			}
			int num = 0;
			this.ClearPipeStall(this.bulkInPipeId, out num);
			if (num != 0)
			{
				exception = new Win32Exception(num);
				return;
			}
			try
			{
				this.BeginReadInternal(asyncResult.Buffer, asyncResult.Offset, asyncResult.Count, asyncResult.RetryCount++, asyncResult.AsyncCallback, asyncResult.AsyncState);
			}
			catch (Exception ex)
			{
				exception = ex;
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x000087B8 File Offset: 0x000069B8
		private unsafe void ReadIOCompletionCallback(uint errorCode, uint numBytes, NativeOverlapped* nativeOverlapped)
		{
			try
			{
				Overlapped overlapped = Overlapped.Unpack(nativeOverlapped);
				DTSFUsbStreamReadAsyncResult dtsfusbStreamReadAsyncResult = (DTSFUsbStreamReadAsyncResult)overlapped.AsyncResult;
				Overlapped.Free(nativeOverlapped);
				Exception ex = null;
				if (errorCode != 0U)
				{
					this.RetryRead(errorCode, dtsfusbStreamReadAsyncResult, out ex);
					if (ex != null)
					{
						dtsfusbStreamReadAsyncResult.SetAsCompleted(ex, false);
					}
				}
				else
				{
					dtsfusbStreamReadAsyncResult.SetAsCompleted((int)numBytes, false);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00008818 File Offset: 0x00006A18
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback userCallback, object stateObject)
		{
			if (this.deviceHandle.IsClosed)
			{
				throw new ObjectDisposedException(Resources.ERROR_FILE_CLOSED);
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "ArgumentOutOfRange_NeedNonNegNum");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "ArgumentOutOfRange_NeedNonNegNum");
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException("Argument_InvalidOffLen");
			}
			return this.BeginReadInternal(buffer, offset, count, 10, userCallback, stateObject);
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00008898 File Offset: 0x00006A98
		private unsafe IAsyncResult BeginReadInternal(byte[] buffer, int offset, int count, int retryCount, AsyncCallback userCallback, object stateObject)
		{
			NativeOverlapped* ptr = null;
			DTSFUsbStreamReadAsyncResult dtsfusbStreamReadAsyncResult = new DTSFUsbStreamReadAsyncResult(userCallback, stateObject)
			{
				Buffer = buffer,
				Offset = offset,
				Count = count,
				RetryCount = retryCount
			};
			Overlapped overlapped = new Overlapped(0, 0, IntPtr.Zero, dtsfusbStreamReadAsyncResult);
			ptr = overlapped.Pack(new IOCompletionCallback(this.ReadIOCompletionCallback), buffer);
			fixed (byte* ptr2 = buffer)
			{
				if (!NativeMethods.WinUsbReadPipe(this.usbHandle, this.bulkInPipeId, ptr2 + offset, (uint)count, IntPtr.Zero, ptr))
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					if (997 != lastWin32Error)
					{
						Overlapped.Unpack(ptr);
						Overlapped.Free(ptr);
						throw new Win32Exception(lastWin32Error);
					}
				}
			}
			return dtsfusbStreamReadAsyncResult;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000895C File Offset: 0x00006B5C
		public override int EndRead(IAsyncResult asyncResult)
		{
			DTSFUsbStreamReadAsyncResult dtsfusbStreamReadAsyncResult = (DTSFUsbStreamReadAsyncResult)asyncResult;
			return dtsfusbStreamReadAsyncResult.EndInvoke();
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00008978 File Offset: 0x00006B78
		private void RetryWrite(uint errorCode, DTSFUsbStreamWriteAsyncResult asyncResult, out Exception exception)
		{
			exception = null;
			if (this.IsDeviceDisconnected(errorCode))
			{
				exception = new Win32Exception((int)errorCode);
				return;
			}
			if (asyncResult.RetryCount > 10)
			{
				exception = new Win32Exception((int)errorCode);
				return;
			}
			int num = 0;
			this.ClearPipeStall(this.bulkOutPipeId, out num);
			if (num != 0)
			{
				exception = new Win32Exception(num);
				return;
			}
			try
			{
				this.BeginWriteInternal(asyncResult.Buffer, asyncResult.Offset, asyncResult.Count, asyncResult.RetryCount++, asyncResult.AsyncCallback, asyncResult.AsyncState);
			}
			catch (Exception ex)
			{
				exception = ex;
			}
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00008A18 File Offset: 0x00006C18
		private unsafe void WriteIOCompletionCallback(uint errorCode, uint numBytes, NativeOverlapped* nativeOverlapped)
		{
			Overlapped overlapped = Overlapped.Unpack(nativeOverlapped);
			DTSFUsbStreamWriteAsyncResult dtsfusbStreamWriteAsyncResult = (DTSFUsbStreamWriteAsyncResult)overlapped.AsyncResult;
			Overlapped.Free(nativeOverlapped);
			Exception ex = null;
			try
			{
				if (errorCode != 0U)
				{
					this.RetryWrite(errorCode, dtsfusbStreamWriteAsyncResult, out ex);
					if (ex != null)
					{
						dtsfusbStreamWriteAsyncResult.SetAsCompleted(ex, false);
					}
				}
				else
				{
					dtsfusbStreamWriteAsyncResult.SetAsCompleted(ex, false);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00008A78 File Offset: 0x00006C78
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback userCallback, object stateObject)
		{
			if (this.deviceHandle.IsClosed)
			{
				throw new ObjectDisposedException(Resources.ERROR_FILE_CLOSED);
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "ArgumentOutOfRange_NeedNonNegNum");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "ArgumentOutOfRange_NeedNonNegNum");
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException("Argument_InvalidOffLen");
			}
			return this.BeginWriteInternal(buffer, offset, count, 0, userCallback, stateObject);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00008AF4 File Offset: 0x00006CF4
		private unsafe IAsyncResult BeginWriteInternal(byte[] buffer, int offset, int count, int retryCount, AsyncCallback userCallback, object stateObject)
		{
			NativeOverlapped* ptr = null;
			DTSFUsbStreamWriteAsyncResult dtsfusbStreamWriteAsyncResult = new DTSFUsbStreamWriteAsyncResult(userCallback, stateObject)
			{
				Buffer = buffer,
				Offset = offset,
				Count = count,
				RetryCount = retryCount
			};
			Overlapped overlapped = new Overlapped(0, 0, IntPtr.Zero, dtsfusbStreamWriteAsyncResult);
			ptr = overlapped.Pack(new IOCompletionCallback(this.WriteIOCompletionCallback), buffer);
			fixed (byte* ptr2 = buffer)
			{
				if (!NativeMethods.WinUsbWritePipe(this.usbHandle, this.bulkOutPipeId, ptr2 + offset, (uint)count, IntPtr.Zero, ptr))
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					if (997 != lastWin32Error)
					{
						Overlapped.Unpack(ptr);
						Overlapped.Free(ptr);
						throw new Win32Exception(lastWin32Error);
					}
				}
			}
			return dtsfusbStreamWriteAsyncResult;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00008BB8 File Offset: 0x00006DB8
		public override void EndWrite(IAsyncResult asyncResult)
		{
			DTSFUsbStreamWriteAsyncResult dtsfusbStreamWriteAsyncResult = (DTSFUsbStreamWriteAsyncResult)asyncResult;
			dtsfusbStreamWriteAsyncResult.EndInvoke();
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00008BD4 File Offset: 0x00006DD4
		private static SafeFileHandle CreateDeviceHandle(string deviceName, FileShare shareMode, ref int lastError)
		{
			SafeFileHandle result = NativeMethods.CreateFile(deviceName, 3221225472U, (uint)shareMode, IntPtr.Zero, 3U, 1073741952U, IntPtr.Zero);
			lastError = Marshal.GetLastWin32Error();
			return result;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00008C08 File Offset: 0x00006E08
		private void CloseDeviceHandle()
		{
			if (IntPtr.Zero != this.usbHandle)
			{
				NativeMethods.WinUsbFree(this.usbHandle);
				this.usbHandle = IntPtr.Zero;
			}
			if (!this.deviceHandle.IsInvalid && !this.deviceHandle.IsClosed)
			{
				this.deviceHandle.Close();
				this.deviceHandle.SetHandleAsInvalid();
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00008C70 File Offset: 0x00006E70
		private void InitializeDevice()
		{
			WinUsbInterfaceDescriptor winUsbInterfaceDescriptor = default(WinUsbInterfaceDescriptor);
			WinUsbPipeInformation winUsbPipeInformation = default(WinUsbPipeInformation);
			if (!NativeMethods.WinUsbInitialize(this.deviceHandle, ref this.usbHandle))
			{
				throw new IOException(Resources.ERROR_WINUSB_INITIALIZATION);
			}
			if (!NativeMethods.WinUsbQueryInterfaceSettings(this.usbHandle, 0, ref winUsbInterfaceDescriptor))
			{
				throw new IOException(Resources.ERROR_WINUSB_QUERY_INTERFACE_SETTING);
			}
			for (byte b = 0; b < winUsbInterfaceDescriptor.NumEndpoints; b += 1)
			{
				if (!NativeMethods.WinUsbQueryPipe(this.usbHandle, 0, b, ref winUsbPipeInformation))
				{
					throw new IOException(Resources.ERROR_WINUSB_QUERY_PIPE_INFORMATION);
				}
				WinUsbPipeType pipeType = winUsbPipeInformation.PipeType;
				if (pipeType == WinUsbPipeType.Bulk)
				{
					if (this.IsBulkInEndpoint(winUsbPipeInformation.PipeId))
					{
						this.SetupBulkInEndpoint(winUsbPipeInformation.PipeId);
					}
					else
					{
						if (!this.IsBulkOutEndpoint(winUsbPipeInformation.PipeId))
						{
							throw new IOException(Resources.ERROR_INVALID_ENDPOINT_TYPE);
						}
						this.SetupBulkOutEndpoint(winUsbPipeInformation.PipeId);
					}
				}
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00008D48 File Offset: 0x00006F48
		private bool IsBulkInEndpoint(byte pipeId)
		{
			return (pipeId & 128) == 128;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00008D58 File Offset: 0x00006F58
		private bool IsBulkOutEndpoint(byte pipeId)
		{
			return (pipeId & 128) == 0;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00008D64 File Offset: 0x00006F64
		public void PipeReset()
		{
			int num;
			this.ClearPipeStall(this.bulkInPipeId, out num);
			this.ClearPipeStall(this.bulkOutPipeId, out num);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00008D90 File Offset: 0x00006F90
		public void SetTransferTimeout(TimeSpan transferTimeout)
		{
			uint value = (uint)transferTimeout.TotalMilliseconds;
			this.SetPipePolicy(this.bulkInPipeId, 3U, value);
			this.SetPipePolicy(this.bulkOutPipeId, 3U, value);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00008DC2 File Offset: 0x00006FC2
		public void SetShortPacketTerminate()
		{
			this.SetPipePolicy(this.bulkOutPipeId, 1U, true);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00008DD2 File Offset: 0x00006FD2
		private void SetupBulkInEndpoint(byte pipeId)
		{
			this.bulkInPipeId = pipeId;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00008DDB File Offset: 0x00006FDB
		private void SetupBulkOutEndpoint(byte pipeId)
		{
			this.bulkOutPipeId = pipeId;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00008DE4 File Offset: 0x00006FE4
		private void SetPipePolicy(byte pipeId, uint policyType, uint value)
		{
			if (!NativeMethods.WinUsbSetPipePolicy(this.usbHandle, pipeId, policyType, (uint)Marshal.SizeOf(typeof(uint)), ref value))
			{
				throw new IOException(Resources.ERROR_WINUSB_SET_PIPE_POLICY);
			}
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00008E11 File Offset: 0x00007011
		private void SetPipePolicy(byte pipeId, uint policyType, bool value)
		{
			if (!NativeMethods.WinUsbSetPipePolicy(this.usbHandle, pipeId, policyType, (uint)Marshal.SizeOf(typeof(bool)), ref value))
			{
				throw new IOException(Resources.ERROR_WINUSB_SET_PIPE_POLICY);
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00008E40 File Offset: 0x00007040
		private unsafe void ControlTransferSetData(UsbControlRequest request, ushort value)
		{
			WinUsbSetupPacket setupPacket = default(WinUsbSetupPacket);
			setupPacket.RequestType = 33;
			setupPacket.Request = (byte)request;
			setupPacket.Value = value;
			setupPacket.Index = 0;
			setupPacket.Length = 0;
			uint num = 0U;
			byte[] array = null;
			fixed (byte* ptr = array)
			{
				if (!NativeMethods.WinUsbControlTransfer(this.usbHandle, setupPacket, ptr, (uint)setupPacket.Length, ref num, IntPtr.Zero))
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					throw new Win32Exception(lastWin32Error);
				}
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00008ED0 File Offset: 0x000070D0
		private unsafe void ControlTransferGetData(UsbControlRequest request, byte[] buffer)
		{
			WinUsbSetupPacket setupPacket = default(WinUsbSetupPacket);
			setupPacket.RequestType = 161;
			setupPacket.Request = (byte)request;
			setupPacket.Value = 0;
			setupPacket.Index = 0;
			setupPacket.Length = ((buffer == null) ? 0 : ((ushort)buffer.Length));
			uint num = 0U;
			fixed (byte* ptr = buffer)
			{
				if (!NativeMethods.WinUsbControlTransfer(this.usbHandle, setupPacket, ptr, (uint)setupPacket.Length, ref num, IntPtr.Zero))
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					throw new Win32Exception(lastWin32Error);
				}
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00008F65 File Offset: 0x00007165
		private void ClearPipeStall(byte pipeId, out int errorCode)
		{
			errorCode = 0;
			if (!NativeMethods.WinUsbAbortPipe(this.usbHandle, pipeId))
			{
				errorCode = Marshal.GetLastWin32Error();
			}
			if (!NativeMethods.WinUsbResetPipe(this.usbHandle, pipeId))
			{
				errorCode = Marshal.GetLastWin32Error();
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00008F94 File Offset: 0x00007194
		private bool IsDeviceDisconnected(uint errorCode)
		{
			return errorCode == 2U || errorCode == 1167U || errorCode == 31U || errorCode == 121U || errorCode == 995U;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00008FB8 File Offset: 0x000071B8
		protected override void Dispose(bool disposing)
		{
			if (!this.isDisposed && disposing)
			{
				try
				{
					this.CloseDeviceHandle();
				}
				catch (Exception)
				{
				}
				finally
				{
					base.Dispose(disposing);
					this.isDisposed = true;
				}
			}
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00009008 File Offset: 0x00007208
		~DTSFUsbStream()
		{
			this.Dispose(false);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00009038 File Offset: 0x00007238
		private void Connect()
		{
			this.ControlTransferSetData(UsbControlRequest.LineStateSet, 1);
		}

		// Token: 0x04000183 RID: 387
		private const byte UsbEndpointDirectionMask = 128;

		// Token: 0x04000184 RID: 388
		private const int retryCount = 10;

		// Token: 0x04000185 RID: 389
		private string deviceName;

		// Token: 0x04000186 RID: 390
		private SafeFileHandle deviceHandle;

		// Token: 0x04000187 RID: 391
		private IntPtr usbHandle;

		// Token: 0x04000188 RID: 392
		private byte bulkInPipeId;

		// Token: 0x04000189 RID: 393
		private byte bulkOutPipeId;

		// Token: 0x0400018A RID: 394
		private bool isDisposed;

		// Token: 0x0400018B RID: 395
		private TimeSpan completionTimeout = TimeSpan.FromSeconds(5.0);
	}
}
