using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Interop.SirepClient
{
	// Token: 0x02000009 RID: 9
	public class SirepClientClass : ISirepClient, IDisposable
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600004F RID: 79 RVA: 0x000020D8 File Offset: 0x000002D8
		// (remove) Token: 0x06000050 RID: 80 RVA: 0x00002110 File Offset: 0x00000310
		public event EventHandler Disconnect;

		// Token: 0x06000051 RID: 81 RVA: 0x00002148 File Offset: 0x00000348
		public SirepClientClass()
		{
			this.sirepClient = NativeMethods.GetSirepClient();
			this.globalHandle = GCHandle.Alloc(this);
			this.disconnectCallback = new NativeMethods.SirepDisconnectCallback(SirepClientClass.DisconnectCallback);
			NativeMethods.SirepSetDisconnectCallback(this.sirepClient, GCHandle.ToIntPtr(this.globalHandle), this.disconnectCallback);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000021A0 File Offset: 0x000003A0
		~SirepClientClass()
		{
			this.Dispose(false);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000021D0 File Offset: 0x000003D0
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000021E0 File Offset: 0x000003E0
		protected virtual void Dispose(bool disposing)
		{
			lock (SirepClientClass.globalHandleMutex)
			{
				if (this.globalHandle.IsAllocated)
				{
					this.globalHandle.Free();
				}
			}
			if (disposing)
			{
				this.Disconnect = null;
			}
			if (this.sirepClient != IntPtr.Zero)
			{
				NativeMethods.ReleaseSirepClient(this.sirepClient);
				this.sirepClient = IntPtr.Zero;
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002264 File Offset: 0x00000464
		public uint CreateProcess(string command, string arguments, string workingDirectory, uint launchFlags)
		{
			uint result;
			int num = NativeMethods.CreateProcessOnTarget(this.sirepClient, command, arguments, workingDirectory, launchFlags, out result);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			return result;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002290 File Offset: 0x00000490
		public string GetClientSideClientIdentifier()
		{
			string result;
			int clientSideClientIdentifier = NativeMethods.GetClientSideClientIdentifier(this.sirepClient, out result);
			if (clientSideClientIdentifier != 0)
			{
				Marshal.ThrowExceptionForHR(clientSideClientIdentifier);
			}
			return result;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000022B8 File Offset: 0x000004B8
		public void GetExitCode(uint pid)
		{
			int exitCode = NativeMethods.GetExitCode(this.sirepClient, pid);
			if (exitCode != 0)
			{
				Marshal.ThrowExceptionForHR(exitCode);
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000022DC File Offset: 0x000004DC
		public uint GetLastProcessHandle()
		{
			uint result;
			int lastProcessHandle = NativeMethods.GetLastProcessHandle(this.sirepClient, out result);
			if (lastProcessHandle != 0)
			{
				Marshal.ThrowExceptionForHR(lastProcessHandle);
			}
			return result;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002304 File Offset: 0x00000504
		public ulong GetServerSideClientIdentifier()
		{
			ulong result;
			int serverSideClientIdentifier = NativeMethods.GetServerSideClientIdentifier(this.sirepClient, out result);
			if (serverSideClientIdentifier != 0)
			{
				Marshal.ThrowExceptionForHR(serverSideClientIdentifier);
			}
			return result;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002329 File Offset: 0x00000529
		public int GetSirepProtocolRevision()
		{
			return NativeMethods.GetSirepProtocolRevision(this.sirepClient);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002338 File Offset: 0x00000538
		public bool IsDeviceSideFailure()
		{
			bool result;
			int num = NativeMethods.IsDeviceSideFailure(this.sirepClient, out result);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			return result;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002360 File Offset: 0x00000560
		public void IsProcessRunning(uint pid)
		{
			int num = NativeMethods.IsProcessRunning(this.sirepClient, pid);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002384 File Offset: 0x00000584
		public void KillLaunchedProcess(uint handle)
		{
			int num = NativeMethods.KillLaunchedProcess(this.sirepClient, handle);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000023A8 File Offset: 0x000005A8
		public uint LaunchWithOutput(uint timeoutInMs, string command, string arguments, string workingDirectory, uint launchFlags, ILaunchWithOutputCB outputCallback)
		{
			uint result;
			int num = NativeMethods.LaunchWithOutput(this.sirepClient, timeoutInMs, command, arguments, workingDirectory, launchFlags, outputCallback, out result);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			return result;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000023D8 File Offset: 0x000005D8
		public void ResetFailureInfo()
		{
			int num = NativeMethods.ResetFailureInfo(this.sirepClient);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000023FC File Offset: 0x000005FC
		public void SetClientSideClientIdentifierPrefix(string clientSideClientIdentifierPrefix)
		{
			int num = NativeMethods.SetClientSideClientIdentifierPrefix(this.sirepClient, clientSideClientIdentifierPrefix);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002420 File Offset: 0x00000620
		public void SetKeepAliveTimeout(uint timeoutInMs, uint retryIntervalInMs)
		{
			int num = NativeMethods.SetKeepAliveTimeout(this.sirepClient, timeoutInMs, retryIntervalInMs);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002444 File Offset: 0x00000644
		public void SetSendReceiveTimeout(uint timeoutInMs)
		{
			int num = NativeMethods.SetSendReceiveTimeout(this.sirepClient, timeoutInMs);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002468 File Offset: 0x00000668
		public void SirepAbort(uint cookieId)
		{
			int num = NativeMethods.SirepAbort(this.sirepClient, cookieId);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000248C File Offset: 0x0000068C
		public uint SirepConnect(uint timeoutInMs, bool runAsync)
		{
			uint result;
			int num = NativeMethods.SirepConnect(this.sirepClient, timeoutInMs, runAsync, out result);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			return result;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000024B4 File Offset: 0x000006B4
		public void SirepUser(string user, SecureString password)
		{
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				if (password != null)
				{
					intPtr = Marshal.SecureStringToGlobalAllocUnicode(password);
				}
				NativeMethods.SirepUser(this.sirepClient, user, intPtr);
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.ZeroFreeGlobalAllocUnicode(intPtr);
				}
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002504 File Offset: 0x00000704
		public void SirepDisconnect()
		{
			int num = NativeMethods.SirepDisconnect(this.sirepClient);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002528 File Offset: 0x00000728
		public uint SirepGetCurrentState()
		{
			uint result;
			int num = NativeMethods.SirepGetCurrentState(this.sirepClient, out result);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			return result;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002550 File Offset: 0x00000750
		public uint SirepGetDeviceInfo(DeviceInfo deviceInfo)
		{
			uint result;
			int num = NativeMethods.SirepGetDeviceInfo(this.sirepClient, deviceInfo, out result);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			return result;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00002578 File Offset: 0x00000778
		public void SirepGetFile(uint timeoutInMs, string srcFullPath, string destFullPath, bool overwrite)
		{
			int num = NativeMethods.SirepGetFile(this.sirepClient, timeoutInMs, srcFullPath, destFullPath, overwrite);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000025A0 File Offset: 0x000007A0
		public void SirepGetFile(uint timeoutInMs, string srcFullPath, string destFullPath)
		{
			int num = NativeMethods.SirepGetFile(this.sirepClient, timeoutInMs, srcFullPath, destFullPath, false);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000025C8 File Offset: 0x000007C8
		public RemoteFileInfo SirepGetFileInfo(string filePath)
		{
			FileInfox fileInfox;
			int num = NativeMethods.SirepGetFileInfo(this.sirepClient, filePath, out fileInfox);
			if (num != 0)
			{
				return null;
			}
			return new RemoteFileInfo(ref fileInfox);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000025F0 File Offset: 0x000007F0
		public void SirepInitialize(string ipAddress)
		{
			int num = NativeMethods.SirepInitialize(this.sirepClient, ipAddress);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002614 File Offset: 0x00000814
		public void SirepInitializeWithEndpoints(SirepEndpointInfo localEndpoint, SirepEndpointInfo remoteEndpoint)
		{
			int num = NativeMethods.SirepInitializeWithEndpoints(this.sirepClient, ref localEndpoint, ref remoteEndpoint);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000263C File Offset: 0x0000083C
		public void SirepLaunch(string command, string arguments, uint launchFlags)
		{
			int num = NativeMethods.SirepLaunch(this.sirepClient, command, arguments, launchFlags);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00002664 File Offset: 0x00000864
		public void SirepLockService()
		{
			int num = NativeMethods.SirepLockService(this.sirepClient);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00002688 File Offset: 0x00000888
		public bool SirepPing([In] uint timeoutInMs)
		{
			bool result;
			int num = NativeMethods.SirepPing(this.sirepClient, timeoutInMs, out result);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			return result;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000026B0 File Offset: 0x000008B0
		public uint SirepPutFile(uint timeoutInMs, bool runAsync, string srcFullPath, string destFullPath, bool overwrite)
		{
			uint result;
			int num = NativeMethods.SirepPutFile(this.sirepClient, timeoutInMs, runAsync, srcFullPath, destFullPath, overwrite, out result);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			return result;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000026DC File Offset: 0x000008DC
		public uint SirepPutFile(uint timeoutInMs, bool runAsync, string srcFullPath, string destFullPath)
		{
			uint result;
			int num = NativeMethods.SirepPutFile(this.sirepClient, timeoutInMs, runAsync, srcFullPath, destFullPath, false, out result);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			return result;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00002708 File Offset: 0x00000908
		public void SirepRemoveFile(string fileFullPath)
		{
			int num = NativeMethods.SirepRemoveFile(this.sirepClient, fileFullPath);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000272C File Offset: 0x0000092C
		public void SirepUnlockService()
		{
			int num = NativeMethods.SirepUnlockService(this.sirepClient);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00002750 File Offset: 0x00000950
		public void SirepCreateDirectory(string directoryFullPath)
		{
			int num = NativeMethods.SirepCreateDirectory(this.sirepClient, directoryFullPath);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00002774 File Offset: 0x00000974
		public void SirepRemoveDirectory(string directoryFullPath)
		{
			int num = NativeMethods.SirepRemoveDirectory(this.sirepClient, directoryFullPath);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00002798 File Offset: 0x00000998
		public void SirepDirectoryEnum(string directoryFullPath, ILaunchWithOutputCB outputCallback)
		{
			int num = NativeMethods.SirepDirectoryEnum(this.sirepClient, directoryFullPath, outputCallback);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000027BC File Offset: 0x000009BC
		public int SirepUsedProtocol()
		{
			return NativeMethods.SirepUsedProtocol(this.sirepClient);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000027C9 File Offset: 0x000009C9
		public void SirepSetSshPort(ushort sshPort)
		{
			NativeMethods.SirepSetSshPort(this.sirepClient, sshPort);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000027D8 File Offset: 0x000009D8
		private static void DisconnectCallback(IntPtr context)
		{
			lock (SirepClientClass.globalHandleMutex)
			{
				SirepClientClass sirepClientClass = (SirepClientClass)GCHandle.FromIntPtr(context).Target;
				if (sirepClientClass != null)
				{
					EventHandler disconnect = sirepClientClass.Disconnect;
					if (disconnect != null)
					{
						sirepClientClass.Disconnect(sirepClientClass, EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x04000017 RID: 23
		private IntPtr sirepClient;

		// Token: 0x04000018 RID: 24
		private NativeMethods.SirepDisconnectCallback disconnectCallback;

		// Token: 0x04000019 RID: 25
		private GCHandle globalHandle;

		// Token: 0x0400001B RID: 27
		private static object globalHandleMutex = new object();
	}
}
