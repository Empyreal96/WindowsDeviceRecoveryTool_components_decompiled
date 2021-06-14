using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x02000737 RID: 1847
	internal class ParameterCollection : Collection<object>, IList, ICollection, IEnumerable
	{
		// Token: 0x06007613 RID: 30227 RVA: 0x0021AD5D File Offset: 0x00218F5D
		public ParameterCollection(ParameterCollectionChanged parametersChanged)
		{
			this._parametersChanged = parametersChanged;
		}

		// Token: 0x17001C1E RID: 7198
		// (get) Token: 0x06007614 RID: 30228 RVA: 0x0021AD6C File Offset: 0x00218F6C
		bool IList.IsReadOnly
		{
			get
			{
				return this.IsReadOnly;
			}
		}

		// Token: 0x17001C1F RID: 7199
		// (get) Token: 0x06007615 RID: 30229 RVA: 0x0021AD74 File Offset: 0x00218F74
		bool IList.IsFixedSize
		{
			get
			{
				return this.IsFixedSize;
			}
		}

		// Token: 0x06007616 RID: 30230 RVA: 0x0021AD7C File Offset: 0x00218F7C
		protected override void ClearItems()
		{
			this.CheckReadOnly();
			base.ClearItems();
			this.OnCollectionChanged();
		}

		// Token: 0x06007617 RID: 30231 RVA: 0x0021AD90 File Offset: 0x00218F90
		protected override void InsertItem(int index, object value)
		{
			this.CheckReadOnly();
			base.InsertItem(index, value);
			this.OnCollectionChanged();
		}

		// Token: 0x06007618 RID: 30232 RVA: 0x0021ADA6 File Offset: 0x00218FA6
		protected override void RemoveItem(int index)
		{
			this.CheckReadOnly();
			base.RemoveItem(index);
			this.OnCollectionChanged();
		}

		// Token: 0x06007619 RID: 30233 RVA: 0x0021ADBB File Offset: 0x00218FBB
		protected override void SetItem(int index, object value)
		{
			this.CheckReadOnly();
			base.SetItem(index, value);
			this.OnCollectionChanged();
		}

		// Token: 0x17001C20 RID: 7200
		// (get) Token: 0x0600761A RID: 30234 RVA: 0x0021ADD1 File Offset: 0x00218FD1
		// (set) Token: 0x0600761B RID: 30235 RVA: 0x0021ADD9 File Offset: 0x00218FD9
		protected virtual bool IsReadOnly
		{
			get
			{
				return this._isReadOnly;
			}
			set
			{
				this._isReadOnly = value;
			}
		}

		// Token: 0x17001C21 RID: 7201
		// (get) Token: 0x0600761C RID: 30236 RVA: 0x0021AD6C File Offset: 0x00218F6C
		protected bool IsFixedSize
		{
			get
			{
				return this.IsReadOnly;
			}
		}

		// Token: 0x0600761D RID: 30237 RVA: 0x0021ADE2 File Offset: 0x00218FE2
		internal void SetReadOnly(bool isReadOnly)
		{
			this.IsReadOnly = isReadOnly;
		}

		// Token: 0x0600761E RID: 30238 RVA: 0x0021ADEB File Offset: 0x00218FEB
		internal void ClearInternal()
		{
			base.ClearItems();
		}

		// Token: 0x0600761F RID: 30239 RVA: 0x0021ADF3 File Offset: 0x00218FF3
		private void CheckReadOnly()
		{
			if (this.IsReadOnly)
			{
				throw new InvalidOperationException(SR.Get("ObjectDataProviderParameterCollectionIsNotInUse"));
			}
		}

		// Token: 0x06007620 RID: 30240 RVA: 0x0021AE0D File Offset: 0x0021900D
		private void OnCollectionChanged()
		{
			this._parametersChanged(this);
		}

		// Token: 0x04003857 RID: 14423
		private bool _isReadOnly;

		// Token: 0x04003858 RID: 14424
		private ParameterCollectionChanged _parametersChanged;
	}
}
