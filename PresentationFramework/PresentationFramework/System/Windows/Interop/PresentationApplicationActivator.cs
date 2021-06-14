using System;
using System.Runtime.Hosting;
using System.Runtime.Remoting;
using System.Security;
using System.Security.Permissions;
using MS.Internal.Utility;
using MS.Utility;

namespace System.Windows.Interop
{
	// Token: 0x020005B7 RID: 1463
	internal class PresentationApplicationActivator : ApplicationActivator
	{
		// Token: 0x06006158 RID: 24920 RVA: 0x001B5548 File Offset: 0x001B3748
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public override ObjectHandle CreateInstance(ActivationContext actCtx)
		{
			if (EventTrace.IsEnabled(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Verbose))
			{
				EventTrace.EventProvider.TraceEvent(EventTrace.Event.WpfHost_ApplicationActivatorCreateInstanceStart, EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Verbose, (PresentationAppDomainManager.ActivationUri != null) ? PresentationAppDomainManager.ActivationUri.ToString() : string.Empty);
			}
			ObjectHandle result;
			if (PresentationAppDomainManager.ActivationUri != null)
			{
				result = base.CreateInstance(actCtx, new string[]
				{
					BindUriHelper.UriToString(PresentationAppDomainManager.ActivationUri),
					PresentationAppDomainManager.IsDebug.ToString(),
					(PresentationAppDomainManager.DebugSecurityZoneURL == null) ? string.Empty : PresentationAppDomainManager.DebugSecurityZoneURL.ToString()
				});
			}
			else
			{
				result = base.CreateInstance(actCtx);
			}
			bool flag = false;
			try
			{
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
				if (AppDomain.CurrentDomain.ActivationContext != null && AppDomain.CurrentDomain.ActivationContext.Identity.ToString().Equals(actCtx.Identity.ToString()))
				{
					flag = true;
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Verbose, EventTrace.Event.WpfHost_ApplicationActivatorCreateInstanceEnd);
			if (flag)
			{
				return new ObjectHandle(AppDomain.CurrentDomain);
			}
			return result;
		}
	}
}
