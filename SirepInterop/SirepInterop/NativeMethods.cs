using System;
using System.Runtime.InteropServices;

namespace Interop.SirepClient
{
	// Token: 0x02000006 RID: 6
	internal class NativeMethods
	{
		// Token: 0x0600001E RID: 30
		[DllImport("SirepClient.dll")]
		public static extern IntPtr GetSirepClient();

		// Token: 0x0600001F RID: 31
		[DllImport("SirepClient.dll")]
		public static extern void ReleaseSirepClient(IntPtr handle);

		// Token: 0x06000020 RID: 32
		[DllImport("SirepClient.dll")]
		public static extern int CreateProcessOnTarget(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string command, [MarshalAs(UnmanagedType.LPWStr)] string arguments, [MarshalAs(UnmanagedType.LPWStr)] string workingDirectory, uint launchFlags, out uint pid);

		// Token: 0x06000021 RID: 33
		[DllImport("SirepClient.dll")]
		public static extern int GetClientSideClientIdentifier(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] out string clientSideClientIdentifier);

		// Token: 0x06000022 RID: 34
		[DllImport("SirepClient.dll")]
		public static extern int GetExitCode(IntPtr handle, uint pid);

		// Token: 0x06000023 RID: 35
		[DllImport("SirepClient.dll")]
		public static extern int GetLastProcessHandle(IntPtr handle, out uint pid);

		// Token: 0x06000024 RID: 36
		[DllImport("SirepClient.dll")]
		public static extern int GetServerSideClientIdentifier(IntPtr handle, out ulong serverSideClientIdentifier);

		// Token: 0x06000025 RID: 37
		[DllImport("SirepClient.dll")]
		public static extern int GetSirepProtocolRevision(IntPtr handle);

		// Token: 0x06000026 RID: 38
		[DllImport("SirepClient.dll")]
		public static extern int IsDeviceSideFailure(IntPtr handle, out bool isDeviceSideFailure);

		// Token: 0x06000027 RID: 39
		[DllImport("SirepClient.dll")]
		public static extern int IsProcessRunning(IntPtr handle, uint pid);

		// Token: 0x06000028 RID: 40
		[DllImport("SirepClient.dll")]
		public static extern int KillLaunchedProcess(IntPtr handle, uint pid);

		// Token: 0x06000029 RID: 41
		[DllImport("SirepClient.dll")]
		public static extern int LaunchWithOutput(IntPtr handle, uint timeout, [MarshalAs(UnmanagedType.LPWStr)] string command, [MarshalAs(UnmanagedType.LPWStr)] string arguments, [MarshalAs(UnmanagedType.LPWStr)] string workingDirectory, uint launchFlags, [MarshalAs(UnmanagedType.Interface)] ILaunchWithOutputCB outputCallback, out uint exitCode);

		// Token: 0x0600002A RID: 42
		[DllImport("SirepClient.dll")]
		public static extern int ResetFailureInfo(IntPtr handle);

		// Token: 0x0600002B RID: 43
		[DllImport("SirepClient.dll")]
		public static extern int SetClientSideClientIdentifierPrefix(IntPtr handle, [MarshalAs(UnmanagedType.BStr)] string clientSideClientIdentifierPrefix);

		// Token: 0x0600002C RID: 44
		[DllImport("SirepClient.dll")]
		public static extern int SetKeepAliveTimeout(IntPtr handle, uint timeoutInMs, uint retryIntervalInMs);

		// Token: 0x0600002D RID: 45
		[DllImport("SirepClient.dll")]
		public static extern int SetSendReceiveTimeout(IntPtr handle, uint timeoutInMs);

		// Token: 0x0600002E RID: 46
		[DllImport("SirepClient.dll")]
		public static extern int SirepAbort(IntPtr handle, uint cookieId);

		// Token: 0x0600002F RID: 47
		[DllImport("SirepClient.dll")]
		public static extern void SirepUser(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string user, IntPtr pin);

		// Token: 0x06000030 RID: 48
		[DllImport("SirepClient.dll")]
		public static extern int SirepConnect(IntPtr handle, uint timeoutInMs, bool isAsynch, out uint cookieId);

		// Token: 0x06000031 RID: 49
		[DllImport("SirepClient.dll")]
		public static extern int SirepDisconnect(IntPtr handle);

		// Token: 0x06000032 RID: 50
		[DllImport("SirepClient.dll")]
		public static extern int SirepGetCurrentState(IntPtr handle, out uint currentState);

		// Token: 0x06000033 RID: 51
		[DllImport("SirepClient.dll")]
		public static extern int SirepGetDeviceInfo(IntPtr handle, DeviceInfo deviceInfo, out uint value);

