using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000041 RID: 65
	internal class SecuredIEnumWbemClassObjectHandler
	{
		// Token: 0x06000261 RID: 609 RVA: 0x0000D0D2 File Offset: 0x0000B2D2
		internal SecuredIEnumWbemClassObjectHandler(ManagementScope theScope, IEnumWbemClassObject pEnumWbemClassObject)
		{
			this.scope = theScope;
			this.pEnumWbemClassObjectsecurityHelper = pEnumWbemClassObject;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000D0E8 File Offset: 0x0000B2E8
		internal int Reset_()
		{
			return this.pEnumWbemClassObjectsecurityHelper.Reset_();
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000D108 File Offset: 0x0000B308
		internal int Next_(int lTimeout, uint uCount, IWbemClassObject_DoNotMarshal[] ppOutParams, ref uint puReturned)
		{
			return this.pEnumWbemClassObjectsecurityHelper.Next_(lTimeout, uCount, ppOutParams, out puReturned);
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000D130 File Offset: 0x0000B330
		internal int NextAsync_(uint uCount, IWbemObjectSink pSink)
		{
			return this.pEnumWbemClassObjectsecurityHelper.NextAsync_(uCount, pSink);
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000D154 File Offset: 0x0000B354
		internal int Clone_(ref IEnumWbemClassObject ppEnum)
		{
			int result = -2147217407;
			if (this.scope != null)
			{
				IntPtr password = this.scope.Options.GetPassword();
				result = WmiNetUtilsHelper.CloneEnumWbemClassObject_f(out ppEnum, (int)this.scope.Options.Authentication, (int)this.scope.Options.Impersonation, this.pEnumWbemClassObjectsecurityHelper, this.scope.Options.Username, password, this.scope.Options.Authority);
				Marshal.ZeroFreeBSTR(password);
			}
			return result;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000D1DC File Offset: 0x0000B3DC
		internal int Skip_(int lTimeout, uint nCount)
		{
			return this.pEnumWbemClassObjectsecurityHelper.Skip_(lTimeout, nCount);
		}

		// Token: 0x040001BC RID: 444
		private IEnumWbemClassObject pEnumWbemClassObjectsecurityHelper;

		// Token: 0x040001BD RID: 445
		private ManagementScope scope;
	}
}
