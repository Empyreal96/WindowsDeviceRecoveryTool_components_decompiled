using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Microsoft.WindowsDeviceRecoveryTool.Common
{
	// Token: 0x02000008 RID: 8
	[CompilerGenerated]
	public class NotificationObject : INotifyPropertyChanged
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000019 RID: 25 RVA: 0x0000284C File Offset: 0x00000A4C
		// (remove) Token: 0x0600001A RID: 26 RVA: 0x00002888 File Offset: 0x00000A88
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x0600001B RID: 27 RVA: 0x000028C4 File Offset: 0x00000AC4
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			this.RaisePropertyChanged(propertyChanged, propertyName);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000290C File Offset: 0x00000B0C
		protected virtual void RaisePropertyChanged(PropertyChangedEventHandler handler, string propertyName)
		{
			if (handler != null)
			{
				AppDispatcher.Execute(delegate
				{
					handler(this, new PropertyChangedEventArgs(propertyName));
				}, false);
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002964 File Offset: 0x00000B64
		protected void RaisePropertyChanged<T>(Expression<Func<T>> expression)
		{
			string name = ReflectionHelper.GetName<T>(expression);
			this.OnPropertyChanged(name);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002984 File Offset: 0x00000B84
		protected void SetValue<T>(Expression<Func<T>> expression, Action setValueAction)
		{
			if (setValueAction != null)
			{
				setValueAction();
			}
			this.RaisePropertyChanged<T>(expression);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000029AB File Offset: 0x00000BAB
		protected void SetValue<T>(Expression<Func<T>> expression, ref T backfield, T value)
		{
			backfield = value;
			this.RaisePropertyChanged<T>(expression);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000029C0 File Offset: 0x00000BC0
		protected void SetValue<T>(Expression<Func<T>> expression, Action<T> setValueAction, T value)
		{
			if (setValueAction != null)
			{
				setValueAction(value);
				this.RaisePropertyChanged<T>(expression);
			}
		}
	}
}
