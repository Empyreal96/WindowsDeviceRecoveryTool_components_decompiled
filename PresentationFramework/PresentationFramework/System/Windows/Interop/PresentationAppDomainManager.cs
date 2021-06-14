using System;
using System.Runtime.Hosting;
using System.Security;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Utility;

namespace System.Windows.Interop
{
	// Token: 0x020005B8 RID: 1464
	internal class PresentationAppDomainManager : AppDomainManager
	{
		// Token: 0x0600615A RID: 24922 RVA: 0x001B567C File Offset: 0x001B387C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		static PresentationAppDomainManager()
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Verbose, EventTrace.Event.WpfHost_AppDomainManagerCctor);
		}

		// Token: 0x0600615B RID: 24923 RVA: 0x001B568E File Offset: 0x001B388E
		[SecurityCritical]
		public PresentationAppDomainManager()
		{
		}

		// Token: 0x1700176C RID: 5996
		// (get) Token: 0x0600615C RID: 24924 RVA: 0x001B5696 File Offset: 0x001B3896
		public override ApplicationActivator ApplicationActivator
		{
			get
			{
				if (this._appActivator == null)
				{
					this._appActivator = new PresentationApplicationActivator();
				}
				return this._appActivator;
			}
		}

		// Token: 0x0600615D RID: 24925 RVA: 0x001B56B1 File Offset: 0x001B38B1
		[SecurityCritical]
		public override void InitializeNewDomain(AppDomainSetup appDomainInfo)
		{
			this._assemblyFilter = new AssemblyFilter();
			AppDomain.CurrentDomain.AssemblyLoad += this._assemblyFilter.FilterCallback;
		}

		// Token: 0x1700176D RID: 5997
		// (get) Token: 0x0600615E RID: 24926 RVA: 0x001B56D9 File Offset: 0x001B38D9
		public override HostSecurityManager HostSecurityManager
		{
			[SecurityCritical]
			get
			{
				if (this._hostsecuritymanager == null)
				{
					this._hostsecuritymanager = new PresentationHostSecurityManager();
				}
				return this._hostsecuritymanager;
			}
		}

		// Token: 0x0600615F RID: 24927 RVA: 0x001B56F4 File Offset: 0x001B38F4
		[SecurityCritical]
		internal ApplicationProxyInternal CreateApplicationProxyInternal()
		{
			return new ApplicationProxyInternal();
		}

		// Token: 0x1700176E RID: 5998
		// (get) Token: 0x06006160 RID: 24928 RVA: 0x001B56FB File Offset: 0x001B38FB
		// (set) Token: 0x06006161 RID: 24929 RVA: 0x001B5702 File Offset: 0x001B3902
		internal static AppDomain NewAppDomain
		{
			get
			{
				return PresentationAppDomainManager._newAppDomain;
			}
			set
			{
				PresentationAppDomainManager._newAppDomain = value;
			}
		}

		// Token: 0x1700176F RID: 5999
		// (get) Token: 0x06006162 RID: 24930 RVA: 0x001B570A File Offset: 0x001B390A
		// (set) Token: 0x06006163 RID: 24931 RVA: 0x001B5711 File Offset: 0x001B3911
		internal static bool SaveAppDomain
		{
			get
			{
				return PresentationAppDomainManager._saveAppDomain;
			}
			set
			{
				PresentationAppDomainManager._saveAppDomain = value;
				PresentationAppDomainManager._newAppDomain = null;
			}
		}

		// Token: 0x17001770 RID: 6000
		// (get) Token: 0x06006164 RID: 24932 RVA: 0x001B571F File Offset: 0x001B391F
		// (set) Token: 0x06006165 RID: 24933 RVA: 0x001B5726 File Offset: 0x001B3926
		internal static Uri ActivationUri
		{
			get
			{
				return PresentationAppDomainManager._activationUri;
			}
			set
			{
				PresentationAppDomainManager._activationUri = value;
			}
		}

		// Token: 0x17001771 RID: 6001
		// (get) Token: 0x06006166 RID: 24934 RVA: 0x001B572E File Offset: 0x001B392E
		// (set) Token: 0x06006167 RID: 24935 RVA: 0x001B5735 File Offset: 0x001B3935
		internal static Uri DebugSecurityZoneURL
		{
			get
			{
				return PresentationAppDomainManager._debugSecurityZoneURL;
			}
			set
			{
				PresentationAppDomainManager._debugSecurityZoneURL = value;
			}
		}

		// Token: 0x17001772 RID: 6002
		// (get) Token: 0x06006168 RID: 24936 RVA: 0x001B573D File Offset: 0x001B393D
		// (set) Token: 0x06006169 RID: 24937 RVA: 0x001B5744 File Offset: 0x001B3944
		internal static bool IsDebug
		{
			get
			{
				return PresentationAppDomainManager._isdebug;
			}
			set
			{
				PresentationAppDomainManager._isdebug = value;
			}
		}

		// Token: 0x0400314D RID: 12621
		private static bool _isdebug;

		// Token: 0x0400314E RID: 12622
		private ApplicationActivator _appActivator;

		// Token: 0x0400314F RID: 12623
		[SecurityCritical]
		private HostSecurityManager _hostsecuritymanager;

		// Token: 0x04003150 RID: 12624
		private static AppDomain _newAppDomain;

		// Token: 0x04003151 RID: 12625
		private static bool _saveAppDomain;

		// Token: 0x04003152 RID: 12626
		private static Uri _activationUri;

		// Token: 0x04003153 RID: 12627
		private static Uri _debugSecurityZoneURL;

		// Token: 0x04003154 RID: 12628
		[SecurityCritical]
		private AssemblyFilter _assemblyFilter;
	}
}
