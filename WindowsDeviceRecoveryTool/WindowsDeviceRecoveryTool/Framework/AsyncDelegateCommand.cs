using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.WindowsDeviceRecoveryTool.Common;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;

namespace Microsoft.WindowsDeviceRecoveryTool.Framework
{
	// Token: 0x02000044 RID: 68
	public class AsyncDelegateCommand<T> : IAsyncDelegateCommand, IDelegateCommand, ICommand, IDisposable
	{
		// Token: 0x0600025B RID: 603 RVA: 0x0000ED4E File Offset: 0x0000CF4E
		public AsyncDelegateCommand(Action<T, CancellationToken> execute) : this(execute, null, null)
		{
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000ED5C File Offset: 0x0000CF5C
		public AsyncDelegateCommand(Action<T, CancellationToken> execute, Func<object, bool> canExecute) : this(execute, canExecute, null)
		{
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000ED6A File Offset: 0x0000CF6A
		public AsyncDelegateCommand(Action<T, CancellationToken> execute, KeyGesture keyGesture) : this(execute, null, keyGesture)
		{
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000ED78 File Offset: 0x0000CF78
		public AsyncDelegateCommand(Action<T, CancellationToken> execute, Func<object, bool> canExecute, KeyGesture keyGesture)
		{
			this.execute = execute;
			this.canExecute = canExecute;
			this.KeyGesture = keyGesture;
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600025F RID: 607 RVA: 0x0000ED9C File Offset: 0x0000CF9C
		// (remove) Token: 0x06000260 RID: 608 RVA: 0x0000EDD8 File Offset: 0x0000CFD8
		public event EventHandler CanExecuteChanged;

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000261 RID: 609 RVA: 0x0000EE14 File Offset: 0x0000D014
		// (set) Token: 0x06000262 RID: 610 RVA: 0x0000EE2B File Offset: 0x0000D02B
		public KeyGesture KeyGesture { get; private set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000263 RID: 611 RVA: 0x0000EE34 File Offset: 0x0000D034
		public CancellationTokenSource CancellationTokenSource
		{
			get
			{
				return this.tokenSource;
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000EE4C File Offset: 0x0000D04C
		public bool CanExecute(object parameter)
		{
			bool result;
			if (this.canExecute != null)
			{
				bool flag = false;
				try
				{
					flag = this.canExecute(parameter);
				}
				catch (Exception error)
				{
					Tracer<IAsyncDelegateCommand>.WriteError(error);
					throw;
				}
				result = flag;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000EEA0 File Offset: 0x0000D0A0
		public void Execute(object parameter)
		{
			if (parameter is T)
			{
				this.Execute((T)((object)parameter));
			}
			else
			{
				this.Execute(default(T));
			}
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000EF00 File Offset: 0x0000D100
		public virtual void Execute(T parameter)
		{
			this.tokenSource = new CancellationTokenSource();
			this.currentTask = Task.Run(delegate()
			{
				this.SafeExecute(parameter);
			});
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000EF48 File Offset: 0x0000D148
		public void Cancel()
		{
			if (this.tokenSource != null)
			{
				this.tokenSource.Cancel();
			}
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000EF74 File Offset: 0x0000D174
		public void Wait()
		{
			if (this.currentTask != null && !this.currentTask.IsCompleted)
			{
				this.currentTask.Wait();
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000EFC0 File Offset: 0x0000D1C0
		public void RaiseCanExecuteChanged()
		{
			if (this.CanExecuteChanged != null)
			{
				AppDispatcher.Execute(delegate
				{
					this.CanExecuteChanged(this, EventArgs.Empty);
				}, false);
			}
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000EFFA File Offset: 0x0000D1FA
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000F00C File Offset: 0x0000D20C
		private void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					if (this.tokenSource != null)
					{
						this.tokenSource.Dispose();
					}
				}
				this.disposed = true;
			}
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000F068 File Offset: 0x0000D268
		private void SafeExecute(T parameter)
		{
			try
			{
				this.execute(parameter, this.tokenSource.Token);
			}
			catch (Exception ex)
			{
				Exception ex2;
				Exception ex = ex2;
				Tracer<IAsyncDelegateCommand>.WriteError(ex);
				AppDispatcher.Execute(delegate
				{
					throw ex;
				}, false);
			}
		}

		// Token: 0x040000F2 RID: 242
		private readonly Action<T, CancellationToken> execute;

		// Token: 0x040000F3 RID: 243
		private readonly Func<object, bool> canExecute;

		// Token: 0x040000F4 RID: 244
		private CancellationTokenSource tokenSource;

		// Token: 0x040000F5 RID: 245
		private Task currentTask;

		// Token: 0x040000F6 RID: 246
		private bool disposed;
	}
}
