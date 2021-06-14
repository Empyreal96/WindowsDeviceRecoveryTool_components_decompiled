using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace System.Management
{
	/// <summary>Provides a wrapper for parsing and building paths to WMI objects.          </summary>
	// Token: 0x02000033 RID: 51
	[TypeConverter(typeof(ManagementPathConverter))]
	public class ManagementPath : ICloneable
	{
		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600018F RID: 399 RVA: 0x00008B34 File Offset: 0x00006D34
		// (remove) Token: 0x06000190 RID: 400 RVA: 0x00008B6C File Offset: 0x00006D6C
		internal event IdentifierChangedEventHandler IdentifierChanged;

		// Token: 0x06000191 RID: 401 RVA: 0x00008BA1 File Offset: 0x00006DA1
		private void FireIdentifierChanged()
		{
			if (this.IdentifierChanged != null)
			{
				this.IdentifierChanged(this, null);
			}
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00008BB8 File Offset: 0x00006DB8
		internal static string GetManagementPath(IWbemClassObjectFreeThreaded wbemObject)
		{
			string result = null;
			if (wbemObject != null)
			{
				int num = 0;
				int num2 = 0;
				object obj = null;
				int num3 = wbemObject.Get_("__PATH", 0, ref obj, ref num, ref num2);
				if (num3 < 0 || obj == DBNull.Value)
				{
					num3 = wbemObject.Get_("__RELPATH", 0, ref obj, ref num, ref num2);
					if (num3 < 0)
					{
						if (((long)num3 & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
						{
							ManagementException.ThrowWithExtendedInfo((ManagementStatus)num3);
						}
						else
						{
							Marshal.ThrowExceptionForHR(num3, WmiNetUtilsHelper.GetErrorInfo_f());
						}
					}
				}
				if (DBNull.Value == obj)
				{
					result = null;
				}
				else
				{
					result = (string)obj;
				}
			}
			return result;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00008C50 File Offset: 0x00006E50
		internal static bool IsValidNamespaceSyntax(string nsPath)
		{
			if (nsPath.Length != 0)
			{
				char[] anyOf = new char[]
				{
					'\\',
					'/'
				};
				if (nsPath.IndexOfAny(anyOf) == -1 && string.Compare("root", nsPath, StringComparison.OrdinalIgnoreCase) != 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00008C91 File Offset: 0x00006E91
		internal static ManagementPath _Clone(ManagementPath path)
		{
			return ManagementPath._Clone(path, null);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00008C9C File Offset: 0x00006E9C
		internal static ManagementPath _Clone(ManagementPath path, IdentifierChangedEventHandler handler)
		{
			ManagementPath managementPath = new ManagementPath();
			if (handler != null)
			{
				managementPath.IdentifierChanged = handler;
			}
			if (path != null && path.wmiPath != null)
			{
				managementPath.wmiPath = path.wmiPath;
				managementPath.isWbemPathShared = (path.isWbemPathShared = true);
			}
			return managementPath;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementPath" /> class that is empty. This is the default constructor.          </summary>
		// Token: 0x06000196 RID: 406 RVA: 0x00008CE1 File Offset: 0x00006EE1
		public ManagementPath() : this(null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementPath" /> class for the given path.          </summary>
		/// <param name="path"> The object path. </param>
		// Token: 0x06000197 RID: 407 RVA: 0x00008CEA File Offset: 0x00006EEA
		public ManagementPath(string path)
		{
			if (path != null && 0 < path.Length)
			{
				this.wmiPath = this.CreateWbemPath(path);
			}
		}

		/// <summary>Returns the full object path as the string representation.          </summary>
		/// <returns>A string containing the full object path represented by this object. This value is equivalent to the value of the <see cref="P:System.Management.ManagementPath.Path" /> property.             </returns>
		// Token: 0x06000198 RID: 408 RVA: 0x00008D0B File Offset: 0x00006F0B
		public override string ToString()
		{
			return this.Path;
		}

		/// <summary>Returns a copy of the <see cref="T:System.Management.ManagementPath" />.          </summary>
		/// <returns>The cloned object.             </returns>
		// Token: 0x06000199 RID: 409 RVA: 0x00008D13 File Offset: 0x00006F13
		public ManagementPath Clone()
		{
			return new ManagementPath(this.Path);
		}

		/// <summary>Creates a new object that is a copy of the current instance.  </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		// Token: 0x0600019A RID: 410 RVA: 0x00008D20 File Offset: 0x00006F20
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		/// <summary>Gets or sets the default scope path used when no scope is specified. The default scope is \\.\root\cimv2, and can be changed by setting this property.          </summary>
		/// <returns>Returns a <see cref="T:System.Management.ManagementPath" /> that contains the default scope (namespace) path used when no scope is specified.</returns>
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00008D28 File Offset: 0x00006F28
		// (set) Token: 0x0600019C RID: 412 RVA: 0x00008D2F File Offset: 0x00006F2F
		public static ManagementPath DefaultPath
		{
			get
			{
				return ManagementPath.defaultPath;
			}
			set
			{
				ManagementPath.defaultPath = value;
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00008D38 File Offset: 0x00006F38
		private IWbemPath CreateWbemPath(string path)
		{
			IWbemPath wbemPath = (IWbemPath)MTAHelper.CreateInMTA(typeof(WbemDefPath));
			ManagementPath.SetWbemPath(wbemPath, path);
			return wbemPath;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00008D62 File Offset: 0x00006F62
		private void SetWbemPath(string path)
		{
			if (this.wmiPath == null)
			{
				this.wmiPath = this.CreateWbemPath(path);
				return;
			}
			ManagementPath.SetWbemPath(this.wmiPath, path);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00008D88 File Offset: 0x00006F88
		private static void SetWbemPath(IWbemPath wbemPath, string path)
		{
			if (wbemPath != null)
			{
				uint num = 4U;
				if (string.Compare(path, "root", StringComparison.OrdinalIgnoreCase) == 0)
				{
					num |= 8U;
				}
				int num2 = wbemPath.SetText_(num, path);
				if (num2 < 0)
				{
					if (((long)num2 & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)num2);
						return;
					}
					Marshal.ThrowExceptionForHR(num2, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00008DE1 File Offset: 0x00006FE1
		private string GetWbemPath()
		{
			return ManagementPath.GetWbemPath(this.wmiPath);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00008DF0 File Offset: 0x00006FF0
		private static string GetWbemPath(IWbemPath wbemPath)
		{
			string text = string.Empty;
			if (wbemPath != null)
			{
				int lFlags = 4;
				uint num = 0U;
				int num2 = wbemPath.GetNamespaceCount_(out num);
				if (num2 >= 0)
				{
					if (num == 0U)
					{
						lFlags = 2;
					}
					uint num3 = 0U;
					num2 = wbemPath.GetText_(lFlags, ref num3, null);
					if (num2 >= 0 && 0U < num3)
					{
						text = new string('0', (int)(num3 - 1U));
						num2 = wbemPath.GetText_(lFlags, ref num3, text);
					}
				}
				if (num2 < 0 && num2 != -2147217400)
				{
					if (((long)num2 & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)num2);
					}
					else
					{
						Marshal.ThrowExceptionForHR(num2, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
			}
			return text;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00008E84 File Offset: 0x00007084
		private void ClearKeys(bool setAsSingleton)
		{
			int num = 0;
			try
			{
				if (this.wmiPath != null)
				{
					IWbemPathKeyList wbemPathKeyList = null;
					num = this.wmiPath.GetKeyList_(out wbemPathKeyList);
					if (wbemPathKeyList != null)
					{
						num = wbemPathKeyList.RemoveAllKeys_(0U);
						if (((long)num & (long)((ulong)-2147483648)) == 0L)
						{
							sbyte bSet = setAsSingleton ? -1 : 0;
							num = wbemPathKeyList.MakeSingleton_(bSet);
							this.FireIdentifierChanged();
						}
					}
				}
			}
			catch (COMException e)
			{
				ManagementException.ThrowWithExtendedInfo(e);
			}
			if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
			{
				ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
				return;
			}
			if (((long)num & (long)((ulong)-2147483648)) != 0L)
			{
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x00008F28 File Offset: 0x00007128
		internal bool IsEmpty
		{
			get
			{
				return this.Path.Length == 0;
			}
		}

		/// <summary>Sets the path as a new class path. This means that the path must have a class name but not key values.          </summary>
		// Token: 0x060001A4 RID: 420 RVA: 0x00008F38 File Offset: 0x00007138
		public void SetAsClass()
		{
			if (this.IsClass || this.IsInstance)
			{
				if (this.isWbemPathShared)
				{
					this.wmiPath = this.CreateWbemPath(this.GetWbemPath());
					this.isWbemPathShared = false;
				}
				this.ClearKeys(false);
				return;
			}
			throw new ManagementException(ManagementStatus.InvalidOperation, null, null);
		}

		/// <summary>Sets the path as a new singleton object path. This means that it is a path to an instance but there are no key values.          </summary>
		// Token: 0x060001A5 RID: 421 RVA: 0x00008F8C File Offset: 0x0000718C
		public void SetAsSingleton()
		{
			if (this.IsClass || this.IsInstance)
			{
				if (this.isWbemPathShared)
				{
					this.wmiPath = this.CreateWbemPath(this.GetWbemPath());
					this.isWbemPathShared = false;
				}
				this.ClearKeys(true);
				return;
			}
			throw new ManagementException(ManagementStatus.InvalidOperation, null, null);
		}

		/// <summary>Gets or sets the string representation of the full path in the object.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the full path.</returns>
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00008FDE File Offset: 0x000071DE
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x00008FE8 File Offset: 0x000071E8
		[RefreshProperties(RefreshProperties.All)]
		public string Path
		{
			get
			{
				return this.GetWbemPath();
			}
			set
			{
				try
				{
					if (this.isWbemPathShared)
					{
						this.wmiPath = this.CreateWbemPath(this.GetWbemPath());
						this.isWbemPathShared = false;
					}
					this.SetWbemPath(value);
				}
				catch
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.FireIdentifierChanged();
			}
		}

		/// <summary>Gets or sets the relative path: class name and keys only.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the relative path.</returns>
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00009044 File Offset: 0x00007244
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x000090CC File Offset: 0x000072CC
		[RefreshProperties(RefreshProperties.All)]
		public string RelativePath
		{
			get
			{
				string text = string.Empty;
				if (this.wmiPath != null)
				{
					uint num = 0U;
					int text_ = this.wmiPath.GetText_(2, ref num, null);
					if (text_ >= 0 && 0U < num)
					{
						text = new string('0', (int)(num - 1U));
						text_ = this.wmiPath.GetText_(2, ref num, text);
					}
					if (text_ < 0 && text_ != -2147217400)
					{
						if (((long)text_ & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
						{
							ManagementException.ThrowWithExtendedInfo((ManagementStatus)text_);
						}
						else
						{
							Marshal.ThrowExceptionForHR(text_, WmiNetUtilsHelper.GetErrorInfo_f());
						}
					}
				}
				return text;
			}
			set
			{
				try
				{
					this.SetRelativePath(value);
				}
				catch (COMException)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.FireIdentifierChanged();
			}
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00009104 File Offset: 0x00007304
		internal void SetRelativePath(string relPath)
		{
			this.wmiPath = new ManagementPath(relPath)
			{
				NamespacePath = this.GetNamespacePath(8),
				Server = this.Server
			}.wmiPath;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00009140 File Offset: 0x00007340
		internal void UpdateRelativePath(string relPath)
		{
			if (relPath == null)
			{
				return;
			}
			string wbemPath = string.Empty;
			string namespacePath = this.GetNamespacePath(8);
			if (namespacePath.Length > 0)
			{
				wbemPath = namespacePath + ":" + relPath;
			}
			else
			{
				wbemPath = relPath;
			}
			if (this.isWbemPathShared)
			{
				this.wmiPath = this.CreateWbemPath(this.GetWbemPath());
				this.isWbemPathShared = false;
			}
			this.SetWbemPath(wbemPath);
		}

		/// <summary>Gets or sets the server part of the path.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the server name.</returns>
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001AC RID: 428 RVA: 0x000091A4 File Offset: 0x000073A4
		// (set) Token: 0x060001AD RID: 429 RVA: 0x00009228 File Offset: 0x00007428
		[RefreshProperties(RefreshProperties.All)]
		public string Server
		{
			get
			{
				string text = string.Empty;
				if (this.wmiPath != null)
				{
					uint num = 0U;
					int server_ = this.wmiPath.GetServer_(ref num, null);
					if (server_ >= 0 && 0U < num)
					{
						text = new string('0', (int)(num - 1U));
						server_ = this.wmiPath.GetServer_(ref num, text);
					}
					if (server_ < 0 && server_ != -2147217399)
					{
						if (((long)server_ & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
						{
							ManagementException.ThrowWithExtendedInfo((ManagementStatus)server_);
						}
						else
						{
							Marshal.ThrowExceptionForHR(server_, WmiNetUtilsHelper.GetErrorInfo_f());
						}
					}
				}
				return text;
			}
			set
			{
				string server = this.Server;
				if (string.Compare(server, value, StringComparison.OrdinalIgnoreCase) != 0)
				{
					if (this.wmiPath == null)
					{
						this.wmiPath = (IWbemPath)MTAHelper.CreateInMTA(typeof(WbemDefPath));
					}
					else if (this.isWbemPathShared)
					{
						this.wmiPath = this.CreateWbemPath(this.GetWbemPath());
						this.isWbemPathShared = false;
					}
					int num = this.wmiPath.SetServer_(value);
					if (num < 0)
					{
						if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
						{
							ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
						}
						else
						{
							Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
						}
					}
					this.FireIdentifierChanged();
				}
			}
		}

		// Token: 0x060001AE RID: 430 RVA: 0x000092D0 File Offset: 0x000074D0
		internal string SetNamespacePath(string nsPath, out bool bChange)
		{
			int num = 0;
			bChange = false;
			if (!ManagementPath.IsValidNamespaceSyntax(nsPath))
			{
				ManagementException.ThrowWithExtendedInfo(ManagementStatus.InvalidNamespace);
			}
			IWbemPath wbemPath = this.CreateWbemPath(nsPath);
			if (this.wmiPath == null)
			{
				this.wmiPath = this.CreateWbemPath("");
			}
			else if (this.isWbemPathShared)
			{
				this.wmiPath = this.CreateWbemPath(this.GetWbemPath());
				this.isWbemPathShared = false;
			}
			string namespacePath = ManagementPath.GetNamespacePath(this.wmiPath, 16);
			string namespacePath2 = ManagementPath.GetNamespacePath(wbemPath, 16);
			if (string.Compare(namespacePath, namespacePath2, StringComparison.OrdinalIgnoreCase) != 0)
			{
				this.wmiPath.RemoveAllNamespaces_();
				bChange = true;
				uint num2 = 0U;
				num = wbemPath.GetNamespaceCount_(out num2);
				if (num >= 0)
				{
					for (uint num3 = 0U; num3 < num2; num3 += 1U)
					{
						uint num4 = 0U;
						num = wbemPath.GetNamespaceAt_(num3, ref num4, null);
						if (num < 0)
						{
							break;
						}
						string text = new string('0', (int)(num4 - 1U));
						num = wbemPath.GetNamespaceAt_(num3, ref num4, text);
						if (num < 0)
						{
							break;
						}
						num = this.wmiPath.SetNamespaceAt_(num3, text);
						if (num < 0)
						{
							break;
						}
					}
				}
			}
			if (num >= 0 && nsPath.Length > 1 && ((nsPath[0] == '\\' && nsPath[1] == '\\') || (nsPath[0] == '/' && nsPath[1] == '/')))
			{
				uint num5 = 0U;
				num = wbemPath.GetServer_(ref num5, null);
				if (num >= 0 && num5 > 0U)
				{
					string text2 = new string('0', (int)(num5 - 1U));
					num = wbemPath.GetServer_(ref num5, text2);
					if (num >= 0)
					{
						num5 = 0U;
						num = this.wmiPath.GetServer_(ref num5, null);
						if (num >= 0)
						{
							string text3 = new string('0', (int)(num5 - 1U));
							num = this.wmiPath.GetServer_(ref num5, text3);
							if (num >= 0 && string.Compare(text3, text2, StringComparison.OrdinalIgnoreCase) != 0)
							{
								num = this.wmiPath.SetServer_(text2);
							}
						}
						else if (num == -2147217399)
						{
							num = this.wmiPath.SetServer_(text2);
							if (num >= 0)
							{
								bChange = true;
							}
						}
					}
				}
				else if (num == -2147217399)
				{
					num = 0;
				}
			}
			if (num < 0)
			{
				if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
				}
				else
				{
					Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
			return namespacePath2;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x000094FB File Offset: 0x000076FB
		internal string GetNamespacePath(int flags)
		{
			return ManagementPath.GetNamespacePath(this.wmiPath, flags);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000950C File Offset: 0x0000770C
		internal static string GetNamespacePath(IWbemPath wbemPath, int flags)
		{
			string text = string.Empty;
			if (wbemPath != null)
			{
				uint num = 0U;
				int num2 = wbemPath.GetNamespaceCount_(out num);
				if (num2 >= 0 && num > 0U)
				{
					uint num3 = 0U;
					num2 = wbemPath.GetText_(flags, ref num3, null);
					if (num2 >= 0 && num3 > 0U)
					{
						text = new string('0', (int)(num3 - 1U));
						num2 = wbemPath.GetText_(flags, ref num3, text);
					}
				}
				if (num2 < 0 && num2 != -2147217400)
				{
					if (((long)num2 & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)num2);
					}
					else
					{
						Marshal.ThrowExceptionForHR(num2, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
			}
			return text;
		}

		/// <summary>Gets or sets the namespace part of the path. Note that this does not include the server name, which can be retrieved separately.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the namespace part of the path.</returns>
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x00009598 File Offset: 0x00007798
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x000095A4 File Offset: 0x000077A4
		[RefreshProperties(RefreshProperties.All)]
		public string NamespacePath
		{
			get
			{
				return this.GetNamespacePath(16);
			}
			set
			{
				bool flag = false;
				try
				{
					this.SetNamespacePath(value, out flag);
				}
				catch (COMException)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (flag)
				{
					this.FireIdentifierChanged();
				}
			}
		}

		/// <summary>Gets or sets the class portion of the path.                       </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value that holds the class portion of the path.</returns>
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x000095E4 File Offset: 0x000077E4
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x000095EC File Offset: 0x000077EC
		[RefreshProperties(RefreshProperties.All)]
		public string ClassName
		{
			get
			{
				return this.internalClassName;
			}
			set
			{
				string className = this.ClassName;
				if (string.Compare(className, value, StringComparison.OrdinalIgnoreCase) != 0)
				{
					this.internalClassName = value;
					this.FireIdentifierChanged();
				}
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00009618 File Offset: 0x00007818
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x00009674 File Offset: 0x00007874
		internal string internalClassName
		{
			get
			{
				string text = string.Empty;
				if (this.wmiPath != null)
				{
					uint num = 0U;
					int className_ = this.wmiPath.GetClassName_(ref num, null);
					if (className_ >= 0 && 0U < num)
					{
						text = new string('0', (int)(num - 1U));
						className_ = this.wmiPath.GetClassName_(ref num, text);
						if (className_ < 0)
						{
							text = string.Empty;
						}
					}
				}
				return text;
			}
			set
			{
				int num = 0;
				if (this.wmiPath == null)
				{
					this.wmiPath = (IWbemPath)MTAHelper.CreateInMTA(typeof(WbemDefPath));
				}
				else if (this.isWbemPathShared)
				{
					this.wmiPath = this.CreateWbemPath(this.GetWbemPath());
					this.isWbemPathShared = false;
				}
				try
				{
					num = this.wmiPath.SetClassName_(value);
				}
				catch (COMException)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (num < 0)
				{
					if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
						return;
					}
					Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether this is a class path.          </summary>
		/// <returns>Returns a <see cref="T:System.Boolean" /> value indicating whether this is a class path.</returns>
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00009720 File Offset: 0x00007920
		public bool IsClass
		{
			get
			{
				if (this.wmiPath == null)
				{
					return false;
				}
				ulong num = 0UL;
				int info_ = this.wmiPath.GetInfo_(0U, out num);
				if (info_ < 0)
				{
					if (((long)info_ & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)info_);
					}
					else
					{
						Marshal.ThrowExceptionForHR(info_, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
				return (num & 4UL) > 0UL;
			}
		}

		/// <summary>Gets or sets a value indicating whether this is an instance path.          </summary>
		/// <returns>Returns a <see cref="T:System.Boolean" /> value indicating whether this is an instance path.</returns>
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x00009780 File Offset: 0x00007980
		public bool IsInstance
		{
			get
			{
				if (this.wmiPath == null)
				{
					return false;
				}
				ulong num = 0UL;
				int info_ = this.wmiPath.GetInfo_(0U, out num);
				if (info_ < 0)
				{
					if (((long)info_ & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)info_);
					}
					else
					{
						Marshal.ThrowExceptionForHR(info_, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
				return (num & 8UL) > 0UL;
			}
		}

		/// <summary>Gets or sets a value indicating whether this is a singleton instance path.          </summary>
		/// <returns>Returns a <see cref="T:System.Boolean" /> value indicating whether this is a singleton instance path.</returns>
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x000097E0 File Offset: 0x000079E0
		public bool IsSingleton
		{
			get
			{
				if (this.wmiPath == null)
				{
					return false;
				}
				ulong num = 0UL;
				int info_ = this.wmiPath.GetInfo_(0U, out num);
				if (info_ < 0)
				{
					if (((long)info_ & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)info_);
					}
					else
					{
						Marshal.ThrowExceptionForHR(info_, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
				return (num & 4096UL) > 0UL;
			}
		}

		// Token: 0x04000148 RID: 328
		private static ManagementPath defaultPath = new ManagementPath("//./root/cimv2");

		// Token: 0x04000149 RID: 329
		private bool isWbemPathShared;

		// Token: 0x0400014B RID: 331
		private IWbemPath wmiPath;
	}
}
