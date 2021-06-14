using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Threading;

namespace <CrtImplementationDetails>
{
	// Token: 0x0200009F RID: 159
	internal class ModuleUninitializer : Stack
	{
		// Token: 0x0600015E RID: 350 RVA: 0x000173C0 File Offset: 0x000167C0
		internal void AddHandler(EventHandler handler)
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					Monitor.Enter(ModuleUninitializer.@lock);
					flag = true;
				}
				RuntimeHelpers.PrepareDelegate(handler);
				this.Push(handler);
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(ModuleUninitializer.@lock);
				}
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x000176F8 File Offset: 0x00016AF8
		private ModuleUninitializer()
		{
			EventHandler value = new EventHandler(this.SingletonDomainUnload);
			AppDomain.CurrentDomain.DomainUnload += value;
			AppDomain.CurrentDomain.ProcessExit += value;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00017444 File Offset: 0x00016844
		[PrePrepareMethod]
		private void SingletonDomainUnload(object source, EventArgs arguments)
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					Monitor.Enter(ModuleUninitializer.@lock);
					flag = true;
				}
				using (IEnumerator enumerator = this.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						((EventHandler)enumerator.Current)(source, arguments);
					}
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(ModuleUninitializer.@lock);
				}
			}
		}

		// Token: 0x04000136 RID: 310
		private static object @lock = new object();

		// Token: 0x04000137 RID: 311
		internal static ModuleUninitializer _ModuleUninitializer = new ModuleUninitializer();
	}
}
