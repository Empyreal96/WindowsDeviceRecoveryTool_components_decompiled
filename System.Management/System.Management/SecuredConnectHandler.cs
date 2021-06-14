using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000042 RID: 66
	internal class SecuredConnectHandler
	{
		// Token: 0x06000267 RID: 615 RVA: 0x0000D1FE File Offset: 0x0000B3FE
		internal SecuredConnectHandler(ManagementScope theScope)
		{
			this.scope = theScope;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000D210 File Offset: 0x0000B410
		internal int ConnectNSecureIWbemServices(string path, ref IWbemServices pServices)
		{
			int result = -2147217407;
			if (this.scope != null)
			{
				bool flag = false;
				IntPtr zero = IntPtr.Zero;
				try
				{
					if (this.scope.Options.EnablePrivileges && !CompatSwitches.AllowIManagementObjectQI)
					{
						WmiNetUtilsHelper.SetSecurity_f(ref flag, ref zero);
					}
					IntPtr password = this.scope.Options.GetPassword();
					result = WmiNetUtilsHelper.ConnectServerWmi_f(path, this.scope.Options.Username, password, this.scope.Options.Locale, this.scope.Options.Flags, this.scope.Options.Authority, this.scope.Options.GetContext(), out pServices, (int)this.scope.Options.Impersonation, (int)this.scope.Options.Authentication);
					Marshal.ZeroFreeBSTR(password);
				}
				finally
				{
					if (flag)
					{
						flag = false;
						WmiNetUtilsHelper.ResetSecurity_f(zero);
					}
				}
			}
			return result;
		}

		// Token: 0x040001BE RID: 446
		private ManagementScope scope;
	}
}
