using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Microsoft.WindowsDeviceRecoveryTool.Common;

namespace Microsoft.WindowsDeviceRecoveryTool.Model
{
	// Token: 0x02000006 RID: 6
	public sealed class CollectionObservable<T> : ObservableCollection<T> where T : NotificationObject
	{
		// Token: 0x06000034 RID: 52 RVA: 0x00002964 File Offset: 0x00000B64
		public CollectionObservable()
		{
			this.CollectionChanged += this.CollectionObservableCollectionChanged;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002984 File Offset: 0x00000B84
		private void CollectionObservableCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				foreach (object obj in e.NewItems)
				{
					INotifyPropertyChanged notifyPropertyChanged = obj as INotifyPropertyChanged;
					if (notifyPropertyChanged != null)
					{
						notifyPropertyChanged.PropertyChanged += this.ItemPropertyChanged;
					}
				}
			}
			if (e.OldItems != null)
			{
				foreach (object obj in e.OldItems)
				{
					INotifyPropertyChanged notifyPropertyChanged = obj as INotifyPropertyChanged;
					if (notifyPropertyChanged != null)
					{
						notifyPropertyChanged.PropertyChanged -= this.ItemPropertyChanged;
					}
				}
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002A94 File Offset: 0x00000C94
		private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			NotifyCollectionChangedEventArgs e2 = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
			this.OnCollectionChanged(e2);
		}
	}
}
