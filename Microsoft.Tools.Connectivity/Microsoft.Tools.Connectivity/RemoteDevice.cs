using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using Interop.SirepClient;

namespace Microsoft.Tools.Connectivity
{
	// Token: 0x02000007 RID: 7
	[CLSCompliant(true)]
	public class RemoteDevice
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000023A3 File Offset: 0x000005A3
		// (set) Token: 0x06000021 RID: 33 RVA: 0x000023AB File Offset: 0x000005AB
		public string UserName { get; set; }

		// Token: 0x1700000D RID: 13
		// (set) Token: 0x06000022 RID: 34 RVA: 0x000023B4 File Offset: 0x000005B4
		public string Password
		{
			set
			{
				this.securePassword = new SecureString();
				for (int i = 0; i < value.Length; i++)
				{
					char c = value[i];
					this.securePassword.AppendChar(c);
				}
				this.securePassword.MakeReadOnly();
			}
		}

		// Token: 0x1700000E RID: 14
		// (set) Token: 0x06000023 RID: 35 RVA: 0x000023FE File Offset: 0x000005FE
		public SecureString SecurePassword
		{
			set
			{
				this.securePassword = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00002407 File Offset: 0x00000607
		// (set) Token: 0x06000025 RID: 37 RVA: 0x0000240F File Offset: 0x0000060F
		public TimeSpan Timeout { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002418 File Offset: 0x00000618
		public Guid UniqueId
		{
			get
			{
				return this.uniqueId;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002420 File Offset: 0x00000620
		public IPAddress IpAddress
		{
			get
			{
				IPAddress result = null;
				if (this.ipEndPoint != null)
				{
					result = this.ipEndPoint.Address;
				}
				return result;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002444 File Offset: 0x00000644
		public RemoteDevice.TransportProtocol Protocol
		{
			get
			{
				return this.protocol;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000029 RID: 41 RVA: 0x0000244C File Offset: 0x0000064C
		public bool IsRunAsLoggedOnSupported
		{
			get
			{
				int num = -1;
				try
				{
					num = this.ProtocolRevision();
				}
				catch
				{
				}
				return num >= 4;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002480 File Offset: 0x00000680
		internal bool IsFileInfoSupported
		{
			get
			{
				int num = -1;
				try
				{
					num = this.ProtocolRevision();
				}
				catch
				{
				}
				return num >= 3;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000024B4 File Offset: 0x000006B4
		// (set) Token: 0x0600002C RID: 44 RVA: 0x000024BC File Offset: 0x000006BC
		public bool IsConnected { get; internal set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600002D RID: 45 RVA: 0x000024C5 File Offset: 0x000006C5
		internal SirepClientClass SirepClient
		{
			get
			{
				return this.sirepClient;
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600002E RID: 46 RVA: 0x000024D0 File Offset: 0x000006D0
		// (remove) Token: 0x0600002F RID: 47 RVA: 0x00002508 File Offset: 0x00000708
		public event EventHandler Disconnected;

		// Token: 0x06000030 RID: 48 RVA: 0x0000253D File Offset: 0x0000073D
		public RemoteDevice(Guid uniqueId) : this()
		{
			this.uniqueId = uniqueId;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x0000254C File Offset: 0x0000074C
		public RemoteDevice(IPAddress ipAddress) : this(new IPEndPoint(ipAddress, 0))
		{
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000255B File Offset: 0x0000075B
		public RemoteDevice(IPEndPoint ipEndPoint) : this()
		{
			this.ipEndPoint = ipEndPoint;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000256C File Offset: 0x0000076C
		public RemoteDevice()
		{
			this.Timeout = this.DefaultTimeout;
			this.UserName = null;
			this.sirepClient = new SirepClientClass();
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000025D0 File Offset: 0x000007D0
		public static Guid ConvertMacAddressToUniqueId(string macAddress)
		{
			if (string.IsNullOrWhiteSpace(macAddress))
			{
				throw new ArgumentNullException("macAddress");
			}
			PhysicalAddress physicalAddress = PhysicalAddress.Parse(macAddress);
			byte[] addressBytes = physicalAddress.GetAddressBytes();
			byte[] array = new byte[8];
			Array.Copy(addressBytes, 0, array, 2, 6);
			return new Guid(0, 0, 0, array);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002618 File Offset: 0x00000818
		public void Discover()
		{
			if (!this.UniqueId.Equals(Guid.Empty))
			{
				this.Discover(new Guid[]
				{
					this.uniqueId
				});
				return;
			}
			this.Discover(null);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002664 File Offset: 0x00000864
		public void Discover(Guid uniqueId)
		{
			this.Discover(new Guid[]
			{
				uniqueId
			});
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000268C File Offset: 0x0000088C
		public void Discover(IEnumerable<Guid> candidateIds)
		{
			if (!this.UniqueId.Equals(Guid.Empty) && this.IsConnected)
			{
				this.Disconnect();
			}
			TimeSpan timeout = this.Timeout;
			bool flag = false;
			if (candidateIds != null && candidateIds.Count<Guid>() > 0)
			{
				timeout = TimeSpan.FromTicks(this.Timeout.Ticks / 2L);
				try
				{
					this.DiscoverInternal(candidateIds, this.ipEndPoint, timeout, candidateIds.First<Guid>());
					flag = true;
				}
				catch (TimeoutException)
				{
				}
			}
			if (!flag)
			{
				this.DiscoverInternal(candidateIds, this.ipEndPoint, timeout, Guid.Empty);
				flag = true;
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x0000272C File Offset: 0x0000092C
		public void Discover(Guid uniqueId, AutoResetEvent cancelEvent)
		{
			this.Discover(new Guid[]
			{
				this.UniqueId
			}, cancelEvent);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000027BC File Offset: 0x000009BC
		public void Discover(IEnumerable<Guid> candidateIds, AutoResetEvent cancelEvent)
		{
			AutoResetEvent completedEvent = new AutoResetEvent(false);
			Exception exception = null;
			ThreadPool.QueueUserWorkItem(delegate(object o)
			{
				try
				{
					this.Discover(candidateIds);
				}
				catch (Exception exception)
				{
					exception = exception;
				}
				finally
				{
					completedEvent.Set();
				}
			});
			switch (WaitHandle.WaitAny(new WaitHandle[]
			{
				completedEvent,
				cancelEvent
			}))
			{
			case 0:
				if (exception != null)
				{
					throw exception;
				}
				break;
			case 1:
				throw new OperationCanceledException();
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002844 File Offset: 0x00000A44
		public void Connect()
		{
			if (this.IsConnected)
			{
				this.Disconnect();
			}
			if (this.ipEndPoint == null)
			{
				this.Discover();
			}
			if (this.ipEndPoint == null)
			{
				throw new OperationFailedException("Device wasn't found");
			}
			SirepEndpointInfo localEndpoint;
			localEndpoint.wszIPAddress = "127.0.0.1";
			localEndpoint.usSirepPort = 0;
			localEndpoint.usEchoPort = 0;
			localEndpoint.usProtocol2Port = 0;
			SirepEndpointInfo remoteEndpoint;
			remoteEndpoint.wszIPAddress = this.ipEndPoint.Address.ToString();
			remoteEndpoint.usSirepPort = 0;
			remoteEndpoint.usEchoPort = 0;
			remoteEndpoint.usProtocol2Port = (ushort)this.ipEndPoint.Port;
			if (!string.IsNullOrWhiteSpace(this.UserName))
			{
				this.sirepClient.SirepUser(this.UserName, this.securePassword);
			}
			this.sirepClient.Disconnect += this.RemoteDeviceDisconnected;
			try
			{
				this.sirepClient.SirepInitializeWithEndpoints(localEndpoint, remoteEndpoint);
				this.sirepClient.SirepSetSshPort((ushort)this.SshPort);
				this.sirepClient.SirepConnect((uint)this.Timeout.TotalMilliseconds, false);
				this.protocol = (RemoteDevice.TransportProtocol)this.sirepClient.SirepUsedProtocol();
				this.IsConnected = true;
			}
			catch (COMException ex)
			{
				this.ExceptionHandler(ex);
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000029E0 File Offset: 0x00000BE0
		public void Connect(AutoResetEvent cancelEvent)
		{
			AutoResetEvent completedEvent = new AutoResetEvent(false);
			Exception exception = null;
			ThreadPool.QueueUserWorkItem(delegate(object o)
			{
				try
				{
					this.Connect();
				}
				catch (Exception exception)
				{
					exception = exception;
				}
				finally
				{
					completedEvent.Set();
				}
			});
			switch (WaitHandle.WaitAny(new WaitHandle[]
			{
				completedEvent,
				cancelEvent
			}))
			{
			case 0:
				if (exception != null)
				{
					throw exception;
				}
				break;
			case 1:
				throw new OperationCanceledException();
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002A64 File Offset: 0x00000C64
		public void Disconnect()
		{
			if (this.IsConnected)
			{
				this.sirepClient.Disconnect -= this.RemoteDeviceDisconnected;
				this.sirepClient.SirepDisconnect();
				this.IsConnected = false;
				if (!this.UniqueId.Equals(Guid.Empty))
				{
					this.ipEndPoint = null;
				}
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002AC0 File Offset: 0x00000CC0
		public bool Ping()
		{
			bool result = false;
			try
			{
				result = this.sirepClient.SirepPing((uint)this.Timeout.TotalMilliseconds);
			}
			catch (COMException ex)
			{
				this.ExceptionHandler(ex);
			}
			return result;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002B74 File Offset: 0x00000D74
		public bool Ping(AutoResetEvent cancelEvent)
		{
			bool ping = false;
			AutoResetEvent completedEvent = new AutoResetEvent(false);
			Exception exception = null;
			ThreadPool.QueueUserWorkItem(delegate(object o)
			{
				try
				{
					if (this.IsConnected)
					{
						ping = this.Ping();
					}
				}
				catch (Exception exception)
				{
					exception = exception;
				}
				finally
				{
					completedEvent.Set();
				}
			});
			switch (WaitHandle.WaitAny(new WaitHandle[]
			{
				completedEvent,
				cancelEvent
			}))
			{
			case 0:
				if (exception != null)
				{
					throw exception;
				}
				break;
			case 1:
				throw new OperationCanceledException();
			}
			return ping;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002C02 File Offset: 0x00000E02
		public RemoteCommand Command(string commandPath)
		{
			return new RemoteCommand(this, commandPath, null);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002C0C File Offset: 0x00000E0C
		public RemoteCommand Command(string commandPath, string arguments)
		{
			return new RemoteCommand(this, commandPath, arguments);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002C18 File Offset: 0x00000E18
		public string RunCommand(string command, string arguments)
		{
			RemoteCommand remoteCommand = new RemoteCommand(this, command, arguments);
			remoteCommand.CaptureOutput = true;
			remoteCommand.Execute();
			return remoteCommand.Output;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002C42 File Offset: 0x00000E42
		public string RunCommand(string command)
		{
			return this.RunCommand(command, null);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002C4C File Offset: 0x00000E4C
		public int StartProcess(string command, string arguments)
		{
			RemoteCommand remoteCommand = new RemoteCommand(this, command, arguments);
			return remoteCommand.CreateProcess();
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002C68 File Offset: 0x00000E68
		public int StartProcess(string command)
		{
			return this.StartProcess(command, null);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002C72 File Offset: 0x00000E72
		public RemoteFile File(string remoteFilePath)
		{
			return new RemoteFile(this, remoteFilePath);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002C7C File Offset: 0x00000E7C
		public void GetFile(string remoteFilePath, string localFilePath, bool overwrite)
		{
			RemoteFile remoteFile = new RemoteFile(this, remoteFilePath);
			remoteFile.Get(localFilePath, overwrite);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002C99 File Offset: 0x00000E99
		public void GetFile(string remoteFilePath, string localFilePath)
		{
			this.GetFile(remoteFilePath, localFilePath, false);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002CA4 File Offset: 0x00000EA4
		public void PutFile(string remoteFilePath, string localFilePath, bool overwrite)
		{
			RemoteFile remoteFile = new RemoteFile(this, remoteFilePath);
			remoteFile.Put(localFilePath, overwrite);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002CC1 File Offset: 0x00000EC1
		public void PutFile(string remoteFilePath, string localFilePath)
		{
			this.PutFile(remoteFilePath, localFilePath, false);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002CCC File Offset: 0x00000ECC
		public void DeleteFile(string remoteFilePath)
		{
			RemoteFile remoteFile = new RemoteFile(this, remoteFilePath);
			remoteFile.Delete();
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002CE7 File Offset: 0x00000EE7
		public RemoteDirectory Directory(string remoteDirPath)
		{
			return new RemoteDirectory(this, remoteDirPath);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002CF0 File Offset: 0x00000EF0
		public void CreateDirectory(string remoteDirPath)
		{
			RemoteDirectory remoteDirectory = new RemoteDirectory(this, remoteDirPath);
			remoteDirectory.Create();
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002D0C File Offset: 0x00000F0C
		public void DeleteDirectory(string remoteDirPath, bool force)
		{
			RemoteDirectory remoteDirectory = new RemoteDirectory(this, remoteDirPath);
			remoteDirectory.Delete(force);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002D28 File Offset: 0x00000F28
		public IEnumerable<string> GetDirectories(string remoteDirPath)
		{
			RemoteDirectory remoteDirectory = new RemoteDirectory(this, remoteDirPath);
			return remoteDirectory.GetDirectories();
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002D44 File Offset: 0x00000F44
		public IEnumerable<string> GetDirectories(string remoteDirPath, string searchPattern, bool recursive)
		{
			RemoteDirectory remoteDirectory = new RemoteDirectory(this, remoteDirPath);
			return remoteDirectory.GetDirectories(searchPattern, recursive);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002D61 File Offset: 0x00000F61
		public IEnumerable<string> GetDirectories(string remoteDirPath, string searchPattern)
		{
			return this.GetDirectories(remoteDirPath, searchPattern, false);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002D6C File Offset: 0x00000F6C
		public IEnumerable<string> GetFiles(string remoteDirPath)
		{
			RemoteDirectory remoteDirectory = new RemoteDirectory(this, remoteDirPath);
			return remoteDirectory.GetFiles();
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002D88 File Offset: 0x00000F88
		public IEnumerable<string> GetFiles(string remoteDirPath, string searchPattern, bool recursive)
		{
			RemoteDirectory remoteDirectory = new RemoteDirectory(this, remoteDirPath);
			return remoteDirectory.GetFiles(searchPattern, recursive);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002DA5 File Offset: 0x00000FA5
		public IEnumerable<string> GetFiles(string remoteDirPath, string searchPattern)
		{
			return this.GetFiles(remoteDirPath, searchPattern, false);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002EC0 File Offset: 0x000010C0
		internal void DiscoverInternal(IEnumerable<Guid> candidateIds, IPEndPoint ipEndPoint, TimeSpan timeout, Guid soughtId)
		{
			DiscoveredDeviceInfo info = null;
			if ((candidateIds == null || candidateIds.Count<Guid>() == 0) && ipEndPoint == null)
			{
				throw new InvalidOperationException("Either a list of candidate Ids or IP Address should be available to start discovering.");
			}
			if (!this.UniqueId.Equals(Guid.Empty) && this.ipEndPoint != null)
			{
				if (this.IsConnected)
				{
					this.Disconnect();
				}
				this.ipEndPoint = null;
			}
			ManualResetEvent completedEvent = new ManualResetEvent(false);
			DeviceDiscoveryService deviceDiscoveryService = new DeviceDiscoveryService();
			deviceDiscoveryService.Discovered += delegate(object s, DiscoveredEventArgs e)
			{
				bool flag = false;
				if (completedEvent != null && completedEvent.WaitOne(0))
				{
					return;
				}
				info = e.Info;
				IPAddress ipaddress = IPAddress.Parse(info.Address);
				if (!soughtId.Equals(Guid.Empty))
				{
					flag = soughtId.Equals(info.UniqueId);
				}
				else if (candidateIds != null)
				{
					flag = candidateIds.Contains(info.UniqueId);
				}
				else if (ipEndPoint != null)
				{
					flag = ipEndPoint.Address.Equals(ipaddress);
				}
				if (flag)
				{
					this.uniqueId = info.UniqueId;
					this.ipEndPoint = new IPEndPoint(ipaddress, info.SirepPort);
					this.SshPort = info.SshPort;
					if (completedEvent != null)
					{
						completedEvent.Set();
					}
				}
			};
			deviceDiscoveryService.Start(soughtId);
			try
			{
				if (!completedEvent.WaitOne(timeout))
				{
					throw new TimeoutException();
				}
			}
			finally
			{
				deviceDiscoveryService.Stop();
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002FB0 File Offset: 0x000011B0
		internal int ProtocolRevision()
		{
			int result = -1;
			try
			{
				result = this.sirepClient.GetSirepProtocolRevision();
			}
			catch (COMException ex)
			{
				this.ExceptionHandler(ex);
			}
			return result;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002FE8 File Offset: 0x000011E8
		internal void EnsureConnection()
		{
			if (!this.IsConnected)
			{
				throw new OperationFailedException("Remote Device Disconnected");
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002FFD File Offset: 0x000011FD
		internal void ExceptionHandler(COMException ex)
		{
			this.ExceptionHandler(ex, null);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003008 File Offset: 0x00001208
		internal void ExceptionHandler(COMException ex, string message)
		{
			string message2 = (!string.IsNullOrWhiteSpace(message)) ? message : ex.Message;
			int errorCode = ex.ErrorCode;
			if (errorCode <= -2147023436)
			{
				if (errorCode != -2147287037)
				{
					switch (errorCode)
					{
					case -2147024894:
						goto IL_E4;
					case -2147024893:
						break;
					case -2147024892:
						goto IL_112;
					case -2147024891:
						throw new AccessDeniedException(message2, ex);
					default:
						if (errorCode != -2147023436)
						{
							goto IL_112;
						}
						throw new TimeoutException(message2, ex);
					}
				}
				throw new ArgumentException(message2, ex);
			}
			if (errorCode <= -2145648626)
			{
				switch (errorCode)
				{
				case -2147014847:
				case -2147014846:
				case -2147014845:
				case -2147014844:
				case -2147014843:
				case -2147014842:
				case -2147014836:
				case -2147014835:
				case -2147014832:
				case -2147014831:
					if (this.IsConnected)
					{
						this.Disconnect();
					}
					throw new OperationFailedException(message2, ex);
				case -2147014841:
				case -2147014840:
				case -2147014839:
				case -2147014838:
				case -2147014837:
				case -2147014834:
				case -2147014833:
					goto IL_112;
				default:
					if (errorCode != -2145648626)
					{
						goto IL_112;
					}
					throw new AccessDeniedException(message2, ex);
				}
			}
			else if (errorCode != -1988886504)
			{
				if (errorCode != -1988886498)
				{
					goto IL_112;
				}
				throw new DirectoryNotFoundException(message2, ex);
			}
			IL_E4:
			throw new FileNotFoundException(message2, ex);
			IL_112:
			throw new OperationFailedException(message2, ex);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003130 File Offset: 0x00001330
		private void RemoteDeviceDisconnected(object sender, EventArgs e)
		{
			this.Disconnect();
			EventHandler disconnected = this.Disconnected;
			if (disconnected != null)
			{
				this.Disconnected(this, e);
			}
		}

		// Token: 0x0400007D RID: 125
		private readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30.0);

		// Token: 0x0400007E RID: 126
		private readonly TimeSpan ConnectionValidationRetryInterval = TimeSpan.FromSeconds(1.0);

		// Token: 0x0400007F RID: 127
		private Guid uniqueId = Guid.Empty;

		// Token: 0x04000080 RID: 128
		private IPEndPoint ipEndPoint;

		// Token: 0x04000081 RID: 129
		private int SshPort;

		// Token: 0x04000082 RID: 130
		private SirepClientClass sirepClient;

		// Token: 0x04000083 RID: 131
		private RemoteDevice.TransportProtocol protocol;

		// Token: 0x04000084 RID: 132
		private SecureString securePassword;

		// Token: 0x02000008 RID: 8
		public enum TransportProtocol
		{
			// Token: 0x0400008A RID: 138
			None,
			// Token: 0x0400008B RID: 139
			Sirep1,
			// Token: 0x0400008C RID: 140
			Sirep2,
			// Token: 0x0400008D RID: 141
			Ssh
		}
	}
}
