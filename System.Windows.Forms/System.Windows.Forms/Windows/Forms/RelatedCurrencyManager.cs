using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x0200032E RID: 814
	internal class RelatedCurrencyManager : CurrencyManager
	{
		// Token: 0x06003260 RID: 12896 RVA: 0x000EAAB4 File Offset: 0x000E8CB4
		internal RelatedCurrencyManager(BindingManagerBase parentManager, string dataField) : base(null)
		{
			this.Bind(parentManager, dataField);
		}

		// Token: 0x06003261 RID: 12897 RVA: 0x000EAAC8 File Offset: 0x000E8CC8
		internal void Bind(BindingManagerBase parentManager, string dataField)
		{
			this.UnwireParentManager(this.parentManager);
			this.parentManager = parentManager;
			this.dataField = dataField;
			this.fieldInfo = parentManager.GetItemProperties().Find(dataField, true);
			if (this.fieldInfo == null || !typeof(IList).IsAssignableFrom(this.fieldInfo.PropertyType))
			{
				throw new ArgumentException(SR.GetString("RelatedListManagerChild", new object[]
				{
					dataField
				}));
			}
			this.finalType = this.fieldInfo.PropertyType;
			this.WireParentManager(this.parentManager);
			this.ParentManager_CurrentItemChanged(parentManager, EventArgs.Empty);
		}

		// Token: 0x06003262 RID: 12898 RVA: 0x000EAB69 File Offset: 0x000E8D69
		private void UnwireParentManager(BindingManagerBase bmb)
		{
			if (bmb != null)
			{
				bmb.CurrentItemChanged -= this.ParentManager_CurrentItemChanged;
				if (bmb is CurrencyManager)
				{
					(bmb as CurrencyManager).MetaDataChanged -= this.ParentManager_MetaDataChanged;
				}
			}
		}

		// Token: 0x06003263 RID: 12899 RVA: 0x000EAB9F File Offset: 0x000E8D9F
		private void WireParentManager(BindingManagerBase bmb)
		{
			if (bmb != null)
			{
				bmb.CurrentItemChanged += this.ParentManager_CurrentItemChanged;
				if (bmb is CurrencyManager)
				{
					(bmb as CurrencyManager).MetaDataChanged += this.ParentManager_MetaDataChanged;
				}
			}
		}

		// Token: 0x06003264 RID: 12900 RVA: 0x000EABD8 File Offset: 0x000E8DD8
		internal override PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			PropertyDescriptor[] array;
			if (listAccessors != null && listAccessors.Length != 0)
			{
				array = new PropertyDescriptor[listAccessors.Length + 1];
				listAccessors.CopyTo(array, 1);
			}
			else
			{
				array = new PropertyDescriptor[1];
			}
			array[0] = this.fieldInfo;
			return this.parentManager.GetItemProperties(array);
		}

		// Token: 0x06003265 RID: 12901 RVA: 0x00017BB3 File Offset: 0x00015DB3
		public override PropertyDescriptorCollection GetItemProperties()
		{
			return this.GetItemProperties(null);
		}

		// Token: 0x06003266 RID: 12902 RVA: 0x000EAC20 File Offset: 0x000E8E20
		internal override string GetListName()
		{
			string listName = this.GetListName(new ArrayList());
			if (listName.Length > 0)
			{
				return listName;
			}
			return base.GetListName();
		}

		// Token: 0x06003267 RID: 12903 RVA: 0x000EAC4A File Offset: 0x000E8E4A
		protected internal override string GetListName(ArrayList listAccessors)
		{
			listAccessors.Insert(0, this.fieldInfo);
			return this.parentManager.GetListName(listAccessors);
		}

		// Token: 0x06003268 RID: 12904 RVA: 0x000EAC65 File Offset: 0x000E8E65
		private void ParentManager_MetaDataChanged(object sender, EventArgs e)
		{
			base.OnMetaDataChanged(e);
		}

		// Token: 0x06003269 RID: 12905 RVA: 0x000EAC70 File Offset: 0x000E8E70
		private void ParentManager_CurrentItemChanged(object sender, EventArgs e)
		{
			if (RelatedCurrencyManager.IgnoreItemChangedTable.Contains(this.parentManager))
			{
				return;
			}
			int listposition = this.listposition;
			try
			{
				base.PullData();
			}
			catch (Exception e2)
			{
				base.OnDataError(e2);
			}
			if (this.parentManager is CurrencyManager)
			{
				CurrencyManager currencyManager = (CurrencyManager)this.parentManager;
				if (currencyManager.Count > 0)
				{
					this.SetDataSource(this.fieldInfo.GetValue(currencyManager.Current));
					this.listposition = ((this.Count > 0) ? 0 : -1);
					goto IL_DC;
				}
				currencyManager.AddNew();
				try
				{
					RelatedCurrencyManager.IgnoreItemChangedTable.Add(currencyManager);
					currencyManager.CancelCurrentEdit();
					goto IL_DC;
				}
				finally
				{
					if (RelatedCurrencyManager.IgnoreItemChangedTable.Contains(currencyManager))
					{
						RelatedCurrencyManager.IgnoreItemChangedTable.Remove(currencyManager);
					}
				}
			}
			this.SetDataSource(this.fieldInfo.GetValue(this.parentManager.Current));
			this.listposition = ((this.Count > 0) ? 0 : -1);
			IL_DC:
			if (listposition != this.listposition)
			{
				this.OnPositionChanged(EventArgs.Empty);
			}
			this.OnCurrentChanged(EventArgs.Empty);
			this.OnCurrentItemChanged(EventArgs.Empty);
		}

		// Token: 0x04001E45 RID: 7749
		private BindingManagerBase parentManager;

		// Token: 0x04001E46 RID: 7750
		private string dataField;

		// Token: 0x04001E47 RID: 7751
		private PropertyDescriptor fieldInfo;

		// Token: 0x04001E48 RID: 7752
		private static List<BindingManagerBase> IgnoreItemChangedTable = new List<BindingManagerBase>();
	}
}
