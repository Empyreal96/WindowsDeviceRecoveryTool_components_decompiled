using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000330 RID: 816
	internal class RelatedPropertyManager : PropertyManager
	{
		// Token: 0x0600326D RID: 12909 RVA: 0x000EADC3 File Offset: 0x000E8FC3
		internal RelatedPropertyManager(BindingManagerBase parentManager, string dataField) : base(RelatedPropertyManager.GetCurrentOrNull(parentManager), dataField)
		{
			this.Bind(parentManager, dataField);
		}

		// Token: 0x0600326E RID: 12910 RVA: 0x000EADDC File Offset: 0x000E8FDC
		private void Bind(BindingManagerBase parentManager, string dataField)
		{
			this.parentManager = parentManager;
			this.dataField = dataField;
			this.fieldInfo = parentManager.GetItemProperties().Find(dataField, true);
			if (this.fieldInfo == null)
			{
				throw new ArgumentException(SR.GetString("RelatedListManagerChild", new object[]
				{
					dataField
				}));
			}
			parentManager.CurrentItemChanged += this.ParentManager_CurrentItemChanged;
			this.Refresh();
		}

		// Token: 0x0600326F RID: 12911 RVA: 0x000EAE44 File Offset: 0x000E9044
		internal override string GetListName()
		{
			string listName = this.GetListName(new ArrayList());
			if (listName.Length > 0)
			{
				return listName;
			}
			return base.GetListName();
		}

		// Token: 0x06003270 RID: 12912 RVA: 0x000EAE6E File Offset: 0x000E906E
		protected internal override string GetListName(ArrayList listAccessors)
		{
			listAccessors.Insert(0, this.fieldInfo);
			return this.parentManager.GetListName(listAccessors);
		}

		// Token: 0x06003271 RID: 12913 RVA: 0x000EAE8C File Offset: 0x000E908C
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

		// Token: 0x06003272 RID: 12914 RVA: 0x000EAED1 File Offset: 0x000E90D1
		private void ParentManager_CurrentItemChanged(object sender, EventArgs e)
		{
			this.Refresh();
		}

		// Token: 0x06003273 RID: 12915 RVA: 0x000EAED9 File Offset: 0x000E90D9
		private void Refresh()
		{
			this.EndCurrentEdit();
			this.SetDataSource(RelatedPropertyManager.GetCurrentOrNull(this.parentManager));
			this.OnCurrentChanged(EventArgs.Empty);
		}

		// Token: 0x17000C7A RID: 3194
		// (get) Token: 0x06003274 RID: 12916 RVA: 0x000EAEFD File Offset: 0x000E90FD
		internal override Type BindType
		{
			get
			{
				return this.fieldInfo.PropertyType;
			}
		}

		// Token: 0x17000C7B RID: 3195
		// (get) Token: 0x06003275 RID: 12917 RVA: 0x000EAF0A File Offset: 0x000E910A
		public override object Current
		{
			get
			{
				if (this.DataSource == null)
				{
					return null;
				}
				return this.fieldInfo.GetValue(this.DataSource);
			}
		}

		// Token: 0x06003276 RID: 12918 RVA: 0x000EAF28 File Offset: 0x000E9128
		private static object GetCurrentOrNull(BindingManagerBase parentManager)
		{
			if (parentManager.Position < 0 || parentManager.Position >= parentManager.Count)
			{
				return null;
			}
			return parentManager.Current;
		}

		// Token: 0x04001E4A RID: 7754
		private BindingManagerBase parentManager;

		// Token: 0x04001E4B RID: 7755
		private string dataField;

		// Token: 0x04001E4C RID: 7756
		private PropertyDescriptor fieldInfo;
	}
}
