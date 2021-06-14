using System;
using System.Windows.Input;
using Microsoft.WindowsDeviceRecoveryTool.Common;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;

namespace Microsoft.WindowsDeviceRecoveryTool.Framework
{
	// Token: 0x020000E8 RID: 232
	public class DelegateCommand<T> : IDelegateCommand, ICommand
	{
		// Token: 0x06000774 RID: 1908 RVA: 0x000276D8 File Offset: 0x000258D8
		public DelegateCommand(Action<T> execute) : this(execute, null, null)
		{
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x000276E6 File Offset: 0x000258E6
		public DelegateCommand(Action<T> execute, Func<object, bool> canExecute) : this(execute, canExecute, null)
		{
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x000276F4 File Offset: 0x000258F4
		public DelegateCommand(Action<T> execute, KeyGesture keyGesture) : this(execute, null, keyGesture)
		{
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00027702 File Offset: 0x00025902
		public DelegateCommand(Action<T> execute, Func<object, bool> canExecute, KeyGesture keyGesture)
		{
			this.execute = execute;
			this.canExecute = canExecute;
			this.KeyGesture = keyGesture;
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000778 RID: 1912 RVA: 0x00027724 File Offset: 0x00025924
		// (remove) Token: 0x06000779 RID: 1913 RVA: 0x00027760 File Offset: 0x00025960
		public event EventHandler CanExecuteChanged;

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x0002779C File Offset: 0x0002599C
		// (set) Token: 0x0600077B RID: 1915 RVA: 0x000277B3 File Offset: 0x000259B3
		public KeyGesture KeyGesture { get; private set; }

		// Token: 0x0600077C RID: 1916 RVA: 0x000277BC File Offset: 0x000259BC
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
					Tracer<IDelegateCommand>.WriteError(error);
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

		// Token: 0x0600077D RID: 1917 RVA: 0x00027810 File Offset: 0x00025A10
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

		// Token: 0x0600077E RID: 1918 RVA: 0x00027874 File Offset: 0x00025A74
		public virtual void Execute(T parameter)
		{
			try
			{
				AppDispatcher.Execute(delegate
				{
					this.execute(parameter);
				}, false);
			}
			catch (Exception error)
			{
				Tracer<IDelegateCommand>.WriteError(error);
				throw;
			}
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x000278E8 File Offset: 0x00025AE8
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

		// Token: 0x04000357 RID: 855
		private readonly Action<T> execute;

		// Token: 0x04000358 RID: 856
		private readonly Func<object, bool> canExecute;
	}
}
