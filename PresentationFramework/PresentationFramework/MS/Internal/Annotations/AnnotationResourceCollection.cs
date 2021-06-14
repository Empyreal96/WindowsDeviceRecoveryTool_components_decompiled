using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Annotations;

namespace MS.Internal.Annotations
{
	// Token: 0x020007C5 RID: 1989
	internal sealed class AnnotationResourceCollection : AnnotationObservableCollection<AnnotationResource>
	{
		// Token: 0x1400016C RID: 364
		// (add) Token: 0x06007B80 RID: 31616 RVA: 0x0022BAC8 File Offset: 0x00229CC8
		// (remove) Token: 0x06007B81 RID: 31617 RVA: 0x0022BB00 File Offset: 0x00229D00
		public event PropertyChangedEventHandler ItemChanged;

		// Token: 0x06007B82 RID: 31618 RVA: 0x0022BB38 File Offset: 0x00229D38
		protected override void ProtectedClearItems()
		{
			List<AnnotationResource> list = new List<AnnotationResource>(this);
			base.Items.Clear();
			this.OnPropertyChanged(this.CountString);
			this.OnPropertyChanged(this.IndexerName);
			this.OnCollectionCleared(list);
		}

		// Token: 0x06007B83 RID: 31619 RVA: 0x0022BB76 File Offset: 0x00229D76
		protected override void ProtectedSetItem(int index, AnnotationResource item)
		{
			base.ObservableCollectionSetItem(index, item);
		}

		// Token: 0x06007B84 RID: 31620 RVA: 0x0022BB80 File Offset: 0x00229D80
		private void OnCollectionCleared(IEnumerable<AnnotationResource> list)
		{
			foreach (object changedItem in list)
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, changedItem, 0));
			}
		}

		// Token: 0x06007B85 RID: 31621 RVA: 0x0022BBD0 File Offset: 0x00229DD0
		private void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x06007B86 RID: 31622 RVA: 0x0022BBDE File Offset: 0x00229DDE
		protected override void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (this.ItemChanged != null)
			{
				this.ItemChanged(sender, e);
			}
		}
	}
}
