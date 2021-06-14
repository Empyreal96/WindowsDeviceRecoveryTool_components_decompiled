using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;
using Nokia.Lucid.Diagnostics;

namespace Nokia.Lucid.UsbDeviceIo
{
	// Token: 0x02000044 RID: 68
	public class WinUsbIo : IDisposable
	{
		// Token: 0x060001BE RID: 446 RVA: 0x0000C5E8 File Offset: 0x0000A7E8
		public WinUsbIo(string devicePath)
		{
			using (EntryExitLogger.Log("WinUsbIo.WinUsbIo(string devicePath)", UsbDeviceIoTraceSource.Instance))
			{
				this.DevicePath = devicePath;
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000C654 File Offset: 0x0000A854
		protected override void Finalize()
		{
			try
			{
				using (EntryExitLogger.Log("WinUsbIo.~WinUsbIo()", UsbDeviceIoTraceSource.Instance))
				{
					this.Dispose(false);
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x0000C6AC File Offset: 0x0000A8AC
		// (set) Token: 0x060001C1 RID: 449 RVA: 0x0000C6B4 File Offset: 0x0000A8B4
		public string DevicePath { get; private set; }

		// Token: 0x060001C2 RID: 450 RVA: 0x0000C6C0 File Offset: 0x0000A8C0
		[CLSCompliant(false)]
		public uint Write(byte[] data, uint length)
		{
			uint result;
			using (EntryExitLogger.Log("WinUsbIo.Write(byte[] data, uint length)", UsbDeviceIoTraceSource.Instance))
			{
				lock (this.senderLock)
				{
					this.CheckIfDisposed();
					NativeMethods.OVERLAPPED overlapped = default(NativeMethods.OVERLAPPED);
					this.writeEvent.Reset();
					overlapped.EventHandle = this.writeEvent.SafeWaitHandle.DangerousGetHandle();
					GCHandle gchandle = GCHandle.Alloc(overlapped, GCHandleType.Pinned);
					GCHandle gchandle2 = GCHandle.Alloc(data, GCHandleType.Pinned);
					uint num = 0U;
					try
					{
						if (!NativeMethods.WinUsb_WritePipe(this.winUsbHandle, this.bulkOutPipe, data, length, ref num, ref overlapped))
						{
							if (Marshal.GetLastWin32Error() != 997)
							{
								throw new Win32Exception();
							}
							if (!this.writeEvent.WaitOne(15000))
							{
								if (!NativeMethods.CancelIoEx(this.deviceHandle.DangerousGetHandle(), ref overlapped))
								{
									RobustTrace.Trace<Win32Exception>(new Action<Win32Exception>(UsbDeviceIoTraceSource.Instance.DeviceIoError), new Win32Exception());
								}
								throw new TimeoutException("send operation timed out");
							}
							WinUsbIo.CheckError(NativeMethods.WinUsb_GetOverlappedResult(this.winUsbHandle, ref overlapped, ref num, false), "Get write overlapped result");
						}
						result = num;
					}
					finally
					{
						gchandle.Free();
						gchandle2.Free();
					}
				}
			}
			return result;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000C848 File Offset: 0x0000AA48
		[CLSCompliant(false)]
		public uint Read(byte[] data, uint length)
		{
			uint result;
			using (EntryExitLogger.Log("WinUsbIo.Read(ref byte[] data, uint length)", UsbDeviceIoTraceSource.Instance))
			{
				this.CheckIfDisposed();
				NativeMethods.OVERLAPPED overlapped = default(NativeMethods.OVERLAPPED);
				this.readEvent.Reset();
				overlapped.EventHandle = this.readEvent.SafeWaitHandle.DangerousGetHandle();
				GCHandle gchandle = GCHandle.Alloc(overlapped, GCHandleType.Pinned);
				GCHandle gchandle2 = GCHandle.Alloc(data, GCHandleType.Pinned);
				try
				{
					uint num = 0U;
					if (!NativeMethods.WinUsb_ReadPipe(this.winUsbHandle, this.bulkInPipe, data, length, ref num, ref overlapped))
					{
						if (Marshal.GetLastWin32Error() != 997)
						{
							throw new Win32Exception();
						}
						WinUsbIo.CheckError(NativeMethods.WinUsb_GetOverlappedResult(this.winUsbHandle, ref overlapped, ref num, true), "Get read overlapped result");
					}
					result = num;
				}
				finally
				{
					gchandle.Free();
					gchandle2.Free();
				}
			}
			return result;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000C938 File Offset: 0x0000AB38
		public void Open()
		{
			using (EntryExitLogger.Log("WinUsbIo.Open()", UsbDeviceIoTraceSource.Instance))
			{
				this.CheckIfDisposed();
				if (this.DevicePath.Length == 0)
				{
					throw new Exception("Device Path length == 0");
				}
				this.deviceHandle = NativeMethods.CreateFile(this.DevicePath, 3221225472U, 0, IntPtr.Zero, 3, 1073741952, IntPtr.Zero);
				if (this.deviceHandle.IsInvalid)
				{
					Win32Exception ex = new Win32Exception();
					string message = ex.Message;
					int nativeErrorCode = ex.NativeErrorCode;
					throw new Win32Exception(nativeErrorCode, "CreateFile failed to create valid handle to device. " + message);
				}
				WinUsbIo.CheckError(NativeMethods.WinUsb_Initialize(this.deviceHandle, out this.winUsbHandle), "Initializing WinUSB with device handle.");
				NativeMethods.USB_INTERFACE_DESCRIPTOR usb_INTERFACE_DESCRIPTOR;
				WinUsbIo.CheckError(NativeMethods.WinUsb_QueryInterfaceSettings(this.winUsbHandle, 0, out usb_INTERFACE_DESCRIPTOR), "Querying Interface Settings");
				for (int i = 0; i < (int)usb_INTERFACE_DESCRIPTOR.bNumEndpoints; i++)
				{
					NativeMethods.WINUSB_PIPE_INFORMATION winusb_PIPE_INFORMATION;
					WinUsbIo.CheckError(NativeMethods.WinUsb_QueryPipe(this.winUsbHandle, 0, Convert.ToByte(i), out winusb_PIPE_INFORMATION), "Querying Pipe Information.");
					if (winusb_PIPE_INFORMATION.PipeType == NativeMethods.USBD_PIPE_TYPE.UsbdPipeTypeBulk && (winusb_PIPE_INFORMATION.PipeId & 128) == 128)
					{
						this.bulkInPipe = winusb_PIPE_INFORMATION.PipeId;
					}
					else if (winusb_PIPE_INFORMATION.PipeType == NativeMethods.USBD_PIPE_TYPE.UsbdPipeTypeBulk && (winusb_PIPE_INFORMATION.PipeId & 128) == 0)
					{
						this.bulkOutPipe = winusb_PIPE_INFORMATION.PipeId;
					}
				}
			}
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000CAB8 File Offset: 0x0000ACB8
		public void CancelIo()
		{
			using (EntryExitLogger.Log("WinUsbIo.CancelIo()", UsbDeviceIoTraceSource.Instance))
			{
				WinUsbIo.CheckError(NativeMethods.CancelIoEx(this.deviceHandle.DangerousGetHandle(), IntPtr.Zero), "Cancel IO:");
			}
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000CB10 File Offset: 0x0000AD10
		public void Close()
		{
			using (EntryExitLogger.Log("WinUsbIo.Close()", UsbDeviceIoTraceSource.Instance))
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000CB58 File Offset: 0x0000AD58
		public void Dispose()
		{
			using (EntryExitLogger.Log("WinUsbIo.Dispose()", UsbDeviceIoTraceSource.Instance))
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000CBA0 File Offset: 0x0000ADA0
		[CLSCompliant(false)]
		public void SetPipePolicy(WinUsbIo.PIPE_TYPE pipeType, WinUsbIo.POLICY_TYPE policyType, uint value)
		{
			using (EntryExitLogger.Log("WinUsbIo.SetPipePolicy(PIPE_TYPE pipeType, POLICY_TYPE policyType, dynamic value)", UsbDeviceIoTraceSource.Instance))
			{
				this.CheckIfDisposed();
				if (pipeType == WinUsbIo.PIPE_TYPE.PipeTypeBulkIn)
				{
					WinUsbIo.CheckError(NativeMethods.WinUsb_SetPipePolicy(this.winUsbHandle, this.bulkInPipe, (uint)policyType, (uint)Marshal.SizeOf(value), ref value), "Setting Bulk In Pipe Policy");
				}
				else if (pipeType == WinUsbIo.PIPE_TYPE.PipeTypeBulkOut)
				{
					WinUsbIo.CheckError(NativeMethods.WinUsb_SetPipePolicy(this.winUsbHandle, this.bulkOutPipe, (uint)policyType, (uint)Marshal.SizeOf(value), ref value), "Setting Bulk Out Pipe Policy");
				}
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000CC3C File Offset: 0x0000AE3C
		public void Flush()
		{
			using (EntryExitLogger.Log("WinUsbIo.Flush()", UsbDeviceIoTraceSource.Instance))
			{
				this.CheckIfDisposed();
				WinUsbIo.CheckError(NativeMethods.WinUsb_FlushPipe(this.winUsbHandle, this.bulkInPipe), "Flushing Bulk In Pipe");
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000CC98 File Offset: 0x0000AE98
		[CLSCompliant(false)]
		public void GetDeviceDescriptor(out NativeMethods.USB_DEVICE_DESCRIPTOR deviceDescriptor)
		{
			using (EntryExitLogger.Log("WinUsbIo.GetDeviceDescriptor(NativeMethods.USB_DEVICE_DESCRIPTOR deviceDescriptor)", UsbDeviceIoTraceSource.Instance))
			{
				this.CheckIfDisposed();
				uint num = 0U;
				uint bufferLength = (uint)Marshal.SizeOf(typeof(NativeMethods.USB_DEVICE_DESCRIPTOR));
				WinUsbIo.CheckError(NativeMethods.WinUsb_GetDescriptor(this.winUsbHandle, 1, 0, 0, out deviceDescriptor, bufferLength, out num), "Reading Device descriptor.");
			}
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000CD08 File Offset: 0x0000AF08
		internal static void CheckError(bool expression, string message)
		{
			if (!expression)
			{
				Win32Exception ex = new Win32Exception();
				string message2 = ex.Message;
				int nativeErrorCode = ex.NativeErrorCode;
				throw new Win32Exception(nativeErrorCode, message + " " + message2);
			}
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000CD40 File Offset: 0x0000AF40
		private void Dispose(bool disposing)
		{
			using (EntryExitLogger.Log("WinUsbIo.Dispose(bool disposing)", UsbDeviceIoTraceSource.Instance))
			{
				if (!this.disposed)
				{
					if (disposing)
					{
						if (this.deviceHandle != null && !this.deviceHandle.IsInvalid)
						{
							this.deviceHandle.Dispose();
							this.deviceHandle = null;
						}
						if (this.readEvent != null)
						{
							this.readEvent.Dispose();
						}
						if (this.writeEvent != null)
						{
							this.writeEvent.Dispose();
						}
					}
					if (this.winUsbHandle != IntPtr.Zero)
					{
						WinUsbIo.CheckError(NativeMethods.WinUsb_Free(this.winUsbHandle), "Freeing WinUsb resources.");
						this.winUsbHandle = IntPtr.Zero;
					}
					this.disposed = true;
				}
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000CE10 File Offset: 0x0000B010
		private void CheckIfDisposed()
		{
			using (EntryExitLogger.Log("WinUsbIo.CheckIfDisposed()", UsbDeviceIoTraceSource.Instance))
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException("WinUsbIo object has been disposed.");
				}
			}
		}

		// Token: 0x0400011C RID: 284
		private readonly ManualResetEvent writeEvent = new ManualResetEvent(false);

		// Token: 0x0400011D RID: 285
		private readonly ManualResetEvent readEvent = new ManualResetEvent(false);

		// Token: 0x0400011E RID: 286
		private readonly object senderLock = new object();

		// Token: 0x0400011F RID: 287
		private SafeFileHandle deviceHandle;

		// Token: 0x04000120 RID: 288
		private IntPtr winUsbHandle;

		// Token: 0x04000121 RID: 289
		private byte bulkInPipe;

		// Token: 0x04000122 RID: 290
		private byte bulkOutPipe;

		// Token: 0x04000123 RID: 291
		private bool disposed;

		// Token: 0x02000045 RID: 69
		public enum PIPE_TYPE
		{
			// Token: 0x04000126 RID: 294
			PipeTypeBulkIn,
			// Token: 0x04000127 RID: 295
			PipeTypeBulkOut
		}

		// Token: 0x02000046 RID: 70
		public enum POLICY_TYPE
		{
			// Token: 0x04000129 RID: 297
			SHORT_PACKET_TERMINATE = 1,
			// Token: 0x0400012A RID: 298
			AUTO_CLEAR_STALL,
			// Token: 0x0400012B RID: 299
			PIPE_TRANSFER_TIMEOUT,
			// Token: 0x0400012C RID: 300
			IGNORE_SHORT_PACKETS,
			// Token: 0x0400012D RID: 301
			ALLOW_PARTIAL_READS,
			// Token: 0x0400012E RID: 302
			AUTO_FLUSH,
			// Token: 0x0400012F RID: 303
			RAW_IO
		}
	}
}
