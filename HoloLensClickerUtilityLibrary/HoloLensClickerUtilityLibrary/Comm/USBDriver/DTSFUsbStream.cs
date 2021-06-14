using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace ClickerUtilityLibrary.Comm.USBDriver
{
	// Token: 0x0200003B RID: 59
	internal class DTSFUsbStream : IDisposable
	{
		// Token: 0x06000160 RID: 352 RVA: 0x00008978 File Offset: 0x00006B78
		public DTSFUsbStream(string deviceName)
		{
			bool flag = string.IsNullOrEmpty(deviceName);
			if (flag)
			{
				throw new ArgumentException("Invalid Argument", "deviceName");
			}
			this.isDisposed = false;
			this.mDeviceName = deviceName;
			try
			{
				this.deviceHandle = DTSFUsbStream.CreateDeviceHandle(this.mDeviceName);
				bool isInvalid = this.deviceHandle.IsInvalid;
				if (isInvalid)
				{
					throw new IOException(string.Format(CultureInfo.CurrentCulture, "Handle for {0} is invalid.", new object[]
					{
						deviceName
					}));
				}
				this.InitializeDevice();
				bool flag2 = !ThreadPool.BindHandle(this.deviceHandle);
				if (flag2)
				{
					throw new IOException(string.Format(CultureInfo.CurrentCulture, "BindHandle on device {0} failed.", new object[]
					{
						deviceName
					}));
				}
			}
			catch (Exception)
			{
				this.CloseDeviceHandle();
				throw;
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00008A50 File Offset: 0x00006C50
		public unsafe int ReadWithTimeout(byte[] buffer, int offset, int count, uint timeout)
		{
			bool isClosed = this.deviceHandle.IsClosed;
			if (isClosed)
			{
				throw new ObjectDisposedException("File closed");
			}
			bool flag = buffer == null;
			if (flag)
			{
				throw new ArgumentNullException("buffer");
			}
			bool flag2 = offset < 0;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException("offset", "ArgumentOutOfRange_NeedNonNegNum");
			}
			bool flag3 = count < 0;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException("count", "ArgumentOutOfRange_NeedNonNegNum");
			}
			bool flag4 = buffer.Length - offset < count;
			if (flag4)
			{
				throw new ArgumentException("Argument_InvalidOffLen");
			}
			this.SetPipePolicy(this.bulkInPipeId, 3U, timeout);
			uint result;
			fixed (byte* ptr = buffer)
			{
				bool flag5 = !NativeMethods.WinUsbReadPipe(this.usbHandle, this.bulkInPipeId, ptr + offset, (uint)count, out result, (NativeOverlapped*)((void*)IntPtr.Zero));
				if (flag5)
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					bool flag6 = 121 != lastWin32Error;
					if (flag6)
					{
						throw new Win32Exception(lastWin32Error);
					}
				}
			}
			return (int)result;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00008B64 File Offset: 0x00006D64
		public unsafe void WriteWithTimeout(byte[] buffer, int offset, int count, uint timeout)
		{
			bool isClosed = this.deviceHandle.IsClosed;
			if (isClosed)
			{
				throw new ObjectDisposedException("File closed");
			}
			bool flag = buffer == null;
			if (flag)
			{
				throw new ArgumentNullException("buffer");
			}
			bool flag2 = offset < 0;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException("offset", "ArgumentOutOfRange_NeedNonNegNum");
			}
			bool flag3 = count < 0;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException("count", "ArgumentOutOfRange_NeedNonNegNum");
			}
			bool flag4 = buffer.Length - offset < count;
			if (flag4)
			{
				throw new ArgumentException("Argument_InvalidOffLen");
			}
			this.SetPipePolicy(this.bulkOutPipeId, 3U, timeout);
			fixed (byte* ptr = buffer)
			{
				uint num;
				bool flag5 = !NativeMethods.WinUsbWritePipe(this.usbHandle, this.bulkOutPipeId, ptr + offset, (uint)count, out num, (NativeOverlapped*)((void*)IntPtr.Zero));
				if (flag5)
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					bool flag6 = 121 != lastWin32Error;
					if (flag6)
					{
						throw new Win32Exception(lastWin32Error);
					}
				}
			}
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00008C6C File Offset: 0x00006E6C
		private static SafeFileHandle CreateDeviceHandle(string deviceName)
		{
			return NativeMethods.CreateFile(deviceName, 3221225472U, 3U, IntPtr.Zero, 3U, 1073741952U, IntPtr.Zero);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00008C9C File Offset: 0x00006E9C
		private void CloseDeviceHandle()
		{
			bool flag = IntPtr.Zero != this.usbHandle;
			if (flag)
			{
				NativeMethods.WinUsbFree(this.usbHandle);
			}
			bool flag2 = !this.deviceHandle.IsInvalid && !this.deviceHandle.IsClosed;
			if (flag2)
			{
				this.deviceHandle.Close();
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00008CFC File Offset: 0x00006EFC
		private void InitializeDevice()
		{
			WinUsbInterfaceDescriptor winUsbInterfaceDescriptor = default(WinUsbInterfaceDescriptor);
			WinUsbPipeInformation winUsbPipeInformation = default(WinUsbPipeInformation);
			bool flag = !NativeMethods.WinUsbInitialize(this.deviceHandle, ref this.usbHandle);
			if (flag)
			{
				throw new IOException("WinUsb Initialization failed.");
			}
			bool flag2 = !NativeMethods.WinUsbQueryInterfaceSettings(this.usbHandle, 0, ref winUsbInterfaceDescriptor);
			if (flag2)
			{
				throw new IOException("WinUsb Query Interface Settings failed.");
			}
			byte b2;
			for (byte b = 0; b < winUsbInterfaceDescriptor.NumEndpoints; b = b2 + 1)
			{
				bool flag3 = !NativeMethods.WinUsbQueryPipe(this.usbHandle, 0, b, ref winUsbPipeInformation);
				if (flag3)
				{
					throw new IOException("WinUsb Query Pipe Information failed");
				}
				WinUsbPipeType pipeType = winUsbPipeInformation.PipeType;
				if (pipeType == WinUsbPipeType.Bulk)
				{
					bool flag4 = this.IsBulkInEndpoint(winUsbPipeInformation.PipeId);
					if (flag4)
					{
						this.SetupBulkInEndpoint(winUsbPipeInformation.PipeId);
					}
					else
					{
						bool flag5 = this.IsBulkOutEndpoint(winUsbPipeInformation.PipeId);
						if (!flag5)
						{
							throw new IOException("Invalid Endpoint Type.");
						}
						this.SetupBulkOutEndpoint(winUsbPipeInformation.PipeId);
					}
				}
				b2 = b;
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00008E10 File Offset: 0x00007010
		private bool IsBulkInEndpoint(byte pipeId)
		{
			return (pipeId & 128) == 128;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00008E30 File Offset: 0x00007030
		private bool IsBulkOutEndpoint(byte pipeId)
		{
			return (pipeId & 128) == 0;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00008E4C File Offset: 0x0000704C
		private void SetupBulkInEndpoint(byte pipeId)
		{
			this.bulkInPipeId = pipeId;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00008E56 File Offset: 0x00007056
		private void SetupBulkOutEndpoint(byte pipeId)
		{
			this.bulkOutPipeId = pipeId;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00008E60 File Offset: 0x00007060
		private void SetPipePolicy(byte pipeId, uint policyType, uint value)
		{
			bool flag = !NativeMethods.WinUsbSetPipePolicy(this.usbHandle, pipeId, policyType, (uint)Marshal.SizeOf(typeof(uint)), ref value);
			if (flag)
			{
				throw new IOException("WinUsb SetPipe Policy failed.");
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00008EA0 File Offset: 0x000070A0
		protected void Dispose(bool disposing)
		{
			bool flag = !this.isDisposed;
			if (flag)
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
					this.isDisposed = true;
				}
			}
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00008EF8 File Offset: 0x000070F8
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00008F0C File Offset: 0x0000710C
		~DTSFUsbStream()
		{
			this.Dispose(false);
		}

		// Token: 0x04000160 RID: 352
		private const byte UsbEndpointDirectionMask = 128;

		// Token: 0x04000161 RID: 353
		private readonly string mDeviceName;

		// Token: 0x04000162 RID: 354
		private SafeFileHandle deviceHandle;

		// Token: 0x04000163 RID: 355
		private IntPtr usbHandle;

		// Token: 0x04000164 RID: 356
		private byte bulkInPipeId;

		// Token: 0x04000165 RID: 357
		private byte bulkOutPipeId;

		// Token: 0x04000166 RID: 358
		private bool isDisposed;
	}
}
