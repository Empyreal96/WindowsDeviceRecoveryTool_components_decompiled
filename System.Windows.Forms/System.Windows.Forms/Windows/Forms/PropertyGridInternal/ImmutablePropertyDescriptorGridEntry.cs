using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x0200048C RID: 1164
	internal class ImmutablePropertyDescriptorGridEntry : PropertyDescriptorGridEntry
	{
		// Token: 0x06004E5E RID: 20062 RVA: 0x001419CE File Offset: 0x0013FBCE
		internal ImmutablePropertyDescriptorGridEntry(PropertyGrid ownerGrid, GridEntry peParent, PropertyDescriptor propInfo, bool hide) : base(ownerGrid, peParent, propInfo, hide)
		{
		}

		// Token: 0x17001366 RID: 4966
		// (get) Token: 0x06004E5F RID: 20063 RVA: 0x001419DB File Offset: 0x0013FBDB
		internal override bool IsPropertyReadOnly
		{
			get
			{
				return this.ShouldRenderReadOnly;
			}
		}

		// Token: 0x17001367 RID: 4967
		// (get) Token: 0x06004E60 RID: 20064 RVA: 0x001419E3 File Offset: 0x0013FBE3
		// (set) Token: 0x06004E61 RID: 20065 RVA: 0x001419EC File Offset: 0x0013FBEC
		public override object PropertyValue
		{
			get
			{
				return base.PropertyValue;
			}
			set
			{
				object valueOwner = this.GetValueOwner();
				GridEntry instanceParentGridEntry = this.InstanceParentGridEntry;
				TypeConverter typeConverter = instanceParentGridEntry.TypeConverter;
				PropertyDescriptorCollection properties = typeConverter.GetProperties(instanceParentGridEntry, valueOwner);
				IDictionary dictionary = new Hashtable(properties.Count);
				object obj = null;
				for (int i = 0; i < properties.Count; i++)
				{
					if (this.propertyInfo.Name != null && this.propertyInfo.Name.Equals(properties[i].Name))
					{
						dictionary[properties[i].Name] = value;
					}
					else
					{
						dictionary[properties[i].Name] = properties[i].GetValue(valueOwner);
					}
				}
				try
				{
					obj = typeConverter.CreateInstance(instanceParentGridEntry, dictionary);
				}
				catch (Exception ex)
				{
					if (string.IsNullOrEmpty(ex.Message))
					{
						throw new TargetInvocationException(SR.GetString("ExceptionCreatingObject", new object[]
						{
							this.InstanceParentGridEntry.PropertyType.FullName,
							ex.ToString()
						}), ex);
					}
					throw;
				}
				if (obj != null)
				{
					instanceParentGridEntry.PropertyValue = obj;
				}
			}
		}

		// Token: 0x06004E62 RID: 20066 RVA: 0x00141B14 File Offset: 0x0013FD14
		internal override bool NotifyValueGivenParent(object obj, int type)
		{
			return this.ParentGridEntry.NotifyValue(type);
		}

		// Token: 0x17001368 RID: 4968
		// (get) Token: 0x06004E63 RID: 20067 RVA: 0x00141B22 File Offset: 0x0013FD22
		public override bool ShouldRenderReadOnly
		{
			get
			{
				return this.InstanceParentGridEntry.ShouldRenderReadOnly;
			}
		}

		// Token: 0x17001369 RID: 4969
		// (get) Token: 0x06004E64 RID: 20068 RVA: 0x00141B30 File Offset: 0x0013FD30
		private GridEntry InstanceParentGridEntry
		{
			get
			{
				GridEntry parentGridEntry = this.ParentGridEntry;
				if (parentGridEntry is CategoryGridEntry)
				{
					parentGridEntry = parentGridEntry.ParentGridEntry;
				}
				return parentGridEntry;
			}
		}
	}
}
