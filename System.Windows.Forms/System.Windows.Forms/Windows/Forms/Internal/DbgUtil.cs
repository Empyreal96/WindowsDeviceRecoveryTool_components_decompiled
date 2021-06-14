using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace System.Windows.Forms.Internal
{
	// Token: 0x02000501 RID: 1281
	[ReflectionPermission(SecurityAction.Assert, MemberAccess = true)]
	[EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
	[FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
	[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
	[UIPermission(SecurityAction.Assert, Unrestricted = true)]
	internal sealed class DbgUtil
	{
		// Token: 0x0600541E RID: 21534
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetUserDefaultLCID();

		// Token: 0x0600541F RID: 21535
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int FormatMessage(int dwFlags, HandleRef lpSource, int dwMessageId, int dwLanguageId, StringBuilder lpBuffer, int nSize, HandleRef arguments);

		// Token: 0x06005420 RID: 21536 RVA: 0x0000701A File Offset: 0x0000521A
		[Conditional("DEBUG")]
		public static void AssertFinalization(object obj, bool disposing)
		{
		}

		// Token: 0x06005421 RID: 21537 RVA: 0x0000701A File Offset: 0x0000521A
		[Conditional("DEBUG")]
		public static void AssertWin32(bool expression, string message)
		{
		}

		// Token: 0x06005422 RID: 21538 RVA: 0x0000701A File Offset: 0x0000521A
		[Conditional("DEBUG")]
		public static void AssertWin32(bool expression, string format, object arg1)
		{
		}

		// Token: 0x06005423 RID: 21539 RVA: 0x0000701A File Offset: 0x0000521A
		[Conditional("DEBUG")]
		public static void AssertWin32(bool expression, string format, object arg1, object arg2)
		{
		}

		// Token: 0x06005424 RID: 21540 RVA: 0x0000701A File Offset: 0x0000521A
		[Conditional("DEBUG")]
		public static void AssertWin32(bool expression, string format, object arg1, object arg2, object arg3)
		{
		}

		// Token: 0x06005425 RID: 21541 RVA: 0x0000701A File Offset: 0x0000521A
		[Conditional("DEBUG")]
		public static void AssertWin32(bool expression, string format, object arg1, object arg2, object arg3, object arg4)
		{
		}

		// Token: 0x06005426 RID: 21542 RVA: 0x0000701A File Offset: 0x0000521A
		[Conditional("DEBUG")]
		public static void AssertWin32(bool expression, string format, object arg1, object arg2, object arg3, object arg4, object arg5)
		{
		}

		// Token: 0x06005427 RID: 21543 RVA: 0x0000701A File Offset: 0x0000521A
		[Conditional("DEBUG")]
		private static void AssertWin32Impl(bool expression, string format, object[] args)
		{
		}

		// Token: 0x06005428 RID: 21544 RVA: 0x0015F2D4 File Offset: 0x0015D4D4
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

		// Token: 0x06005429 RID: 21545 RVA: 0x0015F384 File Offset: 0x0015D584
		private static bool IsCriticalException(Exception ex)
		{
			return ex is StackOverflowException || ex is OutOfMemoryException || ex is ThreadAbortException;
		}

		// Token: 0x17001448 RID: 5192
		// (get) Token: 0x0600542A RID: 21546 RVA: 0x0015F3A1 File Offset: 0x0015D5A1
		public static string StackTrace
		{
			get
			{
				return Environment.StackTrace;
			}
		}

		// Token: 0x0600542B RID: 21547 RVA: 0x0015F3A8 File Offset: 0x0015D5A8
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

		// Token: 0x0600542C RID: 21548 RVA: 0x0015F564 File Offset: 0x0015D764
		public static string StackFramesToStr()
		{
			return DbgUtil.StackFramesToStr(DbgUtil.gdipInitMaxFrameCount);
		}

		// Token: 0x0600542D RID: 21549 RVA: 0x0015F570 File Offset: 0x0015D770
		public static string StackTraceToStr(string message, int frameCount)
		{
			return string.Format(CultureInfo.CurrentCulture, "{0}\r\nTop Stack Trace:\r\n{1}", new object[]
			{
				message,
				DbgUtil.StackFramesToStr(frameCount)
			});
		}

		// Token: 0x0600542E RID: 21550 RVA: 0x0015F594 File Offset: 0x0015D794
		public static string StackTraceToStr(string message)
		{
			return DbgUtil.StackTraceToStr(message, DbgUtil.gdipInitMaxFrameCount);
		}

		// Token: 0x04003634 RID: 13876
		public const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 256;

		// Token: 0x04003635 RID: 13877
		public const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		// Token: 0x04003636 RID: 13878
		public const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		// Token: 0x04003637 RID: 13879
		public const int FORMAT_MESSAGE_DEFAULT = 4608;

		// Token: 0x04003638 RID: 13880
		public static int gdipInitMaxFrameCount = 8;

		// Token: 0x04003639 RID: 13881
		public static int gdiUseMaxFrameCount = 8;

		// Token: 0x0400363A RID: 13882
		public static int finalizeMaxFrameCount = 5;
	}
}
