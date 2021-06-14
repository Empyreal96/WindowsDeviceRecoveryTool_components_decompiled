using System;
using System.Threading;

namespace System.Management
{
	// Token: 0x020000A7 RID: 167
	internal class ThreadDispatch
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x00021B27 File Offset: 0x0001FD27
		public Exception Exception
		{
			get
			{
				return this.exception;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x00021B2F File Offset: 0x0001FD2F
		// (set) Token: 0x06000464 RID: 1124 RVA: 0x00021B37 File Offset: 0x0001FD37
		public object Parameter
		{
			get
			{
				return this.threadParams;
			}
			set
			{
				this.threadParams = value;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x00021B40 File Offset: 0x0001FD40
		// (set) Token: 0x06000466 RID: 1126 RVA: 0x00021B48 File Offset: 0x0001FD48
		public bool IsBackgroundThread
		{
			get
			{
				return this.backgroundThread;
			}
			set
			{
				this.backgroundThread = value;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x00021B51 File Offset: 0x0001FD51
		public object Result
		{
			get
			{
				return this.threadReturn;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x00021B59 File Offset: 0x0001FD59
		// (set) Token: 0x06000469 RID: 1129 RVA: 0x00021B61 File Offset: 0x0001FD61
		public ApartmentState ApartmentType
		{
			get
			{
				return this.apartmentType;
			}
			set
			{
				this.apartmentType = value;
			}
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00021B6A File Offset: 0x0001FD6A
		public ThreadDispatch(ThreadDispatch.ThreadWorkerMethodWithReturn workerMethod) : this()
		{
			this.InitializeThreadState(null, workerMethod, ApartmentState.MTA, false);
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00021B7C File Offset: 0x0001FD7C
		public ThreadDispatch(ThreadDispatch.ThreadWorkerMethodWithReturnAndParam workerMethod) : this()
		{
			this.InitializeThreadState(null, workerMethod, ApartmentState.MTA, false);
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00021B8E File Offset: 0x0001FD8E
		public ThreadDispatch(ThreadDispatch.ThreadWorkerMethodWithParam workerMethod) : this()
		{
			this.InitializeThreadState(null, workerMethod, ApartmentState.MTA, false);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00021BA0 File Offset: 0x0001FDA0
		public ThreadDispatch(ThreadDispatch.ThreadWorkerMethod workerMethod) : this()
		{
			this.InitializeThreadState(null, workerMethod, ApartmentState.MTA, false);
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00021BB2 File Offset: 0x0001FDB2
		public void Start()
		{
			this.exception = null;
			this.DispatchThread();
			if (this.Exception != null)
			{
				throw this.Exception;
			}
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00021BD0 File Offset: 0x0001FDD0
		private ThreadDispatch()
		{
			this.thread = null;
			this.exception = null;
			this.threadParams = null;
			this.threadWorkerMethodWithReturn = null;
			this.threadWorkerMethodWithReturnAndParam = null;
			this.threadWorkerMethod = null;
			this.threadWorkerMethodWithParam = null;
			this.threadReturn = null;
			this.backgroundThread = false;
			this.apartmentType = ApartmentState.MTA;
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00021C29 File Offset: 0x0001FE29
		private void InitializeThreadState(object threadParams, ThreadDispatch.ThreadWorkerMethodWithReturn workerMethod, ApartmentState aptState, bool background)
		{
			this.threadParams = threadParams;
			this.threadWorkerMethodWithReturn = workerMethod;
			this.thread = new Thread(new ThreadStart(this.ThreadEntryPointMethodWithReturn));
			this.thread.SetApartmentState(aptState);
			this.backgroundThread = background;
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00021C64 File Offset: 0x0001FE64
		private void InitializeThreadState(object threadParams, ThreadDispatch.ThreadWorkerMethodWithReturnAndParam workerMethod, ApartmentState aptState, bool background)
		{
			this.threadParams = threadParams;
			this.threadWorkerMethodWithReturnAndParam = workerMethod;
			this.thread = new Thread(new ThreadStart(this.ThreadEntryPointMethodWithReturnAndParam));
			this.thread.SetApartmentState(aptState);
			this.backgroundThread = background;
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00021C9F File Offset: 0x0001FE9F
		private void InitializeThreadState(object threadParams, ThreadDispatch.ThreadWorkerMethod workerMethod, ApartmentState aptState, bool background)
		{
			this.threadParams = threadParams;
			this.threadWorkerMethod = workerMethod;
			this.thread = new Thread(new ThreadStart(this.ThreadEntryPoint));
			this.thread.SetApartmentState(aptState);
			this.backgroundThread = background;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00021CDA File Offset: 0x0001FEDA
		private void InitializeThreadState(object threadParams, ThreadDispatch.ThreadWorkerMethodWithParam workerMethod, ApartmentState aptState, bool background)
		{
			this.threadParams = threadParams;
			this.threadWorkerMethodWithParam = workerMethod;
			this.thread = new Thread(new ThreadStart(this.ThreadEntryPointMethodWithParam));
			this.thread.SetApartmentState(aptState);
			this.backgroundThread = background;
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00021D15 File Offset: 0x0001FF15
		private void DispatchThread()
		{
			this.thread.Start();
			this.thread.Join();
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00021D30 File Offset: 0x0001FF30
		private void ThreadEntryPoint()
		{
			try
			{
				this.threadWorkerMethod();
			}
			catch (Exception ex)
			{
				this.exception = ex;
			}
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00021D64 File Offset: 0x0001FF64
		private void ThreadEntryPointMethodWithParam()
		{
			try
			{
				this.threadWorkerMethodWithParam(this.threadParams);
			}
			catch (Exception ex)
			{
				this.exception = ex;
			}
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00021DA0 File Offset: 0x0001FFA0
		private void ThreadEntryPointMethodWithReturn()
		{
			try
			{
				this.threadReturn = this.threadWorkerMethodWithReturn();
			}
			catch (Exception ex)
			{
				this.exception = ex;
			}
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00021DDC File Offset: 0x0001FFDC
		private void ThreadEntryPointMethodWithReturnAndParam()
		{
			try
			{
				this.threadReturn = this.threadWorkerMethodWithReturnAndParam(this.threadParams);
			}
			catch (Exception ex)
			{
				this.exception = ex;
			}
		}

		// Token: 0x0400048E RID: 1166
		private Thread thread;

		// Token: 0x0400048F RID: 1167
		private Exception exception;

		// Token: 0x04000490 RID: 1168
		private ThreadDispatch.ThreadWorkerMethodWithReturn threadWorkerMethodWithReturn;

		// Token: 0x04000491 RID: 1169
		private ThreadDispatch.ThreadWorkerMethodWithReturnAndParam threadWorkerMethodWithReturnAndParam;

		// Token: 0x04000492 RID: 1170
		private ThreadDispatch.ThreadWorkerMethod threadWorkerMethod;

		// Token: 0x04000493 RID: 1171
		private ThreadDispatch.ThreadWorkerMethodWithParam threadWorkerMethodWithParam;

		// Token: 0x04000494 RID: 1172
		private object threadReturn;

		// Token: 0x04000495 RID: 1173
		private object threadParams;

		// Token: 0x04000496 RID: 1174
		private bool backgroundThread;

		// Token: 0x04000497 RID: 1175
		private ApartmentState apartmentType;

		// Token: 0x02000103 RID: 259
		// (Invoke) Token: 0x06000662 RID: 1634
		public delegate object ThreadWorkerMethodWithReturn();

		// Token: 0x02000104 RID: 260
		// (Invoke) Token: 0x06000666 RID: 1638
		public delegate object ThreadWorkerMethodWithReturnAndParam(object param);

		// Token: 0x02000105 RID: 261
		// (Invoke) Token: 0x0600066A RID: 1642
		public delegate void ThreadWorkerMethod();

		// Token: 0x02000106 RID: 262
		// (Invoke) Token: 0x0600066E RID: 1646
		public delegate void ThreadWorkerMethodWithParam(object param);
	}
}
