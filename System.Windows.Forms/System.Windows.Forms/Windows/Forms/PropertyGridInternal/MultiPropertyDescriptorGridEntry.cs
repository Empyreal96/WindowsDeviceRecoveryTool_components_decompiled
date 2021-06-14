using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x0200048F RID: 1167
	internal class MultiPropertyDescriptorGridEntry : PropertyDescriptorGridEntry
	{
		// Token: 0x06004E7D RID: 20093 RVA: 0x00142109 File Offset: 0x00140309
		public MultiPropertyDescriptorGridEntry(PropertyGrid ownerGrid, GridEntry peParent, object[] objectArray, PropertyDescriptor[] propInfo, bool hide) : base(ownerGrid, peParent, hide)
		{
			this.mergedPd = new MergePropertyDescriptor(propInfo);
			this.objs = objectArray;
			base.Initialize(this.mergedPd);
		}

		// Token: 0x17001372 RID: 4978
		// (get) Token: 0x06004E7E RID: 20094 RVA: 0x00142138 File Offset: 0x00140338
		public override IContainer Container
		{
			get
			{
				IContainer container = null;
				object[] array = this.objs;
				int i = 0;
				while (i < array.Length)
				{
					object obj = array[i];
					IComponent component = obj as IComponent;
					if (component == null)
					{
						container = null;
						break;
					}
					if (component.Site != null)
					{
						if (container == null)
						{
							container = component.Site.Container;
						}
						else if (container != component.Site.Container)
						{
							goto IL_4B;
						}
						i++;
						continue;
					}
					IL_4B:
					container = null;
					break;
				}
				return container;
			}
		}

		// Token: 0x17001373 RID: 4979
		// (get) Token: 0x06004E7F RID: 20095 RVA: 0x001421A0 File Offset: 0x001403A0
		public override bool Expandable
		{
			get
			{
				bool flag = this.GetFlagSet(131072);
				if (flag && base.ChildCollection.Count > 0)
				{
					return true;
				}
				if (this.GetFlagSet(524288))
				{
					return false;
				}
				try
				{
					object[] values = this.mergedPd.GetValues(this.objs);
					for (int i = 0; i < values.Length; i++)
					{
						if (values[i] == null)
						{
							flag = false;
							break;
						}
					}
				}
				catch
				{
					flag = false;
				}
				return flag;
			}
		}

		// Token: 0x17001374 RID: 4980
		// (set) Token: 0x06004E80 RID: 20096 RVA: 0x00142220 File Offset: 0x00140420
		public override object PropertyValue
		{
			set
			{
				base.PropertyValue = value;
				base.RecreateChildren();
				if (this.Expanded)
				{
					this.GridEntryHost.Refresh(false);
				}
			}
		}

		// Token: 0x06004E81 RID: 20097 RVA: 0x0013E494 File Offset: 0x0013C694
		protected override bool CreateChildren()
		{
			return this.CreateChildren(false);
		}

		// Token: 0x06004E82 RID: 20098 RVA: 0x00142244 File Offset: 0x00140444
		protected override bool CreateChildren(bool diffOldChildren)
		{
			bool result;
			try
			{
				if (this.mergedPd.PropertyType.IsValueType || (this.Flags & 512) != 0)
				{
					result = base.CreateChildren(diffOldChildren);
				}
				else
				{
					base.ChildCollection.Clear();
					MultiPropertyDescriptorGridEntry[] mergedProperties = MultiSelectRootGridEntry.PropertyMerger.GetMergedProperties(this.mergedPd.GetValues(this.objs), this, this.PropertySort, this.CurrentTab);
					if (mergedProperties != null)
					{
						base.ChildCollection.AddRange(mergedProperties);
					}
					bool flag = this.Children.Count > 0;
					if (!flag)
					{
						this.SetFlag(524288, true);
					}
					result = flag;
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06004E83 RID: 20099 RVA: 0x001422F4 File Offset: 0x001404F4
		public override object GetChildValueOwner(GridEntry childEntry)
		{
			if (this.mergedPd.PropertyType.IsValueType || (this.Flags & 512) != 0)
			{
				return base.GetChildValueOwner(childEntry);
			}
			return this.mergedPd.GetValues(this.objs);
		}

		// Token: 0x06004E84 RID: 20100 RVA: 0x00142330 File Offset: 0x00140530
		public override IComponent[] GetComponents()
		{
			IComponent[] array = new IComponent[this.objs.Length];
			Array.Copy(this.objs, 0, array, 0, this.objs.Length);
			return array;
		}

		// Token: 0x06004E85 RID: 20101 RVA: 0x00142364 File Offset: 0x00140564
		public override string GetPropertyTextValue(object value)
		{
			bool flag = true;
			try
			{
				if (value == null && this.mergedPd.GetValue(this.objs, out flag) == null && !flag)
				{
					return "";
				}
			}
			catch
			{
				return "";
			}
			return base.GetPropertyTextValue(value);
		}

		// Token: 0x06004E86 RID: 20102 RVA: 0x001423BC File Offset: 0x001405BC
		internal override bool NotifyChildValue(GridEntry pe, int type)
		{
			bool result = false;
			IDesignerHost designerHost = this.DesignerHost;
			DesignerTransaction designerTransaction = null;
			if (designerHost != null)
			{
				designerTransaction = designerHost.CreateTransaction();
			}
			try
			{
				result = base.NotifyChildValue(pe, type);
			}
			finally
			{
				if (designerTransaction != null)
				{
					designerTransaction.Commit();
				}
			}
			return result;
		}

		// Token: 0x06004E87 RID: 20103 RVA: 0x00142408 File Offset: 0x00140608
		protected override void NotifyParentChange(GridEntry ge)
		{
			while (ge != null && ge is PropertyDescriptorGridEntry && ((PropertyDescriptorGridEntry)ge).propertyInfo.Attributes.Contains(NotifyParentPropertyAttribute.Yes))
			{
				object valueOwner = ge.GetValueOwner();
				while (!(ge is PropertyDescriptorGridEntry) || this.OwnersEqual(valueOwner, ge.GetValueOwner()))
				{
					ge = ge.ParentGridEntry;
					if (ge == null)
					{
						break;
					}
				}
				if (ge != null)
				{
					valueOwner = ge.GetValueOwner();
					IComponentChangeService componentChangeService = this.ComponentChangeService;
					if (componentChangeService != null)
					{
						Array array = valueOwner as Array;
						if (array != null)
						{
							for (int i = 0; i < array.Length; i++)
							{
								PropertyDescriptor propertyDescriptor = ((PropertyDescriptorGridEntry)ge).propertyInfo;
								if (propertyDescriptor is MergePropertyDescriptor)
								{
									propertyDescriptor = ((MergePropertyDescriptor)propertyDescriptor)[i];
								}
								if (propertyDescriptor != null)
								{
									componentChangeService.OnComponentChanging(array.GetValue(i), propertyDescriptor);
									componentChangeService.OnComponentChanged(array.GetValue(i), propertyDescriptor, null, null);
								}
							}
						}
						else
						{
							componentChangeService.OnComponentChanging(valueOwner, ((PropertyDescriptorGridEntry)ge).propertyInfo);
							componentChangeService.OnComponentChanged(valueOwner, ((PropertyDescriptorGridEntry)ge).propertyInfo, null, null);
						}
					}
				}
			}
		}

		// Token: 0x06004E88 RID: 20104 RVA: 0x00142518 File Offset: 0x00140718
		internal override bool NotifyValueGivenParent(object obj, int type)
		{
			if (obj is ICustomTypeDescriptor)
			{
				obj = ((ICustomTypeDescriptor)obj).GetPropertyOwner(this.propertyInfo);
			}
			switch (type)
			{
			case 1:
			{
				object[] array = (object[])obj;
				if (array != null && array.Length != 0)
				{
					IDesignerHost designerHost = this.DesignerHost;
					DesignerTransaction designerTransaction = null;
					if (designerHost != null)
					{
						designerTransaction = designerHost.CreateTransaction(SR.GetString("PropertyGridResetValue", new object[]
						{
							this.PropertyName
						}));
					}
					try
					{
						bool flag = !(array[0] is IComponent) || ((IComponent)array[0]).Site == null;
						if (flag && !this.OnComponentChanging())
						{
							if (designerTransaction != null)
							{
								designerTransaction.Cancel();
								designerTransaction = null;
							}
							return false;
						}
						this.mergedPd.ResetValue(obj);
						if (flag)
						{
							this.OnComponentChanged();
						}
						this.NotifyParentChange(this);
					}
					finally
					{
						if (designerTransaction != null)
						{
							designerTransaction.Commit();
						}
					}
					return false;
				}
				return false;
			}
			case 3:
			case 5:
			{
				MergePropertyDescriptor mergePropertyDescriptor = this.propertyInfo as MergePropertyDescriptor;
				if (mergePropertyDescriptor != null)
				{
					object[] array2 = (object[])obj;
					if (this.eventBindings == null)
					{
						this.eventBindings = (IEventBindingService)this.GetService(typeof(IEventBindingService));
					}
					if (this.eventBindings != null)
					{
						EventDescriptor @event = this.eventBindings.GetEvent(mergePropertyDescriptor[0]);
						if (@event != null)
						{
							return base.ViewEvent(obj, null, @event, true);
						}
					}
					return false;
				}
				return base.NotifyValueGivenParent(obj, type);
			}
			}
			return base.NotifyValueGivenParent(obj, type);
		}

		// Token: 0x06004E89 RID: 20105 RVA: 0x00142698 File Offset: 0x00140898
		private bool OwnersEqual(object owner1, object owner2)
		{
			if (!(owner1 is Array))
			{
				return owner1 == owner2;
			}
			Array array = owner1 as Array;
			Array array2 = owner2 as Array;
			if (array != null && array2 != null && array.Length == array2.Length)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (array.GetValue(i) != array2.GetValue(i))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06004E8A RID: 20106 RVA: 0x001426FC File Offset: 0x001408FC
		public override bool OnComponentChanging()
		{
			if (this.ComponentChangeService != null)
			{
				int num = this.objs.Length;
				for (int i = 0; i < num; i++)
				{
					try
					{
						this.ComponentChangeService.OnComponentChanging(this.objs[i], this.mergedPd[i]);
					}
					catch (CheckoutException ex)
					{
						if (ex == CheckoutException.Canceled)
						{
							return false;
						}
						throw ex;
					}
				}
			}
			return true;
		}

		// Token: 0x06004E8B RID: 20107 RVA: 0x0014276C File Offset: 0x0014096C
		public override void OnComponentChanged()
		{
			if (this.ComponentChangeService != null)
			{
				int num = this.objs.Length;
				for (int i = 0; i < num; i++)
				{
					this.ComponentChangeService.OnComponentChanged(this.objs[i], this.mergedPd[i], null, null);
				}
			}
		}

		// Token: 0x04003353 RID: 13139
		private MergePropertyDescriptor mergedPd;

		// Token: 0x04003354 RID: 13140
		private object[] objs;
	}
}
