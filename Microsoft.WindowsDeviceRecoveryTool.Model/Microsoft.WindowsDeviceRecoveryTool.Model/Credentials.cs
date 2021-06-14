using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using Microsoft.WindowsDeviceRecoveryTool.Common;

namespace Microsoft.WindowsDeviceRecoveryTool.Model
{
	// Token: 0x02000020 RID: 32
	public class Credentials : NotificationObject
	{
		// Token: 0x060000ED RID: 237 RVA: 0x000038E4 File Offset: 0x00001AE4
		public Credentials()
		{
			this.InitializeEntropy();
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000EE RID: 238 RVA: 0x000038F8 File Offset: 0x00001AF8
		// (set) Token: 0x060000EF RID: 239 RVA: 0x00003910 File Offset: 0x00001B10
		public string UserName
		{
			get
			{
				return this.userName;
			}
			set
			{
				base.SetValue<string>(() => this.UserName, ref this.userName, value);
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00003960 File Offset: 0x00001B60
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00003978 File Offset: 0x00001B78
		public bool AccountBlocked
		{
			get
			{
				return this.accountBlocked;
			}
			set
			{
				base.SetValue<bool>(() => this.AccountBlocked, ref this.accountBlocked, value);
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x000039C8 File Offset: 0x00001BC8
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x000039E0 File Offset: 0x00001BE0
		public string Password
		{
			get
			{
				return this.password;
			}
			set
			{
				base.SetValue<string>(() => this.Password, ref this.password, value);
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00003A30 File Offset: 0x00001C30
		public string EncryptString(string input)
		{
			byte[] bytes = Encoding.Unicode.GetBytes(input);
			byte[] inArray = ProtectedData.Protect(bytes, this.entropy, DataProtectionScope.CurrentUser);
			return Convert.ToBase64String(inArray);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00003A64 File Offset: 0x00001C64
		public string DecryptString(string input)
		{
			string result;
			try
			{
				if (!string.IsNullOrEmpty(input))
				{
					byte[] bytes = ProtectedData.Unprotect(Convert.FromBase64String(input), this.entropy, DataProtectionScope.CurrentUser);
					result = Encoding.Unicode.GetString(bytes);
				}
				else
				{
					result = string.Empty;
				}
			}
			catch
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00003AC4 File Offset: 0x00001CC4
		public string ToInsecureString(SecureString input)
		{
			IntPtr intPtr = Marshal.SecureStringToBSTR(input);
			string result;
			try
			{
				result = Marshal.PtrToStringBSTR(intPtr);
			}
			finally
			{
				Marshal.ZeroFreeBSTR(intPtr);
			}
			return result;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00003B04 File Offset: 0x00001D04
		private void InitializeEntropy()
		{
			this.entropy = Encoding.Unicode.GetBytes("Windows Phone Recovery Tool, salt and papper entropy");
		}

		// Token: 0x040000AA RID: 170
		private string userName;

		// Token: 0x040000AB RID: 171
		private string password;

		// Token: 0x040000AC RID: 172
		private bool accountBlocked;

		// Token: 0x040000AD RID: 173
		private byte[] entropy;
	}
}
