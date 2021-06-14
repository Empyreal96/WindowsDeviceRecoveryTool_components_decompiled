using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace MS.Internal.Data
{
	// Token: 0x0200070A RID: 1802
	internal abstract class BindingWorker
	{
		// Token: 0x06007378 RID: 29560 RVA: 0x002114F3 File Offset: 0x0020F6F3
		protected BindingWorker(BindingExpression b)
		{
			this._bindingExpression = b;
		}

		// Token: 0x17001B69 RID: 7017
		// (get) Token: 0x06007379 RID: 29561 RVA: 0x0000C238 File Offset: 0x0000A438
		internal virtual Type SourcePropertyType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001B6A RID: 7018
		// (get) Token: 0x0600737A RID: 29562 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual bool CanUpdate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001B6B RID: 7019
		// (get) Token: 0x0600737B RID: 29563 RVA: 0x00211502 File Offset: 0x0020F702
		internal BindingExpression ParentBindingExpression
		{
			get
			{
				return this._bindingExpression;
			}
		}

		// Token: 0x17001B6C RID: 7020
		// (get) Token: 0x0600737C RID: 29564 RVA: 0x0021150A File Offset: 0x0020F70A
		internal Type TargetPropertyType
		{
			get
			{
				return this.TargetProperty.PropertyType;
			}
		}

		// Token: 0x17001B6D RID: 7021
		// (get) Token: 0x0600737D RID: 29565 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual bool IsDBNullValidForUpdate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001B6E RID: 7022
		// (get) Token: 0x0600737E RID: 29566 RVA: 0x0000C238 File Offset: 0x0000A438
		internal virtual object SourceItem
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001B6F RID: 7023
		// (get) Token: 0x0600737F RID: 29567 RVA: 0x0000C238 File Offset: 0x0000A438
		internal virtual string SourcePropertyName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06007380 RID: 29568 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void AttachDataItem()
		{
		}

		// Token: 0x06007381 RID: 29569 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void DetachDataItem()
		{
		}

		// Token: 0x06007382 RID: 29570 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void OnCurrentChanged(ICollectionView collectionView, EventArgs args)
		{
		}

		// Token: 0x06007383 RID: 29571 RVA: 0x0000C238 File Offset: 0x0000A438
		internal virtual object RawValue()
		{
			return null;
		}

		// Token: 0x06007384 RID: 29572 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void UpdateValue(object value)
		{
		}

		// Token: 0x06007385 RID: 29573 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void RefreshValue()
		{
		}

		// Token: 0x06007386 RID: 29574 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual bool UsesDependencyProperty(DependencyObject d, DependencyProperty dp)
		{
			return false;
		}

		// Token: 0x06007387 RID: 29575 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void OnSourceInvalidation(DependencyObject d, DependencyProperty dp, bool isASubPropertyChange)
		{
		}

		// Token: 0x06007388 RID: 29576 RVA: 0x00016748 File Offset: 0x00014948
		internal virtual bool IsPathCurrent()
		{
			return true;
		}

		// Token: 0x17001B70 RID: 7024
		// (get) Token: 0x06007389 RID: 29577 RVA: 0x00211517 File Offset: 0x0020F717
		protected Binding ParentBinding
		{
			get
			{
				return this.ParentBindingExpression.ParentBinding;
			}
		}

		// Token: 0x17001B71 RID: 7025
		// (get) Token: 0x0600738A RID: 29578 RVA: 0x00211524 File Offset: 0x0020F724
		protected bool IsDynamic
		{
			get
			{
				return this.ParentBindingExpression.IsDynamic;
			}
		}

		// Token: 0x17001B72 RID: 7026
		// (get) Token: 0x0600738B RID: 29579 RVA: 0x00211531 File Offset: 0x0020F731
		internal bool IsReflective
		{
			get
			{
				return this.ParentBindingExpression.IsReflective;
			}
		}

		// Token: 0x17001B73 RID: 7027
		// (get) Token: 0x0600738C RID: 29580 RVA: 0x0021153E File Offset: 0x0020F73E
		protected bool IgnoreSourcePropertyChange
		{
			get
			{
				return this.ParentBindingExpression.IgnoreSourcePropertyChange;
			}
		}

		// Token: 0x17001B74 RID: 7028
		// (get) Token: 0x0600738D RID: 29581 RVA: 0x0021154B File Offset: 0x0020F74B
		protected object DataItem
		{
			get
			{
				return this.ParentBindingExpression.DataItem;
			}
		}

		// Token: 0x17001B75 RID: 7029
		// (get) Token: 0x0600738E RID: 29582 RVA: 0x00211558 File Offset: 0x0020F758
		protected DependencyObject TargetElement
		{
			get
			{
				return this.ParentBindingExpression.TargetElement;
			}
		}

		// Token: 0x17001B76 RID: 7030
		// (get) Token: 0x0600738F RID: 29583 RVA: 0x00211565 File Offset: 0x0020F765
		protected DependencyProperty TargetProperty
		{
			get
			{
				return this.ParentBindingExpression.TargetProperty;
			}
		}

		// Token: 0x17001B77 RID: 7031
		// (get) Token: 0x06007390 RID: 29584 RVA: 0x00211572 File Offset: 0x0020F772
		protected DataBindEngine Engine
		{
			get
			{
				return this.ParentBindingExpression.Engine;
			}
		}

		// Token: 0x17001B78 RID: 7032
		// (get) Token: 0x06007391 RID: 29585 RVA: 0x0021157F File Offset: 0x0020F77F
		protected Dispatcher Dispatcher
		{
			get
			{
				return this.ParentBindingExpression.Dispatcher;
			}
		}

		// Token: 0x17001B79 RID: 7033
		// (get) Token: 0x06007392 RID: 29586 RVA: 0x0021158C File Offset: 0x0020F78C
		// (set) Token: 0x06007393 RID: 29587 RVA: 0x00211599 File Offset: 0x0020F799
		protected BindingStatusInternal Status
		{
			get
			{
				return this.ParentBindingExpression.StatusInternal;
			}
			set
			{
				this.ParentBindingExpression.SetStatus(value);
			}
		}

		// Token: 0x06007394 RID: 29588 RVA: 0x002115A7 File Offset: 0x0020F7A7
		protected void SetTransferIsPending(bool value)
		{
			this.ParentBindingExpression.IsTransferPending = value;
		}

		// Token: 0x06007395 RID: 29589 RVA: 0x002115B5 File Offset: 0x0020F7B5
		internal bool HasValue(BindingWorker.Feature id)
		{
			return this._values.HasValue((int)id);
		}

		// Token: 0x06007396 RID: 29590 RVA: 0x002115C3 File Offset: 0x0020F7C3
		internal object GetValue(BindingWorker.Feature id, object defaultValue)
		{
			return this._values.GetValue((int)id, defaultValue);
		}

		// Token: 0x06007397 RID: 29591 RVA: 0x002115D2 File Offset: 0x0020F7D2
		internal void SetValue(BindingWorker.Feature id, object value)
		{
			this._values.SetValue((int)id, value);
		}

		// Token: 0x06007398 RID: 29592 RVA: 0x002115E1 File Offset: 0x0020F7E1
		internal void SetValue(BindingWorker.Feature id, object value, object defaultValue)
		{
			if (object.Equals(value, defaultValue))
			{
				this._values.ClearValue((int)id);
				return;
			}
			this._values.SetValue((int)id, value);
		}

		// Token: 0x06007399 RID: 29593 RVA: 0x00211606 File Offset: 0x0020F806
		internal void ClearValue(BindingWorker.Feature id)
		{
			this._values.ClearValue((int)id);
		}

		// Token: 0x040037AC RID: 14252
		private BindingExpression _bindingExpression;

		// Token: 0x040037AD RID: 14253
		private UncommonValueTable _values;

		// Token: 0x02000B43 RID: 2883
		internal enum Feature
		{
			// Token: 0x04004AC4 RID: 19140
			XmlWorker,
			// Token: 0x04004AC5 RID: 19141
			PendingGetValueRequest,
			// Token: 0x04004AC6 RID: 19142
			PendingSetValueRequest,
			// Token: 0x04004AC7 RID: 19143
			LastFeatureId
		}
	}
}
