using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace System.Management
{
	// Token: 0x020000A6 RID: 166
	internal class MTAHelper
	{
		// Token: 0x0600045A RID: 1114 RVA: 0x0002184C File Offset: 0x0001FA4C
		private static void InitWorkerThread()
		{
			Thread thread = new Thread(new ThreadStart(MTAHelper.WorkerThread));
			thread.SetApartmentState(ApartmentState.MTA);
			thread.IsBackground = true;
			thread.Start();
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00021880 File Offset: 0x0001FA80
		public static object CreateInMTA(Type type)
		{
			if (MTAHelper.IsNoContextMTA())
			{
				return Activator.CreateInstance(type);
			}
			MTAHelper.MTARequest mtarequest = new MTAHelper.MTARequest(type);
			object obj = MTAHelper.critSec;
			lock (obj)
			{
				if (!MTAHelper.workerThreadInitialized)
				{
					MTAHelper.InitWorkerThread();
					MTAHelper.workerThreadInitialized = true;
				}
				int index = MTAHelper.reqList.Add(mtarequest);
				if (!MTAHelper.evtGo.Set())
				{
					MTAHelper.reqList.RemoveAt(index);
					throw new ManagementException(RC.GetString("WORKER_THREAD_WAKEUP_FAILED"));
				}
			}
			mtarequest.evtDone.WaitOne();
			if (mtarequest.exception != null)
			{
				throw mtarequest.exception;
			}
			return mtarequest.createdObject;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00021934 File Offset: 0x0001FB34
		private static void WorkerThread()
		{
			for (;;)
			{
				MTAHelper.evtGo.WaitOne();
				for (;;)
				{
					MTAHelper.MTARequest mtarequest = null;
					object obj = MTAHelper.critSec;
					lock (obj)
					{
						if (MTAHelper.reqList.Count <= 0)
						{
							break;
						}
						mtarequest = (MTAHelper.MTARequest)MTAHelper.reqList[0];
						MTAHelper.reqList.RemoveAt(0);
					}
					try
					{
						mtarequest.createdObject = Activator.CreateInstance(mtarequest.typeToCreate);
					}
					catch (Exception exception)
					{
						mtarequest.exception = exception;
					}
					finally
					{
						mtarequest.evtDone.Set();
					}
				}
			}
		}

		// Token: 0x0600045D RID: 1117
		[SuppressUnmanagedCodeSecurity]
		[DllImport("ole32.dll")]
		private static extern int CoGetObjectContext([In] ref Guid riid, out IntPtr pUnk);

		// Token: 0x0600045E RID: 1118 RVA: 0x000219E8 File Offset: 0x0001FBE8
		public static bool IsNoContextMTA()
		{
			if (Thread.CurrentThread.GetApartmentState() != ApartmentState.MTA)
			{
				return false;
			}
			if (!MTAHelper.CanCallCoGetObjectContext)
			{
				return true;
			}
			IntPtr zero = IntPtr.Zero;
			IntPtr zero2 = IntPtr.Zero;
			try
			{
				if (MTAHelper.CoGetObjectContext(ref MTAHelper.IID_IComThreadingInfo, out zero) != 0)
				{
					return false;
				}
				WmiNetUtilsHelper.APTTYPE apttype;
				if (WmiNetUtilsHelper.GetCurrentApartmentType_f(3, zero, out apttype) != 0)
				{
					return false;
				}
				if (apttype != WmiNetUtilsHelper.APTTYPE.APTTYPE_MTA)
				{
					return false;
				}
				if (Marshal.QueryInterface(zero, ref MTAHelper.IID_IObjectContext, out zero2) == 0)
				{
					return false;
				}
			}
			finally
			{
				if (zero != IntPtr.Zero)
				{
					Marshal.Release(zero);
				}
				if (zero2 != IntPtr.Zero)
				{
					Marshal.Release(zero2);
				}
			}
			return true;
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00021A98 File Offset: 0x0001FC98
		private static bool IsWindows2000OrHigher()
		{
			OperatingSystem osversion = Environment.OSVersion;
			return osversion.Platform == PlatformID.Win32NT && osversion.Version >= new Version(5, 0);
		}

		// Token: 0x04000487 RID: 1159
		private static ArrayList reqList = new ArrayList(3);

		// Token: 0x04000488 RID: 1160
		private static object critSec = new object();

		// Token: 0x04000489 RID: 1161
		private static AutoResetEvent evtGo = new AutoResetEvent(false);

		// Token: 0x0400048A RID: 1162
		private static bool workerThreadInitialized = false;

		// Token: 0x0400048B RID: 1163
		private static Guid IID_IObjectContext = new Guid("51372AE0-CAE7-11CF-BE81-00AA00A2FA25");

		// Token: 0x0400048C RID: 1164
		private static Guid IID_IComThreadingInfo = new Guid("000001ce-0000-0000-C000-000000000046");

		// Token: 0x0400048D RID: 1165
		private static bool CanCallCoGetObjectContext = MTAHelper.IsWindows2000OrHigher();

		// Token: 0x02000102 RID: 258
		private class MTARequest
		{
			// Token: 0x06000660 RID: 1632 RVA: 0x000274C3 File Offset: 0x000256C3
			public MTARequest(Type typeToCreate)
			{
				this.typeToCreate = typeToCreate;
			}

			// Token: 0x04000561 RID: 1377
			public AutoResetEvent evtDone = new AutoResetEvent(false);

			// Token: 0x04000562 RID: 1378
			public Type typeToCreate;

			// Token: 0x04000563 RID: 1379
			public object createdObject;

			// Token: 0x04000564 RID: 1380
			public Exception exception;
		}
	}
}
