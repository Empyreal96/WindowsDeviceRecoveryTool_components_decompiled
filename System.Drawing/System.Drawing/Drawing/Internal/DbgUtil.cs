using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace System.Drawing.Internal
{
	// Token: 0x020000D8 RID: 216
	[ReflectionPermission(SecurityAction.Assert, MemberAccess = true)]
	[EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
	[FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
	[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
	[UIPermission(SecurityAction.Assert, Unrestricted = true)]
	internal sealed class DbgUtil
	{
		// Token: 0x06000B6F RID: 2927
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetUserDefaultLCID();

		// Token: 0x06000B70 RID: 2928
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int FormatMessage(int dwFlags, HandleRef lpSource, int dwMessageId, int dwLanguageId, StringBuilder lpBuffer, int nSize, HandleRef arguments);

		// Token: 0x06000B71 RID: 2929 RVA: 0x00015255 File Offset: 0x00013455
		[Conditional("DEBUG")]
		public static void AssertFinalization(object obj, bool disposing)
		{
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x00015255 File Offset: 0x00013455
		[Conditional("DEBUG")]
		public static void AssertWin32(bool expression, string message)
		{
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x00015255 File Offset: 0x00013455
		[Conditional("DEBUG")]
		public static void AssertWin32(bool expression, string format, object arg1)
		{
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x00015255 File Offset: 0x00013455
		[Conditional("DEBUG")]
		public static void AssertWin32(bool expression, string format, object arg1, object arg2)
		{
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x00015255 File Offset: 0x00013455
		[Conditional("DEBUG")]
		public static void AssertWin32(bool expression, string format, object arg1, object arg2, object arg3)
		{
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x00015255 File Offset: 0x00013455
		[Conditional("DEBUG")]
		public static void AssertWin32(bool expression, string format, object arg1, object arg2, object arg3, object arg4)
		{
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x00015255 File Offset: 0x00013455
		[Conditional("DEBUG")]
		public static void AssertWin32(bool expression, string format, object arg1, object arg2, object arg3, object arg4, object arg5)
		{
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x00015255 File Offset: 0x00013455
		[Conditional("DEBUG")]
		private static void AssertWin32Impl(bool expression, string format, object[] args)
		{
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x00029DDC File Offset: 0x00027FDC
		public static string GetLastErrorStr()
		{
			int num = 255;
			StringBuilder stringBuilder = new StringBuilder(num);
			string text = string.Empty;
			int num2 = 0;
			try
			{
				num2 = Marshal.GetLastWin32Error();
				text = ((DbgUtil.FormatMessage(4608, new HandleRef(null, IntPtr.Zero), num2, DbgUtil.GetUserDefaultLCID(), stringBuilder, num, new HandleRef(null, IntPtr.Zero)) != 0) ? stringBuilder.ToString() : "<error returned>");
			}
			catch (Exception ex)
			{
				if (DbgUtil.IsCriticalException(ex))
				{
					throw;
				}
				text = ex.ToString();
			}
			return string.Format(CultureInfo.CurrentCulture, "0x{0:x8} - {1}", new object[]
			{
				num2,
				text
			});
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x00029E8C File Offset: 0x0002808C
		private static bool IsCriticalException(Exception ex)
		{
			return ex is StackOverflowException || ex is OutOfMemoryException || ex is ThreadAbortException;
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06000B7B RID: 2939 RVA: 0x00029EA9 File Offset: 0x000280A9
		public static string StackTrace
		{
			get
			{
				return Environment.StackTrace;
			}
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x00029EB0 File Offset: 0x000280B0
		public static string StackFramesToStr(int maxFrameCount)
		{
			string text = string.Empty;
			try
			{
				StackTrace stackTrace = new StackTrace(true);
				int i;
				for (i = 0; i < stackTrace.FrameCount; i++)
				{
					StackFrame frame = stackTrace.GetFrame(i);
					if (frame == null || frame.GetMethod().DeclaringType != typeof(DbgUtil))
					{
						break;
					}
				}
				maxFrameCount += i;
				if (maxFrameCount > stackTrace.FrameCount)
				{
					maxFrameCount = stackTrace.FrameCount;
				}
				for (int j = i; j < maxFrameCount; j++)
				{
					StackFrame frame2 = stackTrace.GetFrame(j);
					if (frame2 != null)
					{
						MethodBase method = frame2.GetMethod();
						if (!(method == null))
						{
							string text2 = string.Empty;
							string text3 = frame2.GetFileName();
							int num = (text3 == null) ? -1 : text3.LastIndexOf('\\');
							if (num != -1)
							{
								text3 = text3.Substring(num + 1, text3.Length - num - 1);
							}
							foreach (ParameterInfo parameterInfo in method.GetParameters())
							{
								text2 = text2 + parameterInfo.ParameterType.Name + ", ";
							}
							if (text2.Length > 0)
							{
								text2 = text2.Substring(0, text2.Length - 2);
							}
							text += string.Format(CultureInfo.CurrentCulture, "at {0} {1}.{2}({3})\r\n", new object[]
							{
								text3,
								method.DeclaringType,
								method.Name,
								text2
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (DbgUtil.IsCriticalException(ex))
				{
					throw;
				}
				text += ex.ToString();
			}
			return text.ToString();
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x0002A06C File Offset: 0x0002826C
		public static string StackFramesToStr()
		{
			return DbgUtil.StackFramesToStr(DbgUtil.gdipInitMaxFrameCount);
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x0002A078 File Offset: 0x00028278
		public static string StackTraceToStr(string message, int frameCount)
		{
			return string.Format(CultureInfo.CurrentCulture, "{0}\r\nTop Stack Trace:\r\n{1}", new object[]
			{
				message,
				DbgUtil.StackFramesToStr(frameCount)
			});
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x0002A09C File Offset: 0x0002829C
		public static string StackTraceToStr(string message)
		{
			return DbgUtil.StackTraceToStr(message, DbgUtil.gdipInitMaxFrameCount);
		}

		// Token: 0x04000A24 RID: 2596
		public const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 256;

		// Token: 0x04000A25 RID: 2597
		public const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		// Token: 0x04000A26 RID: 2598
		public const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		// Token: 0x04000A27 RID: 2599
		public const int FORMAT_MESSAGE_DEFAULT = 4608;

		// Token: 0x04000A28 RID: 2600
		public static int gdipInitMaxFrameCount = 8;

		// Token: 0x04000A29 RID: 2601
		public static int gdiUseMaxFrameCount = 8;

		// Token: 0x04000A2A RID: 2602
		public static int finalizeMaxFrameCount = 5;
	}
}
