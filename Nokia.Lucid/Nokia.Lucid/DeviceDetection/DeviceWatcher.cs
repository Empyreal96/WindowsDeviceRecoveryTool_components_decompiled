using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using Nokia.Lucid.DeviceDetection.Primitives;
using Nokia.Lucid.Diagnostics;
using Nokia.Lucid.Interop.Win32Types;
using Nokia.Lucid.Primitives;
using Nokia.Lucid.Properties;

namespace Nokia.Lucid.DeviceDetection
{
	// Token: 0x0200000E RID: 14
	public sealed class DeviceWatcher : IHandleDeviceChanged, IHandleThreadException
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600002C RID: 44 RVA: 0x000030D4 File Offset: 0x000012D4
		// (remove) Token: 0x0600002D RID: 45 RVA: 0x0000310C File Offset: 0x0000130C
		public event EventHandler<DeviceChangedEventArgs> DeviceChanged;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600002E RID: 46 RVA: 0x00003144 File Offset: 0x00001344
		// (remove) Token: 0x0600002F RID: 47 RVA: 0x0000317C File Offset: 0x0000137C
		public event EventHandler<ThreadExceptionEventArgs> ThreadException;

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000031B1 File Offset: 0x000013B1
		// (set) Token: 0x06000031 RID: 49 RVA: 0x000031B9 File Offset: 0x000013B9
		public DeviceTypeMap DeviceTypeMap
		{
			get
			{
				return this.deviceTypeMap;
			}
			set
			{
				this.deviceTypeMap = value;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000032 RID: 50 RVA: 0x000031C2 File Offset: 0x000013C2
		// (set) Token: 0x06000033 RID: 51 RVA: 0x000031CA File Offset: 0x000013CA
		public Expression<Func<DeviceIdentifier, bool>> Filter
		{
			get
			{
				return this.filter;
			}
			set
			{
				this.filter = value;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000034 RID: 52 RVA: 0x000031D3 File Offset: 0x000013D3
		public DeviceWatcherStatus Status
		{
			get
			{
				return (DeviceWatcherStatus)this.currentStatus;
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000033B4 File Offset: 0x000015B4
		public IDisposable Start()
		{
			Expression<Func<DeviceIdentifier, bool>> expression = this.Filter;
			if (expression != null)
			{
				RobustTrace.Trace<Expression<Func<DeviceIdentifier, bool>>>(new Action<Expression<Func<DeviceIdentifier, bool>>>(DeviceDetectionTraceSource.Instance.FilterExpressionCompilation_Start), expression);
				try
				{
					this.compiledFilter = expression.Compile();
				}
				catch (Exception ex)
				{
					if (!ExceptionServices.IsCriticalException(ex))
					{
						RobustTrace.Trace<Expression<Func<DeviceIdentifier, bool>>, Exception>(new Action<Expression<Func<DeviceIdentifier, bool>>, Exception>(DeviceDetectionTraceSource.Instance.FilterExpressionCompilation_Error), expression, ex);
					}
					throw;
				}
				RobustTrace.Trace<Expression<Func<DeviceIdentifier, bool>>>(new Action<Expression<Func<DeviceIdentifier, bool>>>(DeviceDetectionTraceSource.Instance.FilterExpressionCompilation_Stop), expression);
			}
			this.cachedTypeMap = this.deviceTypeMap;
			DeviceWatcherStatus deviceWatcherStatus = (DeviceWatcherStatus)Interlocked.CompareExchange(ref this.currentStatus, 1, 0);
			if (deviceWatcherStatus != DeviceWatcherStatus.Created)
			{
				string message = string.Format(CultureInfo.CurrentCulture, Resources.InvalidOperationException_MessageFormat_CouldNotStartDeviceWatcher, new object[]
				{
					deviceWatcherStatus
				});
				throw new InvalidOperationException(message);
			}
			MessageWindow window = null;
			AggregateException exception = null;
			ManualResetEventSlim threadReady = new ManualResetEventSlim();
			ThreadStart start = delegate()
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					try
					{
						MessageWindow.Create(this, this, ref window);
					}
					catch (AggregateException)
					{
						threadReady.Set();
						throw;
					}
					AggregateException ex2 = null;
					try
					{
						window.AttachWindowProc();
						window.RegisterDeviceNotification(this.cachedTypeMap.InterfaceClasses);
					}
					catch (AggregateException ex3)
					{
						ex2 = ex3;
					}
					finally
					{
						threadReady.Set();
					}
					MessageLoop.Run();
					if (ex2 != null && window.Exception != null)
					{
						throw new AggregateException(new Exception[]
						{
							ex2,
							window.Exception
						});
					}
					if (window.Exception != null)
					{
						throw new AggregateException(new Exception[]
						{
							window.Exception
						});
					}
					if (ex2 != null)
					{
						throw new AggregateException(new Exception[]
						{
							ex2
						});
					}
				}
				catch (AggregateException exception)
				{
					exception = exception;
				}
				finally
				{
					if (window != null)
					{
						window.Dispose();
					}
				}
			};
			Thread thread = new Thread(start)
			{
				IsBackground = true
			};
			thread.Start();
			threadReady.Wait();
			if (window == null)
			{
				thread.Join();
				Interlocked.CompareExchange(ref this.currentStatus, 3, 1);
				throw exception;
			}
			return new DeviceWatcher.InvokeOnceWhenDisposed(delegate()
			{
				if (window.Status == MessageWindowStatus.Created)
				{
					try
					{
						window.CloseAsync();
					}
					catch (Win32Exception)
					{
					}
				}
				thread.Join();
				if (exception == null)
				{
					Interlocked.CompareExchange(ref this.currentStatus, 2, 1);
					return;
				}
				Interlocked.CompareExchange(ref this.currentStatus, 3, 1);
				throw exception;
			});
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003528 File Offset: 0x00001728
		void IHandleDeviceChanged.HandleDeviceChanged(int eventType, ref DEV_BROADCAST_DEVICEINTERFACE data)
		{
			EventHandler<DeviceChangedEventArgs> deviceChanged = this.DeviceChanged;
			if (deviceChanged == null)
			{
				return;
			}
			DeviceType deviceType;
			if (!this.cachedTypeMap.TryGetMapping(data.dbcc_classguid, out deviceType))
			{
				return;
			}
			if (deviceType != DeviceType.Interface && deviceType != DeviceType.PhysicalDevice)
			{
				RobustTrace.Trace<Guid, DeviceType>(new Action<Guid, DeviceType>(DeviceDetectionTraceSource.Instance.InvalidDeviceMapping), data.dbcc_classguid, deviceType);
				return;
			}
			DeviceChangeAction deviceChangeAction;
			if (eventType != 32768)
			{
				if (eventType != 32772)
				{
					return;
				}
				deviceChangeAction = DeviceChangeAction.Detach;
			}
			else
			{
				deviceChangeAction = DeviceChangeAction.Attach;
			}
			DeviceIdentifier deviceIdentifier;
			if (!DeviceIdentifier.TryParse(data.dbcc_name, out deviceIdentifier))
			{
				return;
			}
			RobustTrace.Trace<string>(new Action<string>(DeviceDetectionTraceSource.Instance.FilterExpressionEvaluation_Start), deviceIdentifier.Value);
			bool flag;
			try
			{
				flag = (this.compiledFilter != null && this.compiledFilter(deviceIdentifier));
			}
			catch (Exception ex)
			{
				if (!ExceptionServices.IsCriticalException(ex))
				{
					RobustTrace.Trace<string, Exception>(new Action<string, Exception>(DeviceDetectionTraceSource.Instance.FilterExpressionEvaluation_Error), deviceIdentifier.Value, ex);
				}
				throw;
			}
			if (!flag)
			{
				RobustTrace.Trace<string, bool>(new Action<string, bool>(DeviceDetectionTraceSource.Instance.FilterExpressionEvaluation_Stop), deviceIdentifier.Value, false);
				return;
			}
			RobustTrace.Trace<string, bool>(new Action<string, bool>(DeviceDetectionTraceSource.Instance.FilterExpressionEvaluation_Stop), deviceIdentifier.Value, true);
			RobustTrace.Trace<DeviceChangeAction, string, DeviceType>(new Action<DeviceChangeAction, string, DeviceType>(DeviceDetectionTraceSource.Instance.DeviceChangeEvent_Start), deviceChangeAction, deviceIdentifier.Value, deviceType);
			try
			{
				deviceChanged(this, new DeviceChangedEventArgs(deviceChangeAction, deviceIdentifier.Value, deviceType));
			}
			catch (Exception ex2)
			{
				if (!ExceptionServices.IsCriticalException(ex2))
				{
					RobustTrace.Trace<DeviceChangeAction, string, DeviceType, Exception>(new Action<DeviceChangeAction, string, DeviceType, Exception>(DeviceDetectionTraceSource.Instance.DeviceChangeEvent_Error), deviceChangeAction, deviceIdentifier.Value, deviceType, ex2);
				}
				throw;
			}
			RobustTrace.Trace<DeviceChangeAction, string, DeviceType>(new Action<DeviceChangeAction, string, DeviceType>(DeviceDetectionTraceSource.Instance.DeviceChangeEvent_Stop), deviceChangeAction, deviceIdentifier.Value, deviceType);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000036DC File Offset: 0x000018DC
		bool IHandleThreadException.TryHandleThreadException(Exception exception)
		{
			EventHandler<ThreadExceptionEventArgs> threadException = this.ThreadException;
			if (threadException == null)
			{
				return false;
			}
			ThreadExceptionEventArgs threadExceptionEventArgs = new ThreadExceptionEventArgs(exception);
			threadException(this, threadExceptionEventArgs);
			return threadExceptionEventArgs.IsHandled;
		}

		// Token: 0x04000022 RID: 34
		private int currentStatus;

		// Token: 0x04000023 RID: 35
		private Func<DeviceIdentifier, bool> compiledFilter;

		// Token: 0x04000024 RID: 36
		private DeviceTypeMap cachedTypeMap;

		// Token: 0x04000025 RID: 37
		private Expression<Func<DeviceIdentifier, bool>> filter = FilterExpression.DefaultExpression;

		// Token: 0x04000026 RID: 38
		private DeviceTypeMap deviceTypeMap = DeviceTypeMap.DefaultMap;

		// Token: 0x0200000F RID: 15
		private sealed class InvokeOnceWhenDisposed : IDisposable
		{
			// Token: 0x06000039 RID: 57 RVA: 0x00003728 File Offset: 0x00001928
			public InvokeOnceWhenDisposed(Action action)
			{
				this.action = action;
			}

			// Token: 0x0600003A RID: 58 RVA: 0x00003738 File Offset: 0x00001938
			public void Dispose()
			{
				Action action = Interlocked.Exchange<Action>(ref this.action, null);
				if (action != null)
				{
					action();
				}
			}

			// Token: 0x04000029 RID: 41
			private Action action;
		}
	}
}
