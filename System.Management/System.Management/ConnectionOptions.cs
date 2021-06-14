using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Management
{
	/// <summary>Specifies all settings required to make a WMI connection.</summary>
	// Token: 0x02000032 RID: 50
	public class ConnectionOptions : ManagementOptions
	{
		/// <summary>Gets or sets the locale to be used for the connection operation.</summary>
		/// <returns>Returns a <see cref="T:System.String" /> value used for the locale in a connection to WMI.</returns>
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00008673 File Offset: 0x00006873
		// (set) Token: 0x06000178 RID: 376 RVA: 0x00008689 File Offset: 0x00006889
		public string Locale
		{
			get
			{
				if (this.locale == null)
				{
					return string.Empty;
				}
				return this.locale;
			}
			set
			{
				if (this.locale != value)
				{
					this.locale = value;
					base.FireIdentifierChanged();
				}
			}
		}

		/// <summary>Gets or sets the user name to be used for the connection operation.</summary>
		/// <returns>Returns a <see cref="T:System.String" /> value used as the user name in a connection to WMI.</returns>
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000179 RID: 377 RVA: 0x000086A6 File Offset: 0x000068A6
		// (set) Token: 0x0600017A RID: 378 RVA: 0x000086AE File Offset: 0x000068AE
		public string Username
		{
			get
			{
				return this.username;
			}
			set
			{
				if (this.username != value)
				{
					this.username = value;
					base.FireIdentifierChanged();
				}
			}
		}

		/// <summary>Sets the password for the specified user.</summary>
		/// <returns>Returns a <see cref="T:System.String" /> value used for the password in a connection to WMI.</returns>
		// Token: 0x17000041 RID: 65
		// (set) Token: 0x0600017B RID: 379 RVA: 0x000086CC File Offset: 0x000068CC
		public string Password
		{
			set
			{
				if (value == null)
				{
					if (this.securePassword != null)
					{
						this.securePassword.Dispose();
						this.securePassword = null;
						base.FireIdentifierChanged();
					}
					return;
				}
				if (this.securePassword == null)
				{
					this.securePassword = new SecureString();
					for (int i = 0; i < value.Length; i++)
					{
						this.securePassword.AppendChar(value[i]);
					}
					return;
				}
				SecureString secureString = new SecureString();
				for (int j = 0; j < value.Length; j++)
				{
					secureString.AppendChar(value[j]);
				}
				this.securePassword.Clear();
				this.securePassword = secureString.Copy();
				base.FireIdentifierChanged();
				secureString.Dispose();
			}
		}

		/// <summary>Sets the password for the specified user.</summary>
		/// <returns>Returns a SecureString value used for the password in a connection to WMI.</returns>
		// Token: 0x17000042 RID: 66
		// (set) Token: 0x0600017C RID: 380 RVA: 0x0000877C File Offset: 0x0000697C
		public SecureString SecurePassword
		{
			set
			{
				if (value == null)
				{
					if (this.securePassword != null)
					{
						this.securePassword.Dispose();
						this.securePassword = null;
						base.FireIdentifierChanged();
					}
					return;
				}
				if (this.securePassword == null)
				{
					this.securePassword = value.Copy();
					return;
				}
				this.securePassword.Clear();
				this.securePassword = value.Copy();
				base.FireIdentifierChanged();
			}
		}

		/// <summary>Gets or sets the authority to be used to authenticate the specified user.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> that defines the authority used to authenticate the specified user.</returns>
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600017D RID: 381 RVA: 0x000087DF File Offset: 0x000069DF
		// (set) Token: 0x0600017E RID: 382 RVA: 0x000087F5 File Offset: 0x000069F5
		public string Authority
		{
			get
			{
				if (this.authority == null)
				{
					return string.Empty;
				}
				return this.authority;
			}
			set
			{
				if (this.authority != value)
				{
					this.authority = value;
					base.FireIdentifierChanged();
				}
			}
		}

		/// <summary>Gets or sets the COM impersonation level to be used for operations in this connection.</summary>
		/// <returns>Returns an <see cref="T:System.Management.ImpersonationLevel" /> enumeration value indicating the impersonation level used to connect to WMI.</returns>
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00008812 File Offset: 0x00006A12
		// (set) Token: 0x06000180 RID: 384 RVA: 0x0000881A File Offset: 0x00006A1A
		public ImpersonationLevel Impersonation
		{
			get
			{
				return this.impersonation;
			}
			set
			{
				if (this.impersonation != value)
				{
					this.impersonation = value;
					base.FireIdentifierChanged();
				}
			}
		}

		/// <summary>Gets or sets the COM authentication level to be used for operations in this connection.</summary>
		/// <returns>Returns an <see cref="T:System.Management.AuthenticationLevel" /> enumeration value indicating the COM authentication level used for a connection to the local or a remote computer. </returns>
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00008832 File Offset: 0x00006A32
		// (set) Token: 0x06000182 RID: 386 RVA: 0x0000883A File Offset: 0x00006A3A
		public AuthenticationLevel Authentication
		{
			get
			{
				return this.authentication;
			}
			set
			{
				if (this.authentication != value)
				{
					this.authentication = value;
					base.FireIdentifierChanged();
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether user privileges need to be enabled for the connection operation. This property should only be used when the operation performed requires a certain user privilege to be enabled (for example, a machine restart).          </summary>
		/// <returns>Returns a <see cref="T:System.Boolean" /> value indicating whether user privileges need to be enabled for the connection operation.</returns>
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00008852 File Offset: 0x00006A52
		// (set) Token: 0x06000184 RID: 388 RVA: 0x0000885A File Offset: 0x00006A5A
		public bool EnablePrivileges
		{
			get
			{
				return this.enablePrivileges;
			}
			set
			{
				if (this.enablePrivileges != value)
				{
					this.enablePrivileges = value;
					base.FireIdentifierChanged();
				}
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ConnectionOptions" /> class for the connection operation, using default values. This is the default constructor.          </summary>
		// Token: 0x06000185 RID: 389 RVA: 0x00008874 File Offset: 0x00006A74
		public ConnectionOptions() : this(null, null, null, null, ImpersonationLevel.Impersonate, AuthenticationLevel.Unchanged, false, null, ManagementOptions.InfiniteTimeout)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ConnectionOptions" /> class to be used for a WMI connection, using the specified values.</summary>
		/// <param name="locale">The locale to be used for the connection.</param>
		/// <param name="username">The user name to be used for the connection. If null, the credentials of the currently logged-on user are used.</param>
		/// <param name="password">The password for the given user name. If the user name is also null, the credentials used will be those of the currently logged-on user.</param>
		/// <param name="authority">The authority to be used to authenticate the specified user. </param>
		/// <param name="impersonation">The COM impersonation level to be used for the connection. </param>
		/// <param name="authentication">The COM authentication level to be used for the connection.  </param>
		/// <param name="enablePrivileges">
		///       <see langword="true" /> to enable special user privileges; otherwise, <see langword="false" />. This parameter should only be used when performing an operation that requires special Windows NT user privileges.</param>
		/// <param name="context">A provider-specific, named value pairs object to be passed through to the provider. </param>
		/// <param name="timeout">Reserved for future use. </param>
		// Token: 0x06000186 RID: 390 RVA: 0x00008894 File Offset: 0x00006A94
		public ConnectionOptions(string locale, string username, string password, string authority, ImpersonationLevel impersonation, AuthenticationLevel authentication, bool enablePrivileges, ManagementNamedValueCollection context, TimeSpan timeout) : base(context, timeout)
		{
			if (locale != null)
			{
				this.locale = locale;
			}
			this.username = username;
			this.enablePrivileges = enablePrivileges;
			if (password != null)
			{
				this.securePassword = new SecureString();
				for (int i = 0; i < password.Length; i++)
				{
					this.securePassword.AppendChar(password[i]);
				}
			}
			if (authority != null)
			{
				this.authority = authority;
			}
			if (impersonation != ImpersonationLevel.Default)
			{
				this.impersonation = impersonation;
			}
			if (authentication != AuthenticationLevel.Default)
			{
				this.authentication = authentication;
			}
		}

		/// <summary>Creates a new ConnectionOption.</summary>
		/// <param name="locale">The locale to be used for the connection.</param>
		/// <param name="username">The user name to be used for the connection. If null, the credentials of the currently logged-on user are used.</param>
		/// <param name="password">The password for the given user name. If the user name is also null, the credentials used will be those of the currently logged-on user.</param>
		/// <param name="authority">The authority to be used to authenticate the specified user.</param>
		/// <param name="impersonation">The COM impersonation level to be used for the connection.</param>
		/// <param name="authentication">The COM authentication level to be used for the connection.  </param>
		/// <param name="enablePrivileges">true to enable special user privileges; otherwise, false. This parameter should only be used when performing an operation that requires special Windows NT user privileges.</param>
		/// <param name="context">A provider-specific, named value pairs object to be passed through to the provider.</param>
		/// <param name="timeout">Reserved for future use.</param>
		// Token: 0x06000187 RID: 391 RVA: 0x0000891C File Offset: 0x00006B1C
		public ConnectionOptions(string locale, string username, SecureString password, string authority, ImpersonationLevel impersonation, AuthenticationLevel authentication, bool enablePrivileges, ManagementNamedValueCollection context, TimeSpan timeout) : base(context, timeout)
		{
			if (locale != null)
			{
				this.locale = locale;
			}
			this.username = username;
			this.enablePrivileges = enablePrivileges;
			if (password != null)
			{
				this.securePassword = password.Copy();
			}
			if (authority != null)
			{
				this.authority = authority;
			}
			if (impersonation != ImpersonationLevel.Default)
			{
				this.impersonation = impersonation;
			}
			if (authentication != AuthenticationLevel.Default)
			{
				this.authentication = authentication;
			}
		}

		/// <summary>Returns a copy of the object.</summary>
		/// <returns>The cloned object.</returns>
		// Token: 0x06000188 RID: 392 RVA: 0x00008980 File Offset: 0x00006B80
		public override object Clone()
		{
			ManagementNamedValueCollection context = null;
			if (base.Context != null)
			{
				context = base.Context.Clone();
			}
			return new ConnectionOptions(this.locale, this.username, this.GetSecurePassword(), this.authority, this.impersonation, this.authentication, this.enablePrivileges, context, base.Timeout);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000089DC File Offset: 0x00006BDC
		internal IntPtr GetPassword()
		{
			if (this.securePassword != null)
			{
				try
				{
					return Marshal.SecureStringToBSTR(this.securePassword);
				}
				catch (OutOfMemoryException)
				{
					return IntPtr.Zero;
				}
			}
			return IntPtr.Zero;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00008A20 File Offset: 0x00006C20
		internal SecureString GetSecurePassword()
		{
			if (this.securePassword != null)
			{
				return this.securePassword.Copy();
			}
			return null;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00008A37 File Offset: 0x00006C37
		internal ConnectionOptions(ManagementNamedValueCollection context, TimeSpan timeout, int flags) : base(context, timeout, flags)
		{
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00008A42 File Offset: 0x00006C42
		internal ConnectionOptions(ManagementNamedValueCollection context) : base(context, ManagementOptions.InfiniteTimeout)
		{
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00008A50 File Offset: 0x00006C50
		internal static ConnectionOptions _Clone(ConnectionOptions options)
		{
			return ConnectionOptions._Clone(options, null);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00008A5C File Offset: 0x00006C5C
		internal static ConnectionOptions _Clone(ConnectionOptions options, IdentifierChangedEventHandler handler)
		{
			ConnectionOptions connectionOptions;
			if (options != null)
			{
				connectionOptions = new ConnectionOptions(options.Context, options.Timeout, options.Flags);
				connectionOptions.locale = options.locale;
				connectionOptions.username = options.username;
				connectionOptions.enablePrivileges = options.enablePrivileges;
				if (options.securePassword != null)
				{
					connectionOptions.securePassword = options.securePassword.Copy();
				}
				else
				{
					connectionOptions.securePassword = null;
				}
				if (options.authority != null)
				{
					connectionOptions.authority = options.authority;
				}
				if (options.impersonation != ImpersonationLevel.Default)
				{
					connectionOptions.impersonation = options.impersonation;
				}
				if (options.authentication != AuthenticationLevel.Default)
				{
					connectionOptions.authentication = options.authentication;
				}
			}
			else
			{
				connectionOptions = new ConnectionOptions();
			}
			if (handler != null)
			{
				connectionOptions.IdentifierChanged += handler;
			}
			else if (options != null)
			{
				connectionOptions.IdentifierChanged += options.HandleIdentifierChange;
			}
			return connectionOptions;
		}

		// Token: 0x0400013C RID: 316
		internal const string DEFAULTLOCALE = null;

		// Token: 0x0400013D RID: 317
		internal const string DEFAULTAUTHORITY = null;

		// Token: 0x0400013E RID: 318
		internal const ImpersonationLevel DEFAULTIMPERSONATION = ImpersonationLevel.Impersonate;

		// Token: 0x0400013F RID: 319
		internal const AuthenticationLevel DEFAULTAUTHENTICATION = AuthenticationLevel.Unchanged;

		// Token: 0x04000140 RID: 320
		internal const bool DEFAULTENABLEPRIVILEGES = false;

		// Token: 0x04000141 RID: 321
		private string locale;

		// Token: 0x04000142 RID: 322
		private string username;

		// Token: 0x04000143 RID: 323
		private SecureString securePassword;

		// Token: 0x04000144 RID: 324
		private string authority;

		// Token: 0x04000145 RID: 325
		private ImpersonationLevel impersonation;

		// Token: 0x04000146 RID: 326
		private AuthenticationLevel authentication;

		// Token: 0x04000147 RID: 327
		private bool enablePrivileges;
	}
}
