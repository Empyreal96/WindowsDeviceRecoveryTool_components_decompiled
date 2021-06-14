using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x0200004A RID: 74
	public static class VirtualDiskLib
	{
		// Token: 0x06000184 RID: 388
		[CLSCompliant(false)]
		[DllImport("Virtdisk.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		public static extern int OpenVirtualDisk(ref VIRTUAL_STORAGE_TYPE VirtualStorageType, [MarshalAs(UnmanagedType.LPWStr)] string Path, VIRTUAL_DISK_ACCESS_MASK VirtualDiskAccessMask, OPEN_VIRTUAL_DISK_FLAG Flags, ref OPEN_VIRTUAL_DISK_PARAMETERS Parameters, ref IntPtr Handle);

		// Token: 0x06000185 RID: 389
		[CLSCompliant(false)]
		[DllImport("Virtdisk.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		public static extern int OpenVirtualDisk(ref VIRTUAL_STORAGE_TYPE VirtualStorageType, [MarshalAs(UnmanagedType.LPWStr)] string Path, VIRTUAL_DISK_ACCESS_MASK VirtualDiskAccessMask, OPEN_VIRTUAL_DISK_FLAG Flags, IntPtr Parameters, ref IntPtr Handle);

		// Token: 0x06000186 RID: 390
		[DllImport("Virtdisk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern int GetVirtualDiskPhysicalPath(IntPtr VirtualDiskHandle, ref int DiskPathSizeInBytes, StringBuilder DiskPath);

		// Token: 0x06000187 RID: 391
		[CLSCompliant(false)]
		[DllImport("Virtdisk.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		public static extern int AttachVirtualDisk(IntPtr VirtualDiskHandle, IntPtr SecurityDescriptor, ATTACH_VIRTUAL_DISK_FLAG Flags, uint ProviderSpecificFlags, ref ATTACH_VIRTUAL_DISK_PARAMETERS Parameters, IntPtr Overlapped);

		// Token: 0x06000188 RID: 392
		[CLSCompliant(false)]
		[DllImport("Virtdisk.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		public static extern int DetachVirtualDisk(IntPtr VirtualDiskHandle, DETACH_VIRTUAL_DISK_FLAG Flags, uint ProviderSpecificFlags);

		// Token: 0x06000189 RID: 393
		[CLSCompliant(false)]
		[DllImport("Virtdisk.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		public static extern int CreateVirtualDisk(ref VIRTUAL_STORAGE_TYPE VirtualStorageType, [MarshalAs(UnmanagedType.LPWStr)] string Path, VIRTUAL_DISK_ACCESS_MASK VirtualDiskAccessMask, IntPtr SecurityDescriptor, CREATE_VIRTUAL_DISK_FLAG Flags, uint ProviderSpecificFlags, ref CREATE_VIRTUAL_DISK_PARAMETERS Parameters, IntPtr Overlapped, ref IntPtr Handle);

		// Token: 0x0600018A RID: 394
		[DllImport("kernel32.dll")]
		public static extern bool CloseHandle(IntPtr hObject);
	}
}
