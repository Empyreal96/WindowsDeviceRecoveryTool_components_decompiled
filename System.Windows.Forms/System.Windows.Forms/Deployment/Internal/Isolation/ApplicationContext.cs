using System;
using System.Deployment.Internal.Isolation.Manifest;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000064 RID: 100
	internal class ApplicationContext
	{
		// Token: 0x060001E9 RID: 489 RVA: 0x00008455 File Offset: 0x00006655
		internal ApplicationContext(IActContext a)
		{
			if (a == null)
			{
				throw new ArgumentNullException();
			}
			this._appcontext = a;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000846D File Offset: 0x0000666D
		public ApplicationContext(DefinitionAppId appid)
		{
			if (appid == null)
			{
				throw new ArgumentNullException();
			}
			this._appcontext = IsolationInterop.CreateActContext(appid._id);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000848F File Offset: 0x0000668F
		public ApplicationContext(ReferenceAppId appid)
		{
			if (appid == null)
			{
				throw new ArgumentNullException();
			}
			this._appcontext = IsolationInterop.CreateActContext(appid._id);
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001EC RID: 492 RVA: 0x000084B4 File Offset: 0x000066B4
		public DefinitionAppId Identity
		{
			get
			{
				object obj;
				this._appcontext.GetAppId(out obj);
				return new DefinitionAppId(obj as IDefinitionAppId);
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001ED RID: 493 RVA: 0x000084DC File Offset: 0x000066DC
		public string BasePath
		{
			get
			{
				string result;
				this._appcontext.ApplicationBasePath(0U, out result);
				return result;
			}
		}

		// Token: 0x060001EE RID: 494 RVA: 0x000084F8 File Offset: 0x000066F8
		public string ReplaceStrings(string culture, string toreplace)
		{
			string result;
			this._appcontext.ReplaceStringMacros(0U, culture, toreplace, out result);
			return result;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00008518 File Offset: 0x00006718
		internal ICMS GetComponentManifest(DefinitionIdentity component)
		{
			object obj;
			this._appcontext.GetComponentManifest(0U, component._id, ref IsolationInterop.IID_ICMS, out obj);
			return obj as ICMS;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00008544 File Offset: 0x00006744
		internal string GetComponentManifestPath(DefinitionIdentity component)
		{
			object obj;
			this._appcontext.GetComponentManifest(0U, component._id, ref IsolationInterop.IID_IManifestInformation, out obj);
			string result;
			((IManifestInformation)obj).get_FullPath(out result);
			return result;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00008578 File Offset: 0x00006778
		public string GetComponentPath(DefinitionIdentity component)
		{
			string result;
			this._appcontext.GetComponentPayloadPath(0U, component._id, out result);
			return result;
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000859C File Offset: 0x0000679C
		public DefinitionIdentity MatchReference(ReferenceIdentity TheRef)
		{
			object obj;
			this._appcontext.FindReferenceInContext(0U, TheRef._id, out obj);
			return new DefinitionIdentity(obj as IDefinitionIdentity);
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x000085C8 File Offset: 0x000067C8
		public EnumDefinitionIdentity Components
		{
			get
			{
				object obj;
				this._appcontext.EnumComponents(0U, out obj);
				return new EnumDefinitionIdentity(obj as IEnumDefinitionIdentity);
			}
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x000085EE File Offset: 0x000067EE
		public void PrepareForExecution()
		{
			this._appcontext.PrepareForExecution(IntPtr.Zero, IntPtr.Zero);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00008608 File Offset: 0x00006808
		public ApplicationContext.ApplicationStateDisposition SetApplicationState(ApplicationContext.ApplicationState s)
		{
			uint result;
			this._appcontext.SetApplicationRunningState(0U, (uint)s, out result);
			return (ApplicationContext.ApplicationStateDisposition)result;
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x00008628 File Offset: 0x00006828
		public string StateLocation
		{
			get
			{
				string result;
				this._appcontext.GetApplicationStateFilesystemLocation(0U, UIntPtr.Zero, IntPtr.Zero, out result);
				return result;
			}
		}

		// Token: 0x040001AA RID: 426
		private IActContext _appcontext;

		// Token: 0x02000537 RID: 1335
		public enum ApplicationState
		{
			// Token: 0x0400374B RID: 14155
			Undefined,
			// Token: 0x0400374C RID: 14156
			Starting,
			// Token: 0x0400374D RID: 14157
			Running
		}

		// Token: 0x02000538 RID: 1336
		public enum ApplicationStateDisposition
		{
			// Token: 0x0400374F RID: 14159
			Undefined,
			// Token: 0x04003750 RID: 14160
			Starting,
			// Token: 0x04003751 RID: 14161
			Starting_Migrated = 65537,
			// Token: 0x04003752 RID: 14162
			Running = 2,
			// Token: 0x04003753 RID: 14163
			Running_FirstTime = 131074
		}
	}
}
