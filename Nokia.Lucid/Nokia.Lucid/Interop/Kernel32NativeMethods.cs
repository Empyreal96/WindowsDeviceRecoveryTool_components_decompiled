using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace Nokia.Lucid.Interop
{
	// Token: 0x02000030 RID: 48
	internal static class Kernel32NativeMethods
	{
		// Token: 0x06000138 RID: 312
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetModuleHandle(IntPtr lpModuleName);

		// Token: 0x06000139 RID: 313
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern IntPtr GetModuleHandle(string lpModuleName);

		// Token: 0x0600013A RID: 314
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern WNDPROC GetProcAddress(IntPtr hModule, string lpProcName);

		// Token: 0x0600013B RID: 315
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", ExactSpelling = true)]
		public static extern int GetCurrentThreadId();

		// Token: 0x040000C5 RID: 197
		private const string Kernel32DllName = "kernel32.dll";
	}
}
