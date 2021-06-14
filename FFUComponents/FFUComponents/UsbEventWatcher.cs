using System;
using System.Threading;
using System.Windows.Forms;

namespace FFUComponents
{
	// Token: 0x02000053 RID: 83
	internal class UsbEventWatcher : IDisposable
	{
		// Token: 0x06000179 RID: 377 RVA: 0x00007C64 File Offset: 0x00005E64
		public UsbEventWatcher(IUsbEventSink eventSink, Guid classGuid, Guid ifGuid)
		{
			this.isDisposed = false;
			UsbEventWatcher.UsbFormArgs usbFormArgs = new UsbEventWatcher.UsbFormArgs
			{
				sink = eventSink,
				devClass = classGuid,
				devIf = ifGuid,
				eventWatcher = this,
				contextEvent = new ManualResetEvent(false)
			};
			ParameterizedThreadStart start = delegate(object a)
			{
				bool flag = false;
				do
				{
					try
					{
						UsbEventWatcher.UsbFormArgs usbFormArgs2 = (UsbEventWatcher.UsbFormArgs)a;
						using (UsbEventWatcherForm usbEventWatcherForm = new UsbEventWatcherForm(usbFormArgs2.sink, usbFormArgs2.devClass, usbFormArgs2.devIf))
						{
							using (ApplicationContext applicationContext = new ApplicationContext(usbEventWatcherForm))
							{
								usbFormArgs2.eventWatcher.runningContext = applicationContext;
								usbFormArgs2.contextEvent.Set();
								Application.Run(applicationContext);
								flag = true;
							}
						}
					}
					catch (Exception ex)
					{
						Thread.Sleep(1000);
						FFUManager.HostLogger.EventWriteThreadException(ex.ToString());
					}
				}
				while (!flag);
			};
			Thread thread = new Thread(start);
			thread.Start(usbFormArgs);
			if (!usbFormArgs.contextEvent.WaitOne(UsbEventWatcher.timeout))
			{
				try
				{
					this.Dispose(true);
				}
				catch (Exception)
				{
				}
				throw new TimeoutException();
			}
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00007D30 File Offset: 0x00005F30
		private void Dispose(bool fDisposing)
		{
			if (fDisposing && !this.isDisposed)
			{
				this.isDisposed = true;
				try
				{
					MethodInvoker method = delegate()
					{
						this.runningContext.MainForm.Close();
					};
					IAsyncResult asyncResult = this.runningContext.MainForm.BeginInvoke(method);
					asyncResult.AsyncWaitHandle.WaitOne();
					this.runningContext.ExitThread();
					this.runningContext = null;
				}
				catch (InvalidOperationException)
				{
				}
			}
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00007DA8 File Offset: 0x00005FA8
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x04000168 RID: 360
		private static readonly int timeout = 60000;

		// Token: 0x04000169 RID: 361
		private ApplicationContext runningContext;

		// Token: 0x0400016A RID: 362
		private bool isDisposed;

		// Token: 0x02000054 RID: 84
		private struct UsbFormArgs
		{
			// Token: 0x0400016C RID: 364
			public IUsbEventSink sink;

			// Token: 0x0400016D RID: 365
			public Guid devClass;

			// Token: 0x0400016E RID: 366
			public Guid devIf;

			// Token: 0x0400016F RID: 367
			public UsbEventWatcher eventWatcher;

			// Token: 0x04000170 RID: 368
			public ManualResetEvent contextEvent;
		}
	}
}
