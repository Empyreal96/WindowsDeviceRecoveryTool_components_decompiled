using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000055 RID: 85
	internal static class NativeMethods
	{
		// Token: 0x060001EB RID: 491
		[DllImport("UpdateDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		internal static extern int IU_GetCanonicalUNCPath(string strPath, StringBuilder pathBuffer, int cchPathBuffer);

		// Token: 0x060001EC RID: 492
		[DllImport("UpdateDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		internal static extern int IU_FreeStringList(IntPtr rgFiles, int cFiles);

		// Token: 0x060001ED RID: 493
		[DllImport("UpdateDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool IU_FileExists(string strFile);

		// Token: 0x060001EE RID: 494
		[DllImport("UpdateDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool IU_DirectoryExists(string strDir);

		// Token: 0x060001EF RID: 495
		[DllImport("UpdateDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		internal static extern void IU_EnsureDirectoryExists(string path);

		// Token: 0x060001F0 RID: 496
		[DllImport("UpdateDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		internal static extern void IU_CleanDirectory(string strPath, [MarshalAs(UnmanagedType.Bool)] bool bRemoveDirectory);

		// Token: 0x060001F1 RID: 497
		[DllImport("UpdateDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		internal static extern int IU_GetAllFiles(string strFolder, string strSearchPattern, [MarshalAs(UnmanagedType.Bool)] bool fRecursive, out IntPtr rgFiles, out int cFiles);

		// Token: 0x060001F2 RID: 498
		[DllImport("UpdateDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		internal static extern int IU_GetAllDirectories(string strFolder, string strSearchPattern, [MarshalAs(UnmanagedType.Bool)] bool fRecursive, out IntPtr rgDirectories, out int cDirectories);

		// Token: 0x060001F3 RID: 499 RVA: 0x0000A1C4 File Offset: 0x000083C4
		internal static int MakeHRFromErrorCode(int errorCode)
		{
			return -2147024896 | errorCode;
		}

		// Token: 0x060001F4 RID: 500
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int FormatMessage(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, StringBuilder lpBuffer, int nSize, IntPtr va_list_arguments);

		// Token: 0x060001F5 RID: 501
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool DeleteFile(string lpFileName);

		// Token: 0x060001F6 RID: 502
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool RemoveDirectory(string lpPathName);

		// Token: 0x060001F7 RID: 503
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool MoveFile(string lpPathNameFrom, string lpPathNameTo);

		// Token: 0x060001F8 RID: 504
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CopyFile(string src, string dst, [MarshalAs(UnmanagedType.Bool)] bool failIfExists);

		// Token: 0x060001F9 RID: 505
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern SafeFileHandle CreateFile(string lpFileName, NativeMethods.EFileAccess dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

		// Token: 0x060001FA RID: 506
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern FileAttributes GetFileAttributes(string lpFileName);

		// Token: 0x060001FB RID: 507
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool SetFileAttributes(string lpFileName, FileAttributes attributes);

		// Token: 0x04000143 RID: 323
		private const string STRING_IUCOMMON_DLL = "UpdateDLL.dll";

		// Token: 0x04000144 RID: 324
		private const CallingConvention CALLING_CONVENTION = CallingConvention.Cdecl;

		// Token: 0x04000145 RID: 325
		private const CharSet CHAR_SET = CharSet.Unicode;

		// Token: 0x04000146 RID: 326
		internal const int ERROR_FILE_NOT_FOUND = 2;

		// Token: 0x04000147 RID: 327
		internal const int ERROR_PATH_NOT_FOUND = 3;

		// Token: 0x04000148 RID: 328
		internal const int ERROR_ACCESS_DENIED = 5;

		// Token: 0x04000149 RID: 329
		internal const int ERROR_INVALID_DRIVE = 15;

		// Token: 0x0400014A RID: 330
		internal const int ERROR_NO_MORE_FILES = 18;

		// Token: 0x0400014B RID: 331
		internal const int ERROR_INVALID_NAME = 123;

		// Token: 0x0400014C RID: 332
		internal const int ERROR_ALREADY_EXISTS = 183;

		// Token: 0x0400014D RID: 333
		internal const int ERROR_FILENAME_EXCED_RANGE = 206;

		// Token: 0x0400014E RID: 334
		internal const int ERROR_DIRECTORY = 267;

		// Token: 0x0400014F RID: 335
		internal const int ERROR_OPERATION_ABORTED = 995;

		// Token: 0x04000150 RID: 336
		internal const int INVALID_FILE_ATTRIBUTES = -1;

		// Token: 0x04000151 RID: 337
		internal const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		// Token: 0x04000152 RID: 338
		internal const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		// Token: 0x04000153 RID: 339
		internal const int FORMAT_MESSAGE_ARGUMENT_ARRAY = 8192;

		// Token: 0x04000154 RID: 340
		private const string STRING_KERNEL32_DLL = "kernel32.dll";

		// Token: 0x02000056 RID: 86
		[Flags]
		internal enum EFileAccess : uint
		{
			// Token: 0x04000156 RID: 342
			GenericRead = 2147483648U,
			// Token: 0x04000157 RID: 343
			GenericWrite = 1073741824U,
			// Token: 0x04000158 RID: 344
			GenericExecute = 536870912U,
			// Token: 0x04000159 RID: 345
			GenericAll = 268435456U
		}
	}
}
