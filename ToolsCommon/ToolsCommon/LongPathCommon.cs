using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000057 RID: 87
	internal static class LongPathCommon
	{
		// Token: 0x060001FC RID: 508 RVA: 0x0000A1CD File Offset: 0x000083CD
		internal static Exception GetExceptionFromLastWin32Error()
		{
			return LongPathCommon.GetExceptionFromLastWin32Error("path");
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000A1D9 File Offset: 0x000083D9
		internal static Exception GetExceptionFromLastWin32Error(string parameterName)
		{
			return LongPathCommon.GetExceptionFromWin32Error(Marshal.GetLastWin32Error(), parameterName);
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000A1E6 File Offset: 0x000083E6
		internal static Exception GetExceptionFromWin32Error(int errorCode)
		{
			return LongPathCommon.GetExceptionFromWin32Error(errorCode, "path");
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000A1F4 File Offset: 0x000083F4
		internal static Exception GetExceptionFromWin32Error(int errorCode, string parameterName)
		{
			string messageFromErrorCode = LongPathCommon.GetMessageFromErrorCode(errorCode);
			if (errorCode <= 15)
			{
				switch (errorCode)
				{
				case 2:
					return new FileNotFoundException(messageFromErrorCode);
				case 3:
					return new DirectoryNotFoundException(messageFromErrorCode);
				case 4:
					break;
				case 5:
					return new UnauthorizedAccessException(messageFromErrorCode);
				default:
					if (errorCode == 15)
					{
						return new DriveNotFoundException(messageFromErrorCode);
					}
					break;
				}
			}
			else
			{
				if (errorCode == 123)
				{
					return new ArgumentException(messageFromErrorCode, parameterName);
				}
				if (errorCode == 206)
				{
					return new PathTooLongException(messageFromErrorCode);
				}
				if (errorCode == 995)
				{
					return new OperationCanceledException(messageFromErrorCode);
				}
			}
			return new IOException(messageFromErrorCode, NativeMethods.MakeHRFromErrorCode(errorCode));
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000A284 File Offset: 0x00008484
		private static string GetMessageFromErrorCode(int errorCode)
		{
			StringBuilder stringBuilder = new StringBuilder(512);
			NativeMethods.FormatMessage(12800, IntPtr.Zero, errorCode, 0, stringBuilder, stringBuilder.Capacity, IntPtr.Zero);
			return stringBuilder.ToString();
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000A2C0 File Offset: 0x000084C0
		internal static string[] ConvertPtrArrayToStringArray(IntPtr strPtrArray, int cStrings)
		{
			IntPtr[] array = new IntPtr[cStrings];
			if (strPtrArray != IntPtr.Zero)
			{
				Marshal.Copy(strPtrArray, array, 0, cStrings);
			}
			List<string> list = new List<string>(cStrings);
			for (int i = 0; i < cStrings; i++)
			{
				list.Add(Marshal.PtrToStringUni(array[i]));
			}
			return list.ToArray();
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000A31C File Offset: 0x0000851C
		public static string NormalizeLongPath(string path)
		{
			StringBuilder stringBuilder = new StringBuilder(LongPathCommon.MAX_LONG_PATH);
			int num = NativeMethods.IU_GetCanonicalUNCPath(path, stringBuilder, stringBuilder.Capacity);
			if (num != 0)
			{
				throw LongPathCommon.GetExceptionFromWin32Error(num);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000A354 File Offset: 0x00008554
		public static FileAttributes GetAttributes(string path)
		{
			string lpFileName = LongPathCommon.NormalizeLongPath(path);
			FileAttributes fileAttributes = NativeMethods.GetFileAttributes(lpFileName);
			if (fileAttributes == (FileAttributes)(-1))
			{
				throw LongPathCommon.GetExceptionFromLastWin32Error();
			}
			return fileAttributes;
		}

		// Token: 0x0400015A RID: 346
		private static int MAX_LONG_PATH = 32000;
	}
}
