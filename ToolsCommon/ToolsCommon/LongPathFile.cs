using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x0200005A RID: 90
	public static class LongPathFile
	{
		// Token: 0x0600021D RID: 541 RVA: 0x0000A820 File Offset: 0x00008A20
		public static bool Exists(string path)
		{
			return NativeMethods.IU_FileExists(path);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000A828 File Offset: 0x00008A28
		public static FileAttributes GetAttributes(string path)
		{
			return LongPathCommon.GetAttributes(path);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000A840 File Offset: 0x00008A40
		public static void SetAttributes(string path, FileAttributes attributes)
		{
			string lpFileName = LongPathCommon.NormalizeLongPath(path);
			if (!NativeMethods.SetFileAttributes(lpFileName, attributes))
			{
				throw LongPathCommon.GetExceptionFromLastWin32Error();
			}
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000A864 File Offset: 0x00008A64
		public static void Delete(string path)
		{
			string lpFileName = LongPathCommon.NormalizeLongPath(path);
			if (!LongPathFile.Exists(path))
			{
				return;
			}
			if (!NativeMethods.DeleteFile(lpFileName))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error != 2)
				{
					throw LongPathCommon.GetExceptionFromWin32Error(lastWin32Error);
				}
			}
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000A89C File Offset: 0x00008A9C
		public static void Move(string sourcePath, string destinationPath)
		{
			string lpPathNameFrom = LongPathCommon.NormalizeLongPath(sourcePath);
			string lpPathNameTo = LongPathCommon.NormalizeLongPath(destinationPath);
			if (!NativeMethods.MoveFile(lpPathNameFrom, lpPathNameTo))
			{
				throw LongPathCommon.GetExceptionFromLastWin32Error();
			}
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000A8C8 File Offset: 0x00008AC8
		public static void Copy(string sourcePath, string destinationPath, bool overwrite)
		{
			string src = LongPathCommon.NormalizeLongPath(sourcePath);
			string dst = LongPathCommon.NormalizeLongPath(destinationPath);
			if (!NativeMethods.CopyFile(src, dst, !overwrite))
			{
				throw LongPathCommon.GetExceptionFromLastWin32Error();
			}
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000A8F6 File Offset: 0x00008AF6
		public static void Copy(string sourcePath, string destinationPath)
		{
			LongPathFile.Copy(sourcePath, destinationPath, false);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000A900 File Offset: 0x00008B00
		public static FileStream Open(string path, FileMode mode, FileAccess access)
		{
			return LongPathFile.Open(path, mode, access, FileShare.None, 0, FileOptions.None);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000A90D File Offset: 0x00008B0D
		public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share)
		{
			return LongPathFile.Open(path, mode, access, share, 0, FileOptions.None);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000A91C File Offset: 0x00008B1C
		public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options)
		{
			if (bufferSize == 0)
			{
				bufferSize = 1024;
			}
			string normalizedPath = LongPathCommon.NormalizeLongPath(path);
			SafeFileHandle fileHandle = LongPathFile.GetFileHandle(normalizedPath, mode, access, share, options);
			FileStream fileStream = new FileStream(fileHandle, access, bufferSize, (options & FileOptions.Asynchronous) == FileOptions.Asynchronous);
			if (mode == FileMode.Append)
			{
				fileStream.Seek(0L, SeekOrigin.End);
			}
			return fileStream;
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000A96F File Offset: 0x00008B6F
		public static FileStream OpenRead(string path)
		{
			return LongPathFile.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000A97A File Offset: 0x00008B7A
		public static FileStream OpenWrite(string path)
		{
			return LongPathFile.Open(path, FileMode.Create, FileAccess.ReadWrite);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000A984 File Offset: 0x00008B84
		public static StreamWriter CreateText(string path)
		{
			return new StreamWriter(LongPathFile.Open(path, FileMode.Create, FileAccess.ReadWrite), Encoding.UTF8);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000A998 File Offset: 0x00008B98
		public static byte[] ReadAllBytes(string path)
		{
			byte[] result;
			using (FileStream fileStream = LongPathFile.OpenRead(path))
			{
				byte[] array = new byte[fileStream.Length];
				fileStream.Read(array, 0, array.Length);
				result = array;
			}
			return result;
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000A9E4 File Offset: 0x00008BE4
		public static void WriteAllBytes(string path, byte[] contents)
		{
			using (FileStream fileStream = LongPathFile.OpenWrite(path))
			{
				fileStream.Write(contents, 0, contents.Length);
			}
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000AA20 File Offset: 0x00008C20
		public static string ReadAllText(string path, Encoding encoding)
		{
			string result;
			using (StreamReader streamReader = new StreamReader(LongPathFile.OpenRead(path), encoding, true))
			{
				result = streamReader.ReadToEnd();
			}
			return result;
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000AA60 File Offset: 0x00008C60
		public static string ReadAllText(string path)
		{
			return LongPathFile.ReadAllText(path, Encoding.Default);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000AA70 File Offset: 0x00008C70
		public static void WriteAllText(string path, string contents, Encoding encoding)
		{
			using (StreamWriter streamWriter = new StreamWriter(LongPathFile.OpenWrite(path), encoding))
			{
				streamWriter.Write(contents);
			}
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000AAB0 File Offset: 0x00008CB0
		public static void WriteAllText(string path, string contents)
		{
			LongPathFile.WriteAllText(path, contents, new UTF8Encoding(false));
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000AAC0 File Offset: 0x00008CC0
		public static string[] ReadAllLines(string path, Encoding encoding)
		{
			string[] result;
			using (StreamReader streamReader = new StreamReader(LongPathFile.OpenRead(path), encoding, true))
			{
				List<string> list = new List<string>();
				while (!streamReader.EndOfStream)
				{
					list.Add(streamReader.ReadLine());
				}
				result = list.ToArray();
			}
			return result;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000AB1C File Offset: 0x00008D1C
		public static string[] ReadAllLines(string path)
		{
			return LongPathFile.ReadAllLines(path, Encoding.Default);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000AB2C File Offset: 0x00008D2C
		public static void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
		{
			using (StreamWriter streamWriter = new StreamWriter(LongPathFile.OpenWrite(path), encoding))
			{
				foreach (string value in contents)
				{
					streamWriter.WriteLine(value);
				}
			}
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000AB9C File Offset: 0x00008D9C
		public static void WriteAllLines(string path, IEnumerable<string> contents)
		{
			LongPathFile.WriteAllLines(path, contents, new UTF8Encoding(false));
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000ABAC File Offset: 0x00008DAC
		public static void AppendAllText(string path, string contents, Encoding encoding)
		{
			using (StreamWriter streamWriter = new StreamWriter(LongPathFile.Open(path, FileMode.Append, FileAccess.ReadWrite), encoding))
			{
				streamWriter.Write(contents);
			}
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000ABEC File Offset: 0x00008DEC
		public static void AppendAllText(string path, string contents)
		{
			LongPathFile.AppendAllText(path, contents, new UTF8Encoding(false));
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000ABFC File Offset: 0x00008DFC
		private static SafeFileHandle GetFileHandle(string normalizedPath, FileMode mode, FileAccess access, FileShare share, FileOptions options)
		{
			NativeMethods.EFileAccess underlyingAccess = LongPathFile.GetUnderlyingAccess(access);
			FileMode underlyingMode = LongPathFile.GetUnderlyingMode(mode);
			SafeFileHandle safeFileHandle = NativeMethods.CreateFile(normalizedPath, underlyingAccess, (uint)share, IntPtr.Zero, (uint)underlyingMode, (uint)options, IntPtr.Zero);
			if (safeFileHandle.IsInvalid)
			{
				throw LongPathCommon.GetExceptionFromLastWin32Error();
			}
			return safeFileHandle;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000AC3C File Offset: 0x00008E3C
		private static FileMode GetUnderlyingMode(FileMode mode)
		{
			if (mode == FileMode.Append)
			{
				return FileMode.OpenOrCreate;
			}
			return mode;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000AC48 File Offset: 0x00008E48
		private static NativeMethods.EFileAccess GetUnderlyingAccess(FileAccess access)
		{
			switch (access)
			{
			case FileAccess.Read:
				return (NativeMethods.EFileAccess)2147483648U;
			case FileAccess.Write:
				return NativeMethods.EFileAccess.GenericWrite;
			case FileAccess.ReadWrite:
				return (NativeMethods.EFileAccess)3221225472U;
			default:
				throw new ArgumentOutOfRangeException("access");
			}
		}
	}
}
