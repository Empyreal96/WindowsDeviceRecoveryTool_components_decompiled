using System;

namespace Interop.SirepClient
{
	// Token: 0x02000005 RID: 5
	public interface ISirepClient
	{
		// Token: 0x06000002 RID: 2
		void SirepInitialize(string ipAddress);

		// Token: 0x06000003 RID: 3
		void SirepInitializeWithEndpoints(SirepEndpointInfo localEndpoint, SirepEndpointInfo remoteEndpoint);

		// Token: 0x06000004 RID: 4
		uint SirepConnect(uint timeoutInMs, bool runAsync);

		// Token: 0x06000005 RID: 5
		uint SirepGetCurrentState();

		// Token: 0x06000006 RID: 6
		void SirepDisconnect();

		// Token: 0x06000007 RID: 7
		void SirepAbort(uint cookieId);

		// Token: 0x06000008 RID: 8
		void SirepLockService();

		// Token: 0x06000009 RID: 9
		void SirepUnlockService();

		// Token: 0x0600000A RID: 10
		uint SirepPutFile(uint timeoutInMs, bool runAsync, string srcFullPath, string destFullPath, bool overwrite = false);

		// Token: 0x0600000B RID: 11
		void SirepLaunch(string command, string arguments, uint launchFlags);

		// Token: 0x0600000C RID: 12
		uint SirepGetDeviceInfo(DeviceInfo deviceInfo);

		// Token: 0x0600000D RID: 13
		RemoteFileInfo SirepGetFileInfo(string filePath);

		// Token: 0x0600000E RID: 14
		void SirepGetFile(uint timeoutInMs, string srcFullPath, string destFullPath, bool overwrite = false);

		// Token: 0x0600000F RID: 15
		uint LaunchWithOutput(uint timeoutInMs, string command, string arguments, string workingDirectory, uint launchFlags, ILaunchWithOutputCB outputCallback);

		// Token: 0x06000010 RID: 16
		uint CreateProcess(string command, string arguments, string workingDirectory, uint launchFlags);

		// Token: 0x06000011 RID: 17
		bool SirepPing(uint timeoutInMs);

		// Token: 0x06000012 RID: 18
		void KillLaunchedProcess(uint pid);

		// Token: 0x06000013 RID: 19
		void IsProcessRunning(uint pid);

		// Token: 0x06000014 RID: 20
		void GetExitCode(uint pid);

		// Token: 0x06000015 RID: 21
		uint GetLastProcessHandle();

		// Token: 0x06000016 RID: 22
		void SetClientSideClientIdentifierPrefix(string clientSideClientIdentifierPrefix);

		// Token: 0x06000017 RID: 23
		string GetClientSideClientIdentifier();

		// Token: 0x06000018 RID: 24
		ulong GetServerSideClientIdentifier();

		// Token: 0x06000019 RID: 25
		void SetKeepAliveTimeout(uint timeoutInMs, uint retryIntervalInMs);

		// Token: 0x0600001A RID: 26
		void SetSendReceiveTimeout(uint timeoutInMs);

		// Token: 0x0600001B RID: 27
		void ResetFailureInfo();

		// Token: 0x0600001C RID: 28
		bool IsDeviceSideFailure();

		// Token: 0x0600001D RID: 29
		int GetSirepProtocolRevision();
	}
}
