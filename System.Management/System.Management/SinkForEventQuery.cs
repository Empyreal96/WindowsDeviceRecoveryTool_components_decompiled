using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Management
{
	// Token: 0x02000018 RID: 24
	internal class SinkForEventQuery : IWmiEventSource
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000082 RID: 130 RVA: 0x000047B4 File Offset: 0x000029B4
		// (set) Token: 0x06000083 RID: 131 RVA: 0x000047BC File Offset: 0x000029BC
		public int Status
		{
			get
			{
				return this.status;
			}
			set
			{
				this.status = value;
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000047C8 File Offset: 0x000029C8
		public SinkForEventQuery(ManagementEventWatcher eventWatcher, object context, IWbemServices services)
		{
			this.services = services;
			this.context = context;
			this.eventWatcher = eventWatcher;
			this.status = 0;
			this.isLocal = false;
			if (string.Compare(eventWatcher.Scope.Path.Server, ".", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(eventWatcher.Scope.Path.Server, Environment.MachineName, StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.isLocal = true;
			}
			if (MTAHelper.IsNoContextMTA())
			{
				this.HackToCreateStubInMTA(this);
				return;
			}
			new ThreadDispatch(new ThreadDispatch.ThreadWorkerMethodWithParam(this.HackToCreateStubInMTA))
			{
				Parameter = this
			}.Start();
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004870 File Offset: 0x00002A70
		private void HackToCreateStubInMTA(object param)
		{
			SinkForEventQuery sinkForEventQuery = (SinkForEventQuery)param;
			object obj = null;
			sinkForEventQuery.Status = WmiNetUtilsHelper.GetDemultiplexedStub_f(sinkForEventQuery, sinkForEventQuery.isLocal, out obj);
			sinkForEventQuery.stub = (IWbemObjectSink)obj;
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000086 RID: 134 RVA: 0x000048AB File Offset: 0x00002AAB
		internal IWbemObjectSink Stub
		{
			get
			{
				return this.stub;
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000048B4 File Offset: 0x00002AB4
		public void Indicate(IntPtr pWbemClassObject)
		{
			Marshal.AddRef(pWbemClassObject);
			IWbemClassObjectFreeThreaded wbemObject = new IWbemClassObjectFreeThreaded(pWbemClassObject);
			try
			{
				EventArrivedEventArgs args = new EventArrivedEventArgs(this.context, new ManagementBaseObject(wbemObject));
				this.eventWatcher.FireEventArrived(args);
			}
			catch
			{
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004904 File Offset: 0x00002B04
		public void SetStatus(int flags, int hResult, string message, IntPtr pErrObj)
		{
			try
			{
				this.eventWatcher.FireStopped(new StoppedEventArgs(this.context, hResult));
				if (hResult != -2147217358 && hResult != 262150)
				{
					ThreadPool.QueueUserWorkItem(new WaitCallback(this.Cancel2));
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00004960 File Offset: 0x00002B60
		private void Cancel2(object o)
		{
			try
			{
				this.Cancel();
			}
			catch
			{
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00004988 File Offset: 0x00002B88
		internal void Cancel()
		{
			if (this.stub != null)
			{
				lock (this)
				{
					if (this.stub != null)
					{
						int num = this.services.CancelAsyncCall_(this.stub);
						this.ReleaseStub();
						if (num < 0)
						{
							if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
							{
								ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
							}
							else
							{
								Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
							}
						}
					}
				}
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00004A14 File Offset: 0x00002C14
		internal void ReleaseStub()
		{
			if (this.stub != null)
			{
				lock (this)
				{
					if (this.stub != null)
					{
						try
						{
							Marshal.ReleaseComObject(this.stub);
							this.stub = null;
						}
						catch
						{
						}
					}
				}
			}
		}

		// Token: 0x04000091 RID: 145
		private ManagementEventWatcher eventWatcher;

		// Token: 0x04000092 RID: 146
		private object context;

		// Token: 0x04000093 RID: 147
		private IWbemServices services;

		// Token: 0x04000094 RID: 148
		private IWbemObjectSink stub;

		// Token: 0x04000095 RID: 149
		private int status;

		// Token: 0x04000096 RID: 150
		private bool isLocal;
	}
}
