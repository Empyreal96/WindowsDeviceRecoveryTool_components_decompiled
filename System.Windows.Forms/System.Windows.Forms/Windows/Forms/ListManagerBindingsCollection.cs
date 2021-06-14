using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x020002C2 RID: 706
	[DefaultEvent("CollectionChanged")]
	internal class ListManagerBindingsCollection : BindingsCollection
	{
		// Token: 0x06002963 RID: 10595 RVA: 0x000C0B64 File Offset: 0x000BED64
		internal ListManagerBindingsCollection(BindingManagerBase bindingManagerBase)
		{
			this.bindingManagerBase = bindingManagerBase;
		}

		// Token: 0x06002964 RID: 10596 RVA: 0x000C0B74 File Offset: 0x000BED74
		protected override void AddCore(Binding dataBinding)
		{
			if (dataBinding == null)
			{
				throw new ArgumentNullException("dataBinding");
			}
			if (dataBinding.BindingManagerBase == this.bindingManagerBase)
			{
				throw new ArgumentException(SR.GetString("BindingsCollectionAdd1"), "dataBinding");
			}
			if (dataBinding.BindingManagerBase != null)
			{
				throw new ArgumentException(SR.GetString("BindingsCollectionAdd2"), "dataBinding");
			}
			dataBinding.SetListManager(this.bindingManagerBase);
			base.AddCore(dataBinding);
		}

		// Token: 0x06002965 RID: 10597 RVA: 0x000C0BE4 File Offset: 0x000BEDE4
		protected override void ClearCore()
		{
			int count = this.Count;
			for (int i = 0; i < count; i++)
			{
				Binding binding = base[i];
				binding.SetListManager(null);
			}
			base.ClearCore();
		}

		// Token: 0x06002966 RID: 10598 RVA: 0x000C0C19 File Offset: 0x000BEE19
		protected override void RemoveCore(Binding dataBinding)
		{
			if (dataBinding.BindingManagerBase != this.bindingManagerBase)
			{
				throw new ArgumentException(SR.GetString("BindingsCollectionForeign"));
			}
			dataBinding.SetListManager(null);
			base.RemoveCore(dataBinding);
		}

		// Token: 0x040011DE RID: 4574
		private BindingManagerBase bindingManagerBase;
	}
}
