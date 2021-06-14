using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x0200012C RID: 300
	internal class BindToObject
	{
		// Token: 0x060008F0 RID: 2288 RVA: 0x0001B03D File Offset: 0x0001923D
		private void PropValueChanged(object sender, EventArgs e)
		{
			if (this.bindingManager != null)
			{
				this.bindingManager.OnCurrentChanged(EventArgs.Empty);
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x060008F1 RID: 2289 RVA: 0x0001B058 File Offset: 0x00019258
		private bool IsDataSourceInitialized
		{
			get
			{
				if (this.dataSourceInitialized)
				{
					return true;
				}
				ISupportInitializeNotification supportInitializeNotification = this.dataSource as ISupportInitializeNotification;
				if (supportInitializeNotification == null || supportInitializeNotification.IsInitialized)
				{
					this.dataSourceInitialized = true;
					return true;
				}
				if (this.waitingOnDataSource)
				{
					return false;
				}
				supportInitializeNotification.Initialized += this.DataSource_Initialized;
				this.waitingOnDataSource = true;
				return false;
			}
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x0001B0B3 File Offset: 0x000192B3
		internal BindToObject(Binding owner, object dataSource, string dataMember)
		{
			this.owner = owner;
			this.dataSource = dataSource;
			this.dataMember = new BindingMemberInfo(dataMember);
			this.CheckBinding();
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0001B0E8 File Offset: 0x000192E8
		private void DataSource_Initialized(object sender, EventArgs e)
		{
			ISupportInitializeNotification supportInitializeNotification = this.dataSource as ISupportInitializeNotification;
			if (supportInitializeNotification != null)
			{
				supportInitializeNotification.Initialized -= this.DataSource_Initialized;
			}
			this.waitingOnDataSource = false;
			this.dataSourceInitialized = true;
			this.CheckBinding();
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0001B12C File Offset: 0x0001932C
		internal void SetBindingManagerBase(BindingManagerBase lManager)
		{
			if (this.bindingManager == lManager)
			{
				return;
			}
			if (this.bindingManager != null && this.fieldInfo != null && this.bindingManager.IsBinding && !(this.bindingManager is CurrencyManager))
			{
				this.fieldInfo.RemoveValueChanged(this.bindingManager.Current, new EventHandler(this.PropValueChanged));
				this.fieldInfo = null;
			}
			this.bindingManager = lManager;
			this.CheckBinding();
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x060008F5 RID: 2293 RVA: 0x0001B1A3 File Offset: 0x000193A3
		internal string DataErrorText
		{
			get
			{
				return this.errorText;
			}
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x0001B1AC File Offset: 0x000193AC
		private string GetErrorText(object value)
		{
			IDataErrorInfo dataErrorInfo = value as IDataErrorInfo;
			string text = string.Empty;
			if (dataErrorInfo != null)
			{
				if (this.fieldInfo == null)
				{
					text = dataErrorInfo.Error;
				}
				else
				{
					text = dataErrorInfo[this.fieldInfo.Name];
				}
			}
			return text ?? string.Empty;
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0001B1F8 File Offset: 0x000193F8
		internal object GetValue()
		{
			object obj = this.bindingManager.Current;
			this.errorText = this.GetErrorText(obj);
			if (this.fieldInfo != null)
			{
				obj = this.fieldInfo.GetValue(obj);
			}
			return obj;
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x060008F8 RID: 2296 RVA: 0x0001B234 File Offset: 0x00019434
		internal Type BindToType
		{
			get
			{
				if (this.dataMember.BindingField.Length == 0)
				{
					Type type = this.bindingManager.BindType;
					if (typeof(Array).IsAssignableFrom(type))
					{
						type = type.GetElementType();
					}
					return type;
				}
				if (this.fieldInfo != null)
				{
					return this.fieldInfo.PropertyType;
				}
				return null;
			}
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x0001B290 File Offset: 0x00019490
		internal void SetValue(object value)
		{
			object obj = null;
			if (this.fieldInfo != null)
			{
				obj = this.bindingManager.Current;
				if (obj is IEditableObject)
				{
					((IEditableObject)obj).BeginEdit();
				}
				if (!this.fieldInfo.IsReadOnly)
				{
					this.fieldInfo.SetValue(obj, value);
				}
			}
			else
			{
				CurrencyManager currencyManager = this.bindingManager as CurrencyManager;
				if (currencyManager != null)
				{
					currencyManager[currencyManager.Position] = value;
					obj = value;
				}
			}
			this.errorText = this.GetErrorText(obj);
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x060008FA RID: 2298 RVA: 0x0001B30D File Offset: 0x0001950D
		internal BindingMemberInfo BindingMemberInfo
		{
			get
			{
				return this.dataMember;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x060008FB RID: 2299 RVA: 0x0001B315 File Offset: 0x00019515
		internal object DataSource
		{
			get
			{
				return this.dataSource;
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x060008FC RID: 2300 RVA: 0x0001B31D File Offset: 0x0001951D
		internal PropertyDescriptor FieldInfo
		{
			get
			{
				return this.fieldInfo;
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x060008FD RID: 2301 RVA: 0x0001B325 File Offset: 0x00019525
		internal BindingManagerBase BindingManagerBase
		{
			get
			{
				return this.bindingManager;
			}
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x0001B330 File Offset: 0x00019530
		internal void CheckBinding()
		{
			if (this.owner != null && this.owner.BindableComponent != null && this.owner.ControlAtDesignTime())
			{
				return;
			}
			if (this.owner.BindingManagerBase != null && this.fieldInfo != null && this.owner.BindingManagerBase.IsBinding && !(this.owner.BindingManagerBase is CurrencyManager))
			{
				this.fieldInfo.RemoveValueChanged(this.owner.BindingManagerBase.Current, new EventHandler(this.PropValueChanged));
			}
			if (this.owner != null && this.owner.BindingManagerBase != null && this.owner.BindableComponent != null && this.owner.ComponentCreated && this.IsDataSourceInitialized)
			{
				string bindingField = this.dataMember.BindingField;
				this.fieldInfo = this.owner.BindingManagerBase.GetItemProperties().Find(bindingField, true);
				if (this.owner.BindingManagerBase.DataSource != null && this.fieldInfo == null && bindingField.Length > 0)
				{
					throw new ArgumentException(SR.GetString("ListBindingBindField", new object[]
					{
						bindingField
					}), "dataMember");
				}
				if (this.fieldInfo != null && this.owner.BindingManagerBase.IsBinding && !(this.owner.BindingManagerBase is CurrencyManager))
				{
					this.fieldInfo.AddValueChanged(this.owner.BindingManagerBase.Current, new EventHandler(this.PropValueChanged));
					return;
				}
			}
			else
			{
				this.fieldInfo = null;
			}
		}

		// Token: 0x04000648 RID: 1608
		private PropertyDescriptor fieldInfo;

		// Token: 0x04000649 RID: 1609
		private BindingMemberInfo dataMember;

		// Token: 0x0400064A RID: 1610
		private object dataSource;

		// Token: 0x0400064B RID: 1611
		private BindingManagerBase bindingManager;

		// Token: 0x0400064C RID: 1612
		private Binding owner;

		// Token: 0x0400064D RID: 1613
		private string errorText = string.Empty;

		// Token: 0x0400064E RID: 1614
		private bool dataSourceInitialized;

		// Token: 0x0400064F RID: 1615
		private bool waitingOnDataSource;
	}
}
