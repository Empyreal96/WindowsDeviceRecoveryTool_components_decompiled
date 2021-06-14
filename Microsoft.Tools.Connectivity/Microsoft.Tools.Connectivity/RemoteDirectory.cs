using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.Tools.Connectivity
{
	// Token: 0x0200000C RID: 12
	[CLSCompliant(true)]
	public class RemoteDirectory
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00003804 File Offset: 0x00001A04
		// (set) Token: 0x0600007E RID: 126 RVA: 0x0000380C File Offset: 0x00001A0C
		public TimeSpan Timeout { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00003818 File Offset: 0x00001A18
		public bool Exists
		{
			get
			{
				bool result = false;
				try
				{
					result = this.CheckExists();
				}
				catch
				{
				}
				return result;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00003844 File Offset: 0x00001A44
		internal RemoteDevice RemoteDevice
		{
			get
			{
				return this.remoteDevice;
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000384C File Offset: 0x00001A4C
		internal RemoteDirectory(RemoteDevice remoteDevice, string directoryNamePath)
		{
			this.remoteDevice = remoteDevice;
			char[] trimChars = new char[]
			{
				'\\'
			};
			this.remoteDirectoryName = directoryNamePath.TrimEnd(trimChars) + "\\";
			this.Timeout = this.remoteDevice.Timeout;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000389C File Offset: 0x00001A9C
		public IEnumerable<string> GetDirectories()
		{
			return this.Enumerate(false, true, "*", false);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000038C8 File Offset: 0x00001AC8
		public void Create()
		{
			this.RemoteDevice.EnsureConnection();
			if (this.RemoteDevice.Protocol != RemoteDevice.TransportProtocol.Ssh)
			{
				string reply = null;
				CallbackHandler outputCallback = new CallbackHandler(delegate(uint flags, string data)
				{
					reply += data;
				});
				string text = "/c mkdir \"" + this.remoteDirectoryName + "\"";
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
					this.RemoteDevice.SirepClient.SirepCreateDirectory(this.remoteDirectoryName);
				}
				catch (COMException ex2)
				{
					this.RemoteDevice.ExceptionHandler(ex2, string.Format(CultureInfo.CurrentCulture, "Remote directory '{0}' creation failed.", new object[]
					{
						this.remoteDirectoryName
					}));
				}
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003A7C File Offset: 0x00001C7C
		public void Delete(bool force)
		{
			this.RemoteDevice.EnsureConnection();
			if (this.RemoteDevice.Protocol != RemoteDevice.TransportProtocol.Ssh)
			{
				string reply = null;
				CallbackHandler outputCallback = new CallbackHandler(delegate(uint flags, string data)
				{
					reply += data;
				});
				string text = "/c rmdir ";
				if (force)
				{
					text += "/s /q ";
				}
				text = text + "\"" + this.remoteDirectoryName + "\"";
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
					if (num2 == 2U)
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
					this.RemoteDevice.SirepClient.SirepRemoveDirectory(this.remoteDirectoryName);
				}
				catch (COMException ex2)
				{
					this.RemoteDevice.ExceptionHandler(ex2, string.Format(CultureInfo.CurrentCulture, "Remote directory '{0}' remove failed.", new object[]
					{
						this.remoteDirectoryName
					}));
				}
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003C2C File Offset: 0x00001E2C
		public IEnumerable<string> GetDirectories(string searchPattern)
		{
			return this.Enumerate(false, true, searchPattern, false);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003C38 File Offset: 0x00001E38
		public IEnumerable<string> GetDirectories(string searchPattern, bool recursive)
		{
			return this.Enumerate(false, true, searchPattern, recursive);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003C44 File Offset: 0x00001E44
		public IEnumerable<string> GetFiles()
		{
			return this.Enumerate(true, false, "*", false);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003C54 File Offset: 0x00001E54
		public IEnumerable<string> GetFiles(string searchPattern, bool recursive)
		{
			return this.Enumerate(true, false, searchPattern, recursive);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003C60 File Offset: 0x00001E60
		public IEnumerable<string> GetFileSystemEntries()
		{
			return this.Enumerate(true, true, "*", false);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003C70 File Offset: 0x00001E70
		public IEnumerable<string> GetFileSystemEntries(string searchPattern)
		{
			return this.Enumerate(true, true, searchPattern, false);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003C7C File Offset: 0x00001E7C
		public IEnumerable<string> GetFileSystemEntries(string searchPattern, bool recursive)
		{
			return this.Enumerate(true, true, searchPattern, recursive);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003CA0 File Offset: 0x00001EA0
		private IEnumerable<string> Enumerate(bool includeFiles, bool includeDirectories, string searchPattern = "*", bool recursive = false)
		{
			if (string.IsNullOrWhiteSpace(searchPattern))
			{
				throw new ArgumentNullException("searchPattern");
			}
			this.RemoteDevice.EnsureConnection();
			StringBuilder reply = new StringBuilder();
			CallbackHandler outputCallback = new CallbackHandler(delegate(uint flags, string data)
			{
				reply.Append(data);
			});
			List<string> list = new List<string>();
			if (this.RemoteDevice.Protocol != RemoteDevice.TransportProtocol.Ssh)
			{
				string text = Path.Combine(this.remoteDirectoryName, searchPattern);
				string text2 = "dir ";
				if (recursive)
				{
					text2 += "/s";
				}
				if (text == "\\")
				{
					text2 = text2 + " " + text;
				}
				else
				{
					text2 = text2 + " \"" + text + "\"";
				}
				try
				{
					this.RemoteDevice.SirepClient.LaunchWithOutput((uint)this.Timeout.TotalMilliseconds, "<WPCONDEV>", text2, null, 0U, outputCallback);
				}
				catch (COMException ex)
				{
					this.RemoteDevice.ExceptionHandler(ex);
				}
				string[] array = reply.ToString().Split(new string[]
				{
					"\r\r\n"
				}, StringSplitOptions.RemoveEmptyEntries);
				string[] array2 = array;
				int i = 0;
				while (i < array2.Length)
				{
					string input = array2[i];
					MatchCollection matchCollection = RemoteDirectory.RegexEntry.Matches(input);
					if (matchCollection.Count != 0)
					{
						goto IL_1B7;
					}
					matchCollection = RemoteDirectory.RegexError.Matches(input);
					if (matchCollection.Count > 0)
					{
						try
						{
							int errorCode = (int)uint.Parse(matchCollection[0].Groups["HR"].Value);
							Marshal.ThrowExceptionForHR(errorCode);
							goto IL_23B;
						}
						catch (COMException ex2)
						{
							this.RemoteDevice.ExceptionHandler(ex2);
							goto IL_23B;
						}
						catch (Exception ex3)
						{
							if (ex3 is FormatException || ex3 is OverflowException)
							{
								throw new OperationFailedException(null, ex3);
							}
							throw;
						}
						goto IL_1B7;
					}
					IL_23B:
					i++;
					continue;
					IL_1B7:
					string value = matchCollection[0].Groups["Category"].Value;
					if (value == "D" && includeDirectories)
					{
						list.Add(matchCollection[0].Groups["Path"].Value);
					}
					if (value == "F" && includeFiles)
					{
						list.Add(matchCollection[0].Groups["Path"].Value);
						goto IL_23B;
					}
					goto IL_23B;
				}
			}
			else
			{
				try
				{
					this.RemoteDevice.SirepClient.SirepDirectoryEnum(this.remoteDirectoryName, outputCallback);
				}
				catch (COMException ex4)
				{
					this.RemoteDevice.ExceptionHandler(ex4);
				}
				string[] array3 = reply.ToString().Split(new string[]
				{
					"\n"
				}, StringSplitOptions.RemoveEmptyEntries);
				if (searchPattern != "*")
				{
					searchPattern = searchPattern.Replace(".", "\\.");
					searchPattern = searchPattern.Replace("?", ".");
					searchPattern = searchPattern.Replace("*", ".*");
				}
				foreach (string input2 in array3)
				{
					MatchCollection matchCollection2 = RemoteDirectory.RegexEnum.Matches(input2);
					if (matchCollection2.Count != 0)
					{
						string value2 = matchCollection2[0].Groups["Path"].Value;
						string value3 = matchCollection2[0].Groups["Category"].Value;
						if (searchPattern != "*")
						{
							Regex regex = new Regex(searchPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
							if (regex.Matches(value2).Count == 0)
							{
								goto IL_3C0;
							}
						}
						if (value3 == "D" && includeDirectories)
						{
							list.Add(this.remoteDirectoryName + value2);
						}
						if (value3 == "F" && includeFiles)
						{
							list.Add(this.remoteDirectoryName + value2);
						}
					}
					IL_3C0:;
				}
			}
			return list;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000040D0 File Offset: 0x000022D0
		private bool CheckExists()
		{
			this.RemoteDevice.EnsureConnection();
			bool result = false;
			if (this.RemoteDevice.Protocol != RemoteDevice.TransportProtocol.Ssh)
			{
				string reply = null;
				CallbackHandler outputCallback = new CallbackHandler(delegate(uint flags, string data)
				{
					reply += data;
				});
				string text = "/c  dir \"" + this.remoteDirectoryName + "\"";
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
					if (num2 != 1U)
					{
						throw new OperationFailedException(string.Format(CultureInfo.CurrentCulture, "Remote command '{0} {1}' execution failed with {2}.", new object[]
						{
							"\\windows\\system32\\cmd.exe",
							text,
							exceptionForHR
						}));
					}
					result = false;
				}
				else
				{
					result = true;
				}
			}
			else
			{
				IEnumerable<string> enumerable = this.Enumerate(true, true, "*", false);
				using (IEnumerator<string> enumerator = enumerable.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						string text2 = enumerator.Current;
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x0400009C RID: 156
		private const string DefaultCmdPath = "\\windows\\system32\\cmd.exe";

		// Token: 0x0400009D RID: 157
		private static readonly Regex RegexInfo = new Regex("^I:(?<Path>.*)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

		// Token: 0x0400009E RID: 158
		private static readonly Regex RegexEntry = new Regex("^(?<Category>(D|F)):(?<LastModifiedTime>\\d{2}/\\d{2}/\\d{4} \\d{2}:\\d{2}) (?<Size>-?\\d+):(?<Attributes>-?\\d+):(?<Path>.*)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

		// Token: 0x0400009F RID: 159
		private static readonly Regex RegexSummary = new Regex("^(?<Category>(S|T)):(?<FileCount>\\d+):(?<Size>\\d+):(?<DirCount>\\d+)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

		// Token: 0x040000A0 RID: 160
		private static readonly Regex RegexError = new Regex("^E:(?<HR>\\d+):(?<Path>.*)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

		// Token: 0x040000A1 RID: 161
		private static readonly Regex RegexEnum = new Regex("^(?<Path>.*):(?<Category>(D|F))", RegexOptions.Compiled | RegexOptions.CultureInvariant);

		// Token: 0x040000A2 RID: 162
		private RemoteDevice remoteDevice;

		// Token: 0x040000A3 RID: 163
		private string remoteDirectoryName;
	}
}