		// Token: 0x06000034 RID: 52
		[DllImport("SirepClient.dll")]
		public static extern int SirepGetFile(IntPtr handle, uint timeoutInMs, [MarshalAs(UnmanagedType.LPWStr)] string srcFullPath, [MarshalAs(UnmanagedType.LPWStr)] string destFullPath, bool overwrite = false);

		// Token: 0x06000035 RID: 53
		[DllImport("SirepClient.dll")]
		public static extern int SirepGetFileInfo(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string deviceFile, out FileInfox fileInfo);

		// Token: 0x06000036 RID: 54
		[DllImport("SirepClient.dll")]
		public static extern int SirepInitialize(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string ipAddress);

		// Token: 0x06000037 RID: 55
		[DllImport("SirepClient.dll")]
		public static extern int SirepInitializeWithEndpoints(IntPtr handle, ref SirepEndpointInfo pepLocal, ref SirepEndpointInfo pepRemote);

		// Token: 0x06000038 RID: 56
		[DllImport("SirepClient.dll")]
		public static extern int SirepLaunch(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string command, [MarshalAs(UnmanagedType.LPWStr)] string arguments, uint launchFlags);

		// Token: 0x06000039 RID: 57
		[DllImport("SirepClient.dll")]
		public static extern int SirepLockService(IntPtr handle);

		// Token: 0x0600003A RID: 58
		[DllImport("SirepClient.dll")]
		public static extern int SirepPing(IntPtr handle, uint timeoutInMs, out bool responded);

		// Token: 0x0600003B RID: 59
		[DllImport("SirepClient.dll")]
		public static extern int SirepPutFile(IntPtr handle, uint timeoutInMs, bool isAsynch, [MarshalAs(UnmanagedType.LPWStr)] string srcFullPath, [MarshalAs(UnmanagedType.LPWStr)] string destFullPath, bool overwrite, out uint cookieId);

		// Token: 0x0600003C RID: 60
		[DllImport("SirepClient.dll")]
		public static extern int SirepRemoveFile(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string fileFullPath);

		// Token: 0x0600003D RID: 61
		[DllImport("SirepClient.dll")]
		public static extern int SirepDirectoryEnum(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string fullDirectoryPath, [MarshalAs(UnmanagedType.Interface)] ILaunchWithOutputCB outputCallback);

		// Token: 0x0600003E RID: 62
		[DllImport("SirepClient.dll")]
		public static extern int SirepCreateDirectory(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string directoryFullPath);

		// Token: 0x0600003F RID: 63
		[DllImport("SirepClient.dll")]
		public static extern int SirepRemoveDirectory(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string directoryFullPath);

		// Token: 0x06000040 RID: 64
		[DllImport("SirepClient.dll")]
		public static extern int SirepUsedProtocol(IntPtr handle);

		// Token: 0x06000041 RID: 65
		[DllImport("SirepClient.dll")]
		public static extern int SirepUnlockService(IntPtr handle);

		// Token: 0x06000042 RID: 66
		[DllImport("SirepClient.dll")]
		public static extern void SirepSetDisconnectCallback(IntPtr context, IntPtr callbackContext, [MarshalAs(UnmanagedType.FunctionPtr)] NativeMethods.SirepDisconnectCallback callback);

		// Token: 0x06000043 RID: 67
		[DllImport("SirepClient.dll")]
		public static extern void SirepSetSshPort(IntPtr handle, ushort sshPort);

		// Token: 0x06000044 RID: 68
		[DllImport("SirepClient.dll")]
		public static extern IntPtr BeginSirepEnum(uint options, IntPtr context, [MarshalAs(UnmanagedType.LPWStr)] string soughtGuid, [MarshalAs(UnmanagedType.FunctionPtr)] NativeMethods.SirepEnumCallback callback);

		// Token: 0x06000045 RID: 69
		[DllImport("SirepClient.dll")]
		public static extern IntPtr EndSirepEnum(IntPtr handle);

		// Token: 0x02000007 RID: 7
		// (Invoke) Token: 0x06000048 RID: 72
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate void SirepDisconnectCallback(IntPtr context);

		// Token: 0x02000008 RID: 8
		// (Invoke) Token: 0x0600004C RID: 76
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate void SirepEnumCallback(IntPtr context, uint source, ref Guid guid, [MarshalAs(UnmanagedType.LPWStr)] string name, [MarshalAs(UnmanagedType.LPWStr)] string location, [MarshalAs(UnmanagedType.LPWStr)] string address, [MarshalAs(UnmanagedType.LPWStr)] string architecture, [MarshalAs(UnmanagedType.LPWStr)] string osVersion, ushort sirepPort, ushort sshPort);
	}
}
