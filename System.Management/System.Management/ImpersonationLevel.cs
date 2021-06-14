using System;

namespace System.Management
{
	/// <summary>Describes the impersonation level to be used to connect to WMI.          </summary>
	// Token: 0x02000029 RID: 41
	public enum ImpersonationLevel
	{
		/// <summary>Default impersonation.</summary>
		// Token: 0x0400012B RID: 299
		Default,
		/// <summary>Anonymous COM impersonation level that hides the identity of the caller. Calls to WMI may fail with this impersonation level.</summary>
		// Token: 0x0400012C RID: 300
		Anonymous,
		/// <summary>Identify-level COM impersonation level that allows objects to query the credentials of the caller. Calls to WMI may fail with this impersonation level.</summary>
		// Token: 0x0400012D RID: 301
		Identify,
		/// <summary>Impersonate-level COM impersonation level that allows objects to use the credentials of the caller. This is the recommended impersonation level for WMI calls.</summary>
		// Token: 0x0400012E RID: 302
		Impersonate,
		/// <summary>Delegate-level COM impersonation level that allows objects to permit other objects to use the credentials of the caller. This level, which will work with WMI calls but may constitute an unnecessary security risk, is supported only under Windows 2000.</summary>
		// Token: 0x0400012F RID: 303
		Delegate
	}
}
