using System;
using System.Deployment.Application;
using System.Runtime.CompilerServices;
using System.Windows.Interop;
using MS.Internal;
using MS.Internal.AppModel;

namespace System.Windows
{
	/// <summary>Contains the arguments for the <see cref="E:System.Windows.Application.Startup" /> event.</summary>
	// Token: 0x020000F9 RID: 249
	public class StartupEventArgs : EventArgs
	{
		// Token: 0x060008BB RID: 2235 RVA: 0x0001C3CD File Offset: 0x0001A5CD
		internal StartupEventArgs()
		{
			this._performDefaultAction = true;
		}

		/// <summary>Gets command line arguments that were passed to the application from either the command prompt or the desktop.</summary>
		/// <returns>A string array that contains the command line arguments that were passed to the application from either the command prompt or the desktop. If no command line arguments were passed, the string array as zero items.</returns>
		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060008BC RID: 2236 RVA: 0x0001C3DC File Offset: 0x0001A5DC
		public string[] Args
		{
			get
			{
				if (this._args == null)
				{
					this._args = this.GetCmdLineArgs();
				}
				return this._args;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060008BD RID: 2237 RVA: 0x0001C3F8 File Offset: 0x0001A5F8
		// (set) Token: 0x060008BE RID: 2238 RVA: 0x0001C400 File Offset: 0x0001A600
		internal bool PerformDefaultAction
		{
			get
			{
				return this._performDefaultAction;
			}
			set
			{
				this._performDefaultAction = value;
			}
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x0001C40C File Offset: 0x0001A60C
		private string[] GetCmdLineArgs()
		{
			string[] array;
			if (!BrowserInteropHelper.IsBrowserHosted && (Application.Current.MimeType != MimeType.Application || !this.IsOnNetworkShareForDeployedApps()))
			{
				string[] commandLineArgs = Environment.GetCommandLineArgs();
				Invariant.Assert(commandLineArgs.Length >= 1);
				int num = commandLineArgs.Length - 1;
				num = ((num >= 0) ? num : 0);
				array = new string[num];
				for (int i = 1; i < commandLineArgs.Length; i++)
				{
					array[i - 1] = commandLineArgs[i];
				}
			}
			else
			{
				array = new string[0];
			}
			return array;
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x0001C481 File Offset: 0x0001A681
		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool IsOnNetworkShareForDeployedApps()
		{
			return ApplicationDeployment.IsNetworkDeployed;
		}

		// Token: 0x040007BB RID: 1979
		private string[] _args;

		// Token: 0x040007BC RID: 1980
		private bool _performDefaultAction;
	}
}
