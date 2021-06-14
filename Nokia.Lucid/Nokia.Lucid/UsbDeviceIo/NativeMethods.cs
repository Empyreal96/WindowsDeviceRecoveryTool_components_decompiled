using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Nokia.Lucid.UsbDeviceIo
{
	// Token: 0x0200003D RID: 61
	public static class NativeMethods
	{
		// Token: 0x060001AF RID: 431
		[DllImport("winusb.dll", SetLastError = true)]
		internal static extern bool WinUsb_Free(IntPtr interfaceHandle);

		// Token: 0x060001B0 RID: 432
		[DllImport("winusb.dll", SetLastError = true)]
		internal static extern bool WinUsb_Initialize(SafeFileHandle deviceHandle, out IntPtr interfaceHandle);

		// Token: 0x060001B1 RID: 433
		[DllImport("winusb.dll", SetLastError = true)]
		internal static extern bool WinUsb_QueryInterfaceSettings(IntPtr interfaceHandle, byte alternateInterfaceNumber, out NativeMethods.USB_INTERFACE_DESCRIPTOR usbAlternateInterfaceDescriptor);

		// Token: 0x060001B2 RID: 434
		[DllImport("winusb.dll", SetLastError = true)]
		internal static extern bool WinUsb_QueryPipe(IntPtr interfaceHandle, byte alternateInterfaceNumber, byte pipeIndex, out NativeMethods.WINUSB_PIPE_INFORMATION pipeInformation);

		// Token: 0x060001B3 RID: 435
		[DllImport("winusb.dll", SetLastError = true)]
		internal static extern bool WinUsb_SetPipePolicy(IntPtr interfaceHandle, byte pipeId, uint policyType, uint valueLength, ref uint value);

		// Token: 0x060001B4 RID: 436
		[DllImport("winusb.dll", SetLastError = true)]
		internal static extern bool WinUsb_FlushPipe(IntPtr interfaceHandle, byte pipeId);

		// Token: 0x060001B5 RID: 437
		[DllImport("winusb.dll", SetLastError = true)]
		internal static extern bool WinUsb_AbortPipe(IntPtr interfaceHandle, byte pipeId);

		// Token: 0x060001B6 RID: 438
		[DllImport("winusb.dll", SetLastError = true)]
		internal static extern bool WinUsb_ReadPipe(IntPtr interfaceHandle, byte pipeId, byte[] buffer, uint bufferLength, ref uint lengthTransferred, ref NativeMethods.OVERLAPPED overlapped);

		// Token: 0x060001B7 RID: 439
		[DllImport("winusb.dll", SetLastError = true)]
		internal static extern bool WinUsb_WritePipe(IntPtr interfaceHandle, byte pipeId, byte[] buffer, uint bufferLength, ref uint lengthTransferred, ref NativeMethods.OVERLAPPED overlapped);

		// Token: 0x060001B8 RID: 440
		[DllImport("winusb.dll", SetLastError = true)]
		internal static extern bool WinUsb_GetOverlappedResult(IntPtr interfaceHandle, ref NativeMethods.OVERLAPPED overlapped, ref uint numberOfBytesTransferred, bool wait);

		// Token: 0x060001B9 RID: 441
		[DllImport("winusb.dll", SetLastError = true)]
		internal static extern bool WinUsb_GetDescriptor(IntPtr interfaceHandle, byte descriptorType, byte index, ushort languageId, byte[] buffer, uint bufferLength, ref uint lengthTransferred);

		// Token: 0x060001BA RID: 442
		[DllImport("winusb.dll", SetLastError = true)]
		internal static extern bool WinUsb_GetDescriptor(IntPtr interfaceHandle, byte descriptorType, byte index, ushort languageId, out NativeMethods.USB_DEVICE_DESCRIPTOR deviceDesc, uint bufferLength, out uint lengthTransferred);

		// Token: 0x060001BB RID: 443
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern SafeFileHandle CreateFile(string fileName, uint desiredAccess, int shareMode, IntPtr securityAttributes, int creationDisposition, int flagsAndAttributes, IntPtr templateFile);

		// Token: 0x060001BC RID: 444
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool CancelIoEx(IntPtr hFile, IntPtr overlapped);

		// Token: 0x060001BD RID: 445
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool CancelIoEx(IntPtr hFile, ref NativeMethods.OVERLAPPED overlapped);

		// Token: 0x040000E4 RID: 228
		internal const byte USB_ENDPOINT_DIRECTION_MASK = 128;

		// Token: 0x040000E5 RID: 229
		internal const int FILE_ATTRIBUTE_NORMAL = 128;

		// Token: 0x040000E6 RID: 230
		internal const int FILE_FLAG_OVERLAPPED = 1073741824;

		// Token: 0x040000E7 RID: 231
		internal const int FILE_SHARE_READ = 1;

		// Token: 0x040000E8 RID: 232
		internal const int FILE_SHARE_WRITE = 2;

		// Token: 0x040000E9 RID: 233
		internal const uint GENERIC_READ = 2147483648U;

		// Token: 0x040000EA RID: 234
		internal const uint GENERIC_WRITE = 1073741824U;

		// Token: 0x040000EB RID: 235
		internal const int INVALID_HANDLE_VALUE = -1;

		// Token: 0x040000EC RID: 236
		internal const int OPEN_EXISTING = 3;

		// Token: 0x040000ED RID: 237
		internal const int ERROR_IO_PENDING = 997;

		// Token: 0x040000EE RID: 238
		internal const int USB_DEVICE_DESCRIPTOR_TYPE = 1;

		// Token: 0x0200003E RID: 62
		internal enum POLICY_TYPE
		{
			// Token: 0x040000F0 RID: 240
			SHORT_PACKET_TERMINATE = 1,
			// Token: 0x040000F1 RID: 241
			AUTO_CLEAR_STALL,
			// Token: 0x040000F2 RID: 242
			PIPE_TRANSFER_TIMEOUT,
			// Token: 0x040000F3 RID: 243
			IGNORE_SHORT_PACKETS,
			// Token: 0x040000F4 RID: 244
			ALLOW_PARTIAL_READS,
			// Token: 0x040000F5 RID: 245
			AUTO_FLUSH,
			// Token: 0x040000F6 RID: 246
			RAW_IO
		}

		// Token: 0x0200003F RID: 63
		internal enum USBD_PIPE_TYPE
		{
			// Token: 0x040000F8 RID: 248
			UsbdPipeTypeControl,
			// Token: 0x040000F9 RID: 249
			UsbdPipeTypeIsochronous,
			// Token: 0x040000FA RID: 250
			UsbdPipeTypeBulk,
			// Token: 0x040000FB RID: 251
			UsbdPipeTypeInterrupt
		}

		// Token: 0x02000040 RID: 64
		[CLSCompliant(false)]
		public struct USB_DEVICE_DESCRIPTOR
		{
			// Token: 0x040000FC RID: 252
			public byte bLength;

			// Token: 0x040000FD RID: 253
			public byte bDescriptorType;

			// Token: 0x040000FE RID: 254
			public ushort bcdUsb;

			// Token: 0x040000FF RID: 255
			public byte bDeviceClass;

			// Token: 0x04000100 RID: 256
			public byte bDeviceSubClass;

			// Token: 0x04000101 RID: 257
			public byte bDeviceProtocol;

			// Token: 0x04000102 RID: 258
			public byte bMaxPacketSize0;

			// Token: 0x04000103 RID: 259
			public ushort idVendor;

			// Token: 0x04000104 RID: 260
			public ushort idProduct;

			// Token: 0x04000105 RID: 261
			public ushort bcdDevice;

			// Token: 0x04000106 RID: 262
			public byte iManufacturer;

			// Token: 0x04000107 RID: 263
			public byte iProduct;

			// Token: 0x04000108 RID: 264
			public byte iSerialNumber;

			// Token: 0x04000109 RID: 265
			public byte bNumConfigurations;
		}

		// Token: 0x02000041 RID: 65
		internal struct USB_INTERFACE_DESCRIPTOR
		{
			// Token: 0x0400010A RID: 266
			internal byte bLength;

			// Token: 0x0400010B RID: 267
			internal byte bDescriptorType;

			// Token: 0x0400010C RID: 268
			internal byte bInterfaceNumber;

			// Token: 0x0400010D RID: 269
			internal byte bAlternateSetting;

			// Token: 0x0400010E RID: 270
			internal byte bNumEndpoints;

			// Token: 0x0400010F RID: 271
			internal byte bInterfaceClass;

			// Token: 0x04000110 RID: 272
			internal byte bInterfaceSubClass;

			// Token: 0x04000111 RID: 273
			internal byte bInterfaceProtocol;

			// Token: 0x04000112 RID: 274
			internal byte iInterface;
		}

		// Token: 0x02000042 RID: 66
		internal struct WINUSB_PIPE_INFORMATION
		{
			// Token: 0x04000113 RID: 275
			internal NativeMethods.USBD_PIPE_TYPE PipeType;

			// Token: 0x04000114 RID: 276
			internal byte PipeId;

			// Token: 0x04000115 RID: 277
			internal ushort MaximumPacketSize;

			// Token: 0x04000116 RID: 278
			internal byte Interval;
		}

		// Token: 0x02000043 RID: 67
		internal struct OVERLAPPED
		{
			// Token: 0x04000117 RID: 279
			internal IntPtr InternalLow;

			// Token: 0x04000118 RID: 280
			internal IntPtr InternalHigh;

			// Token: 0x04000119 RID: 281
			internal int OffsetLow;

			// Token: 0x0400011A RID: 282
			internal int OffsetHigh;

			// Token: 0x0400011B RID: 283
			internal IntPtr EventHandle;
		}
	}
}
