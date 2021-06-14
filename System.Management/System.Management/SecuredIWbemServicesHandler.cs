using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000043 RID: 67
	internal class SecuredIWbemServicesHandler
	{
		// Token: 0x06000269 RID: 617 RVA: 0x0000D318 File Offset: 0x0000B518
		internal SecuredIWbemServicesHandler(ManagementScope theScope, IWbemServices pWbemServiecs)
		{
			this.scope = theScope;
			this.pWbemServiecsSecurityHelper = pWbemServiecs;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000D330 File Offset: 0x0000B530
		internal int OpenNamespace_(string strNamespace, int lFlags, ref IWbemServices ppWorkingNamespace, IntPtr ppCallResult)
		{
			return -2147217396;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000D344 File Offset: 0x0000B544
		internal int CancelAsyncCall_(IWbemObjectSink pSink)
		{
			return this.pWbemServiecsSecurityHelper.CancelAsyncCall_(pSink);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000D368 File Offset: 0x0000B568
		internal int QueryObjectSink_(int lFlags, ref IWbemObjectSink ppResponseHandler)
		{
			return this.pWbemServiecsSecurityHelper.QueryObjectSink_(lFlags, out ppResponseHandler);
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000D38C File Offset: 0x0000B58C
		internal int GetObject_(string strObjectPath, int lFlags, IWbemContext pCtx, ref IWbemClassObjectFreeThreaded ppObject, IntPtr ppCallResult)
		{
			int result = -2147217407;
			if (ppCallResult != IntPtr.Zero)
			{
				result = this.pWbemServiecsSecurityHelper.GetObject_(strObjectPath, lFlags, pCtx, out ppObject, ppCallResult);
			}
			return result;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000D3C8 File Offset: 0x0000B5C8
		internal int GetObjectAsync_(string strObjectPath, int lFlags, IWbemContext pCtx, IWbemObjectSink pResponseHandler)
		{
			return this.pWbemServiecsSecurityHelper.GetObjectAsync_(strObjectPath, lFlags, pCtx, pResponseHandler);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000D3F0 File Offset: 0x0000B5F0
		internal int PutClass_(IWbemClassObjectFreeThreaded pObject, int lFlags, IWbemContext pCtx, IntPtr ppCallResult)
		{
			int result = -2147217407;
			if (this.scope != null)
			{
				IntPtr password = this.scope.Options.GetPassword();
				result = WmiNetUtilsHelper.PutClassWmi_f(pObject, lFlags, pCtx, ppCallResult, (int)this.scope.Options.Authentication, (int)this.scope.Options.Impersonation, this.pWbemServiecsSecurityHelper, this.scope.Options.Username, password, this.scope.Options.Authority);
				Marshal.ZeroFreeBSTR(password);
			}
			return result;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000D480 File Offset: 0x0000B680
		internal int PutClassAsync_(IWbemClassObjectFreeThreaded pObject, int lFlags, IWbemContext pCtx, IWbemObjectSink pResponseHandler)
		{
			return this.pWbemServiecsSecurityHelper.PutClassAsync_(pObject, lFlags, pCtx, pResponseHandler);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000D4AC File Offset: 0x0000B6AC
		internal int DeleteClass_(string strClass, int lFlags, IWbemContext pCtx, IntPtr ppCallResult)
		{
			int result = -2147217407;
			if (ppCallResult != IntPtr.Zero)
			{
				result = this.pWbemServiecsSecurityHelper.DeleteClass_(strClass, lFlags, pCtx, ppCallResult);
			}
			return result;
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000D4E4 File Offset: 0x0000B6E4
		internal int DeleteClassAsync_(string strClass, int lFlags, IWbemContext pCtx, IWbemObjectSink pResponseHandler)
		{
			return this.pWbemServiecsSecurityHelper.DeleteClassAsync_(strClass, lFlags, pCtx, pResponseHandler);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000D50C File Offset: 0x0000B70C
		internal int CreateClassEnum_(string strSuperClass, int lFlags, IWbemContext pCtx, ref IEnumWbemClassObject ppEnum)
		{
			int result = -2147217407;
			if (this.scope != null)
			{
				IntPtr password = this.scope.Options.GetPassword();
				result = WmiNetUtilsHelper.CreateClassEnumWmi_f(strSuperClass, lFlags, pCtx, out ppEnum, (int)this.scope.Options.Authentication, (int)this.scope.Options.Impersonation, this.pWbemServiecsSecurityHelper, this.scope.Options.Username, password, this.scope.Options.Authority);
				Marshal.ZeroFreeBSTR(password);
			}
			return result;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000D598 File Offset: 0x0000B798
		internal int CreateClassEnumAsync_(string strSuperClass, int lFlags, IWbemContext pCtx, IWbemObjectSink pResponseHandler)
		{
			return this.pWbemServiecsSecurityHelper.CreateClassEnumAsync_(strSuperClass, lFlags, pCtx, pResponseHandler);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000D5C0 File Offset: 0x0000B7C0
		internal int PutInstance_(IWbemClassObjectFreeThreaded pInst, int lFlags, IWbemContext pCtx, IntPtr ppCallResult)
		{
			int result = -2147217407;
			if (this.scope != null)
			{
				IntPtr password = this.scope.Options.GetPassword();
				result = WmiNetUtilsHelper.PutInstanceWmi_f(pInst, lFlags, pCtx, ppCallResult, (int)this.scope.Options.Authentication, (int)this.scope.Options.Impersonation, this.pWbemServiecsSecurityHelper, this.scope.Options.Username, password, this.scope.Options.Authority);
				Marshal.ZeroFreeBSTR(password);
			}
			return result;
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000D650 File Offset: 0x0000B850
		internal int PutInstanceAsync_(IWbemClassObjectFreeThreaded pInst, int lFlags, IWbemContext pCtx, IWbemObjectSink pResponseHandler)
		{
			return this.pWbemServiecsSecurityHelper.PutInstanceAsync_(pInst, lFlags, pCtx, pResponseHandler);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000D67C File Offset: 0x0000B87C
		internal int DeleteInstance_(string strObjectPath, int lFlags, IWbemContext pCtx, IntPtr ppCallResult)
		{
			int result = -2147217407;
			if (ppCallResult != IntPtr.Zero)
			{
				result = this.pWbemServiecsSecurityHelper.DeleteInstance_(strObjectPath, lFlags, pCtx, ppCallResult);
			}
			return result;
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000D6B4 File Offset: 0x0000B8B4
		internal int DeleteInstanceAsync_(string strObjectPath, int lFlags, IWbemContext pCtx, IWbemObjectSink pResponseHandler)
		{
			return this.pWbemServiecsSecurityHelper.DeleteInstanceAsync_(strObjectPath, lFlags, pCtx, pResponseHandler);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000D6DC File Offset: 0x0000B8DC
		internal int CreateInstanceEnum_(string strFilter, int lFlags, IWbemContext pCtx, ref IEnumWbemClassObject ppEnum)
		{
			int result = -2147217407;
			if (this.scope != null)
			{
				IntPtr password = this.scope.Options.GetPassword();
				result = WmiNetUtilsHelper.CreateInstanceEnumWmi_f(strFilter, lFlags, pCtx, out ppEnum, (int)this.scope.Options.Authentication, (int)this.scope.Options.Impersonation, this.pWbemServiecsSecurityHelper, this.scope.Options.Username, password, this.scope.Options.Authority);
				Marshal.ZeroFreeBSTR(password);
			}
			return result;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000D768 File Offset: 0x0000B968
		internal int CreateInstanceEnumAsync_(string strFilter, int lFlags, IWbemContext pCtx, IWbemObjectSink pResponseHandler)
		{
			return this.pWbemServiecsSecurityHelper.CreateInstanceEnumAsync_(strFilter, lFlags, pCtx, pResponseHandler);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000D790 File Offset: 0x0000B990
		internal int ExecQuery_(string strQueryLanguage, string strQuery, int lFlags, IWbemContext pCtx, ref IEnumWbemClassObject ppEnum)
		{
			int result = -2147217407;
			if (this.scope != null)
			{
				IntPtr password = this.scope.Options.GetPassword();
				result = WmiNetUtilsHelper.ExecQueryWmi_f(strQueryLanguage, strQuery, lFlags, pCtx, out ppEnum, (int)this.scope.Options.Authentication, (int)this.scope.Options.Impersonation, this.pWbemServiecsSecurityHelper, this.scope.Options.Username, password, this.scope.Options.Authority);
				Marshal.ZeroFreeBSTR(password);
			}
			return result;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000D81C File Offset: 0x0000BA1C
		internal int ExecQueryAsync_(string strQueryLanguage, string strQuery, int lFlags, IWbemContext pCtx, IWbemObjectSink pResponseHandler)
		{
			return this.pWbemServiecsSecurityHelper.ExecQueryAsync_(strQueryLanguage, strQuery, lFlags, pCtx, pResponseHandler);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000D844 File Offset: 0x0000BA44
		internal int ExecNotificationQuery_(string strQueryLanguage, string strQuery, int lFlags, IWbemContext pCtx, ref IEnumWbemClassObject ppEnum)
		{
			int result = -2147217407;
			if (this.scope != null)
			{
				IntPtr password = this.scope.Options.GetPassword();
				result = WmiNetUtilsHelper.ExecNotificationQueryWmi_f(strQueryLanguage, strQuery, lFlags, pCtx, out ppEnum, (int)this.scope.Options.Authentication, (int)this.scope.Options.Impersonation, this.pWbemServiecsSecurityHelper, this.scope.Options.Username, password, this.scope.Options.Authority);
				Marshal.ZeroFreeBSTR(password);
			}
			return result;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000D8D0 File Offset: 0x0000BAD0
		internal int ExecNotificationQueryAsync_(string strQueryLanguage, string strQuery, int lFlags, IWbemContext pCtx, IWbemObjectSink pResponseHandler)
		{
			return this.pWbemServiecsSecurityHelper.ExecNotificationQueryAsync_(strQueryLanguage, strQuery, lFlags, pCtx, pResponseHandler);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000D8F8 File Offset: 0x0000BAF8
		internal int ExecMethod_(string strObjectPath, string strMethodName, int lFlags, IWbemContext pCtx, IWbemClassObjectFreeThreaded pInParams, ref IWbemClassObjectFreeThreaded ppOutParams, IntPtr ppCallResult)
		{
			int result = -2147217407;
			if (ppCallResult != IntPtr.Zero)
			{
				result = this.pWbemServiecsSecurityHelper.ExecMethod_(strObjectPath, strMethodName, lFlags, pCtx, pInParams, out ppOutParams, ppCallResult);
			}
			return result;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000D93C File Offset: 0x0000BB3C
		internal int ExecMethodAsync_(string strObjectPath, string strMethodName, int lFlags, IWbemContext pCtx, IWbemClassObjectFreeThreaded pInParams, IWbemObjectSink pResponseHandler)
		{
			return this.pWbemServiecsSecurityHelper.ExecMethodAsync_(strObjectPath, strMethodName, lFlags, pCtx, pInParams, pResponseHandler);
		}

		// Token: 0x040001BF RID: 447
		private IWbemServices pWbemServiecsSecurityHelper;

		// Token: 0x040001C0 RID: 448
		private ManagementScope scope;
	}
}
