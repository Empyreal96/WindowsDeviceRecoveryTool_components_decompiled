using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000044 RID: 68
	internal class SecurityHandler
	{
		// Token: 0x06000281 RID: 641 RVA: 0x0000D96A File Offset: 0x0000BB6A
		internal SecurityHandler(ManagementScope theScope)
		{
			this.scope = theScope;
			if (this.scope != null && this.scope.Options.EnablePrivileges)
			{
				WmiNetUtilsHelper.SetSecurity_f(ref this.needToReset, ref this.handle);
			}
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000D9AA File Offset: 0x0000BBAA
		internal void Reset()
		{
			if (this.needToReset)
			{
				this.needToReset = false;
				if (this.scope != null)
				{
					WmiNetUtilsHelper.ResetSecurity_f(this.handle);
				}
			}
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000D9D4 File Offset: 0x0000BBD4
		internal void Secure(IWbemServices services)
		{
			if (this.scope != null)
			{
				IntPtr password = this.scope.Options.GetPassword();
				int num = WmiNetUtilsHelper.BlessIWbemServices_f(services, this.scope.Options.Username, password, this.scope.Options.Authority, (int)this.scope.Options.Impersonation, (int)this.scope.Options.Authentication);
				Marshal.ZeroFreeBSTR(password);
				if (num < 0)
				{
					Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000DA64 File Offset: 0x0000BC64
		internal void SecureIUnknown(object unknown)
		{
			if (this.scope != null)
			{
				IntPtr password = this.scope.Options.GetPassword();
				int num = WmiNetUtilsHelper.BlessIWbemServicesObject_f(unknown, this.scope.Options.Username, password, this.scope.Options.Authority, (int)this.scope.Options.Impersonation, (int)this.scope.Options.Authentication);
				Marshal.ZeroFreeBSTR(password);
				if (num < 0)
				{
					Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
		}

		// Token: 0x040001C1 RID: 449
		private bool needToReset;

		// Token: 0x040001C2 RID: 450
		private IntPtr handle;

		// Token: 0x040001C3 RID: 451
		private ManagementScope scope;
	}
}
