using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using Interop.SirepClient;

namespace Microsoft.Tools.Connectivity
{
	// Token: 0x0200000B RID: 11
	[CLSCompliant(true)]
	public class RemoteFile
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000070 RID: 112 RVA: 0x0000341C File Offset: 0x0000161C
		// (set) Token: 0x06000071 RID: 113 RVA: 0x00003424 File Offset: 0x00001624
		public TimeSpan Timeout { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00003430 File Offset: 0x00001630
		public bool Exists
		{
			get
			{
				RemoteFileInfo remoteFileInfo = null;
				try
				{
					remoteFileInfo = this.Info();
				}
				catch
				{
				}
				return remoteFileInfo != null;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00003464 File Offset: 0x00001664
		public DateTime LastWriteTime
		{
			get
			{
				RemoteFileInfo remoteFileInfo = null;
				try
				{
					remoteFileInfo = this.Info();
				}
				catch
				{
				}
				if (remoteFileInfo == null)
				{
					return DateTime.MinValue;
				}
				return remoteFileInfo.LastWriteTime;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000074 RID: 116 RVA: 0x000034A0 File Offset: 0x000016A0
		public long Length
		{
			get
			{
				RemoteFileInfo remoteFileInfo = null;
				try
				{
					remoteFileInfo = this.Info();
				}
				catch
				{
				}
				if (remoteFileInfo == null)
				{
					return -1L;
				}
				return remoteFileInfo.FileSize;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000075 RID: 117 RVA: 0x000034D8 File Offset: 0x000016D8
		internal RemoteDevice RemoteDevice
		{
			get
			{
				return this.remoteDevice;
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000034E0 File Offset: 0x000016E0
		internal RemoteFile(RemoteDevice remoteDevice, string fileNamePath)
		{
			this.remoteDevice = remoteDevice;
			this.remoteFileName = fileNamePath;
			this.Timeout = this.remoteDevice.Timeout;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003508 File Offset: 0x00001708
		public void Put(string localFileName, bool overwrite)
		{
			this.RemoteDevice.EnsureConnection();
			try
			{
				this.RemoteDevice.SirepClient.SirepPutFile((uint)this.Timeout.TotalMilliseconds, false, localFileName, this.remoteFileName, overwrite);
			}
			catch (COMException ex)
			{
				this.RemoteDevice.ExceptionHandler(ex);
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000356C File Offset: 0x0000176C
		public void Put(string localFileName)
		{
			this.Put(localFileName, false);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003578 File Offset: 0x00001778
		public void Get(string localFileName, bool overwrite)
		{
			this.RemoteDevice.EnsureConnection();
			try
			{
				this.RemoteDevice.SirepClient.SirepGetFile((uint)this.Timeout.TotalMilliseconds, this.remoteFileName, localFileName, overwrite);
			}
			catch (COMException ex)
			{
				this.RemoteDevice.ExceptionHandler(ex);
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000035D8 File Offset: 0x000017D8
		public void Get(string localFileName)
		{
			this.Get(localFileName, false);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003600 File Offset: 0x00001800
		public void Delete()
		{
			this.RemoteDevice.EnsureConnection();
			if (this.RemoteDevice.Protocol != RemoteDevice.TransportProtocol.Ssh)
			{
				string reply = null;
				CallbackHandler outputCallback = new CallbackHandler(delegate(uint flags, string data)
				{
					reply += data;
				});
				string text = "/c del \"" + this.remoteFileName + "\"";
				uint num = 0U;
				try
				{
					num = this.RemoteDevice.SirepClient.LaunchWithOutput((uint)this.Timeout.TotalMilliseconds, "\\windows\\system32\\cmd.exe", text, Path.GetDirectoryName("\\windows\\system32\\cmd.exe"), 0U, outputCallback);
				}
				catch (COMException ex)
				{
					this.RemoteDevice.ExceptionHandler(ex, string.Format(CultureInfo.CurrentCulture, "Remote command '{0} {1}' execution failed.", new object[]
					{
						"\\windows\\system32\\cmd.exe",
						text
					}));
				}
				if (num != 0U)
				{
					int hrforWin32Error = Helper.GetHRForWin32Error(num);
					Exception exceptionForHR = Marshal.GetExceptionForHR(hrforWin32Error);
					uint num2 = num;
					if (num2 == 1U)
					{
						throw new ArgumentException(exceptionForHR.Message, exceptionForHR);
					}
					throw new OperationFailedException(string.Format(CultureInfo.CurrentCulture, "Remote command '{0} {1}' execution failed with {2}.", new object[]
					{
						"\\windows\\system32\\cmd.exe",
						text,
						exceptionForHR
					}));
				}
			}
			else
			{
				try
				{
					this.RemoteDevice.SirepClient.SirepRemoveFile(this.remoteFileName);
				}
				catch (COMException ex2)
				{
					this.RemoteDevice.ExceptionHandler(ex2);
				}
				finally
				{
					this.remoteFileInfo = null;
				}
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000378C File Offset: 0x0000198C
		internal RemoteFileInfo Info()
		{
			this.RemoteDevice.EnsureConnection();
			if (this.remoteFileInfo == null && this.RemoteDevice.IsFileInfoSupported)
			{
				try
				{
					this.remoteFileInfo = this.RemoteDevice.SirepClient.SirepGetFileInfo(this.remoteFileName);
				}
				catch (COMException ex)
				{
					this.remoteFileInfo = null;
					this.RemoteDevice.ExceptionHandler(ex);
				}
			}
			return this.remoteFileInfo;
		}

		// Token: 0x04000097 RID: 151
		private const string DefaultCmdPath = "\\windows\\system32\\cmd.exe";

		// Token: 0x04000098 RID: 152
		private RemoteDevice remoteDevice;

		// Token: 0x04000099 RID: 153
		private string remoteFileName;

		// Token: 0x0400009A RID: 154
		private RemoteFileInfo remoteFileInfo;
	}
}
