using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Management
{
	// Token: 0x0200004E RID: 78
	internal class WmiEventSink : IWmiEventSource
	{
		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060002DB RID: 731 RVA: 0x000101D8 File Offset: 0x0000E3D8
		// (remove) Token: 0x060002DC RID: 732 RVA: 0x00010210 File Offset: 0x0000E410
		internal event InternalObjectPutEventHandler InternalObjectPut;

		// Token: 0x060002DD RID: 733 RVA: 0x00010248 File Offset: 0x0000E448
		internal static WmiEventSink GetWmiEventSink(ManagementOperationObserver watcher, object context, ManagementScope scope, string path, string className)
		{
			if (MTAHelper.IsNoContextMTA())
			{
				return new WmiEventSink(watcher, context, scope, path, className);
			}
			WmiEventSink.watcherParameter = watcher;
			WmiEventSink.contextParameter = context;
			WmiEventSink.scopeParameter = scope;
			WmiEventSink.pathParameter = path;
			WmiEventSink.classNameParameter = className;
			ThreadDispatch threadDispatch = new ThreadDispatch(new ThreadDispatch.ThreadWorkerMethod(WmiEventSink.HackToCreateWmiEventSink));
			threadDispatch.Start();
			return WmiEventSink.wmiEventSinkNew;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x000102A4 File Offset: 0x0000E4A4
		private static void HackToCreateWmiEventSink()
		{
			WmiEventSink.wmiEventSinkNew = new WmiEventSink(WmiEventSink.watcherParameter, WmiEventSink.contextParameter, WmiEventSink.scopeParameter, WmiEventSink.pathParameter, WmiEventSink.classNameParameter);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x000102CC File Offset: 0x0000E4CC
		protected WmiEventSink(ManagementOperationObserver watcher, object context, ManagementScope scope, string path, string className)
		{
			try
			{
				this.context = context;
				this.watcher = watcher;
				this.className = className;
				this.isLocal = false;
				if (path != null)
				{
					this.path = new ManagementPath(path);
					if (string.Compare(this.path.Server, ".", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(this.path.Server, Environment.MachineName, StringComparison.OrdinalIgnoreCase) == 0)
					{
						this.isLocal = true;
					}
				}
				if (scope != null)
				{
					this.scope = scope.Clone();
					if (path == null && (string.Compare(this.scope.Path.Server, ".", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(this.scope.Path.Server, Environment.MachineName, StringComparison.OrdinalIgnoreCase) == 0))
					{
						this.isLocal = true;
					}
				}
				WmiNetUtilsHelper.GetDemultiplexedStub_f(this, this.isLocal, out this.stub);
				this.hash = Interlocked.Increment(ref WmiEventSink.s_hash);
			}
			catch
			{
			}
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x000103D8 File Offset: 0x0000E5D8
		public override int GetHashCode()
		{
			return this.hash;
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x000103E0 File Offset: 0x0000E5E0
		public IWbemObjectSink Stub
		{
			get
			{
				IWbemObjectSink result;
				try
				{
					result = ((this.stub != null) ? ((IWbemObjectSink)this.stub) : null);
				}
				catch
				{
					result = null;
				}
				return result;
			}
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0001041C File Offset: 0x0000E61C
		public virtual void Indicate(IntPtr pIWbemClassObject)
		{
			Marshal.AddRef(pIWbemClassObject);
			IWbemClassObjectFreeThreaded wbemObject = new IWbemClassObjectFreeThreaded(pIWbemClassObject);
			try
			{
				ObjectReadyEventArgs args = new ObjectReadyEventArgs(this.context, ManagementBaseObject.GetBaseObject(wbemObject, this.scope));
				this.watcher.FireObjectReady(args);
			}
			catch
			{
			}
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00010470 File Offset: 0x0000E670
		public void SetStatus(int flags, int hResult, string message, IntPtr pErrorObj)
		{
			IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded = null;
			if (pErrorObj != IntPtr.Zero)
			{
				Marshal.AddRef(pErrorObj);
				wbemClassObjectFreeThreaded = new IWbemClassObjectFreeThreaded(pErrorObj);
			}
			try
			{
				if (flags == 0)
				{
					if (this.path != null)
					{
						if (this.className == null)
						{
							this.path.RelativePath = message;
						}
						else
						{
							this.path.RelativePath = this.className;
						}
						if (this.InternalObjectPut != null)
						{
							try
							{
								InternalObjectPutEventArgs e = new InternalObjectPutEventArgs(this.path);
								this.InternalObjectPut(this, e);
							}
							catch
							{
							}
						}
						ObjectPutEventArgs args = new ObjectPutEventArgs(this.context, this.path);
						this.watcher.FireObjectPut(args);
					}
					CompletedEventArgs args2;
					if (wbemClassObjectFreeThreaded != null)
					{
						args2 = new CompletedEventArgs(this.context, hResult, new ManagementBaseObject(wbemClassObjectFreeThreaded));
					}
					else
					{
						args2 = new CompletedEventArgs(this.context, hResult, null);
					}
					this.watcher.FireCompleted(args2);
					this.watcher.RemoveSink(this);
				}
				else if ((flags & 2) != 0)
				{
					ProgressEventArgs args3 = new ProgressEventArgs(this.context, (int)((uint)(hResult & -65536) >> 16), hResult & 65535, message);
					this.watcher.FireProgress(args3);
				}
			}
			catch
			{
			}
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x000105AC File Offset: 0x0000E7AC
		internal void Cancel()
		{
			try
			{
				this.scope.GetIWbemServices().CancelAsyncCall_((IWbemObjectSink)this.stub);
			}
			catch
			{
			}
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x000105EC File Offset: 0x0000E7EC
		internal void ReleaseStub()
		{
			try
			{
				if (this.stub != null)
				{
					Marshal.ReleaseComObject(this.stub);
					this.stub = null;
				}
			}
			catch
			{
			}
		}

		// Token: 0x040001DE RID: 478
		private static int s_hash;

		// Token: 0x040001DF RID: 479
		private int hash;

		// Token: 0x040001E0 RID: 480
		private ManagementOperationObserver watcher;

		// Token: 0x040001E1 RID: 481
		private object context;

		// Token: 0x040001E2 RID: 482
		private ManagementScope scope;

		// Token: 0x040001E3 RID: 483
		private object stub;

		// Token: 0x040001E5 RID: 485
		private ManagementPath path;

		// Token: 0x040001E6 RID: 486
		private string className;

		// Token: 0x040001E7 RID: 487
		private bool isLocal;

		// Token: 0x040001E8 RID: 488
		private static ManagementOperationObserver watcherParameter;

		// Token: 0x040001E9 RID: 489
		private static object contextParameter;

		// Token: 0x040001EA RID: 490
		private static ManagementScope scopeParameter;

		// Token: 0x040001EB RID: 491
		private static string pathParameter;

		// Token: 0x040001EC RID: 492
		private static string classNameParameter;

		// Token: 0x040001ED RID: 493
		private static WmiEventSink wmiEventSinkNew;
	}
}
